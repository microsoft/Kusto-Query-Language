using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kusto.Language.Symbols
{
    public enum Tabularity
    {
        /// <summary>
        /// The symbol is not scalar or tabular; void and error.
        /// </summary>
        None,

        /// <summary>
        /// The symbol is scalar; scalar types, columns, tuples and some functions.
        /// </summary>
        Scalar,

        /// <summary>
        /// The symbol is tabular or related; tables, databases, clusters, patterns and some functions.
        /// </summary>
        Tabular,

        /// <summary>
        /// The tabularity is not known.
        /// </summary>
        Unknown,

        /// <summary>
        /// The tabularity was unspecified and should be determined based on other state.
        /// </summary>
        Unspecified
    }
}