using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Symbols;
    using Utils;

    /// <summary>
    /// The known kusto analyzers
    /// </summary>
    public static class KustoAnalyzers
    {
        public static readonly KustoAnalyzer AvoidUsingContains =
            new AvoidUsingContainsAnalyzer();

        public static readonly KustoAnalyzer AvoidUsingIsNullStringComparison =
            new AvoidUsingIsNullStringComparisonAnalyzer();

        public static readonly KustoAnalyzer AvoidUsingShortStringComparision =
            new AvoidUsingShortStringComparisionAnalyzer();

        public static readonly KustoAnalyzer AvoidUsingToBoolOnNumerics =
            new AvoidUsingToBoolOnNumericsAnalyzer();

        public static readonly KustoAnalyzer NullAggregation =
            new NullAggregationAnalyzer();

        public static readonly KustoAnalyzer AvoidUsingFormatDatetimeInPredicate =
            new AvoidUsingFormatDateTimeInPredicateAnalyzer();

        public static readonly KustoAnalyzer AvoidUsingLegacyPartition =
            new AvoidUsingLegacyPartitionAnalyzer();

        public static readonly KustoAnalyzer AvoidUsingObsoleteFunctions =
            new AvoidUsingObsoleteFunctionsAnalyzer();

        public static readonly KustoAnalyzer AvoidJoinWithoutKind =
            new AvoidJoinWithoutKindAnalyzer();

        public static readonly KustoAnalyzer StdevTimespanConversion =
            new StdevTimespanConversionAnalyzer();

        public static readonly KustoAnalyzer ColumnHasSameNameAsVariable =
            new ColumnHasSameNameAsVariableAnalyzer();

        public static readonly KustoAnalyzer PreferUsingMaterializedViewIntrinsic =
            new PreferUsingMaterializedViewIntrinsicAnalyzer();

        public static readonly KustoAnalyzer CalledFunctionHasErrors =
            new CalledFunctionHasErrorsAnalyzer();

        /// <summary>
        /// The set of all known kusto analyzers
        /// </summary>
        public static IReadOnlyList<KustoAnalyzer> All =
             new KustoAnalyzer[]
             {
                 AvoidUsingContains,
                 AvoidUsingIsNullStringComparison,
                 AvoidUsingToBoolOnNumerics,
                 AvoidUsingShortStringComparision,
                 NullAggregation,
                 AvoidUsingFormatDatetimeInPredicate,
                 AvoidUsingObsoleteFunctions,
                 AvoidJoinWithoutKind,
                 StdevTimespanConversion,
                 AvoidUsingLegacyPartition,
                 ColumnHasSameNameAsVariable,
                 PreferUsingMaterializedViewIntrinsic,
                 CalledFunctionHasErrors
             }
             .ToReadOnly();
    }
}