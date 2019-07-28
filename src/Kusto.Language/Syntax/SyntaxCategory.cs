namespace Kusto.Language.Syntax
{
    public enum SyntaxCategory
    {
        None = 0,

        /// <summary>
        /// A language keyword
        /// </summary>
        Keyword,

        /// <summary>
        /// A language identifier (column name, etc)
        /// </summary>
        Identifier,

        /// <summary>
        /// Arbitrary punctuation like (, ), [, ], :, etc
        /// </summary>
        Punctuation,

        /// <summary>
        /// Scalar math operators like +, -, *, /, etc
        /// </summary>
        Operator, 

        /// <summary>
        /// Literal values like 10, 1.5, 'a string'
        /// </summary>
        Literal,

        /// <summary>
        /// Special case for list nodes
        /// </summary>
        List,

        /// <summary>
        /// All other non-terminals
        /// </summary>
        Node,

        /// <summary>
        /// Non-typical syntax nodes: EOF, BadXXX, Directives
        /// </summary>
        Other
    }
}