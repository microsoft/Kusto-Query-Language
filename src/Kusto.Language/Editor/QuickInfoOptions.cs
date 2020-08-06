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
        private QuickInfoOptions(
            string disabledDiagnostics)
        {
            this.DisabledDiagnostics = disabledDiagnostics ?? "";
        }

        /// <summary>
        /// A comma separated list of diagnostic codes that are disabled.
        /// </summary>
        public string DisabledDiagnostics { get; }

        private QuickInfoOptions With(
            string disabledDiagnostics = null)
        {
            var disdx = disabledDiagnostics ?? this.DisabledDiagnostics;

            if (disdx != this.DisabledDiagnostics)
            {
                return new QuickInfoOptions(
                    disdx);
            }
            else
            {
                return this;
            }
        }

        public QuickInfoOptions WithDisabledDiagnostics(string disabledDiagnostics)
        {
            return With(disabledDiagnostics: disabledDiagnostics);
        }

        public static readonly QuickInfoOptions Default = new QuickInfoOptions(
            disabledDiagnostics: "");
    }
}