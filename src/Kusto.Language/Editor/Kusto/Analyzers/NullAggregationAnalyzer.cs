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
    internal sealed class NullAggregationDetector : KustoAnalyzer
    {
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

            return new Diagnostic(
                "KustoNullAggregation",
                category: DiagnosticCategory.Correctness,
                severity: DiagnosticSeverity.Warning,
                message:
                $"If any of the columns referenced in '{fc.ToString()}' contains nulls, significant values might not be included due to scalar arithmetic null propagation rules."
                + Environment.NewLine +
                $"Consider rewriting it as '{fc.Name}({left}) {op} {fc.Name}({right})'.")
                .WithLocation(fc);
        }
    }
}