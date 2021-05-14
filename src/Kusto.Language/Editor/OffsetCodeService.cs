using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Utils;

    /// <summary>
    /// A <see cref="CodeService"/> that wraps another service and adjusts the positions used in the feature API's by a fixed offset.
    /// </summary>
    public class OffsetCodeService : CodeService
    {
        private readonly CodeService _service;

        private readonly int _offset;

        public OffsetCodeService(CodeService service, int offset)
        {
            _service = service;
            _offset = offset;
        }

        public OffsetCodeService WithOffset(int offset)
        {
            return new OffsetCodeService(_service, offset);
        }

        public override string Kind => _service.Kind;

        public override string Text => _service.Text;

        public override ClassificationInfo GetClassifications(int start, int length, bool clipToRange, bool waitForAnalysis, CancellationToken cancellationToken)
        {
            var result = _service.GetClassifications(start - _offset, length, clipToRange, waitForAnalysis, cancellationToken);
            if (result.Classifications.Count > 0 && _offset > 0)
            {
                return new ClassificationInfo(result.Classifications.Select(cr => new ClassifiedRange(cr.Kind, cr.Start + _offset, cr.Length)));
            }
            else
            {
                return result;
            }
        }

        public override IReadOnlyList<ClientParameter> GetClientParameters()
        {
            var result = _service.GetClientParameters();
            if (result.Count > 0 && _offset > 0)
            {
                return result.Select(cp => new ClientParameter(cp.Name, cp.Start + _offset, cp.Length)).ToReadOnly();
            }
            else
            {
                return result;
            }
        }

        public override IReadOnlyList<ClusterReference> GetClusterReferences(CancellationToken cancellationToken)
        {
            var result = _service.GetClusterReferences(cancellationToken);
            if (result.Count > 0 && _offset > 0)
            {
                return result.Select(cr => new ClusterReference(cr.Cluster, cr.Start + _offset, cr.Length)).ToReadOnly();
            }
            else
            {
                return result;
            }
        }

        public override CompletionInfo GetCompletionItems(int position, CompletionOptions options, CancellationToken cancellationToken)
        {
            var result = _service.GetCompletionItems(position - _offset, options, cancellationToken);
            if (_offset > 0)
            {
                return new CompletionInfo(result.Items, result.EditStart + _offset, result.EditLength);
            }
            else
            {
                return result;
            }
        }

        public override IReadOnlyList<DatabaseReference> GetDatabaseReferences(CancellationToken cancellationToken)
        {
            var result = _service.GetDatabaseReferences(cancellationToken);
            if (result.Count > 0 && _offset > 0)
            {
                return result.Select(dr => new DatabaseReference(dr.Database, dr.Cluster, dr.Start + _offset, dr.Length)).ToReadOnly();
            }
            else
            {
                return result;
            }
        }

        public override bool TryGetCachedDiagnostics(out IReadOnlyList<Diagnostic> diagnostics)
        {
            if (_service.TryGetCachedDiagnostics(out diagnostics))
            {
                if (diagnostics.Count > 0 && _offset > 0)
                {
                    diagnostics = diagnostics.Select(dx => dx.WithLocation(dx.Start + _offset, dx.Length)).ToReadOnly();
                }

                return true;
            }

            return false;
        }

        public override IReadOnlyList<Diagnostic> GetDiagnostics(bool waitForAnalysis, CancellationToken cancellationToken)
        {
            var result = _service.GetDiagnostics(waitForAnalysis, cancellationToken);
            if (result.Count > 0 && _offset > 0)
            {
                return result.Select(dx => dx.WithLocation(dx.Start + _offset, dx.Length)).ToReadOnly();
            }
            else
            {
                return result;
            }
        }

        public override bool TryGetCachedAnalyzerDiagnostics(out IReadOnlyList<Diagnostic> diagnostics)
        {
            if (_service.TryGetCachedAnalyzerDiagnostics(out diagnostics))
            {
                if (diagnostics.Count > 0 && _offset > 0)
                {
                    diagnostics = diagnostics.Select(dx => dx.WithLocation(dx.Start + _offset, dx.Length)).ToReadOnly();
                }

                return true;
            }

            return false;
        }

        public override IReadOnlyList<Diagnostic> GetAnalyzerDiagnostics(
            IReadOnlyList<string> analyzers,
            bool waitForAnalysis,
            CancellationToken cancellationToken)
        {
            var result = _service.GetAnalyzerDiagnostics(analyzers, waitForAnalysis, cancellationToken);
            if (result.Count > 0 && _offset > 0)
            {
                return result.Select(dx => dx.WithLocation(dx.Start + _offset, dx.Length)).ToReadOnly();
            }
            else
            {
                return result;
            }
        }

        public override IReadOnlyList<AnalyzerInfo> GetAnalyzers()
        {
            return _service.GetAnalyzers();
        }

        public override FormattedText GetFormattedText(FormattingOptions options, int cursorPosition, CancellationToken cancellationToken)
        {
            var result = _service.GetFormattedText(options, cursorPosition - _offset, cancellationToken);
            if (_offset > 0)
            {
                return new FormattedText(result.Text, result.Position + _offset);
            }
            else
            {
                return result;
            }
        }

        public override string GetMinimalText(MinimalTextKind kind, CancellationToken cancellationToken)
        {
            return _service.GetMinimalText(kind, cancellationToken);
        }

        public override OutlineInfo GetOutlines(CancellationToken cancellationToken)
        {
            var result = _service.GetOutlines(cancellationToken);
            if (result.Ranges.Count > 0 && _offset > 0)
            {
                return new OutlineInfo(result.Ranges.Select(o => new OutlineRange(o.Start + _offset, o.Length, o.CollapsedText)).ToReadOnly());
            }
            else
            {
                return result;
            }
        }

        public override QuickInfo GetQuickInfo(int position, QuickInfoOptions options, CancellationToken cancellationToken)
        {
            return _service.GetQuickInfo(position - _offset, options, cancellationToken);
        }

        public override TextRange GetElement(int position, CancellationToken cancellationToken)
        {
            return _service.GetElement(position);
        }

        public override RelatedInfo GetRelatedElements(int position, FindRelatedOptions options, CancellationToken cancellationToken)
        {
            var result = _service.GetRelatedElements(position - _offset, options, cancellationToken);
            if (result.Elements.Count > 0 && _offset > 0)
            {
                return new RelatedInfo(result.Elements.Select(e =>
                new RelatedElement(e.Start + _offset, e.Length, e.Kind, e.CursorLeft + _offset, e.CursorRight + _offset)).ToReadOnly(), result.CurrentIndex);
            }
            else
            {
                return result;
            }
        }

        public override bool IsFeatureSupported(string feature, int position = -1)
        {
            return _service.IsFeatureSupported(feature, position != -1 ? position - _offset : position);
        }

        public override bool ShouldAutoComplete(int position, char key, CancellationToken cancellationToken)
        {
            return _service.ShouldAutoComplete(position - _offset, key, cancellationToken);
        }
    }
}