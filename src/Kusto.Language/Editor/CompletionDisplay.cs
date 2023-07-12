using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Editor
{
    using Symbols;

    /// <summary>
    /// Generates the text that is the display text in a <see cref="CompletionItem"/> for a given symbol.
    /// </summary>
    public static class CompletionDisplay
    {
        /// <summary>
        /// Returns the display text for the specified symbol.
        /// </summary>
        public static string GetText(Symbol symbol, bool nameOnly = false)
        {
            switch (symbol)
            {
                case TableSymbol t:
                    if (nameOnly)
                        return t.Name;
                    if (t.IsExternal)
                        return $"external_table('{t.Name}')";
                    return t.Name;

                case FunctionSymbol f:
                    if (nameOnly || f.Signatures.Count == 0)
                        return f.Name;
                    return $"{f.Name}{GetSignatureText(f.Signatures[0])}";

                case PatternSymbol pat:
                    return $"{pat.Name}{GetParametersText(pat.Parameters, false)}";

                case VariableSymbol v:
                    if (v.Type is FunctionSymbol)
                        return GetText(v.Type, nameOnly);
                    return v.Name;

                case ParameterSymbol p:
                    if (p.Type is FunctionSymbol)
                        return GetText(p.Type, nameOnly);
                    return p.Name;

                case DatabaseSymbol d:
                    return d.Name;

                case ClusterSymbol cl:
                    return cl.Name;

                case OptionSymbol opt:
                    return opt.Name;

                default:
                    return symbol.Name;
            }
        }

        private static string GetSignatureText(Signature signature)
        {
            return GetParametersText(signature.Parameters, signature.HasRepeatableParameters);
        }

        private static string GetParametersText(IReadOnlyList<Parameter> parameters, bool hasRepeatableParameters)
        {
            var builder = new StringBuilder();

            for (int i = 0; i < parameters.Count; i++)
            {
                var p = parameters[i];

                if (i > 0)
                {
                    builder.Append(", ");
                }

                if (p.IsOptional)
                {
                    // everything after this must be optional too, so just denote the entire section as optional.
                    builder.Append("[");
                    builder.Append(p.Name);
                    builder.Append("]");
                }
                else
                {
                    builder.Append(p.Name);
                }
            }

            if (hasRepeatableParameters)
            {
                builder.Append(", ...");
            }

            var prms = builder.ToString();

            return $"({prms})";
        }
    }
}
