using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kusto.Language.Parsing
{
    /// <summary>
    /// A class used to simplify grammar trees.
    /// </summary>
    public class GrammarSimplifier
    {
        /// <summary>
        /// Returns a simplified version of the grammar tree.
        /// If nothing can be simplified, the same instance is returned.
        /// </summary>
        public static Grammar Simplify(Grammar grammar)
        {
            return grammar.Accept(s_simplifier);
        }

        private static readonly Simplifier s_simplifier = new Simplifier();

        private class Simplifier : GrammarRewriter
        {
            public override Grammar VisitAlternation(AlternationGrammar grammar)
            {
                var newAlts = VisitList(grammar.Alternatives);

                // an alternation with no alternatives is nothing
                if (newAlts.Count == 0)
                    return null;

                // flatten nested alternations
                if (newAlts.Any(g => g is AlternationGrammar))
                    newAlts = newAlts.SelectMany(g => g is AlternationGrammar ag ? ag.Alternatives : new[] { g }).ToList();

                // an alternation of one element is that element
                if (newAlts.Count == 1)
                    return newAlts[0];

                var nonOptionalAlts = MakeNonOptional(newAlts);
                if (nonOptionalAlts != newAlts)
                {
                    return new OptionalGrammar(grammar.With(nonOptionalAlts));
                }
                else if (newAlts == grammar.Alternatives)
                {
                    return grammar;
                }
                else
                {
                    return grammar.With(newAlts);
                }
            }

            /// <summary>
            /// Makes alternatives non-optional
            /// </summary>
            private static IReadOnlyList<Grammar> MakeNonOptional(IReadOnlyList<Grammar> alternatives)
            {
                if (alternatives.Any(a => IsOptional(a)))
                {
                    List<Grammar> newAlts = null;

                    for (int i = 0; i < alternatives.Count; i++)
                    {
                        var alt = alternatives[i];
                        var newAlt = MakeNonOptional(alt);
                        if (newAlt != alt)
                        {
                            alternatives = newAlts = newAlts ?? alternatives.ToList();
                            newAlts[i] = newAlt;
                        }
                    }
                }

                return alternatives;
            }

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

            private static Grammar MakeNonOptional(Grammar grammar)
            {
                switch (grammar)
                {
                    case OptionalGrammar opt:
                        return opt.Optioned;
                    case ZeroOrMoreGrammar zom:
                        return new OneOrMoreGrammar(zom.Repeated, zom.Separator, zom.AllowTrailingSeparator);
                    case TaggedGrammar tagged:
                        return tagged.With(tagged.Tag, MakeNonOptional(tagged.Tagged));
                    default:
                        return grammar;
                }
            }


            public override Grammar VisitSequence(SequenceGrammar grammar)
            {
                var newSteps = VisitList(grammar.Steps);

                // a sequence of no steps, is nothing
                if (newSteps.Count == 0)
                    return null;

                // flatten nested sequences
                if (newSteps.Any(s => s is SequenceGrammar))
                    newSteps = newSteps.SelectMany(s => s is SequenceGrammar sg ? sg.Steps : new[] { s }).ToList();

                newSteps = SimplifySeparatedRepetitions(newSteps);

                // sequence of one element is that element
                if (newSteps.Count == 1)
                    return newSteps[0];

                if (newSteps == grammar.Steps)
                    return grammar;

                return grammar.With(newSteps);
            }

            private static IReadOnlyList<Grammar> SimplifySeparatedRepetitions(IReadOnlyList<Grammar> steps)
            {
                List<Grammar> newSteps = null;

                for (int i = 0; i < steps.Count;)
                {
                    var step = steps[i];

                    switch (step)
                    {
                        case ZeroOrMoreGrammar zom:
                            // convert: a b (c a b)* => {a b, c}+
                            if (zom.Separator == null
                                && zom.Repeated is SequenceGrammar zseq
                                && zseq.Steps.Count - 1 is int len
                                && len <= i
                                && AreEquivalent(steps, i - len, zseq.Steps, 1, len))
                            {
                                newSteps = newSteps ?? steps.ToList();
                                var newSeqSteps = zseq.Steps.ToList();
                                newSeqSteps.RemoveAt(0); // remove separator
                                var newSeparator = zseq.Steps[0];
                                var newRepeated = newSeqSteps.Count == 1 ? newSeqSteps[0] : new SequenceGrammar(newSeqSteps);
                                var newZ = new OneOrMoreGrammar(newRepeated, newSeparator);
                                newSteps[i] = newZ;
                                newSteps.RemoveRange(i - len, newSeqSteps.Count); // remove all repeated outer steps
                                steps = newSteps;
                                // adjust outer steps and continue looking for more separated repetitions
                                i -= len;
                                continue;
                            }

                            // convert: a? (c a)* => {a, c}
                            // this conversion is technically not correct, but unfortunately this pattern is 
                            // appears in the antlr command grammars, probably unintentionally.
                            // should have been written as:  (a (c a)*)?
                            if (zom.Separator == null
                                && zom.Repeated is SequenceGrammar zseq2
                                && zseq2.Steps.Count == 2
                                && i > 0
                                && steps[i - 1] is OptionalGrammar opt2
                                && zseq2.Steps[1].IsEquivalentTo(opt2.Optioned))
                            {
                                newSteps = newSteps ?? steps.ToList();
                                newSteps[i] = new ZeroOrMoreGrammar(zseq2.Steps[1], zseq2.Steps[0]);
                                newSteps.RemoveAt(i - 1);
                                steps = newSteps;
                                i -= 1;
                                continue;
                            }

                            // convert: {a, c} [c] => {a, c+}
                            if (zom.Separator != null
                                && !zom.AllowTrailingSeparator
                                && i < steps.Count - 1
                                && steps[i + 1] is OptionalGrammar opt1
                                && opt1.Optioned.IsEquivalentTo(zom.Separator))
                            {
                                newSteps = newSteps ?? steps.ToList();
                                newSteps.RemoveAt(i + 1); // remove extraneous trailing separator
                                newSteps[i] = zom.With(zom.Repeated, zom.Separator, true);
                                steps = newSteps;
                                continue;
                            }
                            break;

                        case OneOrMoreGrammar oom:
                            if (oom.Separator != null
                                && !oom.AllowTrailingSeparator
                                && i < steps.Count - 1
                                && steps[i + 1] is OptionalGrammar opt3
                                && opt3.Optioned.IsEquivalentTo(oom.Separator))
                            {
                                // convert: (c a)+ [c] => {a, c+}+
                                newSteps = newSteps ?? steps.ToList();
                                newSteps.RemoveAt(i + 1); // remove extraneous trailing separator
                                newSteps[i] = oom.With(oom.Repeated, oom.Separator, true);
                                steps = newSteps;
                                continue;
                            }
                            break;
                    }

                    // nothing special, advance to next step
                    i++;
                }

                return steps;
            }

            private static bool AreEquivalent(
                IReadOnlyList<Grammar> listA, int startA,
                IReadOnlyList<Grammar> listB, int startB,
                int length)
            {
                for (int i = 0; i < length; i++)
                {
                    if (!listA[i + startA].IsEquivalentTo(listB[i + startB]))
                        return false;
                }

                return true;
            }

            public override Grammar VisitOneOrMore(OneOrMoreGrammar grammar)
            {
                var newRepeated = grammar.Repeated.Accept(this);
                var newSeparator = grammar.Separator?.Accept(this);

                switch (newRepeated)
                {
                    // (null)+ => null
                    case null:
                        return null;

                    // (e+)+ => e+
                    // ({e, s}+)+ => {e, s}+
                    case OneOrMoreGrammar o when newSeparator == null:
                        return grammar.With(o.Repeated, o.Separator, false);

                    // (e*)+ => e*
                    // ({e, s})+ => {e, s}
                    case ZeroOrMoreGrammar z when newSeparator == null:
                        return new ZeroOrMoreGrammar(z.Repeated, z.Separator, z.AllowTrailingSeparator);

                    // (e?)+ => e*
                    case OptionalGrammar o:
                        return new ZeroOrMoreGrammar(o.Optioned, newSeparator, grammar.AllowTrailingSeparator);

                    default:
                        if (newRepeated == grammar.Repeated
                            && newSeparator == grammar.Separator)
                            return grammar;

                        return grammar.With(newRepeated, newSeparator, grammar.AllowTrailingSeparator);
                }
            }

            public override Grammar VisitZeroOrMore(ZeroOrMoreGrammar grammar)
            {
                var newRepeated = grammar.Repeated.Accept(this);
                var newSeparator = grammar.Separator?.Accept(this);

                switch (newRepeated)
                {
                    // (null)* => null
                    case null:
                        return null;

                    // (g+)* => g*
                    case OneOrMoreGrammar o when newSeparator == null:
                        return grammar.With(o.Repeated, o.Separator, false);

                    // (g*)+ => g*
                    // ({g, s})+ => {g, s}
                    case ZeroOrMoreGrammar z when newSeparator == null:
                        return grammar.With(z.Repeated, z.Separator, z.AllowTrailingSeparator);

                    // (e?)+ => e*
                    case OptionalGrammar o:
                        return grammar.With(o.Optioned, newSeparator, grammar.AllowTrailingSeparator);

                    default:
                        if (newRepeated == grammar.Repeated
                            && newSeparator == grammar.Separator)
                            return grammar;

                        return grammar.With(newRepeated, newSeparator, grammar.AllowTrailingSeparator);
                }
            }

            public override Grammar VisitOptional(OptionalGrammar grammar)
            {
                var newOptioned = grammar.Optioned.Accept(this);

                switch (newOptioned)
                {
                    // null? => null
                    case null:
                        return null;

                    // (g+)? => g*
                    case OneOrMoreGrammar o:
                        return new ZeroOrMoreGrammar(o.Repeated, o.Separator);

                    // (g*)? => g*
                    case ZeroOrMoreGrammar _:
                        return newOptioned;

                    // (g?)? => g?
                    case OptionalGrammar _:
                        return newOptioned;

                    // (g!)? => g!
                    case RequiredGrammar r:
                        return r;

                    default:
                        if (newOptioned == grammar.Optioned)
                            return grammar;

                        return grammar.With(newOptioned);
                }
            }

            public override Grammar VisitRequired(RequiredGrammar grammar)
            {
                var newRequired = grammar.Required.Accept(this);

                switch (newRequired)
                {
                    // (null)! => null
                    case null:
                        return null;
                    // (g?)! => g?
                    case OptionalGrammar o:
                        return o;
                    // (g!)! => g!
                    case RequiredGrammar r:
                        return r;
                    default:
                        if (newRequired == grammar.Required)
                            return grammar;

                        return grammar.With(newRequired);
                }
            }

            public override Grammar VisitTagged(TaggedGrammar grammar)
            {
                var newTagged = grammar.Tagged.Accept(this);

                if (newTagged is TaggedGrammar t
                    && t.Tag == grammar.Tag)
                {
                    return t;
                }

                if (newTagged == grammar.Tagged)
                    return grammar;

                if (newTagged == null)
                    return null;

                return grammar.With(grammar.Tag, newTagged);
            }

            public override Grammar VisitRule(RuleGrammar grammar) =>
                grammar;

            public override Grammar VisitToken(TokenGrammar grammar) =>
                grammar;
        }
    }
}