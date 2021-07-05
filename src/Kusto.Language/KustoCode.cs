using System;
using System.Collections.Generic;

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
        public bool HasSemantics { get; }

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
        private readonly LexicalToken[] lexerTokens;
        private readonly List<int> lexerTokenStarts;

        /// <summary>
        /// The local cache to use for binding.  Stored here to aid debugging.
        /// </summary>
        private readonly LocalBindingCache localCache;

        /// <summary>
        /// The maximum depth of nodes a syntax tree can have before it is considered non-analyzable.
        /// </summary>
        public static readonly int MaxAnalyzableSyntaxDepth = 500;

        private KustoCode(
            string text, 
            string kind, 
            GlobalState globals, 
            Parser<LexicalToken> grammar, 
            SyntaxTree tree, 
            bool hasSemantics, 
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
            this.HasSemantics = hasSemantics;
            this.ResultType = resultType;
            this.lexerTokens = lexerTokens;
            this.lexerTokenStarts = lexerTokenStarts;
            this.localCache = localCache;
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

            var tokens = TokenParser.ParseTokens(text, alwaysProduceEndToken: true);
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

            var tokens = TokenParser.ParseTokens(text, alwaysProduceEndToken: true);
            var starts = GetTokenStarts(tokens);
            return Create(text, globals, tokens, starts, analyze: true, cancellationToken: cancellationToken);
        }

        public static List<int> GetTokenStarts(LexicalToken[] tokens)
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
                case CodeKinds.Directive:
                    grammar = DirectiveGrammar.DirectiveBlock;
                    syntax = DirectiveGrammar.DirectiveBlock.ParseFirst(tokens);
                    break;
                case CodeKinds.Query:
                default:
                    var queryBlock = QueryGrammar.From(globals).QueryBlock;
                    grammar = queryBlock;
                    // use special query parser for performance
                    syntax = QueryParser.ParseQuery(tokens);
                    break;
            }

            var tree = new SyntaxTree(syntax);

            LocalBindingCache localCache = null;
            TypeSymbol resultType = null;
            var analyzed = false;

            if (analyze)
            {
                cancellationToken.ThrowIfCancellationRequested();
                analyzed = true;

                localCache = new LocalBindingCache();
                if (Binder.TryBind(tree, globals, localCache, null, cancellationToken))
                {
                    resultType = DetermineResultType(syntax);
                }
            }

            return new KustoCode(text, kind, globals, grammar, tree, analyzed, resultType, tokens, tokenStarts, localCache);
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
                return Create(this.Text, this.Globals, this.lexerTokens, this.lexerTokenStarts, analyze: true, cancellationToken);
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
                return Create(this.Text, globals, this.lexerTokens, this.lexerTokenStarts, analyze: this.HasSemantics, cancellationToken);
            }
        }

        /// <summary>
        /// Determines the code kind from the text. See <see cref="CodeKinds"/>.
        /// </summary>
        public static string GetKind(string text)
        {
            var token = TokenParser.ParseToken(text, 0);

            if (token != null)
            {
                if (token.Kind == SyntaxKind.DotToken)
                {
                    return CodeKinds.Command;
                }
                else if (token.Kind == SyntaxKind.DirectiveToken)
                {
                    return CodeKinds.Directive;
                }
            }

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

#if false
            // eable this allow diagnostics from function body expansion to be included to help debugging.
            include |= DiagnosticsInclude.Expansion;
#endif
                var diagnostics = this.Syntax.GetContainedDiagnostics(include, cancellationToken);
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

            return TextFacts.TryGetLineAndOffset(this.lineStarts, position, out line, out lineOffset);
        }

        /// <summary>
        /// Gets the index of the token that includes the text position.
        /// </summary>
        public int GetTokenIndex(int position)
        {
            if (this.lexerTokens.Length == 0)
                return 0;

            var lastTokenIndex = this.lexerTokens.Length - 1;
            var lastToken = this.lexerTokens[lastTokenIndex];
            var lastTokenStart = this.lexerTokenStarts[lastTokenIndex];
            if (position >= lastTokenStart + lastToken.Length)
                return this.lexerTokens.Length - 1;

            var index = this.lexerTokenStarts.BinarySearch(position);
            index = index >= 0 ? index : ~index - 1;

            return index;
        }

        /// <summary>
        /// The lexical tokens produced during parsing.
        /// </summary>
        public IReadOnlyList<LexicalToken> GetLexicalTokens()
        {
            return this.lexerTokens;
        }
    }

    [Flags]
    public enum IncludeFunctionKind
    {
        BuiltInFunctions = 1,
        DatabaseFunctions = BuiltInFunctions << 1,
        LocalFunctions = DatabaseFunctions << 1,

        None = 0,
        All = BuiltInFunctions | DatabaseFunctions | LocalFunctions
    }
}
