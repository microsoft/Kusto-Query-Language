using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Editor
{
    using Utils;

    /// <summary>
    /// The completion info resulting from a call to <see cref="CodeService.GetCompletionItems"/>
    /// </summary>
    public class CompletionInfo
    {
        /// <summary>
        /// The completion items.
        /// </summary>
        public IReadOnlyList<CompletionItem> Items { get; }

        /// <summary>
        /// The start of the text that each <see cref="CompletionItem"/> will replace. 
        /// </summary>
        public int EditStart { get; }

        /// <summary>
        /// The length of the text that each <see cref="CompletionItem"/> will replace.
        /// </summary>
        public int EditLength { get; }

        public CompletionInfo(
            IEnumerable<CompletionItem> items,
            int editStart,
            int editLength)
        {
            this.Items = items.ToReadOnly();
            this.EditStart = editStart;
            this.EditLength = editLength;
        }

        public static readonly CompletionInfo Empty = new CompletionInfo(null, 0, 0);
    }
}