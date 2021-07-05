using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kusto.Language.Syntax;

namespace Kusto.Language.Parsing
{
    using Utils;
    using static CharScanners;
    using static Parsers<char>;

    // The grammar grammar:
    //
    // alternation:
    // s1 | s2      one or more sequences one of which must occur
    //
    // sequence:
    // e1 e2        one or more elements that all must occur
    //
    // element:
    // abc          term
    // 'abc'        quoted term
    // e !          required element
    // e ?          optional element
    // e *          zero or more of the same element
    // e +          one or more of the same element
    // tag = e      element with tag
    // 'tag' = e    element with tag
    // [ a ]        optional alternation
    // { a }        zero or more alternations
    // { a }*       zero or more alternations
    // { a }+       one or more alternations
    // { a, e }     list of zero or more alternations (a) separated by element (e)
    // { a, e }*    list of zero or more alternations (a) separated by element (e)
    // { a, e }+    list of one or more alternations (a) separated by element (e)
    // ( a )        grouped alternation
    // <name>       external grammar rule reference

    /// <summary>
    /// A parser that parsers a grammar for grammars.
    /// </summary>
    public static class GrammarParser
    {
        /// <summary>
        /// Try to parse the grammar grammar.
        /// Returns true if parse succeeds.
        /// </summary>
        /// <param name="text">The grammar grammar.</param>
        /// <param name="grammar">The resulting grammar tree.</param>
        /// <param name="length">The number of characters consumed by parsing.</param>
        /// <returns>True if the parse succeeds.</returns>
        public static bool TryParse(string text, out Grammar grammar, out int length)
        {
            var parser = GetParser();
            var result = parser.Parse(text);
            grammar = result.Value;
            length = result.Length;
            return result.Succeeded;
        }

        /// <summary>
        /// Try to parse the grammar grammar.
        /// Returns true if parse succeeds.
        /// </summary>
        /// <param name="text">The grammar grammar.</param>
        /// <param name="grammar">The resulting grammar tree.</param>
        /// <returns>True if the parse succeeds.</returns>
        public static bool TryParse(string text, out Grammar grammar) =>
            TryParse(text, out grammar, out _);

        /// <summary>
        /// Parses the grammar grammar and returns the resulting <see cref="Grammar"/>.
        /// </summary>
        /// <param name="text">The grammar grammar.</param>
        /// <returns></returns>
        public static Grammar Parse(string text)
        {
            TryParse(text, out var grammar);
            return grammar;
        }

        /// <summary>
        /// Creates a parser that parses the grammar grammar.
        /// </summary>
        private static Parser<char, Grammar> GetParser()
        {
            if (_grammarParser == null)
            {
                _grammarParser = CreateParser();
            }

            return _grammarParser;
        }

        private static Parser<char, Grammar> _grammarParser;

        /// <summary>
        /// Creates a parser that parses the grammar grammar and builds custom results.
        /// </summary>
        private static Parser<char, Grammar> CreateParser()
        {
            // build the parser that parsers the simple grammar grammar
            Parser<char, Grammar> elementCore = null;
            var element = Forward(() => elementCore);

            var WhitespaceCount =
                Count(ZeroOrMore(Whitespace));

            Parser<char, string> TokenText(string text) =>
                Convert(Chars(text), text);

            var IdentifierScan =
                And(Or(Letter, Char('_')), ZeroOrMore(Or(Letter, Digit, Char('_'), Char('-'))));

            var IdentifierText =
                Text(IdentifierScan);

            var IdentifierTextAndOffset =
                TextAndOffset(IdentifierScan);

            var StringLiteralScan =
                And(Char('\''), ZeroOrMore(Not(Char('\''))), Char('\''));

            var StringLiteralText =
                Text(StringLiteralScan);

            var StringLiteralTextAndOffset =
                TextAndOffset(StringLiteralScan);

            Parser<char, string> Token(string text) =>
                Rule(WhitespaceCount, TokenText(text), (ws, tx) => tx);

            var Identifier =
                Rule(WhitespaceCount, IdentifierText, (ws, tx) => tx);

            var IdentifierAndOffset =
                Rule(WhitespaceCount, IdentifierTextAndOffset, (ws, tx) => tx);

            var StringLiteral =
                Rule(WhitespaceCount, StringLiteralText, (ws, tx) => tx);

            var StringLiteralAndOffset =
                Rule(WhitespaceCount, StringLiteralTextAndOffset, (ws, tx) => tx);

            var term =
                First(
                    Rule(Identifier, text => (Grammar)new TokenGrammar(text)),
                    Rule(StringLiteral, text => (Grammar)new TokenGrammar(KustoFacts.GetStringLiteralValue(text))))
                    .WithTag("<term>");

            var rule =
                Rule(
                    Token("<"), Text(OneOrMore(Not(Token(">")))), Token(">"),
                    (open, name, close) => (Grammar)new RuleGrammar(name))
                .WithTag("<rule>");

            var sequence = Produce(
                OneOrMore(element),
                (IReadOnlyList<Grammar> list) =>
                    list.Count == 1 ? list[0] : new SequenceGrammar(list.ToList()))
                .WithTag("<sequence>");

            var alternation =
                List(
                    elementParser: sequence,
                    separatorParser: Token("|"),
                    missingElement: null,
                    missingSeparator: null,
                    endOfList: null,
                    oneOrMore: true,
                    allowTrailingSeparator: false,
                    producer: list =>
                    {
                        if (list.Count == 1)
                        {
                            return list[0].Element;
                        }
                        else
                        {
                            return new AlternationGrammar(list.Select(eas => eas.Element).OfType<Grammar>().ToArray());
                        }
                    }).WithTag("<alternation>");

            var separator = Rule(Token(","), term, (colon, word) => word);

            var repeatition = Rule(
                    Token("{"),
                    alternation,
                    Optional(separator),
                    Token("}"),
                    Optional(First(Token("+"), Token("*"))),
                (open, elem, sep, close, kind) =>
                {
                    var zeroOrMore = kind == null || kind == "*";

                    if (zeroOrMore)
                    {
                        return (Grammar)new ZeroOrMoreGrammar(elem, sep);
                    }
                    else
                    {
                        return (Grammar)new OneOrMoreGrammar(elem, sep);
                    }
                }).WithTag("<repetition>");

            var grouped = Rule(
                Token("("), alternation, Token(")"),
                (open, grammar, close) => grammar)
                .WithTag("<grouping>");

            var optional = Rule(
                Token("["), alternation, Token("]"),
                (open, optioned, close) => (Grammar)new OptionalGrammar(optioned))
                .WithTag("<optional>");

            var primaryElement =
                First(
                    term,           // id or 'id'
                    rule,           // <rule>
                    grouped)        // ( ... )
                    .WithTag("<element>");

            // allow for some postfix abbreviations here
            var postfixPrimary =
                First(
                    optional,       // [a]
                    repeatition,    // {a}

                    ApplyOptional(
                        primaryElement,
                        _left =>
                            First(
                                Rule(_left, Token("!"),
                                    (left, bang) => (Grammar)new RequiredGrammar(left))
                                    .WithTag("<required>"),

                                // alternative to [] 
                                Rule(_left, Token("?"),
                                    (left, question) => (Grammar)new OptionalGrammar(left))
                                    .WithTag("<optional>"),

                                // alternative to { }*
                                Rule(_left, Token("*"),
                                    (left, star) => (Grammar)new ZeroOrMoreGrammar(left))
                                    .WithTag("<zero-or-more>"),

                                // alternative to { }+
                                Rule(_left, Token("+"),
                                    (left, plus) => (Grammar)new OneOrMoreGrammar(left))
                                    .WithTag("<one-or-more>")
                                    )));

            // allow for tag=elem
            var taggedPrimary =
                First(
                    If(And(Identifier, Token("=")),
                        Rule(Identifier, Token("="), postfixPrimary,
                            (id, eq, elem) => (Grammar)new TaggedGrammar(id, elem))),

                    If(And(StringLiteral, Token("=")),
                        Rule(StringLiteral, Token("="), postfixPrimary,
                            (str, eq, elem) => (Grammar)new TaggedGrammar(KustoFacts.GetStringLiteralValue(str), elem))),

                    postfixPrimary
                    );

            elementCore = taggedPrimary;

            return alternation;
        }
    }
}