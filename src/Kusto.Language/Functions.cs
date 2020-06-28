using System.Collections.Generic;

namespace Kusto.Language
{
    using Symbols;
    using static FunctionHelpers;

    /// <summary>
    /// Well known scalar and special functions.
    /// </summary>
    public class Functions
    {
        #region cluster / database / table
        public static readonly FunctionSymbol Cluster =
            new FunctionSymbol("cluster",
                ReturnTypeKind.Parameter0Cluster,
                new Parameter("name", ScalarTypes.String));

        public static readonly FunctionSymbol Database =
            new FunctionSymbol("database",
                ReturnTypeKind.Parameter0Database,
                new Parameter("name", ScalarTypes.String));

        public static readonly FunctionSymbol Table = new FunctionSymbol("table",
                ReturnTypeKind.Parameter0Table,
                new Parameter("name", ScalarTypes.String),
                new Parameter("query_data_scope", ScalarTypes.String, minOccurring: 0));

        public static readonly FunctionSymbol ExternalTable = new FunctionSymbol("external_table",
                ReturnTypeKind.Parameter0ExternalTable,
                new Parameter("name", ScalarTypes.String),
                new Parameter("mapping", ScalarTypes.String, minOccurring: 0));

        public static readonly FunctionSymbol MaterializedView = new FunctionSymbol("materialized_view",
                ReturnTypeKind.Parameter0MaterializedView,
                new Parameter("name", ScalarTypes.String),
                new Parameter("max_age", ScalarTypes.TimeSpan, minOccurring: 0));

        #endregion

        #region string functions
        public static readonly FunctionSymbol Strcat =
            new FunctionSymbol("strcat", ScalarTypes.String,
                new Parameter("arg", ParameterTypeKind.Scalar, maxOccurring: 64))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol StrcatArray =
            new FunctionSymbol("strcat_array", ScalarTypes.String,
                new Parameter("array", ScalarTypes.Dynamic),
                new Parameter("delimiter", ParameterTypeKind.Scalar))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol ArrayStrcat =
            new FunctionSymbol("array_strcat", ScalarTypes.String,
                new Parameter("array", ScalarTypes.Dynamic),
                new Parameter("delimiter", ParameterTypeKind.Scalar))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol StrcatDelim =
            new FunctionSymbol("strcat_delim", ScalarTypes.String,
                new Parameter("delimiter", ParameterTypeKind.Scalar),
                new Parameter("arg", ParameterTypeKind.Scalar, minOccurring: 2, maxOccurring: 64))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol Strcmp =
            new FunctionSymbol("strcmp", ScalarTypes.Long,
                new Parameter("s1", ParameterTypeKind.Scalar),
                new Parameter("s2", ParameterTypeKind.Scalar))
            .ConstantFoldable();

        public static readonly FunctionSymbol Strrep =
            new FunctionSymbol("strrep", ScalarTypes.String,
                new Parameter("value", ParameterTypeKind.Scalar),
                new Parameter("multiplier", ScalarTypes.Long))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol Strlen =
            new FunctionSymbol("strlen", ScalarTypes.Long,
                new Parameter("string", ParameterTypeKind.StringOrDynamic))
            .WithResultNameKind(ResultNameKind.NameAndFirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol StringSize =
            new FunctionSymbol("string_size", ScalarTypes.Long,
                new Parameter("string", ParameterTypeKind.StringOrDynamic))
            .WithResultNameKind(ResultNameKind.NameAndFirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol ToUpper =
            new FunctionSymbol("toupper", ScalarTypes.String,
                new Parameter("value", ParameterTypeKind.Scalar))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol ToLower =
            new FunctionSymbol("tolower", ScalarTypes.String,
                new Parameter("value", ParameterTypeKind.Scalar))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol ToUtf8 =
            new FunctionSymbol("to_utf8", ScalarTypes.Dynamic,
                new Parameter("value", ParameterTypeKind.Scalar))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol Substring =
            new FunctionSymbol("substring", ScalarTypes.String,
                new Parameter("string", ParameterTypeKind.Scalar),
                new Parameter("start", ParameterTypeKind.Integer),
                new Parameter("length", ParameterTypeKind.Integer, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol IndexOf =
            new FunctionSymbol("indexof", ScalarTypes.Long,
                new Parameter("string", ParameterTypeKind.Scalar),
                new Parameter("match", ParameterTypeKind.Scalar),
                new Parameter("start", ParameterTypeKind.Integer, minOccurring: 0),
                new Parameter("length", ParameterTypeKind.Integer, minOccurring: 0),
                new Parameter("occurence", ParameterTypeKind.Integer, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol IndexOfRegex =
            new FunctionSymbol("indexof_regex", ScalarTypes.Long,
                new Parameter("string", ParameterTypeKind.Scalar),
                new Parameter("match", ParameterTypeKind.Scalar),
                new Parameter("start", ParameterTypeKind.Integer, minOccurring: 0),
                new Parameter("length", ParameterTypeKind.Integer, minOccurring: 0),
                new Parameter("occurence", ParameterTypeKind.Integer, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol Reverse =
            new FunctionSymbol("reverse", ScalarTypes.String,
                new Parameter("value", ParameterTypeKind.Scalar))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol Split =
            new FunctionSymbol("split", ScalarTypes.Dynamic,
                new Parameter("source", ParameterTypeKind.Scalar),
                new Parameter("delimiter", ScalarTypes.String),
                new Parameter("requestedIndex", ParameterTypeKind.Integer, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol ParseCommandLine =
            new FunctionSymbol("parse_command_line", ScalarTypes.Dynamic,
                new Parameter("command", ParameterTypeKind.Scalar),
                new Parameter("parser", ScalarTypes.String))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol Extract =
            new FunctionSymbol("extract",
                new Signature(ScalarTypes.String,
                    new Parameter("regex", ScalarTypes.String),
                    new Parameter("captureGroup", ScalarTypes.Long),
                    new Parameter("text", ScalarTypes.String)),
                new Signature(ReturnTypeKind.ParameterNLiteral,
                    new Parameter("regex", ScalarTypes.String),
                    new Parameter("captureGroup", ScalarTypes.Long),
                    new Parameter("text", ScalarTypes.String),
                    new Parameter("typeLiteral", ScalarTypes.Type, ArgumentKind.Literal)))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol ExtractAll_Depricated =
             new FunctionSymbol("extractall",
                new Signature(ScalarTypes.Dynamic,
                    new Parameter("regex", ScalarTypes.String),
                    new Parameter("text", ScalarTypes.String)),
                new Signature(ScalarTypes.Dynamic,
                    new Parameter("regex", ScalarTypes.String),
                    new Parameter("captureGroups", ScalarTypes.Dynamic),
                    new Parameter("text", ScalarTypes.String)))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable()
            .Hide();

        public static readonly FunctionSymbol ExtractAll =
            new FunctionSymbol("extract_all",
                new Signature(ScalarTypes.Dynamic,
                    new Parameter("regex", ScalarTypes.String),
                    new Parameter("text", ScalarTypes.String)),
                new Signature(ScalarTypes.Dynamic,
                    new Parameter("regex", ScalarTypes.String),
                    new Parameter("captureGroups", ScalarTypes.Dynamic),
                    new Parameter("text", ScalarTypes.String)))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol ExtractJson =
            new FunctionSymbol("extractjson",
                new Signature(ScalarTypes.Dynamic,
                    new Parameter("jsonPath", ScalarTypes.String),
                    new Parameter("jsonText", ScalarTypes.String)),
                new Signature(ReturnTypeKind.ParameterNLiteral,
                    new Parameter("jsonPath", ScalarTypes.String),
                    new Parameter("jsonText", ScalarTypes.String),
                    new Parameter("type", ScalarTypes.Type, ArgumentKind.Literal)))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol Replace =
            new FunctionSymbol("replace", ScalarTypes.String,
                new Parameter("regex", ScalarTypes.String),
                new Parameter("rewrite", ScalarTypes.String),
                new Parameter("text", ScalarTypes.String))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol TrimStart =
            new FunctionSymbol("trim_start", ScalarTypes.String,
                new Parameter("regex", ScalarTypes.String),
                new Parameter("text", ScalarTypes.String))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol TrimEnd =
            new FunctionSymbol("trim_end", ScalarTypes.String,
                new Parameter("regex", ScalarTypes.String),
                new Parameter("text", ScalarTypes.String))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol Trim =
            new FunctionSymbol("trim", ScalarTypes.String,
                new Parameter("regex", ScalarTypes.String),
                new Parameter("text", ScalarTypes.String))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol CountOf =
            new FunctionSymbol("countof", ScalarTypes.Long,
                new Parameter("text", ScalarTypes.String),
                new Parameter("search", ScalarTypes.String),
                new Parameter("kind", ScalarTypes.String, ArgumentKind.Literal, new object[] { "normal", "regex" }, isCaseSensitive: true, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol Translate =
            new FunctionSymbol("translate", ScalarTypes.String,
                new Parameter("searchList", ScalarTypes.String, ArgumentKind.Constant),
                new Parameter("replacementList", ScalarTypes.String, ArgumentKind.Constant),
                new Parameter("text", ScalarTypes.String))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol MakeString =
            new FunctionSymbol("make_string",
                ScalarTypes.String,
                new Parameter("value", ParameterTypeKind.IntegerOrDynamic, maxOccurring: MaxRepeat))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol DateTimeToLocaleString =
            new FunctionSymbol("datetime_to_locale_string",
                ScalarTypes.String,
                new Parameter("date", ScalarTypes.DateTime),
                new Parameter("culture", ScalarTypes.String),
                new Parameter("options", ScalarTypes.Dynamic, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable()
            .Hide();

        public static readonly FunctionSymbol NumberToLocaleString =
            new FunctionSymbol("number_to_locale_string",
                ScalarTypes.String,
                new Parameter("number", ParameterTypeKind.Number),
                new Parameter("culture", ScalarTypes.String),
                new Parameter("options", ScalarTypes.Dynamic, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable()
            .Hide();
        #endregion

        #region type conversion functions
        public static readonly new FunctionSymbol ToString =
            new FunctionSymbol("tostring", ScalarTypes.String,
                new Parameter("value", ParameterTypeKind.Scalar))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol ToHex =
            new FunctionSymbol("tohex", ScalarTypes.String,
                new Parameter("value", ParameterTypeKind.Integer))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol ToDynamic_ =  // use _ because build fails claiming ToDynamic exists on object.
            new FunctionSymbol("todynamic", ScalarTypes.Dynamic,
                new Parameter("value", ScalarTypes.String))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol ToObject_Depricated =
            new FunctionSymbol("toobject", ScalarTypes.Dynamic,
                new Parameter("value", ScalarTypes.String))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .Hide();

        public static readonly FunctionSymbol ToLong =
            new FunctionSymbol("tolong", ScalarTypes.Long,
                new Parameter("value", ParameterTypeKind.Scalar))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol ToInt =
            new FunctionSymbol("toint", ScalarTypes.Int,
                new Parameter("value", ParameterTypeKind.Scalar))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol ToReal =
            new FunctionSymbol("toreal", ScalarTypes.Real,
                new Parameter("value", ParameterTypeKind.Scalar))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol ToDouble =
            new FunctionSymbol("todouble", ScalarTypes.Real,
                new Parameter("value", ParameterTypeKind.Scalar))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol ToDateTime =
            new FunctionSymbol("todatetime", ScalarTypes.DateTime,
                new Parameter("value", ParameterTypeKind.Scalar))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol ToTimespan =
            new FunctionSymbol("totimespan", ScalarTypes.TimeSpan,
                new Parameter("value", ParameterTypeKind.Scalar))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol ToTime=
           new FunctionSymbol("totime", ScalarTypes.TimeSpan,
               new Parameter("value", ParameterTypeKind.Scalar))
           .Hide()
           .ConstantFoldable()
           .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol ToBool =
            new FunctionSymbol("tobool", ScalarTypes.Bool,
                new Parameter("value", ParameterTypeKind.Scalar))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol ToBoolean =
            new FunctionSymbol("toboolean", ScalarTypes.Bool,
                new Parameter("value", ParameterTypeKind.Scalar))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol ToDecimal =
            new FunctionSymbol("todecimal", ScalarTypes.Decimal,
                new Parameter("value", ParameterTypeKind.Scalar))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol ToGuid =
            new FunctionSymbol("toguid", ScalarTypes.Guid,
                new Parameter("value", ParameterTypeKind.StringOrDynamic))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly new FunctionSymbol GetType =
            new FunctionSymbol("gettype", ScalarTypes.String,
                new Parameter("value", ParameterTypeKind.Scalar))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("type");
        #endregion

        #region encoding/decoding functions
        public static readonly FunctionSymbol UrlEncode =
            new FunctionSymbol("url_encode", ScalarTypes.String,
                new Parameter("url", ScalarTypes.String))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol UrlEncode_Component =
            new FunctionSymbol("url_encode_component", ScalarTypes.String,
                new Parameter("url", ScalarTypes.String))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol UrlDecode =
            new FunctionSymbol("url_decode", ScalarTypes.String,
                new Parameter("encoded_url", ScalarTypes.String))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol Base64EncodeString =
            new FunctionSymbol("base64_encodestring", ScalarTypes.String,
                new Parameter("string", ScalarTypes.String))
            .ConstantFoldable()
            .Hide() // obsolete function name
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol Base64EncodeToString =
            new FunctionSymbol("base64_encode_tostring", ScalarTypes.String,
                new Parameter("string", ScalarTypes.String))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol Base64DecodeString =
            new FunctionSymbol("base64_decodestring", ScalarTypes.String,
                new Parameter("base64_string", ScalarTypes.String))
            .ConstantFoldable()
            .Hide() // obsolete function name
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol Base64DecodeToString =
                    new FunctionSymbol("base64_decode_tostring", ScalarTypes.String,
                        new Parameter("base64_string", ScalarTypes.String))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol Base64DecodeToArray =
            new FunctionSymbol("base64_decode_toarray", ScalarTypes.Dynamic,
                new Parameter("base64_string", ScalarTypes.String))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol Base64EncodeFromArray =
            new FunctionSymbol("base64_encode_fromarray", ScalarTypes.String,
                new Parameter("base64_string_decodced_as_array", ScalarTypes.Dynamic))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);
        #endregion

        #region parsing functions
        public static readonly FunctionSymbol ParseCsv =
            new FunctionSymbol("parse_csv", ScalarTypes.Dynamic,
                new Parameter("csv_text", ScalarTypes.String))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol ParseJson_Depricated =
            new FunctionSymbol("parsejson", ScalarTypes.Dynamic,
                new Parameter("json_text", ScalarTypes.String))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable()
            .Hide();

        public static readonly FunctionSymbol ParseJson =
            new FunctionSymbol("parse_json", ScalarTypes.Dynamic,
                new Parameter("json_text", ScalarTypes.String))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol ParseXml =
            new FunctionSymbol("parse_xml", ScalarTypes.Dynamic,
                new Parameter("xml_text", ScalarTypes.String))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol ParseUrl_Depricated =
            new FunctionSymbol("parseurl", ScalarTypes.Dynamic,
                new Parameter("url", ScalarTypes.String))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable()
            .Hide();

        public static readonly FunctionSymbol ParseUrl =
            new FunctionSymbol("parse_url", ScalarTypes.Dynamic,
                new Parameter("url", ScalarTypes.String))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol ParseUrlQuery_Depricated =
            new FunctionSymbol("parseurlquery", ScalarTypes.Dynamic,
                new Parameter("query", ScalarTypes.String))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable()
            .Hide();

        public static readonly FunctionSymbol ParseUrlQuery =
            new FunctionSymbol("parse_urlquery", ScalarTypes.Dynamic,
                new Parameter("query", ScalarTypes.String))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        #region IPv4 functions
        public static readonly FunctionSymbol ParseIPV4 =
            new FunctionSymbol("parse_ipv4", ScalarTypes.Long,
                new Parameter("ip", ScalarTypes.String))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol ParseIPV4Mask =
            new FunctionSymbol("parse_ipv4_mask", ScalarTypes.Long,
                new Parameter("ip", ScalarTypes.String),
                new Parameter("prefix", ScalarTypes.Long))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol Ipv4Compare  =
            new FunctionSymbol("ipv4_compare", ScalarTypes.Long,
                new Parameter("ip1", ScalarTypes.String),
                new Parameter("ip2", ScalarTypes.String),
                new Parameter("prefix", ScalarTypes.Long, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol Ipv4IsMatch =
            new FunctionSymbol("ipv4_is_match", ScalarTypes.Bool,
                new Parameter("ip1", ScalarTypes.String),
                new Parameter("ip2", ScalarTypes.String),
                new Parameter("prefix", ScalarTypes.Long, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();
        #endregion

        #region IPv6 functions
        public static readonly FunctionSymbol ParseIPV6 =
            new FunctionSymbol("parse_ipv6", ScalarTypes.String,
                new Parameter("ip", ScalarTypes.String))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol ParseIPV6Mask =
            new FunctionSymbol("parse_ipv6_mask", ScalarTypes.String,
                new Parameter("ip", ScalarTypes.String),
                new Parameter("prefix", ScalarTypes.Long))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol Ipv6Compare =
            new FunctionSymbol("ipv6_compare", ScalarTypes.Long,
                new Parameter("ip1", ScalarTypes.String),
                new Parameter("ip2", ScalarTypes.String),
                new Parameter("prefix", ScalarTypes.Long, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol Ipv6IsMatch =
            new FunctionSymbol("ipv6_is_match", ScalarTypes.Bool,
                new Parameter("ip1", ScalarTypes.String),
                new Parameter("ip2", ScalarTypes.String),
                new Parameter("prefix", ScalarTypes.Long, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();
        #endregion

        public static readonly FunctionSymbol ParsePath =
            new FunctionSymbol("parse_path", ScalarTypes.Dynamic,
                new Parameter("path", ScalarTypes.String))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol ParseUserAgent =
            new FunctionSymbol("parse_user_agent", ScalarTypes.Dynamic,
                new Parameter("user_agent", ScalarTypes.String),
                new Parameter("look_for", ParameterTypeKind.StringOrDynamic, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol ParseVersion =
            new FunctionSymbol("parse_version", ScalarTypes.Decimal,
                new Parameter("version", ScalarTypes.String))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();
        #endregion

        #region date and time functions
        public static readonly FunctionSymbol FormatDatetime =
            new FunctionSymbol("format_datetime", ScalarTypes.String,
                new Parameter("date", ScalarTypes.DateTime),
                new Parameter("format", ScalarTypes.String, ArgumentKind.LiteralNotEmpty))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol FormatTimespan =
            new FunctionSymbol("format_timespan", ScalarTypes.String,
                new Parameter("timespan", ScalarTypes.TimeSpan),
                new Parameter("format", ScalarTypes.String, ArgumentKind.LiteralNotEmpty))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol MakeDatetime =
            new FunctionSymbol("make_datetime",
                new Signature(ScalarTypes.DateTime,
                    new Parameter("dateTime", ParameterTypeKind.Scalar)),
                new Signature(ScalarTypes.DateTime,
                    new Parameter("year", ParameterTypeKind.Number),
                    new Parameter("month", ParameterTypeKind.Number),
                    new Parameter("day", ParameterTypeKind.Number),
                    new Parameter("hour", ParameterTypeKind.Number, minOccurring: 0),
                    new Parameter("minute", ParameterTypeKind.Number, minOccurring: 0),
                    new Parameter("second", ParameterTypeKind.Number, minOccurring: 0)))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.OnlyArgument);

        public static readonly FunctionSymbol MakeTimespan =
            new FunctionSymbol("make_timespan",
                new Signature(ScalarTypes.TimeSpan,
                    new Parameter("timespan", ParameterTypeKind.Scalar)),
                new Signature(ScalarTypes.TimeSpan,
                    new Parameter("hours", ParameterTypeKind.Integer),
                    new Parameter("minutes", ParameterTypeKind.Integer),
                    new Parameter("seconds", ParameterTypeKind.Integer, minOccurring: 0)),
                new Signature(ScalarTypes.TimeSpan,
                    new Parameter("days", ParameterTypeKind.Integer),
                    new Parameter("hours", ParameterTypeKind.Integer),
                    new Parameter("minutes", ParameterTypeKind.Integer),
                    new Parameter("seconds", ParameterTypeKind.Integer)))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.OnlyArgument);

        private static readonly string[] s_dateDiffLiteralValues =
            new[] { "Year", "Quarter", "Month", "Week", "Day", "Hour", "Minute", "Second", "Millisecond", "Microsecond", "Nanosecond" };

        private static readonly string[] s_datePartLiteralValues =
            new[] { "Year", "Quarter", "Month", "WeekOfYear", "Day", "DayOfYear", "Hour", "Minute", "Second", "Millisecond", "Microsecond", "Nanosecond" };

        public static readonly FunctionSymbol DatetimeAdd =
            new FunctionSymbol("datetime_add", ScalarTypes.DateTime,
                new Parameter("part", ScalarTypes.String, ArgumentKind.Literal, s_dateDiffLiteralValues),
                new Parameter("value", ParameterTypeKind.Integer),
                new Parameter("datetime", ScalarTypes.DateTime))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol DatetimeDiff =
            new FunctionSymbol("datetime_diff", ScalarTypes.Long,
                new Parameter("part", ScalarTypes.String, ArgumentKind.Literal, s_dateDiffLiteralValues),
                new Parameter("datetime1", ScalarTypes.DateTime),
                new Parameter("datetime2", ScalarTypes.DateTime))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol DayOfWeek =
            new FunctionSymbol("dayofweek", ScalarTypes.TimeSpan,
                new Parameter("date", ScalarTypes.DateTime))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol DayOfMonth =
            new FunctionSymbol("dayofmonth", ScalarTypes.Int,
                new Parameter("date", ScalarTypes.DateTime))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol DayOfYear =
            new FunctionSymbol("dayofyear", ScalarTypes.Int,
                new Parameter("date", ScalarTypes.DateTime))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol HourOfDay =
            new FunctionSymbol("hourofday", ScalarTypes.Int,
                new Parameter("date", ScalarTypes.DateTime))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        // To be deprecated as current implementation isn't ISO 8601 compliant.
        // A new function, week_of_year, that is ISO 8601 compliant has been added.
        public static readonly FunctionSymbol WeekOfYear =
            new FunctionSymbol("weekofyear", ScalarTypes.Int,
                new Parameter("date", ScalarTypes.DateTime))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None)
            .Hide();

        public static readonly FunctionSymbol WeekOfYearISO =
            new FunctionSymbol("week_of_year", ScalarTypes.Int,
                new Parameter("date", ScalarTypes.DateTime))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol MonthOfYear =
            new FunctionSymbol("monthofyear", ScalarTypes.Int,
                new Parameter("date", ScalarTypes.DateTime))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol StartOfDay =
            new FunctionSymbol("startofday", ScalarTypes.DateTime,
                new Parameter("date", ScalarTypes.DateTime),
                new Parameter("offset", ScalarTypes.Long, minOccurring: 0))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol StartOfWeek =
            new FunctionSymbol("startofweek", ScalarTypes.DateTime,
                new Parameter("date", ScalarTypes.DateTime),
                new Parameter("offset", ScalarTypes.Long, minOccurring: 0))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol StartOfMonth =
            new FunctionSymbol("startofmonth", ScalarTypes.DateTime,
                new Parameter("date", ScalarTypes.DateTime),
                new Parameter("offset", ScalarTypes.Long, minOccurring: 0))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol StartOfYear =
            new FunctionSymbol("startofyear", ScalarTypes.DateTime,
                new Parameter("date", ScalarTypes.DateTime),
                new Parameter("offset", ScalarTypes.Long, minOccurring: 0))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol EndOfDay =
            new FunctionSymbol("endofday", ScalarTypes.DateTime,
                new Parameter("date", ScalarTypes.DateTime),
                new Parameter("offset", ScalarTypes.Long, minOccurring: 0))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol EndOfWeek =
            new FunctionSymbol("endofweek", ScalarTypes.DateTime,
                new Parameter("date", ScalarTypes.DateTime),
                new Parameter("offset", ScalarTypes.Long, minOccurring: 0))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol EndOfMonth =
            new FunctionSymbol("endofmonth", ScalarTypes.DateTime,
                new Parameter("date", ScalarTypes.DateTime),
                new Parameter("offset", ScalarTypes.Long, minOccurring: 0))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol EndOfYear =
            new FunctionSymbol("endofyear", ScalarTypes.DateTime,
                new Parameter("date", ScalarTypes.DateTime),
                new Parameter("offset", ScalarTypes.Long, minOccurring: 0))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol GetYear =
            new FunctionSymbol("getyear", ScalarTypes.Int,
                new Parameter("date", ScalarTypes.DateTime))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol GetMonth =
            new FunctionSymbol("getmonth", ScalarTypes.Int,
                new Parameter("date", ScalarTypes.DateTime))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol DatePart =
            new FunctionSymbol("datepart", ScalarTypes.Int,
                new Parameter("part", ScalarTypes.String, ArgumentKind.Literal, s_datePartLiteralValues),
                new Parameter("date", ScalarTypes.DateTime))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None)
            .Hide();

        public static readonly FunctionSymbol DatetimePart =
            new FunctionSymbol("datetime_part", ScalarTypes.Int,
                new Parameter("part", ScalarTypes.String, ArgumentKind.Literal, s_datePartLiteralValues),
                new Parameter("date", ScalarTypes.DateTime))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol Now =
            new FunctionSymbol("now", ScalarTypes.DateTime,
                new Parameter("offset", ScalarTypes.TimeSpan, minOccurring: 0))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol Ago =
            new FunctionSymbol("ago", ScalarTypes.DateTime,
                new Parameter("timespan", ScalarTypes.TimeSpan, examples: KustoFacts.AgoExamples))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol UnixTimeSecondsToDateTime =
            new FunctionSymbol("unixtime_seconds_todatetime", ScalarTypes.DateTime,
                new Parameter("number", ParameterTypeKind.Number))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol UnixTimeMillisecondsToDateTime =
            new FunctionSymbol("unixtime_milliseconds_todatetime", ScalarTypes.DateTime,
                new Parameter("number", ParameterTypeKind.Number))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol UnixTimeMicrosecondsToDateTime =
            new FunctionSymbol("unixtime_microseconds_todatetime", ScalarTypes.DateTime,
                new Parameter("number", ParameterTypeKind.Number))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol UnixTimeNanosecondsToDateTime =
            new FunctionSymbol("unixtime_nanoseconds_todatetime", ScalarTypes.DateTime,
                new Parameter("number", ParameterTypeKind.Number))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);
        #endregion

        #region hash functions
        public static readonly FunctionSymbol HashCrc32 =
            new FunctionSymbol("__hash_crc32", ScalarTypes.Long,
                new Parameter("source", ParameterTypeKind.NotDynamic),
                new Parameter("mod", ParameterTypeKind.Integer),
                new Parameter("seed", ParameterTypeKind.Integer))
            .ConstantFoldable()
            .Hide();

        public static readonly FunctionSymbol HashManyCrc32 =
            new FunctionSymbol("__hash_many_crc32", ScalarTypes.Long,
                new Parameter("arg", ParameterTypeKind.Scalar, maxOccurring: 2))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable()
            .Hide();

        public static readonly FunctionSymbol HashDjb2 =
            new FunctionSymbol("__hash_djb2", ScalarTypes.Long,
                new Parameter("source", ParameterTypeKind.NotDynamic),
                new Parameter("mod", ParameterTypeKind.Integer, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable()
            .Hide();

        public static readonly FunctionSymbol HashXXH64 =
            new FunctionSymbol("__hash_xxh64", ScalarTypes.Long,
                new Parameter("source", ParameterTypeKind.NotDynamic),
                new Parameter("mod", ParameterTypeKind.Integer),
                new Parameter("seed", ParameterTypeKind.Integer))
            .ConstantFoldable()
            .Hide();

        public static readonly FunctionSymbol Hash =
            new FunctionSymbol("hash", ScalarTypes.Long,
                new Parameter("source", ParameterTypeKind.NotDynamic),
                new Parameter("mod", ParameterTypeKind.Integer, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol HashSha256 =
            new FunctionSymbol("hash_sha256", ScalarTypes.String,
                new Parameter("source", ParameterTypeKind.NotDynamic))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol HashMd5 =
          new FunctionSymbol("hash_md5", ScalarTypes.String,
              new Parameter("source", ParameterTypeKind.NotDynamic))
          .WithResultNameKind(ResultNameKind.None)
          .ConstantFoldable()
          .Hide(); // Unhide by June 30 2020

        public static readonly FunctionSymbol HashCombine =
          new FunctionSymbol("hash_combine", ScalarTypes.Long,
                  new Parameter("source", ParameterTypeKind.Scalar, minOccurring: 2, maxOccurring: MaxRepeat))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol HashMany =
          new FunctionSymbol("hash_many", ScalarTypes.Long,
              new Parameter("source", ParameterTypeKind.NotDynamic, minOccurring: 1, maxOccurring: MaxRepeat))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();
        #endregion

        #region iif / case
        public static readonly FunctionSymbol Iif =
            new FunctionSymbol("iif", ReturnTypeKind.Common,
                new Parameter("if", ScalarTypes.Bool),
                new Parameter("then", ParameterTypeKind.CommonScalarOrDynamic),
                new Parameter("else", ParameterTypeKind.CommonScalarOrDynamic));

        public static readonly FunctionSymbol Iff =
            new FunctionSymbol("iff", ReturnTypeKind.Common,
                new Parameter("if", ScalarTypes.Bool),
                new Parameter("then", ParameterTypeKind.CommonScalarOrDynamic),
                new Parameter("else", ParameterTypeKind.CommonScalarOrDynamic));

        public static readonly FunctionSymbol Case =
            new FunctionSymbol("case", ReturnTypeKind.Common,
                new Parameter("predicate", ScalarTypes.Bool, maxOccurring: MaxRepeat),
                new Parameter("then", ParameterTypeKind.CommonScalarOrDynamic, maxOccurring: MaxRepeat),
                new Parameter("else", ParameterTypeKind.CommonScalarOrDynamic));

        public static readonly FunctionSymbol Assert =
            new FunctionSymbol("assert", ScalarTypes.Bool,
                new Parameter("predicate", ScalarTypes.Bool),
                new Parameter("message", ScalarTypes.String));
        #endregion

        #region bin / floor
        public static readonly FunctionSymbol Bin =
            new FunctionSymbol("bin",
                new Signature(ReturnTypeKind.Parameter0,
                    new Parameter("value", ParameterTypeKind.Number),
                    new Parameter("roundTo", ParameterTypeKind.Number)),
                new Signature(ScalarTypes.TimeSpan,
                    new Parameter("value", ScalarTypes.TimeSpan),
                    new Parameter("roundTo", ScalarTypes.TimeSpan)),
                new Signature(ScalarTypes.DateTime,
                    new Parameter("value", ScalarTypes.DateTime),
                    new Parameter("roundTo", ScalarTypes.DateTime)),
                new Signature(ScalarTypes.DateTime,
                    new Parameter("value", ScalarTypes.DateTime),
                    new Parameter("roundTo", ScalarTypes.TimeSpan)))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol Floor =
            new FunctionSymbol("floor",
                new Signature(ReturnTypeKind.Parameter0,
                    new Parameter("value", ParameterTypeKind.Number),
                    new Parameter("roundTo", ParameterTypeKind.Number)),
                new Signature(ScalarTypes.TimeSpan,
                    new Parameter("value", ScalarTypes.TimeSpan),
                    new Parameter("roundTo", ScalarTypes.TimeSpan)),
                new Signature(ScalarTypes.DateTime,
                    new Parameter("value", ScalarTypes.DateTime),
                    new Parameter("roundTo", ScalarTypes.DateTime)),
                new Signature(ScalarTypes.DateTime,
                    new Parameter("value", ScalarTypes.DateTime),
                    new Parameter("roundTo", ScalarTypes.TimeSpan)))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol BinAt =
            new FunctionSymbol("bin_at",
                new Signature(ScalarTypes.Long,
                    new Parameter("value", ParameterTypeKind.Integer),
                    new Parameter("bin_size", ParameterTypeKind.Integer),
                    new Parameter("fixed_point", ParameterTypeKind.Integer)),
                new Signature(ScalarTypes.Real,
                    new Parameter("value", ParameterTypeKind.Number),
                    new Parameter("bin_size", ParameterTypeKind.Number),
                    new Parameter("fixed_point", ParameterTypeKind.Number)),
                new Signature(ScalarTypes.TimeSpan,
                    new Parameter("value", ScalarTypes.TimeSpan),
                    new Parameter("bin_size", ParameterTypeKind.Number),
                    new Parameter("fixed_point", ScalarTypes.TimeSpan)),
                new Signature(ScalarTypes.TimeSpan,
                    new Parameter("value", ScalarTypes.TimeSpan),
                    new Parameter("bin_size", ScalarTypes.TimeSpan),
                    new Parameter("fixed_point", ScalarTypes.TimeSpan)),
                new Signature(ScalarTypes.DateTime,
                    new Parameter("value", ScalarTypes.DateTime),
                    new Parameter("bin_size", ParameterTypeKind.Summable),
                    new Parameter("fixed_point", ScalarTypes.DateTime)))
                .WithResultNameKind(ResultNameKind.FirstArgument)
                .ConstantFoldable();

        public static readonly FunctionSymbol BinAuto =
            new FunctionSymbol("bin_auto", "bin_at(value, query_bin_auto_size, query_bin_auto_at)", Tabularity.Scalar,
                new Parameter("value", ParameterTypeKind.Summable))
                .WithResultNameKind(ResultNameKind.FirstArgument)
                .ConstantFoldable()
                .Hide();
        #endregion

        #region bool functions  (test state, return bool)
        public static readonly FunctionSymbol Not =
            new FunctionSymbol("not", ScalarTypes.Bool,
                new Parameter("expression", ScalarTypes.Bool))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol NotNull_Depricated =
            new FunctionSymbol("notnull", ScalarTypes.Bool,
                new Parameter("expression", ParameterTypeKind.Scalar))
            .WithResultNameKind(ResultNameKind.None)
            .Hide();

        public static readonly FunctionSymbol IsNotNull =
            new FunctionSymbol("isnotnull", ScalarTypes.Bool,
                new Parameter("expression", ParameterTypeKind.Scalar))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol IsNull =
            new FunctionSymbol("isnull", ScalarTypes.Bool,
                new Parameter("expression", ParameterTypeKind.Scalar))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol NotEmpty_Depricated =
            new FunctionSymbol("notempty", ScalarTypes.Bool,
                new Parameter("value", ParameterTypeKind.Scalar))
            .WithResultNameKind(ResultNameKind.None)
            .Hide();

        public static readonly FunctionSymbol IsNotEmpty =
            new FunctionSymbol("isnotempty", ScalarTypes.Bool,
                new Parameter("value", ParameterTypeKind.Scalar))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol IsEmpty =
            new FunctionSymbol("isempty", ScalarTypes.Bool,
                new Parameter("value", ParameterTypeKind.Scalar))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol IsAscii =
            new FunctionSymbol("isascii", ScalarTypes.Bool,
                new Parameter("value", ParameterTypeKind.Scalar))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol IsUtf8 =
            new FunctionSymbol("isutf8", ScalarTypes.Bool,
                new Parameter("value", ParameterTypeKind.Scalar))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol IsColumnExists =
            new FunctionSymbol("iscolumnexists", ScalarTypes.Bool,
                new Parameter("column_name", ScalarTypes.String, ArgumentKind.Constant))
            .WithResultNameKind(ResultNameKind.None)
            .Hide();

        public static readonly FunctionSymbol ColumnIfExists_Depricated =
            new FunctionSymbol("columnifexists", ReturnTypeKind.Parameter1,
                new Parameter("column_name", ScalarTypes.String, ArgumentKind.Constant),
                new Parameter("defaultValue", ParameterTypeKind.Scalar))
            .WithResultNameKind(ResultNameKind.FirstArgumentValueIfColumn)
            .Hide();

        public static readonly FunctionSymbol ColumnIfExists =
            new FunctionSymbol("column_ifexists", ReturnTypeKind.Parameter1,
                new Parameter("column_name", ScalarTypes.String, ArgumentKind.Constant),
                new Parameter("defaultValue", ParameterTypeKind.Scalar))
            .WithResultNameKind(ResultNameKind.FirstArgumentValueIfColumn);
        #endregion

        #region bitwise functions
        public static readonly FunctionSymbol BinaryAnd =
            new FunctionSymbol("binary_and", ScalarTypes.Long,
                new Parameter("value1", ParameterTypeKind.Integer),
                new Parameter("value2", ParameterTypeKind.Integer))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol BinaryOr =
            new FunctionSymbol("binary_or", ScalarTypes.Long,
                new Parameter("value1", ParameterTypeKind.Integer),
                new Parameter("value2", ParameterTypeKind.Integer))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol BinaryXor =
            new FunctionSymbol("binary_xor", ScalarTypes.Long,
                new Parameter("value1", ParameterTypeKind.Integer),
                new Parameter("value2", ParameterTypeKind.Integer))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol BinaryNot =
            new FunctionSymbol("binary_not", ScalarTypes.Long,
                new Parameter("value", ParameterTypeKind.Integer))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol BinaryShiftRight =
            new FunctionSymbol("binary_shift_right", ScalarTypes.Long,
                new Parameter("value", ParameterTypeKind.Integer),
                new Parameter("shift", ParameterTypeKind.Integer))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol BinaryShiftLeft =
           new FunctionSymbol("binary_shift_left", ScalarTypes.Long,
                new Parameter("value", ParameterTypeKind.Integer),
                new Parameter("shift", ParameterTypeKind.Integer))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol BitsetCountOnes =
           new FunctionSymbol("bitset_count_ones", ScalarTypes.Long,
               new Parameter("value", ParameterTypeKind.Integer))
           .WithResultNameKind(ResultNameKind.FirstArgument)
           .ConstantFoldable();
        #endregion

        #region dynamic array/object functions
        public static readonly FunctionSymbol TreePath =
            new FunctionSymbol("treepath", ScalarTypes.Dynamic,
                new Parameter("object", ScalarTypes.Dynamic))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("tree")
            .ConstantFoldable();

        public static readonly FunctionSymbol Repeat =
            new FunctionSymbol("repeat", ScalarTypes.Dynamic,
                new Parameter("value", ParameterTypeKind.Scalar),
                new Parameter("count", ScalarTypes.Long))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("repeat")
            .ConstantFoldable();

        public static readonly FunctionSymbol Arraylength_Depricated =
            new FunctionSymbol("arraylength", ScalarTypes.Long,
                new Parameter("array", ScalarTypes.Dynamic))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable()
            .Hide();

        public static readonly FunctionSymbol ArrayLength =
            new FunctionSymbol("array_length", ScalarTypes.Long,
                new Parameter("array", ScalarTypes.Dynamic))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol Range =
            new FunctionSymbol("range", ScalarTypes.Dynamic,
                new Parameter("start", ParameterTypeKind.Summable),
                new Parameter("stop", ParameterTypeKind.Summable),
                new Parameter("step", ParameterTypeKind.Summable, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol ArrayConcat =
            new FunctionSymbol("array_concat", ScalarTypes.Dynamic,
                new Parameter("array", ScalarTypes.Dynamic, maxOccurring: 64))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol ArrayIff =
            new FunctionSymbol("array_iff", ScalarTypes.Dynamic,
                new Parameter("condition_array", ScalarTypes.Dynamic),
                new Parameter("when_true", ParameterTypeKind.Scalar),
                new Parameter("when_false", ParameterTypeKind.Scalar))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol ArrayIif =
            new FunctionSymbol("array_iif", ScalarTypes.Dynamic,
                new Parameter("condition_array", ScalarTypes.Dynamic),
                new Parameter("when_true", ParameterTypeKind.Scalar),
                new Parameter("when_false", ParameterTypeKind.Scalar))
            .ConstantFoldable()
            .Hide();

        public static readonly FunctionSymbol ArrayIndexOf =
            new FunctionSymbol("array_index_of", ScalarTypes.Long,
                new Parameter("array", ScalarTypes.Dynamic),
                new Parameter("value", ParameterTypeKind.Scalar))
            .ConstantFoldable();

        public static readonly FunctionSymbol SetHasElement =
            new FunctionSymbol("set_has_element", ScalarTypes.Bool,
                new Parameter("set", ScalarTypes.Dynamic),
                new Parameter("value", ParameterTypeKind.Scalar))
            .ConstantFoldable();

        public static readonly FunctionSymbol ArraySlice =
            new FunctionSymbol("array_slice", ScalarTypes.Dynamic,
                new Parameter("array", ScalarTypes.Dynamic),
                new Parameter("start", ParameterTypeKind.Integer),
                new Parameter("end", ParameterTypeKind.Integer))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol ArraySplit =
            new FunctionSymbol("array_split",
                new Signature(ScalarTypes.Dynamic,
                    new Parameter("array", ScalarTypes.Dynamic),
                    new Parameter("index", ParameterTypeKind.Integer)),
                new Signature(ScalarTypes.Dynamic,
                    new Parameter("array", ScalarTypes.Dynamic),
                    new Parameter("indices", ScalarTypes.Dynamic)))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol ArrayShiftLeft =
            new FunctionSymbol("array_shift_left", ScalarTypes.Dynamic,
                new Parameter("array", ScalarTypes.Dynamic),
                new Parameter("shift_count", ParameterTypeKind.Integer),
                new Parameter("default_value", ParameterTypeKind.Scalar, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol ArrayShiftRight =
            new FunctionSymbol("array_shift_right", ScalarTypes.Dynamic,
                new Parameter("array", ScalarTypes.Dynamic),
                new Parameter("shift_count", ParameterTypeKind.Integer),
                new Parameter("default_value", ParameterTypeKind.Scalar, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol ArrayRotateLeft =
             new FunctionSymbol("array_rotate_left", ScalarTypes.Dynamic,
                 new Parameter("array", ScalarTypes.Dynamic),
                 new Parameter("rotate_count", ParameterTypeKind.Integer))
             .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol ArrayRotateRight =
            new FunctionSymbol("array_rotate_right", ScalarTypes.Dynamic,
                new Parameter("array", ScalarTypes.Dynamic),
                new Parameter("rotate_count", ParameterTypeKind.Integer))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol BagKeys =
            new FunctionSymbol("bag_keys", ScalarTypes.Dynamic,
                new Parameter("object", ScalarTypes.Dynamic))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol Zip =
            new FunctionSymbol("zip", ScalarTypes.Dynamic,
                new Parameter("array", ScalarTypes.Dynamic, minOccurring: 2, maxOccurring: 64))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol Pack =
            new FunctionSymbol("pack", ScalarTypes.Dynamic,
                new Parameter("key", ScalarTypes.String, maxOccurring: MaxRepeat),
                new Parameter("value", ParameterTypeKind.Scalar, maxOccurring: MaxRepeat))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol PackDictionary =
            new FunctionSymbol("pack_dictionary", ScalarTypes.Dynamic,
                new Parameter("key", ScalarTypes.String, maxOccurring: MaxRepeat),
                new Parameter("value", ParameterTypeKind.Scalar, maxOccurring: MaxRepeat))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol PackAll =
            new FunctionSymbol("pack_all", ScalarTypes.Dynamic)
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol PackArray =
            new FunctionSymbol("pack_array", ScalarTypes.Dynamic,
                new Parameter("value", ParameterTypeKind.Scalar, maxOccurring: MaxRepeat))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol SetUnion =
            new FunctionSymbol("set_union", ScalarTypes.Dynamic,
                new Parameter("set", ScalarTypes.Dynamic, minOccurring: 2, maxOccurring: 64))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol SetIntersect =
            new FunctionSymbol("set_intersect", ScalarTypes.Dynamic,
                new Parameter("set", ScalarTypes.Dynamic, minOccurring: 2, maxOccurring: 64))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol SetDifference =
            new FunctionSymbol("set_difference", ScalarTypes.Dynamic,
                new Parameter("set", ScalarTypes.Dynamic, minOccurring: 2, maxOccurring: 64))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol BagMerge =
            new FunctionSymbol("bag_merge", ScalarTypes.Dynamic,
                new Parameter("bag", ScalarTypes.Dynamic, minOccurring: 2, maxOccurring: 64))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        #endregion

        #region digest / series functions
        public static readonly FunctionSymbol PercentileTDigest =
            new FunctionSymbol("percentile_tdigest",
                new Signature(ScalarTypes.Dynamic,
                    new Parameter("tdigest", ScalarTypes.Dynamic),
                    new Parameter("percentile1", ScalarTypes.Real)),
                new Signature(ReturnTypeKind.ParameterNLiteral,
                    new Parameter("tdigest", ScalarTypes.Dynamic),
                    new Parameter("percentile1", ScalarTypes.Real),
                    new Parameter("type", ScalarTypes.Type, ArgumentKind.Literal)))
            .WithResultNameKind(ResultNameKind.NameAndFirstArgument);

        // does not exists in engine?
#if false
        public static readonly FunctionSymbol PercentileArrayTDigest =
            new FunctionSymbol("percentile_array_tdigest",
                new Signature(ScalarTypes.Dynamic,
                    new Parameter("tdigest", ScalarTypes.Dynamic),
                    new Parameter("percentile", ScalarTypes.Real, maxOccurring: MaxRepeat)),
                new Signature(ScalarTypes.Dynamic,
                    new Parameter("tdigest", ScalarTypes.Dynamic),
                    new Parameter("percentiles", ScalarTypes.Dynamic)));
#endif

        public static readonly FunctionSymbol PercentRankTDigest =
            new FunctionSymbol("percentrank_tdigest", ScalarTypes.Real,
                    new Parameter("digest", ScalarTypes.Dynamic),
                    new Parameter("value", ParameterTypeKind.Scalar))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol RankTDigest =
            new FunctionSymbol("rank_tdigest", ScalarTypes.Real,
                new Parameter("digest", ScalarTypes.Dynamic),
                new Parameter("value", ParameterTypeKind.Scalar))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol TDigestMerge =
            new FunctionSymbol("tdigest_merge", ScalarTypes.Dynamic,
                new Parameter("tdigest", ScalarTypes.Dynamic, minOccurring: 2, maxOccurring: 16))
            .WithResultNameKind(ResultNameKind.PrefixOnly)
            .WithResultNamePrefix("tdigests_merge_result");

        public static readonly FunctionSymbol MergeTDigests =
            new FunctionSymbol("merge_tdigests", ScalarTypes.Dynamic,
                new Parameter("tdigest", ScalarTypes.Dynamic, minOccurring: 2, maxOccurring: 16))
            .WithResultNameKind(ResultNameKind.PrefixOnly)
            .WithResultNamePrefix("tdigests_merge_result");

        public static readonly FunctionSymbol HllMerge =
            new FunctionSymbol("hll_merge", ScalarTypes.Dynamic,
                new Parameter("hll", ScalarTypes.Dynamic, minOccurring: 2, maxOccurring: 16))
            .WithResultNameKind(ResultNameKind.PrefixOnly)
            .WithResultNamePrefix("hll_merged_result");

        public static readonly FunctionSymbol DCountHll =
            new FunctionSymbol("dcount_hll", ScalarTypes.Long,
                new Parameter("hll", ScalarTypes.Dynamic))
            .WithResultNameKind(ResultNameKind.NameAndFirstArgument);

        public static readonly FunctionSymbol SeriesFir =
            new FunctionSymbol("series_fir", ScalarTypes.Dynamic,
                new Parameter("series", ScalarTypes.Dynamic),
                new Parameter("filter", ScalarTypes.Dynamic),
                new Parameter("normalize", ScalarTypes.Bool, minOccurring: 0),
                new Parameter("center", ScalarTypes.Bool, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol SeriesStats =
            new FunctionSymbol("series_stats",
                (table, args, sig) => MakePrefixedTuple(sig, "series", args,
                    new TupleSymbol(
                        new ColumnSymbol("min", ScalarTypes.Real),
                        new ColumnSymbol("min_idx", ScalarTypes.Long),
                        new ColumnSymbol("max", ScalarTypes.Real),
                        new ColumnSymbol("max_idx", ScalarTypes.Long),
                        new ColumnSymbol("avg", ScalarTypes.Real),
                        new ColumnSymbol("stdev", ScalarTypes.Real),
                        new ColumnSymbol("variance", ScalarTypes.Real))),
                Tabularity.Scalar,
                new Parameter("series", ScalarTypes.Dynamic),
                new Parameter("ignore_nonfinite", ScalarTypes.Bool, minOccurring:0));

        public static readonly FunctionSymbol SeriesStatsDynamic =
            new FunctionSymbol("series_stats_dynamic", ScalarTypes.Dynamic,
                new Parameter("series", ScalarTypes.Dynamic),
                new Parameter("ignore_nonfinite", ScalarTypes.Bool, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesFitLine =
            new FunctionSymbol("series_fit_line",
                (table, args, sig) => MakePrefixedTuple(sig, "series", args,
                    new TupleSymbol(
                        new ColumnSymbol("rsquare", ScalarTypes.Real),
                        new ColumnSymbol("slope", ScalarTypes.Real),
                        new ColumnSymbol("variance", ScalarTypes.Real),
                        new ColumnSymbol("rvariance", ScalarTypes.Real),
                        new ColumnSymbol("interception", ScalarTypes.Real),
                        new ColumnSymbol("line_fit", ScalarTypes.Dynamic))),
                Tabularity.Scalar,
                new Parameter("series", ScalarTypes.Dynamic));

        public static readonly FunctionSymbol SeriesFitLineDynamic =
            new FunctionSymbol("series_fit_line_dynamic", ScalarTypes.Dynamic,
                new Parameter("series", ScalarTypes.Dynamic))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesFit2Lines =
            new FunctionSymbol("series_fit_2lines",
                (table, args, sig) => MakePrefixedTuple(sig, "array", args,
                    new TupleSymbol(
                        new ColumnSymbol("rsquare", ScalarTypes.Real),
                        new ColumnSymbol("split_idx", ScalarTypes.Long),
                        new ColumnSymbol("variance", ScalarTypes.Real),
                        new ColumnSymbol("rvariance", ScalarTypes.Real),
                        new ColumnSymbol("line_fit", ScalarTypes.Dynamic),
                        new ColumnSymbol("right_rsquare", ScalarTypes.Real),
                        new ColumnSymbol("right_slope", ScalarTypes.Real),
                        new ColumnSymbol("right_interception", ScalarTypes.Real),
                        new ColumnSymbol("right_variance", ScalarTypes.Real),
                        new ColumnSymbol("right_rvariance", ScalarTypes.Real),
                        new ColumnSymbol("left_rsquare", ScalarTypes.Real),
                        new ColumnSymbol("left_slope", ScalarTypes.Real),
                        new ColumnSymbol("left_interception", ScalarTypes.Real),
                        new ColumnSymbol("left_variance", ScalarTypes.Real),
                        new ColumnSymbol("left_rvariance", ScalarTypes.Real))),
                Tabularity.Scalar,
                new Parameter("array", ScalarTypes.Dynamic));

        public static readonly FunctionSymbol SeriesFit2LinesDynamic =
            new FunctionSymbol("series_fit_2lines_dynamic", ScalarTypes.Dynamic,
                new Parameter("series", ScalarTypes.Dynamic))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesOutliers =
            new FunctionSymbol("series_outliers",
                new Signature(ScalarTypes.Dynamic,
                    new Parameter("series", ScalarTypes.Dynamic),
                    new Parameter("kind", ScalarTypes.String, minOccurring: 0),
                    new Parameter("ignore_val", ParameterTypeKind.Number, minOccurring: 0)),
                new Signature(ScalarTypes.Dynamic,
                    new Parameter("series", ScalarTypes.Dynamic),
                    new Parameter("kind", ScalarTypes.String),
                    new Parameter("ignore_val", ScalarTypes.Real),
                    new Parameter("min_percentile", ScalarTypes.Real),
                    new Parameter("max_percentile", ScalarTypes.Real, minOccurring: 0),
                    new Parameter("test_points", ParameterTypeKind.Integer, minOccurring: 0)))
            .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol SeriesIIR =
            new FunctionSymbol("series_iir", ScalarTypes.Dynamic,
                new Parameter("series", ScalarTypes.Dynamic),
                new Parameter("numerators", ScalarTypes.Dynamic),
                new Parameter("denominators", ScalarTypes.Dynamic))
            .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol SeriesPeriodsDetect =
            new FunctionSymbol("series_periods_detect",
                (table, args, sig) => MakePrefixedTuple(sig, "series", args,
                    new TupleSymbol(
                        new ColumnSymbol("periods", ScalarTypes.Dynamic),
                        new ColumnSymbol("scores", ScalarTypes.Dynamic))),
                Tabularity.Scalar,
                new Parameter("series", ScalarTypes.Dynamic),
                new Parameter("min_period", ParameterTypeKind.Number),
                new Parameter("max_period", ParameterTypeKind.Number),
                new Parameter("num_periods", ScalarTypes.Long));

        public static readonly FunctionSymbol SeriesPeriodsValidate =
            new FunctionSymbol("series_periods_validate",
                (table, args, sig) => MakePrefixedTuple(sig, "series", args,
                    new TupleSymbol(
                        new ColumnSymbol("periods", ScalarTypes.Dynamic),
                        new ColumnSymbol("scores", ScalarTypes.Dynamic))),
                Tabularity.Scalar,
                new Parameter("series", ScalarTypes.Dynamic),
                new Parameter("period", ParameterTypeKind.Number, maxOccurring: 16));

        public static readonly FunctionSymbol SeriesFillBackwards =
            new FunctionSymbol("series_fill_backward", ScalarTypes.Dynamic,
                new Parameter("series", ScalarTypes.Dynamic),
                new Parameter("missing_value_placeholder", ParameterTypeKind.Number, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol SeriesFillForward =
            new FunctionSymbol("series_fill_forward", ScalarTypes.Dynamic,
                new Parameter("series", ScalarTypes.Dynamic),
                new Parameter("missing_value_placeholder", ParameterTypeKind.Number, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol SeriesFillConst =
            new FunctionSymbol("series_fill_const", ScalarTypes.Dynamic,
                new Parameter("series", ScalarTypes.Dynamic),
                new Parameter("constant_value", ParameterTypeKind.Number, minOccurring: 0),
                new Parameter("missing_value_placeholder", ParameterTypeKind.Number, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol SeriesFillLinear =
            new FunctionSymbol("series_fill_linear", ScalarTypes.Dynamic,
                new Parameter("series", ScalarTypes.Dynamic),
                new Parameter("missing_value_placeholder", ParameterTypeKind.Number, minOccurring: 0),
                new Parameter("fill_edges", ScalarTypes.Bool, minOccurring: 0),
                new Parameter("constant_value", ParameterTypeKind.Number, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol SeriesAdd =
            new FunctionSymbol("series_add", ScalarTypes.Dynamic,
                new Parameter("series1", ScalarTypes.Dynamic),
                new Parameter("series2", ScalarTypes.Dynamic))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesSubtract =
            new FunctionSymbol("series_subtract", ScalarTypes.Dynamic,
                new Parameter("series1", ScalarTypes.Dynamic),
                new Parameter("series2", ScalarTypes.Dynamic))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesMultiply =
            new FunctionSymbol("series_multiply", ScalarTypes.Dynamic,
                new Parameter("series1", ScalarTypes.Dynamic),
                new Parameter("series2", ScalarTypes.Dynamic))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesDivide =
            new FunctionSymbol("series_divide", ScalarTypes.Dynamic,
                new Parameter("series1", ScalarTypes.Dynamic),
                new Parameter("series2", ScalarTypes.Dynamic))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesGreater =
            new FunctionSymbol("series_greater", ScalarTypes.Dynamic,
                new Parameter("series1", ScalarTypes.Dynamic),
                new Parameter("series2", ScalarTypes.Dynamic))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesGreaterEquals =
            new FunctionSymbol("series_greater_equals", ScalarTypes.Dynamic,
                new Parameter("series1", ScalarTypes.Dynamic),
                new Parameter("series2", ScalarTypes.Dynamic))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesLess =
            new FunctionSymbol("series_less", ScalarTypes.Dynamic,
                new Parameter("series1", ScalarTypes.Dynamic),
                new Parameter("series2", ScalarTypes.Dynamic))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesLessEquals =
            new FunctionSymbol("series_less_equals", ScalarTypes.Dynamic,
                new Parameter("series1", ScalarTypes.Dynamic),
                new Parameter("series2", ScalarTypes.Dynamic))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesEquals =
            new FunctionSymbol("series_equals", ScalarTypes.Dynamic,
                new Parameter("series1", ScalarTypes.Dynamic),
                new Parameter("series2", ScalarTypes.Dynamic))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesNotEquals =
            new FunctionSymbol("series_not_equals", ScalarTypes.Dynamic,
                new Parameter("series1", ScalarTypes.Dynamic),
                new Parameter("series2", ScalarTypes.Dynamic))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesSeasonal =
            new FunctionSymbol("series_seasonal",
                new Signature(ScalarTypes.Dynamic,
                    new Parameter("series", ScalarTypes.Dynamic)),
                new Signature(ScalarTypes.Dynamic,
                    new Parameter("series", ScalarTypes.Dynamic),
                    new Parameter("period", ParameterTypeKind.Integer)),
                new Signature(ScalarTypes.Dynamic,
                    new Parameter("series", ScalarTypes.Dynamic),
                    new Parameter("period", ParameterTypeKind.Integer),
                    new Parameter("test_points", ParameterTypeKind.Integer)))
            .WithResultNameKind(ResultNameKind.None);

        private static TypeSymbol SeriesDecomposeResult(TableSymbol table, IReadOnlyList<Syntax.Expression> args, Signature sig) =>
            MakePrefixedTuple(sig, "series", args,
                new TupleSymbol(
                    new ColumnSymbol("baseline", ScalarTypes.Dynamic),
                    new ColumnSymbol("seasonal", ScalarTypes.Dynamic),
                    new ColumnSymbol("trend", ScalarTypes.Dynamic),
                    new ColumnSymbol("residual", ScalarTypes.Dynamic)));

        private static TypeSymbol SeriesDecomposeAnomaliesResult(TableSymbol table, IReadOnlyList<Syntax.Expression> args, Signature sig) =>
            MakePrefixedTuple(sig, "series", args,
                new TupleSymbol(
                    new ColumnSymbol("ad_flag", ScalarTypes.Dynamic),
                    new ColumnSymbol("ad_score", ScalarTypes.Dynamic),
                    new ColumnSymbol("baseline", ScalarTypes.Dynamic)));

        public static readonly FunctionSymbol SeriesDecompose =
             new FunctionSymbol("series_decompose",
                new Signature(SeriesDecomposeResult, Tabularity.Scalar,
                    new Parameter("series", ScalarTypes.Dynamic),
                    new Parameter("period", ParameterTypeKind.Integer, minOccurring: 0),
                    new Parameter("trend", ScalarTypes.String, minOccurring: 0),
                    new Parameter("test_points", ParameterTypeKind.Integer, minOccurring: 0),
                    new Parameter("seasonality_threshold", ParameterTypeKind.Number, minOccurring: 0)));

        public static readonly FunctionSymbol SeriesDecomposeForecast =
             new FunctionSymbol("series_decompose_forecast",
                new Signature(SeriesDecomposeResult, Tabularity.Scalar,
                    new Parameter("series", ScalarTypes.Dynamic),
                    new Parameter("test_points", ParameterTypeKind.Integer),
                    new Parameter("period", ParameterTypeKind.Integer, minOccurring: 0),
                    new Parameter("trend", ScalarTypes.String, minOccurring: 0),
                    new Parameter("seasonality_threshold", ParameterTypeKind.Number, minOccurring: 0)));

        public static readonly FunctionSymbol SeriesDecomposeAnomalies =
             new FunctionSymbol("series_decompose_anomalies",
                new Signature(SeriesDecomposeAnomaliesResult, Tabularity.Scalar,
                    new Parameter("series", ScalarTypes.Dynamic),
                    new Parameter("threshold", ParameterTypeKind.Number, minOccurring: 0),
                    new Parameter("period", ParameterTypeKind.Integer, minOccurring: 0),
                    new Parameter("trend", ScalarTypes.String, minOccurring: 0),
                    new Parameter("test_points", ParameterTypeKind.Integer, minOccurring: 0),
                    new Parameter("method", ScalarTypes.String, minOccurring: 0),
                    new Parameter("seasonality_threshold", ParameterTypeKind.Number, minOccurring: 0)));

        public static readonly FunctionSymbol SeriesPearsonCorrelation =
            new FunctionSymbol("series_pearson_correlation", ScalarTypes.Real,
                new Parameter("series1", ScalarTypes.Dynamic),
                new Parameter("series2", ScalarTypes.Dynamic))
            .WithResultNameKind(ResultNameKind.None);
        #endregion

        #region math functions
        public static readonly FunctionSymbol Round =
            new FunctionSymbol("round", ReturnTypeKind.Parameter0,
                new Parameter("number", ParameterTypeKind.Number),
                new Parameter("precision", ScalarTypes.Long, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol Ceiling =
            new FunctionSymbol("ceiling", ReturnTypeKind.Parameter0,
                new Parameter("number", ParameterTypeKind.Number))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol Pow =
            new FunctionSymbol("pow", ScalarTypes.Real,
                new Parameter("base", ParameterTypeKind.Number),
                new Parameter("exponent", ParameterTypeKind.Number))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol Sqrt =
            new FunctionSymbol("sqrt", ScalarTypes.Real,
                new Parameter("number", ParameterTypeKind.Number))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol Log =
            new FunctionSymbol("log", ScalarTypes.Real,
                new Parameter("number", ParameterTypeKind.Number))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol Log2 =
            new FunctionSymbol("log2", ScalarTypes.Real,
                new Parameter("number", ParameterTypeKind.Number))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol Log10 =
            new FunctionSymbol("log10", ScalarTypes.Real,
                new Parameter("number", ParameterTypeKind.Number))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol Exp =
            new FunctionSymbol("exp", ScalarTypes.Real,
                new Parameter("number", ParameterTypeKind.Number))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol Exp2 =
            new FunctionSymbol("exp2", ScalarTypes.Real,
                new Parameter("number", ParameterTypeKind.Number))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol Exp10 =
            new FunctionSymbol("exp10", ScalarTypes.Real,
                new Parameter("number", ParameterTypeKind.Number))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol PI =
            new FunctionSymbol("pi", ScalarTypes.Real)
            .ConstantFoldable();

        public static readonly FunctionSymbol Cos =
            new FunctionSymbol("cos", ScalarTypes.Real,
                new Parameter("number", ParameterTypeKind.Number))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol Sin =
            new FunctionSymbol("sin", ScalarTypes.Real,
                new Parameter("number", ParameterTypeKind.Number))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol Tan =
            new FunctionSymbol("tan", ScalarTypes.Real,
                new Parameter("number", ParameterTypeKind.Number))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol Acos =
            new FunctionSymbol("acos", ScalarTypes.Real,
                new Parameter("number", ParameterTypeKind.Number))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol Asin =
            new FunctionSymbol("asin", ScalarTypes.Real,
                new Parameter("number", ParameterTypeKind.Number))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol Atan =
            new FunctionSymbol("atan", ScalarTypes.Real,
                new Parameter("number", ParameterTypeKind.Number))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol Atan2 =
            new FunctionSymbol("atan2", ScalarTypes.Real,
                new Parameter("y", ParameterTypeKind.Number),
                new Parameter("x", ParameterTypeKind.Number))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol Abs =
            new FunctionSymbol("abs",
                new Signature(ScalarTypes.Long,
                    new Parameter("number", ParameterTypeKind.Integer)),
                new Signature(ScalarTypes.Real,
                    new Parameter("number", ParameterTypeKind.RealOrDecimal)))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol Cot =
            new FunctionSymbol("cot", ScalarTypes.Real,
                new Parameter("number", ParameterTypeKind.Number))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol Degrees =
            new FunctionSymbol("degrees", ScalarTypes.Real,
                new Parameter("radians", ParameterTypeKind.Number))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol Radians =
            new FunctionSymbol("radians", ScalarTypes.Real,
                new Parameter("degrees", ParameterTypeKind.Number))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol Sign =
            new FunctionSymbol("sign", ScalarTypes.Real,
                new Parameter("number", ParameterTypeKind.Number))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol Rand =
            new FunctionSymbol("rand", ScalarTypes.Real,
                new Parameter("number", ParameterTypeKind.Integer, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol BetaCdf =
            new FunctionSymbol("beta_cdf", ScalarTypes.Real,
                new Parameter("x", ParameterTypeKind.Number),
                new Parameter("alpha", ParameterTypeKind.Number), 
                new Parameter("beta", ParameterTypeKind.Number)).ConstantFoldable();

        public static readonly FunctionSymbol BetaInv =
            new FunctionSymbol("beta_inv", ScalarTypes.Real,
                new Parameter("probability", ParameterTypeKind.Number),
                new Parameter("alpha", ParameterTypeKind.Number),
                new Parameter("beta", ParameterTypeKind.Number)).ConstantFoldable();

        public static readonly FunctionSymbol BetaPdf =
            new FunctionSymbol("beta_pdf", ScalarTypes.Real,
                new Parameter("x", ParameterTypeKind.Number),
                new Parameter("alpha", ParameterTypeKind.Number),
                new Parameter("beta", ParameterTypeKind.Number)).ConstantFoldable();

        public static readonly FunctionSymbol Gamma =
            new FunctionSymbol("gamma", ScalarTypes.Real,
                new Parameter("number", ParameterTypeKind.Number))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol LogGamma =
            new FunctionSymbol("loggamma", ScalarTypes.Real,
                new Parameter("number", ParameterTypeKind.Number))
            .WithResultNameKind(ResultNameKind.FirstArgument)
            .ConstantFoldable();

        public static readonly FunctionSymbol IsNan =
            new FunctionSymbol("isnan", ScalarTypes.Bool,
                new Parameter("number", ScalarTypes.Real))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol IsInf =
            new FunctionSymbol("isinf", ScalarTypes.Bool,
                new Parameter("number", ScalarTypes.Real))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol IsFinite =
            new FunctionSymbol("isfinite", ScalarTypes.Bool,
                new Parameter("number", ScalarTypes.Real))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol Coalesce =
            new FunctionSymbol("coalesce", ReturnTypeKind.Common,
                new Parameter("arg", ParameterTypeKind.CommonScalar, minOccurring: 2, maxOccurring: 64))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol MaxOf =
            new FunctionSymbol("max_of", ReturnTypeKind.Common,
                new Parameter("arg", ParameterTypeKind.CommonSummable, minOccurring: 2, maxOccurring: 64))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol MinOf =
            new FunctionSymbol("min_of", ReturnTypeKind.Common,
                new Parameter("arg", ParameterTypeKind.CommonSummable, minOccurring: 2, maxOccurring: 64))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol WelchTest =
            new FunctionSymbol("welch_test", ScalarTypes.Real,
                new Parameter("mean1", ParameterTypeKind.Number),
                new Parameter("variance1", ParameterTypeKind.Number),
                new Parameter("count1", ParameterTypeKind.Number),
                new Parameter("mean2", ParameterTypeKind.Number),
                new Parameter("variance2", ParameterTypeKind.Number),
                new Parameter("count2", ParameterTypeKind.Number))
            .WithResultNameKind(ResultNameKind.None);
#endregion

        #region geospatial functions

        public static readonly FunctionSymbol GeoDistance2Points =
            new FunctionSymbol("geo_distance_2points", ScalarTypes.Real,
                new Parameter("p1_longitude", ParameterTypeKind.Number),
                new Parameter("p1_latitude", ParameterTypeKind.Number),
                new Parameter("p2_longitude", ParameterTypeKind.Number),
                new Parameter("p2_latitude", ParameterTypeKind.Number))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol GeoDistancePointToLine =
            new FunctionSymbol("geo_distance_point_to_line", ScalarTypes.Real,
                new Parameter("longitude", ParameterTypeKind.Number),
                new Parameter("latitude", ParameterTypeKind.Number),
                new Parameter("lineString", ScalarTypes.Dynamic))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol GeoPointInCircle =
            new FunctionSymbol("geo_point_in_circle", ScalarTypes.Bool,
                new Parameter("p_longitude", ParameterTypeKind.Number),
                new Parameter("p_latitude", ParameterTypeKind.Number),
                new Parameter("pc_longitude", ParameterTypeKind.Number),
                new Parameter("pc_latitude", ParameterTypeKind.Number),
                new Parameter("c_radius", ParameterTypeKind.Number))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol GeoPointInPolygon =
            new FunctionSymbol("geo_point_in_polygon", ScalarTypes.Bool,
                new Parameter("longitude", ParameterTypeKind.Number),
                new Parameter("latitude", ParameterTypeKind.Number),
                new Parameter("polygon", ScalarTypes.Dynamic))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol GeoPolygonToS2Cells =
            new FunctionSymbol("geo_polygon_to_s2cells", ScalarTypes.Dynamic,
                new Parameter("polygon", ScalarTypes.Dynamic),
                new Parameter("level", ParameterTypeKind.Number, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol GeoPolygonValidate =
            new FunctionSymbol("__geo_polygon_validate", ScalarTypes.String,
                new Parameter("polygon", ScalarTypes.Dynamic))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable()
            .Hide();

        public static readonly FunctionSymbol GeoPointToGeohash =
            new FunctionSymbol("geo_point_to_geohash", ScalarTypes.String,
                new Parameter("longitude", ParameterTypeKind.Number),
                new Parameter("latitude", ParameterTypeKind.Number),
                new Parameter("accuracy", ParameterTypeKind.Number, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol GeohashToCentralPoint =
            new FunctionSymbol("geo_geohash_to_central_point", ScalarTypes.Dynamic,
                new Parameter("geohash", ScalarTypes.String))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol GeoPointToS2Cell =
            new FunctionSymbol("geo_point_to_s2cell", ScalarTypes.String,
                new Parameter("longitude", ParameterTypeKind.Number),
                new Parameter("latitude", ParameterTypeKind.Number),
                new Parameter("level", ParameterTypeKind.Number, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();

        public static readonly FunctionSymbol S2CellToCentralPoint =
            new FunctionSymbol("geo_s2cell_to_central_point", ScalarTypes.Dynamic,
                new Parameter("s2cell", ScalarTypes.String))
            .WithResultNameKind(ResultNameKind.None)
            .ConstantFoldable();
        #endregion

        #region other
        public static readonly FunctionSymbol CurrentClusterEndpoint =
            new FunctionSymbol("current_cluster_endpoint", ScalarTypes.String);

        public static readonly FunctionSymbol CurrentDatabase =
            new FunctionSymbol("current_database", ScalarTypes.String);

        public static readonly FunctionSymbol CurrentPrincipal =
            new FunctionSymbol("current_principal", ScalarTypes.String);
        // result column name is dependent on some guid?

        public static readonly FunctionSymbol CurrentPrincipalDetails =
            new FunctionSymbol("current_principal_details", ScalarTypes.Dynamic)
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol CurrentPrincipalIsMemberOf =
          new FunctionSymbol("current_principal_is_member_of", ScalarTypes.Bool, 
              new Parameter("group", ParameterTypeKind.StringOrDynamic, minOccurring: 1, maxOccurring: 64))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol ExtentId =
            new FunctionSymbol("extent_id", ScalarTypes.Guid)
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol ExtentId2 =
            new FunctionSymbol("extentid", ScalarTypes.Guid).Hide();

        public static readonly FunctionSymbol ExtentTags =
            new FunctionSymbol("extent_tags", ScalarTypes.Dynamic)
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol CurrentNodeId =
            new FunctionSymbol("current_node_id", ScalarTypes.String)
            .WithResultNameKind(ResultNameKind.None)
            .Hide();

        public static readonly FunctionSymbol IngestionTime =
            new FunctionSymbol("ingestion_time", ScalarTypes.DateTime)
            .WithResultNameKind(ResultNameKind.PrefixOnly)
            .WithResultNamePrefix("$IngestionTime");

        public static readonly FunctionSymbol CursorAfter =
            new FunctionSymbol("cursor_after", ScalarTypes.Bool,
                new Parameter("cursor", ScalarTypes.String));

        public static readonly FunctionSymbol CursorBeforeOrAt =
            new FunctionSymbol("cursor_before_or_at", ScalarTypes.Bool,
                new Parameter("cursor", ScalarTypes.String));

        public static readonly FunctionSymbol CursorCurrent =
            new FunctionSymbol("cursor_current", ScalarTypes.String);

        public static readonly FunctionSymbol CursorCurrent2 =
            new FunctionSymbol("current_cursor", ScalarTypes.String).Hide();

        public static readonly FunctionSymbol FormatBytes=
            new FunctionSymbol("format_bytes", ScalarTypes.String,
                new Parameter("size", ParameterTypeKind.Number),
                new Parameter("precision", ScalarTypes.Long, minOccurring: 0),
                new Parameter("format", ScalarTypes.String, ArgumentKind.LiteralNotEmpty, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol RowNumber =
            new FunctionSymbol("row_number", ScalarTypes.Long,
                new Parameter("startingIndex", ScalarTypes.Long, minOccurring: 0),
                new Parameter("restart", ScalarTypes.Bool, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol RowCumSum =
            new FunctionSymbol("row_cumsum", ReturnTypeKind.Parameter0,
                new Parameter("term", ParameterTypeKind.Number),
                new Parameter("restart", ScalarTypes.Bool, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol RowWindowSession =
            new FunctionSymbol("row_window_session", ReturnTypeKind.Parameter0,
                new Parameter("expr", ScalarTypes.DateTime),
                new Parameter("maxDistanceFromFirst", ScalarTypes.TimeSpan),
                new Parameter("minDistanceBetweenNeighbors", ScalarTypes.TimeSpan),
                new Parameter("restart", ScalarTypes.Bool, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol Prev =
            new FunctionSymbol("prev", ReturnTypeKind.Parameter0,
                new Parameter("column", ParameterTypeKind.Scalar, ArgumentKind.Column),
                new Parameter("offset", ScalarTypes.Long, minOccurring: 0),
                new Parameter("default_value", ParameterTypeKind.Scalar, ArgumentKind.Constant, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol Next =
            new FunctionSymbol("next", ReturnTypeKind.Parameter0,
                new Parameter("column", ParameterTypeKind.Scalar, ArgumentKind.Column),
                new Parameter("offset", ScalarTypes.Long, minOccurring: 0),
                new Parameter("default_value", ParameterTypeKind.Scalar, ArgumentKind.Constant, minOccurring: 0))
           .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol RowstoreOrdinalRange =
            new FunctionSymbol("rowstore_ordinal_range", ScalarTypes.Dynamic)
            .WithResultNameKind(ResultNameKind.None)
            .Hide();

        public static readonly FunctionSymbol EstimateDataSize =
            new FunctionSymbol("estimate_data_size",
                new Signature(ScalarTypes.Long,
                    new Parameter("column", ParameterTypeKind.Scalar, ArgumentKind.Column, minOccurring: 1, maxOccurring: MaxRepeat)),
                new Signature(ScalarTypes.Long,
                    new Parameter("column", ParameterTypeKind.Scalar, ArgumentKind.Star)))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol NewGuid = new FunctionSymbol("new_guid", ScalarTypes.Guid)
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol InternalFunnelCompletion =
            new FunctionSymbol("__funnel_completion", ScalarTypes.String,
                new Parameter("events", ScalarTypes.Dynamic),
                new Parameter("times", ScalarTypes.Dynamic),
                new Parameter("sequence", ScalarTypes.Dynamic),
                new Parameter("periods", ScalarTypes.Dynamic))
            .WithResultNameKind(ResultNameKind.None)
            .Hide();
        #endregion

        #region All
        public static IReadOnlyList<FunctionSymbol> All { get; } = new FunctionSymbol[]
        {
#region cluster / database / table
            Cluster,
            Database,
            Table,
            ExternalTable,
            MaterializedView,
#endregion

#region string functions
            Strcat,
            StrcatArray,
            ArrayStrcat,
            StrcatDelim,
            Strcmp,
            Strrep,
            Strlen,
            StringSize,
            ToUpper,
            ToLower,
            ToUtf8,
            Substring,
            IndexOf,
            IndexOfRegex,
            Reverse,
            Split,
            ParseCommandLine,
            Extract,
            ExtractAll_Depricated,
            ExtractAll,
            ExtractJson,
            Replace,
            TrimStart,
            TrimEnd,
            Trim,
            CountOf,
            Translate,
            MakeString,
            DateTimeToLocaleString,
            NumberToLocaleString,
#endregion

#region type conversion functions
            ToString,
            ToHex,
            ToDynamic_,
            ToObject_Depricated,
            ToLong,
            ToInt,
            ToReal,
            ToDouble,
            ToDateTime,
            ToTimespan,
            ToTime,
            ToBool,
            ToBoolean,
            ToDecimal,
            ToGuid,
            GetType,
#endregion

#region encoding/decoding functions
            UrlEncode,
            UrlEncode_Component,
            UrlDecode,
            Base64EncodeString,
            Base64EncodeToString,
            Base64DecodeToArray,
            Base64EncodeFromArray,
            Base64DecodeString,
            Base64DecodeToString,
            #endregion

#region parsing functions
            ParseCsv,
            ParseJson_Depricated,
            ParseJson,
            ParseXml,
            ParseUrl_Depricated,
            ParseUrl,
            ParseUrlQuery_Depricated,
            ParseUrlQuery,
            ParseIPV4,
            ParseIPV4Mask,
            ParseIPV6,
            ParseIPV6Mask,
            ParsePath,
            ParseUserAgent,
            ParseVersion,
            #endregion

#region date and time functions
            FormatDatetime,
            FormatTimespan,
            MakeDatetime,
            MakeTimespan,
            DatetimeAdd,
            DatetimeDiff,
            DayOfWeek,
            DayOfMonth,
            DayOfYear,
            HourOfDay,
            WeekOfYear,
            WeekOfYearISO,
            MonthOfYear,
            StartOfDay,
            StartOfWeek,
            StartOfMonth,
            StartOfYear,
            EndOfDay,
            EndOfWeek,
            EndOfMonth,
            EndOfYear,
            GetYear,
            GetMonth,
            DatePart,
            DatetimePart,
            Now,
            Ago,
            UnixTimeSecondsToDateTime,
            UnixTimeMillisecondsToDateTime,
            UnixTimeMicrosecondsToDateTime,
            UnixTimeNanosecondsToDateTime,
#endregion

#region hash functions
            HashCrc32,
            HashDjb2,
            Hash,
            HashSha256,
            HashMd5,
            HashXXH64,
            HashCombine,
            HashMany,
            HashManyCrc32,
            #endregion

#region iif / case
            Iif,
            Iff,
            Case,
            Assert,
#endregion

#region bin / floor
            Bin,
            Floor,
            BinAt,
            BinAuto,
#endregion

#region bool functions  (test state, return bool)
            Not,
            NotNull_Depricated,
            IsNotNull,
            IsNull,
            NotEmpty_Depricated,
            IsColumnExists,
            IsAscii,
            IsUtf8,
            IsNotEmpty,
            IsEmpty,
            ColumnIfExists_Depricated,
            ColumnIfExists,
#endregion

#region bitwise functions
            BinaryAnd,
            BinaryOr,
            BinaryXor,
            BinaryNot,
            BinaryShiftRight,
            BinaryShiftLeft,
            BitsetCountOnes,
#endregion

#region dynamic array/object functions
            TreePath,
            Repeat,
            Arraylength_Depricated,
            ArrayLength,
            Range,
            ArrayConcat,
            ArrayIif,
            ArrayIff,
            ArrayIndexOf,
            ArraySlice,
            ArraySplit,
            ArrayShiftLeft,
            ArrayShiftRight,
            ArrayRotateLeft,
            ArrayRotateRight,
            BagKeys,
            Zip,
            Pack,
            PackDictionary,
            PackAll,
            PackArray,
            SetHasElement,
            SetUnion,
            SetIntersect,
            SetDifference,
            BagMerge,
#endregion

#region digest / series functions
            PercentileTDigest,
            //PercentileArrayTDigest,
            PercentRankTDigest,
            RankTDigest,
            TDigestMerge,
            MergeTDigests,
            HllMerge,
            DCountHll,
            SeriesFir,
            SeriesStats,
            SeriesStatsDynamic,
            SeriesFitLine,
            SeriesFitLineDynamic,
            SeriesFit2Lines,
            SeriesFit2LinesDynamic,
            SeriesOutliers,
            SeriesIIR,
            SeriesPeriodsDetect,
            SeriesPeriodsValidate,
            SeriesFillBackwards,
            SeriesFillForward,
            SeriesFillConst,
            SeriesFillLinear,
            SeriesAdd,
            SeriesSubtract,
            SeriesMultiply,
            SeriesDivide,
            SeriesGreater,
            SeriesGreaterEquals,
            SeriesLess,
            SeriesLessEquals,
            SeriesEquals,
            SeriesNotEquals,
            SeriesSeasonal,
            SeriesDecompose,
            SeriesDecomposeForecast,
            SeriesDecomposeAnomalies,
            SeriesPearsonCorrelation,
#endregion

#region math functions
            Round,
            Ceiling,
            Pow,
            Sqrt,
            Log,
            Log2,
            Log10,
            Exp,
            Exp2,
            Exp10,
            PI,
            Cos,
            Sin,
            Tan,
            Acos,
            Asin,
            Atan,
            Atan2,
            Abs,
            Cot,
            Degrees,
            Radians,
            Sign,
            Rand,
            BetaCdf,
            BetaInv,
            BetaPdf,
            Gamma,
            LogGamma,
            IsNan,
            IsInf,
            IsFinite,
            Coalesce,
            MaxOf,
            MinOf,
            WelchTest,
#endregion

#region geospatial functions
            GeoDistance2Points,
            GeoDistancePointToLine,
            GeoPointInCircle,
            GeoPointInPolygon,
            GeoPolygonToS2Cells,
            GeoPolygonValidate,
            GeoPointToGeohash,
            GeohashToCentralPoint,
            GeoPointToS2Cell,
            S2CellToCentralPoint,
            #endregion

            #region ip-matching functions
            Ipv4Compare,
            Ipv4IsMatch,
            Ipv6Compare,
            Ipv6IsMatch,
            #endregion

            #region other
            CurrentClusterEndpoint,
            CurrentDatabase,
            CurrentPrincipal,
            CurrentPrincipalDetails,
            CurrentPrincipalIsMemberOf,
            ExtentId,
            ExtentId2,
            ExtentTags,
            CurrentNodeId,
            IngestionTime,
            CursorAfter,
            CursorBeforeOrAt,
            CursorCurrent,
            CursorCurrent2,
            FormatBytes,
            InternalFunnelCompletion,
            RowNumber,
            RowCumSum,
            RowWindowSession,
            Prev,
            Next,
            RowstoreOrdinalRange,
            EstimateDataSize,
            NewGuid
#endregion
        };
#endregion
    }
}