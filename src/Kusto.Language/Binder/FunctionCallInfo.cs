using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Binding
{
    using Syntax;
    using Utils;

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
        private bool? _hasErrors;

        public FunctionCallInfo(Func<FunctionCallExpansion> expander, FunctionBodyFacts facts)
        {
            _expander = expander;
            _facts = facts;
        }

        public FunctionCallInfo(FunctionCallExpansion expansion, FunctionBodyFacts facts)
        {
            _expansion = expansion;
            _facts = facts;
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

        public bool HasErrors
        {
            get
            {
                if (_hasErrors == null)
                {
                    _hasErrors =
                        _facts.HasSyntaxErrors
                        || this.Diagnostics.Any(d => d.Severity == DiagnosticSeverity.Error);
                }

                return _hasErrors.GetValueOrDefault();
            }
        }

        private IReadOnlyList<Diagnostic> _diagnostics;

        public IReadOnlyList<Diagnostic> Diagnostics
        {
            get
            {
                if (_diagnostics == null)
                {
                    if (this.Expansion?.Root is SyntaxNode root)
                    {
                        // relocate diagnostics in original tree positions
                        _diagnostics = root.GetContainedDiagnostics()
                            .Select(d => d.WithLocation(root.GetPositionInOriginalTree(d.Start), d.Length))
                            .ToReadOnly();
                    }
                    else
                    {
                        _diagnostics = Diagnostic.NoDiagnostics;
                    }
                }

                return _diagnostics;
            }
        }
    }
}