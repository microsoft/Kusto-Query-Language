using System;
using System.Collections.Generic;

namespace Kusto.Language.Symbols
{
    public static class ScalarTypes
    {
        /// <summary>
        /// The dynamic type.
        /// </summary>
        public static readonly ScalarSymbol Dynamic = 
            DynamicAnySymbol.Instance;

        /// <summary>
        /// The bool type.
        /// </summary>
        public static readonly ScalarSymbol Bool = 
            new PrimitiveSymbol("bool", new[] { "boolean" }, ScalarFlags.Orderable);

        /// <summary>
        /// The int type.
        /// </summary>
        public static readonly ScalarSymbol Int = 
            new PrimitiveSymbol("int", new[] { "int32", "uint", "uint32", "int8", "uint8", "int16", "uint16" }, ScalarFlags.Integer | ScalarFlags.Numeric | ScalarFlags.Interval | ScalarFlags.Summable | ScalarFlags.Orderable);

        /// <summary>
        /// The long type.
        /// </summary>
        public static readonly ScalarSymbol Long = 
            new PrimitiveSymbol("long", new[] { "int64", "ulong", "uint64" }, ScalarFlags.Integer | ScalarFlags.Numeric | ScalarFlags.Interval | ScalarFlags.Summable | ScalarFlags.Orderable, new[] { Int });

        /// <summary>
        /// The real type.
        /// </summary>
        public static readonly ScalarSymbol Real = 
            new PrimitiveSymbol("real", new[] { "double", "float" }, ScalarFlags.Numeric | ScalarFlags.Interval | ScalarFlags.Summable | ScalarFlags.Orderable, new[] { Int, Long });

        /// <summary>
        /// The decimal type.
        /// </summary>
        public static readonly ScalarSymbol Decimal = 
            new PrimitiveSymbol("decimal", null, ScalarFlags.Numeric | ScalarFlags.Interval | ScalarFlags.Summable | ScalarFlags.Orderable, new[] { Int, Long, Real });

        /// <summary>
        /// The datetime type.
        /// </summary>
        public static readonly ScalarSymbol DateTime = 
            new PrimitiveSymbol("datetime", new[] { "date" }, ScalarFlags.Interval | ScalarFlags.Summable | ScalarFlags.Orderable);

        /// <summary>
        /// The timespan type.
        /// </summary>
        public static readonly ScalarSymbol TimeSpan = 
            new PrimitiveSymbol("timespan", new[] { "time" }, ScalarFlags.Interval | ScalarFlags.Summable | ScalarFlags.Orderable);

        /// <summary>
        /// The guid type.
        /// </summary>
        public static readonly ScalarSymbol Guid = 
            new PrimitiveSymbol("guid", new[] { "uuid", "uniqueid" });

        /// <summary>
        /// The type of a type literal.
        /// </summary>
        public static readonly ScalarSymbol Type = 
            new PrimitiveSymbol("type");

        /// <summary>
        /// The string type.
        /// </summary>
        public static readonly ScalarSymbol String = 
            new PrimitiveSymbol("string", null, ScalarFlags.Orderable, widerThan: new[] { Dynamic });

        /// <summary>
        /// The type used for null in dynamic expressions.
        /// </summary>
        public static readonly ScalarSymbol Null = 
            new PrimitiveSymbol("null", null, ScalarFlags.None);

        /// <summary>
        /// The type used when the type is unknown.
        /// </summary>
        public static readonly ScalarSymbol Unknown = 
            new PrimitiveSymbol("unknown", null, ScalarFlags.All);

        /// <summary>
        /// A dynamic type that is known to contain a boolean.
        /// </summary>
        public static readonly ScalarSymbol DynamicBool = 
            new DynamicPrimitiveSymbol(ScalarTypes.Bool);

        /// <summary>
        /// A dynamic type that is known to contain a long.
        /// </summary>
        public static readonly ScalarSymbol DynamicLong = 
            new DynamicPrimitiveSymbol(ScalarTypes.Long);

        /// <summary>
        /// A dynamic type that is known to contail a real.
        /// </summary>
        public static readonly ScalarSymbol DynamicReal = 
            new DynamicPrimitiveSymbol(ScalarTypes.Real);

        /// <summary>
        /// A dynamic type that is known to contain a datetime.
        /// </summary>
        public static readonly ScalarSymbol DynamicDateTime = 
            new DynamicPrimitiveSymbol(ScalarTypes.DateTime);

        /// <summary>
        /// A dynamic type that is known to contain a timespan.
        /// </summary>
        public static readonly ScalarSymbol DynamicTimeSpan = 
            new DynamicPrimitiveSymbol(ScalarTypes.TimeSpan);

        /// <summary>
        /// A dynamic type that is known to contain a guid.
        /// </summary>
        public static readonly ScalarSymbol DynamicGuid = 
            new DynamicPrimitiveSymbol(ScalarTypes.Guid);

        /// <summary>
        /// A dynamic type that is known to contain a string.
        /// </summary>
        public static readonly ScalarSymbol DynamicString = 
            new DynamicPrimitiveSymbol(ScalarTypes.String);

        /// <summary>
        /// A dynamic type that is known to contain a bag of properties (JSON object).
        /// </summary>
        public static readonly DynamicBagSymbol DynamicBag = 
            DynamicBagSymbol.Empty;

        /// <summary>
        /// A dynamic type that is known to contain an array.
        /// </summary>
        public static readonly DynamicArraySymbol DynamicArray = 
            new DynamicArraySymbol(ScalarTypes.Dynamic);

        /// <summary>
        /// A dynamic type that is known to contain an array of bool.
        /// </summary>
        public static readonly DynamicArraySymbol DynamicArrayOfBool = 
            new DynamicArraySymbol(ScalarTypes.Bool);

        /// <summary>
        /// A dynamic type that is known to contain an array of long.
        /// </summary>
        public static readonly DynamicArraySymbol DynamicArrayOfLong = 
            new DynamicArraySymbol(ScalarTypes.Long);

        /// <summary>
        /// A dynamic type that is known to contain an array of real.
        /// </summary>
        public static readonly DynamicArraySymbol DynamicArrayOfReal = 
            new DynamicArraySymbol(ScalarTypes.Real);

        /// <summary>
        /// A dynamic type that is known to contain an array of datetime.
        /// </summary>
        public static readonly DynamicArraySymbol DynamicArrayOfDateTime = 
            new DynamicArraySymbol(ScalarTypes.DateTime);

        /// <summary>
        /// A dynamic type that is known to contain an array of timespan.
        /// </summary>
        public static readonly DynamicArraySymbol DynamicArrayOfTimeSpan = 
            new DynamicArraySymbol(ScalarTypes.TimeSpan);

        /// <summary>
        /// A dynamic type that is known to contain an array of guid.
        /// </summary>
        public static readonly DynamicArraySymbol DynamicArrayOfGuid = 
            new DynamicArraySymbol(ScalarTypes.Guid);

        /// <summary>
        /// A dynamic type that is known to contain an array of string.
        /// </summary>
        public static readonly DynamicArraySymbol DynamicArrayOfString = 
            new DynamicArraySymbol(ScalarTypes.String);

        /// <summary>
        /// A dynamic type that is known to contain an array of arrays.
        /// </summary>
        public static readonly DynamicArraySymbol DynamicArrayOfArray = 
            new DynamicArraySymbol(ScalarTypes.DynamicArray);

        /// <summary>
        /// A dynamic type that is known to contain an array of property bags (JSON objects).
        /// </summary>
        public static readonly DynamicArraySymbol DynamicArrayOfBag = 
            new DynamicArraySymbol(ScalarTypes.DynamicBag);

        /// <summary>
        /// A dynamic type that is known to contain an array of arrays of real.
        /// </summary>
        public static readonly DynamicArraySymbol DynamicArrayOfArrayOfReal = 
            new DynamicArraySymbol(DynamicArrayOfReal);

        /// <summary>
        /// A dynamic type that is known to contain an array of arrays of string.
        /// </summary>
        public static readonly DynamicArraySymbol DynamicArrayOfArrayOfString = 
            new DynamicArraySymbol(DynamicArrayOfString);

        /// <summary>
        /// The general shape of a geometry returned by a geometric function.
        /// </summary>
        public static readonly DynamicBagSymbol GeoShape = 
            new DynamicBagSymbol(
                new ColumnSymbol("type", ScalarTypes.String),
                new ColumnSymbol("coordinates", ScalarTypes.DynamicArray));

        /// <summary>
        /// Get the dynamic type for the specified underlying type.
        /// </summary>
        public static TypeSymbol GetDynamic(TypeSymbol underlyingType)
        {
            if (underlyingType == null
                || underlyingType == ScalarTypes.Dynamic
                || underlyingType == ScalarTypes.Null
                || underlyingType == ScalarTypes.Unknown)
                return ScalarTypes.Dynamic;
            else if (underlyingType is DynamicSymbol)
                return underlyingType;
            else if (underlyingType == ScalarTypes.Bool)
                return DynamicBool;
            else if (underlyingType == ScalarTypes.Int)
                return DynamicLong;
            else if (underlyingType == ScalarTypes.Long)
                return DynamicLong;
            else if (underlyingType == ScalarTypes.Real)
                return DynamicReal;
            else if (underlyingType == ScalarTypes.DateTime)
                return DynamicDateTime;
            else if (underlyingType == ScalarTypes.TimeSpan)
                return DynamicTimeSpan;
            else if (underlyingType == ScalarTypes.Guid)
                return DynamicGuid;
            else if (underlyingType == ScalarTypes.String)
                return DynamicString;
            else
                return Dynamic;
        }

        /// <summary>
        /// Returns the <see cref="DynamicArraySymbol"/> for a dynamic array of the specified element type.
        /// </summary>
        public static DynamicArraySymbol GetDynamicArray(TypeSymbol elementType)
        {
            // dont make arrays of dynamic primivtes
            if (elementType is DynamicPrimitiveSymbol dp)
                elementType = dp.UnderlyingType;

            if (elementType == null)
                return DynamicArray;
            else if (elementType == ScalarTypes.Bool)
                return DynamicArrayOfBool;
            else if (elementType == ScalarTypes.Int)
                return DynamicArrayOfLong;
            else if (elementType == ScalarTypes.Long)
                return DynamicArrayOfLong;
            else if (elementType == ScalarTypes.Real)
                return DynamicArrayOfReal;
            else if (elementType == ScalarTypes.DateTime)
                return DynamicArrayOfDateTime;
            else if (elementType == ScalarTypes.TimeSpan)
                return DynamicArrayOfTimeSpan;
            else if (elementType == ScalarTypes.Guid)
                return DynamicArrayOfGuid;
            else if (elementType == ScalarTypes.Dynamic)
                return DynamicArray;
            else if (elementType == ScalarTypes.String)
                return DynamicArrayOfString;
            else if (elementType == ScalarTypes.DynamicArray)
                return DynamicArrayOfArray;
            else if (elementType == ScalarTypes.DynamicBag)
                return DynamicArrayOfBag;
            else if (elementType == ScalarTypes.Null)
                return DynamicArray;
            else if (elementType == ScalarTypes.Unknown)
                return DynamicArray;
            else if (elementType == ScalarTypes.DynamicArrayOfReal)
                return DynamicArrayOfArrayOfReal;
            else if (elementType == ScalarTypes.DynamicArrayOfString)
                return DynamicArrayOfArrayOfString;
            else if (elementType is DynamicArraySymbol
                || elementType is DynamicBagSymbol)
                return new DynamicArraySymbol(elementType);
            else
                return DynamicArray;
        }

        /// <summary>
        /// Gets the <see cref="DynamicBagSymbol"/> with the specified properties.
        /// </summary>
        public static DynamicBagSymbol GetDynamicBag(
            IReadOnlyList<ColumnSymbol> properties)
        {
            if (properties == null || properties.Count == 0)
            {
                return DynamicBag;
            }
            else
            {
                return new DynamicBagSymbol(properties);
            }
        }

        /// <summary>
        /// Gets the <see cref="DynamicBagSymbol"/> with the specified properties.
        /// </summary>
        public static DynamicBagSymbol GetDynamicBag(
            params ColumnSymbol[] properties) =>
            GetDynamicBag((IReadOnlyList<ColumnSymbol>)properties);

        /// <summary>
        /// Gets the <see cref="DynamicBagSymbol"/> with the specified properties.
        /// </summary>
        /// <param name="schema">One or more properties specified as: (name: type, ...)</param>
        public static DynamicBagSymbol GetDynamicBag(string schema) =>
            GetDynamicBag(TableSymbol.From(schema).Columns);

        /// <summary>
        /// Gets the <see cref="TupleSymbol"/> with the specified columns.
        /// </summary>
        public static TupleSymbol GetTuple(IReadOnlyList<ColumnSymbol> columns) =>
            new TupleSymbol(columns);

        /// <summary>
        /// Gets the <see cref="TupleSymbol"/> with the specified columns.
        /// </summary>
        public static TupleSymbol GetTuple(params ColumnSymbol[] columns) =>
            new TupleSymbol(columns);

        /// <summary>
        /// Gets the <see cref="TupleSymbol"/> with the specified columns.
        /// </summary>
        /// <param name="schema">One or more columns specified as: (name: type, ...)</param>
        public static TupleSymbol GetTuple(string schema) =>
            GetTuple(TableSymbol.From(schema).Columns);

        /// <summary>
        /// All known scalar symbols
        /// </summary>
        public static readonly IReadOnlyList<ScalarSymbol> All = new ScalarSymbol[]
        {
            Bool,
            Int,
            Long,
            Real,
            Decimal,
            String,
            DateTime,
            TimeSpan,
            Guid,
            Type,
            Dynamic,
            Null
        };

        private static readonly Dictionary<string, ScalarSymbol> s_typeMap
            = new Dictionary<string, ScalarSymbol>();

        static ScalarTypes()
        {
            foreach (var type in All)
            {
                s_typeMap.Add(type.Name, type);

                foreach (var alias in type.Aliases)
                {
                    s_typeMap.Add(alias, type);
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="ScalarSymbol"/> associated with the specified type name.
        /// </summary>
        public static ScalarSymbol GetSymbol(string typeName)
        {
            ScalarSymbol type;
            s_typeMap.TryGetValue(typeName, out type);
            return type;
        }
    }
}