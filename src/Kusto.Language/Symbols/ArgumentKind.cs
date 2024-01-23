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
        /// The argument may be any expression, but the row scope is derived from parameter0 element type.
        /// </summary>
        Expression_Parameter0_Element,

        /// <summary>
        /// The argument must be the star expression
        /// </summary>
        StarOnly,

        /// <summary>
        /// The argument may be the star expression
        /// </summary>
        StarAllowed,

        /// <summary>
        /// The argument must be an aggregate expression
        /// </summary>
        Aggregate,

        /// <summary>
        /// The argument must be a column reference 
        /// </summary>
        Column,

        /// <summary>
        /// The argument must be a column reference of a table from parameter0
        /// </summary>
        Column_Parameter0,

        /// <summary>
        /// The argument must be a column reference that is common between the row context and the table from parameter0.
        /// </summary>
        Column_Parameter0_Common,

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