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
            // can either be a single grammar or a hashset or null
            private object _branchesFrom;

            // all the branches for common terms 
            // can either be a single KeyValuePair, Dictionary or null
            private object _branchesTo;

            public Node(Grammar term)
            {
                if (term != null)
                {
                    AddBranchesFrom(term);
                }
            }

            private void AddBranchesFrom(Grammar term)
            {
                if (_branchesFrom == null)
                {
                    _branchesFrom = term;
                }
                else if (_branchesFrom is Grammar g)
                {
                    if (term != g)
                    {
                        // items is unique by instance here
                        var hs = new HashSet<Grammar>();
                        hs.Add(g);
                        hs.Add(term);
                        _branchesFrom = hs;
                    }
                }
                else if (_branchesFrom is HashSet<Grammar> hset)
                {
                    hset.Add(term);
                }
            }

            private IEnumerable<Grammar> GetBranchesFrom()
            {
                if (_branchesFrom is Grammar g)
                {
                    return new[] { g };
                }
                else if (_branchesFrom is HashSet<Grammar> hs)
                {
                    return hs;
                }
                else
                {
                    return EmptyReadOnlyList<Grammar>.Instance;
                }
            }

            private void AddBranchesTo(Grammar term, Node nextNode)
            {
                if (_branchesTo == null)
                {
                    _branchesTo = new KeyValuePair<Grammar, Node>(term, nextNode);
                }
                else if (_branchesTo is KeyValuePair<Grammar, Node> kvp)
                {
                    // keys don't have to be unique instances here, but similar
                    if (!GrammarEquivalenceComparer.Instance.Equals(kvp.Key, term))
                    {
                        var d = new Dictionary<Grammar, Node>(GrammarEquivalenceComparer.Instance);
                        d.Add(kvp.Key, kvp.Value);
                        d.Add(term, nextNode);
                        _branchesTo = d;
                    }
                }
                else if (_branchesTo is Dictionary<Grammar, Node> bd)
                {
                    bd.Add(term, nextNode);
                }
            }

            private bool TryGetBranchTo(Grammar term, out Node node)
            {
                if (_branchesTo is KeyValuePair<Grammar, Node> kvp)
                {
                    if (GrammarEquivalenceComparer.Instance.Equals(kvp.Key, term))
                    {
                        node = kvp.Value;
                        return true;
                    }
                    else
                    {
                        node = null;
                        return false;
                    }
                }
                else if (_branchesTo is Dictionary<Grammar, Node> d)
                {
                    return d.TryGetValue(term, out node);
                }
                else
                {
                    node = null;
                    return false;
                }
            }

            private IEnumerable<Node> GetBranchesTo()
            {
                if (_branchesTo is KeyValuePair<Grammar, Node> kvp)
                {
                    return new[] { kvp.Value };
                }
                else if (_branchesTo is Dictionary<Grammar, Node> d)
                {
                    return d.Values;
                }
                else
                {
                    return Utils.EmptyReadOnlyList<Node>.Instance;
                }
            }

            public Node AddTerm(Grammar term)
            {
                if (!TryGetBranchTo(term, out var nextNode))
                {
                    nextNode = new Node(term);
                    AddBranchesTo(term, nextNode);
                }
                else
                {
                    nextNode.AddBranchesFrom(term);
                }

                return nextNode;
            }

            public Node GetNext(Grammar term)
            {
                TryGetBranchTo(term, out var nextNode);
                return nextNode;
            }

            public void GetUniqueTerms(HashSet<Grammar> terms)
            {
                if (_branchesFrom is Grammar g)
                {
                    // we only get here from one grammar
                    terms.Add(g);
                }
                else
                {
                    foreach (var node in GetBranchesTo())
                    {
                        node.GetUniqueTerms(terms);
                    }
                }
            }

            public void GetNthTerms(int n, List<Grammar> terms)
            {
                if (n >= 0)
                {
                    foreach (var node in GetBranchesTo())
                    {
                        node.GetNthTerms(n - 1, terms);
                    }
                }
                else
                {
                    terms.AddRange(GetBranchesFrom());
                }
            }

            public void GetAlternativeTerms(Dictionary<Grammar, IReadOnlyList<Grammar>> map)
            {
                var list = GetBranchesTo().SelectMany(n => n.GetBranchesFrom()).ToList();

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

                foreach (var node in GetBranchesTo())
                {
                    node.GetAlternativeTerms(map);
                }
            }
        }
    }
}