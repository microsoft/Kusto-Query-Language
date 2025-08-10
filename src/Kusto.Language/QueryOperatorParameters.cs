using System.Collections.Generic;

namespace Kusto.Language
{
    using Symbols;
    using Utils;

    /// <summary>
    /// Known parameters for specific query operators or expressions
    /// </summary>
    public static class QueryOperatorParameters
    {
        public static readonly QueryOperatorParameter BagExpansion =
            new QueryOperatorParameter("bagexpansion", QueryOperatorParameterValueKind.Word, values: KustoFacts.MvExpandKinds).Hide();

        public static readonly QueryOperatorParameter BinLegacy =
            new QueryOperatorParameter("bin_legacy", QueryOperatorParameterValueKind.Any);

        public static readonly QueryOperatorParameter CrossCluster =
            new QueryOperatorParameter("__crossCluster", QueryOperatorParameterValueKind.Any);

        public static readonly QueryOperatorParameter CrossDB =
            new QueryOperatorParameter("__crossDB", QueryOperatorParameterValueKind.Any);

        public static readonly QueryOperatorParameter DecodeBlocks =
            new QueryOperatorParameter("decodeblocks", QueryOperatorParameterValueKind.BoolLiteral, isRepeatable: false);

        public static readonly QueryOperatorParameter ExpandOutput =
            new QueryOperatorParameter("expandoutput", QueryOperatorParameterValueKind.Any);

        public static readonly QueryOperatorParameter Flags =
            new QueryOperatorParameter("flags", QueryOperatorParameterValueKind.Word);

        public static readonly QueryOperatorParameter HintDotConcurrency =
            new QueryOperatorParameter("hint.concurrency", QueryOperatorParameterValueKind.WordOrNumber, values: KustoFacts.HintConcurrencies, isCaseSensitive: false);

        public static readonly QueryOperatorParameter HintDotDistribution =
            new QueryOperatorParameter("hint.distribution", QueryOperatorParameterValueKind.Word, values: KustoFacts.HintDistributions, isCaseSensitive: false);

        public static readonly QueryOperatorParameter HintDotMaterialized =
            new QueryOperatorParameter("hint.materialized", QueryOperatorParameterValueKind.BoolLiteral);

        public static readonly QueryOperatorParameter HintDotNumPartitions =
            new QueryOperatorParameter("hint.num_partitions", QueryOperatorParameterValueKind.IntegerLiteral, isRepeatable: false);

        public static readonly QueryOperatorParameter HintDotPassFilters =
            new QueryOperatorParameter("hint.pass_filters", QueryOperatorParameterValueKind.Any);

        public static readonly QueryOperatorParameter HintDotPassFiltersColumn =
            new QueryOperatorParameter("hint.pass_filters_column", QueryOperatorParameterValueKind.Column);

        public static readonly QueryOperatorParameter HintDotProgressiveTop =
            new QueryOperatorParameter("hint.progressive_top", QueryOperatorParameterValueKind.BoolLiteral);

        public static readonly QueryOperatorParameter HintDotRemote =
            new QueryOperatorParameter("hint.remote", QueryOperatorParameterValueKind.Word, values: KustoFacts.HintRemotes, isCaseSensitive: false);

        public static readonly QueryOperatorParameter HintDotShuffleKey =
            new QueryOperatorParameter("hint.shufflekey", QueryOperatorParameterValueKind.Column, isRepeatable: true);

        public static readonly QueryOperatorParameter HintDotSpread =
            new QueryOperatorParameter("hint.spread", QueryOperatorParameterValueKind.WordOrNumber, values: KustoFacts.HintSpreads, isCaseSensitive: false);

        public static readonly QueryOperatorParameter HintDotStrategy =
            new QueryOperatorParameter("hint.strategy", QueryOperatorParameterValueKind.Word, values: KustoFacts.HintStrategies, isCaseSensitive: false);

        public static readonly QueryOperatorParameter Id =
            new QueryOperatorParameter("__id", QueryOperatorParameterValueKind.Any);

        public static readonly QueryOperatorParameter IsFuzzy =
            new QueryOperatorParameter("isfuzzy", QueryOperatorParameterValueKind.BoolLiteral, aliases: new[] { "__isFuzzy" });

        public static readonly QueryOperatorParameter BestEffort =
            new QueryOperatorParameter("best_effort", QueryOperatorParameterValueKind.BoolLiteral);

        public static readonly QueryOperatorParameter Kind =
            new QueryOperatorParameter("kind", QueryOperatorParameterValueKind.Word);

        public static readonly QueryOperatorParameter Output =
            new QueryOperatorParameter("output", QueryOperatorParameterValueKind.Word);

        public static readonly QueryOperatorParameter WithComponentId =
            new QueryOperatorParameter("with_component_id", QueryOperatorParameterValueKind.NameDeclaration);

        public static readonly QueryOperatorParameter NoWithSource =
            new QueryOperatorParameter("__noWithSource", QueryOperatorParameterValueKind.Any);

        public static readonly QueryOperatorParameter PackedColumn =
            new QueryOperatorParameter("__packedColumn", QueryOperatorParameterValueKind.Column);

        public static readonly QueryOperatorParameter SourceColumnIndex =
            new QueryOperatorParameter("__sourceColumnIndex", QueryOperatorParameterValueKind.Any);

        public static readonly QueryOperatorParameter WithMatchId =
            new QueryOperatorParameter("with_match_id", QueryOperatorParameterValueKind.NameDeclaration);

        public static readonly QueryOperatorParameter WithItemIndex =
            new QueryOperatorParameter("with_itemindex", QueryOperatorParameterValueKind.NameDeclaration);

        public static readonly QueryOperatorParameter WithSource =
            new QueryOperatorParameter("withsource", QueryOperatorParameterValueKind.NameDeclaration, aliases: new[] { "with_source" });

        public static readonly QueryOperatorParameter WithStepName =
            new QueryOperatorParameter("with_step_name", QueryOperatorParameterValueKind.NameDeclaration);

        public static readonly QueryOperatorParameter ForceRemote =
            new QueryOperatorParameter("force_remote", QueryOperatorParameterValueKind.BoolLiteral);

        public static readonly QueryOperatorParameter Cycles =
            new QueryOperatorParameter("cycles", QueryOperatorParameterValueKind.Word, values: KustoFacts.CyclesKinds);

        public static readonly QueryOperatorParameter ShortestPathsOutputs =
            new QueryOperatorParameter("output", QueryOperatorParameterValueKind.Word, values: KustoFacts.ShortestPathsOutputs);

        public static readonly QueryOperatorParameter ManagedIdentityAuthEnabled =
            new QueryOperatorParameter("AllowManagedIdentityAuthentication", QueryOperatorParameterValueKind.BoolLiteral, isRepeatable: false);

        /// <summary>
        /// All query operator parameters.
        /// Does not include parameters used for other syntax clauses (like render with properties)
        /// </summary>
        public static readonly IReadOnlyList<QueryOperatorParameter> AllParameters = new QueryOperatorParameter[]
        {
            BagExpansion.Hide(),
            BinLegacy.Hide(),
            CrossCluster.Hide(),
            CrossDB.Hide(),
            DecodeBlocks.Hide(),
            ExpandOutput.Hide(),
            //Flags.Hide(),
            HintDotConcurrency.Hide(),
            HintDotDistribution.Hide(),
            HintDotMaterialized.Hide(),
            HintDotNumPartitions.Hide(),
            HintDotPassFilters.Hide(),
            HintDotPassFiltersColumn.Hide(),
            HintDotProgressiveTop.Hide(),
            HintDotRemote.Hide(),
            HintDotShuffleKey.Hide(),
            HintDotSpread.Hide(),
            HintDotStrategy.Hide(),
            Id.Hide(),
            IsFuzzy.Hide(),
            Kind.Hide(),
            NoWithSource.Hide(),
            PackedColumn.Hide(),
            SourceColumnIndex.Hide(),
            Cycles.Hide(),
            WithMatchId.Hide(),
            WithItemIndex.Hide(),
            WithSource.Hide(),
            WithStepName.Hide(),
            ForceRemote.Hide(),
        };

        // parameter sets for specific operators

        public static readonly IReadOnlyList<QueryOperatorParameter> AsParameters = new QueryOperatorParameter[]
        {
            HintDotMaterialized
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> ConsumeParameters = new QueryOperatorParameter[]
        {
            DecodeBlocks,
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> DataTableParameters = new QueryOperatorParameter[]
        {
            // no known parameters
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> DistinctParameters = new QueryOperatorParameter[]
        {
            HintDotShuffleKey,
            HintDotStrategy.WithValues(KustoFacts.DistinctHintStrategies),
            HintDotNumPartitions
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> EvaluateParameters = new QueryOperatorParameter[]
        {
            HintDotDistribution.WithValues(KustoFacts.EvaluateHintDistributions),
            HintDotRemote.WithValues(KustoFacts.EvaluateHintRemotes)
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> ExternalDataWithClauseProperties = new QueryOperatorParameter[]
        {
            // no known parameters
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> InlineExternalTableProperties = new QueryOperatorParameter[]
        {
            // no known parameters
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> FilterParameters = new QueryOperatorParameter[]
        {
            // no known parameters
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> FindParameters = new QueryOperatorParameter[]
        {
            WithSource
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> GraphMatchParameters = new QueryOperatorParameter[]
        {
            Cycles.WithValues(KustoFacts.CyclesKinds),
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> GraphShortestPathsParameters = new QueryOperatorParameter[]
        {
            Output.WithValues(KustoFacts.ShortestPathsOutputs),
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> RestrictStatementParameters = new QueryOperatorParameter[]
        {
            ManagedIdentityAuthEnabled
        }.ToReadOnly();


        public static readonly IReadOnlyList<QueryOperatorParameter> JoinParameters = new QueryOperatorParameter[]
        {
            Kind.WithValues(KustoFacts.JoinKinds),
            HintDotRemote.WithValues(KustoFacts.JoinHintRemotes),
            HintDotShuffleKey,
            HintDotStrategy.WithValues(KustoFacts.JoinHintStrategies),
            HintDotNumPartitions
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> GraphMarkComponentsParameters = new QueryOperatorParameter[]
        {
            Kind.WithValues(KustoFacts.GraphMarkComponentsKinds),
            WithComponentId
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> LookupParameters = new QueryOperatorParameter[]
        {
            Kind.WithValues(KustoFacts.LookupKinds)
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> MakeSeriesParameters = new QueryOperatorParameter[]
        {
            HintDotShuffleKey,
            Kind.WithValues(KustoFacts.MakeSeriesKinds)
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> MvApplyParameters = new QueryOperatorParameter[]
        {
            WithItemIndex,
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> MvExpandParameters = new QueryOperatorParameter[]
        {
            Kind.WithValues(KustoFacts.MvExpandKinds),
            BagExpansion.WithValues(KustoFacts.MvExpandKinds).Hide(),
            WithItemIndex,
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> ParseParameters = new QueryOperatorParameter[]
        {
            Kind.WithValues(KustoFacts.ParseKinds),
            Flags,
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> ParseKvWithProperties = new QueryOperatorParameter[]
        {
            new QueryOperatorParameter("pair_delimiter", QueryOperatorParameterValueKind.StringLiteral),
            new QueryOperatorParameter("kv_delimiter", QueryOperatorParameterValueKind.StringLiteral),
            new QueryOperatorParameter("regex", QueryOperatorParameterValueKind.StringLiteral),
            new QueryOperatorParameter("quote", QueryOperatorParameterValueKind.StringLiteral, isRepeatable: true),
            new QueryOperatorParameter("escape", QueryOperatorParameterValueKind.StringLiteral, isRepeatable: true),
            new QueryOperatorParameter("greedy", QueryOperatorParameterValueKind.BoolLiteral),
        };

        public static readonly IReadOnlyList<QueryOperatorParameter> PartitionByParameters = new QueryOperatorParameter[]
        {
            HintDotConcurrency.WithValues(KustoFacts.PartitionHintConcurrencies),
            HintDotSpread.WithValues(KustoFacts.PartitionHintSpreads),
            HintDotMaterialized,
            HintDotShuffleKey,
            HintDotStrategy.WithValues(KustoFacts.PartitionHintStrategies)
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> PartitionParameters = new QueryOperatorParameter[]
        {
            HintDotConcurrency.WithValues(KustoFacts.PartitionHintConcurrencies),
            HintDotSpread.WithValues(KustoFacts.PartitionHintSpreads),
            HintDotMaterialized,
            HintDotShuffleKey,
            HintDotStrategy.WithValues(KustoFacts.PartitionHintStrategies)
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> ReduceParameters = new[]
        {
            Kind.WithValues(KustoFacts.ReduceByKinds)
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> ReduceWithParameters = new[]
        {
            new QueryOperatorParameter("threshold", QueryOperatorParameterValueKind.NumericLiteral),
            new QueryOperatorParameter("characters", QueryOperatorParameterValueKind.StringLiteral)
        }.ToReadOnly();

        public static readonly QueryOperatorParameter WithSourceId = new QueryOperatorParameter("with_source_id", QueryOperatorParameterValueKind.NameDeclaration);
        public static readonly QueryOperatorParameter WithTargetId = new QueryOperatorParameter("with_target_id", QueryOperatorParameterValueKind.NameDeclaration);
        public static readonly QueryOperatorParameter WithNodeId = new QueryOperatorParameter("with_node_id", QueryOperatorParameterValueKind.NameDeclaration);

        public static readonly IReadOnlyList<QueryOperatorParameter> GraphToTableEdgesParameters = new[]
        {
            WithSourceId,
            WithTargetId
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> GraphToTableNodesParameters = new[]
        {
            WithNodeId
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> GraphMakeParameters = new[]
        {
            HintDotStrategy.WithValues(KustoFacts.PartitionedGraphMakeHintStrategies)
        }.ToReadOnly();

        public static readonly QueryOperatorParameter RenderKind =
            new QueryOperatorParameter("kind", QueryOperatorParameterValueKind.Word, values: KustoFacts.ChartKinds);

        public static readonly QueryOperatorParameter RenderTitle =
            new QueryOperatorParameter("title", QueryOperatorParameterValueKind.String);

        public static readonly QueryOperatorParameter RenderAccumulate =
            new QueryOperatorParameter("accumulate", QueryOperatorParameterValueKind.BoolLiteral);

        public static readonly QueryOperatorParameter RenderWithDeprecated =
            new QueryOperatorParameter("with", QueryOperatorParameterValueKind.StringLiteral).WithHasNoEquals(true);

        public static readonly QueryOperatorParameter RenderByDeprecated =
            new QueryOperatorParameter("by", QueryOperatorParameterValueKind.ColumnList).WithHasNoEquals(true);

        public static readonly IReadOnlyList<QueryOperatorParameter> RenderParameters = new QueryOperatorParameter[]
        {
            RenderKind.Hide(),
            RenderTitle.Hide(),
            RenderAccumulate.Hide(),
            RenderWithDeprecated.Hide(),
            RenderByDeprecated.Hide()
        };

        public static readonly IReadOnlyList<QueryOperatorParameter> RenderWithProperties = new QueryOperatorParameter[]
        {
            RenderKind,
            RenderTitle,
            RenderAccumulate,
            new QueryOperatorParameter("xcolumn", QueryOperatorParameterValueKind.Column),
            new QueryOperatorParameter("ycolumns", QueryOperatorParameterValueKind.ColumnList),
            new QueryOperatorParameter("anomalycolumns", QueryOperatorParameterValueKind.ColumnList),
            new QueryOperatorParameter("series", QueryOperatorParameterValueKind.ColumnList),
            new QueryOperatorParameter("xtitle", QueryOperatorParameterValueKind.String),
            new QueryOperatorParameter("ytitle", QueryOperatorParameterValueKind.String),
            new QueryOperatorParameter("xaxis", QueryOperatorParameterValueKind.Word, values: KustoFacts.ChartAxis),
            new QueryOperatorParameter("yaxis", QueryOperatorParameterValueKind.Word, values: KustoFacts.ChartAxis),
            new QueryOperatorParameter("legend", QueryOperatorParameterValueKind.Word, values: KustoFacts.ChartLegends),
            new QueryOperatorParameter("ysplit", QueryOperatorParameterValueKind.Word, values: KustoFacts.ChartYSplit),
            new QueryOperatorParameter("ymin", QueryOperatorParameterValueKind.ForcedRealLiteral),
            new QueryOperatorParameter("ymax", QueryOperatorParameterValueKind.ForcedRealLiteral),
            new QueryOperatorParameter("xmin", QueryOperatorParameterValueKind.ScalarLiteral),
            new QueryOperatorParameter("xmax", QueryOperatorParameterValueKind.ScalarLiteral),
        };

        public static readonly IReadOnlyList<QueryOperatorParameter> SampleParameters = new QueryOperatorParameter[]
        {
            // no known parameters
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> SampleDistinctParameters = new QueryOperatorParameter[]
        {
            // no known parameters
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> ScanParameters = new QueryOperatorParameter[]
        {
            Kind.WithValues(KustoFacts.ScanKinds).Hide(),
            WithMatchId,
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> SearchParameters = new QueryOperatorParameter[]
        {
            Kind.WithValues(KustoFacts.SearchKinds)
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> SerializedParameters = new QueryOperatorParameter[]
        {
            // no known parameters
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> SortParameters = new QueryOperatorParameter[]
        {
            HintDotStrategy.WithValues(KustoFacts.SortHintStrategies)
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> SummarizeParameters = new QueryOperatorParameter[]
        {
            HintDotShuffleKey,
            HintDotStrategy.WithValues(KustoFacts.SummarizeHintStrategies),
            HintDotNumPartitions
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> TakeParameters = new QueryOperatorParameter[]
        {
            // no known parameters
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> TopParameters = new QueryOperatorParameter[]
        {
            HintDotProgressiveTop,
        }.ToReadOnly();

        public static readonly QueryOperatorParameter ToScalarKindParameter =
            Kind.WithValues(KustoFacts.ToScalarKinds);

        public static readonly QueryOperatorParameter ToTableKindParameter =
            Kind.WithValues(KustoFacts.ToTableKinds);

        public static readonly IReadOnlyList<QueryOperatorParameter> UnionParameters = new QueryOperatorParameter[]
        {
            Kind.WithValues(KustoFacts.UnionKinds),
            WithSource,
            IsFuzzy,
            BestEffort,
            HintDotConcurrency.WithValues(KustoFacts.UnionHintConcurrencies),
            HintDotSpread.WithValues(KustoFacts.UnionHintSpreads)
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> MacroExpandParameters = new QueryOperatorParameter[]
         {
            ForceRemote,
            IsFuzzy,
            BestEffort
         }.ToReadOnly();

        public static readonly QueryOperatorParameter GetSchemaKind =
            new QueryOperatorParameter("kind", QueryOperatorParameterValueKind.Word, values: new[] {"csl"});
    }

    public enum QueryOperatorParameterValueKind
    {
        /// <summary>
        /// Any value
        /// </summary>
        Any,

        /// <summary>
        /// Any scalar literal value
        /// </summary>
        ScalarLiteral,

        /// <summary>
        /// Any integer literal value
        /// </summary>
        IntegerLiteral,

        /// <summary>
        /// Any numeric literal value
        /// </summary>
        NumericLiteral,

        /// <summary>
        /// Any numeric literal, but represented as a double for whatever reason.
        /// </summary>
        ForcedRealLiteral,

        /// <summary>
        /// Any scalar string literal
        /// </summary>
        StringLiteral,

        /// <summary>
        /// Any boolean literal
        /// </summary>
        BoolLiteral,

        /// <summary>
        /// Any summable literal value
        /// </summary>
        SummableLiteral,

        /// <summary>
        /// Any string expression
        /// </summary>
        String,

        /// <summary>
        /// The parameter is an word token (identifier or keyword)
        /// </summary>
        Word,

        /// <summary>
        /// The parameter is an word token (identifier or keyword) or a numeric literal
        /// </summary>
        WordOrNumber,

        /// <summary>
        /// The parameter is a name declaration
        /// </summary>
        NameDeclaration,

        /// <summary>
        /// The parameter is a column reference
        /// </summary>
        Column,

        /// <summary>
        /// The parameter is a list of column references
        /// </summary>
        ColumnList,
    }

    /// <summary>
    /// Describes a parameter that a query operator may have.
    /// </summary>
    public class QueryOperatorParameter : Symbol
    {
        /// <summary>
        /// The kind that the value can take.
        /// </summary>
        public QueryOperatorParameterValueKind ValueKind { get; }

        /// <summary>
        /// True if token/keyword value matches are case sensitive.
        /// </summary>
        public bool IsCaseSensitive { get; }

        /// <summary>
        /// The set of known parameter values.
        /// </summary>
        public IReadOnlyList<string> Values { get; }

        /// <summary>
        /// True if the parameter can be specified more than once.
        /// </summary>
        public bool IsRepeatable { get; }

        /// <summary>
        /// True if the parameter is typed with no equals token between the name and value
        /// </summary>
        public bool HasNoEquals { get; }

        /// <summary>
        /// Any additional names that the parameter can be referenced by.
        /// </summary>
        public IReadOnlyList<string> Aliases { get; }

        private readonly bool _isHidden;
        public override bool IsHidden => _isHidden;

        public override SymbolKind Kind => SymbolKind.QueryOperatorParameter;

        private QueryOperatorParameter(
            string name, 
            QueryOperatorParameterValueKind kind, 
            bool isCaseSensitive, 
            IEnumerable<string> values, 
            bool isRepeatable, 
            bool isHidden,
            bool hasNoEquals,
            IReadOnlyList<string> aliases)
            : base(name)
        {
            this.ValueKind = kind;
            this.Values = values.ToReadOnly();
            this.IsCaseSensitive = isCaseSensitive;
            this.IsRepeatable = isRepeatable;
            _isHidden = isHidden;
            this.HasNoEquals = hasNoEquals;
            this.Aliases = aliases.ToReadOnly();
        }

        public QueryOperatorParameter(
            string name, 
            QueryOperatorParameterValueKind kind, 
            bool isCaseSensitive = true, 
            IEnumerable<string> values = null, 
            bool isRepeatable = false,
            IReadOnlyList<string> aliases = null)
            : this(name, kind, isCaseSensitive, values, isRepeatable, false, false, aliases)
        {
        }

        public QueryOperatorParameter WithIsHidden(bool isHidden)
        {
            if (this.IsHidden != isHidden)
            {
                return new QueryOperatorParameter(this.Name, this.ValueKind, this.IsCaseSensitive, this.Values, this.IsRepeatable, isHidden, this.HasNoEquals, this.Aliases);
            }
            else
            {
                return this;
            }
        }

        public QueryOperatorParameter WithValues(IReadOnlyList<string> values)
        {
            if (this.Values != values)
            {
                return new QueryOperatorParameter(this.Name, this.ValueKind, this.IsCaseSensitive, values, this.IsRepeatable, this.IsHidden, this.HasNoEquals, this.Aliases);
            }
            else
            {
                return this;
            }
        }

        public QueryOperatorParameter WithIsCaseSensitive(bool isCaseSensitive)
        {
            if (this.IsCaseSensitive != isCaseSensitive)
            {
                return new QueryOperatorParameter(this.Name, this.ValueKind, isCaseSensitive, this.Values, this.IsRepeatable, this.IsHidden, this.HasNoEquals, this.Aliases);
            }
            else
            {
                return this;
            }
        }

        public QueryOperatorParameter WithHasNoEquals(bool hasNoEquals)
        {
            if (this.HasNoEquals != hasNoEquals)
            {
                return new QueryOperatorParameter(this.Name, this.ValueKind, this.IsCaseSensitive, this.Values, this.IsRepeatable, this.IsHidden, hasNoEquals, this.Aliases);
            }
            else
            {
                return this;
            }
        }

        public QueryOperatorParameter Hide() => WithIsHidden(true);
    }
}
