using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Parsing
{
    public static class CommandFacts
    {
        /// <summary>
        /// True if the line at the given line start appears to be the start of a Kusto command.
        /// </summary>
        public static bool IsCommandStartLine(string text, int lineStart)
        {
            var lineLength = TextFacts.GetLineLength(text, lineStart);
            var indentationLength = TextFacts.GetIndentationLength(text, lineStart);
            if (lineLength > indentationLength && text[lineStart + indentationLength] == '.')
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Produces a list of kusto command block starts, where each block starts with a new command
        /// </summary>
        public static IReadOnlyList<int> GetCommandBlockStarts(string text) =>
            GetCommandBlockStarts(text, TextFacts.GetLineStarts(text));

        /// <summary>
        /// Produces a list of kusto command block starts, where each block starts with a new command
        /// </summary>
        public static IReadOnlyList<int> GetCommandBlockStarts(string text, IReadOnlyList<int> lineStarts)
        {
            var commandStarts = new List<int>(0);
            int commentStart = -1;

            foreach (var lineStart in lineStarts)
            {
                if (IsCommandStartLine(text, lineStart))
                {
                    // start command from first leading comment line
                    var start = commentStart != -1 ? commentStart : lineStart;
                    if (commandStarts.Count == 0)
                        start = 0; // if first block, then always start at 0
                    commandStarts.Add(start);
                    commentStart = -1;
                }
                else if (CommentFacts.IsCommentLine(text, lineStart))
                {
                    if (commentStart == -1)
                        commentStart = lineStart;
                }
                else
                {
                    // comment sequence is broken.. start over
                    commentStart = -1;
                }
            }

            if (commandStarts.Count == 0)
            {
                // if no commands found, then always have one block starting at 0.
                commandStarts.Add(0);
            }

            return commandStarts;
        }

        /// <summary>
        /// Produces a list of command block texts from a script text of multiple commands.
        /// </summary>
        public static IReadOnlyList<string> GetCommandBlockTexts(string text)
        {
            var lineStarts = TextFacts.GetLineStarts(text);
            IReadOnlyList<int> blockStarts = GetCommandBlockStarts(text, lineStarts);
            blockStarts = ScriptFacts.RemoveDuplicateBlockStarts(blockStarts);
            blockStarts = ScriptFacts.RemoveInvalidKustoBlockStarts(blockStarts, text);

            var blockTexts = new List<string>();
            for (int i = 0; i < blockStarts.Count; i++)
            {
                var start = blockStarts[i];
                var end = i < blockStarts.Count - 1 ? blockStarts[i + 1] : text.Length;
                var length = end - start;
                var blockText = text.Substring(start, length);
                blockTexts.Add(blockText);
            }

            return blockTexts;
        }

        /// <summary>
        /// Produce a list of command texts from a script texts of multiple commands,
        /// where commands have leading and trailing comment and whitespace lines removed.
        /// </summary>
        public static IReadOnlyList<string> GetCommandTexts(string text)
        {
            var commandTexts = new List<string>();

            var blockTexts = GetCommandBlockTexts(text);
            foreach (var blockText in blockTexts)
            {
                var trimmed = CommentFacts.TrimCommentAndWhitespaceLines(blockText);
                if (IsCommandStartLine(trimmed, 0))
                {
                    commandTexts.Add(trimmed);
                }
            }

            return commandTexts;
        }
    }
}
