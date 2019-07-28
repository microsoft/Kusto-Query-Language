using System;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    /// <summary>
    /// Facts about text.
    /// </summary>
    public static class TextFacts
    {
        public static bool IsWhitespace(char ch)
        {
            switch (ch)
            {
                case '\t':
                case ' ':
                case '\r':
                case '\n':
                case '\u000c':
                case '\u00a0':
                case '\u1680':
                case '\u180e':
                case '\u2000':
                case '\u2001':
                case '\u2002':
                case '\u2003':
                case '\u2004':
                case '\u2005':
                case '\u2006':
                case '\u2007':
                case '\u2008':
                case '\u2009':
                case '\u200a':
                case '\u200b':
                case '\u202f':
                case '\u205f':
                case '\u3000':
                case '\uFEFF':
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsLineBreakStart(char ch)
        {
            switch (ch)
            {
                case '\r':      // Carriage Return
                case '\n':      // Line Feed
                case '\u2028':  // Line Separator.
                case '\u2029':  // Paragraph Separator
                    return true;

                default:
                    return false;
            }
        }

        public static int GetLineBreakLength(string text, int position)
        {
            if (position < text.Length)
            {
                var ch = text[position];
                switch (ch)
                {
                    case '\r':
                        if (position + 1 < text.Length && text[position + 1] == '\n')
                        {
                            return 2;
                        }
                        return 1;
                    case '\n':      // Line Feed
                    case '\u2028':  // Line Separator.
                    case '\u2029':  // Paragraph Separator
                        return 1;
                }
            }

            return 0;
        }

        public static bool HasLineBreaks(string text)
        {
            foreach (var c in text)
            {
                if (IsLineBreakStart(c))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the index of the start of the next line break or -1.
        /// </summary>
        public static int GetNextLineBreakStart(string text, int start)
        {
            for (int i = start; i < text.Length; i++)
            {
                if (IsLineBreakStart(text[i]))
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Gets the index of the start of the next line or -1;
        /// </summary>
        public static int GetNextLineStart(string text, int start)
        {
            var nextStart = GetNextLineBreakStart(text, start);
            return nextStart >= 0 ? nextStart + GetLineBreakLength(text, nextStart) : nextStart;
        }

        public static bool IsLetter(char ch)
        {
            return (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z');
        }

        public static bool IsDigit(char ch)
        {
            return (ch >= '0' && ch <= '9');
        }

        public static bool IsHexDigit(char ch)
        {
            return (ch >= '0' && ch <= '9') || (ch >= 'a' && ch <= 'f') || (ch >= 'A' && ch <= 'F');
        }

        /// <summary>
        /// Gets the starting offset of all the lines.
        /// </summary>
        public static void GetLineStarts(string text, List<int> lineStarts)
        {
            lineStarts.Add(0);

            int lineStart = 0;

            for (int i = 0, n = text.Length; i < n;)
            {
                var lb = GetLineBreakLength(text, i);
                if (lb > 0)
                {
                    i += lb;
                    lineStart = i;
                    lineStarts.Add(lineStart);
                }
                else
                {
                    i++;
                }
            }
        }

        /// <summary>
        /// Gets the 1-based line and lineOffset for a position.
        /// </summary>
        public static bool TryGetLineAndOffset(string text, int position, List<int> lineStarts, out int line, out int lineOffset)
        {
            line = lineStarts.BinarySearch(position);
            line = line >= 0 ? line : ~line - 1;

            if (line < 0 || line >= lineStarts.Count)
            {
                line = 0;
                lineOffset = 0;
                return false;
            }

            lineOffset = position - lineStarts[line] + 1; // 1 based
            line++; // 1 based

            return true;
        }

        /// <summary>
        /// Trims the whitespace off the end of the text range.
        /// Returns the new length with the whitespace removed.
        /// </summary>
        public static int TrimEnd(string text)
        {
            return TrimEnd(text, 0, text.Length);
        }

        /// <summary>
        /// Trims the whitespace off the end of the text range.
        /// Returns the new length with the whitespace removed.
        /// </summary>
        public static int TrimEnd(string text, int start, int length)
        {
            for (var ln = length - 1; ln >= 0; ln--)
            {
                if (!IsWhitespace(text[start + ln]))
                    return ln + 1;
            }

            return 0;
        }
    }
}