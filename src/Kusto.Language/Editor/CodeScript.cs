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
        /// An optional function that splits the document text into separate blocks.
        /// If not specified, the blocks are separated by blank lines.
        /// </summary>
        public BlockSeparator Separator { get; }

        /// <summary>
        /// The collection of individual <see cref="CodeBlock"/>s.
        /// </summary>
        public IReadOnlyList<CodeBlock> Blocks => _blocks;

        /// <summary>
        /// The starting positions for all lines in the document.
        /// </summary>
        public IReadOnlyList<int> LineStarts => _lineStarts;

        private readonly List<int> _lineStarts;
        private readonly List<CodeBlock> _blocks;

        private CodeScript(
            string text,
            BlockSeparator separator,
            List<int> lineStarts,
            List<CodeBlock> blocks,
            CodeServiceFactory factory)
        {
            this.Text = text;
            this.Separator = separator;
            this.Factory = factory;
            _lineStarts = lineStarts;
            _blocks = blocks;
        }

        /// <summary>
        /// Create a new <see cref="CodeScript"/> from the specified text and a <see cref="CodeServiceFactory"/>
        /// </summary>
        public static CodeScript From(string text, CodeServiceFactory factory)
        {
            return CreateScript(text ?? "", factory, null, null);
        }

        /// <summary>
        /// Creates a new <see cref="CodeScript"/> with the text changed.
        /// </summary>
        public CodeScript WithText(string newText)
        {
            // reuse any existing blocks and their queries that have not changed.
            return CreateScript(newText ?? "", this.Factory, this.Blocks, this.Separator);
        }

        /// <summary>
        /// Creates a new <see cref="CodeScript"/> with the <see cref="CodeServiceFactory"/> changed.
        /// </summary>
        public CodeScript WithFactory(CodeServiceFactory factory)
        {
            if (factory != this.Factory)
            {
                return CreateScript(this.Text, factory, null, this.Separator);
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Creates a new <see cref="CodeScript"/> with the optional block separator changed.
        /// This function produces a list of text positions that start new blocks.
        /// </summary>
        public CodeScript WithSeparator(BlockSeparator separator)
        {
            if (separator != this.Separator)
            {
                return CreateScript(this.Text, this.Factory, this.Blocks, separator);
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
        public static CodeScript From(string text, GlobalState globals = null)
        {
            return From(text, new KustoCodeServiceFactory(globals ?? GlobalState.Default));
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
            IEnumerable<CodeBlock> existingBlocks,
            BlockSeparator separator)
        {
            var lineStarts = new List<int>();
            Parsing.TextFacts.GetLineStarts(text, lineStarts);

            var blockStarts = separator?.Invoke(text, lineStarts)
                ?? Parsing.ScriptFacts.GetKustoBlockStarts(text, lineStarts);

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

            return new CodeScript(text, separator, lineStarts, blocks, factory);

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
                var index = _blocks.BinarySearch(b => position < b.Start ? 1 : position >= b.End ? -1 : 0);
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

            if (line < 0 || line >= _lineStarts.Count)
                return false;

            var lineStart = _lineStarts[line];
            var lineEnd = (line < _lineStarts.Count - 1) 
                ? _lineStarts[line + 1] 
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
            return Parsing.TextFacts.TryGetLineAndOffset(_lineStarts, position, out line, out lineOffset);
        }
    }

    /// <summary>
    /// A function that produces a list of positions that start new blocks.
    /// </summary>
    public delegate IReadOnlyList<int> BlockSeparator(string text, IReadOnlyList<int> lineStarts);
}