using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kusto.Language.Symbols
{
    public enum Tabularity
    {
        /// <summary>
        /// The symbol is not scalar or tabular; void, error
        /// </summary>
        None,

        /// <summary>
        /// The symbol is scalar; scalar types, columns, tuples and some functions.
        /// </summary>
        Scalar,

        /// <summary>
        /// The symbol is tabular or related; tables, some functions and patterns.
        /// </summary>
        Tabular,

        /// <summary>
        /// Not scalar or tabular entity; cluster, database, entity_group, graph, etc.
        /// </summary>
        Other,

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