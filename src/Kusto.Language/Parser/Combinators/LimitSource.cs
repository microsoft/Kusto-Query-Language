using System;

namespace Kusto.Language.Parsing
{
    /// <summary>
    /// An input source with an artificially constrained index limit.
    /// </summary>
    public sealed class LimitSource<TInput> : Source<TInput>
    {
        private Source<TInput> _source;
        private int _limit;

        public LimitSource(Source<TInput> source, int limit)
        {
            _source = source;
            _limit = limit;
        }

        public override bool IsEnd(int n = 0)
        {
            return n >= _limit || _source.IsEnd(n);
        }

        public override TInput Peek(int n = 0)
        {
            return _source.Peek(n);
        }
    }
}
