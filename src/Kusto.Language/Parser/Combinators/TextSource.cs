using System;
using System.Collections;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    using Utils;

    /// <summary>
    /// A source of text for syntax parsing
    /// </summary>
    public sealed class TextSource : Source<char>
    {
        private readonly string source;
        private int offset;
        private int end;
        private StringTable strings;

        public TextSource(string source, int offset, int length)
        {
            this.source = source;
            this.offset = offset;
            this.end = offset + length;
        }

        public TextSource(string source)
            : this(source, 0, source.Length)
        {
        }

        public override char Peek(int n = 0)
        {
            return this.offset + n < this.end ? this.source[this.offset + n] : '\0';
        }

        public override bool IsEnd(int n = 0)
        {
            return this.offset + n >= this.end;
        }

        /// <summary>
        /// Eat the specified number of characters from the input.
        /// </summary>
        public override void Eat(int n)
        {
            if (this.offset < this.end)
            {
                this.offset += n;
            }
        }

        public string PeekText(int length)
        {
            return PeekText(0, length);
        }

        public string PeekText(int start, int length)
        {
            if (this.strings == null)
            {
                 this.strings = new StringTable();
            }

            return this.strings.Add(this.source, this.offset + start, length);
        }

        public bool Matches(int start, string text)
        {
            // compare first character before calling string.Compare (perf)
            var offs = this.offset + start;
            return offs < this.source.Length 
                && text.Length > 0 
                && this.source[offs] == text[0]
                && string.Compare(this.source, offs, text, 0, text.Length) == 0;
        }

        public bool Matches(int start, string text, bool ignoreCase)
        {
            return string.Compare(this.source, this.offset + start, text, 0, text.Length, ignoreCase) == 0;
        }

        public string EatText(int length)
        {
            var text = this.PeekText(0, length);
            this.Eat(length);
            return text;
        }

        /// <summary>
        ///  The current position within the source text.
        /// </summary>
        public int Position => this.offset;
    }
}