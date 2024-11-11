using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Symbols
{
    /// <summary>
    /// Generates the debug text for the <see cref="Symbol"/>
    /// </summary>
    internal static class DebugDisplay
    {
        /// <summary>
        /// Returns the debug text for the <see cref="Symbol"/>.
        /// </summary>
        public static string GetText(Symbol symbol)
        {
            switch (symbol)
            {
                case null:
                    return "";
                case ColumnSymbol c:
                    return $"{c.Name}: {GetText(c.Type)}";
                case TableSymbol t:
                    return $"{t.Name}: ({string.Join(", ", t.Columns.Select(m => GetText(m)))})";
                case DatabaseSymbol d:
                    return KustoFacts.GetSingleQuotedStringLiteral(!string.IsNullOrEmpty(d.AlternateName) ? d.AlternateName : d.Name);
                case ClusterSymbol c:
                    return KustoFacts.GetSingleQuotedStringLiteral(c.Name);
                case GroupSymbol g:
                    return "[" + string.Join(", ", g.Members.Select(s => GetText(s))) + "]";
                case FunctionSymbol f:
                    return $"{f.Name} = {GetText(f.Signatures[0])}";
                case ParameterSymbol p:
                    return $"{p.Name}: {GetText(p.Type)}";
                case GraphSymbol g:
                    if (g.NodeShape != null)
                    {
                        return $"{g.Name}: [Edge{GetText(g.EdgeShape)}, Node{GetText(g.NodeShape)}]";
                    }
                    else
                    {
                        return $"{g.Name}: [Edge{GetText(g.EdgeShape)}]";
                    }
                case EntityGroupSymbol e:
                    if (e.Definition != null)
                    {
                        return $"{e.Name}: {e.Definition}";
                    }
                    else
                    {
                        return $"{e.Name}: {string.Join(", ", e.Members.Select(m => GetText(m)))}";
                    }
                case PatternSymbol pat:
                    return $"{pat.Name}: ({string.Join(", ", pat.Parameters.Select(p => GetText(p)))})";
                case VariableSymbol v:
                    return $"{v.Name} = {GetText(v.Type)}";

                case TupleSymbol t:
                    return $"[{string.Join(", ", t.Members.Select(m => GetText(m)))}]";
                case PrimitiveSymbol prv:
                    return prv.Name;
                case DynamicAnySymbol d:
                    return "dynamic";
                case DynamicPrimitiveSymbol d:
                    return $"dynamic({GetText(d.UnderlyingType)})";
                case DynamicArraySymbol d:
                    return d.ElementType == ScalarTypes.Dynamic
                        ? "dynamic([])"
                        : $"dynamic([{GetText(d.ElementType)}])";
                case DynamicBagSymbol d:
                    return d.Properties.Count == 0
                        ? "dynamic({})"
                        : $"dynamic({{{string.Join(", ", d.Properties.Select(p => GetText(p)))}}})";
                default:
                    return symbol.Name;
            }
        }

        /// <summary>
        /// Returns the debug text for the parameter.
        /// </summary>
        public static string GetText(Parameter parameter)
        {
            return parameter.TypeKind == ParameterTypeKind.Declared
                ? $"{parameter.Name}: {string.Join("|", parameter.DeclaredTypes.Select(t => GetText(t)))}"
                : $"{parameter.Name}: <{parameter.TypeKind}>";
        }

        /// <summary>
        /// Returns the debug text for the <see cref="Signature"/>
        /// </summary>
        public static string GetText(Signature sig, bool includeSymbolName = false, bool verbose = false)
        {
            var builder = new StringBuilder();

            for (int i = 0; i < sig.Parameters.Count; i++)
            {
                var p = sig.Parameters[i];

                if (i > 0)
                {
                    builder.Append(", ");
                }

                if (p.IsOptional)
                {
                    // everything after this must be optional too, so just denote the entire section as optional.
                    builder.Append("[");
                    builder.Append(GetText(sig, p, verbose));
                    builder.Append("]");
                }
                else
                {
                    builder.Append(GetText(sig, p, verbose));
                }
            }

            if (sig.HasRepeatableParameters)
            {
                builder.Append(", ...");
            }

            var prms = builder.ToString();

            if (includeSymbolName && !string.IsNullOrEmpty(sig.Symbol.Name))
            {
                return $"{sig.Symbol.Name}({prms})";
            }
            else
            {
                return $"({prms})";
            }
        }

        /// <summary>
        /// Returns the debug text for the signature+parameter combination
        /// </summary>
        private static string GetText(Signature signature, Parameter parameter, bool verbose)
        {
            if (verbose)
            {
                var typeText = GetTypeText(signature, parameter);

                if (!string.IsNullOrEmpty(typeText))
                {
                    return $"{parameter.Name}: {typeText}";
                }
            }

            return parameter.Name;
        }

        private static string GetTypeText(Signature signature, Parameter parameter)
        {
            switch (parameter.TypeKind)
            {
                case ParameterTypeKind.Declared:
                    return string.Join("|", parameter.DeclaredTypes.Select(t => GetText(t)));
                case ParameterTypeKind.Integer:
                    return "integer";
                case ParameterTypeKind.IntegerOrArray:
                    return "integer|array";
                case ParameterTypeKind.DynamicArray:
                    return "array|dynamic";
                case ParameterTypeKind.DynamicBag:
                    return "bag|dynamic";
                case ParameterTypeKind.Number:
                    return "number";
                case ParameterTypeKind.NumberOrBool:
                    return "number|bool";
                case ParameterTypeKind.RealOrDecimal:
                    return "real|decimal";
                case ParameterTypeKind.Summable:
                    return "summable";
                case ParameterTypeKind.StringOrDynamic:
                    return "string|dynamic";
                case ParameterTypeKind.StringOrArray:
                    return "string|array";
                case ParameterTypeKind.Parameter0:
                    return GetTypeText(signature, signature.Parameters[0]);
                case ParameterTypeKind.Parameter1:
                    return GetTypeText(signature, signature.Parameters[1]);
                case ParameterTypeKind.Parameter2:
                    return GetTypeText(signature, signature.Parameters[2]);
                case ParameterTypeKind.Tabular:
                    return "()";
                case ParameterTypeKind.Cluster:
                    return "cluster";
                case ParameterTypeKind.Database:
                    return "database";
                case ParameterTypeKind.Scalar:
                default:
                    return "scalar";
            }
        }
    }
}
