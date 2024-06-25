using System;

namespace Kusto.Language.Editor
{
    /// <summary>
    /// The rank that control the ordering of completion items.
    /// </summary>
    public enum CompletionRank
    {
        Literal         = 1,
        Aggregate       = 2,
        Column          = 3,
        Table           = 4,
        Entity          = 5,  // other entities not tables or functions
        Variable        = 6,
        Function        = 7,
        Keyword         = 8,
        StringOperator  = 9,
        MathOperator    = 10,
        Other           = 11,
        Default         = 12 // based on completion kind
    }

    /// <summary>
    /// The priority that controls the ordering of completion items within their rank.
    /// </summary>
    public enum CompletionPriority
    {
        Top         = 1,
        High        = 2,
        Normal      = 3,
        Low         = 4,
        Default     = Normal
    }
}
