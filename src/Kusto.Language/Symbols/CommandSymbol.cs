using System;
using System.Collections.Generic;
using System.Linq;
using Kusto.Language.Parsing;
using Kusto.Language.Syntax;

namespace Kusto.Language.Symbols
{
    using Utils;

    public class CommandSymbol : Symbol
    {
        private string resultSchema;
        private TableSymbol resultType;

        /// <summary>
        /// The language declaration of the result schema: (col:type, ...)
        /// </summary>
        public string ResultSchema
        {
            get
            {
                if (this.resultSchema == null && this.resultType != null)
                {
                    this.resultSchema = this.resultType.Display;
                }

                return this.resultSchema;
            }
        }

        /// <summary>
        /// The <see cref="TableSymbol"/> describing the result schema.
        /// </summary>
        public TableSymbol ResultType
        {
            get
            {
                if (this.resultType == null && this.resultSchema != null)
                {
                    if (this.resultSchema == "()"
                        || this.resultSchema == "(*)")
                    {
                        this.resultType = new TableSymbol().WithIsOpen(true);
                    }
                    else
                    {
                        this.resultType = TableSymbol.From(this.resultSchema);
                    }
                }

                return this.resultType;
            }
        }

        public override SymbolKind Kind => SymbolKind.Command;

        public CommandSymbol(string name, string resultSchema)
            : base(name)
        {
            this.resultSchema = resultSchema;
        }
    }
}
