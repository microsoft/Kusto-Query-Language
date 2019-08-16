using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Parsing
{
    using Utils;
    using static Parsers<char>;

    public static class CharScanners
    {
        /// <summary>
        /// A scanner for matching the specified text characters in sequence.
        /// </summary>
        public static Parser<char> Chars(string text, bool ignoreCase = false)
        {
            if (text.Length == 1)
            {
                return Char(text[0], ignoreCase);
            }

            if (ignoreCase)
            {
                var lower = text.ToLower();
                var upper = text.ToUpper();
                return Match((source, start) =>
                {
                    var textSource = source as TextSource;
                    if (source is TextSource ts)
                    {
                        return ts.Matches(start, text, ignoreCase: true) ? text.Length : -1;
                    }
                    else
                    {
                        for (int i = 0; i < text.Length; i++)
                        {
                            var ch = source.Peek(start + i);
                            if (ch != lower[i] && ch != upper[i])
                                return -1;
                        }

                        return text.Length;
                    }
                }).WithTag($"'{text}'");
            }
            else
            {
                return Match((source, start) =>
                {
                    var textSource = source as TextSource;
                    if (source is TextSource ts)
                    {
                        return ts.Matches(start, text) ? text.Length : -1;
                    }
                    else
                    {
                        for (int i = 0; i < text.Length; i++)
                        {
                            if (source.Peek(start + i) != text[i])
                                return -1;
                        }

                        return text.Length;
                    }
                }).WithTag($"'{text}'");
            }
        }

        //public static Parser<char> Chars(string text, bool ignoreCase = false) =>
        //    And(text.Select(c => Char(c, ignoreCase)).ToArray()).WithTag($"'{text}'");

        /// <summary>
        /// A scanner for matching a single character.
        /// </summary>
        public static Parser<char> Char(char ch, bool ignoreCase = false) =>
            (ignoreCase)
                ? MatchCharIgnoreCase(ch, char.ToUpper(ch), char.ToLower(ch)).WithTag($"'{ch.ToString()}'")
                : Match(c => c == ch).WithTag($"'{ch.ToString()}'");

        private static Parser<char> MatchCharIgnoreCase(char ch, char chUpper, char chLower) =>
            Match(c => c == ch || c == chUpper || c == chLower);

        /// <summary>
        /// A scanner that scans a single letter
        /// </summary>
        public static Parser<char> Letter =
            Match(TextFacts.IsLetter).WithTag("<letter>");

        /// <summary>
        /// A scanner that scans a single digit
        /// </summary>
        public static Parser<char> Digit =
            Match(TextFacts.IsDigit).WithTag("<digit>");

        /// <summary>
        /// A scanner that scans a single hexadecimal digit
        /// </summary>
        public static Parser<char> HexDigit =
            Match(TextFacts.IsHexDigit).WithTag("<hex-digit>");

        /// <summary>
        /// A scanner that scans a single whitespace character.
        /// </summary>
        public static Parser<char> Whitespace =
            Match(TextFacts.IsWhitespace).WithTag("<whitespace>");

        /// <summary>
        /// A scanner that scans a line break.
        /// </summary>
        public static Parser<char> LineBreak =
            Or(
                And(Char('\r'), Optional(Char('\n'))),
                Char('\n'),
                Char('\u2028'),
                Char('\u2029'))
            .WithTag("<line-break>");
    }
}
