// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// WARNING: This file is auto generated during build. Do not modify manually.
using System;
using System.Linq;
using System.Collections.Generic;
using Kusto.Language.Symbols;
using Kusto.Language.Syntax;
using Kusto.Language.Editor;

namespace Kusto.Language.Parsing
{
    using static Parsers<LexicalToken>;
    using static SyntaxParsers;
    using Utils;
    using System.Text;

    public class EngineCommandGrammar : CommandGrammar
    {
        public EngineCommandGrammar(GlobalState globals) : base(globals)
        {
        }

        internal override Parser<LexicalToken, Command>[] CreateCommandParsers(PredefinedRuleParsers rules)
        {
            var AddClusterBlockedPrincipals = Command("AddClusterBlockedPrincipals", 
                Custom(
                    EToken("add", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("blockedprincipals"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        First(
                            Custom(
                                EToken("application"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                Optional(
                                    Custom(
                                        EToken("user"),
                                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                                        new [] {CD(), CD("UserName", CompletionHint.Literal)})),
                                new [] {CD(), CD("AppName", CompletionHint.Literal), CD(isOptional: true)}),
                            Custom(
                                EToken("user"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                new [] {CD(), CD("UserName", CompletionHint.Literal)}))),
                    Optional(
                        First(
                            Custom(
                                EToken("period"),
                                Required(rules.Value, rules.MissingValue),
                                Optional(
                                    Custom(
                                        EToken("reason"),
                                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                                        new [] {CD(), CD("Reason", CompletionHint.Literal)})),
                                new [] {CD(), CD("Period", CompletionHint.Literal), CD(isOptional: true)}),
                            Custom(
                                EToken("reason"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                new [] {CD(), CD("Reason", CompletionHint.Literal)}))),
                    new [] {CD(), CD(), CD(), CD("Principal", CompletionHint.Literal), CD(isOptional: true), CD(isOptional: true)}));

            var AddClusterRole = Command("AddClusterRole", 
                Custom(
                    EToken("add", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    RequiredEToken("admins", "databasecreators", "users", "viewers"),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                CD("Principal", CompletionHint.Literal)),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredEToken(")"),
                    Optional(
                        First(
                            Custom(
                                EToken("skip-results"),
                                Optional(
                                    Custom(
                                        rules.StringLiteral,
                                        CD("Notes", CompletionHint.Literal))),
                                new [] {CD("SkipResults"), CD(CompletionHint.Literal, isOptional: true)}),
                            Custom(
                                rules.StringLiteral,
                                CD("Notes", CompletionHint.Literal)))),
                    new [] {CD(), CD(), CD("Role"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var AddDatabaseRole = Command("AddDatabaseRole", 
                Custom(
                    EToken("add", CompletionKind.CommandPrefix),
                    EToken("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredEToken("admins", "ingestors", "monitors", "unrestrictedviewers", "users", "viewers"),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                CD("Principal", CompletionHint.Literal)),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredEToken(")"),
                    Optional(
                        First(
                            Custom(
                                EToken("skip-results"),
                                Optional(
                                    Custom(
                                        rules.StringLiteral,
                                        CD("Notes", CompletionHint.Literal))),
                                new [] {CD("SkipResults"), CD(CompletionHint.Literal, isOptional: true)}),
                            Custom(
                                rules.StringLiteral,
                                CD("Notes", CompletionHint.Literal)))),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD("Role"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var AddExternalTableAdmins = Command("AddExternalTableAdmins", 
                Custom(
                    EToken("add", CompletionKind.CommandPrefix),
                    EToken("external"),
                    RequiredEToken("table"),
                    Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                    RequiredEToken("admins"),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                CD("principal", CompletionHint.Literal)),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredEToken(")"),
                    Optional(
                        First(
                            Custom(
                                EToken("skip-results"),
                                Optional(
                                    Custom(
                                        rules.StringLiteral,
                                        CD("notes", CompletionHint.Literal))),
                                new [] {CD(), CD(CompletionHint.Literal, isOptional: true)}),
                            Custom(
                                rules.StringLiteral,
                                CD("notes", CompletionHint.Literal)))),
                    new [] {CD(), CD(), CD(), CD("externalTableName", CompletionHint.ExternalTable), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var AddFollowerDatabaseAuthorizedPrincipals = Command("AddFollowerDatabaseAuthorizedPrincipals", 
                Custom(
                    EToken("add", CompletionKind.CommandPrefix),
                    EToken("follower"),
                    RequiredEToken("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    Required(
                        First(
                            Custom(
                                EToken("from"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                RequiredEToken("admins", "monitors", "unrestrictedviewers", "users", "viewers"),
                                new [] {CD(), CD("leaderClusterMetadataPath", CompletionHint.Literal), CD("operationRole")}),
                            Custom(
                                EToken("admins", "monitors", "unrestrictedviewers", "users", "viewers"),
                                CD("operationRole"))),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD("leaderClusterMetadataPath", CompletionHint.Literal), CD("operationRole")}, CreateMissingEToken("from"), rules.MissingStringLiteral(), CreateMissingEToken("Expected admins,monitors,unrestrictedviewers,users,viewers"))),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                CD("principal", CompletionHint.Literal)),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredEToken(")"),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            CD("notes", CompletionHint.Literal))),
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(CompletionHint.Literal, isOptional: true)}));

            var AddFunctionRole = Command("AddFunctionRole", 
                Custom(
                    EToken("add", CompletionKind.CommandPrefix),
                    EToken("function"),
                    Required(rules.DatabaseFunctionNameReference, rules.MissingNameReference),
                    RequiredEToken("admins"),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                CD("Principal", CompletionHint.Literal)),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredEToken(")"),
                    Optional(
                        First(
                            Custom(
                                EToken("skip-results"),
                                Optional(
                                    Custom(
                                        rules.StringLiteral,
                                        CD("Notes", CompletionHint.Literal))),
                                new [] {CD("SkipResults"), CD(CompletionHint.Literal, isOptional: true)}),
                            Custom(
                                rules.StringLiteral,
                                CD("Notes", CompletionHint.Literal)))),
                    new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD("Role"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var AddMaterializedViewAdmins = Command("AddMaterializedViewAdmins", 
                Custom(
                    EToken("add", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    RequiredEToken("admins"),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                CD("principal", CompletionHint.Literal)),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredEToken(")"),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            CD("notes", CompletionHint.Literal))),
                    new [] {CD(), CD(), CD("materializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(CompletionHint.Literal, isOptional: true)}));

            var AddTableRole = Command("AddTableRole", 
                Custom(
                    EToken("add", CompletionKind.CommandPrefix),
                    RequiredEToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredEToken("admins", "ingestors"),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                CD("Principal", CompletionHint.Literal)),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredEToken(")"),
                    Optional(
                        First(
                            Custom(
                                EToken("skip-results"),
                                Optional(
                                    Custom(
                                        rules.StringLiteral,
                                        CD("Notes", CompletionHint.Literal))),
                                new [] {CD("SkipResults"), CD(CompletionHint.Literal, isOptional: true)}),
                            Custom(
                                rules.StringLiteral,
                                CD("Notes", CompletionHint.Literal)))),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD("Role"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var AlterMergeClusterPolicyCallout = Command("AlterMergeClusterPolicyCallout", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("callout"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("Policy", CompletionHint.Literal)}));

            var AlterMergeClusterPolicyCapacity = Command("AlterMergeClusterPolicyCapacity", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("capacity"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("Policy", CompletionHint.Literal)}));

            var AlterMergeClusterPolicyDiagnostics = Command("AlterMergeClusterPolicyDiagnostics", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("diagnostics"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("PolicyName", CompletionHint.Literal)}));

            var AlterMergeClusterPolicyMultiDatabaseAdmins = Command("AlterMergeClusterPolicyMultiDatabaseAdmins", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("multidatabaseadmins"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("Policy", CompletionHint.Literal)}));

            var AlterMergeClusterPolicyQueryWeakConsistency = Command("AlterMergeClusterPolicyQueryWeakConsistency", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("query_weak_consistency"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("Policy", CompletionHint.Literal)}));

            var AlterMergeClusterPolicyRequestClassification = Command("AlterMergeClusterPolicyRequestClassification", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("request_classification"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("Policy", CompletionHint.Literal)}));

            var AlterMergeClusterPolicySharding = Command("AlterMergeClusterPolicySharding", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("sharding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal)}));

            var AlterMergeClusterPolicyStreamingIngestion = Command("AlterMergeClusterPolicyStreamingIngestion", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("streamingingestion"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal)}));

            var AlterMergeClusterPolicyRowStore = Command("AlterMergeClusterPolicyRowStore", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    RequiredEToken("policy"),
                    RequiredEToken("rowstore"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("RowStorePolicy", CompletionHint.Literal)}));

            var AlterMergeColumnPolicyEncoding = Command("AlterMergeColumnPolicyEncoding", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("column"),
                    Required(rules.TableColumnNameReference, rules.MissingNameReference),
                    RequiredEToken("policy"),
                    RequiredEToken("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD(), CD("EncodingPolicy", CompletionHint.Literal)}));

            var AlterMergeDatabasePolicyDiagnostics = Command("AlterMergeDatabasePolicyDiagnostics", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("diagnostics"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("PolicyName", CompletionHint.Literal)}));

            var AlterMergeDatabasePolicyEncoding = Command("AlterMergeDatabasePolicyEncoding", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("EncodingPolicy", CompletionHint.Literal)}));

            var AlterMergeDatabasePolicyMerge = Command("AlterMergeDatabasePolicyMerge", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("merge"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("MergePolicy", CompletionHint.Literal)}));

            var AlterMergeDatabasePolicyRetention = Command("AlterMergeDatabasePolicyRetention", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("retention"),
                    Required(
                        First(
                            Custom(
                                EToken("recoverability"),
                                RequiredEToken("="),
                                RequiredEToken("disabled", "enabled"),
                                new [] {CD(), CD(), CD("RecoverabilityValue")}),
                            Custom(
                                EToken("softdelete"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                Optional(
                                    Custom(
                                        EToken("recoverability"),
                                        RequiredEToken("="),
                                        RequiredEToken("disabled", "enabled"),
                                        new [] {CD(), CD(), CD("RecoverabilityValue")})),
                                new [] {CD(), CD(), CD("SoftDeleteValue", CompletionHint.Literal), CD(isOptional: true)}),
                            Custom(
                                rules.StringLiteral,
                                CD("RetentionPolicy", CompletionHint.Literal))),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("RecoverabilityValue")}, CreateMissingEToken("recoverability"), CreateMissingEToken("="), CreateMissingEToken("Expected disabled,enabled"))),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD()}));

            var AlterMergeDatabasePolicySharding = Command("AlterMergeDatabasePolicySharding", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("sharding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("ShardingPolicy", CompletionHint.Literal)}));

            var AlterMergeDatabasePolicyShardsGrouping = Command("AlterMergeDatabasePolicyShardsGrouping", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("shards_grouping").Hide(),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("ShardsGroupingPolicy", CompletionHint.Literal)}));

            var AlterMergeDatabasePolicyStreamingIngestion = Command("AlterMergeDatabasePolicyStreamingIngestion", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredEToken("policy"),
                    RequiredEToken("streamingingestion"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("StreamingIngestionPolicy", CompletionHint.Literal)}));

            var AlterMergeMaterializedViewPolicyPartitioning = Command("AlterMergeMaterializedViewPolicyPartitioning", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("policy"),
                    EToken("partitioning"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD("Policy", CompletionHint.Literal)}));

            var AlterMergeMaterializedViewPolicyRetention = Command("AlterMergeMaterializedViewPolicyRetention", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    RequiredEToken("policy"),
                    RequiredEToken("retention"),
                    Required(
                        First(
                            Custom(
                                EToken("recoverability"),
                                RequiredEToken("="),
                                RequiredEToken("disabled", "enabled"),
                                new [] {CD(), CD(), CD("RecoverabilityValue")}),
                            Custom(
                                EToken("softdelete"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                Optional(
                                    Custom(
                                        EToken("recoverability"),
                                        RequiredEToken("="),
                                        RequiredEToken("disabled", "enabled"),
                                        new [] {CD(), CD(), CD("RecoverabilityValue")})),
                                new [] {CD(), CD(), CD("SoftDeleteValue", CompletionHint.Literal), CD(isOptional: true)}),
                            Custom(
                                rules.StringLiteral,
                                CD("RetentionPolicy", CompletionHint.Literal))),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("RecoverabilityValue")}, CreateMissingEToken("recoverability"), CreateMissingEToken("="), CreateMissingEToken("Expected disabled,enabled"))),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD()}));

            var AlterMergeTablePolicyEncoding = Command("AlterMergeTablePolicyEncoding", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("EncodingPolicy", CompletionHint.Literal)}));

            var AlterMergeTablePolicyMerge = Command("AlterMergeTablePolicyMerge", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("merge"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("MergePolicy", CompletionHint.Literal)}));

            var AlterMergeTablePolicyRetention = Command("AlterMergeTablePolicyRetention", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("retention"),
                    Required(
                        First(
                            Custom(
                                EToken("recoverability"),
                                RequiredEToken("="),
                                RequiredEToken("disabled", "enabled"),
                                new [] {CD(), CD(), CD("RecoverabilityValue")}),
                            Custom(
                                EToken("softdelete"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                Optional(
                                    Custom(
                                        EToken("recoverability"),
                                        RequiredEToken("="),
                                        RequiredEToken("disabled", "enabled"),
                                        new [] {CD(), CD(), CD("RecoverabilityValue")})),
                                new [] {CD(), CD(), CD("SoftDeleteValue", CompletionHint.Literal), CD(isOptional: true)}),
                            Custom(
                                rules.StringLiteral,
                                CD("RetentionPolicy", CompletionHint.Literal))),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("RecoverabilityValue")}, CreateMissingEToken("recoverability"), CreateMissingEToken("="), CreateMissingEToken("Expected disabled,enabled"))),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD()}));

            var AlterMergeTablePolicyRowOrder = Command("AlterMergeTablePolicyRowOrder", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("roworder"),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.ColumnNameReference,
                                RequiredEToken("asc", "desc"),
                                new [] {CD("ColumnName", CompletionHint.Column), CD()}),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.Column), CD()}, rules.MissingNameReference(), CreateMissingEToken("Expected asc,desc")),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.Column), CD()}, rules.MissingNameReference(), CreateMissingEToken("Expected asc,desc"))))),
                    RequiredEToken(")"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD(CompletionHint.Column), CD()}));

            var AlterMergeTablePolicySharding = Command("AlterMergeTablePolicySharding", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("sharding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("ShardingPolicy", CompletionHint.Literal)}));

            var AlterMergeTablePolicyStreamingIngestion = Command("AlterMergeTablePolicyStreamingIngestion", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("streamingingestion"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("StreamingIngestionPolicy", CompletionHint.Literal)}));

            var AlterMergeTablePolicyUpdate = Command("AlterMergeTablePolicyUpdate", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    RequiredEToken("policy"),
                    RequiredEToken("update"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("UpdatePolicy", CompletionHint.Literal)}));

            var AlterMergeTable = Command("AlterMergeTable", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.NameDeclarationOrStringLiteral,
                                RequiredEToken(":"),
                                Required(rules.Type, rules.MissingType),
                                new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType())))),
                    RequiredEToken(")"),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        First(
                                            EToken("docstring"),
                                            EToken("folder"),
                                            If(Not(And(EToken("docstring", "folder"))), rules.NameDeclarationOrStringLiteral)),
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("docstring"), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("docstring"), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"))),
                    new [] {CD(), CD(), CD(CompletionHint.Table), CD(), CD(CompletionHint.None), CD(), CD(isOptional: true)}));

            var AlterMergeTableColumnDocStrings = Command("AlterMergeTableColumnDocStrings", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("column-docstrings"),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.ColumnNameReference,
                                RequiredEToken(":"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                new [] {CD("ColumnName", CompletionHint.Column), CD(), CD("DocString", CompletionHint.Literal)}),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.Column), CD(), CD("DocString", CompletionHint.Literal)}, rules.MissingNameReference(), CreateMissingEToken(":"), rules.MissingStringLiteral()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.Column), CD(), CD("DocString", CompletionHint.Literal)}, rules.MissingNameReference(), CreateMissingEToken(":"), rules.MissingStringLiteral())))),
                    RequiredEToken(")"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(CompletionHint.Column), CD()}));

            var AlterMergeTablePolicyPartitioning = Command("AlterMergeTablePolicyPartitioning", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredEToken("policy"),
                    RequiredEToken("partitioning"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("Policy", CompletionHint.Literal)}));

            var AlterMergeWorkloadGroup = Command("AlterMergeWorkloadGroup", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    RequiredEToken("workload_group"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("WorkloadGroupName", CompletionHint.None), CD("WorkloadGroup", CompletionHint.Literal)}));

            var AlterCache = Command("AlterCache", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("cache"),
                    RequiredEToken("on"),
                    Required(
                        First(
                            EToken("*"),
                            Custom(
                                rules.BracketedStringLiteral,
                                CD("NodeList", CompletionHint.Literal))),
                        () => CreateMissingEToken("*")),
                    Required(rules.BracketedStringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("Action", CompletionHint.Literal)}));

            var AlterClusterPolicyCaching = Command("AlterClusterPolicyCaching", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("caching"),
                    Required(
                        First(
                            Custom(
                                EToken("hotdata"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredEToken("hotindex"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                new [] {CD(), CD(), CD("HotData", CompletionHint.Literal), CD(), CD(), CD("HotIndex", CompletionHint.Literal)}),
                            Custom(
                                EToken("hot"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                new [] {CD(), CD(), CD("Timespan", CompletionHint.Literal)})),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("HotData", CompletionHint.Literal), CD(), CD(), CD("HotIndex", CompletionHint.Literal)}, CreateMissingEToken("hotdata"), CreateMissingEToken("="), rules.MissingValue(), CreateMissingEToken("hotindex"), CreateMissingEToken("="), rules.MissingValue()))));

            var AlterClusterPolicyCallout = Command("AlterClusterPolicyCallout", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("callout"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("Policy", CompletionHint.Literal)}));

            var AlterClusterPolicyCapacity = Command("AlterClusterPolicyCapacity", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("capacity"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("Policy", CompletionHint.Literal)}));

            var AlterClusterPolicyDiagnostics = Command("AlterClusterPolicyDiagnostics", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("diagnostics"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("PolicyName", CompletionHint.Literal)}));

            var AlterClusterPolicyIngestionBatching = Command("AlterClusterPolicyIngestionBatching", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("ingestionbatching"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("IngestionBatchingPolicy", CompletionHint.Literal)}));

            var AlterClusterPolicyManagedIdentity = Command("AlterClusterPolicyManagedIdentity", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("managed_identity"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("ManagedIdentityPolicy", CompletionHint.Literal)}));

            var AlterClusterPolicyMultiDatabaseAdmins = Command("AlterClusterPolicyMultiDatabaseAdmins", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("multidatabaseadmins"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("Policy", CompletionHint.Literal)}));

            var AlterClusterPolicyQueryWeakConsistency = Command("AlterClusterPolicyQueryWeakConsistency", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("query_weak_consistency"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("Policy", CompletionHint.Literal)}));

            var AlterClusterPolicyRequestClassification = Command("AlterClusterPolicyRequestClassification", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("request_classification"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    RequiredEToken("<|"),
                    Required(rules.CommandInput, rules.MissingExpression),
                    new [] {CD(), CD(), CD(), CD(), CD("Policy", CompletionHint.Literal), CD(), CD("Query", CompletionHint.Tabular)}));

            var AlterClusterPolicyRowStore = Command("AlterClusterPolicyRowStore", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("rowstore"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("RowStorePolicy", CompletionHint.Literal)}));

            var AlterClusterPolicySandbox = Command("AlterClusterPolicySandbox", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("sandbox"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("SandboxPolicy", CompletionHint.Literal)}));

            var AlterClusterPolicySharding = Command("AlterClusterPolicySharding", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("sharding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal)}));

            var AlterClusterPolicyStreamingIngestion = Command("AlterClusterPolicyStreamingIngestion", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    RequiredEToken("streamingingestion"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("StreamingIngestionPolicy", CompletionHint.Literal)}));

            var AlterClusterStorageKeys = Command("AlterClusterStorageKeys", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            EToken("async"),
                            EToken("cluster")),
                        Custom(
                            EToken("async"),
                            RequiredEToken("cluster")),
                        EToken("cluster")),
                    RequiredEToken("storage"),
                    RequiredEToken("keys"),
                    Required(
                        First(
                            EToken("decryption-certificate-thumbprint"),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("decryption-certificate-thumbprint"),
                                new [] {CD(), CD(), CD(CompletionHint.None), CD(), CD()})),
                        () => CreateMissingEToken("decryption-certificate-thumbprint")),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD(), CD("thumbprint", CompletionHint.Literal)}));

            var AlterColumnPolicyCaching = Command("AlterColumnPolicyCaching", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("column"),
                    rules.DatabaseTableColumnNameReference,
                    EToken("policy"),
                    EToken("caching"),
                    Required(
                        First(
                            Custom(
                                EToken("hotdata"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredEToken("hotindex"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                new [] {CD(), CD(), CD("HotData", CompletionHint.Literal), CD(), CD(), CD("HotIndex", CompletionHint.Literal)}),
                            Custom(
                                EToken("hot"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                new [] {CD(), CD(), CD("Timespan", CompletionHint.Literal)})),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("HotData", CompletionHint.Literal), CD(), CD(), CD("HotIndex", CompletionHint.Literal)}, CreateMissingEToken("hotdata"), CreateMissingEToken("="), rules.MissingValue(), CreateMissingEToken("hotindex"), CreateMissingEToken("="), rules.MissingValue())),
                    new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD(), CD()}));

            var AlterColumnPolicyEncodingType = Command("AlterColumnPolicyEncodingType", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("column"),
                    rules.DatabaseTableColumnNameReference,
                    EToken("policy"),
                    EToken("encoding"),
                    EToken("type"),
                    RequiredEToken("="),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD(), CD(), CD(), CD("EncodingPolicyType", CompletionHint.Literal)}));

            var AlterColumnPolicyEncoding = Command("AlterColumnPolicyEncoding", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("column"),
                    rules.DatabaseTableColumnNameReference,
                    EToken("policy"),
                    RequiredEToken("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD(), CD("EncodingPolicy", CompletionHint.Literal)}));

            var AlterColumnType = Command("AlterColumnType", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("column"),
                    Required(rules.DatabaseTableColumnNameReference, rules.MissingNameReference),
                    RequiredEToken("type"),
                    RequiredEToken("="),
                    Required(rules.Type, rules.MissingType),
                    new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD(), CD("ColumnType")}));

            var AlterDatabaseIngestionMapping = Command("AlterDatabaseIngestionMapping", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("ingestion"),
                    RequiredEToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredEToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD("MappingKind"), CD(), CD("MappingName", CompletionHint.Literal), CD("MappingFormat", CompletionHint.Literal)}));

            var AlterDatabasePersistMetadata = Command("AlterDatabasePersistMetadata", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("persist"),
                    RequiredEToken("metadata"),
                    Required(
                        First(
                            Custom(
                                rules.StringLiteral,
                                EToken(";"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                new [] {CD("BlobContainerUrl", CompletionHint.Literal), CD(), CD("StorageAccountKey", CompletionHint.Literal)}),
                            Custom(
                                rules.StringLiteral,
                                CD("Path", CompletionHint.Literal))),
                        () => (SyntaxElement)new CustomNode(new [] {CD("BlobContainerUrl", CompletionHint.Literal), CD(), CD("StorageAccountKey", CompletionHint.Literal)}, rules.MissingStringLiteral(), CreateMissingEToken(";"), rules.MissingStringLiteral())),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD(CompletionHint.Literal)}));

            var AlterDatabasePolicyCaching = Command("AlterDatabasePolicyCaching", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("caching"),
                    Required(
                        First(
                            Custom(
                                EToken("hotdata"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredEToken("hotindex"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                new [] {CD(), CD(), CD("HotData", CompletionHint.Literal), CD(), CD(), CD("HotIndex", CompletionHint.Literal)}),
                            Custom(
                                EToken("hot"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                new [] {CD(), CD(), CD("Timespan", CompletionHint.Literal)})),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("HotData", CompletionHint.Literal), CD(), CD(), CD("HotIndex", CompletionHint.Literal)}, CreateMissingEToken("hotdata"), CreateMissingEToken("="), rules.MissingValue(), CreateMissingEToken("hotindex"), CreateMissingEToken("="), rules.MissingValue())),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD()}));

            var AlterDatabasePolicyDiagnostics = Command("AlterDatabasePolicyDiagnostics", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("diagnostics"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("PolicyName", CompletionHint.Literal)}));

            var AlterDatabasePolicyEncoding = Command("AlterDatabasePolicyEncoding", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("EncodingPolicy", CompletionHint.Literal)}));

            var AlterDatabasePolicyExtentTagsRetention = Command("AlterDatabasePolicyExtentTagsRetention", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("extent_tags_retention"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("ExtentTagsRetentionPolicy", CompletionHint.Literal)}));

            var AlterDatabasePolicyIngestionBatching = Command("AlterDatabasePolicyIngestionBatching", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("ingestionbatching"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("IngestionBatchingPolicy", CompletionHint.Literal)}));

            var AlterDatabasePolicyManagedIdentity = Command("AlterDatabasePolicyManagedIdentity", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("managed_identity"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("ManagedIdentityPolicy", CompletionHint.Literal)}));

            var AlterDatabasePolicyMerge = Command("AlterDatabasePolicyMerge", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("merge"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("MergePolicy", CompletionHint.Literal)}));

            var AlterDatabasePolicyRetention = Command("AlterDatabasePolicyRetention", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("retention"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("RetentionPolicy", CompletionHint.Literal)}));

            var AlterDatabasePolicySharding = Command("AlterDatabasePolicySharding", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("sharding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("ShardingPolicy", CompletionHint.Literal)}));

            var AlterDatabasePolicyShardsGrouping = Command("AlterDatabasePolicyShardsGrouping", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("shards_grouping").Hide(),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("ShardsGroupingPolicy", CompletionHint.Literal)}));

            var AlterDatabasePolicyStreamingIngestion = Command("AlterDatabasePolicyStreamingIngestion", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    RequiredEToken("streamingingestion"),
                    Required(
                        First(
                            Custom(
                                EToken("disable", "enable"),
                                CD("Status")),
                            Custom(
                                rules.StringLiteral,
                                CD("StreamingIngestionPolicy", CompletionHint.Literal))),
                        () => CreateMissingEToken("Expected disable,enable")),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD()}));

            var AlterDatabasePrettyName = Command("AlterDatabasePrettyName", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("prettyname"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD("DatabasePrettyName", CompletionHint.Literal)}));

            var AlterDatabaseStorageKeys = Command("AlterDatabaseStorageKeys", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            EToken("async"),
                            EToken("database")),
                        Custom(
                            EToken("async"),
                            RequiredEToken("database")),
                        EToken("database")),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredEToken("storage"),
                    RequiredEToken("keys"),
                    Required(
                        First(
                            EToken("decryption-certificate-thumbprint"),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("decryption-certificate-thumbprint"),
                                new [] {CD(), CD(), CD(CompletionHint.None), CD(), CD()})),
                        () => CreateMissingEToken("decryption-certificate-thumbprint")),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(), CD(), CD("thumbprint", CompletionHint.Literal)}));

            var AlterExtentContainersAdd = Command("AlterExtentContainersAdd", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("extentcontainers"),
                    rules.DatabaseNameReference,
                    EToken("add"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        Custom(
                            rules.Value,
                            Required(rules.AnyGuidLiteralOrString, rules.MissingValue),
                            new [] {CD("hardDeletePeriod", CompletionHint.Literal), CD("containerId", CompletionHint.Literal)})),
                    new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD("container", CompletionHint.Literal), CD(CompletionHint.Literal, isOptional: true)}));

            var AlterExtentContainersDrop = Command("AlterExtentContainersDrop", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("extentcontainers"),
                    rules.DatabaseNameReference,
                    EToken("drop"),
                    Optional(
                        Custom(
                            rules.AnyGuidLiteralOrString,
                            CD("container", CompletionHint.Literal))),
                    new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(CompletionHint.Literal, isOptional: true)}));

            var AlterExtentContainersRecycle = Command("AlterExtentContainersRecycle", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("extentcontainers"),
                    rules.DatabaseNameReference,
                    EToken("recycle"),
                    Required(
                        First(
                            Custom(
                                EToken("older"),
                                Required(
                                    First(
                                        rules.Value,
                                        rules.Value),
                                    rules.MissingValue),
                                RequiredEToken("hours"),
                                new [] {CD(), CD("hours", CompletionHint.Literal), CD()}),
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                CD("container", CompletionHint.Literal))),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD("hours", CompletionHint.Literal), CD()}, CreateMissingEToken("older"), rules.MissingValue(), CreateMissingEToken("hours"))),
                    new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD()}));

            var AlterExtentContainersSet = Command("AlterExtentContainersSet", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("extentcontainers"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredEToken("set"),
                    RequiredEToken("state"),
                    Required(rules.AnyGuidLiteralOrString, rules.MissingValue),
                    RequiredEToken("to"),
                    RequiredEToken("readonly", "readwrite"),
                    new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(), CD("container", CompletionHint.Literal), CD(), CD()}));

            var AlterExtentTagsFromQuery = Command("AlterExtentTagsFromQuery", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            EToken("async"),
                            EToken("extent")),
                        EToken("extent")),
                    RequiredEToken("tags"),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                CD("t", CompletionHint.Literal)),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredEToken(")"),
                    Required(
                        Custom(
                            EToken("<|"),
                            Required(rules.CommandInput, rules.MissingExpression),
                            new [] {CD(), CD(CompletionHint.Tabular)}),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Tabular)}, CreateMissingEToken("<|"), rules.MissingExpression())),
                    new [] {CD(), CD(), CD(), CD(), CD(CompletionHint.Literal), CD(), CD("csl")}));

            var AlterSqlExternalTable = Command("AlterSqlExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        EToken("alter", CompletionKind.CommandPrefix),
                        EToken("external"),
                        EToken("table"),
                        rules.NameDeclarationOrStringLiteral,
                        EToken("("),
                        OList(
                            primaryElementParser: Custom(
                                rules.NameDeclarationOrStringLiteral,
                                RequiredEToken(":"),
                                Required(rules.Type, rules.MissingType),
                                new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        EToken(")"),
                        EToken("kind"),
                        RequiredEToken("="),
                        RequiredEToken("sql"),
                        RequiredEToken("table"),
                        RequiredEToken("="),
                        Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                        RequiredEToken("("),
                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                        RequiredEToken(")"),
                        Optional(
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                new [] {CD(), CD(), CD(CompletionHint.None), CD()}))}
                    ,
                    new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD(), CD(), CD(), CD("TableKind"), CD(), CD(), CD(CompletionHint.None), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var AlterStorageExternalTable = Command("AlterStorageExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        EToken("alter", CompletionKind.CommandPrefix),
                        EToken("external"),
                        EToken("table"),
                        rules.NameDeclarationOrStringLiteral,
                        RequiredEToken("("),
                        Required(
                            OList(
                                primaryElementParser: Custom(
                                    rules.NameDeclarationOrStringLiteral,
                                    RequiredEToken(":"),
                                    Required(rules.Type, rules.MissingType),
                                    new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()),
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType())))),
                        RequiredEToken(")"),
                        RequiredEToken("kind"),
                        RequiredEToken("="),
                        Required(
                            First(
                                EToken("adl").Hide(),
                                EToken("blob").Hide(),
                                EToken("storage")),
                            () => CreateMissingEToken("adl")),
                        Required(
                            First(
                                EToken("dataformat"),
                                Custom(
                                    EToken("partition"),
                                    RequiredEToken("by"),
                                    RequiredEToken("("),
                                    Required(
                                        OList(
                                            primaryElementParser: Custom(
                                                rules.NameDeclarationOrStringLiteral,
                                                RequiredEToken(":"),
                                                Required(
                                                    First(
                                                        Custom(
                                                            EToken("datetime"),
                                                            Optional(
                                                                Custom(
                                                                    EToken("="),
                                                                    Required(
                                                                        First(
                                                                            Custom(
                                                                                EToken("bin"),
                                                                                RequiredEToken("("),
                                                                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                                RequiredEToken(","),
                                                                                Required(rules.Value, rules.MissingValue),
                                                                                RequiredEToken(")"),
                                                                                new [] {CD("PartitionFunction"), CD(), CD("DateTimeColumn", CompletionHint.None), CD(), CD("BinValue", CompletionHint.Literal), CD()}),
                                                                            Custom(
                                                                                EToken("startofday", "startofmonth", "startofweek", "startofyear"),
                                                                                RequiredEToken("("),
                                                                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                                RequiredEToken(")"),
                                                                                new [] {CD("PartitionFunction"), CD(), CD("DateTimeColumn", CompletionHint.None), CD()})),
                                                                        () => (SyntaxElement)new CustomNode(new [] {CD("PartitionFunction"), CD(), CD("DateTimeColumn", CompletionHint.None), CD(), CD("BinValue", CompletionHint.Literal), CD()}, CreateMissingEToken("bin"), CreateMissingEToken("("), rules.MissingNameDeclaration(), CreateMissingEToken(","), rules.MissingValue(), CreateMissingEToken(")"))))),
                                                            new [] {CD("PartitionType"), CD(isOptional: true)}),
                                                        Custom(
                                                            EToken("long"),
                                                            RequiredEToken("="),
                                                            RequiredEToken("hash"),
                                                            RequiredEToken("("),
                                                            Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                            RequiredEToken(","),
                                                            Required(rules.Value, rules.MissingValue),
                                                            RequiredEToken(")"),
                                                            new [] {CD("PartitionType"), CD(), CD("PartitionFunction"), CD(), CD("StringColumn", CompletionHint.None), CD(), CD("HashMod", CompletionHint.Literal), CD()}),
                                                        Custom(
                                                            EToken("string"),
                                                            Optional(
                                                                Custom(
                                                                    EToken("="),
                                                                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                    new [] {CD(), CD("StringColumn", CompletionHint.None)})),
                                                            new [] {CD("PartitionType"), CD(isOptional: true)})),
                                                    () => (SyntaxElement)new CustomNode(new [] {CD("PartitionType"), CD(isOptional: true)}, CreateMissingEToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingEToken("="), (SyntaxElement)new CustomNode(new [] {CD("PartitionFunction"), CD(), CD("DateTimeColumn", CompletionHint.None), CD(), CD("BinValue", CompletionHint.Literal), CD()}, CreateMissingEToken("bin"), CreateMissingEToken("("), rules.MissingNameDeclaration(), CreateMissingEToken(","), rules.MissingValue(), CreateMissingEToken(")"))))),
                                                new [] {CD("PartitionName", CompletionHint.None), CD(), CD()}),
                                            separatorParser: EToken(","),
                                            secondaryElementParser: null,
                                            missingPrimaryElement: null,
                                            missingSeparator: null,
                                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PartitionName", CompletionHint.None), CD(), CD()}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), (SyntaxElement)new CustomNode(new [] {CD("PartitionType"), CD(isOptional: true)}, CreateMissingEToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingEToken("="), (SyntaxElement)new CustomNode(new [] {CD("PartitionFunction"), CD(), CD("DateTimeColumn", CompletionHint.None), CD(), CD("BinValue", CompletionHint.Literal), CD()}, CreateMissingEToken("bin"), CreateMissingEToken("("), rules.MissingNameDeclaration(), CreateMissingEToken(","), rules.MissingValue(), CreateMissingEToken(")"))))),
                                            endOfList: null,
                                            oneOrMore: true,
                                            allowTrailingSeparator: false,
                                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PartitionName", CompletionHint.None), CD(), CD()}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), (SyntaxElement)new CustomNode(new [] {CD("PartitionType"), CD(isOptional: true)}, CreateMissingEToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingEToken("="), (SyntaxElement)new CustomNode(new [] {CD("PartitionFunction"), CD(), CD("DateTimeColumn", CompletionHint.None), CD(), CD("BinValue", CompletionHint.Literal), CD()}, CreateMissingEToken("bin"), CreateMissingEToken("("), rules.MissingNameDeclaration(), CreateMissingEToken(","), rules.MissingValue(), CreateMissingEToken(")")))))))),
                                    RequiredEToken(")"),
                                    Optional(
                                        Custom(
                                            EToken("pathformat"),
                                            RequiredEToken("="),
                                            RequiredEToken("("),
                                            Required(
                                                First(
                                                    Custom(
                                                        rules.StringLiteral,
                                                        Required(
                                                            List(
                                                                Custom(
                                                                    First(
                                                                        Custom(
                                                                            EToken("datetime_pattern"),
                                                                            RequiredEToken("("),
                                                                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                                                                            RequiredEToken(","),
                                                                            Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                            RequiredEToken(")"),
                                                                            new [] {CD(), CD(), CD("DateTimeFormat", CompletionHint.Literal), CD(), CD("PartitionName", CompletionHint.None), CD()}),
                                                                        Custom(
                                                                            If(Not(EToken("datetime_pattern")), rules.NameDeclarationOrStringLiteral),
                                                                            CD("PartitionName", CompletionHint.None))),
                                                                    Optional(
                                                                        Custom(
                                                                            rules.StringLiteral,
                                                                            CD("PathSeparator", CompletionHint.Literal))),
                                                                    new [] {CD(), CD(CompletionHint.Literal, isOptional: true)}),
                                                                missingElement: null,
                                                                oneOrMore: true,
                                                                producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray())),
                                                            () => new SyntaxList<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Literal, isOptional: true)}, (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("DateTimeFormat", CompletionHint.Literal), CD(), CD("PartitionName", CompletionHint.None), CD()}, CreateMissingEToken("datetime_pattern"), CreateMissingEToken("("), rules.MissingStringLiteral(), CreateMissingEToken(","), rules.MissingNameDeclaration(), CreateMissingEToken(")")), rules.MissingStringLiteral()))),
                                                        new [] {CD("PathSeparator", CompletionHint.Literal), CD()}),
                                                    List(
                                                        Custom(
                                                            First(
                                                                Custom(
                                                                    EToken("datetime_pattern"),
                                                                    RequiredEToken("("),
                                                                    rules.StringLiteral,
                                                                    EToken(","),
                                                                    rules.NameDeclarationOrStringLiteral,
                                                                    EToken(")"),
                                                                    new [] {CD(), CD(), CD("DateTimeFormat", CompletionHint.Literal), CD(), CD("PartitionName", CompletionHint.None), CD()}),
                                                                Custom(
                                                                    EToken("datetime_pattern"),
                                                                    RequiredEToken("("),
                                                                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                                                                    RequiredEToken(","),
                                                                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                    RequiredEToken(")"),
                                                                    new [] {CD(), CD(), CD("DateTimeFormat", CompletionHint.Literal), CD(), CD("PartitionName", CompletionHint.None), CD()}),
                                                                Custom(
                                                                    If(Not(EToken("datetime_pattern")), rules.NameDeclarationOrStringLiteral),
                                                                    CD("PartitionName", CompletionHint.None))),
                                                            Optional(
                                                                Custom(
                                                                    rules.StringLiteral,
                                                                    CD("PathSeparator", CompletionHint.Literal))),
                                                            new [] {CD(), CD(CompletionHint.Literal, isOptional: true)}),
                                                        missingElement: null,
                                                        oneOrMore: true,
                                                        producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray()))),
                                                () => (SyntaxElement)new CustomNode(new [] {CD("PathSeparator", CompletionHint.Literal), CD()}, rules.MissingStringLiteral(), new SyntaxList<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Literal, isOptional: true)}, (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("DateTimeFormat", CompletionHint.Literal), CD(), CD("PartitionName", CompletionHint.None), CD()}, CreateMissingEToken("datetime_pattern"), CreateMissingEToken("("), rules.MissingStringLiteral(), CreateMissingEToken(","), rules.MissingNameDeclaration(), CreateMissingEToken(")")), rules.MissingStringLiteral())))),
                                            RequiredEToken(")"),
                                            new [] {CD(), CD(), CD(), CD(CompletionHint.Literal), CD()})),
                                    RequiredEToken("dataformat"),
                                    new [] {CD(), CD(), CD(), CD(CompletionHint.None), CD(), CD(isOptional: true), CD()})),
                            () => CreateMissingEToken("dataformat")),
                        RequiredEToken("="),
                        RequiredEToken("apacheavro", "avro", "csv", "json", "multijson", "orc", "parquet", "psv", "raw", "scsv", "sohsv", "sstream", "tsve", "tsv", "txt", "w3clogfile"),
                        RequiredEToken("("),
                        Required(
                            OList(
                                primaryElementParser: Custom(
                                    rules.StringLiteral,
                                    CD("StorageConnectionString", CompletionHint.Literal)),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingStringLiteral,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                        RequiredEToken(")"),
                        Optional(
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                new [] {CD(), CD(), CD(CompletionHint.None), CD()}))}
                    ,
                    new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD(), CD(), CD(), CD("TableKind"), CD(), CD(), CD("DataFormatKind"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var AlterExternalTableDocString = Command("AlterExternalTableDocString", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("external"),
                    EToken("table"),
                    rules.ExternalTableNameReference,
                    EToken("docstring"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD("tableName", CompletionHint.ExternalTable), CD(), CD("docStringValue", CompletionHint.Literal)}));

            var AlterExternalTableFolder = Command("AlterExternalTableFolder", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("external"),
                    EToken("table"),
                    rules.ExternalTableNameReference,
                    EToken("folder"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD("tableName", CompletionHint.ExternalTable), CD(), CD("folderValue", CompletionHint.Literal)}));

            var AlterExternalTableMapping = Command("AlterExternalTableMapping", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("external"),
                    RequiredEToken("table"),
                    Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                    RequiredEToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD("MappingName", CompletionHint.Literal), CD("MappingFormat", CompletionHint.Literal)}));

            var AlterFollowerClusterConfiguration = Command("AlterFollowerClusterConfiguration", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("follower"),
                    EToken("cluster"),
                    RequiredEToken("configuration"),
                    RequiredEToken("from"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(
                        First(
                            Custom(
                                EToken("database-name-prefix"),
                                RequiredEToken("="),
                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                new [] {CD(), CD(), CD("databaseNamePrefix", CompletionHint.None)}),
                            Custom(
                                EToken("default-caching-policies-modification-kind"),
                                RequiredEToken("="),
                                RequiredEToken("none", "replace", "union"),
                                new [] {CD(), CD(), CD("modificationKind")}),
                            Custom(
                                EToken("default-principals-modification-kind"),
                                RequiredEToken("="),
                                RequiredEToken("none", "replace", "union"),
                                new [] {CD(), CD(), CD("modificationKind")}),
                            Custom(
                                EToken("follow-authorized-principals"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                new [] {CD(), CD(), CD("followAuthorizedPrincipals", CompletionHint.Literal)})),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("databaseNamePrefix", CompletionHint.None)}, CreateMissingEToken("database-name-prefix"), CreateMissingEToken("="), rules.MissingNameDeclaration())),
                    new [] {CD(), CD(), CD(), CD(), CD(), CD("leaderClusterMetadataPath", CompletionHint.Literal), CD()}));

            var AlterFollowerDatabaseAuthorizedPrincipals = Command("AlterFollowerDatabaseAuthorizedPrincipals", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("follower"),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    First(
                        Custom(
                            EToken("from"),
                            rules.StringLiteral,
                            EToken("policy"),
                            new [] {CD(), CD("leaderClusterMetadataPath", CompletionHint.Literal), CD()}),
                        EToken("policy")),
                    RequiredEToken("caching"),
                    Required(
                        First(
                            Custom(
                                EToken("hotdata"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredEToken("hotindex"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                new [] {CD(), CD(), CD("hotDataToken", CompletionHint.Literal), CD(), CD(), CD("hotIndexToken", CompletionHint.Literal)}),
                            Custom(
                                EToken("hot"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                new [] {CD(), CD(), CD("hotToken", CompletionHint.Literal)})),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("hotDataToken", CompletionHint.Literal), CD(), CD(), CD("hotIndexToken", CompletionHint.Literal)}, CreateMissingEToken("hotdata"), CreateMissingEToken("="), rules.MissingValue(), CreateMissingEToken("hotindex"), CreateMissingEToken("="), rules.MissingValue())),
                    Optional(
                        First(
                            Custom(
                                EToken(","),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            EToken("hot_window"),
                                            RequiredEToken("="),
                                            Required(
                                                Custom(
                                                    rules.Value,
                                                    RequiredEToken(".."),
                                                    Required(rules.Value, rules.MissingValue),
                                                    new [] {CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal)}),
                                                () => (SyntaxElement)new CustomNode(new [] {CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal)}, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())),
                                            new [] {CD(), CD(), CD("p", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("p", CompletionHint.Literal)}, CreateMissingEToken("hot_window"), CreateMissingEToken("="), (SyntaxElement)new CustomNode(new [] {CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal)}, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("p", CompletionHint.Literal)}, CreateMissingEToken("hot_window"), CreateMissingEToken("="), (SyntaxElement)new CustomNode(new [] {CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal)}, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())))))),
                            OList(
                                primaryElementParser: Custom(
                                    EToken("hot_window"),
                                    RequiredEToken("="),
                                    Required(
                                        Custom(
                                            rules.Value,
                                            RequiredEToken(".."),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal)}),
                                        () => (SyntaxElement)new CustomNode(new [] {CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal)}, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())),
                                    new [] {CD(), CD(), CD("p", CompletionHint.Literal)}),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("p", CompletionHint.Literal)}, CreateMissingEToken("hot_window"), CreateMissingEToken("="), (SyntaxElement)new CustomNode(new [] {CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal)}, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())),
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)))),
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(), CD(), CD(), CD("hotWindows", isOptional: true)}));

            var AlterFollowerDatabaseConfiguration = Command("AlterFollowerDatabaseConfiguration", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("follower"),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    First(
                        Custom(
                            EToken("caching-policies-modification-kind"),
                            EToken("="),
                            EToken("none", "replace", "union"),
                            new [] {CD(), CD(), CD("modificationKind")}),
                        Custom(
                            EToken("caching-policies-modification-kind"),
                            RequiredEToken("="),
                            RequiredEToken("none", "replace", "union"),
                            new [] {CD(), CD(), CD("modificationKind")}),
                        Custom(
                            EToken("database-name-override"),
                            RequiredEToken("="),
                            Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                            new [] {CD(), CD(), CD("databaseNameOverride", CompletionHint.None)}),
                        Custom(
                            EToken("from"),
                            rules.StringLiteral,
                            First(
                                Custom(
                                    EToken("caching-policies-modification-kind"),
                                    EToken("="),
                                    EToken("none", "replace", "union"),
                                    new [] {CD(), CD(), CD("modificationKind")}),
                                Custom(
                                    EToken("caching-policies-modification-kind"),
                                    RequiredEToken("="),
                                    RequiredEToken("none", "replace", "union"),
                                    new [] {CD(), CD(), CD("modificationKind")}),
                                Custom(
                                    EToken("database-name-override"),
                                    RequiredEToken("="),
                                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                    new [] {CD(), CD(), CD("databaseNameOverride", CompletionHint.None)}),
                                Custom(
                                    EToken("metadata"),
                                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                                    new [] {CD(), CD("serializedDatabaseMetadataOverride", CompletionHint.Literal)}),
                                Custom(
                                    EToken("prefetch-extents"),
                                    RequiredEToken("="),
                                    Required(rules.Value, rules.MissingValue),
                                    new [] {CD(), CD(), CD("prefetchExtents", CompletionHint.Literal)}),
                                Custom(
                                    EToken("principals-modification-kind"),
                                    RequiredEToken("="),
                                    RequiredEToken("none", "replace", "union"),
                                    new [] {CD(), CD(), CD("modificationKind")})),
                            new [] {CD(), CD("leaderClusterMetadataPath", CompletionHint.Literal), CD()}),
                        Custom(
                            EToken("metadata"),
                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                            new [] {CD(), CD("serializedDatabaseMetadataOverride", CompletionHint.Literal)}),
                        Custom(
                            EToken("prefetch-extents"),
                            RequiredEToken("="),
                            Required(rules.Value, rules.MissingValue),
                            new [] {CD(), CD(), CD("prefetchExtents", CompletionHint.Literal)}),
                        Custom(
                            EToken("principals-modification-kind"),
                            RequiredEToken("="),
                            RequiredEToken("none", "replace", "union"),
                            new [] {CD(), CD(), CD("modificationKind")})),
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD()}));

            var AlterFollowerDatabaseChildEntities = Command("AlterFollowerDatabaseChildEntities", 
                Custom(
                    new Parser<LexicalToken>[] {
                        EToken("alter", CompletionKind.CommandPrefix),
                        EToken("follower"),
                        EToken("database"),
                        rules.DatabaseNameReference,
                        First(
                            Custom(
                                EToken("external"),
                                EToken("tables")),
                            Custom(
                                EToken("external"),
                                RequiredEToken("tables")),
                            Custom(
                                EToken("from"),
                                rules.StringLiteral,
                                First(
                                    Custom(
                                        EToken("external"),
                                        EToken("tables")),
                                    Custom(
                                        EToken("external"),
                                        RequiredEToken("tables")),
                                    EToken("materialized-views"),
                                    EToken("tables")),
                                new [] {CD(), CD("leaderClusterMetadataPath", CompletionHint.Literal), CD()}),
                            EToken("materialized-views"),
                            EToken("tables")),
                        RequiredEToken("exclude", "include"),
                        RequiredEToken("add", "drop"),
                        RequiredEToken("("),
                        Required(
                            OList(
                                primaryElementParser: Custom(
                                    rules.WildcardedNameDeclaration,
                                    CD("ename", CompletionHint.None)),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingNameDeclaration,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration()))),
                        RequiredEToken(")")}
                    ,
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(), CD("entityListKind"), CD("operationName"), CD(), CD(CompletionHint.None), CD()}));

            var AlterFollowerTablesPolicyCaching = Command("AlterFollowerTablesPolicyCaching", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("follower"),
                    RequiredEToken("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    Required(
                        First(
                            Custom(
                                EToken("from"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                Required(
                                    First(
                                        Custom(
                                            EToken("materialized-views"),
                                            RequiredEToken("("),
                                            Required(
                                                OList(
                                                    primaryElementParser: Custom(
                                                        rules.NameDeclarationOrStringLiteral,
                                                        CD("name", CompletionHint.None)),
                                                    separatorParser: EToken(","),
                                                    secondaryElementParser: null,
                                                    missingPrimaryElement: null,
                                                    missingSeparator: null,
                                                    missingSecondaryElement: rules.MissingNameDeclaration,
                                                    endOfList: null,
                                                    oneOrMore: true,
                                                    allowTrailingSeparator: false,
                                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration()))),
                                            RequiredEToken(")"),
                                            new [] {CD(), CD(), CD(CompletionHint.None), CD()}),
                                        Custom(
                                            EToken("materialized-view"),
                                            Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                                            new [] {CD(), CD("name", CompletionHint.MaterializedView)}),
                                        Custom(
                                            EToken("tables"),
                                            RequiredEToken("("),
                                            Required(
                                                OList(
                                                    primaryElementParser: Custom(
                                                        rules.NameDeclarationOrStringLiteral,
                                                        CD("name", CompletionHint.None)),
                                                    separatorParser: EToken(","),
                                                    secondaryElementParser: null,
                                                    missingPrimaryElement: null,
                                                    missingSeparator: null,
                                                    missingSecondaryElement: rules.MissingNameDeclaration,
                                                    endOfList: null,
                                                    oneOrMore: true,
                                                    allowTrailingSeparator: false,
                                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration()))),
                                            RequiredEToken(")"),
                                            new [] {CD(), CD(), CD(CompletionHint.None), CD()}),
                                        Custom(
                                            EToken("table"),
                                            Required(rules.TableNameReference, rules.MissingNameReference),
                                            new [] {CD(), CD("name", CompletionHint.Table)})),
                                    () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD(CompletionHint.None), CD()}, CreateMissingEToken("materialized-views"), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration())), CreateMissingEToken(")"))),
                                new [] {CD(), CD("leaderClusterMetadataPath", CompletionHint.Literal), CD()}),
                            Custom(
                                EToken("materialized-views"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            CD("name", CompletionHint.None)),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingNameDeclaration,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration()))),
                                RequiredEToken(")"),
                                new [] {CD(), CD(), CD(CompletionHint.None), CD()}),
                            Custom(
                                EToken("materialized-view"),
                                Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                                new [] {CD(), CD("name", CompletionHint.MaterializedView)}),
                            Custom(
                                EToken("tables"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            CD("name", CompletionHint.None)),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingNameDeclaration,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration()))),
                                RequiredEToken(")"),
                                new [] {CD(), CD(), CD(CompletionHint.None), CD()}),
                            Custom(
                                EToken("table"),
                                Required(rules.TableNameReference, rules.MissingNameReference),
                                new [] {CD(), CD("name", CompletionHint.Table)})),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD("leaderClusterMetadataPath", CompletionHint.Literal), CD()}, CreateMissingEToken("from"), rules.MissingStringLiteral(), (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD(CompletionHint.None), CD()}, CreateMissingEToken("materialized-views"), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration())), CreateMissingEToken(")")))),
                    RequiredEToken("policy"),
                    RequiredEToken("caching"),
                    Required(
                        First(
                            Custom(
                                EToken("hotdata"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredEToken("hotindex"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                new [] {CD(), CD(), CD("hotDataToken", CompletionHint.Literal), CD(), CD(), CD("hotIndexToken", CompletionHint.Literal)}),
                            Custom(
                                EToken("hot"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                new [] {CD(), CD(), CD("hotToken", CompletionHint.Literal)})),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("hotDataToken", CompletionHint.Literal), CD(), CD(), CD("hotIndexToken", CompletionHint.Literal)}, CreateMissingEToken("hotdata"), CreateMissingEToken("="), rules.MissingValue(), CreateMissingEToken("hotindex"), CreateMissingEToken("="), rules.MissingValue())),
                    Optional(
                        First(
                            Custom(
                                EToken(","),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            EToken("hot_window"),
                                            RequiredEToken("="),
                                            Required(
                                                Custom(
                                                    rules.Value,
                                                    RequiredEToken(".."),
                                                    Required(rules.Value, rules.MissingValue),
                                                    new [] {CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal)}),
                                                () => (SyntaxElement)new CustomNode(new [] {CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal)}, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())),
                                            new [] {CD(), CD(), CD("p", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("p", CompletionHint.Literal)}, CreateMissingEToken("hot_window"), CreateMissingEToken("="), (SyntaxElement)new CustomNode(new [] {CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal)}, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("p", CompletionHint.Literal)}, CreateMissingEToken("hot_window"), CreateMissingEToken("="), (SyntaxElement)new CustomNode(new [] {CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal)}, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())))))),
                            OList(
                                primaryElementParser: Custom(
                                    EToken("hot_window"),
                                    RequiredEToken("="),
                                    Required(
                                        Custom(
                                            rules.Value,
                                            RequiredEToken(".."),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal)}),
                                        () => (SyntaxElement)new CustomNode(new [] {CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal)}, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())),
                                    new [] {CD(), CD(), CD("p", CompletionHint.Literal)}),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("p", CompletionHint.Literal)}, CreateMissingEToken("hot_window"), CreateMissingEToken("="), (SyntaxElement)new CustomNode(new [] {CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal)}, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())),
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)))),
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(), CD(), CD(), CD(), CD("hotWindows", isOptional: true)}));

            var AlterFunctionDocString = Command("AlterFunctionDocString", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("function"),
                    If(Not(EToken("with")), rules.DatabaseFunctionNameReference),
                    EToken("docstring"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(CompletionHint.Function), CD(), CD("Documentation", CompletionHint.Literal)}));

            var AlterFunctionFolder = Command("AlterFunctionFolder", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("function"),
                    If(Not(EToken("with")), rules.DatabaseFunctionNameReference),
                    EToken("folder"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD(), CD("Folder", CompletionHint.Literal)}));

            var AlterFunction = Command("AlterFunction", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("function"),
                    Required(
                        First(
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                Required(rules.DatabaseFunctionNameReference, rules.MissingNameReference),
                                new [] {CD(), CD(), CD(CompletionHint.None), CD(), CD("FunctionName", CompletionHint.Function)}),
                            Custom(
                                If(Not(EToken("with")), rules.DatabaseFunctionNameReference),
                                CD("FunctionName", CompletionHint.Function))),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD(CompletionHint.None), CD(), CD("FunctionName", CompletionHint.Function)}, CreateMissingEToken("with"), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()))), CreateMissingEToken(")"), rules.MissingNameReference())),
                    Required(rules.FunctionDeclaration, rules.MissingFunctionDeclaration)));

            var AlterMaterializedViewAutoUpdateSchema = Command("AlterMaterializedViewAutoUpdateSchema", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    If(Not(EToken("with")), rules.MaterializedViewNameReference),
                    EToken("autoUpdateSchema"),
                    RequiredEToken("false", "true"),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD()}));

            var AlterMaterializedViewDocString = Command("AlterMaterializedViewDocString", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    If(Not(EToken("with")), rules.MaterializedViewNameReference),
                    EToken("docstring"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD("Documentation", CompletionHint.Literal)}));

            var AlterMaterializedViewFolder = Command("AlterMaterializedViewFolder", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    If(Not(EToken("with")), rules.MaterializedViewNameReference),
                    EToken("folder"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD("Folder", CompletionHint.Literal)}));

            var AlterMaterializedViewLookback = Command("AlterMaterializedViewLookback", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    If(Not(EToken("with")), rules.MaterializedViewNameReference),
                    EToken("lookback"),
                    Required(rules.Value, rules.MissingValue),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD("Lookback", CompletionHint.Literal)}));

            var AlterMaterializedView = Command("AlterMaterializedView", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    First(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            OList(
                                primaryElementParser: Custom(
                                    First(
                                        EToken("dimensionTables"),
                                        EToken("lookback"),
                                        If(Not(And(EToken("dimensionTables", "lookback"))), rules.NameDeclarationOrStringLiteral)),
                                    RequiredEToken("="),
                                    rules.Value,
                                    new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("dimensionTables"), CreateMissingEToken("="), rules.MissingValue()),
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            EToken(")"),
                            rules.MaterializedViewNameReference,
                            new [] {CD(), CD(), CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView)}),
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        First(
                                            EToken("dimensionTables"),
                                            EToken("lookback"),
                                            If(Not(And(EToken("dimensionTables", "lookback"))), rules.NameDeclarationOrStringLiteral)),
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("dimensionTables"), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("dimensionTables"), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                            new [] {CD(), CD(), CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView)}),
                        Custom(
                            If(Not(EToken("with")), rules.MaterializedViewNameReference),
                            CD("MaterializedViewName", CompletionHint.MaterializedView))),
                    RequiredEToken("on"),
                    RequiredEToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Required(rules.FunctionBody, rules.MissingFunctionBody),
                    new [] {CD(), CD(), CD(), CD(), CD(), CD(CompletionHint.Table), CD()}));

            var AlterMaterializedViewPolicyCaching = Command("AlterMaterializedViewPolicyCaching", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    If(Not(EToken("with")), rules.MaterializedViewNameReference),
                    EToken("policy"),
                    EToken("caching"),
                    Required(
                        First(
                            Custom(
                                EToken("hotdata"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredEToken("hotindex"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                new [] {CD(), CD(), CD("HotData", CompletionHint.Literal), CD(), CD(), CD("HotIndex", CompletionHint.Literal)}),
                            Custom(
                                EToken("hot"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                new [] {CD(), CD(), CD("Timespan", CompletionHint.Literal)})),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("HotData", CompletionHint.Literal), CD(), CD(), CD("HotIndex", CompletionHint.Literal)}, CreateMissingEToken("hotdata"), CreateMissingEToken("="), rules.MissingValue(), CreateMissingEToken("hotindex"), CreateMissingEToken("="), rules.MissingValue())),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD()}));

            var AlterMaterializedViewPolicyPartitioning = Command("AlterMaterializedViewPolicyPartitioning", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    If(Not(EToken("with")), rules.MaterializedViewNameReference),
                    EToken("policy"),
                    EToken("partitioning"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD("Policy", CompletionHint.Literal)}));

            var AlterMaterializedViewPolicyRetention = Command("AlterMaterializedViewPolicyRetention", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    If(Not(EToken("with")), rules.MaterializedViewNameReference),
                    EToken("policy"),
                    EToken("retention"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD("RetentionPolicy", CompletionHint.Literal)}));

            var AlterMaterializedViewPolicyRowLevelSecurity = Command("AlterMaterializedViewPolicyRowLevelSecurity", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    Required(If(Not(EToken("with")), rules.MaterializedViewNameReference), rules.MissingNameReference),
                    RequiredEToken("policy"),
                    RequiredEToken("row_level_security"),
                    RequiredEToken("disable", "enable"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD(), CD("Query", CompletionHint.Literal)}));

            var AlterPoliciesOfRetention = Command("AlterPoliciesOfRetention", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("policies"),
                    RequiredEToken("of"),
                    RequiredEToken("retention"),
                    Required(
                        First(
                            Custom(
                                EToken("internal"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                new [] {CD(), CD("policies", CompletionHint.Literal)}),
                            Custom(
                                rules.StringLiteral,
                                CD("policies", CompletionHint.Literal))),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD("policies", CompletionHint.Literal)}, CreateMissingEToken("internal"), rules.MissingStringLiteral()))));

            var AlterTablesPolicyCaching = Command("AlterTablesPolicyCaching", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("tables"),
                    EToken("("),
                    OList(
                        primaryElementParser: Custom(
                            rules.TableNameReference,
                            CD("TableName", CompletionHint.Table)),
                        separatorParser: EToken(","),
                        secondaryElementParser: null,
                        missingPrimaryElement: null,
                        missingSeparator: null,
                        missingSecondaryElement: rules.MissingNameReference,
                        endOfList: null,
                        oneOrMore: true,
                        allowTrailingSeparator: false,
                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                    EToken(")"),
                    EToken("policy"),
                    EToken("caching"),
                    Required(
                        First(
                            Custom(
                                EToken("hotdata"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredEToken("hotindex"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                new [] {CD(), CD(), CD("HotData", CompletionHint.Literal), CD(), CD(), CD("HotIndex", CompletionHint.Literal)}),
                            Custom(
                                EToken("hot"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                new [] {CD(), CD(), CD("Timespan", CompletionHint.Literal)})),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("HotData", CompletionHint.Literal), CD(), CD(), CD("HotIndex", CompletionHint.Literal)}, CreateMissingEToken("hotdata"), CreateMissingEToken("="), rules.MissingValue(), CreateMissingEToken("hotindex"), CreateMissingEToken("="), rules.MissingValue())),
                    Optional(
                        First(
                            Custom(
                                EToken(","),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            EToken("hot_window"),
                                            RequiredEToken("="),
                                            Required(
                                                Custom(
                                                    rules.Value,
                                                    RequiredEToken(".."),
                                                    Required(rules.Value, rules.MissingValue),
                                                    new [] {CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal)}),
                                                () => (SyntaxElement)new CustomNode(new [] {CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal)}, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())),
                                            new [] {CD(), CD(), CD("p", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("p", CompletionHint.Literal)}, CreateMissingEToken("hot_window"), CreateMissingEToken("="), (SyntaxElement)new CustomNode(new [] {CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal)}, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("p", CompletionHint.Literal)}, CreateMissingEToken("hot_window"), CreateMissingEToken("="), (SyntaxElement)new CustomNode(new [] {CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal)}, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())))))),
                            OList(
                                primaryElementParser: Custom(
                                    EToken("hot_window"),
                                    RequiredEToken("="),
                                    Required(
                                        Custom(
                                            rules.Value,
                                            RequiredEToken(".."),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal)}),
                                        () => (SyntaxElement)new CustomNode(new [] {CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal)}, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())),
                                    new [] {CD(), CD(), CD("p", CompletionHint.Literal)}),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("p", CompletionHint.Literal)}, CreateMissingEToken("hot_window"), CreateMissingEToken("="), (SyntaxElement)new CustomNode(new [] {CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal)}, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())),
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)))),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(), CD(), CD(isOptional: true)}));

            var AlterTablesPolicyIngestionBatching = Command("AlterTablesPolicyIngestionBatching", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("tables"),
                    EToken("("),
                    OList(
                        primaryElementParser: Custom(
                            rules.TableNameReference,
                            CD("TableName", CompletionHint.Table)),
                        separatorParser: EToken(","),
                        secondaryElementParser: null,
                        missingPrimaryElement: null,
                        missingSeparator: null,
                        missingSecondaryElement: rules.MissingNameReference,
                        endOfList: null,
                        oneOrMore: true,
                        allowTrailingSeparator: false,
                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                    EToken(")"),
                    EToken("policy"),
                    EToken("ingestionbatching"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(), CD("IngestionBatchingPolicy", CompletionHint.Literal)}));

            var AlterTablesPolicyIngestionTime = Command("AlterTablesPolicyIngestionTime", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("tables"),
                    EToken("("),
                    OList(
                        primaryElementParser: Custom(
                            rules.TableNameReference,
                            CD("TableName", CompletionHint.Table)),
                        separatorParser: EToken(","),
                        secondaryElementParser: null,
                        missingPrimaryElement: null,
                        missingSeparator: null,
                        missingSecondaryElement: rules.MissingNameReference,
                        endOfList: null,
                        oneOrMore: true,
                        allowTrailingSeparator: false,
                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                    EToken(")"),
                    EToken("policy"),
                    EToken("ingestiontime"),
                    RequiredEToken("true"),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(), CD()}));

            var AlterTablesPolicyMerge = Command("AlterTablesPolicyMerge", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("tables"),
                    EToken("("),
                    OList(
                        primaryElementParser: Custom(
                            rules.TableNameReference,
                            CD("TableName", CompletionHint.Table)),
                        separatorParser: EToken(","),
                        secondaryElementParser: null,
                        missingPrimaryElement: null,
                        missingSeparator: null,
                        missingSecondaryElement: rules.MissingNameReference,
                        endOfList: null,
                        oneOrMore: true,
                        allowTrailingSeparator: false,
                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                    EToken(")"),
                    EToken("policy"),
                    EToken("merge"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(), CD("policy", CompletionHint.Literal)}));

            var AlterTablesPolicyRestrictedViewAccess = Command("AlterTablesPolicyRestrictedViewAccess", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("tables"),
                    EToken("("),
                    OList(
                        primaryElementParser: Custom(
                            rules.TableNameReference,
                            CD("TableName", CompletionHint.Table)),
                        separatorParser: EToken(","),
                        secondaryElementParser: null,
                        missingPrimaryElement: null,
                        missingSeparator: null,
                        missingSecondaryElement: rules.MissingNameReference,
                        endOfList: null,
                        oneOrMore: true,
                        allowTrailingSeparator: false,
                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                    EToken(")"),
                    EToken("policy"),
                    EToken("restricted_view_access"),
                    RequiredEToken("false", "true"),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(), CD()}));

            var AlterTablesPolicyRetention = Command("AlterTablesPolicyRetention", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("tables"),
                    EToken("("),
                    OList(
                        primaryElementParser: Custom(
                            rules.TableNameReference,
                            CD("TableName", CompletionHint.Table)),
                        separatorParser: EToken(","),
                        secondaryElementParser: null,
                        missingPrimaryElement: null,
                        missingSeparator: null,
                        missingSecondaryElement: rules.MissingNameReference,
                        endOfList: null,
                        oneOrMore: true,
                        allowTrailingSeparator: false,
                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                    EToken(")"),
                    EToken("policy"),
                    EToken("retention"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(), CD("RetentionPolicy", CompletionHint.Literal)}));

            var AlterTablesPolicyRowOrder = Command("AlterTablesPolicyRowOrder", 
                Custom(
                    new Parser<LexicalToken>[] {
                        EToken("alter", CompletionKind.CommandPrefix),
                        EToken("tables"),
                        RequiredEToken("("),
                        Required(
                            OList(
                                primaryElementParser: Custom(
                                    rules.TableNameReference,
                                    CD("TableName", CompletionHint.Table)),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingNameReference,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameReference()))),
                        RequiredEToken(")"),
                        RequiredEToken("policy"),
                        RequiredEToken("roworder"),
                        RequiredEToken("("),
                        Required(
                            OList(
                                primaryElementParser: Custom(
                                    rules.NameDeclarationOrStringLiteral,
                                    RequiredEToken("asc", "desc"),
                                    new [] {CD("ColumnName", CompletionHint.None), CD()}),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD()}, rules.MissingNameDeclaration(), CreateMissingEToken("Expected asc,desc")),
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD()}, rules.MissingNameDeclaration(), CreateMissingEToken("Expected asc,desc"))))),
                        RequiredEToken(")")}
                    ,
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(), CD(), CD(CompletionHint.None), CD()}));

            var AlterTableColumnStatisticsMethod = Command("AlterTableColumnStatisticsMethod", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("column"),
                    RequiredEToken("statistics"),
                    RequiredEToken("method"),
                    RequiredEToken("="),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD(), CD("newMethod", CompletionHint.Literal)}));

            var AlterTablePolicyAutoDelete = Command("AlterTablePolicyAutoDelete", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("auto_delete"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("AutoDeletePolicy", CompletionHint.Literal)}));

            var AlterTablePolicyCaching = Command("AlterTablePolicyCaching", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("caching"),
                    Required(
                        First(
                            Custom(
                                EToken("hotdata"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredEToken("hotindex"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                new [] {CD(), CD(), CD("HotData", CompletionHint.Literal), CD(), CD(), CD("HotIndex", CompletionHint.Literal)}),
                            Custom(
                                EToken("hot"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                new [] {CD(), CD(), CD("Timespan", CompletionHint.Literal)})),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("HotData", CompletionHint.Literal), CD(), CD(), CD("HotIndex", CompletionHint.Literal)}, CreateMissingEToken("hotdata"), CreateMissingEToken("="), rules.MissingValue(), CreateMissingEToken("hotindex"), CreateMissingEToken("="), rules.MissingValue())),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD()}));

            var AlterTablePolicyEncoding = Command("AlterTablePolicyEncoding", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("EncodingPolicy", CompletionHint.Literal)}));

            var AlterTablePolicyExtentTagsRetention = Command("AlterTablePolicyExtentTagsRetention", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("extent_tags_retention"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("ExtentTagsRetentionPolicy", CompletionHint.Literal)}));

            var AlterTablePolicyIngestionBatching = Command("AlterTablePolicyIngestionBatching", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("ingestionbatching"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("IngestionBatchingPolicy", CompletionHint.Literal)}));

            var AlterTablePolicyMerge = Command("AlterTablePolicyMerge", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("merge"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("MergePolicy", CompletionHint.Literal)}));

            var AlterTablePolicyRestrictedViewAccess = Command("AlterTablePolicyRestrictedViewAccess", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("restricted_view_access"),
                    RequiredEToken("false", "true"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD()}));

            var AlterTablePolicyRetention = Command("AlterTablePolicyRetention", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("retention"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("RetentionPolicy", CompletionHint.Literal)}));

            var AlterTablePolicyRowOrder = Command("AlterTablePolicyRowOrder", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("roworder"),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.ColumnNameReference,
                                RequiredEToken("asc", "desc"),
                                new [] {CD("ColumnName", CompletionHint.Column), CD()}),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.Column), CD()}, rules.MissingNameReference(), CreateMissingEToken("Expected asc,desc")),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.Column), CD()}, rules.MissingNameReference(), CreateMissingEToken("Expected asc,desc"))))),
                    RequiredEToken(")"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD(CompletionHint.Column), CD()}));

            var AlterTablePolicySharding = Command("AlterTablePolicySharding", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("sharding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("ShardingPolicy", CompletionHint.Literal)}));

            var AlterTablePolicyStreamingIngestion = Command("AlterTablePolicyStreamingIngestion", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("streamingingestion"),
                    Required(
                        First(
                            Custom(
                                EToken("disable", "enable"),
                                CD("Status")),
                            Custom(
                                rules.StringLiteral,
                                CD("StreamingIngestionPolicy", CompletionHint.Literal))),
                        () => CreateMissingEToken("Expected disable,enable")),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD()}));

            var AlterTablePolicyUpdate = Command("AlterTablePolicyUpdate", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    RequiredEToken("update"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("UpdatePolicy", CompletionHint.Literal)}));

            var AlterTableRowStoreReferencesDisableBlockedKeys = Command("AlterTableRowStoreReferencesDisableBlockedKeys", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("rowstore_references"),
                    EToken("disable"),
                    EToken("blocked"),
                    RequiredEToken("keys"),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD(), CD(isOptional: true)}));

            var AlterTableRowStoreReferencesDisableKey = Command("AlterTableRowStoreReferencesDisableKey", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("rowstore_references"),
                    EToken("disable"),
                    EToken("key"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD("rowStoreKey", CompletionHint.Literal), CD(isOptional: true)}));

            var AlterTableRowStoreReferencesDisableRowStore = Command("AlterTableRowStoreReferencesDisableRowStore", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("rowstore_references"),
                    EToken("disable"),
                    RequiredEToken("rowstore"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD("rowStoreName", CompletionHint.None), CD(isOptional: true)}));

            var AlterTableRowStoreReferencesDropBlockedKeys = Command("AlterTableRowStoreReferencesDropBlockedKeys", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("rowstore_references"),
                    EToken("drop"),
                    EToken("blocked"),
                    RequiredEToken("keys"),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD(), CD(isOptional: true)}));

            var AlterTableRowStoreReferencesDropKey = Command("AlterTableRowStoreReferencesDropKey", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("rowstore_references"),
                    EToken("drop"),
                    EToken("key"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD("rowStoreKey", CompletionHint.Literal), CD(isOptional: true)}));

            var AlterTableRowStoreReferencesDropRowStore = Command("AlterTableRowStoreReferencesDropRowStore", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    RequiredEToken("rowstore_references"),
                    RequiredEToken("drop"),
                    RequiredEToken("rowstore"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD("rowStoreName", CompletionHint.None), CD(isOptional: true)}));

            var AlterTable = Command("AlterTable", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.NameDeclarationOrStringLiteral,
                                RequiredEToken(":"),
                                Required(rules.Type, rules.MissingType),
                                new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType())))),
                    RequiredEToken(")"),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        First(
                                            EToken("docstring"),
                                            EToken("folder"),
                                            If(Not(And(EToken("docstring", "folder"))), rules.NameDeclarationOrStringLiteral)),
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("docstring"), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("docstring"), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"))),
                    new [] {CD(), CD(), CD(CompletionHint.Table), CD(), CD(CompletionHint.None), CD(), CD(isOptional: true)}));

            var AlterTableColumnDocStrings = Command("AlterTableColumnDocStrings", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("column-docstrings"),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.ColumnNameReference,
                                RequiredEToken(":"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                new [] {CD("ColumnName", CompletionHint.Column), CD(), CD("DocString", CompletionHint.Literal)}),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.Column), CD(), CD("DocString", CompletionHint.Literal)}, rules.MissingNameReference(), CreateMissingEToken(":"), rules.MissingStringLiteral()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.Column), CD(), CD("DocString", CompletionHint.Literal)}, rules.MissingNameReference(), CreateMissingEToken(":"), rules.MissingStringLiteral())))),
                    RequiredEToken(")"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(CompletionHint.Column), CD()}));

            var AlterTableColumnsPolicyEncoding = Command("AlterTableColumnsPolicyEncoding", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("columns"),
                    RequiredEToken("policy"),
                    RequiredEToken("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD("EncodingPolicies", CompletionHint.Literal)}));

            var AlterTableColumnStatistics = Command("AlterTableColumnStatistics", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("column"),
                    RequiredEToken("statistics"),
                    CommaList(
                        Custom(
                            rules.NameDeclarationOrStringLiteral,
                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                            new [] {CD("c2", CompletionHint.None), CD("statisticsValues2", CompletionHint.Literal)})),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(CompletionHint.None)}));

            var AlterTableDocString = Command("AlterTableDocString", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("docstring"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("Documentation", CompletionHint.Literal)}));

            var AlterTableFolder = Command("AlterTableFolder", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("folder"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("Folder", CompletionHint.Literal)}));

            var AlterTableIngestionMapping = Command("AlterTableIngestionMapping", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("ingestion"),
                    RequiredEToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredEToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("MappingKind"), CD(), CD("MappingName", CompletionHint.Literal), CD("MappingFormat", CompletionHint.Literal)}));

            var AlterTablePolicyIngestionTime = Command("AlterTablePolicyIngestionTime", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("policy"),
                    EToken("ingestiontime"),
                    RequiredEToken("true"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD()}));

            var AlterTablePolicyPartitioning = Command("AlterTablePolicyPartitioning", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("policy"),
                    EToken("partitioning"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("Policy", CompletionHint.Literal)}));

            var AlterTablePolicyRowLevelSecurity = Command("AlterTablePolicyRowLevelSecurity", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    RequiredEToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredEToken("policy"),
                    RequiredEToken("row_level_security"),
                    RequiredEToken("disable", "enable"),
                    Required(
                        First(
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                CommaList(
                                    Custom(
                                        If(Not(EToken(")")), rules.NameDeclarationOrStringLiteral),
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)})),
                                RequiredEToken(")"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                new [] {CD(), CD(), CD(CompletionHint.None), CD(), CD("Query", CompletionHint.Literal)}),
                            Custom(
                                rules.StringLiteral,
                                CD("Query", CompletionHint.Literal))),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD(CompletionHint.None), CD(), CD("Query", CompletionHint.Literal)}, CreateMissingEToken("with"), CreateMissingEToken("("), SyntaxList<SeparatedElement<SyntaxElement>>.Empty(), CreateMissingEToken(")"), rules.MissingStringLiteral())),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD()}));

            var AppendTable = Command("AppendTable", 
                Custom(
                    EToken("append", CompletionKind.CommandPrefix),
                    Required(
                        First(
                            Custom(
                                EToken("async"),
                                Required(rules.TableNameReference, rules.MissingNameReference),
                                new [] {CD(), CD("TableName", CompletionHint.Table)}),
                            Custom(
                                If(Not(EToken("async")), rules.TableNameReference),
                                CD("TableName", CompletionHint.Table))),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD("TableName", CompletionHint.Table)}, CreateMissingEToken("async"), rules.MissingNameReference())),
                    Required(
                        First(
                            EToken("<|"),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            First(
                                                EToken("creationTime"),
                                                EToken("distributed"),
                                                EToken("docstring"),
                                                EToken("extend_schema"),
                                                EToken("folder"),
                                                EToken("format"),
                                                EToken("ignoreFirstRecord"),
                                                EToken("ingestIfNotExists"),
                                                EToken("ingestionMappingReference"),
                                                EToken("ingestionMapping"),
                                                EToken("persistDetails"),
                                                EToken("policy_ingestionTime"),
                                                EToken("recreate_schema"),
                                                EToken("tags"),
                                                EToken("validationPolicy"),
                                                EToken("zipPattern"),
                                                If(Not(And(EToken("creationTime", "distributed", "docstring", "extend_schema", "folder", "format", "ignoreFirstRecord", "ingestIfNotExists", "ingestionMappingReference", "ingestionMapping", "persistDetails", "policy_ingestionTime", "recreate_schema", "tags", "validationPolicy", "zipPattern"))), rules.NameDeclarationOrStringLiteral)),
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("<|"))),
                        () => CreateMissingEToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    new [] {CD(), CD(), CD(), CD("QueryOrCommand", CompletionHint.Tabular)}));

            var AttachDatabaseMetadata = Command("AttachDatabaseMetadata", 
                Custom(
                    EToken("attach", CompletionKind.CommandPrefix),
                    EToken("database"),
                    EToken("metadata"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredEToken("from"),
                    Required(
                        First(
                            Custom(
                                rules.StringLiteral,
                                EToken(";"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                new [] {CD("BlobContainerUrl", CompletionHint.Literal), CD(), CD("StorageAccountKey", CompletionHint.Literal)}),
                            Custom(
                                rules.StringLiteral,
                                CD("Path", CompletionHint.Literal))),
                        () => (SyntaxElement)new CustomNode(new [] {CD("BlobContainerUrl", CompletionHint.Literal), CD(), CD("StorageAccountKey", CompletionHint.Literal)}, rules.MissingStringLiteral(), CreateMissingEToken(";"), rules.MissingStringLiteral())),
                    new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(CompletionHint.Literal)}));

            var AttachDatabase = Command("AttachDatabase", 
                Custom(
                    EToken("attach", CompletionKind.CommandPrefix),
                    EToken("database"),
                    Required(If(Not(EToken("metadata")), rules.DatabaseNameReference), rules.MissingNameReference),
                    RequiredEToken("from"),
                    Required(
                        First(
                            Custom(
                                rules.StringLiteral,
                                EToken(";"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                new [] {CD("BlobContainerUrl", CompletionHint.Literal), CD(), CD("StorageAccountKey", CompletionHint.Literal)}),
                            Custom(
                                rules.StringLiteral,
                                CD("Path", CompletionHint.Literal))),
                        () => (SyntaxElement)new CustomNode(new [] {CD("BlobContainerUrl", CompletionHint.Literal), CD(), CD("StorageAccountKey", CompletionHint.Literal)}, rules.MissingStringLiteral(), CreateMissingEToken(";"), rules.MissingStringLiteral())),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(CompletionHint.Literal)}));

            var AttachExtentsIntoTableByContainer = Command("AttachExtentsIntoTableByContainer", 
                Custom(
                    EToken("attach", CompletionKind.CommandPrefix),
                    EToken("extents"),
                    EToken("into"),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("by"),
                    EToken("container"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(
                        List(
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                CD("eid", CompletionHint.Literal)),
                            missingElement: null,
                            oneOrMore: true,
                            producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray())),
                        () => new SyntaxList<SyntaxElement>(rules.MissingValue())),
                    new [] {CD(), CD(), CD(), CD(), CD("tableName", CompletionHint.Table), CD(), CD(), CD("containerUri", CompletionHint.Literal), CD(CompletionHint.Literal)}));

            var AttachExtentsIntoTableByMetadata = Command("AttachExtentsIntoTableByMetadata", 
                Custom(
                    EToken("attach", CompletionKind.CommandPrefix),
                    Required(
                        First(
                            Custom(
                                EToken("async"),
                                RequiredEToken("extents")),
                            EToken("extents")),
                        () => (SyntaxElement)new CustomNode(CreateMissingEToken("async"), CreateMissingEToken("extents"))),
                    List(
                        elementParser: Custom(
                            EToken("into"),
                            RequiredEToken("table"),
                            Required(rules.TableNameReference, rules.MissingNameReference),
                            new [] {CD(), CD(), CD("tableName", CompletionHint.Table)}),
                        missingElement: () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("tableName", CompletionHint.Table)}, CreateMissingEToken("into"), CreateMissingEToken("table"), rules.MissingNameReference()),
                        oneOrMore: false,
                        producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray())),
                    RequiredEToken("by"),
                    RequiredEToken("metadata"),
                    Required(
                        Custom(
                            EToken("<|"),
                            Required(rules.CommandInput, rules.MissingExpression),
                            new [] {CD(), CD(CompletionHint.Tabular)}),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Tabular)}, CreateMissingEToken("<|"), rules.MissingExpression())),
                    new [] {CD(), CD(), CD(), CD(), CD(), CD("csl")}));

            var CancelOperation = Command("CancelOperation", 
                Custom(
                    EToken("cancel", CompletionKind.CommandPrefix),
                    EToken("operation"),
                    Required(rules.AnyGuidLiteralOrString, rules.MissingValue),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD("obj", CompletionHint.Literal), CD(isOptional: true)}));

            var CancelQuery = Command("CancelQuery", 
                Custom(
                    EToken("cancel", CompletionKind.CommandPrefix),
                    RequiredEToken("query"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("ClientRequestId", CompletionHint.Literal)}));

            var CleanDatabaseExtentContainers = Command("CleanDatabaseExtentContainers", 
                Custom(
                    EToken("clean", CompletionKind.CommandPrefix),
                    RequiredEToken("databases"),
                    Optional(
                        First(
                            Custom(
                                EToken("("),
                                CommaList(
                                    Custom(
                                        If(Not(EToken(")")), rules.DatabaseNameReference),
                                        CD("databaseName", CompletionHint.Database))),
                                RequiredEToken(")"),
                                new [] {CD(), CD(CompletionHint.Database), CD()}),
                            Custom(
                                EToken("async"),
                                Optional(
                                    Custom(
                                        EToken("("),
                                        CommaList(
                                            Custom(
                                                If(Not(EToken(")")), rules.DatabaseNameReference),
                                                CD("databaseName", CompletionHint.Database))),
                                        RequiredEToken(")"),
                                        new [] {CD(), CD(CompletionHint.Database), CD()})),
                                new [] {CD(), CD(isOptional: true)}))),
                    RequiredEToken("extentcontainers"),
                    new [] {CD(), CD(), CD(isOptional: true), CD()}));

            var ClearRemoteClusterDatabaseSchema = Command("ClearRemoteClusterDatabaseSchema", 
                Custom(
                    new Parser<LexicalToken>[] {
                        EToken("clear", CompletionKind.CommandPrefix),
                        EToken("cache"),
                        RequiredEToken("remote-schema"),
                        RequiredEToken("cluster"),
                        RequiredEToken("("),
                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                        RequiredEToken(")"),
                        RequiredEToken("."),
                        RequiredEToken("database"),
                        RequiredEToken("("),
                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                        RequiredEToken(")")}
                    ,
                    new [] {CD(), CD(), CD(), CD(), CD(), CD("clusterName", CompletionHint.Literal), CD(), CD(), CD(), CD(), CD("databaseName", CompletionHint.Literal), CD()}));

            var ClearDatabaseCacheQueryResults = Command("ClearDatabaseCacheQueryResults", 
                Custom(
                    EToken("clear", CompletionKind.CommandPrefix),
                    EToken("database"),
                    EToken("cache"),
                    EToken("query_results")));

            var ClearDatabaseCacheQueryWeakConsistency = Command("ClearDatabaseCacheQueryWeakConsistency", 
                Custom(
                    EToken("clear", CompletionKind.CommandPrefix),
                    EToken("database"),
                    EToken("cache"),
                    EToken("query_weak_consistency")));

            var ClearDatabaseCacheStreamingIngestionSchema = Command("ClearDatabaseCacheStreamingIngestionSchema", 
                Custom(
                    EToken("clear", CompletionKind.CommandPrefix),
                    EToken("database"),
                    RequiredEToken("cache"),
                    RequiredEToken("streamingingestion"),
                    RequiredEToken("schema")));

            var ClearMaterializedViewData = Command("ClearMaterializedViewData", 
                Custom(
                    EToken("clear", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("data"),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD()}));

            var ClearMaterializedViewStatistics = Command("ClearMaterializedViewStatistics", 
                Custom(
                    EToken("clear", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    RequiredEToken("statistics"),
                    new [] {CD(), CD(), CD("viewName", CompletionHint.MaterializedView), CD()}));

            var ClearTableCacheStreamingIngestionSchema = Command("ClearTableCacheStreamingIngestionSchema", 
                Custom(
                    EToken("clear", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("cache"),
                    RequiredEToken("streamingingestion"),
                    RequiredEToken("schema"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD()}));

            var ClearTableData = Command("ClearTableData", 
                Custom(
                    EToken("clear", CompletionKind.CommandPrefix),
                    Required(
                        First(
                            Custom(
                                EToken("async"),
                                RequiredEToken("table")),
                            EToken("table")),
                        () => (SyntaxElement)new CustomNode(CreateMissingEToken("async"), CreateMissingEToken("table"))),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredEToken("data"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD()}));

            var CreateMergeTables = Command("CreateMergeTables", 
                Custom(
                    EToken("create-merge", CompletionKind.CommandPrefix),
                    EToken("tables"),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.NameDeclarationOrStringLiteral,
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredEToken(":"),
                                            Required(rules.Type, rules.MissingType),
                                            new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType())))),
                                RequiredEToken(")"),
                                new [] {CD("TableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD()}),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("TableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD()}, rules.MissingNameDeclaration(), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()))), CreateMissingEToken(")")),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("TableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD()}, rules.MissingNameDeclaration(), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()))), CreateMissingEToken(")"))))),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD(CompletionHint.None), CD(isOptional: true)}));

            var CreateMergeTable = Command("CreateMergeTable", 
                Custom(
                    EToken("create-merge", CompletionKind.CommandPrefix),
                    RequiredEToken("table"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.NameDeclarationOrStringLiteral,
                                RequiredEToken(":"),
                                Required(rules.Type, rules.MissingType),
                                new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType())))),
                    RequiredEToken(")"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD()}));

            var CreateOrAlterContinuousExport = Command("CreateOrAlterContinuousExport", 
                Custom(
                    EToken("create-or-alter", CompletionKind.CommandPrefix),
                    EToken("continuous-export"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Required(
                        First(
                            Custom(
                                EToken("over"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            CD("TableName", CompletionHint.None)),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingNameDeclaration,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration()))),
                                RequiredEToken(")"),
                                RequiredEToken("to"),
                                new [] {CD(), CD(), CD(CompletionHint.None), CD(), CD()}),
                            EToken("to")),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD(CompletionHint.None), CD(), CD()}, CreateMissingEToken("over"), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration())), CreateMissingEToken(")"), CreateMissingEToken("to"))),
                    RequiredEToken("table"),
                    Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                    Required(
                        First(
                            EToken("<|"),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("<|"),
                                new [] {CD(), CD(), CD(CompletionHint.None), CD(), CD()})),
                        () => CreateMissingEToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    new [] {CD(), CD(), CD("ContinuousExportName", CompletionHint.None), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD("Query", CompletionHint.Tabular)}));

            var CreateOrAlterSqlExternalTable = Command("CreateOrAlterSqlExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        EToken("create-or-alter", CompletionKind.CommandPrefix),
                        EToken("external"),
                        EToken("table"),
                        rules.NameDeclarationOrStringLiteral,
                        EToken("("),
                        OList(
                            primaryElementParser: Custom(
                                rules.NameDeclarationOrStringLiteral,
                                RequiredEToken(":"),
                                Required(rules.Type, rules.MissingType),
                                new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        EToken(")"),
                        EToken("kind"),
                        RequiredEToken("="),
                        RequiredEToken("sql"),
                        RequiredEToken("table"),
                        RequiredEToken("="),
                        Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                        RequiredEToken("("),
                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                        RequiredEToken(")"),
                        Optional(
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                new [] {CD(), CD(), CD(CompletionHint.None), CD()}))}
                    ,
                    new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD(), CD(), CD(), CD("TableKind"), CD(), CD(), CD(CompletionHint.None), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var CreateOrAlterStorageExternalTable = Command("CreateOrAlterStorageExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        EToken("create-or-alter", CompletionKind.CommandPrefix),
                        EToken("external"),
                        RequiredEToken("table"),
                        Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                        RequiredEToken("("),
                        Required(
                            OList(
                                primaryElementParser: Custom(
                                    rules.NameDeclarationOrStringLiteral,
                                    RequiredEToken(":"),
                                    Required(rules.Type, rules.MissingType),
                                    new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()),
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType())))),
                        RequiredEToken(")"),
                        RequiredEToken("kind"),
                        RequiredEToken("="),
                        Required(
                            First(
                                EToken("adl").Hide(),
                                EToken("blob").Hide(),
                                EToken("storage")),
                            () => CreateMissingEToken("adl")),
                        Required(
                            First(
                                EToken("dataformat"),
                                Custom(
                                    EToken("partition"),
                                    RequiredEToken("by"),
                                    RequiredEToken("("),
                                    Required(
                                        OList(
                                            primaryElementParser: Custom(
                                                rules.NameDeclarationOrStringLiteral,
                                                RequiredEToken(":"),
                                                Required(
                                                    First(
                                                        Custom(
                                                            EToken("datetime"),
                                                            Optional(
                                                                Custom(
                                                                    EToken("="),
                                                                    Required(
                                                                        First(
                                                                            Custom(
                                                                                EToken("bin"),
                                                                                RequiredEToken("("),
                                                                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                                RequiredEToken(","),
                                                                                Required(rules.Value, rules.MissingValue),
                                                                                RequiredEToken(")"),
                                                                                new [] {CD("PartitionFunction"), CD(), CD("DateTimeColumn", CompletionHint.None), CD(), CD("BinValue", CompletionHint.Literal), CD()}),
                                                                            Custom(
                                                                                EToken("startofday", "startofmonth", "startofweek", "startofyear"),
                                                                                RequiredEToken("("),
                                                                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                                RequiredEToken(")"),
                                                                                new [] {CD("PartitionFunction"), CD(), CD("DateTimeColumn", CompletionHint.None), CD()})),
                                                                        () => (SyntaxElement)new CustomNode(new [] {CD("PartitionFunction"), CD(), CD("DateTimeColumn", CompletionHint.None), CD(), CD("BinValue", CompletionHint.Literal), CD()}, CreateMissingEToken("bin"), CreateMissingEToken("("), rules.MissingNameDeclaration(), CreateMissingEToken(","), rules.MissingValue(), CreateMissingEToken(")"))))),
                                                            new [] {CD("PartitionType"), CD(isOptional: true)}),
                                                        Custom(
                                                            EToken("long"),
                                                            RequiredEToken("="),
                                                            RequiredEToken("hash"),
                                                            RequiredEToken("("),
                                                            Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                            RequiredEToken(","),
                                                            Required(rules.Value, rules.MissingValue),
                                                            RequiredEToken(")"),
                                                            new [] {CD("PartitionType"), CD(), CD("PartitionFunction"), CD(), CD("StringColumn", CompletionHint.None), CD(), CD("HashMod", CompletionHint.Literal), CD()}),
                                                        Custom(
                                                            EToken("string"),
                                                            Optional(
                                                                Custom(
                                                                    EToken("="),
                                                                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                    new [] {CD(), CD("StringColumn", CompletionHint.None)})),
                                                            new [] {CD("PartitionType"), CD(isOptional: true)})),
                                                    () => (SyntaxElement)new CustomNode(new [] {CD("PartitionType"), CD(isOptional: true)}, CreateMissingEToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingEToken("="), (SyntaxElement)new CustomNode(new [] {CD("PartitionFunction"), CD(), CD("DateTimeColumn", CompletionHint.None), CD(), CD("BinValue", CompletionHint.Literal), CD()}, CreateMissingEToken("bin"), CreateMissingEToken("("), rules.MissingNameDeclaration(), CreateMissingEToken(","), rules.MissingValue(), CreateMissingEToken(")"))))),
                                                new [] {CD("PartitionName", CompletionHint.None), CD(), CD()}),
                                            separatorParser: EToken(","),
                                            secondaryElementParser: null,
                                            missingPrimaryElement: null,
                                            missingSeparator: null,
                                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PartitionName", CompletionHint.None), CD(), CD()}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), (SyntaxElement)new CustomNode(new [] {CD("PartitionType"), CD(isOptional: true)}, CreateMissingEToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingEToken("="), (SyntaxElement)new CustomNode(new [] {CD("PartitionFunction"), CD(), CD("DateTimeColumn", CompletionHint.None), CD(), CD("BinValue", CompletionHint.Literal), CD()}, CreateMissingEToken("bin"), CreateMissingEToken("("), rules.MissingNameDeclaration(), CreateMissingEToken(","), rules.MissingValue(), CreateMissingEToken(")"))))),
                                            endOfList: null,
                                            oneOrMore: true,
                                            allowTrailingSeparator: false,
                                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PartitionName", CompletionHint.None), CD(), CD()}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), (SyntaxElement)new CustomNode(new [] {CD("PartitionType"), CD(isOptional: true)}, CreateMissingEToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingEToken("="), (SyntaxElement)new CustomNode(new [] {CD("PartitionFunction"), CD(), CD("DateTimeColumn", CompletionHint.None), CD(), CD("BinValue", CompletionHint.Literal), CD()}, CreateMissingEToken("bin"), CreateMissingEToken("("), rules.MissingNameDeclaration(), CreateMissingEToken(","), rules.MissingValue(), CreateMissingEToken(")")))))))),
                                    RequiredEToken(")"),
                                    Optional(
                                        Custom(
                                            EToken("pathformat"),
                                            RequiredEToken("="),
                                            RequiredEToken("("),
                                            Required(
                                                First(
                                                    Custom(
                                                        rules.StringLiteral,
                                                        Required(
                                                            List(
                                                                Custom(
                                                                    First(
                                                                        Custom(
                                                                            EToken("datetime_pattern"),
                                                                            RequiredEToken("("),
                                                                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                                                                            RequiredEToken(","),
                                                                            Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                            RequiredEToken(")"),
                                                                            new [] {CD(), CD(), CD("DateTimeFormat", CompletionHint.Literal), CD(), CD("PartitionName", CompletionHint.None), CD()}),
                                                                        Custom(
                                                                            If(Not(EToken("datetime_pattern")), rules.NameDeclarationOrStringLiteral),
                                                                            CD("PartitionName", CompletionHint.None))),
                                                                    Optional(
                                                                        Custom(
                                                                            rules.StringLiteral,
                                                                            CD("PathSeparator", CompletionHint.Literal))),
                                                                    new [] {CD(), CD(CompletionHint.Literal, isOptional: true)}),
                                                                missingElement: null,
                                                                oneOrMore: true,
                                                                producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray())),
                                                            () => new SyntaxList<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Literal, isOptional: true)}, (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("DateTimeFormat", CompletionHint.Literal), CD(), CD("PartitionName", CompletionHint.None), CD()}, CreateMissingEToken("datetime_pattern"), CreateMissingEToken("("), rules.MissingStringLiteral(), CreateMissingEToken(","), rules.MissingNameDeclaration(), CreateMissingEToken(")")), rules.MissingStringLiteral()))),
                                                        new [] {CD("PathSeparator", CompletionHint.Literal), CD()}),
                                                    List(
                                                        Custom(
                                                            First(
                                                                Custom(
                                                                    EToken("datetime_pattern"),
                                                                    RequiredEToken("("),
                                                                    rules.StringLiteral,
                                                                    EToken(","),
                                                                    rules.NameDeclarationOrStringLiteral,
                                                                    EToken(")"),
                                                                    new [] {CD(), CD(), CD("DateTimeFormat", CompletionHint.Literal), CD(), CD("PartitionName", CompletionHint.None), CD()}),
                                                                Custom(
                                                                    EToken("datetime_pattern"),
                                                                    RequiredEToken("("),
                                                                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                                                                    RequiredEToken(","),
                                                                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                    RequiredEToken(")"),
                                                                    new [] {CD(), CD(), CD("DateTimeFormat", CompletionHint.Literal), CD(), CD("PartitionName", CompletionHint.None), CD()}),
                                                                Custom(
                                                                    If(Not(EToken("datetime_pattern")), rules.NameDeclarationOrStringLiteral),
                                                                    CD("PartitionName", CompletionHint.None))),
                                                            Optional(
                                                                Custom(
                                                                    rules.StringLiteral,
                                                                    CD("PathSeparator", CompletionHint.Literal))),
                                                            new [] {CD(), CD(CompletionHint.Literal, isOptional: true)}),
                                                        missingElement: null,
                                                        oneOrMore: true,
                                                        producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray()))),
                                                () => (SyntaxElement)new CustomNode(new [] {CD("PathSeparator", CompletionHint.Literal), CD()}, rules.MissingStringLiteral(), new SyntaxList<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Literal, isOptional: true)}, (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("DateTimeFormat", CompletionHint.Literal), CD(), CD("PartitionName", CompletionHint.None), CD()}, CreateMissingEToken("datetime_pattern"), CreateMissingEToken("("), rules.MissingStringLiteral(), CreateMissingEToken(","), rules.MissingNameDeclaration(), CreateMissingEToken(")")), rules.MissingStringLiteral())))),
                                            RequiredEToken(")"),
                                            new [] {CD(), CD(), CD(), CD(CompletionHint.Literal), CD()})),
                                    RequiredEToken("dataformat"),
                                    new [] {CD(), CD(), CD(), CD(CompletionHint.None), CD(), CD(isOptional: true), CD()})),
                            () => CreateMissingEToken("dataformat")),
                        RequiredEToken("="),
                        RequiredEToken("apacheavro", "avro", "csv", "json", "multijson", "orc", "parquet", "psv", "raw", "scsv", "sohsv", "sstream", "tsve", "tsv", "txt", "w3clogfile"),
                        RequiredEToken("("),
                        Required(
                            OList(
                                primaryElementParser: Custom(
                                    rules.StringLiteral,
                                    CD("StorageConnectionString", CompletionHint.Literal)),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingStringLiteral,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                        RequiredEToken(")"),
                        Optional(
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                new [] {CD(), CD(), CD(CompletionHint.None), CD()}))}
                    ,
                    new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD(), CD(), CD(), CD("TableKind"), CD(), CD(), CD("DataFormatKind"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var CreateOrAlterFunction = Command("CreateOrAlterFunction", 
                Custom(
                    EToken("create-or-alter", CompletionKind.CommandPrefix),
                    EToken("function"),
                    Required(
                        First(
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                new [] {CD(), CD(), CD(CompletionHint.None), CD(), CD("FunctionName", CompletionHint.None)}),
                            Custom(
                                If(Not(EToken("with")), rules.NameDeclarationOrStringLiteral),
                                CD("FunctionName", CompletionHint.None))),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD(CompletionHint.None), CD(), CD("FunctionName", CompletionHint.None)}, CreateMissingEToken("with"), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()))), CreateMissingEToken(")"), rules.MissingNameDeclaration())),
                    Required(rules.FunctionDeclaration, rules.MissingFunctionDeclaration)));

            var CreateOrAlterMaterializedView = Command("CreateOrAlterMaterializedView", 
                Custom(
                    EToken("create-or-alter", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    Required(
                        First(
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            First(
                                                EToken("autoUpdateSchema"),
                                                EToken("backfill"),
                                                EToken("dimensionTables"),
                                                EToken("docString"),
                                                EToken("effectiveDateTime"),
                                                EToken("folder"),
                                                EToken("lookback"),
                                                EToken("updateExtentsCreationTime"),
                                                If(Not(And(EToken("autoUpdateSchema", "backfill", "dimensionTables", "docString", "effectiveDateTime", "folder", "lookback", "updateExtentsCreationTime"))), rules.NameDeclarationOrStringLiteral)),
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("autoUpdateSchema"), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("autoUpdateSchema"), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                                new [] {CD(), CD(), CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView)}),
                            Custom(
                                If(Not(EToken("with")), rules.MaterializedViewNameReference),
                                CD("MaterializedViewName", CompletionHint.MaterializedView))),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView)}, CreateMissingEToken("with"), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("autoUpdateSchema"), CreateMissingEToken("="), rules.MissingValue()))), CreateMissingEToken(")"), rules.MissingNameReference())),
                    RequiredEToken("on"),
                    RequiredEToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Required(rules.FunctionBody, rules.MissingFunctionBody),
                    new [] {CD(), CD(), CD(), CD(), CD(), CD(CompletionHint.Table), CD()}));

            var CreateOrAleterWorkloadGroup = Command("CreateOrAleterWorkloadGroup", 
                Custom(
                    EToken("create-or-alter", CompletionKind.CommandPrefix),
                    RequiredEToken("workload_group"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("WorkloadGroupName", CompletionHint.None), CD("WorkloadGroup", CompletionHint.Literal)}));

            var CreateBasicAuthUser = Command("CreateBasicAuthUser", 
                Custom(
                    EToken("create", CompletionKind.CommandPrefix),
                    EToken("basicauth"),
                    RequiredEToken("user"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        Custom(
                            EToken("password"),
                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                            new [] {CD(), CD("Password", CompletionHint.Literal)})),
                    new [] {CD(), CD(), CD(), CD("UserName", CompletionHint.Literal), CD(isOptional: true)}));

            var CreateDatabasePersist = Command("CreateDatabasePersist", 
                Custom(
                    EToken("create", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.NameDeclarationOrStringLiteral,
                    EToken("persist"),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                CD("Container", CompletionHint.Literal)),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredEToken(")"),
                    Optional(EToken("ifnotexists")),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.None), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var CreateDatabaseVolatile = Command("CreateDatabaseVolatile", 
                Custom(
                    EToken("create", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.NameDeclarationOrStringLiteral,
                    EToken("volatile"),
                    Optional(EToken("ifnotexists")),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.None), CD(), CD(isOptional: true)}));

            var CreateDatabaseIngestionMapping = Command("CreateDatabaseIngestionMapping", 
                Custom(
                    EToken("create", CompletionKind.CommandPrefix),
                    EToken("database"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    RequiredEToken("ingestion"),
                    RequiredEToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredEToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.None), CD(), CD("MappingKind"), CD(), CD("MappingName", CompletionHint.Literal), CD("MappingFormat", CompletionHint.Literal)}));

            var CreateSqlExternalTable = Command("CreateSqlExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        EToken("create", CompletionKind.CommandPrefix),
                        EToken("external"),
                        EToken("table"),
                        rules.NameDeclarationOrStringLiteral,
                        EToken("("),
                        OList(
                            primaryElementParser: Custom(
                                rules.NameDeclarationOrStringLiteral,
                                RequiredEToken(":"),
                                Required(rules.Type, rules.MissingType),
                                new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        EToken(")"),
                        EToken("kind"),
                        RequiredEToken("="),
                        RequiredEToken("sql"),
                        RequiredEToken("table"),
                        RequiredEToken("="),
                        Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                        RequiredEToken("("),
                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                        RequiredEToken(")"),
                        Optional(
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                new [] {CD(), CD(), CD(CompletionHint.None), CD()}))}
                    ,
                    new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD(), CD(), CD(), CD("TableKind"), CD(), CD(), CD(CompletionHint.None), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var CreateStorageExternalTable = Command("CreateStorageExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        EToken("create", CompletionKind.CommandPrefix),
                        EToken("external"),
                        EToken("table"),
                        rules.NameDeclarationOrStringLiteral,
                        RequiredEToken("("),
                        Required(
                            OList(
                                primaryElementParser: Custom(
                                    rules.NameDeclarationOrStringLiteral,
                                    RequiredEToken(":"),
                                    Required(rules.Type, rules.MissingType),
                                    new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()),
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType())))),
                        RequiredEToken(")"),
                        RequiredEToken("kind"),
                        RequiredEToken("="),
                        Required(
                            First(
                                EToken("adl").Hide(),
                                EToken("blob").Hide(),
                                EToken("storage")),
                            () => CreateMissingEToken("adl")),
                        Required(
                            First(
                                EToken("dataformat"),
                                Custom(
                                    EToken("partition"),
                                    RequiredEToken("by"),
                                    RequiredEToken("("),
                                    Required(
                                        OList(
                                            primaryElementParser: Custom(
                                                rules.NameDeclarationOrStringLiteral,
                                                RequiredEToken(":"),
                                                Required(
                                                    First(
                                                        Custom(
                                                            EToken("datetime"),
                                                            Optional(
                                                                Custom(
                                                                    EToken("="),
                                                                    Required(
                                                                        First(
                                                                            Custom(
                                                                                EToken("bin"),
                                                                                RequiredEToken("("),
                                                                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                                RequiredEToken(","),
                                                                                Required(rules.Value, rules.MissingValue),
                                                                                RequiredEToken(")"),
                                                                                new [] {CD("PartitionFunction"), CD(), CD("DateTimeColumn", CompletionHint.None), CD(), CD("BinValue", CompletionHint.Literal), CD()}),
                                                                            Custom(
                                                                                EToken("startofday", "startofmonth", "startofweek", "startofyear"),
                                                                                RequiredEToken("("),
                                                                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                                RequiredEToken(")"),
                                                                                new [] {CD("PartitionFunction"), CD(), CD("DateTimeColumn", CompletionHint.None), CD()})),
                                                                        () => (SyntaxElement)new CustomNode(new [] {CD("PartitionFunction"), CD(), CD("DateTimeColumn", CompletionHint.None), CD(), CD("BinValue", CompletionHint.Literal), CD()}, CreateMissingEToken("bin"), CreateMissingEToken("("), rules.MissingNameDeclaration(), CreateMissingEToken(","), rules.MissingValue(), CreateMissingEToken(")"))))),
                                                            new [] {CD("PartitionType"), CD(isOptional: true)}),
                                                        Custom(
                                                            EToken("long"),
                                                            RequiredEToken("="),
                                                            RequiredEToken("hash"),
                                                            RequiredEToken("("),
                                                            Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                            RequiredEToken(","),
                                                            Required(rules.Value, rules.MissingValue),
                                                            RequiredEToken(")"),
                                                            new [] {CD("PartitionType"), CD(), CD("PartitionFunction"), CD(), CD("StringColumn", CompletionHint.None), CD(), CD("HashMod", CompletionHint.Literal), CD()}),
                                                        Custom(
                                                            EToken("string"),
                                                            Optional(
                                                                Custom(
                                                                    EToken("="),
                                                                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                    new [] {CD(), CD("StringColumn", CompletionHint.None)})),
                                                            new [] {CD("PartitionType"), CD(isOptional: true)})),
                                                    () => (SyntaxElement)new CustomNode(new [] {CD("PartitionType"), CD(isOptional: true)}, CreateMissingEToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingEToken("="), (SyntaxElement)new CustomNode(new [] {CD("PartitionFunction"), CD(), CD("DateTimeColumn", CompletionHint.None), CD(), CD("BinValue", CompletionHint.Literal), CD()}, CreateMissingEToken("bin"), CreateMissingEToken("("), rules.MissingNameDeclaration(), CreateMissingEToken(","), rules.MissingValue(), CreateMissingEToken(")"))))),
                                                new [] {CD("PartitionName", CompletionHint.None), CD(), CD()}),
                                            separatorParser: EToken(","),
                                            secondaryElementParser: null,
                                            missingPrimaryElement: null,
                                            missingSeparator: null,
                                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PartitionName", CompletionHint.None), CD(), CD()}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), (SyntaxElement)new CustomNode(new [] {CD("PartitionType"), CD(isOptional: true)}, CreateMissingEToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingEToken("="), (SyntaxElement)new CustomNode(new [] {CD("PartitionFunction"), CD(), CD("DateTimeColumn", CompletionHint.None), CD(), CD("BinValue", CompletionHint.Literal), CD()}, CreateMissingEToken("bin"), CreateMissingEToken("("), rules.MissingNameDeclaration(), CreateMissingEToken(","), rules.MissingValue(), CreateMissingEToken(")"))))),
                                            endOfList: null,
                                            oneOrMore: true,
                                            allowTrailingSeparator: false,
                                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PartitionName", CompletionHint.None), CD(), CD()}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), (SyntaxElement)new CustomNode(new [] {CD("PartitionType"), CD(isOptional: true)}, CreateMissingEToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingEToken("="), (SyntaxElement)new CustomNode(new [] {CD("PartitionFunction"), CD(), CD("DateTimeColumn", CompletionHint.None), CD(), CD("BinValue", CompletionHint.Literal), CD()}, CreateMissingEToken("bin"), CreateMissingEToken("("), rules.MissingNameDeclaration(), CreateMissingEToken(","), rules.MissingValue(), CreateMissingEToken(")")))))))),
                                    RequiredEToken(")"),
                                    Optional(
                                        Custom(
                                            EToken("pathformat"),
                                            RequiredEToken("="),
                                            RequiredEToken("("),
                                            Required(
                                                First(
                                                    Custom(
                                                        rules.StringLiteral,
                                                        Required(
                                                            List(
                                                                Custom(
                                                                    First(
                                                                        Custom(
                                                                            EToken("datetime_pattern"),
                                                                            RequiredEToken("("),
                                                                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                                                                            RequiredEToken(","),
                                                                            Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                            RequiredEToken(")"),
                                                                            new [] {CD(), CD(), CD("DateTimeFormat", CompletionHint.Literal), CD(), CD("PartitionName", CompletionHint.None), CD()}),
                                                                        Custom(
                                                                            If(Not(EToken("datetime_pattern")), rules.NameDeclarationOrStringLiteral),
                                                                            CD("PartitionName", CompletionHint.None))),
                                                                    Optional(
                                                                        Custom(
                                                                            rules.StringLiteral,
                                                                            CD("PathSeparator", CompletionHint.Literal))),
                                                                    new [] {CD(), CD(CompletionHint.Literal, isOptional: true)}),
                                                                missingElement: null,
                                                                oneOrMore: true,
                                                                producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray())),
                                                            () => new SyntaxList<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Literal, isOptional: true)}, (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("DateTimeFormat", CompletionHint.Literal), CD(), CD("PartitionName", CompletionHint.None), CD()}, CreateMissingEToken("datetime_pattern"), CreateMissingEToken("("), rules.MissingStringLiteral(), CreateMissingEToken(","), rules.MissingNameDeclaration(), CreateMissingEToken(")")), rules.MissingStringLiteral()))),
                                                        new [] {CD("PathSeparator", CompletionHint.Literal), CD()}),
                                                    List(
                                                        Custom(
                                                            First(
                                                                Custom(
                                                                    EToken("datetime_pattern"),
                                                                    RequiredEToken("("),
                                                                    rules.StringLiteral,
                                                                    EToken(","),
                                                                    rules.NameDeclarationOrStringLiteral,
                                                                    EToken(")"),
                                                                    new [] {CD(), CD(), CD("DateTimeFormat", CompletionHint.Literal), CD(), CD("PartitionName", CompletionHint.None), CD()}),
                                                                Custom(
                                                                    EToken("datetime_pattern"),
                                                                    RequiredEToken("("),
                                                                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                                                                    RequiredEToken(","),
                                                                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                    RequiredEToken(")"),
                                                                    new [] {CD(), CD(), CD("DateTimeFormat", CompletionHint.Literal), CD(), CD("PartitionName", CompletionHint.None), CD()}),
                                                                Custom(
                                                                    If(Not(EToken("datetime_pattern")), rules.NameDeclarationOrStringLiteral),
                                                                    CD("PartitionName", CompletionHint.None))),
                                                            Optional(
                                                                Custom(
                                                                    rules.StringLiteral,
                                                                    CD("PathSeparator", CompletionHint.Literal))),
                                                            new [] {CD(), CD(CompletionHint.Literal, isOptional: true)}),
                                                        missingElement: null,
                                                        oneOrMore: true,
                                                        producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray()))),
                                                () => (SyntaxElement)new CustomNode(new [] {CD("PathSeparator", CompletionHint.Literal), CD()}, rules.MissingStringLiteral(), new SyntaxList<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Literal, isOptional: true)}, (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("DateTimeFormat", CompletionHint.Literal), CD(), CD("PartitionName", CompletionHint.None), CD()}, CreateMissingEToken("datetime_pattern"), CreateMissingEToken("("), rules.MissingStringLiteral(), CreateMissingEToken(","), rules.MissingNameDeclaration(), CreateMissingEToken(")")), rules.MissingStringLiteral())))),
                                            RequiredEToken(")"),
                                            new [] {CD(), CD(), CD(), CD(CompletionHint.Literal), CD()})),
                                    RequiredEToken("dataformat"),
                                    new [] {CD(), CD(), CD(), CD(CompletionHint.None), CD(), CD(isOptional: true), CD()})),
                            () => CreateMissingEToken("dataformat")),
                        RequiredEToken("="),
                        RequiredEToken("apacheavro", "avro", "csv", "json", "multijson", "orc", "parquet", "psv", "raw", "scsv", "sohsv", "sstream", "tsve", "tsv", "txt", "w3clogfile"),
                        RequiredEToken("("),
                        Required(
                            OList(
                                primaryElementParser: Custom(
                                    rules.StringLiteral,
                                    CD("StorageConnectionString", CompletionHint.Literal)),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingStringLiteral,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                        RequiredEToken(")"),
                        Optional(
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                new [] {CD(), CD(), CD(CompletionHint.None), CD()}))}
                    ,
                    new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD(), CD(), CD(), CD("TableKind"), CD(), CD(), CD("DataFormatKind"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var CreateExternalTableMapping = Command("CreateExternalTableMapping", 
                Custom(
                    EToken("create", CompletionKind.CommandPrefix),
                    EToken("external"),
                    RequiredEToken("table"),
                    Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                    RequiredEToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD("MappingName", CompletionHint.Literal), CD("MappingFormat", CompletionHint.Literal)}));

            var CreateFunction = Command("CreateFunction", 
                Custom(
                    EToken("create", CompletionKind.CommandPrefix),
                    EToken("function"),
                    Optional(
                        First(
                            Custom(
                                EToken("ifnotexists"),
                                Optional(
                                    Custom(
                                        EToken("with"),
                                        RequiredEToken("("),
                                        CommaList(
                                            Custom(
                                                If(Not(EToken(")")), rules.NameDeclarationOrStringLiteral),
                                                RequiredEToken("="),
                                                Required(rules.Value, rules.MissingValue),
                                                new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)})),
                                        RequiredEToken(")"),
                                        new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                                new [] {CD(), CD(isOptional: true)}),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                CommaList(
                                    Custom(
                                        If(Not(EToken(")")), rules.NameDeclarationOrStringLiteral),
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)})),
                                RequiredEToken(")"),
                                new [] {CD(), CD(), CD(CompletionHint.None), CD()}))),
                    Required(If(Not(And(EToken("ifnotexists", "with"))), rules.NameDeclarationOrStringLiteral), rules.MissingNameDeclaration),
                    Required(rules.FunctionDeclaration, rules.MissingFunctionDeclaration),
                    new [] {CD(), CD(), CD(isOptional: true), CD("FunctionName", CompletionHint.None), CD()}));

            var CreateRequestSupport = Command("CreateRequestSupport", 
                Custom(
                    EToken("create", CompletionKind.CommandPrefix),
                    EToken("request_support"),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD(isOptional: true)}));

            var CreateRowStore = Command("CreateRowStore", 
                Custom(
                    EToken("create", CompletionKind.CommandPrefix),
                    EToken("rowstore"),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD(isOptional: true)}));

            var CreateTables = Command("CreateTables", 
                Custom(
                    EToken("create", CompletionKind.CommandPrefix),
                    EToken("tables"),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.NameDeclarationOrStringLiteral,
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredEToken(":"),
                                            Required(rules.Type, rules.MissingType),
                                            new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType())))),
                                RequiredEToken(")"),
                                new [] {CD("TableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD()}),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("TableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD()}, rules.MissingNameDeclaration(), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()))), CreateMissingEToken(")")),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("TableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD()}, rules.MissingNameDeclaration(), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()))), CreateMissingEToken(")"))))),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD(CompletionHint.None), CD(isOptional: true)}));

            var CreateTable = Command("CreateTable", 
                Custom(
                    EToken("create", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.NameDeclarationOrStringLiteral,
                    EToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.NameDeclarationOrStringLiteral,
                                RequiredEToken(":"),
                                Required(rules.Type, rules.MissingType),
                                new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType())))),
                    RequiredEToken(")"),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        First(
                                            EToken("docstring"),
                                            EToken("folder"),
                                            If(Not(And(EToken("docstring", "folder"))), rules.NameDeclarationOrStringLiteral)),
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("docstring"), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("docstring"), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"))),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD(), CD(isOptional: true)}));

            var CreateTableBasedOnAnother = Command("CreateTableBasedOnAnother", 
                Custom(
                    EToken("create", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.NameDeclarationOrStringLiteral,
                    EToken("based-on"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        First(
                                            EToken("docstring"),
                                            EToken("folder"),
                                            If(Not(And(EToken("docstring", "folder"))), rules.NameDeclarationOrStringLiteral)),
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("docstring"), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("docstring"), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"))),
                    new [] {CD(), CD(), CD("NewTableName", CompletionHint.None), CD(), CD("TableName", CompletionHint.None), CD(isOptional: true)}));

            var CreateTableIngestionMapping = Command("CreateTableIngestionMapping", 
                Custom(
                    EToken("create", CompletionKind.CommandPrefix),
                    EToken("table"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    RequiredEToken("ingestion"),
                    RequiredEToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredEToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.None), CD(), CD("MappingKind"), CD(), CD("MappingName", CompletionHint.Literal), CD("MappingFormat", CompletionHint.Literal)}));

            var CreateTempStorage = Command("CreateTempStorage", 
                Custom(
                    EToken("create", CompletionKind.CommandPrefix),
                    EToken("tempstorage")));

            var CreateMaterializedView = Command("CreateMaterializedView", 
                Custom(
                    EToken("create", CompletionKind.CommandPrefix),
                    Optional(
                        First(
                            Custom(
                                EToken("async"),
                                Optional(EToken("ifnotexists")),
                                new [] {CD(), CD(isOptional: true)}),
                            EToken("ifnotexists"))),
                    RequiredEToken("materialized-view"),
                    Required(
                        First(
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            First(
                                                EToken("autoUpdateSchema"),
                                                EToken("backfill"),
                                                EToken("dimensionTables"),
                                                EToken("docString"),
                                                EToken("effectiveDateTime"),
                                                EToken("folder"),
                                                EToken("lookback"),
                                                EToken("updateExtentsCreationTime"),
                                                If(Not(And(EToken("autoUpdateSchema", "backfill", "dimensionTables", "docString", "effectiveDateTime", "folder", "lookback", "updateExtentsCreationTime"))), rules.NameDeclarationOrStringLiteral)),
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("autoUpdateSchema"), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("autoUpdateSchema"), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                new [] {CD(), CD(), CD(), CD(), CD("MaterializedViewName", CompletionHint.None)}),
                            Custom(
                                If(Not(EToken("with")), rules.NameDeclarationOrStringLiteral),
                                CD("MaterializedViewName", CompletionHint.None))),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD(), CD(), CD("MaterializedViewName", CompletionHint.None)}, CreateMissingEToken("with"), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("autoUpdateSchema"), CreateMissingEToken("="), rules.MissingValue()))), CreateMissingEToken(")"), rules.MissingNameDeclaration())),
                    RequiredEToken("on"),
                    RequiredEToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Required(rules.FunctionBody, rules.MissingFunctionBody),
                    new [] {CD(), CD(isOptional: true), CD(), CD(), CD(), CD(), CD(CompletionHint.Table), CD()}));

            var DefineTables = Command("DefineTables", 
                Custom(
                    EToken("define", CompletionKind.CommandPrefix),
                    RequiredEToken("tables"),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.NameDeclarationOrStringLiteral,
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredEToken(":"),
                                            Required(rules.Type, rules.MissingType),
                                            new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType())))),
                                RequiredEToken(")"),
                                new [] {CD("TableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD()}),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("TableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD()}, rules.MissingNameDeclaration(), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()))), CreateMissingEToken(")")),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("TableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD()}, rules.MissingNameDeclaration(), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")}, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()))), CreateMissingEToken(")"))))),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD(CompletionHint.None), CD(isOptional: true)}));

            var DeleteClusterPolicyCaching = Command("DeleteClusterPolicyCaching", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("caching")));

            var DeleteClusterPolicyCallout = Command("DeleteClusterPolicyCallout", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("callout")));

            var DeleteClusterPolicyManagedIdentity = Command("DeleteClusterPolicyManagedIdentity", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("managed_identity")));

            var DeleteClusterPolicyRequestClassification = Command("DeleteClusterPolicyRequestClassification", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("request_classification")));

            var DeleteClusterPolicyRowStore = Command("DeleteClusterPolicyRowStore", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("rowstore")));

            var DeleteClusterPolicySandbox = Command("DeleteClusterPolicySandbox", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("sandbox")));

            var DeleteClusterPolicySharding = Command("DeleteClusterPolicySharding", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("sharding")));

            var DeleteClusterPolicyStreamingIngestion = Command("DeleteClusterPolicyStreamingIngestion", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    RequiredEToken("policy"),
                    RequiredEToken("streamingingestion")));

            var DeleteColumnPolicyCaching = Command("DeleteColumnPolicyCaching", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("column"),
                    rules.DatabaseTableColumnNameReference,
                    RequiredEToken("policy"),
                    RequiredEToken("caching"),
                    new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD()}));

            var DeleteColumnPolicyEncoding = Command("DeleteColumnPolicyEncoding", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("column"),
                    Required(rules.TableColumnNameReference, rules.MissingNameReference),
                    RequiredEToken("policy"),
                    RequiredEToken("encoding"),
                    new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD()}));

            var DeleteDatabasePolicyCaching = Command("DeleteDatabasePolicyCaching", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("caching"),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD()}));

            var DeleteDatabasePolicyDiagnostics = Command("DeleteDatabasePolicyDiagnostics", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("diagnostics"),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD()}));

            var DeleteDatabasePolicyEncoding = Command("DeleteDatabasePolicyEncoding", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("encoding"),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD()}));

            var DeleteDatabasePolicyExtentTagsRetention = Command("DeleteDatabasePolicyExtentTagsRetention", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("extent_tags_retention"),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD()}));

            var DeleteDatabasePolicyIngestionBatching = Command("DeleteDatabasePolicyIngestionBatching", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("ingestionbatching"),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD()}));

            var DeleteDatabasePolicyManagedIdentity = Command("DeleteDatabasePolicyManagedIdentity", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("managed_identity"),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD()}));

            var DeleteDatabasePolicyMerge = Command("DeleteDatabasePolicyMerge", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("merge"),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD()}));

            var DeleteDatabasePolicyRetention = Command("DeleteDatabasePolicyRetention", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("retention"),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD()}));

            var DeleteDatabasePolicySharding = Command("DeleteDatabasePolicySharding", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("sharding"),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD()}));

            var DeleteDatabasePolicyShardsGrouping = Command("DeleteDatabasePolicyShardsGrouping", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("shards_grouping").Hide(),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD()}));

            var DeleteDatabasePolicyStreamingIngestion = Command("DeleteDatabasePolicyStreamingIngestion", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredEToken("policy"),
                    RequiredEToken("streamingingestion"),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD()}));

            var DropFollowerDatabasePolicyCaching = Command("DropFollowerDatabasePolicyCaching", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("follower"),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    RequiredEToken("caching"),
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(), CD()}));

            var DropFollowerTablesPolicyCaching = Command("DropFollowerTablesPolicyCaching", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("follower"),
                    RequiredEToken("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    Required(
                        First(
                            Custom(
                                EToken("materialized-views"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            CD("name", CompletionHint.None)),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingNameDeclaration,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration()))),
                                RequiredEToken(")"),
                                new [] {CD(), CD(), CD(CompletionHint.None), CD()}),
                            Custom(
                                EToken("materialized-view"),
                                Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                                new [] {CD(), CD("name", CompletionHint.MaterializedView)}),
                            Custom(
                                EToken("tables"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            CD("name", CompletionHint.None)),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingNameDeclaration,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration()))),
                                RequiredEToken(")"),
                                new [] {CD(), CD(), CD(CompletionHint.None), CD()}),
                            Custom(
                                EToken("table"),
                                Required(rules.TableNameReference, rules.MissingNameReference),
                                new [] {CD(), CD("name", CompletionHint.Table)})),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD(CompletionHint.None), CD()}, CreateMissingEToken("materialized-views"), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration())), CreateMissingEToken(")"))),
                    RequiredEToken("policy"),
                    RequiredEToken("caching"),
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(), CD(), CD()}));

            var DeleteMaterializedViewPolicyCaching = Command("DeleteMaterializedViewPolicyCaching", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("policy"),
                    EToken("caching"),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD()}));

            var DeleteMaterializedViewPolicyPartitioning = Command("DeleteMaterializedViewPolicyPartitioning", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("policy"),
                    EToken("partitioning"),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD()}));

            var DeleteMaterializedViewPolicyRowLevelSecurity = Command("DeleteMaterializedViewPolicyRowLevelSecurity", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    RequiredEToken("policy"),
                    RequiredEToken("row_level_security"),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD()}));

            var DeletePoliciesOfRetention = Command("DeletePoliciesOfRetention", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("policies"),
                    RequiredEToken("of"),
                    RequiredEToken("retention"),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                CD("entity", CompletionHint.Literal)),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredEToken(")"),
                    new [] {CD(), CD(), CD(), CD(), CD(), CD(CompletionHint.Literal), CD()}));

            var DeleteTablePolicyAutoDelete = Command("DeleteTablePolicyAutoDelete", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("auto_delete"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var DeleteTablePolicyCaching = Command("DeleteTablePolicyCaching", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("caching"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var DeleteTablePolicyEncoding = Command("DeleteTablePolicyEncoding", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("encoding"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var DeleteTablePolicyExtentTagsRetention = Command("DeleteTablePolicyExtentTagsRetention", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("extent_tags_retention"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var DeleteTablePolicyIngestionBatching = Command("DeleteTablePolicyIngestionBatching", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("ingestionbatching"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var DeleteTablePolicyMerge = Command("DeleteTablePolicyMerge", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("merge"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var DeleteTablePolicyRestrictedViewAccess = Command("DeleteTablePolicyRestrictedViewAccess", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("restricted_view_access"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var DeleteTablePolicyRetention = Command("DeleteTablePolicyRetention", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("retention"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var DeleteTablePolicyRowOrder = Command("DeleteTablePolicyRowOrder", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("roworder"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var DeleteTablePolicySharding = Command("DeleteTablePolicySharding", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("sharding"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var DeleteTablePolicyStreamingIngestion = Command("DeleteTablePolicyStreamingIngestion", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("streamingingestion"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var DeleteTablePolicyUpdate = Command("DeleteTablePolicyUpdate", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    RequiredEToken("policy"),
                    RequiredEToken("update"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var DeleteTablePolicyIngestionTime = Command("DeleteTablePolicyIngestionTime", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("policy"),
                    EToken("ingestiontime"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var DeleteTablePolicyPartitioning = Command("DeleteTablePolicyPartitioning", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("policy"),
                    EToken("partitioning"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var DeleteTablePolicyRowLevelSecurity = Command("DeleteTablePolicyRowLevelSecurity", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("policy"),
                    RequiredEToken("row_level_security"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var DeleteTableRecords = Command("DeleteTableRecords", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    Required(
                        First(
                            Custom(
                                EToken("async"),
                                RequiredEToken("table")),
                            EToken("table")),
                        () => (SyntaxElement)new CustomNode(CreateMissingEToken("async"), CreateMissingEToken("table"))),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredEToken("records"),
                    Required(
                        First(
                            Custom(
                                Custom(
                                    EToken("<|"),
                                    Required(rules.CommandInput, rules.MissingExpression),
                                    new [] {CD(), CD(CompletionHint.Tabular)}),
                                CD("csl")),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                Required(
                                    Custom(
                                        EToken("<|"),
                                        Required(rules.CommandInput, rules.MissingExpression),
                                        new [] {CD(), CD(CompletionHint.Tabular)}),
                                    () => (SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Tabular)}, CreateMissingEToken("<|"), rules.MissingExpression())),
                                new [] {CD(), CD(), CD(CompletionHint.None), CD(), CD("csl")})),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Tabular)}, CreateMissingEToken("<|"), rules.MissingExpression())),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var DetachDatabase = Command("DetachDatabase", 
                Custom(
                    EToken("detach", CompletionKind.CommandPrefix),
                    EToken("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database)}));

            var DisableContinuousExport = Command("DisableContinuousExport", 
                Custom(
                    EToken("disable", CompletionKind.CommandPrefix),
                    EToken("continuous-export"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    new [] {CD(), CD(), CD("ContinousExportName", CompletionHint.None)}));

            var DisableDatabaseMaintenanceMode = Command("DisableDatabaseMaintenanceMode", 
                Custom(
                    EToken("disable", CompletionKind.CommandPrefix),
                    EToken("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredEToken("maintenance_mode"),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD()}));

            var DisablePlugin = Command("DisablePlugin", 
                Custom(
                    EToken("disable", CompletionKind.CommandPrefix),
                    RequiredEToken("plugin"),
                    Required(
                        First(
                            rules.StringLiteral,
                            rules.NameDeclarationOrStringLiteral),
                        rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("pluginName", CompletionHint.Literal)}));

            var DropPretendExtentsByProperties = Command("DropPretendExtentsByProperties", 
                Custom(
                    EToken("drop-pretend", CompletionKind.CommandPrefix),
                    RequiredEToken("extents"),
                    Required(
                        First(
                            EToken("from"),
                            Custom(
                                EToken("older"),
                                Required(rules.Value, rules.MissingValue),
                                RequiredEToken("days", "hours"),
                                RequiredEToken("from"),
                                new [] {CD(), CD("Older", CompletionHint.Literal), CD(), CD()})),
                        () => CreateMissingEToken("from")),
                    Required(
                        First(
                            Custom(
                                EToken("all"),
                                RequiredEToken("tables")),
                            Custom(
                                If(Not(EToken("all")), rules.TableNameReference),
                                CD("TableName", CompletionHint.Table))),
                        () => (SyntaxElement)new CustomNode(CreateMissingEToken("all"), CreateMissingEToken("tables"))),
                    Optional(
                        First(
                            Custom(
                                EToken("limit"),
                                Required(rules.Value, rules.MissingValue),
                                new [] {CD(), CD("LimitCount", CompletionHint.Literal)}),
                            Custom(
                                EToken("trim"),
                                RequiredEToken("by"),
                                RequiredEToken("datasize", "extentsize"),
                                Required(rules.Value, rules.MissingValue),
                                RequiredEToken("bytes", "GB", "MB"),
                                Optional(
                                    Custom(
                                        EToken("limit"),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD(), CD("LimitCount", CompletionHint.Literal)})),
                                new [] {CD(), CD(), CD(), CD("TrimSize", CompletionHint.Literal), CD(), CD(isOptional: true)}))),
                    new [] {CD(), CD(), CD(), CD(), CD(isOptional: true)}));

            var DropBasicAuthUser = Command("DropBasicAuthUser", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("basicauth"),
                    RequiredEToken("user"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD("UserName", CompletionHint.Literal)}));

            var DropClusterBlockedPrincipals = Command("DropClusterBlockedPrincipals", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("blockedprincipals"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        First(
                            Custom(
                                EToken("application"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                Optional(
                                    Custom(
                                        EToken("user"),
                                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                                        new [] {CD(), CD("UserName", CompletionHint.Literal)})),
                                new [] {CD(), CD("AppName", CompletionHint.Literal), CD(isOptional: true)}),
                            Custom(
                                EToken("user"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                new [] {CD(), CD("UserName", CompletionHint.Literal)}))),
                    new [] {CD(), CD(), CD(), CD("Principal", CompletionHint.Literal), CD(isOptional: true)}));

            var DropClusterRole = Command("DropClusterRole", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    RequiredEToken("admins", "databasecreators", "users", "viewers"),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                CD("Principal", CompletionHint.Literal)),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredEToken(")"),
                    Optional(
                        First(
                            Custom(
                                EToken("skip-results"),
                                Optional(
                                    Custom(
                                        rules.StringLiteral,
                                        CD("Notes", CompletionHint.Literal))),
                                new [] {CD("SkipResults"), CD(CompletionHint.Literal, isOptional: true)}),
                            Custom(
                                rules.StringLiteral,
                                CD("Notes", CompletionHint.Literal)))),
                    new [] {CD(), CD(), CD("Role"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var DropColumn = Command("DropColumn", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("column"),
                    Required(rules.TableColumnNameReference, rules.MissingNameReference),
                    new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column)}));

            var DropContinuousExport = Command("DropContinuousExport", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("continuous-export"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    new [] {CD(), CD(), CD("ContinuousExportName", CompletionHint.None)}));

            var DropDatabaseIngestionMapping = Command("DropDatabaseIngestionMapping", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("ingestion"),
                    RequiredEToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredEToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD("MappingKind"), CD(), CD("MappingName", CompletionHint.Literal)}));

            var DropDatabasePrettyName = Command("DropDatabasePrettyName", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("prettyname"),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD()}));

            var DropDatabaseRole = Command("DropDatabaseRole", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredEToken("admins", "ingestors", "monitors", "unrestrictedviewers", "users", "viewers"),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                CD("Principal", CompletionHint.Literal)),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredEToken(")"),
                    Optional(
                        First(
                            Custom(
                                EToken("skip-results"),
                                Optional(
                                    Custom(
                                        rules.StringLiteral,
                                        CD("Notes", CompletionHint.Literal))),
                                new [] {CD("SkipResults"), CD(CompletionHint.Literal, isOptional: true)}),
                            Custom(
                                rules.StringLiteral,
                                CD("Notes", CompletionHint.Literal)))),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD("Role"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var DropEmptyExtentContainers = Command("DropEmptyExtentContainers", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            EToken("async"),
                            EToken("empty")),
                        Custom(
                            EToken("async"),
                            RequiredEToken("empty")),
                        EToken("empty")),
                    RequiredEToken("extentcontainers"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredEToken("until"),
                    RequiredEToken("="),
                    Required(rules.Value, rules.MissingValue),
                    Optional(
                        First(
                            Custom(
                                EToken("whatif"),
                                Optional(
                                    Custom(
                                        EToken("with"),
                                        RequiredEToken("("),
                                        Required(
                                            OList(
                                                primaryElementParser: Custom(
                                                    rules.NameDeclarationOrStringLiteral,
                                                    RequiredEToken("="),
                                                    Required(rules.Value, rules.MissingValue),
                                                    new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                                separatorParser: EToken(","),
                                                secondaryElementParser: null,
                                                missingPrimaryElement: null,
                                                missingSeparator: null,
                                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                                endOfList: null,
                                                oneOrMore: true,
                                                allowTrailingSeparator: false,
                                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                        RequiredEToken(")"),
                                        new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                                new [] {CD(), CD(isOptional: true)}),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                new [] {CD(), CD(), CD(CompletionHint.None), CD()}))),
                    new [] {CD(), CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(), CD("d", CompletionHint.Literal), CD(isOptional: true)}));

            var DropExtentsPartitionMetadata = Command("DropExtentsPartitionMetadata", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("extents"),
                    EToken("partition"),
                    RequiredEToken("metadata"),
                    RequiredEToken("from"),
                    RequiredEToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Required(
                        First(
                            Custom(
                                Custom(
                                    EToken("<|"),
                                    Required(rules.CommandInput, rules.MissingExpression),
                                    new [] {CD(), CD(CompletionHint.Tabular)}),
                                CD("csl")),
                            Custom(
                                EToken("between"),
                                RequiredEToken("("),
                                Required(rules.Value, rules.MissingValue),
                                RequiredEToken(".."),
                                Required(rules.Value, rules.MissingValue),
                                RequiredEToken(")"),
                                Required(
                                    Custom(
                                        EToken("<|"),
                                        Required(rules.CommandInput, rules.MissingExpression),
                                        new [] {CD(), CD(CompletionHint.Tabular)}),
                                    () => (SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Tabular)}, CreateMissingEToken("<|"), rules.MissingExpression())),
                                new [] {CD(), CD(), CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal), CD(), CD("csl")})),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Tabular)}, CreateMissingEToken("<|"), rules.MissingExpression())),
                    new [] {CD(), CD(), CD(), CD(), CD(), CD(), CD("TableName", CompletionHint.Table), CD()}));

            var DropExtents = Command("DropExtents", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("extents"),
                    Required(
                        First(
                            Custom(
                                EToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.AnyGuidLiteralOrString,
                                            CD("ExtentId", CompletionHint.Literal)),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingValue,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingValue()))),
                                RequiredEToken(")"),
                                Optional(
                                    Custom(
                                        EToken("from"),
                                        Required(rules.TableNameReference, rules.MissingNameReference),
                                        new [] {CD(), CD("TableName", CompletionHint.Table)})),
                                new [] {CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}),
                            Custom(
                                EToken("<|"),
                                Required(rules.CommandInput, rules.MissingExpression),
                                new [] {CD(), CD("Query", CompletionHint.Tabular)}),
                            Custom(
                                EToken("from"),
                                Required(
                                    First(
                                        Custom(
                                            EToken("all"),
                                            RequiredEToken("tables")),
                                        Custom(
                                            If(Not(EToken("all")), rules.TableNameReference),
                                            CD("TableName", CompletionHint.Table))),
                                    () => (SyntaxElement)new CustomNode(CreateMissingEToken("all"), CreateMissingEToken("tables"))),
                                Optional(
                                    First(
                                        Custom(
                                            EToken("limit"),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD(), CD("LimitCount", CompletionHint.Literal)}),
                                        Custom(
                                            EToken("trim"),
                                            RequiredEToken("by"),
                                            RequiredEToken("datasize", "extentsize"),
                                            Required(rules.Value, rules.MissingValue),
                                            RequiredEToken("bytes", "GB", "MB"),
                                            Optional(
                                                Custom(
                                                    EToken("limit"),
                                                    Required(rules.Value, rules.MissingValue),
                                                    new [] {CD(), CD("LimitCount", CompletionHint.Literal)})),
                                            new [] {CD(), CD(), CD(), CD("TrimSize", CompletionHint.Literal), CD(), CD(isOptional: true)}))),
                                new [] {CD(), CD(), CD(isOptional: true)}),
                            Custom(
                                EToken("older"),
                                Required(rules.Value, rules.MissingValue),
                                RequiredEToken("days", "hours"),
                                RequiredEToken("from"),
                                Required(
                                    First(
                                        Custom(
                                            EToken("all"),
                                            RequiredEToken("tables")),
                                        Custom(
                                            If(Not(EToken("all")), rules.TableNameReference),
                                            CD("TableName", CompletionHint.Table))),
                                    () => (SyntaxElement)new CustomNode(CreateMissingEToken("all"), CreateMissingEToken("tables"))),
                                Optional(
                                    First(
                                        Custom(
                                            EToken("limit"),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD(), CD("LimitCount", CompletionHint.Literal)}),
                                        Custom(
                                            EToken("trim"),
                                            RequiredEToken("by"),
                                            RequiredEToken("datasize", "extentsize"),
                                            Required(rules.Value, rules.MissingValue),
                                            RequiredEToken("bytes", "GB", "MB"),
                                            Optional(
                                                Custom(
                                                    EToken("limit"),
                                                    Required(rules.Value, rules.MissingValue),
                                                    new [] {CD(), CD("LimitCount", CompletionHint.Literal)})),
                                            new [] {CD(), CD(), CD(), CD("TrimSize", CompletionHint.Literal), CD(), CD(isOptional: true)}))),
                                new [] {CD(), CD("Older", CompletionHint.Literal), CD(), CD(), CD(), CD(isOptional: true)}),
                            Custom(
                                EToken("whatif"),
                                RequiredEToken("<|"),
                                Required(rules.CommandInput, rules.MissingExpression),
                                new [] {CD(), CD(), CD("Query", CompletionHint.Tabular)})),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}, CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingValue())), CreateMissingEToken(")"), (SyntaxElement)new CustomNode(new [] {CD(), CD("TableName", CompletionHint.Table)}, CreateMissingEToken("from"), rules.MissingNameReference())))));

            var DropExtentTagsRetention = Command("DropExtentTagsRetention", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("extent"),
                    EToken("tags"),
                    RequiredEToken("retention")));

            var DropExtent = Command("DropExtent", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("extent"),
                    Required(rules.AnyGuidLiteralOrString, rules.MissingValue),
                    Optional(
                        Custom(
                            EToken("from"),
                            Required(rules.TableNameReference, rules.MissingNameReference),
                            new [] {CD(), CD("TableName", CompletionHint.Table)})),
                    new [] {CD(), CD(), CD("ExtentId", CompletionHint.Literal), CD(isOptional: true)}));

            var DropExternalTableAdmins = Command("DropExternalTableAdmins", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("external"),
                    EToken("table"),
                    rules.ExternalTableNameReference,
                    EToken("admins"),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                CD("principal", CompletionHint.Literal)),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredEToken(")"),
                    Optional(
                        First(
                            Custom(
                                EToken("skip-results"),
                                Optional(
                                    Custom(
                                        rules.StringLiteral,
                                        CD("notes", CompletionHint.Literal))),
                                new [] {CD(), CD(CompletionHint.Literal, isOptional: true)}),
                            Custom(
                                rules.StringLiteral,
                                CD("notes", CompletionHint.Literal)))),
                    new [] {CD(), CD(), CD(), CD("externalTableName", CompletionHint.ExternalTable), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var DropExternalTableMapping = Command("DropExternalTableMapping", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("external"),
                    EToken("table"),
                    rules.ExternalTableNameReference,
                    EToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD("MappingName", CompletionHint.Literal)}));

            var DropExternalTable = Command("DropExternalTable", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("external"),
                    RequiredEToken("table"),
                    Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                    new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable)}));

            var DropFollowerDatabases = Command("DropFollowerDatabases", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("follower"),
                    First(
                        Custom(
                            EToken("databases"),
                            EToken("("),
                            OList(
                                primaryElementParser: Custom(
                                    rules.DatabaseNameReference,
                                    CD("databaseName", CompletionHint.Database)),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingNameReference,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            EToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.Database), CD()}),
                        Custom(
                            EToken("databases"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.DatabaseNameReference,
                                        CD("databaseName", CompletionHint.Database)),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: rules.MissingNameReference,
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameReference()))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.Database), CD()}),
                        Custom(
                            EToken("database"),
                            rules.DatabaseNameReference,
                            new [] {CD(), CD("databaseName", CompletionHint.Database)})),
                    RequiredEToken("from"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("leaderClusterMetadataPath", CompletionHint.Literal)}));

            var DropFollowerDatabaseAuthorizedPrincipals = Command("DropFollowerDatabaseAuthorizedPrincipals", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("follower"),
                    RequiredEToken("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredEToken("admins", "monitors", "unrestrictedviewers", "users", "viewers"),
                    Required(
                        First(
                            EToken("("),
                            Custom(
                                EToken("from"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                RequiredEToken("("),
                                new [] {CD(), CD("leaderClusterMetadataPath", CompletionHint.Literal), CD()})),
                        () => CreateMissingEToken("(")),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                CD("principal", CompletionHint.Literal)),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredEToken(")"),
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD("operationRole"), CD(), CD(CompletionHint.Literal), CD()}));

            var DropFunctions = Command("DropFunctions", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("functions"),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.DatabaseFunctionNameReference,
                                CD("FunctionName", CompletionHint.Function)),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingNameReference,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameReference()))),
                    RequiredEToken(")"),
                    Optional(EToken("ifexists")),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Function), CD(), CD(isOptional: true)}));

            var DropFunctionRole = Command("DropFunctionRole", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("function"),
                    rules.DatabaseFunctionNameReference,
                    EToken("admins"),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                CD("Principal", CompletionHint.Literal)),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredEToken(")"),
                    Optional(
                        First(
                            Custom(
                                EToken("skip-results"),
                                Optional(
                                    Custom(
                                        rules.StringLiteral,
                                        CD("Notes", CompletionHint.Literal))),
                                new [] {CD("SkipResults"), CD(CompletionHint.Literal, isOptional: true)}),
                            Custom(
                                rules.StringLiteral,
                                CD("Notes", CompletionHint.Literal)))),
                    new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD("Role"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var DropFunction = Command("DropFunction", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("function"),
                    Required(rules.DatabaseFunctionNameReference, rules.MissingNameReference),
                    Optional(EToken("ifexists")),
                    new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD(isOptional: true)}));

            var DropMaterializedViewAdmins = Command("DropMaterializedViewAdmins", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("admins"),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                CD("principal", CompletionHint.Literal)),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredEToken(")"),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            CD("notes", CompletionHint.Literal))),
                    new [] {CD(), CD(), CD("materializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(CompletionHint.Literal, isOptional: true)}));

            var DropMaterializedView = Command("DropMaterializedView", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView)}));

            var DropRowStore = Command("DropRowStore", 
                Custom(
                    EToken("detach", "drop"),
                    EToken("rowstore"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Optional(EToken("ifexists")),
                    new [] {CD(), CD(), CD("rowStoreName", CompletionHint.None), CD(isOptional: true)}));

            var StoredQueryResultsDrop = Command("StoredQueryResultsDrop", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("stored_query_results"),
                    RequiredEToken("by"),
                    RequiredEToken("user"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("Principal", CompletionHint.Literal)}));

            var StoredQueryResultDrop = Command("StoredQueryResultDrop", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("stored_query_result"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    new [] {CD(), CD(), CD("StoredQueryResultName", CompletionHint.None)}));

            var DropStoredQueryResultContainers = Command("DropStoredQueryResultContainers", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("storedqueryresultcontainers"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    Required(
                        List(
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                CD("containerId", CompletionHint.Literal)),
                            missingElement: null,
                            oneOrMore: true,
                            producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray())),
                        () => new SyntaxList<SyntaxElement>(rules.MissingValue())),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(CompletionHint.Literal)}));

            var DropTables = Command("DropTables", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("tables"),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.TableNameReference,
                                CD("TableName", CompletionHint.Table)),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingNameReference,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameReference()))),
                    RequiredEToken(")"),
                    Optional(EToken("ifexists")),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(isOptional: true)}));

            var DropTableColumns = Command("DropTableColumns", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("columns"),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.ColumnNameReference,
                                CD("ColumnName", CompletionHint.Column)),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingNameReference,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameReference()))),
                    RequiredEToken(")"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(CompletionHint.Column), CD()}));

            var DropTableIngestionMapping = Command("DropTableIngestionMapping", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("ingestion"),
                    RequiredEToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredEToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("MappingKind"), CD(), CD("MappingName", CompletionHint.Literal)}));

            var DropTableRole = Command("DropTableRole", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("admins", "ingestors"),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                CD("Principal", CompletionHint.Literal)),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredEToken(")"),
                    Optional(
                        First(
                            Custom(
                                EToken("skip-results"),
                                Optional(
                                    Custom(
                                        rules.StringLiteral,
                                        CD("Notes", CompletionHint.Literal))),
                                new [] {CD("SkipResults"), CD(CompletionHint.Literal, isOptional: true)}),
                            Custom(
                                rules.StringLiteral,
                                CD("Notes", CompletionHint.Literal)))),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD("Role"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var DropTable = Command("DropTable", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Optional(EToken("ifexists")),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(isOptional: true)}));

            var DropTempStorage = Command("DropTempStorage", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("tempstorage"),
                    RequiredEToken("older"),
                    Required(rules.Value, rules.MissingValue),
                    new [] {CD(), CD(), CD(), CD("olderThan", CompletionHint.Literal)}));

            var DropUnusedStoredQueryResultContainers = Command("DropUnusedStoredQueryResultContainers", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("unused"),
                    RequiredEToken("storedqueryresultcontainers"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    new [] {CD(), CD(), CD(), CD("databaseName", CompletionHint.Database)}));

            var DropWorkloadGroup = Command("DropWorkloadGroup", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    RequiredEToken("workload_group"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    new [] {CD(), CD(), CD("WorkloadGroupName", CompletionHint.None)}));

            var EnableContinuousExport = Command("EnableContinuousExport", 
                Custom(
                    EToken("enable", CompletionKind.CommandPrefix),
                    EToken("continuous-export"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    new [] {CD(), CD(), CD("ContinuousExportName", CompletionHint.None)}));

            var EnableDatabaseMaintenanceMode = Command("EnableDatabaseMaintenanceMode", 
                Custom(
                    EToken("enable", CompletionKind.CommandPrefix),
                    EToken("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredEToken("maintenance_mode"),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD()}));

            var EnableDisableMaterializedView = Command("EnableDisableMaterializedView", 
                Custom(
                    EToken("disable", "enable"),
                    EToken("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView)}));

            var EnablePlugin = Command("EnablePlugin", 
                Custom(
                    EToken("enable", CompletionKind.CommandPrefix),
                    RequiredEToken("plugin"),
                    Required(
                        First(
                            rules.StringLiteral,
                            rules.NameDeclarationOrStringLiteral),
                        rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("name", CompletionHint.Literal)}));

            var ExportToSqlTable = Command("ExportToSqlTable", 
                Custom(
                    EToken("export", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            EToken("async"),
                            EToken("to")),
                        EToken("to")),
                    EToken("sql"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(
                        First(
                            EToken("<|"),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("<|"),
                                new [] {CD(), CD(), CD(CompletionHint.None), CD(), CD()})),
                        () => CreateMissingEToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    new [] {CD(), CD(), CD(), CD("SqlTableName", CompletionHint.None), CD("SqlConnectionString", CompletionHint.Literal), CD(), CD("Query", CompletionHint.Tabular)}));

            var ExportToExternalTable = Command("ExportToExternalTable", 
                Custom(
                    EToken("export", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            EToken("async"),
                            EToken("to")),
                        EToken("to")),
                    EToken("table"),
                    Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                    Required(
                        First(
                            EToken("<|"),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("<|"),
                                new [] {CD(), CD(), CD(CompletionHint.None), CD(), CD()})),
                        () => CreateMissingEToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD("Query", CompletionHint.Tabular)}));

            var ExportToStorage = Command("ExportToStorage", 
                Custom(
                    EToken("export", CompletionKind.CommandPrefix),
                    Optional(
                        First(
                            Custom(
                                EToken("async"),
                                Optional(EToken("compressed")),
                                new [] {CD(), CD(isOptional: true)}),
                            EToken("compressed"))),
                    RequiredEToken("to"),
                    RequiredEToken("csv", "json", "parquet", "tsv"),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                CD("DataConnectionString", CompletionHint.Literal)),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredEToken(")"),
                    Required(
                        First(
                            EToken("<|"),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("<|"),
                                new [] {CD(), CD(), CD(CompletionHint.None), CD(), CD()})),
                        () => CreateMissingEToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    new [] {CD(), CD(isOptional: true), CD(), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(), CD("Query", CompletionHint.Tabular)}));

            var IngestInlineIntoTable = Command("IngestInlineIntoTable", 
                Custom(
                    EToken("ingest", CompletionKind.CommandPrefix),
                    EToken("inline"),
                    RequiredEToken("into"),
                    RequiredEToken("table"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Required(
                        First(
                            Custom(
                                EToken("["),
                                Required(rules.BracketedInputText, rules.MissingInputText),
                                RequiredEToken("]"),
                                new [] {CD(), CD("Data", CompletionHint.None), CD()}),
                            Custom(
                                EToken("<|"),
                                Required(rules.InputText, rules.MissingInputText),
                                new [] {CD(), CD("Data", CompletionHint.None)}),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            First(
                                                EToken("creationTime"),
                                                EToken("distributed"),
                                                EToken("docstring"),
                                                EToken("extend_schema"),
                                                EToken("folder"),
                                                EToken("format"),
                                                EToken("ignoreFirstRecord"),
                                                EToken("ingestIfNotExists"),
                                                EToken("ingestionMappingReference"),
                                                EToken("ingestionMapping"),
                                                EToken("persistDetails"),
                                                EToken("policy_ingestionTime"),
                                                EToken("recreate_schema"),
                                                EToken("tags"),
                                                EToken("validationPolicy"),
                                                EToken("zipPattern"),
                                                If(Not(And(EToken("creationTime", "distributed", "docstring", "extend_schema", "folder", "format", "ignoreFirstRecord", "ingestIfNotExists", "ingestionMappingReference", "ingestionMapping", "persistDetails", "policy_ingestionTime", "recreate_schema", "tags", "validationPolicy", "zipPattern"))), rules.NameDeclarationOrStringLiteral)),
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("<|"),
                                Required(rules.InputText, rules.MissingInputText),
                                new [] {CD(), CD(), CD(), CD(), CD(), CD("Data", CompletionHint.None)})),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD("Data", CompletionHint.None), CD()}, CreateMissingEToken("["), rules.MissingInputText(), CreateMissingEToken("]"))),
                    new [] {CD(), CD(), CD(), CD(), CD("TableName", CompletionHint.None), CD()}));

            var IngestIntoTable = Command("IngestIntoTable", 
                Custom(
                    EToken("ingest", CompletionKind.CommandPrefix),
                    Required(
                        First(
                            Custom(
                                EToken("async"),
                                RequiredEToken("into")),
                            EToken("into")),
                        () => (SyntaxElement)new CustomNode(CreateMissingEToken("async"), CreateMissingEToken("into"))),
                    RequiredEToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Required(
                        First(
                            Custom(
                                EToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.StringLiteral,
                                            CD("Path", CompletionHint.Literal)),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingStringLiteral,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                                RequiredEToken(")"),
                                new [] {CD(), CD(CompletionHint.Literal), CD()}),
                            Custom(
                                rules.StringLiteral,
                                CD("Path", CompletionHint.Literal))),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Literal), CD()}, CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral())), CreateMissingEToken(")"))),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        First(
                                            EToken("creationTime"),
                                            EToken("distributed"),
                                            EToken("docstring"),
                                            EToken("extend_schema"),
                                            EToken("folder"),
                                            EToken("format"),
                                            EToken("ignoreFirstRecord"),
                                            EToken("ingestIfNotExists"),
                                            EToken("ingestionMappingReference"),
                                            EToken("ingestionMapping"),
                                            EToken("persistDetails"),
                                            EToken("policy_ingestionTime"),
                                            EToken("recreate_schema"),
                                            EToken("tags"),
                                            EToken("validationPolicy"),
                                            EToken("zipPattern"),
                                            If(Not(And(EToken("creationTime", "distributed", "docstring", "extend_schema", "folder", "format", "ignoreFirstRecord", "ingestIfNotExists", "ingestionMappingReference", "ingestionMapping", "persistDetails", "policy_ingestionTime", "recreate_schema", "tags", "validationPolicy", "zipPattern"))), rules.NameDeclarationOrStringLiteral)),
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"))),
                    new [] {CD(), CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(isOptional: true)}));

            var MergeExtentsDryrun = Command("MergeExtentsDryrun", 
                Custom(
                    EToken("merge", CompletionKind.CommandPrefix),
                    EToken("dryrun"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.AnyGuidLiteralOrString,
                                CD("GUID", CompletionHint.Literal)),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingValue,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingValue()))),
                    RequiredEToken(")"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(CompletionHint.Literal), CD()}));

            var MergeExtents = Command("MergeExtents", 
                Custom(
                    EToken("merge", CompletionKind.CommandPrefix),
                    Required(
                        First(
                            Custom(
                                EToken("async"),
                                Required(rules.TableNameReference, rules.MissingNameReference),
                                new [] {CD(), CD("TableName", CompletionHint.Table)}),
                            Custom(
                                If(Not(And(EToken("dryrun", "async"))), rules.TableNameReference),
                                CD("TableName", CompletionHint.Table))),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD("TableName", CompletionHint.Table)}, CreateMissingEToken("async"), rules.MissingNameReference())),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.AnyGuidLiteralOrString,
                                CD("GUID", CompletionHint.Literal)),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingValue,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingValue()))),
                    RequiredEToken(")"),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            RequiredEToken("rebuild"),
                            RequiredEToken("="),
                            RequiredEToken("true"),
                            RequiredEToken(")"))),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var MoveExtentsFrom = Command("MoveExtentsFrom", 
                Custom(
                    EToken("move", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            EToken("async"),
                            EToken("extents")),
                        EToken("extents")),
                    First(
                        Custom(
                            EToken("("),
                            OList(
                                primaryElementParser: Custom(
                                    rules.AnyGuidLiteralOrString,
                                    CD("GUID", CompletionHint.Literal)),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingValue,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            EToken(")"),
                            new [] {CD(), CD(CompletionHint.Literal), CD()}),
                        Custom(
                            EToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.AnyGuidLiteralOrString,
                                        CD("GUID", CompletionHint.Literal)),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: rules.MissingValue,
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingValue()))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(CompletionHint.Literal), CD()}),
                        EToken("all")),
                    RequiredEToken("from"),
                    RequiredEToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredEToken("to"),
                    RequiredEToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    new [] {CD(), CD(), CD(), CD(), CD(), CD("SourceTableName", CompletionHint.Table), CD(), CD(), CD("DestinationTableName", CompletionHint.Table)}));

            var MoveExtentsQuery = Command("MoveExtentsQuery", 
                Custom(
                    EToken("move", CompletionKind.CommandPrefix),
                    Required(
                        First(
                            Custom(
                                EToken("async"),
                                RequiredEToken("extents")),
                            EToken("extents")),
                        () => (SyntaxElement)new CustomNode(CreateMissingEToken("async"), CreateMissingEToken("extents"))),
                    RequiredEToken("to"),
                    RequiredEToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredEToken("<|"),
                    Required(rules.CommandInput, rules.MissingExpression),
                    new [] {CD(), CD(), CD(), CD(), CD("DestinationTableName", CompletionHint.Table), CD(), CD("Query", CompletionHint.Tabular)}));

            var RenameColumns = Command("RenameColumns", 
                Custom(
                    EToken("rename", CompletionKind.CommandPrefix),
                    EToken("columns"),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.NameDeclarationOrStringLiteral,
                                RequiredEToken("="),
                                Required(rules.DatabaseTableColumnNameReference, rules.MissingNameReference),
                                new [] {CD("NewColumnName", CompletionHint.None), CD(), CD("ColumnName", CompletionHint.Column)}),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("NewColumnName", CompletionHint.None), CD(), CD("ColumnName", CompletionHint.Column)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingNameReference()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("NewColumnName", CompletionHint.None), CD(), CD("ColumnName", CompletionHint.Column)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingNameReference())))),
                    new [] {CD(), CD(), CD(CompletionHint.None)}));

            var RenameColumn = Command("RenameColumn", 
                Custom(
                    EToken("rename", CompletionKind.CommandPrefix),
                    EToken("column"),
                    Required(rules.DatabaseTableColumnNameReference, rules.MissingNameReference),
                    RequiredEToken("to"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD("NewColumnName", CompletionHint.None)}));

            var RenameMaterializedView = Command("RenameMaterializedView", 
                Custom(
                    EToken("rename", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    RequiredEToken("to"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD("NewMaterializedViewName", CompletionHint.None)}));

            var RenameTables = Command("RenameTables", 
                Custom(
                    EToken("rename", CompletionKind.CommandPrefix),
                    EToken("tables"),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.NameDeclarationOrStringLiteral,
                                RequiredEToken("="),
                                Required(rules.TableNameReference, rules.MissingNameReference),
                                new [] {CD("NewTableName", CompletionHint.None), CD(), CD("TableName", CompletionHint.Table)}),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("NewTableName", CompletionHint.None), CD(), CD("TableName", CompletionHint.Table)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingNameReference()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("NewTableName", CompletionHint.None), CD(), CD("TableName", CompletionHint.Table)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingNameReference())))),
                    new [] {CD(), CD(), CD(CompletionHint.None)}));

            var RenameTable = Command("RenameTable", 
                Custom(
                    EToken("rename", CompletionKind.CommandPrefix),
                    RequiredEToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredEToken("to"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("NewTableName", CompletionHint.None)}));

            var ReplaceExtents = Command("ReplaceExtents", 
                Custom(
                    new Parser<LexicalToken>[] {
                        EToken("replace", CompletionKind.CommandPrefix),
                        Required(
                            First(
                                Custom(
                                    EToken("async"),
                                    RequiredEToken("extents")),
                                EToken("extents")),
                            () => (SyntaxElement)new CustomNode(CreateMissingEToken("async"), CreateMissingEToken("extents"))),
                        RequiredEToken("in"),
                        RequiredEToken("table"),
                        Required(rules.TableNameReference, rules.MissingNameReference),
                        RequiredEToken("<|"),
                        RequiredEToken("{"),
                        Required(rules.CommandInput, rules.MissingExpression),
                        RequiredEToken("}"),
                        RequiredEToken(","),
                        RequiredEToken("{"),
                        Required(rules.CommandInput, rules.MissingExpression),
                        RequiredEToken("}")}
                    ,
                    new [] {CD(), CD(), CD(), CD(), CD("DestinationTableName", CompletionHint.Table), CD(), CD(), CD("ExtentsToDropQuery", CompletionHint.Tabular), CD(), CD(), CD(), CD("ExtentsToMoveQuery", CompletionHint.Tabular), CD()}));

            var SetOrAppendTable = Command("SetOrAppendTable", 
                Custom(
                    EToken("set-or-append", CompletionKind.CommandPrefix),
                    Required(
                        First(
                            Custom(
                                EToken("async"),
                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                new [] {CD(), CD("TableName", CompletionHint.None)}),
                            Custom(
                                If(Not(EToken("async")), rules.NameDeclarationOrStringLiteral),
                                CD("TableName", CompletionHint.None))),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD("TableName", CompletionHint.None)}, CreateMissingEToken("async"), rules.MissingNameDeclaration())),
                    Required(
                        First(
                            EToken("<|"),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            First(
                                                EToken("creationTime"),
                                                EToken("distributed"),
                                                EToken("docstring"),
                                                EToken("extend_schema"),
                                                EToken("folder"),
                                                EToken("format"),
                                                EToken("ignoreFirstRecord"),
                                                EToken("ingestIfNotExists"),
                                                EToken("ingestionMappingReference"),
                                                EToken("ingestionMapping"),
                                                EToken("persistDetails"),
                                                EToken("policy_ingestionTime"),
                                                EToken("recreate_schema"),
                                                EToken("tags"),
                                                EToken("validationPolicy"),
                                                EToken("zipPattern"),
                                                If(Not(And(EToken("creationTime", "distributed", "docstring", "extend_schema", "folder", "format", "ignoreFirstRecord", "ingestIfNotExists", "ingestionMappingReference", "ingestionMapping", "persistDetails", "policy_ingestionTime", "recreate_schema", "tags", "validationPolicy", "zipPattern"))), rules.NameDeclarationOrStringLiteral)),
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("<|"))),
                        () => CreateMissingEToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    new [] {CD(), CD(), CD(), CD("QueryOrCommand", CompletionHint.Tabular)}));

            var StoredQueryResultSetOrReplace = Command("StoredQueryResultSetOrReplace", 
                Custom(
                    EToken("set-or-replace", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            EToken("async"),
                            EToken("stored_query_result")),
                        EToken("stored_query_result")),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Required(
                        First(
                            EToken("<|"),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            First(
                                                EToken("creationTime"),
                                                EToken("distributed"),
                                                EToken("docstring"),
                                                EToken("extend_schema"),
                                                EToken("folder"),
                                                EToken("format"),
                                                EToken("ignoreFirstRecord"),
                                                EToken("ingestIfNotExists"),
                                                EToken("ingestionMappingReference"),
                                                EToken("ingestionMapping"),
                                                EToken("persistDetails"),
                                                EToken("policy_ingestionTime"),
                                                EToken("recreate_schema"),
                                                EToken("tags"),
                                                EToken("validationPolicy"),
                                                EToken("zipPattern"),
                                                If(Not(And(EToken("creationTime", "distributed", "docstring", "extend_schema", "folder", "format", "ignoreFirstRecord", "ingestIfNotExists", "ingestionMappingReference", "ingestionMapping", "persistDetails", "policy_ingestionTime", "recreate_schema", "tags", "validationPolicy", "zipPattern"))), rules.NameDeclarationOrStringLiteral)),
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("<|"))),
                        () => CreateMissingEToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    new [] {CD(), CD(), CD("StoredQueryResultName", CompletionHint.None), CD(), CD("Query", CompletionHint.Tabular)}));

            var SetOrReplaceTable = Command("SetOrReplaceTable", 
                Custom(
                    EToken("set-or-replace", CompletionKind.CommandPrefix),
                    Required(
                        First(
                            Custom(
                                EToken("async"),
                                Required(If(Not(EToken("stored_query_result")), rules.NameDeclarationOrStringLiteral), rules.MissingNameDeclaration),
                                new [] {CD(), CD("TableName", CompletionHint.None)}),
                            Custom(
                                If(Not(And(EToken("async", "stored_query_result"))), rules.NameDeclarationOrStringLiteral),
                                CD("TableName", CompletionHint.None))),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD("TableName", CompletionHint.None)}, CreateMissingEToken("async"), rules.MissingNameDeclaration())),
                    Required(
                        First(
                            EToken("<|"),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            First(
                                                EToken("creationTime"),
                                                EToken("distributed"),
                                                EToken("docstring"),
                                                EToken("extend_schema"),
                                                EToken("folder"),
                                                EToken("format"),
                                                EToken("ignoreFirstRecord"),
                                                EToken("ingestIfNotExists"),
                                                EToken("ingestionMappingReference"),
                                                EToken("ingestionMapping"),
                                                EToken("persistDetails"),
                                                EToken("policy_ingestionTime"),
                                                EToken("recreate_schema"),
                                                EToken("tags"),
                                                EToken("validationPolicy"),
                                                EToken("zipPattern"),
                                                If(Not(And(EToken("creationTime", "distributed", "docstring", "extend_schema", "folder", "format", "ignoreFirstRecord", "ingestIfNotExists", "ingestionMappingReference", "ingestionMapping", "persistDetails", "policy_ingestionTime", "recreate_schema", "tags", "validationPolicy", "zipPattern"))), rules.NameDeclarationOrStringLiteral)),
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("<|"))),
                        () => CreateMissingEToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    new [] {CD(), CD(), CD(), CD("QueryOrCommand", CompletionHint.Tabular)}));

            var SetAccess = Command("SetAccess", 
                Custom(
                    EToken("set", CompletionKind.CommandPrefix),
                    EToken("access"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredEToken("to"),
                    RequiredEToken("readonly", "readwrite"),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD("AccessMode")}));

            var SetClusterRole = Command("SetClusterRole", 
                Custom(
                    EToken("set", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    RequiredEToken("admins", "databasecreators", "users", "viewers"),
                    Required(
                        First(
                            Custom(
                                EToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.StringLiteral,
                                            CD("Principal", CompletionHint.Literal)),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingStringLiteral,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                                RequiredEToken(")"),
                                Optional(
                                    First(
                                        Custom(
                                            EToken("skip-results"),
                                            Optional(
                                                Custom(
                                                    rules.StringLiteral,
                                                    CD("Notes", CompletionHint.Literal))),
                                            new [] {CD("SkipResults"), CD(CompletionHint.Literal, isOptional: true)}),
                                        Custom(
                                            rules.StringLiteral,
                                            CD("Notes", CompletionHint.Literal)))),
                                new [] {CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}),
                            Custom(
                                EToken("none"),
                                Optional(
                                    Custom(
                                        EToken("skip-results"),
                                        CD("SkipResults"))),
                                new [] {CD(), CD(isOptional: true)})),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}, CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral())), CreateMissingEToken(")"), (SyntaxElement)new CustomNode(new [] {CD("SkipResults"), CD(CompletionHint.Literal, isOptional: true)}, CreateMissingEToken("skip-results"), rules.MissingStringLiteral()))),
                    new [] {CD(), CD(), CD("Role"), CD()}));

            var SetContinuousExportCursor = Command("SetContinuousExportCursor", 
                Custom(
                    EToken("set", CompletionKind.CommandPrefix),
                    EToken("continuous-export"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    RequiredEToken("cursor"),
                    RequiredEToken("to"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("jobName", CompletionHint.None), CD(), CD(), CD("cursorValue", CompletionHint.Literal)}));

            var SetDatabaseRole = Command("SetDatabaseRole", 
                Custom(
                    EToken("set", CompletionKind.CommandPrefix),
                    EToken("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredEToken("admins", "ingestors", "monitors", "unrestrictedviewers", "users", "viewers"),
                    Required(
                        First(
                            Custom(
                                EToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.StringLiteral,
                                            CD("Principal", CompletionHint.Literal)),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingStringLiteral,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                                RequiredEToken(")"),
                                Optional(
                                    First(
                                        Custom(
                                            EToken("skip-results"),
                                            Optional(
                                                Custom(
                                                    rules.StringLiteral,
                                                    CD("Notes", CompletionHint.Literal))),
                                            new [] {CD("SkipResults"), CD(CompletionHint.Literal, isOptional: true)}),
                                        Custom(
                                            rules.StringLiteral,
                                            CD("Notes", CompletionHint.Literal)))),
                                new [] {CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}),
                            Custom(
                                EToken("none"),
                                Optional(
                                    Custom(
                                        EToken("skip-results"),
                                        CD("SkipResults"))),
                                new [] {CD(), CD(isOptional: true)})),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}, CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral())), CreateMissingEToken(")"), (SyntaxElement)new CustomNode(new [] {CD("SkipResults"), CD(CompletionHint.Literal, isOptional: true)}, CreateMissingEToken("skip-results"), rules.MissingStringLiteral()))),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD("Role"), CD()}));

            var SetExternalTableAdmins = Command("SetExternalTableAdmins", 
                Custom(
                    EToken("set", CompletionKind.CommandPrefix),
                    EToken("external"),
                    RequiredEToken("table"),
                    Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                    RequiredEToken("admins"),
                    Required(
                        First(
                            Custom(
                                EToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.StringLiteral,
                                            CD("principal", CompletionHint.Literal)),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingStringLiteral,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                                RequiredEToken(")"),
                                Optional(
                                    First(
                                        Custom(
                                            EToken("skip-results"),
                                            Optional(
                                                Custom(
                                                    rules.StringLiteral,
                                                    CD("notes", CompletionHint.Literal))),
                                            new [] {CD(), CD(CompletionHint.Literal, isOptional: true)}),
                                        Custom(
                                            rules.StringLiteral,
                                            CD("notes", CompletionHint.Literal)))),
                                new [] {CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}),
                            Custom(
                                EToken("none"),
                                Optional(EToken("skip-results")),
                                new [] {CD(), CD(isOptional: true)})),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}, CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral())), CreateMissingEToken(")"), (SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Literal, isOptional: true)}, CreateMissingEToken("skip-results"), rules.MissingStringLiteral()))),
                    new [] {CD(), CD(), CD(), CD("externalTableName", CompletionHint.ExternalTable), CD(), CD()}));

            var SetFunctionRole = Command("SetFunctionRole", 
                Custom(
                    EToken("set", CompletionKind.CommandPrefix),
                    EToken("function"),
                    Required(rules.DatabaseFunctionNameReference, rules.MissingNameReference),
                    RequiredEToken("admins"),
                    Required(
                        First(
                            Custom(
                                EToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.StringLiteral,
                                            CD("Principal", CompletionHint.Literal)),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingStringLiteral,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                                RequiredEToken(")"),
                                Optional(
                                    First(
                                        Custom(
                                            EToken("skip-results"),
                                            Optional(
                                                Custom(
                                                    rules.StringLiteral,
                                                    CD("Notes", CompletionHint.Literal))),
                                            new [] {CD("SkipResults"), CD(CompletionHint.Literal, isOptional: true)}),
                                        Custom(
                                            rules.StringLiteral,
                                            CD("Notes", CompletionHint.Literal)))),
                                new [] {CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}),
                            Custom(
                                EToken("none"),
                                Optional(
                                    Custom(
                                        EToken("skip-results"),
                                        CD("SkipResults"))),
                                new [] {CD(), CD(isOptional: true)})),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}, CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral())), CreateMissingEToken(")"), (SyntaxElement)new CustomNode(new [] {CD("SkipResults"), CD(CompletionHint.Literal, isOptional: true)}, CreateMissingEToken("skip-results"), rules.MissingStringLiteral()))),
                    new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD("Role"), CD()}));

            var SetMaterializedViewAdmins = Command("SetMaterializedViewAdmins", 
                Custom(
                    EToken("set", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("admins"),
                    Required(
                        First(
                            Custom(
                                EToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.StringLiteral,
                                            CD("principal", CompletionHint.Literal)),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingStringLiteral,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                                RequiredEToken(")"),
                                Optional(
                                    Custom(
                                        rules.StringLiteral,
                                        CD("notes", CompletionHint.Literal))),
                                new [] {CD(), CD(CompletionHint.Literal), CD(), CD(CompletionHint.Literal, isOptional: true)}),
                            EToken("none")),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Literal), CD(), CD(CompletionHint.Literal, isOptional: true)}, CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral())), CreateMissingEToken(")"), rules.MissingStringLiteral())),
                    new [] {CD(), CD(), CD("materializedViewName", CompletionHint.MaterializedView), CD(), CD()}));

            var SetMaterializedViewConcurrency = Command("SetMaterializedViewConcurrency", 
                Custom(
                    EToken("set", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("concurrency"),
                    Optional(
                        Custom(
                            EToken("="),
                            Required(
                                First(
                                    rules.Value,
                                    rules.Value),
                                rules.MissingValue),
                            new [] {CD(), CD("n", CompletionHint.Literal)})),
                    new [] {CD(), CD(), CD("viewName", CompletionHint.MaterializedView), CD(), CD(isOptional: true)}));

            var SetMaterializedViewCursor = Command("SetMaterializedViewCursor", 
                Custom(
                    EToken("set", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    RequiredEToken("cursor"),
                    RequiredEToken("to"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD("CursorValue", CompletionHint.Literal)}));

            var StoredQueryResultSet = Command("StoredQueryResultSet", 
                Custom(
                    EToken("set", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            EToken("async"),
                            EToken("stored_query_result")),
                        EToken("stored_query_result")),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Required(
                        First(
                            EToken("<|"),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            First(
                                                EToken("creationTime"),
                                                EToken("distributed"),
                                                EToken("docstring"),
                                                EToken("extend_schema"),
                                                EToken("folder"),
                                                EToken("format"),
                                                EToken("ignoreFirstRecord"),
                                                EToken("ingestIfNotExists"),
                                                EToken("ingestionMappingReference"),
                                                EToken("ingestionMapping"),
                                                EToken("persistDetails"),
                                                EToken("policy_ingestionTime"),
                                                EToken("recreate_schema"),
                                                EToken("tags"),
                                                EToken("validationPolicy"),
                                                EToken("zipPattern"),
                                                If(Not(And(EToken("creationTime", "distributed", "docstring", "extend_schema", "folder", "format", "ignoreFirstRecord", "ingestIfNotExists", "ingestionMappingReference", "ingestionMapping", "persistDetails", "policy_ingestionTime", "recreate_schema", "tags", "validationPolicy", "zipPattern"))), rules.NameDeclarationOrStringLiteral)),
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("<|"))),
                        () => CreateMissingEToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    new [] {CD(), CD(), CD("StoredQueryResultName", CompletionHint.None), CD(), CD("Query", CompletionHint.Tabular)}));

            var SetTableRowStoreReferences = Command("SetTableRowStoreReferences", 
                Custom(
                    EToken("set", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    RequiredEToken("rowstore_references"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("references", CompletionHint.Literal)}));

            var SetTableRole = Command("SetTableRole", 
                Custom(
                    EToken("set", CompletionKind.CommandPrefix),
                    EToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredEToken("admins", "ingestors"),
                    Required(
                        First(
                            Custom(
                                EToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.StringLiteral,
                                            CD("Principal", CompletionHint.Literal)),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingStringLiteral,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                                RequiredEToken(")"),
                                Optional(
                                    First(
                                        Custom(
                                            EToken("skip-results"),
                                            Optional(
                                                Custom(
                                                    rules.StringLiteral,
                                                    CD("Notes", CompletionHint.Literal))),
                                            new [] {CD("SkipResults"), CD(CompletionHint.Literal, isOptional: true)}),
                                        Custom(
                                            rules.StringLiteral,
                                            CD("Notes", CompletionHint.Literal)))),
                                new [] {CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}),
                            Custom(
                                EToken("none"),
                                Optional(
                                    Custom(
                                        EToken("skip-results"),
                                        CD("SkipResults"))),
                                new [] {CD(), CD(isOptional: true)})),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}, CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral())), CreateMissingEToken(")"), (SyntaxElement)new CustomNode(new [] {CD("SkipResults"), CD(CompletionHint.Literal, isOptional: true)}, CreateMissingEToken("skip-results"), rules.MissingStringLiteral()))),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD("Role"), CD()}));

            var SetTable = Command("SetTable", 
                Custom(
                    EToken("set", CompletionKind.CommandPrefix),
                    Required(
                        First(
                            Custom(
                                EToken("async"),
                                Required(If(Not(EToken("stored_query_result")), rules.NameDeclarationOrStringLiteral), rules.MissingNameDeclaration),
                                new [] {CD(), CD("TableName", CompletionHint.None)}),
                            Custom(
                                If(Not(And(EToken("access", "cluster", "continuous-export", "database", "external", "function", "materialized-view", "async", "stored_query_result", "table"))), rules.NameDeclarationOrStringLiteral),
                                CD("TableName", CompletionHint.None))),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD("TableName", CompletionHint.None)}, CreateMissingEToken("async"), rules.MissingNameDeclaration())),
                    Required(
                        First(
                            EToken("<|"),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            First(
                                                EToken("creationTime"),
                                                EToken("distributed"),
                                                EToken("docstring"),
                                                EToken("extend_schema"),
                                                EToken("folder"),
                                                EToken("format"),
                                                EToken("ignoreFirstRecord"),
                                                EToken("ingestIfNotExists"),
                                                EToken("ingestionMappingReference"),
                                                EToken("ingestionMapping"),
                                                EToken("persistDetails"),
                                                EToken("policy_ingestionTime"),
                                                EToken("recreate_schema"),
                                                EToken("tags"),
                                                EToken("validationPolicy"),
                                                EToken("zipPattern"),
                                                If(Not(And(EToken("creationTime", "distributed", "docstring", "extend_schema", "folder", "format", "ignoreFirstRecord", "ingestIfNotExists", "ingestionMappingReference", "ingestionMapping", "persistDetails", "policy_ingestionTime", "recreate_schema", "tags", "validationPolicy", "zipPattern"))), rules.NameDeclarationOrStringLiteral)),
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("<|"))),
                        () => CreateMissingEToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    new [] {CD(), CD(), CD(), CD("QueryOrCommand", CompletionHint.Tabular)}));

            var ShowBasicAuthUsers = Command("ShowBasicAuthUsers", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("basicauth"),
                    RequiredEToken("users")));

            var ShowCache = Command("ShowCache", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cache")));

            var ShowCallStacks = Command("ShowCallStacks", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("callstacks"),
                    Optional(
                        First(
                            Custom(
                                EToken("for"),
                                Required(rules.Value, rules.MissingValue),
                                Optional(
                                    Custom(
                                        EToken("with"),
                                        RequiredEToken("("),
                                        Required(
                                            OList(
                                                primaryElementParser: Custom(
                                                    rules.NameDeclarationOrStringLiteral,
                                                    RequiredEToken("="),
                                                    Required(rules.Value, rules.MissingValue),
                                                    new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                                separatorParser: EToken(","),
                                                secondaryElementParser: null,
                                                missingPrimaryElement: null,
                                                missingSeparator: null,
                                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                                endOfList: null,
                                                oneOrMore: true,
                                                allowTrailingSeparator: false,
                                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                        RequiredEToken(")"),
                                        new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                                new [] {CD(), CD("duration", CompletionHint.Literal), CD(isOptional: true)}),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                new [] {CD(), CD(), CD(CompletionHint.None), CD()}))),
                    new [] {CD(), CD(), CD(isOptional: true)}));

            var ShowCapacity = Command("ShowCapacity", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("capacity"),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            RequiredEToken("scope"),
                            RequiredEToken("="),
                            RequiredEToken("cluster", "workloadgroup"),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(), CD(), CD("Scope"), CD()})),
                    new [] {CD(), CD(), CD(isOptional: true)}));

            var ShowClusterAdminState = Command("ShowClusterAdminState", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("admin"),
                    RequiredEToken("state")));

            var ShowClusterBlockedPrincipals = Command("ShowClusterBlockedPrincipals", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("blockedprincipals")));

            var ShowClusterExtentsMetadata = Command("ShowClusterExtentsMetadata", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("extents"),
                    Optional(
                        First(
                            Custom(
                                EToken("("),
                                OList(
                                    primaryElementParser: Custom(
                                        rules.AnyGuidLiteralOrString,
                                        CD("ExtentId", CompletionHint.Literal)),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: rules.MissingValue,
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                EToken(")"),
                                Optional(EToken("hot")),
                                new [] {CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}),
                            EToken("hot"))),
                    EToken("metadata"),
                    Optional(
                        First(
                            Custom(
                                EToken("where"),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            EToken("tags"),
                                            RequiredEToken("!contains", "!has", "contains", "has"),
                                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                                            new [] {CD(), CD(), CD("Tag", CompletionHint.Literal)}),
                                        separatorParser: EToken("and"),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("Tag", CompletionHint.Literal)}, CreateMissingEToken("tags"), CreateMissingEToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("Tag", CompletionHint.Literal)}, CreateMissingEToken("tags"), CreateMissingEToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral())))),
                                Optional(
                                    Custom(
                                        EToken("with"),
                                        RequiredEToken("("),
                                        RequiredEToken("extentsShowFilteringRuntimePolicy"),
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        RequiredEToken(")"),
                                        new [] {CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()})),
                                new [] {CD(), CD(), CD(isOptional: true)}),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                RequiredEToken("extentsShowFilteringRuntimePolicy"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredEToken(")"),
                                new [] {CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()}))),
                    new [] {CD(), CD(), CD(), CD(isOptional: true), CD(), CD(isOptional: true)}));

            var ShowClusterExtents = Command("ShowClusterExtents", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("extents"),
                    Optional(
                        First(
                            Custom(
                                EToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.AnyGuidLiteralOrString,
                                            CD("ExtentId", CompletionHint.Literal)),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingValue,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingValue()))),
                                RequiredEToken(")"),
                                Optional(EToken("hot")),
                                new [] {CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}),
                            EToken("hot"))),
                    Optional(
                        First(
                            Custom(
                                EToken("where"),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            EToken("tags"),
                                            RequiredEToken("!contains", "!has", "contains", "has"),
                                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                                            new [] {CD(), CD(), CD("Tag", CompletionHint.Literal)}),
                                        separatorParser: EToken("and"),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("Tag", CompletionHint.Literal)}, CreateMissingEToken("tags"), CreateMissingEToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("Tag", CompletionHint.Literal)}, CreateMissingEToken("tags"), CreateMissingEToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral())))),
                                Optional(
                                    Custom(
                                        EToken("with"),
                                        RequiredEToken("("),
                                        RequiredEToken("extentsShowFilteringRuntimePolicy"),
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        RequiredEToken(")"),
                                        new [] {CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()})),
                                new [] {CD(), CD(), CD(isOptional: true)}),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                RequiredEToken("extentsShowFilteringRuntimePolicy"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredEToken(")"),
                                new [] {CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()}))),
                    new [] {CD(), CD(), CD(), CD(isOptional: true), CD(isOptional: true)}));

            var ShowClusterJournal = Command("ShowClusterJournal", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("journal")));

            var ShowClusterMonitoring = Command("ShowClusterMonitoring", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("monitoring")));

            var ShowClusterNetwork = Command("ShowClusterNetwork", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("network"),
                    Optional(
                        Custom(
                            rules.Value,
                            CD("bytes", CompletionHint.Literal))),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Literal, isOptional: true)}));

            var ShowClusterPendingContinuousExports = Command("ShowClusterPendingContinuousExports", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("pending"),
                    RequiredEToken("continuous-exports"),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD(), CD(), CD(isOptional: true)}));

            var ShowClusterPolicyCaching = Command("ShowClusterPolicyCaching", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("caching")));

            var ShowClusterPolicyCallout = Command("ShowClusterPolicyCallout", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("callout")));

            var ShowClusterPolicyCapacity = Command("ShowClusterPolicyCapacity", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("capacity")));

            var ShowClusterPolicyDiagnostics = Command("ShowClusterPolicyDiagnostics", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("diagnostics")));

            var ShowClusterPolicyIngestionBatching = Command("ShowClusterPolicyIngestionBatching", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("ingestionbatching")));

            var ShowClusterPolicyManagedIdentity = Command("ShowClusterPolicyManagedIdentity", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("managed_identity")));

            var ShowClusterPolicyMultiDatabaseAdmins = Command("ShowClusterPolicyMultiDatabaseAdmins", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("multidatabaseadmins")));

            var ShowClusterPolicyQueryWeakConsistency = Command("ShowClusterPolicyQueryWeakConsistency", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("query_weak_consistency")));

            var ShowClusterPolicyRequestClassification = Command("ShowClusterPolicyRequestClassification", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("request_classification")));

            var ShowClusterPolicyRowStore = Command("ShowClusterPolicyRowStore", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("rowstore")));

            var ShowClusterPolicySandbox = Command("ShowClusterPolicySandbox", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("sandbox")));

            var ShowClusterPolicySharding = Command("ShowClusterPolicySharding", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("sharding"),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD(), CD(), CD(isOptional: true)}));

            var ShowClusterPolicyStreamingIngestion = Command("ShowClusterPolicyStreamingIngestion", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    RequiredEToken("streamingingestion")));

            var ShowClusterPrincipals = Command("ShowClusterPrincipals", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("principals")));

            var ShowClusterPrincipalRoles = Command("ShowClusterPrincipalRoles", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("principal"),
                    Required(
                        First(
                            EToken("roles"),
                            Custom(
                                rules.StringLiteral,
                                RequiredEToken("roles"),
                                new [] {CD("Principal", CompletionHint.Literal), CD()})),
                        () => CreateMissingEToken("roles")),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD(), CD(), CD(isOptional: true)}));

            var ShowClusterSandboxesStats = Command("ShowClusterSandboxesStats", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("sandboxes"),
                    RequiredEToken("stats")));

            var ShowClusterScaleIn = Command("ShowClusterScaleIn", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("scalein"),
                    Required(
                        First(
                            rules.Value,
                            rules.Value),
                        rules.MissingValue),
                    RequiredEToken("nodes"),
                    new [] {CD(), CD(), CD(), CD("num", CompletionHint.Literal), CD()}));

            var ShowClusterStorageKeysHash = Command("ShowClusterStorageKeysHash", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("storage"),
                    RequiredEToken("keys"),
                    RequiredEToken("hash")));

            var ShowCluster = Command("ShowCluster", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("cluster")));

            var ShowColumnPolicyCaching = Command("ShowColumnPolicyCaching", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("column"),
                    First(
                        EToken("*"),
                        If(Not(EToken("*")), rules.DatabaseTableColumnNameReference)),
                    RequiredEToken("policy"),
                    RequiredEToken("caching"),
                    new [] {CD(), CD(), CD("ColumnName"), CD(), CD()}));

            var ShowColumnPolicyEncoding = Command("ShowColumnPolicyEncoding", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("column"),
                    Required(If(Not(EToken("*")), rules.TableColumnNameReference), rules.MissingNameReference),
                    RequiredEToken("policy"),
                    RequiredEToken("encoding"),
                    new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD()}));

            var ShowCommandsAndQueries = Command("ShowCommandsAndQueries", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("commands-and-queries")));

            var ShowCommands = Command("ShowCommands", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("commands")));

            var ShowContinuousExports = Command("ShowContinuousExports", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("continuous-exports")));

            var ShowContinuousExportExportedArtifacts = Command("ShowContinuousExportExportedArtifacts", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("continuous-export"),
                    rules.NameDeclarationOrStringLiteral,
                    EToken("exported-artifacts"),
                    new [] {CD(), CD(), CD("ContinuousExportName", CompletionHint.None), CD()}));

            var ShowContinuousExportFailures = Command("ShowContinuousExportFailures", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("continuous-export"),
                    rules.NameDeclarationOrStringLiteral,
                    EToken("failures"),
                    new [] {CD(), CD(), CD("ContinuousExportName", CompletionHint.None), CD()}));

            var ShowContinuousExport = Command("ShowContinuousExport", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("continuous-export"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    new [] {CD(), CD(), CD("ContinuousExportName", CompletionHint.None)}));

            var ShowDatabasesSchemaAsJson = Command("ShowDatabasesSchemaAsJson", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("databases"),
                    EToken("("),
                    OList(
                        primaryElementParser: Custom(
                            rules.DatabaseNameReference,
                            Optional(
                                Custom(
                                    EToken("if_later_than"),
                                    rules.StringLiteral,
                                    new [] {CD(), CD("Version", CompletionHint.Literal)})),
                            new [] {CD("DatabaseName", CompletionHint.Database), CD(isOptional: true)}),
                        separatorParser: EToken(","),
                        secondaryElementParser: null,
                        missingPrimaryElement: null,
                        missingSeparator: null,
                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("DatabaseName", CompletionHint.Database), CD(isOptional: true)}, rules.MissingNameReference(), (SyntaxElement)new CustomNode(new [] {CD(), CD("Version", CompletionHint.Literal)}, CreateMissingEToken("if_later_than"), rules.MissingStringLiteral())),
                        endOfList: null,
                        oneOrMore: true,
                        allowTrailingSeparator: false,
                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                    EToken(")"),
                    EToken("schema"),
                    EToken("as"),
                    RequiredEToken("json"),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Database), CD(), CD(), CD(), CD()}));

            var ShowDatabasesSchema = Command("ShowDatabasesSchema", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("databases"),
                    EToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.DatabaseNameReference,
                                Optional(
                                    Custom(
                                        EToken("if_later_than"),
                                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                                        new [] {CD(), CD("Version", CompletionHint.Literal)})),
                                new [] {CD("DatabaseName", CompletionHint.Database), CD(isOptional: true)}),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("DatabaseName", CompletionHint.Database), CD(isOptional: true)}, rules.MissingNameReference(), (SyntaxElement)new CustomNode(new [] {CD(), CD("Version", CompletionHint.Literal)}, CreateMissingEToken("if_later_than"), rules.MissingStringLiteral())),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("DatabaseName", CompletionHint.Database), CD(isOptional: true)}, rules.MissingNameReference(), (SyntaxElement)new CustomNode(new [] {CD(), CD("Version", CompletionHint.Literal)}, CreateMissingEToken("if_later_than"), rules.MissingStringLiteral()))))),
                    RequiredEToken(")"),
                    RequiredEToken("schema"),
                    Optional(EToken("details")),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Database), CD(), CD(), CD(isOptional: true)}));

            var ShowClusterDatabasesDataStats = Command("ShowClusterDatabasesDataStats", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            EToken("cluster"),
                            EToken("databases")),
                        EToken("databases")),
                    EToken("datastats")));

            var ShowClusterDatabasesDetails = Command("ShowClusterDatabasesDetails", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            EToken("cluster"),
                            EToken("databases")),
                        EToken("databases")),
                    EToken("details")));

            var ShowClusterDatabasesIdentity = Command("ShowClusterDatabasesIdentity", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            EToken("cluster"),
                            EToken("databases")),
                        EToken("databases")),
                    RequiredEToken("identity")));

            var ShowDatabasesManagementGroups = Command("ShowDatabasesManagementGroups", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("databases"),
                    EToken("management"),
                    RequiredEToken("groups")));

            var ShowClusterDatabasesPolicies = Command("ShowClusterDatabasesPolicies", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            EToken("cluster"),
                            EToken("databases")),
                        EToken("databases")),
                    EToken("policies")));

            var ShowClusterDatabases = Command("ShowClusterDatabases", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            EToken("cluster"),
                            EToken("databases")),
                        Custom(
                            EToken("cluster"),
                            RequiredEToken("databases")),
                        EToken("databases")),
                    Optional(
                        Custom(
                            EToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.DatabaseNameReference,
                                        CD("DatabaseName", CompletionHint.Database)),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: rules.MissingNameReference,
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameReference()))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(CompletionHint.Database), CD()})),
                    new [] {CD(), CD(), CD(isOptional: true)}));

            var ShowDatabaseCacheQueryResults = Command("ShowDatabaseCacheQueryResults", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    EToken("cache"),
                    RequiredEToken("query_results")));

            var ShowDatabaseDataStats = Command("ShowDatabaseDataStats", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    EToken("datastats")));

            var ShowDatabaseDetails = Command("ShowDatabaseDetails", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    EToken("details")));

            var ShowDatabaseExtentsMetadata = Command("ShowDatabaseExtentsMetadata", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            EToken("databases"),
                            EToken("("),
                            OList(
                                primaryElementParser: Custom(
                                    rules.DatabaseNameReference,
                                    CD("DatabaseName", CompletionHint.Database)),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingNameReference,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            EToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.Database), CD()}),
                        Custom(
                            EToken("database"),
                            Optional(
                                Custom(
                                    If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                                    CD("DatabaseName", CompletionHint.Database))),
                            new [] {CD(), CD(CompletionHint.Database, isOptional: true)})),
                    EToken("extents"),
                    Optional(
                        First(
                            Custom(
                                EToken("("),
                                OList(
                                    primaryElementParser: Custom(
                                        rules.AnyGuidLiteralOrString,
                                        CD("ExtentId", CompletionHint.Literal)),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: rules.MissingValue,
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                EToken(")"),
                                Optional(EToken("hot")),
                                new [] {CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}),
                            EToken("hot"))),
                    EToken("metadata"),
                    Optional(
                        First(
                            Custom(
                                EToken("where"),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            EToken("tags"),
                                            RequiredEToken("!contains", "!has", "contains", "has"),
                                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                                            new [] {CD(), CD(), CD("Tag", CompletionHint.Literal)}),
                                        separatorParser: EToken("and"),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("Tag", CompletionHint.Literal)}, CreateMissingEToken("tags"), CreateMissingEToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("Tag", CompletionHint.Literal)}, CreateMissingEToken("tags"), CreateMissingEToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral())))),
                                Optional(
                                    Custom(
                                        EToken("with"),
                                        RequiredEToken("("),
                                        RequiredEToken("extentsShowFilteringRuntimePolicy"),
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        RequiredEToken(")"),
                                        new [] {CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()})),
                                new [] {CD(), CD(), CD(isOptional: true)}),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                RequiredEToken("extentsShowFilteringRuntimePolicy"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredEToken(")"),
                                new [] {CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()}))),
                    new [] {CD(), CD(), CD(), CD(isOptional: true), CD(), CD(isOptional: true)}));

            var ShowDatabaseExtents = Command("ShowDatabaseExtents", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            EToken("databases"),
                            EToken("("),
                            OList(
                                primaryElementParser: Custom(
                                    rules.DatabaseNameReference,
                                    CD("DatabaseName", CompletionHint.Database)),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingNameReference,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            EToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.Database), CD()}),
                        Custom(
                            EToken("databases"),
                            EToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.DatabaseNameReference,
                                        CD("DatabaseName", CompletionHint.Database)),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: rules.MissingNameReference,
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameReference()))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.Database), CD()}),
                        Custom(
                            EToken("database"),
                            Optional(
                                Custom(
                                    If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                                    CD("DatabaseName", CompletionHint.Database))),
                            new [] {CD(), CD(CompletionHint.Database, isOptional: true)})),
                    RequiredEToken("extents"),
                    Optional(
                        First(
                            Custom(
                                EToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.AnyGuidLiteralOrString,
                                            CD("ExtentId", CompletionHint.Literal)),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingValue,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingValue()))),
                                RequiredEToken(")"),
                                Optional(EToken("hot")),
                                new [] {CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}),
                            EToken("hot"))),
                    Optional(
                        First(
                            Custom(
                                EToken("where"),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            EToken("tags"),
                                            RequiredEToken("!contains", "!has", "contains", "has"),
                                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                                            new [] {CD(), CD(), CD("Tag", CompletionHint.Literal)}),
                                        separatorParser: EToken("and"),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("Tag", CompletionHint.Literal)}, CreateMissingEToken("tags"), CreateMissingEToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("Tag", CompletionHint.Literal)}, CreateMissingEToken("tags"), CreateMissingEToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral())))),
                                Optional(
                                    Custom(
                                        EToken("with"),
                                        RequiredEToken("("),
                                        RequiredEToken("extentsShowFilteringRuntimePolicy"),
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        RequiredEToken(")"),
                                        new [] {CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()})),
                                new [] {CD(), CD(), CD(isOptional: true)}),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                RequiredEToken("extentsShowFilteringRuntimePolicy"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredEToken(")"),
                                new [] {CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()}))),
                    new [] {CD(), CD(), CD(), CD(isOptional: true), CD(isOptional: true)}));

            var ShowDatabaseExtentTagsStatistics = Command("ShowDatabaseExtentTagsStatistics", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    EToken("extent"),
                    RequiredEToken("tags"),
                    RequiredEToken("statistics"),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            RequiredEToken("minCreationTime"),
                            RequiredEToken("="),
                            Required(rules.Value, rules.MissingValue),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(), CD(), CD("minCreationTime", CompletionHint.Literal), CD()})),
                    new [] {CD(), CD(), CD(), CD(), CD(), CD(isOptional: true)}));

            var ShowDatabaseIdentity = Command("ShowDatabaseIdentity", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    EToken("identity")));

            var ShowDatabasePolicies = Command("ShowDatabasePolicies", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    EToken("policies")));

            var ShowDatabaseCslSchema = Command("ShowDatabaseCslSchema", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    First(
                        EToken("cslschema"),
                        Custom(
                            If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                            EToken("cslschema"),
                            new [] {CD("databaseName", CompletionHint.Database), CD()})),
                    Optional(
                        First(
                            Custom(
                                EToken("if_later_than"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                new [] {CD(), CD("databaseVersion", CompletionHint.Literal)}),
                            Custom(
                                EToken("script"),
                                Optional(
                                    Custom(
                                        EToken("if_later_than"),
                                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                                        new [] {CD(), CD("databaseVersion", CompletionHint.Literal)})),
                                new [] {CD(), CD(isOptional: true)}))),
                    new [] {CD(), CD(), CD(), CD(isOptional: true)}));

            var ShowDatabaseIngestionMappings = Command("ShowDatabaseIngestionMappings", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    First(
                        EToken("ingestion"),
                        Custom(
                            If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                            EToken("ingestion"),
                            new [] {CD("databaseName", CompletionHint.Database), CD()})),
                    Required(
                        First(
                            EToken("mappings"),
                            Custom(
                                EToken("apacheavro", "avro", "csv", "json", "orc", "parquet", "sstream", "w3clogfile"),
                                RequiredEToken("mappings"),
                                new [] {CD("kind"), CD()})),
                        () => CreateMissingEToken("mappings")),
                    Optional(
                        First(
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                new [] {CD(), CD(), CD(CompletionHint.None), CD()}),
                            Custom(
                                rules.StringLiteral,
                                Optional(
                                    Custom(
                                        EToken("with"),
                                        RequiredEToken("("),
                                        Required(
                                            OList(
                                                primaryElementParser: Custom(
                                                    rules.NameDeclarationOrStringLiteral,
                                                    RequiredEToken("="),
                                                    Required(rules.Value, rules.MissingValue),
                                                    new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                                separatorParser: EToken(","),
                                                secondaryElementParser: null,
                                                missingPrimaryElement: null,
                                                missingSeparator: null,
                                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                                endOfList: null,
                                                oneOrMore: true,
                                                allowTrailingSeparator: false,
                                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                        RequiredEToken(")"),
                                        new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                                new [] {CD("name", CompletionHint.Literal), CD(isOptional: true)}))),
                    new [] {CD(), CD(), CD(), CD(), CD(isOptional: true)}));

            var ShowDatabaseSchemaAsCslScript = Command("ShowDatabaseSchemaAsCslScript", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    First(
                        EToken("schema"),
                        Custom(
                            If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                            EToken("schema"),
                            new [] {CD("DatabaseName", CompletionHint.Database), CD()})),
                    First(
                        EToken("as"),
                        Custom(
                            EToken("if_later_than"),
                            rules.StringLiteral,
                            EToken("as"),
                            new [] {CD(), CD("Version", CompletionHint.Literal), CD()})),
                    EToken("csl"),
                    RequiredEToken("script"),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD(), CD(), CD(), CD(), CD(isOptional: true)}));

            var ShowDatabaseSchemaAsJson = Command("ShowDatabaseSchemaAsJson", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    First(
                        EToken("schema"),
                        Custom(
                            If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                            EToken("schema"),
                            new [] {CD("DatabaseName", CompletionHint.Database), CD()})),
                    First(
                        EToken("as"),
                        Custom(
                            EToken("if_later_than"),
                            rules.StringLiteral,
                            EToken("as"),
                            new [] {CD(), CD("Version", CompletionHint.Literal), CD()})),
                    RequiredEToken("json")));

            var ShowDatabaseSchema = Command("ShowDatabaseSchema", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    First(
                        EToken("schema"),
                        Custom(
                            If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                            EToken("schema"),
                            new [] {CD("databaseName", CompletionHint.Database), CD()})),
                    Optional(
                        First(
                            Custom(
                                EToken("details"),
                                Optional(
                                    Custom(
                                        EToken("if_later_than"),
                                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                                        new [] {CD(), CD("databaseVersion", CompletionHint.Literal)})),
                                new [] {CD(), CD(isOptional: true)}),
                            Custom(
                                EToken("if_later_than"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                new [] {CD(), CD("databaseVersion", CompletionHint.Literal)}))),
                    new [] {CD(), CD(), CD(), CD(isOptional: true)}));

            var DatabaseShardGroupsStatisticsShow = Command("DatabaseShardGroupsStatisticsShow", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    First(
                        EToken("shard-groups").Hide(),
                        Custom(
                            If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                            EToken("shard-groups").Hide(),
                            new [] {CD("DatabaseName", CompletionHint.Database), CD()})),
                    RequiredEToken("statistics").Hide()));

            var ShowDatabaseExtentContainersCleanOperations = Command("ShowDatabaseExtentContainersCleanOperations", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    EToken("extentcontainers"),
                    RequiredEToken("clean"),
                    RequiredEToken("operations"),
                    Optional(
                        Custom(
                            rules.AnyGuidLiteralOrString,
                            CD("obj", CompletionHint.Literal))),
                    new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(), CD(), CD(CompletionHint.Literal, isOptional: true)}));

            var ShowDatabaseJournal = Command("ShowDatabaseJournal", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    EToken("journal"),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD()}));

            var ShowDatabasePolicyCaching = Command("ShowDatabasePolicyCaching", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    First(
                        EToken("*"),
                        If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference)),
                    EToken("policy"),
                    EToken("caching"),
                    new [] {CD(), CD(), CD("DatabaseName"), CD(), CD()}));

            var ShowDatabasePolicyDiagnostics = Command("ShowDatabasePolicyDiagnostics", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    EToken("policy"),
                    EToken("diagnostics"),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD()}));

            var ShowDatabasePolicyEncoding = Command("ShowDatabasePolicyEncoding", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    EToken("policy"),
                    EToken("encoding"),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD()}));

            var ShowDatabasePolicyExtentTagsRetention = Command("ShowDatabasePolicyExtentTagsRetention", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    First(
                        EToken("*"),
                        If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference)),
                    EToken("policy"),
                    EToken("extent_tags_retention"),
                    new [] {CD(), CD(), CD("DatabaseName"), CD(), CD()}));

            var ShowDatabasePolicyHardRetentionViolations = Command("ShowDatabasePolicyHardRetentionViolations", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    EToken("policy"),
                    EToken("hardretention"),
                    RequiredEToken("violations"),
                    new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(), CD()}));

            var ShowDatabasePolicyIngestionBatching = Command("ShowDatabasePolicyIngestionBatching", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    First(
                        EToken("*"),
                        If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference)),
                    EToken("policy"),
                    EToken("ingestionbatching"),
                    new [] {CD(), CD(), CD("DatabaseName"), CD(), CD()}));

            var ShowDatabasePolicyManagedIdentity = Command("ShowDatabasePolicyManagedIdentity", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    EToken("policy"),
                    EToken("managed_identity"),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD()}));

            var ShowDatabasePolicyMerge = Command("ShowDatabasePolicyMerge", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    First(
                        EToken("*"),
                        If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference)),
                    EToken("policy"),
                    EToken("merge"),
                    new [] {CD(), CD(), CD("DatabaseName"), CD(), CD()}));

            var ShowDatabasePolicyRetention = Command("ShowDatabasePolicyRetention", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    First(
                        EToken("*"),
                        If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference)),
                    EToken("policy"),
                    EToken("retention"),
                    new [] {CD(), CD(), CD("DatabaseName"), CD(), CD()}));

            var ShowDatabasePolicySharding = Command("ShowDatabasePolicySharding", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    First(
                        EToken("*"),
                        If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference)),
                    EToken("policy"),
                    EToken("sharding"),
                    new [] {CD(), CD(), CD("DatabaseName"), CD(), CD()}));

            var ShowDatabasePolicyShardsGrouping = Command("ShowDatabasePolicyShardsGrouping", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    First(
                        EToken("*"),
                        If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference)),
                    EToken("policy"),
                    EToken("shards_grouping").Hide(),
                    new [] {CD(), CD(), CD("DatabaseName"), CD(), CD()}));

            var ShowDatabasePolicySoftRetentionViolations = Command("ShowDatabasePolicySoftRetentionViolations", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    EToken("policy"),
                    EToken("softretention"),
                    RequiredEToken("violations"),
                    new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(), CD()}));

            var ShowDatabasePolicyStreamingIngestion = Command("ShowDatabasePolicyStreamingIngestion", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    EToken("policy"),
                    RequiredEToken("streamingingestion"),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD()}));

            var ShowDatabasePrincipals = Command("ShowDatabasePrincipals", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    EToken("principals"),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD()}));

            var ShowDatabasePrincipalRoles = Command("ShowDatabasePrincipalRoles", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    EToken("principal"),
                    Required(
                        First(
                            EToken("roles"),
                            Custom(
                                rules.StringLiteral,
                                RequiredEToken("roles"),
                                new [] {CD("Principal", CompletionHint.Literal), CD()})),
                        () => CreateMissingEToken("roles")),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD(isOptional: true)}));

            var ShowDatabasePurgeOperation = Command("ShowDatabasePurgeOperation", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    EToken("purge"),
                    Required(
                        First(
                            Custom(
                                EToken("operations"),
                                Optional(
                                    Custom(
                                        rules.AnyGuidLiteralOrString,
                                        CD("obj", CompletionHint.Literal))),
                                new [] {CD(), CD(CompletionHint.Literal, isOptional: true)}),
                            Custom(
                                EToken("operation"),
                                Required(rules.AnyGuidLiteralOrString, rules.MissingValue),
                                new [] {CD(), CD("obj", CompletionHint.Literal)})),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Literal, isOptional: true)}, CreateMissingEToken("operations"), rules.MissingValue())),
                    new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD()}));

            var ShowDatabaseSchemaViolations = Command("ShowDatabaseSchemaViolations", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    RequiredEToken("schema"),
                    RequiredEToken("violations"),
                    new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD()}));

            var ShowDatabase = Command("ShowDatabase", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database")));

            var ShowDiagnostics = Command("ShowDiagnostics", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("diagnostics"),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            RequiredEToken("scope"),
                            RequiredEToken("="),
                            RequiredEToken("cluster", "workloadgroup"),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(), CD(), CD("Scope"), CD()})),
                    new [] {CD(), CD(), CD(isOptional: true)}));

            var ShowEntitySchema = Command("ShowEntitySchema", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("entity"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    RequiredEToken("schema"),
                    RequiredEToken("as"),
                    RequiredEToken("json"),
                    Optional(
                        First(
                            Custom(
                                EToken("except"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                new [] {CD(), CD("excludedFunctions", CompletionHint.Literal)}),
                            Custom(
                                EToken("in"),
                                RequiredEToken("databases"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.StringLiteral,
                                            CD("item", CompletionHint.Literal)),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingStringLiteral,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                                RequiredEToken(")"),
                                Optional(
                                    Custom(
                                        EToken("except"),
                                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                                        new [] {CD(), CD("excludedFunctions", CompletionHint.Literal)})),
                                new [] {CD(), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}))),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD("entity", CompletionHint.None), CD(), CD(), CD(), CD(isOptional: true), CD(isOptional: true)}));

            var ShowExtentContainers = Command("ShowExtentContainers", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("extentcontainers"),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD(isOptional: true)}));

            var ShowExtentColumnStorageStats = Command("ShowExtentColumnStorageStats", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("extent"),
                    rules.AnyGuidLiteralOrString,
                    EToken("column"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    RequiredEToken("storage"),
                    RequiredEToken("stats"),
                    new [] {CD(), CD(), CD("extentId", CompletionHint.Literal), CD(), CD("columnName", CompletionHint.None), CD(), CD()}));

            var ShowExtentDetails = Command("ShowExtentDetails", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("extent"),
                    Required(
                        First(
                            Custom(
                                EToken("details"),
                                Required(
                                    First(
                                        Custom(
                                            rules.AnyGuidLiteralOrString,
                                            CD("eid", CompletionHint.Literal)),
                                        Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            CD("tname", CompletionHint.None))),
                                    rules.MissingValue),
                                new [] {CD(), CD(CompletionHint.Literal)}),
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                CD("eid", CompletionHint.Literal)),
                            Custom(
                                If(Not(EToken("details")), rules.NameDeclarationOrStringLiteral),
                                CD("tname", CompletionHint.None))),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Literal)}, CreateMissingEToken("details"), rules.MissingValue()))));

            var ShowExternalTables = Command("ShowExternalTables", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("external"),
                    EToken("tables")));

            var ShowExternalTableArtifacts = Command("ShowExternalTableArtifacts", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("external"),
                    EToken("table"),
                    rules.ExternalTableNameReference,
                    EToken("artifacts"),
                    Optional(
                        Custom(
                            EToken("limit"),
                            Required(rules.Value, rules.MissingValue),
                            new [] {CD(), CD("LimitCount", CompletionHint.Literal)})),
                    new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD(isOptional: true)}));

            var ShowExternalTableCslSchema = Command("ShowExternalTableCslSchema", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("external"),
                    EToken("table"),
                    rules.ExternalTableNameReference,
                    EToken("cslschema"),
                    new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD()}));

            var ShowExternalTableMappings = Command("ShowExternalTableMappings", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("external"),
                    EToken("table"),
                    rules.ExternalTableNameReference,
                    EToken("mappings"),
                    new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD()}));

            var ShowExternalTableMapping = Command("ShowExternalTableMapping", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("external"),
                    EToken("table"),
                    rules.ExternalTableNameReference,
                    EToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD("MappingName", CompletionHint.Literal)}));

            var ShowExternalTablePrincipals = Command("ShowExternalTablePrincipals", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("external"),
                    EToken("table"),
                    rules.ExternalTableNameReference,
                    EToken("principals"),
                    new [] {CD(), CD(), CD(), CD("tableName", CompletionHint.ExternalTable), CD()}));

            var ShowExternalTablesPrincipalRoles = Command("ShowExternalTablesPrincipalRoles", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("external"),
                    EToken("table"),
                    rules.ExternalTableNameReference,
                    EToken("principal"),
                    Required(
                        First(
                            EToken("roles"),
                            Custom(
                                rules.StringLiteral,
                                RequiredEToken("roles"),
                                new [] {CD("Principal", CompletionHint.Literal), CD()})),
                        () => CreateMissingEToken("roles")),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD(), CD(isOptional: true)}));

            var ShowExternalTableSchema = Command("ShowExternalTableSchema", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("external"),
                    EToken("table"),
                    rules.ExternalTableNameReference,
                    EToken("schema"),
                    RequiredEToken("as"),
                    RequiredEToken("csl", "json"),
                    new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD(), CD()}));

            var ShowExternalTable = Command("ShowExternalTable", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("external"),
                    RequiredEToken("table"),
                    Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                    new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable)}));

            var ShowFabric = Command("ShowFabric", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("fabric"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    new [] {CD(), CD(), CD("id", CompletionHint.None)}));

            var ShowFollowerDatabase = Command("ShowFollowerDatabase", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("follower"),
                    Required(
                        First(
                            Custom(
                                EToken("databases"),
                                Optional(
                                    Custom(
                                        EToken("("),
                                        CommaList(
                                            Custom(
                                                If(Not(EToken(")")), rules.DatabaseNameReference),
                                                CD("databaseName", CompletionHint.Database))),
                                        RequiredEToken(")"),
                                        new [] {CD(), CD(CompletionHint.Database), CD()})),
                                new [] {CD(), CD(isOptional: true)}),
                            Custom(
                                EToken("database"),
                                Required(rules.DatabaseNameReference, rules.MissingNameReference),
                                new [] {CD(), CD("databaseName", CompletionHint.Database)})),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(isOptional: true)}, CreateMissingEToken("databases"), (SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Database), CD()}, CreateMissingEToken("("), SyntaxList<SeparatedElement<SyntaxElement>>.Empty(), CreateMissingEToken(")"))))));

            var ShowFreshness = Command("ShowFreshness", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("freshness").Hide(),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Optional(
                        First(
                            Custom(
                                EToken("column"),
                                Required(rules.ColumnNameReference, rules.MissingNameReference),
                                Optional(
                                    Custom(
                                        EToken("threshold"),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD(), CD("threshold", CompletionHint.Literal)})),
                                new [] {CD(), CD("columnName", CompletionHint.Column), CD(isOptional: true)}),
                            Custom(
                                EToken("threshold"),
                                Required(rules.Value, rules.MissingValue),
                                new [] {CD(), CD("threshold", CompletionHint.Literal)}))),
                    new [] {CD(), CD(), CD("tableName", CompletionHint.Table), CD(isOptional: true)}));

            var ShowFunctions = Command("ShowFunctions", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("functions")));

            var ShowFunctionPrincipals = Command("ShowFunctionPrincipals", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("function"),
                    rules.DatabaseFunctionNameReference,
                    EToken("principals"),
                    new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD()}));

            var ShowFunctionPrincipalRoles = Command("ShowFunctionPrincipalRoles", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("function"),
                    rules.DatabaseFunctionNameReference,
                    EToken("principal"),
                    Required(
                        First(
                            EToken("roles"),
                            Custom(
                                rules.StringLiteral,
                                RequiredEToken("roles"),
                                new [] {CD("Principal", CompletionHint.Literal), CD()})),
                        () => CreateMissingEToken("roles")),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD(), CD(), CD(isOptional: true)}));

            var ShowFunctionSchemaAsJson = Command("ShowFunctionSchemaAsJson", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("function"),
                    rules.DatabaseFunctionNameReference,
                    EToken("schema"),
                    RequiredEToken("as"),
                    RequiredEToken("json"),
                    new [] {CD(), CD(), CD("functionName", CompletionHint.Function), CD(), CD(), CD()}));

            var ShowFunction = Command("ShowFunction", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("function"),
                    Required(rules.DatabaseFunctionNameReference, rules.MissingNameReference),
                    new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function)}));

            var ShowIngestionFailures = Command("ShowIngestionFailures", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("ingestion"),
                    EToken("failures"),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            RequiredEToken("OperationId"),
                            RequiredEToken("="),
                            Required(rules.AnyGuidLiteralOrString, rules.MissingValue),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(), CD(), CD("OperationId", CompletionHint.Literal), CD()})),
                    new [] {CD(), CD(), CD(), CD(isOptional: true)}));

            var ShowIngestionMappings = Command("ShowIngestionMappings", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            EToken("cluster"),
                            EToken("ingestion")),
                        Custom(
                            EToken("cluster"),
                            RequiredEToken("ingestion")),
                        EToken("ingestion")),
                    Required(
                        First(
                            EToken("mappings"),
                            Custom(
                                EToken("apacheavro", "avro", "csv", "json", "orc", "parquet", "sstream", "w3clogfile"),
                                RequiredEToken("mappings"),
                                new [] {CD("kind"), CD()})),
                        () => CreateMissingEToken("mappings")),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD(), CD(isOptional: true)}));

            var ShowJournal = Command("ShowJournal", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("journal")));

            var ShowMaterializedViewsDetails = Command("ShowMaterializedViewsDetails", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-views"),
                    First(
                        Custom(
                            EToken("("),
                            OList(
                                primaryElementParser: Custom(
                                    rules.MaterializedViewNameReference,
                                    CD("MaterializedViewName", CompletionHint.MaterializedView)),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingNameReference,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            EToken(")"),
                            EToken("details"),
                            new [] {CD(), CD(CompletionHint.MaterializedView), CD(), CD()}),
                        Custom(
                            EToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.MaterializedViewNameReference,
                                        CD("MaterializedViewName", CompletionHint.MaterializedView)),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: rules.MissingNameReference,
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameReference()))),
                            RequiredEToken(")"),
                            RequiredEToken("details"),
                            new [] {CD(), CD(CompletionHint.MaterializedView), CD(), CD()}),
                        EToken("details"))));

            var ShowMaterializedViews = Command("ShowMaterializedViews", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-views")));

            var ShowMaterializedViewCslSchema = Command("ShowMaterializedViewCslSchema", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("cslschema"),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD()}));

            var ShowMaterializedViewDetails = Command("ShowMaterializedViewDetails", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("details"),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD()}));

            var ShowMaterializedViewDiagnostics = Command("ShowMaterializedViewDiagnostics", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("diagnostics"),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD("viewName", CompletionHint.MaterializedView), CD(), CD(isOptional: true)}));

            var ShowMaterializedViewExtents = Command("ShowMaterializedViewExtents", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("extents"),
                    Optional(
                        First(
                            Custom(
                                EToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.AnyGuidLiteralOrString,
                                            CD("ExtentId", CompletionHint.Literal)),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingValue,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingValue()))),
                                RequiredEToken(")"),
                                Optional(EToken("hot")),
                                new [] {CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}),
                            EToken("hot"))),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(isOptional: true)}));

            var ShowMaterializedViewFailures = Command("ShowMaterializedViewFailures", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("failures"),
                    new [] {CD(), CD(), CD("viewName", CompletionHint.MaterializedView), CD()}));

            var ShowMaterializedViewPolicyCaching = Command("ShowMaterializedViewPolicyCaching", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("policy"),
                    EToken("caching"),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD()}));

            var ShowMaterializedViewPolicyMerge = Command("ShowMaterializedViewPolicyMerge", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("policy"),
                    EToken("merge"),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD()}));

            var ShowMaterializedViewPolicyPartitioning = Command("ShowMaterializedViewPolicyPartitioning", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("policy"),
                    EToken("partitioning"),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD()}));

            var ShowMaterializedViewPolicyRetention = Command("ShowMaterializedViewPolicyRetention", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("policy"),
                    EToken("retention"),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD()}));

            var ShowMaterializedViewPolicyRowLevelSecurity = Command("ShowMaterializedViewPolicyRowLevelSecurity", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("policy"),
                    RequiredEToken("row_level_security"),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD()}));

            var ShowMaterializedViewPrincipals = Command("ShowMaterializedViewPrincipals", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("principals"),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD()}));

            var ShowMaterializedViewSchemaAsJson = Command("ShowMaterializedViewSchemaAsJson", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("schema"),
                    RequiredEToken("as"),
                    RequiredEToken("json"),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD()}));

            var ShowMaterializedViewStatistics = Command("ShowMaterializedViewStatistics", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("statistics"),
                    new [] {CD(), CD(), CD("viewName", CompletionHint.MaterializedView), CD()}));

            var ShowMaterializedView = Command("ShowMaterializedView", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView)}));

            var ShowMemory = Command("ShowMemory", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("memory"),
                    Optional(EToken("details")),
                    new [] {CD(), CD(), CD(isOptional: true)}));

            var ShowOperations = Command("ShowOperations", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("operations"),
                    Optional(
                        First(
                            Custom(
                                EToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.AnyGuidLiteralOrString,
                                            CD("OperationId", CompletionHint.Literal)),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingValue,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingValue()))),
                                RequiredEToken(")"),
                                new [] {CD(), CD(CompletionHint.Literal), CD()}),
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                CD("OperationId", CompletionHint.Literal)))),
                    new [] {CD(), CD(), CD(isOptional: true)}));

            var ShowOperationDetails = Command("ShowOperationDetails", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("operation"),
                    Required(rules.AnyGuidLiteralOrString, rules.MissingValue),
                    RequiredEToken("details"),
                    new [] {CD(), CD(), CD("OperationId", CompletionHint.Literal), CD()}));

            var ShowPlugins = Command("ShowPlugins", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("plugins"),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD(isOptional: true)}));

            var ShowPrincipalAccess = Command("ShowPrincipalAccess", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("principal"),
                    EToken("access"),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD(), CD(isOptional: true)}));

            var ShowPrincipalRoles = Command("ShowPrincipalRoles", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("principal"),
                    Required(
                        First(
                            EToken("roles"),
                            Custom(
                                rules.StringLiteral,
                                RequiredEToken("roles"),
                                new [] {CD("Principal", CompletionHint.Literal), CD()})),
                        () => CreateMissingEToken("roles")),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD(), CD(isOptional: true)}));

            var ShowQueries = Command("ShowQueries", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("queries")));

            var ShowQueryExecution = Command("ShowQueryExecution", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("queryexecution"),
                    Required(
                        Custom(
                            EToken("<|"),
                            Required(rules.CommandInput, rules.MissingExpression),
                            new [] {CD(), CD(CompletionHint.Tabular)}),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Tabular)}, CreateMissingEToken("<|"), rules.MissingExpression())),
                    new [] {CD(), CD(), CD("queryText")}));

            var ShowQueryPlan = Command("ShowQueryPlan", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("queryplan"),
                    Required(
                        First(
                            EToken("<|"),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            First(
                                                EToken("reconstructCsl"),
                                                If(Not(EToken("reconstructCsl")), rules.NameDeclarationOrStringLiteral)),
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("reconstructCsl"), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)}, CreateMissingEToken("reconstructCsl"), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("<|"))),
                        () => CreateMissingEToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    new [] {CD(), CD(), CD(), CD("Query", CompletionHint.Tabular)}));

            var ShowQueryCallTree = Command("ShowQueryCallTree", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("query"),
                    RequiredEToken("call-tree"),
                    Required(
                        Custom(
                            EToken("<|"),
                            Required(rules.CommandInput, rules.MissingExpression),
                            new [] {CD(), CD(CompletionHint.Tabular)}),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Tabular)}, CreateMissingEToken("<|"), rules.MissingExpression())),
                    new [] {CD(), CD(), CD(), CD("queryText")}));

            var ShowRequestSupport = Command("ShowRequestSupport", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("request_support"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("key", CompletionHint.Literal)}));

            var ShowRowStores = Command("ShowRowStores", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("rowstores")));

            var ShowRowStoreSeals = Command("ShowRowStoreSeals", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("rowstore"),
                    EToken("seals"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD(), CD("tableName", CompletionHint.Literal), CD(isOptional: true)}));

            var ShowRowStoreTransactions = Command("ShowRowStoreTransactions", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("rowstore"),
                    EToken("transactions")));

            var ShowRowStore = Command("ShowRowStore", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("rowstore"),
                    Required(If(Not(And(EToken("seals", "transactions"))), rules.NameDeclarationOrStringLiteral), rules.MissingNameDeclaration),
                    new [] {CD(), CD(), CD("rowStoreName", CompletionHint.None)}));

            var ShowRunningQueries = Command("ShowRunningQueries", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("running"),
                    RequiredEToken("queries"),
                    Optional(
                        Custom(
                            EToken("by"),
                            Required(
                                First(
                                    EToken("*"),
                                    Custom(
                                        EToken("user"),
                                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                                        new [] {CD(), CD("UserName", CompletionHint.Literal)})),
                                () => CreateMissingEToken("*")))),
                    new [] {CD(), CD(), CD(), CD(isOptional: true)}));

            var ShowSchema = Command("ShowSchema", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            EToken("cluster"),
                            EToken("schema")),
                        EToken("schema")),
                    Optional(
                        First(
                            Custom(
                                EToken("as"),
                                RequiredEToken("json")),
                            EToken("details"))),
                    new [] {CD(), CD(), CD(isOptional: true)}));

            var StoredQueryResultsShow = Command("StoredQueryResultsShow", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("stored_query_results"),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            CommaList(
                                Custom(
                                    If(Not(EToken(")")), rules.NameDeclarationOrStringLiteral),
                                    RequiredEToken("="),
                                    Required(rules.Value, rules.MissingValue),
                                    new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)})),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD(isOptional: true)}));

            var StoredQueryResultShowSchema = Command("StoredQueryResultShowSchema", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("stored_query_result"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    RequiredEToken("schema"),
                    new [] {CD(), CD(), CD("StoredQueryResultName", CompletionHint.None), CD()}));

            var ShowStreamingIngestionFailures = Command("ShowStreamingIngestionFailures", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("streamingingestion"),
                    EToken("failures")));

            var ShowStreamingIngestionStatistics = Command("ShowStreamingIngestionStatistics", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("streamingingestion"),
                    RequiredEToken("statistics")));

            var ShowTablesColumnStatistics = Command("ShowTablesColumnStatistics", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("tables"),
                    EToken("column"),
                    RequiredEToken("statistics"),
                    RequiredEToken("older"),
                    Required(rules.Value, rules.MissingValue),
                    new [] {CD(), CD(), CD(), CD(), CD(), CD("outdatewindow", CompletionHint.Literal)}));

            var ShowTablesDetails = Command("ShowTablesDetails", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("tables"),
                    First(
                        Custom(
                            EToken("("),
                            OList(
                                primaryElementParser: Custom(
                                    rules.TableNameReference,
                                    CD("TableName", CompletionHint.Table)),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingNameReference,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            EToken(")"),
                            EToken("details"),
                            new [] {CD(), CD(CompletionHint.Table), CD(), CD()}),
                        EToken("details"))));

            var ShowTables = Command("ShowTables", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("tables"),
                    Optional(
                        Custom(
                            EToken("("),
                            OList(
                                primaryElementParser: Custom(
                                    rules.TableNameReference,
                                    CD("TableName", CompletionHint.Table)),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingNameReference,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            EToken(")"),
                            new [] {CD(), CD(CompletionHint.Table), CD()})),
                    new [] {CD(), CD(), CD(isOptional: true)}));

            var ShowTableExtentsMetadata = Command("ShowTableExtentsMetadata", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            EToken("tables"),
                            EToken("("),
                            OList(
                                primaryElementParser: Custom(
                                    rules.TableNameReference,
                                    CD("TableName", CompletionHint.Table)),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingNameReference,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            EToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.Table), CD()}),
                        Custom(
                            EToken("table"),
                            If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                            new [] {CD(), CD("TableName", CompletionHint.Table)})),
                    EToken("extents"),
                    Optional(
                        First(
                            Custom(
                                EToken("("),
                                OList(
                                    primaryElementParser: Custom(
                                        rules.AnyGuidLiteralOrString,
                                        CD("ExtentId", CompletionHint.Literal)),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: rules.MissingValue,
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                EToken(")"),
                                Optional(EToken("hot")),
                                new [] {CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}),
                            EToken("hot"))),
                    EToken("metadata"),
                    Optional(
                        First(
                            Custom(
                                EToken("where"),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            EToken("tags"),
                                            RequiredEToken("!contains", "!has", "contains", "has"),
                                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                                            new [] {CD(), CD(), CD("Tag", CompletionHint.Literal)}),
                                        separatorParser: EToken("and"),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("Tag", CompletionHint.Literal)}, CreateMissingEToken("tags"), CreateMissingEToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("Tag", CompletionHint.Literal)}, CreateMissingEToken("tags"), CreateMissingEToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral())))),
                                Optional(
                                    Custom(
                                        EToken("with"),
                                        RequiredEToken("("),
                                        RequiredEToken("extentsShowFilteringRuntimePolicy"),
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        RequiredEToken(")"),
                                        new [] {CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()})),
                                new [] {CD(), CD(), CD(isOptional: true)}),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                RequiredEToken("extentsShowFilteringRuntimePolicy"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredEToken(")"),
                                new [] {CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()}))),
                    new [] {CD(), CD(), CD(), CD(isOptional: true), CD(), CD(isOptional: true)}));

            var ShowTableExtents = Command("ShowTableExtents", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            EToken("tables"),
                            EToken("("),
                            OList(
                                primaryElementParser: Custom(
                                    rules.TableNameReference,
                                    CD("TableName", CompletionHint.Table)),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingNameReference,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            EToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.Table), CD()}),
                        Custom(
                            EToken("table"),
                            If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                            new [] {CD(), CD("TableName", CompletionHint.Table)})),
                    EToken("extents"),
                    Optional(
                        First(
                            Custom(
                                EToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.AnyGuidLiteralOrString,
                                            CD("ExtentId", CompletionHint.Literal)),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingValue,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingValue()))),
                                RequiredEToken(")"),
                                Optional(EToken("hot")),
                                new [] {CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}),
                            EToken("hot"))),
                    Optional(
                        First(
                            Custom(
                                EToken("where"),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            EToken("tags"),
                                            RequiredEToken("!contains", "!has", "contains", "has"),
                                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                                            new [] {CD(), CD(), CD("Tag", CompletionHint.Literal)}),
                                        separatorParser: EToken("and"),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("Tag", CompletionHint.Literal)}, CreateMissingEToken("tags"), CreateMissingEToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("Tag", CompletionHint.Literal)}, CreateMissingEToken("tags"), CreateMissingEToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral())))),
                                Optional(
                                    Custom(
                                        EToken("with"),
                                        RequiredEToken("("),
                                        RequiredEToken("extentsShowFilteringRuntimePolicy"),
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        RequiredEToken(")"),
                                        new [] {CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()})),
                                new [] {CD(), CD(), CD(isOptional: true)}),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                RequiredEToken("extentsShowFilteringRuntimePolicy"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredEToken(")"),
                                new [] {CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()}))),
                    new [] {CD(), CD(), CD(), CD(isOptional: true), CD(isOptional: true)}));

            var TableShardGroupsStatisticsShow = Command("TableShardGroupsStatisticsShow", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            EToken("tables"),
                            EToken("("),
                            OList(
                                primaryElementParser: Custom(
                                    rules.TableNameReference,
                                    CD("TableName", CompletionHint.Table)),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingNameReference,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            EToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.Table), CD()}),
                        Custom(
                            EToken("tables"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.TableNameReference,
                                        CD("TableName", CompletionHint.Table)),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: rules.MissingNameReference,
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameReference()))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.Table), CD()}),
                        Custom(
                            EToken("table"),
                            Required(If(Not(And(EToken("*", "usage"))), rules.TableNameReference), rules.MissingNameReference),
                            new [] {CD(), CD("TableName", CompletionHint.Table)})),
                    RequiredEToken("shard-groups").Hide(),
                    RequiredEToken("statistics").Hide()));

            var ShowTableStarPolicyCaching = Command("ShowTableStarPolicyCaching", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    EToken("*"),
                    EToken("policy"),
                    EToken("caching")));

            var ShowTableStarPolicyExtentTagsRetention = Command("ShowTableStarPolicyExtentTagsRetention", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    EToken("*"),
                    EToken("policy"),
                    EToken("extent_tags_retention")));

            var ShowTableStarPolicyIngestionBatching = Command("ShowTableStarPolicyIngestionBatching", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    EToken("*"),
                    EToken("policy"),
                    EToken("ingestionbatching")));

            var ShowTableStarPolicyIngestionTime = Command("ShowTableStarPolicyIngestionTime", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    EToken("*"),
                    EToken("policy"),
                    EToken("ingestiontime")));

            var ShowTableStarPolicyMerge = Command("ShowTableStarPolicyMerge", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    EToken("*"),
                    EToken("policy"),
                    EToken("merge")));

            var ShowTableStarPolicyPartitioning = Command("ShowTableStarPolicyPartitioning", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    EToken("*"),
                    EToken("policy"),
                    EToken("partitioning")));

            var ShowTableStarPolicyRestrictedViewAccess = Command("ShowTableStarPolicyRestrictedViewAccess", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    EToken("*"),
                    EToken("policy"),
                    EToken("restricted_view_access")));

            var ShowTableStarPolicyRetention = Command("ShowTableStarPolicyRetention", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    EToken("*"),
                    EToken("policy"),
                    EToken("retention")));

            var ShowTableStarPolicyRowLevelSecurity = Command("ShowTableStarPolicyRowLevelSecurity", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    EToken("*"),
                    EToken("policy"),
                    EToken("row_level_security")));

            var ShowTableStarPolicyRowOrder = Command("ShowTableStarPolicyRowOrder", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    EToken("*"),
                    EToken("policy"),
                    EToken("roworder")));

            var ShowTableStarPolicySharding = Command("ShowTableStarPolicySharding", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    EToken("*"),
                    EToken("policy"),
                    EToken("sharding")));

            var ShowTableStarPolicyUpdate = Command("ShowTableStarPolicyUpdate", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    EToken("*"),
                    RequiredEToken("policy"),
                    RequiredEToken("update")));

            var ShowTableUsageStatisticsDetails = Command("ShowTableUsageStatisticsDetails", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    EToken("usage"),
                    EToken("statistics"),
                    EToken("details")));

            var ShowTableUsageStatistics = Command("ShowTableUsageStatistics", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    EToken("usage"),
                    RequiredEToken("statistics"),
                    Optional(
                        Custom(
                            EToken("by"),
                            Required(rules.Value, rules.MissingValue),
                            new [] {CD(), CD("partitionBy", CompletionHint.Literal)})),
                    new [] {CD(), CD(), CD(), CD(), CD(isOptional: true)}));

            var ShowTablePolicyAutoDelete = Command("ShowTablePolicyAutoDelete", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("policy"),
                    EToken("auto_delete"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var ShowTablePolicyCaching = Command("ShowTablePolicyCaching", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("policy"),
                    EToken("caching"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var ShowTablePolicyEncoding = Command("ShowTablePolicyEncoding", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("policy"),
                    EToken("encoding"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var ShowTablePolicyExtentTagsRetention = Command("ShowTablePolicyExtentTagsRetention", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("policy"),
                    EToken("extent_tags_retention"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var ShowTablePolicyIngestionBatching = Command("ShowTablePolicyIngestionBatching", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("policy"),
                    EToken("ingestionbatching"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var ShowTablePolicyMerge = Command("ShowTablePolicyMerge", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("policy"),
                    EToken("merge"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var ShowTablePolicyPartitioning = Command("ShowTablePolicyPartitioning", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("policy"),
                    EToken("partitioning"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var ShowTablePolicyRestrictedViewAccess = Command("ShowTablePolicyRestrictedViewAccess", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("policy"),
                    EToken("restricted_view_access"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var ShowTablePolicyRetention = Command("ShowTablePolicyRetention", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("policy"),
                    EToken("retention"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var ShowTablePolicyRowOrder = Command("ShowTablePolicyRowOrder", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("policy"),
                    EToken("roworder"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var ShowTablePolicySharding = Command("ShowTablePolicySharding", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("policy"),
                    EToken("sharding"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var ShowTablePolicyStreamingIngestion = Command("ShowTablePolicyStreamingIngestion", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("policy"),
                    EToken("streamingingestion"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var ShowTablePolicyUpdate = Command("ShowTablePolicyUpdate", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("policy"),
                    RequiredEToken("update"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var ShowTableRowStoreReferences = Command("ShowTableRowStoreReferences", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("rowstore_references"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD()}));

            var ShowTableRowStoreSealInfo = Command("ShowTableRowStoreSealInfo", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("rowstore_sealinfo"),
                    new [] {CD(), CD(), CD("tableName", CompletionHint.Table), CD()}));

            var ShowTableRowStores = Command("ShowTableRowStores", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    RequiredEToken("rowstores"),
                    new [] {CD(), CD(), CD("tableName", CompletionHint.Table), CD()}));

            var ShowTableColumnsClassification = Command("ShowTableColumnsClassification", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                    EToken("columns"),
                    RequiredEToken("classification"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var ShowTableColumnStatitics = Command("ShowTableColumnStatitics", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                    EToken("column"),
                    RequiredEToken("statistics"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var ShowTableCslSchema = Command("ShowTableCslSchema", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                    EToken("cslschema"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD()}));

            var ShowTableDetails = Command("ShowTableDetails", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                    EToken("details"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD()}));

            var ShowTableDimensions = Command("ShowTableDimensions", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                    EToken("dimensions"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD()}));

            var ShowTableIngestionMappings = Command("ShowTableIngestionMappings", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                    EToken("ingestion"),
                    EToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    EToken("mappings"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("MappingKind"), CD()}));

            var ShowTableIngestionMapping = Command("ShowTableIngestionMapping", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                    EToken("ingestion"),
                    RequiredEToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredEToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("MappingKind"), CD(), CD("MappingName", CompletionHint.Literal)}));

            var ShowTablePolicyIngestionTime = Command("ShowTablePolicyIngestionTime", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                    EToken("policy"),
                    EToken("ingestiontime"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var ShowTablePolicyRowLevelSecurity = Command("ShowTablePolicyRowLevelSecurity", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                    EToken("policy"),
                    RequiredEToken("row_level_security"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()}));

            var ShowTablePrincipals = Command("ShowTablePrincipals", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                    EToken("principals"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD()}));

            var ShowTablePrincipalRoles = Command("ShowTablePrincipalRoles", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                    EToken("principal"),
                    Required(
                        First(
                            EToken("roles"),
                            Custom(
                                rules.StringLiteral,
                                RequiredEToken("roles"),
                                new [] {CD("Principal", CompletionHint.Literal), CD()})),
                        () => CreateMissingEToken("roles")),
                    Optional(
                        Custom(
                            EToken("with"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)}, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            new [] {CD(), CD(), CD(CompletionHint.None), CD()})),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(isOptional: true)}));

            var ShowTableSchemaAsJson = Command("ShowTableSchemaAsJson", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                    EToken("schema"),
                    RequiredEToken("as"),
                    RequiredEToken("json"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD()}));

            var TableShardGroupsShow = Command("TableShardGroupsShow", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                    EToken("shard-groups").Hide(),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD()}));

            var ShowTable = Command("ShowTable", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    Required(If(Not(And(EToken("*", "usage"))), rules.TableNameReference), rules.MissingNameReference),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table)}));

            var ShowVersion = Command("ShowVersion", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("version")));

            var ShowWorkloadGroups = Command("ShowWorkloadGroups", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("workload_groups")));

            var ShowWorkloadGroup = Command("ShowWorkloadGroup", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    RequiredEToken("workload_group"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    new [] {CD(), CD(), CD("WorkloadGroup", CompletionHint.None)}));

            var UndoDropTable = Command("UndoDropTable", 
                Custom(
                    EToken("undo", CompletionKind.CommandPrefix),
                    RequiredEToken("drop"),
                    RequiredEToken("table"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Required(
                        First(
                            Custom(
                                EToken("as"),
                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                RequiredEToken("version"),
                                new [] {CD(), CD("TableName", CompletionHint.None), CD()}),
                            EToken("version")),
                        () => (SyntaxElement)new CustomNode(new [] {CD(), CD("TableName", CompletionHint.None), CD()}, CreateMissingEToken("as"), rules.MissingNameDeclaration(), CreateMissingEToken("version"))),
                    RequiredEToken("="),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.None), CD(), CD(), CD("Version", CompletionHint.Literal)}));

            var commandParsers = new Parser<LexicalToken, Command>[]
            {
                AddClusterBlockedPrincipals,
                AddClusterRole,
                AddDatabaseRole,
                AddExternalTableAdmins,
                AddFollowerDatabaseAuthorizedPrincipals,
                AddFunctionRole,
                AddMaterializedViewAdmins,
                AddTableRole,
                AlterMergeClusterPolicyCallout,
                AlterMergeClusterPolicyCapacity,
                AlterMergeClusterPolicyDiagnostics,
                AlterMergeClusterPolicyMultiDatabaseAdmins,
                AlterMergeClusterPolicyQueryWeakConsistency,
                AlterMergeClusterPolicyRequestClassification,
                AlterMergeClusterPolicySharding,
                AlterMergeClusterPolicyStreamingIngestion,
                AlterMergeClusterPolicyRowStore,
                AlterMergeColumnPolicyEncoding,
                AlterMergeDatabasePolicyDiagnostics,
                AlterMergeDatabasePolicyEncoding,
                AlterMergeDatabasePolicyMerge,
                AlterMergeDatabasePolicyRetention,
                AlterMergeDatabasePolicySharding,
                AlterMergeDatabasePolicyShardsGrouping,
                AlterMergeDatabasePolicyStreamingIngestion,
                AlterMergeMaterializedViewPolicyPartitioning,
                AlterMergeMaterializedViewPolicyRetention,
                AlterMergeTablePolicyEncoding,
                AlterMergeTablePolicyMerge,
                AlterMergeTablePolicyRetention,
                AlterMergeTablePolicyRowOrder,
                AlterMergeTablePolicySharding,
                AlterMergeTablePolicyStreamingIngestion,
                AlterMergeTablePolicyUpdate,
                AlterMergeTable,
                AlterMergeTableColumnDocStrings,
                AlterMergeTablePolicyPartitioning,
                AlterMergeWorkloadGroup,
                AlterCache,
                AlterClusterPolicyCaching,
                AlterClusterPolicyCallout,
                AlterClusterPolicyCapacity,
                AlterClusterPolicyDiagnostics,
                AlterClusterPolicyIngestionBatching,
                AlterClusterPolicyManagedIdentity,
                AlterClusterPolicyMultiDatabaseAdmins,
                AlterClusterPolicyQueryWeakConsistency,
                AlterClusterPolicyRequestClassification,
                AlterClusterPolicyRowStore,
                AlterClusterPolicySandbox,
                AlterClusterPolicySharding,
                AlterClusterPolicyStreamingIngestion,
                AlterClusterStorageKeys,
                AlterColumnPolicyCaching,
                AlterColumnPolicyEncodingType,
                AlterColumnPolicyEncoding,
                AlterColumnType,
                AlterDatabaseIngestionMapping,
                AlterDatabasePersistMetadata,
                AlterDatabasePolicyCaching,
                AlterDatabasePolicyDiagnostics,
                AlterDatabasePolicyEncoding,
                AlterDatabasePolicyExtentTagsRetention,
                AlterDatabasePolicyIngestionBatching,
                AlterDatabasePolicyManagedIdentity,
                AlterDatabasePolicyMerge,
                AlterDatabasePolicyRetention,
                AlterDatabasePolicySharding,
                AlterDatabasePolicyShardsGrouping,
                AlterDatabasePolicyStreamingIngestion,
                AlterDatabasePrettyName,
                AlterDatabaseStorageKeys,
                AlterExtentContainersAdd,
                AlterExtentContainersDrop,
                AlterExtentContainersRecycle,
                AlterExtentContainersSet,
                AlterExtentTagsFromQuery,
                AlterSqlExternalTable,
                AlterStorageExternalTable,
                AlterExternalTableDocString,
                AlterExternalTableFolder,
                AlterExternalTableMapping,
                AlterFollowerClusterConfiguration,
                AlterFollowerDatabaseAuthorizedPrincipals,
                AlterFollowerDatabaseConfiguration,
                AlterFollowerDatabaseChildEntities,
                AlterFollowerTablesPolicyCaching,
                AlterFunctionDocString,
                AlterFunctionFolder,
                AlterFunction,
                AlterMaterializedViewAutoUpdateSchema,
                AlterMaterializedViewDocString,
                AlterMaterializedViewFolder,
                AlterMaterializedViewLookback,
                AlterMaterializedView,
                AlterMaterializedViewPolicyCaching,
                AlterMaterializedViewPolicyPartitioning,
                AlterMaterializedViewPolicyRetention,
                AlterMaterializedViewPolicyRowLevelSecurity,
                AlterPoliciesOfRetention,
                AlterTablesPolicyCaching,
                AlterTablesPolicyIngestionBatching,
                AlterTablesPolicyIngestionTime,
                AlterTablesPolicyMerge,
                AlterTablesPolicyRestrictedViewAccess,
                AlterTablesPolicyRetention,
                AlterTablesPolicyRowOrder,
                AlterTableColumnStatisticsMethod,
                AlterTablePolicyAutoDelete,
                AlterTablePolicyCaching,
                AlterTablePolicyEncoding,
                AlterTablePolicyExtentTagsRetention,
                AlterTablePolicyIngestionBatching,
                AlterTablePolicyMerge,
                AlterTablePolicyRestrictedViewAccess,
                AlterTablePolicyRetention,
                AlterTablePolicyRowOrder,
                AlterTablePolicySharding,
                AlterTablePolicyStreamingIngestion,
                AlterTablePolicyUpdate,
                AlterTableRowStoreReferencesDisableBlockedKeys,
                AlterTableRowStoreReferencesDisableKey,
                AlterTableRowStoreReferencesDisableRowStore,
                AlterTableRowStoreReferencesDropBlockedKeys,
                AlterTableRowStoreReferencesDropKey,
                AlterTableRowStoreReferencesDropRowStore,
                AlterTable,
                AlterTableColumnDocStrings,
                AlterTableColumnsPolicyEncoding,
                AlterTableColumnStatistics,
                AlterTableDocString,
                AlterTableFolder,
                AlterTableIngestionMapping,
                AlterTablePolicyIngestionTime,
                AlterTablePolicyPartitioning,
                AlterTablePolicyRowLevelSecurity,
                AppendTable,
                AttachDatabaseMetadata,
                AttachDatabase,
                AttachExtentsIntoTableByContainer,
                AttachExtentsIntoTableByMetadata,
                CancelOperation,
                CancelQuery,
                CleanDatabaseExtentContainers,
                ClearRemoteClusterDatabaseSchema,
                ClearDatabaseCacheQueryResults,
                ClearDatabaseCacheQueryWeakConsistency,
                ClearDatabaseCacheStreamingIngestionSchema,
                ClearMaterializedViewData,
                ClearMaterializedViewStatistics,
                ClearTableCacheStreamingIngestionSchema,
                ClearTableData,
                CreateMergeTables,
                CreateMergeTable,
                CreateOrAlterContinuousExport,
                CreateOrAlterSqlExternalTable,
                CreateOrAlterStorageExternalTable,
                CreateOrAlterFunction,
                CreateOrAlterMaterializedView,
                CreateOrAleterWorkloadGroup,
                CreateBasicAuthUser,
                CreateDatabasePersist,
                CreateDatabaseVolatile,
                CreateDatabaseIngestionMapping,
                CreateSqlExternalTable,
                CreateStorageExternalTable,
                CreateExternalTableMapping,
                CreateFunction,
                CreateRequestSupport,
                CreateRowStore,
                CreateTables,
                CreateTable,
                CreateTableBasedOnAnother,
                CreateTableIngestionMapping,
                CreateTempStorage,
                CreateMaterializedView,
                DefineTables,
                DeleteClusterPolicyCaching,
                DeleteClusterPolicyCallout,
                DeleteClusterPolicyManagedIdentity,
                DeleteClusterPolicyRequestClassification,
                DeleteClusterPolicyRowStore,
                DeleteClusterPolicySandbox,
                DeleteClusterPolicySharding,
                DeleteClusterPolicyStreamingIngestion,
                DeleteColumnPolicyCaching,
                DeleteColumnPolicyEncoding,
                DeleteDatabasePolicyCaching,
                DeleteDatabasePolicyDiagnostics,
                DeleteDatabasePolicyEncoding,
                DeleteDatabasePolicyExtentTagsRetention,
                DeleteDatabasePolicyIngestionBatching,
                DeleteDatabasePolicyManagedIdentity,
                DeleteDatabasePolicyMerge,
                DeleteDatabasePolicyRetention,
                DeleteDatabasePolicySharding,
                DeleteDatabasePolicyShardsGrouping,
                DeleteDatabasePolicyStreamingIngestion,
                DropFollowerDatabasePolicyCaching,
                DropFollowerTablesPolicyCaching,
                DeleteMaterializedViewPolicyCaching,
                DeleteMaterializedViewPolicyPartitioning,
                DeleteMaterializedViewPolicyRowLevelSecurity,
                DeletePoliciesOfRetention,
                DeleteTablePolicyAutoDelete,
                DeleteTablePolicyCaching,
                DeleteTablePolicyEncoding,
                DeleteTablePolicyExtentTagsRetention,
                DeleteTablePolicyIngestionBatching,
                DeleteTablePolicyMerge,
                DeleteTablePolicyRestrictedViewAccess,
                DeleteTablePolicyRetention,
                DeleteTablePolicyRowOrder,
                DeleteTablePolicySharding,
                DeleteTablePolicyStreamingIngestion,
                DeleteTablePolicyUpdate,
                DeleteTablePolicyIngestionTime,
                DeleteTablePolicyPartitioning,
                DeleteTablePolicyRowLevelSecurity,
                DeleteTableRecords,
                DetachDatabase,
                DisableContinuousExport,
                DisableDatabaseMaintenanceMode,
                DisablePlugin,
                DropPretendExtentsByProperties,
                DropBasicAuthUser,
                DropClusterBlockedPrincipals,
                DropClusterRole,
                DropColumn,
                DropContinuousExport,
                DropDatabaseIngestionMapping,
                DropDatabasePrettyName,
                DropDatabaseRole,
                DropEmptyExtentContainers,
                DropExtentsPartitionMetadata,
                DropExtents,
                DropExtentTagsRetention,
                DropExtent,
                DropExternalTableAdmins,
                DropExternalTableMapping,
                DropExternalTable,
                DropFollowerDatabases,
                DropFollowerDatabaseAuthorizedPrincipals,
                DropFunctions,
                DropFunctionRole,
                DropFunction,
                DropMaterializedViewAdmins,
                DropMaterializedView,
                DropRowStore,
                StoredQueryResultsDrop,
                StoredQueryResultDrop,
                DropStoredQueryResultContainers,
                DropTables,
                DropTableColumns,
                DropTableIngestionMapping,
                DropTableRole,
                DropTable,
                DropTempStorage,
                DropUnusedStoredQueryResultContainers,
                DropWorkloadGroup,
                EnableContinuousExport,
                EnableDatabaseMaintenanceMode,
                EnableDisableMaterializedView,
                EnablePlugin,
                ExportToSqlTable,
                ExportToExternalTable,
                ExportToStorage,
                IngestInlineIntoTable,
                IngestIntoTable,
                MergeExtentsDryrun,
                MergeExtents,
                MoveExtentsFrom,
                MoveExtentsQuery,
                RenameColumns,
                RenameColumn,
                RenameMaterializedView,
                RenameTables,
                RenameTable,
                ReplaceExtents,
                SetOrAppendTable,
                StoredQueryResultSetOrReplace,
                SetOrReplaceTable,
                SetAccess,
                SetClusterRole,
                SetContinuousExportCursor,
                SetDatabaseRole,
                SetExternalTableAdmins,
                SetFunctionRole,
                SetMaterializedViewAdmins,
                SetMaterializedViewConcurrency,
                SetMaterializedViewCursor,
                StoredQueryResultSet,
                SetTableRowStoreReferences,
                SetTableRole,
                SetTable,
                ShowBasicAuthUsers,
                ShowCache,
                ShowCallStacks,
                ShowCapacity,
                ShowClusterAdminState,
                ShowClusterBlockedPrincipals,
                ShowClusterExtentsMetadata,
                ShowClusterExtents,
                ShowClusterJournal,
                ShowClusterMonitoring,
                ShowClusterNetwork,
                ShowClusterPendingContinuousExports,
                ShowClusterPolicyCaching,
                ShowClusterPolicyCallout,
                ShowClusterPolicyCapacity,
                ShowClusterPolicyDiagnostics,
                ShowClusterPolicyIngestionBatching,
                ShowClusterPolicyManagedIdentity,
                ShowClusterPolicyMultiDatabaseAdmins,
                ShowClusterPolicyQueryWeakConsistency,
                ShowClusterPolicyRequestClassification,
                ShowClusterPolicyRowStore,
                ShowClusterPolicySandbox,
                ShowClusterPolicySharding,
                ShowClusterPolicyStreamingIngestion,
                ShowClusterPrincipals,
                ShowClusterPrincipalRoles,
                ShowClusterSandboxesStats,
                ShowClusterScaleIn,
                ShowClusterStorageKeysHash,
                ShowCluster,
                ShowColumnPolicyCaching,
                ShowColumnPolicyEncoding,
                ShowCommandsAndQueries,
                ShowCommands,
                ShowContinuousExports,
                ShowContinuousExportExportedArtifacts,
                ShowContinuousExportFailures,
                ShowContinuousExport,
                ShowDatabasesSchemaAsJson,
                ShowDatabasesSchema,
                ShowClusterDatabasesDataStats,
                ShowClusterDatabasesDetails,
                ShowClusterDatabasesIdentity,
                ShowDatabasesManagementGroups,
                ShowClusterDatabasesPolicies,
                ShowClusterDatabases,
                ShowDatabaseCacheQueryResults,
                ShowDatabaseDataStats,
                ShowDatabaseDetails,
                ShowDatabaseExtentsMetadata,
                ShowDatabaseExtents,
                ShowDatabaseExtentTagsStatistics,
                ShowDatabaseIdentity,
                ShowDatabasePolicies,
                ShowDatabaseCslSchema,
                ShowDatabaseIngestionMappings,
                ShowDatabaseSchemaAsCslScript,
                ShowDatabaseSchemaAsJson,
                ShowDatabaseSchema,
                DatabaseShardGroupsStatisticsShow,
                ShowDatabaseExtentContainersCleanOperations,
                ShowDatabaseJournal,
                ShowDatabasePolicyCaching,
                ShowDatabasePolicyDiagnostics,
                ShowDatabasePolicyEncoding,
                ShowDatabasePolicyExtentTagsRetention,
                ShowDatabasePolicyHardRetentionViolations,
                ShowDatabasePolicyIngestionBatching,
                ShowDatabasePolicyManagedIdentity,
                ShowDatabasePolicyMerge,
                ShowDatabasePolicyRetention,
                ShowDatabasePolicySharding,
                ShowDatabasePolicyShardsGrouping,
                ShowDatabasePolicySoftRetentionViolations,
                ShowDatabasePolicyStreamingIngestion,
                ShowDatabasePrincipals,
                ShowDatabasePrincipalRoles,
                ShowDatabasePurgeOperation,
                ShowDatabaseSchemaViolations,
                ShowDatabase,
                ShowDiagnostics,
                ShowEntitySchema,
                ShowExtentContainers,
                ShowExtentColumnStorageStats,
                ShowExtentDetails,
                ShowExternalTables,
                ShowExternalTableArtifacts,
                ShowExternalTableCslSchema,
                ShowExternalTableMappings,
                ShowExternalTableMapping,
                ShowExternalTablePrincipals,
                ShowExternalTablesPrincipalRoles,
                ShowExternalTableSchema,
                ShowExternalTable,
                ShowFabric,
                ShowFollowerDatabase,
                ShowFreshness,
                ShowFunctions,
                ShowFunctionPrincipals,
                ShowFunctionPrincipalRoles,
                ShowFunctionSchemaAsJson,
                ShowFunction,
                ShowIngestionFailures,
                ShowIngestionMappings,
                ShowJournal,
                ShowMaterializedViewsDetails,
                ShowMaterializedViews,
                ShowMaterializedViewCslSchema,
                ShowMaterializedViewDetails,
                ShowMaterializedViewDiagnostics,
                ShowMaterializedViewExtents,
                ShowMaterializedViewFailures,
                ShowMaterializedViewPolicyCaching,
                ShowMaterializedViewPolicyMerge,
                ShowMaterializedViewPolicyPartitioning,
                ShowMaterializedViewPolicyRetention,
                ShowMaterializedViewPolicyRowLevelSecurity,
                ShowMaterializedViewPrincipals,
                ShowMaterializedViewSchemaAsJson,
                ShowMaterializedViewStatistics,
                ShowMaterializedView,
                ShowMemory,
                ShowOperations,
                ShowOperationDetails,
                ShowPlugins,
                ShowPrincipalAccess,
                ShowPrincipalRoles,
                ShowQueries,
                ShowQueryExecution,
                ShowQueryPlan,
                ShowQueryCallTree,
                ShowRequestSupport,
                ShowRowStores,
                ShowRowStoreSeals,
                ShowRowStoreTransactions,
                ShowRowStore,
                ShowRunningQueries,
                ShowSchema,
                StoredQueryResultsShow,
                StoredQueryResultShowSchema,
                ShowStreamingIngestionFailures,
                ShowStreamingIngestionStatistics,
                ShowTablesColumnStatistics,
                ShowTablesDetails,
                ShowTables,
                ShowTableExtentsMetadata,
                ShowTableExtents,
                TableShardGroupsStatisticsShow,
                ShowTableStarPolicyCaching,
                ShowTableStarPolicyExtentTagsRetention,
                ShowTableStarPolicyIngestionBatching,
                ShowTableStarPolicyIngestionTime,
                ShowTableStarPolicyMerge,
                ShowTableStarPolicyPartitioning,
                ShowTableStarPolicyRestrictedViewAccess,
                ShowTableStarPolicyRetention,
                ShowTableStarPolicyRowLevelSecurity,
                ShowTableStarPolicyRowOrder,
                ShowTableStarPolicySharding,
                ShowTableStarPolicyUpdate,
                ShowTableUsageStatisticsDetails,
                ShowTableUsageStatistics,
                ShowTablePolicyAutoDelete,
                ShowTablePolicyCaching,
                ShowTablePolicyEncoding,
                ShowTablePolicyExtentTagsRetention,
                ShowTablePolicyIngestionBatching,
                ShowTablePolicyMerge,
                ShowTablePolicyPartitioning,
                ShowTablePolicyRestrictedViewAccess,
                ShowTablePolicyRetention,
                ShowTablePolicyRowOrder,
                ShowTablePolicySharding,
                ShowTablePolicyStreamingIngestion,
                ShowTablePolicyUpdate,
                ShowTableRowStoreReferences,
                ShowTableRowStoreSealInfo,
                ShowTableRowStores,
                ShowTableColumnsClassification,
                ShowTableColumnStatitics,
                ShowTableCslSchema,
                ShowTableDetails,
                ShowTableDimensions,
                ShowTableIngestionMappings,
                ShowTableIngestionMapping,
                ShowTablePolicyIngestionTime,
                ShowTablePolicyRowLevelSecurity,
                ShowTablePrincipals,
                ShowTablePrincipalRoles,
                ShowTableSchemaAsJson,
                TableShardGroupsShow,
                ShowTable,
                ShowVersion,
                ShowWorkloadGroups,
                ShowWorkloadGroup,
                UndoDropTable
            };

            return commandParsers;
        }
    }
}

