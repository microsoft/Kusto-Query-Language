using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kusto.Language.Utils
{
    public static class ListExtensions
    {
        public static IReadOnlyList<T> ToReadOnly<T>(this IEnumerable<T> list)
        {
            if (list == null)
            {
                return EmptyReadOnlyList<T>.Instance;
            }

            var readOnlyCollection = list as ReadOnlyCollection<T>;
            if (readOnlyCollection != null)
            {
                return readOnlyCollection;
            }

            return list.ToList().AsReadOnly();
        }

        /// <summary>
        /// Searches for an item in the ordered array that matches the comparison.
        /// The comparer function returns 0 if it matches the item, -1 if the item comes before and 1 if the item comes after.
        /// Returns the index of the item found or -1 if the item was not found.
        /// </summary>
        public static int BinarySearch<T>(this IReadOnlyList<T> array, Func<T, int> comparer)
        {
            var left = 0;
            var right = array.Count - 1;

            while (left <= right)
            {
                int mid = (left + right) / 2;

                var c = comparer(array[mid]);

                if (c == 0)
                {
                    return mid;
                }
                else if (c > 0)
                {
                    right = mid - 1;
                }
                else
                {
                    left = mid + 1;
                }
            }

            // not found
            return -1;
        }

        /// <summary>
        /// Return the index of the item in the list or -1 if not found.
        /// </summary>
        public static int IndexOf<T>(this IReadOnlyList<T> list, T item)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(item, list[i]))
                    return i;
            }

            return -1;
        }

        internal static void RemoveAll<T>(this List<T> list, Func<T, bool> selector)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (selector(list[i]))
                {
                    list.RemoveAt(i);
                }
            }
        }

        internal static void SetCount<T>(this List<T> list, int count)
        {
            if (list.Count > count)
            {
                list.RemoveRange(count, list.Count - count);
            }
        }

        internal static string Join(this IReadOnlyList<string> items, string separator, string finalSeparator = null)
        {
            var builder = new StringBuilder();

            for (int i = 0, n = items.Count; i < n; i++)
            {
                if (i > 0)
                    builder.Append(i < n - 1 ? separator : finalSeparator);

                builder.Append(items[i]);
            }

            return builder.ToString();
        }

        internal static bool IsEquivalentTo<K, V>(this IReadOnlyDictionary<K, V> a, IReadOnlyDictionary<K, V> b)
            where K : IEquatable<K>
            where V : IEquatable<V>
        {
            if (a == b)
                return true;

            if (a == null || b == null)
                return false;

            if (a.Count != b.Count)
                return false;

            foreach (var kvp in a)
            {
                if (!b.TryGetValue(kvp.Key, out var bValue))
                    return false;

                if (kvp.Value != null && bValue != null && !kvp.Value.Equals(bValue))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the index of the first match or -1 if no matches.
        /// </summary>
        public static int FirstIndex<T>(this IReadOnlyList<T> list, Func<T, bool> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Returns the index of the last match or -1 if no matches.
        /// </summary>
        public static int LastIndex<T>(this IReadOnlyList<T> list, Func<T, bool> predicate)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (predicate(list[i]))
                    return i;
            }

            return -1;
        }
    }

    /// <summary>
    /// Compares <see cref="IReadOnlyList{T}"/> instances for structural equality (all elements are equal in order)
    /// </summary>
    /// <remarks>Assumes list instances are actually immutable.</remarks>
    internal class ReadOnlyListComparer<T> : IEqualityComparer<IReadOnlyList<T>>
    {
        /// <summary>
        /// A <see cref="ReadOnlyListComparer{T}"/> that compares elements using their default comparer.
        /// </summary>
        public static readonly ReadOnlyListComparer<T> Default = new ReadOnlyListComparer<T>(EqualityComparer<T>.Default);

        private readonly IEqualityComparer<T> comparer;

        public ReadOnlyListComparer(IEqualityComparer<T> comparer)
        {
            this.comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }

        public bool Equals(IReadOnlyList<T> x, IReadOnlyList<T> y)
        {
            if (x == y)
                return true;

            if (x == null || y == null)
                return false;

            if (x.Count != y.Count)
                return false;

            for (int i = 0; i < x.Count; i++)
            {
                if (!comparer.Equals(x[i], y[i]))
                    return false;
            }

            return true;
        }

        public int GetHashCode(IReadOnlyList<T> list)
        {
            if (list == null)
                return 0;
            if (list.Count == 1)
                return list[0].GetHashCode();

            int hc = 0;
            foreach (var table in list)
            {
                hc = unchecked(hc + table.GetHashCode());
            }

            return hc;
        }
    }
}