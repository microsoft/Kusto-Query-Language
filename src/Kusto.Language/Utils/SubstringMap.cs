using System;
using System.Collections;
using System.Collections.Generic;

namespace Kusto.Language.Utils
{
    /// <summary>
    /// Represents a map between a set of string keys and values
    /// that can be used to quickly find the longest match in a region of another string.
    /// </summary>
    public class SubstringMap<TValue>
    {
        private readonly Node _root;

        public SubstringMap(IEnumerable<KeyValuePair<string, TValue>> keyValuePairs)
        {
            _root = Node.From(keyValuePairs);
        }

        /// <summary>
        /// Finds the 
        /// </summary>
        public KeyValuePair<string, TValue> GetLongestMatch(string text, int start)
        {
            Node bestNode = null;

            var node = _root;
            for (var pos = start; pos < text.Length; pos++)
            {
                node = node.GetSubNode(text[pos]);
                if (node == null)
                    break;
                if (node.HasValue)
                {
                    bestNode = node;
                }
            }

            if (bestNode != null)
                return bestNode.Value;

            return NoValue;
        }

        private static readonly KeyValuePair<string, TValue> NoValue = 
            new KeyValuePair<string, TValue>("", default(TValue));

        private class Node
        {
            /// <summary>
            /// The <see cref="KeyValuePair{TKey, TValue}"/> resolvable at this node.
            /// </summary>
            public KeyValuePair<string, TValue> Value { get; private set; }
            public bool HasValue { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            private CharMap map;

            private Node()
            {
            }

            public static Node From(IEnumerable<KeyValuePair<string, TValue>> keyValuePairs)
            {
                var node = new Node();

                foreach (var pair in keyValuePairs)
                {
                    node.Add(pair);
                }

                return node;
            }

            private void Add(KeyValuePair<string, TValue> pair)
            {
                var node = this;
                Node subNode;

                foreach (var ch in pair.Key)
                {
                    if (node.map == null)
                    {
                        subNode = new Node();
                        node.map = new SingleCharMap(ch, subNode);
                    }
                    else
                    {
                        subNode = node.map.Get(ch);

                        if (subNode == null)
                        {
                            subNode = new Node();
                            node.map = node.map.Add(ch, subNode);
                        }
                    }

                    node = subNode;
                }

                node.Value = pair;
                node.HasValue = true;
            }

            /// <summary>
            /// Gets the <see cref="Node"/> corresponding to the key.
            /// </summary>
            public Node GetSubNode(char key)
            {
                return this.map?.Get(key);
            }

            private abstract class CharMap
            {
                public abstract CharMap Add(char key, Node node);
                public abstract Node Get(char key);
            }

            private class SingleCharMap : CharMap
            {
                private readonly char _key;
                private readonly Node _node;

                public SingleCharMap(char key, Node node)
                {
                    _key = key;
                    _node = node;
                }

                public override CharMap Add(char key, Node node)
                {
                    return new ArrayCharMap(3).Add(_key, _node).Add(key, node);
                }

                public override Node Get(char key)
                {
                    return _key == key ? _node : null;
                }
            }

            private class ArrayCharMap : CharMap
            {
                private readonly KeyValuePair<char, Node>[] _pairs;
                private int _length;

                public ArrayCharMap(int count)
                {
                    _pairs = new KeyValuePair<char, Node>[count];
                    _length = 0;
                }

                public override CharMap Add(char key, Node node)
                {
                    if (_length < _pairs.Length - 1)
                    {
                        _pairs[_length] = new KeyValuePair<char, Node>(key, node);
                        _length++;
                        return this;
                    }
                    else
                    {
                        return new DictionaryMap(_pairs).Add(key, node);
                    }
                }

                public override Node Get(char key)
                {
                    for (int i = 0; i < _length; i++)
                    {
                        var kvp = _pairs[i];
                        if (kvp.Key == key)
                            return kvp.Value;
                    }

                    return null;
                }
            }

            private class DictionaryMap : CharMap
            {
                private readonly Dictionary<char, Node> _map;

                public DictionaryMap(KeyValuePair<char, Node>[] kvps = null)
                {
                    _map = new Dictionary<char, Node>();

                    if (kvps != null)
                    {
                        foreach (var kvp in kvps)
                        {
                            _map.Add(kvp.Key, kvp.Value);
                        }
                    }
                }

                public override CharMap Add(char key, Node node)
                {
                    _map.Add(key, node);
                    return this;
                }

                public override Node Get(char key)
                {
                    _map.TryGetValue(key, out var value);
                    return value;
                }
            }
        }
    }
}