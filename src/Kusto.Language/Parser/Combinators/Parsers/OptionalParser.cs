using System;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    using Utils;

    public class OptionalParser<TInput, TOutput> : Parser<TInput, TOutput>
    {
        public Parser<TInput, TOutput> Parser { get; }
        public Func<TOutput> Producer { get; }

        public OptionalParser(Parser<TInput, TOutput> parser, Func<TOutput> producer)
        {
            Ensure.ArgumentNotNull(parser, nameof(parser));
            Ensure.ArgumentNotNull(producer, nameof(producer));
            this.Parser = parser;
            this.Producer = producer;
        }

        public override bool IsOptional => true;

        public override int ChildParserCount => 1;

        public override Parser<TInput> GetChildParser(int index)
        {
            return index == 0 ? this.Parser : null;
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitOptional(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitOptional(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitOptional(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new OptionalParser<TInput, TOutput>(this.Parser, this.Producer);
        }

        public override ParseResult<TOutput> Parse(Source<TInput> source, int start)
        {
            var result = this.Parser.Parse(source, start);
            if (result.Length < 0)
            {
                return new ParseResult<TOutput>(0, Producer());
            }
            else
            {
                return result;
            }
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            var originalOutputCount = output.Count;
            int length = Parser.Parse(source, inputStart, output, output.Count);

            if (length < 0 || output.Count == originalOutputCount)
            {
                output.SetCount(originalOutputCount);
                output.Add(Producer());
                return 0;
            }

            return length;
        }

        public override int Scan(Source<TInput> source, int start)
        {
            var n = Parser.Scan(source, start);

            if (n < 0)
            {
                return 0;
            }

            return n;
        }
    }
}