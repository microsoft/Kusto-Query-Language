using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kusto.Language.Syntax;
using Kusto.Language.Utils;

namespace Kusto.Language.Parsing
{
    internal enum AllowedNameKind
    {
        /// <summary>
        /// Allow either declared or known query operator names.
        /// </summary>
        DeclaredOrKnown,

        /// <summary>
        /// Allow any known query operator parameter name as a possible name.
        /// </summary>
        KnownOnly,

        /// <summary>
        /// Allow only query operator parameter names specifically declared for the operator
        /// </summary>
        DeclaredOnly
    }

    public class QueryParser
    {
        private readonly Source<LexicalToken> _source;
        private readonly ParseOptions _options;
        private int _pos;

        private QueryParser(Source<LexicalToken> source, int start, ParseOptions options)
        {
            _source = source;
            _options = options ?? ParseOptions.Default;
            _pos = start;
        }

        private QueryParser(LexicalToken[] tokens, int start, ParseOptions options)
            : this(new ArraySource<LexicalToken>(tokens), start, options)
        {
        }

        public static Expression ParseExpression(LexicalToken[] tokens, int start = 0, ParseOptions options = null)
        {
            return new QueryParser(tokens, start, options).ParseExpression();
        }

        public static Expression ParseExpression(string text, ParseOptions options = null)
        {
            return ParseExpression(TokenParser.ParseTokens(text, options), 0, options);
        }

        public static QueryBlock ParseQuery(LexicalToken[] tokens, int start = 0, ParseOptions options = null)
        {
            return new QueryParser(tokens, start, options).ParseQuery();
        }

        public static QueryBlock ParseQuery(string text, ParseOptions options = null)
        {
            return ParseQuery(TokenParser.ParseTokens(text, options), 0, options);
        }

        public static FunctionParameters ParseFunctionParameters(LexicalToken[] tokens, int start = 0, ParseOptions options = null)
        {
            return new QueryParser(tokens, start, options).ParseFunctionParameters();
        }

        public static FunctionParameters ParseFunctionParameters(string text, ParseOptions options = null)
        {
            return ParseFunctionParameters(TokenParser.ParseTokens(text, options), 0, options);
        }

        public static FunctionParameter ParseFunctionParameter(LexicalToken[] tokens, int start = 0, ParseOptions options = null)
        {
            return new QueryParser(tokens, start, options).ParseFunctionParameter();
        }

        public static FunctionParameter ParseFunctionParameter(string text, ParseOptions options = null)
        {
            return ParseFunctionParameter(TokenParser.ParseTokens(text, options), 0, options);
        }

        public static FunctionBody ParseFunctionBody(LexicalToken[] tokens, int start = 0, ParseOptions options = null)
        {
            return new QueryParser(tokens, start, options).ParseFunctionBody();
        }

        public static FunctionBody ParseFunctionBody(string text, ParseOptions options = null)
        {
            return ParseFunctionBody(TokenParser.ParseTokens(text, options), 0, options);
        }

        public static Expression ParseEntityPath(LexicalToken[] tokens, int start = 0, ParseOptions options = null)
        {
            return new QueryParser(tokens, start, options).ParseEntityPathExpression();
        }

        public static Expression ParseEntityPath(string text, ParseOptions options = null)
        {
            return ParseEntityPath(TokenParser.ParseTokens(text, options), 0, options);
        }

        public static Expression ParseEntityGroup(LexicalToken[] tokens, int start = 0, ParseOptions options = null)
        {
            return new QueryParser(tokens, start, options).ParseEntityGroup();
        }

        public static Expression ParseEntityGroup(string text, ParseOptions options = null)
        {
            return ParseEntityGroup(TokenParser.ParseTokens(text, options), 0, options);
        }

        public static Expression ParseLiteral(LexicalToken[] tokens, int start = 0, ParseOptions options = null)
        {
            return new QueryParser(tokens, start, options).ParseLiteral();
        }

        public static Expression ParseLiteral(string text, ParseOptions options = null)
        {
            return ParseLiteral(TokenParser.ParseTokens(text, options), 0, options);
        }

        public static RowSchema ParseRowSchema(LexicalToken[] tokens, int start = 0, ParseOptions options = null)
        {
            return new QueryParser(tokens, start, options).ParseRowSchema();
        }

        public static RowSchema ParseRowSchema(string text, ParseOptions options = null)
        {
            return ParseRowSchema(TokenParser.ParseTokens(text, options), 0, options);
        }

        #region Stack Safe Parsing

        /// <summary>
        /// The maximum expression depth recursive parsing will allow before switching to a stack safe parsing strategy.
        /// </summary>
        const int MaxDepth = 300;

        /// <summary>
        /// The current expression depth.
        /// </summary>
        private int _depth;

        /// <summary>
        /// The safe safe grammar parser.
        /// </summary>
        private StackSafeParser<LexicalToken> _safeParser;

        /// <summary>
        /// The output list used for grammar parsing.
        /// </summary>
        private List<object> _safeOutput;

        /// <summary>
        /// The grammar to parse with the stack safe parser
        /// </summary>
        private QueryGrammar _safeQueryGrammar;

        /// <summary>
        /// Parse using fnParse unless max depth has been exceeded then safe parse fnGrammar.
        /// </summary>
        private TResult StackSafeParse<TResult>(
            Func<QueryParser, TResult> fnParse,
            Func<QueryGrammar, Parser<LexicalToken, TResult>> fnGrammar)
        {
            if (_depth > MaxDepth)
            {
                if (_safeParser == null)
                {
                    _safeOutput = new List<object>();
                    _safeParser = new StackSafeParser<LexicalToken>(_source, _safeOutput);
                    _safeQueryGrammar = QueryGrammar.From(GlobalState.Default.WithParseOptions(_options));
                }

                var grammar = fnGrammar(_safeQueryGrammar);
                var len = _safeParser.Parse(grammar, _pos, 0);
                if (len >= 0)
                {
                    _pos += len;
                    var result = (TResult)_safeOutput[0];
                    _safeOutput.Clear();
                    return result;
                }
                else
                {
                    return default(TResult);
                }
            }
            else
            {
                _depth++;
                var result = fnParse(this);
                _depth--;
                return result;
            }
        }

        #endregion

        #region Reset Points

        /// <summary>
        /// Gets the reset point for the current input position.
        /// </summary>
        internal int GetResetPoint()
        {
            return _pos;
        }

        /// <summary>
        /// Resets the parser to the reset point.
        /// </summary>
        internal void Reset(int resetPoint)
        {
            _pos = resetPoint;
        }

        /// <summary>
        /// Returns the best result of the set of parsers
        /// </summary>
        private TElement ParseBest<TElement>(IReadOnlyList<Func<QueryParser, TElement>> parsers)
            where TElement : SyntaxElement
        {
            var start = GetResetPoint();
            var bestEnd = start;
            TElement bestResult = null;

            foreach (var parser in parsers)
            {
                Reset(start);
                var result = parser(this);
                var end = GetResetPoint();
                if (end > bestEnd)
                {
                    bestEnd = end;
                    bestResult = result;
                }
            }

            Reset(bestEnd);
            return bestResult;
        }

        #endregion

        #region Tokens

        /// <summary>
        /// Invokes the parser, but limits the amount of tokens it can consume.
        /// </summary>
        private TSyntax Limit<TSyntax>(int limit, Func<QueryParser, TSyntax> fnParser)
        {
            var source = new LimitSource<LexicalToken>(_source, _pos + limit);
            var limitedParser = new QueryParser(source, _pos, _options);
            var result = fnParser(limitedParser);
            _pos = limitedParser._pos;
            return result;
        }

        /// <summary>
        /// Returns the next <see cref="LexicalToken"/>
        /// </summary>
        private LexicalToken PeekToken()
        {
            return !_source.IsEnd(_pos) ? _source.Peek(_pos) : NoToken;
        }

        /// <summary>
        /// Returns the next <see cref="LexicalToken"/>
        /// </summary>
        private LexicalToken PeekToken(int offset)
        {
            var index = _pos + offset;
            return !_source.IsEnd(index) ? _source.Peek(index) : NoToken;
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
        //private SyntaxToken ParseRequiredToken(IReadOnlyList<SyntaxKind> kinds)
        //{
        //    return ParseToken(kinds) ?? CreateMissingToken(kinds);
        //}

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
        private SyntaxToken ParseToken(string text, SyntaxKind? asKind = null)
        {
            var len = SyntaxParsers.MatchesText(_source, _pos, text);
            if (len > 0)
            {
                var token = SyntaxParsers.ProduceSyntaxToken(_source, _pos, len, text, asKind);
                _pos += len;
                return token;
            }

            return null;
        }

        /// <summary>
        /// Converts the next count adjacent tokens into a single token
        /// or returns null.
        /// </summary>
        private SyntaxToken ParseToken(int count, SyntaxKind? asKind = null)
        {
            var token = SyntaxParsers.ProduceSyntaxToken(_source, _pos, count, asKind);

            if (token != null)
            {
                _pos += count;
            }

            return token;
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

        /// <summary>
        /// Parses one or more adjacent lexical tokens that together matches the text into a single token,
        /// or returns a missing token.
        /// </summary>
        private SyntaxToken ParseRequiredToken(string text, SyntaxKind kind = SyntaxKind.IdentifierToken)
        {
            return ParseToken(text, kind) ?? SyntaxParsers.CreateMissingToken(text);
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

        private static readonly HashSet<SyntaxKind> s_extendedKeyordsAsIdentifiers =
            KustoFacts.ExtendedKeywordsAsIdentifiers.ToHashSetEx();

        private bool ScanExtendedKeywordAsIdentifier(int offset = 0)
        {
            var token = PeekToken(offset);
            return token.Kind.IsKeyword() && s_extendedKeyordsAsIdentifiers.Contains(token.Kind);
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

        private NameDeclaration CreateMissingNameDeclaration()
        {
            var dx = this.PeekToken() is LexicalToken token && token.Kind.IsKeyword()
                        ? DiagnosticFacts.GetMissingNameWithKeyword(token.Text)
                        : DiagnosticFacts.GetMissingName();
            return new NameDeclaration(SyntaxToken.Missing(SyntaxKind.IdentifierToken), new[] { dx });
        }

        private NameReference CreateMissingNameReference()
        {
            var dx = this.PeekToken() is LexicalToken token && token.Kind.IsKeyword()
                        ? DiagnosticFacts.GetMissingNameWithKeyword(token.Text)
                        : DiagnosticFacts.GetMissingName();

            return new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken), new[] { dx });
        }

        private Func<NameReference> _fnCreateMissingNameReference;
        private Func<NameReference> FnCreateMissingNameReference
        {
            get
            {
                if (_fnCreateMissingNameReference == null)
                    _fnCreateMissingNameReference = this.CreateMissingNameReference;
                return _fnCreateMissingNameReference;
            }
        }


        private Func<Expression> _fnCreateMissingNameReferenceAsExpression;
        private Func<Expression> FnCreateMissingNameReferenceAsExpression
        {
            get
            {
                if (_fnCreateMissingNameReferenceAsExpression == null)
                    _fnCreateMissingNameReferenceAsExpression = () => (Expression)this.CreateMissingNameReference();
                return _fnCreateMissingNameReferenceAsExpression;
            }
        }

        private static SyntaxToken CreateMissingNameToken(IReadOnlyList<string> texts) =>
            SyntaxToken.Missing(SyntaxKind.IdentifierToken, DiagnosticFacts.GetTokenExpected(texts));

        private static readonly Func<Expression> CreateMissingStringLiteral = () =>
            new LiteralExpression(
                SyntaxKind.StringLiteralExpression,
                SyntaxToken.Missing(SyntaxKind.StringLiteralToken),
                new[] { DiagnosticFacts.GetMissingString() });

        private static readonly Func<Expression> CreateMissingBoolLiteral = () =>
            new LiteralExpression(
                SyntaxKind.BooleanLiteralExpression,
                SyntaxToken.Missing(SyntaxKind.BooleanLiteralToken),
                new[] { DiagnosticFacts.GetMissingBoolean() });

        private static readonly Func<Expression> CreateMissingLongLiteral = () =>
            new LiteralExpression(SyntaxKind.LongLiteralExpression,
                SyntaxToken.Missing(SyntaxKind.LongLiteralToken),
                new[] { DiagnosticFacts.GetMissingNumber() });

        private static readonly Func<Expression> CreateMissingRealLiteral = () =>
            new LiteralExpression(SyntaxKind.RealLiteralExpression,
                SyntaxToken.Missing(SyntaxKind.RealLiteralToken),
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

        private Expression CreateMissingExpression()
        {
            // check to see if following token was a keyword and if so report enhanced diagnostic
            var dx = this.PeekToken() is LexicalToken token && token.Kind.IsKeyword()
                ? DiagnosticFacts.GetMissingExpressionWithKeyword(token.Text)
                : DiagnosticFacts.GetMissingExpression();

            return new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken), new[] { dx });
        }

        private static readonly Func<Name> CreateMissingIdentifierName = () =>
            new TokenName(SyntaxToken.Missing(SyntaxKind.IdentifierToken), new[] { DiagnosticFacts.GetMissingExpression() });

        private static readonly Func<SchemaTypeExpression> CreateMissingSchemaType = () =>
            new SchemaTypeExpression(
                SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                SyntaxList<SeparatedElement<Expression>>.Empty(),
                SyntaxToken.Missing(SyntaxKind.CloseParenToken),
                new[] { DiagnosticFacts.GetMissingSchemaDeclaration() });

        private static readonly Func<RowSchema> CreateMissingRowSchema = () =>
            new RowSchema(
                SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                null,
                SyntaxList<SeparatedElement<NameAndTypeDeclaration>>.Empty(),
                SyntaxToken.Missing(SyntaxKind.CloseParenToken),
                new[] { DiagnosticFacts.GetMissingSchemaDeclaration() });

        private static readonly Func<EvaluateRowSchema> CreateMissingEvaluateRowSchema = () =>
            new EvaluateRowSchema(
                SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                null,
                null,
                null,
                SyntaxList<SeparatedElement<NameAndTypeDeclaration>>.Empty(),
                SyntaxToken.Missing(SyntaxKind.CloseParenToken),
                new[] { DiagnosticFacts.GetMissingSchemaDeclaration() });

        private static readonly Func<NamedParameter> CreateMissingNamedParameter = () =>
            new NamedParameter(
                new NameDeclaration(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                SyntaxToken.Missing(SyntaxKind.EqualToken),
                new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                diagnostics: new[] { DiagnosticFacts.GetMissingParameter() });

        private static ExpressionList CreateMissingArgumentList() =>
            new ExpressionList(
                SyntaxToken.Missing(SyntaxKind.OpenParenToken, DiagnosticFacts.GetTokenExpected(SyntaxKind.OpenParenToken)),
                SyntaxList<SeparatedElement<Expression>>.Empty(),
                SyntaxToken.Missing(SyntaxKind.CloseParenToken, DiagnosticFacts.GetTokenExpected(SyntaxKind.CloseParenToken)));

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
                    else if (createMissingElement != null)
                    {
                        list.Add(new SeparatedElement<TElement>(createMissingElement(), null));
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

        private static readonly Func<QueryParser, bool> FnScanCommonListEnd =
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
                    return ParseBracedName();
                default:
                    return ParseIdentifierName();
            }
        }

        private Name ParseExtendedName()
        {
            switch (PeekToken().Kind)
            {
                case SyntaxKind.OpenBracketToken:
                    return ParseBracketedName();
                case SyntaxKind.OpenBraceToken:
                    return ParseBracedName();
                default:
                    return ParseIdentifierName()
                        ?? ParseExtendedKeyordAsIdentifierName();
            }
        }

        private SyntaxToken ParseKeywordOrIdentifier()
        {
            var tok = PeekToken();
            if (tok.Kind == SyntaxKind.IdentifierToken
                || (tok.Kind.IsKeyword() && tok.Kind.CanBeIdentifier()))
            {
                return ParseToken();
            }

            return null;
        }

        private Name ParseIdentifierName()
        {
            if (ParseKeywordOrIdentifier() is SyntaxToken token)
                return new TokenName(token);
            return null;
        }

        private Name ParseExtendedKeyordAsIdentifierName()
        {
            if (ScanExtendedKeywordAsIdentifier())
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

        private int ScanExtendedName(int offset = 0)
        {
            var result = ScanName(offset);
            if (result >= 0)
                return result;
            return ScanExtendedKeywordAsIdentifier(offset) ? 1 : -1;
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

            // must start with asterisk or a single name/keyword and then an asterisk
            if (token.Kind == SyntaxKind.AsteriskToken)
            {
                offset++;
            }
            else if ((token.Kind == SyntaxKind.IdentifierToken || ScanExtendedKeywordAsIdentifier(offset))
                && PeekToken(offset + 1) is LexicalToken nextToken
                && nextToken.Kind == SyntaxKind.AsteriskToken
                && (nextToken.Trivia.Length == 0 || _options.AllowNonAdjacentWildcardParts))
            {
                offset += 2;
            }
            else
            {
                return -1;
            }

            // then followed by zero or more additional identifiers, keywords or asterisks.
            while (
                ((token = PeekToken(offset)).Kind == SyntaxKind.IdentifierToken
                    || token.Kind == SyntaxKind.LongLiteralToken
                    || token.Kind == SyntaxKind.AsteriskToken
                    || ScanExtendedKeywordAsIdentifier(offset))
                && (token.Trivia.Length == 0 || _options.AllowNonAdjacentWildcardParts))
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
                var trivia = PeekToken().Trivia;
                var text = GetCombinedTokenText(0, len);
                var valueText = _options.AllowNonAdjacentWildcardParts
                    ? GetCombinedTokenText(0, len, includeInnerTrivia: false)
                    : text;
                var lit = SyntaxToken.Identifier(trivia, text, valueText);
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

        private string GetCombinedTokenText(int start = 0, int length = 1, bool includeInnerTrivia = true)
        {
            if (length == 1)
            {
                return PeekToken(start).Text;
            }

            var builder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                var token = PeekToken(start + i);
                if (i > 0 && includeInnerTrivia)
                    builder.Append(token.Trivia);
                builder.Append(token.Text);
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

        /// <summary>
        /// Includes bracketed names, identifiers and limited keywords-as-identifiers
        /// </summary>
        private NameReference ParseNameReference()
        {
            var name = ParseName();
            return name != null ? new NameReference(name) : null;
        }

        private static Func<QueryParser, Expression> FnParseNameReference =
            qp => qp.ParseNameReference();

        /// <summary>
        /// Includes bracketed names, identifiers and extended keywords-as-identifiers
        /// </summary>
        private NameReference ParseExtendedNameReference()
        {
            var name = ParseExtendedName();
            return name != null ? new NameReference(name) : null;
        }

        /// <summary>
        /// Includes bracketed names, identifiers and limited keywords-as-identifiers
        /// </summary>
        private NameDeclaration ParseNameDeclaration()
        {
            var name = ParseName();
            return name != null ? new NameDeclaration(name) : null;
        }

        /// <summary>
        /// Includes bracketed names, identifiers and extended keywords-as-identifiers
        /// </summary>
        private NameDeclaration ParseExtendedNameDeclaration()
        {
            var name = ParseExtendedName();
            return name != null ? new NameDeclaration(name) : null;
        }

        /// <summary>
        /// Scan a braced name (client parameter) and returns amount of tokens it consumes.
        /// </summary>
        private int ScanBracedName(int offset = 0)
        {
            var pos = offset;
            var token = PeekToken(pos);
            if (token.Kind != SyntaxKind.OpenBraceToken)
                return -1;
            pos++;

            token = PeekToken(pos);
            if ((token.Kind != SyntaxKind.IdentifierToken
                   && token.Kind.GetCategory() != SyntaxCategory.Keyword)
                || token.Trivia.Length > 0)
                return -1;
            pos++;

            token = PeekToken(pos);
            if (token.Kind == SyntaxKind.OpenBracketToken
                && token.Trivia.Length == 0)
            {
                pos++;

                token = PeekToken(pos);
                if (token.Kind == SyntaxKind.MinusToken
                    && token.Trivia.Length == 0)
                    pos++;

                token = PeekToken(pos);
                if (token.Kind != SyntaxKind.LongLiteralToken
                    || token.Trivia.Length > 0)
                    return -1;
                pos++;

                token = PeekToken(pos);
                if (token.Kind != SyntaxKind.CloseBracketToken
                    || token.Trivia.Length > 0)
                    return -1;
                pos++;
            }

            token = PeekToken(pos);
            if (token.Kind != SyntaxKind.CloseBraceToken
                || token.Trivia.Length > 0)
                return -1;
            pos++;

            return pos - offset;
        }

        private Name ParseBracedName()
        {
            var len = ScanBracedName();
            if (len > 2)
            {
                var open = SyntaxToken.From(PeekToken(0));
                var nameText = GetCombinedTokenText(1, len - 2);
                var close = SyntaxToken.From(PeekToken(len - 1));
                var nameToken = SyntaxToken.Identifier("", nameText, nameText);
                _pos += len;
                return new BracedName(open, nameToken, close);
            }
            return null;
        }

        private Expression ParseBracedNameReference()
        {
            var name = ParseBracedName();
            return name != null ? new NameReference(name) : null;
        }

        internal static bool IsKeywordInNamePosition(Source<LexicalToken> source, int start)
        {
            if (source.Peek(start) is LexicalToken token && token.Kind.IsKeyword()
                && source.Peek(start + 1) is LexicalToken nextToken)
            {
                // look for token following keyword that only happens after names in expressions
                // like infix binary operators, etc.
                switch (nextToken.Kind)
                {
                    // infix binary operators
                    case SyntaxKind.AndKeyword:
                    case SyntaxKind.OrKeyword:
                    case SyntaxKind.EqualEqualToken:
                    case SyntaxKind.BangEqualToken:
                    case SyntaxKind.EqualTildeToken:
                    case SyntaxKind.BangTildeToken:
                    case SyntaxKind.GreaterThanToken:
                    case SyntaxKind.GreaterThanOrEqualToken:
                    case SyntaxKind.LessThanToken:
                    case SyntaxKind.LessThanOrEqualToken:
                    case SyntaxKind.AsteriskToken:
                    case SyntaxKind.SlashToken:
                    case SyntaxKind.PercentToken:
                    case SyntaxKind.HasKeyword:
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
                    case SyntaxKind.InKeyword:
                    case SyntaxKind.InCsKeyword:
                    case SyntaxKind.NotInKeyword:
                    case SyntaxKind.NotInCsKeyword:
                    case SyntaxKind.HasAnyKeyword:
                    case SyntaxKind.HasAllKeyword:
                    case SyntaxKind.BetweenKeyword:
                    case SyntaxKind.NotBetweenKeyword:

                    // these could be prefix unary starting an expression after a keyword starting a clause
                    //case SyntaxKind.MinusToken:  
                    //case SyntaxKind.PlusToken:

                    // tokens that would only occur at after names in expressions
                    case SyntaxKind.CloseParenToken:
                    case SyntaxKind.CloseBracketToken:
                    case SyntaxKind.CloseBraceToken:
                    case SyntaxKind.DotToken:
                    case SyntaxKind.CommaToken:

                    // not really related to expressions but do indicate preceeding keyword was likely meant as a name
                    case SyntaxKind.ColonToken:
                    case SyntaxKind.BarToken:
                        return true;

                    // this could be start of parenthesized expression after a keyword starting a clause
                    case SyntaxKind.OpenParenToken:

                    // this could be start of bracketted name after keyword
                    case SyntaxKind.OpenBracketToken:
                        return false;
                }
            }

            return false;
        }

        private bool IsKeywordInNamePosition()
        {
            return IsKeywordInNamePosition(_source, _pos);
        }

        private NameReference ParseInvalidKeywordAsNameReference()
        {
            if (IsKeywordInNamePosition())
            {
                var token = ParseToken();
                return new NameReference(new TokenName(token), new[] { DiagnosticFacts.GetNameRequiresBrackets(token.Text) });
            }

            return null;
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

        private LiteralExpression ParseStringLiteral()
        {
            if (PeekToken().Kind == SyntaxKind.StringLiteralToken)
            {
                return new LiteralExpression(SyntaxKind.StringLiteralExpression, ParseToken());
            }

            return null;
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
                case SyntaxKind.RawGuidLiteralToken:
                    return new LiteralExpression(SyntaxKind.GuidLiteralExpression, ParseToken(), new[] { DiagnosticFacts.GetRawGuidLiteralNotAllowed() });
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
                    return ParseBracedNameReference();
            }

            return null;
        }

        private static readonly Func<QueryParser, Expression> FnParseExpression =
            qp => qp.ParseExpression();

        private static readonly Func<QueryParser, Expression> FnParseLiteral =
            qp => qp.ParseLiteral();

        private int ScanSignedNumericLiteral(int offset = 0)
        {
            var sign = PeekToken(offset);
            var number = PeekToken(offset + 1);
            if ((sign.Kind == SyntaxKind.PlusToken || sign.Kind == SyntaxKind.MinusToken)
                && number.Trivia.Length == 0
                && (number.Kind == SyntaxKind.LongLiteralToken
                || number.Kind == SyntaxKind.RealLiteralToken
                || number.Kind == SyntaxKind.TimespanLiteralToken)
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
                    case SyntaxKind.TimespanLiteralToken:
                        return new LiteralExpression(SyntaxKind.TimespanLiteralExpression, signedNumberToken);
                }
            }

            return null;
        }

#if false
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
#endif

        private int ScanForcedRealLiteral(int offset = 0)
        {
            var number = PeekToken(offset);
            if ((number.Kind == SyntaxKind.LongLiteralToken
                || number.Kind == SyntaxKind.RealLiteralToken)
                && number.Text.Length > 0 && char.IsDigit(number.Text[0]))
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        private Expression ParseForcedRealLiteral()
        {
            var token = PeekToken();
            switch (token.Kind)
            {
                case SyntaxKind.LongLiteralToken:
                case SyntaxKind.RealLiteralToken:
                    if (token.Text.Length > 0 && char.IsDigit(token.Text[0]))
                        return new LiteralExpression(SyntaxKind.RealLiteralExpression, ParseToken());
                    break;
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return ParseSignedForcedRealLiteral();
                case SyntaxKind.OpenBraceToken:
                    return ParseBracedNameReference();
            }

            return null;
        }

        private Expression ParseSignedForcedRealLiteral()
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
                    case SyntaxKind.RealLiteralToken:
                        return new LiteralExpression(SyntaxKind.RealLiteralExpression, signedNumberToken);
                }
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

        private Expression ParseJsonValue() =>
            StackSafeParse(
                q => q.ParseJsonValue_Unsafe(),
                g => g.JsonValue
                );

        private Expression ParseJsonValue_Unsafe()
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
                case SyntaxKind.RawGuidLiteralToken:
                case SyntaxKind.DecimalLiteralToken:
                case SyntaxKind.StringLiteralToken:
                case SyntaxKind.DynamicKeyword:
                    return ParseLiteral();
                case SyntaxKind.NullKeyword:
                    return ParseNullLiteral();
                case SyntaxKind.OpenBracketToken:
                    return ParseJsonArray();
                case SyntaxKind.OpenBraceToken:
                    if (ScanBracedName() > 0)
                    {
                        return ParseBracedNameReference();
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

        private static readonly Func<QueryParser, Expression> FnParseJsonValue =
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

        private static readonly Func<QueryParser, JsonPair> FnParseJsonPair =
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

        private bool ScanTypeOfLiteral(int offset = 0)
        {
            return ScanTypeOfScalar(offset)
                || (PeekToken(offset).Kind == SyntaxKind.TypeOfKeyword && PeekToken(offset + 1).Kind == SyntaxKind.OpenParenToken);
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
                else if (PeekToken(1).Kind == SyntaxKind.OpenParenToken)
                {
                    var keyword = ParseToken();
                    var open = ParseToken();
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

        private static readonly Func<QueryParser, Expression> FnParseTypeOfElement =
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

        private TypeExpression ParseInvalidParamType()
        {
            if (PeekToken() is LexicalToken token
                && token.Kind is SyntaxKind kind
                && (ScanParamTypeExtended()
                    || kind == SyntaxKind.IdentifierToken
                    || (kind.IsKeyword() && kind.CanBeIdentifier())))
            {
                return new PrimitiveTypeExpression(ParseToken(), new[] { DiagnosticFacts.GetInvalidTypeName(token.Text) });
            }

            return null;
        }

        private NameAndTypeDeclaration ParseNameAndTypeDeclaration()
        {
            var name = ParseExtendedNameDeclaration();
            if (name != null)
            {
                var colon = ParseRequiredToken(SyntaxKind.ColonToken);

                var type = PeekToken().Kind == SyntaxKind.OpenParenToken
                    ? ParseSchemaType()
                    : ParseParamType() ?? ParseInvalidParamType() ?? CreateMissingType();

                return new NameAndTypeDeclaration(name, colon, type);
            }
            else if (PeekToken().Kind == SyntaxKind.ColonToken)
            {
                // name is missing
                var colon = ParseToken();
                var type = ParseParamType() ?? ParseInvalidParamType() ?? CreateMissingType();
                return new NameAndTypeDeclaration(CreateMissingNameDeclaration(), colon, type);
            }

            return null;
        }

        private Expression ParseNameAndOptionalTypeDeclaration()
        {
            if (PeekToken().Kind == SyntaxKind.ColonToken)
            {
                var colon = ParseToken();
                var type = ParseParamType() ?? ParseInvalidParamType() ?? CreateMissingType();
                return new NameAndTypeDeclaration(CreateMissingNameDeclaration(), colon, type);
            }

            var name = ParseExtendedNameDeclaration();
            if (name != null && PeekToken().Kind == SyntaxKind.ColonToken)
            {
                var colon = ParseToken();
                var type = ParseParamType() ?? ParseInvalidParamType() ?? CreateMissingType();
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

        private static readonly Func<QueryParser, NameAndTypeDeclaration> FnParseNameAndTypeDeclaration =
            qp => qp.ParseNameAndTypeDeclaration();

        private static readonly Func<QueryParser, Expression> FnParseNameAndTypeDeclarationExpression =
            qp => (Expression)qp.ParseNameAndTypeDeclaration();

        /// <summary>
        /// Parses a multi-part schema declaration:  (name: type, name: type, ...)
        /// </summary>
        private SchemaTypeExpression ParseSchemaMultipartType()
        {
            if (PeekToken().Kind == SyntaxKind.OpenParenToken)
            {
                var open = ParseToken();
                var list = ParseCommaList(FnParseNameAndTypeDeclarationExpression, CreateMissingNameAndTypeDeclarationExpression, FnScanCommonListEnd, allowTrailingComma: true);
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new SchemaTypeExpression(open, list, close);
            }

            return null;
        }

        /// <summary>
        /// Parses a tabular row schema
        /// </summary>
        private RowSchema ParseRowSchema(bool oneOrMore = false)
        {
            if (PeekToken().Kind == SyntaxKind.OpenParenToken)
            {
                var open = ParseToken();
                var leadingComma = ParseToken(SyntaxKind.CommaToken);
                var list = ParseCommaList(FnParseNameAndTypeDeclaration, CreateMissingNameAndTypeDeclaration, FnScanCommonListEnd, oneOrMore: oneOrMore, allowTrailingComma: true);
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new RowSchema(open, leadingComma, list, close);
            }

            return null;
        }

        private EvaluateRowSchema ParseEvaluateRowSchema()
        {
            if (PeekToken().Kind == SyntaxKind.OpenParenToken)
            {
                var open = ParseToken();
                var leadingComma = ParseToken(SyntaxKind.CommaToken);
                var asteriskToken = ParseToken(SyntaxKind.AsteriskToken);
                var asteriskTokenComma = ParseToken(SyntaxKind.CommaToken);
                var list = ParseCommaList(FnParseNameAndTypeDeclaration, CreateMissingNameAndTypeDeclaration, FnScanCommonListEnd, allowTrailingComma: true);
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new EvaluateRowSchema(open, leadingComma, asteriskToken, asteriskTokenComma, list, close);
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
                case SyntaxKind.RawGuidLiteralToken:
                case SyntaxKind.StringLiteralToken:
                case SyntaxKind.DynamicKeyword:
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
                    if (ScanTypeOfLiteral()) // typeof can be an identifier so need to scan further than just the typeof keyword
                        return ParseTypeOfLiteral();
                    if (ScanFunctionCallStart())
                        return ParseFunctionCallExpression();
                    return ParsePrimaryPathSelector()
                        ?? ParseInvalidKeywordAsNameReference();
            }
        }

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_dataTableParameters =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.DataTableParameters);

        private DataTableExpression ParseDataTableExpression()
        {
            var keyword = ParseToken(SyntaxKind.DataTableKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_dataTableParameters);
                var schema = ParseRowSchema() ?? CreateMissingRowSchema();
                var open = ParseRequiredToken(SyntaxKind.OpenBracketToken);
                var leadingComma = ParseToken(SyntaxKind.CommaToken);
                var values = ParseCommaList(FnParseLiteral, CreateMissingValue, FnScanCommonListEnd, allowTrailingComma: true);
                var close = ParseRequiredToken(SyntaxKind.CloseBracketToken);
                return new DataTableExpression(keyword, parameters, schema, open, leadingComma, values, close);
            }

            return null;
        }

        private ContextualDataTableExpression ParseContextualDataTableExpression()
        {
            var keyword = ParseToken(SyntaxKind.ContextualDataTableKeyword);
            if (keyword != null)
            {
                var id = ParseLiteral() ?? ParseUnnamedExpression() ?? CreateMissingExpression();
                var schema = ParseRowSchema() ?? CreateMissingRowSchema();
                return new ContextualDataTableExpression(keyword, id, schema);
            }

            return null;
        }

        private ExternalDataExpression ParseExternalDataExpression()
        {
            switch (PeekToken().Kind)
            {
                case SyntaxKind.ExternalDataKeyword:
                case SyntaxKind.External_DataKeyword:
                    var keyword = ParseToken();
                    var parameters = ParseQueryOperatorParameterList(s_dataTableParameters);
                    var schema = ParseRowSchema() ?? CreateMissingRowSchema();
                    var open = ParseRequiredToken(SyntaxKind.OpenBracketToken);
                    var values = ParseCommaList(FnParseExpression, CreateMissingValue, FnScanCommonListEnd, allowTrailingComma: true);
                    var close = ParseRequiredToken(SyntaxKind.CloseBracketToken);
                    var clause = ParseExternalDataWithClause();
                    return new ExternalDataExpression(keyword, parameters, schema, open, values, close, clause);

                default:
                    return null;
            }
        }

        private Expression ParseExternalDataPropertyValue()
        {
            switch (PeekToken().Kind)
            {
                case SyntaxKind.StringLiteralToken:
                case SyntaxKind.LongLiteralToken:
                case SyntaxKind.RealLiteralToken:
                case SyntaxKind.BooleanLiteralToken:
                case SyntaxKind.DateTimeLiteralToken:
                case SyntaxKind.TypeOfLiteralExpression:
                    return ParseLiteral();
                case SyntaxKind.RawGuidLiteralToken:
                case SyntaxKind.GuidLiteralToken:
                    return new LiteralExpression(SyntaxKind.GuidLiteralExpression, ParseToken());
                default:
                    return ParseRenameNameDeclaration();
            }
        }

        private NamedParameter ParseExternalDataProperty()
        {
            var name = ParseRenameNameDeclaration();
            if (name != null)
            {
                var equal = ParseRequiredToken(SyntaxKind.EqualToken);
                var value = ParseExternalDataPropertyValue() ?? CreateMissingValue();
                return new NamedParameter(name, equal, value);
            }

            return null;
        }

        private static readonly Func<QueryParser, NamedParameter> FnParseExternalDataProperty =
            qp => qp.ParseExternalDataProperty();

        private ExternalDataWithClause ParseExternalDataWithClause()
        {
            var keyword = ParseToken(SyntaxKind.WithKeyword);
            if (keyword != null)
            {
                var open = ParseRequiredToken(SyntaxKind.OpenParenToken);
                var properties = ParseCommaList(FnParseExternalDataProperty, CreateMissingNamedParameter, FnScanCommonListEnd);
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new ExternalDataWithClause(keyword, open, properties, close);
            }

            return null;
        }

        // Inline External Table handling

        private bool ScanExternalTableSchema(int offset = 0)
        {
            if (PeekToken(offset).Kind == SyntaxKind.OpenParenToken)
            {
                var index = offset + 1;
                while (true)
                {
                    var token = PeekToken(index);
                    if (token == NoToken || token.Kind == SyntaxKind.ColonToken)
                    {
                        return true;
                    }
                    else if (token.Kind == SyntaxKind.CloseParenToken)
                    {
                        break;
                    }
                    index++;
                }
            }
            return false;
        }

        private InlineExternalTableExpression ParseInlineExternalTableExpression()
        {
            // Support usage of inline_external_table as identifier
            if (PeekToken().Kind == SyntaxKind.InlineExternalTableKeyword && ScanExternalTableSchema(1))
            {
                var keyword = ParseToken();
                var parameters = ParseQueryOperatorParameterList(s_dataTableParameters);
                var schema = ParseRowSchema(true) ?? CreateMissingRowSchema();
                var kind = ParseInlineExternalTableKindClause();
                var partitionClause = ParseInlineExternalTablePartitionClause();
                var pathFormatClause = ParseInlineExternalTablePathFormatClause();
                var dataFormat = ParseInlineExternalDataFormatClause();
                var connectionStrings = ParseInlineExternalTableConnectionStringsClause();
                var withClause = ParseExternalDataWithClause();
                return new InlineExternalTableExpression(keyword, parameters, schema, kind, partitionClause, pathFormatClause, dataFormat, connectionStrings, withClause);
            }
            return null;
        }

        private InlineExternalTablePartitionClause ParseInlineExternalTablePartitionClause()
        {
            if (PeekToken().Kind == SyntaxKind.PartitionKeyword)
            {
                var partition = ParseRequiredToken(SyntaxKind.PartitionKeyword);
                var by = ParseRequiredToken(SyntaxKind.ByKeyword);
                var open = ParseRequiredToken(SyntaxKind.OpenParenToken);
                var optionalComma = ParseToken(SyntaxKind.CommaToken);
                var partitionColumns = ParseCommaList(FnParsePartitionColumnDeclaration, FnCreateMissingPartitionColumnDeclaration, fnScanEnd: FnScanCommonListEnd, allowTrailingComma: true);
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new InlineExternalTablePartitionClause(partition, by, open, optionalComma, partitionColumns, close);
            }
            return null;
        }

        private static readonly Func<PartitionColumnDeclaration> FnCreateMissingPartitionColumnDeclaration =
            () => new PartitionColumnDeclaration(
                        new NameDeclaration(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                        SyntaxToken.Missing(SyntaxKind.ColonToken),
                        new PrimitiveTypeExpression(SyntaxToken.Missing(SyntaxKind.StringKeyword)),
                        SyntaxToken.Missing(SyntaxKind.EqualToken),
                        null,
                        new[] { DiagnosticFacts.GetMissingPartitionColumnDeclaration() });

        private static readonly Func<QueryParser, PartitionColumnDeclaration> FnParsePartitionColumnDeclaration =
            qp => qp.ParsePartitionColumnDeclaration();

        private PartitionColumnDeclaration ParsePartitionColumnDeclaration()
        {
            var name = ParseExtendedNameDeclaration();

            var colon = ParseRequiredToken(SyntaxKind.ColonToken);

            var type = ParsePartitionColumnType() ?? CreateMissingType();

            var equal = ParseToken(SyntaxKind.EqualToken);
            var expression = equal != null ? ParseUnnamedExpression() : null;
            return new PartitionColumnDeclaration(name, colon, type, equal, expression);
        }

        private TypeExpression ParsePartitionColumnType()
        {
            switch (PeekToken().Kind)
            {
                case SyntaxKind.DateTimeKeyword:
                case SyntaxKind.LongKeyword:
                case SyntaxKind.StringKeyword:
                    return new PrimitiveTypeExpression(ParseToken());
            }
            return new PrimitiveTypeExpression(ParseToken(), new[] { DiagnosticFacts.GetWrongPartitionColumnType ()} );
        }

        private InlineExternalTablePathFormatClause ParseInlineExternalTablePathFormatClause()
        {
            if (PeekToken().Kind == SyntaxKind.PathFormatKeyword)
            {
                var pathFormat = ParseRequiredToken(SyntaxKind.PathFormatKeyword);
                var equal = ParseRequiredToken(SyntaxKind.EqualToken);
                var open = ParseRequiredToken(SyntaxKind.OpenParenToken);
                var optionalSeparator = ParseStringLiteral();
                var pathFormatElements = ParseList(FnParseExternalTablePathFormatToken, FnCreateMissingExternalTablePathFormatToken, fnScanEnd: FnScanCommonListEnd, oneOrMore: true);
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new InlineExternalTablePathFormatClause(pathFormat, equal, open, optionalSeparator, pathFormatElements, close);
            }
            return null;
        }

        private static readonly Func<InlineExternalTablePathFormatPartitionColumnReference> FnCreateMissingExternalTablePathFormatToken =
            () => new InlineExternalTablePathFormatPartitionColumnReference(
                CreateMissingValue(),
                (LiteralExpression)CreateMissingStringLiteral(),
                new[] { DiagnosticFacts.GetMissingPathFormatTokens() });

        private static readonly Func<QueryParser, InlineExternalTablePathFormatPartitionColumnReference> FnParseExternalTablePathFormatToken =
            qp => qp.ParseExternalTablePathFormatToken();

        private InlineExternalTablePathFormatPartitionColumnReference ParseExternalTablePathFormatToken()
        {
            var partitionColumnReference = PeekToken().Kind == SyntaxKind.DateTimePatternKeyword
                ? new DateTimePattern(
                    ParseRequiredToken(SyntaxKind.DateTimePatternKeyword), // date_time_pattern token
                    ParseRequiredToken(SyntaxKind.OpenParenToken), // (
                    ParseStringLiteral(), // literal containing the date time pattern
                    ParseRequiredToken(SyntaxKind.CommaToken), // ,
                    ParseNameReference(), // partition column name
                    ParseRequiredToken(SyntaxKind.CloseParenToken)) // )
                : (Expression)ParseNameReference();

            if (partitionColumnReference == null)
            {
                //TODO: Is there better way to make token as skipped, need this to avoid endless loop in ParseList
                ParseToken();
            }
            return partitionColumnReference == null
                ? new InlineExternalTablePathFormatPartitionColumnReference(
                    CreateMissingValue(),
                    (LiteralExpression)CreateMissingStringLiteral(),
                    new[] { DiagnosticFacts.GetUnknownTokenInPathFormatDefinition() })
                : new InlineExternalTablePathFormatPartitionColumnReference(
                    partitionColumnReference,
                    ParseStringLiteral());
        }


        private InlineExternalTableConnectionStringsClause ParseInlineExternalTableConnectionStringsClause()
        {
            if (PeekToken().Kind == SyntaxKind.OpenParenToken)
            {
                var open = ParseRequiredToken(SyntaxKind.OpenParenToken);
                var values = ParseCommaList(FnParseExpression, CreateMissingValue, FnScanCommonListEnd, allowTrailingComma: true);
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new InlineExternalTableConnectionStringsClause(open, values, close);
            }
            return new InlineExternalTableConnectionStringsClause(
                SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                SyntaxList<SeparatedElement<Expression>>.Empty(),
                SyntaxToken.Missing(SyntaxKind.CloseParenToken),
                new[] { DiagnosticFacts.GetMissingConnectionStrings() });
        }

        private InlineExternalTableKindClause ParseInlineExternalTableKindClause()
        {
            if (PeekToken().Kind == SyntaxKind.KindKeyword)
            {
                var keyword = ParseToken();
                var equal = ParseRequiredToken(SyntaxKind.EqualToken);
                var value = ParseRequiredToken(KustoFacts.InlineExternalTableKinds);
                return new InlineExternalTableKindClause(keyword, equal, value);
            }

            return new InlineExternalTableKindClause(
                SyntaxToken.Missing(SyntaxKind.KindKeyword),
                SyntaxToken.Missing(SyntaxKind.EqualToken),
                SyntaxToken.Missing(SyntaxKind.StringLiteralToken),
                new[] { DiagnosticFacts.GetMissingExternalTableKind() });
        }

        private InlineExternalTableDataFormatClause ParseInlineExternalDataFormatClause()
        {
            if (PeekToken().Kind == SyntaxKind.DataFormatKeyword)
            {
                var keyword = ParseToken();
                var equal = ParseRequiredToken(SyntaxKind.EqualToken);
                var value = ParseRequiredToken(SyntaxKind.IdentifierToken);
                return new InlineExternalTableDataFormatClause(keyword, equal, value);
            }

            return new InlineExternalTableDataFormatClause(
                SyntaxToken.Missing(SyntaxKind.DataFormatKeyword),
                SyntaxToken.Missing(SyntaxKind.EqualToken),
                SyntaxToken.Missing(SyntaxKind.StringLiteralToken),
                new[] { DiagnosticFacts.GetMissingDataFormat() });
        }


        // End of Inline External Table handling

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
                if (keywordName == "aggregations")
                {
                    return new MaterializedViewCombineClause(
                        keyword,
                        ParseRequiredToken(SyntaxKind.OpenParenToken),
                        ParseSummarizeOperator() ?? CreateMissingExpression(),
                        ParseRequiredToken(SyntaxKind.CloseParenToken));
                }
                else
                {
                    return new MaterializedViewCombineClause(
                        keyword,
                        ParseRequiredToken(SyntaxKind.OpenParenToken),
                        ParseExpression() ?? CreateMissingExpression(),
                        ParseRequiredToken(SyntaxKind.CloseParenToken));
                }
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
            var selector = ParsePathElementSelector();
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

        private Expression ParsePathElementSelectorOrFunctionCall()
        {
            if (ScanFunctionCallStart())
            {
                return ParseFunctionCallExpression();
            }
            else
            {
                return ParsePathElementSelector();
            }
        }

        private Expression ParseBarePathElementSelector()
        {
            var token = PeekToken();
            if (token.Kind == SyntaxKind.AtToken)
            {
                return new AtExpression(ParseToken());
            }
            else if (IsSpecialKeywordAsIdentifier(token))
            {
                return new NameReference(new TokenName(ParseToken()));
            }
            else
            {
                return ParseNameReference();
            }
        }

        private static Func<QueryParser, Expression> FnParseBarePathElementSelector =
            qp => qp.ParseBarePathElementSelector();

        private static readonly HashSet<SyntaxKind> _specialKeywordsAsIdentifiers =
            KustoFacts.SpecialKeywordsAsIdentifiers.ToHashSetEx();

        private static bool IsSpecialKeywordAsIdentifier(LexicalToken token) =>
            token.Kind.IsKeyword() && _specialKeywordsAsIdentifiers.Contains(token.Kind);

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

        private int ScanRenameName(int offset = 0)
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
                    return ScanExtendedKeywordAsIdentifier(offset) ? 1 : -1;
            }
        }

        private int ScanRenameList(int offset = 0)
        {
            if (PeekToken(offset).Kind == SyntaxKind.OpenParenToken)
            {
                int start = offset;
                offset++;

                while (!ScanCommonListEnd(offset))
                {
                    var len = ScanRenameName(offset);
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

        private Name ParseRenameName()
        {
            switch (PeekToken().Kind)
            {
                case SyntaxKind.OpenBracketToken:
                    return ParseBracketedName();
                case SyntaxKind.OpenBraceToken:
                    return ParseBracedName();
                default:
                    return ParseIdentifierName()
                        ?? ParseExtendedKeyordAsIdentifierName();
            }
        }

        private NameDeclaration ParseRenameNameDeclaration()
        {
            var name = ParseRenameName();
            return name != null ? new NameDeclaration(name) : null;
        }

        private static readonly Func<QueryParser, NameDeclaration> FnParseRenameNameDeclaration =
            qp => qp.ParseRenameNameDeclaration();

        internal static int ScanDashedName(Source<LexicalToken> source, int start)
        {
            int position = start;

            var token = source.Peek(position);
            if (token != null
                && (token.Kind == SyntaxKind.IdentifierToken
                    || token.Kind.IsKeyword()))
            {
                position++;

                while (true)
                {
                    token = source.Peek(position);

                    if (token == null
                        || token.Trivia.Length > 0
                        || (token.Kind != SyntaxKind.IdentifierToken
                            && !token.Kind.IsKeyword()
                            && token.Kind != SyntaxKind.MinusToken))
                    {
                        break;
                    }

                    position++;
                }
            }

            return position - start;
        }

        private int ScanDashedName() =>
            ScanDashedName(_source, _pos);

        private TokenName ParseDashedName()
        {
            var len = ScanDashedName();

            if (len > 0 && ParseToken(len) is SyntaxToken token)
            {
                return new TokenName(token);
            }

            return null;
        }

        private Expression ParseNamedExpression()
        {
            if (ScanRenameName() is int nameLen
                && nameLen > 0
                && PeekToken(nameLen).Kind == SyntaxKind.EqualToken)
            {
                var name = ParseRenameNameDeclaration();
                var equal = ParseToken(SyntaxKind.EqualToken);
                var expr = ParseUnnamedExpression() ?? CreateMissingExpression();
                return new SimpleNamedExpression(name, equal, expr);
            }
            else if (ScanDashedName() is int dashNameLen
                && dashNameLen > 0
                && PeekToken(dashNameLen).Kind == SyntaxKind.EqualToken)
            {
                // special case of illegal name being used as named-expression name.
                var name = ParseDashedName();
                var nameDecl = new NameDeclaration(name, new[] { DiagnosticFacts.GetNameRequiresBrackets(name.Name.Text) });
                var equal = ParseToken(SyntaxKind.EqualToken);
                var expr = ParseUnnamedExpression() ?? CreateMissingExpression();
                return new SimpleNamedExpression(nameDecl, equal, expr);
            }
            else if (ScanRenameList() is int nameListLen
                && nameListLen > 0
                && PeekToken(nameListLen).Kind == SyntaxKind.EqualToken)
            {
                var open = ParseToken();
                var list = ParseCommaList(FnParseRenameNameDeclaration, CreateMissingNameDeclaration, FnScanCommonListEnd, oneOrMore: true);
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

        private static readonly Func<QueryParser, Expression> FnParseNamedExpression =
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

        private static readonly Func<QueryParser, Expression> FnParseArgument =
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

        private static readonly IReadOnlyList<string> s_functionsWithKeywordNames =
            Functions.All.Select(f => f.Name).Concat(
            Aggregates.All.Select(f => f.Name))
            .Where(n => (SyntaxFacts.IsKeyword(n) && !SyntaxFacts.IsKeywordThatCanBeIdentifier(n))
                     || IsMultiTokenName(n))
            .ToList();

        private int ScanFunctionCallName(int offset = 0)
        {
            var len = ScanName(offset);
            if (len > 0)
                return len;

            for (int i = 0; i < s_functionsWithKeywordNames.Count; i++)
            {
                len = ScanToken(s_functionsWithKeywordNames[i], offset);
                if (len > 0)
                    return len;
            }

            return -1;
        }

        private bool ScanFunctionCallStart(int offset = 0)
        {
            var len = ScanFunctionCallName(offset);
            if (len > 0 && PeekToken(offset + len).Kind == SyntaxKind.OpenParenToken)
                return true;

            return false;
        }

        private NameReference ParseFunctionCallName()
        {
            var len = ScanName();
            if (len > 0)
            {
                return ParseNameReference();
            }

            // special case for known functions with names that are keywords that
            // cannot normally be used as identifiers
            for (int i = 0; i < s_functionsWithKeywordNames.Count; i++)
            {
                var token = ParseToken(s_functionsWithKeywordNames[i]);
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

        private FunctionCallExpression ParseRequiredFunctionCallExpression()
        {
            if (ScanFunctionCallStart())
            {
                var name = ParseFunctionCallName();
                var arguments = ParseArgumentList();
                return new FunctionCallExpression(name, arguments);
            }
            else if (ScanFunctionCallName() > 0)
            {
                var name = ParseFunctionCallName();
                var arguments = CreateMissingArgumentList();
                return new FunctionCallExpression(name, arguments);
            }
            else
            {
                return CreateMissingFunctionCallExpression();
            }
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
                    var dot = ParseToken();
                    var selector = ParsePathElementSelectorOrFunctionCall() ?? CreateMissingNameReference();
                    expr = new PathExpression(expr, dot, selector);
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
            if (expr != null
                && GetStringOperationKind(PeekToken().Kind) is SyntaxKind opKind && opKind != SyntaxKind.None)
            {
                expr = new BinaryExpression(opKind, expr, ParseToken(), ParseUnaryPlusOrMinusExpression() ?? CreateMissingExpression());
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

            if (expr != null
                && GetRelationalExpressionKind(PeekToken().Kind) is SyntaxKind opKind
                && opKind != SyntaxKind.None)
            {
                expr = new BinaryExpression(opKind, expr, ParseToken(), ParseAdditiveExpression() ?? CreateMissingExpression());
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
                switch (PeekToken().Kind)
                {
                    case SyntaxKind.EqualEqualToken:
                        expr = new BinaryExpression(SyntaxKind.EqualExpression, expr, ParseToken(), ParseRelationalExpresion() ?? CreateMissingExpression());
                        break;
                    case SyntaxKind.BangEqualToken:
                        expr = new BinaryExpression(SyntaxKind.NotEqualExpression, expr, ParseToken(), ParseRelationalExpresion() ?? CreateMissingTokenLiteral());
                        break;
                    case SyntaxKind.LessThanGreaterThanToken:
                        expr = new BinaryExpression(SyntaxKind.NotEqualExpression, expr, ParseToken(), ParseRelationalExpresion() ?? CreateMissingTokenLiteral());
                        break;
                    case SyntaxKind.InKeyword:
                        if (PeekToken(1).Kind != SyntaxKind.RangeKeyword)
                            expr = new InExpression(SyntaxKind.InExpression, expr, ParseToken(), ParseRequiredInOperatorExpressionList());
                        break;
                    case SyntaxKind.InCsKeyword:
                        expr = new InExpression(SyntaxKind.InCsExpression, expr, ParseToken(), ParseRequiredInOperatorExpressionList());
                        break;
                    case SyntaxKind.NotInKeyword:
                        expr = new InExpression(SyntaxKind.NotInExpression, expr, ParseToken(), ParseRequiredInOperatorExpressionList());
                        break;
                    case SyntaxKind.NotInCsKeyword:
                        expr = new InExpression(SyntaxKind.NotInCsExpression, expr, ParseToken(), ParseRequiredInOperatorExpressionList());
                        break;
                    case SyntaxKind.HasAnyKeyword:
                        expr = new HasAnyExpression(SyntaxKind.HasAnyExpression, expr, ParseToken(), ParseRequiredInOperatorExpressionList());
                        break;
                    case SyntaxKind.HasAllKeyword:
                        expr = new HasAllExpression(SyntaxKind.HasAllExpression, expr, ParseToken(), ParseRequiredInOperatorExpressionList());
                        break;
                    case SyntaxKind.BetweenKeyword:
                        expr = new BetweenExpression(SyntaxKind.BetweenExpression, expr, ParseToken(), ParseRequiredExpressionCouple());
                        break;
                    case SyntaxKind.NotBetweenKeyword:
                        expr = new BetweenExpression(SyntaxKind.NotBetweenExpression, expr, ParseToken(), ParseRequiredExpressionCouple());
                        break;
                }
            }

            return expr;
        }

        internal bool ScanIsQueryExpression()
        {
            // if it starts with a query operator, then its a query expression.
            if (ScanPossibleQueryOperator())
                return true;

            // if is there a pipe/bar before we get to end of the expression then it is a query expression.
            if (ScanIsPipeBeforeEndOfExpression())
                return true;

            return false;
        }

        private bool ScanIsPipeBeforeEndOfExpression()
        {
            int offset = 0;
            int parenDepth = 0;

            while (true)
            {
                var token = PeekToken(offset);
                switch (token.Kind)
                {
                    case SyntaxKind.None:
                    case SyntaxKind.SemicolonToken:
                        // end of expression
                        return false;
                    case SyntaxKind.CommaToken:
                        // comma ends expression 
                        if (parenDepth == 0)
                            return false;
                        break;
                    case SyntaxKind.CloseParenToken:
                        // close paren can end expression
                        if (parenDepth == 0)
                            return false;
                        parenDepth--;
                        break;
                    case SyntaxKind.OpenParenToken:
                        parenDepth++;
                        break;
                    case SyntaxKind.BarToken:
                        // looks like pipe expression starts here
                        if (parenDepth == 0)
                            return true;
                        break;
                }

                offset++;
            }
        }

        private ExpressionList ParseRequiredInOperatorExpressionList()
        {
            var openParen = ParseRequiredToken(SyntaxKind.OpenParenToken);
            SyntaxList<SeparatedElement<Expression>> exprList;

            if (ScanIsQueryExpression())
            {
                // if query, then list is one query expression
                var query = ParseExpression() ?? CreateMissingExpression();
                exprList = new SyntaxList<SeparatedElement<Expression>>(
                    new SeparatedElement<Expression>(query));
            }
            else
            {
                // expression list of unnamed expressions (not queries)
                exprList = ParseCommaList(FnParseUnnamedExpression, CreateMissingExpression, FnScanCommonListEnd, oneOrMore: true);
            }

            var closeParen = ParseRequiredToken(SyntaxKind.CloseParenToken);

            return new ExpressionList(openParen, exprList, closeParen);
        }

        private ExpressionCouple ParseRequiredExpressionCouple()
        {
            return new ExpressionCouple(
                ParseRequiredToken(SyntaxKind.OpenParenToken),
                ParseUnnamedExpression() ?? CreateMissingExpression(),
                ParseRequiredToken(SyntaxKind.DotDotToken),
                ParseUnnamedExpression() ?? CreateMissingExpression(),
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

        private Expression ParseUnnamedExpression() =>
            StackSafeParse(
                q => q.ParseUnnamedExpression_Unsafe(),
                g => g.UnnamedExpression
                );

        private Expression ParseUnnamedExpression_Unsafe()
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
                        case SyntaxKind.RawGuidLiteralToken:
                        case SyntaxKind.IntLiteralToken:
                            return ParsePrimaryExpression();
                    }
                    break;
            }

            return ParseLogicalOrExpression();
        }

        private static readonly Func<QueryParser, Expression> FnParseUnnamedExpression =
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

        private Expression ParseAnyQueryOperatorParameterForcedRealValue()
        {
            var expr = ParseForcedRealLiteral();

            if (expr == null)
                expr = ParseLiteral();

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
                case QueryOperatorParameterValueKind.Any:
                    return ParseAnyQueryOperatorParameterValue();
                case QueryOperatorParameterValueKind.StringLiteral:
                    return ParseAnyQueryOperatorParameterValue()
                        ?? CreateMissingStringLiteral();
                case QueryOperatorParameterValueKind.BoolLiteral:
                    return ParseAnyQueryOperatorParameterValue()
                        ?? CreateMissingBoolLiteral();
                case QueryOperatorParameterValueKind.IntegerLiteral:
                case QueryOperatorParameterValueKind.NumericLiteral:
                case QueryOperatorParameterValueKind.SummableLiteral:
                    return ParseAnyQueryOperatorParameterValue()
                        ?? CreateMissingLongLiteral();
                case QueryOperatorParameterValueKind.ForcedRealLiteral:
                    return ParseAnyQueryOperatorParameterForcedRealValue()
                        ?? CreateMissingRealLiteral();
                case QueryOperatorParameterValueKind.ScalarLiteral:
                    return ParseAnyQueryOperatorParameterValue()
                        ?? CreateMissingValue();
                case QueryOperatorParameterValueKind.String:
                    return ParseFunctionCallOrPath()
                        ?? CreateMissingValue();
                case QueryOperatorParameterValueKind.Word:
                case QueryOperatorParameterValueKind.WordOrNumber:
                    if (queryParameter.Values.Count > 0)
                        return ParseTokenLiteral(queryParameter.Values)
                            ?? ParseAnyQueryOperatorParameterValue()
                            ?? CreateMissingTokenLiteral(queryParameter.Values);
                    return ParseAnyQueryOperatorParameterValue()
                        ?? CreateMissingValue();
                case QueryOperatorParameterValueKind.NameDeclaration:
                    return ParseNameDeclaration()
                        ?? ParseAnyQueryOperatorParameterValue()
                        ?? CreateMissingNameDeclaration();
                case QueryOperatorParameterValueKind.Column:
                    return ParseNameReference()
                        ?? ParseAnyQueryOperatorParameterValue()
                        ?? CreateMissingNameReference();
                case QueryOperatorParameterValueKind.ColumnList:
                    return ParseNameReferenceList(fnEndNameList ?? FnScanQueryOperatorParameterNameListEnd);
                default:
                    return ParseAnyQueryOperatorParameterValue()
                        ?? CreateMissingValue();
            }
        }

        private bool ScanQueryOperatorParameterNameListEnd()
        {
            if (PeekToken().Kind == SyntaxKind.CommaToken)
            {
                return ScanKnownQueryOperatorParameterName(equalasNeeded: false, offset: 1) > 0
                    || ScanCommonListEnd(1);
            }
            else
            {
                return ScanKnownQueryOperatorParameterName(equalasNeeded: true) > 0
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
                        ParseQueryOperatorParameterValue(parameter, fnEndNameList),
                        GetExpressionHint(parameter));
                }
                else if (PeekToken(len).Kind == SyntaxKind.EqualToken)
                {
                    return new NamedParameter(
                        new NameDeclaration(new TokenName(ParseToken(parameter.Name))),
                        ParseRequiredToken(SyntaxKind.EqualToken),
                        ParseQueryOperatorParameterValue(parameter, fnEndNameList),
                        GetExpressionHint(parameter));
                }
            }

            return null;
        }

        private static Editor.CompletionHint GetExpressionHint(QueryOperatorParameter parameter)
        {
            switch (parameter.ValueKind)
            {
                case QueryOperatorParameterValueKind.Column:
                case QueryOperatorParameterValueKind.ColumnList:
                    return Editor.CompletionHint.Column;
                default:
                    return Editor.CompletionHint.None;
            }
        }

        private NamedParameter ParseQueryOperatorParameter()
        {
            var nameToken = ParseQueryOperatorParameterName(_queryOperatorParameterNamesAllowed, _queryOperatorParameterEqualsNeeded);
            if (nameToken != null)
            {
                var name = new NameDeclaration(new TokenName(nameToken));
                var equal = ParseRequiredToken(SyntaxKind.EqualToken);

                if ((_operatorSpecificNameToQueryOperatorParameterMap != null
                    && _operatorSpecificNameToQueryOperatorParameterMap.TryGetValue(nameToken.Text, out var queryParameter))
                    || s_nameToDefaultQueryOperatorParameterMap.TryGetValue(nameToken.Text, out queryParameter))
                {
                    var value = ParseQueryOperatorParameterValue(queryParameter);
                    return new NamedParameter(name, equal, value, GetExpressionHint(queryParameter));
                }

                // not a known parameter, but parse it anyway
                return new NamedParameter(name, equal, ParseAnyQueryOperatorParameterValue() ?? CreateMissingValue());
            }

            return null;
        }

        private static readonly Func<QueryParser, NamedParameter> FnParseQueryOperatorParameter =
            qp => qp.ParseQueryOperatorParameter();

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_nameToDefaultQueryOperatorParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.AllParameters);

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
            // if its not a keyword or a legal identifier, then assume it is a multi-token name like foo-bar
            return !SyntaxFacts.IsKeyword(name) && !KustoFacts.CanBeIdentifier(name);
        }

        /// <summary>
        /// The set of known query operator parameter names
        /// </summary>
        private static readonly HashSet<string> s_knownQueryOperaterParameterNames =
            new HashSet<string>(
                s_nameToDefaultQueryOperatorParameterMap.Keys
                .Concat(KustoFacts.KnownQueryOperatorParameterNames));

        /// <summary>
        /// The list of known query parameters names that are likely to be parsed as multiple lexical tokens
        /// </summary>
        private static readonly IReadOnlyList<string> s_multiTokenQueryOperatorParameterNames =
            s_knownQueryOperaterParameterNames.Where(IsMultiTokenName).ToList();

        private bool TryGetSpecificQueryOperatorParameter(string name, out QueryOperatorParameter parameter)
        {
            if (_operatorSpecificNameToQueryOperatorParameterMap != null)
            {
                return _operatorSpecificNameToQueryOperatorParameterMap.TryGetValue(name, out parameter);
            }

            parameter = null;
            return false;
        }

        private bool IsSpecificQueryOperatorParameterName(string name)
        {
            return TryGetSpecificQueryOperatorParameter(name, out _);
        }

        private bool ScanSpecificQueryOperatorParameterName(bool equalsNeeded, int offset = 0)
        {
            var lt = PeekToken(offset);

            if ((lt.Kind == SyntaxKind.IdentifierToken || lt.Kind.IsKeyword())
                && TryGetSpecificQueryOperatorParameter(lt.Text, out var parameter))
            {
                if (parameter.HasNoEquals || !equalsNeeded)
                    return true;

                // allow this to be a query operator parameter name if it is followed by equals
                return PeekToken(offset + 1).Kind == SyntaxKind.EqualToken;
            }
            else
            {
                return false;
            }
        }

        private SyntaxToken ParseSpecificQueryOperatorParameterName()
        {
            if (ScanSpecificQueryOperatorParameterName(equalsNeeded: false))
            {
                return ParseToken();
            }

            return null;
        }

        private bool IsKnownQueryOperatorParameterName(string name)
        {
            return s_knownQueryOperaterParameterNames.Contains(name)
                || IsSpecificQueryOperatorParameterName(name);
        }

        private int ScanKnownQueryOperatorParameterName(bool equalasNeeded, int offset = 0)
        {
            var token = PeekToken(offset);

            int len = -1;

            if ((token.Kind == SyntaxKind.IdentifierToken || token.Kind.IsKeyword())
                && IsKnownQueryOperatorParameterName(token.Text))
            {
                len = 1;
            }
            else
            {
                // check for any special names that don't conform to identifiers
                for (int i = 0; i < s_multiTokenQueryOperatorParameterNames.Count; i++)
                {
                    len = ScanToken(s_multiTokenQueryOperatorParameterNames[i]);
                    if (len > 0)
                        break;
                }
            }

            if (len > 0
                && equalasNeeded
                && PeekToken(offset + len).Kind != SyntaxKind.EqualToken)
                return -1;

            return len;
        }

        private SyntaxToken ParseKnownQueryOperatorParameterName()
        {
            var lt = PeekToken();
            if ((lt.Kind == SyntaxKind.IdentifierToken || lt.Kind.IsKeyword())
                && IsKnownQueryOperatorParameterName(lt.Text))
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

        private int ScanQueryOperatorParameterName(AllowedNameKind namesAllowed, bool equalsNeeded, int offset = 0)
        {
            int len = -1;

            if (namesAllowed == AllowedNameKind.DeclaredOnly || namesAllowed == AllowedNameKind.DeclaredOrKnown)
            {
                len = ScanSpecificQueryOperatorParameterName(equalsNeeded, offset) ? 1 : -1;
            }

            if (len < 0 && (namesAllowed == AllowedNameKind.KnownOnly || namesAllowed == AllowedNameKind.DeclaredOrKnown))
            {
                len = ScanKnownQueryOperatorParameterName(equalsNeeded, offset);
            }

            return len;
        }

        private SyntaxToken ParseQueryOperatorParameterName(AllowedNameKind namesAllowed, bool equalsNeeded)
        {
            var len = ScanQueryOperatorParameterName(namesAllowed, equalsNeeded);
            if (len > 0)
            {
                switch (namesAllowed)
                {
                    case AllowedNameKind.DeclaredOnly:
                        return ParseSpecificQueryOperatorParameterName();
                    case AllowedNameKind.KnownOnly:
                        return ParseKnownQueryOperatorParameterName();
                    case AllowedNameKind.DeclaredOrKnown:
                    default:
                        return ParseSpecificQueryOperatorParameterName()
                               ?? ParseKnownQueryOperatorParameterName();
                }
            }

            return null;
        }

        private NameReference ParseNameReferenceListName()
        {
            return ScanSpecificQueryOperatorParameterName(equalsNeeded: true)
                ? null // don't consume other known query operator parameter names as names in the name list
                : ParseExtendedNameReference();
        }

        private static readonly Func<QueryParser, NameReference> FnParseNameReferenceListName =
            qp => qp.ParseNameReferenceListName();

        private NameReferenceList ParseNameReferenceList(Func<QueryParser, bool> fnEndList)
        {
            var names = ParseCommaList(FnParseNameReferenceListName, FnCreateMissingNameReference, fnEndList, oneOrMore: true);
            return new NameReferenceList(names);
        }

        private IReadOnlyDictionary<string, QueryOperatorParameter> _operatorSpecificNameToQueryOperatorParameterMap;
        private AllowedNameKind _queryOperatorParameterNamesAllowed;
        private bool _queryOperatorParameterEqualsNeeded;

        private SyntaxList<NamedParameter> ParseQueryOperatorParameterList(
            IReadOnlyDictionary<string, QueryOperatorParameter> nameToParameterMap = null,
            AllowedNameKind namesAllowed = AllowedNameKind.DeclaredOrKnown,
            bool equalsNeeded = false)
        {
            var oldParameters = _operatorSpecificNameToQueryOperatorParameterMap;
            var oldNamesAllowed = _queryOperatorParameterNamesAllowed;
            var oldEqualsNeeded = _queryOperatorParameterEqualsNeeded;

            _operatorSpecificNameToQueryOperatorParameterMap = nameToParameterMap;
            _queryOperatorParameterNamesAllowed = namesAllowed;
            _queryOperatorParameterEqualsNeeded = equalsNeeded;

            var list = ParseList(FnParseQueryOperatorParameter);

            _operatorSpecificNameToQueryOperatorParameterMap = oldParameters;
            _queryOperatorParameterNamesAllowed = oldNamesAllowed;
            _queryOperatorParameterEqualsNeeded = oldEqualsNeeded;

            return list;
        }

        private SyntaxList<SeparatedElement<NamedParameter>> ParseQueryOperatorParameterCommaList(
            IReadOnlyDictionary<string, QueryOperatorParameter> nameToParameterMap = null,
            Func<QueryParser, bool> fnScanEnd = null,
            AllowedNameKind namesAllowed = AllowedNameKind.DeclaredOrKnown)
        {
            var oldParameters = _operatorSpecificNameToQueryOperatorParameterMap;
            var oldNamesAllowed = _queryOperatorParameterNamesAllowed;

            _operatorSpecificNameToQueryOperatorParameterMap = nameToParameterMap;
            _queryOperatorParameterNamesAllowed = namesAllowed;

            var list = ParseCommaList(FnParseQueryOperatorParameter, CreateMissingNamedParameter, FnScanCommonListEnd);

            _operatorSpecificNameToQueryOperatorParameterMap = oldParameters;
            _queryOperatorParameterNamesAllowed = oldNamesAllowed;

            return list;
        }

        #endregion

        #region Entity Names

        private Expression ParseBracketedEntityNamePathElementSelector() =>
            ParseBracketedWildcardedNameReference()
            ?? ParseBracketedNameReference();

        private static Func<QueryParser, Expression> FnParseBracketedEntityNamePathElementSelector =
            qp => qp.ParseBracketedEntityNamePathElementSelector();

        private Expression ParseEntityPathExpression()
        {
            var expr = ParsePathElementSelectorOrFunctionCall();

            if (expr != null)
            {
                while (true)
                {
                    var kind = PeekToken().Kind;
                    if (kind == SyntaxKind.DotToken)
                    {
                        expr = new PathExpression(expr, ParseToken(), ParsePathElementSelectorOrFunctionCall() ?? CreateMissingNameReference());
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

        private bool ScanQualifiedEntityStart()
        {
            return ((PeekToken().Text == Functions.Database.Name
                || PeekToken().Text == Functions.Cluster.Name)
                && PeekToken(1).Kind == SyntaxKind.OpenParenToken);
        }

        private Expression ParseSimplePathExpression()
        {
            var expr = ParsePathElementSelector();

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
            ParseSimplePathExpression();

        private static readonly Func<QueryParser, Expression> FnParseSimplePathExpression =
            qp => qp.ParseSimplePathExpression();

        private Expression ParseWildcardedEntityExpression()
        {
            if (ScanWildcardedName() > 0)
            {
                return ParseWildcardedNameReference();
            }
            else
            {
                var expr = ParsePathElementSelectorOrFunctionCall();

                if (expr != null)
                {
                    while (true)
                    {
                        var kind = PeekToken().Kind;
                        if (kind == SyntaxKind.DotToken)
                        {
                            var dot = ParseToken();
                            if (ScanWildcardedName() > 0)
                            {
                                expr = new PathExpression(expr, dot, ParseWildcardedNameReference());
                                return expr;
                            }
                            else
                            {
                                expr = new PathExpression(expr, dot, ParsePathElementSelectorOrFunctionCall() ?? CreateMissingNameReference());
                            }
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
        }

        private static Func<QueryParser, Expression> FnParseWildcardedEntityExpression =
            qp => qp.ParseWildcardedEntityExpression();


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
                var expressions = ParseCommaList(FnParseNamedExpression, CreateMissingExpression, FnScanCommonListEnd, oneOrMore: true);
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
                var expressions = ParseCommaList(FnParseSimplePathExpression, FnCreateMissingNameReferenceAsExpression, FnScanFacetExpressionListEnd, oneOrMore: true);
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
                var parameters = ParseQueryOperatorParameterList(s_filterOperatorParameterMap, equalsNeeded: true);
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
                var kind = ParseQueryOperatorParameter(QueryOperatorParameters.GetSchemaKind);
                return new GetSchemaOperator(keyword, kind);
            }
            return null;
        }

        #endregion

        #region find

        private Expression ParseFindOperand_NameWithOptionalAsOperator()
        {
            Expression expr =
                ParseBracketedEntityNamePathElementSelector()
                ?? ParseBarePathElementSelector();

            if (expr != null
                && ParseToken(SyntaxKind.BarToken) is SyntaxToken barToken)
            {
                var asOp = ParseAsOperator() ?? CreateMissingQueryOperator();
                expr = new PipeExpression(expr, barToken, asOp);
            }

            return expr;
        }

        private static Func<QueryParser, Expression> FnParseFindOperand_NameWithOptionalAsOperator =
            qp => qp.ParseFindOperand_NameWithOptionalAsOperator();

        private static IReadOnlyList<Func<QueryParser, Expression>> FindOperandParsers =
            new[]
            {
                FnParseFindOperand_NameWithOptionalAsOperator,
                FnParseWildcardedEntityExpression
            };

        private Expression ParseFindOperand()
        {
            return ParseBest(FindOperandParsers);
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
                var type = ParseParamTypeExtended() ?? ParseInvalidParamType() ?? CreateMissingType();
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
                var parameters = ParseQueryOperatorParameterList(s_searchOperatorParameterMap, equalsNeeded: true);
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

        private ForkExpression CreateMissingForkExpression() =>
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

        private Expression ParseForkPipeExpression()
        {
            Expression expr = ParseForkPipeQueryOperator();
            if (expr != null)
            {
                while (PeekToken().Kind == SyntaxKind.BarToken)
                {
                    var pipe = ParseToken();
                    var pipedOperator = ParseRequiredQueryOperator();
                    expr = new PipeExpression(expr, pipe, pipedOperator);
                }
            }

            return expr;
        }

        private QueryOperator ParseForkPipeQueryOperator() =>
            ParsePipedQueryOperator()
            ?? ParseUnpipedQueryOperator(); // not legal, but let semantic analyzer flag the error.

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

        private PartitionSubquery ParsePartitionSubquery()
        {
            var open = ParseToken(SyntaxKind.OpenParenToken);
            if (open != null)
            {
                var expr = ParsePipeSubExpression() ?? ParseExpression() ?? CreateMissingExpression();
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new PartitionSubquery(open, expr, close);
            }

            return null;
        }

        private PartitionScope ParsePartitionScope()
        {
            var inKeyword = ParseToken(SyntaxKind.InKeyword);
            if (inKeyword != null)
            {
                var scope = ParseFunctionCallExpression() ?? ParseDynamicLiteral() ?? CreateMissingExpression();
                return new PartitionScope(inKeyword, scope);
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
                    return ParsePartitionSubquery();
                default:
                    return null;
            }
        }

        private PartitionOperand CreateMissingPartitionOperand() =>
            new PartitionSubquery(
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
                var byExpr = ParseSimplePathExpression() ?? CreateMissingNameReference();
                var scope = ParsePartitionScope();
                var operand = ParsePartitionOperand() ?? CreateMissingPartitionOperand();
                return new PartitionOperator(keyword, parameters, byKeyword, byExpr, scope, operand);
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

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_GraphMarkComponentsParametersMap =
                        CreateQueryOperatorParameterMap(QueryOperatorParameters.GraphMarkComponentsParameters);

        private JoinOperator ParseJoinOperator()
        {
            var keyword = ParseToken(SyntaxKind.JoinKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_joinOperatorParameterMap, equalsNeeded: true);
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
                var parameters = ParseQueryOperatorParameterList(s_lookupOperatorParameterMap, equalsNeeded: true);
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

        private MakeSeriesExpression CreateMissingMakeSeriesExpression() =>
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
                var parameters = ParseQueryOperatorParameterList(s_makeSeriesOperatorParameterMap, equalsNeeded: true);
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

        private MvExpandExpression CreateMissingMvExpandExpression() =>
            new MvExpandExpression(CreateMissingExpression(), null);

        private static readonly IReadOnlyList<SyntaxKind> s_mvExpandExpressionListEnd =
            new[] { SyntaxKind.LimitKeyword };

        private static readonly Func<QueryParser, bool> FnScanMvExpandExpressionListEnd =
            qp => qp.ScanCustomListEnd(s_mvExpandExpressionListEnd);

        private SyntaxList<SeparatedElement<MvExpandExpression>> ParseMvExpandExpressionList()
        {
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
                var parameters = ParseQueryOperatorParameterList(s_mvExpandOperatorParameterMap, equalsNeeded: true);
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

        private MvApplyExpression CreateMissingMvApplyExpression() =>
            new MvApplyExpression(CreateMissingExpression(), null);

        private static readonly IReadOnlyList<SyntaxKind> s_mvApplyExpressionListEnd =
            new[] { SyntaxKind.LimitKeyword, SyntaxKind.IdKeyword, SyntaxKind.OnKeyword };

        private bool ScanMvApplyExpressionListEnd()
        {
            // don't allow use of one of the expected sub-clause keywords as a expression name
            // unless it is is obvious it is part of the expression
            if (ScanCustomListEnd(s_mvApplyExpressionListEnd))
            {
                if (PeekToken(1) is LexicalToken nextToken
                   && nextToken.Kind != SyntaxKind.CommaToken
                   && nextToken.Kind != SyntaxKind.ToKeyword
                   && nextToken.Kind != SyntaxKind.EqualToken)
                {
                    return true;
                }
            }

            return ScanCommonListEnd();
        }

        private static readonly Func<QueryParser, bool> FnScanMvApplyExpressionListEnd =
            qp => qp.ScanMvApplyExpressionListEnd();

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

        private MvApplySubqueryExpression CreateMissingMvApplySubqueryExpression() =>
            new MvApplySubqueryExpression(
                CreateMissingToken(SyntaxKind.OpenParenToken),
                CreateMissingExpression(),
                CreateMissingToken(SyntaxKind.CloseParenToken));

        private MvApplyOperator ParseMvApplyOperator()
        {
            var keyword = ParseToken(SyntaxKind.MvApplyKeyword) ?? ParseToken(SyntaxKind.MvDashApplyKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_mvApplyOperatorParmeterMap, equalsNeeded: true);
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
                var schema = ParseEvaluateRowSchema() ?? CreateMissingEvaluateRowSchema();
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
                var functionCall = ParseRequiredFunctionCallExpression();
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
                var parameters = ParseQueryOperatorParameterList(s_parseOperatorParameterMap, equalsNeeded: true);
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
                var parameters = ParseQueryOperatorParameterList(s_parseOperatorParameterMap, equalsNeeded: true);
                var expr = ParseUnnamedExpression() ?? CreateMissingExpression();
                var with = ParseRequiredToken(SyntaxKind.WithKeyword);
                var withExprs = ParseList(FnParseParseWithExpression);
                return new ParseWhereOperator(keyword, parameters, expr, with, withExprs);
            }

            return null;
        }

        #endregion

        #region parse-kv
        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_parseKvOperatorWithParametersMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.ParseKvWithProperties);

        private ParseKvWithClause ParseParseKvWithClause()
        {
            var keyword = ParseToken(SyntaxKind.WithKeyword);
            if (keyword != null)
            {
                var open = ParseRequiredToken(SyntaxKind.OpenParenToken);
                var props = ParseQueryOperatorParameterCommaList(s_parseKvOperatorWithParametersMap, namesAllowed: AllowedNameKind.DeclaredOnly);
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new ParseKvWithClause(keyword, open, props, close);
            }

            return null;
        }

        private ParseKvOperator ParseParseKvOperator()
        {
            var keyword = ParseToken(SyntaxKind.ParseKvKeyword);
            if (keyword != null)
            {
                var expr = ParseUnnamedExpression() ?? CreateMissingExpression();
                var asKeyword = ParseRequiredToken(SyntaxKind.AsKeyword);
                var keys = ParseRowSchema() ?? CreateMissingRowSchema();
                var withClause = ParseParseKvWithClause();
                return new ParseKvOperator(keyword, expr, asKeyword, keys, withClause);
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
                if (kind == SyntaxKind.AscKeyword
                    || kind == SyntaxKind.DescKeyword
                    || kind == SyntaxKind.GrannyAscKeyword
                    || kind == SyntaxKind.GrannyDescKeyword)
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
                var expressions = ParseCommaList(FnParseProjectReorderExpression, CreateMissingExpression, FnScanCommonListEnd);
                return new ProjectReorderOperator(keyword, expressions);
            }

            return null;
        }

        private ProjectByNamesOperator ParseProjectByNamesOperator()
        {
            var keyword = ParseToken(SyntaxKind.ProjectByNamesKeyword);
            if (keyword != null)
            {
                var expressions = ParseCommaList(FnParseUnnamedExpression, CreateMissingExpression, FnScanCommonListEnd);
                return new ProjectByNamesOperator(keyword, expressions);
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
                var parameters = ParseQueryOperatorParameterList(s_sampleOperatorParameterMap, equalsNeeded: true);
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
                var parameters = ParseQueryOperatorParameterList(s_sampleDistinctOperatorParameterMap, equalsNeeded: true);
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

        private NamedExpression ParseSummarizeByBinClause()
        {
            // this is support for legacy syntax
            if (PeekToken().Kind == SyntaxKind.BinKeyword
                && PeekToken(1).Kind == SyntaxKind.EqualToken)
            {
                var keyword = ParseToken(SyntaxKind.BinKeyword);
                var equal = ParseToken(SyntaxKind.EqualToken);
                var value = ParseUnnamedExpression() ?? CreateMissingExpression();
                return new SimpleNamedExpression(
                    new NameDeclaration(new TokenName(keyword)),
                    equal,
                    value);
            }

            return null;
        }

        private bool ScanSummarizeByClauseExpressionListEnd()
        {
            // don't consume expression if it looks like legacy bin=value syntax
            if (PeekToken().Kind == SyntaxKind.BinKeyword
                && PeekToken(1).Kind == SyntaxKind.EqualToken
                && PeekToken(2).Kind.IsLiteral())
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
                var binClause = ParseSummarizeByBinClause();
                return new SummarizeByClause(keyword, expressions, binClause);
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
                var parameters = ParseQueryOperatorParameterList(s_summarizeOperatorParameterMap, equalsNeeded: true);
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
                ?? ParseNamedExpression();
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
                var parameters = ParseQueryOperatorParameterList(s_distinctOperatorParameterMap, equalsNeeded: true);
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
                var parameters = ParseQueryOperatorParameterList(s_takeOperatorParameterMap, equalsNeeded: true);
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
            var keyword
                = ParseToken(SyntaxKind.AscKeyword)
                ?? ParseToken(SyntaxKind.DescKeyword)
                ?? ParseToken(SyntaxKind.GrannyAscKeyword)
                ?? ParseToken(SyntaxKind.GrannyDescKeyword);
            var nulls = ParseOrderingNullsClause();
            return new OrderingClause(keyword, nulls);
        }

        private OrderingClause ParseRequiredOrderingNoNullsClause()
        {
            var keyword
                = ParseToken(SyntaxKind.AscKeyword)
                ?? ParseToken(SyntaxKind.DescKeyword)
                ?? ParseToken(SyntaxKind.GrannyAscKeyword)
                ?? ParseToken(SyntaxKind.GrannyDescKeyword);
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
                    case SyntaxKind.GrannyAscKeyword:
                    case SyntaxKind.GrannyDescKeyword:
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
            var name = ParseExtendedNameReference();
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

        private ScanStepOutput ParseScanStepOutput()
        {
            var output = ParseToken(SyntaxKind.OutputKeyword);
            if (output != null)
            {
                var equality = ParseRequiredToken(SyntaxKind.EqualToken);
                var outputKind = ParseRequiredToken(KustoFacts.ScanStepOutputValues);
                return new ScanStepOutput(output, equality, outputKind);
            }

            return null;
        }

        private ScanStep ParseScanStep()
        {
            var keyword = ParseToken(SyntaxKind.StepKeyword);
            if (keyword != null)
            {
                var name = ParseExtendedNameDeclaration() ?? CreateMissingNameDeclaration();
                var optional = ParseToken(SyntaxKind.OptionalKeyword);
                var output = ParseScanStepOutput();
                var colon = ParseRequiredToken(SyntaxKind.ColonToken);
                var expr = ParseUnnamedExpression() ?? CreateMissingExpression();
                var computation = ParseScanComputationClause();
                var semicolon = ParseRequiredToken(SyntaxKind.SemicolonToken);
                return new ScanStep(keyword, name, optional, output, colon, expr, computation, semicolon);
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

        private ScanPartitionByClause ParseScanPartitionByClause()
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
                var declarations = ParseCommaList(FnParseFunctionParameter, CreateMissingFunctionParameter, FnScanCommonListEnd);
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
                var partition = ParseScanPartitionByClause();
                var declare = ParseScanDeclareClause();
                var with = ParseRequiredToken(SyntaxKind.WithKeyword);
                var open = ParseRequiredToken(SyntaxKind.OpenParenToken);
                var steps = ParseList(FnParseScanStep, fnScanEnd: FnScanCommonListEnd);
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new ScanOperator(keyword, parameters, order, partition, declare, with, open, steps, close);
            }

            return null;
        }

        private PartitionByOperator ParsePartitionByOperator()
        {
            if (ParseToken(SyntaxKind.PartitionByKeyword) is SyntaxToken keyword)
            {
                var parameters = ParseQueryOperatorParameterList(s_partitionByParameters);
                var entity = ParseSimplePathExpression() ?? CreateMissingNameReference();
                var idClause = ParsePartitionByIdClause();
                var openParen = ParseRequiredToken(SyntaxKind.OpenParenToken);
                var subQuery = ParseContextualSubExpression() ?? CreateMissingExpression();
                var closeParen = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new PartitionByOperator(keyword, parameters, entity, idClause, openParen, subQuery, closeParen);
            }

            return null;
        }

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_partitionByParameters =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.PartitionByParameters);

        private PartitionByIdClause ParsePartitionByIdClause()
        {
            if (ParseToken(SyntaxKind.IdKeyword) is SyntaxToken keyword)
            {
                var value = ParseLiteral() ?? CreateMissingValue();
                return new PartitionByIdClause(keyword, value);
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
                var parameters = ParseQueryOperatorParameterList(s_topOperatorParameterMap, equalsNeeded: true);
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

        private static IReadOnlyList<Func<QueryParser, Expression>> ParseUnionExpressionParsers =
            new[]
            {
                FnParseBracketedEntityNamePathElementSelector,
                FnParseWildcardedEntityExpression,
                FnParseBarePathElementSelector
            };

        private Expression ParseUnionExpression()
        {
            return
                ParseParenthesizedExpression()
                ?? ParseBest(ParseUnionExpressionParsers);
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
                var parameters = ParseQueryOperatorParameterList(s_unionOperatorParameterMap, equalsNeeded: true);
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
                var parameters = ParseQueryOperatorParameterList(s_asOperatorParameterMap, equalsNeeded: true);
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
                var parameters = ParseQueryOperatorParameterList(s_serializeOperatorParameterMap, equalsNeeded: true);
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
                var leadingComma = ParseToken(SyntaxKind.CommaToken); // optional
                var props = ParseQueryOperatorParameterCommaList(s_renderOperatorWithPropertiesMap, namesAllowed: AllowedNameKind.DeclaredOnly);
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new RenderWithClause(keyword, open, leadingComma, props, close);
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

        #region MacroExpand
        private Expression ParseEntityGroupReference()
        {
            var explicitOrFunctionCallEntityGroup = ParseEntityGroup();
            if (explicitOrFunctionCallEntityGroup != null)
            {
                return explicitOrFunctionCallEntityGroup;
            }

            if (ScanQualifiedEntityStart())
                return ParseEntityPathExpression();

            return ParseNameReference();
        }

        private MacroExpandScopeReferenceName OptionalParseMacroExpandScopeReferenceName()
        {
            var asKeyword = ParseToken(SyntaxKind.AsKeyword);
            if (asKeyword != null)
            {
                var scopeReferenceName = ParseNameDeclaration() ?? CreateMissingNameDeclaration();
                return new MacroExpandScopeReferenceName(asKeyword, scopeReferenceName);
            }

            return null;
        }

        private MacroExpandOperator ParseMacroExpand()
        {
            var keyword = ParseToken(SyntaxKind.MacroExpandKeyword);
            if (keyword != null)
            {
                var parameters = ParseQueryOperatorParameterList(s_unionOperatorParameterMap, equalsNeeded: true);
                var entityGroupExpression = ParseEntityGroupReference() ?? CreateMissingExpression();
                var macroExpandScopeReferenceName = OptionalParseMacroExpandScopeReferenceName();
                var open = ParseRequiredToken(SyntaxKind.OpenParenToken);
                var queryBlocksStatementList = ParseQueryBlockStatementList();
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new MacroExpandOperator(keyword, parameters, entityGroupExpression, macroExpandScopeReferenceName, open, queryBlocksStatementList, close);
            }

            return null;
        }
        #endregion

        #region assert-schema
        private AssertSchemaOperator ParseAssertSchemaOperator()
        {
            var keyword = ParseToken(SyntaxKind.AssertSchemaKeyword);
            if (keyword != null)
            {
                var schema = ParseRowSchema() ?? CreateMissingRowSchema();
                return new AssertSchemaOperator(keyword, schema);
            }

            return null;
        }
        #endregion

        #region make-graph
        private QueryOperator ParseMakeGraphOperator()
        {
            if (ParseToken(SyntaxKind.MakeGraphKeyword) is SyntaxToken makeGraphKeyword)
            {
                var parameters = ParseQueryOperatorParameterList(s_graphMakeParameterMap, equalsNeeded: true);
                var sourceColumn = ParseNameReference() ?? CreateMissingNameReference();
                var directionToken =
                    ParseToken("-->", SyntaxKind.DashDashGreaterThanToken)
                    ?? ParseToken("--", SyntaxKind.DashDashToken)
                    ?? CreateMissingDirectionToken();
                var targetColumn = ParseNameReference() ?? CreateMissingNameReference();
                var withClause = ParseMakeGraphWithImplicitIdClause() ?? ParseMakeGraphWithTablesAndKeysClause();
                var partitionedByClause = ParseMakeGraphPartitionedByClause();

                return new MakeGraphOperator(makeGraphKeyword, parameters, sourceColumn, directionToken, targetColumn, withClause, partitionedByClause);
            }

            return null;
        }

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_graphMakeParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.GraphMakeParameters);

        private static Func<SyntaxToken> CreateMissingDirectionToken = () =>
            CreateMissingToken(new[] { SyntaxKind.DashDashGreaterThanToken, SyntaxKind.DashDashToken });

        private MakeGraphWithClause ParseMakeGraphWithTablesAndKeysClause()
        {
            if (ParseToken(SyntaxKind.WithKeyword) is SyntaxToken withKeyword)
            {
                var tablesAndKeys = ParseCommaList(FnParseMakeGraphTableAndKeyClause, CreateMissingMakeGraphTableAndKeyClause, FnScanMakeGraphWhereClauseListEnd, oneOrMore: true);
                return new MakeGraphWithTablesAndKeysClause(withKeyword, tablesAndKeys);
            }

            return null;
        }

        private static Func<MakeGraphTableAndKeyClause> CreateMissingMakeGraphTableAndKeyClause = () =>
            new MakeGraphTableAndKeyClause(
                new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                SyntaxToken.Missing(SyntaxKind.OnKeyword),
                new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                new[] { DiagnosticFacts.GetMissingExpression() }
                );


        private static Func<QueryParser, MakeGraphTableAndKeyClause> FnParseMakeGraphTableAndKeyClause =
            qp => qp.ParseMakeGraphTableAndKeyClause();

        private static readonly Func<QueryParser, bool> FnScanMakeGraphWhereClauseListEnd =
            qp => qp.ScanMakeGraphTableAndKeyClauseListEnd();

        private bool ScanMakeGraphTableAndKeyClauseListEnd(int offset = 0)
            => ScanCommonListEnd() || PeekToken(offset).Kind == SyntaxKind.PartitionedByKeyword;

        private MakeGraphTableAndKeyClause ParseMakeGraphTableAndKeyClause()
        {
            if (ParseInvocationExpression() is Expression table)
            {
                var onKeyword = ParseRequiredToken(SyntaxKind.OnKeyword);
                var column = ParseNameReference() ?? CreateMissingNameReference();
                return new MakeGraphTableAndKeyClause(table, onKeyword, column);
            }

            return null;
        }

        private MakeGraphWithClause ParseMakeGraphWithImplicitIdClause()
        {
            if (PeekToken().Kind == SyntaxKind.WithNodeIdKeyword)
            {
                var withNodeIdKeyword = ParseToken();
                var equalToken = ParseRequiredToken(SyntaxKind.EqualToken);
                var name = ParseNameDeclaration() ?? CreateMissingNameDeclaration();
                return new MakeGraphWithImplicitIdClause(withNodeIdKeyword, equalToken, name);
            }
            return null;
        }

        private MakeGraphPartitionedByClause ParseMakeGraphPartitionedByClause()
        {
            if (ParseToken(SyntaxKind.PartitionedByKeyword) is SyntaxToken partitionedByKeyword)
            {
                var entity = ParseSimplePathExpression() ?? CreateMissingNameReference();
                var openParen = ParseRequiredToken(SyntaxKind.OpenParenToken);
                var subQuery = ParseContextualSubExpression() ?? CreateMissingExpression();
                var closeParen = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new MakeGraphPartitionedByClause(partitionedByKeyword, (NameReference)entity, openParen, subQuery, closeParen);
            }

            return null;
        }
        #endregion

        #region GraphMatchOperator
        private GraphMatchOperator ParseGraphMatchOperator()
        {
            if (ParseToken(SyntaxKind.GraphMatchKeyword) is SyntaxToken keyword)
            {
                var parameters = ParseQueryOperatorParameterList(s_graphMatchParameterMap, equalsNeeded: true);
                var patterns = ParseCommaList(FnParseGraphMatchPattern, FnCreateMissingGraphMatchPattern, oneOrMore: true);
                var whereClause = ParseWhereClause();
                var projectClause = ParseProjectClause();

                return new GraphMatchOperator(keyword, parameters, patterns, whereClause, projectClause);
            }

            return null;
        }

        private GraphShortestPathsOperator ParseGraphShortestPathsOperator()
        {
            if (ParseToken(SyntaxKind.GraphShortestPathsKeyword) is SyntaxToken keyword)
            {
                var parameters = ParseQueryOperatorParameterList(s_graphShortestPathsParameterMap, equalsNeeded: true);
                var patterns = ParseCommaList(FnParseGraphMatchPattern, FnCreateMissingGraphMatchPattern, oneOrMore: true);
                var whereClause = ParseWhereClause();
                var projectClause = ParseProjectClause();

                return new GraphShortestPathsOperator(keyword, parameters, patterns, whereClause, projectClause);
            }

            return null;
        }

        private static readonly SyntaxKind[] s_rangeEndTokens = new SyntaxKind[]
        {
            SyntaxKind.BracketDashGreaterThanToken,
            SyntaxKind.BracketDashToken
        };

        private static Func<GraphMatchPatternNotation> FnCreateMissingGraphMatchPatternNotation =
            () => new GraphMatchPatternNode(
                SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                new NameDeclaration(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                SyntaxToken.Missing(SyntaxKind.CloseParenToken),
                new Diagnostic[] { DiagnosticFacts.GetMissingGraphMatchPattern() }
            );

        private static Func<GraphMatchPattern> FnCreateMissingGraphMatchPattern =
            () => new GraphMatchPattern(new SyntaxList<GraphMatchPatternNotation>(FnCreateMissingGraphMatchPatternNotation()));

        private static Func<QueryParser, GraphMatchPatternNotation> FnParseGraphMatchPatternNotation = qp => qp.ParseGraphMatchPatternNode() ?? qp.ParseGraphMatchPatternEdge();

        private static Func<QueryParser, GraphMatchPattern> FnParseGraphMatchPattern = qp => new GraphMatchPattern(qp.ParseList(FnParseGraphMatchPatternNotation, FnCreateMissingGraphMatchPatternNotation, oneOrMore: true));

        private GraphMatchPatternNotation ParseGraphMatchPatternNode()
        {
            if (ParseToken(SyntaxKind.OpenParenToken) is SyntaxToken open)
            {
                var name = ParseNameDeclaration();
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new GraphMatchPatternNode(open, name, close);
            }

            return null;
        }

        private GraphMatchPatternNotation ParseGraphMatchPatternEdge()
        {
            var firstToken =
                ParseToken("-->", SyntaxKind.DashDashGreaterThanToken)
                ?? ParseToken("<--", SyntaxKind.LessThanDashDashToken)
                ?? ParseToken("--", SyntaxKind.DashDashToken);

            if (firstToken != null)
            {
                return new GraphMatchPatternEdge(firstToken, null, null, null);
            }

            firstToken =
                ParseToken("-[", SyntaxKind.DashBracketToken)
                ?? ParseToken("<-[", SyntaxKind.LessThanDashBracketToken);

            if (firstToken != null)
            {
                var name = ParseNameDeclaration();
                var range = ParseGraphMatchPatternEdgeRange();

                var lastToken =
                    ParseToken("]->", SyntaxKind.BracketDashGreaterThanToken)
                    ?? ParseToken("]-", SyntaxKind.BracketDashToken)
                    ?? CreateMissingToken(s_rangeEndTokens);

                return new GraphMatchPatternEdge(firstToken, name, range, lastToken);
            }

            return null;
        }

        private GraphMatchPatternEdgeRange ParseGraphMatchPatternEdgeRange()
        {
            if (ParseToken(SyntaxKind.AsteriskToken) is SyntaxToken asterisk)
            {
                var rangeStart = ParseInvocationExpression() ?? CreateMissingValue();
                var dotDotToken = ParseRequiredToken(SyntaxKind.DotDotToken);
                var rangeEnd = ParseInvocationExpression() ?? CreateMissingExpression();
                return new GraphMatchPatternEdgeRange(asterisk, rangeStart, dotDotToken, rangeEnd);
            }

            return null;
        }

        private WhereClause ParseWhereClause()
        {
            if (ParseToken(SyntaxKind.WhereKeyword) is SyntaxToken keyword)
            {
                var expression = ParseExpression() ?? CreateMissingExpression();
                return new WhereClause(keyword, expression);
            }

            return null;
        }

        private ProjectClause ParseProjectClause()
        {
            if (ParseToken(SyntaxKind.ProjectKeyword) is SyntaxToken keyword)
            {
                var expressions = ParseCommaList(FnParseNamedExpression, CreateMissingExpression, FnScanCommonListEnd, oneOrMore: true);
                return new ProjectClause(keyword, expressions);
            }

            return null;
        }
        #endregion

        #region GraphToTableOperator
        private GraphToTableOperator ParseGraphToTableOperator()
        {
            if (ParseToken(SyntaxKind.GraphToTableKeyword) is SyntaxToken keyword)
            {
                var outputClause = ParseCommaList(FnParseGraphToTableOutputClause, CreateGraphToTableOutputClause, oneOrMore: true);
                return new GraphToTableOperator(keyword, outputClause);
            }

            return null;
        }

        private static Func<GraphToTableOutputClause> CreateGraphToTableOutputClause = () =>
            new GraphToTableOutputClause(
                SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                new GraphToTableAsClause(
                    SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                    new NameDeclaration(SyntaxToken.Missing(SyntaxKind.IdentifierToken))
                ),
                null,
                new Diagnostic[] { DiagnosticFacts.GetIncorrectNumberOfOutputGraphEntities() }
            );

        private static Func<QueryParser, GraphToTableOutputClause> FnParseGraphToTableOutputClause =
            qp => qp.ParseGraphToTableOutputClause();

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_graphMatchParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.GraphMatchParameters);

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_graphShortestPathsParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.GraphShortestPathsParameters);

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_graphToTableOperatorEdgesParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.GraphToTableEdgesParameters);

        private static readonly IReadOnlyDictionary<string, QueryOperatorParameter> s_graphToTableOperatorNodesParameterMap =
            CreateQueryOperatorParameterMap(QueryOperatorParameters.GraphToTableNodesParameters);

        private GraphToTableOutputClause ParseGraphToTableOutputClause()
        {
            var keywordToken = ParseToken(SyntaxKind.NodesKeyword) ?? ParseToken(SyntaxKind.GraphEdgesKeyword);
            if (!(keywordToken is SyntaxToken keyword))
            {
                return null;
            }

            var queryParams = keyword.Kind == SyntaxKind.NodesKeyword
                ? s_graphToTableOperatorNodesParameterMap
                : s_graphToTableOperatorEdgesParameterMap;

            var asClause = ParseGraphToTableAsClause();
            var parameters = ParseQueryOperatorParameterList(queryParams, equalsNeeded: true);

            return new GraphToTableOutputClause(keyword, asClause, parameters);
        }

        private GraphToTableAsClause ParseGraphToTableAsClause()
        {
            if (ParseToken(SyntaxKind.AsKeyword) is SyntaxToken keyword)
            {
                var name = ParseNameDeclaration() ?? CreateMissingNameDeclaration();
                return new GraphToTableAsClause(keyword, name);
            }

            return null;
        }
        #endregion

        #region GraphMarkComponentsOperator
        private GraphMarkComponentsOperator ParseGraphMarkComponentsOperator()
        {
            if (ParseToken(SyntaxKind.GraphMarkComponentsKeyword) is SyntaxToken keyword)
            {
                var parameters = ParseQueryOperatorParameterList(s_GraphMarkComponentsParametersMap, equalsNeeded: true);
                return new GraphMarkComponentsOperator(keyword, parameters);
            }

            return null;
        }
        #endregion

        #region GraphWhereNodesOperator
        private GraphWhereNodesOperator ParseGraphWhereNodesOperator()
        {
            if (ParseToken(SyntaxKind.GraphWhereNodesKeyword) is SyntaxToken keyword)
            {
                var expression = ParseUnnamedExpression() ?? CreateMissingExpression();
                return new GraphWhereNodesOperator(keyword, expression);
            }

            return null;
        }
        #endregion

        #region GraphWhereEdgesOperator
        private GraphWhereEdgesOperator ParseGraphWhereEdgesOperator()
        {
            if (ParseToken(SyntaxKind.GraphWhereEdgesKeyword) is SyntaxToken keyword)
            {
                var expression = ParseUnnamedExpression() ?? CreateMissingExpression();
                return new GraphWhereEdgesOperator(keyword, expression);
            }

            return null;
        }
        #endregion

        #endregion

        #region Query Expressions

        /// <summary>
        /// Query operators that can occur at the start of a query
        /// </summary>
        private QueryOperator ParseUnpipedQueryOperator()
        {
            switch (PeekToken().Kind)
            {
                case SyntaxKind.EvaluateKeyword:        // can be identifier
                    return ParseEvaluateOperator();
                case SyntaxKind.FindKeyword:
                    return ParseFindOperator();
                case SyntaxKind.MacroExpandKeyword:
                    return ParseMacroExpand();
                case SyntaxKind.PrintKeyword:
                    return ParsePrintOperator();
                case SyntaxKind.SearchKeyword:
                    return ParseSearchOperator();
                case SyntaxKind.UnionKeyword:
                    return ParseUnionOperator();
            }

            return null;
        }

        /// <summary>
        /// Query operators that occur after a pipe
        /// </summary>
        /// <returns></returns>
        private QueryOperator ParsePipedQueryOperator()
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
                    return ParseEvaluateOperator();         // can be identifier
                case SyntaxKind.ExecuteAndCacheKeyword:
                    return ParseExecuteAndCacheOperator();
                case SyntaxKind.ExtendKeyword:
                    return ParseExtendOperator();
                case SyntaxKind.FacetKeyword:               // can be identifier
                    return ParseFacetOperator();
                case SyntaxKind.FilterKeyword:
                case SyntaxKind.WhereKeyword:
                    return ParseFilterOperator();
                case SyntaxKind.FindKeyword:
                    return ParseFindOperator();
                case SyntaxKind.ForkKeyword:                // can be identifier
                    return ParseForkOperator();
                case SyntaxKind.GetSchemaKeyword:
                    return ParseGetSchemaOperator();
                case SyntaxKind.InvokeKeyword:
                    return ParseInvokeOperator();
                case SyntaxKind.JoinKeyword:
                    return ParseJoinOperator();
                case SyntaxKind.LookupKeyword:              // can be identifier
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
                case SyntaxKind.ParseKvKeyword:
                    return ParseParseKvOperator();
                case SyntaxKind.PartitionByKeyword:         // can be identifier
                    return ParsePartitionByOperator();
                case SyntaxKind.PartitionKeyword:           // can be identifier
                    return ParsePartitionOperator();
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
                case SyntaxKind.ProjectByNamesKeyword:
                    return ParseProjectByNamesOperator();
                case SyntaxKind.RangeKeyword:               // can be identifier
                    return ParseRangeOperator();
                case SyntaxKind.InlineExternalTableKeyword:  // can be identifier
                    return ParseInlineExternalTableExpression();
                case SyntaxKind.ReduceKeyword:              // can be identifier
                    return ParseReduceByOperator();
                case SyntaxKind.RenderKeyword:              // can be identifier
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
                case SyntaxKind.MakeGraphKeyword:
                    return ParseMakeGraphOperator();
                case SyntaxKind.GraphMatchKeyword:
                    return ParseGraphMatchOperator();
                case SyntaxKind.GraphShortestPathsKeyword:
                    return ParseGraphShortestPathsOperator();
                case SyntaxKind.GraphToTableKeyword:
                    return ParseGraphToTableOperator();
                case SyntaxKind.GraphMarkComponentsKeyword:
                    return ParseGraphMarkComponentsOperator();
                case SyntaxKind.GraphWhereNodesKeyword:
                    return ParseGraphWhereNodesOperator();
                case SyntaxKind.GraphWhereEdgesKeyword:
                    return ParseGraphWhereEdgesOperator();
                case SyntaxKind.AssertSchemaKeyword:
                    return ParseAssertSchemaOperator();
                default:
                    return null;
            }
        }

        /// <summary>
        /// Returns true if it looks like it is a query operator
        /// </summary>
        private bool ScanPossibleQueryOperator()
        {
            int nameLen;
            var kind = PeekToken().Kind;

            switch (kind)
            {
                // keywords can be identifier so might not actually be query operators
                case SyntaxKind.EvaluateKeyword:
                    // evaluate <parameter-name> ...  or evaluate <plugin-name> ...
                    return ScanName(1) > 0
                        || ScanQueryOperatorParameterName(AllowedNameKind.KnownOnly, equalsNeeded: false, 1) > 0;

                case SyntaxKind.FacetKeyword:
                    // facet by
                    return PeekToken(1).Kind == SyntaxKind.ByKeyword;

                case SyntaxKind.PartitionKeyword:
                    // partition by  or  partition <parameter-name>
                    return PeekToken(1).Kind == SyntaxKind.ByKeyword
                        || ScanQueryOperatorParameterName(AllowedNameKind.KnownOnly, equalsNeeded: false, 1) > 0;

                case SyntaxKind.RangeKeyword:
                    // range <name> from ...
                    return (nameLen = ScanName(1)) > 0
                        && PeekToken(1 + nameLen).Kind == SyntaxKind.FromKeyword;

                case SyntaxKind.InlineExternalTableKeyword:
                    // inline_external_table ( ...
                    return PeekToken(1).Kind == SyntaxKind.OpenParenToken;

                case SyntaxKind.ReduceKeyword:
                    // reduce by  or  reduce xxx
                    return PeekToken(1).Kind == SyntaxKind.ByKeyword
                        || ScanQueryOperatorParameterName(AllowedNameKind.KnownOnly, equalsNeeded: false, 1) > 0;

                case SyntaxKind.RenderKeyword:
                    // render <chart-type>
                    return ScanToken(KustoFacts.ChartTypes, 1) > 0
                        || ScanIdentifierOrKeywordAsIdentifier(1); // other unknown chart type?

                case SyntaxKind.ForkKeyword:
                case SyntaxKind.LookupKeyword:
                case SyntaxKind.PartitionByKeyword:
                    // can be identifier but too complex for look ahead
                    return false;

                case SyntaxKind.AsKeyword:
                case SyntaxKind.AssertSchemaKeyword:
                case SyntaxKind.ConsumeKeyword:
                case SyntaxKind.CountKeyword:
                case SyntaxKind.DistinctKeyword:
                case SyntaxKind.ExecuteAndCacheKeyword:
                case SyntaxKind.ExtendKeyword:
                case SyntaxKind.FilterKeyword:
                case SyntaxKind.FindKeyword:
                case SyntaxKind.GetSchemaKeyword:
                case SyntaxKind.GraphMatchKeyword:
                case SyntaxKind.GraphToTableKeyword:
                case SyntaxKind.InvokeKeyword:
                case SyntaxKind.JoinKeyword:
                case SyntaxKind.LimitKeyword:
                case SyntaxKind.MacroExpandKeyword:
                case SyntaxKind.MakeGraphKeyword:
                case SyntaxKind.GraphMarkComponentsKeyword:
                case SyntaxKind.MakeSeriesKeyword:
                case SyntaxKind.MvApplyKeyword:
                case SyntaxKind.MvDashApplyKeyword:
                case SyntaxKind.MvExpandKeyword:
                case SyntaxKind.MvDashExpandKeyword:
                case SyntaxKind.OrderKeyword:
                case SyntaxKind.ParseKeyword:
                case SyntaxKind.ParseWhereKeyword:
                case SyntaxKind.ParseKvKeyword:
                case SyntaxKind.PrintKeyword:
                case SyntaxKind.ProjectKeyword:
                case SyntaxKind.ProjectAwayKeyword:
                case SyntaxKind.ProjectKeepKeyword:
                case SyntaxKind.ProjectRenameKeyword:
                case SyntaxKind.ProjectByNamesKeyword:
                case SyntaxKind.ProjectReorderKeyword:
                case SyntaxKind.SampleKeyword:
                case SyntaxKind.SampleDistinctKeyword:
                case SyntaxKind.ScanKeyword:
                case SyntaxKind.SearchKeyword:
                case SyntaxKind.SerializeKeyword:
                case SyntaxKind.SortKeyword:
                case SyntaxKind.SummarizeKeyword:
                case SyntaxKind.TakeKeyword:
                case SyntaxKind.TopKeyword:
                case SyntaxKind.TopHittersKeyword:
                case SyntaxKind.TopNestedKeyword:
                case SyntaxKind.UnionKeyword:
                case SyntaxKind.WhereKeyword:
                    // if cannot be identifier it must be query operator
                    return !kind.CanBeIdentifier();

                default:
                    return false;
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

        private QueryOperator ParseRequiredQueryOperator()
        {
            return ParsePipedQueryOperator()
                ?? ParseUnpipedQueryOperator() // allow unpiped to parse here and deal with errors during semantic analysis
                ?? ParseBadQueryOperator() // not a query operator after a pipe, parse anyway?
                ?? CreateMissingQueryOperator();
        }

        private Expression ParsePipeExpression()
        {
            Expression expr =
                ScanPossibleQueryOperator()
                    ? (ParseUnpipedQueryOperator() ?? ParsePipedQueryOperator() ?? ParseUnnamedExpression())
                    : (ParseUnnamedExpression() ?? ParseUnpipedQueryOperator() ?? ParsePipedQueryOperator());

            if (expr != null)
            {
                while (PeekToken().Kind == SyntaxKind.BarToken)
                {
                    var pipe = ParseToken();
                    var pipedOperator = ParseRequiredQueryOperator();
                    expr = new PipeExpression(expr, pipe, pipedOperator);
                }
            }

            return expr;
        }

        private Expression ParsePipeSubExpression()
        {
            Expression expr = ParseRequiredQueryOperator();

            if (expr != null)
            {
                while (PeekToken().Kind == SyntaxKind.BarToken)
                {
                    var pipe = ParseToken();
                    var pipedOperator = ParseRequiredQueryOperator();
                    expr = new PipeExpression(expr, pipe, pipedOperator);
                }
            }

            return expr;
        }

        private Expression ParseContextualSubExpression()
        {
            var expr = (Expression)ParseContextualDataTableExpression()
                ?? ParseRequiredQueryOperator();

            if (expr != null)
            {
                while (PeekToken().Kind == SyntaxKind.BarToken)
                {
                    var pipe = ParseToken();
                    var pipedOperator = ParseRequiredQueryOperator();
                    expr = new PipeExpression(expr, pipe, pipedOperator);
                }
            }

            return expr;
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
            var statement =
                ParseLetStatement()
                ?? ParseQueryParametersStatement();

            if (statement != null)
            {
                return new SeparatedElement<Statement>(
                    statement,
                    ParseRequiredToken(SyntaxKind.SemicolonToken));
            }

            return null;
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
            // optional view keyword
            var token = PeekToken(offset);
            if (token.Kind == SyntaxKind.ViewKeyword)
            {
                offset++;
                token = PeekToken(offset);
            }

            // if this looks like parameter list then it must be a function declaration
            if (token.Kind == SyntaxKind.OpenParenToken)
            {
                var nextKind = PeekToken(offset + 1).Kind;
                if (nextKind == SyntaxKind.CloseParenToken
                    || nextKind == SyntaxKind.AsteriskToken)
                    return true;
                var nameLen = ScanExtendedName(offset + 1);
                return nameLen > 0 && PeekToken(offset + 1 + nameLen).Kind == SyntaxKind.ColonToken;
            }

            return false;
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

        private Expression ParseEntityGroup()
        {
            if (PeekToken().Kind == SyntaxKind.EntityGroupKeyword)
            {
                // try parse it as a function call entity_group("eg").
                var functionCallExpression = ParseFunctionCallExpression();
                if (functionCallExpression != null)
                {
                    return functionCallExpression;
                }

                // if the above didn't work, try to parse explicit entity group: entity_group[cluster('c1').database('db1'), ...]
                var keyword = ParseToken();
                var openBracket = ParseRequiredToken(SyntaxKind.OpenBracketToken);
                var expressions = ParseCommaList(FnParseUnnamedExpression, CreateMissingExpression, FnScanCommonListEnd, oneOrMore: true);
                var closeBracket = ParseRequiredToken(SyntaxKind.CloseBracketToken);
                return new EntityGroup(keyword, openBracket, expressions, closeBracket);
            }

            return null;
        }

        private Statement ParseLetStatement()
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
                else if (PeekToken().Kind == SyntaxKind.EntityGroupKeyword)
                {
                    return new LetStatement(keyword, name, equal, ParseEntityGroup());
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
        private static readonly IReadOnlyList<Func<QueryParser, Expression>> ParseRestrictExpressionParsers =
            new[]
            {
                FnParseWildcardedEntityExpression,
                FnParseNameReference
            };

        private Expression ParseRestrictExpression()
        {
            return ParseBest(ParseRestrictExpressionParsers);
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
                var withClause = ParseRestrictStatementWithClause();
                return new RestrictStatement(keyword, accessKeyword, toKeyword, open, expressions, close, withClause);
            }

            return null;
        }
         
        private RestrictStatementWithClause ParseRestrictStatementWithClause()
        {
            var keyword = ParseToken(SyntaxKind.WithKeyword);
            if (keyword != null)
            {
                var open = ParseRequiredToken(SyntaxKind.OpenParenToken);
                var props = ParseCommaList(parser => parser.ParseRestrictProperty(), CreateMissingNamedParameter, FnScanCommonListEnd);
                var close = ParseRequiredToken(SyntaxKind.CloseParenToken);
                return new RestrictStatementWithClause(keyword, open, props, close);
            }

            return null;
        }

        private NamedParameter ParseRestrictProperty()
        {
            var name = ParseRenameNameDeclaration();
            if (name != null)
            {
                var equal = ParseRequiredToken(SyntaxKind.EqualToken);
                var value = ParseExternalDataPropertyValue() ?? CreateMissingValue();
                return new NamedParameter(name, equal, value);
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
            if (ScanPossibleStatement())
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
                }
            }

            var expr = ParseExpression();
            if (expr != null)
                return new ExpressionStatement(expr);
            return null;
        }

        /// <summary>
        /// Returns true if it looks like it is a statement
        /// </summary>
        private bool ScanPossibleStatement()
        {
            switch (PeekToken().Kind)
            {
                case SyntaxKind.AliasKeyword:
                    return PeekToken(1).Kind == SyntaxKind.DatabaseKeyword;

                case SyntaxKind.LetKeyword:
                    return ScanName(1) > 0;

                case SyntaxKind.DeclareKeyword:
                    return PeekToken(1).Kind == SyntaxKind.PatternKeyword
                        || PeekToken(1).Kind == SyntaxKind.QueryParametersKeyword;

                case SyntaxKind.RestrictKeyword:
                    return PeekToken(1).Kind == SyntaxKind.AccessKeyword;

                case SyntaxKind.SetKeyword:
                    // cannot be identifier, must be set statement.
                    return true;

                default:
                    return false;
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

        private SyntaxList<Directive> ParseDirectiveList()
        {
            List<Directive> directives = null;

            while (ParseToken(SyntaxKind.DirectiveToken) is SyntaxToken directive)
            {
                if (directives == null)
                    directives = new List<Directive>();
                directives.Add(new Directive(directive));
            }

            return directives != null
                ? new SyntaxList<Directive>(directives)
                : SyntaxList<Directive>.Empty();
        }

        /// <summary>
        /// Parses and entire query
        /// </summary>
        private QueryBlock ParseQuery() =>
            new QueryBlock(
                ParseDirectiveList(),
                ParseQueryBlockStatementList(),
                ParseSkippedTokens(),
                ParseToken(SyntaxKind.EndOfTextToken));

        #endregion
    }
}
