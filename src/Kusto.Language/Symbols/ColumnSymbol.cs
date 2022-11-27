using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    using Syntax;
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

        /// <summary>
        /// One or more columns that this column is based on
        /// due to either an automatic rename like with join operator
        /// or unification of multiple columns like with union operator.
        /// </summary>
        public IReadOnlyList<ColumnSymbol> OriginalColumns { get; }

        /// <summary>
        /// The expression the column is computed from or
        /// the location where the column is first introduced.
        /// </summary>
        public SyntaxNode Source { get; }

        public override SymbolKind Kind => SymbolKind.Column;

        public ColumnSymbol(
            string name, 
            TypeSymbol type, 
            string description = null, 
            IReadOnlyList<ColumnSymbol> originalColumns = null,
            SyntaxNode source = null)
            : base(name)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this.Description = description ?? "";

            if (originalColumns != null && originalColumns.Count > 0)
            {
                this.OriginalColumns = GetTrulyOriginalColumns(originalColumns).ToReadOnly();
            }
            else
            {
                this.OriginalColumns = EmptyReadOnlyList<ColumnSymbol>.Instance;
            }

            this.Source = source;
        }

        /// <summary>
        /// Gets the set of columns that do not declare other original columns.
        /// </summary>
        private static IReadOnlyList<ColumnSymbol> GetTrulyOriginalColumns(IReadOnlyList<ColumnSymbol> columns)
        {
            if (columns.All(c => c.OriginalColumns.Count == 0))
                return columns;

            var mostOriginal = new List<ColumnSymbol>();
            foreach (var col in columns)
            {
                if (col.OriginalColumns.Count > 0)
                {
                    mostOriginal.AddRange(col.OriginalColumns);
                }
                else
                {
                    mostOriginal.Add(col);
                }
            }

            return mostOriginal;
        }

        public override Tabularity Tabularity => Tabularity.Scalar;

        /// <summary>
        /// Returns a <see cref="ColumnSymbol"/> with the name specified.
        /// </summary>
        public ColumnSymbol WithName(string name)
        {
            if (name != this.Name)
            {
                return new ColumnSymbol(name, this.Type, this.Description, this.OriginalColumns, this.Source);
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
                return new ColumnSymbol(this.Name, type, this.Description, this.OriginalColumns, this.Source);
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
                return new ColumnSymbol(this.Name, this.Type, description, this.OriginalColumns, this.Source);
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Returns a <see cref="ColumnSymbol"/> with the specified list of original columns.
        /// </summary>
        public ColumnSymbol WithOriginalColumns(IReadOnlyList<ColumnSymbol> originalColumns)
        {
            if (this.OriginalColumns != originalColumns)
            {
                return new ColumnSymbol(this.Name, this.Type, this.Description, originalColumns, this.Source);
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Returns a <see cref="ColumnSymbol"/> with the specified list of original columns.
        /// </summary>
        public ColumnSymbol WithOriginalColumns(params ColumnSymbol[] originalColumns)
        {
            return WithOriginalColumns((IReadOnlyList<ColumnSymbol>)originalColumns);
        }

        /// <summary>
        /// Returns a <see cref="ColumnSymbol"/> with the specified source expression.
        /// </summary>
        public ColumnSymbol WithSource(SyntaxNode source)
        {
            if (this.Source != source)
            {
                return new ColumnSymbol(this.Name, this.Type, this.Description, this.OriginalColumns, source);
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