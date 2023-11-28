using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Symbols;
    using Syntax;
    using System.Text;
    using static ActorUtilities;

    internal static class KustoQualifier
    {
        /// <summary>
        /// Returns the query text with the database entities fully qualified.
        /// </summary>
        public static string FullyQualify(string query, GlobalState globals)
        {
            return Requalify(query, globals, null, null);
        }

        /// <summary>
        /// Returns the query text with the database entities minimally qualified.
        /// </summary>
        public static string MinimallyQualify(string query, GlobalState globals)
        {
            return Requalify(query, globals, globals.Cluster, globals.Database);
        }

        /// <summary>
        /// Returns the query text with the database entities minimally qualified to match the new default cluster and database.
        /// </summary>
        public static string Requalify(string text, GlobalState globals, ClusterSymbol newCluster, DatabaseSymbol newDatabase)
        {
            if (KustoCode.GetKind(text) != CodeKinds.Query)
                return text;
            var code = KustoCode.ParseAndAnalyze(text, globals);
            return Requalify(text, code.Syntax, globals, newCluster, newDatabase);
        }

        /// <summary>
        /// Requalify (minimally) the entity expressions within the text with respect to the target cluster and database.
        /// </summary>
        internal static EditString Requalify(
            EditString text, 
            SyntaxNode root, 
            GlobalState globals, 
            ClusterSymbol cluster, 
            DatabaseSymbol database)
        {
            var edits = new List<TextEdit>();

            var entityExpressions = root.GetDescendants<Expression>(n => 
                GetEntityExpression(n, globals) == n);

            foreach (var entity in entityExpressions)
            {
                // don't requalify expressions with leading entity group elements
                var groupPart = GetEntityExpressionEntityGroupElementPart(entity);
                if (groupPart != null)
                    continue;

                var clusterPart = GetEntityExpressionClusterPart(entity);
                var databasePart = GetEntityExpressionDatabasePart(entity);
                var entityPart = GetEntityExpressionEntityPart(entity, globals);
                var entityPartStart = entityPart != null ? entityPart.TextStart : entity.End;

                Symbol entitySymbol = GetEntityReference(entityPart);

                DatabaseSymbol entityDatabase = 
                    databasePart != null ? databasePart.ResultType as DatabaseSymbol
                    : (entitySymbol == null ? globals.Database : globals.GetDatabase(entitySymbol));

                if (entityDatabase != null && entityDatabase.Name == "")
                    entityDatabase = null;

                ClusterSymbol entityCluster =
                    clusterPart != null ? clusterPart.ResultType as ClusterSymbol
                    : ((entityDatabase == null) 
                        ? (databasePart != null ? globals.Cluster : null)
                        : globals.GetCluster(entityDatabase));

                if (entityCluster != null && entityCluster.Name == "")
                    entityCluster = null;

                if (entityCluster == cluster 
                    && entityDatabase == database
                    && cluster != null 
                    && database != null
                    && entityPart != null)
                {
                    // entity reference is qualified and matches new defaults, so remove qualification
                    edits.Add(TextEdit.Deletion(entity.TextStart, entityPartStart - entity.TextStart));
                }
                else if (entityCluster == cluster && cluster != null)
                {
                    // entity reference is for correct cluster but not correct database, replace existing qualifier with database qualifier.
                    var dbQualifier = GetDatabaseQualifier(databasePart, entityDatabase) ?? "";
                    var entityText = entityPart != null ? entityPart.ToString(IncludeTrivia.Interior) : "";
                    var replacementText = JoinPath(dbQualifier, entityText);
                    edits.Add(TextEdit.Replacement(entity.TextStart, entity.Width, replacementText));
                }
                else
                {
                    // entity reference does not match target database or cluster, replace existing qualifier with full qualification
                    var dbQualifier = GetDatabaseQualifier(databasePart, entityDatabase) ?? "";
                    var clusterQualifier = GetClusterQualifier(clusterPart, entityCluster) ?? "";
                    var entityText = entityPart != null ? entityPart.ToString(IncludeTrivia.Interior) : "";
                    var replacementText = JoinPath(clusterQualifier, dbQualifier, entityText);
                    edits.Add(TextEdit.Replacement(entity.TextStart, entity.Width, replacementText));
                }
            }

            return text.ApplyAll(edits);
        }

        private static string JoinPath(params string[] elements)
        {
            var builder = new StringBuilder();

            foreach (var element in elements)
            {
                if (!string.IsNullOrWhiteSpace(element))
                {
                    if (builder.Length > 0)
                        builder.Append(".");
                    builder.Append(element);
                }
            }

            return builder.ToString();
        }

        private static string GetDatabaseQualifier(Expression dbPart, DatabaseSymbol entityDatabase)
        {
            if (dbPart != null)
                return dbPart.ToString(IncludeTrivia.Interior);

            if (entityDatabase != null)
            {
                var dbName = entityDatabase.AlternateName.Length > 0 ? entityDatabase.AlternateName : entityDatabase.Name;
                var dbNameLiteral = KustoFacts.GetSingleQuotedStringLiteral(dbName);
                return $"database({dbNameLiteral})";
            }

            return null;
        }

        private static string GetClusterQualifier(Expression clusterPart, ClusterSymbol entityCluster)
        {
            if (clusterPart != null)
                return clusterPart.ToString(IncludeTrivia.Interior);

            if (entityCluster != null)
            {
                var clusterNameLiteral = KustoFacts.GetSingleQuotedStringLiteral(entityCluster.Name);
                return $"cluster({clusterNameLiteral})";
            }

            return null;
        }
    }
}