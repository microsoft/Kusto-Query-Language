using System;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    using Utils;

    /// <summary>
    /// A parser that succeeds if it successfully consumes tokens.
    /// </summary>
    public class MatchParser<TInput> : Parser<TInput>
    {
        public SourceConsumer<TInput> Consumer { get; }

        public MatchParser(SourceConsumer<TInput> consumer)
        {
            this.Consumer = consumer;
        }

        public MatchParser(Func<TInput, bool> predicate)
            : this((source, start) => !source.IsEnd(start) && predicate(source.Peek(start)) ? 1 : -1)
        {
        }

        public override bool IsMatch => true;

        public override int ChildParserCount => 0;

        public override Parser<TInput> GetChildParser(int index)
        {
            return null;
        }

        protected override Parser<TInput> Clone()
        {
            return new MatchParser<TInput>(this.Consumer);
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitMatch(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitMatch(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitMatch(this, arg);
        }

        public override int Scan(Source<TInput> source, int start)
        {
            return this.Consumer(source, start);
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            return Scan(source, inputStart);
        }
    }

    /// <summary>
    /// A parser that succeeds if it successfully consumes tokens.
    /// The consumed tokens are converted into a produced values.
    /// </summary>
    public class MatchParser<TInput, TOutput> : Parser<TInput, TOutput>
    {
        public SourceConsumer<TInput> Consumer { get; }
        public SourceProducer<TInput, TOutput> Producer { get; }

        public MatchParser(SourceConsumer<TInput> consumer, SourceProducer<TInput, TOutput> producer)
        {
            Ensure.ArgumentNotNull(consumer, nameof(consumer));
            Ensure.ArgumentNotNull(producer, nameof(producer));

            this.Consumer = consumer;
            this.Producer = producer;
        }

        public MatchParser(Func<TInput, bool> predicate, Func<TInput, TOutput> producer)
            : this(
                  (source, start) => !source.IsEnd(start) && predicate(source.Peek(start)) ? 1 : -1,
                  (source, start, length) => producer(source.Peek(start)))
        {
        }

        public override bool IsMatch => true;

        public override int ChildParserCount => 0;

        public override Parser<TInput> GetChildParser(int index)
        {
            return null;
        }

        protected override Parser<TInput> Clone()
        {
            return new MatchParser<TInput, TOutput>(this.Consumer, this.Producer);
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitMatch(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitMatch(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitMatch(this, arg);
        }

        public override int Scan(Source<TInput> source, int start)
        {
            return this.Consumer(source, start);
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            var length = this.Consumer(source, inputStart);

            if (length >= 0)
            {
                var value = this.Producer(source, inputStart, length);
                output.Add(value);
            }

            return length;
        }

        public override ParseResult<TOutput> Parse(Source<TInput> source, int start = 0)
        {
            var length = this.Consumer(source, start);

            if (length > 0)
            {
                var value = this.Producer(source, start, length);
                return new ParseResult<TOutput>(length, value);
            }

            return new ParseResult<TOutput>(length, default(TOutput));
        }
    }
}