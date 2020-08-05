using System;
using System.Collections;
using System.Collections.Generic;

namespace Kusto.Language
{
    using Syntax;

    [System.Diagnostics.DebuggerDisplay("{Severity}: ({Start}..{Start+Length}): {Message}")]
    public sealed class Diagnostic
    {
        /// <summary>
        /// The code that uniquely identifies the specific kind of diagnostic.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// The category of the diagnostic; Correctness, Performance, General, etc
        /// </summary>
        public string Category { get; }

        /// <summary>
        /// The severity of the diagnostic; Error, Warning, Suggestion, etc
        /// </summary>
        public string Severity { get; }

        /// <summary>
        /// A short description of the diagnostic.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The message of the diagnostic.
        /// </summary>
        public string Message { get; }

        private readonly int start;
        private readonly int length;

        public Diagnostic(string code, string message)
            : this(code, category: null, severity: null, message: message, description: null, start: 0, length: 0)
        {
        }

        public Diagnostic(string code, string category, string severity, string description)
            : this(code, category, severity, description: description, message: null, start: 0, length: 0)
        {
        }

        public Diagnostic(string code, string category, string severity, string description, string message)
            : this(code, category, severity, description: description, message: message, start: 0, length: 0)
        {
        }


        private Diagnostic(string code, string category, string severity, string description, string message, int start, int length)
        {
            this.Code = code ?? "";
            this.Category = category ?? DiagnosticCategory.General;
            this.Severity = severity ?? DiagnosticSeverity.Error;
            this.Description = description ?? message ?? "";
            this.Message = message ?? description ?? "";
            this.start = start >= 0 ? start: 0;
            this.length = length >= 0 ? length: 0;
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

        private Diagnostic With(
            string code = null,
            string category = null,
            string severity = null,
            string description = null,
            string message = null,
            int start = -1,
            int length = -1)
        {
            code = code ?? this.Code;
            category = category ?? this.Category;
            severity = severity ?? this.Severity;
            description = description ?? this.Description;
            message = message ?? this.Message;
            start = start >= 0 ? start : this.start;
            length = length >= 0 ? length : this.length;

            if (code != this.Code
                || category != this.Code
                || severity != this.Severity
                || description != this.Description
                || message != this.Message
                || start != this.start
                || length != this.length)
            {
                return new Diagnostic(code, category, severity, description, message, start, length);
            }
            else
            {
                return this;
            }
        }

        public Diagnostic WithCode(string code)
        {
            return With(code: code);
        }

        public Diagnostic WithCategory(string category)
        {
            return With(category: category);
        }

        public Diagnostic WithSeverity(string severity)
        {
            return With(severity: severity);
        }

        public Diagnostic WithMessage(string message)
        {
            return With(message: message);
        }

        public Diagnostic WithLocation(SyntaxElement location)
        {
            return With(start: location.TextStart, length: location.Width);
        }

        public Diagnostic WithLocation(int start, int length)
        {
            return With(start: start, length: length);
        }

        public static IReadOnlyList<Diagnostic> NoDiagnostics = new Diagnostic[0];
    }

    public static class DiagnosticCategory
    {
        public static string General = nameof(General);
        public static string Correctness = nameof(Correctness);
        public static string Performance = nameof(Performance);
    }

    public static class DiagnosticSeverity
    {
        /// <summary>
        /// A diagnostic that represents code that will fail to execute.
        /// </summary>
        public const string Error = nameof(Error);

        /// <summary>
        /// A diagnostic that represents code that will execute but with possible unintended consequence.
        /// </summary>
        public const string Warning = nameof(Warning);

        /// <summary>
        /// A diagnostic that represents a suggestion to improve the code.
        /// </summary>
        public const string Suggestion = nameof(Suggestion);

        /// <summary>
        /// A diagnostic that represents information about the code.
        /// </summary>
        public const string Information = nameof(Information);

        /// <summary>
        /// A diagnostic that is not meant to be relayed to the user.
        /// </summary>
        public const string Hidden = nameof(Hidden);
    }
}