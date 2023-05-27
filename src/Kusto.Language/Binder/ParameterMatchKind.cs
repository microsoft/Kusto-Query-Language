using System;

namespace Kusto.Language.Binding
{
    /// <summary>
    /// The kind of match that an argument can have with its corresponding signature parameter.
    /// </summary>
    internal enum ParameterMatchKind
    {
        // These are in order of which is better.. a better match is one that is more specific.

        /// <summary>
        /// There is no match between the argument and the parameter.
        /// </summary>
        None,

        /// <summary>
        /// The argument had an unknown type.
        /// </summary>
        Unknown,

        /// <summary>
        /// The argument is converted to dynamic.
        /// </summary>
        Dynamic,

        /// <summary>
        /// The argument is narrowed to the compatible parameter type.
        /// </summary>
        Compatible,

        /// <summary>
        /// The argument is widened to the parameter type
        /// </summary>
        Promoted,  

        /// <summary>
        /// The argument's type is not the excluded type
        /// </summary>
        NotType,

        /// <summary>
        /// The argument's type is one of many possible types.
        /// </summary>
        OneOfMany,  

        /// <summary>
        /// The argument's type is a scalar type
        /// </summary>
        Scalar,

        /// <summary>
        /// The argument's type is a summable scalar type
        /// </summary>
        Summable,

        /// <summary>
        /// The argument's type is an orderable scalar type
        /// </summary>
        Orderable,

        /// <summary>
        /// The argumet's type is a number
        /// </summary>
        Number,

        /// <summary>
        /// The argument's type is an integer
        /// </summary>
        Integer,

        /// <summary>
        /// The argument's type is tabular.
        /// </summary>
        Tabular,

        /// <summary>
        /// The argument's type is a table.
        /// </summary>
        Table,

        /// <summary>
        /// The argument's type is a database
        /// </summary>
        Database,

        /// <summary>
        /// The argument's type is a cluster
        /// </summary>
        Cluster,

        /// <summary>
        /// The argument's type is an exact match for the parameter type
        /// </summary>
        Exact
    }
}
