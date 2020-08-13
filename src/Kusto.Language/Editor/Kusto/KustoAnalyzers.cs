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

        public static readonly KustoAnalyzer AvoidUsingIsNullStringComparison =
            new AvoidUsingIsNullStringComparisonAnalyzer();

        public static readonly KustoAnalyzer AvoidUsingToBoolOnNumerics =
            new AvoidUsingToBoolOnNumericsAnalyzer();

        public static readonly KustoAnalyzer AvoidUsingShortStringComparision =
            new AvoidUsingShortStringComparisonAnalyzer();

        public static readonly KustoAnalyzer NullAggregation =
            new NullAggregationAnalyzer();

        public static readonly KustoAnalyzer AvoidUsingFormatDatetimeInPredicate =
            new AvoidUsingFormatDatetimeInPredicatesAnalyzer();

        public static readonly KustoAnalyzer AvoidUsingObsoleteFunctions =
            new AvoidObsoleteFunctionsAnalyzer();

        public static IReadOnlyList<KustoAnalyzer> All =
             new KustoAnalyzer[]
             {
                 AvoidUsingContains,
                 AvoidUsingIsNullStringComparison,
                 AvoidUsingToBoolOnNumerics,
                 AvoidUsingShortStringComparision,
                 NullAggregation,
                 AvoidUsingFormatDatetimeInPredicate,
                 AvoidUsingObsoleteFunctions
             }
             .ToReadOnly();
    }
}