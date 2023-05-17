using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kusto.Language.Editor;

namespace Kusto.Language.Syntax
{
    using Utils;

    /// <summary>
    /// A non-terminal element in the syntax (contains one or more nodes/tokens/lists).
    /// </summary>
    public abstract partial class SyntaxNode : SyntaxElement
    {
        private int fullWidth;

        protected SyntaxNode(IReadOnlyList<Diagnostic> diagnostics)
            : base(diagnostics)
        {
        }

        protected override void Init()
        {
            base.Init();
            this.fullWidth = this.ComputeFullWidth();
        }

        public override int FullWidth => this.fullWidth;

        /// <summary>
        /// Creates a copy of this <see cref="SyntaxNode"/>
        /// </summary>
        public new SyntaxNode Clone(bool includeDiagnostics = true) => (SyntaxNode)this.CloneCore(includeDiagnostics);

        public abstract void Accept(SyntaxVisitor visitor);

        public abstract TResult Accept<TResult>(SyntaxVisitor<TResult> visitor);

        /// <summary>
        /// Invokes the action for this node and its descendant nodes, in lexical order, top down.
        /// </summary>
        /// <param name="action">The action that is invoked for each <see cref="SyntaxNode"/></param>
        public void WalkNodes(Action<SyntaxNode> action)
        {
            WalkNodes(this, action);
        }

        /// <summary>
        /// Returns the original <see cref="SyntaxNode"/> in the original <see cref="SyntaxTree"/>
        /// for a node in a copied tree fragment.
        /// </summary>
        public SyntaxNode GetOriginalNode()
        {
            var node = this;

            while (node.Tree?.Original != null)
            {
                var startInOriginal = node.Tree.OffsetInOriginal + node.TextStart;
                var locationInOriginal = node.Tree.Original.Root.GetNodeAt(startInOriginal, node.Width);

                if (locationInOriginal != null)
                {
                    node = locationInOriginal;
                    continue;
                }
            }

            return node;
        }

    }

    public partial class Expression
    {
        /// <summary>
        /// True if the expression is a literal.
        /// </summary>
        public virtual bool IsLiteral => false;

        /// <summary>
        /// The value of the literal expression.
        /// </summary>
        public virtual object LiteralValue => null;
    }

    public partial class LiteralExpression
    {
        public override bool IsLiteral => true;

        public override object LiteralValue => this.Token.Value;
    }

    public partial class CompoundStringLiteralExpression
    {
        public override bool IsLiteral => true;

        private string literalValue;

        public override object LiteralValue
        {
            get
            {
                if (this.literalValue == null)
                {
                    this.literalValue = string.Concat(this.Tokens.Select(t => t.ValueText));
                }

                return this.literalValue;
            }
        }
    }

    public partial class TypeOfLiteralExpression
    {
        public override bool IsLiteral => true;

        public override object LiteralValue => null;
    }

    public partial class DynamicExpression
    {
        public override bool IsLiteral => true;

        public override object LiteralValue
        {
            get
            {
                if (this.Kind == SyntaxKind.NullLiteralExpression)
                {
                    return null;
                }
                else
                {
                    return this.Expression.ToString(IncludeTrivia.Minimal);
                }
            }
        }
    }

    public partial class NameDeclaration
    {
        public string SimpleName => this.Name.SimpleName;

        public NameDeclaration(SyntaxToken nameToken, IReadOnlyList<Diagnostic> diagnostics = null)
            : this(new TokenName(nameToken), diagnostics)
        {
        }
    }

    public partial class NameReference
    {
        public string SimpleName => this.Name.SimpleName;

        public NameReference(Name name, IReadOnlyList<Diagnostic> diagnostics = null)
            : this(name, Symbols.SymbolMatch.Default, diagnostics)
        {
        }

        public NameReference(SyntaxToken nameToken, IReadOnlyList<Diagnostic> diagnostics = null)
            : this(new TokenName(nameToken), diagnostics)
        {
        }
    }

    public partial class Name
    {
        public virtual string SimpleName => "";
    }

    public partial class TokenName
    {
        public override string SimpleName => this.Name.ValueText;
    }

    public partial class BracketedName
    {
        public override string SimpleName => (this.Name.LiteralValue as string) ?? "";
    }

    public partial class BracedName
    {
        public override string SimpleName => this.Name.ValueText;
    }

    public partial class WildcardedName
    {
        public override string SimpleName => this.Pattern.ValueText;
    }

    public partial class BracketedWildcardedName
    {
        public override string SimpleName => this.Pattern.ValueText;
    }

    public partial class NamedParameter
    {
        public override CompletionHint GetCompletionHint(int index)
        {
            if (index == 2)
            {
                return this.ExpressionHint;
            }
            else
            {
                return base.GetCompletionHint(index);
            }
        }
    }
}