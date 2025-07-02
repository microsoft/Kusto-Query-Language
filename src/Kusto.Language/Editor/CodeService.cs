using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using System.Linq;
    using Utils;

    /// <summary>
    /// A service that provides intellisense and other editor related features over a piece of code.
    /// </summary>
    public abstract class CodeService
    {
        /// <summary>
        /// The kind of code found in the text (see <see cref="CodeKinds"/>).
        /// </summary>
        public virtual string Kind => "";

        /// <summary>
        /// The text of the code.
        /// </summary>
        public virtual string Text => "";

        /// <summary>
        /// Determines if the feature is supported at the position within the text.
        /// If the position is not specified, then the feature is considered over the entire text.
        /// </summary>
        /// <param name="feature">The name of feature. See <see cref="CodeServiceFeatures"/></param>
        /// <param name="position">The text position where the feature support is in question. If not specified, the entire block is considered.</param>
        public abstract bool IsFeatureSupported(string feature, int position = -1);

        /// <summary>
        /// Gets the diagnostics if already computed.
        /// </summary>
        public abstract bool TryGetCachedDiagnostics(out IReadOnlyList<Diagnostic> diagnostics);

        /// <summary>
        /// Gets the diagnostics for the code.
        /// </summary>
        /// <param name="waitForAnalysis">If false, only return pre-computed results if any.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        public abstract IReadOnlyList<Diagnostic> GetDiagnostics(bool waitForAnalysis = true, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the analyzers diagnostics if already computed.
        /// </summary>
        public abstract bool TryGetCachedAnalyzerDiagnostics(out IReadOnlyList<Diagnostic> diagnostics);

        /// <summary>
        /// Gets any additional diagnostics for the code.
        /// </summary>
        /// <param name="waitForAnalysis">If false, only return pre-computed results if any.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        public abstract IReadOnlyList<Diagnostic> GetAnalyzerDiagnostics(
            bool waitForAnalysis = true,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets a combined list of all syntax, semantic and analyzer diagnostics.
        /// </summary>
        public virtual IReadOnlyList<Diagnostic> GetCombinedDiagnostics(bool waitForAnalysis = true, DisabledDiagnostics filter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetDiagnostics(waitForAnalysis, cancellationToken)
                .Concat(GetAnalyzerDiagnostics(waitForAnalysis, cancellationToken))
                .Where(d => filter == null || filter.IsDiagnosticEnabled(d))
                .ToList();
        }

        /// <summary>
        /// Gets the list of information about the analyzers available via this <see cref="CodeService"/>.
        /// </summary>
        public abstract IReadOnlyList<AnalyzerInfo> GetAnalyzers();

        /// <summary>
        /// Gets the set of code actions available at the specified position.
        /// </summary>
        /// <param name="position">The text position to get code actions for.</param>
        /// <param name="selectionStart">The start of the text selection range.</param>
        /// <param name="selectionLength">The length of the text selection range (or zero if no selection).</param>
        /// <param name="options">An optional set of options.</param>
        /// <param name="waitForAnalysis">If false only cached diagnostics will be considered.</param>
        /// <param name="actorName">An optional actor's name to get actions from. If null, actions are obtains from all known actors.</param>
        /// <param name="cancellationToken">an optional cancellation token.</param>
        public abstract CodeActionInfo GetCodeActions(
            int position,
            int selectionStart,
            int selectionLength,
            CodeActionOptions options = null, 
            bool waitForAnalysis = true, 
            string actorName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Applies the code action at ths specified position.
        /// </summary>
        /// <param name="action">The action to apply.</param>
        /// <param name="caretPosition">The text position of the caret before the action is applied.</param>
        /// <param name="options">An optional set of options.</param>
        /// <param name="cancellationToken">an optional cancellation token.</param>
        public abstract CodeActionResult ApplyCodeAction(
            ApplyAction action,
            int caretPosition,
            CodeActionOptions options = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the classifications for the elements the specified text range.
        /// </summary>
        /// <param name="start">The start of the text range to get classifications for.</param>
        /// <param name="length">The length of the text range to get classifications for.</param>
        /// <param name="clipToRange">If true, then adjust the start and end of classification ranges so they do not start before or go beyond the specified range.</param>
        /// <param name="waitForAnalysis">If false, do not require semantic analysis to be performed.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        public abstract ClassificationInfo GetClassifications(int start, int length, bool clipToRange = true, bool waitForAnalysis = true, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the ranges of the text that can be expanded or collapsed.
        /// </summary>
        /// <param name="options">Options for how outlining behaves.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        public abstract OutlineInfo GetOutlines(OutliningOptions options, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the ranges of the text that can be expanded or collapsed.
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        public OutlineInfo GetOutlines(CancellationToken cancellationToken = default(CancellationToken)) =>
            GetOutlines(OutliningOptions.Default, cancellationToken);

        /// <summary>
        /// Determines if a completion list should be shown automatically during typing.
        /// </summary>
        /// <param name="position">The text position in question.</param>
        /// <param name="key">The last key typed.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        public abstract bool ShouldAutoComplete(int position, char key, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the completion items for the position within the text.
        /// </summary>
        /// <param name="position">The text position to get completion items for.</param>
        /// <param name="options">Optional options.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        public abstract CompletionInfo GetCompletionItems(int position, CompletionOptions options = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the <see cref="QuickInfo"/> associated with the position within the text.
        /// </summary>
        /// <param name="position">The text position to get quick info for.</param>
        /// <param name="options">Any options specified for quick info.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        public abstract QuickInfo GetQuickInfo(int position, QuickInfoOptions options = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the <see cref="TextRange"/> of the syntax element at or adjacent to the text position.
        /// </summary>
        /// <param name="position">The text position to get the corresponding element of.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        public abstract TextRange GetElement(int position, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets a list of all the syntax elements related to the syntax element at or adjacent to the text position.
        /// </summary>
        /// <param name="position">The text position of the corresponding element to get the related elements of.</param>
        /// <param name="options">Optional options</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        public abstract RelatedInfo GetRelatedElements(int position, FindRelatedOptions options = FindRelatedOptions.None, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets a list of all the explicit cluster references in the text.
        /// These are clusters specified in calls to the cluster() function.
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        public abstract IReadOnlyList<ClusterReference> GetClusterReferences(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets a list of all the explicit database references in the text.
        /// These are databases specified in calls to the database() function.
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        public abstract IReadOnlyList<DatabaseReference> GetDatabaseReferences(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the text with all whitespace/trivia minimized.
        /// </summary>
        /// <param name="kind">The kind of minimal text to produce.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        public abstract string GetMinimalText(MinimalTextKind kind, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the text with all the whitespace/trivia formatted using the specified options.
        /// </summary>
        /// <param name="options">Optional options.</param>
        /// <param name="caretPosition">The text position of the caret before formatting.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        public abstract FormattedText GetFormattedText(FormattingOptions options = null, int caretPosition = 0, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the client parameter references embedded in the text.
        /// </summary>
        public abstract IReadOnlyList<ClientParameter> GetClientParameters();
    }
}