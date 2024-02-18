using System;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    using Utils;

    /// <summary>
    /// A parser that converts the tokens that would otherwise be consumed by another parser (Pattern) into a different output.
    /// </summary>
    public sealed class ConvertParser<TInput, TOutput> : ResultPrimaryParser<TInput, TOutput>
    {
        public Parser<TInput> Pattern { get; }
        public SourceProducer<TInput, TOutput> ListProducer { get; }
        public Func<TInput, TOutput> SingleProducer { get; }

        private ConvertParser(
            Parser<TInput> pattern,
            SourceProducer<TInput, TOutput> listProducer,
            Func<TInput, TOutput> singleProducer)
        {
            Ensure.ArgumentNotNull(pattern, nameof(pattern));
            Ensure.IsTrue(listProducer != null || singleProducer != null);

            this.Pattern = pattern;
            this.ListProducer = listProducer;
            this.SingleProducer = singleProducer;
        }

        public ConvertParser(
            Parser<TInput> pattern,
            SourceProducer<TInput, TOutput> producer)
            : this(pattern, producer, null)
        {
        }

        public ConvertParser(
            Parser<TInput> pattern,
            Func<IReadOnlyList<TInput>, TOutput> producer)
            : this(pattern, 
                  (Source<TInput> source, int start, int length) =>
                  {
                      var values = s_inputListPool.AllocateFromPool();
                      try
                      {
                          for (int i = 0; i < length; i++)
                          {
                              values.Add(source.Peek(start + i));
                          }

                          return producer(values);
                      }
                      finally
                      {
                          s_inputListPool.ReturnToPool(values);
                      }
                  }, null)
        {
        }

        public ConvertParser(
            Parser<TInput> pattern,
            Func<TInput, TOutput> producer)
            : this(pattern, null, producer)
        {
        }

        public override int ChildParserCount => 1;

        public override Parser<TInput> GetChildParser(int index)
        {
            return index == 0 ? this.Pattern : null;
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitConvert(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitConvert(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitConvert(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new ConvertParser<TInput, TOutput>(this.Pattern, this.ListProducer, this.SingleProducer);
        }

        public override ParseResult<TOutput> Parse(Source<TInput> source, int start)
        {
            int length = this.Pattern.Scan(source, start);
            if (length < 0)
                return new ParseResult<TOutput>(length, default(TOutput));

            var value = Produce(source, start, length);
            return new ParseResult<TOutput>(length, value);
        }

        /// <summary>
        /// Common pool of input lists: don't allocate temporary lists!
        /// </summary>
        private static readonly ObjectPool<List<TInput>> s_inputListPool =
            new ObjectPool<List<TInput>>(() => new List<TInput>(), list => list.Clear());

        private TOutput Produce(Source<TInput> source, int start, int length)
        {
            if (this.SingleProducer != null)
            {
                return this.SingleProducer(source.Peek(start));
            }
            else
            {
                return this.ListProducer(source, start, length);
            }
        }

        public override int Scan(Source<TInput> source, int start)
        {
            return this.Pattern.Scan(source, start);
        }
    }
}