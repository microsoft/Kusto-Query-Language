using System.Collections.Generic;

namespace Kusto.Language.Editor
{
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
        /// <param name="analyzers">An optional list of analyzers to use. If null, all known analyzers are used.</param>
        /// <param name="waitForAnalysis">If false, only return pre-computed results if any.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        public abstract IReadOnlyList<Diagnostic> GetAnalyzerDiagnostics(
            IReadOnlyList<string> analyzers = null,
            bool waitForAnalysis = true,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the list of information about the analyzers available via this <see cref="CodeService"/>.
        /// </summary>
        public abstract IReadOnlyList<AnalyzerInfo> GetAnalyzers();

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
        /// <param name="cancellationToken">Optional cancellation token.</param>
        public abstract OutlineInfo GetOutlines(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Determines if a completion list should be shown automatically during typing.
        /// </summary>
        /// <param name="position">The text position of the caret.</param>
        /// <param name="key">The last key typed.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        public abstract bool ShouldAutoComplete(int position, char key, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the completion items for the position within the text.
        /// </summary>
        /// <param name="position">The text position of the caret.</param>
        /// <param name="options">Optional options.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        public abstract CompletionInfo GetCompletionItems(int position, CompletionOptions options = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the <see cref="QuickInfo"/> associated with the position within the text.
        /// </summary>
        /// <param name="position">The text position of the caret.</param>
        /// <param name="options">Any options specified for quick info.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        public abstract QuickInfo GetQuickInfo(int position, QuickInfoOptions options = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the <see cref="TextRange"/> of the syntax element at or adjacent to the text position.
        /// </summary>
        /// <param name="position">The text position of the caret.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        public abstract TextRange GetElement(int position, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets a list of all the syntax elements related to the syntax element at or adjacent to the text position.
        /// </summary>
        /// <param name="position">The text position of the caret.</param>
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
        /// <param name="cursorPosition">The text position of the caret before formatting.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        public abstract FormattedText GetFormattedText(FormattingOptions options = null, int cursorPosition = 0, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the client parameter references embedded in the text.
        /// </summary>
        public abstract IReadOnlyList<ClientParameter> GetClientParameters();
    }

    public enum MinimalTextKind
    {
        /// <summary>
        /// Removes only whitespace and comments before the start of the query
        /// </summary>
        RemoveLeadingWhitespaceAndComments,

        /// <summary>
        /// Removes all whitespace before the query, removes all comments and reduces
        /// remaining whitespace to a single space or line break between tokens.
        /// </summary>
        MinimizeWhitespaceAndRemoveComments,

        /// <summary>
        /// Removes all whitespace before and after the query, removes all comments,
        /// and reduces any remaining whitespace to a single space between tokens.
        /// </summary>
        SingleLine,
    }
}