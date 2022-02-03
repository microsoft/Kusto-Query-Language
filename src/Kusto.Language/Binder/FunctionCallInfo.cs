using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Binding
{
    /// <summary>
    /// Extended semantic information for function calls.
    /// </summary>
    internal class FunctionCallInfo
    {
        /// <summary>
        /// A function that returns the expanded body of the referenced function at the call site.
        /// </summary>
        private readonly Func<FunctionCallExpansion> _expander;
        private readonly bool _hasErrors;
        private FunctionCallExpansion _expansion;

        public FunctionCallInfo(Func<FunctionCallExpansion> expander, bool hasErrors)
        {
            _expander = expander;
            _hasErrors = hasErrors;
        }

        public FunctionCallInfo(FunctionCallExpansion expansion, bool hasErrors)
        {
            _expansion = expansion;
            _hasErrors = hasErrors;
        }

        /// <summary>
        /// The expansion of the function called
        /// </summary>
        public FunctionCallExpansion Expansion
        {
            get
            {
                if (_expansion == null && this._expander != null)
                {
                    _expansion = this._expander();
                }

                return _expansion;
            }
        }

        public bool HasErrors => _hasErrors;

        private IReadOnlyList<Diagnostic> _diagnostics;

        public IReadOnlyList<Diagnostic> Diagnostics
        {
            get
            {
                if (_diagnostics == null)
                {
                    _diagnostics = this.Expansion?.Body.GetContainedDiagnostics() ?? Diagnostic.NoDiagnostics;
                }

                return _diagnostics;
            }
        }
    }
}