using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Parsing
{
    using Utils;
    using static Parsers<char>;

    /// <summary>
    /// A predifined set of common character parsers that produce no output.
    /// These are typically used to do look-ahead scanning.
    /// </summary>
    public static class CharScanners
    {
        /// <summary>
        /// A parser that matches a sequence of text characters.
        /// </summary>
        public static Parser<char> Chars(string text, bool ignoreCase = false) =>
            Match(text, ignoreCase);

        /// <summary>
        /// A parser that matches a single character.
        /// </summary>
        public static Parser<char> Char(char ch, bool ignoreCase = false) =>
            Match(ch, ignoreCase);

        /// <summary>
        /// A parser that matches a single letter.
        /// </summary>
        public static Parser<char> Letter =
            Match(TextFacts.IsLetter).WithTag("<letter>");

        /// <summary>
        /// A parser that matches a single digit.
        /// </summary>
        public static Parser<char> Digit =
            Match(TextFacts.IsDigit).WithTag("<digit>");

        /// <summary>
        /// A parser that matches a single hexadecimal digit.
        /// </summary>
        public static Parser<char> HexDigit =
            Match(TextFacts.IsHexDigit).WithTag("<hex-digit>");

        /// <summary>
        /// A parser that matches a single whitespace character.
        /// </summary>
        public static Parser<char> Whitespace =
            Match(TextFacts.IsWhitespace).WithTag("<whitespace>");

        /// <summary>
        /// A parser that matches a line break.
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
