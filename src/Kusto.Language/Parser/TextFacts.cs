using System;
using System.Collections.Generic;
using Kusto.Language.Utils;

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

        public static bool IsWhitespaceOnly(string text)
        {
            return IsWhitespaceOnly(text, 0, text.Length);
        }

        public static bool IsWhitespaceOnly(string text, int start, int length)
        {
            for (int i = start, n = Math.Min(text.Length, start + length); i < n; i++)
            {
                if (!IsWhitespace(text[i]))
                    return false;
            }

            return true;
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
        /// Returns true if the line is empty or whitespace.
        /// </summary>
        public static bool IsBlankLine(string text, int lineStart)
        {
            var pos = lineStart;

            while (pos < text.Length && IsWhitespace(text[pos]) && !IsLineBreakStart(text[pos]))
                pos++;

            return pos == text.Length || IsLineBreakStart(text[pos]);
        }

        /// <summary>
        /// Gets the line length (including line break characters)
        /// </summary>
        public static int GetLineLength(string text, int lineStart, bool includeLineBreak = false)
        {
            var pos = lineStart;

            while (pos < text.Length && !IsLineBreakStart(text[pos]))
                pos++;

            if (includeLineBreak && pos < text.Length)
                pos += GetLineBreakLength(text, pos);

            return pos - lineStart;
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
        /// Gets the starting position of the 1-based line number.
        /// </summary>
        public static bool TryGetLineStart(string text, int line, out int lineStart)
        {
            if (line < 1)
            {
                lineStart = 0;
                return false;
            }
            else if (line == 1)
            {
                lineStart = 0;
                return true;
            }

            for (int i = 0, n = text.Length, count = 1; i < n;)
            {
                var lb = GetLineBreakLength(text, i);
                if (lb > 0)
                {
                    i += lb;

                    count++;
                    if (count == line)
                    {
                        lineStart = i;
                        return true;
                    }
                }
                else
                {
                    i++;
                }
            }

            // line beyond the end
            lineStart = 0;
            return false;
        }

        private static readonly ObjectPool<List<int>> s_lineStarts =
            new ObjectPool<List<int>>(() => new List<int>(), list => list.Clear());

        /// <summary>
        /// Gets the 1-based line and lineOffset for a position.
        /// </summary>
        public static bool TryGetLineAndOffset(string text, int position, out int line, out int lineOffset)
        {
            var lineStarts = s_lineStarts.AllocateFromPool();
            try
            {
                GetLineStarts(text, lineStarts);
                return TryGetLineAndOffset(lineStarts, position, out line, out lineOffset);
            }
            finally
            {
                s_lineStarts.ReturnToPool(lineStarts);
            }
        }

        /// <summary>
        /// Gets the position corresponding to the 1-based line and lineOffset.
        /// </summary>
        public static bool TryGetPosition(string text, int line, int lineOffset, out int position)
        {
            if (line >= 1 && lineOffset >= 1 
                && TryGetLineStart(text, line, out var lineStart))
            {
                position = lineStart + (lineOffset - 1);
                return position <= text.Length;
            }

            position = 0;
            return false;
        }

        /// <summary>
        /// Gets the 1-based line and lineOffset for a position.
        /// </summary>
        public static bool TryGetLineAndOffset(List<int> lineStarts, int position, out int line, out int lineOffset)
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
        /// Gets the position corresponding to the 1-based line and lineOffset.
        /// </summary>
        public static bool TryGetPosition(IReadOnlyList<int> lineStarts, int line, int lineOffset, out int position)
        {
            if (line >= 1 && line <= lineStarts.Count && lineOffset >= 1)
            {
                var lineStart = lineStarts[line - 1];
                position = lineStart + (lineOffset - 1);
                return true;
            }

            position = 0;
            return false;
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