using System;
using System.Collections.Generic;
using System.Linq;
using Kusto.Language.Syntax;
using Kusto.Language.Utils;

namespace Kusto.Language.Parsing
{
    /// <summary>
    /// Parses <see cref="LexicalToken"/> for Kusto query language
    /// </summary>
    public class TokenParser
    {
        private readonly StringTable _stringTable;

        private TokenParser(StringTable stringTable)
        {
            _stringTable = stringTable;
        }

        private TokenParser()
            : this(new StringTable())
        {
        }

        /// <summary>
        /// A default instance of the <see cref="TokenParser"/>
        /// </summary>
        private static readonly TokenParser Default =
            new TokenParser(null);

        /// <summary>
        /// Parses the token at the starting position in the text.
        /// </summary>
        public static LexicalToken ParseToken(string text, int start = 0, ParseOptions options = null)
        {
            return Default.Parse(text, start, options ?? ParseOptions.Default);
        }

        /// <summary>
        /// Parses all the tokens in the text.
        /// </summary>
        public static LexicalToken[] ParseTokens(string text, ParseOptions options = null)
        {
            var tokens = new List<LexicalToken>();
            ParseTokens(text, tokens, options ?? ParseOptions.Default);
            return tokens.ToArray();
        }

        /// <summary>
        /// Parses all the tokens in the text.
        /// </summary>
        public static void ParseTokens(string text, List<LexicalToken> tokens, ParseOptions options = null)
        {
            options = options ?? ParseOptions.Default;
            var parser = new TokenParser();

            LexicalToken token;
            var pos = 0;
            do
            {
                token = parser.Parse(text, pos, options);
                if (token == null)
                    break;
                tokens.Add(token);
                pos += token.Length;
            }
            while (token.Kind != SyntaxKind.EndOfTextToken);
        }

        /// <summary>
        /// Parses the token at the starting offset in the text.
        /// </summary>
        private LexicalToken Parse(string text, int start, ParseOptions options)
        {
            LexicalToken tok;
            var pos = start;
            var trivia = ParseTrivia(text, pos);
            pos += trivia.Length;
      
            var ch = Peek(text, pos);
            char ch2;

            if (!TextFacts.IsLetterOrDigit(ch))
            {
                var info = GetPunctuationTokenInfo(text, pos);
                if (info != null)
                    return info.GetToken(trivia);

                if (IsStringLiteralStartQuote(ch))
                {
                    tok = ParseStringLiteral(text, pos, trivia);
                    if (tok != null)
                        return tok;
                }
                else if (ch == '@')
                {
                    ch2 = Peek(text, pos + 1);
                    if (IsStringLiteralStartQuote(ch2))
                    {
                        tok = ParseStringLiteral(text, pos, trivia);
                        if (tok != null)
                            return tok;
                    }
                }
                else if (ch == '#')
                {
                    var directiveEnd = TextFacts.GetLineEnd(text, pos);
                    return new LexicalToken(SyntaxKind.DirectiveToken, trivia, GetSubstring(text, pos, directiveEnd - pos));
                }
                else if (IsAtEnd(text, pos))
                {
                    if (trivia.Length > 0 || options.AlwaysProduceEndToken)
                        return new LexicalToken(SyntaxKind.EndOfTextToken, trivia, "");
                    return null;
                }
            }

            // keywords or other unhandled punctuation
            var keywordMatch = s_tokenInfoSubstringMap.GetLongestMatch(text, pos);
            if (keywordMatch.Key.Length > 0)
            {
                var nextChar = Peek(text, pos + keywordMatch.Key.Length);

                // goo literals?
                if (nextChar == '(')
                {
                    var gooKind = GetGooLiteralTokenKind(keywordMatch.Value.Kind);
                    if (gooKind != SyntaxKind.None)
                    {
                        var gooLen = ScanGoo(text, pos + keywordMatch.Key.Length, options);
                        if (gooLen > 0)
                        {
                            var gooText = GetSubstring(text, pos, keywordMatch.Key.Length + gooLen);

                            // validate the expected last character is correct
                            var dx = gooText.EndsWith(")")
                                ? null
                                : DiagnosticFacts.GetMissingText(")").WithLocationKind(DiagnosticLocationKind.RelativeEnd);

                            return new LexicalToken(gooKind, trivia, gooText, dx);
                        }
                    }
                }

                if (!IsIdentifierChar(nextChar))
                {
                    return keywordMatch.Value.GetToken(trivia);
                }
            }

            if (IsIdentifierStartChar(ch))
            {
                // check for identifier-like literal values
                var literalMatch = s_literalValueMap.GetLongestMatch(text, pos);
                if (literalMatch.Key.Length > 0 && !IsIdentifierChar(Peek(text, pos + literalMatch.Key.Length)))
                    return new LexicalToken(literalMatch.Value, trivia, literalMatch.Key);

                // it might be a uuid literal
                var rawGuidLen = ScanRawGuidLiteral(text, pos);
                if (rawGuidLen > 0)
                    return new LexicalToken(SyntaxKind.RawGuidLiteralToken, trivia, GetSubstring(text, pos, rawGuidLen));

                var idLen = ScanIdentifier(text, pos);

                // is this a hidden string?
                if (idLen == 1 && (ch == 'h' || ch == 'H'))
                {
                    ch2 = Peek(text, pos + 1);
                    if (IsStringLiteralStartQuote(ch2) || ch2 == '@')
                    {
                        tok = ParseStringLiteral(text, pos, trivia);
                        if (tok != null)
                            return tok;
                    }
                }

                return new LexicalToken(SyntaxKind.IdentifierToken, trivia, GetSubstring(text, pos, idLen));
            }
            else if (char.IsDigit(ch))
            {
                var rawGuidLen = ScanRawGuidLiteral(text, pos);
                if (rawGuidLen > 0)
                    return new LexicalToken(SyntaxKind.RawGuidLiteralToken, trivia, GetSubstring(text, pos, rawGuidLen));
                var realLen = ScanRealLiteral(text, pos);
                if (realLen >= 0)
                    return new LexicalToken(SyntaxKind.RealLiteralToken, trivia, GetSubstring(text, pos, realLen));
                var timeLen = ScanTimespanLiteral(text, pos);
                if (timeLen >= 0)
                    return new LexicalToken(SyntaxKind.TimespanLiteralToken, trivia, GetSubstring(text, pos, timeLen));
                var longLen = ScanLongLiteral(text, pos);
                if (longLen > 0)
                    return new LexicalToken(SyntaxKind.LongLiteralToken, trivia, GetSubstring(text, pos, longLen));
                var identifierLen = ScanIdentifier(text, pos);
                if (identifierLen >= 0)
                    return new LexicalToken(SyntaxKind.IdentifierToken, trivia, GetSubstring(text, pos, identifierLen));
            }

            // this character is not part of the language
            var subtext = GetSubstring(text, pos, 1);
            return new LexicalToken(SyntaxKind.BadToken, trivia, subtext, new[] { DiagnosticFacts.GetUnexpectedCharacter(subtext) });
        }

        /// <summary>
        /// Returns true if the character is the starting quote character
        /// of string literal.
        /// </summary>
        private static bool IsStringLiteralStartQuote(char ch)
        {
            return ch == '\'' 
                || ch == '"' 
                || ch == '`' 
                || ch == '~';
        }

        private LexicalToken ParseStringLiteral(string text, int start, string trivia)
        {
            // Note: this function repeats logic found in ScanStringLiteral in order to correctly identity when the end quote is not found
            // and apply the diagnostic without requiring ScanStringLiteral to return two values which would perform poorly when translated to javascript.

            var pos = start;
            Diagnostic dx = null;

            var ch = Peek(text, pos);
            if (ch == 'h' || ch == 'H')
            {
                pos++;
                ch = Peek(text, pos);
            }

            var isVerbatim = false;
            if (ch == '@')
            {
                isVerbatim = true;
                pos++;
                ch = Peek(text, pos);
            }

            if (ch == '\'')
            {
                pos++;

                var contentLength = ScanStringLiteralContent(text, pos, ch, isVerbatim);
                pos += contentLength;

                if (Peek(text, pos) == ch)
                {
                    pos++;
                }
                else
                {
                    dx = DiagnosticFacts.GetMissingText("'").WithLocationKind(DiagnosticLocationKind.RelativeEnd);
                }
            }
            else if (ch == '"')
            {
                pos++;

                var contentLength = ScanStringLiteralContent(text, pos, ch, isVerbatim);
                pos += contentLength;

                if (Peek(text, pos) == ch)
                {
                    pos++;
                }
                else
                {
                    dx = DiagnosticFacts.GetMissingText("\"").WithLocationKind(DiagnosticLocationKind.RelativeEnd);
                }
            }
            else if (Matches(text, pos, KustoFacts.MultiLineStringQuote))
            {
                pos += KustoFacts.MultiLineStringQuote.Length;
                pos += ScanMultiLineStringLiteralContent(text, pos, KustoFacts.MultiLineStringQuote);

                if (Matches(text, pos, KustoFacts.MultiLineStringQuote))
                {
                    pos += KustoFacts.MultiLineStringQuote.Length;
                }
                else
                {
                    dx = DiagnosticFacts.GetMissingText(KustoFacts.MultiLineStringQuote).WithLocationKind(DiagnosticLocationKind.RelativeEnd);
                }
            }
            else if (Matches(text, pos, KustoFacts.AlternateMultiLineStringQuote))
            {
                pos += KustoFacts.AlternateMultiLineStringQuote.Length;
                pos += ScanMultiLineStringLiteralContent(text, pos, KustoFacts.AlternateMultiLineStringQuote);

                if (Matches(text, pos, KustoFacts.AlternateMultiLineStringQuote))
                {
                    pos += KustoFacts.AlternateMultiLineStringQuote.Length;
                }
                else
                {
                    dx = DiagnosticFacts.GetMissingText(KustoFacts.AlternateMultiLineStringQuote).WithLocationKind(DiagnosticLocationKind.RelativeEnd);
                }
            }
            else
            {
                return null;
            }

            return new LexicalToken(SyntaxKind.StringLiteralToken, trivia, GetSubstring(text, start, pos - start), dx);
        }

        private static TokenInfo GetPunctuationTokenInfo(string text, int start)
        {
            int pos = start;
            var ch = Peek(text, pos);
            char ch2;

            switch (ch)
            {
                case '(':
                    return OpenParenTokenInfo;
                case ')':
                    return CloseParenTokenInfo;
                case '[':
                    return OpenBracketTokenInfo;
                case ']':
                    return CloseBracketTokenInfo;
                case '{':
                    return OpenBraceTokenInfo;
                case '}':
                    return CloseBraceTokenInfo;
                case '|':
                    return BarTokenInfo;
                case '.':
                    if (Peek(text, pos + 1) == '.')
                        return DotDotTokenInfo;
                    return DotTokenInfo;
                case '+':
                    return PlusTokenInfo;
                case '-':
                    return MinusTokenInfo;
                case '*':
                    return AsteriskTokenInfo;
                case '/':
                    return SlashTokenInfo;
                case '%':
                    return PercentTokenInfo;
                case '<':
                    ch2 = Peek(text, pos + 1);
                    if (ch2 == '=')
                        return LessThanOrEqualTokenInfo;
                    else if (ch2 == '|')
                        return LessThanBarTokenInfo;
                    else if (ch2 == '>')
                        return LessThanGreaterThanTokenInfo;
                    return LessThanTokenInfo;
                case '>':
                    if (Peek(text, pos + 1) == '=')
                        return GreaterThanOrEqualTokenInfo;
                    return GreaterThanTokenInfo;
                case '=':
                    ch2 = Peek(text, pos + 1);
                    if (ch2 == '=')
                        return EqualEqualTokenInfo;
                    else if (ch2 == '>')
                        return FatArrowTokenInfo;
                    else if (ch2 == '~')
                        return EqualTildeTokenInfo;
                    return EqualTokenInfo;
                case '!':
                    ch2 = Peek(text, pos + 1);
                    if (ch2 == '=')
                        return BangEqualTokenInfo;
                    else if (ch2 == '~')
                        return BangTildeTokenInfo;
                    break;
                case ':':
                    return ColonTokenInfo;
                case ';':
                    return SemicolonTokenInfo;
                case ',':
                    return CommaTokenInfo;
                case '@':
                    ch2 = Peek(text, pos + 1);
                    if (!IsStringLiteralStartQuote(ch2))
                    {
                        return AtTokenInfo;
                    }
                    break;
                case '?':
                    return QuestionTokenInfo;
            }

            return null;
        }

        private static readonly string[] s_booleanValues = new[]
        {
            "true", "True", "TRUE",
            "false", "False", "FALSE"
        };

        private static readonly SubstringMap<SyntaxKind> s_literalValueMap =
            new SubstringMap<SyntaxKind>(s_booleanValues.Select(v => new KeyValuePair<string, SyntaxKind>(v, SyntaxKind.BooleanLiteralToken)));

        /// <summary>
        /// Scans raw boolean literals: true or false.
        /// Does not scan bool(xxx).
        /// </summary>
        public static int ScanBooleanLiteral(string text, int start = 0)
        {
            var literalMatch = s_literalValueMap.GetLongestMatch(text, start);
            if (literalMatch.Key.Length > 0 
                && literalMatch.Value == SyntaxKind.BooleanLiteralToken
                && !IsIdentifierChar(Peek(text, start + literalMatch.Key.Length)))
            {
                return literalMatch.Key.Length;
            }

            return -1;
        }

        private static SyntaxKind GetGooLiteralTokenKind(SyntaxKind keywordKind)
        {
            switch (keywordKind)
            {
                case SyntaxKind.LongKeyword:
                case SyntaxKind.Int64Keyword:
                    return SyntaxKind.LongLiteralToken;
                case SyntaxKind.IntKeyword:
                case SyntaxKind.Int32Keyword:
                    return SyntaxKind.IntLiteralToken;
                case SyntaxKind.RealKeyword:
                case SyntaxKind.DoubleKeyword:
                    return SyntaxKind.RealLiteralToken;
                case SyntaxKind.DecimalKeyword:
                    return SyntaxKind.DecimalLiteralToken;
                case SyntaxKind.BoolKeyword:
                    return SyntaxKind.BooleanLiteralToken;
                case SyntaxKind.DateTimeKeyword:
                case SyntaxKind.DateKeyword:
                    return SyntaxKind.DateTimeLiteralToken;
                case SyntaxKind.TimeKeyword:
                case SyntaxKind.TimespanKeyword:
                    return SyntaxKind.TimespanLiteralToken;
                case SyntaxKind.GuidKeyword:
                    return SyntaxKind.GuidLiteralToken;
                default:
                    return SyntaxKind.None;
            }
        }

        /// <summary>
        /// Table of blank strings of varying sizes
        /// </summary>
        private static readonly string[] s_spaces = 
            System.Linq.Enumerable.Range(0, 32).Select(n => new string(' ', n)).ToArray();

        /// <summary>
        /// Parses the sequence of trivia in the string from the specified start position.
        /// </summary>
        public string ParseTrivia(string text, int start)
        {
            // first check for spaces only
            var len = ScanSpaces(text, start);
            int pos = start + len;

            // if next is something that should also be trivia, then use more extensive scan
            var ch = Peek(text, pos);
            if (TextFacts.IsWhitespace(ch))
            {
                // scan additional whitespace
                var wsLen = ScanWhitespace(text, pos);
                pos += wsLen;

                ch = Peek(text, pos);
                if (ch == '/' && Peek(text, pos + 1) == '/')
                {
                    var tlen = ScanTrivia(text, pos);

                    // don't reuse trivia with comments
                    return GetSubstring(text, start, len + wsLen + tlen, intern: false);
                }
                else
                {
                    return GetSubstring(text, start, len + wsLen);
                }
            }
            else if(ch == '/' && Peek(text, pos + 1) == '/')
            {
                var tlen = ScanTrivia(text, pos);

                // don't reuse trivia with comments
                return GetSubstring(text, start, len + tlen, intern: false);
            }
            else if (len == 0)
            {
                return "";
            }
            else if (len == 1)
            {
                return " ";
            }
            else if (len < s_spaces.Length)
            {
                // spaces only and we know these strings already
                return s_spaces[len];
            }
            else
            {
                return GetSubstring(text, start, len);
            }
        }

        private static int ScanSpaces(string text, int start)
        {
            int pos = start;

            while (Peek(text, pos) == ' ')
            {
                pos++;
            }

            return pos - start;
        }

        /// <summary>
        /// Returns the number of consecutive whitespace characters from the start position.
        /// </summary>
        public static int ScanWhitespace(string text, int start)
        {
            int pos = start;

            while (TextFacts.IsWhitespace(Peek(text, pos)))
            {
                pos++;
            }

            return pos - start;
        }

        /// <summary>
        /// Returns the number of consecutive trivia characters from the start position.
        /// </summary>
        public static int ScanTrivia(string text, int start)
        {
            var pos = start;
            char ch;

            while (!IsAtEnd(text, pos))
            {
                ch = Peek(text, pos);

                if (TextFacts.IsWhitespace(ch))
                {
                    pos++;
                    continue;
                }

                var commentLen = ScanComment(text, pos);
                if (commentLen > 0)
                {
                    pos += commentLen;
                    continue;
                }
                else
                {
                    break;
                }
            }

            return pos - start;
        }

        /// <summary>
        /// Returns the number of characters in a comment starting at the start position.
        /// </summary>
        public static int ScanComment(string text, int start)
        {
            if (Peek(text, start) == '/' 
                && Peek(text, start + 1) == '/')
            {
                var end = GetNextLineStart(text, start);
                return end - start;
            }

            return 0;
        }

        private static int GetNextLineStart(string text, int start)
        {
            var end = TextFacts.GetNextLineStart(text, start);
            return end >= 0 ? end : text.Length;
        }

        private static bool IsIdentifierStartChar(char ch)
        {
            return TextFacts.IsLetter(ch) || ch == '_' || ch == '$';
        }

        private static bool IsIdentifierChar(char ch)
        {
            return TextFacts.IsLetterOrDigit(ch) || ch == '_';
        }

        /// <summary>
        /// Returns the number of characters in the identifier or -1 if there is no
        /// identitifer at the starting position.
        /// </summary>
        public static int ScanIdentifier(string text, int start = 0)
        {
            int pos = start;

            var ch = Peek(text, pos);
            if (IsIdentifierStartChar(ch))
            {
                pos++;

                while (!IsAtEnd(text, pos))
                {
                    ch = Peek(text, pos);
                    if (IsIdentifierChar(ch))
                    {
                        pos++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else if (char.IsDigit(ch))
            {
                int len = ScanDigits(text, pos);
                if (len > 0)
                {
                    // must have at least one one letter or _ after digits
                    ch = Peek(text, pos + len);
                    if (TextFacts.IsLetter(ch) || ch == '_')
                    {
                        pos += len;

                        while (pos < text.Length)
                        {
                            ch = text[pos];
                            if (IsIdentifierChar(ch))
                            {
                                pos++;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return pos > start ? pos - start : -1;
        }

        private static int ScanDigits(string text, int start)
        {
            int pos = start;

            while (char.IsDigit(Peek(text, pos)))
            {
                pos++;
            }

            return pos > start ? pos - start : -1;
        }

        private static int ScanHexIntegerLiteral(string text, int start)
        {
            int pos = start;

            if (Peek(text, start) == '0' 
                && (Peek(text, start + 1) == 'x' || Peek(text, start + 1) == 'X'))
            {
                pos += 2;

                if (!TextFacts.IsHexDigit(Peek(text, pos)))
                    return -1;

                pos++;

                while (TextFacts.IsHexDigit(Peek(text, pos)))
                {
                    pos++;
                }
            }

            if (IsIdentifierChar(Peek(text, pos)))
                return -1;

            return pos > start ? pos - start : -1;
        }

        /// <summary>
        /// Scans raw long literal values. 
        /// Does not scan long(xxx).
        /// </summary>
        public static int ScanLongLiteral(string text, int start = 0)
        {
            var hexLen = ScanHexIntegerLiteral(text, start);
            if (hexLen > 0)
                return hexLen;
            var len = ScanDigits(text, start);
            if (len > 0 && !IsIdentifierChar(Peek(text, start + len)))
                return len;
            return -1;
        }

        private static int ScanExponent(string text, int start)
        {
            int pos = start;
            var expch = Peek(text, pos);
            if (expch == 'e' || expch == 'E')
            {
                pos++;
                var signch = Peek(text, pos);
                if (signch == '+' || signch == '-')
                    pos++;
                var exponentLen = ScanDigits(text, pos);
                if (exponentLen <= 0)
                    return -1;
                pos += exponentLen;
            }

            return pos > start ? pos - start : -1;
        }

        /// <summary>
        /// Scans raw real literals: 1.0, etc.
        /// Does not scan real(xxx).
        /// </summary>
        public static int ScanRealLiteral(string text, int start = 0)
        {
            var digitLen = ScanDigits(text, start);
            if (digitLen <= 0)
                return -1;
            var pos = start + digitLen;
            if (Peek(text, pos) == '.' && (Peek(text, pos + 1) != '.' || Peek(text, pos + 2) == '.'))
            {
                pos++;
                var fractionLen = ScanDigits(text, pos);
                if (fractionLen > 0)
                    pos += fractionLen;

                var expLen = ScanExponent(text, pos);
                if (expLen > 0)
                    pos += expLen;
            }
            else
            {
                var expLen = ScanExponent(text, pos);
                if (expLen <= 0)
                    return -1;
                pos += expLen;
            }

            if (IsIdentifierChar(Peek(text, pos)))
                return -1;

            return pos > start ? pos - start : -1;
        }

        /// <summary>
        /// Scans raw timespan literals, 1day, etc.
        /// Does not scan timespan(xxx).
        /// </summary>
        public static int ScanTimespanLiteral(string text, int start = 0)
        {
            var numberLen = ScanDigits(text, start);
            if (numberLen <= 0)
                return -1;
            if (Peek(text, start + numberLen) == '.')
            {
                var fractionLen = ScanDigits(text, start + numberLen + 1);
                if (fractionLen >= 0)
                {
                    numberLen += fractionLen + 1;
                }
            }
            var suffixMatch = TimespanSuffixMap.GetLongestMatch(text, start + numberLen);
            if (suffixMatch.Key.Length <= 0)
                return -1;
            var len = numberLen + suffixMatch.Key.Length;
            if (IsIdentifierChar(Peek(text, start + len)))
                return -1;
            return len;
        }

        private static readonly string[] TimespanSuffixes = new[]
        {
            "m", "min", "minute", "minutes",
            "s", "sec", "second", "seconds",
            "d", "day", "days",
            "h", "hr", "hrs", "hour", "hours",
            "ms", "milli", "millis", "millisec", "millisecond", "milliseconds",
            "micro", "micros", "microsec", "microsecond", "microseconds",
            "nano", "nanos", "nanosec", "nanosecond", "nanoseconds",
            "tick", "ticks"
        };

        private static readonly SubstringMap<bool> TimespanSuffixMap =
            new SubstringMap<bool>(TimespanSuffixes.Select(s => new KeyValuePair<string, bool>(s, true)));

        /// <summary>
        /// Returns the number of characters that are part of the string literal, or -1 if the text
        /// at the starting position is not a string literal.
        /// </summary>
        public static int ScanStringLiteral(string text, int start = 0, bool failWhenMissingEndQuote = false)
        {
            var pos = start;

            var ch = Peek(text, pos);
            if (ch == 'h' || ch == 'H')
            {
                pos++;
                ch = Peek(text, pos);
            }

            var isVerbatim = false;
            if (ch == '@')
            {
                isVerbatim = true;
                pos++;
                ch = Peek(text, pos);
            }

            if (ch == '\'' || ch == '"')
            {
                pos++;

                var contentLength = ScanStringLiteralContent(text, pos, ch, isVerbatim);
                pos += contentLength;

                if (Peek(text, pos) == ch)
                {
                    pos++;
                }
                else if (failWhenMissingEndQuote)
                {
                    return -1;
                }
            }
            else if (Matches(text, pos, KustoFacts.MultiLineStringQuote))
            {
                pos += KustoFacts.MultiLineStringQuote.Length;
                pos += ScanMultiLineStringLiteralContent(text, pos, KustoFacts.MultiLineStringQuote);

                if (Matches(text, pos, KustoFacts.MultiLineStringQuote))
                {
                    pos += KustoFacts.MultiLineStringQuote.Length;
                }
                else if (failWhenMissingEndQuote)
                {
                    return -1;
                }
            }
            else if (Matches(text, pos, KustoFacts.AlternateMultiLineStringQuote))
            {
                pos += KustoFacts.AlternateMultiLineStringQuote.Length;
                pos += ScanMultiLineStringLiteralContent(text, pos, KustoFacts.AlternateMultiLineStringQuote);

                if (Matches(text, pos, KustoFacts.AlternateMultiLineStringQuote))
                {
                    pos += KustoFacts.AlternateMultiLineStringQuote.Length;
                }
                else if (failWhenMissingEndQuote)
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }

            return pos > start ? pos - start : -1;
        }

        private static int ScanStringLiteralContent(string text, int start, char quote, bool isVerbatim)
        {
            int pos = start;

            char ch;
            while (!IsAtEnd(text, pos))
            {
                ch = Peek(text, pos);

                if (ch == quote && isVerbatim && Peek(text, pos + 1) == quote)
                {
                    pos += 2;
                    continue;
                }
                else if (ch == '\\' && !isVerbatim)
                {
                    var escapeLen = ScanStringEscape(text, pos);
                    if (escapeLen > 0)
                    {
                        pos += escapeLen;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                else if (ch == quote
                    || ch == '\r'       // string only sees \r & \n as line breaks
                    || ch == '\n')
                {
                    break;
                }
                else
                {
                    pos++;
                }
            }

            return pos - start;
        }

        private static int ScanMultiLineStringLiteralContent(string text, int start, string endQuote)
        {
            var pos = start;

            while (pos < text.Length && !Matches(text, pos, endQuote))
            {
                pos++;
            }

            return pos - start;
        }

        private static int ScanStringEscape(string text, int start)
        {
            var ch = Peek(text, start);
            if (ch == '\\')
            {
                ch = Peek(text, start + 1);
                switch (ch)
                {
                    case '\\':
                    case '\'':
                    case '"':
                    case 'a':
                    case 'b':
                    case 'f':
                    case 'n':
                    case 'r':
                    case 't':
                    case 'v':
                        return 2;
                    case 'u':
                        var len = ScanFourHexDigits(text, start + 2);
                        if (len > 0)
                            return len + 2;
                        return -1;
                    case 'U':
                        len = ScanEightHexDigits(text, start + 2);
                        if (len > 0)
                            return len + 2;
                        return -1;
                    case 'x':
                        len = ScanTwoHexDigits(text, start + 2);
                        if (len > 0)
                            return len + 2;
                        return -1;
                    default:
                        len = ScanOctalCode(text, start + 1);
                        if (len > 0)
                            return len + 1;
                        break;
                }
            }

            // not an escape
            return -1;
        }

        private static int ScanTwoHexDigits(string text, int start)
        {
            if (start + 1 < text.Length
                && TextFacts.IsHexDigit(text[start])
                && TextFacts.IsHexDigit(text[start + 1]))
            {
                return 2;
            }
            else
            {
                return -1;
            }
        }

        private static int ScanFourHexDigits(string text, int start)
        {
            if (start + 3 < text.Length
                && TextFacts.IsHexDigit(text[start])
                && TextFacts.IsHexDigit(text[start + 1])
                && TextFacts.IsHexDigit(text[start + 2])
                && TextFacts.IsHexDigit(text[start + 3]))
            {
                return 4;
            }
            else
            {
                return -1;
            }
        }

        private static int ScanEightHexDigits(string text, int start)
        {
            if (start + 7 < text.Length
                && TextFacts.IsHexDigit(text[start])
                && TextFacts.IsHexDigit(text[start + 1])
                && TextFacts.IsHexDigit(text[start + 2])
                && TextFacts.IsHexDigit(text[start + 3])
                && TextFacts.IsHexDigit(text[start + 4])
                && TextFacts.IsHexDigit(text[start + 5])
                && TextFacts.IsHexDigit(text[start + 6])
                && TextFacts.IsHexDigit(text[start + 7]))
            {
                return 8;
            }
            else
            {
                return -1;
            }
        }

        private static int ScanTwelveHexDigits(string text, int start)
        {
            if (start + 11 < text.Length
                && TextFacts.IsHexDigit(text[start])
                && TextFacts.IsHexDigit(text[start + 1])
                && TextFacts.IsHexDigit(text[start + 2])
                && TextFacts.IsHexDigit(text[start + 3])
                && TextFacts.IsHexDigit(text[start + 4])
                && TextFacts.IsHexDigit(text[start + 5])
                && TextFacts.IsHexDigit(text[start + 6])
                && TextFacts.IsHexDigit(text[start + 7])
                && TextFacts.IsHexDigit(text[start + 8])
                && TextFacts.IsHexDigit(text[start + 9])
                && TextFacts.IsHexDigit(text[start + 10])
                && TextFacts.IsHexDigit(text[start + 11]))
            {
                return 12;
            }
            else
            {
                return -1;
            }
        }

        private static int ScanOctalCode(string text, int start)
        {
            var ch1 = Peek(text, start);
            if (ch1 >= '0' && ch1 <= '7')
            {
                var ch2 = Peek(text, start + 1);
                if (ch2 >= '0' && ch2 <= '7')
                {
                    var ch3 = Peek(text, start + 2);
                    if (ch3 >= '0' && ch3 <= '7' && ch1 <= '3')
                    {
                        return 3;
                    }
                    else
                    {
                        return 2;
                    }
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Scans the content of a parenthesized literal
        /// </summary>
        private static int ScanGoo(string text, int start, ParseOptions options)
        {
            var pos = start;

            if (Peek(text, pos) == '(')
            {
                pos++;

                char ch;
                while (pos < text.Length 
                    && (ch = text[pos]) != ')'
                    // better intellisense if we stop the insanity at like break (no literal spans multiple lines since dynamic is now an expression)
                    // probably can do even better for numeric literals too since we know the domain fully.
                    && (options.AllowLiteralsWithLineBreaks || !TextFacts.IsLineBreakStart(ch)))  
                {
                    pos++;
                }

                if (Peek(text, pos) == ')')
                    pos++;
            }

            return pos > start ? pos - start : -1;
        }

        /// <summary>
        /// Scans raw guid literals only.
        /// Does not scan guid(xxx) literals.
        /// </summary>
        public int ScanRawGuidLiteral(string text, int start)
        {
            if (start + 35 < text.Length
                && ScanEightHexDigits(text, start) == 8
                && text[start + 8] == '-'
                && ScanFourHexDigits(text, start + 9) == 4
                && text[start + 13] == '-'
                && ScanFourHexDigits(text, start + 14) == 4
                && text[start + 18] == '-'
                && ScanFourHexDigits(text, start + 19) == 4
                && text[start + 23] == '-'
                && ScanTwelveHexDigits(text, start + 24) == 12)
            {
                return 36;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Returns the number of characters in the client parameter of -1 if there is no client parameter.
        /// This is not a normal token, but a special case for the client editor.
        /// </summary>
        public static int ScanClientParameter(
            string text, int start = 0)
        {
            return ScanClientParameter(text, start, out _, out _, out _, out _);
        }

        /// <summary>
        /// Returns the number of characters in the client parameter of -1 if there is no client parameter.
        /// This is not a normal token, but a special case for the client editor.
        /// </summary>
        public static int ScanClientParameter(
            string text, 
            int start,
            out int nameStart,
            out int nameLength,
            out int indexStart, 
            out int indexLength)
        {
            nameStart = -1;
            nameLength = 0;
            indexStart = 0;
            indexLength = 0;

            if (Peek(text, start) == '{')
            {
                nameStart = start + 1;
                nameLength = ScanIdentifier(text, nameStart);
                if (nameLength > 0)
                {
                    if (Peek(text, nameStart + nameLength) == '}')
                    {
                        return nameLength + 2;
                    }
                    else if (Peek(text, nameStart + nameLength) == '[')
                    {
                        indexStart = nameStart + nameLength + 1;
                        var indexEnd = indexStart;

                        // If it's a negative number, skip the negative sign
                        if (Peek(text, indexEnd) == '-')
                            indexEnd++;

                        var litLen = ScanLongLiteral(text, indexEnd);
                        if (litLen <= 0)
                            return -1;

                        indexEnd += litLen;

                        if (Peek(text, indexEnd) == ']' 
                            && Peek(text, indexEnd + 1) == '}')
                        {
                            indexLength = indexEnd - indexStart;
                            return (indexEnd + 2) - start;
                        }
                    }
                }
            }

            return -1;
        }

        private static char Peek(string text, int position)
        {
            if (position >= 0 && position < text.Length)
            {
                return text[position];
            }
            else
            {
                return '\0';
            }
        }

        private static bool IsAtEnd(string text, int position)
        {
            return position >= text.Length;
        }

        private static bool Matches(string text, int start, string match)
        {
            if (start < 0 || start + match.Length > text.Length)
                return false;

            switch (match.Length)
            {
                case 1:
                    return match[0] == text[start];
                case 2:
                    return match[0] == text[start]
                        && match[1] == text[start + 1];
                case 3:
                    return match[0] == text[start]
                        && match[1] == text[start + 1]
                        && match[2] == text[start + 2];
                default:
                    return match[0] == text[start]
                    && string.Compare(text, start, match, 0, match.Length, StringComparison.Ordinal) == 0;
            }
        }

        private string GetSubstring(string text, int start, int len, bool intern = true)
        {
            if (_stringTable != null && intern)
            {
                return _stringTable.Add(text, start, len);
            }
            else
            {
                return text.Substring(start, len);
            }
        }

        #region resusable tokens

        private class TokenInfo
        {
            public readonly SyntaxKind Kind;
            public readonly string Text;
            public readonly LexicalToken ZeroTriviaToken;
            public readonly LexicalToken SingleWhitespaceToken;

            public TokenInfo(SyntaxKind kind)
            {
                this.Kind = kind;
                this.Text = kind.GetText();
                this.ZeroTriviaToken = new LexicalToken(kind, "", this.Text);
                this.SingleWhitespaceToken = new LexicalToken(kind, " ", this.Text);
            }

            public LexicalToken GetToken(string trivia)
            {
                if (trivia.Length == 0)
                    return this.ZeroTriviaToken;
                else if (trivia == " ")
                    return this.SingleWhitespaceToken;
                else
                    return new LexicalToken(this.Kind, trivia, this.Text);
            }
        }

        private static readonly Dictionary<SyntaxKind, TokenInfo> s_kindToTokenInfoMap =
            SyntaxFacts.GetKindsWithFixedText().ToDictionary(k => k, k => new TokenInfo(k));

        private static readonly SubstringMap<TokenInfo> s_tokenInfoSubstringMap =
            new SubstringMap<TokenInfo>(s_kindToTokenInfoMap.Select(kvp => 
                new KeyValuePair<string, TokenInfo>(kvp.Key.GetText(), kvp.Value)));

        private static readonly TokenInfo OpenParenTokenInfo = s_kindToTokenInfoMap[SyntaxKind.OpenParenToken];
        private static readonly TokenInfo CloseParenTokenInfo = s_kindToTokenInfoMap[SyntaxKind.CloseParenToken];
        private static readonly TokenInfo OpenBracketTokenInfo = s_kindToTokenInfoMap[SyntaxKind.OpenBracketToken];
        private static readonly TokenInfo CloseBracketTokenInfo = s_kindToTokenInfoMap[SyntaxKind.CloseBracketToken];
        private static readonly TokenInfo OpenBraceTokenInfo = s_kindToTokenInfoMap[SyntaxKind.OpenBraceToken];
        private static readonly TokenInfo CloseBraceTokenInfo = s_kindToTokenInfoMap[SyntaxKind.CloseBraceToken];
        private static readonly TokenInfo BarTokenInfo = s_kindToTokenInfoMap[SyntaxKind.BarToken];
        private static readonly TokenInfo DotDotTokenInfo = s_kindToTokenInfoMap[SyntaxKind.DotDotToken];
        private static readonly TokenInfo DotTokenInfo = s_kindToTokenInfoMap[SyntaxKind.DotToken];
        private static readonly TokenInfo PlusTokenInfo = s_kindToTokenInfoMap[SyntaxKind.PlusToken];
        private static readonly TokenInfo MinusTokenInfo = s_kindToTokenInfoMap[SyntaxKind.MinusToken];
        private static readonly TokenInfo AsteriskTokenInfo = s_kindToTokenInfoMap[SyntaxKind.AsteriskToken];
        private static readonly TokenInfo SlashTokenInfo = s_kindToTokenInfoMap[SyntaxKind.SlashToken];
        private static readonly TokenInfo PercentTokenInfo = s_kindToTokenInfoMap[SyntaxKind.PercentToken];
        private static readonly TokenInfo LessThanOrEqualTokenInfo = s_kindToTokenInfoMap[SyntaxKind.LessThanOrEqualToken];
        private static readonly TokenInfo LessThanBarTokenInfo = s_kindToTokenInfoMap[SyntaxKind.LessThanBarToken];
        private static readonly TokenInfo LessThanGreaterThanTokenInfo = s_kindToTokenInfoMap[SyntaxKind.LessThanGreaterThanToken];
        private static readonly TokenInfo LessThanTokenInfo = s_kindToTokenInfoMap[SyntaxKind.LessThanToken];
        private static readonly TokenInfo GreaterThanOrEqualTokenInfo = s_kindToTokenInfoMap[SyntaxKind.GreaterThanOrEqualToken];
        private static readonly TokenInfo GreaterThanTokenInfo = s_kindToTokenInfoMap[SyntaxKind.GreaterThanToken];
        private static readonly TokenInfo EqualEqualTokenInfo = s_kindToTokenInfoMap[SyntaxKind.EqualEqualToken];
        private static readonly TokenInfo FatArrowTokenInfo = s_kindToTokenInfoMap[SyntaxKind.FatArrowToken];
        private static readonly TokenInfo EqualTildeTokenInfo = s_kindToTokenInfoMap[SyntaxKind.EqualTildeToken];
        private static readonly TokenInfo EqualTokenInfo = s_kindToTokenInfoMap[SyntaxKind.EqualToken];
        private static readonly TokenInfo BangEqualTokenInfo = s_kindToTokenInfoMap[SyntaxKind.BangEqualToken];
        private static readonly TokenInfo BangTildeTokenInfo = s_kindToTokenInfoMap[SyntaxKind.BangTildeToken];
        private static readonly TokenInfo ColonTokenInfo = s_kindToTokenInfoMap[SyntaxKind.ColonToken];
        private static readonly TokenInfo SemicolonTokenInfo = s_kindToTokenInfoMap[SyntaxKind.SemicolonToken];
        private static readonly TokenInfo CommaTokenInfo = s_kindToTokenInfoMap[SyntaxKind.CommaToken];
        private static readonly TokenInfo AtTokenInfo = s_kindToTokenInfoMap[SyntaxKind.AtToken];
        private static readonly TokenInfo QuestionTokenInfo = s_kindToTokenInfoMap[SyntaxKind.QuestionToken];

        #endregion
    }
}