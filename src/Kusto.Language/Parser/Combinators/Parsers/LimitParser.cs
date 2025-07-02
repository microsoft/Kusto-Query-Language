using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    /// <summary>
    /// A parser that succeeds if the Limited parser succeeds with only the tokens that the Limiter parser successfully scans.
    /// </summary>
    public sealed class LimitParser<TInput, TOutput> : Parser<TInput, TOutput>
    {
        public readonly Parser<TInput> Limiter;
        public readonly Parser<TInput, TOutput> Limited;

        public LimitParser(Parser<TInput> limiter, Parser<TInput, TOutput> limited)
        {
            this.Limiter = limiter;
            this.Limited = limited;
        }

        public override bool IsConditional => true;

        public override int ChildParserCount => 2;

        public override Parser<TInput> GetChildParser(int index)
        {
            switch (index)
            {
                case 0: return this.Limiter;
                case 1: return this.Limited;
                default: return null;
            }
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitLimit(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitLimit(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitLimit(this, arg);
        }

        public override ParseResult<TOutput> Parse(Source<TInput> input, int inputStart)
        {
            var len = this.Limiter.Scan(input, inputStart);
            if (len >= 0)
            {
                var limitSource = new LimitSource<TInput>(input, inputStart + len);
                return this.Limited.Parse(limitSource, inputStart);
            }
            return new ParseResult<TOutput>(-1, default(TOutput));
        }

        public override int Parse(Source<TInput> input, int inputStart, List<object> output, int outputStart)
        {
            var len = this.Limiter.Scan(input, inputStart);
            if (len >= 0)
            {
                var limitSource = new LimitSource<TInput>(input, inputStart + len);
                return this.Limited.Parse(limitSource, inputStart, output, outputStart);
            }
            return -1;
        }

        public override int Scan(Source<TInput> input, int inputStart)
        {
            var len = this.Limiter.Scan(input, inputStart);
            if (len >= 0)
            {
                var limitSource = new LimitSource<TInput>(input, inputStart + len);
                return this.Limited.Scan(limitSource, inputStart);
            }
            return -1;
        }

        protected override Parser<TInput> Clone()
        {
            return new LimitParser<TInput, TOutput>(this.Limiter, this.Limited);
        }
    }
}