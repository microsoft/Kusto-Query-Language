using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language
{
    using Syntax;
    using Symbols;
    using Utils;

    /// <summary>
    /// A class full of helper API's used when building custom return types for functions.
    /// </summary>
    public static class FunctionHelpers
    {
        /// <summary>
        /// The largest number of ocurrances for a repeatable parameter
        /// </summary>
        public const int MaxRepeat = short.MaxValue;

        private static readonly ObjectPool<List<Parameter>> s_parameterListPool =
            new ObjectPool<List<Parameter>>(() => new List<Parameter>(), list => list.Clear());

        public static TupleSymbol MakePrefixedTuple(Signature signature, string parameterName, IReadOnlyList<Expression> args, TupleSymbol baseTuple)
        {
            var functionPrefix = ((FunctionSymbol)signature.Symbol).ResultNamePrefix ?? signature.Symbol.Name;
            var argumentPrefix = GetExpressionResultName(GetArgument(args, signature, parameterName));
            return new TupleSymbol(baseTuple.Columns.Select(c => new ColumnSymbol(MakeColumnName(functionPrefix, argumentPrefix, c.Name), c.Type)));
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
        /// Gets the range of arguments all associated with the same repeating parameter.
        /// </summary>
        public static void GetArgumentRange(List<Parameter> argumentParameters, Parameter parameter, out int start, out int length)
        {
            start = argumentParameters.IndexOf(parameter);
            if (start >= 0)
            {
                var end = start + 1;
                while (end < argumentParameters.Count && argumentParameters[end] == parameter)
                {
                    end++;
                }

                length = end - start;
            }
            else
            {
                length = 0;
            }
        }

        /// <summary>
        /// Adds the column referenced by the argument corresponding to the specified parameter to the list.
        /// </summary>
        public static void AddReferencedColumn(List<ColumnSymbol> columns, IReadOnlyList<Expression> args, Signature signature, string parameterName)
        {
            var arg = GetArgument(args, signature, parameterName);
            if (arg != null && arg.ReferencedSymbol is ColumnSymbol cs)
            {
                columns.Add(cs);
            }
        }

        /// <summary>
        /// Adds the columns referenced by the arguments corresponding to the specified parameter to the list.
        /// </summary>
        public static void AddReferencedColumns(List<ColumnSymbol> columns, IReadOnlyList<Expression> args, Signature signature, string parameterName)
        {
            columns.AddRange(
                GetArguments(args, signature, parameterName)
                .Where(a => a.ReferencedSymbol is ColumnSymbol)
                .Select(a => a.ReferencedSymbol as ColumnSymbol));
        }

        /// <summary>
        /// Gets the first argument associated with the parameter, or null if no parameter is associated with the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="signature">The signature of the function.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        public static Expression GetArgument(IReadOnlyList<Expression> args, Signature signature, string parameterName)
        {
            var p = signature.GetParameter(parameterName);
            if (p != null)
            {
                var argumentParameters = s_parameterListPool.AllocateFromPool();
                try
                {
                    signature.GetArgumentParameters(args, argumentParameters);
                    var argIndex = argumentParameters.IndexOf(p);
                    if (argIndex >= 0 && argIndex < args.Count)
                    {
                        return args[argIndex];
                    }
                }
                finally
                {
                    s_parameterListPool.ReturnToPool(argumentParameters);
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the arguments for the specified parameter.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="signature">The signature of the function.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        public static IEnumerable<Expression> GetArguments(IReadOnlyList<Expression> args, Signature signature, string parameterName)
        {
            var parameter = signature.GetParameter(parameterName);
            if (parameter != null)
            {
                var argumentParameters = s_parameterListPool.AllocateFromPool();
                try
                {
                    signature.GetArgumentParameters(args, argumentParameters);

                    for (int i = 0; i < argumentParameters.Count; i++)
                    {
                        if (argumentParameters[i] == parameter)
                            yield return args[i];
                    }
                }
                finally
                {
                    s_parameterListPool.ReturnToPool(argumentParameters);
                }
            }
        }

        /// <summary>
        /// Gets the column name that will be used in a projection list for the expression.
        /// </summary>
        public static string GetExpressionResultName(Expression expr, string defaultName = "", TableSymbol rowScope = null)
        {
            return Binding.Binder.GetExpressionResultName(expr, defaultName, rowScope);
        }

        /// <summary>
        /// Gets the value of the literal expression.
        /// </summary>
        public static string GetLiteralValue(Expression expr)
        {
            return expr.LiteralValue?.ToString() ?? "";
        }

        /// <summary>
        /// Converts all non letters and digits into underscores.
        /// </summary>
        public static string MakeValidNameFragment(string text)
        {
            if (!text.All(c => char.IsLetterOrDigit(c)))
            {
                return new string(text.Select(c => char.IsLetterOrDigit(c) ? c : '_').ToArray());
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
        public static IReadOnlyList<ColumnSymbol> GetColumnSymbols(SyntaxList<SeparatedElement<Expression>> exprs)
        {
            List<ColumnSymbol> symbols = null;

            for (var i = 0; i < exprs.Count; i++)
            {
                var expr = exprs[i].Element;

                expr = Kusto.Language.Binding.Binder.GetUnderlyingExpression(expr);

                if (expr.ReferencedSymbol is ColumnSymbol c)
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
    }
}