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
// 
// WARNING: Do not modify this file
//          This file is auto generated from the template file 'DataManagerCommandGrammar.tt'
//          Instead modify the corresponding input info file in the Kusto.Language.Generator project.
//          After making changes, use the right-click menu on the .tt file and select 'run custom tool'.

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

    public class DataManagerCommandGrammar : CommandGrammar
    {
        public DataManagerCommandGrammar(GlobalState globals) : base(globals)
        {
        }

        internal override CommandParserInfo[] CreateCommandParsers(PredefinedRuleParsers rules)
        {
            var ShowVersion = Command("ShowVersion", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("version")));

            var commandParsers = new CommandParserInfo[]
            {
                new CommandParserInfo("ShowVersion", ShowVersion)
            };

            return commandParsers;
        }
    }
}

