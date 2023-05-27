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
                    if (nameOnly)
                        return f.Name;
                    return DebugDisplay.GetText(f);

                case PatternSymbol p:
                    return DebugDisplay.GetText(p);

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
    }
}
