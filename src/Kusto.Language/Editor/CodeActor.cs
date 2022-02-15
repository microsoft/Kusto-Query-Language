using System;

namespace Kusto.Language.Editor
{
    public abstract class CodeActor
    {
        /// <summary>
        /// The name of the <see cref="CodeActor"/>
        /// </summary>
        public virtual string Name => this.GetType().Name;
    }
}