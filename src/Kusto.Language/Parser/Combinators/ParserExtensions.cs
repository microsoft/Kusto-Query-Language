using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    public static class ParserExtensions
    {
        /// <summary>
        /// Parses the text into the output list.
        /// </summary>
        public static int Parse(this Parser<char> parser, string text, List<object> output)
        {
            return parser.Parse(new TextSource(text), 0, output, 0);
        }

        /// <summary>
        /// Parses the text and returns a single output value in <see cref="ParseResult{TOutput}"/>.
        /// </summary>
        public static ParseResult<TOutput> Parse<TOutput>(this Parser<char, TOutput> parser, string text)
        {
            return parser.Parse(new TextSource(text), 0);
        }

        /// <summary>
        /// Returns true if the parser successfully parses the text. 
        /// Returns the produced value as an out parameter.
        /// </summary>
        public static bool TryParse<TOutput>(this Parser<char, TOutput> parser, string text, out TOutput value)
        {
            var result = parser.Parse(text);
            value = result.Value;
            return result.Length > 0;
        }
    }
}
