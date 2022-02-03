using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Binding
{
    /// <summary>
    /// Scope kind.
    /// </summary>
    internal enum ScopeKind
    {
        /// <summary>
        /// Normal lookup in <see cref="Binder"/>
        /// </summary>
        Normal,

        /// <summary>
        /// Only aggregate functions are visible
        /// </summary>
        Aggregate,

        /// <summary>
        /// Only plug-in funtions are visible
        /// </summary>
        PlugIn,

        /// <summary>
        /// Only query options are visible
        /// </summary>
        Option
    }
}