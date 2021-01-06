using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    /// <summary>
    /// The kind of operator for a <see cref="OperatorSymbol"/>
    /// </summary>
    public enum OperatorKind
    {
        None, // not an operator

        // unary operators
        UnaryMinus, // negate
        UnaryPlus,

        // binary operators
        Add,
        Subtract,
        Multiply,
        Divide,
        Modulo,

        // string binary operators
        EqualTilde, // TODO: need better name
        BangTilde,  // TODO: need better name
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

        // relational (equalities, inequalities...)
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
        Equal,
        NotEqual,
        And,
        Or,

        In,
        InCs,
        NotIn,
        NotInCs,
        Between,
        NotBetween,
        HasAny,
        HasAll,
    }
}