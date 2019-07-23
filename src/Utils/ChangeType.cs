using System;
using System.Collections;
using System.Collections.Generic;

namespace Kusto.Language.Utils
{
    public static class ConvertHelper
    {
        public static object ChangeType(object value, object sample)
        {
            if (sample is string)
            {
                return Convert.ToString(value);
            }
            else if (sample is int)
            {
                return Convert.ToInt32(value);
            }
            else if (sample is long)
            {
                return Convert.ToInt64(value);
            }
            else if (sample is float)
            {
                return Convert.ToSingle(value);
            }
            else if (sample is double)
            {
                return Convert.ToDouble(value);
            }
            else if (sample is DateTime)
            {
                return Convert.ToDateTime(value);
            }
            else if (sample is Decimal)
            {
                return Convert.ToDecimal(value);
            }
            else if (sample is bool)
            {
                return Convert.ToBoolean(value);
            }
            else if (sample is short)
            {
                return Convert.ToInt16(value);
            }
            else if (sample is byte)
            {
                return Convert.ToByte(value);
            }
            else if (sample is sbyte)
            {
                return Convert.ToSByte(value);
            }
            else
            {
                throw new InvalidOperationException("Undefined type conversion.");
            }
        }
    }
}