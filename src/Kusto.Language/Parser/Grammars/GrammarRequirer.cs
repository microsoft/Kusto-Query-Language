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
        public static (Grammar grammar, GrammarAnalysis analysis) Require(Grammar grammar)
        {
            if (grammar is AlternationGrammar alt)
            {
                return RequireAlternation(alt);
            }
            else
            {
                var result = Require(new[] { grammar });
                return (result.grammars[0], result.analysis);
            }
        }

        /// <summary>
        /// Make all grammar elements in each alternatives required after each becomes sufficiently unique.
        /// </summary>
        public static (IReadOnlyList<Grammar> grammars, GrammarAnalysis analysis) Require(IReadOnlyList<Grammar> alternatives)
        {
            return Require(alternatives, alt => alt, (alt, newAlt) => newAlt);
        }

        /// <summary>
        /// Make all grammar elements in each alternatives required after each becomes sufficiently unique.
        /// </summary>
        public static (IReadOnlyList<T> items, GrammarAnalysis analysis) Require<T>(IReadOnlyList<T> alternatives, Func<T, Grammar> grammarSelector, Func<T, Grammar, T> grammarUpdater)
        {
            // order the alternatives in a manner that groups alternatives with similar sequences together.
            // this allows placing required nodes around sequence steps to enable parsing of partial inputs
            // to choose the best alternative
            var orderedTs = GrammarReorderer.Reorder(alternatives, grammarSelector).ToList();

            var orderedAlts = orderedTs.Select(grammarSelector).ToList();

            // analyze all the alternative grammars
            var analysis = GrammarAnalysis.Create(orderedAlts);

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

            return (orderedTs, analysis);
        }

        private static (Grammar grammar, GrammarAnalysis analysis) RequireAlternation(AlternationGrammar grammar)
        {
            // order the alternatives in a manner that groups alternatives with similar sequences together.
            // this allows placing required nodes around sequence steps to enable parsing of partial inputs
            // to choose the best alternative
            var orderedAlts = GrammarReorderer.Reorder(grammar.Alternatives);

            // analyze all the alternative grammars
            var analysis = GrammarAnalysis.Create(orderedAlts);

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

            return (grammar.With(newAlts ?? orderedAlts), analysis);
        }

        /// <summary>
        /// Makes grammar elements required after the first unique term
        /// </summary>
        private static Grammar MakeRequiredAfterFirstUniqueTerm(Grammar grammar, GrammarAnalysis analysis)
        {
            switch (grammar)
            {
                case SequenceGrammar seq:
                    return MakeRequiredAfterFirstUniqueTerm(seq, analysis);
                case AlternationGrammar alt:
                    return MakeRequiredAfterFirstUniqueTerm(alt, analysis);
                case TaggedGrammar tg:
                    return tg.With(tg.Tag, MakeRequiredAfterFirstUniqueTerm(tg.Tagged, analysis));
                case HiddenGrammar hg:
                    return hg.With(MakeRequiredAfterFirstUniqueTerm(hg.Hidden, analysis));
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

        private static AlternationGrammar MakeRequiredAfterFirstUniqueTerm(AlternationGrammar grammar, GrammarAnalysis analysis)
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
        private static Grammar MakeRequiredAfterFirstUniqueTerm(SequenceGrammar seq, GrammarAnalysis analysis)
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
                    var newAlt = RequireAlternation(alt).grammar;
                    return new RequiredGrammar(newAlt);

                case TaggedGrammar t:
                    // require the tagged sub-grammar
                    return t.With(t.Tag, MakeRequired(t.Tagged));

                case HiddenGrammar h:
                    return h.With(MakeRequired(h.Hidden));

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
                    return RequireAlternation(alt).grammar;

                case TaggedGrammar t:
                    return t.With(t.Tag, MakeRequiredAfterFirstFallibleElement(t.Tagged));

                case HiddenGrammar h:
                    return h.With(MakeRequiredAfterFirstFallibleElement(h.Hidden));

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
                case HiddenGrammar h:
                    return IsRequiredOrOptional(h.Hidden);
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

        private static int GetIndexOfStepWithFirstUniqueTerm(IReadOnlyList<Grammar> steps, GrammarAnalysis analysis)
        {
            for (int i = 0; i < steps.Count; i++)
            {
                if (steps[i].Any(g => analysis.IsUnique(g) || analysis.IsLastNonUnique(g)))
                    return i;
            }

            return -1;
        }

        private static int GetIndexOfFirstRequiredTerm(IReadOnlyList<Grammar> steps)
        {
            for (int i = 0; i < steps.Count; i++)
            {
                // see through tags
                var step = GetUntaggedUnhiddenGrammar(steps[i]);

                if (step is RequiredGrammar)
                {
                    return i;
                }
            }

            return -1;
        }

        private static Grammar GetUntaggedUnhiddenGrammar(Grammar g)
        {
            while (true)
            {
                if (g is TaggedGrammar t)
                {
                    g = t.Tagged;
                }
                else if (g is HiddenGrammar h)
                {
                    g = h.Hidden;
                }
                else
                {
                    break;
                }
            }

            return g;
        }

        private static bool IsTerm(Grammar grammar) =>
            grammar is TokenGrammar
            || grammar is RuleGrammar
            || (grammar is TaggedGrammar tg && IsTerm(tg.Tagged))
            || (grammar is HiddenGrammar hg && IsTerm(hg.Hidden));

        private static bool IsOptional(Grammar grammar)
        {
            switch (grammar)
            {
                case OptionalGrammar _:
                case ZeroOrMoreGrammar _:
                    return true;
                case TaggedGrammar tg:
                    return IsOptional(tg.Tagged);
                case HiddenGrammar hg:
                    return IsOptional(hg.Hidden);
                default:
                    return false;
            }
        }
    }
}