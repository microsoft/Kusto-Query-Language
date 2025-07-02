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
                case '\t':     // tab
                case ' ':      // space
                case '\r':     // carriage return
                case '\n':     // line feed
                case '\u000c': // form feed
                case '\u00a0': // no break space
                case '\u1680': // ogham space mark
                case '\u180e': // mongolian vowel separator
                case '\u2000': // en quad
                case '\u2001': // em quad
                case '\u2002': // en space
                case '\u2003': // em space
                case '\u2004': // three-per-em space
                case '\u2005': // four-per-em space
                case '\u2006': // six-per-em space
                case '\u2007': // figure space
                case '\u2008': // punctuation space
                case '\u2009': // thin space
                case '\u200a': // hair space
                case '\u200b': // zero width space
                case '\u202f': // narrow no break space
                case '\u205f': // medium mathematical space
                case '\u3000': // ideograph space
                case '\uFEFF': // byte order mark
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// True if the text is all whitespace.
        /// </summary>
        public static bool IsWhitespaceOnly(string text)
        {
            return IsWhitespaceOnly(text, 0, text.Length);
        }

        /// <summary>
        /// True if the range of text is all whitespace.
        /// </summary>
        public static bool IsWhitespaceOnly(string text, int start, int length)
        {
            for (int i = start, n = Math.Min(text.Length, start + length); i < n; i++)
            {
                if (!IsWhitespace(text[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// True if the line starting at start position is only whitespace.
        /// </summary>
        public static bool IsWhitespaceLine(string text, int start)
        {
            var length = GetLineLength(text, start);
            return IsWhitespaceOnly(text, start, length);
        }

        /// <summary>
        /// Gets the contiguous whitespace in the text from the starting position.
        /// </summary>
        public static string GetWhitespace(string text, int start)
        {
            var count = GetWhitespaceCount(text, start);
            return count > 0
                ? text.Substring(start, count)
                : "";
        }

        /// <summary>
        /// Gets the count of contiguous whitespace in the text after the starting position.
        /// </summary>
        public static int GetWhitespaceCount(string text, int start)
        {
            var end = start;
            
            while (end < text.Length && TextFacts.IsWhitespace(text[end]))
            {
                end++;
            }

            return end - start;
        }

        /// <summary>
        /// Gets the count of continguous whitespace before the starting position.
        /// </summary>
        public static int GetWhitespaceCountBefore(string text, int start)
        {
            var pos = start - 1;
            while (pos >= 0 && TextFacts.IsWhitespace(text[pos]))
            {
                pos--;
            }

            return start - pos;
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
        /// Gets the index of the end of the next line break or -1
        /// </summary>
        public static int GetNextLineBreakEnd(string text, int start)
        {
            var result = GetNextLineBreakStart(text, start);
            return result >= 0 ? result + GetLineBreakLength(text, result) : -1;
        }

        /// <summary>
        /// Returns the position of end of the line containing the specified position.
        /// Does not include any line break characters.
        /// </summary>
        public static int GetLineEnd(string text, int position)
        {
            var lengthToEnd = GetLineLength(text, position);
            return position + lengthToEnd;
        }

        /// <summary>
        /// Returns the position of the start of the line containing the specified position.
        /// </summary>
        public static int GetLineStart(string text, int position)
        {
            if (TryGetPreviousLineEnd(text, position, out var previousLineEnd))
            {
                var lineBreakLength = GetLineBreakLength(text, previousLineEnd);
                return previousLineEnd + lineBreakLength;
            }

            return 0;
        }

        /// <summary>
        /// Gets the position of the end of the previous line given a position on the current line.
        /// </summary>
        public static bool TryGetPreviousLineEnd(string text, int position, out int previousLineEnd)
        {
            for (; position >= 0; position--)
            {
                var lineBreakLength = GetLineBreakLength(text, position);
                
                if (lineBreakLength == 0)
                    continue;

                if (lineBreakLength == 1 
                    && position > 0
                    && GetLineBreakLength(text, position - 1) is int longerLineBreakLength
                    && longerLineBreakLength == lineBreakLength + 1)
                {
                    previousLineEnd = position - 1;
                }
                else
                {
                    previousLineEnd = position;
                }

                return true;
            }

            previousLineEnd = 0;
            return false;
        }

        /// <summary>
        /// Gets the position of the start of the next line given a position on the current line.
        /// </summary>
        public static bool TryGetNextLineStart(string text, int position, out int nextLineStart)
        {
            var lineEnd = GetLineEnd(text, position);
            var lineBreakLength = GetLineBreakLength(text, lineEnd);
            if (lineBreakLength > 0)
            {
                nextLineStart = lineEnd + lineBreakLength;
                return true;
            }
            else
            {
                nextLineStart = 0;
                return false;
            }
        }

        /// <summary>
        /// Gets the position of the start of the first line break or -1
        /// </summary>
        public static int GetFirstLineBreakStart(string text)
        {
            return GetNextLineBreakStart(text, 0);
        }

        /// <summary>
        /// Gets the index of the end of hte first line break or -1
        /// </summary>
        public static int GetFirstLineBreakEnd(string text)
        {
            return GetNextLineBreakEnd(text, 0);
        }

        /// <summary>
        /// Gets the index of the start of the last line break or -1
        /// </summary>
        public static int GetLastLineBreakStart(string text, int start = 0)
        {
            var result = -1;
            var lastLbEnd = start;

            while (start >= 0)
            {
                var nextLbStart = GetNextLineBreakStart(text, lastLbEnd);
                if (nextLbStart >= 0)
                {
                    lastLbEnd = nextLbStart + GetLineBreakLength(text, lastLbEnd);
                    result = start = nextLbStart;
                    continue;
                }
                else
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the index of the start of the last line break or -1
        /// </summary>
        public static int GetLastLineBreakEnd(string text, int start = 0)
        {
            var result = GetLastLineBreakStart(text, start);
            return result >= 0 ? result + GetLineBreakLength(text, result) : -1;
        }

        /// <summary>
        /// Returns true if the line is empty or whitespace.
        /// </summary>
        public static bool IsBlankLine(string text, int lineStart)
        {
            var pos = GetPositionAfterLeadingWhitespace(text, lineStart);
            return pos == text.Length || IsLineBreakStart(text[pos]);
        }

        /// <summary>
        /// Gets the text position after the leading whitespace on the line.
        /// </summary>
        public static int GetPositionAfterLeadingWhitespace(string text, int lineStart)
        {
            var pos = lineStart;

            while (pos < text.Length && IsWhitespace(text[pos]) && !IsLineBreakStart(text[pos]))
                pos++;

            return pos;
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

        public static bool IsLetterOrDigit(char ch)
        {
            return (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9');
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
        /// Gets the starting offset of all the lines.
        /// </summary>
        public static IReadOnlyList<int> GetLineStarts(string text)
        {
            var starts = new List<int>();
            GetLineStarts(text, starts);
            return starts;
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

        /// <summary>
        /// Trims the whitespace from the start of the range.
        /// </summary>
        public static void TrimRangeStart(string text, ref int start, ref int length)
        {
            var count = GetWhitespaceCount(text, start);
            start += count;
            length -= count;
        }

        /// <summary>
        /// Trims the whitespace from the end of the range.
        /// </summary>
        public static void TrimRangeEnd(string text, int start, ref int length)
        {
            length = TrimEnd(text, start, length);
        }

        /// <summary>
        /// Trims the whitespace from the start and end of the range.
        /// </summary>
        public static void TrimRange(string text, ref int start, ref int length)
        {
            TrimRangeStart(text, ref start, ref length);
            TrimRangeEnd(text, start, ref length);
        }

        /// <summary>
        /// Expands the range to include the start of the first line.
        /// </summary>
        public static void ExpandRangeToStartOfFirstLine(string text, ref int start, ref int length)
        {
            var newStart = GetLineStart(text, start);
            start = newStart;
            length += start - newStart;
        }

        /// <summary>
        /// Expands range to include the end of the last line.
        /// </summary>
        public static void ExpandRangeToEndOfLastLine(string text, int start, ref int length)
        {
            var newEnd = GetLineEnd(text, start + length);
            length = newEnd - start;
        }

        /// <summary>
        /// Expands the range to include the start of the first line and the end of the last line.
        /// </summary>
        public static void ExpandRangeToStartAndEndOfLines(string text, ref int start, ref int length)
        {
            ExpandRangeToStartOfFirstLine(text, ref start, ref length);
            ExpandRangeToEndOfLastLine(text, start, ref length);
        }

        /// <summary>
        /// Gets the length of the whitespace indentation for the line containing the specified position.
        /// </summary>
        public static int GetIndentationLength(string text, int position)
        {
            var startOfLine = GetLineStart(text, position);
            return GetWhitespaceCount(text, startOfLine);
        }

        /// <summary>
        /// Gets the indentation text for the line containing the position.
        /// </summary>
        public static string GetIndentationText(string text, int position)
        {
            var startOfLine = GetLineStart(text, position);
            return GetWhitespace(text, startOfLine);
        }

        /// <summary>
        /// Gets the text of the line for the specified 1-based line number.
        /// </summary>
        public static string GetLineText(string text, int line)
        {
            if (TryGetLineStart(text, line, out var lineStart))
            {
                var lineEnd = GetLineEnd(text, lineStart);
                return text.Substring(lineStart, lineEnd - lineStart);
            }
            return "";
        }

        /// <summary>
        /// Gets the text of the lines for each line in the text.
        /// </summary>
        public static IReadOnlyList<string> GetLineTexts(string text)
        {
            var lineStarts = GetLineStarts(text);
            var lines = new List<string>(lineStarts.Count);
            for (int i = 0; i < lineStarts.Count; i++)
            {
                var lineStart = lineStarts[i];
                var lineLength = GetLineLength(text, lineStart);
                lines.Add(text.Substring(lineStart, lineLength));
            }
            return lines;
        }
    }
}