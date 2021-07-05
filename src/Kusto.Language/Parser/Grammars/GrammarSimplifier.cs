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

                if (newAlts == grammar.Alternatives)
                    return grammar;

                return grammar.With(newAlts);
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

                for (int i = 0; i < steps.Count; i++)
                {
                    var step = steps[i];

                    // convert: a b (c a b)* => {a b, c}
                    if (step is ZeroOrMoreGrammar z
                        && z.Separator == null
                        && z.Repeated is SequenceGrammar zseq
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

                        // adjust outer steps and continue looking for more separated repetitions
                        steps = newSteps;
                        i -= len;
                        continue;
                    }
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
                    // o(null) => null
                    case null:
                        return null;

                    // o(o(e)) => o(e)
                    case OneOrMoreGrammar o when o.Separator == null:
                        return grammar.With(o.Repeated, newSeparator);
                    case OneOrMoreGrammar o when newSeparator == null:
                        return grammar.With(o.Repeated, o.Separator);

                    // o(z(e)) => z(e)
                    case ZeroOrMoreGrammar z when z.Separator == null:
                        return new ZeroOrMoreGrammar(z.Repeated, newSeparator);
                    case ZeroOrMoreGrammar z when newSeparator == null:
                        return new ZeroOrMoreGrammar(z.Repeated, z.Separator);

                    // o(opt(e)) => z(e)
                    case OptionalGrammar o:
                        return new ZeroOrMoreGrammar(o.Optioned, newSeparator);

                    default:
                        if (newRepeated == grammar.Repeated
                            && newSeparator == grammar.Separator)
                            return grammar;

                        return grammar.With(newRepeated, newSeparator);
                }
            }

            public override Grammar VisitZeroOrMore(ZeroOrMoreGrammar grammar)
            {
                var newRepeated = grammar.Repeated.Accept(this);
                var newSeparator = grammar.Separator?.Accept(this);

                switch (newRepeated)
                {
                    // z(null) => null
                    case null:
                        return null;

                    // z(o(g)) => z(g)
                    case OneOrMoreGrammar o when o.Separator == null:
                        return grammar.With(o.Repeated, newSeparator);
                    case OneOrMoreGrammar o when newSeparator == null:
                        return grammar.With(o.Repeated, o.Separator);

                    // z(z(g)) => z(g)
                    case ZeroOrMoreGrammar z when z.Separator == null:
                        return grammar.With(z.Repeated, newSeparator);
                    case ZeroOrMoreGrammar z when newSeparator == null:
                        return grammar.With(z.Repeated, z.Separator);

                    // z(opt(e)) => z(e)
                    case OptionalGrammar o:
                        return grammar.With(o.Optioned, newSeparator);

                    default:
                        if (newRepeated == grammar.Repeated
                            || newSeparator == grammar.Separator)
                            return grammar;


                        return grammar.With(newRepeated, newSeparator);
                }
            }

            public override Grammar VisitOptional(OptionalGrammar grammar)
            {
                var newOptioned = grammar.Optioned.Accept(this);

                switch (newOptioned)
                {
                    // opt(null) => null
                    case null:
                        return null;

                    // opt(o(g)) => z(g);
                    case OneOrMoreGrammar o:
                        return new ZeroOrMoreGrammar(o.Repeated, o.Separator);

                    // opt(z(g)) => z(g)
                    case ZeroOrMoreGrammar _:
                        return newOptioned;

                    // opt(opt(g)) => opt(g)
                    case OptionalGrammar _:
                        return newOptioned;

                    // opt(req(g)) => req(g)
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
                    // req(null) => null
                    case null:
                        return null;
                    // req(opt(g)) => opt(g)
                    case OptionalGrammar o:
                        return o;
                    // req(req(g)) => req(g)
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