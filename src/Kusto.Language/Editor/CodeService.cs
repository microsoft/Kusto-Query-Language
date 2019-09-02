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
        public abstract string Kind { get; }

        /// <summary>
        /// The text of the code.
        /// </summary>
        public abstract string Text { get; }

        /// <summary>
        /// Determines if the feature is supported at the position within the text.
        /// If the position is not specified, then the feature is considered over the entire text.
        /// </summary>
        public abstract bool IsFeatureSupported(string feature, int position = -1);

        /// <summary>
        /// Gets the diagnostics for the code.
        /// </summary>
        public abstract IReadOnlyList<Diagnostic> GetDiagnostics(bool waitForAnalysis = true, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the classifications for the elements the specified text range.
        /// </summary>
        public abstract ClassificationInfo GetClassifications(int start, int length, bool waitForAnalysis = true, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the ranges of the text that can be expanded or collapsed.
        /// </summary>
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
        public abstract CompletionInfo GetCompletionItems(int position, CompletionOptions options = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the <see cref="QuickInfo"/> associated with the position within the text.
        /// </summary>
        public abstract QuickInfo GetQuickInfo(int position, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the <see cref="TextRange"/> of the syntax element at or adjacent to the text position.
        /// </summary>
        public abstract TextRange GetElement(int position, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets a list of all the syntax elements related to the syntax element at or adjacent to the text position.
        /// </summary>
        public abstract RelatedInfo GetRelatedElements(int position, FindRelatedOptions options = FindRelatedOptions.None, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets a list of all the explicit cluster references in the text.
        /// These are clusters specified in calls to the cluster() function.
        /// </summary>
        public abstract IReadOnlyList<ClusterReference> GetClusterReferences(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets a list of all the explicit database references in the text.
        /// These are databases specified in calls to the database() function.
        /// </summary>
        public abstract IReadOnlyList<DatabaseReference> GetDatabaseReferences(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the text with all whitespace/trivia minimized.
        /// </summary>
        public abstract string GetMinimalText(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the text with all the whitespace/trivia formatted using the specified options.
        /// </summary>
        public abstract FormattedText GetFormattedText(FormattingOptions options = null, int cursorPosition = 0, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the client parameter references embedded in the text.
        /// </summary>
        public abstract IReadOnlyList<ClientParameter> GetClientParameters();
    }
}