using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Symbols;
    using Utils;

    /// <summary>
    /// Diagnostic codes produced by kusto analyzers.
    /// They must be unique.
    /// </summary>
    internal static class KustoAnalyzerCodes
    {
        internal const string AvoidUsingContains = "KS500";
        internal const string AvoidUsingIsNullStringComparision = "KS501";
        internal const string AvoidUsingIsNotNullStringComparison = "KS502";
        internal const string AvoidUsingShortStringComparision = "KS503";
        internal const string AvoidUsingToBoolOnNumerics = "KS504";
        internal const string NullAggregation = "KS505";
        internal const string AvoidUsingFormatDateTimeInPredicate = "KS506";
        internal const string AvoidUsingObsoleteFunctions = "KS507";
        internal const string AvoidJoinWithoutKind = "KS508";
        internal const string StdevTimespanConversion = "KS509";
        internal const string ColumnHasSameNameAsVariable = "KS510";
        internal const string PreferUsingMaterializedViewIntrinsic = "KS511";
        internal const string CalledFunctionHasErrors = "KS512";
        internal const string AvoidUsingLegacyPartition = "KS513";
        internal const string AvoidUsingHasWithIPv4Strings = "KS514";
        internal const string PreferUsingOptimizedAlternative = "KS515";
        internal const string AvoidStrlenWithDynamic = "KS516";
    }
}