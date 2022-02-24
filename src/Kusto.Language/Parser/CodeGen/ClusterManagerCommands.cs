// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// WARNING: This file is auto generated during build. Do not modify manually.
using System;
using System.Linq;
using System.Collections.Generic;
using Kusto.Language.Symbols;

namespace Kusto.Language
{
    public static class ClusterManagerCommands
    {
        public static readonly CommandSymbol ShowVersion =
            new CommandSymbol(
                "ShowVersion",
                "(BuildVersion: string, BuildTime: datetime, ServiceType: string, ProductVersion: string)");

        public static readonly IReadOnlyList<CommandSymbol> All = new CommandSymbol[]
        {
            ShowVersion
        };
    }
}

