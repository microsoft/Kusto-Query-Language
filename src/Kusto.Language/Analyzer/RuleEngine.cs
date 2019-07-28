using System;
using System.Collections.Generic;
using System.Text;
using Kusto.Language;

namespace Kusto.Language.Analyzer
{
    public class RuleEngine : IRuleEngine
    {
        public IEnumerable<IRule> Rules { get; private set; }
        public IReadOnlyList<RuleOutcome> Analyze(KustoCode code)
        {
            var outcomes = new List<RuleOutcome>();
            foreach (var rule in Rules)
            {
                outcomes.AddRange(rule.Analyze(code));
            }
            return outcomes;
        }

        public static IRuleEngine Create(IEnumerable<IRule> rules)
        {
            var engine = new RuleEngine();
            engine.Rules = rules;
            return engine;
        }
    }
}
