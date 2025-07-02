using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Kusto.Language;
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
        private IReadOnlyList<Diagnostic> lazyAnalyzerDiagnostics;
        private IReadOnlyList<KustoAnalyzer> analyzers = KustoAnalyzers.All;
        private IReadOnlyList<KustoActor> actors = KustoActors.All;

        private KustoCodeService(
            string text, 
            GlobalState globals, 
            KustoCode code,
            IReadOnlyList<KustoAnalyzer> analyzers,
            IReadOnlyList<KustoActor> actors)
            : base(text)
        {
            this.kind = KustoCode.GetKind(text);
            this.globals = globals ?? code?.Globals ?? GlobalState.Default;
            this.lazyBoundCode = code;
            this.analyzers = analyzers ?? KustoAnalyzers.All;
            this.actors = actors ?? KustoActors.All;
        }

        public KustoCodeService(
            string text, 
            GlobalState globals = null)
            : this(text, globals ?? GlobalState.Default, null, null, null)
        {
        }

        public KustoCodeService(
            KustoCode code)
            : this(code.Text, code.Globals, code, null, null)
        {
        }

        internal KustoCodeService WithActors(IReadOnlyList<KustoActor> actors)
        {
            return new KustoCodeService(this.Text, this.globals, this.lazyBoundCode, this.analyzers, actors);
        }

        internal KustoCodeService WithAnalyzers(IReadOnlyList<KustoAnalyzer> analyzers)
        {
            return new KustoCodeService(this.Text, this.globals, this.lazyBoundCode, analyzers, this.actors);
        }

        public override string Kind => this.kind;

        /// <summary>
        /// Returns true if the text appears parsable
        /// </summary>
        private bool CanBeParsed(string text) =>
            text.Length <= globals.GetProperty(Properties.MaxParseTextSize);

        /// <summary>
        /// Determines if the parsed syntax can be analyzed
        /// </summary>
        private bool CanBeAnalyzed(KustoCode code) =>
            code.Tree.IsSafeToRecurse(globals);

        /// <summary>
        /// Gets the <see cref="KustoCode"/> for the text without waiting for semantic analysis.
        /// </summary>
        private bool TryGetBoundOrUnboundCode(CancellationToken cancellationToken, bool waitForAnalysis, out KustoCode code)
        {
            if (this.lazyUnboundCode == null 
                && this.codeException == null 
                && waitForAnalysis)
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
                        else if (CanBeParsed(this.Text))
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
                && waitForAnalysis)
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
                        else if (CanBeParsed(this.Text))
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
            if (this.lazyDiagnostics == null)
            {
                if (this.TryGetBoundCode(cancellationToken, waitForAnalysis, out var code))
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
                else if (this.codeException != null)
                {
                    var tmp = new[] { DiagnosticFacts.GetInternalFailure() }.ToSafeList();
                    Interlocked.CompareExchange(ref this.lazyDiagnostics, tmp, null);
                }
                else if (!this.CanBeParsed(this.Text))
                {
                    var tmp = new[] { DiagnosticFacts.GetQueryTextSizeExceeded() }.ToSafeList();
                    Interlocked.CompareExchange(ref this.lazyDiagnostics, tmp, null);
                }
            }

            return this.lazyDiagnostics ?? EmptyReadOnlyList<Diagnostic>.Instance;
        }

        public override bool TryGetCachedAnalyzerDiagnostics(out IReadOnlyList<Diagnostic> diagnostics)
        {
            diagnostics = this.lazyAnalyzerDiagnostics;
            return diagnostics != null;
        }

        public override IReadOnlyList<Diagnostic> GetAnalyzerDiagnostics(
            bool waitForAnalysis = true,
            CancellationToken cancellationToken = default)
        {
            if (this.lazyAnalyzerDiagnostics == null
                && this.TryGetBoundCode(cancellationToken, waitForAnalysis, out var code)
                && waitForAnalysis
                && CanBeAnalyzed(code))
            {
                var analyzerDx = new List<Diagnostic>();

                foreach (var analyzer in this.analyzers)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    // have try-catch to keep editor from crashing from analyzer bugs
                    try
                    {
                        analyzer.Analyze(code, analyzerDx, cancellationToken);
                    }
                    catch (Exception e)
                    {
                        analyzerDx.Add(DiagnosticFacts.GetAnalyzerFailure(analyzer.Name, e.Message));
                    }
                }

                // only return distinct analyzer diagnostics that are not repeats of binder diagnostics.
                var binderDx = GetDiagnostics(waitForAnalysis, cancellationToken);
                // note: binderDx is already distinct, so we can compute where remaining anallyzer dx starts
                var distinctAnalyzerDx = binderDx.Concat(analyzerDx).Distinct().Skip(binderDx.Count).ToReadOnly();

                Interlocked.CompareExchange(ref this.lazyAnalyzerDiagnostics, distinctAnalyzerDx, null);
            }

            return this.lazyAnalyzerDiagnostics ?? EmptyReadOnlyList<Diagnostic>.Instance;
        }

        public override IReadOnlyList<AnalyzerInfo> GetAnalyzers()
        {
            if (_analyzerInfos == null)
            {
                _analyzerInfos = this.analyzers.Select(a => new AnalyzerInfo(a.Name, a.Diagnostics)).ToReadOnly();
            }

            return _analyzerInfos;
        }

        private IReadOnlyList<AnalyzerInfo> _analyzerInfos;

        private bool TryGetActor(string name, out KustoActor actor)
        {
            if (_nameToActorMap == null)
            {
                _nameToActorMap = this.actors.ToDictionary(a => a.Name);
            }

            return _nameToActorMap.TryGetValue(name, out actor);
        }

        private Dictionary<string, KustoActor> _nameToActorMap;

        public override CodeActionInfo GetCodeActions(
            int position,
            int selectionStart, 
            int selectionLength,
            CodeActionOptions options, 
            bool waitForAnalysis,
            string actorName,
            CancellationToken cancellationToken)
        {
            options = options ?? CodeActionOptions.Default;
            var actorActions = new List<CodeAction>();

            if (this.TryGetBoundCode(cancellationToken, waitForAnalysis, out var code))
            {
                var actions = new List<CodeAction>();

                // if actorName is specified use just that actor
                var actors = this.actors;
                if (actorName != null)
                {
                    if (TryGetActor(actorName, out var actor))
                    {
                        actors = new[] { actor };
                    }
                    else
                    {
                        actors = EmptyReadOnlyList<KustoActor>.Instance;
                    }
                }

                foreach (var actor in actors)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    try
                    {
                        actorActions.Clear();
                        actor.GetActions(this, code, position, selectionStart, selectionLength, options, actorActions, waitForAnalysis, cancellationToken);

                        // add actor name to action data so we can find the actor later when action is applied
                        actions.AddRange(actorActions.Select(a => AddActorName(a, actor.Name)));
                    }
                    catch (Exception)
                    {
                    }
                }

                if (actions.Count > 1)
                {
                    // present them in ascending alphabetical order
                    actions.Sort((a, b) => string.Compare(a.Title, b.Title, ignoreCase: true));
                }

                return new CodeActionInfo(actions);
            }

            return CodeActionInfo.NoActions;
        }

        private CodeAction AddActorName(CodeAction action, string actorName)
        {
            return ActorUtilities.AddData(action, actorName);
        }

        public override CodeActionResult ApplyCodeAction(
            ApplyAction action, 
            int caretPosition,
            CodeActionOptions options, 
            CancellationToken cancellationToken)
        {
            options = options ?? CodeActionOptions.Default;

            if (this.TryGetBoundCode(cancellationToken, true, out var code))
            {
                var actorName = action.Data.LastOrDefault();
                if (actorName != null)
                {
                    // remove the actor name from the code action's data
                    action = action.RemoveData(1);

                    if (TryGetActor(actorName, out var actor))
                    {
                        return actor.ApplyAction(this, code, action, caretPosition, options, cancellationToken);
                    }
                    else
                    {
                        return CodeActionResult.Failure($"Unknown actor: {actorName}");
                    }
                }

                return CodeActionResult.Failure("Unknown actor");
            }
            else
            {
                return CodeActionResult.Failure("No semantic information available for this query");
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

                    var clientParameterClassifications = GetClientParametersClassifications(start, length, clipToRange);
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

        public override OutlineInfo GetOutlines(OutliningOptions options, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (this.TryGetBoundOrUnboundCode(cancellationToken, true, out var code))
            {
                try
                {
                    return KustoOutliner.GetOutlines(code, options);
                }
                catch (Exception)
                {
                }
            }

            return base.GetOutlines(options, cancellationToken);
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
                    var prevToken = token.GetPreviousToken();

                    if (position == token.TriviaStart 
                        && (token.TriviaWidth > 0 || token.Kind == SyntaxKind.EndOfTextToken)
                        && prevToken != null) // on the end of the previous token
                    {
                        token = prevToken;
                    }

                    if ((position > token.TriviaStart || (position == token.TriviaStart && prevToken == null))                       
                        && (position < token.TextStart || token.Kind == SyntaxKind.EndOfTextToken))
                    {
                        if (TriviaFacts.TryGetCommentSpan(token.Trivia, position - token.TriviaStart, out var commentStart, out var commentLength)
                            && position < commentStart + commentLength)
                        {
                            return new TextRange(commentStart + token.TriviaStart, commentLength);
                        }
                        else
                        {
                            // inside whitespace (not comment) with no affinity to either token
                            // there is no element.
                            return new TextRange(position, 0);
                        }
                    }

                    return new TextRange(token.TextStart, token.Text.Length);
                }
                catch (Exception)
                {
                }
            }

            // could not determine syntax or exception was thrown.. no information
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
                        new KustoReferenceFinder(code).GetClusterReferences(code.Syntax, null, clusters, cancellationToken);
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
                        new KustoReferenceFinder(code).GetDatabaseReferences(code.Syntax, null, this.globals.Cluster, this.globals.Database, refs, cancellationToken);
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

        public override FormattedText GetFormattedText(FormattingOptions options = null, int caretPosition = 0, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (this.TryGetBoundOrUnboundCode(cancellationToken, true, out var code)
                && CanBeAnalyzed(code))
            {
                return KustoFormatter.GetFormattedText(code.Syntax, options, caretPosition);
            }
            else
            {
                return new FormattedText(this.Text, caretPosition);
            }
        }
    }
}