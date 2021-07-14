using System;
using System.Linq;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    using Utils;

    /// <summary>
    /// A grammar rewriter that makes all command grammar elements required 
    /// after the first two tokens
    /// </summary>
    internal class GrammarRequirer
    {
        public static Grammar Require(Grammar grammar)
        {
            return grammar.Accept(new Requirer(grammar));
        }

        private class Requirer : GrammarRewriter
        {
            private readonly Grammar _root;
            private readonly HashSet<Grammar> _uniqueTerms;
            private Context _context;

            public Requirer(Grammar root)
            {
                _root = root;
                _uniqueTerms = new HashSet<Grammar>();
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
                // order the alternatives in a manner that groups alternatives with similar sequences together.
                // this allows placing required nodes around sequence steps to enable parsing of partial inputs
                // to choose the best alternative
                var orderedAlts = GrammarReorderer.Reorder(grammar.Alternatives);

                // Get the first terms for each alternative that is unique relative to other alternatives
                GetUniqueTerms(orderedAlts, _uniqueTerms);

                // treat all alternatives as optional
                // this will keep the first term in each alternative free of require nodes
                var prevContext = _context;
                _context = Context.Optional;

                List<Grammar> newAlts = null;

                for (int i = 0; i < orderedAlts.Count; i++)
                {
                    var alt = orderedAlts[i];

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

                _context = prevContext;

                return grammar.With(newAlts ?? orderedAlts);
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
                    var uniqueIndex = GetIndexOfFirstUniqueTerm(seq.Steps);
                    if (requiredIndex >= 0 && (uniqueIndex < 0 || requiredIndex < uniqueIndex))
                    {
                        ignoreCount = requiredIndex;
                    }
                    else if (uniqueIndex >= 0)
                    {
                        ignoreCount = uniqueIndex + 1;
                    }
                    else if (_context == Context.Optional)
                    {
                        // if we are inside an optional context we need any term after the first non-optional term to be required
                        var firstNonOptional = GetIndexOfNthNonOptionalStep(seq.Steps, 1);
                        ignoreCount = firstNonOptional >= 0
                            ? firstNonOptional + 1
                            : seq.Steps.Count;
                    }
                    else
                    {
                        // not sure if this means require it all or require everything after the first term
                        ignoreCount = 0;
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

            private static Grammar Require(Grammar grammar)
            {
                if (IsRequiredOrOptional(grammar))
                {
                    // already required or optional, so no requiring required
                    return grammar;
                }
                else if (grammar is TaggedGrammar t)
                {
                    // require the tagged sub-grammar
                    return t.With(t.Tag, Require(t.Tagged));
                }
                else
                {
                    // require this grammar
                    return new RequiredGrammar(grammar);
                }
            }

            private static int MaxNthTerm = 10;

            private readonly ObjectPool<List<Grammar>> _grammarPool =
                new ObjectPool<List<Grammar>>(() => new List<Grammar>(), list => list.Clear());

            /// <summary>
            /// Gets a set of terms that are the first unique terms in each alternative relative to the other alternatives.
            /// </summary>
            private void GetUniqueTerms(
                IReadOnlyList<Grammar> alternatives, HashSet<Grammar> uniqueTerms)
            {
                GetUniqueTerms(alternatives, 0, alternatives.Count, 0, uniqueTerms);
            }

            /// <summary>
            /// Gets a set of terms that are the first unique terms in each alternative relative to the other alternatives,
            /// for a range of alternatives.
            /// </summary>
            private void GetUniqueTerms(
                IReadOnlyList<Grammar> alternatives, int start, int length, int nthTerm, HashSet<Grammar> uniqueTerms)
            {
                var end = start + length;
                var subStart = start;
                var subStartTerms = _grammarPool.AllocateFromPool();
                var subEndTerms = _grammarPool.AllocateFromPool();

                try
                {
                    var subStartAlt = alternatives[subStart];
                    GetNthTerm(subStartAlt, nthTerm, subStartTerms);

                    for (int i = start + 1; i < end; i++)
                    {
                        var subEndAlt = alternatives[i];
                        subEndTerms.Clear();
                        GetNthTerm(subEndAlt, nthTerm, subEndTerms);

                        if (!Overlaps(subStartTerms, subEndTerms))
                        {
                            var subLen = i - subStart;
                            if (subLen == 1)
                            {
                                // subStart is unique in the nth position
                                AddRange(uniqueTerms, subStartTerms);
                            }
                            else if (nthTerm < MaxNthTerm)
                            {
                                // otherwise attempt to differentiate this sub range
                                GetUniqueTerms(alternatives, subStart, subLen, nthTerm + 1, uniqueTerms);
                            }

                            subStartTerms.Clear();
                            subStartTerms.AddRange(subEndTerms);
                            subEndTerms.Clear();
                            subStart = i;

                            if (subLen > 1)
                            {
                                // make nth term of last alt appear as unique (even if it is not)
                                subEndAlt = alternatives[i - 1];
                                GetNthTerm(subEndAlt, nthTerm, subEndTerms);
                                AddRange(uniqueTerms, subEndTerms);
                                subEndTerms.Clear();
                            }


                            continue;
                        }
                    }

                    if (subStart > start)
                    {
                        // there is a remaining sub range at the end
                        var subLen = length - subStart;
                        if (subLen == 1)
                        {
                            // subStart is unique in the nth position
                            AddRange(uniqueTerms, subStartTerms);
                        }
                        else if (nthTerm < MaxNthTerm)
                        {
                            // otherwise attempt to differentiate this sub range
                            GetUniqueTerms(alternatives, subStart, subLen, nthTerm + 1, uniqueTerms);
                        }
                    }
                    else if (nthTerm < MaxNthTerm)
                    {
                        // no unique terms in this entire range, try to differentiate by n+1 term
                        GetUniqueTerms(alternatives, start, length, nthTerm + 1, uniqueTerms);
                    }

                    if (end - subStart > 1)
                    {
                        // make nth term of last alt appear as unique (even if it is not)
                        var endAlt = alternatives[end - 1];
                        subEndTerms.Clear();
                        GetNthTerm(endAlt, nthTerm, subEndTerms);
                        AddRange(uniqueTerms, subEndTerms);
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

            private static void AddRange(HashSet<Grammar> hashset, IReadOnlyList<Grammar> items)
            {
                foreach (var item in items)
                {
                    hashset.Add(item);
                }
            }

            private static bool GetNthTerm(Grammar g, int n, List<Grammar> terms, bool includeOptional = false)
            {
                switch (g)
                {
                    case TokenGrammar _:
                    case RuleGrammar _:
                        if (n == 0)
                        {
                            terms.Add(g);
                            return true;
                        }
                        return false;
                    case SequenceGrammar seq:
                        for (int i = 0; i < seq.Steps.Count && n >= 0; i++)
                        {
                            if (includeOptional || !IsOptional(seq.Steps[i]))
                            {
                                if (GetNthTerm(seq.Steps[i], n, terms, includeOptional))
                                    return true;
                                n--;
                            }
                        }
                        return false;
                    case AlternationGrammar alt:
                        var anyHadTerms = false;
                        foreach (var a in alt.Alternatives)
                        {
                            anyHadTerms |= GetNthTerm(a, n, terms, includeOptional);
                        }
                        return anyHadTerms;
                    case OneOrMoreGrammar oom:
                        return GetNthTerm(oom.Repeated, n, terms, includeOptional);
                    case ZeroOrMoreGrammar zom:
                        return includeOptional && GetNthTerm(zom.Repeated, n, terms, includeOptional);
                    case OptionalGrammar opt:
                        return includeOptional && GetNthTerm(opt.Optioned, n, terms, includeOptional);
                    case RequiredGrammar req:
                        return GetNthTerm(req.Required, n, terms, includeOptional);
                    case TaggedGrammar tag:
                        return GetNthTerm(tag.Tagged, n, terms, includeOptional);
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

            private int GetIndexOfFirstUniqueTerm(IReadOnlyList<Grammar> steps)
            {
                for (int i = 0; i < steps.Count; i++)
                {
                    // note: tags were not added to uniqueTerms
                    var step = GetUntaggedGrammar(steps[i]);

                    if (_uniqueTerms.Contains(step))
                    {
                        return i;
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