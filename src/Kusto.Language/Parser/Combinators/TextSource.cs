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
        private readonly string _source;
        private int _offset;
        private int _end;
        private StringTable _strings;

        public TextSource(string source, int offset, int length)
        {
            _source = source;
            _offset = offset;
            _end = offset + length;
        }

        public TextSource(string source)
            : this(source, 0, source.Length)
        {
        }

        public override char Peek(int n = 0)
        {
            return _offset + n < _end ? _source[_offset + n] : '\0';
        }

        public override bool IsEnd(int n = 0)
        {
            return _offset + n >= _end;
        }

        public string PeekText(int length)
        {
            return PeekText(0, length);
        }

        public string PeekText(int start, int length)
        {
            if (_strings == null)
            {
                 _strings = new StringTable();
            }

            return _strings.Add(_source, _offset + start, length);
        }

        public bool Matches(int start, string text)
        {
            // compare first character before calling string.Compare (perf)
            var offs = _offset + start;
            return offs < _source.Length 
                && text.Length > 0 
                && _source[offs] == text[0]
                && string.Compare(_source, offs, text, 0, text.Length) == 0;
        }

        public bool Matches(int start, string text, bool ignoreCase)
        {
            return string.Compare(_source, _offset + start, text, 0, text.Length, ignoreCase) == 0;
        }

        /// <summary>
        ///  The current position within the source text.
        /// </summary>
        public int Position => this._offset;
    }
}