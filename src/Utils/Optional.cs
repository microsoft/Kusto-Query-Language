using System;

namespace Kusto.Language.Utils
{
    internal struct Optional<T>
    {
        public T Value { get; }

        public bool HasValue { get; }

        public Optional(T value)
        {
            this.Value = value;
            this.HasValue = true;
        }

        public static implicit operator Optional<T> (T value)
        {
            return new Optional<T>(value);
        }
    }
}