using System;
using System.Collections;
using System.Collections.Generic;

namespace Kusto.Language.Utils
{
    public static class Interlocked
    {
        public static T CompareExchange<T>(ref T value, T newValue, T comparand)
            where T : class
        {
#if !BRIDGE
            return System.Threading.Interlocked.CompareExchange(ref value, newValue, comparand);
#else
            if (value == comparand)
            {
                var original = value;
                value = newValue;
                return original;
            }
            else 
            {
                return value;
            }
#endif
        }

        public static T Exchange<T>(ref T value, T newValue)
            where T : class
        {
#if !BRIDGE
            return System.Threading.Interlocked.Exchange(ref value, newValue);
#else
            var original = value;
            value = newValue;
            return original;
#endif
        }
    }
}