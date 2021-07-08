using System;
using System.Collections.Generic;

namespace Kusto.Language.Binding
{
    using Symbols;
    using Syntax;
    using Utils;

    /// <summary>
    /// A class that manages building a list of columns in a projection
    /// </summary>
    internal class ProjectionBuilder
    {
        /// <summary>
        /// The current projection list
        /// </summary>
        private readonly List<ColumnSymbol> _projection = new List<ColumnSymbol>();

        /// <summary>
        /// A list of columns that should not be added to the projection.
        /// </summary>
        private readonly HashSet<ColumnSymbol> _doNotAdd = new HashSet<ColumnSymbol>();

        /// <summary>
        /// A map between column names in the projection and their current index.
        /// </summary>
        private readonly Dictionary<string, int> _columnIndexMap = new Dictionary<string, int>();

        /// <summary>
        /// Column names that were explicitly declared in the projection: Name = e
        /// </summary>
        private readonly HashSet<string> _declaredNames = new HashSet<string>();

        /// <summary>
        /// Names already in use.
        /// </summary>
        private readonly UniqueNameTable _uniqueNames = new UniqueNameTable();

        public ProjectionBuilder()
        {
        }

        /// <summary>
        /// Clears the <see cref="ProjectionBuilder"/> so it can be used again.
        /// </summary>
        public void Clear()
        {
            _projection.Clear();
            _doNotAdd.Clear();
            _columnIndexMap.Clear();
            _uniqueNames.Clear();
            _declaredNames.Clear();
        }

        /// <summary>
        /// Gets the projected columns.
        /// </summary>
        public IReadOnlyList<ColumnSymbol> GetProjection()
        {
            return _projection;
        }

        /// <summary>
        /// Adds one or more columns to the projection list, renaming them if necessary.
        /// </summary>
        /// <param name="columns">The list of columns to add.</param>
        /// <param name="declare">If true, then consider these columns to have been explicitly declared, so further declarations cannot use their name.</param>
        /// <param name="doNotRepeat">If true then ignore any further attempt to add these columns.</param>
        public void AddRange(IEnumerable<ColumnSymbol> columns, bool declare = false, bool doNotRepeat = false)
        {
            foreach (var column in columns)
            {
                var c = Add(column, doNotRepeat: doNotRepeat);

                if (declare && c != null)
                {
                    _declaredNames.Add(c.Name);
                }
            }
        }

        /// <summary>
        /// Ignore any further attempt to add this column to the projection.
        /// </summary>
        public void DoNotAdd(ColumnSymbol column)
        {
            _doNotAdd.Add(column);
        }

        /// <summary>
        /// Ignore any further attempt to add any of these columns to the projection.
        /// </summary>
        public void DoNotAddAny(IEnumerable<ColumnSymbol> columns)
        {
            foreach (var col in columns)
            {
                DoNotAdd(col);
            }
        }

        /// <summary>
        /// True if an attempt to add the column will succeed.
        /// </summary>
        public bool CanAdd(ColumnSymbol column)
        {
            return !_doNotAdd.Contains(column);
        }

        /// <summary>
        /// Adds a new undeclared column to the projection list.
        /// The name will be changed if it conflicts with a previously added column.
        /// </summary>
        /// <param name="column">The column to add.</param>
        /// <param name="baseName">The base name to use when generating an alternate unique name for this column.</param>
        /// <param name="replace">If true, allow this column to replace any previously added column with the same name.</param>
        /// <param name="doNotRepeat">If true, ignore any further attempts to add this column.</param>
        public ColumnSymbol Add(ColumnSymbol column, string baseName = null, bool replace = false, bool doNotRepeat = false)
        {
            if (_doNotAdd.Contains(column))
            {
                // this column is ignored when attempting to add it.
                return column;
            }

            if (replace && _columnIndexMap.TryGetValue(column.Name, out var index))
            {
                _projection[index] = column;
            }
            else
            {
                // make sure the column name is unique.
                var uniqueName = _uniqueNames.GetOrAddName(column.Name, baseName);
                if (uniqueName != column.Name)
                {
                    // include knowledge of the original column before it got renamed
                    column = column.WithName(uniqueName);
                }

                _projection.Add(column);
                _columnIndexMap.Add(column.Name, _projection.Count - 1);
            }

            if (doNotRepeat)
            {
                _doNotAdd.Add(column);
            }

            return column;
        }

        /// <summary>
        /// The column is added if a column with the same name is not already declared.
        /// </summary>
        /// <param name="column">The column to declare.</param>
        /// <param name="diagnostics">The diagnostics list to add diagnostics to if the column's name has already been declared.</param>
        /// <param name="location">The syntax location used to associate with diagnostics.</param>
        /// <param name="replace">If true, allow this column to replace any previously added column with the same name, but not specifically declared.</param>
        public void Declare(ColumnSymbol column, List<Diagnostic> diagnostics, SyntaxNode location, bool replace = false)
        {
            // is this name already explicitly declared elsewhere?
            if (_declaredNames.Contains(column.Name))
            {
                diagnostics.Add(DiagnosticFacts.GetDuplicateColumnDeclaration(column.Name).WithLocation(location));
                return;
            }

            if (replace && _columnIndexMap.TryGetValue(column.Name, out var index))
            {
                _projection[index] = column;
                _declaredNames.Add(column.Name);
            }
            else
            {
                var added = Add(column);

                if (added != null)
                {
                    _declaredNames.Add(added.Name);
                }
            }
        }

        /// <summary>
        /// Rename a column that exists from a previous query source.
        /// </summary>
        /// <param name="oldName">The name of a column already in the projection</param>
        /// <param name="newName">The new name for the column.</param>
        /// <param name="diagnostics">The diagnostics list to add diagnostics to if the column's name has already been declared.</param>
        /// <param name="location">The syntax location used to associate with any diagnostics added.</param>
        public ColumnSymbol Rename(string oldName, string newName, List<Diagnostic> diagnostics, SyntaxNode location)
        {
            // find existing column index
            if (!_columnIndexMap.TryGetValue(oldName, out var index))
            {
                // cannot find column... should this be an error?
                return null;
            }

            // is this name already explicitly declared elsewhere?
            if (_declaredNames.Contains(newName))
            {
                diagnostics.Add(DiagnosticFacts.GetDuplicateColumnDeclaration(newName).WithLocation(location));
                return null;
            }

            var oldColumn = _projection[index];
            var newColumn = oldColumn.WithName(newName);

            _projection[index] = newColumn;
            _columnIndexMap.Remove(oldName);
            _columnIndexMap.Add(newName, index);
            _declaredNames.Add(newName);
            _uniqueNames.AddName(newName);

            return newColumn;
        }
    }
}