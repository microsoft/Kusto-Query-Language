using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    internal static class KustoActors
    {
        public static KustoActor FunctionInliner = new InlineDatabaseFunctionActor();
        public static KustoActor ExtractExpression = new ExtractExpressionActor();
        public static KustoActor ConvertToMacroExpand = new ConvertToMacroExpandActor();
        public static KustoActor Rename = new RenameActor();
        public static KustoActor DiagnosticFixer = new DiagnosticFixActor();

        public static IReadOnlyList<KustoActor> All = new KustoActor[]
        {
            FunctionInliner,
            ExtractExpression,
            ConvertToMacroExpand,
            Rename,
            DiagnosticFixer,
        };
    }

    public static class KustoActorNames
    {
        public static string FunctionInliner = KustoActors.FunctionInliner.Name;
        public static string ExtractExpression = KustoActors.ExtractExpression.Name;
        public static string ConvertToMacroExpand = KustoActors.ConvertToMacroExpand.Name;
        public static string DiagnosticFixer = KustoActors.DiagnosticFixer.Name;
        public static string Rename = KustoActors.Rename.Name;
    }
}