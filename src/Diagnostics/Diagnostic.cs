using System;
using System.Collections;
using System.Collections.Generic;

namespace Kusto.Language
{
    using Syntax;

    [System.Diagnostics.DebuggerDisplay("({Start}..{Start+Length}): {Message}")]
    public sealed class Diagnostic
    {
        public string Message { get; }

        private readonly int start;
        private readonly int length;

        public Diagnostic(string message)
            : this(message, 0, 0)
        {
        }

        private Diagnostic(string message, int start, int length)
        {
            this.Message = message;
            this.start = start;
            this.length = length;
        }

        /// <summary>
        /// True if the diagnostic has a source location
        /// </summary>
        public bool HasLocation => this.length > 0;

        /// <summary>
        /// Start of diagnostic location in the source.
        /// </summary>
        public int Start => this.start;

        /// <summary>
        /// Length of diagnostic location in the source.
        /// </summary>
        public int Length => this.length;

        /// <summary>
        /// The position after the end of the diagnostic in source.
        /// </summary>
        public int End => this.Start + this.Length;

        public Diagnostic WithLocation(SyntaxElement location)
        {
            return new Diagnostic(this.Message, location.TextStart, location.Width);
        }

        public Diagnostic WithLocation(int start, int length)
        {
            return new Diagnostic(this.Message, start, length);
        }

        public static IReadOnlyList<Diagnostic> NoDiagnostics = new Diagnostic[0];
    }
}