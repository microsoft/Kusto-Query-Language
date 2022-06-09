using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using System.Linq;

    public static class KustoActors
    {
        public static KustoActor FunctionInliner = new InlineDatabaseFunctionActor();
        public static KustoActor ExtractExpression = new ExtractExpressionActor();

        public static IReadOnlyList<KustoActor> All = new KustoActor[]
        {
            FunctionInliner,
            ExtractExpression
        };
    }
}