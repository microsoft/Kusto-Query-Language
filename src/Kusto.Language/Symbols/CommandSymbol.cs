using System;

namespace Kusto.Language.Symbols
{
    /// <summary>
    /// A <see cref="Symbol"/> corresponding to a control command.
    /// </summary>
    public class CommandSymbol : Symbol
    {
        private string _resultSchema;
        private TableSymbol _resultType;

        /// <summary>
        /// The language declaration of the result schema: (col:type, ...)
        /// </summary>
        public string ResultSchema
        {
            get
            {
                if (_resultSchema == null && _resultType != null)
                {
                    _resultSchema = SchemaDisplay.GetText(_resultType);
                }

                return _resultSchema;
            }
        }

        /// <summary>
        /// The <see cref="TableSymbol"/> describing the result schema.
        /// </summary>
        public TableSymbol ResultType
        {
            get
            {
                if (_resultType == null && _resultSchema != null)
                {
                    if (_resultSchema == "()"
                        || _resultSchema == "(*)")
                    {
                        _resultType = new TableSymbol().WithIsOpen(true);
                    }
                    else
                    {
                        _resultType = TableSymbol.From(_resultSchema);
                    }
                }

                return _resultType;
            }
        }

        public string Construction { get; }

        public override SymbolKind Kind => SymbolKind.Command;

        public CommandSymbol(string name, string resultSchema, string construction = null)
            : base(name)
        {
            _resultSchema = resultSchema;
            this.Construction = construction ?? "";
        }

        public CommandSymbol(string name, TableSymbol resultType, string construction = null)
            : base(name)
        {
            _resultType = resultType;
            this.Construction = construction ?? "";
        }
    }
}
