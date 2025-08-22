using System;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    /// <summary>
    /// An abstraction over a sequence of input items.
    /// </summary>
    public abstract class Source<T>
    {
        /// <summary>
        /// Gets the nth item from the current position in the source.
        /// </summary>
        public abstract T Peek(int n = 0);

        /// <summary>
        /// Returns true if the position is beyond the last element.
        /// </summary>
        public abstract bool IsEnd(int n = 0);

        private SourceCache _cache;
        public SourceCache Cache
        {
            get
            {
                if (_cache == null)
                {
                    Utils.Interlocked.CompareExchange(ref _cache, new SourceCache(), null);
                }
                return _cache;
            }
        }
    }

    public class SourceCache
    {
        // cache does not need to be thread-safe since
        // it is only used by a single parser instance.
        private readonly Dictionary<Type, object> m_cache =
            new Dictionary<Type, object>();

        /// <summary>
        /// Gets or creates a new instance of <see cref="P:T"/>
        /// </summary>
        public T GetOrCreate<T>()
            where T : new()
        {
            if (!m_cache.TryGetValue(typeof(T), out var value))
            {
                value = m_cache[typeof(T)] = new T();
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
                value = m_cache[typeof(T)] = creator();
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