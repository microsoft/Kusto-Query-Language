using System;
using System.Linq;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    using Utils;

    public abstract class GrammarAnalysis
    {
        /// <summary>
        /// Returns true if the grammar node does not have any alternative grammar terms that
        /// can appear in the same position.
        /// </summary>
        public abstract bool IsUnique(Grammar term);

        /// <summary>
        /// Gets a list of alternative terms that can appear in the same position as the given grammar term.
        /// This list will include the specified term as one of the alternatives.
        /// </summary>
        public abstract IReadOnlyList<Grammar> GetAlternativeTerms(Grammar term);

        /// <summary>
        /// Creates a new <see cref="GrammarAnalysis"/> for the specified grammar alternatives.
        /// </summary>
        public static GrammarAnalysis Create(IReadOnlyList<Grammar> alternatives)
        {
            return new MergeAnalysis(alternatives);
        }

        private static GrammarAnalysis _empty;
        public static GrammarAnalysis Empty
        {
            get
            {
                if (_empty == null)
                    _empty = Create(new Grammar[] { });
                return _empty;
            }
        }
    }

    internal class MergeAnalysis : GrammarAnalysis
    {
        private readonly IReadOnlyList<Grammar> _alternatives;
        private readonly IReadOnlyList<GrammarGraph> _graphs;
        private readonly GrammarGraph _mergedGraph;
        private readonly HashSet<Grammar> _uniqueTerms;
        private Dictionary<Grammar, IReadOnlyList<Grammar>> _alternativeMap;

        public MergeAnalysis(IReadOnlyList<Grammar> alternatives)
        {
            _alternatives = alternatives;

            if (alternatives.Count == 1)
            {
                _mergedGraph = new GrammarGraph(alternatives);
                _graphs = new[] { _mergedGraph };
            }
            else if (alternatives.Count == 0)
            {
                _mergedGraph = new GrammarGraph();
                _graphs = new GrammarGraph[] { };
            }
            else
            {
                _graphs = alternatives.Select(r => new GrammarGraph(r)).ToList();
                _mergedGraph = new GrammarGraph(alternatives);
            }

            _uniqueTerms = new HashSet<Grammar>();
            _mergedGraph.GetUniqueTerms(_uniqueTerms);

            if (alternatives.Count > 0)
            {
                // record extra grammar nodes as unique (even though they may not be)
                // when they occur for the last time within the set of alternatives
                AddBlockEnds(0, _alternatives.Count, 0);
            }
        }

        public override bool IsUnique(Grammar term)
        {
            return _uniqueTerms.Contains(term);
        }

        private static int MaxNthTerm = 10;

        private static readonly ObjectPool<List<Grammar>> s_grammarListPool =
            new ObjectPool<List<Grammar>>(() => new List<Grammar>(), hs => hs.Clear());

        /// <summary>
        /// Gets a set of terms that are the first unique terms in each alternative relative to the other alternatives,
        /// for a range of alternatives.
        /// </summary>
        private void AddBlockEnds(
            int start, int length, int nthTerm)
        {
            var end = start + length;
            var subStart = start;
            var subStartTerms = s_grammarListPool.AllocateFromPool();
            var subEndTerms = s_grammarListPool.AllocateFromPool();

            try
            {
                var subStartAlt = _alternatives[subStart];
                var subStartGraph = _graphs[subStart];
                subStartGraph.GetNthTerms(nthTerm, subStartTerms);

                for (int i = start + 1; i < end; i++)
                {
                    var subEndAlt = _alternatives[i];
                    var subEndGraph = _graphs[i];
                    subEndTerms.Clear();
                    subEndGraph.GetNthTerms(nthTerm, subEndTerms);

                    if (!Overlaps(subStartTerms, subEndTerms))
                    {
                        var subLen = i - subStart;
                        if (subLen == 1)
                        {
                            // subStart is unique in the nth position
                            //AddUniqueTerms(subStartTerms);
                        }
                        else if (nthTerm < MaxNthTerm)
                        {
                            // otherwise attempt to differentiate this sub range
                            AddBlockEnds(subStart, subLen, nthTerm + 1);
                        }

                        subStartTerms.Clear();
                        subStartTerms.AddRange(subEndTerms);
                        subEndTerms.Clear();
                        subStart = i;
                        subStartAlt = subEndAlt;
                        subStartGraph = subEndGraph;

                        if (subLen > 1)
                        {
                            // make nth term of last alt appear as unique (even if it is not)
                            subEndAlt = _alternatives[i - 1];
                            subEndGraph = _graphs[i - 1];
                            subEndGraph.GetNthTerms(nthTerm, subEndTerms);
                            AddUniqueTerms(subEndTerms);
                            subEndTerms.Clear();
                        }
                        continue;
                    }
                }

                if (subStart > start)
                {
                    // there is a remaining sub range at the end
                    var subLen = end - subStart;
                    if (subLen == 1)
                    {
                        // subStart is unique in the nth position
                        //AddUniqueTerms(subStartTerms);
                    }
                    else if (nthTerm < MaxNthTerm)
                    {
                        // otherwise attempt to differentiate this sub range
                        AddBlockEnds(subStart, subLen, nthTerm + 1);
                    }
                }
                else if (length == 1)
                {
                    // 1 item range is unique in the nth term always
                    //AddUniqueTerms(subStartTerms);
                }
                else if (nthTerm < MaxNthTerm)
                {
                    // no unique terms in this entire range, try to differentiate by n+1 term
                    AddBlockEnds(start, length, nthTerm + 1);
                }

                if (end - subStart > 1)
                {
                    // make nth term of last alt appear as unique (even if it is not)
                    var endAlt = _alternatives[end - 1];
                    var endGraph = _graphs[end - 1];
                    subEndTerms.Clear();
                    endGraph.GetNthTerms(nthTerm, subEndTerms);
                    AddUniqueTerms(subEndTerms);
                    subEndTerms.Clear();
                }
            }
            finally
            {
                s_grammarListPool.ReturnToPool(subEndTerms);
                s_grammarListPool.ReturnToPool(subStartTerms);
            }
        }

        private static bool Overlaps(List<Grammar> a, List<Grammar> b)
        {
            return b.Any(g => a.Contains(g, GrammarEquivalenceComparer.Instance));
        }

        private void AddUniqueTerms(List<Grammar> items)
        {
            _uniqueTerms.UnionWith(items);
        }

        public override IReadOnlyList<Grammar> GetAlternativeTerms(Grammar term)
        {
            if (_alternativeMap == null)
            {
                var map = new Dictionary<Grammar, IReadOnlyList<Grammar>>();
                _mergedGraph.GetAlternativeTerms(map);
                Interlocked.CompareExchange(ref _alternativeMap, map, null);
            }

            _alternativeMap.TryGetValue(term, out var list);
            return list ?? EmptyReadOnlyList<Grammar>.Instance;
        }
    }
}