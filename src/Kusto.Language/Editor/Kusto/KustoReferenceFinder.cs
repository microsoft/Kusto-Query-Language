using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Kusto.Language;
    using Kusto.Language.Symbols;
    using Syntax;
    using Utils;

    /// <summary>
    /// Functions for finding cluster and database name references in the code.
    /// </summary>
    internal class KustoReferenceFinder
    {
        private readonly KustoCode _code;

        public KustoReferenceFinder(
            KustoCode code)
        {
            _code = code;
        }

        /// <summary>
        /// Gets the references to cluster names in the code.
        /// </summary>
        public void GetClusterReferences(
            SyntaxNode root, 
            SyntaxNode location, 
            List<ClusterReference> clusters, 
            CancellationToken cancellationToken)
        {
            SyntaxElement.WalkNodes(
                root,
                fnBefore: element =>
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (element is Expression ex)
                    {
                        if (element is FunctionCallExpression fc
                            && ex.ReferencedSymbol == Functions.Cluster)
                        {
                            // this is a call to cluster('ccc'), report 'ccc' as a referenced cluster
                            var cluster = GetClusterReference(fc, location);
                            if (cluster != null)
                            {
                                clusters.Add(cluster);
                            }
                        }
                        else if (ex.GetCalledFunctionFacts() is FunctionBodyFacts funFacts)
                        {
                            if (funFacts.HasClusterCall)
                            {
                                // also get all cluster references inside the bodies of called functions
                                var calledBody = ex.GetCalledFunctionBody();
                                if (calledBody != null)
                                {
                                    GetClusterReferences(calledBody, location ?? GetBestFunctionCallLocation(ex), clusters, cancellationToken);
                                }
                            }
                        }
                    }
                    else if (element is Directive d)
                    {
                        var cluster = GetClusterReference(d);
                        if (cluster != null)
                        {
                            clusters.Add(cluster);
                        }
                    }
                },
                fnAfter: node =>
                {
                    if (node.Alternates != null)
                    {
                        foreach (var alt in node.Alternates)
                        {
                            GetClusterReferences(alt, location, clusters, cancellationToken);
                        }
                    }
                });
        }

        private static SyntaxNode GetBestFunctionCallLocation(Expression ex)
        {
            if (ex is FunctionCallExpression fc)
            {
                return fc.Name;
            }

            return ex;
        }

        private static ClusterReference GetClusterReference(FunctionCallExpression fc, SyntaxNode location)
        {
            if (fc.ReferencedSymbol == Functions.Cluster
                && TryGetConstantStringArgumentValue(fc, 0, out var name))
            {
                location = location ?? fc.ArgumentList.Expressions[0].Element;
                return new ClusterReference(name, location.TextStart, location.Width);
            }

            return null;
        }

        private static bool TryGetConstantStringArgumentValue(FunctionCallExpression fc, int index, out string constant)
        {
            if (fc.ArgumentList.Expressions.Count > index && fc.ArgumentList.Expressions[index].Element.IsConstant)
            {
                constant = fc.ArgumentList.Expressions[index].Element.ConstantValue as string;
                return constant != null;
            }

            constant = null;
            return false;
        }

        /// <summary>
        /// Gets all references to database names in the code.
        /// </summary>
        public void GetDatabaseReferences(
            SyntaxNode root, 
            SyntaxNode location, 
            ClusterSymbol currentCluster, 
            DatabaseSymbol currentDatabase, 
            List<DatabaseReference> refs, 
            CancellationToken cancellationToken)
        {
            SyntaxElement.WalkNodes(
                root,
                fnBefore: node =>
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (node is Expression ex)
                    {
                        if (ex is FunctionCallExpression fc
                            && ex.ReferencedSymbol == Functions.Database)
                        {
                            // this is a call to database('xxx').. record 'xxx' as a database referenced
                            var dbref = GetDatabaseReference(fc, location, currentCluster);
                            if (dbref != null)
                            {
                                refs.Add(dbref);
                            }
                        }
                        else if (ex.GetCalledFunctionFacts() is FunctionBodyFacts funFacts)
                        {
                            if (funFacts.HasDatabaseCall)
                            {
                                // also get all database references inside the bodies of called functions
                                var calledBody = ex.GetCalledFunctionBody();
                                if (calledBody != null)
                                {
                                    var db = _code.Globals.GetDatabase(ex.ReferencedSymbol) ?? currentDatabase;
                                    var cluster = _code.Globals.GetCluster(db) ?? currentCluster;
                                    GetDatabaseReferences(calledBody, location ?? GetBestFunctionCallLocation(ex), cluster, db, refs, cancellationToken);
                                }
                            }
                        }
                    }
                    else if (node is Directive d)
                    {
                        // directives can set the default database
                        var dbref = GetDatabaseReference(d, currentCluster);
                        if (dbref != null)
                        {
                            refs.Add(dbref);
                        }
                    }
                },
                fnAfter: node =>
                {
                    if (node.Alternates != null)
                    {
                        foreach (var alternate in node.Alternates)
                        {
                            GetDatabaseReferences(alternate, location, currentCluster, currentDatabase, refs, cancellationToken);
                        }
                    }
                }
            );
        }

        private ClusterReference GetClusterReference(Directive directive)
        {
            if (Binding.Binder.TryGetDirectiveClusterAndDatabase(directive, out var clusterName, out _)
                && !string.IsNullOrEmpty(clusterName))
            {
                return new ClusterReference(clusterName, directive.Token.TextStart, directive.Token.Width);
            }
            return null;
        }

        private DatabaseReference GetDatabaseReference(Directive directive, ClusterSymbol currentCluster)
        {
            if (Binding.Binder.TryGetDirectiveClusterAndDatabase(directive, out var clusterName, out var databaseName))
            {
                clusterName = clusterName ?? GetRelatedCluster(directive, currentCluster);
                return new DatabaseReference(databaseName, clusterName, directive.Token.TextStart, directive.Token.Width);
            }

            return null;
        }

        private string GetRelatedCluster(Directive directive, ClusterSymbol currentCluster)
        {
            // look for previous cluster directive
            if (directive.Parent is SyntaxList<Directive> list)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    var d = list[i];
                    if (d.TextStart < directive.TextStart
                        && d.Name == "cluster"
                        && d.Arguments.Count > 0)
                    {
                        return Binding.Binder.GetDirectiveArgumentStringValue(d.Arguments[0]);
                    }
                }
            }

            return currentCluster?.Name ?? _code.Globals.Cluster.Name;
        }

        private DatabaseReference GetDatabaseReference(
            FunctionCallExpression fc, SyntaxNode location, ClusterSymbol currentCluster)
        {
            if (fc.ReferencedSymbol == Functions.Database
                && TryGetConstantStringArgumentValue(fc, 0, out var databaseName))
            {
                location = location ?? fc.ArgumentList.Expressions[0].Element;

                string clusterName;

                // get cluster name from path expression if possible
                if (fc.Parent is PathExpression p
                    && p.Selector == fc
                    && p.Expression.ResultType is ClusterSymbol cs)
                {
                    clusterName = cs.Name;
                }
                else
                {
                    // otherwise use the current cluster
                    clusterName = currentCluster?.Name ?? _code.Globals.Cluster.Name;
                }

                return new DatabaseReference(databaseName, clusterName, location.GetPositionInOriginalTree(location.TextStart), location.Width);
            }

            return null;
        }
    }
}
