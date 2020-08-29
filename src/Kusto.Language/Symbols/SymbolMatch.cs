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
        /// Any local variable or parameter
        /// </summary>
        Local = Function << 1,

        /// <summary>
        /// Any database
        /// </summary>
        Database = Local << 1,

        /// <summary>
        /// Any cluster
        /// </summary>
        Cluster = Database << 1,

        /// <summary>
        /// Any scalar item
        /// </summary>
        Scalar = Cluster << 1,

        /// <summary>
        /// Any tabular item
        /// </summary>
        Tabular = Scalar << 1,

        /// <summary>
        /// Any materialized view
        /// </summary>
        MaterializedView = Tabular << 1,

        /// <summary>
        /// Any query option
        /// </summary>
        Option = MaterializedView << 1,

        /// <summary>
        /// Any column, table, function or local, scalar or tabular, database or cluster
        /// </summary>
        Any = Column | Table | Function | Local | Scalar | Tabular | Database | Cluster | MaterializedView,

        /// <summary>
        /// Any column, table, function or local, scalar or tabular
        /// </summary>
        Default = Column | Table | Function | Local | Scalar | Tabular | MaterializedView,
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

            if ((match & SymbolMatch.Column) != 0 && symbol.Kind == SymbolKind.Column)
                return true;

            if ((match & SymbolMatch.Table) != 0 && symbol is TableSymbol ts && !ts.IsExternal && !ts.IsMaterializedView)
                return true;

            if ((match & SymbolMatch.ExternalTable) != 0 && symbol is TableSymbol ets && ets.IsExternal)
                return true;

            if ((match & SymbolMatch.MaterializedView) != 0 && symbol is TableSymbol mv && mv.IsMaterializedView)
                return true;

            if ((match & SymbolMatch.Database) != 0 && symbol.Kind == SymbolKind.Database)
                return true;

            if ((match & SymbolMatch.Cluster) != 0 && symbol.Kind == SymbolKind.Cluster)
                return true;

            if ((match & SymbolMatch.Scalar) != 0 && (match & SymbolMatch.Tabular) == 0 && !symbol.IsScalar)
                return false;

            if ((match & SymbolMatch.Tabular) != 0 && (match & SymbolMatch.Scalar) == 0 && !symbol.IsTabular)
                return false;

            if ((match & SymbolMatch.Function) != 0 && (symbol.Kind == SymbolKind.Function || symbol.Kind == SymbolKind.Pattern))
                return true;

            if ((match & SymbolMatch.Local) != 0 && (symbol.Kind == SymbolKind.Variable || symbol.Kind == SymbolKind.Parameter))
                return true;

            return false;
        }

        public static bool Matches(this Symbol symbol, SymbolMatch match)
        {
            return Matches(symbol, null, match);
        }
    }
}