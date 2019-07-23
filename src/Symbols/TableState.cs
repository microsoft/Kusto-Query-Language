using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    using Utils;

    /// <summary>
    /// The state of a <see cref="TableSymbol"/>
    /// </summary>
    [Flags]
    public enum TableState
    {
        None = 0,

        /// <summary>
        /// The table rows are serialized
        /// </summary>
        Serialized = 0b0000_0001,

        /// <summary>
        /// The table rows are sorted
        /// </summary>
        Sorted = 0b0000_0010,

        /// <summary>
        /// The table is open and may contain more columns than specified
        /// </summary>
        Open = 0b0000_0100,
    }

    public static class TableStateExtensions
    {
        public static bool Has(this TableState state, TableState test)
        {
            return (state & test) != 0;
        }

        public static TableState With(this TableState state, TableState stateToAdd)
        {
            return state | stateToAdd;
        }

        public static TableState Without(this TableState state, TableState stateToRemove)
        {
            return state & ~stateToRemove;
        }

        /// <summary>
        /// The table is new, but inherits any global state from the current state.
        /// </summary>
        public static TableState New(this TableState state)
        {
            // new tables still inherit global states like serialized
            return With(state, TableState.Serialized);
        }

        /// <summary>
        /// The table is open, and may have more columns than specified.
        /// </summary>
        public static TableState Open(this TableState state)
        {
            return With(state, TableState.Open);
        }

        /// <summary>
        /// The table is not sorted.
        /// </summary>
        public static TableState Unsorted(this TableState state)
        {
            return Without(state, TableState.Sorted);
        }
    }
}