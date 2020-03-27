using System;
using System.Collections.Generic;
using System.Linq;
using Kusto.Language.Syntax;

namespace Kusto.Language.Parsing
{
    using Utils;
    using static CharScanners;
    using static Parsers<char>;

    /// <summary>
    /// A factory for creating parsers that parse a simple grammar grammar.
    /// </summary>
    ///
    // grammar grammar
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
    // e : tag      element with tag 
    // e : 'tag'    element with tag
    // [ a ]        optional alternation
    // { a }        zero or more alternations
    // { a }*       zero or more alternations
    // { a }+       one or more alternations
    // { a, e }     list of zero or more alternations (a) separated by element (e)
    // { a, e }*    list of zero or more alternations (a) separated by element (e)
    // { a, e }+    list of one or more alternations (a) separated by element (e)
    // ( a )        grouped alternation
    // <name>       external grammar rule
    //
    public static class GrammarGrammar
    {
        /// <summary>
        /// Creates a parser that parses the grammar grammar.
        /// </summary>
        public static Parser<char, TResult> CreateParser<TResult>(
            Func<string, TResult> getRule,
            Func<OffsetValue<string>, TResult> createTerm,
            Func<TResult, TResult> createOptional,
            Func<TResult, TResult> createRequired,
            Func<TResult, string, TResult> createTagged,
            Func<IReadOnlyList<TResult>, TResult> createSequence,
            Func<IReadOnlyList<TResult>, TResult> createAlternation,
            Func<TResult, TResult> createZeroOrMore,
            Func<TResult, TResult> createOneOrMore,
            Func<TResult, TResult, TResult> createZeroOrMoreSeparated,
            Func<TResult, TResult, TResult> createOneOrMoreSeparated)
            where TResult : class
        {
            // build the parser that parsers the simple grammar grammar
            Parser<char, TResult> elementCore = null;
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
                    Rule(IdentifierAndOffset, text => createTerm(text)),
                    Rule(StringLiteralAndOffset, text => createTerm(new OffsetValue<string>(text.Offset, KustoFacts.GetStringLiteralValue(text.Value)))))
                    .WithTag("<term>");

            var rule =
                Rule(
                    Token("<"), Text(OneOrMore(Not(Token(">")))), Token(">"),
                    (open, name, close) =>
                    {
                        var ruleValue = getRule(name);
                        if (ruleValue == null)
                        {
                            throw new InvalidOperationException($"Unknown rule <{name}>");
                        }
                        else
                        {
                            return ruleValue;
                        }
                    }).WithTag("<rule>");

            var sequence = Produce(
                OneOrMore(element),
                (IReadOnlyList<TResult> list) =>
                    list.Count == 0 ? list[0] : createSequence(list.ToList()))
                .WithTag("<sequence>");

            var alternation =
                SeparatedList(
                    element: sequence.Cast<object>(),
                    separator: Token("|").Cast<object>(),
                    missingElement: () => null,
                    missingSeparator: () => null,
                    endOfList: null,
                    oneOrMore: false,
                    allowTrailingSeparator: false,
                    producer: (IReadOnlyList<object> list) =>
                    {
                        if (list.Count > 1)
                        {
                            return createAlternation(list.OfType<TResult>().ToArray());
                        }
                        else
                        {
                            return (TResult)list[0];
                        }
                    }).WithTag("<alternation>");

            var separator = Rule(Token(","), term, (colon, word) => word);

            var repeatition = Rule(Token("{"), alternation, Optional(separator), Token("}"), Optional(First(Token("+"), Token("*"))),
                    (open, elem, sep, close, kind) =>
                    {
                        var zeroOrMore = kind == null || kind == "*";

                        if (zeroOrMore)
                        {
                            if (sep != null)
                            {
                                return createZeroOrMoreSeparated(elem, sep);
                            }
                            else
                            {
                                return createZeroOrMore(elem);
                            }
                        }
                        else
                        {
                            if (sep != null)
                            {
                                return createOneOrMoreSeparated(elem, sep);
                            }
                            else
                            {
                                return createOneOrMore(elem);
                            }
                        }
                    }).WithTag("<repetition>");

            var grouped = Rule(
                Token("("), alternation, Token(")"),
                (open, parser, close) => parser)
                .WithTag("<grouping>");

            var optional = Rule(
                Token("["), alternation, Token("]"),
                (open, tuplet, close) => createOptional(tuplet))
                .WithTag("<optional>");

            var primaryElement =
                First(
                    term,           // id or 'id'
                    rule,           // <rule>
                    grouped)        // ( ... )
                    .WithTag("<element>");

            var postfixPrimary =
                First(
                    optional,
                    repeatition,

                    ApplyOptional(
                        primaryElement,
                        _left =>
                            First(
                                Rule(_left, Token("!"),
                                    (left, bang) => createRequired(left))
                                    .WithTag("<required>"),

                                // alternative to [] 
                                Rule(_left, Token("?"),
                                    (left, question) => createOptional(left))
                                    .WithTag("<optional>"),
                        
                                // alternative to { }*
                                Rule(_left, Token("*"),
                                    (left, star) => createZeroOrMore(left))
                                    .WithTag("<zero-or-more>"),

                                // alternative to { }+
                                Rule(_left, Token("+"),
                                    (left, plus) => createOneOrMore(left))
                                    .WithTag("<one-or-more>")
                                    )));

            // either id=elem or elem:id
            var taggedPrimary =
                First(
                    Rule(Identifier, Token("="), postfixPrimary,
                        (id, eq, elem) => createTagged(elem, id)),

                    Rule(StringLiteral, Token("="), postfixPrimary,
                        (str, eq, elem) => createTagged(elem, KustoFacts.GetStringLiteralValue(str))),

                    ApplyOptional(
                        postfixPrimary,
                        _left =>
                            First(
                                Rule(_left, Token(":"), Identifier,
                                    (left, colon, id) => createTagged(left, id)),
                                Rule(_left, Token(":"), StringLiteral,
                                    (left, colon, str) => createTagged(left, KustoFacts.GetStringLiteralValue(str)))))
                    .WithTag("<tagged>"));

            elementCore = taggedPrimary;

            return sequence;
        }
    }
}
