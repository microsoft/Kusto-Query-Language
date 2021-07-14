using System;

namespace Kusto.Language.Parsing
{
    /// <summary>
    /// A rewriter that apples the function or visitor to each node or rewritten node in the tree, bottom up.
    /// </summary>
    public sealed class GrammarTreeMapper : GrammarRewriter
    {
        private readonly Func<Grammar, Grammar> _fnMap;

        public GrammarTreeMapper(Func<Grammar, Grammar> fnMap)
        {
            _fnMap = fnMap;
        }

        public GrammarTreeMapper(GrammarVisitor<Grammar> visitor)
            : this(g => g.Accept(visitor))
        {
        }

        private Grammar Map(Grammar grammar) =>
            grammar != null ? _fnMap(grammar) : null;

        public override Grammar VisitAlternation(AlternationGrammar grammar) =>
            Map(base.VisitAlternation(grammar));

        public override Grammar VisitSequence(SequenceGrammar grammar) =>
            Map(base.VisitSequence(grammar));

        public override Grammar VisitOneOrMore(OneOrMoreGrammar grammar) =>
            Map(base.VisitOneOrMore(grammar));

        public override Grammar VisitZeroOrMore(ZeroOrMoreGrammar grammar) =>
            Map(base.VisitZeroOrMore(grammar));

        public override Grammar VisitOptional(OptionalGrammar grammar) =>
            Map(base.VisitOptional(grammar));

        public override Grammar VisitRequired(RequiredGrammar grammar) =>
            Map(base.VisitRequired(grammar));

        public override Grammar VisitTagged(TaggedGrammar grammar) =>
            Map(base.VisitTagged(grammar));

        public override Grammar VisitRule(RuleGrammar grammar) =>
            Map(base.VisitRule(grammar));

        public override Grammar VisitToken(TokenGrammar grammar) =>
            Map(base.VisitToken(grammar));
    }
}