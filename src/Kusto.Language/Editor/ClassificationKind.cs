using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Editor
{
    public enum ClassificationKind
    {
        /// <summary>
        /// The text is considered plain text.
        /// </summary>
        PlainText,

        /// <summary>
        /// The text is a comment.
        /// </summary>
        Comment,

        /// <summary>
        /// The text is punctuation: (),;:
        /// </summary>
        Punctuation,

        /// <summary>
        /// The text is a directive:  #
        /// </summary>
        Directive,

        /// <summary>
        /// The text is a non-string literal.
        /// </summary>
        Literal,

        /// <summary>
        /// The text is a string literal.
        /// </summary>
        StringLiteral,

        /// <summary>
        /// The text is a type name.
        /// </summary>
        Type,

        /// <summary>
        /// The text is a column name.
        /// </summary>
        Column,

        /// <summary>
        /// The text is a table name.
        /// </summary>
        Table,

        /// <summary>
        /// The textg is a database name.
        /// </summary>
        Database,

        /// <summary>
        /// The text is a function name.
        /// </summary>
        Function,

        /// <summary>
        /// The text is a parameter name.
        /// </summary>
        Parameter,

        /// <summary>
        /// The text is a variable name.
        /// </summary>
        Variable,

        /// <summary>
        /// The text is an identifier.
        /// </summary>
        Identifier,

        /// <summary>
        /// The text is a client parameter.
        /// </summary>
        ClientParameter,

        /// <summary>
        /// The text is a query parameter. (ie kind=x)
        /// </summary>
        QueryParameter,

        /// <summary>
        /// The text is a scalar operator keyword: has, in, between
        /// </summary>
        ScalarOperator,

        /// <summary>
        /// The text is a math or logic operator: +, -, /, *, ==
        /// </summary>
        MathOperator,

        /// <summary>
        /// The text is a query operator.
        /// </summary>
        QueryOperator,

        /// <summary>
        /// The text is a command keyword
        /// </summary>
        Command,

        /// <summary>
        /// The text is a language keyword
        /// </summary>
        Keyword, 

        /// <summary>
        /// The text is a materialized view entity
        /// </summary>
        MaterializedView,

        /// <summary>
        /// A member of a table/tuple schema declaration
        /// </summary>
        SchemaMember,
    
        /// <summary>
        /// A member of a function signature declaration
        /// </summary>
        SignatureParameter,

        /// <summary>
        /// A query option
        /// </summary>
        Option,
    }
}