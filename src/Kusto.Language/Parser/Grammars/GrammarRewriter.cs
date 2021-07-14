using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kusto.Language.Parsing
{
    /// <summary>
    /// A base visitor class for implementing grammar tree rewriters.
    /// The default implementations of this class handle reconstructing parent nodes when child nodes are changed.
    /// </summary>
    public abstract class GrammarRewriter : GrammarVisitor<Grammar>
    {
        public override Grammar VisitAlternation(AlternationGrammar grammar)
        {
            var newList = VisitList(grammar.Alternatives);
            if (newList.Count == 0)
            {
                return null;
            }
            else if (newList.Count == 1)
            {
                return newList[0];
            }
            else
            {
                return grammar.With(newList);
            }
        }

        public override Grammar VisitSequence(SequenceGrammar grammar)
        {
            var newList = VisitList(grammar.Steps);
            if (newList.Count == 0)
            {
                return null;
            }
            else if (newList.Count == 1)
            {
                return newList[0];
            }
            else
            {
                return grammar.With(newList);
            }
        }

        public override Grammar VisitOneOrMore(OneOrMoreGrammar grammar) =>
            grammar.With(grammar.Repeated.Accept(this), grammar.Separator?.Accept(this));

        public override Grammar VisitZeroOrMore(ZeroOrMoreGrammar grammar) =>
            grammar.With(grammar.Repeated.Accept(this), grammar.Separator?.Accept(this));

        public override Grammar VisitOptional(OptionalGrammar grammar) =>
            grammar.With(grammar.Optioned.Accept(this));

        public override Grammar VisitRequired(RequiredGrammar grammar) =>
            grammar.With(grammar.Required.Accept(this));

        public override Grammar VisitTagged(TaggedGrammar grammar) =>
            grammar.With(grammar.Tag, grammar.Tagged.Accept(this));

        public override Grammar VisitRule(RuleGrammar grammar) =>
            grammar;

        public override Grammar VisitToken(TokenGrammar grammar) =>
            grammar;

        public IReadOnlyList<Grammar> VisitList(IReadOnlyList<Grammar> list)
        {
            List<Grammar> newList = null;

            for (int i = 0; i < list.Count; i++)
            {
                var elem = list[i];
                var newElem = elem.Accept(this);
                if (newElem != elem || newList != null)
                {
                    if (newList == null)
                        newList = list.Take(i).ToList();
                    if (newElem != null)
                        newList.Add(newElem);
                }
            }

            return newList ?? list;
        }
    }
}