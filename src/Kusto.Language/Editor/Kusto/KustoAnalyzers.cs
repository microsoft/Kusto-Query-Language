using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Utils;

    /// <summary>
    /// The base class for any <see cref="KustoCode"/> analyzer.
    /// </summary>
    public static class KustoAnalyzers
    {
        public static readonly KustoAnalyzer AvoidUsingContains =
            new AvoidUsingContainsAnalyzer();

        public static readonly KustoAnalyzer AvoidUsingShortStringComparision =
            new AvoidUsingShortStringComparisonRule();

        public static readonly KustoAnalyzer NullAggregation =
            new NullAggregationDetector();

        public static IReadOnlyList<KustoAnalyzer> All =
             new KustoAnalyzer[]
             {
                 AvoidUsingContains,
                 AvoidUsingShortStringComparision,
                 NullAggregation,
             }
             .ToReadOnly();
    }
}