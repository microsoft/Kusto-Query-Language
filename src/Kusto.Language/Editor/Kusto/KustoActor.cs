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
        public abstract void GetActions(KustoCode code, int position, int length, CodeActionOptions options, List<CodeAction> actions, CancellationToken cancellationToken);

        /// <summary>
        /// Applies the action at the specified position if possible.
        /// </summary>
        public abstract CodeActionResult ApplyAction(KustoCode code, int position, int length, CodeActionOptions options, CodeAction action, CancellationToken cancellationToken);
    }
}