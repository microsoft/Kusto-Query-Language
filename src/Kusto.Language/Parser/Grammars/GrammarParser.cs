using System;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
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
    // # e           hidden element
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
    public class GrammarParser
    {
        /// <summary>
        /// The text being parsed
        /// </summary>
        private readonly string _text;

        /// <summary>
        /// The current character position the parser is at.
        /// </summary>
        private int _position;

        private GrammarParser(string text, int position = 0)
        {
            _text = text;
            _position = position;
        }

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
            try
            {
                var parser = new GrammarParser(text);
                grammar = parser.ParseAlternationLevel();
                length = parser._position;
                return grammar != null;
            }
            catch (Exception)
            {
                grammar = null;
                length = 0;
                return false;
            }
        }

        /// <summary>
        /// Try to parse the grammar grammar.
        /// Returns true if the parse succeeds.
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
        /// <returns>The parsed grammar or null</returns>
        public static Grammar Parse(string text)
        {
            var parser = new GrammarParser(text);
            return parser.ParseAlternationLevel();
        }

        /// <summary>
        /// Parse an alternation or a sequence level element
        /// </summary>
        private Grammar ParseAlternationLevel()
        {
            var element = ParseSequenceLevel();

            List<Grammar> elements = null;
            while (ParseToken("|") != null)
            {
                if (elements == null)
                {
                    elements = new List<Grammar>();
                    elements.Add(element);
                }

                element = ParseSequenceLevel() ?? throw CreateExpectedGrammarException();
                elements.Add(element);
            }

            if (elements != null)
            {
                return new AlternationGrammar(elements.AsReadOnly());
            }
            else
            {
                return element;
            }
        }

        /// <summary>
        /// Parse a sequence or a tagged level element
        /// </summary>
        private Grammar ParseSequenceLevel()
        {
            List<Grammar> elements = null;

            var firstElement = ParseTaggedLevel();
            if (firstElement != null)
            {
                while (true)
                {
                    var nextElement = ParseTaggedLevel();
                    if (nextElement == null)
                        break;

                    if (elements == null)
                    {
                        elements = new List<Grammar>();
                        elements.Add(firstElement);
                    }

                    elements.Add(nextElement);
                }
            }

            if (elements != null)
            {
                return new SequenceGrammar(elements.AsReadOnly());
            }
            else
            {
                return firstElement;
            }
        }

        /// <summary>
        /// Parse a tagged element or a hidden level element
        /// </summary>
        private Grammar ParseTaggedLevel()
        {
            SkipTrivia();

            if (ScanIdentifierEquals() > 0)
            {
                var name = ParseIdentifier();
                ExpectToken("=");
                var tagged = ParseHiddenLevel() ?? throw CreateExpectedGrammarException();
                return new TaggedGrammar(name, tagged);
            }
            else if (ScanStringLiteralEquals() > 0)
            {
                var value = ParseStringLiteral();
                ExpectToken("=");
                var tagged = ParseHiddenLevel() ?? throw CreateExpectedGrammarException();
                return new TaggedGrammar(value, tagged);
            }
            else
            {
                return ParseHiddenLevel();
            }
        }

        /// <summary>
        /// Parse a hidden element or a bracketed level element
        /// </summary>
        private Grammar ParseHiddenLevel()
        {
            if (ParseToken("#") != null)
            {
                var element = ParseBracketedLevel() ?? throw CreateExpectedGrammarException();
                return new HiddenGrammar(element);
            }
            else
            {
                return ParseBracketedLevel();
            }
        }

        /// <summary>
        /// Parse a optional, zero-or-more, one-or-more or postfix level element
        /// </summary>
        private Grammar ParseBracketedLevel()
        {
            return ParseOptional()
                ?? ParseRepetition()
                ?? ParsePostfixLevel();
        }

        /// <summary>
        /// Parse an optional grammar element
        /// </summary>
        private Grammar ParseOptional()
        {
            if (ParseToken("[") != null)
            {
                var element = ParseAlternationLevel() ?? throw CreateExpectedGrammarException();
                ExpectToken("]");
                return new OptionalGrammar(element);
            }

            return null;
        }

        /// <summary>
        /// Parse a zero-or-more or one-or-more grammar element
        /// </summary>
        private Grammar ParseRepetition()
        {
            if (ParseToken("{") != null)
            {
                var element = ParseAlternationLevel() ?? throw CreateExpectedGrammarException();

                Grammar separator = null;
                bool allowTrailing = false;

                if (ParseToken(",") != null)
                {
                    separator = ParseTerm() ?? throw CreateExpectedTermException();
                    allowTrailing = ParseToken("~") != null;
                }

                ExpectToken("}");

                var postfix = ParseToken("*") ?? ParseToken("+");

                var zeroOrMore = postfix == null || postfix == "*";
                if (zeroOrMore)
                {
                    return (Grammar)new ZeroOrMoreGrammar(element, separator, allowTrailing);
                }
                else
                {
                    return (Grammar)new OneOrMoreGrammar(element, separator, allowTrailing);
                }
            }

            return null;
        }

        /// <summary>
        /// Parse a postfix required, optional, zero-or-more, one-or-more or a primary level element
        /// </summary>
        private Grammar ParsePostfixLevel()
        {
            var element = ParsePrimaryLevel();

            if (ParseToken("!") != null)
            {
                return new RequiredGrammar(element);
            }
            else if (ParseToken("?") != null)
            {
                return new OptionalGrammar(element);
            }
            else if (ParseToken("*") != null)
            {
                return new ZeroOrMoreGrammar(element);
            }
            else if (ParseToken("+") != null)
            {
                return new OneOrMoreGrammar(element);
            }
            else
            {
                return element;
            }
        }

        /// <summary>
        /// Parse a term, rule or grouped element
        /// </summary>
        private Grammar ParsePrimaryLevel()
        {
            return ParseTerm()
                ?? ParseRule()
                ?? ParseGrouped();
        }

        /// <summary>
        /// Parse an identifier or string literal token grammar element
        /// </summary>
        private Grammar ParseTerm()
        {
            if (ParseIdentifier() is string id)
            {
                return new TokenGrammar(id);
            }
            else if (ParseStringLiteral() is string value)
            {
                return new TokenGrammar(value);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Parse a rule element
        /// </summary>
        private Grammar ParseRule()
        {
            var name = ParseRuleName();
            if (name != null)
            {
                return new RuleGrammar(name);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Parse an parenthesized grammar element
        /// </summary>
        private Grammar ParseGrouped()
        {
            if (ParseToken("(") != null)
            {
                var element = ParseAlternationLevel();
                ExpectToken(")");
                return element;
            }

            return null;
        }

        /// <summary>
        /// Parse a token
        /// </summary>
        private string ParseToken(string token)
        {
            SkipTrivia();

            if (Matches(_text, _position, token))
            {
                _position += token.Length;
                return token;
            }

            return null;
        }

        /// <summary>
        /// Parse an expected token or throw an exception
        /// </summary>
        private void ExpectToken(string token)
        {
            if (ParseToken(token) == null)
            {
                throw CreateExpectedTokenException(token);
            }
        }

        /// <summary>
        /// Parse an identifier
        /// </summary>
        private string ParseIdentifier()
        {
            SkipTrivia();

            var start = _position;
            var idLen = ScanIdentifier();
            if (idLen > 0)
            {
                _position += idLen;
                return _text.Substring(start, idLen);
            }

            return null;
        }

        /// <summary>
        /// Parse a string literal
        /// </summary>
        private string ParseStringLiteral()
        {
            SkipTrivia();

            var start = _position;
            var literalLen = ScanStringLiteral();
            if (literalLen > 0)
            {
                _position += literalLen;
                var literal = _text.Substring(start, literalLen);
                return KustoFacts.GetStringLiteralValue(literal);
            }

            return null;
        }

        /// <summary>
        /// Parse a rule name
        /// </summary>
        private string ParseRuleName()
        {
            SkipTrivia();

            var start = _position;
            var ruleLen = ScanRuleName();
            if (ruleLen > 0)
            {
                _position += ruleLen;
                return _text.Substring(start + 1, ruleLen - 2);
            }

            return null;
        }

        /// <summary>
        /// Advances current position past any trivia
        /// </summary>
        private void SkipTrivia()
        {
            _position += ScanTrivia();
        }

        /// <summary>
        /// Scans an identifier followed by an equals.
        /// </summary>
        private int ScanIdentifierEquals(int offset = 0)
        {
            var idLen = ScanIdentifier(offset + ScanTrivia(offset));

            if (idLen > 0)
            {
                var equalLen = ScanToken("=", offset + idLen + ScanTrivia(offset));
                if (equalLen > 0)
                {
                    return idLen + equalLen;
                }
            }

            return 0;
        }

        /// <summary>
        /// Scans a string literal followed by an equals.
        /// </summary>
        private int ScanStringLiteralEquals(int offset = 0)
        {
            var idLen = ScanStringLiteral(offset + ScanTrivia(offset));

            if (idLen > 0)
            {
                var tokenLen = ScanToken("=", offset + idLen + ScanTrivia(offset));
                if (tokenLen > 0)
                {
                    return idLen + tokenLen;
                }
            }

            return 0;
        }

        /// <summary>
        /// Scans an identifier.
        /// </summary>
        private int ScanIdentifier(int offset = 0)
        {
            var len = 0;

            if (IsIdentifierFirstCharacter(Peek(offset)))
            {
                len++;

                while (IsIdentifierBodyCharacter(Peek(offset + len)))
                {
                    len++;
                }
            }

            return len;
        }

        /// <summary>
        /// Returns true if the character is legal for the first character in an identifier.
        /// </summary>
        private static bool IsIdentifierFirstCharacter(char ch)
        {
            return char.IsLetter(ch) || ch == '_';
        }

        /// <summary>
        /// Returns true if the character is legal for a character after the first in an identifier.
        /// </summary>
        private static bool IsIdentifierBodyCharacter(char ch)
        {
            return char.IsLetterOrDigit(ch) || ch == '_' || ch == '-';
        }

        /// <summary>
        /// Scans a string literal, returning the number of characters of the quoted string or zero.
        /// </summary>
        private int ScanStringLiteral(int offset = 0)
        {
            var len = 0;

            if (Peek(offset) == '\'')
            {
                len++;

                char ch;
                while ((ch = Peek(offset + len)) != '\'' && ch != '\0')
                {
                    len++;
                }

                if (Peek(offset + len) == '\'')
                {
                    len++;
                }
                else
                {
                    throw CreateStringLiteralMissingEndQuoteException();
                }
            }

            return len;
        }

        /// <summary>
        /// Scans a rule name, returning the number of characters in the bracketed name or zero.
        /// </summary>
        private int ScanRuleName(int offset = 0)
        {
            var len = 0;

            if (Peek(offset) == '<')
            {
                len++;

                char ch;
                while ((ch = Peek(offset + len)) != '>' && ch != '\0')
                {
                    len++;
                }

                if (Peek(offset + len) == '>')
                {
                    len++;
                }
                else
                {
                    throw CreateRuleNameMissingEndBracketException();
                }

                if (len < 3)
                {
                    throw CreateRuleNameRequiresAtLeastOneCharacter();
                }
            }

            return len;
        }

        /// <summary>
        /// Scans the specified token, returning the number of characters in the token or zero.
        /// </summary>
        private int ScanToken(string token, int offset = 0)
        {
            if (Matches(_text, _position + offset, token))
            {
                return token.Length;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Return the number of trivia characters that existing a sequence starting at the current position + offset
        /// </summary>
        private int ScanTrivia(int offset = 0)
        {
            var len = ScanSpaces(offset);

            if (char.IsWhiteSpace(Peek(offset + len)))
            {
                return len + ScanWhitespace(offset + len);
            }

            return len;
        }

        /// <summary>
        /// Returns the number of space characters that exist in a sequence starting at the current position + offset
        /// </summary>
        private int ScanSpaces(int offset = 0)
        {
            var start = offset;

            while (Peek(offset) == ' ')
            {
                offset++;
            }

            return offset - start;
        }

        /// <summary>
        /// Returns the number of whitespace characters exist in a sequence starting at the current position + offset
        /// </summary>
        private int ScanWhitespace(int offset = 0)
        {
            var start = offset;

            while (char.IsWhiteSpace(Peek(offset)))
            {
                offset++;
            }

            return offset - start;
        }

        /// <summary>
        /// Returns the character at the parsing position + the offset
        /// </summary>
        private char Peek(int offset = 0)
        {
            if (_position + offset < _text.Length)
            {
                return _text[_position + offset];
            }
            else
            {
                return '\0';
            }
        }

        /// <summary>
        /// True if the match text matches the characters starting at the position inside the specified text.
        /// </summary>
        private static bool Matches(string text, int start, string match)
        {
            return start + match.Length <= text.Length
                && match[0] == text[start]
                && (match.Length == 1 || string.Compare(text, start, match, 0, match.Length, StringComparison.Ordinal) == 0);
        }

        private Exception CreateExpectedGrammarException() =>
            new InvalidOperationException($"Expected grammar element at position {_position}");

        private Exception CreateExpectedTokenException(string token) =>
            new InvalidOperationException($"Expected token '{token}' at position {_position}");

        private Exception CreateExpectedTermException() =>
            new InvalidOperationException($"Expected identifier or string literal at position {_position}");

        private Exception CreateRuleNameMissingEndBracketException() =>
            new InvalidOperationException($"Rule name missing end bracket '>' at position {_position}");

        private Exception CreateRuleNameRequiresAtLeastOneCharacter() =>
            new InvalidOperationException($"Rule name requires at least one character at position {_position}");

        private Exception CreateStringLiteralMissingEndQuoteException() =>
            new InvalidOperationException($"String literal missing end quote at position {_position}");
    }
}