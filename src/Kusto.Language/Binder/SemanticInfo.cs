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
        private readonly object _referencedSymbolOrSignature;

        /// <summary>
        /// The symbol referenced by the <see cref="SyntaxNode"/>,
        /// a column, function, operator, etc.
        /// May be null.
        /// </summary>
        public Symbol ReferencedSymbol
        {
            get
            {
                if (_referencedSymbolOrSignature is Signature sig)
                {
                    return sig.Symbol;
                }
                else
                {
                    return _referencedSymbolOrSignature as Symbol;
                }
            }
        }

        /// <summary>
        /// The matching signature of the function or operator symbol referenced by the <see cref="SyntaxNode"/>.
        /// May be null.
        /// </summary>
        public Signature ReferencedSignature
        {
            get
            {
                if (_referencedSymbolOrSignature is Signature sig)
                {
                    return sig;
                }
                else if (_referencedSymbolOrSignature is FunctionSymbol fn && fn.Signatures.Count == 1)
                {
                    return fn.Signatures[0];
                }
                else if (_referencedSymbolOrSignature is VariableSymbol vs && vs.Type is FunctionSymbol vfn && vfn.Signatures.Count == 1)
                {
                    return vfn.Signatures[0];
                }
                else
                {
                    return null;
                }
            }
        }

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

        private SemanticInfo(
            object referenced,
            TypeSymbol result,
            IEnumerable<Diagnostic> diagnostics,
            bool isConstant,
            FunctionCallInfo calledFunctionInfo)
        {
            _referencedSymbolOrSignature = referenced;
            this.ResultType = result;
            this.Diagnostics = diagnostics != null ? diagnostics.ToReadOnly() : Diagnostic.NoDiagnostics;
            this.IsConstant = isConstant;
            this.CalledFunctionInfo = calledFunctionInfo;
        }

        public SemanticInfo(
            Symbol referencedSymbol,
            TypeSymbol result,
            IEnumerable<Diagnostic> diagnostics = null,
            bool isConstant = false,
            FunctionCallInfo calledFunctionInfo = null)
            : this((object)referencedSymbol, result, diagnostics, isConstant, calledFunctionInfo)
        {
        }

        public SemanticInfo(
            Signature referencedSignature,
            TypeSymbol result,
            IEnumerable<Diagnostic> diagnostics = null,
            bool isConstant = false,
            FunctionCallInfo calledFunctionInfo = null)
            : this((object)referencedSignature, result, diagnostics, isConstant, calledFunctionInfo)
        {
        }

        public SemanticInfo(TypeSymbol result, IEnumerable<Diagnostic> diagnostics = null, bool isConstant = false, FunctionCallInfo calledFunctionInfo = null)
            : this((Symbol)null, result, diagnostics, isConstant, calledFunctionInfo)
        {
        }

        public SemanticInfo(Symbol referencedSymbol, TypeSymbol result, Diagnostic diagnostic)
            : this(referencedSymbol, result, diagnostic != null ? new List<Diagnostic> { diagnostic }.AsReadOnly() : Diagnostic.NoDiagnostics)
        {
        }

        public SemanticInfo(Signature referencedSignature, TypeSymbol result, Diagnostic diagnostic)
            : this(referencedSignature, result, diagnostic != null ? new List<Diagnostic> { diagnostic }.AsReadOnly() : Diagnostic.NoDiagnostics)
        {
        }

        public SemanticInfo(TypeSymbol result, Diagnostic diagnostic)
            : this((Symbol)null, result, diagnostic)
        {
        }

        public SemanticInfo(IEnumerable<Diagnostic> diagnostics)
            : this((Symbol)null, null, diagnostics)
        {
        }

        public SemanticInfo WithReferencedSymbol(Symbol symbol)
        {
            return new SemanticInfo(symbol, this.ResultType, this.Diagnostics, this.IsConstant, this.CalledFunctionInfo);
        }

        public SemanticInfo WithReferencedSignature(Signature signature)
        {
            return new SemanticInfo(signature, this.ResultType, this.Diagnostics, this.IsConstant, this.CalledFunctionInfo);
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