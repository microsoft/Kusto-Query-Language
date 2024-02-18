using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Parsing;
    using Utils;

    public class ParserPath<TInput>
    {
        private readonly ParserPath<TInput> _parent;
        private readonly Parser<TInput> _parser;
        private ParserPath<TInput>[] _children;

        private ParserPath(ParserPath<TInput> parentPath, Parser<TInput> parser)
        {
            _parent = parentPath;
            _parser = parser;
        }

        public ParserPath(Parser<TInput> parser)
            : this(null, parser)
        {
        }

        public Parser<TInput> Parser => _parser;
        public ParserPath<TInput> Parent => _parent;

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
                    tmp[i] = new ParserPath<TInput>(this, _parser.GetChildParser(i));
                }

                Interlocked.CompareExchange(ref _children, tmp, null);
            }

            return _children[index];
        }

        public static ParserPath<TInput> From(IReadOnlyList<Parser<TInput>> path)
        {
            ParserPath<TInput> current = null;

            foreach (var parser in path)
            {
                current = new ParserPath<TInput>(current, parser);
            }

            return current;
        }
    }
}