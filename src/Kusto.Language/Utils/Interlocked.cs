using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Kusto.Language.Utils
{
    #region class Interlocked
    /// <summary>
    /// Helper functions for interlocked/atomic operations.
    /// </summary>
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
    #endregion

    #region struct CacheLineSeparated
    /// <summary>
    /// Mimicking the .NET dotnet/runtime/libraries/System.Private.CoreLib/src/System/Threading/LowLevelLifoSemaphore.cs
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [StructLayout(LayoutKind.Sequential)]
    internal struct CacheLineSeparated<T>
    {
        private readonly PaddingFor32 m_pad1;
        public T m_value;
        private readonly PaddingFor32 m_pad2;

        public CacheLineSeparated(T value)
        {
            m_value = value;
        }

        // TODO: Keep m_value public, or provide Inerlocked member functions?

        #region Additional code taken from .NET ("Internal")
        /// <summary>A class for common padding constants and eventually routines.</summary>
        internal static class PaddingHelpers
        {
            /// <summary>A size greater than or equal to the size of the most common CPU cache lines.</summary>
#if TARGET_ARM64 || TARGET_LOONGARCH64
        internal const int CACHE_LINE_SIZE = 128;
#else
            internal const int CACHE_LINE_SIZE = 64;
#endif
        }

        /// <summary>Padding structure used to minimize false sharing</summary>
        [StructLayout(LayoutKind.Explicit, Size = PaddingHelpers.CACHE_LINE_SIZE - sizeof(int))]
        internal readonly struct PaddingFor32
        {
        }

        /// <summary>Padded reference to an object.</summary>
        [StructLayout(LayoutKind.Explicit, Size = PaddingHelpers.CACHE_LINE_SIZE)]
        internal struct PaddedReference
        {
            [FieldOffset(0)]
            public object Object;
        }
        #endregion
    }
    #endregion


}