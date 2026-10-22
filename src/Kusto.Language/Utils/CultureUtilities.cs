using System;
using System.Globalization;

namespace Kusto.Language.Utils
{
    internal static class CultureUtilities
    {
#if !BRIDGE
        /// <summary>
        /// True when the host's OrdinalIgnoreCase folds case per UTF-16 code unit and therefore does
        /// not fold supplementary-plane scalars (NLS, used by .NET Framework and by .NET Core on
        /// Windows before .NET 5). Detected at runtime rather than from the compilation target,
        /// because the same assembly can be loaded on hosts that differ here - the netstandard2.0
        /// asset of this package is consumed by .NET Framework, where no compile-time symbol would
        /// tell us which behavior applies.
        /// </summary>
        private static readonly bool s_needsSupplementaryCaseFolding =
            !string.Equals("\ud801\udc00", "\ud801\udc28", StringComparison.OrdinalIgnoreCase);
#endif

#if BRIDGE
        [Bridge.Template("{value}.toLowerCase()")]
#endif
        public static string ToLowerInvariant(string value)
        {
#if BRIDGE
            // Bridge maps string casing to JavaScript's locale-independent casing APIs.
            return value.ToLower();
#else
            return value.ToLowerInvariant();
#endif
        }

#if BRIDGE
        [Bridge.Template("{value}.toUpperCase()")]
#endif
        public static string ToUpperInvariant(string value)
        {
#if BRIDGE
            // Bridge maps string casing to JavaScript's locale-independent casing APIs.
            return value.ToUpper();
#else
            return value.ToUpperInvariant();
#endif
        }

        public static int Compare(string left, string right, bool ignoreCase = false)
        {
#if BRIDGE
            if (ignoreCase && left != null && right != null)
                return CompareOrdinalIgnoreCaseBridge(left, 0, left.Length, right, 0, right.Length);
#endif

            var result = string.Compare(
                left,
                right,
                ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);

#if !BRIDGE
            // Keep ordering consistent with Equals, which folds supplementary-plane scalars on hosts
            // whose OrdinalIgnoreCase does not. Without this, the same comparer would report two
            // strings equal while ordering them apart.
            if (result != 0
                && ignoreCase
                && s_needsSupplementaryCaseFolding
                && left != null
                && right != null
                && (ContainsHighSurrogate(left, 0, left.Length)
                    || ContainsHighSurrogate(right, 0, right.Length)))
            {
                return CompareSupplementaryOrdinalIgnoreCase(left, 0, left.Length, right, 0, right.Length, result);
            }
#endif

            return result;
        }

        public static int Compare(
            string left,
            int leftIndex,
            string right,
            int rightIndex,
            int length,
            bool ignoreCase = false)
        {
            var result = string.Compare(
                left,
                leftIndex,
                right,
                rightIndex,
                length,
                ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);

#if BRIDGE
            if (ignoreCase
                && left != null
                && right != null
                && leftIndex >= 0
                && rightIndex >= 0
                && length >= 0
                && length <= left.Length - leftIndex
                && length <= right.Length - rightIndex)
            {
                return CompareOrdinalIgnoreCaseBridge(left, leftIndex, length, right, rightIndex, length);
            }
#endif

#if !BRIDGE
            if (result != 0
                && ignoreCase
                && s_needsSupplementaryCaseFolding
                && left != null
                && right != null
                && leftIndex >= 0
                && rightIndex >= 0
                && length >= 0
                && length <= left.Length - leftIndex
                && length <= right.Length - rightIndex
                && (ContainsHighSurrogate(left, leftIndex, length)
                    || ContainsHighSurrogate(right, rightIndex, length)))
            {
                return CompareSupplementaryOrdinalIgnoreCase(left, leftIndex, length, right, rightIndex, length, result);
            }
#endif

            return result;
        }

        public static bool Equals(string left, string right, bool ignoreCase = false)
        {
            if (!ignoreCase)
                return string.Equals(left, right, StringComparison.Ordinal);

            if (left == null || right == null)
                return ReferenceEquals(left, right);

            // Ordinal case folding is scalar-wise, so it never changes the number of UTF-16 code units.
            return left.Length == right.Length
                && Equals(left, 0, right, 0, left.Length, ignoreCase: true);
        }

        public static bool EqualsOrdinalIgnoreCase(char left, char right)
        {
            if (left == right)
                return true;

            if (left <= 0x7f && right <= 0x7f)
            {
                var leftKey = left >= 'a' && left <= 'z' ? (char)(left - ('a' - 'A')) : left;
                var rightKey = right >= 'a' && right <= 'z' ? (char)(right - ('a' - 'A')) : right;
                return leftKey == rightKey;
            }

#if BRIDGE
            return GetOrdinalIgnoreCaseKey(left) == GetOrdinalIgnoreCaseKey(right);
#else
            return string.Equals(left.ToString(), right.ToString(), StringComparison.OrdinalIgnoreCase);
#endif
        }

        /// <summary>
        /// Compares <paramref name="length"/> characters of both strings starting at the given indexes.
        /// Returns false when the requested range does not fit within either string, so that callers
        /// never have to pre-validate the range (string.Compare silently compares a shorter span instead,
        /// and the BRIDGE path indexes the strings directly).
        /// </summary>
        public static bool Equals(
            string left,
            int leftIndex,
            string right,
            int rightIndex,
            int length,
            bool ignoreCase = false)
        {
            if (left == null || right == null)
                return ReferenceEquals(left, right);

            if (leftIndex < 0
                || rightIndex < 0
                || length < 0
                || length > left.Length - leftIndex
                || length > right.Length - rightIndex)
            {
                return false;
            }

#if BRIDGE
            if (ignoreCase)
                return EqualsOrdinalIgnoreCaseBridge(left, leftIndex, right, rightIndex, length);
#endif

            var equal = Compare(left, leftIndex, right, rightIndex, length, ignoreCase) == 0;

#if !BRIDGE
            // Where the host does not fold supplementary-plane scalars, fold them ourselves so that
            // every target agrees (for example the Deseret pair U+10400/U+10428). Folding can only
            // turn a non-match into a match, so this only has to run once the ordinal comparison has
            // already said "not equal" and a surrogate is actually present.
            if (!equal
                && ignoreCase
                && s_needsSupplementaryCaseFolding
                && ContainsHighSurrogate(left, leftIndex, length))
            {
                return EqualsSupplementaryOrdinalIgnoreCase(left, leftIndex, right, rightIndex, length);
            }
#endif

            return equal;
        }

#if !BRIDGE
        /// <summary>
        /// Compares two spans by first mapping them to their invariant upper case, which does fold
        /// supplementary-plane scalars on hosts whose OrdinalIgnoreCase does not. Falls back to the
        /// host's own answer if upper casing changes a length, since the spans no longer correspond.
        /// </summary>
        private static int CompareSupplementaryOrdinalIgnoreCase(
            string left,
            int leftIndex,
            int leftLength,
            string right,
            int rightIndex,
            int rightLength,
            int fallback)
        {
            var upperLeft = left.Substring(leftIndex, leftLength).ToUpperInvariant();
            var upperRight = right.Substring(rightIndex, rightLength).ToUpperInvariant();

            if (upperLeft.Length != leftLength || upperRight.Length != rightLength)
                return fallback;

            return string.Compare(upperLeft, upperRight, StringComparison.OrdinalIgnoreCase);
        }

        private static bool ContainsHighSurrogate(string text, int index, int length)
        {
            for (var offset = 0; offset < length; offset++)
            {
                var value = text[index + offset];
                if (value >= '\ud800' && value <= '\udbff')
                    return true;
            }

            return false;
        }

        private static bool EqualsSupplementaryOrdinalIgnoreCase(
            string left,
            int leftIndex,
            string right,
            int rightIndex,
            int length)
        {
            for (var offset = 0; offset < length;)
            {
                var leftIsPair = IsSupplementaryPairAt(left, leftIndex + offset, leftIndex + length);
                var rightIsPair = IsSupplementaryPairAt(right, rightIndex + offset, rightIndex + length);
                if (leftIsPair != rightIsPair)
                    return false;

                if (!leftIsPair)
                {
                    if (!EqualsOrdinalIgnoreCase(left[leftIndex + offset], right[rightIndex + offset]))
                        return false;

                    offset++;
                    continue;
                }

                // Invariant uppercasing does map supplementary scalars on hosts whose
                // OrdinalIgnoreCase does not.
                if ((left[leftIndex + offset] != right[rightIndex + offset]
                        || left[leftIndex + offset + 1] != right[rightIndex + offset + 1])
                    && !string.Equals(
                        left.Substring(leftIndex + offset, 2).ToUpperInvariant(),
                        right.Substring(rightIndex + offset, 2).ToUpperInvariant(),
                        StringComparison.Ordinal))
                {
                    return false;
                }

                offset += 2;
            }

            return true;
        }

        private static bool IsSupplementaryPairAt(string text, int index, int end)
        {
            return index + 1 < end
                && (text[index] & 0xfc00) == 0xd800
                && (text[index + 1] & 0xfc00) == 0xdc00;
        }
#endif

#if BRIDGE
        private static int CompareOrdinalIgnoreCaseBridge(
            string left,
            int leftIndex,
            int leftLength,
            string right,
            int rightIndex,
            int rightLength)
        {
            var commonLength = Math.Min(leftLength, rightLength);
            for (var offset = 0; offset < commonLength; offset++)
            {
                if (left[leftIndex + offset] == right[rightIndex + offset])
                    continue;

                var leftKey = GetOrdinalIgnoreCaseKeyAt(left, leftIndex + offset, leftIndex, leftIndex + leftLength);
                var rightKey = GetOrdinalIgnoreCaseKeyAt(right, rightIndex + offset, rightIndex, rightIndex + rightLength);
                if (leftKey != rightKey)
                    return leftKey < rightKey ? -1 : 1;
            }

            return leftLength.CompareTo(rightLength);
        }

        private static char GetOrdinalIgnoreCaseKeyAt(string text, int index, int start, int end)
        {
            if (text[index] >= '\ud800'
                && text[index] <= '\udbff'
                && index + 1 < end
                && text[index + 1] >= '\udc00'
                && text[index + 1] <= '\udfff')
            {
                return GetOrdinalIgnoreCaseKey(text.Substring(index, 2))[0];
            }

            if (index > start
                && text[index] >= '\udc00'
                && text[index] <= '\udfff'
                && text[index - 1] >= '\ud800'
                && text[index - 1] <= '\udbff')
            {
                return GetOrdinalIgnoreCaseKey(text.Substring(index - 1, 2))[1];
            }

            return GetOrdinalIgnoreCaseKey(text[index]);
        }

        // Bridge implements OrdinalIgnoreCase with JavaScript's full-string casing, which
        // equates some characters that .NET ordinal casing keeps distinct. Compare one
        // Unicode scalar at a time and accept only one-scalar, non-ASCII case mappings.
        private static bool EqualsOrdinalIgnoreCaseBridge(
            string left,
            int leftIndex,
            string right,
            int rightIndex,
            int length)
        {
            // ECMAScript case mappings for supplementary scalars remain one supplementary
            // scalar, so direct ordinal comparison is equivalent to the scalar-key path.
            if (length == 2
                && IsSupplementaryScalarAt(left, leftIndex)
                && IsSupplementaryScalarAt(right, rightIndex))
            {
                var leftScalar = leftIndex == 0 && left.Length == length
                    ? left
                    : left.Substring(leftIndex, length);
                var rightScalar = rightIndex == 0 && right.Length == length
                    ? right
                    : right.Substring(rightIndex, length);
                return Compare(leftScalar, rightScalar, ignoreCase: true) == 0;
            }

            var requiresUnicodeComparison = false;
            for (var offset = 0; offset < length; offset++)
            {
                var leftChar = left[leftIndex + offset];
                var rightChar = right[rightIndex + offset];
                if (leftChar == rightChar)
                    continue;

                if (leftChar > 0x7f || rightChar > 0x7f)
                {
                    requiresUnicodeComparison = true;
                    break;
                }

                var leftKey = leftChar >= 'a' && leftChar <= 'z'
                    ? (char)(leftChar - ('a' - 'A'))
                    : leftChar;
                var rightKey = rightChar >= 'a' && rightChar <= 'z'
                    ? (char)(rightChar - ('a' - 'A'))
                    : rightChar;
                if (leftKey != rightKey)
                    return false;
            }

            if (!requiresUnicodeComparison)
                return true;
            for (var offset = 0; offset < length;)
            {
                var leftScalarLength = GetScalarLength(left, leftIndex + offset, leftIndex + length);
                var rightScalarLength = GetScalarLength(right, rightIndex + offset, rightIndex + length);
                if (leftScalarLength != rightScalarLength)
                    return false;

                if (leftScalarLength == 1)
                {
                    var leftChar = left[leftIndex + offset];
                    var rightChar = right[rightIndex + offset];
                    if (leftChar != rightChar
                        && GetOrdinalIgnoreCaseKey(leftChar) != GetOrdinalIgnoreCaseKey(rightChar))
                    {
                        return false;
                    }
                }
                else
                {
                    var leftScalar = left.Substring(leftIndex + offset, leftScalarLength);
                    var rightScalar = right.Substring(rightIndex + offset, rightScalarLength);
                    var leftKey = GetOrdinalIgnoreCaseKey(leftScalar);
                    var rightKey = GetOrdinalIgnoreCaseKey(rightScalar);
                    if (Compare(leftKey, rightKey) != 0)
                        return false;
                }

                offset += leftScalarLength;
            }

            return true;
        }

        private static int GetScalarLength(string text, int index, int end)
        {
            return text[index] >= '\ud800'
                && text[index] <= '\udbff'
                && index + 1 < end
                && text[index + 1] >= '\udc00'
                && text[index + 1] <= '\udfff'
                    ? 2
                    : 1;
        }

        [Bridge.Template(
            "({index} + 1 < {text}.length "
            + "&& ({text}.charCodeAt({index}) & 0xFC00) === 0xD800 "
            + "&& ({text}.charCodeAt({index} + 1) & 0xFC00) === 0xDC00)")]
        private static bool IsSupplementaryScalarAt(string text, int index)
        {
            return index + 1 < text.Length
                && (text[index] & 0xfc00) == 0xd800
                && (text[index + 1] & 0xfc00) == 0xdc00;
        }

        private static char GetOrdinalIgnoreCaseKey(char value)
        {
            if (value <= 0x7f)
            {
                return value >= 'a' && value <= 'z'
                    ? (char)(value - ('a' - 'A'))
                    : value;
            }

            var text = value.ToString();
            var upper = ToUpperInvariant(text);
            if (upper.Length == 1 && upper[0] > 0x7f)
                return upper[0];

            var lower = ToLowerInvariant(text);
            if (lower.Length == 1 && lower[0] > 0x7f)
                return lower[0];

            // .NET ordinal casing does not fold non-ASCII characters to ASCII.
            return value;
        }

        private static string GetOrdinalIgnoreCaseKey(string value)
        {
            var upper = ToUpperInvariant(value);
            if (upper.Length == value.Length && ContainsOnlyNonAscii(upper))
                return upper;

            var lower = ToLowerInvariant(value);
            if (lower.Length == value.Length && ContainsOnlyNonAscii(lower))
                return lower;

            return value;
        }

        private static bool ContainsOnlyNonAscii(string value)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (value[i] <= 0x7f)
                    return false;
            }

            return true;
        }
#endif

        public static bool StartsWith(string text, string value, bool ignoreCase = false)
        {
            // Route case-insensitive matching through Equals so every target shares one definition
            // of ordinal case folding.
            if (ignoreCase && text != null && value != null)
                return Equals(text, 0, value, 0, value.Length, ignoreCase: true);

            return text.StartsWith(
                value,
                ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        }

        public static bool EndsWith(string text, string value, bool ignoreCase = false)
        {
            if (ignoreCase && text != null && value != null)
                return Equals(text, text.Length - value.Length, value, 0, value.Length, ignoreCase: true);

            return text.EndsWith(
                value,
                ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        }

#if BRIDGE
        [Bridge.Template("{text}.indexOf({value}, {startIndex})")]
#endif
        public static int IndexOf(string text, string value, int startIndex = 0)
        {
#if BRIDGE
            // JavaScript's indexOf is always ordinal.
            return text.IndexOf(value, startIndex);
#else
            return text.IndexOf(value, startIndex, StringComparison.Ordinal);
#endif
        }

        /// <summary>
        /// Returns the index of the first ordinal occurrence of the
        /// <paramref name="valueLength"/> characters of <paramref name="value"/> starting at
        /// <paramref name="valueStart"/>, searching <paramref name="text"/> from
        /// <paramref name="textStart"/>, or -1 if there is none.
        /// </summary>
        public static int IndexOf(
            string text,
            int textStart,
            string value,
            int valueStart,
            int valueLength,
            bool ignoreCase = false)
        {
            var firstChar = value[valueStart];
            var lastStart = text.Length - valueLength;

            if (!ignoreCase)
            {
                // string.IndexOf(char, int) is ordinal on every target, and is the fastest way to
                // skip over the positions that cannot start a match.
                while (textStart <= lastStart)
                {
                    var position = text.IndexOf(firstChar, textStart);
                    if (position < 0 || position > lastStart)
                        return -1;

                    if (Equals(text, position, value, valueStart, valueLength))
                        return position;

                    textStart = position + 1;
                }

                return -1;
            }

            for (var position = textStart; position <= lastStart; position++)
            {
                // Filter on the first character before paying for the full comparison (perf).
                if (EqualsOrdinalIgnoreCase(text[position], firstChar)
                    && Equals(text, position, value, valueStart, valueLength, ignoreCase: true))
                {
                    return position;
                }
            }

            return -1;
        }

        public static bool TryParseInt32Invariant(string text, out int value)
        {
#if BRIDGE
            return Int32.TryParse(text, out value);
#else
            return Int32.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
#endif
        }

        public static bool TryParseInt64Invariant(string text, out long value)
        {
#if BRIDGE
            return Int64.TryParse(text, out value);
#else
            return Int64.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
#endif
        }

        /// <summary>
        /// Parses a real/double *literal* the way the engine does, where ',' is a group separator:
        /// real(1,5) is 15 and real(1,234,567) is 1234567. Other numeric text (timespan components,
        /// client directive arguments) must use <see cref="TryParseDoubleInvariant"/>, which rejects
        /// group separators, because the engine rejects them there.
        /// </summary>
        public static bool TryParseRealLiteralInvariant(string text, out double value)
        {
#if BRIDGE
            if (!TryRemoveRealLiteralGroupSeparators(text, out var digits))
            {
                value = default(double);
                return false;
            }

            return Double.TryParse(digits, CultureInfo.InvariantCulture, out value);
#else
            return Double.TryParse(
                text,
                NumberStyles.Float | NumberStyles.AllowThousands,
                CultureInfo.InvariantCulture,
                out value);
#endif
        }

        public static bool TryParseDoubleInvariant(string text, out double value)
        {
            if (ContainsInvariantGroupSeparator(text))
            {
                value = default(double);
                return false;
            }

#if BRIDGE
            return Double.TryParse(text, CultureInfo.InvariantCulture, out value);
#else
            return Double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
#endif
        }

        public static bool TryParseDecimalInvariant(string text, out decimal value)
        {
            // Unlike real, the engine rejects decimal literals containing a group separator
            // (decimal(1,5) is a syntax error), so keep rejecting them here.
            if (ContainsInvariantGroupSeparator(text))
            {
                value = default(decimal);
                return false;
            }

#if BRIDGE
            return Decimal.TryParse(text, CultureInfo.InvariantCulture, out value);
#else
            var styles = NumberStyles.Number & ~NumberStyles.AllowThousands;
            return Decimal.TryParse(text, styles, CultureInfo.InvariantCulture, out value);
#endif
        }

        public static bool TryParseDateTimeInvariant(string text, out DateTime value)
        {
#if BRIDGE
            var currentCulture = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
                return DateTime.TryParse(text, out value);
            }
            finally
            {
                CultureInfo.CurrentCulture = currentCulture;
            }
#else
            return DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.None, out value);
#endif
        }

        public static bool TryParseTimeSpanInvariant(string text, out TimeSpan value)
        {
#if BRIDGE
            value = default(TimeSpan);
            return false;
#else
            return TimeSpan.TryParse(text, CultureInfo.InvariantCulture, out value);
#endif
        }

        public static string ToInvariantString(int value, string format)
        {
            return value.ToString(format, CultureInfo.InvariantCulture);
        }

        public static string ToInvariantString(object value)
        {
            return value is IFormattable formattable
                ? formattable.ToString(null, CultureInfo.InvariantCulture)
                : value.ToString();
        }

        private static bool ContainsInvariantGroupSeparator(string text)
        {
            return text.IndexOf(',') >= 0;
        }

#if BRIDGE
        private static readonly char[] s_realIntegralPartTerminators = new[] { '.', 'e', 'E' };

        // Bridge's Double.TryParse has no NumberStyles overload, so apply AllowThousands' rule here:
        // a group separator must continue the integral part's digit run, so "1,5", "1,,5" and "1,.5"
        // are accepted while ",5", "1 ,5", "1.5,3" and "1e3,5" are rejected.
        private static bool TryRemoveRealLiteralGroupSeparators(string text, out string result)
        {
            var firstSeparator = text.IndexOf(',');
            if (firstSeparator < 0)
            {
                result = text;
                return true;
            }

            var integralEnd = text.IndexOfAny(s_realIntegralPartTerminators);
            if (integralEnd < 0)
                integralEnd = text.Length;

            for (var i = firstSeparator; i >= 0; i = text.IndexOf(',', i + 1))
            {
                if (i >= integralEnd || i == 0 || !IsDigitOrGroupSeparator(text[i - 1]))
                {
                    result = null;
                    return false;
                }
            }

            result = text.Replace(",", "");
            return true;
        }

        private static bool IsDigitOrGroupSeparator(char ch)
        {
            return (ch >= '0' && ch <= '9') || ch == ',';
        }
#endif
    }
}
