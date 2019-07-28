using System;

namespace Kusto.Language.Editor
{
    /// <summary>
    /// The priority that controls the order ranking of completion items
    /// when presented.
    /// </summary>
    public enum CompletionPriority
    {
        Top,
        High,
        Normal,
        Low,
    }
}
