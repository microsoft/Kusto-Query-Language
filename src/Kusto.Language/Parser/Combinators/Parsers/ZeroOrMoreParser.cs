using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    using Utils;

    public sealed class ZeroOrMoreParser<TInput> : Parser<TInput>
    {
        public Parser<TInput> Parser { get; }

        public bool ZeroOrOne { get; }

        public ZeroOrMoreParser(Parser<TInput> parser, bool zeroOrOne = false)
        {
            Ensure.ArgumentNotNull(parser, nameof(parser));
            this.Parser = parser;
            this.ZeroOrOne = zeroOrOne;
        }

        public override bool IsOptional => true;
        public override bool IsRepetition => true;

        public override int ChildParserCount => 1;

        public override Parser<TInput> GetChildParser(int index)
        {
            return index == 0 ? this.Parser : null;
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitZeroOrMore(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitZeroOrMore(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitZeroOrMore(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new ZeroOrMoreParser<TInput>(this.Parser, this.ZeroOrOne);
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            var length = 0;

            while (true)
            {
                var len = Parser.Parse(source, inputStart + length, output, output.Count);
                if (len > 0)
                {
                    length += len;
                }

                if (len <= 0 || this.ZeroOrOne)
                    return length;
            }
        }

        public override int Scan(Source<TInput> source, int start)
        {
            var len = 0;

            while (true)
            {
                var n = Parser.Scan(source, start + len);
                if (n > 0)
                {
                    len += n;
                }

                if (n <= 0 || this.ZeroOrOne)
                    break;
            }

            return len;
        }
    }
}