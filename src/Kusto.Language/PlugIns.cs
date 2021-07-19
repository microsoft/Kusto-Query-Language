using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language
{
    using Symbols;
    using Syntax;
    using static FunctionHelpers;

    /// <summary>
    /// Well known plugins.
    /// </summary>
    public static class PlugIns
    {
        public static readonly FunctionSymbol ActiveUseCounts =
            new FunctionSymbol("active_users_count",
                (table, args, signature) => {
                    var cols = new List<ColumnSymbol>();
                    AddReferencedColumn(cols, args, signature, "TimelineColumn"); // timeline
                    AddReferencedColumns(cols, args, signature, "Dimension"); // dimensions
                    cols.Add(new ColumnSymbol("dcount", ScalarTypes.Long));
                    return new TableSymbol(cols);
                },
                Tabularity.Tabular,
                new Parameter("IdColumn", ParameterTypeKind.NotDynamic, ArgumentKind.Column),
                new Parameter("TimelineColumn", ParameterTypeKind.Summable, ArgumentKind.Column),
                new Parameter("Start", ParameterTypeKind.Summable, ArgumentKind.Constant),
                new Parameter("End", ParameterTypeKind.Summable, ArgumentKind.Constant),
                new Parameter("LookbackWindow", ParameterTypeKind.Summable, ArgumentKind.Constant),
                new Parameter("Period", ParameterTypeKind.Summable, ArgumentKind.Constant),
                new Parameter("ActivePeriods", ParameterTypeKind.Summable, ArgumentKind.Constant),
                new Parameter("Step", ParameterTypeKind.Summable, ArgumentKind.Constant),
                new Parameter("Dimension", ParameterTypeKind.NotDynamic, ArgumentKind.Column, minOccurring: 0, maxOccurring: MaxRepeat)
                );

        public static readonly FunctionSymbol ActivityCountsMetrics =
            new FunctionSymbol("activity_counts_metrics",
                (table, args, signature) =>
                {
                    var cols = new List<ColumnSymbol>();
                    AddReferencedColumn(cols, args, signature, "TimelineColumn"); // timeline column
                    AddReferencedColumns(cols, args, signature, "Dimension"); // dimension columns
                    cols.Add(new ColumnSymbol("count", ScalarTypes.Long));
                    cols.Add(new ColumnSymbol("dcount", ScalarTypes.Long));
                    cols.Add(new ColumnSymbol("new_dcount", ScalarTypes.Long));
                    cols.Add(new ColumnSymbol("aggregated_dcount", ScalarTypes.Long));
                    return new TableSymbol(cols);
                },
                Tabularity.Tabular,
                new Parameter("IdColumn", ParameterTypeKind.NotDynamic, ArgumentKind.Column),
                new Parameter("TimelineColumn", ParameterTypeKind.Summable, ArgumentKind.Column),
                new Parameter("Start", ParameterTypeKind.Summable, ArgumentKind.Constant),
                new Parameter("End", ParameterTypeKind.Summable, ArgumentKind.Constant),
                new Parameter("Step", ParameterTypeKind.Summable, ArgumentKind.Constant),
                new Parameter("Dimension", ParameterTypeKind.NotDynamic, ArgumentKind.Column, minOccurring: 0, maxOccurring: MaxRepeat)
                );

        public static readonly FunctionSymbol ActivityEngagement =
            new FunctionSymbol("activity_engagement",
                new Signature(
                    (table, args, signature) =>
                    {
                        var cols = new List<ColumnSymbol>();
                        AddReferencedColumn(cols, args, signature, "TimelineColumn"); // timeline column
                        cols.Add(new ColumnSymbol("dcount_activities_inner", ScalarTypes.Long)); // inner activity
                        cols.Add(new ColumnSymbol("dcount_activities_outer", ScalarTypes.Long)); // outer activity
                        cols.Add(new ColumnSymbol("activity_ratio", ScalarTypes.Real));
                        return new TableSymbol(cols);
                    },
                    Tabularity.Tabular,
                    new Parameter("IdColumn", ParameterTypeKind.NotDynamic, ArgumentKind.Column),
                    new Parameter("TimelineColumn", ParameterTypeKind.Summable, ArgumentKind.Column),
                    new Parameter("InnerActivityWindow", ParameterTypeKind.Summable, ArgumentKind.Constant),
                    new Parameter("OuterActivityWindow", ParameterTypeKind.Summable, ArgumentKind.Constant)),
                new Signature(
                    (table, args, signature) =>
                    {
                        var cols = new List<ColumnSymbol>();
                        AddReferencedColumn(cols, args, signature, "TimelineColumn"); // timeline column
                        AddReferencedColumns(cols, args, signature, "Dimension"); // dimension columns
                        cols.Add(new ColumnSymbol("dcount_activities_inner", ScalarTypes.Long)); // inner activity
                        cols.Add(new ColumnSymbol("dcount_activities_outer", ScalarTypes.Long)); // outer activity
                        cols.Add(new ColumnSymbol("activity_ratio", ScalarTypes.Real));
                        return new TableSymbol(cols);
                    },
                    Tabularity.Tabular,
                    new Parameter("IdColumn", ParameterTypeKind.NotDynamic, ArgumentKind.Column),
                    new Parameter("TimelineColumn", ParameterTypeKind.Summable, ArgumentKind.Column),
                    new Parameter("Start", ParameterTypeKind.Summable, ArgumentKind.Constant),
                    new Parameter("End", ParameterTypeKind.Summable, ArgumentKind.Constant),
                    new Parameter("InnerActivityWindow", ParameterTypeKind.Summable, ArgumentKind.Constant),
                    new Parameter("OuterActivityWindow", ParameterTypeKind.Summable, ArgumentKind.Constant),
                    new Parameter("Dimension", ParameterTypeKind.NotDynamic, ArgumentKind.Column, minOccurring: 0, maxOccurring: MaxRepeat))
                );

        public static readonly FunctionSymbol ActivityMetrics =
            new FunctionSymbol("activity_metrics",
                new Signature(
                    (table, args, signature) => {
                        var cols = new List<ColumnSymbol>();
                        AddReferencedColumn(cols, args, signature, "TimelineColumn"); // timeline columns
                        AddReferencedColumns(cols, args, signature, "Dimension"); // dimension columns
                        cols.Add(new ColumnSymbol("dcount_values", ScalarTypes.Long));
                        cols.Add(new ColumnSymbol("dcount_newvalues", ScalarTypes.Long));
                        cols.Add(new ColumnSymbol("retention_rate", ScalarTypes.Real));
                        cols.Add(new ColumnSymbol("churn_rate", ScalarTypes.Real));
                        return new TableSymbol(cols);
                    },
                    Tabularity.Tabular,
                    new Parameter("IdColumn", ParameterTypeKind.Scalar, ArgumentKind.Column),
                    new Parameter("TimelineColumn", ParameterTypeKind.Summable, ArgumentKind.Column),
                    new Parameter("Start", ParameterTypeKind.Summable, ArgumentKind.Constant),
                    new Parameter("End", ParameterTypeKind.Summable, ArgumentKind.Constant),
                    new Parameter("Step", ParameterTypeKind.Summable, ArgumentKind.Constant),
                    new Parameter("Dimension", ParameterTypeKind.NotDynamic, ArgumentKind.Column, minOccurring: 0, maxOccurring: MaxRepeat)),
                new Signature(
                    (table, args, signature) => {
                        var cols = new List<ColumnSymbol>();
                        AddReferencedColumn(cols, args, signature, "TimelineColumn"); // timeline columns
                        cols.Add(new ColumnSymbol("dcount_values", ScalarTypes.Long));
                        cols.Add(new ColumnSymbol("dcount_newvalues", ScalarTypes.Long));
                        cols.Add(new ColumnSymbol("retention_rate", ScalarTypes.Real));
                        cols.Add(new ColumnSymbol("churn_rate", ScalarTypes.Real));
                        return new TableSymbol(cols);
                    },
                    Tabularity.Tabular,
                    new Parameter("IdColumn", ParameterTypeKind.Scalar, ArgumentKind.Column),
                    new Parameter("TimelineColumn", ParameterTypeKind.Summable, ArgumentKind.Column),
                    new Parameter("Start", ParameterTypeKind.Summable, ArgumentKind.Constant, minOccurring: 0),
                    new Parameter("End", ParameterTypeKind.Summable, ArgumentKind.Constant, minOccurring: 0))
                );

        private static Parameter nam_IdColumn = new Parameter("IdColumn", ParameterTypeKind.NotDynamic, ArgumentKind.Column);
        private static Parameter nam_TimelineColumn = new Parameter("TimelineColumn", ParameterTypeKind.Summable, ArgumentKind.Column);
        private static Parameter nam_Start = new Parameter("Start", ParameterTypeKind.Summable, ArgumentKind.Constant);
        private static Parameter nam_End = new Parameter("End", ParameterTypeKind.Summable, ArgumentKind.Constant);
        private static Parameter nam_Window = new Parameter("Window", ParameterTypeKind.Scalar);
        private static Parameter nam_Cohort = new Parameter("Cohort", ParameterTypeKind.Scalar, ArgumentKind.Constant, minOccurring: 0);
        private static Parameter nam_Dimension = new Parameter("Dimension", ParameterTypeKind.NotDynamic, ArgumentKind.Column, minOccurring: 0, maxOccurring: MaxRepeat);
        private static Parameter nam_Lookback = new Parameter("lookback", ParameterTypeKind.Tabular, minOccurring: 0);

        public static readonly FunctionSymbol NewActivityMetrics =
            new FunctionSymbol("new_activity_metrics",
                 new Signature(
                     (table, args, signature) =>
                     {
                         var cols = new List<ColumnSymbol>();

                         var timelineArg = GetArgument(args, signature, "TimelineColumn"); // timeline column
                         if (timelineArg != null)
                         {
                             var timelineArgName = GetExpressionResultName(timelineArg);
                             cols.Add(new ColumnSymbol(MakeColumnName("from", timelineArgName), timelineArg.ResultType));
                             cols.Add(new ColumnSymbol(MakeColumnName("to", timelineArgName), timelineArg.ResultType));
                         }

                         AddReferencedColumns(cols, args, signature, "Dimension"); // dimension columns

                         cols.Add(new ColumnSymbol("dcount_new_values", ScalarTypes.Long));
                         cols.Add(new ColumnSymbol("dcount_retained_values", ScalarTypes.Long));
                         cols.Add(new ColumnSymbol("dcount_churn_values", ScalarTypes.Long));
                         cols.Add(new ColumnSymbol("retention_rate", ScalarTypes.Real));
                         cols.Add(new ColumnSymbol("churn_rate", ScalarTypes.Real));
                         return new TableSymbol(cols);
                     },
                     Tabularity.Tabular,
                     nam_IdColumn,
                     nam_TimelineColumn,
                     nam_Start,
                     nam_End,
                     nam_Window,
                     nam_Cohort,
                     nam_Dimension,
                     nam_Lookback)
                .WithLayout((sig, args, list) =>
                {
                    // add these even if not that many arguments supplied.. should not cause a problem
                    list.Add(nam_IdColumn);
                    list.Add(nam_TimelineColumn);
                    list.Add(nam_Start);
                    list.Add(nam_End);
                    list.Add(nam_Window);

                    // if the next argument is a constant (or not a column == dimension), then it must be the optional cohort argument
                    if (args.Count > 5 && (args[5].IsConstant || !(args[5].ReferencedSymbol is ColumnSymbol)))
                        list.Add(nam_Cohort);

                    // after cohort, all non-tabular arguments are dimension arguments
                    int i = list.Count;
                    for (; i < args.Count && !args[i].ResultType.IsTabular; i++)
                    {
                        list.Add(nam_Dimension);
                    }

                    // this last argument should be the lookback table
                    if (i < args.Count)
                        list.Add(nam_Lookback);
                }));

        public static readonly IReadOnlyList<ColumnSymbol> AutoClusterColumns = new[] {
            new ColumnSymbol("SegmentId", ScalarTypes.Long),
            new ColumnSymbol("Count", ScalarTypes.Long),
            new ColumnSymbol("Percent", ScalarTypes.Real)
        };

        public static readonly FunctionSymbol AutoCluster =
            new FunctionSymbol("autocluster",
                (table, args) => new TableSymbol(AutoClusterColumns.Concat(table.Columns)).WithInheritableProperties(table),
                Tabularity.Tabular,
                new Parameter("SizeWeight", ScalarTypes.Real, defaultValueIndicator: "~", minOccurring: 0),
                new Parameter("WeightColumn", ParameterTypeKind.Scalar, ArgumentKind.Column, defaultValueIndicator: "~", minOccurring: 0),
                new Parameter("NumSeeds", ParameterTypeKind.Integer, defaultValueIndicator: "~", minOccurring: 0),
                new Parameter("CustomWildcard", ParameterTypeKind.Scalar, minOccurring: 0, maxOccurring: MaxRepeat));

        public static readonly FunctionSymbol BagUnpack =
             new FunctionSymbol("bag_unpack",
                 (table, args) => new TableSymbol(table.Columns.Where(c => args.Count == 0 || c != args[0].ReferencedSymbol)).WithInheritableProperties(table).WithIsOpen(true),
                 Tabularity.Tabular,
                 new Parameter("column", ScalarTypes.Dynamic, ArgumentKind.Column),
                 new Parameter("column_prefix", ScalarTypes.String, ArgumentKind.LiteralNotEmpty, minOccurring: 0));

        public static readonly IReadOnlyList<ColumnSymbol> BasketColumns = new[] {
            new ColumnSymbol("SegmentId", ScalarTypes.Long),
            new ColumnSymbol("Count", ScalarTypes.Long),
            new ColumnSymbol("Percent", ScalarTypes.Real)
        };

        public static readonly FunctionSymbol Basket =
             new FunctionSymbol("basket",
                 (table, args) => new TableSymbol(BasketColumns.Concat(table.Columns)).WithInheritableProperties(table),
                 Tabularity.Tabular,
                 new Parameter("Threshold", ScalarTypes.Real, defaultValueIndicator: "~", minOccurring: 0),
                 new Parameter("WeightColumn", ParameterTypeKind.Scalar, ArgumentKind.Column, defaultValueIndicator: "~", minOccurring: 0),
                 new Parameter("MaxDimensions", ParameterTypeKind.Integer, defaultValueIndicator: "~", minOccurring: 0),
                 new Parameter("CustomWildcard", ParameterTypeKind.Scalar, minOccurring: 0, maxOccurring: MaxRepeat));

        public static readonly FunctionSymbol CosmosdbSqlRequest =
             new FunctionSymbol("cosmosdb_sql_request",
                 (table, args) => new TableSymbol().WithIsOpen(true), // the schema comes from the cosmos database at runtime
                 Tabularity.Tabular,
                 new Parameter("endpoint", ScalarTypes.String),
                 new Parameter("authorization_string", ScalarTypes.String),
                 new Parameter("database_name", ScalarTypes.String),
                 new Parameter("collection_name", ScalarTypes.String),
                 new Parameter("sql_query", ScalarTypes.String)
                 );

        public static readonly FunctionSymbol AzureDigitalTwinsQueryRequest =
                     new FunctionSymbol("azure_digital_twins_query_request",
                         (table, args) => new TableSymbol().WithIsOpen(true), // depends on the SELECT command provided
                         Tabularity.Tabular,
                         new Parameter("endpoint", ScalarTypes.String),
                         new Parameter("sql_query", ScalarTypes.String)
                         );
        
        public static readonly FunctionSymbol DCountIntersect =
             new FunctionSymbol("dcount_intersect",
                 (table, args) => new TableSymbol(table.Columns.Concat(args.Select((a, i) => new ColumnSymbol("s" + i, ScalarTypes.Long)))).WithInheritableProperties(table),
                 Tabularity.Tabular,
                 new Parameter("hll", ScalarTypes.Dynamic, minOccurring: 2, maxOccurring: MaxRepeat));

        public static readonly IReadOnlyList<ColumnSymbol> DiffPatternsColumns = new[] {
            new ColumnSymbol("SegmentId", ScalarTypes.Long),
            new ColumnSymbol("CountA", ScalarTypes.Long),
            new ColumnSymbol("CountB", ScalarTypes.Long),
            new ColumnSymbol("PercentA", ScalarTypes.Real),
            new ColumnSymbol("PercentB", ScalarTypes.Real),
            new ColumnSymbol("PercentDiffAB", ScalarTypes.Real)
        };

        public static readonly FunctionSymbol DiffPatterns =
             new FunctionSymbol("diffpatterns",
                 (table, args) => new TableSymbol(DiffPatternsColumns.Concat(table.Columns)).WithInheritableProperties(table),
                 Tabularity.Tabular,
                 new Parameter("SplitColumn", ParameterTypeKind.Scalar, ArgumentKind.Column),
                 new Parameter("SplitValueA", ScalarTypes.String),
                 new Parameter("SplitValueB", ScalarTypes.String),
                 new Parameter("WeightColumn", ParameterTypeKind.Scalar, ArgumentKind.Column, defaultValueIndicator: "~", minOccurring: 0),
                 new Parameter("Threshold", ScalarTypes.Real, defaultValueIndicator: "~", minOccurring: 0),
                 new Parameter("MaxDimensions", ParameterTypeKind.Integer, defaultValueIndicator: "~", minOccurring: 0),
                 new Parameter("CustomWildcard", ParameterTypeKind.Scalar, minOccurring: 0, maxOccurring: MaxRepeat));

        public static readonly FunctionSymbol EstimateRowsCount =
             new FunctionSymbol("estimate_rows_count",
                 new Signature(
                     (table, args) => new TableSymbol(new ColumnSymbol("EstimatedRowsCount", ScalarTypes.Long)),
                    Tabularity.Tabular));

        public static readonly FunctionSymbol ExecuteShowCommand =
             new FunctionSymbol("execute_show_command",
                 (table, args) => new TableSymbol().WithIsOpen(true), // depends on contents of command string
                 Tabularity.Tabular,
                 new Parameter("connection_string", ScalarTypes.String),
                 new Parameter("command", ScalarTypes.String));

        public static readonly FunctionSymbol ExecuteQuery =
             new FunctionSymbol("execute_query",
                 (table, args) => new TableSymbol().WithIsOpen(true), // depends on contents of command string
                 Tabularity.Tabular,
                 new Parameter("connection_string", ScalarTypes.String),
                 new Parameter("query", ScalarTypes.String));

        public static readonly FunctionSymbol ExternalDatatable =
             new FunctionSymbol("external_datatable",
                 new Signature(
                     (table, args) => new TableSymbol().WithIsOpen(true), // depends on the data sent from the client
                    Tabularity.Tabular));

#if false   // problem with multiple repeating parameters
            new FunctionSymbol("funnel_analysis",
                 (table, args) => table,
                 Tabularity.Tabular,
                 new Parameter("IdColumn", ParameterTypeKind.Scalar) // needs to be numeric or dynamic? 
                 ),
#endif
        public static readonly FunctionSymbol FunnelSequence =
            new FunctionSymbol("funnel_sequence",
                (table, args, signature) =>
                {
                    // only declare first table, as additional schema is not useful to intellisense
                    var cols = new List<ColumnSymbol>();
                    AddReferencedColumn(cols, args, signature, "TimelineColumn");

                    var stateArg = GetArgument(args, signature, "StateColumn");
                    if (stateArg != null)
                    {
                        cols.Add(new ColumnSymbol("prev", stateArg.ResultType));
                        cols.Add(new ColumnSymbol("next", stateArg.ResultType));
                    }

                    cols.Add(new ColumnSymbol("dcount", ScalarTypes.Long));
                    cols.Add(new ColumnSymbol("samples", ScalarTypes.Dynamic));
                    return new TableSymbol(cols);
                },
                Tabularity.Tabular,
                new Parameter("IdColumn", ParameterTypeKind.NotDynamic, ArgumentKind.Column),
                new Parameter("TimelineColumn", ParameterTypeKind.Summable, ArgumentKind.Column),
                new Parameter("Start", ParameterTypeKind.Summable, ArgumentKind.Constant),
                new Parameter("End", ParameterTypeKind.Summable, ArgumentKind.Constant),
                new Parameter("MaxWindowSizeBetweenSteps", ParameterTypeKind.Summable, ArgumentKind.Constant),
                new Parameter("Step", ParameterTypeKind.Summable, ArgumentKind.Constant),
                new Parameter("StateColumn", ParameterTypeKind.NotDynamic, ArgumentKind.Column),
                new Parameter("Sequence", ScalarTypes.Dynamic, ArgumentKind.Constant));

        public static readonly FunctionSymbol FunnelSequenceCompletion =
            new FunctionSymbol("funnel_sequence_completion",
                (table, args, signature) => {
                    var cols = new List<ColumnSymbol>();
                    AddReferencedColumn(cols, args, signature, "TimelineColumn");

                    var stateArg = GetArgument(args, signature, "StateColumn");
                    if (stateArg != null)
                    {
                        cols.Add(new ColumnSymbol(GetExpressionResultName(stateArg), ScalarTypes.String));
                    }

                    cols.Add(new ColumnSymbol("Period", ScalarTypes.TimeSpan));
                    cols.Add(new ColumnSymbol("dcount", ScalarTypes.Long));
                    return new TableSymbol(cols);
                },
                Tabularity.Tabular,
                new Parameter("IdColumn", ParameterTypeKind.NotDynamic, ArgumentKind.Column),
                new Parameter("TimelineColumn", ParameterTypeKind.Summable, ArgumentKind.Column),
                new Parameter("Start", ParameterTypeKind.Summable, ArgumentKind.Constant),
                new Parameter("End", ParameterTypeKind.Summable, ArgumentKind.Constant),
                new Parameter("BinSize", ParameterTypeKind.Summable, ArgumentKind.Constant),
                new Parameter("StateColumn", ParameterTypeKind.NotDynamic, ArgumentKind.Column),
                new Parameter("Sequence", ScalarTypes.Dynamic, ArgumentKind.Constant),
                new Parameter("MaxSequencePeriods", ScalarTypes.Dynamic, ArgumentKind.Constant));

        public static readonly IReadOnlyList<ColumnSymbol> HttpRequestColumns = new[] {
            new ColumnSymbol("ResponseHeaders", ScalarTypes.Dynamic),
            new ColumnSymbol("ResponseBody", ScalarTypes.Dynamic)
        };

        public static readonly FunctionSymbol HttpRequest =
             new FunctionSymbol("http_request",
                 new TableSymbol(HttpRequestColumns),
                 new Parameter("Uri", ScalarTypes.String, ArgumentKind.Constant),
                 new Parameter("RequestHeaders", ScalarTypes.Dynamic, minOccurring: 0),
                 new Parameter("Options", ScalarTypes.Dynamic, minOccurring: 0));

        public static readonly FunctionSymbol HttpRequestPost =
             new FunctionSymbol("http_request_post",
                 new TableSymbol(HttpRequestColumns),
                 new Parameter("Uri", ScalarTypes.String, ArgumentKind.Constant),
                 new Parameter("RequestHeaders", ScalarTypes.Dynamic, minOccurring: 0),
                 new Parameter("Options", ScalarTypes.Dynamic, minOccurring: 0),
                 new Parameter("Content", ScalarTypes.String, ArgumentKind.Constant, minOccurring: 0));

        public static readonly FunctionSymbol Identity =
             new FunctionSymbol("identity",
                 new Signature(
                     (table, args) => table,
                     Tabularity.Tabular));

        public static readonly FunctionSymbol IdentityV3 =
             new FunctionSymbol("identity_v3",
                (table, args) => table,
                Tabularity.Tabular,
                new Parameter("mode", ScalarTypes.String, ArgumentKind.Constant),
                new Parameter("exceptionText", ScalarTypes.String, ArgumentKind.Constant));

        public static readonly FunctionSymbol InferStorageSchema =
             new FunctionSymbol("infer_storage_schema",
                 new TableSymbol(new[] {
                    new ColumnSymbol("CslSchema", ScalarTypes.String),
                 }),
                 new Parameter("Options", ScalarTypes.Dynamic));

        public static readonly FunctionSymbol SchemaMerge =
             new FunctionSymbol("schema_merge",
                 new TableSymbol(new[] {
                    new ColumnSymbol("ColumnName", ScalarTypes.String),
                    new ColumnSymbol("ColumnOrdinal", ScalarTypes.Int),
                    new ColumnSymbol("DataType", ScalarTypes.String),
                    new ColumnSymbol("ColumnType", ScalarTypes.String),
                 }),
                 new Parameter("PreserveOrder", ScalarTypes.Bool));

        public static readonly IReadOnlyList<ColumnSymbol> NarrowColumns = new[]
        {
            new ColumnSymbol("Row", ScalarTypes.Long),
            new ColumnSymbol("Column", ScalarTypes.String),
            new ColumnSymbol("Value", ScalarTypes.String)
        };

        public static readonly FunctionSymbol Narrow =
            new FunctionSymbol("narrow",
                 new TableSymbol(NarrowColumns));

        public static readonly FunctionSymbol Pivot =
             new FunctionSymbol("pivot",
                 (table, args) =>
                 {
                     var pivotColumn = args.Count > 0 ? args[0].ReferencedSymbol as ColumnSymbol : null;

                     var aggregateColumn =
                        args.Count > 1
                        && args[1] is Syntax.FunctionCallExpression fc
                        && fc.ArgumentList.Expressions.Count > 0
                            ? fc.ArgumentList.Expressions[0].Element.ReferencedSymbol as ColumnSymbol
                            : null;

                     // columns specified
                     var columns = args.Skip(2).Select(a => a.ReferencedSymbol as ColumnSymbol).Where(c => c != null).ToList();

                     if (columns.Count == 0)
                     {
                         // all columns exept explicity mentioned pivot and aggregate column
                         columns.AddRange(table.Columns.Where(c => c != pivotColumn && c != aggregateColumn));
                     }

                     // pivot table is open because it has additional columns based on data values
                     return new TableSymbol(columns).WithIsOpen(true);
                 },
                 Tabularity.Tabular,
                 new Parameter("pivotColumn", ParameterTypeKind.NotDynamic, ArgumentKind.Column),
                 new Parameter("aggregateFunction", ParameterTypeKind.Scalar, ArgumentKind.Aggregate, minOccurring: 0),
                 new Parameter("columnName", ParameterTypeKind.Scalar, ArgumentKind.Column, minOccurring: 0, maxOccurring: MaxRepeat));

        public static readonly FunctionSymbol Preview =
             new FunctionSymbol("preview",
                 (table, args) => new GroupSymbol( // multiple result tables
                     table,
                     new TableSymbol(new ColumnSymbol("Count", ScalarTypes.Long))),
                 Tabularity.Tabular,
                 new Parameter("NumberOfRows", ParameterTypeKind.Integer));

        public static readonly FunctionSymbol CSharp =
             new FunctionSymbol("csharp",
                 (table, args) => new TableSymbol().WithIsOpen(true), // TODO: can we parse the output schema argument?
                 Tabularity.Tabular,
                 new Parameter("OutputSchema", ScalarTypes.Type),
                 new Parameter("Script", ScalarTypes.String),
                 new Parameter("ScriptParameters", ScalarTypes.Dynamic, minOccurring: 0));

        public static readonly FunctionSymbol Python =
             new FunctionSymbol("python",
                 (table, args) => new TableSymbol().WithIsOpen(true), // TODO: can we parse the output schema argument?
                 Tabularity.Tabular,
                 new Parameter("OutputSchema", ScalarTypes.Type),
                 new Parameter("Script", ScalarTypes.String),
                 new Parameter("ScriptParameters", ScalarTypes.Dynamic, minOccurring: 0));

        public static readonly FunctionSymbol R =
             new FunctionSymbol("r",
                 (table, args) => new TableSymbol().WithIsOpen(true), // TODO: can we parse the output schema argument?
                 Tabularity.Tabular,
                 new Parameter("OutputSchema", ScalarTypes.Type),
                 new Parameter("Script", ScalarTypes.String),
                 new Parameter("ScriptParameters", ScalarTypes.Dynamic, minOccurring: 0));

        public static readonly FunctionSymbol RollingPercentile =
             new FunctionSymbol("rolling_percentile",
                 (table, args, signature) =>
                 {
                     var cols = new List<ColumnSymbol>();
                     AddReferencedColumn(cols, args, signature, "IndexColumn");
                     AddReferencedColumns(cols, args, signature, "Dimension");
                     var binsPerWindow = GetArgument(args, signature, "BinsPerWindow")?.LiteralValue?.ToString() ?? "0";
                     var percentile = GetArgument(args, signature, "Percentile")?.LiteralValue?.ToString() ?? "0";
                     var valueColumn = GetArgument(args, signature, "ValueColumn")?.ReferencedSymbol as ColumnSymbol;
                     cols.Add(new ColumnSymbol($"rolling_{binsPerWindow}_percentile_{valueColumn?.Name ?? "value"}_{percentile}", valueColumn?.Type ?? ScalarTypes.Long));
                     return new TableSymbol(cols);
                 },
                 Tabularity.Tabular,
                 new Parameter("ValueColumn", ParameterTypeKind.Summable, ArgumentKind.Column),
                 new Parameter("Percentile", ParameterTypeKind.Number, ArgumentKind.Constant),
                 new Parameter("IndexColumn", ParameterTypeKind.Summable, ArgumentKind.Column),
                 new Parameter("BinSize", ParameterTypeKind.Summable),
                 new Parameter("BinsPerWindow", ParameterTypeKind.Integer, ArgumentKind.Constant),
                 new Parameter("Dimension", ParameterTypeKind.NotDynamic, ArgumentKind.Column, minOccurring: 0, maxOccurring: MaxRepeat)
                 );

        public static readonly FunctionSymbol RowsNear =
             new FunctionSymbol("rows_near",
                (table, args) => table,
                Tabularity.Tabular,
                new Parameter("Condition", ScalarTypes.Bool, ArgumentKind.Expression),
                new Parameter("NumRows", ParameterTypeKind.Integer, ArgumentKind.Constant),
                new Parameter("NumRowsAfter", ParameterTypeKind.Integer, ArgumentKind.Constant, minOccurring:0));

        public static readonly FunctionSymbol SessionCount =
             new FunctionSymbol("session_count",
                 (table, args, signature) =>
                 {
                     var cols = new List<ColumnSymbol>();
                     AddReferencedColumn(cols, args, signature, "TimelineColumn");
                     AddReferencedColumns(cols, args, signature, "Dimension");
                     cols.Add(new ColumnSymbol("count_sessions", ScalarTypes.Long));
                     return new TableSymbol(cols);
                 },
                 Tabularity.Tabular,
                 new Parameter("IdColumn", ParameterTypeKind.NotDynamic, ArgumentKind.Column),
                 new Parameter("TimelineColumn", ParameterTypeKind.Summable, ArgumentKind.Column),
                 new Parameter("Start", ParameterTypeKind.Summable, ArgumentKind.Constant),
                 new Parameter("End", ParameterTypeKind.Summable, ArgumentKind.Constant),
                 new Parameter("Bin", ParameterTypeKind.Summable, ArgumentKind.Constant),
                 new Parameter("LookBackWindow", ParameterTypeKind.Summable, ArgumentKind.Constant),
                 new Parameter("Dimension", ParameterTypeKind.NotDynamic, ArgumentKind.Column, minOccurring: 0, maxOccurring: MaxRepeat));


        private static readonly Parameter SD_TimelineColumn = new Parameter("TimelineColumn", ParameterTypeKind.Summable, ArgumentKind.Column);
        private static readonly Parameter SD_MaxSequenceStepWindows = new Parameter("MaxSeqeunceStepWindow", ParameterTypeKind.Summable, ArgumentKind.Constant);
        private static readonly Parameter SD_MaxSequenceSpan = new Parameter("MaxSequenceSpan", ParameterTypeKind.Summable, ArgumentKind.Constant);
        private static readonly Parameter SD_Expr = new Parameter("Expr", ScalarTypes.Bool, ArgumentKind.Expression, minOccurring: 1, maxOccurring: MaxRepeat);
        private static readonly Parameter SD_Dimension = new Parameter("Dimension", ParameterTypeKind.NotDynamic, ArgumentKind.Column, minOccurring: 0, maxOccurring: MaxRepeat);

        public static readonly FunctionSymbol SequenceDetect =
             new FunctionSymbol("sequence_detect",
                 new Signature(
                     (table, args, signature) =>
                     {
                         var cols = new List<ColumnSymbol>();

                         AddReferencedColumns(cols, args, signature, SD_Dimension.Name);

                         var timelineArg = GetArgument(args, signature, SD_TimelineColumn.Name);
                         if (timelineArg != null)
                         {
                             var timelineArgName = GetExpressionResultName(timelineArg);

                             cols.AddRange(GetArguments(args, signature, SD_Expr.Name).Select(a =>
                                new ColumnSymbol(MakeColumnName(GetExpressionResultName(a), timelineArgName), timelineArg.ResultType)));
                         }

                         cols.Add(new ColumnSymbol("Duration", ScalarTypes.TimeSpan));
                         return new TableSymbol(cols);
                     },
                     Tabularity.Tabular,
                     SD_TimelineColumn,
                     SD_MaxSequenceStepWindows,
                     SD_MaxSequenceSpan,
                     SD_Expr,
                     SD_Dimension)
                 .WithLayout((sig, args, list) =>
                 {
                     list.Add(SD_TimelineColumn);
                     list.Add(SD_MaxSequenceStepWindows);
                     list.Add(SD_MaxSequenceSpan);
                     list.Add(SD_Expr); // first expr required

                     int i = list.Count;

                     // any following bool args are also expr args
                     for (;  i < args.Count && args[i].ResultType == ScalarTypes.Bool; i++)
                     {
                         list.Add(SD_Expr);
                     }

                     // all remaining args are dimensions
                     for (;  i < args.Count; i++)
                     {
                         list.Add(SD_Dimension);
                     }
                 }));

        public static readonly FunctionSymbol SlidingWindowCounts =
             new FunctionSymbol("sliding_window_counts",
                 (table, args, signature) =>
                 {
                     var cols = new List<ColumnSymbol>();
                     AddReferencedColumn(cols, args, signature, "TimelineColumn");
                     AddReferencedColumns(cols, args, signature, "Dimension");
                     cols.Add(new ColumnSymbol("Count", ScalarTypes.Long));
                     cols.Add(new ColumnSymbol("Dcount", ScalarTypes.Long));
                     return new TableSymbol(cols);
                 },
                 Tabularity.Tabular,
                 new Parameter("IdColumn", ParameterTypeKind.NotDynamic, ArgumentKind.Column),
                 new Parameter("TimelineColumn", ParameterTypeKind.Summable, ArgumentKind.Column),
                 new Parameter("Start", ParameterTypeKind.Summable, ArgumentKind.Constant),
                 new Parameter("End", ParameterTypeKind.Summable, ArgumentKind.Constant),
                 new Parameter("LookBackWindow", ParameterTypeKind.Summable, ArgumentKind.Constant),
                 new Parameter("Step", ParameterTypeKind.Summable, ArgumentKind.Constant),
                 new Parameter("Dimension", ParameterTypeKind.NotDynamic, ArgumentKind.Column, minOccurring: 0, maxOccurring: MaxRepeat));

        public static readonly FunctionSymbol SqlRequest =
             new FunctionSymbol("sql_request",
                 (table, args) => new TableSymbol().WithIsOpen(true), // the schema comes from the database at runtime
                 Tabularity.Tabular,
                 new Parameter("ConnectionString", ScalarTypes.String),
                 new Parameter("SqlQuery", ScalarTypes.String));

        public static readonly FunctionSymbol MySqlRequest =
             new FunctionSymbol("mysql_request",
                 (table, args) => new TableSymbol().WithIsOpen(true), // the schema comes from the database at runtime
                 Tabularity.Tabular,
                 new Parameter("ConnectionString", ScalarTypes.String),
                 new Parameter("SqlQuery", ScalarTypes.String));

        public static IReadOnlyList<FunctionSymbol> All { get; } = new FunctionSymbol[]
        {
            ActiveUseCounts,
            ActivityCountsMetrics,
            ActivityEngagement,
            ActivityMetrics,
            AzureDigitalTwinsQueryRequest,
            AutoCluster,
            BagUnpack,
            Basket,
            CosmosdbSqlRequest,
            DCountIntersect,
            DiffPatterns,
            EstimateRowsCount,
            ExecuteShowCommand,
            ExecuteQuery,
            ExternalDatatable,
            // FunnelAnalysis,
            FunnelSequence,
            FunnelSequenceCompletion,
            HttpRequest,
            HttpRequestPost,
            Identity,
            IdentityV3,
            InferStorageSchema,
            Narrow,
            NewActivityMetrics,
            Pivot,
            Preview,
            Python,
            R,
            RollingPercentile,
            RowsNear,
            SessionCount,
            SequenceDetect,
            SlidingWindowCounts,
            SqlRequest,
            MySqlRequest,
        };

        private static Dictionary<string, FunctionSymbol> s_nameToPlugInMap;

        /// <summary>
        /// Gets the plug-in function given the name, or null if no plug-in is defined with the specified name.
        /// </summary>
        public static FunctionSymbol GetPlugIn(string name)
        {
            if (s_nameToPlugInMap == null)
            {
                s_nameToPlugInMap = All.ToDictionary(f => f.Name);
            }

            s_nameToPlugInMap.TryGetValue(name, out var fn);
            return fn;
        }
    }
}
