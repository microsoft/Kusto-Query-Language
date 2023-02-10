using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Symbols;
    using Utils;

    internal class AvoidUsingObsoleteFunctionsAnalyzer : KustoAnalyzer
    {
        private static readonly Diagnostic _diagnostic =
            new Diagnostic(
                KustoAnalyzerCodes.AvoidUsingObsoleteFunctions,
                DiagnosticCategory.Correctness,
                DiagnosticSeverity.Warning,
                description: "Avoid using obsolete/deprecated functions.");

        protected override IEnumerable<Diagnostic> GetDiagnostics()
        {
            return new[] { _diagnostic };
        }

        public override void Analyze(KustoCode code, List<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            foreach (var node in code.Syntax.GetDescendants<FunctionCallExpression>(
                fc => fc.ReferencedSymbol is FunctionSymbol fs 
                    && (fs.IsObsolete || (fc.ReferencedSignature is Signature sig && sig.IsObsolete))))
            {
                if (node.ReferencedSymbol is FunctionSymbol fs)
                {
                    if (fs.IsObsolete)
                    {
                        diagnostics.Add(
                            _diagnostic.WithMessage($"The function '{fs.Name}' is deprecated; use '{fs.Alternative}' instead.")
                            .WithLocation(node.Name)
                            );
                    }
                    else if (node.ReferencedSignature is Signature sig && sig.IsObsolete)
                    {
                        diagnostics.Add(
                            _diagnostic.WithMessage($"This form of the function '{fs.Name}' is deprecated; use '{sig.Alternative}' instead.")
                            .WithLocation(node.Name)
                            );
                    }
                }
            }
        }

        protected override void GetFixAction(KustoCode code, Diagnostic dx, CodeActionOptions options, List<CodeAction> actions, CancellationToken cancellationToken)
        {
            if (code.Syntax.GetNodeAt(dx.Start, dx.Length) is Name name
                && name.Parent is NameReference nameRef
                && nameRef.Parent is FunctionCallExpression fc
                && fc.ReferencedSymbol is FunctionSymbol fs)
            {
                var altFunction = fs.IsObsolete ? fs.Alternative
                                : fc.ReferencedSignature is Signature sig ? sig.Alternative
                                : null;

                if (altFunction != null 
                    && CanBeRenamed(fc.ReferencedSignature, altFunction, code.Globals))
                {
                    actions.Add(CodeAction.Create(
                        kind: "Obsolete",
                        title: $"Change to '{altFunction}'",
                        description: $"Convert from using deprecated function '{fs.Name}' to preferred function '{altFunction}'",
                        data: new string[] { "rename", dx.Start.ToString(), dx.Length.ToString(), altFunction }));
                }
            }
        }

        protected override FixEdits GetFixEdits(KustoCode code, ApplyAction action, int caretPosition, CodeActionOptions options, CancellationToken cancellationToken)
        {
            if (action.Data[0] == "rename"
                && Int32.TryParse(action.Data[1], out var start)
                && Int32.TryParse(action.Data[2], out var length)
                && action.Data[3] is string altName)
            {
                return new FixEdits(
                    caretPosition,
                    TextEdit.Replacement(start, length, altName));
            }
            else
            {
                return new FixEdits(caretPosition);
            }
        }

        private bool CanBeRenamed(Signature sig, string altName, GlobalState globals)
        {
            switch (sig.Symbol.Name)
            {
                case "row_rank":
                    // signature is similar but not semantically the same
                    return false;

                default:
                    return IsRenameCompatible(sig, altName, globals);
            }
        }

        private bool IsRenameCompatible(Signature sig, string altName, GlobalState globals)
        {
            var altSym = globals.GetFunction(altName)
                      ?? globals.GetAggregate(altName)
                      ?? globals.GetPlugIn(altName);

            if (altSym == null)
                return false;

            // check for any matching signature
            foreach (var altSig in altSym.Signatures)
            {
                if (IsRenameCompatible(sig, altSig))
                    return true;
            }

            return false;
        }

        private static bool IsRenameCompatible(Signature sig1, Signature sig2)
        {
            if (sig1.Parameters.Count != sig2.Parameters.Count)
                return false;

            for (int i = 0; i < sig1.Parameters.Count; i++)
            {
                if (!IsCompatible(sig1.Parameters[i], sig2.Parameters[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsCompatible(Parameter p1, Parameter p2)
        {
            if (p1.TypeKind != p2.TypeKind)
                return false;

            if (p1.TypeKind == ParameterTypeKind.Declared)
            {
                if (p1.DeclaredTypes.Count != p2.DeclaredTypes.Count)
                    return false;
               
                for (int i = 0; i < p1.DeclaredTypes.Count; i++)
                {
                    if (p1.DeclaredTypes[i] != p2.DeclaredTypes[i])
                        return false;
                }
            }

            return true;
        }
    }
}