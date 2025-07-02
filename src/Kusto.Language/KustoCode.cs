using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language
{
    using Binding;
    using Editor;
    using Parsing;
    using Symbols;
    using Syntax;
    using Utils;

    /// <summary>
    /// A model of a Kusto code block, with the breakdown of its syntax, diagnostics and referenced symbols.
    /// </summary>
    public sealed class KustoCode
    {
        /// <summary>
        /// The text of the code.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// The kind of the code. See <see cref="CodeKinds"/>.
        /// </summary>
        public string Kind { get; }

        /// <summary>
        /// The <see cref="SyntaxTree"/> of the parsed code.
        /// </summary>
        internal SyntaxTree Tree { get; }

        /// <summary>
        /// The root <see cref="SyntaxNode"/> of the parsed code.
        /// </summary>
        public SyntaxNode Syntax => Tree.Root;

        /// <summary>
        /// The grammar rule used to parse the code.
        /// </summary>
        internal Parser<LexicalToken> Grammar { get; }

        /// <summary>
        /// True if semantic analysis has been performed.
        /// </summary>
        public bool HasSemantics => 
            _analysisState == AnalysisState.Performed;

        /// <summary>
        /// The resulting <see cref="TypeSymbol"/> of the query or control command in the code.
        /// This value is only available when semantic analysis has been performed.
        /// </summary>
        public TypeSymbol ResultType { get; }

        /// <summary>
        /// The <see cref="GlobalState"/> used during parsing and semantic analysis.
        /// </summary>
        public GlobalState Globals { get; }

        /// <summary>
        /// The deepest node depth of the syntax tree.
        /// </summary>
        public int MaxDepth => Tree.Depth;

        /// <summary>
        /// The tokens produced by the lexer.
        /// These are kept around to make reparsing faster, and are used by completion.
        /// </summary>
        private readonly LexicalToken[] _lexerTokens;
        private readonly List<int> _lexerTokenStarts;

        /// <summary>
        /// The local cache to use for binding.  Stored here to aid debugging.
        /// </summary>
        internal readonly LocalBindingCache _localCache;

        private enum AnalysisState
        {
            NotRequested,
            Performed,
            NotSafe
        }

        private AnalysisState _analysisState;

        private KustoCode(
            string text, 
            string kind, 
            GlobalState globals, 
            Parser<LexicalToken> grammar, 
            SyntaxTree tree, 
            AnalysisState analysisState, 
            TypeSymbol resultType,
            LexicalToken[] lexerTokens, 
            List<int> lexerTokenStarts,
            LocalBindingCache localCache)
        {
            this.Text = text;
            this.Kind = kind;
            this.Globals = globals;
            this.Grammar = grammar;
            this.Tree = tree;
            this.ResultType = resultType;
            _lexerTokens = lexerTokens;
            _lexerTokenStarts = lexerTokenStarts;
            _localCache = localCache;
            _analysisState = analysisState;
        }

        /// <summary>
        /// The language dialect of the code.
        /// </summary>
        public KustoDialect Dialect
        {
            get
            {
                switch (Kind)
                {
                    case CodeKinds.Command:
                        switch (this.Globals.ServerKind)
                        {
                            case ServerKinds.ClusterManager:
                                return KustoDialect.ClusterManagerCommand;
                            case ServerKinds.DataManager:
                                return KustoDialect.DataManagerCommand;
                            case ServerKinds.Engine:
                            default:
                                return KustoDialect.EngineCommand;
                        }

                    case CodeKinds.Query:
                    case CodeKinds.Directive:
                        return KustoDialect.Query;

                    default:
                        return KustoDialect.Unknown;
                }
            }
        }

        /// <summary>
        /// Create a new <see cref="KustoCode"/> instance from the text and globals. Does not perform semantic analysis.
        /// </summary>
        /// <param name="text">The code text</param>
        /// <param name="globals">The globals to use for parsing and semantic analysis. Defaults to <see cref="GlobalState.Default"/></param>.
        public static KustoCode Parse(string text, GlobalState globals = null)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            globals = globals ?? GlobalState.Default;
            globals = globals.WithParseOptions(globals.ParseOptions.WithAlwaysProduceEndTokens(true));
            var tokens = TokenParser.ParseTokens(text, globals.ParseOptions);
            var starts = GetTokenStarts(tokens);
            return Create(text, globals, tokens, starts, analyze: false, cancellationToken: default(CancellationToken));
        }

        /// <summary>
        /// Create a new <see cref="KustoCode"/> instance from the text and globals and performs semantic analysis.
        /// </summary>
        /// <param name="text">The code text</param>
        /// <param name="globals">The globals to use for parsing and semantic analysis. Defaults to <see cref="GlobalState.Default"/></param>.
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel parsing and semantic analysis.</param>
        public static KustoCode ParseAndAnalyze(string text, GlobalState globals = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            globals = globals ?? GlobalState.Default;
            globals = globals.WithParseOptions(globals.ParseOptions.WithAlwaysProduceEndTokens(true));
            var tokens = TokenParser.ParseTokens(text, globals.ParseOptions);
            var starts = GetTokenStarts(tokens);
            return Create(text, globals, tokens, starts, analyze: true, cancellationToken: default(CancellationToken));
        }

        /// <summary>
        /// Gets the starting offsets for each token
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        private static List<int> GetTokenStarts(LexicalToken[] tokens)
        {
            var starts = new List<int>(tokens.Length + 1);
            int start = 0;
            
            for (int i = 0; i < tokens.Length; i++)
            {
                starts.Add(start);
                start = start + tokens[i].Length;
            }

            // add one more for the end
            starts.Add(start);

            return starts;
        }

        /// <summary>
        /// Creates a new <see cref="KustoCode"/> form the already parsed lexical tokens.
        /// </summary>
        private static KustoCode Create(string text, GlobalState globals, LexicalToken[] tokens, List<int> tokenStarts, bool analyze, CancellationToken cancellationToken)
        {
            Parser<LexicalToken> grammar;
            SyntaxNode syntax;

            globals = globals ?? GlobalState.Default;

            var kind = GetKind(text);
            switch (kind)
            {
                case CodeKinds.Command:
                    var commandBlock = CommandGrammar.From(globals).CommandBlock;
                    grammar = commandBlock;
                    syntax = commandBlock.ParseFirst(tokens);
                    break;
                case CodeKinds.Query:
                default:
                    var queryBlock = QueryGrammar.From(globals).QueryBlock;
                    grammar = queryBlock;
                    switch (globals.ParseOptions.ParserKind)
                    {
                        case ParserKind.Default:
                        default:
                            syntax = QueryParser.ParseQuery(tokens, options: globals.ParseOptions);
                            break;
                        case ParserKind.Grammar:
                            syntax = queryBlock.ParseFirst(tokens);
                            break;
                    }
                    break;
            }

            var tree = new SyntaxTree(syntax);

            LocalBindingCache localCache = null;
            TypeSymbol resultType = null;
            var analysisState = AnalysisState.NotRequested;

            if (analyze)
            {
                cancellationToken.ThrowIfCancellationRequested();

                localCache = new LocalBindingCache();
                if (Binder.TryBind(tree, globals, localCache, null, cancellationToken))
                {
                    resultType = DetermineResultType(syntax);
                    analysisState = AnalysisState.Performed;
                }
                else
                {
                    // if the tree is too deep, then don't bother trying to analyze it
                    // because it will likely fail.
                    analysisState = AnalysisState.NotSafe;
                }
            }

            return new KustoCode(text, kind, globals, grammar, tree, analysisState, resultType, tokens, tokenStarts, localCache);
        }

        /// <summary>
        /// Determines the result type of a query or control command block.
        /// </summary>
        private static TypeSymbol DetermineResultType(SyntaxNode root)
        {
            SyntaxList<SeparatedElement<Statement>> statements;

            switch (root)
            {
                case QueryBlock qb:
                    statements = qb.Statements;
                    break;

                case CommandBlock cb:
                    statements = cb.Statements;
                    break;

                default:
                    return null;
            }

            // get the last expression's type
            if (statements.Count > 0
                && statements[statements.Count - 1].Element is ExpressionStatement es)
            {
                return es.Expression.ResultType;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a new <see cref="KustoCode"/> with semantic analysis performed
        /// or the current instance if semantic analysis has already been performed.
        /// </summary>
        public KustoCode Analyze(GlobalState globals = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (globals == null)
            {
                globals = this.Globals;
            }

            if (this.HasSemantics && this.Globals == globals)
            {
                return this;
            }
            else
            {
                return Create(this.Text, globals, _lexerTokens, _lexerTokenStarts, analyze: true, cancellationToken);
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="KustoCode"/> with the specified <see cref="GlobalState"/>
        /// </summary>
        public KustoCode WithGlobals(GlobalState globals, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (this.Globals == globals)
            {
                return this;
            }
            else
            {
                return Create(this.Text, globals, this._lexerTokens, this._lexerTokenStarts, analyze: this.HasSemantics, cancellationToken);
            }
        }

        /// <summary>
        /// Determines the code kind from the text. See <see cref="CodeKinds"/>.
        /// </summary>
        public static string GetKind(string text)
        {
            var position = 0;

            while (position < text.Length)
            {
                var token = TokenParser.ParseToken(text, position);
                if (token != null)
                {
                    if (token.Kind == SyntaxKind.DotToken)
                        return CodeKinds.Command;

                    if (token.Kind == SyntaxKind.DirectiveToken)
                    {
                        // skip directive line and continue looking
                        var nextStart = TextFacts.GetNextLineStart(text, position + token.Length);
                        if (nextStart > position)
                        {
                            position = nextStart;
                            continue;
                        }
                    }
                }
                break;
            }

            // not a command, so it must be a query.
            return CodeKinds.Query;
        }
 
        private IReadOnlyList<Diagnostic> diagnostics;

        /// <summary>
        /// Gets all diagnostics in the code (syntactic and semantic)
        /// </summary>
        public IReadOnlyList<Diagnostic> GetDiagnostics(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (this.diagnostics == null)
            {
                var include = DiagnosticsInclude.Syntactic | DiagnosticsInclude.Semantic;
                var diagnostics = this.Syntax.GetContainedDiagnostics(include, cancellationToken);

                if (_analysisState == AnalysisState.NotSafe)
                {
                    diagnostics = diagnostics.ToSafeList().AddItem(DiagnosticFacts.GetQuerySyntaxDepthExceeded());
                }

                Interlocked.CompareExchange(ref this.diagnostics, diagnostics, null);
            }

            return this.diagnostics;
        }

        private IReadOnlyList<Diagnostic> syntaxDiagnostics;

        /// <summary>
        /// Gets syntax diagnostics in the code.
        /// </summary>
        public IReadOnlyList<Diagnostic> GetSyntaxDiagnostics(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (this.syntaxDiagnostics == null)
            {
                var diagnostics = this.Syntax.GetContainedSyntaxDiagnostics();
                Interlocked.CompareExchange(ref this.syntaxDiagnostics, diagnostics, null);
            }

            return this.syntaxDiagnostics;
        }

        /// <summary>
        /// Gets a list of all the symbols in the scope related to the specified text position.
        /// </summary>
        public IReadOnlyList<Symbol> GetSymbolsInScope(int position, SymbolMatch match = SymbolMatch.Any, IncludeFunctionKind include = IncludeFunctionKind.All, CancellationToken cancellationToken = default(CancellationToken))
        {
            var symbols = new List<Symbol>();

            if (this.HasSemantics)
            {
                Binder.GetSymbolsInScope(this.Tree, position, this.Globals, match, include, symbols, cancellationToken);
            }

            return symbols.ToReadOnly();
        }

        /// <summary>
        /// Gets the symbol that would be referenced by the name, if the name were to occur at the position in the text.
        /// </summary>
        public Symbol GetSpeculativeReferencedSymbol(int position, string name, SymbolMatch match = SymbolMatch.Any, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (this.HasSemantics)
            {
                return Binder.GetReferencedSymbol(this.Tree, position, name, this.Globals, match, cancellationToken);
            }

            return null;
        }

        /// <summary>
        /// Gets the <see cref="TableSymbol"/> that holds the columns that are implicitly in scope at the position within the query.
        /// </summary>
        public TableSymbol GetColumnsInScope(int position, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (this.HasSemantics)
            {
                return Binder.GetRowScope(this.Tree, position, this.Globals, cancellationToken);
            }

            return null;
        }

        private List<int> lineStarts;

        /// <summary>
        /// Gets the 1-based line and lineOffset for a position in the text.
        /// </summary>
        public bool TryGetLineAndOffset(int position, out int line, out int lineOffset)
        {
            if (lineStarts == null)
            {
                var tmp = new List<int>();
                TextFacts.GetLineStarts(this.Text, tmp);
                Interlocked.CompareExchange(ref lineStarts, tmp, null);
            }

            var success = TextFacts.TryGetLineAndOffset(this.lineStarts, position, out line, out lineOffset);

            if (success && (position < 0 || position > this.Text.Length))
            {
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Gets the index of the token that includes the text position.
        /// </summary>
        public int GetTokenIndex(int position)
        {
            if (this._lexerTokens.Length == 0)
                return 0;

            var lastTokenIndex = this._lexerTokens.Length - 1;
            var lastToken = this._lexerTokens[lastTokenIndex];
            var lastTokenStart = this._lexerTokenStarts[lastTokenIndex];
            if (position >= lastTokenStart + lastToken.Length)
                return this._lexerTokens.Length - 1;

            var index = this._lexerTokenStarts.BinarySearch(position);
            index = index >= 0 ? index : ~index - 1;

            return index;
        }

        /// <summary>
        /// The lexical tokens produced during parsing.
        /// </summary>
        public IReadOnlyList<LexicalToken> GetLexicalTokens()
        {
            return this._lexerTokens;
        }
    }

    [Flags]
    public enum IncludeFunctionKind
    {
        BuiltInFunctions = 1,
        DatabaseFunctions = BuiltInFunctions << 1,
        LocalFunctions = DatabaseFunctions << 1,
        LocalViews = LocalFunctions << 1,

        None = 0,
        All = BuiltInFunctions | DatabaseFunctions | LocalFunctions | LocalViews
    }

    public enum ParserKind
    {
        /// <summary>
        /// Use the grammar parser combinators to parse queries.
        /// </summary>
        Grammar,

        /// <summary>
        /// Use the faster query parser.
        /// </summary>
        Default
    }
}
