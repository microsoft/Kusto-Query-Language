using System;
using System.Collections.Generic;
using System.Linq;
using Kusto.Language.Syntax;

namespace Kusto.Language.Analyzer.Rules
{
    internal sealed class NullAggregationDetector : IRule
    {
        public string Name => "Null Aggregation";

        public string Description => "Detects expressions in aggregation context that may result in null";

        public IReadOnlyList<RuleOutcome> Analyze(KustoCode code)
        {
            var outcomes = new List<RuleOutcome>();
            foreach (var node in code.Syntax.GetDescendants<SummarizeOperator>())
            {
                var invocations = GetProblematicInvocations(node);
                var results = invocations.Select(inv => GetOutcomeRule(inv));
                outcomes.AddRange(results);
            }
            return outcomes;
        }

        private RuleOutcome GetOutcomeRule(FunctionCallExpression fc)
        {
            var binaryExpression = fc.ArgumentList.Expressions[0].Element as BinaryExpression; 
            var left = binaryExpression.Left.ToString();
            var right = binaryExpression.Right.ToString();
            var op = binaryExpression.Operator.ToString();
            return new RuleOutcome(
                this.Name,
                score: 5,
                referenceText: fc.ToString(),
                message: 
                $"If any of the columns referenced in '{fc.ToString()}' contains nulls, significant values might not be included due to scalar arithmetic null propagation rules." + Environment.NewLine +
                $"Consider rewriting it as 'sum({left}) {op} sum({right})'.",
                severity: Severity.Warning,
                category: Category.Correctness,
                textStart: fc.TextStart
            );
        }

        private IEnumerable<FunctionCallExpression> GetProblematicInvocations(SyntaxNode node)
        {
            if (!(node is SummarizeOperator op))
            {
                return Enumerable.Empty<FunctionCallExpression>();
            }

            var sumCalls = op.Aggregates.GetDescendants<FunctionCallExpression>(fc => fc.Name.SimpleName.Equals("sum", StringComparison.Ordinal));

            var exprs = sumCalls
                .Where(
                fc =>
                (fc.ArgumentList.Expressions[0].Element is BinaryExpression b) &&
                (b.Kind == SyntaxKind.AddExpression || b.Kind == SyntaxKind.SubtractExpression))
                .ToList();

            return exprs;
        }
    }
}
