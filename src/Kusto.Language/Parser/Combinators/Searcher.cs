using Kusto.Language.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Kusto.Language.Parsing
{
    public class Searcher<TInput>: ParserVisitor<TInput, int, int>
    {
        private readonly Source<TInput> _source;
        private readonly SearchAction<TInput> _beforeAction;
        private readonly Action<Parser<TInput>> _afterAction;
        private bool _prevWasMissing;

        public Searcher(
            Source<TInput> source, 
            bool prevWasMissing,
            SearchAction<TInput> beforeAction, 
            Action<Parser<TInput>> afterAction)
        {
            _source = source;
            _prevWasMissing = prevWasMissing;
            _beforeAction = beforeAction;
            _afterAction = afterAction;
        }

        private const int MaxDepth = 400;
        private int _depth;
        private StackSafeSearcher<TInput> safeSearcher;

        private int Search(Parser<TInput> parser, int start)
        {
            if (_depth >= MaxDepth)
            {
                if (safeSearcher == null)
                {
                    safeSearcher = new StackSafeSearcher<TInput>(_source, _beforeAction, _afterAction);
                }

                var result = safeSearcher.Search(parser, start, _prevWasMissing);
                _prevWasMissing = result.Missing;
                return result.Length;
            }

            _depth++;
            int len;

            if (parser.IsHidden)
            {
                len = parser.Scan(_source, start);
            }
            else
            {
                if (_beforeAction != null)
                    _beforeAction(parser, _source, start, _prevWasMissing);

                len = parser.Accept(this, start);

                if (_afterAction != null)
                    _afterAction(parser);
            }

            _depth--;
            return len;
        }

        public static SearchResult Search(Parser<TInput> parser, Source<TInput> source, int start, bool prevWasMissing, SearchAction<TInput> beforeAction, Action<Parser<TInput>> afterAction = null)
        {
            var searcher = new Searcher<TInput>(source, prevWasMissing, beforeAction, afterAction);
            var len = searcher.Search(parser, start);
            return new SearchResult(len, searcher._prevWasMissing);
        }

        public override int VisitApply<TLeft, TOutput>(ApplyParser<TInput, TLeft, TOutput> parser, int start)
        {
            var leftLen = Search(parser.LeftParser, start);
            if (leftLen < 0)
            {
                return leftLen;
            }

            while (true)
            {
                var rightLen = Search(parser.RightParser, start + leftLen);
                if (rightLen < 0)
                {
                    if (parser.ApplyKind == ApplyKind.One)
                    {
                        // failed to be applied once
                        return -leftLen + rightLen;
                    }
                    else
                    {
                        return leftLen;
                    }
                }
                else
                {
                    leftLen += rightLen;

                    if (parser.ApplyKind != ApplyKind.ZeroOrMore)
                    {
                        return leftLen;
                    }
                }
            }
        }

        private int VisitFirst(IReadOnlyList<Parser<TInput>> parsers, int start)
        {
            var oldPrevWasMissing = _prevWasMissing;

            int max = -1;
            var maxPrevWasMissing = _prevWasMissing;
            int min = -1;
            var minPrevWasMissing = _prevWasMissing;

            for (int i = 0; i < parsers.Count; i++)
            {
                var p = parsers[i];
                _prevWasMissing = oldPrevWasMissing;
                var n = Search(p, start);

                if (n > max)
                {
                    max = n;
                    maxPrevWasMissing = _prevWasMissing;
                }
                else if (n < min)
                {
                    min = n;
                    minPrevWasMissing = _prevWasMissing;
                }
            }

            if (max >= 0)
            {
                _prevWasMissing = maxPrevWasMissing;
                return max;
            }
            else
            {
                _prevWasMissing = minPrevWasMissing;
                return min;
            }
        }

        public override int VisitBest<TOutput>(BestParser<TInput, TOutput> parser, int start)
        {
            return VisitFirst(parser.Parsers, start);
        }

        public override int VisitBest(BestParser<TInput> parser, int start)
        {
            return VisitFirst(parser.Parsers, start);
        }

        public override int VisitConvert<TOutput>(ConvertParser<TInput, TOutput> parser, int start)
        {
            return Search(parser.Pattern, start);
        }

        public override int VisitFails(FailsParser<TInput> parser, int start)
        {
            int len = parser.Scan(_source, start);
            _prevWasMissing = false;
            return len;
        }

        public override int VisitFirst<TOutput>(FirstParser<TInput, TOutput> parser, int start)
        {
            return VisitFirst(parser.Parsers, start);
        }

        public override int VisitFirst(FirstParser<TInput> parser, int start)
        {
            return VisitFirst(parser.Parsers, start);
        }

        public override int VisitForward<TOutput>(ForwardParser<TInput, TOutput> parser, int start)
        {
            return Search(parser.DeferredParser(), start);
        }

        public override int VisitIf<TOutput>(IfParser<TInput, TOutput> parser, int start)
        {
            var oldPrevWasMissing = _prevWasMissing;

            var length = Search(parser.Test, start);
            if (length < 0)
                return length;

            _prevWasMissing = oldPrevWasMissing;
            return Search(parser.Parser, start);
        }

        public override int VisitIf(IfParser<TInput> parser, int start)
        {
            var oldPrevWasMissing = _prevWasMissing;

            var length = parser.Test.Scan(_source, start);
            if (length >= 0)
            {
                _prevWasMissing = oldPrevWasMissing;
                length = Search(parser.Parser, start);
            }
            else
            {
                _prevWasMissing = oldPrevWasMissing;
                length = Search(parser.Test, start);
            }

            if (length < 0)
            {
                _prevWasMissing = oldPrevWasMissing;
            }

            return length;
        }

        public override int VisitMap<TOutput>(MapParser<TInput, TOutput> parser, int start)
        {
            var len = parser.Scan(_source, start);
            _prevWasMissing = len >= 0;
            return len;
        }

        public override int VisitMatch(MatchParser<TInput> parser, int start)
        {
            var len = parser.Consumer(_source, start);
            if (len >= 0)
            {
                _prevWasMissing = false;
            }

            return len;
        }

        public override int VisitMatch<TOutput>(MatchParser<TInput, TOutput> parser, int start)
        {
            var len = parser.Consumer(_source, start);
            if (len >= 0)
            {
                _prevWasMissing = false;
            }

            return len;
        }

        public override int VisitNot(NotParser<TInput> parser, int start)
        {
            _prevWasMissing = false;

            // for finding, allow not appear to succeed so that things that condition on it will continue to find.
            if (_source.IsEnd(start))
            {
                return 0;
            }

            var len = parser.Pattern.Scan(_source, start);
            if (len >= 0)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        public override int VisitOneOrMore(OneOrMoreParser<TInput> parser, int start)
        {
            int len = Search(parser.Parser, start);
            if (len < 0)
                return len;

            var totalLen = 0;
            while (len > 0)
            {
                totalLen += len;
                len = Search(parser.Parser, start + totalLen);
            }

            return totalLen;
        }

        public override int VisitOptional<TOutput>(OptionalParser<TInput, TOutput> parser, int start)
        {
            var oldPrevWasMissing = _prevWasMissing;

            var len = Search(parser.Parser, start);
            if (len < 0) 
            {
                _prevWasMissing = oldPrevWasMissing;
                return 0;
            }

            return len;
        }

        public override int VisitProduce<TOutput>(ProduceParser<TInput, TOutput> parser, int start)
        {
            return Search(parser.Parser, start);
        }

        public override int VisitRequired<TOutput>(RequiredParser<TInput, TOutput> parser, int start)
        {
            var len = Search(parser.Parser, start);
            if (len < 0)
            {
                _prevWasMissing = true;
                return 0;
            }

            return len;
        }

        public override int VisitRule<TOutput>(RuleParser<TInput, TOutput> parser, int start)
        {
            return VisitSequence(parser.Parsers, start);
        }

        public override int VisitSequence(SequenceParser<TInput> parser, int start)
        {
            return VisitSequence(parser.Parsers, start);
       }

        private int VisitSequence(IReadOnlyList<Parser<TInput>> parsers, int start)
        {
            var totalLen = 0;

            for (int i = 0; i < parsers.Count; i++)
            {
                var len = Search(parsers[i], start + totalLen);
                if (len < 0)
                {
                    return len - totalLen;
                }

                totalLen += len;
            }

            return totalLen;
        }

        public override int VisitZeroOrMore(ZeroOrMoreParser<TInput> parser, int start)
        {
            var totalLen = 0;

            if (parser.ZeroOrOne)
            {
                var len = Search(parser.Parser, start);
                if (len > 0)
                {
                    totalLen += len;
                    len = Search(parser.Parser, start + len);
                    if (len > 0)
                        totalLen += len;
                }
            }
            else
            {
                var len = Search(parser.Parser, start);
                while (len > 0)
                {
                    totalLen += len;
                    len = Search(parser.Parser, start + totalLen);
                }
            }

            return totalLen;
        }
    }
}