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
    internal static class KustoAnalyzers
    {
        public static readonly KustoAnalyzer AvoidUsingContains =
            new AvoidUsingContainsAnalyzer();

        public static readonly KustoAnalyzer AvoidUsingNullStringComparison =
            new AvoidUsingNullStringComparisonAnalyzer();

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

        public static readonly KustoAnalyzer AvoidUsingHasWithIPv4Strings =
            new AvoidUsingHasWithIPv4StringsAnalyzer();

        public static readonly KustoAnalyzer PreferUsingOptimizedAlternative =
            new PreferUsingOptimizedAlternative();

        public static readonly KustoAnalyzer AvoidStrlenWithDynamic =
            new AvoidStrlenWithDynamicAnalyzer();

        /// <summary>
        /// The set of all known kusto analyzers
        /// </summary>
        public static IReadOnlyList<KustoAnalyzer> All =
             new KustoAnalyzer[]
             {
                 AvoidUsingContains,
                 AvoidUsingNullStringComparison,
                 AvoidUsingToBoolOnNumerics,
                 NullAggregation,
                 AvoidUsingFormatDatetimeInPredicate,
                 AvoidUsingObsoleteFunctions,
                 AvoidJoinWithoutKind,
                 StdevTimespanConversion,
                 AvoidUsingLegacyPartition,
                 ColumnHasSameNameAsVariable,
                 PreferUsingMaterializedViewIntrinsic,
                 CalledFunctionHasErrors,
                 AvoidUsingHasWithIPv4Strings,
                 PreferUsingOptimizedAlternative,
                 AvoidStrlenWithDynamic
             }
             .ToReadOnly();
    }
}