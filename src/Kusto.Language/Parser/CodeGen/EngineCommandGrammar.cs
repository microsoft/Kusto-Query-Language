﻿// 
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
            var shape0 = CD("Role");
            var shape1 = new [] {CD(), CD(), CD("Role"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true), CD(CompletionHint.Literal, isOptional: true)};
            var shape2 = CD("Principal", CompletionHint.Literal);
            var shape3 = CD("SkipResults");
            var shape4 = CD("Notes", CompletionHint.Literal);
            var shape5 = CD("principal", CompletionHint.Literal);
            var shape6 = CD("notes", CompletionHint.Literal);
            var shape7 = CD("Policy", CompletionHint.Literal);
            var shape8 = new [] {CD(), CD(), CD(), CD(), CD("Policy", CompletionHint.Literal)};
            var shape9 = CD("policy", CompletionHint.Literal);
            var shape10 = new [] {CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal)};
            var shape11 = CD("PolicyName", CompletionHint.Literal);
            var shape12 = new [] {CD(), CD(), CD(), CD(), CD("PolicyName", CompletionHint.Literal)};
            var shape13 = CD("EncodingPolicy", CompletionHint.Literal);
            var shape14 = CD("IngestionBatchingPolicy", CompletionHint.Literal);
            var shape15 = new [] {CD(), CD(), CD(), CD(), CD("IngestionBatchingPolicy", CompletionHint.Literal)};
            var shape16 = CD("ManagedIdentityPolicy", CompletionHint.Literal);
            var shape17 = new [] {CD(), CD(), CD(), CD(), CD("ManagedIdentityPolicy", CompletionHint.Literal)};
            var shape18 = CD("RecoverabilityValue");
            var shape19 = new [] {CD(), CD(), CD("RecoverabilityValue")};
            var shape20 = CD("DatabaseName", CompletionHint.Database);
            var shape21 = CD("MergePolicy", CompletionHint.Literal);
            var shape22 = CD("PropertyName", CompletionHint.None);
            var shape23 = CD("Value", CompletionHint.Literal);
            var shape24 = new [] {CD("PropertyName", CompletionHint.None), CD(), CD("Value", CompletionHint.Literal)};
            var shape25 = new [] {CD(), CD(), CD(CompletionHint.None), CD()};
            var shape26 = CD("SoftDeleteValue", CompletionHint.Literal);
            var shape27 = new [] {CD(), CD(), CD("SoftDeleteValue", CompletionHint.Literal), CD(isOptional: true)};
            var shape28 = CD("ShardingPolicy", CompletionHint.Literal);
            var shape29 = CD("ShardsGroupingPolicy", CompletionHint.Literal);
            var shape30 = CD("StreamingIngestionPolicy", CompletionHint.Literal);
            var shape31 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("Policy", CompletionHint.Literal)};
            var shape32 = CD("databaseName", CompletionHint.Literal);
            var shape33 = CD("MaterializedViewName", CompletionHint.MaterializedView);
            var shape34 = CD("RetentionPolicy", CompletionHint.Literal);
            var shape35 = CD("TableName", CompletionHint.Table);
            var shape36 = CD("PartitionFunction");
            var shape37 = CD("DateTimeColumn", CompletionHint.None);
            var shape38 = new [] {CD("PartitionFunction"), CD(), CD("DateTimeColumn", CompletionHint.None), CD()};
            var shape39 = CD("PartitionType");
            var shape40 = CD("PartitionName", CompletionHint.None);
            var shape41 = CD("PathSeparator", CompletionHint.Literal);
            var shape42 = CD("KindType");
            var shape43 = CD("StorageConnectionString", CompletionHint.Literal);
            var shape44 = new [] {CD(), CD(CompletionHint.Literal), CD()};
            var shape45 = CD("ColumnName", CompletionHint.Column);
            var shape46 = CD("PropertyValue", CompletionHint.Literal);
            var shape47 = CD("tableName", CompletionHint.Table);
            var shape48 = new [] {CD(), CD(CompletionHint.Tabular)};
            var shape49 = CD("csl");
            var shape50 = CD("MappingKind");
            var shape51 = CD("MappingName", CompletionHint.Literal);
            var shape52 = CD("MappingFormat", CompletionHint.Literal);
            var shape53 = new [] {CD("PropertyName", CompletionHint.None), CD(), CD("PropertyValue", CompletionHint.Literal)};
            var shape54 = CD("thumbprint", CompletionHint.Literal);
            var shape55 = CD("t", CompletionHint.Literal);
            var shape56 = new [] {CD(), CD(), CD(), CD("tableName", CompletionHint.Table), CD(), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true), CD("csl")};
            var shape57 = CD("RowStorePolicy", CompletionHint.Literal);
            var shape58 = new [] {CD(), CD(), CD(), CD(), CD("RowStorePolicy", CompletionHint.Literal)};
            var shape59 = new [] {CD(), CD(), CD(), CD(), CD("StreamingIngestionPolicy", CompletionHint.Literal)};
            var shape60 = CD("HotData", CompletionHint.Literal);
            var shape61 = CD("HotIndex", CompletionHint.Literal);
            var shape62 = new [] {CD(), CD(), CD("HotData", CompletionHint.Literal), CD(), CD(), CD("HotIndex", CompletionHint.Literal)};
            var shape63 = CD("Timespan", CompletionHint.Literal);
            var shape64 = new [] {CD(), CD(), CD("Timespan", CompletionHint.Literal)};
            var shape65 = new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD(), CD("EncodingPolicy", CompletionHint.Literal)};
            var shape66 = CD("ColumnType");
            var shape67 = new [] {CD(), CD(), CD(), CD(), CD("EncodingPolicy", CompletionHint.Literal)};
            var shape68 = new [] {CD(), CD(), CD(), CD(), CD("MergePolicy", CompletionHint.Literal)};
            var shape69 = new [] {CD(), CD(), CD(), CD(), CD("ShardingPolicy", CompletionHint.Literal)};
            var shape70 = new [] {CD(), CD(), CD(), CD(), CD("ShardsGroupingPolicy", CompletionHint.Literal)};
            var shape71 = CD("Status");
            var shape72 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD("MappingKind"), CD(), CD("MappingName", CompletionHint.Literal), CD("MappingFormat", CompletionHint.Literal)};
            var shape73 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD()};
            var shape74 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("PolicyName", CompletionHint.Literal)};
            var shape75 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("EncodingPolicy", CompletionHint.Literal)};
            var shape76 = CD("ExtentTagsRetentionPolicy", CompletionHint.Literal);
            var shape77 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("IngestionBatchingPolicy", CompletionHint.Literal)};
            var shape78 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("ManagedIdentityPolicy", CompletionHint.Literal)};
            var shape79 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("MergePolicy", CompletionHint.Literal)};
            var shape80 = CD("DataFormatKind");
            var shape81 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD(), CD(), CD("DataFormatKind"), CD(isOptional: true), CD(isOptional: true)};
            var shape82 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("ShardingPolicy", CompletionHint.Literal)};
            var shape83 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("ShardsGroupingPolicy", CompletionHint.Literal)};
            var shape84 = CD("databaseName", CompletionHint.Database);
            var shape85 = CD("EntityGroupName", CompletionHint.EntityGroup);
            var shape86 = CD("clusterName", CompletionHint.Literal);
            var shape87 = new [] {CD(), CD(), CD("clusterName", CompletionHint.Literal), CD(), CD(), CD(), CD(), CD("databaseName", CompletionHint.Literal), CD()};
            var shape88 = new [] {CD(), CD(), CD("databaseName", CompletionHint.Literal), CD()};
            var shape89 = CD("container", CompletionHint.Literal);
            var shape90 = CD("ColumnName", CompletionHint.None);
            var shape91 = new [] {CD("ColumnName", CompletionHint.None), CD(), CD("ColumnType")};
            var shape92 = CD("BinValue", CompletionHint.Literal);
            var shape93 = new [] {CD("PartitionFunction"), CD(), CD("DateTimeColumn", CompletionHint.None), CD(), CD("BinValue", CompletionHint.Literal), CD()};
            var shape94 = CD("StringColumn", CompletionHint.None);
            var shape95 = new [] {CD("PartitionType"), CD(isOptional: true)};
            var shape96 = new [] {CD("PartitionName", CompletionHint.None), CD(), CD()};
            var shape97 = CD("DateTimeFormat", CompletionHint.Literal);
            var shape98 = new [] {CD(), CD(), CD("DateTimeFormat", CompletionHint.Literal), CD(), CD("PartitionName", CompletionHint.None), CD()};
            var shape99 = new [] {CD(), CD(CompletionHint.Literal, isOptional: true)};
            var shape100 = new [] {CD(), CD(), CD(), CD(CompletionHint.Literal, isOptional: true), CD(), CD()};
            var shape101 = new [] {CD(), CD(), CD(), CD(CompletionHint.None), CD(), CD(isOptional: true)};
            var shape102 = CD("TableKind");
            var shape103 = CD("HashMod", CompletionHint.Literal);
            var shape104 = new [] {CD("PartitionType"), CD(), CD("PartitionFunction"), CD(), CD("StringColumn", CompletionHint.None), CD(), CD("HashMod", CompletionHint.Literal), CD()};
            var shape105 = new [] {CD(), CD("StringColumn", CompletionHint.None)};
            var shape106 = CD("CatalogExpression", CompletionHint.Literal);
            var shape107 = new [] {CD(), CD(), CD("CatalogExpression", CompletionHint.Literal)};
            var shape108 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD(), CD(), CD(), CD("TableKind"), CD(isOptional: true), CD(isOptional: true), CD(), CD(), CD("DataFormatKind"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)};
            var shape109 = CD("ExternalTableName", CompletionHint.None);
            var shape110 = CD("tableName", CompletionHint.ExternalTable);
            var shape111 = CD("ExternalTableName", CompletionHint.ExternalTable);
            var shape112 = CD("leaderClusterMetadataPath", CompletionHint.Literal);
            var shape113 = CD("modificationKind");
            var shape114 = new [] {CD(), CD(), CD("modificationKind")};
            var shape115 = CD("dbName", CompletionHint.Database);
            var shape116 = new [] {CD(), CD("leaderClusterMetadataPath", CompletionHint.Literal)};
            var shape117 = CD("entityListKind");
            var shape118 = CD("operationName");
            var shape119 = new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(isOptional: true), CD(), CD("entityListKind"), CD("operationName"), CD(), CD(CompletionHint.None), CD()};
            var shape120 = CD("hotDataToken", CompletionHint.Literal);
            var shape121 = CD("hotIndexToken", CompletionHint.Literal);
            var shape122 = new [] {CD(), CD(), CD("hotDataToken", CompletionHint.Literal), CD(), CD(), CD("hotIndexToken", CompletionHint.Literal)};
            var shape123 = CD("hotToken", CompletionHint.Literal);
            var shape124 = new [] {CD(), CD(), CD("hotToken", CompletionHint.Literal)};
            var shape125 = CD("d2", CompletionHint.Literal);
            var shape126 = new [] {CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal)};
            var shape127 = CD("p", CompletionHint.Literal);
            var shape128 = new [] {CD(), CD(), CD("p", CompletionHint.Literal)};
            var shape129 = new [] {CD(isOptional: true), CD()};
            var shape130 = CD("hotWindows", isOptional: true);
            var shape131 = CD("d1", CompletionHint.Literal);
            var shape132 = new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(isOptional: true), CD(), CD(), CD("modificationKind")};
            var shape133 = CD("name", CompletionHint.None);
            var shape134 = new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(isOptional: true), CD(), CD(), CD(CompletionHint.None), CD(), CD(), CD(), CD(), CD("hotWindows", isOptional: true)};
            var shape135 = CD("FunctionName", CompletionHint.Function);
            var shape136 = CD("PropertyName");
            var shape137 = new [] {CD("PropertyName"), CD(), CD("PropertyValue", CompletionHint.Literal)};
            var shape138 = CD("Documentation", CompletionHint.Literal);
            var shape139 = CD("Folder", CompletionHint.Literal);
            var shape140 = new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD()};
            var shape141 = new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD("Policy", CompletionHint.Literal)};
            var shape142 = CD("TemplateName", CompletionHint.None);
            var shape143 = new [] {CD(), CD(), CD("KindType")};
            var shape144 = CD("ConnectionString", CompletionHint.Literal);
            var shape145 = new [] {CD(), CD("ConnectionString", CompletionHint.Literal), CD()};
            var shape146 = new [] {CD(), CD(), CD("TemplateName", CompletionHint.None), CD(isOptional: true), CD(isOptional: true), CD(isOptional: true)};
            var shape147 = new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(), CD()};
            var shape148 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD()};
            var shape149 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("EncodingPolicy", CompletionHint.Literal)};
            var shape150 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("IngestionBatchingPolicy", CompletionHint.Literal)};
            var shape151 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("MergePolicy", CompletionHint.Literal)};
            var shape152 = new [] {CD("PartitionType"), CD(), CD()};
            var shape153 = new [] {CD("PartitionType"), CD(), CD("StringColumn", CompletionHint.None)};
            var shape154 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(isOptional: true), CD(), CD(), CD("KindType"), CD(isOptional: true), CD(isOptional: true)};
            var shape155 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("Policy", CompletionHint.Literal)};
            var shape156 = CD("Query", CompletionHint.Literal);
            var shape157 = new [] {CD("ColumnName", CompletionHint.Column), CD()};
            var shape158 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD(CompletionHint.Column), CD()};
            var shape159 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("ShardingPolicy", CompletionHint.Literal)};
            var shape160 = CD("UpdatePolicy", CompletionHint.Literal);
            var shape161 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("UpdatePolicy", CompletionHint.Literal), CD(isOptional: true)};
            var shape162 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD(), CD(isOptional: true)};
            var shape163 = CD("rowStoreKey", CompletionHint.Literal);
            var shape164 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD("rowStoreKey", CompletionHint.Literal), CD(isOptional: true)};
            var shape165 = CD("rowStoreName", CompletionHint.None);
            var shape166 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD("rowStoreName", CompletionHint.None), CD(isOptional: true)};
            var shape167 = new [] {CD(), CD(CompletionHint.None), CD()};
            var shape168 = new [] {CD(), CD(), CD(CompletionHint.Table), CD(), CD(isOptional: true)};
            var shape169 = CD("DocString", CompletionHint.Literal);
            var shape170 = new [] {CD("ColumnName", CompletionHint.Column), CD(), CD("DocString", CompletionHint.Literal)};
            var shape171 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(CompletionHint.Column), CD()};
            var shape172 = new [] {CD(), CD(), CD("tableName", CompletionHint.Table), CD(), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true), CD("csl")};
            var shape173 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("MappingKind"), CD(), CD("MappingName", CompletionHint.Literal), CD("MappingFormat", CompletionHint.Literal), CD(isOptional: true)};
            var shape174 = CD("TableName", CompletionHint.Database);
            var shape175 = new [] {CD(), CD(), CD("TableName", CompletionHint.Database), CD(), CD(), CD("Policy", CompletionHint.Literal)};
            var shape176 = CD("QueryOrCommand", CompletionHint.Tabular);
            var shape177 = CD("Path", CompletionHint.Literal);
            var shape178 = new [] {CD(), CD(), CD("tableName", CompletionHint.Table)};
            var shape179 = CD("Version", CompletionHint.Literal);
            var shape180 = new [] {CD(), CD(), CD("Version", CompletionHint.Literal)};
            var shape181 = new [] {CD("ReadOnly"), CD(isOptional: true)};
            var shape182 = CD("TableName", CompletionHint.None);
            var shape183 = CD("Query", CompletionHint.Tabular);
            var shape184 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD(), CD(), CD(), CD("TableKind"), CD(), CD("StorageConnectionString", CompletionHint.Literal), CD(), CD(isOptional: true)};
            var shape185 = new [] {CD(), CD(), CD(CompletionHint.None)};
            var shape186 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.None), CD(), CD(CompletionHint.None), CD(), CD(), CD(), CD("TableKind"), CD(isOptional: true), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)};
            var shape187 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.None), CD(), CD(), CD("TableKind"), CD(), CD("StorageConnectionString", CompletionHint.Literal), CD(), CD(isOptional: true)};
            var shape188 = CD("WorkloadGroupName", CompletionHint.None);
            var shape189 = CD("WorkloadGroup", CompletionHint.Literal);
            var shape190 = new [] {CD(), CD(), CD("WorkloadGroupName", CompletionHint.None), CD("WorkloadGroup", CompletionHint.Literal)};
            var shape191 = CD("MaterializedViewName", CompletionHint.None);
            var shape192 = CD("DatabaseName", CompletionHint.None);
            var shape193 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.None), CD(), CD("MappingKind"), CD(), CD("MappingName", CompletionHint.Literal), CD("MappingFormat", CompletionHint.Literal)};
            var shape194 = CD("IfNotExists");
            var shape195 = CD("EntityGroupName", CompletionHint.None);
            var shape196 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD("MappingName", CompletionHint.Literal), CD("MappingFormat", CompletionHint.Literal)};
            var shape197 = CD("FunctionName", CompletionHint.None);
            var shape198 = new [] {CD(), CD(), CD(isOptional: true)};
            var shape199 = new [] {CD("TableName", CompletionHint.None), CD()};
            var shape200 = new [] {CD(), CD(), CD(CompletionHint.None), CD(isOptional: true)};
            var shape201 = new [] {CD(), CD(), CD("TableName", CompletionHint.None), CD(), CD(isOptional: true)};
            var shape202 = new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD()};
            var shape203 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD()};
            var shape204 = CD("name", CompletionHint.MaterializedView);
            var shape205 = new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(), CD(), CD(CompletionHint.None), CD(), CD(), CD()};
            var shape206 = CD("name", CompletionHint.Table);
            var shape207 = new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD()};
            var shape208 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD()};
            var shape209 = CD("AppName", CompletionHint.Literal);
            var shape210 = new [] {CD(), CD("AppName", CompletionHint.Literal)};
            var shape211 = CD("UserName", CompletionHint.Literal);
            var shape212 = new [] {CD(), CD("UserName", CompletionHint.Literal)};
            var shape213 = CD("ContinuousExportName", CompletionHint.None);
            var shape214 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD("Role"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true), CD(CompletionHint.Literal, isOptional: true)};
            var shape215 = CD("IfExists");
            var shape216 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD()};
            var shape217 = CD("SkipSeal");
            var shape218 = CD("d", CompletionHint.Literal);
            var shape219 = CD("TrimSize", CompletionHint.Literal);
            var shape220 = new [] {CD(), CD(), CD(), CD("TrimSize", CompletionHint.Literal), CD()};
            var shape221 = CD("LimitCount", CompletionHint.Literal);
            var shape222 = new [] {CD(), CD("LimitCount", CompletionHint.Literal)};
            var shape223 = CD("Older", CompletionHint.Literal);
            var shape224 = CD("ExtentId", CompletionHint.Literal);
            var shape225 = new [] {CD(), CD("TableName", CompletionHint.Table)};
            var shape226 = CD("externalTableName", CompletionHint.ExternalTable);
            var shape227 = new [] {CD(), CD(), CD(), CD("externalTableName", CompletionHint.ExternalTable), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true), CD(CompletionHint.Literal, isOptional: true)};
            var shape228 = CD("serviceType", CompletionHint.Literal);
            var shape229 = new [] {CD(), CD(), CD(), CD("serviceType", CompletionHint.Literal), CD()};
            var shape230 = CD("operationRole");
            var shape231 = new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD("Role"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true), CD(CompletionHint.Literal, isOptional: true)};
            var shape232 = new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD()};
            var shape233 = CD("materializedViewName", CompletionHint.MaterializedView);
            var shape234 = new [] {CD(), CD(), CD("materializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(CompletionHint.Literal, isOptional: true)};
            var shape235 = new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView)};
            var shape236 = new [] {CD(), CD(), CD("rowStoreName", CompletionHint.None), CD(isOptional: true)};
            var shape237 = CD("containerId", CompletionHint.Literal);
            var shape238 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD("Role"), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true), CD(CompletionHint.Literal, isOptional: true)};
            var shape239 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD()};
            var shape240 = new [] {CD(), CD(), CD("ContinuousExportName", CompletionHint.None)};
            var shape241 = new [] {CD(), CD(), CD(), CD(isOptional: true), CD(), CD(CompletionHint.Tabular)};
            var shape242 = new [] {CD(), CD(isOptional: true), CD(), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true), CD(), CD("Query", CompletionHint.Tabular)};
            var shape243 = CD("Data", CompletionHint.None);
            var shape244 = CD("GUID", CompletionHint.Literal);
            var shape245 = CD("SourceTableName", CompletionHint.Table);
            var shape246 = CD("DestinationTableName", CompletionHint.Table);
            var shape247 = CD("NewColumnName", CompletionHint.None);
            var shape248 = CD("NewTableName", CompletionHint.None);
            var shape249 = CD("ExtentsToDropQuery", CompletionHint.Tabular);
            var shape250 = CD("ExtentsToMoveQuery", CompletionHint.Tabular);
            var shape251 = CD("StoredQueryResultName", CompletionHint.None);
            var shape252 = new [] {CD(), CD(isOptional: true), CD("TableName", CompletionHint.None), CD(isOptional: true), CD(), CD("QueryOrCommand", CompletionHint.Tabular)};
            var shape253 = new [] {CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true), CD(CompletionHint.Literal, isOptional: true)};
            var shape254 = new [] {CD(), CD(isOptional: true)};
            var shape255 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD("Role"), CD()};
            var shape256 = CD("PrettyName", CompletionHint.Literal);
            var shape257 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD("PrettyName", CompletionHint.Literal)};
            var shape258 = new [] {CD(), CD(), CD(), CD("StoredQueryResultName", CompletionHint.None), CD(isOptional: true), CD(), CD("Query", CompletionHint.Tabular)};
            var shape259 = CD("viewName", CompletionHint.MaterializedView);
            var shape260 = new [] {CD(), CD(), CD(), CD(isOptional: true), CD()};
            var shape261 = CD("Tag", CompletionHint.Literal);
            var shape262 = new [] {CD(), CD(), CD("Tag", CompletionHint.Literal)};
            var shape263 = new [] {CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()};
            var shape264 = new [] {CD(), CD(), CD(), CD(), CD(isOptional: true)};
            var shape265 = new [] {CD(), CD(), CD(), CD(isOptional: true)};
            var shape266 = new [] {CD(), CD(), CD("ContinuousExportName", CompletionHint.None), CD()};
            var shape267 = new [] {CD(), CD(CompletionHint.None)};
            var shape268 = new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.None), CD(isOptional: true), CD(), CD()};
            var shape269 = new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD()};
            var shape270 = new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(CompletionHint.None), CD(), CD()};
            var shape271 = new [] {CD(), CD("Version", CompletionHint.Literal)};
            var shape272 = new [] {CD("DatabaseName", CompletionHint.Database), CD(isOptional: true)};
            var shape273 = CD("Details");
            var shape274 = new [] {CD(), CD(CompletionHint.Database)};
            var shape275 = CD("DatabaseName");
            var shape276 = new [] {CD(), CD(), CD("DatabaseName"), CD(), CD()};
            var shape277 = new [] {CD(), CD(), CD(), CD(isOptional: true), CD(isOptional: true)};
            var shape278 = new [] {CD(), CD(), CD(), CD(), CD(), CD(), CD(isOptional: true)};
            var shape279 = new [] {CD(), CD(), CD(), CD(), CD(isOptional: true), CD(isOptional: true)};
            var shape280 = new [] {CD(), CD(), CD(), CD(), CD(), CD(isOptional: true)};
            var shape281 = CD("name", CompletionHint.Literal);
            var shape282 = CD("databaseVersion", CompletionHint.Literal);
            var shape283 = new [] {CD(), CD("databaseVersion", CompletionHint.Literal)};
            var shape284 = CD("Script");
            var shape285 = new [] {CD(), CD(), CD(), CD(), CD(), CD(isOptional: true), CD(isOptional: true)};
            var shape286 = new [] {CD(), CD(), CD(), CD(), CD("Version", CompletionHint.Literal), CD(), CD(), CD(isOptional: true), CD(isOptional: true)};
            var shape287 = CD("flavor");
            var shape288 = new [] {CD(), CD(), CD("flavor"), CD(isOptional: true)};
            var shape289 = CD("obj", CompletionHint.Literal);
            var shape290 = new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(isOptional: true), CD(isOptional: true)};
            var shape291 = new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(), CD()};
            var shape292 = new [] {CD("Principal", CompletionHint.Literal), CD()};
            var shape293 = new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD()};
            var shape294 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD(), CD(isOptional: true), CD(isOptional: true)};
            var shape295 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD(), CD(isOptional: true)};
            var shape296 = new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("Version", CompletionHint.Literal), CD(), CD(), CD(isOptional: true), CD(isOptional: true)};
            var shape297 = CD("Scope");
            var shape298 = new [] {CD(), CD(), CD(), CD(), CD("Scope"), CD()};
            var shape299 = new [] {CD(), CD(), CD("EntityGroupName", CompletionHint.EntityGroup)};
            var shape300 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD()};
            var shape301 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD("MappingName", CompletionHint.Literal)};
            var shape302 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD()};
            var shape303 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD(), CD()};
            var shape304 = new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable)};
            var shape305 = new [] {CD(), CD(CompletionHint.Database), CD()};
            var shape306 = new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function)};
            var shape307 = new [] {CD(), CD(), CD(isOptional: true), CD(isOptional: true)};
            var shape308 = new [] {CD(), CD(), CD("MappingKind"), CD(), CD(isOptional: true)};
            var shape309 = new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD()};
            var shape310 = new [] {CD(), CD(), CD("viewName", CompletionHint.MaterializedView), CD(), CD(isOptional: true)};
            var shape311 = new [] {CD(), CD(), CD("viewName", CompletionHint.MaterializedView), CD()};
            var shape312 = new [] {CD(), CD(), CD("TemplateName", CompletionHint.None)};
            var shape313 = CD("OperationId", CompletionHint.Literal);
            var shape314 = CD("queryText");
            var shape315 = new [] {CD(), CD(), CD("tableName", CompletionHint.Table), CD()};
            var shape316 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(isOptional: true)};
            var shape317 = CD("ShardsGroupId", CompletionHint.Literal);
            var shape318 = new [] {CD(), CD(), CD("TableName", CompletionHint.Table)};
            var shape319 = CD("eid", CompletionHint.Literal);
            var shape320 = CD("tableName", CompletionHint.None);

            Func<Source<LexicalToken>, int, SyntaxElement> missing0 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral(source, start)));
            Func<Source<LexicalToken>, int, SyntaxElement> missing1 = (source, start) => (SyntaxElement)new CustomNode(shape48, CreateMissingToken("<|"), rules.MissingExpression(source, start));
            Func<Source<LexicalToken>, int, SyntaxElement> missing2 = (source, start) => (SyntaxElement)new CustomNode(shape19, CreateMissingToken("recoverability"), CreateMissingToken("="), CreateMissingToken("disabled", "enabled"));
            Func<Source<LexicalToken>, int, SyntaxElement> missing3 = (source, start) => (SyntaxElement)new CustomNode(shape44, CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral(source, start))), CreateMissingToken(")"));
            Func<Source<LexicalToken>, int, SyntaxElement> missing4 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape24, rules.MissingNameDeclaration(source, start), CreateMissingToken("="), rules.MissingValue(source, start))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing5 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape87, CreateMissingToken("cluster"), CreateMissingToken("("), rules.MissingStringLiteral(source, start), CreateMissingToken(")"), CreateMissingToken("."), CreateMissingToken("database"), CreateMissingToken("("), rules.MissingStringLiteral(source, start), CreateMissingToken(")"))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing6 = (source, start) => (SyntaxElement)new CustomNode(shape93, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(source, start), CreateMissingToken(","), rules.MissingValue(source, start), CreateMissingToken(")"));
            Func<Source<LexicalToken>, int, SyntaxElement> missing7 = (source, start) => (SyntaxElement)new CustomNode(shape152, CreateMissingToken("datetime"), CreateMissingToken("="), (SyntaxElement)new CustomNode(shape93, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(source, start), CreateMissingToken(","), rules.MissingValue(source, start), CreateMissingToken(")")));
            Func<Source<LexicalToken>, int, SyntaxElement> missing8 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape96, rules.MissingNameDeclaration(source, start), CreateMissingToken(":"), (SyntaxElement)new CustomNode(shape152, CreateMissingToken("datetime"), CreateMissingToken("="), (SyntaxElement)new CustomNode(shape93, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(source, start), CreateMissingToken(","), rules.MissingValue(source, start), CreateMissingToken(")"))))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing9 = (source, start) => new SyntaxList<SyntaxElement>((SyntaxElement)new CustomNode(shape99, (SyntaxElement)new CustomNode(shape98, CreateMissingToken("datetime_pattern"), CreateMissingToken("("), rules.MissingStringLiteral(source, start), CreateMissingToken(","), rules.MissingNameDeclaration(source, start), CreateMissingToken(")")), rules.MissingStringLiteral(source, start)));
            Func<Source<LexicalToken>, int, SyntaxElement> missing10 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape157, rules.MissingNameReference(source, start), CreateMissingToken("asc", "desc"))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing11 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape53, rules.MissingNameDeclaration(source, start), CreateMissingToken("="), rules.MissingValue(source, start))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing12 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape91, rules.MissingNameDeclaration(source, start), CreateMissingToken(":"), rules.MissingType(source, start))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing13 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape137, CreateMissingToken("docstring"), CreateMissingToken("="), rules.MissingValue(source, start))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing14 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape170, rules.MissingNameReference(source, start), CreateMissingToken(":"), rules.MissingStringLiteral(source, start))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing15 = (source, start) => CreateMissingToken("*");
            Func<Source<LexicalToken>, int, SyntaxElement> missing16 = (source, start) => (SyntaxElement)new CustomNode(shape62, CreateMissingToken("hotdata"), CreateMissingToken("="), rules.MissingValue(source, start), CreateMissingToken("hotindex"), CreateMissingToken("="), rules.MissingValue(source, start));
            Func<Source<LexicalToken>, int, SyntaxElement> missing17 = (source, start) => CreateMissingToken("disable");
            Func<Source<LexicalToken>, int, SyntaxElement> missing18 = (source, start) => (SyntaxElement)new CustomNode(new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape87, CreateMissingToken("cluster"), CreateMissingToken("("), rules.MissingStringLiteral(source, start), CreateMissingToken(")"), CreateMissingToken("."), CreateMissingToken("database"), CreateMissingToken("("), rules.MissingStringLiteral(source, start), CreateMissingToken(")")))), CreateMissingToken(")"));
            Func<Source<LexicalToken>, int, SyntaxElement> missing19 = (source, start) => (SyntaxElement)new CustomNode(new [] {CD(), CD("hours", CompletionHint.Literal), CD()}, CreateMissingToken("older"), rules.MissingValue(source, start), CreateMissingToken("hours"));
            Func<Source<LexicalToken>, int, SyntaxElement> missing20 = (source, start) => (SyntaxElement)new CustomNode(shape95, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape93, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(source, start), CreateMissingToken(","), rules.MissingValue(source, start), CreateMissingToken(")"))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing21 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape96, rules.MissingNameDeclaration(source, start), CreateMissingToken(":"), (SyntaxElement)new CustomNode(shape95, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape93, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(source, start), CreateMissingToken(","), rules.MissingValue(source, start), CreateMissingToken(")")))))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing22 = (source, start) => (SyntaxElement)new CustomNode(shape95, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape93, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(source, start), CreateMissingToken(","), rules.MissingValue(source, start), CreateMissingToken(")"))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing23 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape96, rules.MissingNameDeclaration(source, start), CreateMissingToken(":"), (SyntaxElement)new CustomNode(shape95, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape93, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(source, start), CreateMissingToken(","), rules.MissingValue(source, start), CreateMissingToken(")")))))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing24 = (source, start) => (SyntaxElement)new CustomNode(shape95, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape93, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(source, start), CreateMissingToken(","), rules.MissingValue(source, start), CreateMissingToken(")"))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing25 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape96, rules.MissingNameDeclaration(source, start), CreateMissingToken(":"), (SyntaxElement)new CustomNode(shape95, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape93, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(source, start), CreateMissingToken(","), rules.MissingValue(source, start), CreateMissingToken(")")))))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing26 = (source, start) => (SyntaxElement)new CustomNode(new [] {CD(), CD("serviceToNode", CompletionHint.Literal)}, CreateMissingToken("assignments"), rules.MissingStringLiteral(source, start));
            Func<Source<LexicalToken>, int, SyntaxElement> missing27 = (source, start) => (SyntaxElement)new CustomNode(new [] {CD(), CD(), CD("databaseNamePrefix", CompletionHint.None)}, CreateMissingToken("database-name-prefix"), CreateMissingToken("="), rules.MissingNameDeclaration(source, start));
            Func<Source<LexicalToken>, int, SyntaxElement> missing28 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameDeclaration(source, start)));
            Func<Source<LexicalToken>, int, SyntaxElement> missing29 = (source, start) => (SyntaxElement)new CustomNode(shape122, CreateMissingToken("hotdata"), CreateMissingToken("="), rules.MissingValue(source, start), CreateMissingToken("hotindex"), CreateMissingToken("="), rules.MissingValue(source, start));
            Func<Source<LexicalToken>, int, SyntaxElement> missing30 = (source, start) => (SyntaxElement)new CustomNode(shape126, rules.MissingValue(source, start), CreateMissingToken(".."), rules.MissingValue(source, start));
            Func<Source<LexicalToken>, int, SyntaxElement> missing31 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape137, CreateMissingToken("dimensionMaterializedViews"), CreateMissingToken("="), rules.MissingValue(source, start))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing32 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameReference(source, start)));
            Func<Source<LexicalToken>, int, SyntaxElement> missing33 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("ColumnName", CompletionHint.None), CD()}, rules.MissingNameDeclaration(source, start), CreateMissingToken("asc", "desc"))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing34 = (source, start) => (SyntaxElement)new CustomNode(shape167, CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape91, rules.MissingNameDeclaration(source, start), CreateMissingToken(":"), rules.MissingType(source, start)))), CreateMissingToken(")"));
            Func<Source<LexicalToken>, int, SyntaxElement> missing35 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape137, CreateMissingToken("creationTime"), CreateMissingToken("="), rules.MissingValue(source, start))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing36 = (source, start) => (SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Table), CD()}, CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingNameReference(source, start))), CreateMissingToken(")"));
            Func<Source<LexicalToken>, int, SyntaxElement> missing37 = (source, start) => new SyntaxList<SyntaxElement>(rules.MissingValue(source, start));
            Func<Source<LexicalToken>, int, SyntaxElement> missing38 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape199, rules.MissingNameDeclaration(source, start), (SyntaxElement)new CustomNode(shape167, CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape91, rules.MissingNameDeclaration(source, start), CreateMissingToken(":"), rules.MissingType(source, start)))), CreateMissingToken(")")))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing39 = (source, start) => (SyntaxElement)new CustomNode(shape95, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape93, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(source, start), CreateMissingToken(","), rules.MissingValue(source, start), CreateMissingToken(")"))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing40 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape96, rules.MissingNameDeclaration(source, start), CreateMissingToken(":"), (SyntaxElement)new CustomNode(shape95, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape93, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(source, start), CreateMissingToken(","), rules.MissingValue(source, start), CreateMissingToken(")")))))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing41 = (source, start) => (SyntaxElement)new CustomNode(shape95, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape93, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(source, start), CreateMissingToken(","), rules.MissingValue(source, start), CreateMissingToken(")"))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing42 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape96, rules.MissingNameDeclaration(source, start), CreateMissingToken(":"), (SyntaxElement)new CustomNode(shape95, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape93, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(source, start), CreateMissingToken(","), rules.MissingValue(source, start), CreateMissingToken(")")))))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing43 = (source, start) => (SyntaxElement)new CustomNode(shape95, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape93, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(source, start), CreateMissingToken(","), rules.MissingValue(source, start), CreateMissingToken(")"))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing44 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape96, rules.MissingNameDeclaration(source, start), CreateMissingToken(":"), (SyntaxElement)new CustomNode(shape95, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape93, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(source, start), CreateMissingToken(","), rules.MissingValue(source, start), CreateMissingToken(")")))))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing45 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape137, CreateMissingToken("autoUpdateSchema"), CreateMissingToken("="), rules.MissingValue(source, start))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing46 = (source, start) => (SyntaxElement)new CustomNode(shape95, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape93, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(source, start), CreateMissingToken(","), rules.MissingValue(source, start), CreateMissingToken(")"))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing47 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape96, rules.MissingNameDeclaration(source, start), CreateMissingToken(":"), (SyntaxElement)new CustomNode(shape95, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape93, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(source, start), CreateMissingToken(","), rules.MissingValue(source, start), CreateMissingToken(")")))))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing48 = (source, start) => (SyntaxElement)new CustomNode(shape95, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape93, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(source, start), CreateMissingToken(","), rules.MissingValue(source, start), CreateMissingToken(")"))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing49 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape96, rules.MissingNameDeclaration(source, start), CreateMissingToken(":"), (SyntaxElement)new CustomNode(shape95, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape93, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(source, start), CreateMissingToken(","), rules.MissingValue(source, start), CreateMissingToken(")")))))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing50 = (source, start) => (SyntaxElement)new CustomNode(shape95, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape93, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(source, start), CreateMissingToken(","), rules.MissingValue(source, start), CreateMissingToken(")"))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing51 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape96, rules.MissingNameDeclaration(source, start), CreateMissingToken(":"), (SyntaxElement)new CustomNode(shape95, CreateMissingToken("datetime"), (SyntaxElement)new CustomNode(CreateMissingToken("="), (SyntaxElement)new CustomNode(shape93, CreateMissingToken("bin"), CreateMissingToken("("), rules.MissingNameDeclaration(source, start), CreateMissingToken(","), rules.MissingValue(source, start), CreateMissingToken(")")))))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing52 = (source, start) => (SyntaxElement)new CustomNode(CreateMissingToken("all"), CreateMissingToken("tables"));
            Func<Source<LexicalToken>, int, SyntaxElement> missing53 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingValue(source, start)));
            Func<Source<LexicalToken>, int, SyntaxElement> missing54 = (source, start) => (SyntaxElement)new CustomNode(CreateMissingToken("all"), CreateMissingToken("tables"));
            Func<Source<LexicalToken>, int, SyntaxElement> missing55 = (source, start) => (SyntaxElement)new CustomNode(CreateMissingToken("all"), CreateMissingToken("tables"));
            Func<Source<LexicalToken>, int, SyntaxElement> missing56 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape137, CreateMissingToken("ContinueOnErrors"), CreateMissingToken("="), rules.MissingValue(source, start))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing57 = (source, start) => (SyntaxElement)new CustomNode(new [] {CD(), CD("Data", CompletionHint.None), CD()}, CreateMissingToken("["), rules.MissingInputText(source, start), CreateMissingToken("]"));
            Func<Source<LexicalToken>, int, SyntaxElement> missing58 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("NewColumnName", CompletionHint.None), CD(), CD("ColumnName", CompletionHint.Column)}, rules.MissingNameDeclaration(source, start), CreateMissingToken("="), rules.MissingNameReference(source, start))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing59 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(new [] {CD("NewTableName", CompletionHint.None), CD(), CD("TableName", CompletionHint.Table)}, rules.MissingNameDeclaration(source, start), CreateMissingToken("="), rules.MissingNameReference(source, start))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing60 = (source, start) => (SyntaxElement)new CustomNode(shape253, CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral(source, start))), CreateMissingToken(")"), CreateMissingToken("skip-results"), rules.MissingStringLiteral(source, start));
            Func<Source<LexicalToken>, int, SyntaxElement> missing61 = (source, start) => (SyntaxElement)new CustomNode(new [] {CD(), CD(CompletionHint.Literal), CD(), CD(CompletionHint.Literal, isOptional: true)}, CreateMissingToken("("), new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>(rules.MissingStringLiteral(source, start))), CreateMissingToken(")"), rules.MissingStringLiteral(source, start));
            Func<Source<LexicalToken>, int, SyntaxElement> missing62 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape262, CreateMissingToken("tags"), CreateMissingToken("!contains", "!has", "contains", "has"), rules.MissingStringLiteral(source, start))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing63 = (source, start) => CreateMissingToken("roles");
            Func<Source<LexicalToken>, int, SyntaxElement> missing64 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape272, rules.MissingNameReference(source, start), (SyntaxElement)new CustomNode(shape271, CreateMissingToken("if_later_than"), rules.MissingStringLiteral(source, start)))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing65 = (source, start) => (SyntaxElement)new CustomNode(shape99, CreateMissingToken("operations"), rules.MissingValue(source, start));
            Func<Source<LexicalToken>, int, SyntaxElement> missing66 = (source, start) => (SyntaxElement)new CustomNode(CreateMissingToken("corrupted"), CreateMissingToken("datetime"));
            Func<Source<LexicalToken>, int, SyntaxElement> missing67 = (source, start) => (SyntaxElement)new CustomNode(shape254, CreateMissingToken("databases"), (SyntaxElement)new CustomNode(shape305, CreateMissingToken("("), SyntaxList<SeparatedElement<SyntaxElement>>.Empty(), CreateMissingToken(")")));
            Func<Source<LexicalToken>, int, SyntaxElement> missing68 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape137, CreateMissingToken("reconstructCsl"), CreateMissingToken("="), rules.MissingValue(source, start))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing69 = (source, start) => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>((SyntaxElement)new CustomNode(shape137, CreateMissingToken("from"), CreateMissingToken("="), rules.MissingValue(source, start))));
            Func<Source<LexicalToken>, int, SyntaxElement> missing70 = (source, start) => (SyntaxElement)new CustomNode(CreateMissingToken("corrupted"), CreateMissingToken("datetime"));

            var fragment0 = Custom(
                    Token("recoverability"),
                    RequiredToken("="),
                    RequiredToken("disabled", "enabled"),
                    shape19);
            var fragment1 = Custom(
                    rules.NameDeclaration,
                    RequiredToken("="),
                    Required(rules.Value, rules.MissingValue),
                    shape24);
            var fragment2 = Custom(
                    Token("softdelete"),
                    RequiredToken("="),
                    Required(rules.Value, rules.MissingValue),
                    Optional(
                        fragment0),
                    shape27);
            var fragment3 = Custom(
                    Token("with"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            fragment1),
                        missing4),
                    RequiredToken(")"),
                    shape25);
            var fragment4 = Custom(
                    Token("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape43),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    shape44);
            var fragment5 = Custom(
                    Token("with"),
                    RequiredToken("("),
                    ZeroOrMoreCommaList(
                        fragment1),
                    RequiredToken(")"),
                    shape25);
            var fragment6 = Custom(
                    Token("<|"),
                    Required(rules.QueryInput, rules.MissingExpression),
                    shape48);
            var fragment7 = Custom(
                    Token("with"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.NameDeclaration,
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape53)),
                        missing11),
                    RequiredToken(")"),
                    shape25);
            var fragment8 = Custom(
                    Token("hotdata"),
                    RequiredToken("="),
                    Required(rules.Value, rules.MissingValue),
                    RequiredToken("hotindex"),
                    RequiredToken("="),
                    Required(rules.Value, rules.MissingValue),
                    shape62);
            var fragment9 = Custom(
                    Token("hot"),
                    RequiredToken("="),
                    Required(rules.Value, rules.MissingValue),
                    shape64);
            var fragment10 = Custom(
                    Token("cluster"),
                    RequiredToken("("),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    RequiredToken(")"),
                    RequiredToken("."),
                    RequiredToken("database"),
                    RequiredToken("("),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    RequiredToken(")"),
                    shape87);
            var fragment11 = Custom(
                    Token("database"),
                    RequiredToken("("),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    RequiredToken(")"),
                    shape88);
            var fragment12 = Custom(
                    rules.NameDeclaration,
                    RequiredToken(":"),
                    Required(rules.Type, rules.MissingType),
                    shape91);
            var fragment13 = Custom(
                    Token("bin"),
                    RequiredToken("("),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    RequiredToken(","),
                    Required(rules.Value, rules.MissingValue),
                    RequiredToken(")"),
                    shape93);
            var fragment14 = Custom(
                    Token("startofday"),
                    RequiredToken("("),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    RequiredToken(")"),
                    shape38);
            var fragment15 = Custom(
                    Token("startofmonth"),
                    RequiredToken("("),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    RequiredToken(")"),
                    shape38);
            var fragment16 = Custom(
                    Token("startofweek"),
                    RequiredToken("("),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    RequiredToken(")"),
                    shape38);
            var fragment17 = Custom(
                    Token("startofyear"),
                    RequiredToken("("),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    RequiredToken(")"),
                    shape38);
            var fragment18 = Custom(
                    Token("pathformat"),
                    RequiredToken("="),
                    RequiredToken("("),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape41)),
                    Required(
                        OneOrMoreList(
                            Custom(
                                First(
                                    Custom(
                                        Token("datetime_pattern"),
                                        RequiredToken("("),
                                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                                        RequiredToken(","),
                                        Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                                        RequiredToken(")"),
                                        shape98),
                                    Custom(
                                        If(Not(Token("datetime_pattern")), rules.NameDeclaration),
                                        shape40)),
                                Optional(
                                    Custom(
                                        rules.StringLiteral,
                                        shape41)),
                                shape99)),
                        missing9),
                    RequiredToken(")"),
                    shape100);
            var fragment19 = Custom(
                    Token("partition"),
                    RequiredToken("by"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.NameDeclaration,
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
                                                            fragment13,
                                                            fragment14,
                                                            fragment15,
                                                            fragment16,
                                                            fragment17),
                                                        missing6))),
                                            shape95),
                                        Custom(
                                            Token("long"),
                                            RequiredToken("="),
                                            RequiredToken("hash"),
                                            RequiredToken("("),
                                            Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                                            RequiredToken(","),
                                            Required(rules.Value, rules.MissingValue),
                                            RequiredToken(")"),
                                            shape104),
                                        Custom(
                                            Token("string"),
                                            Optional(
                                                Custom(
                                                    Token("="),
                                                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                                                    shape105)),
                                            shape95)),
                                    missing22),
                                shape96)),
                        missing23),
                    RequiredToken(")"),
                    Optional(
                        fragment18),
                    shape101);
            var fragment20 = Custom(
                    Token("catalog"),
                    RequiredToken("="),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape107);
            var fragment21 = Custom(
                    Token("from"),
                    rules.StringLiteral,
                    shape116);
            var fragment22 = Custom(
                    Token("from"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape116);
            var fragment23 = Custom(
                    Token("hotdata"),
                    RequiredToken("="),
                    Required(rules.Value, rules.MissingValue),
                    RequiredToken("hotindex"),
                    RequiredToken("="),
                    Required(rules.Value, rules.MissingValue),
                    shape122);
            var fragment24 = Custom(
                    Token("hot"),
                    RequiredToken("="),
                    Required(rules.Value, rules.MissingValue),
                    shape124);
            var fragment25 = Custom(
                    Optional(Token(",")),
                    OneOrMoreCommaList(
                        Custom(
                            Token("hot_window"),
                            RequiredToken("="),
                            Required(
                                Custom(
                                    rules.Value,
                                    RequiredToken(".."),
                                    Required(rules.Value, rules.MissingValue),
                                    shape126),
                                missing30),
                            shape128)),
                    shape129);
            var fragment26 = Custom(
                    First(
                        Token("dimensionMaterializedViews"),
                        Token("dimensionTables"),
                        Token("lookback"),
                        If(Not(And(Token("dimensionMaterializedViews", "dimensionTables", "lookback"))), rules.NameDeclaration)),
                    RequiredToken("="),
                    Required(rules.Value, rules.MissingValue),
                    shape137);
            var fragment27 = Custom(
                    Token("kind"),
                    RequiredToken("="),
                    RequiredToken("delta"),
                    shape143);
            var fragment28 = Custom(
                    Token("("),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    RequiredToken(")"),
                    shape145);
            var fragment29 = Custom(
                    Token("partition"),
                    RequiredToken("by"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.NameDeclaration,
                                RequiredToken(":"),
                                Required(
                                    First(
                                        Custom(
                                            Token("datetime"),
                                            RequiredToken("="),
                                            Required(
                                                First(
                                                    fragment13,
                                                    fragment14,
                                                    fragment15,
                                                    fragment16,
                                                    fragment17),
                                                missing6),
                                            shape152),
                                        Custom(
                                            Token("string"),
                                            RequiredToken("="),
                                            Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                                            shape153)),
                                    missing7),
                                shape96)),
                        missing8),
                    RequiredToken(")"),
                    Optional(
                        fragment18),
                    shape101);
            var fragment30 = Custom(
                    rules.ColumnNameReference,
                    RequiredToken("asc", "desc"),
                    shape157);
            var fragment31 = Custom(
                    Token("("),
                    Required(
                        OneOrMoreCommaList(
                            fragment12),
                        missing12),
                    RequiredToken(")"),
                    shape167);
            var fragment32 = Custom(
                    Token("with"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                First(
                                    Token("docstring"),
                                    Token("folder"),
                                    If(Not(And(Token("docstring", "folder"))), rules.NameDeclaration)),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape137)),
                        missing13),
                    RequiredToken(")"));
            var fragment33 = Custom(
                    rules.ColumnNameReference,
                    RequiredToken(":"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape170);
            var fragment34 = Custom(
                    Token("into"),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    shape178);
            var fragment35 = Custom(
                    Token("readonly"),
                    Optional(
                        Custom(
                            Token("version"),
                            RequiredToken("="),
                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                            shape180)),
                    shape181);
            var fragment36 = Custom(
                    Token("table"),
                    RequiredToken("="),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    shape185);
            var fragment37 = Custom(
                    Token("with"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                First(
                                    Token("autoUpdateSchema"),
                                    Token("backfill"),
                                    Token("dimensionMaterializedViews"),
                                    Token("dimensionTables"),
                                    Token("docString"),
                                    Token("effectiveDateTime"),
                                    Token("folder"),
                                    Token("lookback"),
                                    Token("updateExtentsCreationTime"),
                                    If(Not(And(Token("autoUpdateSchema", "backfill", "dimensionMaterializedViews", "dimensionTables", "docString", "effectiveDateTime", "folder", "lookback", "updateExtentsCreationTime"))), rules.NameDeclaration)),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape137)),
                        missing45),
                    RequiredToken(")"));
            var fragment38 = Custom(
                    rules.NameDeclaration,
                    RequiredToken("="),
                    Required(rules.Value, rules.MissingValue),
                    shape53);
            var fragment39 = Custom(
                    rules.NameDeclaration,
                    Required(
                        fragment31,
                        missing34),
                    shape199);
            var fragment40 = Custom(
                    Token("application"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape210);
            var fragment41 = Custom(
                    Token("user"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape212);
            var fragment42 = Custom(
                    Token("all"),
                    RequiredToken("tables"));
            var fragment43 = Custom(
                    Token("trim"),
                    RequiredToken("by"),
                    RequiredToken("datasize", "extentsize"),
                    Required(rules.Value, rules.MissingValue),
                    RequiredToken("bytes", "GB", "MB"),
                    shape220);
            var fragment44 = Custom(
                    Token("limit"),
                    Required(rules.Value, rules.MissingValue),
                    shape222);
            var fragment45 = Custom(
                    Token("from"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    shape225);
            var fragment46 = Custom(
                    Token("with"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                First(
                                    Token("ContinueOnErrors"),
                                    Token("ThrowOnErrors"),
                                    If(Not(And(Token("ContinueOnErrors", "ThrowOnErrors"))), rules.NameDeclaration)),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape137)),
                        missing56),
                    RequiredToken(")"));
            var fragment47 = Custom(
                    Token("with"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
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
                                    Token("small_dimension_table"),
                                    Token("tags"),
                                    Token("validationPolicy"),
                                    Token("zipPattern"),
                                    If(Not(And(Token("creationTime", "distributed", "docstring", "extend_schema", "folder", "format", "ignoreFirstRecord", "ingestIfNotExists", "ingestionMappingReference", "ingestionMapping", "persistDetails", "policy_ingestionTime", "recreate_schema", "small_dimension_table", "tags", "validationPolicy", "zipPattern"))), rules.NameDeclaration)),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                shape137)),
                        missing35),
                    RequiredToken(")"));
            var fragment48 = Custom(
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
                        Token("small_dimension_table"),
                        Token("tags"),
                        Token("validationPolicy"),
                        Token("zipPattern"),
                        If(Not(And(Token("creationTime", "distributed", "docstring", "extend_schema", "folder", "format", "ignoreFirstRecord", "ingestIfNotExists", "ingestionMappingReference", "ingestionMapping", "persistDetails", "policy_ingestionTime", "recreate_schema", "small_dimension_table", "tags", "validationPolicy", "zipPattern"))), rules.NameDeclaration)),
                    RequiredToken("="),
                    Required(rules.Value, rules.MissingValue),
                    shape137);
            var fragment49 = Custom(
                    Token("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape177),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    shape44);
            var fragment50 = Custom(
                    Token("with"),
                    RequiredToken("("),
                    RequiredToken("rebuild"),
                    RequiredToken("="),
                    RequiredToken("true"),
                    RequiredToken(")"));
            var fragment51 = Custom(
                    Token("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape253);
            var fragment52 = Custom(
                    Token("none"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    shape254);
            var fragment53 = Custom(
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            First(
                                rules.WildcardedNameDeclaration,
                                rules.DatabaseNameReference),
                            shape192),
                        fnMissingElement: rules.MissingNameDeclaration),
                    Token(")"),
                    shape167);
            var fragment54 = Custom(
                    Token("tags"),
                    RequiredToken("!contains", "!has", "contains", "has"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape262);
            var fragment55 = Custom(
                    Token("with"),
                    RequiredToken("("),
                    RequiredToken("extentsShowFilteringRuntimePolicy"),
                    RequiredToken("="),
                    Required(rules.Value, rules.MissingValue),
                    RequiredToken(")"),
                    shape263);
            var fragment56 = Custom(
                    Token("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                shape224),
                            fnMissingElement: rules.MissingValue),
                        missing53),
                    RequiredToken(")"),
                    shape44);
            var fragment57 = Custom(
                    Token(","),
                    OneOrMoreCommaList(
                        Custom(
                            First(
                                rules.WildcardedNameDeclaration,
                                rules.DatabaseNameReference),
                            shape192),
                        fnMissingElement: rules.MissingNameDeclaration),
                    shape267);
            var fragment58 = Custom(
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.AnyGuidLiteralOrString,
                            shape224),
                        fnMissingElement: rules.MissingValue),
                    Token(")"),
                    shape44);
            var fragment59 = Custom(
                    Token("where"),
                    Required(
                        OneOrMoreList(
                            fragment54,
                            separatorParser: Token("and")),
                        missing62));
            var fragment60 = Custom(
                    rules.DatabaseNameReference,
                    Optional(
                        Custom(
                            Token("if_later_than"),
                            rules.StringLiteral,
                            shape271)),
                    shape272);
            var fragment61 = Custom(
                    rules.DatabaseNameReference,
                    Optional(
                        Custom(
                            Token("if_later_than"),
                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                            shape271)),
                    shape272);
            var fragment62 = Custom(
                    Token(","),
                    OneOrMoreCommaList(
                        fragment60),
                    shape274);
            var fragment63 = Custom(
                    Token(","),
                    Required(
                        OneOrMoreCommaList(
                            fragment61),
                        missing64),
                    shape274);
            var fragment64 = Custom(
                    Token("if_later_than"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape283);
            var fragment65 = Custom(
                    Token("if_later_than"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape271);
            var fragment66 = Custom(
                    rules.StringLiteral,
                    RequiredToken("roles"),
                    shape292);
            var fragment67 = Custom(
                    Token("with"),
                    RequiredToken("("),
                    RequiredToken("scope"),
                    RequiredToken("="),
                    RequiredToken("cluster", "workloadgroup"),
                    RequiredToken(")"),
                    shape298);
            var fragment68 = Custom(
                    Token("("),
                    ZeroOrMoreCommaList(
                        Custom(
                            If(Not(Token(")")), rules.DatabaseNameReference),
                            shape84),
                        fnMissingElement: rules.MissingNameReference
                        ),
                    RequiredToken(")"),
                    shape305);
            var fragment69 = Custom(
                    Token("as"),
                    RequiredToken("json"));
            var fragment70 = Custom(
                    Token("corrupted"),
                    RequiredToken("datetime"));

            var AddClusterRole = Command("AddClusterRole", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("admins"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape1));

            var AddClusterRole2 = Command("AddClusterRole", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("alldatabasesadmins"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape1));

            var AddClusterRole3 = Command("AddClusterRole", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("alldatabasesmonitors"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape1));

            var AddClusterRole4 = Command("AddClusterRole", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("alldatabasesviewers"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape1));

            var AddClusterBlockedPrincipals = Command("AddClusterBlockedPrincipals", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("blockedprincipals"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        fragment40),
                    Optional(
                        fragment41),
                    Optional(
                        Custom(
                            Token("period"),
                            Required(rules.Value, rules.MissingValue),
                            new [] {CD(), CD("Period", CompletionHint.Literal)})),
                    Optional(
                        Custom(
                            Token("reason"),
                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                            new [] {CD(), CD("Reason", CompletionHint.Literal)})),
                    new [] {CD(), CD(), CD(), CD("Principal", CompletionHint.Literal), CD(isOptional: true), CD(isOptional: true), CD(isOptional: true), CD(isOptional: true)}));

            var AddClusterRole5 = Command("AddClusterRole", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("databasecreators"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape1));

            var AddClusterRole6 = Command("AddClusterRole", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("monitors"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape1));

            var AddClusterRole7 = Command("AddClusterRole", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("ops"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape1));

            var AddClusterRole8 = Command("AddClusterRole", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("users"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape1));

            var AddClusterRole9 = Command("AddClusterRole", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("viewers"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape1));

            var AddDatabaseRole = Command("AddDatabaseRole", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("admins", "ingestors", "monitors", "unrestrictedviewers", "users", "viewers"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape214));

            var AddExternalTableAdmins = Command("AddExternalTableAdmins", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("add", CompletionKind.CommandPrefix),
                        Token("external"),
                        RequiredToken("table"),
                        Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                        RequiredToken("admins"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.StringLiteral,
                                    shape5),
                                fnMissingElement: rules.MissingStringLiteral),
                            missing0),
                        RequiredToken(")"),
                        Optional(Token("skip-results")),
                        Optional(
                            Custom(
                                rules.StringLiteral,
                                shape6))}
                    ,
                    shape227));

            var AddFollowerDatabaseAuthorizedPrincipals = Command("AddFollowerDatabaseAuthorizedPrincipals", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("add", CompletionKind.CommandPrefix),
                        Token("follower"),
                        RequiredToken("database"),
                        Required(rules.DatabaseNameReference, rules.MissingNameReference),
                        Optional(
                            fragment22),
                        RequiredToken("admins", "monitors", "unrestrictedviewers", "users", "viewers"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.StringLiteral,
                                    shape5),
                                fnMissingElement: rules.MissingStringLiteral),
                            missing0),
                        RequiredToken(")"),
                        Optional(
                            Custom(
                                rules.StringLiteral,
                                shape6))}
                    ,
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(isOptional: true), CD("operationRole"), CD(), CD(CompletionHint.Literal), CD(), CD(CompletionHint.Literal, isOptional: true)}));

            var AddFunctionRole = Command("AddFunctionRole", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("function"),
                    Required(rules.FunctionNameReference, rules.MissingNameReference),
                    RequiredToken("admins"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape231));

            var AddMaterializedViewAdmins = Command("AddMaterializedViewAdmins", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    RequiredToken("admins"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape5),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape6)),
                    shape234));

            var AddTableRole = Command("AddTableRole", 
                Custom(
                    Token("add", CompletionKind.CommandPrefix),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredToken("admins", "ingestors"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape238));

            var AlterMergeExtentTagsFromQuery = Command("AlterMergeExtentTagsFromQuery", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter-merge", CompletionKind.CommandPrefix),
                        Token("async"),
                        RequiredToken("table"),
                        Required(rules.TableNameReference, rules.MissingNameReference),
                        RequiredToken("extent"),
                        RequiredToken("tags"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.StringLiteral,
                                    shape55),
                                fnMissingElement: rules.MissingStringLiteral),
                            missing0),
                        RequiredToken(")"),
                        Optional(
                            fragment5),
                        Required(
                            fragment6,
                            missing1)}
                    ,
                    shape56));

            var AlterMergeClusterPolicyCallout = Command("AlterMergeClusterPolicyCallout", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("callout"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape8));

            var AlterMergeClusterPolicyCapacity = Command("AlterMergeClusterPolicyCapacity", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("capacity"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape8));

            var AlterMergeClusterPolicyDiagnostics = Command("AlterMergeClusterPolicyDiagnostics", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("diagnostics"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape12));

            var AlterMergeClusterPolicyIngestionBatching = Command("AlterMergeClusterPolicyIngestionBatching", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("ingestionbatching"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape15));

            var AlterMergeClusterPolicyManagedIdentity = Command("AlterMergeClusterPolicyManagedIdentity", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("managed_identity"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape17));

            var AlterMergeClusterPolicyMultiDatabaseAdmins = Command("AlterMergeClusterPolicyMultiDatabaseAdmins", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("multidatabaseadmins"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape8));

            var AlterMergeClusterPolicyQueryWeakConsistency = Command("AlterMergeClusterPolicyQueryWeakConsistency", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("query_weak_consistency"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape8));

            var AlterMergeClusterPolicyRequestClassification = Command("AlterMergeClusterPolicyRequestClassification", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("request_classification"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape8));

            var AlterMergeClusterPolicyRowStore = Command("AlterMergeClusterPolicyRowStore", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("rowstore"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape58));

            var AlterMergeClusterPolicySharding = Command("AlterMergeClusterPolicySharding", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("sharding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape10));

            var AlterMergeClusterPolicyStreamingIngestion = Command("AlterMergeClusterPolicyStreamingIngestion", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("policy"),
                    RequiredToken("streamingingestion"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape10));

            var AlterMergeColumnPolicyEncoding = Command("AlterMergeColumnPolicyEncoding", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("column"),
                    Required(rules.TableColumnNameReference, rules.MissingNameReference),
                    RequiredToken("policy"),
                    RequiredToken("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape65));

            var AlterMergeDatabasePolicyDiagnostics = Command("AlterMergeDatabasePolicyDiagnostics", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    Token("diagnostics"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape12));

            var AlterMergeDatabasePolicyEncoding = Command("AlterMergeDatabasePolicyEncoding", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    Token("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape67));

            var AlterMergeDatabasePolicyIngestionBatching = Command("AlterMergeDatabasePolicyIngestionBatching", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    Token("ingestionbatching"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape15));

            var AlterMergeDatabasePolicyManagedIdentity = Command("AlterMergeDatabasePolicyManagedIdentity", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    Token("managed_identity"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape17));

            var AlterMergeDatabasePolicyMerge = Command("AlterMergeDatabasePolicyMerge", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    Token("merge"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape68));

            var AlterMergeDatabasePolicyRetention = Command("AlterMergeDatabasePolicyRetention", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    Token("retention"),
                    Required(
                        First(
                            fragment0,
                            fragment2,
                            Custom(
                                rules.StringLiteral,
                                shape34)),
                        missing2)));

            var AlterMergeDatabasePolicySharding = Command("AlterMergeDatabasePolicySharding", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    Token("sharding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape69));

            var AlterMergeDatabasePolicyShardsGrouping = Command("AlterMergeDatabasePolicyShardsGrouping", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    Token("shards_grouping").Hide(),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape70));

            var AlterMergeDatabasePolicyStreamingIngestion = Command("AlterMergeDatabasePolicyStreamingIngestion", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    RequiredToken("streamingingestion"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape59));

            var AlterMergeDatabaseIngestionMapping = Command("AlterMergeDatabaseIngestionMapping", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("ingestion"),
                    RequiredToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape72));

            var AlterMergeDatabasePolicyDiagnostics2 = Command("AlterMergeDatabasePolicyDiagnostics", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("diagnostics"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape74));

            var AlterMergeDatabasePolicyEncoding2 = Command("AlterMergeDatabasePolicyEncoding", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape75));

            var AlterMergeDatabasePolicyIngestionBatching2 = Command("AlterMergeDatabasePolicyIngestionBatching", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("ingestionbatching"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape77));

            var AlterMergeDatabasePolicyManagedIdentity2 = Command("AlterMergeDatabasePolicyManagedIdentity", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("managed_identity"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape78));

            var AlterMergeDatabasePolicyMerge2 = Command("AlterMergeDatabasePolicyMerge", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("merge"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape79));

            var AlterMergeDatabasePolicyMirroring = Command("AlterMergeDatabasePolicyMirroring", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter-merge", CompletionKind.CommandPrefix),
                        Token("database"),
                        If(Not(Token("policy")), rules.DatabaseNameReference),
                        Token("policy"),
                        Token("mirroring"),
                        Token("dataformat"),
                        RequiredToken("="),
                        RequiredToken("parquet"),
                        Optional(
                            fragment4),
                        Optional(
                            fragment3)}
                    ,
                    shape81));

            var AlterMergeDatabasePolicyMirroringWithJson = Command("AlterMergeDatabasePolicyMirroringWithJson", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("mirroring"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape31));

            var AlterMergeDatabasePolicyRetention2 = Command("AlterMergeDatabasePolicyRetention", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("retention"),
                    Required(
                        First(
                            fragment0,
                            fragment2,
                            Custom(
                                rules.StringLiteral,
                                shape34)),
                        missing2),
                    shape73));

            var AlterMergeDatabasePolicySharding2 = Command("AlterMergeDatabasePolicySharding", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("sharding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape82));

            var AlterMergeDatabasePolicyShardsGrouping2 = Command("AlterMergeDatabasePolicyShardsGrouping", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("shards_grouping").Hide(),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape83));

            var AlterMergeDatabasePolicyStreamingIngestion2 = Command("AlterMergeDatabasePolicyStreamingIngestion", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("policy"),
                    RequiredToken("streamingingestion"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("StreamingIngestionPolicy", CompletionHint.Literal)}));

            var AlterMergeDatabaseTableMirroringPolicyTemplates = Command("AlterMergeDatabaseTableMirroringPolicyTemplates", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(If(Not(Token("policy")), rules.DatabaseNameReference), rules.MissingNameReference),
                    RequiredToken("policy"),
                    RequiredToken("table_mirroring_templates"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape31));

            var AlterMergeEntityGroup = Command("AlterMergeEntityGroup", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("entity_group"),
                    Required(rules.EntityGroupNameReference, rules.MissingNameReference),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            First(
                                fragment10,
                                fragment11)),
                        missing5),
                    RequiredToken(")"),
                    new [] {CD(), CD(), CD("EntityGroupName", CompletionHint.EntityGroup), CD(), CD(), CD()}));

            var AlterMergeMaterializedViewPolicyMerge = Command("AlterMergeMaterializedViewPolicyMerge", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    RequiredToken("policy"),
                    RequiredToken("merge"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD("MergePolicy", CompletionHint.Literal)}));

            var AlterMergeMaterializedViewPolicyPartitioning = Command("AlterMergeMaterializedViewPolicyPartitioning", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.DatabaseMaterializedViewNameReference,
                    Token("policy"),
                    Token("partitioning"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape141));

            var AlterMergeMaterializedViewPolicyRetention = Command("AlterMergeMaterializedViewPolicyRetention", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Required(rules.DatabaseMaterializedViewNameReference, rules.MissingNameReference),
                    RequiredToken("policy"),
                    RequiredToken("retention"),
                    Required(
                        First(
                            fragment0,
                            fragment2,
                            Custom(
                                rules.StringLiteral,
                                shape34)),
                        missing2),
                    shape140));

            var AlterMergeMirroringTemplate = Command("AlterMergeMirroringTemplate", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("mirroring-template"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Optional(
                        fragment27),
                    Optional(
                        fragment28),
                    Optional(
                        fragment3),
                    shape146));

            var AlterMergeTablePolicyEncoding = Command("AlterMergeTablePolicyEncoding", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape149));

            var AlterMergeTablePolicyIngestionBatching = Command("AlterMergeTablePolicyIngestionBatching", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("ingestionbatching"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape150));

            var AlterMergeTablePolicyMerge = Command("AlterMergeTablePolicyMerge", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("merge"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape151));

            var AlterMergeTablePolicyMirroring = Command("AlterMergeTablePolicyMirroring", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter-merge", CompletionKind.CommandPrefix),
                        Token("table"),
                        rules.DatabaseTableNameReference,
                        Token("policy"),
                        Token("mirroring"),
                        Optional(
                            fragment29),
                        RequiredToken("kind"),
                        RequiredToken("="),
                        RequiredToken("delta"),
                        Optional(
                            fragment4),
                        Optional(
                            fragment3)}
                    ,
                    shape154));

            var AlterMergeTablePolicyPartitioning = Command("AlterMergeTablePolicyPartitioning", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("partitioning"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape155));

            var AlterMergeTablePolicyRetention = Command("AlterMergeTablePolicyRetention", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("retention"),
                    Required(
                        First(
                            fragment0,
                            fragment2,
                            Custom(
                                rules.StringLiteral,
                                shape34)),
                        missing2),
                    shape148));

            var AlterMergeTablePolicyRowOrder = Command("AlterMergeTablePolicyRowOrder", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("roworder"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            fragment30),
                        missing10),
                    RequiredToken(")"),
                    shape158));

            var AlterMergeTablePolicySharding = Command("AlterMergeTablePolicySharding", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("sharding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape159));

            var AlterMergeTablePolicyStreamingIngestion = Command("AlterMergeTablePolicyStreamingIngestion", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("streamingingestion"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("StreamingIngestionPolicy", CompletionHint.Literal)}));

            var AlterMergeTablePolicyUpdate = Command("AlterMergeTablePolicyUpdate", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    RequiredToken("policy"),
                    RequiredToken("update"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        fragment7),
                    shape161));

            var AlterMergeTable = Command("AlterMergeTable", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    fragment31,
                    Optional(
                        fragment32),
                    shape168));

            var AlterMergeTableColumnDocStrings = Command("AlterMergeTableColumnDocStrings", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("column-docstrings"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            fragment33),
                        missing14),
                    RequiredToken(")"),
                    shape171));

            var AlterMergeExtentTagsFromQuery2 = Command("AlterMergeExtentTagsFromQuery", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter-merge", CompletionKind.CommandPrefix),
                        Token("table"),
                        rules.TableNameReference,
                        Token("extent"),
                        RequiredToken("tags"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.StringLiteral,
                                    shape55),
                                fnMissingElement: rules.MissingStringLiteral),
                            missing0),
                        RequiredToken(")"),
                        Optional(
                            fragment5),
                        Required(
                            fragment6,
                            missing1)}
                    ,
                    shape172));

            var AlterMergeTableIngestionMapping = Command("AlterMergeTableIngestionMapping", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    RequiredToken("ingestion"),
                    RequiredToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        fragment7),
                    shape173));

            var AlterMergeTablePolicyMirroringWithJson = Command("AlterMergeTablePolicyMirroringWithJson", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    Token("table"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("policy"),
                    RequiredToken("mirroring"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape175));

            var AlterMergeWorkloadGroup = Command("AlterMergeWorkloadGroup", 
                Custom(
                    Token("alter-merge", CompletionKind.CommandPrefix),
                    RequiredToken("workload_group"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape190));

            var AlterClusterStorageKeys = Command("AlterClusterStorageKeys", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("async"),
                    Token("cluster"),
                    RequiredToken("storage"),
                    RequiredToken("keys"),
                    Optional(
                        fragment7),
                    RequiredToken("decryption-certificate-thumbprint"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD(), CD(isOptional: true), CD(), CD("thumbprint", CompletionHint.Literal)}));

            var AlterDatabaseStorageKeys = Command("AlterDatabaseStorageKeys", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("async"),
                    Token("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("storage"),
                    RequiredToken("keys"),
                    Optional(
                        fragment7),
                    RequiredToken("decryption-certificate-thumbprint"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(), CD(isOptional: true), CD(), CD("thumbprint", CompletionHint.Literal)}));

            var AlterExtentTagsFromQuery = Command("AlterExtentTagsFromQuery", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("async"),
                        RequiredToken("table"),
                        Required(rules.TableNameReference, rules.MissingNameReference),
                        RequiredToken("extent"),
                        RequiredToken("tags"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.StringLiteral,
                                    shape55),
                                fnMissingElement: rules.MissingStringLiteral),
                            missing0),
                        RequiredToken(")"),
                        Optional(
                            fragment5),
                        Required(
                            fragment6,
                            missing1)}
                    ,
                    shape56));

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
                                CD("NodeList", CompletionHint.Literal))),
                        missing15),
                    Required(rules.BracketedStringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("Action", CompletionHint.Literal)}));

            var AlterClusterPolicyCaching = Command("AlterClusterPolicyCaching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("caching"),
                    Required(
                        First(
                            fragment8,
                            fragment9),
                        missing16)));

            var AlterClusterPolicyCallout = Command("AlterClusterPolicyCallout", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("callout"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape8));

            var AlterClusterPolicyCapacity = Command("AlterClusterPolicyCapacity", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("capacity"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape8));

            var AlterClusterPolicyDiagnostics = Command("AlterClusterPolicyDiagnostics", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("diagnostics"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape12));

            var AlterClusterPolicyIngestionBatching = Command("AlterClusterPolicyIngestionBatching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("ingestionbatching"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape15));

            var AlterClusterPolicyManagedIdentity = Command("AlterClusterPolicyManagedIdentity", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("managed_identity"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape17));

            var AlterClusterPolicyMultiDatabaseAdmins = Command("AlterClusterPolicyMultiDatabaseAdmins", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("multidatabaseadmins"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape8));

            var AlterClusterPolicyQueryWeakConsistency = Command("AlterClusterPolicyQueryWeakConsistency", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("query_weak_consistency"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape8));

            var AlterClusterPolicyRequestClassification = Command("AlterClusterPolicyRequestClassification", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("request_classification"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    RequiredToken("<|"),
                    Required(rules.QueryInput, rules.MissingExpression),
                    new [] {CD(), CD(), CD(), CD(), CD("Policy", CompletionHint.Literal), CD(), CD("Query", CompletionHint.Tabular)}));

            var AlterClusterPolicyRowStore = Command("AlterClusterPolicyRowStore", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("rowstore"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape58));

            var AlterClusterPolicySandbox = Command("AlterClusterPolicySandbox", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("sandbox"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("SandboxPolicy", CompletionHint.Literal)}));

            var AlterClusterPolicySharding = Command("AlterClusterPolicySharding", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    Token("sharding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape10));

            var AlterClusterPolicyStreamingIngestion = Command("AlterClusterPolicyStreamingIngestion", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("policy"),
                    RequiredToken("streamingingestion"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape59));

            var AlterClusterStorageKeys2 = Command("AlterClusterStorageKeys", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("storage"),
                    RequiredToken("keys"),
                    Optional(
                        fragment7),
                    RequiredToken("decryption-certificate-thumbprint"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD(isOptional: true), CD(), CD("thumbprint", CompletionHint.Literal)}));

            var AlterColumnsPolicyEncodingByQuery = Command("AlterColumnsPolicyEncodingByQuery", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("columns"),
                    RequiredToken("policy"),
                    RequiredToken("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    RequiredToken("<|"),
                    Required(rules.QueryInput, rules.MissingExpression),
                    new [] {CD(), CD(), CD(), CD(), CD("EncodingPolicy", CompletionHint.Literal), CD(), CD("QueryOrCommand", CompletionHint.Tabular)}));

            var AlterColumnPolicyCaching = Command("AlterColumnPolicyCaching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("column"),
                    rules.DatabaseTableColumnNameReference,
                    Token("policy"),
                    Token("caching"),
                    Required(
                        First(
                            fragment8,
                            fragment9),
                        missing16),
                    new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD(), CD()}));

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
                    new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD(), CD(), CD(), CD("EncodingPolicyType", CompletionHint.Literal)}));

            var AlterColumnPolicyEncoding = Command("AlterColumnPolicyEncoding", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("column"),
                    rules.DatabaseTableColumnNameReference,
                    Token("policy"),
                    RequiredToken("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape65));

            var AlterColumnType = Command("AlterColumnType", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("column"),
                    Required(rules.DatabaseTableColumnNameReference, rules.MissingNameReference),
                    RequiredToken("type"),
                    RequiredToken("="),
                    Required(rules.Type, rules.MissingType),
                    new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD(), CD("ColumnType")}));

            var AlterDatabasePolicyCaching = Command("AlterDatabasePolicyCaching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    Token("caching"),
                    Required(
                        First(
                            fragment8,
                            fragment9),
                        missing16)));

            var AlterDatabasePolicyDiagnostics = Command("AlterDatabasePolicyDiagnostics", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    Token("diagnostics"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape12));

            var AlterDatabasePolicyEncoding = Command("AlterDatabasePolicyEncoding", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    Token("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape67));

            var AlterDatabasePolicyExtentTagsRetention = Command("AlterDatabasePolicyExtentTagsRetention", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    Token("extent_tags_retention"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("ExtentTagsRetentionPolicy", CompletionHint.Literal)}));

            var AlterDatabasePolicyIngestionBatching = Command("AlterDatabasePolicyIngestionBatching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    Token("ingestionbatching"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape15));

            var AlterDatabasePolicyManagedIdentity = Command("AlterDatabasePolicyManagedIdentity", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    Token("managed_identity"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape17));

            var AlterDatabasePolicyMerge = Command("AlterDatabasePolicyMerge", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    Token("merge"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape68));

            var AlterDatabasePolicyRetention = Command("AlterDatabasePolicyRetention", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    Token("retention"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("RetentionPolicy", CompletionHint.Literal)}));

            var AlterDatabasePolicySharding = Command("AlterDatabasePolicySharding", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    Token("sharding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape69));

            var AlterDatabasePolicyShardsGrouping = Command("AlterDatabasePolicyShardsGrouping", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    Token("shards_grouping").Hide(),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape70));

            var AlterDatabasePolicyStreamingIngestion = Command("AlterDatabasePolicyStreamingIngestion", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    RequiredToken("streamingingestion"),
                    Required(
                        First(
                            Custom(
                                Token("disable"),
                                shape71),
                            Custom(
                                Token("enable"),
                                shape71),
                            Custom(
                                rules.StringLiteral,
                                shape30)),
                        missing17)));

            var AlterDatabaseIngestionMapping = Command("AlterDatabaseIngestionMapping", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("ingestion"),
                    RequiredToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape72));

            var AlterDatabasePersistMetadata = Command("AlterDatabasePersistMetadata", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("persist"),
                    RequiredToken("metadata"),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            Optional(
                                Custom(
                                    Token("allow-non-empty-container"),
                                    CD("AllowNonEmptyContainer"))),
                            new [] {CD("Path", CompletionHint.Literal), CD(isOptional: true)})),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD(CompletionHint.Literal, isOptional: true)}));

            var AlterDatabasePolicyCaching2 = Command("AlterDatabasePolicyCaching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("caching"),
                    Required(
                        First(
                            fragment8,
                            fragment9),
                        missing16),
                    shape73));

            var AlterDatabasePolicyDiagnostics2 = Command("AlterDatabasePolicyDiagnostics", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("diagnostics"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape74));

            var AlterDatabasePolicyEncoding2 = Command("AlterDatabasePolicyEncoding", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape75));

            var AlterDatabasePolicyExtentTagsRetention2 = Command("AlterDatabasePolicyExtentTagsRetention", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("extent_tags_retention"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("ExtentTagsRetentionPolicy", CompletionHint.Literal)}));

            var AlterDatabasePolicyIngestionBatching2 = Command("AlterDatabasePolicyIngestionBatching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("ingestionbatching"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape77));

            var AlterDatabasePolicyManagedIdentity2 = Command("AlterDatabasePolicyManagedIdentity", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("managed_identity"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape78));

            var AlterDatabasePolicyMerge2 = Command("AlterDatabasePolicyMerge", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("merge"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape79));

            var AlterDatabasePolicyMirroring = Command("AlterDatabasePolicyMirroring", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("database"),
                        If(Not(Token("policy")), rules.DatabaseNameReference),
                        Token("policy"),
                        Token("mirroring"),
                        Token("dataformat"),
                        RequiredToken("="),
                        RequiredToken("parquet"),
                        Optional(
                            fragment4),
                        Optional(
                            fragment3)}
                    ,
                    shape81));

            var AlterDatabasePolicyMirroringWithJson = Command("AlterDatabasePolicyMirroringWithJson", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("mirroring"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape31));

            var AlterDatabasePolicyRetention2 = Command("AlterDatabasePolicyRetention", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("retention"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("RetentionPolicy", CompletionHint.Literal)}));

            var AlterDatabasePolicySharding2 = Command("AlterDatabasePolicySharding", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("sharding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape82));

            var AlterDatabasePolicyShardsGrouping2 = Command("AlterDatabasePolicyShardsGrouping", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("shards_grouping").Hide(),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape83));

            var AlterDatabasePolicyStreamingIngestion2 = Command("AlterDatabasePolicyStreamingIngestion", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("policy"),
                    RequiredToken("streamingingestion"),
                    Required(
                        First(
                            Custom(
                                Token("disable"),
                                shape71),
                            Custom(
                                Token("enable"),
                                shape71),
                            Custom(
                                rules.StringLiteral,
                                shape30)),
                        missing17),
                    shape73));

            var AlterDatabaseTableMirroringPolicyTemplates = Command("AlterDatabaseTableMirroringPolicyTemplates", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("policy"),
                    RequiredToken("table_mirroring_templates"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape31));

            var AlterDatabasePrettyName = Command("AlterDatabasePrettyName", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("prettyname"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape257));

            var AlterDatabaseStorageKeys2 = Command("AlterDatabaseStorageKeys", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("storage"),
                    RequiredToken("keys"),
                    Optional(
                        fragment7),
                    RequiredToken("decryption-certificate-thumbprint"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(), CD(isOptional: true), CD(), CD("thumbprint", CompletionHint.Literal)}));

            var AlterEntityGroup = Command("AlterEntityGroup", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("entity_group"),
                    Required(rules.EntityGroupNameReference, rules.MissingNameReference),
                    RequiredToken("("),
                    Required(
                        Custom(
                            OneOrMoreCommaList(
                                First(
                                    fragment10,
                                    fragment11)),
                            RequiredToken(")")),
                        missing18),
                    new [] {CD(), CD(), CD("EntityGroupName", CompletionHint.EntityGroup), CD(), CD()}));

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
                            shape89)),
                    new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(CompletionHint.Literal, isOptional: true)}));

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
                                new [] {CD(), CD("hours", CompletionHint.Literal), CD()}),
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                shape89)),
                        missing19),
                    shape293));

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
                    new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(), CD("container", CompletionHint.Literal), CD(), CD()}));

            var AlterStorageExternalTable = Command("AlterStorageExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.NameDeclaration,
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                fragment12),
                            missing12),
                        RequiredToken(")"),
                        RequiredToken("kind"),
                        RequiredToken("="),
                        RequiredToken("adl").Hide(),
                        Optional(
                            fragment19),
                        Optional(
                            fragment20),
                        RequiredToken("dataformat"),
                        RequiredToken("="),
                        RequiredToken("apacheavro", "avro", "csv", "json", "multijson", "orc", "parquet", "psv", "raw", "scsv", "sohsv", "sstream", "tsve", "tsv", "txt", "w3clogfile"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.StringLiteral,
                                    shape43),
                                fnMissingElement: rules.MissingStringLiteral),
                            missing0),
                        RequiredToken(")"),
                        Optional(
                            fragment3)}
                    ,
                    shape108));

            var AlterStorageExternalTable2 = Command("AlterStorageExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.NameDeclaration,
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                fragment12),
                            missing12),
                        RequiredToken(")"),
                        RequiredToken("kind"),
                        RequiredToken("="),
                        RequiredToken("blob").Hide(),
                        Optional(
                            fragment19),
                        Optional(
                            fragment20),
                        RequiredToken("dataformat"),
                        RequiredToken("="),
                        RequiredToken("apacheavro", "avro", "csv", "json", "multijson", "orc", "parquet", "psv", "raw", "scsv", "sohsv", "sstream", "tsve", "tsv", "txt", "w3clogfile"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.StringLiteral,
                                    shape43),
                                fnMissingElement: rules.MissingStringLiteral),
                            missing0),
                        RequiredToken(")"),
                        Optional(
                            fragment3)}
                    ,
                    shape108));

            var AlterStorageExternalTable3 = Command("AlterStorageExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.NameDeclaration,
                        Token("("),
                        OneOrMoreCommaList(
                            fragment12),
                        Token(")"),
                        Token("kind"),
                        RequiredToken("="),
                        RequiredToken("delta"),
                        RequiredToken("("),
                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                        RequiredToken(")"),
                        Optional(
                            fragment3)}
                    ,
                    shape184));

            var AlterSqlExternalTable = Command("AlterSqlExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.NameDeclaration,
                        Token("("),
                        OneOrMoreCommaList(
                            fragment12),
                        Token(")"),
                        Token("kind"),
                        RequiredToken("="),
                        RequiredToken("sql"),
                        Optional(
                            fragment36),
                        RequiredToken("("),
                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                        RequiredToken(")"),
                        Optional(
                            fragment3)}
                    ,
                    shape186));

            var AlterStorageExternalTable4 = Command("AlterStorageExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.NameDeclaration,
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                fragment12),
                            missing12),
                        RequiredToken(")"),
                        RequiredToken("kind"),
                        RequiredToken("="),
                        RequiredToken("storage"),
                        Optional(
                            fragment19),
                        Optional(
                            fragment20),
                        RequiredToken("dataformat"),
                        RequiredToken("="),
                        RequiredToken("apacheavro", "avro", "csv", "json", "multijson", "orc", "parquet", "psv", "raw", "scsv", "sohsv", "sstream", "tsve", "tsv", "txt", "w3clogfile"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.StringLiteral,
                                    shape43),
                                fnMissingElement: rules.MissingStringLiteral),
                            missing0),
                        RequiredToken(")"),
                        Optional(
                            fragment3)}
                    ,
                    shape108));

            var AlterExternalTableDocString = Command("AlterExternalTableDocString", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    rules.ExternalTableNameReference,
                    Token("docstring"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD("tableName", CompletionHint.ExternalTable), CD(), CD("docStringValue", CompletionHint.Literal)}));

            var AlterExternalTableFolder = Command("AlterExternalTableFolder", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    rules.ExternalTableNameReference,
                    RequiredToken("folder"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD("tableName", CompletionHint.ExternalTable), CD(), CD("folderValue", CompletionHint.Literal)}));

            var AlterStorageExternalTable5 = Command("AlterStorageExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.NameDeclaration,
                        Token("kind"),
                        RequiredToken("="),
                        RequiredToken("delta"),
                        RequiredToken("("),
                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                        RequiredToken(")"),
                        Optional(
                            fragment3)}
                    ,
                    shape187));

            var AlterExternalTableMapping = Command("AlterExternalTableMapping", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    rules.ExternalTableNameReference,
                    Token("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape196));

            var AlterExternalTablePolicyQueryAcceleration = Command("AlterExternalTablePolicyQueryAcceleration", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("external"),
                    RequiredToken("table"),
                    Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                    RequiredToken("policy"),
                    RequiredToken("query_acceleration"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD(), CD("Policy", CompletionHint.Literal)}));

            var AlterFabricServiceAssignmentsCommand = Command("AlterFabricServiceAssignmentsCommand", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("fabric"),
                    RequiredToken("service"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(
                        First(
                            Custom(
                                Token("assignments"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                new [] {CD(), CD("serviceToNode", CompletionHint.Literal)}),
                            Custom(
                                Token("assignment"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                RequiredToken("to"),
                                Required(rules.StringLiteral, rules.MissingStringLiteral),
                                new [] {CD(), CD("serviceId", CompletionHint.Literal), CD(), CD("nodeId", CompletionHint.Literal)})),
                        missing26),
                    shape229));

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
                                Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                                new [] {CD(), CD(), CD("databaseNamePrefix", CompletionHint.None)}),
                            Custom(
                                Token("default-caching-policies-modification-kind"),
                                RequiredToken("="),
                                RequiredToken("none", "replace", "union"),
                                shape114),
                            Custom(
                                Token("default-principals-modification-kind"),
                                RequiredToken("="),
                                RequiredToken("none", "replace", "union"),
                                shape114),
                            Custom(
                                Token("follow-authorized-principals"),
                                RequiredToken("="),
                                Required(rules.Value, rules.MissingValue),
                                new [] {CD(), CD(), CD("followAuthorizedPrincipals", CompletionHint.Literal)})),
                        missing27),
                    new [] {CD(), CD(), CD(), CD(), CD(), CD("leaderClusterMetadataPath", CompletionHint.Literal), CD()}));

            var AlterFollowerDatabaseConfiguration = Command("AlterFollowerDatabaseConfiguration", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("follower"),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Optional(
                        fragment21),
                    Token("caching-policies-modification-kind"),
                    RequiredToken("="),
                    RequiredToken("none", "replace", "union"),
                    shape132));

            var AlterFollowerDatabaseConfiguration2 = Command("AlterFollowerDatabaseConfiguration", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("follower"),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Optional(
                        fragment21),
                    Token("database-name-override"),
                    RequiredToken("="),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(isOptional: true), CD(), CD(), CD("databaseNameOverride", CompletionHint.None)}));

            var AlterFollowerDatabaseChildEntities = Command("AlterFollowerDatabaseChildEntities", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("follower"),
                        Token("database"),
                        rules.DatabaseNameReference,
                        Optional(
                            fragment21),
                        Token("external"),
                        RequiredToken("tables"),
                        RequiredToken("exclude", "include"),
                        RequiredToken("add", "drop"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.WildcardedNameDeclaration,
                                    CD("ename", CompletionHint.None)),
                                fnMissingElement: rules.MissingNameDeclaration),
                            missing28),
                        RequiredToken(")")}
                    ,
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(isOptional: true), CD(), CD(), CD("entityListKind"), CD("operationName"), CD(), CD(CompletionHint.None), CD()}));

            var AlterFollowerTablesPolicyCaching = Command("AlterFollowerTablesPolicyCaching", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("follower"),
                        RequiredToken("database"),
                        Required(rules.DatabaseNameReference, rules.MissingNameReference),
                        Optional(
                            fragment22),
                        RequiredToken("materialized-views"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.NameDeclaration,
                                    shape133),
                                fnMissingElement: rules.MissingNameDeclaration),
                            missing28),
                        RequiredToken(")"),
                        RequiredToken("policy"),
                        RequiredToken("caching"),
                        Required(
                            First(
                                fragment23,
                                fragment24),
                            missing29),
                        Optional(
                            fragment25)}
                    ,
                    shape134));

            var AlterFollowerDatabaseChildEntities2 = Command("AlterFollowerDatabaseChildEntities", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("follower"),
                        Token("database"),
                        rules.DatabaseNameReference,
                        Optional(
                            fragment21),
                        Token("materialized-views"),
                        RequiredToken("exclude"),
                        RequiredToken("add", "drop"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.WildcardedNameDeclaration,
                                    CD("ename", CompletionHint.None)),
                                fnMissingElement: rules.MissingNameDeclaration),
                            missing28),
                        RequiredToken(")")}
                    ,
                    shape119));

            var AlterFollowerDatabaseChildEntities3 = Command("AlterFollowerDatabaseChildEntities", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("follower"),
                        Token("database"),
                        rules.DatabaseNameReference,
                        Optional(
                            fragment21),
                        Token("materialized-views"),
                        RequiredToken("include"),
                        RequiredToken("add", "drop"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.WildcardedNameDeclaration,
                                    CD("ename", CompletionHint.None)),
                                fnMissingElement: rules.MissingNameDeclaration),
                            missing28),
                        RequiredToken(")")}
                    ,
                    shape119));

            var AlterFollowerTablesPolicyCaching2 = Command("AlterFollowerTablesPolicyCaching", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("follower"),
                        RequiredToken("database"),
                        Required(rules.DatabaseNameReference, rules.MissingNameReference),
                        Optional(
                            fragment22),
                        RequiredToken("materialized-view"),
                        Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                        RequiredToken("policy"),
                        RequiredToken("caching"),
                        Required(
                            First(
                                fragment23,
                                fragment24),
                            missing29),
                        Optional(
                            fragment25)}
                    ,
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(isOptional: true), CD(), CD("name", CompletionHint.MaterializedView), CD(), CD(), CD(), CD("hotWindows", isOptional: true)}));

            var AlterFollowerDatabaseConfiguration3 = Command("AlterFollowerDatabaseConfiguration", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("follower"),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Optional(
                        fragment21),
                    Token("metadata"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(isOptional: true), CD(), CD("serializedDatabaseMetadataOverride", CompletionHint.Literal)}));

            var AlterFollowerDatabaseAuthorizedPrincipals = Command("AlterFollowerDatabaseAuthorizedPrincipals", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("follower"),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Optional(
                        fragment21),
                    Token("policy"),
                    RequiredToken("caching"),
                    Required(
                        First(
                            fragment23,
                            fragment24),
                        missing29),
                    Optional(
                        fragment25),
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(isOptional: true), CD(), CD(), CD(), CD("hotWindows", isOptional: true)}));

            var AlterFollowerDatabaseConfiguration4 = Command("AlterFollowerDatabaseConfiguration", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("follower"),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Optional(
                        fragment21),
                    Token("prefetch-extents"),
                    RequiredToken("="),
                    Required(rules.Value, rules.MissingValue),
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(isOptional: true), CD(), CD(), CD("prefetchExtents", CompletionHint.Literal)}));

            var AlterFollowerDatabaseConfiguration5 = Command("AlterFollowerDatabaseConfiguration", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("follower"),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Optional(
                        fragment21),
                    Token("principals-modification-kind"),
                    RequiredToken("="),
                    RequiredToken("none", "replace", "union"),
                    shape132));

            var AlterFollowerTablesPolicyCaching3 = Command("AlterFollowerTablesPolicyCaching", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("follower"),
                        RequiredToken("database"),
                        Required(rules.DatabaseNameReference, rules.MissingNameReference),
                        Optional(
                            fragment22),
                        RequiredToken("tables"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.NameDeclaration,
                                    shape133),
                                fnMissingElement: rules.MissingNameDeclaration),
                            missing28),
                        RequiredToken(")"),
                        RequiredToken("policy"),
                        RequiredToken("caching"),
                        Required(
                            First(
                                fragment23,
                                fragment24),
                            missing29),
                        Optional(
                            fragment25)}
                    ,
                    shape134));

            var AlterFollowerDatabaseChildEntities4 = Command("AlterFollowerDatabaseChildEntities", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("follower"),
                        Token("database"),
                        rules.DatabaseNameReference,
                        Optional(
                            fragment21),
                        Token("tables"),
                        RequiredToken("exclude"),
                        RequiredToken("add", "drop"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.WildcardedNameDeclaration,
                                    CD("ename", CompletionHint.None)),
                                fnMissingElement: rules.MissingNameDeclaration),
                            missing28),
                        RequiredToken(")")}
                    ,
                    shape119));

            var AlterFollowerDatabaseChildEntities5 = Command("AlterFollowerDatabaseChildEntities", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("follower"),
                        Token("database"),
                        rules.DatabaseNameReference,
                        Optional(
                            fragment21),
                        Token("tables"),
                        RequiredToken("include"),
                        RequiredToken("add", "drop"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.WildcardedNameDeclaration,
                                    CD("ename", CompletionHint.None)),
                                fnMissingElement: rules.MissingNameDeclaration),
                            missing28),
                        RequiredToken(")")}
                    ,
                    shape119));

            var AlterFollowerTablesPolicyCaching4 = Command("AlterFollowerTablesPolicyCaching", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("follower"),
                        RequiredToken("database"),
                        Required(rules.DatabaseNameReference, rules.MissingNameReference),
                        Optional(
                            fragment22),
                        RequiredToken("table"),
                        Required(rules.TableNameReference, rules.MissingNameReference),
                        RequiredToken("policy"),
                        RequiredToken("caching"),
                        Required(
                            First(
                                fragment23,
                                fragment24),
                            missing29),
                        Optional(
                            fragment25)}
                    ,
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(isOptional: true), CD(), CD("name", CompletionHint.Table), CD(), CD(), CD(), CD("hotWindows", isOptional: true)}));

            var AlterFunction = Command("AlterFunction", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("function"),
                    RequiredToken("with"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            fragment1),
                        missing4),
                    RequiredToken(")"),
                    Required(If(Not(Token("with")), rules.FunctionNameReference), rules.MissingNameReference),
                    Required(rules.FunctionDeclaration, rules.MissingFunctionDeclaration),
                    new [] {CD(), CD(), CD(), CD(), CD(CompletionHint.None), CD(), CD("FunctionName", CompletionHint.Function), CD()}));

            var AlterFunctionDocString = Command("AlterFunctionDocString", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("function"),
                    If(Not(Token("with")), rules.FunctionNameReference),
                    Token("docstring"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(CompletionHint.Function), CD(), CD("Documentation", CompletionHint.Literal)}));

            var AlterFunctionFolder = Command("AlterFunctionFolder", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("function"),
                    If(Not(Token("with")), rules.FunctionNameReference),
                    Token("folder"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD(), CD("Folder", CompletionHint.Literal)}));

            var AlterFunction2 = Command("AlterFunction", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("function"),
                    Required(If(Not(Token("with")), rules.FunctionNameReference), rules.MissingNameReference),
                    Required(rules.FunctionDeclaration, rules.MissingFunctionDeclaration),
                    shape232));

            var AlterGraphModelPolicyRetention = Command("AlterGraphModelPolicyRetention", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("graph_model").Hide(),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    RequiredToken("policy"),
                    RequiredToken("retention"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("ModelName", CompletionHint.None), CD(), CD(), CD("RetentionPolicy", CompletionHint.Literal)}));

            var AlterMaterializedViewOverMaterializedView = Command("AlterMaterializedViewOverMaterializedView", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("materialized-view"),
                        Token("with"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                fragment26),
                            missing31),
                        RequiredToken(")"),
                        Required(If(Not(Token("with")), rules.MaterializedViewNameReference), rules.MissingNameReference),
                        RequiredToken("on"),
                        RequiredToken("materialized-view"),
                        Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                        Required(rules.FunctionBody, rules.MissingFunctionBody)}
                    ,
                    new [] {CD(), CD(), CD(), CD(), CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD(CompletionHint.MaterializedView), CD()}));

            var AlterMaterializedView = Command("AlterMaterializedView", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("materialized-view"),
                        Token("with"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                fragment26),
                            missing31),
                        RequiredToken(")"),
                        Required(If(Not(Token("with")), rules.MaterializedViewNameReference), rules.MissingNameReference),
                        RequiredToken("on"),
                        RequiredToken("table"),
                        Required(rules.TableNameReference, rules.MissingNameReference),
                        Required(rules.FunctionBody, rules.MissingFunctionBody)}
                    ,
                    new [] {CD(), CD(), CD(), CD(), CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD(CompletionHint.Table), CD()}));

            var AlterMaterializedViewAutoUpdateSchema = Command("AlterMaterializedViewAutoUpdateSchema", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    If(Not(Token("with")), rules.MaterializedViewNameReference),
                    Token("autoUpdateSchema"),
                    RequiredToken("false", "true"),
                    shape207));

            var AlterMaterializedViewDocString = Command("AlterMaterializedViewDocString", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    If(Not(Token("with")), rules.MaterializedViewNameReference),
                    Token("docstring"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD("Documentation", CompletionHint.Literal)}));

            var AlterMaterializedViewFolder = Command("AlterMaterializedViewFolder", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    If(Not(Token("with")), rules.MaterializedViewNameReference),
                    Token("folder"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD("Folder", CompletionHint.Literal)}));

            var AlterMaterializedViewLookback = Command("AlterMaterializedViewLookback", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    If(Not(Token("with")), rules.MaterializedViewNameReference),
                    Token("lookback"),
                    Required(rules.Value, rules.MissingValue),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD("Lookback", CompletionHint.Literal)}));

            var AlterMaterializedViewOverMaterializedView2 = Command("AlterMaterializedViewOverMaterializedView", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    If(Not(Token("with")), rules.MaterializedViewNameReference),
                    Token("on"),
                    Token("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    Required(rules.FunctionBody, rules.MissingFunctionBody),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD(CompletionHint.MaterializedView), CD()}));

            var AlterMaterializedView2 = Command("AlterMaterializedView", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    If(Not(Token("with")), rules.MaterializedViewNameReference),
                    RequiredToken("on"),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Required(rules.FunctionBody, rules.MissingFunctionBody),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD(CompletionHint.Table), CD()}));

            var AlterMaterializedViewPolicyCaching = Command("AlterMaterializedViewPolicyCaching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    If(Not(Token("with")), rules.DatabaseMaterializedViewNameReference),
                    Token("policy"),
                    Token("caching"),
                    Required(
                        First(
                            fragment8,
                            fragment9),
                        missing16),
                    shape140));

            var AlterMaterializedViewPolicyPartitioning = Command("AlterMaterializedViewPolicyPartitioning", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    If(Not(Token("with")), rules.DatabaseMaterializedViewNameReference),
                    Token("policy"),
                    Token("partitioning"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape141));

            var AlterMaterializedViewPolicyRetention = Command("AlterMaterializedViewPolicyRetention", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    If(Not(Token("with")), rules.DatabaseMaterializedViewNameReference),
                    Token("policy"),
                    Token("retention"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD("RetentionPolicy", CompletionHint.Literal)}));

            var AlterMaterializedViewPolicyRowLevelSecurity = Command("AlterMaterializedViewPolicyRowLevelSecurity", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Required(If(Not(Token("with")), rules.DatabaseMaterializedViewNameReference), rules.MissingNameReference),
                    RequiredToken("policy"),
                    RequiredToken("row_level_security"),
                    RequiredToken("disable", "enable"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD(), CD("Query", CompletionHint.Literal)}));

            var AlterMirroringTemplate = Command("AlterMirroringTemplate", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("mirroring-template"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Optional(
                        fragment27),
                    Optional(
                        fragment28),
                    Optional(
                        fragment3),
                    shape146));

            var AlterPoliciesOfRetention = Command("AlterPoliciesOfRetention", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("policies"),
                    RequiredToken("of"),
                    RequiredToken("retention"),
                    Optional(Token("internal")),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD(isOptional: true), CD("policies", CompletionHint.Literal)}));

            var AlterTablesPolicyCaching = Command("AlterTablesPolicyCaching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.TableNameReference,
                            shape35),
                        fnMissingElement: rules.MissingNameReference),
                    Token(")"),
                    Token("policy"),
                    Token("caching"),
                    Required(
                        First(
                            fragment8,
                            fragment9),
                        missing16),
                    Optional(
                        fragment25),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(), CD(), CD(isOptional: true)}));

            var AlterTablesPolicyIngestionBatching = Command("AlterTablesPolicyIngestionBatching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.TableNameReference,
                            shape35),
                        fnMissingElement: rules.MissingNameReference),
                    Token(")"),
                    Token("policy"),
                    Token("ingestionbatching"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(), CD("IngestionBatchingPolicy", CompletionHint.Literal)}));

            var AlterTablesPolicyIngestionTime = Command("AlterTablesPolicyIngestionTime", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.TableNameReference,
                            shape35),
                        fnMissingElement: rules.MissingNameReference),
                    Token(")"),
                    Token("policy"),
                    Token("ingestiontime"),
                    RequiredToken("true"),
                    shape147));

            var AlterTablesPolicyMerge = Command("AlterTablesPolicyMerge", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.TableNameReference,
                            shape35),
                        fnMissingElement: rules.MissingNameReference),
                    Token(")"),
                    Token("policy"),
                    Token("merge"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(), CD("policy", CompletionHint.Literal)}));

            var AlterTablesPolicyRestrictedViewAccess = Command("AlterTablesPolicyRestrictedViewAccess", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.TableNameReference,
                            shape35),
                        fnMissingElement: rules.MissingNameReference),
                    Token(")"),
                    Token("policy"),
                    Token("restricted_view_access"),
                    RequiredToken("false", "true"),
                    shape147));

            var AlterTablesPolicyRetention = Command("AlterTablesPolicyRetention", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.TableNameReference,
                            shape35),
                        fnMissingElement: rules.MissingNameReference),
                    Token(")"),
                    Token("policy"),
                    Token("retention"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(), CD("RetentionPolicy", CompletionHint.Literal)}));

            var AlterTablesPolicyRowOrder = Command("AlterTablesPolicyRowOrder", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("tables"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.TableNameReference,
                                    shape35),
                                fnMissingElement: rules.MissingNameReference),
                            missing32),
                        RequiredToken(")"),
                        RequiredToken("policy"),
                        RequiredToken("roworder"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.NameDeclaration,
                                    RequiredToken("asc", "desc"),
                                    new [] {CD("ColumnName", CompletionHint.None), CD()})),
                            missing33),
                        RequiredToken(")")}
                    ,
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(), CD(), CD(CompletionHint.None), CD()}));

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
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD(), CD("newMethod", CompletionHint.Literal)}));

            var AlterTablePolicyAutoDelete = Command("AlterTablePolicyAutoDelete", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("auto_delete"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("AutoDeletePolicy", CompletionHint.Literal)}));

            var AlterTablePolicyCaching = Command("AlterTablePolicyCaching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("caching"),
                    Required(
                        First(
                            fragment8,
                            fragment9),
                        missing16),
                    shape148));

            var AlterTablePolicyEncoding = Command("AlterTablePolicyEncoding", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape149));

            var AlterTablePolicyExtentTagsRetention = Command("AlterTablePolicyExtentTagsRetention", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("extent_tags_retention"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("ExtentTagsRetentionPolicy", CompletionHint.Literal)}));

            var AlterTablePolicyIngestionBatching = Command("AlterTablePolicyIngestionBatching", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("ingestionbatching"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape150));

            var AlterTablePolicyIngestionTime = Command("AlterTablePolicyIngestionTime", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("ingestiontime"),
                    RequiredToken("true"),
                    shape148));

            var AlterTablePolicyMerge = Command("AlterTablePolicyMerge", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("merge"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape151));

            var AlterTablePolicyMirroring = Command("AlterTablePolicyMirroring", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("table"),
                        rules.DatabaseTableNameReference,
                        Token("policy"),
                        Token("mirroring"),
                        Optional(
                            fragment29),
                        RequiredToken("kind"),
                        RequiredToken("="),
                        RequiredToken("delta"),
                        Optional(
                            fragment4),
                        Optional(
                            fragment3)}
                    ,
                    shape154));

            var AlterTablePolicyPartitioning = Command("AlterTablePolicyPartitioning", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("partitioning"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape155));

            var AlterTablePolicyRestrictedViewAccess = Command("AlterTablePolicyRestrictedViewAccess", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("restricted_view_access"),
                    RequiredToken("false", "true"),
                    shape148));

            var AlterTablePolicyRetention = Command("AlterTablePolicyRetention", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("retention"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD("RetentionPolicy", CompletionHint.Literal)}));

            var AlterTablePolicyRowLevelSecurity = Command("AlterTablePolicyRowLevelSecurity", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("row_level_security"),
                    RequiredToken("disable", "enable"),
                    Optional(
                        fragment5),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD(isOptional: true), CD("Query", CompletionHint.Literal)}));

            var AlterTablePolicyRowOrder = Command("AlterTablePolicyRowOrder", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("roworder"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            fragment30),
                        missing10),
                    RequiredToken(")"),
                    shape158));

            var AlterTablePolicySharding = Command("AlterTablePolicySharding", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("sharding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape159));

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
                                Token("disable"),
                                shape71),
                            Custom(
                                Token("enable"),
                                shape71),
                            Custom(
                                rules.StringLiteral,
                                shape30)),
                        missing17),
                    shape148));

            var AlterTablePolicyUpdate = Command("AlterTablePolicyUpdate", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    RequiredToken("update"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        fragment7),
                    shape161));

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
                        fragment7),
                    shape162));

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
                        fragment7),
                    shape164));

            var AlterTableRowStoreReferencesDisableRowStore = Command("AlterTableRowStoreReferencesDisableRowStore", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("rowstore_references"),
                    Token("disable"),
                    RequiredToken("rowstore"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Optional(
                        fragment7),
                    shape166));

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
                        fragment7),
                    shape162));

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
                        fragment7),
                    shape164));

            var AlterTableRowStoreReferencesDropRowStore = Command("AlterTableRowStoreReferencesDropRowStore", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    RequiredToken("rowstore_references"),
                    RequiredToken("drop"),
                    RequiredToken("rowstore"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Optional(
                        fragment7),
                    shape166));

            var AlterTable = Command("AlterTable", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    fragment31,
                    Optional(
                        fragment32),
                    shape168));

            var AlterTableColumnDocStrings = Command("AlterTableColumnDocStrings", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("column-docstrings"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            fragment33),
                        missing14),
                    RequiredToken(")"),
                    shape171));

            var AlterTableColumnsPolicyEncoding = Command("AlterTableColumnsPolicyEncoding", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("columns"),
                    RequiredToken("policy"),
                    RequiredToken("encoding"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(), CD("EncodingPolicies", CompletionHint.Literal)}));

            var AlterTableColumnStatistics = Command("AlterTableColumnStatistics", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("column"),
                    RequiredToken("statistics"),
                    ZeroOrMoreCommaList(
                        Custom(
                            rules.NameDeclaration,
                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                            new [] {CD("c2", CompletionHint.None), CD("statisticsValues2", CompletionHint.Literal)})),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(CompletionHint.None)}));

            var AlterTableDocString = Command("AlterTableDocString", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("docstring"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("Documentation", CompletionHint.Literal)}));

            var AlterExtentTagsFromQuery2 = Command("AlterExtentTagsFromQuery", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("alter", CompletionKind.CommandPrefix),
                        Token("table"),
                        Required(rules.TableNameReference, rules.MissingNameReference),
                        RequiredToken("extent"),
                        RequiredToken("tags"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.StringLiteral,
                                    shape55),
                                fnMissingElement: rules.MissingStringLiteral),
                            missing0),
                        RequiredToken(")"),
                        Optional(
                            fragment5),
                        Required(
                            fragment6,
                            missing1)}
                    ,
                    shape172));

            var AlterTableFolder = Command("AlterTableFolder", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("folder"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("Folder", CompletionHint.Literal)}));

            var AlterTableIngestionMapping = Command("AlterTableIngestionMapping", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    RequiredToken("ingestion"),
                    RequiredToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        fragment7),
                    shape173));

            var AlterTablePolicyMirroringWithJson = Command("AlterTablePolicyMirroringWithJson", 
                Custom(
                    Token("alter", CompletionKind.CommandPrefix),
                    RequiredToken("table"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("policy"),
                    RequiredToken("mirroring"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape175));

            var AppendTable = Command("AppendTable", 
                Custom(
                    Token("append", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    Required(If(Not(Token("async")), rules.TableNameReference), rules.MissingNameReference),
                    Optional(
                        fragment47),
                    RequiredToken("<|"),
                    Required(rules.QueryInput, rules.MissingExpression),
                    new [] {CD(), CD(isOptional: true), CD("TableName", CompletionHint.Table), CD(isOptional: true), CD(), CD("QueryOrCommand", CompletionHint.Tabular)}));

            var ApplyTableMirroringPolicyTemplateToTables = Command("ApplyTableMirroringPolicyTemplateToTables", 
                Custom(
                    Token("apply", CompletionKind.CommandPrefix),
                    RequiredToken("table_mirroring_template"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Required(
                        First(
                            Custom(
                                Token("("),
                                Required(
                                    OneOrMoreCommaList(
                                        Custom(
                                            rules.TableNameReference,
                                            shape35),
                                        fnMissingElement: rules.MissingNameReference),
                                    missing32),
                                RequiredToken(")"),
                                new [] {CD(), CD(CompletionHint.Table), CD()}),
                            Custom(
                                fragment6,
                                shape49)),
                        missing36),
                    new [] {CD(), CD(), CD("TemplateName", CompletionHint.None), CD()}));

            var AttachExtentsIntoTableByMetadata = Command("AttachExtentsIntoTableByMetadata", 
                Custom(
                    Token("attach", CompletionKind.CommandPrefix),
                    Token("async"),
                    RequiredToken("extents"),
                    ZeroOrMoreList(
                        fragment34),
                    RequiredToken("by"),
                    RequiredToken("metadata"),
                    Required(
                        fragment6,
                        missing1),
                    new [] {CD(), CD(), CD(), CD(), CD(), CD(), CD("csl")}));

            var AttachDatabase = Command("AttachDatabase", 
                Custom(
                    Token("attach", CompletionKind.CommandPrefix),
                    RequiredToken("database"),
                    Optional(
                        Token("all", "metadata")).Hide(),
                    Required(If(Not(And(Token("all", "metadata"))), rules.DatabaseNameReference), rules.MissingNameReference),
                    RequiredToken("from"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        fragment35),
                    Optional(
                        fragment7),
                    new [] {CD(), CD(), CD(isOptional: true), CD("DatabaseName", CompletionHint.Database), CD(), CD("Path", CompletionHint.Literal), CD(isOptional: true), CD(isOptional: true)}));

            var AttachExtentsIntoTableByMetadata2 = Command("AttachExtentsIntoTableByMetadata", 
                Custom(
                    Token("attach", CompletionKind.CommandPrefix),
                    Token("extents"),
                    RequiredToken("by"),
                    RequiredToken("metadata"),
                    Required(
                        fragment6,
                        missing1),
                    new [] {CD(), CD(), CD(), CD(), CD("csl")}));

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
                        OneOrMoreList(
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                shape319)),
                        missing37),
                    new [] {CD(), CD(), CD(), CD(), CD("tableName", CompletionHint.Table), CD(), CD(), CD("containerUri", CompletionHint.Literal), CD(CompletionHint.Literal)}));

            var AttachExtentsIntoTableByMetadata3 = Command("AttachExtentsIntoTableByMetadata", 
                Custom(
                    Token("attach", CompletionKind.CommandPrefix),
                    Token("extents"),
                    RequiredToken("into"),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    ZeroOrMoreList(
                        fragment34),
                    RequiredToken("by"),
                    RequiredToken("metadata"),
                    Required(
                        fragment6,
                        missing1),
                    new [] {CD(), CD(), CD(), CD(), CD("tableName", CompletionHint.Table), CD(), CD(), CD(), CD("csl")}));

            var AttachDatabase2 = Command("AttachDatabase", 
                Custom(
                    Token("attach", CompletionKind.CommandPrefix),
                    RequiredToken("from"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        fragment35),
                    Optional(
                        fragment7),
                    new [] {CD(), CD(), CD("Path", CompletionHint.Literal), CD(isOptional: true), CD(isOptional: true)}));

            var CancelOperation = Command("CancelOperation", 
                Custom(
                    Token("cancel", CompletionKind.CommandPrefix),
                    Token("operation"),
                    Required(rules.AnyGuidLiteralOrString, rules.MissingValue),
                    Optional(
                        fragment7),
                    new [] {CD(), CD(), CD("obj", CompletionHint.Literal), CD(isOptional: true)}));

            var CancelQuery = Command("CancelQuery", 
                Custom(
                    Token("cancel", CompletionKind.CommandPrefix),
                    RequiredToken("query"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("ClientRequestId", CompletionHint.Literal)}));

            var CleanDatabaseExtentContainers = Command("CleanDatabaseExtentContainers", 
                Custom(
                    Token("clean", CompletionKind.CommandPrefix),
                    RequiredToken("databases"),
                    Optional(Token("async")),
                    Optional(
                        fragment68),
                    RequiredToken("extentcontainers"),
                    new [] {CD(), CD(), CD(isOptional: true), CD(isOptional: true), CD()}));

            var ClearTableData = Command("ClearTableData", 
                Custom(
                    Token("clear", CompletionKind.CommandPrefix),
                    RequiredToken("async"),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredToken("data"),
                    new [] {CD(), CD(), CD(), CD("TableName", CompletionHint.Table), CD()}));

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
                    new [] {CD(), CD(), CD(), CD(), CD(), CD("clusterName", CompletionHint.Literal), CD(), CD(), CD(), CD(), CD("databaseName", CompletionHint.Literal), CD()}));

            var ClearClusterCredStoreCache = Command("ClearClusterCredStoreCache", 
                Custom(
                    Token("clear", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("cache"),
                    Token("credstore")));

            var ClearExternalArtifactsCache = Command("ClearExternalArtifactsCache", 
                Custom(
                    Token("clear", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("cache"),
                    Token("external-artifacts"),
                    Required(
                        Custom(
                            Token("("),
                            Required(
                                OneOrMoreCommaList(
                                    Custom(
                                        rules.StringLiteral,
                                        CD("ArtifactUri", CompletionHint.Literal)),
                                    fnMissingElement: rules.MissingStringLiteral),
                                missing0),
                            RequiredToken(")"),
                            shape44),
                        missing3)));

            var ClearClusterGroupMembershipCache = Command("ClearClusterGroupMembershipCache", 
                Custom(
                    Token("clear", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("cache"),
                    RequiredToken("groupmembership"),
                    Optional(
                        fragment5),
                    shape264));

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
                    shape309));

            var ClearMaterializedViewStatistics = Command("ClearMaterializedViewStatistics", 
                Custom(
                    Token("clear", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    RequiredToken("statistics"),
                    shape311));

            var ClearTableCacheStreamingIngestionSchema = Command("ClearTableCacheStreamingIngestionSchema", 
                Custom(
                    Token("clear", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("cache"),
                    RequiredToken("streamingingestion"),
                    RequiredToken("schema"),
                    shape148));

            var ClearTableData2 = Command("ClearTableData", 
                Custom(
                    Token("clear", CompletionKind.CommandPrefix),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredToken("data"),
                    shape239));

            var CreateMergeTables = Command("CreateMergeTables", 
                Custom(
                    Token("create-merge", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Required(
                        OneOrMoreCommaList(
                            fragment39),
                        missing38),
                    Optional(
                        fragment7),
                    shape200));

            var CreateMergeTable = Command("CreateMergeTable", 
                Custom(
                    Token("create-merge", CompletionKind.CommandPrefix),
                    RequiredToken("table"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Required(
                        fragment31,
                        missing34),
                    Optional(
                        fragment32),
                    shape201));

            var CreateOrAlterContinuousExport = Command("CreateOrAlterContinuousExport", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("create-or-alter", CompletionKind.CommandPrefix),
                        Token("continuous-export"),
                        Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                        Optional(
                            Custom(
                                Token("over"),
                                RequiredToken("("),
                                Required(
                                    OneOrMoreCommaList(
                                        Custom(
                                            rules.NameDeclaration,
                                            shape182),
                                        fnMissingElement: rules.MissingNameDeclaration),
                                    missing28),
                                RequiredToken(")"),
                                shape25)),
                        RequiredToken("to"),
                        RequiredToken("table"),
                        Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                        Optional(
                            fragment7),
                        RequiredToken("<|"),
                        Required(rules.QueryInput, rules.MissingExpression)}
                    ,
                    new [] {CD(), CD(), CD("ContinuousExportName", CompletionHint.None), CD(isOptional: true), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(isOptional: true), CD(), CD("Query", CompletionHint.Tabular)}));

            var CreateOrAlterDatabaseIngestionMapping = Command("CreateOrAlterDatabaseIngestionMapping", 
                Custom(
                    Token("create-or-alter", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    RequiredToken("ingestion"),
                    RequiredToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape193));

            var CreateOrAlterEntityGroupCommand = Command("CreateOrAlterEntityGroupCommand", 
                Custom(
                    Token("create-or-alter", CompletionKind.CommandPrefix),
                    Token("entity_group"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            First(
                                fragment10,
                                fragment11)),
                        missing5),
                    RequiredToken(")"),
                    new [] {CD(), CD(), CD("EntityGroupName", CompletionHint.None), CD(), CD(), CD()}));

            var CreateOrAlterStorageExternalTable = Command("CreateOrAlterStorageExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("create-or-alter", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.NameDeclaration,
                        Token("("),
                        Required(
                            OneOrMoreCommaList(
                                fragment12),
                            missing12),
                        RequiredToken(")"),
                        RequiredToken("kind"),
                        RequiredToken("="),
                        RequiredToken("adl").Hide(),
                        Optional(
                            fragment19),
                        Optional(
                            fragment20),
                        RequiredToken("dataformat"),
                        RequiredToken("="),
                        RequiredToken("apacheavro", "avro", "csv", "json", "multijson", "orc", "parquet", "psv", "raw", "scsv", "sohsv", "sstream", "tsve", "tsv", "txt", "w3clogfile"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.StringLiteral,
                                    shape43),
                                fnMissingElement: rules.MissingStringLiteral),
                            missing0),
                        RequiredToken(")"),
                        Optional(
                            fragment3)}
                    ,
                    shape108));

            var CreateOrAlterStorageExternalTable2 = Command("CreateOrAlterStorageExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("create-or-alter", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.NameDeclaration,
                        Token("("),
                        Required(
                            OneOrMoreCommaList(
                                fragment12),
                            missing12),
                        RequiredToken(")"),
                        RequiredToken("kind"),
                        RequiredToken("="),
                        RequiredToken("blob").Hide(),
                        Optional(
                            fragment19),
                        Optional(
                            fragment20),
                        RequiredToken("dataformat"),
                        RequiredToken("="),
                        RequiredToken("apacheavro", "avro", "csv", "json", "multijson", "orc", "parquet", "psv", "raw", "scsv", "sohsv", "sstream", "tsve", "tsv", "txt", "w3clogfile"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.StringLiteral,
                                    shape43),
                                fnMissingElement: rules.MissingStringLiteral),
                            missing0),
                        RequiredToken(")"),
                        Optional(
                            fragment3)}
                    ,
                    shape108));

            var CreateOrAlterStorageExternalTable3 = Command("CreateOrAlterStorageExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("create-or-alter", CompletionKind.CommandPrefix),
                        Token("external"),
                        RequiredToken("table"),
                        Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                fragment12),
                            missing12),
                        RequiredToken(")"),
                        RequiredToken("kind"),
                        RequiredToken("="),
                        RequiredToken("delta"),
                        RequiredToken("("),
                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                        RequiredToken(")"),
                        Optional(
                            fragment3)}
                    ,
                    shape184));

            var CreateOrAlterSqlExternalTable = Command("CreateOrAlterSqlExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("create-or-alter", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.NameDeclaration,
                        Token("("),
                        OneOrMoreCommaList(
                            fragment12),
                        Token(")"),
                        Token("kind"),
                        RequiredToken("="),
                        RequiredToken("sql"),
                        Optional(
                            fragment36),
                        RequiredToken("("),
                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                        RequiredToken(")"),
                        Optional(
                            fragment3)}
                    ,
                    shape186));

            var CreateOrAlterStorageExternalTable4 = Command("CreateOrAlterStorageExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("create-or-alter", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.NameDeclaration,
                        Token("("),
                        Required(
                            OneOrMoreCommaList(
                                fragment12),
                            missing12),
                        RequiredToken(")"),
                        RequiredToken("kind"),
                        RequiredToken("="),
                        RequiredToken("storage"),
                        Optional(
                            fragment19),
                        Optional(
                            fragment20),
                        RequiredToken("dataformat"),
                        RequiredToken("="),
                        RequiredToken("apacheavro", "avro", "csv", "json", "multijson", "orc", "parquet", "psv", "raw", "scsv", "sohsv", "sstream", "tsve", "tsv", "txt", "w3clogfile"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.StringLiteral,
                                    shape43),
                                fnMissingElement: rules.MissingStringLiteral),
                            missing0),
                        RequiredToken(")"),
                        Optional(
                            fragment3)}
                    ,
                    shape108));

            var CreateOrAlterStorageExternalTable5 = Command("CreateOrAlterStorageExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("create-or-alter", CompletionKind.CommandPrefix),
                        Token("external"),
                        RequiredToken("table"),
                        Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                        RequiredToken("kind"),
                        RequiredToken("="),
                        RequiredToken("delta"),
                        RequiredToken("("),
                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                        RequiredToken(")"),
                        Optional(
                            fragment3)}
                    ,
                    shape187));

            var CreateOrAlterFunction = Command("CreateOrAlterFunction", 
                Custom(
                    Token("create-or-alter", CompletionKind.CommandPrefix),
                    Token("function"),
                    Optional(
                        fragment3),
                    Required(If(Not(Token("with")), rules.NameDeclaration), rules.MissingNameDeclaration),
                    Required(rules.FunctionDeclaration, rules.MissingFunctionDeclaration),
                    new [] {CD(), CD(), CD(isOptional: true), CD("FunctionName", CompletionHint.None), CD()}));

            var GraphModelCreateOrAlter = Command("GraphModelCreateOrAlter", 
                Custom(
                    Token("create-or-alter", CompletionKind.CommandPrefix),
                    Token("graph_model").Hide(),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(CompletionHint.None), CD(CompletionHint.Literal)}));

            var CreateOrAlterMaterializedViewOverMaterializedView = Command("CreateOrAlterMaterializedViewOverMaterializedView", 
                Custom(
                    Token("create-or-alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Optional(
                        fragment37),
                    If(Not(Token("with")), rules.MaterializedViewNameReference),
                    Token("on"),
                    Token("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    Required(rules.FunctionBody, rules.MissingFunctionBody),
                    new [] {CD(), CD(), CD(isOptional: true), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD(CompletionHint.MaterializedView), CD()}));

            var CreateOrAlterMaterializedView = Command("CreateOrAlterMaterializedView", 
                Custom(
                    Token("create-or-alter", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Optional(
                        fragment37),
                    Required(If(Not(Token("with")), rules.MaterializedViewNameReference), rules.MissingNameReference),
                    RequiredToken("on"),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Required(rules.FunctionBody, rules.MissingFunctionBody),
                    new [] {CD(), CD(), CD(isOptional: true), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD(CompletionHint.Table), CD()}));

            var CreateOrAlterTableIngestionMapping = Command("CreateOrAlterTableIngestionMapping", 
                Custom(
                    Token("create-or-alter", CompletionKind.CommandPrefix),
                    Token("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredToken("ingestion"),
                    RequiredToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        fragment7),
                    shape173));

            var CreateOrAleterWorkloadGroup = Command("CreateOrAleterWorkloadGroup", 
                Custom(
                    Token("create-or-alter", CompletionKind.CommandPrefix),
                    RequiredToken("workload_group"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape190));

            var CreateMaterializedViewOverMaterializedView = Command("CreateMaterializedViewOverMaterializedView", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("create", CompletionKind.CommandPrefix),
                        Token("async"),
                        Optional(Token("ifnotexists")),
                        Token("materialized-view"),
                        Optional(
                            fragment37),
                        If(Not(Token("with")), rules.NameDeclaration),
                        Token("on"),
                        Token("materialized-view"),
                        Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                        Required(rules.FunctionBody, rules.MissingFunctionBody)}
                    ,
                    new [] {CD(), CD(), CD(isOptional: true), CD(), CD(isOptional: true), CD("MaterializedViewName", CompletionHint.None), CD(), CD(), CD(CompletionHint.MaterializedView), CD()}));

            var CreateMaterializedView = Command("CreateMaterializedView", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("create", CompletionKind.CommandPrefix),
                        Token("async"),
                        Optional(Token("ifnotexists")),
                        RequiredToken("materialized-view"),
                        Optional(
                            fragment37),
                        Required(If(Not(Token("with")), rules.NameDeclaration), rules.MissingNameDeclaration),
                        RequiredToken("on"),
                        RequiredToken("table"),
                        Required(rules.TableNameReference, rules.MissingNameReference),
                        Required(rules.FunctionBody, rules.MissingFunctionBody)}
                    ,
                    new [] {CD(), CD(), CD(isOptional: true), CD(), CD(isOptional: true), CD("MaterializedViewName", CompletionHint.None), CD(), CD(), CD(CompletionHint.Table), CD()}));

            var CreateDatabase = Command("CreateDatabase", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    RequiredToken("ifnotexists"),
                    Optional(
                        fragment7),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.None), CD("IfNotExists"), CD(isOptional: true)}));

            var CreateDatabaseIngestionMapping = Command("CreateDatabaseIngestionMapping", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.NameDeclaration,
                    Token("ingestion"),
                    RequiredToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape193));

            var CreateDatabase2 = Command("CreateDatabase", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    RequiredToken("persist"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape177),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("ifnotexists"),
                            shape194)),
                    Optional(
                        fragment7),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.None), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true), CD(isOptional: true)}));

            var CreateDatabase3 = Command("CreateDatabase", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    RequiredToken("volatile"),
                    Optional(
                        Custom(
                            Token("ifnotexists"),
                            shape194)),
                    Optional(
                        fragment7),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.None), CD(), CD(isOptional: true), CD(isOptional: true)}));

            var CreateDatabase4 = Command("CreateDatabase", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    RequiredToken("with"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            fragment38),
                        missing11),
                    RequiredToken(")"),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.None), CD(), CD(), CD(CompletionHint.None), CD()}));

            var CreateDatabase5 = Command("CreateDatabase", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.None)}));

            var CreateEntityGroupCommand = Command("CreateEntityGroupCommand", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("entity_group"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Optional(Token("ifnotexists")),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            First(
                                Custom(
                                    Token("cluster"),
                                    Token("("),
                                    rules.StringLiteral,
                                    Token(")"),
                                    Token("."),
                                    RequiredToken("database"),
                                    RequiredToken("("),
                                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                                    RequiredToken(")"),
                                    shape87),
                                Custom(
                                    Token("cluster"),
                                    RequiredToken("("),
                                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                                    RequiredToken(")"),
                                    new [] {CD(), CD(), CD("clusterName", CompletionHint.Literal), CD()}),
                                fragment11)),
                        missing5),
                    RequiredToken(")"),
                    new [] {CD(), CD(), CD("EntityGroupName", CompletionHint.None), CD(isOptional: true), CD(), CD(), CD()}));

            var CreateStorageExternalTable = Command("CreateStorageExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("create", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.NameDeclaration,
                        Token("("),
                        Required(
                            OneOrMoreCommaList(
                                fragment12),
                            missing12),
                        RequiredToken(")"),
                        RequiredToken("kind"),
                        RequiredToken("="),
                        RequiredToken("adl").Hide(),
                        Optional(
                            fragment19),
                        Optional(
                            fragment20),
                        RequiredToken("dataformat"),
                        RequiredToken("="),
                        RequiredToken("apacheavro", "avro", "csv", "json", "multijson", "orc", "parquet", "psv", "raw", "scsv", "sohsv", "sstream", "tsve", "tsv", "txt", "w3clogfile"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.StringLiteral,
                                    shape43),
                                fnMissingElement: rules.MissingStringLiteral),
                            missing0),
                        RequiredToken(")"),
                        Optional(
                            fragment3)}
                    ,
                    shape108));

            var CreateStorageExternalTable2 = Command("CreateStorageExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("create", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.NameDeclaration,
                        Token("("),
                        Required(
                            OneOrMoreCommaList(
                                fragment12),
                            missing12),
                        RequiredToken(")"),
                        RequiredToken("kind"),
                        RequiredToken("="),
                        RequiredToken("blob").Hide(),
                        Optional(
                            fragment19),
                        Optional(
                            fragment20),
                        RequiredToken("dataformat"),
                        RequiredToken("="),
                        RequiredToken("apacheavro", "avro", "csv", "json", "multijson", "orc", "parquet", "psv", "raw", "scsv", "sohsv", "sstream", "tsve", "tsv", "txt", "w3clogfile"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.StringLiteral,
                                    shape43),
                                fnMissingElement: rules.MissingStringLiteral),
                            missing0),
                        RequiredToken(")"),
                        Optional(
                            fragment3)}
                    ,
                    shape108));

            var CreateStorageExternalTable3 = Command("CreateStorageExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("create", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.NameDeclaration,
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                fragment12),
                            missing12),
                        RequiredToken(")"),
                        RequiredToken("kind"),
                        RequiredToken("="),
                        RequiredToken("delta"),
                        RequiredToken("("),
                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                        RequiredToken(")"),
                        Optional(
                            fragment3)}
                    ,
                    shape184));

            var CreateSqlExternalTable = Command("CreateSqlExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("create", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.NameDeclaration,
                        Token("("),
                        OneOrMoreCommaList(
                            fragment12),
                        Token(")"),
                        Token("kind"),
                        RequiredToken("="),
                        RequiredToken("sql"),
                        Optional(
                            fragment36),
                        RequiredToken("("),
                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                        RequiredToken(")"),
                        Optional(
                            fragment3)}
                    ,
                    shape186));

            var CreateStorageExternalTable4 = Command("CreateStorageExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("create", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.NameDeclaration,
                        Token("("),
                        Required(
                            OneOrMoreCommaList(
                                fragment12),
                            missing12),
                        RequiredToken(")"),
                        RequiredToken("kind"),
                        RequiredToken("="),
                        RequiredToken("storage"),
                        Optional(
                            fragment19),
                        Optional(
                            fragment20),
                        RequiredToken("dataformat"),
                        RequiredToken("="),
                        RequiredToken("apacheavro", "avro", "csv", "json", "multijson", "orc", "parquet", "psv", "raw", "scsv", "sohsv", "sstream", "tsve", "tsv", "txt", "w3clogfile"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.StringLiteral,
                                    shape43),
                                fnMissingElement: rules.MissingStringLiteral),
                            missing0),
                        RequiredToken(")"),
                        Optional(
                            fragment3)}
                    ,
                    shape108));

            var CreateStorageExternalTable5 = Command("CreateStorageExternalTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("create", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.NameDeclaration,
                        RequiredToken("kind"),
                        RequiredToken("="),
                        RequiredToken("delta"),
                        RequiredToken("("),
                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                        RequiredToken(")"),
                        Optional(
                            fragment3)}
                    ,
                    shape187));

            var CreateExternalTableMapping = Command("CreateExternalTableMapping", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("external"),
                    RequiredToken("table"),
                    Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                    RequiredToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape196));

            var CreateFunction = Command("CreateFunction", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("function"),
                    Optional(Token("ifnotexists")),
                    Optional(
                        fragment5),
                    Required(If(Not(And(Token("ifnotexists", "with"))), rules.NameDeclaration), rules.MissingNameDeclaration),
                    Required(rules.FunctionDeclaration, rules.MissingFunctionDeclaration),
                    new [] {CD(), CD(), CD(isOptional: true), CD(isOptional: true), CD("FunctionName", CompletionHint.None), CD()}));

            var CreateMaterializedViewOverMaterializedView2 = Command("CreateMaterializedViewOverMaterializedView", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("ifnotexists"),
                    Token("materialized-view"),
                    Optional(
                        fragment37),
                    If(Not(Token("with")), rules.NameDeclaration),
                    Token("on"),
                    Token("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    Required(rules.FunctionBody, rules.MissingFunctionBody),
                    new [] {CD(), CD(), CD(), CD(isOptional: true), CD("MaterializedViewName", CompletionHint.None), CD(), CD(), CD(CompletionHint.MaterializedView), CD()}));

            var CreateMaterializedView2 = Command("CreateMaterializedView", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("ifnotexists"),
                    RequiredToken("materialized-view"),
                    Optional(
                        fragment37),
                    Required(If(Not(Token("with")), rules.NameDeclaration), rules.MissingNameDeclaration),
                    RequiredToken("on"),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Required(rules.FunctionBody, rules.MissingFunctionBody),
                    new [] {CD(), CD(), CD(), CD(isOptional: true), CD("MaterializedViewName", CompletionHint.None), CD(), CD(), CD(CompletionHint.Table), CD()}));

            var CreateMaterializedViewOverMaterializedView3 = Command("CreateMaterializedViewOverMaterializedView", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Optional(
                        fragment37),
                    If(Not(Token("with")), rules.NameDeclaration),
                    Token("on"),
                    Token("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    Required(rules.FunctionBody, rules.MissingFunctionBody),
                    new [] {CD(), CD(), CD(isOptional: true), CD("MaterializedViewName", CompletionHint.None), CD(), CD(), CD(CompletionHint.MaterializedView), CD()}));

            var CreateMaterializedView3 = Command("CreateMaterializedView", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Optional(
                        fragment37),
                    Required(If(Not(Token("with")), rules.NameDeclaration), rules.MissingNameDeclaration),
                    RequiredToken("on"),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Required(rules.FunctionBody, rules.MissingFunctionBody),
                    new [] {CD(), CD(), CD(isOptional: true), CD("MaterializedViewName", CompletionHint.None), CD(), CD(), CD(CompletionHint.Table), CD()}));

            var CreateMirroringTemplate = Command("CreateMirroringTemplate", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("mirroring-template"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Optional(
                        fragment27),
                    Optional(
                        fragment28),
                    Optional(
                        fragment3),
                    shape146));

            var CreateRequestSupport = Command("CreateRequestSupport", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("request_support"),
                    Optional(
                        fragment7),
                    shape198));

            var CreateRowStore = Command("CreateRowStore", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("rowstore"),
                    Optional(
                        fragment7),
                    shape198));

            var CreateTables = Command("CreateTables", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Required(
                        OneOrMoreCommaList(
                            fragment39),
                        missing38),
                    Optional(
                        fragment7),
                    shape200));

            var CreateTable = Command("CreateTable", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.NameDeclaration,
                    fragment31,
                    Optional(
                        fragment32),
                    shape201));

            var CreateTableBasedOnAnother = Command("CreateTableBasedOnAnother", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.NameDeclaration,
                    Token("based-on"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Optional(Token("ifnotexists")),
                    Optional(
                        fragment32),
                    new [] {CD(), CD(), CD("NewTableName", CompletionHint.None), CD(), CD("TableName", CompletionHint.None), CD(isOptional: true), CD(isOptional: true)}));

            var CreateTableIngestionMapping = Command("CreateTableIngestionMapping", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    Token("table"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    RequiredToken("ingestion"),
                    RequiredToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        fragment7),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.None), CD(), CD("MappingKind"), CD(), CD("MappingName", CompletionHint.Literal), CD("MappingFormat", CompletionHint.Literal), CD(isOptional: true)}));

            var CreateTempStorage = Command("CreateTempStorage", 
                Custom(
                    Token("create", CompletionKind.CommandPrefix),
                    RequiredToken("tempstorage")));

            var DefineTables = Command("DefineTables", 
                Custom(
                    Token("define", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Required(
                        OneOrMoreCommaList(
                            fragment39),
                        missing38),
                    Optional(
                        fragment7),
                    shape200));

            var DefineTable = Command("DefineTable", 
                Custom(
                    Token("define", CompletionKind.CommandPrefix),
                    RequiredToken("table"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Required(
                        fragment31,
                        missing34),
                    Optional(
                        fragment32),
                    shape201));

            var DeleteMaterializedViewRecords = Command("DeleteMaterializedViewRecords", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("async"),
                    Token("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    RequiredToken("records"),
                    Optional(
                        fragment7),
                    Required(
                        fragment6,
                        missing1),
                    new [] {CD(), CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(isOptional: true), CD("csl")}));

            var DeleteTableRecords = Command("DeleteTableRecords", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    RequiredToken("async"),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredToken("records"),
                    Optional(
                        fragment7),
                    Required(
                        fragment6,
                        missing1),
                    new [] {CD(), CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(isOptional: true), CD("csl")}));

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
                    shape202));

            var DeleteColumnPolicyEncoding = Command("DeleteColumnPolicyEncoding", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("column"),
                    Required(rules.TableColumnNameReference, rules.MissingNameReference),
                    RequiredToken("policy"),
                    RequiredToken("encoding"),
                    shape202));

            var DeleteDatabasePolicyCaching = Command("DeleteDatabasePolicyCaching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("caching"),
                    shape203));

            var DeleteDatabasePolicyDiagnostics = Command("DeleteDatabasePolicyDiagnostics", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("diagnostics"),
                    shape203));

            var DeleteDatabasePolicyEncoding = Command("DeleteDatabasePolicyEncoding", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("encoding"),
                    shape203));

            var DeleteDatabasePolicyExtentTagsRetention = Command("DeleteDatabasePolicyExtentTagsRetention", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("extent_tags_retention"),
                    shape203));

            var DeleteDatabasePolicyIngestionBatching = Command("DeleteDatabasePolicyIngestionBatching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("ingestionbatching"),
                    shape203));

            var DeleteDatabasePolicyManagedIdentity = Command("DeleteDatabasePolicyManagedIdentity", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("managed_identity"),
                    shape203));

            var DeleteDatabasePolicyMerge = Command("DeleteDatabasePolicyMerge", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("merge"),
                    shape203));

            var DeleteDatabasePolicyMirroring = Command("DeleteDatabasePolicyMirroring", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("mirroring"),
                    shape203));

            var DeleteDatabasePolicyRetention = Command("DeleteDatabasePolicyRetention", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("retention"),
                    shape203));

            var DeleteDatabasePolicySharding = Command("DeleteDatabasePolicySharding", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("sharding"),
                    shape203));

            var DeleteDatabasePolicyShardsGrouping = Command("DeleteDatabasePolicyShardsGrouping", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("shards_grouping").Hide(),
                    shape203));

            var DeleteDatabasePolicyStreamingIngestion = Command("DeleteDatabasePolicyStreamingIngestion", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    Token("streamingingestion"),
                    shape203));

            var DropDatabaseTableMirroringPolicyTemplates = Command("DropDatabaseTableMirroringPolicyTemplates", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("policy"),
                    RequiredToken("table_mirroring_templates"),
                    shape203));

            var DeleteExternalTablePolicyQueryAcceleration = Command("DeleteExternalTablePolicyQueryAcceleration", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("external"),
                    RequiredToken("table"),
                    Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                    RequiredToken("policy"),
                    RequiredToken("query_acceleration"),
                    shape302));

            var DropFollowerTablesPolicyCaching = Command("DropFollowerTablesPolicyCaching", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("delete", CompletionKind.CommandPrefix),
                        Token("follower"),
                        RequiredToken("database"),
                        Required(rules.DatabaseNameReference, rules.MissingNameReference),
                        RequiredToken("materialized-views"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.NameDeclaration,
                                    shape133),
                                fnMissingElement: rules.MissingNameDeclaration),
                            missing28),
                        RequiredToken(")"),
                        RequiredToken("policy"),
                        RequiredToken("caching")}
                    ,
                    shape205));

            var DropFollowerTablesPolicyCaching2 = Command("DropFollowerTablesPolicyCaching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("follower"),
                    RequiredToken("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    RequiredToken("policy"),
                    RequiredToken("caching"),
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(), CD("name", CompletionHint.MaterializedView), CD(), CD()}));

            var DropFollowerDatabasePolicyCaching = Command("DropFollowerDatabasePolicyCaching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("follower"),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("policy"),
                    RequiredToken("caching"),
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(), CD()}));

            var DropFollowerTablesPolicyCaching3 = Command("DropFollowerTablesPolicyCaching", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("delete", CompletionKind.CommandPrefix),
                        Token("follower"),
                        RequiredToken("database"),
                        Required(rules.DatabaseNameReference, rules.MissingNameReference),
                        RequiredToken("tables"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.NameDeclaration,
                                    shape133),
                                fnMissingElement: rules.MissingNameDeclaration),
                            missing28),
                        RequiredToken(")"),
                        RequiredToken("policy"),
                        RequiredToken("caching")}
                    ,
                    shape205));

            var DropFollowerTablesPolicyCaching4 = Command("DropFollowerTablesPolicyCaching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("follower"),
                    RequiredToken("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredToken("policy"),
                    RequiredToken("caching"),
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD(), CD("name", CompletionHint.Table), CD(), CD()}));

            var DeleteMaterializedViewRecords2 = Command("DeleteMaterializedViewRecords", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    RequiredToken("records"),
                    Optional(
                        fragment7),
                    Required(
                        fragment6,
                        missing1),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(isOptional: true), CD("csl")}));

            var DeleteMaterializedViewPolicyCaching = Command("DeleteMaterializedViewPolicyCaching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.DatabaseMaterializedViewNameReference,
                    Token("policy"),
                    Token("caching"),
                    shape207));

            var DeleteMaterializedViewPolicyPartitioning = Command("DeleteMaterializedViewPolicyPartitioning", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.DatabaseMaterializedViewNameReference,
                    Token("policy"),
                    Token("partitioning"),
                    shape207));

            var DeleteMaterializedViewPolicyRowLevelSecurity = Command("DeleteMaterializedViewPolicyRowLevelSecurity", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Required(rules.DatabaseMaterializedViewNameReference, rules.MissingNameReference),
                    RequiredToken("policy"),
                    RequiredToken("row_level_security"),
                    shape207));

            var DeleteMirroringTemplate = Command("DeleteMirroringTemplate", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("mirroring-template"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    shape312));

            var DeletePoliciesOfRetention = Command("DeletePoliciesOfRetention", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("policies"),
                    RequiredToken("of"),
                    RequiredToken("retention"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                CD("entity", CompletionHint.Literal)),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    new [] {CD(), CD(), CD(), CD(), CD(), CD(CompletionHint.Literal), CD()}));

            var DeleteTablePolicyAutoDelete = Command("DeleteTablePolicyAutoDelete", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("auto_delete"),
                    shape208));

            var DeleteTablePolicyCaching = Command("DeleteTablePolicyCaching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("caching"),
                    shape208));

            var DeleteTablePolicyEncoding = Command("DeleteTablePolicyEncoding", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("encoding"),
                    shape208));

            var DeleteTablePolicyExtentTagsRetention = Command("DeleteTablePolicyExtentTagsRetention", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("extent_tags_retention"),
                    shape208));

            var DeleteTablePolicyIngestionBatching = Command("DeleteTablePolicyIngestionBatching", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("ingestionbatching"),
                    shape208));

            var DeleteTablePolicyIngestionTime = Command("DeleteTablePolicyIngestionTime", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("ingestiontime"),
                    shape208));

            var DeleteTablePolicyMerge = Command("DeleteTablePolicyMerge", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("merge"),
                    shape208));

            var DeleteTablePolicyMirroring = Command("DeleteTablePolicyMirroring", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("mirroring"),
                    shape208));

            var DeleteTablePolicyPartitioning = Command("DeleteTablePolicyPartitioning", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("partitioning"),
                    shape208));

            var DeleteTablePolicyRestrictedViewAccess = Command("DeleteTablePolicyRestrictedViewAccess", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("restricted_view_access"),
                    shape208));

            var DeleteTablePolicyRetention = Command("DeleteTablePolicyRetention", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("retention"),
                    shape208));

            var DeleteTablePolicyRowLevelSecurity = Command("DeleteTablePolicyRowLevelSecurity", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("row_level_security"),
                    shape208));

            var DeleteTablePolicyRowOrder = Command("DeleteTablePolicyRowOrder", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("roworder"),
                    shape208));

            var DeleteTablePolicySharding = Command("DeleteTablePolicySharding", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("sharding"),
                    shape208));

            var DeleteTablePolicyStreamingIngestion = Command("DeleteTablePolicyStreamingIngestion", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    Token("policy"),
                    Token("streamingingestion"),
                    shape208));

            var DeleteTablePolicyUpdate = Command("DeleteTablePolicyUpdate", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    RequiredToken("policy"),
                    RequiredToken("update"),
                    shape208));

            var DeleteTableRecords2 = Command("DeleteTableRecords", 
                Custom(
                    Token("delete", CompletionKind.CommandPrefix),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredToken("records"),
                    Optional(
                        fragment7),
                    Required(
                        fragment6,
                        missing1),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(isOptional: true), CD("csl")}));

            var DetachDatabase = Command("DetachDatabase", 
                Custom(
                    Token("detach", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    Optional(
                        Custom(
                            Token("ifexists"),
                            shape215)),
                    Optional(
                        Custom(
                            Token("skip-seal"),
                            shape217)),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(isOptional: true), CD(isOptional: true)}));

            var DropRowStore = Command("DropRowStore", 
                Custom(
                    Token("detach", CompletionKind.CommandPrefix),
                    RequiredToken("rowstore"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Optional(Token("ifexists")),
                    shape236));

            var DisableContinuousExport = Command("DisableContinuousExport", 
                Custom(
                    Token("disable", CompletionKind.CommandPrefix),
                    Token("continuous-export"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    new [] {CD(), CD(), CD("ContinousExportName", CompletionHint.None)}));

            var DisableDatabaseStreamingIngestionMaintenanceMode = Command("DisableDatabaseStreamingIngestionMaintenanceMode", 
                Custom(
                    Token("disable", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("streamingingestion_maintenance_mode")));

            var DisableDatabaseMaintenanceMode = Command("DisableDatabaseMaintenanceMode", 
                Custom(
                    Token("disable", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(If(Not(Token("streamingingestion_maintenance_mode")), rules.DatabaseNameReference), rules.MissingNameReference),
                    RequiredToken("maintenance_mode"),
                    shape216));

            var EnableDisableMaterializedView = Command("EnableDisableMaterializedView", 
                Custom(
                    Token("disable", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    shape235));

            var DisablePlugin = Command("DisablePlugin", 
                Custom(
                    Token("disable", CompletionKind.CommandPrefix),
                    RequiredToken("plugin"),
                    Required(
                        First(
                            rules.StringLiteral,
                            rules.NameDeclaration),
                        rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("pluginName", CompletionHint.Literal)}));

            var DropPretendExtentsByProperties = Command("DropPretendExtentsByProperties", 
                Custom(
                    Token("drop-pretend", CompletionKind.CommandPrefix),
                    RequiredToken("extents"),
                    Optional(
                        Custom(
                            Token("older"),
                            Required(rules.Value, rules.MissingValue),
                            RequiredToken("days", "hours"),
                            new [] {CD(), CD("Older", CompletionHint.Literal), CD()})),
                    RequiredToken("from"),
                    Required(
                        First(
                            fragment42,
                            Custom(
                                If(Not(Token("all")), rules.TableNameReference),
                                shape35)),
                        missing52),
                    Optional(
                        fragment43),
                    Optional(
                        fragment44),
                    new [] {CD(), CD(), CD(isOptional: true), CD(), CD(), CD(isOptional: true), CD(isOptional: true)}));

            var DropEmptyExtentContainers = Command("DropEmptyExtentContainers", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("drop", CompletionKind.CommandPrefix),
                        Token("async"),
                        Token("empty"),
                        RequiredToken("extentcontainers"),
                        Required(rules.DatabaseNameReference, rules.MissingNameReference),
                        RequiredToken("until"),
                        RequiredToken("="),
                        Required(rules.Value, rules.MissingValue),
                        Optional(Token("whatif")),
                        Optional(
                            fragment7)}
                    ,
                    new [] {CD(), CD(), CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(), CD("d", CompletionHint.Literal), CD(isOptional: true), CD(isOptional: true)}));

            var DropExtentTagsFromTable = Command("DropExtentTagsFromTable", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("drop", CompletionKind.CommandPrefix),
                        Token("async"),
                        Token("table"),
                        rules.TableNameReference,
                        Token("extent"),
                        Token("tags"),
                        Token("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.StringLiteral,
                                    shape55),
                                fnMissingElement: rules.MissingStringLiteral),
                            missing0),
                        RequiredToken(")"),
                        Optional(
                            fragment5)}
                    ,
                    new [] {CD(), CD(), CD(), CD("tableName", CompletionHint.Table), CD(), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var DropExtentTagsFromQuery = Command("DropExtentTagsFromQuery", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("async"),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredToken("extent"),
                    RequiredToken("tags"),
                    Required(
                        fragment6,
                        missing1),
                    new [] {CD(), CD(), CD(), CD("tableName", CompletionHint.Table), CD(), CD(), CD("csl")}));

            var DropExtentTagsFromQuery2 = Command("DropExtentTagsFromQuery", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("drop", CompletionKind.CommandPrefix),
                        Token("async"),
                        RequiredToken("table"),
                        Required(rules.TableNameReference, rules.MissingNameReference),
                        RequiredToken("extent"),
                        RequiredToken("tags"),
                        RequiredToken("with"),
                        RequiredToken("("),
                        ZeroOrMoreCommaList(
                            fragment1),
                        RequiredToken(")"),
                        Required(
                            fragment6,
                            missing1)}
                    ,
                    new [] {CD(), CD(), CD(), CD("tableName", CompletionHint.Table), CD(), CD(), CD(), CD(), CD(CompletionHint.None), CD(), CD("csl")}));

            var DropClusterRole = Command("DropClusterRole", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("admins"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape1));

            var DropClusterRole2 = Command("DropClusterRole", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("alldatabasesadmins"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape1));

            var DropClusterRole3 = Command("DropClusterRole", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("alldatabasesmonitors"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape1));

            var DropClusterRole4 = Command("DropClusterRole", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("alldatabasesviewers"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape1));

            var DropClusterBlockedPrincipals = Command("DropClusterBlockedPrincipals", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("blockedprincipals"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        fragment40),
                    Optional(
                        fragment41),
                    new [] {CD(), CD(), CD(), CD("Principal", CompletionHint.Literal), CD(isOptional: true), CD(isOptional: true)}));

            var DropClusterRole5 = Command("DropClusterRole", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("databasecreators"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape1));

            var DropClusterRole6 = Command("DropClusterRole", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("monitors"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape1));

            var DropClusterRole7 = Command("DropClusterRole", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("ops"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape1));

            var DropClusterRole8 = Command("DropClusterRole", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("users"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape1));

            var DropClusterRole9 = Command("DropClusterRole", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("viewers"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape1));

            var DropColumn = Command("DropColumn", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("column"),
                    Required(rules.TableColumnNameReference, rules.MissingNameReference),
                    new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column)}));

            var DropContinuousExport = Command("DropContinuousExport", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("continuous-export"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    shape240));

            var DropDatabaseRole = Command("DropDatabaseRole", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("admins"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape214));

            var DetachDatabase2 = Command("DetachDatabase", 
                Custom(
                    Token("drop").Hide(),
                    Token("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("ifexists"),
                    Optional(
                        Custom(
                            Token("skip-seal"),
                            shape217)),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD("IfExists"), CD(isOptional: true)}));

            var DropDatabaseIngestionMapping = Command("DropDatabaseIngestionMapping", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("ingestion"),
                    RequiredToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD("MappingKind"), CD(), CD("MappingName", CompletionHint.Literal)}));

            var DropDatabaseRole2 = Command("DropDatabaseRole", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("ingestors"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape214));

            var DropDatabaseRole3 = Command("DropDatabaseRole", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("monitors"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape214));

            var DropDatabasePrettyName = Command("DropDatabasePrettyName", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("prettyname"),
                    shape216));

            var DetachDatabase3 = Command("DetachDatabase", 
                Custom(
                    Token("drop").Hide(),
                    Token("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("skip-seal"),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD("SkipSeal")}));

            var DropDatabaseRole4 = Command("DropDatabaseRole", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("unrestrictedviewers"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape214));

            var DropDatabaseRole5 = Command("DropDatabaseRole", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("users"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape214));

            var DropDatabaseRole6 = Command("DropDatabaseRole", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("viewers"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape214));

            var DetachDatabase4 = Command("DetachDatabase", 
                Custom(
                    Token("drop").Hide(),
                    Token("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database)}));

            var DropEmptyExtentContainers2 = Command("DropEmptyExtentContainers", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("empty"),
                    RequiredToken("extentcontainers"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("until"),
                    RequiredToken("="),
                    Required(rules.Value, rules.MissingValue),
                    Optional(Token("whatif")),
                    Optional(
                        fragment7),
                    new [] {CD(), CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(), CD("d", CompletionHint.Literal), CD(isOptional: true), CD(isOptional: true)}));

            var DropEntityGroup = Command("DropEntityGroup", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("entity_group"),
                    Required(rules.EntityGroupNameReference, rules.MissingNameReference),
                    shape299));

            var DropExtents = Command("DropExtents", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("extents"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                shape224),
                            fnMissingElement: rules.MissingValue),
                        missing53),
                    RequiredToken(")"),
                    Optional(
                        fragment45),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var DropExtents2 = Command("DropExtents", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("extents"),
                    RequiredToken("<|"),
                    Required(rules.QueryInput, rules.MissingExpression),
                    new [] {CD(), CD(), CD(), CD("Query", CompletionHint.Tabular)}));

            var DropExtents3 = Command("DropExtents", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("extents"),
                    RequiredToken("from"),
                    Required(
                        First(
                            fragment42,
                            Custom(
                                If(Not(Token("all")), rules.TableNameReference),
                                shape35)),
                        missing54),
                    Optional(
                        fragment43),
                    Optional(
                        fragment44),
                    shape279));

            var DropExtents4 = Command("DropExtents", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("extents"),
                    RequiredToken("older"),
                    Required(rules.Value, rules.MissingValue),
                    RequiredToken("days", "hours"),
                    RequiredToken("from"),
                    Required(
                        First(
                            fragment42,
                            Custom(
                                If(Not(Token("all")), rules.TableNameReference),
                                shape35)),
                        missing55),
                    Optional(
                        fragment43),
                    Optional(
                        fragment44),
                    new [] {CD(), CD(), CD(), CD("Older", CompletionHint.Literal), CD(), CD(), CD(), CD(isOptional: true), CD(isOptional: true)}));

            var DropExtentsPartitionMetadata = Command("DropExtentsPartitionMetadata", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("extents"),
                    Token("partition"),
                    RequiredToken("metadata"),
                    RequiredToken("from"),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Optional(
                        Custom(
                            Token("between"),
                            RequiredToken("("),
                            Required(rules.Value, rules.MissingValue),
                            RequiredToken(".."),
                            Required(rules.Value, rules.MissingValue),
                            RequiredToken(")"),
                            new [] {CD(), CD(), CD("d1", CompletionHint.Literal), CD(), CD("d2", CompletionHint.Literal), CD()})),
                    Required(
                        fragment6,
                        missing1),
                    new [] {CD(), CD(), CD(), CD(), CD(), CD(), CD("TableName", CompletionHint.Table), CD(isOptional: true), CD("csl")}));

            var DropExtents5 = Command("DropExtents", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("extents"),
                    RequiredToken("whatif"),
                    RequiredToken("<|"),
                    Required(rules.QueryInput, rules.MissingExpression),
                    new [] {CD(), CD(), CD(), CD(), CD("Query", CompletionHint.Tabular)}));

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
                        fragment45),
                    new [] {CD(), CD(), CD("ExtentId", CompletionHint.Literal), CD(isOptional: true)}));

            var DropExternalTableAdmins = Command("DropExternalTableAdmins", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("drop", CompletionKind.CommandPrefix),
                        Token("external"),
                        Token("table"),
                        rules.ExternalTableNameReference,
                        Token("admins"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.StringLiteral,
                                    shape5),
                                fnMissingElement: rules.MissingStringLiteral),
                            missing0),
                        RequiredToken(")"),
                        Optional(Token("skip-results")),
                        Optional(
                            Custom(
                                rules.StringLiteral,
                                shape6))}
                    ,
                    shape227));

            var DropExternalTableMapping = Command("DropExternalTableMapping", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    rules.ExternalTableNameReference,
                    Token("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape301));

            var DropExternalTable = Command("DropExternalTable", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("external"),
                    RequiredToken("table"),
                    Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                    shape304));

            var DropFabricServiceAssignmentsCommand = Command("DropFabricServiceAssignmentsCommand", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("fabric"),
                    RequiredToken("service"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    RequiredToken("assignments"),
                    shape229));

            var DropFollowerDatabases = Command("DropFollowerDatabases", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("follower"),
                    Token("databases"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.DatabaseNameReference,
                                shape84),
                            fnMissingElement: rules.MissingNameReference),
                        missing32),
                    RequiredToken(")"),
                    RequiredToken("from"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD(CompletionHint.Database), CD(), CD(), CD("leaderClusterMetadataPath", CompletionHint.Literal)}));

            var DropFollowerDatabases2 = Command("DropFollowerDatabases", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("follower"),
                    Token("database"),
                    rules.DatabaseNameReference,
                    Token("from"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD("leaderClusterMetadataPath", CompletionHint.Literal)}));

            var DropFollowerDatabaseAuthorizedPrincipals = Command("DropFollowerDatabaseAuthorizedPrincipals", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("follower"),
                    RequiredToken("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("admins", "monitors", "unrestrictedviewers", "users", "viewers"),
                    Optional(
                        fragment22),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape5),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    new [] {CD(), CD(), CD(), CD("dbName", CompletionHint.Database), CD("operationRole"), CD(isOptional: true), CD(), CD(CompletionHint.Literal), CD()}));

            var DropFunctions = Command("DropFunctions", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("functions"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.FunctionNameReference,
                                shape135),
                            fnMissingElement: rules.MissingNameReference),
                        missing32),
                    RequiredToken(")"),
                    Optional(Token("ifexists")),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Function), CD(), CD(isOptional: true)}));

            var DropFunctionRole = Command("DropFunctionRole", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("function"),
                    rules.FunctionNameReference,
                    Token("admins"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape231));

            var DropFunction = Command("DropFunction", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("function"),
                    Required(rules.FunctionNameReference, rules.MissingNameReference),
                    RequiredToken("ifexists"),
                    shape232));

            var DropFunction2 = Command("DropFunction", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("function"),
                    Required(rules.FunctionNameReference, rules.MissingNameReference),
                    shape306));

            var GraphModelDrop = Command("GraphModelDrop", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("graph_model").Hide(),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    shape185));

            var GraphSnapshotsDrop = Command("GraphSnapshotsDrop", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("graph_snapshots").Hide(),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    shape185));

            var GraphSnapshotDrop = Command("GraphSnapshotDrop", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("graph_snapshot").Hide(),
                    Required(rules.QualifiedNameDeclaration, rules.MissingNameDeclaration),
                    shape185));

            var DropMaterializedViewAdmins = Command("DropMaterializedViewAdmins", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("admins"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape5),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape6)),
                    shape234));

            var DropMaterializedView = Command("DropMaterializedView", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    shape235));

            var DropRowStore2 = Command("DropRowStore", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("rowstore"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Optional(Token("ifexists")),
                    shape236));

            var StoredQueryResultsDrop = Command("StoredQueryResultsDrop", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("stored_query_results"),
                    RequiredToken("by"),
                    RequiredToken("user"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("Principal", CompletionHint.Literal)}));

            var StoredQueryResultDrop = Command("StoredQueryResultDrop", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("stored_query_result"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    new [] {CD(), CD(), CD("StoredQueryResultName", CompletionHint.None)}));

            var DropStoredQueryResultContainers = Command("DropStoredQueryResultContainers", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("storedqueryresultcontainers"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    Required(
                        OneOrMoreList(
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                shape237)),
                        missing37),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(CompletionHint.Literal)}));

            var DropTables = Command("DropTables", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("tables"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.TableNameReference,
                                shape35),
                            fnMissingElement: rules.MissingNameReference),
                        missing32),
                    RequiredToken(")"),
                    Optional(Token("ifexists")),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(isOptional: true)}));

            var DropTableRole = Command("DropTableRole", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("admins"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape238));

            var DropTableColumns = Command("DropTableColumns", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("columns"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.ColumnNameReference,
                                shape45),
                            fnMissingElement: rules.MissingNameReference),
                        missing32),
                    RequiredToken(")"),
                    shape171));

            var DropExtentTagsFromTable2 = Command("DropExtentTagsFromTable", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("extent"),
                    Token("tags"),
                    Token("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape55),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        fragment5),
                    new [] {CD(), CD(), CD("tableName", CompletionHint.Table), CD(), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var DropExtentTagsFromQuery3 = Command("DropExtentTagsFromQuery", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredToken("extent"),
                    RequiredToken("tags"),
                    Required(
                        fragment6,
                        missing1),
                    new [] {CD(), CD(), CD("tableName", CompletionHint.Table), CD(), CD(), CD("csl")}));

            var DropExtentTagsFromQuery4 = Command("DropExtentTagsFromQuery", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("drop", CompletionKind.CommandPrefix),
                        Token("table"),
                        Required(rules.TableNameReference, rules.MissingNameReference),
                        RequiredToken("extent"),
                        RequiredToken("tags"),
                        RequiredToken("with"),
                        RequiredToken("("),
                        ZeroOrMoreCommaList(
                            fragment1),
                        RequiredToken(")"),
                        Required(
                            fragment6,
                            missing1)}
                    ,
                    new [] {CD(), CD(), CD("tableName", CompletionHint.Table), CD(), CD(), CD(), CD(), CD(CompletionHint.None), CD(), CD("csl")}));

            var DropTable = Command("DropTable", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredToken("ifexists"),
                    shape239));

            var DropTableIngestionMapping = Command("DropTableIngestionMapping", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("ingestion"),
                    RequiredToken("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                    RequiredToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("MappingKind"), CD(), CD("MappingName", CompletionHint.Literal)}));

            var DropTableRole2 = Command("DropTableRole", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("ingestors"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.StringLiteral,
                                shape2),
                            fnMissingElement: rules.MissingStringLiteral),
                        missing0),
                    RequiredToken(")"),
                    Optional(
                        Custom(
                            Token("skip-results"),
                            shape3)),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape4)),
                    shape238));

            var DropTable2 = Command("DropTable", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    shape318));

            var DropTempStorage = Command("DropTempStorage", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("tempstorage"),
                    RequiredToken("older"),
                    Required(rules.Value, rules.MissingValue),
                    new [] {CD(), CD(), CD(), CD("olderThan", CompletionHint.Literal)}));

            var DropUnusedStoredQueryResultContainers = Command("DropUnusedStoredQueryResultContainers", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    Token("unused"),
                    RequiredToken("storedqueryresultcontainers"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    new [] {CD(), CD(), CD(), CD("databaseName", CompletionHint.Database)}));

            var DropWorkloadGroup = Command("DropWorkloadGroup", 
                Custom(
                    Token("drop", CompletionKind.CommandPrefix),
                    RequiredToken("workload_group"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    new [] {CD(), CD(), CD("WorkloadGroupName", CompletionHint.None)}));

            var EnableContinuousExport = Command("EnableContinuousExport", 
                Custom(
                    Token("enable", CompletionKind.CommandPrefix),
                    Token("continuous-export"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    shape240));

            var EnableDatabaseStreamingIngestionMaintenanceMode = Command("EnableDatabaseStreamingIngestionMaintenanceMode", 
                Custom(
                    Token("enable", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("streamingingestion_maintenance_mode")));

            var EnableDatabaseMaintenanceMode = Command("EnableDatabaseMaintenanceMode", 
                Custom(
                    Token("enable", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(If(Not(Token("streamingingestion_maintenance_mode")), rules.DatabaseNameReference), rules.MissingNameReference),
                    RequiredToken("maintenance_mode"),
                    shape216));

            var EnableDisableMaterializedView2 = Command("EnableDisableMaterializedView", 
                Custom(
                    Token("enable", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    shape235));

            var EnablePlugin = Command("EnablePlugin", 
                Custom(
                    Token("enable", CompletionKind.CommandPrefix),
                    RequiredToken("plugin"),
                    Required(
                        First(
                            rules.StringLiteral,
                            rules.NameDeclaration),
                        rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("name", CompletionHint.Literal)}));

            var ExecuteClusterScript = Command("ExecuteClusterScript", 
                Custom(
                    Token("execute", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("script"),
                    Optional(
                        fragment46),
                    RequiredToken("<|"),
                    Required(rules.ScriptInput, rules.MissingStatement),
                    shape241));

            var ExecuteDatabaseScript = Command("ExecuteDatabaseScript", 
                Custom(
                    Token("execute", CompletionKind.CommandPrefix),
                    RequiredToken("database"),
                    RequiredToken("script"),
                    Optional(
                        fragment46),
                    RequiredToken("<|"),
                    Required(rules.ScriptInput, rules.MissingStatement),
                    shape241));

            var ExecuteDatabaseScript2 = Command("ExecuteDatabaseScript", 
                Custom(
                    Token("execute", CompletionKind.CommandPrefix),
                    RequiredToken("script"),
                    Optional(
                        fragment46),
                    RequiredToken("<|"),
                    Required(rules.ScriptInput, rules.MissingStatement),
                    new [] {CD(), CD(), CD(isOptional: true), CD(), CD(CompletionHint.Tabular)}));

            var ExportToStorage = Command("ExportToStorage", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("export", CompletionKind.CommandPrefix),
                        Optional(Token("async")),
                        RequiredToken("compressed"),
                        RequiredToken("to"),
                        RequiredToken("csv", "json", "parquet", "tsv"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.StringLiteral,
                                    CD("DataConnectionString", CompletionHint.Literal)),
                                fnMissingElement: rules.MissingStringLiteral),
                            missing0),
                        RequiredToken(")"),
                        Optional(
                            fragment7),
                        RequiredToken("<|"),
                        Required(rules.QueryInput, rules.MissingExpression)}
                    ,
                    new [] {CD(), CD(isOptional: true), CD(), CD(), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true), CD(), CD("Query", CompletionHint.Tabular)}));

            var ExportToStorage2 = Command("ExportToStorage", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("export", CompletionKind.CommandPrefix),
                        Optional(Token("async")),
                        RequiredToken("to"),
                        RequiredToken("csv"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.StringLiteral,
                                    CD("DataConnectionString", CompletionHint.Literal)),
                                fnMissingElement: rules.MissingStringLiteral),
                            missing0),
                        RequiredToken(")"),
                        Optional(
                            fragment7),
                        RequiredToken("<|"),
                        Required(rules.QueryInput, rules.MissingExpression)}
                    ,
                    shape242));

            var ExportToStorage3 = Command("ExportToStorage", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("export", CompletionKind.CommandPrefix),
                        Optional(Token("async")),
                        RequiredToken("to"),
                        RequiredToken("json"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.StringLiteral,
                                    CD("DataConnectionString", CompletionHint.Literal)),
                                fnMissingElement: rules.MissingStringLiteral),
                            missing0),
                        RequiredToken(")"),
                        Optional(
                            fragment7),
                        RequiredToken("<|"),
                        Required(rules.QueryInput, rules.MissingExpression)}
                    ,
                    shape242));

            var ExportToStorage4 = Command("ExportToStorage", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("export", CompletionKind.CommandPrefix),
                        Optional(Token("async")),
                        RequiredToken("to"),
                        RequiredToken("parquet"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.StringLiteral,
                                    CD("DataConnectionString", CompletionHint.Literal)),
                                fnMissingElement: rules.MissingStringLiteral),
                            missing0),
                        RequiredToken(")"),
                        Optional(
                            fragment7),
                        RequiredToken("<|"),
                        Required(rules.QueryInput, rules.MissingExpression)}
                    ,
                    shape242));

            var ExportToSqlTable = Command("ExportToSqlTable", 
                Custom(
                    Token("export", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    Token("to"),
                    Token("sql"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        fragment7),
                    RequiredToken("<|"),
                    Required(rules.QueryInput, rules.MissingExpression),
                    new [] {CD(), CD(isOptional: true), CD(), CD(), CD("SqlTableName", CompletionHint.None), CD("SqlConnectionString", CompletionHint.Literal), CD(isOptional: true), CD(), CD("Query", CompletionHint.Tabular)}));

            var ExportToExternalTable = Command("ExportToExternalTable", 
                Custom(
                    Token("export", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    Token("to"),
                    Token("table"),
                    Required(rules.ExternalTableNameReference, rules.MissingNameReference),
                    Optional(
                        fragment7),
                    RequiredToken("<|"),
                    Required(rules.QueryInput, rules.MissingExpression),
                    new [] {CD(), CD(isOptional: true), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(isOptional: true), CD(), CD("Query", CompletionHint.Tabular)}));

            var ExportToStorage5 = Command("ExportToStorage", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("export", CompletionKind.CommandPrefix),
                        Optional(Token("async")),
                        RequiredToken("to"),
                        RequiredToken("tsv"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.StringLiteral,
                                    CD("DataConnectionString", CompletionHint.Literal)),
                                fnMissingElement: rules.MissingStringLiteral),
                            missing0),
                        RequiredToken(")"),
                        Optional(
                            fragment7),
                        RequiredToken("<|"),
                        Required(rules.QueryInput, rules.MissingExpression)}
                    ,
                    shape242));

            var IngestIntoTable = Command("IngestIntoTable", 
                Custom(
                    Token("ingest", CompletionKind.CommandPrefix),
                    RequiredToken("async"),
                    RequiredToken("into"),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Required(
                        First(
                            fragment49,
                            Custom(
                                rules.StringLiteral,
                                shape177)),
                        missing3),
                    Optional(
                        fragment47),
                    new [] {CD(), CD(), CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(isOptional: true)}));

            var IngestInlineIntoTable = Command("IngestInlineIntoTable", 
                Custom(
                    Token("ingest", CompletionKind.CommandPrefix),
                    Token("inline"),
                    RequiredToken("into"),
                    RequiredToken("table"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Required(
                        First(
                            Custom(
                                Token("["),
                                Required(rules.BracketedInputText, rules.MissingInputText),
                                RequiredToken("]"),
                                new [] {CD(), CD("Data", CompletionHint.None), CD()}),
                            Custom(
                                Token("<|"),
                                Required(rules.InputText, rules.MissingInputText),
                                new [] {CD(), CD("Data", CompletionHint.None)}),
                            Custom(
                                Token("with"),
                                RequiredToken("("),
                                Required(
                                    OneOrMoreCommaList(
                                        fragment48),
                                    missing35),
                                RequiredToken(")"),
                                RequiredToken("<|"),
                                Required(rules.InputText, rules.MissingInputText),
                                new [] {CD(), CD(), CD(), CD(), CD(), CD("Data", CompletionHint.None)})),
                        missing57),
                    new [] {CD(), CD(), CD(), CD(), CD("TableName", CompletionHint.None), CD()}));

            var IngestIntoTable2 = Command("IngestIntoTable", 
                Custom(
                    Token("ingest", CompletionKind.CommandPrefix),
                    RequiredToken("into"),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Required(
                        First(
                            fragment49,
                            Custom(
                                rules.StringLiteral,
                                shape177)),
                        missing3),
                    Optional(
                        fragment47),
                    new [] {CD(), CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(isOptional: true)}));

            var AttachDatabase3 = Command("AttachDatabase", 
                Custom(
                    Token("load").Hide(),
                    Optional(
                        Custom(
                            Token("database"),
                            Optional(
                                Token("all", "metadata")).Hide(),
                            Required(If(Not(And(Token("all", "metadata"))), rules.DatabaseNameReference), rules.MissingNameReference),
                            new [] {CD(), CD(isOptional: true), CD("DatabaseName", CompletionHint.Database)})),
                    RequiredToken("from"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        fragment35),
                    Optional(
                        fragment7),
                    new [] {CD(), CD(isOptional: true), CD(), CD("Path", CompletionHint.Literal), CD(isOptional: true), CD(isOptional: true)}));

            var GraphSnapshotMake = Command("GraphSnapshotMake", 
                Custom(
                    Token("make").Hide(),
                    Optional(Token("async")),
                    RequiredToken("graph_snapshot").Hide(),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    RequiredToken("from").Hide(),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    new [] {CD(), CD(isOptional: true), CD(), CD(CompletionHint.None), CD(), CD(CompletionHint.None)}));

            var MergeExtents = Command("MergeExtents", 
                Custom(
                    Token("merge", CompletionKind.CommandPrefix),
                    RequiredToken("async"),
                    Required(If(Not(And(Token("async", "database", "dryrun"))), rules.TableNameReference), rules.MissingNameReference),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                shape244),
                            fnMissingElement: rules.MissingValue),
                        missing53),
                    RequiredToken(")"),
                    Optional(
                        fragment50),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var MergeDatabaseShardGroups = Command("MergeDatabaseShardGroups", 
                Custom(
                    Token("merge", CompletionKind.CommandPrefix),
                    Token("database").Hide(),
                    RequiredToken("shard-groups").Hide()));

            var MergeExtentsDryrun = Command("MergeExtentsDryrun", 
                Custom(
                    Token("merge", CompletionKind.CommandPrefix),
                    Token("dryrun"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                shape244),
                            fnMissingElement: rules.MissingValue),
                        missing53),
                    RequiredToken(")"),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(CompletionHint.Literal), CD()}));

            var MergeExtents2 = Command("MergeExtents", 
                Custom(
                    Token("merge", CompletionKind.CommandPrefix),
                    Required(If(Not(And(Token("async", "database", "dryrun"))), rules.TableNameReference), rules.MissingNameReference),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                shape244),
                            fnMissingElement: rules.MissingValue),
                        missing53),
                    RequiredToken(")"),
                    Optional(
                        fragment50),
                    new [] {CD(), CD("TableName", CompletionHint.Table), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var MoveExtentsFrom = Command("MoveExtentsFrom", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("move", CompletionKind.CommandPrefix),
                        Optional(Token("async")),
                        Token("extents"),
                        Token("all"),
                        RequiredToken("from"),
                        RequiredToken("table"),
                        Required(rules.TableNameReference, rules.MissingNameReference),
                        RequiredToken("to"),
                        RequiredToken("table"),
                        Required(rules.TableNameReference, rules.MissingNameReference)}
                    ,
                    new [] {CD(), CD(isOptional: true), CD(), CD(), CD(), CD(), CD("SourceTableName", CompletionHint.Table), CD(), CD(), CD("DestinationTableName", CompletionHint.Table)}));

            var MoveExtentsFrom2 = Command("MoveExtentsFrom", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("move", CompletionKind.CommandPrefix),
                        Optional(Token("async")),
                        Token("extents"),
                        Token("from"),
                        RequiredToken("table"),
                        Required(rules.TableNameReference, rules.MissingNameReference),
                        RequiredToken("to"),
                        RequiredToken("table"),
                        Required(rules.TableNameReference, rules.MissingNameReference),
                        Optional(
                            fragment5),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.AnyGuidLiteralOrString,
                                    shape244),
                                fnMissingElement: rules.MissingValue),
                            missing53),
                        RequiredToken(")")}
                    ,
                    new [] {CD(), CD(isOptional: true), CD(), CD(), CD(), CD("SourceTableName", CompletionHint.Table), CD(), CD(), CD("DestinationTableName", CompletionHint.Table), CD(isOptional: true), CD(), CD(CompletionHint.Literal), CD()}));

            var MoveExtentsQuery = Command("MoveExtentsQuery", 
                Custom(
                    Token("move", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    RequiredToken("extents"),
                    RequiredToken("to"),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Optional(
                        fragment5),
                    RequiredToken("<|"),
                    Required(rules.QueryInput, rules.MissingExpression),
                    new [] {CD(), CD(isOptional: true), CD(), CD(), CD(), CD("DestinationTableName", CompletionHint.Table), CD(isOptional: true), CD(), CD("Query", CompletionHint.Tabular)}));

            var PatchExtentCorruptedDatetime = Command("PatchExtentCorruptedDatetime", 
                Custom(
                    Token("patch"),
                    Optional(
                        Custom(
                            Token("table"),
                            Required(rules.TableNameReference, rules.MissingNameReference),
                            new [] {CD(), CD(CompletionHint.Table)})),
                    RequiredToken("extents"),
                    RequiredToken("corrupted"),
                    RequiredToken("datetime"),
                    new [] {CD(), CD(isOptional: true), CD(), CD(), CD()}).Hide());

            var RenameColumns = Command("RenameColumns", 
                Custom(
                    Token("rename", CompletionKind.CommandPrefix),
                    Token("columns"),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.NameDeclaration,
                                RequiredToken("="),
                                Required(rules.DatabaseTableColumnNameReference, rules.MissingNameReference),
                                new [] {CD("NewColumnName", CompletionHint.None), CD(), CD("ColumnName", CompletionHint.Column)})),
                        missing58),
                    shape185));

            var RenameColumn = Command("RenameColumn", 
                Custom(
                    Token("rename", CompletionKind.CommandPrefix),
                    Token("column"),
                    Required(rules.DatabaseTableColumnNameReference, rules.MissingNameReference),
                    RequiredToken("to"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    new [] {CD(), CD(), CD("ColumnName", CompletionHint.Column), CD(), CD("NewColumnName", CompletionHint.None)}));

            var RenameMaterializedView = Command("RenameMaterializedView", 
                Custom(
                    Token("rename", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    RequiredToken("to"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD("NewMaterializedViewName", CompletionHint.None)}));

            var RenameTables = Command("RenameTables", 
                Custom(
                    Token("rename", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.NameDeclaration,
                                RequiredToken("="),
                                Required(rules.TableNameReference, rules.MissingNameReference),
                                new [] {CD("NewTableName", CompletionHint.None), CD(), CD("TableName", CompletionHint.Table)})),
                        missing59),
                    shape185));

            var RenameTable = Command("RenameTable", 
                Custom(
                    Token("rename", CompletionKind.CommandPrefix),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredToken("to"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("NewTableName", CompletionHint.None)}));

            var ReplaceExtents = Command("ReplaceExtents", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("replace", CompletionKind.CommandPrefix),
                        RequiredToken("async"),
                        RequiredToken("extents"),
                        RequiredToken("in"),
                        RequiredToken("table"),
                        Required(rules.TableNameReference, rules.MissingNameReference),
                        Optional(
                            fragment5),
                        RequiredToken("<|"),
                        RequiredToken("{"),
                        Required(rules.QueryInput, rules.MissingExpression),
                        RequiredToken("}"),
                        RequiredToken(","),
                        RequiredToken("{"),
                        Required(rules.QueryInput, rules.MissingExpression),
                        RequiredToken("}")}
                    ,
                    new [] {CD(), CD(), CD(), CD(), CD(), CD("DestinationTableName", CompletionHint.Table), CD(isOptional: true), CD(), CD(), CD("ExtentsToDropQuery", CompletionHint.Tabular), CD(), CD(), CD(), CD("ExtentsToMoveQuery", CompletionHint.Tabular), CD()}));

            var ReplaceDatabaseKeyVaultSecrets = Command("ReplaceDatabaseKeyVaultSecrets", 
                Custom(
                    Token("replace"),
                    Token("database"),
                    RequiredToken("keyvaultsecrets"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    new [] {CD(), CD(), CD(), CD("secrets", CompletionHint.None)}).Hide());

            var ReplaceExtents2 = Command("ReplaceExtents", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("replace", CompletionKind.CommandPrefix),
                        RequiredToken("extents"),
                        RequiredToken("in"),
                        RequiredToken("table"),
                        Required(rules.TableNameReference, rules.MissingNameReference),
                        Optional(
                            fragment5),
                        RequiredToken("<|"),
                        RequiredToken("{"),
                        Required(rules.QueryInput, rules.MissingExpression),
                        RequiredToken("}"),
                        RequiredToken(","),
                        RequiredToken("{"),
                        Required(rules.QueryInput, rules.MissingExpression),
                        RequiredToken("}")}
                    ,
                    new [] {CD(), CD(), CD(), CD(), CD("DestinationTableName", CompletionHint.Table), CD(isOptional: true), CD(), CD(), CD("ExtentsToDropQuery", CompletionHint.Tabular), CD(), CD(), CD(), CD("ExtentsToMoveQuery", CompletionHint.Tabular), CD()}));

            var SetOrAppendTable = Command("SetOrAppendTable", 
                Custom(
                    Token("set-or-append", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    Required(If(Not(Token("async")), rules.NameDeclaration), rules.MissingNameDeclaration),
                    Optional(
                        fragment47),
                    RequiredToken("<|"),
                    Required(rules.QueryInput, rules.MissingExpression),
                    shape252));

            var StoredQueryResultSetOrReplace = Command("StoredQueryResultSetOrReplace", 
                Custom(
                    Token("set-or-replace", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    Token("stored_query_result"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Optional(
                        fragment47),
                    RequiredToken("<|"),
                    Required(rules.QueryInput, rules.MissingExpression),
                    new [] {CD(), CD(isOptional: true), CD(), CD("StoredQueryResultName", CompletionHint.None), CD(isOptional: true), CD(), CD("Query", CompletionHint.Tabular)}));

            var SetOrReplaceTable = Command("SetOrReplaceTable", 
                Custom(
                    Token("set-or-replace", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    Required(If(Not(And(Token("async", "stored_query_result"))), rules.NameDeclaration), rules.MissingNameDeclaration),
                    Optional(
                        fragment47),
                    RequiredToken("<|"),
                    Required(rules.QueryInput, rules.MissingExpression),
                    shape252));

            var SetAccess = Command("SetAccess", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("access"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("to"),
                    RequiredToken("readonly", "readwrite"),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD("AccessMode")}));

            var StoredQueryResultSet = Command("StoredQueryResultSet", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("async"),
                    Token("ifnotexists"),
                    RequiredToken("stored_query_result"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Optional(
                        fragment47),
                    RequiredToken("<|"),
                    Required(rules.QueryInput, rules.MissingExpression),
                    new [] {CD(), CD(), CD(), CD(), CD("StoredQueryResultName", CompletionHint.None), CD(isOptional: true), CD(), CD("Query", CompletionHint.Tabular)}));

            var StoredQueryResultSet2 = Command("StoredQueryResultSet", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("async"),
                    Token("stored_query_result"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Optional(
                        fragment47),
                    RequiredToken("<|"),
                    Required(rules.QueryInput, rules.MissingExpression),
                    shape258));

            var SetTable = Command("SetTable", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    RequiredToken("async"),
                    Required(If(Not(And(Token("access", "async", "cluster", "continuous-export", "database", "external", "function", "ifnotexists", "materialized-view", "stored_query_result", "table"))), rules.NameDeclaration), rules.MissingNameDeclaration),
                    Optional(
                        fragment47),
                    RequiredToken("<|"),
                    Required(rules.QueryInput, rules.MissingExpression),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.None), CD(isOptional: true), CD(), CD("QueryOrCommand", CompletionHint.Tabular)}));

            var SetClusterRole = Command("SetClusterRole", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    RequiredToken("admins", "alldatabasesadmins", "alldatabasesmonitors", "alldatabasesviewers", "databasecreators", "monitors", "ops", "users", "viewers"),
                    Required(
                        First(
                            fragment51,
                            fragment52),
                        missing60),
                    new [] {CD(), CD(), CD("Role"), CD()}));

            var SetContinuousExportCursor = Command("SetContinuousExportCursor", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("continuous-export"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    RequiredToken("cursor"),
                    RequiredToken("to"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("jobName", CompletionHint.None), CD(), CD(), CD("cursorValue", CompletionHint.Literal)}));

            var SetDatabaseRole = Command("SetDatabaseRole", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("admins"),
                    Required(
                        First(
                            fragment51,
                            fragment52),
                        missing60),
                    shape255));

            var SetDatabaseRole2 = Command("SetDatabaseRole", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("ingestors"),
                    Required(
                        First(
                            fragment51,
                            fragment52),
                        missing60),
                    shape255));

            var SetDatabaseRole3 = Command("SetDatabaseRole", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("monitors"),
                    Required(
                        First(
                            fragment51,
                            fragment52),
                        missing60),
                    shape255));

            var AlterDatabasePrettyName2 = Command("AlterDatabasePrettyName", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(Token("policy")), rules.DatabaseNameReference),
                    Token("prettyname"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape257));

            var SetDatabaseRole4 = Command("SetDatabaseRole", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("unrestrictedviewers"),
                    Required(
                        First(
                            fragment51,
                            fragment52),
                        missing60),
                    shape255));

            var SetDatabaseRole5 = Command("SetDatabaseRole", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("users"),
                    Required(
                        First(
                            fragment51,
                            fragment52),
                        missing60),
                    shape255));

            var SetDatabaseRole6 = Command("SetDatabaseRole", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("database"),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("viewers"),
                    Required(
                        First(
                            fragment51,
                            fragment52),
                        missing60),
                    shape255));

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
                                    OneOrMoreCommaList(
                                        Custom(
                                            rules.StringLiteral,
                                            shape5),
                                        fnMissingElement: rules.MissingStringLiteral),
                                    missing0),
                                RequiredToken(")"),
                                Optional(Token("skip-results")),
                                Optional(
                                    Custom(
                                        rules.StringLiteral,
                                        shape6)),
                                shape253),
                            Custom(
                                Token("none"),
                                Optional(Token("skip-results")),
                                shape254)),
                        missing60),
                    new [] {CD(), CD(), CD(), CD("externalTableName", CompletionHint.ExternalTable), CD(), CD()}));

            var SetFunctionRole = Command("SetFunctionRole", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("function"),
                    Required(rules.FunctionNameReference, rules.MissingNameReference),
                    RequiredToken("admins"),
                    Required(
                        First(
                            fragment51,
                            fragment52),
                        missing60),
                    new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD("Role"), CD()}));

            var StoredQueryResultSet3 = Command("StoredQueryResultSet", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("ifnotexists"),
                    RequiredToken("stored_query_result"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Optional(
                        fragment47),
                    RequiredToken("<|"),
                    Required(rules.QueryInput, rules.MissingExpression),
                    shape258));

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
                                    OneOrMoreCommaList(
                                        Custom(
                                            rules.StringLiteral,
                                            shape5),
                                        fnMissingElement: rules.MissingStringLiteral),
                                    missing0),
                                RequiredToken(")"),
                                Optional(
                                    Custom(
                                        rules.StringLiteral,
                                        shape6)),
                                new [] {CD(), CD(CompletionHint.Literal), CD(), CD(CompletionHint.Literal, isOptional: true)}),
                            Token("none")),
                        missing61),
                    new [] {CD(), CD(), CD("materializedViewName", CompletionHint.MaterializedView), CD(), CD()}));

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
                            new [] {CD(), CD("n", CompletionHint.Literal)})),
                    shape310));

            var SetMaterializedViewCursor = Command("SetMaterializedViewCursor", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Required(rules.MaterializedViewNameReference, rules.MissingNameReference),
                    RequiredToken("cursor"),
                    RequiredToken("to"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(), CD("CursorValue", CompletionHint.Literal)}));

            var StoredQueryResultSet4 = Command("StoredQueryResultSet", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("stored_query_result"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Optional(
                        fragment47),
                    RequiredToken("<|"),
                    Required(rules.QueryInput, rules.MissingExpression),
                    new [] {CD(), CD(), CD("StoredQueryResultName", CompletionHint.None), CD(isOptional: true), CD(), CD("Query", CompletionHint.Tabular)}));

            var SetTableRowStoreReferences = Command("SetTableRowStoreReferences", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.DatabaseTableNameReference,
                    RequiredToken("rowstore_references"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("references", CompletionHint.Literal)}));

            var SetTableRole = Command("SetTableRole", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Token("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredToken("admins", "ingestors"),
                    Required(
                        First(
                            fragment51,
                            fragment52),
                        missing60),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD("Role"), CD()}));

            var SetTable2 = Command("SetTable", 
                Custom(
                    Token("set", CompletionKind.CommandPrefix),
                    Required(If(Not(And(Token("access", "async", "cluster", "continuous-export", "database", "external", "function", "ifnotexists", "materialized-view", "stored_query_result", "table"))), rules.NameDeclaration), rules.MissingNameDeclaration),
                    Optional(
                        fragment47),
                    RequiredToken("<|"),
                    Required(rules.QueryInput, rules.MissingExpression),
                    new [] {CD(), CD("TableName", CompletionHint.None), CD(isOptional: true), CD(), CD("QueryOrCommand", CompletionHint.Tabular)}));

            var ShowCache = Command("ShowCache", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cache")));

            var ShowCallStacks = Command("ShowCallStacks", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("callstacks"),
                    Optional(
                        fragment7),
                    shape198));

            var ShowCapacity = Command("ShowCapacity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("capacity"),
                    Optional(
                        Custom(
                            Token("data-export", "extents-merge", "extents-partition", "ingestions", "materialized-view", "mirroring", "periodic-storage-artifacts-cleanup", "purge-storage-artifacts-cleanup", "queries", "query-acceleration", "stored-query-results", "streaming-ingestion-post-processing", "table-purge"),
                            CD("Resource"))),
                    Optional(
                        fragment67),
                    shape307));

            var ShowCertificates = Command("ShowCertificates", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("certificates")));

            var ShowCloudSettings = Command("ShowCloudSettings", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cloudsettings")));

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

            var ShowClusterDatabasesDataStats = Command("ShowClusterDatabasesDataStats", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("databases"),
                    Optional(
                        fragment53),
                    Token("datastats"),
                    shape260));

            var ShowClusterDatabasesDetails = Command("ShowClusterDatabasesDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("databases"),
                    Optional(
                        fragment53),
                    Token("details"),
                    shape260));

            var ShowClusterDatabasesIdentity = Command("ShowClusterDatabasesIdentity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("databases"),
                    Optional(
                        fragment53),
                    Token("identity"),
                    shape260));

            var ShowClusterDatabasesMetadata = Command("ShowClusterDatabasesMetadata", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("databases"),
                    Optional(
                        fragment53),
                    Token("metadata"),
                    shape260));

            var ShowClusterDatabasesPolicies = Command("ShowClusterDatabasesPolicies", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("databases"),
                    Optional(
                        fragment53),
                    Token("policies"),
                    shape260));

            var ShowClusterDatabasesDetails2 = Command("ShowClusterDatabasesDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("databases"),
                    Optional(
                        fragment53),
                    Token("verbose"),
                    shape260));

            var ShowClusterDatabases = Command("ShowClusterDatabases", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("databases"),
                    Optional(
                        Custom(
                            Token("("),
                            Required(
                                OneOrMoreCommaList(
                                    Custom(
                                        First(
                                            rules.WildcardedNameDeclaration,
                                            rules.DatabaseNameReference),
                                        shape192),
                                    fnMissingElement: rules.MissingNameDeclaration),
                                missing28),
                            RequiredToken(")"),
                            shape167)),
                    shape265));

            var ShowClusterDetails = Command("ShowClusterDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("details")));

            var ShowClusterExtentsMetadata = Command("ShowClusterExtentsMetadata", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("extents"),
                    Optional(
                        fragment58),
                    Optional(Token("hot")),
                    Token("metadata"),
                    Optional(
                        fragment59),
                    Optional(
                        fragment55),
                    new [] {CD(), CD(), CD(), CD(isOptional: true), CD(isOptional: true), CD(), CD(isOptional: true), CD(isOptional: true)}));

            var ShowClusterExtents = Command("ShowClusterExtents", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("extents"),
                    Optional(
                        fragment56),
                    Optional(Token("hot")),
                    RequiredToken("where"),
                    Required(
                        OneOrMoreList(
                            fragment54,
                            separatorParser: Token("and")),
                        missing62),
                    Optional(
                        fragment55),
                    new [] {CD(), CD(), CD(), CD(isOptional: true), CD(isOptional: true), CD(), CD(), CD(isOptional: true)}));

            var ShowClusterExtents2 = Command("ShowClusterExtents", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("cluster"),
                        Token("extents"),
                        Optional(
                            fragment56),
                        Optional(Token("hot")),
                        RequiredToken("with"),
                        RequiredToken("("),
                        RequiredToken("extentsShowFilteringRuntimePolicy"),
                        RequiredToken("="),
                        Required(rules.Value, rules.MissingValue),
                        RequiredToken(")")}
                    ,
                    new [] {CD(), CD(), CD(), CD(isOptional: true), CD(isOptional: true), CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()}));

            var ShowClusterExtents3 = Command("ShowClusterExtents", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("extents"),
                    Optional(
                        fragment56),
                    Optional(Token("hot")),
                    shape277));

            var ShowIngestionMappings = Command("ShowIngestionMappings", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("ingestion"),
                    Optional(
                        Custom(
                            Token("apacheavro", "avro", "csv", "json", "orc", "parquet", "sstream", "w3clogfile"),
                            shape50)),
                    RequiredToken("mappings"),
                    Optional(
                        fragment7),
                    new [] {CD(), CD(), CD(), CD(isOptional: true), CD(), CD(isOptional: true)}));

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
                            CD("bytes", CompletionHint.Literal))),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Literal, isOptional: true)}));

            var ShowClusterPendingContinuousExports = Command("ShowClusterPendingContinuousExports", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("pending"),
                    RequiredToken("continuous-exports"),
                    Optional(
                        fragment7),
                    shape264));

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
                        fragment7),
                    shape264));

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
                            fragment66),
                        missing63),
                    Optional(
                        fragment7),
                    shape264));

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
                    new [] {CD(), CD(), CD(), CD("num", CompletionHint.Literal), CD()}));

            var ShowSchema = Command("ShowSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("schema"),
                    Optional(
                        First(
                            fragment69,
                            Token("details"))),
                    shape265));

            var ShowClusterServices = Command("ShowClusterServices", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("cluster"),
                    Token("services")));

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
                    Token("*"),
                    RequiredToken("policy"),
                    RequiredToken("caching"),
                    new [] {CD(), CD(), CD("ColumnName"), CD(), CD()}));

            var ShowColumnPolicyCaching2 = Command("ShowColumnPolicyCaching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("column"),
                    If(Not(Token("*")), rules.DatabaseTableColumnNameReference),
                    RequiredToken("policy"),
                    RequiredToken("caching"),
                    shape202));

            var ShowColumnPolicyEncoding = Command("ShowColumnPolicyEncoding", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("column"),
                    Required(If(Not(Token("*")), rules.TableColumnNameReference), rules.MissingNameReference),
                    RequiredToken("policy"),
                    RequiredToken("encoding"),
                    shape202));

            var ShowCommandsAndQueries = Command("ShowCommandsAndQueries", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("commands-and-queries")));

            var ShowCommands = Command("ShowCommands", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("commands")));

            var ShowCommConcurrency = Command("ShowCommConcurrency", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("commconcurrency")));

            var ShowCommPools = Command("ShowCommPools", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("commpools")));

            var ShowContinuousExports = Command("ShowContinuousExports", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("continuous-exports")));

            var ShowContinuousExportExportedArtifacts = Command("ShowContinuousExportExportedArtifacts", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("continuous-export"),
                    rules.NameDeclaration,
                    Token("exported-artifacts"),
                    shape266));

            var ShowContinuousExportFailures = Command("ShowContinuousExportFailures", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("continuous-export"),
                    rules.NameDeclaration,
                    Token("failures"),
                    shape266));

            var ShowContinuousExport = Command("ShowContinuousExport", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("continuous-export"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    shape240));

            var ShowClusterDatabasesDataStats2 = Command("ShowClusterDatabasesDataStats", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    rules.WildcardedNameDeclaration,
                    Optional(
                        fragment57),
                    Token(")"),
                    Token("datastats"),
                    shape268));

            var ShowClusterDatabasesDetails3 = Command("ShowClusterDatabasesDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    rules.WildcardedNameDeclaration,
                    Optional(
                        fragment57),
                    Token(")"),
                    Token("details"),
                    shape268));

            var ShowClusterDatabasesIdentity2 = Command("ShowClusterDatabasesIdentity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    rules.WildcardedNameDeclaration,
                    Optional(
                        fragment57),
                    Token(")"),
                    Token("identity"),
                    shape268));

            var ShowClusterDatabasesMetadata2 = Command("ShowClusterDatabasesMetadata", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    rules.WildcardedNameDeclaration,
                    Optional(
                        fragment57),
                    Token(")"),
                    Token("metadata"),
                    shape268));

            var ShowClusterDatabasesPolicies2 = Command("ShowClusterDatabasesPolicies", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    rules.WildcardedNameDeclaration,
                    Optional(
                        fragment57),
                    Token(")"),
                    Token("policies"),
                    shape268));

            var ShowClusterDatabasesDetails4 = Command("ShowClusterDatabasesDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    rules.WildcardedNameDeclaration,
                    Optional(
                        fragment57),
                    Token(")"),
                    Token("verbose"),
                    shape268));

            var ShowClusterDatabases2 = Command("ShowClusterDatabases", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    RequiredToken("("),
                    Required(rules.WildcardedNameDeclaration, rules.MissingNameDeclaration),
                    Optional(
                        Custom(
                            Token(","),
                            Required(
                                OneOrMoreCommaList(
                                    Custom(
                                        First(
                                            rules.WildcardedNameDeclaration,
                                            rules.DatabaseNameReference),
                                        shape192),
                                    fnMissingElement: rules.MissingNameDeclaration),
                                missing28),
                            shape267)),
                    RequiredToken(")"),
                    new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.None), CD(isOptional: true), CD()}));

            var ShowClusterDatabasesDataStats3 = Command("ShowClusterDatabasesDataStats", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    rules.DatabaseNameReference,
                    Token(")"),
                    Token("datastats"),
                    shape269));

            var ShowClusterDatabasesDetails5 = Command("ShowClusterDatabasesDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    rules.DatabaseNameReference,
                    Token(")"),
                    Token("details"),
                    shape269));

            var ShowDatabaseExtentsMetadata = Command("ShowDatabaseExtentsMetadata", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("databases"),
                        Token("("),
                        rules.DatabaseNameReference,
                        Token(")"),
                        Token("extents"),
                        Optional(
                            fragment58),
                        Optional(Token("hot")),
                        Token("metadata"),
                        Optional(
                            fragment59),
                        Optional(
                            fragment55)}
                    ,
                    new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD(isOptional: true), CD(isOptional: true), CD(), CD(isOptional: true), CD(isOptional: true)}));

            var ShowDatabaseExtents = Command("ShowDatabaseExtents", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("databases"),
                        Token("("),
                        rules.DatabaseNameReference,
                        Token(")"),
                        Token("extents"),
                        Optional(
                            fragment56),
                        Optional(Token("hot")),
                        RequiredToken("where"),
                        Required(
                            OneOrMoreList(
                                fragment54,
                                separatorParser: Token("and")),
                            missing62),
                        Optional(
                            fragment55)}
                    ,
                    new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD(isOptional: true), CD(isOptional: true), CD(), CD(), CD(isOptional: true)}));

            var ShowDatabaseExtents2 = Command("ShowDatabaseExtents", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("databases"),
                        Token("("),
                        rules.DatabaseNameReference,
                        Token(")"),
                        Token("extents"),
                        Optional(
                            fragment56),
                        Optional(Token("hot")),
                        RequiredToken("with"),
                        RequiredToken("("),
                        RequiredToken("extentsShowFilteringRuntimePolicy"),
                        RequiredToken("="),
                        Required(rules.Value, rules.MissingValue),
                        RequiredToken(")")}
                    ,
                    new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD(isOptional: true), CD(isOptional: true), CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()}));

            var ShowDatabaseExtents3 = Command("ShowDatabaseExtents", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    rules.DatabaseNameReference,
                    Token(")"),
                    Token("extents"),
                    Optional(
                        fragment56),
                    Optional(Token("hot")),
                    new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD(isOptional: true), CD(isOptional: true)}));

            var ShowClusterDatabasesIdentity3 = Command("ShowClusterDatabasesIdentity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    rules.DatabaseNameReference,
                    Token(")"),
                    Token("identity"),
                    shape269));

            var ShowClusterDatabasesMetadata3 = Command("ShowClusterDatabasesMetadata", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    rules.DatabaseNameReference,
                    Token(")"),
                    Token("metadata"),
                    shape269));

            var ShowClusterDatabasesPolicies3 = Command("ShowClusterDatabasesPolicies", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    rules.DatabaseNameReference,
                    Token(")"),
                    Token("policies"),
                    shape269));

            var ShowDatabasesSchemaAsJson = Command("ShowDatabasesSchemaAsJson", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    rules.DatabaseNameReference,
                    Token(")"),
                    Token("schema"),
                    Token("as"),
                    RequiredToken("json"),
                    Optional(
                        fragment7),
                    new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD(), CD(), CD(isOptional: true)}));

            var ShowDatabasesSchemaAsJson2 = Command("ShowDatabasesSchemaAsJson", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("databases"),
                        Token("("),
                        rules.DatabaseNameReference,
                        Token(")"),
                        Token("schema"),
                        Token("details"),
                        Token("as"),
                        RequiredToken("json"),
                        Optional(
                            fragment7)}
                    ,
                    new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD(), CD(), CD(), CD(isOptional: true)}));

            var ShowDatabasesSchema = Command("ShowDatabasesSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken(")"),
                    RequiredToken("schema"),
                    RequiredToken("details"),
                    new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("Details")}));

            var ShowDatabasesSchema2 = Command("ShowDatabasesSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken(")"),
                    RequiredToken("schema"),
                    shape269));

            var ShowClusterDatabasesDetails6 = Command("ShowClusterDatabasesDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    rules.DatabaseNameReference,
                    Token(")"),
                    Token("verbose"),
                    shape269));

            var ShowClusterDatabases3 = Command("ShowClusterDatabases", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    RequiredToken("("),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken(")"),
                    new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD()}));

            var ShowClusterDatabasesDataStats4 = Command("ShowClusterDatabasesDataStats", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    rules.DatabaseNameReference,
                    Token(","),
                    OneOrMoreCommaList(
                        Custom(
                            First(
                                rules.WildcardedNameDeclaration,
                                rules.DatabaseNameReference),
                            shape192),
                        fnMissingElement: rules.MissingNameDeclaration),
                    Token(")"),
                    Token("datastats"),
                    shape270));

            var ShowClusterDatabasesDetails7 = Command("ShowClusterDatabasesDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    rules.DatabaseNameReference,
                    Token(","),
                    OneOrMoreCommaList(
                        Custom(
                            First(
                                rules.WildcardedNameDeclaration,
                                rules.DatabaseNameReference),
                            shape192),
                        fnMissingElement: rules.MissingNameDeclaration),
                    Token(")"),
                    Token("details"),
                    shape270));

            var ShowDatabaseExtentsMetadata2 = Command("ShowDatabaseExtentsMetadata", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("databases"),
                        Token("("),
                        rules.DatabaseNameReference,
                        Token(","),
                        OneOrMoreCommaList(
                            Custom(
                                rules.DatabaseNameReference,
                                shape20),
                            fnMissingElement: rules.MissingNameReference),
                        Token(")"),
                        Token("extents"),
                        Optional(
                            fragment58),
                        Optional(Token("hot")),
                        Token("metadata"),
                        Optional(
                            fragment59),
                        Optional(
                            fragment55)}
                    ,
                    new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(CompletionHint.Database), CD(), CD(), CD(isOptional: true), CD(isOptional: true), CD(), CD(isOptional: true), CD(isOptional: true)}));

            var ShowDatabaseExtents4 = Command("ShowDatabaseExtents", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("databases"),
                        Token("("),
                        rules.DatabaseNameReference,
                        Token(","),
                        OneOrMoreCommaList(
                            Custom(
                                rules.DatabaseNameReference,
                                shape20),
                            fnMissingElement: rules.MissingNameReference),
                        Token(")"),
                        Token("extents"),
                        Optional(
                            fragment56),
                        Optional(Token("hot")),
                        RequiredToken("where"),
                        Required(
                            OneOrMoreList(
                                fragment54,
                                separatorParser: Token("and")),
                            missing62),
                        Optional(
                            fragment55)}
                    ,
                    new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(CompletionHint.Database), CD(), CD(), CD(isOptional: true), CD(isOptional: true), CD(), CD(), CD(isOptional: true)}));

            var ShowDatabaseExtents5 = Command("ShowDatabaseExtents", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("databases"),
                        Token("("),
                        rules.DatabaseNameReference,
                        Token(","),
                        OneOrMoreCommaList(
                            Custom(
                                rules.DatabaseNameReference,
                                shape20),
                            fnMissingElement: rules.MissingNameReference),
                        Token(")"),
                        Token("extents"),
                        Optional(
                            fragment56),
                        Optional(Token("hot")),
                        RequiredToken("with"),
                        RequiredToken("("),
                        RequiredToken("extentsShowFilteringRuntimePolicy"),
                        RequiredToken("="),
                        Required(rules.Value, rules.MissingValue),
                        RequiredToken(")")}
                    ,
                    new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(CompletionHint.Database), CD(), CD(), CD(isOptional: true), CD(isOptional: true), CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()}));

            var ShowDatabaseExtents6 = Command("ShowDatabaseExtents", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("databases"),
                        Token("("),
                        rules.DatabaseNameReference,
                        Token(","),
                        OneOrMoreCommaList(
                            Custom(
                                rules.DatabaseNameReference,
                                shape20),
                            fnMissingElement: rules.MissingNameReference),
                        Token(")"),
                        Token("extents"),
                        Optional(
                            fragment56),
                        Optional(Token("hot"))}
                    ,
                    new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(CompletionHint.Database), CD(), CD(), CD(isOptional: true), CD(isOptional: true)}));

            var ShowClusterDatabasesIdentity4 = Command("ShowClusterDatabasesIdentity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    rules.DatabaseNameReference,
                    Token(","),
                    OneOrMoreCommaList(
                        Custom(
                            First(
                                rules.WildcardedNameDeclaration,
                                rules.DatabaseNameReference),
                            shape192),
                        fnMissingElement: rules.MissingNameDeclaration),
                    Token(")"),
                    Token("identity"),
                    shape270));

            var ShowClusterDatabasesMetadata4 = Command("ShowClusterDatabasesMetadata", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    rules.DatabaseNameReference,
                    Token(","),
                    OneOrMoreCommaList(
                        Custom(
                            First(
                                rules.WildcardedNameDeclaration,
                                rules.DatabaseNameReference),
                            shape192),
                        fnMissingElement: rules.MissingNameDeclaration),
                    Token(")"),
                    Token("metadata"),
                    shape270));

            var ShowClusterDatabasesPolicies4 = Command("ShowClusterDatabasesPolicies", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    rules.DatabaseNameReference,
                    Token(","),
                    OneOrMoreCommaList(
                        Custom(
                            First(
                                rules.WildcardedNameDeclaration,
                                rules.DatabaseNameReference),
                            shape192),
                        fnMissingElement: rules.MissingNameDeclaration),
                    Token(")"),
                    Token("policies"),
                    shape270));

            var ShowDatabasesSchemaAsJson3 = Command("ShowDatabasesSchemaAsJson", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("databases"),
                        Token("("),
                        rules.DatabaseNameReference,
                        Token(","),
                        OneOrMoreCommaList(
                            fragment60),
                        Token(")"),
                        Token("schema"),
                        Token("as"),
                        RequiredToken("json"),
                        Optional(
                            fragment7)}
                    ,
                    new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(CompletionHint.Database), CD(), CD(), CD(), CD(), CD(isOptional: true)}));

            var ShowDatabasesSchemaAsJson4 = Command("ShowDatabasesSchemaAsJson", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("databases"),
                        Token("("),
                        rules.DatabaseNameReference,
                        Token(","),
                        OneOrMoreCommaList(
                            fragment60),
                        Token(")"),
                        Token("schema"),
                        Token("details"),
                        Token("as"),
                        RequiredToken("json"),
                        Optional(
                            fragment7)}
                    ,
                    new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(CompletionHint.Database), CD(), CD(), CD(), CD(), CD(), CD(isOptional: true)}));

            var ShowDatabasesSchema3 = Command("ShowDatabasesSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken(","),
                    Required(
                        OneOrMoreCommaList(
                            fragment61),
                        missing64),
                    RequiredToken(")"),
                    RequiredToken("schema"),
                    RequiredToken("details"),
                    new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(CompletionHint.Database), CD(), CD(), CD("Details")}));

            var ShowDatabasesSchema4 = Command("ShowDatabasesSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken(","),
                    Required(
                        OneOrMoreCommaList(
                            fragment61),
                        missing64),
                    RequiredToken(")"),
                    RequiredToken("schema"),
                    new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(CompletionHint.Database), CD(), CD()}));

            var ShowClusterDatabasesDetails8 = Command("ShowClusterDatabasesDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    rules.DatabaseNameReference,
                    Token(","),
                    OneOrMoreCommaList(
                        Custom(
                            First(
                                rules.WildcardedNameDeclaration,
                                rules.DatabaseNameReference),
                            shape192),
                        fnMissingElement: rules.MissingNameDeclaration),
                    Token(")"),
                    Token("verbose"),
                    shape270));

            var ShowClusterDatabases4 = Command("ShowClusterDatabases", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    RequiredToken("("),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken(","),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                First(
                                    rules.WildcardedNameDeclaration,
                                    rules.DatabaseNameReference),
                                shape192),
                            fnMissingElement: rules.MissingNameDeclaration),
                        missing28),
                    RequiredToken(")"),
                    new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(CompletionHint.None), CD()}));

            var ShowDatabasesSchemaAsJson5 = Command("ShowDatabasesSchemaAsJson", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("databases"),
                        Token("("),
                        rules.DatabaseNameReference,
                        Token("if_later_than"),
                        rules.StringLiteral,
                        Optional(
                            fragment62),
                        Token(")"),
                        Token("schema"),
                        Token("as"),
                        RequiredToken("json"),
                        Optional(
                            fragment7)}
                    ,
                    new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD("Version", CompletionHint.Literal), CD(isOptional: true), CD(), CD(), CD(), CD(), CD(isOptional: true)}));

            var ShowDatabasesSchemaAsJson6 = Command("ShowDatabasesSchemaAsJson", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("databases"),
                        Token("("),
                        rules.DatabaseNameReference,
                        Token("if_later_than"),
                        rules.StringLiteral,
                        Optional(
                            fragment62),
                        Token(")"),
                        Token("schema"),
                        Token("details"),
                        Token("as"),
                        RequiredToken("json"),
                        Optional(
                            fragment7)}
                    ,
                    new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD("Version", CompletionHint.Literal), CD(isOptional: true), CD(), CD(), CD(), CD(), CD(), CD(isOptional: true)}));

            var ShowDatabasesSchema5 = Command("ShowDatabasesSchema", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("databases"),
                        Token("("),
                        Required(rules.DatabaseNameReference, rules.MissingNameReference),
                        RequiredToken("if_later_than"),
                        Required(rules.StringLiteral, rules.MissingStringLiteral),
                        Optional(
                            fragment63),
                        RequiredToken(")"),
                        RequiredToken("schema"),
                        RequiredToken("details")}
                    ,
                    new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD("Version", CompletionHint.Literal), CD(isOptional: true), CD(), CD(), CD("Details")}));

            var ShowDatabasesSchema6 = Command("ShowDatabasesSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("("),
                    Required(rules.DatabaseNameReference, rules.MissingNameReference),
                    RequiredToken("if_later_than"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(
                        fragment63),
                    RequiredToken(")"),
                    RequiredToken("schema"),
                    new [] {CD(), CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD("Version", CompletionHint.Literal), CD(isOptional: true), CD(), CD()}));

            var ShowClusterDatabasesDataStats5 = Command("ShowClusterDatabasesDataStats", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("datastats")));

            var ShowClusterDatabasesDetails9 = Command("ShowClusterDatabasesDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("details")));

            var ShowDatabasesEntities = Command("ShowDatabasesEntities", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("entities"),
                    Optional(
                        fragment5),
                    shape265));

            var ShowClusterDatabasesIdentity5 = Command("ShowClusterDatabasesIdentity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("identity")));

            var ShowDatabasesManagementGroups = Command("ShowDatabasesManagementGroups", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("management"),
                    RequiredToken("groups")));

            var ShowClusterDatabasesMetadata5 = Command("ShowClusterDatabasesMetadata", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("metadata")));

            var ShowClusterDatabasesPolicies5 = Command("ShowClusterDatabasesPolicies", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("policies")));

            var ShowDatabasesSchemaAsJson7 = Command("ShowDatabasesSchemaAsJson", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("schema"),
                    Token("as"),
                    RequiredToken("json"),
                    Optional(
                        fragment7),
                    shape280));

            var ShowDatabasesSchemaAsJson8 = Command("ShowDatabasesSchemaAsJson", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("schema"),
                    Token("details"),
                    Token("as"),
                    RequiredToken("json"),
                    Optional(
                        fragment7),
                    shape278));

            var ShowDatabasesSchema7 = Command("ShowDatabasesSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("schema"),
                    RequiredToken("details"),
                    new [] {CD(), CD(), CD(), CD("Details")}));

            var ShowDatabasesSchema8 = Command("ShowDatabasesSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("schema")));

            var ShowClusterDatabasesDetails10 = Command("ShowClusterDatabasesDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases"),
                    Token("verbose")));

            var ShowClusterDatabases5 = Command("ShowClusterDatabases", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("databases")));

            var ShowDatabasePolicyCaching = Command("ShowDatabasePolicyCaching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("*"),
                    Token("policy"),
                    Token("caching"),
                    shape276));

            var ShowDatabasePolicyExtentTagsRetention = Command("ShowDatabasePolicyExtentTagsRetention", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("*"),
                    Token("policy"),
                    Token("extent_tags_retention"),
                    shape276));

            var ShowDatabasePolicyIngestionBatching = Command("ShowDatabasePolicyIngestionBatching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("*"),
                    Token("policy"),
                    Token("ingestionbatching"),
                    shape276));

            var ShowDatabasePolicyMerge = Command("ShowDatabasePolicyMerge", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("*"),
                    Token("policy"),
                    Token("merge"),
                    shape276));

            var ShowDatabasePolicyRetention = Command("ShowDatabasePolicyRetention", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("*"),
                    Token("policy"),
                    Token("retention"),
                    shape276));

            var ShowDatabasePolicySharding = Command("ShowDatabasePolicySharding", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("*"),
                    Token("policy"),
                    Token("sharding"),
                    shape276));

            var ShowDatabasePolicyShardsGrouping = Command("ShowDatabasePolicyShardsGrouping", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("*"),
                    RequiredToken("policy"),
                    RequiredToken("shards_grouping").Hide(),
                    shape276));

            var ShowDatabaseCacheQueryResults = Command("ShowDatabaseCacheQueryResults", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("cache"),
                    RequiredToken("query_results")));

            var ShowDatabaseCslSchema = Command("ShowDatabaseCslSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("cslschema"),
                    Optional(
                        Custom(
                            Token("script"),
                            shape284)),
                    Optional(
                        fragment64),
                    shape277));

            var ShowDatabaseDataStats = Command("ShowDatabaseDataStats", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("datastats"),
                    Optional(
                        fragment7),
                    shape265));

            var ShowDatabaseDetails = Command("ShowDatabaseDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("details"),
                    Optional(
                        fragment7),
                    shape288));

            var ShowDatabaseExtentsMetadata3 = Command("ShowDatabaseExtentsMetadata", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("database"),
                        Token("extents"),
                        Token("("),
                        OneOrMoreCommaList(
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                shape224),
                            fnMissingElement: rules.MissingValue),
                        Token(")"),
                        Optional(Token("hot")),
                        Token("metadata"),
                        Optional(
                            fragment59),
                        Optional(
                            fragment55)}
                    ,
                    new [] {CD(), CD(), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true), CD(), CD(isOptional: true), CD(isOptional: true)}));

            var ShowDatabaseExtents7 = Command("ShowDatabaseExtents", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("database"),
                        Token("extents"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.AnyGuidLiteralOrString,
                                    shape224),
                                fnMissingElement: rules.MissingValue),
                            missing53),
                        RequiredToken(")"),
                        Optional(Token("hot")),
                        RequiredToken("where"),
                        Required(
                            OneOrMoreList(
                                fragment54,
                                separatorParser: Token("and")),
                            missing62),
                        Optional(
                            fragment55)}
                    ,
                    new [] {CD(), CD(), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true), CD(), CD(), CD(isOptional: true)}));

            var ShowDatabaseExtents8 = Command("ShowDatabaseExtents", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("database"),
                        Token("extents"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.AnyGuidLiteralOrString,
                                    shape224),
                                fnMissingElement: rules.MissingValue),
                            missing53),
                        RequiredToken(")"),
                        Optional(Token("hot")),
                        RequiredToken("with"),
                        RequiredToken("("),
                        RequiredToken("extentsShowFilteringRuntimePolicy"),
                        RequiredToken("="),
                        Required(rules.Value, rules.MissingValue),
                        RequiredToken(")")}
                    ,
                    new [] {CD(), CD(), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true), CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()}));

            var ShowDatabaseExtents9 = Command("ShowDatabaseExtents", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("extents"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                shape224),
                            fnMissingElement: rules.MissingValue),
                        missing53),
                    RequiredToken(")"),
                    Optional(Token("hot")),
                    new [] {CD(), CD(), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var ShowDatabaseExtentsMetadata4 = Command("ShowDatabaseExtentsMetadata", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("extents"),
                    Token("hot"),
                    Token("metadata"),
                    Optional(
                        fragment59),
                    Optional(
                        fragment55),
                    shape285));

            var ShowDatabaseExtents10 = Command("ShowDatabaseExtents", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("extents"),
                    RequiredToken("hot"),
                    RequiredToken("where"),
                    Required(
                        OneOrMoreList(
                            fragment54,
                            separatorParser: Token("and")),
                        missing62),
                    Optional(
                        fragment55),
                    shape278));

            var ShowDatabaseExtents11 = Command("ShowDatabaseExtents", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("database"),
                        Token("extents"),
                        RequiredToken("hot"),
                        RequiredToken("with"),
                        RequiredToken("("),
                        RequiredToken("extentsShowFilteringRuntimePolicy"),
                        RequiredToken("="),
                        Required(rules.Value, rules.MissingValue),
                        RequiredToken(")")}
                    ,
                    new [] {CD(), CD(), CD(), CD(), CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()}));

            var ShowDatabaseExtents12 = Command("ShowDatabaseExtents", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("extents"),
                    RequiredToken("hot")));

            var ShowDatabaseExtentsMetadata5 = Command("ShowDatabaseExtentsMetadata", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("extents"),
                    Token("metadata"),
                    Optional(
                        fragment59),
                    Optional(
                        fragment55),
                    shape279));

            var ShowDatabaseExtentsPartitioningStatistics = Command("ShowDatabaseExtentsPartitioningStatistics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("extents"),
                    Token("partitioning"),
                    RequiredToken("statistics")));

            var ShowDatabaseExtents13 = Command("ShowDatabaseExtents", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("extents"),
                    RequiredToken("where"),
                    Required(
                        OneOrMoreList(
                            fragment54,
                            separatorParser: Token("and")),
                        missing62),
                    Optional(
                        fragment55),
                    shape280));

            var ShowDatabaseExtents14 = Command("ShowDatabaseExtents", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("extents"),
                    RequiredToken("with"),
                    RequiredToken("("),
                    RequiredToken("extentsShowFilteringRuntimePolicy"),
                    RequiredToken("="),
                    Required(rules.Value, rules.MissingValue),
                    RequiredToken(")"),
                    new [] {CD(), CD(), CD(), CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()}));

            var ShowDatabaseExtents15 = Command("ShowDatabaseExtents", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("extents")));

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
                            new [] {CD(), CD(), CD(), CD(), CD("minCreationTime", CompletionHint.Literal), CD()})),
                    shape280));

            var ShowDatabaseIdentity = Command("ShowDatabaseIdentity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("identity"),
                    Optional(
                        fragment7),
                    shape265));

            var ShowDatabaseIngestionMappings = Command("ShowDatabaseIngestionMappings", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("ingestion"),
                    Optional(
                        Custom(
                            Token("apacheavro", "avro", "csv", "json", "orc", "parquet", "sstream", "w3clogfile"),
                            shape50)),
                    RequiredToken("mappings"),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape281)),
                    Optional(
                        fragment7),
                    new [] {CD(), CD(), CD(), CD(isOptional: true), CD(), CD(CompletionHint.Literal, isOptional: true), CD(isOptional: true)}));

            var ShowDatabaseKeyVaultSecrets = Command("ShowDatabaseKeyVaultSecrets", 
                Custom(
                    Token("show"),
                    Token("database"),
                    Token("keyvault"),
                    RequiredToken("secrets")).Hide());

            var ShowDatabaseCslSchema2 = Command("ShowDatabaseCslSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("kqlschema"),
                    Optional(
                        Custom(
                            Token("script"),
                            shape284)),
                    Optional(
                        fragment64),
                    shape277));

            var ShowDatabaseMetadata = Command("ShowDatabaseMetadata", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("metadata"),
                    Optional(
                        fragment7),
                    shape265));

            var ShowDatabasePolicies = Command("ShowDatabasePolicies", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policies"),
                    Optional(
                        fragment7),
                    shape265));

            var ShowDatabasePolicyCaching2 = Command("ShowDatabasePolicyCaching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    Token("caching")));

            var ShowDatabasePolicyExtentTagsRetention2 = Command("ShowDatabasePolicyExtentTagsRetention", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    Token("extent_tags_retention")));

            var ShowDatabasePolicyIngestionBatching2 = Command("ShowDatabasePolicyIngestionBatching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    Token("ingestionbatching")));

            var ShowDatabasePolicyMerge2 = Command("ShowDatabasePolicyMerge", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    Token("merge")));

            var ShowDatabasePolicyRetention2 = Command("ShowDatabasePolicyRetention", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    Token("retention")));

            var ShowDatabasePolicySharding2 = Command("ShowDatabasePolicySharding", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    Token("sharding")));

            var ShowDatabasePolicyShardsGrouping2 = Command("ShowDatabasePolicyShardsGrouping", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("policy"),
                    RequiredToken("shards_grouping").Hide()));

            var ShowDatabasePrincipals = Command("ShowDatabasePrincipals", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("principals")));

            var ShowDatabaseSchemaAsCslScript = Command("ShowDatabaseSchemaAsCslScript", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("schema"),
                    Token("as"),
                    RequiredToken("csl"),
                    Optional(
                        Custom(
                            Token("script"),
                            shape284)),
                    Optional(
                        fragment7),
                    shape285));

            var ShowDatabaseSchemaAsJson = Command("ShowDatabaseSchemaAsJson", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("schema"),
                    Token("as"),
                    Token("json"),
                    Optional(
                        fragment7),
                    shape280));

            var ShowDatabaseSchemaAsCslScript2 = Command("ShowDatabaseSchemaAsCslScript", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("schema"),
                    Token("as"),
                    RequiredToken("kql"),
                    Optional(
                        Custom(
                            Token("script"),
                            shape284)),
                    Optional(
                        fragment7),
                    shape285));

            var ShowDatabaseSchema = Command("ShowDatabaseSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("schema"),
                    RequiredToken("details"),
                    Optional(
                        fragment65),
                    new [] {CD(), CD(), CD(), CD("Details"), CD(isOptional: true)}));

            var ShowDatabaseSchemaAsCslScript3 = Command("ShowDatabaseSchemaAsCslScript", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("schema"),
                    Token("if_later_than"),
                    rules.StringLiteral,
                    Token("as"),
                    RequiredToken("csl"),
                    Optional(
                        Custom(
                            Token("script"),
                            shape284)),
                    Optional(
                        fragment7),
                    shape286));

            var ShowDatabaseSchemaAsJson2 = Command("ShowDatabaseSchemaAsJson", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("schema"),
                    Token("if_later_than"),
                    rules.StringLiteral,
                    Token("as"),
                    Token("json"),
                    Optional(
                        fragment7),
                    new [] {CD(), CD(), CD(), CD(), CD("Version", CompletionHint.Literal), CD(), CD(), CD(isOptional: true)}));

            var ShowDatabaseSchemaAsCslScript4 = Command("ShowDatabaseSchemaAsCslScript", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("schema"),
                    Token("if_later_than"),
                    rules.StringLiteral,
                    Token("as"),
                    RequiredToken("kql"),
                    Optional(
                        Custom(
                            Token("script"),
                            shape284)),
                    Optional(
                        fragment7),
                    shape286));

            var ShowDatabaseSchema2 = Command("ShowDatabaseSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("schema"),
                    RequiredToken("if_later_than"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD(), CD(), CD("Version", CompletionHint.Literal)}));

            var ShowDatabaseSchemaViolations = Command("ShowDatabaseSchemaViolations", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("schema"),
                    Token("violations")));

            var ShowDatabaseSchema3 = Command("ShowDatabaseSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("schema")));

            var DatabaseShardGroupsStatisticsShow = Command("DatabaseShardGroupsStatisticsShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("shard-groups").Hide(),
                    RequiredToken("statistics").Hide()));

            var ShowStorageArtifactsCleanupState = Command("ShowStorageArtifactsCleanupState", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("storage"),
                    RequiredToken("artifacts"),
                    RequiredToken("cleanup"),
                    RequiredToken("state"),
                    Optional(
                        fragment5),
                    shape278));

            var ShowDatabaseDetails2 = Command("ShowDatabaseDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    Token("verbose"),
                    Optional(
                        fragment7),
                    shape288));

            var ShowDatabase = Command("ShowDatabase", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    RequiredToken("with"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            fragment38),
                        missing11),
                    RequiredToken(")"),
                    new [] {CD(), CD(), CD(), CD(), CD(CompletionHint.None), CD()}));

            var ShowDatabaseCslSchema3 = Command("ShowDatabaseCslSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("cslschema"),
                    Optional(
                        Custom(
                            Token("script"),
                            shape284)),
                    Optional(
                        fragment64),
                    shape290));

            var ShowDatabaseEntity = Command("ShowDatabaseEntity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("entity"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Optional(
                        fragment5),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD("EntityName", CompletionHint.None), CD(isOptional: true)}));

            var ShowDatabaseExtentContainersCleanOperations = Command("ShowDatabaseExtentContainersCleanOperations", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("extentcontainers"),
                    RequiredToken("clean"),
                    RequiredToken("operations"),
                    Optional(
                        Custom(
                            rules.AnyGuidLiteralOrString,
                            shape289)),
                    new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(), CD(), CD(CompletionHint.Literal, isOptional: true)}));

            var ShowDatabaseExtentsMetadata6 = Command("ShowDatabaseExtentsMetadata", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("database"),
                        If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                        Token("extents"),
                        Token("("),
                        OneOrMoreCommaList(
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                shape224),
                            fnMissingElement: rules.MissingValue),
                        Token(")"),
                        Optional(Token("hot")),
                        Token("metadata"),
                        Optional(
                            fragment59),
                        Optional(
                            fragment55)}
                    ,
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true), CD(), CD(isOptional: true), CD(isOptional: true)}));

            var ShowDatabaseExtents16 = Command("ShowDatabaseExtents", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("database"),
                        If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                        Token("extents"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.AnyGuidLiteralOrString,
                                    shape224),
                                fnMissingElement: rules.MissingValue),
                            missing53),
                        RequiredToken(")"),
                        Optional(Token("hot")),
                        RequiredToken("where"),
                        Required(
                            OneOrMoreList(
                                fragment54,
                                separatorParser: Token("and")),
                            missing62),
                        Optional(
                            fragment55)}
                    ,
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true), CD(), CD(), CD(isOptional: true)}));

            var ShowDatabaseExtents17 = Command("ShowDatabaseExtents", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("database"),
                        If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                        Token("extents"),
                        RequiredToken("("),
                        Required(
                            OneOrMoreCommaList(
                                Custom(
                                    rules.AnyGuidLiteralOrString,
                                    shape224),
                                fnMissingElement: rules.MissingValue),
                            missing53),
                        RequiredToken(")"),
                        Optional(Token("hot")),
                        RequiredToken("with"),
                        RequiredToken("("),
                        RequiredToken("extentsShowFilteringRuntimePolicy"),
                        RequiredToken("="),
                        Required(rules.Value, rules.MissingValue),
                        RequiredToken(")")}
                    ,
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true), CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()}));

            var ShowDatabaseExtents18 = Command("ShowDatabaseExtents", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("extents"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                shape224),
                            fnMissingElement: rules.MissingValue),
                        missing53),
                    RequiredToken(")"),
                    Optional(Token("hot")),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD(CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var ShowDatabaseExtentsMetadata7 = Command("ShowDatabaseExtentsMetadata", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("extents"),
                    Token("hot"),
                    Token("metadata"),
                    Optional(
                        fragment59),
                    Optional(
                        fragment55),
                    shape294));

            var ShowDatabaseExtents19 = Command("ShowDatabaseExtents", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("extents"),
                    RequiredToken("hot"),
                    RequiredToken("where"),
                    Required(
                        OneOrMoreList(
                            fragment54,
                            separatorParser: Token("and")),
                        missing62),
                    Optional(
                        fragment55),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD(), CD(), CD(isOptional: true)}));

            var ShowDatabaseExtents20 = Command("ShowDatabaseExtents", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("database"),
                        If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                        Token("extents"),
                        RequiredToken("hot"),
                        RequiredToken("with"),
                        RequiredToken("("),
                        RequiredToken("extentsShowFilteringRuntimePolicy"),
                        RequiredToken("="),
                        Required(rules.Value, rules.MissingValue),
                        RequiredToken(")")}
                    ,
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()}));

            var ShowDatabaseExtents21 = Command("ShowDatabaseExtents", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("extents"),
                    RequiredToken("hot"),
                    shape203));

            var ShowDatabaseExtentsMetadata8 = Command("ShowDatabaseExtentsMetadata", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("extents"),
                    Token("metadata"),
                    Optional(
                        fragment59),
                    Optional(
                        fragment55),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD(isOptional: true), CD(isOptional: true)}));

            var ShowDatabaseExtentsPartitioningStatistics2 = Command("ShowDatabaseExtentsPartitioningStatistics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("extents"),
                    Token("partitioning"),
                    RequiredToken("statistics"),
                    shape73));

            var ShowDatabaseExtents22 = Command("ShowDatabaseExtents", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("extents"),
                    RequiredToken("where"),
                    Required(
                        OneOrMoreList(
                            fragment54,
                            separatorParser: Token("and")),
                        missing62),
                    Optional(
                        fragment55),
                    shape295));

            var ShowDatabaseExtents23 = Command("ShowDatabaseExtents", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("database"),
                        If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                        Token("extents"),
                        RequiredToken("with"),
                        RequiredToken("("),
                        RequiredToken("extentsShowFilteringRuntimePolicy"),
                        RequiredToken("="),
                        Required(rules.Value, rules.MissingValue),
                        RequiredToken(")")}
                    ,
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()}));

            var ShowDatabaseExtents24 = Command("ShowDatabaseExtents", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("extents"),
                    shape216));

            var ShowDatabaseIngestionMappings2 = Command("ShowDatabaseIngestionMappings", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("ingestion"),
                    Optional(
                        Custom(
                            Token("apacheavro", "avro", "csv", "json", "orc", "parquet", "sstream", "w3clogfile"),
                            shape50)),
                    RequiredToken("mappings"),
                    Optional(
                        Custom(
                            rules.StringLiteral,
                            shape281)),
                    Optional(
                        fragment7),
                    new [] {CD(), CD(), CD("databaseName", CompletionHint.Database), CD(), CD(isOptional: true), CD(), CD(CompletionHint.Literal, isOptional: true), CD(isOptional: true)}));

            var ShowDatabaseJournal = Command("ShowDatabaseJournal", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("journal"),
                    shape216));

            var ShowDatabaseCslSchema4 = Command("ShowDatabaseCslSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("kqlschema"),
                    Optional(
                        Custom(
                            Token("script"),
                            shape284)),
                    Optional(
                        fragment64),
                    shape290));

            var ShowDatabasePolicyCaching3 = Command("ShowDatabasePolicyCaching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("caching"),
                    shape203));

            var ShowDatabasePolicyDiagnostics = Command("ShowDatabasePolicyDiagnostics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("diagnostics"),
                    shape203));

            var ShowDatabasePolicyEncoding = Command("ShowDatabasePolicyEncoding", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("encoding"),
                    shape203));

            var ShowDatabasePolicyExtentTagsRetention3 = Command("ShowDatabasePolicyExtentTagsRetention", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("extent_tags_retention"),
                    shape203));

            var ShowDatabasePolicyHardRetentionViolations = Command("ShowDatabasePolicyHardRetentionViolations", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("hardretention"),
                    RequiredToken("violations"),
                    shape291));

            var ShowDatabasePolicyIngestionBatching3 = Command("ShowDatabasePolicyIngestionBatching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("ingestionbatching"),
                    shape203));

            var ShowDatabasePolicyManagedIdentity = Command("ShowDatabasePolicyManagedIdentity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("managed_identity"),
                    shape203));

            var ShowDatabasePolicyMerge3 = Command("ShowDatabasePolicyMerge", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("merge"),
                    shape203));

            var ShowDatabasePolicyMirroring = Command("ShowDatabasePolicyMirroring", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("mirroring"),
                    shape203));

            var ShowDatabasePolicyRetention3 = Command("ShowDatabasePolicyRetention", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("retention"),
                    shape203));

            var ShowDatabasePolicySharding3 = Command("ShowDatabasePolicySharding", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("sharding"),
                    shape203));

            var ShowDatabasePolicyShardsGrouping3 = Command("ShowDatabasePolicyShardsGrouping", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("policy"),
                    RequiredToken("shards_grouping").Hide(),
                    shape203));

            var ShowDatabasePolicySoftRetentionViolations = Command("ShowDatabasePolicySoftRetentionViolations", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("softretention"),
                    RequiredToken("violations"),
                    shape291));

            var ShowDatabasePolicyStreamingIngestion = Command("ShowDatabasePolicyStreamingIngestion", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("policy"),
                    Token("streamingingestion"),
                    shape203));

            var ShowDatabaseTableMirroringPolicyTemplates = Command("ShowDatabaseTableMirroringPolicyTemplates", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("policy"),
                    RequiredToken("table_mirroring_templates"),
                    shape203));

            var ShowDatabasePrincipals2 = Command("ShowDatabasePrincipals", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("principals"),
                    shape216));

            var ShowDatabasePrincipalRoles = Command("ShowDatabasePrincipalRoles", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("principal"),
                    Required(
                        First(
                            Token("roles"),
                            fragment66),
                        missing63),
                    Optional(
                        fragment7),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD(isOptional: true)}));

            var ShowDatabasePurgeOperation = Command("ShowDatabasePurgeOperation", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("purge"),
                    Required(
                        First(
                            Custom(
                                Token("operations"),
                                Optional(
                                    Custom(
                                        rules.AnyGuidLiteralOrString,
                                        shape289)),
                                shape99),
                            Custom(
                                Token("operation"),
                                Required(rules.AnyGuidLiteralOrString, rules.MissingValue),
                                new [] {CD(), CD("obj", CompletionHint.Literal)})),
                        missing65),
                    shape293));

            var ShowDatabaseSchemaAsCslScript5 = Command("ShowDatabaseSchemaAsCslScript", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("schema"),
                    Token("as"),
                    RequiredToken("csl"),
                    Optional(
                        Custom(
                            Token("script"),
                            shape284)),
                    Optional(
                        fragment7),
                    shape294));

            var ShowDatabaseSchemaAsJson3 = Command("ShowDatabaseSchemaAsJson", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("schema"),
                    Token("as"),
                    Token("json"),
                    Optional(
                        fragment7),
                    shape295));

            var ShowDatabaseSchemaAsCslScript6 = Command("ShowDatabaseSchemaAsCslScript", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("schema"),
                    Token("as"),
                    RequiredToken("kql"),
                    Optional(
                        Custom(
                            Token("script"),
                            shape284)),
                    Optional(
                        fragment7),
                    shape294));

            var ShowDatabaseSchema4 = Command("ShowDatabaseSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("schema"),
                    RequiredToken("details"),
                    Optional(
                        fragment65),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD("Details"), CD(isOptional: true)}));

            var ShowDatabaseSchemaAsCslScript7 = Command("ShowDatabaseSchemaAsCslScript", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("database"),
                        If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                        Token("schema"),
                        Token("if_later_than"),
                        rules.StringLiteral,
                        Token("as"),
                        RequiredToken("csl"),
                        Optional(
                            Custom(
                                Token("script"),
                                shape284)),
                        Optional(
                            fragment7)}
                    ,
                    shape296));

            var ShowDatabaseSchemaAsJson4 = Command("ShowDatabaseSchemaAsJson", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("schema"),
                    Token("if_later_than"),
                    rules.StringLiteral,
                    Token("as"),
                    Token("json"),
                    Optional(
                        fragment7),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("Version", CompletionHint.Literal), CD(), CD(), CD(isOptional: true)}));

            var ShowDatabaseSchemaAsCslScript8 = Command("ShowDatabaseSchemaAsCslScript", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("database"),
                        If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                        Token("schema"),
                        Token("if_later_than"),
                        rules.StringLiteral,
                        Token("as"),
                        RequiredToken("kql"),
                        Optional(
                            Custom(
                                Token("script"),
                                shape284)),
                        Optional(
                            fragment7)}
                    ,
                    shape296));

            var ShowDatabaseSchema5 = Command("ShowDatabaseSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("schema"),
                    RequiredToken("if_later_than"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("DatabaseName", CompletionHint.Database), CD(), CD(), CD("Version", CompletionHint.Literal)}));

            var ShowDatabaseSchemaViolations2 = Command("ShowDatabaseSchemaViolations", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("schema"),
                    Token("violations"),
                    shape203));

            var ShowDatabaseSchema6 = Command("ShowDatabaseSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    Token("schema"),
                    shape216));

            var DatabaseShardGroupsStatisticsShow2 = Command("DatabaseShardGroupsStatisticsShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database"),
                    If(Not(And(Token("*", "cache", "cslschema", "datastats", "details", "extents", "extent", "identity", "ingestion", "keyvault", "kqlschema", "metadata", "policies", "policy", "principals", "schema", "shard-groups", "storage", "verbose", "with"))), rules.DatabaseNameReference),
                    RequiredToken("shard-groups").Hide(),
                    RequiredToken("statistics").Hide(),
                    shape203));

            var ShowDatabase2 = Command("ShowDatabase", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("database")));

            var ShowDataOperations = Command("ShowDataOperations", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("data"),
                    RequiredToken("operations")));

            var ShowDiagnostics = Command("ShowDiagnostics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("diagnostics"),
                    Optional(
                        fragment67),
                    shape198));

            var ShowEntityGroups = Command("ShowEntityGroups", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("entity_groups")));

            var ShowEntityGroup = Command("ShowEntityGroup", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("entity_group"),
                    Required(rules.EntityGroupNameReference, rules.MissingNameReference),
                    shape299));

            var ShowEntitySchema = Command("ShowEntitySchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("entity"),
                    Required(rules.QualifiedNameDeclaration, rules.MissingNameDeclaration),
                    RequiredToken("schema"),
                    RequiredToken("as"),
                    RequiredToken("json"),
                    Optional(
                        Custom(
                            Token("in"),
                            RequiredToken("databases"),
                            RequiredToken("("),
                            Required(
                                OneOrMoreCommaList(
                                    Custom(
                                        rules.StringLiteral,
                                        CD("item", CompletionHint.Literal)),
                                    fnMissingElement: rules.MissingStringLiteral),
                                missing0),
                            RequiredToken(")"),
                            new [] {CD(), CD(), CD(), CD(CompletionHint.Literal), CD()})),
                    Optional(
                        Custom(
                            Token("except"),
                            Required(rules.StringLiteral, rules.MissingStringLiteral),
                            new [] {CD(), CD("excludedFunctions", CompletionHint.Literal)})),
                    Optional(
                        fragment7),
                    new [] {CD(), CD(), CD("entity", CompletionHint.None), CD(), CD(), CD(), CD(isOptional: true), CD(isOptional: true), CD(isOptional: true)}));

            var ShowExtentContainers = Command("ShowExtentContainers", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("extentcontainers"),
                    Optional(
                        fragment7),
                    shape198));

            var ShowExtentCorruptedDatetime = Command("ShowExtentCorruptedDatetime", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("extents"),
                    Required(
                        fragment70,
                        missing66).Hide()));

            var ShowExternalTablesQueryAccelerationStatatistics = Command("ShowExternalTablesQueryAccelerationStatatistics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("tables"),
                    Token("operations"),
                    RequiredToken("query_acceleration"),
                    RequiredToken("statistics")));

            var ShowExternalTables = Command("ShowExternalTables", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("tables"),
                    RequiredToken("with"),
                    RequiredToken("("),
                    ZeroOrMoreCommaList(
                        fragment1),
                    RequiredToken(")"),
                    new [] {CD(), CD(), CD(), CD(), CD(), CD(CompletionHint.None), CD()}));

            var ShowExternalTables2 = Command("ShowExternalTables", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("tables")));

            var ShowExternalTablesPolicyQueryAcceleration = Command("ShowExternalTablesPolicyQueryAcceleration", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    Token("*"),
                    RequiredToken("policy"),
                    RequiredToken("query_acceleration")));

            var ShowExternalTableArtifacts = Command("ShowExternalTableArtifacts", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    If(Not(Token("*")), rules.ExternalTableNameReference),
                    Token("artifacts"),
                    Optional(
                        fragment44),
                    new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD(isOptional: true)}));

            var ShowExternalTableCslSchema = Command("ShowExternalTableCslSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    If(Not(Token("*")), rules.ExternalTableNameReference),
                    Token("cslschema"),
                    shape300));

            var ShowExternalTableCslSchema2 = Command("ShowExternalTableCslSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    If(Not(Token("*")), rules.ExternalTableNameReference),
                    Token("kqlschema"),
                    shape300));

            var ShowExternalTableMappings = Command("ShowExternalTableMappings", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    If(Not(Token("*")), rules.ExternalTableNameReference),
                    Token("mappings"),
                    shape300));

            var ShowExternalTableMapping = Command("ShowExternalTableMapping", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    If(Not(Token("*")), rules.ExternalTableNameReference),
                    Token("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    shape301));

            var ShowExternalTableQueryAccelerationStatatistics = Command("ShowExternalTableQueryAccelerationStatatistics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    If(Not(Token("*")), rules.ExternalTableNameReference),
                    Token("operations"),
                    RequiredToken("query_acceleration"),
                    RequiredToken("statistics"),
                    shape303));

            var ShowExternalTablePolicyQueryAcceleration = Command("ShowExternalTablePolicyQueryAcceleration", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    If(Not(Token("*")), rules.ExternalTableNameReference),
                    Token("policy"),
                    RequiredToken("query_acceleration"),
                    shape302));

            var ShowExternalTablePrincipals = Command("ShowExternalTablePrincipals", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    If(Not(Token("*")), rules.ExternalTableNameReference),
                    Token("principals"),
                    new [] {CD(), CD(), CD(), CD("tableName", CompletionHint.ExternalTable), CD()}));

            var ShowExternalTablesPrincipalRoles = Command("ShowExternalTablesPrincipalRoles", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    If(Not(Token("*")), rules.ExternalTableNameReference),
                    Token("principal"),
                    Required(
                        First(
                            Token("roles"),
                            fragment66),
                        missing63),
                    Optional(
                        fragment7),
                    new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD(), CD(isOptional: true)}));

            var ShowExternalTableSchema = Command("ShowExternalTableSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    Token("table"),
                    If(Not(Token("*")), rules.ExternalTableNameReference),
                    Token("schema"),
                    RequiredToken("as"),
                    RequiredToken("csl", "json", "kql"),
                    shape303));

            var ShowExternalTable = Command("ShowExternalTable", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    RequiredToken("table"),
                    Required(If(Not(Token("*")), rules.ExternalTableNameReference), rules.MissingNameReference),
                    RequiredToken("with"),
                    RequiredToken("("),
                    ZeroOrMoreCommaList(
                        fragment1),
                    RequiredToken(")"),
                    new [] {CD(), CD(), CD(), CD("ExternalTableName", CompletionHint.ExternalTable), CD(), CD(), CD(CompletionHint.None), CD()}));

            var ShowExternalTable2 = Command("ShowExternalTable", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("external"),
                    RequiredToken("table"),
                    Required(If(Not(Token("*")), rules.ExternalTableNameReference), rules.MissingNameReference),
                    shape304));

            var ShowFabricCache = Command("ShowFabricCache", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("fabriccache")));

            var ShowFabricClocks = Command("ShowFabricClocks", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("fabricclocks")));

            var ShowFabricLocks = Command("ShowFabricLocks", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("fabriclocks")));

            var ShowFabric = Command("ShowFabric", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("fabric"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    new [] {CD(), CD(), CD("id", CompletionHint.None)}));

            var ShowFeatureFlags = Command("ShowFeatureFlags", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("featureflags")));

            var ShowFollowerDatabase = Command("ShowFollowerDatabase", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("follower"),
                    Required(
                        First(
                            Custom(
                                Token("databases"),
                                Optional(
                                    fragment68),
                                shape254),
                            Custom(
                                Token("database"),
                                Required(rules.DatabaseNameReference, rules.MissingNameReference),
                                new [] {CD(), CD("databaseName", CompletionHint.Database)})),
                        missing67)));

            var ShowFreshness = Command("ShowFreshness", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("freshness").Hide(),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    Optional(
                        Custom(
                            Token("column"),
                            Required(rules.ColumnNameReference, rules.MissingNameReference),
                            new [] {CD(), CD("columnName", CompletionHint.Column)})),
                    Optional(
                        Custom(
                            Token("threshold"),
                            Required(rules.Value, rules.MissingValue),
                            new [] {CD(), CD("threshold", CompletionHint.Literal)})),
                    new [] {CD(), CD(), CD("tableName", CompletionHint.Table), CD(isOptional: true), CD(isOptional: true)}));

            var ShowFunctions = Command("ShowFunctions", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("functions")));

            var ShowFunctionPrincipals = Command("ShowFunctionPrincipals", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("function"),
                    rules.FunctionNameReference,
                    Token("principals"),
                    shape232));

            var ShowFunctionPrincipalRoles = Command("ShowFunctionPrincipalRoles", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("function"),
                    rules.FunctionNameReference,
                    Token("principal"),
                    Required(
                        First(
                            Token("roles"),
                            fragment66),
                        missing63),
                    Optional(
                        fragment7),
                    new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD(), CD(), CD(isOptional: true)}));

            var ShowFunctionSchemaAsJson = Command("ShowFunctionSchemaAsJson", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("function"),
                    rules.FunctionNameReference,
                    Token("schema"),
                    RequiredToken("as"),
                    RequiredToken("json"),
                    Optional(
                        fragment5),
                    new [] {CD(), CD(), CD("functionName", CompletionHint.Function), CD(), CD(), CD(), CD(isOptional: true)}));

            var ShowFunction = Command("ShowFunction", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("function"),
                    Required(rules.FunctionNameReference, rules.MissingNameReference),
                    RequiredToken("with"),
                    RequiredToken("("),
                    ZeroOrMoreCommaList(
                        fragment1),
                    RequiredToken(")"),
                    new [] {CD(), CD(), CD("FunctionName", CompletionHint.Function), CD(), CD(), CD(CompletionHint.None), CD()}));

            var ShowFunction2 = Command("ShowFunction", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("function"),
                    Required(rules.FunctionNameReference, rules.MissingNameReference),
                    shape306));

            var GraphModelsShow = Command("GraphModelsShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("graph_models").Hide(),
                    Optional(Token("details")),
                    Optional(
                        fragment5),
                    shape307));

            var GraphModelShow = Command("GraphModelShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("graph_model").Hide(),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Optional(Token("details")),
                    Optional(
                        fragment5),
                    new [] {CD(), CD(), CD(CompletionHint.None), CD(isOptional: true), CD(isOptional: true)}));

            var GraphSnapshotsShow = Command("GraphSnapshotsShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("graph_snapshots").Hide(),
                    Required(rules.WildcardedNameDeclaration, rules.MissingNameDeclaration),
                    Optional(Token("details")),
                    shape200));

            var GraphSnapshotShow = Command("GraphSnapshotShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("graph_snapshot").Hide(),
                    Required(rules.QualifiedNameDeclaration, rules.MissingNameDeclaration),
                    Optional(Token("details")),
                    shape200));

            var ShowIngestionMappings2 = Command("ShowIngestionMappings", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("ingestion"),
                    RequiredToken("apacheavro"),
                    RequiredToken("mappings"),
                    Optional(
                        fragment7),
                    shape308));

            var ShowIngestionMappings3 = Command("ShowIngestionMappings", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("ingestion"),
                    RequiredToken("avro"),
                    RequiredToken("mappings"),
                    Optional(
                        fragment7),
                    shape308));

            var ShowIngestionMappings4 = Command("ShowIngestionMappings", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("ingestion"),
                    RequiredToken("csv"),
                    RequiredToken("mappings"),
                    Optional(
                        fragment7),
                    shape308));

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
                            new [] {CD(), CD(), CD(), CD(), CD("OperationId", CompletionHint.Literal), CD()})),
                    shape265));

            var ShowIngestionMappings5 = Command("ShowIngestionMappings", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("ingestion"),
                    RequiredToken("json"),
                    RequiredToken("mappings"),
                    Optional(
                        fragment7),
                    shape308));

            var ShowIngestionMappings6 = Command("ShowIngestionMappings", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("ingestion"),
                    RequiredToken("mappings"),
                    Optional(
                        fragment7),
                    shape265));

            var ShowIngestionMappings7 = Command("ShowIngestionMappings", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("ingestion"),
                    RequiredToken("orc"),
                    RequiredToken("mappings"),
                    Optional(
                        fragment7),
                    shape308));

            var ShowIngestionMappings8 = Command("ShowIngestionMappings", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("ingestion"),
                    RequiredToken("parquet"),
                    RequiredToken("mappings"),
                    Optional(
                        fragment7),
                    shape308));

            var ShowIngestionMappings9 = Command("ShowIngestionMappings", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("ingestion"),
                    RequiredToken("sstream"),
                    RequiredToken("mappings"),
                    Optional(
                        fragment7),
                    shape308));

            var ShowIngestionMappings10 = Command("ShowIngestionMappings", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("ingestion"),
                    RequiredToken("w3clogfile"),
                    RequiredToken("mappings"),
                    Optional(
                        fragment7),
                    shape308));

            var ShowJournal = Command("ShowJournal", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("journal")));

            var ShowMaterializedViewsDetails = Command("ShowMaterializedViewsDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-views"),
                    Token("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.MaterializedViewNameReference,
                                shape33),
                            fnMissingElement: rules.MissingNameReference),
                        missing32),
                    RequiredToken(")"),
                    RequiredToken("details"),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.MaterializedView), CD(), CD()}));

            var ShowMaterializedViewsDetails2 = Command("ShowMaterializedViewsDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-views"),
                    Token("details")));

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
                    shape309));

            var ShowMaterializedViewDetails = Command("ShowMaterializedViewDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("details"),
                    shape309));

            var ShowMaterializedViewDiagnostics = Command("ShowMaterializedViewDiagnostics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("diagnostics"),
                    Optional(
                        fragment7),
                    shape310));

            var ShowMaterializedViewExtents = Command("ShowMaterializedViewExtents", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("extents"),
                    Optional(
                        fragment56),
                    Optional(Token("hot")),
                    new [] {CD(), CD(), CD("MaterializedViewName", CompletionHint.MaterializedView), CD(), CD(isOptional: true), CD(isOptional: true)}));

            var ShowMaterializedViewFailures = Command("ShowMaterializedViewFailures", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("failures"),
                    shape311));

            var ShowMaterializedViewCslSchema2 = Command("ShowMaterializedViewCslSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("kqlschema"),
                    shape309));

            var ShowMaterializedViewPolicyMerge = Command("ShowMaterializedViewPolicyMerge", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("policy"),
                    Token("merge"),
                    shape207));

            var ShowMaterializedViewPolicyPartitioning = Command("ShowMaterializedViewPolicyPartitioning", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("policy"),
                    Token("partitioning"),
                    shape207));

            var ShowMaterializedViewPolicyRetention = Command("ShowMaterializedViewPolicyRetention", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("policy"),
                    RequiredToken("retention"),
                    shape207));

            var ShowMaterializedViewPrincipals = Command("ShowMaterializedViewPrincipals", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("principals"),
                    shape309));

            var ShowMaterializedViewSchemaAsJson = Command("ShowMaterializedViewSchemaAsJson", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("schema"),
                    RequiredToken("as"),
                    RequiredToken("json"),
                    shape140));

            var ShowMaterializedViewStatistics = Command("ShowMaterializedViewStatistics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    Token("statistics"),
                    shape311));

            var ShowMaterializedView = Command("ShowMaterializedView", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.MaterializedViewNameReference,
                    shape235));

            var ShowMaterializedViewPolicyCaching = Command("ShowMaterializedViewPolicyCaching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    rules.DatabaseMaterializedViewNameReference,
                    Token("policy"),
                    Token("caching"),
                    shape207));

            var ShowMaterializedViewPolicyRowLevelSecurity = Command("ShowMaterializedViewPolicyRowLevelSecurity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("materialized-view"),
                    Required(rules.DatabaseMaterializedViewNameReference, rules.MissingNameReference),
                    RequiredToken("policy"),
                    RequiredToken("row_level_security"),
                    shape207));

            var ShowMemory = Command("ShowMemory", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("memory"),
                    Optional(Token("details")),
                    shape198));

            var ShowMemPools = Command("ShowMemPools", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("mempools")));

            var ShowMirroringTemplates = Command("ShowMirroringTemplates", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("mirroring-templates")));

            var ShowMirroringTemplate = Command("ShowMirroringTemplate", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("mirroring-template"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    shape312));

            var ShowOperations = Command("ShowOperations", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("operations"),
                    Optional(
                        First(
                            Custom(
                                Token("("),
                                Required(
                                    OneOrMoreCommaList(
                                        Custom(
                                            rules.AnyGuidLiteralOrString,
                                            shape313),
                                        fnMissingElement: rules.MissingValue),
                                    missing53),
                                RequiredToken(")"),
                                shape44),
                            Custom(
                                rules.AnyGuidLiteralOrString,
                                shape313))),
                    shape198));

            var ShowOperationDetails = Command("ShowOperationDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("operation"),
                    Required(rules.AnyGuidLiteralOrString, rules.MissingValue),
                    RequiredToken("details"),
                    new [] {CD(), CD(), CD("OperationId", CompletionHint.Literal), CD()}));

            var ShowPlugins = Command("ShowPlugins", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("plugins"),
                    Optional(
                        fragment7),
                    shape198));

            var ShowPrincipalAccess = Command("ShowPrincipalAccess", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("principal"),
                    Token("access"),
                    Optional(
                        fragment7),
                    shape265));

            var ShowPrincipalRoles = Command("ShowPrincipalRoles", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("principal"),
                    RequiredToken("roles"),
                    Optional(
                        fragment7),
                    shape265));

            var ShowPrincipalRoles2 = Command("ShowPrincipalRoles", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("principal"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    RequiredToken("roles"),
                    Optional(
                        fragment7),
                    new [] {CD(), CD(), CD("Principal", CompletionHint.Literal), CD(), CD(isOptional: true)}));

            var ShowQueries = Command("ShowQueries", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("queries")));

            var ShowQueryExecution = Command("ShowQueryExecution", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("queryexecution"),
                    Required(
                        fragment6,
                        missing1),
                    new [] {CD(), CD(), CD("queryText")}));

            var ShowQueryPlan = Command("ShowQueryPlan", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("queryplan"),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OneOrMoreCommaList(
                                    Custom(
                                        First(
                                            Token("reconstructCsl"),
                                            Token("showExternalArtifacts"),
                                            If(Not(And(Token("reconstructCsl", "showExternalArtifacts"))), rules.NameDeclaration)),
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape137)),
                                missing68),
                            RequiredToken(")"))),
                    RequiredToken("<|"),
                    Required(rules.QueryInput, rules.MissingExpression),
                    new [] {CD(), CD(), CD(isOptional: true), CD(), CD("Query", CompletionHint.Tabular)}));

            var ShowQueryCallTree = Command("ShowQueryCallTree", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("query"),
                    RequiredToken("call-tree"),
                    Required(
                        fragment6,
                        missing1),
                    new [] {CD(), CD(), CD(), CD("queryText")}));

            var ShowRequestSupport = Command("ShowRequestSupport", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("request_support"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("key", CompletionHint.Literal)}));

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
                        fragment7),
                    new [] {CD(), CD(), CD(), CD("tableName", CompletionHint.Literal), CD(isOptional: true)}));

            var ShowRowStoreTransactions = Command("ShowRowStoreTransactions", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("rowstore"),
                    Token("transactions")));

            var ShowRowStore = Command("ShowRowStore", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("rowstore"),
                    Required(If(Not(And(Token("seals", "transactions"))), rules.NameDeclaration), rules.MissingNameDeclaration),
                    new [] {CD(), CD(), CD("rowStoreName", CompletionHint.None)}));

            var ShowRunningCallouts = Command("ShowRunningCallouts", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("running"),
                    Token("callouts")));

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
                                    fragment41),
                                missing15))),
                    shape265));

            var ShowSchema2 = Command("ShowSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("schema"),
                    Optional(
                        First(
                            fragment69,
                            Token("details"))),
                    shape198));

            var ShowServicePoints = Command("ShowServicePoints", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("servicepoints")));

            var StoredQueryResultsShow = Command("StoredQueryResultsShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("stored_query_results"),
                    Optional(
                        Custom(
                            If(Not(Token("with")), rules.NameDeclaration),
                            shape251)),
                    Optional(
                        fragment5),
                    new [] {CD(), CD(), CD(CompletionHint.None, isOptional: true), CD(isOptional: true)}));

            var StoredQueryResultShowSchema = Command("StoredQueryResultShowSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("stored_query_result"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    RequiredToken("schema"),
                    new [] {CD(), CD(), CD("StoredQueryResultName", CompletionHint.None), CD()}));

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

            var ShowTablesDetails = Command("ShowTablesDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.TableNameReference,
                            shape35),
                        fnMissingElement: rules.MissingNameReference),
                    Token(")"),
                    Token("details"),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD()}));

            var ShowTableExtentsMetadata = Command("ShowTableExtentsMetadata", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("tables"),
                        Token("("),
                        OneOrMoreCommaList(
                            Custom(
                                rules.TableNameReference,
                                shape35),
                            fnMissingElement: rules.MissingNameReference),
                        Token(")"),
                        Token("extents"),
                        Optional(
                            fragment58),
                        Optional(Token("hot")),
                        Token("metadata"),
                        Optional(
                            fragment59),
                        Optional(
                            fragment55)}
                    ,
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(isOptional: true), CD(isOptional: true), CD(), CD(isOptional: true), CD(isOptional: true)}));

            var ShowTableExtents = Command("ShowTableExtents", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("tables"),
                        Token("("),
                        OneOrMoreCommaList(
                            Custom(
                                rules.TableNameReference,
                                shape35),
                            fnMissingElement: rules.MissingNameReference),
                        Token(")"),
                        Token("extents"),
                        Optional(
                            fragment56),
                        Optional(Token("hot")),
                        RequiredToken("where"),
                        Required(
                            OneOrMoreList(
                                fragment54,
                                separatorParser: Token("and")),
                            missing62),
                        Optional(
                            fragment55)}
                    ,
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(isOptional: true), CD(isOptional: true), CD(), CD(), CD(isOptional: true)}));

            var ShowTableExtents2 = Command("ShowTableExtents", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("tables"),
                        Token("("),
                        OneOrMoreCommaList(
                            Custom(
                                rules.TableNameReference,
                                shape35),
                            fnMissingElement: rules.MissingNameReference),
                        Token(")"),
                        Token("extents"),
                        Optional(
                            fragment56),
                        Optional(Token("hot")),
                        RequiredToken("with"),
                        RequiredToken("("),
                        RequiredToken("extentsShowFilteringRuntimePolicy"),
                        RequiredToken("="),
                        Required(rules.Value, rules.MissingValue),
                        RequiredToken(")")}
                    ,
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(isOptional: true), CD(isOptional: true), CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()}));

            var ShowTableExtents3 = Command("ShowTableExtents", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.TableNameReference,
                            shape35),
                        fnMissingElement: rules.MissingNameReference),
                    Token(")"),
                    Token("extents"),
                    Optional(
                        fragment56),
                    Optional(Token("hot")),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD(isOptional: true), CD(isOptional: true)}));

            var TablesShardGroupsStatisticsShow = Command("TablesShardGroupsStatisticsShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("("),
                    OneOrMoreCommaList(
                        Custom(
                            rules.TableNameReference,
                            shape35),
                        fnMissingElement: rules.MissingNameReference),
                    Token(")"),
                    Token("shard-groups").Hide(),
                    RequiredToken("statistics").Hide(),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD(), CD(), CD()}));

            var ShowTables = Command("ShowTables", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("tables"),
                    RequiredToken("("),
                    Required(
                        OneOrMoreCommaList(
                            Custom(
                                rules.TableNameReference,
                                shape35),
                            fnMissingElement: rules.MissingNameReference),
                        missing32),
                    RequiredToken(")"),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.Table), CD()}));

            var ShowTablesColumnStatistics = Command("ShowTablesColumnStatistics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("column"),
                    RequiredToken("statistics"),
                    RequiredToken("older"),
                    Required(rules.Value, rules.MissingValue),
                    new [] {CD(), CD(), CD(), CD(), CD(), CD("outdatewindow", CompletionHint.Literal)}));

            var ShowTablesDetails2 = Command("ShowTablesDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("details")));

            var TablesShardGroupsStatisticsShow2 = Command("TablesShardGroupsStatisticsShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("tables"),
                    Token("shard-groups").Hide(),
                    RequiredToken("statistics").Hide()));

            var ShowTables2 = Command("ShowTables", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("tables")));

            var ShowTableOperationsMirroringStatus = Command("ShowTableOperationsMirroringStatus", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    Token("operations"),
                    RequiredToken("mirroring-status"),
                    new [] {CD(), CD(), CD("TableName"), CD(), CD()}));

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

            var ShowTableStarPolicyMirroring = Command("ShowTableStarPolicyMirroring", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Token("*"),
                    Token("policy"),
                    Token("mirroring")));

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

            var ShowTablePolicyAutoDelete = Command("ShowTablePolicyAutoDelete", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("auto_delete"),
                    shape208));

            var ShowTablePolicyCaching = Command("ShowTablePolicyCaching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("caching"),
                    shape208));

            var ShowTablePolicyEncoding = Command("ShowTablePolicyEncoding", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("encoding"),
                    shape208));

            var ShowTablePolicyExtentTagsRetention = Command("ShowTablePolicyExtentTagsRetention", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("extent_tags_retention"),
                    shape208));

            var ShowTablePolicyIngestionBatching = Command("ShowTablePolicyIngestionBatching", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("ingestionbatching"),
                    shape208));

            var ShowTablePolicyIngestionTime = Command("ShowTablePolicyIngestionTime", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("ingestiontime"),
                    shape208));

            var ShowTablePolicyMerge = Command("ShowTablePolicyMerge", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("merge"),
                    shape208));

            var ShowTablePolicyMirroring = Command("ShowTablePolicyMirroring", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("mirroring"),
                    shape208));

            var ShowTablePolicyPartitioning = Command("ShowTablePolicyPartitioning", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("partitioning"),
                    shape208));

            var ShowTablePolicyRestrictedViewAccess = Command("ShowTablePolicyRestrictedViewAccess", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("restricted_view_access"),
                    shape208));

            var ShowTablePolicyRetention = Command("ShowTablePolicyRetention", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("retention"),
                    shape208));

            var ShowTablePolicyRowLevelSecurity = Command("ShowTablePolicyRowLevelSecurity", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("row_level_security"),
                    shape208));

            var ShowTablePolicyRowOrder = Command("ShowTablePolicyRowOrder", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("roworder"),
                    shape208));

            var ShowTablePolicySharding = Command("ShowTablePolicySharding", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("sharding"),
                    shape208));

            var ShowTablePolicyStreamingIngestion = Command("ShowTablePolicyStreamingIngestion", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    Token("streamingingestion"),
                    shape208));

            var ShowTablePolicyUpdate = Command("ShowTablePolicyUpdate", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("policy"),
                    RequiredToken("update"),
                    shape208));

            var ShowTableRowStoreReferences = Command("ShowTableRowStoreReferences", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("rowstore_references"),
                    shape239));

            var ShowTableRowStoreSealInfo = Command("ShowTableRowStoreSealInfo", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    Token("rowstore_sealinfo"),
                    shape315));

            var ShowTableRowStores = Command("ShowTableRowStores", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.DatabaseTableNameReference),
                    RequiredToken("rowstores"),
                    shape315));

            var ShowTableColumnsClassification = Command("ShowTableColumnsClassification", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("columns"),
                    RequiredToken("classification"),
                    shape208));

            var ShowTableColumnStatitics = Command("ShowTableColumnStatitics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("column"),
                    RequiredToken("statistics"),
                    shape208));

            var ShowTableCslSchema = Command("ShowTableCslSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("cslschema"),
                    shape239));

            var ShowTableDataStatistics = Command("ShowTableDataStatistics", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("data"),
                    RequiredToken("statistics"),
                    Optional(
                        Custom(
                            Token("with"),
                            RequiredToken("("),
                            Required(
                                OneOrMoreCommaList(
                                    Custom(
                                        First(
                                            Token("from"),
                                            Token("samplepercent"),
                                            Token("scope"),
                                            Token("to"),
                                            If(Not(And(Token("from", "samplepercent", "scope", "to"))), rules.NameDeclaration)),
                                        RequiredToken("="),
                                        Required(rules.Value, rules.MissingValue),
                                        shape137)),
                                missing69),
                            RequiredToken(")"))),
                    shape316));

            var ShowTableDetails = Command("ShowTableDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("details"),
                    shape239));

            var ShowTableDimensions = Command("ShowTableDimensions", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("dimensions"),
                    shape239));

            var ShowExtentCorruptedDatetime2 = Command("ShowExtentCorruptedDatetime", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("extents"),
                    Required(
                        fragment70,
                        missing70).Hide(),
                    new [] {CD(), CD(), CD(CompletionHint.Table), CD(), CD()}));

            var ShowTableExtentsMetadata2 = Command("ShowTableExtentsMetadata", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("extents"),
                    Optional(
                        fragment58),
                    Optional(Token("hot")),
                    Token("metadata"),
                    Optional(
                        fragment59),
                    Optional(
                        fragment55),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(isOptional: true), CD(isOptional: true), CD(), CD(isOptional: true), CD(isOptional: true)}));

            var ShowTableExtents4 = Command("ShowTableExtents", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("extents"),
                    Optional(
                        fragment56),
                    Optional(Token("hot")),
                    RequiredToken("where"),
                    Required(
                        OneOrMoreList(
                            fragment54,
                            separatorParser: Token("and")),
                        missing62),
                    Optional(
                        fragment55),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(isOptional: true), CD(isOptional: true), CD(), CD(), CD(isOptional: true)}));

            var ShowTableExtents5 = Command("ShowTableExtents", 
                Custom(
                    new Parser<LexicalToken>[] {
                        Token("show", CompletionKind.CommandPrefix),
                        Token("table"),
                        If(Not(Token("*")), rules.TableNameReference),
                        Token("extents"),
                        Optional(
                            fragment56),
                        Optional(Token("hot")),
                        RequiredToken("with"),
                        RequiredToken("("),
                        RequiredToken("extentsShowFilteringRuntimePolicy"),
                        RequiredToken("="),
                        Required(rules.Value, rules.MissingValue),
                        RequiredToken(")")}
                    ,
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(isOptional: true), CD(isOptional: true), CD(), CD(), CD(), CD(), CD("policy", CompletionHint.Literal), CD()}));

            var ShowTableExtents6 = Command("ShowTableExtents", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("extents"),
                    Optional(
                        fragment56),
                    Optional(Token("hot")),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(isOptional: true), CD(isOptional: true)}));

            var ShowTableIngestionMappings = Command("ShowTableIngestionMappings", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("ingestion"),
                    Optional(
                        Custom(
                            Token("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                            shape50)),
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
                            Token("avro", "csv", "json", "orc", "parquet", "w3clogfile"),
                            shape50)),
                    RequiredToken("mapping"),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(isOptional: true), CD(), CD("MappingName", CompletionHint.Literal)}));

            var ShowTableCslSchema2 = Command("ShowTableCslSchema", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("kqlschema"),
                    shape239));

            var ShowTableOperationsMirroringExportedArtifacts = Command("ShowTableOperationsMirroringExportedArtifacts", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("operations"),
                    Token("mirroring-exported-artifacts"),
                    shape208));

            var ShowTableOperationsMirroringFailures = Command("ShowTableOperationsMirroringFailures", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("operations"),
                    Token("mirroring-failures"),
                    shape208));

            var ShowTableOperationsMirroringStatus2 = Command("ShowTableOperationsMirroringStatus", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("operations"),
                    RequiredToken("mirroring-status"),
                    shape208));

            var ShowTablePrincipals = Command("ShowTablePrincipals", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("principals"),
                    shape239));

            var ShowTablePrincipalRoles = Command("ShowTablePrincipalRoles", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("principal"),
                    Required(
                        First(
                            Token("roles"),
                            fragment66),
                        missing63),
                    Optional(
                        fragment7),
                    shape316));

            var ShowTableSchemaAsJson = Command("ShowTableSchemaAsJson", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("schema"),
                    RequiredToken("as"),
                    RequiredToken("json"),
                    shape148));

            var TableShardGroupsStatisticsShow = Command("TableShardGroupsStatisticsShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("shard-groups").Hide(),
                    Token("statistics").Hide(),
                    shape208));

            var TableShardGroupsShow = Command("TableShardGroupsShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("shard-groups").Hide(),
                    shape239));

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

            var TableShardsGroupShow = Command("TableShardsGroupShow", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    If(Not(Token("*")), rules.TableNameReference),
                    Token("shards-group").Hide(),
                    Required(rules.AnyGuidLiteralOrString, rules.MissingValue),
                    RequiredToken("shards").Hide(),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("ShardsGroupId", CompletionHint.Literal), CD()}));

            var ShowTable = Command("ShowTable", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Token("table"),
                    Required(If(Not(Token("*")), rules.TableNameReference), rules.MissingNameReference),
                    shape318));

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
                    Token("workload_group"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    new [] {CD(), CD(), CD("WorkloadGroup", CompletionHint.None)}));

            var ShowExtentDetails = Command("ShowExtentDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Required(If(Not(And(Token("cache", "callstacks", "capacity", "certificates", "cloudsettings", "cluster", "column", "commands-and-queries", "commands", "commconcurrency", "commpools", "continuous-exports", "continuous-export", "databases", "database", "data", "diagnostics", "entity_groups", "entity_group", "entity", "extentcontainers", "extents", "external", "fabriccache", "fabricclocks", "fabriclocks", "fabric", "featureflags", "follower", "freshness", "functions", "function", "graph_models", "graph_model", "graph_snapshots", "graph_snapshot", "ingestion", "journal", "materialized-views", "materialized-view", "memory", "mempools", "mirroring-templates", "mirroring-template", "operations", "operation", "plugins", "principal", "queries", "queryexecution", "queryplan", "query", "request_support", "rowstores", "rowstore", "running", "schema", "servicepoints", "stored_query_results", "stored_query_result", "streamingingestion", "tables", "table", "tcpconnections", "tcpports", "threadpools", "version", "workload_groups", "workload_group"))), rules.NameDeclaration), rules.MissingNameDeclaration),
                    RequiredToken("extent"),
                    RequiredToken("details"),
                    Optional(
                        Custom(
                            rules.AnyGuidLiteralOrString,
                            shape319)),
                    Optional(
                        fragment5),
                    new [] {CD(), CD("tableName", CompletionHint.None), CD(), CD(), CD(CompletionHint.Literal, isOptional: true), CD(isOptional: true)}));

            var ShowExtentDetails2 = Command("ShowExtentDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Required(If(Not(And(Token("cache", "callstacks", "capacity", "certificates", "cloudsettings", "cluster", "column", "commands-and-queries", "commands", "commconcurrency", "commpools", "continuous-exports", "continuous-export", "databases", "database", "data", "diagnostics", "entity_groups", "entity_group", "entity", "extentcontainers", "extents", "external", "fabriccache", "fabricclocks", "fabriclocks", "fabric", "featureflags", "follower", "freshness", "functions", "function", "graph_models", "graph_model", "graph_snapshots", "graph_snapshot", "ingestion", "journal", "materialized-views", "materialized-view", "memory", "mempools", "mirroring-templates", "mirroring-template", "operations", "operation", "plugins", "principal", "queries", "queryexecution", "queryplan", "query", "request_support", "rowstores", "rowstore", "running", "schema", "servicepoints", "stored_query_results", "stored_query_result", "streamingingestion", "tables", "table", "tcpconnections", "tcpports", "threadpools", "version", "workload_groups", "workload_group"))), rules.NameDeclaration), rules.MissingNameDeclaration),
                    RequiredToken("extent"),
                    RequiredToken("with"),
                    RequiredToken("("),
                    ZeroOrMoreCommaList(
                        fragment1),
                    RequiredToken(")"),
                    new [] {CD(), CD("tableName", CompletionHint.None), CD(), CD(), CD(), CD(CompletionHint.None), CD()}));

            var ShowExtentColumnStorageStats = Command("ShowExtentColumnStorageStats", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    If(Not(And(Token("cache", "callstacks", "capacity", "certificates", "cloudsettings", "cluster", "column", "commands-and-queries", "commands", "commconcurrency", "commpools", "continuous-exports", "continuous-export", "databases", "database", "data", "diagnostics", "entity_groups", "entity_group", "entity", "extentcontainers", "extents", "external", "fabriccache", "fabricclocks", "fabriclocks", "fabric", "featureflags", "follower", "freshness", "functions", "function", "graph_models", "graph_model", "graph_snapshots", "graph_snapshot", "ingestion", "journal", "materialized-views", "materialized-view", "memory", "mempools", "mirroring-templates", "mirroring-template", "operations", "operation", "plugins", "principal", "queries", "queryexecution", "queryplan", "query", "request_support", "rowstores", "rowstore", "running", "schema", "servicepoints", "stored_query_results", "stored_query_result", "streamingingestion", "tables", "table", "tcpconnections", "tcpports", "threadpools", "version", "workload_groups", "workload_group"))), rules.NameDeclaration),
                    Token("extent"),
                    rules.AnyGuidLiteralOrString,
                    Token("column"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    RequiredToken("storage"),
                    RequiredToken("stats"),
                    new [] {CD(), CD("tableName", CompletionHint.None), CD(), CD("extentId", CompletionHint.Literal), CD(), CD("columnName", CompletionHint.None), CD(), CD()}));

            var ShowExtentDetails3 = Command("ShowExtentDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Required(If(Not(And(Token("cache", "callstacks", "capacity", "certificates", "cloudsettings", "cluster", "column", "commands-and-queries", "commands", "commconcurrency", "commpools", "continuous-exports", "continuous-export", "databases", "database", "data", "diagnostics", "entity_groups", "entity_group", "entity", "extentcontainers", "extents", "external", "fabriccache", "fabricclocks", "fabriclocks", "fabric", "featureflags", "follower", "freshness", "functions", "function", "graph_models", "graph_model", "graph_snapshots", "graph_snapshot", "ingestion", "journal", "materialized-views", "materialized-view", "memory", "mempools", "mirroring-templates", "mirroring-template", "operations", "operation", "plugins", "principal", "queries", "queryexecution", "queryplan", "query", "request_support", "rowstores", "rowstore", "running", "schema", "servicepoints", "stored_query_results", "stored_query_result", "streamingingestion", "tables", "table", "tcpconnections", "tcpports", "threadpools", "version", "workload_groups", "workload_group"))), rules.NameDeclaration), rules.MissingNameDeclaration),
                    RequiredToken("extent"),
                    Required(rules.AnyGuidLiteralOrString, rules.MissingValue),
                    Optional(
                        fragment5),
                    new [] {CD(), CD("tableName", CompletionHint.None), CD(), CD("eid", CompletionHint.Literal), CD(isOptional: true)}));

            var ShowExtentDetails4 = Command("ShowExtentDetails", 
                Custom(
                    Token("show", CompletionKind.CommandPrefix),
                    Required(If(Not(And(Token("cache", "callstacks", "capacity", "certificates", "cloudsettings", "cluster", "column", "commands-and-queries", "commands", "commconcurrency", "commpools", "continuous-exports", "continuous-export", "databases", "database", "data", "diagnostics", "entity_groups", "entity_group", "entity", "extentcontainers", "extents", "external", "fabriccache", "fabricclocks", "fabriclocks", "fabric", "featureflags", "follower", "freshness", "functions", "function", "graph_models", "graph_model", "graph_snapshots", "graph_snapshot", "ingestion", "journal", "materialized-views", "materialized-view", "memory", "mempools", "mirroring-templates", "mirroring-template", "operations", "operation", "plugins", "principal", "queries", "queryexecution", "queryplan", "query", "request_support", "rowstores", "rowstore", "running", "schema", "servicepoints", "stored_query_results", "stored_query_result", "streamingingestion", "tables", "table", "tcpconnections", "tcpports", "threadpools", "version", "workload_groups", "workload_group"))), rules.NameDeclaration), rules.MissingNameDeclaration),
                    RequiredToken("extent"),
                    new [] {CD(), CD("tableName", CompletionHint.None), CD()}));

            var TableShuffleExtents = Command("TableShuffleExtents", 
                Custom(
                    Token("shuffle", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    Token("table"),
                    rules.TableNameReference,
                    Token("extents"),
                    First(
                        Custom(
                            Token("("),
                            OneOrMoreCommaList(
                                Custom(
                                    rules.AnyGuidLiteralOrString,
                                    shape244),
                                fnMissingElement: rules.MissingValue),
                            Token(")"),
                            shape44),
                        Custom(
                            Token("("),
                            Required(
                                OneOrMoreCommaList(
                                    Custom(
                                        rules.AnyGuidLiteralOrString,
                                        shape244),
                                    fnMissingElement: rules.MissingValue),
                                missing53),
                            RequiredToken(")"),
                            shape44),
                        Token("all")),
                    Optional(
                        fragment5),
                    new [] {CD(), CD(isOptional: true), CD(), CD("TableName", CompletionHint.Table), CD(), CD(), CD(isOptional: true)}));

            var TableShuffleExtentsQuery = Command("TableShuffleExtentsQuery", 
                Custom(
                    Token("shuffle", CompletionKind.CommandPrefix),
                    Optional(Token("async")),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredToken("extents"),
                    Optional(
                        fragment5),
                    RequiredToken("<|"),
                    Required(rules.QueryInput, rules.MissingExpression),
                    new [] {CD(), CD(isOptional: true), CD(), CD("tableName", CompletionHint.Table), CD(), CD(isOptional: true), CD(), CD("Query", CompletionHint.Tabular)}));

            var UndoDropTable = Command("UndoDropTable", 
                Custom(
                    Token("undo", CompletionKind.CommandPrefix),
                    RequiredToken("drop"),
                    RequiredToken("table"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Optional(
                        Custom(
                            Token("as"),
                            Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                            new [] {CD(), CD("TableName", CompletionHint.None)})),
                    RequiredToken("version"),
                    RequiredToken("="),
                    Required(rules.StringLiteral, rules.MissingStringLiteral),
                    Optional(Token("internal")),
                    new [] {CD(), CD(), CD(), CD(CompletionHint.None), CD(isOptional: true), CD(), CD(), CD("Version", CompletionHint.Literal), CD(isOptional: true)}));

            var TableDataUpdate = Command("TableDataUpdate", 
                Custom(
                    Token("update", CompletionKind.CommandPrefix),
                    Token("table"),
                    rules.TableNameReference,
                    Token("delete"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    RequiredToken("append"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Required(
                        fragment6,
                        missing1),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD("DeleteIdentifier", CompletionHint.None), CD(), CD("AppendIdentifier", CompletionHint.None), CD("csl")}));

            var TableDataUpdateShortSyntax = Command("TableDataUpdateShortSyntax", 
                Custom(
                    Token("update", CompletionKind.CommandPrefix),
                    RequiredToken("table"),
                    Required(rules.TableNameReference, rules.MissingNameReference),
                    RequiredToken("on"),
                    Required(rules.NameDeclaration, rules.MissingNameDeclaration),
                    Required(
                        fragment6,
                        missing1),
                    new [] {CD(), CD(), CD("TableName", CompletionHint.Table), CD(), CD(CompletionHint.None), CD("csl")}));

            var commandParsers = new Parser<LexicalToken, Command>[]
            {
                AddClusterRole,
                AddClusterRole2,
                AddClusterRole3,
                AddClusterRole4,
                AddClusterBlockedPrincipals,
                AddClusterRole5,
                AddClusterRole6,
                AddClusterRole7,
                AddClusterRole8,
                AddClusterRole9,
                AddDatabaseRole,
                AddExternalTableAdmins,
                AddFollowerDatabaseAuthorizedPrincipals,
                AddFunctionRole,
                AddMaterializedViewAdmins,
                AddTableRole,
                AlterMergeExtentTagsFromQuery,
                AlterMergeClusterPolicyCallout,
                AlterMergeClusterPolicyCapacity,
                AlterMergeClusterPolicyDiagnostics,
                AlterMergeClusterPolicyIngestionBatching,
                AlterMergeClusterPolicyManagedIdentity,
                AlterMergeClusterPolicyMultiDatabaseAdmins,
                AlterMergeClusterPolicyQueryWeakConsistency,
                AlterMergeClusterPolicyRequestClassification,
                AlterMergeClusterPolicyRowStore,
                AlterMergeClusterPolicySharding,
                AlterMergeClusterPolicyStreamingIngestion,
                AlterMergeColumnPolicyEncoding,
                AlterMergeDatabasePolicyDiagnostics,
                AlterMergeDatabasePolicyEncoding,
                AlterMergeDatabasePolicyIngestionBatching,
                AlterMergeDatabasePolicyManagedIdentity,
                AlterMergeDatabasePolicyMerge,
                AlterMergeDatabasePolicyRetention,
                AlterMergeDatabasePolicySharding,
                AlterMergeDatabasePolicyShardsGrouping,
                AlterMergeDatabasePolicyStreamingIngestion,
                AlterMergeDatabaseIngestionMapping,
                AlterMergeDatabasePolicyDiagnostics2,
                AlterMergeDatabasePolicyEncoding2,
                AlterMergeDatabasePolicyIngestionBatching2,
                AlterMergeDatabasePolicyManagedIdentity2,
                AlterMergeDatabasePolicyMerge2,
                AlterMergeDatabasePolicyMirroring,
                AlterMergeDatabasePolicyMirroringWithJson,
                AlterMergeDatabasePolicyRetention2,
                AlterMergeDatabasePolicySharding2,
                AlterMergeDatabasePolicyShardsGrouping2,
                AlterMergeDatabasePolicyStreamingIngestion2,
                AlterMergeDatabaseTableMirroringPolicyTemplates,
                AlterMergeEntityGroup,
                AlterMergeMaterializedViewPolicyMerge,
                AlterMergeMaterializedViewPolicyPartitioning,
                AlterMergeMaterializedViewPolicyRetention,
                AlterMergeMirroringTemplate,
                AlterMergeTablePolicyEncoding,
                AlterMergeTablePolicyIngestionBatching,
                AlterMergeTablePolicyMerge,
                AlterMergeTablePolicyMirroring,
                AlterMergeTablePolicyPartitioning,
                AlterMergeTablePolicyRetention,
                AlterMergeTablePolicyRowOrder,
                AlterMergeTablePolicySharding,
                AlterMergeTablePolicyStreamingIngestion,
                AlterMergeTablePolicyUpdate,
                AlterMergeTable,
                AlterMergeTableColumnDocStrings,
                AlterMergeExtentTagsFromQuery2,
                AlterMergeTableIngestionMapping,
                AlterMergeTablePolicyMirroringWithJson,
                AlterMergeWorkloadGroup,
                AlterClusterStorageKeys,
                AlterDatabaseStorageKeys,
                AlterExtentTagsFromQuery,
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
                AlterClusterStorageKeys2,
                AlterColumnsPolicyEncodingByQuery,
                AlterColumnPolicyCaching,
                AlterColumnPolicyEncodingType,
                AlterColumnPolicyEncoding,
                AlterColumnType,
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
                AlterDatabaseIngestionMapping,
                AlterDatabasePersistMetadata,
                AlterDatabasePolicyCaching2,
                AlterDatabasePolicyDiagnostics2,
                AlterDatabasePolicyEncoding2,
                AlterDatabasePolicyExtentTagsRetention2,
                AlterDatabasePolicyIngestionBatching2,
                AlterDatabasePolicyManagedIdentity2,
                AlterDatabasePolicyMerge2,
                AlterDatabasePolicyMirroring,
                AlterDatabasePolicyMirroringWithJson,
                AlterDatabasePolicyRetention2,
                AlterDatabasePolicySharding2,
                AlterDatabasePolicyShardsGrouping2,
                AlterDatabasePolicyStreamingIngestion2,
                AlterDatabaseTableMirroringPolicyTemplates,
                AlterDatabasePrettyName,
                AlterDatabaseStorageKeys2,
                AlterEntityGroup,
                AlterExtentContainersAdd,
                AlterExtentContainersDrop,
                AlterExtentContainersRecycle,
                AlterExtentContainersSet,
                AlterStorageExternalTable,
                AlterStorageExternalTable2,
                AlterStorageExternalTable3,
                AlterSqlExternalTable,
                AlterStorageExternalTable4,
                AlterExternalTableDocString,
                AlterExternalTableFolder,
                AlterStorageExternalTable5,
                AlterExternalTableMapping,
                AlterExternalTablePolicyQueryAcceleration,
                AlterFabricServiceAssignmentsCommand,
                AlterFollowerClusterConfiguration,
                AlterFollowerDatabaseConfiguration,
                AlterFollowerDatabaseConfiguration2,
                AlterFollowerDatabaseChildEntities,
                AlterFollowerTablesPolicyCaching,
                AlterFollowerDatabaseChildEntities2,
                AlterFollowerDatabaseChildEntities3,
                AlterFollowerTablesPolicyCaching2,
                AlterFollowerDatabaseConfiguration3,
                AlterFollowerDatabaseAuthorizedPrincipals,
                AlterFollowerDatabaseConfiguration4,
                AlterFollowerDatabaseConfiguration5,
                AlterFollowerTablesPolicyCaching3,
                AlterFollowerDatabaseChildEntities4,
                AlterFollowerDatabaseChildEntities5,
                AlterFollowerTablesPolicyCaching4,
                AlterFunction,
                AlterFunctionDocString,
                AlterFunctionFolder,
                AlterFunction2,
                AlterGraphModelPolicyRetention,
                AlterMaterializedViewOverMaterializedView,
                AlterMaterializedView,
                AlterMaterializedViewAutoUpdateSchema,
                AlterMaterializedViewDocString,
                AlterMaterializedViewFolder,
                AlterMaterializedViewLookback,
                AlterMaterializedViewOverMaterializedView2,
                AlterMaterializedView2,
                AlterMaterializedViewPolicyCaching,
                AlterMaterializedViewPolicyPartitioning,
                AlterMaterializedViewPolicyRetention,
                AlterMaterializedViewPolicyRowLevelSecurity,
                AlterMirroringTemplate,
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
                AlterTablePolicyIngestionTime,
                AlterTablePolicyMerge,
                AlterTablePolicyMirroring,
                AlterTablePolicyPartitioning,
                AlterTablePolicyRestrictedViewAccess,
                AlterTablePolicyRetention,
                AlterTablePolicyRowLevelSecurity,
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
                AlterExtentTagsFromQuery2,
                AlterTableFolder,
                AlterTableIngestionMapping,
                AlterTablePolicyMirroringWithJson,
                AppendTable,
                ApplyTableMirroringPolicyTemplateToTables,
                AttachExtentsIntoTableByMetadata,
                AttachDatabase,
                AttachExtentsIntoTableByMetadata2,
                AttachExtentsIntoTableByContainer,
                AttachExtentsIntoTableByMetadata3,
                AttachDatabase2,
                CancelOperation,
                CancelQuery,
                CleanDatabaseExtentContainers,
                ClearTableData,
                ClearRemoteClusterDatabaseSchema,
                ClearClusterCredStoreCache,
                ClearExternalArtifactsCache,
                ClearClusterGroupMembershipCache,
                ClearDatabaseCacheQueryResults,
                ClearDatabaseCacheQueryWeakConsistency,
                ClearDatabaseCacheStreamingIngestionSchema,
                ClearMaterializedViewData,
                ClearMaterializedViewStatistics,
                ClearTableCacheStreamingIngestionSchema,
                ClearTableData2,
                CreateMergeTables,
                CreateMergeTable,
                CreateOrAlterContinuousExport,
                CreateOrAlterDatabaseIngestionMapping,
                CreateOrAlterEntityGroupCommand,
                CreateOrAlterStorageExternalTable,
                CreateOrAlterStorageExternalTable2,
                CreateOrAlterStorageExternalTable3,
                CreateOrAlterSqlExternalTable,
                CreateOrAlterStorageExternalTable4,
                CreateOrAlterStorageExternalTable5,
                CreateOrAlterFunction,
                GraphModelCreateOrAlter,
                CreateOrAlterMaterializedViewOverMaterializedView,
                CreateOrAlterMaterializedView,
                CreateOrAlterTableIngestionMapping,
                CreateOrAleterWorkloadGroup,
                CreateMaterializedViewOverMaterializedView,
                CreateMaterializedView,
                CreateDatabase,
                CreateDatabaseIngestionMapping,
                CreateDatabase2,
                CreateDatabase3,
                CreateDatabase4,
                CreateDatabase5,
                CreateEntityGroupCommand,
                CreateStorageExternalTable,
                CreateStorageExternalTable2,
                CreateStorageExternalTable3,
                CreateSqlExternalTable,
                CreateStorageExternalTable4,
                CreateStorageExternalTable5,
                CreateExternalTableMapping,
                CreateFunction,
                CreateMaterializedViewOverMaterializedView2,
                CreateMaterializedView2,
                CreateMaterializedViewOverMaterializedView3,
                CreateMaterializedView3,
                CreateMirroringTemplate,
                CreateRequestSupport,
                CreateRowStore,
                CreateTables,
                CreateTable,
                CreateTableBasedOnAnother,
                CreateTableIngestionMapping,
                CreateTempStorage,
                DefineTables,
                DefineTable,
                DeleteMaterializedViewRecords,
                DeleteTableRecords,
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
                DeleteDatabasePolicyMirroring,
                DeleteDatabasePolicyRetention,
                DeleteDatabasePolicySharding,
                DeleteDatabasePolicyShardsGrouping,
                DeleteDatabasePolicyStreamingIngestion,
                DropDatabaseTableMirroringPolicyTemplates,
                DeleteExternalTablePolicyQueryAcceleration,
                DropFollowerTablesPolicyCaching,
                DropFollowerTablesPolicyCaching2,
                DropFollowerDatabasePolicyCaching,
                DropFollowerTablesPolicyCaching3,
                DropFollowerTablesPolicyCaching4,
                DeleteMaterializedViewRecords2,
                DeleteMaterializedViewPolicyCaching,
                DeleteMaterializedViewPolicyPartitioning,
                DeleteMaterializedViewPolicyRowLevelSecurity,
                DeleteMirroringTemplate,
                DeletePoliciesOfRetention,
                DeleteTablePolicyAutoDelete,
                DeleteTablePolicyCaching,
                DeleteTablePolicyEncoding,
                DeleteTablePolicyExtentTagsRetention,
                DeleteTablePolicyIngestionBatching,
                DeleteTablePolicyIngestionTime,
                DeleteTablePolicyMerge,
                DeleteTablePolicyMirroring,
                DeleteTablePolicyPartitioning,
                DeleteTablePolicyRestrictedViewAccess,
                DeleteTablePolicyRetention,
                DeleteTablePolicyRowLevelSecurity,
                DeleteTablePolicyRowOrder,
                DeleteTablePolicySharding,
                DeleteTablePolicyStreamingIngestion,
                DeleteTablePolicyUpdate,
                DeleteTableRecords2,
                DetachDatabase,
                DropRowStore,
                DisableContinuousExport,
                DisableDatabaseStreamingIngestionMaintenanceMode,
                DisableDatabaseMaintenanceMode,
                EnableDisableMaterializedView,
                DisablePlugin,
                DropPretendExtentsByProperties,
                DropEmptyExtentContainers,
                DropExtentTagsFromTable,
                DropExtentTagsFromQuery,
                DropExtentTagsFromQuery2,
                DropClusterRole,
                DropClusterRole2,
                DropClusterRole3,
                DropClusterRole4,
                DropClusterBlockedPrincipals,
                DropClusterRole5,
                DropClusterRole6,
                DropClusterRole7,
                DropClusterRole8,
                DropClusterRole9,
                DropColumn,
                DropContinuousExport,
                DropDatabaseRole,
                DetachDatabase2,
                DropDatabaseIngestionMapping,
                DropDatabaseRole2,
                DropDatabaseRole3,
                DropDatabasePrettyName,
                DetachDatabase3,
                DropDatabaseRole4,
                DropDatabaseRole5,
                DropDatabaseRole6,
                DetachDatabase4,
                DropEmptyExtentContainers2,
                DropEntityGroup,
                DropExtents,
                DropExtents2,
                DropExtents3,
                DropExtents4,
                DropExtentsPartitionMetadata,
                DropExtents5,
                DropExtentTagsRetention,
                DropExtent,
                DropExternalTableAdmins,
                DropExternalTableMapping,
                DropExternalTable,
                DropFabricServiceAssignmentsCommand,
                DropFollowerDatabases,
                DropFollowerDatabases2,
                DropFollowerDatabaseAuthorizedPrincipals,
                DropFunctions,
                DropFunctionRole,
                DropFunction,
                DropFunction2,
                GraphModelDrop,
                GraphSnapshotsDrop,
                GraphSnapshotDrop,
                DropMaterializedViewAdmins,
                DropMaterializedView,
                DropRowStore2,
                StoredQueryResultsDrop,
                StoredQueryResultDrop,
                DropStoredQueryResultContainers,
                DropTables,
                DropTableRole,
                DropTableColumns,
                DropExtentTagsFromTable2,
                DropExtentTagsFromQuery3,
                DropExtentTagsFromQuery4,
                DropTable,
                DropTableIngestionMapping,
                DropTableRole2,
                DropTable2,
                DropTempStorage,
                DropUnusedStoredQueryResultContainers,
                DropWorkloadGroup,
                EnableContinuousExport,
                EnableDatabaseStreamingIngestionMaintenanceMode,
                EnableDatabaseMaintenanceMode,
                EnableDisableMaterializedView2,
                EnablePlugin,
                ExecuteClusterScript,
                ExecuteDatabaseScript,
                ExecuteDatabaseScript2,
                ExportToStorage,
                ExportToStorage2,
                ExportToStorage3,
                ExportToStorage4,
                ExportToSqlTable,
                ExportToExternalTable,
                ExportToStorage5,
                IngestIntoTable,
                IngestInlineIntoTable,
                IngestIntoTable2,
                AttachDatabase3,
                GraphSnapshotMake,
                MergeExtents,
                MergeDatabaseShardGroups,
                MergeExtentsDryrun,
                MergeExtents2,
                MoveExtentsFrom,
                MoveExtentsFrom2,
                MoveExtentsQuery,
                PatchExtentCorruptedDatetime,
                RenameColumns,
                RenameColumn,
                RenameMaterializedView,
                RenameTables,
                RenameTable,
                ReplaceExtents,
                ReplaceDatabaseKeyVaultSecrets,
                ReplaceExtents2,
                SetOrAppendTable,
                StoredQueryResultSetOrReplace,
                SetOrReplaceTable,
                SetAccess,
                StoredQueryResultSet,
                StoredQueryResultSet2,
                SetTable,
                SetClusterRole,
                SetContinuousExportCursor,
                SetDatabaseRole,
                SetDatabaseRole2,
                SetDatabaseRole3,
                AlterDatabasePrettyName2,
                SetDatabaseRole4,
                SetDatabaseRole5,
                SetDatabaseRole6,
                SetExternalTableAdmins,
                SetFunctionRole,
                StoredQueryResultSet3,
                SetMaterializedViewAdmins,
                SetMaterializedViewConcurrency,
                SetMaterializedViewCursor,
                StoredQueryResultSet4,
                SetTableRowStoreReferences,
                SetTableRole,
                SetTable2,
                ShowCache,
                ShowCallStacks,
                ShowCapacity,
                ShowCertificates,
                ShowCloudSettings,
                ShowClusterAdminState,
                ShowClusterBlockedPrincipals,
                ShowClusterDatabasesDataStats,
                ShowClusterDatabasesDetails,
                ShowClusterDatabasesIdentity,
                ShowClusterDatabasesMetadata,
                ShowClusterDatabasesPolicies,
                ShowClusterDatabasesDetails2,
                ShowClusterDatabases,
                ShowClusterDetails,
                ShowClusterExtentsMetadata,
                ShowClusterExtents,
                ShowClusterExtents2,
                ShowClusterExtents3,
                ShowIngestionMappings,
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
                ShowSchema,
                ShowClusterServices,
                ShowClusterStorageKeysHash,
                ShowCluster,
                ShowColumnPolicyCaching,
                ShowColumnPolicyCaching2,
                ShowColumnPolicyEncoding,
                ShowCommandsAndQueries,
                ShowCommands,
                ShowCommConcurrency,
                ShowCommPools,
                ShowContinuousExports,
                ShowContinuousExportExportedArtifacts,
                ShowContinuousExportFailures,
                ShowContinuousExport,
                ShowClusterDatabasesDataStats2,
                ShowClusterDatabasesDetails3,
                ShowClusterDatabasesIdentity2,
                ShowClusterDatabasesMetadata2,
                ShowClusterDatabasesPolicies2,
                ShowClusterDatabasesDetails4,
                ShowClusterDatabases2,
                ShowClusterDatabasesDataStats3,
                ShowClusterDatabasesDetails5,
                ShowDatabaseExtentsMetadata,
                ShowDatabaseExtents,
                ShowDatabaseExtents2,
                ShowDatabaseExtents3,
                ShowClusterDatabasesIdentity3,
                ShowClusterDatabasesMetadata3,
                ShowClusterDatabasesPolicies3,
                ShowDatabasesSchemaAsJson,
                ShowDatabasesSchemaAsJson2,
                ShowDatabasesSchema,
                ShowDatabasesSchema2,
                ShowClusterDatabasesDetails6,
                ShowClusterDatabases3,
                ShowClusterDatabasesDataStats4,
                ShowClusterDatabasesDetails7,
                ShowDatabaseExtentsMetadata2,
                ShowDatabaseExtents4,
                ShowDatabaseExtents5,
                ShowDatabaseExtents6,
                ShowClusterDatabasesIdentity4,
                ShowClusterDatabasesMetadata4,
                ShowClusterDatabasesPolicies4,
                ShowDatabasesSchemaAsJson3,
                ShowDatabasesSchemaAsJson4,
                ShowDatabasesSchema3,
                ShowDatabasesSchema4,
                ShowClusterDatabasesDetails8,
                ShowClusterDatabases4,
                ShowDatabasesSchemaAsJson5,
                ShowDatabasesSchemaAsJson6,
                ShowDatabasesSchema5,
                ShowDatabasesSchema6,
                ShowClusterDatabasesDataStats5,
                ShowClusterDatabasesDetails9,
                ShowDatabasesEntities,
                ShowClusterDatabasesIdentity5,
                ShowDatabasesManagementGroups,
                ShowClusterDatabasesMetadata5,
                ShowClusterDatabasesPolicies5,
                ShowDatabasesSchemaAsJson7,
                ShowDatabasesSchemaAsJson8,
                ShowDatabasesSchema7,
                ShowDatabasesSchema8,
                ShowClusterDatabasesDetails10,
                ShowClusterDatabases5,
                ShowDatabasePolicyCaching,
                ShowDatabasePolicyExtentTagsRetention,
                ShowDatabasePolicyIngestionBatching,
                ShowDatabasePolicyMerge,
                ShowDatabasePolicyRetention,
                ShowDatabasePolicySharding,
                ShowDatabasePolicyShardsGrouping,
                ShowDatabaseCacheQueryResults,
                ShowDatabaseCslSchema,
                ShowDatabaseDataStats,
                ShowDatabaseDetails,
                ShowDatabaseExtentsMetadata3,
                ShowDatabaseExtents7,
                ShowDatabaseExtents8,
                ShowDatabaseExtents9,
                ShowDatabaseExtentsMetadata4,
                ShowDatabaseExtents10,
                ShowDatabaseExtents11,
                ShowDatabaseExtents12,
                ShowDatabaseExtentsMetadata5,
                ShowDatabaseExtentsPartitioningStatistics,
                ShowDatabaseExtents13,
                ShowDatabaseExtents14,
                ShowDatabaseExtents15,
                ShowDatabaseExtentTagsStatistics,
                ShowDatabaseIdentity,
                ShowDatabaseIngestionMappings,
                ShowDatabaseKeyVaultSecrets,
                ShowDatabaseCslSchema2,
                ShowDatabaseMetadata,
                ShowDatabasePolicies,
                ShowDatabasePolicyCaching2,
                ShowDatabasePolicyExtentTagsRetention2,
                ShowDatabasePolicyIngestionBatching2,
                ShowDatabasePolicyMerge2,
                ShowDatabasePolicyRetention2,
                ShowDatabasePolicySharding2,
                ShowDatabasePolicyShardsGrouping2,
                ShowDatabasePrincipals,
                ShowDatabaseSchemaAsCslScript,
                ShowDatabaseSchemaAsJson,
                ShowDatabaseSchemaAsCslScript2,
                ShowDatabaseSchema,
                ShowDatabaseSchemaAsCslScript3,
                ShowDatabaseSchemaAsJson2,
                ShowDatabaseSchemaAsCslScript4,
                ShowDatabaseSchema2,
                ShowDatabaseSchemaViolations,
                ShowDatabaseSchema3,
                DatabaseShardGroupsStatisticsShow,
                ShowStorageArtifactsCleanupState,
                ShowDatabaseDetails2,
                ShowDatabase,
                ShowDatabaseCslSchema3,
                ShowDatabaseEntity,
                ShowDatabaseExtentContainersCleanOperations,
                ShowDatabaseExtentsMetadata6,
                ShowDatabaseExtents16,
                ShowDatabaseExtents17,
                ShowDatabaseExtents18,
                ShowDatabaseExtentsMetadata7,
                ShowDatabaseExtents19,
                ShowDatabaseExtents20,
                ShowDatabaseExtents21,
                ShowDatabaseExtentsMetadata8,
                ShowDatabaseExtentsPartitioningStatistics2,
                ShowDatabaseExtents22,
                ShowDatabaseExtents23,
                ShowDatabaseExtents24,
                ShowDatabaseIngestionMappings2,
                ShowDatabaseJournal,
                ShowDatabaseCslSchema4,
                ShowDatabasePolicyCaching3,
                ShowDatabasePolicyDiagnostics,
                ShowDatabasePolicyEncoding,
                ShowDatabasePolicyExtentTagsRetention3,
                ShowDatabasePolicyHardRetentionViolations,
                ShowDatabasePolicyIngestionBatching3,
                ShowDatabasePolicyManagedIdentity,
                ShowDatabasePolicyMerge3,
                ShowDatabasePolicyMirroring,
                ShowDatabasePolicyRetention3,
                ShowDatabasePolicySharding3,
                ShowDatabasePolicyShardsGrouping3,
                ShowDatabasePolicySoftRetentionViolations,
                ShowDatabasePolicyStreamingIngestion,
                ShowDatabaseTableMirroringPolicyTemplates,
                ShowDatabasePrincipals2,
                ShowDatabasePrincipalRoles,
                ShowDatabasePurgeOperation,
                ShowDatabaseSchemaAsCslScript5,
                ShowDatabaseSchemaAsJson3,
                ShowDatabaseSchemaAsCslScript6,
                ShowDatabaseSchema4,
                ShowDatabaseSchemaAsCslScript7,
                ShowDatabaseSchemaAsJson4,
                ShowDatabaseSchemaAsCslScript8,
                ShowDatabaseSchema5,
                ShowDatabaseSchemaViolations2,
                ShowDatabaseSchema6,
                DatabaseShardGroupsStatisticsShow2,
                ShowDatabase2,
                ShowDataOperations,
                ShowDiagnostics,
                ShowEntityGroups,
                ShowEntityGroup,
                ShowEntitySchema,
                ShowExtentContainers,
                ShowExtentCorruptedDatetime,
                ShowExternalTablesQueryAccelerationStatatistics,
                ShowExternalTables,
                ShowExternalTables2,
                ShowExternalTablesPolicyQueryAcceleration,
                ShowExternalTableArtifacts,
                ShowExternalTableCslSchema,
                ShowExternalTableCslSchema2,
                ShowExternalTableMappings,
                ShowExternalTableMapping,
                ShowExternalTableQueryAccelerationStatatistics,
                ShowExternalTablePolicyQueryAcceleration,
                ShowExternalTablePrincipals,
                ShowExternalTablesPrincipalRoles,
                ShowExternalTableSchema,
                ShowExternalTable,
                ShowExternalTable2,
                ShowFabricCache,
                ShowFabricClocks,
                ShowFabricLocks,
                ShowFabric,
                ShowFeatureFlags,
                ShowFollowerDatabase,
                ShowFreshness,
                ShowFunctions,
                ShowFunctionPrincipals,
                ShowFunctionPrincipalRoles,
                ShowFunctionSchemaAsJson,
                ShowFunction,
                ShowFunction2,
                GraphModelsShow,
                GraphModelShow,
                GraphSnapshotsShow,
                GraphSnapshotShow,
                ShowIngestionMappings2,
                ShowIngestionMappings3,
                ShowIngestionMappings4,
                ShowIngestionFailures,
                ShowIngestionMappings5,
                ShowIngestionMappings6,
                ShowIngestionMappings7,
                ShowIngestionMappings8,
                ShowIngestionMappings9,
                ShowIngestionMappings10,
                ShowJournal,
                ShowMaterializedViewsDetails,
                ShowMaterializedViewsDetails2,
                ShowMaterializedViews,
                ShowMaterializedViewCslSchema,
                ShowMaterializedViewDetails,
                ShowMaterializedViewDiagnostics,
                ShowMaterializedViewExtents,
                ShowMaterializedViewFailures,
                ShowMaterializedViewCslSchema2,
                ShowMaterializedViewPolicyMerge,
                ShowMaterializedViewPolicyPartitioning,
                ShowMaterializedViewPolicyRetention,
                ShowMaterializedViewPrincipals,
                ShowMaterializedViewSchemaAsJson,
                ShowMaterializedViewStatistics,
                ShowMaterializedView,
                ShowMaterializedViewPolicyCaching,
                ShowMaterializedViewPolicyRowLevelSecurity,
                ShowMemory,
                ShowMemPools,
                ShowMirroringTemplates,
                ShowMirroringTemplate,
                ShowOperations,
                ShowOperationDetails,
                ShowPlugins,
                ShowPrincipalAccess,
                ShowPrincipalRoles,
                ShowPrincipalRoles2,
                ShowQueries,
                ShowQueryExecution,
                ShowQueryPlan,
                ShowQueryCallTree,
                ShowRequestSupport,
                ShowRowStores,
                ShowRowStoreSeals,
                ShowRowStoreTransactions,
                ShowRowStore,
                ShowRunningCallouts,
                ShowRunningQueries,
                ShowSchema2,
                ShowServicePoints,
                StoredQueryResultsShow,
                StoredQueryResultShowSchema,
                ShowStreamingIngestionFailures,
                ShowStreamingIngestionStatistics,
                ShowTablesDetails,
                ShowTableExtentsMetadata,
                ShowTableExtents,
                ShowTableExtents2,
                ShowTableExtents3,
                TablesShardGroupsStatisticsShow,
                ShowTables,
                ShowTablesColumnStatistics,
                ShowTablesDetails2,
                TablesShardGroupsStatisticsShow2,
                ShowTables2,
                ShowTableOperationsMirroringStatus,
                ShowTableStarPolicyCaching,
                ShowTableStarPolicyExtentTagsRetention,
                ShowTableStarPolicyIngestionBatching,
                ShowTableStarPolicyIngestionTime,
                ShowTableStarPolicyMerge,
                ShowTableStarPolicyMirroring,
                ShowTableStarPolicyPartitioning,
                ShowTableStarPolicyRestrictedViewAccess,
                ShowTableStarPolicyRetention,
                ShowTableStarPolicyRowLevelSecurity,
                ShowTableStarPolicyRowOrder,
                ShowTableStarPolicySharding,
                ShowTableStarPolicyUpdate,
                ShowTablePolicyAutoDelete,
                ShowTablePolicyCaching,
                ShowTablePolicyEncoding,
                ShowTablePolicyExtentTagsRetention,
                ShowTablePolicyIngestionBatching,
                ShowTablePolicyIngestionTime,
                ShowTablePolicyMerge,
                ShowTablePolicyMirroring,
                ShowTablePolicyPartitioning,
                ShowTablePolicyRestrictedViewAccess,
                ShowTablePolicyRetention,
                ShowTablePolicyRowLevelSecurity,
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
                ShowTableDataStatistics,
                ShowTableDetails,
                ShowTableDimensions,
                ShowExtentCorruptedDatetime2,
                ShowTableExtentsMetadata2,
                ShowTableExtents4,
                ShowTableExtents5,
                ShowTableExtents6,
                ShowTableIngestionMappings,
                ShowTableIngestionMapping,
                ShowTableCslSchema2,
                ShowTableOperationsMirroringExportedArtifacts,
                ShowTableOperationsMirroringFailures,
                ShowTableOperationsMirroringStatus2,
                ShowTablePrincipals,
                ShowTablePrincipalRoles,
                ShowTableSchemaAsJson,
                TableShardGroupsStatisticsShow,
                TableShardGroupsShow,
                TableShardsGroupMetadataShow,
                TableShardsGroupShow,
                ShowTable,
                ShowTcpConnections,
                ShowTcpPorts,
                ShowThreadPools,
                ShowVersion,
                ShowWorkloadGroups,
                ShowWorkloadGroup,
                ShowExtentDetails,
                ShowExtentDetails2,
                ShowExtentColumnStorageStats,
                ShowExtentDetails3,
                ShowExtentDetails4,
                TableShuffleExtents,
                TableShuffleExtentsQuery,
                UndoDropTable,
                TableDataUpdate,
                TableDataUpdateShortSyntax
            };

            return commandParsers;
        }
    }
}

