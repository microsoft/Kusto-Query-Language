using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Binding
{
    using Syntax;
    using Symbols;
    using Utils;

    /// <summary>
    /// The semantic information associated with a <see cref="SyntaxNode"/>.
    /// </summary>
    internal class SemanticInfo
    {
        /// <summary>
        /// The symbol referenced by the <see cref="SyntaxNode"/>,
        /// a column, function, operator, etc.
        /// May be null.
        /// </summary>
        public Symbol ReferencedSymbol { get; }

        /// <summary>
        /// The result type of the expression.
        /// May be null if the node is not an expression.
        /// </summary>
        public TypeSymbol ResultType { get; }

        /// <summary>
        /// If true then the expression is considered constant.
        /// </summary>
        public bool IsConstant { get; }

        /// <summary>
        /// Diagnostics discovered during binding.
        /// </summary>
        public IReadOnlyList<Diagnostic> Diagnostics { get; }

        /// <summary>
        /// The expansion of the function called
        /// </summary>
        public FunctionCallInfo CalledFunctionInfo { get; }

        public SemanticInfo(
            Symbol referenced, 
            TypeSymbol result, 
            IEnumerable<Diagnostic> diagnostics = null, 
            bool isConstant = false, 
            FunctionCallInfo calledFunctionInfo = null)
        {
            this.ReferencedSymbol = referenced;
            this.ResultType = result;
            this.Diagnostics = diagnostics != null ? diagnostics.ToReadOnly() : Diagnostic.NoDiagnostics;
            this.IsConstant = isConstant;
            this.CalledFunctionInfo = calledFunctionInfo;
        }

        public SemanticInfo(TypeSymbol result, IEnumerable<Diagnostic> diagnostics = null, bool isConstant = false, FunctionCallInfo calledFunctionInfo = null)
            : this(null, result, diagnostics, isConstant, calledFunctionInfo)
        {
        }

        public SemanticInfo(Symbol referenced, TypeSymbol result, Diagnostic diagnostic)
            : this(referenced, result, diagnostic != null ? new List<Diagnostic> { diagnostic }.AsReadOnly() : Diagnostic.NoDiagnostics)
        {
        }

        public SemanticInfo(TypeSymbol result, Diagnostic diagnostic)
            : this(null, result, diagnostic)
        {
        }

        public SemanticInfo(IEnumerable<Diagnostic> diagnostics)
            : this(null, null, diagnostics)
        {
        }

        public SemanticInfo WithReferencedSymbol(Symbol symbol)
        {
            return new SemanticInfo(symbol, this.ResultType, this.Diagnostics, this.IsConstant, this.CalledFunctionInfo);
        }

        public SemanticInfo WithResultType(TypeSymbol type)
        {
            return new SemanticInfo(this.ReferencedSymbol, type, this.Diagnostics, this.IsConstant, this.CalledFunctionInfo);
        }

        public SemanticInfo WithDiagnostics(IEnumerable<Diagnostic> diagnostics)
        {
            return new SemanticInfo(this.ReferencedSymbol, this.ResultType, diagnostics, this.IsConstant, this.CalledFunctionInfo);
        }

        public SemanticInfo WithIsConstant(bool isConstant)
        {
            return new SemanticInfo(this.ReferencedSymbol, this.ResultType, this.Diagnostics, isConstant, this.CalledFunctionInfo);
        }

        public SemanticInfo WithCalledFunctionInfo(FunctionCallInfo calledFunctionInfo)
        {
            return new SemanticInfo(this.ReferencedSymbol, this.ResultType, this.Diagnostics, this.IsConstant, calledFunctionInfo);
        }

        /// <summary>
        /// A default <see cref="SemanticInfo"/> for nodes that are determined to have not information.
        /// </summary>
        public static readonly SemanticInfo Empty = new SemanticInfo((TypeSymbol)null);
    }
}