using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Utils;

    /// <summary>
    /// A <see cref="KustoActor"/> implements code fixes or refactorings.
    /// </summary>
    internal abstract class KustoActor
    {
        /// <summary>
        /// The name of this actor.
        /// This is used find the actor that created the <see cref="CodeAction"/> when the action is applied.
        /// </summary>
        public virtual string Name => this.GetType().Name;

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