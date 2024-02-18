using System;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    using Utils;

    /// <summary>
    /// A function that consumes input items by returning the number of input items consumed.
    /// </summary>
    public delegate int SourceConsumer<TInput>(Source<TInput> source, int start);
}