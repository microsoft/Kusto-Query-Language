using System;
using System.Collections.Generic;

namespace Kusto.Language.Symbols
{
    public static class ScalarTypes
    {
        public static readonly ScalarSymbol Bool = new ScalarSymbol("bool", new[] { "boolean" }, ScalarFlags.Orderable);
        public static readonly ScalarSymbol Int = new ScalarSymbol("int", new[] { "int32", "uint", "uint32", "int8", "uint8", "int16", "uint16" }, ScalarFlags.Integer | ScalarFlags.Numeric | ScalarFlags.Interval | ScalarFlags.Summable | ScalarFlags.Orderable );
        public static readonly ScalarSymbol Long = new ScalarSymbol("long", new[] { "int64", "ulong", "uint64" }, ScalarFlags.Integer | ScalarFlags.Numeric | ScalarFlags.Interval | ScalarFlags.Summable | ScalarFlags.Orderable, new[] { Int });
        public static readonly ScalarSymbol Real = new ScalarSymbol("real", new[] { "double", "float", "single" }, ScalarFlags.Numeric | ScalarFlags.Interval | ScalarFlags.Summable | ScalarFlags.Orderable, new[] { Int, Long });
        public static readonly ScalarSymbol Decimal = new ScalarSymbol("decimal", null, ScalarFlags.Numeric | ScalarFlags.Interval | ScalarFlags.Summable | ScalarFlags.Orderable, new[] { Int, Long, Real });
        public static readonly ScalarSymbol DateTime = new ScalarSymbol("datetime", new[] { "date" }, ScalarFlags.Interval | ScalarFlags.Summable | ScalarFlags.Orderable);
        public static readonly ScalarSymbol TimeSpan = new ScalarSymbol("timespan", new[] { "time" }, ScalarFlags.Interval | ScalarFlags.Summable | ScalarFlags.Orderable);
        public static readonly ScalarSymbol Guid = new ScalarSymbol("guid", new[] { "uuid", "uniqueid" });
        public static readonly ScalarSymbol Type = new ScalarSymbol("type");
        public static readonly ScalarSymbol Dynamic = new ScalarSymbol("dynamic", null, ScalarFlags.Interval);
        public static readonly ScalarSymbol String = new ScalarSymbol("string", null, ScalarFlags.Orderable, widerThan: new[] { Dynamic });

        /// <summary>
        /// This is not actually a scalar type, but is used when a column has an unknown type.
        /// </summary>
        public static readonly ScalarSymbol Unknown = new ScalarSymbol("unknown", null, ScalarFlags.All);

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
            Dynamic
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

        public static ScalarSymbol GetSymbol(string typeName)
        {
            ScalarSymbol type;
            s_typeMap.TryGetValue(typeName, out type);
            return type;
        }
    }
}