using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Kusto.Language.Parsing
{
    using System.Text;
    using Utils;

    /// <summary>
    /// A grammar rule that knows how to parse input and produce output, including values from the left.
    /// </summary>
    public abstract class RightParser<TInput> : Parser<TInput>
    {
    }
}
