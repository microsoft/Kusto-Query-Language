using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    using Utils;

    [Flags]
    public enum SymbolMatch
    {
        None = 0,

        /// <summary>
        /// Any column
        /// </summary>
        Column = 1,

        /// <summary>
        /// Any table (not external)
        /// </summary>
        Table = Column << 1,

        /// <summary>
        /// Any external table
        /// </summary>
        ExternalTable = Table << 1,

        /// <summary>
        /// Any function
        /// </summary>
        Function = ExternalTable << 1,

        /// <summary>
        /// Any local view
        /// </summary>
        View = Function << 1,

        /// <summary>
        /// Any local variable or parameter
        /// </summary>
        Local = View << 1,

        /// <summary>
        /// Any database
        /// </summary>
        Database = Local << 1,

        /// <summary>
        /// Any cluster
        /// </summary>
        Cluster = Database << 1,

        /// <summary>
        /// Any entity group
        /// </summary>
        EntityGroup = Cluster << 1,

        /// <summary>
        /// Any entity group element
        /// </summary>
        EntityGroupElement = EntityGroup << 1,

        /// <summary>
        /// Include scalar items (applies to variables and functions)
        /// </summary>
        Scalar = EntityGroupElement << 1,

        /// <summary>
        /// Include tabular items (applies to variables and functions)
        /// </summary>
        Tabular = Scalar << 1,

        /// <summary>
        /// Include non-scalar items (applies to variable and functions)
        /// </summary>
        NonScalar = Tabular << 1,

        /// <summary>
        /// Any materialized view
        /// </summary>
        MaterializedView = NonScalar << 1,

        /// <summary>
        /// Any query option
        /// </summary>
        Option = MaterializedView << 1,

        /// <summary>
        /// Any graph
        /// </summary>
        Graph = Option << 1,

        /// <summary>
        /// Any stored query result
        /// </summary>
        StoredQueryResult = Graph << 1,

        /// <summary>
        /// Any graph model
        /// </summary>
        GraphModel = StoredQueryResult << 1,

        /// <summary>
        /// Any graph snapshot
        /// </summary>
        GraphSnapshot = GraphModel << 1,

        /// <summary>
        /// Any column, table, function or local, scalar or tabular, database or cluster
        /// </summary>
        Any = Column | Table | Function | View | Local | Database | Cluster | MaterializedView | EntityGroup | EntityGroupElement | Graph | GraphModel | GraphSnapshot,

        /// <summary>
        /// Any column, table, function or local, scalar or tabular
        /// </summary>
        Default = Column | Table | Function | View | Local | MaterializedView | EntityGroup | EntityGroupElement | Graph | ExternalTable,
    }

    public static class SymbolMatchExtensions
    {
        public static bool Matches(this Symbol symbol, string name, SymbolMatch match, bool ignoreCase = false)
        {
            if (name != null)
            {
                if (ignoreCase)
                {
                    if (string.Compare(symbol.Name, name, ignoreCase) != 0)
                        return false;
                }
                else 
                {
                    // compare first character before calling string.Compare (perf)
                    var sn = symbol.Name;
                    if (name.Length == 0 
                        || sn.Length == 0 
                        || name[0] != sn[0]
                        || string.Compare(sn, name) != 0)
                        return false;
                }
            }

            if ((match & SymbolMatch.Column) != 0 && symbol is ColumnSymbol)
                return true;

            if ((match & SymbolMatch.Table) != 0 && symbol is TableSymbol ts && !ts.IsExternal && !ts.IsMaterializedView && !ts.IsStoredQueryResult)
                return true;

            if ((match & SymbolMatch.ExternalTable) != 0 && symbol is TableSymbol ets && ets.IsExternal)
                return true;

            if ((match & SymbolMatch.MaterializedView) != 0 && symbol is TableSymbol mv && mv.IsMaterializedView)
                return true;

            if ((match & SymbolMatch.Database) != 0 && symbol is DatabaseSymbol)
                return true;

            if ((match & SymbolMatch.Cluster) != 0 && symbol is ClusterSymbol)
                return true;

            if ((match & SymbolMatch.EntityGroup) != 0 && IsTypeOrVariable<EntityGroupSymbol>(symbol))
                return true;

            if ((match & SymbolMatch.EntityGroupElement) != 0 && IsTypeOrVariable<EntityGroupElementSymbol>(symbol))
                return true;

            if ((match & SymbolMatch.Graph) != 0 && IsTypeOrVariable<GraphSymbol>(symbol))
                return true;

            if ((match & SymbolMatch.GraphModel) != 0 && symbol is GraphModelSymbol)
                return true;

            if ((match & SymbolMatch.GraphSnapshot) != 0 && symbol is GraphSnapshotSymbol)
                return true;

            if ((match & SymbolMatch.StoredQueryResult) != 0 && IsTypeOrVariable<StoredQueryResultSymbol>(symbol))
                return true;

            // Tabularity Filters

            // if symbol is only allowed to be scalar but it is not 
            if ((match & SymbolMatch.Scalar) != 0 
                && (match & SymbolMatch.NonScalar) == 0  // not claimed to allow non-scalar
                && (match & SymbolMatch.Tabular) == 0    // tabular not allowed
                && !symbol.IsScalar)
                return false;

            // if symbol is not allowed to be scalar but it is
            if ((match & SymbolMatch.NonScalar) != 0 
                && (match & SymbolMatch.Scalar) == 0 // did not also claim it could be scalar
                && symbol.Tabularity == Tabularity.Scalar)
                return false;

            // if symbol is only allowed to be tabular but it is not
            if ((match & SymbolMatch.Tabular) != 0 
                && (match & SymbolMatch.Scalar) == 0 
                && (match & SymbolMatch.NonScalar) == 0
                && !symbol.IsTabular)
                return false;

            // The remaining symbols are affected by tabularity filters above
            if ((match & SymbolMatch.Function) != 0 && (symbol is FunctionSymbol || symbol is PatternSymbol))
                return true;

            if ((match & SymbolMatch.View) != 0 && (symbol is FunctionSymbol fs2 && fs2.IsView))
                return true;

            if ((match & SymbolMatch.Local) != 0 && (symbol is VariableSymbol || symbol is ParameterSymbol))
                return true;

            return false;
        }

        public static bool Matches(this Symbol symbol, SymbolMatch match)
        {
            return Matches(symbol, null, match);
        }

        /// <summary>
        /// True if the symbol matches the type or is a variable with a value that matches the type.
        /// </summary>
        private static bool IsTypeOrVariable<T>(Symbol symbol)
        {
            return symbol is T
                || (symbol is VariableSymbol vs && vs.Type is T);
        }
    }
}