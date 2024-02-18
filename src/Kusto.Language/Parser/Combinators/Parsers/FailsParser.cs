using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    /// <summary>
    /// A parser that succeeds if the specified Pattern parser fails.
    /// </summary>
    public sealed class FailsParser<TInput> : Parser<TInput>
    {
        public Parser<TInput> Pattern { get; }

        public FailsParser(Parser<TInput> pattern)
        {
            this.Pattern = pattern;
        }

        protected override Parser<TInput> Clone()
        {
            return new FailsParser<TInput>(this.Pattern);
        }

        public override bool IsNegation => true;

        public override int ChildParserCount => 1;

        public override Parser<TInput> GetChildParser(int index)
        {
            return index == 0 ? this.Pattern : null;
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitFails(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitFails(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitFails(this, arg);
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            return this.Scan(source, inputStart);
        }

        public override int Scan(Source<TInput> source, int start)
        {
            // At end is a succeess here
            if (source.IsEnd(start))
                return 0;

            if (this.Pattern.Scan(source, start) >= 0)
                return -1; // if pattern scan succeeds then this scan fails

            return 0;
        }
    }
}