using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kusto.Language.Parsing
{
    /// <summary>
    /// Builds a textual representation of a parser's grammar.
    /// </summary>
    public static class Describer
    {
        /// <summary>
        /// Returns a textual representation of a parser's grammar.
        /// </summary>
        public static string Describe<TInput>(Parser<TInput> parser, bool showRequired = false)
        {
            var builder = new Writer<TInput>(showRequired);
            builder.Visit(parser);
            return builder.ToString();
        }

        private class Writer<TInput> : ParserVisitor<TInput>
        {
            private StringBuilder builder;
            private string separator;
            private bool showRequired;

            public Writer(bool showRequired)
            {
                this.builder = new StringBuilder();
                this.showRequired = showRequired;
            }

            public override string ToString()
            {
                return this.builder.ToString();
            }

            public void Visit(Parser<TInput> parser)
            {
                if (parser != null && !parser.IsHidden)
                {
                    if (!string.IsNullOrEmpty(parser.Tag))
                    {
                        WriteTerm(parser.Tag);
                    }
                    else
                    {
                        parser.Accept(this);
                    }
                }
            }

            private void WriteTerm(string term)
            {
                if (!string.IsNullOrEmpty(term))
                {
                    builder.Append(term);
                }
            }

            private void WriteAlternation(IReadOnlyList<Parser<TInput>> parsers)
            {
                this.WriteSeparated(" | ", parsers);
            }

            private void WriteSequence(IReadOnlyList<Parser<TInput>> parsers)
            {
                this.WriteSeparated(" ", parsers);
            }

            private void WriteOptional(Parser<TInput> parser)
            {
                this.WriteBracketed("[", "]", parser);
            }

            private void WriteRequired(Parser<TInput> parser)
            {
                var nested = new Writer<TInput>(this.showRequired);
                nested.Visit(parser);

                if (nested.separator != null)
                {
                    this.builder.Append("(");
                    this.builder.Append(nested.ToString());
                    this.builder.Append(")");
                }
                else
                {
                    this.builder.Append(nested.ToString());
                }

                this.builder.Append("!");
            }

            private void WriteZeroOrMore(Parser<TInput> parser)
            {
                this.WriteBracketed("{", "}", parser);
            }

            private void WriteOneOrMore(Parser<TInput> parser)
            {
                this.WriteBracketed("{", "}+", parser);
            }

            private void WriteSeparated(string separator, IReadOnlyList<Parser<TInput>> parsers)
            {
                var builders = new List<Writer<TInput>>();

                for (int i = 0, n = parsers.Count; i < n; i++)
                {
                    var parser = parsers[i];
                    if (!parser.IsHidden)
                    {
                        var nestedBuilder = new Writer<TInput>(this.showRequired);

                        nestedBuilder.Visit(parser);

                        if (nestedBuilder.builder.Length > 0)
                        {
                            builders.Add(nestedBuilder);
                        }
                    }
                }

                if (builders.Count > 1)
                {
                    this.separator = separator;

                    for (int i = 0; i < builders.Count; i++)
                    {
                        if (i > 0)
                            this.builder.Append(separator);

                        var grammarBuilder = builders[i];
                        if (grammarBuilder.separator != null && grammarBuilder.separator != separator)
                        {
                            // if item is separated, but using a different separator, add parens
                            this.builder.Append("(");
                            this.builder.Append(grammarBuilder.builder.ToString());
                            this.builder.Append(")");
                        }
                        else
                        {
                            this.builder.Append(grammarBuilder.builder.ToString());
                        }
                    }
                }
                else if (builders.Count == 1)
                {
                    this.builder = builders[0].builder;
                    this.separator = builders[0].separator;
                }
            }

            private void WriteBracketed(string startBracket, string endBracket, Parser<TInput> parser, Action<Parser<TInput>, Writer<TInput>> action = null)
            {
                var nestedBuilder = new Writer<TInput>(this.showRequired);
                nestedBuilder.Visit(parser);

                var text = nestedBuilder.ToString();
                if (text.Length > 0)
                {
                    builder.Append(startBracket);
                    builder.Append(text);
                    builder.Append(endBracket);
                }
            }

            public override void VisitApply<TLeft, TOutput>(ApplyParser<TInput, TLeft, TOutput> parser)
            {
                switch (parser.ApplyKind)
                {
                    case ApplyKind.ZeroOrMore:
                        WriteSequence(new Parser<TInput>[] { parser.LeftParser, Parsers<TInput>.ZeroOrMore(parser.RightParser) });
                        break;

                    case ApplyKind.ZeroOrOne:
                        WriteSequence(new Parser<TInput>[] { parser.LeftParser, Parsers<TInput>.ZeroOrOne(parser.RightParser) });
                        break;

                    default:
                        WriteSequence(new Parser<TInput>[] { parser.LeftParser, parser.RightParser });
                        break;
                }
            }

            public override void VisitBest<TOutput>(BestParser<TInput, TOutput> parser)
            {
                WriteAlternation(parser.Parsers);
            }

            public override void VisitBest(BestParser<TInput> parser)
            {
                WriteAlternation(parser.Parsers);
            }

            public override void VisitConvert<TOutput>(ConvertParser<TInput, TOutput> parser)
            {
                this.Visit(parser.Pattern);
            }

            public override void VisitFails(FailsParser<TInput> parser)
            {
                WriteBracketed("fails(", ")", parser.Pattern);
            }

            public override void VisitFirst<TOutput>(FirstParser<TInput, TOutput> parser)
            {
                WriteAlternation(parser.Parsers);
            }

            public override void VisitFirst(FirstParser<TInput> parser)
            {
                WriteAlternation(parser.Parsers);
            }

            public override void VisitForward<TOutput>(ForwardParser<TInput, TOutput> parser)
            {
                WriteTerm("forward()");
            }

            public override void VisitIf<TOutput>(IfParser<TInput, TOutput> parser)
            {
                this.Visit(parser.Parser);
            }

            public override void VisitIf(IfParser<TInput> parser)
            {
                this.Visit(parser.Parser);
            }

            public override void VisitMap<TOutput>(MapParser<TInput, TOutput> parser)
            {
                WriteTerm("map()");
            }

            public override void VisitMatch(MatchParser<TInput> parser)
            {
                WriteTerm("match()");
            }

            public override void VisitMatch<TOutput>(MatchParser<TInput, TOutput> parser)
            {
                WriteTerm("match()");
            }

            public override void VisitNot(NotParser<TInput> parser)
            {
                WriteBracketed("not(", ")", parser.Pattern);
            }

            public override void VisitOneOrMore(OneOrMoreParser<TInput> parser)
            {
                WriteOneOrMore(parser.Parser);
            }

            public override void VisitOptional<TOutput>(OptionalParser<TInput, TOutput> parser)
            {
                WriteOptional(parser.Parser);
            }

            public override void VisitProduce<TOutput>(ProduceParser<TInput, TOutput> parser)
            {
                this.Visit(parser.Parser);
            }

            public override void VisitRequired<TOutput>(RequiredParser<TInput, TOutput> parser)
            {
                if (this.showRequired)
                {
                    WriteRequired(parser.Parser);
                }
                else
                {
                    this.Visit(parser.Parser);
                }
            }

            public override void VisitRule<TOutput>(RuleParser<TInput, TOutput> parser)
            {
                WriteSequence(parser.Parsers);
            }

            public override void VisitSequence(SequenceParser<TInput> parser)
            {
                WriteSequence(parser.Parsers);
            }

            public override void VisitZeroOrMore(ZeroOrMoreParser<TInput> parser)
            {
                if (parser.ZeroOrOne)
                {
                    WriteOptional(parser.Parser);
                }
                else
                {
                    WriteZeroOrMore(parser.Parser);
                }
            }
        }
    }
}