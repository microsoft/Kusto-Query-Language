using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Symbols;
    using Utils;

    /// <summary>
    /// The base class for any <see cref="KustoCode"/> analyzer.
    /// </summary>
    public static class KustoAnalyzers
    {
        public static readonly KustoAnalyzer AvoidUsingContains = KustoAnalyzer.Create(
            nameof(AvoidUsingContains),
            new Diagnostic(
                "KS500",
                DiagnosticCategory.Performance,
                DiagnosticSeverity.Suggestion,
                description: "Avoid using contains operator",
                message: $"Avoid using the 'contains' operator as it has a high compute price." + Environment.NewLine +
                         $"Use the 'has' operator in cases when full term match is desired (see: https://aka.ms/kusto.stringterms)."),
            (code, dx, diagnostics) =>
            {
                foreach (var node in code.Syntax.GetDescendants<BinaryExpression>())
                {
                    if (node.Kind == SyntaxKind.ContainsExpression ||
                        node.Kind == SyntaxKind.NotContainsExpression ||
                        node.Kind == SyntaxKind.ContainsCsExpression ||
                        node.Kind == SyntaxKind.NotContainsCsExpression)
                    {
                        // only report if db column is being used directly
                        if (IsDbColumn(node.Left, code.Globals))
                        {
                            diagnostics.Add(dx.WithLocation(node.Operator));
                        }
                    }
                }
            });

        private static bool IsDbColumn(Expression expr, GlobalState globals) =>
            expr.ReferencedSymbol is ColumnSymbol c && globals.GetTable(c) != null;

        // KS501 & KS502
        public static readonly KustoAnalyzer AvoidUsingIsNullStringComparison =
            new AvoidUsingIsNullStringComparisonAnalyzer();

        public static readonly KustoAnalyzer AvoidUsingShortStringComparision = KustoAnalyzer.Create(
            nameof(AvoidUsingShortStringComparision),
            new Diagnostic(
                "KS503",
                category: DiagnosticCategory.Performance,
                severity: DiagnosticSeverity.Suggestion,
                description: "Avoid short string comparisons",
                message: "Avoid using short strings (less than 4 characters) for string comparison operations (see: https://aka.ms/kusto.stringterms)."),
            (code, dx, diagnostics) =>
            {
                foreach (var node in code.Syntax.GetDescendants<BinaryExpression>())
                {
                    if (!node.Right.IsConstant && !node.Left.IsConstant)
                        continue;

                    if (node.Kind == SyntaxKind.EqualExpression ||
                        node.Kind == SyntaxKind.HasExpression ||
                        node.Kind == SyntaxKind.NotEqualExpression ||
                        node.Kind == SyntaxKind.NotHasExpression ||
                        node.Kind == SyntaxKind.StartsWithExpression ||
                        node.Kind == SyntaxKind.NotStartsWithExpression)
                    {
                        string constValue = null;

                        if (node.Right.IsConstant && node.Right.ResultType == ScalarTypes.String)
                        {
                            constValue = node.Right.ConstantValue as string;
                        }
                        else if (node.Left.IsConstant && node.Left.ResultType == ScalarTypes.String)
                        {
                            constValue = node.Left.ConstantValue as string;
                        }

                        if (!string.IsNullOrEmpty(constValue) && constValue.Length < 4)
                        {
                            // only report if db column is being used directly
                            if (!IsDbColumn(node.Right, code.Globals) && !IsDbColumn(node.Left, code.Globals))
                                continue;

                            diagnostics.Add(dx.WithLocation(node));
                        }
                    }
                }
            });

        public static readonly KustoAnalyzer AvoidUsingToBoolOnNumerics = KustoAnalyzer.Create(
            nameof(AvoidUsingToBoolOnNumerics),
            new Diagnostic(
                "KS504",
                category: DiagnosticCategory.Correctness,
                severity: DiagnosticSeverity.Warning,
                description: "Avoid using tobool on numeric arguments",
                message: "Avoid using tobool on numeric arguments, use comparison operators instead"),
            (code, dx, diagnostics) =>
            {
                foreach (var node in code.Syntax.GetDescendants<FunctionCallExpression>())
                {
                    if ((node.ReferencedSymbol == Functions.ToBool || node.ReferencedSymbol == Functions.ToBoolean)
                        && node.ArgumentList.Expressions.Count > 0)
                    {
                        var firstArgumentType = node.ArgumentList.Expressions[0].Element.ResultType;
                        if (firstArgumentType == ScalarTypes.DateTime ||
                            firstArgumentType == ScalarTypes.Int ||
                            firstArgumentType == ScalarTypes.Decimal ||
                            firstArgumentType == ScalarTypes.Guid ||
                            firstArgumentType == ScalarTypes.Long ||
                            firstArgumentType == ScalarTypes.Real ||
                            firstArgumentType == ScalarTypes.TimeSpan)
                        {
                            diagnostics.Add(dx.WithLocation(node));
                        }
                    }
                }
            });

        public static readonly KustoAnalyzer NullAggregation = KustoAnalyzer.Create(
            nameof(NullAggregation),
            new Diagnostic(
                "KS505",
                category: DiagnosticCategory.Correctness,
                severity: DiagnosticSeverity.Warning,
                description: "Avoid using operations on possible null values inside sum aggregates"),
            (code, dx, diagnostics) =>
            {
                foreach (var node in code.Syntax.GetDescendants<SummarizeOperator>())
                {
                    var badSums = node.Aggregates.GetDescendants<FunctionCallExpression>(fc =>
                        fc.ReferencedSymbol == Aggregates.Sum
                        && fc.ArgumentList.Expressions[0].Element is BinaryExpression b
                        && (b.Kind == SyntaxKind.AddExpression
                         || b.Kind == SyntaxKind.SubtractExpression));

                    foreach (var bs in badSums)
                    {
                        var binaryExpression = bs.ArgumentList.Expressions[0].Element as BinaryExpression;
                        var left = binaryExpression.Left.ToString();
                        var right = binaryExpression.Right.ToString();
                        var op = binaryExpression.Operator.Text;

                        diagnostics.Add(dx.WithMessage(
                            $"If any of the columns referenced in '{bs}' contains nulls, significant values might not be included due to scalar arithmetic null propagation rules."
                            + Environment.NewLine +
                            $"Consider rewriting it as '{bs.Name}({left}) {op} {bs.Name}({right})'.")
                            .WithLocation(bs));
                    }
                }
            });

        public static readonly KustoAnalyzer AvoidUsingFormatDatetimeInPredicate = KustoAnalyzer.Create(
            nameof(AvoidUsingFormatDatetimeInPredicate),
            new Diagnostic(
                "KS506",
                DiagnosticCategory.Performance,
                DiagnosticSeverity.Suggestion,
                description: "Avoid using format_datetime() in a filter or predicate",
                message: $"Avoid using the 'format_datetime' function in a filter or predicate." + Environment.NewLine +
                         $"Instead, use specific datetime functions like startofday or bin."),
            (code, dx, diagnostics) =>
            {
                foreach (var node in code.Syntax.GetDescendants<FunctionCallExpression>(fc =>
                    fc.ReferencedSymbol == Functions.FormatDatetime))
                {
                    if (IsInFilter(node) || IsInPredicate(node))
                    {
                        diagnostics.Add(dx.WithLocation(node));
                    }
                }
            });

        private static bool IsInFilter(SyntaxNode node)
        {
            return node.GetFirstAncestor<FilterOperator>() != null;
        }

        private static bool IsInPredicate(SyntaxNode node)
        {
            return node.GetFirstAncestor<BinaryExpression>(be => IsComparisonOperator(be.Kind)) != null;
        }

        private static bool IsComparisonOperator(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.EqualExpression:
                case SyntaxKind.NotEqualExpression:
                case SyntaxKind.EqualTildeExpression:
                case SyntaxKind.GreaterThanOrEqualExpression:
                case SyntaxKind.GreaterThanExpression:
                case SyntaxKind.LessThanExpression:
                case SyntaxKind.LessThanOrEqualExpression:
                    return true;

                default:
                    return false;
            }
        }

        public static readonly KustoAnalyzer AvoidUsingObsoleteFunctions = KustoAnalyzer.Create(
            nameof(AvoidUsingObsoleteFunctions),
            new Diagnostic(
                "KS507",
                DiagnosticCategory.Correctness,
                DiagnosticSeverity.Warning,
                description: "Avoid using obsolete/depricated functions."),
            (code, dx, diagnostics) =>
            {
                foreach (var node in code.Syntax.GetDescendants<FunctionCallExpression>(
                    fc => fc.ReferencedSymbol is FunctionSymbol fs && fs.IsObsolete))
                {
                    var symbol = (FunctionSymbol)node.ReferencedSymbol;
                    diagnostics.Add(
                        dx.WithMessage($"The function '{symbol.Name}' is deprecated; use '{symbol.Alternative}' instead.")
                        .WithLocation(node.Name)
                        );
                }
            });

        public static readonly KustoAnalyzer AvoidJoinWithoutKind = KustoAnalyzer.Create(
            nameof(AvoidJoinWithoutKind),
            new Diagnostic(
                "KS508",
                DiagnosticCategory.Correctness,
                DiagnosticSeverity.Warning,
                description: "Avoid using joins without specifying the join kind.",
                message: "Avoid using joins without specifying the join kind. The default join behavior may be unexpected."),
            (code, dx, diagnostics) =>
            {
                foreach (var node in code.Syntax.GetDescendants<JoinOperator>(op =>
                    op.ConditionClause != null
                    && !op.Parameters.Any(p => p.Name.SimpleName == "kind")
                    ))
                {
                    diagnostics.Add(dx.WithLocation(node.JoinKeyword));
                }
            });

        public static readonly KustoAnalyzer StdevTimespanConversion = KustoAnalyzer.Create(
            nameof(StdevTimespanConversion),
            new Diagnostic(
                "KS509",
                category: DiagnosticCategory.Correctness,
                severity: DiagnosticSeverity.Suggestion,
                description: "This use of stdev() results in a number. To get a timespan use totimespan() on the result."),
            (code, dx, diagnostics) =>
            {
                var timespanStdevs = code.Syntax.GetDescendants<FunctionCallExpression>(fc =>
                    fc.ReferencedSymbol == Aggregates.Stdev
                    && fc.ArgumentList.Expressions.Count == 1
                    && fc.ArgumentList.Expressions[0].Element.ResultType == ScalarTypes.TimeSpan);

                foreach (var fc in timespanStdevs)
                {
                    var isConverted = IsArgumentOfFunction(fc, Functions.ToTimespan);
                    if (!isConverted)
                    {
                        diagnostics.Add(dx.WithLocation(fc.Name));
                    }
                }
            });

        private static bool IsArgumentOfFunction(Expression argument, FunctionSymbol symbol)
        {
            return argument.Parent is SeparatedElement<Expression> el
                && el.Parent is SyntaxList<SeparatedElement<Expression>> list
                && list.Parent is ExpressionList elist
                && elist.Parent is FunctionCallExpression fcp
                && fcp.ReferencedSymbol == symbol;
        }

        public static IReadOnlyList<KustoAnalyzer> All =
             new KustoAnalyzer[]
             {
                 AvoidUsingContains,
                 AvoidUsingIsNullStringComparison,
                 AvoidUsingToBoolOnNumerics,
                 AvoidUsingShortStringComparision,
                 NullAggregation,
                 AvoidUsingFormatDatetimeInPredicate,
                 AvoidUsingObsoleteFunctions,
                 AvoidJoinWithoutKind,
                 StdevTimespanConversion
             }
             .ToReadOnly();
    }

    /// <summary>
    /// Analyzer for detecting usage of short string comparisons
    /// </summary>
    internal class AvoidUsingIsNullStringComparisonAnalyzer : KustoAnalyzer
    {
        private static readonly Diagnostic _diagnostic_equals =
            new Diagnostic(
                "KS501",
                category: DiagnosticCategory.Correctness,
                severity: DiagnosticSeverity.Warning,
                description: "Avoid using isnull on string arguments",
                message: "Avoid using isnull on string arguments, use isempty() instead");

        private static readonly Diagnostic _diagnostic_not_equals =
            new Diagnostic(
                "KS502",
                category: DiagnosticCategory.Correctness,
                severity: DiagnosticSeverity.Warning,
                description: "Avoid using isnotnull on string arguments",
                message: "Avoid using isnotnull on string arguments, use isnotempty() instead");

        protected override IEnumerable<Diagnostic> GetDiagnostics()
        {
            return new[] { _diagnostic_equals, _diagnostic_not_equals };
        }

        public override void Analyze(KustoCode code, List<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            foreach (var node in code.Syntax.GetDescendants<FunctionCallExpression>())
            {
                if ((node.ReferencedSymbol == Functions.IsNull ||
                    node.ReferencedSymbol == Functions.IsNotNull) &&
                    node.ArgumentList.Expressions.Count > 0 &&
                    node.ArgumentList.Expressions[0].Element.ResultType == ScalarTypes.String)
                {
                    if (node.ReferencedSymbol == Functions.IsNull)
                    {
                        diagnostics.Add(_diagnostic_equals.WithLocation(node));
                    }
                    else
                    {
                        diagnostics.Add(_diagnostic_not_equals.WithLocation(node));
                    }
                }
            }
        }
    }
}