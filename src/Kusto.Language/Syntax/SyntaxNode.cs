using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kusto.Language.Editor;

namespace Kusto.Language.Syntax
{
    using Utils;
    using Parsing;

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
        /// Returns the corresponding node in the original syntax tree
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

        /// <summary>
        /// Gets the equivalent position in the original syntax tree
        /// as the position within this copied tree fragment.
        /// </summary>
        public int GetPositionInOriginalTree(int position)
        {
            var originalPosition = position;
            var tree = this.Tree;

            while (tree.Original != null)
            {
                originalPosition += tree.OffsetInOriginal;
                tree = tree.Original;
            }

            return originalPosition;
        }
    }

    public static class SyntaxNodeExtensions
    {
        /// <summary>
        /// Copies this node as the root of a separate syntax tree fragment.
        /// </summary>
        public static T CopyAsFragment<T>(this T node)
            where T : SyntaxNode
        {
            var cloned = node.Clone();
            // creating new tree attaches to cloned node
            var _= new SyntaxTree(cloned, node.Tree, node.TriviaStart);
            return (T)cloned;
        }
    }

    public partial class Expression
    {
        /// <summary>
        /// True if the expression is a literal.
        /// </summary>
        public virtual bool IsLiteral => false;

        /// <summary>
        /// The value info of the literal expression.
        /// </summary>
        public virtual ValueInfo LiteralValueInfo => null;

        /// <summary>
        /// The value of the literal expression.
        /// </summary>
        public object LiteralValue => LiteralValueInfo?.Value;
    }

    public partial class LiteralExpression
    {
        public override bool IsLiteral => true;

        public override ValueInfo LiteralValueInfo => 
            new ValueInfo(this.Token.Text, this.Token.ValueText, this.Token.Value);
    }

    public partial class CompoundStringLiteralExpression
    {
        public override bool IsLiteral => true;

        private ValueInfo _literalValue;

        public override ValueInfo LiteralValueInfo
        {
            get
            {
                if (_literalValue == null)
                {
                    var text = this.ToString(IncludeTrivia.Minimal);
                    var valueText = string.Concat(this.Tokens.Select(t => t.ValueText));
                    _literalValue = new ValueInfo(text, valueText, valueText);
                }

                return _literalValue;
            }
        }
    }

    public partial class TypeOfLiteralExpression
    {
        public override bool IsLiteral => true;

        public override ValueInfo LiteralValueInfo => null;
    }

    public partial class DynamicExpression
    {
        public override bool IsLiteral => true;

        private ValueInfo _literalInfo;

        public override ValueInfo LiteralValueInfo
        {
            get
            {
                if (_literalInfo == null)
                {
                    var text = this.ToString(IncludeTrivia.Minimal);
                    var valueText = this.Expression.ToString(IncludeTrivia.Minimal);

                    if (this.Kind == SyntaxKind.NullLiteralExpression)
                    {
                        _literalInfo = new ValueInfo(text, valueText, null);
                    }
                    else
                    {
                        _literalInfo = new ValueInfo(text, valueText, valueText);
                    }
                }

                return _literalInfo;
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
        public override string SimpleName => this.Name.LiteralValue as string ?? "";
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

    public partial class Directive
    {
        /// <summary>
        /// The name of the directive
        /// </summary>
        public string Name => this.Info.Name;

        /// <summary>
        /// The text after the name contain any arguments
        /// </summary>
        public string ArgumentsText => this.Info.ArgumentsText;

        /// <summary>
        /// The text parsed into arguments.
        /// </summary>
        public IReadOnlyList<ClientDirectiveArgument> Arguments => this.Info.Arguments;

        /// <summary>
        /// The parsed client directive.
        /// </summary>
        private ClientDirective Info
        {
            get
            {
                if (_lazyClientDirective == null)
                {
                    ClientDirective.TryParse(this.Token.Text, out var info);
                    Interlocked.CompareExchange(ref _lazyClientDirective, info, null);
                }
                return _lazyClientDirective;
            }
        }

        private ClientDirective _lazyClientDirective;
    }
}