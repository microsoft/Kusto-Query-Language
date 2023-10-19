using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Symbols;
    using Utils;

    internal class NullAggregationAnalyzer : KustoAnalyzer
    {
        private static readonly Diagnostic _diagnostic =
            new Diagnostic(
                KustoAnalyzerCodes.NullAggregation,
                category: DiagnosticCategory.Correctness,
                severity: DiagnosticSeverity.Warning,
                description: "Avoid using operations on possible null values inside sum aggregates");

        protected override IEnumerable<Diagnostic> GetDiagnostics()
        {
            return new[] { _diagnostic };
        }

        public override void Analyze(KustoCode code, List<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            foreach (var node in code.Syntax.GetDescendants<SummarizeOperator>())
            {
                var badSums = node.Aggregates.GetDescendants<FunctionCallExpression>(fc =>
                    fc.ReferencedSymbol == Aggregates.Sum
                    && fc.ArgumentList.Expressions.Count > 0
                    && fc.ArgumentList.Expressions[0].Element is BinaryExpression b
                    && (b.Kind == SyntaxKind.AddExpression
                     || b.Kind == SyntaxKind.SubtractExpression));

                foreach (var bs in badSums)
                {
                    var binaryExpression = bs.ArgumentList.Expressions[0].Element as BinaryExpression;
                    var left = binaryExpression.Left.ToString();
                    var right = binaryExpression.Right.ToString();
                    var op = binaryExpression.Operator.Text;

                    diagnostics.Add(_diagnostic.WithMessage(
                        $"If any of the columns referenced in '{bs}' contains nulls, significant values might not be included due to scalar arithmetic null propagation rules."
                        + Environment.NewLine +
                        $"Consider rewriting it as '{bs.Name}({left}) {op} {bs.Name}({right})'.")
                        .WithLocation(bs));
                }
            }
        }
    }
}