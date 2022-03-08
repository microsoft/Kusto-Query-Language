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
            var shape251 = new [] {CD("NewColumnName", CompletionHint.None), CD(), CD("ColumnName", CompletionHint.Column)};
            var shape252 = new [] {CD(), CD(), CD(CompletionHint.None)};
            var shape253 = new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD("NewColumnName", CompletionHint.None)};
            var shape254 = new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD("NewMaterializedViewName", CompletionHint.None)};
            var shape255 = new [] {CD("NewTableName", CompletionHint.None), CD(), CD("TableName", CompletionHint.Table)};
            var shape256 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("NewTableName", CompletionHint.None)};
            var shape257 = new [] {CD(), CD(), CD(), CD(), CD("DestinationTableName", CompletionHint.Table), CD(), CD(), CD("ExtentsToDropQuery", CompletionHint.Tabular), CD(), CD(), CD(), CD("ExtentsToMoveQuery", CompletionHint.Tabular), CD()};
            var shape258 = new [] {CD(), CD("TableName", CompletionHint.None)};
            var shape259 = new [] {CD(), CD(), CD("StoredQueryResultName", CompletionHint.None), CD(), CD("Query", CompletionHint.Tabular)};
            var shape260 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD("AccessMode")};
            var shape261 = new [] {CD(), CD(), CD("Role"), CD()};
            var shape262 = new [] {CD(), CD(), CD("jobName", CompletionHint.None), CD(), CD(), CD("cursorValue", CompletionHint.Literal)};
            var shape263 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD("Role"), CD()};
            var shape264 = new [] {CD(), CD(), CD(), CD("externalTableName", CompletionHint.ExternalTable), CD(), CD()};
            var shape265 = new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD("Role"), CD()};
            var shape266 = new [] {CD(), CD(CompletionHint.Literal), CD(), CD(CompletionHint.Literal, isOptional: true)};
            var shape267 = new [] {CD(), CD(), CD("materializedViewName", CompletionHint.MaterializedView), CD(), CD()};
            var shape268 = new [] {CD(), CD("n", CompletionHint.Literal)};
            var shape269 = new [] {CD(), CD(), CD("viewName", CompletionHint.MaterializedView), CD(), CD(isOptional: true)};
            var shape270 = new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD("CursorValue", CompletionHint.Literal)};
            var shape271 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("references", CompletionHint.Literal)};
            var shape272 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD("Role"), CD()};
            var shape273 = new [] {CD(), CD("duration", CompletionHint.Literal), CD(isOptional: true)};
            var shape274 = new [] {CD(), CD(), CD(), CD(), CD("Scope"), CD()};
            var shape275 = new [] {CD(), CD(), CD("Tag", CompletionHint.Literal)};
            var shape276 = new [] {CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()};
            var shape277 = new [] {CD(), CD(), CD(), CD(isOptional: true), CD(), CD(isOptional: true)};
            var shape278 = new [] {CD(), CD(), CD(), CD(isOptional: true), CD(isOptional: true)};
            var shape279 = new [] {CD(), CD(), CD(), CD(CompletionHint.Literal, isOptional: true)};
            var shape280 = new [] {CD("Principal", CompletionHint.Literal), CD()};
            var shape281 = new [] {CD(), CD(), CD(), CD("num", CompletionHint.Literal), CD()};
            var shape282 = new [] {CD(), CD(), CD("ColumnName"), CD(), CD()};
            var shape283 = new [] {CD(), CD(), CD("ContinuousExportName", CompletionHint.None), CD()};
            var shape284 = new [] {CD(), CD("Version", CompletionHint.Literal)};
            var shape285 = new [] {CD("DatabaseName", CompletionHint.Database), CD(isOptional: true)};
            var shape286 = new [] {CD(), CD(), CD(), CD(CompletionHint.Database), CD(), CD(), CD(), CD()};
            var shape287 = new [] {CD(), CD(), CD(), CD(CompletionHint.Database), CD(), CD(), CD(isOptional: true)};
            var shape288 = new [] {CD(), CD(CompletionHint.Database, isOptional: true)};
            var shape289 = new [] {CD(), CD(), CD(), CD(), CD("minCreationTime", CompletionHint.Literal), CD()};
            var shape290 = new [] {CD(), CD(), CD(), CD(), CD(), CD(isOptional: true)};
            var shape291 = new [] {CD("databaseName", CompletionHint.Database), CD()};
            var shape292 = new [] {CD(), CD("databaseVersion", CompletionHint.Literal)};
            var shape293 = new [] {CD(), CD(), CD(), CD(isOptional: true)};
            var shape294 = new [] {CD("kind"), CD()};
            var shape295 = new [] {CD("name", CompletionHint.Literal), CD(isOptional: true)};
            var shape296 = new [] {CD("DatabaseName", CompletionHint.Database), CD()};
            var shape297 = new [] {CD(), CD("Version", CompletionHint.Literal), CD()};
            var shape298 = new [] {CD(), CD(), CD(), CD(), CD(), CD(), CD(isOptional: true)};
            var shape299 = new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(), CD(), CD(CompletionHint.Literal, isOptional: true)};
            var shape300 = new [] {CD(), CD(), CD("DatabaseName"), CD(), CD()};
            var shape301 = new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(), CD()};
            var shape302 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD(isOptional: true)};
            var shape303 = new [] {CD(), CD("obj", CompletionHint.Literal)};
            var shape304 = new [] {CD(), CD("excludedFunctions", CompletionHint.Literal)};
            var shape305 = new [] {CD(), CD(), CD("entity", CompletionHint.None), CD(), CD(), CD(), CD(isOptional: true), CD(isOptional: true)};
            var shape306 = new [] {CD(), CD(), CD("extentId", CompletionHint.Literal), CD(), CD("columnName", CompletionHint.None), CD(), CD()};
            var shape307 = new [] {CD(), CD(CompletionHint.Literal)};
            var shape308 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD(isOptional: true)};
            var shape309 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD()};
            var shape310 = new [] {CD(), CD(), CD(), CD("tableName", CompletionHint.ExternalTable), CD()};
            var shape311 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD(), CD(isOptional: true)};
            var shape312 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD(), CD()};
            var shape313 = new [] {CD(), CD(), CD("id", CompletionHint.None)};
            var shape314 = new [] {CD(), CD("threshold", CompletionHint.Literal)};
            var shape315 = new [] {CD(), CD("columnName", CompletionHint.Column), CD(isOptional: true)};
            var shape316 = new [] {CD(), CD(), CD("tableName", CompletionHint.Table), CD(isOptional: true)};
            var shape317 = new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD()};
            var shape318 = new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD(), CD(), CD(isOptional: true)};
            var shape319 = new [] {CD(), CD(), CD("functionName", CompletionHint.Function), CD(), CD(), CD()};
            var shape320 = new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function)};
            var shape321 = new [] {CD(), CD(), CD(), CD(), CD("OperationId", CompletionHint.Literal), CD()};
            var shape322 = new [] {CD(), CD(CompletionHint.MaterializedView), CD(), CD()};
            var shape323 = new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(isOptional: true)};
            var shape324 = new [] {CD(), CD(), CD("OperationId", CompletionHint.Literal), CD()};
            var shape325 = new [] {CD(), CD(), CD("queryText")};
            var shape326 = new [] {CD(), CD(), CD(), CD("Query", CompletionHint.Tabular)};
            var shape327 = new [] {CD(), CD(), CD(), CD("queryText")};
            var shape328 = new [] {CD(), CD(), CD("key", CompletionHint.Literal)};
            var shape329 = new [] {CD(), CD(), CD(), CD("tableName", CompletionHint.Literal), CD(isOptional: true)};
            var shape330 = new [] {CD(), CD(), CD("rowStoreName", CompletionHint.None)};
            var shape331 = new [] {CD(), CD(), CD("StoredQueryResultName", CompletionHint.None), CD()};
            var shape332 = new [] {CD(), CD(), CD(), CD(), CD(), CD("outdatewindow", CompletionHint.Literal)};
            var shape333 = new [] {CD(), CD(CompletionHint.Table), CD(), CD()};
            var shape334 = new [] {CD(), CD(CompletionHint.Table), CD()};
            var shape335 = new [] {CD(), CD(), CD(CompletionHint.Table), CD()};
            var shape336 = new [] {CD(), CD("partitionBy", CompletionHint.Literal)};
            var shape337 = new [] {CD(), CD(), CD("tableName", CompletionHint.Table), CD()};
            var shape338 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("MappingKind"), CD()};
            var shape339 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(isOptional: true)};
            var shape340 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table)};
            var shape341 = new [] {CD(), CD(), CD("WorkloadGroup", CompletionHint.None)};
            var shape342 = new [] {CD(), CD("TableName", CompletionHint.None), CD()};
            var shape343 = new [] {CD(), CD(), CD(), CD(CompletionHint.None), CD(), CD(), CD("Version", CompletionHint.Literal)};

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
                                        shape1)),
                                shape2),
                            Custom(
                                EToken("user"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                shape1))),
                    Optional(
                        First(
                            Custom(
                                EToken("period"),
                                Required(rules.Value, rules.MissingValue),
                                Optional(
                                    Custom(
                                        EToken("reason"),
                                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                                        shape3)),
                                shape4),
                            Custom(
                                EToken("reason"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                shape3))),
                    shape5));

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
                                shape0),
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
                                        shape0)),
                                shape6),
                            Custom(
                                rules.StringLiteral,
                                shape0))),
                    shape7));

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
                                shape0),
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
                                        shape0)),
                                shape6),
                            Custom(
                                rules.StringLiteral,
                                shape0))),
                    shape9));

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
                                shape0),
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
                                        shape0)),
                                shape11),
                            Custom(
                                rules.StringLiteral,
                                shape0))),
                    shape12));

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
                                shape13),
                            Custom(
                                EToken("admins", "monitors", "unrestrictedviewers", "users", "viewers"))),
                        () => (SyntaxElement)new CustomNode(shape13, CreateMissingEToken("from"), rules.MissingStringLiteral(), CreateMissingEToken("Expected admins,monitors,unrestrictedviewers,users,viewers"))),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                shape0),
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
                            shape0)),
                    shape14));

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
                                shape0),
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
                                        shape0)),
                                shape6),
                            Custom(
                                rules.StringLiteral,
                                shape0))),
                    shape16));

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
                                shape0),
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
                            shape0)),
                    shape18));

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
                                shape0),
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
                                        shape0)),
                                shape6),
                            Custom(
                                rules.StringLiteral,
                                shape0))),
                    shape20));

            var AlterMergeClusterPolicyCallout = Command("AlterMergeClusterPolicyCallout", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("callout"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape21));

            var AlterMergeClusterPolicyCapacity = Command("AlterMergeClusterPolicyCapacity", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("capacity"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape21));

            var AlterMergeClusterPolicyDiagnostics = Command("AlterMergeClusterPolicyDiagnostics", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("diagnostics"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape22));

            var AlterMergeClusterPolicyMultiDatabaseAdmins = Command("AlterMergeClusterPolicyMultiDatabaseAdmins", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("multidatabaseadmins"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape21));

            var AlterMergeClusterPolicyQueryWeakConsistency = Command("AlterMergeClusterPolicyQueryWeakConsistency", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("query_weak_consistency"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape21));

            var AlterMergeClusterPolicyRequestClassification = Command("AlterMergeClusterPolicyRequestClassification", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("request_classification"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape21));

            var AlterMergeClusterPolicySharding = Command("AlterMergeClusterPolicySharding", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("sharding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape23));

            var AlterMergeClusterPolicyStreamingIngestion = Command("AlterMergeClusterPolicyStreamingIngestion", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("streamingingestion"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape23));

            var AlterMergeClusterPolicyRowStore = Command("AlterMergeClusterPolicyRowStore", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    RequiredEToken("policy"),
                    RequiredEToken("rowstore"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape24));

            var AlterMergeColumnPolicyEncoding = Command("AlterMergeColumnPolicyEncoding", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("column"),
                    Required(rules.TableColumnNameReference, rules.MissingNameReference),
                    RequiredEToken("policy"),
                    RequiredEToken("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape26));

            var AlterMergeDatabasePolicyDiagnostics = Command("AlterMergeDatabasePolicyDiagnostics", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("diagnostics"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape27));

            var AlterMergeDatabasePolicyEncoding = Command("AlterMergeDatabasePolicyEncoding", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape28));

            var AlterMergeDatabasePolicyMerge = Command("AlterMergeDatabasePolicyMerge", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("merge"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape29));

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
                                shape30),
                            Custom(
                                EToken("softdelete"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                Optional(
                                    Custom(
                                        EToken("recoverability"),
                                        RequiredEToken("="),
                                        RequiredEToken("disabled", "enabled"),
                                        shape30)),
                                shape31),
                            Custom(
                                rules.StringLiteral,
                                shape0)),
                        () => (SyntaxElement)new CustomNode(shape30, CreateMissingEToken("recoverability"), CreateMissingEToken("="), CreateMissingEToken("Expected disabled,enabled"))),
                    shape32));

            var AlterMergeDatabasePolicySharding = Command("AlterMergeDatabasePolicySharding", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("sharding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape33));

            var AlterMergeDatabasePolicyShardsGrouping = Command("AlterMergeDatabasePolicyShardsGrouping", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("shards_grouping").Hide(),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape34));

            var AlterMergeDatabasePolicyStreamingIngestion = Command("AlterMergeDatabasePolicyStreamingIngestion", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredEToken("policy"),
                    RequiredEToken("streamingingestion"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape35));

            var AlterMergeMaterializedViewPolicyPartitioning = Command("AlterMergeMaterializedViewPolicyPartitioning", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("policy"),
                    EToken("partitioning"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape36));

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
                                shape30),
                            Custom(
                                EToken("softdelete"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                Optional(
                                    Custom(
                                        EToken("recoverability"),
                                        RequiredEToken("="),
                                        RequiredEToken("disabled", "enabled"),
                                        shape30)),
                                shape31),
                            Custom(
                                rules.StringLiteral,
                                shape0)),
                        () => (SyntaxElement)new CustomNode(shape30, CreateMissingEToken("recoverability"), CreateMissingEToken("="), CreateMissingEToken("Expected disabled,enabled"))),
                    shape37));

            var AlterMergeTablePolicyEncoding = Command("AlterMergeTablePolicyEncoding", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape38));

            var AlterMergeTablePolicyMerge = Command("AlterMergeTablePolicyMerge", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("merge"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape39));

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
                                shape30),
                            Custom(
                                EToken("softdelete"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                Optional(
                                    Custom(
                                        EToken("recoverability"),
                                        RequiredEToken("="),
                                        RequiredEToken("disabled", "enabled"),
                                        shape30)),
                                shape31),
                            Custom(
                                rules.StringLiteral,
                                shape0)),
                        () => (SyntaxElement)new CustomNode(shape30, CreateMissingEToken("recoverability"), CreateMissingEToken("="), CreateMissingEToken("Expected disabled,enabled"))),
                    shape40));

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
                                shape41),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape41, rules.MissingNameReference(), CreateMissingEToken("Expected asc,desc")),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape41, rules.MissingNameReference(), CreateMissingEToken("Expected asc,desc"))))),
                    RequiredEToken(")"),
                    shape42));

            var AlterMergeTablePolicySharding = Command("AlterMergeTablePolicySharding", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("sharding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape43));

            var AlterMergeTablePolicyStreamingIngestion = Command("AlterMergeTablePolicyStreamingIngestion", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("streamingingestion"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape44));

            var AlterMergeTablePolicyUpdate = Command("AlterMergeTablePolicyUpdate", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    RequiredEToken("policy"),
                    RequiredEToken("update"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape45));

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
                                shape47),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType())))),
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
                                        shape48),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingEToken("docstring"), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingEToken("docstring"), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"))),
                    shape49));

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
                                shape50),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape50, rules.MissingNameReference(), CreateMissingEToken(":"), rules.MissingStringLiteral()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape50, rules.MissingNameReference(), CreateMissingEToken(":"), rules.MissingStringLiteral())))),
                    RequiredEToken(")"),
                    shape51));

            var AlterMergeTablePolicyPartitioning = Command("AlterMergeTablePolicyPartitioning", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    EToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredEToken("policy"),
                    RequiredEToken("partitioning"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape52));

            var AlterMergeWorkloadGroup = Command("AlterMergeWorkloadGroup", 
                Custom(
                    EToken("alter-merge", CompletionKind.CommandPrefix),
                    RequiredEToken("workload_group"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape53));

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
                                shape0)),
                        () => CreateMissingEToken("*")),
                    Required(rules.BracketedStringLiteral, rules.MissingStringLiteral),
                    shape54));

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
                                shape55),
                            Custom(
                                EToken("hot"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape56)),
                        () => (SyntaxElement)new CustomNode(shape55, CreateMissingEToken("hotdata"), CreateMissingEToken("="), rules.MissingValue(), CreateMissingEToken("hotindex"), CreateMissingEToken("="), rules.MissingValue()))));

            var AlterClusterPolicyCallout = Command("AlterClusterPolicyCallout", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("callout"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape21));

            var AlterClusterPolicyCapacity = Command("AlterClusterPolicyCapacity", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("capacity"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape21));

            var AlterClusterPolicyDiagnostics = Command("AlterClusterPolicyDiagnostics", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("diagnostics"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape22));

            var AlterClusterPolicyIngestionBatching = Command("AlterClusterPolicyIngestionBatching", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("ingestionbatching"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape57));

            var AlterClusterPolicyManagedIdentity = Command("AlterClusterPolicyManagedIdentity", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("managed_identity"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape58));

            var AlterClusterPolicyMultiDatabaseAdmins = Command("AlterClusterPolicyMultiDatabaseAdmins", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("multidatabaseadmins"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape21));

            var AlterClusterPolicyQueryWeakConsistency = Command("AlterClusterPolicyQueryWeakConsistency", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("query_weak_consistency"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape21));

            var AlterClusterPolicyRequestClassification = Command("AlterClusterPolicyRequestClassification", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("request_classification"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    RequiredEToken("<|"),
                    Required(rules.CommandInput, rules.MissingExpression),
                    shape60));

            var AlterClusterPolicyRowStore = Command("AlterClusterPolicyRowStore", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("rowstore"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape24));

            var AlterClusterPolicySandbox = Command("AlterClusterPolicySandbox", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("sandbox"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape61));

            var AlterClusterPolicySharding = Command("AlterClusterPolicySharding", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    EToken("sharding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape23));

            var AlterClusterPolicyStreamingIngestion = Command("AlterClusterPolicyStreamingIngestion", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("cluster"),
                    EToken("policy"),
                    RequiredEToken("streamingingestion"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape62));

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
                                            shape63),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("decryption-certificate-thumbprint"),
                                shape64)),
                        () => CreateMissingEToken("decryption-certificate-thumbprint")),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape65));

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
                                shape55),
                            Custom(
                                EToken("hot"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape56)),
                        () => (SyntaxElement)new CustomNode(shape55, CreateMissingEToken("hotdata"), CreateMissingEToken("="), rules.MissingValue(), CreateMissingEToken("hotindex"), CreateMissingEToken("="), rules.MissingValue())),
                    shape66));

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
                    shape67));

            var AlterColumnPolicyEncoding = Command("AlterColumnPolicyEncoding", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("column"),
                    rules.DatabaseTableColumnNameReference,
                    EToken("policy"),
                    RequiredEToken("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape26));

            var AlterColumnType = Command("AlterColumnType", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("column"),
                    Required(rules.DatabaseTableColumnNameReference, rules.MissingNameReference),
                    RequiredEToken("type"),
                    RequiredEToken("="),
                    Required(rules.Type, rules.MissingType),
                    shape68));

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
                    shape69));

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
                                shape70),
                            Custom(
                                rules.StringLiteral,
                                shape0)),
                        () => (SyntaxElement)new CustomNode(shape70, rules.MissingStringLiteral(), CreateMissingEToken(";"), rules.MissingStringLiteral())),
                    shape71));

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
                                shape55),
                            Custom(
                                EToken("hot"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape56)),
                        () => (SyntaxElement)new CustomNode(shape55, CreateMissingEToken("hotdata"), CreateMissingEToken("="), rules.MissingValue(), CreateMissingEToken("hotindex"), CreateMissingEToken("="), rules.MissingValue())),
                    shape32));

            var AlterDatabasePolicyDiagnostics = Command("AlterDatabasePolicyDiagnostics", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("diagnostics"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape27));

            var AlterDatabasePolicyEncoding = Command("AlterDatabasePolicyEncoding", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape28));

            var AlterDatabasePolicyExtentTagsRetention = Command("AlterDatabasePolicyExtentTagsRetention", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("extent_tags_retention"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape72));

            var AlterDatabasePolicyIngestionBatching = Command("AlterDatabasePolicyIngestionBatching", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("ingestionbatching"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape73));

            var AlterDatabasePolicyManagedIdentity = Command("AlterDatabasePolicyManagedIdentity", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("managed_identity"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape74));

            var AlterDatabasePolicyMerge = Command("AlterDatabasePolicyMerge", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("merge"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape29));

            var AlterDatabasePolicyRetention = Command("AlterDatabasePolicyRetention", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("retention"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape75));

            var AlterDatabasePolicySharding = Command("AlterDatabasePolicySharding", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("sharding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape33));

            var AlterDatabasePolicyShardsGrouping = Command("AlterDatabasePolicyShardsGrouping", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("shards_grouping").Hide(),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape34));

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
                                EToken("disable", "enable")),
                            Custom(
                                rules.StringLiteral,
                                shape0)),
                        () => CreateMissingEToken("Expected disable,enable")),
                    shape32));

            var AlterDatabasePrettyName = Command("AlterDatabasePrettyName", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("prettyname"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape76));

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
                                            shape63),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("decryption-certificate-thumbprint"),
                                shape64)),
                        () => CreateMissingEToken("decryption-certificate-thumbprint")),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape77));

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
                            shape78)),
                    shape79));

            var AlterExtentContainersDrop = Command("AlterExtentContainersDrop", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("extentcontainers"),
                    rules.DatabaseNameReference,
                    EToken("drop"),
                    Optional(
                        Custom(
                            rules.AnyGuidLiteralOrString,
                            shape0)),
                    shape80));

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
                                shape81),
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                shape0)),
                        () => (SyntaxElement)new CustomNode(shape81, CreateMissingEToken("older"), rules.MissingValue(), CreateMissingEToken("hours"))),
                    shape82));

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
                    shape83));

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
                                shape0),
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
                            shape84),
                        () => (SyntaxElement)new CustomNode(shape84, CreateMissingEToken("<|"), rules.MissingExpression())),
                    shape85));

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
                                shape47),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()),
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
                                            shape86),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                shape87))}
                    ,
                    shape88));

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
                                    shape47),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()),
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType())))),
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
                                                                                shape89),
                                                                            Custom(
                                                                                EToken("startofday", "startofmonth", "startofweek", "startofyear"),
                                                                                RequiredEToken("("),
                                                                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                                RequiredEToken(")"),
                                                                                shape90)),
                                                                        () => (SyntaxElement)new CustomNode(shape89, CreateMissingEToken("bin"), CreateMissingEToken("("), rules.MissingNameDeclaration(), CreateMissingEToken(","), rules.MissingValue(), CreateMissingEToken(")"))))),
                                                            shape91),
                                                        Custom(
                                                            EToken("long"),
                                                            RequiredEToken("="),
                                                            RequiredEToken("hash"),
                                                            RequiredEToken("("),
                                                            Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                            RequiredEToken(","),
                                                            Required(rules.Value, rules.MissingValue),
                                                            RequiredEToken(")"),
                                                            shape92),
                                                        Custom(
                                                            EToken("string"),
                                                            Optional(
                                                                Custom(
                                                                    EToken("="),
                                                                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                    shape93)),
                                                            shape91)),
                                                    () => (SyntaxElement)new CustomNode(shape91, CreateMissingEToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingEToken("="), (SyntaxElement)new CustomNode(shape89, CreateMissingEToken("bin"), CreateMissingEToken("("), rules.MissingNameDeclaration(), CreateMissingEToken(","), rules.MissingValue(), CreateMissingEToken(")"))))),
                                                shape94),
                                            separatorParser: EToken(","),
                                            secondaryElementParser: null,
                                            missingPrimaryElement: null,
                                            missingSeparator: null,
                                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape94, rules.MissingNameDeclaration(), CreateMissingEToken(":"), (SyntaxElement)new CustomNode(shape91, CreateMissingEToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingEToken("="), (SyntaxElement)new CustomNode(shape89, CreateMissingEToken("bin"), CreateMissingEToken("("), rules.MissingNameDeclaration(), CreateMissingEToken(","), rules.MissingValue(), CreateMissingEToken(")"))))),
                                            endOfList: null,
                                            oneOrMore: true,
                                            allowTrailingSeparator: false,
                                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape94, rules.MissingNameDeclaration(), CreateMissingEToken(":"), (SyntaxElement)new CustomNode(shape91, CreateMissingEToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingEToken("="), (SyntaxElement)new CustomNode(shape89, CreateMissingEToken("bin"), CreateMissingEToken("("), rules.MissingNameDeclaration(), CreateMissingEToken(","), rules.MissingValue(), CreateMissingEToken(")")))))))),
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
                                                                            shape95),
                                                                        Custom(
                                                                            If(Not(EToken("datetime_pattern")), rules.NameDeclarationOrStringLiteral),
                                                                            shape46)),
                                                                    Optional(
                                                                        Custom(
                                                                            rules.StringLiteral,
                                                                            shape0)),
                                                                    shape11),
                                                                missingElement: null,
                                                                oneOrMore: true,
                                                                producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray())),
                                                            () => new SyntaxList<SyntaxElement>((SyntaxElement)new CustomNode(shape11, (SyntaxElement)new CustomNode(shape95, CreateMissingEToken("datetime_pattern"), CreateMissingEToken("("), rules.MissingStringLiteral(), CreateMissingEToken(","), rules.MissingNameDeclaration(), CreateMissingEToken(")")), rules.MissingStringLiteral()))),
                                                        shape96),
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
                                                                    shape95),
                                                                Custom(
                                                                    EToken("datetime_pattern"),
                                                                    RequiredEToken("("),
                                                                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                                                                    RequiredEToken(","),
                                                                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                    RequiredEToken(")"),
                                                                    shape95),
                                                                Custom(
                                                                    If(Not(EToken("datetime_pattern")), rules.NameDeclarationOrStringLiteral),
                                                                    shape46)),
                                                            Optional(
                                                                Custom(
                                                                    rules.StringLiteral,
                                                                    shape0)),
                                                            shape11),
                                                        missingElement: null,
                                                        oneOrMore: true,
                                                        producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray()))),
                                                () => (SyntaxElement)new CustomNode(shape96, rules.MissingStringLiteral(), new SyntaxList<SyntaxElement>((SyntaxElement)new CustomNode(shape11, (SyntaxElement)new CustomNode(shape95, CreateMissingEToken("datetime_pattern"), CreateMissingEToken("("), rules.MissingStringLiteral(), CreateMissingEToken(","), rules.MissingNameDeclaration(), CreateMissingEToken(")")), rules.MissingStringLiteral())))),
                                            RequiredEToken(")"),
                                            shape97)),
                                    RequiredEToken("dataformat"),
                                    shape98)),
                            () => CreateMissingEToken("dataformat")),
                        RequiredEToken("="),
                        RequiredEToken("apacheavro", "avro", "csv", "json", "multijson", "orc", "parquet", "psv", "raw", "scsv", "sohsv", "sstream", "tsve", "tsv", "txt", "w3clogfile"),
                        RequiredEToken("("),
                        Required(
                            OList(
                                primaryElementParser: Custom(
                                    rules.StringLiteral,
                                    shape0),
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
                                            shape86),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                shape87))}
                    ,
                    shape99));

            var AlterExternalTableDocString = Command("AlterExternalTableDocString", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("external"),
                    EToken("table"),
                    rules.ExternalTableNameReference,
                    EToken("docstring"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape100));

            var AlterExternalTableFolder = Command("AlterExternalTableFolder", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("external"),
                    EToken("table"),
                    rules.ExternalTableNameReference,
                    EToken("folder"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape101));

            var AlterExternalTableMapping = Command("AlterExternalTableMapping", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("external"),
                    RequiredEToken("table"),
                    Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                    RequiredEToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape102));

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
                                shape103),
                            Custom(
                                EToken("default-caching-policies-modification-kind"),
                                RequiredEToken("="),
                                RequiredEToken("none", "replace", "union"),
                                shape104),
                            Custom(
                                EToken("default-principals-modification-kind"),
                                RequiredEToken("="),
                                RequiredEToken("none", "replace", "union"),
                                shape104),
                            Custom(
                                EToken("follow-authorized-principals"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape105)),
                        () => (SyntaxElement)new CustomNode(shape103, CreateMissingEToken("database-name-prefix"), CreateMissingEToken("="), rules.MissingNameDeclaration())),
                    shape106));

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
                            shape107),
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
                                shape108),
                            Custom(
                                EToken("hot"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape109)),
                        () => (SyntaxElement)new CustomNode(shape108, CreateMissingEToken("hotdata"), CreateMissingEToken("="), rules.MissingValue(), CreateMissingEToken("hotindex"), CreateMissingEToken("="), rules.MissingValue())),
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
                                                    shape110),
                                                () => (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())),
                                            shape111),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape111, CreateMissingEToken("hot_window"), CreateMissingEToken("="), (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape111, CreateMissingEToken("hot_window"), CreateMissingEToken("="), (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())))))),
                            OList(
                                primaryElementParser: Custom(
                                    EToken("hot_window"),
                                    RequiredEToken("="),
                                    Required(
                                        Custom(
                                            rules.Value,
                                            RequiredEToken(".."),
                                            Required(rules.Value, rules.MissingValue),
                                            shape110),
                                        () => (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())),
                                    shape111),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape111, CreateMissingEToken("hot_window"), CreateMissingEToken("="), (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())),
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)))),
                    shape113));

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
                            shape104),
                        Custom(
                            EToken("caching-policies-modification-kind"),
                            RequiredEToken("="),
                            RequiredEToken("none", "replace", "union"),
                            shape104),
                        Custom(
                            EToken("database-name-override"),
                            RequiredEToken("="),
                            Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                            shape114),
                        Custom(
                            EToken("from"),
                            rules.StringLiteral,
                            First(
                                Custom(
                                    EToken("caching-policies-modification-kind"),
                                    EToken("="),
                                    EToken("none", "replace", "union"),
                                    shape104),
                                Custom(
                                    EToken("caching-policies-modification-kind"),
                                    RequiredEToken("="),
                                    RequiredEToken("none", "replace", "union"),
                                    shape104),
                                Custom(
                                    EToken("database-name-override"),
                                    RequiredEToken("="),
                                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                    shape114),
                                Custom(
                                    EToken("metadata"),
                                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                                    shape115),
                                Custom(
                                    EToken("prefetch-extents"),
                                    RequiredEToken("="),
                                    Required(rules.Value, rules.MissingValue),
                                    shape116),
                                Custom(
                                    EToken("principals-modification-kind"),
                                    RequiredEToken("="),
                                    RequiredEToken("none", "replace", "union"),
                                    shape104)),
                            shape107),
                        Custom(
                            EToken("metadata"),
                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                            shape115),
                        Custom(
                            EToken("prefetch-extents"),
                            RequiredEToken("="),
                            Required(rules.Value, rules.MissingValue),
                            shape116),
                        Custom(
                            EToken("principals-modification-kind"),
                            RequiredEToken("="),
                            RequiredEToken("none", "replace", "union"),
                            shape104)),
                    shape117));

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
                                shape107),
                            EToken("materialized-views"),
                            EToken("tables")),
                        RequiredEToken("exclude", "include"),
                        RequiredEToken("add", "drop"),
                        RequiredEToken("("),
                        Required(
                            OList(
                                primaryElementParser: Custom(
                                    rules.WildcardedNameDeclaration,
                                    shape46),
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
                    shape118));

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
                                                        shape46),
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
                                            shape87),
                                        Custom(
                                            EToken("materialized-view"),
                                            Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                                            shape119),
                                        Custom(
                                            EToken("tables"),
                                            RequiredEToken("("),
                                            Required(
                                                OList(
                                                    primaryElementParser: Custom(
                                                        rules.NameDeclarationOrStringLiteral,
                                                        shape46),
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
                                            shape87),
                                        Custom(
                                            EToken("table"),
                                            Required(rules.TableNameReference, rules.MissingNameReference),
                                            shape120)),
                                    () => (SyntaxElement)new CustomNode(shape87, CreateMissingEToken("materialized-views"), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration())), CreateMissingEToken(")"))),
                                shape107),
                            Custom(
                                EToken("materialized-views"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            shape46),
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
                                shape87),
                            Custom(
                                EToken("materialized-view"),
                                Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                                shape119),
                            Custom(
                                EToken("tables"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            shape46),
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
                                shape87),
                            Custom(
                                EToken("table"),
                                Required(rules.TableNameReference, rules.MissingNameReference),
                                shape120)),
                        () => (SyntaxElement)new CustomNode(shape107, CreateMissingEToken("from"), rules.MissingStringLiteral(), (SyntaxElement)new CustomNode(shape87, CreateMissingEToken("materialized-views"), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration())), CreateMissingEToken(")")))),
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
                                shape108),
                            Custom(
                                EToken("hot"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape109)),
                        () => (SyntaxElement)new CustomNode(shape108, CreateMissingEToken("hotdata"), CreateMissingEToken("="), rules.MissingValue(), CreateMissingEToken("hotindex"), CreateMissingEToken("="), rules.MissingValue())),
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
                                                    shape110),
                                                () => (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())),
                                            shape111),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape111, CreateMissingEToken("hot_window"), CreateMissingEToken("="), (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape111, CreateMissingEToken("hot_window"), CreateMissingEToken("="), (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())))))),
                            OList(
                                primaryElementParser: Custom(
                                    EToken("hot_window"),
                                    RequiredEToken("="),
                                    Required(
                                        Custom(
                                            rules.Value,
                                            RequiredEToken(".."),
                                            Required(rules.Value, rules.MissingValue),
                                            shape110),
                                        () => (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())),
                                    shape111),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape111, CreateMissingEToken("hot_window"), CreateMissingEToken("="), (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())),
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)))),
                    shape121));

            var AlterFunctionDocString = Command("AlterFunctionDocString", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("function"),
                    If(Not(EToken("with")), rules.DatabaseFunctionNameReference),
                    EToken("docstring"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape122));

            var AlterFunctionFolder = Command("AlterFunctionFolder", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("function"),
                    If(Not(EToken("with")), rules.DatabaseFunctionNameReference),
                    EToken("folder"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape123));

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
                                            shape86),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                Required(rules.DatabaseFunctionNameReference, rules.MissingNameReference),
                                shape124),
                            Custom(
                                If(Not(EToken("with")), rules.DatabaseFunctionNameReference),
                                shape15)),
                        () => (SyntaxElement)new CustomNode(shape124, CreateMissingEToken("with"), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()))), CreateMissingEToken(")"), rules.MissingNameReference())),
                    Required(rules.FunctionDeclaration, rules.MissingFunctionDeclaration)));

            var AlterMaterializedViewAutoUpdateSchema = Command("AlterMaterializedViewAutoUpdateSchema", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    If(Not(EToken("with")), rules.MaterializedViewNameReference),
                    EToken("autoUpdateSchema"),
                    RequiredEToken("false", "true"),
                    shape125));

            var AlterMaterializedViewDocString = Command("AlterMaterializedViewDocString", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    If(Not(EToken("with")), rules.MaterializedViewNameReference),
                    EToken("docstring"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape126));

            var AlterMaterializedViewFolder = Command("AlterMaterializedViewFolder", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    If(Not(EToken("with")), rules.MaterializedViewNameReference),
                    EToken("folder"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape127));

            var AlterMaterializedViewLookback = Command("AlterMaterializedViewLookback", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    If(Not(EToken("with")), rules.MaterializedViewNameReference),
                    EToken("lookback"),
                    Required(rules.Value, rules.MissingValue),
                    shape128));

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
                                    shape48),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingEToken("dimensionTables"), CreateMissingEToken("="), rules.MissingValue()),
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            EToken(")"),
                            rules.MaterializedViewNameReference,
                            shape129),
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
                                        shape48),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingEToken("dimensionTables"), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingEToken("dimensionTables"), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                            shape129),
                        Custom(
                            If(Not(EToken("with")), rules.MaterializedViewNameReference),
                            shape17)),
                    RequiredEToken("on"),
                    RequiredEToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Required(rules.FunctionBody, rules.MissingFunctionBody),
                    shape130));

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
                                shape55),
                            Custom(
                                EToken("hot"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape56)),
                        () => (SyntaxElement)new CustomNode(shape55, CreateMissingEToken("hotdata"), CreateMissingEToken("="), rules.MissingValue(), CreateMissingEToken("hotindex"), CreateMissingEToken("="), rules.MissingValue())),
                    shape37));

            var AlterMaterializedViewPolicyPartitioning = Command("AlterMaterializedViewPolicyPartitioning", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    If(Not(EToken("with")), rules.MaterializedViewNameReference),
                    EToken("policy"),
                    EToken("partitioning"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape36));

            var AlterMaterializedViewPolicyRetention = Command("AlterMaterializedViewPolicyRetention", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    If(Not(EToken("with")), rules.MaterializedViewNameReference),
                    EToken("policy"),
                    EToken("retention"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape131));

            var AlterMaterializedViewPolicyRowLevelSecurity = Command("AlterMaterializedViewPolicyRowLevelSecurity", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    Required(If(Not(EToken("with")), rules.MaterializedViewNameReference), rules.MissingNameReference),
                    RequiredEToken("policy"),
                    RequiredEToken("row_level_security"),
                    RequiredEToken("disable", "enable"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape132));

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
                                shape133),
                            Custom(
                                rules.StringLiteral,
                                shape0)),
                        () => (SyntaxElement)new CustomNode(shape133, CreateMissingEToken("internal"), rules.MissingStringLiteral()))));

            var AlterTablesPolicyCaching = Command("AlterTablesPolicyCaching", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("tables"),
                    EToken("("),
                    OList(
                        primaryElementParser: Custom(
                            rules.TableNameReference,
                            shape19),
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
                                shape55),
                            Custom(
                                EToken("hot"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape56)),
                        () => (SyntaxElement)new CustomNode(shape55, CreateMissingEToken("hotdata"), CreateMissingEToken("="), rules.MissingValue(), CreateMissingEToken("hotindex"), CreateMissingEToken("="), rules.MissingValue())),
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
                                                    shape110),
                                                () => (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())),
                                            shape111),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape111, CreateMissingEToken("hot_window"), CreateMissingEToken("="), (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape111, CreateMissingEToken("hot_window"), CreateMissingEToken("="), (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())))))),
                            OList(
                                primaryElementParser: Custom(
                                    EToken("hot_window"),
                                    RequiredEToken("="),
                                    Required(
                                        Custom(
                                            rules.Value,
                                            RequiredEToken(".."),
                                            Required(rules.Value, rules.MissingValue),
                                            shape110),
                                        () => (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())),
                                    shape111),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape111, CreateMissingEToken("hot_window"), CreateMissingEToken("="), (SyntaxElement)new CustomNode(shape110, rules.MissingValue(), CreateMissingEToken(".."), rules.MissingValue())),
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)))),
                    shape134));

            var AlterTablesPolicyIngestionBatching = Command("AlterTablesPolicyIngestionBatching", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("tables"),
                    EToken("("),
                    OList(
                        primaryElementParser: Custom(
                            rules.TableNameReference,
                            shape19),
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
                    shape135));

            var AlterTablesPolicyIngestionTime = Command("AlterTablesPolicyIngestionTime", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("tables"),
                    EToken("("),
                    OList(
                        primaryElementParser: Custom(
                            rules.TableNameReference,
                            shape19),
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
                    shape136));

            var AlterTablesPolicyMerge = Command("AlterTablesPolicyMerge", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("tables"),
                    EToken("("),
                    OList(
                        primaryElementParser: Custom(
                            rules.TableNameReference,
                            shape19),
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
                    shape137));

            var AlterTablesPolicyRestrictedViewAccess = Command("AlterTablesPolicyRestrictedViewAccess", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("tables"),
                    EToken("("),
                    OList(
                        primaryElementParser: Custom(
                            rules.TableNameReference,
                            shape19),
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
                    shape136));

            var AlterTablesPolicyRetention = Command("AlterTablesPolicyRetention", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("tables"),
                    EToken("("),
                    OList(
                        primaryElementParser: Custom(
                            rules.TableNameReference,
                            shape19),
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
                    shape138));

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
                                    shape19),
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
                                    shape139),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape139, rules.MissingNameDeclaration(), CreateMissingEToken("Expected asc,desc")),
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape139, rules.MissingNameDeclaration(), CreateMissingEToken("Expected asc,desc"))))),
                        RequiredEToken(")")}
                    ,
                    shape140));

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
                    shape141));

            var AlterTablePolicyAutoDelete = Command("AlterTablePolicyAutoDelete", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("auto_delete"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape142));

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
                                shape55),
                            Custom(
                                EToken("hot"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape56)),
                        () => (SyntaxElement)new CustomNode(shape55, CreateMissingEToken("hotdata"), CreateMissingEToken("="), rules.MissingValue(), CreateMissingEToken("hotindex"), CreateMissingEToken("="), rules.MissingValue())),
                    shape40));

            var AlterTablePolicyEncoding = Command("AlterTablePolicyEncoding", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape38));

            var AlterTablePolicyExtentTagsRetention = Command("AlterTablePolicyExtentTagsRetention", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("extent_tags_retention"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape143));

            var AlterTablePolicyIngestionBatching = Command("AlterTablePolicyIngestionBatching", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("ingestionbatching"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape144));

            var AlterTablePolicyMerge = Command("AlterTablePolicyMerge", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("merge"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape39));

            var AlterTablePolicyRestrictedViewAccess = Command("AlterTablePolicyRestrictedViewAccess", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("restricted_view_access"),
                    RequiredEToken("false", "true"),
                    shape40));

            var AlterTablePolicyRetention = Command("AlterTablePolicyRetention", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("retention"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape145));

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
                                shape41),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape41, rules.MissingNameReference(), CreateMissingEToken("Expected asc,desc")),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape41, rules.MissingNameReference(), CreateMissingEToken("Expected asc,desc"))))),
                    RequiredEToken(")"),
                    shape42));

            var AlterTablePolicySharding = Command("AlterTablePolicySharding", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("sharding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape43));

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
                                EToken("disable", "enable")),
                            Custom(
                                rules.StringLiteral,
                                shape0)),
                        () => CreateMissingEToken("Expected disable,enable")),
                    shape40));

            var AlterTablePolicyUpdate = Command("AlterTablePolicyUpdate", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    RequiredEToken("update"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape45));

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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape146));

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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape147));

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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape148));

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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape146));

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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape147));

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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape148));

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
                                shape47),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType())))),
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
                                        shape48),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingEToken("docstring"), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingEToken("docstring"), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"))),
                    shape49));

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
                                shape50),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape50, rules.MissingNameReference(), CreateMissingEToken(":"), rules.MissingStringLiteral()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape50, rules.MissingNameReference(), CreateMissingEToken(":"), rules.MissingStringLiteral())))),
                    RequiredEToken(")"),
                    shape51));

            var AlterTableColumnsPolicyEncoding = Command("AlterTableColumnsPolicyEncoding", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("columns"),
                    RequiredEToken("policy"),
                    RequiredEToken("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape149));

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
                            shape150)),
                    shape151));

            var AlterTableDocString = Command("AlterTableDocString", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("docstring"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape152));

            var AlterTableFolder = Command("AlterTableFolder", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("folder"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape153));

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
                    shape154));

            var AlterTablePolicyIngestionTime = Command("AlterTablePolicyIngestionTime", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("policy"),
                    EToken("ingestiontime"),
                    RequiredEToken("true"),
                    shape40));

            var AlterTablePolicyPartitioning = Command("AlterTablePolicyPartitioning", 
                Custom(
                    EToken("alter", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("policy"),
                    EToken("partitioning"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape52));

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
                                        shape86)),
                                RequiredEToken(")"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                shape155),
                            Custom(
                                rules.StringLiteral,
                                shape0)),
                        () => (SyntaxElement)new CustomNode(shape155, CreateMissingEToken("with"), CreateMissingEToken("("), SyntaxList<SeparatedElement<SyntaxElement>>.Empty(), CreateMissingEToken(")"), rules.MissingStringLiteral())),
                    shape156));

            var AppendTable = Command("AppendTable", 
                Custom(
                    EToken("append", CompletionKind.CommandPrefix),
                    Required(
                        First(
                            Custom(
                                EToken("async"),
                                Required(rules.TableNameReference, rules.MissingNameReference),
                                shape157),
                            Custom(
                                If(Not(EToken("async")), rules.TableNameReference),
                                shape19)),
                        () => (SyntaxElement)new CustomNode(shape157, CreateMissingEToken("async"), rules.MissingNameReference())),
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
                                            shape48),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("<|"))),
                        () => CreateMissingEToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    shape158));

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
                                shape70),
                            Custom(
                                rules.StringLiteral,
                                shape0)),
                        () => (SyntaxElement)new CustomNode(shape70, rules.MissingStringLiteral(), CreateMissingEToken(";"), rules.MissingStringLiteral())),
                    shape159));

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
                                shape70),
                            Custom(
                                rules.StringLiteral,
                                shape0)),
                        () => (SyntaxElement)new CustomNode(shape70, rules.MissingStringLiteral(), CreateMissingEToken(";"), rules.MissingStringLiteral())),
                    shape160));

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
                                shape0),
                            missingElement: null,
                            oneOrMore: true,
                            producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray())),
                        () => new SyntaxList<SyntaxElement>(rules.MissingValue())),
                    shape161));

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
                            shape162),
                        missingElement: () => (SyntaxElement)new CustomNode(shape162, CreateMissingEToken("into"), CreateMissingEToken("table"), rules.MissingNameReference()),
                        oneOrMore: false,
                        producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray())),
                    RequiredEToken("by"),
                    RequiredEToken("metadata"),
                    Required(
                        Custom(
                            EToken("<|"),
                            Required(rules.CommandInput, rules.MissingExpression),
                            shape84),
                        () => (SyntaxElement)new CustomNode(shape84, CreateMissingEToken("<|"), rules.MissingExpression())),
                    shape163));

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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape164));

            var CancelQuery = Command("CancelQuery", 
                Custom(
                    EToken("cancel", CompletionKind.CommandPrefix),
                    RequiredEToken("query"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape165));

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
                                        shape8)),
                                RequiredEToken(")"),
                                shape166),
                            Custom(
                                EToken("async"),
                                Optional(
                                    Custom(
                                        EToken("("),
                                        CommaList(
                                            Custom(
                                                If(Not(EToken(")")), rules.DatabaseNameReference),
                                                shape8)),
                                        RequiredEToken(")"),
                                        shape166)),
                                shape167))),
                    RequiredEToken("extentcontainers"),
                    shape168));

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
                    shape169));

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
                    shape170));

            var ClearMaterializedViewStatistics = Command("ClearMaterializedViewStatistics", 
                Custom(
                    EToken("clear", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    RequiredEToken("statistics"),
                    shape171));

            var ClearTableCacheStreamingIngestionSchema = Command("ClearTableCacheStreamingIngestionSchema", 
                Custom(
                    EToken("clear", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("cache"),
                    RequiredEToken("streamingingestion"),
                    RequiredEToken("schema"),
                    shape40));

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
                    shape172));

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
                                            shape47),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType())))),
                                RequiredEToken(")"),
                                shape173),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape173, rules.MissingNameDeclaration(), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()))), CreateMissingEToken(")")),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape173, rules.MissingNameDeclaration(), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()))), CreateMissingEToken(")"))))),
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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape174));

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
                                shape47),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType())))),
                    RequiredEToken(")"),
                    shape175));

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
                                            shape46),
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
                                shape64),
                            EToken("to")),
                        () => (SyntaxElement)new CustomNode(shape64, CreateMissingEToken("over"), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration())), CreateMissingEToken(")"), CreateMissingEToken("to"))),
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
                                            shape63),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("<|"),
                                shape64)),
                        () => CreateMissingEToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    shape176));

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
                                shape47),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()),
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
                                            shape86),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                shape87))}
                    ,
                    shape88));

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
                                    shape47),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()),
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType())))),
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
                                                                                shape89),
                                                                            Custom(
                                                                                EToken("startofday", "startofmonth", "startofweek", "startofyear"),
                                                                                RequiredEToken("("),
                                                                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                                RequiredEToken(")"),
                                                                                shape90)),
                                                                        () => (SyntaxElement)new CustomNode(shape89, CreateMissingEToken("bin"), CreateMissingEToken("("), rules.MissingNameDeclaration(), CreateMissingEToken(","), rules.MissingValue(), CreateMissingEToken(")"))))),
                                                            shape91),
                                                        Custom(
                                                            EToken("long"),
                                                            RequiredEToken("="),
                                                            RequiredEToken("hash"),
                                                            RequiredEToken("("),
                                                            Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                            RequiredEToken(","),
                                                            Required(rules.Value, rules.MissingValue),
                                                            RequiredEToken(")"),
                                                            shape92),
                                                        Custom(
                                                            EToken("string"),
                                                            Optional(
                                                                Custom(
                                                                    EToken("="),
                                                                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                    shape93)),
                                                            shape91)),
                                                    () => (SyntaxElement)new CustomNode(shape91, CreateMissingEToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingEToken("="), (SyntaxElement)new CustomNode(shape89, CreateMissingEToken("bin"), CreateMissingEToken("("), rules.MissingNameDeclaration(), CreateMissingEToken(","), rules.MissingValue(), CreateMissingEToken(")"))))),
                                                shape94),
                                            separatorParser: EToken(","),
                                            secondaryElementParser: null,
                                            missingPrimaryElement: null,
                                            missingSeparator: null,
                                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape94, rules.MissingNameDeclaration(), CreateMissingEToken(":"), (SyntaxElement)new CustomNode(shape91, CreateMissingEToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingEToken("="), (SyntaxElement)new CustomNode(shape89, CreateMissingEToken("bin"), CreateMissingEToken("("), rules.MissingNameDeclaration(), CreateMissingEToken(","), rules.MissingValue(), CreateMissingEToken(")"))))),
                                            endOfList: null,
                                            oneOrMore: true,
                                            allowTrailingSeparator: false,
                                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape94, rules.MissingNameDeclaration(), CreateMissingEToken(":"), (SyntaxElement)new CustomNode(shape91, CreateMissingEToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingEToken("="), (SyntaxElement)new CustomNode(shape89, CreateMissingEToken("bin"), CreateMissingEToken("("), rules.MissingNameDeclaration(), CreateMissingEToken(","), rules.MissingValue(), CreateMissingEToken(")")))))))),
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
                                                                            shape95),
                                                                        Custom(
                                                                            If(Not(EToken("datetime_pattern")), rules.NameDeclarationOrStringLiteral),
                                                                            shape46)),
                                                                    Optional(
                                                                        Custom(
                                                                            rules.StringLiteral,
                                                                            shape0)),
                                                                    shape11),
                                                                missingElement: null,
                                                                oneOrMore: true,
                                                                producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray())),
                                                            () => new SyntaxList<SyntaxElement>((SyntaxElement)new CustomNode(shape11, (SyntaxElement)new CustomNode(shape95, CreateMissingEToken("datetime_pattern"), CreateMissingEToken("("), rules.MissingStringLiteral(), CreateMissingEToken(","), rules.MissingNameDeclaration(), CreateMissingEToken(")")), rules.MissingStringLiteral()))),
                                                        shape96),
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
                                                                    shape95),
                                                                Custom(
                                                                    EToken("datetime_pattern"),
                                                                    RequiredEToken("("),
                                                                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                                                                    RequiredEToken(","),
                                                                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                    RequiredEToken(")"),
                                                                    shape95),
                                                                Custom(
                                                                    If(Not(EToken("datetime_pattern")), rules.NameDeclarationOrStringLiteral),
                                                                    shape46)),
                                                            Optional(
                                                                Custom(
                                                                    rules.StringLiteral,
                                                                    shape0)),
                                                            shape11),
                                                        missingElement: null,
                                                        oneOrMore: true,
                                                        producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray()))),
                                                () => (SyntaxElement)new CustomNode(shape96, rules.MissingStringLiteral(), new SyntaxList<SyntaxElement>((SyntaxElement)new CustomNode(shape11, (SyntaxElement)new CustomNode(shape95, CreateMissingEToken("datetime_pattern"), CreateMissingEToken("("), rules.MissingStringLiteral(), CreateMissingEToken(","), rules.MissingNameDeclaration(), CreateMissingEToken(")")), rules.MissingStringLiteral())))),
                                            RequiredEToken(")"),
                                            shape97)),
                                    RequiredEToken("dataformat"),
                                    shape98)),
                            () => CreateMissingEToken("dataformat")),
                        RequiredEToken("="),
                        RequiredEToken("apacheavro", "avro", "csv", "json", "multijson", "orc", "parquet", "psv", "raw", "scsv", "sohsv", "sstream", "tsve", "tsv", "txt", "w3clogfile"),
                        RequiredEToken("("),
                        Required(
                            OList(
                                primaryElementParser: Custom(
                                    rules.StringLiteral,
                                    shape0),
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
                                            shape86),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                shape87))}
                    ,
                    shape99));

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
                                            shape86),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                shape177),
                            Custom(
                                If(Not(EToken("with")), rules.NameDeclarationOrStringLiteral),
                                shape46)),
                        () => (SyntaxElement)new CustomNode(shape177, CreateMissingEToken("with"), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()))), CreateMissingEToken(")"), rules.MissingNameDeclaration())),
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
                                            shape48),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingEToken("autoUpdateSchema"), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingEToken("autoUpdateSchema"), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                                shape129),
                            Custom(
                                If(Not(EToken("with")), rules.MaterializedViewNameReference),
                                shape17)),
                        () => (SyntaxElement)new CustomNode(shape129, CreateMissingEToken("with"), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingEToken("autoUpdateSchema"), CreateMissingEToken("="), rules.MissingValue()))), CreateMissingEToken(")"), rules.MissingNameReference())),
                    RequiredEToken("on"),
                    RequiredEToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Required(rules.FunctionBody, rules.MissingFunctionBody),
                    shape130));

            var CreateOrAleterWorkloadGroup = Command("CreateOrAleterWorkloadGroup", 
                Custom(
                    EToken("create-or-alter", CompletionKind.CommandPrefix),
                    RequiredEToken("workload_group"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape53));

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
                            shape178)),
                    shape179));

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
                                shape0),
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
                    shape180));

            var CreateDatabaseVolatile = Command("CreateDatabaseVolatile", 
                Custom(
                    EToken("create", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.NameDeclarationOrStringLiteral,
                    EToken("volatile"),
                    Optional(EToken("ifnotexists")),
                    shape181));

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
                    shape182));

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
                                shape47),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()),
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
                                            shape86),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                shape87))}
                    ,
                    shape88));

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
                                    shape47),
                                separatorParser: EToken(","),
                                secondaryElementParser: null,
                                missingPrimaryElement: null,
                                missingSeparator: null,
                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()),
                                endOfList: null,
                                oneOrMore: true,
                                allowTrailingSeparator: false,
                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType())))),
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
                                                                                shape89),
                                                                            Custom(
                                                                                EToken("startofday", "startofmonth", "startofweek", "startofyear"),
                                                                                RequiredEToken("("),
                                                                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                                RequiredEToken(")"),
                                                                                shape90)),
                                                                        () => (SyntaxElement)new CustomNode(shape89, CreateMissingEToken("bin"), CreateMissingEToken("("), rules.MissingNameDeclaration(), CreateMissingEToken(","), rules.MissingValue(), CreateMissingEToken(")"))))),
                                                            shape91),
                                                        Custom(
                                                            EToken("long"),
                                                            RequiredEToken("="),
                                                            RequiredEToken("hash"),
                                                            RequiredEToken("("),
                                                            Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                            RequiredEToken(","),
                                                            Required(rules.Value, rules.MissingValue),
                                                            RequiredEToken(")"),
                                                            shape92),
                                                        Custom(
                                                            EToken("string"),
                                                            Optional(
                                                                Custom(
                                                                    EToken("="),
                                                                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                    shape93)),
                                                            shape91)),
                                                    () => (SyntaxElement)new CustomNode(shape91, CreateMissingEToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingEToken("="), (SyntaxElement)new CustomNode(shape89, CreateMissingEToken("bin"), CreateMissingEToken("("), rules.MissingNameDeclaration(), CreateMissingEToken(","), rules.MissingValue(), CreateMissingEToken(")"))))),
                                                shape94),
                                            separatorParser: EToken(","),
                                            secondaryElementParser: null,
                                            missingPrimaryElement: null,
                                            missingSeparator: null,
                                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape94, rules.MissingNameDeclaration(), CreateMissingEToken(":"), (SyntaxElement)new CustomNode(shape91, CreateMissingEToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingEToken("="), (SyntaxElement)new CustomNode(shape89, CreateMissingEToken("bin"), CreateMissingEToken("("), rules.MissingNameDeclaration(), CreateMissingEToken(","), rules.MissingValue(), CreateMissingEToken(")"))))),
                                            endOfList: null,
                                            oneOrMore: true,
                                            allowTrailingSeparator: false,
                                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape94, rules.MissingNameDeclaration(), CreateMissingEToken(":"), (SyntaxElement)new CustomNode(shape91, CreateMissingEToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingEToken("="), (SyntaxElement)new CustomNode(shape89, CreateMissingEToken("bin"), CreateMissingEToken("("), rules.MissingNameDeclaration(), CreateMissingEToken(","), rules.MissingValue(), CreateMissingEToken(")")))))))),
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
                                                                            shape95),
                                                                        Custom(
                                                                            If(Not(EToken("datetime_pattern")), rules.NameDeclarationOrStringLiteral),
                                                                            shape46)),
                                                                    Optional(
                                                                        Custom(
                                                                            rules.StringLiteral,
                                                                            shape0)),
                                                                    shape11),
                                                                missingElement: null,
                                                                oneOrMore: true,
                                                                producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray())),
                                                            () => new SyntaxList<SyntaxElement>((SyntaxElement)new CustomNode(shape11, (SyntaxElement)new CustomNode(shape95, CreateMissingEToken("datetime_pattern"), CreateMissingEToken("("), rules.MissingStringLiteral(), CreateMissingEToken(","), rules.MissingNameDeclaration(), CreateMissingEToken(")")), rules.MissingStringLiteral()))),
                                                        shape96),
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
                                                                    shape95),
                                                                Custom(
                                                                    EToken("datetime_pattern"),
                                                                    RequiredEToken("("),
                                                                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                                                                    RequiredEToken(","),
                                                                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                                                    RequiredEToken(")"),
                                                                    shape95),
                                                                Custom(
                                                                    If(Not(EToken("datetime_pattern")), rules.NameDeclarationOrStringLiteral),
                                                                    shape46)),
                                                            Optional(
                                                                Custom(
                                                                    rules.StringLiteral,
                                                                    shape0)),
                                                            shape11),
                                                        missingElement: null,
                                                        oneOrMore: true,
                                                        producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray()))),
                                                () => (SyntaxElement)new CustomNode(shape96, rules.MissingStringLiteral(), new SyntaxList<SyntaxElement>((SyntaxElement)new CustomNode(shape11, (SyntaxElement)new CustomNode(shape95, CreateMissingEToken("datetime_pattern"), CreateMissingEToken("("), rules.MissingStringLiteral(), CreateMissingEToken(","), rules.MissingNameDeclaration(), CreateMissingEToken(")")), rules.MissingStringLiteral())))),
                                            RequiredEToken(")"),
                                            shape97)),
                                    RequiredEToken("dataformat"),
                                    shape98)),
                            () => CreateMissingEToken("dataformat")),
                        RequiredEToken("="),
                        RequiredEToken("apacheavro", "avro", "csv", "json", "multijson", "orc", "parquet", "psv", "raw", "scsv", "sohsv", "sstream", "tsve", "tsv", "txt", "w3clogfile"),
                        RequiredEToken("("),
                        Required(
                            OList(
                                primaryElementParser: Custom(
                                    rules.StringLiteral,
                                    shape0),
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
                                            shape86),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape86, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                shape87))}
                    ,
                    shape99));

            var CreateExternalTableMapping = Command("CreateExternalTableMapping", 
                Custom(
                    EToken("create", CompletionKind.CommandPrefix),
                    EToken("external"),
                    RequiredEToken("table"),
                    Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                    RequiredEToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape102));

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
                                                shape86)),
                                        RequiredEToken(")"),
                                        shape87)),
                                shape167),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                CommaList(
                                    Custom(
                                        If(Not(EToken(")")), rules.NameDeclarationOrStringLiteral),
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape86)),
                                RequiredEToken(")"),
                                shape87))),
                    Required(If(Not(And(EToken("ifnotexists", "with"))), rules.NameDeclarationOrStringLiteral), rules.MissingNameDeclaration),
                    Required(rules.FunctionDeclaration, rules.MissingFunctionDeclaration),
                    shape183));

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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape184));

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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape184));

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
                                            shape47),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType())))),
                                RequiredEToken(")"),
                                shape173),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape173, rules.MissingNameDeclaration(), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()))), CreateMissingEToken(")")),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape173, rules.MissingNameDeclaration(), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()))), CreateMissingEToken(")"))))),
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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape174));

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
                                shape47),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType())))),
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
                                        shape48),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingEToken("docstring"), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingEToken("docstring"), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"))),
                    shape185));

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
                                        shape48),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingEToken("docstring"), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingEToken("docstring"), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"))),
                    shape186));

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
                    shape187));

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
                                shape167),
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
                                            shape48),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingEToken("autoUpdateSchema"), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingEToken("autoUpdateSchema"), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                shape188),
                            Custom(
                                If(Not(EToken("with")), rules.NameDeclarationOrStringLiteral),
                                shape46)),
                        () => (SyntaxElement)new CustomNode(shape188, CreateMissingEToken("with"), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingEToken("autoUpdateSchema"), CreateMissingEToken("="), rules.MissingValue()))), CreateMissingEToken(")"), rules.MissingNameDeclaration())),
                    RequiredEToken("on"),
                    RequiredEToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Required(rules.FunctionBody, rules.MissingFunctionBody),
                    shape189));

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
                                            shape47),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType())))),
                                RequiredEToken(")"),
                                shape173),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape173, rules.MissingNameDeclaration(), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()))), CreateMissingEToken(")")),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape173, rules.MissingNameDeclaration(), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape47, rules.MissingNameDeclaration(), CreateMissingEToken(":"), rules.MissingType()))), CreateMissingEToken(")"))))),
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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape174));

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
                    shape190));

            var DeleteColumnPolicyEncoding = Command("DeleteColumnPolicyEncoding", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("column"),
                    Required(rules.TableColumnNameReference, rules.MissingNameReference),
                    RequiredEToken("policy"),
                    RequiredEToken("encoding"),
                    shape190));

            var DeleteDatabasePolicyCaching = Command("DeleteDatabasePolicyCaching", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("caching"),
                    shape191));

            var DeleteDatabasePolicyDiagnostics = Command("DeleteDatabasePolicyDiagnostics", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("diagnostics"),
                    shape191));

            var DeleteDatabasePolicyEncoding = Command("DeleteDatabasePolicyEncoding", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("encoding"),
                    shape191));

            var DeleteDatabasePolicyExtentTagsRetention = Command("DeleteDatabasePolicyExtentTagsRetention", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("extent_tags_retention"),
                    shape191));

            var DeleteDatabasePolicyIngestionBatching = Command("DeleteDatabasePolicyIngestionBatching", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("ingestionbatching"),
                    shape191));

            var DeleteDatabasePolicyManagedIdentity = Command("DeleteDatabasePolicyManagedIdentity", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("managed_identity"),
                    shape191));

            var DeleteDatabasePolicyMerge = Command("DeleteDatabasePolicyMerge", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("merge"),
                    shape191));

            var DeleteDatabasePolicyRetention = Command("DeleteDatabasePolicyRetention", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("retention"),
                    shape191));

            var DeleteDatabasePolicySharding = Command("DeleteDatabasePolicySharding", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("sharding"),
                    shape191));

            var DeleteDatabasePolicyShardsGrouping = Command("DeleteDatabasePolicyShardsGrouping", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    EToken("shards_grouping").Hide(),
                    shape191));

            var DeleteDatabasePolicyStreamingIngestion = Command("DeleteDatabasePolicyStreamingIngestion", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredEToken("policy"),
                    RequiredEToken("streamingingestion"),
                    shape191));

            var DropFollowerDatabasePolicyCaching = Command("DropFollowerDatabasePolicyCaching", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("follower"),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("policy"),
                    RequiredEToken("caching"),
                    shape192));

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
                                            shape46),
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
                                shape87),
                            Custom(
                                EToken("materialized-view"),
                                Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                                shape119),
                            Custom(
                                EToken("tables"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            shape46),
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
                                shape87),
                            Custom(
                                EToken("table"),
                                Required(rules.TableNameReference, rules.MissingNameReference),
                                shape120)),
                        () => (SyntaxElement)new CustomNode(shape87, CreateMissingEToken("materialized-views"), CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration())), CreateMissingEToken(")"))),
                    RequiredEToken("policy"),
                    RequiredEToken("caching"),
                    shape193));

            var DeleteMaterializedViewPolicyCaching = Command("DeleteMaterializedViewPolicyCaching", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("policy"),
                    EToken("caching"),
                    shape125));

            var DeleteMaterializedViewPolicyPartitioning = Command("DeleteMaterializedViewPolicyPartitioning", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("policy"),
                    EToken("partitioning"),
                    shape125));

            var DeleteMaterializedViewPolicyRowLevelSecurity = Command("DeleteMaterializedViewPolicyRowLevelSecurity", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    RequiredEToken("policy"),
                    RequiredEToken("row_level_security"),
                    shape125));

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
                                shape0),
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
                    shape194));

            var DeleteTablePolicyAutoDelete = Command("DeleteTablePolicyAutoDelete", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("auto_delete"),
                    shape195));

            var DeleteTablePolicyCaching = Command("DeleteTablePolicyCaching", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("caching"),
                    shape195));

            var DeleteTablePolicyEncoding = Command("DeleteTablePolicyEncoding", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("encoding"),
                    shape195));

            var DeleteTablePolicyExtentTagsRetention = Command("DeleteTablePolicyExtentTagsRetention", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("extent_tags_retention"),
                    shape195));

            var DeleteTablePolicyIngestionBatching = Command("DeleteTablePolicyIngestionBatching", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("ingestionbatching"),
                    shape195));

            var DeleteTablePolicyMerge = Command("DeleteTablePolicyMerge", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("merge"),
                    shape195));

            var DeleteTablePolicyRestrictedViewAccess = Command("DeleteTablePolicyRestrictedViewAccess", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("restricted_view_access"),
                    shape195));

            var DeleteTablePolicyRetention = Command("DeleteTablePolicyRetention", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("retention"),
                    shape195));

            var DeleteTablePolicyRowOrder = Command("DeleteTablePolicyRowOrder", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("roworder"),
                    shape195));

            var DeleteTablePolicySharding = Command("DeleteTablePolicySharding", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("sharding"),
                    shape195));

            var DeleteTablePolicyStreamingIngestion = Command("DeleteTablePolicyStreamingIngestion", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    EToken("policy"),
                    EToken("streamingingestion"),
                    shape195));

            var DeleteTablePolicyUpdate = Command("DeleteTablePolicyUpdate", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    RequiredEToken("policy"),
                    RequiredEToken("update"),
                    shape195));

            var DeleteTablePolicyIngestionTime = Command("DeleteTablePolicyIngestionTime", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("policy"),
                    EToken("ingestiontime"),
                    shape195));

            var DeleteTablePolicyPartitioning = Command("DeleteTablePolicyPartitioning", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("policy"),
                    EToken("partitioning"),
                    shape195));

            var DeleteTablePolicyRowLevelSecurity = Command("DeleteTablePolicyRowLevelSecurity", 
                Custom(
                    EToken("delete", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("policy"),
                    RequiredEToken("row_level_security"),
                    shape195));

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
                                    shape84)),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape63),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                Required(
                                    Custom(
                                        EToken("<|"),
                                        Required(rules.CommandInput, rules.MissingExpression),
                                        shape84),
                                    () => (SyntaxElement)new CustomNode(shape84, CreateMissingEToken("<|"), rules.MissingExpression())),
                                shape196)),
                        () => (SyntaxElement)new CustomNode(shape84, CreateMissingEToken("<|"), rules.MissingExpression())),
                    shape195));

            var DetachDatabase = Command("DetachDatabase", 
                Custom(
                    EToken("detach", CompletionKind.CommandPrefix),
                    EToken("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    shape197));

            var DisableContinuousExport = Command("DisableContinuousExport", 
                Custom(
                    EToken("disable", CompletionKind.CommandPrefix),
                    EToken("continuous-export"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    shape198));

            var DisableDatabaseMaintenanceMode = Command("DisableDatabaseMaintenanceMode", 
                Custom(
                    EToken("disable", CompletionKind.CommandPrefix),
                    EToken("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredEToken("maintenance_mode"),
                    shape199));

            var DisablePlugin = Command("DisablePlugin", 
                Custom(
                    EToken("disable", CompletionKind.CommandPrefix),
                    RequiredEToken("plugin"),
                    Required(
                        First(
                            rules.StringLiteral,
                            rules.NameDeclarationOrStringLiteral),
                        rules.MissingStringLiteral),
                    shape200));

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
                                shape201)),
                        () => CreateMissingEToken("from")),
                    Required(
                        First(
                            Custom(
                                EToken("all"),
                                RequiredEToken("tables")),
                            Custom(
                                If(Not(EToken("all")), rules.TableNameReference),
                                shape19)),
                        () => (SyntaxElement)new CustomNode(CreateMissingEToken("all"), CreateMissingEToken("tables"))),
                    Optional(
                        First(
                            Custom(
                                EToken("limit"),
                                Required(rules.Value, rules.MissingValue),
                                shape202),
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
                                        shape202)),
                                shape203))),
                    shape204));

            var DropBasicAuthUser = Command("DropBasicAuthUser", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("basicauth"),
                    RequiredEToken("user"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape205));

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
                                        shape1)),
                                shape2),
                            Custom(
                                EToken("user"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                shape1))),
                    shape206));

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
                                shape0),
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
                                        shape0)),
                                shape6),
                            Custom(
                                rules.StringLiteral,
                                shape0))),
                    shape7));

            var DropColumn = Command("DropColumn", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("column"),
                    Required(rules.TableColumnNameReference, rules.MissingNameReference),
                    shape207));

            var DropContinuousExport = Command("DropContinuousExport", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("continuous-export"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    shape208));

            var DropDatabaseIngestionMapping = Command("DropDatabaseIngestionMapping", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("ingestion"),
                    RequiredEToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredEToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape209));

            var DropDatabasePrettyName = Command("DropDatabasePrettyName", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("database"),
                    rules.DatabaseNameReference,
                    EToken("prettyname"),
                    shape199));

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
                                shape0),
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
                                        shape0)),
                                shape6),
                            Custom(
                                rules.StringLiteral,
                                shape0))),
                    shape9));

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
                                                    shape63),
                                                separatorParser: EToken(","),
                                                secondaryElementParser: null,
                                                missingPrimaryElement: null,
                                                missingSeparator: null,
                                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                                endOfList: null,
                                                oneOrMore: true,
                                                allowTrailingSeparator: false,
                                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                        RequiredEToken(")"),
                                        shape87)),
                                shape167),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape63),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                shape87))),
                    shape210));

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
                                    shape84)),
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
                                        shape84),
                                    () => (SyntaxElement)new CustomNode(shape84, CreateMissingEToken("<|"), rules.MissingExpression())),
                                shape211)),
                        () => (SyntaxElement)new CustomNode(shape84, CreateMissingEToken("<|"), rules.MissingExpression())),
                    shape212));

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
                                            shape0),
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
                                        shape157)),
                                shape213),
                            Custom(
                                EToken("<|"),
                                Required(rules.CommandInput, rules.MissingExpression),
                                shape214),
                            Custom(
                                EToken("from"),
                                Required(
                                    First(
                                        Custom(
                                            EToken("all"),
                                            RequiredEToken("tables")),
                                        Custom(
                                            If(Not(EToken("all")), rules.TableNameReference),
                                            shape19)),
                                    () => (SyntaxElement)new CustomNode(CreateMissingEToken("all"), CreateMissingEToken("tables"))),
                                Optional(
                                    First(
                                        Custom(
                                            EToken("limit"),
                                            Required(rules.Value, rules.MissingValue),
                                            shape202),
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
                                                    shape202)),
                                            shape203))),
                                shape184),
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
                                            shape19)),
                                    () => (SyntaxElement)new CustomNode(CreateMissingEToken("all"), CreateMissingEToken("tables"))),
                                Optional(
                                    First(
                                        Custom(
                                            EToken("limit"),
                                            Required(rules.Value, rules.MissingValue),
                                            shape202),
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
                                                    shape202)),
                                            shape203))),
                                shape215),
                            Custom(
                                EToken("whatif"),
                                RequiredEToken("<|"),
                                Required(rules.CommandInput, rules.MissingExpression),
                                shape216)),
                        () => (SyntaxElement)new CustomNode(shape213, CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingValue())), CreateMissingEToken(")"), (SyntaxElement)new CustomNode(shape157, CreateMissingEToken("from"), rules.MissingNameReference())))));

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
                            shape157)),
                    shape217));

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
                                shape0),
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
                                        shape0)),
                                shape11),
                            Custom(
                                rules.StringLiteral,
                                shape0))),
                    shape12));

            var DropExternalTableMapping = Command("DropExternalTableMapping", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("external"),
                    EToken("table"),
                    rules.ExternalTableNameReference,
                    EToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape218));

            var DropExternalTable = Command("DropExternalTable", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("external"),
                    RequiredEToken("table"),
                    Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                    shape219));

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
                                    shape8),
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
                            shape220),
                        Custom(
                            EToken("databases"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.DatabaseNameReference,
                                        shape8),
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
                            shape220),
                        Custom(
                            EToken("database"),
                            rules.DatabaseNameReference,
                            shape221)),
                    RequiredEToken("from"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape222));

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
                                shape107)),
                        () => CreateMissingEToken("(")),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                shape0),
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
                    shape223));

            var DropFunctions = Command("DropFunctions", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("functions"),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.DatabaseFunctionNameReference,
                                shape15),
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
                    shape224));

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
                                shape0),
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
                                        shape0)),
                                shape6),
                            Custom(
                                rules.StringLiteral,
                                shape0))),
                    shape16));

            var DropFunction = Command("DropFunction", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("function"),
                    Required(rules.DatabaseFunctionNameReference, rules.MissingNameReference),
                    Optional(EToken("ifexists")),
                    shape225));

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
                                shape0),
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
                            shape0)),
                    shape18));

            var DropMaterializedView = Command("DropMaterializedView", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    shape226));

            var DropRowStore = Command("DropRowStore", 
                Custom(
                    EToken("detach", "drop"),
                    EToken("rowstore"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    Optional(EToken("ifexists")),
                    shape227));

            var StoredQueryResultsDrop = Command("StoredQueryResultsDrop", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("stored_query_results"),
                    RequiredEToken("by"),
                    RequiredEToken("user"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape228));

            var StoredQueryResultDrop = Command("StoredQueryResultDrop", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("stored_query_result"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    shape229));

            var DropStoredQueryResultContainers = Command("DropStoredQueryResultContainers", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("storedqueryresultcontainers"),
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
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("tables"),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.TableNameReference,
                                shape19),
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
                    shape231));

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
                                shape25),
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
                    shape51));

            var DropTableIngestionMapping = Command("DropTableIngestionMapping", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.TableNameReference,
                    EToken("ingestion"),
                    RequiredEToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredEToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape232));

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
                                shape0),
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
                                        shape0)),
                                shape6),
                            Custom(
                                rules.StringLiteral,
                                shape0))),
                    shape20));

            var DropTable = Command("DropTable", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Optional(EToken("ifexists")),
                    shape233));

            var DropTempStorage = Command("DropTempStorage", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("tempstorage"),
                    RequiredEToken("older"),
                    Required(rules.Value, rules.MissingValue),
                    shape234));

            var DropUnusedStoredQueryResultContainers = Command("DropUnusedStoredQueryResultContainers", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    EToken("unused"),
                    RequiredEToken("storedqueryresultcontainers"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    shape235));

            var DropWorkloadGroup = Command("DropWorkloadGroup", 
                Custom(
                    EToken("drop", CompletionKind.CommandPrefix),
                    RequiredEToken("workload_group"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    shape236));

            var EnableContinuousExport = Command("EnableContinuousExport", 
                Custom(
                    EToken("enable", CompletionKind.CommandPrefix),
                    EToken("continuous-export"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    shape208));

            var EnableDatabaseMaintenanceMode = Command("EnableDatabaseMaintenanceMode", 
                Custom(
                    EToken("enable", CompletionKind.CommandPrefix),
                    EToken("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredEToken("maintenance_mode"),
                    shape199));

            var EnableDisableMaterializedView = Command("EnableDisableMaterializedView", 
                Custom(
                    EToken("disable", "enable"),
                    EToken("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    shape226));

            var EnablePlugin = Command("EnablePlugin", 
                Custom(
                    EToken("enable", CompletionKind.CommandPrefix),
                    RequiredEToken("plugin"),
                    Required(
                        First(
                            rules.StringLiteral,
                            rules.NameDeclarationOrStringLiteral),
                        rules.MissingStringLiteral),
                    shape237));

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
                                            shape63),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("<|"),
                                shape64)),
                        () => CreateMissingEToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    shape238));

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
                                            shape63),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("<|"),
                                shape64)),
                        () => CreateMissingEToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    shape239));

            var ExportToStorage = Command("ExportToStorage", 
                Custom(
                    EToken("export", CompletionKind.CommandPrefix),
                    Optional(
                        First(
                            Custom(
                                EToken("async"),
                                Optional(EToken("compressed")),
                                shape167),
                            EToken("compressed"))),
                    RequiredEToken("to"),
                    RequiredEToken("csv", "json", "parquet", "tsv"),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.StringLiteral,
                                shape0),
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
                                            shape63),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("<|"),
                                shape64)),
                        () => CreateMissingEToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    shape240));

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
                                shape241),
                            Custom(
                                EToken("<|"),
                                Required(rules.InputText, rules.MissingInputText),
                                shape242),
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
                                            shape48),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("<|"),
                                Required(rules.InputText, rules.MissingInputText),
                                shape243)),
                        () => (SyntaxElement)new CustomNode(shape241, CreateMissingEToken("["), rules.MissingInputText(), CreateMissingEToken("]"))),
                    shape244));

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
                                            shape0),
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
                                shape245),
                            Custom(
                                rules.StringLiteral,
                                shape0)),
                        () => (SyntaxElement)new CustomNode(shape245, CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral())), CreateMissingEToken(")"))),
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
                                        shape48),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"))),
                    shape246));

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
                                shape0),
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
                    shape247));

            var MergeExtents = Command("MergeExtents", 
                Custom(
                    EToken("merge", CompletionKind.CommandPrefix),
                    Required(
                        First(
                            Custom(
                                EToken("async"),
                                Required(rules.TableNameReference, rules.MissingNameReference),
                                shape157),
                            Custom(
                                If(Not(And(EToken("dryrun", "async"))), rules.TableNameReference),
                                shape19)),
                        () => (SyntaxElement)new CustomNode(shape157, CreateMissingEToken("async"), rules.MissingNameReference())),
                    RequiredEToken("("),
                    Required(
                        OList(
                            primaryElementParser: Custom(
                                rules.AnyGuidLiteralOrString,
                                shape0),
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
                    shape248));

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
                                    shape0),
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
                            shape245),
                        Custom(
                            EToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.AnyGuidLiteralOrString,
                                        shape0),
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
                            shape245),
                        EToken("all")),
                    RequiredEToken("from"),
                    RequiredEToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredEToken("to"),
                    RequiredEToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    shape249));

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
                    shape250));

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
                                shape251),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape251, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingNameReference()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape251, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingNameReference())))),
                    shape252));

            var RenameColumn = Command("RenameColumn", 
                Custom(
                    EToken("rename", CompletionKind.CommandPrefix),
                    EToken("column"),
                    Required(rules.DatabaseTableColumnNameReference, rules.MissingNameReference),
                    RequiredEToken("to"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    shape253));

            var RenameMaterializedView = Command("RenameMaterializedView", 
                Custom(
                    EToken("rename", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    RequiredEToken("to"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    shape254));

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
                                shape255),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape255, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingNameReference()),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape255, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingNameReference())))),
                    shape252));

            var RenameTable = Command("RenameTable", 
                Custom(
                    EToken("rename", CompletionKind.CommandPrefix),
                    RequiredEToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredEToken("to"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    shape256));

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
                    shape257));

            var SetOrAppendTable = Command("SetOrAppendTable", 
                Custom(
                    EToken("set-or-append", CompletionKind.CommandPrefix),
                    Required(
                        First(
                            Custom(
                                EToken("async"),
                                Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                                shape258),
                            Custom(
                                If(Not(EToken("async")), rules.NameDeclarationOrStringLiteral),
                                shape46)),
                        () => (SyntaxElement)new CustomNode(shape258, CreateMissingEToken("async"), rules.MissingNameDeclaration())),
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
                                            shape48),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("<|"))),
                        () => CreateMissingEToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    shape158));

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
                                            shape48),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("<|"))),
                        () => CreateMissingEToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    shape259));

            var SetOrReplaceTable = Command("SetOrReplaceTable", 
                Custom(
                    EToken("set-or-replace", CompletionKind.CommandPrefix),
                    Required(
                        First(
                            Custom(
                                EToken("async"),
                                Required(If(Not(EToken("stored_query_result")), rules.NameDeclarationOrStringLiteral), rules.MissingNameDeclaration),
                                shape258),
                            Custom(
                                If(Not(And(EToken("async", "stored_query_result"))), rules.NameDeclarationOrStringLiteral),
                                shape46)),
                        () => (SyntaxElement)new CustomNode(shape258, CreateMissingEToken("async"), rules.MissingNameDeclaration())),
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
                                            shape48),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("<|"))),
                        () => CreateMissingEToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    shape158));

            var SetAccess = Command("SetAccess", 
                Custom(
                    EToken("set", CompletionKind.CommandPrefix),
                    EToken("access"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredEToken("to"),
                    RequiredEToken("readonly", "readwrite"),
                    shape260));

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
                                            shape0),
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
                                                    shape0)),
                                            shape6),
                                        Custom(
                                            rules.StringLiteral,
                                            shape0))),
                                shape213),
                            Custom(
                                EToken("none"),
                                Optional(
                                    Custom(
                                        EToken("skip-results"))),
                                shape167)),
                        () => (SyntaxElement)new CustomNode(shape213, CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral())), CreateMissingEToken(")"), (SyntaxElement)new CustomNode(shape6, CreateMissingEToken("skip-results"), rules.MissingStringLiteral()))),
                    shape261));

            var SetContinuousExportCursor = Command("SetContinuousExportCursor", 
                Custom(
                    EToken("set", CompletionKind.CommandPrefix),
                    EToken("continuous-export"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    RequiredEToken("cursor"),
                    RequiredEToken("to"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape262));

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
                                            shape0),
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
                                                    shape0)),
                                            shape6),
                                        Custom(
                                            rules.StringLiteral,
                                            shape0))),
                                shape213),
                            Custom(
                                EToken("none"),
                                Optional(
                                    Custom(
                                        EToken("skip-results"))),
                                shape167)),
                        () => (SyntaxElement)new CustomNode(shape213, CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral())), CreateMissingEToken(")"), (SyntaxElement)new CustomNode(shape6, CreateMissingEToken("skip-results"), rules.MissingStringLiteral()))),
                    shape263));

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
                                            shape0),
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
                                                    shape0)),
                                            shape11),
                                        Custom(
                                            rules.StringLiteral,
                                            shape0))),
                                shape213),
                            Custom(
                                EToken("none"),
                                Optional(EToken("skip-results")),
                                shape167)),
                        () => (SyntaxElement)new CustomNode(shape213, CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral())), CreateMissingEToken(")"), (SyntaxElement)new CustomNode(shape11, CreateMissingEToken("skip-results"), rules.MissingStringLiteral()))),
                    shape264));

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
                                            shape0),
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
                                                    shape0)),
                                            shape6),
                                        Custom(
                                            rules.StringLiteral,
                                            shape0))),
                                shape213),
                            Custom(
                                EToken("none"),
                                Optional(
                                    Custom(
                                        EToken("skip-results"))),
                                shape167)),
                        () => (SyntaxElement)new CustomNode(shape213, CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral())), CreateMissingEToken(")"), (SyntaxElement)new CustomNode(shape6, CreateMissingEToken("skip-results"), rules.MissingStringLiteral()))),
                    shape265));

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
                                            shape0),
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
                                        shape0)),
                                shape266),
                            EToken("none")),
                        () => (SyntaxElement)new CustomNode(shape266, CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral())), CreateMissingEToken(")"), rules.MissingStringLiteral())),
                    shape267));

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
                            shape268)),
                    shape269));

            var SetMaterializedViewCursor = Command("SetMaterializedViewCursor", 
                Custom(
                    EToken("set", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    RequiredEToken("cursor"),
                    RequiredEToken("to"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape270));

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
                                            shape48),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("<|"))),
                        () => CreateMissingEToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    shape259));

            var SetTableRowStoreReferences = Command("SetTableRowStoreReferences", 
                Custom(
                    EToken("set", CompletionKind.CommandPrefix),
                    EToken("table"),
                    rules.DatabaseTableNameReference,
                    RequiredEToken("rowstore_references"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape271));

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
                                            shape0),
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
                                                    shape0)),
                                            shape6),
                                        Custom(
                                            rules.StringLiteral,
                                            shape0))),
                                shape213),
                            Custom(
                                EToken("none"),
                                Optional(
                                    Custom(
                                        EToken("skip-results"))),
                                shape167)),
                        () => (SyntaxElement)new CustomNode(shape213, CreateMissingEToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral())), CreateMissingEToken(")"), (SyntaxElement)new CustomNode(shape6, CreateMissingEToken("skip-results"), rules.MissingStringLiteral()))),
                    shape272));

            var SetTable = Command("SetTable", 
                Custom(
                    EToken("set", CompletionKind.CommandPrefix),
                    Required(
                        First(
                            Custom(
                                EToken("async"),
                                Required(If(Not(EToken("stored_query_result")), rules.NameDeclarationOrStringLiteral), rules.MissingNameDeclaration),
                                shape258),
                            Custom(
                                If(Not(And(EToken("access", "cluster", "continuous-export", "database", "external", "function", "materialized-view", "async", "stored_query_result", "table"))), rules.NameDeclarationOrStringLiteral),
                                shape46)),
                        () => (SyntaxElement)new CustomNode(shape258, CreateMissingEToken("async"), rules.MissingNameDeclaration())),
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
                                            shape48),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingEToken("creationTime"), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("<|"))),
                        () => CreateMissingEToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    shape158));

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
                                                    shape63),
                                                separatorParser: EToken(","),
                                                secondaryElementParser: null,
                                                missingPrimaryElement: null,
                                                missingSeparator: null,
                                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                                endOfList: null,
                                                oneOrMore: true,
                                                allowTrailingSeparator: false,
                                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                        RequiredEToken(")"),
                                        shape87)),
                                shape273),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            RequiredEToken("="),
                                            Required(rules.Value, rules.MissingValue),
                                            shape63),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                shape87))),
                    shape184));

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
                            shape274)),
                    shape184));

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
                                        shape0),
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
                                shape213),
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
                                            shape275),
                                        separatorParser: EToken("and"),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape275, CreateMissingEToken("tags"), CreateMissingEToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape275, CreateMissingEToken("tags"), CreateMissingEToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral())))),
                                Optional(
                                    Custom(
                                        EToken("with"),
                                        RequiredEToken("("),
                                        RequiredEToken("extentsShowFilteringRuntimePolicy"),
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        RequiredEToken(")"),
                                        shape276)),
                                shape184),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                RequiredEToken("extentsShowFilteringRuntimePolicy"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredEToken(")"),
                                shape276))),
                    shape277));

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
                                            shape0),
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
                                shape213),
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
                                            shape275),
                                        separatorParser: EToken("and"),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape275, CreateMissingEToken("tags"), CreateMissingEToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape275, CreateMissingEToken("tags"), CreateMissingEToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral())))),
                                Optional(
                                    Custom(
                                        EToken("with"),
                                        RequiredEToken("("),
                                        RequiredEToken("extentsShowFilteringRuntimePolicy"),
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        RequiredEToken(")"),
                                        shape276)),
                                shape184),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                RequiredEToken("extentsShowFilteringRuntimePolicy"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredEToken(")"),
                                shape276))),
                    shape278));

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
                            shape0)),
                    shape279));

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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape204));

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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape204));

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
                                shape280)),
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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape204));

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
                    shape281));

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
                    shape282));

            var ShowColumnPolicyEncoding = Command("ShowColumnPolicyEncoding", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("column"),
                    Required(If(Not(EToken("*")), rules.TableColumnNameReference), rules.MissingNameReference),
                    RequiredEToken("policy"),
                    RequiredEToken("encoding"),
                    shape190));

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
                    shape283));

            var ShowContinuousExportFailures = Command("ShowContinuousExportFailures", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("continuous-export"),
                    rules.NameDeclarationOrStringLiteral,
                    EToken("failures"),
                    shape283));

            var ShowContinuousExport = Command("ShowContinuousExport", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("continuous-export"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    shape208));

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
                                    shape284)),
                            shape285),
                        separatorParser: EToken(","),
                        secondaryElementParser: null,
                        missingPrimaryElement: null,
                        missingSeparator: null,
                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape285, rules.MissingNameReference(), (SyntaxElement)new CustomNode(shape284, CreateMissingEToken("if_later_than"), rules.MissingStringLiteral())),
                        endOfList: null,
                        oneOrMore: true,
                        allowTrailingSeparator: false,
                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                    EToken(")"),
                    EToken("schema"),
                    EToken("as"),
                    RequiredEToken("json"),
                    shape286));

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
                                        shape284)),
                                shape285),
                            separatorParser: EToken(","),
                            secondaryElementParser: null,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape285, rules.MissingNameReference(), (SyntaxElement)new CustomNode(shape284, CreateMissingEToken("if_later_than"), rules.MissingStringLiteral())),
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape285, rules.MissingNameReference(), (SyntaxElement)new CustomNode(shape284, CreateMissingEToken("if_later_than"), rules.MissingStringLiteral()))))),
                    RequiredEToken(")"),
                    RequiredEToken("schema"),
                    Optional(EToken("details")),
                    shape287));

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
                                        shape8),
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
                            shape166)),
                    shape184));

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
                                    shape8),
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
                            shape220),
                        Custom(
                            EToken("database"),
                            Optional(
                                Custom(
                                    If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                                    shape8)),
                            shape288)),
                    EToken("extents"),
                    Optional(
                        First(
                            Custom(
                                EToken("("),
                                OList(
                                    primaryElementParser: Custom(
                                        rules.AnyGuidLiteralOrString,
                                        shape0),
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
                                shape213),
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
                                            shape275),
                                        separatorParser: EToken("and"),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape275, CreateMissingEToken("tags"), CreateMissingEToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape275, CreateMissingEToken("tags"), CreateMissingEToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral())))),
                                Optional(
                                    Custom(
                                        EToken("with"),
                                        RequiredEToken("("),
                                        RequiredEToken("extentsShowFilteringRuntimePolicy"),
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        RequiredEToken(")"),
                                        shape276)),
                                shape184),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                RequiredEToken("extentsShowFilteringRuntimePolicy"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredEToken(")"),
                                shape276))),
                    shape277));

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
                                    shape8),
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
                            shape220),
                        Custom(
                            EToken("databases"),
                            EToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.DatabaseNameReference,
                                        shape8),
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
                            shape220),
                        Custom(
                            EToken("database"),
                            Optional(
                                Custom(
                                    If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                                    shape8)),
                            shape288)),
                    RequiredEToken("extents"),
                    Optional(
                        First(
                            Custom(
                                EToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.AnyGuidLiteralOrString,
                                            shape0),
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
                                shape213),
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
                                            shape275),
                                        separatorParser: EToken("and"),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape275, CreateMissingEToken("tags"), CreateMissingEToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape275, CreateMissingEToken("tags"), CreateMissingEToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral())))),
                                Optional(
                                    Custom(
                                        EToken("with"),
                                        RequiredEToken("("),
                                        RequiredEToken("extentsShowFilteringRuntimePolicy"),
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        RequiredEToken(")"),
                                        shape276)),
                                shape184),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                RequiredEToken("extentsShowFilteringRuntimePolicy"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredEToken(")"),
                                shape276))),
                    shape278));

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
                            shape289)),
                    shape290));

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
                            shape291)),
                    Optional(
                        First(
                            Custom(
                                EToken("if_later_than"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                shape292),
                            Custom(
                                EToken("script"),
                                Optional(
                                    Custom(
                                        EToken("if_later_than"),
                                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                                        shape292)),
                                shape167))),
                    shape293));

            var ShowDatabaseIngestionMappings = Command("ShowDatabaseIngestionMappings", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    First(
                        EToken("ingestion"),
                        Custom(
                            If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                            EToken("ingestion"),
                            shape291)),
                    Required(
                        First(
                            EToken("mappings"),
                            Custom(
                                EToken("apacheavro", "avro", "csv", "json", "orc", "parquet", "sstream", "w3clogfile"),
                                RequiredEToken("mappings"),
                                shape294)),
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
                                            shape63),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                shape87),
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
                                                    shape63),
                                                separatorParser: EToken(","),
                                                secondaryElementParser: null,
                                                missingPrimaryElement: null,
                                                missingSeparator: null,
                                                missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                                endOfList: null,
                                                oneOrMore: true,
                                                allowTrailingSeparator: false,
                                                producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                            () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                                        RequiredEToken(")"),
                                        shape87)),
                                shape295))),
                    shape204));

            var ShowDatabaseSchemaAsCslScript = Command("ShowDatabaseSchemaAsCslScript", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    First(
                        EToken("schema"),
                        Custom(
                            If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                            EToken("schema"),
                            shape296)),
                    First(
                        EToken("as"),
                        Custom(
                            EToken("if_later_than"),
                            rules.StringLiteral,
                            EToken("as"),
                            shape297)),
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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape298));

            var ShowDatabaseSchemaAsJson = Command("ShowDatabaseSchemaAsJson", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    First(
                        EToken("schema"),
                        Custom(
                            If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                            EToken("schema"),
                            shape296)),
                    First(
                        EToken("as"),
                        Custom(
                            EToken("if_later_than"),
                            rules.StringLiteral,
                            EToken("as"),
                            shape297)),
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
                            shape291)),
                    Optional(
                        First(
                            Custom(
                                EToken("details"),
                                Optional(
                                    Custom(
                                        EToken("if_later_than"),
                                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                                        shape292)),
                                shape167),
                            Custom(
                                EToken("if_later_than"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                shape292))),
                    shape293));

            var DatabaseShardGroupsStatisticsShow = Command("DatabaseShardGroupsStatisticsShow", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    First(
                        EToken("shard-groups").Hide(),
                        Custom(
                            If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                            EToken("shard-groups").Hide(),
                            shape296)),
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
                            shape0)),
                    shape299));

            var ShowDatabaseJournal = Command("ShowDatabaseJournal", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    EToken("journal"),
                    shape199));

            var ShowDatabasePolicyCaching = Command("ShowDatabasePolicyCaching", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    First(
                        EToken("*"),
                        If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference)),
                    EToken("policy"),
                    EToken("caching"),
                    shape300));

            var ShowDatabasePolicyDiagnostics = Command("ShowDatabasePolicyDiagnostics", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    EToken("policy"),
                    EToken("diagnostics"),
                    shape191));

            var ShowDatabasePolicyEncoding = Command("ShowDatabasePolicyEncoding", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    EToken("policy"),
                    EToken("encoding"),
                    shape191));

            var ShowDatabasePolicyExtentTagsRetention = Command("ShowDatabasePolicyExtentTagsRetention", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    First(
                        EToken("*"),
                        If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference)),
                    EToken("policy"),
                    EToken("extent_tags_retention"),
                    shape300));

            var ShowDatabasePolicyHardRetentionViolations = Command("ShowDatabasePolicyHardRetentionViolations", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    EToken("policy"),
                    EToken("hardretention"),
                    RequiredEToken("violations"),
                    shape301));

            var ShowDatabasePolicyIngestionBatching = Command("ShowDatabasePolicyIngestionBatching", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    First(
                        EToken("*"),
                        If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference)),
                    EToken("policy"),
                    EToken("ingestionbatching"),
                    shape300));

            var ShowDatabasePolicyManagedIdentity = Command("ShowDatabasePolicyManagedIdentity", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    EToken("policy"),
                    EToken("managed_identity"),
                    shape191));

            var ShowDatabasePolicyMerge = Command("ShowDatabasePolicyMerge", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    First(
                        EToken("*"),
                        If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference)),
                    EToken("policy"),
                    EToken("merge"),
                    shape300));

            var ShowDatabasePolicyRetention = Command("ShowDatabasePolicyRetention", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    First(
                        EToken("*"),
                        If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference)),
                    EToken("policy"),
                    EToken("retention"),
                    shape300));

            var ShowDatabasePolicySharding = Command("ShowDatabasePolicySharding", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    First(
                        EToken("*"),
                        If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference)),
                    EToken("policy"),
                    EToken("sharding"),
                    shape300));

            var ShowDatabasePolicyShardsGrouping = Command("ShowDatabasePolicyShardsGrouping", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    First(
                        EToken("*"),
                        If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference)),
                    EToken("policy"),
                    EToken("shards_grouping").Hide(),
                    shape300));

            var ShowDatabasePolicySoftRetentionViolations = Command("ShowDatabasePolicySoftRetentionViolations", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    EToken("policy"),
                    EToken("softretention"),
                    RequiredEToken("violations"),
                    shape301));

            var ShowDatabasePolicyStreamingIngestion = Command("ShowDatabasePolicyStreamingIngestion", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    EToken("policy"),
                    RequiredEToken("streamingingestion"),
                    shape191));

            var ShowDatabasePrincipals = Command("ShowDatabasePrincipals", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    EToken("principals"),
                    shape199));

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
                                shape280)),
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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape302));

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
                                        shape0)),
                                shape11),
                            Custom(
                                EToken("operation"),
                                Required(rules.AnyGuidLiteralOrString, rules.MissingValue),
                                shape303)),
                        () => (SyntaxElement)new CustomNode(shape11, CreateMissingEToken("operations"), rules.MissingValue())),
                    shape82));

            var ShowDatabaseSchemaViolations = Command("ShowDatabaseSchemaViolations", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("database"),
                    If(Not(And(EToken("cache", "datastats", "details", "extents", "extent", "identity", "policies", "cslschema", "ingestion", "schema", "shard-groups", "*"))), rules.DatabaseNameReference),
                    RequiredEToken("schema"),
                    RequiredEToken("violations"),
                    shape82));

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
                            shape274)),
                    shape184));

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
                                shape304),
                            Custom(
                                EToken("in"),
                                RequiredEToken("databases"),
                                RequiredEToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.StringLiteral,
                                            shape0),
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
                                        shape304)),
                                shape248))),
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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape305));

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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape184));

            var ShowExtentColumnStorageStats = Command("ShowExtentColumnStorageStats", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("extent"),
                    rules.AnyGuidLiteralOrString,
                    EToken("column"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    RequiredEToken("storage"),
                    RequiredEToken("stats"),
                    shape306));

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
                                            shape0),
                                        Custom(
                                            rules.NameDeclarationOrStringLiteral,
                                            shape46)),
                                    rules.MissingValue),
                                shape307),
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                shape0),
                            Custom(
                                If(Not(EToken("details")), rules.NameDeclarationOrStringLiteral),
                                shape46)),
                        () => (SyntaxElement)new CustomNode(shape307, CreateMissingEToken("details"), rules.MissingValue()))));

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
                            shape202)),
                    shape308));

            var ShowExternalTableCslSchema = Command("ShowExternalTableCslSchema", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("external"),
                    EToken("table"),
                    rules.ExternalTableNameReference,
                    EToken("cslschema"),
                    shape309));

            var ShowExternalTableMappings = Command("ShowExternalTableMappings", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("external"),
                    EToken("table"),
                    rules.ExternalTableNameReference,
                    EToken("mappings"),
                    shape309));

            var ShowExternalTableMapping = Command("ShowExternalTableMapping", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("external"),
                    EToken("table"),
                    rules.ExternalTableNameReference,
                    EToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape218));

            var ShowExternalTablePrincipals = Command("ShowExternalTablePrincipals", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("external"),
                    EToken("table"),
                    rules.ExternalTableNameReference,
                    EToken("principals"),
                    shape310));

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
                                shape280)),
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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape311));

            var ShowExternalTableSchema = Command("ShowExternalTableSchema", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("external"),
                    EToken("table"),
                    rules.ExternalTableNameReference,
                    EToken("schema"),
                    RequiredEToken("as"),
                    RequiredEToken("csl", "json"),
                    shape312));

            var ShowExternalTable = Command("ShowExternalTable", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("external"),
                    RequiredEToken("table"),
                    Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                    shape219));

            var ShowFabric = Command("ShowFabric", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("fabric"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    shape313));

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
                                                shape8)),
                                        RequiredEToken(")"),
                                        shape166)),
                                shape167),
                            Custom(
                                EToken("database"),
                                Required(rules.DatabaseNameReference, rules.MissingNameReference),
                                shape221)),
                        () => (SyntaxElement)new CustomNode(shape167, CreateMissingEToken("databases"), (SyntaxElement)new CustomNode(shape166, CreateMissingEToken("("), SyntaxList<SeparatedElement<SyntaxElement>>.Empty(), CreateMissingEToken(")"))))));

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
                                        shape314)),
                                shape315),
                            Custom(
                                EToken("threshold"),
                                Required(rules.Value, rules.MissingValue),
                                shape314))),
                    shape316));

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
                    shape317));

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
                                shape280)),
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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape318));

            var ShowFunctionSchemaAsJson = Command("ShowFunctionSchemaAsJson", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("function"),
                    rules.DatabaseFunctionNameReference,
                    EToken("schema"),
                    RequiredEToken("as"),
                    RequiredEToken("json"),
                    shape319));

            var ShowFunction = Command("ShowFunction", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("function"),
                    Required(rules.DatabaseFunctionNameReference, rules.MissingNameReference),
                    shape320));

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
                            shape321)),
                    shape293));

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
                                shape294)),
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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape293));

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
                                    shape17),
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
                            shape322),
                        Custom(
                            EToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.MaterializedViewNameReference,
                                        shape17),
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
                            shape322),
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
                    shape170));

            var ShowMaterializedViewDetails = Command("ShowMaterializedViewDetails", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("details"),
                    shape170));

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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape269));

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
                                            shape0),
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
                                shape213),
                            EToken("hot"))),
                    shape323));

            var ShowMaterializedViewFailures = Command("ShowMaterializedViewFailures", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("failures"),
                    shape171));

            var ShowMaterializedViewPolicyCaching = Command("ShowMaterializedViewPolicyCaching", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("policy"),
                    EToken("caching"),
                    shape125));

            var ShowMaterializedViewPolicyMerge = Command("ShowMaterializedViewPolicyMerge", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("policy"),
                    EToken("merge"),
                    shape125));

            var ShowMaterializedViewPolicyPartitioning = Command("ShowMaterializedViewPolicyPartitioning", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("policy"),
                    EToken("partitioning"),
                    shape125));

            var ShowMaterializedViewPolicyRetention = Command("ShowMaterializedViewPolicyRetention", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("policy"),
                    EToken("retention"),
                    shape125));

            var ShowMaterializedViewPolicyRowLevelSecurity = Command("ShowMaterializedViewPolicyRowLevelSecurity", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("policy"),
                    RequiredEToken("row_level_security"),
                    shape125));

            var ShowMaterializedViewPrincipals = Command("ShowMaterializedViewPrincipals", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("principals"),
                    shape170));

            var ShowMaterializedViewSchemaAsJson = Command("ShowMaterializedViewSchemaAsJson", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("schema"),
                    RequiredEToken("as"),
                    RequiredEToken("json"),
                    shape37));

            var ShowMaterializedViewStatistics = Command("ShowMaterializedViewStatistics", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    rules.MaterializedViewNameReference,
                    EToken("statistics"),
                    shape171));

            var ShowMaterializedView = Command("ShowMaterializedView", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    shape226));

            var ShowMemory = Command("ShowMemory", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("memory"),
                    Optional(EToken("details")),
                    shape184));

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
                                            shape0),
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
                                shape245),
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                shape0))),
                    shape184));

            var ShowOperationDetails = Command("ShowOperationDetails", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("operation"),
                    Required(rules.AnyGuidLiteralOrString, rules.MissingValue),
                    RequiredEToken("details"),
                    shape324));

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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape184));

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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape293));

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
                                shape280)),
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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape293));

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
                            shape84),
                        () => (SyntaxElement)new CustomNode(shape84, CreateMissingEToken("<|"), rules.MissingExpression())),
                    shape325));

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
                                            shape48),
                                        separatorParser: EToken(","),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape48, CreateMissingEToken("reconstructCsl"), CreateMissingEToken("="), rules.MissingValue()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape48, CreateMissingEToken("reconstructCsl"), CreateMissingEToken("="), rules.MissingValue())))),
                                RequiredEToken(")"),
                                RequiredEToken("<|"))),
                        () => CreateMissingEToken("<|")),
                    Required(rules.CommandInput, rules.MissingExpression),
                    shape326));

            var ShowQueryCallTree = Command("ShowQueryCallTree", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("query"),
                    RequiredEToken("call-tree"),
                    Required(
                        Custom(
                            EToken("<|"),
                            Required(rules.CommandInput, rules.MissingExpression),
                            shape84),
                        () => (SyntaxElement)new CustomNode(shape84, CreateMissingEToken("<|"), rules.MissingExpression())),
                    shape327));

            var ShowRequestSupport = Command("ShowRequestSupport", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("request_support"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape328));

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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape329));

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
                    shape330));

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
                                        shape1)),
                                () => CreateMissingEToken("*")))),
                    shape293));

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
                    shape184));

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
                                    shape86)),
                            RequiredEToken(")"),
                            shape87)),
                    shape184));

            var StoredQueryResultShowSchema = Command("StoredQueryResultShowSchema", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("stored_query_result"),
                    Required(rules.NameDeclarationOrStringLiteral, rules.MissingNameDeclaration),
                    RequiredEToken("schema"),
                    shape331));

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
                    shape332));

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
                                    shape19),
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
                            shape333),
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
                                    shape19),
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
                            shape334)),
                    shape184));

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
                                    shape19),
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
                            shape335),
                        Custom(
                            EToken("table"),
                            If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                            shape157)),
                    EToken("extents"),
                    Optional(
                        First(
                            Custom(
                                EToken("("),
                                OList(
                                    primaryElementParser: Custom(
                                        rules.AnyGuidLiteralOrString,
                                        shape0),
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
                                shape213),
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
                                            shape275),
                                        separatorParser: EToken("and"),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape275, CreateMissingEToken("tags"), CreateMissingEToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape275, CreateMissingEToken("tags"), CreateMissingEToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral())))),
                                Optional(
                                    Custom(
                                        EToken("with"),
                                        RequiredEToken("("),
                                        RequiredEToken("extentsShowFilteringRuntimePolicy"),
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        RequiredEToken(")"),
                                        shape276)),
                                shape184),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                RequiredEToken("extentsShowFilteringRuntimePolicy"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredEToken(")"),
                                shape276))),
                    shape277));

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
                                    shape19),
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
                            shape335),
                        Custom(
                            EToken("table"),
                            If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                            shape157)),
                    EToken("extents"),
                    Optional(
                        First(
                            Custom(
                                EToken("("),
                                Required(
                                    OList(
                                        primaryElementParser: Custom(
                                            rules.AnyGuidLiteralOrString,
                                            shape0),
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
                                shape213),
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
                                            shape275),
                                        separatorParser: EToken("and"),
                                        secondaryElementParser: null,
                                        missingPrimaryElement: null,
                                        missingSeparator: null,
                                        missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape275, CreateMissingEToken("tags"), CreateMissingEToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral()),
                                        endOfList: null,
                                        oneOrMore: true,
                                        allowTrailingSeparator: false,
                                        producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                    () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape275, CreateMissingEToken("tags"), CreateMissingEToken("Expected !contains,!has,contains,has"), rules.MissingStringLiteral())))),
                                Optional(
                                    Custom(
                                        EToken("with"),
                                        RequiredEToken("("),
                                        RequiredEToken("extentsShowFilteringRuntimePolicy"),
                                        RequiredEToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        RequiredEToken(")"),
                                        shape276)),
                                shape184),
                            Custom(
                                EToken("with"),
                                RequiredEToken("("),
                                RequiredEToken("extentsShowFilteringRuntimePolicy"),
                                RequiredEToken("="),
                                Required(rules.Value, rules.MissingValue),
                                RequiredEToken(")"),
                                shape276))),
                    shape278));

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
                                    shape19),
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
                            shape335),
                        Custom(
                            EToken("tables"),
                            RequiredEToken("("),
                            Required(
                                OList(
                                    primaryElementParser: Custom(
                                        rules.TableNameReference,
                                        shape19),
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
                            shape335),
                        Custom(
                            EToken("table"),
                            Required(If(Not(And(EToken("*", "usage"))), rules.TableNameReference), rules.MissingNameReference),
                            shape157)),
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
                            shape336)),
                    shape204));

            var ShowTablePolicyAutoDelete = Command("ShowTablePolicyAutoDelete", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("policy"),
                    EToken("auto_delete"),
                    shape195));

            var ShowTablePolicyCaching = Command("ShowTablePolicyCaching", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("policy"),
                    EToken("caching"),
                    shape195));

            var ShowTablePolicyEncoding = Command("ShowTablePolicyEncoding", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("policy"),
                    EToken("encoding"),
                    shape195));

            var ShowTablePolicyExtentTagsRetention = Command("ShowTablePolicyExtentTagsRetention", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("policy"),
                    EToken("extent_tags_retention"),
                    shape195));

            var ShowTablePolicyIngestionBatching = Command("ShowTablePolicyIngestionBatching", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("policy"),
                    EToken("ingestionbatching"),
                    shape195));

            var ShowTablePolicyMerge = Command("ShowTablePolicyMerge", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("policy"),
                    EToken("merge"),
                    shape195));

            var ShowTablePolicyPartitioning = Command("ShowTablePolicyPartitioning", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("policy"),
                    EToken("partitioning"),
                    shape195));

            var ShowTablePolicyRestrictedViewAccess = Command("ShowTablePolicyRestrictedViewAccess", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("policy"),
                    EToken("restricted_view_access"),
                    shape195));

            var ShowTablePolicyRetention = Command("ShowTablePolicyRetention", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("policy"),
                    EToken("retention"),
                    shape195));

            var ShowTablePolicyRowOrder = Command("ShowTablePolicyRowOrder", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("policy"),
                    EToken("roworder"),
                    shape195));

            var ShowTablePolicySharding = Command("ShowTablePolicySharding", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("policy"),
                    EToken("sharding"),
                    shape195));

            var ShowTablePolicyStreamingIngestion = Command("ShowTablePolicyStreamingIngestion", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("policy"),
                    EToken("streamingingestion"),
                    shape195));

            var ShowTablePolicyUpdate = Command("ShowTablePolicyUpdate", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("policy"),
                    RequiredEToken("update"),
                    shape195));

            var ShowTableRowStoreReferences = Command("ShowTableRowStoreReferences", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("rowstore_references"),
                    shape172));

            var ShowTableRowStoreSealInfo = Command("ShowTableRowStoreSealInfo", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    EToken("rowstore_sealinfo"),
                    shape337));

            var ShowTableRowStores = Command("ShowTableRowStores", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.DatabaseTableNameReference),
                    RequiredEToken("rowstores"),
                    shape337));

            var ShowTableColumnsClassification = Command("ShowTableColumnsClassification", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                    EToken("columns"),
                    RequiredEToken("classification"),
                    shape195));

            var ShowTableColumnStatitics = Command("ShowTableColumnStatitics", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                    EToken("column"),
                    RequiredEToken("statistics"),
                    shape195));

            var ShowTableCslSchema = Command("ShowTableCslSchema", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                    EToken("cslschema"),
                    shape172));

            var ShowTableDetails = Command("ShowTableDetails", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                    EToken("details"),
                    shape172));

            var ShowTableDimensions = Command("ShowTableDimensions", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                    EToken("dimensions"),
                    shape172));

            var ShowTableIngestionMappings = Command("ShowTableIngestionMappings", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                    EToken("ingestion"),
                    EToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    EToken("mappings"),
                    shape338));

            var ShowTableIngestionMapping = Command("ShowTableIngestionMapping", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                    EToken("ingestion"),
                    RequiredEToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredEToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape232));

            var ShowTablePolicyIngestionTime = Command("ShowTablePolicyIngestionTime", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                    EToken("policy"),
                    EToken("ingestiontime"),
                    shape195));

            var ShowTablePolicyRowLevelSecurity = Command("ShowTablePolicyRowLevelSecurity", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                    EToken("policy"),
                    RequiredEToken("row_level_security"),
                    shape195));

            var ShowTablePrincipals = Command("ShowTablePrincipals", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                    EToken("principals"),
                    shape172));

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
                                shape280)),
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
                                        shape63),
                                    separatorParser: EToken(","),
                                    secondaryElementParser: null,
                                    missingPrimaryElement: null,
                                    missingSeparator: null,
                                    missingSecondaryElement: () => (SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue()),
                                    endOfList: null,
                                    oneOrMore: true,
                                    allowTrailingSeparator: false,
                                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                                () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape63, rules.MissingNameDeclaration(), CreateMissingEToken("="), rules.MissingValue())))),
                            RequiredEToken(")"),
                            shape87)),
                    shape339));

            var ShowTableSchemaAsJson = Command("ShowTableSchemaAsJson", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                    EToken("schema"),
                    RequiredEToken("as"),
                    RequiredEToken("json"),
                    shape40));

            var TableShardGroupsShow = Command("TableShardGroupsShow", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    If(Not(And(EToken("*", "usage"))), rules.TableNameReference),
                    EToken("shard-groups").Hide(),
                    shape172));

            var ShowTable = Command("ShowTable", 
                Custom(
                    EToken("show", CompletionKind.CommandPrefix),
                    EToken("table"),
                    Required(If(Not(And(EToken("*", "usage"))), rules.TableNameReference), rules.MissingNameReference),
                    shape340));

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
                    shape341));

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
                                shape342),
                            EToken("version")),
                        () => (SyntaxElement)new CustomNode(shape342, CreateMissingEToken("as"), rules.MissingNameDeclaration(), CreateMissingEToken("version"))),
                    RequiredEToken("="),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
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

