using System;

namespace Kusto.Language.Editor
{
    /// <summary>
    /// The rank that control the ordering of completion items.
    /// </summary>
    public enum CompletionRank
    {
        Literal,
        Aggregate,
        Column,
        Table,
        Entity,  // other entities (not tables or functions)
        Variable,
        Function,
        Keyword,
        StringOperator,
        MathOperator,
        Other,
        Default
    }

    /// <summary>
    /// The priority that controls the ordering of completion items within their rank.
    /// </summary>
    public enum CompletionPriority
    {
        Top,
        High,
        Normal,
        Low,
    }
}
