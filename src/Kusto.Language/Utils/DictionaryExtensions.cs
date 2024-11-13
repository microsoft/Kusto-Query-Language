using System;
using System.Collections;
using System.Collections.Generic;

namespace Kusto.Language.Utils
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.TryGetValue(key, out var existingValue))
                return existingValue;

            dictionary.Add(key, value);
            return value;
        }

        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueFactory)
        {
            if (dictionary.TryGetValue(key, out var existingValue))
                return existingValue;

            var value = valueFactory(key);
            dictionary.Add(key, value);
            return value;
        }

        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            dictionary[key] = value;
        }

        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> fnAddFactory, Func<TKey, TValue, TValue> fnUpdateFactory)
        {
            if (dictionary.TryGetValue(key, out var oldValue))
            {
                dictionary[key] = fnUpdateFactory(key, oldValue);
            }
            else
            {
                dictionary[key] = fnAddFactory(key);
            }
        }

        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue, TValue> fnAddFactory, Func<TKey, TValue, TValue, TValue> fnUpdateFactory, TValue value)
        {
            if (dictionary.TryGetValue(key, out var oldValue))
            {
                var newValue = fnUpdateFactory(key, oldValue, value);
                dictionary[key] = newValue;
            }
            else
            {
                var addValue = fnAddFactory(key, value);
                dictionary.Add(key, addValue);
            }
        }
    }
}
