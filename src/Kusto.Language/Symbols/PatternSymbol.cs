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
        public IReadOnlyList<string> ArgumentValues { get; }

        public string PathValue { get; }

        public string Body { get; }

        public PatternMatch Declaration { get; }

        private PatternSignature(
            IReadOnlyList<string> argumentValues,
            string pathValue,
            string body,
            PatternMatch declaration)
        {
            this.ArgumentValues = argumentValues;
            this.PathValue = pathValue;
            this.Body = body;
            this.Declaration = declaration;
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
            PatternMatch declaration)
            : this(argumentValues, pathValue, null, declaration)
        {
        }
    }
}