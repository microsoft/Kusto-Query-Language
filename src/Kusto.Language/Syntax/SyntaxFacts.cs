using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Syntax
{
    using Symbols;
    using Utils;

    /// <summary>
    /// Facts about <see cref="SyntaxKind"/>
    /// </summary>
    public static class SyntaxFacts
    {
        private static SyntaxData[] kindToDataMap;
        private static TextKeyedDictionary<SyntaxKind> textToKindMap;

        private class SyntaxData
        {
            public SyntaxKind Kind { get; }

            public string Text { get; }

            public SyntaxCategory Category { get; }

            public OperatorKind OperatorKind { get; }

            public bool CanBeIdentifier { get; }

            public bool IsType { get; }

            public SyntaxData(SyntaxKind kind, string text, SyntaxCategory category = SyntaxCategory.Keyword, OperatorKind opKind = OperatorKind.None, bool canBeIdentifier = false, bool isType = false)
            {
                this.Kind = kind;
                this.Text = text;
                this.Category = category;
                this.OperatorKind = opKind;
                this.CanBeIdentifier = canBeIdentifier;
                this.IsType = isType;
            }
        }

        static SyntaxFacts()
        {
            var data = new List<SyntaxData>() {
                new SyntaxData(SyntaxKind.None, "", SyntaxCategory.None ),

                new SyntaxData(SyntaxKind.__CrossClusterKeyword, "__crossCluster"),
                new SyntaxData(SyntaxKind.__CrossDBKeyword, "__crossDB"),
                new SyntaxData(SyntaxKind.__IdKeyword, "__id"),
                new SyntaxData(SyntaxKind.__IsFuzzyKeyword, "__isFuzzy"),
                new SyntaxData(SyntaxKind.__NoWithSourceKeyword, "__noWithSource"),
                new SyntaxData(SyntaxKind.__PackedColumnKeyword, "__packedColumn"),
                new SyntaxData(SyntaxKind.__SourceColumnIndexKeyword, "__sourceColumnIndex"),
                new SyntaxData(SyntaxKind._3DChartKeyword, "3Dchart"),

                new SyntaxData(SyntaxKind.AccessKeyword, "access", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.AccumulateKeyword, "accumulate"),
                new SyntaxData(SyntaxKind.AliasKeyword, "alias", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.AndKeyword, "and", opKind: OperatorKind.And),
                new SyntaxData(SyntaxKind.AnomalyChartKeyword, "anomalychart"),
                new SyntaxData(SyntaxKind.AreaChartKeyword, "areachart"),
                new SyntaxData(SyntaxKind.AsKeyword, "as"),
                new SyntaxData(SyntaxKind.AscKeyword, "asc"),
                new SyntaxData(SyntaxKind.AssertSchemaKeyword, "assert-schema"),

                new SyntaxData(SyntaxKind.BagExpansionKeyword, "bagexpansion"),
                new SyntaxData(SyntaxKind.BarChartKeyword, "barchart"),
                new SyntaxData(SyntaxKind.BetweenKeyword, "between", opKind: OperatorKind.Between),
                new SyntaxData(SyntaxKind.BinKeyword, "bin", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.BinLegacy, "bin_legacy"),
                new SyntaxData(SyntaxKind.ByKeyword, "by"),

                new SyntaxData(SyntaxKind.CachingPolicyKeyword, "cachingpolicy"),
                new SyntaxData(SyntaxKind.CalloutKeyword, "callout"),
                new SyntaxData(SyntaxKind.CancelKeyword, "cancel"),
                new SyntaxData(SyntaxKind.CardKeyword, "card"),
                new SyntaxData(SyntaxKind.ColumnChartKeyword, "columnchart"),
                new SyntaxData(SyntaxKind.CommandsAndQueriesKeyword, "commands-and-queries"),
                new SyntaxData(SyntaxKind.ConsumeKeyword, "consume", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ContainsKeyword, "contains", opKind: OperatorKind.Contains),
                new SyntaxData(SyntaxKind.ContainsCsKeyword, "containscs", opKind: OperatorKind.ContainsCs),
                new SyntaxData(SyntaxKind.Contains_CsKeyword, "contains_cs", opKind: OperatorKind.ContainsCs),
                new SyntaxData(SyntaxKind.ContextualDataTableKeyword, "__contextual_datatable"),
                new SyntaxData(SyntaxKind.CountKeyword, "count", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.CyclesKeyword, "cycles"),

                new SyntaxData(SyntaxKind.DatabaseKeyword, "database", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.DataExportKeyword, "dataexport"),
                new SyntaxData(SyntaxKind.DataScopeKeyword, "datascope"),
                new SyntaxData(SyntaxKind.DataTableKeyword, "datatable"),
                new SyntaxData(SyntaxKind.DeclareKeyword, "declare" , canBeIdentifier: true),
                new SyntaxData(SyntaxKind.DecodeBlocksKeyword, "decodeblocks"),
                new SyntaxData(SyntaxKind.DefaultKeyword, "default", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.DescKeyword, "desc"),
                new SyntaxData(SyntaxKind.DistinctKeyword, "distinct", canBeIdentifier: true),

                new SyntaxData(SyntaxKind.GraphEdgesKeyword, "edges", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.EarliestKeyword, "earliest"),
                new SyntaxData(SyntaxKind.EndsWithKeyword, "endswith", opKind: OperatorKind.EndsWith),
                new SyntaxData(SyntaxKind.EndsWithCsKeyword, "endswith_cs", opKind: OperatorKind.EndsWithCs),
                new SyntaxData(SyntaxKind.EncodingPolicyKeyword, "encodingpolicy"),
                new SyntaxData(SyntaxKind.EntityGroupKeyword, "entity_group", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.EvaluateKeyword, "evaluate", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ExecuteAndCacheKeyword, "__executeAndCache"),
                new SyntaxData(SyntaxKind.ExpandOutputKeyword, "expandoutput"),
                new SyntaxData(SyntaxKind.ExtendKeyword, "extend", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ExtentTagsRetentionKeyword, "extent_tags_retention"),
                new SyntaxData(SyntaxKind.ExternalDataKeyword, "externaldata"),
                new SyntaxData(SyntaxKind.External_DataKeyword, "external_data"),

                // Inline External Table keywords
                new SyntaxData(SyntaxKind.DataFormatKeyword, "dataformat", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.InlineExternalTableKeyword, "inline_external_table", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.DateTimePatternKeyword, "datetime_pattern", canBeIdentifier: true),

                // End Inline External Table keywords

                new SyntaxData(SyntaxKind.FacetKeyword, "facet", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.FilterKeyword, "filter", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.FindKeyword, "find"),
                new SyntaxData(SyntaxKind.FirstKeyword, "first"),
                new SyntaxData(SyntaxKind.FlagsKeyword, "flags"),
                new SyntaxData(SyntaxKind.ForkKeyword, "fork", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.FromKeyword, "from", canBeIdentifier: true),

                new SyntaxData(SyntaxKind.GetSchemaKeyword, "getschema", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.GrannyAscKeyword, "granny-asc"),
                new SyntaxData(SyntaxKind.GrannyDescKeyword, "granny-desc"),
                new SyntaxData(SyntaxKind.GraphMatchKeyword, "graph-match"),
                new SyntaxData(SyntaxKind.GraphShortestPathsKeyword, "graph-shortest-paths"),
                new SyntaxData(SyntaxKind.GraphToTableKeyword, "graph-to-table"),

                new SyntaxData(SyntaxKind.HardDeleteKeyword, "harddelete"),
                new SyntaxData(SyntaxKind.HardRetentionKeyword, "hardretention"),
                new SyntaxData(SyntaxKind.HasAnyKeyword, "has_any", opKind: OperatorKind.HasAny),
                new SyntaxData(SyntaxKind.HasAllKeyword, "has_all", opKind: OperatorKind.HasAll),
                new SyntaxData(SyntaxKind.HasKeyword, "has", opKind: OperatorKind.Has),
                new SyntaxData(SyntaxKind.HasCsKeyword, "has_cs", opKind: OperatorKind.HasCs),
                new SyntaxData(SyntaxKind.HasPrefixKeyword, "hasprefix", opKind: OperatorKind.HasPrefix),
                new SyntaxData(SyntaxKind.HasPrefixCsKeyword, "hasprefix_cs", opKind: OperatorKind.HasPrefixCs),
                new SyntaxData(SyntaxKind.HasSuffixKeyword, "hassuffix", opKind: OperatorKind.HasSuffix),
                new SyntaxData(SyntaxKind.HasSuffixCsKeyword, "hassuffix_cs", opKind: OperatorKind.HasSuffixCs),
                new SyntaxData(SyntaxKind.HintDotConcurrencyKeyword,"hint.concurrency"),
                new SyntaxData(SyntaxKind.HintDotDistributionKeyword,"hint.distribution"),
                new SyntaxData(SyntaxKind.HintDotMaterializedKeyword,"hint.materialized"),
                new SyntaxData(SyntaxKind.HintDotNumPartitions,"hint.num_partitions"),
                new SyntaxData(SyntaxKind.HintDotProgressiveTopKeyword,"hint.progressive_top"),
                new SyntaxData(SyntaxKind.HintDotShuffleKeyKeyword,"hint.shufflekey"),
                new SyntaxData(SyntaxKind.HintDotSpreadKeyword,"hint.spread"),
                new SyntaxData(SyntaxKind.HintDotRemoteKeyword,"hint.remote"),
                new SyntaxData(SyntaxKind.HintDotStrategyKeyword,"hint.strategy"),
                new SyntaxData(SyntaxKind.HotCacheKeyword, "hotcache"),

                new SyntaxData(SyntaxKind.IdKeyword, "id", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.InKeyword, "in", opKind: OperatorKind.In),
                new SyntaxData(SyntaxKind.InCsKeyword, "in~", opKind: OperatorKind.InCs),
                new SyntaxData(SyntaxKind.InvokeKeyword, "invoke"),
                new SyntaxData(SyntaxKind.IsFuzzyKeyword, "isfuzzy"),
                new SyntaxData(SyntaxKind.BestEffortKeyword, "best_effort", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ForceRemoteKeyword, "force_remote"),

                new SyntaxData(SyntaxKind.JoinKeyword, "join", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.JournalKeyword, "journal"),

                new SyntaxData(SyntaxKind.KindKeyword, "kind"),

                new SyntaxData(SyntaxKind.LadderChartKeyword, "ladderchart"),
                new SyntaxData(SyntaxKind.LastKeyword, "last"),
                new SyntaxData(SyntaxKind.LatestKeyword, "latest"),
                new SyntaxData(SyntaxKind.LetKeyword, "let", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.LikeKeyword, "like", opKind: OperatorKind.Like),
                new SyntaxData(SyntaxKind.LikeCsKeyword, "likecs", opKind: OperatorKind.LikeCs),
                new SyntaxData(SyntaxKind.LimitKeyword, "limit", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.LineChartKeyword, "linechart"),
                new SyntaxData(SyntaxKind.LookupKeyword, "lookup", canBeIdentifier: true),

                new SyntaxData(SyntaxKind.MacroExpandKeyword, "macro-expand"),
                new SyntaxData(SyntaxKind.MakeGraphKeyword, "make-graph"),
                new SyntaxData(SyntaxKind.GraphMarkComponentsKeyword, "graph-mark-components"),
                new SyntaxData(SyntaxKind.GraphWhereNodesKeyword, "graph-where-nodes"),
                new SyntaxData(SyntaxKind.GraphWhereEdgesKeyword, "graph-where-edges"),
                new SyntaxData(SyntaxKind.MakeSeriesKeyword, "make-series"),
                new SyntaxData(SyntaxKind.MatchesRegexKeyword, "matches regex", opKind: OperatorKind.MatchRegex),
                new SyntaxData(SyntaxKind.MaterializeKeyword, "materialize"),
                new SyntaxData(SyntaxKind.MaterializedViewCombineKeyword, "materialized-view-combine"),
                new SyntaxData(SyntaxKind.MaterializedViewsKeyword, "materialized-views"),
                new SyntaxData(SyntaxKind.MdmKeyword, "mdm"),
                new SyntaxData(SyntaxKind.MissingKeyword, "missing"),
                new SyntaxData(SyntaxKind.MvDashApplyKeyword, "mv-apply"),
                new SyntaxData(SyntaxKind.MvApplyKeyword, "mvapply", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.MvDashExpandKeyword, "mv-expand"),
                new SyntaxData(SyntaxKind.MvExpandKeyword, "mvexpand", canBeIdentifier: true),

                new SyntaxData(SyntaxKind.NodesKeyword, "nodes", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.NoOptimizationKeyword, "nooptimization"),
                new SyntaxData(SyntaxKind.NotBetweenKeyword, "!between", opKind: OperatorKind.NotBetween),
                new SyntaxData(SyntaxKind.NotContainsKeyword, "notcontains", opKind: OperatorKind.NotContains),
                new SyntaxData(SyntaxKind.NotContainsCsKeyword, "notcontainscs", opKind: OperatorKind.NotContainsCs),
                new SyntaxData(SyntaxKind.NotBangContainsKeyword, "!contains", opKind: OperatorKind.NotContains),
                new SyntaxData(SyntaxKind.NotBangContainsCsKeyword, "!contains_cs", opKind: OperatorKind.NotContainsCs),
                new SyntaxData(SyntaxKind.NotEndsWithKeyword, "!endswith", opKind: OperatorKind.NotEndsWith),
                new SyntaxData(SyntaxKind.NotEndsWithCsKeyword, "!endswith_cs", opKind: OperatorKind.NotEndsWithCs),
                new SyntaxData(SyntaxKind.NotHasKeyword, "!has", opKind: OperatorKind.NotHas),
                new SyntaxData(SyntaxKind.NotHasCsKeyword, "!has_cs", opKind: OperatorKind.NotHasCs),
                new SyntaxData(SyntaxKind.NotHasPrefixKeyword, "!hasprefix", opKind: OperatorKind.NotHasPrefix),
                new SyntaxData(SyntaxKind.NotHasPrefixCsKeyword, "!hasprefix_cs", opKind: OperatorKind.NotHasPrefixCs),
                new SyntaxData(SyntaxKind.NotHasSuffixKeyword, "!hassuffix", opKind: OperatorKind.NotHasSuffix),
                new SyntaxData(SyntaxKind.NotHasSuffixCsKeyword, "!hassuffix_cs", opKind: OperatorKind.NotHasSuffixCs),
                new SyntaxData(SyntaxKind.NotInKeyword, "!in", opKind: OperatorKind.NotIn),
                new SyntaxData(SyntaxKind.NotInCsKeyword, "!in~", opKind: OperatorKind.NotInCs),
                new SyntaxData(SyntaxKind.NotLikeKeyword, "notlike", opKind: OperatorKind.NotLike),
                new SyntaxData(SyntaxKind.NotLikeCsKeyword, "notlikecs", opKind: OperatorKind.NotLikeCs),
                new SyntaxData(SyntaxKind.NotStartsWithKeyword, "!startswith", opKind: OperatorKind.NotStartsWith),
                new SyntaxData(SyntaxKind.NotStartsWithCsKeyword, "!startswith_cs", opKind: OperatorKind.NotStartsWithCs),
                new SyntaxData(SyntaxKind.NullKeyword, "null", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.NullsKeyword, "nulls", canBeIdentifier: true),

                new SyntaxData(SyntaxKind.OfKeyword, "of"),
                new SyntaxData(SyntaxKind.OnKeyword, "on", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.OptionalKeyword, "optional", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.OrKeyword, "or", opKind: OperatorKind.Or),
                new SyntaxData(SyntaxKind.OrderKeyword, "order", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.OthersKeyword, "others"),
                new SyntaxData(SyntaxKind.OutputKeyword, "output", canBeIdentifier: true),

                new SyntaxData(SyntaxKind.PackKeyword, "pack", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ParseKeyword, "parse", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ParseWhereKeyword, "parse-where"),
                new SyntaxData(SyntaxKind.ParseKvKeyword, "parse-kv"),
                new SyntaxData(SyntaxKind.PartitionedByKeyword, "partitioned-by"),
                new SyntaxData(SyntaxKind.PartitionByKeyword, "__partitionby", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.PartitionKeyword, "partition", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.PathFormatKeyword, "pathformat"),
                new SyntaxData(SyntaxKind.PatternKeyword, "pattern", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.PieChartKeyword, "piechart"),
                new SyntaxData(SyntaxKind.PivotChartKeyword, "pivotchart"),
                new SyntaxData(SyntaxKind.PlotlyKeyword, "plotly", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.GraphKeyword, "graph", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.PrintKeyword, "print"),
                new SyntaxData(SyntaxKind.ProjectKeyword, "project"),
                new SyntaxData(SyntaxKind.ProjectAwayKeyword, "project-away"),
                new SyntaxData(SyntaxKind._ProjectAwayKeyword, "__projectAway"),
                new SyntaxData(SyntaxKind.ProjectByNamesKeyword, "project-by-names"),
                new SyntaxData(SyntaxKind.ProjectKeepKeyword, "project-keep"),
                new SyntaxData(SyntaxKind.ProjectRenameKeyword, "project-rename"),
                new SyntaxData(SyntaxKind.ProjectReorderKeyword, "project-reorder"),
                new SyntaxData(SyntaxKind.ProjectSmartKeyword, "project-smart"),

                new SyntaxData(SyntaxKind.QueriesKeyword, "queries"),
                new SyntaxData(SyntaxKind.QueryParametersKeyword, "query_parameters", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.QueryResultsKeyword, "query_results"),

                new SyntaxData(SyntaxKind.RangeKeyword, "range", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ReduceKeyword, "reduce", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.RegexKeyword, "regex", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.RelaxedKeyword, "relaxed"),
                new SyntaxData(SyntaxKind.RenderKeyword, "render", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.RestrictedViewAccessKeyword, "restricted_view_access"),
                new SyntaxData(SyntaxKind.RestrictKeyword, "restrict", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.RowLevelSecurityKeyword, "row_level_security"),
                new SyntaxData(SyntaxKind.RowstoreKeyword, "rowstore"),
                new SyntaxData(SyntaxKind.RowstoreReferencesKeyword, "rowstore_references"),
                new SyntaxData(SyntaxKind.RowstoreSealInfoKeyword, "rowstore_sealinfo"),
                new SyntaxData(SyntaxKind.RowstorePolicyKeyword, "rowstorepolicy"),
                new SyntaxData(SyntaxKind.RowstoresKeyword, "rowstores"),

                new SyntaxData(SyntaxKind.SampleKeyword, "sample"),
                new SyntaxData(SyntaxKind.SampleDistinctKeyword, "sample-distinct"),
                new SyntaxData(SyntaxKind.ScanKeyword, "scan", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ScatterChartKeyword, "scatterchart"),
                new SyntaxData(SyntaxKind.SealKeyword, "seal"),
                new SyntaxData(SyntaxKind.SealsKeyword, "seals"),
                new SyntaxData(SyntaxKind.SearchKeyword, "search"),
                new SyntaxData(SyntaxKind.SerializeKeyword, "serialize", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.SetKeyword, "set"),
                new SyntaxData(SyntaxKind.SetOrAppendKeyword, "set-or-append"),
                new SyntaxData(SyntaxKind.SetOrReplaceKeyword, "set-or-replace"),
                new SyntaxData(SyntaxKind.ShardsKeyword, "shards"),
                new SyntaxData(SyntaxKind.SimpleKeyword, "simple"),
                new SyntaxData(SyntaxKind.SoftDeleteKeyword, "softdelete"),
                new SyntaxData(SyntaxKind.SoftRetentionKeyword, "softretention"),
                new SyntaxData(SyntaxKind.SortKeyword, "sort", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.SqlKeyword, "sql"),
                new SyntaxData(SyntaxKind.StackedAreaChartKeyword, "stackedareachart"),
                new SyntaxData(SyntaxKind.StartsWithKeyword, "startswith", opKind: OperatorKind.StartsWith),
                new SyntaxData(SyntaxKind.StartsWithCsKeyword, "startswith_cs", opKind: OperatorKind.StartsWithCs),
                new SyntaxData(SyntaxKind.StatisticsKeyword, "statistics"),
                new SyntaxData(SyntaxKind.StepKeyword, "step", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.StoredQueryResultContainersKeyword, "storedqueryresultcontainers"),
                new SyntaxData(SyntaxKind.SummarizeKeyword, "summarize", canBeIdentifier: true),

                new SyntaxData(SyntaxKind.TablePurgeKeyword, "tablepurge"),
                new SyntaxData(SyntaxKind.TakeKeyword, "take", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.TimeChartKeyword, "timechart"),
                new SyntaxData(SyntaxKind.TimelineKeyword, "timeline"),
                new SyntaxData(SyntaxKind.TimePivotKeyword, "timepivot"),
                new SyntaxData(SyntaxKind.TitleKeyword, "title"),
                new SyntaxData(SyntaxKind.ToKeyword, "to"),
                new SyntaxData(SyntaxKind.TopKeyword, "top", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.TopHittersKeyword, "top-hitters"),
                new SyntaxData(SyntaxKind.TopNestedKeyword, "top-nested"),
                new SyntaxData(SyntaxKind.ToScalarKeyword, "toscalar"),
                new SyntaxData(SyntaxKind.ToTableKeyword, "totable"),
                new SyntaxData(SyntaxKind.TreeMapKeyword, "treemap"),
                new SyntaxData(SyntaxKind.TypeOfKeyword, "typeof", canBeIdentifier: true),

                new SyntaxData(SyntaxKind.UnionKeyword, "union"),
                new SyntaxData(SyntaxKind.UnrestrictedViewersKeyword, "unrestrictedviewers"),

                new SyntaxData(SyntaxKind.VerboseKeyword, "verbose"),
                new SyntaxData(SyntaxKind.ViewKeyword, "view", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ViewersKeyword, "viewers"),
                new SyntaxData(SyntaxKind.ViewsKeyword, "views"),

                new SyntaxData(SyntaxKind.WhereKeyword, "where"),
                new SyntaxData(SyntaxKind.WithKeyword, "with", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.WithItemIndexKeyword, "with_itemindex"),
                new SyntaxData(SyntaxKind.WithMatchIdKeyword, "with_match_id"),
                new SyntaxData(SyntaxKind.With_SourceKeyword, "with_source"),
                new SyntaxData(SyntaxKind.WithStepNameKeyword, "with_step_name"),
                new SyntaxData(SyntaxKind.WithNodeIdKeyword, "with_node_id", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.WithSourceKeyword, "withsource"),
                new SyntaxData(SyntaxKind.WriteAheadLogKeyword, "writeaheadlog"),

                // type name keywords?
                new SyntaxData(SyntaxKind.BoolKeyword, "bool", canBeIdentifier: true, isType: true),
                new SyntaxData(SyntaxKind.BooleanKeyword, "boolean", isType: true),
                new SyntaxData(SyntaxKind.ByteKeyword, "byte", isType: true),
                new SyntaxData(SyntaxKind.CharKeyword, "char", isType: true),
                new SyntaxData(SyntaxKind.DateKeyword, "date", isType: true),
                new SyntaxData(SyntaxKind.DateTimeKeyword, "datetime", isType: true),
                new SyntaxData(SyntaxKind.DoubleKeyword, "double", isType: true),
                new SyntaxData(SyntaxKind.DynamicKeyword, "dynamic", isType: true),
                new SyntaxData(SyntaxKind.FloatKeyword, "float", isType: true),
                new SyntaxData(SyntaxKind.GuidKeyword, "guid", canBeIdentifier: true, isType: true),
                new SyntaxData(SyntaxKind.IntKeyword, "int", isType: true),
                new SyntaxData(SyntaxKind.Int16Keyword, "int16", isType: true),
                new SyntaxData(SyntaxKind.Int32Keyword, "int32", isType: true),
                new SyntaxData(SyntaxKind.Int64Keyword, "int64", isType: true),
                new SyntaxData(SyntaxKind.Int8Keyword, "int8", isType: true),
                new SyntaxData(SyntaxKind.LongKeyword, "long", isType: true),
                new SyntaxData(SyntaxKind.RealKeyword, "real", isType: true),
                new SyntaxData(SyntaxKind.DecimalKeyword, "decimal", isType: true),
                new SyntaxData(SyntaxKind.StringKeyword, "string", isType: true),
                new SyntaxData(SyntaxKind.TimeKeyword, "time", isType: true),
                new SyntaxData(SyntaxKind.TimespanKeyword, "timespan", isType: true),
                new SyntaxData(SyntaxKind.UIntKeyword, "uint", isType: true),
                new SyntaxData(SyntaxKind.UInt16Keyword, "uint16", isType: true),
                new SyntaxData(SyntaxKind.UInt32Keyword, "uint32", isType: true),
                new SyntaxData(SyntaxKind.UInt64Keyword, "uint64", isType: true),
                new SyntaxData(SyntaxKind.UInt8Keyword, "uint8", isType: true),
                new SyntaxData(SyntaxKind.ULongKeyword, "ulong", isType: true),
                new SyntaxData(SyntaxKind.UniqueIdKeyword, "uniqueid", isType: true),
                new SyntaxData(SyntaxKind.UuidKeyword, "uuid", canBeIdentifier: true, isType: true),

                // punctuation
                new SyntaxData(SyntaxKind.OpenParenToken, "(", SyntaxCategory.Punctuation),
                new SyntaxData(SyntaxKind.CloseParenToken, ")", SyntaxCategory.Punctuation),
                new SyntaxData(SyntaxKind.OpenBracketToken, "[", SyntaxCategory.Punctuation),
                new SyntaxData(SyntaxKind.CloseBracketToken, "]", SyntaxCategory.Punctuation),
                new SyntaxData(SyntaxKind.OpenBraceToken, "{", SyntaxCategory.Punctuation),
                new SyntaxData(SyntaxKind.CloseBraceToken, "}", SyntaxCategory.Punctuation),
                new SyntaxData(SyntaxKind.BarToken, "|", SyntaxCategory.Punctuation),
                new SyntaxData(SyntaxKind.LessThanBarToken, "<|", SyntaxCategory.Punctuation),
                new SyntaxData(SyntaxKind.PlusToken, "+", SyntaxCategory.Operator, OperatorKind.Add),
                new SyntaxData(SyntaxKind.MinusToken, "-", SyntaxCategory.Operator, OperatorKind.Subtract),
                new SyntaxData(SyntaxKind.AsteriskToken, "*", SyntaxCategory.Operator, OperatorKind.Multiply),
                new SyntaxData(SyntaxKind.SlashToken, "/", SyntaxCategory.Operator, OperatorKind.Divide),
                new SyntaxData(SyntaxKind.PercentToken, "%", SyntaxCategory.Operator, OperatorKind.Modulo),
                new SyntaxData(SyntaxKind.DotToken, ".", SyntaxCategory.Punctuation),
                new SyntaxData(SyntaxKind.DotDotToken, "..", SyntaxCategory.Punctuation),
                new SyntaxData(SyntaxKind.BangToken, "!", SyntaxCategory.Punctuation),
                new SyntaxData(SyntaxKind.LessThanToken, "<", SyntaxCategory.Operator, OperatorKind.LessThan),
                new SyntaxData(SyntaxKind.LessThanOrEqualToken, "<=", SyntaxCategory.Operator, OperatorKind.LessThanOrEqual),
                new SyntaxData(SyntaxKind.GreaterThanToken, ">", SyntaxCategory.Operator, OperatorKind.GreaterThan),
                new SyntaxData(SyntaxKind.GreaterThanOrEqualToken, ">=", SyntaxCategory.Operator, OperatorKind.GreaterThanOrEqual),
                new SyntaxData(SyntaxKind.EqualToken, "=", SyntaxCategory.Punctuation),
                new SyntaxData(SyntaxKind.EqualEqualToken, "==", SyntaxCategory.Operator, OperatorKind.Equal),
                new SyntaxData(SyntaxKind.BangEqualToken, "!=", SyntaxCategory.Operator, OperatorKind.NotEqual),
                new SyntaxData(SyntaxKind.LessThanGreaterThanToken, "<>", SyntaxCategory.Operator, OperatorKind.NotEqual),
                new SyntaxData(SyntaxKind.ColonToken, ":", SyntaxCategory.Punctuation),
                new SyntaxData(SyntaxKind.SemicolonToken, ";", SyntaxCategory.Punctuation),
                new SyntaxData(SyntaxKind.CommaToken, ",", SyntaxCategory.Punctuation),
                new SyntaxData(SyntaxKind.EqualTildeToken, "=~", SyntaxCategory.Operator, OperatorKind.EqualTilde),
                new SyntaxData(SyntaxKind.BangTildeToken, "!~", SyntaxCategory.Operator, OperatorKind.BangTilde),
                new SyntaxData(SyntaxKind.AtToken, "@", SyntaxCategory.Punctuation),
                new SyntaxData(SyntaxKind.QuestionToken, "?", SyntaxCategory.Punctuation),
                new SyntaxData(SyntaxKind.FatArrowToken, "=>", SyntaxCategory.Punctuation),

                // literal tokens
                new SyntaxData(SyntaxKind.StringLiteralToken, "", SyntaxCategory.Literal),
                new SyntaxData(SyntaxKind.BooleanLiteralToken, "", SyntaxCategory.Literal),
                new SyntaxData(SyntaxKind.LongLiteralToken, "", SyntaxCategory.Literal),
                new SyntaxData(SyntaxKind.IntLiteralToken, "", SyntaxCategory.Literal),
                new SyntaxData(SyntaxKind.RealLiteralToken, "", SyntaxCategory.Literal),
                new SyntaxData(SyntaxKind.DecimalLiteralToken, "", SyntaxCategory.Literal),
                new SyntaxData(SyntaxKind.DateTimeLiteralToken, "", SyntaxCategory.Literal),
                new SyntaxData(SyntaxKind.TimespanLiteralToken, "", SyntaxCategory.Literal),
                new SyntaxData(SyntaxKind.GuidLiteralToken, "", SyntaxCategory.Literal),
                new SyntaxData(SyntaxKind.RawGuidLiteralToken, "", SyntaxCategory.Literal),

                // identifiers
                new SyntaxData(SyntaxKind.IdentifierToken, "", SyntaxCategory.Identifier),

                // other tokens
                new SyntaxData(SyntaxKind.EndOfTextToken, "", SyntaxCategory.Other),
                new SyntaxData(SyntaxKind.DirectiveToken, "", SyntaxCategory.Other),
                new SyntaxData(SyntaxKind.BadToken, "", SyntaxCategory.Other),

                // pseudo tokens -- not produced by lexer (introduced by parser)
                new SyntaxData(SyntaxKind.DashDashToken, "--", SyntaxCategory.Other),
                new SyntaxData(SyntaxKind.DashDashGreaterThanToken, "-->", SyntaxCategory.Other),
                new SyntaxData(SyntaxKind.LessThanDashDashToken, "<--", SyntaxCategory.Other),
                new SyntaxData(SyntaxKind.BracketDashToken, "]-", SyntaxCategory.Other),
                new SyntaxData(SyntaxKind.BracketDashGreaterThanToken, "]->", SyntaxCategory.Other),
                new SyntaxData(SyntaxKind.DashBracketToken, "-[", SyntaxCategory.Other),
                new SyntaxData(SyntaxKind.LessThanDashBracketToken, "<-[", SyntaxCategory.Other),

                // list
                new SyntaxData(SyntaxKind.List, "", SyntaxCategory.List),

                // nodes
                new SyntaxData(SyntaxKind.SeparatedElement, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ExpressionList, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ExpressionCouple, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.RenameList, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.CustomNode, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.TokenName, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.BracketedName, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.BracedName, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.WildcardedName, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.BracketedWildcardedName, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.NameDeclaration, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.NameReference, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.ParenthesizedExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.PathExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ElementExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.SimpleNamedExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.CompoundNamedExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.FunctionCallExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ToScalarExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ToTableExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.BracketedExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.RangeOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.PipeExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.NamedParameter, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.DataScopeExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.DataTableExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ContextualDataTableExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ExternalDataExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ExternalDataWithClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ExternalDataUriList, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.MaterializedViewCombineExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.MaterializedViewCombineClause, "", SyntaxCategory.Node),

                // Inline External Table nodes
                new SyntaxData(SyntaxKind.InlineExternalTableKindClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.InlineExternalTableDataFormatClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.InlineExternalTablePathFormatClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.InlineExternalTablePartitionClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.InlineExternalTableExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.PartitionColumnDeclaration, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.DateTimePattern, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.InlineExternalTablePathFormatPartitionColumnReference, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.InlineExternalTableConnectionStringsClause, "", SyntaxCategory.Node),

                

                // End Inline External Table nodes

                new SyntaxData(SyntaxKind.IntLiteralExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.BooleanLiteralExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.LongLiteralExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.RealLiteralExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.DecimalLiteralExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.DateTimeLiteralExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.TimespanLiteralExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.TypeOfLiteralExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.GuidLiteralExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.StringLiteralExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.CompoundStringLiteralExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.NullLiteralExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.TokenLiteralExpression, "", SyntaxCategory.Node),

                // nullary expression?
                new SyntaxData(SyntaxKind.StarExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.AtExpression, "", SyntaxCategory.Node),

                // unary operartor expressions
                new SyntaxData(SyntaxKind.UnaryPlusExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.UnaryMinusExpression, "", SyntaxCategory.Node),

                // binary operator expressions
                new SyntaxData(SyntaxKind.AddExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.SubtractExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.MultiplyExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.DivideExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ModuloExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.LessThanExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.LessThanOrEqualExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.GreaterThanExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.GreaterThanOrEqualExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.EqualExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.NotEqualExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.AndExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.OrExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.InExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.InCsExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.HasAnyExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.HasAllExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.NotInExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.NotInCsExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.BetweenExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.NotBetweenExpression, "", SyntaxCategory.Node),

                // string binary operators
                new SyntaxData(SyntaxKind.EqualTildeExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.BangTildeExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.HasExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.HasCsExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.NotHasExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.NotHasCsExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.HasPrefixExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.HasPrefixCsExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.NotHasPrefixExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.NotHasPrefixCsExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.HasSuffixExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.HasSuffixCsExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.NotHasSuffixExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.NotHasSuffixCsExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.LikeExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.LikeCsExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.NotLikeExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.NotLikeCsExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ContainsExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ContainsCsExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.NotContainsExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.NotContainsCsExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.StartsWithExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.StartsWithCsExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.NotStartsWithExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.NotStartsWithCsExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.EndsWithExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.EndsWithCsExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.NotEndsWithExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.NotEndsWithCsExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.MatchesRegexExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.SearchExpression, "", SyntaxCategory.Node),

                // dynamic/json expressions
                new SyntaxData(SyntaxKind.JsonObjectExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.JsonPair, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.JsonArrayExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.DynamicExpression, "", SyntaxCategory.Node),

                // common query-related expressions & clauses
                new SyntaxData(SyntaxKind.TypedColumnReference, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.PackExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.NameAndTypeDeclaration, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.PrimitiveTypeExpression, "", SyntaxCategory.None),
                new SyntaxData(SyntaxKind.SchemaTypeExpression, "", SyntaxCategory.None),
                new SyntaxData(SyntaxKind.ToTypeOfClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.DataScopeClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.NameEqualsClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.DefaultExpressionClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.EvaluateSchemaClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.RowSchema, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.EvaluateRowSchema, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.EntityGroupExpression, "", SyntaxCategory.Node),

                // query operators
                new SyntaxData(SyntaxKind.BadQueryOperator, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.AsOperator, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.AssertSchemaOperator, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.ConsumeOperator, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.CountOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.CountAsIdentifierClause, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.DistinctOperator, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.ExecuteAndCacheOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ExtendOperator, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.FacetOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.FacetWithOperatorClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.FacetWithExpressionClause, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.FilterOperator, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.FindOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.FindInClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.FindProjectClause, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.ForkOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ForkExpression, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.GetSchemaOperator, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.GraphMatchOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.GraphShortestPathsOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.GraphMatchPattern, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.GraphMatchPatternNode, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.GraphMatchPatternEdge, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.GraphMatchPatternEdgeRange, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.GraphToTableOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.GraphToTableOutputClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.GraphToTableAsClause, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.InvokeOperator, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.LookupOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.JoinOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.JoinOnClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.MacroExpandScopeReferenceName, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.JoinWhereClause, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.MacroExpandOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.MakeGraphOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.MakeGraphWithTablesAndKeysClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.MakeGraphWithImplicitIdClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.MakeGraphTableAndKeyClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.GraphMarkComponentsOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.GraphWhereNodesOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.GraphWhereEdgesOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.MakeGraphPartitionedByClause, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.MakeSeriesOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.MakeSeriesExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.MakeSeriesOnClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.MakeSeriesInRangeClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.MakeSeriesToClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.MakeSeriesFromClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.MakeSeriesStepClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.MakeSeriesFromToStepClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.MakeSeriesByClause, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.MvApplyOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.MvApplyExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.MvApplyRowLimitClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.MvApplyContextIdClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.MvApplySubqueryExpression, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.MvExpandOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.MvExpandExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.MvExpandRowLimitClause, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.PartitionByOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.PartitionByIdClause, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.PartitionOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.PartitionSubquery, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.PartitionQuery, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.PartitionScope, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ParseOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ParseWhereOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ParseKvWithClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ParseKvOperator, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.EvaluateOperator, "", SyntaxCategory.Node), // evaluate

                new SyntaxData(SyntaxKind.PrintOperator, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.ProjectClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ProjectOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ProjectAwayOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ProjectByNamesOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ProjectKeepOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ProjectRenameOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ProjectReorderOperator, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.ReduceByOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ReduceByWithClause, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.RenderOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.RenderWithClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.NameReferenceList, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.SampleOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.SampleDistinctOperator, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.ScanOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ScanOrderByClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ScanPartitionByClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ScanDeclareClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ScanStep, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ScanStepOutput, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ScanComputationClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ScanAssignment, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.SearchOperator, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.SerializeOperator, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.SummarizeOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.SummarizeByClause, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.SortOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.OrderedExpression, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.OrderingClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.OrderingNullsClause, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.TakeOperator, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.TopOperator, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.TopHittersOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.TopHittersByClause, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.TopNestedOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.TopNestedClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.TopNestedWithOthersClause, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.UnionOperator, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.WhereClause, "", SyntaxCategory.Node),

                // statements
                new SyntaxData(SyntaxKind.AliasStatement, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.ExpressionStatement, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.FunctionDeclaration, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.FunctionParameters, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.FunctionParameter, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.DefaultValueDeclaration, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.FunctionBody, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.LetStatement, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.MaterializeExpression, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.PatternStatement, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.PatternPathParameter, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.PatternDeclaration, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.PatternMatch, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.PatternPathValue, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.RestrictStatement, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.RestrictStatementWithClause, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.SetOptionStatement, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.OptionValueClause, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.QueryParametersStatement, "", SyntaxCategory.Node),

                // commands
                new SyntaxData(SyntaxKind.CommandWithValueClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.CommandWithPropertyListClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.BadCommand, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.UnknownCommand, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.CustomCommand, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.PartialCommand, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.CommandAndSkippedTokens, "", SyntaxCategory.Node),

                // other
                new SyntaxData(SyntaxKind.QueryBlock, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.CommandBlock, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.DirectiveBlock, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.SkippedTokens, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.InputTextToken, "", SyntaxCategory.Other),
                new SyntaxData(SyntaxKind.Directive, "", SyntaxCategory.Other)
            };

            // put in sorted order  TODO: fix list to be in order 
            data.Sort((d1, d2) => string.Compare(d1.Text, d2.Text));

            var count = Enum.GetValues(typeof(SyntaxKind)).Length;

            kindToDataMap = new SyntaxData[count];
            textToKindMap = new TextKeyedDictionary<SyntaxKind>();

            for (int i = 0; i < data.Count; i++)
            {
                var d = data[i];

                // prove no overlapping kinds
                Ensure.IsNull(kindToDataMap[(int)d.Kind]);

                kindToDataMap[(int)d.Kind] = d;

                if (d.Text != null)
                {
                    textToKindMap.GetOrAddValue(d.Text, d.Kind);
                }
            }

#if DEBUG
            if (data.Count != kindToDataMap.Length
                || !kindToDataMap.All(item => item != null))
            {
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    // "Why did the debugger stop here?" you might be asking yourself.
                    // Most likely it happened because "somebody" just added a new value to the
                    // 'SyntaxKind' enum without adding a corresponding entry to the
                    // 'data' variable (see the of this method).
                    System.Diagnostics.Debugger.Break();
                }

                System.Diagnostics.Debug.Fail(typeof(SyntaxFacts).FullName + ".cctor: Mismatch between SyntaxKind enum and SyntaxFacts kindToDataMap");

            }
#endif
        }

        /// <summary>
        /// Gets the text of the specified keyword.
        /// </summary>
        public static string GetText(this SyntaxKind kind) 
            => kindToDataMap[(int)kind].Text;

        /// <summary>
        /// Gets the <see cref="SyntaxCategory"/> of the <see cref="SyntaxKind"/>
        /// </summary>
        public static SyntaxCategory GetCategory(this SyntaxKind kind) 
            => kindToDataMap[(int)kind].Category;

        public static bool IsKeyword(this SyntaxKind kind)
            => GetCategory(kind) == SyntaxCategory.Keyword;

        public static bool IsPunctuation(this SyntaxKind kind)
            => GetCategory(kind) == SyntaxCategory.Punctuation;

        public static bool IsOperator(this SyntaxKind kind)
            => GetCategory(kind) == SyntaxCategory.Operator;

        public static bool IsLiteral(this SyntaxKind kind)
            => GetCategory(kind) == SyntaxCategory.Literal;

        public static bool IsType(this SyntaxKind kind)
            => kindToDataMap[(int)kind].IsType;

        public static OperatorKind GetOperatorKind(this SyntaxKind kind)
            => kindToDataMap[(int)kind].OperatorKind;

        /// <summary>
        /// Gets the <see cref="SyntaxKind"/> corresponding to the text.
        /// </summary>
        public static bool TryGetKind(string text, out SyntaxKind kind)
        {
            return TryGetKind(text, 0, text.Length, out kind);
        }

        /// <summary>
        /// Gets the <see cref="SyntaxKind"/> corresponding to the text.
        /// </summary>
        public static bool TryGetKind(string text, int offset, int length, out SyntaxKind kind)
        {
            return textToKindMap.TryGetValue(text, offset, length, out kind);
        }

        /// <summary>
        /// Gets the set of <see cref="SyntaxKind"/>'s that are in the specified <see cref="SyntaxCategory"/>.
        /// </summary>
        public static IEnumerable<SyntaxKind> GetKinds(SyntaxCategory category)
        {
            foreach (var datum in kindToDataMap)
            {
                if (datum.Category == category)
                {
                    yield return datum.Kind;
                }
            }
        }

        /// <summary>
        /// Get all <see cref="SyntaxKind"/>'s that have a single text representation: keywords and punctuation.
        /// </summary>
        public static IEnumerable<SyntaxKind> GetKindsWithFixedText()
        {
            foreach (var datum in kindToDataMap)
            {
                if (datum.Text.Length > 0)
                {
                    yield return datum.Kind;
                }
            }
        }

        /// <summary>
        /// True if the keyword can also be used as an identifier.
        /// </summary>
        public static bool CanBeIdentifier(this SyntaxKind kind)
            => kindToDataMap[(int)kind].CanBeIdentifier;

        /// <summary>
        /// True if the text is a keyword (in this table).
        /// </summary>
        public static bool IsKeyword(string text) =>
            TryGetKind(text, out var kind) && kind.IsKeyword();

        /// <summary>
        /// True if the text is a keyword that can be an identifier (in this table).
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsKeywordThatCanBeIdentifier(string text) =>
            TryGetKind(text, out var kind) && kind.IsKeyword() && kind.CanBeIdentifier();

        /// <summary>
        /// All the keywords in Kusto
        /// </summary>
        public static IEnumerable<string> Keywords =>
            GetKinds(SyntaxCategory.Keyword).Select(GetText);

        /// <summary>
        /// All the punctuation in Kusto
        /// </summary>
        public static IEnumerable<string> Punctuation =>
            GetKinds(SyntaxCategory.Punctuation).Select(GetText);

        /// <summary>
        /// All the scalar math operators in Kusto
        /// </summary>
        public static IEnumerable<string> Operators =>
            GetKinds(SyntaxCategory.Operator).Select(GetText);
    }
}