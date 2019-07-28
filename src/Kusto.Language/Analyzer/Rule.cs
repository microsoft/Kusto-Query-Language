using System;
using System.Collections.Generic;
using System.Text;
using Kusto.Language;
using Kusto.Language.Syntax;

namespace Kusto.Language.Analyzer
{
    public sealed class RuleOutcome
    {
        public RuleOutcome(
            string ruleName,
            int score,
            string message,
            string referenceText,
            Severity severity,
            Category category,
            int textStart)
        {
            RuleName = ruleName;
            Score = score;
            Message = message;
            Severity = severity;
            Category = category;
            TextStart = textStart;
            ReferenceText = referenceText;
        }

        public string RuleName { get; private set; }
        public int Score { get; private set; }
        public string ReferenceText { get; private set; }
        public string Message { get; private set; }
        public Severity Severity { get; private set; }
        public Category Category { get; private set; }
        public int TextStart { get; private set; }
    }

    public enum Severity
    {
        Suggestion,
        Warning,
        Error,
    }

    public enum Category
    {
        Correctness,
        Performance
    }
}
