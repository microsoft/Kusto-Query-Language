using System;
using System.Linq;
using System.Text;

namespace Kusto.Language.Utils
{
    internal static class StringExtensions
    {
        /// <summary>
        /// Returns a new string with the characters filtered to only characters matching the predicate.
        /// </summary>
        public static string Filter(this string text, Func<char, bool> fnPredicate)
        {
            if (text.All(fnPredicate))
                return text;

            var builder = new StringBuilder();

            foreach (var ch in text)
            {
                if (fnPredicate(ch))
                    builder.Append(ch);
            }

            return builder.ToString();
        }
    }
}
