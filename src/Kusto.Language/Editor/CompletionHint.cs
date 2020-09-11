using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    /// <summary>
    /// A hint that describes the category of syntax element that belongs in
    /// an associated syntax node location, used to determine appropriate
    /// completion items.
    /// </summary>
    [Flags]
    public enum CompletionHint
    {
        /// <summary>
        /// No completion
        /// </summary>
        None = 0,

        /// <summary>
        /// Inherit hint from parent
        /// </summary>
        Inherit = 1,

        /// <summary>
        /// An expression (scalar or tabular)
        /// </summary>
        Expression = Inherit << 1,

        /// <summary>
        /// A scalar expression (non-boolean)
        /// </summary>
        Scalar = Expression << 1,

        /// <summary>
        /// A tabular expression
        /// </summary>
        Tabular = Scalar << 1,

        /// <summary>
        /// A boolean expression
        /// </summary>
        Boolean = Tabular << 1,

        /// <summary>
        /// A numeric expression
        /// </summary>
        Number = Boolean << 1,

        /// <summary>
        /// Literal value
        /// </summary>
        Literal = Number << 1,

        /// <summary>
        /// aggregate expression
        /// </summary>
        Aggregate = Literal << 1,

        /// <summary>
        /// A tabular function
        /// </summary>
        TabularFunction = Aggregate << 1,

        /// <summary>
        /// A scalar function
        /// </summary>
        ScalarFunction = TabularFunction << 1,

        /// <summary>
        /// A database function
        /// </summary>
        DatabaseFunction = ScalarFunction << 1,

        /// <summary>
        /// Any function
        /// </summary>
        Function = DatabaseFunction << 1,

        /// <summary>
        /// A name declaration
        /// </summary>
        Declaration = Function << 1,

        /// <summary>
        /// A column name reference
        /// </summary>
        Column = Declaration << 1,

        /// <summary>
        /// A table name reference
        /// </summary>
        Table = Column << 1,

        /// <summary>
        /// A database expression: database('database')
        /// </summary>
        Database = Table << 1,

        /// <summary>
        /// A cluster expression: cluster('cluster')
        /// </summary>
        Cluster = Database << 1,

        /// <summary>
        /// Syntax completions only
        /// </summary>
        Syntax = Cluster << 1,

        /// <summary>
        /// A query operator
        /// </summary>
        Query = Syntax << 1,

        /// <summary>
        /// A command name
        /// </summary>
        Command = Query << 1,

        /// <summary>
        /// A keyword
        /// </summary>
        Keyword = Command << 1,

        /// <summary>
        /// A clause
        /// </summary>
        Clause = Keyword << 1,

        /// <summary>
        /// A materialized view
        /// </summary>
        MaterializedView = Clause << 1,

        /// <summary>
        /// A query option
        /// </summary>
        Option = MaterializedView << 1,

        /// <summary>
        /// An external table
        /// </summary>
        ExternalTable = Option << 1
    }
}