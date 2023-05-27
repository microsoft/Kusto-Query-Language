using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language
{
    using Syntax;
    using Symbols;
    using Utils;
    using Kusto.Language.Parsing;

    /// <summary>
    /// A class full of helper API's used when building custom return types for functions.
    /// </summary>
    public static class FunctionHelpers
    {
        /// <summary>
        /// The largest number of ocurrances for a repeatable parameter
        /// </summary>
        public const int MaxRepeat = short.MaxValue;

        public static TupleSymbol MakePrefixedTuple(CustomReturnTypeContext context, string parameterName, TupleSymbol baseTuple)
        {
            var functionPrefix = ((FunctionSymbol)context.Signature.Symbol).ResultNamePrefix ?? context.Signature.Symbol.Name;
            var argumentPrefix = context.GetResultName(context.GetArgument(parameterName));
            return new TupleSymbol(baseTuple.Columns.Select(c => c.WithName(MakeColumnName(functionPrefix, argumentPrefix, c.Name)).WithOriginalColumns(c)));
        }

        /// <summary>
        /// Makes a column name by joining multiple parts using underscores.
        /// </summary>
        public static string MakeColumnName(params string[] nameParts)
        {
            if (nameParts.Length == 1)
            {
                return nameParts[0];
            }
            else
            {
                return string.Join("_", nameParts.Where(p => p != null));
            }
        }

        /// <summary>
        /// Adds the column referenced by the argument corresponding to the specified parameter to the list.
        /// </summary>
        public static void AddReferencedColumn(List<ColumnSymbol> columns, CustomReturnTypeContext context, string parameterName)
        {
            var arg = context.GetArgument(parameterName);
            if (arg != null && arg.ReferencedSymbol is ColumnSymbol cs)
            {
                columns.Add(cs);
            }
        }

        /// <summary>
        /// Adds the columns referenced by the arguments corresponding to the specified parameter to the list.
        /// </summary>
        public static void AddReferencedColumns(List<ColumnSymbol> columns, CustomReturnTypeContext context, string parameterName)
        {
            columns.AddRange(context.GetArguments(parameterName).Select(a => a.ReferencedSymbol).OfType<ColumnSymbol>());
        }

        /// <summary>
        /// Gets the value of the constant expression.
        /// </summary>
        public static string GetConstantValue(Expression expr)
        {
            return expr.ConstantValue?.ToString() ?? "";
        }

        /// <summary>
        /// Gets the values for all the constant expressions.
        /// </summary>
        public static IReadOnlyList<string> GetConstantValues(SyntaxList<SeparatedElement<Expression>> expressions)
        {
            var list = new List<string>();

            for (int i = 0; i < expressions.Count; i++)
            {
                var expr = expressions[i].Element;
                list.Add(GetConstantValue(expr));
            }

            return list;
        }

        /// <summary>
        /// Converts all non letters and digits into underscores.
        /// </summary>
        public static string MakeValidNameFragment(string text)
        {
            if (!text.All(c => TextFacts.IsLetterOrDigit(c)))
            {
                return new string(text.Select(c => TextFacts.IsLetterOrDigit(c) ? c : '_').ToArray());
            }
            else
            {
                return text;
            }
        }

        /// <summary>
        /// Gets the <see cref="ColumnSymbol"/>s referenced in the by clause of the summarize operator
        /// given the args to an invocation of an aggregate.
        /// </summary>
        public static IReadOnlyList<ColumnSymbol> GetSummarizeByColumns(IReadOnlyList<Expression> args)
        {
            if (args.Count > 0)
            {
                var op = args[0].GetFirstAncestorOrSelf<SummarizeOperator>();
                if (op != null && op.ByClause != null)
                {
                    return GetColumnSymbols(op.ByClause.Expressions);
                }
            }

            return EmptyReadOnlyList<ColumnSymbol>.Instance;
        }

        /// <summary>
        /// Get the list of <see cref="ColumnSymbol"/> referenced by the expressions in a list of expressions.
        /// </summary>
        private static IReadOnlyList<ColumnSymbol> GetColumnSymbols(SyntaxList<SeparatedElement<Expression>> exprs)
        {
            List<ColumnSymbol> symbols = null;

            for (var i = 0; i < exprs.Count; i++)
            {
                var expr = exprs[i].Element;

                expr = Binding.Binder.GetUnderlyingExpression(expr);

                if (Binding.Binder.GetResultColumn(expr) is ColumnSymbol c)
                {
                    if (symbols == null)
                    {
                        symbols = new List<ColumnSymbol>();
                    }

                    symbols.Add(c);
                }
            }

            return symbols ?? EmptyReadOnlyList<ColumnSymbol>.Instance;
        }

        public static bool IsBoolean(Expression expr)
        {
            return expr.ResultType == ScalarTypes.Bool
                || (expr is LiteralExpression lit && lit.Kind == SyntaxKind.BooleanLiteralExpression);
        }
    }
}