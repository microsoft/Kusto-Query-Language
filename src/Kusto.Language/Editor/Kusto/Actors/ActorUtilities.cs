using System;

namespace Kusto.Language.Editor
{
    using Kusto.Language.Parsing;
    using Kusto.Language.Symbols;
    using Kusto.Language.Syntax;
    using Utils;

    internal static class ActorUtilities
    {
        /// <summary>
        /// Gets the biggest node that fits within the range.
        /// </summary>
        public static SyntaxNode GetNodeInRange(KustoCode code, int start, int length)
        {
            var node = code.Syntax.GetNodeAt(start, length);

            // check if we got the node that ends in the correct position.
            // if the node goes beyond the range then the range might have been off by just a token or within the next token's whitespace
            if (node.End > start + length)
            {
                // adjust end to see if we are just one token off from fitting
                var lastToken = code.Syntax.GetTokenAt(start + length - 1);
                if (lastToken == null)
                    return null;

                var prevToken = lastToken.GetPreviousToken();
                if (prevToken == null)
                    return null;

                var newEnd = prevToken.End;

                length = newEnd - start;

                node = code.Syntax.GetNodeAt(start, length);

                // if range is still too big then fail
                if (node.End > start + length)
                    return null;
            }

            return node;
        }

        public static NameReference GetFirstReference(SyntaxNode root, FunctionSymbol symbol)
        {
            return root.GetFirstDescendant<NameReference>(nr => nr.ReferencedSymbol == symbol);
        }

        public static bool TryGetNearestTopLevelStatementInsertionPosition(KustoCode code, int position, out int insertPosition)
        {
            // first find statement that the position is inside of
            var statement = GetEnclosingTopLevelStatement(code, position);
            if (statement != null)
            {
                insertPosition = GetInsertionPositionBefore(statement);
                return true;
            }
            else
            {
                insertPosition = 0;
                return false;
            }
        }

        public static bool TryGetNearestStatementInsertionPosition(KustoCode code, int position, out int insertPosition)
        {
            // first find statement that the position is inside of
            var node = GetEnclosingStatementOrFunctionBodyExpression(code, position);
            if (node != null)
            {
                insertPosition = GetInsertionPositionBefore(node);
                return true;
            }
            else
            {
                insertPosition = 0;
                return false;
            }
        }

        /// <summary>
        /// Gets the stastement insertion position before the start of the specified node.
        /// </summary>
        private static int GetInsertionPositionBefore(SyntaxNode node)
        {
            var trivia = node.GetFirstToken()?.Trivia ?? "";

            if (node.TriviaStart == 0)
            {
                // statement is the first statement of the query block
                // assume any leading trivia with line breaks is comments and that comments are for entire query
                var lastLbEnd = TextFacts.GetLastLineBreakEnd(trivia);
                return (lastLbEnd >= 0 ? lastLbEnd : 0) + node.TriviaStart;
            }
            else
            {
                // statement is not first, so place new statement after on the next new line
                var firstLbEnd = TextFacts.GetFirstLineBreakEnd(trivia);
                return (firstLbEnd >= 0 ? firstLbEnd : 0) + node.TriviaStart;
            }
        }

        public static Statement GetEnclosingTopLevelStatement(KustoCode code, int position)
        {
            var token = code.Syntax.GetTokenAt(position);
            var node = token.Parent;

            while (node != null && !IsTopLevelStatement(node))
            {
                node = node.GetFirstAncestor<Statement>();
            }

            return node as Statement;
        }

        public static SyntaxNode GetEnclosingStatementOrFunctionBodyExpression(KustoCode code, int position)
        {
            var token = code.Syntax.GetTokenAt(position);
            var node = token.Parent;

            while (node != null
                && !IsTopLevelStatement(node)
                && !IsFunctionBodyStatement(node)
                && !IsFunctionBodyExpression(node))
            {
                node = node.GetFirstAncestor<SyntaxNode>();
            }

            return node;
        }

        public static bool IsTopLevelStatement(SyntaxNode node)
        {
            return node is Statement
                && node.Parent is SeparatedElement<Statement> element
                && element.Parent is SyntaxList<SeparatedElement<Statement>> list
                && list.Parent is QueryBlock;
        }

        public static bool IsFunctionBodyStatement(SyntaxNode node)
        {
            return node is Statement
                && node.Parent is SeparatedElement<Statement> element
                && element.Parent is SyntaxList<SeparatedElement<Statement>> list
                && list.Parent is FunctionBody;
        }

        public static bool IsFunctionBodyExpression(SyntaxNode node)
        {
            return node.Parent is FunctionBody fb && fb.Expression == node;
        }

        /// <summary>
        /// Determines if the node is the right hand side of a path expression after a database(xxx) or cluster().database() expression.
        /// </summary>
        public static bool IsDatabaseQualifiedName(SyntaxNode node)
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

        /// <summary>
        /// Returns true if the symbol is a member of a known database but not a member of the current database.
        /// </summary>
        public static bool IsDatabaseMemberNotFromCurrentDatabase(Symbol symbol, GlobalState globals)
        {
            return globals.GetDatabase(symbol) is DatabaseSymbol db && db != globals.Database;
        }

        /// <summary>
        /// Gets the query text with the database() and cluster().database() qualifiers removed for a specified symbol.
        /// </summary>
        public static EditString GetQueryWithDatabaseQualifiersRemoved(SyntaxNode query, Symbol qualifiedSymbol, GlobalState globals)
        {
            var text = new EditString(query.ToString());

            var nameRefsToDequalify = query.GetDescendants<NameReference>(nr =>
                IsDatabaseQualifiedName(nr) && nr.ReferencedSymbol == qualifiedSymbol);

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

        /// <summary>
        /// Gets the new line characters that are currently being used by the text,
        /// or Environment.NewLine if none are found.
        /// </summary>
        public static string GetInferredNewLine(string text)
        {
            var newLinePos = text.LastIndexOf('\n');

            if (newLinePos >= 0)
            {
                if (newLinePos > 0 && text[newLinePos - 1] == '\r')
                {
                    return "\r\n";
                }
                else
                {
                    return "\n";
                }
            }
            else
            {
                return Environment.NewLine;
            }
        }


        public static bool IsExpressionOfLetStatement(SyntaxNode node) =>
            node.Parent is LetStatement ls && ls.Expression == node;

        public static bool IsSelectorOfPathExpression(SyntaxNode node) =>
            node.Parent is PathExpression pe && pe.Selector == node;

        public static bool IsOperatorOfPipeExpression(SyntaxNode node) =>
            node.Parent is PipeExpression pe && pe.Operator == node;

        public static bool IsFreeStandingExpression(Expression ex) =>
            !IsSelectorOfPathExpression(ex)
            && !IsOperatorOfPipeExpression(ex);

        /// <summary>
        /// Gets a name that does not already refer to something in the code at the specified position.
        /// </summary>
        public static string GetNewName(KustoCode code, int position, string baseName)
        {
            var name = baseName;
            var count = 0;

            // check if the name already refers to something..
            // if it does, try a different name
            if (code.GetSpeculativeReferencedSymbol(position, name) != null)
            {
                count++;
                name = baseName + count;
            }

            return name;
        }
    }
}