using System;
using System.Collections;
using System.Collections.Generic;

namespace Kusto.Language.Utils
{
    public static class Interlocked
    {
        /// <summary>
        /// Compares two values, and if they are equal replaces with the new value. 
        /// Returns the original value.
        /// </summary>
        public static T CompareExchange<T>(ref T value, T newValue, T comparand)
            where T : class
        {
#if !BRIDGE
            return System.Threading.Interlocked.CompareExchange(ref value, newValue, comparand);
#else
            var original = value;

            if (original == comparand)
            {
                value = newValue;
            }

            return original;
#endif
        }

        /// <summary>
        /// Compares two values, and if they are equal replaces with the new value. 
        /// Returns the original value.
        /// </summary>
        public static int CompareExchange(ref int value, int newValue, int comparand)
        {
#if !BRIDGE
            return System.Threading.Interlocked.CompareExchange(ref value, newValue, comparand);
#else
            var original = value;

            if (original == comparand)
            {
                value = newValue;
            }

            return original;
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