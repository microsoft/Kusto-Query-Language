using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Symbols;
    using Syntax;

    internal static class AnalyzerUtilities
    {
        /// <summary>
        /// Returns true if the expr refers to a database column.
        /// </summary>
        public static bool IsDbColumn(Expression expr, GlobalState globals) =>
            expr.ReferencedSymbol is ColumnSymbol c && globals.GetTable(c) != null;


        /// <summary>
        /// Returns true if the operator expression has at least one operand that 
        /// refers to a database column.
        /// </summary>
        public static bool HasDbColumnOperand(Expression expr, GlobalState globals)
        {
            return HasMatchingOperand(expr, operand => IsDbColumn(operand, globals));
        }

        /// <summary>
        /// Returns true if the expression is a short string constant
        /// </summary>
        public static bool IsShortStringConstant(Expression expr, GlobalState globals, int lengthNotShort = 3) =>
            expr.ConstantValue is string s && s.Length < lengthNotShort;

        /// <summary>
        /// Returns true if the operator expression has an operand that matches.
        /// </summary>
        public static bool HasMatchingOperand(Expression expr, Func<Expression, bool> fnMatches, bool includeListOperandValues = false)
        {
            return GetFirstMatchingOperand(expr, fnMatches, includeListOperandValues) != null;
        }

        /// <summary>
        /// Returns true if the list has an element that matches.
        /// </summary>
        public static bool HasAnyMatchingElement(SyntaxList<SeparatedElement<Expression>> list, Func<Expression, bool> fnMatches)
        {
            return GetFirstMatchingElement(list, fnMatches) != null;
        }

        /// <summary>
        /// Returns the first matching operand (lexical order) or null if there are no matches.
        /// </summary>
        public static Expression GetFirstMatchingOperand(Expression expr, Func<Expression, bool> fnMatches, bool includeListOperandValues = false)
        {
            switch (expr)
            {
                case BinaryExpression be:
                    if (fnMatches(be.Left))
                        return be.Left;
                    if (fnMatches(be.Right))
                        return be.Right;
                    break;
                case PrefixUnaryExpression pu:
                    if (fnMatches(pu.Expression))
                        return pu.Expression;
                    break;
                case HasAllExpression hall:
                    if (fnMatches(hall.Left))
                        return hall.Left;
                    if (includeListOperandValues)
                    {
                        return GetFirstMatchingElement(hall.Right.Expressions, fnMatches);
                    }
                    break;
                case HasAnyExpression hany:
                    if (fnMatches(hany.Left))
                        return hany.Left;
                    if (includeListOperandValues)
                    {
                        return GetFirstMatchingElement(hany.Right.Expressions, fnMatches);
                    }
                    break;
            }
            return null;
        }

        /// <summary>
        /// Returns the first matching element or null if there are no matches.
        /// </summary>
        private static Expression GetFirstMatchingElement(SyntaxList<SeparatedElement<Expression>> list, Func<Expression, bool> fnMatches)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (fnMatches(list[i].Element))
                    return list[i].Element;
            }

            return null;
        }

        /// <summary>
        /// Adds edits to change name to new name.
        /// </summary>
        public static List<TextEdit> AddRenameEdits(this List<TextEdit> edits, Name name, string newName)
        {
            edits.Add(TextEdit.Replacement(name.TextStart, name.Width, newName));
            return edits;
        }

        /// <summary>
        /// Adds edits to remove an argument
        /// </summary>
        public static List<TextEdit> AddRemoveArgumentEdits(this List<TextEdit> edits, Expression argument)
        {
            var nextToken = argument.GetLastToken().GetNextToken();
            var prevToken = argument.GetFirstToken().GetPreviousToken();

            int start;
            int end;

            if (prevToken != null && nextToken != null)
            {
                start = prevToken.Kind == SyntaxKind.CommaToken ? prevToken.TextStart
                    : prevToken.Kind == SyntaxKind.OpenParenToken ? prevToken.End
                    : argument.TriviaStart;

                end = nextToken.Kind == SyntaxKind.CommaToken ? nextToken.TextStart
                    : nextToken.Kind == SyntaxKind.CloseParenToken ? nextToken.TextStart
                    : argument.End;
            }
            else if (prevToken != null)
            {
                start = prevToken.Kind == SyntaxKind.CommaToken ? prevToken.TextStart
                    : prevToken.Kind == SyntaxKind.OpenParenToken ? prevToken.End
                    : argument.TriviaStart;
                end = argument.End;
            }
            else if (nextToken != null)
            {
                start = argument.TriviaStart;

                end = nextToken.Kind == SyntaxKind.CommaToken ? nextToken.TextStart
                    : nextToken.Kind == SyntaxKind.CloseParenToken ? nextToken.TextStart
                    : argument.End;
            }
            else
            {
                start = argument.TriviaStart;
                end = argument.End;
            }

            edits.Add(TextEdit.Deletion(start, end - start));

            return edits;
        }

        /// <summary>
        /// Adds edits to remove an argument
        /// </summary>
        public static List<TextEdit> AddRemoveArgumentEdits(this List<TextEdit> edits, FunctionCallExpression functionCall, int argumentIndex)
        {
            if (argumentIndex < functionCall.ArgumentList.Expressions.Count)
            {
                AddRemoveArgumentEdits(edits, functionCall.ArgumentList.Expressions[argumentIndex].Element);
            }

            return edits;
        }


        /// <summary>
        /// Adds edits to insert an argument at a specified position
        /// </summary>
        public static List<TextEdit> AddInsertArgumentEdits(this List<TextEdit> edits, FunctionCallExpression functionCall, int argumentIndex, string newArgumentText)
        {
            if (functionCall.ArgumentList.Expressions.Count == 0)
            {
                // no current arguments
                edits.Add(TextEdit.Insertion(functionCall.ArgumentList.CloseParen.TriviaStart, newArgumentText));
            }
            if (argumentIndex >= functionCall.ArgumentList.Expressions.Count)
            {
                // after end (with existing arguments
                edits.Add(TextEdit.Insertion(functionCall.ArgumentList.CloseParen.TriviaStart, $", {newArgumentText}"));
            }
            else if (functionCall.ArgumentList.Expressions.Count > argumentIndex)
            {
                // before existing argument
                var seperatedArg = functionCall.ArgumentList.Expressions[argumentIndex];
                edits.Add(TextEdit.Insertion(seperatedArg.TextStart, $"{newArgumentText}, "));
            }

            return edits;
        }

        /// <summary>
        /// Adds edits to insert an argument at a specified position
        /// </summary>
        public static List<TextEdit> AddAddArgumentEdits(this List<TextEdit> edits, FunctionCallExpression functionCall, string newArgumentText)
        {
            return AddInsertArgumentEdits(edits, functionCall, functionCall.ArgumentList.Expressions.Count, newArgumentText);
        }

        /// <summary>
        /// Returns the function call that specifies this argument.
        /// </summary>
        public static FunctionCallExpression GetFunctionCall(Expression argument)
        {
            if (argument.Parent is SeparatedElement sep
                && sep.Parent is SyntaxList list
                && list.Parent is ExpressionList exprList
                && exprList.Parent is FunctionCallExpression fc)
            {
                return fc;
            }

            return null;
        }


        /// <summary>
        /// Adds edits to remove outer function call from argument.
        /// </summary>
        public static List<TextEdit> AddRemoveOuterFunctionCallEdits(this List<TextEdit> edits, FunctionCallExpression outer, Expression argument)
        {
            edits.Add(TextEdit.Deletion(outer.TextStart, argument.TriviaStart - outer.TextStart));
            edits.Add(TextEdit.Deletion(argument.End, outer.End - argument.End));
            return edits;
        }
    }
}