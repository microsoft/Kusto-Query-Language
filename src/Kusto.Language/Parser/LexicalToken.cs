using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Kusto.Language.Syntax;

namespace Kusto.Language.Parsing
{
    /// <summary>
    /// A lexeme produced by <see cref="LexicalGrammar"/> that includes leading whitespace and the text of a single identifiable piece of 
    /// the language grammar such as an identifier, keyword or punctuation.
    /// </summary>
    [DebuggerDisplay("{DebugText}")]
    public class LexicalToken : IToken
    {
        /// <summary>
        /// The starting position of the token (including trivia) in the source text.
        /// </summary>
        public int TriviaStart { get; }

        /// <summary>
        /// The length of the token in characters (including trivia)
        /// </summary>
        public int FullWidth { get; }

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

        public LexicalToken(int start, SyntaxKind kind, string trivia, string text, IReadOnlyList<Diagnostic> diagnostics = null)
        {
            this.TriviaStart = start;
            this.Kind = kind;
            this.Trivia = trivia ?? "";
            this.Text = text ?? "";
            this.FullWidth = this.Trivia.Length + this.Text.Length;
            this.Diagnostics = diagnostics ?? Diagnostic.NoDiagnostics;
        }

        /// <summary>
        /// The character position after the end of this token.
        /// </summary>
        public int End => TriviaStart + FullWidth;

        public int TriviaWidth => this.Trivia.Length;

        public int TextStart => this.TriviaStart + this.Trivia.Length;

        public int Width => this.Text.Length;

        private string DebugText => this.Text.Length > 0 ? this.Text : SyntaxFacts.GetText(this.Kind);
    }

    public interface IToken
    {
        /// <summary>
        /// The starting position of the token (including trivia) in the source.
        /// </summary>
        int TriviaStart { get; }

        /// <summary>
        /// The starting position of the token text (excluding trivia) in the source.
        /// </summary>
        int TextStart { get; }

        /// <summary>
        /// The length of the token in characters (including trivia)
        /// </summary>
        int FullWidth { get; }

        /// <summary>
        /// The length of the token text in characters (excluding trivia)
        /// </summary>
        int Width { get; }

        /// <summary>
        /// The kind of the token
        /// </summary>
        SyntaxKind Kind { get; }

        /// <summary>
        /// The trivia (whitespace, etc) that preceeds the proper text of the token.
        /// </summary>
        string Trivia { get; }

        /// <summary>
        /// The text of the token.
        /// </summary>
        string Text { get; }
    }
}