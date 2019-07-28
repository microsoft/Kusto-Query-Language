using System.Collections.Generic;

namespace Kusto.Language.Analyzer.Rules
{
    public static class KustoRulesCollection
    {
        public static readonly IReadOnlyList<IRule> Rules
            = new List<IRule>()
            {
                new NullAggregationDetector(),
                new AvoidUsingContainsRule(),
                new AvoidUsingShortStringComparisonRule(),
            };
    }
}
