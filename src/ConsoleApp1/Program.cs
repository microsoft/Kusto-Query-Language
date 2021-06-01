using System;
using Kusto.Language;
using Kusto.Language.Symbols;
using Kusto.Language.Syntax;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            // parse only
            var query = "T | project a = a + b | where a > 10.0";
            var code = KustoCode.Parse(query);
        }
    }
}
