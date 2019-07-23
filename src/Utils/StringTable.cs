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
    internal class StringTable : IEnumerable<string>
    {
        /// <summary/>
        private readonly TextKeyedDictionary<string> map
            = new TextKeyedDictionary<string>();

        /// <summary>
        /// Construct a new empty <see cref="StringTable"/>
        /// </summary>
        public StringTable()
        {
        }

        /// <summary>
        /// Add a new string to the table.
        /// </summary>
        /// <returns>The instance of the value added or already found in the table.</returns>
        public string Add(string text)
        {
            string result;

            if (!this.map.TryGetValue(text, 0, text.Length, out result))
            {
                result = this.map.GetOrAddValue(text, 0, text.Length, (_t, _s, _l) => (_s > 0 || _l < _t.Length) ? _t.Substring(_s, _l) : _t);
            }

            return result;
        }

        /// <summary>
        /// Add a set of strings to the table.
        /// </summary>
        public void Add(IEnumerable<string> strings)
        {
            foreach (var s in strings)
            {
                this.Add(s);
            }
        }

        /// <summary>
        /// Add a new string sub range to the table.
        /// </summary>
        /// <returns>The instance of the value added or already found in the table.</returns>
        public string Add(string text, int start, int length)
        {
            string result;

            if (!this.map.TryGetValue(text, start, length, out result))
            {
                result = this.map.GetOrAddValue(text, start, length, (_t, _s, _l) => _t.Substring(_s, _l));
            }

            return result;
        }

        /// <summary>
        /// True if the table contains the string value.
        /// </summary>
        public bool Contains(string text)
        {
            return this.map.ContainsKey(text);
        }

        /// <summary>
        /// True if the table contains the sub string value.
        /// </summary>
        public bool ContainsKey(string text, int start, int length)
        {
            return this.map.ContainsKey(text, start, length);
        }

        /// <summary/>
        public IEnumerator<string> GetEnumerator()
        {
            return this.map.GetEnumerator();
        }

        /// <summary/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}