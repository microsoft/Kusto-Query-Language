using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kusto.Language.Syntax;
using Kusto.Language.Utils;

namespace Kusto.Language.Parsing
{
    public class QueryParser
    {
        private readonly LexicalToken[] _tokens;
        private readonly Source<LexicalToken> _source;
        private int _pos;

        private QueryParser(LexicalToken[] tokens, int start = 0)
        {
            _tokens = tokens;
            _source = new ArraySource<LexicalToken>(tokens);
            _pos = start;
        }

        public static Expression ParseExpression(LexicalToken[] tokens, int start = 0)
        {
            return new QueryParser(tokens, start).ParseExpression();
        }
       
        public static Expression ParseExpression(string text)
        {
            return ParseExpression(TokenParser.ParseTokens(text));
        }

        public static QueryBlock ParseQuery(LexicalToken[] tokens, int start = 0)
        {
            return new QueryParser(tokens, start).ParseQuery();
        }

        public static QueryBlock ParseQuery(string text)
        {
            return ParseQuery(TokenParser.ParseTokens(text));
        }

        public static FunctionParameters ParseFunctionParameters(LexicalToken[] tokens, int start = 0)
        {
            return new QueryParser(tokens, start).ParseFunctionParameters();
        }

        public static FunctionParameters ParseFunctionParameters(string text)
        {
            return ParseFunctionParameters(TokenParser.ParseTokens(text));
        }

        public static FunctionBody ParseFunctionBody(LexicalToken[] tokens, int start = 0)
        {
            return new QueryParser(tokens, start).ParseFunctionBody();
        }

        public static FunctionBody ParseFunctionBody(string text)
        {
            return ParseFunctionBody(TokenParser.ParseTokens(text));
        }

        public static Expression ParseLiteral(LexicalToken[] tokens, int start = 0)
        {
            return new QueryParser(tokens, start).ParseLiteral();
        }

        public static Expression ParseLiteral(string text)
        {
            return ParseLiteral(TokenParser.ParseTokens(text));
        }

        public static SchemaTypeExpression ParseSchemaType(LexicalToken[] tokens, int start = 0)
        {
            return new QueryParser(tokens, start).ParseSchemaType();
        }

        public static SchemaTypeExpression ParseSchemaType(string text)
        {
            return ParseSchemaType(TokenParser.ParseTokens(text));
        }

        #region Reset Points

        private int GetResetPoint()
        {
            return _pos;
        }

        private void Reset(int resetPoint)
        {
            _pos = resetPoint;
        }

        #endregion

        #region Tokens

        /// <summary>
        /// Returns the next <see cref="LexicalToken"/>
        /// </summary>
        private LexicalToken PeekToken()
        {
            return _pos < _tokens.Length ? _tokens[_pos] : NoToken;
        }

        /// <summary>
        /// Returns the next <see cref="LexicalToken"/>
        /// </summary>
        private LexicalToken PeekToken(int offset)
        {
            var index = _pos + offset;
            return index < _tokens.Length ? _tokens[index] : NoToken;
        }

        private static readonly LexicalToken NoToken =
            new LexicalToken(SyntaxKind.None, "", "");

        /// <summary>
        /// Returns the next <see cref="LexicalToken"/> as a <see cref="SyntaxToken"/>,
        /// or null if there are no more tokens.
        /// </summary>
        private SyntaxToken ParseToken()
        {
            var tok = PeekToken();
            if (tok.Kind != SyntaxKind.None)
            {
                _pos++;
                return SyntaxToken.From(tok);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the next <see cref="SyntaxToken"/> if it matches the kind or null.
        /// </summary>
        private SyntaxToken ParseToken(SyntaxKind kind)
        {
            if (PeekToken().Kind == kind)
            {
                return ParseToken();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the next <see cref="SyntaxToken"/> if it matches one of the kinds or null.
        /// </summary>
        private SyntaxToken ParseToken(IReadOnlyList<SyntaxKind> kinds)
        {
            if (kinds.Contains(PeekToken().Kind))
            {
                return ParseToken();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the next <see cref="SyntaxToken"/> if it matches the kind or a missing version of that token kind.
        /// </summary>
        private SyntaxToken ParseRequiredToken(SyntaxKind kind)
        {
            return ParseToken(kind) ?? CreateMissingToken(kind);
        }

        private static SyntaxToken CreateMissingToken(SyntaxKind kind)
        {
            return SyntaxParsers.CreateMissingToken(kind);
        }

        /// <summary>
        /// Returns the next <see cref="SyntaxToken"/> if it matches one of the kinds or a missing version of that token kind.
        /// </summary>
        private SyntaxToken ParseRequiredToken(IReadOnlyList<SyntaxKind> kinds)
        {
            return ParseToken(kinds) ?? CreateMissingToken(kinds);
        }

        private static SyntaxToken CreateMissingToken(IReadOnlyList<SyntaxKind> kinds)
        {
            return SyntaxParsers.CreateMissingToken(kinds);
        }

        /// <summary>
        /// Scans one or more adjacent lexical tokens that together matches the text.
        /// </summary>
        private int ScanToken(string text, int offset = 0)
        {
            return SyntaxParsers.MatchesText(_source, _pos + offset, text);
        }

        /// <summary>
        /// Scans one or more adjacent lexical tokens that together matches the one of the texts.
        /// </summary>
        private int ScanToken(IReadOnlyList<string> texts, int offset = 0)
        {
            for (int i = 0; i < texts.Count; i++)
            {
                var len = ScanToken(texts[i], offset);
                if (len > 0)
                    return len;
            }

            return -1;
        }

        /// <summary>
        /// Parses one or more adjacent lexical tokens that together matches the text into a single token,
        /// or returns null.
        /// </summary>
        private SyntaxToken ParseToken(string text)
        {
            var len = SyntaxParsers.MatchesText(_source, _pos, text);
            if (len > 0)
            {
                var token = SyntaxParsers.ProduceSyntaxToken(_source, _pos, len, text);
                _pos += len;
                return token;
            }

            return null;
        }

        private SyntaxToken ParseToken(IReadOnlyList<string> texts)
        {
            for (int i = 0; i < texts.Count; i++)
            {
                var token = ParseToken(texts[i]);
                if (token != null)
                    return token;
            }

            return null;
        }

        private SyntaxToken ParseCombinedIdentifier(int count)
        {
            if (count == 1)
            {
                return ParseToken();
            }
            else
            {
                var builder = new StringBuilder();

                var firstToken = PeekToken();
                builder.Append(firstToken.Text);

                for (int i = 1; i < count; i++)
                {
                    var tok = PeekToken(i);
                    builder.Append(tok.Trivia);
                    builder.Append(tok.Text);
                }

                _pos += count;

                return SyntaxToken.Identifier(firstToken.Trivia, builder.ToString());
            }
        }

        /// <summary>
        /// Parses one or more adjacent lexical tokens that together matches the text into a single token,
        /// or returns a missing token.
        /// </summary>
        private SyntaxToken ParseRequiredToken(string text)
        {
            return ParseToken(text) ?? SyntaxParsers.CreateMissingToken(text);
        }

        private SyntaxToken ParseRequiredToken(IReadOnlyList<string> texts)
        {
            return ParseToken(texts) ?? SyntaxParsers.CreateMissingToken(texts);
        }

        private bool ScanIdentifierOrKeywordAsIdentifier(int offset = 0)
        {
            var token = PeekToken(offset);
            return token.Kind == SyntaxKind.IdentifierToken
                || (token.Kind.IsKeyword() && token.Kind.CanBeIdentifier());
        }

        private SyntaxToken ParseIdentiferOrKeywordAsIdentifier()
        {
            var kind = PeekToken().Kind;
            if (kind == SyntaxKind.IdentifierToken
                || (kind.IsKeyword() && kind.CanBeIdentifier()))
            {
                return ParseToken();
            }
            else
            {
                return null;
            }
        }

        private Expression ParseIdentifierOrKeywordTokenLiteral()
        {
            var token = ParseIdentiferOrKeywordAsIdentifier();
            if (token != null)
            {
                return new LiteralExpression(SyntaxKind.TokenLiteralExpression, token);
            }

            return null;
        }

        #endregion

        #region Missing Nodes

        // CreateXXX functions are specified as Func<T> so they can be passed to ParseCommaList & ParseList
        // without causing possible delegate allocations in translation to JavaScript

        private static readonly Func<NameDeclaration> CreateMissingNameDeclaration = () =>
            new NameDeclaration(SyntaxToken.Missing(SyntaxKind.IdentifierToken), new[] { DiagnosticFacts.GetMissingName() });

        private static readonly Func<NameReference> CreateMissingNameReference = () =>
            new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken), new[] { DiagnosticFacts.GetMissingName() });

        private static readonly Func<Expression> CreateMissingNameReferenceExpression = () =>
            CreateMissingNameReference();

        private static SyntaxToken CreateMissingNameToken(IReadOnlyList<string> texts) =>
            SyntaxToken.Missing(SyntaxKind.IdentifierToken, DiagnosticFacts.GetTokenExpected(texts));

        private static readonly Func<Expression> CreateMissingStringLiteral = () =>
            new LiteralExpression(
                SyntaxKind.StringLiteralExpression,
                SyntaxToken.Missing(SyntaxKind.StringLiteralToken),
                new[] { DiagnosticFacts.GetMissingString() });

        private static readonly Func<Expression> CreateMissingBoolLiteral = () =>
            new LiteralExpression(
                SyntaxKind.StringLiteralExpression,
                SyntaxToken.Missing(SyntaxKind.BooleanLiteralToken),
                new[] { DiagnosticFacts.GetMissingBoolean() });

        private static readonly Func<Expression> CreateMissingLongLiteral = () =>
            new LiteralExpression(SyntaxKind.LongLiteralExpression,
                SyntaxToken.Missing(SyntaxKind.LongLiteralToken),
                new[] { DiagnosticFacts.GetMissingNumber() });

        private static Expression CreateMissingTokenLiteral(IReadOnlyList<string> tokens = null) =>
            new LiteralExpression(SyntaxKind.TokenLiteralExpression,
                SyntaxToken.Missing(SyntaxKind.IdentifierToken),
                new[] { tokens != null && tokens.Count > 0
                    ? DiagnosticFacts.GetTokenExpected(tokens)
                    : DiagnosticFacts.GetTokenExpected("token") });

        private static Expression CreateMissingTypeOfLiteral() =>
            new TypeOfLiteralExpression(
                SyntaxToken.Missing(SyntaxKind.TypeOfKeyword),
                SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                SyntaxList<SeparatedElement<Expression>>.Empty(),
                SyntaxToken.Missing(SyntaxKind.CloseParenToken),
                new[] { DiagnosticFacts.GetMissingTypeOfLiteral() });

        private static readonly Func<Expression> CreateMissingJsonValue = () =>
            new LiteralExpression(
                SyntaxKind.StringLiteralExpression,
                SyntaxToken.Missing(SyntaxKind.StringLiteralToken),
                new[] { DiagnosticFacts.GetMissingJsonValue() });

        private static readonly Func<JsonPair> CreateMissingJsonPair = () =>
            new JsonPair(
                SyntaxToken.Missing(SyntaxKind.StringLiteralToken),
                SyntaxToken.Missing(SyntaxKind.ColonToken),
                new LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxToken.Missing(SyntaxKind.StringLiteralToken)),
                new[] { DiagnosticFacts.GetMissingJsonPair() });

        private static readonly Func<TypeExpression> CreateMissingType = () =>
            new PrimitiveTypeExpression(SyntaxToken.Missing(SyntaxKind.IdentifierToken), new[] { DiagnosticFacts.GetMissingTypeName() });

        private static readonly Func<Expression> CreateMissingTypeExpression = () =>
            CreateMissingType();

        private static readonly Func<NameAndTypeDeclaration> CreateMissingNameAndTypeDeclaration = () =>
            new NameAndTypeDeclaration(
                new NameDeclaration(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                SyntaxToken.Missing(SyntaxKind.ColonToken),
                new PrimitiveTypeExpression(SyntaxToken.Missing(SyntaxKind.StringKeyword)),
                new[] { DiagnosticFacts.GetMissingParameter() });

        private static readonly Func<Expression> CreateMissingNameAndTypeDeclarationExpression = () =>
            CreateMissingNameAndTypeDeclaration();

        private static readonly Func<Expression> CreateMissingValue = () =>
            new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken), new[] { DiagnosticFacts.GetMissingValue() });

        private static readonly Func<Expression> CreateMissingExpression = () =>
            new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken), new[] { DiagnosticFacts.GetMissingExpression() });

        private static readonly Func<SchemaTypeExpression> CreateMissingSchema = () =>
            new SchemaTypeExpression(
                SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                SyntaxList<SeparatedElement<Expression>>.Empty(),
                SyntaxToken.Missing(SyntaxKind.CloseParenToken),
                new[] { DiagnosticFacts.GetMissingSchemaDeclaration() });

        private static readonly Func<NamedParameter> CreateMissingNamedParameter = () =>
            new NamedParameter(
                new NameDeclaration(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                SyntaxToken.Missing(SyntaxKind.EqualToken),
                new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                diagnostics: new[] { DiagnosticFacts.GetMissingParameter() });

        private static FunctionCallExpression CreateMissingFunctionCallExpression() =>
            new FunctionCallExpression(
                new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                new ExpressionList(
                    SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                    SyntaxList<SeparatedElement<Expression>>.Empty(),
                    SyntaxToken.Missing(SyntaxKind.CloseParenToken)),
                new[] { DiagnosticFacts.GetMissingFunctionCall() });

        private static MaterializedViewCombineClause CreateMissingMaterializedViewCombineClause(string name) =>
            new MaterializedViewCombineClause(
                SyntaxToken.Missing(SyntaxKind.MaterializedViewCombineClause),
                SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                SyntaxToken.Missing(SyntaxKind.CloseParenToken),
                new[] { DiagnosticFacts.GetMissingClause(name) });

        private static readonly Func<QueryOperator> CreateMissingQueryOperator = () =>
            new BadQueryOperator(SyntaxToken.Missing(SyntaxKind.IdentifierToken), new[] { DiagnosticFacts.GetQueryOperatorExpected() });

        private static readonly Func<Expression> CreateMissingQueryOperatorExpression = () =>
            new BadQueryOperator(SyntaxToken.Missing(SyntaxKind.IdentifierToken), new[] { DiagnosticFacts.GetQueryOperatorExpected() });

        #endregion

        #region Lists

        private SyntaxList<SeparatedElement<TElement>> ParseCommaList<TElement>(
            Func<QueryParser, TElement> elementParser,
            Func<TElement> createMissingElement = null,
            Func<QueryParser, bool> fnScanEnd = null,
            bool oneOrMore = false,
            bool allowTrailingComma = false)
            where TElement : SyntaxElement
        {
            var list = new List<SeparatedElement<TElement>>();

            var element = (fnScanEnd == null || !fnScanEnd(this))
                ? elementParser(this)
                : null;

            if (element == null
                && PeekToken().Kind == SyntaxKind.CommaToken
                && createMissingElement != null)
            {
                element = createMissingElement();
            }

            while (element != null)
            {
                var token = PeekToken();
                var kind = token.Kind;

                if (kind == SyntaxKind.EndOfTextToken
                    || kind == SyntaxKind.None
                    || (fnScanEnd != null && fnScanEnd(this)))
                {
                    list.Add(new SeparatedElement<TElement>(element, null));
                    break;
                }
                else if (kind == SyntaxKind.CommaToken)
                {
                    var comma = ParseToken();
                    list.Add(new SeparatedElement<TElement>(element, comma));

                    if (createMissingElement != null)
                    {
                        element = (fnScanEnd == null || !fnScanEnd(this))
                            ? elementParser(this)
                            : null;

                        if (element != null)
                        {
                            continue;
                        }
                        else if (allowTrailingComma)
                        {
                            break;
                        }
                        else
                        {
                            list.Add(new SeparatedElement<TElement>(createMissingElement(), null));
                        }
                    }

                    break;
                }
                else if (fnScanEnd != null)
                {
                    var nextElement = elementParser(this);
                    if (nextElement != null)
                    {
                        list.Add(new SeparatedElement<TElement>(element, SyntaxParsers.CreateMissingToken(SyntaxKind.CommaToken)));
                    }
                    else
                    {
                        list.Add(new SeparatedElement<TElement>(element, null));
                    }

                    element = nextElement;
                    continue;
                }
                else
                {
                    // no command and no end function.. so just be done
                    list.Add(new SeparatedElement<TElement>(element, null));
                    break;
                }
            }


            if (oneOrMore && list.Count == 0 && createMissingElement != null)
            {
                list.Add(new SeparatedElement<TElement>(createMissingElement()));
            }

            return new SyntaxList<SeparatedElement<TElement>>(list);
        }

        private SyntaxList<TElement> ParseList<TElement>(
            Func<QueryParser, TElement> elementParser,
            Func<TElement> createMissingElement = null,
            Func<QueryParser, bool> fnScanEnd = null,
            bool oneOrMore = false)
            where TElement : SyntaxElement
        {
            var list = new List<TElement>();

            while (fnScanEnd == null || !fnScanEnd(this))
            {
                var element = elementParser(this);
                if (element == null)
                    break;
                list.Add(element);
            }

            if (oneOrMore && list.Count == 0 && createMissingElement != null)
            {
                list.Add(createMissingElement());
            }

            return new SyntaxList<TElement>(list);
        }

        private static Func<QueryParser, bool> FnScanCommonListEnd =
            qp => qp.ScanCommonListEnd();

        private bool ScanCommonListEnd(int offset = 0)
        {
            switch (PeekToken(offset).Kind)
            {
                case SyntaxKind.CloseParenToken:
                case SyntaxKind.CloseBracketToken:
                case SyntaxKind.CloseBraceToken:
                case SyntaxKind.BarToken:
                case SyntaxKind.SemicolonToken:
                case SyntaxKind.EndOfTextToken:
                case SyntaxKind.None:
                    return true;
                default:
                    return false;
            }
        }

        private bool ScanCustomListEnd(SyntaxKind kind, int offset = 0)
        {
            if (PeekToken(offset).Kind == kind)
                return true;
            return ScanCommonListEnd();
        }

        private bool ScanCustomListEnd(IReadOnlyList<SyntaxKind> kinds, int offset = 0)
        {
            var kind = PeekToken(offset).Kind;

            for (int i = 0; i < kinds.Count; i++)
            {
                if (kind == kinds[i])
                    return true;
            }

            return ScanCommonListEnd();
        }

        #endregion

        #region Names

        private Name ParseName()
        {
            switch (PeekToken().Kind)
            {
                case SyntaxKind.OpenBracketToken:
                    return ParseBracketedName();
                case SyntaxKind.OpenBraceToken:
                    return ParseClientParameterName();
                default:
                    return ParseIdentifierName();
            }
        }

        private Name ParseIdentifierName()
        {
            var tok = PeekToken();
            if (tok.Kind == SyntaxKind.IdentifierToken
                || (tok.Kind.IsKeyword() && tok.Kind.CanBeIdentifier()))
            {
                return new TokenName(ParseToken());
            }

            return null;
        }

        private Name ParseBracketedName()
        {
            if (ScanBracketedName() > 0)
            {
                var open = ParseToken();
                var expr = ParseStringOrCompoundStringLiteral();
                var close = ParseRequiredToken(SyntaxKind.CloseBracketToken);
                return new BracketedName(open, expr, close);
            }

            return null;
        }

        private int ScanName(int offset = 0)
        {
            var tok = PeekToken(offset);
            switch (tok.Kind)
            {
                case SyntaxKind.IdentifierToken:
                    return 1;
                case SyntaxKind.OpenBracketToken:
                    return ScanBracketedName(offset);
                case SyntaxKind.OpenBraceToken:
                    if (ScanIdentifierOrKeywordAsIdentifier(offset + 1)
                        && PeekToken(offset + 2).Kind == SyntaxKind.CloseBraceToken)
                    {
                        return 3;
                    }
                    return -1;
                default:
                    return ScanIdentifierOrKeywordAsIdentifier(offset) ? 1 : -1;
            }
        }

        private int ScanBracketedName(int offset = 0)
        {
            if (PeekToken(offset).Kind == SyntaxKind.OpenBracketToken
                && ScanStringOrCompoundStringLiteral(offset + 1) is int len
                && len > 0
                && PeekToken(offset + 1 + len).Kind == SyntaxKind.CloseBracketToken)
            {
                return len + 2;
            }
            else
            {
                return -1;
            }
        }

        private int ScanWildcardedName(int offset = 0)
        {
            var start = offset;

            var token = PeekToken(offset);

            if (token.Kind == SyntaxKind.AsteriskToken)
            {
                offset++;
            }
            else if ((token.Kind == SyntaxKind.IdentifierToken || token.Kind.IsKeyword())
                && PeekToken(offset + 1).Kind == SyntaxKind.AsteriskToken)
            {
                offset += 2;
            }
            else
            {
                return -1;
            }

            while (
                ((token = PeekToken(offset)).Kind == SyntaxKind.IdentifierToken
                    || token.Kind == SyntaxKind.LongLiteralToken
                    || token.Kind == SyntaxKind.AsteriskToken
                    || token.Kind.IsKeyword())
                && token.Trivia.Length == 0)
            {
                offset++;
            }

            return offset > start ? offset - start : -1;
        }

        private SyntaxToken ParseWildcardedIdentifier()
        {
            var len = ScanWildcardedName();
            if (len > 0)
            {
                var lit = SyntaxToken.Identifier(PeekToken().Trivia, GetTokenText(0, len));
                _pos += len;
                return lit;
            }

            return null;
        }

        private NameReference ParseWildcardedNameReference()
        {
            var id = ParseWildcardedIdentifier();
            if (id != null)
            {
                return new NameReference(new WildcardedName(id));
            }
            return null;
        }

        private bool ScanBracketedWildcardedName(int offset = 0)
        {
            return PeekToken(offset).Kind == SyntaxKind.OpenBracketToken
                && ScanWildcardedName(offset + 1) > 0;
        }

        private NameReference ParseBracketedWildcardedNameReference()
        {
            if (ScanBracketedWildcardedName())
            {
                var open = ParseToken();
                var wildcard = ParseWildcardedIdentifier();
                var close = ParseRequiredToken(SyntaxKind.CloseBracketToken);
                return new NameReference(new BracketedWildcardedName(open, wildcard, close));
            }

            return null;
        }

        private string GetTokenText(int start = 0, int length = 1)
        {
            if (length == 1)
            {
                return PeekToken(start).Text;
            }

            var builder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                builder.Append(PeekToken(start + i).Text);
            }

            return builder.ToString();
        }

        private NameReference ParseBracketedNameReference()
        {
            var name = ParseBracketedName();
            if (name != null)
            {
                return new NameReference(name);
            }

            return null;
        }

        private NameReference ParseNameReference()
        {
            var name = ParseName();
            return name != null ? new NameReference(name) : null;
        }

        private NameDeclaration ParseNameDeclaration()
        {
            var name = ParseName();
            return name != null ? new NameDeclaration(name) : null;
        }

        private bool ScanClientParameterName(int offset = 0)
        {
            return PeekToken(offset).Kind == SyntaxKind.OpenBraceToken
                && PeekToken(offset + 1) is LexicalToken name
                && (name.Kind == SyntaxKind.IdentifierToken || (name.Kind.IsKeyword() && name.Kind.CanBeIdentifier()))
                && name.Trivia.Length == 0
                && PeekToken(offset + 2) is LexicalToken close
                && close.Kind == SyntaxKind.CloseBraceToken
                && close.Trivia.Length == 0;
        }

        private Name ParseClientParameterName()
        {
            if (ScanClientParameterName())
            {
                return new BracedName(ParseToken(), ParseToken(), ParseToken());
            }

            return null;
        }

        private Expression ParseClientParameterReference()
        {
            var name = ParseClientParameterName();
            return name != null ? new NameReference(name) : null;
        }

        #endregion

        #region Literals

        private int ScanStringOrCompoundStringLiteral(int offset = 0)
        {
            var start = offset;
            while (PeekToken(offset).Kind == SyntaxKind.StringLiteralToken)
            {
                offset++;
            }

            return offset > start ? offset - start : -1;
        }

        private Expression ParseStringOrCompoundStringLiteral()
        {
            if (PeekToken().Kind == SyntaxKind.StringLiteralToken)
            {
                if (PeekToken(1).Kind != SyntaxKind.StringLiteralToken)
                {
                    return new LiteralExpression(SyntaxKind.StringLiteralExpression, ParseToken());
                }

                var tokens = new List<SyntaxToken>();
                while (PeekToken().Kind == SyntaxKind.StringLiteralToken)
                {
                    tokens.Add(ParseToken());
                }

                return new CompoundStringLiteralExpression(new SyntaxList<SyntaxToken>(tokens));
            }

            return null;
        }

        private Expression ParseLiteral()
        {
            switch (PeekToken().Kind)
            {
                case SyntaxKind.StringLiteralToken:
                    return ParseStringOrCompoundStringLiteral();
                case SyntaxKind.BooleanLiteralToken:
                    return new LiteralExpression(SyntaxKind.BooleanLiteralExpression, ParseToken());
                case SyntaxKind.LongLiteralToken:
                    return new LiteralExpression(SyntaxKind.LongLiteralExpression, ParseToken());
                case SyntaxKind.RealLiteralToken:
                    return new LiteralExpression(SyntaxKind.RealLiteralExpression, ParseToken());
                case SyntaxKind.DecimalLiteralToken:
                    return new LiteralExpression(SyntaxKind.DecimalLiteralExpression, ParseToken());
                case SyntaxKind.IntLiteralToken:
                    return new LiteralExpression(SyntaxKind.IntLiteralExpression, ParseToken());
                case SyntaxKind.GuidLiteralToken:
                    return new LiteralExpression(SyntaxKind.GuidLiteralExpression, ParseToken());
                case SyntaxKind.DateTimeLiteralToken:
                    return new LiteralExpression(SyntaxKind.DateTimeLiteralExpression, ParseToken());
                case SyntaxKind.TimespanLiteralToken:
                    return new LiteralExpression(SyntaxKind.TimespanLiteralExpression, ParseToken());
                case SyntaxKind.DynamicKeyword:
                    return ParseDynamicLiteral();
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return ParseSignedNumericLiteral();
                case SyntaxKind.TypeOfKeyword:
                    return ParseTypeOfLiteral();
                case SyntaxKind.OpenBraceToken:
                    return ParseClientParameterReference();
            }

            return null;
        }

        private static Func<QueryParser, Expression> FnParseLiteral =
            qp => qp.ParseLiteral();

        private int ScanSignedNumericLiteral(int offset = 0)
        {
            var sign = PeekToken(offset);
            var number = PeekToken(offset + 1);
            if ((sign.Kind == SyntaxKind.PlusToken || sign.Kind == SyntaxKind.MinusToken)
                && number.Trivia.Length == 0
                && (number.Kind == SyntaxKind.LongLiteralToken
                || number.Kind == SyntaxKind.RealLiteralToken)
                && number.Text.Length > 0 && char.IsDigit(number.Text[0]))
            {
                return 2;
            }
            else
            {
                return -1;
            }
        }

        private Expression ParseSignedNumericLiteral()
        {
            var len = ScanSignedNumericLiteral();
            if (len == 2)
            {
                var sign = ParseToken();
                var number = ParseToken();
                var signedNumberToken = SyntaxToken.Literal(sign.Trivia, sign.Text + number.Text, number.Kind);

                switch (number.Kind)
                {
                    case SyntaxKind.LongLiteralToken:
                        return new LiteralExpression(SyntaxKind.LongLiteralExpression, signedNumberToken);
                    case SyntaxKind.RealLiteralToken:
                        return new LiteralExpression(SyntaxKind.RealLiteralExpression, signedNumberToken);
                }
            }

            return null;
        }

        private Expression ParseNumericLiteral()
        {
            var token = PeekToken();
            switch (token.Kind)
            {
                case SyntaxKind.LongLiteralToken:
                    return new LiteralExpression(SyntaxKind.LongLiteralExpression, ParseToken());
                case SyntaxKind.RealLiteralToken:
                    return new LiteralExpression(SyntaxKind.RealLiteralExpression, ParseToken());
                case SyntaxKind.DecimalLiteralToken:
                    return new LiteralExpression(SyntaxKind.DecimalLiteralExpression, ParseToken());
                case SyntaxKind.IntLiteralToken:
                    return new LiteralExpression(SyntaxKind.IntLiteralExpression, ParseToken());
                case SyntaxKind.DateTimeLiteralToken:
                    return new LiteralExpression(SyntaxKind.DateTimeLiteralExpression, ParseToken());
                case SyntaxKind.TimespanLiteralToken:
                    return new LiteralExpression(SyntaxKind.TimespanLiteralExpression, ParseToken());
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return ParseSignedNumericLiteral();
            }

            return null;
        }

        private Expression ParseDynamicLiteral()
        {
            if (PeekToken().Kind == SyntaxKind.DynamicKeyword
                && PeekToken(1).Kind == SyntaxKind.OpenParenToken)
            {
                var keyword = ParseToken();
                var open = ParseToken();
                var value = ParseJsonValue() ?? CreateMissingJsonValue();
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new DynamicExpression(keyword, open, value, close);
            }

            return null;
        }

        private Expression ParseJsonValue()
        {
            switch (PeekToken().Kind)
            {
                case SyntaxKind.LongLiteralToken:
                case SyntaxKind.RealLiteralToken:
                case SyntaxKind.MinusToken:
                    return ParseJsonNumber();
                case SyntaxKind.BooleanLiteralToken:
                case SyntaxKind.DateTimeLiteralToken:
                case SyntaxKind.TimespanLiteralToken:
                case SyntaxKind.GuidLiteralToken:
                case SyntaxKind.DecimalLiteralToken:
                case SyntaxKind.StringLiteralToken:
                case SyntaxKind.DynamicKeyword:
                    return ParseLiteral();
                case SyntaxKind.NullKeyword:
                    return ParseNullLiteral();
                case SyntaxKind.OpenBracketToken:
                    return ParseJsonArray();
                case SyntaxKind.OpenBraceToken:
                    if (ScanClientParameterName())
                    {
                        return ParseClientParameterReference();
                    }
                    return ParseJsonObject();
            }

            return null;
        }

        private Expression ParseJsonNumber()
        {
            switch (PeekToken().Kind)
            {
                case SyntaxKind.LongLiteralToken:
                case SyntaxKind.RealLiteralToken:
                    return ParseLiteral();
                case SyntaxKind.MinusToken:
                    var numberToken = PeekToken(1);
                    if (numberToken.Kind == SyntaxKind.LongLiteralToken
                        || numberToken.Kind == SyntaxKind.RealLiteralToken)
                    {
                        var op = ParseToken();
                        var literal = ParseLiteral();
                        return new PrefixUnaryExpression(SyntaxKind.UnaryMinusExpression, op, literal);
                    }
                    break;
            }

            return null;
        }

        private static Func<QueryParser, Expression> FnParseJsonValue =
            qp => qp.ParseJsonValue();

        private Expression ParseJsonArray()
        {
            if (PeekToken().Kind == SyntaxKind.OpenBracketToken)
            {
                var open = ParseToken();
                var list = ParseCommaList(FnParseJsonValue, CreateMissingJsonValue, FnScanCommonListEnd);
                var close = ParseRequiredToken(SyntaxKind.CloseBracketToken);
                return new JsonArrayExpression(open, list, close);
            }

            return null;
        }

        private static Func<QueryParser, JsonPair> FnParseJsonPair =
            qp => qp.ParseJsonPair();

        private Expression ParseJsonObject()
        {
            if (PeekToken().Kind == SyntaxKind.OpenBraceToken)
            {
                var open = ParseToken();
                var list = ParseCommaList(FnParseJsonPair, CreateMissingJsonPair, FnScanCommonListEnd);
                var close = ParseRequiredToken(SyntaxKind.CloseBraceToken);
                return new JsonObjectExpression(open, list, close);
            }

            return null;
        }

        private JsonPair ParseJsonPair()
        {
            switch (PeekToken().Kind)
            {
                case SyntaxKind.StringLiteralToken:
                    var name = ParseToken();
                    var colon = ParseRequiredToken(SyntaxKind.ColonToken);
                    var value = ParseJsonValue() ?? CreateMissingJsonValue();
                    return new JsonPair(name, colon, value);
                case SyntaxKind.ColonToken:
                    name = SyntaxToken.Missing(SyntaxKind.StringLiteralToken, DiagnosticFacts.GetMissingString());
                    colon = ParseToken();
                    value = ParseJsonValue() ?? CreateMissingJsonValue();
                    return new JsonPair(name, colon, value);
                default:
                    value = ParseJsonValue();
                    if (value != null)
                    {
                        name = SyntaxToken.Missing(SyntaxKind.StringLiteralToken, DiagnosticFacts.GetMissingString());
                        colon = SyntaxParsers.CreateMissingToken(SyntaxKind.ColonToken);
                        return new JsonPair(name, colon, value);
                    }
                    else
                    {
                        return null;
                    }
            }
        }

        private Expression ParseNullLiteral()
        {
            var nullToken = ParseToken(SyntaxKind.NullKeyword);
            return nullToken != null ? new LiteralExpression(SyntaxKind.NullLiteralExpression, nullToken) : null;
        }

        private bool ScanTypeOfScalar(int offset = 0)
        {
            return PeekToken(offset).Kind == SyntaxKind.TypeOfKeyword
                && PeekToken(offset + 1).Kind == SyntaxKind.OpenParenToken
                && (ScanParamTypeExtended(offset + 2)
                     || (PeekToken(offset + 2).Kind == SyntaxKind.IdentifierToken
                         && PeekToken(offset + 3).Kind == SyntaxKind.CloseParenToken));
        }

        private Expression ParseTypeOfLiteral()
        {
            if (PeekToken().Kind == SyntaxKind.TypeOfKeyword)
            {
                if (ScanTypeOfScalar())
                {
                    var keyword = ParseToken();
                    var open = ParseRequiredToken(SyntaxKind.OpenParenToken);
                    var type = ParseParamTypeExtended() ?? ParseIdentifierTypeExpression() ?? CreateMissingType();
                    var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                    return new TypeOfLiteralExpression(keyword, open, new SyntaxList<SeparatedElement<Expression>>(new SeparatedElement<Expression>(type)), close);
                }
                else
                {
                    var keyword = ParseToken();
                    var open = ParseRequiredToken(SyntaxKind.OpenParenToken);
                    var list = ParseCommaList(FnParseTypeOfElement, CreateMissingTypeExpression, FnScanCommonListEnd);
                    var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                    return new TypeOfLiteralExpression(keyword, open, list, close);
                }
            }

            return null;
        }

        private Expression ParseStarExpression()
        {
            var token = ParseToken(SyntaxKind.AsteriskToken);
            if (token != null)
            {
                return new StarExpression(token);
            }

            return null;
        }

        private Expression ParseTypeOfElement() =>
            ParseStarExpression()
            ?? ParseNameAndTypeDeclaration();

        private static Func<QueryParser, Expression> FnParseTypeOfElement =
            qp => qp.ParseTypeOfElement();

        #endregion

        #region Schemas
        private bool ScanParamType(int offset = 0)
        {
            switch (PeekToken(offset).Kind)
            {
                case SyntaxKind.BoolKeyword:
                case SyntaxKind.BooleanKeyword:
                case SyntaxKind.DateKeyword:
                case SyntaxKind.DateTimeKeyword:
                case SyntaxKind.DecimalKeyword:
                case SyntaxKind.DoubleKeyword:
                case SyntaxKind.DynamicKeyword:
                case SyntaxKind.GuidKeyword:
                case SyntaxKind.IntKeyword:
                case SyntaxKind.Int64Keyword:
                case SyntaxKind.Int8Keyword:
                case SyntaxKind.LongKeyword:
                case SyntaxKind.RealKeyword:
                case SyntaxKind.StringKeyword:
                case SyntaxKind.TimeKeyword:
                case SyntaxKind.TimespanKeyword:
                case SyntaxKind.UniqueIdKeyword:
                    return true;
                default:
                    return false;
            }
        }

        private bool ScanParamTypeExtended(int offset = 0)
        {
            if (ScanParamType(offset))
                return true;

            switch (PeekToken(offset).Kind)
            {
                case SyntaxKind.FloatKeyword:
                case SyntaxKind.Int16Keyword:
                case SyntaxKind.Int32Keyword:
                case SyntaxKind.SingleKeyword:
                case SyntaxKind.UIntKeyword:
                case SyntaxKind.UInt16Keyword:
                case SyntaxKind.UInt32Keyword:
                case SyntaxKind.UInt64Keyword:
                case SyntaxKind.UInt8Keyword:
                case SyntaxKind.ULongKeyword:
                    return true;
                default:
                    return false;
            }
        }

        private TypeExpression ParseParamType()
        {
            if (ScanParamType())
            {
                return new PrimitiveTypeExpression(ParseToken());
            }

            return null;
        }

        private TypeExpression ParseParamTypeExtended()
        {
            if (ScanParamTypeExtended())
            {
                return new PrimitiveTypeExpression(ParseToken());
            }

            return null;
        }

        private TypeExpression ParseIdentifierTypeExpression()
        {
            var kind = PeekToken().Kind;
            if (kind == SyntaxKind.IdentifierToken
                || (kind.IsKeyword() && kind.CanBeIdentifier()))
            {
                return new PrimitiveTypeExpression(ParseToken());
            }

            return null;
        }

        private NameAndTypeDeclaration ParseNameAndTypeDeclaration()
        {
            var name = ParseNameDeclaration();
            if (name != null)
            {
                var colon = ParseRequiredToken(SyntaxKind.ColonToken);

                var type = PeekToken().Kind == SyntaxKind.OpenParenToken
                    ? ParseSchemaType()
                    : ParseParamType() ?? ParseIdentifierTypeExpression() ?? CreateMissingType();

                return new NameAndTypeDeclaration(name, colon, type);
            }
            else if (PeekToken().Kind == SyntaxKind.ColonToken)
            {
                // name is missing
                var colon = ParseToken();
                var type = ParseParamType() ?? ParseIdentifierTypeExpression() ?? CreateMissingType();
                return new NameAndTypeDeclaration(CreateMissingNameDeclaration(), colon, type);
            }

            return null;
        }

        private Expression ParseNameAndOptionalTypeDeclaration()
        {
            if (PeekToken().Kind == SyntaxKind.ColonToken)
            {
                var colon = ParseToken();
                var type = ParseParamTypeExtended() ?? ParseIdentifierTypeExpression() ?? CreateMissingType();
                return new NameAndTypeDeclaration(CreateMissingNameDeclaration(), colon, type);
            }

            var name = ParseNameDeclaration();
            if (name != null && PeekToken().Kind == SyntaxKind.ColonToken)
            {
                var colon = ParseToken();
                var type = ParseParamTypeExtended() ?? ParseIdentifierTypeExpression() ?? CreateMissingType();
                return new NameAndTypeDeclaration(name, colon, type);
            }

            return name;
        }

        private bool ScanSchemaTypeStart(int offset = 0)
        {
            var len = ScanName(offset);
            return len > 0
                && PeekToken(offset + len + 1).Kind == SyntaxKind.ColonToken
                && PeekToken(offset + len + 2).Kind == SyntaxKind.OpenParenToken;
        }

        /// <summary>
        /// Parses a schema delaration:  (name: type, name: type, ...)
        /// </summary>
        private SchemaTypeExpression ParseSchemaType()
        {
            if (PeekToken().Kind == SyntaxKind.OpenParenToken)
            {
                if (PeekToken(1).Kind == SyntaxKind.AsteriskToken)
                {
                    var open = ParseToken();
                    var asterisk = new StarExpression(ParseToken());
                    var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                    return new SchemaTypeExpression(
                        open,
                        new SyntaxList<SeparatedElement<Expression>>(new SeparatedElement<Expression>(asterisk)),
                        close);
                }
                else
                {
                    return ParseSchemaMultipartType();
                }
            }

            return null;
        }

        private static Func<QueryParser, NameAndTypeDeclaration> FnParseNameAndTypeDeclaration =
            qp => qp.ParseNameAndTypeDeclaration();

        private static Func<QueryParser, Expression> FnParseNameAndTypeDeclarationExpression =
            qp => (Expression)qp.ParseNameAndTypeDeclaration();

        /// <summary>
        /// Parses a multi-part schema declaration:  (name: type, name: type, ...)
        /// </summary>
        private SchemaTypeExpression ParseSchemaMultipartType()
        {
            if (PeekToken().Kind == SyntaxKind.OpenParenToken)
            {
                var open = ParseToken();
                var list = ParseCommaList(FnParseNameAndTypeDeclarationExpression, CreateMissingNameAndTypeDeclarationExpression, FnScanCommonListEnd);
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new SchemaTypeExpression(open, list, close);
            }

            return null;
        }

        #endregion

        #region Non-Query Expressions

        private Expression ParsePrimaryExpression()
        {
            switch (PeekToken().Kind)
            {
                case SyntaxKind.LongLiteralToken:
                case SyntaxKind.RealLiteralToken:
                case SyntaxKind.BooleanLiteralToken:
                case SyntaxKind.IntLiteralToken:
                case SyntaxKind.DecimalLiteralToken:
                case SyntaxKind.TimespanLiteralToken:
                case SyntaxKind.DateTimeLiteralToken:
                case SyntaxKind.GuidLiteralToken:
                case SyntaxKind.StringLiteralToken:
                case SyntaxKind.DynamicKeyword:
                case SyntaxKind.TypeOfKeyword:
                    return ParseLiteral();
                case SyntaxKind.DataTableKeyword:
                    return ParseDataTableExpression();
                case SyntaxKind.ContextualDataTableKeyword:
                    return ParseContextualDataTableExpression();
                case SyntaxKind.ExternalDataKeyword:
                case SyntaxKind.External_DataKeyword:
                    return ParseExternalDataExpression();
                case SyntaxKind.MaterializedViewCombineKeyword:
                    return ParseMaterializedViewCombineExpression();
                case SyntaxKind.OpenParenToken:
                    return ParseParenthesizedExpression();
                case SyntaxKind.ToScalarKeyword:
                    return ParseToScalarExpression();
                default:
                    if (ScanFunctionCallStart())
                        return ParseDotCompositeFunctionCall();
                    return ParsePrimaryPathSelector();
            }
        }

        private static IReadOnlyDictionary<string, QueryOperatorParameter> s_dataTableParameters =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.DataTableParameters);

        private DataTableExpression ParseDataTableExpression()
        {
            var keyword = ParseToken(SyntaxKind.DataTableKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_dataTableParameters);
                var schema = ParseSchemaMultipartType() ?? CreateMissingSchema();
                var open = ParseRequiredToken(SyntaxKind.OpenBracketToken);
                var values = ParseCommaList(FnParseLiteral, CreateMissingValue, FnScanCommonListEnd, allowTrailingComma: true);
                var close = ParseRequiredToken(SyntaxKind.CloseBracketToken);
                return new DataTableExpression(keyword, parameters, schema, open, values, close);
            }

            return null;
        }

        private ContextualDataTableExpression ParseContextualDataTableExpression()
        {
            var keyword = ParseToken(SyntaxKind.ContextualDataTableKeyword);
            if (keyword != null)
            {
                var id = ParseLiteral() ?? ParseUnnamedExpression() ?? CreateMissingExpression();
                var schema = ParseSchemaMultipartType() ?? CreateMissingSchema();
                return new ContextualDataTableExpression(keyword, id, schema);
            }

            return null;
        }

        private static readonly IReadOnlyList<SyntaxKind> s_externalDataKeywords =
            new[] { SyntaxKind.ExternalDataKeyword, SyntaxKind.External_DataKeyword };

        private ExternalDataExpression ParseExternalDataExpression()
        {
            switch (PeekToken().Kind)
            {
                case SyntaxKind.ExternalDataKeyword:
                case SyntaxKind.External_DataKeyword:
                    var keyword = ParseToken();
                    var parameters = ParseQueryOperatorParameterList(s_dataTableParameters);
                    var schema = ParseSchemaMultipartType() ?? CreateMissingSchema();
                    var open = ParseRequiredToken(SyntaxKind.OpenBracketToken);
                    var values = ParseCommaList(FnParseLiteral, CreateMissingValue, FnScanCommonListEnd, allowTrailingComma: true);
                    var close = ParseRequiredToken(SyntaxKind.CloseBracketToken);
                    var clause = ParseExternalDataWithClause();
                    return new ExternalDataExpression(keyword, parameters, schema, open, values, close, clause);

                default:
                    return null;
            }
        }

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_externalDataWithClauseParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.ExternalDataWithClauseProperties);

        private ExternalDataWithClause ParseExternalDataWithClause()
        {
            var keyword = ParseToken(SyntaxKind.WithKeyword);
            if (keyword != null)
            {
                var open = ParseRequiredToken(SyntaxKind.OpenParenToken);
                var parameters = ParseQueryOperatorParameterCommaList(s_externalDataWithClauseParameterMap, FnScanCommonListEnd);
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new ExternalDataWithClause(keyword, open, parameters, close);
            }

            return null;
        }

        private MaterializedViewCombineNameClause ParseRequiredMaterializedViewNameClause()
        {
            return new MaterializedViewCombineNameClause(
                ParseRequiredToken(SyntaxKind.OpenParenToken),
                ParseExpression() ?? CreateMissingExpression(),
                ParseRequiredToken(SyntaxKind.CloseParenToken));
        }

        private MaterializedViewCombineClause ParseRequiredMaterializedViewCombineClause(string keywordName)
        {
            var keyword = ParseToken(keywordName);
            if (keyword != null)
            {
                return new MaterializedViewCombineClause(
                    keyword,
                    ParseRequiredToken(SyntaxKind.OpenParenToken),
                    ParseExpression() ?? CreateMissingExpression(),
                    ParseRequiredToken(SyntaxKind.CloseParenToken));
            }
            else
            {
                return CreateMissingMaterializedViewCombineClause(keywordName);
            }
        }

        private MaterializedViewCombineExpression ParseMaterializedViewCombineExpression()
        {
            var keyword = ParseToken(SyntaxKind.MaterializedViewCombineKeyword);
            if (keyword != null)
            {
                var nameClause = ParseRequiredMaterializedViewNameClause();
                var baseClause = ParseRequiredMaterializedViewCombineClause("base");
                var deltaClause = ParseRequiredMaterializedViewCombineClause("delta");
                var aggregationsClause = ParseRequiredMaterializedViewCombineClause("aggregations");
                return new MaterializedViewCombineExpression(keyword, nameClause, baseClause, deltaClause, aggregationsClause);
            }

            return null;
        }

        private Expression ParsePrimaryPathSelector()
        {
            var selector = ParseRootPathElementSelector();
            if (selector != null)
            {
                var dataScope = ParseDataScopeClause();
                if (dataScope != null)
                {
                    return new DataScopeExpression(selector, dataScope);
                }
            }

            return selector;
        }

        private DataScopeClause ParseDataScopeClause()
        {
            if (PeekToken().Kind == SyntaxKind.DataScopeKeyword)
            {
                var keyword = ParseToken();
                var equal = ParseRequiredToken(SyntaxKind.EqualToken);
                var value = ParseRequiredToken(KustoFacts.DataScopeValues);
                return new DataScopeClause(keyword, equal, value);
            }

            return null;
        }

        private Expression ParsePathElementSelector()
        {
            if (PeekToken().Kind == SyntaxKind.OpenBracketToken)
            {
                return ParseBracketedPathElementSelector();
            }
            else
            {
                return ParseBarePathElementSelector();
            }
        }

        private Expression ParseRootPathElementSelector()
        {
            if (PeekToken().Kind == SyntaxKind.OpenBracketToken)
            {
                return ParseRootBracketedPathElementSelector();
            }
            else
            {
                return ParseBarePathElementSelector();
            }
        }

        private Expression ParseBarePathElementSelector()
        {
            if (PeekToken().Kind == SyntaxKind.AtToken)
            {
                return new AtExpression(ParseToken());
            }
            else
            {
                return ParseNameReference();
            }
        }

        private Expression ParseRootBracketedPathElementSelector()
        {
            if (PeekToken().Kind == SyntaxKind.OpenBracketToken)
            {
                return ParseBracketedWildcardedNameReference()
                    ?? ParseBracketedNameReference();
            }

            return null;
        }

        private Expression ParseBracketedPathElementSelector()
        {
            if (PeekToken().Kind == SyntaxKind.OpenBracketToken)
            {
                return ParseBracketedWildcardedNameReference()
                    ?? ParseBracketedNameReference()
                    ?? ParseBracketedExpression();
            }

            return null;
        }

        private Expression ParseBracketedExpression()
        {
            if (PeekToken().Kind == SyntaxKind.OpenBracketToken)
            {
                var open = ParseToken();
                var expr = ParseUnnamedExpression() ?? CreateMissingNameReference();
                var close = ParseRequiredToken(SyntaxKind.CloseBracketToken);
                return new BracketedExpression(open, expr, close);
            }

            return null;
        }

        private Expression ParseParenthesizedExpression()
        {
            if (PeekToken().Kind == SyntaxKind.OpenParenToken)
            {
                var open = ParseToken();
                var expr = ParseExpression() ?? CreateMissingExpression();
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new ParenthesizedExpression(open, expr, close);
            }

            return null;
        }

        private int ScanNameList(int offset = 0)
        {
            if (PeekToken(offset).Kind == SyntaxKind.OpenParenToken)
            {
                int start = offset;
                offset++;

                while (!ScanCommonListEnd(offset))
                {
                    var len = ScanName(offset);
                    if (len > 0)
                    {
                        offset += len;

                        if (PeekToken(offset).Kind == SyntaxKind.CommaToken)
                        {
                            offset += 1;
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        return -1;
                    }
                }

                if (PeekToken(offset).Kind == SyntaxKind.CloseParenToken)
                {
                    offset++;
                    return offset - start;
                }
            }

            return -1;
        }

        private static Func<QueryParser, NameDeclaration> FnParseNameDeclaration =
            qp => qp.ParseNameDeclaration();

        private Expression ParseNamedExpression()
        {
            if (ScanName() is int nameLen
                && nameLen > 0
                && PeekToken(nameLen).Kind == SyntaxKind.EqualToken)
            {
                var name = ParseNameDeclaration();
                var equal = ParseToken(SyntaxKind.EqualToken);
                var expr = ParseUnnamedExpression() ?? CreateMissingExpression();
                return new SimpleNamedExpression(name, equal, expr);
            }
            else if (ScanNameList() is int nameListLen
                && nameListLen > 0
                && PeekToken(nameListLen).Kind == SyntaxKind.EqualToken)
            {
                var open = ParseToken();
                var list = ParseCommaList(FnParseNameDeclaration, CreateMissingNameDeclaration, FnScanCommonListEnd, oneOrMore: true);
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                var equal = ParseRequiredToken(SyntaxKind.EqualToken);
                var expr = ParseUnnamedExpression() ?? CreateMissingExpression();
                return new CompoundNamedExpression(new RenameList(open, list, close), equal, expr);
            }
            else
            {
                return ParseUnnamedExpression();
            }
        }

        private static Func<QueryParser, Expression> FnParseNamedExpression =
            qp => qp.ParseNamedExpression();

        private Expression ParseArgument()
        {
            if (PeekToken().Kind == SyntaxKind.AsteriskToken
                && (PeekToken(1).Kind == SyntaxKind.CloseParenToken
                    || PeekToken(1).Kind == SyntaxKind.CommaToken))
            {
                return new StarExpression(ParseToken());
            }
            else
            {
                return ParseNamedExpression();
            }
        }

        private static Func<QueryParser, Expression> FnParseArgument =
            qp => qp.ParseArgument();

        private ExpressionList ParseArgumentList()
        {
            if (PeekToken().Kind == SyntaxKind.OpenParenToken)
            {
                var open = ParseToken();
                var args = ParseCommaList(FnParseArgument, CreateMissingExpression, FnScanCommonListEnd);
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new ExpressionList(open, args, close);
            }

            return null;
        }

        private static readonly IReadOnlyList<string> s_multiTokenFunctionNames =
            Functions.All.Select(f => f.Name).Concat(Aggregates.All.Select(f => f.Name)).Where(name => IsMultiTokenName(name)).ToArray();

        private bool ScanFunctionCallStart(int offset = 0)
        {
            var len = ScanName(offset);
            if (len > 0 && PeekToken(offset + len).Kind == SyntaxKind.OpenParenToken)
                return true;

            for (int i = 0; i < s_multiTokenFunctionNames.Count; i++)
            {
                len = ScanToken(s_multiTokenFunctionNames[i]);
                if (len > 0 && PeekToken(offset + len).Kind == SyntaxKind.OpenParenToken)
                    return true;
            }

            return false;
        }

        private NameReference ParseFunctionCallName()
        {
            var len = ScanName();
            if (len > 0 && PeekToken(len).Kind == SyntaxKind.OpenParenToken)
            {
                return ParseNameReference();
            }

            for (int i = 0; i < s_multiTokenFunctionNames.Count; i++)
            {
                var token = ParseToken(s_multiTokenFunctionNames[i]);
                if (token != null)
                {
                    return new NameReference(new TokenName(token));
                }
            }

            return null;
        }

        private FunctionCallExpression ParseFunctionCallExpression()
        {
            if (ScanFunctionCallStart())
            {
                var name = ParseFunctionCallName();
                var arguments = ParseArgumentList();
                return new FunctionCallExpression(name, arguments);
            }

            return null;
        }

        private Expression ParseDotCompositeFunctionCall()
        {
            Expression expr = ParseFunctionCallExpression();

            if (expr != null)
            {
                while (PeekToken().Kind == SyntaxKind.DotToken && ScanFunctionCallStart(1))
                {
                    var dot = ParseToken();
                    var call = ParseFunctionCallExpression();
                    expr = new PathExpression(expr, dot, call);
                }
            }

            return expr;
        }

        private Expression ParseToTableExpression()
        {
            var keyword = ParseToken(SyntaxKind.ToTableKeyword);
            if (keyword != null)
            {
                var kind = ParseQueryOperatorParameter(QueryOperatorParameters.ToTableKindParameter);
                var open = ParseRequiredToken(SyntaxKind.OpenParenToken);
                var expr = ParseExpression() ?? CreateMissingExpression();
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new ToTableExpression(keyword, kind, open, expr, close);
            }

            return null;
        }

        private Expression ParseToScalarExpression()
        {
            var keyword = ParseToken(SyntaxKind.ToScalarKeyword);
            if (keyword != null)
            {
                var kind = ParseQueryOperatorParameter(QueryOperatorParameters.ToScalarKindParameter);
                var open = ParseRequiredToken(SyntaxKind.OpenParenToken);
                var expr = ParseExpression() ?? CreateMissingExpression();
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new ToScalarExpression(keyword, kind, open, expr, close);
            }

            return null;
        }

        private Expression ParseFunctionCallOrPath()
        {
            if (PeekToken().Kind == SyntaxKind.ToTableKeyword)
            {
                return ParseToTableExpression();
            }

            var expr = ParsePrimaryExpression();

            while (expr != null)
            {
                var kind = PeekToken().Kind;
                if (kind == SyntaxKind.DotToken)
                {
                    expr = new PathExpression(expr, ParseToken(), ParsePathElementSelector() ?? CreateMissingNameReference());
                }
                else if (kind == SyntaxKind.OpenBracketToken)
                {
                    expr = new ElementExpression(expr, ParseBracketedExpression());
                }
                else
                {
                    break;
                }
            }

            return expr;
        }

        private Expression ParseUnaryPlusOrMinusExpression()
        {
            switch (PeekToken().Kind)
            {
                case SyntaxKind.MinusToken:
                    return new PrefixUnaryExpression(SyntaxKind.UnaryMinusExpression, ParseToken(), ParseFunctionCallOrPath() ?? CreateMissingExpression());
                case SyntaxKind.PlusToken:
                    return new PrefixUnaryExpression(SyntaxKind.UnaryPlusExpression, ParseToken(), ParseFunctionCallOrPath() ?? CreateMissingExpression());
                default:
                    return ParseFunctionCallOrPath();
            }
        }

        private Expression ParseInvocationExpression()
        {
            return ParseUnaryPlusOrMinusExpression();
        }

        private Expression ParseStringOperation()
        {
            if (PeekToken().Kind == SyntaxKind.AsteriskToken
                && GetStringOperationKind(PeekToken(1).Kind) is SyntaxKind starOpKind
                && starOpKind != SyntaxKind.None)
            {
                return new BinaryExpression(starOpKind, new StarExpression(ParseToken()), ParseToken(), ParseUnaryPlusOrMinusExpression() ?? CreateMissingExpression());
            }

            var expr = ParseUnaryPlusOrMinusExpression();
            if (expr != null)
            {
                while (GetStringOperationKind(PeekToken().Kind) is SyntaxKind opKind && opKind != SyntaxKind.None)
                {
                    expr = new BinaryExpression(opKind, expr, ParseToken(), ParseUnaryPlusOrMinusExpression() ?? CreateMissingExpression());
                }
            }

            return expr;
        }

        private static SyntaxKind GetStringOperationKind(SyntaxKind tokenKind)
        {
            switch (tokenKind)
            {
                case SyntaxKind.EqualTildeToken:
                    return SyntaxKind.EqualTildeExpression;
                case SyntaxKind.BangTildeToken:
                    return SyntaxKind.BangTildeExpression;
                case SyntaxKind.HasKeyword:
                    return SyntaxKind.HasExpression;
                case SyntaxKind.ColonToken:
                    return SyntaxKind.SearchExpression;
                case SyntaxKind.NotHasKeyword:
                    return SyntaxKind.NotHasExpression;
                case SyntaxKind.HasCsKeyword:
                    return SyntaxKind.HasCsExpression;
                case SyntaxKind.NotHasCsKeyword:
                    return SyntaxKind.NotHasCsExpression;
                case SyntaxKind.HasPrefixKeyword:
                    return SyntaxKind.HasPrefixExpression;
                case SyntaxKind.NotHasPrefixKeyword:
                    return SyntaxKind.NotHasPrefixExpression;
                case SyntaxKind.HasPrefixCsKeyword:
                    return SyntaxKind.HasPrefixCsExpression;
                case SyntaxKind.NotHasPrefixCsKeyword:
                    return SyntaxKind.NotHasPrefixCsExpression;
                case SyntaxKind.HasSuffixKeyword:
                    return SyntaxKind.HasSuffixExpression;
                case SyntaxKind.NotHasSuffixKeyword:
                    return SyntaxKind.NotHasSuffixExpression;
                case SyntaxKind.HasSuffixCsKeyword:
                    return SyntaxKind.HasSuffixCsExpression;
                case SyntaxKind.NotHasSuffixCsKeyword:
                    return SyntaxKind.NotHasSuffixCsExpression;
                case SyntaxKind.LikeKeyword:
                    return SyntaxKind.LikeExpression;
                case SyntaxKind.NotLikeKeyword:
                    return SyntaxKind.NotLikeExpression;
                case SyntaxKind.LikeCsKeyword:
                    return SyntaxKind.LikeCsExpression;
                case SyntaxKind.NotLikeCsKeyword:
                    return SyntaxKind.NotLikeCsExpression;
                case SyntaxKind.ContainsKeyword:
                    return SyntaxKind.ContainsExpression;
                case SyntaxKind.NotContainsKeyword:
                    return SyntaxKind.NotContainsExpression;
                case SyntaxKind.NotBangContainsKeyword:
                    return SyntaxKind.NotContainsExpression;
                case SyntaxKind.ContainsCsKeyword:
                    return SyntaxKind.ContainsCsExpression;
                case SyntaxKind.Contains_CsKeyword:
                    return SyntaxKind.ContainsCsExpression;
                case SyntaxKind.NotContainsCsKeyword:
                    return SyntaxKind.NotContainsCsExpression;
                case SyntaxKind.NotBangContainsCsKeyword:
                    return SyntaxKind.NotContainsCsExpression;
                case SyntaxKind.StartsWithKeyword:
                    return SyntaxKind.StartsWithExpression;
                case SyntaxKind.NotStartsWithKeyword:
                    return SyntaxKind.NotStartsWithExpression;
                case SyntaxKind.StartsWithCsKeyword:
                    return SyntaxKind.StartsWithCsExpression;
                case SyntaxKind.NotStartsWithCsKeyword:
                    return SyntaxKind.NotStartsWithCsExpression;
                case SyntaxKind.EndsWithKeyword:
                    return SyntaxKind.EndsWithExpression;
                case SyntaxKind.NotEndsWithKeyword:
                    return SyntaxKind.NotEndsWithExpression;
                case SyntaxKind.EndsWithCsKeyword:
                    return SyntaxKind.EndsWithCsExpression;
                case SyntaxKind.NotEndsWithCsKeyword:
                    return SyntaxKind.NotEndsWithCsExpression;
                case SyntaxKind.MatchesRegexKeyword:
                    return SyntaxKind.MatchesRegexExpression;
                default:
                    return SyntaxKind.None;
            }
        }

        private Expression ParseMultiplicativeExpression()
        {
            var expr = ParseStringOperation();

            if (expr != null)
            {
                while (GetMultiplicativeExpressionKind(PeekToken().Kind) is SyntaxKind opKind
                    && opKind != SyntaxKind.None)
                {
                    expr = new BinaryExpression(opKind, expr, ParseToken(), ParseStringOperation() ?? CreateMissingExpression());
                }
            }

            return expr;
        }

        private static SyntaxKind GetMultiplicativeExpressionKind(SyntaxKind tokenKind)
        {
            switch (tokenKind)
            {
                case SyntaxKind.AsteriskToken:
                    return SyntaxKind.MultiplyExpression;
                case SyntaxKind.SlashToken:
                    return SyntaxKind.DivideExpression;
                case SyntaxKind.PercentToken:
                    return SyntaxKind.ModuloExpression;
                default:
                    return SyntaxKind.None;
            }
        }

        private Expression ParseAdditiveExpression()
        {
            var expr = ParseMultiplicativeExpression();

            if (expr != null)
            {
                while (GetAdditiveExpressionKind(PeekToken().Kind) is SyntaxKind opKind
                    && opKind != SyntaxKind.None)
                {
                    expr = new BinaryExpression(opKind, expr, ParseToken(), ParseMultiplicativeExpression() ?? CreateMissingExpression());
                }
            }

            return expr;
        }

        private static SyntaxKind GetAdditiveExpressionKind(SyntaxKind tokenKind)
        {
            switch (tokenKind)
            {
                case SyntaxKind.PlusToken:
                    return SyntaxKind.AddExpression;
                case SyntaxKind.MinusToken:
                    return SyntaxKind.SubtractExpression;
                default:
                    return SyntaxKind.None;
            }
        }

        private Expression ParseRelationalExpresion()
        {
            var expr = ParseAdditiveExpression();

            if (expr != null)
            {
                while (GetRelationalExpressionKind(PeekToken().Kind) is SyntaxKind opKind
                    && opKind != SyntaxKind.None)
                {
                    expr = new BinaryExpression(opKind, expr, ParseToken(), ParseAdditiveExpression() ?? CreateMissingExpression());
                }
            }

            return expr;
        }

        private static SyntaxKind GetRelationalExpressionKind(SyntaxKind tokenKind)
        {
            switch (tokenKind)
            {
                case SyntaxKind.LessThanToken:
                    return SyntaxKind.LessThanExpression;
                case SyntaxKind.LessThanOrEqualToken:
                    return SyntaxKind.LessThanOrEqualExpression;
                case SyntaxKind.GreaterThanToken:
                    return SyntaxKind.GreaterThanExpression;
                case SyntaxKind.GreaterThanOrEqualToken:
                    return SyntaxKind.GreaterThanOrEqualExpression;
                default:
                    return SyntaxKind.None;
            }
        }

        private Expression ParseEqualityExpression()
        {
            if (PeekToken().Kind == SyntaxKind.AsteriskToken
                && PeekToken(1).Kind == SyntaxKind.EqualEqualToken)
            {
                return new BinaryExpression(SyntaxKind.EqualExpression, new StarExpression(ParseToken()), ParseToken(), ParseRelationalExpresion() ?? CreateMissingExpression());
            }

            var expr = ParseRelationalExpresion();

            if (expr != null)
            {
                while (true)
                {
                    switch (PeekToken().Kind)
                    {
                        case SyntaxKind.EqualEqualToken:
                            expr = new BinaryExpression(SyntaxKind.EqualExpression, expr, ParseToken(), ParseRelationalExpresion() ?? CreateMissingExpression());
                            continue;
                        case SyntaxKind.BangEqualToken:
                            expr = new BinaryExpression(SyntaxKind.NotEqualExpression, expr, ParseToken(), ParseRelationalExpresion() ?? CreateMissingTokenLiteral());
                            continue;
                        case SyntaxKind.LessThanGreaterThanToken:
                            expr = new BinaryExpression(SyntaxKind.NotEqualExpression, expr, ParseToken(), ParseRelationalExpresion() ?? CreateMissingTokenLiteral());
                            continue;
                        case SyntaxKind.InKeyword:
                            if (PeekToken(1).Kind == SyntaxKind.RangeKeyword)
                                return expr;
                            expr = new InExpression(SyntaxKind.InExpression, expr, ParseToken(), ParseRequiredInOperatorExpressionList());
                            continue;
                        case SyntaxKind.InCsKeyword:
                            expr = new InExpression(SyntaxKind.InCsExpression, expr, ParseToken(), ParseRequiredInOperatorExpressionList());
                            continue;
                        case SyntaxKind.NotInKeyword:
                            expr = new InExpression(SyntaxKind.NotInExpression, expr, ParseToken(), ParseRequiredInOperatorExpressionList());
                            continue;
                        case SyntaxKind.NotInCsKeyword:
                            expr = new InExpression(SyntaxKind.NotInCsExpression, expr, ParseToken(), ParseRequiredInOperatorExpressionList());
                            continue;
                        case SyntaxKind.HasAnyKeyword:
                            expr = new HasAnyExpression(SyntaxKind.HasAnyExpression, expr, ParseToken(), ParseRequiredInOperatorExpressionList());
                            continue;
                        case SyntaxKind.HasAllKeyword:
                            expr = new HasAllExpression(SyntaxKind.HasAllExpression, expr, ParseToken(), ParseRequiredInOperatorExpressionList());
                            continue;
                        case SyntaxKind.BetweenKeyword:
                            expr = new BetweenExpression(SyntaxKind.BetweenExpression, expr, ParseToken(), ParseRequiredExpressionCouple());
                            continue;
                        case SyntaxKind.NotBetweenKeyword:
                            expr = new BetweenExpression(SyntaxKind.NotBetweenExpression, expr, ParseToken(), ParseRequiredExpressionCouple());
                            continue;
                        default:
                            return expr;
                    }
                }
            }

            return expr;
        }

        private ExpressionList ParseRequiredInOperatorExpressionList()
        {
            return new ExpressionList(
                ParseRequiredToken(SyntaxKind.OpenParenToken),
                ParseCommaList(FnParseUnnamedExpression, CreateMissingExpression, FnScanCommonListEnd, oneOrMore: true),
                ParseRequiredToken(SyntaxKind.CloseParenToken));
        }

        private ExpressionCouple ParseRequiredExpressionCouple()
        {
            return new ExpressionCouple(
                ParseRequiredToken(SyntaxKind.OpenParenToken),
                ParseInvocationExpression() ?? CreateMissingExpression(),
                ParseRequiredToken(SyntaxKind.DotDotToken),
                ParseInvocationExpression() ?? CreateMissingExpression(),
                ParseRequiredToken(SyntaxKind.CloseParenToken));
        }

        private Expression ParseLogicalAndExpression()
        {
            var expr = ParseEqualityExpression();

            if (expr != null)
            {
                while (PeekToken().Kind == SyntaxKind.AndKeyword)
                {
                    expr = new BinaryExpression(SyntaxKind.AndExpression, expr, ParseToken(), ParseEqualityExpression() ?? CreateMissingExpression());
                }
            }

            return expr;
        }

        private Expression ParseLogicalOrExpression()
        {
            var expr = ParseLogicalAndExpression();

            if (expr != null)
            {
                while (PeekToken().Kind == SyntaxKind.OrKeyword)
                {
                    expr = new BinaryExpression(SyntaxKind.OrExpression, expr, ParseToken(), ParseLogicalAndExpression() ?? CreateMissingExpression());
                }
            }

            return expr;
        }

        private Expression ParseUnnamedExpression()
        {
            // shortcut for identifier/literal followed by punctuation that would end an expression
            switch (PeekToken(1).Kind)
            {
                case SyntaxKind.CommaToken:
                case SyntaxKind.CloseParenToken:
                case SyntaxKind.CloseBraceToken:
                case SyntaxKind.CloseBracketToken:
                case SyntaxKind.BarToken:
                    switch (PeekToken().Kind)
                    {
                        case SyntaxKind.IdentifierToken:
                        case SyntaxKind.BooleanLiteralToken:
                        case SyntaxKind.LongLiteralToken:
                        case SyntaxKind.RealLiteralToken:
                        case SyntaxKind.DecimalLiteralToken:
                        case SyntaxKind.DateTimeLiteralToken:
                        case SyntaxKind.TimespanLiteralToken:
                        case SyntaxKind.GuidLiteralToken:
                        case SyntaxKind.IntLiteralToken:
                            return ParsePrimaryExpression();
                    }
                    break;
            }

            if (_depth > MaxDepth)
            {
                return SafeParseUnnamedExpression();
            }
            else
            {
                _depth++;
                var result = ParseLogicalOrExpression();
                _depth--;
                return result;
            }
        }

        const int MaxDepth = 500;
        private int _depth;
        private StackSafeParser<LexicalToken> _safeParser;
        private List<object> _safeOutput;
        private Parser<LexicalToken> _safeGrammar;

        private Expression SafeParseUnnamedExpression()
        {
            if (_safeParser == null)
            {
                _safeOutput = new List<object>();
                _safeParser = new StackSafeParser<LexicalToken>(_source, _safeOutput);
                _safeGrammar = QueryGrammar.From(GlobalState.Default).Expression;
            }

            var len = _safeParser.Parse(_safeGrammar, _pos, 0);
            if (len >= 0)
            {
                _pos += len;
                var result = (Expression)_safeOutput[0];
                _safeOutput.Clear();
                return result;
            }
            else
            {
                return null;
            }
        }

        private static Func<QueryParser, Expression> FnParseUnnamedExpression =
            qp => qp.ParseUnnamedExpression();

#endregion

#region Query operator parameters

        private Expression ParseAnyQueryOperatorParameterValue()
        {
            var expr = ParseLiteral();

            if (expr == null)
                expr = ParseIdentifierOrKeywordTokenLiteral();

            if (expr == null)
                expr = ParseNameReference();

            return expr;
        }

        private Expression ParseTokenLiteral(IReadOnlyList<string> texts)
        {
            var token = ParseToken(texts);
            if (token != null)
                return new LiteralExpression(SyntaxKind.TokenLiteralExpression, token);
            return null;
        }

        private Expression ParseQueryOperatorParameterValue(QueryOperatorParameter queryParameter, Func<QueryParser, bool> fnEndNameList = null)
        {
            switch (queryParameter.ValueKind)
            {
                case QueryOperatorParameterValueKind.StringLiteral:
                    return ParseAnyQueryOperatorParameterValue() ?? CreateMissingStringLiteral();
                case QueryOperatorParameterValueKind.BoolLiteral:
                    return ParseAnyQueryOperatorParameterValue() ?? CreateMissingBoolLiteral();
                case QueryOperatorParameterValueKind.IntegerLiteral:
                case QueryOperatorParameterValueKind.NumericLiteral:
                case QueryOperatorParameterValueKind.SummableLiteral:
                    return ParseAnyQueryOperatorParameterValue() ?? CreateMissingLongLiteral();
                case QueryOperatorParameterValueKind.ScalarLiteral:
                    return ParseAnyQueryOperatorParameterValue() ?? CreateMissingValue();
                case QueryOperatorParameterValueKind.Word:
                case QueryOperatorParameterValueKind.WordOrNumber:
                    if (queryParameter.Values.Count > 0)
                        return ParseTokenLiteral(queryParameter.Values) ?? ParseAnyQueryOperatorParameterValue() ?? CreateMissingTokenLiteral(queryParameter.Values);
                    return ParseAnyQueryOperatorParameterValue() ?? CreateMissingValue();
                case QueryOperatorParameterValueKind.NameDeclaration:
                    return ParseNameDeclaration() ?? ParseAnyQueryOperatorParameterValue() ?? CreateMissingNameDeclaration();
                case QueryOperatorParameterValueKind.Column:
                    return ParseNameReference() ?? ParseAnyQueryOperatorParameterValue() ?? CreateMissingNameReference();
                case QueryOperatorParameterValueKind.ColumnList:
                    return ParseNameReferenceList(fnEndNameList ?? FnScanQueryOperatorParameterNameListEnd);
                default:
                    return ParseAnyQueryOperatorParameterValue() ?? CreateMissingValue();
            }
        }

        private bool ScanQueryOperatorParameterNameListEnd()
        {
            if (PeekToken().Kind == SyntaxKind.CommaToken)
            {
                return ScanKnownQueryOperatorParameterName(1) > 0
                    || ScanCommonListEnd(1);
            }
            else
            {
                return ScanKnownQueryOperatorParameterName() > 0
                    || ScanCommonListEnd();
            }
        }

        private static readonly Func<QueryParser, bool> FnScanQueryOperatorParameterNameListEnd =
            qp => qp.ScanQueryOperatorParameterNameListEnd();

        private NamedParameter ParseQueryOperatorParameter(QueryOperatorParameter parameter, Func<QueryParser, bool> fnEndNameList = null)
        {
            var len = ScanToken(parameter.Name);
            if (len > 0)
            {
                if (parameter.HasNoEquals)
                {
                    return new NamedParameter(
                        new NameDeclaration(new TokenName(ParseToken(parameter.Name))),
                        SyntaxToken.Missing(SyntaxKind.EqualToken),
                        ParseQueryOperatorParameterValue(parameter, fnEndNameList));
                }
                else if (PeekToken(len).Kind == SyntaxKind.EqualToken)
                {
                    return new NamedParameter(
                        new NameDeclaration(new TokenName(ParseToken(parameter.Name))),
                        ParseRequiredToken(SyntaxKind.EqualToken),
                        ParseQueryOperatorParameterValue(parameter, fnEndNameList));
                }
            }

            return null;
        }

        private NamedParameter ParseQueryOperatorParameter()
        {
            var nameToken = ParseQueryOperatorParameterName(_allowKnownQueryOperatorParametersOnly);
            if (nameToken != null)
            {
                var name = new NameDeclaration(new TokenName(nameToken));
                var equal = ParseRequiredToken(SyntaxKind.EqualToken);

                if ((_specificNameToQueryOperatorParameterMap != null
                    && _specificNameToQueryOperatorParameterMap.TryGetValue(nameToken.Text, out var queryParameter))
                    || s_nameToDefaultQueryOperatorParameterMap.TryGetValue(nameToken.Text, out queryParameter))
                {
                    var value = ParseQueryOperatorParameterValue(queryParameter);
                    return new NamedParameter(name, equal, value);
                }

                // now a known parameter, but parse it anyway
                return new NamedParameter(name, equal, ParseAnyQueryOperatorParameterValue() ?? CreateMissingValue());
            }

            return null;
        }

        private static Func<QueryParser, NamedParameter> FnParseQueryOperatorParameter =
            qp => qp.ParseQueryOperatorParameter();

        private static IReadOnlyDictionary<string, QueryOperatorParameter> s_nameToDefaultQueryOperatorParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.AllKnownParameters);

        private static IReadOnlyDictionary<string, QueryOperatorParameter> CreateQueryOperatorParameterMap(IReadOnlyList<QueryOperatorParameter> parameters)
        {
            var map = new Dictionary<string, QueryOperatorParameter>();

            for (int i = 0; i < parameters.Count; i++)
            {
                var p = parameters[i];
                map[p.Name] = p;

                if (p.Aliases != null && p.Aliases.Count > 0)
                {
                    foreach (var aliasName in p.Aliases)
                    {
                        map[aliasName] = p;
                    }
                }
            }

            return map;
        }

        private static bool IsMultiTokenName(string name)
        {
            return !KustoFacts.IsKeyword(name) // known keywords will still be one token
                && name.Any(c => !(char.IsLetterOrDigit(c) || c == '_'));
        }

        /// <summary>
        /// The set of known query operator parameter names
        /// </summary>
        private static HashSet<string> s_knownQueryOperaterParameterNames =
            new HashSet<string>(
                s_nameToDefaultQueryOperatorParameterMap.Keys
                .Concat(KustoFacts.KnownQueryOperatorParameterNames));

        /// <summary>
        /// The list of known query parameters names that are likely to be parsed as multiple lexical tokens
        /// </summary>
        private static IReadOnlyList<string> s_multiTokenQueryOperatorParameterNames =
            s_knownQueryOperaterParameterNames.Where(IsMultiTokenName).ToList();

        private bool IsKnownQueyrOperatorParameterName(string name)
        {
            return s_knownQueryOperaterParameterNames.Contains(name)
                || (_specificNameToQueryOperatorParameterMap != null
                    && _specificNameToQueryOperatorParameterMap.ContainsKey(name));
        }

        private int ScanKnownQueryOperatorParameterName(int offset = 0)
        {
            var token = PeekToken(offset);
            if ((token.Kind == SyntaxKind.IdentifierToken || token.Kind.IsKeyword())
                && IsKnownQueyrOperatorParameterName(token.Text))
            {
                return 1;
            }
            else
            {
                // check for any special names that don't conform to identifiers
                for (int i = 0; i < s_multiTokenQueryOperatorParameterNames.Count; i++)
                {
                    var len = ScanToken(s_multiTokenQueryOperatorParameterNames[i]);
                    if (len > 0)
                        return len;
                }
            }

            return -1;
        }

        private SyntaxToken ParseKnownQueryOperatorParaterName()
        {
            var lt = PeekToken();
            if ((lt.Kind == SyntaxKind.IdentifierToken || lt.Kind.IsKeyword())
                && IsKnownQueyrOperatorParameterName(lt.Text))
            {
                return ParseToken();
            }
            else
            {
                // check for any special names that don't conform to identifiers
                for (int i = 0; i < s_multiTokenQueryOperatorParameterNames.Count; i++)
                {
                    var tok = ParseToken(s_multiTokenQueryOperatorParameterNames[i]);
                    if (tok != null)
                        return tok;
                }
            }

            return null;
        }

        private int ScanSecondaryQueryOperatorParameterNamePart(int offset = 0)
        {
            var t = PeekToken(offset);
            return (t.Kind == SyntaxKind.IdentifierToken
                    || t.Kind.IsKeyword()
                    || t.Kind == SyntaxKind.LongLiteralToken
                    || t.Kind == SyntaxKind.DotToken
                    || t.Kind == SyntaxKind.MinusToken)
                    && t.Trivia.Length == 0
                ? 1 : -1;
        }

        private int ScanAnyQueryOperatorParameterName(int offset = 0)
        {
            int start = offset;
            int len;

            // check for allow name pattern
            var t = PeekToken(offset);
            if (t.Kind == SyntaxKind.IdentifierToken || (t.Kind.IsKeyword() && t.Kind.CanBeIdentifier()))
            {
                offset++;

                while ((len = ScanSecondaryQueryOperatorParameterNamePart(offset)) > 0)
                {
                    offset += len;
                }

                if (offset > start)
                    return offset - start;
            }
            else if (t.Kind.IsKeyword())
            {
                offset++;

                while ((len = ScanSecondaryQueryOperatorParameterNamePart(offset)) > 0)
                {
                    offset += len;
                }

                if (offset - start > 1)
                    return offset - start;
            }

            return ScanKnownQueryOperatorParameterName(offset);
        }

        private SyntaxToken ParseQueryOperatorParameterName(bool knownNamesOnly = false)
        {
            var name = ParseKnownQueryOperatorParaterName();
            if (name != null)
                return name;

            if (!knownNamesOnly)
            {
                var len = ScanAnyQueryOperatorParameterName();
                if (len > 0 && PeekToken(len).Kind == SyntaxKind.EqualToken)
                {
                    return ParseCombinedIdentifier(len);
                }
            }

            return null;
        }

        private NameReference ParseNameReferenceListName()
        {
            return ScanKnownQueryOperatorParameterName() > 0
                ? null // don't consume other known query operator parameter names as names in the name list
                : ParseNameReference();
        }

        private static Func<QueryParser, NameReference> FnParseNameReferenceListName =
            qp => qp.ParseNameReferenceListName();

        private NameReferenceList ParseNameReferenceList(Func<QueryParser, bool> fnEndList)
        {
            var names = ParseCommaList(FnParseNameReferenceListName, CreateMissingNameReference, fnEndList, oneOrMore: true);
            return new NameReferenceList(names);
        }

        private IReadOnlyDictionary<string, QueryOperatorParameter> _specificNameToQueryOperatorParameterMap;
        private bool _allowKnownQueryOperatorParametersOnly;

        private SyntaxList<NamedParameter> ParseQueryOperatorParameterList(
            IReadOnlyDictionary<string, QueryOperatorParameter> nameToParameterMap = null,
            bool knownParametersOnly = false)
        {
            var oldParameters = _specificNameToQueryOperatorParameterMap;
            var oldKnownParametersOnly = _allowKnownQueryOperatorParametersOnly;

            _specificNameToQueryOperatorParameterMap = nameToParameterMap;
            _allowKnownQueryOperatorParametersOnly = knownParametersOnly;

            var list = ParseList(FnParseQueryOperatorParameter);

            _specificNameToQueryOperatorParameterMap = oldParameters;
            _allowKnownQueryOperatorParametersOnly = oldKnownParametersOnly;

            return list;
        }

        private SyntaxList<SeparatedElement<NamedParameter>> ParseQueryOperatorParameterCommaList(
            IReadOnlyDictionary<string, QueryOperatorParameter> nameToParameterMap = null,
            Func<QueryParser, bool> fnScanEnd = null,
            bool knownParametersOnly = false)
        {
            var oldParameters = _specificNameToQueryOperatorParameterMap;
            var oldKnownParametersOnly = _allowKnownQueryOperatorParametersOnly;

            _specificNameToQueryOperatorParameterMap = nameToParameterMap;
            _allowKnownQueryOperatorParametersOnly = knownParametersOnly;

            var list = ParseCommaList(FnParseQueryOperatorParameter, CreateMissingNamedParameter, FnScanCommonListEnd);

            _specificNameToQueryOperatorParameterMap = oldParameters;
            _allowKnownQueryOperatorParametersOnly = oldKnownParametersOnly;

            return list;
        }

#endregion

#region Entity Names

        private Expression ParseBracketedEntityNamePathElementSelector() =>
            ParseBracketedWildcardedNameReference()
            ?? ParseBracketedNameReference();

        private Expression ParseEntityPathExpression()
        {
            var expr = ParseRootPathElementSelector();

            if (expr != null)
            {
                while (true)
                {
                    var kind = PeekToken().Kind;
                    if (kind == SyntaxKind.DotToken)
                    {
                        expr = new PathExpression(expr, ParseToken(), ParsePathElementSelector() ?? CreateMissingNameReference());
                    }
                    else if (kind == SyntaxKind.OpenBracketToken)
                    {
                        expr = new ElementExpression(expr, ParseBracketedPathElementSelector());
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return expr;
        }

        private Expression ParseEntityReferenceExpression() =>
            ParseEntityPathExpression();

        private static readonly Func<QueryParser, Expression> FnParseEntityReferenceExpression =
            qp => qp.ParseEntityReferenceExpression();

        private Expression ParseWildcardedEntityPathSelector() =>
            ParseWildcardedNameReference()
            ?? ParseBracketedEntityNamePathElementSelector()
            ?? ParseBarePathElementSelector();

        private Expression ParseWildcardedEntityExpression()
        {
            if (ScanWildcardedName() > 0)
            {
                return ParseWildcardedNameReference();
            }
            else if (ScanFunctionCallStart())
            {
                var expr = ParseDotCompositeFunctionCall();

                if (expr != null && PeekToken().Kind == SyntaxKind.DotToken)
                {
                    expr = new PathExpression(
                        expr,
                        ParseToken(),
                        ParseWildcardedEntityPathSelector() ?? CreateMissingNameReference());
                }

                return expr;
            }

            return null;
        }

#endregion

#region Query Operators

#region consume 

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_consumeOperatorParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.ConsumeParameters);

        private ConsumeOperator ParseConsumeOperator()
        {
            var keyword = ParseToken(SyntaxKind.ConsumeKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_consumeOperatorParameterMap);
                return new ConsumeOperator(keyword, parameters);
            }

            return null;
        }

#endregion

#region count

        private CountAsIdentifierClause ParseCountAsIdentifierClause()
        {
            var keyword = ParseToken(SyntaxKind.AsKeyword);
            if (keyword != null)
            {
                var id = ParseRequiredToken(SyntaxKind.IdentifierToken);
                return new CountAsIdentifierClause(keyword, id);
            }

            return null;
        }

        private CountOperator ParseCountOperator()
        {
            // don't collide with count() function
            if (PeekToken().Kind == SyntaxKind.CountKeyword
                && PeekToken(1).Kind != SyntaxKind.OpenParenToken)
            {
                var keyword = ParseToken();
                var asClause = ParseCountAsIdentifierClause();
                return new CountOperator(keyword, asClause);

            }

            return null;
        }

#endregion

#region executeAndCache

        private ExecuteAndCacheOperator ParseExecuteAndCacheOperator()
        {
            var keyword = ParseToken(SyntaxKind.ExecuteAndCacheKeyword);
            if (keyword != null)
            {
                return new ExecuteAndCacheOperator(keyword);
            }

            return null;
        }

#endregion

#region extend

        private ExtendOperator ParseExtendOperator()
        {
            var keyword = ParseToken(SyntaxKind.ExtendKeyword);
            if (keyword != null)
            {
                var expressions = ParseCommaList(FnParseNamedExpression, CreateMissingExpression, FnScanCommonListEnd);
                return new ExtendOperator(keyword, expressions);
            }

            return null;
        }

#endregion

#region facet

        private FacetWithClause ParseFacetWithClause()
        {
            var keyword = ParseToken(SyntaxKind.WithKeyword);
            if (keyword != null)
            {
                var open = ParseToken(SyntaxKind.OpenParenToken);
                if (open != null)
                {
                    var expr = ParseForkPipeExpression() ?? CreateMissingQueryOperatorExpression();
                    var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                    return new FacetWithExpressionClause(keyword, open, expr, close);
                }
                else
                {
                    var op = ParseForkPipeQueryOperator() ?? CreateMissingQueryOperator();
                    return new FacetWithOperatorClause(keyword, op);
                }
            }

            return null;
        }

        private static readonly Func<QueryParser, bool> FnScanFacetExpressionListEnd =
            qp => qp.ScanCustomListEnd(SyntaxKind.WithKeyword);

        private FacetOperator ParseFacetOperator()
        {
            var keyword = ParseToken(SyntaxKind.FacetKeyword);
            if (keyword != null)
            {
                var byKeyword = ParseRequiredToken(SyntaxKind.ByKeyword);
                var expressions = ParseCommaList(FnParseEntityReferenceExpression, CreateMissingNameReferenceExpression, FnScanFacetExpressionListEnd, oneOrMore: true);
                var withClause = ParseFacetWithClause();
                return new FacetOperator(keyword, byKeyword, expressions, withClause);
            }

            return null;
        }

#endregion

#region filter / where

        private static readonly IReadOnlyList<SyntaxKind> s_filterOperatorKeywords =
            new[] { SyntaxKind.WhereKeyword, SyntaxKind.FilterKeyword };

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_filterOperatorParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.FilterParameters);

        private FilterOperator ParseFilterOperator()
        {
            var keyword = ParseToken(SyntaxKind.WhereKeyword) ?? ParseToken(SyntaxKind.FilterKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_filterOperatorParameterMap, knownParametersOnly: true);
                var expr = ParseNamedExpression() ?? CreateMissingExpression();
                return new FilterOperator(keyword, parameters, expr);
            }

            return null;
        }

#endregion

#region getschema

        private GetSchemaOperator ParseGetSchemaOperator()
        {
            var keyword = ParseToken(SyntaxKind.GetSchemaKeyword);
            if (keyword != null)
            {
                return new GetSchemaOperator(keyword);
            }

            return null;
        }

#endregion

#region find

        private Expression ParseFindOperand()
        {
            return ParseWildcardedEntityExpression()
                ?? ParseBracketedEntityNamePathElementSelector()
                ?? ParseBarePathElementSelector();
        }

        private static readonly Func<QueryParser, Expression> FnParseFindOperand =
            qp => qp.ParseFindOperand();

        private FindInClause ParseFindInClause()
        {
            var keyword = ParseToken(SyntaxKind.InKeyword);
            if (keyword != null)
            {
                var open = ParseRequiredToken(SyntaxKind.OpenParenToken);
                var expressions = ParseCommaList(FnParseFindOperand, CreateMissingExpression, FnScanCommonListEnd, oneOrMore: true);
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new FindInClause(keyword, open, expressions, close);
            }

            return null;
        }

        private Expression ParseTypedColumnNameReference()
        {
            var name = ParseNameReference();

            if (name != null && PeekToken().Kind == SyntaxKind.ColonToken)
            {
                var colon = ParseToken();
                var type = ParseParamTypeExtended() ?? ParseIdentifierTypeExpression() ?? CreateMissingType();
                return new TypedColumnReference(name, colon, type);
            }

            return name;
        }

        private static readonly Func<QueryParser, Expression> FnParseTypeColumnNameReference =
            qp => qp.ParseTypedColumnNameReference();

        private PackExpression ParsePackExpression()
        {
            var keyword = ParseToken(SyntaxKind.PackKeyword);
            if (keyword != null)
            {
                return new PackExpression(
                    keyword,
                    ParseRequiredToken(SyntaxKind.OpenParenToken),
                    ParseRequiredToken(SyntaxKind.AsteriskToken),
                    ParseRequiredToken(SyntaxKind.CloseParenToken));
            }

            return null;
        }

        private Expression ParseFindProjectColumn()
        {
            return ParsePackExpression()
                ?? ParseStarExpression()
                ?? ParseTypedColumnNameReference();
        }

        private static readonly Func<QueryParser, Expression> FnParseFindProjectColumn =
            qp => qp.ParseFindProjectColumn();

        private FindProjectClause ParseFindProjectClause()
        {
            switch (PeekToken().Kind)
            {
                case SyntaxKind.ProjectKeyword:
                    return new FindProjectClause(ParseToken(), ParseCommaList(FnParseFindProjectColumn, CreateMissingExpression, oneOrMore: true));
                case SyntaxKind.ProjectSmartKeyword:
                    return new FindProjectClause(ParseToken(), SyntaxList<SeparatedElement<Expression>>.Empty());
                default:
                    return null;
            }
        }

        private FindProjectClause ParseFindProjectAwayClause()
        {
            var keyword = ParseToken(SyntaxKind._ProjectAwayKeyword);
            if (keyword != null)
            {
                var columns = ParseCommaList(FnParseFindProjectColumn, CreateMissingExpression, oneOrMore: true);
                return new FindProjectClause(keyword, columns);
            }

            return null;
        }

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_findOperatorParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.FindParameters);

        private FindOperator ParseFindOperator()
        {
            var keyword = ParseToken(SyntaxKind.FindKeyword);
            if (keyword != null)
            {
                var dataScope = ParseDataScopeClause();
                var parameters = ParseQueryOperatorParameterList(s_findOperatorParameterMap);
                var inClause = ParseFindInClause();
                var where = (parameters.Count > 0 || inClause != null)
                    ? ParseRequiredToken(SyntaxKind.WhereKeyword)
                    : ParseToken(SyntaxKind.WhereKeyword);
                var expr = ParseUnnamedExpression() ?? CreateMissingExpression();
                var project = ParseFindProjectClause();
                var projectAway = ParseFindProjectAwayClause();
                return new FindOperator(keyword, dataScope, parameters, inClause, where, expr, project, projectAway);
            }

            return null;
        }

#endregion

#region search

        private Expression ParseSearchPredicate()
        {
            var expr = ParseUnnamedExpression();
            if (expr == null)
            {
                expr = ParseStarExpression();
                if (expr != null && ParseToken(SyntaxKind.AndKeyword) is SyntaxToken andKeyword)
                {
                    return new BinaryExpression(SyntaxKind.AndExpression, expr, andKeyword, ParseUnnamedExpression() ?? CreateMissingExpression());
                }
            }

            return expr;
        }

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_searchOperatorParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.SearchParameters);

        private SearchOperator ParseSearchOperator()
        {
            var keyword = ParseToken(SyntaxKind.SearchKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_searchOperatorParameterMap);
                var dataScope = ParseDataScopeClause();
                var inClause = ParseFindInClause();
                var condition = ParseSearchPredicate() ?? CreateMissingExpression();
                return new SearchOperator(keyword, parameters, dataScope, inClause, condition);
            }

            return null;
        }

#endregion

#region fork

        private int ScanNameEqualsClause(int offset = 0)
        {
            var len = ScanName(offset);
            return len > 0 && PeekToken(len).Kind == SyntaxKind.EqualToken
                ? len + 1
                : -1;
        }

        private NameEqualsClause ParseNameEqualsClause()
        {
            if (ScanNameEqualsClause() > 0)
            {
                var name = ParseNameDeclaration();
                var equal = ParseRequiredToken(SyntaxKind.EqualToken);
                return new NameEqualsClause(name, equal);
            }

            return null;
        }

        private ForkExpression ParseForkExpression()
        {
            if (ScanNameEqualsClause() > 0)
            {
                var nameEqual = ParseNameEqualsClause();
                var open = ParseRequiredToken(SyntaxKind.OpenParenToken);
                var expr = ParseForkPipeExpression() ?? ParseExpression() ?? CreateMissingExpression();
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new ForkExpression(nameEqual, open, expr, close);
            }
            else if (PeekToken().Kind == SyntaxKind.OpenParenToken)
            {
                var open = ParseRequiredToken(SyntaxKind.OpenParenToken);
                var expr = ParseForkPipeExpression() ?? ParseExpression() ?? CreateMissingExpression();
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new ForkExpression(null, open, expr, close);
            }

            return null;
        }

        private static readonly Func<QueryParser, ForkExpression> FnParseForkExpression =
            qp => qp.ParseForkExpression();

        private static readonly Func<ForkExpression> CreateMissingForkExpression = () =>
            new ForkExpression(
                null,
                CreateMissingToken(SyntaxKind.OpenParenToken),
                CreateMissingExpression(),
                CreateMissingToken(SyntaxKind.CloseParenToken));

        private ForkOperator ParseForkOperator()
        {
            var keyword = ParseToken(SyntaxKind.ForkKeyword);
            if (keyword != null)
            {
                var expressions = ParseList(FnParseForkExpression, CreateMissingForkExpression, oneOrMore: true);
                return new ForkOperator(keyword, expressions);
            }

            return null;
        }

        private Expression ParseForkPipeExpression() =>
            ParsePipeExpression();

        private QueryOperator ParseForkPipeQueryOperator() =>
            ParseQueryOperator();

#endregion

#region partition

        private PartitionQuery ParsePartitionQuery()
        {
            var open = ParseToken(SyntaxKind.OpenBraceToken);
            if (open != null)
            {
                var expr = ParseExpression() ?? CreateMissingExpression();
                var close = ParseRequiredToken(SyntaxKind.CloseBraceToken);
                return new PartitionQuery(open, expr, close);
            }

            return null;
        }

        private PartitionSubquery ParsePartitionUnscopedSubquery()
        {
            var open = ParseToken(SyntaxKind.OpenParenToken);
            if (open != null)
            {
                var expr = ParsePipeSubExpression() ?? ParseExpression() ?? CreateMissingExpression();
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new PartitionSubquery(null, open, expr, close);
            }

            return null;
        }

        private PartitionSubquery ParsePartitionScopedSubquery()
        {
            var inKeyword = ParseToken(SyntaxKind.InKeyword);
            if (inKeyword != null)
            {
                var scope = ParseFunctionCallExpression() ?? ParseDynamicLiteral() ?? CreateMissingExpression();
                var open = ParseRequiredToken(SyntaxKind.OpenParenToken);
                var expr = ParsePipeSubExpression() ?? ParseExpression() ?? CreateMissingExpression();
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new PartitionSubquery(new PartitionScope(inKeyword, scope), open, expr, close);
            }

            return null;
        }

        private PartitionOperand ParsePartitionOperand()
        {
            switch (PeekToken().Kind)
            {
                case SyntaxKind.OpenBraceToken:
                    return ParsePartitionQuery();
                case SyntaxKind.OpenParenToken:
                    return ParsePartitionUnscopedSubquery();
                case SyntaxKind.InKeyword:
                    return ParsePartitionScopedSubquery();
                default:
                    return null;
            }
        }

        private static PartitionOperand CreateMissingPartitionOperand() =>
            new PartitionSubquery(
                null,
                CreateMissingToken(SyntaxKind.OpenParenToken),
                CreateMissingExpression(),
                CreateMissingToken(SyntaxKind.CloseParenToken));

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_partitionOperatorParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.PartitionParameters);

        private PartitionOperator ParsePartitionOperator()
        {
            var keyword = ParseToken(SyntaxKind.PartitionKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_partitionOperatorParameterMap);
                var byKeyword = ParseRequiredToken(SyntaxKind.ByKeyword);
                var byExpr = ParseEntityReferenceExpression() ?? CreateMissingNameReference();
                var operand = ParsePartitionOperand() ?? CreateMissingPartitionOperand();
                return new PartitionOperator(keyword, parameters, byKeyword, byExpr, operand);
            }

            return null;
        }

#endregion

#region join

        private JoinConditionClause ParseJoinOnClause()
        {
            var keyword = ParseToken(SyntaxKind.OnKeyword);
            if (keyword != null)
            {
                var expressions = ParseCommaList(FnParseUnnamedExpression, CreateMissingExpression, oneOrMore: true);
                return new JoinOnClause(keyword, expressions);
            }

            return null;
        }

        private JoinConditionClause ParseJoinWhereClause()
        {
            var keyword = ParseToken(SyntaxKind.WhereKeyword);
            if (keyword != null)
            {
                var expr = ParseUnnamedExpression() ?? CreateMissingExpression();
                return new JoinWhereClause(keyword, expr);
            }

            return null;
        }

        private static JoinOnClause CreateMissingJoinOnClause() =>
            new JoinOnClause(
                SyntaxToken.Missing(SyntaxKind.JoinOnClause),
                SyntaxList<SeparatedElement<Expression>>.Empty(),
                new[] { DiagnosticFacts.GetMissingJoinOnClause() });


        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_joinOperatorParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.JoinParameters);

        private JoinOperator ParseJoinOperator()
        {
            var keyword = ParseToken(SyntaxKind.JoinKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_joinOperatorParameterMap);
                var expr = ParseUnnamedExpression() ?? CreateMissingExpression();
                var condition = ParseJoinOnClause() ?? ParseJoinWhereClause();
                return new JoinOperator(keyword, parameters, expr, condition);
            }

            return null;
        }

#endregion

#region lookup

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_lookupOperatorParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.LookupParameters);

        private LookupOperator ParseLookupOperator()
        {
            var keyword = ParseToken(SyntaxKind.LookupKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_lookupOperatorParameterMap);
                var expression = ParseUnnamedExpression() ?? CreateMissingExpression();
                var condition = ParseJoinOnClause() ?? CreateMissingJoinOnClause();
                return new LookupOperator(keyword, parameters, expression, condition);
            }

            return null;
        }

#endregion

#region make-series

        private DefaultExpressionClause ParseDefaultExpressionClause()
        {
            var keyword = ParseToken(SyntaxKind.DefaultKeyword);
            if (keyword != null)
            {
                var equal = ParseRequiredToken(SyntaxKind.EqualToken);
                var expr = ParseNamedExpression() ?? CreateMissingExpression();
                return new DefaultExpressionClause(keyword, equal, expr);
            }

            return null;
        }

        private MakeSeriesByClause ParseMakeSeriesByClause()
        {
            var keyword = ParseToken(SyntaxKind.ByKeyword);
            if (keyword != null)
            {
                var expressions = ParseCommaList(FnParseNamedExpression, CreateMissingExpression, oneOrMore: true);
                return new MakeSeriesByClause(keyword, expressions);
            }

            return null;
        }

        private MakeSeriesExpression ParseMakeSeriesExpression()
        {
            var expr = ParseNamedExpression();
            if (expr != null)
            {
                var defClause = ParseDefaultExpressionClause();
                return new MakeSeriesExpression(expr, defClause);
            }

            return null;
        }

        private static readonly Func<QueryParser, MakeSeriesExpression> FnParseMakeSeriesExpression =
            qp => qp.ParseMakeSeriesExpression();

        private static Func<MakeSeriesExpression> CreateMissingMakeSeriesExpression = () =>
            new MakeSeriesExpression(CreateMissingExpression(), null);

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_makeSeriesOperatorParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.MakeSeriesParameters);

        private static readonly IReadOnlyList<SyntaxKind> s_makeSeriesExpressionListEnds =
            new[] { SyntaxKind.OnKeyword, SyntaxKind.FromKeyword, SyntaxKind.InKeyword };

        private static readonly Func<QueryParser, bool> FnScanMakeSeriesExpressionListEnd =
            qp => qp.ScanCustomListEnd(s_makeSeriesExpressionListEnds);

        private MakeSeriesRangeClause ParseRequiredMakeSeriesInRangeClause()
        {
            var inKeyword = ParseRequiredToken(SyntaxKind.InKeyword);
            var rangeKeyword = ParseRequiredToken(SyntaxKind.RangeKeyword);
            var open = ParseRequiredToken(SyntaxKind.OpenParenToken);
            var expressions = ParseCommaList(FnParseNamedExpression, CreateMissingExpression, FnScanCommonListEnd);
            var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
            return new MakeSeriesInRangeClause(inKeyword, rangeKeyword, new ExpressionList(open, expressions, close));
        }

        private MakeSeriesFromClause ParseMakeSeriesFromClause()
        {
            var keyword = ParseToken(SyntaxKind.FromKeyword);
            if (keyword != null)
            {
                var expr = ParseUnnamedExpression() ?? CreateMissingExpression();
                return new MakeSeriesFromClause(keyword, expr);
            }

            return null;
        }

        private MakeSeriesToClause ParseMakeSeriesToClause()
        {
            var keyword = ParseToken(SyntaxKind.ToKeyword);
            if (keyword != null)
            {
                var expr = ParseUnnamedExpression() ?? CreateMissingExpression();
                return new MakeSeriesToClause(keyword, expr);
            }

            return null;
        }

        private MakeSeriesStepClause ParseRequiredMakeSeriesStepClause()
        {
            var keyword = ParseRequiredToken(SyntaxKind.StepKeyword);
            var expr = ParseUnnamedExpression() ?? CreateMissingExpression();
            return new MakeSeriesStepClause(keyword, expr);
        }

        private MakeSeriesRangeClause ParseMakeSeriesFromToStepClause()
        {
            var kind = PeekToken().Kind;
            if (kind == SyntaxKind.FromKeyword || kind == SyntaxKind.ToKeyword || kind == SyntaxKind.StepKeyword)
            {
                var fromClause = ParseMakeSeriesFromClause();
                var toClause = ParseMakeSeriesToClause();
                var stepClause = ParseRequiredMakeSeriesStepClause();
                return new MakeSeriesFromToStepClause(fromClause, toClause, stepClause);
            }

            return null;
        }

        private MakeSeriesOperator ParseMakeSeriesOperator()
        {
            var keyword = ParseToken(SyntaxKind.MakeSeriesKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_makeSeriesOperatorParameterMap, knownParametersOnly: true);
                var expressions = ParseCommaList(FnParseMakeSeriesExpression, CreateMissingMakeSeriesExpression, FnScanMakeSeriesExpressionListEnd, oneOrMore: true);
                var onClause = new MakeSeriesOnClause(ParseRequiredToken(SyntaxKind.OnKeyword), ParseNamedExpression() ?? CreateMissingExpression());
                var rangeClause = ParseMakeSeriesFromToStepClause() ?? ParseRequiredMakeSeriesInRangeClause();
                var byClause = ParseMakeSeriesByClause();
                return new MakeSeriesOperator(keyword, parameters, expressions, onClause, rangeClause, byClause);
            }

            return null;
        }

#endregion

#region mv-expand

        private ToTypeOfClause ParseToTypeOfClause()
        {
            var keyword = ParseToken(SyntaxKind.ToKeyword);
            if (keyword != null)
            {
                var typeOf = (TypeOfLiteralExpression)(ParseTypeOfLiteral() ?? CreateMissingTypeOfLiteral());
                return new ToTypeOfClause(keyword, typeOf);
            }

            return null;
        }

        private MvExpandExpression ParseMvExpandExpression()
        {
            if (PeekToken().Kind == SyntaxKind.ToKeyword)
            {
                return new MvExpandExpression(CreateMissingExpression(), ParseToTypeOfClause());
            }

            var expr = ParseNamedExpression();
            if (expr != null)
            {
                var typeOf = ParseToTypeOfClause();
                return new MvExpandExpression(expr, typeOf);
            }

            return null;
        }

        private static readonly Func<QueryParser, MvExpandExpression> FnParseMvExpandExpression =
            qp => qp.ParseMvExpandExpression();

        private static readonly Func<MvExpandExpression> CreateMissingMvExpandExpression = () =>
            new MvExpandExpression(CreateMissingExpression(), null);

        private static readonly IReadOnlyList<SyntaxKind> s_mvExpandExpressionListEnd =
            new[] { SyntaxKind.LimitKeyword };

        private static readonly Func<QueryParser, bool> FnScanMvExpandExpressionListEnd =
            qp => qp.ScanCustomListEnd(s_mvExpandExpressionListEnd);

        private SyntaxList<SeparatedElement<MvExpandExpression>> ParseMvExpandExpressionList()
        {
            if (PeekToken().Kind == SyntaxKind.ToKeyword)
            {
                var position = GetResetPoint();

                // if only one item that is just "to typeof(xxx)" then allow expression to be null w/o error
                var clause = ParseToTypeOfClause();
                if (PeekToken().Kind != SyntaxKind.CommaToken)
                {
                    return new SyntaxList<SeparatedElement<MvExpandExpression>>(new[] {
                    new SeparatedElement<MvExpandExpression>(new MvExpandExpression(null, clause))});
                }
                else
                {
                    Reset(position);
                }
            }

            return ParseCommaList(FnParseMvExpandExpression, CreateMissingMvExpandExpression, FnScanMvExpandExpressionListEnd, oneOrMore: true);
        }

        private MvExpandRowLimitClause ParseMvExpandRowLimitClause()
        {
            var keyword = ParseToken(SyntaxKind.LimitKeyword);
            if (keyword != null)
            {
                var expr = ParseUnnamedExpression() ?? CreateMissingExpression();
                return new MvExpandRowLimitClause(keyword, expr);
            }

            return null;
        }

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_mvExpandOperatorParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.MvExpandParameters);

        private MvExpandOperator ParseMvExpandOperator()
        {
            var keyword = ParseToken(SyntaxKind.MvExpandKeyword) ?? ParseToken(SyntaxKind.MvDashExpandKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_mvExpandOperatorParameterMap, knownParametersOnly: true);
                var expressions = ParseMvExpandExpressionList();
                var rowLimit = ParseMvExpandRowLimitClause();
                return new MvExpandOperator(keyword, parameters, expressions, rowLimit);
            }

            return null;
        }

#endregion

#region mv-apply

        private MvApplyExpression ParseMvApplyExpression()
        {
            if (PeekToken().Kind == SyntaxKind.ToKeyword)
            {
                var toTypeOf = ParseToTypeOfClause();
                return new MvApplyExpression(CreateMissingExpression(), toTypeOf);
            }

            var expr = ParseNamedExpression();
            if (expr != null)
            {
                var toTypeOf = ParseToTypeOfClause();
                return new MvApplyExpression(expr, toTypeOf);
            }

            return null;
        }

        private static readonly Func<QueryParser, MvApplyExpression> FnParseMvApplyExpression =
            qp => qp.ParseMvApplyExpression();

        private static readonly Func<MvApplyExpression> CreateMissingMvApplyExpression = () =>
            new MvApplyExpression(CreateMissingExpression(), null);

        private static readonly IReadOnlyList<SyntaxKind> s_mvApplyExpressionListEnd =
            new[] { SyntaxKind.LimitKeyword, SyntaxKind.IdKeyword, SyntaxKind.OnKeyword };

        private static readonly Func<QueryParser, bool> FnScanMvApplyExpressionListEnd =
            qp => qp.ScanCustomListEnd(s_mvApplyExpressionListEnd);

        private SyntaxList<SeparatedElement<MvApplyExpression>> ParseMvApplyExpressionList()
        {
            if (PeekToken().Kind == SyntaxKind.ToKeyword)
            {
                var position = GetResetPoint();

                // if only one item that is just "to typeof(xxx)" then allow expression to be null w/o error
                var clause = ParseToTypeOfClause();
                if (PeekToken().Kind != SyntaxKind.CommaToken)
                {
                    return new SyntaxList<SeparatedElement<MvApplyExpression>>(new[] {
                        new SeparatedElement<MvApplyExpression>(new MvApplyExpression(null, clause))});
                }
                else
                {
                    Reset(position);
                }
            }

            return ParseCommaList(FnParseMvApplyExpression, CreateMissingMvApplyExpression, FnScanMvApplyExpressionListEnd, oneOrMore: true);
        }

        private MvApplyRowLimitClause ParseMvApplyRowLimitClause()
        {
            var keyword = ParseToken(SyntaxKind.LimitKeyword);
            if (keyword != null)
            {
                var expr = ParseUnnamedExpression() ?? CreateMissingExpression();
                return new MvApplyRowLimitClause(keyword, expr);
            }

            return null;
        }

        private MvApplyContextIdClause ParseMvApplyContextIdClause()
        {
            var keyword = ParseToken(SyntaxKind.IdKeyword);
            if (keyword != null)
            {
                var expr = ParseUnnamedExpression() ?? CreateMissingExpression();
                return new MvApplyContextIdClause(keyword, expr);
            }

            return null;
        }

        private MvApplySubqueryExpression ParseMvApplySubqueryExpression()
        {
            var open = ParseToken(SyntaxKind.OpenParenToken);
            if (open != null)
            {
                var expr = ParseContextualSubExpression() ?? CreateMissingExpression();
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new MvApplySubqueryExpression(open, expr, close);
            }

            return null;
        }

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_mvApplyOperatorParmeterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.MvApplyParameters);

        private static MvApplySubqueryExpression CreateMissingMvApplySubqueryExpression() =>
            new MvApplySubqueryExpression(
                CreateMissingToken(SyntaxKind.OpenParenToken),
                CreateMissingExpression(),
                CreateMissingToken(SyntaxKind.CloseParenToken));

        private MvApplyOperator ParseMvApplyOperator()
        {
            var keyword = ParseToken(SyntaxKind.MvApplyKeyword) ?? ParseToken(SyntaxKind.MvDashApplyKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_mvApplyOperatorParmeterMap, knownParametersOnly: true);
                var expressions = ParseMvApplyExpressionList();
                var rowLimit = ParseMvApplyRowLimitClause();
                var idClause = ParseMvApplyContextIdClause();
                var onKeyword = ParseRequiredToken(SyntaxKind.OnKeyword);
                var subquery = ParseMvApplySubqueryExpression() ?? CreateMissingMvApplySubqueryExpression();
                return new MvApplyOperator(keyword, parameters, expressions, rowLimit, idClause, onKeyword, subquery);
            }

            return null;
        }

#endregion

#region evaluate

        private EvaluateSchemaClause ParseEvaluateSchemaClause()
        {
            var colon = ParseToken(SyntaxKind.ColonToken);
            if (colon != null)
            {
                var schema = ParseSchemaMultipartType() ?? CreateMissingSchema();
                return new EvaluateSchemaClause(colon, schema);
            }

            return null;
        }

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_evaluateOperatorParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.EvaluateParameters);

        private EvaluateOperator ParseEvaluateOperator()
        {
            var keyword = ParseToken(SyntaxKind.EvaluateKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_evaluateOperatorParameterMap);
                var functionCall = ParseFunctionCallExpression() ?? CreateMissingFunctionCallExpression();
                var schema = ParseEvaluateSchemaClause();
                return new EvaluateOperator(keyword, parameters, functionCall, schema);
            }

            return null;
        }

#endregion

#region parse / parse-where

        private SyntaxNode ParseParseWithExpression()
        {
            return ParseStarExpression()
                ?? ParseStringOrCompoundStringLiteral()
                ?? ParseNameAndOptionalTypeDeclaration();
        }

        private static readonly Func<QueryParser, SyntaxNode> FnParseParseWithExpression =
            qp => qp.ParseParseWithExpression();

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_parseOperatorParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.ParseParameters);

        private ParseOperator ParseParseOperator()
        {
            var keyword = ParseToken(SyntaxKind.ParseKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_parseOperatorParameterMap);
                var expr = ParseUnnamedExpression() ?? CreateMissingExpression();
                var with = ParseRequiredToken(SyntaxKind.WithKeyword);
                var withExprs = ParseList(FnParseParseWithExpression);
                return new ParseOperator(keyword, parameters, expr, with, withExprs);
            }

            return null;
        }

        private ParseWhereOperator ParseParseWhereOperator()
        {
            var keyword = ParseToken(SyntaxKind.ParseWhereKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_parseOperatorParameterMap);
                var expr = ParseUnnamedExpression() ?? CreateMissingExpression();
                var with = ParseRequiredToken(SyntaxKind.WithKeyword);
                var withExprs = ParseList(FnParseParseWithExpression);
                return new ParseWhereOperator(keyword, parameters, expr, with, withExprs);
            }

            return null;
        }

#endregion

#region project / project-rename / project-away / project-keep / project-reorder

        private ProjectOperator ParseProjectOperator()
        {
            var keyword = ParseToken(SyntaxKind.ProjectKeyword);
            if (keyword != null)
            {
                var expressions = ParseCommaList(FnParseNamedExpression, CreateMissingExpression, FnScanCommonListEnd);
                return new ProjectOperator(keyword, expressions);
            }

            return null;
        }

        private ProjectRenameOperator ParseProjectRenameOperator()
        {
            var keyword = ParseToken(SyntaxKind.ProjectRenameKeyword);
            if (keyword != null)
            {
                var expressions = ParseCommaList(FnParseNamedExpression, CreateMissingExpression, FnScanCommonListEnd);
                return new ProjectRenameOperator(keyword, expressions);
            }

            return null;
        }

        private static readonly Func<QueryParser, Expression> FnParseSimpleOrWildcardedNameReferenceExpression =
            qp => qp.ParseWildcardedNameReference() ?? qp.ParseNameReference();

        private ProjectAwayOperator ParseProjectAwayOperator()
        {
            var keyword = ParseToken(SyntaxKind.ProjectAwayKeyword);
            if (keyword != null)
            {
                var expressions = ParseCommaList(FnParseSimpleOrWildcardedNameReferenceExpression, CreateMissingExpression, FnScanCommonListEnd);
                return new ProjectAwayOperator(keyword, expressions);
            }

            return null;
        }

        private ProjectKeepOperator ParseProjectKeepOperator()
        {
            var keyword = ParseToken(SyntaxKind.ProjectKeepKeyword);
            if (keyword != null)
            {
                var expressions = ParseCommaList(FnParseSimpleOrWildcardedNameReferenceExpression, CreateMissingExpression, FnScanCommonListEnd);
                return new ProjectKeepOperator(keyword, expressions);
            }

            return null;
        }

        private Expression ParseProjectReorderExpression()
        {
            Expression expr = ParseWildcardedNameReference() ?? ParseNameReference();
            if (expr != null)
            {
                var kind = PeekToken().Kind;
                if (kind == SyntaxKind.AscKeyword || kind == SyntaxKind.DescKeyword)
                {
                    expr = new OrderedExpression(expr, ParseRequiredOrderingNoNullsClause());
                }
            }

            return expr;
        }

        private static readonly Func<QueryParser, Expression> FnParseProjectReorderExpression =
            qp => qp.ParseProjectReorderExpression();

        private ProjectReorderOperator ParseProjectReorderOperator()
        {
            var keyword = ParseToken(SyntaxKind.ProjectReorderKeyword);
            if (keyword != null)
            {
                var expressions = ParseCommaList(FnParseProjectReorderExpression, CreateMissingExpression, FnScanCommonListEnd, oneOrMore: true);
                return new ProjectReorderOperator(keyword, expressions);
            }

            return null;
        }

#endregion

#region sample / sample-distinct

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_sampleOperatorParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.SampleParameters);

        private SampleOperator ParseSampleOperator()
        {
            var keyword = ParseToken(SyntaxKind.SampleKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_sampleOperatorParameterMap, knownParametersOnly: true);
                var expr = ParseNamedExpression() ?? CreateMissingExpression();
                return new SampleOperator(keyword, parameters, expr);
            }

            return null;
        }

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_sampleDistinctOperatorParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.SampleDistinctParameters);

        private SampleDistinctOperator ParseSampleDistinctOperator()
        {
            var keyword = ParseToken(SyntaxKind.SampleDistinctKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_sampleDistinctOperatorParameterMap, knownParametersOnly: true);
                var expr = ParseNamedExpression() ?? CreateMissingExpression();
                var ofKeyword = ParseRequiredToken(SyntaxKind.OfKeyword);
                var ofExpr = ParseNamedExpression() ?? CreateMissingExpression();
                return new SampleDistinctOperator(keyword, parameters, expr, ofKeyword, ofExpr);
            }

            return null;
        }

#endregion

#region reduce

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_reduceOperatorWithParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.ReduceWithParameters);

        private ReduceByWithClause ParseReduceByWithClause()
        {
            var keyword = ParseToken(SyntaxKind.WithKeyword);
            if (keyword != null)
            {
                var withParameters = ParseQueryOperatorParameterCommaList(s_reduceOperatorWithParameterMap);
                return new ReduceByWithClause(keyword, withParameters);
            }

            return null;
        }

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_reduceOperatorParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.ReduceWithParameters);

        private ReduceByOperator ParseReduceByOperator()
        {
            var keyword = ParseToken(SyntaxKind.ReduceKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_reduceOperatorParameterMap);
                var byKeyword = ParseRequiredToken(SyntaxKind.ByKeyword);
                var expr = ParseNamedExpression() ?? CreateMissingExpression();
                var withClause = ParseReduceByWithClause();
                return new ReduceByOperator(keyword, parameters, byKeyword, expr, withClause);
            }

            return null;
        }

#endregion

#region summarize

        private bool ScanSummarizeByClauseExpressionListEnd()
        {
            if (PeekToken().Kind == SyntaxKind.BinKeyword && PeekToken(1).Kind == SyntaxKind.EqualToken)
                return true;
            return ScanCommonListEnd();
        }

        private static readonly Func<QueryParser, bool> FnScanSummarizeByClauseExpressionListEnd =
            qp => qp.ScanSummarizeByClauseExpressionListEnd();

        private SummarizeByClause ParseSummarizeByClause()
        {
            var keyword = ParseToken(SyntaxKind.ByKeyword);
            if (keyword != null)
            {
                var expressions = ParseCommaList(FnParseNamedExpression, CreateMissingExpression, FnScanSummarizeByClauseExpressionListEnd, oneOrMore: true);
                return new SummarizeByClause(keyword, expressions);

            }

            return null;
        }

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_summarizeOperatorParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.SummarizeParameters);

        private static readonly Func<QueryParser, bool> FnScanSummarizeExpressionListEnd =
            qp => qp.ScanCustomListEnd(SyntaxKind.ByKeyword);

        private SummarizeOperator ParseSummarizeOperator()
        {
            var keyword = ParseToken(SyntaxKind.SummarizeKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_summarizeOperatorParameterMap, knownParametersOnly: true);
                var expressions = ParseCommaList(FnParseNamedExpression, CreateMissingExpression, FnScanSummarizeExpressionListEnd);
                var byClause = ParseSummarizeByClause();
                return new SummarizeOperator(keyword, parameters, expressions, byClause);
            }

            return null;
        }

#endregion

#region distinct

        private Expression ParseDistinctExpression()
        {
            return ParseStarExpression()
                ?? ParseUnnamedExpression();
        }

        private static readonly Func<QueryParser, Expression> FnParseDistinctExpression =
            qp => qp.ParseDistinctExpression();

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_distinctOperatorParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.DistinctParameters);

        private DistinctOperator ParseDistinctOperator()
        {
            var keyword = ParseToken(SyntaxKind.DistinctKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_distinctOperatorParameterMap);
                var expressions = ParseCommaList(FnParseDistinctExpression, CreateMissingExpression, FnScanCommonListEnd, oneOrMore: true);
                return new DistinctOperator(keyword, parameters, expressions);
            }

            return null;
        }

#endregion

#region take

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_takeOperatorParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.TakeParameters);

        private TakeOperator ParseTakeOperator()
        {
            var keyword = ParseToken(SyntaxKind.TakeKeyword) ?? ParseToken(SyntaxKind.LimitKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_takeOperatorParameterMap, knownParametersOnly: true);
                var expr = ParseNamedExpression() ?? CreateMissingExpression();
                return new TakeOperator(keyword, parameters, expr);
            }

            return null;
        }

#endregion

#region order / sort

        private SyntaxToken CreateMissingFirstOrLastToken() =>
            SyntaxToken.Missing("", SyntaxKind.FirstKeyword, new[] { DiagnosticFacts.GetMissingFirstOrLast() });

        private OrderingNullsClause ParseOrderingNullsClause()
        {
            var keyword = ParseToken(SyntaxKind.NullsKeyword);
            if (keyword != null)
            {
                var firstOrLast = ParseToken(SyntaxKind.FirstKeyword) ?? ParseToken(SyntaxKind.LastKeyword) ?? CreateMissingFirstOrLastToken();
                return new OrderingNullsClause(keyword, firstOrLast);
            }

            return null;
        }

        private OrderingClause ParseRequiredOrderingClause()
        {
            var keyword = ParseToken(SyntaxKind.AscKeyword) ?? ParseToken(SyntaxKind.DescKeyword);
            var nulls = ParseOrderingNullsClause();
            return new OrderingClause(keyword, nulls);
        }

        private OrderingClause ParseRequiredOrderingNoNullsClause()
        {
            var keyword = ParseToken(SyntaxKind.AscKeyword) ?? ParseToken(SyntaxKind.DescKeyword);
            return new OrderingClause(keyword, null);
        }

        private Expression ParseOrderedExpression()
        {
            var expr = ParseNamedExpression();
            if (expr != null)
            {
                var kind = PeekToken().Kind;
                switch (kind)
                {
                    case SyntaxKind.AscKeyword:
                    case SyntaxKind.DescKeyword:
                    case SyntaxKind.NullsKeyword:
                        expr = new OrderedExpression(expr, ParseRequiredOrderingClause());
                        break;
                }
            }

            return expr;
        }

        private static readonly Func<QueryParser, Expression> FnParseOrderedExpression =
            qp => qp.ParseOrderedExpression();

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_sortOperatorParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.SortParameters);

        private SortOperator ParseSortOperator()
        {
            var keyword = ParseToken(SyntaxKind.OrderKeyword) ?? ParseToken(SyntaxKind.SortKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_sortOperatorParameterMap);
                var byKeyword = ParseRequiredToken(SyntaxKind.ByKeyword);
                var expressions = ParseCommaList(FnParseOrderedExpression, CreateMissingExpression, FnScanCommonListEnd, oneOrMore: true);
                return new SortOperator(keyword, parameters, byKeyword, expressions);
            }

            return null;
        }

#endregion

#region scan

        private ScanAssignment ParseScanAssignment()
        {
            var name = ParseNameReference();
            if (name != null)
            {
                var equal = ParseRequiredToken(SyntaxKind.EqualToken);
                var expr = ParseUnnamedExpression() ?? CreateMissingExpression();
                return new ScanAssignment(name, equal, expr);
            }

            return null;
        }

        private static readonly Func<QueryParser, ScanAssignment> FnParseScanAssignment =
            qp => qp.ParseScanAssignment();

        private static readonly Func<ScanAssignment> CreateMissingScanAssignment = () =>
            new ScanAssignment(
                new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                SyntaxToken.Missing(SyntaxKind.EqualEqualToken),
                new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                new[] { DiagnosticFacts.GetMissingName() });

        private ScanComputationClause ParseScanComputationClause()
        {
            var arrow = ParseToken(SyntaxKind.FatArrowToken);
            if (arrow != null)
            {
                var assignments = ParseCommaList(FnParseScanAssignment, CreateMissingScanAssignment, FnScanCommonListEnd, oneOrMore: true);
                return new ScanComputationClause(arrow, assignments);
            }

            return null;
        }

        private ScanStep ParseScanStep()
        {
            var keyword = ParseToken(SyntaxKind.StepKeyword);
            if (keyword != null)
            {
                var name = ParseNameDeclaration() ?? CreateMissingNameDeclaration();
                var optional = ParseToken(SyntaxKind.OptionalKeyword);
                var colon = ParseRequiredToken(SyntaxKind.ColonToken);
                var expr = ParseUnnamedExpression() ?? CreateMissingExpression();
                var computation = ParseScanComputationClause();
                var semicolon = ParseRequiredToken(SyntaxKind.SemicolonToken);
                return new ScanStep(keyword, name, optional, colon, expr, computation, semicolon);
            }

            return null;
        }

        private static readonly Func<QueryParser, ScanStep> FnParseScanStep =
            qp => qp.ParseScanStep();

        private static readonly IReadOnlyList<SyntaxKind> s_scanListEnd =
            new[] { SyntaxKind.PartitionKeyword, SyntaxKind.OrderKeyword, SyntaxKind.ByKeyword, SyntaxKind.DeclareKeyword, SyntaxKind.WithKeyword };

        private static readonly Func<QueryParser, bool> FnScanScanListEnd =
            qp => qp.ScanCustomListEnd(s_scanListEnd);

        private ScanOrderByClause ParseScanOrderByClause()
        {
            var keyword = ParseToken(SyntaxKind.OrderKeyword);
            if (keyword != null)
            {
                var byKeyword = ParseRequiredToken(SyntaxKind.ByKeyword);
                var expressions = ParseCommaList(FnParseOrderedExpression, CreateMissingExpression, FnScanScanListEnd, oneOrMore: true);
                return new ScanOrderByClause(keyword, byKeyword, expressions);
            }

            return null;
        }

        private ScanPartitionByClause ParseScanParitionByClause()
        {
            var keyword = ParseToken(SyntaxKind.PartitionKeyword);
            if (keyword != null)
            {
                var byKeyword = ParseRequiredToken(SyntaxKind.ByKeyword);
                var expressions = ParseCommaList(FnParseUnnamedExpression, CreateMissingExpression, FnScanScanListEnd, oneOrMore: true);
                return new ScanPartitionByClause(keyword, byKeyword, expressions);
            }

            return null;
        }

        private ScanDeclareClause ParseScanDeclareClause()
        {
            var keyword = ParseToken(SyntaxKind.DeclareKeyword);
            if (keyword != null)
            {
                var open = ParseRequiredToken(SyntaxKind.OpenParenToken);
                var declarations = ParseCommaList(FnParseFunctionParameter, CreateMissingFunctionParameter, FnScanCommonListEnd, oneOrMore: true);
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new ScanDeclareClause(keyword, open, declarations, close);
            }

            return null;
        }

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_scanOperatorParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.ScanParameters);

        private ScanOperator ParseScanOperator()
        {
            var keyword = ParseToken(SyntaxKind.ScanKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_scanOperatorParameterMap);
                var order = ParseScanOrderByClause();
                var partition = ParseScanParitionByClause();
                var declare = ParseScanDeclareClause();
                var with = ParseRequiredToken(SyntaxKind.WithKeyword);
                var open = ParseRequiredToken(SyntaxKind.OpenParenToken);
                var steps = ParseList(FnParseScanStep, fnScanEnd: FnScanCommonListEnd);
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new ScanOperator(keyword, parameters, order, partition, declare, with, open, steps, close);
            }

            return null;
        }

#endregion

#region top / top-nested / top-hitters

        private TopHittersByClause ParseTopHittersByClause()
        {
            var keyword = ParseToken(SyntaxKind.ByKeyword);
            if (keyword != null)
            {
                var expr = ParseNamedExpression() ?? CreateMissingExpression();
                return new TopHittersByClause(keyword, expr);
            }

            return null;
        }

        private TopHittersOperator ParseTopHittersOperator()
        {
            var keyword = ParseToken(SyntaxKind.TopHittersKeyword);
            if (keyword != null)
            {
                var expr = ParseNamedExpression() ?? CreateMissingExpression();
                var ofKeyword = ParseRequiredToken(SyntaxKind.OfKeyword);
                var ofExpr = ParseNamedExpression() ?? CreateMissingExpression();
                var byClause = ParseTopHittersByClause();
                return new TopHittersOperator(keyword, expr, ofKeyword, ofExpr, byClause);
            }

            return null;
        }

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_topOperatorParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.TopParameters);

        private TopOperator ParseTopOperator()
        {
            var keyword = ParseToken(SyntaxKind.TopKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_topOperatorParameterMap, knownParametersOnly: true);
                var expr = ParseNamedExpression() ?? CreateMissingExpression();
                var byKeyword = ParseRequiredToken(SyntaxKind.ByKeyword);
                var byExpr = ParseOrderedExpression() ?? CreateMissingExpression();
                return new TopOperator(keyword, parameters, expr, byKeyword, byExpr);
            }

            return null;
        }

        private TopNestedWithOthersClause ParseTopNestedWithOthersClause()
        {
            var keyword = ParseToken(SyntaxKind.WithKeyword);
            if (keyword != null)
            {
                var others = ParseRequiredToken(SyntaxKind.OthersKeyword);
                var equal = ParseRequiredToken(SyntaxKind.EqualToken);
                var value = ParseLiteral() ?? ParseUnnamedExpression() ?? CreateMissingExpression();
                return new TopNestedWithOthersClause(keyword, others, equal, value);
            }

            return null;
        }

        private TopNestedClause ParseTopNestedClause()
        {
            var keyword = ParseToken(SyntaxKind.TopNestedKeyword);
            if (keyword != null)
            {
                var expr = ParseNamedExpression();
                var ofKeyword = ParseRequiredToken(SyntaxKind.OfKeyword);
                var ofExpr = ParseNamedExpression() ?? CreateMissingExpression();
                var withClause = ParseTopNestedWithOthersClause();
                var byKeyword = ParseRequiredToken(SyntaxKind.ByKeyword);
                var byExpr = ParseOrderedExpression() ?? CreateMissingExpression();
                return new TopNestedClause(keyword, expr, ofKeyword, ofExpr, withClause, byKeyword, byExpr);
            }

            return null;
        }

        private static readonly Func<QueryParser, TopNestedClause> FnParseTopNestedClause =
            qp => qp.ParseTopNestedClause();

        private static readonly Func<TopNestedClause> CreateMissingTopNestedClause = () =>
            new TopNestedClause(
                SyntaxToken.Missing(SyntaxKind.TopNestedKeyword),
                expression: null,
                ofKeyword: SyntaxToken.Missing(SyntaxKind.OfKeyword),
                ofExpression: new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                withOthersClause: null,
                byKeyword: SyntaxToken.Missing(SyntaxKind.ByKeyword),
                byExpression: new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                diagnostics: new[] { DiagnosticFacts.GetMissingClause() }
                );

        private TopNestedOperator ParseTopNestedOperator()
        {
            if (PeekToken().Kind == SyntaxKind.TopNestedKeyword)
            {
                var clauses = ParseCommaList(FnParseTopNestedClause, CreateMissingTopNestedClause, FnScanCommonListEnd, oneOrMore: true);
                return new TopNestedOperator(clauses);
            }

            return null;
        }

#endregion

#region union

        private Expression ParseUnionExpression()
        {
            var expr = ParseParenthesizedExpression();

            if (expr == null)
                expr = ParseWildcardedEntityExpression();

            if (expr == null)
                expr = ParseBracketedEntityNamePathElementSelector();

            if (expr == null)
                expr = ParseBarePathElementSelector();

            return expr;
        }

        private static readonly Func<QueryParser, Expression> FnParseUnionExpression =
            qp => qp.ParseUnionExpression();

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_unionOperatorParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.UnionParameters);

        private UnionOperator ParseUnionOperator()
        {
            var keyword = ParseToken(SyntaxKind.UnionKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_unionOperatorParameterMap);
                var expressions = ParseCommaList(FnParseUnionExpression, CreateMissingExpression, FnScanCommonListEnd, oneOrMore: true);
                return new UnionOperator(keyword, parameters, expressions);
            }

            return null;
        }

#endregion

#region as

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_asOperatorParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.AsParameters);

        private AsOperator ParseAsOperator()
        {
            var keyword = ParseToken(SyntaxKind.AsKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_asOperatorParameterMap, knownParametersOnly: true);
                var name = ParseNameDeclaration() ?? CreateMissingNameDeclaration();
                return new AsOperator(keyword, parameters, name);
            }

            return null;
        }

#endregion

#region serialize

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_serializeOperatorParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.SerializedParameters);

        private SerializeOperator ParseSerializeOperator()
        {
            var keyword = ParseToken(SyntaxKind.SerializeKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_serializeOperatorParameterMap, knownParametersOnly: true);
                var expressions = ParseCommaList(FnParseNamedExpression, CreateMissingExpression, FnScanCommonListEnd);
                return new SerializeOperator(keyword, parameters, expressions);
            }

            return null;
        }

#endregion

#region range

        private RangeOperator ParseRangeOperator()
        {
            // don't parse as range operator if it looks like the range function
            if (PeekToken().Kind == SyntaxKind.RangeKeyword && PeekToken(1).Kind != SyntaxKind.OpenParenToken)
            {
                var keyword = ParseToken();
                var name = ParseNameDeclaration() ?? CreateMissingNameDeclaration();
                var fromKeyword = ParseRequiredToken(SyntaxKind.FromKeyword);
                var fromExpr = ParseUnnamedExpression() ?? CreateMissingExpression();
                var toKeyword = ParseRequiredToken(SyntaxKind.ToKeyword);
                var toExpr = ParseUnnamedExpression() ?? CreateMissingExpression();
                var stepKeyword = ParseRequiredToken(SyntaxKind.StepKeyword);
                var stepExpr = ParseUnnamedExpression() ?? CreateMissingExpression();
                return new RangeOperator(keyword, name, fromKeyword, fromExpr, toKeyword, toExpr, stepKeyword, stepExpr);
            }

            return null;
        }

#endregion

#region invoke

        private InvokeOperator ParseInvokeOperator()
        {
            var keyword = ParseToken(SyntaxKind.InvokeKeyword);
            if (keyword != null)
            {
                var functionCall = ParseDotCompositeFunctionCall() ?? CreateMissingExpression();
                return new InvokeOperator(keyword, functionCall);
            }

            return null;
        }

#endregion

#region render

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_renderOperatorWithPropertiesMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.RenderWithProperties);

        private RenderWithClause ParseRenderWithClause()
        {
            var keyword = ParseToken(SyntaxKind.WithKeyword);
            if (keyword != null)
            {
                var open = ParseRequiredToken(SyntaxKind.OpenParenToken);
                var props = ParseQueryOperatorParameterCommaList(s_renderOperatorWithPropertiesMap);
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new RenderWithClause(keyword, open, props, close);
            }

            return null;
        }

        private static readonly IReadOnlyList<string> s_renderDeprecatedParameterNameListEndNames =
            new[] { "kind", "title", "accumulate", "with", "by" };

        private static readonly Func<QueryParser, bool> FnRenderDeprecatedParameterNameListEnd =
            qp => qp.ScanToken(s_renderDeprecatedParameterNameListEndNames) > 0;

        private NamedParameter ParseDeprecatedRenderProperty()
        {
            return ParseQueryOperatorParameter(QueryOperatorParameters.RenderKind)
                ?? ParseQueryOperatorParameter(QueryOperatorParameters.RenderTitle)
                ?? ParseQueryOperatorParameter(QueryOperatorParameters.RenderAccumulate)
                ?? (PeekToken().Kind == SyntaxKind.WithKeyword && PeekToken(1).Kind != SyntaxKind.OpenParenToken
                    ? ParseQueryOperatorParameter(QueryOperatorParameters.RenderWithDeprecated) : null)
                ?? ParseQueryOperatorParameter(QueryOperatorParameters.RenderByDeprecated, FnRenderDeprecatedParameterNameListEnd);
        }

        private static readonly Func<QueryParser, NamedParameter> FnParseDeprecatedRenderProperty =
            qp => qp.ParseDeprecatedRenderProperty();

        private RenderOperator ParseRenderOperator()
        {
            var keyword = ParseToken(SyntaxKind.RenderKeyword);
            if (keyword != null)
            {
                var chartType = ParseToken(KustoFacts.ChartTypes) ?? ParseToken(SyntaxKind.IdentifierToken) ?? CreateMissingNameToken(KustoFacts.ChartTypes);
                var parameters = ParseList(FnParseDeprecatedRenderProperty);
                var withClause = ParseRenderWithClause();
                return new RenderOperator(keyword, chartType, parameters, withClause);
            }

            return null;
        }

#endregion

#region print

        private PrintOperator ParsePrintOperator()
        {
            var keyword = ParseToken(SyntaxKind.PrintKeyword);
            if (keyword != null)
            {
                var expressions = ParseCommaList(FnParseNamedExpression, CreateMissingExpression, FnScanCommonListEnd, oneOrMore: true);
                return new PrintOperator(keyword, expressions);
            }

            return null;
        }

#endregion

#endregion

#region Query Expressions

        private QueryOperator ParseQueryOperator()
        {
            switch (PeekToken().Kind)
            {
                case SyntaxKind.AsKeyword:
                    return ParseAsOperator();
                case SyntaxKind.ConsumeKeyword:
                    return ParseConsumeOperator();
                case SyntaxKind.CountKeyword:
                    return ParseCountOperator();
                case SyntaxKind.DistinctKeyword:
                    return ParseDistinctOperator();
                case SyntaxKind.EvaluateKeyword:
                    return ParseEvaluateOperator();
                case SyntaxKind.ExecuteAndCacheKeyword:
                    return ParseExecuteAndCacheOperator();
                case SyntaxKind.ExtendKeyword:
                    return ParseExtendOperator();
                case SyntaxKind.FacetKeyword:
                    return ParseFacetOperator();
                case SyntaxKind.FilterKeyword:
                case SyntaxKind.WhereKeyword:
                    return ParseFilterOperator();
                case SyntaxKind.FindKeyword:
                    return ParseFindOperator();
                case SyntaxKind.ForkKeyword:
                    return ParseForkOperator();
                case SyntaxKind.GetSchemaKeyword:
                    return ParseGetSchemaOperator();
                case SyntaxKind.InvokeKeyword:
                    return ParseInvokeOperator();
                case SyntaxKind.JoinKeyword:
                    return ParseJoinOperator();
                case SyntaxKind.LookupKeyword:
                    return ParseLookupOperator();
                case SyntaxKind.MakeSeriesKeyword:
                    return ParseMakeSeriesOperator();
                case SyntaxKind.MvApplyKeyword:
                case SyntaxKind.MvDashApplyKeyword:
                    return ParseMvApplyOperator();
                case SyntaxKind.MvExpandKeyword:
                case SyntaxKind.MvDashExpandKeyword:
                    return ParseMvExpandOperator();
                case SyntaxKind.ParseKeyword:
                    return ParseParseOperator();
                case SyntaxKind.ParseWhereKeyword:
                    return ParseParseWhereOperator();
                case SyntaxKind.PartitionKeyword:
                    return ParsePartitionOperator();
                case SyntaxKind.PrintKeyword:
                    return ParsePrintOperator();
                case SyntaxKind.ProjectKeyword:
                    return ParseProjectOperator();
                case SyntaxKind.ProjectAwayKeyword:
                    return ParseProjectAwayOperator();
                case SyntaxKind.ProjectKeepKeyword:
                    return ParseProjectKeepOperator();
                case SyntaxKind.ProjectRenameKeyword:
                    return ParseProjectRenameOperator();
                case SyntaxKind.ProjectReorderKeyword:
                    return ParseProjectReorderOperator();
                case SyntaxKind.RangeKeyword:
                    return ParseRangeOperator();
                case SyntaxKind.ReduceKeyword:
                    return ParseReduceByOperator();
                case SyntaxKind.RenderKeyword:
                    return ParseRenderOperator();
                case SyntaxKind.SampleKeyword:
                    return ParseSampleOperator();
                case SyntaxKind.SampleDistinctKeyword:
                    return ParseSampleDistinctOperator();
                case SyntaxKind.ScanKeyword:
                    return ParseScanOperator();
                case SyntaxKind.SearchKeyword:
                    return ParseSearchOperator();
                case SyntaxKind.SerializeKeyword:
                    return ParseSerializeOperator();
                case SyntaxKind.SortKeyword:
                case SyntaxKind.OrderKeyword:
                    return ParseSortOperator();
                case SyntaxKind.SummarizeKeyword:
                    return ParseSummarizeOperator();
                case SyntaxKind.TakeKeyword:
                case SyntaxKind.LimitKeyword:
                    return ParseTakeOperator();
                case SyntaxKind.TopKeyword:
                    return ParseTopOperator();
                case SyntaxKind.TopHittersKeyword:
                    return ParseTopHittersOperator();
                case SyntaxKind.TopNestedKeyword:
                    return ParseTopNestedOperator();
                case SyntaxKind.UnionKeyword:
                    return ParseUnionOperator();
                default:
                    return null;
            }
        }

        private BadQueryOperator ParseBadQueryOperator()
        {
            var id = ParseToken(SyntaxKind.IdentifierToken);
            if (id != null)
            {
                return new BadQueryOperator(id, new[] { DiagnosticFacts.GetQueryOperatorExpected() });
            }

            return null;
        }

        private Expression ParsePipeExpression()
        {
            Expression expr = ParseQueryOperator();

            if (expr == null)
                expr = ParseUnnamedExpression();

            if (expr != null)
            {
                while (PeekToken().Kind == SyntaxKind.BarToken)
                {
                    expr = new PipeExpression(
                        expr,
                        ParseToken(),
                        ParseQueryOperator() ?? ParseBadQueryOperator() ?? CreateMissingQueryOperator());
                }
            }

            return expr;
        }

        private Expression ParsePipeSubExpression()
        {
            // allow all query operators to farse successfully and cache in semantic analysis
            return ParsePipeExpression();
        }

        private QueryOperator ParseFollowingPipeQueryOperator()
        {
            // allow all query operators to farse successfully and cache in semantic analysis
            return ParseQueryOperator();
        }

        // todo: check if this should just be normal PipeExpression too
        private Expression ParseContextualSubExpression()
        {
            return ParsePipeSubExpression();
        }

        private Expression ParseExpression()
        {
            return ParsePipeExpression();
        }

#endregion

#region Statements

#region alias

        private AliasStatement ParseAliasStatement()
        {
            var keyword = ParseToken(SyntaxKind.AliasKeyword);
            if (keyword != null)
            {
                var database = ParseRequiredToken(SyntaxKind.DatabaseKeyword);
                var name = ParseNameDeclaration() ?? CreateMissingNameDeclaration();
                var equal = ParseRequiredToken(SyntaxKind.EqualToken);
                var expr = ParseUnnamedExpression() ?? CreateMissingExpression();
                return new AliasStatement(keyword, database, name, equal, expr);
            }

            return null;
        }

#endregion

#region let

        private MaterializeExpression ParseMaterializeExpression()
        {
            var keyword = ParseToken(SyntaxKind.MaterializeKeyword);
            if (keyword != null)
            {
                var open = ParseRequiredToken(SyntaxKind.OpenParenToken);
                var expr = ParsePipeExpression() ?? CreateMissingExpression();
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new MaterializeExpression(keyword, open, expr, close);
            }

            return null;
        }

        private DefaultValueDeclaration ParseDefaultValueDeclaration()
        {
            var equal = ParseToken(SyntaxKind.EqualToken);
            if (equal != null)
            {
                var expr = ParseLiteral() ?? ParseIdentifierOrKeywordTokenLiteral() ?? CreateMissingExpression();
                return new DefaultValueDeclaration(equal, expr);
            }

            return null;
        }

        private FunctionParameter ParseFunctionParameter()
        {
            var name = ParseNameAndTypeDeclaration();
            if (name != null)
            {
                var defValue = ParseDefaultValueDeclaration();
                return new FunctionParameter(name, defValue);
            }

            return null;
        }

        private static readonly Func<QueryParser, FunctionParameter> FnParseFunctionParameter =
            qp => qp.ParseFunctionParameter();

        private static readonly Func<FunctionParameter> CreateMissingFunctionParameter = () =>
            new FunctionParameter(
                new NameAndTypeDeclaration(
                    new NameDeclaration(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                    SyntaxToken.Missing(SyntaxKind.ColonToken),
                    new PrimitiveTypeExpression(SyntaxToken.Missing(SyntaxKind.StringKeyword))),
                 null,
                 new[] { DiagnosticFacts.GetMissingParameter() });


        /// <summary>
        /// Parses function parameter list:  (name: type [= value], name: type [= value], ...)
        /// </summary>
        private FunctionParameters ParseFunctionParameters()
        {
            var open = ParseToken(SyntaxKind.OpenParenToken);
            if (open != null)
            {
                var parameters = ParseCommaList(FnParseFunctionParameter, CreateMissingFunctionParameter, FnScanCommonListEnd);
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new FunctionParameters(open, parameters, close);
            }

            return null;
        }

        private SeparatedElement<Statement> ParseFunctionBodyStatement()
        {
            switch (PeekToken().Kind)
            {
                case SyntaxKind.LetKeyword:
                    return new SeparatedElement<Statement>(
                        ParseLetStatement(), 
                        ParseRequiredToken(SyntaxKind.SemicolonToken));
                case SyntaxKind.QueryParametersKeyword:
                    return new SeparatedElement<Statement>(
                        ParseQueryParametersStatement(),
                        ParseRequiredToken(SyntaxKind.SemicolonToken));
                default:
                    return null;
            }
        }

        private static readonly Func<QueryParser, SeparatedElement<Statement>> FnParseFunctionBodyStatements =
            qp => qp.ParseFunctionBodyStatement();

        private static readonly Func<Statement> CreateMissingStatement = () =>
            new ExpressionStatement(
                new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                new[] { DiagnosticFacts.GetMissingStatement() });

        private static readonly Func<SeparatedElement<Statement>> CreateMissingStatementElement = () =>
            new SeparatedElement<Statement>(
                new ExpressionStatement(
                    new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                    new[] { DiagnosticFacts.GetMissingStatement() }));


        private FunctionBody ParseFunctionBody()
        {
            var open = ParseToken(SyntaxKind.OpenBraceToken);
            if (open != null)
            {
                var statements = ParseList(FnParseFunctionBodyStatements, CreateMissingStatementElement, FnScanCommonListEnd);
                var expr = ParseExpression();
                var semicolon = ParseToken(SyntaxKind.SemicolonToken);
                var close = ParseRequiredToken(SyntaxKind.CloseBraceToken);
                return new FunctionBody(open, statements, expr, semicolon, close);
            }

            return null;
        }

        private bool ScanFunctionDeclarationStart(int offset = 0)
        {
            switch (PeekToken(offset).Kind)
            {
                case SyntaxKind.ViewKeyword:
                    return true;
                case SyntaxKind.OpenParenToken:
                    var nextKind = PeekToken(offset + 1).Kind;
                    if (nextKind == SyntaxKind.CloseParenToken
                        || nextKind == SyntaxKind.AsteriskToken)
                        return true;
                    var nameLen = ScanName(offset + 1);
                    return nameLen > 0 && PeekToken(offset + 1 + nameLen).Kind == SyntaxKind.ColonToken;
                default:
                    return false;
            }
        }

        private static FunctionParameters CreateMissingFunctionParameters() =>
            new FunctionParameters(
                CreateMissingToken(SyntaxKind.OpenParenToken),
                SyntaxList<SeparatedElement<FunctionParameter>>.Empty(),
                CreateMissingToken(SyntaxKind.CloseParenToken));

        private static FunctionBody CreateMissingFunctionBody() =>
            new FunctionBody(
                CreateMissingToken(SyntaxKind.OpenBraceToken),
                SyntaxList<SeparatedElement<Statement>>.Empty(),
                null,
                null,
                CreateMissingToken(SyntaxKind.CloseBraceToken));

        private FunctionDeclaration ParseFunctionDeclaration()
        {
            switch (PeekToken().Kind)
            {
                case SyntaxKind.ViewKeyword:
                    return new FunctionDeclaration(
                        ParseToken(),
                        ParseFunctionParameters() ?? CreateMissingFunctionParameters(),
                        ParseFunctionBody() ?? CreateMissingFunctionBody());
                case SyntaxKind.OpenParenToken:
                    return new FunctionDeclaration(
                        null,
                        ParseFunctionParameters() ?? CreateMissingFunctionParameters(),
                        ParseFunctionBody() ?? CreateMissingFunctionBody());
                default:
                    return null;
            }
        }

        private LetStatement ParseLetStatement()
        {
            var keyword = ParseToken(SyntaxKind.LetKeyword);
            if (keyword != null)
            {
                var name = ParseNameDeclaration() ?? CreateMissingNameDeclaration();
                var equal = ParseRequiredToken(SyntaxKind.EqualToken);

                if (PeekToken().Kind == SyntaxKind.MaterializeKeyword)
                {
                    return new LetStatement(keyword, name, equal, ParseMaterializeExpression());
                }
                else if (ScanFunctionDeclarationStart())
                {
                    return new LetStatement(keyword, name, equal, ParseFunctionDeclaration());
                }
                else
                {
                    return new LetStatement(keyword, name, equal, ParseExpression() ?? CreateMissingExpression());
                }
            }

            return null;
        }

#endregion

#region set option

        private OptionValueClause ParseOptionValueClause()
        {
            var equal = ParseToken(SyntaxKind.EqualToken);
            if (equal != null)
            {
                var expr = ParseUnnamedExpression() ?? CreateMissingExpression();
                return new OptionValueClause(equal, expr);
            }

            return null;
        }

        private SetOptionStatement ParseSetOptionStatement()
        {
            var keyword = ParseToken(SyntaxKind.SetKeyword);
            if (keyword != null)
            {
                var name = ParseNameDeclaration() ?? CreateMissingNameDeclaration();
                var valueClause = ParseOptionValueClause();
                return new SetOptionStatement(keyword, name, valueClause);
            }

            return null;
        }

#endregion

#region declare query_parameters

        private QueryParametersStatement ParseQueryParametersStatement()
        {
            var keyword = ParseToken(SyntaxKind.DeclareKeyword);
            if (keyword != null)
            {
                var parametersKeyword = ParseRequiredToken(SyntaxKind.QueryParametersKeyword);
                var open = ParseRequiredToken(SyntaxKind.OpenParenToken);
                var parameters = ParseCommaList(FnParseFunctionParameter, CreateMissingFunctionParameter, FnScanCommonListEnd, oneOrMore: true);
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new QueryParametersStatement(keyword, parametersKeyword, open, parameters, close);
            }

            return null;
        }

#endregion

#region restrict

        private Expression ParseRestrictExpression()
        {
            return ParseWildcardedEntityExpression()
                ?? ParseNameReference();
        }

        private static readonly Func<QueryParser, Expression> FnParseRestrictExpression =
            qp => qp.ParseRestrictExpression();

        private RestrictStatement ParseRestrictStatement()
        {
            var keyword = ParseToken(SyntaxKind.RestrictKeyword);
            if (keyword != null)
            {
                var accessKeyword = ParseRequiredToken(SyntaxKind.AccessKeyword);
                var toKeyword = ParseRequiredToken(SyntaxKind.ToKeyword);
                var open = ParseRequiredToken(SyntaxKind.OpenParenToken);
                var expressions = ParseCommaList(FnParseRestrictExpression, CreateMissingExpression, FnScanCommonListEnd, oneOrMore: true);
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new RestrictStatement(keyword, accessKeyword, toKeyword, open, expressions, close);
            }

            return null;
        }

#endregion

#region declare pattern

        private PatternPathValue ParsePatternPathValue()
        {
            var dot = ParseToken(SyntaxKind.DotToken);
            if (dot != null)
            {
                var open = ParseRequiredToken(SyntaxKind.OpenBracketToken);
                var value = ParseLiteral() ?? CreateMissingStringLiteral();
                var close = ParseRequiredToken(SyntaxKind.CloseBracketToken);
                return new PatternPathValue(dot, open, value, close);
            }

            return null;
        }

        private SeparatedElement<Statement> ParsePatternMatchStatementElement()
        {
            var statement = ParseLetStatement();
            if (statement != null)
            {
                var semicolon = ParseRequiredToken(SyntaxKind.SemicolonToken);
                return new SeparatedElement<Statement>(statement, semicolon);
            }

            return null;
        }

        private static readonly Func<QueryParser, SeparatedElement<Statement>> FnParsePatternMatchStatementElement =
            qp => qp.ParsePatternMatchStatementElement();

        private FunctionBody ParseRequiredPatternMatchFunctionBody()
        {
            var open = ParseRequiredToken(SyntaxKind.OpenBraceToken);
            var statements = ParseList(FnParsePatternMatchStatementElement, CreateMissingStatementElement, FnScanCommonListEnd);
            var expr = ParseExpression();
            var semicolon = ParseToken(SyntaxKind.SemicolonToken);
            var close = ParseRequiredToken(SyntaxKind.CloseBraceToken);
            return new FunctionBody(open, statements, expr, semicolon, close);
        }

        private PatternMatch ParsePatternMatch()
        {
            var open = ParseToken(SyntaxKind.OpenParenToken);
            if (open != null)
            {
                var expressions = ParseCommaList(FnParseUnnamedExpression, CreateMissingStringLiteral, FnScanCommonListEnd, oneOrMore: true);
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                var pathValue = ParsePatternPathValue();
                var equal = ParseRequiredToken(SyntaxKind.EqualToken);
                var body = ParseRequiredPatternMatchFunctionBody();
                var semicolon = ParseRequiredToken(SyntaxKind.SemicolonToken);
                return new PatternMatch(new ExpressionList(open, expressions, close), pathValue, equal, body, semicolon);
            }

            return null;
        }

        private static readonly Func<QueryParser, PatternMatch> FnParsePatternMatch =
            qp => qp.ParsePatternMatch();

        private PatternPathParameter ParsePatternPathParameter()
        {
            var open = ParseToken(SyntaxKind.OpenBracketToken);
            if (open != null)
            {
                var name = ParseNameAndTypeDeclaration() ?? CreateMissingNameAndTypeDeclaration();
                var close = ParseRequiredToken(SyntaxKind.CloseBracketToken);
                return new PatternPathParameter(open, name, close);
            }

            return null;
        }

        private static readonly Func<PatternMatch> CreateMissingPatternMatch = () =>
            new PatternMatch(
                new ExpressionList(
                    SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                    SyntaxList<SeparatedElement<Expression>>.Empty(),
                    SyntaxToken.Missing(SyntaxKind.CloseParenToken)),
                null, // path value okay to be null
                SyntaxToken.Missing(SyntaxKind.EqualToken),
                new FunctionBody(
                    SyntaxToken.Missing(SyntaxKind.OpenBraceToken),
                    SyntaxList<SeparatedElement<Statement>>.Empty(),
                    null, // expression okay to be null
                    null, // semicolon okay to be null
                    SyntaxToken.Missing(SyntaxKind.CloseBraceToken)),
                SyntaxToken.Missing(SyntaxKind.SemicolonToken),
                new[] { DiagnosticFacts.GetMissingPatternMatch() });

        private PatternDeclaration ParsePatternDeclaration()
        {
            var equal = ParseToken(SyntaxKind.EqualToken);
            if (equal != null)
            {
                var openP = ParseRequiredToken(SyntaxKind.OpenParenToken);
                var decls = ParseCommaList(FnParseNameAndTypeDeclaration, CreateMissingNameAndTypeDeclaration, FnScanCommonListEnd);
                var closeP = ParseRequiredToken(SyntaxKind.CloseParenToken);
                var path = ParsePatternPathParameter();
                var openB = ParseRequiredToken(SyntaxKind.OpenBraceToken);
                var matches = ParseList(FnParsePatternMatch, CreateMissingPatternMatch, FnScanCommonListEnd, oneOrMore: true);
                var closeB = ParseRequiredToken(SyntaxKind.CloseBraceToken);
                return new PatternDeclaration(equal, openP, decls, closeP, path, openB, matches, closeB);
            }

            return null;
        }

        private PatternStatement ParsePatternStatement()
        {
            if (PeekToken().Kind == SyntaxKind.DeclareKeyword && PeekToken(1).Kind == SyntaxKind.PatternKeyword)
            {
                var declare = ParseToken();
                var pattern = ParseToken();
                var name = ParseNameDeclaration() ?? CreateMissingNameDeclaration();
                var declaration = ParsePatternDeclaration();
                return new PatternStatement(declare, pattern, name, declaration);
            }

            return null;
        }

#endregion

#endregion

#region Query Block

        private Statement ParseQueryBlockStatement()
        {
            switch (PeekToken().Kind)
            {
                case SyntaxKind.AliasKeyword:
                    return ParseAliasStatement();
                case SyntaxKind.LetKeyword:
                    return ParseLetStatement();
                case SyntaxKind.SetKeyword:
                    return ParseSetOptionStatement();
                case SyntaxKind.DeclareKeyword:
                    if (PeekToken(1).Kind == SyntaxKind.PatternKeyword)
                        return ParsePatternStatement();
                    else
                        return ParseQueryParametersStatement();
                case SyntaxKind.RestrictKeyword:
                    return ParseRestrictStatement();
                default:
                    var expr = ParseExpression();
                    if (expr != null)
                        return new ExpressionStatement(expr);
                    return null;
            }
        }

        private SyntaxList<SeparatedElement<Statement>> ParseQueryBlockStatementList()
        {
            var list = new List<SeparatedElement<Statement>>();

            var statement = ParseQueryBlockStatement();
            while (statement != null)
            {
                var semicolon = ParseToken(SyntaxKind.SemicolonToken);
                if (semicolon != null)
                {
                    list.Add(new SeparatedElement<Statement>(statement, semicolon));
                    statement = ParseQueryBlockStatement();
                    continue;
                }
                else
                {
                    var nextStatement = ParseQueryBlockStatement();
                    if (nextStatement != null)
                    {
                        list.Add(new SeparatedElement<Statement>(statement, CreateMissingToken(SyntaxKind.SemicolonToken)));
                    }
                    else
                    {
                        list.Add(new SeparatedElement<Statement>(statement, null));
                    }

                    statement = nextStatement;
                }
            }

            return new SyntaxList<SeparatedElement<Statement>>(list);
        }

        private SkippedTokens ParseSkippedTokens()
        {
            List<SyntaxToken> tokens = null;

            SyntaxKind kind;
            while ((kind = PeekToken().Kind) != SyntaxKind.EndOfTextToken && kind != SyntaxKind.None)
            {
                if (tokens == null)
                    tokens = new List<SyntaxToken>();

                tokens.Add(ParseToken());
            }

            if (tokens != null)
            {
                // just add the diagnostic to the first token to avoid the overwhemling underlines in the editor
                tokens[0] = (SyntaxToken)tokens[0].WithAdditionalDiagnostics(DiagnosticFacts.GetIncompleteFragment());
                return new SkippedTokens(new SyntaxList<SyntaxToken>(tokens));
            }

            return null;
        }

        /// <summary>
        /// Parses and entire query
        /// </summary>
        private QueryBlock ParseQuery() =>
            new QueryBlock(
                ParseQueryBlockStatementList(),
                ParseSkippedTokens(),
                ParseToken(SyntaxKind.EndOfTextToken));

#endregion

#region Experimental
#if false
        private readonly Stack<Expression> operandStack = new Stack<Expression>();
        private readonly Stack<SyntaxToken> operatorStack = new Stack<SyntaxToken>();

        private Expression ParsePrimaryExpression2()
        {
            if (PeekToken().Kind == SyntaxKind.AsteriskToken)
            {
                var kind = PeekToken(1).Kind;
                if (kind == SyntaxKind.EqualEqualToken
                    || GetStringOperationKind(kind) != SyntaxKind.None)
                {
                    return ParseStarExpression();
                }
            }

            return ParseUnaryPlusOrMinusExpression();
        }

        private Expression ParseUnnamedExpression2()
        {
            var opStackStart = operatorStack.Count;

            var expr = ParsePrimaryExpression2();
            if (expr == null)
                return null;

            operandStack.Push(expr);

            while (true)
            {
                var opKind = PeekToken().Kind;

                var precidence = GetPrecedenceLevel();

                // complete any pending expressions with tighter precedence
                while (operatorStack.Count > opStackStart
                    && GetPrecedenceLevel(operatorStack.Peek().Kind) <= precidence)
                {
                    var op = operatorStack.Pop();
                    var right = operandStack.Pop();
                    var left = operandStack.Pop();
                    var exprKind = GetInfixOperationKind(op.Kind);
                    operandStack.Push(new BinaryExpression(exprKind, left, op, right));
                }

                if (precidence == InfixPrecidence.None)
                    break;

                var opToken = ParseToken();

                switch (opKind)
                {
                    case SyntaxKind.InKeyword:
                    case SyntaxKind.InCsKeyword:
                    case SyntaxKind.NotInKeyword:
                    case SyntaxKind.NotInCsKeyword:
                        var left = operandStack.Pop();
                        operandStack.Push(new InExpression(GetInfixOperationKind(opKind), left, opToken, ParseRequiredInOperatorExpressionList()));
                        break;
                    case SyntaxKind.HasAnyKeyword:
                        left = operandStack.Pop();
                        operandStack.Push(new HasAnyExpression(SyntaxKind.HasAnyExpression, left, opToken, ParseRequiredInOperatorExpressionList()));
                        break;
                    case SyntaxKind.HasAllKeyword:
                        left = operandStack.Pop();
                        operandStack.Push(new HasAllExpression(SyntaxKind.HasAllExpression, left, opToken, ParseRequiredInOperatorExpressionList()));
                        break;
                    case SyntaxKind.BetweenKeyword:
                    case SyntaxKind.NotBetweenKeyword:
                        left = operandStack.Pop();
                        operandStack.Push(new BetweenExpression(GetInfixOperationKind(opKind), left, opToken, ParseRequiredExpressionCouple()));
                        break;
                    default:
                        operatorStack.Push(opToken);
                        operandStack.Push(ParsePrimaryExpression2() ?? CreateMissingExpression());
                        break;
                }
            }

            return operandStack.Pop();
        }

        private enum InfixPrecidence
        {
            String,
            Multiplicative,
            Additive,
            Relational,
            Equality,
            And,
            Or,
            None
        }

        private InfixPrecidence GetPrecedenceLevel()
        {
            var token = PeekToken();
            if (token.Kind == SyntaxKind.InKeyword && PeekToken(1).Kind == SyntaxKind.RangeKeyword)
                return InfixPrecidence.None;
            return GetPrecedenceLevel(token.Kind);
        }

        private static InfixPrecidence GetPrecedenceLevel(SyntaxKind opTokenKind)
        {
            switch (opTokenKind)
            {
                case SyntaxKind.EqualTildeToken:
                case SyntaxKind.BangTildeToken:
                case SyntaxKind.HasKeyword:
                case SyntaxKind.ColonToken:
                case SyntaxKind.NotHasKeyword:
                case SyntaxKind.HasCsKeyword:
                case SyntaxKind.NotHasCsKeyword:
                case SyntaxKind.HasPrefixKeyword:
                case SyntaxKind.NotHasPrefixKeyword:
                case SyntaxKind.HasPrefixCsKeyword:
                case SyntaxKind.NotHasPrefixCsKeyword:
                case SyntaxKind.HasSuffixKeyword:
                case SyntaxKind.NotHasSuffixKeyword:
                case SyntaxKind.HasSuffixCsKeyword:
                case SyntaxKind.NotHasSuffixCsKeyword:
                case SyntaxKind.LikeKeyword:
                case SyntaxKind.NotLikeKeyword:
                case SyntaxKind.LikeCsKeyword:
                case SyntaxKind.NotLikeCsKeyword:
                case SyntaxKind.ContainsKeyword:
                case SyntaxKind.NotContainsKeyword:
                case SyntaxKind.NotBangContainsKeyword:
                case SyntaxKind.ContainsCsKeyword:
                case SyntaxKind.Contains_CsKeyword:
                case SyntaxKind.NotContainsCsKeyword:
                case SyntaxKind.NotBangContainsCsKeyword:
                case SyntaxKind.StartsWithKeyword:
                case SyntaxKind.NotStartsWithKeyword:
                case SyntaxKind.StartsWithCsKeyword:
                case SyntaxKind.NotStartsWithCsKeyword:
                case SyntaxKind.EndsWithKeyword:
                case SyntaxKind.NotEndsWithKeyword:
                case SyntaxKind.EndsWithCsKeyword:
                case SyntaxKind.NotEndsWithCsKeyword:
                case SyntaxKind.MatchesRegexKeyword:
                    return InfixPrecidence.String;
                case SyntaxKind.AsteriskToken:
                case SyntaxKind.SlashToken:
                case SyntaxKind.PercentToken:
                    return InfixPrecidence.Multiplicative;
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return InfixPrecidence.Additive;
                case SyntaxKind.LessThanToken:
                case SyntaxKind.LessThanOrEqualToken:
                case SyntaxKind.GreaterThanToken:
                case SyntaxKind.GreaterThanOrEqualToken:
                    return InfixPrecidence.Relational;
                case SyntaxKind.EqualEqualToken:
                case SyntaxKind.BangEqualToken:
                case SyntaxKind.LessThanGreaterThanToken:
                case SyntaxKind.InKeyword:
                case SyntaxKind.InCsKeyword:
                case SyntaxKind.NotInKeyword:
                case SyntaxKind.NotInCsKeyword:
                case SyntaxKind.HasAnyKeyword:
                case SyntaxKind.HasAllKeyword:
                case SyntaxKind.BetweenKeyword:
                case SyntaxKind.NotBetweenKeyword:
                    return InfixPrecidence.Equality;
                case SyntaxKind.AndKeyword:
                    return InfixPrecidence.And;
                case SyntaxKind.OrKeyword:
                    return InfixPrecidence.Or;
                default:
                    return InfixPrecidence.None;
            }
        }

        private static SyntaxKind GetInfixOperationKind(SyntaxKind tokenKind)
        {
            switch (tokenKind)
            {
                case SyntaxKind.EqualTildeToken:
                    return SyntaxKind.EqualTildeExpression;
                case SyntaxKind.BangTildeToken:
                    return SyntaxKind.BangTildeExpression;
                case SyntaxKind.HasKeyword:
                    return SyntaxKind.HasExpression;
                case SyntaxKind.ColonToken:
                    return SyntaxKind.SearchExpression;
                case SyntaxKind.NotHasKeyword:
                    return SyntaxKind.NotHasExpression;
                case SyntaxKind.HasCsKeyword:
                    return SyntaxKind.HasCsExpression;
                case SyntaxKind.NotHasCsKeyword:
                    return SyntaxKind.NotHasCsExpression;
                case SyntaxKind.HasPrefixKeyword:
                    return SyntaxKind.HasPrefixExpression;
                case SyntaxKind.NotHasPrefixKeyword:
                    return SyntaxKind.NotHasPrefixExpression;
                case SyntaxKind.HasPrefixCsKeyword:
                    return SyntaxKind.HasPrefixCsExpression;
                case SyntaxKind.NotHasPrefixCsKeyword:
                    return SyntaxKind.NotHasPrefixCsExpression;
                case SyntaxKind.HasSuffixKeyword:
                    return SyntaxKind.HasSuffixExpression;
                case SyntaxKind.NotHasSuffixKeyword:
                    return SyntaxKind.NotHasSuffixExpression;
                case SyntaxKind.HasSuffixCsKeyword:
                    return SyntaxKind.HasSuffixCsExpression;
                case SyntaxKind.NotHasSuffixCsKeyword:
                    return SyntaxKind.NotHasSuffixCsExpression;
                case SyntaxKind.LikeKeyword:
                    return SyntaxKind.LikeExpression;
                case SyntaxKind.NotLikeKeyword:
                    return SyntaxKind.NotLikeExpression;
                case SyntaxKind.LikeCsKeyword:
                    return SyntaxKind.LikeCsExpression;
                case SyntaxKind.NotLikeCsKeyword:
                    return SyntaxKind.NotLikeCsExpression;
                case SyntaxKind.ContainsKeyword:
                    return SyntaxKind.ContainsExpression;
                case SyntaxKind.NotContainsKeyword:
                    return SyntaxKind.NotContainsExpression;
                case SyntaxKind.NotBangContainsKeyword:
                    return SyntaxKind.NotContainsExpression;
                case SyntaxKind.ContainsCsKeyword:
                    return SyntaxKind.ContainsCsExpression;
                case SyntaxKind.Contains_CsKeyword:
                    return SyntaxKind.ContainsCsExpression;
                case SyntaxKind.NotContainsCsKeyword:
                    return SyntaxKind.NotContainsCsExpression;
                case SyntaxKind.NotBangContainsCsKeyword:
                    return SyntaxKind.NotContainsCsExpression;
                case SyntaxKind.StartsWithKeyword:
                    return SyntaxKind.StartsWithExpression;
                case SyntaxKind.NotStartsWithKeyword:
                    return SyntaxKind.NotStartsWithExpression;
                case SyntaxKind.StartsWithCsKeyword:
                    return SyntaxKind.StartsWithCsExpression;
                case SyntaxKind.NotStartsWithCsKeyword:
                    return SyntaxKind.NotStartsWithCsExpression;
                case SyntaxKind.EndsWithKeyword:
                    return SyntaxKind.EndsWithExpression;
                case SyntaxKind.NotEndsWithKeyword:
                    return SyntaxKind.NotEndsWithExpression;
                case SyntaxKind.EndsWithCsKeyword:
                    return SyntaxKind.EndsWithCsExpression;
                case SyntaxKind.NotEndsWithCsKeyword:
                    return SyntaxKind.NotEndsWithCsExpression;
                case SyntaxKind.MatchesRegexKeyword:
                    return SyntaxKind.MatchesRegexExpression;

                case SyntaxKind.AsteriskToken:
                    return SyntaxKind.MultiplyExpression;
                case SyntaxKind.SlashToken:
                    return SyntaxKind.DivideExpression;
                case SyntaxKind.PercentToken:
                    return SyntaxKind.ModuloExpression;

                case SyntaxKind.PlusToken:
                    return SyntaxKind.AddExpression;
                case SyntaxKind.MinusToken:
                    return SyntaxKind.SubtractExpression;

                case SyntaxKind.LessThanToken:
                    return SyntaxKind.LessThanExpression;
                case SyntaxKind.LessThanOrEqualToken:
                    return SyntaxKind.LessThanOrEqualExpression;
                case SyntaxKind.GreaterThanToken:
                    return SyntaxKind.GreaterThanExpression;
                case SyntaxKind.GreaterThanOrEqualToken:
                    return SyntaxKind.GreaterThanOrEqualExpression;

                case SyntaxKind.EqualEqualToken:
                    return SyntaxKind.EqualExpression;
                case SyntaxKind.BangEqualToken:
                    return SyntaxKind.NotEqualExpression;
                case SyntaxKind.LessThanGreaterThanToken:
                    return SyntaxKind.NotEqualExpression;
                case SyntaxKind.InKeyword:
                    return SyntaxKind.InExpression;
                case SyntaxKind.InCsKeyword:
                    return SyntaxKind.InCsExpression;
                case SyntaxKind.NotInKeyword:
                    return SyntaxKind.NotInExpression;
                case SyntaxKind.NotInCsKeyword:
                    return SyntaxKind.NotInCsExpression;
                case SyntaxKind.HasAnyKeyword:
                    return SyntaxKind.HasAnyExpression;
                case SyntaxKind.HasAllKeyword:
                    return SyntaxKind.HasAllExpression;
                case SyntaxKind.BetweenKeyword:
                    return SyntaxKind.BetweenExpression;
                case SyntaxKind.NotBetweenKeyword:
                    return SyntaxKind.NotBetweenExpression;

                case SyntaxKind.AndKeyword:
                    return SyntaxKind.AndExpression;
                case SyntaxKind.OrKeyword:
                    return SyntaxKind.OrExpression;

                default:
                    return SyntaxKind.None;
            }
        }
#endif

#endregion
    }
}