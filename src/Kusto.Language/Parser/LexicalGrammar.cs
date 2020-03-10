using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kusto.Language.Syntax;

namespace Kusto.Language.Parsing
{
    using static CharScanners;
    using static Parsers<char>;

    /// <summary>
    /// Parsers for the Kusto lexical grammar (tokens)
    /// </summary>
    public class LexicalGrammar
    {
        /// <summary>
        /// Gets all the tokens for the text (in lexical order).
        /// </summary>
        public static LexicalToken[] GetTokens(string text, bool alwaysProduceEndToken = false)
        {
            var list = new List<object>();
            var parser = alwaysProduceEndToken ? TokensAndEnd : Tokens;
            parser.Parse(text, list);
            return list.Select(e => (LexicalToken)e).ToArray();
        }

        /// <summary>
        /// Gets the first token of the text.
        /// </summary>
        public static LexicalToken GetFirstToken(string text)
        {
            return TokenOrEnd.Parse(text).Value;
        }

        public static readonly Parser<char> Identifier = Or(
                And(Or(Char('$'), Char('_'), Letter), ZeroOrMore(Or(Char('_'), Letter, Digit))),
                And(OneOrMore(Digit), Or(Char('_'), Letter), ZeroOrMore(Or(Char('_'), Letter, Digit))));

        public static readonly Parser<char> ClientParameter =
            And(Char('{'), Identifier, Char('}'));

        private static readonly Parser<char> SingleLineComment = 
            And(Chars("//"), ZeroOrMore(Not(LineBreak)));

        private static readonly Parser<char> Trivia = 
            ZeroOrMore(Or(OneOrMore(Whitespace), SingleLineComment));

        private static readonly Parser<char> NonHexIntegerNumber = 
            OneOrMore(Digit);

        private static readonly Parser<char> HexIntegerPrefix = Or(
            Chars("0x"), 
            Chars("0X"));

        private static readonly Parser<char> HexIntegerNumber = 
            And(HexIntegerPrefix, OneOrMore(HexDigit));

        private static readonly Parser<char> LongLiteral = Or(
            NonHexIntegerNumber, 
            HexIntegerNumber);

        private static readonly Parser<char> PlusOrMinus = Or(
            Char('+'), 
            Char('-'));

        private static readonly Parser<char> Exponent = 
            And(Or(Char('e'), Char('E')), Optional(PlusOrMinus), OneOrMore(Digit));

        private static readonly Parser<char> RealLiteral = Or(
            And(NonHexIntegerNumber, Char('.'), Fails(And(Char('.'), Fails(Char('.')))), ZeroOrMore(Digit), Optional(Exponent)),
            And(NonHexIntegerNumber, Exponent)
            );

        private static readonly Parser<char> Goo = 
            And(Char('('), ZeroOrMore(Not(Char(')'))), Optional(Char(')')));

        private static readonly Parser<char> PrefixedLongLiteral = Or(
            And(Chars("long"), Goo));

        private static readonly Parser<char> PrefixedIntLiteral = Or(
            And(Chars("int"), Goo));

        private static readonly Parser<char> PrefixedRealLiteral = Or(
            And(Chars("real"), Goo),
            And(Chars("double"), Goo));

        private static readonly Parser<char> PrefixedDecimalLiteral =
            And(Chars("decimal"), Goo);

        private static readonly Parser<char> PrefixedBooleanLiteral =
            And(Chars("bool"), Goo);

        private static readonly Parser<char> BooleanLiteral =
            Or(Chars("true"), Chars("false"), Chars("TRUE"), Chars("FALSE"), Chars("True"), Chars("False"));

        private static readonly Parser<char> PrefixedDateTimeLiteral =
            And(Chars("datetime"), Goo);

        private static readonly Parser<char> PrefixedTimespanLiteral =
            And(Or(Chars("time"), Chars("timespan")), Goo);

        private static readonly Parser<char> TimespanLiteral =
            And(NonHexIntegerNumber, Or(
                And(Char('m'), Optional(Or(And(Chars("in"), Optional(Chars("ute"))), Chars("inutes")))),
                And(Char('s'), Optional(Or(And(Chars("ec"), Optional(Chars("ond"))), Chars("econds")))),
                And(Char('d'), Optional(And(Chars("ay"), Optional(Char('s'))))),
                And(Char('h'), Optional(And(Chars("our"), Optional(Char('s'))))),
                And(Chars("hr"), Optional(Char('s'))),
                Chars("ms"),
                And(Chars("milli"), Optional(And(Chars("s"), Optional(And(Chars("ec"), Optional(And(Chars("ond"), Optional(Char('s'))))))))),
                And(Chars("micro"), Optional(And(Chars("s"), Optional(And(Chars("ec"), Optional(And(Chars("ond"), Optional(Char('s'))))))))),
                And(Chars("nano"), Optional(And(Chars("s"), Optional(And(Chars("ec"), Optional(And(Chars("ond"), Optional(Char('s'))))))))),
                And(Chars("tick"), Optional(Char('s')))
                ));

        // guid literals
        private static readonly Parser<char> PrefixedGuidLiteral =
           And(Chars("guid"), Goo);

        // string literals
        private static readonly Parser<char> TwoHexDigits =
            And(HexDigit, HexDigit);

        private static readonly Parser<char> FourHexDigits =
            And(HexDigit, HexDigit, HexDigit, HexDigit);

        private static readonly Parser<char> EightHexDigits =
            And(FourHexDigits, FourHexDigits);

        private static readonly Parser<char> ZeroToThree =
            Or(Char('0'), Char('1'), Char('2'), Char('3'));

        private static readonly Parser<char> ZeroToSeven =
            Or(ZeroToThree, Char('4'), Char('5'), Char('6'), Char('7'));

        private static readonly Parser<char> Escape = And(Char('\\'), Or(
                Char('\''),
                Char('\"'),
                Char('\\'),
                Char('a'),
                Char('b'),
                Char('f'),
                Char('n'),
                Char('r'),
                Char('t'),
                And(Char('u'), FourHexDigits),
                And(Char('U'), EightHexDigits),
                And(Char('x'), TwoHexDigits),
                Char('v'),
                And(ZeroToThree, ZeroToSeven, ZeroToSeven),
                And(ZeroToSeven, Optional(ZeroToSeven))
                ));

        private static readonly Parser<char> HiddenPrefix = Or(Char('h'), Char('H'));

        private static readonly Parser<char> SingleQuoteStringLiteral =
            And(Optional(HiddenPrefix), Or(
                And(Char('\''), ZeroOrMore(Or(Escape, Not(Or(Char('\\'), Char('\''), Char('\r'), Char('\n'))))), Optional(Char('\''))),
                And(Chars("@'"), ZeroOrMore(Or(Chars("''"), Not(Or(Char('\''), Char('\r'), Char('\n'))))), Optional(Char('\'')))
                ));

        private static readonly Parser<char> DoubleQuoteStringLiteral =
            And(Optional(HiddenPrefix), Or(
                And(Char('"'), ZeroOrMore(Or(Escape, Not(Or(Char('\\'), Char('"'), Char('\r'), Char('\n'))))), Optional(Char('"'))),
                And(Chars("@\""), ZeroOrMore(Or(Chars("\"\""), Not(Or(Char('"'), Char('\r'), Char('\n'))))), Optional(Char('"')))
                ));

        private static readonly Parser<char> Directive =
            And(Char('#'), ZeroOrMore(Not(LineBreak)));

        private struct FixedTextInfo
        {
            public readonly SyntaxKind Kind;
            public readonly string Text;

            public FixedTextInfo(SyntaxKind kind, string text)
            {
                this.Kind = kind;
                this.Text = text;
            }
        }

        private static RightParser<char, LexicalToken> EndCheckedToken(LeftValue<OffsetValue<string>> left, Parser<char> scanner, SyntaxKind kind, char expectedEndChar) =>
            Rule(left, Text(scanner),
                (trivia, text) => 
                {
                    // validate the expected last character is correct
                    var dx = text[text.Length -1] == expectedEndChar 
                        ? null 
                        : new Diagnostic[] { DiagnosticFacts.GetMissingCharacter(expectedEndChar) };

                    return new LexicalToken(trivia.Offset, kind, trivia.Value, text, dx);
                });

        private static RightParser<char, LexicalToken> GooCheckedToken(LeftValue<OffsetValue<string>> left, Parser<char> gooScanner, SyntaxKind kind) =>
            EndCheckedToken(left, gooScanner, kind, ')');

        private static readonly Parser<char, FixedTextInfo> KeywordOrPunctuationInfo =
            Map(SyntaxFacts.GetKindsWithFixedText().Select(k => new KeyValuePair<IEnumerable<char>, FixedTextInfo>(k.GetText(), new FixedTextInfo(k, k.GetText()))));

        /// <summary>
        /// Any token (except EndOfText token)
        /// </summary>
        public static readonly Parser<char, LexicalToken> Token =
            Apply(
                TextAndOffset(Trivia),
                _trivia => Best(
                    Rule(_trivia, KeywordOrPunctuationInfo,
                        (trivia, info) => new LexicalToken(trivia.Offset, info.Kind, trivia.Value, info.Text)),
                    EndCheckedToken(_trivia, SingleQuoteStringLiteral, SyntaxKind.StringLiteralToken, '\''),
                    EndCheckedToken(_trivia, DoubleQuoteStringLiteral, SyntaxKind.StringLiteralToken, '"'),
                    GooCheckedToken(_trivia, PrefixedBooleanLiteral, SyntaxKind.BooleanLiteralToken),
                    GooCheckedToken(_trivia, PrefixedGuidLiteral, SyntaxKind.GuidLiteralToken),
                    GooCheckedToken(_trivia, PrefixedDateTimeLiteral, SyntaxKind.DateTimeLiteralToken),
                    GooCheckedToken(_trivia, PrefixedTimespanLiteral, SyntaxKind.TimespanLiteralToken),
                    GooCheckedToken(_trivia, PrefixedLongLiteral, SyntaxKind.LongLiteralToken),
                    GooCheckedToken(_trivia, PrefixedIntLiteral, SyntaxKind.IntLiteralToken),
                    GooCheckedToken(_trivia, PrefixedRealLiteral, SyntaxKind.RealLiteralToken),
                    GooCheckedToken(_trivia, PrefixedDecimalLiteral, SyntaxKind.DecimalLiteralToken),
                    Rule(_trivia, Text(BooleanLiteral),
                        (trivia, text) => new LexicalToken(trivia.Offset, SyntaxKind.BooleanLiteralToken, trivia.Value, text)),
                    Rule(_trivia, Text(LongLiteral),
                        (trivia, text) => new LexicalToken(trivia.Offset, SyntaxKind.LongLiteralToken, trivia.Value, text)),
                    Rule(_trivia, Text(RealLiteral),
                        (trivia, text) => new LexicalToken(trivia.Offset, SyntaxKind.RealLiteralToken, trivia.Value, text)),
                    Rule(_trivia, Text(TimespanLiteral),
                        (trivia, text) => new LexicalToken(trivia.Offset, SyntaxKind.TimespanLiteralToken, trivia.Value, text)),
                    Rule(_trivia, Text(Identifier),
                        (trivia, text) => new LexicalToken(trivia.Offset, SyntaxKind.IdentifierToken, trivia.Value, text)),
                    Rule(_trivia, Text(Directive),
                        (trivia, text) => new LexicalToken(trivia.Offset, SyntaxKind.DirectiveToken, trivia.Value, text)),
                    Rule(_trivia, Text(Any),
                        (trivia, text) => new LexicalToken(trivia.Offset, SyntaxKind.BadToken, trivia.Value, text, new[] { DiagnosticFacts.GetUnexpectedCharacter(text) }))
                    ));

        /// <summary>
        /// The end of text token (holds onto trailing trivia)
        /// </summary>
        public static readonly Parser<char, LexicalToken> EndOfText =
            Apply(
                TextAndOffset(Trivia),
                _trivia =>
                    If(Fails(Any),
                        Rule(_trivia, (trivia) => new LexicalToken(trivia.Offset, SyntaxKind.EndOfTextToken, trivia.Value, null))));

        /// <summary>
        /// Any token including EndOfText token.
        /// </summary>
        public static readonly Parser<char, LexicalToken> TokenOrEnd =
            First(Token, EndOfText);

        /// <summary>
        /// All tokens including a possible EndOfText token if there is trailing trivia.
        /// </summary>
        public static readonly Parser<char> Tokens =
            And(ZeroOrMore(Token), ZeroOrOne(If(Any, EndOfText)));

        /// <summary>
        /// All tokens including an EndOfText token even if there is no trailing trivia.
        /// </summary>
        public static readonly Parser<char> TokensAndEnd =
            And(ZeroOrMore(Token), EndOfText);
    }
}