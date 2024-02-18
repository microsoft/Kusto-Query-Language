using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    using Utils;

    /// <summary>
    /// A parser that succeeds if both the Test parser scans and Parser parsers succeed.
    /// </summary>
    public sealed class IfParser<TInput> : Parser<TInput>
    {
        public Parser<TInput> Test { get; }
        public Parser<TInput> Parser { get; }

        public IfParser(Parser<TInput> test, Parser<TInput> parser)
        {
            Ensure.ArgumentNotNull(test, nameof(test));
            Ensure.ArgumentNotNull(parser, nameof(parser));

            this.Test = test;
            this.Parser = parser;
        }

        public override bool IsConditional => true;

        public override int ChildParserCount => 2;

        public override Parser<TInput> GetChildParser(int index)
        {
            switch (index)
            {
                case 0: return this.Test;
                case 1: return this.Parser;
                default: return null;
            }
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitIf(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitIf(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitIf(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new IfParser<TInput>(this.Test, this.Parser);
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            var length = this.Test.Scan(source, inputStart);
            if (length < 0)
                return length;

            return this.Parser.Parse(source, inputStart, output, outputStart);
        }

        public override int Scan(Source<TInput> source, int start)
        {
            var length = this.Test.Scan(source, start);
            if (length < 0)
                return length;

            return this.Parser.Scan(source, start);
        }
    }

    /// <summary>
    /// A parser that succeeds if both the Test parser scans and Parser parsers succeed.
    /// </summary>
    public sealed class IfParser<TInput, TOutput> : Parser<TInput, TOutput>
    {
        public Parser<TInput> Test { get; }
        public Parser<TInput, TOutput> Parser { get; }

        public IfParser(Parser<TInput> test, Parser<TInput, TOutput> parser)
        {
            Ensure.ArgumentNotNull(test, nameof(test));
            Ensure.ArgumentNotNull(parser, nameof(parser));
            this.Test = test;
            this.Parser = parser;
        }

        public override bool IsConditional => true;

        public override int ChildParserCount => 2;

        public override Parser<TInput> GetChildParser(int index)
        {
            switch (index)
            {
                case 0: return this.Test;
                case 1: return this.Parser;
                default: return null;
            }
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitIf(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitIf(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitIf(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new IfParser<TInput, TOutput>(this.Test, this.Parser);
        }

        public override ParseResult<TOutput> Parse(Source<TInput> source, int start)
        {
            var length = this.Test.Scan(source, start);
            if (length < 0)
                return new ParseResult<TOutput>(length, default(TOutput));

            return this.Parser.Parse(source, start);
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            var length = this.Test.Scan(source, inputStart);

            if (length < 0)
                return length;

            return this.Parser.Parse(source, inputStart, output, outputStart);
        }

        public override int Scan(Source<TInput> source, int start)
        {
            var length = this.Test.Scan(source, start);
            if (length < 0)
                return length;

            return this.Parser.Scan(source, start);
        }
    }
}