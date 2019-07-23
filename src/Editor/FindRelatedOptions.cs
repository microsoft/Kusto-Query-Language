using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Editor
{
    [Flags]
    public enum FindRelatedOptions
    {
        /// <summary>
        /// No options.
        /// </summary>
        None = 0,

        /// <summary>
        /// Consider variables that reference other items to be related to those items
        /// </summary>
        SeeThroughVariables = 1
    }
}