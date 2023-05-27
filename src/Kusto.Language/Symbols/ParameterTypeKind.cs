using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    /// <summary>
    /// The kind of parameter type constraint specified in a <see cref="Parameter"/>
    /// </summary>
    public enum ParameterTypeKind
    {
        /// <summary>
        /// The parameter type can be any type
        /// </summary>
        Any,

        /// <summary>
        /// The parameter type is the type specified.
        /// </summary>
        Declared,

        /// <summary>
        /// Any scalar value (non-tabular)
        /// </summary>
        Scalar,

        /// <summary>
        /// Any tabular value (non-scalar)
        /// </summary>
        Tabular,

        /// <summary>
        /// Any database
        /// </summary>
        Database,

        /// <summary>
        /// Any cluster
        /// </summary>
        Cluster,

        /// <summary>
        /// Any scalar integer type (int, long)
        /// </summary>
        Integer,

        /// <summary>
        /// Either real or decimal
        /// </summary>
        RealOrDecimal,

        /// <summary>
        /// Either a string or a dynamic (string) value
        /// </summary>
        StringOrDynamic,

        /// <summary>
        /// Either a string or an array of strings
        /// </summary>
        StringOrArray,

        /// <summary>
        /// Either an integer or dynamic array of integers.
        /// </summary>
        IntegerOrArray,

        /// <summary>
        /// The parameter is any dynamic array type (or dynamic)
        /// </summary>
        DynamicArray,

        /// <summary>
        /// The parameter is any dynamic bag type (or dynamic)
        /// </summary>
        DynamicBag,

        /// <summary>
        /// Any scalar numeric type (int, long, real, decimal)
        /// </summary>
        Number,

        /// <summary>
        /// Any scalar numeric type (int, long, real, decimal) or bool
        /// </summary>
        NumberOrBool,

        /// <summary>
        /// Any scalar type that is summable (number, timespan, datetime)
        /// </summary>
        Summable,

        /// <summary>
        /// Any scalar type that is orderable (number, timespan, datetime, string, bool)
        /// </summary>
        Orderable,

        /// <summary>
        /// Any scalar type, except Real
        /// </summary>
        NotRealOrBool,

        /// <summary>
        /// Any scalar type, except Dynamic
        /// </summary>
        NotDynamic,

        /// <summary>
        /// Any scalar type, except Bool
        /// </summary>
        NotBool,

        /// <summary>
        /// The argument type must be the same type as the type of the argument for parameter 0
        /// </summary>
        Parameter0,

        /// <summary>
        /// The argument type must be the same type as the type of the  argument for parameter 1
        /// </summary>
        Parameter1,

        /// <summary>
        /// The argument type must be the same type as the type of the argument for parameter 2
        /// </summary>
        Parameter2,

        /// <summary>
        /// The argument type must be promotable to the common scalar type of all the parameters marked CommonXXX,
        /// but not be dynamic.
        /// </summary>
        CommonScalar,

        /// <summary>
        /// The argument type must be promotable to the common numeric type of all the parameters marked CommonXXX,
        /// but not dynamic.
        /// </summary>
        CommonNumber,

        /// <summary>
        /// The argument type must be promotable to the common summable type of all the parameters marked CommonXXX,
        /// but not dynamic.
        /// </summary>
        CommonSummable,

        /// <summary>
        /// The argument type must be promotable to the common orderable type of all the parameters marked CommonXXX,
        /// but not dynamic.
        /// </summary>
        CommonOrderable,

        /// <summary>
        /// The argument type must be promotable to the common scalar type of all the parameters marked CommonXXX, or be dynamic.
        /// </summary>
        CommonScalarOrDynamic,
    }
}