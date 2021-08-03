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
            return grammar.Accept(new Requirer(grammar));
        }

        /// <summary>
        /// Makes all grammar elements in each alternatives's grammar required after each alternative becomes sufficiently unique.
        /// </summary>
        public static IReadOnlyList<T> Require<T>(IReadOnlyList<T> alternatives, Func<T, Grammar> grammarSelector, Func<T, Grammar, T> grammarUpdater)
        {
            return new Requirer(null).RequireAlternatives(alternatives, grammarSelector, grammarUpdater);
        }

        private class Requirer : GrammarRewriter
        {
            private readonly Grammar _root;
            private HashSet<Grammar> _uniqueTerms;
            private Context _context;

            public Requirer(Grammar root)
            {
                _root = root;
            }

            private enum Context
            {
                None,
                Optional
            }

            public override Grammar VisitOptional(OptionalGrammar grammar)
            {
                var prevContext = _context;
                _context = Context.Optional;
                var result = base.VisitOptional(grammar);
                _context = prevContext;
                return result;
            }

            public override Grammar VisitZeroOrMore(ZeroOrMoreGrammar grammar)
            {
                var prevContext = _context;
                _context = Context.Optional;
                var result = base.VisitZeroOrMore(grammar);
                _context = prevContext;
                return result;
            }

            public override Grammar VisitOneOrMore(OneOrMoreGrammar grammar)
            {
                // oddly, one-or-more grammars need to be able to fail to terminate the list
                // so pretend that they are optional so we don't force require their contents
                var prevContext = _context;
                _context = Context.Optional;
                var result = base.VisitOneOrMore(grammar);
                _context = prevContext;
                return result;
            }

            public override Grammar VisitAlternation(AlternationGrammar grammar)
            {
                var prevContext = _context;
                _context = Context.None;

                // order the alternatives in a manner that groups alternatives with similar sequences together.
                // this allows placing required nodes around sequence steps to enable parsing of partial inputs
                // to choose the best alternative
                var orderedAlts = GrammarReorderer.Reorder(grammar.Alternatives);

                // Get the first terms for each alternative that is unique relative to other alternatives
                var altToUniqueTerms = new Dictionary<Grammar, HashSet<Grammar>>(orderedAlts.Count);
                GetUniqueTerms(orderedAlts, altToUniqueTerms);

                var prevUniqueTerms = _uniqueTerms;

                List<Grammar> newAlts = null;

                for (int i = 0; i < orderedAlts.Count; i++)
                {
                    var alt = orderedAlts[i];

                    altToUniqueTerms.TryGetValue(alt, out var uniqueTerms);
                    _uniqueTerms = uniqueTerms;

                    var newAlt = alt.Accept(this);

                    if (newAlt != alt || newAlts != null)
                    {
                        if (newAlts == null)
                        {
                            newAlts = orderedAlts.Take(i).ToList();
                        }

                        newAlts.Add(newAlt);
                    }
                }

                _uniqueTerms = prevUniqueTerms;
                _context = prevContext;

                return grammar.With(newAlts ?? orderedAlts);
            }

            public IReadOnlyList<T> RequireAlternatives<T>(IReadOnlyList<T> alternatives, Func<T, Grammar> grammarSelector, Func<T, Grammar, T> updater)
            {
                // order the alternatives in a manner that groups alternatives with similar sequences together.
                // this allows placing required nodes around sequence steps to enable parsing of partial inputs
                // to choose the best alternative
                var orderedTs = GrammarReorderer.Reorder(alternatives, grammarSelector).ToList();

                var orderedAlts = orderedTs.Select(grammarSelector).ToList();

                // Get the first terms for each alternative that is unique relative to other alternatives
                var altToUniqueTerms = new Dictionary<Grammar, HashSet<Grammar>>(alternatives.Count);
                GetUniqueTerms(orderedAlts, altToUniqueTerms);

                for (int i = 0; i < orderedTs.Count; i++)
                {
                    var t = orderedTs[i];
                    var alt = grammarSelector(t);

                    altToUniqueTerms.TryGetValue(alt, out var uniqueTerms);
                    _uniqueTerms = uniqueTerms;

                    var newAlt = alt.Accept(this);

                    if (newAlt != alt)
                    {
                        orderedTs[i] = updater(t, newAlt);
                    }
                }

                return orderedTs;
            }

            public override Grammar VisitSequence(SequenceGrammar grammar)
            {
                var result = base.VisitSequence(grammar);
                if (result is SequenceGrammar seq)
                {
                    return AddRequire(seq, isRoot: grammar == _root);
                }
                else
                {
                    return result;
                }
            }

            /// <summary>
            /// Wrap sequence steps in <see cref="RequiredGrammar"/> nodes if they 
            /// 1) occur after the point where the first term in the sequence is known to be unique relative to other alternative sequences.
            /// 2) occur after the first non-optional term in an optional context
            /// 3) after 2 non-optional terms for the root sequence
            /// </summary>
            private Grammar AddRequire(SequenceGrammar seq, bool isRoot)
            {
                // determine which steps can be ignored from being required
                int ignoreCount;

                if (isRoot)
                {
                    // if this is the root sequence then ignore first two terms 
                    // todo: update this when logic changes to consider all commands together
                    var secondNonOptional = GetIndexOfNthNonOptionalStep(seq.Steps, 2);
                    ignoreCount = secondNonOptional >= 0
                        ? secondNonOptional + 1
                        : seq.Steps.Count;

                    // if entire root sequence is only two terms, don't ignore both
                    if (ignoreCount == seq.Steps.Count)
                        ignoreCount--;
                }
                else
                {
                    // if we can identify the first unique term in these steps, all steps after that must be required
                    var requiredIndex = GetIndexOfFirstRequiredTerm(seq.Steps);
                    var uniqueIndex = GetIndexOfStepWithFirstUniqueTerm(seq.Steps);
                    if (requiredIndex >= 0 && (uniqueIndex < 0 || requiredIndex < uniqueIndex))
                    {
                        ignoreCount = requiredIndex;
                    }
                    else if (_context == Context.Optional)
                    {
                        // if we are inside an optional context we need any term after the first non-optional term to be required
                        var firstNonOptional = GetIndexOfNthNonOptionalStep(seq.Steps, 1);
                        ignoreCount = firstNonOptional >= 0
                            ? firstNonOptional + 1
                            : seq.Steps.Count;
                    }
                    else if (uniqueIndex >= 0)
                    {
                        ignoreCount = uniqueIndex + 1;
                    }
                    else
                    {
                        // nothing was unique, so don't require anything
                        ignoreCount = seq.Steps.Count;
                    }
                }

                List<Grammar> newSteps = null;

                for (int i = 0; i < seq.Steps.Count; i++)
                {
                    var step = seq.Steps[i];

                    var isIgnored = i < ignoreCount;
                    if (!isIgnored)
                    {
                        step = Require(step);

                        if (newSteps == null)
                        {
                            newSteps = seq.Steps.Take(i).ToList();
                        }
                    }

                    if (newSteps != null)
                    {
                        newSteps.Add(step);
                    }
                }

                return newSteps != null
                    ? seq.With(newSteps)
                    : seq;
            }

            /// <summary>
            /// Wraps grammar node with <see cref="RequiredGrammar"/> if necessary.
            /// </summary>
            private static Grammar Require(Grammar grammar)
            {
                switch (grammar)
                {
                    case OptionalGrammar _:
                    case ZeroOrMoreGrammar _:
                    case RequiredGrammar _:
                        // optional or already required, so don't add require to it.
                        return grammar;

                    case TaggedGrammar t:
                        // require the tagged sub-grammar
                        return t.With(t.Tag, Require(t.Tagged));

                    case SequenceGrammar s:
                        if (IsAllRequiredOrOptional(s.Steps))
                        {
                            return s;
                        }
                        else
                        {
                            // require all parts of this grammar individually
                            // note: if we just required the sequence itself, then the parser would fail to parse partial sequences
                            return s.With(s.Steps.Select(st => Require(st)).ToList());
                        }

                    case OneOrMoreGrammar _:
                    case AlternationGrammar _:
                    case TokenGrammar _:
                    case RuleGrammar _:
                        // add require to this grammar node
                        // note: alternations interiors are handled via recursion of this visitor
                        // note: oneOrMore interiors also handled via recursion of this visitor,
                        //       since we must not require all sub elements or the list won't terminate
                        return new RequiredGrammar(grammar);

                    default:
                        throw new InvalidOperationException($"Unhandled grammar: {grammar.GetType().Name}");
                }
            }

            private static int MaxNthTerm = 10;

            private readonly ObjectPool<List<Grammar>> _grammarPool =
                new ObjectPool<List<Grammar>>(() => new List<Grammar>(), list => list.Clear());

            /// <summary>
            /// Gets a set of terms that are the first unique terms in each alternative relative to the other alternatives.
            /// </summary>
            private void GetUniqueTerms(
                IReadOnlyList<Grammar> alternatives, Dictionary<Grammar, HashSet<Grammar>> altToUniqueTerms)
            {
                GetUniqueTerms(alternatives, 0, alternatives.Count, 0, altToUniqueTerms);
            }

            /// <summary>
            /// Gets a set of terms that are the first unique terms in each alternative relative to the other alternatives,
            /// for a range of alternatives.
            /// </summary>
            private void GetUniqueTerms(
                IReadOnlyList<Grammar> alternatives, int start, int length, int nthTerm, Dictionary<Grammar, HashSet<Grammar>> altToUniqueTerms)
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
                                AddUniqueTerms(subStartAlt, altToUniqueTerms, subStartTerms);
                            }
                            else if (nthTerm < MaxNthTerm)
                            {
                                // otherwise attempt to differentiate this sub range
                                GetUniqueTerms(alternatives, subStart, subLen, nthTerm + 1, altToUniqueTerms);
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
                                AddUniqueTerms(subEndAlt, altToUniqueTerms, subEndTerms);
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
                            AddUniqueTerms(subStartAlt, altToUniqueTerms, subStartTerms);
                        }
                        else if (nthTerm < MaxNthTerm)
                        {
                            // otherwise attempt to differentiate this sub range
                            GetUniqueTerms(alternatives, subStart, subLen, nthTerm + 1, altToUniqueTerms);
                        }
                    }
                    else if (length == 1)
                    {
                        // 1 item range is unique in the nth term always
                        AddUniqueTerms(subStartAlt, altToUniqueTerms, subStartTerms);
                    }
                    else if (nthTerm < MaxNthTerm)
                    {
                        // no unique terms in this entire range, try to differentiate by n+1 term
                        GetUniqueTerms(alternatives, start, length, nthTerm + 1, altToUniqueTerms);
                    }

                    if (end - subStart > 1)
                    {
                        // make nth term of last alt appear as unique (even if it is not)
                        var endAlt = alternatives[end - 1];
                        subEndTerms.Clear();
                        GetNthTerms(endAlt, nthTerm, subEndTerms);
                        AddUniqueTerms(endAlt, altToUniqueTerms, subEndTerms);
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

            private static void AddUniqueTerms(Grammar alternative, Dictionary<Grammar, HashSet<Grammar>> altToUniqueTerms, IReadOnlyList<Grammar> items)
            {
                if (altToUniqueTerms.TryGetValue(alternative, out var uniqueTerms))
                {
                    foreach (var item in items)
                    {
                        uniqueTerms.Add(item);
                    }
                }
                else
                {
                    uniqueTerms = new HashSet<Grammar>(items);
                    altToUniqueTerms.Add(alternative, uniqueTerms);
                }
            }

            private static void GetNthTerms(Grammar g, int n, List<Grammar> terms, bool includeOptional = false)
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
                            var size = GetSize(s, includeOptional);
                            if (n < totalSize + size)
                            {
                                GetNthTerms(s, n - totalSize, terms, includeOptional);
                                return;
                            }
                            totalSize += size;
                        }
                        break;
                    case AlternationGrammar alt:
                        foreach (var a in alt.Alternatives)
                        {
                            var size = GetSize(a, includeOptional);
                            if (n < size)
                            {
                                GetNthTerms(a, n, terms, includeOptional);
                            }
                        }
                        break;
                    case OneOrMoreGrammar oom:
                        GetNthTerms(oom.Repeated, n, terms, includeOptional);
                        break;
                    case ZeroOrMoreGrammar zom:
                        if (includeOptional) 
                        {
                            GetNthTerms(zom.Repeated, n, terms, includeOptional);
                        }
                        break;
                    case OptionalGrammar opt:
                        if (includeOptional)
                        {
                            GetNthTerms(opt.Optioned, n, terms, includeOptional);
                        }
                        break;
                    case RequiredGrammar req:
                        GetNthTerms(req.Required, n, terms, includeOptional);
                        break;
                    case TaggedGrammar tag:
                        GetNthTerms(tag.Tagged, n, terms, includeOptional);
                        break;
                    default:
                        throw new InvalidOperationException($"Unhandled grammar {g.GetType().Name}");
                }
            }

            private static int GetSize(Grammar g, bool includeOptional = false)
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
                            if (includeOptional || !IsOptional(s))
                            {
                                var size = GetSize(s, includeOptional);
                                totalSize += size;
                            }
                        }
                        return totalSize;
                    case AlternationGrammar alt:
                        var minSize = 0;
                        foreach (var a in alt.Alternatives)
                        {
                            var size = GetSize(a, includeOptional);
                            if (minSize == 0 || size < minSize)
                                minSize = size;
                        }
                        return minSize;
                    case OneOrMoreGrammar oom:
                        return GetSize(oom.Repeated, includeOptional);
                    case ZeroOrMoreGrammar zom:
                        return includeOptional ? GetSize(zom.Repeated, includeOptional) : 0;
                    case OptionalGrammar opt:
                        return includeOptional ? GetSize(opt.Optioned, includeOptional) : 0;
                    case RequiredGrammar req:
                        return GetSize(req.Required, includeOptional);
                    case TaggedGrammar tag:
                        return GetSize(tag.Tagged, includeOptional);
                    default:
                        throw new InvalidOperationException($"Unhandled grammar {g.GetType().Name}");
                }
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

            private int GetIndexOfStepWithFirstUniqueTerm(IReadOnlyList<Grammar> steps)
            {
                if (_uniqueTerms != null)
                {
                    var terms = _grammarPool.AllocateFromPool();

                    for (int i = 0; i < steps.Count; i++)
                    {
                        for (int n = 0; ; n++)
                        {
                            GetNthTerms(steps[i], n, terms);
                            if (terms.Any(t => _uniqueTerms.Contains(t)))
                                return i;
                            if (terms.Count == 0)
                                break;
                            terms.Clear();
                        }
                    }
                }

                return -1;
            }

            private int GetIndexOfFirstRequiredTerm(IReadOnlyList<Grammar> steps)
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
                    default:
                        return false;
                }
            }
        }
    }
}