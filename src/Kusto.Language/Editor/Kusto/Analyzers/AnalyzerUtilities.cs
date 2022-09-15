using System;

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
        public static bool IsShortStringConstant(Expression expr, GlobalState globals, int lengthNotShort = 4) =>
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
    }
}