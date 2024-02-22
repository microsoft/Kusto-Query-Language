using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace Kusto.Language.Parsing
{
    using Syntax;
    using Editor;
    using Utils;

    using static Parsers<LexicalToken>;

    /// <summary>
    /// Parser and Scanners for working with Kusto syntax.
    /// </summary>
    public static class SyntaxParsers
    {
        /// <summary>
        /// Creates a missing <see cref="SyntaxToken"/> for a token that was expected to have the specified <see cref="SyntaxKind"/>.
        /// </summary>
        public static SyntaxToken CreateMissingToken(SyntaxKind kind, Diagnostic diagnostic = null)
        {
            var dx = diagnostic ??
                (kind == SyntaxKind.IdentifierToken ? DiagnosticFacts.GetMissingName() : DiagnosticFacts.GetTokenExpected(kind));
            return SyntaxToken.Missing("", kind, new[] { dx });
        }

        /// <summary>
        /// Creates a missing <see cref="SyntaxToken"/> for a token that was expected to have the specified <see cref="SyntaxKind"/>.
        /// </summary>
        public static SyntaxToken CreateMissingToken(IReadOnlyList<SyntaxKind> kinds)
        {
            return SyntaxToken.Missing("", kinds[0], new[] { DiagnosticFacts.GetTokenExpected(kinds) });
        }

        /// <summary>
        /// Creates a missing <see cref="SyntaxToken"/> for a token that was expected to have the specified text.
        /// </summary>
        public static SyntaxToken CreateMissingToken(string text, Diagnostic diagnostic = null)
        {
            if (!SyntaxFacts.TryGetKind(text, out var kind))
            {
                kind = SyntaxKind.IdentifierToken;
            }

            return SyntaxToken.Missing("", kind, new[] { diagnostic ?? DiagnosticFacts.GetTokenExpected(new[] { text }) });
        }

        /// <summary>
        /// Creates a missing <see cref="SyntaxToken"/> for a token that was expected to have one of the specified texts.
        /// </summary>
        public static SyntaxToken CreateMissingToken(IReadOnlyList<string> texts)
        {
            return SyntaxToken.Missing("", SyntaxKind.IdentifierToken, new[] { DiagnosticFacts.GetTokenExpected(texts) });
        }

        /// <summary>
        /// A parser that consumes the next <see cref="LexicalToken"/>, producing the corresponding <see cref="SyntaxToken"/>.
        /// </summary>
        public static readonly Parser<LexicalToken, SyntaxToken> AnyToken =
            Match(t => true, t => SyntaxToken.From(t)).WithTag("<any>");

        /// <summary>
        /// A parser that consumes the next <see cref="LexicalToken"/> as long as it does not have the kind <see cref="SyntaxKind.EndOfTextToken"/>, producing the corresponding <see cref="SyntaxToken"/>.
        /// </summary>
        public static readonly Parser<LexicalToken, SyntaxToken> AnyTokenButEnd =
            Match(t => t.Kind != SyntaxKind.EndOfTextToken, t => SyntaxToken.From(t)).WithTag("<any!end>");

        /// <summary>
        /// A parser that consumes only the end of text token.
        /// </summary>
        public static readonly Parser<LexicalToken> EndOfText =
            Match(t => t.Kind == SyntaxKind.EndOfTextToken);

        /// <summary>
        /// Gets the default tag to assign a token parser, based on the token's text.
        /// </summary>
        private static string GetDefaultTag(string text)
        {
            if (SyntaxFacts.TryGetKind(text, out var kind))
            {
                return GetDefaultTag(kind);
            }
            else
            {
                return text;
            }
        }

        /// <summary>
        /// Gets the default tag to assign a token parser, based on the token's kind.
        /// </summary>
        private static string GetDefaultTag(SyntaxKind kind)
        {
            return kind.GetCategory() == SyntaxCategory.Punctuation
                    ? $"'{kind.GetText()}'"
                    : kind.GetText() ?? kind.ToString().ToLower();
        }

        /// <summary>
        /// A parser that consumes the next next <see cref="LexicalToken"/> if it has the specified <see cref="SyntaxKind"/>, producing a corresponding <see cref="SyntaxToken"/>.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> Token(SyntaxKind kind, CompletionKind? ckind = null, CompletionPriority priority = CompletionPriority.Normal, string ctext = null)
        {
            var item = CreateCompletionItem(kind, ckind ?? GetCompletionKind(kind), priority, ctext);
            return Token(kind, item);
        }

        /// <summary>
        /// A parser that consumes the next next <see cref="LexicalToken"/> if it has the specified <see cref="SyntaxKind"/>, producing a corresponding <see cref="SyntaxToken"/>.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> Token(SyntaxKind kind, params CompletionItem[] items)
        {
            var rule = Match(t => t.Kind == kind, lt => SyntaxToken.From(lt)).WithTag(GetDefaultTag(kind));

            if (items != null && items.Length == 0)
            {
                items = new[] { CreateCompletionItem(kind, GetCompletionKind(kind), CompletionPriority.Normal, null) };
            }

            if (items != null)
            {
                rule = rule.WithAnnotations(items);
            }

            return rule;
        }

        /// <summary>
        /// Creates a new version of the parser with the ComplationItem annotation set.
        /// </summary>
        public static Parser<LexicalToken, TElement> WithCompletion<TElement>(this Parser<LexicalToken, TElement> parser, params CompletionItem[] items)
        {
            return parser.WithAnnotations(parser.Annotations.Where(a => !(a is CompletionItem)).Concat(items));
        }

        /// <summary>
        /// Creates a new version of the parser with the CompletionHint annotation set.
        /// </summary>
        public static Parser<LexicalToken, TElement> WithCompletionHint<TElement>(this Parser<LexicalToken, TElement> parser, CompletionHint hint)
        {
            return parser.WithAnnotations(parser.Annotations.Where(a => !(a is CompletionHint)).Concat(new[] { (object)hint }));
        }

        /// <summary>
        /// A parser that consumes the next next <see cref="LexicalToken"/> if it has one of the specified <see cref="SyntaxKind"/>s, producing a corresponding <see cref="SyntaxToken"/>.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> Token(IReadOnlyList<SyntaxKind> kinds, CompletionKind? defaultKind = null, CompletionPriority priority = CompletionPriority.Normal)
        {
            Ensure.ArgumentNotNull(kinds, nameof(kinds));
            var set = new HashSet<SyntaxKind>(kinds);

            var rule = Match(t => set.Contains(t.Kind), lt => SyntaxToken.From(lt)).WithTag(string.Join(" | ", kinds.Select(k => GetDefaultTag(k))));

            var items = GetCompletionItems(set, defaultKind, priority).ToList();
            if (items.Count > 0)
            {
                rule = rule.WithAnnotations(items);
            }

            return rule;
        }

        /// <summary>
        /// Matches one or more lexical tokens to the corresponding text.
        /// </summary>
        public static int MatchesText(Source<LexicalToken> source, int start, string text)
        {
            // consume all lexical tokens with combined text that matches the specified text
            int textOffset = 0;
            int i = 0;

            for (; !source.IsEnd(start + i); i++)
            {
                var token = source.Peek(start + i);

                // only first token can have trivia
                if (i > 0 && token.Trivia.Length > 0)
                    break;

                // token has more text than remaining
                if (token.Text.Length > text.Length - textOffset)
                    break;

                // text parts must match exactly
                if (string.Compare(token.Text, 0, text, textOffset, token.Text.Length) != 0)
                    break;

                textOffset += token.Text.Length;

                if (textOffset == text.Length)
                    return (i + 1);
            }

            return -(i + 1);
        }

        /// <summary>
        /// Create a <see cref="SyntaxToken"/> from one or more adjacent <see cref="LexicalToken"/>.
        /// </summary>
        public static SyntaxToken ProduceSyntaxToken(Source<LexicalToken> source, int start, int length, SyntaxKind? asKind = null)
        {
            var token = source.Peek(start);
            if (token != null)
            {
                string text = token.Text;

                for (int i = 1; i < length; i++)
                {
                    token = source.Peek(start + i);
                    if (token == null || token.Trivia.Length > 0)
                        return null;
                    text += token.Text;
                }

                return ProduceSyntaxToken(source, start, length, text, asKind);
            }

            return null;
        }

        /// <summary>
        /// Create a <see cref="SyntaxToken"/> from one or more adjacent <see cref="LexicalToken"/>.
        /// </summary>
        public static SyntaxToken ProduceSyntaxToken(Source<LexicalToken> source, int start, int length, string text, SyntaxKind? asKind = null)
        {
            var firstToken = source.Peek(start);

            if (length == 1 && (asKind == null || asKind.Value == firstToken.Kind))
            {
                return SyntaxToken.From(firstToken);
            }
            else if (asKind is SyntaxKind kind)
            {
                // assigning kinds only works for token categories that hold onto their text
                switch (kind.GetCategory())
                {
                    case SyntaxCategory.Identifier:
                        return SyntaxToken.Identifier(firstToken.Trivia, text);
                    case SyntaxCategory.Other:
                        return SyntaxToken.Other(firstToken.Trivia, text, kind);
                    default:
                        throw new InvalidOperationException($"Cannot produce syntax token for kind: {kind}");
                }
            }
            else
            {
                return SyntaxToken.Identifier(firstToken.Trivia, text);
            }
        }

        /// <summary>
        /// Gets the text of a series of tokens.
        /// </summary>
        public static string GetCombinedTokenText(Source<LexicalToken> source, int start, int length, bool includeInnerTrivia = true)
        {
            if (length == 1)
            {
                return source.Peek(start).Text;
            }

            var builder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                var token = source.Peek(start + i);
                if (i > 0 && includeInnerTrivia)
                    builder.Append(token.Trivia);
                builder.Append(token.Text);
            }

            return builder.ToString();
        }

        /// <summary>
        /// A parser that consumes the next <see cref="LexicalToken"/> (or series of adjacent tokens) that combined has the specified text, producing a single <see cref="SyntaxToken"/>.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> MatchText(string text, SyntaxKind? asKind = null) =>
            Match(
                (source, start) => MatchesText(source, start, text), 
                (source, start, length) => ProduceSyntaxToken(source, start, length, text, asKind));

        /// <summary>
        /// A parser that consumes the next <see cref="LexicalToken"/> (or series of adjacent tokens) if it has the specified text, producing a single <see cref="SyntaxToken"/>.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> Token(string text, SyntaxKind? asKind = null, CompletionKind? ckind = null, CompletionPriority priority = CompletionPriority.Normal, string ctext = null)
        {
            var rule = MatchText(text, asKind).WithTag(GetDefaultTag(text));

            var item = CreateCompletionItem(text, ckind ?? GetCompletionKind(text), priority, ctext);
            return rule.WithCompletion(item);
        }

        /// <summary>
        /// A parser that consumes the next <see cref="LexicalToken"/> (or series of adjacent tokens) if it has the specified text, producing a single <see cref="SyntaxToken"/>.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> Token(string text, CompletionKind ckind, CompletionPriority priority = CompletionPriority.Normal, string ctext = null)
        {
            var rule = MatchText(text).WithTag(GetDefaultTag(text));

            var item = CreateCompletionItem(text, ckind, priority, ctext);
            return rule.WithCompletion(item);
        }

        /// <summary>
        /// A parser that consumes the next <see cref="LexicalToken"/> (or series of adjacent tokens) if it has one of the specified texts, producing a single <see cref="SyntaxToken"/>.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> Token(IReadOnlyList<string> texts, CompletionKind? defaultKind = null, CompletionPriority priority = CompletionPriority.Normal)
        {
            Ensure.ArgumentNotNull(texts, nameof(texts));

            var rule = First(texts.Select(t => MatchText(t)).ToArray())
                    .WithTag(string.Join(" | ", texts.Select(t => GetDefaultTag(t))));

            var items = GetCompletionItems(texts, defaultKind, priority).ToList();
            if (items.Count > 0)
            {
                rule = rule.WithAnnotations(items);
            }

            return rule;
        }

        /// <summary>
        /// A parser that consumes the next <see cref="LexicalToken"/> (or series of adjacent tokens) if it has the specified text, producing a single <see cref="SyntaxToken"/>.
        /// It does not show up in intellisense completion lists.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> HiddenToken(string text, SyntaxKind? asKind = null)
        {
            return MatchText(text, asKind).WithTag(GetDefaultTag(text));
        }

        /// <summary>
        /// A parser that consumes the next next <see cref="LexicalToken"/> if it has the specified <see cref="SyntaxKind"/>, producing a corresponding <see cref="SyntaxToken"/>.
        /// It does not show up in intellisense completion lists.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> HiddenToken(SyntaxKind tokenKind)
        {
            return Match(t => t.Kind == tokenKind, lt => SyntaxToken.From(lt)).WithTag(GetDefaultTag(tokenKind));
        }

        /// <summary>
        /// A parser that consumes the next <see cref="LexicalToken"/> (or series of adjacent tokens) if it has one of the specified texts, producing a single <see cref="SyntaxToken"/>.
        /// It does not show up in intellisense completion lists.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> HiddenToken(IReadOnlyList<string> texts)
        {
            Ensure.ArgumentNotNull(texts, nameof(texts));
            var set = new HashSet<string>(texts);

            var rule = Match(t => set.Contains(t.Text), lt => SyntaxToken.From(lt)).WithTag(string.Join(" | ", texts.Select(t => GetDefaultTag(t))));

            return rule;
        }

        /// <summary>
        /// A parser that consumes the next <see cref="LexicalToken"/> if it has the specified <see cref="SyntaxKind"/>, producing a corresponding <see cref="SyntaxToken"/> or an equivalent missing token otherwise.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> RequiredToken(SyntaxKind kind, CompletionKind? ckind = null, CompletionPriority priority = CompletionPriority.Normal, string ctext = null) =>
            Required(Token(kind, ckind, priority, ctext), () => CreateMissingToken(kind));

        /// <summary>
        /// A parser that consumes the next <see cref="LexicalToken"/> if it has the specified <see cref="SyntaxKind"/>, producing a corresponding <see cref="SyntaxToken"/> or an equivalent missing token otherwise.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> RequiredToken(SyntaxKind kind, CompletionItem item) =>
            Required(Token(kind, item), () => CreateMissingToken(kind));

        /// <summary>
        /// A parser that consumes the next <see cref="LexicalToken"/> if it has one of the specified <see cref="SyntaxKind"/>, producing a corresponding <see cref="SyntaxToken"/> or an equivalent missing token otherwise.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> RequiredToken(IReadOnlyList<SyntaxKind> kinds, CompletionKind? ckind = null, CompletionPriority priority = CompletionPriority.Normal) =>
            Required(Token(kinds, ckind, priority), () => CreateMissingToken(kinds));

        /// <summary>
        /// A parser that consumes the next <see cref="LexicalToken"/> (or series of adjacent tokens) if it has the specified text, producing a corresponding <see cref="SyntaxToken"/> or an equivalent missing token otherwise.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> RequiredToken(string text, SyntaxKind? asKind = null, CompletionKind? ckind = null, CompletionPriority priority = CompletionPriority.Normal, string ctext = null) =>
            Required(Token(text, asKind, ckind, priority, ctext), () => CreateMissingToken(text));

        /// <summary>
        /// A parser that consumes the next <see cref="LexicalToken"/> (or series of adjacent tokens) if it has one of the specified texts, producing a corresponding <see cref="SyntaxToken"/> or an equivalent missing token otherwise.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> RequiredToken(IReadOnlyList<string> texts, CompletionKind? ckind = null, CompletionPriority priority = CompletionPriority.Normal) =>
            Required(Token(texts, ckind, priority), () => CreateMissingToken(texts));

        /// <summary>
        /// Gets the default <see cref="CompletionItem"/> for a token with the specified <see cref="SyntaxKind"/>.
        /// </summary>
        public static CompletionItem CreateCompletionItem(SyntaxKind kind, CompletionKind ckind, CompletionPriority priority, string ctext = null)
        {
            var text = SyntaxFacts.GetText(kind);
            return CreateCompletionItem(text, ckind, priority, ctext);
        }

        /// <summary>
        /// Gets the default <see cref="CompletionItem"/> for a token with the specified text.
        /// </summary>
        public static CompletionItem CreateCompletionItem(string text, CompletionKind ckind, CompletionPriority priority, string ctext = null, string matchText = null)
        {
            // no text is not going to work
            if (string.IsNullOrWhiteSpace(text))
                return null;

            // hide any syntax that starts with _ from completion
            if (text.StartsWith("_", StringComparison.Ordinal))
                return null;

            var item = new CompletionItem(ckind, text, matchText: matchText, priority: priority);

            if (ctext != null)
            {
                item = item.WithApplyTexts(ctext);
            }

            return item;
        }

        /// <summary>
        /// Gets the <see cref="CompletionKind"/> for the token text.
        /// </summary>
        public static CompletionKind GetCompletionKind(string text, CompletionKind? defaultKind = null)
        {
            return SyntaxFacts.TryGetKind(text, out var kind)
                ? GetCompletionKind(kind, defaultKind)
                : defaultKind ?? CompletionKind.Syntax;
        }

        /// <summary>
        /// Gets the default <see cref="CompletionKind"/> for the token kind.
        /// </summary>
        public static CompletionKind GetCompletionKind(SyntaxKind kind, CompletionKind? defaultKind = null)
        {
            switch (kind.GetCategory())
            {
                case SyntaxCategory.Keyword:
                    return CompletionKind.Keyword;
                case SyntaxCategory.Operator:
                    return CompletionKind.ScalarInfix;
                case SyntaxCategory.Punctuation:
                    return CompletionKind.Punctuation;
                default:
                    return defaultKind ?? CompletionKind.Syntax;
            }
        }

        /// <summary>
        /// Gets the default <see cref="CompletionItem"/> for tokens with any of the specified <see cref="SyntaxKind"/>.
        /// </summary>
        private static IEnumerable<CompletionItem> GetCompletionItems(IEnumerable<SyntaxKind> kinds, CompletionKind? defaultKind, CompletionPriority priority) =>
            kinds.Select(k => CreateCompletionItem(k, GetCompletionKind(k, defaultKind), priority)).Where(i => i != null);

        /// <summary>
        /// Gets the default <see cref="CompletionItem"/> for tokens with any of the specified texts.
        /// </summary>
        private static IEnumerable<CompletionItem> GetCompletionItems(IEnumerable<string> texts, CompletionKind? defaultKind, CompletionPriority priority) =>
            texts.Select(t => CreateCompletionItem(t, GetCompletionKind(t, defaultKind), priority)).Where(i => i != null);

        /// <summary>
        /// A parser that parses a <see cref="SyntaxList"/> of elements.
        /// </summary>
        public static Parser<LexicalToken, SyntaxList<TElement>> List<TElement>(
            Parser<LexicalToken, TElement> elementParser,
            Func<Source<LexicalToken>, int, TElement> fnMissingElement = null,
            bool oneOrMore = false)
            where TElement : SyntaxElement
        {
            return Parsers<LexicalToken>.List(
                elementParser,
                fnMissingElement,
                oneOrMore,
                elements =>
                    new SyntaxList<TElement>(elements.ToArray()));
        }

        /// <summary>
        /// A parser that parses a <see cref="SyntaxList"/> of <see cref="SeparatedElement{TElement}"/>'s
        /// </summary>
        public static Parser<LexicalToken, SyntaxList<SeparatedElement<TElement>>> SeparatedList<TElement>(
            Parser<LexicalToken, TElement> primaryElementParser,
            SyntaxKind separatorKind,
            Func<Source<LexicalToken>, int, TElement> fnMissingElement,
            Parser<LexicalToken> endOfList = null,
            bool oneOrMore = false,
            bool allowTrailingSeparator = false)
            where TElement : SyntaxElement
        {
            return SeparatedList(
                primaryElementParser,
                separatorKind,
                primaryElementParser.WithTag("..."),
                fnMissingElement,
                endOfList,
                oneOrMore,
                allowTrailingSeparator);
        }

        /// <summary>
        /// A parser that parses a <see cref="SyntaxList"/> of <see cref="SeparatedElement"/>'s.
        /// </summary>
        public static Parser<LexicalToken, SyntaxList<SeparatedElement<TElement>>> SeparatedList<TElement>(
            Parser<LexicalToken, TElement> primaryElementParser,
            SyntaxKind separatorKind,
            Parser<LexicalToken, TElement> secondaryElementParser,
            Func<Source<LexicalToken>, int, TElement> fnMissingElement,
            Parser<LexicalToken> endOfList = null,
            bool oneOrMore = false,
            bool allowTrailingSeparator = false)
            where TElement : SyntaxElement
        {
            return OList(
                primaryElementParser,
                Token(separatorKind),
                secondaryElementParser,
                fnMissingElement,
                (source, start) => CreateMissingToken(separatorKind),
                fnMissingElement,
                endOfList,
                oneOrMore,
                allowTrailingSeparator,
                MakeSeparatedList<TElement>);
        }

        /// <summary>
        /// Determines if a typical comma separated list has ended
        /// </summary>
        public static Parser<LexicalToken> EndOfCommaList = Match(t =>
            t.Kind == SyntaxKind.EndOfTextToken
            || t.Kind == SyntaxKind.CloseParenToken
            || t.Kind == SyntaxKind.CloseBracketToken
            || t.Kind == SyntaxKind.CloseBraceToken
            || t.Kind == SyntaxKind.BarToken
            || t.Kind == SyntaxKind.SemicolonToken);

        /// <summary>
        /// A parser that parses a typical comma separated <see cref="SyntaxList"/> of <see cref="SeparatedElement"/>'s.
        /// </summary>
        public static Parser<LexicalToken, SyntaxList<SeparatedElement<TElement>>> CommaList<TElement>(
            Parser<LexicalToken, TElement> elementParser,
            Func<Source<LexicalToken>, int, TElement> fnMissingElement,
            bool oneOrMore = false,
            bool allowTrailingComma = false,
            IEnumerable<SyntaxKind> endKinds = null)
            where TElement : SyntaxElement
        {
            Parser<LexicalToken> endOfList = EndOfCommaList;

            if (endKinds != null)
            {
                var hash = new HashSet<SyntaxKind>(endKinds);
                endOfList = First(EndOfCommaList, Match(t => hash.Contains(t.Kind)));
            }

            return SeparatedList(elementParser, SyntaxKind.CommaToken, fnMissingElement, endOfList, oneOrMore, allowTrailingComma);
        }

        /// <summary>
        /// A parser that parses a typical comma separated <see cref="SyntaxList"/> of <see cref="SeparatedElement"/>'s.
        /// </summary>
        public static Parser<LexicalToken, SyntaxList<SeparatedElement<TElement>>> OneOrMoreCommaList<TElement>(
            Parser<LexicalToken, TElement> elementParser,
            Func<Source<LexicalToken>, int, TElement> fnMissingElement)
            where TElement : SyntaxElement
        {
            return Produce(
                Sequence(
                    Required(elementParser.Cast<SyntaxElement>(), (source, start) => (SyntaxElement)fnMissingElement(source, start)),
                    ZeroOrMore(
                        Sequence(
                            Rule(Token(SyntaxKind.CommaToken), t => (SyntaxElement)t),
                            Rule(elementParser, l => (SyntaxElement)l)))),
                elements => MakeSeparatedList<TElement>(elements));
        }

        /// <summary>
        /// Constructs a SyntaxList&lt;SeparatedElement&lt;TElement&gt;&gt; from a list of items and separators.
        /// </summary>
        public static SyntaxList<SeparatedElement<TElement>> MakeSeparatedList<TElement>(params SyntaxElement[] elements)
        where TElement : SyntaxElement
        {
            return MakeSeparatedList<TElement>((IReadOnlyList<SyntaxElement>)elements);
        }

        /// <summary>
        /// Constructs a SyntaxList&lt;SeparatedElement&lt;TElement&gt;&gt; from a list of items and separators.
        /// </summary>
        public static SyntaxList<SeparatedElement<TElement>> MakeSeparatedList<TElement>(IReadOnlyList<object> elements)
            where TElement : SyntaxElement
        {
            if (elements == null)
                return SyntaxList<SeparatedElement<TElement>>.Empty();

            var separatedElements = new List<SeparatedElement<TElement>>();

            for (int i = 0; i < elements.Count; i += 2)
            {
                var element = (TElement)elements[i];
                var separator = (i < elements.Count - 1) ? (SyntaxToken)elements[i + 1] : null;
                separatedElements.Add(new SeparatedElement<TElement>(element, separator));
            }

            return new SyntaxList<SeparatedElement<TElement>>(separatedElements);
        }

        /// <summary>
        /// Repeatedly parses all matching items.
        /// </summary>
        public static IEnumerable<TParser> ParseAll<TParser>(this Parser<LexicalToken, TParser> parser, string text, bool alwaysProduceEndToken = false)
        {
            return ParseAll(parser, TokenParser.ParseTokens(text, ParseOptions.Default.WithAlwaysProduceEndTokens(alwaysProduceEndToken)));
        }

        /// <summary>
        /// Repeatedly parses all matching items.
        /// </summary>
        public static IEnumerable<TParser> ParseAll<TParser>(this Parser<LexicalToken, TParser> parser, IReadOnlyList<LexicalToken> tokens)
        {
            var source = new ArraySource<LexicalToken>(tokens);
            var start = 0;

            while (!source.IsEnd(start))
            {
                var result = parser.Parse(source, start);

                if (result.Length <= 0)
                    break;

                yield return result.Value;
                start += result.Length;
            }
        }

        /// <summary>
        /// Parses the first matching item.
        /// </summary>
        public static TParser ParseFirst<TParser>(this Parser<LexicalToken, TParser> parser, string text, bool alwaysProduceEOF = false)
        {
            return ParseFirst(parser, TokenParser.ParseTokens(text, ParseOptions.Default.WithAlwaysProduceEndTokens(alwaysProduceEOF)));
        }

        /// <summary>
        /// Parses the first matching item.
        /// </summary>
        public static TParser ParseFirst<TParser>(this Parser<LexicalToken, TParser> parser, IReadOnlyList<LexicalToken> tokens)
        {
            var source = new ArraySource<LexicalToken>(tokens);
            var result = parser.Parse(source, 0);
            return result.Value;
        }

        /// <summary>
        /// Scans the first matching item.
        /// </summary>
        public static int ScanFirst(this Parser<LexicalToken> parser, string text, bool alwaysProduceEOF = false)
        {
            var source = new ArraySource<LexicalToken>(TokenParser.ParseTokens(text, ParseOptions.Default.WithAlwaysProduceEndTokens(alwaysProduceEOF)));
            return parser.Scan(source, 0);
        }

        /// <summary>
        /// Adds examples of completions as annotations onto this grammar rule.
        /// </summary>
        public static Parser<LexicalToken, TParser> Examples<TParser>(this Parser<LexicalToken, TParser> parser, IReadOnlyList<string> values) =>
            parser.WithAnnotations(values.Select(v => new CompletionItem(CompletionKind.Example, v)));

        /// <summary>
        /// Adds examples of completions as annotations onto this grammar rule.
        /// </summary>
        public static Parser<LexicalToken, TParser> Examples<TParser>(this Parser<LexicalToken, TParser> parser, params string[] values) =>
            Examples(parser, (IReadOnlyList<string>)values);
    }
}