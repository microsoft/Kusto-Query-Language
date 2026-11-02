using System;

namespace Kusto.Language.Parsing
{
    /// <summary>
    /// How the text of a datetime literal was interpreted by <see cref="KustoDateTimeParser"/>.
    /// </summary>
    internal enum DateTimeInterpretation
    {
        /// <summary>
        /// The text is not a valid datetime literal.
        /// </summary>
        Invalid,

        /// <summary>
        /// The text is the <c>min</c> or <c>max</c> keyword.
        /// </summary>
        MinMax,

        /// <summary>
        /// The text is the <c>now</c> keyword. This is a valid literal, but it has no constant value.
        /// </summary>
        Now,

        /// <summary>
        /// The text is the <c>null</c> keyword, as in <c>datetime('null')</c>. This is a valid literal whose
        /// value is null, so it has no constant value.
        /// </summary>
        Null,

        /// <summary>
        /// The text is a fixed width form selected purely by its length, such as <c>yyyy</c>,
        /// <c>yyyyMMdd</c> or <c>yyyyMMdd HH:mm:ss</c>.
        /// </summary>
        FixedWidth,

        /// <summary>
        /// The text is a number that was interpreted as a count of 100 nanosecond ticks since year one.
        /// </summary>
        Ticks,

        /// <summary>
        /// The text is a number that was interpreted as a count of seconds since 1970-01-01.
        /// </summary>
        EpochSeconds,

        /// <summary>
        /// The text was interpreted by the general purpose date/time parser.
        /// </summary>
        General,
    }

    /// <summary>
    /// Parses the text of a <c>datetime(...)</c> literal the same way the Kusto engine parses it.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The engine parses datetime literals (and constant folded <c>todatetime</c> arguments) with
    /// <c>Kusto.Cloud.Platform.Time.ExtendedDateTime.TryParseInexactUtc(value, allowEpochTime: true)</c>,
    /// reached via <c>Kusto.Data.Common.CslDateTimeLiteral.TryParse</c>. This type reproduces those rules so
    /// that the value reported here agrees with the value the engine computes.
    /// </para>
    /// <para>
    /// Those rules are quirky by design, and reproducing them is the point: a bare number is a year at four
    /// digits, a tick count when it lands in year 1900 or later, and a count of seconds since 1970 otherwise.
    /// Do not "fix" the quirks here. Diverging from the engine is the bug this type exists to prevent, and
    /// <c>DateTimeLiteralParityTests</c> asserts the agreement.
    /// </para>
    /// <para>
    /// The per row <c>todatetime</c> conversion is a different parser again, implemented natively in
    /// <c>Src/Engine/Libs/datetime/DateTimeParser.cpp</c>, and is NOT modelled here.
    /// </para>
    /// </remarks>
    internal static class KustoDateTimeParser
    {
        /// <summary>
        /// Ticks at 1970-01-01, the epoch used when a number is interpreted as a count of seconds.
        /// </summary>
        private const long EpochTicks = 621355968000000000L;

        /// <summary>
        /// A number is interpreted as a tick count when doing so lands at or after this instant
        /// (1900-01-01 UTC), and as a count of seconds since the epoch otherwise.
        /// </summary>
        /// <remarks>
        /// Expressed in ticks rather than as a year on purpose. Comparing <see cref="DateTime.Year"/> against
        /// 1900 would read the year in the local time zone once transpiled to JavaScript, so a value in the
        /// last hours of 1899 UTC would be treated as 1900 and take the wrong branch.
        /// </remarks>
        private const long MinTicksInterpretedAsTicks = 599266080000000000L;

        /// <summary>
        /// The largest count of seconds since the epoch that still lands inside the datetime range.
        /// </summary>
        private const long MaxEpochSeconds = (MaxTicks - EpochTicks) / 10000000L;

        private static readonly int[] s_daysToMonth365 =
            new[] { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365 };

        private static readonly int[] s_daysToMonth366 =
            new[] { 0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366 };

        private const long TicksPerSecond = 10000000L;
        private const long TicksPerMinute = TicksPerSecond * 60;
        private const long TicksPerHour = TicksPerMinute * 60;
        private const long TicksPerDay = TicksPerHour * 24;
        private const long MaxTicks = 3155378975999999999L;
        private const int MaxOffsetMinutes = 14 * 60;

        /// <summary>
        /// Splits a datetime into its UTC calendar parts.
        /// </summary>
        /// <remarks>
        /// This works from the tick count rather than from <see cref="DateTime.Year"/> and friends, because
        /// those properties apply the local time zone when this code is transpiled to JavaScript. That shifts
        /// the reported parts, and for dates before standard time was adopted it shifts them by a whole
        /// number of seconds rather than hours.
        /// </remarks>
        public static void GetUtcParts(
            DateTime value,
            out int year, out int month, out int day,
            out int hour, out int minute, out int second)
        {
            var ticks = value.Ticks;

            var days = (int)(ticks / TicksPerDay);
            var timeOfDay = ticks % TicksPerDay;

            hour = (int)(timeOfDay / TicksPerHour);
            minute = (int)((timeOfDay / TicksPerMinute) % 60);
            second = (int)((timeOfDay / TicksPerSecond) % 60);

            var n400 = days / 146097;
            days -= n400 * 146097;
            var n100 = days / 36524;
            if (n100 == 4) n100 = 3;
            days -= n100 * 36524;
            var n4 = days / 1461;
            days -= n4 * 1461;
            var n1 = days / 365;
            if (n1 == 4) n1 = 3;
            days -= n1 * 365;

            year = n400 * 400 + n100 * 100 + n4 * 4 + n1 + 1;

            var months = (n1 == 3 && (n4 != 24 || n100 == 3)) ? s_daysToMonth366 : s_daysToMonth365;
            month = 1;
            while (month < 12 && days >= months[month])
                month++;

            day = days - months[month - 1] + 1;
        }

        /// <summary>
        /// Counts the ticks of a UTC instant given as calendar parts, without going through the platform's
        /// calendar conversion, which is local time zone sensitive once transpiled to JavaScript.
        /// </summary>
        /// <remarks>
        /// This returns ticks rather than a datetime because a rounded up fraction can carry the result past
        /// the end of year 9999, and the caller has to be able to see that rather than have it throw.
        /// </remarks>
        private static long ToUtcTicks(int year, int month, int day, int hour, int minute, int second, int fractionTicks)
        {
            var y = year - 1;
            var months = IsLeapYear(year) ? s_daysToMonth366 : s_daysToMonth365;
            long days = y * 365 + y / 4 - y / 100 + y / 400 + months[month - 1] + (day - 1);

            return days * TicksPerDay
                + hour * TicksPerHour
                + minute * TicksPerMinute
                + second * TicksPerSecond
                + fractionTicks;
        }

        /// <summary>
        /// Parses the value text of a datetime literal.
        /// </summary>
        /// <param name="text">The literal's value text, with any quoting already removed.</param>
        /// <param name="value">
        /// The constant value of the literal, or the default when the literal has none. Only meaningful when
        /// the result satisfies <see cref="HasConstantValue"/>.
        /// </param>
        /// <returns>How <paramref name="text"/> was interpreted.</returns>
        /// <remarks>
        /// This deliberately does not follow the TryParse shape. A literal can be valid yet have no constant
        /// value - <c>datetime(now)</c> is the case - so a bool result would have to report that valid literal
        /// as a failure, and every caller would have to know to check the interpretation to tell the two apart.
        /// </remarks>
        public static DateTimeInterpretation Parse(string text, out DateTime value)
        {
            value = default(DateTime);

            if (string.IsNullOrEmpty(text))
                return DateTimeInterpretation.Invalid;

            var trimmed = text.Trim();
            if (trimmed.Length == 0)
                return DateTimeInterpretation.Invalid;

            // These keywords are matched case sensitively, as the service does, so 'NULL' falls through to the
            // remaining rules rather than being read as null.
            switch (trimmed)
            {
                case "null":
                    return DateTimeInterpretation.Null;
                case "min":
                    value = DateTime.MinValue;
                    return DateTimeInterpretation.MinMax;
                case "max":
                    value = DateTime.MaxValue;
                    return DateTimeInterpretation.MinMax;
            }

            // The engine resolves 'now' to the current time, so the literal is valid but has no constant value.
            // Reporting a value here would make it depend on when the query text happened to be parsed.
            if (IsNowKeyword(trimmed))
                return DateTimeInterpretation.Now;

            if (TryParseFixedWidth(trimmed, out value))
                return DateTimeInterpretation.FixedWidth;

            long number;
            if (TryParseInt64(trimmed, out number))
                return ParseNumber(number, out value);

            if (TryParseGeneral(trimmed, out value))
                return DateTimeInterpretation.General;

            value = default(DateTime);
            return DateTimeInterpretation.Invalid;
        }

        /// <summary>
        /// True when an interpretation carries a constant value, as opposed to an invalid literal or one
        /// whose value is only known at execution time.
        /// </summary>
        public static bool HasConstantValue(DateTimeInterpretation interpretation)
        {
            return interpretation != DateTimeInterpretation.Invalid
                && interpretation != DateTimeInterpretation.Now
                && interpretation != DateTimeInterpretation.Null;
        }

        /// <summary>
        /// Interprets a bare number as either a tick count or a count of seconds since the epoch.
        /// </summary>
        private static DateTimeInterpretation ParseNumber(long number, out DateTime value)
        {
            value = default(DateTime);

            // The engine constructs a DateTime from the number without guarding the range, so an out of range
            // number throws and the query fails to parse. Report it as invalid, which is what the user sees.
            if (number < 0 || number > MaxTicks)
                return DateTimeInterpretation.Invalid;

            if (number >= MinTicksInterpretedAsTicks)
            {
                value = new DateTime(number, DateTimeKind.Utc);
                return DateTimeInterpretation.Ticks;
            }

            // Same unguarded construction, this time of epoch plus seconds.
            if (number > MaxEpochSeconds)
                return DateTimeInterpretation.Invalid;

            value = new DateTime(EpochTicks + number * TicksPerSecond, DateTimeKind.Utc);
            return DateTimeInterpretation.EpochSeconds;
        }

        /// <summary>
        /// Parses the fixed width forms the engine selects by the text's length alone.
        /// </summary>
        private static bool TryParseFixedWidth(string text, out DateTime value)
        {
            value = default(DateTime);

            int year, month = 1, day = 1, hour = 0, minute = 0, second = 0;
            var fractionTicks = 0;

            switch (text.Length)
            {
                // yyyy, yyyyMM, yyyyMMdd, yyyyMMddHH, yyyyMMddHHmm, yyyyMMddHHmmss
                case 4:
                case 6:
                case 8:
                case 10:
                case 12:
                case 14:
                    if (!AllDigits(text, 0, text.Length))
                        return false;

                    year = ToInt(text, 0, 4);
                    if (text.Length >= 6) month = ToInt(text, 4, 2);
                    if (text.Length >= 8) day = ToInt(text, 6, 2);
                    if (text.Length >= 10) hour = ToInt(text, 8, 2);
                    if (text.Length >= 12) minute = ToInt(text, 10, 2);
                    if (text.Length >= 14) second = ToInt(text, 12, 2);
                    break;

                // yyyyMMdd HH:mm:ss, optionally followed by one to seven fractional digits.
                // Note that length 18 is absent, matching the engine: there is no format for a
                // trailing '.' with no digits after it.
                case 17:
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                    if (!AllDigits(text, 0, 8)
                        || text[8] != ' '
                        || !AllDigits(text, 9, 2)
                        || text[11] != ':'
                        || !AllDigits(text, 12, 2)
                        || text[14] != ':'
                        || !AllDigits(text, 15, 2))
                        return false;

                    year = ToInt(text, 0, 4);
                    month = ToInt(text, 4, 2);
                    day = ToInt(text, 6, 2);
                    hour = ToInt(text, 9, 2);
                    minute = ToInt(text, 12, 2);
                    second = ToInt(text, 15, 2);

                    if (text.Length > 17)
                    {
                        if (text[17] != '.')
                            return false;

                        var digits = text.Length - 18;
                        if (!AllDigits(text, 18, digits))
                            return false;

                        fractionTicks = ToInt(text, 18, digits);
                        for (var i = digits; i < 7; i++)
                            fractionTicks *= 10;
                    }
                    break;

                default:
                    return false;
            }

            if (!IsValidDate(year, month, day, hour, minute, second))
                return false;

            // At most seven fractional digits, so this cannot carry past the end of year 9999.
            value = new DateTime(ToUtcTicks(year, month, day, hour, minute, second, fractionTicks), DateTimeKind.Utc);
            return true;
        }

        /// <summary>
        /// The general purpose fallback, used for anything that is neither a fixed width form nor a number.
        /// </summary>
        /// <remarks>
        /// The common calendar forms are parsed here rather than by the platform, because the platform's
        /// parser assumes local time once transpiled to JavaScript and so shifts every unqualified literal by
        /// the browser's UTC offset. Anything not recognised here still falls through to the platform, which
        /// remains local time sensitive in JavaScript.
        /// </remarks>
        private static bool TryParseGeneral(string text, out DateTime value)
        {
            bool recognized;
            if (TryParseCalendar(text, out value, out recognized))
                return true;

            // The shape was a calendar form but the values were out of range, so this is simply invalid.
            // Falling through would hand it to the platform parser, and a JavaScript Date silently rolls
            // such dates over: 2020-02-30 would become 2020-03-01 rather than being rejected.
            if (recognized)
                return false;

            // Unrecognised suffixes and incomplete month-name dates still reach the platform. Reject
            // impossible calendar components before a JavaScript Date can roll them over.
            if (StartsWithOutOfRangeDate(text) || IsOutOfRangeMonthNameDate(text))
                return false;

#if !BRIDGE
            // The service parses with the invariant culture. It also has a separate branch for text ending in
            // 'Z' that only exists to preserve DateTimeKind, which AdjustToUniversal already does here.
            var parsed = DateTime.TryParse(
                text,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.AdjustToUniversal
                    | System.Globalization.DateTimeStyles.AssumeUniversal,
                out value);
#else
            // Bridge does not support the style aware overload, so this cannot assume UTC the way the service
            // does, but it can at least read the text with the invariant culture as the service does.
            var parsed = Utils.CultureUtilities.TryParseDateTimeInvariant(text, out value);
#endif

            // Bridge's DateTime is a JavaScript Date, which accepts dates the service rejects because they
            // fall outside the representable range, such as year zero. Reject those here so that both
            // platforms agree on which literals are valid.
            if (parsed && (value.Ticks < 0 || value.Ticks > MaxTicks))
            {
                value = default(DateTime);
                return false;
            }

            return parsed;
        }

        /// <summary>
        /// Parses year-first, month/day/year and English month-name dates, optionally followed by a weekday,
        /// time and UTC offset. The time after a 'T' follows the platform's stricter ISO 8601 rules.
        /// A literal with no offset is taken to be UTC, as the service does.
        /// </summary>
        /// <param name="text">The literal's value text.</param>
        /// <param name="value">The parsed value, when the text is one of these shapes and is in range.</param>
        /// <param name="recognized">
        /// True when the text matched one of these shapes, whether or not its values were in range. The caller
        /// uses this to tell "not one of my forms, try the platform" apart from "one of my forms, but invalid".
        /// </param>
        private static bool TryParseCalendar(string text, out DateTime value, out bool recognized)
        {
            value = default(DateTime);
            recognized = false;

            int year, month, day;
            var pos = 0;

            var isYearFirstDate = TryParseYearFirstDate(text, ref pos, out year, out month, out day);
            if (!isYearFirstDate)
            {
                pos = 0;
                if (!TryParseSlashDate(text, ref pos, out year, out month, out day))
                {
                    pos = 0;
                    if (!TryParseMonthNameDate(text, ref pos, out year, out month, out day))
                        return false;
                }
            }

            int hour = 0, minute = 0, second = 0, fractionTicks = 0, offsetMinutes = 0;
            var dateEnd = pos;
            var weekday = -1;

            // The platform reads a 'T' after the date, even after whitespace, as the start of an ISO 8601 time.
            // It parses that time strictly and only accepts it after a year first date, so 2020-01-01T1:2:3 and
            // 1/2/2020T12:30:45 are both invalid. A 'T' that starts a word is a day name instead.
            var timeMark = SkipWhitespace(text, pos);
            if (IsTimeMark(text, timeMark))
            {
                recognized = true;
                pos = timeMark + 1;

                if (!isYearFirstDate
                    || !TryParseIsoTime(text, ref pos, out hour, out minute, out second, out fractionTicks, out offsetMinutes))
                    return false;
            }
            else
            {
                var wordStart = SkipWeekdaySeparators(text, pos);
                if (wordStart < 0)
                    return false;

                var wordLength = CountLetters(text, wordStart);
                weekday = IndexOfName(s_dayNames, text, wordStart, wordLength);
                if (weekday >= 0)
                    pos = SkipWeekdaySeparators(text, wordStart + wordLength);
                else
                    pos = SkipWhitespace(text, pos);

                if (pos < 0)
                    return false;

                // A time can touch a weekday, but cannot run into the last numeric date component.
                if (pos == dateEnd && pos < text.Length && TextFacts.IsDigit(text[pos]))
                    return false;

                // A bare AM/PM after a numeric date component adjusts midnight, not that component:
                // Feb 2020 3 PM means February 3 at noon, not February 1 at 15:00.
                var allowBareTimeMark = weekday < 0 && TextFacts.IsDigit(text[dateEnd - 1]);
                if (!TryParseTimeOfDay(text, ref pos, allowBareTimeMark,
                    out hour, out minute, out second, out fractionTicks, out offsetMinutes))
                    return false;

                if (pos != text.Length)
                    return false;

                // The whole text is one of these shapes, so its validity is now decided here.
                recognized = true;
            }

            if (!IsValidDate(year, month, day, hour, minute, second))
                return false;

            // .NET limits the offset as a whole to 14 hours, so +14:00 is accepted and +14:01 is not.
            // .NET Framework accepts larger offsets, and so does a JavaScript Date, so this cannot be left to
            // the platform.
            if (offsetMinutes > MaxOffsetMinutes || offsetMinutes < -MaxOffsetMinutes)
                return false;

            // The platform adds the rounded fraction before it applies the offset, and rejects the text if
            // that alone carries past the end of year 9999, even when the offset would bring it back in range.
            var ticks = ToUtcTicks(year, month, day, hour, minute, second, fractionTicks);
            if (ticks > MaxTicks)
                return false;

            // The weekday is checked after rounding the fraction but before applying the offset.
            // Day zero is Monday, matching the order in s_dayNames.
            if (weekday >= 0 && (ticks / TicksPerDay) % 7 != weekday)
                return false;

            ticks -= offsetMinutes * TicksPerMinute;

            // The platform wraps an instant that the offset takes before year one forward a whole day rather
            // than rejecting it, so 0001-01-01T00:00:00+01:00 is 0001-01-01T23:00:00Z. It only does this at
            // the lower bound: an offset that takes the instant past the end of year 9999 is rejected.
            if (ticks < 0)
                ticks += TicksPerDay;

            if (ticks > MaxTicks)
                return false;

            value = new DateTime(ticks, DateTimeKind.Utc);
            return true;
        }

        /// <summary>
        /// True when the text starts with a year first (yyyy-M-d or yyyy/M/d) or month/day/year date whose
        /// parts are out of range, followed by the end of the text, a space, a 'T', a 'Z' or an offset's sign.
        /// </summary>
        private static bool StartsWithOutOfRangeDate(string text)
        {
            int year, month, day;
            var pos = 0;

            if (!TryParseYearFirstDate(text, ref pos, out year, out month, out day))
            {
                pos = 0;
                if (!TryParseSlashDate(text, ref pos, out year, out month, out day))
                    return false;
            }

            if (pos < text.Length && !IsAfterDate(text[pos]))
                return false;

            return !IsValidDate(year, month, day, 0, 0, 0);
        }

        private static bool IsAfterDate(char c)
        {
            return c == ' ' || c == 'T' || c == 't' || c == 'Z' || c == 'z' || c == '+' || c == '-';
        }

        private static readonly string[] s_monthNames = new[]
        {
            "january", "february", "march", "april", "may", "june",
            "july", "august", "september", "october", "november", "december"
        };

        private static readonly string[] s_dayNames = new[]
        {
            "monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday"
        };

        /// <summary>
        /// True when the text is a date written with an English month name, such as February 30, 2020 or
        /// 30 Feb 2020, whose day is out of range for its month.
        /// </summary>
        /// <remarks>
        /// Only the plain arrangements are recognised: one month name, full or abbreviated to three letters, a one
        /// or two digit day, an optional four digit year, and optionally day names, a time and a zone. Anything
        /// else is left to the platform. Without a year, the platform reads a number before the month as a year,
        /// so only a day after the month is checked, and against a leap year since the year is the current one.
        /// </remarks>
        private static bool IsOutOfRangeMonthNameDate(string text)
        {
            int month = 0, day = -1, year = -1;
            var dayBeforeMonth = false;
            var hasTime = false;

            var pos = 0;
            while (pos < text.Length)
            {
                if (IsMonthNameDateSeparator(text[pos]))
                {
                    pos++;
                    continue;
                }

                var start = pos;
                while (pos < text.Length && !IsMonthNameDateSeparator(text[pos]))
                    pos++;

                var length = pos - start;

                if (IsTimeWord(text, start, length, ref hasTime))
                    continue;

                // A number after the time could be part of it, such as a fraction of a second.
                if (AllDigits(text, start, length))
                {
                    if (hasTime)
                        return false;

                    if (length <= 2 && day < 0)
                    {
                        day = ToInt(text, start, length);
                        dayBeforeMonth = month == 0;
                    }
                    else if (length == 4 && year < 0)
                    {
                        year = ToInt(text, start, length);
                    }
                    else
                    {
                        return false;
                    }

                    continue;
                }

                var monthIndex = IndexOfName(s_monthNames, text, start, length);
                if (monthIndex >= 0)
                {
                    if (month != 0)
                        return false;

                    month = monthIndex + 1;
                    continue;
                }

                if (IndexOfName(s_dayNames, text, start, length) < 0
                    && !IsWord(text, start, length, "z")
                    && !IsWord(text, start, length, "gmt")
                    && !(hasTime && (IsWord(text, start, length, "am") || IsWord(text, start, length, "pm"))))
                    return false;
            }

            if (month == 0)
                return false;

            if (year >= 0)
                return !IsValidDate(year, month, day < 0 ? 1 : day, 0, 0, 0);

            if (day < 0 || dayBeforeMonth)
                return false;

            return day < 1 || day > GetDaysInMonth(2000, month);
        }

        private static bool IsMonthNameDateSeparator(char c)
        {
            return char.IsWhiteSpace(c) || c == ',' || c == '.' || c == '-';
        }

        /// <summary>
        /// True when the word is a time, such as 12:30:45, 13:00Z or 12:00+01:00. A word with a ':' in it that
        /// is not a time makes the whole text unrecognised.
        /// </summary>
        private static bool IsTimeWord(string text, int start, int length, ref bool hasTime)
        {
            var hasColon = false;
            for (var i = start; i < start + length; i++)
            {
                if (text[i] == ':')
                    hasColon = true;
            }

            if (!hasColon)
                return false;

            for (var i = start; i < start + length; i++)
            {
                var c = text[i];
                if (!TextFacts.IsDigit(c) && c != ':' && c != '+' && c != 'Z' && c != 'z')
                    return false;
            }

            hasTime = true;
            return true;
        }

        /// <summary>
        /// The index of the name the word spells out in full or abbreviates to its first three letters,
        /// ignoring case, or -1.
        /// </summary>
        private static int IndexOfName(string[] names, string text, int start, int length)
        {
            for (var i = 0; i < names.Length; i++)
            {
                var name = names[i];
                if ((length == name.Length || length == 3) && StartsWithIgnoringCase(name, text, start, length))
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// True when the word is the given lower case ASCII word, ignoring case.
        /// </summary>
        private static bool IsWord(string text, int start, int length, string word)
        {
            return length == word.Length && StartsWithIgnoringCase(word, text, start, length);
        }

        /// <summary>
        /// True when the lower case ASCII word starts with <c>text[start..start+length)</c>, ignoring case.
        /// </summary>
        private static bool StartsWithIgnoringCase(string word, string text, int start, int length)
        {
            if (length > word.Length)
                return false;

            for (var i = 0; i < length; i++)
            {
                var c = text[start + i];
                if (c >= 'A' && c <= 'Z')
                    c = (char)(c - 'A' + 'a');

                if (c != word[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Parses a four digit year followed by a one or two digit month and day, separated by '-' or '/'.
        /// The platform also accepts a different separator between the month and day.
        /// </summary>
        private static bool TryParseYearFirstDate(string text, ref int pos, out int year, out int month, out int day)
        {
            year = month = day = 0;

            if (text.Length < 5 || !AllDigits(text, 0, 4))
                return false;

            var separator = text[4];
            if (separator != '-' && separator != '/')
                return false;

            pos = 5;
            if (!TryParseNumberUpTo(text, ref pos, 2, out month)
                || !(TryConsume(text, ref pos, '-') || TryConsume(text, ref pos, '/'))
                || !TryParseNumberUpTo(text, ref pos, 2, out day))
                return false;

            year = ToInt(text, 0, 4);
            return true;
        }

        /// <summary>
        /// Reads an English month name, a one or two digit day and a four digit year, in any order.
        /// Incomplete dates and ambiguous short years remain the platform's responsibility.
        /// </summary>
        private static bool TryParseMonthNameDate(string text, ref int pos, out int year, out int month, out int day)
        {
            year = day = -1;
            month = 0;
            for (var part = 0; part < 3; part++)
            {
                var length = CountDigits(text, pos);
                if (length > 0)
                {
                    if (length == 4 && year < 0)
                        year = ToInt(text, pos, length);
                    else if (length <= 2 && day < 0)
                        day = ToInt(text, pos, length);
                    else
                        return false;
                }
                else
                {
                    length = CountLetters(text, pos);
                    var index = IndexOfName(s_monthNames, text, pos, length);
                    if (index < 0 || month != 0)
                        return false;

                    month = index + 1;
                }

                pos += length;
                if (part < 2)
                {
                    var hasPunctuation = false;
                    while (pos < text.Length && IsMonthNameDateSeparator(text[pos]))
                    {
                        // Repeated or mixed punctuation has context-sensitive meaning to the platform.
                        if (!char.IsWhiteSpace(text[pos]))
                        {
                            if (hasPunctuation)
                                return false;

                            hasPunctuation = true;
                        }

                        pos++;
                    }
                }
            }

            return year >= 0 && month > 0 && day >= 0;
        }

        private static bool TryParseSlashDate(string text, ref int pos, out int year, out int month, out int day)
        {
            year = month = day = 0;

            // the invariant culture orders these as month/day/year
            int m, d, y;
            if (!TryParseNumberUpTo(text, ref pos, 2, out m)
                || !TryConsume(text, ref pos, '/')
                || !TryParseNumberUpTo(text, ref pos, 2, out d)
                || !TryConsume(text, ref pos, '/'))
                return false;

            var yearStart = pos;
            if (!TryParseNumberUpTo(text, ref pos, 4, out y))
                return false;

            month = m;
            day = d;
            year = pos - yearStart <= 2 ? ToFourDigitYear(y) : y;
            return true;
        }

        /// <summary>
        /// Applies the two digit year window the invariant culture uses, so that 1/2/03 and 1/2/3 both mean
        /// 2003 and not year 3. Only a year written with one or two digits is mapped: 003 stays year 3, as it
        /// does there.
        /// </summary>
        /// <remarks>
        /// <see cref="TwoDigitYearMax"/> mirrors the invariant calendar's value rather than reading it, since
        /// the culture APIs are not available once this is transpiled to JavaScript. Hardcoding it keeps both
        /// platforms in step, and <c>DateTimeLiteralParityTests</c> fails if the platform ever disagrees.
        /// </remarks>
        private const int TwoDigitYearMax = 2049;

        private static int ToFourDigitYear(int year)
        {
            var candidate = (TwoDigitYearMax / 100) * 100 + year;
            return candidate > TwoDigitYearMax ? candidate - 100 : candidate;
        }

        /// <summary>
        /// Parses an optional time, AM/PM and zone after a date. Unlike the time after a 'T', each part can
        /// be one or two digits, an hour alone requires AM/PM, and a zone does not require a time.
        /// </summary>
        private static bool TryParseTimeOfDay(
            string text, ref int pos, bool allowBareTimeMark,
            out int hour, out int minute, out int second, out int fractionTicks, out int offsetMinutes)
        {
            hour = minute = second = fractionTicks = offsetMinutes = 0;

            var hasHour = TryParseNumberUpTo(text, ref pos, 2, out hour);
            var hasMinutes = hasHour && TryConsume(text, ref pos, ':');
            if (hasMinutes)
            {
                if (!TryParseNumberUpTo(text, ref pos, 2, out minute))
                    return false;

                if (TryConsume(text, ref pos, ':'))
                {
                    if (!TryParseNumberUpTo(text, ref pos, 2, out second))
                        return false;

                    if (TryConsume(text, ref pos, '.') && !TryParseFraction(text, ref pos, out fractionTicks))
                        return false;
                }
            }

            pos = SkipWhitespace(text, pos);
            var wordLength = CountLetters(text, pos);
            var am = IsWord(text, pos, wordLength, "am");
            var pm = IsWord(text, pos, wordLength, "pm");
            if (am || pm)
            {
                if (!hasHour && !allowBareTimeMark)
                    return false;

                // The platform accepts 0..12 AM and 0..23 PM, including 13:00 PM as 13:00.
                if (am)
                    hour = hour > 12 ? -1 : hour == 12 ? 0 : hour;
                else if (hour < 12)
                    hour += 12;

                pos = SkipWhitespace(text, pos + wordLength);
            }
            else if (hasHour && !hasMinutes)
            {
                return false;
            }

            if (IsWord(text, pos, CountLetters(text, pos), "gmt"))
            {
                pos += 3;
                return true;
            }

            return TryParseZone(text, ref pos, out offsetMinutes);
        }

        /// <summary>
        /// Parses the time after a 'T' as strictly as the platform's ISO 8601 parse does: the hour, minute and
        /// optional second are exactly two digits each, whitespace is allowed around each of them, and a
        /// fraction, introduced by either '.' or ',', can only follow the seconds directly. The time can then
        /// be followed by a 'Z' or a UTC offset, and nothing else.
        /// </summary>
        private static bool TryParseIsoTime(
            string text, ref int pos,
            out int hour, out int minute, out int second, out int fractionTicks, out int offsetMinutes)
        {
            hour = minute = second = fractionTicks = offsetMinutes = 0;

            pos = SkipWhitespace(text, pos);
            if (!TryParseTwoDigits(text, ref pos, out hour))
                return false;

            pos = SkipWhitespace(text, pos);
            if (!TryConsume(text, ref pos, ':'))
                return false;

            pos = SkipWhitespace(text, pos);
            if (!TryParseTwoDigits(text, ref pos, out minute))
                return false;

            pos = SkipWhitespace(text, pos);
            if (TryConsume(text, ref pos, ':'))
            {
                pos = SkipWhitespace(text, pos);
                if (!TryParseTwoDigits(text, ref pos, out second))
                    return false;

                if ((TryConsume(text, ref pos, '.') || TryConsume(text, ref pos, ','))
                    && !TryParseFraction(text, ref pos, out fractionTicks))
                    return false;

                pos = SkipWhitespace(text, pos);
            }

            return TryParseZone(text, ref pos, out offsetMinutes) && pos == text.Length;
        }

        /// <summary>
        /// Parses an optional 'Z' or UTC offset at the current position. Anything else is left for the caller.
        /// </summary>
        private static bool TryParseZone(string text, ref int pos, out int offsetMinutes)
        {
            offsetMinutes = 0;

            if (pos == text.Length)
                return true;

            var c = text[pos];
            if (c == 'Z' || c == 'z')
            {
                pos++;
                return true;
            }

            if (c == '+' || c == '-')
            {
                pos++;
                return TryParseOffset(text, ref pos, c == '-', out offsetMinutes);
            }

            return true;
        }

        /// <summary>
        /// Parses the digits of a UTC offset after its sign the way the platform does. One or two digits are
        /// hours, optionally followed by ':' and one or two digits of minutes. Three or four digits are hours
        /// and minutes together, so +100 is one hour and +015 is fifteen minutes. The minutes must be under 60.
        /// </summary>
        private static bool TryParseOffset(string text, ref int pos, bool negative, out int offsetMinutes)
        {
            offsetMinutes = 0;

            var length = CountDigits(text, pos);
            int hours, minutes = 0;

            if (length == 1 || length == 2)
            {
                hours = ToInt(text, pos, length);
                pos += length;

                if (TryConsume(text, ref pos, ':'))
                {
                    var minuteLength = CountDigits(text, pos);
                    if (minuteLength < 1 || minuteLength > 2)
                        return false;

                    minutes = ToInt(text, pos, minuteLength);
                    pos += minuteLength;
                }
            }
            else if (length == 3 || length == 4)
            {
                var digits = ToInt(text, pos, length);
                hours = digits / 100;
                minutes = digits % 100;
                pos += length;
            }
            else
            {
                return false;
            }

            if (minutes > 59)
                return false;

            offsetMinutes = (negative ? -1 : 1) * (hours * 60 + minutes);
            return true;
        }

        /// <summary>
        /// Parses the digits of a fraction of a second, after its separator. At least one digit is required.
        /// </summary>
        private static bool TryParseFraction(string text, ref int pos, out int fractionTicks)
        {
            fractionTicks = 0;

            var length = CountDigits(text, pos);
            if (length == 0)
                return false;

            fractionTicks = ToRoundedFractionTicks(text, pos, pos + length);
            pos += length;
            return true;
        }

        private static bool TryParseTwoDigits(string text, ref int pos, out int value)
        {
            value = 0;

            if (pos + 2 > text.Length || !AllDigits(text, pos, 2))
                return false;

            value = ToInt(text, pos, 2);
            pos += 2;
            return true;
        }

        /// <summary>
        /// True when the character at the position is a 'T' that marks the start of a time, rather than the
        /// first letter of a word such as a day name.
        /// </summary>
        private static bool IsTimeMark(string text, int pos)
        {
            return pos < text.Length
                && (text[pos] == 'T' || text[pos] == 't')
                && (pos + 1 == text.Length || !char.IsLetter(text[pos + 1]));
        }

        private static int SkipWhitespace(string text, int pos)
        {
            while (pos < text.Length && char.IsWhiteSpace(text[pos]))
                pos++;

            return pos;
        }

        private static int SkipWeekdaySeparators(string text, int pos)
        {
            var hasPunctuation = false;
            while (pos < text.Length && (char.IsWhiteSpace(text[pos]) || text[pos] == ',' || text[pos] == '.'))
            {
                if (!char.IsWhiteSpace(text[pos]))
                {
                    if (hasPunctuation)
                        return -1;

                    hasPunctuation = true;
                }

                pos++;
            }

            return pos;
        }

        private static int CountLetters(string text, int pos)
        {
            var count = 0;
            while (pos + count < text.Length && char.IsLetter(text[pos + count]))
                count++;

            return count;
        }

        private static int CountDigits(string text, int pos)
        {
            var count = 0;
            while (pos + count < text.Length && TextFacts.IsDigit(text[pos + count]))
                count++;

            return count;
        }

        /// <summary>
        /// Converts the fractional second digits in <c>text[start..end)</c> to ticks the way the platform's
        /// parser does, so that the reported value agrees with the service to the tick.
        /// </summary>
        /// <remarks>
        /// The platform accumulates every digit into a double, scales it to ticks and rounds half to even.
        /// Reproducing those exact floating point steps, rather than rounding the decimal digits, is what makes
        /// a case such as .12345675 come out the same here. The result can be a whole second, which the caller
        /// has to allow to carry into the seconds.
        /// </remarks>
        private static int ToRoundedFractionTicks(string text, int start, int end)
        {
            var fraction = 0.0;
            var decimalBase = 0.1;
            for (var i = start; i < end; i++)
            {
                fraction += (text[i] - '0') * decimalBase;
                decimalBase *= 0.1;
            }

            // Written out rather than calling Math.Round, whose midpoint handling is not guaranteed to match
            // once transpiled to JavaScript.
            var scaled = fraction * 10000000.0;
            var whole = Math.Floor(scaled);
            var remainder = scaled - whole;
            if (remainder > 0.5 || (remainder == 0.5 && (whole % 2.0) != 0.0))
                whole += 1.0;

            return (int)whole;
        }

        private static bool TryConsume(string text, ref int pos, char c)
        {
            if (pos < text.Length && text[pos] == c)
            {
                pos++;
                return true;
            }

            return false;
        }

        private static bool TryParseNumberUpTo(string text, ref int pos, int maxDigits, out int value)
        {
            value = 0;

            var start = pos;
            while (pos < text.Length && pos - start < maxDigits && text[pos] >= '0' && text[pos] <= '9')
            {
                value = value * 10 + (text[pos] - '0');
                pos++;
            }

            return pos > start;
        }

        private static bool IsNowKeyword(string text)
        {
            return text.Length == 3
                && (text[0] == 'n' || text[0] == 'N')
                && (text[1] == 'o' || text[1] == 'O')
                && (text[2] == 'w' || text[2] == 'W');
        }

        /// <summary>
        /// Parses an optionally signed run of digits, matching what the engine's Int64 parse accepts
        /// once the text has been trimmed.
        /// </summary>
        private static bool TryParseInt64(string text, out long value)
        {
            value = 0;

            var index = 0;
            var negative = false;
            if (text[0] == '-' || text[0] == '+')
            {
                negative = text[0] == '-';
                index = 1;
            }

            if (index >= text.Length)
                return false;

            for (; index < text.Length; index++)
            {
                var c = text[index];
                if (c < '0' || c > '9')
                    return false;

                var digit = c - '0';

                // Overflowing the engine's Int64 parse means it falls through to the general parser.
                if (value > (long.MaxValue - digit) / 10)
                    return false;

                value = value * 10 + digit;
            }

            if (negative)
                value = -value;

            return true;
        }

        private static bool IsValidDate(int year, int month, int day, int hour, int minute, int second)
        {
            return year >= 1 && year <= 9999
                && month >= 1 && month <= 12
                && day >= 1 && day <= GetDaysInMonth(year, month)
                && hour >= 0 && hour <= 23
                && minute >= 0 && minute <= 59
                && second >= 0 && second <= 59;
        }

        private static int GetDaysInMonth(int year, int month)
        {
            var months = IsLeapYear(year) ? s_daysToMonth366 : s_daysToMonth365;
            return months[month] - months[month - 1];
        }

        private static bool IsLeapYear(int year)
        {
            return (year % 4) == 0 && ((year % 100) != 0 || (year % 400) == 0);
        }

        private static bool AllDigits(string text, int start, int length)
        {
            for (var i = start; i < start + length; i++)
            {
                if (!TextFacts.IsDigit(text[i]))
                    return false;
            }

            return true;
        }

        private static int ToInt(string text, int start, int length)
        {
            var result = 0;
            for (var i = start; i < start + length; i++)
                result = result * 10 + (text[i] - '0');

            return result;
        }
    }
}
