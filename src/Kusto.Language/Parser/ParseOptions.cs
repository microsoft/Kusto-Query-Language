using System;

namespace Kusto.Language
{
    public class ParseOptions : IEquatable<ParseOptions>
    {
        /// <summary>
        /// Determines whether the token parser will always include an end token.
        /// </summary>
        public bool AlwaysProduceEndToken { get; }

        /// <summary>
        /// Determines whether parenthesized literals like datetime(...) are allowed to 
        /// contain line breaks.
        /// </summary>
        public bool AllowLiteralsWithLineBreaks { get; }

        /// <summary>
        /// Determines whether the parser allows parts of wildcarded name to be non-adjacent.
        /// </summary>
        public bool AllowNonAdjacentWildcardParts { get; }

        /// <summary>
        /// Determines which kind of parser to use.
        /// </summary>
        public ParserKind ParserKind { get; }

        private ParseOptions(
            bool alwaysProduceEndTokens,
            bool allowLiteralsWithLineBreaks,
            bool allowNonAdjacentWildcardParts,
            ParserKind parserKind)
        {
            this.AlwaysProduceEndToken = alwaysProduceEndTokens;
            this.AllowLiteralsWithLineBreaks = allowLiteralsWithLineBreaks;
            this.AllowNonAdjacentWildcardParts = allowNonAdjacentWildcardParts;
            this.ParserKind = parserKind;
        }

        private ParseOptions With(
            bool? alwaysProduceEndTokens = null,
            bool? allowLiteralsWithLineBreaks = null,
            bool? allowNonAdjacentWildcardParts = null,
            ParserKind? parserKind = null)
        {
            var newAlwaysProduceEndTokens = alwaysProduceEndTokens ?? this.AlwaysProduceEndToken;
            var newAllowLiteralsWithLineBreaks = allowLiteralsWithLineBreaks ?? this.AllowLiteralsWithLineBreaks;
            var newAllowNonAdjacentWildcardParts = allowNonAdjacentWildcardParts ?? this.AllowNonAdjacentWildcardParts;
            var newParserKind = parserKind ?? this.ParserKind;

            if (newAlwaysProduceEndTokens != this.AlwaysProduceEndToken 
                || newAllowLiteralsWithLineBreaks != this.AllowLiteralsWithLineBreaks               
                || newAllowNonAdjacentWildcardParts != this.AllowNonAdjacentWildcardParts
                || newParserKind != this.ParserKind)
            {
                return new ParseOptions(
                    newAlwaysProduceEndTokens,
                    newAllowLiteralsWithLineBreaks,
                    newAllowNonAdjacentWildcardParts,
                    newParserKind);
            }
            else
            { 
                return this; 
            }
        }

        /// <summary>
        /// Returns <see cref="ParseOptions"/> with the <see cref="AlwaysProduceEndToken"/> property set to specified value.
        /// </summary>
        public ParseOptions WithAlwaysProduceEndTokens(bool alwaysProduceEndTokens) =>
            With(alwaysProduceEndTokens: alwaysProduceEndTokens);

        /// <summary>
        /// Returns <see cref="ParseOptions"/> with the <see cref="AllowLiteralsWithLineBreaks"/> property set to specified value.
        /// </summary>
        public ParseOptions WithAllowLiteralsWithLineBreaks(bool allow) =>
            With(allowLiteralsWithLineBreaks: allow);

        /// <summary>
        /// Returns <see cref="ParseOptions"/> with the <see cref="AllowNonAdjacentWildcardParts"/> property set to specified value.
        /// </summary>
        public ParseOptions WithAllowNonAdjacentWildcardParts(bool allow) =>
            With(allowNonAdjacentWildcardParts: allow);

        /// <summary>
        /// Returns <see cref="ParseOptions"/> with the <see cref="ParserKind"/> property set to specified value.
        /// </summary>
        public ParseOptions WithParserKind(ParserKind kind) =>
            With(parserKind: kind);

        public bool Equals(ParseOptions other)
        {
            return EqualExceptForParseKind(other)
                && other.ParserKind == ParserKind;
        }

        public bool EqualExceptForParseKind(ParseOptions other)
        {
            return other.AlwaysProduceEndToken == this.AlwaysProduceEndToken
                && other.AllowLiteralsWithLineBreaks == this.AllowLiteralsWithLineBreaks
                && other.AllowNonAdjacentWildcardParts == this.AllowNonAdjacentWildcardParts;
        }

        /// <summary>
        /// Default parse options.
        /// </summary>
        public static ParseOptions Default = new ParseOptions(
            alwaysProduceEndTokens: true,
            allowLiteralsWithLineBreaks: false,
            allowNonAdjacentWildcardParts: false,
            parserKind: ParserKind.Default);
    }
}