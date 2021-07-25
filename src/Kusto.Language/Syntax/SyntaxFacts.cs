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

                new SyntaxData(SyntaxKind.AccessKeyword, "access", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.AccountsKeyword, "accounts", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.AccumulateKeyword, "accumulate"),
                new SyntaxData(SyntaxKind.AddKeyword, "add", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.AdminKeyword, "admin", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.AdminsKeyword, "admins", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.AliasKeyword, "alias" /*, canBeIdentifier: true*/),
                new SyntaxData(SyntaxKind.AllKeyword, "all", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.AlterKeyword, "alter", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.AlterMergeKeyword, "alter-merge", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.AndKeyword, "and", opKind: OperatorKind.And),
                new SyntaxData(SyntaxKind.AppendKeyword, "append", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ArtifactsKeyword, "artifacts", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.AsKeyword, "as"),
                new SyntaxData(SyntaxKind.AscKeyword, "asc"),
                new SyntaxData(SyntaxKind.AsyncKeyword, "async", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.AttachKeyword, "attach", canBeIdentifier: true),

                new SyntaxData(SyntaxKind.BagExpansionKeyword, "bagexpansion"),
                new SyntaxData(SyntaxKind.BasicAuthKeyword, "basicauth", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.BestEffortKeyword, "best_effort", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.BetweenKeyword, "between", opKind: OperatorKind.Between),
                new SyntaxData(SyntaxKind.BinKeyword, "bin", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.BinLegacyKeyword, "bin_legacy"),
                new SyntaxData(SyntaxKind.ByKeyword, "by"),

                new SyntaxData(SyntaxKind.CacheKeyword, "cache", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.CachingKeyword, "caching", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.CachingPolicyKeyword, "cachingpolicy"),
                new SyntaxData(SyntaxKind.CalloutKeyword, "callout"),
                new SyntaxData(SyntaxKind.CancelKeyword, "cancel"),
                new SyntaxData(SyntaxKind.CapacityKeyword, "capacity", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.CharactersKeyword, "characters"),
                new SyntaxData(SyntaxKind.ClusterKeyword, "cluster", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ColumnKeyword, "column", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ColumnsKeyword, "columns", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ColumnDocStringsKeyword, "column-docstrings"),
                new SyntaxData(SyntaxKind.CommandsKeyword, "commands", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.CommandsAndQueriesKeyword, "commands-and-queries", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.CommConcurrencyKeyword, "commconcurrency", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.CommPoolsKeyword, "commpools", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.CompletedKeyword, "completed", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.CompressedKeyword, "compressed", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ConsumeKeyword, "consume"),
                new SyntaxData(SyntaxKind.ContainsKeyword, "contains", opKind: OperatorKind.Contains),
                new SyntaxData(SyntaxKind.ContainsCsKeyword, "containscs", opKind: OperatorKind.ContainsCs),
                new SyntaxData(SyntaxKind.Contains_CsKeyword, "contains_cs", opKind: OperatorKind.ContainsCs),
                new SyntaxData(SyntaxKind.ContextualDataTableKeyword, "__contextual_datatable"),
                new SyntaxData(SyntaxKind.ContinuousExport, "continuous-export"),
                new SyntaxData(SyntaxKind.ContinuousExports, "continuous-exports"),
                new SyntaxData(SyntaxKind.CountKeyword, "count", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.CrashKeyword, "crash", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.CreateKeyword, "create", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.CreatedOnKeyword, "createdon", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.CreateOrAlterKeyword, "create-or-alter"),
                new SyntaxData(SyntaxKind.CreateMergeKeyword, "create-merge"),
                new SyntaxData(SyntaxKind.CslKeyword, "csl", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.CslSchemaKeyword, "cslschema", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.CsvKeyword, "csv", canBeIdentifier: true),

                new SyntaxData(SyntaxKind.DataKeyword, "data", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.DatabaseKeyword, "database", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.DatabaseCreatorsKeyword, "databasecreators", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.DatabasesKeyword, "databases", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.DataScopeKeyword, "datascope"),
                new SyntaxData(SyntaxKind.DataTableKeyword, "datatable"),
                new SyntaxData(SyntaxKind.DeclareKeyword, "declare" /*, canBeIdentifier: true*/),
                new SyntaxData(SyntaxKind.DecryptionCertificateThumbPrintKeyword, "decryption-certificate-thumbprint", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.DefaultKeyword, "default", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.DefineKeyword, "define", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.DeleteKeyword, "delete", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.DescKeyword, "desc"),
                new SyntaxData(SyntaxKind.DetachKeyword, "detach", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.DetailsKeyword, "details", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.DiagnosticsKeyword, "diagnostics", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.DisableKeyword, "disable", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.DistinctKeyword, "distinct"),
                new SyntaxData(SyntaxKind.DocStringKeyword, "docstring", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.DropKeyword, "drop", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.DropPretendKeyword, "drop-pretend", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.DupNextFailedIngestKeyword, "dup-next-failed-ingest", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.DupNextIngestKeyword, "dup-next-ingest", canBeIdentifier: true),

                new SyntaxData(SyntaxKind.EarliestKeyword, "earliest"),
                new SyntaxData(SyntaxKind.EchoKeyword, "echo", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.EffectiveKeyword, "effective", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.EnableKeyword, "enable", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.EncodingKeyword, "encoding", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.EncodingPolicyKeyword, "encodingpolicy", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.EndsWithKeyword, "endswith", opKind: OperatorKind.EndsWith),
                new SyntaxData(SyntaxKind.EndsWithCsKeyword, "endswith_cs", opKind: OperatorKind.EndsWithCs),
                new SyntaxData(SyntaxKind.EntityKeyword, "entity", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.EphemeralKeyword, "ephemeral", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.EvaluateKeyword, "evaluate", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ExceptKeyword, "except", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ExecuteKeyword, "execute", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ExecuteAndCacheKeyword, "__executeAndCache"),
                new SyntaxData(SyntaxKind.ExpandOutputKeyword, "expandoutput"),
                new SyntaxData(SyntaxKind.ExportKeyword, "export", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ExtendKeyword, "extend"),
                new SyntaxData(SyntaxKind.ExtentKeyword, "extent", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ExtentContainersKeyword, "extentcontainers", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ExtentsKeyword, "extents", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ExtentsMergeKeyword, "extentsmerge", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ExternalKeyword, "external"),
                new SyntaxData(SyntaxKind.ExternalDataKeyword, "externaldata"),
                new SyntaxData(SyntaxKind.External_DataKeyword, "external_data"),

                new SyntaxData(SyntaxKind.FabricKeyword, "fabric", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.FabricCacheKeyword, "fabriccache", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.FabricClocksKeyword, "fabricclocks", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.FabricLocksKeyword, "fabriclocks", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.FacetKeyword, "facet", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.FailuresKeyword, "failures", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.FilterKeyword, "filter"),
                new SyntaxData(SyntaxKind.FindKeyword, "find"),
                new SyntaxData(SyntaxKind.FirstKeyword, "first"),
                new SyntaxData(SyntaxKind.FlagsKeyword, "flags"),
                new SyntaxData(SyntaxKind.FolderKeyword, "folder", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ForceKeyword, "force", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ForkKeyword, "fork", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.FreshnessKeyword, "freshness", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.FromKeyword, "from", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.FunctionKeyword, "function", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.FunctionsKeyword, "functions", canBeIdentifier: true),

                new SyntaxData(SyntaxKind.GetSchemaKeyword, "getschema"),
                new SyntaxData(SyntaxKind.GroupsKeyword, "groups", canBeIdentifier: true),

                new SyntaxData(SyntaxKind.HardDeleteKeyword, "harddelete", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.HasKeyword, "has", opKind: OperatorKind.Has),
                new SyntaxData(SyntaxKind.HasCsKeyword, "has_cs", opKind: OperatorKind.HasCs),
                new SyntaxData(SyntaxKind.HashKeyword, "hash", canBeIdentifier: true),
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
                new SyntaxData(SyntaxKind.HotCacheKeyword, "hotcache", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.HotKeyword, "hot", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.HotDataKeyword, "hotdata", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.HotIndexKeyword, "hotindex", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.HoursKeyword, "hours", canBeIdentifier: true),

                new SyntaxData(SyntaxKind.IdKeyword, "id", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.IfLaterThanKeyword, "if_later_than", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.IfExistsKeyword, "ifexists", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.IfNotExistsKeyword, "ifnotexists", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.InKeyword, "in", opKind: OperatorKind.In),
                new SyntaxData(SyntaxKind.InCsKeyword, "in~", opKind: OperatorKind.InCs),
                new SyntaxData(SyntaxKind.HasAnyKeyword, "has_any", opKind: OperatorKind.HasAny),
                new SyntaxData(SyntaxKind.HasAllKeyword, "has_all", opKind: OperatorKind.HasAll),
                new SyntaxData(SyntaxKind.IngestKeyword, "ingest", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.IngestionKeyword, "ingestion", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.IngestionsKeyword, "ingestions", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.IngestionTimeKeyword, "ingestiontime", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.IngestorsKeyword, "ingestors", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.InlineKeyword, "inline", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.IntoKeyword, "into", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.InvokeKeyword, "invoke"),
                new SyntaxData(SyntaxKind.IsFuzzyKeyword, "isfuzzy", canBeIdentifier: true),

                new SyntaxData(SyntaxKind.JoinKeyword, "join"),
                new SyntaxData(SyntaxKind.JournalKeyword, "journal"),
                new SyntaxData(SyntaxKind.JsonKeyword, "json", canBeIdentifier: true),

                new SyntaxData(SyntaxKind.KeysKeyword, "keys", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.KindKeyword, "kind", canBeIdentifier: true),

                new SyntaxData(SyntaxKind.LastKeyword, "last"),
                new SyntaxData(SyntaxKind.LatestKeyword, "latest"),
                new SyntaxData(SyntaxKind.LetKeyword, "let" /*, canBeIdentifier: true*/),
                new SyntaxData(SyntaxKind.LikeKeyword, "like", opKind: OperatorKind.Like),
                new SyntaxData(SyntaxKind.LikeCsKeyword, "likecs", opKind: OperatorKind.LikeCs),
                new SyntaxData(SyntaxKind.LimitKeyword, "limit"),
                new SyntaxData(SyntaxKind.LoadKeyword, "load", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.LocalKeyword, "local", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.LookupKeyword, "lookup", canBeIdentifier: true),

                new SyntaxData(SyntaxKind.MakeSeriesKeyword, "make-series"),
                new SyntaxData(SyntaxKind.MappingKeyword, "mapping", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.MappingsKeyword, "mappings", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.MatchesRegexKeyword, "matches regex", opKind: OperatorKind.MatchRegex),
                new SyntaxData(SyntaxKind.MaterializeKeyword, "materialize"),
                new SyntaxData(SyntaxKind.MaterializedViewCombineKeyword, "materialized-view-combine"),
                new SyntaxData(SyntaxKind.MdmKeyword, "mdm"),
                new SyntaxData(SyntaxKind.MemoryKeyword, "memory", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.MergeKeyword, "merge", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.MetadataKeyword, "metadata", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.MissingKeyword, "missing"),
                new SyntaxData(SyntaxKind.MonitoringKeyword, "monitoring", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.MoveKeyword, "move", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.MvDashApplyKeyword, "mv-apply"),
                new SyntaxData(SyntaxKind.MvApplyKeyword, "mvapply"),
                new SyntaxData(SyntaxKind.MvDashExpandKeyword, "mv-expand"),
                new SyntaxData(SyntaxKind.MvExpandKeyword, "mvexpand"),

                new SyntaxData(SyntaxKind.NanKeyword, "nan", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.NoneKeyword, "none", canBeIdentifier: true),
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
                new SyntaxData(SyntaxKind.OlderKeyword, "older", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.OnKeyword, "on"),
                new SyntaxData(SyntaxKind.OperationsKeyword, "operations", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.OptionalKeyword, "optional", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.OrKeyword, "or", opKind: OperatorKind.Or),
                new SyntaxData(SyntaxKind.OrderKeyword, "order"),
                new SyntaxData(SyntaxKind.OthersKeyword, "others", canBeIdentifier: true),

                new SyntaxData(SyntaxKind.PackKeyword, "pack", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ParseKeyword, "parse"),
                new SyntaxData(SyntaxKind.ParseWhereKeyword, "parse-where"),
                new SyntaxData(SyntaxKind.PartitionKeyword, "partition", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.PartitioningKeyword, "partitioning"),
                new SyntaxData(SyntaxKind.PasswordKeyword, "password", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.PatternKeyword, "pattern", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.PersistKeyword, "persist", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.PluginKeyword, "plugin", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.PluginsKeyword, "plugins", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.PoliciesKeyword, "policies", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.PolicyKeyword, "policy", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.PretendKeyword, "pretend", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.PrettyNameKeyword, "prettyname", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.PrincipalKeyword, "principal", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.PrincipalsKeyword, "principals", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.PrintKeyword, "print"),
                new SyntaxData(SyntaxKind.ProjectKeyword, "project"),
                new SyntaxData(SyntaxKind.ProjectAwayKeyword, "project-away"),
                new SyntaxData(SyntaxKind._ProjectAwayKeyword, "__projectAway"),
                new SyntaxData(SyntaxKind.ProjectKeepKeyword, "project-keep"),
                new SyntaxData(SyntaxKind.ProjectRenameKeyword, "project-rename"),
                new SyntaxData(SyntaxKind.ProjectReorderKeyword, "project-reorder"),
                new SyntaxData(SyntaxKind.ProjectSmartKeyword, "project-smart"),
                new SyntaxData(SyntaxKind.PurgeKeyword, "purge", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.PurgeCleanupKeyword, "purge-cleanup"),

                new SyntaxData(SyntaxKind.QueriesKeyword, "queries", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.QueryKeyword, "query", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.QueryExecutionKeyword, "queryexecution", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.QueryParametersKeyword, "query_parameters", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.QueryPlanKeyword, "queryplan", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.QueryThrottlingKeyword, "querythrottling", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.QuickKeyword, "quick", canBeIdentifier: true),

                new SyntaxData(SyntaxKind.RangeKeyword, "range", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ReadOnlyKeyword, "readonly", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ReadWriteKeyword, "readwrite", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.RebalanceKeyword, "rebalance"),
                new SyntaxData(SyntaxKind.RebalancePretendKeyword, "rebalance-pretend"),
                new SyntaxData(SyntaxKind.RebuildKeyword, "rebuild", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.RecycleKeyword, "recycle", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ReduceKeyword, "reduce", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.RegexKeyword, "regex"),
                new SyntaxData(SyntaxKind.RelaxedKeyword, "relaxed"),
                new SyntaxData(SyntaxKind.RenameKeyword, "rename", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.RenderKeyword, "render", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ReplaceKeyword, "replace", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.RequestClassificationKeyword, "request_classification", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ResetKeyword, "reset"),
                new SyntaxData(SyntaxKind.RestrictKeyword, "restrict" /*, canBeIdentifier: true*/),
                new SyntaxData(SyntaxKind.RetentionKeyword, "retention", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.RolesKeyword, "roles", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.RowOrderKeyword, "roworder", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.RowOrderPolicyKeyword, "roworderpolicy", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.RowStoreKeyword, "rowstore"),
                new SyntaxData(SyntaxKind.RowStorePolicyKeyword, "rowstorepolicy"),
                new SyntaxData(SyntaxKind.RowStoresKeyword, "rowstores"),
                new SyntaxData(SyntaxKind.RunningKeyword, "running", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.RunKeyword, "run"),

                new SyntaxData(SyntaxKind.SandboxKeyword, "sandbox"),
                new SyntaxData(SyntaxKind.SandboxesKeyword, "sandboxes"),
                new SyntaxData(SyntaxKind.SampleKeyword, "sample"),
                new SyntaxData(SyntaxKind.SampleDistinctKeyword, "sample-distinct"),
                new SyntaxData(SyntaxKind.ScanKeyword, "scan", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.SaveKeyword, "save", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.SchemaKeyword, "schema", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ScriptKeyword, "script", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.SealKeyword, "seal"),
                new SyntaxData(SyntaxKind.SearchKeyword, "search"),
                new SyntaxData(SyntaxKind.SerializeKeyword, "serialize"),
                new SyntaxData(SyntaxKind.ServicePointsKeyword, "servicepoints"),
                new SyntaxData(SyntaxKind.SetKeyword, "set"),
                new SyntaxData(SyntaxKind.SetOrAppendKeyword, "set-or-append", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.SetOrReplaceKeyword, "set-or-replace", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ShardingKeyword, "sharding"),
                new SyntaxData(SyntaxKind.ShowKeyword, "show", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.SimpleKeyword, "simple"),
                new SyntaxData(SyntaxKind.StartsWithKeyword, "startswith", opKind: OperatorKind.StartsWith),
                new SyntaxData(SyntaxKind.StartsWithCsKeyword, "startswith_cs", opKind: OperatorKind.StartsWithCs),
                new SyntaxData(SyntaxKind.SoftDeleteKeyword, "softdelete"),
                new SyntaxData(SyntaxKind.SortKeyword, "sort"),
                new SyntaxData(SyntaxKind.SqlKeyword, "sql"),
                new SyntaxData(SyntaxKind.StateKeyword, "state", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.StepKeyword, "step", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.StorageKeyword, "storage", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.StreamKeyword, "stream", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.SummarizeKeyword, "summarize"),

                new SyntaxData(SyntaxKind.TableKeyword, "table", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.TablePurgeKeyword, "tablepurge"),
                new SyntaxData(SyntaxKind.TablesKeyword, "tables", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.TagsKeyword, "tags", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.TakeKeyword, "take"),
                new SyntaxData(SyntaxKind.TcpConnectionsKeyword, "tcpconnections", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.TempStorageKeyword, "tempstorage", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ThreadPoolsKeyword, "threadpools"),
                new SyntaxData(SyntaxKind.ThresholdKeyword, "threshold", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ThrowKeyword, "throw"),
                new SyntaxData(SyntaxKind.TimeoutKeyword, "timeout", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.TitleKeyword, "title", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ToKeyword, "to"),
                new SyntaxData(SyntaxKind.TopKeyword, "top"),
                new SyntaxData(SyntaxKind.TopHittersKeyword, "top-hitters"),
                new SyntaxData(SyntaxKind.TopNestedKeyword, "top-nested"),
                new SyntaxData(SyntaxKind.ToScalarKeyword, "toscalar"),
                new SyntaxData(SyntaxKind.ToTableKeyword, "totable"),
                new SyntaxData(SyntaxKind.TraceKeyword, "trace", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.TraceResultsKeyword, "traceresults", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.TsvKeyword, "tsv", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.TypeKeyword, "type", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.TypeOfKeyword, "typeof", canBeIdentifier: true),

                new SyntaxData(SyntaxKind.UndoKeyword, "undo", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.UnionKeyword, "union"),
                new SyntaxData(SyntaxKind.UpdateKeyword, "update", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.UserKeyword, "user", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.UsersKeyword, "users", canBeIdentifier: true),

                new SyntaxData(SyntaxKind.VerboseKeyword, "verbose"),
                new SyntaxData(SyntaxKind.VersionKeyword, "version", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ViewKeyword, "view", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.ViewersKeyword, "viewers"),
                new SyntaxData(SyntaxKind.VolatileKeyword, "volatile", canBeIdentifier: true),

                new SyntaxData(SyntaxKind.WarmKeyword, "warm", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.WarmingKeyword, "warming", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.WhatIfKeyword, "whatif", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.WhereKeyword, "where"),
                new SyntaxData(SyntaxKind.WithKeyword, "with"),
                new SyntaxData(SyntaxKind.WorkloadGroupKeyword, "workload_group", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.WorkloadGroupsKeyword, "workload_groups", canBeIdentifier: true),
                new SyntaxData(SyntaxKind.WriteAheadLogKeyword, "writeaheadlog", canBeIdentifier: true),

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
                new SyntaxData(SyntaxKind.SingleKeyword, "single", isType: true),
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

                // other tokens
                new SyntaxData(SyntaxKind.IdentifierToken, "", SyntaxCategory.Identifier),
                new SyntaxData(SyntaxKind.EndOfTextToken, "", SyntaxCategory.Other),
                new SyntaxData(SyntaxKind.DirectiveToken, "", SyntaxCategory.Other),
                new SyntaxData(SyntaxKind.BadToken, "", SyntaxCategory.Other),

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

                // query operators
                new SyntaxData(SyntaxKind.BadQueryOperator, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.AsOperator, "", SyntaxCategory.Node),

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

                new SyntaxData(SyntaxKind.InvokeOperator, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.LookupOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.JoinOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.JoinOnClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.JoinWhereClause, "", SyntaxCategory.Node),

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

                new SyntaxData(SyntaxKind.PartitionOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.PartitionSubquery, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.PartitionQuery, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.PartitionScope, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ParseOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ParseWhereOperator, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.EvaluateOperator, "", SyntaxCategory.Node), // evaluate

                new SyntaxData(SyntaxKind.PrintOperator, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.ProjectOperator, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.ProjectAwayOperator, "", SyntaxCategory.Node),
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

                new SyntaxData(SyntaxKind.SetOptionStatement, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.OptionValueClause, "", SyntaxCategory.Node),

                new SyntaxData(SyntaxKind.QueryParametersStatement, "", SyntaxCategory.Node),

                // commands
                new SyntaxData(SyntaxKind.CommandWithValueClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.CommandWithPropertyListClause, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.BadCommand, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.UnknownCommand, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.CustomCommand, "", SyntaxCategory.Node),

                // other
                new SyntaxData(SyntaxKind.QueryBlock, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.CommandBlock, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.DirectiveBlock, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.SkippedTokens, "", SyntaxCategory.Node),
                new SyntaxData(SyntaxKind.InputTextToken, "", SyntaxCategory.Other),
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