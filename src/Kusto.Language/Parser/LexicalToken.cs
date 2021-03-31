using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Kusto.Language.Syntax;

namespace Kusto.Language.Parsing
{
    /// <summary>
    /// A piece of source text that represents simple identifiers, keywords, literals or punctuation.
    /// <see cref="LexicalToken"/>'s are produced by <see cref="TokenParser"/>
    /// </summary>
    [DebuggerDisplay("{DebugText}")]
    public class LexicalToken
    {
        /// <summary>
        /// The kind of the token
        /// </summary>
        public SyntaxKind Kind { get; }

        /// <summary>
        /// The trivia (whitespace, etc) that preceeds the proper text of the token.
        /// </summary>
        public string Trivia { get; }

        /// <summary>
        /// The text of the token.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Any diagnostics associated with the token.
        /// </summary>
        public IReadOnlyList<Diagnostic> Diagnostics { get; }

        public LexicalToken(SyntaxKind kind, string trivia, string text, IReadOnlyList<Diagnostic> diagnostics = null)
        {
            this.Kind = kind;
            this.Trivia = trivia ?? "";
            this.Text = text ?? "";
            this.Diagnostics = diagnostics ?? Diagnostic.NoDiagnostics;
        }

        /// <summary>
        /// The combined length of the trivia and text of the token
        /// </summary>
        public int Length => this.Trivia.Length + this.Text.Length;

        private string DebugText => this.Text.Length > 0 ? this.Text : SyntaxFacts.GetText(this.Kind);
    }
}
