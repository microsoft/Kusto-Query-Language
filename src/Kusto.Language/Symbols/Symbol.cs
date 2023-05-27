using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    using Utils;

    [System.Diagnostics.DebuggerDisplay("Symbol: {Kind} {DebugText}")]
    public abstract class Symbol
    {
        /// <summary>
        /// The text used to designate the symbol in the debugger
        /// </summary>
        private string DebugText =>
            DebugDisplay.GetText(this);

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
            if (targetType.IsScalar
                && sourceType.Kind == SymbolKind.Tuple 
                && sourceType is TupleSymbol stt
                && stt.Columns.Count == 1)
                return AreAssignable(targetType, stt.Columns[0].Type);

            if (targetType == ScalarTypes.Dynamic)
            {
                if (sourceType is DynamicSymbol)
                    return true;

                if (sourceType is ScalarSymbol
                    && allowedConversion >= Conversion.Dynamic)
                    return true;
            }

            if (targetType == ScalarTypes.DynamicArray
                && sourceType is DynamicArraySymbol
                && allowedConversion != Conversion.None)
                return true;

            if (targetType == ScalarTypes.DynamicBag
                && sourceType is DynamicBagSymbol
                && allowedConversion != Conversion.None)
                return true;

            if (targetType is DynamicPrimitiveSymbol tp
                && sourceType is DynamicPrimitiveSymbol sp)
                return AreAssignable(tp.UnderlyingType, sp.UnderlyingType, allowedConversion);

            if (targetType.Kind != sourceType.Kind)
                return false;

            switch (targetType)
            {
                case ColumnSymbol tarCol:
                    var srcCol = (ColumnSymbol)sourceType;
                    return tarCol.Name == srcCol.Name && AreAssignable(tarCol.Type, srcCol.Type, allowedConversion);

                case TupleSymbol _:
                case GroupSymbol _:
                    return AreMembersEqual(targetType, sourceType);

                case TableSymbol tarTable:
                    return AreTablesAssignable(tarTable, (TableSymbol)sourceType);

                case PrimitiveSymbol tarPrim:
                    var scrPrim = (ScalarSymbol)sourceType;

                    switch (allowedConversion)
                    {
                        case Conversion.Promotable:
                            return scrPrim.IsPromotableTo(tarPrim);
                        case Conversion.Compatible:
                            return scrPrim.IsPromotableTo(tarPrim)
                                || tarPrim.IsPromotableTo(scrPrim);
                        case Conversion.Any:
                            return true;
                        default:
                            return false;
                    }

                case DynamicArraySymbol tarArray:
                    var srcArray = (DynamicArraySymbol)sourceType;
                    return AreAssignable(srcArray.ElementType, tarArray.ElementType, allowedConversion);

                case DynamicBagSymbol tarBag:
                    var srcBag = (DynamicBagSymbol)sourceType;
                    return AreBagsAssignable(tarBag, srcBag);
            }

            return false;
        }

        /// <summary>
        /// True if the members of the source type are assignable to the members of the target type.
        /// </summary>
        private static bool AreMembersEqual(Symbol target, Symbol source)
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
        /// True if a bag value can be assigned to a parameter of specific bag type.
        /// </summary>
        private static bool AreBagsAssignable(DynamicBagSymbol targetBag, DynamicBagSymbol sourceBag)
        {
            // ensure that the source bag has at least the properties specified of the target bag.

            foreach (var targetProperty in targetBag.Properties)
            {
                if (!sourceBag.TryGetProperty(targetProperty.Name, out var sourceProperty)
                    || !AreAssignable(targetProperty.Type, sourceProperty.Type, Conversion.Any))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// True if a table value can be assigned to a parameter of a specific table type.
        /// </summary>
        private static bool AreTablesAssignable(TableSymbol target, TableSymbol source)
        {
            // ensure that the value table has at least the columns specified for the parameter table.

            foreach (var targetColumn in target.Columns)
            {
                if (!source.TryGetColumn(targetColumn.Name, out var sourceColumn)
                    || !AreAssignable(targetColumn.Type, sourceColumn.Type))
                {
                    return false;
                }
            }

            return true;
        }
    }
}