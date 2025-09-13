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

        internal override CommandParserInfo[] CreateCommandParsers(PredefinedRuleParsers rules)
        {
            var shape0 = CD("PropertyName", CompletionHint.None);
            var shape1 = CD("PropertyValue", CompletionHint.Literal);
            var shape2 = new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)};
            var shape3 = new [] {CD(), CD(), CD(CompletionHint.None), CD()};
            var shape4 = new [] {CD(), CD(), CD(), CD(isOptional: true)};
            var shape5 = CD("DatabaseName", CompletionHint.None);
            var shape6 = new [] {CD(), CD(CompletionHint.None), CD()};
            var shape7 = new [] {CD(), CD(isOptional: true), CD(), CD(isOptional: true), CD()};
            var shape8 = CD("Path", CompletionHint.Literal);
            var shape9 = CD("DatabaseName", CompletionHint.Database);
            var shape10 = CD("Version", CompletionHint.Literal);
            var shape11 = new [] {CD(), CD("Version", CompletionHint.Literal)};
            var shape12 = new [] {CD("DatabaseName", CompletionHint.Database), CD()};
            var shape13 = CD("Script");
            var shape14 = new [] {CD(), CD(), CD(CompletionHint.Database, isOptional: true), CD(), CD(isOptional: true), CD(isOptional: true)};
            var shape15 = CD("Details");
            var shape16 = new [] {CD("DatabaseName", CompletionHint.Database), CD(isOptional: true)};
            var shape17 = new [] {CD(), CD(CompletionHint.Database), CD(), CD()};
            var shape18 = new [] {CD(), CD(), CD(), CD(isOptional: true), CD(), CD(), CD(isOptional: true)};
            var shape19 = CD("MappingKind");
            var shape20 = CD("MappingName", CompletionHint.Literal);
            var shape21 = CD("MappingFormat", CompletionHint.Literal);
            var shape22 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.None), CD(), CD("MappingKind"), CD(), CD("MappingName", CompletionHint.Literal), CD("MappingFormat", CompletionHint.Literal)};
            var shape23 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD("MappingKind"), CD(), CD("MappingName", CompletionHint.Literal), CD("MappingFormat", CompletionHint.Literal)};
            var shape24 = CD("databaseName", CompletionHint.Database);
            var shape25 = new [] {CD(), CD(), CD(isOptional: true)};
            var shape26 = CD("TableName", CompletionHint.Table);
            var shape27 = new [] {CD(), CD(CompletionHint.Table), CD()};
            var shape28 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD()};
            var shape29 = CD("TableName", CompletionHint.None);
            var shape30 = CD("PropertyName");
            var shape31 = new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)};
            var shape32 = CD("ColumnName", CompletionHint.None);
            var shape33 = CD("ColumnType");
            var shape34 = new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")};
            var shape35 = new [] {CD(), CD(), CD("TableName", CompletionHint.None), CD(), CD(isOptional: true)};
            var shape36 = new [] {CD("TableName", CompletionHint.None), CD()};
            var shape37 = new [] {CD(), CD(), CD(CompletionHint.None), CD(isOptional: true)};
            var shape38 = new [] {CD(), CD(), CD(CompletionHint.Table), CD(), CD(isOptional: true)};
            var shape39 = CD("NewTableName", CompletionHint.None);
            var shape40 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("MappingKind"), CD(), CD("MappingName", CompletionHint.Literal), CD("MappingFormat", CompletionHint.Literal), CD(isOptional: true)};
            var shape41 = CD("NewColumnName", CompletionHint.None);
            var shape42 = CD("ColumnName", CompletionHint.Column);
            var shape43 = new [] {CD(), CD(), CD(CompletionHint.None)};
            var shape44 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(CompletionHint.Column), CD()};
            var shape45 = CD("DocString", CompletionHint.Literal);
            var shape46 = new [] {CD("ColumnName", CompletionHint.Column), CD(), CD("DocString", CompletionHint.Literal)};
            var shape47 = CD("Value", CompletionHint.Literal);
            var shape48 = new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)};
            var shape49 = CD("FunctionName", CompletionHint.Function);
            var shape50 = CD("FunctionName", CompletionHint.None);
            var shape51 = new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD(isOptional: true)};
            var shape52 = CD("Documentation", CompletionHint.Literal);
            var shape53 = CD("Folder", CompletionHint.Literal);
            var shape54 = CD("ExternalTableName", CompletionHint.ExternalTable);
            var shape55 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD()};
            var shape56 = CD("PartitionType");
            var shape57 = CD("PartitionFunction");
            var shape58 = CD("DateTimeColumn", CompletionHint.None);
            var shape59 = new [] {CD("PartitionType"), CD(isOptional: true)};
            var shape60 = CD("StringColumn", CompletionHint.None);
            var shape61 = CD("PartitionName", CompletionHint.None);
            var shape62 = CD("PathSeparator", CompletionHint.Literal);
            var shape63 = CD("ExternalTableName", CompletionHint.None);
            var shape64 = CD("TableKind");
            var shape65 = CD("StorageConnectionString", CompletionHint.Literal);
            var shape66 = new [] {CD(), CD("StringColumn", CompletionHint.None)};
            var shape67 = CD("BinValue", CompletionHint.Literal);
            var shape68 = new [] {CD("PartitionFunction"), CD(), CD("DateTimeColumn", CompletionHint.None), CD(), CD("BinValue", CompletionHint.Literal), CD()};
            var shape69 = new [] {CD("PartitionFunction"), CD(), CD("DateTimeColumn", CompletionHint.None), CD()};
            var shape70 = CD("HashMod", CompletionHint.Literal);
            var shape71 = new [] {CD("PartitionType"), CD(), CD("PartitionFunction"), CD(), CD("StringColumn", CompletionHint.None), CD(), CD("HashMod", CompletionHint.Literal), CD()};
            var shape72 = new [] {CD("PartitionName", CompletionHint.None), CD(), CD()};
            var shape73 = CD("DateTimeFormat", CompletionHint.Literal);
            var shape74 = new [] {CD(), CD(), CD("DateTimeFormat", CompletionHint.Literal), CD(), CD("PartitionName", CompletionHint.None), CD()};
            var shape75 = new [] {CD(CompletionHint.None), CD(CompletionHint.Literal, isOptional: true)};
            var shape76 = new [] {CD(), CD(), CD(), CD(CompletionHint.Literal, isOptional: true), CD(CompletionHint.None), CD()};
            var shape77 = new [] {CD(), CD(), CD(), CD(CompletionHint.None), CD(), CD(isOptional: true)};
            var shape78 = CD("CatalogExpression", CompletionHint.Literal);
            var shape79 = new [] {CD(), CD(), CD("CatalogExpression", CompletionHint.Literal)};
            var shape80 = CD("DataFormatKind");
            var shape81 = new [] {CD(), CD(), CD("ExternalTableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD(), CD(), CD(), CD("TableKind"), CD(isOptional: true), CD(isOptional: true), CD(), CD(), CD("DataFormatKind"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)};
            var shape82 = new [] {CD(), CD(), CD("ExternalTableName", CompletionHint.None), CD(isOptional: true), CD(), CD(), CD("TableKind"), CD(), CD("StorageConnectionString", CompletionHint.Literal), CD(), CD(isOptional: true)};
            var shape83 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD(), CD(), CD(), CD("TableKind"), CD(isOptional: true), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)};
            var shape84 = CD("externalTableName", CompletionHint.ExternalTable);
            var shape85 = CD("principal", CompletionHint.Literal);
            var shape86 = CD("notes", CompletionHint.Literal);
            var shape87 = new [] {CD(), CD(), CD(), CD("externalTableName", CompletionHint.ExternalTable), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true), CD(CompletionHint.Literal, isOptional: true)};
            var shape88 = CD("tableName", CompletionHint.ExternalTable);
            var shape89 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD("MappingName", CompletionHint.Literal), CD("MappingFormat", CompletionHint.Literal)};
            var shape90 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD("MappingName", CompletionHint.Literal)};
            var shape91 = CD("WorkloadGroupName", CompletionHint.None);
            var shape92 = CD("WorkloadGroup", CompletionHint.Literal);
            var shape93 = new [] {CD(), CD(), CD("WorkloadGroupName", CompletionHint.None), CD("WorkloadGroup", CompletionHint.Literal)};
            var shape94 = new [] {CD(), CD(), CD(CompletionHint.Database, isOptional: true), CD(), CD()};
            var shape95 = CD("Timespan", CompletionHint.Literal);
            var shape96 = new [] {CD(), CD(), CD("Timespan", CompletionHint.Literal)};
            var shape97 = CD("HotData", CompletionHint.Literal);
            var shape98 = CD("HotIndex", CompletionHint.Literal);
            var shape99 = new [] {CD(), CD(), CD("HotData", CompletionHint.Literal), CD(), CD(), CD("HotIndex", CompletionHint.Literal)};
            var shape100 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD()};
            var shape101 = CD("MaterializedViewName", CompletionHint.MaterializedView);
            var shape102 = CD("ModelName", CompletionHint.GraphModel);
            var shape103 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()};
            var shape104 = new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD()};
            var shape105 = new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD()};
            var shape106 = new [] {CD(), CD(), CD("ModelName", CompletionHint.GraphModel), CD(), CD()};
            var shape107 = CD("RetentionPolicy", CompletionHint.Literal);
            var shape108 = CD("RecoverabilityValue");
            var shape109 = new [] {CD(), CD(), CD("RecoverabilityValue")};
            var shape110 = CD("SoftDeleteValue", CompletionHint.Literal);
            var shape111 = new [] {CD(), CD(), CD("SoftDeleteValue", CompletionHint.Literal), CD(isOptional: true)};
            var shape112 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD()};
            var shape113 = new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(), CD()};
            var shape114 = CD("Query", CompletionHint.Literal);
            var shape115 = new [] {CD("ColumnName", CompletionHint.Column), CD()};
            var shape116 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD(CompletionHint.Column), CD()};
            var shape117 = CD("UpdatePolicy", CompletionHint.Literal);
            var shape118 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("UpdatePolicy", CompletionHint.Literal), CD(isOptional: true)};
            var shape119 = CD("IngestionBatchingPolicy", CompletionHint.Literal);
            var shape120 = new [] {CD(), CD(), CD(), CD(), CD("IngestionBatchingPolicy", CompletionHint.Literal)};
            var shape121 = new [] {CD(), CD(), CD(CompletionHint.Database, isOptional: true), CD(), CD(), CD("IngestionBatchingPolicy", CompletionHint.Literal)};
            var shape122 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("IngestionBatchingPolicy", CompletionHint.Literal)};
            var shape123 = CD("EncodingPolicy", CompletionHint.Literal);
            var shape124 = new [] {CD(), CD(), CD(CompletionHint.Database, isOptional: true), CD(), CD(), CD("EncodingPolicy", CompletionHint.Literal)};
            var shape125 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("EncodingPolicy", CompletionHint.Literal)};
            var shape126 = new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD(), CD("EncodingPolicy", CompletionHint.Literal)};
            var shape127 = CD("MergePolicy", CompletionHint.Literal);
            var shape128 = new [] {CD(), CD(), CD(CompletionHint.Database, isOptional: true), CD(), CD(), CD("MergePolicy", CompletionHint.Literal)};
            var shape129 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("MergePolicy", CompletionHint.Literal)};
            var shape130 = CD("Policy", CompletionHint.Literal);
            var shape131 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD(), CD("Policy", CompletionHint.Literal)};
            var shape132 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD()};
            var shape133 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD(), CD()};
            var shape134 = new [] {CD("PartitionType"), CD(), CD("StringColumn", CompletionHint.None)};
            var shape135 = new [] {CD("PartitionType"), CD(), CD()};
            var shape136 = CD("KindType");
            var shape137 = new [] {CD(), CD(CompletionHint.Literal), CD()};
            var shape138 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(isOptional: true), CD(), CD(), CD("KindType"), CD(isOptional: true), CD(isOptional: true)};
            var shape139 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("Policy", CompletionHint.Literal)};
            var shape140 = CD("TemplateName", CompletionHint.None);
            var shape141 = new [] {CD(), CD(), CD("KindType")};
            var shape142 = CD("ConnectionString", CompletionHint.Literal);
            var shape143 = new [] {CD(), CD("ConnectionString", CompletionHint.Literal), CD()};
            var shape144 = new [] {CD(), CD(), CD("TemplateName", CompletionHint.None), CD(isOptional: true), CD(isOptional: true), CD(isOptional: true)};
            var shape145 = new [] {CD(), CD(), CD("TemplateName", CompletionHint.None)};
            var shape146 = new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD("Policy", CompletionHint.Literal)};
            var shape147 = new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(), CD()};
            var shape148 = CD("RowStorePolicy", CompletionHint.Literal);
            var shape149 = new [] {CD(), CD(), CD(), CD(), CD("RowStorePolicy", CompletionHint.Literal)};
            var shape150 = CD("ShardingPolicy", CompletionHint.Literal);
            var shape151 = new [] {CD(), CD(), CD(CompletionHint.Database, isOptional: true), CD(), CD(), CD("ShardingPolicy", CompletionHint.Literal)};
            var shape152 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("ShardingPolicy", CompletionHint.Literal)};
            var shape153 = CD("policy", CompletionHint.Literal);
            var shape154 = new [] {CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal)};
            var shape155 = CD("ShardsGroupingPolicy", CompletionHint.Literal);
            var shape156 = new [] {CD(), CD(), CD(CompletionHint.Database, isOptional: true), CD(), CD(), CD("ShardsGroupingPolicy", CompletionHint.Literal)};
            var shape157 = new [] {CD(), CD(), CD(CompletionHint.Database, isOptional: true), CD(), CD(), CD(CompletionHint.Literal)};
            var shape158 = CD("StreamingIngestionPolicy", CompletionHint.Literal);
            var shape159 = CD("Status");
            var shape160 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(CompletionHint.Literal)};
            var shape161 = CD("ManagedIdentityPolicy", CompletionHint.Literal);
            var shape162 = new [] {CD(), CD(), CD(CompletionHint.Database, isOptional: true), CD(), CD(), CD("ManagedIdentityPolicy", CompletionHint.Literal)};
            var shape163 = new [] {CD(), CD(), CD(), CD(), CD("ManagedIdentityPolicy", CompletionHint.Literal)};
            var shape164 = new [] {CD(), CD(), CD(), CD(), CD("Policy", CompletionHint.Literal)};
            var shape165 = CD("Query", CompletionHint.NonScalar);
            var shape166 = CD("PolicyName", CompletionHint.Literal);
            var shape167 = new [] {CD(), CD(), CD(CompletionHint.Database, isOptional: true), CD(), CD(), CD("PolicyName", CompletionHint.Literal)};
            var shape168 = new [] {CD(), CD(), CD(), CD(), CD("PolicyName", CompletionHint.Literal)};
            var shape169 = CD("ExtentTagsRetentionPolicy", CompletionHint.Literal);
            var shape170 = CD("Principal", CompletionHint.Literal);
            var shape171 = new [] {CD("Principal", CompletionHint.Literal), CD()};
            var shape172 = new [] {CD(), CD(), CD(), CD(), CD(isOptional: true)};
            var shape173 = CD("Role");
            var shape174 = CD("SkipResults");
            var shape175 = CD("Notes", CompletionHint.Literal);
            var shape176 = new [] {CD(), CD(), CD("Role"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true), CD(CompletionHint.Literal, isOptional: true)};
            var shape177 = new [] {CD(), CD(isOptional: true)};
            var shape178 = new [] {CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true), CD(CompletionHint.Literal, isOptional: true)};
            var shape179 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD("Role"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true), CD(CompletionHint.Literal, isOptional: true)};
            var shape180 = new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD("Role"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true), CD(CompletionHint.Literal, isOptional: true)};
            var shape181 = CD("AppName", CompletionHint.Literal);
            var shape182 = new [] {CD(), CD("AppName", CompletionHint.Literal)};
            var shape183 = CD("UserName", CompletionHint.Literal);
            var shape184 = new [] {CD(), CD("UserName", CompletionHint.Literal)};
            var shape185 = CD("Data", CompletionHint.None);
            var shape186 = CD("QueryOrCommand", CompletionHint.NonScalar);
            var shape187 = new [] {CD(), CD(isOptional: true), CD("TableName", CompletionHint.None), CD(isOptional: true), CD(), CD("QueryOrCommand", CompletionHint.NonScalar)};
            var shape188 = CD("ContinuousExportName", CompletionHint.None);
            var shape189 = new [] {CD(), CD(), CD("ContinuousExportName", CompletionHint.None), CD()};
            var shape190 = new [] {CD(), CD(), CD("ContinuousExportName", CompletionHint.None)};
            var shape191 = CD("MaterializedViewName", CompletionHint.None);
            var shape192 = new [] {CD(), CD(), CD(isOptional: true), CD()};
            var shape193 = new [] {CD(), CD(), CD(isOptional: true), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD(CompletionHint.Table), CD()};
            var shape194 = new [] {CD(), CD(), CD(isOptional: true), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD(CompletionHint.MaterializedView), CD()};
            var shape195 = new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView)};
            var shape196 = new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD()};
            var shape197 = new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD()};
            var shape198 = CD("Scope");
            var shape199 = new [] {CD(), CD(), CD(), CD(), CD("Scope"), CD()};
            var shape200 = CD("OperationId", CompletionHint.Literal);
            var shape201 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD()};
            var shape202 = CD("ExtentId", CompletionHint.Literal);
            var shape203 = CD("Tag", CompletionHint.Literal);
            var shape204 = new [] {CD(), CD(), CD("Tag", CompletionHint.Literal)};
            var shape205 = new [] {CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()};
            var shape206 = new [] {CD(), CD(), CD(), CD(isOptional: true), CD(isOptional: true), CD(isOptional: true), CD(isOptional: true)};
            var shape207 = new [] {CD(), CD(CompletionHint.Database, isOptional: true)};
            var shape208 = new [] {CD(), CD(), CD(CompletionHint.Database), CD()};
            var shape209 = new [] {CD(), CD(), CD(), CD(isOptional: true), CD(isOptional: true), CD(), CD(isOptional: true), CD(isOptional: true)};
            var shape210 = new [] {CD(), CD(), CD(CompletionHint.Database, isOptional: true), CD(), CD(), CD()};
            var shape211 = new [] {CD(), CD("TableName", CompletionHint.Table)};
            var shape212 = new [] {CD(), CD(), CD(CompletionHint.Table), CD()};
            var shape213 = CD("ShardsGroupId", CompletionHint.Literal);
            var shape214 = CD("GUID", CompletionHint.Literal);
            var shape215 = CD("SourceTableName", CompletionHint.Table);
            var shape216 = CD("DestinationTableName", CompletionHint.Table);
            var shape217 = new [] {CD(), CD("Query", CompletionHint.NonScalar)};
            var shape218 = CD("LimitCount", CompletionHint.Literal);
            var shape219 = new [] {CD(), CD("LimitCount", CompletionHint.Literal)};
            var shape220 = CD("TrimSize", CompletionHint.Literal);
            var shape221 = new [] {CD(), CD(), CD(), CD("TrimSize", CompletionHint.Literal), CD()};
            var shape222 = new [] {CD(), CD(), CD(isOptional: true), CD(isOptional: true)};
            var shape223 = CD("d1", CompletionHint.Literal);
            var shape224 = CD("d2", CompletionHint.Literal);
            var shape225 = CD("Older", CompletionHint.Literal);
            var shape226 = CD("StoredQueryResultName", CompletionHint.None);
            var shape227 = new [] {CD(), CD(), CD(CompletionHint.GraphModel), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(CompletionHint.Literal, isOptional: true)};
            var shape228 = new [] {CD(), CD(), CD(CompletionHint.GraphModel)};
            var shape229 = new [] {CD(), CD(), CD(CompletionHint.GraphModel), CD()};
            var shape230 = CD("serviceType", CompletionHint.Literal);
            var shape231 = new [] {CD(), CD(), CD(), CD("serviceType", CompletionHint.Literal), CD()};
            var shape232 = CD("clusterName", CompletionHint.Literal);
            var shape233 = CD("databaseName", CompletionHint.Literal);
            var shape234 = CD("EntityGroupName", CompletionHint.None);
            var shape235 = new [] {CD(), CD(), CD("clusterName", CompletionHint.Literal), CD(), CD(), CD(), CD(), CD("databaseName", CompletionHint.Literal), CD()};
            var shape236 = new [] {CD(), CD(), CD("databaseName", CompletionHint.Literal), CD()};
            var shape237 = CD("EntityGroupName", CompletionHint.EntityGroup);
            var shape238 = new [] {CD(), CD(), CD("EntityGroupName", CompletionHint.EntityGroup)};
            var shape239 = CD("container", CompletionHint.Literal);
            var shape240 = CD("thumbprint", CompletionHint.Literal);
            var shape241 = CD("tableName", CompletionHint.None);
            var shape242 = CD("tableName", CompletionHint.Table);
            var shape243 = CD("eid", CompletionHint.Literal);
            var shape244 = new [] {CD(), CD(CompletionHint.NonScalar)};
            var shape245 = CD("csl");
            var shape246 = CD("t", CompletionHint.Literal);
            var shape247 = new [] {CD(), CD(isOptional: true), CD(), CD("tableName", CompletionHint.Table), CD(), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true), CD("csl")};
            var shape248 = CD("modificationKind");
            var shape249 = new [] {CD(), CD(), CD("modificationKind")};
            var shape250 = CD("leaderClusterMetadataPath", CompletionHint.Literal);
            var shape251 = CD("dbName", CompletionHint.Database);
            var shape252 = CD("operationRole");
            var shape253 = new [] {CD(), CD("leaderClusterMetadataPath", CompletionHint.Literal)};
            var shape254 = new [] {CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal)};
            var shape255 = CD("p", CompletionHint.Literal);
            var shape256 = new [] {CD(), CD(), CD("p", CompletionHint.Literal)};
            var shape257 = new [] {CD(isOptional: true), CD()};
            var shape258 = new [] {CD(), CD("databaseName", CompletionHint.Database)};
            var shape259 = new [] {CD(), CD(CompletionHint.Database), CD()};
            var shape260 = CD("name", CompletionHint.None);
            var shape261 = CD("hotDataToken", CompletionHint.Literal);
            var shape262 = CD("hotIndexToken", CompletionHint.Literal);
            var shape263 = new [] {CD(), CD(), CD("hotDataToken", CompletionHint.Literal), CD(), CD(), CD("hotIndexToken", CompletionHint.Literal)};
            var shape264 = CD("hotToken", CompletionHint.Literal);
            var shape265 = new [] {CD(), CD(), CD("hotToken", CompletionHint.Literal)};
            var shape266 = CD("hotWindows", isOptional: true);
            var shape267 = CD("name", CompletionHint.Table);
            var shape268 = new [] {CD(), CD("name", CompletionHint.Table)};
            var shape269 = CD("name", CompletionHint.MaterializedView);
            var shape270 = new [] {CD(), CD("name", CompletionHint.MaterializedView)};
            var shape271 = new [] {CD(), CD(CompletionHint.Literal), CD(), CD(CompletionHint.Literal, isOptional: true)};
            var shape272 = CD("materializedViewName", CompletionHint.MaterializedView);
            var shape273 = new [] {CD(), CD(), CD("materializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(CompletionHint.Literal, isOptional: true)};
            var shape274 = CD("viewName", CompletionHint.MaterializedView);
            var shape275 = new [] {CD(), CD(), CD("viewName", CompletionHint.MaterializedView), CD()};
            var shape276 = new [] {CD(), CD(), CD("viewName", CompletionHint.MaterializedView), CD(), CD(isOptional: true)};
            var shape277 = CD("obj", CompletionHint.Literal);
            var shape278 = CD("name", CompletionHint.Literal);
            var shape279 = CD("rowStoreName", CompletionHint.None);
            var shape280 = new [] {CD(), CD(isOptional: true), CD(), CD(isOptional: true)};
            var shape281 = CD("rowStoreKey", CompletionHint.Literal);
            var shape282 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD("rowStoreKey", CompletionHint.Literal), CD(isOptional: true)};
            var shape283 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD("rowStoreName", CompletionHint.None), CD(isOptional: true)};
            var shape284 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD(), CD(isOptional: true)};
            var shape285 = new [] {CD(), CD(), CD("tableName", CompletionHint.Table), CD()};
            var shape286 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(isOptional: true)};
            var shape287 = CD("containerId", CompletionHint.Literal);
            var shape288 = CD("queryText");
            var shape289 = new [] {CD(), CD(isOptional: true), CD(), CD()};
            var shape290 = new [] {CD(), CD(CompletionHint.Table)};

            var fragment0 = Custom(
                    Token("with"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.NameDeclaration,
                            Token("="),
                            rules.Value,
                            shape2)),
                    Token(")"),
                    shape3);
            var fragment1 = Custom(
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            Best(
                                rules.WildcardedNameDeclaration,
                                rules.DatabaseNameReference),
                            shape5)),
                    Token(")"),
                    shape6);
            var fragment2 = Custom(
                    Token("if_later_than"),
                    rules.StringLiteral,
                    shape11);
            var fragment3 = Custom(
                    If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                    Token("schema"),
                    shape12);
            var fragment4 = Custom(
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.DatabaseNameReference,
                            Optional(
                                fragment2),
                            shape16)),
                    Token(")"),
                    Token("schema"),
                    shape17);
            var fragment5 = Custom(
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.TableNameReference,
                            shape26)),
                    Token(")"),
                    shape27);
            var fragment6 = Custom(
                    Token("with"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            Best(
                                Token("docstring"),
                                Token("folder"),
                                If(Not(And(Token("docstring", "folder"))), rules.NameDeclaration)),
                            Token("="),
                            rules.Value,
                            shape31)),
                    Token(")"));
            var fragment7 = Custom(
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.NameDeclaration,
                            Token(":"),
                            rules.Type,
                            shape34)),
                    Token(")"),
                    shape6);
            var fragment8 = Custom(
                    rules.NameDeclaration,
                    fragment7,
                    shape36);
            var fragment9 = Custom(
                    rules.ColumnNameReference,
                    Token(":"),
                    rules.StringLiteral,
                    shape46);
            var fragment10 = Custom(
                    Token("with"),
                    Token("("),
                    ZeroOrMoreCommaList(
                        Custom(
                            If(Not(Token(")")), rules.NameDeclaration),
                            Token("="),
                            rules.Value,
                            shape48)),
                    Token(")"),
                    shape3);
            var fragment11 = Custom(
                    rules.NameDeclaration,
                    Token("="),
                    rules.Value,
                    shape48);
            var fragment12 = Custom(
                    Token("with"),
                    Token("("),
                    OneOrMoreCommaList(
                        fragment11),
                    Token(")"),
                    shape3);
            var fragment13 = Custom(
                    rules.NameDeclaration,
                    Token(":"),
                    rules.Type,
                    shape34);
            var fragment14 = Custom(
                    new Parser<LexicalToken>[] {
                        Token("external"),
                        Token("table"),
                        rules.NameDeclaration,
                        Token("("),
                        OneOrMoreCommaList(
                            fragment13),
                        Token(")"),
                        Token("kind"),
                        Token("="),
                        Best(
                            Token("storage"),
                            Token("blob").Hide(),
                            Token("adl").Hide()),
                        Optional(
                            Custom(
                                Token("partition"),
                                Token("by"),
                                Token("("),
                                OneOrMoreCommaList(
                                    Custom(
                                        rules.NameDeclaration,
                                        Token(":"),
                                        Best(
                                            Custom(
                                                Token("string"),
                                                Optional(
                                                    Custom(
                                                        Token("="),
                                                        rules.NameDeclaration,
                                                        shape66)),
                                                shape59),
                                            Custom(
                                                Token("datetime"),
                                                Optional(
                                                    Custom(
                                                        Token("="),
                                                        Best(
                                                            Custom(
                                                                Token("bin"),
                                                                Token("("),
                                                                rules.NameDeclaration,
                                                                Token(","),
                                                                rules.Value,
                                                                Token(")"),
                                                                shape68),
                                                            Custom(
                                                                Token("startofday", "startofweek", "startofmonth", "startofyear"),
                                                                Token("("),
                                                                rules.NameDeclaration,
                                                                Token(")"),
                                                                shape69)))),
                                                shape59),
                                            Custom(
                                                Token("long"),
                                                Token("="),
                                                Token("hash"),
                                                Token("("),
                                                rules.NameDeclaration,
                                                Token(","),
                                                rules.Value,
                                                Token(")"),
                                                shape71)),
                                        shape72)),
                                Token(")"),
                                Optional(
                                    Custom(
                                        Token("pathformat"),
                                        Token("="),
                                        Token("("),
                                        Optional(
                                            Custom(
                                                rules.StringLiteral,
                                                shape62)),
                                        OneOrMoreList(
                                            Custom(
                                                Best(
                                                    Custom(
                                                        If(Not(Token("datetime_pattern")), rules.NameDeclaration),
                                                        shape61),
                                                    Custom(
                                                        Token("datetime_pattern"),
                                                        Token("("),
                                                        rules.StringLiteral,
                                                        Token(","),
                                                        rules.NameDeclaration,
                                                        Token(")"),
                                                        shape74)),
                                                Optional(
                                                    Custom(
                                                        rules.StringLiteral,
                                                        shape62)),
                                                shape75)),
                                        Token(")"),
                                        shape76)),
                                shape77)),
                        Optional(
                            Custom(
                                Token("catalog"),
                                Token("="),
                                rules.StringLiteral,
                                shape79)),
                        Token("dataformat"),
                        Token("="),
                        Token("avro", "apacheavro", "csv", "json", "multijson", "orc", "parquet", "psv", "raw", "scsv", "sohsv", "sstream", "tsv", "tsve", "txt", "w3clogfile", "azmonstream"),
                        Token("("),
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape65)),
                        Token(")"),
                        Optional(
                            fragment12)}
                    ,
                    shape81);
            var fragment15 = Custom(
                    new Parser<LexicalToken>[] {
                        Token("external"),
                        Token("table"),
                        rules.NameDeclaration,
                        Optional(
                            fragment7),
                        Token("kind"),
                        Token("="),
                        Token("delta"),
                        Token("("),
                        rules.StringLiteral,
                        Token(")"),
                        Optional(
                            fragment12)}
                    ,
                    shape82);
            var fragment16 = Custom(
                    Token("table"),
                    Token("="),
                    rules.NameDeclaration,
                    shape43);
            var fragment17 = Custom(
                    Token("hot"),
                    Token("="),
                    rules.Value,
                    shape96);
            var fragment18 = Custom(
                    Token("hotdata"),
                    Token("="),
                    rules.Value,
                    Token("hotindex"),
                    Token("="),
                    rules.Value,
                    shape99);
            var fragment19 = Custom(
                    Token("recoverability"),
                    Token("="),
                    Token("disabled", "enabled"),
                    shape109);
            var fragment20 = Custom(
                    Token("softdelete"),
                    Token("="),
                    rules.Value,
                    Optional(
                        fragment19),
                    shape111);
            var fragment21 = Custom(
                    rules.ColumnNameReference,
                    Token("asc", "desc"),
                    shape115);
            var fragment22 = Custom(
                    Token("bin"),
                    Token("("),
                    rules.NameDeclaration,
                    Token(","),
                    rules.Value,
                    Token(")"),
                    shape68);
            var fragment23 = Custom(
                    Token("startofday", "startofweek", "startofmonth", "startofyear"),
                    Token("("),
                    rules.NameDeclaration,
                    Token(")"),
                    shape69);
            var fragment24 = Custom(
                    Token("pathformat"),
                    Token("="),
                    Token("("),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape62)),
                    OneOrMoreList(
                        Custom(
                            Best(
                                Custom(
                                    If(Not(Token("datetime_pattern")), rules.NameDeclaration),
                                    shape61),
                                Custom(
                                    Token("datetime_pattern"),
                                    Token("("),
                                    rules.StringLiteral,
                                    Token(","),
                                    rules.NameDeclaration,
                                    Token(")"),
                                    shape74)),
                            Optional(
                                Custom(
                                    rules.StringLiteral,
                                    shape62)),
                            shape75)),
                    Token(")"),
                    shape76);
            var fragment25 = Custom(
                    Token("partition"),
                    Token("by"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.NameDeclaration,
                            Token(":"),
                            Best(
                                Custom(
                                    Token("string"),
                                    Token("="),
                                    rules.NameDeclaration,
                                    shape134),
                                Custom(
                                    Token("datetime"),
                                    Token("="),
                                    Best(
                                        fragment22,
                                        fragment23),
                                    shape135)),
                            shape72)),
                    Token(")"),
                    Optional(
                        fragment24),
                    shape77);
            var fragment26 = Custom(
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.StringLiteral,
                            shape65)),
                    Token(")"),
                    shape137);
            var fragment27 = Custom(
                    Token("kind"),
                    Token("="),
                    Token("delta"),
                    shape141);
            var fragment28 = Custom(
                    Token("("),
                    rules.StringLiteral,
                    Token(")"),
                    shape143);
            var fragment29 = Custom(
                    Token("with"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            Token("AutoApplyToNewTables", "IsEnabled", "Backfill"),
                            Token("="),
                            Token("true", "false"))),
                    Token(")"));
            var fragment30 = Custom(
                    rules.StringLiteral,
                    Token("roles"),
                    shape171);
            var fragment31 = Custom(
                    Token("none"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape174)),
                    shape177);
            var fragment32 = Custom(
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.StringLiteral,
                            shape170)),
                    Token(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape174)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape175)),
                    shape178);
            var fragment33 = Custom(
                    Token("application"),
                    rules.StringLiteral,
                    shape182);
            var fragment34 = Custom(
                    Token("user"),
                    rules.StringLiteral,
                    shape184);
            var fragment35 = Custom(
                    Best(
                        Token("ingestionMapping"),
                        Token("ingestionMappingReference"),
                        Token("creationTime"),
                        Token("distributed"),
                        Token("docstring"),
                        Token("extend_schema"),
                        Token("folder"),
                        Token("format"),
                        Token("ingestIfNotExists"),
                        Token("ignoreFirstRecord"),
                        Token("persistDetails"),
                        Token("policy_ingestionTime"),
                        Token("recreate_schema"),
                        Token("tags"),
                        Token("validationPolicy"),
                        Token("zipPattern"),
                        Token("small_dimension_table"),
                        If(Not(And(Token("ingestionMapping", "ingestionMappingReference", "creationTime", "distributed", "docstring", "extend_schema", "folder", "format", "ingestIfNotExists", "ignoreFirstRecord", "persistDetails", "policy_ingestionTime", "recreate_schema", "tags", "validationPolicy", "zipPattern", "small_dimension_table"))), rules.NameDeclaration)),
                    Token("="),
                    rules.Value,
                    shape31);
            var fragment36 = Custom(
                    Token("with"),
                    Token("("),
                    OneOrMoreCommaList(
                        fragment35),
                    Token(")"));
            var fragment37 = Custom(
                    Token("with"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            Best(
                                Token("lookback"),
                                Token("lookback_column"),
                                Token("backfill"),
                                Token("effectiveDateTime"),
                                Token("updateExtentsCreationTime"),
                                Token("autoUpdateSchema"),
                                Token("dimensionTables"),
                                Token("dimensionMaterializedViews"),
                                Token("folder"),
                                Token("docString"),
                                If(Not(And(Token("lookback", "lookback_column", "backfill", "effectiveDateTime", "updateExtentsCreationTime", "autoUpdateSchema", "dimensionTables", "dimensionMaterializedViews", "folder", "docString"))), rules.NameDeclaration)),
                            Token("="),
                            rules.Value,
                            shape31)),
                    Token(")"));
            var fragment38 = Custom(
                    Token("with"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            Best(
                                Token("lookback"),
                                Token("lookback_column"),
                                Token("dimensionTables"),
                                Token("dimensionMaterializedViews"),
                                If(Not(And(Token("lookback", "lookback_column", "dimensionTables", "dimensionMaterializedViews"))), rules.NameDeclaration)),
                            Token("="),
                            rules.Value,
                            shape31)),
                    Token(")"));
            var fragment39 = Custom(
                    Token("with"),
                    Token("("),
                    Token("scope"),
                    Token("="),
                    Token("cluster", "workloadgroup"),
                    Token(")"),
                    shape199);
            var fragment40 = Custom(
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.AnyGuidLiteralOrString,
                            shape202)),
                    Token(")"),
                    shape137);
            var fragment41 = Custom(
                    Token("where"),
                    OneOrMoreList(
                        Custom(
                            Token("tags"),
                            Token("has", "contains", "!has", "!contains"),
                            rules.StringLiteral,
                            shape204),
                        separatorParser: Token("and")));
            var fragment42 = Custom(
                    Token("with"),
                    Token("("),
                    Token("extentsShowFilteringRuntimePolicy"),
                    Token("="),
                    rules.Value,
                    Token(")"),
                    shape205);
            var fragment43 = Custom(
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                            shape9)),
                    shape207);
            var fragment44 = Custom(
                    Token("databases"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.DatabaseNameReference,
                            shape9)),
                    Token(")"),
                    shape208);
            var fragment45 = Custom(
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    shape211);
            var fragment46 = Custom(
                    Token("tables"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.TableNameReference,
                            shape26)),
                    Token(")"),
                    shape212);
            var fragment47 = Custom(
                    Token("from"),
                    rules.TableNameReference,
                    shape211);
            var fragment48 = Custom(
                    Token("<|"),
                    rules.QueryInput,
                    shape217);
            var fragment49 = Custom(
                    Token("limit"),
                    rules.Value,
                    shape219);
            var fragment50 = Custom(
                    Token("all"),
                    Token("tables"));
            var fragment51 = Custom(
                    Token("trim"),
                    Token("by"),
                    Token("extentsize", "datasize"),
                    rules.Value,
                    Token("MB", "GB", "bytes"),
                    shape221);
            var fragment52 = Custom(
                    Token("with"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            Best(
                                Token("ContinueOnErrors"),
                                Token("ThrowOnErrors"),
                                If(Not(And(Token("ContinueOnErrors", "ThrowOnErrors"))), rules.NameDeclaration)),
                            Token("="),
                            rules.Value,
                            shape31)),
                    Token(")"));
            var fragment53 = Custom(
                    Token("cluster"),
                    Token("("),
                    rules.StringLiteral,
                    Token(")"),
                    Token("."),
                    Token("database"),
                    Token("("),
                    rules.StringLiteral,
                    Token(")"),
                    shape235);
            var fragment54 = Custom(
                    Token("database"),
                    Token("("),
                    rules.StringLiteral,
                    Token(")"),
                    shape236);
            var fragment55 = Custom(
                    Token("<|"),
                    rules.QueryInput,
                    shape244);
            var fragment56 = Custom(
                    Token("from"),
                    rules.StringLiteral,
                    shape253);
            var fragment57 = Custom(
                    Optional(Token(",")),
                    OneOrMoreCommaList(
                        Custom(
                            Token("hot_window"),
                            Token("="),
                            Custom(
                                rules.Value,
                                Token(".."),
                                rules.Value,
                                shape254),
                            shape256)),
                    shape257);
            var fragment58 = Custom(
                    Token("database"),
                    rules.DatabaseNameReference,
                    shape258);
            var fragment59 = Custom(
                    Token("("),
                    ZeroOrMoreCommaList(
                        Custom(
                            If(Not(Token(")")), rules.DatabaseNameReference),
                            shape24)),
                    Token(")"),
                    shape259);
            var fragment60 = Custom(
                    Token("hotdata"),
                    Token("="),
                    rules.Value,
                    Token("hotindex"),
                    Token("="),
                    rules.Value,
                    shape263);
            var fragment61 = Custom(
                    Token("hot"),
                    Token("="),
                    rules.Value,
                    shape265);
            var fragment62 = Custom(
                    Token("table"),
                    rules.TableNameReference,
                    shape268);
            var fragment63 = Custom(
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    shape270);
            var fragment64 = Custom(
                    Token("tables"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.NameDeclaration,
                            shape260)),
                    Token(")"),
                    shape3);
            var fragment65 = Custom(
                    Token("materialized-views"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.NameDeclaration,
                            shape260)),
                    Token(")"),
                    shape3);
            var fragment66 = Custom(
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.StringLiteral,
                            shape85)),
                    Token(")"),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape86)),
                    shape271);
            var fragment67 = Custom(
                    Token("table"),
                    rules.TableNameReference,
                    shape290);

            var ShowDatabase = Command("ShowDatabase", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        fragment0),
                    shape25));

            var ShowDatabaseDetails = Command("ShowDatabaseDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("details", "verbose"),
                    Optional(
                        fragment0),
                    new [] {CD(), CD(), CD("flavor"), CD(isOptional: true)}));

            var ShowDatabaseIdentity = Command("ShowDatabaseIdentity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("identity"),
                    Optional(
                        fragment0),
                    shape4));

            var ShowDatabasePolicies = Command("ShowDatabasePolicies", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policies"),
                    Optional(
                        fragment0),
                    shape4));

            var ShowDatabaseDataStats = Command("ShowDatabaseDataStats", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("datastats"),
                    Optional(
                        fragment0),
                    shape4));

            var ShowDatabaseMetadata = Command("ShowDatabaseMetadata", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("metadata"),
                    Optional(
                        fragment0),
                    shape4));

            var ShowClusterDatabases = Command("ShowClusterDatabases", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Optional(Token("cluster")),
                    Token("databases"),
                    Optional(
                        fragment1),
                    shape280));

            var ShowClusterDatabasesDetails = Command("ShowClusterDatabasesDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Optional(Token("cluster")),
                    Token("databases"),
                    Optional(
                        fragment1),
                    Token("details", "verbose"),
                    shape7));

            var ShowClusterDatabasesIdentity = Command("ShowClusterDatabasesIdentity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Optional(Token("cluster")),
                    Token("databases"),
                    Optional(
                        fragment1),
                    Token("identity"),
                    shape7));

            var ShowClusterDatabasesPolicies = Command("ShowClusterDatabasesPolicies", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Optional(Token("cluster")),
                    Token("databases"),
                    Optional(
                        fragment1),
                    Token("policies"),
                    shape7));

            var ShowClusterDatabasesDataStats = Command("ShowClusterDatabasesDataStats", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Optional(Token("cluster")),
                    Token("databases"),
                    Optional(
                        fragment1),
                    Token("datastats"),
                    shape7));

            var ShowClusterDatabasesMetadata = Command("ShowClusterDatabasesMetadata", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Optional(Token("cluster")),
                    Token("databases"),
                    Optional(
                        fragment1),
                    Token("metadata"),
                    shape7));

            var CreateDatabase = Command("CreateDatabase", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.NameDeclaration,
                    Optional(
                        Best(
                            Custom(
                                Token("persist"),
                                Token("("),
                                OneOrMoreCommaList(
                                    Custom(
                                        rules.StringLiteral,
                                        shape8)),
                                Token(")"),
                                new [] {CD(), CD(), CD(CompletionHint.Literal), CD()}),
                            Token("volatile"))),
                    Optional(
                        Custom(
                            Token("ifnotexists"),
                            CD("IfNotExists"))),
                    Optional(
                        fragment0),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.None), CD(isOptional: true), CD(isOptional: true), CD(isOptional: true)}));

            var AttachDatabase = Command("AttachDatabase", 
                Custom(
                    Best(
                        Token("attach"),
                        Token("load").Hide()),
                    Optional(
                        Custom(
                            Token("database"),
                            Optional(
                                Token("all", "metadata")).Hide(),
                            If(Not(And(Token("all", "metadata"))), rules.DatabaseNameReference),
                            new [] {CD(), CD(isOptional: true), CD("DatabaseName", CompletionHint.Database)})),
                    Token("from"),
                    rules.StringLiteral,
                    Optional(
                        Custom(
                            Token("readonly"),
                            Optional(
                                Custom(
                                    Token("version"),
                                    Token("="),
                                    rules.StringLiteral,
                                    new [] {CD(), CD(), CD("Version", CompletionHint.Literal)})),
                            new [] {CD("ReadOnly"), CD(isOptional: true)})),
                    Optional(
                        fragment0),
                    new [] {CD(), CD(isOptional: true), CD(), CD("Path", CompletionHint.Literal), CD(isOptional: true), CD(isOptional: true)}));

            var DetachDatabase = Command("DetachDatabase", 
                Custom(
                    Best(
                        Token("detach"),
                        Token("drop").Hide()),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Optional(
                        Custom(
                            Token("ifexists"),
                            CD("IfExists"))),
                    Optional(
                        Custom(
                            Token("skip-seal"),
                            CD("SkipSeal"))),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(isOptional: true), CD(isOptional: true)}));

            var AlterDatabasePrettyName = Command("AlterDatabasePrettyName", 
                Custom(
                    Token("alter", "set"),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(And(Token("prettyname", "policy"))), rules.DatabaseNameReference),
                            shape9)),
                    Token("prettyname"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD(CompletionHint.Database, isOptional: true), CD(), CD("PrettyName", CompletionHint.Literal)}));

            var DropDatabasePrettyName = Command("DropDatabasePrettyName", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("prettyname"),
                    shape201));

            var AlterDatabasePersistMetadata = Command("AlterDatabasePersistMetadata", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("prettyname", "policy"))), rules.DatabaseNameReference),
                    Token("persist"),
                    Token("metadata"),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            Optional(
                                Custom(
                                    Token("allow-non-empty-container"),
                                    CD("AllowNonEmptyContainer"))),
                            new [] {CD("Path", CompletionHint.Literal), CD(isOptional: true)})),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD(CompletionHint.Literal, isOptional: true)}));

            var SetAccess = Command("SetAccess", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("access"),
                    rules.DatabaseNameReference,
                    Token("to"),
                    Token("readonly", "readwrite"),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD("AccessMode")}));

            var ShowDatabaseSchema = Command("ShowDatabaseSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                            shape9)),
                    Token("schema"),
                    Optional(
                        Custom(
                            Token("details"),
                            shape15)),
                    Optional(
                        fragment2),
                    shape14));

            var ShowDatabaseSchemaAsJson = Command("ShowDatabaseSchemaAsJson", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Best(
                        Token("schema"),
                        fragment3),
                    Optional(
                        fragment2),
                    Token("as"),
                    Token("json"),
                    Optional(
                        fragment0),
                    shape18));

            var ShowDatabaseSchemaAsCslScript = Command("ShowDatabaseSchemaAsCslScript", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Best(
                        Token("schema"),
                        fragment3),
                    Optional(
                        fragment2),
                    Token("as"),
                    Token("kql", "csl"),
                    Optional(
                        Custom(
                            Token("script"),
                            shape13)),
                    Optional(
                        fragment0),
                    new [] {CD(), CD(), CD(), CD(isOptional: true), CD(), CD(), CD(isOptional: true), CD(isOptional: true)}));

            var ShowDatabaseCslSchema = Command("ShowDatabaseCslSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                            shape24)),
                    Token("cslschema", "kqlschema"),
                    Optional(
                        Custom(
                            Token("script"),
                            shape13)),
                    Optional(
                        Custom(
                            Token("if_later_than"),
                            rules.StringLiteral,
                            new [] {CD(), CD("databaseVersion", CompletionHint.Literal)})),
                    shape14));

            var ShowDatabaseSchemaViolations = Command("ShowDatabaseSchemaViolations", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                            shape9)),
                    Token("schema"),
                    Token("violations"),
                    shape94));

            var ShowDatabasesSchema = Command("ShowDatabasesSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Best(
                        Token("schema"),
                        fragment4),
                    Optional(
                        Custom(
                            Token("details"),
                            shape15)),
                    shape4));

            var ShowDatabasesSchemaAsJson = Command("ShowDatabasesSchemaAsJson", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Best(
                        Token("schema"),
                        fragment4),
                    Optional(Token("details")),
                    Token("as"),
                    Token("json"),
                    Optional(
                        fragment0),
                    shape18));

            var CreateDatabaseIngestionMapping = Command("CreateDatabaseIngestionMapping", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.NameDeclaration,
                    Token("ingestion"),
                    Token("csv", "json", "avro", "parquet", "orc", "w3clogfile", "azmonstream"),
                    Token("mapping"),
                    rules.StringLiteral,
                    rules.StringLiteral,
                    shape22));

            var CreateOrAlterDatabaseIngestionMapping = Command("CreateOrAlterDatabaseIngestionMapping", 
                Custom(
                    Token("create-or-alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.NameDeclaration,
                    Token("ingestion"),
                    Token("csv", "json", "avro", "parquet", "orc", "w3clogfile", "azmonstream"),
                    Token("mapping"),
                    rules.StringLiteral,
                    rules.StringLiteral,
                    shape22));

            var AlterDatabaseIngestionMapping = Command("AlterDatabaseIngestionMapping", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("prettyname", "policy"))), rules.DatabaseNameReference),
                    Token("ingestion"),
                    Token("csv", "json", "avro", "parquet", "orc", "w3clogfile", "azmonstream"),
                    Token("mapping"),
                    rules.StringLiteral,
                    rules.StringLiteral,
                    shape23));

            var AlterMergeDatabaseIngestionMapping = Command("AlterMergeDatabaseIngestionMapping", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("ingestion"),
                    Token("csv", "json", "avro", "parquet", "orc", "w3clogfile", "azmonstream"),
                    Token("mapping"),
                    rules.StringLiteral,
                    rules.StringLiteral,
                    shape23));

            var ShowDatabaseIngestionMappings = Command("ShowDatabaseIngestionMappings", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                            shape24)),
                    Token("ingestion"),
                    Optional(
                        Custom(
                            Token("csv", "avro", "apacheavro", "json", "parquet", "sstream", "orc", "w3clogfile", "azmonstream"),
                            shape19)),
                    Token("mappings"),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape278)),
                    Optional(
                        fragment0),
                    new [] {CD(), CD(), CD(CompletionHint.Database, isOptional: true), CD(), CD(isOptional: true), CD(), CD(CompletionHint.Literal, isOptional: true), CD(isOptional: true)}));

            var ShowIngestionMappings = Command("ShowIngestionMappings", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Optional(Token("cluster")),
                    Token("ingestion"),
                    Optional(
                        Custom(
                            Token("csv", "avro", "apacheavro", "json", "parquet", "sstream", "orc", "w3clogfile", "azmonstream"),
                            shape19)),
                    Token("mappings"),
                    Optional(
                        fragment0),
                    new [] {CD(), CD(isOptional: true), CD(), CD(isOptional: true), CD(), CD(isOptional: true)}));

            var DropDatabaseIngestionMapping = Command("DropDatabaseIngestionMapping", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("ingestion"),
                    Token("csv", "json", "avro", "parquet", "orc", "w3clogfile", "azmonstream"),
                    Token("mapping"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD("MappingKind"), CD(), CD("MappingName", CompletionHint.Literal)}));

            var ShowTables = Command("ShowTables", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Optional(
                        fragment5),
                    shape25));

            var ShowTable = Command("ShowTable", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table)}));

            var ShowTablesDetails = Command("ShowTablesDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Optional(
                        fragment5),
                    Token("details"),
                    shape192));

            var ShowTableDetails = Command("ShowTableDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("details"),
                    shape28));

            var ShowTableCslSchema = Command("ShowTableCslSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("kqlschema", "cslschema"),
                    shape28));

            var ShowTableSchemaAsJson = Command("ShowTableSchemaAsJson", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("schema"),
                    Token("as"),
                    Token("json"),
                    shape100));

            var CreateTable = Command("CreateTable", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.NameDeclaration,
                    fragment7,
                    Optional(
                        fragment6),
                    shape35));

            var CreateTableBasedOnAnother = Command("CreateTableBasedOnAnother", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.NameDeclaration,
                    Token("based-on"),
                    rules.NameDeclaration,
                    Optional(Token("ifnotexists")),
                    Optional(
                        fragment6),
                    new [] {CD(), CD(), CD("NewTableName", CompletionHint.None), CD(), CD("TableName", CompletionHint.None), CD(isOptional: true), CD(isOptional: true)}));

            var CreateMergeTable = Command("CreateMergeTable", 
                Custom(
                    Token("create-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.NameDeclaration,
                    fragment7,
                    Optional(
                        fragment6),
                    shape35));

            var DefineTable = Command("DefineTable", 
                Custom(
                    Token("define", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.NameDeclaration,
                    fragment7,
                    Optional(
                        fragment6),
                    shape35));

            var CreateTables = Command("CreateTables", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("tables"),
                    OneOrMoreCommaList(
                        fragment8),
                    Optional(
                        fragment0),
                    shape37));

            var CreateMergeTables = Command("CreateMergeTables", 
                Custom(
                    Token("create-merge", CompletionKind.CommandPrefix),
                    Token("tables"),
                    OneOrMoreCommaList(
                        fragment8),
                    Optional(
                        fragment0),
                    shape37));

            var DefineTables = Command("DefineTables", 
                Custom(
                    Token("define", CompletionKind.CommandPrefix),
                    Token("tables"),
                    OneOrMoreCommaList(
                        fragment8),
                    Optional(
                        fragment0),
                    shape37));

            var AlterTable = Command("AlterTable", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    fragment7,
                    Optional(
                        fragment6),
                    shape38));

            var AlterMergeTable = Command("AlterMergeTable", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    fragment7,
                    Optional(
                        fragment6),
                    shape38));

            var AlterTableDocString = Command("AlterTableDocString", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("docstring"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("Documentation", CompletionHint.Literal)}));

            var AlterTableFolder = Command("AlterTableFolder", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("folder"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("Folder", CompletionHint.Literal)}));

            var RenameTable = Command("RenameTable", 
                Custom(
                    Token("rename", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("to"),
                    rules.NameDeclaration,
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("NewTableName", CompletionHint.None)}));

            var RenameTables = Command("RenameTables", 
                Custom(
                    Token("rename", CompletionKind.CommandPrefix),
                    Token("tables"),
                    OneOrMoreCommaList(
                        Custom(
                            rules.NameDeclaration,
                            Token("="),
                            rules.TableNameReference,
                            new [] {CD("NewTableName", CompletionHint.None), CD(), CD("TableName", CompletionHint.Table)})),
                    shape43));

            var UndoDropExtentContainer = Command("UndoDropExtentContainer", 
                Custom(
                    Token("undo", CompletionKind.CommandPrefix),
                    Token("drop"),
                    Token("extentcontainer"),
                    rules.AnyGuidLiteralOrString,
                    new [] {CD(), CD(), CD(), CD("ContainerID", CompletionHint.Literal)}));

            var DropTable = Command("DropTable", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Optional(Token("ifexists")),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(isOptional: true)}));

            var UndoDropTable = Command("UndoDropTable", 
                Custom(
                    Token("undo", CompletionKind.CommandPrefix),
                    Token("drop"),
                    Token("table"),
                    rules.NameDeclaration,
                    Optional(
                        Custom(
                            Token("as"),
                            rules.NameDeclaration,
                            new [] {CD(), CD("TableName", CompletionHint.None)})),
                    Token("version"),
                    Token("="),
                    rules.StringLiteral,
                    Optional(Token("internal")),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.None), CD(isOptional: true), CD(), CD(), CD("Version", CompletionHint.Literal), CD(isOptional: true)}));

            var DropTables = Command("DropTables", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.TableNameReference,
                            shape26)),
                    Token(")"),
                    Optional(Token("ifexists")),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(isOptional: true)}));

            var CreateTableIngestionMapping = Command("CreateTableIngestionMapping", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.NameDeclaration,
                    Token("ingestion"),
                    Token("csv", "json", "avro", "parquet", "orc", "w3clogfile", "azmonstream"),
                    Token("mapping"),
                    rules.StringLiteral,
                    rules.StringLiteral,
                    Optional(
                        fragment0),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.None), CD(), CD("MappingKind"), CD(), CD("MappingName", CompletionHint.Literal), CD("MappingFormat", CompletionHint.Literal), CD(isOptional: true)}));

            var CreateOrAlterTableIngestionMapping = Command("CreateOrAlterTableIngestionMapping", 
                Custom(
                    Token("create-or-alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("ingestion"),
                    Token("csv", "json", "avro", "parquet", "orc", "w3clogfile", "azmonstream"),
                    Token("mapping"),
                    rules.StringLiteral,
                    rules.StringLiteral,
                    Optional(
                        fragment0),
                    shape40));

            var AlterTableIngestionMapping = Command("AlterTableIngestionMapping", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("ingestion"),
                    Token("csv", "json", "avro", "parquet", "orc", "w3clogfile", "azmonstream"),
                    Token("mapping"),
                    rules.StringLiteral,
                    rules.StringLiteral,
                    Optional(
                        fragment0),
                    shape40));

            var AlterMergeTableIngestionMapping = Command("AlterMergeTableIngestionMapping", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("ingestion"),
                    Token("csv", "json", "avro", "parquet", "orc", "w3clogfile", "azmonstream"),
                    Token("mapping"),
                    rules.StringLiteral,
                    rules.StringLiteral,
                    Optional(
                        fragment0),
                    shape40));

            var ShowTableIngestionMappings = Command("ShowTableIngestionMappings", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("ingestion"),
                    Optional(
                        Custom(
                            Token("csv", "json", "avro", "parquet", "orc", "w3clogfile", "azmonstream"),
                            shape19)),
                    Token("mappings"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(isOptional: true), CD()}));

            var ShowTableIngestionMapping = Command("ShowTableIngestionMapping", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("ingestion"),
                    Optional(
                        Custom(
                            Token("csv", "json", "avro", "parquet", "orc", "w3clogfile", "azmonstream"),
                            shape19)),
                    Token("mapping"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(isOptional: true), CD(), CD("MappingName", CompletionHint.Literal)}));

            var DropTableIngestionMapping = Command("DropTableIngestionMapping", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("ingestion"),
                    Token("csv", "json", "avro", "parquet", "orc", "w3clogfile", "azmonstream"),
                    Token("mapping"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("MappingKind"), CD(), CD("MappingName", CompletionHint.Literal)}));

            var RenameColumn = Command("RenameColumn", 
                Custom(
                    Token("rename", CompletionKind.CommandPrefix),
                    Token("column"),
                    rules.DatabaseTableColumnNameReference,
                    Token("to"),
                    rules.NameDeclaration,
                    new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD("NewColumnName", CompletionHint.None)}));

            var RenameColumns = Command("RenameColumns", 
                Custom(
                    Token("rename", CompletionKind.CommandPrefix),
                    Token("columns"),
                    OneOrMoreCommaList(
                        Custom(
                            rules.NameDeclaration,
                            Token("="),
                            rules.DatabaseTableColumnNameReference,
                            new [] {CD("NewColumnName", CompletionHint.None), CD(), CD("ColumnName", CompletionHint.Column)})),
                    shape43));

            var AlterColumnType = Command("AlterColumnType", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("column"),
                    rules.DatabaseTableColumnNameReference,
                    Token("type"),
                    Token("="),
                    rules.Type,
                    new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD(), CD("ColumnType")}));

            var DropColumn = Command("DropColumn", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("column"),
                    rules.TableColumnNameReference,
                    new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column)}));

            var DropTableColumns = Command("DropTableColumns", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("columns"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.ColumnNameReference,
                            shape42)),
                    Token(")"),
                    shape44));

            var AlterTableColumnDocStrings = Command("AlterTableColumnDocStrings", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("column-docstrings"),
                    Token("("),
                    OneOrMoreCommaList(
                        fragment9),
                    Token(")"),
                    shape44));

            var AlterMergeTableColumnDocStrings = Command("AlterMergeTableColumnDocStrings", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("column-docstrings"),
                    Token("("),
                    OneOrMoreCommaList(
                        fragment9),
                    Token(")"),
                    shape44));

            var ShowFunctions = Command("ShowFunctions", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("functions")));

            var ShowFunction = Command("ShowFunction", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("function"),
                    rules.FunctionNameReference,
                    Optional(
                        fragment10),
                    shape51));

            var CreateFunction = Command("CreateFunction", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("function"),
                    Optional(Token("ifnotexists")),
                    Optional(
                        fragment10),
                    If(Not(And(Token("ifnotexists", "with"))), rules.NameDeclaration),
                    rules.FunctionDeclaration,
                    new [] {CD(), CD(), CD(isOptional: true), CD(isOptional: true), CD("FunctionName", CompletionHint.None), CD()}));

            var AlterFunction = Command("AlterFunction", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("function"),
                    Optional(
                        fragment12),
                    If(Not(Token("with")), rules.FunctionNameReference),
                    rules.FunctionDeclaration,
                    new [] {CD(), CD(), CD(isOptional: true), CD("FunctionName", CompletionHint.Function), CD()}));

            var CreateOrAlterFunction = Command("CreateOrAlterFunction", 
                Custom(
                    Token("create-or-alter", CompletionKind.CommandPrefix),
                    Token("function"),
                    Optional(
                        fragment12),
                    If(Not(Token("with")), rules.NameDeclaration),
                    rules.FunctionDeclaration,
                    new [] {CD(), CD(), CD(isOptional: true), CD("FunctionName", CompletionHint.None), CD()}));

            var DropFunction = Command("DropFunction", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("function"),
                    rules.FunctionNameReference,
                    Optional(Token("ifexists")),
                    shape51));

            var DropFunctions = Command("DropFunctions", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("functions"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.FunctionNameReference,
                            shape49)),
                    Token(")"),
                    Optional(Token("ifexists")),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Function), CD(), CD(isOptional: true)}));

            var AlterFunctionDocString = Command("AlterFunctionDocString", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("function"),
                    If(Not(Token("with")), rules.FunctionNameReference),
                    Token("docstring"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD(CompletionHint.Function), CD(), CD("Documentation", CompletionHint.Literal)}));

            var AlterFunctionFolder = Command("AlterFunctionFolder", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("function"),
                    If(Not(Token("with")), rules.FunctionNameReference),
                    Token("folder"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD(), CD("Folder", CompletionHint.Literal)}));

            var ShowExternalTables = Command("ShowExternalTables", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("tables"),
                    Optional(
                        fragment10),
                    shape4));

            var ShowExternalTable = Command("ShowExternalTable", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    If(Not(Token("*")), rules.ExternalTableNameReference),
                    Optional(
                        fragment10),
                    new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(isOptional: true)}));

            var ShowExternalTablesDetails = Command("ShowExternalTablesDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("tables"),
                    Token("details")));

            var ShowExternalTableDetails = Command("ShowExternalTableDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    If(Not(Token("*")), rules.ExternalTableNameReference),
                    Token("details"),
                    shape55));

            var ShowExternalTableCslSchema = Command("ShowExternalTableCslSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    If(Not(Token("*")), rules.ExternalTableNameReference),
                    Token("cslschema", "kqlschema"),
                    shape55));

            var ShowExternalTableSchema = Command("ShowExternalTableSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    If(Not(Token("*")), rules.ExternalTableNameReference),
                    Token("schema"),
                    Token("as"),
                    Token("json", "csl", "kql"),
                    shape133));

            var ShowExternalTableArtifacts = Command("ShowExternalTableArtifacts", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    If(Not(Token("*")), rules.ExternalTableNameReference),
                    Token("artifacts"),
                    Optional(
                        fragment49),
                    new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD(isOptional: true)}));

            var DropExternalTable = Command("DropExternalTable", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    rules.ExternalTableNameReference,
                    new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable)}));

            var CreateStorageExternalTable = Command("CreateStorageExternalTable", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Best(
                        fragment14,
                        fragment15)));

            var AlterStorageExternalTable = Command("AlterStorageExternalTable", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Best(
                        fragment14,
                        fragment15)));

            var CreateOrAlterStorageExternalTable = Command("CreateOrAlterStorageExternalTable", 
                Custom(
                    Token("create-or-alter", CompletionKind.CommandPrefix),
                    Best(
                        fragment14,
                        fragment15)));

            var CreateSqlExternalTable = Command("CreateSqlExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("create", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.NameDeclaration,
                        Token("("),
                        OneOrMoreCommaList(
                            fragment13),
                        Token(")"),
                        Token("kind"),
                        Token("="),
                        Token("sql"),
                        Optional(
                            fragment16),
                        Token("("),
                        rules.StringLiteral,
                        Token(")"),
                        Optional(
                            fragment12)}
                    ,
                    shape83));

            var AlterSqlExternalTable = Command("AlterSqlExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.NameDeclaration,
                        Token("("),
                        OneOrMoreCommaList(
                            fragment13),
                        Token(")"),
                        Token("kind"),
                        Token("="),
                        Token("sql"),
                        Optional(
                            fragment16),
                        Token("("),
                        rules.StringLiteral,
                        Token(")"),
                        Optional(
                            fragment12)}
                    ,
                    shape83));

            var CreateOrAlterSqlExternalTable = Command("CreateOrAlterSqlExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("create-or-alter", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.NameDeclaration,
                        Token("("),
                        OneOrMoreCommaList(
                            fragment13),
                        Token(")"),
                        Token("kind"),
                        Token("="),
                        Token("sql"),
                        Optional(
                            fragment16),
                        Token("("),
                        rules.StringLiteral,
                        Token(")"),
                        Optional(
                            fragment12)}
                    ,
                    shape83));

            var CreateExternalTableMapping = Command("CreateExternalTableMapping", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    rules.ExternalTableNameReference,
                    Token("mapping"),
                    rules.StringLiteral,
                    rules.StringLiteral,
                    shape89));

            var SetExternalTableAdmins = Command("SetExternalTableAdmins", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    rules.ExternalTableNameReference,
                    Token("admins"),
                    Best(
                        Custom(
                            Token("none"),
                            Optional(Token("skip-results")),
                            shape177),
                        Custom(
                            Token("("),
                            OneOrMoreCommaList(
                                Custom(
                                    rules.StringLiteral,
                                    shape85)),
                            Token(")"),
                            Optional(Token("skip-results")),
                            Optional(
                                Custom(
                                    rules.StringLiteral,
                                    shape86)),
                            shape178)),
                    new [] {CD(), CD(), CD(), CD("externalTableName", CompletionHint.ExternalTable), CD(), CD()}));

            var AddExternalTableAdmins = Command("AddExternalTableAdmins", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("add", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.ExternalTableNameReference,
                        Token("admins"),
                        Token("("),
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape85)),
                        Token(")"),
                        Optional(Token("skip-results")),
                        Optional(
                            Custom(
                                rules.StringLiteral,
                                shape86))}
                    ,
                    shape87));

            var DropExternalTableAdmins = Command("DropExternalTableAdmins", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("drop", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.ExternalTableNameReference,
                        Token("admins"),
                        Token("("),
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape85)),
                        Token(")"),
                        Optional(Token("skip-results")),
                        Optional(
                            Custom(
                                rules.StringLiteral,
                                shape86))}
                    ,
                    shape87));

            var AlterExternalTableDocString = Command("AlterExternalTableDocString", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    rules.ExternalTableNameReference,
                    Token("docstring"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD(), CD("tableName", CompletionHint.ExternalTable), CD(), CD("docStringValue", CompletionHint.Literal)}));

            var AlterExternalTableFolder = Command("AlterExternalTableFolder", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    rules.ExternalTableNameReference,
                    Token("folder"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD(), CD("tableName", CompletionHint.ExternalTable), CD(), CD("folderValue", CompletionHint.Literal)}));

            var ShowExternalTablePrincipals = Command("ShowExternalTablePrincipals", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    If(Not(Token("*")), rules.ExternalTableNameReference),
                    Token("principals"),
                    new [] {CD(), CD(), CD(), CD("tableName", CompletionHint.ExternalTable), CD()}));

            var ShowFabric = Command("ShowFabric", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("fabric"),
                    rules.NameDeclaration,
                    new [] {CD(), CD(), CD("id", CompletionHint.None)}));

            var AlterExternalTableMapping = Command("AlterExternalTableMapping", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    rules.ExternalTableNameReference,
                    Token("mapping"),
                    rules.StringLiteral,
                    rules.StringLiteral,
                    shape89));

            var ShowExternalTableMappings = Command("ShowExternalTableMappings", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    If(Not(Token("*")), rules.ExternalTableNameReference),
                    Token("mappings"),
                    shape55));

            var ShowExternalTableMapping = Command("ShowExternalTableMapping", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    If(Not(Token("*")), rules.ExternalTableNameReference),
                    Token("mapping"),
                    rules.StringLiteral,
                    shape90));

            var DropExternalTableMapping = Command("DropExternalTableMapping", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    rules.ExternalTableNameReference,
                    Token("mapping"),
                    rules.StringLiteral,
                    shape90));

            var ShowWorkloadGroups = Command("ShowWorkloadGroups", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("workload_groups")));

            var ShowWorkloadGroup = Command("ShowWorkloadGroup", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("workload_group"),
                    rules.NameDeclaration,
                    new [] {CD(), CD(), CD("WorkloadGroup", CompletionHint.None)}));

            var CreateOrAleterWorkloadGroup = Command("CreateOrAleterWorkloadGroup", 
                Custom(
                    Token("create-or-alter", CompletionKind.CommandPrefix),
                    Token("workload_group"),
                    rules.NameDeclaration,
                    rules.StringLiteral,
                    shape93));

            var AlterMergeWorkloadGroup = Command("AlterMergeWorkloadGroup", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("workload_group"),
                    rules.NameDeclaration,
                    rules.StringLiteral,
                    shape93));

            var DropWorkloadGroup = Command("DropWorkloadGroup", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("workload_group"),
                    rules.NameDeclaration,
                    new [] {CD(), CD(), CD("WorkloadGroupName", CompletionHint.None)}));

            var ShowDatabasePolicyCaching = Command("ShowDatabasePolicyCaching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            Best(
                                If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                                Token("*")),
                            shape9)),
                    Token("policy"),
                    Token("caching"),
                    shape94));

            var ShowTablePolicyCaching = Command("ShowTablePolicyCaching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("caching"),
                    shape103));

            var ShowTableStarPolicyCaching = Command("ShowTableStarPolicyCaching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    Token("policy"),
                    Token("caching")));

            var ShowColumnPolicyCaching = Command("ShowColumnPolicyCaching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("column"),
                    Best(
                        If(Not(Token("*")), rules.DatabaseTableColumnNameReference),
                        Token("*")),
                    Token("policy"),
                    Token("caching"),
                    shape104));

            var ShowMaterializedViewPolicyCaching = Command("ShowMaterializedViewPolicyCaching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.DatabaseMaterializedViewNameReference,
                    Token("policy"),
                    Token("caching"),
                    shape105));

            var ShowGraphModelPolicyCaching = Command("ShowGraphModelPolicyCaching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("graph_model"),
                    If(Not(Token("*")), rules.GraphModelNameReference),
                    Token("policy"),
                    Token("caching"),
                    shape106));

            var ShowGraphModelStarPolicyCaching = Command("ShowGraphModelStarPolicyCaching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("graph_model"),
                    Token("*"),
                    Token("policy"),
                    Token("caching")));

            var ShowClusterPolicyCaching = Command("ShowClusterPolicyCaching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("caching")));

            var AlterDatabasePolicyCaching = Command("AlterDatabasePolicyCaching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(And(Token("prettyname", "policy"))), rules.DatabaseNameReference),
                            shape9)),
                    Token("policy"),
                    Token("caching"),
                    Best(
                        fragment17,
                        fragment18),
                    shape210));

            var AlterTablePolicyCaching = Command("AlterTablePolicyCaching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("caching"),
                    Best(
                        fragment17,
                        fragment18),
                    shape100));

            var AlterTablesPolicyCaching = Command("AlterTablesPolicyCaching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.TableNameReference,
                            shape26)),
                    Token(")"),
                    Token("policy"),
                    Token("caching"),
                    Best(
                        fragment17,
                        fragment18),
                    Optional(
                        fragment57),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(), CD(), CD(isOptional: true)}));

            var AlterColumnPolicyCaching = Command("AlterColumnPolicyCaching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("column"),
                    rules.DatabaseTableColumnNameReference,
                    Token("policy"),
                    Token("caching"),
                    Best(
                        fragment17,
                        fragment18),
                    new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD(), CD()}));

            var AlterMaterializedViewPolicyCaching = Command("AlterMaterializedViewPolicyCaching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    If(Not(Token("with")), rules.DatabaseMaterializedViewNameReference),
                    Token("policy"),
                    Token("caching"),
                    Best(
                        fragment17,
                        fragment18),
                    shape197));

            var AlterGraphModelPolicyCaching = Command("AlterGraphModelPolicyCaching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("graph_model"),
                    rules.GraphModelNameReference,
                    Token("policy"),
                    Token("caching"),
                    Best(
                        fragment17,
                        fragment18),
                    new [] {CD(), CD(), CD("ModelName", CompletionHint.GraphModel), CD(), CD(), CD()}));

            var AlterClusterPolicyCaching = Command("AlterClusterPolicyCaching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("caching"),
                    Best(
                        fragment17,
                        fragment18)));

            var DeleteDatabasePolicyCaching = Command("DeleteDatabasePolicyCaching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("caching"),
                    shape112));

            var DeleteTablePolicyCaching = Command("DeleteTablePolicyCaching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("caching"),
                    shape103));

            var DeleteColumnPolicyCaching = Command("DeleteColumnPolicyCaching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("column"),
                    rules.DatabaseTableColumnNameReference,
                    Token("policy"),
                    Token("caching"),
                    shape104));

            var DeleteMaterializedViewPolicyCaching = Command("DeleteMaterializedViewPolicyCaching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.DatabaseMaterializedViewNameReference,
                    Token("policy"),
                    Token("caching"),
                    shape105));

            var DeleteGraphModelPolicyCaching = Command("DeleteGraphModelPolicyCaching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("graph_model"),
                    rules.GraphModelNameReference,
                    Token("policy"),
                    Token("caching"),
                    shape106));

            var DeleteClusterPolicyCaching = Command("DeleteClusterPolicyCaching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("caching")));

            var ShowTablePolicyIngestionTime = Command("ShowTablePolicyIngestionTime", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("ingestiontime"),
                    shape103));

            var ShowTableStarPolicyIngestionTime = Command("ShowTableStarPolicyIngestionTime", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    Token("policy"),
                    Token("ingestiontime")));

            var AlterTablePolicyIngestionTime = Command("AlterTablePolicyIngestionTime", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("ingestiontime"),
                    Token("true"),
                    shape100));

            var AlterTablesPolicyIngestionTime = Command("AlterTablesPolicyIngestionTime", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.TableNameReference,
                            shape26)),
                    Token(")"),
                    Token("policy"),
                    Token("ingestiontime"),
                    Token("true"),
                    shape147));

            var DeleteTablePolicyIngestionTime = Command("DeleteTablePolicyIngestionTime", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("ingestiontime"),
                    shape103));

            var ShowTablePolicyRetention = Command("ShowTablePolicyRetention", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("retention"),
                    shape103));

            var ShowTableStarPolicyRetention = Command("ShowTableStarPolicyRetention", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    Token("policy"),
                    Token("retention")));

            var ShowGraphPolicyRetention = Command("ShowGraphPolicyRetention", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("graph_model"),
                    If(Not(Token("*")), rules.GraphModelNameReference),
                    Token("policy"),
                    Token("retention"),
                    shape106));

            var ShowGraphStarPolicyRetention = Command("ShowGraphStarPolicyRetention", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("graph_model"),
                    Token("*"),
                    Token("policy"),
                    Token("retention")));

            var ShowDatabasePolicyRetention = Command("ShowDatabasePolicyRetention", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            Best(
                                If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                                Token("*")),
                            shape9)),
                    Token("policy"),
                    Token("retention"),
                    shape94));

            var AlterTablePolicyRetention = Command("AlterTablePolicyRetention", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("retention"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("RetentionPolicy", CompletionHint.Literal)}));

            var AlterMaterializedViewPolicyRetention = Command("AlterMaterializedViewPolicyRetention", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    If(Not(Token("with")), rules.DatabaseMaterializedViewNameReference),
                    Token("policy"),
                    Token("retention"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD("RetentionPolicy", CompletionHint.Literal)}));

            var AlterDatabasePolicyRetention = Command("AlterDatabasePolicyRetention", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(And(Token("prettyname", "policy"))), rules.DatabaseNameReference),
                            shape9)),
                    Token("policy"),
                    Token("retention"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD(CompletionHint.Database, isOptional: true), CD(), CD(), CD("RetentionPolicy", CompletionHint.Literal)}));

            var AlterGraphModelPolicyRetention = Command("AlterGraphModelPolicyRetention", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("graph_model"),
                    rules.GraphModelNameReference,
                    Token("policy"),
                    Token("retention"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD("ModelName", CompletionHint.GraphModel), CD(), CD(), CD("RetentionPolicy", CompletionHint.Literal)}));

            var AlterTablesPolicyRetention = Command("AlterTablesPolicyRetention", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.TableNameReference,
                            shape26)),
                    Token(")"),
                    Token("policy"),
                    Token("retention"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(), CD("RetentionPolicy", CompletionHint.Literal)}));

            var AlterMergeTablePolicyRetention = Command("AlterMergeTablePolicyRetention", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("retention"),
                    Best(
                        Custom(
                            rules.StringLiteral,
                            shape107),
                        fragment20,
                        fragment19),
                    shape160));

            var AlterMergeMaterializedViewPolicyRetention = Command("AlterMergeMaterializedViewPolicyRetention", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.DatabaseMaterializedViewNameReference,
                    Token("policy"),
                    Token("retention"),
                    Best(
                        Custom(
                            rules.StringLiteral,
                            shape107),
                        fragment20,
                        fragment19),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD(CompletionHint.Literal)}));

            var AlterMergeDatabasePolicyRetention = Command("AlterMergeDatabasePolicyRetention", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(Token("policy")), rules.DatabaseNameReference),
                            shape9)),
                    Token("policy"),
                    Token("retention"),
                    Best(
                        Custom(
                            rules.StringLiteral,
                            shape107),
                        fragment20,
                        fragment19),
                    shape157));

            var DeleteTablePolicyRetention = Command("DeleteTablePolicyRetention", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("retention"),
                    shape103));

            var DeleteDatabasePolicyRetention = Command("DeleteDatabasePolicyRetention", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("retention"),
                    shape112));

            var ShowDatabasePolicyHardRetentionViolations = Command("ShowDatabasePolicyHardRetentionViolations", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("hardretention"),
                    Token("violations"),
                    shape113));

            var ShowDatabasePolicySoftRetentionViolations = Command("ShowDatabasePolicySoftRetentionViolations", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("softretention"),
                    Token("violations"),
                    shape113));

            var ShowTablePolicyRowLevelSecurity = Command("ShowTablePolicyRowLevelSecurity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("row_level_security"),
                    shape103));

            var ShowTableStarPolicyRowLevelSecurity = Command("ShowTableStarPolicyRowLevelSecurity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    Token("policy"),
                    Token("row_level_security")));

            var AlterTablePolicyRowLevelSecurity = Command("AlterTablePolicyRowLevelSecurity", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("row_level_security"),
                    Token("enable", "disable"),
                    Optional(
                        fragment10),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD(isOptional: true), CD("Query", CompletionHint.Literal)}));

            var DeleteTablePolicyRowLevelSecurity = Command("DeleteTablePolicyRowLevelSecurity", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("row_level_security"),
                    shape103));

            var ShowMaterializedViewPolicyRowLevelSecurity = Command("ShowMaterializedViewPolicyRowLevelSecurity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.DatabaseMaterializedViewNameReference,
                    Token("policy"),
                    Token("row_level_security"),
                    shape105));

            var AlterMaterializedViewPolicyRowLevelSecurity = Command("AlterMaterializedViewPolicyRowLevelSecurity", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    If(Not(Token("with")), rules.DatabaseMaterializedViewNameReference),
                    Token("policy"),
                    Token("row_level_security"),
                    Token("enable", "disable"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD(), CD("Query", CompletionHint.Literal)}));

            var DeleteMaterializedViewPolicyRowLevelSecurity = Command("DeleteMaterializedViewPolicyRowLevelSecurity", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.DatabaseMaterializedViewNameReference,
                    Token("policy"),
                    Token("row_level_security"),
                    shape105));

            var ShowTablePolicyRowOrder = Command("ShowTablePolicyRowOrder", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("roworder"),
                    shape103));

            var ShowTableStarPolicyRowOrder = Command("ShowTableStarPolicyRowOrder", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    Token("policy"),
                    Token("roworder")));

            var AlterTablePolicyRowOrder = Command("AlterTablePolicyRowOrder", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("roworder"),
                    Token("("),
                    OneOrMoreCommaList(
                        fragment21),
                    Token(")"),
                    shape116));

            var AlterTablesPolicyRowOrder = Command("AlterTablesPolicyRowOrder", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("tables"),
                        Token("("),
                        OneOrMoreCommaList(
                            Custom(
                                rules.TableNameReference,
                                shape26)),
                        Token(")"),
                        Token("policy"),
                        Token("roworder"),
                        Token("("),
                        OneOrMoreCommaList(
                            Custom(
                                rules.NameDeclaration,
                                Token("asc", "desc"),
                                new [] {CD("ColumnName", CompletionHint.None), CD()})),
                        Token(")")}
                    ,
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(), CD(), CD(CompletionHint.None), CD()}));

            var AlterMergeTablePolicyRowOrder = Command("AlterMergeTablePolicyRowOrder", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("roworder"),
                    Token("("),
                    OneOrMoreCommaList(
                        fragment21),
                    Token(")"),
                    shape116));

            var DeleteTablePolicyRowOrder = Command("DeleteTablePolicyRowOrder", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("roworder"),
                    shape103));

            var ShowTablePolicyUpdate = Command("ShowTablePolicyUpdate", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("update"),
                    shape103));

            var ShowTableStarPolicyUpdate = Command("ShowTableStarPolicyUpdate", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    Token("policy"),
                    Token("update")));

            var AlterTablePolicyUpdate = Command("AlterTablePolicyUpdate", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("update"),
                    rules.StringLiteral,
                    Optional(
                        fragment0),
                    shape118));

            var AlterMergeTablePolicyUpdate = Command("AlterMergeTablePolicyUpdate", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("update"),
                    rules.StringLiteral,
                    Optional(
                        fragment0),
                    shape118));

            var DeleteTablePolicyUpdate = Command("DeleteTablePolicyUpdate", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("update"),
                    shape103));

            var ShowClusterPolicyIngestionBatching = Command("ShowClusterPolicyIngestionBatching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("ingestionbatching")));

            var ShowDatabasePolicyIngestionBatching = Command("ShowDatabasePolicyIngestionBatching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            Best(
                                If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                                Token("*")),
                            shape9)),
                    Token("policy"),
                    Token("ingestionbatching"),
                    shape94));

            var ShowTablePolicyIngestionBatching = Command("ShowTablePolicyIngestionBatching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("ingestionbatching"),
                    shape103));

            var ShowTableStarPolicyIngestionBatching = Command("ShowTableStarPolicyIngestionBatching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    Token("policy"),
                    Token("ingestionbatching")));

            var AlterClusterPolicyIngestionBatching = Command("AlterClusterPolicyIngestionBatching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("ingestionbatching"),
                    rules.StringLiteral,
                    shape120));

            var AlterMergeClusterPolicyIngestionBatching = Command("AlterMergeClusterPolicyIngestionBatching", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("ingestionbatching"),
                    rules.StringLiteral,
                    shape120));

            var AlterDatabasePolicyIngestionBatching = Command("AlterDatabasePolicyIngestionBatching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(And(Token("prettyname", "policy"))), rules.DatabaseNameReference),
                            shape9)),
                    Token("policy"),
                    Token("ingestionbatching"),
                    rules.StringLiteral,
                    shape121));

            var AlterMergeDatabasePolicyIngestionBatching = Command("AlterMergeDatabasePolicyIngestionBatching", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(Token("policy")), rules.DatabaseNameReference),
                            shape9)),
                    Token("policy"),
                    Token("ingestionbatching"),
                    rules.StringLiteral,
                    shape121));

            var AlterTablePolicyIngestionBatching = Command("AlterTablePolicyIngestionBatching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("ingestionbatching"),
                    rules.StringLiteral,
                    shape122));

            var AlterMergeTablePolicyIngestionBatching = Command("AlterMergeTablePolicyIngestionBatching", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("ingestionbatching"),
                    rules.StringLiteral,
                    shape122));

            var AlterTablesPolicyIngestionBatching = Command("AlterTablesPolicyIngestionBatching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.TableNameReference,
                            shape26)),
                    Token(")"),
                    Token("policy"),
                    Token("ingestionbatching"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(), CD("IngestionBatchingPolicy", CompletionHint.Literal)}));

            var DeleteDatabasePolicyIngestionBatching = Command("DeleteDatabasePolicyIngestionBatching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("ingestionbatching"),
                    shape112));

            var DeleteTablePolicyIngestionBatching = Command("DeleteTablePolicyIngestionBatching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("ingestionbatching"),
                    shape103));

            var ShowDatabasePolicyEncoding = Command("ShowDatabasePolicyEncoding", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("encoding"),
                    shape112));

            var ShowTablePolicyEncoding = Command("ShowTablePolicyEncoding", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("encoding"),
                    shape103));

            var ShowColumnPolicyEncoding = Command("ShowColumnPolicyEncoding", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("column"),
                    If(Not(Token("*")), rules.TableColumnNameReference),
                    Token("policy"),
                    Token("encoding"),
                    shape104));

            var AlterDatabasePolicyEncoding = Command("AlterDatabasePolicyEncoding", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(And(Token("prettyname", "policy"))), rules.DatabaseNameReference),
                            shape9)),
                    Token("policy"),
                    Token("encoding"),
                    rules.StringLiteral,
                    shape124));

            var AlterTablePolicyEncoding = Command("AlterTablePolicyEncoding", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("encoding"),
                    rules.StringLiteral,
                    shape125));

            var AlterTableColumnsPolicyEncoding = Command("AlterTableColumnsPolicyEncoding", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("columns"),
                    Token("policy"),
                    Token("encoding"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD("EncodingPolicies", CompletionHint.Literal)}));

            var AlterColumnPolicyEncoding = Command("AlterColumnPolicyEncoding", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("column"),
                    rules.DatabaseTableColumnNameReference,
                    Token("policy"),
                    Token("encoding"),
                    rules.StringLiteral,
                    shape126));

            var AlterColumnsPolicyEncodingByQuery = Command("AlterColumnsPolicyEncodingByQuery", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("columns"),
                    Token("policy"),
                    Token("encoding"),
                    rules.StringLiteral,
                    Token("<|"),
                    rules.QueryInput,
                    new [] {CD(), CD(), CD(), CD(), CD("EncodingPolicy", CompletionHint.Literal), CD(), CD("QueryOrCommand", CompletionHint.NonScalar)}));

            var AlterColumnPolicyEncodingType = Command("AlterColumnPolicyEncodingType", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("column"),
                    rules.DatabaseTableColumnNameReference,
                    Token("policy"),
                    Token("encoding"),
                    Token("type"),
                    Token("="),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD(), CD(), CD(), CD("EncodingPolicyType", CompletionHint.Literal)}));

            var AlterMergeDatabasePolicyEncoding = Command("AlterMergeDatabasePolicyEncoding", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(Token("policy")), rules.DatabaseNameReference),
                            shape9)),
                    Token("policy"),
                    Token("encoding"),
                    rules.StringLiteral,
                    shape124));

            var AlterMergeTablePolicyEncoding = Command("AlterMergeTablePolicyEncoding", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("encoding"),
                    rules.StringLiteral,
                    shape125));

            var AlterMergeColumnPolicyEncoding = Command("AlterMergeColumnPolicyEncoding", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("column"),
                    rules.TableColumnNameReference,
                    Token("policy"),
                    Token("encoding"),
                    rules.StringLiteral,
                    shape126));

            var DeleteDatabasePolicyEncoding = Command("DeleteDatabasePolicyEncoding", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("encoding"),
                    shape112));

            var DeleteTablePolicyEncoding = Command("DeleteTablePolicyEncoding", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("encoding"),
                    shape103));

            var DeleteColumnPolicyEncoding = Command("DeleteColumnPolicyEncoding", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("column"),
                    rules.TableColumnNameReference,
                    Token("policy"),
                    Token("encoding"),
                    shape104));

            var ShowDatabasePolicyMerge = Command("ShowDatabasePolicyMerge", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            Best(
                                If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                                Token("*")),
                            shape9)),
                    Token("policy"),
                    Token("merge"),
                    shape94));

            var ShowTablePolicyMerge = Command("ShowTablePolicyMerge", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("merge"),
                    shape103));

            var ShowTableStarPolicyMerge = Command("ShowTableStarPolicyMerge", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    Token("policy"),
                    Token("merge")));

            var AlterDatabasePolicyMerge = Command("AlterDatabasePolicyMerge", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(And(Token("prettyname", "policy"))), rules.DatabaseNameReference),
                            shape9)),
                    Token("policy"),
                    Token("merge"),
                    rules.StringLiteral,
                    shape128));

            var AlterTablePolicyMerge = Command("AlterTablePolicyMerge", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("merge"),
                    rules.StringLiteral,
                    shape129));

            var AlterTablesPolicyMerge = Command("AlterTablesPolicyMerge", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.TableNameReference,
                            shape26)),
                    Token(")"),
                    Token("policy"),
                    Token("merge"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(), CD("policy", CompletionHint.Literal)}));

            var AlterMergeDatabasePolicyMerge = Command("AlterMergeDatabasePolicyMerge", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(Token("policy")), rules.DatabaseNameReference),
                            shape9)),
                    Token("policy"),
                    Token("merge"),
                    rules.StringLiteral,
                    shape128));

            var AlterMergeTablePolicyMerge = Command("AlterMergeTablePolicyMerge", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("merge"),
                    rules.StringLiteral,
                    shape129));

            var AlterMergeMaterializedViewPolicyMerge = Command("AlterMergeMaterializedViewPolicyMerge", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("policy"),
                    Token("merge"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD("MergePolicy", CompletionHint.Literal)}));

            var DeleteDatabasePolicyMerge = Command("DeleteDatabasePolicyMerge", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("merge"),
                    shape112));

            var DeleteTablePolicyMerge = Command("DeleteTablePolicyMerge", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("merge"),
                    shape103));

            var ShowExternalTablePolicyQueryAcceleration = Command("ShowExternalTablePolicyQueryAcceleration", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    If(Not(Token("*")), rules.ExternalTableNameReference),
                    Token("policy"),
                    Token("query_acceleration"),
                    shape132));

            var ShowExternalTablesPolicyQueryAcceleration = Command("ShowExternalTablesPolicyQueryAcceleration", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    Token("*"),
                    Token("policy"),
                    Token("query_acceleration")));

            var AlterExternalTablePolicyQueryAcceleration = Command("AlterExternalTablePolicyQueryAcceleration", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    rules.ExternalTableNameReference,
                    Token("policy"),
                    Token("query_acceleration"),
                    rules.StringLiteral,
                    shape131));

            var AlterMergeExternalTablePolicyQueryAcceleration = Command("AlterMergeExternalTablePolicyQueryAcceleration", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    rules.ExternalTableNameReference,
                    Token("policy"),
                    Token("query_acceleration"),
                    rules.StringLiteral,
                    shape131));

            var DeleteExternalTablePolicyQueryAcceleration = Command("DeleteExternalTablePolicyQueryAcceleration", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    rules.ExternalTableNameReference,
                    Token("policy"),
                    Token("query_acceleration"),
                    shape132));

            var ShowExternalTableQueryAccelerationStatatistics = Command("ShowExternalTableQueryAccelerationStatatistics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    If(Not(Token("*")), rules.ExternalTableNameReference),
                    Token("operations"),
                    Token("query_acceleration"),
                    Token("statistics"),
                    shape133));

            var ShowExternalTablesQueryAccelerationStatatistics = Command("ShowExternalTablesQueryAccelerationStatatistics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("tables"),
                    Token("operations"),
                    Token("query_acceleration"),
                    Token("statistics")));

            var AlterTablePolicyMirroring = Command("AlterTablePolicyMirroring", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("table"),
                        rules.DatabaseTableNameReference,
                        Token("policy"),
                        Token("mirroring"),
                        Optional(
                            fragment25),
                        Token("kind"),
                        Token("="),
                        Token("delta"),
                        Optional(
                            fragment26),
                        Optional(
                            fragment12)}
                    ,
                    shape138));

            var AlterMergeTablePolicyMirroring = Command("AlterMergeTablePolicyMirroring", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter-merge", CompletionKind.CommandPrefix),
                        Token("table"),
                        rules.DatabaseTableNameReference,
                        Token("policy"),
                        Token("mirroring"),
                        Optional(
                            fragment25),
                        Token("kind"),
                        Token("="),
                        Token("delta"),
                        Optional(
                            fragment26),
                        Optional(
                            fragment12)}
                    ,
                    shape138));

            var AlterTablePolicyMirroringWithJson = Command("AlterTablePolicyMirroringWithJson", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("mirroring"),
                    rules.StringLiteral,
                    shape139));

            var AlterMergeTablePolicyMirroringWithJson = Command("AlterMergeTablePolicyMirroringWithJson", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("mirroring"),
                    rules.StringLiteral,
                    shape139));

            var DeleteTablePolicyMirroring = Command("DeleteTablePolicyMirroring", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("mirroring"),
                    shape103));

            var ShowTablePolicyMirroring = Command("ShowTablePolicyMirroring", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("mirroring"),
                    shape103));

            var ShowTableStarPolicyMirroring = Command("ShowTableStarPolicyMirroring", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    Token("policy"),
                    Token("mirroring")));

            var CreateMirroringTemplate = Command("CreateMirroringTemplate", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("mirroring-template"),
                    rules.NameDeclaration,
                    Optional(
                        fragment27),
                    Optional(
                        fragment28),
                    Optional(
                        fragment29),
                    shape144));

            var CreateOrAlterMirroringTemplate = Command("CreateOrAlterMirroringTemplate", 
                Custom(
                    Token("create-or-alter", CompletionKind.CommandPrefix),
                    Token("mirroring-template"),
                    rules.NameDeclaration,
                    Optional(
                        fragment27),
                    Optional(
                        fragment28),
                    Optional(
                        fragment29),
                    shape144));

            var AlterMirroringTemplate = Command("AlterMirroringTemplate", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("mirroring-template"),
                    rules.NameDeclaration,
                    Optional(
                        fragment27),
                    Optional(
                        fragment28),
                    Optional(
                        fragment29),
                    shape144));

            var AlterMergeMirroringTemplate = Command("AlterMergeMirroringTemplate", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("mirroring-template"),
                    rules.NameDeclaration,
                    Optional(
                        fragment27),
                    Optional(
                        fragment28),
                    Optional(
                        fragment29),
                    shape144));

            var DeleteMirroringTemplate = Command("DeleteMirroringTemplate", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("mirroring-template"),
                    rules.NameDeclaration,
                    shape145));

            var ShowMirroringTemplate = Command("ShowMirroringTemplate", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("mirroring-template"),
                    rules.NameDeclaration,
                    shape145));

            var ShowMirroringTemplates = Command("ShowMirroringTemplates", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("mirroring-templates")));

            var ApplyMirroringTemplate = Command("ApplyMirroringTemplate", 
                Custom(
                    Token("apply", CompletionKind.CommandPrefix),
                    Token("mirroring-template"),
                    rules.NameDeclaration,
                    Best(
                        fragment48,
                        OneOrMoreCommaList(
                            Custom(
                                If(Not(Token("<|")), rules.TableNameReference),
                                shape26))),
                    new [] {CD(), CD(), CD("TemplateName", CompletionHint.None), CD()}));

            var ShowTablePolicyPartitioning = Command("ShowTablePolicyPartitioning", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("partitioning"),
                    shape103));

            var ShowTableStarPolicyPartitioning = Command("ShowTableStarPolicyPartitioning", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    Token("policy"),
                    Token("partitioning")));

            var AlterTablePolicyPartitioning = Command("AlterTablePolicyPartitioning", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("partitioning"),
                    rules.StringLiteral,
                    shape139));

            var AlterMergeTablePolicyPartitioning = Command("AlterMergeTablePolicyPartitioning", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("partitioning"),
                    rules.StringLiteral,
                    shape139));

            var AlterMaterializedViewPolicyPartitioning = Command("AlterMaterializedViewPolicyPartitioning", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    If(Not(Token("with")), rules.DatabaseMaterializedViewNameReference),
                    Token("policy"),
                    Token("partitioning"),
                    rules.StringLiteral,
                    shape146));

            var AlterMergeMaterializedViewPolicyPartitioning = Command("AlterMergeMaterializedViewPolicyPartitioning", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.DatabaseMaterializedViewNameReference,
                    Token("policy"),
                    Token("partitioning"),
                    rules.StringLiteral,
                    shape146));

            var DeleteTablePolicyPartitioning = Command("DeleteTablePolicyPartitioning", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("partitioning"),
                    shape103));

            var DeleteMaterializedViewPolicyPartitioning = Command("DeleteMaterializedViewPolicyPartitioning", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.DatabaseMaterializedViewNameReference,
                    Token("policy"),
                    Token("partitioning"),
                    shape105));

            var ShowTablePolicyRestrictedViewAccess = Command("ShowTablePolicyRestrictedViewAccess", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("restricted_view_access"),
                    shape103));

            var ShowTableStarPolicyRestrictedViewAccess = Command("ShowTableStarPolicyRestrictedViewAccess", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    Token("policy"),
                    Token("restricted_view_access")));

            var AlterTablePolicyRestrictedViewAccess = Command("AlterTablePolicyRestrictedViewAccess", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("restricted_view_access"),
                    Token("true", "false"),
                    shape100));

            var AlterTablesPolicyRestrictedViewAccess = Command("AlterTablesPolicyRestrictedViewAccess", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.TableNameReference,
                            shape26)),
                    Token(")"),
                    Token("policy"),
                    Token("restricted_view_access"),
                    Token("true", "false"),
                    shape147));

            var DeleteTablePolicyRestrictedViewAccess = Command("DeleteTablePolicyRestrictedViewAccess", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("restricted_view_access"),
                    shape103));

            var ShowClusterPolicyRowStore = Command("ShowClusterPolicyRowStore", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("rowstore")));

            var AlterClusterPolicyRowStore = Command("AlterClusterPolicyRowStore", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("rowstore"),
                    rules.StringLiteral,
                    shape149));

            var AlterMergeClusterPolicyRowStore = Command("AlterMergeClusterPolicyRowStore", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("rowstore"),
                    rules.StringLiteral,
                    shape149));

            var DeleteClusterPolicyRowStore = Command("DeleteClusterPolicyRowStore", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("rowstore")));

            var ShowClusterPolicySandbox = Command("ShowClusterPolicySandbox", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("sandbox")));

            var AlterClusterPolicySandbox = Command("AlterClusterPolicySandbox", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("sandbox"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD(), CD(), CD("SandboxPolicy", CompletionHint.Literal)}));

            var DeleteClusterPolicySandbox = Command("DeleteClusterPolicySandbox", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("sandbox")));

            var ShowClusterSandboxesStats = Command("ShowClusterSandboxesStats", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("sandboxes"),
                    Token("stats")));

            var ShowDatabasePolicySharding = Command("ShowDatabasePolicySharding", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            Best(
                                If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                                Token("*")),
                            shape9)),
                    Token("policy"),
                    Token("sharding"),
                    shape94));

            var ShowTablePolicySharding = Command("ShowTablePolicySharding", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("sharding"),
                    shape103));

            var ShowTableStarPolicySharding = Command("ShowTableStarPolicySharding", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    Token("policy"),
                    Token("sharding")));

            var AlterDatabasePolicySharding = Command("AlterDatabasePolicySharding", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(And(Token("prettyname", "policy"))), rules.DatabaseNameReference),
                            shape9)),
                    Token("policy"),
                    Token("sharding"),
                    rules.StringLiteral,
                    shape151));

            var AlterTablePolicySharding = Command("AlterTablePolicySharding", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("sharding"),
                    rules.StringLiteral,
                    shape152));

            var AlterMergeDatabasePolicySharding = Command("AlterMergeDatabasePolicySharding", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(Token("policy")), rules.DatabaseNameReference),
                            shape9)),
                    Token("policy"),
                    Token("sharding"),
                    rules.StringLiteral,
                    shape151));

            var AlterMergeTablePolicySharding = Command("AlterMergeTablePolicySharding", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("sharding"),
                    rules.StringLiteral,
                    shape152));

            var DeleteDatabasePolicySharding = Command("DeleteDatabasePolicySharding", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("sharding"),
                    shape112));

            var DeleteTablePolicySharding = Command("DeleteTablePolicySharding", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("sharding"),
                    shape103));

            var AlterClusterPolicySharding = Command("AlterClusterPolicySharding", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("sharding"),
                    rules.StringLiteral,
                    shape154));

            var AlterMergeClusterPolicySharding = Command("AlterMergeClusterPolicySharding", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("sharding"),
                    rules.StringLiteral,
                    shape154));

            var DeleteClusterPolicySharding = Command("DeleteClusterPolicySharding", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("sharding")));

            var ShowClusterPolicySharding = Command("ShowClusterPolicySharding", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("sharding"),
                    Optional(
                        fragment0),
                    shape172));

            var ShowDatabasePolicyShardsGrouping = Command("ShowDatabasePolicyShardsGrouping", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            Best(
                                If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                                Token("*")),
                            shape9)),
                    Token("policy"),
                    Token("shards_grouping").Hide(),
                    shape94));

            var AlterDatabasePolicyShardsGrouping = Command("AlterDatabasePolicyShardsGrouping", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(And(Token("prettyname", "policy"))), rules.DatabaseNameReference),
                            shape9)),
                    Token("policy"),
                    Token("shards_grouping").Hide(),
                    rules.StringLiteral,
                    shape156));

            var AlterMergeDatabasePolicyShardsGrouping = Command("AlterMergeDatabasePolicyShardsGrouping", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(Token("policy")), rules.DatabaseNameReference),
                            shape9)),
                    Token("policy"),
                    Token("shards_grouping").Hide(),
                    rules.StringLiteral,
                    shape156));

            var DeleteDatabasePolicyShardsGrouping = Command("DeleteDatabasePolicyShardsGrouping", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("shards_grouping").Hide(),
                    shape112));

            var ShowDatabasePolicyStreamingIngestion = Command("ShowDatabasePolicyStreamingIngestion", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("streamingingestion"),
                    shape112));

            var ShowTablePolicyStreamingIngestion = Command("ShowTablePolicyStreamingIngestion", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("streamingingestion"),
                    shape103));

            var ShowClusterPolicyStreamingIngestion = Command("ShowClusterPolicyStreamingIngestion", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("streamingingestion")));

            var AlterDatabasePolicyStreamingIngestion = Command("AlterDatabasePolicyStreamingIngestion", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(And(Token("prettyname", "policy"))), rules.DatabaseNameReference),
                            shape9)),
                    Token("policy"),
                    Token("streamingingestion"),
                    Best(
                        Custom(
                            rules.StringLiteral,
                            shape158),
                        Custom(
                            Token("enable", "disable"),
                            shape159)),
                    shape157));

            var AlterMergeDatabasePolicyStreamingIngestion = Command("AlterMergeDatabasePolicyStreamingIngestion", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(Token("policy")), rules.DatabaseNameReference),
                            shape9)),
                    Token("policy"),
                    Token("streamingingestion"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD(CompletionHint.Database, isOptional: true), CD(), CD(), CD("StreamingIngestionPolicy", CompletionHint.Literal)}));

            var AlterTablePolicyStreamingIngestion = Command("AlterTablePolicyStreamingIngestion", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("streamingingestion"),
                    Best(
                        Custom(
                            rules.StringLiteral,
                            shape158),
                        Custom(
                            Token("enable", "disable"),
                            shape159)),
                    shape160));

            var AlterMergeTablePolicyStreamingIngestion = Command("AlterMergeTablePolicyStreamingIngestion", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("streamingingestion"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("StreamingIngestionPolicy", CompletionHint.Literal)}));

            var AlterClusterPolicyStreamingIngestion = Command("AlterClusterPolicyStreamingIngestion", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("streamingingestion"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD(), CD(), CD("StreamingIngestionPolicy", CompletionHint.Literal)}));

            var AlterMergeClusterPolicyStreamingIngestion = Command("AlterMergeClusterPolicyStreamingIngestion", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("streamingingestion"),
                    rules.StringLiteral,
                    shape154));

            var DeleteDatabasePolicyStreamingIngestion = Command("DeleteDatabasePolicyStreamingIngestion", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("streamingingestion"),
                    shape112));

            var DeleteTablePolicyStreamingIngestion = Command("DeleteTablePolicyStreamingIngestion", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("streamingingestion"),
                    shape103));

            var DeleteClusterPolicyStreamingIngestion = Command("DeleteClusterPolicyStreamingIngestion", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("streamingingestion")));

            var ShowDatabasePolicyManagedIdentity = Command("ShowDatabasePolicyManagedIdentity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("managed_identity"),
                    shape112));

            var ShowClusterPolicyManagedIdentity = Command("ShowClusterPolicyManagedIdentity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("managed_identity")));

            var AlterDatabasePolicyManagedIdentity = Command("AlterDatabasePolicyManagedIdentity", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(And(Token("prettyname", "policy"))), rules.DatabaseNameReference),
                            shape9)),
                    Token("policy"),
                    Token("managed_identity"),
                    rules.StringLiteral,
                    shape162));

            var AlterMergeDatabasePolicyManagedIdentity = Command("AlterMergeDatabasePolicyManagedIdentity", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(Token("policy")), rules.DatabaseNameReference),
                            shape9)),
                    Token("policy"),
                    Token("managed_identity"),
                    rules.StringLiteral,
                    shape162));

            var AlterClusterPolicyManagedIdentity = Command("AlterClusterPolicyManagedIdentity", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("managed_identity"),
                    rules.StringLiteral,
                    shape163));

            var AlterMergeClusterPolicyManagedIdentity = Command("AlterMergeClusterPolicyManagedIdentity", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("managed_identity"),
                    rules.StringLiteral,
                    shape163));

            var DeleteDatabasePolicyManagedIdentity = Command("DeleteDatabasePolicyManagedIdentity", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("managed_identity"),
                    shape112));

            var DeleteClusterPolicyManagedIdentity = Command("DeleteClusterPolicyManagedIdentity", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("managed_identity")));

            var ShowTablePolicyAutoDelete = Command("ShowTablePolicyAutoDelete", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("auto_delete"),
                    shape103));

            var AlterTablePolicyAutoDelete = Command("AlterTablePolicyAutoDelete", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("auto_delete"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("AutoDeletePolicy", CompletionHint.Literal)}));

            var DeleteTablePolicyAutoDelete = Command("DeleteTablePolicyAutoDelete", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("auto_delete"),
                    shape103));

            var ShowClusterPolicyCallout = Command("ShowClusterPolicyCallout", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("callout")));

            var AlterClusterPolicyCallout = Command("AlterClusterPolicyCallout", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("callout"),
                    rules.StringLiteral,
                    shape164));

            var AlterMergeClusterPolicyCallout = Command("AlterMergeClusterPolicyCallout", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("callout"),
                    rules.StringLiteral,
                    shape164));

            var DeleteClusterPolicyCallout = Command("DeleteClusterPolicyCallout", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("callout")));

            var ShowClusterPolicyCapacity = Command("ShowClusterPolicyCapacity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("capacity")));

            var AlterClusterPolicyCapacity = Command("AlterClusterPolicyCapacity", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("capacity"),
                    rules.StringLiteral,
                    shape164));

            var AlterMergeClusterPolicyCapacity = Command("AlterMergeClusterPolicyCapacity", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("capacity"),
                    rules.StringLiteral,
                    shape164));

            var ShowClusterPolicyRequestClassification = Command("ShowClusterPolicyRequestClassification", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("request_classification")));

            var AlterClusterPolicyRequestClassification = Command("AlterClusterPolicyRequestClassification", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("request_classification"),
                    rules.StringLiteral,
                    Token("<|"),
                    rules.QueryInput,
                    new [] {CD(), CD(), CD(), CD(), CD("Policy", CompletionHint.Literal), CD(), CD("Query", CompletionHint.NonScalar)}));

            var AlterMergeClusterPolicyRequestClassification = Command("AlterMergeClusterPolicyRequestClassification", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("request_classification"),
                    rules.StringLiteral,
                    shape164));

            var DeleteClusterPolicyRequestClassification = Command("DeleteClusterPolicyRequestClassification", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("request_classification")));

            var ShowClusterPolicyMultiDatabaseAdmins = Command("ShowClusterPolicyMultiDatabaseAdmins", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("multidatabaseadmins")));

            var AlterClusterPolicyMultiDatabaseAdmins = Command("AlterClusterPolicyMultiDatabaseAdmins", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("multidatabaseadmins"),
                    rules.StringLiteral,
                    shape164));

            var AlterMergeClusterPolicyMultiDatabaseAdmins = Command("AlterMergeClusterPolicyMultiDatabaseAdmins", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("multidatabaseadmins"),
                    rules.StringLiteral,
                    shape164));

            var ShowDatabasePolicyDiagnostics = Command("ShowDatabasePolicyDiagnostics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("diagnostics"),
                    shape112));

            var ShowClusterPolicyDiagnostics = Command("ShowClusterPolicyDiagnostics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("diagnostics")));

            var AlterDatabasePolicyDiagnostics = Command("AlterDatabasePolicyDiagnostics", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(And(Token("prettyname", "policy"))), rules.DatabaseNameReference),
                            shape9)),
                    Token("policy"),
                    Token("diagnostics"),
                    rules.StringLiteral,
                    shape167));

            var AlterMergeDatabasePolicyDiagnostics = Command("AlterMergeDatabasePolicyDiagnostics", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(Token("policy")), rules.DatabaseNameReference),
                            shape9)),
                    Token("policy"),
                    Token("diagnostics"),
                    rules.StringLiteral,
                    shape167));

            var AlterClusterPolicyDiagnostics = Command("AlterClusterPolicyDiagnostics", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("diagnostics"),
                    rules.StringLiteral,
                    shape168));

            var AlterMergeClusterPolicyDiagnostics = Command("AlterMergeClusterPolicyDiagnostics", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("diagnostics"),
                    rules.StringLiteral,
                    shape168));

            var DeleteDatabasePolicyDiagnostics = Command("DeleteDatabasePolicyDiagnostics", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("diagnostics"),
                    shape112));

            var ShowClusterPolicyQueryWeakConsistency = Command("ShowClusterPolicyQueryWeakConsistency", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("query_weak_consistency")));

            var AlterClusterPolicyQueryWeakConsistency = Command("AlterClusterPolicyQueryWeakConsistency", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("query_weak_consistency"),
                    rules.StringLiteral,
                    shape164));

            var AlterMergeClusterPolicyQueryWeakConsistency = Command("AlterMergeClusterPolicyQueryWeakConsistency", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("query_weak_consistency"),
                    rules.StringLiteral,
                    shape164));

            var ShowTablePolicyExtentTagsRetention = Command("ShowTablePolicyExtentTagsRetention", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("extent_tags_retention"),
                    shape103));

            var ShowTableStarPolicyExtentTagsRetention = Command("ShowTableStarPolicyExtentTagsRetention", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    Token("policy"),
                    Token("extent_tags_retention")));

            var ShowDatabasePolicyExtentTagsRetention = Command("ShowDatabasePolicyExtentTagsRetention", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            Best(
                                If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                                Token("*")),
                            shape9)),
                    Token("policy"),
                    Token("extent_tags_retention"),
                    shape94));

            var AlterTablePolicyExtentTagsRetention = Command("AlterTablePolicyExtentTagsRetention", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("extent_tags_retention"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("ExtentTagsRetentionPolicy", CompletionHint.Literal)}));

            var AlterDatabasePolicyExtentTagsRetention = Command("AlterDatabasePolicyExtentTagsRetention", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(And(Token("prettyname", "policy"))), rules.DatabaseNameReference),
                            shape9)),
                    Token("policy"),
                    Token("extent_tags_retention"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD(CompletionHint.Database, isOptional: true), CD(), CD(), CD("ExtentTagsRetentionPolicy", CompletionHint.Literal)}));

            var DeleteTablePolicyExtentTagsRetention = Command("DeleteTablePolicyExtentTagsRetention", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("extent_tags_retention"),
                    shape103));

            var DeleteDatabasePolicyExtentTagsRetention = Command("DeleteDatabasePolicyExtentTagsRetention", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("extent_tags_retention"),
                    shape112));

            var ShowPrincipalRoles = Command("ShowPrincipalRoles", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("principal"),
                    Best(
                        Token("roles"),
                        fragment30),
                    Optional(
                        fragment0),
                    shape4));

            var ShowDatabasePrincipalRoles = Command("ShowDatabasePrincipalRoles", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                    Token("principal"),
                    Best(
                        Token("roles"),
                        fragment30),
                    Optional(
                        fragment0),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD(isOptional: true)}));

            var ShowTablePrincipalRoles = Command("ShowTablePrincipalRoles", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("principal"),
                    Best(
                        Token("roles"),
                        fragment30),
                    Optional(
                        fragment0),
                    shape286));

            var ShowGraphModelPrincipalRoles = Command("ShowGraphModelPrincipalRoles", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("graph_model"),
                    If(Not(Token("*")), rules.GraphModelNameReference),
                    Token("principal"),
                    Best(
                        Token("roles"),
                        fragment30),
                    Optional(
                        fragment0),
                    new [] {CD(), CD(), CD(CompletionHint.GraphModel), CD(), CD(), CD(isOptional: true)}));

            var ShowExternalTablesPrincipalRoles = Command("ShowExternalTablesPrincipalRoles", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    If(Not(Token("*")), rules.ExternalTableNameReference),
                    Token("principal"),
                    Best(
                        Token("roles"),
                        fragment30),
                    Optional(
                        fragment0),
                    new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD(), CD(isOptional: true)}));

            var ShowFunctionPrincipalRoles = Command("ShowFunctionPrincipalRoles", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("function"),
                    rules.FunctionNameReference,
                    Token("principal"),
                    Best(
                        Token("roles"),
                        fragment30),
                    Optional(
                        fragment0),
                    new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD(), CD(), CD(isOptional: true)}));

            var ShowClusterPrincipalRoles = Command("ShowClusterPrincipalRoles", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("principal"),
                    Best(
                        Token("roles"),
                        fragment30),
                    Optional(
                        fragment0),
                    shape172));

            var ShowClusterPrincipals = Command("ShowClusterPrincipals", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("principals")));

            var ShowDatabasePrincipals = Command("ShowDatabasePrincipals", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                            shape9)),
                    Token("principals"),
                    new [] {CD(), CD(), CD(CompletionHint.Database, isOptional: true), CD()}));

            var ShowTablePrincipals = Command("ShowTablePrincipals", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("principals"),
                    shape28));

            var ShowGraphModelPrincipals = Command("ShowGraphModelPrincipals", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("graph_model"),
                    If(Not(Token("*")), rules.GraphModelNameReference),
                    Token("principals"),
                    shape229));

            var ShowFunctionPrincipals = Command("ShowFunctionPrincipals", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("function"),
                    rules.FunctionNameReference,
                    Token("principals"),
                    new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD()}));

            var AddClusterRole = Command("AddClusterRole", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("admins", "alldatabasesadmins", "alldatabasesviewers", "alldatabasesmonitors", "databasecreators", "monitors", "ops", "users", "viewers"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.StringLiteral,
                            shape170)),
                    Token(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape174)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape175)),
                    shape176));

            var DropClusterRole = Command("DropClusterRole", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("admins", "alldatabasesadmins", "alldatabasesviewers", "alldatabasesmonitors", "databasecreators", "monitors", "ops", "users", "viewers"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.StringLiteral,
                            shape170)),
                    Token(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape174)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape175)),
                    shape176));

            var SetClusterRole = Command("SetClusterRole", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("admins", "alldatabasesadmins", "alldatabasesviewers", "alldatabasesmonitors", "databasecreators", "monitors", "ops", "users", "viewers"),
                    Best(
                        fragment31,
                        fragment32),
                    new [] {CD(), CD(), CD("Role"), CD()}));

            var AddDatabaseRole = Command("AddDatabaseRole", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(And(Token("admins", "ingestors", "monitors", "unrestrictedviewers", "users", "viewers"))), rules.DatabaseNameReference),
                            shape9)),
                    Token("admins", "ingestors", "monitors", "unrestrictedviewers", "users", "viewers"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.StringLiteral,
                            shape170)),
                    Token(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape174)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape175)),
                    new [] {CD(), CD(), CD(CompletionHint.Database, isOptional: true), CD("Role"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true), CD(CompletionHint.Literal, isOptional: true)}));

            var DropDatabaseRole = Command("DropDatabaseRole", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("admins", "ingestors", "monitors", "unrestrictedviewers", "users", "viewers"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.StringLiteral,
                            shape170)),
                    Token(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape174)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape175)),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD("Role"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true), CD(CompletionHint.Literal, isOptional: true)}));

            var SetDatabaseRole = Command("SetDatabaseRole", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("prettyname")), rules.DatabaseNameReference),
                    Token("admins", "ingestors", "monitors", "unrestrictedviewers", "users", "viewers"),
                    Best(
                        fragment31,
                        fragment32),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD("Role"), CD()}));

            var AddTableRole = Command("AddTableRole", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("admins", "ingestors"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.StringLiteral,
                            shape170)),
                    Token(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape174)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape175)),
                    shape179));

            var DropTableRole = Command("DropTableRole", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("admins", "ingestors"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.StringLiteral,
                            shape170)),
                    Token(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape174)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape175)),
                    shape179));

            var SetTableRole = Command("SetTableRole", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("admins", "ingestors"),
                    Best(
                        fragment31,
                        fragment32),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD("Role"), CD()}));

            var AddFunctionRole = Command("AddFunctionRole", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("function"),
                    rules.FunctionNameReference,
                    Token("admins"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.StringLiteral,
                            shape170)),
                    Token(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape174)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape175)),
                    shape180));

            var DropFunctionRole = Command("DropFunctionRole", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("function"),
                    rules.FunctionNameReference,
                    Token("admins"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.StringLiteral,
                            shape170)),
                    Token(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape174)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape175)),
                    shape180));

            var SetFunctionRole = Command("SetFunctionRole", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("function"),
                    rules.FunctionNameReference,
                    Token("admins"),
                    Best(
                        fragment31,
                        fragment32),
                    new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD("Role"), CD()}));

            var ShowClusterBlockedPrincipals = Command("ShowClusterBlockedPrincipals", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("blockedprincipals")));

            var AddClusterBlockedPrincipals = Command("AddClusterBlockedPrincipals", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("blockedprincipals"),
                    rules.StringLiteral,
                    Optional(
                        fragment33),
                    Optional(
                        fragment34),
                    Optional(
                        Custom(
                            Token("period"),
                            rules.Value,
                            new [] {CD(), CD("Period", CompletionHint.Literal)})),
                    Optional(
                        Custom(
                            Token("reason"),
                            rules.StringLiteral,
                            new [] {CD(), CD("Reason", CompletionHint.Literal)})),
                    new [] {CD(), CD(), CD(), CD("Principal", CompletionHint.Literal), CD(isOptional: true), CD(isOptional: true), CD(isOptional: true), CD(isOptional: true)}));

            var DropClusterBlockedPrincipals = Command("DropClusterBlockedPrincipals", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("blockedprincipals"),
                    rules.StringLiteral,
                    Optional(
                        fragment33),
                    Optional(
                        fragment34),
                    new [] {CD(), CD(), CD(), CD("Principal", CompletionHint.Literal), CD(isOptional: true), CD(isOptional: true)}));

            var SetClusterMaintenanceMode = Command("SetClusterMaintenanceMode", 
                Custom(
                    Token("enable", "disable"),
                    Token("cluster"),
                    Token("maintenance_mode")));

            var IngestIntoTable = Command("IngestIntoTable", 
                Custom(
                    Token("ingest", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    Token("into"),
                    Token("table"),
                    rules.TableNameReference,
                    Best(
                        Custom(
                            rules.StringLiteral,
                            shape8),
                        Custom(
                            Token("("),
                            OneOrMoreCommaList(
                                Custom(
                                    rules.StringLiteral,
                                    shape8)),
                            Token(")"),
                            shape137)),
                    Optional(
                        fragment36),
                    new [] {CD(), CD(isOptional: true), CD(), CD(), CD("TableName", CompletionHint.Table), CD(CompletionHint.Literal), CD(isOptional: true)}));

            var IngestInlineIntoTable = Command("IngestInlineIntoTable", 
                Custom(
                    Token("ingest", CompletionKind.CommandPrefix),
                    Token("inline"),
                    Token("into"),
                    Token("table"),
                    rules.NameDeclaration,
                    Best(
                        Custom(
                            Token("["),
                            rules.BracketedInputText,
                            Token("]"),
                            new [] {CD(), CD("Data", CompletionHint.None), CD()}),
                        Custom(
                            Token("with"),
                            Token("("),
                            OneOrMoreCommaList(
                                fragment35),
                            Token(")"),
                            Token("<|"),
                            rules.InputText,
                            new [] {CD(), CD(), CD(), CD(), CD(), CD("Data", CompletionHint.None)}),
                        Custom(
                            Token("<|"),
                            rules.InputText,
                            new [] {CD(), CD("Data", CompletionHint.None)})),
                    new [] {CD(), CD(), CD(), CD(), CD("TableName", CompletionHint.None), CD()}));

            var SetTable = Command("SetTable", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    If(Not(And(Token("database", "access", "external", "cluster", "table", "function", "async", "continuous-export", "materialized-view", "ifnotexists", "stored_query_result", "graph_model"))), rules.NameDeclaration),
                    Optional(
                        fragment36),
                    Token("<|"),
                    rules.QueryInput,
                    shape187));

            var AppendTable = Command("AppendTable", 
                Custom(
                    Token("append", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    If(Not(Token("async")), rules.TableNameReference),
                    Optional(
                        fragment36),
                    Token("<|"),
                    rules.QueryInput,
                    new [] {CD(), CD(isOptional: true), CD("TableName", CompletionHint.Table), CD(isOptional: true), CD(), CD("QueryOrCommand", CompletionHint.NonScalar)}));

            var SetOrAppendTable = Command("SetOrAppendTable", 
                Custom(
                    Token("set-or-append", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    If(Not(Token("async")), rules.NameDeclaration),
                    Optional(
                        fragment36),
                    Token("<|"),
                    rules.QueryInput,
                    shape187));

            var SetOrReplaceTable = Command("SetOrReplaceTable", 
                Custom(
                    Token("set-or-replace", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    If(Not(And(Token("async", "stored_query_result"))), rules.NameDeclaration),
                    Optional(
                        fragment36),
                    Token("<|"),
                    rules.QueryInput,
                    shape187));

            var ExportToStorage = Command("ExportToStorage", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("export", CompletionKind.CommandPrefix),
                        Optional(Token("async")),
                        Optional(Token("compressed")),
                        Token("to"),
                        Token("csv", "tsv", "json", "parquet"),
                        Token("("),
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                CD("DataConnectionString", CompletionHint.Literal))),
                        Token(")"),
                        Optional(
                            fragment0),
                        Token("<|"),
                        rules.QueryInput}
                    ,
                    new [] {CD(), CD(isOptional: true), CD(isOptional: true), CD(), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true), CD(), CD("Query", CompletionHint.NonScalar)}));

            var ExportToSqlTable = Command("ExportToSqlTable", 
                Custom(
                    Token("export", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    Token("to"),
                    Token("sql"),
                    rules.NameDeclaration,
                    rules.StringLiteral,
                    Optional(
                        fragment0),
                    Token("<|"),
                    rules.QueryInput,
                    new [] {CD(), CD(isOptional: true), CD(), CD(), CD("SqlTableName", CompletionHint.None), CD("SqlConnectionString", CompletionHint.Literal), CD(isOptional: true), CD(), CD("Query", CompletionHint.NonScalar)}));

            var ExportToExternalTable = Command("ExportToExternalTable", 
                Custom(
                    Token("export", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    Token("to"),
                    Token("table"),
                    rules.ExternalTableNameReference,
                    Optional(
                        fragment0),
                    Token("<|"),
                    rules.QueryInput,
                    new [] {CD(), CD(isOptional: true), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(isOptional: true), CD(), CD("Query", CompletionHint.NonScalar)}));

            var CreateOrAlterContinuousExport = Command("CreateOrAlterContinuousExport", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("create-or-alter", CompletionKind.CommandPrefix),
                        Token("continuous-export"),
                        rules.NameDeclaration,
                        Optional(
                            Custom(
                                Token("over"),
                                Token("("),
                                OneOrMoreCommaList(
                                    Custom(
                                        rules.NameDeclaration,
                                        shape29)),
                                Token(")"),
                                shape3)),
                        Token("to"),
                        Token("table"),
                        rules.ExternalTableNameReference,
                        Optional(
                            fragment0),
                        Token("<|"),
                        rules.QueryInput}
                    ,
                    new [] {CD(), CD(), CD("ContinuousExportName", CompletionHint.None), CD(isOptional: true), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(isOptional: true), CD(), CD("Query", CompletionHint.NonScalar)}));

            var ShowContinuousExport = Command("ShowContinuousExport", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("continuous-export"),
                    rules.NameDeclaration,
                    shape190));

            var ShowContinuousExports = Command("ShowContinuousExports", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("continuous-exports")));

            var ShowClusterPendingContinuousExports = Command("ShowClusterPendingContinuousExports", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("pending"),
                    Token("continuous-exports"),
                    Optional(
                        fragment0),
                    shape172));

            var ShowContinuousExportExportedArtifacts = Command("ShowContinuousExportExportedArtifacts", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("continuous-export"),
                    rules.NameDeclaration,
                    Token("exported-artifacts"),
                    shape189));

            var ShowContinuousExportFailures = Command("ShowContinuousExportFailures", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("continuous-export"),
                    rules.NameDeclaration,
                    Token("failures"),
                    shape189));

            var SetContinuousExportCursor = Command("SetContinuousExportCursor", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("continuous-export"),
                    rules.NameDeclaration,
                    Token("cursor"),
                    Token("to"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD("jobName", CompletionHint.None), CD(), CD(), CD("cursorValue", CompletionHint.Literal)}));

            var DropContinuousExport = Command("DropContinuousExport", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("continuous-export"),
                    rules.NameDeclaration,
                    shape190));

            var EnableContinuousExport = Command("EnableContinuousExport", 
                Custom(
                    Token("enable", CompletionKind.CommandPrefix),
                    Token("continuous-export"),
                    rules.NameDeclaration,
                    shape190));

            var DisableContinuousExport = Command("DisableContinuousExport", 
                Custom(
                    Token("disable", CompletionKind.CommandPrefix),
                    Token("continuous-export"),
                    rules.NameDeclaration,
                    new [] {CD(), CD(), CD("ContinousExportName", CompletionHint.None)}));

            var CreateMaterializedView = Command("CreateMaterializedView", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("create", CompletionKind.CommandPrefix),
                        Optional(Token("async")),
                        Optional(Token("ifnotexists")),
                        Token("materialized-view"),
                        Optional(
                            fragment37),
                        If(Not(Token("with")), rules.NameDeclaration),
                        Token("on"),
                        Token("table"),
                        rules.TableNameReference,
                        rules.FunctionBody}
                    ,
                    new [] {CD(), CD(isOptional: true), CD(isOptional: true), CD(), CD(isOptional: true), CD("MaterializedViewName", CompletionHint.None), CD(), CD(), CD(CompletionHint.Table), CD()}));

            var CreateMaterializedViewOverMaterializedView = Command("CreateMaterializedViewOverMaterializedView", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("create", CompletionKind.CommandPrefix),
                        Optional(Token("async")),
                        Optional(Token("ifnotexists")),
                        Token("materialized-view"),
                        Optional(
                            fragment37),
                        If(Not(Token("with")), rules.NameDeclaration),
                        Token("on"),
                        Token("materialized-view"),
                        rules.MaterializedViewNameReference,
                        rules.FunctionBody}
                    ,
                    new [] {CD(), CD(isOptional: true), CD(isOptional: true), CD(), CD(isOptional: true), CD("MaterializedViewName", CompletionHint.None), CD(), CD(), CD(CompletionHint.MaterializedView), CD()}));

            var RenameMaterializedView = Command("RenameMaterializedView", 
                Custom(
                    Token("rename", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("to"),
                    rules.NameDeclaration,
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD("NewMaterializedViewName", CompletionHint.None)}));

            var ShowMaterializedView = Command("ShowMaterializedView", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    shape195));

            var ShowMaterializedViews = Command("ShowMaterializedViews", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-views")));

            var ShowMaterializedViewsDetails = Command("ShowMaterializedViewsDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-views"),
                    Optional(
                        Custom(
                            Token("("),
                            OneOrMoreCommaList(
                                Custom(
                                    rules.MaterializedViewNameReference,
                                    shape101)),
                            Token(")"),
                            new [] {CD(), CD(CompletionHint.MaterializedView), CD()})),
                    Token("details"),
                    shape192));

            var ShowMaterializedViewDetails = Command("ShowMaterializedViewDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("details"),
                    shape196));

            var ShowMaterializedViewPolicyRetention = Command("ShowMaterializedViewPolicyRetention", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("policy"),
                    Token("retention"),
                    shape105));

            var ShowMaterializedViewPolicyMerge = Command("ShowMaterializedViewPolicyMerge", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("policy"),
                    Token("merge"),
                    shape105));

            var ShowMaterializedViewPolicyPartitioning = Command("ShowMaterializedViewPolicyPartitioning", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("policy"),
                    Token("partitioning"),
                    shape105));

            var ShowMaterializedViewExtents = Command("ShowMaterializedViewExtents", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("extents"),
                    Optional(
                        fragment40),
                    Optional(Token("hot")),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(isOptional: true), CD(isOptional: true)}));

            var AlterMaterializedView = Command("AlterMaterializedView", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Optional(
                        fragment38),
                    If(Not(Token("with")), rules.MaterializedViewNameReference),
                    Token("on"),
                    Token("table"),
                    rules.TableNameReference,
                    rules.FunctionBody,
                    shape193));

            var AlterMaterializedViewOverMaterializedView = Command("AlterMaterializedViewOverMaterializedView", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Optional(
                        fragment38),
                    If(Not(Token("with")), rules.MaterializedViewNameReference),
                    Token("on"),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    rules.FunctionBody,
                    shape194));

            var CreateOrAlterMaterializedView = Command("CreateOrAlterMaterializedView", 
                Custom(
                    Token("create-or-alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Optional(
                        fragment37),
                    If(Not(Token("with")), rules.MaterializedViewNameReference),
                    Token("on"),
                    Token("table"),
                    rules.TableNameReference,
                    rules.FunctionBody,
                    shape193));

            var CreateOrAlterMaterializedViewOverMaterializedView = Command("CreateOrAlterMaterializedViewOverMaterializedView", 
                Custom(
                    Token("create-or-alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Optional(
                        fragment37),
                    If(Not(Token("with")), rules.MaterializedViewNameReference),
                    Token("on"),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    rules.FunctionBody,
                    shape194));

            var DropMaterializedView = Command("DropMaterializedView", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    shape195));

            var EnableDisableMaterializedView = Command("EnableDisableMaterializedView", 
                Custom(
                    Token("enable", "disable"),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    shape195));

            var ShowMaterializedViewPrincipals = Command("ShowMaterializedViewPrincipals", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("principals"),
                    shape196));

            var ShowMaterializedViewSchemaAsJson = Command("ShowMaterializedViewSchemaAsJson", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("schema"),
                    Token("as"),
                    Token("json"),
                    shape197));

            var ShowMaterializedViewCslSchema = Command("ShowMaterializedViewCslSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("kqlschema", "cslschema"),
                    shape196));

            var AlterMaterializedViewFolder = Command("AlterMaterializedViewFolder", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    If(Not(Token("with")), rules.MaterializedViewNameReference),
                    Token("folder"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD("Folder", CompletionHint.Literal)}));

            var AlterMaterializedViewDocString = Command("AlterMaterializedViewDocString", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    If(Not(Token("with")), rules.MaterializedViewNameReference),
                    Token("docstring"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD("Documentation", CompletionHint.Literal)}));

            var AlterMaterializedViewLookback = Command("AlterMaterializedViewLookback", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    If(Not(Token("with")), rules.MaterializedViewNameReference),
                    Token("lookback"),
                    rules.Value,
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD("Lookback", CompletionHint.Literal)}));

            var AlterMaterializedViewAutoUpdateSchema = Command("AlterMaterializedViewAutoUpdateSchema", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    If(Not(Token("with")), rules.MaterializedViewNameReference),
                    Token("autoUpdateSchema"),
                    Token("true", "false"),
                    shape105));

            var ClearMaterializedViewData = Command("ClearMaterializedViewData", 
                Custom(
                    Token("clear", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("data"),
                    shape196));

            var SetMaterializedViewCursor = Command("SetMaterializedViewCursor", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("cursor"),
                    Token("to"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD("CursorValue", CompletionHint.Literal)}));

            var ShowTableOperationsMirroringStatus = Command("ShowTableOperationsMirroringStatus", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Best(
                        If(Not(Token("*")), rules.TableNameReference),
                        Token("*")),
                    Token("operations"),
                    Token("mirroring-status"),
                    shape103));

            var ShowTableOperationsMirroringExportedArtifacts = Command("ShowTableOperationsMirroringExportedArtifacts", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("operations"),
                    Token("mirroring-exported-artifacts"),
                    shape103));

            var ShowTableOperationsMirroringFailures = Command("ShowTableOperationsMirroringFailures", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("operations"),
                    Token("mirroring-failures"),
                    shape103));

            var ShowCluster = Command("ShowCluster", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster")));

            var ShowClusterDetails = Command("ShowClusterDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("details")));

            var ShowDiagnostics = Command("ShowDiagnostics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("diagnostics"),
                    Optional(
                        fragment39),
                    shape25));

            var ShowCapacity = Command("ShowCapacity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("capacity"),
                    Optional(
                        Custom(
                            Token("ingestions", "extents-merge", "table-purge", "data-export", "mirroring", "query-acceleration", "extents-partition", "streaming-ingestion-post-processing", "materialized-view", "graph_snapshot", "queries", "stored-query-results", "purge-storage-artifacts-cleanup", "periodic-storage-artifacts-cleanup"),
                            CD("Resource"))),
                    Optional(
                        fragment39),
                    shape222));

            var ShowOperations = Command("ShowOperations", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("operations"),
                    Optional(
                        Best(
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                shape200),
                            Custom(
                                Token("("),
                                OneOrMoreCommaList(
                                    Custom(
                                        rules.AnyGuidLiteralOrString,
                                        shape200)),
                                Token(")"),
                                shape137))),
                    new [] {CD(), CD(), CD(CompletionHint.Literal, isOptional: true)}));

            var ShowOperationDetails = Command("ShowOperationDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("operation"),
                    rules.AnyGuidLiteralOrString,
                    Token("details"),
                    new [] {CD(), CD(), CD("OperationId", CompletionHint.Literal), CD()}));

            var ShowJournal = Command("ShowJournal", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("journal")));

            var ShowDatabaseJournal = Command("ShowDatabaseJournal", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                    Token("journal"),
                    shape201));

            var ShowClusterJournal = Command("ShowClusterJournal", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("journal")));

            var ShowQueries = Command("ShowQueries", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("queries")));

            var ShowRunningQueries = Command("ShowRunningQueries", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("running"),
                    Token("queries"),
                    Optional(
                        Custom(
                            Token("by"),
                            Best(
                                fragment34,
                                Token("*")))),
                    shape4));

            var CancelQuery = Command("CancelQuery", 
                Custom(
                    Token("cancel", CompletionKind.CommandPrefix),
                    Token("query"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD("ClientRequestId", CompletionHint.Literal)}));

            var ShowQueryPlan = Command("ShowQueryPlan", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("queryplan"),
                    Optional(
                        Custom(
                            Token("with"),
                            Token("("),
                            OneOrMoreCommaList(
                                Custom(
                                    Best(
                                        Token("reconstructCsl"),
                                        Token("showExternalArtifacts"),
                                        If(Not(And(Token("reconstructCsl", "showExternalArtifacts"))), rules.NameDeclaration)),
                                    Token("="),
                                    rules.Value,
                                    shape31)),
                            Token(")"))),
                    Token("<|"),
                    rules.QueryInput,
                    new [] {CD(), CD(), CD(isOptional: true), CD(), CD("Query", CompletionHint.NonScalar)}));

            var ShowCache = Command("ShowCache", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cache")));

            var AlterCache = Command("AlterCache", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cache"),
                    Token("on"),
                    Best(
                        Token("*"),
                        Custom(
                            rules.BracketedStringLiteral,
                            CD("NodeList", CompletionHint.Literal))),
                    rules.BracketedStringLiteral,
                    new [] {CD(), CD(), CD(), CD(), CD("Action", CompletionHint.Literal)}));

            var ShowCommands = Command("ShowCommands", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("commands")));

            var ShowCommandsAndQueries = Command("ShowCommandsAndQueries", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("commands-and-queries")));

            var ShowIngestionFailures = Command("ShowIngestionFailures", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("ingestion"),
                    Token("failures"),
                    Optional(
                        Custom(
                            Token("with"),
                            Token("("),
                            Token("OperationId"),
                            Token("="),
                            rules.AnyGuidLiteralOrString,
                            Token(")"),
                            new [] {CD(), CD(), CD(), CD(), CD("OperationId", CompletionHint.Literal), CD()})),
                    shape4));

            var ShowDataOperations = Command("ShowDataOperations", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("data"),
                    Token("operations")));

            var ShowDatabaseKeyVaultSecrets = Command("ShowDatabaseKeyVaultSecrets", 
                Custom(
                    Token("show"),
                    Token("database"),
                    Token("keyvault"),
                    Token("secrets")).Hide());

            var ShowClusterExtents = Command("ShowClusterExtents", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("extents"),
                    Optional(
                        fragment40),
                    Optional(Token("hot")),
                    Optional(
                        fragment41),
                    Optional(
                        fragment42),
                    shape206));

            var ShowClusterExtentsMetadata = Command("ShowClusterExtentsMetadata", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("extents"),
                    Optional(
                        fragment40),
                    Optional(Token("hot")),
                    Token("metadata"),
                    Optional(
                        fragment41),
                    Optional(
                        fragment42),
                    shape209));

            var ShowDatabaseExtents = Command("ShowDatabaseExtents", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Best(
                        fragment43,
                        fragment44),
                    Token("extents"),
                    Optional(
                        fragment40),
                    Optional(Token("hot")),
                    Optional(
                        fragment41),
                    Optional(
                        fragment42),
                    shape206));

            var ShowDatabaseExtentsMetadata = Command("ShowDatabaseExtentsMetadata", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Best(
                        fragment43,
                        fragment44),
                    Token("extents"),
                    Optional(
                        fragment40),
                    Optional(Token("hot")),
                    Token("metadata"),
                    Optional(
                        fragment41),
                    Optional(
                        fragment42),
                    shape209));

            var ShowDatabaseExtentTagsStatistics = Command("ShowDatabaseExtentTagsStatistics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("extent"),
                    Token("tags"),
                    Token("statistics"),
                    Optional(
                        Custom(
                            Token("with"),
                            Token("("),
                            Token("minCreationTime"),
                            Token("="),
                            rules.Value,
                            Token(")"),
                            new [] {CD(), CD(), CD(), CD(), CD("minCreationTime", CompletionHint.Literal), CD()})),
                    new [] {CD(), CD(), CD(), CD(), CD(), CD(isOptional: true)}));

            var ShowDatabaseExtentsPartitioningStatistics = Command("ShowDatabaseExtentsPartitioningStatistics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                            shape9)),
                    Token("extents"),
                    Token("partitioning"),
                    Token("statistics"),
                    shape210));

            var ShowTableExtents = Command("ShowTableExtents", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Best(
                        fragment45,
                        fragment46),
                    Token("extents"),
                    Optional(
                        fragment40),
                    Optional(Token("hot")),
                    Optional(
                        fragment41),
                    Optional(
                        fragment42),
                    shape206));

            var ShowTableExtentsMetadata = Command("ShowTableExtentsMetadata", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Best(
                        fragment45,
                        fragment46),
                    Token("extents"),
                    Optional(
                        fragment40),
                    Optional(Token("hot")),
                    Token("metadata"),
                    Optional(
                        fragment41),
                    Optional(
                        fragment42),
                    shape209));

            var TableShardsGroupShow = Command("TableShardsGroupShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("shards-group").Hide(),
                    rules.AnyGuidLiteralOrString,
                    Token("shards").Hide(),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("ShardsGroupId", CompletionHint.Literal), CD()}));

            var TableShardsGroupMetadataShow = Command("TableShardsGroupMetadataShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("shards-group").Hide(),
                    rules.AnyGuidLiteralOrString,
                    Token("shards").Hide(),
                    Token("metadata").Hide(),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("ShardsGroupId", CompletionHint.Literal), CD(), CD()}));

            var TableShardGroupsShow = Command("TableShardGroupsShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("shard-groups").Hide(),
                    shape28));

            var TableShardGroupsStatisticsShow = Command("TableShardGroupsStatisticsShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("shard-groups").Hide(),
                    Token("statistics").Hide(),
                    shape103));

            var TablesShardGroupsStatisticsShow = Command("TablesShardGroupsStatisticsShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Optional(
                        fragment5),
                    Token("shard-groups").Hide(),
                    Token("statistics").Hide(),
                    new [] {CD(), CD(), CD(isOptional: true), CD(), CD()}));

            var DatabaseShardGroupsStatisticsShow = Command("DatabaseShardGroupsStatisticsShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Optional(
                        Custom(
                            If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                            shape9)),
                    Token("shard-groups").Hide(),
                    Token("statistics").Hide(),
                    shape94));

            var MergeExtents = Command("MergeExtents", 
                Custom(
                    Token("merge", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    If(Not(And(Token("async", "dryrun", "database"))), rules.TableNameReference),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.AnyGuidLiteralOrString,
                            shape214)),
                    Token(")"),
                    Optional(
                        Custom(
                            Token("with"),
                            Token("("),
                            Token("rebuild"),
                            Token("="),
                            Token("true"),
                            Token(")"))),
                    new [] {CD(), CD(isOptional: true), CD("TableName", CompletionHint.Table), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var MergeExtentsDryrun = Command("MergeExtentsDryrun", 
                Custom(
                    Token("merge", CompletionKind.CommandPrefix),
                    Token("dryrun"),
                    rules.TableNameReference,
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.AnyGuidLiteralOrString,
                            shape214)),
                    Token(")"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(CompletionHint.Literal), CD()}));

            var MoveExtentsFrom = Command("MoveExtentsFrom", 
                Custom(
                    Token("move", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    Token("extents"),
                    Best(
                        Custom(
                            Token("all"),
                            Token("from"),
                            Token("table"),
                            rules.TableNameReference,
                            Token("to"),
                            Token("table"),
                            rules.TableNameReference,
                            new [] {CD(), CD(), CD(), CD("SourceTableName", CompletionHint.Table), CD(), CD(), CD("DestinationTableName", CompletionHint.Table)}),
                        Custom(
                            new Parser<LexicalToken>[] {
                                Token("from"),
                                Token("table"),
                                rules.TableNameReference,
                                Token("to"),
                                Token("table"),
                                rules.TableNameReference,
                                Optional(
                                    fragment10),
                                Token("("),
                                OneOrMoreCommaList(
                                    Custom(
                                        rules.AnyGuidLiteralOrString,
                                        shape214)),
                                Token(")")}
                            ,
                            new [] {CD(), CD(), CD("SourceTableName", CompletionHint.Table), CD(), CD(), CD("DestinationTableName", CompletionHint.Table), CD(isOptional: true), CD(), CD(CompletionHint.Literal), CD()})),
                    shape289));

            var MoveExtentsQuery = Command("MoveExtentsQuery", 
                Custom(
                    Token("move", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    Token("extents"),
                    Token("to"),
                    Token("table"),
                    rules.TableNameReference,
                    Optional(
                        fragment10),
                    Token("<|"),
                    rules.QueryInput,
                    new [] {CD(), CD(isOptional: true), CD(), CD(), CD(), CD("DestinationTableName", CompletionHint.Table), CD(isOptional: true), CD(), CD("Query", CompletionHint.NonScalar)}));

            var TableShuffleExtents = Command("TableShuffleExtents", 
                Custom(
                    Token("shuffle", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    Token("table"),
                    rules.TableNameReference,
                    Token("extents"),
                    Best(
                        Token("all"),
                        Custom(
                            Token("("),
                            OneOrMoreCommaList(
                                Custom(
                                    rules.AnyGuidLiteralOrString,
                                    shape214)),
                            Token(")"),
                            shape137)),
                    Optional(
                        fragment10),
                    new [] {CD(), CD(isOptional: true), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(isOptional: true)}));

            var TableShuffleExtentsQuery = Command("TableShuffleExtentsQuery", 
                Custom(
                    Token("shuffle", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    Token("table"),
                    rules.TableNameReference,
                    Token("extents"),
                    Optional(
                        fragment10),
                    Token("<|"),
                    rules.QueryInput,
                    new [] {CD(), CD(isOptional: true), CD(), CD("tableName", CompletionHint.Table), CD(), CD(isOptional: true), CD(), CD("Query", CompletionHint.NonScalar)}));

            var ReplaceExtents = Command("ReplaceExtents", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("replace", CompletionKind.CommandPrefix),
                        Optional(Token("async")),
                        Token("extents"),
                        Token("in"),
                        Token("table"),
                        rules.TableNameReference,
                        Optional(
                            fragment10),
                        Token("<|"),
                        Token("{"),
                        rules.QueryInput,
                        Token("}"),
                        Token(","),
                        Token("{"),
                        rules.QueryInput,
                        Token("}")}
                    ,
                    new [] {CD(), CD(isOptional: true), CD(), CD(), CD(), CD("DestinationTableName", CompletionHint.Table), CD(isOptional: true), CD(), CD(), CD("ExtentsToDropQuery", CompletionHint.NonScalar), CD(), CD(), CD(), CD("ExtentsToMoveQuery", CompletionHint.NonScalar), CD()}));

            var DropExtent = Command("DropExtent", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("extent"),
                    rules.AnyGuidLiteralOrString,
                    Optional(
                        fragment47),
                    new [] {CD(), CD(), CD("ExtentId", CompletionHint.Literal), CD(isOptional: true)}));

            var DropExtents = Command("DropExtents", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("extents"),
                    Best(
                        Custom(
                            Token("("),
                            OneOrMoreCommaList(
                                Custom(
                                    rules.AnyGuidLiteralOrString,
                                    shape202)),
                            Token(")"),
                            Optional(
                                fragment47),
                            new [] {CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}),
                        Custom(
                            Token("whatif"),
                            Token("<|"),
                            rules.QueryInput,
                            new [] {CD(), CD(), CD("Query", CompletionHint.NonScalar)}),
                        fragment48,
                        Custom(
                            Token("older"),
                            rules.Value,
                            Token("days", "hours"),
                            Token("from"),
                            Best(
                                fragment50,
                                Custom(
                                    If(Not(Token("all")), rules.TableNameReference),
                                    shape26)),
                            Optional(
                                fragment51),
                            Optional(
                                fragment49),
                            new [] {CD(), CD("Older", CompletionHint.Literal), CD(), CD(), CD(), CD(isOptional: true), CD(isOptional: true)}),
                        Custom(
                            Token("from"),
                            Best(
                                fragment50,
                                Custom(
                                    If(Not(Token("all")), rules.TableNameReference),
                                    shape26)),
                            Optional(
                                fragment51),
                            Optional(
                                fragment49),
                            shape222))));

            var DropExtentsPartitionMetadata = Command("DropExtentsPartitionMetadata", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("extents"),
                    Token("partition"),
                    Token("metadata"),
                    Token("from"),
                    Token("table"),
                    rules.TableNameReference,
                    Optional(
                        Custom(
                            Token("between"),
                            Token("("),
                            rules.Value,
                            Token(".."),
                            rules.Value,
                            Token(")"),
                            new [] {CD(), CD(), CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal), CD()})),
                    fragment55,
                    new [] {CD(), CD(), CD(), CD(), CD(), CD(), CD("TableName", CompletionHint.Table), CD(isOptional: true), CD("csl")}));

            var DropPretendExtentsByProperties = Command("DropPretendExtentsByProperties", 
                Custom(
                    Token("drop-pretend", CompletionKind.CommandPrefix),
                    Token("extents"),
                    Optional(
                        Custom(
                            Token("older"),
                            rules.Value,
                            Token("days", "hours"),
                            new [] {CD(), CD("Older", CompletionHint.Literal), CD()})),
                    Token("from"),
                    Best(
                        fragment50,
                        Custom(
                            If(Not(Token("all")), rules.TableNameReference),
                            shape26)),
                    Optional(
                        fragment51),
                    Optional(
                        fragment49),
                    new [] {CD(), CD(), CD(isOptional: true), CD(), CD(), CD(isOptional: true), CD(isOptional: true)}));

            var ShowVersion = Command("ShowVersion", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("version")));

            var ClearTableData = Command("ClearTableData", 
                Custom(
                    Token("clear", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    Token("table"),
                    rules.TableNameReference,
                    Token("data"),
                    new [] {CD(), CD(isOptional: true), CD(), CD("TableName", CompletionHint.Table), CD()}));

            var ClearTableCacheStreamingIngestionSchema = Command("ClearTableCacheStreamingIngestionSchema", 
                Custom(
                    Token("clear", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("cache"),
                    Token("streamingingestion"),
                    Token("schema"),
                    shape100));

            var ShowStorageArtifactsCleanupState = Command("ShowStorageArtifactsCleanupState", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("data", "metadata"),
                    Token("storage"),
                    Token("artifacts"),
                    Token("cleanup"),
                    Token("state"),
                    Optional(
                        fragment10),
                    new [] {CD(), CD(), CD(), CD(), CD(), CD(), CD(), CD(isOptional: true)}));

            var ClusterDropStorageArtifactsCleanupState = Command("ClusterDropStorageArtifactsCleanupState", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("data", "metadata", "all"),
                    Token("storage"),
                    Token("artifacts"),
                    Token("cleanup"),
                    Token("state")));

            var StoredQueryResultSet = Command("StoredQueryResultSet", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    Optional(Token("ifnotexists")),
                    Token("stored_query_result"),
                    rules.NameDeclaration,
                    Optional(
                        fragment36),
                    Token("<|"),
                    rules.QueryInput,
                    new [] {CD(), CD(isOptional: true), CD(isOptional: true), CD(), CD("StoredQueryResultName", CompletionHint.None), CD(isOptional: true), CD(), CD("Query", CompletionHint.NonScalar)}));

            var StoredQueryResultSetOrReplace = Command("StoredQueryResultSetOrReplace", 
                Custom(
                    Token("set-or-replace", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    Token("stored_query_result"),
                    rules.NameDeclaration,
                    Optional(
                        fragment36),
                    Token("<|"),
                    rules.QueryInput,
                    new [] {CD(), CD(isOptional: true), CD(), CD("StoredQueryResultName", CompletionHint.None), CD(isOptional: true), CD(), CD("Query", CompletionHint.NonScalar)}));

            var StoredQueryResultsShow = Command("StoredQueryResultsShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("stored_query_results"),
                    Optional(
                        Custom(
                            If(Not(Token("with")), rules.NameDeclaration),
                            shape226)),
                    Optional(
                        fragment10),
                    new [] {CD(), CD(), CD(CompletionHint.None, isOptional: true), CD(isOptional: true)}));

            var StoredQueryResultShowSchema = Command("StoredQueryResultShowSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("stored_query_result"),
                    rules.NameDeclaration,
                    Token("schema"),
                    new [] {CD(), CD(), CD("StoredQueryResultName", CompletionHint.None), CD()}));

            var StoredQueryResultDrop = Command("StoredQueryResultDrop", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("stored_query_result"),
                    rules.NameDeclaration,
                    new [] {CD(), CD(), CD("StoredQueryResultName", CompletionHint.None)}));

            var StoredQueryResultsDrop = Command("StoredQueryResultsDrop", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("stored_query_results"),
                    Token("by"),
                    Token("user"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD(), CD(), CD("Principal", CompletionHint.Literal)}));

            var GraphModelCreateOrAlter = Command("GraphModelCreateOrAlter", 
                Custom(
                    Token("create-or-alter", CompletionKind.CommandPrefix),
                    Token("graph_model"),
                    rules.NameDeclaration,
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD(CompletionHint.None), CD(CompletionHint.Literal)}));

            var GraphModelShow = Command("GraphModelShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("graph_model"),
                    If(Not(Token("*")), rules.GraphModelNameReference),
                    Optional(Token("details")),
                    Optional(
                        fragment10),
                    new [] {CD(), CD(), CD(CompletionHint.GraphModel), CD(isOptional: true), CD(isOptional: true)}));

            var GraphModelsShow = Command("GraphModelsShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("graph_models"),
                    Optional(Token("details")),
                    Optional(
                        fragment10),
                    shape222));

            var GraphModelDrop = Command("GraphModelDrop", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("graph_model"),
                    rules.GraphModelNameReference,
                    shape228));

            var SetGraphModelAdmins = Command("SetGraphModelAdmins", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("graph_model"),
                    rules.GraphModelNameReference,
                    Token("admins"),
                    Best(
                        Token("none"),
                        fragment66),
                    new [] {CD(), CD(), CD(CompletionHint.GraphModel), CD(), CD()}));

            var AddGraphModelAdmins = Command("AddGraphModelAdmins", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("graph_model"),
                    rules.GraphModelNameReference,
                    Token("admins"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.StringLiteral,
                            shape85)),
                    Token(")"),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape86)),
                    shape227));

            var DropGraphModelAdmins = Command("DropGraphModelAdmins", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("graph_model"),
                    rules.GraphModelNameReference,
                    Token("admins"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.StringLiteral,
                            shape85)),
                    Token(")"),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape86)),
                    shape227));

            var GraphSnapshotMake = Command("GraphSnapshotMake", 
                Custom(
                    Token("make", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    Token("graph_snapshot"),
                    rules.NameDeclaration,
                    Token("from"),
                    rules.GraphModelNameReference,
                    new [] {CD(), CD(isOptional: true), CD(), CD(CompletionHint.None), CD(), CD(CompletionHint.GraphModel)}));

            var GraphSnapshotShow = Command("GraphSnapshotShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("graph_snapshot"),
                    rules.GraphModelSnapshotNameReference,
                    Optional(Token("details")),
                    new [] {CD(), CD(), CD(CompletionHint.GraphModel), CD(isOptional: true)}));

            var GraphSnapshotsShow = Command("GraphSnapshotsShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("graph_snapshots"),
                    rules.QualifiedWildcardedNameDeclaration,
                    Optional(Token("details")),
                    shape37));

            var GraphSnapshotDrop = Command("GraphSnapshotDrop", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("graph_snapshot"),
                    rules.GraphModelSnapshotNameReference,
                    shape228));

            var GraphSnapshotsDrop = Command("GraphSnapshotsDrop", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("graph_snapshots"),
                    rules.QualifiedWildcardedNameDeclaration,
                    shape43));

            var GraphSnapshotShowStatistics = Command("GraphSnapshotShowStatistics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("graph_snapshot"),
                    rules.QualifiedWildcardedNameDeclaration,
                    Token("statistics").Hide(),
                    shape3));

            var GraphSnapshotsShowStatistics = Command("GraphSnapshotsShowStatistics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("graph_snapshots"),
                    rules.GraphModelSnapshotNameReference,
                    Token("statistics").Hide(),
                    shape229));

            var GraphSnapshotShowFailures = Command("GraphSnapshotShowFailures", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("graph_snapshots"),
                    rules.GraphModelSnapshotNameReference,
                    Token("failures").Hide(),
                    shape229));

            var ShowCertificates = Command("ShowCertificates", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("certificates")));

            var ShowCloudSettings = Command("ShowCloudSettings", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cloudsettings")));

            var ShowCommConcurrency = Command("ShowCommConcurrency", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("commconcurrency")));

            var ShowCommPools = Command("ShowCommPools", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("commpools")));

            var ShowFabricCache = Command("ShowFabricCache", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("fabriccache")));

            var ShowFabricLocks = Command("ShowFabricLocks", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("fabriclocks")));

            var ShowFabricClocks = Command("ShowFabricClocks", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("fabricclocks")));

            var ShowFeatureFlags = Command("ShowFeatureFlags", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("featureflags")));

            var ShowMemPools = Command("ShowMemPools", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("mempools")));

            var ShowServicePoints = Command("ShowServicePoints", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("servicepoints")));

            var ShowTcpConnections = Command("ShowTcpConnections", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("tcpconnections")));

            var ShowTcpPorts = Command("ShowTcpPorts", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("tcpports")));

            var ShowThreadPools = Command("ShowThreadPools", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("threadpools")));

            var ExecuteDatabaseScript = Command("ExecuteDatabaseScript", 
                Custom(
                    Token("execute", CompletionKind.CommandPrefix),
                    Optional(Token("database")),
                    Token("script"),
                    Optional(
                        fragment52),
                    Token("<|"),
                    rules.ScriptInput,
                    new [] {CD(), CD(isOptional: true), CD(), CD(isOptional: true), CD(), CD(CompletionHint.NonScalar)}));

            var ExecuteClusterScript = Command("ExecuteClusterScript", 
                Custom(
                    Token("execute", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("script"),
                    Optional(
                        fragment52),
                    Token("<|"),
                    rules.ScriptInput,
                    new [] {CD(), CD(), CD(), CD(isOptional: true), CD(), CD(CompletionHint.NonScalar)}));

            var CreateRequestSupport = Command("CreateRequestSupport", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("request_support"),
                    Optional(
                        fragment0),
                    shape25));

            var ShowRequestSupport = Command("ShowRequestSupport", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("request_support"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD("key", CompletionHint.Literal)}));

            var ShowClusterAdminState = Command("ShowClusterAdminState", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("admin"),
                    Token("state")));

            var ClearRemoteClusterDatabaseSchema = Command("ClearRemoteClusterDatabaseSchema", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("clear", CompletionKind.CommandPrefix),
                        Token("cache"),
                        Token("remote-schema"),
                        Token("cluster"),
                        Token("("),
                        rules.StringLiteral,
                        Token(")"),
                        Token("."),
                        Token("database"),
                        Token("("),
                        rules.StringLiteral,
                        Token(")")}
                    ,
                    new [] {CD(), CD(), CD(), CD(), CD(), CD("clusterName", CompletionHint.Literal), CD(), CD(), CD(), CD(), CD("databaseName", CompletionHint.Literal), CD()}));

            var ShowClusterMonitoring = Command("ShowClusterMonitoring", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("monitoring")));

            var ShowClusterScaleIn = Command("ShowClusterScaleIn", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("scalein"),
                    Best(
                        rules.Value,
                        rules.Value),
                    Token("nodes"),
                    new [] {CD(), CD(), CD(), CD("num", CompletionHint.Literal), CD()}));

            var ShowClusterServices = Command("ShowClusterServices", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("services")));

            var ShowClusterNetwork = Command("ShowClusterNetwork", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("network"),
                    Optional(
                        Custom(
                            rules.Value,
                            CD("bytes", CompletionHint.Literal))),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Literal, isOptional: true)}));

            var AlterClusterStorageKeys = Command("AlterClusterStorageKeys", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    Token("cluster"),
                    Token("storage"),
                    Token("keys"),
                    Optional(
                        fragment0),
                    Token("decryption-certificate-thumbprint"),
                    rules.StringLiteral,
                    new [] {CD(), CD(isOptional: true), CD(), CD(), CD(), CD(isOptional: true), CD(), CD("thumbprint", CompletionHint.Literal)}));

            var ShowClusterStorageKeysHash = Command("ShowClusterStorageKeysHash", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("storage"),
                    Token("keys"),
                    Token("hash")));

            var AlterFabricServiceAssignmentsCommand = Command("AlterFabricServiceAssignmentsCommand", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("fabric"),
                    Token("service"),
                    rules.StringLiteral,
                    Best(
                        Custom(
                            Token("assignment"),
                            rules.StringLiteral,
                            Token("to"),
                            rules.StringLiteral,
                            new [] {CD(), CD("serviceId", CompletionHint.Literal), CD(), CD("nodeId", CompletionHint.Literal)}),
                        Custom(
                            Token("assignments"),
                            rules.StringLiteral,
                            new [] {CD(), CD("serviceToNode", CompletionHint.Literal)})),
                    shape231));

            var DropFabricServiceAssignmentsCommand = Command("DropFabricServiceAssignmentsCommand", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("fabric"),
                    Token("service"),
                    rules.StringLiteral,
                    Token("assignments"),
                    shape231));

            var CreateEntityGroupCommand = Command("CreateEntityGroupCommand", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("entity_group"),
                    rules.NameDeclaration,
                    Optional(Token("ifnotexists")),
                    Token("("),
                    OneOrMoreCommaList(
                        Best(
                            Custom(
                                Token("cluster"),
                                Token("("),
                                rules.StringLiteral,
                                Token(")"),
                                new [] {CD(), CD(), CD("clusterName", CompletionHint.Literal), CD()}),
                            fragment53,
                            fragment54)),
                    Token(")"),
                    new [] {CD(), CD(), CD("EntityGroupName", CompletionHint.None), CD(isOptional: true), CD(), CD(), CD()}));

            var CreateOrAlterEntityGroupCommand = Command("CreateOrAlterEntityGroupCommand", 
                Custom(
                    Token("create-or-alter", CompletionKind.CommandPrefix),
                    Token("entity_group"),
                    rules.NameDeclaration,
                    Token("("),
                    OneOrMoreCommaList(
                        Best(
                            fragment53,
                            fragment54)),
                    Token(")"),
                    new [] {CD(), CD(), CD("EntityGroupName", CompletionHint.None), CD(), CD(), CD()}));

            var AlterEntityGroup = Command("AlterEntityGroup", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("entity_group"),
                    rules.EntityGroupNameReference,
                    Token("("),
                    Custom(
                        OneOrMoreCommaList(
                            Best(
                                fragment53,
                                fragment54)),
                        Token(")")),
                    new [] {CD(), CD(), CD("EntityGroupName", CompletionHint.EntityGroup), CD(), CD()}));

            var AlterMergeEntityGroup = Command("AlterMergeEntityGroup", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("entity_group"),
                    rules.EntityGroupNameReference,
                    Token("("),
                    OneOrMoreCommaList(
                        Best(
                            fragment53,
                            fragment54)),
                    Token(")"),
                    new [] {CD(), CD(), CD("EntityGroupName", CompletionHint.EntityGroup), CD(), CD(), CD()}));

            var DropEntityGroup = Command("DropEntityGroup", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("entity_group"),
                    rules.EntityGroupNameReference,
                    shape238));

            var ShowEntityGroup = Command("ShowEntityGroup", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("entity_group"),
                    rules.EntityGroupNameReference,
                    shape238));

            var ShowEntityGroups = Command("ShowEntityGroups", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("entity_groups")));

            var AlterExtentContainersAdd = Command("AlterExtentContainersAdd", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("extentcontainers"),
                    rules.DatabaseNameReference,
                    Token("add"),
                    rules.StringLiteral,
                    Optional(
                        Custom(
                            rules.Value,
                            rules.AnyGuidLiteralOrString,
                            new [] {CD("hardDeletePeriod", CompletionHint.Literal), CD("containerId", CompletionHint.Literal)})),
                    new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD("container", CompletionHint.Literal), CD(CompletionHint.Literal, isOptional: true)}));

            var AlterExtentContainersDrop = Command("AlterExtentContainersDrop", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("extentcontainers"),
                    rules.DatabaseNameReference,
                    Token("drop"),
                    Optional(
                        Custom(
                            rules.AnyGuidLiteralOrString,
                            shape239)),
                    new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(CompletionHint.Literal, isOptional: true)}));

            var AlterExtentContainersRecycle = Command("AlterExtentContainersRecycle", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("extentcontainers"),
                    rules.DatabaseNameReference,
                    Token("recycle"),
                    Best(
                        Custom(
                            rules.AnyGuidLiteralOrString,
                            shape239),
                        Custom(
                            Token("older"),
                            Best(
                                rules.Value,
                                rules.Value),
                            Token("hours"),
                            new [] {CD(), CD("hours", CompletionHint.Literal), CD()})),
                    new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(CompletionHint.Literal)}));

            var AlterExtentContainersSet = Command("AlterExtentContainersSet", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("extentcontainers"),
                    rules.DatabaseNameReference,
                    Token("set"),
                    Token("state"),
                    rules.AnyGuidLiteralOrString,
                    Token("to"),
                    Token("readonly", "readwrite"),
                    new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(), CD("container", CompletionHint.Literal), CD(), CD()}));

            var ShowExtentContainers = Command("ShowExtentContainers", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("extentcontainers"),
                    Optional(
                        fragment0),
                    shape25));

            var DropEmptyExtentContainers = Command("DropEmptyExtentContainers", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("drop", CompletionKind.CommandPrefix),
                        Optional(Token("async")),
                        Token("empty"),
                        Token("extentcontainers"),
                        rules.DatabaseNameReference,
                        Token("until"),
                        Token("="),
                        rules.Value,
                        Optional(Token("whatif")),
                        Optional(
                            fragment0)}
                    ,
                    new [] {CD(), CD(isOptional: true), CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(), CD("d", CompletionHint.Literal), CD(isOptional: true), CD(isOptional: true)}));

            var CleanDatabaseExtentContainers = Command("CleanDatabaseExtentContainers", 
                Custom(
                    Token("clean", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Optional(Token("async")),
                    Optional(
                        fragment59),
                    Token("extentcontainers"),
                    new [] {CD(), CD(), CD(isOptional: true), CD(isOptional: true), CD()}));

            var ShowDatabaseExtentContainersCleanOperations = Command("ShowDatabaseExtentContainersCleanOperations", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                    Token("extentcontainers"),
                    Token("clean"),
                    Token("operations"),
                    Optional(
                        Custom(
                            rules.AnyGuidLiteralOrString,
                            shape277)),
                    new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(), CD(), CD(CompletionHint.Literal, isOptional: true)}));

            var ClearDatabaseCacheQueryResults = Command("ClearDatabaseCacheQueryResults", 
                Custom(
                    Token("clear", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("cache"),
                    Token("query_results")));

            var ShowDatabaseCacheQueryResults = Command("ShowDatabaseCacheQueryResults", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("cache"),
                    Token("query_results")));

            var ShowDatabasesManagementGroups = Command("ShowDatabasesManagementGroups", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("management"),
                    Token("groups")));

            var AlterDatabaseStorageKeys = Command("AlterDatabaseStorageKeys", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    Token("database"),
                    If(Not(And(Token("prettyname", "policy"))), rules.DatabaseNameReference),
                    Token("storage"),
                    Token("keys"),
                    Optional(
                        fragment0),
                    Token("decryption-certificate-thumbprint"),
                    rules.StringLiteral,
                    new [] {CD(), CD(isOptional: true), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(), CD(isOptional: true), CD(), CD("thumbprint", CompletionHint.Literal)}));

            var ClearDatabaseCacheStreamingIngestionSchema = Command("ClearDatabaseCacheStreamingIngestionSchema", 
                Custom(
                    Token("clear", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("cache"),
                    Token("streamingingestion"),
                    Token("schema")));

            var ClearDatabaseCacheQueryWeakConsistency = Command("ClearDatabaseCacheQueryWeakConsistency", 
                Custom(
                    Token("clear", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("cache"),
                    Token("query_weak_consistency")));

            var ShowEntitySchema = Command("ShowEntitySchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("entity"),
                    rules.QualifiedNameDeclaration,
                    Token("schema"),
                    Token("as"),
                    Token("json"),
                    Optional(
                        Custom(
                            Token("in"),
                            Token("databases"),
                            Token("("),
                            OneOrMoreCommaList(
                                Custom(
                                    rules.StringLiteral,
                                    CD("item", CompletionHint.Literal))),
                            Token(")"),
                            new [] {CD(), CD(), CD(), CD(CompletionHint.Literal), CD()})),
                    Optional(
                        Custom(
                            Token("except"),
                            rules.StringLiteral,
                            new [] {CD(), CD("excludedFunctions", CompletionHint.Literal)})),
                    Optional(
                        fragment0),
                    new [] {CD(), CD(), CD("entity", CompletionHint.None), CD(), CD(), CD(), CD(isOptional: true), CD(isOptional: true), CD(isOptional: true)}));

            var ShowExtentDetails = Command("ShowExtentDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    If(Not(And(Token("database", "cluster", "databases", "ingestion", "tables", "table", "functions", "function", "external", "fabric", "workload_groups", "workload_group", "column", "materialized-view", "graph_model", "mirroring-template", "mirroring-templates", "principal", "continuous-export", "continuous-exports", "materialized-views", "diagnostics", "capacity", "operations", "operation", "journal", "queries", "running", "queryplan", "cache", "commands", "commands-and-queries", "data", "version", "stored_query_results", "stored_query_result", "graph_models", "graph_snapshot", "graph_snapshots", "certificates", "cloudsettings", "commconcurrency", "commpools", "fabriccache", "fabriclocks", "fabricclocks", "featureflags", "mempools", "servicepoints", "tcpconnections", "tcpports", "threadpools", "request_support", "entity_group", "entity_groups", "extentcontainers", "entity", "follower", "freshness", "memory", "plugins", "queryexecution", "rowstore", "rowstores", "schema", "callstacks", "filesystem", "streamingingestion", "query", "extents"))), rules.NameDeclaration),
                    Token("extent"),
                    Optional(Token("details")),
                    Optional(
                        Custom(
                            rules.AnyGuidLiteralOrString,
                            shape243)),
                    Optional(
                        fragment10),
                    new [] {CD(), CD("tableName", CompletionHint.None), CD(), CD(isOptional: true), CD(CompletionHint.Literal, isOptional: true), CD(isOptional: true)}));

            var ShowExtentColumnStorageStats = Command("ShowExtentColumnStorageStats", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    If(Not(And(Token("database", "cluster", "databases", "ingestion", "tables", "table", "functions", "function", "external", "fabric", "workload_groups", "workload_group", "column", "materialized-view", "graph_model", "mirroring-template", "mirroring-templates", "principal", "continuous-export", "continuous-exports", "materialized-views", "diagnostics", "capacity", "operations", "operation", "journal", "queries", "running", "queryplan", "cache", "commands", "commands-and-queries", "data", "version", "stored_query_results", "stored_query_result", "graph_models", "graph_snapshot", "graph_snapshots", "certificates", "cloudsettings", "commconcurrency", "commpools", "fabriccache", "fabriclocks", "fabricclocks", "featureflags", "mempools", "servicepoints", "tcpconnections", "tcpports", "threadpools", "request_support", "entity_group", "entity_groups", "extentcontainers", "entity", "follower", "freshness", "memory", "plugins", "queryexecution", "rowstore", "rowstores", "schema", "callstacks", "filesystem", "streamingingestion", "query", "extents"))), rules.NameDeclaration),
                    Token("extent"),
                    rules.AnyGuidLiteralOrString,
                    Token("column"),
                    rules.NameDeclaration,
                    Token("storage"),
                    Token("stats"),
                    new [] {CD(), CD("tableName", CompletionHint.None), CD(), CD("extentId", CompletionHint.Literal), CD(), CD("columnName", CompletionHint.None), CD(), CD()}));

            var AttachExtentsIntoTableByContainer = Command("AttachExtentsIntoTableByContainer", 
                Custom(
                    Token("attach", CompletionKind.CommandPrefix),
                    Token("extents"),
                    Token("into"),
                    Token("table"),
                    rules.TableNameReference,
                    Token("by"),
                    Token("container"),
                    rules.StringLiteral,
                    OneOrMoreList(
                        Custom(
                            rules.AnyGuidLiteralOrString,
                            shape243)),
                    new [] {CD(), CD(), CD(), CD(), CD("tableName", CompletionHint.Table), CD(), CD(), CD("containerUri", CompletionHint.Literal), CD(CompletionHint.Literal)}));

            var AttachExtentsIntoTableByMetadata = Command("AttachExtentsIntoTableByMetadata", 
                Custom(
                    Token("attach", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    Token("extents"),
                    ZeroOrMoreList(
                        Custom(
                            Token("into"),
                            Token("table"),
                            rules.TableNameReference,
                            new [] {CD(), CD(), CD("tableName", CompletionHint.Table)})),
                    Token("by"),
                    Token("metadata"),
                    fragment55,
                    new [] {CD(), CD(isOptional: true), CD(), CD(), CD(), CD(), CD("csl")}));

            var AlterExtentTagsFromQuery = Command("AlterExtentTagsFromQuery", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Optional(Token("async")),
                        Token("table"),
                        rules.TableNameReference,
                        Token("extent"),
                        Token("tags"),
                        Token("("),
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape246)),
                        Token(")"),
                        Optional(
                            fragment10),
                        fragment55}
                    ,
                    shape247));

            var AlterMergeExtentTagsFromQuery = Command("AlterMergeExtentTagsFromQuery", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter-merge", CompletionKind.CommandPrefix),
                        Optional(Token("async")),
                        Token("table"),
                        rules.TableNameReference,
                        Token("extent"),
                        Token("tags"),
                        Token("("),
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape246)),
                        Token(")"),
                        Optional(
                            fragment10),
                        fragment55}
                    ,
                    shape247));

            var DropExtentTagsFromQuery = Command("DropExtentTagsFromQuery", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    Token("table"),
                    rules.TableNameReference,
                    Token("extent"),
                    Token("tags"),
                    Optional(
                        fragment10),
                    fragment55,
                    new [] {CD(), CD(isOptional: true), CD(), CD("tableName", CompletionHint.Table), CD(), CD(), CD(isOptional: true), CD("csl")}));

            var DropExtentTagsFromTable = Command("DropExtentTagsFromTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("drop", CompletionKind.CommandPrefix),
                        Optional(Token("async")),
                        Token("table"),
                        rules.TableNameReference,
                        Token("extent"),
                        Token("tags"),
                        Token("("),
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape246)),
                        Token(")"),
                        Optional(
                            fragment10)}
                    ,
                    new [] {CD(), CD(isOptional: true), CD(), CD("tableName", CompletionHint.Table), CD(), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var DropExtentTagsRetention = Command("DropExtentTagsRetention", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("extent"),
                    Token("tags"),
                    Token("retention")));

            var MergeDatabaseShardGroups = Command("MergeDatabaseShardGroups", 
                Custom(
                    Token("merge", CompletionKind.CommandPrefix),
                    Token("database").Hide(),
                    Token("shard-groups").Hide()));

            var AlterFollowerClusterConfiguration = Command("AlterFollowerClusterConfiguration", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("follower"),
                    Token("cluster"),
                    Token("configuration"),
                    Token("from"),
                    rules.StringLiteral,
                    Best(
                        Custom(
                            Token("follow-authorized-principals"),
                            Token("="),
                            rules.Value,
                            new [] {CD(), CD(), CD("followAuthorizedPrincipals", CompletionHint.Literal)}),
                        Custom(
                            Token("default-principals-modification-kind"),
                            Token("="),
                            Token("none", "union", "replace"),
                            shape249),
                        Custom(
                            Token("default-caching-policies-modification-kind"),
                            Token("="),
                            Token("none", "union", "replace"),
                            shape249),
                        Custom(
                            Token("database-name-prefix"),
                            Token("="),
                            rules.NameDeclaration,
                            new [] {CD(), CD(), CD("databaseNamePrefix", CompletionHint.None)})),
                    new [] {CD(), CD(), CD(), CD(), CD(), CD("leaderClusterMetadataPath", CompletionHint.Literal), CD()}));

            var AddFollowerDatabaseAuthorizedPrincipals = Command("AddFollowerDatabaseAuthorizedPrincipals", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("add", CompletionKind.CommandPrefix),
                        Token("follower"),
                        Token("database"),
                        rules.DatabaseNameReference,
                        Optional(
                            fragment56),
                        Token("admins", "users", "viewers", "unrestrictedviewers", "monitors"),
                        Token("("),
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape85)),
                        Token(")"),
                        Optional(
                            Custom(
                                rules.StringLiteral,
                                shape86))}
                    ,
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(isOptional: true), CD("operationRole"), CD(), CD(CompletionHint.Literal), CD(), CD(CompletionHint.Literal, isOptional: true)}));

            var DropFollowerDatabaseAuthorizedPrincipals = Command("DropFollowerDatabaseAuthorizedPrincipals", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("follower"),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("admins", "users", "viewers", "unrestrictedviewers", "monitors"),
                    Optional(
                        fragment56),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.StringLiteral,
                            shape85)),
                    Token(")"),
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD("operationRole"), CD(isOptional: true), CD(), CD(CompletionHint.Literal), CD()}));

            var AlterFollowerDatabaseAuthorizedPrincipals = Command("AlterFollowerDatabaseAuthorizedPrincipals", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("follower"),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Optional(
                        fragment56),
                    Token("policy"),
                    Token("caching"),
                    Best(
                        fragment60,
                        fragment61),
                    Optional(
                        fragment57),
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(isOptional: true), CD(), CD(), CD(), CD("hotWindows", isOptional: true)}));

            var DropFollowerDatabasePolicyCaching = Command("DropFollowerDatabasePolicyCaching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("follower"),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("caching"),
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(), CD()}));

            var AlterFollowerDatabaseChildEntities = Command("AlterFollowerDatabaseChildEntities", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("follower"),
                        Token("database"),
                        rules.DatabaseNameReference,
                        Optional(
                            fragment56),
                        Best(
                            Token("tables"),
                            Custom(
                                Token("external"),
                                Token("tables")),
                            Token("materialized-views")),
                        Token("exclude", "include"),
                        Token("add", "drop"),
                        Token("("),
                        OneOrMoreCommaList(
                            Custom(
                                rules.WildcardedNameDeclaration,
                                CD("ename", CompletionHint.None))),
                        Token(")")}
                    ,
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(isOptional: true), CD(), CD("entityListKind"), CD("operationName"), CD(), CD(CompletionHint.None), CD()}));

            var AlterFollowerDatabaseConfiguration = Command("AlterFollowerDatabaseConfiguration", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("follower"),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Optional(
                        fragment56),
                    Best(
                        Custom(
                            Token("principals-modification-kind"),
                            Token("="),
                            Token("none", "union", "replace"),
                            shape249),
                        Custom(
                            Token("caching-policies-modification-kind"),
                            Token("="),
                            Token("none", "union", "replace"),
                            shape249),
                        Custom(
                            Token("prefetch-extents"),
                            Token("="),
                            rules.Value,
                            new [] {CD(), CD(), CD("prefetchExtents", CompletionHint.Literal)}),
                        Custom(
                            Token("metadata"),
                            rules.StringLiteral,
                            new [] {CD(), CD("serializedDatabaseMetadataOverride", CompletionHint.Literal)}),
                        Custom(
                            Token("database-name-override"),
                            Token("="),
                            rules.NameDeclaration,
                            new [] {CD(), CD(), CD("databaseNameOverride", CompletionHint.None)})),
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(isOptional: true), CD()}));

            var DropFollowerDatabases = Command("DropFollowerDatabases", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("follower"),
                    Best(
                        fragment58,
                        Custom(
                            Token("databases"),
                            Token("("),
                            OneOrMoreCommaList(
                                Custom(
                                    rules.DatabaseNameReference,
                                    shape24)),
                            Token(")"),
                            shape208)),
                    Token("from"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD(), CD(), CD("leaderClusterMetadataPath", CompletionHint.Literal)}));

            var ShowFollowerDatabase = Command("ShowFollowerDatabase", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("follower"),
                    Best(
                        fragment58,
                        Custom(
                            Token("databases"),
                            Optional(
                                fragment59),
                            shape177))));

            var AlterFollowerTablesPolicyCaching = Command("AlterFollowerTablesPolicyCaching", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("follower"),
                        Token("database"),
                        rules.DatabaseNameReference,
                        Optional(
                            fragment56),
                        Best(
                            fragment62,
                            fragment63,
                            fragment64,
                            fragment65),
                        Token("policy"),
                        Token("caching"),
                        Best(
                            fragment60,
                            fragment61),
                        Optional(
                            fragment57)}
                    ,
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(isOptional: true), CD(), CD(), CD(), CD(), CD("hotWindows", isOptional: true)}));

            var DropFollowerTablesPolicyCaching = Command("DropFollowerTablesPolicyCaching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("follower"),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Best(
                        fragment62,
                        fragment63,
                        fragment64,
                        fragment65),
                    Token("policy"),
                    Token("caching"),
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(), CD(), CD()}));

            var ShowFreshness = Command("ShowFreshness", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("freshness").Hide(),
                    rules.TableNameReference,
                    Optional(
                        Custom(
                            Token("column"),
                            rules.ColumnNameReference,
                            new [] {CD(), CD("columnName", CompletionHint.Column)})),
                    Optional(
                        Custom(
                            Token("threshold"),
                            rules.Value,
                            new [] {CD(), CD("threshold", CompletionHint.Literal)})),
                    new [] {CD(), CD(), CD("tableName", CompletionHint.Table), CD(isOptional: true), CD(isOptional: true)}));

            var ShowFunctionSchemaAsJson = Command("ShowFunctionSchemaAsJson", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("function"),
                    rules.FunctionNameReference,
                    Token("schema"),
                    Token("as"),
                    Token("json"),
                    Optional(
                        fragment10),
                    new [] {CD(), CD(), CD("functionName", CompletionHint.Function), CD(), CD(), CD(), CD(isOptional: true)}));

            var SetMaterializedViewAdmins = Command("SetMaterializedViewAdmins", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("admins"),
                    Best(
                        Token("none"),
                        fragment66),
                    new [] {CD(), CD(), CD("materializedViewName", CompletionHint.MaterializedView), CD(), CD()}));

            var AddMaterializedViewAdmins = Command("AddMaterializedViewAdmins", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("admins"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.StringLiteral,
                            shape85)),
                    Token(")"),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape86)),
                    shape273));

            var DropMaterializedViewAdmins = Command("DropMaterializedViewAdmins", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("admins"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.StringLiteral,
                            shape85)),
                    Token(")"),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape86)),
                    shape273));

            var SetMaterializedViewConcurrency = Command("SetMaterializedViewConcurrency", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("concurrency"),
                    Optional(
                        Custom(
                            Token("="),
                            Best(
                                rules.Value,
                                rules.Value),
                            new [] {CD(), CD("n", CompletionHint.Literal)})),
                    shape276));

            var ClearMaterializedViewStatistics = Command("ClearMaterializedViewStatistics", 
                Custom(
                    Token("clear", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("statistics"),
                    shape275));

            var ShowMaterializedViewStatistics = Command("ShowMaterializedViewStatistics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("statistics"),
                    shape275));

            var ShowMaterializedViewDiagnostics = Command("ShowMaterializedViewDiagnostics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("diagnostics"),
                    Optional(
                        fragment0),
                    shape276));

            var ShowMaterializedViewFailures = Command("ShowMaterializedViewFailures", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("failures"),
                    shape275));

            var ShowMemory = Command("ShowMemory", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("memory"),
                    Optional(Token("details")),
                    shape25));

            var CancelOperation = Command("CancelOperation", 
                Custom(
                    Token("cancel", CompletionKind.CommandPrefix),
                    Token("operation"),
                    rules.AnyGuidLiteralOrString,
                    Optional(
                        fragment0),
                    new [] {CD(), CD(), CD("obj", CompletionHint.Literal), CD(isOptional: true)}));

            var DisablePlugin = Command("DisablePlugin", 
                Custom(
                    Token("disable", CompletionKind.CommandPrefix),
                    Token("plugin"),
                    Best(
                        rules.StringLiteral,
                        rules.NameDeclaration),
                    new [] {CD(), CD(), CD("pluginName", CompletionHint.Literal)}));

            var EnablePlugin = Command("EnablePlugin", 
                Custom(
                    Token("enable", CompletionKind.CommandPrefix),
                    Token("plugin"),
                    Best(
                        rules.StringLiteral,
                        rules.NameDeclaration),
                    new [] {CD(), CD(), CD("name", CompletionHint.Literal)}));

            var ShowPlugins = Command("ShowPlugins", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("plugins"),
                    Optional(
                        fragment0),
                    shape25));

            var ShowPrincipalAccess = Command("ShowPrincipalAccess", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("principal"),
                    Token("access"),
                    Optional(
                        fragment0),
                    shape4));

            var ShowDatabasePurgeOperation = Command("ShowDatabasePurgeOperation", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                    Token("purge"),
                    Best(
                        Custom(
                            Token("operation"),
                            rules.AnyGuidLiteralOrString,
                            new [] {CD(), CD("obj", CompletionHint.Literal)}),
                        Custom(
                            Token("operations"),
                            Optional(
                                Custom(
                                    rules.AnyGuidLiteralOrString,
                                    shape277)),
                            new [] {CD(), CD(CompletionHint.Literal, isOptional: true)})),
                    new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD()}));

            var ShowQueryExecution = Command("ShowQueryExecution", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("queryexecution"),
                    fragment55,
                    new [] {CD(), CD(), CD("queryText")}));

            var AlterPoliciesOfRetention = Command("AlterPoliciesOfRetention", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("policies"),
                    Token("of"),
                    Token("retention"),
                    Optional(Token("internal")),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD(), CD(), CD(isOptional: true), CD("policies", CompletionHint.Literal)}));

            var DeletePoliciesOfRetention = Command("DeletePoliciesOfRetention", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("policies"),
                    Token("of"),
                    Token("retention"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.StringLiteral,
                            CD("entity", CompletionHint.Literal))),
                    Token(")"),
                    new [] {CD(), CD(), CD(), CD(), CD(), CD(CompletionHint.Literal), CD()}));

            var CreateRowStore = Command("CreateRowStore", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("rowstore"),
                    Optional(
                        fragment0),
                    shape25));

            var DropRowStore = Command("DropRowStore", 
                Custom(
                    Token("drop", "detach"),
                    Token("rowstore"),
                    rules.NameDeclaration,
                    Optional(Token("ifexists")),
                    new [] {CD(), CD(), CD("rowStoreName", CompletionHint.None), CD(isOptional: true)}));

            var ShowRowStore = Command("ShowRowStore", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("rowstore"),
                    If(Not(And(Token("transactions", "seals"))), rules.NameDeclaration),
                    new [] {CD(), CD(), CD("rowStoreName", CompletionHint.None)}));

            var ShowRowStores = Command("ShowRowStores", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("rowstores")));

            var ShowRowStoreTransactions = Command("ShowRowStoreTransactions", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("rowstore"),
                    Token("transactions")));

            var ShowRowStoreSeals = Command("ShowRowStoreSeals", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("rowstore"),
                    Token("seals"),
                    rules.StringLiteral,
                    Optional(
                        fragment0),
                    new [] {CD(), CD(), CD(), CD("tableName", CompletionHint.Literal), CD(isOptional: true)}));

            var ShowSchema = Command("ShowSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Optional(Token("cluster")),
                    Token("schema"),
                    Optional(
                        Best(
                            Token("details"),
                            Custom(
                                Token("as"),
                                Token("json")))),
                    shape280));

            var ShowCallStacks = Command("ShowCallStacks", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("callstacks"),
                    Optional(
                        fragment0),
                    shape25));

            var ShowFileSystem = Command("ShowFileSystem", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("filesystem"),
                    Optional(
                        fragment0),
                    shape25));

            var ShowRunningCallouts = Command("ShowRunningCallouts", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("running"),
                    Token("callouts")));

            var ShowStreamingIngestionFailures = Command("ShowStreamingIngestionFailures", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("streamingingestion"),
                    Token("failures")));

            var ShowStreamingIngestionStatistics = Command("ShowStreamingIngestionStatistics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("streamingingestion"),
                    Token("statistics")));

            var AlterTableRowStoreReferencesDropKey = Command("AlterTableRowStoreReferencesDropKey", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("rowstore_references"),
                    Token("drop"),
                    Token("key"),
                    rules.StringLiteral,
                    Optional(
                        fragment0),
                    shape282));

            var AlterTableRowStoreReferencesDropRowStore = Command("AlterTableRowStoreReferencesDropRowStore", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("rowstore_references"),
                    Token("drop"),
                    Token("rowstore"),
                    rules.NameDeclaration,
                    Optional(
                        fragment0),
                    shape283));

            var AlterTableRowStoreReferencesDropBlockedKeys = Command("AlterTableRowStoreReferencesDropBlockedKeys", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("rowstore_references"),
                    Token("drop"),
                    Token("blocked"),
                    Token("keys"),
                    Optional(
                        fragment0),
                    shape284));

            var AlterTableRowStoreReferencesDisableKey = Command("AlterTableRowStoreReferencesDisableKey", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("rowstore_references"),
                    Token("disable"),
                    Token("key"),
                    rules.StringLiteral,
                    Optional(
                        fragment0),
                    shape282));

            var AlterTableRowStoreReferencesDisableRowStore = Command("AlterTableRowStoreReferencesDisableRowStore", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("rowstore_references"),
                    Token("disable"),
                    Token("rowstore"),
                    rules.NameDeclaration,
                    Optional(
                        fragment0),
                    shape283));

            var AlterTableRowStoreReferencesDisableBlockedKeys = Command("AlterTableRowStoreReferencesDisableBlockedKeys", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("rowstore_references"),
                    Token("disable"),
                    Token("blocked"),
                    Token("keys"),
                    Optional(
                        fragment0),
                    shape284));

            var SetTableRowStoreReferences = Command("SetTableRowStoreReferences", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("rowstore_references"),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("references", CompletionHint.Literal)}));

            var ShowTableRowStoreReferences = Command("ShowTableRowStoreReferences", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("rowstore_references"),
                    shape28));

            var AlterTableColumnStatistics = Command("AlterTableColumnStatistics", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("column"),
                    Token("statistics"),
                    ZeroOrMoreCommaList(
                        Custom(
                            rules.NameDeclaration,
                            rules.StringLiteral,
                            new [] {CD("c2", CompletionHint.None), CD("statisticsValues2", CompletionHint.Literal)})),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(CompletionHint.None)}));

            var AlterTableColumnStatisticsMethod = Command("AlterTableColumnStatisticsMethod", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("column"),
                    Token("statistics"),
                    Token("method"),
                    Token("="),
                    rules.StringLiteral,
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD(), CD("newMethod", CompletionHint.Literal)}));

            var ShowTableColumnStatitics = Command("ShowTableColumnStatitics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("column"),
                    Token("statistics"),
                    shape103));

            var ShowTableDimensions = Command("ShowTableDimensions", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("dimensions"),
                    shape28));

            var DeleteTableRecords = Command("DeleteTableRecords", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    Token("table"),
                    rules.TableNameReference,
                    Token("records"),
                    Optional(
                        fragment0),
                    fragment55,
                    new [] {CD(), CD(isOptional: true), CD(), CD("TableName", CompletionHint.Table), CD(), CD(isOptional: true), CD("csl")}));

            var TableDataUpdate = Command("TableDataUpdate", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("update", CompletionKind.CommandPrefix),
                        Optional(Token("async")),
                        Token("table"),
                        rules.TableNameReference,
                        Token("delete"),
                        rules.NameDeclaration,
                        Token("append"),
                        rules.NameDeclaration,
                        Optional(
                            fragment0),
                        fragment55}
                    ,
                    new [] {CD(), CD(isOptional: true), CD(), CD("TableName", CompletionHint.Table), CD(), CD("DeleteIdentifier", CompletionHint.None), CD(), CD("AppendIdentifier", CompletionHint.None), CD(isOptional: true), CD("csl")}));

            var DeleteMaterializedViewRecords = Command("DeleteMaterializedViewRecords", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("records"),
                    Optional(
                        fragment0),
                    fragment55,
                    new [] {CD(), CD(isOptional: true), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(isOptional: true), CD("csl")}));

            var ShowTableColumnsClassification = Command("ShowTableColumnsClassification", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("columns"),
                    Token("classification"),
                    shape103));

            var ShowTableRowStores = Command("ShowTableRowStores", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("rowstores"),
                    shape285));

            var ShowTableRowStoreSealInfo = Command("ShowTableRowStoreSealInfo", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("rowstore_sealinfo"),
                    shape285));

            var ShowTablesColumnStatistics = Command("ShowTablesColumnStatistics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("column"),
                    Token("statistics"),
                    Token("older"),
                    rules.Value,
                    new [] {CD(), CD(), CD(), CD(), CD(), CD("outdatewindow", CompletionHint.Literal)}));

            var ShowTableDataStatistics = Command("ShowTableDataStatistics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("data"),
                    Token("statistics"),
                    Optional(
                        Custom(
                            Token("with"),
                            Token("("),
                            OneOrMoreCommaList(
                                Custom(
                                    Best(
                                        Token("samplepercent"),
                                        Token("scope"),
                                        Token("from"),
                                        Token("to"),
                                        If(Not(And(Token("samplepercent", "scope", "from", "to"))), rules.NameDeclaration)),
                                    Token("="),
                                    rules.Value,
                                    shape31)),
                            Token(")"))),
                    shape286));

            var CreateTempStorage = Command("CreateTempStorage", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("tempstorage")));

            var DropTempStorage = Command("DropTempStorage", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("tempstorage"),
                    Token("older"),
                    rules.Value,
                    new [] {CD(), CD(), CD(), CD("olderThan", CompletionHint.Literal)}));

            var DropStoredQueryResultContainers = Command("DropStoredQueryResultContainers", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("storedqueryresultcontainers"),
                    rules.DatabaseNameReference,
                    OneOrMoreList(
                        Custom(
                            rules.AnyGuidLiteralOrString,
                            shape287)),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(CompletionHint.Literal)}));

            var DropUnusedStoredQueryResultContainers = Command("DropUnusedStoredQueryResultContainers", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("unused"),
                    Token("storedqueryresultcontainers"),
                    rules.DatabaseNameReference,
                    new [] {CD(), CD(), CD(), CD("databaseName", CompletionHint.Database)}));

            var EnableDatabaseMaintenanceMode = Command("EnableDatabaseMaintenanceMode", 
                Custom(
                    Token("enable", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("streamingingestion_maintenance_mode")), rules.DatabaseNameReference),
                    Token("maintenance_mode"),
                    shape201));

            var DisableDatabaseMaintenanceMode = Command("DisableDatabaseMaintenanceMode", 
                Custom(
                    Token("disable", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("streamingingestion_maintenance_mode")), rules.DatabaseNameReference),
                    Token("maintenance_mode"),
                    shape201));

            var EnableDatabaseStreamingIngestionMaintenanceMode = Command("EnableDatabaseStreamingIngestionMaintenanceMode", 
                Custom(
                    Token("enable", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("streamingingestion_maintenance_mode")));

            var DisableDatabaseStreamingIngestionMaintenanceMode = Command("DisableDatabaseStreamingIngestionMaintenanceMode", 
                Custom(
                    Token("disable", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("streamingingestion_maintenance_mode")));

            var ShowQueryCallTree = Command("ShowQueryCallTree", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("query"),
                    Token("call-tree"),
                    fragment55,
                    new [] {CD(), CD(), CD(), CD("queryText")}));

            var ShowExtentCorruptedDatetime = Command("ShowExtentCorruptedDatetime", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Optional(
                        fragment67),
                    Token("extents"),
                    Custom(
                        Token("corrupted"),
                        Token("datetime")).Hide(),
                    shape289));

            var PatchExtentCorruptedDatetime = Command("PatchExtentCorruptedDatetime", 
                Custom(
                    Token("patch"),
                    Optional(
                        fragment67),
                    Token("extents"),
                    Token("corrupted"),
                    Token("datetime"),
                    new [] {CD(), CD(isOptional: true), CD(), CD(), CD()}).Hide());

            var ClearClusterCredStoreCache = Command("ClearClusterCredStoreCache", 
                Custom(
                    Token("clear", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("cache"),
                    Token("credstore")));

            var ClearClusterGroupMembershipCache = Command("ClearClusterGroupMembershipCache", 
                Custom(
                    Token("clear", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("cache"),
                    Token("groupmembership"),
                    Optional(
                        fragment10),
                    shape172));

            var ClearExternalArtifactsCache = Command("ClearExternalArtifactsCache", 
                Custom(
                    Token("clear", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("cache"),
                    Token("external-artifacts"),
                    Custom(
                        Token("("),
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                CD("ArtifactUri", CompletionHint.Literal))),
                        Token(")"),
                        shape137)));

            var ShowDatabasesEntities = Command("ShowDatabasesEntities", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("entities"),
                    Optional(
                        fragment10),
                    shape4));

            var ShowDatabaseEntity = Command("ShowDatabaseEntity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("with", "details", "verbose", "identity", "policies", "datastats", "metadata", "schema", "cslschema", "kqlschema", "ingestion", "*", "policy", "principals", "keyvault", "extents", "extent", "shard-groups", "data", "cache"))), rules.DatabaseNameReference),
                    Token("entity"),
                    rules.NameDeclaration,
                    Optional(
                        fragment10),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD("EntityName", CompletionHint.None), CD(isOptional: true)}));

            var ReplaceDatabaseKeyVaultSecrets = Command("ReplaceDatabaseKeyVaultSecrets", 
                Custom(
                    Token("replace"),
                    Token("database"),
                    Token("keyvaultsecrets"),
                    rules.NameDeclaration,
                    new [] {CD(), CD(), CD(), CD("secrets", CompletionHint.None)}).Hide());

            var commandParsers = new CommandParserInfo[]
            {
                new CommandParserInfo("ShowDatabase", ShowDatabase),
                new CommandParserInfo("ShowDatabaseDetails", ShowDatabaseDetails),
                new CommandParserInfo("ShowDatabaseIdentity", ShowDatabaseIdentity),
                new CommandParserInfo("ShowDatabasePolicies", ShowDatabasePolicies),
                new CommandParserInfo("ShowDatabaseDataStats", ShowDatabaseDataStats),
                new CommandParserInfo("ShowDatabaseMetadata", ShowDatabaseMetadata),
                new CommandParserInfo("ShowClusterDatabases", ShowClusterDatabases),
                new CommandParserInfo("ShowClusterDatabasesDetails", ShowClusterDatabasesDetails),
                new CommandParserInfo("ShowClusterDatabasesIdentity", ShowClusterDatabasesIdentity),
                new CommandParserInfo("ShowClusterDatabasesPolicies", ShowClusterDatabasesPolicies),
                new CommandParserInfo("ShowClusterDatabasesDataStats", ShowClusterDatabasesDataStats),
                new CommandParserInfo("ShowClusterDatabasesMetadata", ShowClusterDatabasesMetadata),
                new CommandParserInfo("CreateDatabase", CreateDatabase),
                new CommandParserInfo("AttachDatabase", AttachDatabase),
                new CommandParserInfo("DetachDatabase", DetachDatabase),
                new CommandParserInfo("AlterDatabasePrettyName", AlterDatabasePrettyName),
                new CommandParserInfo("DropDatabasePrettyName", DropDatabasePrettyName),
                new CommandParserInfo("AlterDatabasePersistMetadata", AlterDatabasePersistMetadata),
                new CommandParserInfo("SetAccess", SetAccess),
                new CommandParserInfo("ShowDatabaseSchema", ShowDatabaseSchema),
                new CommandParserInfo("ShowDatabaseSchemaAsJson", ShowDatabaseSchemaAsJson),
                new CommandParserInfo("ShowDatabaseSchemaAsCslScript", ShowDatabaseSchemaAsCslScript),
                new CommandParserInfo("ShowDatabaseCslSchema", ShowDatabaseCslSchema),
                new CommandParserInfo("ShowDatabaseSchemaViolations", ShowDatabaseSchemaViolations),
                new CommandParserInfo("ShowDatabasesSchema", ShowDatabasesSchema),
                new CommandParserInfo("ShowDatabasesSchemaAsJson", ShowDatabasesSchemaAsJson),
                new CommandParserInfo("CreateDatabaseIngestionMapping", CreateDatabaseIngestionMapping),
                new CommandParserInfo("CreateOrAlterDatabaseIngestionMapping", CreateOrAlterDatabaseIngestionMapping),
                new CommandParserInfo("AlterDatabaseIngestionMapping", AlterDatabaseIngestionMapping),
                new CommandParserInfo("AlterMergeDatabaseIngestionMapping", AlterMergeDatabaseIngestionMapping),
                new CommandParserInfo("ShowDatabaseIngestionMappings", ShowDatabaseIngestionMappings),
                new CommandParserInfo("ShowIngestionMappings", ShowIngestionMappings),
                new CommandParserInfo("DropDatabaseIngestionMapping", DropDatabaseIngestionMapping),
                new CommandParserInfo("ShowTables", ShowTables),
                new CommandParserInfo("ShowTable", ShowTable),
                new CommandParserInfo("ShowTablesDetails", ShowTablesDetails),
                new CommandParserInfo("ShowTableDetails", ShowTableDetails),
                new CommandParserInfo("ShowTableCslSchema", ShowTableCslSchema),
                new CommandParserInfo("ShowTableSchemaAsJson", ShowTableSchemaAsJson),
                new CommandParserInfo("CreateTable", CreateTable),
                new CommandParserInfo("CreateTableBasedOnAnother", CreateTableBasedOnAnother),
                new CommandParserInfo("CreateMergeTable", CreateMergeTable),
                new CommandParserInfo("DefineTable", DefineTable),
                new CommandParserInfo("CreateTables", CreateTables),
                new CommandParserInfo("CreateMergeTables", CreateMergeTables),
                new CommandParserInfo("DefineTables", DefineTables),
                new CommandParserInfo("AlterTable", AlterTable),
                new CommandParserInfo("AlterMergeTable", AlterMergeTable),
                new CommandParserInfo("AlterTableDocString", AlterTableDocString),
                new CommandParserInfo("AlterTableFolder", AlterTableFolder),
                new CommandParserInfo("RenameTable", RenameTable),
                new CommandParserInfo("RenameTables", RenameTables),
                new CommandParserInfo("UndoDropExtentContainer", UndoDropExtentContainer),
                new CommandParserInfo("DropTable", DropTable),
                new CommandParserInfo("UndoDropTable", UndoDropTable),
                new CommandParserInfo("DropTables", DropTables),
                new CommandParserInfo("CreateTableIngestionMapping", CreateTableIngestionMapping),
                new CommandParserInfo("CreateOrAlterTableIngestionMapping", CreateOrAlterTableIngestionMapping),
                new CommandParserInfo("AlterTableIngestionMapping", AlterTableIngestionMapping),
                new CommandParserInfo("AlterMergeTableIngestionMapping", AlterMergeTableIngestionMapping),
                new CommandParserInfo("ShowTableIngestionMappings", ShowTableIngestionMappings),
                new CommandParserInfo("ShowTableIngestionMapping", ShowTableIngestionMapping),
                new CommandParserInfo("DropTableIngestionMapping", DropTableIngestionMapping),
                new CommandParserInfo("RenameColumn", RenameColumn),
                new CommandParserInfo("RenameColumns", RenameColumns),
                new CommandParserInfo("AlterColumnType", AlterColumnType),
                new CommandParserInfo("DropColumn", DropColumn),
                new CommandParserInfo("DropTableColumns", DropTableColumns),
                new CommandParserInfo("AlterTableColumnDocStrings", AlterTableColumnDocStrings),
                new CommandParserInfo("AlterMergeTableColumnDocStrings", AlterMergeTableColumnDocStrings),
                new CommandParserInfo("ShowFunctions", ShowFunctions),
                new CommandParserInfo("ShowFunction", ShowFunction),
                new CommandParserInfo("CreateFunction", CreateFunction),
                new CommandParserInfo("AlterFunction", AlterFunction),
                new CommandParserInfo("CreateOrAlterFunction", CreateOrAlterFunction),
                new CommandParserInfo("DropFunction", DropFunction),
                new CommandParserInfo("DropFunctions", DropFunctions),
                new CommandParserInfo("AlterFunctionDocString", AlterFunctionDocString),
                new CommandParserInfo("AlterFunctionFolder", AlterFunctionFolder),
                new CommandParserInfo("ShowExternalTables", ShowExternalTables),
                new CommandParserInfo("ShowExternalTable", ShowExternalTable),
                new CommandParserInfo("ShowExternalTablesDetails", ShowExternalTablesDetails),
                new CommandParserInfo("ShowExternalTableDetails", ShowExternalTableDetails),
                new CommandParserInfo("ShowExternalTableCslSchema", ShowExternalTableCslSchema),
                new CommandParserInfo("ShowExternalTableSchema", ShowExternalTableSchema),
                new CommandParserInfo("ShowExternalTableArtifacts", ShowExternalTableArtifacts),
                new CommandParserInfo("DropExternalTable", DropExternalTable),
                new CommandParserInfo("CreateStorageExternalTable", CreateStorageExternalTable),
                new CommandParserInfo("AlterStorageExternalTable", AlterStorageExternalTable),
                new CommandParserInfo("CreateOrAlterStorageExternalTable", CreateOrAlterStorageExternalTable),
                new CommandParserInfo("CreateSqlExternalTable", CreateSqlExternalTable),
                new CommandParserInfo("AlterSqlExternalTable", AlterSqlExternalTable),
                new CommandParserInfo("CreateOrAlterSqlExternalTable", CreateOrAlterSqlExternalTable),
                new CommandParserInfo("CreateExternalTableMapping", CreateExternalTableMapping),
                new CommandParserInfo("SetExternalTableAdmins", SetExternalTableAdmins),
                new CommandParserInfo("AddExternalTableAdmins", AddExternalTableAdmins),
                new CommandParserInfo("DropExternalTableAdmins", DropExternalTableAdmins),
                new CommandParserInfo("AlterExternalTableDocString", AlterExternalTableDocString),
                new CommandParserInfo("AlterExternalTableFolder", AlterExternalTableFolder),
                new CommandParserInfo("ShowExternalTablePrincipals", ShowExternalTablePrincipals),
                new CommandParserInfo("ShowFabric", ShowFabric),
                new CommandParserInfo("AlterExternalTableMapping", AlterExternalTableMapping),
                new CommandParserInfo("ShowExternalTableMappings", ShowExternalTableMappings),
                new CommandParserInfo("ShowExternalTableMapping", ShowExternalTableMapping),
                new CommandParserInfo("DropExternalTableMapping", DropExternalTableMapping),
                new CommandParserInfo("ShowWorkloadGroups", ShowWorkloadGroups),
                new CommandParserInfo("ShowWorkloadGroup", ShowWorkloadGroup),
                new CommandParserInfo("CreateOrAleterWorkloadGroup", CreateOrAleterWorkloadGroup),
                new CommandParserInfo("AlterMergeWorkloadGroup", AlterMergeWorkloadGroup),
                new CommandParserInfo("DropWorkloadGroup", DropWorkloadGroup),
                new CommandParserInfo("ShowDatabasePolicyCaching", ShowDatabasePolicyCaching),
                new CommandParserInfo("ShowTablePolicyCaching", ShowTablePolicyCaching),
                new CommandParserInfo("ShowTableStarPolicyCaching", ShowTableStarPolicyCaching),
                new CommandParserInfo("ShowColumnPolicyCaching", ShowColumnPolicyCaching),
                new CommandParserInfo("ShowMaterializedViewPolicyCaching", ShowMaterializedViewPolicyCaching),
                new CommandParserInfo("ShowGraphModelPolicyCaching", ShowGraphModelPolicyCaching),
                new CommandParserInfo("ShowGraphModelStarPolicyCaching", ShowGraphModelStarPolicyCaching),
                new CommandParserInfo("ShowClusterPolicyCaching", ShowClusterPolicyCaching),
                new CommandParserInfo("AlterDatabasePolicyCaching", AlterDatabasePolicyCaching),
                new CommandParserInfo("AlterTablePolicyCaching", AlterTablePolicyCaching),
                new CommandParserInfo("AlterTablesPolicyCaching", AlterTablesPolicyCaching),
                new CommandParserInfo("AlterColumnPolicyCaching", AlterColumnPolicyCaching),
                new CommandParserInfo("AlterMaterializedViewPolicyCaching", AlterMaterializedViewPolicyCaching),
                new CommandParserInfo("AlterGraphModelPolicyCaching", AlterGraphModelPolicyCaching),
                new CommandParserInfo("AlterClusterPolicyCaching", AlterClusterPolicyCaching),
                new CommandParserInfo("DeleteDatabasePolicyCaching", DeleteDatabasePolicyCaching),
                new CommandParserInfo("DeleteTablePolicyCaching", DeleteTablePolicyCaching),
                new CommandParserInfo("DeleteColumnPolicyCaching", DeleteColumnPolicyCaching),
                new CommandParserInfo("DeleteMaterializedViewPolicyCaching", DeleteMaterializedViewPolicyCaching),
                new CommandParserInfo("DeleteGraphModelPolicyCaching", DeleteGraphModelPolicyCaching),
                new CommandParserInfo("DeleteClusterPolicyCaching", DeleteClusterPolicyCaching),
                new CommandParserInfo("ShowTablePolicyIngestionTime", ShowTablePolicyIngestionTime),
                new CommandParserInfo("ShowTableStarPolicyIngestionTime", ShowTableStarPolicyIngestionTime),
                new CommandParserInfo("AlterTablePolicyIngestionTime", AlterTablePolicyIngestionTime),
                new CommandParserInfo("AlterTablesPolicyIngestionTime", AlterTablesPolicyIngestionTime),
                new CommandParserInfo("DeleteTablePolicyIngestionTime", DeleteTablePolicyIngestionTime),
                new CommandParserInfo("ShowTablePolicyRetention", ShowTablePolicyRetention),
                new CommandParserInfo("ShowTableStarPolicyRetention", ShowTableStarPolicyRetention),
                new CommandParserInfo("ShowGraphPolicyRetention", ShowGraphPolicyRetention),
                new CommandParserInfo("ShowGraphStarPolicyRetention", ShowGraphStarPolicyRetention),
                new CommandParserInfo("ShowDatabasePolicyRetention", ShowDatabasePolicyRetention),
                new CommandParserInfo("AlterTablePolicyRetention", AlterTablePolicyRetention),
                new CommandParserInfo("AlterMaterializedViewPolicyRetention", AlterMaterializedViewPolicyRetention),
                new CommandParserInfo("AlterDatabasePolicyRetention", AlterDatabasePolicyRetention),
                new CommandParserInfo("AlterGraphModelPolicyRetention", AlterGraphModelPolicyRetention),
                new CommandParserInfo("AlterTablesPolicyRetention", AlterTablesPolicyRetention),
                new CommandParserInfo("AlterMergeTablePolicyRetention", AlterMergeTablePolicyRetention),
                new CommandParserInfo("AlterMergeMaterializedViewPolicyRetention", AlterMergeMaterializedViewPolicyRetention),
                new CommandParserInfo("AlterMergeDatabasePolicyRetention", AlterMergeDatabasePolicyRetention),
                new CommandParserInfo("DeleteTablePolicyRetention", DeleteTablePolicyRetention),
                new CommandParserInfo("DeleteDatabasePolicyRetention", DeleteDatabasePolicyRetention),
                new CommandParserInfo("ShowDatabasePolicyHardRetentionViolations", ShowDatabasePolicyHardRetentionViolations),
                new CommandParserInfo("ShowDatabasePolicySoftRetentionViolations", ShowDatabasePolicySoftRetentionViolations),
                new CommandParserInfo("ShowTablePolicyRowLevelSecurity", ShowTablePolicyRowLevelSecurity),
                new CommandParserInfo("ShowTableStarPolicyRowLevelSecurity", ShowTableStarPolicyRowLevelSecurity),
                new CommandParserInfo("AlterTablePolicyRowLevelSecurity", AlterTablePolicyRowLevelSecurity),
                new CommandParserInfo("DeleteTablePolicyRowLevelSecurity", DeleteTablePolicyRowLevelSecurity),
                new CommandParserInfo("ShowMaterializedViewPolicyRowLevelSecurity", ShowMaterializedViewPolicyRowLevelSecurity),
                new CommandParserInfo("AlterMaterializedViewPolicyRowLevelSecurity", AlterMaterializedViewPolicyRowLevelSecurity),
                new CommandParserInfo("DeleteMaterializedViewPolicyRowLevelSecurity", DeleteMaterializedViewPolicyRowLevelSecurity),
                new CommandParserInfo("ShowTablePolicyRowOrder", ShowTablePolicyRowOrder),
                new CommandParserInfo("ShowTableStarPolicyRowOrder", ShowTableStarPolicyRowOrder),
                new CommandParserInfo("AlterTablePolicyRowOrder", AlterTablePolicyRowOrder),
                new CommandParserInfo("AlterTablesPolicyRowOrder", AlterTablesPolicyRowOrder),
                new CommandParserInfo("AlterMergeTablePolicyRowOrder", AlterMergeTablePolicyRowOrder),
                new CommandParserInfo("DeleteTablePolicyRowOrder", DeleteTablePolicyRowOrder),
                new CommandParserInfo("ShowTablePolicyUpdate", ShowTablePolicyUpdate),
                new CommandParserInfo("ShowTableStarPolicyUpdate", ShowTableStarPolicyUpdate),
                new CommandParserInfo("AlterTablePolicyUpdate", AlterTablePolicyUpdate),
                new CommandParserInfo("AlterMergeTablePolicyUpdate", AlterMergeTablePolicyUpdate),
                new CommandParserInfo("DeleteTablePolicyUpdate", DeleteTablePolicyUpdate),
                new CommandParserInfo("ShowClusterPolicyIngestionBatching", ShowClusterPolicyIngestionBatching),
                new CommandParserInfo("ShowDatabasePolicyIngestionBatching", ShowDatabasePolicyIngestionBatching),
                new CommandParserInfo("ShowTablePolicyIngestionBatching", ShowTablePolicyIngestionBatching),
                new CommandParserInfo("ShowTableStarPolicyIngestionBatching", ShowTableStarPolicyIngestionBatching),
                new CommandParserInfo("AlterClusterPolicyIngestionBatching", AlterClusterPolicyIngestionBatching),
                new CommandParserInfo("AlterMergeClusterPolicyIngestionBatching", AlterMergeClusterPolicyIngestionBatching),
                new CommandParserInfo("AlterDatabasePolicyIngestionBatching", AlterDatabasePolicyIngestionBatching),
                new CommandParserInfo("AlterMergeDatabasePolicyIngestionBatching", AlterMergeDatabasePolicyIngestionBatching),
                new CommandParserInfo("AlterTablePolicyIngestionBatching", AlterTablePolicyIngestionBatching),
                new CommandParserInfo("AlterMergeTablePolicyIngestionBatching", AlterMergeTablePolicyIngestionBatching),
                new CommandParserInfo("AlterTablesPolicyIngestionBatching", AlterTablesPolicyIngestionBatching),
                new CommandParserInfo("DeleteDatabasePolicyIngestionBatching", DeleteDatabasePolicyIngestionBatching),
                new CommandParserInfo("DeleteTablePolicyIngestionBatching", DeleteTablePolicyIngestionBatching),
                new CommandParserInfo("ShowDatabasePolicyEncoding", ShowDatabasePolicyEncoding),
                new CommandParserInfo("ShowTablePolicyEncoding", ShowTablePolicyEncoding),
                new CommandParserInfo("ShowColumnPolicyEncoding", ShowColumnPolicyEncoding),
                new CommandParserInfo("AlterDatabasePolicyEncoding", AlterDatabasePolicyEncoding),
                new CommandParserInfo("AlterTablePolicyEncoding", AlterTablePolicyEncoding),
                new CommandParserInfo("AlterTableColumnsPolicyEncoding", AlterTableColumnsPolicyEncoding),
                new CommandParserInfo("AlterColumnPolicyEncoding", AlterColumnPolicyEncoding),
                new CommandParserInfo("AlterColumnsPolicyEncodingByQuery", AlterColumnsPolicyEncodingByQuery),
                new CommandParserInfo("AlterColumnPolicyEncodingType", AlterColumnPolicyEncodingType),
                new CommandParserInfo("AlterMergeDatabasePolicyEncoding", AlterMergeDatabasePolicyEncoding),
                new CommandParserInfo("AlterMergeTablePolicyEncoding", AlterMergeTablePolicyEncoding),
                new CommandParserInfo("AlterMergeColumnPolicyEncoding", AlterMergeColumnPolicyEncoding),
                new CommandParserInfo("DeleteDatabasePolicyEncoding", DeleteDatabasePolicyEncoding),
                new CommandParserInfo("DeleteTablePolicyEncoding", DeleteTablePolicyEncoding),
                new CommandParserInfo("DeleteColumnPolicyEncoding", DeleteColumnPolicyEncoding),
                new CommandParserInfo("ShowDatabasePolicyMerge", ShowDatabasePolicyMerge),
                new CommandParserInfo("ShowTablePolicyMerge", ShowTablePolicyMerge),
                new CommandParserInfo("ShowTableStarPolicyMerge", ShowTableStarPolicyMerge),
                new CommandParserInfo("AlterDatabasePolicyMerge", AlterDatabasePolicyMerge),
                new CommandParserInfo("AlterTablePolicyMerge", AlterTablePolicyMerge),
                new CommandParserInfo("AlterTablesPolicyMerge", AlterTablesPolicyMerge),
                new CommandParserInfo("AlterMergeDatabasePolicyMerge", AlterMergeDatabasePolicyMerge),
                new CommandParserInfo("AlterMergeTablePolicyMerge", AlterMergeTablePolicyMerge),
                new CommandParserInfo("AlterMergeMaterializedViewPolicyMerge", AlterMergeMaterializedViewPolicyMerge),
                new CommandParserInfo("DeleteDatabasePolicyMerge", DeleteDatabasePolicyMerge),
                new CommandParserInfo("DeleteTablePolicyMerge", DeleteTablePolicyMerge),
                new CommandParserInfo("ShowExternalTablePolicyQueryAcceleration", ShowExternalTablePolicyQueryAcceleration),
                new CommandParserInfo("ShowExternalTablesPolicyQueryAcceleration", ShowExternalTablesPolicyQueryAcceleration),
                new CommandParserInfo("AlterExternalTablePolicyQueryAcceleration", AlterExternalTablePolicyQueryAcceleration),
                new CommandParserInfo("AlterMergeExternalTablePolicyQueryAcceleration", AlterMergeExternalTablePolicyQueryAcceleration),
                new CommandParserInfo("DeleteExternalTablePolicyQueryAcceleration", DeleteExternalTablePolicyQueryAcceleration),
                new CommandParserInfo("ShowExternalTableQueryAccelerationStatatistics", ShowExternalTableQueryAccelerationStatatistics),
                new CommandParserInfo("ShowExternalTablesQueryAccelerationStatatistics", ShowExternalTablesQueryAccelerationStatatistics),
                new CommandParserInfo("AlterTablePolicyMirroring", AlterTablePolicyMirroring),
                new CommandParserInfo("AlterMergeTablePolicyMirroring", AlterMergeTablePolicyMirroring),
                new CommandParserInfo("AlterTablePolicyMirroringWithJson", AlterTablePolicyMirroringWithJson),
                new CommandParserInfo("AlterMergeTablePolicyMirroringWithJson", AlterMergeTablePolicyMirroringWithJson),
                new CommandParserInfo("DeleteTablePolicyMirroring", DeleteTablePolicyMirroring),
                new CommandParserInfo("ShowTablePolicyMirroring", ShowTablePolicyMirroring),
                new CommandParserInfo("ShowTableStarPolicyMirroring", ShowTableStarPolicyMirroring),
                new CommandParserInfo("CreateMirroringTemplate", CreateMirroringTemplate),
                new CommandParserInfo("CreateOrAlterMirroringTemplate", CreateOrAlterMirroringTemplate),
                new CommandParserInfo("AlterMirroringTemplate", AlterMirroringTemplate),
                new CommandParserInfo("AlterMergeMirroringTemplate", AlterMergeMirroringTemplate),
                new CommandParserInfo("DeleteMirroringTemplate", DeleteMirroringTemplate),
                new CommandParserInfo("ShowMirroringTemplate", ShowMirroringTemplate),
                new CommandParserInfo("ShowMirroringTemplates", ShowMirroringTemplates),
                new CommandParserInfo("ApplyMirroringTemplate", ApplyMirroringTemplate),
                new CommandParserInfo("ShowTablePolicyPartitioning", ShowTablePolicyPartitioning),
                new CommandParserInfo("ShowTableStarPolicyPartitioning", ShowTableStarPolicyPartitioning),
                new CommandParserInfo("AlterTablePolicyPartitioning", AlterTablePolicyPartitioning),
                new CommandParserInfo("AlterMergeTablePolicyPartitioning", AlterMergeTablePolicyPartitioning),
                new CommandParserInfo("AlterMaterializedViewPolicyPartitioning", AlterMaterializedViewPolicyPartitioning),
                new CommandParserInfo("AlterMergeMaterializedViewPolicyPartitioning", AlterMergeMaterializedViewPolicyPartitioning),
                new CommandParserInfo("DeleteTablePolicyPartitioning", DeleteTablePolicyPartitioning),
                new CommandParserInfo("DeleteMaterializedViewPolicyPartitioning", DeleteMaterializedViewPolicyPartitioning),
                new CommandParserInfo("ShowTablePolicyRestrictedViewAccess", ShowTablePolicyRestrictedViewAccess),
                new CommandParserInfo("ShowTableStarPolicyRestrictedViewAccess", ShowTableStarPolicyRestrictedViewAccess),
                new CommandParserInfo("AlterTablePolicyRestrictedViewAccess", AlterTablePolicyRestrictedViewAccess),
                new CommandParserInfo("AlterTablesPolicyRestrictedViewAccess", AlterTablesPolicyRestrictedViewAccess),
                new CommandParserInfo("DeleteTablePolicyRestrictedViewAccess", DeleteTablePolicyRestrictedViewAccess),
                new CommandParserInfo("ShowClusterPolicyRowStore", ShowClusterPolicyRowStore),
                new CommandParserInfo("AlterClusterPolicyRowStore", AlterClusterPolicyRowStore),
                new CommandParserInfo("AlterMergeClusterPolicyRowStore", AlterMergeClusterPolicyRowStore),
                new CommandParserInfo("DeleteClusterPolicyRowStore", DeleteClusterPolicyRowStore),
                new CommandParserInfo("ShowClusterPolicySandbox", ShowClusterPolicySandbox),
                new CommandParserInfo("AlterClusterPolicySandbox", AlterClusterPolicySandbox),
                new CommandParserInfo("DeleteClusterPolicySandbox", DeleteClusterPolicySandbox),
                new CommandParserInfo("ShowClusterSandboxesStats", ShowClusterSandboxesStats),
                new CommandParserInfo("ShowDatabasePolicySharding", ShowDatabasePolicySharding),
                new CommandParserInfo("ShowTablePolicySharding", ShowTablePolicySharding),
                new CommandParserInfo("ShowTableStarPolicySharding", ShowTableStarPolicySharding),
                new CommandParserInfo("AlterDatabasePolicySharding", AlterDatabasePolicySharding),
                new CommandParserInfo("AlterTablePolicySharding", AlterTablePolicySharding),
                new CommandParserInfo("AlterMergeDatabasePolicySharding", AlterMergeDatabasePolicySharding),
                new CommandParserInfo("AlterMergeTablePolicySharding", AlterMergeTablePolicySharding),
                new CommandParserInfo("DeleteDatabasePolicySharding", DeleteDatabasePolicySharding),
                new CommandParserInfo("DeleteTablePolicySharding", DeleteTablePolicySharding),
                new CommandParserInfo("AlterClusterPolicySharding", AlterClusterPolicySharding),
                new CommandParserInfo("AlterMergeClusterPolicySharding", AlterMergeClusterPolicySharding),
                new CommandParserInfo("DeleteClusterPolicySharding", DeleteClusterPolicySharding),
                new CommandParserInfo("ShowClusterPolicySharding", ShowClusterPolicySharding),
                new CommandParserInfo("ShowDatabasePolicyShardsGrouping", ShowDatabasePolicyShardsGrouping),
                new CommandParserInfo("AlterDatabasePolicyShardsGrouping", AlterDatabasePolicyShardsGrouping),
                new CommandParserInfo("AlterMergeDatabasePolicyShardsGrouping", AlterMergeDatabasePolicyShardsGrouping),
                new CommandParserInfo("DeleteDatabasePolicyShardsGrouping", DeleteDatabasePolicyShardsGrouping),
                new CommandParserInfo("ShowDatabasePolicyStreamingIngestion", ShowDatabasePolicyStreamingIngestion),
                new CommandParserInfo("ShowTablePolicyStreamingIngestion", ShowTablePolicyStreamingIngestion),
                new CommandParserInfo("ShowClusterPolicyStreamingIngestion", ShowClusterPolicyStreamingIngestion),
                new CommandParserInfo("AlterDatabasePolicyStreamingIngestion", AlterDatabasePolicyStreamingIngestion),
                new CommandParserInfo("AlterMergeDatabasePolicyStreamingIngestion", AlterMergeDatabasePolicyStreamingIngestion),
                new CommandParserInfo("AlterTablePolicyStreamingIngestion", AlterTablePolicyStreamingIngestion),
                new CommandParserInfo("AlterMergeTablePolicyStreamingIngestion", AlterMergeTablePolicyStreamingIngestion),
                new CommandParserInfo("AlterClusterPolicyStreamingIngestion", AlterClusterPolicyStreamingIngestion),
                new CommandParserInfo("AlterMergeClusterPolicyStreamingIngestion", AlterMergeClusterPolicyStreamingIngestion),
                new CommandParserInfo("DeleteDatabasePolicyStreamingIngestion", DeleteDatabasePolicyStreamingIngestion),
                new CommandParserInfo("DeleteTablePolicyStreamingIngestion", DeleteTablePolicyStreamingIngestion),
                new CommandParserInfo("DeleteClusterPolicyStreamingIngestion", DeleteClusterPolicyStreamingIngestion),
                new CommandParserInfo("ShowDatabasePolicyManagedIdentity", ShowDatabasePolicyManagedIdentity),
                new CommandParserInfo("ShowClusterPolicyManagedIdentity", ShowClusterPolicyManagedIdentity),
                new CommandParserInfo("AlterDatabasePolicyManagedIdentity", AlterDatabasePolicyManagedIdentity),
                new CommandParserInfo("AlterMergeDatabasePolicyManagedIdentity", AlterMergeDatabasePolicyManagedIdentity),
                new CommandParserInfo("AlterClusterPolicyManagedIdentity", AlterClusterPolicyManagedIdentity),
                new CommandParserInfo("AlterMergeClusterPolicyManagedIdentity", AlterMergeClusterPolicyManagedIdentity),
                new CommandParserInfo("DeleteDatabasePolicyManagedIdentity", DeleteDatabasePolicyManagedIdentity),
                new CommandParserInfo("DeleteClusterPolicyManagedIdentity", DeleteClusterPolicyManagedIdentity),
                new CommandParserInfo("ShowTablePolicyAutoDelete", ShowTablePolicyAutoDelete),
                new CommandParserInfo("AlterTablePolicyAutoDelete", AlterTablePolicyAutoDelete),
                new CommandParserInfo("DeleteTablePolicyAutoDelete", DeleteTablePolicyAutoDelete),
                new CommandParserInfo("ShowClusterPolicyCallout", ShowClusterPolicyCallout),
                new CommandParserInfo("AlterClusterPolicyCallout", AlterClusterPolicyCallout),
                new CommandParserInfo("AlterMergeClusterPolicyCallout", AlterMergeClusterPolicyCallout),
                new CommandParserInfo("DeleteClusterPolicyCallout", DeleteClusterPolicyCallout),
                new CommandParserInfo("ShowClusterPolicyCapacity", ShowClusterPolicyCapacity),
                new CommandParserInfo("AlterClusterPolicyCapacity", AlterClusterPolicyCapacity),
                new CommandParserInfo("AlterMergeClusterPolicyCapacity", AlterMergeClusterPolicyCapacity),
                new CommandParserInfo("ShowClusterPolicyRequestClassification", ShowClusterPolicyRequestClassification),
                new CommandParserInfo("AlterClusterPolicyRequestClassification", AlterClusterPolicyRequestClassification),
                new CommandParserInfo("AlterMergeClusterPolicyRequestClassification", AlterMergeClusterPolicyRequestClassification),
                new CommandParserInfo("DeleteClusterPolicyRequestClassification", DeleteClusterPolicyRequestClassification),
                new CommandParserInfo("ShowClusterPolicyMultiDatabaseAdmins", ShowClusterPolicyMultiDatabaseAdmins),
                new CommandParserInfo("AlterClusterPolicyMultiDatabaseAdmins", AlterClusterPolicyMultiDatabaseAdmins),
                new CommandParserInfo("AlterMergeClusterPolicyMultiDatabaseAdmins", AlterMergeClusterPolicyMultiDatabaseAdmins),
                new CommandParserInfo("ShowDatabasePolicyDiagnostics", ShowDatabasePolicyDiagnostics),
                new CommandParserInfo("ShowClusterPolicyDiagnostics", ShowClusterPolicyDiagnostics),
                new CommandParserInfo("AlterDatabasePolicyDiagnostics", AlterDatabasePolicyDiagnostics),
                new CommandParserInfo("AlterMergeDatabasePolicyDiagnostics", AlterMergeDatabasePolicyDiagnostics),
                new CommandParserInfo("AlterClusterPolicyDiagnostics", AlterClusterPolicyDiagnostics),
                new CommandParserInfo("AlterMergeClusterPolicyDiagnostics", AlterMergeClusterPolicyDiagnostics),
                new CommandParserInfo("DeleteDatabasePolicyDiagnostics", DeleteDatabasePolicyDiagnostics),
                new CommandParserInfo("ShowClusterPolicyQueryWeakConsistency", ShowClusterPolicyQueryWeakConsistency),
                new CommandParserInfo("AlterClusterPolicyQueryWeakConsistency", AlterClusterPolicyQueryWeakConsistency),
                new CommandParserInfo("AlterMergeClusterPolicyQueryWeakConsistency", AlterMergeClusterPolicyQueryWeakConsistency),
                new CommandParserInfo("ShowTablePolicyExtentTagsRetention", ShowTablePolicyExtentTagsRetention),
                new CommandParserInfo("ShowTableStarPolicyExtentTagsRetention", ShowTableStarPolicyExtentTagsRetention),
                new CommandParserInfo("ShowDatabasePolicyExtentTagsRetention", ShowDatabasePolicyExtentTagsRetention),
                new CommandParserInfo("AlterTablePolicyExtentTagsRetention", AlterTablePolicyExtentTagsRetention),
                new CommandParserInfo("AlterDatabasePolicyExtentTagsRetention", AlterDatabasePolicyExtentTagsRetention),
                new CommandParserInfo("DeleteTablePolicyExtentTagsRetention", DeleteTablePolicyExtentTagsRetention),
                new CommandParserInfo("DeleteDatabasePolicyExtentTagsRetention", DeleteDatabasePolicyExtentTagsRetention),
                new CommandParserInfo("ShowPrincipalRoles", ShowPrincipalRoles),
                new CommandParserInfo("ShowDatabasePrincipalRoles", ShowDatabasePrincipalRoles),
                new CommandParserInfo("ShowTablePrincipalRoles", ShowTablePrincipalRoles),
                new CommandParserInfo("ShowGraphModelPrincipalRoles", ShowGraphModelPrincipalRoles),
                new CommandParserInfo("ShowExternalTablesPrincipalRoles", ShowExternalTablesPrincipalRoles),
                new CommandParserInfo("ShowFunctionPrincipalRoles", ShowFunctionPrincipalRoles),
                new CommandParserInfo("ShowClusterPrincipalRoles", ShowClusterPrincipalRoles),
                new CommandParserInfo("ShowClusterPrincipals", ShowClusterPrincipals),
                new CommandParserInfo("ShowDatabasePrincipals", ShowDatabasePrincipals),
                new CommandParserInfo("ShowTablePrincipals", ShowTablePrincipals),
                new CommandParserInfo("ShowGraphModelPrincipals", ShowGraphModelPrincipals),
                new CommandParserInfo("ShowFunctionPrincipals", ShowFunctionPrincipals),
                new CommandParserInfo("AddClusterRole", AddClusterRole),
                new CommandParserInfo("DropClusterRole", DropClusterRole),
                new CommandParserInfo("SetClusterRole", SetClusterRole),
                new CommandParserInfo("AddDatabaseRole", AddDatabaseRole),
                new CommandParserInfo("DropDatabaseRole", DropDatabaseRole),
                new CommandParserInfo("SetDatabaseRole", SetDatabaseRole),
                new CommandParserInfo("AddTableRole", AddTableRole),
                new CommandParserInfo("DropTableRole", DropTableRole),
                new CommandParserInfo("SetTableRole", SetTableRole),
                new CommandParserInfo("AddFunctionRole", AddFunctionRole),
                new CommandParserInfo("DropFunctionRole", DropFunctionRole),
                new CommandParserInfo("SetFunctionRole", SetFunctionRole),
                new CommandParserInfo("ShowClusterBlockedPrincipals", ShowClusterBlockedPrincipals),
                new CommandParserInfo("AddClusterBlockedPrincipals", AddClusterBlockedPrincipals),
                new CommandParserInfo("DropClusterBlockedPrincipals", DropClusterBlockedPrincipals),
                new CommandParserInfo("SetClusterMaintenanceMode", SetClusterMaintenanceMode),
                new CommandParserInfo("IngestIntoTable", IngestIntoTable),
                new CommandParserInfo("IngestInlineIntoTable", IngestInlineIntoTable),
                new CommandParserInfo("SetTable", SetTable),
                new CommandParserInfo("AppendTable", AppendTable),
                new CommandParserInfo("SetOrAppendTable", SetOrAppendTable),
                new CommandParserInfo("SetOrReplaceTable", SetOrReplaceTable),
                new CommandParserInfo("ExportToStorage", ExportToStorage),
                new CommandParserInfo("ExportToSqlTable", ExportToSqlTable),
                new CommandParserInfo("ExportToExternalTable", ExportToExternalTable),
                new CommandParserInfo("CreateOrAlterContinuousExport", CreateOrAlterContinuousExport),
                new CommandParserInfo("ShowContinuousExport", ShowContinuousExport),
                new CommandParserInfo("ShowContinuousExports", ShowContinuousExports),
                new CommandParserInfo("ShowClusterPendingContinuousExports", ShowClusterPendingContinuousExports),
                new CommandParserInfo("ShowContinuousExportExportedArtifacts", ShowContinuousExportExportedArtifacts),
                new CommandParserInfo("ShowContinuousExportFailures", ShowContinuousExportFailures),
                new CommandParserInfo("SetContinuousExportCursor", SetContinuousExportCursor),
                new CommandParserInfo("DropContinuousExport", DropContinuousExport),
                new CommandParserInfo("EnableContinuousExport", EnableContinuousExport),
                new CommandParserInfo("DisableContinuousExport", DisableContinuousExport),
                new CommandParserInfo("CreateMaterializedView", CreateMaterializedView),
                new CommandParserInfo("CreateMaterializedViewOverMaterializedView", CreateMaterializedViewOverMaterializedView),
                new CommandParserInfo("RenameMaterializedView", RenameMaterializedView),
                new CommandParserInfo("ShowMaterializedView", ShowMaterializedView),
                new CommandParserInfo("ShowMaterializedViews", ShowMaterializedViews),
                new CommandParserInfo("ShowMaterializedViewsDetails", ShowMaterializedViewsDetails),
                new CommandParserInfo("ShowMaterializedViewDetails", ShowMaterializedViewDetails),
                new CommandParserInfo("ShowMaterializedViewPolicyRetention", ShowMaterializedViewPolicyRetention),
                new CommandParserInfo("ShowMaterializedViewPolicyMerge", ShowMaterializedViewPolicyMerge),
                new CommandParserInfo("ShowMaterializedViewPolicyPartitioning", ShowMaterializedViewPolicyPartitioning),
                new CommandParserInfo("ShowMaterializedViewExtents", ShowMaterializedViewExtents),
                new CommandParserInfo("AlterMaterializedView", AlterMaterializedView),
                new CommandParserInfo("AlterMaterializedViewOverMaterializedView", AlterMaterializedViewOverMaterializedView),
                new CommandParserInfo("CreateOrAlterMaterializedView", CreateOrAlterMaterializedView),
                new CommandParserInfo("CreateOrAlterMaterializedViewOverMaterializedView", CreateOrAlterMaterializedViewOverMaterializedView),
                new CommandParserInfo("DropMaterializedView", DropMaterializedView),
                new CommandParserInfo("EnableDisableMaterializedView", EnableDisableMaterializedView),
                new CommandParserInfo("ShowMaterializedViewPrincipals", ShowMaterializedViewPrincipals),
                new CommandParserInfo("ShowMaterializedViewSchemaAsJson", ShowMaterializedViewSchemaAsJson),
                new CommandParserInfo("ShowMaterializedViewCslSchema", ShowMaterializedViewCslSchema),
                new CommandParserInfo("AlterMaterializedViewFolder", AlterMaterializedViewFolder),
                new CommandParserInfo("AlterMaterializedViewDocString", AlterMaterializedViewDocString),
                new CommandParserInfo("AlterMaterializedViewLookback", AlterMaterializedViewLookback),
                new CommandParserInfo("AlterMaterializedViewAutoUpdateSchema", AlterMaterializedViewAutoUpdateSchema),
                new CommandParserInfo("ClearMaterializedViewData", ClearMaterializedViewData),
                new CommandParserInfo("SetMaterializedViewCursor", SetMaterializedViewCursor),
                new CommandParserInfo("ShowTableOperationsMirroringStatus", ShowTableOperationsMirroringStatus),
                new CommandParserInfo("ShowTableOperationsMirroringExportedArtifacts", ShowTableOperationsMirroringExportedArtifacts),
                new CommandParserInfo("ShowTableOperationsMirroringFailures", ShowTableOperationsMirroringFailures),
                new CommandParserInfo("ShowCluster", ShowCluster),
                new CommandParserInfo("ShowClusterDetails", ShowClusterDetails),
                new CommandParserInfo("ShowDiagnostics", ShowDiagnostics),
                new CommandParserInfo("ShowCapacity", ShowCapacity),
                new CommandParserInfo("ShowOperations", ShowOperations),
                new CommandParserInfo("ShowOperationDetails", ShowOperationDetails),
                new CommandParserInfo("ShowJournal", ShowJournal),
                new CommandParserInfo("ShowDatabaseJournal", ShowDatabaseJournal),
                new CommandParserInfo("ShowClusterJournal", ShowClusterJournal),
                new CommandParserInfo("ShowQueries", ShowQueries),
                new CommandParserInfo("ShowRunningQueries", ShowRunningQueries),
                new CommandParserInfo("CancelQuery", CancelQuery),
                new CommandParserInfo("ShowQueryPlan", ShowQueryPlan),
                new CommandParserInfo("ShowCache", ShowCache),
                new CommandParserInfo("AlterCache", AlterCache),
                new CommandParserInfo("ShowCommands", ShowCommands),
                new CommandParserInfo("ShowCommandsAndQueries", ShowCommandsAndQueries),
                new CommandParserInfo("ShowIngestionFailures", ShowIngestionFailures),
                new CommandParserInfo("ShowDataOperations", ShowDataOperations),
                new CommandParserInfo("ShowDatabaseKeyVaultSecrets", ShowDatabaseKeyVaultSecrets),
                new CommandParserInfo("ShowClusterExtents", ShowClusterExtents),
                new CommandParserInfo("ShowClusterExtentsMetadata", ShowClusterExtentsMetadata),
                new CommandParserInfo("ShowDatabaseExtents", ShowDatabaseExtents),
                new CommandParserInfo("ShowDatabaseExtentsMetadata", ShowDatabaseExtentsMetadata),
                new CommandParserInfo("ShowDatabaseExtentTagsStatistics", ShowDatabaseExtentTagsStatistics),
                new CommandParserInfo("ShowDatabaseExtentsPartitioningStatistics", ShowDatabaseExtentsPartitioningStatistics),
                new CommandParserInfo("ShowTableExtents", ShowTableExtents),
                new CommandParserInfo("ShowTableExtentsMetadata", ShowTableExtentsMetadata),
                new CommandParserInfo("TableShardsGroupShow", TableShardsGroupShow),
                new CommandParserInfo("TableShardsGroupMetadataShow", TableShardsGroupMetadataShow),
                new CommandParserInfo("TableShardGroupsShow", TableShardGroupsShow),
                new CommandParserInfo("TableShardGroupsStatisticsShow", TableShardGroupsStatisticsShow),
                new CommandParserInfo("TablesShardGroupsStatisticsShow", TablesShardGroupsStatisticsShow),
                new CommandParserInfo("DatabaseShardGroupsStatisticsShow", DatabaseShardGroupsStatisticsShow),
                new CommandParserInfo("MergeExtents", MergeExtents),
                new CommandParserInfo("MergeExtentsDryrun", MergeExtentsDryrun),
                new CommandParserInfo("MoveExtentsFrom", MoveExtentsFrom),
                new CommandParserInfo("MoveExtentsQuery", MoveExtentsQuery),
                new CommandParserInfo("TableShuffleExtents", TableShuffleExtents),
                new CommandParserInfo("TableShuffleExtentsQuery", TableShuffleExtentsQuery),
                new CommandParserInfo("ReplaceExtents", ReplaceExtents),
                new CommandParserInfo("DropExtent", DropExtent),
                new CommandParserInfo("DropExtents", DropExtents),
                new CommandParserInfo("DropExtentsPartitionMetadata", DropExtentsPartitionMetadata),
                new CommandParserInfo("DropPretendExtentsByProperties", DropPretendExtentsByProperties),
                new CommandParserInfo("ShowVersion", ShowVersion),
                new CommandParserInfo("ClearTableData", ClearTableData),
                new CommandParserInfo("ClearTableCacheStreamingIngestionSchema", ClearTableCacheStreamingIngestionSchema),
                new CommandParserInfo("ShowStorageArtifactsCleanupState", ShowStorageArtifactsCleanupState),
                new CommandParserInfo("ClusterDropStorageArtifactsCleanupState", ClusterDropStorageArtifactsCleanupState),
                new CommandParserInfo("StoredQueryResultSet", StoredQueryResultSet),
                new CommandParserInfo("StoredQueryResultSetOrReplace", StoredQueryResultSetOrReplace),
                new CommandParserInfo("StoredQueryResultsShow", StoredQueryResultsShow),
                new CommandParserInfo("StoredQueryResultShowSchema", StoredQueryResultShowSchema),
                new CommandParserInfo("StoredQueryResultDrop", StoredQueryResultDrop),
                new CommandParserInfo("StoredQueryResultsDrop", StoredQueryResultsDrop),
                new CommandParserInfo("GraphModelCreateOrAlter", GraphModelCreateOrAlter),
                new CommandParserInfo("GraphModelShow", GraphModelShow),
                new CommandParserInfo("GraphModelsShow", GraphModelsShow),
                new CommandParserInfo("GraphModelDrop", GraphModelDrop),
                new CommandParserInfo("SetGraphModelAdmins", SetGraphModelAdmins),
                new CommandParserInfo("AddGraphModelAdmins", AddGraphModelAdmins),
                new CommandParserInfo("DropGraphModelAdmins", DropGraphModelAdmins),
                new CommandParserInfo("GraphSnapshotMake", GraphSnapshotMake),
                new CommandParserInfo("GraphSnapshotShow", GraphSnapshotShow),
                new CommandParserInfo("GraphSnapshotsShow", GraphSnapshotsShow),
                new CommandParserInfo("GraphSnapshotDrop", GraphSnapshotDrop),
                new CommandParserInfo("GraphSnapshotsDrop", GraphSnapshotsDrop),
                new CommandParserInfo("GraphSnapshotShowStatistics", GraphSnapshotShowStatistics),
                new CommandParserInfo("GraphSnapshotsShowStatistics", GraphSnapshotsShowStatistics),
                new CommandParserInfo("GraphSnapshotShowFailures", GraphSnapshotShowFailures),
                new CommandParserInfo("ShowCertificates", ShowCertificates),
                new CommandParserInfo("ShowCloudSettings", ShowCloudSettings),
                new CommandParserInfo("ShowCommConcurrency", ShowCommConcurrency),
                new CommandParserInfo("ShowCommPools", ShowCommPools),
                new CommandParserInfo("ShowFabricCache", ShowFabricCache),
                new CommandParserInfo("ShowFabricLocks", ShowFabricLocks),
                new CommandParserInfo("ShowFabricClocks", ShowFabricClocks),
                new CommandParserInfo("ShowFeatureFlags", ShowFeatureFlags),
                new CommandParserInfo("ShowMemPools", ShowMemPools),
                new CommandParserInfo("ShowServicePoints", ShowServicePoints),
                new CommandParserInfo("ShowTcpConnections", ShowTcpConnections),
                new CommandParserInfo("ShowTcpPorts", ShowTcpPorts),
                new CommandParserInfo("ShowThreadPools", ShowThreadPools),
                new CommandParserInfo("ExecuteDatabaseScript", ExecuteDatabaseScript),
                new CommandParserInfo("ExecuteClusterScript", ExecuteClusterScript),
                new CommandParserInfo("CreateRequestSupport", CreateRequestSupport),
                new CommandParserInfo("ShowRequestSupport", ShowRequestSupport),
                new CommandParserInfo("ShowClusterAdminState", ShowClusterAdminState),
                new CommandParserInfo("ClearRemoteClusterDatabaseSchema", ClearRemoteClusterDatabaseSchema),
                new CommandParserInfo("ShowClusterMonitoring", ShowClusterMonitoring),
                new CommandParserInfo("ShowClusterScaleIn", ShowClusterScaleIn),
                new CommandParserInfo("ShowClusterServices", ShowClusterServices),
                new CommandParserInfo("ShowClusterNetwork", ShowClusterNetwork),
                new CommandParserInfo("AlterClusterStorageKeys", AlterClusterStorageKeys),
                new CommandParserInfo("ShowClusterStorageKeysHash", ShowClusterStorageKeysHash),
                new CommandParserInfo("AlterFabricServiceAssignmentsCommand", AlterFabricServiceAssignmentsCommand),
                new CommandParserInfo("DropFabricServiceAssignmentsCommand", DropFabricServiceAssignmentsCommand),
                new CommandParserInfo("CreateEntityGroupCommand", CreateEntityGroupCommand),
                new CommandParserInfo("CreateOrAlterEntityGroupCommand", CreateOrAlterEntityGroupCommand),
                new CommandParserInfo("AlterEntityGroup", AlterEntityGroup),
                new CommandParserInfo("AlterMergeEntityGroup", AlterMergeEntityGroup),
                new CommandParserInfo("DropEntityGroup", DropEntityGroup),
                new CommandParserInfo("ShowEntityGroup", ShowEntityGroup),
                new CommandParserInfo("ShowEntityGroups", ShowEntityGroups),
                new CommandParserInfo("AlterExtentContainersAdd", AlterExtentContainersAdd),
                new CommandParserInfo("AlterExtentContainersDrop", AlterExtentContainersDrop),
                new CommandParserInfo("AlterExtentContainersRecycle", AlterExtentContainersRecycle),
                new CommandParserInfo("AlterExtentContainersSet", AlterExtentContainersSet),
                new CommandParserInfo("ShowExtentContainers", ShowExtentContainers),
                new CommandParserInfo("DropEmptyExtentContainers", DropEmptyExtentContainers),
                new CommandParserInfo("CleanDatabaseExtentContainers", CleanDatabaseExtentContainers),
                new CommandParserInfo("ShowDatabaseExtentContainersCleanOperations", ShowDatabaseExtentContainersCleanOperations),
                new CommandParserInfo("ClearDatabaseCacheQueryResults", ClearDatabaseCacheQueryResults),
                new CommandParserInfo("ShowDatabaseCacheQueryResults", ShowDatabaseCacheQueryResults),
                new CommandParserInfo("ShowDatabasesManagementGroups", ShowDatabasesManagementGroups),
                new CommandParserInfo("AlterDatabaseStorageKeys", AlterDatabaseStorageKeys),
                new CommandParserInfo("ClearDatabaseCacheStreamingIngestionSchema", ClearDatabaseCacheStreamingIngestionSchema),
                new CommandParserInfo("ClearDatabaseCacheQueryWeakConsistency", ClearDatabaseCacheQueryWeakConsistency),
                new CommandParserInfo("ShowEntitySchema", ShowEntitySchema),
                new CommandParserInfo("ShowExtentDetails", ShowExtentDetails),
                new CommandParserInfo("ShowExtentColumnStorageStats", ShowExtentColumnStorageStats),
                new CommandParserInfo("AttachExtentsIntoTableByContainer", AttachExtentsIntoTableByContainer),
                new CommandParserInfo("AttachExtentsIntoTableByMetadata", AttachExtentsIntoTableByMetadata),
                new CommandParserInfo("AlterExtentTagsFromQuery", AlterExtentTagsFromQuery),
                new CommandParserInfo("AlterMergeExtentTagsFromQuery", AlterMergeExtentTagsFromQuery),
                new CommandParserInfo("DropExtentTagsFromQuery", DropExtentTagsFromQuery),
                new CommandParserInfo("DropExtentTagsFromTable", DropExtentTagsFromTable),
                new CommandParserInfo("DropExtentTagsRetention", DropExtentTagsRetention),
                new CommandParserInfo("MergeDatabaseShardGroups", MergeDatabaseShardGroups),
                new CommandParserInfo("AlterFollowerClusterConfiguration", AlterFollowerClusterConfiguration),
                new CommandParserInfo("AddFollowerDatabaseAuthorizedPrincipals", AddFollowerDatabaseAuthorizedPrincipals),
                new CommandParserInfo("DropFollowerDatabaseAuthorizedPrincipals", DropFollowerDatabaseAuthorizedPrincipals),
                new CommandParserInfo("AlterFollowerDatabaseAuthorizedPrincipals", AlterFollowerDatabaseAuthorizedPrincipals),
                new CommandParserInfo("DropFollowerDatabasePolicyCaching", DropFollowerDatabasePolicyCaching),
                new CommandParserInfo("AlterFollowerDatabaseChildEntities", AlterFollowerDatabaseChildEntities),
                new CommandParserInfo("AlterFollowerDatabaseConfiguration", AlterFollowerDatabaseConfiguration),
                new CommandParserInfo("DropFollowerDatabases", DropFollowerDatabases),
                new CommandParserInfo("ShowFollowerDatabase", ShowFollowerDatabase),
                new CommandParserInfo("AlterFollowerTablesPolicyCaching", AlterFollowerTablesPolicyCaching),
                new CommandParserInfo("DropFollowerTablesPolicyCaching", DropFollowerTablesPolicyCaching),
                new CommandParserInfo("ShowFreshness", ShowFreshness),
                new CommandParserInfo("ShowFunctionSchemaAsJson", ShowFunctionSchemaAsJson),
                new CommandParserInfo("SetMaterializedViewAdmins", SetMaterializedViewAdmins),
                new CommandParserInfo("AddMaterializedViewAdmins", AddMaterializedViewAdmins),
                new CommandParserInfo("DropMaterializedViewAdmins", DropMaterializedViewAdmins),
                new CommandParserInfo("SetMaterializedViewConcurrency", SetMaterializedViewConcurrency),
                new CommandParserInfo("ClearMaterializedViewStatistics", ClearMaterializedViewStatistics),
                new CommandParserInfo("ShowMaterializedViewStatistics", ShowMaterializedViewStatistics),
                new CommandParserInfo("ShowMaterializedViewDiagnostics", ShowMaterializedViewDiagnostics),
                new CommandParserInfo("ShowMaterializedViewFailures", ShowMaterializedViewFailures),
                new CommandParserInfo("ShowMemory", ShowMemory),
                new CommandParserInfo("CancelOperation", CancelOperation),
                new CommandParserInfo("DisablePlugin", DisablePlugin),
                new CommandParserInfo("EnablePlugin", EnablePlugin),
                new CommandParserInfo("ShowPlugins", ShowPlugins),
                new CommandParserInfo("ShowPrincipalAccess", ShowPrincipalAccess),
                new CommandParserInfo("ShowDatabasePurgeOperation", ShowDatabasePurgeOperation),
                new CommandParserInfo("ShowQueryExecution", ShowQueryExecution),
                new CommandParserInfo("AlterPoliciesOfRetention", AlterPoliciesOfRetention),
                new CommandParserInfo("DeletePoliciesOfRetention", DeletePoliciesOfRetention),
                new CommandParserInfo("CreateRowStore", CreateRowStore),
                new CommandParserInfo("DropRowStore", DropRowStore),
                new CommandParserInfo("ShowRowStore", ShowRowStore),
                new CommandParserInfo("ShowRowStores", ShowRowStores),
                new CommandParserInfo("ShowRowStoreTransactions", ShowRowStoreTransactions),
                new CommandParserInfo("ShowRowStoreSeals", ShowRowStoreSeals),
                new CommandParserInfo("ShowSchema", ShowSchema),
                new CommandParserInfo("ShowCallStacks", ShowCallStacks),
                new CommandParserInfo("ShowFileSystem", ShowFileSystem),
                new CommandParserInfo("ShowRunningCallouts", ShowRunningCallouts),
                new CommandParserInfo("ShowStreamingIngestionFailures", ShowStreamingIngestionFailures),
                new CommandParserInfo("ShowStreamingIngestionStatistics", ShowStreamingIngestionStatistics),
                new CommandParserInfo("AlterTableRowStoreReferencesDropKey", AlterTableRowStoreReferencesDropKey),
                new CommandParserInfo("AlterTableRowStoreReferencesDropRowStore", AlterTableRowStoreReferencesDropRowStore),
                new CommandParserInfo("AlterTableRowStoreReferencesDropBlockedKeys", AlterTableRowStoreReferencesDropBlockedKeys),
                new CommandParserInfo("AlterTableRowStoreReferencesDisableKey", AlterTableRowStoreReferencesDisableKey),
                new CommandParserInfo("AlterTableRowStoreReferencesDisableRowStore", AlterTableRowStoreReferencesDisableRowStore),
                new CommandParserInfo("AlterTableRowStoreReferencesDisableBlockedKeys", AlterTableRowStoreReferencesDisableBlockedKeys),
                new CommandParserInfo("SetTableRowStoreReferences", SetTableRowStoreReferences),
                new CommandParserInfo("ShowTableRowStoreReferences", ShowTableRowStoreReferences),
                new CommandParserInfo("AlterTableColumnStatistics", AlterTableColumnStatistics),
                new CommandParserInfo("AlterTableColumnStatisticsMethod", AlterTableColumnStatisticsMethod),
                new CommandParserInfo("ShowTableColumnStatitics", ShowTableColumnStatitics),
                new CommandParserInfo("ShowTableDimensions", ShowTableDimensions),
                new CommandParserInfo("DeleteTableRecords", DeleteTableRecords),
                new CommandParserInfo("TableDataUpdate", TableDataUpdate),
                new CommandParserInfo("DeleteMaterializedViewRecords", DeleteMaterializedViewRecords),
                new CommandParserInfo("ShowTableColumnsClassification", ShowTableColumnsClassification),
                new CommandParserInfo("ShowTableRowStores", ShowTableRowStores),
                new CommandParserInfo("ShowTableRowStoreSealInfo", ShowTableRowStoreSealInfo),
                new CommandParserInfo("ShowTablesColumnStatistics", ShowTablesColumnStatistics),
                new CommandParserInfo("ShowTableDataStatistics", ShowTableDataStatistics),
                new CommandParserInfo("CreateTempStorage", CreateTempStorage),
                new CommandParserInfo("DropTempStorage", DropTempStorage),
                new CommandParserInfo("DropStoredQueryResultContainers", DropStoredQueryResultContainers),
                new CommandParserInfo("DropUnusedStoredQueryResultContainers", DropUnusedStoredQueryResultContainers),
                new CommandParserInfo("EnableDatabaseMaintenanceMode", EnableDatabaseMaintenanceMode),
                new CommandParserInfo("DisableDatabaseMaintenanceMode", DisableDatabaseMaintenanceMode),
                new CommandParserInfo("EnableDatabaseStreamingIngestionMaintenanceMode", EnableDatabaseStreamingIngestionMaintenanceMode),
                new CommandParserInfo("DisableDatabaseStreamingIngestionMaintenanceMode", DisableDatabaseStreamingIngestionMaintenanceMode),
                new CommandParserInfo("ShowQueryCallTree", ShowQueryCallTree),
                new CommandParserInfo("ShowExtentCorruptedDatetime", ShowExtentCorruptedDatetime),
                new CommandParserInfo("PatchExtentCorruptedDatetime", PatchExtentCorruptedDatetime),
                new CommandParserInfo("ClearClusterCredStoreCache", ClearClusterCredStoreCache),
                new CommandParserInfo("ClearClusterGroupMembershipCache", ClearClusterGroupMembershipCache),
                new CommandParserInfo("ClearExternalArtifactsCache", ClearExternalArtifactsCache),
                new CommandParserInfo("ShowDatabasesEntities", ShowDatabasesEntities),
                new CommandParserInfo("ShowDatabaseEntity", ShowDatabaseEntity),
                new CommandParserInfo("ReplaceDatabaseKeyVaultSecrets", ReplaceDatabaseKeyVaultSecrets)
            };

            return commandParsers;
        }
    }
}

