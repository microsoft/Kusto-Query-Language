using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language
{
    /// <summary>
    /// The set of predefined <see cref="GlobalStateProperty"/>.
    /// </summary>
    public static class Properties
    {
        public static readonly GlobalStateProperty<bool> AllowClientParameters = 
            new GlobalStateProperty<bool>(nameof(AllowClientParameters));
    }
}