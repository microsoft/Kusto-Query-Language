// <#+
#if !T4
namespace Kusto.Language.Generator
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
#endif

    public static class SyntaxNodeInfos
    {
        public static SyntaxNodeInfo[] All = new SyntaxNodeInfo[]
        {
            new SyntaxNodeInfo
            {
                Name = "DirectiveBlock",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "DirectiveBlock",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Directives", Type = "SyntaxList<Directive>", Completion="None"},
                    new SyntaxNodeProperty { Name = "SkippedTokens", Type = "SyntaxList<SyntaxToken>", Completion="None"},
                    new SyntaxNodeProperty { Name = "EndOfText", Type = "SyntaxToken", Optional=true, Completion="None" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "Directive",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "Directive",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Token", Type = "SyntaxToken", Completion="None"},
                }
            },

            new SyntaxNodeInfo
            {
                Name = "SkippedTokens",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "SkippedTokens",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Tokens", Type = "SyntaxList<SyntaxToken>", Completion="None"},
                }
            },

            new SyntaxNodeInfo
            {
                Name = "QueryBlock",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "QueryBlock",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Directives", Type = "SyntaxList<Directive>", Completion="None"}, 
                    new SyntaxNodeProperty { Name = "Statements", Type = "SyntaxList<SeparatedElement<Statement>>", Completion="NonScalar"},
                    new SyntaxNodeProperty { Name = "SkippedTokens", Type = "SkippedTokens", Optional=true, Completion="None"},
                    new SyntaxNodeProperty { Name = "EndOfQuery", Type = "SyntaxToken", Optional=true, Completion="None" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "Expression",
                Doc = "A node in the Kusto syntax that represents an expression",
                Base = "SyntaxNode",
                Abstract = true,
            },

            new SyntaxNodeInfo
            {
                Name = "Clause",
                Doc = "A node in the Kusto syntax that represents a clause",
                Base = "SyntaxNode",
                Abstract = true,
            },

            new SyntaxNodeInfo
            {
                Name = "TypeOfLiteralExpression",
                Doc = "A node in the Kusto syntax that represents a typeof expression",
                Base = "Expression",
                Sealed = true,
                Kind = "TypeOfLiteralExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "TypeOfKeyword", Type = "SyntaxToken", Completion="None" },
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Types", Type = "SyntaxList<SeparatedElement<Expression>>", Completion="Declaration" },
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "QueryOperator",
                Doc = "A node in the Kusto syntax that represents a query operator",
                Base = "Expression",
                Abstract = true
            },

            new SyntaxNodeInfo
            {
                Name = "BadQueryOperator",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "BadQueryOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Keyword", Type = "SyntaxToken" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "Statement",
                Doc = "A node in the Kusto syntax that represents a statement.",
                Base = "SyntaxNode",
                Abstract = true,
            },

            new SyntaxNodeInfo
            {
                Name = "CompoundStringLiteralExpression",
                Doc = "A node in the kusto syntax that name expression.",
                Base = "Expression",
                Sealed = true,
                Kind = "CompoundStringLiteralExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Tokens", Type = "SyntaxList<SyntaxToken>", Doc = "One or more tokens that comprise the string literal value."}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "Name",
                Doc = "A node in the Kusto syntax that represents a name.",
                Base = "SyntaxNode",
                Abstract = true,
            },

            new SyntaxNodeInfo
            {
                Name = "TokenName",
                Doc = "A node in the kusto syntax that represents a single identifier name.",
                Base = "Name",
                Sealed = true,
                Kind = "TokenName",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Name", Type = "SyntaxToken", Doc = "The token that is the name."}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "BracketedName",
                Doc = "A node in the kusto syntax that represents a bracketed name.",
                Base = "Name",
                Sealed = true,
                Kind = "BracketedName",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "OpenBracket", Type = "SyntaxToken", Doc = "The open bracket token.", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "Name", Type = "Expression", Doc = "The string literal expression that comprises the name.", Completion="Literal"},
                    new SyntaxNodeProperty { Name = "CloseBracket", Type = "SyntaxToken", Doc = "The close bracket token.", Completion="Syntax"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "BracedName",
                Doc = "A node in the kusto syntax that represents a client parameter.",
                Base = "Name",
                Sealed = true,
                Kind = "BracedName",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "OpenBrace", Type = "SyntaxToken"},
                    new SyntaxNodeProperty { Name = "Name", Type = "SyntaxToken"},
                    new SyntaxNodeProperty { Name = "CloseBrace", Type = "SyntaxToken"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "WildcardedName",
                Doc = "",
                Base = "Name",
                Sealed = true,
                Kind = "WildcardedName",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Pattern", Type = "SyntaxToken", Completion="Syntax"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "BracketedWildcardedName",
                Doc = "",
                Base = "Name",
                Sealed = true,
                Kind = "BracketedWildcardedName",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "OpenBracket", Type = "SyntaxToken", Doc = "The open bracket token.", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "Pattern", Type = "SyntaxToken", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "CloseBracket", Type = "SyntaxToken", Doc = "The close bracket token.", Completion="Syntax"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "NameDeclaration",
                Doc = "A node in the Kusto syntax that represents a name declaration.",
                Base = "Expression",
                Sealed = true,
                Kind = "NameDeclaration",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Name", Type = "Name", Doc = "The table name"},
                }
            },

            new SyntaxNodeInfo
            {
                Name = "NameReference",
                Doc = "A node in the Kusto syntax that represents a name reference.",
                Base = "Expression",
                Sealed = true,
                Kind = "NameReference",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Name", Type = "Name", Doc = "The table name"},
                    new SyntaxNodeProperty { Name = "Match", Type = "Kusto.Language.Symbols.SymbolMatch", IsSyntax=false }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "LiteralExpression",
                Doc = "A node in the kusto syntax that represents a literal expression.",
                Base = "Expression",
                Sealed = true,
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Token", Type = "SyntaxToken", Doc = "The token with the literal value.", Completion="Literal"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "StarExpression",
                Doc = "A node in the kusto syntax that represents an expression comprised of a single token.",
                Base = "Expression",
                Sealed = true,
                Kind = "StarExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "AsteriskToken", Type = "SyntaxToken", Doc = "The token.", Completion="Syntax"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "AtExpression",
                Doc = "A node in the kusto syntax that represents an expression comprised of a single token.",
                Base = "Expression",
                Sealed = true,
                Kind = "AtExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "AtToken", Type = "SyntaxToken", Doc = "The token.", Completion="Syntax"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "JsonExpression",
                Doc = "A node in the Kusto syntax that represents a json expression",
                Base = "Expression",
                Abstract = true,
            },

            new SyntaxNodeInfo
            {
                Name = "JsonPair",
                Doc = "A node in the kusto syntax that represents a JSON scalar expression.",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "JsonPair",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Name", Type = "SyntaxToken", Doc = "The token with the name of the JSON element.", Completion="Declaration"},
                    new SyntaxNodeProperty { Name = "Colon", Type = "SyntaxToken", Doc = "The colon token.", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "Value", Type = "Expression", Doc = "The value of the JSON element.", Completion="Literal"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "JsonObjectExpression",
                Doc = "A node in the kusto syntax that represents a JSON object expression.",
                Base = "JsonExpression",
                Sealed = true,
                Kind = "JsonObjectExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "OpenBrace", Type = "SyntaxToken", Doc = "The open brace token.", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "Pairs", Type = "SyntaxList<SeparatedElement<JsonPair>>", Doc = "The list of name value pairs.", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "CloseBrace", Type = "SyntaxToken", Doc = "The close brace token.", Completion="Syntax"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "JsonArrayExpression",
                Doc = "A node in the kusto syntax that represents a JSON array expression.",
                Base = "JsonExpression",
                Sealed = true,
                Kind = "JsonArrayExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "OpenBracket", Type = "SyntaxToken", Doc = "The open bracket token.", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "Values", Type = "SyntaxList<SeparatedElement<Expression>>", Doc = "The list of values.", Completion="Literal"},
                    new SyntaxNodeProperty { Name = "CloseBracket", Type = "SyntaxToken", Doc = "The close bracket token.", Completion="Syntax"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "DynamicExpression",
                Doc = "A node in the kusto syntax that represents a dynamic expression.",
                Base = "Expression",
                Sealed = true,
                Kind = "DynamicExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Dynamic", Type = "SyntaxToken", Doc = "The dynamic keyword.", Completion="Keyword"},
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Doc = "The open bracket token.", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Doc = "The body of the dynamic expression.", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Doc = "The close bracket token.", Completion="Syntax"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ParenthesizedExpression",
                Doc = "A node in the kusto syntax that represents a parenthesized expression.",
                Base = "Expression",
                Sealed = true,
                Kind = "ParenthesizedExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Doc = "The open parenthesis token.", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Doc = "The parenthesized expression."},
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Doc = "The close parenthesis token.", Completion="Syntax"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ExpressionList",
                Doc = "A node in the kusto syntax that represents a parenthesized list of expressions.",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "ExpressionList",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Doc = "The open parenthesis token.", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "Expressions", Type = "SyntaxList<SeparatedElement<Expression>>", Doc = "The list of expressions.", Completion="Scalar"},
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Doc = "The close parenthesis token.", Completion="Syntax"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ExpressionCouple",
                Doc = "A node in the kusto syntax that represents a parenthesized pair of expressions.",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "ExpressionCouple",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Doc = "The open parenthesis token.", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "First", Type = "Expression", Doc = "The first expression.", Completion="Scalar"},
                    new SyntaxNodeProperty { Name = "DotDot", Type = "SyntaxToken", Doc = "The .. token.", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "Second", Type = "Expression", Doc = "The second expression.", Completion="Scalar"},
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Doc = "The close parenthesis token.", Completion="Syntax"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "PrefixUnaryExpression",
                Doc = "A node in the kusto syntax that represents a prefix unary expression.",
                Base = "Expression",
                Sealed = true,
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Operator", Type = "SyntaxToken", Doc = "The operator token.", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Doc = "The expression."}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "BinaryExpression",
                Doc = "A node in the kusto syntax that represents a binary expression.",
                Base = "Expression",
                Sealed = true,
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Left", Type = "Expression", Doc = "The left side expression."},
                    new SyntaxNodeProperty { Name = "Operator", Type = "SyntaxToken", Doc = "The operator token.", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "Right", Type = "Expression", Doc = "The right side expression."}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "InExpression",
                Doc = "A node in the kusto syntax that represents an in expression.",
                Base = "Expression",
                Sealed = true,
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Left", Type = "Expression", Doc = "The left side expression."},
                    new SyntaxNodeProperty { Name = "Operator", Type = "SyntaxToken", Doc = "The in or !in keyword.", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "Right", Type = "ExpressionList", Doc = "The list of expressions.", Completion="Syntax"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "HasAnyExpression",
                Doc = "A node in the kusto syntax that represents a has_any expression.",
                Base = "Expression",
                Sealed = true,
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Left", Type = "Expression", Doc = "The left side expression."},
                    new SyntaxNodeProperty { Name = "Operator", Type = "SyntaxToken", Doc = "The has_any keyword.", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "Right", Type = "ExpressionList", Doc = "The list of expressions.", Completion="Syntax"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "HasAllExpression",
                Doc = "A node in the kusto syntax that represents a has_all expression.",
                Base = "Expression",
                Sealed = true,
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Left", Type = "Expression", Doc = "The left side expression."},
                    new SyntaxNodeProperty { Name = "Operator", Type = "SyntaxToken", Doc = "The has_all keyword.", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "Right", Type = "ExpressionList", Doc = "The list of expressions.", Completion="Syntax"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "BetweenExpression",
                Doc = "A node in the kusto syntax that represents a between expression.",
                Base = "Expression",
                Sealed = true,
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Left", Type = "Expression", Doc = "The left side expression."},
                    new SyntaxNodeProperty { Name = "Operator", Type = "SyntaxToken", Doc = "The between or !between keyword.", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "Right", Type = "ExpressionCouple", Doc = "The list of expressions."}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "FunctionCallExpression",
                Doc = "A node in the kusto syntax that represents a function call expression.",
                Base = "Expression",
                Sealed = true,
                Kind = "FunctionCallExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Name", Type = "NameReference", Doc = "The name of the function call.", Completion="Function"},
                    new SyntaxNodeProperty { Name = "ArgumentList", Type = "ExpressionList", Doc = "The arguments", Completion="Syntax"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ToScalarExpression",
                Doc = "A node in the Kusto syntax that represents the toscalar operation.",
                Base = "Expression",
                Sealed = true,
                Kind = "ToScalarExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ToScalar", Type = "SyntaxToken" },
                    new SyntaxNodeProperty { Name = "KindParameter", Type="NamedParameter", Optional=true, Completion="Clause" },
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="NonScalar" },
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion="Syntax"},
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ToTableExpression",
                Doc = "A node in the Kusto syntax that represents the totable operation.",
                Base = "Expression",
                Sealed = true,
                Kind = "ToTableExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ToTable", Type = "SyntaxToken" },
                    new SyntaxNodeProperty { Name = "KindParameter", Type="NamedParameter", Optional=true, Completion="Clause" },
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Scalar" },
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion="Syntax"},
                }
            },

            new SyntaxNodeInfo
            {
                Name = "MaterializedViewCombineExpression",
                Doc = "A node in the Kusto syntax that represents the materilized-view-combine expression.",
                Base = "Expression",
                Sealed = true,
                Kind = "MaterializedViewCombineExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "MaterializedViewCombineKeyword", Type="SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "ViewName", Type="MaterializedViewCombineNameClause", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "BaseClause", Type="MaterializedViewCombineClause", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "DeltaClause", Type="MaterializedViewCombineClause", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "AggregationsClause", Type="MaterializedViewCombineClause", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "MaterializedViewCombineNameClause",
                Doc = "A node in the Kusto syntax that represents the materilized-view-combine view name part.",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "MaterializedViewCombineExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Value", Type = "Expression", Completion="Literal"},
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "MaterializedViewCombineClause",
                Doc = "A node in the Kusto syntax that represents the materilized-view-combine clause.",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "MaterializedViewCombineClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Keyword", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "NamedExpression",
                Doc = "A node in the Kusto syntax that represents a named expression.",
                Base = "Expression",
                Abstract = true
            },

            new SyntaxNodeInfo
            {
                Name = "SimpleNamedExpression",
                Doc = "A node in the kusto syntax that represents a named expression.",
                Base = "NamedExpression",
                Sealed = true,
                Kind = "SimpleNamedExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Name", Type = "NameDeclaration", Doc = "The name of the expression.", Completion="Declaration"},
                    new SyntaxNodeProperty { Name = "EqualToken", Type = "SyntaxToken", Doc = "The equal token.", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Doc = "The named expression."}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "RenameList",
                Doc = "A node in the kusto syntax that represents a parenthesized list of names.",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "RenameList",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Doc = "The open parenthesis token.", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "Names", Type = "SyntaxList<SeparatedElement<NameDeclaration>>", Doc = "The list of name declarations.", Completion="Declaration"},
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Doc = "The close parenthesis token.", Completion="Syntax"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "CompoundNamedExpression",
                Doc = "A node in the kusto syntax that represents a compound named expression.",
                Base = "NamedExpression",
                Sealed = true,
                Kind = "CompoundNamedExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Names", Type = "RenameList", Doc = "The set of names or keywords.", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "EqualToken", Type = "SyntaxToken", Doc = "The equal token.", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Doc = "The named expression.", Completion="Scalar"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "BracketedExpression",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "BracketedExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "OpenBracket", Type = "SyntaxToken", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Literal"},
                    new SyntaxNodeProperty { Name = "CloseBracket", Type = "SyntaxToken", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "PathExpression",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "PathExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression"},
                    new SyntaxNodeProperty { Name = "Dot", Type = "SyntaxToken", Optional = true, Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "Selector", Type = "Expression", Completion="Expression" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ElementExpression",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "ElementExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression"},
                    new SyntaxNodeProperty { Name = "Selector", Type = "Expression", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "PipeExpression",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "PipeExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="NonScalar"},
                    new SyntaxNodeProperty { Name = "Bar", Type = "SyntaxToken", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "Operator", Type = "QueryOperator", Completion="Query"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "RangeOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "RangeOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "RangeToken", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Name", Type = "NameDeclaration", Completion="Declaration" },
                    new SyntaxNodeProperty { Name = "FromToken", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "From", Type = "Expression", Completion="Scalar" },
                    new SyntaxNodeProperty { Name = "ToToken", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "To", Type = "Expression", Completion="Scalar"},
                    new SyntaxNodeProperty { Name = "StepToken", Type = "SyntaxToken", Completion="Keyword"},
                    new SyntaxNodeProperty { Name = "Step", Type = "Expression", Completion="Scalar"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "NamedParameter",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "NamedParameter",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Name", Type = "NameDeclaration", Completion="Declaration" },
                    new SyntaxNodeProperty { Name = "EqualToken", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="None" },
                    new SyntaxNodeProperty { Name = "ExpressionHint", Type="CompletionHint", DefaultValue="CompletionHint.None", IsSyntax=false }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ConsumeOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "ConsumeOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ConsumeKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "CountOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "CountOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "CountKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "AsIdentifier", Type = "CountAsIdentifierClause", Optional=true, Completion="Clause" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "CountAsIdentifierClause",
                Doc = "",
                Base = "Clause",
                Sealed = true,
                Kind = "CountAsIdentifierClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "AsKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Identifier", Type = "SyntaxToken", Completion="Declaration" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ExecuteAndCacheOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "ExecuteAndCacheOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ExecuteAndCacheKeyword", Type = "SyntaxToken" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ExtendOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "ExtendOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ExtendKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Expressions", Type = "SyntaxList<SeparatedElement<Expression>>", Completion="Scalar" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "FacetOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "FacetOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "FacetKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "ByKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Expressions", Type = "SyntaxList<SeparatedElement<Expression>>", Completion="Scalar" },
                    new SyntaxNodeProperty { Name = "WithClause", Type = "FacetWithClause", Optional=true, Completion="Clause"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "FacetWithClause",
                Doc = "",
                Abstract = true,
                Base = "Clause"
            },

            new SyntaxNodeInfo
            {
                Name = "FacetWithOperatorClause",
                Doc = "",
                Base = "FacetWithClause",
                Sealed = true,
                Kind = "FacetWithOperatorClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "WithKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Operator", Type = "QueryOperator", Completion="Query" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "FacetWithExpressionClause",
                Doc = "",
                Base = "FacetWithClause",
                Sealed = true,
                Kind = "FacetWithExpressionClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "WithKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Query" },
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "FilterOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "FilterOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Keyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None" },
                    new SyntaxNodeProperty { Name = "Condition", Type = "Expression", Completion="Boolean" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "GetSchemaOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "GetSchemaOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "GetSchemaKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "KindParameter", Type="NamedParameter", Optional=true, Completion="Clause" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "FindOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "FindOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "FindKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "DataScope", Type = "DataScopeClause", Optional=true, Completion="Clause" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None" },
                    new SyntaxNodeProperty { Name = "InClause", Type = "FindInClause", Optional=true, Completion="Clause" },
                    new SyntaxNodeProperty { Name = "WhereKeyword", Type = "SyntaxToken", Optional=true, Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Condition", Type = "Expression", Completion="Boolean" },
                    new SyntaxNodeProperty { Name = "Project", Type = "FindProjectClause", Optional=true, Completion="Clause" },
                    new SyntaxNodeProperty { Name = "ProjectAway", Type = "FindProjectClause", Optional=true, Completion="Clause" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "DataScopeClause",
                Doc = "",
                Base = "Clause",
                Sealed = true,
                Kind = "DataScopeClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "DataScopeKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "EqualToken", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Value", Type = "SyntaxToken", Completion="Keyword"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "TypedColumnReference",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "TypedColumnReference",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Column", Type = "NameReference", Completion="Column" },
                    new SyntaxNodeProperty { Name = "ColonToken", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Type", Type = "TypeExpression", Completion="Syntax" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "FindInClause",
                Doc = "",
                Base = "Clause",
                Sealed = true,
                Kind = "FindInClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "InKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Expressions", Type = "SyntaxList<SeparatedElement<Expression>>", Completion="NonScalar"},
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "FindProjectClause",
                Doc = "",
                Base = "Clause",
                Sealed = true,
                Kind = "FindProjectClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ProjectKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Columns", Type = "SyntaxList<SeparatedElement<Expression>>", Completion="Scalar"},
                }
            },

            new SyntaxNodeInfo
            {
                Name = "PackExpression",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "PackExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "PackKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "AsteriskToken", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "NameAndTypeDeclaration",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "NameAndTypeDeclaration",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Name", Type = "NameDeclaration", Completion="Declaration" },
                    new SyntaxNodeProperty { Name = "Colon", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Type", Type = "TypeExpression", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "TypeExpression",
                Doc = "",
                Base = "Expression",
                Abstract = true
            },

            new SyntaxNodeInfo
            {
                Name = "PrimitiveTypeExpression",
                Doc = "",
                Base = "TypeExpression",
                Sealed = true,
                Kind = "PrimitiveTypeExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Type", Type = "SyntaxToken", Completion="Keyword" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "SearchOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "SearchOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "SearchKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None" },
                    new SyntaxNodeProperty { Name = "DataScope", Type = "DataScopeClause", Optional=true, Completion="Clause" },
                    new SyntaxNodeProperty { Name = "InClause", Type = "FindInClause", Optional=true, Completion="Clause" },
                    new SyntaxNodeProperty { Name = "Condition", Type = "Expression", Completion="Scalar" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ForkOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "ForkOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ForkKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Expressions", Type = "SyntaxList<ForkExpression>", Completion="NonScalar" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "NameEqualsClause",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "NameEqualsClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Name", Type = "NameDeclaration", Completion="Declaration" },
                    new SyntaxNodeProperty { Name = "EqualToken", Type = "SyntaxToken", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ForkExpression",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "ForkExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "NameEquals", Type = "NameEqualsClause", Optional=true, Completion="Clause" },
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Clause" },
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "MakeSeriesOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "MakeSeriesOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "MakeSeriesKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None" },
                    new SyntaxNodeProperty { Name = "Aggregates", Type = "SyntaxList<SeparatedElement<MakeSeriesExpression>>", Completion="Aggregate" },
                    new SyntaxNodeProperty { Name = "OnClause", Type = "MakeSeriesOnClause", Completion="Clause" },
                    new SyntaxNodeProperty { Name = "RangeClause", Type = "MakeSeriesRangeClause", Completion="Clause"},
                    new SyntaxNodeProperty { Name = "ByClause", Type = "MakeSeriesByClause", Optional=true, Completion="Clause" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "MakeSeriesExpression",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "MakeSeriesExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Scalar" },
                    new SyntaxNodeProperty { Name = "DefaultExpression", Type = "DefaultExpressionClause", Optional=true, Completion="Clause" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "DefaultExpressionClause",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "DefaultExpressionClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "DefaultKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "EqualToken", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Literal" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "MakeSeriesOnClause",
                Doc = "",
                Base = "Clause",
                Sealed = true,
                Kind = "MakeSeriesOnClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "OnKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Scalar" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "MakeSeriesFromClause",
                Doc = "",
                Base = "Clause",
                Sealed = true,
                Kind = "MakeSeriesFromClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "FromKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Scalar" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "MakeSeriesToClause",
                Doc = "",
                Base = "Clause",
                Sealed = true,
                Kind = "MakeSeriesToClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ToKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Scalar" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "MakeSeriesStepClause",
                Doc = "",
                Base = "Clause",
                Sealed = true,
                Kind = "MakeSeriesStepClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "StepKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Scalar" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "MakeSeriesRangeClause",
                Doc = "",
                Base = "Clause",
                Abstract = true
            },

            new SyntaxNodeInfo
            {
                Name = "MakeSeriesInRangeClause",
                Doc = "",
                Base = "MakeSeriesRangeClause",
                Sealed = true,
                Kind = "MakeSeriesInRangeClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "InKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "RangeKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Arguments", Type = "ExpressionList", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "MakeSeriesFromToStepClause",
                Doc = "",
                Base = "MakeSeriesRangeClause",
                Sealed = true,
                Kind = "MakeSeriesFromToStepClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "MakeSeriesFromClause", Type = "MakeSeriesFromClause", Optional=true, Completion="Clause"},
                    new SyntaxNodeProperty { Name = "MakeSeriesToClause", Type = "MakeSeriesToClause", Optional=true, Completion="Clause"},
                    new SyntaxNodeProperty { Name = "MakeSeriesStepClause", Type = "MakeSeriesStepClause", Completion="Clause"}
                }
            },


            new SyntaxNodeInfo
            {
                Name = "MakeSeriesByClause",
                Doc = "",
                Base = "Clause",
                Sealed = true,
                Kind = "MakeSeriesByClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ByKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Expressions", Type = "SyntaxList<SeparatedElement<Expression>>", Completion="Scalar" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "MvExpandOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "MvExpandOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "MvExpandKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None" },
                    new SyntaxNodeProperty { Name = "Expressions", Type = "SyntaxList<SeparatedElement<MvExpandExpression>>", Completion="Scalar"},
                    new SyntaxNodeProperty { Name = "RowLimitClause", Type = "MvExpandRowLimitClause", Optional=true, Completion="Clause"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "MvExpandExpression",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "MvExpandExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Optional=false, Completion="Scalar" },
                    new SyntaxNodeProperty { Name = "ToTypeOf", Type = "ToTypeOfClause", Optional=true, Completion="Clause" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "MvExpandRowLimitClause",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "MvExpandRowLimitClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "LimitKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "RowLimit", Type = "Expression", Completion="Scalar" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "MvApplyOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "MvApplyOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "MvApplyKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None" },
                    new SyntaxNodeProperty { Name = "Expressions", Type = "SyntaxList<SeparatedElement<MvApplyExpression>>", Completion="Scalar"},
                    new SyntaxNodeProperty { Name = "RowLimitClause", Type = "MvApplyRowLimitClause", Optional=true, Completion="Clause"},
                    new SyntaxNodeProperty { Name = "ContextIdClause", Type = "MvApplyContextIdClause", Optional=true, Completion="Clause"},
                    new SyntaxNodeProperty { Name = "OnKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Subquery", Type="MvApplySubqueryExpression", Completion="Clause" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "MvApplyExpression",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "MvApplyExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Optional=true, Completion="Scalar" },
                    new SyntaxNodeProperty { Name = "ToTypeOf", Type = "ToTypeOfClause", Optional=true, Completion="Clause" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "MvApplyRowLimitClause",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "MvApplyRowLimitClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "LimitKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "RowLimit", Type = "Expression", Completion="Scalar" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "MvApplyContextIdClause",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "MvApplyContextIdClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "IdKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Id", Type = "Expression", Completion="Scalar" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "MvApplySubqueryExpression",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "MvApplySubqueryExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Query" },
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ToTypeOfClause",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "ToTypeOfClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ToKeyword", Type = "SyntaxToken", Completion="Keyword"},
                    new SyntaxNodeProperty { Name = "TypeOf", Type = "TypeOfLiteralExpression", Completion="Keyword"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "EvaluateSchemaClause",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "EvaluateSchemaClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ColonToken", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Schema", Type = "EvaluateRowSchema", Optional=true, Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "EvaluateOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "EvaluateOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "EvaluateKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None"},
                    new SyntaxNodeProperty { Name = "FunctionCall", Type = "FunctionCallExpression", Completion="Function"},
                    new SyntaxNodeProperty { Name = "Schema", Type = "EvaluateSchemaClause", Optional=true, Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ParseOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "ParseOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ParseKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None"},
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Scalar"},
                    new SyntaxNodeProperty { Name = "WithKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Patterns", Type = "SyntaxList<SyntaxNode>", Completion="Syntax"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ParseWhereOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "ParseWhereOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ParseKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None"},
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Scalar"},
                    new SyntaxNodeProperty { Name = "WithKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Patterns", Type = "SyntaxList<SyntaxNode>", Completion="Syntax"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ParseKvWithClause",
                Doc = "",
                Base = "Clause",
                Sealed = true,
                Kind = "ParseKvWithClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "WithKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Properties", Type = "SyntaxList<SeparatedElement<NamedParameter>>", Completion="None"},
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ParseKvOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "ParseKvOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ParseKvKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Scalar"},
                    new SyntaxNodeProperty { Name = "AsKeyword", Type = "SyntaxToken", Completion="Keyword"},
                    new SyntaxNodeProperty { Name = "Keys", Type = "RowSchema", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "WithClause", Type = "ParseKvWithClause", Optional=true, Completion="Clause" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "PartitionOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "PartitionOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "PartitionKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None" },
                    new SyntaxNodeProperty { Name = "ByKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "ByExpression", Type = "Expression", Completion="Scalar" },
                    new SyntaxNodeProperty { Name = "Scope", Type = "PartitionScope", Completion="Syntax", Optional=true },
                    new SyntaxNodeProperty { Name = "Operand", Type = "PartitionOperand", Completion="NonScalar" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "PartitionOperand",
                Base = "Expression",
                Abstract = true
            },

            new SyntaxNodeInfo
            {
                Name = "PartitionQuery",
                Base = "PartitionOperand",
                Sealed = true,
                Kind = "PartitionQuery",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "OpenBrace", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Query", Type = "Expression", Completion="NonScalar" },
                    new SyntaxNodeProperty { Name = "CloseBrace", Type = "SyntaxToken", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "PartitionScope",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "PartitionScope",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "InKeyword", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Scalar" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "PartitionSubquery",
                Base = "PartitionOperand",
                Sealed = true,
                Kind = "PartitionSubquery",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Subquery", Type = "Expression", Completion="Query" },
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ProjectOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "ProjectOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ProjectKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Expressions", Type = "SyntaxList<SeparatedElement<Expression>>", Completion="Scalar"},
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ProjectAwayOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "ProjectAwayOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ProjectAwayKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Expressions", Type = "SyntaxList<SeparatedElement<Expression>>", Completion="Scalar"},
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ProjectByNamesOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "ProjectByNamesOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ProjectByNamesKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Expressions", Type = "SyntaxList<SeparatedElement<Expression>>", Completion="Scalar"},
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ProjectKeepOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "ProjectKeepOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ProjectKeepKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Expressions", Type = "SyntaxList<SeparatedElement<Expression>>", Completion="Scalar"},
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ProjectRenameOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "ProjectRenameOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ProjectRenameKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Expressions", Type = "SyntaxList<SeparatedElement<Expression>>", Completion="Scalar"},
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ProjectReorderOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "ProjectReorderOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ProjectReorderKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Expressions", Type = "SyntaxList<SeparatedElement<Expression>>", Completion="Scalar"},
                }
            },

            new SyntaxNodeInfo
            {
                Name = "SampleOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "SampleOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "SampleKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None"},
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Number"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "SampleDistinctOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "SampleDistinctOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "SampleDistinctKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None"},
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Number" },
                    new SyntaxNodeProperty { Name = "OfKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "OfExpression", Type = "Expression", Completion="Scalar"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "EntityGroup",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "EntityGroupExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "EntityGroupKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "OpenBracket", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Entities", Type = "SyntaxList<SeparatedElement<Expression>>"},
                    new SyntaxNodeProperty { Name = "CloseBracket", Type = "SyntaxToken", Completion="Syntax" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ReduceByOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "ReduceByOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ReduceKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None"},
                    new SyntaxNodeProperty { Name = "ByKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Scalar" },
                    new SyntaxNodeProperty { Name = "With", Type = "ReduceByWithClause", Optional=true, Completion="Clause"},
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ReduceByWithClause",
                Doc = "",
                Base = "Clause",
                Sealed = true,
                Kind = "ReduceByWithClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "WithKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<SeparatedElement<NamedParameter>>", Completion="Syntax"},
                }
            },

            new SyntaxNodeInfo
            {
                Name = "SummarizeOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "SummarizeOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "SummarizeKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None"},
                    new SyntaxNodeProperty { Name = "Aggregates", Type = "SyntaxList<SeparatedElement<Expression>>", Completion="Aggregate" },
                    new SyntaxNodeProperty { Name = "ByClause", Type = "SummarizeByClause", Optional=true, Completion="Clause" }
                }
            },

             new SyntaxNodeInfo
            {
                Name = "MacroExpandScopeReferenceName",
                Doc = "",
                Base = "Clause",
                Sealed = true,
                Kind = "MacroExpandScopeReferenceName",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "AsKeyword", Type = "SyntaxToken", Completion="Keyword"},
                    new SyntaxNodeProperty { Name = "EntityGroupReferenceName", Type = "NameDeclaration", Completion="EntityGroup"},
                }
            },

            new SyntaxNodeInfo
            {
                Name = "MacroExpandOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "MacroExpandOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "MacroExpandKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None"},
                    new SyntaxNodeProperty { Name = "EntityGroup", Type = "Expression", Completion="EntityGroup"},
                    new SyntaxNodeProperty { Name = "ScopeReferenceName", Type = "MacroExpandScopeReferenceName", Optional=true, Completion="None"},
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion="Keyword"},
                    new SyntaxNodeProperty { Name = "StatementList", Type = "SyntaxList<SeparatedElement<Statement>>", Completion="NonScalar"},
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion="Keyword"},
                }
            },

            new SyntaxNodeInfo
            {
                Name = "SummarizeByClause",
                Doc = "",
                Base = "Clause",
                Sealed = true,
                Kind = "SummarizeByClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ByKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Expressions", Type = "SyntaxList<SeparatedElement<Expression>>", Completion="Scalar"},
                    new SyntaxNodeProperty { Name = "BinClause", Type = "NamedExpression", Optional=true, Completion="None" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "DistinctOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "DistinctOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "DistinctKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None"},
                    new SyntaxNodeProperty { Name = "Expressions", Type = "SyntaxList<SeparatedElement<Expression>>", Completion="Scalar" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "TakeOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "TakeOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Keyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Number" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "SortOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "SortOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Keyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None" },
                    new SyntaxNodeProperty { Name = "ByKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Expressions", Type = "SyntaxList<SeparatedElement<Expression>>", Completion="Scalar" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "OrderedExpression",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "OrderedExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Scalar" },
                    new SyntaxNodeProperty { Name = "Ordering", Type = "OrderingClause", Completion="Clause" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "OrderingClause",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "OrderingClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "AscOrDescKeyword", Type = "SyntaxToken", Optional=true, Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "NullsClause", Type = "OrderingNullsClause", Optional=true, Completion="Clause" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "OrderingNullsClause",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "OrderingNullsClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "NullsKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "FirstOrLastKeyword", Type = "SyntaxToken", Completion="Keyword" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "TopHittersOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "TopHittersOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "TopHittersKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Number" },
                    new SyntaxNodeProperty { Name = "OfKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "OfExpression", Type = "Expression", Completion="Scalar" },
                    new SyntaxNodeProperty { Name = "ByClause", Type = "TopHittersByClause", Optional=true, Completion="Clause" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "TopHittersByClause",
                Doc = "",
                Base = "Clause",
                Sealed = true,
                Kind = "TopHittersByClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ByKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Scalar" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "TopOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "TopOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "TopKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Number" },
                    new SyntaxNodeProperty { Name = "ByKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "ByExpression", Type = "Expression", Completion="Scalar" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "TopNestedOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "TopNestedOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Clauses", Type = "SyntaxList<SeparatedElement<TopNestedClause>>", Completion="Clause" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "TopNestedClause",
                Doc = "",
                Base = "Clause",
                Sealed = true,
                Kind = "TopNestedClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "TopNestedKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Optional=true, Completion="Number" },
                    new SyntaxNodeProperty { Name = "OfKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "OfExpression", Type = "Expression", Completion="Scalar" },
                    new SyntaxNodeProperty { Name = "WithOthersClause", Type = "TopNestedWithOthersClause", Optional=true, Completion="Clause" },
                    new SyntaxNodeProperty { Name = "ByKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "ByExpression", Type = "Expression", Completion="Scalar" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "TopNestedWithOthersClause",
                Doc = "",
                Base = "Clause",
                Sealed = true,
                Kind = "TopNestedWithOthersClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "WithKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "OthersKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Equal", Type="SyntaxToken", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Scalar" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "UnionOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "UnionOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "UnionKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None" },
                    new SyntaxNodeProperty { Name = "Expressions", Type = "SyntaxList<SeparatedElement<Expression>>", Completion="NonScalar" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "AsOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "AsOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "AsKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None" },
                    new SyntaxNodeProperty { Name = "Name", Type = "NameDeclaration", Completion="Declaration" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "SerializeOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "SerializeOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "SerializeKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None" },
                    new SyntaxNodeProperty { Name = "Expressions", Type = "SyntaxList<SeparatedElement<Expression>>", Completion="Scalar" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "InvokeOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "InvokeOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "InvokeKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Function", Type = "Expression", Completion="TabularFunction" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "RenderOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "RenderOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "RenderKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "ChartType", Type = "SyntaxToken", Optional=true, Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None" },
                    new SyntaxNodeProperty { Name = "WithClause", Type = "RenderWithClause", Optional=true, Completion="Clause" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "MakeGraphOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "MakeGraphOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name="MakeGraphKeyword", Type="SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name="Parameters", Type="SyntaxList<NamedParameter>", Completion="Syntax" },
                    new SyntaxNodeProperty { Name="SourceColumn", Type="NameReference", Completion="Column" },
                    new SyntaxNodeProperty { Name="DirectionToken", Type="SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name="TargetColumn", Type="NameReference", Completion="Column" },
                    new SyntaxNodeProperty { Name="WithClause", Type="MakeGraphWithClause", Optional=true, Completion="Clause" },
                    new SyntaxNodeProperty { Name="PartitionedByClause", Type="MakeGraphPartitionedByClause", Optional=true, Completion="Clause" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "MakeGraphWithClause",
                Doc = "",
                Base = "SyntaxNode",
                Kind = "MakeGraphWithClause",
                Abstract = true,
            },

            new SyntaxNodeInfo
            {
                Name = "MakeGraphWithTablesAndKeysClause",
                Doc = "",
                Base = "MakeGraphWithClause",
                Sealed = true,
                Kind = "MakeGraphWithTablesAndKeysClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name="WithKeyword", Type="SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name="TablesAndKeys", Type="SyntaxList<SeparatedElement<MakeGraphTableAndKeyClause>>", Completion="NonScalar" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "MakeGraphWithImplicitIdClause",
                Doc = "",
                Base = "MakeGraphWithClause",
                Sealed = true,
                Kind = "MakeGraphWithImplicitIdClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name="WithNodeIdKeyword", Type="SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "EqualToken", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Name", Type = "NameDeclaration", Completion="Declaration" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "GraphMarkComponentsOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "GraphMarkComponentsOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name="GraphMarkComponentsKeyword", Type="SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "GraphWhereNodesOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "GraphWhereNodesOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name="GraphWhereNodesKeyword", Type="SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name="Condition", Type="Expression", Completion="Boolean" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "GraphWhereEdgesOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "GraphWhereEdgesOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name="GraphWhereEdgesKeyword", Type="SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name="Condition", Type="Expression", Completion="Boolean" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "MakeGraphTableAndKeyClause",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "MakeGraphTableAndKeyClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name="Table", Type="Expression", Completion="NonScalar" },
                    new SyntaxNodeProperty { Name="OnKeyword", Type="SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name="Column", Type="NameReference", Completion="Column" }
                }
            },

            new SyntaxNodeInfo
            { 
                Name = "MakeGraphPartitionedByClause",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "MakeGraphPartitionedByClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name="PartitionedByKeyword", Type="SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Entity", Type = "NameReference", Completion="Column" },
                    new SyntaxNodeProperty { Name = "OpenParen", Type="SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Subquery", Type="Expression", Completion="Query" },
                    new SyntaxNodeProperty { Name = "CloseParen", Type="SyntaxToken", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "GraphToTableOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "GraphToTableOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name="GraphToTableKeyword", Type="SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name="OutputClause", Type="SyntaxList<SeparatedElement<GraphToTableOutputClause>>", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "GraphToTableOutputClause",
                Doc = "",
                Base = "Clause",
                Sealed = true,
                Kind = "GraphToTableOutputClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name="EntityKeyword", Type="SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name="AsClause", Type = "GraphToTableAsClause", Optional=true, Completion="Syntax" },
                    new SyntaxNodeProperty { Name="Parameters", Type="SyntaxList<NamedParameter>", Optional=true, Completion="None" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "GraphToTableAsClause",
                Doc = "",
                Base = "Clause",
                Sealed = true,
                Kind = "GraphToTableAsClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "AsKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Name", Type = "NameDeclaration", Completion="Declaration" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "GraphMatchOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "GraphMatchOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name="GraphMatchKeyword", Type="SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name="Parameters", Type = "SyntaxList<NamedParameter>", Completion="None"},
                    new SyntaxNodeProperty { Name="Patterns", Type="SyntaxList<SeparatedElement<GraphMatchPattern>>", Completion="Syntax" },
                    new SyntaxNodeProperty { Name="WhereClause", Type="WhereClause", Optional=true, Completion="Syntax" },
                    new SyntaxNodeProperty { Name="ProjectClause", Type="ProjectClause", Optional=true, Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "GraphShortestPathsOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "GraphShortestPathsOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name="GraphShortestPathsKeyword", Type="SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name="Parameters", Type = "SyntaxList<NamedParameter>", Completion="None"},
                    new SyntaxNodeProperty { Name="Patterns", Type="SyntaxList<SeparatedElement<GraphMatchPattern>>", Completion="Syntax" },
                    new SyntaxNodeProperty { Name="WhereClause", Type="WhereClause", Optional=true, Completion="Syntax" },
                    new SyntaxNodeProperty { Name="ProjectClause", Type="ProjectClause", Optional=true, Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "GraphMatchPattern",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "GraphMatchPattern",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name="PatternElements", Type="SyntaxList<GraphMatchPatternNotation>", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "GraphMatchPatternNotation",
                Doc = "",
                Base = "SyntaxNode",
                Abstract = true,
            },

            new SyntaxNodeInfo
            {
                Name = "GraphMatchPatternNode",
                Doc = "",
                Base = "GraphMatchPatternNotation",
                Sealed = true,
                Kind = "GraphMatchPatternNode",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name="Open", Type="SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name="Name", Type="NameDeclaration", Optional=true, Completion="None" },
                    new SyntaxNodeProperty { Name="Close", Type="SyntaxToken", Completion="Keyword" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "GraphMatchPatternEdge",
                Doc = "",
                Base = "GraphMatchPatternNotation",
                Sealed = true,
                Kind = "GraphMatchPatternEdge",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name="FirstToken", Type="SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name="Name", Type="NameDeclaration", Optional=true, Completion="None"},
                    new SyntaxNodeProperty { Name="Range", Type="GraphMatchPatternEdgeRange", Optional=true, Completion="None"},
                    new SyntaxNodeProperty { Name="LastToken", Type="SyntaxToken", Optional=true, Completion="Syntax" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "GraphMatchPatternEdgeRange",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "GraphMatchPatternEdgeRange",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name="Asterisk", Type="SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name="RangeStart", Type="Expression", Completion="Scalar"},
                    new SyntaxNodeProperty { Name="DotDotToken", Type="SyntaxToken", Completion="Syntax"},
                    new SyntaxNodeProperty { Name="RangeEnd", Type="Expression", Completion="Scalar" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "WhereClause",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "WhereClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name="WhereKeyword", Type="SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name="Condition", Type="Expression", Completion="Boolean" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ProjectClause",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "ProjectClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name="ProjectKeyword", Type="SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name="Expressions", Type="SyntaxList<SeparatedElement<Expression>>", Completion="Scalar" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "NameReferenceList",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "NameReferenceList",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Names", Type = "SyntaxList<SeparatedElement<NameReference>>" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "RenderWithClause",
                Doc = "",
                Base = "Clause",
                Sealed = true,
                Kind = "RenderWithClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "WithKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "LeadingComma", Type = "SyntaxToken", Optional=true, Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Properties", Type = "SyntaxList<SeparatedElement<NamedParameter>>", Completion="None"},
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "PrintOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "PrintOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "PrintKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Expressions", Type = "SyntaxList<SeparatedElement<Expression>>", Completion="Scalar" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "AliasStatement",
                Doc = "",
                Base = "Statement",
                Sealed = true,
                Kind = "AliasStatement",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "AliasKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "DatabaseKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Name", Type="NameDeclaration", Completion="Declaration" },
                    new SyntaxNodeProperty { Name = "Equal", Type="SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Expression", Type="Expression", Completion="Database" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "LetStatement",
                Doc = "",
                Base = "Statement",
                Sealed = true,
                Kind = "LetStatement",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "LetKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Name", Type = "NameDeclaration", Completion="Declaration" },
                    new SyntaxNodeProperty { Name = "Equal", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Expression" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "FunctionDeclaration",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "FunctionDeclaration",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ViewKeyword", Type = "SyntaxToken", Optional=true, Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "FunctionParameters", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Body", Type = "FunctionBody", Completion="Clause" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "FunctionParameters",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "FunctionParameters",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<SeparatedElement<FunctionParameter>>", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion="Syntax" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "FunctionParameter",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "FunctionParameter",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "NameAndType", Type = "NameAndTypeDeclaration" },
                    new SyntaxNodeProperty { Name = "DefaultValue", Type = "DefaultValueDeclaration", Optional=true, Completion="Syntax" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "DefaultValueDeclaration",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "DefaultValueDeclaration",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Equal", Type = "SyntaxToken" },
                    new SyntaxNodeProperty { Name = "Value", Type = "Expression", Completion="Scalar" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "FunctionBody",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "FunctionBody",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "OpenBrace", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Statements", Type = "SyntaxList<SeparatedElement<Statement>>", Completion="NonScalar" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Optional=true, Completion="Expression" },
                    new SyntaxNodeProperty { Name = "Semicolon", Type = "SyntaxToken", Optional=true, Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "CloseBrace", Type = "SyntaxToken", Completion="Syntax" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "SchemaTypeExpression",
                Doc = "",
                Base = "TypeExpression",
                Sealed = true,
                Kind = "SchemaTypeExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Columns", Type = "SyntaxList<SeparatedElement<Expression>>", Completion="Declaration" },
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ExpressionStatement",
                Doc = "",
                Base = "Statement",
                Sealed = true,
                Kind = "ExpressionStatement",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Inherit" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "MaterializeExpression",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "MaterializeExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "MaterializeKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="NonScalar" },
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "SetOptionStatement",
                Doc = "",
                Base = "Statement",
                Sealed = true,
                Kind = "SetOptionStatement",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "SetKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Name", Type = "NameDeclaration", Completion="Option" },
                    new SyntaxNodeProperty { Name = "ValueClause", Type = "OptionValueClause", Optional=true, Completion="Clause" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "OptionValueClause",
                Doc = "",
                Base = "Clause",
                Sealed = true,
                Kind = "OptionValueClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Equal", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Scalar" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "QueryParametersStatement",
                Doc = "",
                Base = "Statement",
                Sealed = true,
                Kind = "QueryParametersStatement",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "DeclareKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "QueryParametersKeyword", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<SeparatedElement<FunctionParameter>>", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion="Syntax" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "RestrictStatementWithClause",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "RestrictStatementWithClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "WithKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Properties", Type = "SyntaxList<SeparatedElement<NamedParameter>>", Completion="None"},
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "RestrictStatement",
                Doc = "",
                Base = "Statement",
                Sealed = true,
                Kind = "RestrictStatement",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "RestrictKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "AccessKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "ToKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Expressions", Type = "SyntaxList<SeparatedElement<Expression>>", Completion="Scalar" },
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "WithClause", Type = "RestrictStatementWithClause", Optional=true, Completion="Clause" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "PatternStatement",
                Doc = "",
                Base = "Statement",
                Sealed = true,
                Kind = "PatternStatement",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "DeclareKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "PatternKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Name", Type = "NameDeclaration", Completion="Declaration" },
                    new SyntaxNodeProperty { Name = "Pattern", Type = "PatternDeclaration", Optional=true, Completion="Clause" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "PatternDeclaration",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "PatternDeclaration",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "EqualToken", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<SeparatedElement<NameAndTypeDeclaration>>", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "PathParameter", Type = "PatternPathParameter", Optional=true, Completion="Clause" },
                    new SyntaxNodeProperty { Name = "OpenBrace", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Patterns", Type = "SyntaxList<PatternMatch>", Completion="Clause" },
                    new SyntaxNodeProperty { Name = "CloseBrace", Type = "SyntaxToken", Completion="Syntax" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "PatternPathParameter",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "PatternPathParameter",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "OpenBracket", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Parameter", Type = "NameAndTypeDeclaration", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "CloseBracket", Type = "SyntaxToken", Completion="Syntax" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "PatternMatch",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "PatternMatch",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ParameterValues", Type = "ExpressionList", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "PathValue", Type = "PatternPathValue", Optional=true, Completion="Clause" },
                    new SyntaxNodeProperty { Name = "EqualToken", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Body", Type = "FunctionBody", Completion="Clause" },
                    new SyntaxNodeProperty { Name = "SemicolonToken", Type = "SyntaxToken", Completion="Syntax" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "PatternPathValue",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "PatternPathValue",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "DotToken", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "OpenBracket", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Value", Type = "Expression", Completion="Scalar" },
                    new SyntaxNodeProperty { Name = "CloseBracket", Type = "SyntaxToken", Completion="Syntax" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "DataScopeExpression",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "DataScopeExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression" },
                    new SyntaxNodeProperty { Name = "DataScopeClause", Type = "DataScopeClause", Completion="Clause" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "DataTableExpression",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "DataTableExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "DataTableKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None" },
                    new SyntaxNodeProperty { Name = "Schema", Type = "RowSchema", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "OpenBracket", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "LeadingComma", Type="SyntaxToken", Optional=true, Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Values", Type = "SyntaxList<SeparatedElement<Expression>>", Completion="Literal" },
                    new SyntaxNodeProperty { Name = "CloseBracket", Type = "SyntaxToken", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "RowSchema",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "RowSchema",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "LeadingComma", Type = "SyntaxToken", Optional=true, Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Columns", Type = "SyntaxList<SeparatedElement<NameAndTypeDeclaration>>", Completion="Declaration" },
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "EvaluateRowSchema",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "EvaluateRowSchema",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "LeadingComma", Type = "SyntaxToken", Optional=true, Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "AsteriskToken", Type = "SyntaxToken", Optional=true, Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "AsteriskTokenComma", Type = "SyntaxToken", Optional=true, Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Columns", Type = "SyntaxList<SeparatedElement<NameAndTypeDeclaration>>", Completion="Declaration" },
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ExternalDataExpression",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "ExternalDataExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ExternalDataKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="None" },
                    new SyntaxNodeProperty { Name = "Schema", Type = "RowSchema", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "OpenBracket", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "URIs", Type="SyntaxList<SeparatedElement<Expression>>", Completion="None" },
                    new SyntaxNodeProperty { Name = "CloseBracket", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "WithClause", Type = "ExternalDataWithClause", Optional=true, Completion="Clause" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ContextualDataTableExpression",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "ContextualDataTableExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ContextualDataTableKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Id", Type = "Expression", Completion="None" },
                    new SyntaxNodeProperty { Name = "Schema", Type = "RowSchema", Completion="Syntax" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ExternalDataWithClause",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "ExternalDataWithClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "WithKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Properties", Type="SyntaxList<SeparatedElement<NamedParameter>>" },
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion="Syntax" }
                }
            },

            // Inline External Table Definitions

            new SyntaxNodeInfo
            {
                Name = "InlineExternalTableKindClause",
                Doc = "",
                Base = "Clause",
                Sealed = true,
                Kind = "InlineExternalTableKindClause",
                Properties = new []
                {
                    // Reusing existing keyword
                    new SyntaxNodeProperty { Name = "KindKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "EqualToken", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Value", Type = "SyntaxToken", Completion="Keyword"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "InlineExternalTableDataFormatClause",
                Doc = "",
                Base = "Clause",
                Sealed = true,
                Kind = "InlineExternalTableDataFormatClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "DataFormatKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "EqualToken", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Value", Type = "SyntaxToken", Completion="Keyword"}
                }
            },

            new SyntaxNodeInfo
            {
                Name = "DateTimePattern",
                Doc = "datetime pattern expression in Inline External Table path format.",
                Base = "Expression",
                Sealed = true,
                Kind = "DateTimePattern",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "DateTimePatternKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion = "Syntax" },
                    new SyntaxNodeProperty { Name = "StringLiteral", Type = "LiteralExpression", Completion="Keyword"},
                    new SyntaxNodeProperty { Name = "Comma", Type = "SyntaxToken", Completion = "Syntax" },
                    new SyntaxNodeProperty { Name = "PartitionColumn", Type = "NameReference", Completion="Column"},
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion = "Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "InlineExternalTablePathFormatPartitionColumnReference",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "InlineExternalTablePathFormatPartitionColumnReference",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "PartitionColumnExpression", Type = "Expression", Completion="Column"},
                    new SyntaxNodeProperty { Name = "SeparatorLiteral", Type = "LiteralExpression", Optional=true, Completion="Syntax"},
                }
            },

            new SyntaxNodeInfo
            {
                Name = "InlineExternalTablePathFormatClause",
                Doc = "",
                Base = "Clause",
                Sealed = true,
                Kind = "InlineExternalTablePathFormatClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "PathFormatKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "EqualToken", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion = "Syntax" },
                    new SyntaxNodeProperty { Name = "OptionalSeparatorLiteral", Type = "LiteralExpression", Optional=true, Completion="Syntax"},  // To support syntax: "literal" PartitionExpr "literal"
                    new SyntaxNodeProperty { Name = "PathExpressions", Type = "SyntaxList<InlineExternalTablePathFormatPartitionColumnReference>", Completion="Column"},
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion = "Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "PartitionColumnDeclaration",
                Doc = "",
                Base = "Expression",
                Sealed = true,
                Kind = "PartitionColumnDeclaration",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Name", Type = "NameDeclaration", Completion="Declaration" },
                    new SyntaxNodeProperty { Name = "Colon", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Type", Type = "TypeExpression", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Equal", Type = "SyntaxToken", Optional=true, Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Expr", Type = "Expression", Optional=true, Completion="Column" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "InlineExternalTablePartitionClause",
                Doc = "A clause that specifies the partitioning for inline external table.",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "InlineExternalTablePartitionClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "PartitionKeyword", Type = "SyntaxToken", Completion = "Keyword" },
                    new SyntaxNodeProperty { Name = "ByKeyword", Type = "SyntaxToken", Completion = "Keyword" },
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion = "Syntax" },
                    new SyntaxNodeProperty { Name = "LeadingComma", Type = "SyntaxToken", Optional=true, Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "PartitionColumns", Type = "SyntaxList<SeparatedElement<PartitionColumnDeclaration>>", Completion="Declaration" },
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion = "Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "InlineExternalTableConnectionStringsClause",
                Doc = "A clause that specifies list of connection strings for inline external table.",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "InlineExternalTableConnectionStringsClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion = "Syntax" },
                    new SyntaxNodeProperty { Name = "ConnectionStrings", Type = "SyntaxList<SeparatedElement<Expression>>", Completion = "None"},
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion = "Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "InlineExternalTableExpression",
                Doc = "A node in the kusto syntax that represents an inline external table expression.",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "InlineExternalTableExpression",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "InlineExternalTableKeyword", Type = "SyntaxToken", Completion = "Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion = "None" },
                    new SyntaxNodeProperty { Name = "Schema", Type = "RowSchema", Completion = "Syntax" },
                    new SyntaxNodeProperty { Name = "KindParameter", Type = "InlineExternalTableKindClause", Completion = "Syntax" },
                    new SyntaxNodeProperty { Name = "PartitionClause", Type = "InlineExternalTablePartitionClause", Optional = true, Completion = "Syntax" },
                    new SyntaxNodeProperty { Name = "PathFormat", Type = "InlineExternalTablePathFormatClause", Optional = true, Completion = "Syntax" },
                    new SyntaxNodeProperty { Name = "DataFormatParameter", Type = "InlineExternalTableDataFormatClause", Completion = "Syntax" },
                    new SyntaxNodeProperty { Name = "ConnectionStrings", Type = "InlineExternalTableConnectionStringsClause", Completion = "Syntax" },
                    new SyntaxNodeProperty { Name = "WithClause", Type = "ExternalDataWithClause", Optional=true, Completion="Syntax" }
                }
            },

            // End of Inline External Table Definitions

            new SyntaxNodeInfo
            {
                Name = "JoinOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "JoinOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "JoinKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="NonScalar" },
                    new SyntaxNodeProperty { Name = "ConditionClause", Type = "JoinConditionClause", Optional=true, Completion="Clause" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "LookupOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "LookupOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "LookupKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="NonScalar" },
                    new SyntaxNodeProperty { Name = "LookupClause", Type = "JoinConditionClause", Completion="Clause" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "JoinConditionClause",
                Doc = "",
                Abstract = true,
                Base = "Clause"
            },

            new SyntaxNodeInfo
            {
                Name = "JoinOnClause",
                Doc = "",
                Base = "JoinConditionClause",
                Sealed = true,
                Kind = "JoinOnClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "OnKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Expressions", Type = "SyntaxList<SeparatedElement<Expression>>", Completion="Column" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "JoinWhereClause",
                Doc = "",
                Base = "JoinConditionClause",
                Sealed = true,
                Kind = "JoinWhereClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "WhereKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Boolean" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ScanOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "ScanOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ScanKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "OrderByClause", Type = "ScanOrderByClause", Optional=true, Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "PartitionByClause", Type = "ScanPartitionByClause", Optional=true, Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "DeclareClause", Type = "ScanDeclareClause", Optional=true, Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "WithKeyword", Type="SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "OpenParenToken", Type = "SyntaxToken", Optional=true, Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Steps", Type = "SyntaxList<ScanStep>", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "CloseParenToken", Type = "SyntaxToken", Optional=true, Completion="Syntax" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "AssertSchemaOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "AssertSchemaOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "AssertSchemaKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Schema", Type = "RowSchema", Completion="Syntax" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ScanDeclareClause",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "ScanDeclareClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "DeclareKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Declarations", Type = "SyntaxList<SeparatedElement<FunctionParameter>>", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ScanOrderByClause",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "ScanOrderByClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "OrderKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "ByKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Expressions", Type = "SyntaxList<SeparatedElement<Expression>>", Completion="Scalar" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ScanPartitionByClause",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "ScanPartitionByClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "PartitionKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "ByKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Expressions", Type = "SyntaxList<SeparatedElement<Expression>>", Completion="Scalar" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ScanStepOutput",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "ScanStepOutput",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "OutputKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "EqualToken", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "OutputKind", Type = "SyntaxToken", Completion="Keyword" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ScanStep",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "ScanStep",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "StepKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Name", Type = "NameDeclaration", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "OptionalKeyword", Type = "SyntaxToken", Optional=true, Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "ScanStepOutput", Type = "ScanStepOutput", Optional=true, Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "ColonToken", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Condition", Type="Expression", Completion="Scalar" },
                    new SyntaxNodeProperty { Name = "ComputationClause", Type="ScanComputationClause", Optional=true, Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "SemicolonToken", Type = "SyntaxToken", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ScanComputationClause",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "ScanComputationClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "ArrowToken", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Assignments", Type = "SyntaxList<SeparatedElement<ScanAssignment>>", Completion="Column" },
                }
            },

            new SyntaxNodeInfo
            {
                Name = "ScanAssignment",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "ScanAssignment",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Name", Type = "NameReference", Completion="Column" },
                    new SyntaxNodeProperty { Name = "EqualToken", Type = "SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Expression", Type = "Expression", Completion="Scalar" },
                }
            },

            new SyntaxNodeInfo
            {
                Name="PartitionByOperator",
                Doc = "",
                Base = "QueryOperator",
                Sealed = true,
                Kind = "PartitionByOperator",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "PartitionByKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Parameters", Type = "SyntaxList<NamedParameter>", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Entity", Type = "Expression", Completion="Column" },
                    new SyntaxNodeProperty { Name = "IdClause", Type="PartitionByIdClause", Optional=true, Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "OpenParen", Type="SyntaxToken", Completion="Syntax" },
                    new SyntaxNodeProperty { Name = "Subquery", Type="Expression", Completion="Query" },
                    new SyntaxNodeProperty { Name = "CloseParen", Type="SyntaxToken", Completion="Syntax" }
                }
            },

            new SyntaxNodeInfo
            {
                Name="PartitionByIdClause",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "PartitionByIdClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "IdKeyword", Type = "SyntaxToken", Completion="Keyword" },
                    new SyntaxNodeProperty { Name = "Value", Type = "Expression", Completion="Literal" }
                }
            },

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //  Commands
            //

            new SyntaxNodeInfo
            {
                Name = "CommandWithClause",
                Doc = "",
                Base = "Clause",
                Abstract = true
            },

            new SyntaxNodeInfo
            {
                Name = "CommandWithValueClause",
                Doc = "",
                Base = "CommandWithClause",
                Sealed = true,
                Kind = "CommandWithValueClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "WithKeyword", Type = "SyntaxToken", Completion="Keyword"},
                    new SyntaxNodeProperty { Name = "Value", Type = "Expression", Optional=true, Completion="Literal" }
                }
            },

            new SyntaxNodeInfo
            {
                Name = "CommandWithPropertyListClause",
                Doc = "",
                Base = "CommandWithClause",
                Sealed = true,
                Kind = "CommandWithPropertyListClause",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "WithKeyword", Type = "SyntaxToken", Completion="Keyword"},
                    new SyntaxNodeProperty { Name = "OpenParen", Type = "SyntaxToken", Completion="Keyword"},
                    new SyntaxNodeProperty { Name = "Properties", Type = "SyntaxList<SeparatedElement<NamedParameter>>", Completion="Scalar" },
                    new SyntaxNodeProperty { Name = "CloseParen", Type = "SyntaxToken", Completion="Keyword"},
                }
            },

            new SyntaxNodeInfo
            {
                Name = "Command",
                Doc = "",
                Base = "Expression",
                Abstract = true
            },

            new SyntaxNodeInfo
            {
                Name = "UnknownCommand",
                Doc = "",
                Base = "Command",
                Sealed = true,
                Kind = "UnknownCommand",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "DotToken", Type = "SyntaxToken", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "Parts", Type = "SyntaxList<SyntaxElement>", Completion="Syntax"},
                }
            },

            new SyntaxNodeInfo
            {
                Name = "CustomCommand",
                Doc = "",
                Base = "Command",
                Sealed = true,
                Kind = "CustomCommand",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "CommandKind", Type = "string", IsSyntax=false },
                    new SyntaxNodeProperty { Name = "DotToken", Type = "SyntaxToken", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "Custom", Type = "SyntaxElement", Completion="Syntax"},
                }
            },

            new SyntaxNodeInfo
            {
                Name = "PartialCommand",
                Doc = "",
                Base = "Command",
                Sealed = true,
                Kind = "PartialCommand",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "CommandKinds", Type = "IReadOnlyList<string>", IsSyntax=false },
                    new SyntaxNodeProperty { Name = "DotToken", Type = "SyntaxToken", Completion="Syntax"},
                    new SyntaxNodeProperty { Name = "Parts", Type = "SyntaxList<SyntaxElement>", Completion="Syntax"},
                }
            },

            new SyntaxNodeInfo
            {
                Name = "CommandAndSkippedTokens",
                Doc = "",
                Base = "Command",
                Sealed = true,
                Kind = "CommandAndSkippedTokens",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Command", Type = "Command", Completion="NonScalar"},
                    new SyntaxNodeProperty { Name = "SkippedTokens", Type="SkippedTokens", Optional=true, Completion="None"},
                }
            },

            new SyntaxNodeInfo
            {
                Name = "BadCommand",
                Doc = "",
                Base = "Command",
                Sealed = true,
                Kind = "BadCommand",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "DotToken", Type = "SyntaxToken", Completion="Syntax"},
                }
            },

            new SyntaxNodeInfo
            {
                Name = "CommandBlock",
                Doc = "",
                Base = "SyntaxNode",
                Sealed = true,
                Kind = "CommandBlock",
                Properties = new []
                {
                    new SyntaxNodeProperty { Name = "Directives", Type = "SyntaxList<Directive>", Completion="None"},
                    new SyntaxNodeProperty { Name = "Statements", Type = "SyntaxList<SeparatedElement<Statement>>", Completion="NonScalar"},
                    new SyntaxNodeProperty { Name = "SkippedTokens", Type="SkippedTokens", Optional=true, Completion="None"},
                    new SyntaxNodeProperty { Name = "EndOfCommand", Type="SyntaxToken", Optional=true, Completion="None"},
                }
            }
        };

    public static KnownTypeInfo[] KnownTypes = new KnownTypeInfo[]
    {
    };
};

#if !T4
}
#endif
// #>