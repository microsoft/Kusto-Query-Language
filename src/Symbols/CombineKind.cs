using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    public enum CombineKind
    {
        /// <summary>
        /// Multiple columns with same name and type become one column.
        /// Columns with same name and different type are renamed to be unique with a numeric suffix added.
        /// </summary>
        UnifySameNameAndType, // union style

        /// <summary>
        /// Multiple columns with the same name will become one column.
        /// If the types differ, the column type will be dynamic.
        /// </summary>
        UnifySameName, // find style

        /// <summary>
        /// Columns with the same name will be be renamed to be unique with a numeric suffix added.
        /// </summary>
        UniqueNames
    }
}