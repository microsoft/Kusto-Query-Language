using System;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    using Utils;

    public sealed class RequiredParser<TInput, TOutput> : Parser<TInput, TOutput>
    {
        public Parser<TInput, TOutput> Parser { get; }
        public Func<Source<TInput>, int, TOutput> Producer { get; }

        public RequiredParser(Parser<TInput, TOutput> parser, Func<Source<TInput>, int, TOutput> producer)
        {
            Ensure.ArgumentNotNull(parser, nameof(parser));
            Ensure.ArgumentNotNull(producer, nameof(producer));
            this.Parser = parser;
            this.Producer = producer;
        }

        public override bool IsRequired => true;

        public override int ChildParserCount => 1;

        public override Parser<TInput> GetChildParser(int index)
        {
            return index == 0 ? this.Parser : null;
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitRequired(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitRequired(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitRequired(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new RequiredParser<TInput, TOutput>(this.Parser, this.Producer);
        }

        public override ParseResult<TOutput> Parse(Source<TInput> source, int start)
        {
            var result = Parser.Parse(source, start);
            if (result.Length < 0)
            {
                return new ParseResult<TOutput>(0, this.Producer(source, start));
            }
            else
            {
                return result;
            }
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            var originalOutputCount = output.Count;

            var length = this.Parser.Parse(source, inputStart, output, output.Count);
            if (length < 0 || output.Count == originalOutputCount)
            {
                output.SetCount(originalOutputCount);
                output.Add(this.Producer(source, inputStart));
                return 0;
            }

            return length;
        }

        public override int Scan(Source<TInput> source, int start)
        {
            var len = this.Parser.Scan(source, start);
            return (len < 0) ? 0 : len;
        }
    }
}