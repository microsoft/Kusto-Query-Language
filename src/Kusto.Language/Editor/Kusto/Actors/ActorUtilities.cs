using System;

namespace Kusto.Language.Editor
{
    using Kusto.Language.Binding;
    using Kusto.Language.Parsing;
    using Kusto.Language.Symbols;
    using Kusto.Language.Syntax;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utils;

    internal static class ActorUtilities
    {
        /// <summary>
        /// Get's the token near the position
        /// </summary>
        public static SyntaxToken GetTokenNear(KustoCode code, int position)
        {
            var token = code.Syntax.GetTokenAt(position);

            if (token != null && position == token.TriviaStart && token.Trivia.Length > 0)
            {
                var prevToken = token.GetPreviousToken();
                if (prevToken != null)
                    return prevToken;
            }

            return token;
        }

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

        /// <summary>
        /// Adjusts the range to not include the leading and trailing trivia between tokens.
        /// </summary>
        public static void TrimRange(KustoCode code, ref int start, ref int length)
        {
            var expectedEnd = start + length;

            // adjust for start/end within whitespace between tokens

            var endToken = code.Syntax.GetTokenAt(expectedEnd);
            if (endToken != null && start < endToken.TextStart)
            {
                var adjustedEnd = endToken.TriviaStart;
                length = adjustedEnd - start;
                expectedEnd = start + length;
            }

            var startToken = code.Syntax.GetTokenAt(start);
            if (startToken != null && start < startToken.TextStart)
            {
                start = startToken.TextStart;
                length = expectedEnd - start;
            }
        }

        /// <summary>
        /// Gets the adjusted range around the sequence of query operators in a pipe expression.
        /// Returns false if the range does not correctly surround a distinct sub query
        /// </summary>
        public static bool TryGetAdjustedSubqueryRange(KustoCode code, ref int start, ref int length)
        {
            TrimRange(code, ref start, ref length);

            if (code.Syntax.GetNodeAt(start, length) is Expression ex)
            {
                if (ex is PipeExpression pe)
                {
                    var firstOp = GetPipeOperatorOrExpressionFromRangeStart(pe, start);
                    if (firstOp == null)
                        return false;

                    if (firstOp.TextStart > start)
                    {
                        var end = start + length;
                        start = firstOp.TextStart;
                        length = end - start;
                    }

                    var lastOp = GetPipeOperatorOrExpressionFromRangeEnd(pe, start + length);
                    if (lastOp == null)
                        return false;

                    if (lastOp.End != start + length)
                    {
                        length = lastOp.End - start;
                    }
                }

                return true;
            }

            return false;
        }

        public static Expression GetPipeOperatorOrExpressionFromRangeStart(PipeExpression pe, int start)
        {
            while (true)
            {
                // allow operator to include range that overlaps leading pipe
                if (start >= pe.Expression.End && start <= pe.Operator.TextStart)
                {
                    return pe.Operator;
                }
                else if (start <= pe.Expression.End && pe.Expression is PipeExpression ppe)
                {
                    pe = ppe;
                    continue;
                }
                else if (start >= pe.Expression.TriviaStart && start <= pe.Expression.TextStart)
                {
                    return pe.Expression;
                }
                else
                {
                    return null;
                }
            }
        }

        public static Expression GetPipeOperatorOrExpressionFromRangeEnd(PipeExpression pe, int end)
        {
            while (true)
            {
                // allow operator to include range that overlaps leading pipe
                if (end == pe.End)
                {
                    return pe.Operator;
                }
                else if (end < pe.Operator.TextStart && end >= pe.Expression.End)
                {
                    return pe.Expression;
                }
                else if (pe.Expression is PipeExpression ppe)
                {
                    pe = ppe;
                    continue;
                }
                else
                {
                    return null;
                }
            }
        }

        public static bool IsFirstInQuery(Expression expr)
        {
            if (expr.Parent is PipeExpression pe)
            {
                return pe.Expression == expr;
            }
            else
            {
                // not part of pipe
                return true;
            }
        }

        /// <summary>
        /// Gets the first named reference to the specified symbol within the syntax tree.
        /// </summary>
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

        /// <summary>
        /// Gets a list of all the referenced symbols declared outside the fragment.
        /// </summary>
        public static IReadOnlyList<Symbol> GetExternalSymbolsReferenced(KustoCode code, int start, int length)
        {
            // this is all the visible symbols declared before the range
            var symbolsInScope = code.GetSymbolsInScope(start).ToHashSetEx();

            // what about columns that are found in context before the fragment, but also found within the fragment (query operators)
            return code.Syntax.GetDescendantsOrSelf<Expression>(ex => IsInRange(ex, start, length) && ex.ReferencedSymbol != null && symbolsInScope.Contains(ex.ReferencedSymbol))
                    .Select(ex => ex.ReferencedSymbol)
                    .Distinct()
                    .ToReadOnly();
        }


        private static bool IsInRange(SyntaxElement element, int start, int length)
        {
            var end = start + length;
            return element.TriviaStart <= end && element.End >= start;
        }


        /// <summary>
        /// Gets all the columns in scope within a query operator.
        /// </summary>
        public static IReadOnlyList<ColumnSymbol> GetColumnsInScope(KustoCode code, int position)
        {
            var token = code.Syntax.GetTokenAt(position);
            var op = token.Parent.GetFirstAncestorOrSelf<QueryOperator>();
            
            if (op != null)
            {
                // this is faster, but might not be true in all contexts inside a query operator
                if (op.Parent is PipeExpression pe && pe.Operator == op && pe.Expression.ResultType is TableSymbol ts)
                {
                    return ts.Columns;
                }
                else if (code.GetColumnsInScope(op.TextStart) is TableSymbol ts2)
                {
                    return ts2.Columns;
                }
            }

            return EmptyReadOnlyList<ColumnSymbol>.Instance;
        }

        /// <summary>
        /// Gets the schema representation of a table as it would be represented in Kusto.
        /// </summary>
        public static string GetSchema(IReadOnlyList<ColumnSymbol> columns)
        {
            return "(" + string.Join(", ", columns.Select(c => $"{KustoFacts.BracketNameIfNecessary(c.Name)}: {GetKustoType(c.Type)}")) + ")";
        }

        /// <summary>
        /// Gets the text for a type/schema declaration as it would be represented in a kusto query.
        /// </summary>
        public static string GetKustoType(TypeSymbol type)
        {
            if (type is ScalarSymbol s)
            {
                return s.Name;
            }
            else if (type is TableSymbol t)
            {
                if (t.Columns.Count == 0)
                {
                    return "(*)";
                }
                else
                {
                    return GetSchema(t.Columns);
                }
            }
            else
            {
                return "unknown";
            }
        }

        /// <summary>
        /// Changes the indentation of the individual lines to the specified indenation.
        /// </summary>
        public static string ChangeIndentation(string text, string newIndentation, bool includeFirstLine)
        {
            var builder = new StringBuilder();

            var lineStart = 0;
            if (!includeFirstLine)
            {
                var lineLength = TextFacts.GetLineLength(text, lineStart, includeLineBreak: true);
                builder.Append(text, lineStart, lineLength);
                lineStart += lineLength;
            }

            while (lineStart < text.Length)
            {
                var lineLength = TextFacts.GetLineLength(text, lineStart, includeLineBreak: true);
                var wsLength = TokenParser.ScanWhitespace(text, lineStart);
                builder.Append(newIndentation);
                builder.Append(text, lineStart + wsLength, lineLength - wsLength);
                lineStart += lineLength;
            }

            return builder.ToString();
        }
    }
}