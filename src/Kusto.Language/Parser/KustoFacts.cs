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

    public static class KustoFacts
    {
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
            "single",
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

        public static readonly IReadOnlyList<string> StorageTypes = ExtendedParamTypes;

        public static string[] KnownQueryOperatorParameterNames = new string[] {
            "bagexpansion",
            "bin_legacy",
            "decodeblocks",
            "expandoutput",
            "hint.concurrency",
            "hint.distribution",
            "hint.materialized",
            "hint.num_partitions",
            "hint.pass_filters",
            "hint.pass_filters_column",
            "hint.progressive_top",
            "hint.remote",
            "hint.shufflekey",
            "hint.spread",
            "hint.strategy",
            "isfuzzy",
            "kind",
            "with_itemindex",
            "with_match_id",
            "with_step_name",
            "withsource",
            "with_source",
            "__crossCluster",
            "__crossDB",
            "__id",
            "__isFuzzy",
            "__noWithSource",
            "__packedColumn",
            "__sourceColumnIndex",
          };

        public static readonly IReadOnlyList<string> ChartTypes = new string[]
        {
            "table", "list", "barchart", "piechart", "ladderchart", "timechart", "linechart", "anomalychart", "pivotchart", "areachart",
            "stackedareachart", "scatterchart", "timepivot", "columnchart", "timeline", "3Dchart", "card", "treemap"
        };

        /// <summary>
        /// Chart types not shown in intellisense
        /// </summary>
        public static readonly IReadOnlyList<string> HiddenChartTypes = new string[]
        {
            "3Dchart"
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

        public static readonly IReadOnlyList<string> JoinKinds = new string[]
        {
            "inner", "fullouter", "innerunique", "leftanti", "leftantisemi", "anti",
            "leftouter", "leftsemi", "rightanti", "rightantisemi", "rightouter", "rightsemi"
        };

        public static readonly IReadOnlyList<string> JoinHintRemotes = new string[]
        {
            "auto", "left", "local", "right"
        };

        public static readonly IReadOnlyList<string> PluginHintRemotes = new string[]
        {
            "local", "auto"
        };

        public static readonly IReadOnlyList<string> JoinHintStrategies = new string[]
        {
            "broadcast", "centralized", "shuffle"
        };

        public static readonly IReadOnlyList<string> LookupKinds = new string[]
        {
            "inner", "leftouter"
        };

        public static readonly IReadOnlyList<string> SummarizeHintStrategies = new string[]
        {
            "shuffle"
        };

        public static readonly IReadOnlyList<string> PartitionHintStrategies = new string[]
        {
            "shuffle", "native", "legacy"
        };

        public static readonly IReadOnlyList<string> OrderByHintStrategies = new string[]
        {
            "splitBlock", "multipleBlocks"
        };

        public static readonly IReadOnlyList<string> DistributionHintStrategies = new string[]
        {
            "single", "per_node", "per_shard", "default"
        };

        public static readonly IReadOnlyList<string> SearchKinds = new string[]
        {
            "default", "case_insensitive", "case_sensitive"
        };

        public static readonly IReadOnlyList<string> MakeSeriesKinds = new string[]
        {
            "nonempty"
        };

        public static readonly IReadOnlyList<string> MvExpandKinds = new string[]
        {
            "bag", "array"
        };

        public static readonly IReadOnlyList<string> ReduceByKinds = new string[]
        {
            "source"
        };

        public static readonly IReadOnlyList<string> UnionWithSourceProperties = new string[] {
            "withsource", "with_source"
        };

        public static string UnionIsFuzzyProperty => "isfuzzy";

        public static readonly IReadOnlyList<string> UnionKinds = new string[]
        {
            "inner", "outer"
        };

        public static readonly IReadOnlyList<string> ParseKinds = new string[]
        {
            "simple", "regex", "relaxed"
        };

        public static readonly IReadOnlyList<string> DataScopeValues = new string[]
        {
            "all", "hotcache"
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

        /// <summary>
        /// Keywords that can always be used as identifiers
        /// </summary>
        public static readonly IReadOnlyList<SyntaxKind> KeywordsAsIdentifiers =
            SyntaxFacts.GetKindsWithFixedText().Where(k => k.IsKeyword() && k.CanBeIdentifier()).ToArray();

        /// <summary>
        /// Keywords that can sometimes be used as identifiers
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
        /// Keywords that can be identifiers is in distinct locations.
        /// </summary>
        public static readonly IReadOnlyList<SyntaxKind> SpecialKeywordsAsIdentifiers = new SyntaxKind[]
        {
            SyntaxKind.KindKeyword,
            SyntaxKind.WithSourceKeyword,
            SyntaxKind.With_SourceKeyword
        };

        public static readonly IReadOnlyList<SyntaxKind> ForkOperatorKinds = new SyntaxKind[]
        {
            SyntaxKind.CountOperator,
            SyntaxKind.ExtendOperator,
            SyntaxKind.FilterOperator,
            SyntaxKind.ParseOperator,
            SyntaxKind.ParseWhereOperator,
            SyntaxKind.TakeOperator,
            SyntaxKind.TopNestedOperator,
            SyntaxKind.ProjectOperator,
            SyntaxKind.ProjectAwayOperator,
            SyntaxKind.ProjectKeepOperator,
            SyntaxKind.ProjectRenameOperator,
            SyntaxKind.ProjectReorderOperator,
            SyntaxKind.SummarizeOperator,
            SyntaxKind.DistinctOperator,
            SyntaxKind.TopHittersOperator,
            SyntaxKind.TopOperator,
            SyntaxKind.SortOperator,
            SyntaxKind.MvExpandOperator,
            SyntaxKind.MvApplyOperator,
            SyntaxKind.ReduceByOperator,
            SyntaxKind.SampleOperator,
            SyntaxKind.SampleDistinctOperator,
            SyntaxKind.AsOperator,
            SyntaxKind.InvokeOperator,
            SyntaxKind.ExecuteAndCacheOperator
        };

        /// <summary>
        /// Query operators that can come after a pipe 
        /// </summary>
        public static readonly IReadOnlyList<SyntaxKind> PostPipeOperatorKinds = new SyntaxKind[]       
        {
            SyntaxKind.ConsumeOperator,
            SyntaxKind.CountOperator,
            SyntaxKind.ExecuteAndCacheOperator,
            SyntaxKind.ExtendOperator,
            SyntaxKind.FacetOperator,
            SyntaxKind.FilterOperator,
            SyntaxKind.GetSchemaOperator,
            SyntaxKind.JoinOperator,
            SyntaxKind.ForkOperator,
            SyntaxKind.LookupOperator,
            SyntaxKind.MakeSeriesOperator,
            SyntaxKind.MvApplyOperator,
            SyntaxKind.MvExpandOperator,
            SyntaxKind.EvaluateOperator,
            SyntaxKind.ParseOperator,
            SyntaxKind.ParseWhereOperator,
            SyntaxKind.PartitionOperator,
            SyntaxKind.ProjectOperator,
            SyntaxKind.SampleOperator,
            SyntaxKind.SampleDistinctOperator,
            SyntaxKind.ProjectAwayOperator,
            SyntaxKind.ProjectKeepOperator,
            SyntaxKind.ProjectRenameOperator,
            SyntaxKind.ProjectReorderOperator,
            SyntaxKind.ReduceByOperator,
            SyntaxKind.SummarizeOperator,
            SyntaxKind.DistinctOperator,
            SyntaxKind.TakeOperator,
            SyntaxKind.SortOperator,
            SyntaxKind.TopHittersOperator,
            SyntaxKind.TopOperator,
            SyntaxKind.TopNestedOperator,
            SyntaxKind.UnionOperator,
            SyntaxKind.RenderOperator,
            SyntaxKind.AsOperator,
            SyntaxKind.SerializeOperator,
            SyntaxKind.InvokeOperator,
            SyntaxKind.ScanOperator
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
                || (expr.Parent is FacetWithExpressionClause fwce && fwce.Expression == expr)
                || (expr.Parent is FacetWithOperatorClause fwoc && fwoc.Operator == expr)
                || (expr.Parent is Expression pe && IsChildOfPipeStartingExpression(pe))
                || (expr.Parent is MaterializedViewCombineClause mvc && mvc.Parent is MaterializedViewCombineExpression mve && mve.AggregationsClause == mvc);
        }

        public static readonly IReadOnlyList<string> ScanOperatorKinds = new string[]
        {
            "partial",
            "full"
        };

        /// <summary>
        /// True if the text can be used as an identifier in all contexts that declare or reference names.
        /// (note: some names may be legal in one context but not another)
        /// </summary>
        public static bool CanBeIdentifier(string text) =>
            TokenParser.ScanIdentifier(text) == text.Length   // looks like an identifier
            && !(TokenParser.ScanTimespanLiteral(text) == text.Length)  // does not look like a timespan literal: 10days, etc
            && !(TokenParser.ScanBooleanLiteral(text) == text.Length)   // does not look like a boolean literal: true, false
            && (!IsKeyword(text) || IsKeywordThatCanBeIdentifier(text));

        /// <summary>
        /// True if the text is a keyword.
        /// </summary>
        public static bool IsKeyword(string text) =>
            SyntaxFacts.TryGetKind(text, out var kind) && kind.GetCategory() == SyntaxCategory.Keyword;

        /// <summary>
        /// True if the text is a keyword that can be an identifier.
        /// </summary>
        public static bool IsKeywordThatCanBeIdentifier(string text) =>
            SyntaxFacts.TryGetKind(text, out var kind) && kind.GetCategory() == SyntaxCategory.Keyword && SyntaxFacts.CanBeIdentifier(kind);

        /// <summary>
        /// Adds bracketting and quoting to name if necessary.
        /// </summary>
        public static string BracketNameIfNecessary(string name)
        {
            if (!CanBeIdentifier(name))
            {
                return GetBracketedName(name);
            }

            return name;
        }

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

        public static string GetExpressionResultName(Expression expr, string defaultName = "", TableSymbol rowScope = null)
        {
            return Binder.GetExpressionResultName(expr, defaultName, rowScope);
        }

        public static string GetStringLiteralValue(string literal)
        {
            int start = 0;
            int end = literal.Length;
            bool verbatim = false;

            if (end == 0)
                return string.Empty;

            // do not include H prefix in value
            if (literal[0] == 'h' || literal[0] == 'H')
            {
                literal = literal.Substring(1);
                end = literal.Length;
            }

            // check for multi-line string
            if (TryParseMultiLineStringLiteral(literal, out var multiLineLiteral))
                return multiLineLiteral;

            if (start < literal.Length && literal[start] == '@')
            {
                verbatim = true;
                start++; // do not include @ prefix in value
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
            else if (!verbatim && !bracketed && literal.IndexOf('\\', start) >= start)
            {
                return DecodeEscapes(literal, start, end - start);
            }
            else if (verbatim && !bracketed && HasInteriorQuote(literal, start, end, endQuote))
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

        private static bool TryParseMultiLineStringLiteral(string str, out string literal)
        {
            return TryParseSimpleStringLiteral(str, MultiLineStringQuote, out literal)
                || TryParseSimpleStringLiteral(str, AlternateMultiLineStringQuote, out literal);
        }

        private static bool TryParseSimpleStringLiteral(string str, string quote, out string literal)
        {
            // check for multi-line string
            if (str.StartsWith(quote, StringComparison.Ordinal))
            {
                var twiceQuoteLen = quote.Length << 1;
                if (str.Length >= twiceQuoteLen && str.EndsWith(quote, StringComparison.Ordinal))
                {
                    literal = str.Substring(quote.Length, str.Length - twiceQuoteLen);
                    return true;
                }
                else
                {
                    literal = str.Substring(quote.Length);
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
            int start = 0;
            int end = clusterUriOrName.Length;

            int hostNamePrefixStart = clusterUriOrName.IndexOf(HostNamePrefix);
            if (hostNamePrefixStart > 0)
            {
                start = hostNamePrefixStart + HostNamePrefix.Length;
                int colon = clusterUriOrName.IndexOf(':', start);
                if (colon >= 0)
                    end = colon;
            }

            if (start == 0 && end == clusterUriOrName.Length)
            {
                return clusterUriOrName;
            }
            else
            {
                return clusterUriOrName.Substring(start, end - start);
            }
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
    }
}