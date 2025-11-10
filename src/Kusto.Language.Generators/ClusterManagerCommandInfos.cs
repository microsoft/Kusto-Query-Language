// <#+
#if !T4
using System.Collections.Generic;

namespace Kusto.Language.Generators
{

#endif

    public static class ClusterManagerCommandInfos
    {
        public static readonly CommandInfo ShowVersion =
            new CommandInfo(nameof(ShowVersion),
                "show version",
                "(BuildVersion: string, BuildTime: datetime, ServiceType: string, ProductVersion: string)");
    }

#if !T4
}
#endif
// #>