using System;
using System.Collections;
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
        public static LexicalToken ParseToken(string text, int start = 0, bool alwaysProduceEndToken = false)
        {
            return Default.Parse(text, start, alwaysProduceEndToken);
        }

        /// <summary>
        /// Parses all the tokens in the text.
        /// </summary>
        public static LexicalToken[] ParseTokens(string text, bool alwaysProduceEndToken = false)
        {
            var tokens = new List<LexicalToken>();
            ParseTokens(text, tokens, alwaysProduceEndToken);
            return tokens.ToArray();
        }

        /// <summary>
        /// Parses all the tokens in the text.
        /// </summary>
        public static void ParseTokens(string text, List<LexicalToken> tokens, bool alwaysProduceEndToken = false)
        {
            var parser = new TokenParser();

            LexicalToken token;
            var pos = 0;
            do
            {
                token = parser.Parse(text, pos, alwaysProduceEndToken);
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
        private LexicalToken Parse(string text, int start, bool alwaysProduceEndToken)
        {
            LexicalToken tok;

            var pos = start;
            var trivia = ParseTrivia(text, pos);
            pos += trivia.Length;
      
            var ch = Peek(text, pos);
            char ch2;

            if (!char.IsLetterOrDigit(ch))
            {
                var info = GetPunctuationTokenInfo(text, pos);
                if (info != null)
                    return info.GetToken(trivia);

                switch (ch)
                {
                    case '\'':
                    case '"':
                    case '`':
                        tok = ParseStringLiteral(text, pos, trivia);
                        if (tok != null)
                            return tok;
                        break;
                    case '@':
                        ch2 = Peek(text, pos + 1);
                        if (ch2 == '\'' || ch2 == '"' || ch2 == '`')
                        {
                            tok = ParseStringLiteral(text, pos, trivia);
                            if (tok != null)
                                return tok;
                        }
                        break;
                    case '#':
                        var directiveEnd = GetNextLineStart(text, pos);
                        return new LexicalToken(SyntaxKind.DirectiveToken, trivia, GetSubstring(text, pos, directiveEnd - pos));
                    case '\0':
                        if (trivia.Length > 0 || alwaysProduceEndToken)
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
                        var gooLen = ScanGoo(text, pos + keywordMatch.Key.Length);
                        if (gooLen > 0)
                        {
                            return EndCheckedToken(pos, gooKind, trivia, GetSubstring(text, pos, keywordMatch.Key.Length + gooLen), ")");
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

                var len = ScanIdentifier(text, pos);

                // is this a hidden string?
                if (len == 1 && (ch == 'h' || ch == 'H'))
                {
                    ch2 = Peek(text, pos + 1);
                    if (ch2 == '\'' || ch2 == '"' || ch2 == '@' || ch2 == '`')
                    {
                        tok = ParseStringLiteral(text, pos, trivia);
                        if (tok != null)
                            return tok;
                    }
                }

                return new LexicalToken(SyntaxKind.IdentifierToken, trivia, GetSubstring(text, pos, len));
            }
            else if (char.IsDigit(ch))
            {
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

        private LexicalToken ParseStringLiteral(string text, int start, string trivia)
        {
            var len = ScanStringLiteral(text, start);
            if (len > 0)
            {
                var endQuote = GetStringLiteralQuote(text, start);
                return EndCheckedToken(start, SyntaxKind.StringLiteralToken, trivia, GetSubstring(text, start, len), endQuote);
            }

            return null;
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
                    if (ch2 != '\'' && ch2 != '"' && ch2 != '`')
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

        private static SyntaxKind GetGooLiteralTokenKind(SyntaxKind keywordKind)
        {
            switch (keywordKind)
            {
                case SyntaxKind.LongKeyword:
                    return SyntaxKind.LongLiteralToken;
                case SyntaxKind.IntKeyword:
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

        private static readonly SubstringMap<SyntaxKind> s_literalValueMap =
            new SubstringMap<SyntaxKind>(s_booleanValues.Select(v => new KeyValuePair<string, SyntaxKind>(v, SyntaxKind.BooleanLiteralToken)));

        /// <summary>
        /// Table of blank strings of varying sizes
        /// </summary>
        private static readonly string[] s_spaces = 
            System.Linq.Enumerable.Range(0, 32).Select(n => new string(' ', n)).ToArray();

        public string ParseTrivia(string text, int start)
        {
            // first check for spaces only
            var len = ScanSpaces(text, start);
            int pos = start + len;

            // if next is something that should also be trivia, then use more extensive scan
            var ch = Peek(text, pos);
            if (char.IsWhiteSpace(ch))
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

        private static int ScanWhitespace(string text, int start)
        {
            int pos = start;

            while (char.IsWhiteSpace(Peek(text, pos)))
            {
                pos++;
            }

            return pos - start;
        }

        public static int ScanTrivia(string text, int start)
        {
            var pos = start;
            char ch;

            while ((ch = Peek(text, pos)) != '\0')
            {
                if (char.IsWhiteSpace(ch))
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
            return char.IsLetter(ch) || ch == '_' || ch == '$';
        }

        private static bool IsIdentifierChar(char ch)
        {
            return char.IsLetterOrDigit(ch) || ch == '_';
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

                while ((ch = Peek(text, pos)) != '\0')
                {
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
                    if (char.IsLetter(ch) || ch == '_')
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

        private static int ScanLongLiteral(string text, int start)
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

        private static int ScanRealLiteral(string text, int start)
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

        private static int ScanTimespanLiteral(string text, int start)
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
        public static int ScanStringLiteral(string text, int start = 0)
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
            }
            else if (ch == '`' && Peek(text, pos + 1) == '`' && Peek(text, pos + 2) == '`')
            {
                pos += 3;
                
                while (pos < text.Length &&
                    !(text[pos] == '`' && Peek(text, pos + 1) == '`' && Peek(text, pos + 2) == '`'))
                {
                    pos++;
                }

                // end ```
                if (Peek(text, pos) == '`')
                    pos++;
                if (Peek(text, pos) == '`')
                    pos++;
                if (Peek(text, pos) == '`')
                    pos++;
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
            while ((ch = Peek(text, pos)) != '\0')
            {
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
                    || ch == '\r'
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
            if (start < text.Length + 2
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
            if (start < text.Length + 4
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
            if (start < text.Length + 8
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

        private static string GetStringLiteralQuote(string text, int start)
        {
            var pos = start;

            var ch = Peek(text, pos);
            if (ch == 'h' || ch == 'H')
            {
                pos++;
                ch = Peek(text, pos);
            }

            if (ch == '@')
            {
                pos++;
                ch = Peek(text, pos);
            }

            if (ch == '\'')
            {
                return "'";
            }
            else if (ch == '"')
            {
                return "\"";
            }
            else if (ch == '`')
            {
                return "```";
            }
            else
            {
                return null;
            }
        }

        private static int ScanGoo(string text, int start)
        {
            var pos = start;

            if (Peek(text, pos) == '(')
            {
                pos++;

                while (pos < text.Length && text[pos] != ')')
                {
                    pos++;
                }

                if (Peek(text, pos) == ')')
                    pos++;
            }

            return pos > start ? pos - start : -1;
        }

        /// <summary>
        /// Returns the number of characters in the client parameter of -1 if
        /// there is not client parameter at the starting position.
        /// </summary>
        public static int ScanClientParameter(string text, int start = 0)
        {
            if (Peek(text, start) == '{')
            {
                var idLen = ScanIdentifier(text, start + 1);
                if (idLen > 0)
                {
                    if (Peek(text, start + idLen + 1) == '}')
                        return idLen + 2;
                }
            }

            return -1;
        }

        private static char Peek(string text, int position)
        {
            if (position < text.Length)
            {
                return text[position];
            }
            else
            {
                return '\0';
            }
        }

        private static bool Matches(string text, int start, string match)
        {
            return start + match.Length < text.Length
                && match[0] == text[start]
                && string.Compare(text, start, match, 0, match.Length, StringComparison.Ordinal) == 0;
        }

        private static LexicalToken EndCheckedToken(int start, SyntaxKind kind, string trivia, string text, string expectedEndChars)
        {
            // validate the expected last character is correct
            var dx = text.EndsWith(expectedEndChars)
                ? null
                : new Diagnostic[] { DiagnosticFacts.GetMissingText(expectedEndChars) };

            return new LexicalToken(kind, trivia, text, dx);
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