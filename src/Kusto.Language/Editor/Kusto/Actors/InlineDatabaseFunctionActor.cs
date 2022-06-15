using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Kusto.Language.Symbols;
    using Kusto.Language.Syntax;
    using Utils;
    using static ActorUtilities;

    internal class InlineDatabaseFunctionActor : KustoActor
    {
        private static readonly CodeAction InlineAction = new CodeAction(
            nameof(InlineDatabaseFunctionActor), "Inline Function", "Copy database function into this query", "");

        public override void GetActions(
            KustoCode code, 
            int position, 
            int length, 
            CodeActionOptions options,
            List<CodeAction> actions,
            CancellationToken cancellationToken)
        {
            var token = code.Syntax.GetTokenAt(position);
            if (position >= token.TextStart && position <= token.End && length == 0)
            {
                var node = code.Syntax.GetNodeAt(token.TextStart, token.Text.Length);
                if (node is Name name
                    && name.Parent is NameReference nr
                    && nr.ReferencedSymbol is FunctionSymbol fs
                    && code.Globals.IsDatabaseFunction(fs)
                    && IsGoodReference(nr, fs))
                {
                    actions.Add(InlineAction);
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

        public override CodeActionResult ApplyAction(
            KustoCode code, 
            int position, 
            int length,
            CodeActionOptions options,
            CodeAction action, 
            CancellationToken cancellationToken)
        {
            var token = code.Syntax.GetTokenAt(position);
            if (position >= token.TextStart && position <= token.End && length == 0)
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

                    if (TryGetNearestTopLevelStatementInsertionPosition(code, firstRef.TextStart, out var insertPosition))
                    {
                        var body = nr.Parent is FunctionCallExpression fc
                            ? fc.GetCalledFunctionBody()
                            : nr.GetCalledFunctionBody();

                        if (body != null)
                        {
                            var requalifiedBody = GetBodyWithBraces(GetRequalifiedDatabaseFunctionBody(body, code.Globals));
                            var declaration = GetLetStatement(fs, requalifiedBody);
                            var requalifiedQuery = GetQueryWithDatabaseQualifiersRemoved(code.Syntax, fs, code.Globals);
                            var newText = requalifiedQuery.Insert(insertPosition, declaration + "\n");
                            var newPosition = newText.GetCurrentPosition(position);
                            return new CodeActionResult(newText, newPosition);
                        }
                        else
                        {
                            return CodeActionResult.Failure("Could not access definition of referenced function");
                        }
                    }
                }
            }

            // nothing happened
            return CodeActionResult.Failure("Reference to database function not found");
        }

        private static string GetLetStatement(FunctionSymbol function, string body = null)
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
    }
}