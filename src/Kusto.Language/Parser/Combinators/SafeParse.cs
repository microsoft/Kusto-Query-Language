using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    using System.Diagnostics;
    using Utils;

    public static class SafeParser
    {
        public static int ParseSafe<TInput>(this Parser<TInput> parser, Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            var ssp = ParserPool<TInput>.Pool.AllocateFromPool();
            try
            {
                ssp.Initialize(source, output);
                return ssp.Parse(parser, inputStart, outputStart);
            }
            finally
            {
                ParserPool<TInput>.Pool.ReturnToPool(ssp);
            }
        }

        private static class ParserPool<TInput>
        {
            public static readonly ObjectPool<StackSafeParser<TInput>> Pool =
                new ObjectPool<StackSafeParser<TInput>>(() => new StackSafeParser<TInput>(null, null), p => p.Clear());
        }
    }

    internal class StackSafeParser<TInput> : ParserVisitor<TInput, Parser<TInput>>
    {
        private Source<TInput> source;
        private List<object> output;
        private List<ParseState> stack;
        private int stackPosition;
        private ParseState state;
        private StackSafeScanner<TInput> scanner;

        public StackSafeParser(Source<TInput> source, List<object> output)
        {
            this.stack = new List<ParseState>();
            this.stackPosition = -1;
            this.scanner = new StackSafeScanner<TInput>(source);

            Initialize(source, output);
        }

        public void Initialize(Source<TInput> source, List<object> output)
        {
            this.source = source;
            this.output = output;
            this.scanner.Initialize(source);
        }

        public void Clear()
        {
            this.source = null;
            this.output = null;
            this.scanner.Clear();
        }

        private class ParseState
        {
            public Parser<TInput> Parser { get; private set; }

            /// <summary>
            /// The input start
            /// </summary>
            public int InputStart { get; private set; }

            /// <summary>
            /// The true start of the output for the production
            /// </summary>
            public int OutputStart { get; private set; }

            /// <summary>
            /// The output count at the beginning of the parse. 
            /// This may occur after the OutputStart in right-side parsers
            /// </summary>
            public int OriginalOutputCount { get; private set; }

            /// <summary>
            /// The accumulated input length consumed by this parser
            /// </summary>
            public int InputLength;

            /// <summary>
            /// The output start for the next parser.
            /// </summary>
            public int NextOutputStart;

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

            public void Init(Parser<TInput> parser, int inputStart, int outputStart, int outputCount)
            {
                this.Parser = parser;
                this.InputStart = inputStart;
                this.OutputStart = outputStart;
                this.OriginalOutputCount = outputCount;
                this.InputLength = 0;
                this.State = 0;
                this.LastResult = 0;
                this.NextOutputStart = outputCount;
                this.BestFailedResult = 0;
                this.BestSuccessResult = 0;
            }
        }

        private void Push(Parser<TInput> parser, int inputStart, int outputStart)
        {
            this.stackPosition++;

            if (this.stackPosition == this.stack.Count)
            {
                this.stack.Add(new ParseState());
            }

            this.state = this.stack[this.stackPosition];
            this.state.Init(parser, inputStart, outputStart, this.output.Count);
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
        /// Parse using private stack, does not use the call stack.
        /// </summary>
        public int Parse(Parser<TInput> parser, int inputStart, int outputStart)
        {
            this.state = default(ParseState);
            this.stackPosition = -1;

            Push(parser, inputStart, outputStart);

            while (true)
            {
                var nextParser = this.state.Parser.Accept(this);

                if (nextParser != null)
                {
                    Push(nextParser, inputStart: this.state.InputStart + this.state.InputLength, outputStart: this.state.NextOutputStart);
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
                state.NextOutputStart = state.OutputStart;
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
                    state.NextOutputStart = state.OutputStart;
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
                int minLength = -1;
                int maxLength = -1;
                int bestParser = -1;

                // figure out which parser will consume most input
                for (int i = 0; i < parser.Parsers.Count; i++)
                {
                    var p = parser.Parsers[i];
                    var length = scanner.Scan(p, state.InputStart);

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
                    state.State = 1;
                    state.NextOutputStart = state.OutputStart;
                    return parser.Parsers[bestParser];
                }
                else
                {
                    state.InputLength = -1;
                    return null;
                }
            }
            else
            {
                state.InputLength = state.LastResult;
                return null;
            }
        }

        public override Parser<TInput> VisitBest<TOutput>(BestParser<TInput, TOutput> parser)
        {
            if (state.State == 0)
            {
                int minLength = -1;
                int maxLength = -1;
                int bestParser = -1;

                // figure out which parser will consume most input
                for (int i = 0; i < parser.Parsers.Count; i++)
                {
                    var p = parser.Parsers[i];
                    var length = scanner.Scan(p, state.InputStart);

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
                    state.State = 1;
                    state.NextOutputStart = state.OutputStart;
                    return parser.Parsers[bestParser];
                }
                else
                {
                    state.InputLength = -1;
                    return null;
                }
            }
            else
            {
                state.InputLength = state.LastResult;
                return null;
            }
        }

        public override Parser<TInput> VisitConvert<TOutput>(ConvertParser<TInput, TOutput> parser)
        {
            state.InputLength = parser.Parse(source, state.InputStart, output, output.Count);
            return null;
        }

        public override Parser<TInput> VisitFails(FailsParser<TInput> parser)
        {
            throw new System.NotImplementedException();
        }

        public override Parser<TInput> VisitFirst<TOutput>(FirstParser<TInput, TOutput> parser)
        {
            if (state.State == 0)
            {
                state.State = 1;
                state.NextOutputStart = state.OutputStart;
                return parser.Parsers[0];
            }
            else if (state.LastResult < 0)
            {
                if (state.LastResult < state.BestFailedResult)
                {
                    state.BestFailedResult = state.LastResult;
                }

                output.SetCount(state.OriginalOutputCount);

                if (state.State < parser.Parsers.Count)
                {
                    var next = parser.Parsers[state.State];
                    state.State++;
                    state.NextOutputStart = state.OutputStart;
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
                state.NextOutputStart = state.OutputStart;
                return parser.Parsers[0];
            }
            else if (state.LastResult < 0)
            {
                if (state.LastResult < state.BestFailedResult)
                {
                    state.BestFailedResult = state.LastResult;
                }

                output.SetCount(state.OriginalOutputCount);

                if (state.State < parser.Parsers.Count)
                {
                    var next = parser.Parsers[state.State];
                    state.State++;
                    state.NextOutputStart = state.OutputStart;
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
                state.NextOutputStart = state.OutputStart;
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
                var length = Scan(parser.Test, source, state.InputStart);

                if (length < 0)
                {
                    state.InputLength = length;
                    return null;
                }
                else
                {
                    state.State = 1;
                    state.NextOutputStart = state.OutputStart;
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
                var length = Scan(parser.Test, source, state.InputStart);

                if (length < 0)
                {
                    state.InputLength = length;
                    return null;
                }
                else
                {
                    state.State = 1;
                    state.NextOutputStart = state.OutputStart;
                    return parser.Parser;
                }
            }
            else
            {
                state.InputLength = state.LastResult;
                return null;
            }
        }

        private static int Scan(Parser<TInput> parser, Source<TInput> source, int start)
        {
            return SafeScanner.ScanSafe(parser, source, start);
        }

        public override Parser<TInput> VisitMap<TOutput>(MapParser<TInput, TOutput> parser)
        {
            state.InputLength = parser.Parse(source, state.InputStart, output, output.Count);
            return null;
        }

        public override Parser<TInput> VisitMatch(MatchParser<TInput> parser)
        {
            throw new System.NotImplementedException();
        }

        public override Parser<TInput> VisitMatch<TOutput>(MatchParser<TInput, TOutput> parser)
        {
            state.InputLength = parser.Parse(source, state.InputStart, output, output.Count);
            return null;
        }

        public override Parser<TInput> VisitNot(NotParser<TInput> parser)
        {
            throw new System.NotImplementedException();
        }

        public override Parser<TInput> VisitOneOrMore(OneOrMoreParser<TInput> parser)
        {
            if (state.State == 0)
            {
                state.State = 1;
                state.NextOutputStart = output.Count;
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
                // keep going as long as parsing is successful and consumed input
                state.State++;
                state.InputLength += state.LastResult;
                state.NextOutputStart = output.Count;
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
                state.NextOutputStart = output.Count;
                return parser.Parser;
            }
            else if (state.LastResult < 0)
            {
                output.SetCount(state.OriginalOutputCount);
                output.Add(parser.Producer());
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
                state.NextOutputStart = state.OutputStart;
                return parser.Parser;
            }
            else
            {
                if (state.LastResult >= 0)
                {
                    var value = parser.Producer(output, state.OutputStart);
                    output.SetCount(state.OutputStart);
                    output.Add(value);
                }
                else
                {
                    output.SetCount(state.OriginalOutputCount);
                }

                state.InputLength = state.LastResult;
                return null;
            }
        }

        public override Parser<TInput> VisitRequired<TOutput>(RequiredParser<TInput, TOutput> parser)
        {
            if (state.State == 0)
            {
                state.State = 1;
                state.NextOutputStart = output.Count;
                return parser.Parser;
            }
            else if (state.LastResult < 0)
            {
                output.SetCount(state.OriginalOutputCount);
                output.Add(parser.Producer());
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
                output.SetCount(state.OriginalOutputCount);
                return null;
            }
            else
            {
                state.InputLength += state.LastResult;

                if (state.State >= parser.Parsers.Count)
                {
                    var value = parser.ListProducer(output, state.OutputStart);
                    output.SetCount(state.OutputStart);
                    output.Add(value);
                    return null;
                }
                else
                {
                    var next = parser.Parsers[state.State];
                    state.State++;
                    state.NextOutputStart = output.Count;
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
                output.SetCount(state.OriginalOutputCount);
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
                    state.NextOutputStart = output.Count;
                    return next;
                }
            }
        }

        public override Parser<TInput> VisitZeroOrMore(ZeroOrMoreParser<TInput> parser)
        {
            if (state.State == 0 || (state.LastResult > 0 && !parser.ZeroOrOne))
            {
                // keep going as long as parsing is successful and consumed input
                state.State++;
                state.InputLength += state.LastResult;
                state.NextOutputStart = output.Count;
                return parser.Parser;
            }
            else
            {
                return null;
            }
        }
    }
}