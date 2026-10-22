using System;
using System.Collections.Generic;

namespace Kusto.Language.Utils
{
    /// <summary>
    /// Compares strings using <see cref="CultureUtilities"/>, so that collections and sorts share the
    /// same definition of ordinal comparison as the rest of the language services rather than picking
    /// up <see cref="System.StringComparer"/>'s host-dependent behavior.
    /// </summary>
    internal sealed class CultureStringComparer : IComparer<string>, IEqualityComparer<string>
    {
        private readonly bool _ignoreCase;

        private CultureStringComparer(bool ignoreCase)
        {
            _ignoreCase = ignoreCase;
        }

        public static readonly CultureStringComparer Ordinal =
            new CultureStringComparer(ignoreCase: false);

        public static readonly CultureStringComparer OrdinalIgnoreCase =
            new CultureStringComparer(ignoreCase: true);

        public int Compare(string x, string y) =>
            CultureUtilities.Compare(x, y, _ignoreCase);

        public bool Equals(string x, string y) =>
            CultureUtilities.Equals(x, y, _ignoreCase);

        public int GetHashCode(string obj)
        {
            if (obj == null)
                return 0;

            // Equal strings must hash equally. Case folding only ever maps a scalar to its
            // upper-case form, so two strings that compare equal ignoring case always have the
            // same upper-case form, which makes this a valid (if coarser) hash.
            return _ignoreCase
                ? CultureUtilities.ToUpperInvariant(obj).GetHashCode()
                : obj.GetHashCode();
        }
    }
}
