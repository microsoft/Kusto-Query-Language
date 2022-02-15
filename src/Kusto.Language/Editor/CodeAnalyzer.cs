using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Utils;

    public abstract class CodeAnalyzer
    {
        /// <summary>
        /// The name of this <see cref="CodeAnalyzer"/>
        /// </summary>
        public virtual string Name => this.GetType().Name;

        /// <summary>
        /// The diagnostics produced by this <see cref="CodeAnalyzer"/>.
        /// </summary>
        public virtual IReadOnlyList<Diagnostic> Diagnostics =>
            EmptyReadOnlyList<Diagnostic>.Instance;
    }
}