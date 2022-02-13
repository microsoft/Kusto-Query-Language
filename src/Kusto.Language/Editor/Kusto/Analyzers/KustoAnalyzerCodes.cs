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
        internal static string AvoidUsingContains = "KS500";
        internal static string AvoidUsingIsNullStringComparision = "KS501";
        internal static string AvoidUsingIsNotNullStringComparison = "KS502";
        internal static string AvoidUsingShortStringComparision = "KS503";
        internal static string AvoidUsingToBoolOnNumerics = "KS504";
        internal static string NullAggregation = "KS505";
        internal static string AvoidUsingFormatDateTimeInPredicate = "KS506";
        internal static string AvoidUsingObsoleteFunctions = "KS507";
        internal static string AvoidJoinWithoutKind = "KS508";
        internal static string StdevTimespanConversion = "KS509";
        internal static string ColumnHasSameNameAsVariable = "KS510";
        internal static string PreferUsingMaterializedViewIntrinsic = "KS511";
        internal static string CalledFunctionHasErrors = "KS512";
        internal static string AvoidUsingLegacyPartition = "KS513";
    }

}