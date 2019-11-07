using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{

    /// <summary>
    /// The kind of return type specified in a <see cref="Signature"/>
    /// </summary>
    public enum ReturnTypeKind
    {
        /// <summary>
        /// The return type is the type specified
        /// </summary>
        Declared,

        /// <summary>
        /// The return type is computed based on the body or declaration.
        /// </summary>
        Computed,

        /// <summary>
        /// The return type is determined by a custom function.
        /// </summary>
        Custom,

        /// <summary>
        /// The return type is the same as the type of the argument for parameter 0
        /// </summary>
        Parameter0,

        /// <summary>
        /// The return type is the same as the type of the argument for parameter 1
        /// </summary>
        Parameter1,

        /// <summary>
        /// The return type is the same as the type of the argument for parameter 2
        /// </summary>
        Parameter2,

        /// <summary>
        /// The return type is the same as the type of the argument for the last parameter
        /// </summary>
        ParameterN,

        /// <summary>
        /// The return the is the same as the type specified by the last parameter's argument's type literal
        /// </summary>
        ParameterNLiteral,

        /// <summary>
        /// The return type is the same as the type of the argument for parameter 0, but promoted (int -> long, decimal -> real)
        /// </summary>
        Parameter0Promoted,

        /// <summary>
        /// The return type is a cluster. If the argument for parameter 0 is a literal string, the named cluster is used.
        /// </summary>
        Parameter0Cluster,

        /// <summary>
        /// The return type is a database. If the argument for parameter 0 is a literal string, the named database is used.
        /// </summary>
        Parameter0Database,

        /// <summary>
        /// The return type is a table. If the argument for parameter 0 is a literal string, the named table is used.
        /// </summary>
        Parameter0Table,

        /// <summary>
        /// The return type is a table. If the argument for parameter 0 is a literal string, the named external table is used.
        /// </summary>
        Parameter0ExternalTable,

        /// <summary>
        /// The return type is the common scalar type of all the parameters marked as Common.
        /// </summary>
        Common,

        /// <summary>
        /// The return type is the widest numeric scalar type of all the arguments.
        /// </summary>
        Widest,

        /// <summary>
        /// The return type is a materialized view. If the argument for parameter 0 is a literal string, the named materialized view is used.
        /// </summary>
        Parameter0MaterializedView,
    }
}