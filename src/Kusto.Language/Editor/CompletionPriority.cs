using System;

namespace Kusto.Language.Editor
{
    /// <summary>
    /// The rank that control the ordering of completion items.
    /// </summary>
    public enum CompletionRank
    {
        Literal = 0,
        Aggregate = Literal + 1,
        Column = Aggregate + 1,
        Table = Column + 1,
        Variable = Table + 1,
        Function = Variable + 1,
        MaterializedView = Function + 1,
        Keyword = MaterializedView + 1,
        StringOperator = Keyword + 1,
        MathOperator = StringOperator + 1,
        Other = MathOperator + 1,

        Default = Other + 1
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
