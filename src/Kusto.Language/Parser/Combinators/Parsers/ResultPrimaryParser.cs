using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    /// <summary>
    /// A parser with output list based parsing implemented over result based parsing.
    /// These parsers should *not* wrap other parsers.
    /// </summary>
    public abstract class ResultPrimaryParser<TInput, TOutput> : Parser<TInput, TOutput>
    {
        public override int Parse(Source<TInput> source, int inputStart, List<object> output, int outputStart)
        {
            var result = this.Parse(source, inputStart);

            if (result.Length >= 0)
            {
                output.Add(result.Value);
            }

            return result.Length;
        }
    }
}