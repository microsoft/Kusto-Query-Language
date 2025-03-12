using System;
using System.Collections.Generic;
using System.Linq;
 
namespace Kusto.Language.Parsing
{
    public static class ScriptFacts
    {
        /// <summary>
        /// Produces a list of block starts for a Kusto multi-block document.
        /// </summary>
        public static IReadOnlyList<int> GetKustoBlockStarts(string text, IReadOnlyList<int> lineStarts)
        {
            var blockStarts = GetLineSeparatedBlockStarts(text, lineStarts);
            blockStarts = RemoveDuplicateBlockStarts(blockStarts);
            blockStarts = RemoveInvalidKustoBlockStarts(blockStarts, text);
            return blockStarts;
        }

        /// <summary>
        /// Produces a list of block starts by separating blocks by blank lines.
        /// </summary>
        public static IReadOnlyList<int> GetLineSeparatedBlockStarts(string text, IReadOnlyList<int> lineStarts)
        {
            var blockStarts = new List<int>(0);
            blockStarts.Add(0); // first block starts at 0

            bool newBlockNextWhitespaceLine = false; // no prior block
            bool newBlockNextNonWhitespaceLine = false; // already added first block

            for (int i = 0; i < lineStarts.Count; i++)
            {
                var lineStart = lineStarts[i];
                var lineEnd = i + 1 < lineStarts.Count ? lineStarts[i + 1] : text.Length;
                var allWhitespace = TextFacts.IsWhitespaceOnly(text, lineStart, lineEnd - lineStart);

                if (allWhitespace)
                {
                    if (newBlockNextWhitespaceLine)
                    {
                        blockStarts.Add(lineStart);
                    }
                    newBlockNextWhitespaceLine = true;
                    newBlockNextNonWhitespaceLine = true;
                }
                else if (newBlockNextNonWhitespaceLine)
                {
                    blockStarts.Add(lineStart);
                    newBlockNextWhitespaceLine = false;
                    newBlockNextNonWhitespaceLine = false;
                }
            }

            return blockStarts;
        }

        /// <summary>
        /// Removes any duplicates from list of block starts.
        /// </summary>
        public static IReadOnlyList<int> RemoveDuplicateBlockStarts(IEnumerable<int> blockStarts)
        {
            var starts = blockStarts.OrderBy(x => x).ToList();

            for (int i = 0; i < starts.Count - 1;)
            {
                if (starts[i] == starts[i + 1])
                {
                    starts.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            return starts;
        }

        /// <summary>
        /// Removes block starts that cannot appear in a kusto multi-block document.
        /// </summary>
        public static IReadOnlyList<int> RemoveInvalidKustoBlockStarts(IReadOnlyList<int> blockStarts, string text)
        {
            return RemoveInvalidBlockStarts(blockStarts, GetInvalidKustoBlockStartRanges(text));
        }

        /// <summary>
        /// Removes block starts that occur in invalid ranges.
        /// </summary>
        public static IReadOnlyList<int> RemoveInvalidBlockStarts(IReadOnlyList<int> blockStarts, IReadOnlyList<Editor.TextRange> invalidRanges)
        {
            var starts = blockStarts.OrderBy(x => x).ToList();

            for (int iRange = 0, iBlock = 0; iRange < invalidRanges.Count && iBlock < starts.Count;)
            {
                var range = invalidRanges[iRange];
                var blockStart = starts[iBlock];
                if (range.End <= blockStart)
                {
                    // range is before block
                    iRange++;
                }
                else if (range.Start >= blockStart)
                {
                    // range is after block
                    iBlock++;
                }
                else
                {
                    // range overlaps block start
                    starts.RemoveAt(iBlock);
                }
            }

            return starts;
        }

        /// <summary>
        /// Produces a list of ordered invalid ranges where blocks may not start in a Kusto multi-block document.
        /// </summary>
        public static IReadOnlyList<Editor.TextRange> GetInvalidKustoBlockStartRanges(string text)
        {
            var invalidRanges = new List<Editor.TextRange>();

            // look for invalid places for blocks to start
            for (int i = 0, n = text.Length; i < n;)
            {
                // skip over strings in case they contain blank lines
                // or they may contain characters that would otherwise appear to be the start
                // of multi-line string
                int strlen = TokenParser.ScanStringLiteral(text, i);
                if (strlen > 0)
                {
                    invalidRanges.Add(new Editor.TextRange(i, strlen));
                    i += strlen;
                    continue;
                }
                else
                {
                    // skip over comments as they may contain characters that appear to be the start
                    // of a multi-line string
                    int commentLen = TokenParser.ScanComment(text, i);
                    if (commentLen > 0)
                    {
                        invalidRanges.Add(new Editor.TextRange(i, commentLen));
                        i += commentLen;
                        continue;
                    }
                }

                i += 1;
            }

            return invalidRanges;
        }

    }
}
