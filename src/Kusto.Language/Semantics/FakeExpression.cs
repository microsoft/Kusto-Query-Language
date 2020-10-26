using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kusto.Language.Symbols;

namespace Kusto.Language.Syntax
{
    public static class FakeExpression
    {
        public static Expression Create(TypeSymbol type)
        {
            var ex = new LiteralExpression(SyntaxKind.TokenLiteralExpression, SyntaxToken.Missing(SyntaxKind.IdentifierToken));
            Binding.Binder.DefaultSetSemanticInfo(ex, new SemanticInfo(type));
            return ex;
        }

        public static Expression CreateNamed(string name, TypeSymbol type)
        {
            var named = new SimpleNamedExpression(
                new NameDeclaration(new TokenName(SyntaxToken.Identifier("", name))),
                SyntaxToken.Punctuation("", SyntaxKind.EqualToken),
                Create(type));
            Binding.Binder.DefaultSetSemanticInfo(named, new SemanticInfo(type));
            return named;
        }
    }
}