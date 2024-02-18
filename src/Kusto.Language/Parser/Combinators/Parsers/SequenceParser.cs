using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Parsing
{
    using Utils;

    public sealed class SequenceParser<TInput> : Parser<TInput>
    {
        private readonly Parser<TInput>[] _parsers;
        public IReadOnlyList<Parser<TInput>> Parsers => _parsers;

        public SequenceParser(IReadOnlyList<Parser<TInput>> parsers)
        {
            Ensure.ArgumentNotNull(parsers, nameof(parsers));
            Ensure.ElementsNotNull(parsers, nameof(parsers));
            _parsers = parsers.ToArray();
        }

        public override bool IsSequence => true;

        public override int ChildParserCount => _parsers.Length;

        public override Parser<TInput> GetChildParser(int index)
        {
            return index >= 0 && index < _parsers.Length ? _parsers[index] : null;
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitSequence(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitSequence(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitSequence(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new SequenceParser<TInput>(this.Parsers);
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            int length = 0;
            var originalOutputCount = output.Count;

            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                var len = parser.Parse(source, inputStart + length, output, output.Count);

                if (len < 0)
                {
                    output.SetCount(originalOutputCount);
                    return -length + len;
                }

                length += len;
            }

            return length;
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
    }
}