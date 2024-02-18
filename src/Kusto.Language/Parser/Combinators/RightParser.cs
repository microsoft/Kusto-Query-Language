using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    /// <summary>
    /// A parser that is allowed on the right side of an Apply.
    /// </summary>
    public struct RightParser<TInput, TOutput>
    {
        /// <summary>
        /// The underlying parser that is on the right side of an Apply.
        /// </summary>
        internal Parser<TInput, TOutput> Parser { get; }

        internal RightParser(Parser<TInput, TOutput> parser)
        {
            this.Parser = parser;
        }

        /// <summary>
        /// Creates a copy of this <see cref="RightParser{TInput, TOutput}"/> with the tag specified.
        /// </summary>
        public RightParser<TInput, TOutput> WithTag(string tag) => new RightParser<TInput, TOutput>(this.Parser.WithTag(tag));

        /// <summary>
        /// Creates a copy of this <see cref="RightParser{TInput, TOutput}"/> with the annotations specified.
        /// </summary>
        public RightParser<TInput, TOutput> WithAnnotations(IEnumerable<object> annotations) => new RightParser<TInput, TOutput>(this.Parser.WithAnnotations(annotations));

        /// <summary>
        /// Creates a copy of this <see cref="RightParser{TInput, TOutput}"/> with the IsHidden property specified.
        /// </summary>
        public RightParser<TInput, TOutput> WithIsHidden(bool isHidden) => new RightParser<TInput, TOutput>(this.Parser.WithIsHidden(isHidden));

        /// <summary>
        /// Creates a copy of this <see cref="RightParser{TInput, TOutput}"/> with the IsHidden property set to true.
        /// </summary>
        public RightParser<TInput, TOutput> Hide() => this.WithIsHidden(true);
    }
}
