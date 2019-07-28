using System;
using System.Collections.Generic;

namespace Kusto.Language.Utils
{
    public static class EmptyReadOnlyList<T>
    {
        public static IReadOnlyList<T> Instance = new List<T>().AsReadOnly();
    }
}