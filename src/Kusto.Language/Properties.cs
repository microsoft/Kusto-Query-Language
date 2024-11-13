using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language
{
    /// <summary>
    /// The set of predefined <see cref="GlobalStateProperty"/>.
    /// </summary>
    public static class Properties
    {
        /// <summary>
        /// This property is true client parameters are allowed.
        /// </summary>
        public static readonly GlobalStateProperty<bool> AllowClientParameters = 
            new GlobalStateProperty<bool>(nameof(AllowClientParameters));

        /// <summary>
        ///  The maximum text size allowed by the parser.
        ///  If this text size is exceeded the text will not be parsed.
        ///  Intellisense only.
        ///  This limit is used to improve typing speed for large queries.
        /// </summary>
        public static readonly GlobalStateProperty<int> MaxParseTextSize = 
            new GlobalStateProperty<int>(nameof(MaxParseTextSize), 4 * 1024 * 1024);

        /// <summary>
        /// The maximum depth allowed for syntax trees to be analyzed.
        /// If this limit is exceeded semantic anaysis will not be performed.
        /// This limit is used to prevent stack overflow.
        /// </summary>
        public static readonly GlobalStateProperty<int> MaxAnalysisDepth =
            new GlobalStateProperty<int>(nameof(MaxAnalysisDepth), 500);

        /// <summary>
        /// The maxmimum number of cached expansions per database function.
        /// </summary>
        public static readonly GlobalStateProperty<int> MaxCachedExpansions =
            new GlobalStateProperty<int>(nameof(MaxCachedExpansions), 10);

        /// <summary>
        /// The maximum number of cache result types per database function.
        /// </summary>
        public static readonly GlobalStateProperty<int> MaxCachedResultTypes =
            new GlobalStateProperty<int>(nameof(MaxCachedResultTypes), 50);
    }
}