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
        /// The syntax of the parsed code.
        /// </summary>
        public SyntaxNode Syntax { get; }

        /// <summary>
        /// The grammar rule used to parse the code.
        /// </summary>
        internal Parser<LexicalToken> Grammar { get; }

        /// <summary>
        /// True if semantic analysis has been performed.
        /// </summary>
        public bool HasSemantics { get; }

        /// <summary>
        /// The <see cref="GlobalState"/> used during parsing and semantic analysis.
        /// </summary>
        public GlobalState Globals { get; }

        /// <summary>
        /// The deepest node depth of the syntax tree.
        /// </summary>
        public int MaxDepth { get; }

        /// <summary>
        /// The tokens produced by the lexer.
        /// These are kept around to make reparsing faster, and are used by completion.
        /// </summary>
        private readonly LexicalToken[] lexerTokens;

        /// <summary>
        /// the tokens produced by the lexer.
        /// </summary>
        internal IReadOnlyList<LexicalToken> LexerTokens => this.lexerTokens;

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
            SyntaxNode syntax, 
            bool hasSemantics, 
            LexicalToken[] lexerTokens, 
            LocalBindingCache localCache, 
            int maxDepth)
        {
            this.Text = text;
            this.Kind = kind;
            this.Globals = globals;
            this.Grammar = grammar;
            this.Syntax = syntax;
            this.HasSemantics = hasSemantics;
            this.lexerTokens = lexerTokens;
            this.localCache = localCache;
            this.MaxDepth = maxDepth;
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

            var tokens = LexicalGrammar.GetTokens(text, alwaysProduceEndToken: true);
            return Create(text, globals ?? GlobalState.Default, tokens, analyze: false, cancellationToken: default(CancellationToken));
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

            var tokens = LexicalGrammar.GetTokens(text, alwaysProduceEndToken: true);
            return Create(text, globals ?? GlobalState.Default, tokens, analyze: true, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Creates a new <see cref="KustoCode"/> form the already parsed lexical tokens.
        /// </summary>
        private static KustoCode Create(string text, GlobalState globals, LexicalToken[] tokens, bool analyze, CancellationToken cancellationToken)
        {
            Parser<LexicalToken> grammar;
            SyntaxNode syntax;

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
                    syntax = queryBlock.ParseFirst(tokens);
                    break;
            }

            var maxDepth = ComputeMaxDepth(syntax);
            var isAnalyzable = maxDepth <= MaxAnalyzableSyntaxDepth;

            syntax.InitializeTriviaStarts();

            LocalBindingCache localCache = null;

            if (analyze && isAnalyzable)
            {
                cancellationToken.ThrowIfCancellationRequested();
                localCache = new LocalBindingCache();
                Binder.Bind(syntax, globals, localCache, null, cancellationToken);
            }

            return new KustoCode(text, kind, globals, grammar, syntax, analyze && isAnalyzable, tokens, localCache, maxDepth);
        }

        /// <summary>
        /// Walks the entire syntax tree and evaluates the maximum depth of all the nodes.
        /// </summary>
        private static int ComputeMaxDepth(SyntaxElement root)
        {
            var maxDepth = 0;
            var depth = 0;

            SyntaxElement.Walk(
                root,
                fnBefore: e =>
                {
                    depth++;
                    if (depth > maxDepth)
                        maxDepth = depth;
                },
                fnAfter: e => depth--);

            return maxDepth;
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
                return Create(this.Text, this.Globals, this.lexerTokens, analyze: true, cancellationToken);
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
                return Create(this.Text, globals, this.lexerTokens, analyze: this.HasSemantics, cancellationToken);
            }
        }

        /// <summary>
        /// Determines the code kind from the text. See <see cref="CodeKinds"/>.
        /// </summary>
        public static string GetKind(string text)
        {
            var token = LexicalGrammar.GetFirstToken(text);

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
                Binder.GetSymbolsInScope(this.Syntax, position, this.Globals, match, include, symbols, cancellationToken);
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

            return TextFacts.TryGetLineAndOffset(this.Text, position, this.lineStarts, out line, out lineOffset);
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
