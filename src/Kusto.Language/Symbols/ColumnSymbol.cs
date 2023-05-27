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

        /// <summary>
        /// Example values used in intellisense completion lists.
        /// </summary>
        public IReadOnlyList<string> Examples { get; }

        public override SymbolKind Kind => SymbolKind.Column;

        public ColumnSymbol(
            string name, 
            TypeSymbol type, 
            string description = null, 
            IReadOnlyList<ColumnSymbol> originalColumns = null,
            SyntaxNode source = null,
            IReadOnlyList<string> examples = null)
            : base(name)
        {
            this.Type = (type == null || type.IsError) ? ScalarTypes.Unknown : type;
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

            this.Examples = examples.ToReadOnly();
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
        /// Create a new instance of <see cref="ColumnSymbol"/> if any of the specified values
        /// differs from current values.
        /// </summary>
        private ColumnSymbol With(
            Optional<string> name = default,
            Optional<TypeSymbol> type = default,
            Optional<string> description = default,
            Optional<IReadOnlyList<ColumnSymbol>> originalColumns = default,
            Optional<SyntaxNode> source = default,
            Optional<IReadOnlyList<string>> examples = default)
        {
            var newName = name.HasValue ? name.Value : this.Name;
            var newType = type.HasValue ? type.Value : this.Type;
            var newDesc = description.HasValue ? description.Value : this.Description;
            var newOC = originalColumns.HasValue ? originalColumns.Value : this.OriginalColumns;
            var newSource = source.HasValue ? source.Value : this.Source;
            var newExamples = examples.HasValue ? examples.Value : this.Examples;

            if (newName != this.Name
                || newType != this.Type
                || newDesc != this.Description
                || newOC != this.OriginalColumns
                || newSource != this.Source
                || newExamples != this.Examples)
            {
                return new ColumnSymbol(newName, newType, newDesc, newOC, newSource, newExamples);
            }

            return this;
        }

        /// <summary>
        /// Returns a <see cref="ColumnSymbol"/> with the name specified.
        /// </summary>
        public ColumnSymbol WithName(string name)
        {
            return With(name: name);
        }

        /// <summary>
        /// Returns a <see cref="ColumnSymbol"/> with the type specified.
        /// </summary>
        public ColumnSymbol WithType(TypeSymbol type)
        {
            return With(type: type);
        }

        /// <summary>
        /// Returns a <see cref="ColumnSymbol"/> with the specified description.
        /// </summary>
        public ColumnSymbol WithDescription(string description)
        {
            return With(description: description);
        }

        /// <summary>
        /// Returns a <see cref="ColumnSymbol"/> with the specified list of original columns.
        /// </summary>
        public ColumnSymbol WithOriginalColumns(IReadOnlyList<ColumnSymbol> originalColumns)
        {
            return With(originalColumns: new Optional<IReadOnlyList<ColumnSymbol>>(originalColumns));
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
            return With(source: source);
        }

        /// <summary>
        /// Returns a <see cref="ColumnSymbol"/> with the specified examples.
        /// </summary>
        public ColumnSymbol WithExamples(IReadOnlyList<string> examples)
        {
            return With(examples: new Optional<IReadOnlyList<string>>( examples ));
        }

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