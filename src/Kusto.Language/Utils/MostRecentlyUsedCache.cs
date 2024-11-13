using System;
using System.Collections.Generic;

namespace Kusto.Language.Utils
{
    /// <summary>
    /// A map that limits the number of entries,
    /// retaining only the most recently accessed ones.
    /// </summary>
    internal class MostRecentlyUsedCache<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        private List<KeyValuePair<TKey, TValue>> _pairs;
        private readonly int _maxSize;

        public MostRecentlyUsedCache(int maxSize = 10)
        {
            _pairs = new List<KeyValuePair<TKey, TValue>>();
            _maxSize = maxSize;
        }

        /// <summary>
        /// The current number of entries in the cache.
        /// </summary>
        public int Count => _pairs.Count;

        /// <summary>
        /// Gets the value for the key and marks the key as recently used.
        /// </summary>
        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (this)
            {
                for (int i = 0; i < _pairs.Count; i++)
                {
                    var pair = _pairs[i];
                    if (pair.Key.Equals(key))
                    {
                        value = pair.Value;

                        // move most recently accessed pair to front of list
                        _pairs.RemoveAt(i);
                        _pairs.Insert(0, pair);

                        return true;
                    }
                }

                value = default(TValue);
                return false;
            }
        }

        /// <summary>
        /// Adds or updates the value for the key and removes any items beyond the max size.
        /// </summary>
        public void AddOrUpdate(TKey key, TValue value)
        {
            lock (this)
            {
                for (int i = 0; i < _pairs.Count; i++)
                {
                    var pair = _pairs[i];
                    if (pair.Key.Equals(key))
                    {
                        value = pair.Value;

                        // update and move to front
                        _pairs.RemoveAt(i);
                        _pairs.Insert(0, new KeyValuePair<TKey, TValue>(key, value));
                        return;
                    }
                }

                // did not find, so add
                _pairs.Insert(0, new KeyValuePair<TKey, TValue>(key, value));

                while (_pairs.Count > _maxSize)
                    _pairs.RemoveAt(_pairs.Count - 1);
            }
        }
    }
}
