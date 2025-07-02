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
// 
// WARNING: Do not modify this file
//          This file is auto generated from the template file 'ClusterManagerCommands.tt'
//          Instead modify the corresponding input info file in the Kusto.Language.Generator project.
//          After making changes, use the right-click menu on the .tt file and select 'run custom tool'.

using System;
using System.Linq;
using System.Collections.Generic;
using Kusto.Language.Symbols;

namespace Kusto.Language
{
    public static class ClusterManagerCommands
    {
        public static readonly CommandSymbol ShowVersion =
            new CommandSymbol("ShowVersion", "(BuildVersion: string, BuildTime: datetime, ServiceType: string, ProductVersion: string)");

        public static readonly IReadOnlyList<CommandSymbol> All = new CommandSymbol[]
        {
            ShowVersion
        };
    }
}

