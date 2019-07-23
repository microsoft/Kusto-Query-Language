using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Editor
{
    public class QuickInfo
    {
        public string Text { get; }

        public QuickInfo(string text)
        {
            this.Text = text;
        }


        public static QuickInfo Empty = new QuickInfo(string.Empty);
    }
}