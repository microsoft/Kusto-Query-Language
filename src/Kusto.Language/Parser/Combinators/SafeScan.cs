using System;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    using Utils;

    public static class SafeScanner
    {
        public static int ScanSafe<TInput>(this Parser<TInput> parser, Source<TInput> source, int start)
        {
            var sss = ScannerPool<TInput>.Pool.AllocateFromPool();
            try
            {
                sss.Initialize(source);
                return sss.Scan(parser, start);
            }
            finally
            {
                ScannerPool<TInput>.Pool.ReturnToPool(sss);
            }
        }

        private static class ScannerPool<TInput>
        {
            public static readonly ObjectPool<StackSafeScanner<TInput>> Pool =
                new ObjectPool<StackSafeScanner<TInput>>(() => new StackSafeScanner<TInput>(null), s => s.Clear());
        }
    }

    internal class StackSafeScanner<TInput> : ParserVisitor<TInput, Parser<TInput>>
    {
        private Source<TInput> source;
        private List<ScanState> stack;
        private int stackPosition;
        private ScanState state;

        public StackSafeScanner(Source<TInput> source)
        {
            this.stack = new List<ScanState>();
            this.stackPosition = -1;
            Initialize(source);
        }

        public void Initialize(Source<TInput> source)
        {
            this.source = source;
        }

        public void Clear()
        {
            this.source = null;
        }

        private class ScanState
        {
            public Parser<TInput> Parser { get; private set; }

            /// <summary>
            /// The input start
            /// </summary>
            public int InputStart { get; private set; }

            /// <summary>
            /// The accumulated input length consumed by this parser
            /// </summary>
            public int InputLength;

            /// <summary>
            /// The parser execution state
            /// </summary>
            public int State;

            /// <summary>
            /// The result of the last sub-parser
            /// </summary>
            public int LastResult;

            /// <summary>
            /// The result length of the best failed parser
            /// </summary>
            public int BestFailedResult;

            /// <summary>
            /// The result length of the best successful parser
            /// </summary>
            public int BestSuccessResult;

            public void Init(Parser<TInput> parser, int inputStart)
            {
                this.Parser = parser;
                this.InputStart = inputStart;
                this.InputLength = 0;
                this.State = 0;
                this.LastResult = 0;
                this.BestFailedResult = 0;
                this.BestSuccessResult = 0;
            }
        }

        private void Push(Parser<TInput> parser, int inputStart)
        {
            this.stackPosition++;

            if (this.stackPosition == this.stack.Count)
            {
                this.stack.Add(new ScanState());
            }

            this.state = this.stack[this.stackPosition];
            this.state.Init(parser, inputStart);
        }

        private void Pop()
        {
            this.stackPosition--;

            if (this.stackPosition >= 0)
            {
                this.state = this.stack[this.stackPosition];
            }
        }

        /// <summary>
        /// Parse using a state machine, does not use the call stack.
        /// </summary>
        public int Scan(Parser<TInput> parser, int start)
        {
            this.state = default(ScanState);
            this.stackPosition = -1;

            Push(parser, start);

            while (true)
            {
                var nextParser = this.state.Parser.Accept(this);

                if (nextParser != null)
                {
                    Push(nextParser, inputStart: this.state.InputStart + this.state.InputLength);
                }
                else
                {
                    var result = this.state.InputLength;

                    if (this.stackPosition == 0)
                    {
                        return result;
                    }
                    else
                    {
                        Pop();
                        this.state.LastResult = result;
                    }
                }
            }
        }

        public override Parser<TInput> VisitApply<TLeft, TOutput>(ApplyParser<TInput, TLeft, TOutput> parser)
        {
            if (state.State == 0)
            {
                state.State = 1;
                return parser.LeftParser;
            }
            else if (state.State == 1)
            {
                if (state.LastResult < 0)
                {
                    if (parser.ApplyKind == ApplyKind.One)
                    {
                        state.InputLength = -state.InputLength + state.LastResult;
                    }
                    else
                    {
                        state.InputLength = state.LastResult;
                    }

                    return null;
                }
                else
                {
                    state.InputLength = state.LastResult;
                    state.State = 2;
                    return parser.RightParser;
                }
            }
            else
            {
                if (state.LastResult > 0)
                {
                    state.InputLength += state.LastResult;

                    if (parser.ApplyKind == ApplyKind.ZeroOrMore)
                    {
                        return parser.RightParser;
                    }
                }

                return null;
            }
        }

        public override Parser<TInput> VisitBest(BestParser<TInput> parser)
        {
            if (state.State == 0)
            {
                state.BestFailedResult = -1;
                state.BestSuccessResult = -1;
                state.State = 1;
                return parser.Parsers[0];
            }
            else
            {
                if (state.LastResult > state.BestSuccessResult)
                {
                    state.BestSuccessResult = state.LastResult;
                }
                else if (state.LastResult < state.BestFailedResult)
                {
                    state.BestFailedResult = state.LastResult;
                }

                if (state.State >= parser.Parsers.Count)
                {
                    if (state.BestSuccessResult >= 0)
                    {
                        state.InputLength = state.BestSuccessResult;
                        return null;
                    }
                    else
                    {
                        state.InputLength = state.BestFailedResult;
                        return null;
                    }
                }
                else
                {
                    state.InputLength = 0;
                    var next = parser.Parsers[state.State];
                    state.State++;
                    return next;
                }
            }
        }

        public override Parser<TInput> VisitBest<TOutput>(BestParser<TInput, TOutput> parser)
        {
            if (state.State == 0)
            {
                state.BestFailedResult = -1;
                state.BestSuccessResult = -1;
                state.State = 1;
                return parser.Parsers[0];
            }
            else
            {
                if (state.LastResult > state.BestSuccessResult)
                {
                    state.BestSuccessResult = state.LastResult;
                }
                else if (state.LastResult < state.BestFailedResult)
                {
                    state.BestFailedResult = state.LastResult;
                }

                if (state.State >= parser.Parsers.Count)
                {
                    if (state.BestSuccessResult >= 0)
                    {
                        state.InputLength = state.BestSuccessResult;
                        return null;
                    }
                    else
                    {
                        state.InputLength = state.BestFailedResult;
                        return null;
                    }
                }
                else
                {
                    state.InputLength = 0;
                    var next = parser.Parsers[state.State];
                    state.State++;
                    return next;
                }
            }
        }

        public override Parser<TInput> VisitConvert<TOutput>(ConvertParser<TInput, TOutput> parser)
        {
            if (state.State == 0)
            {
                state.State = 1;
                return parser.Pattern;
            }
            else
            {
                state.InputLength = state.LastResult;
                return null;
            }
        }

        public override Parser<TInput> VisitFails(FailsParser<TInput> parser)
        {
            if (state.State == 0)
            {
                if (source.IsEnd(state.InputStart))
                {
                    state.InputLength = 0;
                    return null;
                }
                else
                {
                    state.State++;
                    return parser.Pattern;
                }
            }
            else
            {
                if (state.LastResult >= 0)
                {
                    state.InputLength = -1;
                }
                else
                {
                    state.InputLength = 0;
                }

                return null;
            }
        }

        public override Parser<TInput> VisitFirst<TOutput>(FirstParser<TInput, TOutput> parser)
        {
            if (state.State == 0)
            {
                state.State = 1;
                return parser.Parsers[0];
            }
            else if (state.LastResult < 0)
            {
                if (state.LastResult < state.BestFailedResult)
                {
                    state.BestFailedResult = state.LastResult;
                }

                if (state.State < parser.Parsers.Count)
                {
                    var next = parser.Parsers[state.State];
                    state.State++;
                    return next;
                }
                else
                {
                    state.InputLength = state.BestFailedResult;
                    return null;
                }
            }
            else
            {
                state.InputLength = state.LastResult;
                return null;
            }
        }

        public override Parser<TInput> VisitFirst(FirstParser<TInput> parser)
        {
            if (state.State == 0)
            {
                state.State = 1;
                return parser.Parsers[0];
            }
            else if (state.LastResult < 0)
            {
                if (state.LastResult < state.BestFailedResult)
                {
                    state.BestFailedResult = state.LastResult;
                }

                if (state.State < parser.Parsers.Count)
                {
                    var next = parser.Parsers[state.State];
                    state.State++;
                    return next;
                }
                else
                {
                    state.InputLength = state.BestFailedResult;
                    return null;
                }
            }
            else
            {
                state.InputLength = state.LastResult;
                return null;
            }
        }

        public override Parser<TInput> VisitForward<TOutput>(ForwardParser<TInput, TOutput> parser)
        {
            if (state.State == 0)
            {
                state.State = 1;
                return parser.DeferredParser();
            }
            else
            {
                state.InputLength = state.LastResult;
                return null;
            }
        }

        public override Parser<TInput> VisitIf<TOutput>(IfParser<TInput, TOutput> parser)
        {
            if (state.State == 0)
            {
                state.State = 1;
                return parser.Test;
            }
            else if (state.State == 1)
            {
                var length = state.LastResult;
                if (length < 0)
                {
                    state.InputLength = length;
                    return null;
                }
                else
                {
                    state.State = 2;
                    return parser.Parser;
                }
            }
            else
            {
                state.InputLength = state.LastResult;
                return null;
            }
        }

        public override Parser<TInput> VisitIf(IfParser<TInput> parser)
        {
            if (state.State == 0)
            {
                state.State = 1;
                return parser.Test;
            }
            else if (state.State == 1)
            {
                var length = state.LastResult;
                if (length < 0)
                {
                    state.InputLength = length;
                    return null;
                }
                else
                {
                    state.State = 2;
                    return parser.Parser;
                }
            }
            else
            {
                state.InputLength = state.LastResult;
                return null;
            }
        }

        public override Parser<TInput> VisitMap<TOutput>(MapParser<TInput, TOutput> parser)
        {
            // safe to call scan here because map is limited.
            state.InputLength = parser.Scan(source, state.InputStart);
            return null;
        }

        public override Parser<TInput> VisitMatch(MatchParser<TInput> parser)
        {
            // not proven safe here because scan of MatchParser is user defined
            state.InputLength = parser.Scan(source, state.InputStart);
            return null;
        }

        public override Parser<TInput> VisitMatch<TOutput>(MatchParser<TInput, TOutput> parser)
        {
            // not proven safe here because scan of MatchParser is user defined
            state.InputLength = parser.Scan(source, state.InputStart);
            return null;
        }

        public override Parser<TInput> VisitNot(NotParser<TInput> parser)
        {
            if (state.State == 0)
            {
                if (source.IsEnd(state.InputStart))
                {
                    state.InputLength = -1;
                    return null;
                }
                else
                {
                    state.State++;
                    return parser.Pattern;
                }
            }
            else
            {
                if (state.LastResult >= 0)
                {
                    state.InputLength = -1;
                    return null;
                }
                else
                {
                    state.InputLength = 1;
                    return null;
                }
            }
        }

        public override Parser<TInput> VisitOneOrMore(OneOrMoreParser<TInput> parser)
        {
            if (state.State == 0)
            {
                state.State = 1;
                return parser.Parser;
            }
            else if (state.State == 1 && state.LastResult < 0)
            {
                // first parse did not succeed, fail
                state.InputLength = state.LastResult;
                return null;
            }
            else if (state.LastResult > 0)
            {
                // keep going as long as scanning is successful and consumed input
                state.State++;
                state.InputLength += state.LastResult;
                return parser.Parser;
            }
            else
            {
                return null;
            }
        }

        public override Parser<TInput> VisitOptional<TOutput>(OptionalParser<TInput, TOutput> parser)
        {
            if (state.State == 0)
            {
                state.State = 1;
                return parser.Parser;
            }
            else if (state.LastResult < 0)
            {
                state.InputLength = 0;
                return null;
            }
            else
            {
                state.InputLength = state.LastResult;
                return null;
            }
        }

        public override Parser<TInput> VisitProduce<TOutput>(ProduceParser<TInput, TOutput> parser)
        {
            if (state.State == 0)
            {
                state.State = 1;
                return parser.Parser;
            }
            else
            {
                state.InputLength = state.LastResult;
                return null;
            }
        }

        public override Parser<TInput> VisitRequired<TOutput>(RequiredParser<TInput, TOutput> parser)
        {
            if (state.State == 0)
            {
                state.State = 1;
                return parser.Parser;
            }
            else if (state.LastResult < 0)
            {
                state.InputLength = 0;
                return null;
            }
            else
            {
                state.InputLength = state.LastResult;
                return null;
            }
        }

        public override Parser<TInput> VisitRule<TOutput>(RuleParser<TInput, TOutput> parser)
        {
            if (state.LastResult < 0)
            {
                // last parser failed, so fail the whole rule
                state.InputLength = -state.InputLength + state.LastResult;
                return null;
            }
            else
            {
                state.InputLength += state.LastResult;

                if (state.State >= parser.Parsers.Count)
                {
                    return null;
                }
                else
                {
                    var next = parser.Parsers[state.State];
                    state.State++;
                    return next;
                }
            }
        }

        public override Parser<TInput> VisitSequence(SequenceParser<TInput> parser)
        {
            if (state.LastResult < 0)
            {
                // last parser failed, so fail the whole rule
                state.InputLength = -state.InputLength + state.LastResult;
                return null;
            }
            else
            {
                state.InputLength += state.LastResult;

                if (state.State >= parser.Parsers.Count)
                {
                    return null;
                }
                else
                {
                    var next = parser.Parsers[state.State];
                    state.State++;
                    return next;
                }
            }
        }

        public override Parser<TInput> VisitZeroOrMore(ZeroOrMoreParser<TInput> parser)
        {
            if (state.State == 0 || (state.LastResult > 0 && !parser.ZeroOrOne))
            {
                // keep going as long as scanning is successful and consumed input
                state.State++;
                state.InputLength += state.LastResult;
                return parser.Parser;
            }
            else
            {
                return null;
            }
        }
    }
}