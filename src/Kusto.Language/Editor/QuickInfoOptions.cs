using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Kusto.Language.Editor
{
    public class QuickInfoOptions
    {
        private QuickInfoOptions(
            DisabledDiagnostics filter)
        {
            this.DiagnosticFilter = filter ?? DisabledDiagnostics.Default;
        }

        /// <summary>
        /// A comma separated list of diagnostic codes that are disabled.
        /// </summary>
        public DisabledDiagnostics DiagnosticFilter { get; }

        private QuickInfoOptions With(
            DisabledDiagnostics filter)
        {
            var newFilter = filter ?? DisabledDiagnostics.Default;

            if (newFilter != this.DiagnosticFilter)
            {
                return new QuickInfoOptions(newFilter);
            }
            else
            {
                return this;
            }
        }

        public QuickInfoOptions WithDiagnosticFilter(DisabledDiagnostics filter)
        {
            return With(filter: filter);
        }

        public static readonly QuickInfoOptions Default = new QuickInfoOptions(
            filter: null);
    }
}