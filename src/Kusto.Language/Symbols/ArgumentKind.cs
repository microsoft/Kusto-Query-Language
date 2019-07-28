using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    /// <summary>
    /// The argument kind expected for a <see cref="Parameter"/>
    /// </summary>
    public enum ArgumentKind
    {
        /// <summary>
        /// The argument may be any expression
        /// </summary>
        Expression,

        /// <summary>
        /// The argument must be the star expression
        /// </summary>
        Star,

        /// <summary>
        /// The argument must be an aggregate expression
        /// </summary>
        Aggregate,

        /// <summary>
        /// The argument must be a column reference
        /// </summary>
        Column,

        /// <summary>
        /// The argument must be a literal expression.
        /// </summary>
        Literal,

        /// <summary>
        /// The argument must be a non-empty literal expression.
        /// </summary>
        LiteralNotEmpty,

        /// <summary>
        /// The argument must be a constant expression.
        /// </summary>
        Constant,
    }
}