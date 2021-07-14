using System;
using System.Linq;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    /// <summary>
    /// Reorders alternations to place sequences with explicit terms ahead of 
    /// sequences of optional terms.
    /// </summary>
    internal class GrammarReorderer
    {
        /// <summary>
        /// Reorders all the contained alternations
        /// </summary>
        public static Grammar ReorderAlternations(Grammar root)
        {
            return root.Accept(s_treeMapper);
        }

        /// <summary>
        /// Reorders the list of alternatives
        /// </summary>
        public static IReadOnlyList<Grammar> Reorder(IReadOnlyList<Grammar> alternatives) =>
            Reorder(alternatives, a => a);

        /// <summary>
        /// Reorders a list of items that keyed by a grammar.
        /// </summary>
        public static IReadOnlyList<T> Reorder<T>(IReadOnlyList<T> items, Func<T, Grammar> keySelector)
        {
            // do stable sort
            return items
                .Select((item, index) => (item, index))
                .OrderBy(x => keySelector(x.item), GrammarCompararer.Instance)
                .ThenBy(x => x.index)
                .Select(x => x.item)
                .ToList();
        }

        private static GrammarTreeMapper s_treeMapper =
            new GrammarTreeMapper(new Orderer());

        private class Orderer : DefaultGrammarVisitor<Grammar>
        {
            public override Grammar DefaultVisit(Grammar grammar) =>
                grammar;

            public override Grammar VisitAlternation(AlternationGrammar grammar)
            {
                var orderedAlts = Reorder(grammar.Alternatives);
                return grammar.With(orderedAlts);
            }
        }
    }

    internal class GrammarCompararer : IComparer<Grammar>
    {
        public static readonly GrammarCompararer Instance = new GrammarCompararer();

        public int Compare(Grammar x, Grammar y)
        {
            if (x == y)
                return 0;

            var sx = x as SequenceGrammar;
            var sy = y as SequenceGrammar;
            if (sx != null && sy != null)
                return CompareSequence(sx, sy);

            var fx = GetFirstElement(x);
            var fy = GetFirstElement(y);

            // non-optional elements always beat optional elements
            if (!fx.IsOptional && fy.IsOptional)
            {
                return -1;
            }
            else if (fx.IsOptional && !fy.IsOptional)
            {
                return 1;
            }

            // assume alternations are already ordered
            var ax = fx.Grammar as AlternationGrammar;
            var ay = fy.Grammar as AlternationGrammar;
            if (ax != null && ay != null)
            {
                return Compare(ax.Alternatives[0], ay.Alternatives[0]);
            }
            else if (ax != null)
            {
                return Compare(ax.Alternatives[0], y);
            }
            else if (ay != null)
            {
                return Compare(x, ay.Alternatives[0]);
            }

            // token always beats non-token
            var tx = fx.Grammar as TokenGrammar;
            var ty = fy.Grammar as TokenGrammar;

            if (tx != null && ty != null)
            {
                // both are tokens, use text ordering
                var result = tx.TokenText.CompareTo(ty.TokenText);
                if (result != 0)
                    return result;
            }
            else if (tx != null && ty == null)
            {
                // x is token, but y is not: x wins
                return -1;
            }
            else if (tx == null && ty != null)
            {
                // y is token but x is not: y wins
                return 1;
            }

            // otherwise rule always beats non-rule
            var rx = fx.Grammar as RuleGrammar;
            var ry = fx.Grammar as RuleGrammar;

            if (rx != null && ry != null)
            {
                // both are rules, use rule name ordering
                var result = rx.RuleName.CompareTo(ry.RuleName);
                if (result != 0)
                    return result;
            }
            else if (rx != null && ry == null)
            {
                return -1;
            }
            else if (rx == null && ry != null)
            {
                return 1;
            }

            if (sx != null && IsFallible(sx.Steps, 1))
            {
                // x has an additional fallible term
                return -1;
            }
            else if (sy != null && IsFallible(sy.Steps, 1))
            {
                // y has an additional fallible term
                return 1;
            }
            else
            {
                // neither are tokens, rules or alternations, so give up
                return 0;
            }
        }

        private int CompareSequence(SequenceGrammar sx, SequenceGrammar sy)
        {
            var n = Math.Min(sx.Steps.Count, sy.Steps.Count);
            for (int i = 0; i < n; i++)
            {
                var result = Compare(sx.Steps[i], sy.Steps[i]);
                if (result != 0)
                    return result;
            }

            if (sx.Steps.Count > n)
            {
                if (IsFallible(sx.Steps, n))
                {
                    // sx wins because sx has additional grammar that can fail
                    return -1;
                }
                else
                {
                    // sy wins because it is shorter
                    return 1;
                }
            }
            else if (sy.Steps.Count > n)
            {
                if (IsFallible(sy.Steps, n))
                {
                    // sy wins because it has additional grammar that can fail
                    return 1;
                }
                else
                {
                    // sx wins because it is shorter
                    return -1;
                }
            }
            else
            {
                // neither win because they are both the same length
                return 0;
            }
        }

        private static bool IsFallible(Grammar grammar)
        {
            switch (grammar)
            {
                case TokenGrammar _:
                case RuleGrammar _:
                    return true;
                case TaggedGrammar t:
                    return IsFallible(t.Tagged);
                case OneOrMoreGrammar oom:
                    return IsFallible(oom.Repeated);
                case SequenceGrammar seq:
                    return IsFallible(seq.Steps);
                case AlternationGrammar alt:
                    return alt.Alternatives.Any(a => IsFallible(a));
                default:
                    return false;
            }
        }

        private static bool IsFallible(IReadOnlyList<Grammar> steps) =>
            IsFallible(steps, 0, steps.Count);

        private static bool IsFallible(IReadOnlyList<Grammar> steps, int start) =>
            IsFallible(steps, start, steps.Count - start);

        private static bool IsFallible(IReadOnlyList<Grammar> steps, int start, int length)
        {
            for (int i = start, n = start + length; i < n; i++)
            {
                if (IsFallible(steps[i]))
                    return true;
            }

            return false;
        }

        private struct Element
        {
            public readonly Grammar Grammar;
            public readonly bool IsOptional;

            public Element(Grammar first, bool optional) 
            { 
                this.Grammar = first; 
                this.IsOptional = optional; 
            }
        }

        private static Element GetFirstElement(Grammar g)
        {
            switch (g)
            {
                case TokenGrammar _:
                case RuleGrammar _:
                    return new Element(g, false);
                case SequenceGrammar seq:
                    return GetFirstElement(seq.Steps[0]);
                case AlternationGrammar alt:
                    // cannot break this down any more
                    return new Element(alt, false);
                case OneOrMoreGrammar oom:
                    return GetFirstElement(oom.Repeated);
                case ZeroOrMoreGrammar zom:
                    return new Element(GetFirstElement(zom.Repeated).Grammar, true);
                case OptionalGrammar opt:
                    return new Element(GetFirstElement(opt.Optioned).Grammar, true);
                case RequiredGrammar req:
                    return new Element(GetFirstElement(req.Required).Grammar, true);
                case TaggedGrammar tag:
                    return GetFirstElement(tag.Tagged);
                default:
                    return new Element(g, false);
            }
        }
    }
}