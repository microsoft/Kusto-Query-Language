using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Utils;

    /// <summary>
    /// 
    /// </summary>
    public abstract class KustoActor
    {
        /// <summary>
        /// The name of the actor.
        /// </summary>
        public string Name => this.GetType().Name;

        /// <summary>
        /// Gets the set of actions that this actor offers for the code at the given position.
        /// </summary>
        public abstract void GetActions(KustoCode code, int position, IReadOnlyList<Diagnostic> additionalDiagnostics, List<CodeAction> actions, CancellationToken cancellationToken);

        /// <summary>
        /// Applies the action at the specified position if possible.
        /// </summary>
        public abstract CodeActionResult ApplyAction(KustoCode code, int position, IReadOnlyList<Diagnostic> additionalDiagnostics, CodeAction action, CancellationToken cancellationToken);
    }
}