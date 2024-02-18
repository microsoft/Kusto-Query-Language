using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    using Utils;

    /// <summary>
    /// A parser with result based parsing implemented over output-list based parsing.
    /// </summary>
    public abstract class ListPrimaryParser<TInput, TOutput> : Parser<TInput, TOutput>
    {
        /// <summary>
        /// Common pool of output lists: don't allocate temporary lists!
        /// </summary>
        private static readonly ObjectPool<List<object>> s_outputListPool =
            new ObjectPool<List<object>>(() => new List<object>(), list => list.Clear(), size: 50);

        public override ParseResult<TOutput> Parse(Source<TInput> source, int start)
        {
            var list = s_outputListPool.AllocateFromPool();
            try
            {
                var n = this.Parse(source, start, list, 0);

                return new ParseResult<TOutput>(n,
                    n >= 0 && list.Count > 0
                        ? (TOutput)list[0]
                        : default(TOutput));
            }
            finally
            {
                s_outputListPool.ReturnToPool(list);
            }
        }
    }
}