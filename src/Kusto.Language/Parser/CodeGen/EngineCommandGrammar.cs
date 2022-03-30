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
// WARNING: Do not modify this file
//          This file is auto generated from the template file 'EngineCommandGrammar.tt'
//          Instead modify the corresponding input info file in the Kusto.Language.Generator project.
//          After making changes, use the right-click menu on the .tt file and select 'run custom tool'.

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
            var shape0 = CD(CompletionHint.Literal);
            var shape1 = new [] {CD(), CD("UserName", CompletionHint.Literal)};
            var shape2 = new [] {CD(), CD("AppName", CompletionHint.Literal), CD(isOptional: true)};
            var shape3 = new [] {CD(), CD("Reason", CompletionHint.Literal)};
            var shape4 = new [] {CD(), CD("Period", CompletionHint.Literal), CD(isOptional: true)};
            var shape5 = new [] {CD(), CD(), CD(), CD("Principal", CompletionHint.Literal), CD(isOptional: true), CD(isOptional: true)};
            var shape6 = new [] {CD("SkipResults"), CD(CompletionHint.Literal, isOptional: true)};
            var shape7 = new [] {CD(), CD(), CD("Role"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)};
            var shape8 = CD(CompletionHint.Database);
            var shape9 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD("Role"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)};
            var shape10 = CD(CompletionHint.ExternalTable);
            var shape11 = new [] {CD(), CD(CompletionHint.Literal, isOptional: true)};
            var shape12 = new [] {CD(), CD(), CD(), CD("externalTableName", CompletionHint.ExternalTable), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)};
            var shape13 = new [] {CD(), CD("leaderClusterMetadataPath", CompletionHint.Literal), CD("operationRole")};
            var shape14 = new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(CompletionHint.Literal, isOptional: true)};
            var shape15 = CD(CompletionHint.Function);
            var shape16 = new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD("Role"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)};
            var shape17 = CD(CompletionHint.MaterializedView);
            var shape18 = new [] {CD(), CD(), CD("materializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(CompletionHint.Literal, isOptional: true)};
            var shape19 = CD(CompletionHint.Table);
            var shape20 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD("Role"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)};
            var shape21 = new [] {CD(), CD(), CD(), CD(), CD("Policy", CompletionHint.Literal)};
            var shape22 = new [] {CD(), CD(), CD(), CD(), CD("PolicyName", CompletionHint.Literal)};
            var shape23 = new [] {CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal)};
            var shape24 = new [] {CD(), CD(), CD(), CD(), CD("RowStorePolicy", CompletionHint.Literal)};
            var shape25 = CD(CompletionHint.Column);
            var shape26 = new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD(), CD("EncodingPolicy", CompletionHint.Literal)};
            var shape27 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("PolicyName", CompletionHint.Literal)};
            var shape28 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("EncodingPolicy", CompletionHint.Literal)};
            var shape29 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("MergePolicy", CompletionHint.Literal)};
            var shape30 = new [] {CD(), CD(), CD("RecoverabilityValue")};
            var shape31 = new [] {CD(), CD(), CD("SoftDeleteValue", CompletionHint.Literal), CD(isOptional: true)};
            var shape32 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD()};
            var shape33 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("ShardingPolicy", CompletionHint.Literal)};
            var shape34 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("ShardsGroupingPolicy", CompletionHint.Literal)};
            var shape35 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("StreamingIngestionPolicy", CompletionHint.Literal)};
            var shape36 = new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD("Policy", CompletionHint.Literal)};
            var shape37 = new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD()};
            var shape38 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("EncodingPolicy", CompletionHint.Literal)};
            var shape39 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("MergePolicy", CompletionHint.Literal)};
            var shape40 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD()};
            var shape41 = new [] {CD("ColumnName", CompletionHint.Column), CD()};
            var shape42 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD(CompletionHint.Column), CD()};
            var shape43 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("ShardingPolicy", CompletionHint.Literal)};
            var shape44 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("StreamingIngestionPolicy", CompletionHint.Literal)};
            var shape45 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("UpdatePolicy", CompletionHint.Literal)};
            var shape46 = CD(CompletionHint.None);
            var shape47 = new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")};
            var shape48 = new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)};
            var shape49 = new [] {CD(), CD(), CD(CompletionHint.Table), CD(), CD(CompletionHint.None), CD(), CD(isOptional: true)};
            var shape50 = new [] {CD("ColumnName", CompletionHint.Column), CD(), CD("DocString", CompletionHint.Literal)};
            var shape51 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(CompletionHint.Column), CD()};
            var shape52 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("Policy", CompletionHint.Literal)};
            var shape53 = new [] {CD(), CD(), CD("WorkloadGroupName", CompletionHint.None), CD("WorkloadGroup", CompletionHint.Literal)};
            var shape54 = new [] {CD(), CD(), CD(), CD(), CD("Action", CompletionHint.Literal)};
            var shape55 = new [] {CD(), CD(), CD("HotData", CompletionHint.Literal), CD(), CD(), CD("HotIndex", CompletionHint.Literal)};
            var shape56 = new [] {CD(), CD(), CD("Timespan", CompletionHint.Literal)};
            var shape57 = new [] {CD(), CD(), CD(), CD(), CD("IngestionBatchingPolicy", CompletionHint.Literal)};
            var shape58 = new [] {CD(), CD(), CD(), CD(), CD("ManagedIdentityPolicy", CompletionHint.Literal)};
            var shape59 = CD(CompletionHint.Tabular);
            var shape60 = new [] {CD(), CD(), CD(), CD(), CD("Policy", CompletionHint.Literal), CD(), CD("Query", CompletionHint.Tabular)};
            var shape61 = new [] {CD(), CD(), CD(), CD(), CD("SandboxPolicy", CompletionHint.Literal)};
            var shape62 = new [] {CD(), CD(), CD(), CD(), CD("StreamingIngestionPolicy", CompletionHint.Literal)};
            var shape63 = new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)};
            var shape64 = new [] {CD(), CD(), CD(CompletionHint.None), CD(), CD()};
            var shape65 = new [] {CD(), CD(), CD(), CD(), CD(), CD("thumbprint", CompletionHint.Literal)};
            var shape66 = new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD(), CD()};
            var shape67 = new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD(), CD(), CD(), CD("EncodingPolicyType", CompletionHint.Literal)};
            var shape68 = new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD(), CD("ColumnType")};
            var shape69 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD("MappingKind"), CD(), CD("MappingName", CompletionHint.Literal), CD("MappingFormat", CompletionHint.Literal)};
            var shape70 = new [] {CD("BlobContainerUrl", CompletionHint.Literal), CD(), CD("StorageAccountKey", CompletionHint.Literal)};
            var shape71 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD(CompletionHint.Literal)};
            var shape72 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("ExtentTagsRetentionPolicy", CompletionHint.Literal)};
            var shape73 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("IngestionBatchingPolicy", CompletionHint.Literal)};
            var shape74 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("ManagedIdentityPolicy", CompletionHint.Literal)};
            var shape75 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("RetentionPolicy", CompletionHint.Literal)};
            var shape76 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD("DatabasePrettyName", CompletionHint.Literal)};
            var shape77 = new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(), CD(), CD("thumbprint", CompletionHint.Literal)};
            var shape78 = new [] {CD("hardDeletePeriod", CompletionHint.Literal), CD("containerId", CompletionHint.Literal)};
            var shape79 = new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD("container", CompletionHint.Literal), CD(CompletionHint.Literal, isOptional: true)};
            var shape80 = new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(CompletionHint.Literal, isOptional: true)};
            var shape81 = new [] {CD(), CD("hours", CompletionHint.Literal), CD()};
            var shape82 = new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD()};
            var shape83 = new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(), CD("container", CompletionHint.Literal), CD(), CD()};
            var shape84 = new [] {CD(), CD(CompletionHint.Tabular)};
            var shape85 = new [] {CD(), CD(), CD(), CD(), CD(CompletionHint.Literal), CD(), CD("csl")};
            var shape86 = new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)};
            var shape87 = new [] {CD(), CD(), CD(CompletionHint.None), CD()};
            var shape88 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD(), CD(), CD(), CD("TableKind"), CD(), CD(), CD(CompletionHint.None), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)};
            var shape89 = new [] {CD("PartitionFunction"), CD(), CD("DateTimeColumn", CompletionHint.None), CD(), CD("BinValue", CompletionHint.Literal), CD()};
            var shape90 = new [] {CD("PartitionFunction"), CD(), CD("DateTimeColumn", CompletionHint.None), CD()};
            var shape91 = new [] {CD("PartitionType"), CD(isOptional: true)};
            var shape92 = new [] {CD("PartitionType"), CD(), CD("PartitionFunction"), CD(), CD("StringColumn", CompletionHint.None), CD(), CD("HashMod", CompletionHint.Literal), CD()};
            var shape93 = new [] {CD(), CD("StringColumn", CompletionHint.None)};
            var shape94 = new [] {CD("PartitionName", CompletionHint.None), CD(), CD()};
            var shape95 = new [] {CD(), CD(), CD("DateTimeFormat", CompletionHint.Literal), CD(), CD("PartitionName", CompletionHint.None), CD()};
            var shape96 = new [] {CD("PathSeparator", CompletionHint.Literal), CD()};
            var shape97 = new [] {CD(), CD(), CD(), CD(CompletionHint.Literal), CD()};
            var shape98 = new [] {CD(), CD(), CD(), CD(CompletionHint.None), CD(), CD(isOptional: true), CD()};
            var shape99 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD(), CD(), CD(), CD("TableKind"), CD(), CD(), CD("DataFormatKind"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)};
            var shape100 = new [] {CD(), CD(), CD(), CD("tableName", CompletionHint.ExternalTable), CD(), CD("docStringValue", CompletionHint.Literal)};
            var shape101 = new [] {CD(), CD(), CD(), CD("tableName", CompletionHint.ExternalTable), CD(), CD("folderValue", CompletionHint.Literal)};
            var shape102 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD("MappingName", CompletionHint.Literal), CD("MappingFormat", CompletionHint.Literal)};
            var shape103 = new [] {CD(), CD(), CD("databaseNamePrefix", CompletionHint.None)};
            var shape104 = new [] {CD(), CD(), CD("modificationKind")};
            var shape105 = new [] {CD(), CD(), CD("followAuthorizedPrincipals", CompletionHint.Literal)};
            var shape106 = new [] {CD(), CD(), CD(), CD(), CD(), CD("leaderClusterMetadataPath", CompletionHint.Literal), CD()};
            var shape107 = new [] {CD(), CD("leaderClusterMetadataPath", CompletionHint.Literal), CD()};
            var shape108 = new [] {CD(), CD(), CD("hotDataToken", CompletionHint.Literal), CD(), CD(), CD("hotIndexToken", CompletionHint.Literal)};
            var shape109 = new [] {CD(), CD(), CD("hotToken", CompletionHint.Literal)};
            var shape110 = new [] {CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal)};
            var shape111 = new [] {CD(), CD(), CD("p", CompletionHint.Literal)};
            var shape112 = CD(isOptional: true);
            var shape113 = new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(), CD(), CD(), CD("hotWindows", isOptional: true)};
            var shape114 = new [] {CD(), CD(), CD("databaseNameOverride", CompletionHint.None)};
            var shape115 = new [] {CD(), CD("serializedDatabaseMetadataOverride", CompletionHint.Literal)};
            var shape116 = new [] {CD(), CD(), CD("prefetchExtents", CompletionHint.Literal)};
            var shape117 = new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD()};
            var shape118 = new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(), CD("entityListKind"), CD("operationName"), CD(), CD(CompletionHint.None), CD()};
            var shape119 = new [] {CD(), CD("name", CompletionHint.MaterializedView)};
            var shape120 = new [] {CD(), CD("name", CompletionHint.Table)};
            var shape121 = new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(), CD(), CD(), CD(), CD("hotWindows", isOptional: true)};
            var shape122 = new [] {CD(), CD(), CD(CompletionHint.Function), CD(), CD("Documentation", CompletionHint.Literal)};
            var shape123 = new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD(), CD("Folder", CompletionHint.Literal)};
            var shape124 = new [] {CD(), CD(), CD(CompletionHint.None), CD(), CD("FunctionName", CompletionHint.Function)};
            var shape125 = new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD()};
            var shape126 = new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD("Documentation", CompletionHint.Literal)};
            var shape127 = new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD("Folder", CompletionHint.Literal)};
            var shape128 = new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD("Lookback", CompletionHint.Literal)};
            var shape129 = new [] {CD(), CD(), CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView)};
            var shape130 = new [] {CD(), CD(), CD(), CD(), CD(), CD(CompletionHint.Table), CD()};
            var shape131 = new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD("RetentionPolicy", CompletionHint.Literal)};
            var shape132 = new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD(), CD("Query", CompletionHint.Literal)};
            var shape133 = new [] {CD(), CD("policies", CompletionHint.Literal)};
            var shape134 = new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(), CD(), CD(isOptional: true)};
            var shape135 = new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(), CD("IngestionBatchingPolicy", CompletionHint.Literal)};
            var shape136 = new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(), CD()};
            var shape137 = new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(), CD("policy", CompletionHint.Literal)};
            var shape138 = new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(), CD("RetentionPolicy", CompletionHint.Literal)};
            var shape139 = new [] {CD("ColumnName", CompletionHint.None), CD()};
            var shape140 = new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(), CD(), CD(CompletionHint.None), CD()};
            var shape141 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD(), CD("newMethod", CompletionHint.Literal)};
            var shape142 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("AutoDeletePolicy", CompletionHint.Literal)};
            var shape143 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("ExtentTagsRetentionPolicy", CompletionHint.Literal)};
            var shape144 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("IngestionBatchingPolicy", CompletionHint.Literal)};
            var shape145 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("RetentionPolicy", CompletionHint.Literal)};
            var shape146 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD(), CD(isOptional: true)};
            var shape147 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD("rowStoreKey", CompletionHint.Literal), CD(isOptional: true)};
            var shape148 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD("rowStoreName", CompletionHint.None), CD(isOptional: true)};
            var shape149 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD("EncodingPolicies", CompletionHint.Literal)};
            var shape150 = new [] {CD("c2", CompletionHint.None), CD("statisticsValues2", CompletionHint.Literal)};
            var shape151 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(CompletionHint.None)};
            var shape152 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("Documentation", CompletionHint.Literal)};
            var shape153 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("Folder", CompletionHint.Literal)};
            var shape154 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("MappingKind"), CD(), CD("MappingName", CompletionHint.Literal), CD("MappingFormat", CompletionHint.Literal)};
            var shape155 = new [] {CD(), CD(), CD(CompletionHint.None), CD(), CD("Query", CompletionHint.Literal)};
            var shape156 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD()};
            var shape157 = new [] {CD(), CD("TableName", CompletionHint.Table)};
            var shape158 = new [] {CD(), CD(), CD(), CD("QueryOrCommand", CompletionHint.Tabular)};
            var shape159 = new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(CompletionHint.Literal)};
            var shape160 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(CompletionHint.Literal)};
            var shape161 = new [] {CD(), CD(), CD(), CD(), CD("tableName", CompletionHint.Table), CD(), CD(), CD("containerUri", CompletionHint.Literal), CD(CompletionHint.Literal)};
            var shape162 = new [] {CD(), CD(), CD("tableName", CompletionHint.Table)};
            var shape163 = new [] {CD(), CD(), CD(), CD(), CD(), CD("csl")};
            var shape164 = new [] {CD(), CD(), CD("obj", CompletionHint.Literal), CD(isOptional: true)};
            var shape165 = new [] {CD(), CD(), CD("ClientRequestId", CompletionHint.Literal)};
            var shape166 = new [] {CD(), CD(CompletionHint.Database), CD()};
            var shape167 = new [] {CD(), CD(isOptional: true)};
            var shape168 = new [] {CD(), CD(), CD(isOptional: true), CD()};
            var shape169 = new [] {CD(), CD(), CD(), CD(), CD(), CD("clusterName", CompletionHint.Literal), CD(), CD(), CD(), CD(), CD("databaseName", CompletionHint.Literal), CD()};
            var shape170 = new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD()};
            var shape171 = new [] {CD(), CD(), CD("viewName", CompletionHint.MaterializedView), CD()};
            var shape172 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD()};
            var shape173 = new [] {CD("TableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD()};
            var shape174 = new [] {CD(), CD(), CD(CompletionHint.None), CD(isOptional: true)};
            var shape175 = new [] {CD(), CD(), CD("TableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD()};
            var shape176 = new [] {CD(), CD(), CD("ContinuousExportName", CompletionHint.None), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD("Query", CompletionHint.Tabular)};
            var shape177 = new [] {CD(), CD(), CD(CompletionHint.None), CD(), CD("FunctionName", CompletionHint.None)};
            var shape178 = new [] {CD(), CD("Password", CompletionHint.Literal)};
            var shape179 = new [] {CD(), CD(), CD(), CD("UserName", CompletionHint.Literal), CD(isOptional: true)};
            var shape180 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.None), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)};
            var shape181 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.None), CD(), CD(isOptional: true)};
            var shape182 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.None), CD(), CD("MappingKind"), CD(), CD("MappingName", CompletionHint.Literal), CD("MappingFormat", CompletionHint.Literal)};
            var shape183 = new [] {CD(), CD(), CD(isOptional: true), CD("FunctionName", CompletionHint.None), CD()};
            var shape184 = new [] {CD(), CD(), CD(isOptional: true)};
            var shape185 = new [] {CD(), CD(), CD("TableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD(), CD(isOptional: true)};
            var shape186 = new [] {CD(), CD(), CD("NewTableName", CompletionHint.None), CD(), CD("TableName", CompletionHint.None), CD(isOptional: true)};
            var shape187 = new [] {CD(), CD(), CD("TableName", CompletionHint.None), CD(), CD("MappingKind"), CD(), CD("MappingName", CompletionHint.Literal), CD("MappingFormat", CompletionHint.Literal)};
            var shape188 = new [] {CD(), CD(), CD(), CD(), CD("MaterializedViewName", CompletionHint.None)};
            var shape189 = new [] {CD(), CD(isOptional: true), CD(), CD(), CD(), CD(), CD(CompletionHint.Table), CD()};
            var shape190 = new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD()};
            var shape191 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD()};
            var shape192 = new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(), CD()};
            var shape193 = new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(), CD(), CD()};
            var shape194 = new [] {CD(), CD(), CD(), CD(), CD(), CD(CompletionHint.Literal), CD()};
            var shape195 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()};
            var shape196 = new [] {CD(), CD(), CD(CompletionHint.None), CD(), CD("csl")};
            var shape197 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database)};
            var shape198 = new [] {CD(), CD(), CD("ContinousExportName", CompletionHint.None)};
            var shape199 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD()};
            var shape200 = new [] {CD(), CD(), CD("pluginName", CompletionHint.Literal)};
            var shape201 = new [] {CD(), CD("Older", CompletionHint.Literal), CD(), CD()};
            var shape202 = new [] {CD(), CD("LimitCount", CompletionHint.Literal)};
            var shape203 = new [] {CD(), CD(), CD(), CD("TrimSize", CompletionHint.Literal), CD(), CD(isOptional: true)};
            var shape204 = new [] {CD(), CD(), CD(), CD(), CD(isOptional: true)};
            var shape205 = new [] {CD(), CD(), CD(), CD("UserName", CompletionHint.Literal)};
            var shape206 = new [] {CD(), CD(), CD(), CD("Principal", CompletionHint.Literal), CD(isOptional: true)};
            var shape207 = new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column)};
            var shape208 = new [] {CD(), CD(), CD("ContinuousExportName", CompletionHint.None)};
            var shape209 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD("MappingKind"), CD(), CD("MappingName", CompletionHint.Literal)};
            var shape210 = new [] {CD(), CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(), CD("d", CompletionHint.Literal), CD(isOptional: true)};
            var shape211 = new [] {CD(), CD(), CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal), CD(), CD("csl")};
            var shape212 = new [] {CD(), CD(), CD(), CD(), CD(), CD(), CD("TableName", CompletionHint.Table), CD()};
            var shape213 = new [] {CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)};
            var shape214 = new [] {CD(), CD("Query", CompletionHint.Tabular)};
            var shape215 = new [] {CD(), CD("Older", CompletionHint.Literal), CD(), CD(), CD(), CD(isOptional: true)};
            var shape216 = new [] {CD(), CD(), CD("Query", CompletionHint.Tabular)};
            var shape217 = new [] {CD(), CD(), CD("ExtentId", CompletionHint.Literal), CD(isOptional: true)};
            var shape218 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD("MappingName", CompletionHint.Literal)};
            var shape219 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable)};
            var shape220 = new [] {CD(), CD(), CD(CompletionHint.Database), CD()};
            var shape221 = new [] {CD(), CD("databaseName", CompletionHint.Database)};
            var shape222 = new [] {CD(), CD(), CD(), CD(), CD("leaderClusterMetadataPath", CompletionHint.Literal)};
            var shape223 = new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD("operationRole"), CD(), CD(CompletionHint.Literal), CD()};
            var shape224 = new [] {CD(), CD(), CD(), CD(CompletionHint.Function), CD(), CD(isOptional: true)};
            var shape225 = new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD(isOptional: true)};
            var shape226 = new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView)};
            var shape227 = new [] {CD(), CD(), CD("rowStoreName", CompletionHint.None), CD(isOptional: true)};
            var shape228 = new [] {CD(), CD(), CD(), CD(), CD("Principal", CompletionHint.Literal)};
            var shape229 = new [] {CD(), CD(), CD("StoredQueryResultName", CompletionHint.None)};
            var shape230 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(CompletionHint.Literal)};
            var shape231 = new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(isOptional: true)};
            var shape232 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("MappingKind"), CD(), CD("MappingName", CompletionHint.Literal)};
            var shape233 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(isOptional: true)};
            var shape234 = new [] {CD(), CD(), CD(), CD("olderThan", CompletionHint.Literal)};
            var shape235 = new [] {CD(), CD(), CD(), CD("databaseName", CompletionHint.Database)};
            var shape236 = new [] {CD(), CD(), CD("WorkloadGroupName", CompletionHint.None)};
            var shape237 = new [] {CD(), CD(), CD("name", CompletionHint.Literal)};
            var shape238 = new [] {CD(), CD(), CD(), CD("SqlTableName", CompletionHint.None), CD("SqlConnectionString", CompletionHint.Literal), CD(), CD("Query", CompletionHint.Tabular)};
            var shape239 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD("Query", CompletionHint.Tabular)};
            var shape240 = new [] {CD(), CD(isOptional: true), CD(), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(), CD("Query", CompletionHint.Tabular)};
            var shape241 = new [] {CD(), CD("Data", CompletionHint.None), CD()};
            var shape242 = new [] {CD(), CD("Data", CompletionHint.None)};
            var shape243 = new [] {CD(), CD(), CD(), CD(), CD(), CD("Data", CompletionHint.None)};
            var shape244 = new [] {CD(), CD(), CD(), CD(), CD("TableName", CompletionHint.None), CD()};
            var shape245 = new [] {CD(), CD(CompletionHint.Literal), CD()};
            var shape246 = new [] {CD(), CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(isOptional: true)};
            var shape247 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(CompletionHint.Literal), CD()};
            var shape248 = new [] {CD(), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)};
            var shape249 = new [] {CD(), CD(), CD(), CD(), CD(), CD("SourceTableName", CompletionHint.Table), CD(), CD(), CD("DestinationTableName", CompletionHint.Table)};
            var shape250 = new [] {CD(), CD(), CD(), CD(), CD("DestinationTableName", CompletionHint.Table), CD(), CD("Query", CompletionHint.Tabular)};
            var shape251 = new [] {CD(), CD(CompletionHint.Table), CD()};
            var shape252 = new [] {CD("NewColumnName", CompletionHint.None), CD(), CD("ColumnName", CompletionHint.Column)};
            var shape253 = new [] {CD(), CD(), CD(CompletionHint.None)};
            var shape254 = new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD("NewColumnName", CompletionHint.None)};
            var shape255 = new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD("NewMaterializedViewName", CompletionHint.None)};
            var shape256 = new [] {CD("NewTableName", CompletionHint.None), CD(), CD("TableName", CompletionHint.Table)};
            var shape257 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("NewTableName", CompletionHint.None)};
            var shape258 = new [] {CD(), CD(), CD(), CD(), CD("DestinationTableName", CompletionHint.Table), CD(), CD(), CD("ExtentsToDropQuery", CompletionHint.Tabular), CD(), CD(), CD(), CD("ExtentsToMoveQuery", CompletionHint.Tabular), CD()};
            var shape259 = new [] {CD(), CD("TableName", CompletionHint.None)};
            var shape260 = new [] {CD(), CD(), CD("StoredQueryResultName", CompletionHint.None), CD(), CD("Query", CompletionHint.Tabular)};
            var shape261 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD("AccessMode")};
            var shape262 = new [] {CD(), CD(), CD("Role"), CD()};
            var shape263 = new [] {CD(), CD(), CD("jobName", CompletionHint.None), CD(), CD(), CD("cursorValue", CompletionHint.Literal)};
            var shape264 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD("Role"), CD()};
            var shape265 = new [] {CD(), CD(), CD(), CD("externalTableName", CompletionHint.ExternalTable), CD(), CD()};
            var shape266 = new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD("Role"), CD()};
            var shape267 = new [] {CD(), CD(CompletionHint.Literal), CD(), CD(CompletionHint.Literal, isOptional: true)};
            var shape268 = new [] {CD(), CD(), CD("materializedViewName", CompletionHint.MaterializedView), CD(), CD()};
            var shape269 = new [] {CD(), CD("n", CompletionHint.Literal)};
            var shape270 = new [] {CD(), CD(), CD("viewName", CompletionHint.MaterializedView), CD(), CD(isOptional: true)};
            var shape271 = new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD("CursorValue", CompletionHint.Literal)};
            var shape272 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("references", CompletionHint.Literal)};
            var shape273 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD("Role"), CD()};
            var shape274 = new [] {CD(), CD(), CD(), CD(), CD("Scope"), CD()};
            var shape275 = new [] {CD("Resource"), CD(isOptional: true)};
            var shape276 = new [] {CD(), CD(), CD("Tag", CompletionHint.Literal)};
            var shape277 = new [] {CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()};
            var shape278 = new [] {CD(), CD(), CD(), CD(isOptional: true), CD(), CD(isOptional: true)};
            var shape279 = new [] {CD(), CD(), CD(), CD(isOptional: true), CD(isOptional: true)};
            var shape280 = new [] {CD(), CD(), CD(), CD(CompletionHint.Literal, isOptional: true)};
            var shape281 = new [] {CD("Principal", CompletionHint.Literal), CD()};
            var shape282 = new [] {CD(), CD(), CD(), CD("num", CompletionHint.Literal), CD()};
            var shape283 = new [] {CD(), CD(), CD("ColumnName"), CD(), CD()};
            var shape284 = new [] {CD(), CD(), CD("ContinuousExportName", CompletionHint.None), CD()};
            var shape285 = new [] {CD(), CD("Version", CompletionHint.Literal)};
            var shape286 = new [] {CD("DatabaseName", CompletionHint.Database), CD(isOptional: true)};
            var shape287 = new [] {CD(), CD(), CD(), CD(CompletionHint.Database), CD(), CD(), CD(), CD()};
            var shape288 = new [] {CD(), CD(), CD(), CD(CompletionHint.Database), CD(), CD(), CD(isOptional: true)};
            var shape289 = new [] {CD(), CD(CompletionHint.Database, isOptional: true)};
            var shape290 = new [] {CD(), CD(), CD(), CD(), CD("minCreationTime", CompletionHint.Literal), CD()};
            var shape291 = new [] {CD(), CD(), CD(), CD(), CD(), CD(isOptional: true)};
            var shape292 = new [] {CD("databaseName", CompletionHint.Database), CD()};
            var shape293 = new [] {CD(), CD("databaseVersion", CompletionHint.Literal)};
            var shape294 = new [] {CD(), CD(), CD(), CD(isOptional: true)};
            var shape295 = new [] {CD("kind"), CD()};
            var shape296 = new [] {CD("name", CompletionHint.Literal), CD(isOptional: true)};
            var shape297 = new [] {CD("DatabaseName", CompletionHint.Database), CD()};
            var shape298 = new [] {CD(), CD("Version", CompletionHint.Literal), CD()};
            var shape299 = new [] {CD(), CD(), CD(), CD(), CD(), CD(), CD(isOptional: true)};
            var shape300 = new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(), CD(), CD(CompletionHint.Literal, isOptional: true)};
            var shape301 = new [] {CD(), CD(), CD("DatabaseName"), CD(), CD()};
            var shape302 = new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(), CD()};
            var shape303 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD(isOptional: true)};
            var shape304 = new [] {CD(), CD("obj", CompletionHint.Literal)};
            var shape305 = new [] {CD(), CD("excludedFunctions", CompletionHint.Literal)};
            var shape306 = new [] {CD(), CD(), CD("entity", CompletionHint.None), CD(), CD(), CD(), CD(isOptional: true), CD(isOptional: true)};
            var shape307 = new [] {CD(), CD(), CD("extentId", CompletionHint.Literal), CD(), CD("columnName", CompletionHint.None), CD(), CD()};
            var shape308 = new [] {CD(), CD(CompletionHint.Literal)};
            var shape309 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD(isOptional: true)};
            var shape310 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD()};
            var shape311 = new [] {CD(), CD(), CD(), CD("tableName", CompletionHint.ExternalTable), CD()};
            var shape312 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD(), CD(isOptional: true)};
            var shape313 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD(), CD()};
            var shape314 = new [] {CD(), CD(), CD("id", CompletionHint.None)};
            var shape315 = new [] {CD(), CD("threshold", CompletionHint.Literal)};
            var shape316 = new [] {CD(), CD("columnName", CompletionHint.Column), CD(isOptional: true)};
            var shape317 = new [] {CD(), CD(), CD("tableName", CompletionHint.Table), CD(isOptional: true)};
            var shape318 = new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD()};
            var shape319 = new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD(), CD(), CD(isOptional: true)};
            var shape320 = new [] {CD(), CD(), CD("functionName", CompletionHint.Function), CD(), CD(), CD()};
            var shape321 = new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function)};
            var shape322 = new [] {CD(), CD(), CD(), CD(), CD("OperationId", CompletionHint.Literal), CD()};
            var shape323 = new [] {CD(), CD(CompletionHint.MaterializedView), CD(), CD()};
            var shape324 = new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(isOptional: true)};
            var shape325 = new [] {CD(), CD(), CD("OperationId", CompletionHint.Literal), CD()};
            var shape326 = new [] {CD(), CD(), CD("queryText")};
            var shape327 = new [] {CD(), CD(), CD(), CD("Query", CompletionHint.Tabular)};
            var shape328 = new [] {CD(), CD(), CD(), CD("queryText")};
            var shape329 = new [] {CD(), CD(), CD("key", CompletionHint.Literal)};
            var shape330 = new [] {CD(), CD(), CD(), CD("tableName", CompletionHint.Literal), CD(isOptional: true)};
            var shape331 = new [] {CD(), CD(), CD("rowStoreName", CompletionHint.None)};
            var shape332 = new [] {CD(), CD(), CD("StoredQueryResultName", CompletionHint.None), CD()};
            var shape333 = new [] {CD(), CD(), CD(), CD(), CD(), CD("outdatewindow", CompletionHint.Literal)};
            var shape334 = new [] {CD(), CD(CompletionHint.Table), CD(), CD()};
            var shape335 = new [] {CD(), CD(), CD(CompletionHint.Table), CD()};
            var shape336 = new [] {CD(), CD("partitionBy", CompletionHint.Literal)};
            var shape337 = new [] {CD(), CD(), CD("tableName", CompletionHint.Table), CD()};
            var shape338 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("MappingKind"), CD()};
            var shape339 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(isOptional: true)};
            var shape340 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table)};
            var shape341 = new [] {CD(), CD(), CD("WorkloadGroup", CompletionHint.None)};
            var shape342 = new [] {CD(), CD("TableName", CompletionHint.None), CD()};
            var shape343 = new [] {CD(), CD(), CD(), CD(CompletionHint.None), CD(), CD(), CD("Version", CompletionHint.Literal), CD(isOptional: true)};

            var AddClusterBlockedPrincipals = Command("AddClusterBlockedPrincipals", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("blockedprincipals"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        First(
                            Custom(
                                Token("application"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                Optional(
                                    Custom(
                                        Token("user"),
                                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                                        shape1)),
                                shape2),
                            Custom(
                                Token("user"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                shape1))),
                    Optional(
                        First(
                            Custom(
                                Token("period"),
                                Required(rules.Value, rules.MissingValue),
                                Optional(
                                    Custom(
                                        Token("reason"),
                                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                                        shape3)),
                                shape4),
                            Custom(
                                Token("reason"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                shape3))),
                    shape5));

            var AddClusterRole = Command("AddClusterRole", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("admins", "databasecreators", "users", "viewers"),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                shape0),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredToken(")"),
                    Optional(
                        First(
                            Custom(
                                Token("skip-results"),
                                Optional(
                                    Custom(
                                        rules.StringLiteral,
                                        shape0)),
                                shape6),
                            Custom(
                                rules.StringLiteral,
                                shape0))),
                    shape7));

            var AddDatabaseRole = Command("AddDatabaseRole", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("admins", "ingestors", "monitors", "unrestrictedviewers", "users", "viewers"),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                shape0),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredToken(")"),
                    Optional(
                        First(
                            Custom(
                                Token("skip-results"),
                                Optional(
                                    Custom(
                                        rules.StringLiteral,
                                        shape0)),
                                shape6),
                            Custom(
                                rules.StringLiteral,
                                shape0))),
                    shape9));

            var AddExternalTableAdmins = Command("AddExternalTableAdmins", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("external"),
                    RequiredToken("table"),
                    Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                    RequiredToken("admins"),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                shape0),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredToken(")"),
                    Optional(
                        First(
                            Custom(
                                Token("skip-results"),
                                Optional(
                                    Custom(
                                        rules.StringLiteral,
                                        shape0)),
                                shape11),
                            Custom(
                                rules.StringLiteral,
                                shape0))),
                    shape12));

            var AddFollowerDatabaseAuthorizedPrincipals = Command("AddFollowerDatabaseAuthorizedPrincipals", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("follower"),
                    RequiredToken("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    Required(
                        First(
                            Custom(
                                Token("from"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                RequiredToken("admins", "monitors", "unrestrictedviewers", "users", "viewers"),
                                shape13),
                            Custom(
                                Token("admins", "monitors", "unrestrictedviewers", "users", "viewers"))),
                        () => (SyntaxElement)new CustomNode(shape13, CreateMissingToken("from"), rules.MissingStringLiteral(), CreateMissingToken("Expected admins,monitors,unrestrictedviewers,users,viewers"))),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                shape0),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape0)),
                    shape14));

            var AddFunctionRole = Command("AddFunctionRole", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("function"),
                    Required(rules.DatabaseFunctionNameReference, rules.MissingNameReference),
                    RequiredToken("admins"),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                shape0),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredToken(")"),
                    Optional(
                        First(
                            Custom(
                                Token("skip-results"),
                                Optional(
                                    Custom(
                                        rules.StringLiteral,
                                        shape0)),
                                shape6),
                            Custom(
                                rules.StringLiteral,
                                shape0))),
                    shape16));

            var AddMaterializedViewAdmins = Command("AddMaterializedViewAdmins", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    RequiredToken("admins"),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                shape0),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape0)),
                    shape18));

            var AddTableRole = Command("AddTableRole", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredToken("admins", "ingestors"),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                shape0),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredToken(")"),
                    Optional(
                        First(
                            Custom(
                                Token("skip-results"),
                                Optional(
                                    Custom(
                                        rules.StringLiteral,
                                        shape0)),
                                shape6),
                            Custom(
                                rules.StringLiteral,
                                shape0))),
                    shape20));

            var AlterMergeClusterPolicyCallout = Command("AlterMergeClusterPolicyCallout", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("callout"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape21));

            var AlterMergeClusterPolicyCapacity = Command("AlterMergeClusterPolicyCapacity", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("capacity"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape21));

            var AlterMergeClusterPolicyDiagnostics = Command("AlterMergeClusterPolicyDiagnostics", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("diagnostics"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape22));

            var AlterMergeClusterPolicyMultiDatabaseAdmins = Command("AlterMergeClusterPolicyMultiDatabaseAdmins", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("multidatabaseadmins"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape21));

            var AlterMergeClusterPolicyQueryWeakConsistency = Command("AlterMergeClusterPolicyQueryWeakConsistency", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("query_weak_consistency"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape21));

            var AlterMergeClusterPolicyRequestClassification = Command("AlterMergeClusterPolicyRequestClassification", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("request_classification"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape21));

            var AlterMergeClusterPolicySharding = Command("AlterMergeClusterPolicySharding", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("sharding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape23));

            var AlterMergeClusterPolicyStreamingIngestion = Command("AlterMergeClusterPolicyStreamingIngestion", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("streamingingestion"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape23));

            var AlterMergeClusterPolicyRowStore = Command("AlterMergeClusterPolicyRowStore", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("policy"),
                    RequiredToken("rowstore"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape24));

            var AlterMergeColumnPolicyEncoding = Command("AlterMergeColumnPolicyEncoding", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("column"),
                    Required(rules.TableColumnNameReference, rules.MissingNameReference),
                    RequiredToken("policy"),
                    RequiredToken("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape26));

            var AlterMergeDatabasePolicyDiagnostics = Command("AlterMergeDatabasePolicyDiagnostics", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("diagnostics"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape27));

            var AlterMergeDatabasePolicyEncoding = Command("AlterMergeDatabasePolicyEncoding", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape28));

            var AlterMergeDatabasePolicyMerge = Command("AlterMergeDatabasePolicyMerge", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("merge"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape29));

            var AlterMergeDatabasePolicyRetention = Command("AlterMergeDatabasePolicyRetention", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("retention"),
                    Required(
                        First(
                            Custom(
                                Token("recoverability"),
                                RequiredToken("="),
                                RequiredToken("disabled", "enabled"),
                                shape30),
                            Custom(
                                Token("softdelete"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                Optional(
                                    Custom(
                                        Token("recoverability"),
                                        RequiredToken("="),
                                        RequiredToken("disabled", "enabled"),
                                        shape30)),
                                shape31),
                            Custom(
                                rules.StringLiteral,
                                shape0)),
                        () => (SyntaxElement)new CustomNode(shape30, CreateMissingToken("recoverability"), CreateMissingToken("="), CreateMissingToken("Expected disabled,enabled"))),
                    shape32));

            var AlterMergeDatabasePolicySharding = Command("AlterMergeDatabasePolicySharding", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("sharding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape33));

            var AlterMergeDatabasePolicyShardsGrouping = Command("AlterMergeDatabasePolicyShardsGrouping", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("shards_grouping").Hide(),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape34));

            var AlterMergeDatabasePolicyStreamingIngestion = Command("AlterMergeDatabasePolicyStreamingIngestion", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("policy"),
                    RequiredToken("streamingingestion"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape35));

            var AlterMergeMaterializedViewPolicyPartitioning = Command("AlterMergeMaterializedViewPolicyPartitioning", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("policy"),
                    Token("partitioning"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape36));

            var AlterMergeMaterializedViewPolicyRetention = Command("AlterMergeMaterializedViewPolicyRetention", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    RequiredToken("policy"),
                    RequiredToken("retention"),
                    Required(
                        First(
                            Custom(
                                Token("recoverability"),
                                RequiredToken("="),
                                RequiredToken("disabled", "enabled"),
                                shape30),
                            Custom(
                                Token("softdelete"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                Optional(
                                    Custom(
                                        Token("recoverability"),
                                        RequiredToken("="),
                                        RequiredToken("disabled", "enabled"),
                                        shape30)),
                                shape31),
                            Custom(
                                rules.StringLiteral,
                                shape0)),
                        () => (SyntaxElement)new CustomNode(shape30, CreateMissingToken("recoverability"), CreateMissingToken("="), CreateMissingToken("Expected disabled,enabled"))),
                    shape37));

            var AlterMergeTablePolicyEncoding = Command("AlterMergeTablePolicyEncoding", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape38));

            var AlterMergeTablePolicyMerge = Command("AlterMergeTablePolicyMerge", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("merge"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape39));

            var AlterMergeTablePolicyRetention = Command("AlterMergeTablePolicyRetention", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("retention"),
                    Required(
                        First(
                            Custom(
                                Token("recoverability"),
                                RequiredToken("="),
                                RequiredToken("disabled", "enabled"),
                                shape30),
                            Custom(
                                Token("softdelete"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                Optional(
                                    Custom(
                                        Token("recoverability"),
                                        RequiredToken("="),
                                        RequiredToken("disabled", "enabled"),
                                        shape30)),
                                shape31),
                            Custom(
                                rules.StringLiteral,
                                shape0)),
                        () => (SyntaxElement)new CustomNode(shape30, CreateMissingToken("recoverability"), CreateMissingToken("="), CreateMissingToken("Expected disabled,enabled"))),
                    shape40));

            var AlterMergeTablePolicyRowOrder = Command("AlterMergeTablePolicyRowOrder", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("roworder"),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.ColumnNameReference,
                                RequiredToken("asc", "desc"),
                                shape41),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape41, rules.MissingNameReference(), CreateMissingToken("Expected asc,desc")),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape41, rules.MissingNameReference(), CreateMissingToken("Expected asc,desc"))))),
                    RequiredToken(")"),
                    shape42));

            var AlterMergeTablePolicySharding = Command("AlterMergeTablePolicySharding", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("sharding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape43));

            var AlterMergeTablePolicyStreamingIngestion = Command("AlterMergeTablePolicyStreamingIngestion", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("streamingingestion"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape44));

            var AlterMergeTablePolicyUpdate = Command("AlterMergeTablePolicyUpdate", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    RequiredToken("policy"),
                    RequiredToken("update"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape45));

            var AlterMergeTable = Command("AlterMergeTable", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.NameDeclarationOrStringLiteral,
                                RequiredToken(":"),
                                Required(rules.Type, rules.MissingType),
                                shape47),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType())))),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        First(
                                            Token("docstring"),
                                            Token("folder"),
                                            If(Not(And(Token("docstring", "folder"))), rules.NameDeclarationOrStringLiteral)),
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape48),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingToken("docstring"), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingToken("docstring"), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"))),
                    shape49));

            var AlterMergeTableColumnDocStrings = Command("AlterMergeTableColumnDocStrings", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("column-docstrings"),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.ColumnNameReference,
                                RequiredToken(":"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                shape50),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape50, rules.MissingNameReference(), CreateMissingToken(":"), rules.MissingStringLiteral()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape50, rules.MissingNameReference(), CreateMissingToken(":"), rules.MissingStringLiteral())))),
                    RequiredToken(")"),
                    shape51));

            var AlterMergeTablePolicyPartitioning = Command("AlterMergeTablePolicyPartitioning", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredToken("policy"),
                    RequiredToken("partitioning"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape52));

            var AlterMergeWorkloadGroup = Command("AlterMergeWorkloadGroup", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    RequiredToken("workload_group"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape53));

            var AlterCache = Command("AlterCache", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cache"),
                    RequiredToken("on"),
                    Required(
                        First(
                            Token("*"),
                            Custom(
                                rules.BracketedStringLiteral,
                                shape0)),
                        () => CreateMissingToken("*")),
                    Required(rules.BracketedStringLiteral, rules.MissingStringLiteral),
                    shape54));

            var AlterClusterPolicyCaching = Command("AlterClusterPolicyCaching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("caching"),
                    Required(
                        First(
                            Custom(
                                Token("hotdata"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredToken("hotindex"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape55),
                            Custom(
                                Token("hot"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape56)),
                        () => (SyntaxElement)new CustomNode(shape55, CreateMissingToken("hotdata"), CreateMissingToken("="), rules.MissingValue(), CreateMissingToken("hotindex"), CreateMissingToken("="), rules.MissingValue()))));

            var AlterClusterPolicyCallout = Command("AlterClusterPolicyCallout", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("callout"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape21));

            var AlterClusterPolicyCapacity = Command("AlterClusterPolicyCapacity", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("capacity"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape21));

            var AlterClusterPolicyDiagnostics = Command("AlterClusterPolicyDiagnostics", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("diagnostics"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape22));

            var AlterClusterPolicyIngestionBatching = Command("AlterClusterPolicyIngestionBatching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("ingestionbatching"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape57));

            var AlterClusterPolicyManagedIdentity = Command("AlterClusterPolicyManagedIdentity", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("managed_identity"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape58));

            var AlterClusterPolicyMultiDatabaseAdmins = Command("AlterClusterPolicyMultiDatabaseAdmins", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("multidatabaseadmins"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape21));

            var AlterClusterPolicyQueryWeakConsistency = Command("AlterClusterPolicyQueryWeakConsistency", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("query_weak_consistency"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape21));

            var AlterClusterPolicyRequestClassification = Command("AlterClusterPolicyRequestClassification", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("request_classification"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    RequiredToken("<|"),
                    Required(rules.CommandInput, rules.MissingExpression),
                    shape60));

            var AlterClusterPolicyRowStore = Command("AlterClusterPolicyRowStore", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("rowstore"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape24));

            var AlterClusterPolicySandbox = Command("AlterClusterPolicySandbox", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("sandbox"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape61));

            var AlterClusterPolicySharding = Command("AlterClusterPolicySharding", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("sharding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape23));

            var AlterClusterPolicyStreamingIngestion = Command("AlterClusterPolicyStreamingIngestion", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    RequiredToken("streamingingestion"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape62));

            var AlterClusterStorageKeys = Command("AlterClusterStorageKeys", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            Token("async"),
                            Token("cluster")),
                        Custom(
                            Token("async"),
                            RequiredToken("cluster")),
                        Token("cluster")),
                    RequiredToken("storage"),
                    RequiredToken("keys"),
                    Required(
                        First(
                            Token("decryption-certificate-thumbprint"),
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape63),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                RequiredToken("decryption-certificate-thumbprint"),
                                shape64)),
                        () => CreateMissingToken("decryption-certificate-thumbprint")),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape65));

            var AlterColumnPolicyCaching = Command("AlterColumnPolicyCaching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("column"),
                    rules.DatabaseTableColumnNameReference,
                    Token("policy"),
                    Token("caching"),
                    Required(
                        First(
                            Custom(
                                Token("hotdata"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredToken("hotindex"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape55),
                            Custom(
                                Token("hot"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape56)),
                        () => (SyntaxElement)new CustomNode(shape55, CreateMissingToken("hotdata"), CreateMissingToken("="), rules.MissingValue(), CreateMissingToken("hotindex"), CreateMissingToken("="), rules.MissingValue())),
                    shape66));

            var AlterColumnPolicyEncodingType = Command("AlterColumnPolicyEncodingType", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("column"),
                    rules.DatabaseTableColumnNameReference,
                    Token("policy"),
                    Token("encoding"),
                    Token("type"),
                    RequiredToken("="),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape67));

            var AlterColumnPolicyEncoding = Command("AlterColumnPolicyEncoding", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("column"),
                    rules.DatabaseTableColumnNameReference,
                    Token("policy"),
                    RequiredToken("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape26));

            var AlterColumnType = Command("AlterColumnType", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("column"),
                    Required(rules.DatabaseTableColumnNameReference, rules.MissingNameReference),
                    RequiredToken("type"),
                    RequiredToken("="),
                    Required(rules.Type, rules.MissingType),
                    shape68));

            var AlterDatabaseIngestionMapping = Command("AlterDatabaseIngestionMapping", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("ingestion"),
                    RequiredToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape69));

            var AlterDatabasePersistMetadata = Command("AlterDatabasePersistMetadata", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("persist"),
                    RequiredToken("metadata"),
                    Required(
                        First(
                            Custom(
                                rules.StringLiteral,
                                Token(";"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                shape70),
                            Custom(
                                rules.StringLiteral,
                                shape0)),
                        () => (SyntaxElement)new CustomNode(shape70, rules.MissingStringLiteral(), CreateMissingToken(";"), rules.MissingStringLiteral())),
                    shape71));

            var AlterDatabasePolicyCaching = Command("AlterDatabasePolicyCaching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("caching"),
                    Required(
                        First(
                            Custom(
                                Token("hotdata"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredToken("hotindex"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape55),
                            Custom(
                                Token("hot"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape56)),
                        () => (SyntaxElement)new CustomNode(shape55, CreateMissingToken("hotdata"), CreateMissingToken("="), rules.MissingValue(), CreateMissingToken("hotindex"), CreateMissingToken("="), rules.MissingValue())),
                    shape32));

            var AlterDatabasePolicyDiagnostics = Command("AlterDatabasePolicyDiagnostics", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("diagnostics"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape27));

            var AlterDatabasePolicyEncoding = Command("AlterDatabasePolicyEncoding", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape28));

            var AlterDatabasePolicyExtentTagsRetention = Command("AlterDatabasePolicyExtentTagsRetention", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("extent_tags_retention"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape72));

            var AlterDatabasePolicyIngestionBatching = Command("AlterDatabasePolicyIngestionBatching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("ingestionbatching"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape73));

            var AlterDatabasePolicyManagedIdentity = Command("AlterDatabasePolicyManagedIdentity", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("managed_identity"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape74));

            var AlterDatabasePolicyMerge = Command("AlterDatabasePolicyMerge", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("merge"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape29));

            var AlterDatabasePolicyRetention = Command("AlterDatabasePolicyRetention", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("retention"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape75));

            var AlterDatabasePolicySharding = Command("AlterDatabasePolicySharding", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("sharding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape33));

            var AlterDatabasePolicyShardsGrouping = Command("AlterDatabasePolicyShardsGrouping", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("shards_grouping").Hide(),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape34));

            var AlterDatabasePolicyStreamingIngestion = Command("AlterDatabasePolicyStreamingIngestion", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    RequiredToken("streamingingestion"),
                    Required(
                        First(
                            Custom(
                                Token("disable", "enable")),
                            Custom(
                                rules.StringLiteral,
                                shape0)),
                        () => CreateMissingToken("Expected disable,enable")),
                    shape32));

            var AlterDatabasePrettyName = Command("AlterDatabasePrettyName", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("prettyname"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape76));

            var AlterDatabaseStorageKeys = Command("AlterDatabaseStorageKeys", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            Token("async"),
                            Token("database")),
                        Custom(
                            Token("async"),
                            RequiredToken("database")),
                        Token("database")),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("storage"),
                    RequiredToken("keys"),
                    Required(
                        First(
                            Token("decryption-certificate-thumbprint"),
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape63),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                RequiredToken("decryption-certificate-thumbprint"),
                                shape64)),
                        () => CreateMissingToken("decryption-certificate-thumbprint")),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape77));

            var AlterExtentContainersAdd = Command("AlterExtentContainersAdd", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("extentcontainers"),
                    rules.DatabaseNameReference,
                    Token("add"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        Custom(
                            rules.Value,
                            Required(rules.AnyGuidLiteralOrString, rules.MissingValue),
                            shape78)),
                    shape79));

            var AlterExtentContainersDrop = Command("AlterExtentContainersDrop", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("extentcontainers"),
                    rules.DatabaseNameReference,
                    Token("drop"),
                    Optional(
                        Custom(
                            rules.AnyGuidLiteralOrString,
                            shape0)),
                    shape80));

            var AlterExtentContainersRecycle = Command("AlterExtentContainersRecycle", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("extentcontainers"),
                    rules.DatabaseNameReference,
                    Token("recycle"),
                    Required(
                        First(
                            Custom(
                                Token("older"),
                                Required(
                                    First(
                                        rules.Value,
                                        rules.Value),
                                    rules.MissingValue),
                                RequiredToken("hours"),
                                shape81),
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                shape0)),
                        () => (SyntaxElement)new CustomNode(shape81, CreateMissingToken("older"), rules.MissingValue(), CreateMissingToken("hours"))),
                    shape82));

            var AlterExtentContainersSet = Command("AlterExtentContainersSet", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("extentcontainers"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("set"),
                    RequiredToken("state"),
                    Required(rules.AnyGuidLiteralOrString, rules.MissingValue),
                    RequiredToken("to"),
                    RequiredToken("readonly", "readwrite"),
                    shape83));

            var AlterExtentTagsFromQuery = Command("AlterExtentTagsFromQuery", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            Token("async"),
                            Token("extent")),
                        Token("extent")),
                    RequiredToken("tags"),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                shape0),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredToken(")"),
                    Required(
                        Custom(
                            Token("<|"),
                            Required(rules.CommandInput, rules.MissingExpression),
                            shape84),
                        () => (SyntaxElement)new CustomNode(shape84, CreateMissingToken("<|"), rules.MissingExpression())),
                    shape85));

            var AlterSqlExternalTable = Command("AlterSqlExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.NameDeclarationOrStringLiteral,
                        Token("("),
                        OList(
                            primaryElementParser: Custom(
                                rules.NameDeclarationOrStringLiteral,
                                RequiredToken(":"),
                                Required(rules.Type, rules.MissingType),
                                shape47),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        Token(")"),
                        Token("kind"),
                        RequiredToken("="),
                        RequiredToken("sql"),
                        RequiredToken("table"),
                        RequiredToken("="),
                        Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                        RequiredToken("("),
                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                        RequiredToken(")"),
                        Optional(
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape86),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                shape87))}
                    ,
                    shape88));

            var AlterStorageExternalTable = Command("AlterStorageExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.NameDeclarationOrStringLiteral,
                        RequiredToken("("),
                        Required(
                            OList(
                                primaryElementParser: Custom(
                                    rules.NameDeclarationOrStringLiteral,
                                    RequiredToken(":"),
                                    Required(rules.Type, rules.MissingType),
                                    shape47),
                                separatorParser: Token(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType()),
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType())))),
                        RequiredToken(")"),
                        RequiredToken("kind"),
                        RequiredToken("="),
                        Required(
                            First(
                                Token("adl").Hide(),
                                Token("blob").Hide(),
                                Token("storage")),
                            () => CreateMissingToken("adl")),
                        Required(
                            First(
                                Token("dataformat"),
                                Custom(
                                    Token("partition"),
                                    RequiredToken("by"),
                                    RequiredToken("("),
                                    Required(
                                        OList(
                                            primaryElementParser: Custom(
                                                rules.NameDeclarationOrStringLiteral,
                                                RequiredToken(":"),
                                                Required(
                                                    First(
                                                        Custom(
                                                            Token("datetime"),
                                                            Optional(
                                                                Custom(
                                                                    Token("="),
                                                                    Required(
                                                                        First(
                                                                            Custom(
                                                                                Token("bin"),
                                                                                RequiredToken("("),
                                                                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                                RequiredToken(","),
                                                                                Required(rules.Value, rules.MissingValue),
                                                                                RequiredToken(")"),
                                                                                shape89),
                                                                            Custom(
                                                                                Token("startofday", "startofmonth", "startofweek", "startofyear"),
                                                                                RequiredToken("("),
                                                                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                                RequiredToken(")"),
                                                                                shape90)),
                                                                        () => (SyntaxElement)new CustomNode(shape89, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(), CreateMissingToken(","), rules.MissingValue(), CreateMissingToken(")"))))),
                                                            shape91),
                                                        Custom(
                                                            Token("long"),
                                                            RequiredToken("="),
                                                            RequiredToken("hash"),
                                                            RequiredToken("("),
                                                            Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                            RequiredToken(","),
                                                            Required(rules.Value, rules.MissingValue),
                                                            RequiredToken(")"),
                                                            shape92),
                                                        Custom(
                                                            Token("string"),
                                                            Optional(
                                                                Custom(
                                                                    Token("="),
                                                                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                    shape93)),
                                                            shape91)),
                                                    () => (SyntaxElement)new CustomNode(shape91, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape89, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(), CreateMissingToken(","), rules.MissingValue(), CreateMissingToken(")"))))),
                                                shape94),
                                            separatorParser: Token(","),
                                            secondaryElementParser: null,
                                            missingPrimaryElement: null,
                                            missingSeparator: null,
                                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape94, rules.MissingNameDeclaration(), CreateMissingToken(":"), (SyntaxElement)new CustomNode(shape91, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape89, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(), CreateMissingToken(","), rules.MissingValue(), CreateMissingToken(")"))))),
                                            endOfList: null,
                                            oneOrMore: true,
                                            allowTrailingSeparator: false,
                                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape94, rules.MissingNameDeclaration(), CreateMissingToken(":"), (SyntaxElement)new CustomNode(shape91, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape89, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(), CreateMissingToken(","), rules.MissingValue(), CreateMissingToken(")")))))))),
                                    RequiredToken(")"),
                                    Optional(
                                        Custom(
                                            Token("pathformat"),
                                            RequiredToken("="),
                                            RequiredToken("("),
                                            Required(
                                                First(
                                                    Custom(
                                                        rules.StringLiteral,
                                                        Required(
                                                            List(
                                                                Custom(
                                                                    First(
                                                                        Custom(
                                                                            Token("datetime_pattern"),
                                                                            RequiredToken("("),
                                                                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                                                                            RequiredToken(","),
                                                                            Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                            RequiredToken(")"),
                                                                            shape95),
                                                                        Custom(
                                                                            If(Not(Token("datetime_pattern")), rules.NameDeclarationOrStringLiteral),
                                                                            shape46)),
                                                                    Optional(
                                                                        Custom(
                                                                            rules.StringLiteral,
                                                                            shape0)),
                                                                    shape11),
                                                                missingElement: null,
                                                                oneOrMore: true,
                                                                producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray())),
                                                            () => new SyntaxList<SyntaxElement>((SyntaxElement)new CustomNode(shape11, (SyntaxElement)new CustomNode(shape95, CreateMissingToken("datetime_pattern"), CreateMissingToken("("), rules.MissingStringLiteral(), CreateMissingToken(","), rules.MissingNameDeclaration(), CreateMissingToken(")")), rules.MissingStringLiteral()))),
                                                        shape96),
                                                    List(
                                                        Custom(
                                                            First(
                                                                Custom(
                                                                    Token("datetime_pattern"),
                                                                    RequiredToken("("),
                                                                    rules.StringLiteral,
                                                                    Token(","),
                                                                    rules.NameDeclarationOrStringLiteral,
                                                                    Token(")"),
                                                                    shape95),
                                                                Custom(
                                                                    Token("datetime_pattern"),
                                                                    RequiredToken("("),
                                                                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                                                                    RequiredToken(","),
                                                                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                    RequiredToken(")"),
                                                                    shape95),
                                                                Custom(
                                                                    If(Not(Token("datetime_pattern")), rules.NameDeclarationOrStringLiteral),
                                                                    shape46)),
                                                            Optional(
                                                                Custom(
                                                                    rules.StringLiteral,
                                                                    shape0)),
                                                            shape11),
                                                        missingElement: null,
                                                        oneOrMore: true,
                                                        producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray()))),
                                                () => (SyntaxElement)new CustomNode(shape96, rules.MissingStringLiteral(), new SyntaxList<SyntaxElement>((SyntaxElement)new CustomNode(shape11, (SyntaxElement)new CustomNode(shape95, CreateMissingToken("datetime_pattern"), CreateMissingToken("("), rules.MissingStringLiteral(), CreateMissingToken(","), rules.MissingNameDeclaration(), CreateMissingToken(")")), rules.MissingStringLiteral())))),
                                            RequiredToken(")"),
                                            shape97)),
                                    RequiredToken("dataformat"),
                                    shape98)),
                            () => CreateMissingToken("dataformat")),
                        RequiredToken("="),
                        RequiredToken("apacheavro", "avro", "csv", "json", "multijson", "orc", "parquet", "psv", "raw", "scsv", "sohsv", "sstream", "tsve", "tsv", "txt", "w3clogfile"),
                        RequiredToken("("),
                        Required(
                            OList(
                                primaryElementParser: Custom(
                                    rules.StringLiteral,
                                    shape0),
                                separatorParser: Token(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingStringLiteral,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                        RequiredToken(")"),
                        Optional(
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape86),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                shape87))}
                    ,
                    shape99));

            var AlterExternalTableDocString = Command("AlterExternalTableDocString", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    rules.ExternalTableNameReference,
                    Token("docstring"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape100));

            var AlterExternalTableFolder = Command("AlterExternalTableFolder", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    rules.ExternalTableNameReference,
                    Token("folder"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape101));

            var AlterExternalTableMapping = Command("AlterExternalTableMapping", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("external"),
                    RequiredToken("table"),
                    Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                    RequiredToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape102));

            var AlterFollowerClusterConfiguration = Command("AlterFollowerClusterConfiguration", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("follower"),
                    Token("cluster"),
                    RequiredToken("configuration"),
                    RequiredToken("from"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(
                        First(
                            Custom(
                                Token("database-name-prefix"),
                                RequiredToken("="),
                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                shape103),
                            Custom(
                                Token("default-caching-policies-modification-kind"),
                                RequiredToken("="),
                                RequiredToken("none", "replace", "union"),
                                shape104),
                            Custom(
                                Token("default-principals-modification-kind"),
                                RequiredToken("="),
                                RequiredToken("none", "replace", "union"),
                                shape104),
                            Custom(
                                Token("follow-authorized-principals"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape105)),
                        () => (SyntaxElement)new CustomNode(shape103, CreateMissingToken("database-name-prefix"), CreateMissingToken("="), rules.MissingNameDeclaration())),
                    shape106));

            var AlterFollowerDatabaseAuthorizedPrincipals = Command("AlterFollowerDatabaseAuthorizedPrincipals", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("follower"),
                    Token("database"),
                    rules.DatabaseNameReference,
                    First(
                        Custom(
                            Token("from"),
                            rules.StringLiteral,
                            Token("policy"),
                            shape107),
                        Token("policy")),
                    RequiredToken("caching"),
                    Required(
                        First(
                            Custom(
                                Token("hotdata"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredToken("hotindex"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape108),
                            Custom(
                                Token("hot"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape109)),
                        () => (SyntaxElement)new CustomNode(shape108, CreateMissingToken("hotdata"), CreateMissingToken("="), rules.MissingValue(), CreateMissingToken("hotindex"), CreateMissingToken("="), rules.MissingValue())),
                    Optional(
                        First(
                            Custom(
                                Token(","),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            Token("hot_window"),
                                            RequiredToken("="),
                                            Required(
                                                Custom(
                                                    rules.Value,
                                                    RequiredToken(".."),
                                                    Required(rules.Value, rules.MissingValue),
                                                    shape110),
                                                () => (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingToken(".."), rules.MissingValue())),
                                            shape111),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape111, CreateMissingToken("hot_window"), CreateMissingToken("="), (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingToken(".."), rules.MissingValue())),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape111, CreateMissingToken("hot_window"), CreateMissingToken("="), (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingToken(".."), rules.MissingValue())))))),
                            OList(
                                primaryElementParser: Custom(
                                    Token("hot_window"),
                                    RequiredToken("="),
                                    Required(
                                        Custom(
                                            rules.Value,
                                            RequiredToken(".."),
                                            Required(rules.Value, rules.MissingValue),
                                            shape110),
                                        () => (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingToken(".."), rules.MissingValue())),
                                    shape111),
                                separatorParser: Token(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape111, CreateMissingToken("hot_window"), CreateMissingToken("="), (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingToken(".."), rules.MissingValue())),
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)))),
                    shape113));

            var AlterFollowerDatabaseConfiguration = Command("AlterFollowerDatabaseConfiguration", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("follower"),
                    Token("database"),
                    rules.DatabaseNameReference,
                    First(
                        Custom(
                            Token("caching-policies-modification-kind"),
                            Token("="),
                            Token("none", "replace", "union"),
                            shape104),
                        Custom(
                            Token("caching-policies-modification-kind"),
                            RequiredToken("="),
                            RequiredToken("none", "replace", "union"),
                            shape104),
                        Custom(
                            Token("database-name-override"),
                            RequiredToken("="),
                            Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                            shape114),
                        Custom(
                            Token("from"),
                            rules.StringLiteral,
                            First(
                                Custom(
                                    Token("caching-policies-modification-kind"),
                                    Token("="),
                                    Token("none", "replace", "union"),
                                    shape104),
                                Custom(
                                    Token("caching-policies-modification-kind"),
                                    RequiredToken("="),
                                    RequiredToken("none", "replace", "union"),
                                    shape104),
                                Custom(
                                    Token("database-name-override"),
                                    RequiredToken("="),
                                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                    shape114),
                                Custom(
                                    Token("metadata"),
                                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                                    shape115),
                                Custom(
                                    Token("prefetch-extents"),
                                    RequiredToken("="),
                                    Required(rules.Value, rules.MissingValue),
                                    shape116),
                                Custom(
                                    Token("principals-modification-kind"),
                                    RequiredToken("="),
                                    RequiredToken("none", "replace", "union"),
                                    shape104)),
                            shape107),
                        Custom(
                            Token("metadata"),
                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                            shape115),
                        Custom(
                            Token("prefetch-extents"),
                            RequiredToken("="),
                            Required(rules.Value, rules.MissingValue),
                            shape116),
                        Custom(
                            Token("principals-modification-kind"),
                            RequiredToken("="),
                            RequiredToken("none", "replace", "union"),
                            shape104)),
                    shape117));

            var AlterFollowerDatabaseChildEntities = Command("AlterFollowerDatabaseChildEntities", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("follower"),
                        Token("database"),
                        rules.DatabaseNameReference,
                        First(
                            Custom(
                                Token("external"),
                                Token("tables")),
                            Custom(
                                Token("external"),
                                RequiredToken("tables")),
                            Custom(
                                Token("from"),
                                rules.StringLiteral,
                                First(
                                    Custom(
                                        Token("external"),
                                        Token("tables")),
                                    Custom(
                                        Token("external"),
                                        RequiredToken("tables")),
                                    Token("materialized-views"),
                                    Token("tables")),
                                shape107),
                            Token("materialized-views"),
                            Token("tables")),
                        RequiredToken("exclude", "include"),
                        RequiredToken("add", "drop"),
                        RequiredToken("("),
                        Required(
                            OList(
                                primaryElementParser: Custom(
                                    rules.WildcardedNameDeclaration,
                                    shape46),
                                separatorParser: Token(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingNameDeclaration,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration()))),
                        RequiredToken(")")}
                    ,
                    shape118));

            var AlterFollowerTablesPolicyCaching = Command("AlterFollowerTablesPolicyCaching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("follower"),
                    RequiredToken("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    Required(
                        First(
                            Custom(
                                Token("from"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                Required(
                                    First(
                                        Custom(
                                            Token("materialized-views"),
                                            RequiredToken("("),
                                            Required(
                                                OList(
                                                    primaryElementParser: Custom(
                                                        rules.NameDeclarationOrStringLiteral,
                                                        shape46),
                                                    separatorParser: Token(","),
                                                    secondaryElementParser: null,
                                                    missingPrimaryElement: null,
                                                    missingSeparator: null,
                                                    missingSecondaryElement: rules.MissingNameDeclaration,
                                                    endOfList: null,
                                                    oneOrMore: true,
                                                    allowTrailingSeparator: false,
                                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration()))),
                                            RequiredToken(")"),
                                            shape87),
                                        Custom(
                                            Token("materialized-view"),
                                            Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                                            shape119),
                                        Custom(
                                            Token("tables"),
                                            RequiredToken("("),
                                            Required(
                                                OList(
                                                    primaryElementParser: Custom(
                                                        rules.NameDeclarationOrStringLiteral,
                                                        shape46),
                                                    separatorParser: Token(","),
                                                    secondaryElementParser: null,
                                                    missingPrimaryElement: null,
                                                    missingSeparator: null,
                                                    missingSecondaryElement: rules.MissingNameDeclaration,
                                                    endOfList: null,
                                                    oneOrMore: true,
                                                    allowTrailingSeparator: false,
                                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration()))),
                                            RequiredToken(")"),
                                            shape87),
                                        Custom(
                                            Token("table"),
                                            Required(rules.TableNameReference, rules.MissingNameReference),
                                            shape120)),
                                    () => (SyntaxElement)new CustomNode(shape87, CreateMissingToken("materialized-views"), CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration())), CreateMissingToken(")"))),
                                shape107),
                            Custom(
                                Token("materialized-views"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            shape46),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingNameDeclaration,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration()))),
                                RequiredToken(")"),
                                shape87),
                            Custom(
                                Token("materialized-view"),
                                Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                                shape119),
                            Custom(
                                Token("tables"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            shape46),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingNameDeclaration,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration()))),
                                RequiredToken(")"),
                                shape87),
                            Custom(
                                Token("table"),
                                Required(rules.TableNameReference, rules.MissingNameReference),
                                shape120)),
                        () => (SyntaxElement)new CustomNode(shape107, CreateMissingToken("from"), rules.MissingStringLiteral(), (SyntaxElement)new CustomNode(shape87, CreateMissingToken("materialized-views"), CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration())), CreateMissingToken(")")))),
                    RequiredToken("policy"),
                    RequiredToken("caching"),
                    Required(
                        First(
                            Custom(
                                Token("hotdata"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredToken("hotindex"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape108),
                            Custom(
                                Token("hot"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape109)),
                        () => (SyntaxElement)new CustomNode(shape108, CreateMissingToken("hotdata"), CreateMissingToken("="), rules.MissingValue(), CreateMissingToken("hotindex"), CreateMissingToken("="), rules.MissingValue())),
                    Optional(
                        First(
                            Custom(
                                Token(","),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            Token("hot_window"),
                                            RequiredToken("="),
                                            Required(
                                                Custom(
                                                    rules.Value,
                                                    RequiredToken(".."),
                                                    Required(rules.Value, rules.MissingValue),
                                                    shape110),
                                                () => (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingToken(".."), rules.MissingValue())),
                                            shape111),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape111, CreateMissingToken("hot_window"), CreateMissingToken("="), (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingToken(".."), rules.MissingValue())),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape111, CreateMissingToken("hot_window"), CreateMissingToken("="), (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingToken(".."), rules.MissingValue())))))),
                            OList(
                                primaryElementParser: Custom(
                                    Token("hot_window"),
                                    RequiredToken("="),
                                    Required(
                                        Custom(
                                            rules.Value,
                                            RequiredToken(".."),
                                            Required(rules.Value, rules.MissingValue),
                                            shape110),
                                        () => (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingToken(".."), rules.MissingValue())),
                                    shape111),
                                separatorParser: Token(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape111, CreateMissingToken("hot_window"), CreateMissingToken("="), (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingToken(".."), rules.MissingValue())),
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)))),
                    shape121));

            var AlterFunctionDocString = Command("AlterFunctionDocString", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("function"),
                    If(Not(Token("with")), rules.DatabaseFunctionNameReference),
                    Token("docstring"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape122));

            var AlterFunctionFolder = Command("AlterFunctionFolder", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("function"),
                    If(Not(Token("with")), rules.DatabaseFunctionNameReference),
                    Token("folder"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape123));

            var AlterFunction = Command("AlterFunction", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("function"),
                    Required(
                        First(
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape86),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                Required(rules.DatabaseFunctionNameReference, rules.MissingNameReference),
                                shape124),
                            Custom(
                                If(Not(Token("with")), rules.DatabaseFunctionNameReference),
                                shape15)),
                        () => (SyntaxElement)new CustomNode(shape124, CreateMissingToken("with"), CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()))), CreateMissingToken(")"), rules.MissingNameReference())),
                    Required(rules.FunctionDeclaration, rules.MissingFunctionDeclaration)));

            var AlterMaterializedViewAutoUpdateSchema = Command("AlterMaterializedViewAutoUpdateSchema", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    If(Not(Token("with")), rules.MaterializedViewNameReference),
                    Token("autoUpdateSchema"),
                    RequiredToken("false", "true"),
                    shape125));

            var AlterMaterializedViewDocString = Command("AlterMaterializedViewDocString", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    If(Not(Token("with")), rules.MaterializedViewNameReference),
                    Token("docstring"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape126));

            var AlterMaterializedViewFolder = Command("AlterMaterializedViewFolder", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    If(Not(Token("with")), rules.MaterializedViewNameReference),
                    Token("folder"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape127));

            var AlterMaterializedViewLookback = Command("AlterMaterializedViewLookback", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    If(Not(Token("with")), rules.MaterializedViewNameReference),
                    Token("lookback"),
                    Required(rules.Value, rules.MissingValue),
                    shape128));

            var AlterMaterializedView = Command("AlterMaterializedView", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    First(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            OList(
                                primaryElementParser: Custom(
                                    First(
                                        Token("dimensionTables"),
                                        Token("lookback"),
                                        If(Not(And(Token("dimensionTables", "lookback"))), rules.NameDeclarationOrStringLiteral)),
                                    RequiredToken("="),
                                    rules.Value,
                                    shape48),
                                separatorParser: Token(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingToken("dimensionTables"), CreateMissingToken("="), rules.MissingValue()),
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            Token(")"),
                            rules.MaterializedViewNameReference,
                            shape129),
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        First(
                                            Token("dimensionTables"),
                                            Token("lookback"),
                                            If(Not(And(Token("dimensionTables", "lookback"))), rules.NameDeclarationOrStringLiteral)),
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape48),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingToken("dimensionTables"), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingToken("dimensionTables"), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                            shape129),
                        Custom(
                            If(Not(Token("with")), rules.MaterializedViewNameReference),
                            shape17)),
                    RequiredToken("on"),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Required(rules.FunctionBody, rules.MissingFunctionBody),
                    shape130));

            var AlterMaterializedViewPolicyCaching = Command("AlterMaterializedViewPolicyCaching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    If(Not(Token("with")), rules.MaterializedViewNameReference),
                    Token("policy"),
                    Token("caching"),
                    Required(
                        First(
                            Custom(
                                Token("hotdata"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredToken("hotindex"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape55),
                            Custom(
                                Token("hot"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape56)),
                        () => (SyntaxElement)new CustomNode(shape55, CreateMissingToken("hotdata"), CreateMissingToken("="), rules.MissingValue(), CreateMissingToken("hotindex"), CreateMissingToken("="), rules.MissingValue())),
                    shape37));

            var AlterMaterializedViewPolicyPartitioning = Command("AlterMaterializedViewPolicyPartitioning", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    If(Not(Token("with")), rules.MaterializedViewNameReference),
                    Token("policy"),
                    Token("partitioning"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape36));

            var AlterMaterializedViewPolicyRetention = Command("AlterMaterializedViewPolicyRetention", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    If(Not(Token("with")), rules.MaterializedViewNameReference),
                    Token("policy"),
                    Token("retention"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape131));

            var AlterMaterializedViewPolicyRowLevelSecurity = Command("AlterMaterializedViewPolicyRowLevelSecurity", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Required(If(Not(Token("with")), rules.MaterializedViewNameReference), rules.MissingNameReference),
                    RequiredToken("policy"),
                    RequiredToken("row_level_security"),
                    RequiredToken("disable", "enable"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape132));

            var AlterPoliciesOfRetention = Command("AlterPoliciesOfRetention", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("policies"),
                    RequiredToken("of"),
                    RequiredToken("retention"),
                    Required(
                        First(
                            Custom(
                                Token("internal"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                shape133),
                            Custom(
                                rules.StringLiteral,
                                shape0)),
                        () => (SyntaxElement)new CustomNode(shape133, CreateMissingToken("internal"), rules.MissingStringLiteral()))));

            var AlterTablesPolicyCaching = Command("AlterTablesPolicyCaching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("("),
                    OList(
                        primaryElementParser: Custom(
                            rules.TableNameReference,
                            shape19),
                        separatorParser: Token(","),
                        secondaryElementParser: null,
                        missingPrimaryElement: null,
                        missingSeparator: null,
                        missingSecondaryElement: rules.MissingNameReference,
                        endOfList: null,
                        oneOrMore: true,
                        allowTrailingSeparator: false,
                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                    Token(")"),
                    Token("policy"),
                    Token("caching"),
                    Required(
                        First(
                            Custom(
                                Token("hotdata"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredToken("hotindex"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape55),
                            Custom(
                                Token("hot"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape56)),
                        () => (SyntaxElement)new CustomNode(shape55, CreateMissingToken("hotdata"), CreateMissingToken("="), rules.MissingValue(), CreateMissingToken("hotindex"), CreateMissingToken("="), rules.MissingValue())),
                    Optional(
                        First(
                            Custom(
                                Token(","),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            Token("hot_window"),
                                            RequiredToken("="),
                                            Required(
                                                Custom(
                                                    rules.Value,
                                                    RequiredToken(".."),
                                                    Required(rules.Value, rules.MissingValue),
                                                    shape110),
                                                () => (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingToken(".."), rules.MissingValue())),
                                            shape111),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape111, CreateMissingToken("hot_window"), CreateMissingToken("="), (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingToken(".."), rules.MissingValue())),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape111, CreateMissingToken("hot_window"), CreateMissingToken("="), (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingToken(".."), rules.MissingValue())))))),
                            OList(
                                primaryElementParser: Custom(
                                    Token("hot_window"),
                                    RequiredToken("="),
                                    Required(
                                        Custom(
                                            rules.Value,
                                            RequiredToken(".."),
                                            Required(rules.Value, rules.MissingValue),
                                            shape110),
                                        () => (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingToken(".."), rules.MissingValue())),
                                    shape111),
                                separatorParser: Token(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape111, CreateMissingToken("hot_window"), CreateMissingToken("="), (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingToken(".."), rules.MissingValue())),
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)))),
                    shape134));

            var AlterTablesPolicyIngestionBatching = Command("AlterTablesPolicyIngestionBatching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("("),
                    OList(
                        primaryElementParser: Custom(
                            rules.TableNameReference,
                            shape19),
                        separatorParser: Token(","),
                        secondaryElementParser: null,
                        missingPrimaryElement: null,
                        missingSeparator: null,
                        missingSecondaryElement: rules.MissingNameReference,
                        endOfList: null,
                        oneOrMore: true,
                        allowTrailingSeparator: false,
                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                    Token(")"),
                    Token("policy"),
                    Token("ingestionbatching"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape135));

            var AlterTablesPolicyIngestionTime = Command("AlterTablesPolicyIngestionTime", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("("),
                    OList(
                        primaryElementParser: Custom(
                            rules.TableNameReference,
                            shape19),
                        separatorParser: Token(","),
                        secondaryElementParser: null,
                        missingPrimaryElement: null,
                        missingSeparator: null,
                        missingSecondaryElement: rules.MissingNameReference,
                        endOfList: null,
                        oneOrMore: true,
                        allowTrailingSeparator: false,
                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                    Token(")"),
                    Token("policy"),
                    Token("ingestiontime"),
                    RequiredToken("true"),
                    shape136));

            var AlterTablesPolicyMerge = Command("AlterTablesPolicyMerge", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("("),
                    OList(
                        primaryElementParser: Custom(
                            rules.TableNameReference,
                            shape19),
                        separatorParser: Token(","),
                        secondaryElementParser: null,
                        missingPrimaryElement: null,
                        missingSeparator: null,
                        missingSecondaryElement: rules.MissingNameReference,
                        endOfList: null,
                        oneOrMore: true,
                        allowTrailingSeparator: false,
                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                    Token(")"),
                    Token("policy"),
                    Token("merge"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape137));

            var AlterTablesPolicyRestrictedViewAccess = Command("AlterTablesPolicyRestrictedViewAccess", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("("),
                    OList(
                        primaryElementParser: Custom(
                            rules.TableNameReference,
                            shape19),
                        separatorParser: Token(","),
                        secondaryElementParser: null,
                        missingPrimaryElement: null,
                        missingSeparator: null,
                        missingSecondaryElement: rules.MissingNameReference,
                        endOfList: null,
                        oneOrMore: true,
                        allowTrailingSeparator: false,
                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                    Token(")"),
                    Token("policy"),
                    Token("restricted_view_access"),
                    RequiredToken("false", "true"),
                    shape136));

            var AlterTablesPolicyRetention = Command("AlterTablesPolicyRetention", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("("),
                    OList(
                        primaryElementParser: Custom(
                            rules.TableNameReference,
                            shape19),
                        separatorParser: Token(","),
                        secondaryElementParser: null,
                        missingPrimaryElement: null,
                        missingSeparator: null,
                        missingSecondaryElement: rules.MissingNameReference,
                        endOfList: null,
                        oneOrMore: true,
                        allowTrailingSeparator: false,
                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                    Token(")"),
                    Token("policy"),
                    Token("retention"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape138));

            var AlterTablesPolicyRowOrder = Command("AlterTablesPolicyRowOrder", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("tables"),
                        RequiredToken("("),
                        Required(
                            OList(
                                primaryElementParser: Custom(
                                    rules.TableNameReference,
                                    shape19),
                                separatorParser: Token(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingNameReference,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameReference()))),
                        RequiredToken(")"),
                        RequiredToken("policy"),
                        RequiredToken("roworder"),
                        RequiredToken("("),
                        Required(
                            OList(
                                primaryElementParser: Custom(
                                    rules.NameDeclarationOrStringLiteral,
                                    RequiredToken("asc", "desc"),
                                    shape139),
                                separatorParser: Token(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape139, rules.MissingNameDeclaration(), CreateMissingToken("Expected asc,desc")),
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape139, rules.MissingNameDeclaration(), CreateMissingToken("Expected asc,desc"))))),
                        RequiredToken(")")}
                    ,
                    shape140));

            var AlterTableColumnStatisticsMethod = Command("AlterTableColumnStatisticsMethod", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("column"),
                    RequiredToken("statistics"),
                    RequiredToken("method"),
                    RequiredToken("="),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape141));

            var AlterTablePolicyAutoDelete = Command("AlterTablePolicyAutoDelete", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("auto_delete"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape142));

            var AlterTablePolicyCaching = Command("AlterTablePolicyCaching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("caching"),
                    Required(
                        First(
                            Custom(
                                Token("hotdata"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredToken("hotindex"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape55),
                            Custom(
                                Token("hot"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape56)),
                        () => (SyntaxElement)new CustomNode(shape55, CreateMissingToken("hotdata"), CreateMissingToken("="), rules.MissingValue(), CreateMissingToken("hotindex"), CreateMissingToken("="), rules.MissingValue())),
                    shape40));

            var AlterTablePolicyEncoding = Command("AlterTablePolicyEncoding", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape38));

            var AlterTablePolicyExtentTagsRetention = Command("AlterTablePolicyExtentTagsRetention", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("extent_tags_retention"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape143));

            var AlterTablePolicyIngestionBatching = Command("AlterTablePolicyIngestionBatching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("ingestionbatching"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape144));

            var AlterTablePolicyMerge = Command("AlterTablePolicyMerge", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("merge"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape39));

            var AlterTablePolicyRestrictedViewAccess = Command("AlterTablePolicyRestrictedViewAccess", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("restricted_view_access"),
                    RequiredToken("false", "true"),
                    shape40));

            var AlterTablePolicyRetention = Command("AlterTablePolicyRetention", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("retention"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape145));

            var AlterTablePolicyRowOrder = Command("AlterTablePolicyRowOrder", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("roworder"),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.ColumnNameReference,
                                RequiredToken("asc", "desc"),
                                shape41),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape41, rules.MissingNameReference(), CreateMissingToken("Expected asc,desc")),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape41, rules.MissingNameReference(), CreateMissingToken("Expected asc,desc"))))),
                    RequiredToken(")"),
                    shape42));

            var AlterTablePolicySharding = Command("AlterTablePolicySharding", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("sharding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape43));

            var AlterTablePolicyStreamingIngestion = Command("AlterTablePolicyStreamingIngestion", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("streamingingestion"),
                    Required(
                        First(
                            Custom(
                                Token("disable", "enable")),
                            Custom(
                                rules.StringLiteral,
                                shape0)),
                        () => CreateMissingToken("Expected disable,enable")),
                    shape40));

            var AlterTablePolicyUpdate = Command("AlterTablePolicyUpdate", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    RequiredToken("update"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape45));

            var AlterTableRowStoreReferencesDisableBlockedKeys = Command("AlterTableRowStoreReferencesDisableBlockedKeys", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("rowstore_references"),
                    Token("disable"),
                    Token("blocked"),
                    RequiredToken("keys"),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape146));

            var AlterTableRowStoreReferencesDisableKey = Command("AlterTableRowStoreReferencesDisableKey", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("rowstore_references"),
                    Token("disable"),
                    Token("key"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape147));

            var AlterTableRowStoreReferencesDisableRowStore = Command("AlterTableRowStoreReferencesDisableRowStore", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("rowstore_references"),
                    Token("disable"),
                    RequiredToken("rowstore"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape148));

            var AlterTableRowStoreReferencesDropBlockedKeys = Command("AlterTableRowStoreReferencesDropBlockedKeys", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("rowstore_references"),
                    Token("drop"),
                    Token("blocked"),
                    RequiredToken("keys"),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape146));

            var AlterTableRowStoreReferencesDropKey = Command("AlterTableRowStoreReferencesDropKey", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("rowstore_references"),
                    Token("drop"),
                    Token("key"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape147));

            var AlterTableRowStoreReferencesDropRowStore = Command("AlterTableRowStoreReferencesDropRowStore", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    RequiredToken("rowstore_references"),
                    RequiredToken("drop"),
                    RequiredToken("rowstore"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape148));

            var AlterTable = Command("AlterTable", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.NameDeclarationOrStringLiteral,
                                RequiredToken(":"),
                                Required(rules.Type, rules.MissingType),
                                shape47),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType())))),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        First(
                                            Token("docstring"),
                                            Token("folder"),
                                            If(Not(And(Token("docstring", "folder"))), rules.NameDeclarationOrStringLiteral)),
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape48),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingToken("docstring"), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingToken("docstring"), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"))),
                    shape49));

            var AlterTableColumnDocStrings = Command("AlterTableColumnDocStrings", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("column-docstrings"),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.ColumnNameReference,
                                RequiredToken(":"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                shape50),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape50, rules.MissingNameReference(), CreateMissingToken(":"), rules.MissingStringLiteral()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape50, rules.MissingNameReference(), CreateMissingToken(":"), rules.MissingStringLiteral())))),
                    RequiredToken(")"),
                    shape51));

            var AlterTableColumnsPolicyEncoding = Command("AlterTableColumnsPolicyEncoding", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("columns"),
                    RequiredToken("policy"),
                    RequiredToken("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape149));

            var AlterTableColumnStatistics = Command("AlterTableColumnStatistics", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("column"),
                    RequiredToken("statistics"),
                    CommaList(
                        Custom(
                            rules.NameDeclarationOrStringLiteral,
                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                            shape150)),
                    shape151));

            var AlterTableDocString = Command("AlterTableDocString", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("docstring"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape152));

            var AlterTableFolder = Command("AlterTableFolder", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("folder"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape153));

            var AlterTableIngestionMapping = Command("AlterTableIngestionMapping", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("ingestion"),
                    RequiredToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape154));

            var AlterTablePolicyIngestionTime = Command("AlterTablePolicyIngestionTime", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("policy"),
                    Token("ingestiontime"),
                    RequiredToken("true"),
                    shape40));

            var AlterTablePolicyPartitioning = Command("AlterTablePolicyPartitioning", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("policy"),
                    Token("partitioning"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape52));

            var AlterTablePolicyRowLevelSecurity = Command("AlterTablePolicyRowLevelSecurity", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredToken("policy"),
                    RequiredToken("row_level_security"),
                    RequiredToken("disable", "enable"),
                    Required(
                        First(
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                CommaList(
                                    Custom(
                                        If(Not(Token(")")), rules.NameDeclarationOrStringLiteral),
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape86)),
                                RequiredToken(")"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                shape155),
                            Custom(
                                rules.StringLiteral,
                                shape0)),
                        () => (SyntaxElement)new CustomNode(shape155, CreateMissingToken("with"), CreateMissingToken("("), SyntaxList<SeparatedElement<SyntaxElement>>.Empty(), CreateMissingToken(")"), rules.MissingStringLiteral())),
                    shape156));

            var AppendTable = Command("AppendTable", 
                Custom(
                    Token("append", CompletionKind.CommandPrefix),
                    Required(
                        First(
                            Custom(
                                Token("async"),
                                Required(rules.TableNameReference, rules.MissingNameReference),
                                shape157),
                            Custom(
                                If(Not(Token("async")), rules.TableNameReference),
                                shape19)),
                        () => (SyntaxElement)new CustomNode(shape157, CreateMissingToken("async"), rules.MissingNameReference())),
                    Required(
                        First(
                            Token("<|"),
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            First(
                                                Token("creationTime"),
                                                Token("distributed"),
                                                Token("docstring"),
                                                Token("extend_schema"),
                                                Token("folder"),
                                                Token("format"),
                                                Token("ignoreFirstRecord"),
                                                Token("ingestIfNotExists"),
                                                Token("ingestionMappingReference"),
                                                Token("ingestionMapping"),
                                                Token("persistDetails"),
                                                Token("policy_ingestionTime"),
                                                Token("recreate_schema"),
                                                Token("tags"),
                                                Token("validationPolicy"),
                                                Token("zipPattern"),
                                                If(Not(And(Token("creationTime", "distributed", "docstring", "extend_schema", "folder", "format", "ignoreFirstRecord", "ingestIfNotExists", "ingestionMappingReference", "ingestionMapping", "persistDetails", "policy_ingestionTime", "recreate_schema", "tags", "validationPolicy", "zipPattern"))), rules.NameDeclarationOrStringLiteral)),
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape48),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingToken("creationTime"), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingToken("creationTime"), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                RequiredToken("<|"))),
                        () => CreateMissingToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    shape158));

            var AttachDatabaseMetadata = Command("AttachDatabaseMetadata", 
                Custom(
                    Token("attach", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("metadata"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("from"),
                    Required(
                        First(
                            Custom(
                                rules.StringLiteral,
                                Token(";"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                shape70),
                            Custom(
                                rules.StringLiteral,
                                shape0)),
                        () => (SyntaxElement)new CustomNode(shape70, rules.MissingStringLiteral(), CreateMissingToken(";"), rules.MissingStringLiteral())),
                    shape159));

            var AttachDatabase = Command("AttachDatabase", 
                Custom(
                    Token("attach", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(If(Not(Token("metadata")), rules.DatabaseNameReference), rules.MissingNameReference),
                    RequiredToken("from"),
                    Required(
                        First(
                            Custom(
                                rules.StringLiteral,
                                Token(";"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                shape70),
                            Custom(
                                rules.StringLiteral,
                                shape0)),
                        () => (SyntaxElement)new CustomNode(shape70, rules.MissingStringLiteral(), CreateMissingToken(";"), rules.MissingStringLiteral())),
                    shape160));

            var AttachExtentsIntoTableByContainer = Command("AttachExtentsIntoTableByContainer", 
                Custom(
                    Token("attach", CompletionKind.CommandPrefix),
                    Token("extents"),
                    Token("into"),
                    Token("table"),
                    rules.TableNameReference,
                    Token("by"),
                    Token("container"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(
                        List(
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                shape0),
                            missingElement: null,
                            oneOrMore: true,
                            producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray())),
                        () => new SyntaxList<SyntaxElement>(rules.MissingValue())),
                    shape161));

            var AttachExtentsIntoTableByMetadata = Command("AttachExtentsIntoTableByMetadata", 
                Custom(
                    Token("attach", CompletionKind.CommandPrefix),
                    Required(
                        First(
                            Custom(
                                Token("async"),
                                RequiredToken("extents")),
                            Token("extents")),
                        () => (SyntaxElement)new CustomNode(CreateMissingToken("async"), CreateMissingToken("extents"))),
                    List(
                        elementParser: Custom(
                            Token("into"),
                            RequiredToken("table"),
                            Required(rules.TableNameReference, rules.MissingNameReference),
                            shape162),
                        missingElement: () => (SyntaxElement)new CustomNode(shape162, CreateMissingToken("into"), CreateMissingToken("table"), rules.MissingNameReference()),
                        oneOrMore: false,
                        producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray())),
                    RequiredToken("by"),
                    RequiredToken("metadata"),
                    Required(
                        Custom(
                            Token("<|"),
                            Required(rules.CommandInput, rules.MissingExpression),
                            shape84),
                        () => (SyntaxElement)new CustomNode(shape84, CreateMissingToken("<|"), rules.MissingExpression())),
                    shape163));

            var CancelOperation = Command("CancelOperation", 
                Custom(
                    Token("cancel", CompletionKind.CommandPrefix),
                    Token("operation"),
                    Required(rules.AnyGuidLiteralOrString, rules.MissingValue),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape164));

            var CancelQuery = Command("CancelQuery", 
                Custom(
                    Token("cancel", CompletionKind.CommandPrefix),
                    RequiredToken("query"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape165));

            var CleanDatabaseExtentContainers = Command("CleanDatabaseExtentContainers", 
                Custom(
                    Token("clean", CompletionKind.CommandPrefix),
                    RequiredToken("databases"),
                    Optional(
                        First(
                            Custom(
                                Token("("),
                                CommaList(
                                    Custom(
                                        If(Not(Token(")")), rules.DatabaseNameReference),
                                        shape8)),
                                RequiredToken(")"),
                                shape166),
                            Custom(
                                Token("async"),
                                Optional(
                                    Custom(
                                        Token("("),
                                        CommaList(
                                            Custom(
                                                If(Not(Token(")")), rules.DatabaseNameReference),
                                                shape8)),
                                        RequiredToken(")"),
                                        shape166)),
                                shape167))),
                    RequiredToken("extentcontainers"),
                    shape168));

            var ClearRemoteClusterDatabaseSchema = Command("ClearRemoteClusterDatabaseSchema", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("clear", CompletionKind.CommandPrefix),
                        Token("cache"),
                        RequiredToken("remote-schema"),
                        RequiredToken("cluster"),
                        RequiredToken("("),
                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                        RequiredToken(")"),
                        RequiredToken("."),
                        RequiredToken("database"),
                        RequiredToken("("),
                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                        RequiredToken(")")}
                    ,
                    shape169));

            var ClearDatabaseCacheQueryResults = Command("ClearDatabaseCacheQueryResults", 
                Custom(
                    Token("clear", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("cache"),
                    Token("query_results")));

            var ClearDatabaseCacheQueryWeakConsistency = Command("ClearDatabaseCacheQueryWeakConsistency", 
                Custom(
                    Token("clear", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("cache"),
                    Token("query_weak_consistency")));

            var ClearDatabaseCacheStreamingIngestionSchema = Command("ClearDatabaseCacheStreamingIngestionSchema", 
                Custom(
                    Token("clear", CompletionKind.CommandPrefix),
                    Token("database"),
                    RequiredToken("cache"),
                    RequiredToken("streamingingestion"),
                    RequiredToken("schema")));

            var ClearMaterializedViewData = Command("ClearMaterializedViewData", 
                Custom(
                    Token("clear", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("data"),
                    shape170));

            var ClearMaterializedViewStatistics = Command("ClearMaterializedViewStatistics", 
                Custom(
                    Token("clear", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    RequiredToken("statistics"),
                    shape171));

            var ClearTableCacheStreamingIngestionSchema = Command("ClearTableCacheStreamingIngestionSchema", 
                Custom(
                    Token("clear", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("cache"),
                    RequiredToken("streamingingestion"),
                    RequiredToken("schema"),
                    shape40));

            var ClearTableData = Command("ClearTableData", 
                Custom(
                    Token("clear", CompletionKind.CommandPrefix),
                    Required(
                        First(
                            Custom(
                                Token("async"),
                                RequiredToken("table")),
                            Token("table")),
                        () => (SyntaxElement)new CustomNode(CreateMissingToken("async"), CreateMissingToken("table"))),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredToken("data"),
                    shape172));

            var CreateMergeTables = Command("CreateMergeTables", 
                Custom(
                    Token("create-merge", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.NameDeclarationOrStringLiteral,
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredToken(":"),
                                            Required(rules.Type, rules.MissingType),
                                            shape47),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType())))),
                                RequiredToken(")"),
                                shape173),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape173, rules.MissingNameDeclaration(), CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType()))), CreateMissingToken(")")),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape173, rules.MissingNameDeclaration(), CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType()))), CreateMissingToken(")"))))),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape174));

            var CreateMergeTable = Command("CreateMergeTable", 
                Custom(
                    Token("create-merge", CompletionKind.CommandPrefix),
                    RequiredToken("table"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.NameDeclarationOrStringLiteral,
                                RequiredToken(":"),
                                Required(rules.Type, rules.MissingType),
                                shape47),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType())))),
                    RequiredToken(")"),
                    shape175));

            var CreateOrAlterContinuousExport = Command("CreateOrAlterContinuousExport", 
                Custom(
                    Token("create-or-alter", CompletionKind.CommandPrefix),
                    Token("continuous-export"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Required(
                        First(
                            Custom(
                                Token("over"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            shape46),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingNameDeclaration,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration()))),
                                RequiredToken(")"),
                                RequiredToken("to"),
                                shape64),
                            Token("to")),
                        () => (SyntaxElement)new CustomNode(shape64, CreateMissingToken("over"), CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration())), CreateMissingToken(")"), CreateMissingToken("to"))),
                    RequiredToken("table"),
                    Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                    Required(
                        First(
                            Token("<|"),
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape63),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                RequiredToken("<|"),
                                shape64)),
                        () => CreateMissingToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    shape176));

            var CreateOrAlterSqlExternalTable = Command("CreateOrAlterSqlExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("create-or-alter", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.NameDeclarationOrStringLiteral,
                        Token("("),
                        OList(
                            primaryElementParser: Custom(
                                rules.NameDeclarationOrStringLiteral,
                                RequiredToken(":"),
                                Required(rules.Type, rules.MissingType),
                                shape47),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        Token(")"),
                        Token("kind"),
                        RequiredToken("="),
                        RequiredToken("sql"),
                        RequiredToken("table"),
                        RequiredToken("="),
                        Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                        RequiredToken("("),
                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                        RequiredToken(")"),
                        Optional(
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape86),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                shape87))}
                    ,
                    shape88));

            var CreateOrAlterStorageExternalTable = Command("CreateOrAlterStorageExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("create-or-alter", CompletionKind.CommandPrefix),
                        Token("external"),
                        RequiredToken("table"),
                        Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                        RequiredToken("("),
                        Required(
                            OList(
                                primaryElementParser: Custom(
                                    rules.NameDeclarationOrStringLiteral,
                                    RequiredToken(":"),
                                    Required(rules.Type, rules.MissingType),
                                    shape47),
                                separatorParser: Token(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType()),
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType())))),
                        RequiredToken(")"),
                        RequiredToken("kind"),
                        RequiredToken("="),
                        Required(
                            First(
                                Token("adl").Hide(),
                                Token("blob").Hide(),
                                Token("storage")),
                            () => CreateMissingToken("adl")),
                        Required(
                            First(
                                Token("dataformat"),
                                Custom(
                                    Token("partition"),
                                    RequiredToken("by"),
                                    RequiredToken("("),
                                    Required(
                                        OList(
                                            primaryElementParser: Custom(
                                                rules.NameDeclarationOrStringLiteral,
                                                RequiredToken(":"),
                                                Required(
                                                    First(
                                                        Custom(
                                                            Token("datetime"),
                                                            Optional(
                                                                Custom(
                                                                    Token("="),
                                                                    Required(
                                                                        First(
                                                                            Custom(
                                                                                Token("bin"),
                                                                                RequiredToken("("),
                                                                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                                RequiredToken(","),
                                                                                Required(rules.Value, rules.MissingValue),
                                                                                RequiredToken(")"),
                                                                                shape89),
                                                                            Custom(
                                                                                Token("startofday", "startofmonth", "startofweek", "startofyear"),
                                                                                RequiredToken("("),
                                                                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                                RequiredToken(")"),
                                                                                shape90)),
                                                                        () => (SyntaxElement)new CustomNode(shape89, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(), CreateMissingToken(","), rules.MissingValue(), CreateMissingToken(")"))))),
                                                            shape91),
                                                        Custom(
                                                            Token("long"),
                                                            RequiredToken("="),
                                                            RequiredToken("hash"),
                                                            RequiredToken("("),
                                                            Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                            RequiredToken(","),
                                                            Required(rules.Value, rules.MissingValue),
                                                            RequiredToken(")"),
                                                            shape92),
                                                        Custom(
                                                            Token("string"),
                                                            Optional(
                                                                Custom(
                                                                    Token("="),
                                                                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                    shape93)),
                                                            shape91)),
                                                    () => (SyntaxElement)new CustomNode(shape91, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape89, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(), CreateMissingToken(","), rules.MissingValue(), CreateMissingToken(")"))))),
                                                shape94),
                                            separatorParser: Token(","),
                                            secondaryElementParser: null,
                                            missingPrimaryElement: null,
                                            missingSeparator: null,
                                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape94, rules.MissingNameDeclaration(), CreateMissingToken(":"), (SyntaxElement)new CustomNode(shape91, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape89, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(), CreateMissingToken(","), rules.MissingValue(), CreateMissingToken(")"))))),
                                            endOfList: null,
                                            oneOrMore: true,
                                            allowTrailingSeparator: false,
                                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape94, rules.MissingNameDeclaration(), CreateMissingToken(":"), (SyntaxElement)new CustomNode(shape91, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape89, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(), CreateMissingToken(","), rules.MissingValue(), CreateMissingToken(")")))))))),
                                    RequiredToken(")"),
                                    Optional(
                                        Custom(
                                            Token("pathformat"),
                                            RequiredToken("="),
                                            RequiredToken("("),
                                            Required(
                                                First(
                                                    Custom(
                                                        rules.StringLiteral,
                                                        Required(
                                                            List(
                                                                Custom(
                                                                    First(
                                                                        Custom(
                                                                            Token("datetime_pattern"),
                                                                            RequiredToken("("),
                                                                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                                                                            RequiredToken(","),
                                                                            Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                            RequiredToken(")"),
                                                                            shape95),
                                                                        Custom(
                                                                            If(Not(Token("datetime_pattern")), rules.NameDeclarationOrStringLiteral),
                                                                            shape46)),
                                                                    Optional(
                                                                        Custom(
                                                                            rules.StringLiteral,
                                                                            shape0)),
                                                                    shape11),
                                                                missingElement: null,
                                                                oneOrMore: true,
                                                                producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray())),
                                                            () => new SyntaxList<SyntaxElement>((SyntaxElement)new CustomNode(shape11, (SyntaxElement)new CustomNode(shape95, CreateMissingToken("datetime_pattern"), CreateMissingToken("("), rules.MissingStringLiteral(), CreateMissingToken(","), rules.MissingNameDeclaration(), CreateMissingToken(")")), rules.MissingStringLiteral()))),
                                                        shape96),
                                                    List(
                                                        Custom(
                                                            First(
                                                                Custom(
                                                                    Token("datetime_pattern"),
                                                                    RequiredToken("("),
                                                                    rules.StringLiteral,
                                                                    Token(","),
                                                                    rules.NameDeclarationOrStringLiteral,
                                                                    Token(")"),
                                                                    shape95),
                                                                Custom(
                                                                    Token("datetime_pattern"),
                                                                    RequiredToken("("),
                                                                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                                                                    RequiredToken(","),
                                                                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                    RequiredToken(")"),
                                                                    shape95),
                                                                Custom(
                                                                    If(Not(Token("datetime_pattern")), rules.NameDeclarationOrStringLiteral),
                                                                    shape46)),
                                                            Optional(
                                                                Custom(
                                                                    rules.StringLiteral,
                                                                    shape0)),
                                                            shape11),
                                                        missingElement: null,
                                                        oneOrMore: true,
                                                        producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray()))),
                                                () => (SyntaxElement)new CustomNode(shape96, rules.MissingStringLiteral(), new SyntaxList<SyntaxElement>((SyntaxElement)new CustomNode(shape11, (SyntaxElement)new CustomNode(shape95, CreateMissingToken("datetime_pattern"), CreateMissingToken("("), rules.MissingStringLiteral(), CreateMissingToken(","), rules.MissingNameDeclaration(), CreateMissingToken(")")), rules.MissingStringLiteral())))),
                                            RequiredToken(")"),
                                            shape97)),
                                    RequiredToken("dataformat"),
                                    shape98)),
                            () => CreateMissingToken("dataformat")),
                        RequiredToken("="),
                        RequiredToken("apacheavro", "avro", "csv", "json", "multijson", "orc", "parquet", "psv", "raw", "scsv", "sohsv", "sstream", "tsve", "tsv", "txt", "w3clogfile"),
                        RequiredToken("("),
                        Required(
                            OList(
                                primaryElementParser: Custom(
                                    rules.StringLiteral,
                                    shape0),
                                separatorParser: Token(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingStringLiteral,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                        RequiredToken(")"),
                        Optional(
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape86),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                shape87))}
                    ,
                    shape99));

            var CreateOrAlterFunction = Command("CreateOrAlterFunction", 
                Custom(
                    Token("create-or-alter", CompletionKind.CommandPrefix),
                    Token("function"),
                    Required(
                        First(
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape86),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                shape177),
                            Custom(
                                If(Not(Token("with")), rules.NameDeclarationOrStringLiteral),
                                shape46)),
                        () => (SyntaxElement)new CustomNode(shape177, CreateMissingToken("with"), CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()))), CreateMissingToken(")"), rules.MissingNameDeclaration())),
                    Required(rules.FunctionDeclaration, rules.MissingFunctionDeclaration)));

            var CreateOrAlterMaterializedView = Command("CreateOrAlterMaterializedView", 
                Custom(
                    Token("create-or-alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Required(
                        First(
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            First(
                                                Token("autoUpdateSchema"),
                                                Token("backfill"),
                                                Token("dimensionTables"),
                                                Token("docString"),
                                                Token("effectiveDateTime"),
                                                Token("folder"),
                                                Token("lookback"),
                                                Token("updateExtentsCreationTime"),
                                                If(Not(And(Token("autoUpdateSchema", "backfill", "dimensionTables", "docString", "effectiveDateTime", "folder", "lookback", "updateExtentsCreationTime"))), rules.NameDeclarationOrStringLiteral)),
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape48),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingToken("autoUpdateSchema"), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingToken("autoUpdateSchema"), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                                shape129),
                            Custom(
                                If(Not(Token("with")), rules.MaterializedViewNameReference),
                                shape17)),
                        () => (SyntaxElement)new CustomNode(shape129, CreateMissingToken("with"), CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingToken("autoUpdateSchema"), CreateMissingToken("="), rules.MissingValue()))), CreateMissingToken(")"), rules.MissingNameReference())),
                    RequiredToken("on"),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Required(rules.FunctionBody, rules.MissingFunctionBody),
                    shape130));

            var CreateOrAleterWorkloadGroup = Command("CreateOrAleterWorkloadGroup", 
                Custom(
                    Token("create-or-alter", CompletionKind.CommandPrefix),
                    RequiredToken("workload_group"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape53));

            var CreateBasicAuthUser = Command("CreateBasicAuthUser", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("basicauth"),
                    RequiredToken("user"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        Custom(
                            Token("password"),
                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                            shape178)),
                    shape179));

            var CreateDatabasePersist = Command("CreateDatabasePersist", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.NameDeclarationOrStringLiteral,
                    Token("persist"),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                shape0),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredToken(")"),
                    Optional(Token("ifnotexists")),
                    shape180));

            var CreateDatabaseVolatile = Command("CreateDatabaseVolatile", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.NameDeclarationOrStringLiteral,
                    Token("volatile"),
                    Optional(Token("ifnotexists")),
                    shape181));

            var CreateDatabaseIngestionMapping = Command("CreateDatabaseIngestionMapping", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    RequiredToken("ingestion"),
                    RequiredToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape182));

            var CreateSqlExternalTable = Command("CreateSqlExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("create", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.NameDeclarationOrStringLiteral,
                        Token("("),
                        OList(
                            primaryElementParser: Custom(
                                rules.NameDeclarationOrStringLiteral,
                                RequiredToken(":"),
                                Required(rules.Type, rules.MissingType),
                                shape47),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        Token(")"),
                        Token("kind"),
                        RequiredToken("="),
                        RequiredToken("sql"),
                        RequiredToken("table"),
                        RequiredToken("="),
                        Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                        RequiredToken("("),
                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                        RequiredToken(")"),
                        Optional(
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape86),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                shape87))}
                    ,
                    shape88));

            var CreateStorageExternalTable = Command("CreateStorageExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("create", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.NameDeclarationOrStringLiteral,
                        RequiredToken("("),
                        Required(
                            OList(
                                primaryElementParser: Custom(
                                    rules.NameDeclarationOrStringLiteral,
                                    RequiredToken(":"),
                                    Required(rules.Type, rules.MissingType),
                                    shape47),
                                separatorParser: Token(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType()),
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType())))),
                        RequiredToken(")"),
                        RequiredToken("kind"),
                        RequiredToken("="),
                        Required(
                            First(
                                Token("adl").Hide(),
                                Token("blob").Hide(),
                                Token("storage")),
                            () => CreateMissingToken("adl")),
                        Required(
                            First(
                                Token("dataformat"),
                                Custom(
                                    Token("partition"),
                                    RequiredToken("by"),
                                    RequiredToken("("),
                                    Required(
                                        OList(
                                            primaryElementParser: Custom(
                                                rules.NameDeclarationOrStringLiteral,
                                                RequiredToken(":"),
                                                Required(
                                                    First(
                                                        Custom(
                                                            Token("datetime"),
                                                            Optional(
                                                                Custom(
                                                                    Token("="),
                                                                    Required(
                                                                        First(
                                                                            Custom(
                                                                                Token("bin"),
                                                                                RequiredToken("("),
                                                                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                                RequiredToken(","),
                                                                                Required(rules.Value, rules.MissingValue),
                                                                                RequiredToken(")"),
                                                                                shape89),
                                                                            Custom(
                                                                                Token("startofday", "startofmonth", "startofweek", "startofyear"),
                                                                                RequiredToken("("),
                                                                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                                RequiredToken(")"),
                                                                                shape90)),
                                                                        () => (SyntaxElement)new CustomNode(shape89, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(), CreateMissingToken(","), rules.MissingValue(), CreateMissingToken(")"))))),
                                                            shape91),
                                                        Custom(
                                                            Token("long"),
                                                            RequiredToken("="),
                                                            RequiredToken("hash"),
                                                            RequiredToken("("),
                                                            Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                            RequiredToken(","),
                                                            Required(rules.Value, rules.MissingValue),
                                                            RequiredToken(")"),
                                                            shape92),
                                                        Custom(
                                                            Token("string"),
                                                            Optional(
                                                                Custom(
                                                                    Token("="),
                                                                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                    shape93)),
                                                            shape91)),
                                                    () => (SyntaxElement)new CustomNode(shape91, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape89, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(), CreateMissingToken(","), rules.MissingValue(), CreateMissingToken(")"))))),
                                                shape94),
                                            separatorParser: Token(","),
                                            secondaryElementParser: null,
                                            missingPrimaryElement: null,
                                            missingSeparator: null,
                                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape94, rules.MissingNameDeclaration(), CreateMissingToken(":"), (SyntaxElement)new CustomNode(shape91, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape89, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(), CreateMissingToken(","), rules.MissingValue(), CreateMissingToken(")"))))),
                                            endOfList: null,
                                            oneOrMore: true,
                                            allowTrailingSeparator: false,
                                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape94, rules.MissingNameDeclaration(), CreateMissingToken(":"), (SyntaxElement)new CustomNode(shape91, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape89, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(), CreateMissingToken(","), rules.MissingValue(), CreateMissingToken(")")))))))),
                                    RequiredToken(")"),
                                    Optional(
                                        Custom(
                                            Token("pathformat"),
                                            RequiredToken("="),
                                            RequiredToken("("),
                                            Required(
                                                First(
                                                    Custom(
                                                        rules.StringLiteral,
                                                        Required(
                                                            List(
                                                                Custom(
                                                                    First(
                                                                        Custom(
                                                                            Token("datetime_pattern"),
                                                                            RequiredToken("("),
                                                                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                                                                            RequiredToken(","),
                                                                            Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                            RequiredToken(")"),
                                                                            shape95),
                                                                        Custom(
                                                                            If(Not(Token("datetime_pattern")), rules.NameDeclarationOrStringLiteral),
                                                                            shape46)),
                                                                    Optional(
                                                                        Custom(
                                                                            rules.StringLiteral,
                                                                            shape0)),
                                                                    shape11),
                                                                missingElement: null,
                                                                oneOrMore: true,
                                                                producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray())),
                                                            () => new SyntaxList<SyntaxElement>((SyntaxElement)new CustomNode(shape11, (SyntaxElement)new CustomNode(shape95, CreateMissingToken("datetime_pattern"), CreateMissingToken("("), rules.MissingStringLiteral(), CreateMissingToken(","), rules.MissingNameDeclaration(), CreateMissingToken(")")), rules.MissingStringLiteral()))),
                                                        shape96),
                                                    List(
                                                        Custom(
                                                            First(
                                                                Custom(
                                                                    Token("datetime_pattern"),
                                                                    RequiredToken("("),
                                                                    rules.StringLiteral,
                                                                    Token(","),
                                                                    rules.NameDeclarationOrStringLiteral,
                                                                    Token(")"),
                                                                    shape95),
                                                                Custom(
                                                                    Token("datetime_pattern"),
                                                                    RequiredToken("("),
                                                                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                                                                    RequiredToken(","),
                                                                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                    RequiredToken(")"),
                                                                    shape95),
                                                                Custom(
                                                                    If(Not(Token("datetime_pattern")), rules.NameDeclarationOrStringLiteral),
                                                                    shape46)),
                                                            Optional(
                                                                Custom(
                                                                    rules.StringLiteral,
                                                                    shape0)),
                                                            shape11),
                                                        missingElement: null,
                                                        oneOrMore: true,
                                                        producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray()))),
                                                () => (SyntaxElement)new CustomNode(shape96, rules.MissingStringLiteral(), new SyntaxList<SyntaxElement>((SyntaxElement)new CustomNode(shape11, (SyntaxElement)new CustomNode(shape95, CreateMissingToken("datetime_pattern"), CreateMissingToken("("), rules.MissingStringLiteral(), CreateMissingToken(","), rules.MissingNameDeclaration(), CreateMissingToken(")")), rules.MissingStringLiteral())))),
                                            RequiredToken(")"),
                                            shape97)),
                                    RequiredToken("dataformat"),
                                    shape98)),
                            () => CreateMissingToken("dataformat")),
                        RequiredToken("="),
                        RequiredToken("apacheavro", "avro", "csv", "json", "multijson", "orc", "parquet", "psv", "raw", "scsv", "sohsv", "sstream", "tsve", "tsv", "txt", "w3clogfile"),
                        RequiredToken("("),
                        Required(
                            OList(
                                primaryElementParser: Custom(
                                    rules.StringLiteral,
                                    shape0),
                                separatorParser: Token(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingStringLiteral,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                        RequiredToken(")"),
                        Optional(
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape86),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                shape87))}
                    ,
                    shape99));

            var CreateExternalTableMapping = Command("CreateExternalTableMapping", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("external"),
                    RequiredToken("table"),
                    Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                    RequiredToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape102));

            var CreateFunction = Command("CreateFunction", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("function"),
                    Optional(
                        First(
                            Custom(
                                Token("ifnotexists"),
                                Optional(
                                    Custom(
                                        Token("with"),
                                        RequiredToken("("),
                                        CommaList(
                                            Custom(
                                                If(Not(Token(")")), rules.NameDeclarationOrStringLiteral),
                                                RequiredToken("="),
                                                Required(rules.Value, rules.MissingValue),
                                                shape86)),
                                        RequiredToken(")"),
                                        shape87)),
                                shape167),
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                CommaList(
                                    Custom(
                                        If(Not(Token(")")), rules.NameDeclarationOrStringLiteral),
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape86)),
                                RequiredToken(")"),
                                shape87))),
                    Required(If(Not(And(Token("ifnotexists", "with"))), rules.NameDeclarationOrStringLiteral), rules.MissingNameDeclaration),
                    Required(rules.FunctionDeclaration, rules.MissingFunctionDeclaration),
                    shape183));

            var CreateRequestSupport = Command("CreateRequestSupport", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("request_support"),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape184));

            var CreateRowStore = Command("CreateRowStore", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("rowstore"),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape184));

            var CreateTables = Command("CreateTables", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.NameDeclarationOrStringLiteral,
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredToken(":"),
                                            Required(rules.Type, rules.MissingType),
                                            shape47),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType())))),
                                RequiredToken(")"),
                                shape173),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape173, rules.MissingNameDeclaration(), CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType()))), CreateMissingToken(")")),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape173, rules.MissingNameDeclaration(), CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType()))), CreateMissingToken(")"))))),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape174));

            var CreateTable = Command("CreateTable", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.NameDeclarationOrStringLiteral,
                    Token("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.NameDeclarationOrStringLiteral,
                                RequiredToken(":"),
                                Required(rules.Type, rules.MissingType),
                                shape47),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType())))),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        First(
                                            Token("docstring"),
                                            Token("folder"),
                                            If(Not(And(Token("docstring", "folder"))), rules.NameDeclarationOrStringLiteral)),
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape48),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingToken("docstring"), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingToken("docstring"), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"))),
                    shape185));

            var CreateTableBasedOnAnother = Command("CreateTableBasedOnAnother", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.NameDeclarationOrStringLiteral,
                    Token("based-on"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        First(
                                            Token("docstring"),
                                            Token("folder"),
                                            If(Not(And(Token("docstring", "folder"))), rules.NameDeclarationOrStringLiteral)),
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape48),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingToken("docstring"), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingToken("docstring"), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"))),
                    shape186));

            var CreateTableIngestionMapping = Command("CreateTableIngestionMapping", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("table"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    RequiredToken("ingestion"),
                    RequiredToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape187));

            var CreateTempStorage = Command("CreateTempStorage", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("tempstorage")));

            var CreateMaterializedView = Command("CreateMaterializedView", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Optional(
                        First(
                            Custom(
                                Token("async"),
                                Optional(Token("ifnotexists")),
                                shape167),
                            Token("ifnotexists"))),
                    RequiredToken("materialized-view"),
                    Required(
                        First(
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            First(
                                                Token("autoUpdateSchema"),
                                                Token("backfill"),
                                                Token("dimensionTables"),
                                                Token("docString"),
                                                Token("effectiveDateTime"),
                                                Token("folder"),
                                                Token("lookback"),
                                                Token("updateExtentsCreationTime"),
                                                If(Not(And(Token("autoUpdateSchema", "backfill", "dimensionTables", "docString", "effectiveDateTime", "folder", "lookback", "updateExtentsCreationTime"))), rules.NameDeclarationOrStringLiteral)),
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape48),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingToken("autoUpdateSchema"), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingToken("autoUpdateSchema"), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                shape188),
                            Custom(
                                If(Not(Token("with")), rules.NameDeclarationOrStringLiteral),
                                shape46)),
                        () => (SyntaxElement)new CustomNode(shape188, CreateMissingToken("with"), CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingToken("autoUpdateSchema"), CreateMissingToken("="), rules.MissingValue()))), CreateMissingToken(")"), rules.MissingNameDeclaration())),
                    RequiredToken("on"),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Required(rules.FunctionBody, rules.MissingFunctionBody),
                    shape189));

            var DefineTables = Command("DefineTables", 
                Custom(
                    Token("define", CompletionKind.CommandPrefix),
                    RequiredToken("tables"),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.NameDeclarationOrStringLiteral,
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredToken(":"),
                                            Required(rules.Type, rules.MissingType),
                                            shape47),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType())))),
                                RequiredToken(")"),
                                shape173),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape173, rules.MissingNameDeclaration(), CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType()))), CreateMissingToken(")")),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape173, rules.MissingNameDeclaration(), CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingToken(":"), rules.MissingType()))), CreateMissingToken(")"))))),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape174));

            var DeleteClusterPolicyCaching = Command("DeleteClusterPolicyCaching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("caching")));

            var DeleteClusterPolicyCallout = Command("DeleteClusterPolicyCallout", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("callout")));

            var DeleteClusterPolicyManagedIdentity = Command("DeleteClusterPolicyManagedIdentity", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("managed_identity")));

            var DeleteClusterPolicyRequestClassification = Command("DeleteClusterPolicyRequestClassification", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("request_classification")));

            var DeleteClusterPolicyRowStore = Command("DeleteClusterPolicyRowStore", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("rowstore")));

            var DeleteClusterPolicySandbox = Command("DeleteClusterPolicySandbox", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("sandbox")));

            var DeleteClusterPolicySharding = Command("DeleteClusterPolicySharding", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("sharding")));

            var DeleteClusterPolicyStreamingIngestion = Command("DeleteClusterPolicyStreamingIngestion", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("policy"),
                    RequiredToken("streamingingestion")));

            var DeleteColumnPolicyCaching = Command("DeleteColumnPolicyCaching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("column"),
                    rules.DatabaseTableColumnNameReference,
                    RequiredToken("policy"),
                    RequiredToken("caching"),
                    shape190));

            var DeleteColumnPolicyEncoding = Command("DeleteColumnPolicyEncoding", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("column"),
                    Required(rules.TableColumnNameReference, rules.MissingNameReference),
                    RequiredToken("policy"),
                    RequiredToken("encoding"),
                    shape190));

            var DeleteDatabasePolicyCaching = Command("DeleteDatabasePolicyCaching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("caching"),
                    shape191));

            var DeleteDatabasePolicyDiagnostics = Command("DeleteDatabasePolicyDiagnostics", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("diagnostics"),
                    shape191));

            var DeleteDatabasePolicyEncoding = Command("DeleteDatabasePolicyEncoding", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("encoding"),
                    shape191));

            var DeleteDatabasePolicyExtentTagsRetention = Command("DeleteDatabasePolicyExtentTagsRetention", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("extent_tags_retention"),
                    shape191));

            var DeleteDatabasePolicyIngestionBatching = Command("DeleteDatabasePolicyIngestionBatching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("ingestionbatching"),
                    shape191));

            var DeleteDatabasePolicyManagedIdentity = Command("DeleteDatabasePolicyManagedIdentity", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("managed_identity"),
                    shape191));

            var DeleteDatabasePolicyMerge = Command("DeleteDatabasePolicyMerge", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("merge"),
                    shape191));

            var DeleteDatabasePolicyRetention = Command("DeleteDatabasePolicyRetention", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("retention"),
                    shape191));

            var DeleteDatabasePolicySharding = Command("DeleteDatabasePolicySharding", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("sharding"),
                    shape191));

            var DeleteDatabasePolicyShardsGrouping = Command("DeleteDatabasePolicyShardsGrouping", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("shards_grouping").Hide(),
                    shape191));

            var DeleteDatabasePolicyStreamingIngestion = Command("DeleteDatabasePolicyStreamingIngestion", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("policy"),
                    RequiredToken("streamingingestion"),
                    shape191));

            var DropFollowerDatabasePolicyCaching = Command("DropFollowerDatabasePolicyCaching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("follower"),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    RequiredToken("caching"),
                    shape192));

            var DropFollowerTablesPolicyCaching = Command("DropFollowerTablesPolicyCaching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("follower"),
                    RequiredToken("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    Required(
                        First(
                            Custom(
                                Token("materialized-views"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            shape46),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingNameDeclaration,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration()))),
                                RequiredToken(")"),
                                shape87),
                            Custom(
                                Token("materialized-view"),
                                Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                                shape119),
                            Custom(
                                Token("tables"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            shape46),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingNameDeclaration,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration()))),
                                RequiredToken(")"),
                                shape87),
                            Custom(
                                Token("table"),
                                Required(rules.TableNameReference, rules.MissingNameReference),
                                shape120)),
                        () => (SyntaxElement)new CustomNode(shape87, CreateMissingToken("materialized-views"), CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration())), CreateMissingToken(")"))),
                    RequiredToken("policy"),
                    RequiredToken("caching"),
                    shape193));

            var DeleteMaterializedViewPolicyCaching = Command("DeleteMaterializedViewPolicyCaching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("policy"),
                    Token("caching"),
                    shape125));

            var DeleteMaterializedViewPolicyPartitioning = Command("DeleteMaterializedViewPolicyPartitioning", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("policy"),
                    Token("partitioning"),
                    shape125));

            var DeleteMaterializedViewPolicyRowLevelSecurity = Command("DeleteMaterializedViewPolicyRowLevelSecurity", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    RequiredToken("policy"),
                    RequiredToken("row_level_security"),
                    shape125));

            var DeletePoliciesOfRetention = Command("DeletePoliciesOfRetention", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("policies"),
                    RequiredToken("of"),
                    RequiredToken("retention"),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                shape0),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredToken(")"),
                    shape194));

            var DeleteTablePolicyAutoDelete = Command("DeleteTablePolicyAutoDelete", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("auto_delete"),
                    shape195));

            var DeleteTablePolicyCaching = Command("DeleteTablePolicyCaching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("caching"),
                    shape195));

            var DeleteTablePolicyEncoding = Command("DeleteTablePolicyEncoding", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("encoding"),
                    shape195));

            var DeleteTablePolicyExtentTagsRetention = Command("DeleteTablePolicyExtentTagsRetention", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("extent_tags_retention"),
                    shape195));

            var DeleteTablePolicyIngestionBatching = Command("DeleteTablePolicyIngestionBatching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("ingestionbatching"),
                    shape195));

            var DeleteTablePolicyMerge = Command("DeleteTablePolicyMerge", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("merge"),
                    shape195));

            var DeleteTablePolicyRestrictedViewAccess = Command("DeleteTablePolicyRestrictedViewAccess", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("restricted_view_access"),
                    shape195));

            var DeleteTablePolicyRetention = Command("DeleteTablePolicyRetention", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("retention"),
                    shape195));

            var DeleteTablePolicyRowOrder = Command("DeleteTablePolicyRowOrder", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("roworder"),
                    shape195));

            var DeleteTablePolicySharding = Command("DeleteTablePolicySharding", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("sharding"),
                    shape195));

            var DeleteTablePolicyStreamingIngestion = Command("DeleteTablePolicyStreamingIngestion", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("streamingingestion"),
                    shape195));

            var DeleteTablePolicyUpdate = Command("DeleteTablePolicyUpdate", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    RequiredToken("policy"),
                    RequiredToken("update"),
                    shape195));

            var DeleteTablePolicyIngestionTime = Command("DeleteTablePolicyIngestionTime", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("policy"),
                    Token("ingestiontime"),
                    shape195));

            var DeleteTablePolicyPartitioning = Command("DeleteTablePolicyPartitioning", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("policy"),
                    Token("partitioning"),
                    shape195));

            var DeleteTablePolicyRowLevelSecurity = Command("DeleteTablePolicyRowLevelSecurity", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("policy"),
                    RequiredToken("row_level_security"),
                    shape195));

            var DeleteTableRecords = Command("DeleteTableRecords", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Required(
                        First(
                            Custom(
                                Token("async"),
                                RequiredToken("table")),
                            Token("table")),
                        () => (SyntaxElement)new CustomNode(CreateMissingToken("async"), CreateMissingToken("table"))),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredToken("records"),
                    Required(
                        First(
                            Custom(
                                Custom(
                                    Token("<|"),
                                    Required(rules.CommandInput, rules.MissingExpression),
                                    shape84)),
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape63),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                Required(
                                    Custom(
                                        Token("<|"),
                                        Required(rules.CommandInput, rules.MissingExpression),
                                        shape84),
                                    () => (SyntaxElement)new CustomNode(shape84, CreateMissingToken("<|"), rules.MissingExpression())),
                                shape196)),
                        () => (SyntaxElement)new CustomNode(shape84, CreateMissingToken("<|"), rules.MissingExpression())),
                    shape195));

            var DetachDatabase = Command("DetachDatabase", 
                Custom(
                    Token("detach", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    shape197));

            var DisableContinuousExport = Command("DisableContinuousExport", 
                Custom(
                    Token("disable", CompletionKind.CommandPrefix),
                    Token("continuous-export"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    shape198));

            var DisableDatabaseMaintenanceMode = Command("DisableDatabaseMaintenanceMode", 
                Custom(
                    Token("disable", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("maintenance_mode"),
                    shape199));

            var DisablePlugin = Command("DisablePlugin", 
                Custom(
                    Token("disable", CompletionKind.CommandPrefix),
                    RequiredToken("plugin"),
                    Required(
                        First(
                            rules.StringLiteral,
                            rules.NameDeclarationOrStringLiteral),
                        rules.MissingStringLiteral),
                    shape200));

            var DropPretendExtentsByProperties = Command("DropPretendExtentsByProperties", 
                Custom(
                    Token("drop-pretend", CompletionKind.CommandPrefix),
                    RequiredToken("extents"),
                    Required(
                        First(
                            Token("from"),
                            Custom(
                                Token("older"),
                                Required(rules.Value, rules.MissingValue),
                                RequiredToken("days", "hours"),
                                RequiredToken("from"),
                                shape201)),
                        () => CreateMissingToken("from")),
                    Required(
                        First(
                            Custom(
                                Token("all"),
                                RequiredToken("tables")),
                            Custom(
                                If(Not(Token("all")), rules.TableNameReference),
                                shape19)),
                        () => (SyntaxElement)new CustomNode(CreateMissingToken("all"), CreateMissingToken("tables"))),
                    Optional(
                        First(
                            Custom(
                                Token("limit"),
                                Required(rules.Value, rules.MissingValue),
                                shape202),
                            Custom(
                                Token("trim"),
                                RequiredToken("by"),
                                RequiredToken("datasize", "extentsize"),
                                Required(rules.Value, rules.MissingValue),
                                RequiredToken("bytes", "GB", "MB"),
                                Optional(
                                    Custom(
                                        Token("limit"),
                                        Required(rules.Value, rules.MissingValue),
                                        shape202)),
                                shape203))),
                    shape204));

            var DropBasicAuthUser = Command("DropBasicAuthUser", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("basicauth"),
                    RequiredToken("user"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape205));

            var DropClusterBlockedPrincipals = Command("DropClusterBlockedPrincipals", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("blockedprincipals"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        First(
                            Custom(
                                Token("application"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                Optional(
                                    Custom(
                                        Token("user"),
                                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                                        shape1)),
                                shape2),
                            Custom(
                                Token("user"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                shape1))),
                    shape206));

            var DropClusterRole = Command("DropClusterRole", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("admins", "databasecreators", "users", "viewers"),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                shape0),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredToken(")"),
                    Optional(
                        First(
                            Custom(
                                Token("skip-results"),
                                Optional(
                                    Custom(
                                        rules.StringLiteral,
                                        shape0)),
                                shape6),
                            Custom(
                                rules.StringLiteral,
                                shape0))),
                    shape7));

            var DropColumn = Command("DropColumn", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("column"),
                    Required(rules.TableColumnNameReference, rules.MissingNameReference),
                    shape207));

            var DropContinuousExport = Command("DropContinuousExport", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("continuous-export"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    shape208));

            var DropDatabaseIngestionMapping = Command("DropDatabaseIngestionMapping", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("ingestion"),
                    RequiredToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape209));

            var DropDatabasePrettyName = Command("DropDatabasePrettyName", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("prettyname"),
                    shape199));

            var DropDatabaseRole = Command("DropDatabaseRole", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("admins", "ingestors", "monitors", "unrestrictedviewers", "users", "viewers"),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                shape0),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredToken(")"),
                    Optional(
                        First(
                            Custom(
                                Token("skip-results"),
                                Optional(
                                    Custom(
                                        rules.StringLiteral,
                                        shape0)),
                                shape6),
                            Custom(
                                rules.StringLiteral,
                                shape0))),
                    shape9));

            var DropEmptyExtentContainers = Command("DropEmptyExtentContainers", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            Token("async"),
                            Token("empty")),
                        Custom(
                            Token("async"),
                            RequiredToken("empty")),
                        Token("empty")),
                    RequiredToken("extentcontainers"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("until"),
                    RequiredToken("="),
                    Required(rules.Value, rules.MissingValue),
                    Optional(
                        First(
                            Custom(
                                Token("whatif"),
                                Optional(
                                    Custom(
                                        Token("with"),
                                        RequiredToken("("),
                                        Required(
                                            OList(
                                                primaryElementParser: Custom(
                                                    rules.NameDeclarationOrStringLiteral,
                                                    RequiredToken("="),
                                                    Required(rules.Value, rules.MissingValue),
                                                    shape63),
                                                separatorParser: Token(","),
                                                secondaryElementParser: null,
                                                missingPrimaryElement: null,
                                                missingSeparator: null,
                                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                                endOfList: null,
                                                oneOrMore: true,
                                                allowTrailingSeparator: false,
                                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                                        RequiredToken(")"),
                                        shape87)),
                                shape167),
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape63),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                shape87))),
                    shape210));

            var DropExtentsPartitionMetadata = Command("DropExtentsPartitionMetadata", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("extents"),
                    Token("partition"),
                    RequiredToken("metadata"),
                    RequiredToken("from"),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Required(
                        First(
                            Custom(
                                Custom(
                                    Token("<|"),
                                    Required(rules.CommandInput, rules.MissingExpression),
                                    shape84)),
                            Custom(
                                Token("between"),
                                RequiredToken("("),
                                Required(rules.Value, rules.MissingValue),
                                RequiredToken(".."),
                                Required(rules.Value, rules.MissingValue),
                                RequiredToken(")"),
                                Required(
                                    Custom(
                                        Token("<|"),
                                        Required(rules.CommandInput, rules.MissingExpression),
                                        shape84),
                                    () => (SyntaxElement)new CustomNode(shape84, CreateMissingToken("<|"), rules.MissingExpression())),
                                shape211)),
                        () => (SyntaxElement)new CustomNode(shape84, CreateMissingToken("<|"), rules.MissingExpression())),
                    shape212));

            var DropExtents = Command("DropExtents", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("extents"),
                    Required(
                        First(
                            Custom(
                                Token("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.AnyGuidLiteralOrString,
                                            shape0),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingValue,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingValue()))),
                                RequiredToken(")"),
                                Optional(
                                    Custom(
                                        Token("from"),
                                        Required(rules.TableNameReference, rules.MissingNameReference),
                                        shape157)),
                                shape213),
                            Custom(
                                Token("<|"),
                                Required(rules.CommandInput, rules.MissingExpression),
                                shape214),
                            Custom(
                                Token("from"),
                                Required(
                                    First(
                                        Custom(
                                            Token("all"),
                                            RequiredToken("tables")),
                                        Custom(
                                            If(Not(Token("all")), rules.TableNameReference),
                                            shape19)),
                                    () => (SyntaxElement)new CustomNode(CreateMissingToken("all"), CreateMissingToken("tables"))),
                                Optional(
                                    First(
                                        Custom(
                                            Token("limit"),
                                            Required(rules.Value, rules.MissingValue),
                                            shape202),
                                        Custom(
                                            Token("trim"),
                                            RequiredToken("by"),
                                            RequiredToken("datasize", "extentsize"),
                                            Required(rules.Value, rules.MissingValue),
                                            RequiredToken("bytes", "GB", "MB"),
                                            Optional(
                                                Custom(
                                                    Token("limit"),
                                                    Required(rules.Value, rules.MissingValue),
                                                    shape202)),
                                            shape203))),
                                shape184),
                            Custom(
                                Token("older"),
                                Required(rules.Value, rules.MissingValue),
                                RequiredToken("days", "hours"),
                                RequiredToken("from"),
                                Required(
                                    First(
                                        Custom(
                                            Token("all"),
                                            RequiredToken("tables")),
                                        Custom(
                                            If(Not(Token("all")), rules.TableNameReference),
                                            shape19)),
                                    () => (SyntaxElement)new CustomNode(CreateMissingToken("all"), CreateMissingToken("tables"))),
                                Optional(
                                    First(
                                        Custom(
                                            Token("limit"),
                                            Required(rules.Value, rules.MissingValue),
                                            shape202),
                                        Custom(
                                            Token("trim"),
                                            RequiredToken("by"),
                                            RequiredToken("datasize", "extentsize"),
                                            Required(rules.Value, rules.MissingValue),
                                            RequiredToken("bytes", "GB", "MB"),
                                            Optional(
                                                Custom(
                                                    Token("limit"),
                                                    Required(rules.Value, rules.MissingValue),
                                                    shape202)),
                                            shape203))),
                                shape215),
                            Custom(
                                Token("whatif"),
                                RequiredToken("<|"),
                                Required(rules.CommandInput, rules.MissingExpression),
                                shape216)),
                        () => (SyntaxElement)new CustomNode(shape213, CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingValue())), CreateMissingToken(")"), (SyntaxElement)new CustomNode(shape157, CreateMissingToken("from"), rules.MissingNameReference())))));

            var DropExtentTagsRetention = Command("DropExtentTagsRetention", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("extent"),
                    Token("tags"),
                    RequiredToken("retention")));

            var DropExtent = Command("DropExtent", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("extent"),
                    Required(rules.AnyGuidLiteralOrString, rules.MissingValue),
                    Optional(
                        Custom(
                            Token("from"),
                            Required(rules.TableNameReference, rules.MissingNameReference),
                            shape157)),
                    shape217));

            var DropExternalTableAdmins = Command("DropExternalTableAdmins", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    rules.ExternalTableNameReference,
                    Token("admins"),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                shape0),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredToken(")"),
                    Optional(
                        First(
                            Custom(
                                Token("skip-results"),
                                Optional(
                                    Custom(
                                        rules.StringLiteral,
                                        shape0)),
                                shape11),
                            Custom(
                                rules.StringLiteral,
                                shape0))),
                    shape12));

            var DropExternalTableMapping = Command("DropExternalTableMapping", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    rules.ExternalTableNameReference,
                    Token("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape218));

            var DropExternalTable = Command("DropExternalTable", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("external"),
                    RequiredToken("table"),
                    Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                    shape219));

            var DropFollowerDatabases = Command("DropFollowerDatabases", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("follower"),
                    First(
                        Custom(
                            Token("databases"),
                            Token("("),
                            OList(
                                primaryElementParser: Custom(
                                    rules.DatabaseNameReference,
                                    shape8),
                                separatorParser: Token(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingNameReference,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            Token(")"),
                            shape220),
                        Custom(
                            Token("databases"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.DatabaseNameReference,
                                        shape8),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: rules.MissingNameReference,
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameReference()))),
                            RequiredToken(")"),
                            shape220),
                        Custom(
                            Token("database"),
                            rules.DatabaseNameReference,
                            shape221)),
                    RequiredToken("from"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape222));

            var DropFollowerDatabaseAuthorizedPrincipals = Command("DropFollowerDatabaseAuthorizedPrincipals", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("follower"),
                    RequiredToken("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("admins", "monitors", "unrestrictedviewers", "users", "viewers"),
                    Required(
                        First(
                            Token("("),
                            Custom(
                                Token("from"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                RequiredToken("("),
                                shape107)),
                        () => CreateMissingToken("(")),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                shape0),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredToken(")"),
                    shape223));

            var DropFunctions = Command("DropFunctions", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("functions"),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.DatabaseFunctionNameReference,
                                shape15),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingNameReference,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameReference()))),
                    RequiredToken(")"),
                    Optional(Token("ifexists")),
                    shape224));

            var DropFunctionRole = Command("DropFunctionRole", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("function"),
                    rules.DatabaseFunctionNameReference,
                    Token("admins"),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                shape0),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredToken(")"),
                    Optional(
                        First(
                            Custom(
                                Token("skip-results"),
                                Optional(
                                    Custom(
                                        rules.StringLiteral,
                                        shape0)),
                                shape6),
                            Custom(
                                rules.StringLiteral,
                                shape0))),
                    shape16));

            var DropFunction = Command("DropFunction", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("function"),
                    Required(rules.DatabaseFunctionNameReference, rules.MissingNameReference),
                    Optional(Token("ifexists")),
                    shape225));

            var DropMaterializedViewAdmins = Command("DropMaterializedViewAdmins", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("admins"),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                shape0),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape0)),
                    shape18));

            var DropMaterializedView = Command("DropMaterializedView", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    shape226));

            var DropRowStore = Command("DropRowStore", 
                Custom(
                    Token("detach", "drop"),
                    Token("rowstore"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Optional(Token("ifexists")),
                    shape227));

            var StoredQueryResultsDrop = Command("StoredQueryResultsDrop", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("stored_query_results"),
                    RequiredToken("by"),
                    RequiredToken("user"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape228));

            var StoredQueryResultDrop = Command("StoredQueryResultDrop", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("stored_query_result"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    shape229));

            var DropStoredQueryResultContainers = Command("DropStoredQueryResultContainers", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("storedqueryresultcontainers"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    Required(
                        List(
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                shape0),
                            missingElement: null,
                            oneOrMore: true,
                            producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray())),
                        () => new SyntaxList<SyntaxElement>(rules.MissingValue())),
                    shape230));

            var DropTables = Command("DropTables", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("tables"),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.TableNameReference,
                                shape19),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingNameReference,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameReference()))),
                    RequiredToken(")"),
                    Optional(Token("ifexists")),
                    shape231));

            var DropTableColumns = Command("DropTableColumns", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("columns"),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.ColumnNameReference,
                                shape25),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingNameReference,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameReference()))),
                    RequiredToken(")"),
                    shape51));

            var DropTableIngestionMapping = Command("DropTableIngestionMapping", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("ingestion"),
                    RequiredToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape232));

            var DropTableRole = Command("DropTableRole", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("admins", "ingestors"),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                shape0),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredToken(")"),
                    Optional(
                        First(
                            Custom(
                                Token("skip-results"),
                                Optional(
                                    Custom(
                                        rules.StringLiteral,
                                        shape0)),
                                shape6),
                            Custom(
                                rules.StringLiteral,
                                shape0))),
                    shape20));

            var DropTable = Command("DropTable", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Optional(Token("ifexists")),
                    shape233));

            var DropTempStorage = Command("DropTempStorage", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("tempstorage"),
                    RequiredToken("older"),
                    Required(rules.Value, rules.MissingValue),
                    shape234));

            var DropUnusedStoredQueryResultContainers = Command("DropUnusedStoredQueryResultContainers", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("unused"),
                    RequiredToken("storedqueryresultcontainers"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    shape235));

            var DropWorkloadGroup = Command("DropWorkloadGroup", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    RequiredToken("workload_group"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    shape236));

            var EnableContinuousExport = Command("EnableContinuousExport", 
                Custom(
                    Token("enable", CompletionKind.CommandPrefix),
                    Token("continuous-export"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    shape208));

            var EnableDatabaseMaintenanceMode = Command("EnableDatabaseMaintenanceMode", 
                Custom(
                    Token("enable", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("maintenance_mode"),
                    shape199));

            var EnableDisableMaterializedView = Command("EnableDisableMaterializedView", 
                Custom(
                    Token("disable", "enable"),
                    Token("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    shape226));

            var EnablePlugin = Command("EnablePlugin", 
                Custom(
                    Token("enable", CompletionKind.CommandPrefix),
                    RequiredToken("plugin"),
                    Required(
                        First(
                            rules.StringLiteral,
                            rules.NameDeclarationOrStringLiteral),
                        rules.MissingStringLiteral),
                    shape237));

            var ExportToSqlTable = Command("ExportToSqlTable", 
                Custom(
                    Token("export", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            Token("async"),
                            Token("to")),
                        Token("to")),
                    Token("sql"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(
                        First(
                            Token("<|"),
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape63),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                RequiredToken("<|"),
                                shape64)),
                        () => CreateMissingToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    shape238));

            var ExportToExternalTable = Command("ExportToExternalTable", 
                Custom(
                    Token("export", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            Token("async"),
                            Token("to")),
                        Token("to")),
                    Token("table"),
                    Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                    Required(
                        First(
                            Token("<|"),
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape63),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                RequiredToken("<|"),
                                shape64)),
                        () => CreateMissingToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    shape239));

            var ExportToStorage = Command("ExportToStorage", 
                Custom(
                    Token("export", CompletionKind.CommandPrefix),
                    Optional(
                        First(
                            Custom(
                                Token("async"),
                                Optional(Token("compressed")),
                                shape167),
                            Token("compressed"))),
                    RequiredToken("to"),
                    RequiredToken("csv", "json", "parquet", "tsv"),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                shape0),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingStringLiteral,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                    RequiredToken(")"),
                    Required(
                        First(
                            Token("<|"),
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape63),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                RequiredToken("<|"),
                                shape64)),
                        () => CreateMissingToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    shape240));

            var IngestInlineIntoTable = Command("IngestInlineIntoTable", 
                Custom(
                    Token("ingest", CompletionKind.CommandPrefix),
                    Token("inline"),
                    RequiredToken("into"),
                    RequiredToken("table"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Required(
                        First(
                            Custom(
                                Token("["),
                                Required(rules.BracketedInputText, rules.MissingInputText),
                                RequiredToken("]"),
                                shape241),
                            Custom(
                                Token("<|"),
                                Required(rules.InputText, rules.MissingInputText),
                                shape242),
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            First(
                                                Token("creationTime"),
                                                Token("distributed"),
                                                Token("docstring"),
                                                Token("extend_schema"),
                                                Token("folder"),
                                                Token("format"),
                                                Token("ignoreFirstRecord"),
                                                Token("ingestIfNotExists"),
                                                Token("ingestionMappingReference"),
                                                Token("ingestionMapping"),
                                                Token("persistDetails"),
                                                Token("policy_ingestionTime"),
                                                Token("recreate_schema"),
                                                Token("tags"),
                                                Token("validationPolicy"),
                                                Token("zipPattern"),
                                                If(Not(And(Token("creationTime", "distributed", "docstring", "extend_schema", "folder", "format", "ignoreFirstRecord", "ingestIfNotExists", "ingestionMappingReference", "ingestionMapping", "persistDetails", "policy_ingestionTime", "recreate_schema", "tags", "validationPolicy", "zipPattern"))), rules.NameDeclarationOrStringLiteral)),
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape48),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingToken("creationTime"), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingToken("creationTime"), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                RequiredToken("<|"),
                                Required(rules.InputText, rules.MissingInputText),
                                shape243)),
                        () => (SyntaxElement)new CustomNode(shape241, CreateMissingToken("["), rules.MissingInputText(), CreateMissingToken("]"))),
                    shape244));

            var IngestIntoTable = Command("IngestIntoTable", 
                Custom(
                    Token("ingest", CompletionKind.CommandPrefix),
                    Required(
                        First(
                            Custom(
                                Token("async"),
                                RequiredToken("into")),
                            Token("into")),
                        () => (SyntaxElement)new CustomNode(CreateMissingToken("async"), CreateMissingToken("into"))),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Required(
                        First(
                            Custom(
                                Token("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.StringLiteral,
                                            shape0),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingStringLiteral,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                                RequiredToken(")"),
                                shape245),
                            Custom(
                                rules.StringLiteral,
                                shape0)),
                        () => (SyntaxElement)new CustomNode(shape245, CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral())), CreateMissingToken(")"))),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        First(
                                            Token("creationTime"),
                                            Token("distributed"),
                                            Token("docstring"),
                                            Token("extend_schema"),
                                            Token("folder"),
                                            Token("format"),
                                            Token("ignoreFirstRecord"),
                                            Token("ingestIfNotExists"),
                                            Token("ingestionMappingReference"),
                                            Token("ingestionMapping"),
                                            Token("persistDetails"),
                                            Token("policy_ingestionTime"),
                                            Token("recreate_schema"),
                                            Token("tags"),
                                            Token("validationPolicy"),
                                            Token("zipPattern"),
                                            If(Not(And(Token("creationTime", "distributed", "docstring", "extend_schema", "folder", "format", "ignoreFirstRecord", "ingestIfNotExists", "ingestionMappingReference", "ingestionMapping", "persistDetails", "policy_ingestionTime", "recreate_schema", "tags", "validationPolicy", "zipPattern"))), rules.NameDeclarationOrStringLiteral)),
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape48),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingToken("creationTime"), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingToken("creationTime"), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"))),
                    shape246));

            var MergeExtentsDryrun = Command("MergeExtentsDryrun", 
                Custom(
                    Token("merge", CompletionKind.CommandPrefix),
                    Token("dryrun"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.AnyGuidLiteralOrString,
                                shape0),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingValue,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingValue()))),
                    RequiredToken(")"),
                    shape247));

            var MergeExtents = Command("MergeExtents", 
                Custom(
                    Token("merge", CompletionKind.CommandPrefix),
                    Required(
                        First(
                            Custom(
                                Token("async"),
                                Required(rules.TableNameReference, rules.MissingNameReference),
                                shape157),
                            Custom(
                                If(Not(And(Token("dryrun", "async"))), rules.TableNameReference),
                                shape19)),
                        () => (SyntaxElement)new CustomNode(shape157, CreateMissingToken("async"), rules.MissingNameReference())),
                    RequiredToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.AnyGuidLiteralOrString,
                                shape0),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: rules.MissingValue,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingValue()))),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            RequiredToken("rebuild"),
                            RequiredToken("="),
                            RequiredToken("true"),
                            RequiredToken(")"))),
                    shape248));

            var MoveExtentsFrom = Command("MoveExtentsFrom", 
                Custom(
                    Token("move", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            Token("async"),
                            Token("extents")),
                        Token("extents")),
                    First(
                        Custom(
                            Token("("),
                            OList(
                                primaryElementParser: Custom(
                                    rules.AnyGuidLiteralOrString,
                                    shape0),
                                separatorParser: Token(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingValue,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            Token(")"),
                            shape245),
                        Custom(
                            Token("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.AnyGuidLiteralOrString,
                                        shape0),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: rules.MissingValue,
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingValue()))),
                            RequiredToken(")"),
                            shape245),
                        Token("all")),
                    RequiredToken("from"),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredToken("to"),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    shape249));

            var MoveExtentsQuery = Command("MoveExtentsQuery", 
                Custom(
                    Token("move", CompletionKind.CommandPrefix),
                    Required(
                        First(
                            Custom(
                                Token("async"),
                                RequiredToken("extents")),
                            Token("extents")),
                        () => (SyntaxElement)new CustomNode(CreateMissingToken("async"), CreateMissingToken("extents"))),
                    RequiredToken("to"),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredToken("<|"),
                    Required(rules.CommandInput, rules.MissingExpression),
                    shape250));

            var PatchExtentCorruptedDatetime = Command("PatchExtentCorruptedDatetime", 
                Custom(
                    Token("patch"),
                    Required(
                        First(
                            Token("extents"),
                            Custom(
                                Token("table"),
                                Required(rules.TableNameReference, rules.MissingNameReference),
                                RequiredToken("extents"),
                                shape251)),
                        () => CreateMissingToken("extents")),
                    RequiredToken("corrupted"),
                    RequiredToken("datetime")).Hide());

            var RenameColumns = Command("RenameColumns", 
                Custom(
                    Token("rename", CompletionKind.CommandPrefix),
                    Token("columns"),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.NameDeclarationOrStringLiteral,
                                RequiredToken("="),
                                Required(rules.DatabaseTableColumnNameReference, rules.MissingNameReference),
                                shape252),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape252, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingNameReference()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape252, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingNameReference())))),
                    shape253));

            var RenameColumn = Command("RenameColumn", 
                Custom(
                    Token("rename", CompletionKind.CommandPrefix),
                    Token("column"),
                    Required(rules.DatabaseTableColumnNameReference, rules.MissingNameReference),
                    RequiredToken("to"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    shape254));

            var RenameMaterializedView = Command("RenameMaterializedView", 
                Custom(
                    Token("rename", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    RequiredToken("to"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    shape255));

            var RenameTables = Command("RenameTables", 
                Custom(
                    Token("rename", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.NameDeclarationOrStringLiteral,
                                RequiredToken("="),
                                Required(rules.TableNameReference, rules.MissingNameReference),
                                shape256),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape256, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingNameReference()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape256, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingNameReference())))),
                    shape253));

            var RenameTable = Command("RenameTable", 
                Custom(
                    Token("rename", CompletionKind.CommandPrefix),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredToken("to"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    shape257));

            var ReplaceExtents = Command("ReplaceExtents", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("replace", CompletionKind.CommandPrefix),
                        Required(
                            First(
                                Custom(
                                    Token("async"),
                                    RequiredToken("extents")),
                                Token("extents")),
                            () => (SyntaxElement)new CustomNode(CreateMissingToken("async"), CreateMissingToken("extents"))),
                        RequiredToken("in"),
                        RequiredToken("table"),
                        Required(rules.TableNameReference, rules.MissingNameReference),
                        RequiredToken("<|"),
                        RequiredToken("{"),
                        Required(rules.CommandInput, rules.MissingExpression),
                        RequiredToken("}"),
                        RequiredToken(","),
                        RequiredToken("{"),
                        Required(rules.CommandInput, rules.MissingExpression),
                        RequiredToken("}")}
                    ,
                    shape258));

            var SetOrAppendTable = Command("SetOrAppendTable", 
                Custom(
                    Token("set-or-append", CompletionKind.CommandPrefix),
                    Required(
                        First(
                            Custom(
                                Token("async"),
                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                shape259),
                            Custom(
                                If(Not(Token("async")), rules.NameDeclarationOrStringLiteral),
                                shape46)),
                        () => (SyntaxElement)new CustomNode(shape259, CreateMissingToken("async"), rules.MissingNameDeclaration())),
                    Required(
                        First(
                            Token("<|"),
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            First(
                                                Token("creationTime"),
                                                Token("distributed"),
                                                Token("docstring"),
                                                Token("extend_schema"),
                                                Token("folder"),
                                                Token("format"),
                                                Token("ignoreFirstRecord"),
                                                Token("ingestIfNotExists"),
                                                Token("ingestionMappingReference"),
                                                Token("ingestionMapping"),
                                                Token("persistDetails"),
                                                Token("policy_ingestionTime"),
                                                Token("recreate_schema"),
                                                Token("tags"),
                                                Token("validationPolicy"),
                                                Token("zipPattern"),
                                                If(Not(And(Token("creationTime", "distributed", "docstring", "extend_schema", "folder", "format", "ignoreFirstRecord", "ingestIfNotExists", "ingestionMappingReference", "ingestionMapping", "persistDetails", "policy_ingestionTime", "recreate_schema", "tags", "validationPolicy", "zipPattern"))), rules.NameDeclarationOrStringLiteral)),
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape48),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingToken("creationTime"), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingToken("creationTime"), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                RequiredToken("<|"))),
                        () => CreateMissingToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    shape158));

            var StoredQueryResultSetOrReplace = Command("StoredQueryResultSetOrReplace", 
                Custom(
                    Token("set-or-replace", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            Token("async"),
                            Token("stored_query_result")),
                        Token("stored_query_result")),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Required(
                        First(
                            Token("<|"),
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            First(
                                                Token("creationTime"),
                                                Token("distributed"),
                                                Token("docstring"),
                                                Token("extend_schema"),
                                                Token("folder"),
                                                Token("format"),
                                                Token("ignoreFirstRecord"),
                                                Token("ingestIfNotExists"),
                                                Token("ingestionMappingReference"),
                                                Token("ingestionMapping"),
                                                Token("persistDetails"),
                                                Token("policy_ingestionTime"),
                                                Token("recreate_schema"),
                                                Token("tags"),
                                                Token("validationPolicy"),
                                                Token("zipPattern"),
                                                If(Not(And(Token("creationTime", "distributed", "docstring", "extend_schema", "folder", "format", "ignoreFirstRecord", "ingestIfNotExists", "ingestionMappingReference", "ingestionMapping", "persistDetails", "policy_ingestionTime", "recreate_schema", "tags", "validationPolicy", "zipPattern"))), rules.NameDeclarationOrStringLiteral)),
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape48),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingToken("creationTime"), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingToken("creationTime"), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                RequiredToken("<|"))),
                        () => CreateMissingToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    shape260));

            var SetOrReplaceTable = Command("SetOrReplaceTable", 
                Custom(
                    Token("set-or-replace", CompletionKind.CommandPrefix),
                    Required(
                        First(
                            Custom(
                                Token("async"),
                                Required(If(Not(Token("stored_query_result")), rules.NameDeclarationOrStringLiteral), rules.MissingNameDeclaration),
                                shape259),
                            Custom(
                                If(Not(And(Token("async", "stored_query_result"))), rules.NameDeclarationOrStringLiteral),
                                shape46)),
                        () => (SyntaxElement)new CustomNode(shape259, CreateMissingToken("async"), rules.MissingNameDeclaration())),
                    Required(
                        First(
                            Token("<|"),
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            First(
                                                Token("creationTime"),
                                                Token("distributed"),
                                                Token("docstring"),
                                                Token("extend_schema"),
                                                Token("folder"),
                                                Token("format"),
                                                Token("ignoreFirstRecord"),
                                                Token("ingestIfNotExists"),
                                                Token("ingestionMappingReference"),
                                                Token("ingestionMapping"),
                                                Token("persistDetails"),
                                                Token("policy_ingestionTime"),
                                                Token("recreate_schema"),
                                                Token("tags"),
                                                Token("validationPolicy"),
                                                Token("zipPattern"),
                                                If(Not(And(Token("creationTime", "distributed", "docstring", "extend_schema", "folder", "format", "ignoreFirstRecord", "ingestIfNotExists", "ingestionMappingReference", "ingestionMapping", "persistDetails", "policy_ingestionTime", "recreate_schema", "tags", "validationPolicy", "zipPattern"))), rules.NameDeclarationOrStringLiteral)),
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape48),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingToken("creationTime"), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingToken("creationTime"), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                RequiredToken("<|"))),
                        () => CreateMissingToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    shape158));

            var SetAccess = Command("SetAccess", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("access"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("to"),
                    RequiredToken("readonly", "readwrite"),
                    shape261));

            var SetClusterRole = Command("SetClusterRole", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("admins", "databasecreators", "users", "viewers"),
                    Required(
                        First(
                            Custom(
                                Token("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.StringLiteral,
                                            shape0),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingStringLiteral,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                                RequiredToken(")"),
                                Optional(
                                    First(
                                        Custom(
                                            Token("skip-results"),
                                            Optional(
                                                Custom(
                                                    rules.StringLiteral,
                                                    shape0)),
                                            shape6),
                                        Custom(
                                            rules.StringLiteral,
                                            shape0))),
                                shape213),
                            Custom(
                                Token("none"),
                                Optional(
                                    Custom(
                                        Token("skip-results"))),
                                shape167)),
                        () => (SyntaxElement)new CustomNode(shape213, CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral())), CreateMissingToken(")"), (SyntaxElement)new CustomNode(shape6, CreateMissingToken("skip-results"), rules.MissingStringLiteral()))),
                    shape262));

            var SetContinuousExportCursor = Command("SetContinuousExportCursor", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("continuous-export"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    RequiredToken("cursor"),
                    RequiredToken("to"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape263));

            var SetDatabaseRole = Command("SetDatabaseRole", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("admins", "ingestors", "monitors", "unrestrictedviewers", "users", "viewers"),
                    Required(
                        First(
                            Custom(
                                Token("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.StringLiteral,
                                            shape0),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingStringLiteral,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                                RequiredToken(")"),
                                Optional(
                                    First(
                                        Custom(
                                            Token("skip-results"),
                                            Optional(
                                                Custom(
                                                    rules.StringLiteral,
                                                    shape0)),
                                            shape6),
                                        Custom(
                                            rules.StringLiteral,
                                            shape0))),
                                shape213),
                            Custom(
                                Token("none"),
                                Optional(
                                    Custom(
                                        Token("skip-results"))),
                                shape167)),
                        () => (SyntaxElement)new CustomNode(shape213, CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral())), CreateMissingToken(")"), (SyntaxElement)new CustomNode(shape6, CreateMissingToken("skip-results"), rules.MissingStringLiteral()))),
                    shape264));

            var SetExternalTableAdmins = Command("SetExternalTableAdmins", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("external"),
                    RequiredToken("table"),
                    Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                    RequiredToken("admins"),
                    Required(
                        First(
                            Custom(
                                Token("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.StringLiteral,
                                            shape0),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingStringLiteral,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                                RequiredToken(")"),
                                Optional(
                                    First(
                                        Custom(
                                            Token("skip-results"),
                                            Optional(
                                                Custom(
                                                    rules.StringLiteral,
                                                    shape0)),
                                            shape11),
                                        Custom(
                                            rules.StringLiteral,
                                            shape0))),
                                shape213),
                            Custom(
                                Token("none"),
                                Optional(Token("skip-results")),
                                shape167)),
                        () => (SyntaxElement)new CustomNode(shape213, CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral())), CreateMissingToken(")"), (SyntaxElement)new CustomNode(shape11, CreateMissingToken("skip-results"), rules.MissingStringLiteral()))),
                    shape265));

            var SetFunctionRole = Command("SetFunctionRole", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("function"),
                    Required(rules.DatabaseFunctionNameReference, rules.MissingNameReference),
                    RequiredToken("admins"),
                    Required(
                        First(
                            Custom(
                                Token("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.StringLiteral,
                                            shape0),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingStringLiteral,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                                RequiredToken(")"),
                                Optional(
                                    First(
                                        Custom(
                                            Token("skip-results"),
                                            Optional(
                                                Custom(
                                                    rules.StringLiteral,
                                                    shape0)),
                                            shape6),
                                        Custom(
                                            rules.StringLiteral,
                                            shape0))),
                                shape213),
                            Custom(
                                Token("none"),
                                Optional(
                                    Custom(
                                        Token("skip-results"))),
                                shape167)),
                        () => (SyntaxElement)new CustomNode(shape213, CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral())), CreateMissingToken(")"), (SyntaxElement)new CustomNode(shape6, CreateMissingToken("skip-results"), rules.MissingStringLiteral()))),
                    shape266));

            var SetMaterializedViewAdmins = Command("SetMaterializedViewAdmins", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("admins"),
                    Required(
                        First(
                            Custom(
                                Token("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.StringLiteral,
                                            shape0),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingStringLiteral,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                                RequiredToken(")"),
                                Optional(
                                    Custom(
                                        rules.StringLiteral,
                                        shape0)),
                                shape267),
                            Token("none")),
                        () => (SyntaxElement)new CustomNode(shape267, CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral())), CreateMissingToken(")"), rules.MissingStringLiteral())),
                    shape268));

            var SetMaterializedViewConcurrency = Command("SetMaterializedViewConcurrency", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("concurrency"),
                    Optional(
                        Custom(
                            Token("="),
                            Required(
                                First(
                                    rules.Value,
                                    rules.Value),
                                rules.MissingValue),
                            shape269)),
                    shape270));

            var SetMaterializedViewCursor = Command("SetMaterializedViewCursor", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    RequiredToken("cursor"),
                    RequiredToken("to"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape271));

            var StoredQueryResultSet = Command("StoredQueryResultSet", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            Token("async"),
                            Token("stored_query_result")),
                        Token("stored_query_result")),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Required(
                        First(
                            Token("<|"),
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            First(
                                                Token("creationTime"),
                                                Token("distributed"),
                                                Token("docstring"),
                                                Token("extend_schema"),
                                                Token("folder"),
                                                Token("format"),
                                                Token("ignoreFirstRecord"),
                                                Token("ingestIfNotExists"),
                                                Token("ingestionMappingReference"),
                                                Token("ingestionMapping"),
                                                Token("persistDetails"),
                                                Token("policy_ingestionTime"),
                                                Token("recreate_schema"),
                                                Token("tags"),
                                                Token("validationPolicy"),
                                                Token("zipPattern"),
                                                If(Not(And(Token("creationTime", "distributed", "docstring", "extend_schema", "folder", "format", "ignoreFirstRecord", "ingestIfNotExists", "ingestionMappingReference", "ingestionMapping", "persistDetails", "policy_ingestionTime", "recreate_schema", "tags", "validationPolicy", "zipPattern"))), rules.NameDeclarationOrStringLiteral)),
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape48),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingToken("creationTime"), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingToken("creationTime"), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                RequiredToken("<|"))),
                        () => CreateMissingToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    shape260));

            var SetTableRowStoreReferences = Command("SetTableRowStoreReferences", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    RequiredToken("rowstore_references"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape272));

            var SetTableRole = Command("SetTableRole", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredToken("admins", "ingestors"),
                    Required(
                        First(
                            Custom(
                                Token("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.StringLiteral,
                                            shape0),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingStringLiteral,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                                RequiredToken(")"),
                                Optional(
                                    First(
                                        Custom(
                                            Token("skip-results"),
                                            Optional(
                                                Custom(
                                                    rules.StringLiteral,
                                                    shape0)),
                                            shape6),
                                        Custom(
                                            rules.StringLiteral,
                                            shape0))),
                                shape213),
                            Custom(
                                Token("none"),
                                Optional(
                                    Custom(
                                        Token("skip-results"))),
                                shape167)),
                        () => (SyntaxElement)new CustomNode(shape213, CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral())), CreateMissingToken(")"), (SyntaxElement)new CustomNode(shape6, CreateMissingToken("skip-results"), rules.MissingStringLiteral()))),
                    shape273));

            var SetTable = Command("SetTable", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Required(
                        First(
                            Custom(
                                Token("async"),
                                Required(If(Not(Token("stored_query_result")), rules.NameDeclarationOrStringLiteral), rules.MissingNameDeclaration),
                                shape259),
                            Custom(
                                If(Not(And(Token("access", "cluster", "continuous-export", "database", "external", "function", "materialized-view", "async", "stored_query_result", "table"))), rules.NameDeclarationOrStringLiteral),
                                shape46)),
                        () => (SyntaxElement)new CustomNode(shape259, CreateMissingToken("async"), rules.MissingNameDeclaration())),
                    Required(
                        First(
                            Token("<|"),
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            First(
                                                Token("creationTime"),
                                                Token("distributed"),
                                                Token("docstring"),
                                                Token("extend_schema"),
                                                Token("folder"),
                                                Token("format"),
                                                Token("ignoreFirstRecord"),
                                                Token("ingestIfNotExists"),
                                                Token("ingestionMappingReference"),
                                                Token("ingestionMapping"),
                                                Token("persistDetails"),
                                                Token("policy_ingestionTime"),
                                                Token("recreate_schema"),
                                                Token("tags"),
                                                Token("validationPolicy"),
                                                Token("zipPattern"),
                                                If(Not(And(Token("creationTime", "distributed", "docstring", "extend_schema", "folder", "format", "ignoreFirstRecord", "ingestIfNotExists", "ingestionMappingReference", "ingestionMapping", "persistDetails", "policy_ingestionTime", "recreate_schema", "tags", "validationPolicy", "zipPattern"))), rules.NameDeclarationOrStringLiteral)),
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape48),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingToken("creationTime"), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingToken("creationTime"), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                RequiredToken("<|"))),
                        () => CreateMissingToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    shape158));

            var ShowBasicAuthUsers = Command("ShowBasicAuthUsers", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("basicauth"),
                    RequiredToken("users")));

            var ShowCache = Command("ShowCache", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cache")));

            var ShowCallStacks = Command("ShowCallStacks", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("callstacks"),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape184));

            var ShowCapacity = Command("ShowCapacity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("capacity"),
                    Optional(
                        First(
                            Custom(
                                Token("data-export", "extents-merge", "extents-partition", "ingestions", "periodic-storage-artifacts-cleanup", "purge-storage-artifacts-cleanup", "queries", "stored-query-results", "streaming-ingestion-post-processing", "table-purge"),
                                Optional(
                                    Custom(
                                        Token("with"),
                                        RequiredToken("("),
                                        RequiredToken("scope"),
                                        RequiredToken("="),
                                        RequiredToken("cluster", "workloadgroup"),
                                        RequiredToken(")"),
                                        shape274)),
                                shape275),
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                RequiredToken("scope"),
                                RequiredToken("="),
                                RequiredToken("cluster", "workloadgroup"),
                                RequiredToken(")"),
                                shape274))),
                    shape184));

            var ShowClusterAdminState = Command("ShowClusterAdminState", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("admin"),
                    RequiredToken("state")));

            var ShowClusterBlockedPrincipals = Command("ShowClusterBlockedPrincipals", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("blockedprincipals")));

            var ShowClusterExtentsMetadata = Command("ShowClusterExtentsMetadata", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("extents"),
                    Optional(
                        First(
                            Custom(
                                Token("("),
                                OList(
                                    primaryElementParser: Custom(
                                        rules.AnyGuidLiteralOrString,
                                        shape0),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: rules.MissingValue,
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                Token(")"),
                                Optional(Token("hot")),
                                shape213),
                            Token("hot"))),
                    Token("metadata"),
                    Optional(
                        First(
                            Custom(
                                Token("where"),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            Token("tags"),
                                            RequiredToken("!contains", "!has", "contains", "has"),
                                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                                            shape276),
                                        separatorParser: Token("and"),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape276, CreateMissingToken("tags"), CreateMissingToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape276, CreateMissingToken("tags"), CreateMissingToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral())))),
                                Optional(
                                    Custom(
                                        Token("with"),
                                        RequiredToken("("),
                                        RequiredToken("extentsShowFilteringRuntimePolicy"),
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        RequiredToken(")"),
                                        shape277)),
                                shape184),
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                RequiredToken("extentsShowFilteringRuntimePolicy"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredToken(")"),
                                shape277))),
                    shape278));

            var ShowClusterExtents = Command("ShowClusterExtents", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("extents"),
                    Optional(
                        First(
                            Custom(
                                Token("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.AnyGuidLiteralOrString,
                                            shape0),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingValue,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingValue()))),
                                RequiredToken(")"),
                                Optional(Token("hot")),
                                shape213),
                            Token("hot"))),
                    Optional(
                        First(
                            Custom(
                                Token("where"),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            Token("tags"),
                                            RequiredToken("!contains", "!has", "contains", "has"),
                                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                                            shape276),
                                        separatorParser: Token("and"),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape276, CreateMissingToken("tags"), CreateMissingToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape276, CreateMissingToken("tags"), CreateMissingToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral())))),
                                Optional(
                                    Custom(
                                        Token("with"),
                                        RequiredToken("("),
                                        RequiredToken("extentsShowFilteringRuntimePolicy"),
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        RequiredToken(")"),
                                        shape277)),
                                shape184),
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                RequiredToken("extentsShowFilteringRuntimePolicy"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredToken(")"),
                                shape277))),
                    shape279));

            var ShowClusterJournal = Command("ShowClusterJournal", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("journal")));

            var ShowClusterMonitoring = Command("ShowClusterMonitoring", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("monitoring")));

            var ShowClusterNetwork = Command("ShowClusterNetwork", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("network"),
                    Optional(
                        Custom(
                            rules.Value,
                            shape0)),
                    shape280));

            var ShowClusterPendingContinuousExports = Command("ShowClusterPendingContinuousExports", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("pending"),
                    RequiredToken("continuous-exports"),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape204));

            var ShowClusterPolicyCaching = Command("ShowClusterPolicyCaching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("caching")));

            var ShowClusterPolicyCallout = Command("ShowClusterPolicyCallout", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("callout")));

            var ShowClusterPolicyCapacity = Command("ShowClusterPolicyCapacity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("capacity")));

            var ShowClusterPolicyDiagnostics = Command("ShowClusterPolicyDiagnostics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("diagnostics")));

            var ShowClusterPolicyIngestionBatching = Command("ShowClusterPolicyIngestionBatching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("ingestionbatching")));

            var ShowClusterPolicyManagedIdentity = Command("ShowClusterPolicyManagedIdentity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("managed_identity")));

            var ShowClusterPolicyMultiDatabaseAdmins = Command("ShowClusterPolicyMultiDatabaseAdmins", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("multidatabaseadmins")));

            var ShowClusterPolicyQueryWeakConsistency = Command("ShowClusterPolicyQueryWeakConsistency", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("query_weak_consistency")));

            var ShowClusterPolicyRequestClassification = Command("ShowClusterPolicyRequestClassification", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("request_classification")));

            var ShowClusterPolicyRowStore = Command("ShowClusterPolicyRowStore", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("rowstore")));

            var ShowClusterPolicySandbox = Command("ShowClusterPolicySandbox", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("sandbox")));

            var ShowClusterPolicySharding = Command("ShowClusterPolicySharding", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("sharding"),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape204));

            var ShowClusterPolicyStreamingIngestion = Command("ShowClusterPolicyStreamingIngestion", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    RequiredToken("streamingingestion")));

            var ShowClusterPrincipals = Command("ShowClusterPrincipals", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("principals")));

            var ShowClusterPrincipalRoles = Command("ShowClusterPrincipalRoles", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("principal"),
                    Required(
                        First(
                            Token("roles"),
                            Custom(
                                rules.StringLiteral,
                                RequiredToken("roles"),
                                shape281)),
                        () => CreateMissingToken("roles")),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape204));

            var ShowClusterSandboxesStats = Command("ShowClusterSandboxesStats", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("sandboxes"),
                    RequiredToken("stats")));

            var ShowClusterScaleIn = Command("ShowClusterScaleIn", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("scalein"),
                    Required(
                        First(
                            rules.Value,
                            rules.Value),
                        rules.MissingValue),
                    RequiredToken("nodes"),
                    shape282));

            var ShowClusterStorageKeysHash = Command("ShowClusterStorageKeysHash", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("storage"),
                    RequiredToken("keys"),
                    RequiredToken("hash")));

            var ShowCluster = Command("ShowCluster", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster")));

            var ShowColumnPolicyCaching = Command("ShowColumnPolicyCaching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("column"),
                    First(
                        Token("*"),
                        If(Not(Token("*")), rules.DatabaseTableColumnNameReference)),
                    RequiredToken("policy"),
                    RequiredToken("caching"),
                    shape283));

            var ShowColumnPolicyEncoding = Command("ShowColumnPolicyEncoding", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("column"),
                    Required(If(Not(Token("*")), rules.TableColumnNameReference), rules.MissingNameReference),
                    RequiredToken("policy"),
                    RequiredToken("encoding"),
                    shape190));

            var ShowCommandsAndQueries = Command("ShowCommandsAndQueries", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("commands-and-queries")));

            var ShowCommands = Command("ShowCommands", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("commands")));

            var ShowContinuousExports = Command("ShowContinuousExports", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("continuous-exports")));

            var ShowContinuousExportExportedArtifacts = Command("ShowContinuousExportExportedArtifacts", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("continuous-export"),
                    rules.NameDeclarationOrStringLiteral,
                    Token("exported-artifacts"),
                    shape284));

            var ShowContinuousExportFailures = Command("ShowContinuousExportFailures", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("continuous-export"),
                    rules.NameDeclarationOrStringLiteral,
                    Token("failures"),
                    shape284));

            var ShowContinuousExport = Command("ShowContinuousExport", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("continuous-export"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    shape208));

            var ShowDatabasesSchemaAsJson = Command("ShowDatabasesSchemaAsJson", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    OList(
                        primaryElementParser: Custom(
                            rules.DatabaseNameReference,
                            Optional(
                                Custom(
                                    Token("if_later_than"),
                                    rules.StringLiteral,
                                    shape285)),
                            shape286),
                        separatorParser: Token(","),
                        secondaryElementParser: null,
                        missingPrimaryElement: null,
                        missingSeparator: null,
                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape286, rules.MissingNameReference(), (SyntaxElement)new CustomNode(shape285, CreateMissingToken("if_later_than"), rules.MissingStringLiteral())),
                        endOfList: null,
                        oneOrMore: true,
                        allowTrailingSeparator: false,
                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                    Token(")"),
                    Token("schema"),
                    Token("as"),
                    RequiredToken("json"),
                    shape287));

            var ShowDatabasesSchema = Command("ShowDatabasesSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.DatabaseNameReference,
                                Optional(
                                    Custom(
                                        Token("if_later_than"),
                                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                                        shape285)),
                                shape286),
                            separatorParser: Token(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape286, rules.MissingNameReference(), (SyntaxElement)new CustomNode(shape285, CreateMissingToken("if_later_than"), rules.MissingStringLiteral())),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape286, rules.MissingNameReference(), (SyntaxElement)new CustomNode(shape285, CreateMissingToken("if_later_than"), rules.MissingStringLiteral()))))),
                    RequiredToken(")"),
                    RequiredToken("schema"),
                    Optional(Token("details")),
                    shape288));

            var ShowClusterDatabasesDataStats = Command("ShowClusterDatabasesDataStats", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            Token("cluster"),
                            Token("databases")),
                        Token("databases")),
                    Token("datastats")));

            var ShowClusterDatabasesDetails = Command("ShowClusterDatabasesDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            Token("cluster"),
                            Token("databases")),
                        Token("databases")),
                    Token("details")));

            var ShowClusterDatabasesIdentity = Command("ShowClusterDatabasesIdentity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            Token("cluster"),
                            Token("databases")),
                        Token("databases")),
                    RequiredToken("identity")));

            var ShowDatabasesManagementGroups = Command("ShowDatabasesManagementGroups", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("management"),
                    RequiredToken("groups")));

            var ShowClusterDatabasesPolicies = Command("ShowClusterDatabasesPolicies", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            Token("cluster"),
                            Token("databases")),
                        Token("databases")),
                    Token("policies")));

            var ShowClusterDatabases = Command("ShowClusterDatabases", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            Token("cluster"),
                            Token("databases")),
                        Custom(
                            Token("cluster"),
                            RequiredToken("databases")),
                        Token("databases")),
                    Optional(
                        Custom(
                            Token("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.DatabaseNameReference,
                                        shape8),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: rules.MissingNameReference,
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameReference()))),
                            RequiredToken(")"),
                            shape166)),
                    shape184));

            var ShowDatabaseCacheQueryResults = Command("ShowDatabaseCacheQueryResults", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("cache"),
                    RequiredToken("query_results")));

            var ShowDatabaseDataStats = Command("ShowDatabaseDataStats", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("datastats")));

            var ShowDatabaseDetails = Command("ShowDatabaseDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("details")));

            var ShowDatabaseExtentsMetadata = Command("ShowDatabaseExtentsMetadata", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            Token("databases"),
                            Token("("),
                            OList(
                                primaryElementParser: Custom(
                                    rules.DatabaseNameReference,
                                    shape8),
                                separatorParser: Token(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingNameReference,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            Token(")"),
                            shape220),
                        Custom(
                            Token("database"),
                            Optional(
                                Custom(
                                    If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                                    shape8)),
                            shape289)),
                    Token("extents"),
                    Optional(
                        First(
                            Custom(
                                Token("("),
                                OList(
                                    primaryElementParser: Custom(
                                        rules.AnyGuidLiteralOrString,
                                        shape0),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: rules.MissingValue,
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                Token(")"),
                                Optional(Token("hot")),
                                shape213),
                            Token("hot"))),
                    Token("metadata"),
                    Optional(
                        First(
                            Custom(
                                Token("where"),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            Token("tags"),
                                            RequiredToken("!contains", "!has", "contains", "has"),
                                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                                            shape276),
                                        separatorParser: Token("and"),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape276, CreateMissingToken("tags"), CreateMissingToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape276, CreateMissingToken("tags"), CreateMissingToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral())))),
                                Optional(
                                    Custom(
                                        Token("with"),
                                        RequiredToken("("),
                                        RequiredToken("extentsShowFilteringRuntimePolicy"),
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        RequiredToken(")"),
                                        shape277)),
                                shape184),
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                RequiredToken("extentsShowFilteringRuntimePolicy"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredToken(")"),
                                shape277))),
                    shape278));

            var ShowDatabaseExtents = Command("ShowDatabaseExtents", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            Token("databases"),
                            Token("("),
                            OList(
                                primaryElementParser: Custom(
                                    rules.DatabaseNameReference,
                                    shape8),
                                separatorParser: Token(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingNameReference,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            Token(")"),
                            shape220),
                        Custom(
                            Token("databases"),
                            Token("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.DatabaseNameReference,
                                        shape8),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: rules.MissingNameReference,
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameReference()))),
                            RequiredToken(")"),
                            shape220),
                        Custom(
                            Token("database"),
                            Optional(
                                Custom(
                                    If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                                    shape8)),
                            shape289)),
                    RequiredToken("extents"),
                    Optional(
                        First(
                            Custom(
                                Token("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.AnyGuidLiteralOrString,
                                            shape0),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingValue,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingValue()))),
                                RequiredToken(")"),
                                Optional(Token("hot")),
                                shape213),
                            Token("hot"))),
                    Optional(
                        First(
                            Custom(
                                Token("where"),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            Token("tags"),
                                            RequiredToken("!contains", "!has", "contains", "has"),
                                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                                            shape276),
                                        separatorParser: Token("and"),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape276, CreateMissingToken("tags"), CreateMissingToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape276, CreateMissingToken("tags"), CreateMissingToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral())))),
                                Optional(
                                    Custom(
                                        Token("with"),
                                        RequiredToken("("),
                                        RequiredToken("extentsShowFilteringRuntimePolicy"),
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        RequiredToken(")"),
                                        shape277)),
                                shape184),
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                RequiredToken("extentsShowFilteringRuntimePolicy"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredToken(")"),
                                shape277))),
                    shape279));

            var ShowDatabaseExtentTagsStatistics = Command("ShowDatabaseExtentTagsStatistics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("extent"),
                    RequiredToken("tags"),
                    RequiredToken("statistics"),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            RequiredToken("minCreationTime"),
                            RequiredToken("="),
                            Required(rules.Value, rules.MissingValue),
                            RequiredToken(")"),
                            shape290)),
                    shape291));

            var ShowDatabaseIdentity = Command("ShowDatabaseIdentity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("identity")));

            var ShowDatabasePolicies = Command("ShowDatabasePolicies", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policies")));

            var ShowDatabaseCslSchema = Command("ShowDatabaseCslSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    First(
                        Token("cslschema"),
                        Custom(
                            If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                            Token("cslschema"),
                            shape292)),
                    Optional(
                        First(
                            Custom(
                                Token("if_later_than"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                shape293),
                            Custom(
                                Token("script"),
                                Optional(
                                    Custom(
                                        Token("if_later_than"),
                                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                                        shape293)),
                                shape167))),
                    shape294));

            var ShowDatabaseIngestionMappings = Command("ShowDatabaseIngestionMappings", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    First(
                        Token("ingestion"),
                        Custom(
                            If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                            Token("ingestion"),
                            shape292)),
                    Required(
                        First(
                            Token("mappings"),
                            Custom(
                                Token("apacheavro", "avro", "csv", "json", "orc", "parquet", "sstream", "w3clogfile"),
                                RequiredToken("mappings"),
                                shape295)),
                        () => CreateMissingToken("mappings")),
                    Optional(
                        First(
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape63),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                shape87),
                            Custom(
                                rules.StringLiteral,
                                Optional(
                                    Custom(
                                        Token("with"),
                                        RequiredToken("("),
                                        Required(
                                            OList(
                                                primaryElementParser: Custom(
                                                    rules.NameDeclarationOrStringLiteral,
                                                    RequiredToken("="),
                                                    Required(rules.Value, rules.MissingValue),
                                                    shape63),
                                                separatorParser: Token(","),
                                                secondaryElementParser: null,
                                                missingPrimaryElement: null,
                                                missingSeparator: null,
                                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                                endOfList: null,
                                                oneOrMore: true,
                                                allowTrailingSeparator: false,
                                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                                        RequiredToken(")"),
                                        shape87)),
                                shape296))),
                    shape204));

            var ShowDatabaseSchemaAsCslScript = Command("ShowDatabaseSchemaAsCslScript", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    First(
                        Token("schema"),
                        Custom(
                            If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                            Token("schema"),
                            shape297)),
                    First(
                        Token("as"),
                        Custom(
                            Token("if_later_than"),
                            rules.StringLiteral,
                            Token("as"),
                            shape298)),
                    Token("csl"),
                    RequiredToken("script"),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape299));

            var ShowDatabaseSchemaAsJson = Command("ShowDatabaseSchemaAsJson", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    First(
                        Token("schema"),
                        Custom(
                            If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                            Token("schema"),
                            shape297)),
                    First(
                        Token("as"),
                        Custom(
                            Token("if_later_than"),
                            rules.StringLiteral,
                            Token("as"),
                            shape298)),
                    RequiredToken("json")));

            var ShowDatabaseSchema = Command("ShowDatabaseSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    First(
                        Token("schema"),
                        Custom(
                            If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                            Token("schema"),
                            shape292)),
                    Optional(
                        First(
                            Custom(
                                Token("details"),
                                Optional(
                                    Custom(
                                        Token("if_later_than"),
                                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                                        shape293)),
                                shape167),
                            Custom(
                                Token("if_later_than"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                shape293))),
                    shape294));

            var DatabaseShardGroupsStatisticsShow = Command("DatabaseShardGroupsStatisticsShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    First(
                        Token("shard-groups").Hide(),
                        Custom(
                            If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                            Token("shard-groups").Hide(),
                            shape297)),
                    RequiredToken("statistics").Hide()));

            var ShowDatabaseExtentContainersCleanOperations = Command("ShowDatabaseExtentContainersCleanOperations", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    Token("extentcontainers"),
                    RequiredToken("clean"),
                    RequiredToken("operations"),
                    Optional(
                        Custom(
                            rules.AnyGuidLiteralOrString,
                            shape0)),
                    shape300));

            var ShowDatabaseJournal = Command("ShowDatabaseJournal", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    Token("journal"),
                    shape199));

            var ShowDatabasePolicyCaching = Command("ShowDatabasePolicyCaching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    First(
                        Token("*"),
                        If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference)),
                    Token("policy"),
                    Token("caching"),
                    shape301));

            var ShowDatabasePolicyDiagnostics = Command("ShowDatabasePolicyDiagnostics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("diagnostics"),
                    shape191));

            var ShowDatabasePolicyEncoding = Command("ShowDatabasePolicyEncoding", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("encoding"),
                    shape191));

            var ShowDatabasePolicyExtentTagsRetention = Command("ShowDatabasePolicyExtentTagsRetention", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    First(
                        Token("*"),
                        If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference)),
                    Token("policy"),
                    Token("extent_tags_retention"),
                    shape301));

            var ShowDatabasePolicyHardRetentionViolations = Command("ShowDatabasePolicyHardRetentionViolations", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("hardretention"),
                    RequiredToken("violations"),
                    shape302));

            var ShowDatabasePolicyIngestionBatching = Command("ShowDatabasePolicyIngestionBatching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    First(
                        Token("*"),
                        If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference)),
                    Token("policy"),
                    Token("ingestionbatching"),
                    shape301));

            var ShowDatabasePolicyManagedIdentity = Command("ShowDatabasePolicyManagedIdentity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("managed_identity"),
                    shape191));

            var ShowDatabasePolicyMerge = Command("ShowDatabasePolicyMerge", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    First(
                        Token("*"),
                        If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference)),
                    Token("policy"),
                    Token("merge"),
                    shape301));

            var ShowDatabasePolicyRetention = Command("ShowDatabasePolicyRetention", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    First(
                        Token("*"),
                        If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference)),
                    Token("policy"),
                    Token("retention"),
                    shape301));

            var ShowDatabasePolicySharding = Command("ShowDatabasePolicySharding", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    First(
                        Token("*"),
                        If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference)),
                    Token("policy"),
                    Token("sharding"),
                    shape301));

            var ShowDatabasePolicyShardsGrouping = Command("ShowDatabasePolicyShardsGrouping", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    First(
                        Token("*"),
                        If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference)),
                    Token("policy"),
                    Token("shards_grouping").Hide(),
                    shape301));

            var ShowDatabasePolicySoftRetentionViolations = Command("ShowDatabasePolicySoftRetentionViolations", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("softretention"),
                    RequiredToken("violations"),
                    shape302));

            var ShowDatabasePolicyStreamingIngestion = Command("ShowDatabasePolicyStreamingIngestion", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    Token("policy"),
                    RequiredToken("streamingingestion"),
                    shape191));

            var ShowDatabasePrincipals = Command("ShowDatabasePrincipals", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    Token("principals"),
                    shape199));

            var ShowDatabasePrincipalRoles = Command("ShowDatabasePrincipalRoles", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    Token("principal"),
                    Required(
                        First(
                            Token("roles"),
                            Custom(
                                rules.StringLiteral,
                                RequiredToken("roles"),
                                shape281)),
                        () => CreateMissingToken("roles")),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape303));

            var ShowDatabasePurgeOperation = Command("ShowDatabasePurgeOperation", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    Token("purge"),
                    Required(
                        First(
                            Custom(
                                Token("operations"),
                                Optional(
                                    Custom(
                                        rules.AnyGuidLiteralOrString,
                                        shape0)),
                                shape11),
                            Custom(
                                Token("operation"),
                                Required(rules.AnyGuidLiteralOrString, rules.MissingValue),
                                shape304)),
                        () => (SyntaxElement)new CustomNode(shape11, CreateMissingToken("operations"), rules.MissingValue())),
                    shape82));

            var ShowDatabaseSchemaViolations = Command("ShowDatabaseSchemaViolations", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    RequiredToken("schema"),
                    RequiredToken("violations"),
                    shape82));

            var ShowDatabase = Command("ShowDatabase", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database")));

            var ShowDiagnostics = Command("ShowDiagnostics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("diagnostics"),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            RequiredToken("scope"),
                            RequiredToken("="),
                            RequiredToken("cluster", "workloadgroup"),
                            RequiredToken(")"),
                            shape274)),
                    shape184));

            var ShowEntitySchema = Command("ShowEntitySchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("entity"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    RequiredToken("schema"),
                    RequiredToken("as"),
                    RequiredToken("json"),
                    Optional(
                        First(
                            Custom(
                                Token("except"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                shape305),
                            Custom(
                                Token("in"),
                                RequiredToken("databases"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.StringLiteral,
                                            shape0),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingStringLiteral,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral()))),
                                RequiredToken(")"),
                                Optional(
                                    Custom(
                                        Token("except"),
                                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                                        shape305)),
                                shape248))),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape306));

            var ShowExtentContainers = Command("ShowExtentContainers", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("extentcontainers"),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape184));

            var ShowExtentColumnStorageStats = Command("ShowExtentColumnStorageStats", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("extent"),
                    rules.AnyGuidLiteralOrString,
                    Token("column"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    RequiredToken("storage"),
                    RequiredToken("stats"),
                    shape307));

            var ShowExtentDetails = Command("ShowExtentDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("extent"),
                    Required(
                        First(
                            Custom(
                                Token("details"),
                                Required(
                                    First(
                                        Custom(
                                            rules.AnyGuidLiteralOrString,
                                            shape0),
                                        Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            shape46)),
                                    rules.MissingValue),
                                shape308),
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                shape0),
                            Custom(
                                If(Not(Token("details")), rules.NameDeclarationOrStringLiteral),
                                shape46)),
                        () => (SyntaxElement)new CustomNode(shape308, CreateMissingToken("details"), rules.MissingValue()))));

            var ShowExternalTables = Command("ShowExternalTables", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("tables")));

            var ShowExternalTableArtifacts = Command("ShowExternalTableArtifacts", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    rules.ExternalTableNameReference,
                    Token("artifacts"),
                    Optional(
                        Custom(
                            Token("limit"),
                            Required(rules.Value, rules.MissingValue),
                            shape202)),
                    shape309));

            var ShowExternalTableCslSchema = Command("ShowExternalTableCslSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    rules.ExternalTableNameReference,
                    Token("cslschema"),
                    shape310));

            var ShowExternalTableMappings = Command("ShowExternalTableMappings", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    rules.ExternalTableNameReference,
                    Token("mappings"),
                    shape310));

            var ShowExternalTableMapping = Command("ShowExternalTableMapping", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    rules.ExternalTableNameReference,
                    Token("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape218));

            var ShowExternalTablePrincipals = Command("ShowExternalTablePrincipals", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    rules.ExternalTableNameReference,
                    Token("principals"),
                    shape311));

            var ShowExternalTablesPrincipalRoles = Command("ShowExternalTablesPrincipalRoles", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    rules.ExternalTableNameReference,
                    Token("principal"),
                    Required(
                        First(
                            Token("roles"),
                            Custom(
                                rules.StringLiteral,
                                RequiredToken("roles"),
                                shape281)),
                        () => CreateMissingToken("roles")),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape312));

            var ShowExternalTableSchema = Command("ShowExternalTableSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    rules.ExternalTableNameReference,
                    Token("schema"),
                    RequiredToken("as"),
                    RequiredToken("csl", "json"),
                    shape313));

            var ShowExternalTable = Command("ShowExternalTable", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    RequiredToken("table"),
                    Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                    shape219));

            var ShowFabric = Command("ShowFabric", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("fabric"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    shape314));

            var ShowFollowerDatabase = Command("ShowFollowerDatabase", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("follower"),
                    Required(
                        First(
                            Custom(
                                Token("databases"),
                                Optional(
                                    Custom(
                                        Token("("),
                                        CommaList(
                                            Custom(
                                                If(Not(Token(")")), rules.DatabaseNameReference),
                                                shape8)),
                                        RequiredToken(")"),
                                        shape166)),
                                shape167),
                            Custom(
                                Token("database"),
                                Required(rules.DatabaseNameReference, rules.MissingNameReference),
                                shape221)),
                        () => (SyntaxElement)new CustomNode(shape167, CreateMissingToken("databases"), (SyntaxElement)new CustomNode(shape166, CreateMissingToken("("), SyntaxList<SeparatedElement<SyntaxElement>>.Empty(), CreateMissingToken(")"))))));

            var ShowFreshness = Command("ShowFreshness", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("freshness").Hide(),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Optional(
                        First(
                            Custom(
                                Token("column"),
                                Required(rules.ColumnNameReference, rules.MissingNameReference),
                                Optional(
                                    Custom(
                                        Token("threshold"),
                                        Required(rules.Value, rules.MissingValue),
                                        shape315)),
                                shape316),
                            Custom(
                                Token("threshold"),
                                Required(rules.Value, rules.MissingValue),
                                shape315))),
                    shape317));

            var ShowFunctions = Command("ShowFunctions", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("functions")));

            var ShowFunctionPrincipals = Command("ShowFunctionPrincipals", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("function"),
                    rules.DatabaseFunctionNameReference,
                    Token("principals"),
                    shape318));

            var ShowFunctionPrincipalRoles = Command("ShowFunctionPrincipalRoles", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("function"),
                    rules.DatabaseFunctionNameReference,
                    Token("principal"),
                    Required(
                        First(
                            Token("roles"),
                            Custom(
                                rules.StringLiteral,
                                RequiredToken("roles"),
                                shape281)),
                        () => CreateMissingToken("roles")),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape319));

            var ShowFunctionSchemaAsJson = Command("ShowFunctionSchemaAsJson", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("function"),
                    rules.DatabaseFunctionNameReference,
                    Token("schema"),
                    RequiredToken("as"),
                    RequiredToken("json"),
                    shape320));

            var ShowFunction = Command("ShowFunction", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("function"),
                    Required(rules.DatabaseFunctionNameReference, rules.MissingNameReference),
                    shape321));

            var ShowIngestionFailures = Command("ShowIngestionFailures", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("ingestion"),
                    Token("failures"),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            RequiredToken("OperationId"),
                            RequiredToken("="),
                            Required(rules.AnyGuidLiteralOrString, rules.MissingValue),
                            RequiredToken(")"),
                            shape322)),
                    shape294));

            var ShowIngestionMappings = Command("ShowIngestionMappings", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            Token("cluster"),
                            Token("ingestion")),
                        Custom(
                            Token("cluster"),
                            RequiredToken("ingestion")),
                        Token("ingestion")),
                    Required(
                        First(
                            Token("mappings"),
                            Custom(
                                Token("apacheavro", "avro", "csv", "json", "orc", "parquet", "sstream", "w3clogfile"),
                                RequiredToken("mappings"),
                                shape295)),
                        () => CreateMissingToken("mappings")),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape294));

            var ShowJournal = Command("ShowJournal", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("journal")));

            var ShowMaterializedViewsDetails = Command("ShowMaterializedViewsDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-views"),
                    First(
                        Custom(
                            Token("("),
                            OList(
                                primaryElementParser: Custom(
                                    rules.MaterializedViewNameReference,
                                    shape17),
                                separatorParser: Token(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingNameReference,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            Token(")"),
                            Token("details"),
                            shape323),
                        Custom(
                            Token("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.MaterializedViewNameReference,
                                        shape17),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: rules.MissingNameReference,
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameReference()))),
                            RequiredToken(")"),
                            RequiredToken("details"),
                            shape323),
                        Token("details"))));

            var ShowMaterializedViews = Command("ShowMaterializedViews", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-views")));

            var ShowMaterializedViewCslSchema = Command("ShowMaterializedViewCslSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("cslschema"),
                    shape170));

            var ShowMaterializedViewDetails = Command("ShowMaterializedViewDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("details"),
                    shape170));

            var ShowMaterializedViewDiagnostics = Command("ShowMaterializedViewDiagnostics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("diagnostics"),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape270));

            var ShowMaterializedViewExtents = Command("ShowMaterializedViewExtents", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("extents"),
                    Optional(
                        First(
                            Custom(
                                Token("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.AnyGuidLiteralOrString,
                                            shape0),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingValue,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingValue()))),
                                RequiredToken(")"),
                                Optional(Token("hot")),
                                shape213),
                            Token("hot"))),
                    shape324));

            var ShowMaterializedViewFailures = Command("ShowMaterializedViewFailures", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("failures"),
                    shape171));

            var ShowMaterializedViewPolicyCaching = Command("ShowMaterializedViewPolicyCaching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("policy"),
                    Token("caching"),
                    shape125));

            var ShowMaterializedViewPolicyMerge = Command("ShowMaterializedViewPolicyMerge", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("policy"),
                    Token("merge"),
                    shape125));

            var ShowMaterializedViewPolicyPartitioning = Command("ShowMaterializedViewPolicyPartitioning", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("policy"),
                    Token("partitioning"),
                    shape125));

            var ShowMaterializedViewPolicyRetention = Command("ShowMaterializedViewPolicyRetention", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("policy"),
                    Token("retention"),
                    shape125));

            var ShowMaterializedViewPolicyRowLevelSecurity = Command("ShowMaterializedViewPolicyRowLevelSecurity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("policy"),
                    RequiredToken("row_level_security"),
                    shape125));

            var ShowMaterializedViewPrincipals = Command("ShowMaterializedViewPrincipals", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("principals"),
                    shape170));

            var ShowMaterializedViewSchemaAsJson = Command("ShowMaterializedViewSchemaAsJson", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("schema"),
                    RequiredToken("as"),
                    RequiredToken("json"),
                    shape37));

            var ShowMaterializedViewStatistics = Command("ShowMaterializedViewStatistics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("statistics"),
                    shape171));

            var ShowMaterializedView = Command("ShowMaterializedView", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    shape226));

            var ShowMemory = Command("ShowMemory", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("memory"),
                    Optional(Token("details")),
                    shape184));

            var ShowOperations = Command("ShowOperations", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("operations"),
                    Optional(
                        First(
                            Custom(
                                Token("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.AnyGuidLiteralOrString,
                                            shape0),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingValue,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingValue()))),
                                RequiredToken(")"),
                                shape245),
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                shape0))),
                    shape184));

            var ShowOperationDetails = Command("ShowOperationDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("operation"),
                    Required(rules.AnyGuidLiteralOrString, rules.MissingValue),
                    RequiredToken("details"),
                    shape325));

            var ShowPlugins = Command("ShowPlugins", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("plugins"),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape184));

            var ShowPrincipalAccess = Command("ShowPrincipalAccess", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("principal"),
                    Token("access"),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape294));

            var ShowPrincipalRoles = Command("ShowPrincipalRoles", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("principal"),
                    Required(
                        First(
                            Token("roles"),
                            Custom(
                                rules.StringLiteral,
                                RequiredToken("roles"),
                                shape281)),
                        () => CreateMissingToken("roles")),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape294));

            var ShowQueries = Command("ShowQueries", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("queries")));

            var ShowQueryExecution = Command("ShowQueryExecution", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("queryexecution"),
                    Required(
                        Custom(
                            Token("<|"),
                            Required(rules.CommandInput, rules.MissingExpression),
                            shape84),
                        () => (SyntaxElement)new CustomNode(shape84, CreateMissingToken("<|"), rules.MissingExpression())),
                    shape326));

            var ShowQueryPlan = Command("ShowQueryPlan", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("queryplan"),
                    Required(
                        First(
                            Token("<|"),
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            First(
                                                Token("reconstructCsl"),
                                                If(Not(Token("reconstructCsl")), rules.NameDeclarationOrStringLiteral)),
                                            RequiredToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape48),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingToken("reconstructCsl"), CreateMissingToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingToken("reconstructCsl"), CreateMissingToken("="), rules.MissingValue())))),
                                RequiredToken(")"),
                                RequiredToken("<|"))),
                        () => CreateMissingToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    shape327));

            var ShowQueryCallTree = Command("ShowQueryCallTree", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("query"),
                    RequiredToken("call-tree"),
                    Required(
                        Custom(
                            Token("<|"),
                            Required(rules.CommandInput, rules.MissingExpression),
                            shape84),
                        () => (SyntaxElement)new CustomNode(shape84, CreateMissingToken("<|"), rules.MissingExpression())),
                    shape328));

            var ShowRequestSupport = Command("ShowRequestSupport", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("request_support"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape329));

            var ShowRowStores = Command("ShowRowStores", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("rowstores")));

            var ShowRowStoreSeals = Command("ShowRowStoreSeals", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("rowstore"),
                    Token("seals"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape330));

            var ShowRowStoreTransactions = Command("ShowRowStoreTransactions", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("rowstore"),
                    Token("transactions")));

            var ShowRowStore = Command("ShowRowStore", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("rowstore"),
                    Required(If(Not(And(Token("seals", "transactions"))), rules.NameDeclarationOrStringLiteral), rules.MissingNameDeclaration),
                    shape331));

            var ShowRunningQueries = Command("ShowRunningQueries", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("running"),
                    RequiredToken("queries"),
                    Optional(
                        Custom(
                            Token("by"),
                            Required(
                                First(
                                    Token("*"),
                                    Custom(
                                        Token("user"),
                                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                                        shape1)),
                                () => CreateMissingToken("*")))),
                    shape294));

            var ShowSchema = Command("ShowSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            Token("cluster"),
                            Token("schema")),
                        Token("schema")),
                    Optional(
                        First(
                            Custom(
                                Token("as"),
                                RequiredToken("json")),
                            Token("details"))),
                    shape184));

            var StoredQueryResultsShow = Command("StoredQueryResultsShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("stored_query_results"),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            CommaList(
                                Custom(
                                    If(Not(Token(")")), rules.NameDeclarationOrStringLiteral),
                                    RequiredToken("="),
                                    Required(rules.Value, rules.MissingValue),
                                    shape86)),
                            RequiredToken(")"),
                            shape87)),
                    shape184));

            var StoredQueryResultShowSchema = Command("StoredQueryResultShowSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("stored_query_result"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    RequiredToken("schema"),
                    shape332));

            var ShowStreamingIngestionFailures = Command("ShowStreamingIngestionFailures", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("streamingingestion"),
                    Token("failures")));

            var ShowStreamingIngestionStatistics = Command("ShowStreamingIngestionStatistics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("streamingingestion"),
                    RequiredToken("statistics")));

            var ShowTablesColumnStatistics = Command("ShowTablesColumnStatistics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("column"),
                    RequiredToken("statistics"),
                    RequiredToken("older"),
                    Required(rules.Value, rules.MissingValue),
                    shape333));

            var ShowTablesDetails = Command("ShowTablesDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("tables"),
                    First(
                        Custom(
                            Token("("),
                            OList(
                                primaryElementParser: Custom(
                                    rules.TableNameReference,
                                    shape19),
                                separatorParser: Token(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingNameReference,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            Token(")"),
                            Token("details"),
                            shape334),
                        Token("details"))));

            var ShowTables = Command("ShowTables", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Optional(
                        Custom(
                            Token("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.TableNameReference,
                                        shape19),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: rules.MissingNameReference,
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameReference()))),
                            RequiredToken(")"),
                            shape251)),
                    shape184));

            var ShowExtentCorruptedDatetime = Command("ShowExtentCorruptedDatetime", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    First(
                        Token("extents"),
                        Custom(
                            Token("table"),
                            If(Not(And(Token("*", "usage"))), rules.TableNameReference),
                            Token("extents"),
                            shape251)),
                    Required(
                        Custom(
                            Token("corrupted"),
                            RequiredToken("datetime")),
                        () => (SyntaxElement)new CustomNode(CreateMissingToken("corrupted"), CreateMissingToken("datetime"))).Hide()));

            var ShowTableExtentsMetadata = Command("ShowTableExtentsMetadata", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            Token("tables"),
                            Token("("),
                            OList(
                                primaryElementParser: Custom(
                                    rules.TableNameReference,
                                    shape19),
                                separatorParser: Token(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingNameReference,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            Token(")"),
                            shape335),
                        Custom(
                            Token("table"),
                            If(Not(And(Token("*", "usage"))), rules.TableNameReference),
                            shape157)),
                    Token("extents"),
                    Optional(
                        First(
                            Custom(
                                Token("("),
                                OList(
                                    primaryElementParser: Custom(
                                        rules.AnyGuidLiteralOrString,
                                        shape0),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: rules.MissingValue,
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                Token(")"),
                                Optional(Token("hot")),
                                shape213),
                            Token("hot"))),
                    Token("metadata"),
                    Optional(
                        First(
                            Custom(
                                Token("where"),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            Token("tags"),
                                            RequiredToken("!contains", "!has", "contains", "has"),
                                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                                            shape276),
                                        separatorParser: Token("and"),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape276, CreateMissingToken("tags"), CreateMissingToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape276, CreateMissingToken("tags"), CreateMissingToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral())))),
                                Optional(
                                    Custom(
                                        Token("with"),
                                        RequiredToken("("),
                                        RequiredToken("extentsShowFilteringRuntimePolicy"),
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        RequiredToken(")"),
                                        shape277)),
                                shape184),
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                RequiredToken("extentsShowFilteringRuntimePolicy"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredToken(")"),
                                shape277))),
                    shape278));

            var ShowTableExtents = Command("ShowTableExtents", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            Token("tables"),
                            Token("("),
                            OList(
                                primaryElementParser: Custom(
                                    rules.TableNameReference,
                                    shape19),
                                separatorParser: Token(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingNameReference,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            Token(")"),
                            shape335),
                        Custom(
                            Token("tables"),
                            Token("("),
                            OList(
                                primaryElementParser: Custom(
                                    rules.TableNameReference,
                                    shape19),
                                separatorParser: Token(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingNameReference,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            RequiredToken(")"),
                            shape335),
                        Custom(
                            Token("table"),
                            If(Not(And(Token("*", "usage"))), rules.TableNameReference),
                            shape157)),
                    RequiredToken("extents"),
                    Optional(
                        First(
                            Custom(
                                Token("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.AnyGuidLiteralOrString,
                                            shape0),
                                        separatorParser: Token(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: rules.MissingValue,
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingValue()))),
                                RequiredToken(")"),
                                Optional(Token("hot")),
                                shape213),
                            Token("hot"))),
                    Optional(
                        First(
                            Custom(
                                Token("where"),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            Token("tags"),
                                            RequiredToken("!contains", "!has", "contains", "has"),
                                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                                            shape276),
                                        separatorParser: Token("and"),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape276, CreateMissingToken("tags"), CreateMissingToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape276, CreateMissingToken("tags"), CreateMissingToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral())))),
                                Optional(
                                    Custom(
                                        Token("with"),
                                        RequiredToken("("),
                                        RequiredToken("extentsShowFilteringRuntimePolicy"),
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        RequiredToken(")"),
                                        shape277)),
                                shape184),
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                RequiredToken("extentsShowFilteringRuntimePolicy"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredToken(")"),
                                shape277))),
                    shape279));

            var TableShardGroupsStatisticsShow = Command("TableShardGroupsStatisticsShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    First(
                        Custom(
                            Token("tables"),
                            Token("("),
                            OList(
                                primaryElementParser: Custom(
                                    rules.TableNameReference,
                                    shape19),
                                separatorParser: Token(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: rules.MissingNameReference,
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            Token(")"),
                            shape335),
                        Custom(
                            Token("tables"),
                            Token("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.TableNameReference,
                                        shape19),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: rules.MissingNameReference,
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameReference()))),
                            RequiredToken(")"),
                            shape335),
                        Custom(
                            Token("table"),
                            If(Not(And(Token("*", "usage"))), rules.TableNameReference),
                            shape157)),
                    RequiredToken("shard-groups").Hide(),
                    RequiredToken("statistics").Hide()));

            var ShowTableStarPolicyCaching = Command("ShowTableStarPolicyCaching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    Token("policy"),
                    Token("caching")));

            var ShowTableStarPolicyExtentTagsRetention = Command("ShowTableStarPolicyExtentTagsRetention", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    Token("policy"),
                    Token("extent_tags_retention")));

            var ShowTableStarPolicyIngestionBatching = Command("ShowTableStarPolicyIngestionBatching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    Token("policy"),
                    Token("ingestionbatching")));

            var ShowTableStarPolicyIngestionTime = Command("ShowTableStarPolicyIngestionTime", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    Token("policy"),
                    Token("ingestiontime")));

            var ShowTableStarPolicyMerge = Command("ShowTableStarPolicyMerge", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    Token("policy"),
                    Token("merge")));

            var ShowTableStarPolicyPartitioning = Command("ShowTableStarPolicyPartitioning", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    Token("policy"),
                    Token("partitioning")));

            var ShowTableStarPolicyRestrictedViewAccess = Command("ShowTableStarPolicyRestrictedViewAccess", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    Token("policy"),
                    Token("restricted_view_access")));

            var ShowTableStarPolicyRetention = Command("ShowTableStarPolicyRetention", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    Token("policy"),
                    Token("retention")));

            var ShowTableStarPolicyRowLevelSecurity = Command("ShowTableStarPolicyRowLevelSecurity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    Token("policy"),
                    Token("row_level_security")));

            var ShowTableStarPolicyRowOrder = Command("ShowTableStarPolicyRowOrder", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    Token("policy"),
                    Token("roworder")));

            var ShowTableStarPolicySharding = Command("ShowTableStarPolicySharding", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    Token("policy"),
                    Token("sharding")));

            var ShowTableStarPolicyUpdate = Command("ShowTableStarPolicyUpdate", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    RequiredToken("policy"),
                    RequiredToken("update")));

            var ShowTableUsageStatisticsDetails = Command("ShowTableUsageStatisticsDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("usage"),
                    Token("statistics"),
                    Token("details")));

            var ShowTableUsageStatistics = Command("ShowTableUsageStatistics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("usage"),
                    RequiredToken("statistics"),
                    Optional(
                        Custom(
                            Token("by"),
                            Required(rules.Value, rules.MissingValue),
                            shape336)),
                    shape204));

            var ShowTablePolicyAutoDelete = Command("ShowTablePolicyAutoDelete", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("auto_delete"),
                    shape195));

            var ShowTablePolicyCaching = Command("ShowTablePolicyCaching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("caching"),
                    shape195));

            var ShowTablePolicyEncoding = Command("ShowTablePolicyEncoding", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("encoding"),
                    shape195));

            var ShowTablePolicyExtentTagsRetention = Command("ShowTablePolicyExtentTagsRetention", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("extent_tags_retention"),
                    shape195));

            var ShowTablePolicyIngestionBatching = Command("ShowTablePolicyIngestionBatching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("ingestionbatching"),
                    shape195));

            var ShowTablePolicyMerge = Command("ShowTablePolicyMerge", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("merge"),
                    shape195));

            var ShowTablePolicyPartitioning = Command("ShowTablePolicyPartitioning", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("partitioning"),
                    shape195));

            var ShowTablePolicyRestrictedViewAccess = Command("ShowTablePolicyRestrictedViewAccess", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("restricted_view_access"),
                    shape195));

            var ShowTablePolicyRetention = Command("ShowTablePolicyRetention", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("retention"),
                    shape195));

            var ShowTablePolicyRowOrder = Command("ShowTablePolicyRowOrder", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("roworder"),
                    shape195));

            var ShowTablePolicySharding = Command("ShowTablePolicySharding", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("sharding"),
                    shape195));

            var ShowTablePolicyStreamingIngestion = Command("ShowTablePolicyStreamingIngestion", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("streamingingestion"),
                    shape195));

            var ShowTablePolicyUpdate = Command("ShowTablePolicyUpdate", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.DatabaseTableNameReference),
                    Token("policy"),
                    RequiredToken("update"),
                    shape195));

            var ShowTableRowStoreReferences = Command("ShowTableRowStoreReferences", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.DatabaseTableNameReference),
                    Token("rowstore_references"),
                    shape172));

            var ShowTableRowStoreSealInfo = Command("ShowTableRowStoreSealInfo", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.DatabaseTableNameReference),
                    Token("rowstore_sealinfo"),
                    shape337));

            var ShowTableRowStores = Command("ShowTableRowStores", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.DatabaseTableNameReference),
                    RequiredToken("rowstores"),
                    shape337));

            var ShowTableColumnsClassification = Command("ShowTableColumnsClassification", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.TableNameReference),
                    Token("columns"),
                    RequiredToken("classification"),
                    shape195));

            var ShowTableColumnStatitics = Command("ShowTableColumnStatitics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.TableNameReference),
                    Token("column"),
                    RequiredToken("statistics"),
                    shape195));

            var ShowTableCslSchema = Command("ShowTableCslSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.TableNameReference),
                    Token("cslschema"),
                    shape172));

            var ShowTableDetails = Command("ShowTableDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.TableNameReference),
                    Token("details"),
                    shape172));

            var ShowTableDimensions = Command("ShowTableDimensions", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.TableNameReference),
                    Token("dimensions"),
                    shape172));

            var ShowTableIngestionMappings = Command("ShowTableIngestionMappings", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.TableNameReference),
                    Token("ingestion"),
                    Token("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    Token("mappings"),
                    shape338));

            var ShowTableIngestionMapping = Command("ShowTableIngestionMapping", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.TableNameReference),
                    Token("ingestion"),
                    RequiredToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape232));

            var ShowTablePolicyIngestionTime = Command("ShowTablePolicyIngestionTime", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.TableNameReference),
                    Token("policy"),
                    Token("ingestiontime"),
                    shape195));

            var ShowTablePolicyRowLevelSecurity = Command("ShowTablePolicyRowLevelSecurity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.TableNameReference),
                    Token("policy"),
                    RequiredToken("row_level_security"),
                    shape195));

            var ShowTablePrincipals = Command("ShowTablePrincipals", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.TableNameReference),
                    Token("principals"),
                    shape172));

            var ShowTablePrincipalRoles = Command("ShowTablePrincipalRoles", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.TableNameReference),
                    Token("principal"),
                    Required(
                        First(
                            Token("roles"),
                            Custom(
                                rules.StringLiteral,
                                RequiredToken("roles"),
                                shape281)),
                        () => CreateMissingToken("roles")),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.NameDeclarationOrStringLiteral,
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape63),
                                    separatorParser: Token(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingToken("="), rules.MissingValue())))),
                            RequiredToken(")"),
                            shape87)),
                    shape339));

            var ShowTableSchemaAsJson = Command("ShowTableSchemaAsJson", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.TableNameReference),
                    Token("schema"),
                    RequiredToken("as"),
                    RequiredToken("json"),
                    shape40));

            var TableShardGroupsShow = Command("TableShardGroupsShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(And(Token("*", "usage"))), rules.TableNameReference),
                    Token("shard-groups").Hide(),
                    shape172));

            var ShowTable = Command("ShowTable", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Required(If(Not(And(Token("*", "usage"))), rules.TableNameReference), rules.MissingNameReference),
                    shape340));

            var ShowVersion = Command("ShowVersion", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("version")));

            var ShowWorkloadGroups = Command("ShowWorkloadGroups", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("workload_groups")));

            var ShowWorkloadGroup = Command("ShowWorkloadGroup", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    RequiredToken("workload_group"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    shape341));

            var UndoDropTable = Command("UndoDropTable", 
                Custom(
                    Token("undo", CompletionKind.CommandPrefix),
                    RequiredToken("drop"),
                    RequiredToken("table"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Required(
                        First(
                            Custom(
                                Token("as"),
                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                RequiredToken("version"),
                                shape342),
                            Token("version")),
                        () => (SyntaxElement)new CustomNode(shape342, CreateMissingToken("as"), rules.MissingNameDeclaration(), CreateMissingToken("version"))),
                    RequiredToken("="),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(Token("internal")),
                    shape343));

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
                PatchExtentCorruptedDatetime,
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
                ShowExtentCorruptedDatetime,
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

