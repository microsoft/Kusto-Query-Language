using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Parsing
{
    using Utils;

    public static class ScannerExtensions
    {
        /// <summary>
        /// Determines if the scanner matches the specified text.
        /// </summary>
        public static bool Matches(this Parser<char> scanner, string text)
        {
            return Matches(scanner, text, 0, text.Length);
        }

        /// <summary>
        /// Determines if the scanner matches the specified text.
        /// </summary>
        public static bool Matches(this Parser<char> scanner, string text, int offset, int length)
        {
            var source = s_sourcePool.AllocateFromPool();
            try
            {
                source.Init(text);
                int len = scanner.Scan(source, offset);
                return len == length; // must scan all characters
            }
            finally
            {
                s_sourcePool.ReturnToPool(source);
            }
        }

        private static ObjectPool<ReuseableTextSource> s_sourcePool =
            new ObjectPool<ReuseableTextSource>(() => new ReuseableTextSource(), source => source.Clear());

        /// <summary>
        /// A source of text for syntax parsing
        /// </summary>
        private sealed class ReuseableTextSource : Source<char>
        {
            private string _source = string.Empty;
            private int _offset;
            private int _end;

            public ReuseableTextSource()
            {
            }

            public void Init(string source, int offset, int length)
            {
                _source = source;
                _offset = offset;
                _end = offset + length;
            }

            public void Init(string source)
            {
                Init(source, 0, source.Length);
            }

            public void Clear()
            {
                _source = string.Empty;
                _offset = 0;
                _end = 0;
            }

            public override char Peek(int n = 0)
            {
                return _offset + n < _end ? _source[_offset + n] : '\0';
            }

            public override bool IsEnd(int n = 0)
            {
                return _offset + n >= _end;
            }
        }
    }
}