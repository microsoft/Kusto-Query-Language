using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    using Utils;

    [System.Diagnostics.DebuggerDisplay("Symbol: {Kind} {Display}")]
    public abstract class Symbol
    {
        /// <summary>
        /// The name of the symbol.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// An alternate name of the symbol.
        /// </summary>
        public virtual string AlternateName => "";

        /// <summary>
        /// The <see cref="SymbolKind"/> of the symbol.
        /// </summary>
        public virtual SymbolKind Kind => SymbolKind.None;

        protected Symbol(string name)
        {
            this.Name = name ?? "";
        }

        /// <summary>
        /// If true, the symbol is hidden from Intellisense.
        /// </summary>
        public virtual bool IsHidden => this.Name.StartsWith("__", StringComparison.Ordinal); // symbols that start with __ are internal only.

        /// <summary>
        /// True if the symbol is an error symbol.
        /// </summary>
        public virtual bool IsError => false;

        /// <summary>
        /// Identifies whether the symbol is scalar or tabular.
        /// </summary>
        public virtual Tabularity Tabularity => Tabularity.Unknown;

        /// <summary>
        /// True if the symbol is scalar or unknown.
        /// </summary>
        public bool IsScalar
        {
            get
            {
                switch (this.Tabularity)
                {
                    case Tabularity.Scalar:
                    case Tabularity.Unknown:
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// True if the symbol is tabular or unknown.
        /// </summary>
        public bool IsTabular
        {
            get
            {
                switch (this.Tabularity)
                {
                    case Tabularity.Tabular:
                    case Tabularity.Unknown:
                        return true;
                    default:
                        return false;
                }
            }
        }

        private string _display;

        protected virtual string GetDisplay() => 
            !string.IsNullOrEmpty(this.AlternateName) 
                ? $"{this.Name} ({this.AlternateName})" 
                : this.Name;

        /// <summary>
        /// A description of the symbol.
        /// </summary>
        public string Display
        {
            get
            {
                if (this._display == null)
                {
                    this._display = this.GetDisplay();
                }

                return this._display;
            }
        }

        /// <summary>
        /// All the symbols contained by this symbol.
        /// </summary>
        public virtual IReadOnlyList<Symbol> Members => 
            EmptyReadOnlyList<Symbol>.Instance;

        /// <summary>
        /// Gets all the matching members.
        /// </summary>
        public virtual void GetMembers(string name, SymbolMatch match, List<Symbol> symbols, bool ignoreCase = false)
        {
            foreach (var symbol in this.Members)
            {
                if (symbol.Matches(name, match, ignoreCase))
                {
                    symbols.Add(symbol);
                }
            }
        }

        /// <summary>
        /// Gets all the matching members.
        /// </summary>
        public void GetMembers(SymbolMatch match, List<Symbol> symbols, bool ignoreCase = false)
        {
            this.GetMembers(null, match, symbols, ignoreCase);
        }

        /// <summary>
        /// Returns the first member that matches or null.
        /// </summary>
        public Symbol GetFirstMember(string name, SymbolMatch match = SymbolMatch.Any, bool ignoreCase = false)
        {
            foreach (var symbol in this.Members)
            {
                if (symbol.Matches(name, match, ignoreCase))
                {
                    return symbol;
                }
            }

            return null;
        }

        /// <summary>
        /// Determines the result type of an expression that references the specified symbol
        /// </summary>
        public static TypeSymbol GetResultType(Symbol symbol)
        {
            switch (symbol)
            {
                case ColumnSymbol c:
                    return c.Type;

                case VariableSymbol v:
                    return GetResultType(v.Type);

                case EntityGroupElementSymbol e:
                    return GetResultType(e.UnderlyingSymbol);

                case ParameterSymbol p:
                    return p.Type;

                case GroupSymbol g:
                    var resultSymbols = new List<Symbol>();

                    foreach (var m in g.Members)
                    {
                        var rs = GetResultType(m);
                        if (rs != null)
                        {
                            resultSymbols.Add(rs);
                        }
                    }

                    if (resultSymbols.Count == 1)
                    {
                        return resultSymbols[0] as TypeSymbol;
                    }
                    else if (resultSymbols.Count > 1)
                    {
                        return new GroupSymbol(resultSymbols);
                    }
                    else
                    {
                        return null;
                    }

                case TypeSymbol t:
                    return t;

                default:
                    return null;
            }
        }

        /// <summary>
        /// True if this symbol can be assigned to the specified type.
        /// </summary>
        public bool IsAssignableTo(Symbol targetType, Conversion allowedConversion = Conversion.None)
        {
            return AreAssignable(targetType, this, allowedConversion);
        }

        /// <summary>
        /// True if this symbol can be assigned to any of the specified types.
        /// </summary>
        public bool IsAssignableToAny(IReadOnlyList<TypeSymbol> targetTypes, Conversion allowedConversion = Conversion.None)
        {
            return AreAssignable(targetTypes, this, allowedConversion);
        }

        /// <summary>
        /// True if a value of type <see cref="P:sourceType"/> can be assigned to any types in <see cref="P:targetTypes"/>
        /// </summary>
        private static bool AreAssignable(IReadOnlyList<TypeSymbol> targetTypes, Symbol sourceType, Conversion allowedConversion = Conversion.None)
        {
            for (int i = 0; i < targetTypes.Count; i++)
            {
                if (AreAssignable(targetTypes[i], sourceType, allowedConversion))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// True if a value of type <see cref="P:sourceType"/> can be assigned to of type <see cref="P:targetType"/>
        /// </summary>
        private static bool AreAssignable(Symbol targetType, Symbol sourceType, Conversion allowedConversion = Conversion.None)
        {
            if (targetType == sourceType)
                return true;

            if (targetType == null || sourceType == null)
                return false;

            if (sourceType == ScalarTypes.Unknown && targetType.IsScalar)
                return true;

            if (targetType == ScalarTypes.Unknown && sourceType.IsScalar)
                return true;

            // a single column tuple is assignable to a scalar
            if (sourceType.Kind == SymbolKind.Tuple
                && targetType.Kind == SymbolKind.Scalar
                && sourceType is TupleSymbol stt
                && stt.Columns.Count == 1)
                return AreAssignable(targetType, stt.Columns[0].Type);

            if (targetType.Kind != sourceType.Kind)
                return false;

            switch (targetType)
            {
                case ColumnSymbol tarCol:
                    var srcCol = (ColumnSymbol)sourceType;
                    return tarCol.Name == srcCol.Name && AreAssignable(tarCol.Type, srcCol.Type, allowedConversion);

                case TupleSymbol _:
                case GroupSymbol _:
                    return MembersEqual(targetType, sourceType);

                case TableSymbol tarTable:
                    return TablesAssignable(tarTable, (TableSymbol)sourceType);

                case ScalarSymbol tarScalar:
                    var srcScalar = (ScalarSymbol)sourceType;

                    switch (allowedConversion)
                    {
                        case Conversion.Promotable:
                            return srcScalar.IsPromotableTo(tarScalar);
                        case Conversion.Compatible:
                            return srcScalar.IsPromotableTo(tarScalar) || tarScalar.IsPromotableTo(srcScalar);
                        case Conversion.Any:
                            return true;
                        default:
                            return false;
                    }
            }

            return false;
        }

        /// <summary>
        /// True if the members of the source type are assignable to the members of the target type.
        /// </summary>
        public static bool MembersEqual(Symbol target, Symbol source)
        {
            if (target.Members.Count != source.Members.Count)
                return false;

            for (int i = 0, n = target.Members.Count; i < n; i++)
            {
                if (!AreAssignable(target.Members[i], source.Members[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// True if a table value can be assigned to a parameter of a specific table type.
        /// </summary>
        private static bool TablesAssignable(TableSymbol target, TableSymbol source)
        {
            // ensure that the value table has at least the columns specified for the parameter table.

            foreach (var tarCol in target.Columns)
            {
                if (!source.TryGetColumn(tarCol.Name, out var valueColumn)
                    || !AreAssignable(tarCol.Type, valueColumn.Type))
                {
                    return false;
                }
            }

            return true;
        }
    }
}