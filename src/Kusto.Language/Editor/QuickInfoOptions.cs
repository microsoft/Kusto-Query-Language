using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Kusto.Language.Editor
{
    using Utils;

    public class QuickInfoOptions
    {
        /// <summary>
        /// Shows item descriptions for this position.
        /// </summary>
        public bool ShowDescriptions { get; }

        /// <summary>
        /// Shows diagnostics for this position.
        /// </summary>
        public bool ShowDiagnostics { get; }

        /// <summary>
        /// A comma separated list of diagnostic codes that are disabled.
        /// </summary>
        public DisabledDiagnostics DiagnosticFilter { get; }

        private QuickInfoOptions(
            bool showDescriptions,
            bool showDiagnostics,
            DisabledDiagnostics filter)
        {
            this.ShowDescriptions = showDescriptions;
            this.ShowDiagnostics = showDiagnostics;
            this.DiagnosticFilter = filter ?? DisabledDiagnostics.Default;
        }

        private QuickInfoOptions With(
            Optional<bool> showDiagnostics = default,
            Optional<bool> showDescriptions = default,
            Optional<DisabledDiagnostics> filter = default)
        {
            var newShowDiagnostics = showDiagnostics.HasValue ? showDiagnostics.Value : this.ShowDiagnostics;
            var newShowDescriptions = showDescriptions.HasValue ? showDescriptions.Value : this.ShowDescriptions;
            var newFilter = filter.HasValue ? filter.Value ?? DisabledDiagnostics.Default : this.DiagnosticFilter;

            if (newShowDiagnostics != this.ShowDiagnostics
                || newShowDescriptions != this.ShowDescriptions
                || newFilter != this.DiagnosticFilter)
            {
                return new QuickInfoOptions(
                    newShowDescriptions,
                    newShowDiagnostics,
                    newFilter);
            }
            else
            {
                return this;
            }
        }

        public QuickInfoOptions WithShowDescriptions(bool show)
        {
            return With(showDescriptions: show);
        }

        public QuickInfoOptions WithShowDiagnostics(bool show)
        {
            return With(showDiagnostics: show);
        }

        public QuickInfoOptions WithDiagnosticFilter(DisabledDiagnostics filter)
        {
            return With(filter: filter);
        }

        public static readonly QuickInfoOptions Default =
            new QuickInfoOptions(true, true, null);
    }
}