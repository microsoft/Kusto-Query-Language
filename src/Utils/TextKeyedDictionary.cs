using System;
using System.Collections;
using System.Collections.Generic;

namespace Kusto.Language.Utils
{
    /// <summary>
    /// A table of strings accessible via lookup of string ranges
    /// </summary>
    /// <remarks>
    /// You can use this class to look up strings in the table using a range of characters from another string
    /// without having to first get a substring to use as a key. This avoids unnecessary alloctions.
    /// </remarks>
    internal class TextKeyedDictionary<TValue> : IEnumerable<TValue>
    {
        /// <summary/>
        private readonly Dictionary<Key, TValue> map
            = new Dictionary<Key, TValue>();

        /// <summary>
        /// Construct a new empty <see cref="TextKeyedDictionary{TValue}"/>
        /// </summary>
        public TextKeyedDictionary()
        {
        }

        /// <summary>
        /// Gets the corresponding value. Returns true if the value is found.
        /// </summary>
        public bool TryGetValue(string text, int start, int length, out TValue value)
        {
            Key key = new Key(text, start, length);
            return this.map.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets the corresponding value or adds it.
        /// </summary>
        public TValue GetOrAddValue(string text, TValue newValue)
        {
            return this.GetOrAddValue(text, 0, text.Length, newValue);
        }

        /// <summary>
        /// Gets the corresponding value or adds it.
        /// </summary>
        public TValue GetOrAddValue(string text, int start, int length, TValue newValue)
        {
            TValue value;
            Key key = new Key(text, start, length);

            if (!this.map.TryGetValue(key, out value))
            {
                value = newValue;
                this.map.Add(key, value);
            }

            return value;
        }

        /// <summary>
        /// Gets the corresponding value or adds it.
        /// </summary>
        public TValue GetOrAddValue(string text, Func<string, int, int, TValue> evaluator)
        {
            return this.GetOrAddValue(text, 0, text.Length, evaluator);
        }

        /// <summary>
        /// Gets the corresponding value or adds it.
        /// </summary>
        public TValue GetOrAddValue(string text, int start, int length, Func<string, int, int, TValue> evaluator)
        {
            TValue value;
            Key key = new Key(text, start, length);

            if (!this.map.TryGetValue(key, out value))
            {
                value = evaluator(text, start, length);
                this.map.Add(key, value);
            }

            return value;
        }

        /// <summary>
        /// True if the table contains the string value.
        /// </summary>
        public bool ContainsKey(string text)
        {
            return this.ContainsKey(text, 0, text.Length);
        }

        /// <summary>
        /// True if the table contains the sub string value.
        /// </summary>
        public bool ContainsKey(string text, int start, int length)
        {
            if (start < 0 || length < 1 || start + length > text.Length)
            {
                return false;
            }

            return this.map.ContainsKey(new Key(text, start, length));
        }

        /// <summary/>
        public IEnumerator<TValue> GetEnumerator()
        {
            return this.map.Values.GetEnumerator();
        }

        /// <summary/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary/>
        private struct Key : IEquatable<Key>
        {
            /// <summary/>
            private readonly string text;

            /// <summary/>
            private readonly int start;

            /// <summary/>
            private readonly int length;

            /// <summary/>
            private readonly int hash;

            /// <summary/>
            public Key(string text)
                : this(text, 0, text.Length)
            {
            }

            /// <summary/>
            public Key(string text, int start, int length)
            {
                this.text = text;
                this.start = start;
                this.length = length;
                this.hash = GetFNVHashCode(text, start, length);
            }

            /// <summary>
            /// The offset bias value used in the FNV-1a algorithm
            /// See http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function
            /// </summary>
            private const int FnvOffsetBias = unchecked((int)2166136261);

            /// <summary>
            /// The generative factor used in the FNV-1a algorithm
            /// See http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function
            /// </summary>
            private const int FnvPrime = 16777619;

            /// <summary>
            /// Compute the hashcode of a sub-string using FNV-1a
            /// See http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function
            /// </summary>
            private static int GetFNVHashCode(string text, int start, int length)
            {
                int hashCode = FnvOffsetBias;
                int end = start + length;

                for (int i = start; i < end; i++)
                {
                    hashCode = unchecked((hashCode ^ text[i]) * FnvPrime);
                }

                return hashCode;
            }

            /// <summary/>
            public bool Equals(Key other)
            {
                if (this.hash != other.hash
                    || this.length != other.length)
                {
                    return false;
                }

                for (int i = 0; i < this.length; i++)
                {
                    if (this.text[this.start + i] != other.text[other.start + i])
                    {
                        return false;
                    }
                }

                return true;
            }

            /// <summary/>
            public override int GetHashCode()
            {
                return this.hash;
            }
        }
    }

    internal static class TextKeyedDictionaryExtensions
    {
        public static TextKeyedDictionary<TValue> ToTextKeyedDictionary<TSource, TValue>(
            this IEnumerable<TSource> source, 
            Func<TSource, string> keySelector, 
            Func<TSource, TValue> valueSelector)
        {
            var tkd = new TextKeyedDictionary<TValue>();

            foreach (var item in source)
            {
                tkd.GetOrAddValue(keySelector(item), valueSelector(item));
            }

            return tkd;
        }
    }
}