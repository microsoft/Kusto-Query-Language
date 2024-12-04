using System;
using System.Collections.Generic;
#if !BRIDGE
using System.Collections.Concurrent;
#endif

namespace Kusto.Language.Utils
{
    internal class ThreadSafeDictionary<TKey, TValue>
    {
#if !BRIDGE
        private readonly ConcurrentDictionary<TKey, TValue> _dictionary;

        public ThreadSafeDictionary()
        {
            _dictionary = new ConcurrentDictionary<TKey, TValue>();
        }

        public ThreadSafeDictionary(IEqualityComparer<TKey> comparer)
        {
            _dictionary = new ConcurrentDictionary<TKey, TValue>(comparer);
        }
#else
        // bridge does not support concurrent types and no threading in javascript

        private readonly Dictionary<TKey, TValue> _dictionary;

        public ThreadSafeDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }

        public ThreadSafeDictionary(IEqualityComparer<TKey> comparer)
        {
            _dictionary = new Dictionary<TKey, TValue>(comparer);
        }
#endif

        public int Count => _dictionary.Count;

        public ICollection<TKey> Keys => _dictionary.Keys;
        public ICollection<TValue> Values => _dictionary.Values;

        public bool TryGetValue(TKey key, out TValue value) =>
            _dictionary.TryGetValue(key, out value);

        public bool TryAdd(TKey key, TValue value) =>
            _dictionary.TryAdd(key, value);

        public TValue GetOrAdd(TKey key, TValue value) =>
            _dictionary.GetOrAdd(key, value);

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory) =>
            _dictionary.GetOrAdd(key, valueFactory);

        public void AddOrUpdate(TKey key, TValue value) =>
            this.AddOrUpdate(key, (k, ev) => ev, (k, ev, v) => v, value);

        public void AddOrUpdate(TKey key, Func<TKey, TValue> addFactory, Func<TKey, TValue, TValue> updateFactory) =>
            _dictionary.AddOrUpdate(key, addFactory, updateFactory);

#if NET472_OR_GREATER || NET || BRIDGE
        public void AddOrUpdate(TKey key, Func<TKey, TValue, TValue> addFactory, Func<TKey, TValue, TValue, TValue> updateFactory, TValue value) =>
            _dictionary.AddOrUpdate(key, addFactory, updateFactory, value);
#else
        public void AddOrUpdate(TKey key, Func<TKey, TValue, TValue> addFactory, Func<TKey, TValue, TValue, TValue> updateFactory, TValue value) =>
            _dictionary.AddOrUpdate(key, k => addFactory(k, value), (k, v) => updateFactory(k, v, value));
#endif
    }
}
