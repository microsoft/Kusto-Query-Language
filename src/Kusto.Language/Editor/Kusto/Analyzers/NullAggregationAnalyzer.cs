using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Symbols;
    using Utils;

    /// <summary>
    /// Detects expressions in aggregation context that may result in null
    /// </summary>
    internal sealed class NullAggregationAnalyzer : KustoAnalyzer
    {
        private static readonly Diagnostic _diagnostic = new Diagnostic(
                "KS505",
                category: DiagnosticCategory.Correctness,
                severity: DiagnosticSeverity.Warning,
                description: "Avoid using operations on possible null values inside aggregates");

        protected override IEnumerable<Diagnostic> GetDiagnostics()
        {
            return new[] { _diagnostic };
        }

        public override IReadOnlyList<Diagnostic> Analyze(KustoCode code, CancellationToken cancellationToken)
        {
            var diagnostics = new List<Diagnostic>();

            foreach (var node in code.Syntax.GetDescendants<SummarizeOperator>())
            {
                var badSums = node.Aggregates.GetDescendants<FunctionCallExpression>(fc =>
                    fc.ReferencedSymbol == Aggregates.Sum
                    && fc.ArgumentList.Expressions[0].Element is BinaryExpression b
                    && (b.Kind == SyntaxKind.AddExpression 
                     || b.Kind == SyntaxKind.SubtractExpression)
                    );

                foreach (var bs in badSums)
                {
                    diagnostics.Add(GetDiagnostic(bs));
                }
            }

            return diagnostics;
        }

        private static Diagnostic GetDiagnostic(FunctionCallExpression fc)
        {
            var binaryExpression = fc.ArgumentList.Expressions[0].Element as BinaryExpression;
            var left = binaryExpression.Left.ToString();
            var right = binaryExpression.Right.ToString();
            var op = binaryExpression.Operator.Text;

            return _diagnostic
                .WithMessage(
                $"If any of the columns referenced in '{fc}' contains nulls, significant values might not be included due to scalar arithmetic null propagation rules."
                + Environment.NewLine +
                $"Consider rewriting it as '{fc.Name}({left}) {op} {fc.Name}({right})'.")
                .WithLocation(fc);
        }
    }
}