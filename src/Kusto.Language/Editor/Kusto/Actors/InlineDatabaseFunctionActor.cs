using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Kusto.Language.Symbols;
    using Kusto.Language.Syntax;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using Utils;


    internal class InlineDatabaseFunctionActor : KustoActor
    {
        private static readonly CodeAction InlineDatabaseFunction = new CodeAction(
            nameof(InlineDatabaseFunctionActor), "Refactor", "Copy database function into this query", "");

        public override void GetActions(KustoCode code, int position, IReadOnlyList<Diagnostic> additionalDiagnostics, List<CodeAction> actions, CancellationToken cancellationToken)
        {
            var token = code.Syntax.GetTokenAt(position);
            if (position >= token.TextStart && position <= token.End)
            {
                var node = code.Syntax.GetNodeAt(token.TextStart, token.Text.Length);
                if (node is Name name
                    && name.Parent is NameReference nr
                    && nr.ReferencedSymbol is FunctionSymbol fs
                    && code.Globals.IsDatabaseFunction(fs)
                    && IsGoodReference(nr, fs))
                {
                    actions.Add(InlineDatabaseFunction);
                }
            }
        }

        private static bool IsGoodReference(NameReference nameRef, FunctionSymbol fs)
        {
            // disallow argument-list-less function references to functions that require arguments
            // as these won't have expansions later when applied.
            return nameRef.Parent is FunctionCallExpression
                || fs.MinArgumentCount == 0;
        }

        public override CodeActionResult ApplyAction(KustoCode code, int position, IReadOnlyList<Diagnostic> additionalDiagnostics, CodeAction action, CancellationToken cancellationToken)
        {
            var token = code.Syntax.GetTokenAt(position);
            if (position >= token.TextStart && position <= token.End)
            {
                var node = code.Syntax.GetNodeAt(token.TextStart, token.Text.Length);
                if (node is Name name
                    && name.Parent is NameReference nr
                    && nr.ReferencedSymbol is FunctionSymbol fs
                    && code.Globals.IsDatabaseFunction(fs)
                    && IsGoodReference(nr, fs))
                {
                    // we need to insert the function before the first reference to it.
                    var firstRef = GetFirstReference(code.Syntax, fs);

                    if (TryGetInsertionPosition(code, firstRef.TextStart, out var insertPosition))
                    {
                        var body = nr.Parent is FunctionCallExpression fc
                            ? fc.GetCalledFunctionBody()
                            : nr.GetCalledFunctionBody();

                        if (body != null)
                        {
                            var requalifiedBody = GetBodyWithBraces(GetRequalifiedDatabaseFunctionBody(body, code.Globals));
                            var declaration = GetDeclarationStatement(fs, requalifiedBody);
                            var requalifiedQuery = GetRequalifiedQuery(code.Syntax, fs, code.Globals);
                            var newText = requalifiedQuery.Insert(insertPosition, declaration + "\n");
                            var newPosition = newText.GetCurrentPosition(position);
                            return new CodeActionResult(newText, newPosition);
                        }
                        else
                        {
                            return new CodeActionResult(code.Text, position, "Could not access definition of referenced function");
                        }
                    }
                }
            }

            // nothing happened
            return new CodeActionResult(code.Text, position, "Reference to database function not found");
        }

        private static NameReference GetFirstReference(SyntaxNode root, FunctionSymbol symbol)
        {
            return root.GetFirstDescendant<NameReference>(nr => nr.ReferencedSymbol == symbol);
        }

        private static bool TryGetInsertionPosition(KustoCode code, int position, out int insertPosition)
        {
            // first find statement that the position is inside of
            var statement = GetTopLevelStatement(code, position);
            if (statement != null)
            {
                insertPosition = statement.TriviaStart;
                return true;
            }
            else
            {
                insertPosition = 0;
                return false;
            }
        }

        private static Statement GetTopLevelStatement(KustoCode code, int position)
        {
            var token = code.Syntax.GetTokenAt(position);
            SyntaxNode node = token.Parent;

            while (node != null && !IsTopLevelStatement(node))
            {
                node = node.GetFirstAncestor<Statement>();
            }

            return node as Statement;
        }

        private static bool IsTopLevelStatement(SyntaxNode node)
        {
            return node is Statement
                && node.Parent is SeparatedElement<Statement> element
                && element.Parent is SyntaxList<SeparatedElement<Statement>> list
                && list.Parent is QueryBlock;
        }

        private static string GetDeclarationStatement(FunctionSymbol function, string body = null)
        {
            if (body == null)
                body = GetBodyWithBraces(function.Signatures[0].Body);
            var parameters = Parameter.GetParameterListDeclaration(function.Signatures[0].Parameters);
            return $"let {KustoFacts.BracketNameIfNecessary(function.Name)} = {parameters} {body};";
        }

        private static EditString GetBodyWithBraces(string body)
        {
            var edit = new EditString(body);

            if (!edit.CurrentText.StartsWith("{", StringComparison.Ordinal))
                edit = edit.Prepend("{\n");

            if (!edit.CurrentText.EndsWith("}", StringComparison.Ordinal))
                edit = edit.Append("\n}");

            return edit;
        }

        private static string GetRequalifiedDatabaseFunctionBody(SyntaxNode body, GlobalState globals)
        {
            var text = body.ToString(IncludeTrivia.All);

            // need to qualify any unqualified reference to database functions/tables that are not from the current database
            var nodesToQualify = body.GetDescendants<SyntaxNode>(n =>
                !IsDatabaseQualifiedName(n)
                && GetReferencedDatabaseMember(n) is Symbol sym
                && IsDatabaseMemberNotFromCurrentDatabase(sym, globals));

            // edit from the end
            // todo: update this to make all edits in one call
            for (int i = nodesToQualify.Count - 1; i >= 0; i--)
            {
                var n = nodesToQualify[i];
                var db = globals.GetDatabase(GetReferencedDatabaseMember(n));
                if (db != null)
                {
                    var dbNameLiteral = KustoFacts.GetSingleQuotedStringLiteral(db.Name);
                    var cluster = globals.GetCluster(db);
                    if (cluster != globals.Cluster)
                    {
                        var clusterNameLiteral = KustoFacts.GetSingleQuotedStringLiteral(cluster.Name);
                        text = text.Insert(n.TextStart, $"cluster({clusterNameLiteral}).database({dbNameLiteral}).");

                    }
                    else
                    {
                        text = text.Insert(n.TextStart, $"database({dbNameLiteral}).");
                    }
                }
            }

            return text;
        }

        private static Symbol GetReferencedDatabaseMember(SyntaxNode node)
        {
            if (node is FunctionCallExpression fc)
            {
                if (fc.ReferencedSymbol == Functions.Table)
                    return fc.ResultType;

                return fc.ReferencedSymbol;
            }
            else if (node is NameReference nr && !(nr.Parent is FunctionCallExpression))
            {
                return nr.ReferencedSymbol;
            }
            else
            {
                return null;
            }
        }

        private static bool IsDatabaseQualifiedName(SyntaxNode node)
        {
            if (node.Parent is FunctionCallExpression pfc)
                node = pfc;

            if (node.Parent is PathExpression pe)
            {
                return (pe.Expression is FunctionCallExpression fc && fc.ReferencedSymbol == Functions.Database)
                  || (pe.Expression is PathExpression ppe && ppe.Selector is FunctionCallExpression ppefc && ppefc.ReferencedSymbol == Functions.Database);
            }

            return false;
        }

        private static bool IsDatabaseMemberNotFromCurrentDatabase(Symbol symbol, GlobalState globals)
        {
            return globals.GetDatabase(symbol) is DatabaseSymbol db && db != globals.Database;
        }

        private static EditString GetRequalifiedQuery(SyntaxNode query, FunctionSymbol symbol, GlobalState globals)
        {
            var text = new EditString(query.ToString());

            var nameRefsToDequalify = query.GetDescendants<NameReference>(nr =>
                IsDatabaseQualifiedName(nr) && nr.ReferencedSymbol == symbol);

            for (int i = nameRefsToDequalify.Count - 1; i >= 0; i--)
            {
                var nr = nameRefsToDequalify[i];
                var qualifier = GetQualifierToRemove(nr);
                if (qualifier != nr)
                {
                    var len = nr.TextStart - qualifier.TextStart;
                    text = text.Remove(qualifier.TextStart, len);
                }
            }

            return text;
        }

        private static SyntaxNode GetQualifierToRemove(SyntaxNode node)
        {
            if (node.Parent is FunctionCallExpression fc)
                node = node.Parent;

            if (IsDatabaseQualifiedName(node))
            {
                // works for database(...).name and cluster(...).database(...).name
                return node.Parent;
            }
            else
            {
                return node;
            }
        }
    }
}