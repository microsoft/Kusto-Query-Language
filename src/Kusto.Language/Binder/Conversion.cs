using System;

namespace Kusto.Language.Binding
{
    /// <summary>
    /// The kinds of conversions allowed between values of two different types.
    /// </summary>
    internal enum Conversion
    {
        /// <summary>
        /// No conversion allowed between different scalar types (strict)
        /// </summary>
        None,

        /// <summary>
        /// Type promotion (widening) allowed.
        /// </summary>
        Promotable,

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