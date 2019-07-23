using Kusto.Language;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kusto.Language.Analyzer
{
    public interface IRuleEngine
    {
        IReadOnlyList<RuleOutcome> Analyze(KustoCode code);
    }
}
