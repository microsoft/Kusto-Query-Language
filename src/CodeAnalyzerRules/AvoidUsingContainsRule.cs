using System;
using System.Collections.Generic;
using Kusto.Language.Syntax;

namespace Kusto.Language.Analyzer.Rules
{
    internal class AvoidUsingContainsRule : IRule
    {
        public string Name => "Avoid using 'contains'";

        public string Description => "Detects expressions that use 'contains' operator";

        public IReadOnlyList<RuleOutcome> Analyze(KustoCode code)
        {
            var outcomes = new List<RuleOutcome>();
            foreach (var node in code.Syntax.GetDescendants<BinaryExpression>())
            {
                if (node.Kind == SyntaxKind.ContainsExpression ||
                    node.Kind == SyntaxKind.NotContainsExpression || 
                    node.Kind == SyntaxKind.ContainsCsExpression ||
                    node.Kind == SyntaxKind.NotContainsCsExpression)
                {
                    var ruleOutcome = new RuleOutcome(Name,
                        score: 10,
                        message:
                        $"Avoid using 'contains' operator as it has high compute price." + Environment.NewLine +
                        $"Use 'has' operator in cases when full term match is desired.",
                        referenceText: node.ToString(),
                        severity: Severity.Suggestion,
                        category: Category.Performance,
                        textStart: node.TextStart);
                    outcomes.Add(ruleOutcome);
                }
            }
            return outcomes;
        }
    }
}
