using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Utils;

    public sealed class OutlineInfo
    {
        /// <summary>
        /// The ranges that can be collapsed.
        /// </summary>
        public IReadOnlyList<OutlineRange> Ranges { get; }

        public OutlineInfo(IReadOnlyList<OutlineRange> ranges)
        {
            this.Ranges = ranges ?? EmptyReadOnlyList<OutlineRange>.Instance;
        }

        public static readonly OutlineInfo Empty = new OutlineInfo(null);
    }
}