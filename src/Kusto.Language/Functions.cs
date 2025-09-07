using System.Collections.Generic;

namespace Kusto.Language
{
    using System.Linq;
    using Kusto.Language.Syntax;
    using Symbols;
    using Utils;
    using static FunctionHelpers;

    /// <summary>
    /// Well known scalar and special functions.
    /// </summary>
    public partial class Functions
    {
        #region cluster / database / table / etc
        public static readonly FunctionSymbol Cluster =
            new FunctionSymbol("cluster",
                ReturnTypeKind.Parameter0Cluster,
                new Parameter("name", ScalarTypes.String));

        public static readonly FunctionSymbol Database =
            new FunctionSymbol("database",
                ReturnTypeKind.Parameter0Database,
                new Parameter("name", ScalarTypes.String, minOccurring: 0));

        public static readonly FunctionSymbol Table =
            new FunctionSymbol("table",
                ReturnTypeKind.Parameter0Table,
                new Parameter("name", ScalarTypes.String),
                new Parameter("query_data_scope", ScalarTypes.String, minOccurring: 0));

        public static readonly FunctionSymbol ExternalTable =
            new FunctionSymbol("external_table",
                new Signature(
                    ReturnTypeKind.Parameter0ExternalTable,
                    new Parameter("name", ScalarTypes.String),
                    new Parameter("mapping", ScalarTypes.String, minOccurring: 0)
                    ),
                new Signature(
                    ReturnTypeKind.Parameter0ExternalTable,
                    new Parameter("name", ScalarTypes.String),
                    new Parameter("max_age", ScalarTypes.TimeSpan)
                    ));

        public static readonly FunctionSymbol MaterializedView =
            new FunctionSymbol("materialized_view",
                ReturnTypeKind.Parameter0MaterializedView,
                new Parameter("name", ScalarTypes.String),
                new Parameter("max_age", ScalarTypes.TimeSpan, minOccurring: 0));

        public static readonly FunctionSymbol EntityGroup =
            new FunctionSymbol("entity_group",
                ReturnTypeKind.Parameter0EntityGroup,
                new Parameter("name", ScalarTypes.String));

        public static readonly FunctionSymbol StoredQueryResult =
            new FunctionSymbol("stored_query_result",
                new Signature(
                    ReturnTypeKind.Parameter0StoredQueryResult,
                    new Parameter("name", ScalarTypes.String)),
                new Signature(
                    ReturnTypeKind.Parameter0StoredQueryResult,
                    new Parameter("name", ScalarTypes.String),
                    new Parameter("index", ScalarTypes.Long)),
                new Signature(
                    ReturnTypeKind.Parameter0StoredQueryResult,
                    new Parameter("name", ScalarTypes.String),
                    new Parameter("table_name", ScalarTypes.String)));

        public static readonly FunctionSymbol Graph =
            new FunctionSymbol("graph",
                new Signature(
                    ReturnTypeKind.Parameter0Graph,
                    new Parameter("name", ScalarTypes.String),
                    new Parameter("snapshot", ScalarTypes.String, minOccurring: 0, maxOccurring: 1)
                    ),
                new Signature(
                    ReturnTypeKind.Parameter0Graph,
                    new Parameter("name", ScalarTypes.String),
                    new Parameter("volatile", ScalarTypes.Bool)
                    ));
        #endregion

        #region string functions
        public static readonly FunctionSymbol Strcat =
            new FunctionSymbol("strcat", ScalarTypes.String,
                    new Parameter("arg", ParameterTypeKind.Scalar, maxOccurring: 64))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol StrcatArray =
            new FunctionSymbol("strcat_array", ScalarTypes.String,
                    new Parameter("array", ParameterTypeKind.DynamicArray),
                    new Parameter("delimiter", ParameterTypeKind.Scalar))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol ArrayStrcat =
            new FunctionSymbol("array_strcat", ScalarTypes.String,
                    new Parameter("array", ParameterTypeKind.DynamicArray),
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
                    new Parameter("multiplier", ScalarTypes.Long),
                    new Parameter("delimiter", ParameterTypeKind.Scalar, minOccurring: 0))
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

        public static readonly FunctionSymbol ToUtf8_Deprecated =
            new FunctionSymbol("to_utf8",
                    ScalarTypes.DynamicArray,
                    new Parameter("value", ParameterTypeKind.Scalar))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable()
                .Obsolete("unicode_codepoints_from_string");

        public static readonly FunctionSymbol UnicodeCodepointsFromString =
            new FunctionSymbol("unicode_codepoints_from_string",
                    ScalarTypes.DynamicArray,
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

        public static readonly FunctionSymbol RegexQuote =
            new FunctionSymbol("regex_quote", ScalarTypes.String,
                    new Parameter("string", ParameterTypeKind.Scalar))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol IndexOf =
            new FunctionSymbol("indexof", ScalarTypes.Long,
                    new Parameter("string", ParameterTypeKind.Scalar),
                    new Parameter("match", ParameterTypeKind.Scalar),
                    new Parameter("start", ParameterTypeKind.Integer, minOccurring: 0),
                    new Parameter("length", ParameterTypeKind.Integer, minOccurring: 0),
                    new Parameter("occurrence", ParameterTypeKind.Integer, ArgumentKind.Constant, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol IndexOfRegex =
            new FunctionSymbol("indexof_regex", ScalarTypes.Long,
                    new Parameter("string", ParameterTypeKind.Scalar),
                    new Parameter("match", ParameterTypeKind.Scalar, ArgumentKind.Constant),
                    new Parameter("start", ParameterTypeKind.Integer, minOccurring: 0),
                    new Parameter("length", ParameterTypeKind.Integer, minOccurring: 0),
                    new Parameter("occurrence", ParameterTypeKind.Integer, ArgumentKind.Constant, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol HasAnyIndex =
            new FunctionSymbol("has_any_index", ScalarTypes.Long,
                    new Parameter("source", ParameterTypeKind.StringOrDynamic),
                    new Parameter("values", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol Reverse =
            new FunctionSymbol("reverse", ScalarTypes.String,
                    new Parameter("value", ParameterTypeKind.Scalar))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol Split =
            new FunctionSymbol("split",
                    ScalarTypes.DynamicArrayOfString,
                    new Parameter("source", ParameterTypeKind.Scalar),
                    new Parameter("delimiter", ScalarTypes.String),
                    new Parameter("requestedIndex", ParameterTypeKind.Integer, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.FirstArgument)
                .ConstantFoldable();

        public static readonly FunctionSymbol ParseCommandLine =
            new FunctionSymbol("parse_command_line",
                    ScalarTypes.DynamicArrayOfString,
                    new Parameter("command", ParameterTypeKind.Scalar),
                    new Parameter("parser", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol Extract =
            new FunctionSymbol("extract",
                    new Signature(ScalarTypes.String,
                        new Parameter("regex", ScalarTypes.String, ArgumentKind.Constant),
                        new Parameter("captureGroup", ScalarTypes.Long),
                        new Parameter("source", ScalarTypes.String)),
                    new Signature(ReturnTypeKind.ParameterNLiteral,
                        new Parameter("regex", ScalarTypes.String, ArgumentKind.Constant),
                        new Parameter("captureGroup", ScalarTypes.Long),
                        new Parameter("source", ScalarTypes.String),
                        new Parameter("typeLiteral", ScalarTypes.Type, ArgumentKind.Literal)))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol ExtractAll_Deprecated =
            new FunctionSymbol("extractall",
                    new Signature(
                        ScalarTypes.DynamicArray,
                        new Parameter("regex", ScalarTypes.String, ArgumentKind.Constant),
                        new Parameter("source", ScalarTypes.String)),
                    new Signature(
                        ScalarTypes.DynamicArray,
                        new Parameter("regex", ScalarTypes.String, ArgumentKind.Constant),
                        new Parameter("captureGroups", ParameterTypeKind.DynamicArray),
                        new Parameter("source", ScalarTypes.String)))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable()
                .Obsolete("extract_all");

        public static readonly FunctionSymbol ExtractAll =
            new FunctionSymbol("extract_all",
                    new Signature(
                        ScalarTypes.DynamicArray,
                        new Parameter("regex", ScalarTypes.String, ArgumentKind.Constant),
                        new Parameter("source", ScalarTypes.String)),
                    new Signature(
                        ScalarTypes.DynamicArray,
                        new Parameter("regex", ScalarTypes.String, ArgumentKind.Constant),
                        new Parameter("captureGroups", ParameterTypeKind.DynamicArray),
                        new Parameter("source", ScalarTypes.String)))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol ExtractJson_Deprecated =
            new FunctionSymbol("extractjson",
                    new Signature(ScalarTypes.String,
                        new Parameter("jsonPath", ScalarTypes.String, ArgumentKind.Constant),
                        new Parameter("jsonText", ScalarTypes.String)),
                    new Signature(ReturnTypeKind.ParameterNLiteral,
                        new Parameter("jsonPath", ScalarTypes.String, ArgumentKind.Constant),
                        new Parameter("jsonText", ScalarTypes.String),
                        new Parameter("type", ScalarTypes.Type, ArgumentKind.Literal)))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol ExtractJson =
            new FunctionSymbol("extract_json",
                    new Signature(ScalarTypes.String,
                        new Parameter("jsonPath", ScalarTypes.String, ArgumentKind.Constant),
                        new Parameter("jsonText", ScalarTypes.String)),
                    new Signature(ReturnTypeKind.ParameterNLiteral,
                        new Parameter("jsonPath", ScalarTypes.String, ArgumentKind.Constant),
                        new Parameter("jsonText", ScalarTypes.String),
                        new Parameter("type", ScalarTypes.Type, ArgumentKind.Literal)))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol Replace =
            new FunctionSymbol("replace", ScalarTypes.String,
                    new Parameter("regex", ScalarTypes.String, ArgumentKind.Constant),
                    new Parameter("rewrite", ScalarTypes.String, ArgumentKind.Constant),
                    new Parameter("source", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable()
                .Obsolete("replace_regex' or 'replace_string"); // added ' for better message formatting

        public static readonly FunctionSymbol ReplaceRegex =
            new FunctionSymbol("replace_regex", ScalarTypes.String,
                    new Parameter("source", ScalarTypes.String),
                    new Parameter("lookup_regex", ScalarTypes.String, ArgumentKind.Constant),
                    new Parameter("rewrite_pattern", ScalarTypes.String, ArgumentKind.Constant))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol ReplaceString =
            new FunctionSymbol("replace_string", ScalarTypes.String,
                    new Parameter("text", ScalarTypes.String),
                    new Parameter("lookup", ScalarTypes.String),
                    new Parameter("rewrite", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol ReplaceStrings =
            new FunctionSymbol("replace_strings", ScalarTypes.String,
                    new Parameter("text", ScalarTypes.String),
                    new Parameter("lookups", ParameterTypeKind.DynamicArray),
                    new Parameter("rewrites", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol TrimStart =
            new FunctionSymbol("trim_start", ScalarTypes.String,
                    new Parameter("regex", ScalarTypes.String, ArgumentKind.Constant),
                    new Parameter("source", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol TrimEnd =
            new FunctionSymbol("trim_end", ScalarTypes.String,
                    new Parameter("regex", ScalarTypes.String, ArgumentKind.Constant),
                    new Parameter("source", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol Trim =
            new FunctionSymbol("trim", ScalarTypes.String,
                    new Parameter("regex", ScalarTypes.String, ArgumentKind.Constant),
                    new Parameter("source", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol CountOf =
            new FunctionSymbol("countof", ScalarTypes.Long,
                    new Parameter("source", ScalarTypes.String),
                    new Parameter("search", ScalarTypes.String),
                    new Parameter("kind", ScalarTypes.String, ArgumentKind.Literal, new object[] { "normal", "regex" }, isCaseSensitive: true, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol Translate =
            new FunctionSymbol("translate", ScalarTypes.String,
                    new Parameter("searchList", ScalarTypes.String),
                    new Parameter("replacementList", ScalarTypes.String),
                    new Parameter("source", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol MakeString_Deprecated =
            new FunctionSymbol("make_string",
                    ScalarTypes.String,
                    new Parameter("value", ParameterTypeKind.IntegerOrArray, maxOccurring: MaxRepeat))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable()
                .Obsolete("unicode_codepoints_to_string");

        public static readonly FunctionSymbol UnicodeCodepointsToString =
            new FunctionSymbol("unicode_codepoints_to_string",
                    ScalarTypes.String,
                    new Parameter("value", ParameterTypeKind.IntegerOrArray, maxOccurring: MaxRepeat))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();
        #endregion

        #region type conversion functions
        public static readonly new FunctionSymbol ToString =
            new FunctionSymbol("tostring", ScalarTypes.String,
                    new Parameter("value", ParameterTypeKind.Scalar))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol ToHex =
            new FunctionSymbol("tohex", ScalarTypes.String,
                    new Parameter("value", ParameterTypeKind.Integer),
                    new Parameter("minLength", ParameterTypeKind.Integer, minOccurring: 0))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol ToDynamic_ =  // use _ because build fails claiming ToDynamic exists on object.
                new FunctionSymbol("todynamic",
                        ScalarTypes.Dynamic,
                        new Parameter("value", ScalarTypes.String))
                    .ConstantFoldable()
                    .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol ToObject_Deprecated =
            new FunctionSymbol("toobject",
                    ScalarTypes.Dynamic,
                    new Parameter("value", ScalarTypes.String))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.FirstArgument)
                .Obsolete("todynamic");

        public static readonly FunctionSymbol ToLong =
            new FunctionSymbol("tolong",
                    ScalarTypes.Long,
                    new Parameter("value", ParameterTypeKind.Scalar))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol ToInt =
            new FunctionSymbol("toint",
                    ScalarTypes.Int,
                    new Parameter("value", ParameterTypeKind.Scalar))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol ToReal =
            new FunctionSymbol("toreal",
                    ScalarTypes.Real,
                    new Parameter("value", ParameterTypeKind.Scalar))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol ToDouble =
            new FunctionSymbol("todouble",
                    ScalarTypes.Real,
                    new Parameter("value", ParameterTypeKind.Scalar))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol ToDateTime =
            new FunctionSymbol("todatetime",
                    ScalarTypes.DateTime,
                    new Parameter("value", ParameterTypeKind.Scalar))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol ToTimespan =
            new FunctionSymbol("totimespan",
                    ScalarTypes.TimeSpan,
                    new Parameter("value", ParameterTypeKind.Scalar))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol ToTime =
            new FunctionSymbol("totime",
                    ScalarTypes.TimeSpan,
                    new Parameter("value", ParameterTypeKind.Scalar))
                .Obsolete("totimespan")
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol ToBool =
            new FunctionSymbol("tobool",
                    ScalarTypes.Bool,
                    new Parameter("value", ParameterTypeKind.Scalar))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol ToBoolean =
            new FunctionSymbol("toboolean",
                    ScalarTypes.Bool,
                    new Parameter("value", ParameterTypeKind.Scalar))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol ToDecimal =
            new FunctionSymbol("todecimal",
                    ScalarTypes.Decimal,
                    new Parameter("value", ParameterTypeKind.Scalar))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol ToGuid =
            new FunctionSymbol("toguid",
                    ScalarTypes.Guid,
                    new Parameter("value", ParameterTypeKind.StringOrDynamic))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly new FunctionSymbol GetType =
            new FunctionSymbol("gettype",
                    ScalarTypes.String,
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
                .Obsolete("base64_encode_tostring")
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
                .Obsolete("base64_decode_tostring")
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol Base64DecodeToString =
            new FunctionSymbol("base64_decode_tostring",
                    ScalarTypes.String,
                    new Parameter("base64_string", ScalarTypes.String))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol Base64DecodeToArray =
            new FunctionSymbol("base64_decode_toarray",
                    ScalarTypes.DynamicArray,
                    new Parameter("base64_string", ScalarTypes.String))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol Base64DecodeToGuid =
            new FunctionSymbol("base64_decode_toguid",
                    ScalarTypes.Guid,
                    new Parameter("base64_string", ScalarTypes.String))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol Base64EncodeFromGuid =
            new FunctionSymbol("base64_encode_fromguid",
                    ScalarTypes.String,
                    new Parameter("guid", ScalarTypes.Guid))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol Base64EncodeFromArray =
            new FunctionSymbol("base64_encode_fromarray",
                    ScalarTypes.String,
                    new Parameter("base64_string_decoded_as_array", ParameterTypeKind.DynamicArray))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol ZlibDecompressString =
            new FunctionSymbol("zlib_decompress_from_base64_string",
                    ScalarTypes.String,
                    new Parameter("string", ScalarTypes.String))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol ZlibCompressString =
            new FunctionSymbol("zlib_compress_to_base64_string",
                    ScalarTypes.String,
                    new Parameter("string", ScalarTypes.String))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol GzipDecompressString =
            new FunctionSymbol("gzip_decompress_from_base64_string",
                    ScalarTypes.String,
                    new Parameter("string", ScalarTypes.String))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol GzipCompressString =
            new FunctionSymbol("gzip_compress_to_base64_string",
                    ScalarTypes.String,
                    new Parameter("string", ScalarTypes.String))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol Lz4CompressDynamicArray =
            new FunctionSymbol("__lz4_compress_dynamic_array_to_base64_string",
                    ScalarTypes.String,
                    new Parameter("dynamic", ParameterTypeKind.DynamicArray))
                .ConstantFoldable()
                .Hide()
                .WithResultNameKind(ResultNameKind.None);
        #endregion

        #region parsing functions
        public static readonly FunctionSymbol ParseCsv =
            new FunctionSymbol("parse_csv",
                    ScalarTypes.DynamicArray,
                    new Parameter("csv_text", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.FirstArgument)
                .ConstantFoldable();

        public static readonly FunctionSymbol ParseJson_Deprecated =
            new FunctionSymbol("parsejson",
                    ScalarTypes.Dynamic,
                    new Parameter("json_text", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.FirstArgument)
                .ConstantFoldable()
                .Obsolete("parse_json");

        public static readonly FunctionSymbol ParseJson =
            new FunctionSymbol("parse_json",
                    ScalarTypes.Dynamic,
                    new Parameter("json_text", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.FirstArgument)
                .ConstantFoldable();

        public static readonly FunctionSymbol ParseXml =
            new FunctionSymbol("parse_xml",
                    ScalarTypes.DynamicBag,
                    new Parameter("xml_text", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.FirstArgument)
                .ConstantFoldable();

        private static TypeSymbol ParseUrlResult =
            ScalarTypes.GetDynamicBag(
                new ColumnSymbol("Scheme", ScalarTypes.String),
                new ColumnSymbol("Host", ScalarTypes.String),
                new ColumnSymbol("Port", ScalarTypes.String),
                new ColumnSymbol("Path", ScalarTypes.String),
                new ColumnSymbol("Username", ScalarTypes.String),
                new ColumnSymbol("Password", ScalarTypes.String),
                new ColumnSymbol("Query Parameters", ScalarTypes.Dynamic),
                new ColumnSymbol("Fragment", ScalarTypes.String));

        public static readonly FunctionSymbol ParseUrl_Deprecated =
            new FunctionSymbol("parseurl",
                    ParseUrlResult,
                    new Parameter("url", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable()
                .Obsolete("parse_url");

        public static readonly FunctionSymbol ParseUrl =
            new FunctionSymbol("parse_url",
                    ParseUrlResult,
                    new Parameter("url", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        private static readonly TypeSymbol ParseUrlQueryResult =
            ScalarTypes.GetDynamicBag(
                new ColumnSymbol("Query Parameters", ScalarTypes.Dynamic));

        public static readonly FunctionSymbol ParseUrlQuery_Deprecated =
            new FunctionSymbol("parseurlquery",
                    ParseUrlQueryResult,
                    new Parameter("query", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable()
                .Obsolete("parse_urlquery");

        public static readonly FunctionSymbol ParseUrlQuery =
            new FunctionSymbol("parse_urlquery",
                    ParseUrlQueryResult,
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

        public static readonly FunctionSymbol FormatIPV4 =
            new FunctionSymbol("format_ipv4", ScalarTypes.String,
                    new Parameter("ip", ParameterTypeKind.Scalar), // TODO: restrict to String, Int or Long
                    new Parameter("prefix", ScalarTypes.Long, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol FormatIPV4Mask =
            new FunctionSymbol("format_ipv4_mask", ScalarTypes.String,
                    new Parameter("ip", ParameterTypeKind.Scalar), // TODO: restrict to String, Int or Long
                    new Parameter("prefix", ScalarTypes.Long))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol Ipv4Compare =
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

        public static readonly FunctionSymbol Ipv4IsInRange =
            new FunctionSymbol("ipv4_is_in_range", ScalarTypes.Bool,
                    new Parameter("ip", ScalarTypes.String),
                    new Parameter("ip_range", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol Ipv4IsInAnyRange =
            new FunctionSymbol("ipv4_is_in_any_range",
                    new Signature(ScalarTypes.Bool,
                        new Parameter("ip", ParameterTypeKind.StringOrDynamic),
                        new Parameter("ranges", ScalarTypes.String, maxOccurring: MaxRepeat)),
                    new Signature(ScalarTypes.Bool,
                        new Parameter("ip", ParameterTypeKind.StringOrDynamic),
                        new Parameter("ranges", ParameterTypeKind.DynamicArray)))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol Ipv4NetmaskSuffix =
            new FunctionSymbol("ipv4_netmask_suffix", ScalarTypes.Long,
                    new Parameter("ip", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol Ipv4IsPrivate =
            new FunctionSymbol("ipv4_is_private", ScalarTypes.Bool,
                    new Parameter("ip", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol Ipv4RangeToCidrList =
            new FunctionSymbol("ipv4_range_to_cidr_list",
                    ScalarTypes.DynamicArray,
                    new Parameter("start_ip", ScalarTypes.String),
                    new Parameter("end_ip", ScalarTypes.String))
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

        public static readonly FunctionSymbol Ipv6IsInRange =
            new FunctionSymbol("ipv6_is_in_range", ScalarTypes.Bool,
                    new Parameter("ip", ScalarTypes.String),
                    new Parameter("ip_range", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol Ipv6IsInAnyRange =
            new FunctionSymbol("ipv6_is_in_any_range",
                    new Signature(ScalarTypes.Bool,
                        new Parameter("ip", ParameterTypeKind.StringOrDynamic),
                        new Parameter("ranges", ScalarTypes.String, maxOccurring: MaxRepeat)),
                    new Signature(ScalarTypes.Bool,
                        new Parameter("ip", ParameterTypeKind.StringOrDynamic),
                        new Parameter("ranges", ParameterTypeKind.DynamicArray)))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol Ipv6LookupRanges =
            new FunctionSymbol("__ipv6_lookup_ranges",
                    new Signature(ScalarTypes.DynamicArray,
                        new Parameter("ip", ParameterTypeKind.StringOrDynamic),
                        new Parameter("ranges", ParameterTypeKind.DynamicArray)))
                .WithResultNameKind(ResultNameKind.None).Hide();
        #endregion

        private static readonly TypeSymbol ParsePathResult =
            ScalarTypes.GetDynamicBag(
                new ColumnSymbol("Scheme", ScalarTypes.String),
                new ColumnSymbol("RootPath", ScalarTypes.String),
                new ColumnSymbol("DirectoryPath", ScalarTypes.String),
                new ColumnSymbol("DirectoryName", ScalarTypes.String),
                new ColumnSymbol("Filename", ScalarTypes.String),
                new ColumnSymbol("Extension", ScalarTypes.String),
                new ColumnSymbol("AlternateDataStreamName", ScalarTypes.String));

        public static readonly FunctionSymbol ParsePath =
            new FunctionSymbol("parse_path",
                    ParsePathResult,
                    new Parameter("path", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        private static readonly TypeSymbol ParseUserAgentResult =
            ScalarTypes.GetDynamicBag(
                new ColumnSymbol("Browser",
                    ScalarTypes.GetDynamicBag(
                        new ColumnSymbol("Family", ScalarTypes.String),
                        new ColumnSymbol("MajorVersion", ScalarTypes.String),
                        new ColumnSymbol("MinorVersion", ScalarTypes.String),
                        new ColumnSymbol("Patch", ScalarTypes.String))),
                new ColumnSymbol("OperatingSystem",
                    ScalarTypes.GetDynamicBag(
                        new ColumnSymbol("Family", ScalarTypes.String),
                        new ColumnSymbol("MajorVersion", ScalarTypes.String),
                        new ColumnSymbol("MinorVersion", ScalarTypes.String),
                        new ColumnSymbol("Patch", ScalarTypes.String),
                        new ColumnSymbol("PatchMinor", ScalarTypes.String))),
                new ColumnSymbol("Device",
                    ScalarTypes.GetDynamicBag(
                        new ColumnSymbol("Family", ScalarTypes.String),
                        new ColumnSymbol("Brand", ScalarTypes.String),
                        new ColumnSymbol("Model", ScalarTypes.String))));

        public static readonly FunctionSymbol ParseUserAgent =
            new FunctionSymbol("parse_user_agent",
                    ParseUserAgentResult,
                    new Parameter("user_agent", ScalarTypes.String),
                    new Parameter("look_for", ParameterTypeKind.StringOrArray, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol ParseVersion =
            new FunctionSymbol("parse_version",
                    ScalarTypes.Decimal,
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
                        new Parameter("year", ParameterTypeKind.Number),
                        new Parameter("month", ParameterTypeKind.Number),
                        new Parameter("day", ParameterTypeKind.Number),
                        new Parameter("hour", ParameterTypeKind.Number, minOccurring: 0),
                        new Parameter("minute", ParameterTypeKind.Number, minOccurring: 0),
                        new Parameter("second", ParameterTypeKind.Number, minOccurring: 0)),
                    new Signature(ScalarTypes.DateTime,
                        new Parameter("value", ParameterTypeKind.Scalar)))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.OnlyArgument);

        public static readonly FunctionSymbol MakeTimespan =
            new FunctionSymbol("make_timespan",
                    new Signature(ScalarTypes.TimeSpan,
                        new Parameter("days", ParameterTypeKind.Integer),
                        new Parameter("hours", ParameterTypeKind.Integer),
                        new Parameter("minutes", ParameterTypeKind.Integer),
                        new Parameter("seconds", ParameterTypeKind.Integer)),
                    new Signature(ScalarTypes.TimeSpan,
                        new Parameter("hours", ParameterTypeKind.Integer),
                        new Parameter("minutes", ParameterTypeKind.Integer),
                        new Parameter("seconds", ParameterTypeKind.Integer, minOccurring: 0)),
                    new Signature(ScalarTypes.TimeSpan,
                        new Parameter("value", ParameterTypeKind.Scalar)))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.OnlyArgument);

        public static readonly FunctionSymbol DatetimeAdd =
            new FunctionSymbol("datetime_add", ScalarTypes.DateTime,
                    new Parameter("part", ScalarTypes.String, ArgumentKind.Literal, KustoFacts.DateDiffParts),
                    new Parameter("value", ParameterTypeKind.Integer),
                    new Parameter("datetime", ScalarTypes.DateTime))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol DatetimeDiff =
            new FunctionSymbol("datetime_diff", ScalarTypes.Long,
                    new Parameter("part", ScalarTypes.String, ArgumentKind.Literal, KustoFacts.DateDiffParts),
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
                .Obsolete("week_of_year");

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
                    new Parameter("part", ScalarTypes.String, ArgumentKind.Literal, KustoFacts.DateTimeParts),
                    new Parameter("date", ScalarTypes.DateTime))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.None)
                .Obsolete("datetime_part");

        public static readonly FunctionSymbol DatetimePart =
            new FunctionSymbol("datetime_part", ScalarTypes.Int,
                    new Parameter("part", ScalarTypes.String, ArgumentKind.Literal, KustoFacts.DateTimeParts),
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

        public static readonly FunctionSymbol DatetimeLocalToUtc =
            new FunctionSymbol("datetime_local_to_utc", ScalarTypes.DateTime,
                    new Parameter("from", ScalarTypes.DateTime),
                    new Parameter("timezone", ScalarTypes.String))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol DatetimeUtcToLocal =
            new FunctionSymbol("datetime_utc_to_local", ScalarTypes.DateTime,
                    new Parameter("from", ScalarTypes.DateTime),
                    new Parameter("timezone", ScalarTypes.String))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol DateTimeListTimezones =
            new FunctionSymbol("datetime_list_timezones",
                    ScalarTypes.DynamicArray)
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol PunycodeDecode =
            new FunctionSymbol("punycode_to_string", ScalarTypes.String,
                    new Parameter("string", ScalarTypes.String))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol PunycodeEncode =
            new FunctionSymbol("punycode_from_string", ScalarTypes.String,
                    new Parameter("string", ScalarTypes.String))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol PunycodeDomainDecode =
            new FunctionSymbol("punycode_domain_from_string", ScalarTypes.String,
                    new Parameter("string", ScalarTypes.String))
                .ConstantFoldable()
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol PunycodeDomainEncode =
            new FunctionSymbol("punycode_domain_to_string", ScalarTypes.String,
                    new Parameter("string", ScalarTypes.String))
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

        public static readonly FunctionSymbol InternalHashXXH64 =
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

        public static readonly FunctionSymbol HashXXH64 =
            new FunctionSymbol("hash_xxhash64", ScalarTypes.Long,
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
                .ConstantFoldable();

        public static readonly FunctionSymbol HashSha1 =
            new FunctionSymbol("hash_sha1", ScalarTypes.String,
                    new Parameter("source", ParameterTypeKind.NotDynamic))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

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
            new FunctionSymbol("iif", ReturnTypeKind.CommonNonDynamic,
                    new Parameter("if", ScalarTypes.Bool),
                    new Parameter("then", ParameterTypeKind.CommonScalarOrDynamic),
                    new Parameter("else", ParameterTypeKind.CommonScalarOrDynamic))
                .ConstantFoldable();

        public static readonly FunctionSymbol Iff =
            new FunctionSymbol("iff", ReturnTypeKind.CommonNonDynamic,
                    new Parameter("if", ScalarTypes.Bool),
                    new Parameter("then", ParameterTypeKind.CommonScalarOrDynamic),
                    new Parameter("else", ParameterTypeKind.CommonScalarOrDynamic))
                .ConstantFoldable();

        public static readonly FunctionSymbol Case =
            new FunctionSymbol("case",
                    new Signature(ReturnTypeKind.CommonNonDynamic,
                            new Parameter("predicate", ScalarTypes.Bool, maxOccurring: MaxRepeat),
                            new Parameter("then", ParameterTypeKind.CommonScalarOrDynamic, maxOccurring: MaxRepeat),
                            new Parameter("else", ParameterTypeKind.CommonScalarOrDynamic))
                        .WithLayout(ParameterLayouts.BlockRepeating))
                .ConstantFoldable();

        public static readonly FunctionSymbol Assert =
            new FunctionSymbol("assert", ScalarTypes.Bool,
                new Parameter("predicate", ScalarTypes.Bool),
                new Parameter("message", ScalarTypes.String));
        #endregion

        #region bin / floor
        public static readonly FunctionSymbol Bin =
            new FunctionSymbol("bin",
                    new Signature(ReturnTypeKind.Widest,
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
                    new Signature(ReturnTypeKind.Widest,
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
                    new Signature(ReturnTypeKind.Widest,
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
            new FunctionSymbol("bin_auto", //"bin_at(value, query_bin_auto_size, query_bin_auto_at)", 
                    context =>
                    {
                        if (context.GetArgument("value")?.ResultType is ScalarSymbol valueType)
                        {
                            if (valueType.IsNumeric)
                            {
                                return
                                    TypeFacts.GetCommonScalarType(
                                            valueType,
                                            context.GetResultType("query_bin_auto_size"),
                                            context.GetResultType("query_bin_auto_at"))
                                        .PromoteToLong();
                            }
                            else
                            {
                                return valueType.PromoteToLong();
                            }
                        }
                        else
                        {
                            return ScalarTypes.Unknown;
                        }
                    },
                    Tabularity.Scalar,
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

        public static readonly FunctionSymbol NotNull_Deprecated =
            new FunctionSymbol("notnull", ScalarTypes.Bool,
                    new Parameter("expression", ParameterTypeKind.Scalar))
                .WithResultNameKind(ResultNameKind.None)
                .Obsolete("isnotnull");

        public static readonly FunctionSymbol IsNotNull =
            new FunctionSymbol("isnotnull", ScalarTypes.Bool,
                    new Parameter("expression", ParameterTypeKind.Scalar))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol IsNull =
            new FunctionSymbol("isnull", ScalarTypes.Bool,
                    new Parameter("expression", ParameterTypeKind.Scalar))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol NotEmpty_Deprecated =
            new FunctionSymbol("notempty", ScalarTypes.Bool,
                    new Parameter("value", ParameterTypeKind.Scalar))
                .WithResultNameKind(ResultNameKind.None)
                .Obsolete("isnotempty");

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

        public static readonly FunctionSymbol ColumnIfExists_Deprecated =
            new FunctionSymbol("columnifexists", ReturnTypeKind.Parameter1,
                    new Parameter("column_name", ScalarTypes.String, ArgumentKind.Constant),
                    new Parameter("defaultValue", ParameterTypeKind.Scalar))
                .WithResultNameKind(ResultNameKind.FirstArgumentValueIfColumn)
                .Obsolete("column_ifexists");

        public static readonly FunctionSymbol ColumnIfExists =
            new FunctionSymbol("column_ifexists", ReturnTypeKind.Parameter1,
                    new Parameter("column_name", ScalarTypes.String, ArgumentKind.Constant),
                    new Parameter("defaultValue", ParameterTypeKind.Scalar))
                .WithResultNameKind(ResultNameKind.FirstArgumentValueIfColumn);

        public static readonly FunctionSymbol Around = new FunctionSymbol("around", ScalarTypes.Bool,
                new Parameter("value", ParameterTypeKind.Scalar),
                new Parameter("center", ParameterTypeKind.Scalar),
                new Parameter("delta", ParameterTypeKind.Scalar))
            // TODO: Check how is it possible to define different type combinations:
            // (datetime, datetime, timespan)
            // (numeric, numeric, numeric)
            .WithResultNameKind(ResultNameKind.None);
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
            new FunctionSymbol("treepath",
                    ScalarTypes.DynamicArrayOfString,
                    new Parameter("object", ParameterTypeKind.DynamicBag))
                .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
                .WithResultNamePrefix("tree")
                .ConstantFoldable();

        public static readonly FunctionSymbol Repeat =
            new FunctionSymbol("repeat",
                    ReturnTypeKind.Parameter0Array,
                    new Parameter("value", ParameterTypeKind.Scalar),
                    new Parameter("count", ScalarTypes.Long))
                .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
                .WithResultNamePrefix("repeat")
                .ConstantFoldable();

        public static readonly FunctionSymbol Arraylength_Deprecated =
            new FunctionSymbol("arraylength", ScalarTypes.Long,
                    new Parameter("array", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable()
                .Obsolete("array_length");

        public static readonly FunctionSymbol ArrayLength =
            new FunctionSymbol("array_length", ScalarTypes.Long,
                    new Parameter("array", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol ArrayReverse =
            new FunctionSymbol("array_reverse",
                    ReturnTypeKind.Parameter0,
                    new Parameter("value", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol Range =
            new FunctionSymbol("range",
                    ReturnTypeKind.Parameter0Array,
                    new Parameter("start", ParameterTypeKind.Summable),
                    new Parameter("stop", ParameterTypeKind.Summable),
                    new Parameter("step", ParameterTypeKind.Summable, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol ArrayConcat =
            new FunctionSymbol("array_concat",
                    context =>
                    {
                        var arrays = context.GetArguments("array");
                        return TypeFacts.GetCommonResultType(arrays, Conversion.None) ?? ScalarTypes.DynamicArray;
                    },
                    Tabularity.Scalar,
                    new Parameter("array", ParameterTypeKind.DynamicArray, maxOccurring: 64))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol ArrayIff =
            new FunctionSymbol("array_iff",
                    context =>
                    {
                        var whenTrue = context.GetArgument("when_true");
                        var whenFalse = context.GetArgument("when_true");
                        var commonResult = TypeFacts.GetCommonResultType(whenTrue, whenFalse);
                        if (!(commonResult is DynamicSymbol))
                            commonResult = ScalarTypes.GetDynamicArray(commonResult);
                        return commonResult;
                    },
                    Tabularity.Scalar,
                    new Parameter("condition_array", ParameterTypeKind.DynamicArray),
                    new Parameter("when_true", ParameterTypeKind.Scalar),
                    new Parameter("when_false", ParameterTypeKind.Scalar))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol ArrayIif =
            new FunctionSymbol("array_iif",
                    context =>
                    {
                        var whenTrue = context.GetArgument("when_true");
                        var whenFalse = context.GetArgument("when_true");
                        var commonResult = TypeFacts.GetCommonResultType(whenTrue, whenFalse);
                        if (!(commonResult is DynamicSymbol))
                            commonResult = ScalarTypes.GetDynamicArray(commonResult);
                        return commonResult;
                    },
                    Tabularity.Scalar,
                    new Parameter("condition_array", ParameterTypeKind.DynamicArray),
                    new Parameter("when_true", ParameterTypeKind.Scalar),
                    new Parameter("when_false", ParameterTypeKind.Scalar))
                .ConstantFoldable()
                .Hide();

        public static readonly FunctionSymbol ArrayIndexOf =
            new FunctionSymbol("array_index_of",
                    ScalarTypes.Long,
                    new Parameter("array", ParameterTypeKind.DynamicArray),
                    new Parameter("value", ParameterTypeKind.Scalar),
                    new Parameter("start", ParameterTypeKind.Integer, minOccurring: 0),
                    new Parameter("length", ParameterTypeKind.Integer, minOccurring: 0),
                    new Parameter("occurrence", ParameterTypeKind.Integer, ArgumentKind.Constant, minOccurring: 0))
                .ConstantFoldable();

        public static readonly FunctionSymbol SetHasElement =
            new FunctionSymbol("set_has_element",
                    ScalarTypes.Bool,
                    new Parameter("set", ParameterTypeKind.DynamicArray),
                    new Parameter("value", ParameterTypeKind.Scalar))
                .ConstantFoldable();

        public static readonly FunctionSymbol ArraySlice =
            new FunctionSymbol("array_slice",
                    ReturnTypeKind.Parameter0,
                    new Parameter("array", ParameterTypeKind.DynamicArray),
                    new Parameter("start", ParameterTypeKind.Integer),
                    new Parameter("end", ParameterTypeKind.Integer))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol ArraySplit =
            new FunctionSymbol("array_split",
                    new Signature(
                        ReturnTypeKind.Parameter0,
                        new Parameter("array", ParameterTypeKind.DynamicArray),
                        new Parameter("index", ParameterTypeKind.Integer)),
                    new Signature(
                        ReturnTypeKind.Parameter0,
                        new Parameter("array", ParameterTypeKind.DynamicArray),
                        new Parameter("indices", ParameterTypeKind.DynamicArray)))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol ArrayShiftLeft =
            new FunctionSymbol("array_shift_left",
                    ReturnTypeKind.Parameter0,
                    new Parameter("array", ParameterTypeKind.DynamicArray),
                    new Parameter("shift_count", ParameterTypeKind.Integer),
                    new Parameter("default_value", ParameterTypeKind.Scalar, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol ArrayShiftRight =
            new FunctionSymbol("array_shift_right",
                    ReturnTypeKind.Parameter0,
                    new Parameter("array", ParameterTypeKind.DynamicArray),
                    new Parameter("shift_count", ParameterTypeKind.Integer),
                    new Parameter("default_value", ParameterTypeKind.Scalar, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol ArrayRotateLeft =
            new FunctionSymbol("array_rotate_left",
                    ReturnTypeKind.Parameter0,
                    new Parameter("array", ParameterTypeKind.DynamicArray),
                    new Parameter("rotate_count", ParameterTypeKind.Integer))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol ArrayRotateRight =
            new FunctionSymbol("array_rotate_right",
                    ReturnTypeKind.Parameter0,
                    new Parameter("array", ParameterTypeKind.DynamicArray),
                    new Parameter("rotate_count", ParameterTypeKind.Integer))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        private static readonly Parameter m_ArraySort_ArraysArg =
            new Parameter("arrays", ParameterTypeKind.DynamicArray, minOccurring: 1, maxOccurring: 64);

        private static readonly Parameter m_ArraySort_NullsLastArg =
            new Parameter("nulls_last", ScalarTypes.Bool, ArgumentKind.Constant, minOccurring: 0, maxOccurring: 1);

        public static readonly FunctionSymbol ArraySortAsc =
            new FunctionSymbol("array_sort_asc",
                new Signature(
                        GetArraySortResult,
                        Tabularity.Scalar,
                        m_ArraySort_ArraysArg,
                        m_ArraySort_NullsLastArg)
                    .WithLayout(ValidateArgumentsForArraySort));

        public static readonly FunctionSymbol ArraySortDesc =
            new FunctionSymbol("array_sort_desc",
                new Signature(
                        GetArraySortResult,
                        Tabularity.Scalar,
                        m_ArraySort_ArraysArg,
                        m_ArraySort_NullsLastArg)
                    .WithLayout(ValidateArgumentsForArraySort));

        private static void ValidateArgumentsForArraySort(Signature signature, IReadOnlyList<Expression> arguments, List<Parameter> argumentParameters)
        {
            for (var i = 0; i < arguments.Count; i++)
            {
                if ((i < arguments.Count - 1) || !IsBoolean(arguments[i]))
                {
                    argumentParameters.Add(m_ArraySort_ArraysArg);
                }
                else
                {
                    argumentParameters.Add(m_ArraySort_NullsLastArg);
                }
            }
        }

        private static TypeSymbol GetArraySortResult(CustomReturnTypeContext context)
        {
            var result = new List<ColumnSymbol>();

            for (int i = 0;
                 (i < context.Arguments.Count) && (context.Arguments[i].ResultType.IsDynamicArray()); i++)
            {
                var argument = context.Arguments[i];
                var argumentExpressionName = context.GetResultName(argument);
                var resultColumnName = string.IsNullOrEmpty(argumentExpressionName) ? $"array{i}_sorted" : $"{argumentExpressionName}_sorted";
                var resultColumn = new ColumnSymbol(resultColumnName, argument.ResultType, source: argument);
                result.Add(resultColumn);
            }

            return new TupleSymbol(result);
        }

        public static readonly FunctionSymbol BagKeys =
            new FunctionSymbol("bag_keys",
                    ScalarTypes.DynamicArrayOfString,
                    new Parameter("object", ParameterTypeKind.DynamicBag))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol Zip =
            new FunctionSymbol("zip",
                    ScalarTypes.DynamicArrayOfArray,
                    new Parameter("array", ParameterTypeKind.DynamicArray, minOccurring: 2, maxOccurring: 64))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol Pack =
            new FunctionSymbol("pack",
                    new Signature(
                            GetBagPackReturnType,
                            Tabularity.Scalar,
                            new Parameter("key", ScalarTypes.String, maxOccurring: MaxRepeat),
                            new Parameter("value", ParameterTypeKind.Scalar, maxOccurring: MaxRepeat))
                        .WithLayout(ParameterLayouts.BlockRepeating))
                .WithResultNameKind(ResultNameKind.None)
                .Obsolete("bag_pack")
                .ConstantFoldable();

        public static readonly FunctionSymbol PackDictionary =
            new FunctionSymbol("pack_dictionary",
                    new Signature(
                            GetBagPackReturnType,
                            Tabularity.Scalar,
                            new Parameter("key", ScalarTypes.String, maxOccurring: MaxRepeat),
                            new Parameter("value", ParameterTypeKind.Scalar, maxOccurring: MaxRepeat))
                        .WithLayout(ParameterLayouts.BlockRepeating))
                .WithResultNameKind(ResultNameKind.None)
                .Obsolete("bag_pack")
                .ConstantFoldable();

        public static readonly FunctionSymbol BagPack =
            new FunctionSymbol("bag_pack",
                    new Signature(
                            GetBagPackReturnType,
                            Tabularity.Scalar,
                            new Parameter("key", ScalarTypes.String, maxOccurring: MaxRepeat),
                            new Parameter("value", ParameterTypeKind.Scalar, maxOccurring: MaxRepeat))
                        .WithLayout(ParameterLayouts.BlockRepeating))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        private static TypeSymbol GetBagPackReturnType(CustomReturnTypeContext context)
        {
            var keys = context.GetArguments("key");
            var values = context.GetArguments("value");
            var columns = new List<ColumnSymbol>(keys.Count);

            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i].ConstantValue is string name)
                {
                    var type = (i < values.Count ? values[i].ResultType : null) ?? ScalarTypes.Unknown;
                    columns.Add(new ColumnSymbol(name, type));
                }
            }

            return ScalarTypes.GetDynamicBag(columns);
        }

        public static readonly FunctionSymbol BagPackColumns =
            new FunctionSymbol("bag_pack_columns",
                    GetBagPackColumnsReturnType,
                    Tabularity.Scalar,
                    new Parameter("value", ParameterTypeKind.Scalar, maxOccurring: MaxRepeat, argumentKind: ArgumentKind.Column))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        private static TypeSymbol GetBagPackColumnsReturnType(CustomReturnTypeContext context)
        {
            var values = context.GetArguments("value");
            var columns = new List<ColumnSymbol>(values.Count);

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].ReferencedSymbol is ColumnSymbol col)
                {
                    columns.Add(col);
                }
            }

            return ScalarTypes.GetDynamicBag(columns);
        }

        public static readonly FunctionSymbol PackAll =
            new FunctionSymbol("pack_all",
                    context => ScalarTypes.GetDynamicBag(context.RowScope?.Columns),
                    Tabularity.Scalar,
                new Parameter("ignore_null_empty", ParameterTypeKind.Scalar, ArgumentKind.Literal, minOccurring: 0, maxOccurring: 1))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol PackArray =
            new FunctionSymbol("pack_array",
                    new Signature(
                        context =>
                        {
                            var values = context.GetArguments("value");
                            var type = TypeFacts.GetCommonResultType(values);
                            return ScalarTypes.GetDynamicArray(type);
                        },
                        Tabularity.Scalar,
                        new Parameter("value", ParameterTypeKind.Scalar, maxOccurring: MaxRepeat)),
                    new Signature(
                        context =>
                        {
                            return context.RowScope != null
                                ? ScalarTypes.GetDynamicArray(TypeFacts.GetCommonColumnType(context.RowScope.Columns))
                                : ScalarTypes.DynamicArray;
                        },
                        Tabularity.Scalar,
                        new Parameter("value", ParameterTypeKind.Scalar, ArgumentKind.StarOnly)))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        private static TypeSymbol GetSetType(CustomReturnTypeContext context)
        {
            var sets = context.GetArguments("set");
            var commonType = TypeFacts.GetCommonResultType(sets);
            return commonType is DynamicArraySymbol ? commonType : ScalarTypes.DynamicArray;
        }

        public static readonly FunctionSymbol SetUnion =
            new FunctionSymbol("set_union",
                    GetSetType,
                    Tabularity.Scalar,
                    new Parameter("set", ParameterTypeKind.DynamicArray, minOccurring: 2, maxOccurring: 64))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol SetIntersect =
            new FunctionSymbol("set_intersect",
                    GetSetType,
                    Tabularity.Scalar,
                    new Parameter("set", ParameterTypeKind.DynamicArray, minOccurring: 2, maxOccurring: 64))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol SetDifference =
            new FunctionSymbol("set_difference",
                    GetSetType,
                    Tabularity.Scalar,
                    new Parameter("set", ParameterTypeKind.DynamicArray, minOccurring: 2, maxOccurring: 64))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol SetEquals =
            new FunctionSymbol("set_equals",
                    ScalarTypes.Bool,
                    new Parameter("set1", ParameterTypeKind.DynamicArray),
                    new Parameter("set2", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol BagMerge =
            new FunctionSymbol("bag_merge",
                    context =>
                    {
                        var bags = context.GetArguments("bag");
                        if (bags.Count > 0
                            && bags[0].ResultType is DynamicBagSymbol merged)
                        {
                            for (int i = 1; i < bags.Count; i++)
                            {
                                if (bags[i].ResultType is DynamicBagSymbol obj)
                                {
                                    merged = ScalarTypes.GetDynamicBag(TypeFacts.Union(merged.Properties, obj.Properties));
                                }
                            }

                            return merged;
                        }

                        return ScalarTypes.DynamicBag;
                    },
                    Tabularity.Scalar,
                    new Parameter("bag", ParameterTypeKind.DynamicBag, minOccurring: 2, maxOccurring: 64))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol DynamicToJson =
            new FunctionSymbol("dynamic_to_json",
                    ScalarTypes.String,
                    new Parameter("dynamic", ParameterTypeKind.Scalar))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol BagRemoveKeys =
            new FunctionSymbol("bag_remove_keys",
                    context =>
                    {
                        var bag = context.GetArgument("bag");
                        var keys = context.GetArgument("keys");

                        if (bag != null &&
                            bag.ResultType is DynamicBagSymbol bs
                            && keys is JsonArrayExpression ja)
                        {
                            var namesToRemove = GetConstantValues(ja.Values).ToHashSetEx();
                            var newProps = bs.Properties.Where(p => !namesToRemove.Contains(p.Name)).ToList();
                            return ScalarTypes.GetDynamicBag(newProps);
                        }

                        return ScalarTypes.DynamicBag;
                    },
                    Tabularity.Scalar,
                    new Parameter("bag", ParameterTypeKind.DynamicBag),
                    new Parameter("keys", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol BagZip =
            new FunctionSymbol("bag_zip",
                    ScalarTypes.DynamicBag,
                    new Parameter("keys", ParameterTypeKind.DynamicArray),
                    new Parameter("values", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol JaccardIndex =
            new FunctionSymbol("jaccard_index",
                    ScalarTypes.DynamicArrayOfReal,
                    new Parameter("set", ParameterTypeKind.DynamicArray, minOccurring: 2, maxOccurring: 2))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol BagHasKey =
            new FunctionSymbol("bag_has_key",
                    ScalarTypes.Bool,
                    new Parameter("bag", ParameterTypeKind.DynamicBag),
                    new Parameter("key", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol BagSetKey =
            new FunctionSymbol("bag_set_key",
                    context =>
                    {
                        var bag = context.GetArgument("bag");
                        var key = context.GetArgument("key");
                        var value = context.GetArgument("value");

                        if (bag != null
                            && key != null
                            && key.ConstantValue is string name
                            && !string.IsNullOrWhiteSpace(name)
                            && !IsJsonPath(name) // maybe someday
                            && value != null
                            && value.ResultType is ScalarSymbol type)
                        {
                            var currentBag = bag.ResultType as DynamicBagSymbol ?? ScalarTypes.DynamicBag;
                            var newProperty = new ColumnSymbol(name, type);
                            return currentBag.AddOrUpdateProperty(newProperty);
                        }

                        return bag?.ResultType ?? ScalarTypes.DynamicBag;
                    },
                    Tabularity.Scalar,
                    new Parameter("bag", ParameterTypeKind.DynamicBag),
                    new Parameter("key", ScalarTypes.String, ArgumentKind.Constant),
                    new Parameter("value", ParameterTypeKind.Scalar))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();


        private static bool IsJsonPath(string key)
        {
            return key.Contains('$')
                   || key.Contains('.')
                   || key.Contains('[')
                   || key.Contains(']');
        }
        #endregion

        #region digest / series functions
        public static readonly FunctionSymbol PercentileTDigest =
            new FunctionSymbol("percentile_tdigest",
                    new Signature(
                        ScalarTypes.Dynamic,
                        new Parameter("tdigest", ParameterTypeKind.DynamicArray),
                        new Parameter("percentile1", ScalarTypes.Real)),
                    new Signature(
                        ReturnTypeKind.ParameterNLiteral,
                        new Parameter("tdigest", ParameterTypeKind.DynamicArray),
                        new Parameter("percentile1", ScalarTypes.Real),
                        new Parameter("type", ScalarTypes.Type, ArgumentKind.Literal)))
                .WithResultNameKind(ResultNameKind.NameAndFirstArgument);

        public static readonly FunctionSymbol PercentileArrayTDigest =
            new FunctionSymbol("percentiles_array_tdigest",
                    new Signature(
                        ScalarTypes.DynamicArray,
                        new Parameter("tdigest", ParameterTypeKind.DynamicArray),
                        new Parameter("percentile", ScalarTypes.Real, maxOccurring: MaxRepeat)),
                    new Signature(
                        ScalarTypes.DynamicArray,
                        new Parameter("tdigest", ParameterTypeKind.DynamicArray),
                        new Parameter("percentiles", ParameterTypeKind.DynamicArray)))
                .WithResultNamePrefix("percentile_tdigest")
                .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument);

        public static readonly FunctionSymbol PercentRankTDigest =
            new FunctionSymbol("percentrank_tdigest",
                    ScalarTypes.Real,
                    new Parameter("digest", ParameterTypeKind.DynamicArray),
                    new Parameter("value", ParameterTypeKind.Scalar))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol RankTDigest =
            new FunctionSymbol("rank_tdigest",
                    ScalarTypes.Real,
                    new Parameter("digest", ParameterTypeKind.DynamicArray),
                    new Parameter("value", ParameterTypeKind.Scalar))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol TdigestIsValid =
            new FunctionSymbol("tdigest_isvalid",
                    ScalarTypes.Bool,
                    new Parameter("digest", ParameterTypeKind.DynamicArray),
                    new Parameter("value", ParameterTypeKind.Scalar))
                .WithResultNameKind(ResultNameKind.None)
                .Hide();

        public static readonly FunctionSymbol HllIsValid =
            new FunctionSymbol("hll_isvalid",
                    ScalarTypes.Bool,
                    new Parameter("hll", ParameterTypeKind.DynamicArray),
                    new Parameter("value", ParameterTypeKind.Scalar))
                .WithResultNameKind(ResultNameKind.None)
                .Hide();

        public static readonly FunctionSymbol TDigestMerge =
            new FunctionSymbol("tdigest_merge",
                    ScalarTypes.DynamicArray,
                    new Parameter("tdigest", ParameterTypeKind.DynamicArray, minOccurring: 2, maxOccurring: 16))
                .WithResultNameKind(ResultNameKind.PrefixOnly)
                .WithResultNamePrefix("tdigests_merge_result");

        public static readonly FunctionSymbol MergeTDigest =
            new FunctionSymbol("merge_tdigest",
                    ScalarTypes.DynamicArray,
                    new Parameter("tdigest", ParameterTypeKind.DynamicArray, minOccurring: 2, maxOccurring: 16))
                .WithResultNameKind(ResultNameKind.PrefixOnly)
                .WithResultNamePrefix("tdigests_merge_result");

        public static readonly FunctionSymbol HllMerge =
            new FunctionSymbol("hll_merge",
                    ScalarTypes.DynamicArray,
                    new Parameter("hll", ParameterTypeKind.DynamicArray, minOccurring: 2, maxOccurring: 16))
                .WithResultNameKind(ResultNameKind.PrefixOnly)
                .WithResultNamePrefix("hll_merged_result");

        public static readonly FunctionSymbol DCountHll =
            new FunctionSymbol("dcount_hll",
                    ScalarTypes.Long,
                    new Parameter("hll", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.NameAndFirstArgument);

        public static readonly FunctionSymbol HllNormalize =
            new FunctionSymbol("__hll_normalize", ScalarTypes.Dynamic,
                    new Parameter("hll", ScalarTypes.Dynamic, minOccurring: 2, maxOccurring: 16))
                .WithResultNameKind(ResultNameKind.FirstArgument)
                .Hide();

        public static readonly FunctionSymbol SeriesFir =
            new FunctionSymbol("series_fir",
                    ScalarTypes.DynamicArrayOfReal,
                    new Parameter("series", ParameterTypeKind.DynamicArray),
                    new Parameter("filter", ParameterTypeKind.DynamicArray),
                    new Parameter("normalize", ScalarTypes.Bool, minOccurring: 0),
                    new Parameter("center", ScalarTypes.Bool, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol SeriesStats =
            new FunctionSymbol("series_stats",
                context =>
                {
                    var source = context.GetArgument("series");
                    return MakePrefixedTuple(context, "series",
                        new TupleSymbol(
                            new ColumnSymbol("min", ScalarTypes.Real, source: source),
                            new ColumnSymbol("min_idx", ScalarTypes.Long, source: source),
                            new ColumnSymbol("max", ScalarTypes.Real, source: source),
                            new ColumnSymbol("max_idx", ScalarTypes.Long, source: source),
                            new ColumnSymbol("avg", ScalarTypes.Real, source: source),
                            new ColumnSymbol("stdev", ScalarTypes.Real, source: source),
                            new ColumnSymbol("variance", ScalarTypes.Real, source: source)));
                },
                Tabularity.Scalar,
                new Parameter("series", ParameterTypeKind.DynamicArray),
                new Parameter("ignore_nonfinite", ScalarTypes.Bool, ArgumentKind.Constant, minOccurring: 0));

        public static readonly FunctionSymbol SeriesStatsDynamic =
            new FunctionSymbol("series_stats_dynamic",
                    context =>
                    {
                        var source = context.GetArgument("series");
                        return new DynamicBagSymbol(
                            new ColumnSymbol("min", ScalarTypes.Real, source: source),
                            new ColumnSymbol("min_idx", ScalarTypes.Long, source: source),
                            new ColumnSymbol("max", ScalarTypes.Real, source: source),
                            new ColumnSymbol("max_idx", ScalarTypes.Long, source: source),
                            new ColumnSymbol("avg", ScalarTypes.Real, source: source),
                            new ColumnSymbol("stdev", ScalarTypes.Real, source: source),
                            new ColumnSymbol("variance", ScalarTypes.Real, source: source));
                    },
                    Tabularity.Scalar,
                    new Parameter("series", ParameterTypeKind.DynamicArray),
                    new Parameter("ignore_nonfinite", ScalarTypes.Bool, ArgumentKind.Constant, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol ArraySum =
            new FunctionSymbol("array_sum",
                    ScalarTypes.Real,
                    new Parameter("array", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None)
                .Obsolete("series_sum");

        public static readonly FunctionSymbol SeriesFft =
            new FunctionSymbol("series_fft",
                context =>
                {
                    var source = context.GetArgument("series");
                    return MakePrefixedTuple(context, "series",
                        new TupleSymbol(
                            new ColumnSymbol("real", ScalarTypes.DynamicArrayOfReal, source: source),
                            new ColumnSymbol("imag", ScalarTypes.DynamicArrayOfReal, source: source)));
                },
                Tabularity.Scalar,
                new Parameter("series", ParameterTypeKind.DynamicArray),
                new Parameter("series_imaginary", ParameterTypeKind.DynamicArray, minOccurring: 0));

        public static readonly FunctionSymbol SeriesIfft =
            new FunctionSymbol("series_ifft",
                context =>
                {
                    var source = context.GetArgument("series");
                    return MakePrefixedTuple(context, "series",
                        new TupleSymbol(
                            new ColumnSymbol("real", ScalarTypes.DynamicArrayOfReal, source: source),
                            new ColumnSymbol("imag", ScalarTypes.DynamicArrayOfReal, source: source)));
                },
                Tabularity.Scalar,
                new Parameter("series", ParameterTypeKind.DynamicArray),
                new Parameter("series_imaginary", ParameterTypeKind.DynamicArray, minOccurring: 0));

        public static readonly FunctionSymbol SeriesFitPoly =
            new FunctionSymbol("series_fit_poly",
                context =>
                {
                    var source = context.GetArgument("y_series");
                    return MakePrefixedTuple(context, "y_series",
                        new TupleSymbol(
                            new ColumnSymbol("rsquare", ScalarTypes.Real, source: source),
                            new ColumnSymbol("coefficients", ScalarTypes.DynamicArrayOfReal, source: source),
                            new ColumnSymbol("variance", ScalarTypes.Real, source: source),
                            new ColumnSymbol("rvariance", ScalarTypes.Real, source: source),
                            new ColumnSymbol("poly_fit", ScalarTypes.DynamicArrayOfReal, source: source)));
                },
                Tabularity.Scalar,
                new Parameter("y_series", ParameterTypeKind.DynamicArray),
                new Parameter("x_series", ParameterTypeKind.DynamicArray, minOccurring: 0),
                new Parameter("degree", ScalarTypes.Int, minOccurring: 0));

        public static readonly FunctionSymbol SeriesFitLine =
            new FunctionSymbol("series_fit_line",
                context =>
                {
                    var source = context.GetArgument("series");
                    return MakePrefixedTuple(context, "series",
                        new TupleSymbol(
                            new ColumnSymbol("rsquare", ScalarTypes.Real, source: source),
                            new ColumnSymbol("slope", ScalarTypes.Real, source: source),
                            new ColumnSymbol("variance", ScalarTypes.Real, source: source),
                            new ColumnSymbol("rvariance", ScalarTypes.Real, source: source),
                            new ColumnSymbol("interception", ScalarTypes.Real, source: source),
                            new ColumnSymbol("line_fit", ScalarTypes.DynamicArrayOfReal, source: source)));
                },
                Tabularity.Scalar,
                new Parameter("series", ParameterTypeKind.DynamicArray));

        public static readonly FunctionSymbol SeriesFitLineDynamic =
            new FunctionSymbol("series_fit_line_dynamic",
                    context =>
                    {
                        var source = context.GetArgument("series");
                        return ScalarTypes.GetDynamicBag(
                            new ColumnSymbol("rsquare", ScalarTypes.Real, source: source),
                            new ColumnSymbol("slope", ScalarTypes.Real, source: source),
                            new ColumnSymbol("variance", ScalarTypes.Real, source: source),
                            new ColumnSymbol("rvariance", ScalarTypes.Real, source: source),
                            new ColumnSymbol("interception", ScalarTypes.Real, source: source),
                            new ColumnSymbol("line_fit", ScalarTypes.DynamicArrayOfReal, source: source));
                    },
                    Tabularity.Scalar,
                    new Parameter("series", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesFit2Lines =
            new FunctionSymbol("series_fit_2lines",
                context =>
                {
                    var source = context.GetArgument("array");
                    return MakePrefixedTuple(context, "array",
                        new TupleSymbol(
                            new ColumnSymbol("rsquare", ScalarTypes.Real, source: source),
                            new ColumnSymbol("split_idx", ScalarTypes.Long, source: source),
                            new ColumnSymbol("variance", ScalarTypes.Real, source: source),
                            new ColumnSymbol("rvariance", ScalarTypes.Real, source: source),
                            new ColumnSymbol("line_fit", ScalarTypes.DynamicArrayOfReal, source: source),
                            new ColumnSymbol("right_rsquare", ScalarTypes.Real, source: source),
                            new ColumnSymbol("right_slope", ScalarTypes.Real, source: source),
                            new ColumnSymbol("right_interception", ScalarTypes.Real, source: source),
                            new ColumnSymbol("right_variance", ScalarTypes.Real, source: source),
                            new ColumnSymbol("right_rvariance", ScalarTypes.Real, source: source),
                            new ColumnSymbol("left_rsquare", ScalarTypes.Real, source: source),
                            new ColumnSymbol("left_slope", ScalarTypes.Real, source: source),
                            new ColumnSymbol("left_interception", ScalarTypes.Real, source: source),
                            new ColumnSymbol("left_variance", ScalarTypes.Real, source: source),
                            new ColumnSymbol("left_rvariance", ScalarTypes.Real, source: source)));
                },
                Tabularity.Scalar,
                new Parameter("array", ParameterTypeKind.DynamicArray));


        public static readonly FunctionSymbol SeriesFit2LinesDynamic =
            new FunctionSymbol("series_fit_2lines_dynamic",
                    context =>
                    {
                        var source = context.GetArgument("array");
                        return new DynamicBagSymbol(
                            new ColumnSymbol("rsquare", ScalarTypes.Real, source: source),
                            new ColumnSymbol("split_idx", ScalarTypes.Long, source: source),
                            new ColumnSymbol("variance", ScalarTypes.Real, source: source),
                            new ColumnSymbol("rvariance", ScalarTypes.Real, source: source),
                            new ColumnSymbol("line_fit", ScalarTypes.DynamicArrayOfReal, source: source),
                            new ColumnSymbol("right_rsquare", ScalarTypes.Real, source: source),
                            new ColumnSymbol("right_slope", ScalarTypes.Real, source: source),
                            new ColumnSymbol("right_interception", ScalarTypes.Real, source: source),
                            new ColumnSymbol("right_variance", ScalarTypes.Real, source: source),
                            new ColumnSymbol("right_rvariance", ScalarTypes.Real, source: source),
                            new ColumnSymbol("left_rsquare", ScalarTypes.Real, source: source),
                            new ColumnSymbol("left_slope", ScalarTypes.Real, source: source),
                            new ColumnSymbol("left_interception", ScalarTypes.Real, source: source),
                            new ColumnSymbol("left_variance", ScalarTypes.Real, source: source),
                            new ColumnSymbol("left_rvariance", ScalarTypes.Real, source: source));
                    },
                    Tabularity.Scalar,
                    new Parameter("series", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesOutliers =
            new FunctionSymbol("series_outliers",
                    new Signature(ReturnTypeKind.Parameter0,
                        new Parameter("series", ParameterTypeKind.DynamicArray),
                        new Parameter("kind", ScalarTypes.String, minOccurring: 0),
                        new Parameter("ignore_val", ParameterTypeKind.Number, minOccurring: 0)),
                    new Signature(ReturnTypeKind.Parameter0,
                        new Parameter("series", ParameterTypeKind.DynamicArray),
                        new Parameter("kind", ScalarTypes.String),
                        new Parameter("ignore_val", ScalarTypes.Real),
                        new Parameter("min_percentile", ScalarTypes.Real),
                        new Parameter("max_percentile", ScalarTypes.Real, minOccurring: 0),
                        new Parameter("test_points", ParameterTypeKind.Integer, minOccurring: 0)))
                .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol SeriesIIR =
            new FunctionSymbol("series_iir",
                    ScalarTypes.DynamicArrayOfReal,
                    new Parameter("series", ParameterTypeKind.DynamicArray),
                    new Parameter("numerators", ParameterTypeKind.DynamicArray),
                    new Parameter("denominators", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol SeriesPeriodsDetect =
            new FunctionSymbol("series_periods_detect",
                context =>
                {
                    var source = context.GetArgument("series");
                    return MakePrefixedTuple(context, "series",
                        new TupleSymbol(
                            new ColumnSymbol("periods", ScalarTypes.DynamicArrayOfReal, source: source),
                            new ColumnSymbol("scores", ScalarTypes.DynamicArrayOfReal, source: source)));
                },
                Tabularity.Scalar,
                new Parameter("series", ParameterTypeKind.DynamicArray),
                new Parameter("min_period", ParameterTypeKind.Number),
                new Parameter("max_period", ParameterTypeKind.Number),
                new Parameter("num_periods", ScalarTypes.Long));

        public static readonly FunctionSymbol SeriesPeriodsValidate =
            new FunctionSymbol("series_periods_validate",
                context =>
                {
                    var source = context.GetArgument("series");
                    return MakePrefixedTuple(context, "series",
                        new TupleSymbol(
                            new ColumnSymbol("periods", ScalarTypes.DynamicArrayOfReal, source: source),
                            new ColumnSymbol("scores", ScalarTypes.DynamicArrayOfReal, source: source)));
                },
                Tabularity.Scalar,
                new Parameter("series", ParameterTypeKind.DynamicArray),
                new Parameter("period", ParameterTypeKind.Number, maxOccurring: 16));

        public static readonly FunctionSymbol SeriesFillBackwards =
            new FunctionSymbol("series_fill_backward",
                    ReturnTypeKind.Parameter0,
                    new Parameter("series", ParameterTypeKind.DynamicArray),
                    new Parameter("missing_value_placeholder", ParameterTypeKind.Number, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol SeriesFillForward =
            new FunctionSymbol("series_fill_forward",
                    ReturnTypeKind.Parameter0,
                    new Parameter("series", ParameterTypeKind.DynamicArray),
                    new Parameter("missing_value_placeholder", ParameterTypeKind.Number, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol SeriesFillConst =
            new FunctionSymbol("series_fill_const",
                    ReturnTypeKind.Parameter0,
                    new Parameter("series", ParameterTypeKind.DynamicArray),
                    new Parameter("constant_value", ParameterTypeKind.Number, minOccurring: 0),
                    new Parameter("missing_value_placeholder", ParameterTypeKind.Number, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol SeriesFillLinear =
            new FunctionSymbol("series_fill_linear",
                    ReturnTypeKind.Parameter0,
                    new Parameter("series", ParameterTypeKind.DynamicArray),
                    new Parameter("missing_value_placeholder", ParameterTypeKind.Number, minOccurring: 0),
                    new Parameter("fill_edges", ScalarTypes.Bool, minOccurring: 0),
                    new Parameter("constant_value", ParameterTypeKind.Number, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol SeriesAdd =
            new FunctionSymbol("series_add",
                    new Signature(
                        ScalarTypes.DynamicArray,
                        new Parameter("series1", ParameterTypeKind.DynamicArray),
                        new Parameter("series2", ParameterTypeKind.DynamicArray)),
                    new Signature(
                        ScalarTypes.DynamicArray,
                        new Parameter("series1", ParameterTypeKind.DynamicArray),
                        new Parameter("series2", ParameterTypeKind.Number)),
                    new Signature(
                        ScalarTypes.DynamicArray,
                        new Parameter("series1", ParameterTypeKind.Number),
                        new Parameter("series2", ParameterTypeKind.DynamicArray)))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesSubtract =
            new FunctionSymbol("series_subtract",
                    new Signature(ScalarTypes.DynamicArray,
                        new Parameter("series1", ParameterTypeKind.DynamicArray),
                        new Parameter("series2", ParameterTypeKind.DynamicArray)),
                    new Signature(ScalarTypes.DynamicArray,
                        new Parameter("series1", ParameterTypeKind.DynamicArray),
                        new Parameter("series2", ParameterTypeKind.Number)),
                    new Signature(ScalarTypes.DynamicArray,
                        new Parameter("series1", ParameterTypeKind.Number),
                        new Parameter("series2", ParameterTypeKind.DynamicArray)))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesMultiply =
            new FunctionSymbol("series_multiply",
                    new Signature(ScalarTypes.DynamicArray,
                        new Parameter("series1", ParameterTypeKind.DynamicArray),
                        new Parameter("series2", ParameterTypeKind.DynamicArray)),
                    new Signature(ScalarTypes.DynamicArray,
                        new Parameter("series1", ParameterTypeKind.DynamicArray),
                        new Parameter("series2", ParameterTypeKind.Number)),
                    new Signature(ScalarTypes.DynamicArray,
                        new Parameter("series1", ParameterTypeKind.Number),
                        new Parameter("series2", ParameterTypeKind.DynamicArray)))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesDivide =
            new FunctionSymbol("series_divide",
                    new Signature(
                        ScalarTypes.DynamicArrayOfReal,
                        new Parameter("series1", ParameterTypeKind.DynamicArray),
                        new Parameter("series2", ParameterTypeKind.DynamicArray)),
                    new Signature(
                        ScalarTypes.DynamicArrayOfReal,
                        new Parameter("series1", ParameterTypeKind.DynamicArray),
                        new Parameter("series2", ParameterTypeKind.Number)),
                    new Signature(
                        ScalarTypes.DynamicArrayOfReal,
                        new Parameter("series1", ParameterTypeKind.Number),
                        new Parameter("series2", ParameterTypeKind.DynamicArray)))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesPow =
            new FunctionSymbol("series_pow",
                    new Signature(
                        ScalarTypes.DynamicArrayOfReal,
                        new Parameter("series1", ParameterTypeKind.DynamicArray),
                        new Parameter("series2", ParameterTypeKind.DynamicArray)),
                    new Signature(
                        ScalarTypes.DynamicArrayOfReal,
                        new Parameter("series1", ParameterTypeKind.DynamicArray),
                        new Parameter("series2", ParameterTypeKind.Number)),
                    new Signature(
                        ScalarTypes.DynamicArrayOfReal,
                        new Parameter("series1", ParameterTypeKind.Number),
                        new Parameter("series2", ParameterTypeKind.DynamicArray)))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesGreater =
            new FunctionSymbol("series_greater",
                    new Signature(
                        ScalarTypes.DynamicArrayOfBool,
                        new Parameter("series1", ParameterTypeKind.DynamicArray),
                        new Parameter("series2", ParameterTypeKind.DynamicArray)),
                    new Signature(
                        ScalarTypes.DynamicArrayOfBool,
                        new Parameter("series1", ParameterTypeKind.DynamicArray),
                        new Parameter("series2", ParameterTypeKind.Number)),
                    new Signature(
                        ScalarTypes.DynamicArrayOfBool,
                        new Parameter("series1", ParameterTypeKind.Number),
                        new Parameter("series2", ParameterTypeKind.DynamicArray)))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesGreaterEquals =
            new FunctionSymbol("series_greater_equals",
                    new Signature(
                        ScalarTypes.DynamicArrayOfBool,
                        new Parameter("series1", ParameterTypeKind.DynamicArray),
                        new Parameter("series2", ParameterTypeKind.DynamicArray)),
                    new Signature(
                        ScalarTypes.DynamicArrayOfBool,
                        new Parameter("series1", ParameterTypeKind.DynamicArray),
                        new Parameter("series2", ParameterTypeKind.Number)),
                    new Signature(
                        ScalarTypes.DynamicArrayOfBool,
                        new Parameter("series1", ParameterTypeKind.Number),
                        new Parameter("series2", ParameterTypeKind.DynamicArray)))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesLess =
            new FunctionSymbol("series_less",
                    new Signature(
                        ScalarTypes.DynamicArrayOfBool,
                        new Parameter("series1", ParameterTypeKind.DynamicArray),
                        new Parameter("series2", ParameterTypeKind.DynamicArray)),
                    new Signature(
                        ScalarTypes.DynamicArrayOfBool,
                        new Parameter("series1", ParameterTypeKind.DynamicArray),
                        new Parameter("series2", ParameterTypeKind.Number)),
                    new Signature(
                        ScalarTypes.DynamicArrayOfBool,
                        new Parameter("series1", ParameterTypeKind.Number),
                        new Parameter("series2", ParameterTypeKind.DynamicArray)))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesLessEquals =
            new FunctionSymbol("series_less_equals",
                    new Signature(
                        ScalarTypes.DynamicArrayOfBool,
                        new Parameter("series1", ParameterTypeKind.DynamicArray),
                        new Parameter("series2", ParameterTypeKind.DynamicArray)),
                    new Signature(
                        ScalarTypes.DynamicArrayOfBool,
                        new Parameter("series1", ParameterTypeKind.DynamicArray),
                        new Parameter("series2", ParameterTypeKind.Number)),
                    new Signature(
                        ScalarTypes.DynamicArrayOfBool,
                        new Parameter("series1", ParameterTypeKind.Number),
                        new Parameter("series2", ParameterTypeKind.DynamicArray)))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesEquals =
            new FunctionSymbol("series_equals",
                    new Signature(
                        ScalarTypes.DynamicArrayOfBool,
                        new Parameter("series1", ParameterTypeKind.DynamicArray),
                        new Parameter("series2", ParameterTypeKind.DynamicArray)),
                    new Signature(
                        ScalarTypes.DynamicArrayOfBool,
                        new Parameter("series1", ParameterTypeKind.DynamicArray),
                        new Parameter("series2", ParameterTypeKind.Number)),
                    new Signature(
                        ScalarTypes.DynamicArrayOfBool,
                        new Parameter("series1", ParameterTypeKind.Number),
                        new Parameter("series2", ParameterTypeKind.DynamicArray)))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesNotEquals =
            new FunctionSymbol("series_not_equals",
                    new Signature(
                        ScalarTypes.DynamicArrayOfBool,
                        new Parameter("series1", ParameterTypeKind.DynamicArray),
                        new Parameter("series2", ParameterTypeKind.DynamicArray)),
                    new Signature(
                        ScalarTypes.DynamicArrayOfBool,
                        new Parameter("series1", ParameterTypeKind.DynamicArray),
                        new Parameter("series2", ParameterTypeKind.Number)),
                    new Signature(
                        ScalarTypes.DynamicArrayOfBool,
                        new Parameter("series1", ParameterTypeKind.Number),
                        new Parameter("series2", ParameterTypeKind.DynamicArray)))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesSeasonal =
            new FunctionSymbol("series_seasonal",
                    new Signature(
                        ScalarTypes.DynamicArrayOfReal,
                        new Parameter("series", ParameterTypeKind.DynamicArray)),
                    new Signature(
                        ScalarTypes.DynamicArrayOfReal,
                        new Parameter("series", ParameterTypeKind.DynamicArray),
                        new Parameter("period", ParameterTypeKind.Integer)),
                    new Signature(
                        ScalarTypes.DynamicArrayOfReal,
                        new Parameter("series", ParameterTypeKind.DynamicArray),
                        new Parameter("period", ParameterTypeKind.Integer),
                        new Parameter("test_points", ParameterTypeKind.Integer)))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesExp =
            new FunctionSymbol("series_exp",
                    ScalarTypes.DynamicArrayOfReal,
                    new Parameter("series", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesSign =
            new FunctionSymbol("series_sign",
                    ScalarTypes.DynamicArrayOfLong,
                    new Parameter("series", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesAbs =
            new FunctionSymbol("series_abs",
                    ReturnTypeKind.Parameter0,
                    new Parameter("series", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesSin =
            new FunctionSymbol("series_sin",
                    ScalarTypes.DynamicArrayOfReal,
                    new Parameter("series", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesAsin =
            new FunctionSymbol("series_asin",
                    ScalarTypes.DynamicArrayOfReal,
                    new Parameter("series", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesCos =
            new FunctionSymbol("series_cos",
                    ScalarTypes.DynamicArrayOfReal,
                    new Parameter("series", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesAcos =
            new FunctionSymbol("series_acos",
                    ScalarTypes.DynamicArrayOfReal,
                    new Parameter("series", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesTan =
            new FunctionSymbol("series_tan",
                    ScalarTypes.DynamicArrayOfReal,
                    new Parameter("series", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesAtan =
            new FunctionSymbol("series_atan",
                    ScalarTypes.DynamicArrayOfReal,
                    new Parameter("series", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesLog =
            new FunctionSymbol("series_log",
                    ScalarTypes.DynamicArrayOfReal,
                    new Parameter("series", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesFloor =
            new FunctionSymbol("series_floor",
                    ScalarTypes.DynamicArray,
                    new Parameter("series", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesCeiling =
            new FunctionSymbol("series_ceiling",
                    ReturnTypeKind.Parameter0,
                    new Parameter("series", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None);

        private static TypeSymbol SeriesDecomposeResult(CustomReturnTypeContext context)
        {
            var source = context.GetArgument("series");
            return MakePrefixedTuple(context, "series",
                new TupleSymbol(
                    new ColumnSymbol("baseline", ScalarTypes.DynamicArray, source: source),
                    new ColumnSymbol("seasonal", ScalarTypes.DynamicArray, source: source),
                    new ColumnSymbol("trend", ScalarTypes.DynamicArray, source: source),
                    new ColumnSymbol("residual", ScalarTypes.DynamicArray, source: source)));
        }

        private static TypeSymbol SeriesDecomposeAnomaliesResult(CustomReturnTypeContext context)
        {
            var source = context.GetArgument("series");
            return MakePrefixedTuple(context, "series",
                new TupleSymbol(
                    new ColumnSymbol("ad_flag", ScalarTypes.DynamicArray, source: source),
                    new ColumnSymbol("ad_score", ScalarTypes.DynamicArray, source: source),
                    new ColumnSymbol("baseline", ScalarTypes.DynamicArray, source: source)));
        }

        public static readonly FunctionSymbol SeriesDecompose =
            new FunctionSymbol("series_decompose",
                SeriesDecomposeResult,
                Tabularity.Scalar,
                new Parameter("series", ParameterTypeKind.DynamicArray),
                new Parameter("period", ParameterTypeKind.Integer, minOccurring: 0),
                new Parameter("trend", ScalarTypes.String, minOccurring: 0),
                new Parameter("test_points", ParameterTypeKind.Integer, minOccurring: 0),
                new Parameter("seasonality_threshold", ParameterTypeKind.Number, minOccurring: 0));

        public static readonly FunctionSymbol SeriesDecomposeForecast =
            new FunctionSymbol("series_decompose_forecast",
                SeriesDecomposeResult,
                Tabularity.Scalar,
                new Parameter("series", ParameterTypeKind.DynamicArray),
                new Parameter("test_points", ParameterTypeKind.Integer),
                new Parameter("period", ParameterTypeKind.Integer, minOccurring: 0),
                new Parameter("trend", ScalarTypes.String, minOccurring: 0),
                new Parameter("seasonality_threshold", ParameterTypeKind.Number, minOccurring: 0));

        public static readonly FunctionSymbol SeriesDecomposeAnomalies =
            new FunctionSymbol("series_decompose_anomalies",
                SeriesDecomposeAnomaliesResult,
                Tabularity.Scalar,
                new Parameter("series", ParameterTypeKind.DynamicArray),
                new Parameter("threshold", ParameterTypeKind.Number, minOccurring: 0),
                new Parameter("period", ParameterTypeKind.Integer, minOccurring: 0),
                new Parameter("trend", ScalarTypes.String, minOccurring: 0),
                new Parameter("test_points", ParameterTypeKind.Integer, minOccurring: 0),
                new Parameter("method", ScalarTypes.String, minOccurring: 0),
                new Parameter("seasonality_threshold", ParameterTypeKind.Number, minOccurring: 0));

        public static readonly FunctionSymbol SeriesPearsonCorrelation =
            new FunctionSymbol("series_pearson_correlation",
                    ScalarTypes.Real,
                    new Parameter("series1", ParameterTypeKind.DynamicArray),
                    new Parameter("series2", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesDotProduct =
            new FunctionSymbol("series_dot_product",
                    new Signature(
                        ScalarTypes.Real,
                        new Parameter("series1", ParameterTypeKind.DynamicArray),
                        new Parameter("series2", ParameterTypeKind.DynamicArray)),
                    new Signature(
                        ScalarTypes.Real,
                        new Parameter("series1", ParameterTypeKind.DynamicArray),
                        new Parameter("series2", ParameterTypeKind.Number)),
                    new Signature(
                        ScalarTypes.Real,
                        new Parameter("series1", ParameterTypeKind.Number),
                        new Parameter("series2", ParameterTypeKind.DynamicArray)))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesMagnitude =
            new FunctionSymbol("series_magnitude",
                    ScalarTypes.Real,
                    new Parameter("series", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesSum =
            new FunctionSymbol("series_sum",
                    ScalarTypes.Real,
                    new Parameter("series", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesProduct =
            new FunctionSymbol("series_product",
                    ScalarTypes.Real,
                    new Parameter("series", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol SeriesCosineSimilarity =
            new FunctionSymbol("series_cosine_similarity",
                    ScalarTypes.Real,
                    new Parameter("series1", ParameterTypeKind.DynamicArray),
                    new Parameter("series2", ParameterTypeKind.DynamicArray),
                    new Parameter("series1_magnitude", ScalarTypes.Real, minOccurring: 0),
                    new Parameter("series2_magnitude", ScalarTypes.Real, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.None);
        #endregion

        #region math functions
        public static readonly FunctionSymbol Round =
            new FunctionSymbol("round", ReturnTypeKind.Parameter0,
                    new Parameter("number", ParameterTypeKind.Number),
                    new Parameter("precision", ScalarTypes.Long, ArgumentKind.Constant, minOccurring: 0))
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
                    new Signature(ReturnTypeKind.Parameter0,
                        new Parameter("number", ParameterTypeKind.Number)),
                    new Signature(ScalarTypes.TimeSpan,
                        new Parameter("number", ScalarTypes.TimeSpan)))
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

        public static readonly FunctionSymbol Erf =
            new FunctionSymbol("erf", ScalarTypes.Real,
                    new Parameter("number", ParameterTypeKind.Number))
                .WithResultNameKind(ResultNameKind.FirstArgument)
                .ConstantFoldable();

        public static readonly FunctionSymbol Erfc =
            new FunctionSymbol("erfc", ScalarTypes.Real,
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
                    new Parameter("arg", ParameterTypeKind.CommonOrderable, minOccurring: 2, maxOccurring: 64))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol MinOf =
            new FunctionSymbol("min_of", ReturnTypeKind.Common,
                    new Parameter("arg", ParameterTypeKind.CommonOrderable, minOccurring: 2, maxOccurring: 64))
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
        public static readonly FunctionSymbol GeoFromWkt =
            new FunctionSymbol("geo_from_wkt", ScalarTypes.GeoShape,
                    new Parameter("wkt", ParameterTypeKind.StringOrArray))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoAngle =
            new FunctionSymbol("geo_angle",
                    ScalarTypes.Real,
                    new Parameter("p1_longitude", ParameterTypeKind.Number),
                    new Parameter("p1_latitude", ParameterTypeKind.Number),
                    new Parameter("p2_longitude", ParameterTypeKind.Number),
                    new Parameter("p2_latitude", ParameterTypeKind.Number),
                    new Parameter("p3_longitude", ParameterTypeKind.Number),
                    new Parameter("p3_latitude", ParameterTypeKind.Number))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoAzimuth =
            new FunctionSymbol("geo_azimuth",
                    ScalarTypes.Real,
                    new Parameter("p1_longitude", ParameterTypeKind.Number),
                    new Parameter("p1_latitude", ParameterTypeKind.Number),
                    new Parameter("p2_longitude", ParameterTypeKind.Number),
                    new Parameter("p2_latitude", ParameterTypeKind.Number))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoDistance2Points =
            new FunctionSymbol("geo_distance_2points",
                    ScalarTypes.Real,
                    new Parameter("p1_longitude", ParameterTypeKind.Number),
                    new Parameter("p1_latitude", ParameterTypeKind.Number),
                    new Parameter("p2_longitude", ParameterTypeKind.Number),
                    new Parameter("p2_latitude", ParameterTypeKind.Number),
                    new Parameter("use_spheroid", ParameterTypeKind.NumberOrBool, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoDistancePointToLine =
            new FunctionSymbol("geo_distance_point_to_line",
                    ScalarTypes.Real,
                    new Parameter("longitude", ParameterTypeKind.Number),
                    new Parameter("latitude", ParameterTypeKind.Number),
                    new Parameter("lineString", ParameterTypeKind.DynamicBag))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoDistancePointToPolygon =
            new FunctionSymbol("geo_distance_point_to_polygon",
                    ScalarTypes.Real,
                    new Parameter("longitude", ParameterTypeKind.Number),
                    new Parameter("latitude", ParameterTypeKind.Number),
                    new Parameter("polygon", ParameterTypeKind.DynamicBag))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoPointInCircle =
            new FunctionSymbol("geo_point_in_circle",
                    ScalarTypes.Bool,
                    new Parameter("p_longitude", ParameterTypeKind.Number),
                    new Parameter("p_latitude", ParameterTypeKind.Number),
                    new Parameter("pc_longitude", ParameterTypeKind.Number),
                    new Parameter("pc_latitude", ParameterTypeKind.Number),
                    new Parameter("c_radius", ParameterTypeKind.Number))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoPointInPolygon =
            new FunctionSymbol("geo_point_in_polygon",
                    ScalarTypes.Bool,
                    new Parameter("longitude", ParameterTypeKind.Number),
                    new Parameter("latitude", ParameterTypeKind.Number),
                    new Parameter("polygon", ParameterTypeKind.DynamicBag))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoPointBuffer =
            new FunctionSymbol("geo_point_buffer",
                    ScalarTypes.GeoShape,
                    new Parameter("longitude", ParameterTypeKind.Number),
                    new Parameter("latitude", ParameterTypeKind.Number),
                    new Parameter("radius", ParameterTypeKind.Number),
                    new Parameter("tolerance", ParameterTypeKind.Number, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoLineBuffer =
            new FunctionSymbol("geo_line_buffer",
                    ScalarTypes.GeoShape,
                    new Parameter("lineString", ParameterTypeKind.DynamicBag),
                    new Parameter("radius", ParameterTypeKind.Number),
                    new Parameter("tolerance", ParameterTypeKind.Number, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoPolygonBuffer =
            new FunctionSymbol("geo_polygon_buffer",
                    ScalarTypes.GeoShape,
                    new Parameter("polygon", ParameterTypeKind.DynamicBag),
                    new Parameter("radius", ParameterTypeKind.Number),
                    new Parameter("tolerance", ParameterTypeKind.Number, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoIntersects2Lines =
            new FunctionSymbol("geo_intersects_2lines",
                    ScalarTypes.Bool,
                    new Parameter("lineString1", ParameterTypeKind.DynamicBag),
                    new Parameter("lineString2", ParameterTypeKind.DynamicBag))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoIntersection2Lines =
            new FunctionSymbol("geo_intersection_2lines",
                    ScalarTypes.GeoShape,
                    new Parameter("lineString1", ParameterTypeKind.DynamicBag),
                    new Parameter("lineString2", ParameterTypeKind.DynamicBag))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoIntersectsLineWithPolygon =
            new FunctionSymbol("geo_intersects_line_with_polygon",
                    ScalarTypes.Bool,
                    new Parameter("lineString", ParameterTypeKind.DynamicBag),
                    new Parameter("polygon", ParameterTypeKind.DynamicBag))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoIntersectionLineWithPolygon =
            new FunctionSymbol("geo_intersection_line_with_polygon",
                    ScalarTypes.GeoShape,
                    new Parameter("lineString", ParameterTypeKind.DynamicBag),
                    new Parameter("polygon", ParameterTypeKind.DynamicBag))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoIntersects2Polygons =
            new FunctionSymbol("geo_intersects_2polygons",
                    ScalarTypes.Bool,
                    new Parameter("polygon1", ParameterTypeKind.DynamicBag),
                    new Parameter("polygon2", ParameterTypeKind.DynamicBag))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoIntersection2Polygons =
            new FunctionSymbol("geo_intersection_2polygons",
                    ScalarTypes.GeoShape,
                    new Parameter("polygon1", ParameterTypeKind.DynamicBag),
                    new Parameter("polygon2", ParameterTypeKind.DynamicBag))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoPolygonsUnion =
            new FunctionSymbol("geo_union_polygons_array",
                    ScalarTypes.GeoShape,
                    new Parameter("polygons", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoLinesUnion =
            new FunctionSymbol("geo_union_lines_array",
                    ScalarTypes.GeoShape,
                    new Parameter("lineStrings", ParameterTypeKind.DynamicArray))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoPolygonToS2Cells =
            new FunctionSymbol("geo_polygon_to_s2cells",
                    ScalarTypes.DynamicArrayOfString,
                    new Parameter("polygon", ParameterTypeKind.DynamicBag),
                    new Parameter("level", ParameterTypeKind.Number, minOccurring: 0),
                    new Parameter("radius", ParameterTypeKind.Number, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoLineToS2Cells =
            new FunctionSymbol("geo_line_to_s2cells",
                    ScalarTypes.DynamicArrayOfString,
                    new Parameter("lineString", ParameterTypeKind.DynamicBag),
                    new Parameter("level", ParameterTypeKind.Number, minOccurring: 0),
                    new Parameter("radius", ParameterTypeKind.Number, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoPolygonS2CellCoveringLevel =
            new FunctionSymbol("__geo_polygon_s2cell_covering_level",
                    ScalarTypes.Int,
                    new Parameter("polygon", ParameterTypeKind.DynamicBag))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable()
                .Hide();

        public static readonly FunctionSymbol GeoLineS2CellCoveringLevel =
            new FunctionSymbol("__geo_line_s2cell_covering_level",
                    ScalarTypes.Int,
                    new Parameter("lineString", ParameterTypeKind.DynamicBag))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable()
                .Hide();

        public static readonly FunctionSymbol GeoLengthToS2CellLevel =
            new FunctionSymbol("__geo_length_to_s2cell_level",
                    ScalarTypes.Int,
                    new Parameter("length", ParameterTypeKind.Number))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable()
                .Hide();

        public static readonly FunctionSymbol GeoPolygonDensify =
            new FunctionSymbol("geo_polygon_densify",
                    ScalarTypes.GeoShape,
                    new Parameter("polygon", ParameterTypeKind.DynamicBag),
                    new Parameter("tolerance", ParameterTypeKind.Number, minOccurring: 0),
                    new Parameter("preserve_crossing", ParameterTypeKind.NumberOrBool, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoPolygonArea =
            new FunctionSymbol("geo_polygon_area",
                    ScalarTypes.Real,
                    new Parameter("polygon", ParameterTypeKind.DynamicBag))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoPolygonCentroid =
            new FunctionSymbol("geo_polygon_centroid",
                    ScalarTypes.GeoShape,
                    new Parameter("polygon", ParameterTypeKind.DynamicBag))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoPolygonPerimeter =
            new FunctionSymbol("geo_polygon_perimeter",
                    ScalarTypes.Real,
                    new Parameter("polygon", ParameterTypeKind.DynamicBag))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoLineLength =
            new FunctionSymbol("geo_line_length",
                    ScalarTypes.Real,
                    new Parameter("lineString", ParameterTypeKind.DynamicBag))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoLineCentroid =
            new FunctionSymbol("geo_line_centroid",
                    ScalarTypes.GeoShape,
                    new Parameter("lineString", ParameterTypeKind.DynamicBag))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoLineDensify =
            new FunctionSymbol("geo_line_densify",
                    ScalarTypes.GeoShape,
                    new Parameter("lineString", ParameterTypeKind.DynamicBag),
                    new Parameter("tolerance", ParameterTypeKind.Number, minOccurring: 0),
                    new Parameter("preserve_crossing", ParameterTypeKind.NumberOrBool, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoLineSimplify =
            new FunctionSymbol("geo_line_simplify",
                    ScalarTypes.GeoShape,
                    new Parameter("lineString", ParameterTypeKind.DynamicBag),
                    new Parameter("tolerance", ParameterTypeKind.Number, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoLineLocatePoint =
            new FunctionSymbol("geo_line_locate_point",
                    ScalarTypes.Real,
                    new Parameter("lineString", ParameterTypeKind.DynamicBag),
                    new Parameter("longitude", ParameterTypeKind.Number),
                    new Parameter("latitude", ParameterTypeKind.Number),
                    new Parameter("use_spheroid", ParameterTypeKind.NumberOrBool, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoLineInterpolatePoint =
            new FunctionSymbol("geo_line_interpolate_point",
                    ScalarTypes.GeoShape,
                    new Parameter("lineString", ParameterTypeKind.DynamicBag),
                    new Parameter("fraction", ParameterTypeKind.Number))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoClosestPointOnLine =
            new FunctionSymbol("geo_closest_point_on_line",
                    ScalarTypes.GeoShape,
                    new Parameter("longitude", ParameterTypeKind.Number),
                    new Parameter("latitude", ParameterTypeKind.Number),
                    new Parameter("lineString", ParameterTypeKind.DynamicBag))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoClosestPointOnPolygon =
            new FunctionSymbol("geo_closest_point_on_polygon",
                    ScalarTypes.GeoShape,
                    new Parameter("longitude", ParameterTypeKind.Number),
                    new Parameter("latitude", ParameterTypeKind.Number),
                    new Parameter("polygon", ParameterTypeKind.DynamicBag))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoPolygonSimplify =
            new FunctionSymbol("geo_polygon_simplify",
                    ScalarTypes.GeoShape,
                    new Parameter("polygon", ParameterTypeKind.DynamicBag),
                    new Parameter("tolerance", ParameterTypeKind.Number, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoSimplifyPolygonsArray =
            new FunctionSymbol("geo_simplify_polygons_array",
                    ScalarTypes.GeoShape,
                    new Parameter("polygons", ParameterTypeKind.DynamicArray),
                    new Parameter("tolerance", ParameterTypeKind.Number, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoLineValidate =
            new FunctionSymbol("__geo_line_validate", ScalarTypes.String,
                    new Parameter("lineString", ParameterTypeKind.DynamicBag))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable()
                .Hide();

        public static readonly FunctionSymbol GeoPolygonValidate =
            new FunctionSymbol("__geo_polygon_validate", ScalarTypes.String,
                    new Parameter("polygon", ParameterTypeKind.DynamicBag))
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
            new FunctionSymbol("geo_geohash_to_central_point",
                    ScalarTypes.GeoShape,
                    new Parameter("geohash", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeohashToPolygon =
            new FunctionSymbol("geo_geohash_to_polygon",
                    ScalarTypes.GeoShape,
                    new Parameter("geohash", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeohashNeighbors =
            new FunctionSymbol("geo_geohash_neighbors",
                    ScalarTypes.DynamicArrayOfString,
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

        public static readonly FunctionSymbol GeoS2CellToCentralPoint =
            new FunctionSymbol("geo_s2cell_to_central_point",
                    ScalarTypes.GeoShape,
                    new Parameter("s2cell", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoS2CellNeighbors =
            new FunctionSymbol("geo_s2cell_neighbors",
                    ScalarTypes.DynamicArrayOfString,
                    new Parameter("s2cell", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoS2CellToPolygon =
            new FunctionSymbol("geo_s2cell_to_polygon",
                    ScalarTypes.GeoShape,
                    new Parameter("s2cell", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoPointToH3Cell =
            new FunctionSymbol("geo_point_to_h3cell", ScalarTypes.String,
                    new Parameter("longitude", ParameterTypeKind.Number),
                    new Parameter("latitude", ParameterTypeKind.Number),
                    new Parameter("resolution", ParameterTypeKind.Number, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoH3CellToCentralPoint =
            new FunctionSymbol("geo_h3cell_to_central_point",
                    ScalarTypes.GeoShape,
                    new Parameter("h3cell", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoH3CellToPolygon =
            new FunctionSymbol("geo_h3cell_to_polygon",
                    ScalarTypes.GeoShape,
                    new Parameter("h3cell", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoH3CellNeighbors =
            new FunctionSymbol("geo_h3cell_neighbors",
                    ScalarTypes.DynamicArrayOfString,
                    new Parameter("h3cell", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoH3CellChildren =
            new FunctionSymbol("geo_h3cell_children",
                    ScalarTypes.DynamicArrayOfString,
                    new Parameter("h3cell", ScalarTypes.String),
                    new Parameter("resolution", ParameterTypeKind.Number, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoH3CellParent =
            new FunctionSymbol("geo_h3cell_parent", ScalarTypes.String,
                    new Parameter("h3cell", ScalarTypes.String),
                    new Parameter("resolution", ParameterTypeKind.Number, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoH3CellRings =
            new FunctionSymbol("geo_h3cell_rings",
                    ScalarTypes.DynamicArrayOfArrayOfString,
                    new Parameter("h3cell", ScalarTypes.String),
                    new Parameter("distance", ParameterTypeKind.Number))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoH3CellLevel =
            new FunctionSymbol("geo_h3cell_level", ScalarTypes.Int,
                    new Parameter("h3cell", ScalarTypes.String))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();

        public static readonly FunctionSymbol GeoPolygonToH3Cells =
            new FunctionSymbol("geo_polygon_to_h3cells",
                    ScalarTypes.DynamicArrayOfString,
                    new Parameter("polygon", ParameterTypeKind.DynamicBag),
                    new Parameter("resolution", ParameterTypeKind.Number, minOccurring: 0),
                    new Parameter("radius", ParameterTypeKind.Number, minOccurring: 0))
                .WithResultNameKind(ResultNameKind.None)
                .ConstantFoldable();
        #endregion

        #region Graph functions
        public static readonly FunctionSymbol _All =
            new FunctionSymbol("all",
                ScalarTypes.Bool,
                new Parameter("pattern_element", ParameterTypeKind.Scalar),
                new Parameter("expression", ScalarTypes.Bool, ArgumentKind.Expression_Parameter0_Element))
                .WithCustomAvailability(InGraphWhereOrProjectClause);

        public static readonly FunctionSymbol Any =
            new FunctionSymbol("any",
                    ScalarTypes.Bool,
                    new Parameter("pattern_element", ParameterTypeKind.Scalar),
                    new Parameter("expression", ScalarTypes.Bool, ArgumentKind.Expression_Parameter0_Element))
                .WithCustomAvailability(InGraphWhereOrProjectClause);

        public static readonly FunctionSymbol Map =
            new FunctionSymbol("map",
                context =>
                {
                    if (context.Arguments.Count > 1)
                    {
                        var expr = context.Arguments[1];
                        return ScalarTypes.GetDynamicArray(expr.ResultType);
                    }

                    return ScalarTypes.DynamicArray;
                },
                Tabularity.Scalar,
                new Parameter("pattern_element", ParameterTypeKind.Scalar),
                new Parameter("expression", ParameterTypeKind.Scalar, ArgumentKind.Expression_Parameter0_Element))
                .WithCustomAvailability(InGraphWhereOrProjectClause);

        public static readonly FunctionSymbol InnerNodes =
            new FunctionSymbol("inner_nodes",
                    context =>
                    {
                        if (context.Arguments.Count > 0
                            && context.Arguments[0] is Expression arg)
                        {
                            if (arg.GetFirstAncestor<GraphMatchOperator>() is GraphMatchOperator graphMatch
                                    && graphMatch.Parent is PipeExpression pGM
                                    && pGM.Expression.ResultType is GraphSymbol gsGM)
                            {
                                return new TupleSymbol(gsGM.NodeShape?.Columns ?? new ColumnSymbol[0]);
                            }

                            if (arg.GetFirstAncestor<GraphShortestPathsOperator>() is GraphShortestPathsOperator graphShortestPaths
                                    && graphShortestPaths.Parent is PipeExpression pSP
                                    && pSP.Expression.ResultType is GraphSymbol gsSP)
                            {
                                return new TupleSymbol(gsSP.NodeShape?.Columns ?? new ColumnSymbol[0]);
                            }
                        }

                        // could not find node schema
                        return ScalarTypes.DynamicArrayOfBag;
                    },
                    Tabularity.Scalar,
                    new Parameter("edge", ParameterTypeKind.Scalar))
                .WithCustomAvailability(context =>
                {
                    var inGM = context.Location.GetFirstAncestor<GraphMatchOperator>() != null;
                    var inSP = context.Location.GetFirstAncestor<GraphShortestPathsOperator>() != null;

                    // can only occur in first argument of all/any/map functions
                    var inGraphLambda =
                        context.Location.GetFirstAncestor<FunctionCallExpression>(fce =>
                            fce.Name.SimpleName != Functions.InnerNodes.Name) is FunctionCallExpression fc
                        && (fc.Name.SimpleName == Functions._All.Name
                            || fc.Name.SimpleName == Functions.Any.Name
                            || fc.Name.SimpleName == Functions.Map.Name)
                        && (context.Location == fc.ArgumentList
                            || (fc.ArgumentList.Expressions.Count > 0
                                && fc.ArgumentList.Expressions[0].Element.IsAncestorOf(context.Location)));
                    return (inSP || inGM) && inGraphLambda;
                });

        public static readonly FunctionSymbol NodeDegreeIn =
            new FunctionSymbol("node_degree_in",
                ScalarTypes.Long,
                new Parameter("node", ParameterTypeKind.Scalar, minOccurring: 0))
                .WithCustomAvailability(InGraphWhereOrProjectClause);

        public static readonly FunctionSymbol NodeDegreeOut =
            new FunctionSymbol("node_degree_out",
                ScalarTypes.Long,
                new Parameter("node", ParameterTypeKind.Scalar, minOccurring: 0))
                .WithCustomAvailability(InGraphWhereOrProjectClause);

        public static readonly FunctionSymbol NodeId =
            new FunctionSymbol("node_id",
                    ScalarTypes.String,
                    new Parameter("node", ParameterTypeKind.Scalar, minOccurring: 0))
                .WithCustomAvailability(InGraphWhereOrProjectClause)
                .Hide();

        public static readonly FunctionSymbol Labels =
            new FunctionSymbol("labels",
                    ScalarTypes.DynamicArrayOfString,
                    new Parameter("pattern_element", ParameterTypeKind.Scalar, minOccurring: 0))
                .WithCustomAvailability(InGraphWhereOrProjectClause);

        private static bool InGraphWhereOrProjectClause(CustomAvailabilityContext context)
        {
            var gmMatch = (context.Location.GetFirstAncestor<GraphMatchOperator>() is GraphMatchOperator gm &&
                   (InClause(gm.WhereClause) || InClause(gm.ProjectClause)));
            
            var spMatch = (context.Location.GetFirstAncestor<GraphShortestPathsOperator>() is GraphShortestPathsOperator sp &&
                   (InClause(sp.WhereClause) || InClause(sp.ProjectClause)));

            return gmMatch || spMatch;

            bool InClause(SyntaxNode clause) =>
                clause != null &&
                (clause == context.Location || clause.IsAncestorOf(context.Location));
        }

        #endregion

        #region other
        public static readonly FunctionSymbol CurrentClusterEndpoint =
            new FunctionSymbol("current_cluster_endpoint", ScalarTypes.String);

        public static readonly FunctionSymbol CurrentDatabase =
            new FunctionSymbol("current_database", ScalarTypes.String);

        public static readonly FunctionSymbol CurrentPrincipal =
            new FunctionSymbol("current_principal", ScalarTypes.String);
        // result column name is dependent on some guid?

        private static readonly TypeSymbol CurrentPrincipalDetailsResult =
            ScalarTypes.GetDynamicBag(
                new ColumnSymbol("UserPrincipalName", ScalarTypes.String),
                new ColumnSymbol("IdentityProvider", ScalarTypes.String),
                new ColumnSymbol("Authority", ScalarTypes.String),
                new ColumnSymbol("Mfa", ScalarTypes.String),
                new ColumnSymbol("Type", ScalarTypes.String),
                new ColumnSymbol("DisplayName", ScalarTypes.String),
                new ColumnSymbol("ObjectId", ScalarTypes.String),
                new ColumnSymbol("FQN", ScalarTypes.String),
                new ColumnSymbol("Notes", ScalarTypes.String));

        public static readonly FunctionSymbol CurrentPrincipalDetails =
            new FunctionSymbol("current_principal_details",
                CurrentPrincipalDetailsResult)
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol CurrentPrincipalIsMemberOf =
          new FunctionSymbol("current_principal_is_member_of",
              ScalarTypes.Bool,
              new Parameter("group", ParameterTypeKind.StringOrArray, minOccurring: 1, maxOccurring: 64))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol ExtentId =
            new FunctionSymbol("extent_id", ScalarTypes.Guid)
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol ExtentId2 =
            new FunctionSymbol("extentid", ScalarTypes.Guid)
            .Obsolete("extent_id");

        public static readonly FunctionSymbol ExtentTags =
            new FunctionSymbol("extent_tags",
                ScalarTypes.DynamicArrayOfString)
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
            new FunctionSymbol("current_cursor", ScalarTypes.String)
            .Obsolete("cursor_current");

        public static readonly FunctionSymbol FormatBytes =
            new FunctionSymbol("format_bytes", ScalarTypes.String,
                new Parameter("size", ParameterTypeKind.Number),
                new Parameter("precision", ParameterTypeKind.Number, ArgumentKind.Constant, minOccurring: 0),
                new Parameter("format", ScalarTypes.String, ArgumentKind.Constant, minOccurring: 0))
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

        public static readonly FunctionSymbol RowRank =
            new FunctionSymbol("row_rank", ScalarTypes.Long,
                new Parameter("column", ParameterTypeKind.NotDynamic),
                new Parameter("dense", ScalarTypes.Bool, ArgumentKind.Constant, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.None)
            .Obsolete("row_rank_dense");

        public static readonly FunctionSymbol RowRankMin =
            new FunctionSymbol("row_rank_min", ScalarTypes.Long,
                new Parameter("term", ParameterTypeKind.NotDynamic),
                new Parameter("restart", ScalarTypes.Bool, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol RowRankDense =
            new FunctionSymbol("row_rank_dense", ScalarTypes.Long,
                new Parameter("term", ParameterTypeKind.NotDynamic),
                new Parameter("restart", ScalarTypes.Bool, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol RowWindowSession =
            new FunctionSymbol("row_window_session", ReturnTypeKind.Parameter0,
                new Parameter("expr", ScalarTypes.DateTime),
                new Parameter("maxDistanceFromFirst", ScalarTypes.TimeSpan),
                new Parameter("maxDistanceBetweenNeighbors", ScalarTypes.TimeSpan),
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
            new FunctionSymbol("rowstore_ordinal_range",
                ScalarTypes.Dynamic)
            .WithResultNameKind(ResultNameKind.None)
            .Hide();

        public static readonly FunctionSymbol EstimateDataSize =
            new FunctionSymbol("estimate_data_size",
                new Signature(ScalarTypes.Long,
                    new Parameter("column", ParameterTypeKind.Scalar, ArgumentKind.Column, minOccurring: 1, maxOccurring: MaxRepeat)),
                new Signature(ScalarTypes.Long,
                    new Parameter("column", ParameterTypeKind.Scalar, ArgumentKind.StarOnly)))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol NewGuid = new FunctionSymbol("new_guid", ScalarTypes.Guid)
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol HasIpv4 =
            new FunctionSymbol("has_ipv4", ScalarTypes.Bool,
                new Parameter("source", ParameterTypeKind.StringOrDynamic),
                new Parameter("ip", ScalarTypes.String))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol HasIpv4Prefix =
            new FunctionSymbol("has_ipv4_prefix", ScalarTypes.Bool,
                new Parameter("source", ParameterTypeKind.StringOrDynamic),
                new Parameter("ip_prefix", ScalarTypes.String))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol HasAnyIpv4 =
            new FunctionSymbol("has_any_ipv4",
                new Signature(ScalarTypes.Bool,
                    new Parameter("source", ParameterTypeKind.StringOrDynamic),
                    new Parameter("ips", ScalarTypes.String, maxOccurring: MaxRepeat)),
                new Signature(ScalarTypes.Bool,
                    new Parameter("source", ParameterTypeKind.StringOrDynamic),
                    new Parameter("ips", ParameterTypeKind.DynamicArray)))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol HasAnyIpv4Prefix =
            new FunctionSymbol("has_any_ipv4_prefix",
                new Signature(ScalarTypes.Bool,
                    new Parameter("source", ParameterTypeKind.StringOrDynamic),
                    new Parameter("ip_prefixes", ScalarTypes.String, maxOccurring: MaxRepeat)),
                new Signature(ScalarTypes.Bool,
                    new Parameter("source", ParameterTypeKind.StringOrDynamic),
                    new Parameter("ip_prefixes", ParameterTypeKind.DynamicArray)))
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol Invoke =
            new FunctionSymbol("__invoke",
                ReturnTypeKind.Parameter1Literal,
                new Parameter("name", ScalarTypes.String, ArgumentKind.Literal),
                new Parameter("type", ScalarTypes.Type, ArgumentKind.Literal),
                new Parameter("arg", ParameterTypeKind.Any, minOccurring: 0, maxOccurring: MaxRepeat))
            .ConstantFoldable()
            .Hide();

        public static readonly FunctionSymbol Cast =
            new FunctionSymbol("__cast",
                ReturnTypeKind.Parameter0Literal,
                new Parameter("type", ScalarTypes.Type, ArgumentKind.Literal),
                new Parameter("value", ParameterTypeKind.Scalar))
            .ConstantFoldable()
            .Hide();

        public static readonly FunctionSymbol IpGeoLocation =
            new FunctionSymbol("geo_info_from_ip_address", ScalarTypes.Dynamic,
                new Parameter("ip", ScalarTypes.String))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);

        public static readonly FunctionSymbol ColumnNamesOf =
            new FunctionSymbol("column_names_of", ScalarTypes.DynamicArrayOfString,
                new Parameter("table", ParameterTypeKind.Tabular))
            .ConstantFoldable()
            .WithResultNameKind(ResultNameKind.None);
        #endregion

        #region All
        public static IReadOnlyList<FunctionSymbol> All { get; } = new FunctionSymbol[]
        {
#region cluster / database / table / etc
            Cluster,
            Database,
            Table,
            ExternalTable,
            MaterializedView,
            EntityGroup,
            StoredQueryResult,
            Graph,
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
            ToUtf8_Deprecated,
            UnicodeCodepointsFromString,
            Substring,
            RegexQuote,
            IndexOf,
            IndexOfRegex,
            HasAnyIndex,
            Reverse,
            Split,
            ParseCommandLine,
            Extract,
            ExtractAll_Deprecated,
            ExtractAll,
            ExtractJson_Deprecated,
            ExtractJson,
            Replace,
            ReplaceRegex,
            ReplaceString,
            ReplaceStrings,
            TrimStart,
            TrimEnd,
            Trim,
            CountOf,
            Translate,
            MakeString_Deprecated,
            UnicodeCodepointsToString,
            DatetimeLocalToUtc,
            DatetimeUtcToLocal,
            DateTimeListTimezones,
#endregion

#region type conversion functions
            ToString,
            ToHex,
            ToDynamic_,
            ToObject_Deprecated,
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
            Base64DecodeToGuid,
            Base64EncodeFromGuid,
            ZlibDecompressString,
            ZlibCompressString,
            GzipDecompressString,
            GzipCompressString,
            Lz4CompressDynamicArray,
            #endregion

#region parsing functions
            ParseCsv,
            ParseJson_Deprecated,
            ParseJson,
            ParseXml,
            ParseUrl_Deprecated,
            ParseUrl,
            ParseUrlQuery_Deprecated,
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
            HashSha1,
            HashXXH64,
            InternalHashXXH64,
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
            NotNull_Deprecated,
            IsNotNull,
            IsNull,
            NotEmpty_Deprecated,
            IsColumnExists,
            IsAscii,
            IsUtf8,
            IsNotEmpty,
            IsEmpty,
            ColumnIfExists_Deprecated,
            ColumnIfExists,
            Around,
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
            Arraylength_Deprecated,
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
            ArrayReverse,
            ArrayRotateLeft,
            ArrayRotateRight,
            ArraySortAsc,
            ArraySortDesc,
            BagKeys,
            Zip,
            Pack,
            PackDictionary,
            BagPack,
            BagPackColumns,
            PackAll,
            PackArray,
            SetHasElement,
            SetUnion,
            SetIntersect,
            SetDifference,
            SetEquals,
            BagMerge,
            DynamicToJson,
            BagRemoveKeys,
            BagHasKey,
            JaccardIndex,
            BagSetKey,
            BagZip,
            PunycodeDecode,
            PunycodeEncode,
            PunycodeDomainDecode,
            PunycodeDomainEncode,
#endregion

#region digest / series functions
            PercentileTDigest,
            PercentileArrayTDigest,
            PercentRankTDigest,
            RankTDigest,
            TdigestIsValid,
            HllIsValid,
            TDigestMerge,
            MergeTDigest,
            HllMerge,
            DCountHll,
            HllNormalize,
            SeriesFir,
            SeriesStats,
            SeriesStatsDynamic,
            SeriesFft,
            SeriesIfft,
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
            SeriesFitPoly,
            SeriesAdd,
            SeriesSubtract,
            SeriesMultiply,
            SeriesDivide,
            SeriesPow,
            SeriesGreater,
            SeriesGreaterEquals,
            SeriesLess,
            SeriesLessEquals,
            SeriesEquals,
            SeriesNotEquals,
            SeriesExp,
            SeriesSign,
            SeriesAbs,
            SeriesSin,
            SeriesAsin,
            SeriesCos,
            SeriesAcos,
            SeriesTan,
            SeriesAtan,
            SeriesMagnitude,
            SeriesSum,
            SeriesProduct,
            SeriesLog,
            SeriesFloor,
            SeriesCeiling,
            ArraySum,
            SeriesSeasonal,
            SeriesDecompose,
            SeriesDecomposeForecast,
            SeriesDecomposeAnomalies,
            SeriesPearsonCorrelation,
            SeriesDotProduct,
            SeriesCosineSimilarity,
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
            Erf,
            Erfc,
            IsNan,
            IsInf,
            IsFinite,
            Coalesce,
            MaxOf,
            MinOf,
            WelchTest,
#endregion

#region geospatial functions
            GeoFromWkt,
            GeoAngle,
            GeoAzimuth,
            GeoClosestPointOnLine,
            GeoClosestPointOnPolygon,
            GeoDistance2Points,
            GeoDistancePointToLine,
            GeoDistancePointToPolygon,
            GeoPointInCircle,
            GeoPointInPolygon,
            GeoIntersects2Lines,
            GeoIntersectsLineWithPolygon,
            GeoIntersects2Polygons,
            GeoIntersection2Lines,
            GeoIntersectionLineWithPolygon,
            GeoIntersection2Polygons,
            GeoPolygonsUnion,
            GeoSimplifyPolygonsArray,
            GeoLinesUnion,
            GeoPolygonToH3Cells,
            GeoPolygonToS2Cells,
            GeoPolygonS2CellCoveringLevel,
            GeoPolygonDensify,
            GeoPolygonArea,
            GeoPolygonBuffer,
            GeoPolygonCentroid,
            GeoPolygonValidate,
            GeoPolygonPerimeter,
            GeoPolygonSimplify,
            GeoLineS2CellCoveringLevel,
            GeoLineLength,
            GeoLineBuffer,
            GeoLineCentroid,
            GeoLineDensify,
            GeoLineSimplify,
            GeoLineLocatePoint,
            GeoLineInterpolatePoint,
            GeoLineValidate,
            GeoLineToS2Cells,
            GeoLengthToS2CellLevel,
            GeoPointToGeohash,
            GeohashToCentralPoint,
            GeohashToPolygon,
            GeohashNeighbors,
            GeoPointBuffer,
            GeoPointToS2Cell,
            GeoS2CellToCentralPoint,
            GeoS2CellToPolygon,
            GeoS2CellNeighbors,
            GeoPointToH3Cell,
            GeoH3CellToCentralPoint,
            GeoH3CellToPolygon,
            GeoH3CellNeighbors,
            GeoH3CellChildren,
            GeoH3CellParent,
            GeoH3CellRings,
            GeoH3CellLevel,
            #endregion

            #region graph functions
            _All,
            Any,
            Map,
            InnerNodes,
            NodeId,
            NodeDegreeIn,
            NodeDegreeOut,
            Labels,
            #endregion

            #region ip-matching functions
            Ipv4Compare,
            Ipv4IsMatch,
            Ipv6Compare,
            Ipv4IsPrivate,
            Ipv6IsMatch,
            Ipv6IsInRange,
            Ipv6IsInAnyRange,
            Ipv4IsInRange,
            Ipv4IsInAnyRange,
            Ipv4NetmaskSuffix,
            Ipv6LookupRanges,
            #endregion

            #region formatting functions
            FormatIPV4,
            FormatIPV4Mask,
            FormatBytes,
            #endregion

            #region convert functions
            ConvertAngle,
            ConvertEnergy,
            ConvertForce,
            ConvertLength,
            ConvertMass,
            ConvertSpeed,
            ConvertTemperature,
            ConvertVolume,
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
            HasIpv4,
            HasIpv4Prefix,
            HasAnyIpv4,
            HasAnyIpv4Prefix,
            Ipv4RangeToCidrList,
            RowNumber,
            RowCumSum,
            RowRank,
            RowRankDense,
            RowRankMin,
            RowWindowSession,
            Prev,
            Next,
            RowstoreOrdinalRange,
            EstimateDataSize,
            NewGuid,
            Invoke,
            Cast,
            IpGeoLocation,
            ColumnNamesOf,
#endregion
        };
        #endregion
    }
}