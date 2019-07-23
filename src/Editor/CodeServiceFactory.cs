using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Utils;

    /// <summary>
    /// A factory that constructs <see cref="CodeService"/> instances over blocks of code.
    /// </summary>
    public abstract class CodeServiceFactory
    {
        public abstract bool TryGetCodeService(string text, out CodeService service);

        /// <summary>
        /// .Gets a specific factory from the set of factories that compose this factory.
        /// </summary>
        public virtual TFactory GetFactory<TFactory>() where TFactory : CodeServiceFactory
        {
            if (this is TFactory)
            {
                return (TFactory)this;
            }

            return null;
        }

        /// <summary>
        /// Combine an additional factory with this factory.
        /// </summary>
        public virtual CodeServiceFactory WithFactory(CodeServiceFactory factory)
        {
            if (factory is AggregateCodeServiceFactory af)
            {
                return af.WithFactory(this);
            }
            else if (factory.GetType() == this.GetType())
            {
                // specified factory is a replacement for this factory
                return factory;
            }
            else
            {
                return new AggregateCodeServiceFactory(new[] { this }).WithFactory(factory);
            }
        }

        /// <summary>
        /// A <see cref="CodeServiceFactory"/> used when more than one kind of factory is needed.
        /// </summary>
        private class AggregateCodeServiceFactory : CodeServiceFactory
        {
            private readonly IReadOnlyList<CodeServiceFactory> _factories;

            public AggregateCodeServiceFactory(IReadOnlyList<CodeServiceFactory> factories)
            {
                _factories = factories;
            }

            public override bool TryGetCodeService(string text, out CodeService service)
            {
                foreach (var factory in _factories)
                {
                    if (factory.TryGetCodeService(text, out service))
                        return true;
                }

                service = null;
                return false;
            }

            public override TFactory GetFactory<TFactory>()
            {
                return (TFactory)_factories.FirstOrDefault(f => f is TFactory);
            }

            public override CodeServiceFactory WithFactory(CodeServiceFactory factory)
            {
                // if not changed, then return same aggregate
                if (_factories.Contains(factory))
                    return this;

                var list = new List<CodeServiceFactory>(_factories);

                var index = list.FindIndex(f => f.GetType() == factory.GetType());
                if (index >= 0)
                {
                    list[index] = factory;
                }
                else
                {
                    list.Add(factory);
                }

                return new AggregateCodeServiceFactory(list.ToReadOnly());
            }
        }
    }
}