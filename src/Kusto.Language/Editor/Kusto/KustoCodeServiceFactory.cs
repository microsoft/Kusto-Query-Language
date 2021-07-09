using System;

namespace Kusto.Language.Editor
{
    /// <summary>
    /// A factory for creating <see cref="KustoCodeService"/>
    /// </summary>
    public class KustoCodeServiceFactory : CodeServiceFactory
    {
        /// <summary>
        /// The <see cref="GlobalState"/> to instantiate new <see cref="KustoCodeService"/> with.
        /// </summary>
        public GlobalState Globals { get; }

        /// <summary>
        /// Creates a new instance of <see cref="KustoCodeServiceFactory"/>.
        /// </summary>
        public KustoCodeServiceFactory(GlobalState globals)
        {
            if (globals == null)
                throw new ArgumentNullException(nameof(globals));

            // always use a cache
            this.Globals = globals.WithCache();
        }

        /// <summary>
        /// Creates a new <see cref="KustoCodeServiceFactory"/> with <see cref="P:Globals"/> changed.
        /// </summary>
        public KustoCodeServiceFactory WithGlobals(GlobalState globals)
        {
            if (this.Globals != globals)
            {
                return new KustoCodeServiceFactory(globals);
            }
            else
            {
                return this;
            }
        }

        public override bool TryGetCodeService(string text, out CodeService service)
        {
            if (IsKusto(text))
            {
                service = new KustoCodeService(text, this.Globals);
                return true;
            }
            else
            {
                service = null;
                return false;
            }
        }

        /// <summary>
        /// Returns true if the text is believed to be Kusto syntax.
        /// </summary>
        private static bool IsKusto(string text)
        {
            return !IsSql(text); // SQL is not kusto
        }

        /// <summary>
        /// Returns true if the text is believed to be SQL syntax.
        /// </summary>
        private static bool IsSql(string text)
        {
            var firstToken = Parsing.TokenParser.ParseToken(text, 0);
            if (firstToken == null)
                return false;

            // if it starts with kusto comments then it is not SQL
            if (!Parsing.TextFacts.IsWhitespaceOnly(firstToken.Trivia))
                return false;

            // check for -- which is sql line comment start
            // since kusto does not have -- token, check for two adjacent dash tokens
            if (firstToken.Text == "-")
            {
                var secondToken = Parsing.TokenParser.ParseToken(text, firstToken.Length);
                if (secondToken != null 
                    && secondToken.Text == "-" 
                    && secondToken.Trivia.Length == 0)
                {
                    return true;
                }
            }

            // check for known sql query keyword
            return string.Compare(firstToken.Text, "select", ignoreCase: true) == 0;
        }
    }
}