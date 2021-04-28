using System.Collections.Generic;

namespace Kusto.Language
{
    using Symbols;

    /// <summary>
    /// Well known query options
    /// </summary>
    public class Options
    {
        public static readonly OptionSymbol DebugPython =
            new OptionSymbol("query_python_debug", "If set, generate python debug query for the enumerated python node (default first).", new[] { ScalarTypes.Bool, ScalarTypes.Int });

        public static readonly OptionSymbol DeferPartialQueryFailures =
            new OptionSymbol("deferpartialqueryfailures", "If true, disables reporting partial query failures as part of the result set.", ScalarTypes.Bool);

        public static readonly OptionSymbol DoNotImpersonate =
            new OptionSymbol("request_impersonation_disabled", "If specified, indicates that the service should not impersonate the caller's identity.", ScalarTypes.Bool);

        public static readonly OptionSymbol MaterializedViewShuffleQuery =
            new OptionSymbol("materialized_view_shuffle",
                "An hint to use shuffle strategy for materialized views that are referenced in the query.\r\n" +
                "The property is an array of materialized views names and the shuffle keys to use.\r\n" +
                "examples: 'dynamic(", ScalarTypes.Dynamic);

        public static readonly OptionSymbol MaxEntitiesToUnion =
            new OptionSymbol("query_max_entities_in_union", "Overrides the default maximum number of columns a query is allowed to produce.", ScalarTypes.Long);

        public static readonly OptionSymbol MaxMemoryConsumptionPerIterator =
            new OptionSymbol("maxmemoryconsumptionperiterator", "Overrides the default maximum amount of memory a query operator may allocate.", ScalarTypes.Long);

        public static readonly OptionSymbol MaxMemoryConsumptionPerQueryPerNode =
            new OptionSymbol("max_memory_consumption_per_query_per_node", "Overrides the default maximum amount of memory a whole query may allocate per node.", ScalarTypes.Long);

        public static readonly OptionSymbol MaxOutputColumns =
            new OptionSymbol("maxoutputcolumns", "Overrides the default maximum number of columns a query is allowed to produce.", ScalarTypes.Long);

        public static readonly OptionSymbol NoRequestTimeout =
            new OptionSymbol("norequesttimeout", "Enables setting the request timeout to its maximum value.", ScalarTypes.Bool);

        public static readonly OptionSymbol NoTruncation =
            new OptionSymbol("notruncation", "Enables suppressing truncation of the query results returned to the caller.", ScalarTypes.Bool);

        public static readonly OptionSymbol ProgressiveProgressReportPeriod =
            new OptionSymbol("query_results_progressive_update_period", "Hint for Kusto as to how often to send progress frames (takes effect only if OptionResultsProgressiveEnabled is set)");

        public static readonly OptionSymbol ProgressiveQueryMinRowCountPerUpdate =
            new OptionSymbol("query_results_progressive_row_count", "Hint for Kusto as to how many records to send in each update (takes effect only if OptionResultsProgressiveEnabled is set)");

        public static readonly OptionSymbol PushSelectionThroughAggregation =
            new OptionSymbol("push_selection_through_aggregation", "If true, push simple selection through aggregation", ScalarTypes.Bool);

        public static readonly OptionSymbol QueryBinAutoAt =
            new OptionSymbol("query_bin_auto_at", "When evaluating the bin_auto() function, the start value to use.");

        public static readonly OptionSymbol QueryBinAutoSize =
            new OptionSymbol("query_bin_auto_size", "When evaluating the bin_auto() function, the bin size value to use.");

        public static readonly OptionSymbol QueryConsistency =
            new OptionSymbol("queryconsistency", "Controls query consistency.", ScalarTypes.String, new[] { "'strongconsistency'", "'normalconsistency'", "'weakconsistency'" });

        public static readonly OptionSymbol QueryCursorAfterDefault =
            new OptionSymbol("query_cursor_after_default", "The default parameter value of the cursor_after() function when called without parameters.", ScalarTypes.String);

        public static readonly OptionSymbol QueryCursorBeforeOrAtDefault =
            new OptionSymbol("query_cursor_before_or_at_default", "The default parameter value of the cursor_before_or_at() function when called without parameters.", ScalarTypes.String);

        public static readonly OptionSymbol QueryCursorCurrent =
            new OptionSymbol("query_cursor_current", "Overrides the cursor value returned by the cursor_current() or current_cursor() functions.", ScalarTypes.String);

        public static readonly OptionSymbol QueryCursorDisabled =
            new OptionSymbol("query_cursor_disabled", "Disables usage of cursor functions in the context of the query.", ScalarTypes.Bool);

        public static readonly OptionSymbol QueryCursorScopedTables =
            new OptionSymbol("query_cursor_scoped_tables", "List of table names that should be scoped to cursor_after_default .. cursor_before_or_at_default (upper bound is optional).", ScalarTypes.Dynamic);

        public static readonly OptionSymbol QueryDataScope =
            new OptionSymbol("query_datascope", "Controls the query's datascope -- whether the query applies to all data or just part of it.", ScalarTypes.String, new[] { "'default'", "'all'", "'hotcache'" });

        public static readonly OptionSymbol QueryDateTimeScopeColumn =
            new OptionSymbol("query_datetimescope_column", "Controls the column name for the query's datetime scope (query_datetimescope_to / query_datetimescope_from).", ScalarTypes.String);

        public static readonly OptionSymbol QueryDateTimeScopeFrom =
            new OptionSymbol("query_datetimescope_from", "Controls the query's datetime scope (earliest) -- used as auto-applied filter on query_datetimescope_column only (if defined).", ScalarTypes.DateTime);

        public static readonly OptionSymbol QueryDateTimeScopeTo =
            new OptionSymbol("query_datetimescope_to", "Controls the query's datetime scope (latest) -- used as auto-applied filter on query_datetimescope_column only (if defined).", ScalarTypes.DateTime);

        public static readonly OptionSymbol QueryDistributionNodesSpanSize =
            new OptionSymbol("query_distribution_nodes_span",
                "If set, controls the way sub-query merge behaves: the executing node will introduce an additional level\r\n"
                + "in the query hierarchy for each sub-group of nodes; the size of the sub-group is set by this option.", ScalarTypes.Int);

        public static readonly OptionSymbol QueryFanoutNodesPercent =
            new OptionSymbol("query_fanout_nodes_percent", "The percentage of nodes to fanout execution to.", ScalarTypes.Int);

        public static readonly OptionSymbol QueryFanoutThreadsPercent =
            new OptionSymbol("query_fanout_threads_percent", "The percentage of threads to fanout execution to.", ScalarTypes.Int);

        public static readonly OptionSymbol QueryForceRowLevelSecurity =
            new OptionSymbol("query_force_row_level_security", "If specified, forces Row Level Security rules, even if row_level_security policy is disabled", ScalarTypes.Bool);

        public static readonly OptionSymbol QueryLanguage =
            new OptionSymbol("query_language", "Controls how the query text is to be interpreted.", ScalarTypes.String, new[] { "'csl'", "'kql'", "'sql'" });

        public static readonly OptionSymbol QueryNow =
            new OptionSymbol("query_now", "Overrides the datetime value returned by the now(0s) function.", ScalarTypes.DateTime);
        
        public static readonly OptionSymbol QueryResultsApplyGetSchema =
            new OptionSymbol("query_results_apply_getschema", "If set, retrieves the schema of each tabular data in the results of the query instead of the data itself.", ScalarTypes.Bool);

        public static readonly OptionSymbol QueryResultsCacheMaxAge =
            new OptionSymbol("query_results_cache_max_age", "If positive, controls the maximum age of the cached query results which Kusto is allowed to return", ScalarTypes.TimeSpan);

        public static readonly OptionSymbol QueryResultsCachePerShardEnabled =
            new OptionSymbol("query_results_cache_per_shard", "If set, enables per-shard query cache.", ScalarTypes.Bool);

        public static readonly OptionSymbol RequestBlockRowLevelSecurity =
            new OptionSymbol("request_block_row_level_security", "If specified, blocks access to tables for which row_level_security policy is enabled", ScalarTypes.Bool);

        public static readonly OptionSymbol RequestCalloutDisabled =
            new OptionSymbol("request_callout_disabled", "If specified, indicates that the request cannot call-out to a user-provided service.", ScalarTypes.Bool);

        public static readonly OptionSymbol RequestDescription =
            new OptionSymbol("request_description", "Arbitrary text that the author of the request wants to include as the request description.", ScalarTypes.String);

        public static readonly OptionSymbol RequestExternalTableDisabled =
            new OptionSymbol("request_external_table_disabled", " If specified, indicates that the request cannot invoke code in the ExternalTable.", ScalarTypes.Bool);

        public static readonly OptionSymbol RequestReadOnly =
            new OptionSymbol("request_readonly", "If specified, indicates that the request must not be able to write anything.", ScalarTypes.Bool);

        public static readonly OptionSymbol RequestRemoteEntitiesDisabled =
            new OptionSymbol("request_remote_entities_disabled", "If specified, indicates that the request cannot access remote databases and clusters.", ScalarTypes.Bool);

        public static readonly OptionSymbol RequestSandboxedExecutionDisabled =
            new OptionSymbol("request_sandboxed_execution_disabled", "If specified, indicates that the request cannot invoke code in the sandbox.", ScalarTypes.Bool);

        public static readonly OptionSymbol ResultsProgressiveEnabled =
            new OptionSymbol("results_progressive_enabled", "If set, enables the progressive query stream");

        public static readonly OptionSymbol ServerTimeout =
            new OptionSymbol("servertimeout", "Overrides the default request timeout.", ScalarTypes.TimeSpan);

        public static readonly OptionSymbol TakeMaxRecords =
            new OptionSymbol("query_take_max_records", "Enables limiting query results to this number of records.", ScalarTypes.Long);

        public static readonly OptionSymbol TruncationMaxRecords =
            new OptionSymbol("truncationmaxrecords", "Overrides the default maximum number of records a query is allowed to return to the caller (truncation).", ScalarTypes.Long);

        public static readonly OptionSymbol TruncationMaxSize =
            new OptionSymbol("truncationmaxsize", "Overrides the default maximum data size a query is allowed to return to the caller (truncation).", ScalarTypes.Long);

        public static readonly OptionSymbol ValidatePermissions =
            new OptionSymbol("validate_permissions", "Validates user's permissions to perform the query and doesn't run the query itself.", ScalarTypes.Bool);

        public static readonly OptionSymbol RequestUser =
           new OptionSymbol("request_user", "Request user to be used in the traces.", ScalarTypes.String);

        public static readonly OptionSymbol RequestAppName =
           new OptionSymbol("request_app_name", "Request application name to be used in the traces.", ScalarTypes.String);

#if QUERY_COLD_DATA_SCAN_MAX_RECORDS
        public static readonly OptionSymbol QueryColdDataScanMaxRecords =
            new OptionSymbol("query_cold_data_scan_max_records", "Enables limiting query to scanning no more than N records of the cold data.", ScalarTypes.Long);
#endif

        public static readonly IReadOnlyList<OptionSymbol> All = new[]
        {
            DebugPython,
            DeferPartialQueryFailures,
            DoNotImpersonate,
            MaterializedViewShuffleQuery,
            MaxEntitiesToUnion,
            MaxMemoryConsumptionPerIterator,
            MaxMemoryConsumptionPerQueryPerNode,
            MaxOutputColumns,
            NoRequestTimeout,
            NoTruncation,
            ProgressiveProgressReportPeriod,
            ProgressiveQueryMinRowCountPerUpdate,
            PushSelectionThroughAggregation,
            QueryBinAutoAt,
            QueryBinAutoSize,
#if QUERY_COLD_DATA_SCAN_MAX_RECORDS
            QueryColdDataScanMaxRecords,
#endif
            QueryConsistency,
            QueryCursorAfterDefault,
            QueryCursorBeforeOrAtDefault,
            QueryCursorCurrent,
            QueryCursorDisabled,
            QueryCursorScopedTables,
            QueryDataScope,
            QueryDateTimeScopeColumn,
            QueryDateTimeScopeFrom,
            QueryDateTimeScopeTo,
            QueryDistributionNodesSpanSize,
            QueryFanoutNodesPercent,
            QueryFanoutThreadsPercent,
            QueryForceRowLevelSecurity,
            QueryLanguage,
            QueryNow,
            QueryResultsApplyGetSchema,
            QueryResultsCacheMaxAge,
            QueryResultsCachePerShardEnabled,
            RequestBlockRowLevelSecurity,
            RequestCalloutDisabled,
            RequestDescription,
            RequestExternalTableDisabled,
            RequestReadOnly,
            RequestRemoteEntitiesDisabled,
            RequestSandboxedExecutionDisabled,
            ResultsProgressiveEnabled,
            ServerTimeout,
            TakeMaxRecords,
            TruncationMaxRecords,
            TruncationMaxSize,
            ValidatePermissions,
            RequestUser,
            RequestAppName
        };
    }
}