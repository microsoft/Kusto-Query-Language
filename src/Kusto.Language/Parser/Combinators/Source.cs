using System;

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

        /// <summary>
        /// Advances the current position in the source.
        /// </summary>
        public abstract void Eat(int n = 1);
    }
}