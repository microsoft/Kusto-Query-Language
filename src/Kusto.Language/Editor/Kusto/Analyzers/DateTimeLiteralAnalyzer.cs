using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Parsing;
    using Utils;

    /// <summary>
    /// Reports datetime literals whose value is not what the author is likely to have intended.
    /// </summary>
    /// <remarks>
    /// The service interprets the text inside <c>datetime(...)</c> with a set of rules that are largely
    /// invisible from the query text. A bare number in particular means three different things depending on
    /// how many digits it has. These rules cannot be changed without breaking existing queries, so this
    /// analyzer surfaces them instead.
    /// </remarks>
    internal class DateTimeLiteralAnalyzer : KustoAnalyzer
    {
        private static readonly Diagnostic _invalidLiteral =
            new Diagnostic(
                KustoAnalyzerCodes.InvalidDateTimeLiteral,
                category: DiagnosticCategory.Correctness,
                severity: DiagnosticSeverity.Warning,
                description: "Invalid datetime literal",
                message: "This datetime literal cannot be parsed and will be rejected by the service");

        private static readonly Diagnostic _numericLiteral =
            new Diagnostic(
                KustoAnalyzerCodes.AmbiguousNumericDateTimeLiteral,
                category: DiagnosticCategory.Correctness,
                severity: DiagnosticSeverity.Warning,
                description: "Ambiguous numeric datetime literal",
                message: "A numeric datetime literal is interpreted by its digit count, use an explicit date instead");

        protected override IEnumerable<Diagnostic> GetDiagnostics()
        {
            return new[] { _invalidLiteral, _numericLiteral };
        }

        public override void Analyze(KustoCode code, List<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            foreach (var node in code.Syntax.GetDescendants<LiteralExpression>())
            {
                if (node.Kind != SyntaxKind.DateTimeLiteralExpression)
                    continue;

                var token = node.Token;
                if (token == null)
                    continue;

                // datetime(null) has no value to check. This also skips datetime() with no argument at
                // all, which the service rejects but which is a missing-argument problem rather than an
                // unreadable literal.
                if (token.Value == null)
                    continue;

                // An empty literal is not skipped: datetime('') is rejected by the service just as
                // datetime('   ') is, so the parser decides it rather than a length test here.
                var valueText = token.ValueText;
                if (valueText == null)
                    continue;

                DateTime value;
                var interpretation = KustoDateTimeParser.Parse(valueText, out value);

                if (interpretation == DateTimeInterpretation.Invalid)
                {
                    diagnostics.Add(_invalidLiteral
                        .WithMessage($"The datetime literal '{valueText}' cannot be parsed and will be rejected by the service")
                        .WithLocation(node));
                }
                else if (KustoDateTimeParser.HasConstantValue(interpretation)
                    && IsAmbiguousNumeric(valueText, interpretation))
                {
                    int year, month, day, hour, minute, second;
                    KustoDateTimeParser.GetUtcParts(value, out year, out month, out day, out hour, out minute, out second);

                    var date = Pad4(year) + "-" + Pad2(month) + "-" + Pad2(day);
                    var instant = date + "T" + Pad2(hour) + ":" + Pad2(minute) + ":" + Pad2(second)
                        + FormatFraction(value) + "Z";

                    diagnostics.Add(_numericLiteral
                        .WithMessage($"The datetime literal '{valueText}' is interpreted as {DescribeInterpretation(interpretation)}, giving {instant}. Use an explicit date such as datetime({date}) to say what is meant")
                        .WithLocation(node));
                }
            }
        }

        /// <summary>
        /// True for literals written as a bare number whose meaning comes from how many digits it has.
        /// The multi field compact forms such as yyyyMMdd are excluded, since those state a date directly.
        /// </summary>
        private static bool IsAmbiguousNumeric(string valueText, DateTimeInterpretation interpretation)
        {
            switch (interpretation)
            {
                case DateTimeInterpretation.Ticks:
                case DateTimeInterpretation.EpochSeconds:
                    return true;
                case DateTimeInterpretation.FixedWidth:
                    // A bare four digit year reads like a plain number rather than a date.
                    return valueText.Trim().Length == 4;
                default:
                    return false;
            }
        }

        private static string DescribeInterpretation(DateTimeInterpretation interpretation)
        {
            switch (interpretation)
            {
                case DateTimeInterpretation.Ticks:
                    return "a count of ticks since year one";
                case DateTimeInterpretation.EpochSeconds:
                    return "a count of seconds since 1970-01-01";
                default:
                    return "a year";
            }
        }

        /// <summary>
        /// The fractional second of a value as a '.' and seven digits, or nothing when it falls on a whole
        /// second. A tick count can land anywhere within a second, and dropping that part would report an
        /// instant other than the one the service computes.
        /// </summary>
        private static string FormatFraction(DateTime value)
        {
            var fraction = (int)(value.Ticks % 10000000L);
            if (fraction == 0)
                return "";

            var digits = fraction.ToString();
            return "." + new string('0', 7 - digits.Length) + digits;
        }

        private static string Pad2(int value)
        {
            return value < 10 ? "0" + value : value.ToString();
        }

        private static string Pad4(int value)
        {
            if (value < 10) return "000" + value;
            if (value < 100) return "00" + value;
            if (value < 1000) return "0" + value;
            return value.ToString();
        }
    }
}
