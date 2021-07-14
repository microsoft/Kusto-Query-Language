using System;
using System.Linq;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    public class CommandGrammarUtils
    {
        public static Grammar Reorder(Grammar grammar)
        {
            return GrammarReorderer.ReorderAlternations(grammar);
        }

        public static Grammar Unroll(Grammar grammar)
        {
            return GrammarUnroller.Unroll(grammar);
        }

        public static Grammar Require(Grammar grammar)
        {
            return GrammarRequirer.Require(grammar);
        }

        /// <summary>
        /// includes all the grammar transformations used by command parser factory
        /// </summary>
        public static Grammar Adjust(Grammar grammar)
        {
            // add require rules to make grammar partially parseable
            grammar = GrammarRequirer.Require(grammar);

            // reorder alternations so keywords don't get swallowed by name/identifier rules.
            grammar = GrammarReorderer.ReorderAlternations(grammar);

            return grammar;
        }
    }
}