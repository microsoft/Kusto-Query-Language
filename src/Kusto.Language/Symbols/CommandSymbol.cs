using System;
using System.Collections.Generic;
using System.Linq;
using Kusto.Language.Parsing;
using Kusto.Language.Syntax;

namespace Kusto.Language.Symbols
{
    using Utils;

    public class CommandSymbol : Symbol
    {
        /// <summary>
        /// The command grammar
        /// </summary>
        public string Grammar { get; }

        /// <summary>
        /// The result type of this command.
        /// </summary>
        public TypeSymbol Type { get; }

        public override SymbolKind Kind => SymbolKind.Command;

        public CommandSymbol(string name, string grammar, TypeSymbol type = null)
            : base(name)
        {
            this.Grammar = grammar;
            this.Type = type ?? VoidSymbol.Instance;
        }

        public CommandSymbol(string grammar, TypeSymbol type = null)
            : this(grammar, grammar, type)
        {
        }
    }
}
