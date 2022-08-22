using System;
using System.Collections.Generic;

namespace Kusto.Language.Binding
{
    using Symbols;
    using System.Linq;
    using Utils;

    internal class ColumnMap
    {
        /// <summary>
        /// map of column names to columns, column lists or dictionaries of type to columns/column lists.
        /// </summary>
        private Dictionary<string, object> _nameMap;

        public ColumnMap()
        {
            _nameMap = new Dictionary<string, object>();
        }

        public ColumnMap(IEnumerable<ColumnSymbol> columns)
            : this()
        {
            AddRange(columns);
        }

        /// <summary>
        /// Adds a list of columns to the map.
        /// </summary>
        public void AddRange(IEnumerable<ColumnSymbol> columns)
        {
            foreach (var col in columns)
            {
                Add(col);
            }
        }

        /// <summary>
        /// Adds a column to the map.
        /// </summary>
        public void Add(ColumnSymbol column)
        {
            if (_nameMap.TryGetValue(column.Name, out var columnListOrDictionary))
            {
                if (columnListOrDictionary is ColumnSymbol c)
                {
                    if (c.Type == column.Type)
                    {
                        var list = new List<ColumnSymbol>();
                        list.Add(c);
                        list.Add(column);
                        _nameMap[column.Name] = list;
                    }
                    else
                    {
                        var dict = new Dictionary<TypeSymbol, object>();
                        dict[c.Type] = c;
                        dict[column.Type] = column;
                        _nameMap[column.Name] = dict;
                    }
                }
                else if (columnListOrDictionary is List<ColumnSymbol> list)
                {
                    if (list[0].Type == column.Type)
                    {
                        list.Add(column);
                    }
                    else
                    {
                        var dict = new Dictionary<TypeSymbol, object>();
                        dict[list[0].Type] = list;
                        dict[column.Type] = column;
                    }
                }
                else if (columnListOrDictionary is Dictionary<TypeSymbol, object> dict)
                {
                    if (dict.TryGetValue(column.Type, out var columnOrList))
                    {
                        if (columnOrList is ColumnSymbol col)
                        {
                            var newList = new List<ColumnSymbol>();
                            newList.Add(col);
                            newList.Add(column);
                            dict[column.Type] = newList;
                        }
                        else if (columnOrList is List<ColumnSymbol> existingColumns)
                        {
                            existingColumns.Add(column);
                        }
                    }
                    else
                    {
                        dict[column.Type] = column;
                    }
                }
            }
            else
            {
                _nameMap.Add(column.Name, column);
            }
        }

        /// <summary>
        /// Removes all the columns with the specified name from the map.
        /// </summary>
        public void Remove(string name)
        {
            _nameMap.Remove(name);
        }

        /// <summary>
        /// Returns true if there is at least one column with the specified name in the map.
        /// </summary>
        public bool HasColumns(string name)
        {
            return _nameMap.ContainsKey(name);
        }

        /// <summary>
        /// Returns true if the columns with the specified name have more than one type.
        /// </summary>
        public bool HasMultipleTypes(string name)
        {
            return _nameMap.TryGetValue(name, out var columnListOrDictionary)
                && columnListOrDictionary is Dictionary<TypeSymbol, object> dict
                && dict.Count > 1;
        }

        /// <summary>
        /// Gets all the types for all the columns with the specified name.
        /// </summary>
        public IReadOnlyList<TypeSymbol> GetTypes(string name)
        {
            if (_nameMap.TryGetValue(name, out var columnListOrDictionary))
            {
                if (columnListOrDictionary is Dictionary<TypeSymbol, object> dict)
                {
                    return dict.Keys.ToReadOnly();
                }
                else if (columnListOrDictionary is List<ColumnSymbol> list)
                {
                    return new[] { list[0].Type };
                }
                else if (columnListOrDictionary is ColumnSymbol col)
                {
                    return new[] { col.Type };
                }
            }

            return EmptyReadOnlyList<TypeSymbol>.Instance;
        }

        /// <summary>
        /// Returns true if there are more than one column with the given name in the map.
        /// </summary>
        public bool HasMutipleColumns(string name)
        {
            if (_nameMap.TryGetValue(name, out var columnListOrDictionary))
            {
                if (columnListOrDictionary is ColumnSymbol col)
                {
                    return false;
                }
                else if (columnListOrDictionary is List<ColumnSymbol> columns)
                {
                    return columns.Count > 1;
                }
                else if (columnListOrDictionary is Dictionary<TypeSymbol, object> dict)
                {
                    if (dict.Count > 1)
                    {
                        return true;
                    }
                    else if (dict.Count == 1)
                    {
                        var columnOrList = dict.First().Value;
                        if (columnOrList is ColumnSymbol)
                        {
                            return true;
                        }
                        else if (columnOrList is List<ColumnSymbol> cols)
                        {
                            return cols.Count > 1;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if there is more than one columns with the specified name and type.
        /// </summary>
        public bool HasMultipleColumns(string name, TypeSymbol type)
        {
            if (_nameMap.TryGetValue(name, out var columnListOrDictionary))
            {
                if (columnListOrDictionary is Dictionary<TypeSymbol, object> dict)
                {
                    return dict.TryGetValue(type, out var columnOrList)
                        && columnOrList is List<ColumnSymbol> cols
                        && cols.Count > 0;
                }
                else if (columnListOrDictionary is List<ColumnSymbol> columns
                    && columns[0].Type == type)
                {
                    return columns.Count > 1;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets all the columns with the specified name.
        /// </summary>
        public IEnumerable<ColumnSymbol> GetColumns(string name)
        {
            if (_nameMap.TryGetValue(name, out var columnListOrDictionary))
            {
                if (columnListOrDictionary is Dictionary<TypeSymbol, object> dict)
                {
                    foreach (var kvp in dict)
                    {
                        if (kvp.Value is List<ColumnSymbol> cols)
                        {
                            foreach (var c in cols)
                            {
                                yield return c;
                            }
                        }
                        else if (kvp.Value is ColumnSymbol col)
                        {
                            yield return col;
                        }
                    }
                }
                else if (columnListOrDictionary is List<ColumnSymbol> columns)
                {
                    foreach (var c in columns)
                    {
                        yield return c;
                    }
                }
                else if (columnListOrDictionary is ColumnSymbol col)
                {
                    yield return col;
                }
            }
        }

        /// <summary>
        /// Gets all the columns with the specified name and type.
        /// </summary>
        public IReadOnlyList<ColumnSymbol> GetColumns(string name, TypeSymbol type)
        {
            if (_nameMap.TryGetValue(name, out var columnListOrDictionary))
            {
                if (columnListOrDictionary is Dictionary<TypeSymbol, object> dict)
                {
                    if (dict.TryGetValue(type, out var columnOrList))
                    {
                        if (columnOrList is List<ColumnSymbol> cols)
                        {
                            return cols;
                        }
                        else if (columnOrList is ColumnSymbol col)
                        {
                            return new[] { col };
                        }
                    }
                }
                else if (columnListOrDictionary is List<ColumnSymbol> columns)
                {
                    return columns;
                }
                else if (columnListOrDictionary is ColumnSymbol col)
                {
                    return new[] { col };
                }
            }

            return EmptyReadOnlyList<ColumnSymbol>.Instance;
        }
    }
}