using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language
{
    /// <summary>
    /// The kinds of servers that can be connected to.
    /// </summary>
    public static class ServerKinds
    {
        public const string Engine = "Engine";
        public const string DataManager = "DataManager";
        public const string ClusterManager = "ClusterManager";
        public const string AriaBridge = "AriaBridge";
    }
}
