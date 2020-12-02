using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language
{
    using Symbols;
    using Syntax;
    using Utils;

    /// <summary>
    /// Known parameters for specific query operators or expressions
    /// </summary>
    public static class QueryOperatorParameters
    {
        public static readonly IReadOnlyList<QueryOperatorParameter> AsParameters = new QueryOperatorParameter[]
        {
            new QueryOperatorParameter(SyntaxFacts.GetText(SyntaxKind.HintDotMaterializedKeyword), QueryOperatorParameterKind.BoolLiteral),
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> ConsumeParameters = new QueryOperatorParameter[]
        {
            new QueryOperatorParameter("decodeblocks", QueryOperatorParameterKind.BoolLiteral, isRepeatable: false),
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> DataTableParameters = new QueryOperatorParameter[]
        {
            // no known parameters
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> DistinctParameters = new QueryOperatorParameter[]
        {
            // no known parameters
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> EvaluateParameters = new QueryOperatorParameter[]
        {
            new QueryOperatorParameter(SyntaxFacts.GetText(SyntaxKind.HintDotDistributionKeyword), QueryOperatorParameterKind.Word, values: KustoFacts.DistributionHintStrategies)
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> ExternalDataWithClauseProperties = new QueryOperatorParameter[]
        {
            // no known parameters
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> FilterParameters = new QueryOperatorParameter[]
        {
            // no known parameters
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> FindParameters = new QueryOperatorParameter[]
        {
            new QueryOperatorParameter(KustoFacts.FindWithSourceProperty, QueryOperatorParameterKind.NameDeclaration)
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> JoinParameters = new QueryOperatorParameter[]
        {
            new QueryOperatorParameter(SyntaxFacts.GetText(SyntaxKind.KindKeyword), QueryOperatorParameterKind.Word, values: KustoFacts.JoinKinds),
            new QueryOperatorParameter(SyntaxFacts.GetText(SyntaxKind.HintDotRemoteKeyword), QueryOperatorParameterKind.Word, values: KustoFacts.JoinHintRemotes),
            new QueryOperatorParameter(SyntaxFacts.GetText(SyntaxKind.HintDotShuffleKeyKeyword), QueryOperatorParameterKind.Column, isRepeatable: true),
            new QueryOperatorParameter(SyntaxFacts.GetText(SyntaxKind.HintDotStrategyKeyword), QueryOperatorParameterKind.Word, values: KustoFacts.JoinHintStrategies),
            new QueryOperatorParameter(SyntaxFacts.GetText(SyntaxKind.HintDotNumPartitions), QueryOperatorParameterKind.IntegerLiteral, isRepeatable: false),
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> LookupParameters = new QueryOperatorParameter[]
        {
            new QueryOperatorParameter(SyntaxFacts.GetText(SyntaxKind.KindKeyword), QueryOperatorParameterKind.Word, values: KustoFacts.JoinKinds),
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> MakeSeriesParameters = new QueryOperatorParameter[]
        {
            // no known parameters
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> MvApplyParameters = new QueryOperatorParameter[]
        {
            new QueryOperatorParameter(KustoFacts.MvApplyWithItemIndexProperty, QueryOperatorParameterKind.NameDeclaration)
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> MvExpandParameters = new QueryOperatorParameter[]
        {
            new QueryOperatorParameter("kind", QueryOperatorParameterKind.Word, values: KustoFacts.MvExpandKinds),
            new QueryOperatorParameter(KustoFacts.MvExpandBagExpansionProperty, QueryOperatorParameterKind.Word, values: KustoFacts.MvExpandKinds).Hide(),
            new QueryOperatorParameter(KustoFacts.MvExpandWithItemIndexProperty, QueryOperatorParameterKind.NameDeclaration)
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> ParseParameters = new QueryOperatorParameter[]
        {
            new QueryOperatorParameter(SyntaxFacts.GetText(SyntaxKind.KindKeyword), QueryOperatorParameterKind.Word, values: KustoFacts.ParseKinds),
            new QueryOperatorParameter(SyntaxFacts.GetText(SyntaxKind.FlagsKeyword), QueryOperatorParameterKind.Word)
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> PartitionParameters = new QueryOperatorParameter[]
        {
            new QueryOperatorParameter(SyntaxFacts.GetText(SyntaxKind.HintDotConcurrencyKeyword), QueryOperatorParameterKind.WordOrNumber, values: KustoFacts.JoinHintRemotes),
            new QueryOperatorParameter(SyntaxFacts.GetText(SyntaxKind.HintDotSpreadKeyword), QueryOperatorParameterKind.WordOrNumber, values: KustoFacts.JoinHintRemotes),
            new QueryOperatorParameter(SyntaxFacts.GetText(SyntaxKind.HintDotMaterializedKeyword), QueryOperatorParameterKind.BoolLiteral),
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> RenderWithProperties = new QueryOperatorParameter[]
        {
            new QueryOperatorParameter("kind", QueryOperatorParameterKind.Word, values: KustoFacts.ChartKinds),
            new QueryOperatorParameter("title", QueryOperatorParameterKind.StringLiteral),
            new QueryOperatorParameter("accumulate", QueryOperatorParameterKind.BoolLiteral),
            new QueryOperatorParameter("xcolumn", QueryOperatorParameterKind.Column),
            new QueryOperatorParameter("ycolumns", QueryOperatorParameterKind.ColumnList),
            new QueryOperatorParameter("anomalycolumns", QueryOperatorParameterKind.ColumnList),
            new QueryOperatorParameter("series", QueryOperatorParameterKind.ColumnList),
            new QueryOperatorParameter("xtitle", QueryOperatorParameterKind.StringLiteral),
            new QueryOperatorParameter("ytitle", QueryOperatorParameterKind.StringLiteral),
            new QueryOperatorParameter("xaxis", QueryOperatorParameterKind.Word, values: KustoFacts.ChartAxis),
            new QueryOperatorParameter("yaxis", QueryOperatorParameterKind.Word, values: KustoFacts.ChartAxis),
            new QueryOperatorParameter("legend", QueryOperatorParameterKind.Word, values: KustoFacts.ChartLegends),
            new QueryOperatorParameter("ysplit", QueryOperatorParameterKind.Word, values: KustoFacts.ChartYSplit),
            new QueryOperatorParameter("ymin", QueryOperatorParameterKind.NumericLiteral),
            new QueryOperatorParameter("ymax", QueryOperatorParameterKind.NumericLiteral),
        };

        public static readonly IReadOnlyList<QueryOperatorParameter> ReduceParameters = new[]
        {
            new QueryOperatorParameter(SyntaxFacts.GetText(SyntaxKind.KindKeyword), QueryOperatorParameterKind.Word, values: new [] { "source" })
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> ReduceWithParameters = new[]
        {
            new QueryOperatorParameter(SyntaxFacts.GetText(SyntaxKind.ThresholdKeyword), QueryOperatorParameterKind.NumericLiteral),
            new QueryOperatorParameter(SyntaxFacts.GetText(SyntaxKind.CharactersKeyword), QueryOperatorParameterKind.StringLiteral)
        }.ToReadOnly();

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
            new QueryOperatorParameter("kind", QueryOperatorParameterKind.Word, values: KustoFacts.ScanOperatorKinds),
            new QueryOperatorParameter(KustoFacts.ScanOperatorWithMatchIdProperty, QueryOperatorParameterKind.NameDeclaration),
            new QueryOperatorParameter(KustoFacts.ScanOperatorWithStepNameProperty, QueryOperatorParameterKind.NameDeclaration)
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> SearchParameters = new QueryOperatorParameter[]
        {
            new QueryOperatorParameter(SyntaxFacts.GetText(SyntaxKind.KindKeyword), QueryOperatorParameterKind.Word, values: KustoFacts.SearchKinds)
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> SerializedParameters = new QueryOperatorParameter[]
        {
            // no known parameters
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> SortParameters = new QueryOperatorParameter[]
        {
            new QueryOperatorParameter(SyntaxFacts.GetText(SyntaxKind.HintDotStrategyKeyword), QueryOperatorParameterKind.Word, values:KustoFacts.OrderByHintStrategies)
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> SummarizeParameters = new QueryOperatorParameter[]
        {
            new QueryOperatorParameter(SyntaxFacts.GetText(SyntaxKind.HintDotShuffleKeyKeyword), QueryOperatorParameterKind.Column, isRepeatable: true),
            new QueryOperatorParameter(SyntaxFacts.GetText(SyntaxKind.HintDotStrategyKeyword), QueryOperatorParameterKind.Word, values: KustoFacts.SummarizeHintStrategies),
            new QueryOperatorParameter(SyntaxFacts.GetText(SyntaxKind.HintDotNumPartitions), QueryOperatorParameterKind.IntegerLiteral, isRepeatable: false),
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> TakeParameters = new QueryOperatorParameter[]
        {
            // no known parameters
        }.ToReadOnly();

        public static readonly IReadOnlyList<QueryOperatorParameter> TopParameters = new QueryOperatorParameter[]
        {
            new QueryOperatorParameter(SyntaxFacts.GetText(SyntaxKind.HintDotProgressiveTopKeyword), QueryOperatorParameterKind.BoolLiteral)
        }.ToReadOnly();

        public static readonly QueryOperatorParameter ToScalarKindParameter =
            new QueryOperatorParameter(SyntaxFacts.GetText(SyntaxKind.KindKeyword), QueryOperatorParameterKind.Word, values: KustoFacts.ToScalarKinds).Hide();

        public static readonly QueryOperatorParameter ToTableKindParameter =
            new QueryOperatorParameter(SyntaxFacts.GetText(SyntaxKind.KindKeyword), QueryOperatorParameterKind.Word, values: KustoFacts.ToTableKinds).Hide();

        public static readonly IReadOnlyList<QueryOperatorParameter> UnionParameters = new QueryOperatorParameter[]
        {
            new QueryOperatorParameter("kind", QueryOperatorParameterKind.Word, values: KustoFacts.UnionKinds),
            new QueryOperatorParameter("withsource", QueryOperatorParameterKind.NameDeclaration, aliases: new [] { "with_source" }),
            new QueryOperatorParameter(SyntaxFacts.GetText(SyntaxKind.IsFuzzyKeyword), QueryOperatorParameterKind.BoolLiteral),
            new QueryOperatorParameter("hint.concurrency", QueryOperatorParameterKind.NumericLiteral),
            new QueryOperatorParameter("hint.spread", QueryOperatorParameterKind.NumericLiteral)
        }.ToReadOnly();
    }

    public enum QueryOperatorParameterKind
    {
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
        ColumnList
    }

    /// <summary>
    /// Describes a parameter that a query operator may have.
    /// </summary>
    public class QueryOperatorParameter
    {
        /// <summary>
        /// The name of the parameter.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The kind that the value can take.
        /// </summary>
        public QueryOperatorParameterKind Kind { get; }

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
        /// True if the parameters is hidden from intellisense.
        /// </summary>
        public bool IsHidden { get; }

        /// <summary>
        /// Any additional names that the parameter can be referenced by.
        /// </summary>
        public IReadOnlyList<string> Aliases { get; }

        private QueryOperatorParameter(
            string name, 
            QueryOperatorParameterKind kind, 
            bool isCaseSensitive, 
            IEnumerable<string> values, 
            bool isRepeatable, 
            bool isHidden,
            IReadOnlyList<string> aliases)
        {
            this.Name = name;
            this.Kind = kind;
            this.Values = values.ToReadOnly();
            this.IsCaseSensitive = isCaseSensitive;
            this.IsRepeatable = isRepeatable;
            this.IsHidden = isHidden;
            this.Aliases = aliases.ToReadOnly();
        }

        public QueryOperatorParameter(
            string name, 
            QueryOperatorParameterKind kind, 
            bool caseSensitive = true, 
            IEnumerable<string> values = null, 
            bool isRepeatable = false,
            IReadOnlyList<string> aliases = null)
            : this(name, kind, caseSensitive, values, isRepeatable, false, aliases)
        {
        }

        public QueryOperatorParameter WithIsHidden(bool isHidden)
        {
            return new QueryOperatorParameter(this.Name, this.Kind, this.IsCaseSensitive, this.Values, this.IsRepeatable, isHidden, this.Aliases);
        }

        public QueryOperatorParameter Hide() => WithIsHidden(true);
    }
}