using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    using Utils;

    /// <summary>
    /// A symbol representing a scalar operator.
    /// </summary>
    public class OperatorSymbol : Symbol
    {
        public override SymbolKind Kind => SymbolKind.Operator;

        public OperatorKind OperatorKind { get; }

        public TypeSymbol Result { get; }

        public override Tabularity Tabularity => Tabularity.Scalar;

        public IReadOnlyList<Signature> Signatures { get; }

        public OperatorSymbol(OperatorKind kind, IEnumerable<Signature> signatures)
            : base(kind.ToString())
        {
            this.OperatorKind = kind;

            this.Signatures = signatures.ToReadOnly();

            foreach (var signature in this.Signatures)
            {
                signature.Symbol = this;
            }
        }

        public OperatorSymbol(OperatorKind kind, params Signature[] signatures)
            : this(kind, (IEnumerable<Signature>)signatures)
        {
        }

        public OperatorSymbol(OperatorKind kind, TypeSymbol result)
            : this(kind, new[] { new Signature(result) })
        {
        }
    }
}