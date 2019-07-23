using System;
using System.Collections;
using System.Collections.Generic;

namespace Kusto.Language.Utils
{
    public static class Ensure
    {
        [System.Diagnostics.Conditional("DEBUG")]
        public static void IsTrue(bool value, string message = null)
        {
            if (!value)
            {
                throw new InvalidOperationException(message ?? "Expected true");
            }
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void AreEqual<T>(T expected, T actual, string message = null)
            where T : IEquatable<T>
        {
            if (!object.Equals(expected, actual))
            {
                throw new InvalidOperationException(message ?? $"Expected: {expected} actual: {actual}");
            }
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void NotNull(object value, string message = null)
        {
            if (value == null)
            {
                throw new InvalidOperationException(message ?? "Expected not null");
            }
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void IsNull(object value, string message = null)
        {
            if (value != null)
            {
                throw new InvalidOperationException(message ?? "Expected null");
            }
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void ArgumentNotNull(object value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void ElementsNotNull<T>(IReadOnlyList<T> list, string listName)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                {
                    throw new InvalidOperationException($"{listName}[{i}] is null");
                }
            }
        }
    }
}