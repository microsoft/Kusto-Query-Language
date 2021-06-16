using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Utils;

    /// <summary>
    /// A script that contains a sequence of independent <see cref="CodeBlock"/>s,
    /// each with its own <see cref="CodeService"/> for intellisense and editor related features.
    /// </summary>
    public sealed class CodeScript
    {
        /// <summary>
        /// The text that contains the sequence of <see cref="CodeBlock"/>s.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// The factory used to construct a <see cref="CodeService"/> for each block.
        /// </summary>
        public CodeServiceFactory Factory { get; }

        /// <summary>
        /// The collection of individual <see cref="CodeBlock"/>s.
        /// </summary>
        public IReadOnlyList<CodeBlock> Blocks => blocks;

        private readonly List<int> lineStarts;
        private readonly List<CodeBlock> blocks;

        private CodeScript(
            string text, 
            List<int> lineStarts,
            List<CodeBlock> blocks,
            CodeServiceFactory factory)
        {
            this.Text = text;
            this.lineStarts = lineStarts;
            this.blocks = blocks;
            this.Factory = factory;
        }

        /// <summary>
        /// Create a new <see cref="CodeScript"/> from the specified text and a <see cref="CodeServiceFactory"/>
        /// </summary>
        public static CodeScript From(string text, CodeServiceFactory factory)
        {
            return CreateScript(text ?? "", factory);
        }

        /// <summary>
        /// Creates a new <see cref="CodeScript"/> with the text changed.
        /// </summary>
        public CodeScript WithText(string newText)
        {
            // reuse any existing blocks and their queries that have not changed.
            return CreateScript(newText ?? "", this.Factory, this.Blocks);
        }

        /// <summary>
        /// Creates a new <see cref="CodeScript"/> with the <see cref="CodeServiceFactory"/> changed.
        /// </summary>
        public CodeScript WithFactory(CodeServiceFactory factory)
        {
            if (factory != this.Factory)
            {
                return CreateScript(this.Text, factory, null);
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Creates a new <see cref="CodeScript"/> with the text and globals changed.
        /// </summary>
        public CodeScript WithTextAndFactory(string newText, CodeServiceFactory factory)
        {
            return WithText(newText).WithFactory(factory);
        }

        #region Helpers for Kusto
        /// <summary>
        /// Create a new <see cref="CodeScript"/> from the specified text and globals.
        /// </summary>
        public static CodeScript From(string text, GlobalState globals)
        {
            return From(text, new KustoCodeServiceFactory(globals));
        }

        /// <summary>
        /// The <see cref="GlobalState"/> used by Kusto <see cref="CodeBlock"/>s.
        /// </summary>
        public GlobalState Globals => this.Factory.GetFactory<KustoCodeServiceFactory>()?.Globals;

        /// <summary>
        /// Creates a new <see cref="CodeScript"/> with the kusto globals changed.
        /// </summary>
        public CodeScript WithGlobals(GlobalState newGlobals)
        {
            var kustoFactory = this.Factory.GetFactory<KustoCodeServiceFactory>();
            if (kustoFactory != null)
            {
                return WithFactory(this.Factory.WithFactory(kustoFactory.WithGlobals(newGlobals)));
            }

            return this;
        }
        #endregion

        /// <summary>
        /// Creates a <see cref="CodeScript"/> from the text
        /// </summary>
        private static CodeScript CreateScript(
            string text,
            CodeServiceFactory factory,
            IEnumerable<CodeBlock> existingBlocks = null)
        {
            var lineStarts = new List<int>();
            var blockStarts = new List<int>();
            GetStarts(text, lineStarts, blockStarts);

            var existingBlockMap = existingBlocks != null
                ? existingBlocks.ToTextKeyedDictionary(b => b.Text, b => b)
                : null;

            var blocks = new List<CodeBlock>();
            for (int i = 0; i < blockStarts.Count; i++)
            {
                var start = blockStarts[i];
                var length = i + 1 < blockStarts.Count ? blockStarts[i + 1] - start : text.Length - start;

                if (existingBlockMap != null && existingBlockMap.TryGetValue(text, start, length, out var block))
                {
                    block = block.WithStart(start);
                }
                else
                {
                    var blockText = text.Substring(start, length);

                    if (!factory.TryGetCodeService(blockText, out var service))
                    {
                        service = new UnknownCodeService(blockText);
                    }

                    block = new CodeBlock(start, service);
                }

                blocks.Add(block);
            }

            return new CodeScript(text, lineStarts, blocks, factory);
        }

        /// <summary>
        /// Gets the starting offset of all the lines and script blocks.
        /// </summary>
        private static void GetStarts(string text, List<int> lineStarts, List<int> blockStarts)
        {
            lineStarts.Add(0);
            blockStarts.Add(0);

            bool allWhitespace = true; // until proven otherwise
            bool newBlockNextWhitespaceLine = false; // no prior block
            bool newBlockNextNonWhitespaceLine = false; // already added first block
            int lineStart = 0;
            int skipToEnd = 0;  // region to ignore linebreaks informing block breaks

            for (int i = 0, n = text.Length; i < n;)
            {
                var lb = Parsing.TextFacts.GetLineBreakLength(text, i);
                if (lb > 0)
                {
                    i += lb;

                    // next block start happens after one all whitespace line gap
                    if (allWhitespace && i > skipToEnd)
                    {
                        if (newBlockNextWhitespaceLine)
                        {
                            // this is a one line empty block
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
                    else
                    {
                        // first gap line belongs to prior block
                        newBlockNextWhitespaceLine = false;
                    }

                    lineStart = i;
                    lineStarts.Add(lineStart);
                    allWhitespace = true;
                    continue;
                }

                if (i >= skipToEnd)
                {
                    // skip over strings in case they contain blank lines
                    // or they may contain characters that would otherwise appear to be the start
                    // of multi-line string
                    int strlen = Parsing.TokenParser.ScanStringLiteral(text, i);
                    if (strlen > 0)
                    {
                        skipToEnd = i + strlen;
                    }
                    else
                    {
                        // skip over comments as they may contain characters that appear to be the start
                        // of a multi-line string
                        int commentLen = Parsing.TokenParser.ScanComment(text, i);
                        if (commentLen > 0)
                        {
                            skipToEnd = i + commentLen;
                        }
                    }
                }

                if (!char.IsWhiteSpace(text[i]))
                {
                    i++;
                    allWhitespace = false;
                }
                else
                {
                    i++;
                }
            }

            // end case
            if ((allWhitespace && newBlockNextWhitespaceLine) ||
                 (!allWhitespace && newBlockNextNonWhitespaceLine))
            {
                blockStarts.Add(lineStart);
            }
        }

        /// <summary>
        /// Gets the block corresponding to the text position.
        /// </summary>
        public CodeBlock GetBlockAtPosition(int position)
        {
            if (position == 0)
            {
                // first block
                return this.Blocks[0];
            }
            else if (position == this.Text.Length)
            {
                // last block
                return this.Blocks[this.Blocks.Count - 1];
            }
            else if (position >= 0 && position < this.Text.Length)
            {
                var index = this.blocks.BinarySearch(b => position < b.Start ? 1 : position >= b.End ? -1 : 0);
                return this.Blocks[index];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the text position for the line number and character offset.
        /// Returns true if the position is determined, or false if the line/character is outside the text.
        /// </summary>
        /// <param name="line">The line number (1 based)</param>
        /// <param name="lineOffset">The offset of the character in the line (1 based)</param>
        /// <param name="position">The position in the text.</param>
        public bool TryGetTextPosition(int line, int lineOffset, out int position)
        {
            line -= 1;
            lineOffset -= 1;

            position = 0;

            if (line < 0 || line >= this.lineStarts.Count)
                return false;

            var lineStart = this.lineStarts[line];
            var lineEnd = (line < this.lineStarts.Count - 1) 
                ? this.lineStarts[line + 1] 
                : this.Text.Length;

            // don't include line break characters in line length
            while (lineEnd > lineStart && Parsing.TextFacts.GetLineBreakLength(this.Text, lineEnd - 1) > 0)
            {
                lineEnd--;
            }

            var lineLength = lineEnd - lineStart;

            // allow character one position beyond end of line to represent line break or end of text
            // any virtual character beyond this is considered outside the text.
            if (lineOffset < lineLength + 1)
            {
                position = lineStart + lineOffset;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the 1-based line and lineOffset for a position in the text.
        /// </summary>
        public bool TryGetLineAndOffset(int position, out int line, out int lineOffset)
        {
            return Parsing.TextFacts.TryGetLineAndOffset(this.lineStarts, position, out line, out lineOffset);
        }

        public IReadOnlyList<int> LineStarts => this.lineStarts;
    }
}