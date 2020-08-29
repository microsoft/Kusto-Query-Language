using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    using Utils;

    /// <summary>
    /// A symbol for a query options (assigned via set statement)
    /// </summary>
    public sealed class OptionSymbol : Symbol
    {
        public string Description { get; }

        public IReadOnlyList<ScalarSymbol> Types { get; }

        public IReadOnlyList<string> Examples { get; }

        public override SymbolKind Kind => SymbolKind.Option;

        public OptionSymbol(string name, 
            string description = null,
            IReadOnlyList<ScalarSymbol> types = null, 
            IReadOnlyList<string> examples = null)
            : base(name)
        {
            this.Description = description ?? "";
            this.Types = types ?? EmptyReadOnlyList<ScalarSymbol>.Instance;
            this.Examples = examples ?? EmptyReadOnlyList<string>.Instance;
        }

        public OptionSymbol(string name, 
            string description, 
            ScalarSymbol type, 
            IReadOnlyList<string> examples = null)
            : this(name, description, new[] { type }, examples)
        {
        }

        public override Tabularity Tabularity => Tabularity.Scalar;

        protected override string GetDisplay() =>
            $"{this.Name}";
    }
}