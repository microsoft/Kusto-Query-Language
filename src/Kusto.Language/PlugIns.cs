using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language
{
    using Symbols;
    using Syntax;
    using System;
    using static FunctionHelpers;

    /// <summary>
    /// Well known plugins.
    /// </summary>
    public static class PlugIns
    {
        public static readonly FunctionSymbol ActiveUseCounts =
            new FunctionSymbol("active_users_count",
                context =>
                {
                    var cols = new List<ColumnSymbol>();
                    AddReferencedColumn(cols, context, "TimelineColumn"); // timeline
                    AddReferencedColumns(cols, context, "Dimension"); // dimensions
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
                context =>
                {
                    var cols = new List<ColumnSymbol>();
                    AddReferencedColumn(cols, context, "TimelineColumn"); // timeline column
                    AddReferencedColumns(cols, context, "Dimension"); // dimension columns
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
                    context =>
                    {
                        var cols = new List<ColumnSymbol>();
                        AddReferencedColumn(cols, context, "TimelineColumn"); // timeline column
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
                    context =>
                    {
                        var cols = new List<ColumnSymbol>();
                        AddReferencedColumn(cols, context, "TimelineColumn"); // timeline column
                        AddReferencedColumns(cols, context, "Dimension"); // dimension columns
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
                    context =>
                    {
                        var cols = new List<ColumnSymbol>();
                        AddReferencedColumn(cols, context, "TimelineColumn"); // timeline columns
                        AddReferencedColumns(cols, context, "Dimension"); // dimension columns
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
                    context =>
                    {
                        var cols = new List<ColumnSymbol>();
                        AddReferencedColumn(cols, context, "TimelineColumn"); // timeline columns
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
                     context =>
                     {
                         var cols = new List<ColumnSymbol>();

                         var timelineArg = context.GetArgument("TimelineColumn"); // timeline column
                         if (timelineArg != null)
                         {
                             var timelineArgName = context.GetResultName(timelineArg);
                             cols.Add(new ColumnSymbol(MakeColumnName("from", timelineArgName), timelineArg.ResultType));
                             cols.Add(new ColumnSymbol(MakeColumnName("to", timelineArgName), timelineArg.ResultType));
                         }

                         AddReferencedColumns(cols, context, "Dimension"); // dimension columns

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
                    for (; i < args.Count && !(args[i].ResultType is TableSymbol); i++)
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
                context => new TableSymbol(AutoClusterColumns.Concat(context.RowScope.Columns))
                               .WithInheritableProperties(context.RowScope),
                Tabularity.Tabular,
                new Parameter("SizeWeight", ScalarTypes.Real, defaultValueIndicator: "~", minOccurring: 0),
                new Parameter("WeightColumn", ParameterTypeKind.Scalar, ArgumentKind.Column, defaultValueIndicator: "~", minOccurring: 0),
                new Parameter("NumSeeds", ParameterTypeKind.Integer, defaultValueIndicator: "~", minOccurring: 0),
                new Parameter("CustomWildcard", ParameterTypeKind.Scalar, minOccurring: 0, maxOccurring: MaxRepeat));

        public static readonly FunctionSymbol BagUnpack =
             new FunctionSymbol("bag_unpack",
                 context => new TableSymbol(context.RowScope.Columns.Where(c => context.Arguments.Count == 0 || c != context.Arguments[0].ReferencedSymbol))
                                .WithInheritableProperties(context.RowScope)
                                .WithIsOpen(true),
                 Tabularity.Tabular,
                 new Parameter("column", ParameterTypeKind.Scalar, ArgumentKind.Column),
                 new Parameter("column_prefix", ScalarTypes.String, ArgumentKind.LiteralNotEmpty, minOccurring: 0));

        public static readonly IReadOnlyList<ColumnSymbol> BasketColumns = new[] {
            new ColumnSymbol("SegmentId", ScalarTypes.Long),
            new ColumnSymbol("Count", ScalarTypes.Long),
            new ColumnSymbol("Percent", ScalarTypes.Real)
        };

        public static readonly FunctionSymbol Basket =
             new FunctionSymbol("basket",
                 context => new TableSymbol(BasketColumns.Concat(context.RowScope.Columns))
                                .WithInheritableProperties(context.RowScope),
                 Tabularity.Tabular,
                 new Parameter("Threshold", ScalarTypes.Real, defaultValueIndicator: "~", minOccurring: 0),
                 new Parameter("WeightColumn", ParameterTypeKind.Scalar, ArgumentKind.Column, defaultValueIndicator: "~", minOccurring: 0),
                 new Parameter("MaxDimensions", ParameterTypeKind.Integer, defaultValueIndicator: "~", minOccurring: 0),
                 new Parameter("CustomWildcard", ParameterTypeKind.Scalar, minOccurring: 0, maxOccurring: MaxRepeat));

        public static readonly FunctionSymbol DCountIntersect =
             new FunctionSymbol("dcount_intersect",
                 context => new TableSymbol(context.RowScope.Columns.Concat(context.Arguments.Select((a, i) => new ColumnSymbol("s" + i, ScalarTypes.Long))))
                                .WithInheritableProperties(context.RowScope),
                 Tabularity.Tabular,
                 new Parameter("hll", ParameterTypeKind.DynamicArray, minOccurring: 2, maxOccurring: MaxRepeat));

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
                 context => new TableSymbol(DiffPatternsColumns.Concat(context.RowScope.Columns))
                                .WithInheritableProperties(context.RowScope),
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
                    context => new TableSymbol(new ColumnSymbol("EstimatedRowsCount", ScalarTypes.Long)),
                    Tabularity.Tabular));

        public static readonly FunctionSymbol ExecuteShowCommand =
             new FunctionSymbol("execute_show_command",
                 context => new TableSymbol().WithIsOpen(true), // depends on contents of command string
                 Tabularity.Tabular,
                 new Parameter("connection_string", ScalarTypes.String),
                 new Parameter("command", ScalarTypes.String));

        public static readonly FunctionSymbol ExecuteQuery =
             new FunctionSymbol("execute_query",
                 context => new TableSymbol().WithIsOpen(true), // depends on contents of command string
                 Tabularity.Tabular,
                 new Parameter("connection_string", ScalarTypes.String),
                 new Parameter("query", ScalarTypes.String));

        public static readonly FunctionSymbol ExternalDatatable =
             new FunctionSymbol("external_datatable",
                 new Signature(
                    context => new TableSymbol().WithIsOpen(true), // depends on the data sent from the client
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
                context =>
                {
                    // only declare first table, as additional schema is not useful to intellisense
                    var cols = new List<ColumnSymbol>();
                    AddReferencedColumn(cols, context, "TimelineColumn");

                    var stateArg = context.GetArgument("StateColumn");
                    if (stateArg != null)
                    {
                        cols.Add(new ColumnSymbol("prev", stateArg.ResultType, source: stateArg));
                        cols.Add(new ColumnSymbol("next", stateArg.ResultType, source: stateArg));
                    }

                    cols.Add(new ColumnSymbol("dcount", ScalarTypes.Long));
                    cols.Add(new ColumnSymbol("samples", ScalarTypes.DynamicArray));
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
                new Parameter("Sequence", ParameterTypeKind.DynamicArray, ArgumentKind.Constant));

        public static readonly FunctionSymbol FunnelSequenceCompletion =
            new FunctionSymbol("funnel_sequence_completion",
                context =>
                {
                    var cols = new List<ColumnSymbol>();
                    AddReferencedColumn(cols, context, "TimelineColumn");

                    var stateArg = context.GetArgument("StateColumn");
                    if (stateArg != null)
                    {
                        cols.Add(new ColumnSymbol(context.GetResultName(stateArg), ScalarTypes.String, source: stateArg));
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
                new Parameter("Sequence", ParameterTypeKind.DynamicArray, ArgumentKind.Constant),
                new Parameter("MaxSequencePeriods", ParameterTypeKind.DynamicArray, ArgumentKind.Constant));

        public static readonly IReadOnlyList<ColumnSymbol> HttpRequestColumns = new[] {
            new ColumnSymbol("ResponseHeaders", ScalarTypes.DynamicBag),
            new ColumnSymbol("ResponseBody", ScalarTypes.Dynamic)
        };

        public static readonly FunctionSymbol HttpRequest =
             new FunctionSymbol("http_request",
                 new TableSymbol(HttpRequestColumns),
                 new Parameter("Uri", ScalarTypes.String, ArgumentKind.Constant),
                 new Parameter("RequestHeaders", ParameterTypeKind.DynamicBag, minOccurring: 0),
                 new Parameter("Options", ParameterTypeKind.DynamicBag, minOccurring: 0));

        public static readonly FunctionSymbol HttpRequestPost =
             new FunctionSymbol("http_request_post",
                 new TableSymbol(HttpRequestColumns),
                 new Parameter("Uri", ScalarTypes.String, ArgumentKind.Constant),
                 new Parameter("RequestHeaders", ParameterTypeKind.DynamicBag, minOccurring: 0),
                 new Parameter("Options", ParameterTypeKind.DynamicBag, minOccurring: 0),
                 new Parameter("Content", ScalarTypes.String, ArgumentKind.Constant, minOccurring: 0));

        public static readonly FunctionSymbol AIEmbedText_Deprecated =
             new FunctionSymbol("ai_embed_text",
                 context =>
                 {
                     var sourceColumns = context.RowScope.Columns;
                     var columnPrefix = context.GetResultName(context.GetArgument("Text"));

                     var embeddingColumnName = MakeColumnName(columnPrefix, "embedding");
                     var addedColumns = new List<ColumnSymbol> { new ColumnSymbol(embeddingColumnName, ScalarTypes.Dynamic) };

                     if (context.GetArgument("IncludeErrorMessages") != null && 
                        bool.TryParse(GetConstantValue(context.GetArgument("IncludeErrorMessages")), out var includeErrorMessages))
                     {
                         if (includeErrorMessages)
                         {
                             var errorColumnName = MakeColumnName(columnPrefix, "embedding", "error");
                             addedColumns.Add(new ColumnSymbol(errorColumnName, ScalarTypes.String));
                         }
                     }

                     var resultColumns = sourceColumns.Concat(addedColumns);

                     return new TableSymbol(resultColumns);
                 },
                 Tabularity.Tabular,
                 new Parameter("Text", ParameterTypeKind.Scalar, ArgumentKind.Column | ArgumentKind.Literal),
                 new Parameter("ConnectionString", ScalarTypes.String),
                 new Parameter("Options", ParameterTypeKind.DynamicBag, minOccurring: 0),
                 new Parameter("IncludeErrorMessages", ScalarTypes.Bool, minOccurring: 0))
            .Obsolete("ai_embeddings");

        public static readonly FunctionSymbol AIEmbeddings =
            new FunctionSymbol("ai_embeddings",
                context =>
                {
                    var sourceColumns = context.RowScope.Columns;
                    var columnPrefix = context.GetResultName(context.GetArgument("Text"));

                    var embeddingColumnName = MakeColumnName(columnPrefix, "embeddings");
                    var addedColumns = new List<ColumnSymbol> { new ColumnSymbol(embeddingColumnName, ScalarTypes.Dynamic) };

                    if (context.GetArgument("IncludeErrorMessages") != null &&
                       bool.TryParse(GetConstantValue(context.GetArgument("IncludeErrorMessages")), out var includeErrorMessages))
                    {
                        if (includeErrorMessages)
                        {
                            var errorColumnName = MakeColumnName(columnPrefix, "embeddings", "error");
                            addedColumns.Add(new ColumnSymbol(errorColumnName, ScalarTypes.String));
                        }
                    }

                    var resultColumns = sourceColumns.Concat(addedColumns);

                    return new TableSymbol(resultColumns);
                },
                Tabularity.Tabular,
                new Parameter("Text", ParameterTypeKind.Scalar, ArgumentKind.Column | ArgumentKind.Literal),
                new Parameter("ConnectionString", ScalarTypes.String),
                new Parameter("Options", ParameterTypeKind.DynamicBag, minOccurring: 0),
                new Parameter("IncludeErrorMessages", ScalarTypes.Bool, minOccurring: 0));


        public static readonly FunctionSymbol AIChatCompletion =
             new FunctionSymbol("ai_chat_completion",
                 context =>
                 {
                     var sourceColumns = context.RowScope.Columns;
                     var columnPrefix = context.GetResultName(context.GetArgument("Prompt"));

                     var completionColumnName = MakeColumnName(columnPrefix, "completion");
                     var addedColumns = new List<ColumnSymbol> { new ColumnSymbol(completionColumnName, ScalarTypes.String) };

                     if (context.GetArgument("IncludeErrorMessages") != null &&
                        bool.TryParse(GetConstantValue(context.GetArgument("IncludeErrorMessages")), out var includeErrorMessages))
                     {
                         if (includeErrorMessages)
                         {
                             var errorColumnName = MakeColumnName(columnPrefix, "completion", "error");
                             addedColumns.Add(new ColumnSymbol(errorColumnName, ScalarTypes.String));
                         }
                     }

                     var resultColumns = sourceColumns.Concat(addedColumns);

                     return new TableSymbol(resultColumns);
                 },
                 Tabularity.Tabular,
                 new Parameter("Prompt", ParameterTypeKind.DynamicArray, ArgumentKind.Column | ArgumentKind.Literal),
                 new Parameter("ConnectionString", ScalarTypes.String),
                 new Parameter("Options", ParameterTypeKind.DynamicBag, minOccurring: 0),
                 new Parameter("IncludeErrorMessages", ScalarTypes.Bool, minOccurring: 0));

        public static readonly FunctionSymbol AIChatCompletionPrompt =
             new FunctionSymbol("ai_chat_completion_prompt",
                 context =>
                 {
                     var sourceColumns = context.RowScope.Columns;
                     var columnPrefix = context.GetResultName(context.GetArgument("Prompt"));

                     var completionColumnName = MakeColumnName(columnPrefix, "completion");
                     var addedColumns = new List<ColumnSymbol> { new ColumnSymbol(completionColumnName, ScalarTypes.String) };

                     if (context.GetArgument("IncludeErrorMessages") != null &&
                        bool.TryParse(GetConstantValue(context.GetArgument("IncludeErrorMessages")), out var includeErrorMessages))
                     {
                         if (includeErrorMessages)
                         {
                             var errorColumnName = MakeColumnName(columnPrefix, "completion", "error");
                             addedColumns.Add(new ColumnSymbol(errorColumnName, ScalarTypes.String));
                         }
                     }

                     var resultColumns = sourceColumns.Concat(addedColumns);

                     return new TableSymbol(resultColumns);
                 },
                 Tabularity.Tabular,
                 new Parameter("Prompt", ScalarTypes.String, ArgumentKind.Column | ArgumentKind.Literal),
                 new Parameter("ConnectionString", ScalarTypes.String),
                 new Parameter("Options", ParameterTypeKind.DynamicBag, minOccurring: 0),
                 new Parameter("IncludeErrorMessages", ScalarTypes.Bool, minOccurring: 0));


        public static readonly FunctionSymbol Identity =
             new FunctionSymbol("identity",
                 new Signature(
                     context => context.RowScope,
                     Tabularity.Tabular));

        public static readonly FunctionSymbol IdentityV3 =
             new FunctionSymbol("identity_v3",
                context => context.RowScope,
                Tabularity.Tabular,
                new Parameter("mode", ScalarTypes.String, ArgumentKind.Constant),
                new Parameter("exceptionText", ScalarTypes.String, ArgumentKind.Constant));

        public static readonly FunctionSymbol InferStorageSchema =
             new FunctionSymbol("infer_storage_schema",
                 new TableSymbol(new ColumnSymbol("CslSchema", ScalarTypes.String)),
                 new Parameter("Options", ParameterTypeKind.DynamicBag));

        public static readonly FunctionSymbol InferStorageSchemaWithSuggestions =
             new FunctionSymbol("infer_storage_schema_with_suggestions",
                new TableSymbol(new ColumnSymbol("SuggestedTableSchema", ScalarTypes.String)),
                new Parameter("Options", ParameterTypeKind.DynamicBag));

        private static readonly Parameter Geo_lookup_LookupTable = new Parameter("LookupTable", ParameterTypeKind.Tabular);

        private static readonly Parameter Geo_lookup_LookupPolygonKey = new Parameter("LookupPolygonKey", ParameterTypeKind.DynamicBag, ArgumentKind.Column_Parameter0);
        private static readonly Parameter Geo_lookup_LookupLineKey = new Parameter("LookupLineKey", ParameterTypeKind.DynamicBag, ArgumentKind.Column_Parameter0);

        private static readonly Parameter Geo_lookup_SourceLongitudeKey = new Parameter("SourceLongitude", ParameterTypeKind.Scalar, ArgumentKind.Column);
        private static readonly Parameter Geo_lookup_SourceLatitudeKey = new Parameter("SourceLatitude", ParameterTypeKind.Scalar, ArgumentKind.Column);
        private static readonly Parameter Geo_lookup_PolygonRadius = new Parameter("radius", ScalarTypes.Real, ArgumentKind.Literal, minOccurring: 0); // Can be either Literal or Column_Parameter0
        private static readonly Parameter Geo_lookup_LineRadius = new Parameter("radius", ScalarTypes.Real, ArgumentKind.Literal); // Can be either Literal or Column_Parameter0

        private static readonly Parameter Geo_lookup_return_unmatched = new Parameter("return_unmatched", ScalarTypes.Bool, ArgumentKind.Literal, minOccurring: 0);
        private static readonly Parameter Geo_lookup_area_radius = new Parameter("lookup_area_radius", ScalarTypes.Real, ArgumentKind.Literal, minOccurring: 0);
        private static readonly Parameter Geo_lookup_return_key = new Parameter("return_lookup_key", ScalarTypes.Bool, ArgumentKind.Literal, minOccurring: 0);

        public static readonly FunctionSymbol Geo_Polygon_Lookup = new FunctionSymbol(
            "geo_polygon_lookup",
            new Signature(
                context =>
                {
                    var lookupTable = context.GetArgument(Geo_lookup_LookupTable.Name)?.ResultType as TableSymbol;
                    if (lookupTable != null)
                    {
                        var cols = new List<ColumnSymbol>();
                        cols.AddRange(context.RowScope.Columns);

                        if (IsGeoLookupShouldReturnLookupKey(context, Geo_lookup_LookupPolygonKey.Name))
                        {
                            cols.AddRange(lookupTable.Columns);
                        }
                        else
                        {
                            // Remove return_lookup_key
                            var lookupkeyName = context.GetArgument(Geo_lookup_LookupPolygonKey.Name).ReferencedSymbol.Name;
                            cols.AddRange(lookupTable.Columns.Where(c => !StringComparer.OrdinalIgnoreCase.Equals(c.Name, lookupkeyName)));
                        }

                        var combinedColumns = ColumnSymbol.Combine(CombineKind.UniqueNames, cols);
                        return new TableSymbol(combinedColumns);
                    }
                    else
                    {
                        // lookup table unknown, so default to input table
                        return context.RowScope;
                    }
                },
                Tabularity.Tabular,
                Geo_lookup_LookupTable,
                Geo_lookup_LookupPolygonKey,
                Geo_lookup_SourceLongitudeKey,
                Geo_lookup_SourceLatitudeKey,
                Geo_lookup_PolygonRadius,
                Geo_lookup_return_unmatched,
                Geo_lookup_area_radius,
                Geo_lookup_return_key)
                    .WithLayout((signature, args, parameters) =>
                    {
                        parameters.Add(Geo_lookup_LookupTable);
                        parameters.Add(Geo_lookup_LookupPolygonKey);
                        parameters.Add(Geo_lookup_SourceLongitudeKey);
                        parameters.Add(Geo_lookup_SourceLatitudeKey);

                        for (int i = 4; i < args.Count; i++)
                        {
                            if (args[i] is SimpleNamedExpression sne)
                            {
                                switch (sne.Name.SimpleName.ToLower())
                                {
                                    case "radius":
                                        parameters.Add(Geo_lookup_PolygonRadius);
                                        continue;
                                    case "return_unmatched":
                                        parameters.Add(Geo_lookup_return_unmatched);
                                        continue;
                                    case "lookup_area_radius":
                                        parameters.Add(Geo_lookup_area_radius);
                                        continue;
                                    case "return_lookup_key":
                                        parameters.Add(Geo_lookup_return_key);
                                        continue;
                                }
                            }
                            else
                            {
                                switch (i)
                                {
                                    case 4:
                                        parameters.Add(Geo_lookup_PolygonRadius);
                                        continue;
                                    case 5:
                                        parameters.Add(Geo_lookup_return_unmatched);
                                        continue;
                                    case 6:
                                        parameters.Add(Geo_lookup_area_radius);
                                        continue;
                                    case 7:
                                        parameters.Add(Geo_lookup_return_key);
                                        continue;
                                }
                            }
                        }
                    }));

        public static readonly FunctionSymbol Geo_Line_Lookup = new FunctionSymbol(
            "geo_line_lookup",
            new Signature(
                context =>
                {
                    var lookupTable = context.GetArgument(Geo_lookup_LookupTable.Name)?.ResultType as TableSymbol;
                    if (lookupTable != null)
                    {
                        var cols = new List<ColumnSymbol>();
                        cols.AddRange(context.RowScope.Columns);

                        if (IsGeoLookupShouldReturnLookupKey(context, Geo_lookup_LookupLineKey.Name))
                        {
                            cols.AddRange(lookupTable.Columns);
                        }
                        else
                        {
                            // Remove return_lookup_key
                            var lookupkeyName = context.GetArgument(Geo_lookup_LookupLineKey.Name).ReferencedSymbol.Name;
                            cols.AddRange(lookupTable.Columns.Where(c => !StringComparer.OrdinalIgnoreCase.Equals(c.Name, lookupkeyName)));
                        }

                        var combinedColumns = ColumnSymbol.Combine(CombineKind.UniqueNames, cols);
                        return new TableSymbol(combinedColumns);
                    }
                    else
                    {
                        // lookup table unknown, so default to input table
                        return context.RowScope;
                    }
                },
                Tabularity.Tabular,
                Geo_lookup_LookupTable,
                Geo_lookup_LookupLineKey,
                Geo_lookup_SourceLongitudeKey,
                Geo_lookup_SourceLatitudeKey,
                Geo_lookup_LineRadius,
                Geo_lookup_return_unmatched,
                Geo_lookup_area_radius,
                Geo_lookup_return_key)
                    .WithLayout((signature, args, parameters) =>
                    {
                        parameters.Add(Geo_lookup_LookupTable);
                        parameters.Add(Geo_lookup_LookupLineKey);
                        parameters.Add(Geo_lookup_SourceLongitudeKey);
                        parameters.Add(Geo_lookup_SourceLatitudeKey);
                        parameters.Add(Geo_lookup_LineRadius);

                        for (int i = 5; i < args.Count; i++)
                        {
                            if (args[i] is SimpleNamedExpression sne)
                            {
                                switch (sne.Name.SimpleName.ToLower())
                                {
                                    case "return_unmatched":
                                        parameters.Add(Geo_lookup_return_unmatched);
                                        continue;
                                    case "lookup_area_radius":
                                        parameters.Add(Geo_lookup_area_radius);
                                        continue;
                                    case "return_lookup_key":
                                        parameters.Add(Geo_lookup_return_key);
                                        continue;
                                }
                            }
                            else
                            {
                                switch (i)
                                {
                                    case 5:
                                        parameters.Add(Geo_lookup_return_unmatched);
                                        continue;
                                    case 6:
                                        parameters.Add(Geo_lookup_area_radius);
                                        continue;
                                    case 7:
                                        parameters.Add(Geo_lookup_return_key);
                                        continue;
                                }
                            }
                        }
                    }));

        private static bool IsGeoLookupShouldReturnLookupKey(CustomReturnTypeContext context, string lookupKeyName)
        {
            return
                // Lookup key isn't known yet
                context.GetArgument(lookupKeyName)?.ReferencedSymbol == null
                || (context.Arguments != null &&
                    // Boolean value of 'return_lookup_key' is set to true
                    ((context.Arguments.Count == 8 && context.Arguments[7].Kind == SyntaxKind.BooleanLiteralExpression && context.Arguments[7].ConstantValue != null && (bool)context.Arguments[7].ConstantValue)
                    // Named expression value of 'return_lookup_key' is set to true
                    || context.Arguments.Any(arg => arg is SimpleNamedExpression sne
                        && StringComparer.OrdinalIgnoreCase.Equals(sne.Name.SimpleName, Geo_lookup_return_key.Name)
                        && sne.Expression.Kind == SyntaxKind.BooleanLiteralExpression
                        && sne.Expression.ConstantValue != null
                        && (bool)sne.Expression.ConstantValue)));
        }

        private static readonly Parameter Ipv4_lookup_LookupTable = new Parameter("LookupTable", ParameterTypeKind.Tabular);
        private static readonly Parameter Ipv4_lookup_SourceIPv4Key = new Parameter("SourceIPv4Key", ParameterTypeKind.Scalar, ArgumentKind.Column);
        private static readonly Parameter Ipv4_lookup_IPv4LookupKey = new Parameter("IPv4LookupKey", ParameterTypeKind.Scalar, ArgumentKind.Column_Parameter0);
        private static readonly Parameter IPv4_lookup_ExtraKey = new Parameter("ExtraKey", ParameterTypeKind.Scalar, ArgumentKind.Column_Parameter0_Common, minOccurring: 0, maxOccurring: MaxRepeat);
        private static readonly Parameter IPv4_lookup_return_unmatched = new Parameter("return_unmatched", ScalarTypes.Bool, ArgumentKind.Literal, minOccurring: 0);

        public static readonly FunctionSymbol Ipv4_Lookup =
            new FunctionSymbol("ipv4_lookup",
                new Signature(
                    context => {
                        var lookupTable = context.GetArgument(Ipv4_lookup_LookupTable.Name)?.ResultType as TableSymbol;
                        if (lookupTable != null)
                        {
                            var keyColumns = context.GetArguments(IPv4_lookup_ExtraKey.Name).Select(e => e.ReferencedSymbol as ColumnSymbol).Where(c => c != null).ToList();
                            var cols = new List<ColumnSymbol>();
                            // add all left side columns
                            cols.AddRange(context.RowScope.Columns);
                            // add all right side columns except those used as join keys from both tables
                            cols.AddRange(lookupTable.Columns.Where(c => !keyColumns.Any(kc => kc.Name == c.Name)));
                            // make final set of columns have unique names
                            var combinedColumns = ColumnSymbol.Combine(CombineKind.UniqueNames, cols);
                            return new TableSymbol(combinedColumns);
                        }
                        else
                        {
                            // lookup table unknown, so default to input table
                            return context.RowScope;
                        }
                    },
                    Tabularity.Tabular,
                    Ipv4_lookup_LookupTable,
                    Ipv4_lookup_SourceIPv4Key,
                    Ipv4_lookup_IPv4LookupKey,
                    IPv4_lookup_ExtraKey,
                    IPv4_lookup_return_unmatched)
                    .WithLayout((signature, args, parameters) =>
                    {
                        parameters.Add(Ipv4_lookup_LookupTable);
                        parameters.Add(Ipv4_lookup_SourceIPv4Key);
                        parameters.Add(Ipv4_lookup_IPv4LookupKey);

                        for (int i = 3; i < args.Count; i++)
                        {
                            if (i == args.Count - 1
                                && ((args[i] is SimpleNamedExpression sne
                                     && sne.Name.SimpleName == IPv4_lookup_return_unmatched.Name)
                                    || (args[i] is LiteralExpression lit && lit.Kind == SyntaxKind.BooleanLiteralExpression)))
                            {
                                parameters.Add(IPv4_lookup_return_unmatched);
                            }
                            else
                            {
                                parameters.Add(IPv4_lookup_ExtraKey);
                            }
                        }
                    }));

        private static readonly Parameter Ipv6_lookup_LookupTable = new Parameter("LookupTable", ParameterTypeKind.Tabular);
        private static readonly Parameter Ipv6_lookup_SourceIPv6Key = new Parameter("SourceIPv6Key", ParameterTypeKind.Scalar, ArgumentKind.Column);
        private static readonly Parameter Ipv6_lookup_IPv6LookupKey = new Parameter("IPv6LookupKey", ParameterTypeKind.Scalar, ArgumentKind.Column_Parameter0);
        private static readonly Parameter IPv6_lookup_return_unmatched = new Parameter("return_unmatched", ScalarTypes.Bool, ArgumentKind.Literal, minOccurring: 0);

        public static readonly FunctionSymbol Ipv6_Lookup =
            new FunctionSymbol("ipv6_lookup",
                new Signature(
                    context => {
                        var lookupTable = context.GetArgument(Ipv6_lookup_LookupTable.Name)?.ResultType as TableSymbol;
                        if (lookupTable != null)
                        {
                            var cols = new List<ColumnSymbol>();
                            cols.AddRange(context.RowScope.Columns);
                            cols.AddRange(lookupTable.Columns);
                            var combinedColumns = ColumnSymbol.Combine(CombineKind.UniqueNames, cols);
                            return new TableSymbol(combinedColumns);
                        }
                        else
                        {
                            // lookup table unknown, so default to input table
                            return context.RowScope;
                        }
                    },
                    Tabularity.Tabular,
                    Ipv6_lookup_LookupTable,
                    Ipv6_lookup_SourceIPv6Key,
                    Ipv6_lookup_IPv6LookupKey,
                    IPv6_lookup_return_unmatched)
                    .WithLayout((signature, args, parameters) =>
                    {
                        parameters.Add(Ipv6_lookup_LookupTable);
                        parameters.Add(Ipv6_lookup_SourceIPv6Key);
                        parameters.Add(Ipv6_lookup_IPv6LookupKey);
                        parameters.Add(IPv6_lookup_return_unmatched);
                    }));

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
                 context =>
                 {
                     var pivotColumn = context.Arguments.Count > 0 ? context.Arguments[0].ReferencedSymbol as ColumnSymbol : null;

                     var aggregateColumn =
                        context.Arguments.Count > 1
                        && context.Arguments[1] is Syntax.FunctionCallExpression fc
                        && fc.ArgumentList.Expressions.Count > 0
                            ? fc.ArgumentList.Expressions[0].Element.ReferencedSymbol as ColumnSymbol
                            : null;

                     // columns specified
                     var columns = context.Arguments.Skip(2).Select(a => a.ReferencedSymbol as ColumnSymbol).Where(c => c != null).ToList();

                     if (columns.Count == 0)
                     {
                         // all columns exept explicity mentioned pivot and aggregate column
                         columns.AddRange(context.RowScope.Columns.Where(c => c != pivotColumn && c != aggregateColumn));
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
                 context => new GroupSymbol( // multiple result tables
                     context.RowScope,
                     new TableSymbol(new ColumnSymbol("Count", ScalarTypes.Long))),
                 Tabularity.Tabular,
                 new Parameter("NumberOfRows", ParameterTypeKind.Integer));

        private static TableSymbol GetOutputSchema(CustomReturnTypeContext context)
        {
            if (context.Arguments.Count > 0 && context.Arguments[0].ReferencedSymbol is TableSymbol schema)
            {
                return schema;
            }
            else
            {
                return new TableSymbol().WithIsOpen(true);
            }
        }

        public static readonly FunctionSymbol CSharp =
             new FunctionSymbol("csharp",
                 GetOutputSchema,
                 Tabularity.Tabular,
                 new Parameter("OutputSchema", ScalarTypes.Type),
                 new Parameter("Script", ScalarTypes.String),
                 new Parameter("Arguments", ParameterTypeKind.DynamicBag, minOccurring: 0));

        public static readonly FunctionSymbol Python =
             new FunctionSymbol("python",
                 GetOutputSchema,
                 Tabularity.Tabular,
                 new Parameter("OutputSchema", ScalarTypes.Type),
                 new Parameter("Script", ScalarTypes.String),
                 new Parameter("Arguments", ParameterTypeKind.DynamicBag, minOccurring: 0),
                 new Parameter("Artifacts", ParameterTypeKind.DynamicBag, minOccurring: 0));

        public static readonly FunctionSymbol R =
             new FunctionSymbol("r",
                 GetOutputSchema,
                 Tabularity.Tabular,
                 new Parameter("OutputSchema", ScalarTypes.Type),
                 new Parameter("Script", ScalarTypes.String),
                 new Parameter("Arguments", ParameterTypeKind.DynamicBag, minOccurring: 0));

        public static readonly FunctionSymbol RollingPercentile =
             new FunctionSymbol("rolling_percentile",
                 context =>
                 {
                     var cols = new List<ColumnSymbol>();
                     AddReferencedColumn(cols, context, "IndexColumn");
                     AddReferencedColumns(cols, context, "Dimension");
                     var binsPerWindow = context.GetArgument("BinsPerWindow")?.LiteralValue?.ToString() ?? "0";
                     var percentile = context.GetArgument("Percentile")?.LiteralValue?.ToString() ?? "0";
                     var valueColumn = context.GetArgument("ValueColumn")?.ReferencedSymbol as ColumnSymbol;
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
                context => context.RowScope,
                Tabularity.Tabular,
                new Parameter("Condition", ScalarTypes.Bool, ArgumentKind.Expression),
                new Parameter("NumRows", ParameterTypeKind.Integer, ArgumentKind.Constant),
                new Parameter("NumRowsAfter", ParameterTypeKind.Integer, ArgumentKind.Constant, minOccurring: 0));

        public static readonly FunctionSymbol SessionCount =
             new FunctionSymbol("session_count",
                 context =>
                 {
                     var cols = new List<ColumnSymbol>();
                     AddReferencedColumn(cols, context, "TimelineColumn");
                     AddReferencedColumns(cols, context, "Dimension");
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
                     context =>
                     {
                         var cols = new List<ColumnSymbol>();

                         AddReferencedColumns(cols, context, SD_Dimension.Name);

                         var timelineArg = context.GetArgument(SD_TimelineColumn.Name);
                         if (timelineArg != null)
                         {
                             var timelineArgName = context.GetResultName(timelineArg);

                             cols.AddRange(context.GetArguments(SD_Expr.Name).Select(a =>
                                new ColumnSymbol(MakeColumnName(context.GetResultName(a), timelineArgName), timelineArg.ResultType)));
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
                     for (; i < args.Count && IsBoolean(args[i]); i++)
                     {
                         list.Add(SD_Expr);
                     }

                     // all remaining args are dimensions
                     for (; i < args.Count; i++)
                     {
                         list.Add(SD_Dimension);
                     }
                 }));

        public static readonly FunctionSymbol SlidingWindowCounts =
             new FunctionSymbol("sliding_window_counts",
                 context =>
                 {
                     var cols = new List<ColumnSymbol>();
                     AddReferencedColumn(cols, context, "TimelineColumn");
                     AddReferencedColumns(cols, context, "Dimension");
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
                 context => new TableSymbol().WithIsOpen(true), // the schema comes from the database at runtime
                 Tabularity.Tabular,
                 new Parameter("connection_string", ScalarTypes.String),
                 new Parameter("sql_query", ScalarTypes.String),
                 new Parameter("sql_parameters", ParameterTypeKind.DynamicBag, minOccurring: 0),
                 new Parameter("options", ParameterTypeKind.DynamicBag, minOccurring: 0));

        public static readonly FunctionSymbol MySqlRequest =
             new FunctionSymbol("mysql_request",
                 context => new TableSymbol().WithIsOpen(true), // the schema comes from the database at runtime
                 Tabularity.Tabular,
                 new Parameter("connection_string", ScalarTypes.String),
                 new Parameter("sql_query", ScalarTypes.String),
                 new Parameter("sql_parameters", ParameterTypeKind.DynamicBag, minOccurring: 0),
                 new Parameter("options", ParameterTypeKind.DynamicBag, minOccurring: 0));

        public static readonly FunctionSymbol PostgreSqlRequest =
           new FunctionSymbol("postgresql_request",
               context => new TableSymbol().WithIsOpen(true), // the schema comes from the database at runtime
               Tabularity.Tabular,
               new Parameter("connection_string", ScalarTypes.String),
               new Parameter("sql_query", ScalarTypes.String),
               new Parameter("sql_parameters", ParameterTypeKind.DynamicBag, minOccurring: 0),
               new Parameter("options", ParameterTypeKind.DynamicBag, minOccurring: 0))
            .Hide(); // Open once service rollout completes

        public static readonly FunctionSymbol CosmosdbSqlRequest =
             new FunctionSymbol("cosmosdb_sql_request",
                 context => new TableSymbol().WithIsOpen(true), // the schema comes from the cosmos database at runtime
                 Tabularity.Tabular,
                 new Parameter("connection_string", ScalarTypes.String),
                 new Parameter("sql_query", ScalarTypes.String),
                 new Parameter("sql_parameters", ParameterTypeKind.DynamicBag, minOccurring: 0),
                 new Parameter("options", ParameterTypeKind.DynamicBag, minOccurring: 0)
                 );

        public static readonly FunctionSymbol AzureDigitalTwinsQueryRequest =
                     new FunctionSymbol("azure_digital_twins_query_request",
                         context => new TableSymbol().WithIsOpen(true), // depends on the SELECT command provided
                         Tabularity.Tabular,
                         new Parameter("endpoint", ScalarTypes.String),
                         new Parameter("sql_query", ScalarTypes.String)
                         );

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
            CSharp,
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
            InferStorageSchemaWithSuggestions,
            Geo_Polygon_Lookup,
            Geo_Line_Lookup,
            Ipv4_Lookup,
            Ipv6_Lookup,
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
            PostgreSqlRequest,
            AIEmbedText_Deprecated,
            AIChatCompletion,
            AIChatCompletionPrompt,
            AIEmbeddings,
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