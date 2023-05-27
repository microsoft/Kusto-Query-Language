using System;

namespace Kusto.Language.Symbols
{
    /// <summary>
    /// The kinds of conversions allowed between values of two different types.
    /// </summary>
    public enum Conversion
    {
        // the enum values are in order of subsumption, a greater value
        // subsumes all other choices (exception None)

        /// <summary>
        /// No conversion allowed between different scalar types (strict)
        /// </summary>
        None,

        /// <summary>
        /// Type promotion (widening) allowed.
        /// </summary>
        Promotable,

        /// <summary>
        /// Conversions to dynamic allowed.
        /// </summary>
        Dynamic,

        /// <summary>
        /// Conversions between compatible types allowed (widening or narrowing)
        /// </summary>
        Compatible,

        /// <summary>
        /// All conversions allowed (no checking)
        /// </summary>
        Any
    }
}