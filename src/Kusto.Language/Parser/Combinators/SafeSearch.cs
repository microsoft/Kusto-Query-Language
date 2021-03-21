using System;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    using Utils;

    /// <summary>
    /// The action to take each time the search considers a grammar element.
    /// </summary>
    /// <param name="parser">The parser being considered by the search.</param>
    /// <param name="source">The input source.</param>
    /// <param name="start">The starting offset within the input source.</param>
    /// <param name="prevWasMissing">True if the previous rule considered was required and determined to be missing.</param>
    public delegate void SearchAction<TInput>(Parser<TInput> parser, Source<TInput> source, int start, bool prevWasMissing);

    /// <summary>
    /// The result of a search operation.
    /// </summary>
    public struct SearchResult
    {
        /// <summary>
        /// The number of input items that matched the scan pattern, or -1 for failure to match.
        /// </summary>
        public int Length;

        /// <summary>
        /// True if the last element of the scan was required and it failed to match.
        /// </summary>
        public bool Missing;

        public SearchResult(int length, bool missing)
        {
            this.Length = length;
            this.Missing = missing;
        }
    }

    public static class SafeSearcher
    {
        public static SearchResult SearchSafe<TInput>(this Parser<TInput> parser, Source<TInput> source, int start, bool prevWasMissing, SearchAction<TInput> beforeAction, Action<Parser<TInput>> afterAction = null)
        {
            var sss = SearcherPool<TInput>.Pool.AllocateFromPool();
            try
            {
                sss.Initialize(source, beforeAction, afterAction);
                return sss.Search(parser, start, prevWasMissing);
            }
            finally
            {
                SearcherPool<TInput>.Pool.ReturnToPool(sss);
            }
        }

        private static class SearcherPool<TInput>
        {
            public static readonly ObjectPool<StackSafeSearcher<TInput>> Pool =
                new ObjectPool<StackSafeSearcher<TInput>>(() => new StackSafeSearcher<TInput>(null, null, null), s => s.Clear());
        }
    }

    internal class StackSafeSearcher<TInput> : ParserVisitor<TInput, Parser<TInput>>
    {
        private Source<TInput> source;
        private SearchAction<TInput> beforeAction;
        private Action<Parser<TInput>> afterAction;
        private List<SearchState> stack;
        private int stackPosition;
        private SearchState state;
        private bool prevWasMissing;
        private StackSafeScanner<TInput> scanner;

        public StackSafeSearcher(Source<TInput> source, SearchAction<TInput> beforeAction, Action<Parser<TInput>> afterAction)
        {
            this.stack = new List<SearchState>();
            this.stackPosition = -1;
            this.scanner = new StackSafeScanner<TInput>(source);
            Initialize(source, beforeAction, afterAction);
        }

        public void Initialize(Source<TInput> source, SearchAction<TInput> beforeAction, Action<Parser<TInput>> afterAction)
        {
            this.source = source;
            this.beforeAction = beforeAction;
            this.afterAction = afterAction;
            this.scanner.Initialize(source);
        }

        public void Clear()
        {
            this.source = null;
            this.beforeAction = null;
            this.afterAction = null;
            this.scanner.Clear();
        }

        private class SearchState
        {
            public Parser<TInput> Parser { get; private set; }

            /// <summary>
            /// The input start
            /// </summary>
            public int InputStart { get; private set; }

            /// <summary>
            /// The initial state of the prevWasMissing value
            /// </summary>
            public bool PrevWasMissing { get; private set; }

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

            public bool BestFailedMissing;

            /// <summary>
            /// The result length of the best successful parser
            /// </summary>
            public int BestSuccessResult;

            public bool BestSuccessMissing;

            public void Init(Parser<TInput> parser, int inputStart, bool prevWasMissing)
            {
                this.Parser = parser;
                this.InputStart = inputStart;
                this.PrevWasMissing = prevWasMissing;
                this.InputLength = 0;
                this.State = 0;
                this.LastResult = 0;
                this.BestFailedResult = 0;
                this.BestFailedMissing = false;
                this.BestSuccessResult = 0;
                this.BestSuccessMissing = false;
            }
        }

        private void Push(Parser<TInput> parser, int inputStart, bool prevWasMissing)
        {
            this.stackPosition++;

            if (this.stackPosition == stack.Count)
            {
                stack.Add(new SearchState());
            }

            this.state = this.stack[this.stackPosition];
            this.state.Init(parser, inputStart, prevWasMissing);
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
        public SearchResult Search(Parser<TInput> parser, int start, bool initialPrevWasMissing)
        {
            this.state = default(SearchState);
            this.stackPosition = -1;
            this.prevWasMissing = initialPrevWasMissing;

            Push(parser, start, initialPrevWasMissing);

            Parser<TInput> nextParser = null;

            while (true)
            {
                if (this.state.Parser.IsHidden)
                {
                    this.state.InputLength = scanner.Scan(this.state.Parser, this.state.InputStart);
                    nextParser = null;
                }
                else
                {
                    if (this.state.State == 0)
                    {
                        beforeAction(this.state.Parser, source, this.state.InputStart, this.prevWasMissing);
                    }

                    nextParser = this.state.Parser.Accept(this);
                }

                if (nextParser != null)
                {
                    Push(nextParser, inputStart: this.state.InputStart + this.state.InputLength, prevWasMissing: this.prevWasMissing);
                }
                else
                {
                    var length = this.state.InputLength;
                    this.afterAction?.Invoke(this.state.Parser);

                    if (this.stackPosition == 0)
                    {
                        return new SearchResult(length, this.prevWasMissing);
                    }
                    else
                    {
                        Pop();
                        this.state.LastResult = length;
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
            return NextBest(parser.Parsers);
        }

        public override Parser<TInput> VisitBest<TOutput>(BestParser<TInput, TOutput> parser)
        {
            return NextBest(parser.Parsers);
        }

        public override Parser<TInput> VisitConvert<TOutput>(ConvertParser<TInput, TOutput> parser)
        {
            if (state.State == 0)
            {
                state.State++;
                return parser.Pattern;
            }
            else
            {
                state.InputLength = state.LastResult;
                this.prevWasMissing = state.PrevWasMissing;
                return null;
            }
        }

        public override Parser<TInput> VisitFails(FailsParser<TInput> parser)
        {
            var length = parser.Scan(source, state.InputStart);
            state.InputLength = length;
            this.prevWasMissing = false;
            return null;
        }

        public override Parser<TInput> VisitFirst<TOutput>(FirstParser<TInput, TOutput> parser)
        {
            return NextBest(parser.Parsers);
        }

        public override Parser<TInput> VisitFirst(FirstParser<TInput> parser)
        {
            return NextBest(parser.Parsers);
        }

        private Parser<TInput> NextBest(IReadOnlyList<Parser<TInput>> parsers)
        {
            if (state.State == 0)
            {
                state.BestFailedResult = -1;
                state.BestFailedMissing = this.prevWasMissing;
                state.BestSuccessResult = -1;
                state.State = 1;
                return parsers[0];
            }
            else
            {
                if (state.LastResult > state.BestSuccessResult)
                {
                    state.BestSuccessResult = state.LastResult;
                    state.BestSuccessMissing = this.prevWasMissing;
                }
                else if (state.LastResult < state.BestFailedResult)
                {
                    state.BestFailedResult = state.LastResult;
                    state.BestFailedMissing = this.prevWasMissing;
                }

                if (state.State >= parsers.Count)
                {
                    if (state.BestSuccessResult >= 0)
                    {
                        state.InputLength = state.BestSuccessResult;
                        this.prevWasMissing = state.BestSuccessMissing;
                        return null;
                    }
                    else
                    {
                        state.InputLength = state.BestFailedResult;
                        this.prevWasMissing = state.BestFailedMissing;
                        return null;
                    }
                }
                else
                {
                    state.InputLength = 0;
                    this.prevWasMissing = state.PrevWasMissing; // reset
                    var next = parsers[state.State];
                    state.State++;
                    return next;
                }
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
            return NextIf(parser.Test, parser.Parser);
        }

        public override Parser<TInput> VisitIf(IfParser<TInput> parser)
        {
            return NextIf(parser.Test, parser.Parser);
        }

        private Parser<TInput> NextIf(Parser<TInput> test, Parser<TInput> parser)
        {
            if (state.State == 0)
            {
                state.State++;

                if (test.Scan(source, state.InputStart) >= 0)
                {
                    // if test succeeds then search the parser
                    return parser;
                }
                else
                {
                    // otherwise search what we can of the test
                    return test;
                }
            }
            else
            {
                var length = state.LastResult;

                state.InputLength = length;

                if (length < 0)
                {
                    // if we fail, reset prev-was-missing state to initial state.
                    this.prevWasMissing = state.PrevWasMissing;
                }

                return null;
            }
        }

        public override Parser<TInput> VisitMap<TOutput>(MapParser<TInput, TOutput> parser)
        {
            var length = parser.Scan(source, state.InputStart);
            state.InputLength = length;
            this.prevWasMissing = length >= 0;
            return null;
        }

        public override Parser<TInput> VisitMatch(MatchParser<TInput> parser)
        {
            state.InputLength = parser.Consumer(source, state.InputStart);
            if (state.InputLength >= 0)
            {
                this.prevWasMissing = false;
                return null;
            }
            else
            {
                // keep prevWasMissing the same
                return null;
            }
        }

        public override Parser<TInput> VisitMatch<TOutput>(MatchParser<TInput, TOutput> parser)
        {
            state.InputLength = parser.Consumer(source, state.InputStart);
            if (state.InputLength >= 0)
            {
                this.prevWasMissing = false;
                return null;
            }
            else
            {
                // keep prevWasMissing the same
                return null;
            }
        }

        public override Parser<TInput> VisitNot(NotParser<TInput> parser)
        {
            // for finding, allow not appear to succeed so that things that condition on it will continue to find.
            if (source.IsEnd(state.InputStart))
            {
                state.InputLength = 0;
                this.prevWasMissing = false;
                return null;
            }

            var len = parser.Pattern.Scan(source, state.InputStart);
            if (len >= 0)
            {
                state.InputLength = -1;
                this.prevWasMissing = false;
                return null;
            }
            else
            {
                state.InputLength = 1;
                this.prevWasMissing = false;
                return null;
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
                // first search did not succeed, fail
                state.InputLength = state.LastResult;
                return null;
            }
            else if (state.LastResult > 0)
            {
                // keep going as long as searching is successful and consumed input
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
                this.prevWasMissing = state.PrevWasMissing; // use initial missing state
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
                this.prevWasMissing = true;
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
            return NextSequence(parser.Parsers);
        }

        public override Parser<TInput> VisitSequence(SequenceParser<TInput> parser)
        {
            return NextSequence(parser.Parsers);
        }

        private Parser<TInput> NextSequence(IReadOnlyList<Parser<TInput>> parsers)
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

                if (state.State >= parsers.Count)
                {
                    return null;
                }
                else
                {
                    var next = parsers[state.State];
                    state.State++;
                    return next;
                }
            }
        }

        public override Parser<TInput> VisitZeroOrMore(ZeroOrMoreParser<TInput> parser)
        {
            if (state.State == 0 || (state.LastResult > 0 && !parser.ZeroOrOne))
            {
                // keep going as long as searching is successful and consumed input
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