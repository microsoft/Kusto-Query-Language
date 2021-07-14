using System;
using System.Linq;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    /// <summary>
    /// Rewrites sequences with leading optional elements into alternate paths with and without the optional element.
    /// </summary>
    internal class GrammarUnroller
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

            public override Grammar VisitSequence(SequenceGrammar grammar)
            {
                var step = grammar.Steps[0];

                if (step is OptionalGrammar opt)
                {
                    // first step is optional, so split into two alternatives, with and without
                    var listWithout = grammar.Steps.Skip(1).ToList();
                    var listWith = new[] { opt.Optioned }.Concat(listWithout).ToList();

                    return new AlternationGrammar(
                        new[] {
                            new SequenceGrammar(listWith),
                            new SequenceGrammar(listWithout)
                        });
                }
                else if (step is ZeroOrMoreGrammar zero)
                {
                    // first step is zero-or-more, so split into two alternatives, one with one-ore-more, and one without
                    var listWithout = grammar.Steps.Skip(1).ToList();
                    var listWith = new[] { new OneOrMoreGrammar(zero.Repeated, zero.Separator) }.Concat(listWithout).ToList();

                    return new AlternationGrammar(
                        new[] {
                            new SequenceGrammar(listWith),
                            new SequenceGrammar(listWithout)
                        });
                }

                return grammar;
            }
        }
    }
}