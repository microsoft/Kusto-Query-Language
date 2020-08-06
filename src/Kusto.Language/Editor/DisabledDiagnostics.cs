using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    public class DisabledDiagnostics
    {
        private readonly HashSet<string> _severities;
        private readonly HashSet<string> _codes;

        public DisabledDiagnostics(string text)
        {
            var items = (text ?? "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
            _severities = new HashSet<string>(items.Where(i => i.StartsWith(SeverityPrefix)).Select(i => i.Substring(SeverityPrefix.Length)));
            _codes = new HashSet<string>(items.Where(i => !i.StartsWith(SeverityPrefix)));
        }

        public DisabledDiagnostics()
        {
            _severities = new HashSet<string>();
            _codes = new HashSet<string>();
        }

        public static DisabledDiagnostics Parse(string text)
        {
            return new DisabledDiagnostics(text);
        }

        private static readonly string SeverityPrefix = "sev:";

        public override string ToString()
        {
            return string.Join(",", _severities.OrderBy(s => s).Select(s => SeverityPrefix + s).Concat(_codes.OrderBy(c => c)));
        }

        public bool IsSeverityEnabled(string severity)
        {
            return !IsServerityDisabled(severity);
        }

        public bool IsServerityDisabled(string severity)
        {
            return _severities.Contains(severity);
        }

        public bool IsCodeEnabled(string code)
        {
            return !IsCodeDisabled(code);
        }

        public bool IsCodeDisabled(string code)
        {
            return _codes.Contains(code);
        }

        public bool IsDiagnosticEnabled(Diagnostic diagnostic)
        {
            return !IsDiagnosticDisabled(diagnostic);
        }

        public bool IsDiagnosticDisabled(Diagnostic diagnostic)
        {
            return IsServerityDisabled(diagnostic.Severity)
                || IsCodeDisabled(diagnostic.Code);
        }

        public static bool CanDisableSeverity(string severity)
        {
            return severity == DiagnosticSeverity.Warning
                || severity == DiagnosticSeverity.Suggestion
                || severity == DiagnosticSeverity.Information;
        }

        private static HashSet<string> _codesThatCanBeDisabled;

        public static bool CanDisableCode(string code)
        {
            if (_codesThatCanBeDisabled == null)
            {
                _codesThatCanBeDisabled = new HashSet<string>(KustoCodeService.AnalyzerDiagnostics.Select(d => d.Code));
            }

            return _codesThatCanBeDisabled.Contains(code);
        }

        public void SetSeverityEnabled(string severity, bool enabled)
        {
            if (enabled)
            {
                _severities.Remove(severity);
            }
            else if (!enabled && CanDisableSeverity(severity))
            {
                _severities.Add(severity);
            }
        }

        public void SetCodeEnabled(string code, bool enabled)
        {
            if (enabled)
            {
                _codes.Remove(code);
            }
            else if (!enabled && CanDisableCode(code))
            {
                _codes.Add(code);
            }
        }
    }
}