using System;
using System.Linq;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    using Utils;

    /// <summary>
    /// A grammar rewriter that make all grammar rules in a set of alternative grammars required after the point where
    /// each alternative is sufficiently unique.
    /// </summary>
    public class GrammarRequirer
    {
        /// <summary>
        /// Make all grammar elements that are part of nested alternations required after each alternative becomes sufficiently unique.
        /// </summary>
        public static Grammar Require(Grammar grammar)
        {
            if (grammar is AlternationGrammar alt)
            {
                return RequireAlternation(alt);
            }
            else
            {
                return Require(new[] { grammar })[0];
            }
        }

        /// <summary>
        /// Make all grammar elements in each alternatives required after each becomes sufficiently unique.
        /// </summary>
        public static IReadOnlyList<Grammar> Require(IReadOnlyList<Grammar> alternatives)
        {
            return Require(alternatives, alt => alt, (alt, newAlt) => newAlt);
        }

        /// <summary>
        /// Make all grammar elements in each alternatives required after each becomes sufficiently unique.
        /// </summary>
        public static IReadOnlyList<T> Require<T>(IReadOnlyList<T> alternatives, Func<T, Grammar> grammarSelector, Func<T, Grammar, T> grammarUpdater)
        {
            // order the alternatives in a manner that groups alternatives with similar sequences together.
            // this allows placing required nodes around sequence steps to enable parsing of partial inputs
            // to choose the best alternative

            var orderedTs = GrammarReorderer.Reorder(alternatives, grammarSelector).ToList();

            var orderedAlts = orderedTs.Select(grammarSelector).ToList();

            // Get the terms for each alternative that are unique relative to other alternatives
            var analysis = AnalyzeUniqueness(orderedAlts);

            for (int i = 0; i < orderedTs.Count; i++)
            {
                var t = orderedTs[i];
                var alt = grammarSelector(t);

                var newAlt = MakeRequiredAfterFirstUniqueTerm(alt, analysis);

                if (newAlt != alt)
                {
                    orderedTs[i] = grammarUpdater(t, newAlt);
                }
            }

            return orderedTs;
        }

        private static Grammar RequireAlternation(AlternationGrammar grammar)
        {
            // order the alternatives in a manner that groups alternatives with similar sequences together.
            // this allows placing required nodes around sequence steps to enable parsing of partial inputs
            // to choose the best alternative
            var orderedAlts = GrammarReorderer.Reorder(grammar.Alternatives);

            // Get the terms for each alternative that are unique relative to the other alternatives
            var analysis = AnalyzeUniqueness(orderedAlts);

            List<Grammar> newAlts = null;

            for (int i = 0; i < orderedAlts.Count; i++)
            {
                var alt = orderedAlts[i];

                var newAlt = MakeRequiredAfterFirstUniqueTerm(alt, analysis);

                if (newAlt != alt || newAlts != null)
                {
                    if (newAlts == null)
                    {
                        newAlts = orderedAlts.Take(i).ToList();
                    }

                    newAlts.Add(newAlt);
                }
            }

            return grammar.With(newAlts ?? orderedAlts);
        }

        /// <summary>
        /// Makes grammar elements required after the first unique term
        /// </summary>
        private static Grammar MakeRequiredAfterFirstUniqueTerm(Grammar grammar, UniquenessAnalysis analysis)
        {
            switch (grammar)
            {
                case SequenceGrammar seq:
                    return MakeRequiredAfterFirstUniqueTerm(seq, analysis);
                case AlternationGrammar alt:
                    return MakeRequiredAfterFirstUniqueTerm(alt, analysis);
                case TaggedGrammar tg:
                    return tg.With(tg.Tag, MakeRequiredAfterFirstUniqueTerm(tg.Tagged, analysis));
                case OneOrMoreGrammar oom:
                    return oom.With(MakeRequiredAfterFirstUniqueTerm(oom.Repeated, analysis), oom.Separator, oom.AllowTrailingSeparator);
                case ZeroOrMoreGrammar zom:
                    return zom.With(MakeRequiredAfterFirstUniqueTerm(zom.Repeated, analysis), zom.Separator, zom.AllowTrailingSeparator);
                case OptionalGrammar opt:
                    return opt.With(MakeRequiredAfterFirstUniqueTerm(opt.Optioned, analysis));
                case RequiredGrammar req:
                    return req.With(MakeRequiredAfterFirstUniqueTerm(req.Required, analysis));
                case TokenGrammar _:
                case RuleGrammar _:
                    // nothing after this term
                    return grammar;
                default:
                    throw new InvalidOperationException($"Unhandled grammar type: {grammar.GetType().Name}");
            }
        }

        private static AlternationGrammar MakeRequiredAfterFirstUniqueTerm(AlternationGrammar grammar, UniquenessAnalysis analysis)
        {
            List<Grammar> newAlts = null;

            for (int i = 0; i < grammar.Alternatives.Count; i++)
            {
                var alt = grammar.Alternatives[i];

                var newAlt = MakeRequiredAfterFirstUniqueTerm(alt, analysis);

                if (newAlt != alt && newAlts == null)
                {
                    newAlts = grammar.Alternatives.Take(1).ToList();
                }

                if (newAlts != null)
                {
                    newAlts.Add(newAlt);
                }
            }

            return (newAlts != null)
                ? grammar.With(newAlts)
                : grammar;
        }

        /// <summary>
        /// Makes grammar elements required after the first unique term
        /// </summary>
        private static Grammar MakeRequiredAfterFirstUniqueTerm(SequenceGrammar seq, UniquenessAnalysis analysis)
        {
            // determine which steps can be ignored from being required
            int ignoreCount;

            var uniqueIndex = GetIndexOfStepWithFirstUniqueTerm(seq.Steps, analysis);

            // don't count non-fallible unique step as the first unique
            var firstFallible = GetIndexOfNthNonOptionalStep(seq.Steps, 1);
            if (uniqueIndex >= 0 && firstFallible > uniqueIndex)
                uniqueIndex = firstFallible;

            // also check to see if any step is already required
            var requiredIndex = GetIndexOfFirstRequiredTerm(seq.Steps);

            if (requiredIndex >= 0 && (uniqueIndex < 0 || requiredIndex < uniqueIndex))
            {
                // there was already a required element before the first unique term, so start just after it instead.
                ignoreCount = requiredIndex;
            }
            else if (uniqueIndex >= 0)
            {
                // ignore everything up the step with the first unique term
                ignoreCount = uniqueIndex + 1;
            }
            else
            {
                // nothing was unique, so don't require anything
                ignoreCount = seq.Steps.Count;
            }

            List<Grammar> newSteps = null;

            for (int i = 0; i < seq.Steps.Count; i++)
            {
                var step = seq.Steps[i];

                Grammar newStep;
                if (i < ignoreCount)
                {
                    newStep = MakeRequiredAfterFirstUniqueTerm(step, analysis);
                }
                else
                {
                    newStep = MakeRequired(step);
                }

                if (newStep != step && newSteps == null)
                {
                    newSteps = seq.Steps.Take(i).ToList();
                }

                if (newSteps != null)
                {
                    newSteps.Add(newStep);
                }
            }

            return newSteps != null
                ? seq.With(newSteps)
                : seq;
        }

        /// <summary>
        /// Wraps grammar node with <see cref="RequiredGrammar"/> if necessary.
        /// </summary>
        private static Grammar MakeRequired(Grammar grammar)
        {
            switch (grammar)
            {
                case OptionalGrammar _:
                case ZeroOrMoreGrammar _:
                case RequiredGrammar _:
                    return MakeRequiredAfterFirstFallibleElement(grammar);

                case OneOrMoreGrammar _:
                    // one-or-more needs to be fallible internally or it won't ever end
                    var noom = MakeRequiredAfterFirstFallibleElement(grammar);
                    return new RequiredGrammar(noom);

                case SequenceGrammar s:
                    var ns = MakeRequiredAfterFirstFallibleElement(s);
                    if (IsAllRequiredOrOptional(ns.Steps))
                    {
                        return ns;
                    }
                    else
                    {
                        return new RequiredGrammar(ns);
                    }

                case AlternationGrammar alt:
                    // an internal alternation that is being fully required, so re-apply require logic to its parts
                    var newAlt = RequireAlternation(alt);
                    return new RequiredGrammar(newAlt);

                case TaggedGrammar t:
                    // require the tagged sub-grammar
                    return t.With(t.Tag, MakeRequired(t.Tagged));

                case TokenGrammar _:
                case RuleGrammar _:
                    // add require to this grammar node
                    // note: oneOrMore interiors also handled via recursion of this visitor,
                    //       since we must not require all sub elements or the list won't terminate
                    return new RequiredGrammar(grammar);

                default:
                    throw new InvalidOperationException($"Unhandled grammar: {grammar.GetType().Name}");
            }
        }

        /// <summary>
        /// Makes grammar nodes required after the first fallible element
        /// </summary>
        private static Grammar MakeRequiredAfterFirstFallibleElement(Grammar grammar)
        {
            switch (grammar)
            {
                case OptionalGrammar opt:
                    return opt.With(MakeRequiredAfterFirstFallibleElement(opt.Optioned));

                case ZeroOrMoreGrammar zom:
                    return zom.With(MakeRequiredAfterFirstFallibleElement(zom.Repeated), zom.Separator, zom.AllowTrailingSeparator);

                case OneOrMoreGrammar oom:
                    return oom.With(MakeRequiredAfterFirstFallibleElement(oom.Repeated), oom.Separator, oom.AllowTrailingSeparator);

                case RequiredGrammar _:
                    // optional or already required, so don't add require to it.
                    return grammar;

                case SequenceGrammar s:
                    return MakeRequiredAfterFirstFallibleElement(s);

                case AlternationGrammar alt:
                    // note: alt node itself is required, so do require fixup for all alternatives too.
                    return RequireAlternation(alt);

                case TaggedGrammar t:
                    return t.With(t.Tag, MakeRequiredAfterFirstFallibleElement(t.Tagged));

                case TokenGrammar _:
                case RuleGrammar _:
                    // nothing after this fallible element to be required
                    return grammar;

                default:
                    throw new InvalidOperationException($"Unhandled grammar: {grammar.GetType().Name}");
            }
        }

        /// <summary>
        /// Makes grammar nodes required after the first fallible element
        /// </summary>
        private static SequenceGrammar MakeRequiredAfterFirstFallibleElement(SequenceGrammar seq)
        {
            // determine which steps can be ignored from being required
            int ignoreCount;

            var firstNonOptional = GetIndexOfNthNonOptionalStep(seq.Steps, 1);

            // also check to see if any step is already required
            var requiredIndex = GetIndexOfFirstRequiredTerm(seq.Steps);

            if (requiredIndex >= 0 && (firstNonOptional < 0 || requiredIndex < firstNonOptional))
            {
                // there was already a required element before the first non-optional element... is this even possible?
                ignoreCount = requiredIndex;
            }
            else if (firstNonOptional >= 0)
            {
                // ignore everything up to the step with first non-optional term
                ignoreCount = firstNonOptional + 1;
            }
            else
            {
                // there was no non-optional element, so ignore everything
                ignoreCount = seq.Steps.Count;
            }

            List<Grammar> newSteps = null;

            for (int i = 0; i < seq.Steps.Count; i++)
            {
                var step = seq.Steps[i];

                Grammar newStep;
                if (i < ignoreCount)
                {
                    newStep = MakeRequiredAfterFirstFallibleElement(step);
                }
                else
                {
                    newStep = MakeRequired(step);
                }

                if (newStep != step && newSteps == null)
                {
                    newSteps = seq.Steps.Take(i).ToList();
                }

                if (newSteps != null)
                {
                    newSteps.Add(newStep);
                }
            }

            return newSteps != null
                ? seq.With(newSteps)
                : seq;
        }

        /// <summary>
        /// Gets a set of terms that are the first unique terms in each alternative relative to the other alternatives.
        /// </summary>
        private static UniquenessAnalysis AnalyzeUniqueness(IReadOnlyList<Grammar> alternatives)
        {
            // return new AlignmentAnalysis(alternatives);
            return new MergeAnalysis(alternatives);
        }

        private static bool IsRequiredOrOptional(Grammar g)
        {
            switch (g)
            {
                case RequiredGrammar _:
                case OptionalGrammar _:
                case ZeroOrMoreGrammar _:
                    return true;
                case TaggedGrammar t:
                    return IsRequiredOrOptional(t.Tagged);
                case SequenceGrammar s:
                    return IsAllRequiredOrOptional(s.Steps);
                default:
                    return false;
            }
        }

        private static bool IsAllRequiredOrOptional(IReadOnlyList<Grammar> list)
        {
            foreach (var g in list)
            {
                if (!IsRequiredOrOptional(g))
                    return false;
            }

            return true;
        }

        private static int GetIndexOfNthNonOptionalStep(IReadOnlyList<Grammar> steps, int n)
        {
            for (int i = 0; i < steps.Count; i++)
            {
                if (!IsOptional(steps[i]))
                {
                    n--;
                    if (n == 0)
                        return i;
                }
            }

            return -1;
        }

        private static int GetIndexOfStepWithFirstUniqueTerm(IReadOnlyList<Grammar> steps, UniquenessAnalysis analysis)
        {
            for (int i = 0; i < steps.Count; i++)
            {
                if (steps[i].Any(g => analysis.IsUnique(g)))
                    return i;
            }

            return -1;
        }

        private static int GetIndexOfFirstRequiredTerm(IReadOnlyList<Grammar> steps)
        {
            for (int i = 0; i < steps.Count; i++)
            {
                // see through tags
                var step = GetUntaggedGrammar(steps[i]);

                if (step is RequiredGrammar)
                {
                    return i;
                }
            }

            return -1;
        }

        private static Grammar GetUntaggedGrammar(Grammar g)
        {
            while (g is TaggedGrammar t)
            {
                g = t.Tagged;
            }

            return g;
        }

        private static bool IsTerm(Grammar grammar) =>
            grammar is TokenGrammar
            || grammar is RuleGrammar
            || (grammar is TaggedGrammar tg && IsTerm(tg.Tagged));

        private static bool IsOptional(Grammar grammar)
        {
            switch (grammar)
            {
                case OptionalGrammar _:
                case ZeroOrMoreGrammar _:
                    return true;
                case TaggedGrammar tg:
                    return IsOptional(tg.Tagged);
                default:
                    return false;
            }
        }
    }

    internal abstract class UniquenessAnalysis
    {
        public abstract bool IsUnique(Grammar term);
    }

#if false  // prior analysis style.. keeping around for in case we need to revert forward
    internal class AlignmentAnalysis : UniquenessAnalysis
    {
        private readonly HashSet<Grammar> _uniqueTerms;

        public AlignmentAnalysis(IReadOnlyList<Grammar> alternatives)
        {
            _uniqueTerms = new HashSet<Grammar>();
            GetUniqueTerms(alternatives, 0, alternatives.Count, 0);
        }

        public override bool IsUnique(Grammar term)
        {
            return _uniqueTerms.Contains(term);
        }

        private static int MaxNthTerm = 10;

        private readonly ObjectPool<List<Grammar>> _grammarPool =
            new ObjectPool<List<Grammar>>(() => new List<Grammar>(), list => list.Clear());

        /// <summary>
        /// Gets a set of terms that are the first unique terms in each alternative relative to the other alternatives,
        /// for a range of alternatives.
        /// </summary>
        private void GetUniqueTerms(
            IReadOnlyList<Grammar> alternatives, int start, int length, int nthTerm)
        {
            var end = start + length;
            var subStart = start;
            var subStartTerms = _grammarPool.AllocateFromPool();
            var subEndTerms = _grammarPool.AllocateFromPool();

            try
            {
                var subStartAlt = alternatives[subStart];
                GetNthTerms(subStartAlt, nthTerm, subStartTerms);

                for (int i = start + 1; i < end; i++)
                {
                    var subEndAlt = alternatives[i];
                    subEndTerms.Clear();
                    GetNthTerms(subEndAlt, nthTerm, subEndTerms);

                    if (!Overlaps(subStartTerms, subEndTerms))
                    {
                        var subLen = i - subStart;
                        if (subLen == 1)
                        {
                            // subStart is unique in the nth position
                            AddUniqueTerms(subStartTerms);
                        }
                        else if (nthTerm < MaxNthTerm)
                        {
                            // otherwise attempt to differentiate this sub range
                            GetUniqueTerms(alternatives, subStart, subLen, nthTerm + 1);
                        }

                        subStartTerms.Clear();
                        subStartTerms.AddRange(subEndTerms);
                        subEndTerms.Clear();
                        subStart = i;
                        subStartAlt = subEndAlt;

                        if (subLen > 1)
                        {
                            // make nth term of last alt appear as unique (even if it is not)
                            subEndAlt = alternatives[i - 1];
                            GetNthTerms(subEndAlt, nthTerm, subEndTerms);
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
                        AddUniqueTerms(subStartTerms);
                    }
                    else if (nthTerm < MaxNthTerm)
                    {
                        // otherwise attempt to differentiate this sub range
                        GetUniqueTerms(alternatives, subStart, subLen, nthTerm + 1);
                    }
                }
                else if (length == 1)
                {
                    // 1 item range is unique in the nth term always
                    AddUniqueTerms(subStartTerms);
                }
                else if (nthTerm < MaxNthTerm)
                {
                    // no unique terms in this entire range, try to differentiate by n+1 term
                    GetUniqueTerms(alternatives, start, length, nthTerm + 1);
                }

                if (end - subStart > 1)
                {
                    // make nth term of last alt appear as unique (even if it is not)
                    var endAlt = alternatives[end - 1];
                    subEndTerms.Clear();
                    GetNthTerms(endAlt, nthTerm, subEndTerms);
                    AddUniqueTerms(subEndTerms);
                    subEndTerms.Clear();
                }
            }
            finally
            {
                _grammarPool.ReturnToPool(subEndTerms);
                _grammarPool.ReturnToPool(subStartTerms);
            }
        }

        private static bool Overlaps(List<Grammar> a, List<Grammar> b) =>
            b.Any(g => a.Contains(g, GrammarEquivalenceComparer.Instance));

        private void AddUniqueTerms(IReadOnlyList<Grammar> items)
        {
            _uniqueTerms.UnionWith(items);
        }

        private static void GetNthTerms(Grammar g, int n, List<Grammar> terms)
        {
            GetNthTermsMin(g, n, terms);
        }

        /// <summary>
        /// Get all the terms that occur at the nth position, considering only the min length of alternatives
        /// </summary>
        private static void GetNthTermsMin(Grammar g, int n, List<Grammar> terms)
        {
            switch (g)
            {
                case TokenGrammar _:
                case RuleGrammar _:
                    if (n == 0)
                    {
                        terms.Add(g);
                    }
                    break;
                case SequenceGrammar seq:
                    var totalSize = 0;
                    foreach (var s in seq.Steps)
                    {
                        var size = GetMinSize(s);
                        if (n < totalSize + size)
                        {
                            GetNthTermsMin(s, n - totalSize, terms);
                            return;
                        }
                        totalSize += size;
                    }
                    break;
                case AlternationGrammar alt:
                    foreach (var a in alt.Alternatives)
                    {
                        var size = GetMinSize(a);
                        if (n < size)
                        {
                            GetNthTermsMin(a, n, terms);
                        }
                    }
                    break;
                case OneOrMoreGrammar oom:
                    GetNthTermsMin(oom.Repeated, n, terms);
                    break;
                case ZeroOrMoreGrammar _:
                    break;
                case OptionalGrammar _:
                    break;
                case RequiredGrammar req:
                    GetNthTermsMin(req.Required, n, terms);
                    break;
                case TaggedGrammar tag:
                    GetNthTermsMin(tag.Tagged, n, terms);
                    break;
                default:
                    throw new InvalidOperationException($"Unhandled grammar {g.GetType().Name}");
            }
        }

        private static int GetMinSize(Grammar g)
        {
            switch (g)
            {
                case TokenGrammar _:
                case RuleGrammar _:
                    return 1;
                case SequenceGrammar seq:
                    var totalSize = 0;
                    foreach (var s in seq.Steps)
                    {
                        var size = GetMinSize(s);
                        totalSize += size;
                    }
                    return totalSize;
                case AlternationGrammar alt:
                    var minSize = 0;
                    foreach (var a in alt.Alternatives)
                    {
                        var size = GetMinSize(a);
                        if (minSize == 0 || size < minSize)
                            minSize = size;
                    }
                    return minSize;
                case OneOrMoreGrammar oom:
                    return GetMinSize(oom.Repeated);
                case ZeroOrMoreGrammar _:
                    return 0;
                case OptionalGrammar _:
                    return 0;
                case RequiredGrammar req:
                    return GetMinSize(req.Required);
                case TaggedGrammar tag:
                    return GetMinSize(tag.Tagged);
                default:
                    throw new InvalidOperationException($"Unhandled grammar {g.GetType().Name}");
            }
        }
    }
#endif

    internal class MergeAnalysis : UniquenessAnalysis
    {
        private readonly IReadOnlyList<Grammar> _alternatives;
        private readonly IReadOnlyList<GrammarGraph> _graphs;
        private readonly GrammarGraph _mergedGraph;
        private readonly HashSet<Grammar> _uniqueTerms;

        public MergeAnalysis(IReadOnlyList<Grammar> alternatives)
        {
            _alternatives = alternatives;

            if (alternatives.Count == 1)
            {
                _mergedGraph = new GrammarGraph(alternatives);
                _graphs = new[] { _mergedGraph };
            }
            else
            {
                _graphs = alternatives.Select(r => new GrammarGraph(r)).ToList();
                _mergedGraph = new GrammarGraph(alternatives);
            }

            _uniqueTerms = new HashSet<Grammar>();
            _mergedGraph.GetUniqueTerms(_uniqueTerms);

            // record extra grammar nodes as unique (even though they may not be)
            // when the occur for the last time
            AddBlockEnds(0, _alternatives.Count, 0);
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
    }

    /// <summary>
    /// A class that models the merger of a set of alternate grammars
    /// </summary>
    internal class GrammarGraph
    {
        private readonly Node _root;
#if DEBUG
        private readonly Dictionary<Grammar, Node> _grammarToNodeMap;
#endif

        public GrammarGraph()
        {
            _root = new Node(null);

#if DEBUG
            _grammarToNodeMap = new Dictionary<Grammar, Node>();
#endif
        }

        public GrammarGraph(Grammar grammar)
            : this()
        {
            Add(grammar);
        }

        public GrammarGraph(IReadOnlyList<Grammar> alternates)
            : this()
        {
            foreach (var alt in alternates)
            {
                Add(alt);
            }
        }

        /// <summary>
        /// Adds all the grammar terms in the grammar tree
        /// </summary>
        public void Add(Grammar root)
        {
            Add(_root, root, null);
        }

        /// <summary>
        /// Get all the terms that occur only once along their path
        /// </summary>
        public void GetUniqueTerms(HashSet<Grammar> terms)
        {
            _root.GetUniqueTerms(terms);
        }

        /// <summary>
        /// Gets all the terms that can match the nth input item
        /// </summary>
        public void GetNthTerms(int n, List<Grammar> terms)
        {
            _root.GetNthTerms(n, terms);
        }

        private void Add(Node root, Grammar grammar, HashSet<Node> nextNodes)
        {
            switch (grammar)
            {
                case TokenGrammar _:
                case RuleGrammar _:
                    var nn = root.AddTerm(grammar);
                    nextNodes?.Add(nn);
#if DEBUG
                    if (_grammarToNodeMap.ContainsKey(grammar))
                    {
                        // not sure yet to do anything about this
                        // System.Diagnostics.Debug.Fail("Grammar already assigned to a node");
                    }
                    else
                    {
                        _grammarToNodeMap.Add(grammar, nn);
                    }
#endif
                    break;
                case SequenceGrammar seq:
                    AddSteps(seq, 0, root, nextNodes);
                    break;
                case AlternationGrammar alt:
                    foreach (var a in alt.Alternatives)
                    {
                        Add(root, a, nextNodes);
                    }
                    break;
                case OneOrMoreGrammar oom:
                    Add(root, oom.Repeated, nextNodes);
                    break;
                case ZeroOrMoreGrammar zom:
                    // add root for zero case
                    nextNodes?.Add(root);
                    Add(root, zom.Repeated, nextNodes);
                    break;
                case OptionalGrammar opt:
                    // add root for zero case
                    nextNodes?.Add(root);
                    Add(root, opt.Optioned, nextNodes);
                    break;
                case RequiredGrammar req:
                    Add(root, req.Required, nextNodes);
                    break;
                case TaggedGrammar tag:
                    Add(root, tag.Tagged, nextNodes);
                    break;
                default:
                    throw new InvalidOperationException($"Unhandled grammar {grammar.GetType().Name}");
            }
        }

        private void AddSteps(SequenceGrammar grammar, int iStep, Node root, HashSet<Node> finalNext)
        {
            var isFinal = iStep == grammar.Steps.Count - 1;

            HashSet<Node> stepNext = !isFinal
                ? s_nodeListPool.AllocateFromPool()
                : finalNext;

            var step = grammar.Steps[iStep];
            Add(root, step, stepNext);

            if (!isFinal)
            {
                AddSteps(grammar, iStep + 1, stepNext, finalNext);
                s_nodeListPool.ReturnToPool(stepNext);
            }
        }

        private void AddSteps(SequenceGrammar grammar, int iStep, HashSet<Node> roots, HashSet<Node> finalNext)
        {
            var isFinal = iStep == grammar.Steps.Count - 1;
            HashSet<Node> stepNext = !isFinal ? s_nodeListPool.AllocateFromPool() : finalNext;

            foreach (var root in roots)
            {
                var step = grammar.Steps[iStep];
                Add(root, step, stepNext);
            }

            if (!isFinal)
            {
                AddSteps(grammar, iStep + 1, stepNext, finalNext);
                s_nodeListPool.ReturnToPool(stepNext);
            }
        }

        private static readonly ObjectPool<HashSet<Node>> s_nodeListPool =
            new ObjectPool<HashSet<Node>>(() => new HashSet<Node>(), hs => hs.Clear());

        private class Node
        {
            // all the grammar terms from alternate sources that branched to here
            private readonly HashSet<Grammar> _terms;

            // all the branches for common terms 
            private readonly Dictionary<Grammar, Node> _branches;

            public Node(Grammar term)
            {
                _terms = new HashSet<Grammar>();
                _branches = new Dictionary<Grammar, Node>(GrammarEquivalenceComparer.Instance);
                if (term != null)
                {
                    _terms.Add(term);
                }
            }

            public Node AddTerm(Grammar term)
            {
                if (!_branches.TryGetValue(term, out var nextNode))
                {
                    nextNode = new Node(term);
                    _branches.Add(term, nextNode);
                }
                else
                {
                    nextNode._terms.Add(term);
                }

                return nextNode;
            }

            public Node GetNext(Grammar term)
            {
                _branches.TryGetValue(term, out var nextNode);
                return nextNode;
            }

            public void GetUniqueTerms(HashSet<Grammar> terms)
            {
                if (_terms.Count == 1)
                {
                    // get the unique term here
                    terms.UnionWith(_terms);
                }
                else
                {
                    foreach (var node in _branches.Values)
                    {
                        node.GetUniqueTerms(terms);
                    }
                }
            }

            public void GetNthTerms(int n, List<Grammar> terms)
            {
                if (n >= 0)
                {
                    foreach (var node in _branches.Values)
                    {
                        node.GetNthTerms(n - 1, terms);
                    }
                }
                else
                {
                    terms.AddRange(_terms);
                }
            }
        }
    }
}