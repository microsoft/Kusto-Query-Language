using System;
using System.Linq;

namespace Kusto.Language.Symbols
{
    /// <summary>
    /// Generates text for symbols that can be parsed as table schema or function parameters.
    /// </summary>
    public static class SchemaDisplay
    {
        /// <summary>
        /// Gets the schema display text for the <see cref="Symbol"/>.
        /// </summary>
        public static string GetText(Symbol symbol)
        {
            switch (symbol)
            {
                case null:
                    return "";
                case ColumnSymbol c:
                    return $"{KustoFacts.BracketNameIfNecessary(c.Name)}: {GetScalarTypeText(c.Type)}";
                case ParameterSymbol p:
                    return $"{KustoFacts.BracketNameIfNecessary(p.Name)}: {GetScalarTypeText(p.Type)}";
                case TableSymbol t:
                    return $"({string.Join(", ", t.Columns.Select(m => GetText(m)))})";
                case FunctionSymbol f:
                    var s = f.Signatures[0];
                    return $"({string.Join(", ", s.Parameters.Select(p => $"{KustoFacts.BracketNameIfNecessary(p.Name)}: {GetParameterTypeText(p)}"))})";
                default:
                    return symbol.Name;
            }
        }

        /// <summary>
        /// Returns the type display text for the scalar type symbol.
        /// </summary>
        public static string GetScalarTypeText(TypeSymbol type)
        {
            switch (type)
            {
                case PrimitiveSymbol p:
                    return p.Name;
                case DynamicSymbol d:
                    return "dynamic";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Returns the type display text for the parameter's type.
        /// </summary>
        public static string GetParameterTypeText(Parameter parameter)
        {
            // this only works for user defined functions
            // since they only have a single declared type per parameter.
            if (parameter.TypeKind == ParameterTypeKind.Declared
                && parameter.DeclaredTypes.Count == 1)
            {
                var type = parameter.DeclaredTypes[0];
                if (type is TableSymbol t
                    && t.Columns.Count == 0)
                {
                    return "(*)";
                }

                return GetText(type);
            }

            // if we get here and don't know the type, then use dynamic
            // since user defined functions auto-convert all arguments.
            return "dynamic";
        }
    }
}
