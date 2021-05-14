using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Utils;

    /// <summary>
    /// A <see cref="CodeService"/> with default implementations for each feature API.
    /// </summary>
    public abstract class CommonCodeService : CodeService
    {
        private readonly string _text;

        protected CommonCodeService(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            _text = text;
        }

        public override string Text => _text;

        public override bool IsFeatureSupported(string feature, int position = -1)
        {
            return true;
        }

        public override bool TryGetCachedDiagnostics(out IReadOnlyList<Diagnostic> diagnostics)
        {
            diagnostics = null;
            return false;
        }

        public override IReadOnlyList<Diagnostic> GetDiagnostics(bool waitForAnalysis, CancellationToken cancellationToken)
        {
            return EmptyReadOnlyList<Diagnostic>.Instance;
        }

        public override bool TryGetCachedAnalyzerDiagnostics(out IReadOnlyList<Diagnostic> diagnostics)
        {
            diagnostics = null;
            return false;
        }

        public override IReadOnlyList<Diagnostic> GetAnalyzerDiagnostics(IReadOnlyList<string> analyzers, bool waitForAnalysis, CancellationToken cancellationToken = default)
        {
            return EmptyReadOnlyList<Diagnostic>.Instance;
        }

        public override IReadOnlyList<AnalyzerInfo> GetAnalyzers()
        {
            return EmptyReadOnlyList<AnalyzerInfo>.Instance;
        }

        public override ClassificationInfo GetClassifications(int start, int length, bool clipToRange, bool waitForAnalysis, CancellationToken cancellationToken)
        {
            // by default classify entire text as plain-text.
            var classifications = new[] { new ClassifiedRange(ClassificationKind.PlainText, start, length) };
            var clientParameterClassifications = GetClientParametersClassifications();
            var merged = Add(classifications, clientParameterClassifications);
            return new ClassificationInfo(merged);
        }

        protected IReadOnlyList<ClassifiedRange> GetClientParametersClassifications()
        {
            var cps = GetClientParameters();
            if (cps.Count == 0)
                return EmptyReadOnlyList<ClassifiedRange>.Instance;

            var list = new List<ClassifiedRange>();
            foreach (var cp in cps)
            {
                list.Add(new ClassifiedRange(ClassificationKind.ClientParameter, cp.Start, cp.Length));
            }

            return list;
        }

        /// <summary>
        /// Add the two classification ranges together.
        /// If any overlap, classifications from <see cref="P:classifications2"/> take precedence.
        /// </summary>
        protected static IReadOnlyList<ClassifiedRange> Add(
            IReadOnlyList<ClassifiedRange> classifications1,
            IReadOnlyList<ClassifiedRange> classifications2)
        {
            if (classifications2.Count == 0)
                return classifications1;

            if (classifications1.Count == 0)
                return classifications2;

            var list = new List<ClassifiedRange>(classifications1.Count + classifications2.Count);

            int index1 = 0;
            int index2 = 0;
            ClassifiedRange class1 = null;
            ClassifiedRange class2 = null;

            while (true)
            {
                if (class1 == null && index1 < classifications1.Count)
                {
                    class1 = classifications1[index1];
                }

                if (class2 == null && index2 < classifications2.Count)
                {
                    class2 = classifications2[index2];
                }

                if (class1 == null && class2 == null)
                {
                    // we are done
                    break;
                }
                else if (class1 != null && class2 == null)
                {
                    // no more class2, just add class1
                    list.Add(class1);
                    class1 = null;
                    index1++;
                }
                else if (class2 != null && class1 == null)
                {
                    // no more class1, just add class2
                    list.Add(class2);
                    class2 = null;
                    index2++;
                }
                else if (class1.End < class2.Start)
                {
                    // class1 is entirely before class2, so add class1 and advance it
                    list.Add(class1);
                    class1 = null;
                    index1++;
                }
                else if (class2.End < class1.Start)
                {
                    // class2 is entirely before classs1, so add class2 and advance it
                    list.Add(class2);
                    class2 = null;
                    index2++;
                }
                else if (class1.Start >= class2.Start && class1.End <= class2.End)
                {
                    // class1 is entirely within class2
                    // drop class1 because class2 takes precedence
                    class1 = null;
                    index1++;
                }
                else if (class1.Start < class2.Start)
                {
                    // class1 starts before class2
                    var len = class2.Start - class1.Start;

                    // add the first part off class1
                    list.Add(new ClassifiedRange(class1.Kind, class1.Start, len));

                    if (class1.Length > len)
                    {
                        // adjust class1 for next time around
                        class1 = new ClassifiedRange(class1.Kind, class1.Start + len, class1.Length - len);
                    }
                    else
                    {
                        // advance class1 if no more
                        class1 = null;
                        index1++;
                    }
                }
                else if (class2.Start < class1.Start)
                {
                    // class2 starts before class1
                    var len = class1.Start - class2.Start;

                    // add the first part of class2
                    list.Add(new ClassifiedRange(class2.Kind, class2.Start, len));

                    if (class2.Length > len)
                    {
                        class2 = new ClassifiedRange(class2.Kind, class2.Start + len, class2.Length - len);
                    }
                    else
                    {
                        // advance class2 if no more
                        class2 = null;
                        index2++;
                    }
                }
                else if (class1.End <= class2.End)
                {
                    // both start at same position, class1 ends first
                    // just drop class1, since class2 takes precedence
                    class1 = null;
                    index1++;
                }
                else
                {
                    // both start at same position, class2 ends first
                    var len = class2.Length;

                    // add class2 since it takes precedence
                    list.Add(class2);
                    class2 = null;
                    index2++;

                    // adjust remainder of class1
                    class1 = new ClassifiedRange(class1.Kind, class1.Start + len, class1.Length - len);
                }
            }

            return list;
        }

        public override OutlineInfo GetOutlines(CancellationToken cancellationToken)
        {
            var firstToken = Parsing.TokenParser.ParseToken(this.Text, 0);
            if (firstToken != null && firstToken.Text.Length > 0)
            {
                var start = 0;
                var end = start + Parsing.TextFacts.TrimEnd(this.Text);

                // without better language knowledge, just use from start of first token to end of first line as the text to show when collapesed.
                var nextLineBreakStart = Parsing.TextFacts.GetNextLineBreakStart(this.Text, firstToken.Trivia.Length);
                var collapsedTextEnd = nextLineBreakStart >= 0 ? nextLineBreakStart : Text.Length;

                // trim off any extra trailing whitespace from collapsed text
                var collapsedTextLength = Parsing.TextFacts.TrimEnd(this.Text, firstToken.Trivia.Length, collapsedTextEnd - firstToken.Trivia.Length);

                var collapsedText = this.Text.Substring(firstToken.Trivia.Length, collapsedTextLength);

                return new OutlineInfo(new[] { new OutlineRange(start, end - start, collapsedText) });
            }

            return OutlineInfo.Empty;
        }

        public override bool ShouldAutoComplete(int position, char key, CancellationToken cancellationToken)
        {
            return false;
        }

        public override CompletionInfo GetCompletionItems(int position, CompletionOptions options, CancellationToken cancellationToken)
        {
            return CompletionInfo.Empty;
        }

        public override QuickInfo GetQuickInfo(int position, QuickInfoOptions options, CancellationToken cancellationToken)
        {
            return QuickInfo.Empty;
        }
        
        public override TextRange GetElement(int position, CancellationToken cancellationToken)
        {
            return new TextRange(0,0);
        }

        public override RelatedInfo GetRelatedElements(int position, FindRelatedOptions options, CancellationToken cancellationToken)
        {
            return RelatedInfo.Empty;
        }

        public override IReadOnlyList<ClusterReference> GetClusterReferences(CancellationToken cancellationToken)
        {
            return EmptyReadOnlyList<ClusterReference>.Instance;
        }

        public override IReadOnlyList<DatabaseReference> GetDatabaseReferences(CancellationToken cancellationToken)
        {
            return EmptyReadOnlyList<DatabaseReference>.Instance;
        }

        public override string GetMinimalText(MinimalTextKind kind, CancellationToken cancellationToken)
        {
            // use kusto lexer to identify tokens and trivia (as best guess)
            var list = new SyntaxList<SyntaxToken>(Parsing.TokenParser.ParseTokens(this.Text).Select(t => SyntaxToken.From(t)).ToArray());
            return list.ToString(KustoCodeService.GetIncludeTrivia(kind));
        }

        public override FormattedText GetFormattedText(FormattingOptions options, int cursorPosition, CancellationToken cancellationToken)
        {
            return new FormattedText(this.Text, cursorPosition);
        }

        private IReadOnlyList<ClientParameter> _clientParameters;

        public override IReadOnlyList<ClientParameter> GetClientParameters()
        {
            if (_clientParameters == null)
            {
                _clientParameters = GetClientParameters(this.Text, 0);
            }

            return _clientParameters;
        }

        internal IReadOnlyList<ClientParameter> GetClientParameters(string blockText, int offsetInScript)
        {
            List<ClientParameter> list = null;

            var start = 0;
            while (start < blockText.Length)
            {
                var openBrace = blockText.IndexOf("{", start);
                if (openBrace >= 0)
                {
                    var len = Parsing.TokenParser.ScanClientParameter(blockText, openBrace);
                    if (len > 0)
                    {
                        if (list == null)
                        {
                            list = new List<ClientParameter>();
                        }

                        list.Add(new ClientParameter(blockText.Substring(openBrace + 1, len - 2), openBrace + offsetInScript, len));
                        start = openBrace + len;
                    }
                    else
                    {
                        start = openBrace + 1;
                    }
                }
                else
                {
                    break;
                }
            }

            return list != null
                ? list.AsReadOnly()
                : EmptyReadOnlyList<ClientParameter>.Instance;
        }
    }
}