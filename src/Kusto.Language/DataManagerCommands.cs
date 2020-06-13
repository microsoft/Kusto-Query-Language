using System.Collections.Generic;

namespace Kusto.Language
{
    using Symbols;

    public static class DataManagerCommands
    {
        // TODO: update this with actual DM commands
        public static IReadOnlyList<CommandSymbol> All { get; } =
            new CommandSymbol[]
            {
                EngineCommands.ShowVersion
            };
    }
}