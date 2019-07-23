using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Editor
{
    using Utils;

    public class ClassificationInfo
    {
        public IReadOnlyList<ClassifiedRange> Classifications { get; }

        public ClassificationInfo(IEnumerable<ClassifiedRange> classifications)
        {
            this.Classifications = classifications.ToReadOnly();
        }

        public static readonly ClassificationInfo Empty = new ClassificationInfo(null);
    }
}