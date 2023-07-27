using System;
using System.Collections;
using System.Collections.Generic;

namespace Kusto.Language
{
    using Syntax;

    [System.Diagnostics.DebuggerDisplay("{Severity}: ({Start}..{Start+Length}): {Message}")]
    public sealed class Diagnostic : IEquatable<Diagnostic>
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
            : this(code, category: null, severity: null, message: message, description: null, locationKind: DiagnosticLocationKind.Relative, start: 0, length: 0)
        {
        }

        public Diagnostic(string code, string category, string severity, string description)
            : this(code, category, severity, description: description, message: null, locationKind: DiagnosticLocationKind.Relative, start: 0, length: 0)
        {
        }

        public Diagnostic(string code, string category, string severity, string description, string message)
            : this(code, category, severity, description: description, message: message, locationKind: DiagnosticLocationKind.Relative, start: 0, length: 0)
        {
        }

        private Diagnostic(string code, string category, string severity, string description, string message, DiagnosticLocationKind locationKind, int start, int length)
        {
            this.Code = code ?? "";
            this.Category = category ?? DiagnosticCategory.General;
            this.Severity = severity ?? DiagnosticSeverity.Error;
            this.Description = description ?? message ?? "";
            this.Message = message ?? description ?? "";
            this.LocationKind = locationKind;
            this.start = start >= 0 ? start: 0;
            this.length = length >= 0 ? length: 0;
        }

        /// <summary>
        /// True if the diagnostic has an known source location
        /// </summary>
        public bool HasLocation => this.LocationKind == DiagnosticLocationKind.Absolute;

        /// <summary>
        /// The kind of diagnositc location.
        /// </summary>
        public DiagnosticLocationKind LocationKind { get; }

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
            DiagnosticLocationKind? locationKind = null,
            int start = -1,
            int length = -1)
        {
            code = code ?? this.Code;
            category = category ?? this.Category;
            severity = severity ?? this.Severity;
            description = description ?? this.Description;
            message = message ?? this.Message;
            var useLocationKind = locationKind != null ? locationKind.Value : this.LocationKind;
            start = start >= 0 ? start : this.start;
            length = length >= 0 ? length : this.length;

            if (code != this.Code
                || category != this.Code
                || severity != this.Severity
                || description != this.Description
                || message != this.Message
                || useLocationKind != this.LocationKind
                || start != this.start
                || length != this.length)
            {
                return new Diagnostic(code, category, severity, description, message, useLocationKind, start, length);
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

        public Diagnostic WithDescription(string description)
        {
            return With(description: description);
        }

        public Diagnostic WithMessage(string message)
        {
            return With(message: message);
        }

        public Diagnostic WithLocation(SyntaxElement location)
        {
            return With(locationKind: DiagnosticLocationKind.Absolute, start: location.TextStart, length: location.Width);
        }

        public Diagnostic WithLocation(int start, int length)
        {
            return With(locationKind: DiagnosticLocationKind.Absolute, start: start, length: length);
        }

        public Diagnostic WithLocationKind(DiagnosticLocationKind locationKind)
        {
            return With(locationKind: locationKind);
        }

        public static IReadOnlyList<Diagnostic> NoDiagnostics = new Diagnostic[0];

        public bool Equals(Diagnostic other) =>
            this.Code == other.Code
            && this.Message == other.Message
            && this.HasLocation 
            && other.HasLocation
            && this.Start == other.Start
            && this.Length == other.Length;

        public override bool Equals(object obj) =>
            obj is Diagnostic d && Equals(d);

        public override int GetHashCode() =>
            this.Code.GetHashCode()
            + this.Message.GetHashCode()
            + this.Start;
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

    public enum DiagnosticLocationKind
    {
        /// <summary>
        /// The diagnostic location is known with absolute start and length values.
        /// </summary>
        Absolute,

        /// <summary>
        /// The diagnostic location is unknown, but relative to the syntax item it is associated with.
        /// </summary>
        Relative,

        /// <summary>
        /// The diagnostic location is unknown, but after the end of the syntax item it is associated with.
        /// </summary>
        RelativeEnd
    }
}