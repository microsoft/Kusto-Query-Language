using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Binding
{
    using Parsing;
    using Symbols;
    using Syntax;
    using Utils;

    internal sealed partial class Binder
    {
        /// <summary>
        /// Adds all the columns declared by the symbol to the list of columns.
        /// </summary>
        private void AddTableColumns(Symbol symbol, List<ColumnSymbol> columns)
        {
            switch (symbol)
            {
                case TableSymbol t:
                    GetDeclaredAndInferredColumns(t, columns);
                    break;
                case GroupSymbol g:
                    foreach (var s in g.Members)
                    {
                        AddTableColumns(s, columns);
                    }
                    break;
            }
        }

        /// <summary>
        /// Add the table (or all the tables in a group) to the list of tables.
        /// </summary>
        private void AddTables(Symbol symbol, List<TableSymbol> tables)
        {
            switch (symbol)
            {
                case TableSymbol t:
                    tables.Add(t);
                    break;
                case GroupSymbol g:
                    foreach (var m in g.Members)
                    {
                        AddTables(m, tables);
                    }
                    break;
            }
        }

        /// <summary>
        /// Gets a table representing the aggregate set of columns in scope
        /// for find operator expressions.
        /// </summary>
        private TableSymbol GetFindColumnsTable(FindOperator node)
        {
            var tables = GetFindTables(node);
            return GetTableOfColumnsUnifiedByName(tables);
        }

        /// <summary>
        /// Get the set of tables applicable to the find operator.
        /// </summary>
        private IReadOnlyList<TableSymbol> GetFindTables(FindOperator node)
        {
            if (node.InClause != null)
            {
                return GetReferencedTables(node.InClause.Expressions);
            }
            else
            {
                // no in clause or row scope, so all tables in universe then!
                return GetImpliedTables();
            }
        }

        /// <summary>
        /// Gets the set of columns from the tables applicable to the search operator.
        /// </summary>
        private TableSymbol GetSearchColumnsTable(SearchOperator node)
        {
            if (_rowScope != null && node.InClause == null)
            {
                return _rowScope;
            }
            else
            {
                var tables = GetSearchTables(node);

                // access through cache
                return GetTableOfColumnsUnifiedByNameAndType(tables);
            }
        }

        /// <summary>
        /// Gets the set of tables used by the search operator
        /// </summary>
        private IReadOnlyList<TableSymbol> GetSearchTables(SearchOperator node)
        {
            if (node.InClause != null)
            {
                return GetReferencedTables(node.InClause.Expressions);
            }
            else if (_rowScope != null)
            {
                return new[] { _rowScope };
            }
            else
            {
                // no in clause or row scope, so all tables in universe then!
                return GetImpliedTables();
            }
        }

        /// <summary>
        /// Gets all the tables accessible to the current operator through osmosis,
        /// not from pipe operator or sub clause.
        /// </summary>
        private IReadOnlyList<TableSymbol> GetImpliedTables()
        {
            // include current database's tables and any views in scope
            var declaredViews = s_tableListPool.AllocateFromPool();
            try
            {
                GetViewResultTablesInScope(declaredViews);
                if (declaredViews.Count > 0)
                {
                    return _currentDatabase.Tables.Concat(declaredViews).ToList();
                }
                else
                {
                    return _currentDatabase.Tables;
                }
            }
            finally
            {
                s_tableListPool.ReturnToPool(declaredViews);
            }
        }

        /// <summary>
        /// Gets all the result tables of declared views in scope
        /// </summary>
        private void GetViewResultTablesInScope(List<TableSymbol> views)
        {
            var localSymbols = s_symbolListPool.AllocateFromPool();
            try
            {
                // get all declared tabular functions
                _localScope.GetSymbols(SymbolMatch.Tabular | SymbolMatch.View, localSymbols);

                // pick out just view function declarations
                foreach (var sym in localSymbols)
                {
                    if (sym is FunctionSymbol fs
                        && fs.IsView
                        && fs.MinArgumentCount == 0)
                    {
                        var fts = fs.GetReturnType(_globals) as TableSymbol;
                        if (fts != null)
                        {
                            views.Add(fts);
                        }
                    }
                }
            }
            finally
            {
                s_symbolListPool.ReturnToPool(localSymbols);
            }
        }

        /// <summary>
        /// Gets the set of tables referenced by the entity expressions, 
        /// where each expression is a reference to a table or group of tables.
        /// </summary>
        private IReadOnlyList<TableSymbol> GetReferencedTables(SyntaxList<SeparatedElement<Expression>> list)
        {
            var tables = new List<TableSymbol>();

            foreach (var x in list)
            {
                if (x.Element.ResultType is TableSymbol ts)
                {
                    tables.Add(ts);
                }
                else if (x.Element.ResultType is GroupSymbol gs)
                {
                    tables.AddRange(gs.Members.OfType<TableSymbol>());
                }
            }

            return tables;
        }

        /// <summary>
        /// Converts a list of columns into a list of unique (unioned columns)
        /// Columns with the same name and type will be merged into one column.
        /// Columns with the same name but different type will be renamed to include the type name as a suffix.
        /// </summary>
        internal static void UnifyColumnsWithSameNameAndType(List<ColumnSymbol> columns)
        {
            var uniqueNames = s_uniqueNameTablePool.AllocateFromPool();
            var newColumns = s_columnListPool.AllocateFromPool();
            try
            {
                var map = new ColumnMap(columns);

                // go through original column order and build out new column list
                for (int i = 0; i < columns.Count; i++)
                {
                    var col = columns[i];

                    if (map.HasColumns(col.Name))
                    {
                        if (map.HasMultipleTypes(col.Name))
                        {
                            var types = map.GetTypes(col.Name);
                            foreach (var type in types)
                            {
                                var sameTypeColumns = map.GetColumns(col.Name, type);
                                var newType = TypeFacts.GetCommonColumnType(sameTypeColumns, Conversion.None) ?? type;
                                var suggestedName = col.Name + "_" + newType.Name;
                                var newName = uniqueNames.GetOrAddName(suggestedName);
                                var newCol = new ColumnSymbol(newName, newType, originalColumns: sameTypeColumns);
                                newColumns.Add(newCol);
                            }
                        }
                        else if (map.HasMultipleColumns(col.Name, col.Type))
                        {
                            var cols = map.GetColumns(col.Name, col.Type);
                            var newType = TypeFacts.GetCommonColumnType(cols, Conversion.None) ?? col.Type;
                            var newCol = new ColumnSymbol(col.Name, newType, originalColumns: cols);
                            newColumns.Add(GetUniqueColumn(newCol, uniqueNames));
                        }
                        else
                        {
                            newColumns.Add(GetUniqueColumn(col, uniqueNames));
                        }
                    }

                    // we've already handled this name, remove it so we don't try adding it again
                    map.Remove(col.Name);
                }

                // copy new list back to original
                columns.Clear();
                columns.AddRange(newColumns);
            }
            finally
            {
                s_uniqueNameTablePool.ReturnToPool(uniqueNames);
                s_columnListPool.ReturnToPool(newColumns);
            }
        }

        /// <summary>
        /// Converts list of columns to a list of columns with distinct names.
        /// If multiple columns have the same name, but differ in type, the resulting single columns has the type dynamic.
        /// </summary>
        internal static void UnifyColumnsWithSameName(List<ColumnSymbol> columns)
        {
            var newColumns = s_columnListPool.AllocateFromPool();
            try
            {
                var map = new ColumnMap(columns);

                // go through original column order and build out new column list
                for (int i = 0; i < columns.Count; i++)
                {
                    var col = columns[i];

                    if (map.HasColumns(col.Name))
                    {
                        if (map.HasMultipleTypes(col.Name))
                        {
                            var types = map.GetTypes(col.Name);
                            var commonType = TypeFacts.GetCommonScalarType(types) ?? ScalarTypes.Dynamic;
                            var originalCols = map.GetColumns(col.Name).ToList();
                            var newCol = new ColumnSymbol(col.Name, commonType, originalColumns: originalCols);
                            newColumns.Add(newCol);
                        }
                        else if (map.HasMultipleColumns(col.Name, col.Type))
                        {
                            var originalCols = map.GetColumns(col.Name, col.Type);
                            var newCol = new ColumnSymbol(col.Name, col.Type, originalColumns: originalCols);
                            newColumns.Add(newCol);
                        }
                        else
                        {
                            newColumns.Add(col);
                        }
                    }

                    // we've already handled this name, so remove it so we don't add it again
                    map.Remove(col.Name);
                }

                // copy new list back to original
                columns.Clear();
                columns.AddRange(newColumns);
            }
            finally
            {
                s_columnListPool.ReturnToPool(newColumns);
            }
        }

        /// <summary>
        /// Converts a list of columns into a list of unique columns by name.
        /// Columns with the same name will be renamed to include a numeric suffix.
        /// </summary>
        internal static void MakeColumnNamesUnique(List<ColumnSymbol> columns)
        {
            var nameToIndexMap = s_stringToIntMapPool.AllocateFromPool();
            var nameTable = s_uniqueNameTablePool.AllocateFromPool();
            var newColumns = s_columnListPool.AllocateFromPool();
            try
            {
                // associate all unique names with first index where they appear
                // to avoid renamed columns influcing the names of other columns
                // further down the list,
                // so [c, c, c1] will map to [c, c2, c1] not [c, c1, c11]
                for (int index = 0; index < columns.Count; index++)
                {
                    var col = columns[index];
                    if (!nameToIndexMap.ContainsKey(col.Name))
                        nameToIndexMap.Add(col.Name, index);
                }

                nameTable.AddNames(nameToIndexMap.Keys);

                // go through original column order and build out new column list
                for (int index = 0; index < columns.Count; index++)
                {
                    var col = columns[index];
                    if (nameToIndexMap.TryGetValue(col.Name, out var firstIndex)
                        && index != firstIndex)
                    {
                        newColumns.Add(GetUniqueColumn(col, nameTable));
                    }
                    else
                    {
                        newColumns.Add(col);
                    }
                }

                // copy new list back to original
                columns.Clear();
                columns.AddRange(newColumns);
            }
            finally
            {
                s_uniqueNameTablePool.ReturnToPool(nameTable);
                s_columnListPool.ReturnToPool(newColumns);
                s_stringToIntMapPool.ReturnToPool(nameToIndexMap);
            }
        }

        /// <summary>
        /// Gets the columns that appear in both list of columns (by name)
        /// </summary>
        private static void GetCommonColumns(IReadOnlyList<ColumnSymbol> columnsA, IReadOnlyList<ColumnSymbol> columnsB, List<Symbol> result)
        {
            var columns = s_columnListPool.AllocateFromPool();
            try
            {
                GetCommonColumns(columnsA, columnsB, columns);

                foreach (var c in columns)
                {
                    result.Add(c);
                }
            }
            finally
            {
                s_columnListPool.ReturnToPool(columns);
            }
        }

        /// <summary>
        /// Gets the columns that appear in both list of columns (by name)
        /// </summary>
        private static void GetCommonColumns(IReadOnlyList<ColumnSymbol> columnsA, IReadOnlyList<ColumnSymbol> columnsB, List<ColumnSymbol> result)
        {
            var names = s_stringSetPool.AllocateFromPool();
            try
            {
                foreach (var c in columnsB)
                {
                    names.Add(c.Name);
                }

                foreach (var c in columnsA)
                {
                    if (names.Contains(c.Name))
                    {
                        result.Add(c);
                    }
                }

                names.Clear();
                foreach (var c in columnsA)
                {
                    names.Add(c.Name);
                }

                foreach (var c in columnsB)
                {
                    if (names.Contains(c.Name))
                    {
                        result.Add(c);
                    }
                }

                UnifyColumnsWithSameName(result);
            }
            finally
            {
                s_stringSetPool.ReturnToPool(names);
            }
        }

        /// <summary>
        /// Gets the columns that appear in all tables.
        /// </summary>
        internal static void GetCommonColumns(IReadOnlyList<TableSymbol> tables, List<ColumnSymbol> common)
        {
            common.Clear();

            if (tables.Count == 1)
            {
                common.AddRange(tables[0].Columns);
            }
            else if (tables.Count == 2)
            {
                GetCommonColumns(tables[0].Columns, tables[1].Columns, common);
            }
            else if (tables.Count > 2)
            {
                var columnsA = s_columnListPool.AllocateFromPool();
                var columnsC = s_columnListPool.AllocateFromPool();
                try
                {
                    GetCommonColumns(tables[0].Columns, tables[1].Columns, columnsA);

                    for (int i = 2; i < tables.Count; i++)
                    {
                        GetCommonColumns(columnsA, tables[i].Columns, columnsC);

                        if (i < tables.Count - 1)
                        {
                            columnsA.Clear();
                            columnsA.AddRange(columnsC);
                            columnsC.Clear();
                        }
                    }

                    common.AddRange(columnsC);
                }
                finally
                {
                    s_columnListPool.ReturnToPool(columnsA);
                    s_columnListPool.ReturnToPool(columnsC);
                }
            }
        }

        /// <summary>
        /// Gets a column with a unique name (given a set of already used names).
        /// </summary>
        private static ColumnSymbol GetUniqueColumn(ColumnSymbol column, UniqueNameTable uniqueNames)
        {
            var uniqueName = uniqueNames.GetOrAddName(column.Name);
            if (uniqueName != column.Name)
            {
                return new ColumnSymbol(uniqueName, column.Type, originalColumns: new[] { column });
            }
            else
            {
                return column;
            }
        }

        /// <summary>
        /// Creates column symbols for all the columns declared in the schema.
        /// </summary>
        public static void CreateColumnsFromRowSchema(SyntaxList<SeparatedElement<NameAndTypeDeclaration>> schemaColumns, List<ColumnSymbol> columns, List<Diagnostic> diagnostics = null)
        {
            var declaredNames = s_stringSetPool.AllocateFromPool();
            try
            {
                for (int i = 0, n = schemaColumns.Count; i < n; i++)
                {
                    var nat = schemaColumns[i].Element;

                    switch (nat.Type)
                    {
                        case PrimitiveTypeExpression p:
                            var type = GetType(p); // diagnostics should already have been added
                            if (DeclareColumnName(declaredNames, nat.Name.SimpleName, diagnostics, nat.Name.Name))
                            {
                                columns.Add(new ColumnSymbol(nat.Name.SimpleName, type, source: nat.Name));
                            }
                            break;

                        default:
                            diagnostics?.Add(DiagnosticFacts.GetInvalidColumnDeclaration().WithLocation(nat));
                            break;
                    }
                }
            }
            finally
            {
                s_stringSetPool.ReturnToPool(declaredNames);
            }
        }

        /// <summary>
        /// Gets the columns referenced by all expressions
        /// </summary>
        private void GetColumnsInColumnList(SyntaxList<SeparatedElement<Expression>> expressions, List<ColumnSymbol> columns, List<Diagnostic> diagnostics)
        {
            foreach (var elem in expressions)
            {
                GetReferencedColumns(elem.Element, columns, diagnostics);
            }
        }

        /// <summary>
        /// Gets the columns referenced by one expression.
        /// </summary>
        private void GetReferencedColumns(Expression expression, List<ColumnSymbol> columns, List<Diagnostic> diagnostics = null)
        {
            var symbol = GetReferencedSymbol(expression);

            switch (symbol)
            {
                case ColumnSymbol c:
                    columns.Add(c);
                    break;
                case GroupSymbol g:
                    foreach (var m in g.Members)
                    {
                        if (m is ColumnSymbol c)
                        {
                            columns.Add(c);
                        }
                    }
                    break;
                default:
                    diagnostics?.Add(DiagnosticFacts.GetColumnExpected().WithLocation(expression));
                    break;
            }
        }

        /// <summary>
        /// Gets all the columns referenced in the syntax tree.
        /// </summary>
        private void GetReferencedColumnsInTree(SyntaxNode node, List<ColumnSymbol> columns)
        {
            foreach (var nr in node.GetDescendantsOrSelf<NameReference>())
            {
                GetReferencedColumns(nr, columns);
            }
        }
    }
}