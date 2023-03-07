using System;
using System.Collections.Generic;

namespace Kusto.Language.Utils
{
    internal static class ArgumentCheckers
    {
        /// <summary>
        /// Throws an exception if the argument is null
        /// </summary>
        public static T CheckArgumentNull<T>(this T value, string parameterName)
        {
            if (value == null)
                throw new ArgumentNullException(parameterName);
            return value;
        }

        /// <summary>
        /// Throws an exception if the argument is null or empty.
        /// </summary>
        public static IReadOnlyList<T> CheckArgumentNullOrEmpty<T>(this IReadOnlyList<T> list, string parameterName)
        {
            if (list == null)
                throw new ArgumentNullException(parameterName);

            if (list.Count == 0)
                throw new ArgumentException($"Parameter '{parameterName}' must have at least one element.");

            return list;
        }

        /// <summary>
        /// Throws an exception if the argument is null or an element is null.
        /// </summary>
        public static IReadOnlyList<T> CheckArgumentNullOrElementNull<T>(this IReadOnlyList<T> list, string parameterName) where T : class
        {
            if (list == null)
                throw new ArgumentNullException(parameterName);

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                    throw new ArgumentException($"Element {i} of parameter '{parameterName}' is null.");
            }

            return list;
        }

        /// <summary>
        /// Throws an exception if the argument is null, empty or an element is null.
        /// </summary>
        public static IReadOnlyList<T> CheckArgumentNullOrEmptyOrElementNull<T>(this IReadOnlyList<T> list, string parameterName) where T : class
        {
            list.CheckArgumentNullOrElementNull(parameterName);
            list.CheckArgumentNullOrEmpty(parameterName);
            return list;
        }
    }
}
