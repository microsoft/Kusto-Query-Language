using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kusto.Language.Symbols
{
    using Syntax;
    using Utils;

    /// <summary>
    /// The symbol for a pattern.
    /// </summary>
    public sealed class PatternSymbol : TypeSymbol
    {
        public override SymbolKind Kind => SymbolKind.Pattern;

        public IReadOnlyList<Parameter> Parameters { get; }

        public Parameter PathParameter { get; }

        public IReadOnlyList<PatternSignature> Signatures { get; }

        public PatternSymbol(
            string name,
            IReadOnlyList<Parameter> parameters = null,
            Parameter pathParameter = null,
            IReadOnlyList<PatternSignature> signatures = null)
            : base(name)
        {
            this.Parameters = parameters ?? EmptyReadOnlyList<Parameter>.Instance;
            this.PathParameter = pathParameter;
            this.Signatures = signatures ?? EmptyReadOnlyList<PatternSignature>.Instance;

            foreach (var sig in this.Signatures)
            {
                sig.Symbol = this;
            }
        }

        public PatternSymbol(string name)
            : this(name, null, null, null)
        {
        }

        public override Tabularity Tabularity => Tabularity.Tabular;

        protected override string GetDisplay()
        {
            var prms = string.Join(", ", this.Parameters.Select(p => $"{p.Name}:{p.TypeDisplay}"));
            return $"{this.Name}({prms})";
        }
    }

    public class PatternSignature
    {
        public PatternSymbol Symbol { get; internal set; }

        public IReadOnlyList<string> ArgumentValues { get; }
        public string PathValue { get; }

        private readonly string _bodyText;
        private readonly FunctionBody _bodySyntax;
        private Signature _signature;

        private PatternSignature(
            IReadOnlyList<string> argumentValues,
            string pathValue,
            string bodyText,
            FunctionBody bodySyntax)
        {
            this.ArgumentValues = argumentValues;
            this.PathValue = pathValue;
            _bodyText = bodyText;
            _bodySyntax = bodySyntax;
        }

        public PatternSignature(
            IReadOnlyList<string> argumentValues,
            string pathValue,
            string body)
            : this(argumentValues, pathValue, body, null)
        {
        }

        public PatternSignature(
            IReadOnlyList<string> argumentValues,
            string pathValue,
            FunctionBody body)
            : this(argumentValues, pathValue, null, body)
        {
        }

        public Signature Signature
        {
            get
            {
                if (_signature == null && this.Symbol != null)
                {
                    Signature sig = null;

                    var pms = this.Symbol.Parameters;
                    if (this.Symbol?.PathParameter != null)
                        pms = pms.Concat(new[] { this.Symbol.PathParameter }).ToList();

                    if (_bodySyntax != null)
                    {
                        sig = new Signature(_bodySyntax, pms);
                        sig.Symbol = this.Symbol;
                    }
                    else if (_bodyText != null)
                    {
                        sig = new Signature(_bodyText, Tabularity.Unspecified, pms);
                        sig.Symbol = this.Symbol;
                    }

                    Interlocked.CompareExchange(ref _signature, sig, null);
                }

                return _signature;
            }
        }
    }
}