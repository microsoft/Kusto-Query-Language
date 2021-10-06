using System;
using System.Linq;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    /// <summary>
    /// Rewrites sequences with optional elements into alternate paths with and without the optional element.
    /// </summary>
    public class GrammarUnroller
    {
        public static Grammar Unroll(Grammar grammar)
        {
            return grammar.Accept(s_treeMapper);
        }

        private static GrammarTreeMapper s_treeMapper =
            new GrammarTreeMapper(new Unroller());

        private class Unroller : DefaultGrammarVisitor<Grammar>
        {
            public override Grammar DefaultVisit(Grammar grammar) =>
                grammar;

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

            public override Grammar VisitSequence(SequenceGrammar grammar)
            {
                var newSteps = VisitSteps(grammar.Steps);

                if (newSteps.Count == 0)
                {
                    return null;
                }
                else if (newSteps.Count == 1)
                {
                    return newSteps[0];
                }
                else
                {
                    return grammar.With(newSteps);
                }
            }

            public IReadOnlyList<Grammar> VisitSteps(IReadOnlyList<Grammar> steps)
            {
                if (steps.Any(s => IsOptional(s)))
                {
                    var newSteps = new List<Grammar>(steps.Count + 2);

                    for (int i = 0; i < steps.Count; i++)
                    {
                        var step = steps[i];
                        var nextStep = i < steps.Count - 1 ? steps[i + 1] : null;

                        // consider: including all following steps
                        if (nextStep != null)
                        {
                            if (step is TaggedGrammar tg  
                                && tg.Tagged is OptionalGrammar tgo)
                            {
                                // rewrite: n=x? to (n=x)?
                                step = new OptionalGrammar(new TaggedGrammar(tg.Tag, tgo.Optioned));
                            }

                            if (step is OptionalGrammar opt)
                            {
                                // [a] b -> (a b | b)
                                var newStep = new AlternationGrammar(
                                    new SequenceGrammar(
                                        opt.Optioned,
                                        nextStep),
                                    nextStep.Clone());
                                newSteps.Add(newStep);
                                i++; // skip next step
                                continue;
                            }
#if false
                            else if (step is ZeroOrMoreGrammar zero)
                            {
                                // {a} b -> (b | {a}+ b)
                                var newStep = new AlternationGrammar(
                                    nextStep,
                                    new SequenceGrammar(
                                        new OneOrMoreGrammar(zero.Repeated, zero.Separator),
                                        nextStep.Clone()));
                                newSteps.Add(newStep);
                                i++; // skip next step
                                continue;
                            }
#endif
                        }

                        newSteps.Add(step);
                    }

                    return newSteps;
                }
                else
                {
                    return steps;
                }
            }
        }
    }
}