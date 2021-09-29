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
        /// The argument's type is not the excluded type
        /// </summary>
        NotType,

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
        /// The argument type is compatible with the parameter type
        /// </summary>
        Compatible,

        /// <summary>
        /// The arguments type can be promoted to the parameter type
        /// </summary>
        Promoted,  // smaller set than all numbers

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
        /// The argument's type is one of two possible parameter types
        /// </summary>
        OneOfTwo,  // one of two explicit types?

        /// <summary>
        /// The argument's type is an exact match for the parameter type
        /// </summary>
        Exact
    }
}
