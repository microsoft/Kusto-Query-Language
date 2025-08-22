using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Parsing;

    /// <summary>
    /// Finds parsers with annotations that are considered at a specific input offset
    /// </summary>
    /// <remarks>
    /// Starting at the specified start input offset with the root parser, this class scans forward through the input until 
    /// the find offset is reached and then records a list of all parsers with annotations that are considered for this offset.
    /// </remarks>
    internal class AnnotatedParserFinder<TInput> : ParserVisitor<TInput, int, int>
    {
        private Source<TInput> _source;
        private readonly int _findOffset;
        private readonly List<ParserAndIndex<TInput>> _path;
        private readonly List<Parser<TInput>> _results;
        private readonly List<ParserPath<TInput>> _pathResults;
        private bool _prevWasMissing;
        private int _missingCount;
        private int _listMissingCount;


        private AnnotatedParserFinder(
            Source<TInput> source,
            int findOffset,
            List<ParserPath<TInput>> paths)
        {
            _source = source;
            _findOffset = findOffset;
            _prevWasMissing = false;
            _path = new List<ParserAndIndex<TInput>>();
            _results = new List<Parser<TInput>>();
            _pathResults = paths;
        }

        /// <summary>
        /// Finds the parsers with annotations that are considered at the specified find input offset.
        /// </summary>
        public static IReadOnlyList<Parser<TInput>> FindParsers(Source<TInput> source, int findOffset, Parser<TInput> root, int start)
        {
            var finder = new AnnotatedParserFinder<TInput>(source, findOffset, null);
            finder.Find(root, 0, start);
            return finder._results;
        }

        /// <summary>
        /// Finds the list of paths to parsers with annotations that are considered at the specified find input offset.
        /// </summary>
        public static IReadOnlyList<ParserPath<TInput>> FindPaths(Source<TInput> source, int findOffset, Parser<TInput> root, int start)
        {
            var paths = new List<ParserPath<TInput>>();
            var finder = new AnnotatedParserFinder<TInput>(source, findOffset, paths);
            finder.Find(root, 0, start);
            return paths;
        }

        private const int MaxDepth = 400;
        private int _depth;

        private int Find(Parser<TInput> parser, int indexInParent, int start)
        {
            if (_depth >= MaxDepth)
            {
                // too deep? Just let completions fail to work instead of searching further.
                // This should generally be okay because KustoCompleter should be picking an appropriate starting grammar
                // that is not far from the target input offset.
                return -1;
            }

            _depth++;
            int len;

            // add parser to path
            _path.Add(new ParserAndIndex<TInput>(parser, indexInParent));

            if (parser.IsHidden)
            {
                len = parser.Scan(_source, start);
            }
            else
            {
                if (_findOffset == start
                    && !_prevWasMissing
                    && parser.Annotations.Count > 0)
                {
                    // if no results or new results are equally as good
                    if (_results.Count == 0
                        || _missingCount <= _listMissingCount)
                    {
                        // if this path had lesser results, then throw them out in favor of better results
                        if (_missingCount < _listMissingCount)
                        {
                            _results.Clear();
                        }

                        // combine results 
                        _results.Add(parser);
                        _listMissingCount = _missingCount;

                        if (_pathResults != null)
                            _pathResults.Add(ParserPath<TInput>.From(_path));
                    }
                }

                len = parser.Accept(this, start);
            }

            // remove parser from path
            _path.RemoveAt(_path.Count - 1);

            _depth--;
            return len;
        }

        public override int VisitApply<TLeft, TOutput>(ApplyParser<TInput, TLeft, TOutput> parser, int start)
        {
            var leftLen = Find(parser.LeftParser, 0, start);
            if (leftLen < 0)
            {
                return leftLen;
            }

            while (true)
            {
                var rightLen = Find(parser.RightParser, 1, start + leftLen);
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

        public override int VisitBest<TOutput>(BestParser<TInput, TOutput> parser, int start)
        {
            return VisitFirstOrBest(parser.Parsers, start, isBest: true);
        }

        public override int VisitBest(BestParser<TInput> parser, int start)
        {
            return VisitFirstOrBest(parser.Parsers, start, isBest: true);
        }

        private int VisitFirstOrBest(IReadOnlyList<Parser<TInput>> parsers, int start, bool isBest)
        {
            var oldPrevWasMissing = _prevWasMissing;
            var oldMissingCount = _missingCount;

            var bestLen = -1;
            var bestMissingCount = oldMissingCount;
            var bestPrevWasMissing = oldPrevWasMissing;

            for (int i = 0; i < parsers.Count; i++)
            {
                var p = parsers[i];
                int n;

                _prevWasMissing = oldPrevWasMissing;
                _missingCount = oldMissingCount;

                n = Find(p, i, start);

                if (i == 0)
                {
                    bestLen = n;
                    bestMissingCount = _missingCount;
                    bestPrevWasMissing = _prevWasMissing;
                }
                else if (n < 0 && bestLen < 0 && n < bestLen)
                {
                    bestLen = n;
                    bestMissingCount = _missingCount;
                    bestPrevWasMissing = _prevWasMissing;
                }
                else if (n > 0 && (bestLen < 0 || isBest && n > bestLen))
                {
                    bestLen = n;
                    bestMissingCount = _missingCount;
                    bestPrevWasMissing = _prevWasMissing;
                }
            }

            _missingCount = bestMissingCount;
            _prevWasMissing = bestPrevWasMissing;

            return bestLen;
        }

        public override int VisitConvert<TOutput>(ConvertParser<TInput, TOutput> parser, int start)
        {
            return Find(parser.Pattern, 0, start);
        }

        public override int VisitFails(FailsParser<TInput> parser, int start)
        {
            int len = parser.Scan(_source, start);
            _prevWasMissing = false;
            return len;
        }

        public override int VisitFirst<TOutput>(FirstParser<TInput, TOutput> parser, int start)
        {
            return VisitFirstOrBest(parser.Parsers, start, isBest: false);
        }

        public override int VisitFirst(FirstParser<TInput> parser, int start)
        {
            return VisitFirstOrBest(parser.Parsers, start, isBest: false);
        }

        public override int VisitForward<TOutput>(ForwardParser<TInput, TOutput> parser, int start)
        {
            return Find(parser.DeferredParser(), 0, start);
        }

        public override int VisitIf<TOutput>(IfParser<TInput, TOutput> parser, int start)
        {
            var oldPrevWasMissing = _prevWasMissing;
            var oldMissingCount = _missingCount;

            var length = parser.Test.Scan(_source, start);
            if (length >= 0)
            {
                Find(parser.Test, 0, start); // find annotations inside test too
                length = Find(parser.Parser, 1, start);
            }
            else
            {
                length = Find(parser.Test, 0, start);
            }

            if (length < 0)
            {
                _prevWasMissing = oldPrevWasMissing;
                _missingCount = oldMissingCount;
            }

            return length;
        }

        public override int VisitIf(IfParser<TInput> parser, int start)
        {
            var oldPrevWasMissing = _prevWasMissing;
            var oldMissingCount = _missingCount;

            var length = parser.Test.Scan(_source, start);
            if (length >= 0)
            {
                Find(parser.Test, 0, start); // find annotations inside test too
                length = Find(parser.Parser, 1,start);
            }
            else
            {
                length = Find(parser.Test, 0, start);
            }

            if (length < 0)
            {
                _prevWasMissing = oldPrevWasMissing;
                _missingCount = oldMissingCount;
            }

            return length;
        }

        public override int VisitLimit<TOutput>(LimitParser<TInput, TOutput> parser, int start)
        {
            var len = parser.Limiter.Scan(_source, start);
            if (len > 0)
            {
                var oldSource = _source;
                _source = new LimitSource<TInput>(_source, start + len);
                var result = Find(parser.Limited, 0,start);
                _source = oldSource;
                return result;
            }
            return -1;
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
            int len = Find(parser.Parser, 0, start);
            if (len < 0)
                return len;

            var totalLen = 0;
            while (len > 0)
            {
                totalLen += len;
                len = Find(parser.Parser, 0, start + totalLen);
            }

            return totalLen;
        }

        public override int VisitOptional<TOutput>(OptionalParser<TInput, TOutput> parser, int start)
        {
            var oldPrevWasMissing = _prevWasMissing;

            var len = Find(parser.Parser, 0,start);
            if (len < 0)
            {
                _prevWasMissing = oldPrevWasMissing;
                return 0;
            }

            return len;
        }

        public override int VisitProduce<TOutput>(ProduceParser<TInput, TOutput> parser, int start)
        {
            return Find(parser.Parser, 0, start);
        }

        public override int VisitRequired<TOutput>(RequiredParser<TInput, TOutput> parser, int start)
        {
            var len = Find(parser.Parser, 0,start);
            if (len < 0)
            {
                _prevWasMissing = true;
                _missingCount++;
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
                var len = Find(parsers[i], i, start + totalLen);
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
                var len = Find(parser.Parser, 0, start);
                if (len > 0)
                {
                    totalLen += len;
                    len = Find(parser.Parser, 0, start + len);
                    if (len > 0)
                        totalLen += len;
                }
            }
            else
            {
                var len = Find(parser.Parser, 0, start);
                while (len > 0)
                {
                    totalLen += len;
                    len = Find(parser.Parser, 0, start + totalLen);
                }
            }

            return totalLen;
        }
    }
}
