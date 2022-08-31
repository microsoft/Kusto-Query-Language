using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    public class DisabledDiagnostics
    {
        private readonly HashSet<string> _severities;
        private readonly HashSet<string> _codes;

        private DisabledDiagnostics(HashSet<string> severities, HashSet<string> codes)
        {
            _severities = severities;
            _codes = codes;
        }

        public DisabledDiagnostics(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var items = text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
                _severities = new HashSet<string>(items.Where(i => i.StartsWith(SeverityPrefix)).Select(i => i.Substring(SeverityPrefix.Length)));
                _codes = new HashSet<string>(items.Where(i => !i.StartsWith(SeverityPrefix)));
            }
        }

        public DisabledDiagnostics()
        {
        }

        public static DisabledDiagnostics Parse(string text)
        {
            return new DisabledDiagnostics(text);
        }

        private static readonly string SeverityPrefix = "sev:";

        public override string ToString()
        {
            if (_severities != null && _codes != null)
            {
                return string.Join(",", _severities.OrderBy(s => s).Select(s => SeverityPrefix + s).Concat(_codes.OrderBy(c => c)));
            }
            else
            {
                return "";
            }
        }

        public bool IsSeverityEnabled(string severity)
        {
            return !IsServerityDisabled(severity);
        }

        public bool IsServerityDisabled(string severity)
        {
            return _severities != null && _severities.Contains(severity);
        }

        public bool IsCodeEnabled(string code)
        {
            return !IsCodeDisabled(code);
        }

        public bool IsCodeDisabled(string code)
        {
            return _codes != null && _codes.Contains(code);
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

        public Builder ToBuilder()
        {
            return new Builder(this);
        }

        public static readonly DisabledDiagnostics Default 
            = new DisabledDiagnostics(null, null);

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
                _codesThatCanBeDisabled = new HashSet<string>(
                    new KustoCodeService("").GetAnalyzers().SelectMany(a => a.Diagnostics).Select(d => d.Code));
            }

            return _codesThatCanBeDisabled.Contains(code);
        }

        public DisabledDiagnostics WithCodeEnabled(string code, bool enabled)
        {
            var builder = this.ToBuilder();
            builder.SetCodeEnabled(code, enabled);
            return builder.ToDisabledDiagnostics();
        }

        public DisabledDiagnostics WithSeverityEnabled(string severity, bool enabled)
        {
            var builder = this.ToBuilder();
            builder.SetSeverityEnabled(severity, enabled);
            return builder.ToDisabledDiagnostics();
        }

        public class Builder
        {
            private DisabledDiagnostics _source;
            private HashSet<string> _severities;
            private HashSet<string> _codes;

            public Builder(DisabledDiagnostics disabledDiagnostics)
            {
                _source = disabledDiagnostics;
            }

            public Builder()
            {
            }

            public DisabledDiagnostics ToDisabledDiagnostics()
            {
                if (_source != null)
                {
                    return _source;
                }
                else
                {
                    _source = new DisabledDiagnostics(_severities, _codes);
                    _severities = null;
                    _codes = null;
                    return _source;
                }
            }

            public override string ToString()
            {
                return ToDisabledDiagnostics().ToString();
            }

            private void TryInit()
            {
                if (_severities == null)
                {
                    if (_source != null && _source._severities != null)
                    {
                        _severities = new HashSet<string>(_source._severities);
                    }
                    else
                    {
                        _severities = new HashSet<string>();
                    }
                }

                if (_codes == null)
                {
                    if (_source != null && _source._codes != null)
                    {
                        _codes = new HashSet<string>(_source._codes);
                    }
                    else
                    {
                        _codes = new HashSet<string>();
                    }
                }

                _source = null;
            }

            public void SetSeverityEnabled(string severity, bool enabled)
            {
                TryInit();

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
                TryInit();

                if (enabled && _codes != null)
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
}