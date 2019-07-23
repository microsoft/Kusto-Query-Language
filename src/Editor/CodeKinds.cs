using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    /// <summary>
    /// Known code kinds.
    /// </summary>
    public static class CodeKinds
    {
        /// <summary>
        /// The code is a Kusto Query.
        /// </summary>
        public const string Query = nameof(Query);

        /// <summary>
        /// The code is a Kusto Commmand.
        /// </summary>
        public const string Command = nameof(Command);

        /// <summary>
        /// The code is a Kusto Directive
        /// </summary>
        public const string Directive = nameof(Directive);

        /// <summary>
        /// The code kind is not known.
        /// </summary>
        public const string Unknown = nameof(Unknown);
    }
}