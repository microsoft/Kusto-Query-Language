using System;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    using Utils;

    public class ProduceParser<TInput, TProducer> : ListPrimaryParser<TInput, TProducer>
    {
        public Parser<TInput> Parser { get; }
        public Func<List<object>, int, TProducer> Producer { get; }

        public ProduceParser(Parser<TInput> parser, Func<List<object>, int, TProducer> producer)
        {
            Ensure.ArgumentNotNull(parser, nameof(parser));
            Ensure.ArgumentNotNull(producer, nameof(producer));

            this.Parser = parser;
            this.Producer = producer;
        }

        public override int ChildParserCount => 1;

        public override Parser<TInput> GetChildParser(int index)
        {
            return index == 0 ? this.Parser : null;
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitProduce(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitProduce(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitProduce(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new ProduceParser<TInput, TProducer>(this.Parser, this.Producer);
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            int originalOutputCount = output.Count;
            var length = this.Parser.Parse(source, inputStart, output, outputStart);
            return Produce(output, outputStart, originalOutputCount, length);
        }

        private int Produce(List<object> output, int outputStart, int originalOutputCount, int inputLength)
        {
            if (inputLength >= 0)
            {
                var value = this.Producer(output, outputStart);
                output.SetCount(outputStart);
                output.Add(value);
            }
            else
            {
                output.SetCount(originalOutputCount);
            }

            return inputLength;
        }

        public override int Scan(Source<TInput> source, int start)
        {
            return Parser.Scan(source, start);
        }
    }
}