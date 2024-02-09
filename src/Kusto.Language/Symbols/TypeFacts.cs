using System;
using System.Collections.Generic;

namespace Kusto.Language.Symbols
{
    using Syntax;
    using Utils;

    public static class TypeFacts
    {
        /// <summary>
        /// Gets the element type of an array or null if the type is not an array or dynamic.
        /// </summary>
        public static TypeSymbol GetElementType(TypeSymbol type)
        {
            if (type is DynamicArraySymbol array)
            {
                return ScalarTypes.GetDynamic(array.ElementType);
            }
            else if (type == ScalarTypes.Dynamic)
            {
                return ScalarTypes.Dynamic;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the widest numeric type from the set of types
        /// The widest type is the one that can contain the values of all the other types:
        /// </summary>
        public static ScalarSymbol GetWidestScalarType(params TypeSymbol[] scalarTypes) =>
            GetWidestScalarType((IReadOnlyList<TypeSymbol>)scalarTypes);

        /// <summary>
        /// Returns the widest scalar type from the set of types, or null if there are no scalar types.
        /// The widest type is the one that can contain the values of all the other types:
        /// </summary>
        public static ScalarSymbol GetWidestScalarType(IReadOnlyList<TypeSymbol> scalarTypes)
        {
            ScalarSymbol widestType = null;

            if (scalarTypes != null)
            {
                for (int i = 0; i < scalarTypes.Count; i++)
                {
                    var type = scalarTypes[i];

                    if (type is ScalarSymbol s && s.IsNumeric && s != widestType)
                    {
                        if (widestType == null || s.IsWiderThan(widestType))
                        {
                            widestType = s;
                        }
                    }
                }
            }

            return widestType;
        }

        /// <summary>
        /// Gets the common scalar type amongst a set of types.
        /// </summary>
        public static TypeSymbol GetCommonType(
            IReadOnlyList<TypeSymbol> types,
            Func<TypeSymbol, bool> fnInclude = null,
            Conversion allowedConversion = Conversion.Promotable,
            TypeSymbol defaultType = null)
        {
            TypeSymbol commonType = null;

            if (types != null)
            {
                foreach (var type in types)
                {
                    if (type != null && (fnInclude == null || fnInclude(type)))
                    {
                        if (!TryGetCommonType(commonType, type, allowedConversion, out commonType))
                            return defaultType;
                    }
                }
            }

            return commonType ?? defaultType;
        }

        /// <summary>
        /// Gets the common scalar type amongst a set of types.
        /// </summary>
        public static TypeSymbol GetCommonScalarType(
            IReadOnlyList<TypeSymbol> types,
            Conversion allowedConversions = Conversion.Promotable)
        {
            return GetCommonType(types, t => t.IsScalar, allowedConversions);
        }

        /// <summary>
        /// Gets the common scalar type amongst a set of types.
        /// </summary>
        public static TypeSymbol GetCommonScalarType(params TypeSymbol[] types)
        {
            return GetCommonScalarType((IReadOnlyList<TypeSymbol>)types);
        }

        /// <summary>
        /// Gets the common scalar result type amongst a set of expressions.
        /// </summary>
        public static TypeSymbol GetCommonResultType(
            IReadOnlyList<Expression> expressions,
            Conversion allowedConversions = Conversion.Promotable,
            TypeSymbol defaultType = null)
        {
            TypeSymbol commonType = null;

            if (expressions != null)
            {
                foreach (var expr in expressions)
                {
                    if (expr != null)
                    {
                        if (!TryGetCommonType(commonType, expr.ResultType, allowedConversions, out commonType))
                            return defaultType;
                    }
                }
            }

            return commonType ?? defaultType;
        }

        /// <summary>
        /// Gets the common scalar result type amongst a set of expressions.
        /// </summary>
        public static TypeSymbol GetCommonResultType(params Expression[] expressions) =>
            GetCommonResultType((IReadOnlyList<Expression>)expressions);

        /// <summary>
        /// Gets the common scalar result type amongst a set of expressions.
        /// </summary>
        public static TypeSymbol GetCommonResultType(
            SyntaxList<SeparatedElement<Expression>> expressions,
            Conversion allowedConversions = Conversion.Promotable,
            TypeSymbol defaultType = null,
            bool ignoreDynamic = false)
        {
            TypeSymbol commonType = null;

            if (expressions != null)
            {
                for (int i = 0; i < expressions.Count; i++)
                {
                    var expr = expressions[i].Element;
                    if (ignoreDynamic && expr.ResultType is DynamicSymbol)
                        continue;
                    if (!TryGetCommonType(commonType, expr.ResultType, allowedConversions, out commonType))
                        return defaultType;
                }
            }

            if (commonType == null && ignoreDynamic)
                return GetCommonResultType(expressions, allowedConversions, defaultType, false);

            return commonType ?? defaultType;
        }

        /// <summary>
        /// Gets the common type amongst a set of columns.
        /// </summary>
        public static TypeSymbol GetCommonColumnType(
            IReadOnlyList<ColumnSymbol> columns,
            Conversion allowedConversions = Conversion.Promotable,
            TypeSymbol defaultType = null)
        {
            TypeSymbol commonType = null;

            if (columns != null)
            {
                foreach (var col in columns)
                {
                    if (!TryGetCommonType(commonType, col.Type, allowedConversions, out commonType))
                        return defaultType;
                }
            }

            return commonType ?? defaultType;
        }

        /// <summary>
        /// Gets the type that is wider/more-general for each specified type.
        /// </summary>
        private static bool TryGetCommonType(
            TypeSymbol typeA, 
            TypeSymbol typeB, 
            Conversion allowedConversions,
            out TypeSymbol commonType)
        {
            if (typeA == null && typeB == null)
            {
                commonType = null;
                return false;
            }
            else if (typeB == null)
            {
                commonType = typeA;
                return true;
            }
            else if (typeA == null)
            {
                commonType = typeB;
                return true;
            }
            else if (typeA == typeB)
            {
                commonType = typeA;
                return true;
            }
            else if (typeA == ScalarTypes.Null)
            {
                commonType = typeB;
                return true;
            }
            else if (typeB == ScalarTypes.Null)
            {
                commonType = typeA;
                return true;
            }
            else if (typeA == ScalarTypes.Unknown
                || typeB == ScalarTypes.Unknown)
            {
                commonType = ScalarTypes.Unknown;
                return true;
            }
            else if (typeA == ScalarTypes.Dynamic
                || typeB == ScalarTypes.Dynamic)
            {
                commonType = ScalarTypes.Dynamic;
                return true;
            }
            else if (typeA.IsPromotableTo(typeB)
                && IsConversionAllowed(Conversion.Promotable, allowedConversions))
            {
                commonType = typeB;
                return true;
            }
            else if (typeB.IsPromotableTo(typeA)
                && IsConversionAllowed(Conversion.Promotable, allowedConversions))
            {
                commonType = typeA;
                return true;
            }
            else if (typeA is DynamicArraySymbol arrayA && typeB is DynamicArraySymbol arrayB)
            {
                commonType = (arrayA.ElementType == arrayB.ElementType)
                    ? arrayA.ElementType
                    : ScalarTypes.DynamicArray;
                return true;
            }
            else if (typeA is DynamicBagSymbol bagA && typeB is DynamicBagSymbol bagB)
            {
                commonType = ScalarTypes.GetDynamicBag(Intersect(bagA.Properties, bagB.Properties));
                return true;
            }
            else if (typeA is DynamicPrimitiveSymbol dpa
                && typeB is DynamicPrimitiveSymbol dpb
                && TryGetCommonType(dpa.UnderlyingType, dpb.UnderlyingType, allowedConversions, out var dut))
            {
                commonType = ScalarTypes.GetDynamic(dut);
                return true;
            }
            else if (typeA is DynamicPrimitiveSymbol dpa1
                && typeB is ScalarSymbol
                && TryGetCommonType(dpa1.UnderlyingType, typeB, allowedConversions, out dut))
            {
                commonType = ScalarTypes.GetDynamic(dut);
                return true;
            }
            else if (typeA is ScalarSymbol
                && typeB is DynamicPrimitiveSymbol dpb1
                && TryGetCommonType(typeA, dpb1.UnderlyingType, allowedConversions, out dut))
            {
                commonType = ScalarTypes.GetDynamic(dut);
                return true;
            }
            else if (typeA is DynamicSymbol 
                && typeB is DynamicSymbol)
            {
                commonType = ScalarTypes.Dynamic;
                return true;
            }
            else if (typeA is TableSymbol tableA && typeB is TableSymbol tableB)
            {
                commonType = new TableSymbol(Intersect(tableA.Columns, tableB.Columns));
                return true;
            }
            else if (typeA.IsScalar && typeB.IsScalar 
                && IsConversionAllowed(Conversion.Dynamic, allowedConversions))
            {
                commonType = ScalarTypes.Dynamic;
                return true;
            }
            else
            {
                commonType = null;
                return false;
            }
        }

        /// <summary>
        /// Returns true if the specified conversion is allowed given the allowed conversions.
        /// </summary>
        public static bool IsConversionAllowed(Conversion conversion, Conversion allowedConversions)
        {
            return conversion <= allowedConversions;
        }

        /// <summary>
        /// Gets the common argument type for arguments corresponding to parameters constrained to specific <see cref="ParameterTypeKind"/>.CommonXXX values.
        /// </summary>
        public static TypeSymbol GetCommonArgumentType(
            IReadOnlyList<Parameter> argumentParameters, 
            IReadOnlyList<TypeSymbol> argumentTypes,
            TypeSymbol defaultType = null,
            bool ignoreDynamic = false)
        {
            TypeSymbol commonType = null;

            for (int i = 0; i < argumentTypes.Count; i++)
            {
                var parameter = argumentParameters[i];
                if (parameter != null)
                {
                    var argType = argumentTypes[i];

                    if (ignoreDynamic && argType is DynamicSymbol)
                        continue;

                    if ((parameter.TypeKind == ParameterTypeKind.CommonScalar && argType.IsScalar)
                        || (parameter.TypeKind == ParameterTypeKind.CommonNumber && IsNumeric(argType))
                        || (parameter.TypeKind == ParameterTypeKind.CommonSummable && IsSummable(argType))
                        || (parameter.TypeKind == ParameterTypeKind.CommonOrderable && IsOrderable(argType)))
                    {
                        if (!TryGetCommonType(commonType, argType, Conversion.Promotable, out commonType))
                            return defaultType;
                    }
                    else if (parameter.TypeKind == ParameterTypeKind.CommonScalarOrDynamic && argType.IsScalar)
                    {
                        if (!TryGetCommonType(commonType, argType, Conversion.Dynamic, out commonType))
                            return defaultType;
                    }
                }
            }

            if (commonType == null && ignoreDynamic)
                return GetCommonArgumentType(argumentParameters, argumentTypes, defaultType, false);

            return commonType ?? defaultType;
        }

        /// <summary>
        /// Promotes int to long
        /// </summary>
        public static TypeSymbol PromoteToLong(this TypeSymbol type) =>
            type == ScalarTypes.Int ? ScalarTypes.Long : type;

        /// <summary>
        /// Returns true if this type can be promoted to the specified type.
        /// </summary>
        public static bool IsPromotableTo(this TypeSymbol sourceType, TypeSymbol targetType) =>
            (sourceType is ScalarSymbol sourceScalar
             && targetType is ScalarSymbol targetScalar
             && targetScalar.IsWiderThan(sourceScalar))
            ||
            ((sourceType.Kind == SymbolKind.Bag 
                || sourceType.Kind == SymbolKind.Array)
             && targetType == ScalarTypes.Dynamic);

        /// <summary>
        /// Returns true if the type is an integer.
        /// </summary>
        public static bool IsInteger(this Symbol type) =>
            type is ScalarSymbol scalar && scalar.IsInteger;

        /// <summary>
        /// Returns true if the type is an interval.
        /// </summary>
        public static bool IsInterval(this Symbol type) =>
            type is ScalarSymbol scalar && scalar.IsInterval;

        /// <summary>
        /// Returns true if the type is numeric.
        /// </summary>
        public static bool IsNumeric(this Symbol type) =>
            type is ScalarSymbol scalar && scalar.IsNumeric;

        /// <summary>
        /// Returns true if the type is summable.
        /// </summary>
        public static bool IsSummable(this Symbol type) =>
            type is ScalarSymbol scalar && scalar.IsSummable;

        /// <summary>
        /// Returns true if the type is orderable.
        /// </summary>
        public static bool IsOrderable(this Symbol type) =>
            type is ScalarSymbol scalar && scalar.IsOrderable;

        /// <summary>
        /// Returns true if the type is any scalar type except any dynamic symbol.
        /// </summary>
        public static bool IsAnyScalarExceptDynamic(this Symbol type) =>
            type is ScalarSymbol 
            && !(type is DynamicSymbol);

        /// <summary>
        /// Returns true if the type is any scalar type except bool or dynamic(bool)
        /// </summary>
        public static bool IsAnyScalarExceptBool(this Symbol type) =>
            type is ScalarSymbol 
            && type != ScalarTypes.Bool
            && type != ScalarTypes.DynamicBool;

        /// <summary>
        /// Returns true if the type is any scalar except real, bool, dynamic(real) or dynamic(bool)
        /// </summary>
        public static bool IsAnyScalarExceptReadOrBool(this Symbol type) =>
            type is ScalarSymbol 
            && type != ScalarTypes.Real 
            && type != ScalarTypes.Bool
            && type != ScalarTypes.DynamicReal
            && type != ScalarTypes.DynamicBool;

        /// <summary>
        /// Returns true if the type is numeric or bool
        /// </summary>
        public static bool IsNumericOrBool(this Symbol type) =>
            type == ScalarTypes.Bool
            || IsNumeric(type);

        /// <summary>
        /// Returns true if the type is real or decimal
        /// </summary>
        public static bool IsRealOrDecimal(this Symbol type) =>
            type == ScalarTypes.Real
            || type == ScalarTypes.Decimal;

        /// <summary>
        /// Returns true if the type is string or any dynamic
        /// </summary>
        public static bool IsStringOrDynamic(this Symbol type) =>
            type == ScalarTypes.String
            || type is DynamicSymbol;

        /// <summary>
        /// Returns true if the type is string or any array of strings.
        /// </summary>
        public static bool IsStringOrArray(this Symbol type) =>
            type == ScalarTypes.String
            || type == ScalarTypes.DynamicString
            || type == ScalarTypes.DynamicArrayOfString
            || type == ScalarTypes.DynamicArray     // might be array of strings
            || type == ScalarTypes.Dynamic;  // might be string or array of strings 

        /// <summary>
        /// Returns true if the type is any integer, any dynamic integer, dynamic or dynamic array.
        /// </summary>
        public static bool IsIntegerOrArray(this Symbol type) =>
            type.IsInteger()
            || type == ScalarTypes.DynamicLong
            || type == ScalarTypes.DynamicArrayOfLong
            || type == ScalarTypes.DynamicArray
            || type == ScalarTypes.Dynamic;

        /// <summary>
        /// Returns true if the type is any integer, any integer, dynamic integer or dynamic.
        /// </summary>
        public static bool IsIntegerOrDynamic(this Symbol type) =>
            type.IsInteger()
            || type == ScalarTypes.DynamicLong
            || type == ScalarTypes.Dynamic;

        /// <summary>
        /// Returns true if the type is any dynamic array type or dynamic.
        /// </summary>
        public static bool IsDynamicArray(this Symbol type) =>
            type is DynamicArraySymbol
            || type == ScalarTypes.Dynamic; // might be an array

        /// <summary>
        /// Returns true if the type is any dynamic bag type or dynamic.
        /// </summary>
        public static bool IsDynamicBag(this Symbol type) =>
            type is DynamicBagSymbol
            || type == ScalarTypes.Dynamic; // might be a bag

        /// <summary>
        /// Returns true if the type is dynamic, array or bag.
        /// </summary>
        public static bool IsDynamicArrayOrBag(this Symbol type) =>
            type == ScalarTypes.Dynamic
            || type is DynamicArraySymbol
            || type is DynamicBagSymbol;

        private static readonly ObjectPool<Dictionary<string, TypeSymbol>> s_nameToTypePool =
            new ObjectPool<Dictionary<string, TypeSymbol>>(
                () => new Dictionary<string, TypeSymbol>(),
                map => map.Clear());

        private static readonly ObjectPool<List<ColumnSymbol>> s_columnListPool =
            new ObjectPool<List<ColumnSymbol>>(
                () => new List<ColumnSymbol>(),
                list => list.Clear());

        /// <summary>
        /// Returns this list of columns from both lists.
        /// If each contains the same named columns, a column of the common type is used.
        /// </summary>
        public static IReadOnlyList<ColumnSymbol> Union(IReadOnlyList<ColumnSymbol> columnsA, IReadOnlyList<ColumnSymbol> columnsB)
        {
            var nameToTypeMap = s_nameToTypePool.AllocateFromPool();
            var columnList = s_columnListPool.AllocateFromPool();
            try
            {
                foreach (var col in columnsA)
                {
                    nameToTypeMap[col.Name] = col.Type;
                }

                foreach (var col in columnsB)
                {
                    if (nameToTypeMap.TryGetValue(col.Name, out var currentType))
                    {
                        if (!TryGetCommonType(currentType, col.Type, Conversion.Dynamic, out var commonType))
                        {
                            nameToTypeMap[col.Name] = commonType ?? ScalarTypes.Dynamic;
                        }
                    }
                    else
                    {
                        nameToTypeMap[col.Name] = col.Type;
                    }
                }

                foreach (var col in columnsA)
                {
                    if (nameToTypeMap.TryGetValue(col.Name, out var type))
                    {
                        if (col.Type == type)
                        {
                            columnList.Add(col);
                        }
                        else
                        {
                            columnList.Add(new ColumnSymbol(col.Name, type));
                        }

                        nameToTypeMap.Remove(col.Name);
                    }
                }

                foreach (var col in columnsB)
                {
                    if (nameToTypeMap.TryGetValue(col.Name, out var type))
                    {
                        if (col.Type == type)
                        {
                            columnList.Add(col);
                        }
                        else
                        {
                            columnList.Add(new ColumnSymbol(col.Name, type));
                        }

                        nameToTypeMap.Remove(col.Name);
                    }
                }

                return columnList.ToReadOnly();
            }
            finally
            {
                s_nameToTypePool.ReturnToPool(nameToTypeMap);
                s_columnListPool.ReturnToPool(columnList);
            }
        }

        /// <summary>
        /// Returns the list of columns that appear in both lists.
        /// The column types are converted to the common type.
        /// </summary>
        public static IReadOnlyList<ColumnSymbol> Intersect(IReadOnlyList<ColumnSymbol> columnsA, IReadOnlyList<ColumnSymbol> columnsB)
        {
            var nameToTypeMap = s_nameToTypePool.AllocateFromPool();
            var columnList = s_columnListPool.AllocateFromPool();
            try
            {
                foreach (var col in columnsB)
                {
                    nameToTypeMap[col.Name] = col.Type;
                }

                foreach (var col in columnsA)
                {
                    if (nameToTypeMap.TryGetValue(col.Name, out var typeB))
                    {
                        if (TryGetCommonType(col.Type, typeB, Conversion.Dynamic, out var commonType))
                        {
                            if (commonType == col.Type)
                            {
                                columnList.Add(col);
                            }
                            else
                            {
                                columnList.Add(new ColumnSymbol(col.Name, commonType));
                            }
                        }
                        else
                        {
                            columnList.Add(new ColumnSymbol(col.Name, ScalarTypes.Dynamic));
                        }
                    }
                }

                return columnList.ToReadOnly();
            }
            finally
            {
                s_nameToTypePool.ReturnToPool(nameToTypeMap);
                s_columnListPool.ReturnToPool(columnList);
            }
        }
    }
}