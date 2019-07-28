using System;
using System.Collections;
using System.Collections.Generic;

namespace Kusto.Language.Utils
{
    /// <summary>
    /// A class that maintains a set of unique names.
    /// </summary>
    internal class UniqueNameTable
    {
        private readonly Dictionary<string, int> nameMap
            = new Dictionary<string, int>();

        public UniqueNameTable()
        {
        }

        /// <summary>
        /// Adds names already known to be unique to the table.
        /// </summary>
        public void AddNames(IEnumerable<string> names)
        {
            foreach (var name in names)
            {
                this.AddName(name);
            }
        }

        public void AddName(string name)
        {
            if (!string.IsNullOrEmpty(name) && !this.nameMap.ContainsKey(name)) // check to avoid exception if input is bad?
            {
                this.nameMap.Add(name, 0);
            }
        }

        /// <summary>
        /// Adds a name to the table if it is unique, otherwise creates a new unique name.
        /// Returns either the original unique name or the newly created name.
        /// </summary>
        /// <param name="name">The candidate name that you would prefer to use.</param>
        /// <param name="baseName">An optional base name to use when formulating a new name.</param>
        public string GetOrAddName(string name, string baseName = null)
        {
            if (this.nameMap.TryGetValue(name, out var lastUsedSuffix))
            {
                for (int suffix = lastUsedSuffix + 1; suffix < Int32.MaxValue; suffix++)
                {
                    var newName = (baseName ?? name) + suffix;
                    if (!this.nameMap.ContainsKey(newName))
                    {
                        // remember greatest suffix
                        this.nameMap[name] = suffix;

                        // record this name too
                        this.nameMap.Add(newName, 0);

                        return newName;
                    }
                }
            }
            else
            {
                // remember name and suffix
                this.nameMap.Add(name, 0);
            }

            return name;
        }

        public void Clear()
        {
            this.nameMap.Clear();
        }
    }
}