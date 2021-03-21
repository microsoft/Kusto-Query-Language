using System;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    public class ArraySource<TInput> : Source<TInput>
    {
        private readonly IReadOnlyList<TInput> input;
        private int offset;
        private int end;

        public ArraySource(IReadOnlyList<TInput> input, int start = 0, int length = -1)
        {
            this.input = input;
            this.offset = start;

            if (length >= 0)
            {
                this.end = start + Math.Min(length, input.Count - start);
            }
            else
            {
                this.end = input.Count;
            }
        }

        public override TInput Peek(int n)
        {
            if (offset + n < this.end)
            {
                return input[n + offset];
            }
            else
            {
                return default(TInput);
            }
        }

        public override bool IsEnd(int n = 0)
        {
            return offset + n >= this.end;
        }

        public override void Eat(int n)
        {
            offset += n;
        }
    }
}