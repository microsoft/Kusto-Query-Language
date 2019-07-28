using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Editor
{
    /// <summary>
    /// A <see cref="ClassificationKind"/> and text range.
    /// </summary>
    public class ClassifiedRange
    {
        /// <summary>
        /// The classification.
        /// </summary>
        public ClassificationKind Kind { get; }

        /// <summary>
        /// The starting text position of the classified range.
        /// </summary>
        public int Start { get; }

        /// <summary>
        /// The length (in characters) of the classified range.
        /// </summary>
        public int Length { get; }

        public ClassifiedRange(ClassificationKind kind, int start, int length)
        {
            this.Kind = kind;
            this.Start = start;
            this.Length = length;
        }

        /// <summary>
        /// The end of the classification range.
        /// </summary>
        public int End => this.Start + this.Length;
    }
}