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
            // compare first character before the full comparison (perf)
            var offset = _offset + start;
            return text.Length > 0
                && text.Length <= _end - offset
                && _source[offset] == text[0]
                && CultureUtilities.Equals(_source, offset, text, 0, text.Length);
        }

        public bool Matches(int start, string text, bool ignoreCase)
        {
            var offset = _offset + start;
            return text.Length <= _end - offset
                && CultureUtilities.Equals(_source, offset, text, 0, text.Length, ignoreCase);
        }

        /// <summary>
        ///  The current position within the source text.
        /// </summary>
        public int Position => this._offset;
    }
}