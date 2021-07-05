using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kusto.Language.Parsing
{
    /// <summary>
    /// The base definition of a grammar tree nodes.
    /// </summary>
    public abstract class Grammar
    {
        public abstract TResult Accept<TResult>(GrammarVisitor<TResult> visitor);

        public bool IsEquivalentTo(Grammar grammar) =>
            AreEquivalent(this, grammar);

        public static bool AreEquivalent(Grammar a, Grammar b)
        {
            if (a == b)
                return true;

            if (a == null || b == null)
                return false;

            if (a.GetType() == b.GetType())
            {
                switch (a)
                {
                    case SequenceGrammar seqa:
                        var seqb = (SequenceGrammar)b;
                        return AreEquivalent(seqa.Steps, seqb.Steps);

                    case AlternationGrammar alta:
                        var altb = (AlternationGrammar)b;
                        return AreEquivalent(alta.Alternatives, altb.Alternatives);

                    case OptionalGrammar opta:
                        var optb = (OptionalGrammar)b;
                        return AreEquivalent(opta.Optioned, optb.Optioned);

                    case RequiredGrammar reqa:
                        var reqb = (RequiredGrammar)b;
                        return AreEquivalent(reqa.Required, reqb.Required);

                    case ZeroOrMoreGrammar zoma:
                        var zomb = (ZeroOrMoreGrammar)b;
                        return AreEquivalent(zoma.Repeated, zomb.Repeated)
                            && AreEquivalent(zoma.Separator, zomb.Separator);

                    case OneOrMoreGrammar ooma:
                        var oomb = (OneOrMoreGrammar)b;
                        return AreEquivalent(ooma.Repeated, oomb.Repeated)
                            && AreEquivalent(ooma.Separator, oomb.Separator);

                    case TaggedGrammar nama:
                        var namb = (TaggedGrammar)b;
                        return AreEquivalent(nama.Tagged, namb.Tagged);

                    case TokenGrammar toka:
                        var tokb = (TokenGrammar)b;
                        return toka.TokenText == tokb.TokenText;

                    case RuleGrammar rula:
                        var rulb = (RuleGrammar)b;
                        return rula.RuleName == rulb.RuleName;
                }
            }

            return false;
        }

        public static bool AreEquivalent(IReadOnlyList<Grammar> a, IReadOnlyList<Grammar> b)
        {
            if (a == b)
                return true;
            if (a == null || b == null)
                return false;
            if (a.Count != b.Count)
                return false;
            for (int i = 0; i < a.Count; i++)
            {
                if (!AreEquivalent(a[i], b[i]))
                    return false;
            }
            return true;
        }

        public override string ToString() =>
            ToString(GrammarStyle.Brackets);

        public string ToString(GrammarStyle style)
        {
            var builder = new StringBuilder();
            GrammarWriter.WriteTo(builder, this, style);
            return builder.ToString();
        }
    }

    /// <summary>
    /// A grammar element that represents a sequence of two or more grammar elements.
    /// </summary>
    public sealed class SequenceGrammar : Grammar
    {
        public IReadOnlyList<Grammar> Steps { get; }

        public SequenceGrammar(IReadOnlyList<Grammar> steps)
        {
            if (steps == null)
                throw new ArgumentNullException(nameof(steps));
            if (steps.Count < 2)
                throw new InvalidOperationException($"Number of steps must be 2 or more");
            this.Steps = steps;
        }

        public override TResult Accept<TResult>(GrammarVisitor<TResult> visitor) =>
            visitor.VisitSequence(this);

        public SequenceGrammar With(IReadOnlyList<Grammar> steps) =>
            this.Steps != steps ? new SequenceGrammar(steps) : this;
    }

    /// <summary>
    /// A grammar element that represents two or more alternative grammars.
    /// </summary>
    public sealed class AlternationGrammar : Grammar
    {
        public IReadOnlyList<Grammar> Alternatives { get; }

        public AlternationGrammar(IReadOnlyList<Grammar> alternatives)
        {
            if (alternatives == null)
                throw new ArgumentNullException(nameof(alternatives));
            if (alternatives.Count < 2)
                throw new InvalidOperationException($"Number of alternatives must be 2 or more");
            this.Alternatives = alternatives;
        }

        public override TResult Accept<TResult>(GrammarVisitor<TResult> visitor) =>
            visitor.VisitAlternation(this);

        public AlternationGrammar With(IReadOnlyList<Grammar> alternatives) =>
            this.Alternatives != alternatives ? new AlternationGrammar(alternatives) : this;
    }

    /// <summary>
    /// A grammar element that represents an optional grammar.
    /// </summary>
    public sealed class OptionalGrammar : Grammar
    {
        public Grammar Optioned { get; }

        public OptionalGrammar(Grammar optioned)
        {
            this.Optioned = optioned;
        }

        public override TResult Accept<TResult>(GrammarVisitor<TResult> visitor) =>
            visitor.VisitOptional(this);

        public OptionalGrammar With(Grammar optioned) =>
            this.Optioned != optioned ? new OptionalGrammar(optioned) : this;
    }

    /// <summary>
    /// A grammar element that represents a required grammar
    /// </summary>
    public sealed class RequiredGrammar : Grammar
    {
        public Grammar Required { get; }

        public RequiredGrammar(Grammar required)
        {
            this.Required = required;
        }

        public override TResult Accept<TResult>(GrammarVisitor<TResult> visitor) =>
            visitor.VisitRequired(this);

        public RequiredGrammar With(Grammar required) =>
            this.Required != required ? new RequiredGrammar(required) : this;
    }

    /// <summary>
    /// A grammar element that represents zero or more repetitions of a grammar element
    /// and an optional separator.
    /// </summary>
    public sealed class ZeroOrMoreGrammar : Grammar
    {
        public Grammar Repeated { get; }
        public Grammar Separator { get; }

        public ZeroOrMoreGrammar(Grammar repeated, Grammar separator = null)
        {
            this.Repeated = repeated;
            this.Separator = separator;
        }

        public override TResult Accept<TResult>(GrammarVisitor<TResult> visitor) =>
            visitor.VisitZeroOrMore(this);

        public ZeroOrMoreGrammar With(Grammar repeated, Grammar separator) =>
            (this.Repeated != repeated || this.Separator != separator)
                ? new ZeroOrMoreGrammar(repeated, separator)
                : this;
    }

    /// <summary>
    /// A grammar element that represents one or more repetitions of a grammar element
    /// and an optional separator.
    /// </summary>
    public sealed class OneOrMoreGrammar : Grammar
    {
        public Grammar Repeated { get; }
        public Grammar Separator { get; }

        public OneOrMoreGrammar(Grammar repeated, Grammar separator = null)
        {
            this.Repeated = repeated;
            this.Separator = separator;
        }

        public override TResult Accept<TResult>(GrammarVisitor<TResult> visitor) =>
            visitor.VisitOneOrMore(this);

        public OneOrMoreGrammar With(Grammar repeated, Grammar separator) =>
            (this.Repeated != repeated || this.Separator != separator)
                ? new OneOrMoreGrammar(repeated, separator)
                : this;
    }

    /// <summary>
    /// A grammar element that represents a grammar with name tag.
    /// </summary>
    public sealed class TaggedGrammar : Grammar
    {
        public string Tag { get; }
        public Grammar Tagged { get; }

        public TaggedGrammar(string tag, Grammar tagged)
        {
            this.Tag = tag;
            this.Tagged = tagged;
        }

        public override TResult Accept<TResult>(GrammarVisitor<TResult> visitor) =>
            visitor.VisitTagged(this);

        public TaggedGrammar With(string tag, Grammar tagged) =>
            (this.Tag != tag || this.Tagged != tagged)
                ? new TaggedGrammar(tag, tagged)
                : this;
    }

    /// <summary>
    /// A grammar element that represents an expected word or punctuation.
    /// </summary>
    public sealed class TokenGrammar : Grammar
    {
        public string TokenText { get; }

        public TokenGrammar(string tokenText)
        {
            this.TokenText = tokenText;
        }

        public override TResult Accept<TResult>(GrammarVisitor<TResult> visitor) =>
            visitor.VisitToken(this);

        public TokenGrammar With(string tokenText) =>
            this.TokenText != tokenText ? new TokenGrammar(tokenText) : this;
    }

    /// <summary>
    /// A grammar element that represent a reference to an external grammar rule.
    /// </summary>
    public sealed class RuleGrammar : Grammar
    {
        public string RuleName { get; }

        public RuleGrammar(string ruleName)
        {
            this.RuleName = ruleName;
        }

        public override TResult Accept<TResult>(GrammarVisitor<TResult> visitor) =>
            visitor.VisitRule(this);

        public RuleGrammar With(string ruleName) =>
            this.RuleName != ruleName ? new RuleGrammar(ruleName) : this;
    }

    /// <summary>
    /// The visitor pattern for <see cref="Grammar"/> nodes.
    /// </summary>
    public abstract class GrammarVisitor<TResult>
    {
        public abstract TResult VisitSequence(SequenceGrammar grammar);
        public abstract TResult VisitAlternation(AlternationGrammar grammar);
        public abstract TResult VisitOptional(OptionalGrammar grammar);
        public abstract TResult VisitRequired(RequiredGrammar grammar);
        public abstract TResult VisitZeroOrMore(ZeroOrMoreGrammar grammar);
        public abstract TResult VisitOneOrMore(OneOrMoreGrammar grammar);
        public abstract TResult VisitTagged(TaggedGrammar grammar);
        public abstract TResult VisitToken(TokenGrammar grammar);
        public abstract TResult VisitRule(RuleGrammar grammar);
    }
}
