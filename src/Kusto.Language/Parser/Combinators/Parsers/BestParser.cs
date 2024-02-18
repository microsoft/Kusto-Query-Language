using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Parsing
{
    using System;
    using Utils;

    public sealed class BestParser<TInput> : Parser<TInput>
    {
        private readonly Parser<TInput>[] _parsers;
        public IReadOnlyList<Parser<TInput>> Parsers => _parsers;

        public BestParser(IReadOnlyList<Parser<TInput>> parsers)
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
            visitor.VisitBest(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitBest(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitBest(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new BestParser<TInput>(this.Parsers);
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            // must be true: until this is rewritten for use as RightParser
            Ensure.AreEqual(output.Count, outputStart);

            int minLength = -1;
            int maxLength = -1;
            int bestParser = -1;

            // figure out which parser will consume most input
            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                var length = parser.Scan(source, inputStart);

                if (length > maxLength)
                {
                    maxLength = length;
                    bestParser = i;
                }
                else if (length < minLength)
                {
                    minLength = length;
                }
            }

            if (maxLength >= 0)
            {
                _parsers[bestParser].Parse(source, inputStart, output, outputStart);
            }

            return maxLength;
        }

        public override int Scan(Source<TInput> source, int start)
        {
            int max = -1;
            int min = -1;

            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                var n = parser.Scan(source, start);
                if (n > max)
                    max = n;
                else if (n < min)
                    min = n;
            }

            return max >= 0 ? max : min;
        }
    }

    public sealed class BestParser<TInput, TOutput> : Parser<TInput, TOutput>
    {
        private readonly Parser<TInput, TOutput>[] _parsers;
        private readonly Func<TOutput, TOutput, bool> _fnIsBetter;

        public IReadOnlyList<Parser<TInput, TOutput>> Parsers => _parsers;
        public Func<TOutput, TOutput, bool> IsBetter => _fnIsBetter;

        public BestParser(
            IReadOnlyList<Parser<TInput, TOutput>> parsers,
            Func<TOutput, TOutput, bool> fnIsBetter = null)
        {
            Ensure.ArgumentNotNull(parsers, nameof(parsers));
            Ensure.ElementsNotNull(parsers, nameof(parsers));
            _parsers = parsers.ToArray();
            _fnIsBetter = fnIsBetter;
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
            visitor.VisitBest(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitBest(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitBest(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new BestParser<TInput, TOutput>(this.Parsers);
        }

        public override ParseResult<TOutput> Parse(Source<TInput> source, int start)
        {
            int minLength = -1;
            int maxLength = -1;
            int bestParser = -1;
            List<Parser<TInput, TOutput>> candidates = null;

            // figure out which parser will consume most input
            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                var length = parser.Scan(source, start);

                if (length > maxLength)
                {
                    maxLength = length;
                    bestParser = i;

                    if (candidates != null)
                    {
                        candidates.Clear();
                    }
                }
                else if (length == maxLength && bestParser >= 0 && _fnIsBetter != null)
                {
                    if (candidates == null)
                    {
                        candidates = new List<Parser<TInput, TOutput>>();
                    }

                    candidates.Add(_parsers[i]);
                }
                else if (length < minLength)
                {
                    minLength = length;
                }
            }

            if (maxLength >= 0)
            {
                var bestP = _parsers[bestParser];

                if (candidates != null && candidates.Count > 0)
                {
                    var bestV = bestP.Parse(source, start).Value;

                    for (int i = 0; i < candidates.Count; i++)
                    {
                        var otherV = candidates[i].Parse(source, start).Value;
                        if (_fnIsBetter(otherV, bestV))
                        {
                            bestV = otherV;
                        }
                    }

                    return new ParseResult<TOutput>(maxLength, bestV);
                }
                else
                {
                    var result = bestP.Parse(source, start);
                    return new ParseResult<TOutput>(maxLength, result.Value);
                }
            }
            else
            {
                return new ParseResult<TOutput>(minLength, default(TOutput));
            }
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            int minLength = -1;
            int maxLength = -1;
            int bestParser = -1;
            List<Parser<TInput, TOutput>> candidates = null;

            // figure out which parser will consume most input
            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                var length = parser.Scan(source, inputStart);

                if (length > maxLength)
                {
                    maxLength = length;
                    bestParser = i;

                    if (candidates != null)
                    {
                        candidates.Clear();
                    }
                }
                else if (length == maxLength && bestParser >= 0 && _fnIsBetter != null)
                {
                    if (candidates == null)
                    {
                        candidates = new List<Parser<TInput, TOutput>>();
                    }

                    candidates.Add(_parsers[i]);
                }
                else if (length < minLength)
                {
                    minLength = length;
                }
            }

            if (maxLength >= 0)
            {
                var bestP = _parsers[bestParser];

                if (candidates != null && candidates.Count > 0)
                {
                    bestP.Parse(source, inputStart, output, outputStart);
                    var bestV = (TOutput)output[outputStart];
                    output.SetCount(outputStart);

                    for (int i = 0; i < candidates.Count; i++)
                    {
                        candidates[i].Parse(source, inputStart, output, outputStart);
                        var otherV = (TOutput)output[outputStart];
                        output.SetCount(outputStart);

                        if (_fnIsBetter(otherV, bestV))
                        {
                            bestV = otherV;
                        }
                    }

                    output.Add(bestV);
                }
                else
                {
                    bestP.Parse(source, inputStart, output, outputStart);
                }
            }

            return maxLength;
        }

        public override int Scan(Source<TInput> source, int start)
        {
            int max = -1;
            int min = -1;

            for (int i = 0; i < _parsers.Length; i++)
            {
                var parser = _parsers[i];
                var n = parser.Scan(source, start);
                if (n > max)
                    max = n;
                else if (n < min)
                    min = n;
            }

            return max >= 0 ? max : min;
        }
    }
}