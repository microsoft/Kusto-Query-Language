using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using Kusto.Language.Editor;
using Kusto.Language.Utils;

namespace Kusto.Language.Parsing
{
    public static class TriviaFacts
    {
        /// <summary>
        /// Gets the span of the comment at the position.
        /// </summary>
        public static bool TryGetCommentSpan(string trivia, int position, out int start, out int length)
        {
            start = 0;
            length = 0;

            for (int p = 0; p < trivia.Length && p <= position; p++)
            {
                length = GetCommentLength(trivia, p);
                if (length > 0)
                {
                    // is position inside or adjacent to this comment?
                    if (position >= p && position <= p + length)
                    {
                        start = p;
                        return true;
                    }
                    else
                    {
                        p += length - 1;
                    }
                }
            }

            return false;
        }

        private static int GetCommentLength(string trivia, int start)
        {
            var end = start;

            // comment starts with non-whitespace
            if (!TextFacts.IsWhitespace(trivia[start]))
            {
                for (; end < trivia.Length; end++)
                {
                    var lbLen = TextFacts.GetLineBreakLength(trivia, end);
                    if (lbLen > 0)
                    {
                        end += lbLen;
                        break;
                    }
                }
            }

            return end - start;
        }
    }
}