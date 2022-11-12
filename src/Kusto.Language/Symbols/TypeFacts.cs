using System;
using System.Collections.Generic;

namespace Kusto.Language.Symbols
{
    public static class TypeFacts
    {
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

            return widestType;
        }

        /// <summary>
        /// Gets the common scalar type amongst a set of types.
        /// This is either the one type if they are all them same type, the most promoted of the types, or the common type of the types that are not dynamic.
        /// </summary>
        public static TypeSymbol GetCommonScalarType(params TypeSymbol[] types)
        {
            return GetCommonScalarType((IReadOnlyList<TypeSymbol>)types);
        }

        /// <summary>
        /// Gets the common scalar type amongst a set of types.
        /// This is either the one type if they are all them same type, the most promoted of the types, or the common type of the types that are not dynamic.
        /// </summary>
        public static TypeSymbol GetCommonScalarType(IReadOnlyList<TypeSymbol> types)
        {
            TypeSymbol commonType = null;
            bool hadUnknown = false;

            for (int i = 0; i < types.Count; i++)
            {
                var type = types[i];
                if (type is ScalarSymbol)
                {
                    // TODO: should there be a general betterness between types instead of these specific rules?
                    if (commonType == null)
                    {
                        if (type == ScalarTypes.Unknown)
                        {
                            hadUnknown = true;
                        }
                        else
                        {
                            commonType = type;
                        }
                    }
                    else if (commonType.IsPromotableTo(type))
                    {
                        // a type that can be promoted to is better
                        commonType = type;
                    }
                    else if (ScalarTypes.Dynamic.IsAssignableTo(commonType))
                    {
                        // non-dynamic scalars are better
                        commonType = type;
                    }
                }
            }

            if (commonType == null && hadUnknown)
                return ScalarTypes.Unknown;

            return commonType;
        }

        /// <summary>
        /// Promotes int to long
        /// </summary>
        public static TypeSymbol PromoteToLong(this TypeSymbol type) =>
            type == ScalarTypes.Int ? ScalarTypes.Long : type;


        /// <summary>
        /// True if this type can be promoted to the specified type.
        /// </summary>
        public static bool IsPromotableTo(this TypeSymbol sourceType, TypeSymbol targetType) =>
            sourceType is ScalarSymbol sourceScalar
            && targetType is ScalarSymbol targetScalar
            && targetScalar.IsWiderThan(sourceScalar);
    }
}