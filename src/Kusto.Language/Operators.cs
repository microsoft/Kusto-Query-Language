using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language
{
    using Symbols;
    using Utils;

    internal static class Operators
    {
        private static readonly TypeSymbol[] DateAndTimespan = new[] 
        {
            ScalarTypes.DateTime,
            ScalarTypes.TimeSpan
        };

        private static readonly TypeSymbol[] StringOrDynamic = new[]
        {
            ScalarTypes.String,
            ScalarTypes.Dynamic
        };

        private static readonly TypeSymbol[] DynamicAddable = new[]
        {
            ScalarTypes.Int,
            ScalarTypes.Long,
            ScalarTypes.Real,
            ScalarTypes.Decimal,
            ScalarTypes.TimeSpan,
            ScalarTypes.DateTime
        };

        private static OperatorSymbol Binary(OperatorKind kind, TypeSymbol left, TypeSymbol right, TypeSymbol result)
            => new OperatorSymbol(kind, new Signature(result, new Parameter("left", left), new Parameter("right", right)));

        private static OperatorSymbol StringBinary(OperatorKind kind)
            => new OperatorSymbol(kind, 
                new Signature(ScalarTypes.Bool, 
                    new Parameter("left", ParameterTypeKind.StringOrDynamic), 
                    new Parameter("right", ScalarTypes.String)),
                new Signature(ScalarTypes.Bool,
                    new Parameter("left", ParameterTypeKind.StringOrDynamic, ArgumentKind.Star),
                    new Parameter("right", ScalarTypes.String)));

        public static readonly OperatorSymbol UnaryMinus =
            new OperatorSymbol(OperatorKind.UnaryMinus,
                    new Signature(ReturnTypeKind.Parameter0, new Parameter("operand", ParameterTypeKind.Summable)),
                    new Signature(ScalarTypes.Dynamic, new Parameter("operand", ScalarTypes.Dynamic)));

        public static readonly OperatorSymbol UnaryPlus =
            new OperatorSymbol(OperatorKind.UnaryPlus,
                new Signature(ReturnTypeKind.Parameter0, new Parameter("operand", ParameterTypeKind.Summable)),
                new Signature(ScalarTypes.Dynamic, new Parameter("operand", ScalarTypes.Dynamic)));

        public static readonly OperatorSymbol And =
            Binary(OperatorKind.And, ScalarTypes.Bool, ScalarTypes.Bool, ScalarTypes.Bool);

        public static readonly OperatorSymbol Or =
            Binary(OperatorKind.Or, ScalarTypes.Bool, ScalarTypes.Bool, ScalarTypes.Bool);

        public static readonly OperatorSymbol Add =
            new OperatorSymbol(OperatorKind.Add,
                new Signature(ReturnTypeKind.Widest, new Parameter("left", ParameterTypeKind.Number), new Parameter("right", ParameterTypeKind.Number)),
                new Signature(ScalarTypes.TimeSpan, new Parameter("left", ScalarTypes.TimeSpan), new Parameter("right", ScalarTypes.TimeSpan)),
                new Signature(ScalarTypes.DateTime, new Parameter("left", DateAndTimespan), new Parameter("right", DateAndTimespan)),
                new Signature(ScalarTypes.Dynamic, new Parameter("left", ScalarTypes.Dynamic), new Parameter("right", ScalarTypes.Dynamic)),
                new Signature(ScalarTypes.Long, new Parameter("left", ScalarTypes.Dynamic), new Parameter("right", ScalarTypes.Int)),
                new Signature(ScalarTypes.Long, new Parameter("left", ScalarTypes.Int), new Parameter("right", ScalarTypes.Dynamic)),
                new Signature(ReturnTypeKind.Parameter1, new Parameter("left", ScalarTypes.Dynamic), new Parameter("right", DynamicAddable)),
                new Signature(ReturnTypeKind.Parameter0, new Parameter("left", DynamicAddable), new Parameter("right", ScalarTypes.Dynamic)));

        public static readonly OperatorSymbol Subtract =
            new OperatorSymbol(OperatorKind.Subtract,
                new Signature(ReturnTypeKind.Widest, new Parameter("left", ParameterTypeKind.Number), new Parameter("right", ParameterTypeKind.Number)),
                new Signature(ScalarTypes.TimeSpan, new Parameter("left", ScalarTypes.TimeSpan), new Parameter("right", ScalarTypes.TimeSpan)),
                new Signature(ScalarTypes.TimeSpan, new Parameter("left", ScalarTypes.DateTime), new Parameter("right", ScalarTypes.DateTime)),
                new Signature(ScalarTypes.DateTime, new Parameter("left", ScalarTypes.DateTime), new Parameter("right", ScalarTypes.TimeSpan)),
                new Signature(ScalarTypes.DateTime, new Parameter("left", ScalarTypes.TimeSpan), new Parameter("right", ScalarTypes.DateTime)),
                new Signature(ScalarTypes.Dynamic, new Parameter("left", ScalarTypes.Dynamic), new Parameter("right", ScalarTypes.Dynamic)),
                new Signature(ScalarTypes.Long, new Parameter("left", ScalarTypes.Dynamic), new Parameter("right", ScalarTypes.Int)),
                new Signature(ScalarTypes.Long, new Parameter("left", ScalarTypes.Int), new Parameter("right", ScalarTypes.Dynamic)),
                new Signature(ReturnTypeKind.Parameter1, new Parameter("left", ScalarTypes.Dynamic), new Parameter("right", DynamicAddable)),
                new Signature(ReturnTypeKind.Parameter0, new Parameter("left", DynamicAddable), new Parameter("right", ScalarTypes.Dynamic)));

        public static readonly OperatorSymbol Multiply =
            new OperatorSymbol(OperatorKind.Multiply,
                new Signature(ReturnTypeKind.Widest, new Parameter("left", ParameterTypeKind.Number), new Parameter("right", ParameterTypeKind.Number)),
                new Signature(ScalarTypes.TimeSpan, new Parameter("left", ScalarTypes.TimeSpan), new Parameter("right", ScalarTypes.TimeSpan)),
                new Signature(ScalarTypes.TimeSpan, new Parameter("left", ScalarTypes.TimeSpan), new Parameter("right", ParameterTypeKind.Number)),
                new Signature(ScalarTypes.TimeSpan, new Parameter("left", ParameterTypeKind.Number), new Parameter("right", ScalarTypes.TimeSpan)),
                new Signature(ScalarTypes.DateTime, new Parameter("left", ScalarTypes.DateTime), new Parameter("right", ParameterTypeKind.Number)),
                new Signature(ScalarTypes.DateTime, new Parameter("left", ParameterTypeKind.Number), new Parameter("right", ScalarTypes.DateTime)),
                new Signature(ScalarTypes.Dynamic, new Parameter("left", ScalarTypes.Dynamic), new Parameter("right", ScalarTypes.Dynamic)),
                new Signature(ScalarTypes.Long, new Parameter("left", ScalarTypes.Dynamic), new Parameter("right", ScalarTypes.Int)),
                new Signature(ScalarTypes.Long, new Parameter("left", ScalarTypes.Int), new Parameter("right", ScalarTypes.Dynamic)),
                new Signature(ReturnTypeKind.Parameter1, new Parameter("left", ScalarTypes.Dynamic), new Parameter("right", ParameterTypeKind.Number)),
                new Signature(ReturnTypeKind.Parameter0, new Parameter("left", ParameterTypeKind.Number), new Parameter("right", ScalarTypes.Dynamic)));

        public static readonly OperatorSymbol Divide =
            new OperatorSymbol(OperatorKind.Divide,
                new Signature(ReturnTypeKind.Widest, new Parameter("left", ParameterTypeKind.Number), new Parameter("right", ParameterTypeKind.Number)),
                new Signature(ScalarTypes.Real, new Parameter("left", ScalarTypes.TimeSpan), new Parameter("right", ScalarTypes.TimeSpan)),
                new Signature(ScalarTypes.TimeSpan, new Parameter("left", ScalarTypes.TimeSpan), new Parameter("right", ParameterTypeKind.Number)),
                new Signature(ScalarTypes.Real, new Parameter("left", ScalarTypes.DateTime), new Parameter("right", ScalarTypes.DateTime)),
                new Signature(ScalarTypes.Real, new Parameter("left", ScalarTypes.DateTime), new Parameter("right", ScalarTypes.TimeSpan)),
                new Signature(ScalarTypes.DateTime, new Parameter("left", ScalarTypes.DateTime), new Parameter("right", ParameterTypeKind.Number)),
                new Signature(ScalarTypes.Dynamic, new Parameter("left", ScalarTypes.Dynamic), new Parameter("right", ScalarTypes.Dynamic)),
                new Signature(ScalarTypes.Long, new Parameter("left", ScalarTypes.Dynamic), new Parameter("right", ScalarTypes.Int)),
                new Signature(ScalarTypes.Long, new Parameter("left", ScalarTypes.Int), new Parameter("right", ScalarTypes.Dynamic)),
                new Signature(ReturnTypeKind.Parameter1, new Parameter("left", ScalarTypes.Dynamic), new Parameter("right", ParameterTypeKind.Number)),
                new Signature(ReturnTypeKind.Parameter0, new Parameter("left", ParameterTypeKind.Number), new Parameter("right", ScalarTypes.Dynamic)));

        public static readonly OperatorSymbol Modulo =
            new OperatorSymbol(OperatorKind.Modulo,
                new Signature(ReturnTypeKind.Widest, new Parameter("left", ParameterTypeKind.Number), new Parameter("right", ParameterTypeKind.Number)),
                new Signature(ScalarTypes.TimeSpan, new Parameter("left", ScalarTypes.TimeSpan), new Parameter("right", ScalarTypes.TimeSpan)),
                new Signature(ScalarTypes.TimeSpan, new Parameter("left", ScalarTypes.TimeSpan), new Parameter("right", ParameterTypeKind.Number)),
                new Signature(ScalarTypes.TimeSpan, new Parameter("left", ScalarTypes.DateTime), new Parameter("right", ScalarTypes.DateTime)),
                new Signature(ScalarTypes.TimeSpan, new Parameter("left", ScalarTypes.DateTime), new Parameter("right", ScalarTypes.TimeSpan)),
                new Signature(ScalarTypes.DateTime, new Parameter("left", ScalarTypes.DateTime), new Parameter("right", ParameterTypeKind.Number)),
                new Signature(ScalarTypes.Dynamic, new Parameter("left", ScalarTypes.Dynamic), new Parameter("right", ScalarTypes.Dynamic)),
                new Signature(ScalarTypes.Long, new Parameter("left", ScalarTypes.Dynamic), new Parameter("right", ScalarTypes.Int)),
                new Signature(ScalarTypes.Long, new Parameter("left", ScalarTypes.Int), new Parameter("right", ScalarTypes.Dynamic)),
                new Signature(ReturnTypeKind.Parameter1, new Parameter("left", ScalarTypes.Dynamic), new Parameter("right", ParameterTypeKind.Number)),
                new Signature(ReturnTypeKind.Parameter0, new Parameter("left", ParameterTypeKind.Number), new Parameter("right", ScalarTypes.Dynamic)));

        public static readonly OperatorSymbol LessThan =
            new OperatorSymbol(OperatorKind.LessThan,
                new Signature(ScalarTypes.Bool, new Parameter("left", ParameterTypeKind.Number), new Parameter("right", ParameterTypeKind.Number)),
                new Signature(ScalarTypes.Bool, new Parameter("left", ScalarTypes.TimeSpan), new Parameter("right", ScalarTypes.TimeSpan)),
                new Signature(ScalarTypes.Bool, new Parameter("left", ScalarTypes.DateTime), new Parameter("right", ScalarTypes.DateTime)),
                new Signature(ScalarTypes.Bool, new Parameter("left", ScalarTypes.Dynamic), new Parameter("right", ScalarTypes.Dynamic)),
                new Signature(ScalarTypes.Bool, new Parameter("left", ScalarTypes.Dynamic), new Parameter("right", ParameterTypeKind.Number)),
                new Signature(ScalarTypes.Bool, new Parameter("left", ParameterTypeKind.Number), new Parameter("right", ScalarTypes.Dynamic)));

        public static readonly OperatorSymbol LessThanOrEqual =
            new OperatorSymbol(OperatorKind.LessThanOrEqual,
                new Signature(ScalarTypes.Bool, new Parameter("left", ParameterTypeKind.Number), new Parameter("right", ParameterTypeKind.Number)),
                new Signature(ScalarTypes.Bool, new Parameter("left", ScalarTypes.TimeSpan), new Parameter("right", ScalarTypes.TimeSpan)),
                new Signature(ScalarTypes.Bool, new Parameter("left", ScalarTypes.DateTime), new Parameter("right", ScalarTypes.DateTime)),
                new Signature(ScalarTypes.Bool, new Parameter("left", ScalarTypes.Dynamic), new Parameter("right", ScalarTypes.Dynamic)),
                new Signature(ScalarTypes.Bool, new Parameter("left", ScalarTypes.Dynamic), new Parameter("right", ParameterTypeKind.Number)),
                new Signature(ScalarTypes.Bool, new Parameter("left", ParameterTypeKind.Number), new Parameter("right", ScalarTypes.Dynamic)));

        public static readonly OperatorSymbol GreaterThan =
            new OperatorSymbol(OperatorKind.GreaterThan,
                new Signature(ScalarTypes.Bool, new Parameter("left", ParameterTypeKind.Number), new Parameter("right", ParameterTypeKind.Number)),
                new Signature(ScalarTypes.Bool, new Parameter("left", ScalarTypes.TimeSpan), new Parameter("right", ScalarTypes.TimeSpan)),
                new Signature(ScalarTypes.Bool, new Parameter("left", ScalarTypes.DateTime), new Parameter("right", ScalarTypes.DateTime)),
                new Signature(ScalarTypes.Bool, new Parameter("left", ScalarTypes.Dynamic), new Parameter("right", ScalarTypes.Dynamic)),
                new Signature(ScalarTypes.Bool, new Parameter("left", ScalarTypes.Dynamic), new Parameter("right", ParameterTypeKind.Number)),
                new Signature(ScalarTypes.Bool, new Parameter("left", ParameterTypeKind.Number), new Parameter("right", ScalarTypes.Dynamic)));

        public static readonly OperatorSymbol GreaterThanOrEqual =
            new OperatorSymbol(OperatorKind.GreaterThanOrEqual,
                new Signature(ScalarTypes.Bool, new Parameter("left", ParameterTypeKind.Number), new Parameter("right", ParameterTypeKind.Number)),
                new Signature(ScalarTypes.Bool, new Parameter("left", ScalarTypes.TimeSpan), new Parameter("right", ScalarTypes.TimeSpan)),
                new Signature(ScalarTypes.Bool, new Parameter("left", ScalarTypes.DateTime), new Parameter("right", ScalarTypes.DateTime)),
                new Signature(ScalarTypes.Bool, new Parameter("left", ScalarTypes.Dynamic), new Parameter("right", ScalarTypes.Dynamic)),
                new Signature(ScalarTypes.Bool, new Parameter("left", ScalarTypes.Dynamic), new Parameter("right", ParameterTypeKind.Number)),
                new Signature(ScalarTypes.Bool, new Parameter("left", ParameterTypeKind.Number), new Parameter("right", ScalarTypes.Dynamic)));

        public static readonly OperatorSymbol Equal =
            new OperatorSymbol(OperatorKind.Equal,
                new Signature(ScalarTypes.Bool, new Parameter("left", ScalarTypes.Bool), new Parameter("right", ParameterTypeKind.Scalar)).Hide(), // hide bool == ??
                new Signature(ScalarTypes.Bool, new Parameter("left", ParameterTypeKind.NotBool), new Parameter("right", ParameterTypeKind.Scalar)),
                new Signature(ScalarTypes.Bool, new Parameter("left", ParameterTypeKind.NotBool, ArgumentKind.Star), new Parameter("right", ParameterTypeKind.Scalar)));

        public static readonly OperatorSymbol NotEqual =
            new OperatorSymbol(OperatorKind.NotEqual,
                new Signature(ScalarTypes.Bool, new Parameter("left", ScalarTypes.Bool), new Parameter("right", ParameterTypeKind.Scalar)).Hide(), // hide bool != ??
                new Signature(ScalarTypes.Bool, new Parameter("left", ParameterTypeKind.NotBool), new Parameter("right", ParameterTypeKind.Scalar)));

        public static readonly OperatorSymbol EqualTilde =
            StringBinary(OperatorKind.EqualTilde);

        public static readonly OperatorSymbol BangTilde =
            StringBinary(OperatorKind.BangTilde);

        public static readonly OperatorSymbol Has =
            StringBinary(OperatorKind.Has);

        public static readonly OperatorSymbol HasCs =
            StringBinary(OperatorKind.HasCs);

        public static readonly OperatorSymbol NotHas =
            StringBinary(OperatorKind.NotHas);

        public static readonly OperatorSymbol NotHasCs =
            StringBinary(OperatorKind.NotHasCs);

        public static readonly OperatorSymbol HasPrefix =
            StringBinary(OperatorKind.HasPrefix);

        public static readonly OperatorSymbol HasPrefixCs =
            StringBinary(OperatorKind.HasPrefixCs);

        public static readonly OperatorSymbol NotHasPrefix =
            StringBinary(OperatorKind.NotHasPrefix);

        public static readonly OperatorSymbol NotHasPrefixCs =
            StringBinary(OperatorKind.NotHasPrefixCs);

        public static readonly OperatorSymbol HasSuffix =
            StringBinary(OperatorKind.HasSuffix);

        public static readonly OperatorSymbol HasSuffixCs =
            StringBinary(OperatorKind.HasSuffixCs);

        public static readonly OperatorSymbol NotHasSuffix =
            StringBinary(OperatorKind.NotHasSuffix);

        public static readonly OperatorSymbol NotHasSuffixCs =
            StringBinary(OperatorKind.NotHasSuffixCs);

        public static readonly OperatorSymbol Like =
            StringBinary(OperatorKind.Like);

        public static readonly OperatorSymbol LikeCs =
            StringBinary(OperatorKind.LikeCs);

        public static readonly OperatorSymbol NotLike =
            StringBinary(OperatorKind.NotLike);

        public static readonly OperatorSymbol NotLikeCs =
            StringBinary(OperatorKind.NotLikeCs);

        public static readonly OperatorSymbol Contains =
            StringBinary(OperatorKind.Contains);

        public static readonly OperatorSymbol ContainsCs =
            StringBinary(OperatorKind.ContainsCs);

        public static readonly OperatorSymbol NotContains =
            StringBinary(OperatorKind.NotContains);

        public static readonly OperatorSymbol NotContainsCs =
            StringBinary(OperatorKind.NotContainsCs);

        public static readonly OperatorSymbol StartsWith =
            StringBinary(OperatorKind.StartsWith);

        public static readonly OperatorSymbol StartsWithCs =
            StringBinary(OperatorKind.StartsWithCs);

        public static readonly OperatorSymbol NotStartsWith =
            StringBinary(OperatorKind.NotStartsWith);

        public static readonly OperatorSymbol NotStartsWithCs =
            StringBinary(OperatorKind.NotStartsWithCs);

        public static readonly OperatorSymbol EndsWith =
            StringBinary(OperatorKind.EndsWith);

        public static readonly OperatorSymbol EndsWithCs =
            StringBinary(OperatorKind.EndsWithCs);

        public static readonly OperatorSymbol NotEndsWith =
            StringBinary(OperatorKind.NotEndsWith);

        public static readonly OperatorSymbol NotEndsWithCs =
            StringBinary(OperatorKind.NotEndsWithCs);

        public static readonly OperatorSymbol MatchRegex =
            StringBinary(OperatorKind.MatchRegex);

        public static readonly OperatorSymbol Search =
            StringBinary(OperatorKind.Search);

        public static readonly OperatorSymbol In =
            new OperatorSymbol(OperatorKind.In,
                    new Signature(ScalarTypes.Bool, new Parameter("value", ScalarTypes.Bool), new Parameter("table", ParameterTypeKind.Tabular)).Hide(),
                    new Signature(ScalarTypes.Bool, new Parameter("value", ParameterTypeKind.NotBool), new Parameter("table", ParameterTypeKind.Tabular)),
                    new Signature(ScalarTypes.Bool, new Parameter("value", ScalarTypes.Bool), new Parameter("value", ParameterTypeKind.Scalar, maxOccurring: short.MaxValue)).Hide(), // hide bool in (bools)
                    new Signature(ScalarTypes.Bool, new Parameter("value", ParameterTypeKind.NotBool), new Parameter("value", ParameterTypeKind.Scalar, maxOccurring: short.MaxValue)));

        public static readonly OperatorSymbol HasAny =
            new OperatorSymbol(OperatorKind.HasAny,
                new Signature(ScalarTypes.Bool, new Parameter("value", StringOrDynamic), new Parameter("table", ParameterTypeKind.Tabular)),
                new Signature(ScalarTypes.Bool, new Parameter("value", StringOrDynamic), new Parameter("value", ParameterTypeKind.Scalar, maxOccurring: short.MaxValue)));

        public static readonly OperatorSymbol HasAll =
           new OperatorSymbol(OperatorKind.HasAll,
               new Signature(ScalarTypes.Bool, new Parameter("value", StringOrDynamic), new Parameter("table", ParameterTypeKind.Tabular)),
               new Signature(ScalarTypes.Bool, new Parameter("value", StringOrDynamic), new Parameter("value", ParameterTypeKind.Scalar, maxOccurring: short.MaxValue)));

        public static readonly OperatorSymbol InCs =
            new OperatorSymbol(OperatorKind.InCs,
                new Signature(ScalarTypes.Bool, new Parameter("value", StringOrDynamic), new Parameter("table", ParameterTypeKind.Tabular)),
                new Signature(ScalarTypes.Bool, new Parameter("value", StringOrDynamic), new Parameter("value", ParameterTypeKind.Scalar, maxOccurring: short.MaxValue)));

        public static readonly OperatorSymbol NotIn =
            new OperatorSymbol(OperatorKind.NotIn,
                new Signature(ScalarTypes.Bool, new Parameter("value", ScalarTypes.Bool), new Parameter("table", ParameterTypeKind.Tabular)).Hide(),
                new Signature(ScalarTypes.Bool, new Parameter("value", ParameterTypeKind.NotBool), new Parameter("table", ParameterTypeKind.Tabular)),
                new Signature(ScalarTypes.Bool, new Parameter("value", ScalarTypes.Bool), new Parameter("value", ParameterTypeKind.Scalar, maxOccurring: short.MaxValue)).Hide(), // hide bool in (bools)
                new Signature(ScalarTypes.Bool, new Parameter("value", ParameterTypeKind.NotBool), new Parameter("value", ParameterTypeKind.Scalar, maxOccurring: short.MaxValue)));

        public static readonly OperatorSymbol NotInCs =
            new OperatorSymbol(OperatorKind.NotInCs,
                new Signature(ScalarTypes.Bool, new Parameter("value", StringOrDynamic), new Parameter("table", ParameterTypeKind.Tabular)),
                new Signature(ScalarTypes.Bool, new Parameter("value", StringOrDynamic), new Parameter("value", ParameterTypeKind.Scalar, maxOccurring: short.MaxValue)));

        public static readonly OperatorSymbol Between =
            new OperatorSymbol(OperatorKind.Between,
                new Signature(ScalarTypes.Bool, new Parameter("value", ParameterTypeKind.Number), new Parameter("start", ParameterTypeKind.Number), new Parameter("end", ParameterTypeKind.Number)),
                new Signature(ScalarTypes.Bool, new Parameter("value", ParameterTypeKind.Summable), new Parameter("start", ParameterTypeKind.Parameter0), new Parameter("end", ParameterTypeKind.Parameter0)),
                new Signature(ScalarTypes.Bool, new Parameter("value", ScalarTypes.DateTime), new Parameter("start", ScalarTypes.DateTime), new Parameter("end", ScalarTypes.TimeSpan)),
                new Signature(ScalarTypes.Bool, new Parameter("value", ParameterTypeKind.Number), new Parameter("start", ScalarTypes.Dynamic), new Parameter("end", ParameterTypeKind.Parameter1)),
                new Signature(ScalarTypes.Bool, new Parameter("value", ScalarTypes.Dynamic), new Parameter("start", ParameterTypeKind.Number), new Parameter("end", ParameterTypeKind.Parameter1)));

        public static readonly OperatorSymbol NotBetween =
            new OperatorSymbol(OperatorKind.NotBetween,
                new Signature(ScalarTypes.Bool, new Parameter("value", ParameterTypeKind.Number), new Parameter("start", ParameterTypeKind.Number), new Parameter("end", ParameterTypeKind.Number)),
                new Signature(ScalarTypes.Bool, new Parameter("value", ParameterTypeKind.Summable), new Parameter("start", ParameterTypeKind.Parameter0), new Parameter("end", ParameterTypeKind.Parameter0)),
                new Signature(ScalarTypes.Bool, new Parameter("value", ScalarTypes.DateTime), new Parameter("start", ScalarTypes.DateTime), new Parameter("end", ScalarTypes.TimeSpan)),
                new Signature(ScalarTypes.Bool, new Parameter("value", ParameterTypeKind.Number), new Parameter("start", ScalarTypes.Dynamic), new Parameter("end", ParameterTypeKind.Parameter1)),
                new Signature(ScalarTypes.Bool, new Parameter("value", ScalarTypes.Dynamic), new Parameter("start", ParameterTypeKind.Number), new Parameter("end", ParameterTypeKind.Parameter1)));

        public static IReadOnlyList<OperatorSymbol> All { get; } = new OperatorSymbol[]
        {
            // unary
            UnaryMinus,
            UnaryPlus,

            // binary
            And,
            Or,
            Add,
            Subtract,
            Multiply,
            Divide,
            Modulo,
            LessThan,
            LessThanOrEqual,
            GreaterThan,
            GreaterThanOrEqual,
            Equal,
            NotEqual,

            // string binary operators
            EqualTilde,
            BangTilde,
            Has,
            HasCs,
            NotHas,
            NotHasCs,
            HasPrefix,
            HasPrefixCs,
            NotHasPrefix,
            NotHasPrefixCs,
            HasSuffix,
            HasSuffixCs,
            NotHasSuffix,
            NotHasSuffixCs,
            Like,
            LikeCs,
            NotLike,
            NotLikeCs,
            Contains,
            ContainsCs,
            NotContains,
            NotContainsCs,
            StartsWith,
            StartsWithCs,
            NotStartsWith,
            NotStartsWithCs,
            EndsWith,
            EndsWithCs,
            NotEndsWith,
            NotEndsWithCs,
            MatchRegex,
            Search,

            // N-ary operators
            In,
            InCs,
            NotIn,
            NotInCs,
            Between,
            NotBetween,
            HasAny,
            HasAll,
        };
    }
}
