using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    using Syntax;
    using Utils;

    /// <summary>
    /// A symbol representing a type that is any scalar type, 
    /// and is stored dynamic/json column.
    /// </summary>
    public abstract class DynamicSymbol : ScalarSymbol
    {
        protected DynamicSymbol(string name)
            : base(name)
        {
        }
    }

    /// <summary>
    /// A symbol that represents any scalar primitive stored in a dynamic column.
    /// </summary>
    public sealed class DynamicAnySymbol : DynamicSymbol
    {
        public override SymbolKind Kind => SymbolKind.Primitive;
        
        private DynamicAnySymbol()
            : base($"dynamic")
        {
        }

        public static readonly DynamicAnySymbol Instance = new DynamicAnySymbol();
    }


    /// <summary>
    /// A symbol that represents any scalar primitive stored in a dynamic column.
    /// </summary>
    public sealed class DynamicPrimitiveSymbol : DynamicSymbol
    {
        public ScalarSymbol UnderlyingType { get; }

        public override SymbolKind Kind => SymbolKind.Primitive;

        internal DynamicPrimitiveSymbol(ScalarSymbol underlyingType)
            : base("dynamic")
        {
            this.UnderlyingType = underlyingType;
        }
    }

    /// <summary>
    /// A symbol representing an array of values stored in a dynamic column.
    /// </summary>
    public sealed class DynamicArraySymbol : DynamicSymbol
    {
        public TypeSymbol ElementType { get; }

        public override SymbolKind Kind => SymbolKind.Array;

        internal DynamicArraySymbol(TypeSymbol elementType)
            : base("dynamic")
        {
            this.ElementType = elementType;
        }
    }

    /// <summary>
    /// A symbol representing a bag of properties (aka json object),
    /// stored in a dynamic column.
    /// </summary>
    public sealed class DynamicBagSymbol : DynamicSymbol
    {
        public IReadOnlyList<ColumnSymbol> Properties { get; }

        public override IReadOnlyList<Symbol> Members => this.Properties;

        public override SymbolKind Kind => SymbolKind.Bag;

        internal DynamicBagSymbol(IEnumerable<ColumnSymbol> properties)
            : base("dynamic")
        {
            this.Properties = properties
                .ToReadOnly()
                .CheckArgumentNullOrElementNull(nameof(properties));
        }

        public DynamicBagSymbol(params ColumnSymbol[] properties)
            : this((IEnumerable<ColumnSymbol>)properties)
        {
        }

        public override Tabularity Tabularity => Tabularity.Scalar;

        private Dictionary<string, ColumnSymbol> _nameToPropertyMap;

        /// <summary>
        /// Gets the property with the specified name or null.
        /// </summary>
        public bool TryGetProperty(string name, out ColumnSymbol property)
        {
            if (_nameToPropertyMap == null)
            {
                var tmp = new Dictionary<string, ColumnSymbol>();

                foreach (var prop in this.Properties)
                {
                    tmp[prop.Name] = prop;
                }

                Interlocked.CompareExchange(ref _nameToPropertyMap, tmp, null);
            }

            return _nameToPropertyMap.TryGetValue(name, out property);
        }

        /// <summary>
        /// Returns a new <see cref="DynamicBagSymbol"/> instance with the specfied properties,
        /// if the properties are different than the current set of properties.
        /// </summary>
        public DynamicBagSymbol WithProperties(IEnumerable<ColumnSymbol> properties)
        {
            if (properties != this.Properties)
            {
                return new DynamicBagSymbol(properties);
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Returns a <see cref="DynamicBagSymbol"/> with the property added or updated.
        /// </summary>
        public DynamicBagSymbol AddOrUpdateProperty(ColumnSymbol property)
        {
            if (property == null)
                return this;

            if (TryGetProperty(property.Name, out var existingProperty))
            {
                // replace existing property with new property
                return new DynamicBagSymbol(this.Properties.Select(p => p == existingProperty ? property : existingProperty).ToList());
            }
            else
            {
                // add new property
                return new DynamicBagSymbol(this.Properties.Concat(new[] { property }));
            }
        }

        /// <summary>
        /// Returns a new <see cref="DynamicBagSymbol"/> instance with all properties
        /// modified to reference the specified source.
        /// </summary>
        public DynamicBagSymbol WithSource(SyntaxNode source)
        {
            return new DynamicBagSymbol(this.Properties.Select(p => p.WithSource(source)));
        }

        /// <summary>
        /// A dynamic object symbol with no known schema.
        /// </summary>
        public static DynamicBagSymbol Empty = new DynamicBagSymbol();
    }
}
