using System;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    public class ArraySource<TInput> : Source<TInput>
    {
        private readonly IReadOnlyList<TInput> input;
        private int offset;

        public ArraySource(IReadOnlyList<TInput> input)
        {
            this.input = input;
        }

        public override TInput Peek(int n)
        {
            if (offset + n < input.Count)
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
            return offset + n >= input.Count;
        }

        public override void Eat(int n)
        {
            offset += n;
        }
    }
}