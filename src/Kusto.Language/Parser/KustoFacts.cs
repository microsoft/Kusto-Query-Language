using System.Linq;
using System.Collections.Generic;

namespace Kusto.Language
{
    using Binding;
    using Parsing;
    using Syntax;
    using Symbols;
    using System;
    using System.Text;

    public static partial class KustoFacts
    {
        /// <summary>
        /// Types allowed for function parameters and columns.
        /// </summary>
        public static readonly IReadOnlyList<string> ParamTypes = new string[]
        {
            "bool",
            "boolean",
            "date",
            "datetime",
            "decimal",
            "double",
            "dynamic",
            "guid",
            "int",
            "int64",
            "int8",
            "long",
            "real",
            "string",
            "time",
            "timespan",
            "uniqueid"
        };

        /// <summary>
        /// Extended type allowed in some schema declarations.
        /// </summary>
        public static readonly IReadOnlyList<string> ExtendedParamTypes = new string[]
        {
            "bool",
            "boolean",
            "date",
            "datetime",
            "decimal",
            "double",
            "dynamic",
            "float",
            "guid",
            "int",
            "int16",
            "int32",
            "int64",
            "int8",
            "long",
            "real",
            "decimal",
            "string",
            "time",
            "timespan",
            "uint",
            "uint16",
            "uint32",
            "uint64",
            "uint8",
            "ulong",
            "uniqueid"
        };

        public static readonly IReadOnlyList<string> StorageTypes =
            ExtendedParamTypes;


        private static IReadOnlyList<string> _knownQueryOperatorParameterNames;

        public static IReadOnlyList<string> KnownQueryOperatorParameterNames
        {
            get
            {
                // defer calculation of this list to avoid cycle with QueryOperatorParameters
                if (_knownQueryOperatorParameterNames == null)
                {
                    _knownQueryOperatorParameterNames =
                        QueryOperatorParameters.AllParameters
                            .SelectMany(p => new[] { p.Name }.Concat(p.Aliases))
                            .OrderBy(n => n)
                            .ToArray();
                }

                return _knownQueryOperatorParameterNames;
            }
        }

        public static readonly IReadOnlyList<string> ChartTypes = new string[]
        {
            "table", "list", "barchart", "piechart", "ladderchart", "timechart", "linechart", "anomalychart", "pivotchart", "areachart",
            "stackedareachart", "scatterchart", "timepivot", "columnchart", "timeline", "3Dchart", "card", "treemap", "plotly", "graph", "sankey"
        };

        /// <summary>
        /// Chart types not shown in intellisense
        /// </summary>
        public static readonly IReadOnlyList<string> HiddenChartTypes = new string[]
        {
            "3Dchart",
            "sankey", // TODO: unhide it once service-side supports this
            "graph" // TODO: unhide it once service-side supports this
        };

        /// <summary>
        /// Char types shown in intellisense
        /// </summary>
        public static readonly IReadOnlyList<string> VisibleChartTypes =
            ChartTypes.Where(c => !HiddenChartTypes.Contains(c)).ToArray();

        public static readonly IReadOnlyList<string> ChartProperties = new string[]
        {
            "title", "xcolumn", "series", "ycolumns", "anomalycolumns", "kind", "xtitle", "ytitle", "xaxis", "yaxis", "legend", "ysplit", "accumulate", "ymin", "ymax", "xmax", "xmin"
        };

        public static readonly IReadOnlyList<string> ChartKinds = new string[]
        {
            "default", "unstacked", "stacked", "stacked100", "map"
        };

        public static readonly IReadOnlyList<string> ChartLegends = new string[]
        {
            "visible", "hidden"
        };

        public static readonly IReadOnlyList<string> ChartAxis = new string[]
        {
            "linear", "log"
        };

        public static readonly IReadOnlyList<string> ChartYSplit = new string[]
        {
            "none", "axes", "panels"
        };

        public static readonly IReadOnlyList<string> HintDistributions = new string[]
        {
            "single", "per_node", "per_shard", "default"
        };

        public static readonly IReadOnlyList<string> HintRemotes = new string[]
        {
            "auto", "local"
        };

        public static readonly IReadOnlyList<string> HintStrategies = new string[]
        {
            "auto", "broadcast", "centralized", "shuffle"
        };

        public static readonly IReadOnlyList<string> HintSpreads = new string[]
        {
            "auto", "local"
        };

        public static readonly IReadOnlyList<string> HintConcurrencies = new string[]
        {
            "auto", "left", "local", "right", "unresolved"
        };

        public static readonly IReadOnlyList<string> DistinctHintStrategies = HintStrategies;

        public static readonly IReadOnlyList<string> EvaluateHintDistributions = HintDistributions;
        public static readonly IReadOnlyList<string> EvaluateHintRemotes = HintRemotes;

        public static readonly IReadOnlyList<string> JoinKinds = new string[]
        {
            "inner", "fullouter", "innerunique", "leftanti", "leftantisemi", "anti",
            "leftouter", "leftsemi", "rightanti", "rightantisemi", "rightouter", "rightsemi"
        };

        public static readonly IReadOnlyList<string> GraphMarkComponentsKinds = new string[]
        {
           "weak", "strong"
        };

        public static readonly IReadOnlyList<string> JoinHintRemotes = new string[]
        {
            "auto", "left", "local", "right", "unresolved"
        };

        public static readonly IReadOnlyList<string> JoinHintStrategies = HintStrategies;

        public static readonly IReadOnlyList<string> LookupKinds = new string[]
        {
            "inner", "leftouter"
        };

        public static readonly IReadOnlyList<string> MakeSeriesKinds = new string[]
        {
            "nonempty"
        };

        public static readonly IReadOnlyList<string> MvExpandKinds = new string[]
        {
            "bag", "array"
        };

        public static readonly IReadOnlyList<string> InlineExternalTableKinds = new string[]
        {
            "storage"
        };

        public static readonly IReadOnlyList<string> InlineExternalTableDataFormats = new string[]
        {
            "parquet", "avro", "csv", "tsv", "json", "orc", "txt"
        };

        public static readonly IReadOnlyList<string> InlineExternalTablePartitionColumnFunctions = new string[]
        {
            "hash", "bin", "startofday", "startofweek", "startofmonth", "startofyear"
        };

        public static readonly IReadOnlyList<string> PartitionHintConcurrencies = HintConcurrencies;
        public static readonly IReadOnlyList<string> PartitionHintSpreads = HintSpreads;

        public static readonly IReadOnlyList<string> PartitionHintStrategies = new string[]
        {
            "shuffle", "native", "legacy"
        };

        public static readonly IReadOnlyList<string> PartitionedGraphMakeHintStrategies = new string[]
        {
            "shuffle", "native"
        };

        public static readonly IReadOnlyList<string> ReduceByKinds = new string[]
        {
            "source"
        };

        public static readonly IReadOnlyList<string> SearchKinds = new string[]
        {
            "default", "case_insensitive", "case_sensitive"
        };

        public static readonly IReadOnlyList<string> SortHintStrategies = new string[]
        {
            "splitBlock", "multipleBlocks"
        };

        public static readonly IReadOnlyList<string> SummarizeHintStrategies = new string[]
        {
            "shuffle"
        };

        public static readonly IReadOnlyList<string> UnionWithSourceProperties = new string[] {
            "withsource", "with_source"
        };

        public static string UnionIsFuzzyProperty => "isfuzzy";

        public static readonly IReadOnlyList<string> UnionKinds = new string[]
        {
            "inner", "outer"
        };

        public static readonly IReadOnlyList<string> CyclesKinds = new string[]
        {
            "none", "all", "unique_edges"
        };

        public static readonly IReadOnlyList<string> ShortestPathsOutputs = new string[]
        {
            "any", "all"
        };

        public static readonly IReadOnlyList<string> UnionHintConcurrencies = HintConcurrencies;
        public static readonly IReadOnlyList<string> UnionHintSpreads = HintSpreads;

        public static readonly IReadOnlyList<string> ParseKinds = new string[]
        {
            "simple", "regex", "relaxed"
        };

        public static readonly IReadOnlyList<string> DataScopeValues = new string[]
        {
            "all", "hotcache"
        };

        public static readonly IReadOnlyList<string> ScanKinds = new string[]
        {
            "partial",
            "full"
        };

        public static readonly IReadOnlyList<string> ScanStepOutputValues = new string[]
        {
            "all", "last", "none"
        };

        public static readonly IReadOnlyList<string> ToScalarKinds = new string[]
        {
            "nooptimization"
        };

        public static readonly IReadOnlyList<string> ToTableKinds = new string[]
        {
            "nooptimization"
        };

        public static readonly IReadOnlyList<string> LimitExamples = new string[]
        {
            "10", "100", "1000"
        };

        public static readonly IReadOnlyList<string> TopExamples = new string[]
        {
            "10", "100", "1000"
        };

        public static readonly IReadOnlyList<string> AgoExamples = new string[]
        {
            "30min", "1h", "1d"
        };

        public static readonly IReadOnlyList<string> KnownInternalFunctionNames = new string[]
        {
            "__blackbox",
            "__box",
            "__cast",
            "__columnifexists",
            "__const_cast",
            "__fetch_contextual_scalar_value",
            "__geo_line_validate",
            "__geo_polygon_validate",
            "__getobject",
            "__getobjectex",
            "__has_ipv4",
            "__has_ipv4_prefix",
            "__hash_crc32",
            "__hash_djb2",
            "__hash_many_crc32",
            "__hash_xxxh64",
            "__invoke",
            "__is_scalar",
            "__lz4_compress_dynamic_array_to_base64_string",
            "__make_const",
            "__null",
            "__null_as",
            "__object",
            "__query_parameter",
            "__regex_complexity",
            "__row_offset",
            "__rowstore_ref",
            "__search_wildcard",
            "__search_wildcard_explicit_cols",
            "__search_wildcard_explicit_cols_v35",
            "__shard_record_position",
            "__sql_add",
            "__sql_divide",
            "__sql_modulo",
            "__sql_multiply",
            "__sql_subtract",
            "__trace_information",
            "__var",
            "__warning",
            "__get_scalar",
        };

        public static readonly IReadOnlyList<string> DateTimeParts = new string[]
        {
            "year",
            "quarter",
            "month",
            "week_of_year",
            "day",
            "dayofyear",
            "hour",
            "minute",
            "second",
            "millisecond",
            "microsecond",
            "nanosecond"
        };

        public static readonly IReadOnlyList<string> DateDiffParts = new[]
        {
            "year",
            "quarter",
            "month",
            "week",
            "day",
            "hour",
            "minute",
            "second",
            "millisecond",
            "microsecond",
            "nanosecond"
        };

        /// <summary>
        /// Directive names possibly supported by the client.
        /// </summary>
        public static readonly IReadOnlyList<string> Directives = new[]
        {
            "automate",
            "browse",       // open browser to link
            "connect",      // set connection for script or query
            "crp",          // set client request properties for script or query
            "database",     // set default database (and cluster) for query
            "download",
            "qp",           // set query parameters for script or query
            "query",
            "run",          // execute expression (via print operator)
            "save",         // run query & save results to local file
            "sqr",          // run query & save results to server
            "truesight",
            "upload",
            "welcome"       // show welcome message
        };

        /// <summary>
        /// Keywords that can be used as identifiers everywhere.
        /// </summary>
        public static readonly IReadOnlyList<SyntaxKind> KeywordsAsIdentifiers =
            SyntaxFacts.GetKindsWithFixedText().Where(k => k.IsKeyword() && k.CanBeIdentifier()).ToArray();

        /// <summary>
        /// Keywords that can be used as identifiers in some additional locations in queries.
        /// </summary>
        public static readonly IReadOnlyList<SyntaxKind> ExtendedKeywordsAsIdentifiers =
            KeywordsAsIdentifiers.Concat(
                new SyntaxKind[]
                {
                    SyntaxKind.AccumulateKeyword,
                    SyntaxKind.AsKeyword,
                    SyntaxKind.ByKeyword,
                    SyntaxKind.ContainsKeyword,
                    SyntaxKind.ConsumeKeyword,
                    SyntaxKind.CountKeyword,
                    SyntaxKind.DataTableKeyword,
                    SyntaxKind.DistinctKeyword,
                    SyntaxKind.EarliestKeyword,
                    SyntaxKind.ExtendKeyword,
                    SyntaxKind.ExternalDataKeyword,
                    SyntaxKind.InlineExternalTableKeyword,
                    SyntaxKind.DataFormatKeyword,
                    SyntaxKind.DateTimePatternKeyword,
                    // FALSE?? How can this be a keyword it is already a literal 
                    SyntaxKind.FindKeyword,
                    SyntaxKind.FilterKeyword,
                    SyntaxKind.HasKeyword,
                    SyntaxKind.InKeyword,
                    SyntaxKind.InvokeKeyword,
                    SyntaxKind.LatestKeyword,
                    SyntaxKind.LimitKeyword,
                    SyntaxKind.MaterializeKeyword,
                    SyntaxKind.MdmKeyword,
                    SyntaxKind.OfKeyword,
                    SyntaxKind.ParseKeyword,
                    SyntaxKind.PlotlyKeyword,
                    SyntaxKind.PrintKeyword,
                    SyntaxKind.SampleKeyword,
                    SyntaxKind.SampleDistinctKeyword,
                    SyntaxKind.ScanKeyword,
                    SyntaxKind.SearchKeyword,
                    SyntaxKind.SerializeKeyword,
                    SyntaxKind.SetKeyword,
                    SyntaxKind.SortKeyword,
                    SyntaxKind.SqlKeyword,
                    SyntaxKind.SummarizeKeyword,
                    SyntaxKind.TakeKeyword,
                    SyntaxKind.TitleKeyword,
                    SyntaxKind.ToKeyword,
                    SyntaxKind.TopKeyword,
                    SyntaxKind.ToScalarKeyword,
                    SyntaxKind.ToTableKeyword,
                    SyntaxKind.TopNestedKeyword,
                    SyntaxKind.TopHittersKeyword,
                    SyntaxKind.VerboseKeyword,
                    SyntaxKind.ViewersKeyword,
                    SyntaxKind.WhereKeyword
                })
            .ToArray();

        /// <summary>
        /// Keywords that can be used as identifiers in some distinct locations in queries.
        /// </summary>
        public static readonly IReadOnlyList<SyntaxKind> SpecialKeywordsAsIdentifiers = new SyntaxKind[]
        {
            SyntaxKind.KindKeyword,
            SyntaxKind.WithSourceKeyword,
            SyntaxKind.With_SourceKeyword
        };

        public static readonly IReadOnlyList<SyntaxKind> ForkOperatorKinds = new SyntaxKind[]
        {
            SyntaxKind.AsOperator,
            SyntaxKind.CountOperator,
            SyntaxKind.DistinctOperator,
            SyntaxKind.ExecuteAndCacheOperator,
            SyntaxKind.ExtendOperator,
            SyntaxKind.FilterOperator,
            SyntaxKind.InvokeOperator,
            SyntaxKind.MvExpandOperator,
            SyntaxKind.ParseOperator,
            SyntaxKind.ParseWhereOperator,
            SyntaxKind.ParseKvOperator,
            SyntaxKind.ProjectOperator,
            SyntaxKind.ProjectAwayOperator,
            SyntaxKind.ProjectByNamesOperator,
            SyntaxKind.ProjectKeepOperator,
            SyntaxKind.ProjectRenameOperator,
            SyntaxKind.ProjectReorderOperator,
            SyntaxKind.ReduceByOperator,
            SyntaxKind.SampleDistinctOperator,
            SyntaxKind.SampleOperator,
            SyntaxKind.SortOperator,
            SyntaxKind.SummarizeOperator,
            SyntaxKind.TakeOperator,
            SyntaxKind.TopHittersOperator,
            SyntaxKind.TopNestedOperator,
            SyntaxKind.TopOperator
        };

        /// <summary>
        /// Query operators that can come after a pipe 
        /// </summary>
        public static readonly IReadOnlyList<SyntaxKind> PostPipeOperatorKinds = new SyntaxKind[]
        {
            SyntaxKind.AsOperator,
            SyntaxKind.ConsumeOperator,
            SyntaxKind.CountOperator,
            SyntaxKind.DistinctOperator,
            SyntaxKind.EvaluateOperator,
            SyntaxKind.ExecuteAndCacheOperator,
            SyntaxKind.ExtendOperator,
            SyntaxKind.FacetOperator,
            SyntaxKind.FilterOperator,
            SyntaxKind.FindOperator,
            SyntaxKind.ForkOperator,
            SyntaxKind.GetSchemaOperator,
            SyntaxKind.GraphMatchOperator,
            SyntaxKind.GraphToTableOperator,
            SyntaxKind.InvokeOperator,
            SyntaxKind.JoinOperator,
            SyntaxKind.LookupOperator,
            SyntaxKind.MakeSeriesOperator,
            SyntaxKind.MakeGraphOperator,
            SyntaxKind.MvApplyOperator,
            SyntaxKind.MvExpandOperator,
            SyntaxKind.ParseOperator,
            SyntaxKind.ParseWhereOperator,
            SyntaxKind.ParseKvOperator,
            SyntaxKind.PartitionOperator,
            SyntaxKind.PartitionByOperator,
            SyntaxKind.ProjectAwayOperator,
            SyntaxKind.ProjectByNamesOperator,
            SyntaxKind.ProjectKeepOperator,
            SyntaxKind.ProjectOperator,
            SyntaxKind.ProjectRenameOperator,
            SyntaxKind.ProjectReorderOperator,
            SyntaxKind.ReduceByOperator,
            SyntaxKind.RenderOperator,
            SyntaxKind.SampleDistinctOperator,
            SyntaxKind.SampleOperator,
            SyntaxKind.ScanOperator,
            SyntaxKind.SearchOperator,
            SyntaxKind.SerializeOperator,
            SyntaxKind.SummarizeOperator,
            SyntaxKind.TakeOperator,
            SyntaxKind.SortOperator,
            SyntaxKind.TopHittersOperator,
            SyntaxKind.TopOperator,
            SyntaxKind.TopNestedOperator,
            SyntaxKind.UnionOperator
        };

        /// <summary>
        /// True if the query operator is on the right side of a pipe expression
        /// or is in a context that allows operators that would normally only appear
        /// on the right side of a pipe expression.
        /// </summary>
        public static bool HasPipedInput(QueryOperator queryOp)
        {
            return (queryOp.Parent is PipeExpression pe && pe.Operator == queryOp)
                || IsChildOfPipeStartingExpression(queryOp);
        }

        private static bool IsChildOfPipeStartingExpression(Expression expr)
        {
            return (expr.Parent is ForkExpression fce && fce.Expression == expr)
                || (expr.Parent is PartitionSubquery ps && ps.Subquery == expr)
                || (expr.Parent is MvApplySubqueryExpression mvas && mvas.Expression == expr)
                || (expr.Parent is PartitionByOperator pbo && pbo.Subquery == expr)
                || (expr.Parent is MakeGraphPartitionedByClause mgpb && mgpb.Subquery == expr)
                || (expr.Parent is FacetWithExpressionClause fwce && fwce.Expression == expr)
                || (expr.Parent is FacetWithOperatorClause fwoc && fwoc.Operator == expr)
                || (expr.Parent is Expression pe && IsChildOfPipeStartingExpression(pe))
                || (expr.Parent is MaterializedViewCombineClause mvc && mvc.Parent is MaterializedViewCombineExpression mve && mve.AggregationsClause == mvc);
        }

        /// <summary>
        /// True if the text can be used as an identifier everywhere in the specified language dialect.
        /// </summary>
        public static bool CanBeIdentifier(string text, KustoDialect dialect)
        {
            // bad input is bad
            if (string.IsNullOrEmpty(text))
                return false;

            // disallow some otherwise legal identifiers that start with numbers
            if (TextFacts.IsDigit(text[0]))
                return false;

            // does it even look like an identifier?
            if (TokenParser.ScanIdentifier(text) != text.Length)
                return false;

            // is it actually a boolean literal?
            if (TokenParser.ScanBooleanLiteral(text) == text.Length)
                return false;

            // depends on the dialect
            switch (dialect)
            {
                case KustoDialect.ClusterManagerCommand:
                case KustoDialect.DataManagerCommand:
                    // always require brackets to avoid needing to maintain the correct list of keywords
                    return false;

                case KustoDialect.EngineCommand:
                    return !s_engineCommandKeywordsThatNeedBrackets.Contains(text);

                case KustoDialect.Query:
                    if (SyntaxFacts.TryGetKind(text, out var kind)
                        && kind.IsKeyword()
                        && !kind.CanBeIdentifier())
                    {
                        return false;
                    }
                    break;
            }

            return true;
        }

        /// <summary>
        /// True if the text can be used as an identifier everywhere in Kusto queries.
        /// </summary>
        public static bool CanBeIdentifier(string text) =>
            CanBeIdentifier(text, KustoDialect.Query);

        /// <summary>
        /// Adds bracketting and quoting to the name if it cannot be an identifier in the specified language dialect.
        /// </summary>
        public static string BracketNameIfNecessary(string name, KustoDialect dialect)
        {
            if (!CanBeIdentifier(name, dialect))
            {
                return GetBracketedName(name);
            }

            return name;
        }

        /// <summary>
        /// Adds bracketting and quoting to the name if it cannot be an identifier everywhere in Kusto queries.
        /// </summary>
        public static string BracketNameIfNecessary(string name) =>
            BracketNameIfNecessary(name, KustoDialect.Query);

        /// <summary>
        /// Convert name to bracketed form: name -> ['name']
        /// </summary>
        public static string GetBracketedName(string name)
        {
            return "[" + GetStringLiteral(name) + "]";
        }

        /// <summary>
        /// Gets the single-quoted escaped string literal for the text,
        /// unless it contains a single quote, and then get the double-quoted escaped string literal.
        /// </summary>
        public static string GetStringLiteral(string text)
        {
            if (text.Contains("'"))
            {
                return GetDoubleQuotedStringLiteral(text);
            }
            else
            {
                return GetSingleQuotedStringLiteral(text);
            }
        }

        /// <summary>
        /// Gets the single-quoted escaped string literal for the text.
        /// </summary>
        public static string GetSingleQuotedStringLiteral(string text)
        {
            return "'" + GetEscapedString(text, singleQuoteStringEscapes) + "'";
        }

        /// <summary>
        /// Gets the double-quoted escaped string literal for the text.
        /// </summary>
        public static string GetDoubleQuotedStringLiteral(string text)
        {
            return "\"" + GetEscapedString(text, doubleQuoteStringEscapes) + "\"";
        }

        /// <summary>
        /// Gets the multi-line quoted string literal for the text.
        /// </summary>
        public static string GetMultiLineStringLiteral(string text)
        {
            return MultiLineStringQuote + text + MultiLineStringQuote;
        }

        private static Dictionary<char, string> singleQuoteStringEscapes =
            new Dictionary<char, string> {
                { '\'', @"\'" },
                { '\\', @"\\" },
                { '\a', @"\a" },
                { '\b', @"\b" },
                { '\f', @"\f" },
                { '\n', @"\n" },
                { '\r', @"\r" },
                { '\t', @"\t" } };

        private static Dictionary<char, string> doubleQuoteStringEscapes =
            new Dictionary<char, string> {
                { '"', @"\""" },
                { '\\', @"\\" },
                { '\a', @"\a" },
                { '\b', @"\b" },
                { '\f', @"\f" },
                { '\n', @"\n" },
                { '\r', @"\r" },
                { '\t', @"\t" } };

        private static string GetEscapedString(string text, IReadOnlyDictionary<char, string> escapes)
        {
            var builder = new StringBuilder();

            foreach (var ch in text)
            {
                if (escapes.TryGetValue(ch, out var escape))
                {
                    builder.Append(escape);
                }
                else
                {
                    builder.Append(ch);
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Gets the column name used for an expression in a projection.
        /// </summary>
        public static string GetExpressionResultName(Expression expr, string defaultName = "", TableSymbol rowScope = null)
        {
            return Binder.GetExpressionResultName(expr, defaultName, rowScope);
        }

        /// <summary>
        /// Gets the content value of a string literal
        /// </summary>
        public static string GetStringLiteralValue(string literal)
        {
            int start = 0;
            int end = literal.Length;
            var isVerbatim = false;

            if (end == 0)
            {
                return string.Empty;
            }

            // do not include H prefix in value
            if (literal[0] == 'h' || literal[0] == 'H')
            {
                start++;
            }

            if (start < literal.Length && literal[start] == '@')
            {
                start++; // do not include @ prefix in value
                isVerbatim = true;
            }

            // check for multi-line string
            if (TryParseMultiLineStringLiteral(start, literal, out var multiLineLiteral))
            {
                return multiLineLiteral;
            }

            if (start >= literal.Length)
                return string.Empty;

            var startQuote = literal[start];
            var bracketed = startQuote == '[';
            var endQuote = bracketed ? ']' : startQuote;

            start++; // do not include quote in value

            if (end > 0 && literal[end - 1] == endQuote)
                end--; // do not include end quote in value

            if (end <= start)
            {
                return string.Empty;
            }
            else if (!isVerbatim && !bracketed && literal.IndexOf('\\', start) >= start)
            {
                return DecodeEscapes(literal, start, end - start);
            }
            else if (isVerbatim && !bracketed && HasInteriorQuote(literal, start, end, endQuote))
            {
                return DecodeDoubleQuotes(literal, start, end - start, endQuote);
            }
            else if (start > 0 || end < literal.Length)
            {
                return literal.Substring(start, end - start);
            }
            else
            {
                return literal;
            }
        }

        /// <summary>
        /// The quote text at the start and end of a multi-line string.
        /// </summary>
        public static readonly string MultiLineStringQuote = "```";

        /// <summary>
        /// Alternate quote for multi-line strings, to be depreciated.
        /// </summary>
        public static readonly string AlternateMultiLineStringQuote = "~~~";

        private static bool TryParseMultiLineStringLiteral(int start, string text, out string literal)
        {
            return TryParseMultiLineStringLiteral(start, text, MultiLineStringQuote, out literal)
                || TryParseMultiLineStringLiteral(start, text, AlternateMultiLineStringQuote, out literal);
        }

        private static bool TryParseMultiLineStringLiteral(int start, string text, string quote, out string literal)
        {
            // check for multi-line string
            if (start + quote.Length < text.Length
                && string.Compare(text, start, quote, 0, quote.Length, StringComparison.Ordinal) == 0)
            {
                var twiceQuoteLen = quote.Length << 1;
                if (text.Length - start >= twiceQuoteLen && text.EndsWith(quote, StringComparison.Ordinal))
                {
                    literal = text.Substring(start + quote.Length, text.Length - twiceQuoteLen - start);
                    return true;
                }
                else
                {
                    literal = text.Substring(start + quote.Length);
                    return true;
                }
            }

            literal = null;
            return false;
        }

        private static bool HasInteriorQuote(string text, int start, int end, char quote)
        {
            var position = text.IndexOf(quote);
            return position >= 0 && position < end;
        }

        private static string DecodeEscapes(string text, int start, int length)
        {
            var builder = new StringBuilder();

            int i = start;
            int end = start + length;
            while (i < end)
            {
                var ch = text[i];
                if (ch == '\\' && i + 1 < end)
                {
                    var ch2 = text[i + 1];
                    switch (ch2)
                    {
                        case '\'':
                        case '"':
                        case '\\':
                            builder.Append(ch2);
                            i += 2;
                            break;
                        case 'a':
                            builder.Append('\a');
                            i += 2;
                            break;
                        case 'b':
                            builder.Append('\b');
                            i += 2;
                            break;
                        case 'f':
                            builder.Append('\f');
                            i += 2;
                            break;
                        case 'n':
                            builder.Append('\n');
                            i += 2;
                            break;
                        case 'r':
                            builder.Append('\r');
                            i += 2;
                            break;
                        case 't':
                            builder.Append('\t');
                            i += 2;
                            break;
                        case 'v':
                            builder.Append('\v');
                            i += 2;
                            break;
                        case 'u':
                            // 4 char hex encoded character
                            i += 2;
                            builder.Append((char)DecodeHex(text, 4, ref i));
                            break;
#if !BRIDGE
                        case 'U':
                            // 8 char hex encoded character (two surrogate pairs)
                            i += 2;
                            var hex = DecodeHex(text, 8, ref i);
                            var converted = char.ConvertFromUtf32(hex);
                            builder.Append(converted);
                            break;
#endif
                        case 'x':
                            // 2 char hex encoded character (not same as C#)
                            i += 2;
                            builder.Append((char)DecodeHex(text, 2, ref i));
                            break;
                        default:
                            if (char.IsDigit(ch2))
                            {
                                // octal encoded character
                                i++;
                                builder.Append((char)DecodeOctal(text, 3, ref i));
                            }
                            else
                            {
                                // just this character?
                                i += 2;
                                builder.Append(ch2);
                            }
                            break;
                    }
                }
                else
                {
                    builder.Append(ch);
                    i++;
                }
            }

            return builder.ToString();
        }

        private static int DecodeOctal(string text, int length, ref int index)
        {
            int value = 0;
            int count = 0;

            for (; count < 3 && index < text.Length && char.IsDigit(text[index]); count++, index++)
            {
                value = (value << 3) + (text[index] - '0');
            }

            return value;
        }

        private static int DecodeHex(string text, int length, ref int index)
        {
            int value = 0;
            int count = 0;

            for (; count < length && index < text.Length; count++, index++)
            {
                var ch = text[index];

                if (ch >= 'a' && ch <= 'f')
                {
                    value = (value << 4) + (ch - 'a' + 10);
                }
                else if (ch >= 'A' && ch <= 'F')
                {
                    value = (value << 4) + (ch - 'A' + 10);
                }
                else if (ch >= '0' && ch <= '9')
                {
                    value = (value << 4) + (ch - '0');
                }
                else
                {
                    break;
                }
            }

            return value;
        }

        private static string DecodeDoubleQuotes(string text, int start, int length, char quote)
        {
            var builder = new StringBuilder();

            for (int i = start, end = start + length; i < end; i++)
            {
                var ch = text[i];

                if (ch == quote && i + 1 < end && text[i + 1] == quote)
                {
                    i++;
                }

                builder.Append(ch);
            }

            return builder.ToString();
        }

        private const string HostNamePrefix = "://";
        public static readonly string KustoWindowsNet = ".kusto.windows.net";

        /// <summary>
        /// Returns true if the name matches the host name.
        /// </summary>
        public static bool IsHostName(string name, string hostName)
        {
            return string.Compare(name, hostName, StringComparison.OrdinalIgnoreCase) == 0;
        }

        /// <summary>
        /// Gets the host name from a possible uri
        /// </summary>
        public static string GetHostName(string clusterUriOrName)
        {
            GetUriParts(
                clusterUriOrName, 
                out _,  // scheme
                out var hostname, 
                out _, // port
                out _, // path
                out _  // query
                );
            return hostname;
        }

        /// <summary>
        /// Gets the host and path names
        /// </summary>
        public static void GetHostAndPath(string clusterUriOrName, out string host, out string path)
        {
            GetUriParts(
                clusterUriOrName,
                out _, // scheme
                out host,
                out _, // port
                out path,
                out _  // query
                );
        }

        private static void GetUriParts(
            string uri, 
            out string scheme, 
            out string hostname, 
            out string port,
            out string path,
            out string query)
        {
            scheme = null;
            port = null;
            path = null;
            query = null;

            int hostnameStart = 0;

            int hostNamePrefixStart = uri.IndexOf(HostNamePrefix);
            if (hostNamePrefixStart > 0)
            {
                scheme = uri.Substring(0, hostNamePrefixStart);
                hostnameStart = hostNamePrefixStart + HostNamePrefix.Length;
            }

            // extract only permitted host name or IP value characters
            var pos = hostnameStart;
            while (pos < uri.Length &&
                IsHostNameChar(uri[pos]))
            {
                pos++;
            }

            if (hostnameStart == 0 && pos == uri.Length)
            {
                hostname = uri;
            }
            else
            {
                hostname = uri.Substring(hostnameStart, pos - hostnameStart);
            }

            if (pos < uri.Length && uri[pos] == ':')
            {
                pos++;
                var portStart = pos;
                while (pos < uri.Length && char.IsDigit(uri[pos]))
                {
                    pos++;
                }

                if (pos > portStart)
                {
                    port = uri.Substring(portStart, pos - portStart);
                }
            }

            if (pos < uri.Length && uri[pos] == '/')
            {
                pos++;
                var pathStart = pos;
                while (pos < uri.Length && uri[pos] != '?')
                {
                    pos++;
                }

                if (pos > pathStart)
                {
                    path = uri.Substring(pathStart, pos - pathStart);
                }
            }

            if (pos < uri.Length && uri[pos] == '?')
            {
                query = uri.Substring(pos);
            }
        }

        /// <summary>
        /// Returns true if the character is a legal part of a host name or IP address.
        /// </summary>
        private static bool IsHostNameChar(char ch)
        {
            return char.IsLetter(ch) || char.IsDigit(ch) || ch == '-' || ch == '.' || ch == '_';
        }

        /// <summary>
        /// Gets the full name of a host given the short or full name.
        /// </summary>
        public static string GetFullHostName(string hostname, string defaultDomainSuffix)
        {
            if (IsPossibleShortHostName(hostname)
                && !string.IsNullOrEmpty(defaultDomainSuffix)
                && defaultDomainSuffix[0] == '.')
            {
                return hostname + defaultDomainSuffix;
            }
            else
            {
                return hostname;
            }
        }

        /// <summary>
        /// Returns true if the hostname has a short name
        /// </summary>
        public static bool HasShortHostName(string hostname, string defaultDomainSuffix)
        {
            // the full name has a short name, if it is in the default domain and the remaining prefix qualifies as a short name.
            return !string.IsNullOrEmpty(defaultDomainSuffix)
                && defaultDomainSuffix[0] == '.'
                && hostname.EndsWith(defaultDomainSuffix, StringComparison.OrdinalIgnoreCase)
                && hostname.Length > defaultDomainSuffix.Length
                && IsPossibleShortHostName(hostname, 0, hostname.Length - defaultDomainSuffix.Length);
        }

        /// <summary>
        /// Returns true if the name is the short name of the host name.
        /// </summary>
        public static bool IsShortHostName(string name, string hostName, string defaultDomainSuffix)
        {
            // supplied name is the first part of xxx.YYY.ZZZ or xxx.xxx.YYY.ZZZ
            if (!HasShortHostName(hostName, defaultDomainSuffix))
                return false;

            // the hostname should be the combination of the short name and the default domain suffix
            if (name.Length + defaultDomainSuffix.Length != hostName.Length)
                return false;

            return hostName.StartsWith(name, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns true if the name is a possible short host name
        /// </summary>
        public static bool IsPossibleShortHostName(string name)
        {
            return IsPossibleShortHostName(name, 0, name.Length);
        }

        private static bool IsPossibleShortHostName(string name, int start, int len)
        {
            // short names can have zero or one dots:  name or name.region
            return !string.IsNullOrEmpty(name)
                && name[name.Length - 1] != '.'
                && CountDots(name, start, len) < 2;
        }

        private static int CountDots(string text, int start, int len)
        {
            int count = 0;

            for (int i = start, end = start + len; i < end; i++)
            {
                if (text[i] == '.')
                    count++;
            }

            return count;
        }

        /// <summary>
        /// Gets the short name given the full host name, if one exists or null if no short name exists
        /// </summary>
        public static string GetShortHostName(string hostName, string defaultDomainSuffix)
        {
            if (hostName != null && HasShortHostName(hostName, defaultDomainSuffix))
            {
                return hostName.Substring(0, hostName.Length - defaultDomainSuffix.Length);
            }

            return null;
        }

        /// <summary>
        /// True if the text matches the pattern (*, xxx*, *xxx, *xxx*, xxx*yyy, *xxx*yyy*, ...)
        /// The * represents any zero-or-more characters.
        /// </summary>
        public static bool Matches(string pattern, string text, bool ignoreCase = false)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));

            if (text == null)
                throw new ArgumentNullException(nameof(text));

            // empty pattern does not match anything
            if (pattern.Length == 0)
                return false;

            return Matches(pattern, 0, text, 0, ignoreCase);
        }

        private static bool Matches(string pattern, int patternSegmentStart, string text, int textPosition, bool ignoreCase)
        {
            var asteriskPosition = pattern.IndexOf('*', patternSegmentStart);
            var sawAsterisk = asteriskPosition >= 0;

            var patternSegmentEnd = sawAsterisk ? asteriskPosition : pattern.Length;

            // skip over adjacent asterisks
            while (sawAsterisk && asteriskPosition + 1 < pattern.Length && pattern[asteriskPosition + 1] == '*')
            {
                asteriskPosition++;
            }

            var nextPatternStart = sawAsterisk ? asteriskPosition + 1 : pattern.Length;
            var patternSegmentLength = patternSegmentEnd - patternSegmentStart;

            if (patternSegmentLength == 0)
            {
                if (patternSegmentStart >= pattern.Length)
                {
                    // no more pattern segments to match
                    return true;
                }
                else
                {
                    // this is the nothing pattern before the first asterisk
                    return Matches(pattern, nextPatternStart, text, textPosition, ignoreCase);
                }
            }
            else if (patternSegmentStart == 0)
            {
                if (!sawAsterisk)
                {
                    // this is fixed-pattern (no asterisks before or after), so must be exact match
                    return text.Length == patternSegmentLength
                        && string.Compare(text, 0, pattern, 0, patternSegmentLength, ignoreCase) == 0;
                }
                else
                {
                    // this is the first segment (no asterisk before) so its a starts-with pattern segment
                    if (patternSegmentLength > text.Length
                        || string.Compare(text, 0, pattern, 0, patternSegmentLength, ignoreCase) != 0)
                    {
                        return false;
                    }

                    return Matches(pattern, nextPatternStart, text, patternSegmentLength, ignoreCase);
                }
            }
            else if (!sawAsterisk)
            {
                // no asterisk after, so it is an ends-with pattern segment
                return (patternSegmentLength <= text.Length - textPosition
                        && string.Compare(text, text.Length - patternSegmentLength, pattern, patternSegmentStart, patternSegmentLength, ignoreCase) == 0);
            }
            else
            {
                // between asterisks, so this is a contains segment
                var matchesPosition = IndexOf(text, textPosition, pattern, patternSegmentStart, patternSegmentLength);
                if (matchesPosition == -1)
                    return false;

                return Matches(pattern, nextPatternStart, text, matchesPosition + patternSegmentLength, ignoreCase);
            }
        }

        private static int IndexOf(string text, int textStart, string value, int valueStart, int valueLength)
        {
            var firstChar = value[valueStart];

            while (true)
            {
                int firstCharPosition = text.IndexOf(firstChar, textStart);
                if (firstCharPosition < textStart || firstCharPosition + valueLength > text.Length)
                    return -1;

                if (string.Compare(text, firstCharPosition, value, valueStart, valueLength) == 0)
                    return firstCharPosition;

                textStart = firstCharPosition + 1;
            }
        }

        /// <summary>
        /// Attempts to determine the tabularity of an expression (scalar, tabular, none, unknown) given syntax.
        /// </summary>
        public static Tabularity GetSyntaxTabularity(Syntax.Expression expression, GlobalState globals = null)
        {
            globals = globals ?? GlobalState.Default;

            switch (expression)
            {
                case QueryOperator _:  // querys operator are always tabular
                case PipeExpression _: // pipes are always queries
                case Command _:        // commands always have tabular output
                case DataTableExpression _:
                case ToTableExpression _:
                    return Tabularity.Tabular;
                case BinaryExpression _:
                case PrefixUnaryExpression _:
                case InExpression _:
                case HasAnyExpression _:
                case HasAllExpression _:
                case BetweenExpression _:
                case ElementExpression _:
                case BracketedExpression _:
                case LiteralExpression _:
                case CompoundStringLiteralExpression _:
                case DynamicExpression _:
                case ToScalarExpression _:
                    return Tabularity.Scalar;
                case FunctionCallExpression fc:
                    var fn = globals.GetFunction(fc.Name.SimpleName);
                    if (fn != null)
                        return fn.Tabularity;
                    var dbFn = globals.Database?.GetFunction(fc.Name.SimpleName);
                    if (dbFn != null)
                        return dbFn.Tabularity;
                    return Tabularity.Unknown;
                case Expression _:
                default:
                    return Tabularity.Unknown;
            }
        }

        /// <summary>
        /// Attempts to determine the tabularity of a statement (scalar, tabular, none, unknown) given syntax.
        /// </summary>
        public static Tabularity GetSyntaxTabularity(Syntax.Statement statement, GlobalState globals = null)
        {
            if (statement != null
                && statement is ExpressionStatement es)
            {
                return KustoFacts.GetSyntaxTabularity(es.Expression, globals);
            }
            else
            {
                return Tabularity.None;
            }
        }

        /// <summary>
        /// Attempts to determine the tabularity of a block of statements (scalar, tabular, none, unknown) given syntax.
        /// </summary>
        public static Tabularity GetSyntaxTabularity(Syntax.SyntaxList<SeparatedElement<Statement>> statements, GlobalState globals = null)
        {
            if (statements != null
                && statements.Count > 0
                && statements[statements.Count - 1].Element is ExpressionStatement es)
            {
                return KustoFacts.GetSyntaxTabularity(es.Expression, globals);
            }
            else
            {
                return Tabularity.None;
            }
        }

        /// <summary>
        /// Gets the scalar type for the literal syntax kind.
        /// </summary>
        public static ScalarSymbol GetLiteralType(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.BooleanLiteralExpression:
                case SyntaxKind.BooleanLiteralToken:
                    return ScalarTypes.Bool;

                case SyntaxKind.StringLiteralExpression:
                case SyntaxKind.StringLiteralToken:
                case SyntaxKind.CompoundStringLiteralExpression:
                    return ScalarTypes.String;

                case SyntaxKind.IntLiteralExpression:
                case SyntaxKind.IntLiteralToken:
                    return ScalarTypes.Int;

                case SyntaxKind.LongLiteralExpression:
                case SyntaxKind.LongLiteralToken:
                    return ScalarTypes.Long;

                case SyntaxKind.RealLiteralExpression:
                case SyntaxKind.RealLiteralToken:
                    return ScalarTypes.Real;

                case SyntaxKind.DecimalLiteralExpression:
                case SyntaxKind.DecimalLiteralToken:
                    return ScalarTypes.Decimal;

                case SyntaxKind.TimespanLiteralExpression:
                case SyntaxKind.TimespanLiteralToken:
                    return ScalarTypes.TimeSpan;

                case SyntaxKind.DateTimeLiteralExpression:
                case SyntaxKind.DateTimeLiteralToken:
                    return ScalarTypes.DateTime;

                case SyntaxKind.GuidLiteralExpression:
                case SyntaxKind.GuidLiteralToken:
                case SyntaxKind.RawGuidLiteralToken:
                    return ScalarTypes.Guid;

                case SyntaxKind.TypeOfLiteralExpression:
                    return ScalarTypes.Type;

                case SyntaxKind.DynamicExpression:
                    return ScalarTypes.Dynamic;

                default:
                    return ScalarTypes.Unknown;
            }
        }
    }
}