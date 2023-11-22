using System;

namespace Kusto.Language.Utils
{
    internal static class ValueComparer
    {
        /// <summary>
        /// Returns true if the values are equivalent.
        /// The values do not have to be the same type.
        /// </summary>
        public static bool AreEquivalent(object x, object y, bool caseSensitive = false)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            var xType = x.GetType();
            var yType = y.GetType();

            // make both values have the same type for comparison
            if (!TryGetBestComparingType(xType, yType, out var comparingType))
                return false;

            var convertedX = comparingType == xType ? x : Convert.ChangeType(x, comparingType);
            var convertedY = comparingType == yType ? y : Convert.ChangeType(y, comparingType);

            if (comparingType == typeof(string))
            {
                return string.Compare((string)convertedX, (string)convertedY, !caseSensitive) == 0;
            }
            else
            {
                return object.Equals(convertedX, convertedY);
            }
        }

        /// <summary>
        /// Gets the best type for comparing between the two specified types,
        /// such that ChangeType will not throw.
        /// </summary>
        private static bool TryGetBestComparingType(Type typeA, Type typeB, out Type comparingType)
        {
            if (typeA == typeB)
            {
                comparingType = typeA;
                return true;
            }
            else if (typeA == typeof(string) || typeB == typeof(string))
            {
                comparingType = typeof(string);
                return true;
            }
            else if (IsNumeric(typeA) && IsNumeric(typeB))
            {
                if (typeA == typeof(double) || typeB == typeof(double))
                {
                    comparingType = typeof(double);
                }
                else if (typeA == typeof(decimal) || typeB == typeof(decimal))
                {
                    comparingType = typeof(decimal);
                }
                else if (typeA == typeof(float) || typeB == typeof(float))
                {
                    comparingType = typeof(float);
                }
                else if (typeA == typeof(UInt64) || typeB == typeof(UInt64))
                {
                    comparingType = typeof(UInt64);
                }
                else
                {
                    comparingType = typeof(Int64);
                }

                return true;
            }
            else
            {
                comparingType = null;
                return false;
            }
        }

        private static bool IsNumeric(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return true;
                default:
                    return false;
            }
        }
    }
}