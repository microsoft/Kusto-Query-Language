using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Parsing
{
    using Utils;

    public static class CommentFacts
    {
        /// <summary>
        /// Gets the text of the comments in the text.
        /// </summary>
        public static IReadOnlyList<string> GetCommentTexts(string text)
        {
            if (!text.Contains("/"))
                return EmptyReadOnlyList<string>.Instance;

            var commentTexts = new List<string>();
            var lines = TextFacts.GetLineTexts(text);

            foreach (var line in lines)
            {
                if (IsCommentLine(line))
                {
                    commentTexts.Add(GetCommentLineText(line));
                }
            }

            return commentTexts;
        }

        /// <summary>
        /// True if the line at the given line start appears to be a comment line.
        /// </summary>
        public static bool IsCommentLine(string text, int lineStart = 0)
        {
            var lineLength = TextFacts.GetLineLength(text, lineStart);
            var indentationLength = TextFacts.GetIndentationLength(text, lineStart);
            return indentationLength < lineLength && IsCommentStart(text, lineStart + indentationLength);
        }

        /// <summary>
        /// Gets the text of the comment for a comment line.
        /// </summary>
        public static string GetCommentLineText(string text, int lineStart = 0)
        {
            var indentation = TextFacts.GetIndentationLength(text, lineStart);
            if (IsCommentStart(text, lineStart + indentation))
            {
                var lineEnd = TextFacts.GetLineEnd(text, lineStart);
                var commentStart = lineStart + indentation + 2;
                var commentLength = lineEnd - commentStart;
                return text.Substring(commentStart, commentLength).Trim();
            }

            return "";
        }

        /// <summary>
        /// True if the text at the given position appears to be the start of a Kusto comment.
        /// </summary>
        private static bool IsCommentStart(string text, int position)
        {
            return position < text.Length - 1
                && text[position] == '/'
                && text[position + 1] == '/';
        }

        /// <summary>
        /// Trims the leading and trailing comment and whitespace lines from the text.
        /// </summary>
        public static string TrimCommentAndWhitespaceLines(string text)
        {
            var lineStarts = TextFacts.GetLineStarts(text);

            // skip comments and whitespace at start
            int startLine = 0;
            while (startLine < lineStarts.Count && 
                (IsCommentLine(text, lineStarts[startLine]) || TextFacts.IsWhitespaceLine(text, lineStarts[startLine])))
            {
                startLine++;
            }

            // skip comments and whitespace at end
            int endLine = lineStarts.Count - 1;
            while (endLine >= 0 
                && (IsCommentLine(text, lineStarts[endLine]) || TextFacts.IsWhitespaceLine(text, lineStarts[endLine])))
            {
                endLine--;
            }

            if (endLine < startLine)
            {
                // there is nothing that is not a comment or whitespace line
                return "";
            }
            else
            {
                var start = lineStarts[startLine];
                var end = lineStarts[endLine] + TextFacts.GetLineLength(text, lineStarts[endLine]);
                var length = end - start;
                return text.Substring(start, length);
            }
        }
    }
}
