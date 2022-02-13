using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using System.Linq;

    public static class KustoActors
    {
        public static KustoActor FunctionInliner = new InlineDatabaseFunctionActor();

        public static IReadOnlyList<KustoActor> All = new KustoActor[]
        {
            FunctionInliner
        };

        private static Dictionary<string, KustoActor> _nameToActorMap;

        public static bool TryGetActor(string name, out KustoActor fixer)
        {
            if (_nameToActorMap == null)
            {
                _nameToActorMap = All.ToDictionary(f => f.Name);
            }

            return _nameToActorMap.TryGetValue(name, out fixer);
        }
    }
}