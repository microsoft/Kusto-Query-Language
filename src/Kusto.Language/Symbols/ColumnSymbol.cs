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
        /// <summary>
        /// The type of the column.
        /// </summary>
        public TypeSymbol Type { get; }

        /// <summary>
        /// The description of the column.
        /// </summary>
        public string Description { get; }

        public override SymbolKind Kind => SymbolKind.Column;

        public ColumnSymbol(string name, TypeSymbol type, string description = null)
            : base(name)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this.Description = description ?? "";
        }

        public override Tabularity Tabularity => Tabularity.Scalar;

        /// <summary>
        /// Returns a <see cref="ColumnSymbol"/> with the name specified.
        /// </summary>
        public ColumnSymbol WithName(string name)
        {
            if (name != this.Name)
            {
                return new ColumnSymbol(name, this.Type, this.Description);
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
                return new ColumnSymbol(this.Name, type, this.Description);
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Returns a <see cref="ColumnSymbol"/> with the specified description.
        /// </summary>
        public ColumnSymbol WithDescription(string description)
        {
            if (description != this.Description)
            {
                return new ColumnSymbol(this.Name, this.Type, description);
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
            var columns = new List<ColumnSymbol>();

            foreach (var list in columnSets)
            {
                columns.AddRange(list);
            }

            switch (kind)
            {
                case CombineKind.UnifySameNameAndType:
                    Binding.Binder.UnifyColumnsWithSameNameAndType(columns);
                    break;

                case CombineKind.UnifySameName:
                    Binding.Binder.UnifyColumnsWithSameName(columns);
                    break;

                case CombineKind.UniqueNames:
                    Binding.Binder.MakeColumnNamesUnique(columns);
                    break;
            }

            return columns;
        }

        /// <summary>
        /// Combines multiple sets of columns into a single set of columns.
        /// </summary>
        public static IReadOnlyList<ColumnSymbol> Combine(CombineKind kind, params IReadOnlyList<ColumnSymbol>[] columnSets)
        {
            return Combine(kind, (IEnumerable<IReadOnlyList<ColumnSymbol>>)columnSets);
        }
    }
}