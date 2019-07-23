using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    using Utils;

    /// <summary>
    /// A base class for symbols that are types.
    /// </summary>
    public abstract class TypeSymbol : Symbol
    {
        protected TypeSymbol(string name)
            : base(name)
        {
        }
    }
}