using Kusto.Language.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Analyzer
{
    public interface IRule
    {
        string Name { get; }
        string Description { get; }
        IReadOnlyList<RuleOutcome> Analyze(KustoCode code);

    }
}
