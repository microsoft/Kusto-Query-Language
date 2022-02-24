// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// WARNING: This file is auto generated during build. Do not modify manually.
using System;
using System.Linq;
using System.Collections.Generic;
using Kusto.Language.Symbols;
using Kusto.Language.Syntax;
using Kusto.Language.Editor;

namespace Kusto.Language.Parsing
{
    using static Parsers<LexicalToken>;
    using static SyntaxParsers;
    using Utils;
    using System.Text;

    public class ClusterManagerCommandGrammar : CommandGrammar
    {
        public ClusterManagerCommandGrammar(GlobalState globals) : base(globals)
        {
        }

        internal override Parser<LexicalToken, Command>[] CreateCommandParsers(PredefinedRuleParsers rules)
        {
            var ShowVersion = Command("ShowVersion", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    RequiredEToken("version")));

            var commandParsers = new Parser<LexicalToken, Command>[]
            {
                ShowVersion
            };

            return commandParsers;
        }
    }
}

