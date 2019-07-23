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

            this.Globals = globals;
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
            var firstToken = GetFirstKustoToken(text);
            return string.Compare(firstToken, "select", ignoreCase: true) == 0;
        }

        /// <summary>
        /// Gets the text of the first token as understood using Kusto syntax.
        /// </summary>
        private static string GetFirstKustoToken(string text)
        {
            var firstToken = Parsing.LexicalGrammar.GetFirstToken(text);
            return firstToken?.Text ?? string.Empty;
        }
    }
}