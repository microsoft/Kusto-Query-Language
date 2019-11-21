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

        public static TupleSymbol MakePrefixedTuple(Signature signature, string parameterName, IReadOnlyList<Expression> args, TupleSymbol baseTuple)
        {
            var functionPrefix = ((FunctionSymbol)signature.Symbol).ResultNamePrefix ?? signature.Symbol.Name;
            var argumentPrefix = GetReferencedColumn(signature, parameterName, args)?.Name ?? "";
            return new TupleSymbol(baseTuple.Columns.Select(c => new ColumnSymbol(GetPrefixedName(functionPrefix, argumentPrefix, c.Name), c.Type)));
        }

        /// <summary>
        /// Converts a column name into a prefixed column name with potentially both a prefix for the function and a prefix for a column referenced by an argument.
        /// </summary>
        public static string GetPrefixedName(string functionPrefix, string argumentPrefix, string name)
        {
            if (functionPrefix != null && argumentPrefix != null)
            {
                return $"{functionPrefix}_{argumentPrefix}_{name}";
            }
            else if (functionPrefix != null)
            {
                return $"{functionPrefix}_{name}";
            }
            else if (argumentPrefix != null)
            {
                return $"{argumentPrefix}_{name}";
            }
            else
            {
                return name;
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
        /// Adds columns to the columns collection for each column referenced by arguments associated with the specified parameter.
        /// The column's name is the name of the column referenced by the argument expression.
        /// The column's type is either the argument's type or the explicit type if specified.
        /// </summary>
        public static void AddReferencedColumns(List<ColumnSymbol> columns, Signature signature, string parameterName, IReadOnlyList<Expression> args, TypeSymbol type = null)
        {
            var parameter = signature.GetParameter(parameterName);
            var argumentParameters = signature.GetArgumentParameters(args);
            GetArgumentRange(argumentParameters, parameter, out var start, out var length);

            for (int argIndex = start; argIndex >= 0 && argIndex < start + length; argIndex++)
            {
                var arg = args[argIndex];

                if (arg.ReferencedSymbol is ColumnSymbol c)
                {
                    if (type != null && c.Type != type)
                    {
                        c = new ColumnSymbol(c.Name, type);
                    }

                    columns.Add(c);
                }
            }
        }

        /// <summary>
        /// Adds the column to the columns collection referenced by the argument associated with the specified parameter.
        /// The column's name is the name of the column referenced by the argument expression or the explicit name if specified.
        /// The column's type is either the argument's type or the explicit type if specified.
        /// </summary>
        public static void AddReferencedColumn(List<ColumnSymbol> columns, Signature signature, string parameterName, IReadOnlyList<Expression> args, string name = null, TypeSymbol type = null)
        {
            var parameter = signature.GetParameter(parameterName);
            var argumentParameters = signature.GetArgumentParameters(args);
            var argIndex = argumentParameters.IndexOf(parameter);

            if (argIndex >= 0 && argIndex < args.Count)
            {
                var arg = args[argIndex];

                if (arg.ReferencedSymbol is ColumnSymbol c)
                {
                    if (type != null && c.Type != type)
                    {
                        c = new ColumnSymbol(c.Name, type);
                    }

                    if (name != null && c.Name != name)
                    {
                        c = new ColumnSymbol(name, c.Type);
                    }

                    columns.Add(c);
                }
            }
        }

        /// <summary>
        /// Gets the column referenced in an argument to the function corresponding to a specific parameter.
        /// </summary>
        public static ColumnSymbol GetReferencedColumn(Signature signature, string parameterName, IReadOnlyList<Expression> args)
        {
            var parameter = signature.GetParameter(parameterName);
            var argumentParameters = signature.GetArgumentParameters(args);
            var argIndex = argumentParameters.IndexOf(parameter);

            if (argIndex >= 0 && argIndex < args.Count)
            {
                var arg = args[argIndex];

                if (arg.ReferencedSymbol is ColumnSymbol c)
                {
                    return c;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the first argument associated with the parameter
        /// </summary>
        public static Expression GetArgument(IReadOnlyList<Expression> args, Signature signature, string parameterName)
        {
            var p = signature.GetParameter(parameterName);
            if (p != null)
            {
                var argumentParameters = signature.GetArgumentParameters(args);
                var argIndex = argumentParameters.IndexOf(p);
                if (argIndex >= 0 && argIndex < args.Count)
                {
                    return args[argIndex];
                }
            }

            return null;
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

                if (expr is SimpleNamedExpression sn)
                {
                    expr = sn.Expression;
                }

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