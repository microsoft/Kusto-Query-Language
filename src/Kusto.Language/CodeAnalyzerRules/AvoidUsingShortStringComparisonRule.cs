using System.Collections.Generic;
using Kusto.Language.Syntax;

namespace Kusto.Language.Analyzer.Rules
{
    internal class AvoidUsingShortStringComparisonRule : IRule
    {
        public string Name => "Avoid short strings comparison";

        public string Description => "Avoid using short strings (less than 4 characters) for string comparison operations";

        public IReadOnlyList<RuleOutcome> Analyze(KustoCode code)
        {
            var outcomes = new List<RuleOutcome>();
            foreach (var node in code.Syntax.GetDescendants<BinaryExpression>())
            {
                if (!node.Right.IsConstant && !node.Left.IsConstant)
                {
                    continue;
                }

                if (node.Kind == SyntaxKind.EqualExpression ||
                    node.Kind == SyntaxKind.HasExpression ||
                    node.Kind == SyntaxKind.NotEqualExpression ||
                    node.Kind == SyntaxKind.NotHasExpression ||
                    node.Kind == SyntaxKind.StartsWithExpression ||
                    node.Kind == SyntaxKind.NotStartsWithExpression)
                {
                    string constValue = null;
                    if (node.Right.IsConstant && node.Right.ResultType.Name == "string")
                    {
                        constValue = node.Right.ConstantValue?.ToString();
                    }
                    else if (node.Left.IsConstant && node.Left.ResultType.Name == "string")
                    {
                        constValue = node.Left.ConstantValue?.ToString();
                    }

                    if (string.IsNullOrEmpty(constValue))
                    {
                        continue;
                    }

                    if (constValue.Length < 4)
                    {
                        var ruleOutcome = new RuleOutcome(Name,
                        score: 10,
                        message: Description,
                        referenceText: node.ToString(),
                        severity: Severity.Suggestion,
                        category: Category.Performance,
                        textStart: node.TextStart);
                        outcomes.Add(ruleOutcome);
                    }
                }
            }
            return outcomes;
        }
    }
}
