using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    using Utils;

    public enum SymbolKind
    {
        /// <summary>
        /// Not a symbol
        /// </summary>
        None = 0,

        /// <summary>
        /// Scalar type
        /// </summary>
        Scalar,

        /// <summary>
        /// Declared local variable
        /// </summary>
        Variable,

        /// <summary>
        /// Function (local, external or built-in)
        /// </summary>
        Function,

        /// <summary>
        /// Function parameter
        /// </summary>
        Parameter,

        /// <summary>
        /// Pattern
        /// </summary>
        Pattern,

        /// <summary>
        /// A tuple type (has multiple named values, not scalar or tabular)
        /// </summary>
        Tuple,

        /// <summary>
        /// Column (of table or tuple)
        /// </summary>
        Column,

        /// <summary>
        /// A table (has columns)
        /// </summary>
        Table,

        /// <summary>
        /// A database (contains tables, functions, etc)
        /// </summary>
        Database,

        /// <summary>
        /// A cluster (contains databases)
        /// </summary>
        Cluster,

        /// <summary>
        /// Built-in language operator  (+, ==, etc)
        /// </summary>
        Operator,

        /// <summary>
        /// A symbol corresponding to a group of symbols
        /// </summary>
        Group,

        /// <summary>
        /// The void type (when there is no type)
        /// </summary>
        Void,

        /// <summary>
        /// The error type (when a type is unknown due to an error)
        /// </summary>
        Error,

        /// <summary>
        /// Command statements
        /// </summary>
        Command,

        /// <summary>
        /// Materialized view
        /// </summary>
        MaterializedView,

        /// <summary>
        /// A query option (assigned via set statement)
        /// </summary>
        Option,

        /// <summary>
        /// A query operator parameter
        /// </summary>
        QueryOperatorParameter,
    }
}