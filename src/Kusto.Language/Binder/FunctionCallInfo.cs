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
        private FunctionCallExpansion _expansion;
        private FunctionBodyFacts _facts;
        private bool _hasErrors;

        public FunctionCallInfo(Func<FunctionCallExpansion> expander, FunctionBodyFacts facts, bool hasErrors)
        {
            _expander = expander;
            _facts = facts;
            _hasErrors = hasErrors;
        }

        public FunctionCallInfo(FunctionCallExpansion expansion, FunctionBodyFacts facts, bool hasErrors)
        {
            _expansion = expansion;
            _facts = facts;
            _hasErrors = hasErrors;
        }

        /// <summary>
        /// The function body facts associated with the called function.
        /// </summary>
        public FunctionBodyFacts Facts => _facts;

        /// <summary>
        /// The expansion (analyzed syntax tree in context of call arguments) of the called function.
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
                    _diagnostics = this.Expansion?.Root?.GetContainedDiagnostics() ?? Diagnostic.NoDiagnostics;
                }

                return _diagnostics;
            }
        }
    }
}