using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Parsing
{
    using Utils;

    public sealed class FirstParser<TInput> : Parser<TInput>
    {
        private readonly Parser<TInput>[] _parsers;
        public IReadOnlyList<Parser<TInput>> Parsers => _parsers;

        public FirstParser(IReadOnlyList<Parser<TInput>> parsers)
        {
            Ensure.ArgumentNotNull(parsers, nameof(parsers));
            Ensure.ElementsNotNull(parsers, nameof(parsers));
            _parsers = parsers.ToArray();
        }

        public override bool IsAlternation => true;

        public override int ChildParserCount => _parsers.Length;

        public override Parser<TInput> GetChildParser(int index)
        {
            if (index >= 0 && index < _parsers.Length)
                return _parsers[index];
            return null;
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitFirst(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitFirst(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitFirst(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new FirstParser<TInput>(this.Parsers);
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputCount)
        {
            int minLength = -1;

            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                var length = parser.Parse(source, inputStart, output, outputCount);
                if (length >= 0)
                {
                    Ensure.IsTrue(length > 0 || i == this.Parsers.Count - 1, "zero consuming parsers should only occur at end");
                    return length;
                }

                if (length < minLength)
                {
                    minLength = length;
                }
            }

            return minLength;
        }

        public override int Scan(Source<TInput> source, int start)
        {
            int minLength = -1;

            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                var n = parser.Scan(source, start);
                if (n >= 0)
                    return n;

                if (n < minLength)
                {
                    minLength = n;
                }
            }

            return minLength;
        }
    }

    public class FirstParser<TInput, TOutput> : Parser<TInput, TOutput>
    {
        private readonly Parser<TInput, TOutput>[] _parsers;
        public IReadOnlyList<Parser<TInput, TOutput>> Parsers => _parsers;

        public FirstParser(IReadOnlyList<Parser<TInput, TOutput>> parsers)
        {
            Ensure.ArgumentNotNull(parsers, nameof(parsers));
            Ensure.ElementsNotNull(parsers, nameof(parsers));
            _parsers = parsers.ToArray();
        }

        public override bool IsAlternation => true;

        public override int ChildParserCount => _parsers.Length;

        public override Parser<TInput> GetChildParser(int index)
        {
            if (index >= 0 && index < _parsers.Length)
                return _parsers[index];
            return null;
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitFirst(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitFirst(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitFirst(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new FirstParser<TInput, TOutput>(this.Parsers);
        }

        public override ParseResult<TOutput> Parse(Source<TInput> source, int start)
        {
            int minLength = -1;

            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                var result = parser.Parse(source, start);

                if (result.Length >= 0)
                {
                    Ensure.IsTrue(result.Length > 0 || i == this.Parsers.Count - 1, "zero consuming parsers should only occur at end");
                    return result;
                }

                if (result.Length < minLength)
                {
                    minLength = result.Length;
                }
            }

            return new ParseResult<TOutput>(minLength, default(TOutput));
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            int minLength = -1;
            var originalOutputCount = output.Count;

            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                output.SetCount(originalOutputCount);

                var length = parser.Parse(source, inputStart, output, outputStart);
                if (length >= 0)
                {
                    return length;
                }

                if (length < minLength)
                {
                    minLength = length;
                }
            }

            return minLength;
        }

        public override int Scan(Source<TInput> source, int start)
        {
            int minLength = -1;

            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                var n = parser.Scan(source, start);
                if (n >= 0)
                    return n;

                if (n < minLength)
                {
                    minLength = n;
                }
            }

            return minLength;
        }
    }
}