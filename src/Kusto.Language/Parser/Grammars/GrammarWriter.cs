using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kusto.Language.Parsing
{
    public enum GrammarStyle
    {
        /// <summary>
        /// Use syntax like {x} for repetitions and [x] for optional
        /// </summary>
        Brackets,

        /// <summary>
        /// Use syntax like x* for repetitions and x? for optional
        /// </summary>
        Antlr
    }

    /// <summary>
    /// A class used to convert grammar trees into text.
    /// </summary>
    public static class GrammarWriter
    {
        public static void WriteTo(StringBuilder builder, Grammar grammar, GrammarStyle style)
        {
            var writer = new Writer(builder, style);
            grammar.Accept(writer);
        }

        private class Writer : GrammarVisitor<bool>
        {
            private readonly StringBuilder _builder;
            private readonly GrammarStyle _style;

            public Writer(StringBuilder builder, GrammarStyle style)
            {
                _builder = builder ?? new StringBuilder();
                _style = style;
            }

            public override bool VisitAlternation(AlternationGrammar grammar)
            {
                for (int i = 0; i < grammar.Alternatives.Count; i++)
                {
                    var alt = grammar.Alternatives[i];

                    if (i > 0)
                        _builder.Append(" | ");

                    if (alt is AlternationGrammar)
                    {
                        // only nested alternations need to be bracketed inside an alternation
                        WriteParens(alt);
                    }
                    else
                    {
                        Write(alt);
                    }
                }

                return true;
            }

            public override bool VisitOneOrMore(OneOrMoreGrammar grammar)
            {
                if (_style == GrammarStyle.Brackets || grammar.Separator != null)
                {
                    _builder.Append("{");
                    Write(grammar.Repeated);

                    if (grammar.Separator != null)
                    {
                        _builder.Append(", ");
                        Write(grammar.Separator);
                    }

                    _builder.Append("}+");
                    return false;
                }
                else
                {
                    switch (grammar.Repeated)
                    {
                        case TokenGrammar _:
                        case RuleGrammar _:
                            Write(grammar.Repeated);
                            break;
                        default:
                            WriteParens(grammar.Repeated);
                            break;
                    }

                    _builder.Append("+");
                }

                return true;
            }

            public override bool VisitOptional(OptionalGrammar grammar)
            {
                if (_style == GrammarStyle.Brackets)
                {
                    _builder.Append("[");
                    Write(grammar.Optioned);
                    _builder.Append("]");
                }
                else
                {
                    switch (grammar.Optioned)
                    {
                        case TokenGrammar _:
                        case RuleGrammar _:
                            Write(grammar.Optioned);
                            break;
                        default:
                            WriteParens(grammar.Optioned);
                            break;
                    }
                    _builder.Append("?");
                }

                return true;
            }

            public override bool VisitRequired(RequiredGrammar grammar)
            {
                switch (grammar.Required)
                {
                    case TokenGrammar _:
                    case RuleGrammar _:
                        Write(grammar.Required);
                        break;
                    default:
                        WriteParens(grammar.Required);
                        break;
                }

                _builder.Append("!");

                return true;
            }

            public override bool VisitRule(RuleGrammar grammar)
            {
                _builder.Append("<");
                _builder.Append(grammar.RuleName);
                _builder.Append(">");

                return true;
            }

            public override bool VisitSequence(SequenceGrammar grammar)
            {
                for (int i = 0; i < grammar.Steps.Count; i++)
                {
                    if (i > 0)
                        _builder.Append(" ");

                    var step = grammar.Steps[i];

                    if (step is AlternationGrammar || step is SequenceGrammar)
                    {
                        // only needs parens if is a nested alternation or sequence
                        WriteParens(step);
                    }
                    else
                    {
                        Write(step);
                    }
                }

                return true;
            }

            public override bool VisitTagged(TaggedGrammar grammar)
            {
                _builder.Append(grammar.Tag);
                _builder.Append("=");

                switch (grammar.Tagged)
                {
                    case AlternationGrammar _:
                    case SequenceGrammar _:
                    case TaggedGrammar _:
                        // only needs parens if is alternation, sequence or another tag
                        WriteParens(grammar.Tagged);
                        break;
                    default:
                        Write(grammar.Tagged);
                        break;
                }

                return true;
            }

            private static bool IsSimpleToken(string text)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    var ch = text[i];
                    if (!(char.IsLetterOrDigit(ch) || ch == '_' || ch == '-'))
                        return false;
                }

                return true;
            }

            public override bool VisitToken(TokenGrammar grammar)
            {
                if (IsSimpleToken(grammar.TokenText))
                {
                    _builder.Append(grammar.TokenText);
                }
                else
                {
                    _builder.Append('\'');
                    _builder.Append(grammar.TokenText);
                    _builder.Append('\'');
                }

                return true;
            }

            public override bool VisitZeroOrMore(ZeroOrMoreGrammar grammar)
            {
                if (_style == GrammarStyle.Brackets || grammar.Separator != null)
                {
                    _builder.Append("{");
                    Write(grammar.Repeated);

                    if (grammar.Separator != null)
                    {
                        _builder.Append(", ");
                        Write(grammar.Separator);
                    }

                    _builder.Append("}");
                }
                else
                {
                    switch (grammar.Repeated)
                    {
                        case TokenGrammar _:
                        case RuleGrammar _:
                            Write(grammar.Repeated);
                            break;
                        default:
                            WriteParens(grammar.Repeated);
                            break;
                    }

                    _builder.Append("*");
                }

                return true;
            }

            private void Write(Grammar grammar)
            {
                grammar.Accept(this);
            }

            private void WriteParens(Grammar grammar)
            {
                _builder.Append("(");
                grammar.Accept(this);
                _builder.Append(")");
            }
        }
    }
}