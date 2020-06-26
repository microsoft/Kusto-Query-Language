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

        public static readonly IReadOnlyList<string> ChartTypes = new string[]
        {
            "table", "list", "barchart", "piechart", "ladderchart", "timechart", "linechart", "anomalychart", "pivotchart", "areachart",
            "stackedareachart", "scatterchart", "timepivot", "columnchart", "timeline", "3Dchart", "card"
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
            "title", "xcolumn", "series", "ycolumns", "anomalycolumns", "kind", "xtitle", "ytitle", "xaxis", "yaxis", "legend", "ysplit", "accumulate", "ymin", "ymax"
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
            "leftouter", "leftsemi", "rightanti", /*"rightantisemi",*/ "rightouter", "rightsemi"
        };

        public static readonly IReadOnlyList<string> JoinHintRemotes = new string[]
        {
            "auto", "left", "local", "right"
        };

        public static readonly IReadOnlyList<string> JoinHintStrategies = new string[]
        {
            "broadcast", "centralized", "shuffle"
        };

        public static readonly IReadOnlyList<string> SummarizeHintStrategies = new string[]
        {
            "shuffle"
        };

        public static readonly IReadOnlyList<string> DistributionHintStrategies = new string[]
        {
            "single", "per_node", "per_shard", "default"
        };

        public static readonly string FindWithSourceProperty = "withsource";

        public static readonly IReadOnlyList<string> SearchKinds = new string[]
        {
            "default", "case_insensitive", "case_sensitive"
        };

        public static readonly string MvExpandBagExpansionProperty = "bagexpansion";

        public static readonly IReadOnlyList<string> MvExpandBagExpansions = new string[]
        {
            "bag", "array"
        };

        public static readonly string MvExpandWithItemIndexProperty = "with_itemindex";

        public static readonly string MvApplyWithItemIndexProperty = "with_itemindex";

        public static readonly IReadOnlyList<string> ReduceByKinds = new string[]
        {
            "source"
        };

        public static readonly IReadOnlyList<string> UnionWithSourceProperties = new string[] {
            "withsource", "with_source"
        };

        public static string UnionIsFuzzyProperty => SyntaxFacts.GetText(SyntaxKind.IsFuzzyKeyword);

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

        public static readonly IReadOnlyList<SyntaxKind> PartitionOperatorKinds = new SyntaxKind[]
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
            SyntaxKind.ProjectRenameOperator,
            SyntaxKind.ProjectReorderOperator,
            SyntaxKind.SummarizeOperator,
            SyntaxKind.DistinctOperator,
            SyntaxKind.TopHittersOperator,
            SyntaxKind.TopOperator,
            SyntaxKind.SortOperator,
            SyntaxKind.MakeSeriesOperator,
            SyntaxKind.MvExpandOperator,
            SyntaxKind.MvApplyOperator,
            SyntaxKind.ReduceByOperator,
            SyntaxKind.SampleOperator,
            SyntaxKind.SampleDistinctOperator,
            SyntaxKind.AsOperator,
            SyntaxKind.InvokeOperator,
            SyntaxKind.ExecuteAndCacheOperator
        };

        public static readonly IReadOnlyList<string> ScanOperatorKinds = new string[]
        {
            "partial",
            "full"
        };

        public static readonly string ScanOperatorWithMatchIdProperty = "with_match_id";
        public static readonly string ScanOperatorWithStepNameProperty = "with_step_name";

        /// <summary>
        /// True if the text can an identifier in all places that declare or reference names.
        /// </summary>
        public static bool CanBeIdentifier(string text) =>
            !IsKeyword(text) && LexicalGrammar.Identifier.Matches(text);

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

        public static string GetStringLiteralValue(string text)
        {
            int start = 0;
            int end = text.Length;
            bool verbatim = false;

            if (end == 0)
                return string.Empty;

            if (text[0] == 'h' || text[0] == 'H')
                start++; // do not include H prefix in value

            if (start < text.Length && text[start] == '@')
            {
                verbatim = true;
                start++; // do not include @ prefix in value
            }

            if (start >= text.Length)
                return string.Empty;

            var quote = text[start];

            start++; // do not include quote in value

            if (end > 0 && text[end - 1] == quote)
                end--; // do not include end quote in value

            if (end <= start)
            {
                return string.Empty;
            }
            else if (!verbatim && text.IndexOf('\\', start) >= start)
            {
                return DecodeEscapes(text, start, end - start);
            }
            else if (verbatim && HasInteriorQuote(text, start, end, quote))
            {
                return DecodeDoubleQuotes(text, start, end - start, quote);
            }
            else if (start > 0 || end < text.Length)
            {
                return text.Substring(start, end - start);
            }
            else
            {
                return text;
            }
        }

        private static bool HasInteriorQuote(string text, int start, int end, char quote)
        {
            var position = text.IndexOf(quote);
            return position >= 0 && position < end;
        }

        private static string DecodeEscapes(string text, int start, int length)
        {
            var builder = new StringBuilder();

            for (int i = start, end = start + length; i < end; i++)
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
                            i++;
                            break;
                        case 'a':
                            builder.Append('\a');
                            i++;
                            break;
                        case 'b':
                            builder.Append('\b');
                            i++;
                            break;
                        case 'f':
                            builder.Append('\f');
                            i++;
                            break;
                        case 'n':
                            builder.Append('\n');
                            i++;
                            break;
                        case 'r':
                            builder.Append('\r');
                            i++;
                            break;
                        case 't':
                            builder.Append('\t');
                            i++;
                            break;
                        case 'v':
                            builder.Append('\v');
                            i++;
                            break;
                        case 'u':
                        case 'U':
                        case 'x':
                            // hex encoded character
                            builder.Append(DecodeHex(text, ref i));
                            break;
                        default:
                            // octal encoded character
                            builder.Append(DecodeOctal(text, ref i));
                            break;
                    }
                }
                else
                {
                    builder.Append(ch);
                }
            }

            return builder.ToString();
        }

        private static char DecodeOctal(string text, ref int index)
        {
            long value = 0;

            for (;  index < text.Length && char.IsDigit(text[index]); index++)
            {
                value = (value << 3) + (text[index] - '0');
            }

            return unchecked((char)value);
        }

        private static char DecodeHex(string text, ref int index)
        {
            long value = 0;

            for (; index < text.Length; index++)
            {
                var ch = text[index];

                if (ch >= 'a' && ch <= 'f')
                {
                    value = (value << 4) + (ch - 'a');
                }
                else if (ch >= 'A' && ch <= 'F')
                {
                    value = (value << 4) + (ch - 'A');
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

            return (char)value;
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

        private static readonly string HostNamePrefix = "://";
        private static readonly string KustoWindowsNet = ".kusto.windows.net";

        /// <summary>
        /// Returns true if the name matches the host name.
        /// </summary>
        public static bool IsClusterHostName(string name, string hostName)
        {
            return string.Compare(name, hostName, ignoreCase: true) == 0;
        }

        /// <summary>
        /// Returns true if the hostname ends with .kusto.windows.net.
        /// </summary>
        public static bool IsKustoWindowsNet(string hostname)
        {
            return hostname.EndsWith(KustoWindowsNet, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns true if the name is the short name prefix of the cluster host name.
        /// </summary>
        public static bool IsClusterShortName(string name, string hostName)
        {
            // supplied name is the first part of XXX.YYY ?
            return !name.Contains(".")
                && hostName.StartsWith(name + ".", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the host name from a possible cluster uri
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
        /// True if the text matches the pattern (*, xxx*, *xxx, *xxx*, xxx*yyy, *xxx*yyy*, ...)
        /// The * represents any zero-or-more characters.
        /// </summary>
        public static bool Matches(string pattern, string text)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));

            if (text == null)
                throw new ArgumentNullException(nameof(text));

            // empty pattern does not match anything
            if (pattern.Length == 0)
                return false;

            return Matches(pattern, 0, text, 0);
        }

        private static bool Matches(string pattern, int patternSegmentStart, string text, int textPosition)
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
                    return Matches(pattern, nextPatternStart, text, textPosition);
                }
            }
            else if (patternSegmentStart == 0)
            {
                if (!sawAsterisk)
                {
                    // this is fixed-pattern (no asterisks before or after), so must be exact match
                    return text.Length == patternSegmentLength 
                        && string.Compare(text, 0, pattern, 0, patternSegmentLength) == 0;
                }
                else
                {
                    // this is the first segment (no asterisk before) so its a starts-with pattern segment
                    if (patternSegmentLength > text.Length
                        || string.Compare(text, 0, pattern, 0, patternSegmentLength) != 0)
                    {
                        return false;
                    }

                    return Matches(pattern, nextPatternStart, text, patternSegmentLength);
                }
            }
            else if (!sawAsterisk)
            {
                // no asterisk after, so it is an ends-with pattern segment
                return (patternSegmentLength <= text.Length - textPosition
                        && string.Compare(text, text.Length - patternSegmentLength, pattern, patternSegmentStart, patternSegmentLength) == 0);
            }
            else
            {
                // between asterisks, so this is a contains segment
                var matchesPosition = IndexOf(text, textPosition, pattern, patternSegmentStart, patternSegmentLength);
                if (matchesPosition == -1)
                    return false;

                return Matches(pattern, nextPatternStart, text, matchesPosition + patternSegmentLength);
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
    }
}