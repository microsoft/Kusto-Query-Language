using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Utils;

    /// <summary>
    /// A <see cref="CodeActor"/> that operates on a <see cref="KustoCode"/> instance.
    /// </summary>
    public abstract class KustoActor : CodeActor
    {
        /// <summary>
        /// Gets the set of actions that this actor offers for the code at the given position.
        /// </summary>
        public abstract void GetActions(
            KustoCodeService service,
            KustoCode code, 
            int position,
            int selectionStart, 
            int selectionLength,
            CodeActionOptions options, 
            List<CodeAction> actions, 
            bool waitForAnalysis,
            CancellationToken cancellationToken);

        /// <summary>
        /// Applies the action if possible.
        /// </summary>
        public abstract CodeActionResult ApplyAction(
            KustoCodeService serive,
            KustoCode code,
            ApplyAction action,
            int caretPosition,
            CodeActionOptions options, 
            CancellationToken cancellationToken);
    }
}