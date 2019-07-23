using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Utils;

    public static class CodeServiceFeatures
    {
        public const string Diagnostics = nameof(Diagnostics);
        public const string Classification = nameof(Classification);
        public const string Completion = nameof(Completion);
        public const string Outlining = nameof(Outlining);
        public const string QuickInfo = nameof(QuickInfo);
        public const string ClusterReferences = nameof(ClusterReferences);
        public const string DatabaseReferences = nameof(DatabaseReferences);
        public const string MinimalText = nameof(MinimalText);
        public const string Formatting = nameof(Formatting);
        public const string ClientParameters = nameof(ClientParameters);
    }
}