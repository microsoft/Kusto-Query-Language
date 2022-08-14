using System;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    /// <summary>
    /// An input source based on an array or input items.
    /// </summary>
    public sealed class ArraySource<TInput> : Source<TInput>
    {
        private readonly IReadOnlyList<TInput> _input;
        private int _offset;
        private int _end;

        public ArraySource(IReadOnlyList<TInput> input, int start = 0, int length = -1)
        {
            _input = input;
            _offset = start;

            if (length >= 0)
            {
                _end = start + Math.Min(length, input.Count - start);
            }
            else
            {
                _end = input.Count;
            }
        }

        public override TInput Peek(int n)
        {
            if (_offset + n < _end)
            {
                return _input[n + _offset];
            }
            else
            {
                return default(TInput);
            }
        }

        public override bool IsEnd(int n = 0)
        {
            return _offset + n >= _end;
        }
    }
}