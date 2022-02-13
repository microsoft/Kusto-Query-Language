using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Symbols;
    using Utils;

    internal class StdevTimespanConversionAnalyzer : KustoAnalyzer
    {
        private static readonly Diagnostic _diagnostic =
            new Diagnostic(
                KustoAnalyzerCodes.StdevTimespanConversion,
                category: DiagnosticCategory.Correctness,
                severity: DiagnosticSeverity.Suggestion,
                description: "This use of stdev() results in a number. To get a timespan use totimespan() on the result.");

        protected override IEnumerable<Diagnostic> GetDiagnostics()
        {
            return new[] { _diagnostic };
        }

        public override void Analyze(KustoCode code, List<Diagnostic> diagnostics, CancellationToken cancellationToken)
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
                    diagnostics.Add(_diagnostic.WithLocation(fc.Name));
                }
            }
        }

        private static bool IsArgumentOfFunction(Expression argument, FunctionSymbol symbol)
        {
            return argument.Parent is SeparatedElement<Expression> el
                && el.Parent is SyntaxList<SeparatedElement<Expression>> list
                && list.Parent is ExpressionList elist
                && elist.Parent is FunctionCallExpression fcp
                && fcp.ReferencedSymbol == symbol;
        }
    }
}