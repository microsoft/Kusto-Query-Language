using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language
{
    using Symbols;
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
                    AddReferencedColumn(cols, signature, "TimelineColumn", args); // timeline
                    AddReferencedColumns(cols, signature, "Dimension", args); // dimensions
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
                    AddReferencedColumn(cols, signature, "TimelineColumn", args); // timeline column
                    AddReferencedColumns(cols, signature, "Dimension", args); // dimension columns
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
                        AddReferencedColumn(cols, signature, "TimelineColumn", args); // timeline column
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
                        AddReferencedColumn(cols, signature, "TimelineColumn", args); // timeline column
                        AddReferencedColumns(cols, signature, "Dimension", args); // dimension columns
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
                        AddReferencedColumn(cols, signature, "TimelineColumn", args); // timeline columns
                        AddReferencedColumns(cols, signature, "Dimension", args); // dimension columns
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
                        AddReferencedColumn(cols, signature, "TimelineColumn", args); // timeline columns
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

        public static readonly IReadOnlyList<ColumnSymbol> AutoClusterColumns = new[] {
            new ColumnSymbol("SegmentId", ScalarTypes.Long),
            new ColumnSymbol("Count", ScalarTypes.Long),
            new ColumnSymbol("Percent", ScalarTypes.Real)
        };

        public static readonly FunctionSymbol AutoCluster =
            new FunctionSymbol("autocluster",
                (table, args) => table.WithColumns(AutoClusterColumns.Concat(table.Columns)),
                Tabularity.Tabular,
                new Parameter("SizeWeight", ScalarTypes.Real, defaultValueIndicator: "~", minOccurring: 0),
                new Parameter("WeightColumn", ParameterTypeKind.Scalar, ArgumentKind.Column, defaultValueIndicator: "~", minOccurring: 0),
                new Parameter("NumSeeds", ParameterTypeKind.Integer, defaultValueIndicator: "~", minOccurring: 0),
                new Parameter("CustomWildcard", ParameterTypeKind.Scalar, minOccurring: 0, maxOccurring: MaxRepeat));

        public static readonly FunctionSymbol BagUnpack =
             new FunctionSymbol("bag_unpack",
                 (table, args) => table.WithColumns(table.Columns.Where(c => args.Count == 0 || c != args[0].ReferencedSymbol)).Open(),
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
                 (table, args) => table.WithColumns(BasketColumns.Concat(table.Columns)),
                 Tabularity.Tabular,
                 new Parameter("Threshold", ScalarTypes.Real, defaultValueIndicator: "~", minOccurring: 0),
                 new Parameter("WeightColumn", ParameterTypeKind.Scalar, ArgumentKind.Column, defaultValueIndicator: "~", minOccurring: 0),
                 new Parameter("MaxDimensions", ParameterTypeKind.Integer, defaultValueIndicator: "~", minOccurring: 0),
                 new Parameter("CustomWildcard", ParameterTypeKind.Scalar, minOccurring: 0, maxOccurring: MaxRepeat));

        public static readonly FunctionSymbol CosmosdbSqlRequest =
             new FunctionSymbol("cosmosdb_sql_request",
                 (table, args) => new TableSymbol().Open(), // the schema comes from the cosmos database at runtime
                 Tabularity.Tabular,
                 new Parameter("endpoint", ScalarTypes.String),
                 new Parameter("authorization_string", ScalarTypes.String),
                 new Parameter("database_name", ScalarTypes.String),
                 new Parameter("collection_name", ScalarTypes.String),
                 new Parameter("sql_query", ScalarTypes.String)
                 );

        public static readonly FunctionSymbol DCountIntersect =
             new FunctionSymbol("dcount_intersect",
                 (table, args) => table.AddColumns(args.Select((a, i) => new ColumnSymbol("s" + i, ScalarTypes.Long))),
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
                 (table, args) => table.WithColumns(DiffPatternsColumns.Concat(table.Columns)),
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
                 (table, args) => new TableSymbol().Open(), // depends on contents of command string
                 Tabularity.Tabular,
                 new Parameter("command", ScalarTypes.String));

        public static readonly FunctionSymbol ExternalDatatable =
             new FunctionSymbol("external_datatable",
                 new Signature(
                     (table, args) => new TableSymbol().Open(), // depends on the data sent from the client
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
                    AddReferencedColumn(cols, signature, "TimelineColumn", args);
                    AddReferencedColumn(cols, signature, "StateColumn", args, "prev");
                    AddReferencedColumn(cols, signature, "StateColumn", args, "next");
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
                    AddReferencedColumn(cols, signature, "TimelineColumn", args);
                    AddReferencedColumn(cols, signature, "StateColumn", args, type: ScalarTypes.String);
                    cols.Add(new ColumnSymbol("Period", ScalarTypes.TimeSpan));
                    cols.Add(new ColumnSymbol("dcount", ScalarTypes.Long));
                    return new TableSymbol(cols);
                },
                Tabularity.Tabular,
                new Parameter("IdColumn", ParameterTypeKind.NotDynamic, ArgumentKind.Column),
                new Parameter("TimelineColumn", ParameterTypeKind.Summable, ArgumentKind.Column),
                new Parameter("Start", ParameterTypeKind.Summable, ArgumentKind.Constant),
                new Parameter("End", ParameterTypeKind.Summable, ArgumentKind.Constant),
                new Parameter("Step", ParameterTypeKind.Summable, ArgumentKind.Constant),
                new Parameter("StateColumn", ParameterTypeKind.NotDynamic, ArgumentKind.Column),
                new Parameter("Sequence", ScalarTypes.Dynamic, ArgumentKind.Constant),
                new Parameter("MaxSequenceWindows", ScalarTypes.Dynamic, ArgumentKind.Constant));

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

        public static readonly IReadOnlyList<ColumnSymbol> NarrowColumns = new[]
        {
            new ColumnSymbol("Row", ScalarTypes.Long),
            new ColumnSymbol("Column", ScalarTypes.String),
            new ColumnSymbol("Value", ScalarTypes.String)
        };

        public static readonly FunctionSymbol Narrow =
            new FunctionSymbol("narrow",
                 new TableSymbol(NarrowColumns));

#if false // problem with optional parameters and repeatable parameters
            new FunctionSymbol("new_activity_metrics",
                 new Signature(
                     (table, args, signature) =>
                     {
                         var cols = new List<ColumnSymbol>();
                         var timelineColumnParameter = signature.GetParameter("TimelineColumn");
                         var timelineColumnArg = signature.GetArgumentIndex(timelineColumnParameter, args);
                         return table;
                     },
                     Tabularity.Tabular,
                     new Parameter("IdColumn", ParameterTypeKind.NotDynamic, ArgumentKind.Column),
                     new Parameter("TimelineColumn", ParameterTypeKind.Summable, ArgumentKind.Column),
                     new Parameter("Start", ParameterTypeKind.Summable, ArgumentKind.Constant),
                     new Parameter("End", ParameterTypeKind.Summable, ArgumentKind.Constant),
                     new Parameter("Window", ParameterTypeKind.Scalar),
                     new Parameter("Cohort", ParameterTypeKind.Scalar, ArgumentKind.Constant),
                     new Parameter("Dimension", ParameterTypeKind.NotDynamic, ArgumentKind.Column, minOccurring: 0, maxOccurring: MaxRepeat),
                     new Parameter("lookback", ParameterTypeKind.Tabular, minOccurring: 0)),
                 new Signature(
                     (table, args, signature) => table,
                     Tabularity.Tabular,
                     new Parameter("IdColumn", ParameterTypeKind.NotDynamic, ArgumentKind.Column),
                     new Parameter("TimelineColumn", ParameterTypeKind.Summable, ArgumentKind.Column),
                     new Parameter("Start", ParameterTypeKind.Summable, ArgumentKind.Constant),
                     new Parameter("End", ParameterTypeKind.Summable, ArgumentKind.Constant),
                     new Parameter("Window", ParameterTypeKind.Scalar),
                     new Parameter("Dimension", ParameterTypeKind.NotDynamic, ArgumentKind.Column, minOccurring: 0, maxOccurring: MaxRepeat),
                     new Parameter("lookback", ParameterTypeKind.Tabular, minOccurring: 0))
                 ),
#endif

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
                     return new TableSymbol(columns).Open();
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
                 (table, args) => new TableSymbol().Open(), // TODO: can we parse the output schema argument?
                 Tabularity.Tabular,
                 new Parameter("OutputSchema", ScalarTypes.Type),
                 new Parameter("Script", ScalarTypes.String),
                 new Parameter("ScriptParameters", ScalarTypes.Dynamic, minOccurring: 0));

        public static readonly FunctionSymbol Python =
             new FunctionSymbol("python",
                 (table, args) => new TableSymbol().Open(), // TODO: can we parse the output schema argument?
                 Tabularity.Tabular,
                 new Parameter("OutputSchema", ScalarTypes.Type),
                 new Parameter("Script", ScalarTypes.String),
                 new Parameter("ScriptParameters", ScalarTypes.Dynamic, minOccurring: 0));

        public static readonly FunctionSymbol R =
             new FunctionSymbol("r",
                 (table, args) => new TableSymbol().Open(), // TODO: can we parse the output schema argument?
                 Tabularity.Tabular,
                 new Parameter("OutputSchema", ScalarTypes.Type),
                 new Parameter("Script", ScalarTypes.String),
                 new Parameter("ScriptParameters", ScalarTypes.Dynamic, minOccurring: 0));

        public static readonly FunctionSymbol RollingPercentile =
             new FunctionSymbol("rolling_percentile",
                 (table, args, signature) =>
                 {
                     var cols = new List<ColumnSymbol>();
                     AddReferencedColumn(cols, signature, "IndexColumn", args);
                     AddReferencedColumns(cols, signature, "Dimension", args);
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

        public static readonly FunctionSymbol SessionCount =
             new FunctionSymbol("session_count",
                 (table, args, signature) =>
                 {
                     var cols = new List<ColumnSymbol>();
                     AddReferencedColumn(cols, signature, "TimelineColumn", args);
                     AddReferencedColumns(cols, signature, "Dimension", args);
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

        public static readonly FunctionSymbol SlidingWindowCounts =
             new FunctionSymbol("sliding_window_counts",
                 (table, args, signature) =>
                 {
                     var cols = new List<ColumnSymbol>();
                     AddReferencedColumn(cols, signature, "TimelineColumn", args);
                     AddReferencedColumns(cols, signature, "Dimension", args);
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
                 (table, args) => new TableSymbol().Open(), // the schema comes from the database at runtime
                 Tabularity.Tabular,
                 new Parameter("ConnectionString", ScalarTypes.String),
                 new Parameter("SqlQuery", ScalarTypes.String));


        public static IReadOnlyList<FunctionSymbol> All { get; } = new FunctionSymbol[]
        {
            ActiveUseCounts,
            ActivityCountsMetrics,
            ActivityEngagement,
            ActivityMetrics,
            AutoCluster,
            BagUnpack,
            Basket,
            CosmosdbSqlRequest,
            DCountIntersect,
            DiffPatterns,
            EstimateRowsCount,
            ExecuteShowCommand,
            ExternalDatatable,
            // FunnelAnalysis,
            FunnelSequence,
            FunnelSequenceCompletion,
            HttpRequest,
            HttpRequestPost,
            Identity,
            IdentityV3,
            Narrow,
            //NewActivityMetrics,
            Pivot,
            Preview,
            Python,
            R,
            RollingPercentile,
            SessionCount,
            SlidingWindowCounts,
            SqlRequest
        };
    }
}