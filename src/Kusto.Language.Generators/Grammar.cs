// <#+
#if !T4
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kusto.Language.Generators
{
#endif

    #region Grammar Parser

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
                return _text.Substring(start + 1, literalLen - 2);
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

    #endregion

    #region Grammar Nodes

    /// <summary>
    /// The base definition of a grammar tree nodes.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{DebugText}")]
    public abstract class Grammar
    {
        private readonly int _hashCode;

        protected Grammar(int hashCode)
        {
            _hashCode = hashCode;
        }

        public override int GetHashCode() => _hashCode;

        protected static int ComputeHashCode(IReadOnlyList<Grammar> items)
        {
            int hc = 0;

            for (int i = 0; i < items.Count; i++)
            {
                hc += items[i].GetHashCode();
            }

            return hc;
        }

        public abstract TResult Accept<TResult>(GrammarVisitor<TResult> visitor);

        /// <summary>
        /// Returns true if the grammar rule can be considered optional.
        /// </summary>
        public bool IsOptional
        {
            get
            {
                switch (this)
                {
                    case OptionalGrammar _:
                    case ZeroOrMoreGrammar _:
                        return true;
                    case TaggedGrammar tg:
                        return tg.Tagged.IsOptional;
                    case HiddenGrammar hg:
                        return hg.Hidden.IsOptional;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Apply the mapping function to all the grammar nodes in the tree.
        /// </summary>
        public Grammar Apply(Func<Grammar, Grammar> mapper)
        {
            var treeMapper = new GrammarTreeMapper(mapper);
            return this.Accept(treeMapper);
        }

        /// <summary>
        /// Performs a deep copy of the grammar tree
        /// </summary>
        public Grammar Clone()
        {
            switch (this)
            {
                case TokenGrammar tg:
                    return new TokenGrammar(tg.TokenText);
                case RuleGrammar rg:
                    return new RuleGrammar(rg.RuleName);
                default:
                    return Apply(g => {
                        switch (g)
                        {
                            case TokenGrammar t:
                                return new TokenGrammar(t.TokenText);
                            case RuleGrammar r:
                                return new RuleGrammar(r.RuleName);
                            default:
                                return g;
                        }
                    });
            }
        }

        public bool Any(Func<Grammar, bool> predicate)
        {
            return First(predicate) != null;
        }

        public Grammar First(Func<Grammar, bool> predicate)
        {
            if (predicate(this))
                return this;

            switch (this)
            {
                case SequenceGrammar sg:
                    foreach (var step in sg.Steps)
                    {
                        var result = step.First(predicate);
                        if (result != null)
                            return result;
                    }
                    return null;
                case AlternationGrammar ag:
                    foreach (var alt in ag.Alternatives)
                    {
                        var result = alt.First(predicate);
                        if (result != null)
                            return result;
                    }
                    return null;
                case OptionalGrammar og:
                    return og.Optioned.First(predicate);
                case RequiredGrammar rg:
                    return rg.Required.First(predicate);
                case TaggedGrammar tg:
                    return tg.Tagged.First(predicate);
                case HiddenGrammar ht:
                    return ht.Hidden.First(predicate);
                case OneOrMoreGrammar oom:
                    return oom.Repeated.First(predicate)
                        ?? oom.Separator?.First(predicate);
                case ZeroOrMoreGrammar zom:
                    return zom.Repeated.First(predicate)
                        ?? zom.Separator?.First(predicate);
                default:
                    return null;
            }
        }

        public bool IsEquivalentTo(Grammar grammar) =>
            AreEquivalent(this, grammar);


        public static Grammar Unhidden(Grammar g)
        {
            while (g is HiddenGrammar h)
                g = h.Hidden;
            return g;
        }

        public static bool AreEquivalent(Grammar a, Grammar b)
        {
            if (a == b)
                return true;

            if (a == null || b == null)
                return false;

            a = Unhidden(a);
            b = Unhidden(b);

            if (a.GetType() == b.GetType())
            {
                switch (a)
                {
                    case SequenceGrammar seqa:
                        var seqb = (SequenceGrammar)b;
                        return AreEquivalent(seqa.Steps, seqb.Steps);

                    case AlternationGrammar alta:
                        var altb = (AlternationGrammar)b;
                        return AreEquivalent(alta.Alternatives, altb.Alternatives);

                    case OptionalGrammar opta:
                        var optb = (OptionalGrammar)b;
                        return AreEquivalent(opta.Optioned, optb.Optioned);

                    case RequiredGrammar reqa:
                        var reqb = (RequiredGrammar)b;
                        return AreEquivalent(reqa.Required, reqb.Required);

                    case ZeroOrMoreGrammar zoma:
                        var zomb = (ZeroOrMoreGrammar)b;
                        return AreEquivalent(zoma.Repeated, zomb.Repeated)
                            && AreEquivalent(zoma.Separator, zomb.Separator);

                    case OneOrMoreGrammar ooma:
                        var oomb = (OneOrMoreGrammar)b;
                        return AreEquivalent(ooma.Repeated, oomb.Repeated)
                            && AreEquivalent(ooma.Separator, oomb.Separator);

                    case TaggedGrammar nama:
                        var namb = (TaggedGrammar)b;
                        return
                            nama.Tag == namb.Tag
                            && AreEquivalent(nama.Tagged, namb.Tagged);

                    case TokenGrammar toka:
                        var tokb = (TokenGrammar)b;
                        return toka.TokenText == tokb.TokenText;

                    case RuleGrammar rula:
                        var rulb = (RuleGrammar)b;
                        return rula.RuleName == rulb.RuleName;
                }
            }

            return false;
        }

        public static bool AreEquivalent(IReadOnlyList<Grammar> a, IReadOnlyList<Grammar> b)
        {
            if (a == b)
                return true;
            if (a == null || b == null)
                return false;
            if (a.Count != b.Count)
                return false;
            for (int i = 0; i < a.Count; i++)
            {
                if (!AreEquivalent(a[i], b[i]))
                    return false;
            }
            return true;
        }

        public override string ToString() =>
            ToString(GrammarStyle.Brackets);

        public string ToString(GrammarStyle style)
        {
            var builder = new StringBuilder();
            GrammarWriter.WriteTo(builder, this, style);
            return builder.ToString();
        }

        private string DebugText => $"{Kind}: {ToString()}";

        private string Kind
        {
            get
            {
                switch (this)
                {
                    case TokenGrammar _:
                        return "Token";
                    case RuleGrammar _:
                        return "Rule";
                    case SequenceGrammar _:
                        return "Sequence";
                    case AlternationGrammar _:
                        return "Alternation";
                    case OptionalGrammar _:
                        return "Optional";
                    case RequiredGrammar _:
                        return "Required";
                    case OneOrMoreGrammar _:
                        return "OneOrMore";
                    case ZeroOrMoreGrammar _:
                        return "ZeroOrMore";
                    case TaggedGrammar _:
                        return "Tagged";
                    case HiddenGrammar _:
                        return "Hidden";
                    default:
                        return "Unknown";
                };
            }
        }

        /// <summary>
        /// Walks the grammar tree calling the supplied callback actions for each node.
        /// </summary>
        /// <param name="grammar">The starting node of the walk.</param>
        /// <param name="fnVisitChildren">An optional callback function that is invoked for reach grammar node before its child nodes are visited. 
        /// If the callback returns false the children are not visited.</param>
        /// <param name="actionAfter">An optional callback actio that is invoked for each grammar node after its child nodes have been visited.</param>
        public static void Walk(Grammar grammar, Func<Grammar, bool> fnVisitChildren = null, Action<Grammar> actionAfter = null)
        {
            if (grammar == null)
                return;

            if (fnVisitChildren != null)
            {
                // exit if fnBefore return false
                if (!fnVisitChildren(grammar))
                    return;
            }

            switch (grammar)
            {
                case SequenceGrammar sg:
                    foreach (var step in sg.Steps)
                    {
                        Walk(step, fnVisitChildren, actionAfter);
                    }
                    break;
                case AlternationGrammar ag:
                    foreach (var alt in ag.Alternatives)
                    {
                        Walk(alt, fnVisitChildren, actionAfter);
                    }
                    break;
                case OptionalGrammar og:
                    Walk(og.Optioned, fnVisitChildren, actionAfter);
                    break;
                case RequiredGrammar rg:
                    Walk(rg.Required, fnVisitChildren, actionAfter);
                    break;
                case TaggedGrammar tg:
                    Walk(tg.Tagged, fnVisitChildren, actionAfter);
                    break;
                case HiddenGrammar ht:
                    Walk(ht.Hidden, fnVisitChildren, actionAfter);
                    break;
                case OneOrMoreGrammar oom:
                    Walk(oom.Repeated, fnVisitChildren, actionAfter);
                    Walk(oom.Separator, fnVisitChildren, actionAfter);
                    break;
                case ZeroOrMoreGrammar zom:
                    Walk(zom.Repeated, fnVisitChildren, actionAfter);
                    Walk(zom.Separator, fnVisitChildren, actionAfter);
                    break;
                default:
                    return;
            }

            actionAfter?.Invoke(grammar);
        }
    }

    public sealed class GrammarEquivalenceComparer : IEqualityComparer<Grammar>
    {
        private GrammarEquivalenceComparer() { }

        public bool Equals(Grammar x, Grammar y) => x.IsEquivalentTo(y);
        public int GetHashCode(Grammar g) => g.GetHashCode();

        public static readonly GrammarEquivalenceComparer Instance = new GrammarEquivalenceComparer();
    }

    /// <summary>
    /// A grammar element that represents a sequence of two or more grammar elements.
    /// </summary>
    public sealed class SequenceGrammar : Grammar
    {
        public IReadOnlyList<Grammar> Steps { get; }

        public SequenceGrammar(IReadOnlyList<Grammar> steps)
            : base(ComputeHashCode(steps))
        {
            if (steps == null)
                throw new ArgumentNullException(nameof(steps));
            if (steps.Count < 2)
                throw new InvalidOperationException($"Number of steps must be 2 or more");
            this.Steps = steps;
        }

        public SequenceGrammar(params Grammar[] steps)
            : this((IReadOnlyList<Grammar>)steps)
        {
        }

        public static SequenceGrammar Flatten(params Grammar[] steps)
        {
            var list = new List<Grammar>(steps.Length);
            foreach (var step in steps)
            {
                AddFlattened(list, step);
            }
            return new SequenceGrammar(list);
        }

        private static void AddFlattened(List<Grammar> list, Grammar grammar)
        {
            if (grammar is SequenceGrammar seq)
            {
                foreach (var step in seq.Steps)
                {
                    AddFlattened(list, step);
                }
            }
            else
            {
                list.Add(grammar);
            }
        }

        public override TResult Accept<TResult>(GrammarVisitor<TResult> visitor) =>
            visitor.VisitSequence(this);

        public SequenceGrammar With(IReadOnlyList<Grammar> steps) =>
            this.Steps != steps ? new SequenceGrammar(steps) : this;
    }

    /// <summary>
    /// A grammar element that represents two or more alternative grammars.
    /// </summary>
    public sealed class AlternationGrammar : Grammar
    {
        public IReadOnlyList<Grammar> Alternatives { get; }

        public AlternationGrammar(IReadOnlyList<Grammar> alternatives)
            : base(ComputeHashCode(alternatives))
        {
            if (alternatives == null)
                throw new ArgumentNullException(nameof(alternatives));
            if (alternatives.Count < 2)
                throw new InvalidOperationException($"Number of alternatives must be 2 or more");
            this.Alternatives = alternatives;
        }

        public AlternationGrammar(params Grammar[] alternatives)
            : this((IReadOnlyList<Grammar>)alternatives)
        {
        }

        public override TResult Accept<TResult>(GrammarVisitor<TResult> visitor) =>
            visitor.VisitAlternation(this);

        public AlternationGrammar With(IReadOnlyList<Grammar> alternatives) =>
            this.Alternatives != alternatives ? new AlternationGrammar(alternatives) : this;
    }

    /// <summary>
    /// A grammar element that represents an optional grammar.
    /// </summary>
    public sealed class OptionalGrammar : Grammar
    {
        public Grammar Optioned { get; }

        public OptionalGrammar(Grammar optioned)
            : base(optioned.GetHashCode())
        {
            this.Optioned = optioned;
        }

        public override TResult Accept<TResult>(GrammarVisitor<TResult> visitor) =>
            visitor.VisitOptional(this);

        public OptionalGrammar With(Grammar optioned) =>
            this.Optioned != optioned 
                ? new OptionalGrammar(optioned) 
                : this;
    }

    /// <summary>
    /// A grammar element that represents a required grammar
    /// </summary>
    public sealed class RequiredGrammar : Grammar
    {
        public Grammar Required { get; }

        public RequiredGrammar(Grammar required)
            : base(required.GetHashCode())
        {
            this.Required = required;
        }

        public override TResult Accept<TResult>(GrammarVisitor<TResult> visitor) =>
            visitor.VisitRequired(this);

        public RequiredGrammar With(Grammar required) =>
            this.Required != required ? new RequiredGrammar(required) : this;
    }

    /// <summary>
    /// A grammar element that represents zero or more repetitions of a grammar element
    /// and an optional separator.
    /// </summary>
    public sealed class ZeroOrMoreGrammar : Grammar
    {
        public Grammar Repeated { get; }
        public Grammar Separator { get; }
        public bool AllowTrailingSeparator { get; }

        public ZeroOrMoreGrammar(Grammar repeated, Grammar separator = null, bool allowTrailingSeparator = false)
            : base(repeated.GetHashCode() + (separator?.GetHashCode() ?? 0))
        {
            this.Repeated = repeated;
            this.Separator = separator;
            this.AllowTrailingSeparator = allowTrailingSeparator;
        }

        public override TResult Accept<TResult>(GrammarVisitor<TResult> visitor) =>
            visitor.VisitZeroOrMore(this);

        public ZeroOrMoreGrammar With(Grammar repeated, Grammar separator, bool allowTrailingSeparator) =>
            (this.Repeated != repeated || this.Separator != separator || this.AllowTrailingSeparator != allowTrailingSeparator)
                ? new ZeroOrMoreGrammar(repeated, separator, allowTrailingSeparator)
                : this;
    }

    /// <summary>
    /// A grammar element that represents one or more repetitions of a grammar element
    /// and an optional separator.
    /// </summary>
    public sealed class OneOrMoreGrammar : Grammar
    {
        public Grammar Repeated { get; }
        public Grammar Separator { get; }
        public bool AllowTrailingSeparator { get; }

        public OneOrMoreGrammar(Grammar repeated, Grammar separator = null, bool allowTrailingSeparator = false)
            : base(repeated.GetHashCode() + (separator?.GetHashCode() ?? 0))
        {
            this.Repeated = repeated;
            this.Separator = separator;
            this.AllowTrailingSeparator = allowTrailingSeparator;
        }

        public override TResult Accept<TResult>(GrammarVisitor<TResult> visitor) =>
            visitor.VisitOneOrMore(this);

        public OneOrMoreGrammar With(Grammar repeated, Grammar separator, bool allowTrailingSeparator) =>
            (this.Repeated != repeated || this.Separator != separator || this.AllowTrailingSeparator != allowTrailingSeparator)
                ? new OneOrMoreGrammar(repeated, separator, allowTrailingSeparator)
                : this;
    }

    /// <summary>
    /// A grammar element that represents a grammar with name tag.
    /// </summary>
    public sealed class TaggedGrammar : Grammar
    {
        public string Tag { get; }
        public Grammar Tagged { get; }

        public TaggedGrammar(string tag, Grammar tagged)
            : base(tagged.GetHashCode())
        {
            this.Tag = tag;
            this.Tagged = tagged;
        }

        public override TResult Accept<TResult>(GrammarVisitor<TResult> visitor) =>
            visitor.VisitTagged(this);

        public TaggedGrammar With(string tag, Grammar tagged) =>
            (this.Tag != tag || this.Tagged != tagged)
                ? new TaggedGrammar(tag, tagged)
                : this;
    }

    /// <summary>
    /// A grammar element that represents a grammar with name tag.
    /// </summary>
    public sealed class HiddenGrammar : Grammar
    {
        public Grammar Hidden { get; }

        public HiddenGrammar(Grammar hidden)
            : base(hidden.GetHashCode())
        {
            this.Hidden = hidden;
        }

        public override TResult Accept<TResult>(GrammarVisitor<TResult> visitor) =>
            visitor.VisitHidden(this);

        public HiddenGrammar With(Grammar hidden) =>
            hidden != this.Hidden
                ? new HiddenGrammar(hidden)
                : this;
    }

    /// <summary>
    /// A grammar element that represents an expected word or punctuation.
    /// </summary>
    public sealed class TokenGrammar : Grammar
    {
        public string TokenText { get; }

        public TokenGrammar(string tokenText)
            : base(tokenText.GetHashCode())
        {
            this.TokenText = tokenText;
        }

        public override TResult Accept<TResult>(GrammarVisitor<TResult> visitor) =>
            visitor.VisitToken(this);

        public TokenGrammar With(string tokenText) =>
            this.TokenText != tokenText ? new TokenGrammar(tokenText) : this;
    }

    /// <summary>
    /// A grammar element that represent a reference to an external grammar rule.
    /// </summary>
    public sealed class RuleGrammar : Grammar
    {
        public string RuleName { get; }

        public RuleGrammar(string ruleName)
            : base(ruleName.GetHashCode())
        {
            this.RuleName = ruleName;
        }

        public override TResult Accept<TResult>(GrammarVisitor<TResult> visitor) =>
            visitor.VisitRule(this);

        public RuleGrammar With(string ruleName) =>
            this.RuleName != ruleName ? new RuleGrammar(ruleName) : this;
    }

    #endregion

    #region Grammar Visiting / Rewriting / Mapping

    /// <summary>
    /// The visitor pattern for <see cref="Grammar"/> nodes.
    /// </summary>
    public abstract class GrammarVisitor<TResult>
    {
        public abstract TResult VisitSequence(SequenceGrammar grammar);
        public abstract TResult VisitAlternation(AlternationGrammar grammar);
        public abstract TResult VisitOptional(OptionalGrammar grammar);
        public abstract TResult VisitRequired(RequiredGrammar grammar);
        public abstract TResult VisitZeroOrMore(ZeroOrMoreGrammar grammar);
        public abstract TResult VisitOneOrMore(OneOrMoreGrammar grammar);
        public abstract TResult VisitTagged(TaggedGrammar grammar);
        public abstract TResult VisitHidden(HiddenGrammar grammar);
        public abstract TResult VisitToken(TokenGrammar grammar);
        public abstract TResult VisitRule(RuleGrammar grammar);
    }

    /// <summary>
    /// A base visitor class for implementing grammar tree rewriters.
    /// The default implementations of this class handle reconstructing parent nodes when child nodes are changed.
    /// </summary>
    public abstract class DefaultGrammarVisitor<TResult> : GrammarVisitor<TResult>
    {
        public abstract TResult DefaultVisit(Grammar grammar);

        public override TResult VisitAlternation(AlternationGrammar grammar) =>
            DefaultVisit(grammar);

        public override TResult VisitOneOrMore(OneOrMoreGrammar grammar) =>
            DefaultVisit(grammar);

        public override TResult VisitOptional(OptionalGrammar grammar) =>
            DefaultVisit(grammar);

        public override TResult VisitRequired(RequiredGrammar grammar) =>
            DefaultVisit(grammar);

        public override TResult VisitRule(RuleGrammar grammar) =>
            DefaultVisit(grammar);

        public override TResult VisitSequence(SequenceGrammar grammar) =>
            DefaultVisit(grammar);

        public override TResult VisitTagged(TaggedGrammar grammar) =>
            DefaultVisit(grammar);

        public override TResult VisitHidden(HiddenGrammar grammar) =>
            DefaultVisit(grammar);

        public override TResult VisitToken(TokenGrammar grammar) =>
            DefaultVisit(grammar);

        public override TResult VisitZeroOrMore(ZeroOrMoreGrammar grammar) =>
            DefaultVisit(grammar);
    }

    /// <summary>
    /// A base visitor class for implementing grammar tree rewriters.
    /// The default implementations of this class handle reconstructing parent nodes when child nodes are changed.
    /// </summary>
    public abstract class GrammarRewriter : GrammarVisitor<Grammar>
    {
        public override Grammar VisitAlternation(AlternationGrammar grammar)
        {
            var newList = VisitAlternatives(grammar.Alternatives);

            if (newList.Count == 0)
            {
                return null;
            }
            else if (newList.Count == 1)
            {
                return newList[0];
            }
            else
            {
                return grammar.With(newList);
            }
        }

        public virtual IReadOnlyList<Grammar> VisitAlternatives(IReadOnlyList<Grammar> alternatives) =>
            RewriteAlternatives(alternatives);

        public override Grammar VisitSequence(SequenceGrammar grammar)
        {
            var newList = VisitSteps(grammar.Steps);

            if (newList.Count == 0)
            {
                return null;
            }
            else if (newList.Count == 1)
            {
                return newList[0];
            }
            else
            {
                return grammar.With(newList);
            }
        }

        public virtual IReadOnlyList<Grammar> VisitSteps(IReadOnlyList<Grammar> steps) =>
            RewriteSteps(steps);

        public override Grammar VisitOneOrMore(OneOrMoreGrammar grammar) =>
            grammar.With(grammar.Repeated.Accept(this), grammar.Separator?.Accept(this), grammar.AllowTrailingSeparator);

        public override Grammar VisitZeroOrMore(ZeroOrMoreGrammar grammar) =>
            grammar.With(grammar.Repeated.Accept(this), grammar.Separator?.Accept(this), grammar.AllowTrailingSeparator);

        public override Grammar VisitOptional(OptionalGrammar grammar)
        {
            var newOptioned = grammar.Optioned.Accept(this);
            return grammar.With(newOptioned);
        }

        public override Grammar VisitRequired(RequiredGrammar grammar) =>
            grammar.With(grammar.Required.Accept(this));

        public override Grammar VisitTagged(TaggedGrammar grammar) =>
            grammar.With(grammar.Tag, grammar.Tagged.Accept(this));

        public override Grammar VisitHidden(HiddenGrammar grammar) =>
            grammar.With(grammar.Hidden.Accept(this));

        public override Grammar VisitRule(RuleGrammar grammar) =>
            grammar;

        public override Grammar VisitToken(TokenGrammar grammar) =>
            grammar;

        public IReadOnlyList<Grammar> RewriteSteps(IReadOnlyList<Grammar> list)
        {
            List<Grammar> newList = null;

            for (int i = 0; i < list.Count; i++)
            {
                var elem = list[i];
                var newElem = elem.Accept(this);
                if (newElem != elem || newList != null)
                {
                    if (newList == null)
                        newList = list.Take(i).ToList();

                    if (!(elem is SequenceGrammar) && newElem is SequenceGrammar newSeq)
                    {
                        // automatically fold in additional steps
                        newList.AddRange(newSeq.Steps);
                    }
                    else if (newElem != null)
                    {
                        newList.Add(newElem);
                    }
                }
            }

            return newList ?? list;
        }

        public IReadOnlyList<Grammar> RewriteAlternatives(IReadOnlyList<Grammar> list)
        {
            List<Grammar> newList = null;

            for (int i = 0; i < list.Count; i++)
            {
                var elem = list[i];
                var newElem = elem.Accept(this);
                if (newElem != elem || newList != null)
                {
                    if (newList == null)
                        newList = list.Take(i).ToList();

                    if (!(elem is AlternationGrammar) && newElem is AlternationGrammar newAlt)
                    {
                        // automatically fold in additional alternatives
                        newList.AddRange(newAlt.Alternatives);
                    }
                    else if (newElem != null)
                    {
                        newList.Add(newElem);
                    }
                }
            }

            return newList ?? list;
        }

        public IReadOnlyList<Grammar> VisitList(IReadOnlyList<Grammar> list)
        {
            List<Grammar> newList = null;

            for (int i = 0; i < list.Count; i++)
            {
                var elem = list[i];
                var newElem = elem.Accept(this);
                if (newElem != elem || newList != null)
                {
                    if (newList == null)
                        newList = list.Take(i).ToList();
                    if (newElem != null)
                        newList.Add(newElem);
                }
            }

            return newList ?? list;
        }
    }

    /// <summary>
    /// A rewriter that applies the function or visitor to each node or rewritten node in the tree, bottom up.
    /// </summary>
    public sealed class GrammarTreeMapper : GrammarRewriter
    {
        private readonly Func<Grammar, Grammar> _fnMap;

        public GrammarTreeMapper(Func<Grammar, Grammar> fnMap)
        {
            _fnMap = fnMap;
        }

        public GrammarTreeMapper(GrammarVisitor<Grammar> visitor)
            : this(g => g.Accept(visitor))
        {
        }

        private Grammar Map(Grammar grammar) =>
            grammar != null ? _fnMap(grammar) : null;

        public override Grammar VisitAlternation(AlternationGrammar grammar) =>
            Map(base.VisitAlternation(grammar));

        public override Grammar VisitSequence(SequenceGrammar grammar) =>
            Map(base.VisitSequence(grammar));

        public override Grammar VisitOneOrMore(OneOrMoreGrammar grammar) =>
            Map(base.VisitOneOrMore(grammar));

        public override Grammar VisitZeroOrMore(ZeroOrMoreGrammar grammar) =>
            Map(base.VisitZeroOrMore(grammar));

        public override Grammar VisitOptional(OptionalGrammar grammar) =>
            Map(base.VisitOptional(grammar));

        public override Grammar VisitRequired(RequiredGrammar grammar) =>
            Map(base.VisitRequired(grammar));

        public override Grammar VisitTagged(TaggedGrammar grammar) =>
            Map(base.VisitTagged(grammar));

        public override Grammar VisitHidden(HiddenGrammar grammar) =>
            Map(base.VisitHidden(grammar));

        public override Grammar VisitRule(RuleGrammar grammar) =>
            Map(base.VisitRule(grammar));

        public override Grammar VisitToken(TokenGrammar grammar) =>
            Map(base.VisitToken(grammar));
    }

    #endregion

    #region Grammar Writer

    public enum GrammarStyle
    {
        /// <summary>
        /// Use syntax like {x} for repetitions and [x] for optional
        /// </summary>
        Brackets,

        /// <summary>
        /// Use syntax like x* for repetitions and x? for optional
        /// </summary>
        Antlr
    }

    /// <summary>
    /// A class used to convert grammar trees into text.
    /// Used by grammar nodes for ToString().
    /// </summary>
    public static class GrammarWriter
    {
        public static void WriteTo(StringBuilder builder, Grammar grammar, GrammarStyle style)
        {
            var writer = new Writer(builder, style);
            grammar.Accept(writer);
        }

        private class Writer : GrammarVisitor<bool>
        {
            private readonly StringBuilder _builder;
            private readonly GrammarStyle _style;

            public Writer(StringBuilder builder, GrammarStyle style)
            {
                _builder = builder ?? new StringBuilder();
                _style = style;
            }

            public override bool VisitAlternation(AlternationGrammar grammar)
            {
                for (int i = 0; i < grammar.Alternatives.Count; i++)
                {
                    var alt = grammar.Alternatives[i];

                    if (i > 0)
                        _builder.Append(" | ");

                    if (alt is AlternationGrammar)
                    {
                        // only nested alternations need to be bracketed inside an alternation
                        WriteParens(alt);
                    }
                    else
                    {
                        Write(alt);
                    }
                }

                return true;
            }

            public override bool VisitOneOrMore(OneOrMoreGrammar grammar)
            {
                if (_style == GrammarStyle.Brackets || grammar.Separator != null)
                {
                    _builder.Append("{");
                    Write(grammar.Repeated);

                    if (grammar.Separator != null)
                    {
                        _builder.Append(", ");
                        Write(grammar.Separator);

                        if (grammar.AllowTrailingSeparator)
                        {
                            _builder.Append("~");
                        }
                    }

                    _builder.Append("}+");
                }
                else
                {
                    switch (grammar.Repeated)
                    {
                        case TokenGrammar _:
                        case RuleGrammar _:
                            Write(grammar.Repeated);
                            break;
                        default:
                            WriteParens(grammar.Repeated);
                            break;
                    }

                    _builder.Append("+");
                }

                return true;
            }

            public override bool VisitOptional(OptionalGrammar grammar)
            {
                WriteOptional(grammar.Optioned);
                return true;
            }

            private void WriteOptional(Grammar optioned)
            {
                if (_style == GrammarStyle.Brackets)
                {
                    _builder.Append("[");
                    Write(optioned);
                    _builder.Append("]");
                }
                else
                {
                    switch (optioned)
                    {
                        case TokenGrammar _:
                        case RuleGrammar _:
                            Write(optioned);
                            break;
                        default:
                            WriteParens(optioned);
                            break;
                    }
                    _builder.Append("?");
                }
            }

            public override bool VisitRequired(RequiredGrammar grammar)
            {
                switch (grammar.Required)
                {
                    case TokenGrammar _:
                    case RuleGrammar _:
                        Write(grammar.Required);
                        break;
                    default:
                        WriteParens(grammar.Required);
                        break;
                }

                _builder.Append("!");

                return true;
            }

            public override bool VisitRule(RuleGrammar grammar)
            {
                _builder.Append("<");
                _builder.Append(grammar.RuleName);
                _builder.Append(">");

                return true;
            }

            public override bool VisitSequence(SequenceGrammar grammar)
            {
                for (int i = 0; i < grammar.Steps.Count; i++)
                {
                    if (i > 0)
                        _builder.Append(" ");

                    var step = grammar.Steps[i];

                    if (step is AlternationGrammar || step is SequenceGrammar)
                    {
                        // only needs parens if is a nested alternation or sequence
                        WriteParens(step);
                    }
                    else
                    {
                        Write(step);
                    }
                }

                return true;
            }

            public override bool VisitTagged(TaggedGrammar grammar)
            {
                _builder.Append(grammar.Tag);
                _builder.Append("=");

                switch (grammar.Tagged)
                {
                    case AlternationGrammar _:
                    case SequenceGrammar _:
                    case TaggedGrammar _:
                        // only needs parens if is alternation, sequence or another tag
                        WriteParens(grammar.Tagged);
                        break;
                    default:
                        Write(grammar.Tagged);
                        break;
                }

                return true;
            }

            public override bool VisitHidden(HiddenGrammar grammar)
            {
                _builder.Append("#");
                Write(grammar.Hidden);
                return true;
            }

            private static bool IsSimpleToken(string text)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    var ch = text[i];
                    if (!(char.IsLetterOrDigit(ch) || ch == '_' || ch == '-'))
                        return false;
                }

                return true;
            }

            public override bool VisitToken(TokenGrammar grammar)
            {
                if (IsSimpleToken(grammar.TokenText))
                {
                    _builder.Append(grammar.TokenText);
                }
                else
                {
                    _builder.Append('\'');
                    _builder.Append(grammar.TokenText);
                    _builder.Append('\'');
                }

                return true;
            }

            public override bool VisitZeroOrMore(ZeroOrMoreGrammar grammar)
            {
                if (_style == GrammarStyle.Brackets || grammar.Separator != null)
                {
                    _builder.Append("{");
                    Write(grammar.Repeated);

                    if (grammar.Separator != null)
                    {
                        _builder.Append(", ");
                        Write(grammar.Separator);

                        if (grammar.AllowTrailingSeparator)
                        {
                            _builder.Append("~");
                        }
                    }

                    _builder.Append("}");
                }
                else
                {
                    switch (grammar.Repeated)
                    {
                        case TokenGrammar _:
                        case RuleGrammar _:
                            Write(grammar.Repeated);
                            break;
                        default:
                            WriteParens(grammar.Repeated);
                            break;
                    }

                    _builder.Append("*");
                }

                return true;
            }

            private void Write(Grammar grammar)
            {
                grammar.Accept(this);
            }

            private void WriteParens(Grammar grammar)
            {
                _builder.Append("(");
                grammar.Accept(this);
                _builder.Append(")");
            }
        }
    }

    #endregion

    #region Grammar Analysis

    /// <summary>
    /// An analysis of a set of grammars to help determine where they overlap and diverge.
    /// </summary>
    public abstract class GrammarAnalysis
    {
        /// <summary>
        /// Returns true if the grammar node does not have any alternative grammar terms that
        /// can appear in the same position.
        /// </summary>
        public abstract bool IsUnique(Grammar term);

        /// <summary>
        /// Returns true if the grammar element is not unique, but it occurs as part of last alternative in 
        /// list of alternatives.
        /// </summary>
        public abstract bool IsLastNonUnique(Grammar term);

        /// <summary>
        /// Gets a list of alternative terms that can appear in the same position as the given grammar term.
        /// This list will include the specified term as one of the alternatives.
        /// </summary>
        public abstract IReadOnlyList<Grammar> GetAlternativeTerms(Grammar term);

        /// <summary>
        /// Creates a new <see cref="GrammarAnalysis"/> for the specified grammar alternatives.
        /// </summary>
        public static GrammarAnalysis Create(IReadOnlyList<Grammar> alternatives)
        {
            return new MergeAnalysis(alternatives);
        }

        private static GrammarAnalysis _empty;
        public static GrammarAnalysis Empty
        {
            get
            {
                if (_empty == null)
                    _empty = Create(new Grammar[] { });
                return _empty;
            }
        }
    }

    internal class MergeAnalysis : GrammarAnalysis
    {
        private readonly IReadOnlyList<Grammar> _alternatives;
        private readonly IReadOnlyList<GrammarGraph> _graphs;
        private readonly GrammarGraph _mergedGraph;
        private readonly HashSet<Grammar> _uniqueTerms;
        private readonly HashSet<Grammar> _lastTerms;
        private Dictionary<Grammar, IReadOnlyList<Grammar>> _alternativeMap;

        public MergeAnalysis(IReadOnlyList<Grammar> alternatives)
        {
            _alternatives = alternatives;

            if (alternatives.Count == 1)
            {
                _mergedGraph = new GrammarGraph(alternatives);
                _graphs = new[] { _mergedGraph };
            }
            else if (alternatives.Count == 0)
            {
                _mergedGraph = new GrammarGraph();
                _graphs = new GrammarGraph[] { };
            }
            else
            {
                _graphs = alternatives.Select(r => new GrammarGraph(r)).ToList();
                _mergedGraph = new GrammarGraph(alternatives);
            }

            _uniqueTerms = new HashSet<Grammar>();
            _mergedGraph.GetUniqueTerms(_uniqueTerms);

            _lastTerms = new HashSet<Grammar>();
            if (alternatives.Count > 0)
            {
                // determine the set of non-unique grammar nodes that
                // occur for the last time within the set of alternatives
                AddLastTerms(0, _alternatives.Count, 0);
            }
        }

        public override bool IsUnique(Grammar term)
        {
            return _uniqueTerms.Contains(term);
        }

        public override bool IsLastNonUnique(Grammar term)
        {
            return _lastTerms.Contains(term);
        }

        private static int MaxNthTerm = 10;

        /// <summary>
        /// Compute the set of non-unique terms that appear last in the sequence of alternate grammar paths
        /// </summary>
        private void AddLastTerms(
            int start, int length, int nthTerm)
        {
            var end = start + length;
            var subStart = start;
            var subStartTerms = new List<Grammar>();
            var subEndTerms = new List<Grammar>();

            //var subStartAlt = _alternatives[subStart];
            var subStartGraph = _graphs[subStart];
            subStartGraph.GetNthTerms(nthTerm, subStartTerms);

            for (int i = start + 1; i < end; i++)
            {
                var subEndGraph = _graphs[i];
                subEndTerms.Clear();
                subEndGraph.GetNthTerms(nthTerm, subEndTerms);

                if (!Overlaps(subStartTerms, subEndTerms))
                {
                    var subLen = i - subStart;
                    if (nthTerm < MaxNthTerm)
                    {
                        // otherwise attempt to differentiate this sub range
                        AddLastTerms(subStart, subLen, nthTerm + 1);
                    }

                    subStartTerms.Clear();
                    subStartTerms.AddRange(subEndTerms);
                    subEndTerms.Clear();
                    subStart = i;

                    if (subLen > 1)
                    {
                        // mark nth term of last alt as last
                        subEndGraph = _graphs[i - 1];
                        subEndGraph.GetNthTerms(nthTerm, subEndTerms);
                        AddLastTerms(subEndTerms);
                        subEndTerms.Clear();
                    }
                    continue;
                }
            }

            if (subStart > start)
            {
                // there is a remaining sub range at the end
                var subLen = end - subStart;
                if (subLen == 1)
                {
                    // subStart is unique in the nth position
                    //AddUniqueTerms(subStartTerms);
                }
                else if (nthTerm < MaxNthTerm)
                {
                    // otherwise attempt to differentiate this sub range
                    AddLastTerms(subStart, subLen, nthTerm + 1);
                }
            }
            else if (nthTerm < MaxNthTerm)
            {
                // no unique terms in this entire range, try to differentiate by n+1 term
                AddLastTerms(start, length, nthTerm + 1);
            }

            if (end - subStart > 1)
            {
                // mark nth term of last alt as last
                var endAlt = _alternatives[end - 1];
                var endGraph = _graphs[end - 1];
                subEndTerms.Clear();
                endGraph.GetNthTerms(nthTerm, subEndTerms);
                AddLastTerms(subEndTerms);
                subEndTerms.Clear();
            }
        }

        private void AddLastTerms(List<Grammar> items)
        {
            _lastTerms.UnionWith(items);
        }

        private static bool Overlaps(List<Grammar> a, List<Grammar> b)
        {
            return b.Any(g => a.Contains(g, GrammarEquivalenceComparer.Instance));
        }

        public override IReadOnlyList<Grammar> GetAlternativeTerms(Grammar term)
        {
            if (_alternativeMap == null)
            {
                var map = new Dictionary<Grammar, IReadOnlyList<Grammar>>();
                _mergedGraph.GetAlternativeTerms(map);
                Interlocked.CompareExchange(ref _alternativeMap, map, null);
            }

            _alternativeMap.TryGetValue(term, out var list);
            return list ?? EmptyList;
        }

        private static readonly IReadOnlyList<Grammar> EmptyList = new List<Grammar>().AsReadOnly();
    }

    /// <summary>
    /// A class that models the merger of a set of alternate grammars
    /// </summary>
    internal class GrammarGraph
    {
        private readonly Node _root;
#if DEBUG_MAP
        private readonly Dictionary<Grammar, Node> _grammarToNodeMap;
#endif

        public GrammarGraph()
        {
            _root = new Node(null);

#if DEBUG_MAP
            _grammarToNodeMap = new Dictionary<Grammar, Node>();
#endif
        }

        public GrammarGraph(Grammar grammar)
            : this()
        {
            Add(grammar);
        }

        public GrammarGraph(IReadOnlyList<Grammar> alternates)
            : this()
        {
            foreach (var alt in alternates)
            {
                Add(alt);
            }
        }

        /// <summary>
        /// Adds all the grammar terms in the grammar tree
        /// </summary>
        public void Add(Grammar root)
        {
            Add(_root, root, null);
        }

        /// <summary>
        /// Get all the terms that occur only once along their path
        /// </summary>
        public void GetUniqueTerms(HashSet<Grammar> terms)
        {
            _root.GetUniqueTerms(terms);
        }

        /// <summary>
        /// Gets all the terms that can match the nth input item
        /// </summary>
        public void GetNthTerms(int n, List<Grammar> terms)
        {
            _root.GetNthTerms(n, terms);
        }

        /// <summary>
        /// Gets a map of all the alternative terms that can occur in the same position as given term.
        /// The resulting list will include the grammar itself.
        /// </summary>
        public void GetAlternativeTerms(Dictionary<Grammar, IReadOnlyList<Grammar>> map)
        {
            _root.GetAlternativeTerms(map);
        }

        private void Add(Node root, Grammar grammar, HashSet<Node> nextNodes)
        {
            switch (grammar)
            {
                case TokenGrammar _:
                case RuleGrammar _:
                    var nn = root.AddTerm(grammar);
                    nextNodes?.Add(nn);
#if DEBUG_MAP
                    if (_grammarToNodeMap.ContainsKey(grammar))
                    {
                        // not sure yet to do anything about this
                        // System.Diagnostics.Debug.Fail("Grammar already assigned to a node");
                    }
                    else
                    {
                        _grammarToNodeMap.Add(grammar, nn);
                    }
#endif
                    break;
                case SequenceGrammar seq:
                    AddSteps(seq, 0, root, nextNodes);
                    break;
                case AlternationGrammar alt:
                    foreach (var a in alt.Alternatives)
                    {
                        Add(root, a, nextNodes);
                    }
                    break;
                case OneOrMoreGrammar oom:
                    Add(root, oom.Repeated, nextNodes);
                    break;
                case ZeroOrMoreGrammar zom:
                    // add root for zero case
                    nextNodes?.Add(root);
                    Add(root, zom.Repeated, nextNodes);
                    break;
                case OptionalGrammar opt:
                    // add root for zero case
                    nextNodes?.Add(root);
                    Add(root, opt.Optioned, nextNodes);
                    break;
                case RequiredGrammar req:
                    Add(root, req.Required, nextNodes);
                    break;
                case TaggedGrammar tag:
                    Add(root, tag.Tagged, nextNodes);
                    break;
                case HiddenGrammar hid:
                    Add(root, hid.Hidden, nextNodes);
                    break;
                default:
                    throw new InvalidOperationException($"Unhandled grammar {grammar.GetType().Name}");
            }
        }

        private void AddSteps(SequenceGrammar grammar, int iStep, Node root, HashSet<Node> finalNext)
        {
            var isFinal = iStep == grammar.Steps.Count - 1;

            HashSet<Node> stepNext = !isFinal
                ? new HashSet<Node>()
                : finalNext;

            var step = grammar.Steps[iStep];
            Add(root, step, stepNext);

            if (!isFinal)
            {
                AddSteps(grammar, iStep + 1, stepNext, finalNext);
            }
        }

        private void AddSteps(SequenceGrammar grammar, int iStep, HashSet<Node> roots, HashSet<Node> finalNext)
        {
            var isFinal = iStep == grammar.Steps.Count - 1;
            HashSet<Node> stepNext = !isFinal
                ? new HashSet<Node>()
                : finalNext;

            foreach (var root in roots)
            {
                var step = grammar.Steps[iStep];
                Add(root, step, stepNext);
            }

            if (!isFinal)
            {
                AddSteps(grammar, iStep + 1, stepNext, finalNext);
            }
        }

        private class Node
        {
            // all the grammar terms from alternate sources that branched to here
            // can either be a single grammar or a hashset or null
            private object _branchesFrom;

            // all the branches for common terms 
            // can either be a single KeyValuePair, Dictionary or null
            private object _branchesTo;

            public Node(Grammar term)
            {
                if (term != null)
                {
                    AddBranchesFrom(term);
                }
            }

            private void AddBranchesFrom(Grammar term)
            {
                if (_branchesFrom == null)
                {
                    _branchesFrom = term;
                }
                else if (_branchesFrom is Grammar g)
                {
                    if (term != g)
                    {
                        // items is unique by instance here
                        var hs = new HashSet<Grammar>();
                        hs.Add(g);
                        hs.Add(term);
                        _branchesFrom = hs;
                    }
                }
                else if (_branchesFrom is HashSet<Grammar> hset)
                {
                    hset.Add(term);
                }
            }

            private IEnumerable<Grammar> GetBranchesFrom()
            {
                if (_branchesFrom is Grammar g)
                {
                    return new[] { g };
                }
                else if (_branchesFrom is HashSet<Grammar> hs)
                {
                    return hs;
                }
                else
                {
                    return EmptyList;
                }
            }

            private static readonly IReadOnlyList<Grammar> EmptyList = new List<Grammar>().AsReadOnly();

            private void AddBranchesTo(Grammar term, Node nextNode)
            {
                if (_branchesTo == null)
                {
                    _branchesTo = new KeyValuePair<Grammar, Node>(term, nextNode);
                }
                else if (_branchesTo is KeyValuePair<Grammar, Node> kvp)
                {
                    // keys don't have to be unique instances here, but similar
                    if (!GrammarEquivalenceComparer.Instance.Equals(kvp.Key, term))
                    {
                        var d = new Dictionary<Grammar, Node>(GrammarEquivalenceComparer.Instance);
                        d.Add(kvp.Key, kvp.Value);
                        d.Add(term, nextNode);
                        _branchesTo = d;
                    }
                }
                else if (_branchesTo is Dictionary<Grammar, Node> bd)
                {
                    bd.Add(term, nextNode);
                }
            }

            private bool TryGetBranchTo(Grammar term, out Node node)
            {
                if (_branchesTo is KeyValuePair<Grammar, Node> kvp)
                {
                    if (GrammarEquivalenceComparer.Instance.Equals(kvp.Key, term))
                    {
                        node = kvp.Value;
                        return true;
                    }
                    else
                    {
                        node = null;
                        return false;
                    }
                }
                else if (_branchesTo is Dictionary<Grammar, Node> d)
                {
                    return d.TryGetValue(term, out node);
                }
                else
                {
                    node = null;
                    return false;
                }
            }

            private IEnumerable<Node> GetBranchesTo()
            {
                if (_branchesTo is KeyValuePair<Grammar, Node> kvp)
                {
                    return new[] { kvp.Value };
                }
                else if (_branchesTo is Dictionary<Grammar, Node> d)
                {
                    return d.Values;
                }
                else
                {
                    return EmptyNodeList;
                }
            }

            private static readonly IReadOnlyList<Node> EmptyNodeList = new List<Node>().AsReadOnly();

            public Node AddTerm(Grammar term)
            {
                if (!TryGetBranchTo(term, out var nextNode))
                {
                    nextNode = new Node(term);
                    AddBranchesTo(term, nextNode);
                }
                else
                {
                    nextNode.AddBranchesFrom(term);
                }

                return nextNode;
            }

            public Node GetNext(Grammar term)
            {
                TryGetBranchTo(term, out var nextNode);
                return nextNode;
            }

            public void GetUniqueTerms(HashSet<Grammar> terms)
            {
                if (_branchesFrom is Grammar g)
                {
                    // we only get here from one grammar
                    terms.Add(g);
                }
                else
                {
                    foreach (var node in GetBranchesTo())
                    {
                        node.GetUniqueTerms(terms);
                    }
                }
            }

            public void GetNthTerms(int n, List<Grammar> terms)
            {
                if (n >= 0)
                {
                    foreach (var node in GetBranchesTo())
                    {
                        node.GetNthTerms(n - 1, terms);
                    }
                }
                else
                {
                    terms.AddRange(GetBranchesFrom());
                }
            }

            public void GetAlternativeTerms(Dictionary<Grammar, IReadOnlyList<Grammar>> map)
            {
                var list = GetBranchesTo().SelectMany(n => n.GetBranchesFrom()).ToList();

                foreach (var term in list)
                {
                    if (!map.TryGetValue(term, out var existingList))
                    {
                        map[term] = list;
                    }
                }

                foreach (var node in GetBranchesTo())
                {
                    node.GetAlternativeTerms(map);
                }
            }
        }
    }

    #endregion

#if !T4
}
#endif
// #>