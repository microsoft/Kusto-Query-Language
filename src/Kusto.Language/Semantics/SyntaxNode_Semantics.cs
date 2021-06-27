using System;
using System.Collections;
using System.Collections.Generic;

namespace Kusto.Language.Syntax
{
    using Binding;
    using Kusto.Language.Parsing;
    using Symbols;
    using System.Linq;
    using Utils;

    public abstract partial class SyntaxNode
    {
        /// <summary>
        /// The <see cref="ReferencedSymbol"/> referenced by this node.
        /// </summary>
        public Symbol ReferencedSymbol => GetSemanticInfo()?.ReferencedSymbol;

        /// <summary>
        /// The expansion of the referenced user function, or null.
        /// </summary>
        public SyntaxNode GetExpansion()
        {
            return GetSemanticInfo()?.Expander?.Invoke();
        }

        /// <summary>
        /// Semantic diagnostics located at this node.
        /// </summary>
        public IReadOnlyList<Diagnostic> SemanticDiagnostics => this.GetSemanticInfo()?.Diagnostics ?? Diagnostic.NoDiagnostics;

        internal SemanticInfo GetSemanticInfo()
        {
            var data = GetExtendedData(create: false);
            return data?.SemanticInfo;
        }

        /// <summary>
        /// True if this node has already been bound.
        /// </summary>
        internal bool IsBound => this.GetSemanticInfo() != null;
    }

    [Flags]
    public enum DiagnosticsInclude
    {
        Syntactic =    0b0001,
        Semantic =  0b0010,
        Expansion = 0b0100
    }

    public abstract partial class SyntaxElement
    {
        /// <summary>
        /// Gets diagnostics for this <see cref="SyntaxNode"/> an all child elements.
        /// </summary>
        public IReadOnlyList<Diagnostic> GetContainedDiagnostics(DiagnosticsInclude include = DiagnosticsInclude.Syntactic | DiagnosticsInclude.Semantic, CancellationToken cancellationToken = default(CancellationToken))
        {
            var list = new List<Diagnostic>();
            GatherDiagnostics(this, list, include, cancellationToken: cancellationToken);
            return list.AsReadOnly();
        }

        protected static void GatherDiagnostics(
            SyntaxElement root,
            List<Diagnostic> diagnostics,
            DiagnosticsInclude include,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            bool includeSyntax = (include & DiagnosticsInclude.Syntactic) != 0;
            bool includeSemantic = (include & DiagnosticsInclude.Semantic) != 0;
            bool includeExpansion = (include & DiagnosticsInclude.Expansion) != 0;

            var fnDescend = (include == DiagnosticsInclude.Syntactic)
                ? (Func<SyntaxElement, bool>)((SyntaxElement e) => e.ContainsSyntaxDiagnostics)
                : null;

            SyntaxElement.WalkElements(root, 
                fnBefore: element =>
                {
                    if (element.HasSyntaxDiagnostics && includeSyntax)
                    {
                        // each syntax diagnostic is located at the element that carries it.
                        diagnostics.AddRange(element.SyntaxDiagnostics.Select(d => d.HasLocation ? d : SetLocation(d, element)));
                    }

                    if (includeSemantic && element is SyntaxNode node && node.SemanticDiagnostics.Count > 0)
                    {
                        diagnostics.AddRange(node.SemanticDiagnostics);
                    }
                }, 
                fnAfter: element =>
                {
                    if (includeExpansion && element is Expression expr && expr.GetExpansion() is SyntaxNode expansion)
                    {
                        var originalCount = diagnostics.Count;
                        GatherDiagnostics(expansion, diagnostics, include, cancellationToken);

                        if (diagnostics.Count > originalCount)
                        {
                            var name = expr.ReferencedSymbol?.Name ?? "<unknown>";
                            var location = expr is FunctionCallExpression fc ? fc.Name : expr;
                            var errors = diagnostics[originalCount].Message;
                            var dx = DiagnosticFacts.GetErrorInExpansion(name, errors).WithLocation(location);
                            diagnostics.SetCount(originalCount);
                            diagnostics.Add(dx);
                        }
                    }
                },
                fnDescend: fnDescend);
        }

        private static Diagnostic SetLocation(Diagnostic d, SyntaxElement location)
        {
            if (location.Width == 0)
            {
                if (location is SyntaxToken token)
                {
                    // move location to next token if it is
                    // lest than two spaces away and not separated by line breaks
                    var next = token.GetNextToken();

                    if (next != null
                        && (next.TextStart - token.End) < 2
                        && !TextFacts.HasLineBreaks(next.Trivia))
                    {
                        location = next;
                    }
                }
            }

            return d.WithLocation(location);
        }
    }

    public abstract partial class Expression
    {
        /// <summary>
        /// The result type of the expression.
        /// </summary>
        public TypeSymbol ResultType
        {
            get
            {
                var type = this.RawResultType;

                // don't show type as tuple if it only has one column
                if (type is TupleSymbol ts && ts.Columns.Count == 1)
                {
                    return ts.Columns[0].Type;
                }

                return type;
            }
        }

        /// <summary>
        /// The unadjusted result type of the expression.
        /// </summary>
        public TypeSymbol RawResultType => GetSemanticInfo()?.ResultType;

        /// <summary>
        /// True if the expression is considered constant.
        /// </summary>
        public bool IsConstant => GetSemanticInfo()?.IsConstant ?? false;

        /// <summary>
        /// The value of the constant if it is known.
        /// </summary>
        public object ConstantValue =>
            this.IsLiteral ? this.LiteralValue
            : this.ReferencedSymbol is VariableSymbol v && v.IsConstant ? v.ConstantValue
            : null;
    }
}