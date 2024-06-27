using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kusto.Language.Utils
{
    public static class ListExtensions
    {
        /// <summary>
        /// Converts the sequence to a read only list.
        /// </summary>
        public static IReadOnlyList<T> ToReadOnly<T>(this IEnumerable<T> list)
        {
            if (list == null
                || list == EmptyReadOnlyList<T>.Instance
                || (list is IReadOnlyList<T> rol && rol.Count == 0))
            {
                // empty read only list is immutable.
                return EmptyReadOnlyList<T>.Instance;
            }
            else if (list is SafeList<T> safeList)
            {
                // SafeList is immutable
                return safeList;
            }
            else
            {
                // convert to a SafeList to make it immutable.
                return new SafeList<T>(list);
            }
        }

        /// <summary>
        /// Converts the sequence to a <see cref="SafeList{T}"/>
        /// </summary>
        public static SafeList<T> ToSafeList<T>(this IEnumerable<T> list)
        {
            if (list == null
                || list == EmptyReadOnlyList<T>.Instance
                || (list is IReadOnlyList<T> rol && rol.Count == 0))
            {
                // use an sharable empty SafeList
                return SafeList<T>.Empty;
            }
            else if (list is SafeList<T> safeList)
            {
                // already a SafeList
                return safeList;
            }
            else
            {
                // make a new SafeList
                return new SafeList<T>(list);
            }
        }

        /// <summary>
        /// Converts a sequence to a <see cref="HashSet{T}"/>
        /// This function is provided because it does not exist for some runtimes (Bridge.Net included)
        /// </summary>
        public static HashSet<T> ToHashSetEx<T>(this IEnumerable<T> list)
        {
            var hs = new HashSet<T>();

            foreach (var item in list)
            {
                hs.Add(item);
            }

            return hs;
        }

        /// <summary>
        /// Constructs a dictionary from a list that may have duplicate keys.
        /// In case of duplicates, the last matching item is kept.
        /// </summary>
        public static Dictionary<TKey, TValue> ToDictionaryLast<TKey, TValue>(this IEnumerable<TValue> values, Func<TValue, TKey> keySelector)
        {
            return ToDictionaryLast(values, keySelector, v => v);
        }

        /// <summary>
        /// Constructs a dictionary from a list that may have duplicate keys.
        /// In case of duplicates, the last matching item is kept.
        /// </summary>
        public static Dictionary<TKey, TValue> ToDictionaryLast<TItem, TKey, TValue>(this IEnumerable<TItem> items, Func<TItem, TKey> keySelector, Func<TItem, TValue> valueSelector)
        {
            var map = new Dictionary<TKey, TValue>();
            foreach (var item in items)
            {
                var key = keySelector(item);
                var value = valueSelector(item);
                map[key] = value;
            }

            return map;
        }

        public static IReadOnlyList<TItem> AddOrUpdate<TItem, TKey>(this IReadOnlyList<TItem> items, IReadOnlyList<TItem> newOrUpdatedItems, Func<TItem, TKey> keySelector)
        {
            if (newOrUpdatedItems == null || newOrUpdatedItems.Count == 0)
            {
                // no items to add or update
                return items;
            }
            else if (newOrUpdatedItems.Count == 1)
            {
                // single item to add or update
                var item = newOrUpdatedItems[0];
                if (item == null)
                    return items;

                var key = keySelector(item);
                var comparer = EqualityComparer<TKey>.Default;

                return items
                    .Where(it => !comparer.Equals(key, keySelector(it)))
                    .Concat(new[] { item })
                    .ToReadOnly();
            }
            else
            {
                // multiple items to add or updated
                return items
                    .Concat(newOrUpdatedItems)
                    .Where(x => x != null)
                    .DistinctLast(keySelector)
                    .ToReadOnly();
            }
        }

        /// <summary>
        /// Return the sequence of distinct items based on the key, where the first item with a matching key wins.
        /// </summary>
        public static IEnumerable<TItem> DistinctFirst<TItem, TKey>(this IEnumerable<TItem> items, Func<TItem, TKey> keySelector)
        {
            return items.Distinct(new KeyComparer<TItem, TKey>(keySelector));
        }

        /// <summary>
        /// Return the sequence of distinct items based on the key, where last item with a matching key wins.
        /// </summary>
        public static IEnumerable<TItem> DistinctLast<TItem, TKey>(this IEnumerable<TItem> items, Func<TItem, TKey> keySelector)
        {
            return items.Reverse().DistinctFirst(keySelector).Reverse();
        }

        private class KeyComparer<TItem, TKey>
            : IEqualityComparer<TItem>
        {
            private readonly Func<TItem, TKey> _keySelector;

            public KeyComparer(Func<TItem, TKey> keySelector)
            {
                _keySelector = keySelector;
            }

            public bool Equals(TItem x, TItem y)
            {
                return EqualityComparer<TKey>.Default.Equals(
                    _keySelector(x),
                    _keySelector(y));
            }

            public int GetHashCode(TItem obj)
            {
                return _keySelector(obj)?.GetHashCode() ?? 0;
            }
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

        /// <summary>
        /// Returns a new list with the item added.
        /// </summary>
        public static IReadOnlyList<T> Append<T>(this IReadOnlyList<T> list, T newItem)
        {
            return ReplaceRange(list, list.Count, 0, newItem);
        }

        /// <summary>
        /// Returns a new list with the items added.
        /// </summary>
        public static IReadOnlyList<T> Append<T>(this IReadOnlyList<T> list, IReadOnlyList<T> newItems)
        {
            return ReplaceRange(list, list.Count, 0, newItems);
        }

        /// <summary>
        /// Returns a new list with the item at the specified index replaced with the new item.
        /// </summary>
        public static IReadOnlyList<T> Replace<T>(this IReadOnlyList<T> list, int index, T newItem)
        {
            return ReplaceRange(list, index, 1, newItem);
        }

        /// <summary>
        /// Returns a new list with the items in the specified range replaced with the new item.
        /// </summary>
        public static IReadOnlyList<T> ReplaceRange<T>(this IReadOnlyList<T> list, int start, int length, T newItem)
        {
            var newList = list.ToList();
            
            if (length > 0 && start >= 0 && start < newList.Count)
            {
                var removed = Math.Min(newList.Count - start, length);
                if (removed > 0)
                    newList.RemoveRange(start, removed);
            }

            if (newItem != null && start >= 0 && start <= newList.Count)
            {
                newList.Insert(start, newItem);
            }

            return newList;
        }

        /// <summary>
        /// Returns a new list with the items in the specified range replaced with the new item.
        /// </summary>
        public static IReadOnlyList<T> ReplaceRange<T>(this IReadOnlyList<T> list, int start, int length, IReadOnlyList<T> newItems)
        {
            var newList = list.ToList();

            if (length > 0 && start >= 0 && start < newList.Count)
            {
                var removed = Math.Min(newList.Count - start, length);
                if (removed > 0)
                    newList.RemoveRange(start, removed);
            }

            if (newItems != null && newItems.Count  > 0 && start >= 0 && start <= newList.Count)
            {
                newList.InsertRange(start, newItems);
            }

            return newList;
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