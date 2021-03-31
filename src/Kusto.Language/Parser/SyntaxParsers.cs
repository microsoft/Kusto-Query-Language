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
            var item = GetDefaultCompletionItem(kind, ckind, priority, ctext);
            return Token(kind, item);
        }

        /// <summary>
        /// A parser that consumes the next next <see cref="LexicalToken"/> if it has the specified <see cref="SyntaxKind"/>, producing a corresponding <see cref="SyntaxToken"/>.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> Token(SyntaxKind kind, CompletionItem item)
        {
            var rule = Match(t => t.Kind == kind, lt => SyntaxToken.From(lt)).WithTag(GetDefaultTag(kind));

            item = item ?? GetDefaultCompletionItem(kind, null, CompletionPriority.Normal, null);
            if (item != null)
            {
                rule = rule.WithAnnotations(new[] { item });
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
        public static Parser<LexicalToken, SyntaxToken> Token(IReadOnlyList<SyntaxKind> kinds, CompletionKind? ckind = null, CompletionPriority priority = CompletionPriority.Normal)
        {
            Ensure.ArgumentNotNull(kinds, nameof(kinds));
            var set = new HashSet<SyntaxKind>(kinds);

            var rule = Match(t => set.Contains(t.Kind), lt => SyntaxToken.From(lt)).WithTag(string.Join(" | ", kinds.Select(k => GetDefaultTag(k))));

            var items = GetCompletionItems(set, ckind, priority).ToList();
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
        /// Create a <see cref="SyntaxToken"/> from one or more <see cref="LexicalToken"/>.
        /// </summary>
        public static SyntaxToken ProduceSyntaxToken(Source<LexicalToken> source, int start, int length, string text)
        {
            if (length == 1)
            {
                return SyntaxToken.From(source.Peek(start));
            }
            else
            {
                // use the trivia form the first token and the text supplied (instead of concatenating the same text from the tokens)
                return SyntaxToken.Identifier(source.Peek(start).Trivia, text);
            }
        }

        /// <summary>
        /// A parser that consumes the next <see cref="LexicalToken"/> (or series of adjacent tokens) that combined has the specified text, producing a single <see cref="SyntaxToken"/>.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> MatchText(string text) =>
            Match(
                (source, start) => MatchesText(source, start, text), 
                (source, start, length) => ProduceSyntaxToken(source, start, length, text));

        /// <summary>
        /// A parser that consumes the next <see cref="LexicalToken"/> (or series of adjacent tokens) if it has the specified text, producing a single <see cref="SyntaxToken"/>.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> Token(string text, CompletionKind? ckind = null, CompletionPriority priority = CompletionPriority.Normal, string ctext = null)
        {
            var item = GetDefaultCompletionItem(text, ckind, priority, ctext);
            return Token(text, item);
        }

        /// <summary>
        /// A parser that consumes the next <see cref="LexicalToken"/> (or series of adjacent tokens) if it has the specified text, producing a single <see cref="SyntaxToken"/>.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> Token(string text, CompletionItem item)
        {
            var rule = MatchText(text).WithTag(GetDefaultTag(text));

            item = item ?? GetDefaultCompletionItem(text, null, CompletionPriority.Normal, null);
            if (item != null)
            {
                rule = rule.WithAnnotations(new[] { item });
            }

            return rule;
        }

        /// <summary>
        /// A parser that consumes the next <see cref="LexicalToken"/> (or series of adjacent tokens) if it has one of the specified texts, producing a single <see cref="SyntaxToken"/>.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> Token(IReadOnlyList<string> texts, CompletionKind? ckind = null, CompletionPriority priority = CompletionPriority.Normal)
        {
            Ensure.ArgumentNotNull(texts, nameof(texts));
            var set = new HashSet<string>(texts);

            var rule = Match(t => set.Contains(t.Text), lt => SyntaxToken.From(lt)).WithTag(string.Join(" | ", texts.Select(t => GetDefaultTag(t))));

            var items = GetCompletionItems(texts, ckind, priority).ToList();
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
        public static Parser<LexicalToken, SyntaxToken> HiddenToken(string text)
        {
            return MatchText(text).WithTag(GetDefaultTag(text));
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
        public static Parser<LexicalToken, SyntaxToken> RequiredToken(string text, CompletionKind? ckind = null, CompletionPriority priority = CompletionPriority.Normal, string ctext = null) =>
            Required(Token(text, ckind, priority, ctext), () => CreateMissingToken(text));

        /// <summary>
        /// A parser that consumes the next <see cref="LexicalToken"/> (or series of adjacent tokens) if it has the specified text, producing a corresponding <see cref="SyntaxToken"/> or an equivalent missing token otherwise.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> RequiredToken(string text, CompletionItem item) =>
            Required(Token(text, item), () => CreateMissingToken(text));

        /// <summary>
        /// A parser that consumes the next <see cref="LexicalToken"/> (or series of adjacent tokens) if it has one of the specified texts, producing a corresponding <see cref="SyntaxToken"/> or an equivalent missing token otherwise.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> RequiredToken(IReadOnlyList<string> texts, CompletionKind? ckind = null, CompletionPriority priority = CompletionPriority.Normal) =>
            Required(Token(texts, ckind, priority), () => CreateMissingToken(texts));

        /// <summary>
        /// Gets the default <see cref="CompletionItem"/> for a token with the specified <see cref="SyntaxKind"/>.
        /// </summary>
        private static CompletionItem GetDefaultCompletionItem(SyntaxKind kind, CompletionKind? ckind, CompletionPriority priority, string ctext = null)
        {
            var text = SyntaxFacts.GetText(kind);

            if (string.IsNullOrWhiteSpace(text))
                return null;

            switch (kind.GetCategory())
            {
                case SyntaxCategory.Keyword:
                    return GetDefaultCompletionItem(text, ckind ?? CompletionKind.Keyword, priority, ctext);
                case SyntaxCategory.Operator:
                    return GetDefaultCompletionItem(text, ckind ?? CompletionKind.ScalarInfix, priority, ctext);
                case SyntaxCategory.Punctuation:
                    return GetDefaultCompletionItem(text, ckind ?? CompletionKind.Punctuation, priority, ctext);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets the default <see cref="CompletionItem"/> for a token with the specified text.
        /// </summary>
        private static CompletionItem GetDefaultCompletionItem(string text, CompletionKind? ckind, CompletionPriority priority, string ctext = null)
        {
            return GetDefaultCompletionItem(text, ckind ?? CompletionKind.Syntax, priority, ctext);
        }

        /// <summary>
        /// Gets the default <see cref="CompletionItem"/> for a token with the specified text.
        /// </summary>
        private static CompletionItem GetDefaultCompletionItem(string text, CompletionKind ckind, CompletionPriority priority, string ctext = null)
        {
            // hide any syntax that starts with _ from completion
            if (text.StartsWith("_"))
                return null;

            string afterText = null;
            string editText = ctext ?? text;

            if (ctext != null)
            {
                var cursor = ctext.IndexOf('|');
                if (cursor >= 0)
                {
                    afterText = ctext.Substring(cursor + 1);
                    editText = ctext.Substring(0, cursor);
                }
            }

            return new CompletionItem(ckind, displayText: text, editText: editText, afterText: afterText, priority: priority);
        }

        /// <summary>
        /// Gets the default <see cref="CompletionItem"/> for tokens with any of the specified <see cref="SyntaxKind"/>.
        /// </summary>
        private static IEnumerable<CompletionItem> GetCompletionItems(IEnumerable<SyntaxKind> kinds, CompletionKind? ckind, CompletionPriority priority) =>
            kinds.Select(k => GetDefaultCompletionItem(k, ckind, priority)).Where(i => i != null);

        /// <summary>
        /// Gets the default <see cref="CompletionItem"/> for tokens with any of the specified texts.
        /// </summary>
        private static IEnumerable<CompletionItem> GetCompletionItems(IEnumerable<string> texts, CompletionKind? ckind, CompletionPriority priority) =>
            texts.Select(t => GetDefaultCompletionItem(t, ckind, priority)).Where(i => i != null);

        /// <summary>
        /// A parser that parses a <see cref="SyntaxList"/> of elements.
        /// </summary>
        public static Parser<LexicalToken, SyntaxList<TElement>> List<TElement>(
            Parser<LexicalToken, TElement> elementParser,
            TElement missingElement = null,
            bool oneOrMore = false)
            where TElement : SyntaxElement
        {
            return Parsers<LexicalToken>.List(
                elementParser,
                missingElement != null ? (Func<TElement>)(() => (TElement)missingElement.Clone()) : null,
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
            TElement missingElement,
            Parser<LexicalToken> endOfList = null,
            bool oneOrMore = false,
            bool allowTrailingSeparator = false)
            where TElement : SyntaxElement
        {
            return SeparatedList(
                primaryElementParser,
                separatorKind,
                primaryElementParser.WithTag("..."),
                missingElement,
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
            TElement missingElement,
            Parser<LexicalToken> endOfList = null,
            bool oneOrMore = false,
            bool allowTrailingSeparator = false)
            where TElement : SyntaxElement
        {
            return OList(
                primaryElementParser,
                Token(separatorKind),
                secondaryElementParser,
                () => (TElement)missingElement.Clone(),
                () => CreateMissingToken(separatorKind),
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
            TElement missingElement,
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

            return SeparatedList(elementParser, SyntaxKind.CommaToken, missingElement, endOfList, oneOrMore, allowTrailingComma);
        }

        public static Parser<LexicalToken, SyntaxList<SeparatedElement<TElement>>> OneOrMoreCommaList<TElement>(Parser<LexicalToken, TElement> elementParser, TElement missingElement) where TElement : SyntaxNode =>
            Produce(
                Sequence(
                    Required(elementParser.Cast<SyntaxElement>(), () => (SyntaxElement)missingElement.Clone()),
                    ZeroOrMore(
                        Sequence(
                            Rule(Token(SyntaxKind.CommaToken), t => (SyntaxElement)t),
                            Rule(elementParser, l => (SyntaxElement)l)))),
                elements => MakeSeparatedList<TElement>(elements));

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
            return ParseAll(parser, TokenParser.ParseTokens(text, alwaysProduceEndToken));
        }

        /// <summary>
        /// Repeatedly parses all matching items.
        /// </summary>
        public static IEnumerable<TParser> ParseAll<TParser>(this Parser<LexicalToken, TParser> parser, LexicalToken[] tokens)
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
            return ParseFirst(parser, TokenParser.ParseTokens(text, alwaysProduceEOF));
        }

        /// <summary>
        /// Parses the first matching item.
        /// </summary>
        public static TParser ParseFirst<TParser>(this Parser<LexicalToken, TParser> parser, LexicalToken[] tokens)
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
            var source = new ArraySource<LexicalToken>(TokenParser.ParseTokens(text, alwaysProduceEOF));
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