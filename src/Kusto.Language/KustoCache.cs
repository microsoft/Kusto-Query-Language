using System;

namespace Kusto.Language
{
    using Utils;

    public class KustoCache
    {
        public GlobalState Globals { get; }

        private readonly ThreadSafeDictionary<Type, object> m_cache =
            new ThreadSafeDictionary<Type, object>();

        public KustoCache(GlobalState globals)
        {
            this.Globals = globals;
        }

        public KustoCache WithGlobals(GlobalState globals)
        {
            if (this.Globals == globals)
            {
                return this;
            }
            else
            {
                return new KustoCache(globals);
            }
        }

        /// <summary>
        /// Gets or creates a new instance of <see cref="P:T"/>
        /// </summary>
        public T GetOrCreate<T>()
            where T : new()
        {
            if (!m_cache.TryGetValue(typeof(T), out var value))
            {
                value = m_cache.GetOrAdd(typeof(T), new T());
            }

            return (T)value;
        }

        /// <summary>
        /// Gets or creates a new instance of <see cref="P:T"/>
        /// </summary>
        public T GetOrCreate<T>(Func<T> creator)
        {
            if (!m_cache.TryGetValue(typeof(T), out var value))
            {
                var newValue = creator();
                value = m_cache.GetOrAdd(typeof(T), _ => creator());
            }

            return (T)value;
        }

        /// <summary>
        /// Gets the value associated with the type or returns false.
        /// </summary>
        public bool TryGetValue<T>(out T value)
        {
            if (m_cache.TryGetValue(typeof(T), out var obj))
            {
                value = (T)obj;
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }
    }
}