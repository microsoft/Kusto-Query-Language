using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Kusto.Language.Syntax
{
    using Utils;

    /// <summary>
    /// A single token in the syntax grammar.
    /// </summary>
    public abstract class SyntaxToken : SyntaxElement
    {
        protected SyntaxToken(IReadOnlyList<Diagnostic> diagnostics)
            : base(diagnostics)
        {
        }

        /// <summary>
        /// Any whitespace or comments preceding this token.
        /// </summary>
        public virtual string Trivia => "";

        /// <summary>
        /// The raw text of the token
        /// </summary>
        public virtual string Text => "";

        /// <summary>
        /// The value of the literal.
        /// </summary>
        public virtual object Value => this.Text;

        /// <summary>
        /// The text of the literal value (does not include type prefixes/quotes)
        /// </summary>
        public virtual string ValueText => this.Text;

        public override int ChildCount => 0;

        /// <summary>
        /// The width (number of characters) of the trivia preceding the token.
        /// </summary>
        public override int TriviaWidth => this.Trivia.Length;

        /// <summary>
        /// The width (number of characters) of the token including trivia.
        /// </summary>
        public override int FullWidth => this.Trivia.Length + this.Text.Length;

        /// <summary>
        /// The width (number of characters) of the token, not including trivia.
        /// </summary>
        public override int Width => this.Text.Length;

        public override bool IsToken => true;

        /// <summary>
        /// The token is a literal.
        /// </summary>
        public virtual bool IsLiteral => false;

        /// <summary>
        /// The prefix of the literal in the form: prefix(value)
        /// </summary>
        public virtual string Prefix => "";

        /// <summary>
        /// Creates a copy of this <see cref="SyntaxToken"/>
        /// </summary>
        public new SyntaxToken Clone() => (SyntaxToken)this.CloneCore();

        public override string ToString(IncludeTrivia includeTrivia)
        {
            if (!string.IsNullOrEmpty(this.Trivia) && includeTrivia == IncludeTrivia.All)
            {
                return this.Trivia + this.Text;
            }
            else
            {
                return this.Text;
            }
        }

        internal void Write(StringBuilder builder, IncludeTrivia includeTrivia, int initialTriviaStart)
        {
            if (!string.IsNullOrEmpty(this.Trivia))
            {
                bool beforeQuery = this.TriviaStart == initialTriviaStart;
                bool afterQuery = this.Kind == SyntaxKind.EndOfTextToken;

                switch (includeTrivia)
                {
                    case IncludeTrivia.All:
                        builder.Append(this.Trivia);
                        break;

                    case IncludeTrivia.Interior:
                        if (!beforeQuery)
                        {
                            builder.Append(this.Trivia);
                        }
                        break;

                    case IncludeTrivia.Minimal:
                        if (!beforeQuery && !afterQuery)
                        {
                            // if there is any trivia replace it with single space or line-break
                            if (this.Trivia.Contains("\n"))
                            {
                                // trivia had a line feed, so minimal trivia is just one line feed
                                builder.Append("\n");
                            }
                            else
                            {
                                // minimal trivia is just a single space
                                builder.Append(" ");
                            }
                        }
                        break;

                    case IncludeTrivia.SingleLine:
                        if (!beforeQuery && !afterQuery)
                        {
                            // if there was any trivia replace it with a single space.
                            builder.Append(" ");
                        }
                        break;
                }
            }

            builder.Append(this.Text);
        }

        /// <summary>
        /// Gets the next <see cref="SyntaxToken"/> in lexical order.
        /// </summary>
        public SyntaxToken GetNextToken(bool includeZeroWidthTokens = false)
        {
            return GetNextToken(null, this, includeZeroWidthTokens);
        }

        /// <summary>
        /// Gets the previous <see cref="SyntaxToken"/> in lexical orer.
        /// </summary>
        public SyntaxToken GetPreviousToken(bool includeZeroWidthTokens = false)
        {
            return GetPreviousToken(null, this, includeZeroWidthTokens);
        }

        public static SyntaxToken From(Parsing.LexicalToken token, Diagnostic diagnostic = null)
        {
            var dx = token.Diagnostics;
            if (diagnostic != null)
            {
                dx = dx.Concat(new[] { diagnostic }).ToReadOnly();
            }

            switch (token.Kind.GetCategory())
            {
                case SyntaxCategory.Identifier:
                    return SyntaxToken.Identifier(token.Trivia, token.Text, dx);
                case SyntaxCategory.Keyword:
                    return SyntaxToken.Keyword(token.Trivia, token.Kind, dx);
                case SyntaxCategory.Literal:
                    return SyntaxToken.Literal(token.Trivia, token.Text, token.Kind, dx);
                case SyntaxCategory.Punctuation:
                    return SyntaxToken.Punctuation(token.Trivia, token.Kind, dx);
                case SyntaxCategory.Operator:
                    return SyntaxToken.Operator(token.Trivia, token.Kind, dx);
                case SyntaxCategory.Other:
                default:
                    return SyntaxToken.Other(token.Trivia, token.Text, token.Kind, dx);
            }
        }

        public static SyntaxToken Keyword(string trivia, SyntaxKind keyword, IReadOnlyList<Diagnostic> diagnostics = null)
        {
            CheckCategory(keyword, SyntaxCategory.Keyword);
            return new KindToken(trivia, keyword, diagnostics);
        }

        public static SyntaxToken Identifier(string trivia, string text, IReadOnlyList<Diagnostic> diagnostics = null)
        {
            return new IdentifierToken(trivia, text, diagnostics);
        }

        public static SyntaxToken Punctuation(string trivia, SyntaxKind kind, IReadOnlyList<Diagnostic> diagnostics = null)
        {
            CheckCategory(kind, SyntaxCategory.Punctuation);
            return new KindToken(trivia, kind, diagnostics);
        }

        public static SyntaxToken Operator(string trivia, SyntaxKind kind, IReadOnlyList<Diagnostic> diagnostics = null)
        {
            CheckCategory(kind, SyntaxCategory.Operator);
            return new KindToken(trivia, kind, diagnostics);
        }

        public static SyntaxToken Literal(string trivia, string text, SyntaxKind kind, IReadOnlyList<Diagnostic> diagnostics = null)
        {
            CheckCategory(kind, SyntaxCategory.Literal);
            return new LiteralToken(trivia, text, kind, diagnostics);
        }

        public static SyntaxToken Other(string trivia, string text, SyntaxKind kind, IReadOnlyList<Diagnostic> diagnostics = null)
        {
            CheckCategory(kind, SyntaxCategory.Other);
            return new TextAndKindToken(trivia, text, kind, diagnostics);
        }

        public static SyntaxToken Missing(string trivia, SyntaxKind kind, IReadOnlyList<Diagnostic> diagnostics)
        {
            return new MissingToken(trivia, kind, diagnostics);
        }

        public static SyntaxToken Missing(SyntaxKind kind, Diagnostic diagnostic = null)
        {
            return new MissingToken(string.Empty, kind, diagnostic != null ? new[] { diagnostic } : null);
        }

        private static void CheckCategory(SyntaxKind kind, SyntaxCategory category)
        {
            if (kind.GetCategory() != category)
            {
                throw new ArgumentException($"The kind {kind} is not a {category.ToString().ToLower()}");
            }
        }

        /// <summary>
        /// A <see cref="SyntaxToken"/> for identifiers.
        /// </summary>
        private sealed class IdentifierToken : SyntaxToken
        {
            private readonly string trivia;
            private readonly string text;
            private readonly int fullWidth;

            public override string Trivia => this.trivia;
            public override string Text => this.text;
            public override int FullWidth => this.fullWidth;

            public IdentifierToken(string trivia, string text, IReadOnlyList<Diagnostic> diagnostics)
                : base(diagnostics)
            {
                this.trivia = trivia ?? "";
                this.text = text;
                this.fullWidth = this.trivia.Length + this.text.Length;
            }

            public override SyntaxKind Kind => SyntaxKind.IdentifierToken;

            public override object Value => this.Text;

            protected override SyntaxElement CloneCore()
            {
                return new IdentifierToken(this.Trivia, this.Text, this.SyntaxDiagnostics);
            }
        }

        /// <summary>
        /// A <see cref="SyntaxToken"/> for keywords.
        /// </summary>
        private sealed class KindToken : SyntaxToken
        {
            private readonly string trivia;
            private readonly SyntaxKind kind;
            private readonly int fullWidth;

            public override string Trivia => this.trivia;
            public override SyntaxKind Kind => this.kind;
            public override int FullWidth => this.fullWidth;

            public KindToken(string trivia, SyntaxKind kind, IReadOnlyList<Diagnostic> diagnostics)
                : base(diagnostics)
            {
                this.trivia = trivia ?? "";
                this.kind = kind;
                this.fullWidth = this.trivia.Length + SyntaxFacts.GetText(this.Kind).Length;
            }

            public override string Text => SyntaxFacts.GetText(this.Kind);

            protected override SyntaxElement CloneCore()
            {
                return new KindToken(this.Trivia, this.Kind, this.SyntaxDiagnostics);
            }
        }

        /// <summary>
        /// A <see cref="SyntaxToken"/> that encodes the text and kind
        /// </summary>
        private sealed class TextAndKindToken : SyntaxToken
        {
            private readonly string trivia;
            private readonly string text;
            private readonly SyntaxKind kind;
            private readonly int fullWidth;

            public override string Trivia => this.trivia;
            public override string Text => text;
            public override SyntaxKind Kind => this.kind;
            public override int FullWidth => this.fullWidth;

            public TextAndKindToken(string trivia, string text, SyntaxKind kind, IReadOnlyList<Diagnostic> diagnostics)
                : base(diagnostics)
            {
                this.trivia = trivia ?? "";
                this.text = text;
                this.kind = kind;
                this.fullWidth = this.trivia.Length + this.text.Length;
            }

            protected override SyntaxElement CloneCore()
            {
                return new TextAndKindToken(this.Trivia, this.Text, this.Kind, this.SyntaxDiagnostics);
            }
        }

        /// <summary>
        /// A <see cref="SyntaxToken"/> for a literal value.
        /// </summary>
        private sealed class LiteralToken : SyntaxToken
        {
            private readonly string trivia;
            private readonly string text;
            private readonly SyntaxKind kind;
            private readonly int fullWidth;

            public override string Trivia => this.trivia;
            public override string Text => text;
            public override SyntaxKind Kind => this.kind;
            public override int FullWidth => this.fullWidth;

            public LiteralToken(string trivia, string text, SyntaxKind kind, IReadOnlyList<Diagnostic> diagnostics)
                : base(diagnostics)
            {
                this.trivia = trivia ?? "";
                this.text = text;
                this.kind = kind;
                this.fullWidth = this.trivia.Length + this.text.Length;
            }

            protected override SyntaxElement CloneCore()
            {
                return new LiteralToken(this.Trivia, this.Text, this.Kind, this.SyntaxDiagnostics);
            }

            public override bool IsLiteral => true;

            private object value;

            public override object Value
            {
                get
                {
                    if (this.value == null)
                    {
                        this.value = this.GetValue();
                    }

                    return this.value;
                }
            }

            private string valueText;

            public override string ValueText
            {
                get
                {
                    if (this.valueText == null)
                    {
                        if (GetPrefixLength(this.Text) > 0)
                        {
                            this.valueText = GetValueText(this.Text);
                        }
                        else if (this.Kind == SyntaxKind.StringLiteralToken)
                        {
                            this.valueText = (string)this.Value;
                        }
                        else
                        {
                            this.valueText = this.Text;
                        }
                    }

                    return valueText;
                }
            }

            private object GetValue()
            {
                if (this.Kind != SyntaxKind.StringLiteralToken
                    && IsNull(this.Text))
                {
                    return null;
                }

                switch (this.Kind)
                {
                    case SyntaxKind.BooleanLiteralToken:
                        return GetBooleanValue(this.Text);
                    case SyntaxKind.IntLiteralToken:
                        return GetIntValue(this.Text);
                    case SyntaxKind.LongLiteralToken:
                        return GetLongValue(this.Text);
                    case SyntaxKind.RealLiteralToken:
                        return GetRealValue(this.Text);
                    case SyntaxKind.DecimalLiteralToken:
                        return GetDecimalValue(this.Text);
                    case SyntaxKind.TimespanLiteralToken:
                        return GetTimeSpanValue(this.Text);
                    case SyntaxKind.DateTimeLiteralToken:
                        return GetDateTimeValue(this.Text);
                    case SyntaxKind.GuidLiteralToken:
                        return GetGuidValue(this.Text);
                    case SyntaxKind.StringLiteralToken:
                        return KustoFacts.GetStringLiteralValue(this.Text);
                    default:
                        throw new InvalidOperationException($"Unhandled literal syntax kind: {this.Kind}");
                }
            }

            public override string Prefix
            {
                get
                {
                    var prefixLen = GetPrefixLength(this.Text);
                    if (prefixLen > 0)
                    {
                        return this.Text.Substring(0, prefixLen);
                    }
                    else
                    {
                        return "";
                    }
                }
            }

            private static int GetPrefixLength(string text)
            {
                int i = 0;
                while (i < text.Length)
                {
                    var ch = text[i];
                    if (ch == '(' && i > 0)
                    {
                        return i;
                    }
                    else if (!char.IsLetter(ch))
                    {
                        return 0;
                    }
                    else
                    {
                        i++;
                    }
                }

                return 0;
            }

            private static void GetValueSpan(string text, out int start, out int length)
            {
                start = 0;
                int end = text.Length;

                var prefixLen = GetPrefixLength(text);
                if (prefixLen > 0)
                {
                    start = prefixLen + 1; // include (

                    // trim leading whitespace
                    while (start < text.Length && Parsing.TextFacts.IsWhitespace(text[start]))
                        start++;

                    if (text.EndsWith(")"))
                    {
                        end = text.Length - 1;
                    }

                    // trim trailing whitespace
                    while (end > start + 1 && Parsing.TextFacts.IsWhitespace(text[end - 1]))
                        end--;
                }

                length = end - start;
            }

            private static string GetValueText(string text)
            {
                int start;
                int length;
                GetValueSpan(text, out start, out length);

                return (start == 0 && length == text.Length)
                    ? text
                    : text.Substring(start, length);
            }

            private static bool IsNull(string text)
            {
                GetValueSpan(text, out var start, out var length);
                return string.Compare(text, start, "null", 0, length) == 0;
            }

            private static object GetBooleanValue(string text)
            {
                GetValueSpan(text, out var start, out var length);

                if (string.Compare(text, start, "true", 0, length, ignoreCase: true) == 0)
                {
                    return true;
                }
                else if (string.Compare(text, start, "false", 0, length, ignoreCase: true) == 0)
                {
                    return false;
                }
                else
                {
                    return null;
                }
            }

            private static int GetIntValue(string text)
            {
                var valueText = GetValueText(text);
                switch (valueText)
                {
                    case "min":
                        return Int32.MinValue;
                    case "max":
                        return Int32.MaxValue;
                    default:
                        Int32.TryParse(valueText, out var result);
                        return result;
                }
            }

            private static long GetLongValue(string text)
            {
                var valueText = GetValueText(text);
                switch (valueText)
                {
                    case "min":
                        return Int64.MinValue;
                    case "max":
                        return Int64.MaxValue;
                    default:
                        Int64.TryParse(valueText, out var result);
                        return result;
                }
            }

            private static double GetRealValue(string text)
            {
                var valueText = GetValueText(text);
                switch (valueText)
                {
                    case "min":
                        return Double.MinValue;
                    case "max":
                        return Double.MaxValue;
                    default:
                        Double.TryParse(valueText, out var result);
                        return result;
                }
            }

            private static decimal GetDecimalValue(string text)
            {
                var valueText = GetValueText(text);
                switch (valueText)
                {
                    case "min":
                        return Decimal.MinValue;
                    case "max":
                        return Decimal.MaxValue;
                    default:
                        Decimal.TryParse(valueText, out var result);
                        return result;
                }
            }

            private static TimeSpan GetTimeSpanValue(string text)
            {
                var valueText = GetValueText(text);

#if !BRIDGE
                TimeSpan result;
                if (TimeSpan.TryParse(valueText, out result))
                {
                    return result;
                }
#endif
                switch (valueText)
                {
                    case "min":
                        return TimeSpan.MinValue;
                    case "max":
                        return TimeSpan.MaxValue;
                }

                // find number/word split
                int split = 0;
                char ch;
                while (split < valueText.Length
                    && char.IsDigit(ch = valueText[split]) || (valueText[split] == '.'))
                {
                    split++;
                }

                var numberText = valueText.Substring(0, split);
                var wordText = valueText.Substring(split);

                if (Double.TryParse(numberText, out var number))
                {
                    switch (wordText)
                    {
                        case "s":
                        case "sec":
                        case "second":
                        case "seconds":
                            return TimeSpan.FromSeconds(number);
                        case "m":
                        case "min":
                        case "minute":
                        case "minutes":
                            return TimeSpan.FromMinutes(number);
                        case "h":
                        case "hr":
                        case "hrs":
                        case "hour":
                        case "hours":
                            return TimeSpan.FromHours(number);
                        case "d":
                        case "day":
                        case "days":
                            return TimeSpan.FromDays(number);
                        case "ms":
                        case "milli":
                        case "millis":
                        case "millisec":
                        case "millisecond":
                        case "milliseconds":
                            return TimeSpan.FromMilliseconds(number);
                        case "micro":
                        case "micros":
                        case "microsec":
                        case "microsecond":
                        case "microseconds":
                            return TimeSpan.FromSeconds(number / 1_000_000.0);
                        case "nano":
                        case "nanos":
                        case "nanosec":
                        case "nanosecond":
                        case "nanoseconds":
                            return TimeSpan.FromSeconds(number / 1_000_000_000.0);
                        case "tick":
                        case "ticks":
                            return TimeSpan.FromTicks((long)number);
                    }
                }

                // bad timespan literal?
                return TimeSpan.FromSeconds(0.0);
            }

            private static DateTime GetDateTimeValue(string text)
            {
                var valueText = GetValueText(text);
                switch (valueText)
                {
                    case "min":
                        return DateTime.MinValue;
                    case "max":
                        return DateTime.MaxValue;
                    default:
                        DateTime.TryParse(valueText, out var result);
                        return result;
                }
            }

            private static Guid GetGuidValue(string text)
            {
                var valueText = GetValueText(text);
                Guid.TryParse(valueText, out var result);
                return result;
            }
        }

        public class MissingToken : SyntaxToken
        {
            private readonly string trivia;
            private readonly SyntaxKind kind;

            public override string Trivia => this.trivia;
            public override SyntaxKind Kind => this.kind;
            public override int FullWidth => this.trivia.Length;

            public MissingToken(string trivia, SyntaxKind kind, IReadOnlyList<Diagnostic> diagnostics)
                : base(diagnostics)
            {
                this.trivia = trivia;
                this.kind = kind;
            }

            public override string Text => "";

            public override bool IsMissing => true;

            protected override SyntaxElement CloneCore()
            {
                return new MissingToken(this.Trivia, this.Kind, this.SyntaxDiagnostics);
            }
        }
    }
}