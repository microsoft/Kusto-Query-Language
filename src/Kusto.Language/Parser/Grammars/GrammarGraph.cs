using System;
using System.Linq;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    using Utils;

    /// <summary>
    /// A class that models the merger of a set of alternate grammars
    /// </summary>
    internal class GrammarGraph
    {
        private readonly Node _root;
#if DEBUG
        private readonly Dictionary<Grammar, Node> _grammarToNodeMap;
#endif

        public GrammarGraph()
        {
            _root = new Node(null);

#if DEBUG
            _grammarToNodeMap = new Dictionary<Grammar, Node>();
#endif
        }

        public GrammarGraph(Grammar grammar)
            : this()
        {
            Add(grammar);
        }

        public GrammarGraph(IReadOnlyList<Grammar> alternates)
            : this()
        {
            foreach (var alt in alternates)
            {
                Add(alt);
            }
        }

        /// <summary>
        /// Adds all the grammar terms in the grammar tree
        /// </summary>
        public void Add(Grammar root)
        {
            Add(_root, root, null);
        }

        /// <summary>
        /// Get all the terms that occur only once along their path
        /// </summary>
        public void GetUniqueTerms(HashSet<Grammar> terms)
        {
            _root.GetUniqueTerms(terms);
        }

        /// <summary>
        /// Gets all the terms that can match the nth input item
        /// </summary>
        public void GetNthTerms(int n, List<Grammar> terms)
        {
            _root.GetNthTerms(n, terms);
        }

        /// <summary>
        /// Gets a map of all the alternative terms that can occur in the same position as given term.
        /// The resulting list will include the grammar itself.
        /// </summary>
        public void GetAlternativeTerms(Dictionary<Grammar, IReadOnlyList<Grammar>> map)
        {
            _root.GetAlternativeTerms(map);
        }

        private void Add(Node root, Grammar grammar, HashSet<Node> nextNodes)
        {
            switch (grammar)
            {
                case TokenGrammar _:
                case RuleGrammar _:
                    var nn = root.AddTerm(grammar);
                    nextNodes?.Add(nn);
#if DEBUG
                    if (_grammarToNodeMap.ContainsKey(grammar))
                    {
                        // not sure yet to do anything about this
                        // System.Diagnostics.Debug.Fail("Grammar already assigned to a node");
                    }
                    else
                    {
                        _grammarToNodeMap.Add(grammar, nn);
                    }
#endif
                    break;
                case SequenceGrammar seq:
                    AddSteps(seq, 0, root, nextNodes);
                    break;
                case AlternationGrammar alt:
                    foreach (var a in alt.Alternatives)
                    {
                        Add(root, a, nextNodes);
                    }
                    break;
                case OneOrMoreGrammar oom:
                    Add(root, oom.Repeated, nextNodes);
                    break;
                case ZeroOrMoreGrammar zom:
                    // add root for zero case
                    nextNodes?.Add(root);
                    Add(root, zom.Repeated, nextNodes);
                    break;
                case OptionalGrammar opt:
                    // add root for zero case
                    nextNodes?.Add(root);
                    Add(root, opt.Optioned, nextNodes);
                    break;
                case RequiredGrammar req:
                    Add(root, req.Required, nextNodes);
                    break;
                case TaggedGrammar tag:
                    Add(root, tag.Tagged, nextNodes);
                    break;
                case HiddenGrammar hid:
                    Add(root, hid.Hidden, nextNodes);
                    break;
                default:
                    throw new InvalidOperationException($"Unhandled grammar {grammar.GetType().Name}");
            }
        }

        private void AddSteps(SequenceGrammar grammar, int iStep, Node root, HashSet<Node> finalNext)
        {
            var isFinal = iStep == grammar.Steps.Count - 1;

            HashSet<Node> stepNext = !isFinal
                ? s_nodeListPool.AllocateFromPool()
                : finalNext;

            var step = grammar.Steps[iStep];
            Add(root, step, stepNext);

            if (!isFinal)
            {
                AddSteps(grammar, iStep + 1, stepNext, finalNext);
                s_nodeListPool.ReturnToPool(stepNext);
            }
        }

        private void AddSteps(SequenceGrammar grammar, int iStep, HashSet<Node> roots, HashSet<Node> finalNext)
        {
            var isFinal = iStep == grammar.Steps.Count - 1;
            HashSet<Node> stepNext = !isFinal ? s_nodeListPool.AllocateFromPool() : finalNext;

            foreach (var root in roots)
            {
                var step = grammar.Steps[iStep];
                Add(root, step, stepNext);
            }

            if (!isFinal)
            {
                AddSteps(grammar, iStep + 1, stepNext, finalNext);
                s_nodeListPool.ReturnToPool(stepNext);
            }
        }

        private static readonly ObjectPool<HashSet<Node>> s_nodeListPool =
            new ObjectPool<HashSet<Node>>(() => new HashSet<Node>(), hs => hs.Clear());

        private class Node
        {
            // all the grammar terms from alternate sources that branched to here
            private readonly HashSet<Grammar> _terms;

            // all the branches for common terms 
            private readonly Dictionary<Grammar, Node> _branches;

            public Node(Grammar term)
            {
                _terms = new HashSet<Grammar>();
                _branches = new Dictionary<Grammar, Node>(GrammarEquivalenceComparer.Instance);
                if (term != null)
                {
                    _terms.Add(term);
                }
            }

            public Node AddTerm(Grammar term)
            {
                if (!_branches.TryGetValue(term, out var nextNode))
                {
                    nextNode = new Node(term);
                    _branches.Add(term, nextNode);
                }
                else
                {
                    nextNode._terms.Add(term);
                }

                return nextNode;
            }

            public Node GetNext(Grammar term)
            {
                _branches.TryGetValue(term, out var nextNode);
                return nextNode;
            }

            public void GetUniqueTerms(HashSet<Grammar> terms)
            {
                if (_terms.Count == 1)
                {
                    // get the unique term here
                    terms.UnionWith(_terms);
                }
                else
                {
                    foreach (var node in _branches.Values)
                    {
                        node.GetUniqueTerms(terms);
                    }
                }
            }

            public void GetNthTerms(int n, List<Grammar> terms)
            {
                if (n >= 0)
                {
                    foreach (var node in _branches.Values)
                    {
                        node.GetNthTerms(n - 1, terms);
                    }
                }
                else
                {
                    terms.AddRange(_terms);
                }
            }

            public void GetAlternativeTerms(Dictionary<Grammar, IReadOnlyList<Grammar>> map)
            {
                var list = _branches.Values.SelectMany(n => n._terms).ToList();

                foreach (var term in list)
                {
                    if (map.TryGetValue(term, out var existingList))
                    {
                        //map[term] = existingList.Concat(list).ToList();
                    }
                    else
                    {
                        map[term] = list;
                    }
                }

                foreach (var node in _branches.Values)
                {
                    node.GetAlternativeTerms(map);
                }
            }
        }
    }
}