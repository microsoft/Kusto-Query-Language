using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Parsing
{
    using Utils;

    public sealed class RuleParser<TInput, TProducer> : ListPrimaryParser<TInput, TProducer>
    {
        private readonly Parser<TInput>[] _parsers;
        public IReadOnlyList<Parser<TInput>> Parsers => _parsers;

        public Func<List<object>, int, TProducer> ListProducer { get; }
        public Func<Source<TInput>, int, ParseResult<TProducer>> ResultProducer { get; }

        public RuleParser(
            IReadOnlyList<Parser<TInput>> parsers,
            Func<List<object>, int, TProducer> listProducer,
            Func<Source<TInput>, int, ParseResult<TProducer>> resultProducer = null)
        {
            _parsers = parsers.ToArray();
            this.ListProducer = listProducer;
            this.ResultProducer = resultProducer;
        }

        public override bool IsSequence => true;

        public override int ChildParserCount => _parsers.Length;

        public override Parser<TInput> GetChildParser(int index)
        {
            return index >= 0 && index < _parsers.Length ? _parsers[index] : null;
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitRule(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitRule(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitRule(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new RuleParser<TInput, TProducer>(this.Parsers, this.ListProducer, this.ResultProducer);
        }

        public override int Scan(Source<TInput> source, int start)
        {
            var len = 0;

            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                var n = parser.Scan(source, start + len);

                if (n < 0)
                {
                    return n - len;
                }

                len += n;
            }

            return len;
        }

        public override ParseResult<TProducer> Parse(Source<TInput> source, int start)
        {
            if (this.ResultProducer != null)
            {
                return this.ResultProducer(source, start);
            }
            else
            {
                return base.Parse(source, start);
            }
        }

        public override int Parse(Source<TInput> input, int inputStart, List<object> output, int outputStart)
        {
            int length = 0;
            int originalOutputCount = output.Count;

            // invoke each parser in sequence.. if one fails then the whole is in error
            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                int n = parser.Parse(input, inputStart + length, output, output.Count);
                if (n < 0)
                {
                    output.SetCount(originalOutputCount);
                    return n - length;
                }

                length += n;
            }

            var value = this.ListProducer(output, outputStart);
            output.SetCount(outputStart);
            output.Add(value);

            return length;
        }
    }
}