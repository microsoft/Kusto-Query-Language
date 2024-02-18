using System;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    public sealed class MapParser<TInput, TOutput> : ResultPrimaryParser<TInput, TOutput>
    {
        private readonly Node root;

        private MapParser(Node root)
        {
            this.root = root;

        }
        public MapParser(IEnumerable<KeyValuePair<IEnumerable<TInput>, Func<TOutput>>> keyValuePairs)
            : this(Node.From(keyValuePairs))
        {
        }

        public override bool IsMatch => true;

        public override int ChildParserCount => 0;

        public override Parser<TInput> GetChildParser(int index)
        {
            return null;
        }

        public override void Accept(ParserVisitor<TInput> visitor)
        {
            visitor.VisitMap(this);
        }

        public override TResult Accept<TResult>(ParserVisitor<TInput, TResult> visitor)
        {
            return visitor.VisitMap(this);
        }

        public override TResult Accept<TArg, TResult>(ParserVisitor<TInput, TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitMap(this, arg);
        }

        protected override Parser<TInput> Clone()
        {
            return new MapParser<TInput, TOutput>(this.root);
        }

        public override ParseResult<TOutput> Parse(Source<TInput> source, int start)
        {
            var node = root;
            var length = 0;

            var bestLength = -1;
            Func<TOutput> bestOutput = null;

            while (true)
            {
                if (source.IsEnd(start + length))
                    break;

                var input = source.Peek(start + length);

                Node subNode;
                if (node.TryGetValueNode(input, out subNode))
                {
                    length++;

                    if (subNode.HasValue)
                    {
                        bestLength = length;
                        bestOutput = subNode.Value;
                    }

                    node = subNode;
                }
                else
                {
                    break;
                }
            }

            if (bestLength > 0)
            {
                var bestValue = bestOutput();
                return new ParseResult<TOutput>(bestLength, bestValue);
            }
            else
            {
                return new ParseResult<TOutput>(bestLength, default(TOutput));
            }
        }

        public override int Scan(Source<TInput> source, int start)
        {
            var node = root;
            var length = 0;

            var bestLength = -1;

            while (true)
            {
                if (source.IsEnd(start + length))
                    break;

                var input = source.Peek(start + length);

                Node subNode;
                if (node.TryGetValueNode(input, out subNode))
                {
                    length++;

                    if (subNode.HasValue)
                    {
                        bestLength = length;
                    }

                    node = subNode;
                }
                else
                {
                    break;
                }
            }

            return bestLength;
        }

        private class Node
        {
            public bool HasValue { get; private set; }

            public Func<TOutput> Value { get; private set; }

            private Dictionary<TInput, Node> map;

            private Node()
            {
            }

            public static Node From(IEnumerable<KeyValuePair<IEnumerable<TInput>, Func<TOutput>>> keyValuePairs)
            {
                var node = new Node();

                foreach (var pair in keyValuePairs)
                {
                    node.Add(pair.Key, 0, pair.Value);
                }

                return node;
            }

            private void Add(IEnumerable<TInput> sequence, int pos, Func<TOutput> value)
            {
                var node = this;

                foreach (var item in sequence)
                {
                    if (node.map == null)
                    {
                        node.map = new Dictionary<TInput, Node>();
                    }

                    Node subNode;
                    if (!node.map.TryGetValue(item, out subNode))
                    {
                        subNode = new Node();
                        node.map.Add(item, subNode);
                    }

                    node = subNode;
                }

                node.HasValue = true;
                node.Value = value;
            }

            public bool TryGetValueNode(TInput key, out Node node)
            {
                if (this.map != null)
                {
                    return this.map.TryGetValue(key, out node);
                }

                node = null;
                return false;
            }
        }
    }
}