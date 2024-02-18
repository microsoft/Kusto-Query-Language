using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    using Utils;

    public sealed class OneOrMoreParser<TInput> : Parser<TInput>
    {
        public Parser<TInput> Parser { get; }

        public OneOrMoreParser(Parser<TInput> parser)
        {
            Ensure.ArgumentNotNull(parser, nameof(parser));
            this.Parser = parser;
        }

        public override bool IsRepetition => true;

        public override int ChildParserCount => 1;

        public override Parser<TInput> GetChildParser(int index)
        {
            return index == 0 ? this.Parser : null;
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitOneOrMore(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitOneOrMore(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitOneOrMore(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new OneOrMoreParser<TInput>(this.Parser);
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            var firstLen = this.Parser.Parse(source, inputStart, output, output.Count);
            if (firstLen < 0)
            {
                return firstLen;
            }

            var length = firstLen;

            while (true)
            {
                var len = Parser.Parse(source, inputStart + length, output, output.Count);
                if (len <= 0)
                {
                    return length;
                }
                else
                {
                    length += len;
                }
            }
        }

        public override int Scan(Source<TInput> source, int start)
        {
            var n = Parser.Scan(source, start);
            if (n < 0)
            {
                return n;
            }

            var len = n;

            while (true)
            {
                n = Parser.Scan(source, start + len);
                if (n <= 0)
                    break;
                len += n;
            }

            return len;
        }
    }
}