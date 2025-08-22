using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Parsing;
    using Utils;

    [System.Diagnostics.DebuggerDisplay("ParserPath: {DebugText}")]
    public class ParserPath<TInput>
    {
        private readonly ParserPath<TInput> _parent;
        private readonly Parser<TInput> _parser;
        private readonly int _parentIndex;
        private ParserPath<TInput>[] _children;

        private string DebugText => _parent != null
            ? $"{_parent.Parser.Description}/{_parser.Description}"
            : $"{_parser.Description}";

        private ParserPath(ParserPath<TInput> parentPath, ParserAndIndex<TInput> parserAndIndex)
        {
            _parent = parentPath;
            _parser = parserAndIndex.Parser;
            _parentIndex = parserAndIndex.IndexInParent;
        }

        public ParserPath(ParserAndIndex<TInput> parserAndIndex)
            : this(null, parserAndIndex)
        {
        }

        /// <summary>
        /// This parser
        /// </summary>
        public Parser<TInput> Parser => _parser;

        /// <summary>
        /// The parent parser
        /// </summary>
        public ParserPath<TInput> Parent => _parent;

        /// <summary>
        /// Index of child in parent that this parser is.
        /// </summary>
        public int IndexInParent => _parentIndex;

        /// <summary>
        /// The number of child paths
        /// </summary>
        public int ChildCount => _parser.ChildParserCount;

        public ParserPath<TInput> GetChild(int index)
        {
            if (index < 0 || index >= ChildCount)
                return null;

            if (_children == null && ChildCount > 0)
            {
                var tmp = new ParserPath<TInput>[ChildCount];
                
                for (int i = 0; i < tmp.Length; i++)
                {
                    var parserAndIndex = new ParserAndIndex<TInput>(_parser.GetChildParser(i), i);
                    tmp[i] = new ParserPath<TInput>(this, parserAndIndex);
                }

                Interlocked.CompareExchange(ref _children, tmp, null);
            }

            return _children[index];
        }

        /// <summary>
        /// Construct a path from a sequence of parsers
        /// </summary>
        public static ParserPath<TInput> From(IReadOnlyList<ParserAndIndex<TInput>> path)
        {
            ParserPath<TInput> current = null;

            foreach (var parserAndIndex in path)
            {
                current = new ParserPath<TInput>(current, parserAndIndex);
            }

            return current;
        }
    }

    public struct ParserAndIndex<TInput>
    {
        public readonly Parser<TInput> Parser;
        public int IndexInParent;
        public ParserAndIndex(Parser<TInput> parser, int indexInParent)
        {
            this.Parser = parser;
            this.IndexInParent = indexInParent;
        }
    }
}