using System;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    using Utils;

    public sealed class ForwardParser<TInput, TOutput> : ListPrimaryParser<TInput, TOutput>
    {
        public Func<Parser<TInput, TOutput>> DeferredParser { get; }

        public ForwardParser(Func<Parser<TInput, TOutput>> deferredParser)
        {
            Ensure.ArgumentNotNull(deferredParser, nameof(deferredParser));
            this.DeferredParser = deferredParser;
        }

        public override bool IsForward => true;

        public override int ChildParserCount => 1;

        public override Parser<TInput> GetChildParser(int index)
        {
            return index == 0 ? DeferredParser() : null;
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitForward(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitForward(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitForward(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new ForwardParser<TInput, TOutput>(this.DeferredParser);
        }

        // Sanitiy check recursive call depth to catch run-away parsing/scanning on deeply nested function calls, etc
        [ThreadStatic]
        private static int s_callDepth;
        private const int MaxCallDepth = 30;

        public override ParseResult<TOutput> Parse(Source<TInput> source, int start)
        {
            try
            {
                s_callDepth++;

                if (s_callDepth > MaxCallDepth)
                {
                    return base.Parse(source, start);
                }

                return this.DeferredParser().Parse(source, start);
            }
            finally
            {
                s_callDepth--;
            }
        }

        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            try
            {
                s_callDepth++;

                if (s_callDepth > MaxCallDepth)
                {
                    return SafeParser.ParseSafe(this.DeferredParser(), source, inputStart, output, outputStart);
                }

                return this.DeferredParser().Parse(source, inputStart, output, outputStart);
            }
            finally
            {
                s_callDepth--;
            }
        }

        public override int Scan(Source<TInput> source, int start)
        {
            try
            {
                s_callDepth++;

                if (s_callDepth > MaxCallDepth)
                {
                    return SafeScanner.ScanSafe(this.DeferredParser(), source, start);
                }

                return this.DeferredParser().Scan(source, start);
            }
            finally
            {
                s_callDepth--;
            }
        }
    }
}