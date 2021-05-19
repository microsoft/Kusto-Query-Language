using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Kusto.Language.Parsing;
    using Kusto.Language.Symbols;
    using Syntax;
    using System.Linq;
    using System.Text;
    using Utils;

    /// <summary>
    /// A <see cref="CodeService"/> for Kusto queries and commands.
    /// </summary>
    public class KustoCodeService : CommonCodeService
    {
        private readonly string kind;
        private readonly GlobalState globals;

        private KustoCode lazyUnboundCode;
        private KustoCode lazyBoundCode;
        private Exception codeException;
        private IReadOnlyList<Diagnostic> lazyDiagnostics;
        private IReadOnlyList<Diagnostic> lazyExtendedDiagnostics;

        private KustoCodeService(string text, GlobalState globals, KustoCode code)
            : base(text)
        {
            if (globals == null)
                throw new ArgumentNullException(nameof(globals));

            this.kind = KustoCode.GetKind(text);
            this.globals = globals;
            this.lazyBoundCode = code;
        }

        public KustoCodeService(string text, GlobalState globals = null)
            : this(text, globals ?? GlobalState.Default, null)
        {
        }

        public KustoCodeService(KustoCode code)
            : this(code.Text, code.Globals, code)
        {
        }

        public override string Kind => this.kind;

        /// <summary>
        /// Returns true if the text appears parsable
        /// </summary>
        private static bool CanBeParsed(string text) => text.Length <= 4 * 1024 * 1024;

        /// <summary>
        /// Determines if the parsed syntax can be analyzed
        /// </summary>
        private static bool CanBeAnalyzed(KustoCode code) => code.MaxDepth <= KustoCode.MaxAnalyzableSyntaxDepth;

        /// <summary>
        /// Gets the <see cref="KustoCode"/> for the text without waiting for semantic analysis.
        /// </summary>
        private bool TryGetBoundOrUnboundCode(CancellationToken cancellationToken, bool waitForAnalysis, out KustoCode code)
        {
            if (this.lazyUnboundCode == null 
                && this.codeException == null 
                && waitForAnalysis
                && CanBeParsed(this.Text))
            {
                lock (this) // don't let multiple threads duplicate computation work
                {
                    try
                    {
                        if (this.lazyBoundCode != null)
                        {
                            // if bound code is already available, use it instead
                            code = this.lazyBoundCode;
                            return true;
                        }
                        else
                        {
                            var newCode = KustoCode.Parse(this.Text, this.globals);
                            Interlocked.CompareExchange(ref this.lazyUnboundCode, newCode, null);
                        }
                    }
                    catch (Exception e)
                    {
                        this.codeException = e;
                    }
                }
            }

            code = this.lazyUnboundCode;
            return code != null;
        }

        /// <summary>
        /// Gets the <see cref="KustoCode"/> for the text with semantic analysis done.
        /// </summary>
        private bool TryGetBoundCode(CancellationToken cancellationToken, bool waitForAnalysis, out KustoCode code)
        {
            if (this.lazyBoundCode == null 
                && this.codeException == null 
                && waitForAnalysis
                && CanBeParsed(this.Text))
            {
                lock (this) // don't let multiple threads duplicate computation work
                {
                    try
                    {
                        if (this.lazyUnboundCode != null)
                        {
                            // if unbound code is already avaiable, do semantic analysis on it (faster, no need to retokenize)
                            var newCode = this.lazyUnboundCode.Analyze(cancellationToken: cancellationToken);
                            Interlocked.CompareExchange(ref this.lazyBoundCode, newCode, null);
                        }
                        else
                        {
                            var newCode = KustoCode.ParseAndAnalyze(this.Text, this.globals, cancellationToken: cancellationToken);
                            Interlocked.CompareExchange(ref this.lazyBoundCode, newCode, null);
                        }
                    }
                    catch (Exception e)
                    {
                        this.codeException = e;
                    }
                }
            }

            code = this.lazyBoundCode;
            return code != null;
        }

        public override bool IsFeatureSupported(string feature, int position)
        {
            switch (this.Kind)
            {
                case CodeKinds.Query:
                case CodeKinds.Command:
                    // all features are supported for query and command
                    return true;

                case CodeKinds.Directive:
                    switch (feature)
                    {
                        case CodeServiceFeatures.Classification:
                            return true;
                    }
                    return false;

                default:
                    return false;
            }
        }

        public override bool TryGetCachedDiagnostics(out IReadOnlyList<Diagnostic> diagnostics)
        {
            diagnostics = this.lazyDiagnostics;
            return diagnostics != null;
        }

        public override IReadOnlyList<Diagnostic> GetDiagnostics(bool waitForAnalysis = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (this.lazyDiagnostics == null
                && this.TryGetBoundCode(cancellationToken, waitForAnalysis, out var code)
                && CanBeAnalyzed(code))
            {
                // have try-catch to keep editor from crashing from parser bugs
                try
                {
                    var ds = code.GetDiagnostics(cancellationToken);
                    Interlocked.CompareExchange(ref this.lazyDiagnostics, ds, null);
                }
                catch (Exception)
                {
                    Interlocked.CompareExchange(ref this.lazyDiagnostics, EmptyReadOnlyList<Diagnostic>.Instance, null);
                }
            }

            return this.lazyDiagnostics ?? EmptyReadOnlyList<Diagnostic>.Instance;
        }

        public override bool TryGetCachedAnalyzerDiagnostics(out IReadOnlyList<Diagnostic> diagnostics)
        {
            diagnostics = this.lazyExtendedDiagnostics;
            return diagnostics != null;
        }

        public override IReadOnlyList<Diagnostic> GetAnalyzerDiagnostics(
            IReadOnlyList<string> analyzers = null,
            bool waitForAnalysis = true,
            CancellationToken cancellationToken = default)
        {
            if (this.lazyExtendedDiagnostics == null
                && this.TryGetBoundCode(cancellationToken, waitForAnalysis, out var code)
                && waitForAnalysis
                && CanBeAnalyzed(code))
            {
                var ds = new List<Diagnostic>();

                foreach (var analyzer in KustoAnalyzers.All)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (analyzers != null && !analyzers.Contains(analyzer.Name))
                        continue;

                    // have try-catch to keep editor from crashing from analyzer bugs
                    try
                    {
                        analyzer.Analyze(code, ds, cancellationToken);
                    }
                    catch (Exception e)
                    {
                        ds.Add(DiagnosticFacts.AnalysisFailure(analyzer.Name, e.Message));
                    }
                }

                Interlocked.CompareExchange(ref this.lazyExtendedDiagnostics, ds, null);
            }

            return this.lazyExtendedDiagnostics ?? EmptyReadOnlyList<Diagnostic>.Instance;
        }

        private static IReadOnlyList<AnalyzerInfo> _analyzers;

        public override IReadOnlyList<AnalyzerInfo> GetAnalyzers()
        {
            if (_analyzers == null)
            {
                _analyzers = KustoAnalyzers.All.Select(a => new AnalyzerInfo(a.Name, a.Diagnostics)).ToReadOnly();
            }

            return _analyzers;
        }

        private static IReadOnlyList<Diagnostic> _analyzerDiagnostics;

        /// <summary>
        /// The set of known analyzer diagnostics
        /// </summary>
        public static IReadOnlyList<Diagnostic> AnalyzerDiagnostics
        {
            get
            {
                if (_analyzerDiagnostics == null)
                {
                    _analyzerDiagnostics = new KustoCodeService("").GetAnalyzers().SelectMany(a => a.Diagnostics).ToReadOnly();
                }

                return _analyzerDiagnostics;
            }
        }

        public override ClassificationInfo GetClassifications(int start, int length, bool clipToRange = true, bool waitForAnalysis = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (this.TryGetBoundCode(cancellationToken, waitForAnalysis, out var code))
            {
                // have try-catch to keep editor from crashing from parser bugs
                try
                {
                    var classifications = new List<ClassifiedRange>();
                    KustoClassifier.GetClassifications(code.Syntax, start, length, clipToRange, classifications, cancellationToken);

                    var clientParameterClassifications = GetClientParametersClassifications();
                    if (clientParameterClassifications.Count > 0)
                    {
                        var merged = Add(classifications, clientParameterClassifications);
                        return new ClassificationInfo(merged);
                    }
                    else
                    {
                        return new ClassificationInfo(classifications);
                    }
                }
                catch (Exception)
                {
                    return ClassificationInfo.Empty;
                }
            }
            else
            {
                return ClassificationInfo.Empty;
            }
        }

        public override IReadOnlyList<ClientParameter> GetClientParameters()
        {
            var cps = base.GetClientParameters();
            if (cps.Count > 0 && this.TryGetBoundOrUnboundCode(default(CancellationToken), true, out var code))
            {
                var newCps = new List<ClientParameter>();

                foreach (var cp in cps)
                {
                    var token = code.Syntax.GetTokenAt(cp.Start);

                    // only count as true client parameter if it is inside token text
                    if (cp.Start >= token.TextStart)
                    {
                        newCps.Add(cp);
                    }
                }

                return newCps;
            }

            return cps;
        }

        public override OutlineInfo GetOutlines(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (this.TryGetBoundOrUnboundCode(cancellationToken, true, out var code))
            {
                try
                {
                    var collapsedText = GetOutlineCollapsedText(code);
                    var length = TextFacts.TrimEnd(this.Text, 0, this.Text.Length);
                    return new OutlineInfo(new[] { new OutlineRange(0, length, collapsedText) });
                }
                catch (Exception)
                {
                }
            }

            return base.GetOutlines(cancellationToken);
        }

        private static string GetOutlineCollapsedText(KustoCode code)
        {
            var builder = new StringBuilder();

            for (var token = code.Syntax.GetFirstToken(); token != null; token = token.GetNextToken())
            {
                if (token.Text == "|" || token.Text == ";")
                    break;

                if (token.Trivia.Length > 0)
                {
                    if (builder.Length == 0)
                    {
                        builder.Append(token.Trivia);
                    }
                    else
                    {
                        builder.Append(" ");
                    }
                }

                builder.Append(token.Text);
            }

            return builder.ToString();
        }

        public override bool ShouldAutoComplete(int position, char key, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (this.TryGetBoundOrUnboundCode(cancellationToken, true, out var code)
                && CanBeAnalyzed(code))
            {
                try
                {
                    return new KustoCompleter(code, CompletionOptions.Default, cancellationToken)
                        .ShouldAutoComplete(position, key);
                }
                catch (Exception)
                {
                }
            }

            return false;
        }

        public override CompletionInfo GetCompletionItems(int position, CompletionOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (this.TryGetBoundCode(cancellationToken, true, out var code) 
                && CanBeAnalyzed(code))
            {
                // have try-catch to keep editor from crashing from parser bugs
                try
                {
                    return new KustoCompleter(code, options ?? CompletionOptions.Default, cancellationToken)
                        .GetCompletionItems(position);
                }
                catch (Exception)
                {
                }
            }

            return CompletionInfo.Empty;
        }

        public override QuickInfo GetQuickInfo(int position, QuickInfoOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (this.TryGetBoundCode(cancellationToken, true, out var code)
                && CanBeAnalyzed(code))
            {
                // have try-catch to keep editor from crashing from parser bugs
                try
                {
                    return new KustoQuickInfoBuilder(this, code, options ?? QuickInfoOptions.Default).GetQuickInfo(position, cancellationToken);
                }
                catch (Exception)
                {
                    return QuickInfo.Empty;
                }
            }
            else
            {
                if (this.codeException != null)
                {
                    return new QuickInfo(this.codeException.Message);
                }

                return QuickInfo.Empty;
            }
        }

        public override TextRange GetElement(int position, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (this.TryGetBoundCode(cancellationToken, true, out var code))
            {
                try
                {
                    var token = code.Syntax.GetTokenAt(position);
                    return new TextRange(token.TextStart, token.Text.Length);
                }
                catch (Exception)
                {
                }
            }

            return new TextRange(0,0);
        }

        public override RelatedInfo GetRelatedElements(int position, FindRelatedOptions options = FindRelatedOptions.None, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (this.TryGetBoundCode(cancellationToken, true, out var code)
                && CanBeAnalyzed(code))
            {
                try
                {
                    return new KustoRelatedElementFinder(code).FindRelatedElements(position, options);
                }
                catch (Exception)
                {
                }
            }

            return RelatedInfo.Empty;
        }

        public override IReadOnlyList<ClusterReference> GetClusterReferences(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (this.TryGetBoundCode(cancellationToken, true, out var code)
                && CanBeAnalyzed(code))
            {
                try
                {
                    var clusters = new List<ClusterReference>();

                    if (code.HasSemantics)
                    {
                        GetClusterReferences(code.Syntax, null, clusters, cancellationToken);
                    }

                    return clusters.ToReadOnly();
                }
                catch (Exception)
                {
                    return EmptyReadOnlyList<ClusterReference>.Instance;
                }
            }
            else
            {
                return EmptyReadOnlyList<ClusterReference>.Instance;
            }
        }

        private void GetClusterReferences(SyntaxNode root, SyntaxNode location, List<ClusterReference> clusters, CancellationToken cancellationToken)
        {
            root.WalkElements(element =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (element is FunctionCallExpression fc && fc.ReferencedSymbol is FunctionSymbol fs)
                {
                    // if this is a call to cluster('xxx') method then make a cluster reference from the literal argument if possible
                    if (fs == Functions.Cluster)
                    {
                        var cluster = GetClusterReference(fc, location);
                        if (cluster != null)
                        {
                            clusters.Add(cluster);
                        }
                    }
                    else if (fs.Signatures[0].HasClusterCall)
                    {
                        // look for cluster('xxx') calls in function expansions
                        var expansion = fc.GetExpansion();
                        if (expansion != null)
                        {
                            GetClusterReferences(expansion, location ?? fc.Name, clusters, cancellationToken);
                        }
                    }
                }
            });
        }

        private static ClusterReference GetClusterReference(FunctionCallExpression fc, SyntaxNode location)
        {
            if (fc.ReferencedSymbol == Functions.Cluster
                && TryGetConstantStringArgumentValue(fc, 0, out var name))
            {
                location = location ?? fc.ArgumentList.Expressions[0].Element;
                return new ClusterReference(name, location.TextStart, location.Width);
            }

            return null;
        }

        private static bool TryGetConstantStringArgumentValue(FunctionCallExpression fc, int index, out string constant)
        {
            if (fc.ArgumentList.Expressions.Count > index && fc.ArgumentList.Expressions[index].Element.IsConstant)
            {
                constant = fc.ArgumentList.Expressions[index].Element.ConstantValue as string;
                return constant != null;
            }

            constant = null;
            return false;
        }

        public override IReadOnlyList<DatabaseReference> GetDatabaseReferences(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (this.TryGetBoundCode(cancellationToken, true, out var code)
                && CanBeAnalyzed(code))
            {
                try
                {
                    var refs = new List<DatabaseReference>();

                    if (code.HasSemantics)
                    {
                        GetDatabaseReferences(code.Syntax, null, this.globals.Cluster, this.globals.Database, refs, cancellationToken);
                    }

                    return refs;
                }
                catch (Exception)
                {
                    return EmptyReadOnlyList<DatabaseReference>.Instance;
                }
            }
            else
            {
                return EmptyReadOnlyList<DatabaseReference>.Instance;
            }
        }

        private void GetDatabaseReferences(SyntaxNode root, SyntaxNode location, ClusterSymbol defaultCluster, DatabaseSymbol defaultDatabase, List<DatabaseReference> refs, CancellationToken cancellationToken)
        {
            root.WalkElements(element =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (element is FunctionCallExpression fc && fc.ReferencedSymbol is FunctionSymbol fs)
                {
                    if (fs == Functions.Database)
                    {
                        var dbref = GetDatabaseReference(fc, location, defaultCluster);
                        if (dbref != null)
                        {
                            refs.Add(dbref);
                        }
                    }
                    else if (fs.Signatures[0].HasDatabaseCall)
                    {
                        var expansion = fc.GetExpansion();
                        if (expansion != null)
                        {
                            var db = defaultDatabase;
                            var cluster = defaultCluster;

                            db = this.globals.GetDatabase(fs) ?? defaultDatabase;
                            cluster = this.globals.GetCluster(db) ?? defaultCluster;

                            GetDatabaseReferences(expansion, location ?? fc.Name, cluster, db, refs, cancellationToken);
                        }
                    }
                }
            });
        }

        private DatabaseReference GetDatabaseReference(FunctionCallExpression fc, SyntaxNode location, ClusterSymbol defaultCluster)
        {
            if (fc.ReferencedSymbol == Functions.Database
                && TryGetConstantStringArgumentValue(fc, 0, out var databaseName))
            {
                location = location ?? fc.ArgumentList.Expressions[0].Element;

                string cluster;

                // get cluster name from explicit cluster reference (if possible)
                if (!(fc.Parent is PathExpression p 
                    && p.Selector == fc 
                    && p.Expression is FunctionCallExpression fcCluster 
                    && fcCluster.ReferencedSymbol == Functions.Cluster
                    && TryGetConstantStringArgumentValue(fcCluster, 0, out cluster)))
                {
                    // otherwise use the default cluster
                    cluster = defaultCluster.Name;
                }

                return new DatabaseReference(databaseName, cluster, location.TextStart, location.Width);
            }

            return null;
        }

        public static IncludeTrivia GetIncludeTrivia(MinimalTextKind kind)
        {
            switch (kind)
            {
                case MinimalTextKind.RemoveLeadingWhitespaceAndComments:
                    return IncludeTrivia.Interior;
                case MinimalTextKind.MinimizeWhitespaceAndRemoveComments:
                    return IncludeTrivia.Minimal;
                case MinimalTextKind.SingleLine:
                    return IncludeTrivia.SingleLine;
                default:
                    throw new InvalidOperationException($"Unhandled MinimalTextKind '{kind}'");
            }
        }

        public override string GetMinimalText(MinimalTextKind kind, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (this.TryGetBoundOrUnboundCode(cancellationToken, true, out var code))
            {
                return code.Syntax.ToString(GetIncludeTrivia(kind));
            }

            return this.Text;
        }

        public override FormattedText GetFormattedText(FormattingOptions options = null, int cursorPosition = 0, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (this.TryGetBoundOrUnboundCode(cancellationToken, true, out var code)
                && CanBeAnalyzed(code))
            {
                return KustoFormatter.GetFormattedText(code.Syntax, options, cursorPosition);
            }
            else
            {
                return new FormattedText(this.Text, cursorPosition);
            }
        }
    }
}