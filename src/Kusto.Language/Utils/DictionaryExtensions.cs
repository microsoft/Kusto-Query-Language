using System;
using System.Collections.Generic;

namespace Kusto.Language.Utils
{
    public static class DictionaryExtensions
    {
        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value, Func<TValue, TValue> fnUpdate)
        {
            if (dictionary.TryGetValue(key, out var oldValue))
            {
                dictionary[key] = fnUpdate(oldValue);
            }
            else 
            {
                dictionary[key] = value;
            }
        }
    }
}
