using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    using Utils;

    /// <summary>
    /// A symbol representing a column.
    /// </summary>
    public sealed class ColumnSymbol : Symbol
    {
        public TypeSymbol Type { get; }

        public override SymbolKind Kind => SymbolKind.Column;

        public ColumnSymbol(string name, TypeSymbol type)
            : base(name)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public override Tabularity Tabularity => Tabularity.Scalar;

        /// <summary>
        /// Returns a <see cref="ColumnSymbol"/> with the name specified.
        /// </summary>
        public ColumnSymbol WithName(string name)
        {
            if (name != this.Name)
            {
                return new ColumnSymbol(name, this.Type);
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Returns a <see cref="ColumnSymbol"/> with the type specified.
        /// </summary>
        public ColumnSymbol WithType(TypeSymbol type)
        {
            if (type != this.Type)
            {
                return new ColumnSymbol(this.Name, type);
            }
            else
            {
                return this;
            }
        }

        protected override string GetDisplay() =>
            $"{this.Name}: {this.Type.Display}";

        /// <summary>
        /// Combines multiple sets of columns into a single set of columns.
        /// </summary>
        public static IReadOnlyList<ColumnSymbol> Combine(CombineKind kind, IEnumerable<IReadOnlyList<ColumnSymbol>> columnSets)
        {
            var result = new List<ColumnSymbol>();

            foreach (var list in columnSets)
            {
                result.AddRange(list);
            }

            switch (kind)
            {
                case CombineKind.UnifySameNameAndType:
                    Binding.Binder.UnifyColumnsWithSameNameAndType(result);
                    break;

                case CombineKind.UnifySameName:
                    Binding.Binder.UnifyColumnsWithSameName(result);
                    break;

                case CombineKind.UniqueNames:
                    Binding.Binder.MakeColumnNamesUnique(result);
                    break;
            }

            return result;
        }

        public static IReadOnlyList<ColumnSymbol> Combine(CombineKind kind, params IReadOnlyList<ColumnSymbol>[] columnSets)
        {
            return Combine(kind, (IEnumerable<IReadOnlyList<ColumnSymbol>>)columnSets);
        }
    }
}