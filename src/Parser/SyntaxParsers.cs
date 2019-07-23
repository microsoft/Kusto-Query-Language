using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static SyntaxToken CreateMissingToken(SyntaxKind kind, Diagnostic diagnostic = null)
        {
            var dx = diagnostic ??
                (kind == SyntaxKind.IdentifierToken ? DiagnosticFacts.GetMissingName() : DiagnosticFacts.GetTokenExpected(kind));
            return SyntaxToken.Missing("", kind, new[] { dx });
        }

        public static SyntaxToken CreateMissingToken(IReadOnlyList<SyntaxKind> kinds)
        {
            return SyntaxToken.Missing("", kinds[0], new[] { DiagnosticFacts.GetTokenExpected(kinds) });
        }

        public static SyntaxToken CreateMissingToken(string text, Diagnostic diagnostic = null)
        {
            if (!SyntaxFacts.TryGetKind(text, out var kind))
            {
                kind = SyntaxKind.IdentifierToken;
            }

            return SyntaxToken.Missing("", kind, new[] { diagnostic ?? DiagnosticFacts.GetTokenExpected(new[] { text }) });
        }

        public static SyntaxToken CreateMissingToken(IReadOnlyList<string> texts)
        {
            return SyntaxToken.Missing("", SyntaxKind.IdentifierToken, new[] { DiagnosticFacts.GetTokenExpected(texts) });
        }

        /// <summary>
        /// Parses any token
        /// </summary>
        public static readonly Parser<LexicalToken, SyntaxToken> AnyToken =
            Match(t => true, t => SyntaxToken.From(t)).WithTag("<any>");

        /// <summary>
        /// Parse any token but the <see cref="SyntaxKind.EndOfTextToken"/> token.
        /// </summary>
        public static readonly Parser<LexicalToken, SyntaxToken> AnyTokenButEnd =
            Match(t => t.Kind != SyntaxKind.EndOfTextToken, t => SyntaxToken.From(t)).WithTag("<any!end>");


        private static string GetTag(string text)
        {
            if (SyntaxFacts.TryGetKind(text, out var kind))
            {
                return GetTag(kind);
            }
            else
            {
                return text;
            }
        }

        private static string GetTag(SyntaxKind kind)
        {
            return kind.GetCategory() == SyntaxCategory.Punctuation
                    ? $"'{kind.GetText()}'"
                    : kind.GetText() ?? kind.ToString().ToLower();
        }

        /// <summary>
        /// A parser that parses the next <see cref="SyntaxToken"/> with the specified <see cref="SyntaxKind"/>.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> Token(SyntaxKind kind, CompletionKind? ckind = null, CompletionPriority priority = CompletionPriority.Normal, string ctext = null)
        {
            var rule = Match(t => t.Kind == kind, lt => SyntaxToken.From(lt)).WithTag(GetTag(kind));

            var item = GetCompletionItem(kind, ckind, priority, ctext);
            if (item != null)
            {
                rule = rule.WithAnnotations(new[] { item });
            }

            return rule;
        }

        /// <summary>
        /// Add completion annotation to the parser.
        /// </summary>
        public static Parser<LexicalToken, TElement> WithCompletion<TElement>(this Parser<LexicalToken, TElement> parser, CompletionItem item)
        {
            return parser.WithAnnotations(parser.Annotations.Concat(new[] { (object)item }));
        }

        /// <summary>
        /// Add completion hint to the parser.
        /// </summary>
        public static Parser<LexicalToken, TElement> WithCompletionHint<TElement>(this Parser<LexicalToken, TElement> parser, CompletionHint hint)
        {
            return parser.WithAnnotations(parser.Annotations.Concat(new[] { (object)hint }));
        }

        /// <summary>
        /// A parser that parses the next <see cref="SyntaxToken"/> with one of the specified <see cref="SyntaxKind"/>s.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> Token(IReadOnlyList<SyntaxKind> kinds, CompletionKind? ckind = null, CompletionPriority priority = CompletionPriority.Normal)
        {
            Ensure.ArgumentNotNull(kinds, nameof(kinds));
            var set = new HashSet<SyntaxKind>(kinds);

            var rule = Match(t => set.Contains(t.Kind), lt => SyntaxToken.From(lt)).WithTag(string.Join(" | ", kinds.Select(k => GetTag(k))));

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
        private static int MatchesText(Source<LexicalToken> source, int start, string text)
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

        private static SyntaxToken ProduceSyntaxToken(Source<LexicalToken> source, int start, int length, string text)
        {
            if (length == 1)
            {
                return SyntaxToken.From(source.Peek(start));
            }
            else
            {
                return SyntaxToken.Identifier(source.Peek(start).Trivia, text);
            }
        }

        private static Parser<LexicalToken, SyntaxToken> MatchText(string text) =>
            Match(
                (source, start) => MatchesText(source, start, text), 
                (source, start, length) => ProduceSyntaxToken(source, start, length, text));

        /// <summary>
        /// A parser that parses the next <see cref="SyntaxToken"/> with the specified text value.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> Token(string text, CompletionKind? ckind = null, CompletionPriority priority = CompletionPriority.Normal, string ctext = null)
        {
            var rule = MatchText(text).WithTag(GetTag(text));

            var item = GetCompletionItem(text, ckind, priority, ctext);
            if (item != null)
            {
                rule = rule.WithAnnotations(new[] { item });
            }

            return rule;
        }

        /// <summary>
        /// A parser that parses the next <see cref="SyntaxToken"/> with one of the specified text values.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> Token(IReadOnlyList<string> texts, CompletionKind? ckind = null, CompletionPriority priority = CompletionPriority.Normal)
        {
            Ensure.ArgumentNotNull(texts, nameof(texts));
            var set = new HashSet<string>(texts);

            var rule = Match(t => set.Contains(t.Text), lt => SyntaxToken.From(lt)).WithTag(string.Join(" | ", texts.Select(t => GetTag(t))));

            var items = GetCompletionItems(texts, ckind, priority).ToList();
            if (items.Count > 0)
            {
                rule = rule.WithAnnotations(items);
            }

            return rule;
        }

        /// <summary>
        /// A parser that parses the next <see cref="SyntaxToken"/> with the specified <see cref="SyntaxKind"/>, or produces the equivalent missing <see cref="SyntaxToken"/>.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> RequiredToken(SyntaxKind kind, CompletionKind? ckind = null, CompletionPriority priority = CompletionPriority.Normal, string ctext = null) =>
            Required(Token(kind, ckind, priority, ctext), () => CreateMissingToken(kind));

        /// <summary>
        /// A parser that parses the next <see cref="SyntaxToken"/> with one of the specified <see cref="SyntaxKind"/>s, or produces the equivalent missing <see cref="SyntaxToken"/>.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> RequiredToken(IReadOnlyList<SyntaxKind> kinds, CompletionKind? ckind = null, CompletionPriority priority = CompletionPriority.Normal) =>
            Required(Token(kinds, ckind, priority), () => CreateMissingToken(kinds));

        /// <summary>
        /// A parser that parses the next <see cref="SyntaxToken"/> with one of the specified text values, or produces the equivalent missing <see cref="SyntaxToken"/>.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> RequiredToken(string text, CompletionKind? ckind = null, CompletionPriority priority = CompletionPriority.Normal, string ctext = null) =>
            Required(Token(text, ckind, priority, ctext), () => CreateMissingToken(text));

        /// <summary>
        /// A parser that parses the next <see cref="SyntaxToken"/> with one of the specified text values, or produces the equivalent missing <see cref="SyntaxToken"/>.
        /// </summary>
        public static Parser<LexicalToken, SyntaxToken> RequiredToken(IReadOnlyList<string> texts, CompletionKind? ckind = null, CompletionPriority priority = CompletionPriority.Normal) =>
            Required(Token(texts, ckind, priority), () => CreateMissingToken(texts));

        private static CompletionItem GetCompletionItem(SyntaxKind kind, CompletionKind? ckind, CompletionPriority priority, string ctext = null)
        {
            var text = SyntaxFacts.GetText(kind);

            if (string.IsNullOrWhiteSpace(text))
                return null;

            switch (kind.GetCategory())
            {
                case SyntaxCategory.Keyword:
                    return GetCompletionItem(text, ckind ?? CompletionKind.Keyword, priority, ctext);
                case SyntaxCategory.Operator:
                    return GetCompletionItem(text, ckind ?? CompletionKind.ScalarInfix, priority, ctext);
                case SyntaxCategory.Punctuation:
                    return GetCompletionItem(text, ckind ?? CompletionKind.Punctuation, priority, ctext);
                default:
                    return null;
            }
        }

        private static CompletionItem GetCompletionItem(string text, CompletionKind? ckind, CompletionPriority priority, string ctext = null)
        {
            return GetCompletionItem(text, ckind ?? CompletionKind.Syntax, priority, ctext);
        }

        private static CompletionItem GetCompletionItem(string text, CompletionKind ckind, CompletionPriority priority, string ctext = null)
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

        private static IEnumerable<CompletionItem> GetCompletionItems(IEnumerable<SyntaxKind> kinds, CompletionKind? ckind, CompletionPriority priority) =>
            kinds.Select(k => GetCompletionItem(k, ckind, priority)).Where(i => i != null);

        private static IEnumerable<CompletionItem> GetCompletionItems(IEnumerable<string> texts, CompletionKind? ckind, CompletionPriority priority) =>
            texts.Select(t => GetCompletionItem(t, ckind, priority)).Where(i => i != null);

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
                elements => new SyntaxList<TElement>(elements.ToArray()));
        }

        /// <summary>
        /// A parser that parses a <see cref="SyntaxList"/> of <see cref="SeparatedElement"/>'s.
        /// </summary>
        public static Parser<LexicalToken, SyntaxList<SeparatedElement<TElement>>> SeparatedList<TElement>(
            Parser<LexicalToken, TElement> primaryElementParser,
            SyntaxKind separatorKind,
            TElement missingElement,
            bool oneOrMore = false,
            bool allowTrailingSeparator = false)
            where TElement : SyntaxElement
        {
            return SeparatedList(
                primaryElementParser,
                Tag("...", primaryElementParser),
                separatorKind,
                missingElement,
                oneOrMore,
                allowTrailingSeparator);
        }

        /// <summary>
        /// A parser that parses a <see cref="SyntaxList"/> of <see cref="SeparatedElement"/>'s.
        /// </summary>
        public static Parser<LexicalToken, SyntaxList<SeparatedElement<TElement>>> SeparatedList<TElement>(
            Parser<LexicalToken, TElement> primaryElementParser,
            Parser<LexicalToken, TElement> secondaryElementParser,
            SyntaxKind separatorKind,
            TElement missingElement,
            bool oneOrMore = false,
            bool allowTrailingSeparator = false)
            where TElement : SyntaxElement
        {
            return SeparatedList<SyntaxElement, SyntaxList<SeparatedElement<TElement>>>(
                Rule(primaryElementParser, e => (SyntaxElement)e),
                Rule(secondaryElementParser, e => (SyntaxElement)e),
                Rule(Token(separatorKind), e => (SyntaxElement)e),
                () => missingElement.Clone(),
                () => CreateMissingToken(separatorKind),
                oneOrMore,
                allowTrailingSeparator,

                MakeSeparatedList<TElement>);
        }

        public static SyntaxList<SeparatedElement<TElement>> MakeSeparatedList<TElement>(params SyntaxElement[] elements)
            where TElement : SyntaxElement
        {
            return MakeSeparatedList<TElement>((IReadOnlyList<SyntaxElement>)elements);
        }

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
            return ParseAll(parser, LexicalGrammar.GetTokens(text, alwaysProduceEndToken));
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
            return ParseFirst(parser, LexicalGrammar.GetTokens(text, alwaysProduceEOF));
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
        /// Adds examples of completions as annotations onto this grammar rule.
        /// </summary>
        public static Parser<LexicalToken, TParser> Examples<TParser>(this Parser<LexicalToken, TParser> parser, IReadOnlyList<string> values) =>
            parser.WithAnnotations(values.Select(v => new CompletionItem(CompletionKind.Literal, v)));

        /// <summary>
        /// Adds examples of completions as annotations onto this grammar rule.
        /// </summary>
        public static Parser<LexicalToken, TParser> Examples<TParser>(this Parser<LexicalToken, TParser> parser, params string[] values) =>
            Examples(parser, (IReadOnlyList<string>)values);
    }
}