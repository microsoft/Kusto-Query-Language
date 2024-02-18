using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    public sealed class NotParser<TInput> : Parser<TInput>
    {
        public Parser<TInput> Pattern { get; }

        public NotParser(Parser<TInput> parser)
        {
            this.Pattern = parser;
        }

        public override bool IsNegation => true;

        public override int ChildParserCount => 1;

        public override Parser<TInput> GetChildParser(int index)
        {
            return index == 0 ? this.Pattern : null;
        }

        protected override Parser<TInput> Clone()
        {
            return new NotParser<TInput>(this.Pattern);
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitNot(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitNot(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitNot(this, arg);
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            return Scan(source, inputStart);
        }

        public override int Scan(Source<TInput> source, int start)
        {
            // EOF never scans successfully
            if (source.IsEnd(start))
                return -1;

            // if scanning succeeds then fail
            if (this.Pattern.Scan(source, start) >= 0)
                return -1;

            return 1;
        }
    }
}