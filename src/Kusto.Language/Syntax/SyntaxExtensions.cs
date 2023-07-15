using System;
using System.Linq;

namespace Kusto.Language.Syntax
{
    public static class SyntaxExtensions
    {
        /// <summary>
        /// Gets the first <see cref="NamedParameter"/> that matches the <see cref="QueryOperatorParameter"/> definition
        /// </summary>
        public static NamedParameter GetParameter(this SyntaxList<NamedParameter> list, QueryOperatorParameter parameter) =>
            list.FirstOrDefault(np => np.Name.SimpleName == parameter.Name || (parameter.Aliases.Count > 0 && parameter.Aliases.Contains(np.Name.SimpleName)));

        /// <summary>
        /// Gets literal value of the named parameter, or the default value if the parameter is not in the list or the value is not a literal of the correct type.
        /// </summary>
        public static TValue GetParameterLiteralValue<TValue>(this SyntaxList<NamedParameter> list, QueryOperatorParameter declaration, TValue defaultValue = default(TValue))
        {
            if (list.GetParameter(declaration) is NamedParameter parameter
                && parameter.Expression is LiteralExpression lit
                && lit.ConstantValue is TValue value)
            {
                return value;
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets name value of the query operator parameter, or null if the parameter is not present in the list or does not contains a name as a value.
        /// </summary>
        public static string GetParameterNameValue(this SyntaxList<NamedParameter> list, QueryOperatorParameter declaration)
        {
            if (list.GetParameter(declaration) is NamedParameter parameter)
            {
                switch (parameter.Expression)
                {
                    case NameDeclaration nd:
                        return nd.Name.SimpleName;
                    case NameReference nr:
                        return nr.Name.SimpleName;
                    case LiteralExpression le:
                        if (le.Kind == SyntaxKind.StringLiteralExpression
                            || le.Kind == SyntaxKind.TokenLiteralExpression)
                        {
                            return le.LiteralValueInfo?.ValueText;
                        }
                        break;
                    case CompoundStringLiteralExpression cs:
                        return cs.LiteralValueInfo?.ValueText;
                }
            }

            return null;
        }
    }
}