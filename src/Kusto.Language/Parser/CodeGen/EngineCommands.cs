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
//          This file is auto generated from the template file 'EngineCommands.tt'
//          Instead modify the corresponding input info file in the Kusto.Language.Generator project.
//          After making changes, use the right-click menu on the .tt file and select 'run custom tool'.

using System;
using System.Linq;
using System.Collections.Generic;
using Kusto.Language.Symbols;

namespace Kusto.Language
{
    public static class EngineCommands
    {
        private static readonly string _schema0 = "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, ReservedSlot1: bool, DatabaseId: guid, InTransitionTo: string)";
        private static readonly string _schema1 = "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, AuthorizedPrincipals: string, RetentionPolicy: string, MergePolicy: string, ReservedSlot1: string, CachingPolicy: string, ShardingPolicy: string, StreamingIngestionPolicy: string, IngestionBatchingPolicy: string, TotalSize: real, DatabaseId: guid, InTransitionTo: string)";
        private static readonly string _schema2 = "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, CurrentUserIsUnrestrictedViewer: bool, DatabaseId: guid, InTransitionTo: string)";
        private static readonly string _schema3 = "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, DatabaseId: guid, AuthorizedPrincipals: string, RetentionPolicy: string, MergePolicy: string, CachingPolicy: string, ShardingPolicy: string, StreamingIngestionPolicy: string, IngestionBatchingPolicy: string)";
        private static readonly string _schema4 = "(Step: string, Duration: string)";
        private static readonly string _schema5 = "(DatabaseName: string, PrettyName: string)";
        private static readonly string _schema6 = "(DatabaseName: string, TableName: string, ColumnName: string, ColumnType: string, IsDefaultTable: bool, IsDefaultColumn: bool, PrettyName: string, Version: string, Folder: string, DocString: string)";
        private static readonly string _schema7 = "(DatabaseSchema: string)";
        private static readonly string _schema8 = "(Name: string, Kind: string, Mapping: string, LastUpdatedOn: datetime, Database: string)";
        private static readonly string _schema9 = "(TableName: string, DatabaseName: string, Folder: string, DocString: string, TotalExtents: long, TotalExtentSize: real, TotalOriginalSize: real, TotalRowCount: long, HotExtents: long, HotExtentSize: real, HotOriginalSize: real, HotRowCount: long, AuthorizedPrincipals: string, RetentionPolicy: string, CachingPolicy: string, ShardingPolicy: string, MergePolicy: string, StreamingIngestionPolicy: string, IngestionBatchingPolicy: string, MinExtentsCreationTime: datetime, MaxExtentsCreationTime: datetime, RowOrderPolicy: string, TableId: guid)";
        private static readonly string _schema10 = "(TableName: string, Schema: string, DatabaseName: string, Folder: string, DocString: string)";
        private static readonly string _schema11 = "(TableName: string, DatabaseName: string, Folder: string, DocString: string)";
        private static readonly string _schema12 = "(TableName: string, DatabaseName: string)";
        private static readonly string _schema13 = "(Name: string, Kind: string, Mapping: string, LastUpdatedOn: datetime, Database: string, Table: string)";
        private static readonly string _schema14 = "(EntityName: string, DataType: string, Policy: string)";
        private static readonly string _schema15 = "(Name: string, Parameters: string, Body: string, Folder: string, DocString: string)";
        private static readonly string _schema16 = "(TableName: string, TableType: string, Folder: string, DocString: string, Properties: string, ConnectionStrings: dynamic, Partitions: dynamic, PathFormat: string)";
        private static readonly string _schema17 = "(TableName: string, TableType: string, Folder: string, DocString: string, Schema: string, Properties: string)";
        private static readonly string _schema18 = "()";
        private static readonly string _schema19 = "(WorkloadGroupName: string, WorkloadGroup:string)";
        private static readonly string _schema20 = "(PolicyName: string, EntityName: string, Policy: string, ChildEntities: string, EntityType: string)";
        private static readonly string _schema21 = "(Scope: string, DisplayName: string, AADObjectID: string, Role: string)";
        private static readonly string _schema22 = "(Role: string, PrincipalType: string, PrincipalDisplayName: string, PrincipalObjectId: string, PrincipalFQN: string, Notes: string, RoleAssignmentIdentifier: string)";
        private static readonly string _schema23 = "(PrincipalType: string, PrincipalDisplayName: string, PrincipalObjectId: string, PrincipalFQN: string, Application: string, User: string, BlockedUntil: datetime, Reason: string)";
        private static readonly string _schema24 = "(ExtentId: guid, OriginalSize: long, ExtentSize: long, ColumnSize: long, IndexSize: long, RowCount: long)";
        private static readonly string _schema25 = "(Name: string, ExternalTableName: string, Query: string, ForcedLatency: timespan, IntervalBetweenRuns: timespan, CursorScopedTables: string, ExportProperties: string, LastRunTime: datetime, StartCursor: string, IsDisabled: bool, LastRunResult: string, ExportedTo: datetime, IsRunning: bool)";
        private static readonly string _schema26 = "(Name: string, SourceTable: string, Query: string, MaterializedTo: datetime, LastRun: datetime, LastRunResult: string, IsHealthy: bool, IsEnabled: bool, Folder: string, DocString: string, AutoUpdateSchema: bool, EffectiveDateTime: datetime, Lookback:timespan)";
        private static readonly string _schema27 = "(MaterializedViewName: string, DatabaseName: string, Folder: string, DocString: string, TotalExtents: long, TotalExtentSize: real, TotalOriginalSize: real, TotalRowCount: long, HotExtents: long, HotExtentSize: real, HotOriginalSize: real, HotRowCount: long, AuthorizedPrincipals: string, RetentionPolicy: string, CachingPolicy: string, ShardingPolicy: string, MergePolicy: string, MinExtentsCreationTime: datetime, MaxExtentsCreationTime: datetime)";
        private static readonly string _schema28 = "(Event: string, EventTimestamp: datetime, Database: string, EntityName: string, UpdatedEntityName: string, EntityVersion: string, EntityContainerName: string, OriginalEntityState: string, UpdatedEntityState: string, ChangeCommand: string, Principal: string, RootActivityId: guid, ClientRequestId: string, User: string, OriginalEntityVersion: string)";
        private static readonly string _schema29 = "(ExtentId: guid, DatabaseName: string, TableName: string, MaxCreatedOn: datetime, OriginalSize: real, ExtentSize: real, CompressedSize: real, IndexSize: real, Blocks: long, Segments: long, ExtentContainerId: string, RowCount: long, MinCreatedOn: datetime, Tags: string, Kind: string, DeletedRowCount: long)";
        private static readonly string _schema30 = "(ExtentId: guid, DatabaseName: string, TableName: string, ExtentMetadata: string)";
        private static readonly string _schema31 = "(TableId: guid, long ShardGroupCount: long, ShardCount: long, RowCount: long, OriginalSize: long, ShardSize: long, CompressedSize: long, IndexSize: long, DeletedRowCount: long, V2ShardCount: long, V2RowCount: long)";
        private static readonly string _schema32 = "(OriginalExtentId: string, ResultExtentId: string, Duration: timespan)";
        private static readonly string _schema33 = "(OriginalExtentId: string, ResultExtentId: string, Details: string)";
        private static readonly string _schema34 = "(ExtentId: guid, TableName: string, CreatedOn: datetime)";
        private static readonly string _schema35 = "(StoredQueryResultId:guid, Name:string, DatabaseName:string, PrincipalIdentity:string, SizeInBytes:long, RowCount:long, CreatedOn:datetime, ExpiresOn:datetime)";
        private static readonly string _schema36 = "(Name: string, Entities: string)";

        public static readonly CommandSymbol ShowDatabase =
            new CommandSymbol("ShowDatabase", _schema0);

        public static readonly CommandSymbol ShowDatabaseDetails =
            new CommandSymbol("ShowDatabaseDetails", _schema1);

        public static readonly CommandSymbol ShowDatabaseIdentity =
            new CommandSymbol("ShowDatabaseIdentity", _schema2);

        public static readonly CommandSymbol ShowDatabasePolicies =
            new CommandSymbol("ShowDatabasePolicies", _schema3);

        public static readonly CommandSymbol ShowDatabaseDataStats =
            new CommandSymbol(
                "ShowDatabaseDataStats",
                "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, CurrentUseIsUnrestrictedViewer: bool, DatabaseId: guid, OriginalSize: real, ExtentSize: real, CompressedSize: real, IndexSize: real, RowCount: long, HotOriginalSize: real, HotExtentSize: real, HotCompressedSize: real, HotIndexSize: real, HotRowCount: long)");

        public static readonly CommandSymbol ShowClusterDatabases =
            new CommandSymbol("ShowClusterDatabases", _schema0);

        public static readonly CommandSymbol ShowClusterDatabasesDetails =
            new CommandSymbol("ShowClusterDatabasesDetails", _schema1);

        public static readonly CommandSymbol ShowClusterDatabasesIdentity =
            new CommandSymbol("ShowClusterDatabasesIdentity", _schema2);

        public static readonly CommandSymbol ShowClusterDatabasesPolicies =
            new CommandSymbol("ShowClusterDatabasesPolicies", _schema3);

        public static readonly CommandSymbol ShowClusterDatabasesDataStats =
            new CommandSymbol(
                "ShowClusterDatabasesDataStats",
                "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, DatabaseId: guid, OriginalSize: real, ExtentSize: real, CompressedSize: real, IndexSize: real, RowCount: long, HotOriginalSize: real, HotExtentSize: real, HotCompressedSize: real, HotIndexSize: real, HotRowCount: long)");

        public static readonly CommandSymbol CreateDatabasePersist =
            new CommandSymbol(
                "CreateDatabasePersist",
                "(DatabaseName: string, PersistentPath: string, Created: string, StoresMetadata: bool, StoresData: bool)");

        public static readonly CommandSymbol CreateDatabaseVolatile =
            new CommandSymbol(
                "CreateDatabaseVolatile",
                "(DatabaseName: string, PersistentPath: string, Created: bool, StoresMetadata: bool, StoresData: bool)");

        public static readonly CommandSymbol AttachDatabase =
            new CommandSymbol("AttachDatabase", _schema4);

        public static readonly CommandSymbol AttachDatabaseMetadata =
            new CommandSymbol("AttachDatabaseMetadata", _schema4);

        public static readonly CommandSymbol DetachDatabase =
            new CommandSymbol(
                "DetachDatabase",
                "(Table: string, NumberOfRemovedExtents: string)");

        public static readonly CommandSymbol AlterDatabasePrettyName =
            new CommandSymbol("AlterDatabasePrettyName", _schema5);

        public static readonly CommandSymbol DropDatabasePrettyName =
            new CommandSymbol("DropDatabasePrettyName", _schema5);

        public static readonly CommandSymbol AlterDatabasePersistMetadata =
            new CommandSymbol(
                "AlterDatabasePersistMetadata",
                "(Moniker: guid, Url: string, State: string, CreatedOn: datetime, MaxDateTime: datetime, IsRecyclable: bool, StoresDatabaseMetadata: bool, HardDeletePeriod: timespan)");

        public static readonly CommandSymbol SetAccess =
            new CommandSymbol(
                "SetAccess",
                "(DatabaseName: string, RequestedAccessMode: string, Status: string)");

        public static readonly CommandSymbol ShowDatabaseSchema =
            new CommandSymbol("ShowDatabaseSchema", _schema6);

        public static readonly CommandSymbol ShowDatabaseSchemaAsJson =
            new CommandSymbol("ShowDatabaseSchemaAsJson", _schema7);

        public static readonly CommandSymbol ShowDatabaseSchemaAsCslScript =
            new CommandSymbol(
                "ShowDatabaseSchemaAsCslScript",
                "(DatabaseSchemaScript: string)");

        public static readonly CommandSymbol ShowDatabaseCslSchema =
            new CommandSymbol("ShowDatabaseCslSchema", _schema10);

        public static readonly CommandSymbol ShowDatabaseSchemaViolations =
            new CommandSymbol(
                "ShowDatabaseSchemaViolations",
                "(EntityKind: string, EntityName: string, Property: string, Reason: string)");

        public static readonly CommandSymbol ShowDatabasesSchema =
            new CommandSymbol("ShowDatabasesSchema", _schema6);

        public static readonly CommandSymbol ShowDatabasesSchemaAsJson =
            new CommandSymbol("ShowDatabasesSchemaAsJson", _schema7);

        public static readonly CommandSymbol CreateDatabaseIngestionMapping =
            new CommandSymbol("CreateDatabaseIngestionMapping", _schema8);

        public static readonly CommandSymbol AlterDatabaseIngestionMapping =
            new CommandSymbol("AlterDatabaseIngestionMapping", _schema8);

        public static readonly CommandSymbol ShowDatabaseIngestionMappings =
            new CommandSymbol("ShowDatabaseIngestionMappings", _schema8);

        public static readonly CommandSymbol ShowIngestionMappings =
            new CommandSymbol("ShowIngestionMappings", _schema18);

        public static readonly CommandSymbol DropDatabaseIngestionMapping =
            new CommandSymbol("DropDatabaseIngestionMapping", _schema8);

        public static readonly CommandSymbol ShowTables =
            new CommandSymbol("ShowTables", _schema11);

        public static readonly CommandSymbol ShowTable =
            new CommandSymbol(
                "ShowTable",
                "(AttributeName: string, AttributeType: string, ExtentSize: long, CompressionRatio: real, IndexSize: long, IndexSizePercent: real, OriginalSize: long, AttributeId: guid, SharedIndexSize: long, StorageEngineVersion: string)");

        public static readonly CommandSymbol ShowTablesDetails =
            new CommandSymbol("ShowTablesDetails", _schema9);

        public static readonly CommandSymbol ShowTableDetails =
            new CommandSymbol("ShowTableDetails", _schema9);

        public static readonly CommandSymbol ShowTableCslSchema =
            new CommandSymbol("ShowTableCslSchema", _schema10);

        public static readonly CommandSymbol ShowTableSchemaAsJson =
            new CommandSymbol("ShowTableSchemaAsJson", _schema10);

        public static readonly CommandSymbol CreateTable =
            new CommandSymbol("CreateTable", _schema10);

        public static readonly CommandSymbol CreateTableBasedOnAnother =
            new CommandSymbol("CreateTableBasedOnAnother", _schema10);

        public static readonly CommandSymbol CreateMergeTable =
            new CommandSymbol("CreateMergeTable", _schema10);

        public static readonly CommandSymbol DefineTable =
            new CommandSymbol("DefineTable", _schema10);

        public static readonly CommandSymbol CreateTables =
            new CommandSymbol("CreateTables", _schema11);

        public static readonly CommandSymbol CreateMergeTables =
            new CommandSymbol("CreateMergeTables", _schema11);

        public static readonly CommandSymbol DefineTables =
            new CommandSymbol("DefineTables", _schema11);

        public static readonly CommandSymbol AlterTable =
            new CommandSymbol("AlterTable", _schema10);

        public static readonly CommandSymbol AlterMergeTable =
            new CommandSymbol("AlterMergeTable", _schema10);

        public static readonly CommandSymbol AlterTableDocString =
            new CommandSymbol("AlterTableDocString", _schema10);

        public static readonly CommandSymbol AlterTableFolder =
            new CommandSymbol("AlterTableFolder", _schema10);

        public static readonly CommandSymbol RenameTable =
            new CommandSymbol("RenameTable", _schema11);

        public static readonly CommandSymbol RenameTables =
            new CommandSymbol("RenameTables", _schema11);

        public static readonly CommandSymbol DropTable =
            new CommandSymbol("DropTable", _schema12);

        public static readonly CommandSymbol UndoDropTable =
            new CommandSymbol(
                "UndoDropTable",
                "(ExtentId: guid, NumberOfRecords: long, Status: string, FailureReason: string)");

        public static readonly CommandSymbol DropTables =
            new CommandSymbol("DropTables", _schema12);

        public static readonly CommandSymbol CreateTableIngestionMapping =
            new CommandSymbol("CreateTableIngestionMapping", _schema13);

        public static readonly CommandSymbol AlterTableIngestionMapping =
            new CommandSymbol("AlterTableIngestionMapping", _schema13);

        public static readonly CommandSymbol ShowTableIngestionMappings =
            new CommandSymbol("ShowTableIngestionMappings", _schema13);

        public static readonly CommandSymbol ShowTableIngestionMapping =
            new CommandSymbol("ShowTableIngestionMapping", _schema13);

        public static readonly CommandSymbol DropTableIngestionMapping =
            new CommandSymbol("DropTableIngestionMapping", _schema13);

        public static readonly CommandSymbol RenameColumn =
            new CommandSymbol("RenameColumn", _schema14);

        public static readonly CommandSymbol RenameColumns =
            new CommandSymbol("RenameColumns", _schema14);

        public static readonly CommandSymbol AlterColumnType =
            new CommandSymbol("AlterColumnType", _schema14);

        public static readonly CommandSymbol DropColumn =
            new CommandSymbol("DropColumn", _schema10);

        public static readonly CommandSymbol DropTableColumns =
            new CommandSymbol("DropTableColumns", _schema10);

        public static readonly CommandSymbol AlterTableColumnDocStrings =
            new CommandSymbol("AlterTableColumnDocStrings", _schema10);

        public static readonly CommandSymbol AlterMergeTableColumnDocStrings =
            new CommandSymbol("AlterMergeTableColumnDocStrings", _schema10);

        public static readonly CommandSymbol ShowFunctions =
            new CommandSymbol("ShowFunctions", _schema15);

        public static readonly CommandSymbol ShowFunction =
            new CommandSymbol("ShowFunction", _schema15);

        public static readonly CommandSymbol CreateFunction =
            new CommandSymbol("CreateFunction", _schema15);

        public static readonly CommandSymbol AlterFunction =
            new CommandSymbol("AlterFunction", _schema15);

        public static readonly CommandSymbol CreateOrAlterFunction =
            new CommandSymbol("CreateOrAlterFunction", _schema15);

        public static readonly CommandSymbol DropFunction =
            new CommandSymbol(
                "DropFunction",
                "(Name: string)");

        public static readonly CommandSymbol DropFunctions =
            new CommandSymbol("DropFunctions", _schema15);

        public static readonly CommandSymbol AlterFunctionDocString =
            new CommandSymbol("AlterFunctionDocString", _schema15);

        public static readonly CommandSymbol AlterFunctionFolder =
            new CommandSymbol("AlterFunctionFolder", _schema15);

        public static readonly CommandSymbol ShowExternalTables =
            new CommandSymbol("ShowExternalTables", _schema16);

        public static readonly CommandSymbol ShowExternalTable =
            new CommandSymbol("ShowExternalTable", _schema16);

        public static readonly CommandSymbol ShowExternalTableCslSchema =
            new CommandSymbol("ShowExternalTableCslSchema", _schema10);

        public static readonly CommandSymbol ShowExternalTableSchema =
            new CommandSymbol("ShowExternalTableSchema", _schema10);

        public static readonly CommandSymbol ShowExternalTableArtifacts =
            new CommandSymbol(
                "ShowExternalTableArtifacts",
                "(Uri: string, Partition: dynamic, Size: long)");

        public static readonly CommandSymbol DropExternalTable =
            new CommandSymbol("DropExternalTable", _schema16);

        public static readonly CommandSymbol CreateStorageExternalTable =
            new CommandSymbol("CreateStorageExternalTable", _schema16);

        public static readonly CommandSymbol AlterStorageExternalTable =
            new CommandSymbol("AlterStorageExternalTable", _schema17);

        public static readonly CommandSymbol CreateOrAlterStorageExternalTable =
            new CommandSymbol("CreateOrAlterStorageExternalTable", _schema17);

        public static readonly CommandSymbol CreateSqlExternalTable =
            new CommandSymbol("CreateSqlExternalTable", _schema16);

        public static readonly CommandSymbol AlterSqlExternalTable =
            new CommandSymbol("AlterSqlExternalTable", _schema17);

        public static readonly CommandSymbol CreateOrAlterSqlExternalTable =
            new CommandSymbol("CreateOrAlterSqlExternalTable", _schema17);

        public static readonly CommandSymbol CreateExternalTableMapping =
            new CommandSymbol("CreateExternalTableMapping", _schema13);

        public static readonly CommandSymbol SetExternalTableAdmins =
            new CommandSymbol("SetExternalTableAdmins", _schema18);

        public static readonly CommandSymbol AddExternalTableAdmins =
            new CommandSymbol("AddExternalTableAdmins", _schema18);

        public static readonly CommandSymbol DropExternalTableAdmins =
            new CommandSymbol("DropExternalTableAdmins", _schema18);

        public static readonly CommandSymbol AlterExternalTableDocString =
            new CommandSymbol("AlterExternalTableDocString", _schema18);

        public static readonly CommandSymbol AlterExternalTableFolder =
            new CommandSymbol("AlterExternalTableFolder", _schema18);

        public static readonly CommandSymbol ShowExternalTablePrincipals =
            new CommandSymbol("ShowExternalTablePrincipals", _schema18);

        public static readonly CommandSymbol ShowFabric =
            new CommandSymbol("ShowFabric", _schema18);

        public static readonly CommandSymbol AlterExternalTableMapping =
            new CommandSymbol("AlterExternalTableMapping", _schema13);

        public static readonly CommandSymbol ShowExternalTableMappings =
            new CommandSymbol("ShowExternalTableMappings", _schema13);

        public static readonly CommandSymbol ShowExternalTableMapping =
            new CommandSymbol("ShowExternalTableMapping", _schema13);

        public static readonly CommandSymbol DropExternalTableMapping =
            new CommandSymbol("DropExternalTableMapping", _schema13);

        public static readonly CommandSymbol ShowWorkloadGroups =
            new CommandSymbol("ShowWorkloadGroups", _schema19);

        public static readonly CommandSymbol ShowWorkloadGroup =
            new CommandSymbol("ShowWorkloadGroup", _schema19);

        public static readonly CommandSymbol CreateOrAleterWorkloadGroup =
            new CommandSymbol("CreateOrAleterWorkloadGroup", _schema19);

        public static readonly CommandSymbol AlterMergeWorkloadGroup =
            new CommandSymbol("AlterMergeWorkloadGroup", _schema19);

        public static readonly CommandSymbol DropWorkloadGroup =
            new CommandSymbol("DropWorkloadGroup", _schema19);

        public static readonly CommandSymbol ShowDatabasePolicyCaching =
            new CommandSymbol("ShowDatabasePolicyCaching", _schema20);

        public static readonly CommandSymbol ShowTablePolicyCaching =
            new CommandSymbol("ShowTablePolicyCaching", _schema20);

        public static readonly CommandSymbol ShowTableStarPolicyCaching =
            new CommandSymbol("ShowTableStarPolicyCaching", _schema20);

        public static readonly CommandSymbol ShowColumnPolicyCaching =
            new CommandSymbol("ShowColumnPolicyCaching", _schema20);

        public static readonly CommandSymbol ShowMaterializedViewPolicyCaching =
            new CommandSymbol("ShowMaterializedViewPolicyCaching", _schema20);

        public static readonly CommandSymbol ShowClusterPolicyCaching =
            new CommandSymbol("ShowClusterPolicyCaching", _schema20);

        public static readonly CommandSymbol AlterDatabasePolicyCaching =
            new CommandSymbol("AlterDatabasePolicyCaching", _schema20);

        public static readonly CommandSymbol AlterTablePolicyCaching =
            new CommandSymbol("AlterTablePolicyCaching", _schema20);

        public static readonly CommandSymbol AlterTablesPolicyCaching =
            new CommandSymbol("AlterTablesPolicyCaching", _schema18);

        public static readonly CommandSymbol AlterColumnPolicyCaching =
            new CommandSymbol("AlterColumnPolicyCaching", _schema20);

        public static readonly CommandSymbol AlterMaterializedViewPolicyCaching =
            new CommandSymbol("AlterMaterializedViewPolicyCaching", _schema20);

        public static readonly CommandSymbol AlterClusterPolicyCaching =
            new CommandSymbol("AlterClusterPolicyCaching", _schema20);

        public static readonly CommandSymbol DeleteDatabasePolicyCaching =
            new CommandSymbol("DeleteDatabasePolicyCaching", _schema20);

        public static readonly CommandSymbol DeleteTablePolicyCaching =
            new CommandSymbol("DeleteTablePolicyCaching", _schema20);

        public static readonly CommandSymbol DeleteColumnPolicyCaching =
            new CommandSymbol("DeleteColumnPolicyCaching", _schema20);

        public static readonly CommandSymbol DeleteMaterializedViewPolicyCaching =
            new CommandSymbol("DeleteMaterializedViewPolicyCaching", _schema20);

        public static readonly CommandSymbol DeleteClusterPolicyCaching =
            new CommandSymbol("DeleteClusterPolicyCaching", _schema20);

        public static readonly CommandSymbol ShowTablePolicyIngestionTime =
            new CommandSymbol("ShowTablePolicyIngestionTime", _schema20);

        public static readonly CommandSymbol ShowTableStarPolicyIngestionTime =
            new CommandSymbol("ShowTableStarPolicyIngestionTime", _schema20);

        public static readonly CommandSymbol AlterTablePolicyIngestionTime =
            new CommandSymbol("AlterTablePolicyIngestionTime", _schema20);

        public static readonly CommandSymbol AlterTablesPolicyIngestionTime =
            new CommandSymbol("AlterTablesPolicyIngestionTime", _schema20);

        public static readonly CommandSymbol DeleteTablePolicyIngestionTime =
            new CommandSymbol("DeleteTablePolicyIngestionTime", _schema20);

        public static readonly CommandSymbol ShowTablePolicyRetention =
            new CommandSymbol("ShowTablePolicyRetention", _schema20);

        public static readonly CommandSymbol ShowTableStarPolicyRetention =
            new CommandSymbol("ShowTableStarPolicyRetention", _schema20);

        public static readonly CommandSymbol ShowDatabasePolicyRetention =
            new CommandSymbol("ShowDatabasePolicyRetention", _schema20);

        public static readonly CommandSymbol AlterTablePolicyRetention =
            new CommandSymbol("AlterTablePolicyRetention", _schema20);

        public static readonly CommandSymbol AlterMaterializedViewPolicyRetention =
            new CommandSymbol("AlterMaterializedViewPolicyRetention", _schema20);

        public static readonly CommandSymbol AlterDatabasePolicyRetention =
            new CommandSymbol("AlterDatabasePolicyRetention", _schema20);

        public static readonly CommandSymbol AlterTablesPolicyRetention =
            new CommandSymbol("AlterTablesPolicyRetention", _schema20);

        public static readonly CommandSymbol AlterMergeTablePolicyRetention =
            new CommandSymbol("AlterMergeTablePolicyRetention", _schema20);

        public static readonly CommandSymbol AlterMergeMaterializedViewPolicyRetention =
            new CommandSymbol("AlterMergeMaterializedViewPolicyRetention", _schema20);

        public static readonly CommandSymbol AlterMergeDatabasePolicyRetention =
            new CommandSymbol("AlterMergeDatabasePolicyRetention", _schema20);

        public static readonly CommandSymbol DeleteTablePolicyRetention =
            new CommandSymbol("DeleteTablePolicyRetention", _schema20);

        public static readonly CommandSymbol DeleteDatabasePolicyRetention =
            new CommandSymbol("DeleteDatabasePolicyRetention", _schema20);

        public static readonly CommandSymbol ShowDatabasePolicyHardRetentionViolations =
            new CommandSymbol("ShowDatabasePolicyHardRetentionViolations", _schema18);

        public static readonly CommandSymbol ShowDatabasePolicySoftRetentionViolations =
            new CommandSymbol("ShowDatabasePolicySoftRetentionViolations", _schema18);

        public static readonly CommandSymbol ShowTablePolicyRowLevelSecurity =
            new CommandSymbol("ShowTablePolicyRowLevelSecurity", _schema20);

        public static readonly CommandSymbol ShowTableStarPolicyRowLevelSecurity =
            new CommandSymbol("ShowTableStarPolicyRowLevelSecurity", _schema20);

        public static readonly CommandSymbol AlterTablePolicyRowLevelSecurity =
            new CommandSymbol("AlterTablePolicyRowLevelSecurity", _schema20);

        public static readonly CommandSymbol DeleteTablePolicyRowLevelSecurity =
            new CommandSymbol("DeleteTablePolicyRowLevelSecurity", _schema20);

        public static readonly CommandSymbol ShowMaterializedViewPolicyRowLevelSecurity =
            new CommandSymbol("ShowMaterializedViewPolicyRowLevelSecurity", _schema20);

        public static readonly CommandSymbol AlterMaterializedViewPolicyRowLevelSecurity =
            new CommandSymbol("AlterMaterializedViewPolicyRowLevelSecurity", _schema20);

        public static readonly CommandSymbol DeleteMaterializedViewPolicyRowLevelSecurity =
            new CommandSymbol("DeleteMaterializedViewPolicyRowLevelSecurity", _schema20);

        public static readonly CommandSymbol ShowTablePolicyRowOrder =
            new CommandSymbol("ShowTablePolicyRowOrder", _schema20);

        public static readonly CommandSymbol ShowTableStarPolicyRowOrder =
            new CommandSymbol("ShowTableStarPolicyRowOrder", _schema20);

        public static readonly CommandSymbol AlterTablePolicyRowOrder =
            new CommandSymbol("AlterTablePolicyRowOrder", _schema20);

        public static readonly CommandSymbol AlterTablesPolicyRowOrder =
            new CommandSymbol("AlterTablesPolicyRowOrder", _schema20);

        public static readonly CommandSymbol AlterMergeTablePolicyRowOrder =
            new CommandSymbol("AlterMergeTablePolicyRowOrder", _schema20);

        public static readonly CommandSymbol DeleteTablePolicyRowOrder =
            new CommandSymbol("DeleteTablePolicyRowOrder", _schema20);

        public static readonly CommandSymbol ShowTablePolicyUpdate =
            new CommandSymbol("ShowTablePolicyUpdate", _schema20);

        public static readonly CommandSymbol ShowTableStarPolicyUpdate =
            new CommandSymbol("ShowTableStarPolicyUpdate", _schema20);

        public static readonly CommandSymbol AlterTablePolicyUpdate =
            new CommandSymbol("AlterTablePolicyUpdate", _schema20);

        public static readonly CommandSymbol AlterMergeTablePolicyUpdate =
            new CommandSymbol("AlterMergeTablePolicyUpdate", _schema20);

        public static readonly CommandSymbol DeleteTablePolicyUpdate =
            new CommandSymbol("DeleteTablePolicyUpdate", _schema20);

        public static readonly CommandSymbol ShowClusterPolicyIngestionBatching =
            new CommandSymbol("ShowClusterPolicyIngestionBatching", _schema20);

        public static readonly CommandSymbol ShowDatabasePolicyIngestionBatching =
            new CommandSymbol("ShowDatabasePolicyIngestionBatching", _schema20);

        public static readonly CommandSymbol ShowTablePolicyIngestionBatching =
            new CommandSymbol("ShowTablePolicyIngestionBatching", _schema20);

        public static readonly CommandSymbol ShowTableStarPolicyIngestionBatching =
            new CommandSymbol("ShowTableStarPolicyIngestionBatching", _schema20);

        public static readonly CommandSymbol AlterClusterPolicyIngestionBatching =
            new CommandSymbol("AlterClusterPolicyIngestionBatching", _schema20);

        public static readonly CommandSymbol AlterDatabasePolicyIngestionBatching =
            new CommandSymbol("AlterDatabasePolicyIngestionBatching", _schema20);

        public static readonly CommandSymbol AlterTablePolicyIngestionBatching =
            new CommandSymbol("AlterTablePolicyIngestionBatching", _schema20);

        public static readonly CommandSymbol AlterTablesPolicyIngestionBatching =
            new CommandSymbol("AlterTablesPolicyIngestionBatching", _schema20);

        public static readonly CommandSymbol DeleteDatabasePolicyIngestionBatching =
            new CommandSymbol("DeleteDatabasePolicyIngestionBatching", _schema20);

        public static readonly CommandSymbol DeleteTablePolicyIngestionBatching =
            new CommandSymbol("DeleteTablePolicyIngestionBatching", _schema20);

        public static readonly CommandSymbol ShowDatabasePolicyEncoding =
            new CommandSymbol("ShowDatabasePolicyEncoding", _schema20);

        public static readonly CommandSymbol ShowTablePolicyEncoding =
            new CommandSymbol("ShowTablePolicyEncoding", _schema20);

        public static readonly CommandSymbol ShowColumnPolicyEncoding =
            new CommandSymbol("ShowColumnPolicyEncoding", _schema20);

        public static readonly CommandSymbol AlterDatabasePolicyEncoding =
            new CommandSymbol("AlterDatabasePolicyEncoding", _schema20);

        public static readonly CommandSymbol AlterTablePolicyEncoding =
            new CommandSymbol("AlterTablePolicyEncoding", _schema20);

        public static readonly CommandSymbol AlterTableColumnsPolicyEncoding =
            new CommandSymbol("AlterTableColumnsPolicyEncoding", _schema20);

        public static readonly CommandSymbol AlterColumnPolicyEncoding =
            new CommandSymbol("AlterColumnPolicyEncoding", _schema20);

        public static readonly CommandSymbol AlterColumnPolicyEncodingType =
            new CommandSymbol("AlterColumnPolicyEncodingType", _schema20);

        public static readonly CommandSymbol AlterMergeDatabasePolicyEncoding =
            new CommandSymbol("AlterMergeDatabasePolicyEncoding", _schema20);

        public static readonly CommandSymbol AlterMergeTablePolicyEncoding =
            new CommandSymbol("AlterMergeTablePolicyEncoding", _schema20);

        public static readonly CommandSymbol AlterMergeColumnPolicyEncoding =
            new CommandSymbol("AlterMergeColumnPolicyEncoding", _schema20);

        public static readonly CommandSymbol DeleteDatabasePolicyEncoding =
            new CommandSymbol("DeleteDatabasePolicyEncoding", _schema20);

        public static readonly CommandSymbol DeleteTablePolicyEncoding =
            new CommandSymbol("DeleteTablePolicyEncoding", _schema20);

        public static readonly CommandSymbol DeleteColumnPolicyEncoding =
            new CommandSymbol("DeleteColumnPolicyEncoding", _schema20);

        public static readonly CommandSymbol ShowDatabasePolicyMerge =
            new CommandSymbol("ShowDatabasePolicyMerge", _schema20);

        public static readonly CommandSymbol ShowTablePolicyMerge =
            new CommandSymbol("ShowTablePolicyMerge", _schema20);

        public static readonly CommandSymbol ShowTableStarPolicyMerge =
            new CommandSymbol("ShowTableStarPolicyMerge", _schema20);

        public static readonly CommandSymbol AlterDatabasePolicyMerge =
            new CommandSymbol("AlterDatabasePolicyMerge", _schema20);

        public static readonly CommandSymbol AlterTablePolicyMerge =
            new CommandSymbol("AlterTablePolicyMerge", _schema20);

        public static readonly CommandSymbol AlterTablesPolicyMerge =
            new CommandSymbol("AlterTablesPolicyMerge", _schema20);

        public static readonly CommandSymbol AlterMergeDatabasePolicyMerge =
            new CommandSymbol("AlterMergeDatabasePolicyMerge", _schema20);

        public static readonly CommandSymbol AlterMergeTablePolicyMerge =
            new CommandSymbol("AlterMergeTablePolicyMerge", _schema20);

        public static readonly CommandSymbol DeleteDatabasePolicyMerge =
            new CommandSymbol("DeleteDatabasePolicyMerge", _schema20);

        public static readonly CommandSymbol DeleteTablePolicyMerge =
            new CommandSymbol("DeleteTablePolicyMerge", _schema20);

        public static readonly CommandSymbol ShowTablePolicyPartitioning =
            new CommandSymbol("ShowTablePolicyPartitioning", _schema20);

        public static readonly CommandSymbol ShowTableStarPolicyPartitioning =
            new CommandSymbol("ShowTableStarPolicyPartitioning", _schema20);

        public static readonly CommandSymbol AlterTablePolicyPartitioning =
            new CommandSymbol("AlterTablePolicyPartitioning", _schema20);

        public static readonly CommandSymbol AlterMergeTablePolicyPartitioning =
            new CommandSymbol("AlterMergeTablePolicyPartitioning", _schema20);

        public static readonly CommandSymbol AlterMaterializedViewPolicyPartitioning =
            new CommandSymbol("AlterMaterializedViewPolicyPartitioning", _schema20);

        public static readonly CommandSymbol AlterMergeMaterializedViewPolicyPartitioning =
            new CommandSymbol("AlterMergeMaterializedViewPolicyPartitioning", _schema20);

        public static readonly CommandSymbol DeleteTablePolicyPartitioning =
            new CommandSymbol("DeleteTablePolicyPartitioning", _schema20);

        public static readonly CommandSymbol DeleteMaterializedViewPolicyPartitioning =
            new CommandSymbol("DeleteMaterializedViewPolicyPartitioning", _schema20);

        public static readonly CommandSymbol ShowTablePolicyRestrictedViewAccess =
            new CommandSymbol("ShowTablePolicyRestrictedViewAccess", _schema20);

        public static readonly CommandSymbol ShowTableStarPolicyRestrictedViewAccess =
            new CommandSymbol("ShowTableStarPolicyRestrictedViewAccess", _schema20);

        public static readonly CommandSymbol AlterTablePolicyRestrictedViewAccess =
            new CommandSymbol("AlterTablePolicyRestrictedViewAccess", _schema20);

        public static readonly CommandSymbol AlterTablesPolicyRestrictedViewAccess =
            new CommandSymbol("AlterTablesPolicyRestrictedViewAccess", _schema20);

        public static readonly CommandSymbol DeleteTablePolicyRestrictedViewAccess =
            new CommandSymbol("DeleteTablePolicyRestrictedViewAccess", _schema20);

        public static readonly CommandSymbol ShowClusterPolicyRowStore =
            new CommandSymbol("ShowClusterPolicyRowStore", _schema20);

        public static readonly CommandSymbol AlterClusterPolicyRowStore =
            new CommandSymbol("AlterClusterPolicyRowStore", _schema20);

        public static readonly CommandSymbol AlterMergeClusterPolicyRowStore =
            new CommandSymbol("AlterMergeClusterPolicyRowStore", _schema20);

        public static readonly CommandSymbol DeleteClusterPolicyRowStore =
            new CommandSymbol("DeleteClusterPolicyRowStore", _schema20);

        public static readonly CommandSymbol ShowClusterPolicySandbox =
            new CommandSymbol("ShowClusterPolicySandbox", _schema20);

        public static readonly CommandSymbol AlterClusterPolicySandbox =
            new CommandSymbol("AlterClusterPolicySandbox", _schema20);

        public static readonly CommandSymbol DeleteClusterPolicySandbox =
            new CommandSymbol("DeleteClusterPolicySandbox", _schema20);

        public static readonly CommandSymbol ShowClusterSandboxesStats =
            new CommandSymbol("ShowClusterSandboxesStats", _schema20);

        public static readonly CommandSymbol ShowDatabasePolicySharding =
            new CommandSymbol("ShowDatabasePolicySharding", _schema20);

        public static readonly CommandSymbol ShowTablePolicySharding =
            new CommandSymbol("ShowTablePolicySharding", _schema20);

        public static readonly CommandSymbol ShowTableStarPolicySharding =
            new CommandSymbol("ShowTableStarPolicySharding", _schema20);

        public static readonly CommandSymbol AlterDatabasePolicySharding =
            new CommandSymbol("AlterDatabasePolicySharding", _schema20);

        public static readonly CommandSymbol AlterTablePolicySharding =
            new CommandSymbol("AlterTablePolicySharding", _schema20);

        public static readonly CommandSymbol AlterMergeDatabasePolicySharding =
            new CommandSymbol("AlterMergeDatabasePolicySharding", _schema20);

        public static readonly CommandSymbol AlterMergeTablePolicySharding =
            new CommandSymbol("AlterMergeTablePolicySharding", _schema20);

        public static readonly CommandSymbol DeleteDatabasePolicySharding =
            new CommandSymbol("DeleteDatabasePolicySharding", _schema20);

        public static readonly CommandSymbol DeleteTablePolicySharding =
            new CommandSymbol("DeleteTablePolicySharding", _schema20);

        public static readonly CommandSymbol AlterClusterPolicySharding =
            new CommandSymbol("AlterClusterPolicySharding", _schema20);

        public static readonly CommandSymbol AlterMergeClusterPolicySharding =
            new CommandSymbol("AlterMergeClusterPolicySharding", _schema20);

        public static readonly CommandSymbol DeleteClusterPolicySharding =
            new CommandSymbol("DeleteClusterPolicySharding", _schema20);

        public static readonly CommandSymbol ShowClusterPolicySharding =
            new CommandSymbol("ShowClusterPolicySharding", _schema20);

        public static readonly CommandSymbol ShowDatabasePolicyShardsGrouping =
            new CommandSymbol("ShowDatabasePolicyShardsGrouping", _schema20);

        public static readonly CommandSymbol AlterDatabasePolicyShardsGrouping =
            new CommandSymbol("AlterDatabasePolicyShardsGrouping", _schema20);

        public static readonly CommandSymbol AlterMergeDatabasePolicyShardsGrouping =
            new CommandSymbol("AlterMergeDatabasePolicyShardsGrouping", _schema20);

        public static readonly CommandSymbol DeleteDatabasePolicyShardsGrouping =
            new CommandSymbol("DeleteDatabasePolicyShardsGrouping", _schema20);

        public static readonly CommandSymbol ShowDatabasePolicyStreamingIngestion =
            new CommandSymbol("ShowDatabasePolicyStreamingIngestion", _schema20);

        public static readonly CommandSymbol ShowTablePolicyStreamingIngestion =
            new CommandSymbol("ShowTablePolicyStreamingIngestion", _schema20);

        public static readonly CommandSymbol ShowClusterPolicyStreamingIngestion =
            new CommandSymbol("ShowClusterPolicyStreamingIngestion", _schema20);

        public static readonly CommandSymbol AlterDatabasePolicyStreamingIngestion =
            new CommandSymbol("AlterDatabasePolicyStreamingIngestion", _schema20);

        public static readonly CommandSymbol AlterMergeDatabasePolicyStreamingIngestion =
            new CommandSymbol("AlterMergeDatabasePolicyStreamingIngestion", _schema20);

        public static readonly CommandSymbol AlterTablePolicyStreamingIngestion =
            new CommandSymbol("AlterTablePolicyStreamingIngestion", _schema20);

        public static readonly CommandSymbol AlterMergeTablePolicyStreamingIngestion =
            new CommandSymbol("AlterMergeTablePolicyStreamingIngestion", _schema20);

        public static readonly CommandSymbol AlterClusterPolicyStreamingIngestion =
            new CommandSymbol("AlterClusterPolicyStreamingIngestion", _schema20);

        public static readonly CommandSymbol AlterMergeClusterPolicyStreamingIngestion =
            new CommandSymbol("AlterMergeClusterPolicyStreamingIngestion", _schema20);

        public static readonly CommandSymbol DeleteDatabasePolicyStreamingIngestion =
            new CommandSymbol("DeleteDatabasePolicyStreamingIngestion", _schema20);

        public static readonly CommandSymbol DeleteTablePolicyStreamingIngestion =
            new CommandSymbol("DeleteTablePolicyStreamingIngestion", _schema20);

        public static readonly CommandSymbol DeleteClusterPolicyStreamingIngestion =
            new CommandSymbol("DeleteClusterPolicyStreamingIngestion", _schema20);

        public static readonly CommandSymbol ShowDatabasePolicyManagedIdentity =
            new CommandSymbol("ShowDatabasePolicyManagedIdentity", _schema20);

        public static readonly CommandSymbol ShowClusterPolicyManagedIdentity =
            new CommandSymbol("ShowClusterPolicyManagedIdentity", _schema20);

        public static readonly CommandSymbol AlterDatabasePolicyManagedIdentity =
            new CommandSymbol("AlterDatabasePolicyManagedIdentity", _schema20);

        public static readonly CommandSymbol AlterClusterPolicyManagedIdentity =
            new CommandSymbol("AlterClusterPolicyManagedIdentity", _schema20);

        public static readonly CommandSymbol DeleteDatabasePolicyManagedIdentity =
            new CommandSymbol("DeleteDatabasePolicyManagedIdentity", _schema20);

        public static readonly CommandSymbol DeleteClusterPolicyManagedIdentity =
            new CommandSymbol("DeleteClusterPolicyManagedIdentity", _schema20);

        public static readonly CommandSymbol ShowTablePolicyAutoDelete =
            new CommandSymbol("ShowTablePolicyAutoDelete", _schema20);

        public static readonly CommandSymbol AlterTablePolicyAutoDelete =
            new CommandSymbol("AlterTablePolicyAutoDelete", _schema20);

        public static readonly CommandSymbol DeleteTablePolicyAutoDelete =
            new CommandSymbol("DeleteTablePolicyAutoDelete", _schema20);

        public static readonly CommandSymbol ShowClusterPolicyCallout =
            new CommandSymbol("ShowClusterPolicyCallout", _schema20);

        public static readonly CommandSymbol AlterClusterPolicyCallout =
            new CommandSymbol("AlterClusterPolicyCallout", _schema20);

        public static readonly CommandSymbol AlterMergeClusterPolicyCallout =
            new CommandSymbol("AlterMergeClusterPolicyCallout", _schema20);

        public static readonly CommandSymbol DeleteClusterPolicyCallout =
            new CommandSymbol("DeleteClusterPolicyCallout", _schema20);

        public static readonly CommandSymbol ShowClusterPolicyCapacity =
            new CommandSymbol("ShowClusterPolicyCapacity", _schema20);

        public static readonly CommandSymbol AlterClusterPolicyCapacity =
            new CommandSymbol("AlterClusterPolicyCapacity", _schema20);

        public static readonly CommandSymbol AlterMergeClusterPolicyCapacity =
            new CommandSymbol("AlterMergeClusterPolicyCapacity", _schema20);

        public static readonly CommandSymbol ShowClusterPolicyRequestClassification =
            new CommandSymbol("ShowClusterPolicyRequestClassification", _schema20);

        public static readonly CommandSymbol AlterClusterPolicyRequestClassification =
            new CommandSymbol("AlterClusterPolicyRequestClassification", _schema20);

        public static readonly CommandSymbol AlterMergeClusterPolicyRequestClassification =
            new CommandSymbol("AlterMergeClusterPolicyRequestClassification", _schema20);

        public static readonly CommandSymbol DeleteClusterPolicyRequestClassification =
            new CommandSymbol("DeleteClusterPolicyRequestClassification", _schema20);

        public static readonly CommandSymbol ShowClusterPolicyMultiDatabaseAdmins =
            new CommandSymbol("ShowClusterPolicyMultiDatabaseAdmins", _schema20);

        public static readonly CommandSymbol AlterClusterPolicyMultiDatabaseAdmins =
            new CommandSymbol("AlterClusterPolicyMultiDatabaseAdmins", _schema20);

        public static readonly CommandSymbol AlterMergeClusterPolicyMultiDatabaseAdmins =
            new CommandSymbol("AlterMergeClusterPolicyMultiDatabaseAdmins", _schema20);

        public static readonly CommandSymbol ShowDatabasePolicyDiagnostics =
            new CommandSymbol("ShowDatabasePolicyDiagnostics", _schema20);

        public static readonly CommandSymbol ShowClusterPolicyDiagnostics =
            new CommandSymbol("ShowClusterPolicyDiagnostics", _schema20);

        public static readonly CommandSymbol AlterDatabasePolicyDiagnostics =
            new CommandSymbol("AlterDatabasePolicyDiagnostics", _schema20);

        public static readonly CommandSymbol AlterMergeDatabasePolicyDiagnostics =
            new CommandSymbol("AlterMergeDatabasePolicyDiagnostics", _schema20);

        public static readonly CommandSymbol AlterClusterPolicyDiagnostics =
            new CommandSymbol("AlterClusterPolicyDiagnostics", _schema20);

        public static readonly CommandSymbol AlterMergeClusterPolicyDiagnostics =
            new CommandSymbol("AlterMergeClusterPolicyDiagnostics", _schema20);

        public static readonly CommandSymbol DeleteDatabasePolicyDiagnostics =
            new CommandSymbol("DeleteDatabasePolicyDiagnostics", _schema20);

        public static readonly CommandSymbol ShowClusterPolicyQueryWeakConsistency =
            new CommandSymbol("ShowClusterPolicyQueryWeakConsistency", _schema20);

        public static readonly CommandSymbol AlterClusterPolicyQueryWeakConsistency =
            new CommandSymbol("AlterClusterPolicyQueryWeakConsistency", _schema20);

        public static readonly CommandSymbol AlterMergeClusterPolicyQueryWeakConsistency =
            new CommandSymbol("AlterMergeClusterPolicyQueryWeakConsistency", _schema20);

        public static readonly CommandSymbol ShowTablePolicyExtentTagsRetention =
            new CommandSymbol("ShowTablePolicyExtentTagsRetention", _schema20);

        public static readonly CommandSymbol ShowTableStarPolicyExtentTagsRetention =
            new CommandSymbol("ShowTableStarPolicyExtentTagsRetention", _schema20);

        public static readonly CommandSymbol ShowDatabasePolicyExtentTagsRetention =
            new CommandSymbol("ShowDatabasePolicyExtentTagsRetention", _schema20);

        public static readonly CommandSymbol AlterTablePolicyExtentTagsRetention =
            new CommandSymbol("AlterTablePolicyExtentTagsRetention", _schema20);

        public static readonly CommandSymbol AlterDatabasePolicyExtentTagsRetention =
            new CommandSymbol("AlterDatabasePolicyExtentTagsRetention", _schema20);

        public static readonly CommandSymbol DeleteTablePolicyExtentTagsRetention =
            new CommandSymbol("DeleteTablePolicyExtentTagsRetention", _schema20);

        public static readonly CommandSymbol DeleteDatabasePolicyExtentTagsRetention =
            new CommandSymbol("DeleteDatabasePolicyExtentTagsRetention", _schema20);

        public static readonly CommandSymbol ShowPrincipalRoles =
            new CommandSymbol("ShowPrincipalRoles", _schema21);

        public static readonly CommandSymbol ShowDatabasePrincipalRoles =
            new CommandSymbol("ShowDatabasePrincipalRoles", _schema21);

        public static readonly CommandSymbol ShowTablePrincipalRoles =
            new CommandSymbol("ShowTablePrincipalRoles", _schema21);

        public static readonly CommandSymbol ShowExternalTablesPrincipalRoles =
            new CommandSymbol("ShowExternalTablesPrincipalRoles", _schema21);

        public static readonly CommandSymbol ShowFunctionPrincipalRoles =
            new CommandSymbol("ShowFunctionPrincipalRoles", _schema21);

        public static readonly CommandSymbol ShowClusterPrincipalRoles =
            new CommandSymbol("ShowClusterPrincipalRoles", _schema21);

        public static readonly CommandSymbol ShowClusterPrincipals =
            new CommandSymbol("ShowClusterPrincipals", _schema22);

        public static readonly CommandSymbol ShowDatabasePrincipals =
            new CommandSymbol("ShowDatabasePrincipals", _schema22);

        public static readonly CommandSymbol ShowTablePrincipals =
            new CommandSymbol("ShowTablePrincipals", _schema22);

        public static readonly CommandSymbol ShowFunctionPrincipals =
            new CommandSymbol("ShowFunctionPrincipals", _schema22);

        public static readonly CommandSymbol AddClusterRole =
            new CommandSymbol("AddClusterRole", _schema22);

        public static readonly CommandSymbol DropClusterRole =
            new CommandSymbol("DropClusterRole", _schema22);

        public static readonly CommandSymbol SetClusterRole =
            new CommandSymbol("SetClusterRole", _schema22);

        public static readonly CommandSymbol AddDatabaseRole =
            new CommandSymbol("AddDatabaseRole", _schema22);

        public static readonly CommandSymbol DropDatabaseRole =
            new CommandSymbol("DropDatabaseRole", _schema22);

        public static readonly CommandSymbol SetDatabaseRole =
            new CommandSymbol("SetDatabaseRole", _schema22);

        public static readonly CommandSymbol AddTableRole =
            new CommandSymbol("AddTableRole", _schema22);

        public static readonly CommandSymbol DropTableRole =
            new CommandSymbol("DropTableRole", _schema22);

        public static readonly CommandSymbol SetTableRole =
            new CommandSymbol("SetTableRole", _schema22);

        public static readonly CommandSymbol AddFunctionRole =
            new CommandSymbol("AddFunctionRole", _schema22);

        public static readonly CommandSymbol DropFunctionRole =
            new CommandSymbol("DropFunctionRole", _schema22);

        public static readonly CommandSymbol SetFunctionRole =
            new CommandSymbol("SetFunctionRole", _schema22);

        public static readonly CommandSymbol ShowClusterBlockedPrincipals =
            new CommandSymbol("ShowClusterBlockedPrincipals", _schema23);

        public static readonly CommandSymbol AddClusterBlockedPrincipals =
            new CommandSymbol("AddClusterBlockedPrincipals", _schema23);

        public static readonly CommandSymbol DropClusterBlockedPrincipals =
            new CommandSymbol("DropClusterBlockedPrincipals", _schema23);

        public static readonly CommandSymbol IngestIntoTable =
            new CommandSymbol(
                "IngestIntoTable",
                "(ExtentId: guid, ItemLoaded: string, Duration: string, HasErrors: string, OperationId: guid)");

        public static readonly CommandSymbol IngestInlineIntoTable =
            new CommandSymbol(
                "IngestInlineIntoTable",
                "(ExtentId: guid)");

        public static readonly CommandSymbol SetTable =
            new CommandSymbol("SetTable", _schema24);

        public static readonly CommandSymbol AppendTable =
            new CommandSymbol("AppendTable", _schema24);

        public static readonly CommandSymbol SetOrAppendTable =
            new CommandSymbol("SetOrAppendTable", _schema24);

        public static readonly CommandSymbol SetOrReplaceTable =
            new CommandSymbol("SetOrReplaceTable", _schema24);

        public static readonly CommandSymbol ExportToStorage =
            new CommandSymbol("ExportToStorage", _schema18);

        public static readonly CommandSymbol ExportToSqlTable =
            new CommandSymbol("ExportToSqlTable", _schema18);

        public static readonly CommandSymbol ExportToExternalTable =
            new CommandSymbol("ExportToExternalTable", _schema18);

        public static readonly CommandSymbol CreateOrAlterContinuousExport =
            new CommandSymbol("CreateOrAlterContinuousExport", _schema18);

        public static readonly CommandSymbol ShowContinuousExport =
            new CommandSymbol("ShowContinuousExport", _schema25);

        public static readonly CommandSymbol ShowContinuousExports =
            new CommandSymbol("ShowContinuousExports", _schema25);

        public static readonly CommandSymbol ShowClusterPendingContinuousExports =
            new CommandSymbol("ShowClusterPendingContinuousExports", _schema25);

        public static readonly CommandSymbol ShowContinuousExportExportedArtifacts =
            new CommandSymbol(
                "ShowContinuousExportExportedArtifacts",
                "(Timestamp: datetime, ExternalTableName: string, Path: string, NumRecords: long)");

        public static readonly CommandSymbol ShowContinuousExportFailures =
            new CommandSymbol(
                "ShowContinuousExportFailures",
                "(Timestamp: datetime, OperationId: string, Name: string, LastSuccessRun: datetime, FailureKind: string, Details: string)");

        public static readonly CommandSymbol SetContinuousExportCursor =
            new CommandSymbol("SetContinuousExportCursor", _schema18);

        public static readonly CommandSymbol DropContinuousExport =
            new CommandSymbol("DropContinuousExport", _schema25);

        public static readonly CommandSymbol EnableContinuousExport =
            new CommandSymbol("EnableContinuousExport", _schema25);

        public static readonly CommandSymbol DisableContinuousExport =
            new CommandSymbol("DisableContinuousExport", _schema25);

        public static readonly CommandSymbol CreateMaterializedView =
            new CommandSymbol("CreateMaterializedView", _schema18);

        public static readonly CommandSymbol RenameMaterializedView =
            new CommandSymbol("RenameMaterializedView", _schema26);

        public static readonly CommandSymbol ShowMaterializedView =
            new CommandSymbol("ShowMaterializedView", _schema26);

        public static readonly CommandSymbol ShowMaterializedViews =
            new CommandSymbol("ShowMaterializedViews", _schema26);

        public static readonly CommandSymbol ShowMaterializedViewsDetails =
            new CommandSymbol("ShowMaterializedViewsDetails", _schema27);

        public static readonly CommandSymbol ShowMaterializedViewDetails =
            new CommandSymbol("ShowMaterializedViewDetails", _schema27);

        public static readonly CommandSymbol ShowMaterializedViewPolicyRetention =
            new CommandSymbol("ShowMaterializedViewPolicyRetention", _schema20);

        public static readonly CommandSymbol ShowMaterializedViewPolicyMerge =
            new CommandSymbol("ShowMaterializedViewPolicyMerge", _schema20);

        public static readonly CommandSymbol ShowMaterializedViewPolicyPartitioning =
            new CommandSymbol("ShowMaterializedViewPolicyPartitioning", _schema20);

        public static readonly CommandSymbol ShowMaterializedViewExtents =
            new CommandSymbol("ShowMaterializedViewExtents", _schema29);

        public static readonly CommandSymbol AlterMaterializedView =
            new CommandSymbol("AlterMaterializedView", _schema26);

        public static readonly CommandSymbol CreateOrAlterMaterializedView =
            new CommandSymbol("CreateOrAlterMaterializedView", _schema26);

        public static readonly CommandSymbol DropMaterializedView =
            new CommandSymbol("DropMaterializedView", _schema26);

        public static readonly CommandSymbol EnableDisableMaterializedView =
            new CommandSymbol("EnableDisableMaterializedView", _schema26);

        public static readonly CommandSymbol ShowMaterializedViewPrincipals =
            new CommandSymbol("ShowMaterializedViewPrincipals", _schema22);

        public static readonly CommandSymbol ShowMaterializedViewSchemaAsJson =
            new CommandSymbol("ShowMaterializedViewSchemaAsJson", _schema10);

        public static readonly CommandSymbol ShowMaterializedViewCslSchema =
            new CommandSymbol("ShowMaterializedViewCslSchema", _schema10);

        public static readonly CommandSymbol AlterMaterializedViewFolder =
            new CommandSymbol("AlterMaterializedViewFolder", _schema26);

        public static readonly CommandSymbol AlterMaterializedViewDocString =
            new CommandSymbol("AlterMaterializedViewDocString", _schema26);

        public static readonly CommandSymbol AlterMaterializedViewLookback =
            new CommandSymbol("AlterMaterializedViewLookback", _schema26);

        public static readonly CommandSymbol AlterMaterializedViewAutoUpdateSchema =
            new CommandSymbol("AlterMaterializedViewAutoUpdateSchema", _schema26);

        public static readonly CommandSymbol ClearMaterializedViewData =
            new CommandSymbol("ClearMaterializedViewData", _schema34);

        public static readonly CommandSymbol SetMaterializedViewCursor =
            new CommandSymbol("SetMaterializedViewCursor", _schema18);

        public static readonly CommandSymbol ShowCluster =
            new CommandSymbol(
                "ShowCluster",
                "(NodeId: string, Address: string, Name: string, StartTime: datetime, IsAdmin: bool, MachineTotalMemory: long, MachineAvailableMemory: long, ProcessorCount: int, EnvironmentDescription: string)");

        public static readonly CommandSymbol ShowDiagnostics =
            new CommandSymbol(
                "ShowDiagnostics",
                "(IsHealthy: bool, EnvironmentDescription: string, IsScaleOutRequired: bool, MachinesTotal: int, MachinesOffline: int, NodeLastRestartedOn: datetime, AdminLastElectedOn: datetime, ExtentsTotal: int, DiskColdAllocationPercentage: int, InstancesTargetBasedOnDataCapacity: int, TotalOriginalDataSize: real, TotalExtentSize: real, IngestionsLoadFactor: real, IngestionsInProgress: long, IngestionsSuccessRate: real, MergesInProgress: long, BuildVersion: string, BuildTime: datetime, ClusterDataCapacityFactor: real, IsDataWarmingRequired: bool, DataWarmingLastRunOn: datetime, MergesSuccessRate: real, NotHealthyReason: string, IsAttentionRequired: bool, AttentionRequiredReason: string, ProductVersion: string, FailedIngestOperations: int, FailedMergeOperations: int, MaxExtentsInSingleTable: int, TableWithMaxExtents: string, WarmExtentSize: real, NumberOfDatabases: int, PurgeExtentsRebuildLoadFactor: real, PurgeExtentsRebuildInProgress: long, PurgesInProgress: long, MaxSoftRetentionPolicyViolation: timespan, RowStoreLocalStorageCapacityFactor: real, ExportsLoadFactor: real, ExportsInProgress: long, PendingContinuousExports: long, MaxContinuousExportLatenessMinutes: long, RowStoreSealsInProgress: long, IsRowStoreUnhealthy: bool, MachinesSuspended: int, DataPartitioningLoadFactor: real, DataPartitioningOperationsInProgress: long, MinPartitioningPercentageInSingleTable: real, TableWithMinPartitioningPercentage: string, V2DataCapacityFactor: real, V3DataCapacityFactor: real, CurrentDiskCacheShardsPercentage: int, TargetDiskCacheShardsPercentage: int, MaterializedViewsInProgress: long, DataPartitioningOperationsInProgress: real, IngestionCapacityUtilization: real, ShardsWarmingStatus: string, ShardsWarmingTemperature: real, ShardsWarmingDetails: string, StoredQueryResultsInProgress: long, HotDataDiskSpaceUsage: real)");

        public static readonly CommandSymbol ShowCapacity =
            new CommandSymbol(
                "ShowCapacity",
                "(Resource: string, Total: long, Consumed: long, Remaining: long)");

        public static readonly CommandSymbol ShowOperations =
            new CommandSymbol(
                "ShowOperations",
                "(OperationId: guid, Operation: string, NodeId: string, StartedOn: datetime, LastUpdatedOn: datetime, Duration: timespan, State: string, Status: string, RootActivityId: guid, ShouldRetry: bool, Database: string, Principal: string, User: string, AdminEpochStartTime: datetime)");

        public static readonly CommandSymbol ShowOperationDetails =
            new CommandSymbol("ShowOperationDetails", _schema18);

        public static readonly CommandSymbol ShowJournal =
            new CommandSymbol("ShowJournal", _schema28);

        public static readonly CommandSymbol ShowDatabaseJournal =
            new CommandSymbol("ShowDatabaseJournal", _schema28);

        public static readonly CommandSymbol ShowClusterJournal =
            new CommandSymbol("ShowClusterJournal", _schema28);

        public static readonly CommandSymbol ShowQueries =
            new CommandSymbol(
                "ShowQueries",
                "(ClientActivityId: string, Text: string, Database: string, StartedOn: datetime, LastUpdatedOn: datetime, Duration: timespan, State: string, RootActivityId: guid, User: string, FailureReason: string, TotalCpu: timespan, CacheStatistics: dynamic, Application: string, MemoryPeak: long, ScannedExtentsStatistics: dynamic, Principal: string, ClientRequestProperties: dynamic, ResultSetStatistics: dynamic, WorkloadGroup: string)");

        public static readonly CommandSymbol ShowRunningQueries =
            new CommandSymbol(
                "ShowRunningQueries",
                "(ClientActivityId: string, Text: string, Database: string, StartedOn: datetime, LastUpdatedOn: datetime, Duration: timespan, State: string, RootActivityId: guid, User: string, FailureReason: string, TotalCpu: timespan, CacheStatistics: dynamic, Application: string, MemoryPeak: long, ScannedEventStatistics: dynamic, Pricipal: string, ClientRequestProperties: dynamic, ResultSetStatistics: dynamic, WorkloadGroup: string)");

        public static readonly CommandSymbol CancelQuery =
            new CommandSymbol("CancelQuery", _schema18);

        public static readonly CommandSymbol ShowQueryPlan =
            new CommandSymbol(
                "ShowQueryPlan",
                "(ResultType: string, Format: string, Content: string)");

        public static readonly CommandSymbol ShowBasicAuthUsers =
            new CommandSymbol(
                "ShowBasicAuthUsers",
                "(UserName: string)");

        public static readonly CommandSymbol CreateBasicAuthUser =
            new CommandSymbol("CreateBasicAuthUser", _schema18);

        public static readonly CommandSymbol DropBasicAuthUser =
            new CommandSymbol("DropBasicAuthUser", _schema18);

        public static readonly CommandSymbol ShowCache =
            new CommandSymbol(
                "ShowCache",
                "(NodeId: string, TotalMemoryCapacity: long, MemoryCacheCapacity: long, MemoryCacheInUse: long, MemoryCacheHitCount: long, TotalDiskCapacity: long, DiskCacheCapacity: long, DiskCacheInUse: long, DiskCacheHitCount: long, DiskCacheMissCount: long, MemoryCacheDetails: string, DiskCacheDetails: string, WarmedShardsSize: long, ShardsSizeToWarm: long, HotShardsSize: long)");

        public static readonly CommandSymbol AlterCache =
            new CommandSymbol("AlterCache", _schema18);

        public static readonly CommandSymbol ShowCommands =
            new CommandSymbol(
                "ShowCommands",
                "(ClientActivityId: string, CommandType: string, Text: string, Database: string, StartedOn: datetime, LastUpdatedOn: datetime, Duration: timespan, State: string, RootActivityId: guid, User: string, FailureReason: string, Application: string, Principal: string, TotalCpu: timespan, ResourcesUtilization: dynamic, ClientRequestProperties: dynamic, WorkloadGroup: string)");

        public static readonly CommandSymbol ShowCommandsAndQueries =
            new CommandSymbol(
                "ShowCommandsAndQueries",
                "(ClientActivityId: string, CommandType: string, Text: string, Database: string, StartedOn: datetime, LastUpdatedOn: datetime, Duration: timespan, State: string, FailureReason: string, RootActivityId: guid, User: string, Application: string, Principal: string, ClientRequestProperties: dynamic, TotalCpu: timespan, MemoryPeak: long, CacheStatistics: dynamic, ScannedExtentsStatistics: dynamic, ResultSetStatistics: dynamic, WorkloadGroup: string)");

        public static readonly CommandSymbol ShowIngestionFailures =
            new CommandSymbol(
                "ShowIngestionFailures",
                "(OperationId: guid, Database: string, Table: string, FailedOn: datetime, IngestionSourcePath: string, Details: string, FailureKind: string, RootActivityId: guid, OperationKind: string, OriginatesFromUpdatePolicy: bool, ErrorCode: string, Principal: string, ShouldRetry: bool, User: string, IngestionProperties: string)");

        public static readonly CommandSymbol ShowClusterExtents =
            new CommandSymbol("ShowClusterExtents", _schema29);

        public static readonly CommandSymbol ShowClusterExtentsMetadata =
            new CommandSymbol("ShowClusterExtentsMetadata", _schema30);

        public static readonly CommandSymbol ShowDatabaseExtents =
            new CommandSymbol("ShowDatabaseExtents", _schema29);

        public static readonly CommandSymbol ShowDatabaseExtentsMetadata =
            new CommandSymbol("ShowDatabaseExtentsMetadata", _schema30);

        public static readonly CommandSymbol ShowDatabaseExtentTagsStatistics =
            new CommandSymbol(
                "ShowDatabaseExtentTagsStatistics",
                "(TableName: string, TotalExtentsCount: long, TaggedExtentsCount: long, TotalTagsCount: long, TotalTagsLength: long, DropByTagsCount: long, DropByTagsLength: long, IngestByTagsCount: long, IngestByTagsLength: long)");

        public static readonly CommandSymbol ShowTableExtents =
            new CommandSymbol("ShowTableExtents", _schema29);

        public static readonly CommandSymbol ShowTableExtentsMetadata =
            new CommandSymbol("ShowTableExtentsMetadata", _schema30);

        public static readonly CommandSymbol TableShardGroupsShow =
            new CommandSymbol(
                "TableShardGroupsShow",
                "(Id: guid, ShardCount: long, RowCount: long, OriginalSize: long, ShardSize: long, CompressedSize: long, IndexSize: long, DeletedRowCount: long, V2ShardCount: long, V2RowCount: long, DateTimeColumnRanges: dynamic, Partition: dynamic)");

        public static readonly CommandSymbol TableShardGroupsStatisticsShow =
            new CommandSymbol("TableShardGroupsStatisticsShow", _schema31);

        public static readonly CommandSymbol TablesShardGroupsStatisticsShow =
            new CommandSymbol("TablesShardGroupsStatisticsShow", _schema31);

        public static readonly CommandSymbol DatabaseShardGroupsStatisticsShow =
            new CommandSymbol("DatabaseShardGroupsStatisticsShow", _schema31);

        public static readonly CommandSymbol MergeExtents =
            new CommandSymbol("MergeExtents", _schema32);

        public static readonly CommandSymbol MergeExtentsDryrun =
            new CommandSymbol("MergeExtentsDryrun", _schema32);

        public static readonly CommandSymbol MoveExtentsFrom =
            new CommandSymbol("MoveExtentsFrom", _schema33);

        public static readonly CommandSymbol MoveExtentsQuery =
            new CommandSymbol("MoveExtentsQuery", _schema33);

        public static readonly CommandSymbol ReplaceExtents =
            new CommandSymbol("ReplaceExtents", _schema33);

        public static readonly CommandSymbol DropExtent =
            new CommandSymbol("DropExtent", _schema34);

        public static readonly CommandSymbol DropExtents =
            new CommandSymbol("DropExtents", _schema34);

        public static readonly CommandSymbol DropExtentsPartitionMetadata =
            new CommandSymbol("DropExtentsPartitionMetadata", _schema18);

        public static readonly CommandSymbol DropPretendExtentsByProperties =
            new CommandSymbol("DropPretendExtentsByProperties", _schema34);

        public static readonly CommandSymbol ShowVersion =
            new CommandSymbol(
                "ShowVersion",
                "(BuildVersion: string, BuildTime: datetime, ServiceType: string, ProductVersion: string)");

        public static readonly CommandSymbol ClearTableData =
            new CommandSymbol(
                "ClearTableData",
                "(Status: string)");

        public static readonly CommandSymbol ClearTableCacheStreamingIngestionSchema =
            new CommandSymbol(
                "ClearTableCacheStreamingIngestionSchema",
                "(NodeId: string, Status: string)");

        public static readonly CommandSymbol StoredQueryResultSet =
            new CommandSymbol("StoredQueryResultSet", _schema18);

        public static readonly CommandSymbol StoredQueryResultSetOrReplace =
            new CommandSymbol("StoredQueryResultSetOrReplace", _schema18);

        public static readonly CommandSymbol StoredQueryResultsShow =
            new CommandSymbol("StoredQueryResultsShow", _schema35);

        public static readonly CommandSymbol StoredQueryResultShowSchema =
            new CommandSymbol(
                "StoredQueryResultShowSchema",
                "(StoredQueryResult:string, Schema:string)");

        public static readonly CommandSymbol StoredQueryResultDrop =
            new CommandSymbol("StoredQueryResultDrop", _schema35);

        public static readonly CommandSymbol StoredQueryResultsDrop =
            new CommandSymbol("StoredQueryResultsDrop", _schema35);

        public static readonly CommandSymbol CreateRequestSupport =
            new CommandSymbol("CreateRequestSupport", _schema18);

        public static readonly CommandSymbol ShowRequestSupport =
            new CommandSymbol("ShowRequestSupport", _schema18);

        public static readonly CommandSymbol ShowClusterAdminState =
            new CommandSymbol("ShowClusterAdminState", _schema18);

        public static readonly CommandSymbol ClearRemoteClusterDatabaseSchema =
            new CommandSymbol("ClearRemoteClusterDatabaseSchema", _schema18);

        public static readonly CommandSymbol ShowClusterMonitoring =
            new CommandSymbol("ShowClusterMonitoring", _schema18);

        public static readonly CommandSymbol ShowClusterScaleIn =
            new CommandSymbol("ShowClusterScaleIn", _schema18);

        public static readonly CommandSymbol ShowClusterServices =
            new CommandSymbol(
                "ShowClusterServices",
                "(NodeId: string, IsClusterAdmin: bool, IsFabricManager: bool, IsDatabaseAdmin: bool, IsWeakConsistencyNode: bool, IsRowStoreHostNode: bool, IsDataNode: bool)");

        public static readonly CommandSymbol ShowClusterNetwork =
            new CommandSymbol("ShowClusterNetwork", _schema18);

        public static readonly CommandSymbol AlterClusterStorageKeys =
            new CommandSymbol("AlterClusterStorageKeys", _schema18);

        public static readonly CommandSymbol ShowClusterStorageKeysHash =
            new CommandSymbol("ShowClusterStorageKeysHash", _schema18);

        public static readonly CommandSymbol CreateEntityGroupCommand =
            new CommandSymbol("CreateEntityGroupCommand", _schema36);

        public static readonly CommandSymbol AlterEntityGroup =
            new CommandSymbol("AlterEntityGroup", _schema36);

        public static readonly CommandSymbol AlterMergeEntityGroup =
            new CommandSymbol("AlterMergeEntityGroup", _schema36);

        public static readonly CommandSymbol DropEntityGroup =
            new CommandSymbol("DropEntityGroup", _schema36);

        public static readonly CommandSymbol ShowEntityGroup =
            new CommandSymbol("ShowEntityGroup", _schema36);

        public static readonly CommandSymbol ShowEntityGroups =
            new CommandSymbol("ShowEntityGroups", _schema36);

        public static readonly CommandSymbol AlterExtentContainersAdd =
            new CommandSymbol("AlterExtentContainersAdd", _schema18);

        public static readonly CommandSymbol AlterExtentContainersDrop =
            new CommandSymbol("AlterExtentContainersDrop", _schema18);

        public static readonly CommandSymbol AlterExtentContainersRecycle =
            new CommandSymbol("AlterExtentContainersRecycle", _schema18);

        public static readonly CommandSymbol AlterExtentContainersSet =
            new CommandSymbol("AlterExtentContainersSet", _schema18);

        public static readonly CommandSymbol ShowExtentContainers =
            new CommandSymbol("ShowExtentContainers", _schema18);

        public static readonly CommandSymbol DropEmptyExtentContainers =
            new CommandSymbol("DropEmptyExtentContainers", _schema18);

        public static readonly CommandSymbol CleanDatabaseExtentContainers =
            new CommandSymbol("CleanDatabaseExtentContainers", _schema18);

        public static readonly CommandSymbol ShowDatabaseExtentContainersCleanOperations =
            new CommandSymbol("ShowDatabaseExtentContainersCleanOperations", _schema18);

        public static readonly CommandSymbol ClearDatabaseCacheQueryResults =
            new CommandSymbol("ClearDatabaseCacheQueryResults", _schema18);

        public static readonly CommandSymbol ShowDatabaseCacheQueryResults =
            new CommandSymbol("ShowDatabaseCacheQueryResults", _schema18);

        public static readonly CommandSymbol ShowDatabasesManagementGroups =
            new CommandSymbol("ShowDatabasesManagementGroups", _schema18);

        public static readonly CommandSymbol AlterDatabaseStorageKeys =
            new CommandSymbol("AlterDatabaseStorageKeys", _schema18);

        public static readonly CommandSymbol ClearDatabaseCacheStreamingIngestionSchema =
            new CommandSymbol("ClearDatabaseCacheStreamingIngestionSchema", _schema18);

        public static readonly CommandSymbol ClearDatabaseCacheQueryWeakConsistency =
            new CommandSymbol("ClearDatabaseCacheQueryWeakConsistency", _schema18);

        public static readonly CommandSymbol ShowEntitySchema =
            new CommandSymbol("ShowEntitySchema", _schema18);

        public static readonly CommandSymbol ShowExtentDetails =
            new CommandSymbol("ShowExtentDetails", _schema18);

        public static readonly CommandSymbol ShowExtentColumnStorageStats =
            new CommandSymbol("ShowExtentColumnStorageStats", _schema18);

        public static readonly CommandSymbol AttachExtentsIntoTableByContainer =
            new CommandSymbol("AttachExtentsIntoTableByContainer", _schema18);

        public static readonly CommandSymbol AttachExtentsIntoTableByMetadata =
            new CommandSymbol("AttachExtentsIntoTableByMetadata", _schema18);

        public static readonly CommandSymbol AlterExtentTagsFromQuery =
            new CommandSymbol("AlterExtentTagsFromQuery", _schema18);

        public static readonly CommandSymbol AlterMergeExtentTagsFromQuery =
            new CommandSymbol("AlterMergeExtentTagsFromQuery", _schema18);

        public static readonly CommandSymbol DropExtentTagsRetention =
            new CommandSymbol("DropExtentTagsRetention", _schema18);

        public static readonly CommandSymbol MergeDatabaseShardGroups =
            new CommandSymbol("MergeDatabaseShardGroups", _schema18);

        public static readonly CommandSymbol AlterFollowerClusterConfiguration =
            new CommandSymbol("AlterFollowerClusterConfiguration", _schema18);

        public static readonly CommandSymbol AddFollowerDatabaseAuthorizedPrincipals =
            new CommandSymbol("AddFollowerDatabaseAuthorizedPrincipals", _schema18);

        public static readonly CommandSymbol DropFollowerDatabaseAuthorizedPrincipals =
            new CommandSymbol("DropFollowerDatabaseAuthorizedPrincipals", _schema18);

        public static readonly CommandSymbol AlterFollowerDatabaseAuthorizedPrincipals =
            new CommandSymbol("AlterFollowerDatabaseAuthorizedPrincipals", _schema18);

        public static readonly CommandSymbol DropFollowerDatabasePolicyCaching =
            new CommandSymbol("DropFollowerDatabasePolicyCaching", _schema18);

        public static readonly CommandSymbol AlterFollowerDatabaseChildEntities =
            new CommandSymbol("AlterFollowerDatabaseChildEntities", _schema18);

        public static readonly CommandSymbol AlterFollowerDatabaseConfiguration =
            new CommandSymbol("AlterFollowerDatabaseConfiguration", _schema18);

        public static readonly CommandSymbol DropFollowerDatabases =
            new CommandSymbol("DropFollowerDatabases", _schema18);

        public static readonly CommandSymbol ShowFollowerDatabase =
            new CommandSymbol("ShowFollowerDatabase", _schema18);

        public static readonly CommandSymbol AlterFollowerTablesPolicyCaching =
            new CommandSymbol("AlterFollowerTablesPolicyCaching", _schema18);

        public static readonly CommandSymbol DropFollowerTablesPolicyCaching =
            new CommandSymbol("DropFollowerTablesPolicyCaching", _schema18);

        public static readonly CommandSymbol ShowFreshness =
            new CommandSymbol("ShowFreshness", _schema18);

        public static readonly CommandSymbol ShowFunctionSchemaAsJson =
            new CommandSymbol("ShowFunctionSchemaAsJson", _schema18);

        public static readonly CommandSymbol SetMaterializedViewAdmins =
            new CommandSymbol("SetMaterializedViewAdmins", _schema18);

        public static readonly CommandSymbol AddMaterializedViewAdmins =
            new CommandSymbol("AddMaterializedViewAdmins", _schema18);

        public static readonly CommandSymbol DropMaterializedViewAdmins =
            new CommandSymbol("DropMaterializedViewAdmins", _schema18);

        public static readonly CommandSymbol SetMaterializedViewConcurrency =
            new CommandSymbol("SetMaterializedViewConcurrency", _schema18);

        public static readonly CommandSymbol ClearMaterializedViewStatistics =
            new CommandSymbol("ClearMaterializedViewStatistics", _schema18);

        public static readonly CommandSymbol ShowMaterializedViewStatistics =
            new CommandSymbol("ShowMaterializedViewStatistics", _schema18);

        public static readonly CommandSymbol ShowMaterializedViewDiagnostics =
            new CommandSymbol("ShowMaterializedViewDiagnostics", _schema18);

        public static readonly CommandSymbol ShowMaterializedViewFailures =
            new CommandSymbol("ShowMaterializedViewFailures", _schema18);

        public static readonly CommandSymbol ShowMemory =
            new CommandSymbol("ShowMemory", _schema18);

        public static readonly CommandSymbol CancelOperation =
            new CommandSymbol("CancelOperation", _schema18);

        public static readonly CommandSymbol DisablePlugin =
            new CommandSymbol("DisablePlugin", _schema18);

        public static readonly CommandSymbol EnablePlugin =
            new CommandSymbol("EnablePlugin", _schema18);

        public static readonly CommandSymbol ShowPlugins =
            new CommandSymbol("ShowPlugins", _schema18);

        public static readonly CommandSymbol ShowPrincipalAccess =
            new CommandSymbol("ShowPrincipalAccess", _schema18);

        public static readonly CommandSymbol ShowDatabasePurgeOperation =
            new CommandSymbol("ShowDatabasePurgeOperation", _schema18);

        public static readonly CommandSymbol ShowQueryExecution =
            new CommandSymbol("ShowQueryExecution", _schema18);

        public static readonly CommandSymbol AlterPoliciesOfRetention =
            new CommandSymbol("AlterPoliciesOfRetention", _schema18);

        public static readonly CommandSymbol DeletePoliciesOfRetention =
            new CommandSymbol("DeletePoliciesOfRetention", _schema18);

        public static readonly CommandSymbol CreateRowStore =
            new CommandSymbol("CreateRowStore", _schema18);

        public static readonly CommandSymbol DropRowStore =
            new CommandSymbol("DropRowStore", _schema18);

        public static readonly CommandSymbol ShowRowStore =
            new CommandSymbol("ShowRowStore", _schema18);

        public static readonly CommandSymbol ShowRowStores =
            new CommandSymbol("ShowRowStores", _schema18);

        public static readonly CommandSymbol ShowRowStoreTransactions =
            new CommandSymbol("ShowRowStoreTransactions", _schema18);

        public static readonly CommandSymbol ShowRowStoreSeals =
            new CommandSymbol("ShowRowStoreSeals", _schema18);

        public static readonly CommandSymbol ShowSchema =
            new CommandSymbol("ShowSchema", _schema18);

        public static readonly CommandSymbol ShowCallStacks =
            new CommandSymbol("ShowCallStacks", _schema18);

        public static readonly CommandSymbol ShowStreamingIngestionFailures =
            new CommandSymbol("ShowStreamingIngestionFailures", _schema18);

        public static readonly CommandSymbol ShowStreamingIngestionStatistics =
            new CommandSymbol("ShowStreamingIngestionStatistics", _schema18);

        public static readonly CommandSymbol AlterTableRowStoreReferencesDropKey =
            new CommandSymbol("AlterTableRowStoreReferencesDropKey", _schema18);

        public static readonly CommandSymbol AlterTableRowStoreReferencesDropRowStore =
            new CommandSymbol("AlterTableRowStoreReferencesDropRowStore", _schema18);

        public static readonly CommandSymbol AlterTableRowStoreReferencesDropBlockedKeys =
            new CommandSymbol("AlterTableRowStoreReferencesDropBlockedKeys", _schema18);

        public static readonly CommandSymbol AlterTableRowStoreReferencesDisableKey =
            new CommandSymbol("AlterTableRowStoreReferencesDisableKey", _schema18);

        public static readonly CommandSymbol AlterTableRowStoreReferencesDisableRowStore =
            new CommandSymbol("AlterTableRowStoreReferencesDisableRowStore", _schema18);

        public static readonly CommandSymbol AlterTableRowStoreReferencesDisableBlockedKeys =
            new CommandSymbol("AlterTableRowStoreReferencesDisableBlockedKeys", _schema18);

        public static readonly CommandSymbol SetTableRowStoreReferences =
            new CommandSymbol("SetTableRowStoreReferences", _schema18);

        public static readonly CommandSymbol ShowTableRowStoreReferences =
            new CommandSymbol("ShowTableRowStoreReferences", _schema18);

        public static readonly CommandSymbol AlterTableColumnStatistics =
            new CommandSymbol("AlterTableColumnStatistics", _schema18);

        public static readonly CommandSymbol AlterTableColumnStatisticsMethod =
            new CommandSymbol("AlterTableColumnStatisticsMethod", _schema18);

        public static readonly CommandSymbol ShowTableColumnStatitics =
            new CommandSymbol("ShowTableColumnStatitics", _schema18);

        public static readonly CommandSymbol ShowTableDimensions =
            new CommandSymbol("ShowTableDimensions", _schema18);

        public static readonly CommandSymbol DeleteTableRecords =
            new CommandSymbol("DeleteTableRecords", _schema18);

        public static readonly CommandSymbol ShowTableColumnsClassification =
            new CommandSymbol("ShowTableColumnsClassification", _schema18);

        public static readonly CommandSymbol ShowTableRowStores =
            new CommandSymbol("ShowTableRowStores", _schema18);

        public static readonly CommandSymbol ShowTableRowStoreSealInfo =
            new CommandSymbol("ShowTableRowStoreSealInfo", _schema18);

        public static readonly CommandSymbol ShowTablesColumnStatistics =
            new CommandSymbol("ShowTablesColumnStatistics", _schema18);

        public static readonly CommandSymbol ShowTableUsageStatistics =
            new CommandSymbol("ShowTableUsageStatistics", _schema18);

        public static readonly CommandSymbol ShowTableUsageStatisticsDetails =
            new CommandSymbol("ShowTableUsageStatisticsDetails", _schema18);

        public static readonly CommandSymbol CreateTempStorage =
            new CommandSymbol("CreateTempStorage", _schema18);

        public static readonly CommandSymbol DropTempStorage =
            new CommandSymbol("DropTempStorage", _schema18);

        public static readonly CommandSymbol DropStoredQueryResultContainers =
            new CommandSymbol("DropStoredQueryResultContainers", _schema18);

        public static readonly CommandSymbol DropUnusedStoredQueryResultContainers =
            new CommandSymbol("DropUnusedStoredQueryResultContainers", _schema18);

        public static readonly CommandSymbol EnableDatabaseMaintenanceMode =
            new CommandSymbol("EnableDatabaseMaintenanceMode", _schema18);

        public static readonly CommandSymbol DisableDatabaseMaintenanceMode =
            new CommandSymbol("DisableDatabaseMaintenanceMode", _schema18);

        public static readonly CommandSymbol ShowQueryCallTree =
            new CommandSymbol("ShowQueryCallTree", _schema18);

        public static readonly CommandSymbol ShowExtentCorruptedDatetime =
            new CommandSymbol("ShowExtentCorruptedDatetime", _schema18);

        public static readonly CommandSymbol PatchExtentCorruptedDatetime =
            new CommandSymbol("PatchExtentCorruptedDatetime", _schema18);

        public static readonly IReadOnlyList<CommandSymbol> All = new CommandSymbol[]
        {
            ShowDatabase,
            ShowDatabaseDetails,
            ShowDatabaseIdentity,
            ShowDatabasePolicies,
            ShowDatabaseDataStats,
            ShowClusterDatabases,
            ShowClusterDatabasesDetails,
            ShowClusterDatabasesIdentity,
            ShowClusterDatabasesPolicies,
            ShowClusterDatabasesDataStats,
            CreateDatabasePersist,
            CreateDatabaseVolatile,
            AttachDatabase,
            AttachDatabaseMetadata,
            DetachDatabase,
            AlterDatabasePrettyName,
            DropDatabasePrettyName,
            AlterDatabasePersistMetadata,
            SetAccess,
            ShowDatabaseSchema,
            ShowDatabaseSchemaAsJson,
            ShowDatabaseSchemaAsCslScript,
            ShowDatabaseCslSchema,
            ShowDatabaseSchemaViolations,
            ShowDatabasesSchema,
            ShowDatabasesSchemaAsJson,
            CreateDatabaseIngestionMapping,
            AlterDatabaseIngestionMapping,
            ShowDatabaseIngestionMappings,
            ShowIngestionMappings,
            DropDatabaseIngestionMapping,
            ShowTables,
            ShowTable,
            ShowTablesDetails,
            ShowTableDetails,
            ShowTableCslSchema,
            ShowTableSchemaAsJson,
            CreateTable,
            CreateTableBasedOnAnother,
            CreateMergeTable,
            DefineTable,
            CreateTables,
            CreateMergeTables,
            DefineTables,
            AlterTable,
            AlterMergeTable,
            AlterTableDocString,
            AlterTableFolder,
            RenameTable,
            RenameTables,
            DropTable,
            UndoDropTable,
            DropTables,
            CreateTableIngestionMapping,
            AlterTableIngestionMapping,
            ShowTableIngestionMappings,
            ShowTableIngestionMapping,
            DropTableIngestionMapping,
            RenameColumn,
            RenameColumns,
            AlterColumnType,
            DropColumn,
            DropTableColumns,
            AlterTableColumnDocStrings,
            AlterMergeTableColumnDocStrings,
            ShowFunctions,
            ShowFunction,
            CreateFunction,
            AlterFunction,
            CreateOrAlterFunction,
            DropFunction,
            DropFunctions,
            AlterFunctionDocString,
            AlterFunctionFolder,
            ShowExternalTables,
            ShowExternalTable,
            ShowExternalTableCslSchema,
            ShowExternalTableSchema,
            ShowExternalTableArtifacts,
            DropExternalTable,
            CreateStorageExternalTable,
            AlterStorageExternalTable,
            CreateOrAlterStorageExternalTable,
            CreateSqlExternalTable,
            AlterSqlExternalTable,
            CreateOrAlterSqlExternalTable,
            CreateExternalTableMapping,
            SetExternalTableAdmins,
            AddExternalTableAdmins,
            DropExternalTableAdmins,
            AlterExternalTableDocString,
            AlterExternalTableFolder,
            ShowExternalTablePrincipals,
            ShowFabric,
            AlterExternalTableMapping,
            ShowExternalTableMappings,
            ShowExternalTableMapping,
            DropExternalTableMapping,
            ShowWorkloadGroups,
            ShowWorkloadGroup,
            CreateOrAleterWorkloadGroup,
            AlterMergeWorkloadGroup,
            DropWorkloadGroup,
            ShowDatabasePolicyCaching,
            ShowTablePolicyCaching,
            ShowTableStarPolicyCaching,
            ShowColumnPolicyCaching,
            ShowMaterializedViewPolicyCaching,
            ShowClusterPolicyCaching,
            AlterDatabasePolicyCaching,
            AlterTablePolicyCaching,
            AlterTablesPolicyCaching,
            AlterColumnPolicyCaching,
            AlterMaterializedViewPolicyCaching,
            AlterClusterPolicyCaching,
            DeleteDatabasePolicyCaching,
            DeleteTablePolicyCaching,
            DeleteColumnPolicyCaching,
            DeleteMaterializedViewPolicyCaching,
            DeleteClusterPolicyCaching,
            ShowTablePolicyIngestionTime,
            ShowTableStarPolicyIngestionTime,
            AlterTablePolicyIngestionTime,
            AlterTablesPolicyIngestionTime,
            DeleteTablePolicyIngestionTime,
            ShowTablePolicyRetention,
            ShowTableStarPolicyRetention,
            ShowDatabasePolicyRetention,
            AlterTablePolicyRetention,
            AlterMaterializedViewPolicyRetention,
            AlterDatabasePolicyRetention,
            AlterTablesPolicyRetention,
            AlterMergeTablePolicyRetention,
            AlterMergeMaterializedViewPolicyRetention,
            AlterMergeDatabasePolicyRetention,
            DeleteTablePolicyRetention,
            DeleteDatabasePolicyRetention,
            ShowDatabasePolicyHardRetentionViolations,
            ShowDatabasePolicySoftRetentionViolations,
            ShowTablePolicyRowLevelSecurity,
            ShowTableStarPolicyRowLevelSecurity,
            AlterTablePolicyRowLevelSecurity,
            DeleteTablePolicyRowLevelSecurity,
            ShowMaterializedViewPolicyRowLevelSecurity,
            AlterMaterializedViewPolicyRowLevelSecurity,
            DeleteMaterializedViewPolicyRowLevelSecurity,
            ShowTablePolicyRowOrder,
            ShowTableStarPolicyRowOrder,
            AlterTablePolicyRowOrder,
            AlterTablesPolicyRowOrder,
            AlterMergeTablePolicyRowOrder,
            DeleteTablePolicyRowOrder,
            ShowTablePolicyUpdate,
            ShowTableStarPolicyUpdate,
            AlterTablePolicyUpdate,
            AlterMergeTablePolicyUpdate,
            DeleteTablePolicyUpdate,
            ShowClusterPolicyIngestionBatching,
            ShowDatabasePolicyIngestionBatching,
            ShowTablePolicyIngestionBatching,
            ShowTableStarPolicyIngestionBatching,
            AlterClusterPolicyIngestionBatching,
            AlterDatabasePolicyIngestionBatching,
            AlterTablePolicyIngestionBatching,
            AlterTablesPolicyIngestionBatching,
            DeleteDatabasePolicyIngestionBatching,
            DeleteTablePolicyIngestionBatching,
            ShowDatabasePolicyEncoding,
            ShowTablePolicyEncoding,
            ShowColumnPolicyEncoding,
            AlterDatabasePolicyEncoding,
            AlterTablePolicyEncoding,
            AlterTableColumnsPolicyEncoding,
            AlterColumnPolicyEncoding,
            AlterColumnPolicyEncodingType,
            AlterMergeDatabasePolicyEncoding,
            AlterMergeTablePolicyEncoding,
            AlterMergeColumnPolicyEncoding,
            DeleteDatabasePolicyEncoding,
            DeleteTablePolicyEncoding,
            DeleteColumnPolicyEncoding,
            ShowDatabasePolicyMerge,
            ShowTablePolicyMerge,
            ShowTableStarPolicyMerge,
            AlterDatabasePolicyMerge,
            AlterTablePolicyMerge,
            AlterTablesPolicyMerge,
            AlterMergeDatabasePolicyMerge,
            AlterMergeTablePolicyMerge,
            DeleteDatabasePolicyMerge,
            DeleteTablePolicyMerge,
            ShowTablePolicyPartitioning,
            ShowTableStarPolicyPartitioning,
            AlterTablePolicyPartitioning,
            AlterMergeTablePolicyPartitioning,
            AlterMaterializedViewPolicyPartitioning,
            AlterMergeMaterializedViewPolicyPartitioning,
            DeleteTablePolicyPartitioning,
            DeleteMaterializedViewPolicyPartitioning,
            ShowTablePolicyRestrictedViewAccess,
            ShowTableStarPolicyRestrictedViewAccess,
            AlterTablePolicyRestrictedViewAccess,
            AlterTablesPolicyRestrictedViewAccess,
            DeleteTablePolicyRestrictedViewAccess,
            ShowClusterPolicyRowStore,
            AlterClusterPolicyRowStore,
            AlterMergeClusterPolicyRowStore,
            DeleteClusterPolicyRowStore,
            ShowClusterPolicySandbox,
            AlterClusterPolicySandbox,
            DeleteClusterPolicySandbox,
            ShowClusterSandboxesStats,
            ShowDatabasePolicySharding,
            ShowTablePolicySharding,
            ShowTableStarPolicySharding,
            AlterDatabasePolicySharding,
            AlterTablePolicySharding,
            AlterMergeDatabasePolicySharding,
            AlterMergeTablePolicySharding,
            DeleteDatabasePolicySharding,
            DeleteTablePolicySharding,
            AlterClusterPolicySharding,
            AlterMergeClusterPolicySharding,
            DeleteClusterPolicySharding,
            ShowClusterPolicySharding,
            ShowDatabasePolicyShardsGrouping,
            AlterDatabasePolicyShardsGrouping,
            AlterMergeDatabasePolicyShardsGrouping,
            DeleteDatabasePolicyShardsGrouping,
            ShowDatabasePolicyStreamingIngestion,
            ShowTablePolicyStreamingIngestion,
            ShowClusterPolicyStreamingIngestion,
            AlterDatabasePolicyStreamingIngestion,
            AlterMergeDatabasePolicyStreamingIngestion,
            AlterTablePolicyStreamingIngestion,
            AlterMergeTablePolicyStreamingIngestion,
            AlterClusterPolicyStreamingIngestion,
            AlterMergeClusterPolicyStreamingIngestion,
            DeleteDatabasePolicyStreamingIngestion,
            DeleteTablePolicyStreamingIngestion,
            DeleteClusterPolicyStreamingIngestion,
            ShowDatabasePolicyManagedIdentity,
            ShowClusterPolicyManagedIdentity,
            AlterDatabasePolicyManagedIdentity,
            AlterClusterPolicyManagedIdentity,
            DeleteDatabasePolicyManagedIdentity,
            DeleteClusterPolicyManagedIdentity,
            ShowTablePolicyAutoDelete,
            AlterTablePolicyAutoDelete,
            DeleteTablePolicyAutoDelete,
            ShowClusterPolicyCallout,
            AlterClusterPolicyCallout,
            AlterMergeClusterPolicyCallout,
            DeleteClusterPolicyCallout,
            ShowClusterPolicyCapacity,
            AlterClusterPolicyCapacity,
            AlterMergeClusterPolicyCapacity,
            ShowClusterPolicyRequestClassification,
            AlterClusterPolicyRequestClassification,
            AlterMergeClusterPolicyRequestClassification,
            DeleteClusterPolicyRequestClassification,
            ShowClusterPolicyMultiDatabaseAdmins,
            AlterClusterPolicyMultiDatabaseAdmins,
            AlterMergeClusterPolicyMultiDatabaseAdmins,
            ShowDatabasePolicyDiagnostics,
            ShowClusterPolicyDiagnostics,
            AlterDatabasePolicyDiagnostics,
            AlterMergeDatabasePolicyDiagnostics,
            AlterClusterPolicyDiagnostics,
            AlterMergeClusterPolicyDiagnostics,
            DeleteDatabasePolicyDiagnostics,
            ShowClusterPolicyQueryWeakConsistency,
            AlterClusterPolicyQueryWeakConsistency,
            AlterMergeClusterPolicyQueryWeakConsistency,
            ShowTablePolicyExtentTagsRetention,
            ShowTableStarPolicyExtentTagsRetention,
            ShowDatabasePolicyExtentTagsRetention,
            AlterTablePolicyExtentTagsRetention,
            AlterDatabasePolicyExtentTagsRetention,
            DeleteTablePolicyExtentTagsRetention,
            DeleteDatabasePolicyExtentTagsRetention,
            ShowPrincipalRoles,
            ShowDatabasePrincipalRoles,
            ShowTablePrincipalRoles,
            ShowExternalTablesPrincipalRoles,
            ShowFunctionPrincipalRoles,
            ShowClusterPrincipalRoles,
            ShowClusterPrincipals,
            ShowDatabasePrincipals,
            ShowTablePrincipals,
            ShowFunctionPrincipals,
            AddClusterRole,
            DropClusterRole,
            SetClusterRole,
            AddDatabaseRole,
            DropDatabaseRole,
            SetDatabaseRole,
            AddTableRole,
            DropTableRole,
            SetTableRole,
            AddFunctionRole,
            DropFunctionRole,
            SetFunctionRole,
            ShowClusterBlockedPrincipals,
            AddClusterBlockedPrincipals,
            DropClusterBlockedPrincipals,
            IngestIntoTable,
            IngestInlineIntoTable,
            SetTable,
            AppendTable,
            SetOrAppendTable,
            SetOrReplaceTable,
            ExportToStorage,
            ExportToSqlTable,
            ExportToExternalTable,
            CreateOrAlterContinuousExport,
            ShowContinuousExport,
            ShowContinuousExports,
            ShowClusterPendingContinuousExports,
            ShowContinuousExportExportedArtifacts,
            ShowContinuousExportFailures,
            SetContinuousExportCursor,
            DropContinuousExport,
            EnableContinuousExport,
            DisableContinuousExport,
            CreateMaterializedView,
            RenameMaterializedView,
            ShowMaterializedView,
            ShowMaterializedViews,
            ShowMaterializedViewsDetails,
            ShowMaterializedViewDetails,
            ShowMaterializedViewPolicyRetention,
            ShowMaterializedViewPolicyMerge,
            ShowMaterializedViewPolicyPartitioning,
            ShowMaterializedViewExtents,
            AlterMaterializedView,
            CreateOrAlterMaterializedView,
            DropMaterializedView,
            EnableDisableMaterializedView,
            ShowMaterializedViewPrincipals,
            ShowMaterializedViewSchemaAsJson,
            ShowMaterializedViewCslSchema,
            AlterMaterializedViewFolder,
            AlterMaterializedViewDocString,
            AlterMaterializedViewLookback,
            AlterMaterializedViewAutoUpdateSchema,
            ClearMaterializedViewData,
            SetMaterializedViewCursor,
            ShowCluster,
            ShowDiagnostics,
            ShowCapacity,
            ShowOperations,
            ShowOperationDetails,
            ShowJournal,
            ShowDatabaseJournal,
            ShowClusterJournal,
            ShowQueries,
            ShowRunningQueries,
            CancelQuery,
            ShowQueryPlan,
            ShowBasicAuthUsers,
            CreateBasicAuthUser,
            DropBasicAuthUser,
            ShowCache,
            AlterCache,
            ShowCommands,
            ShowCommandsAndQueries,
            ShowIngestionFailures,
            ShowClusterExtents,
            ShowClusterExtentsMetadata,
            ShowDatabaseExtents,
            ShowDatabaseExtentsMetadata,
            ShowDatabaseExtentTagsStatistics,
            ShowTableExtents,
            ShowTableExtentsMetadata,
            TableShardGroupsShow,
            TableShardGroupsStatisticsShow,
            TablesShardGroupsStatisticsShow,
            DatabaseShardGroupsStatisticsShow,
            MergeExtents,
            MergeExtentsDryrun,
            MoveExtentsFrom,
            MoveExtentsQuery,
            ReplaceExtents,
            DropExtent,
            DropExtents,
            DropExtentsPartitionMetadata,
            DropPretendExtentsByProperties,
            ShowVersion,
            ClearTableData,
            ClearTableCacheStreamingIngestionSchema,
            StoredQueryResultSet,
            StoredQueryResultSetOrReplace,
            StoredQueryResultsShow,
            StoredQueryResultShowSchema,
            StoredQueryResultDrop,
            StoredQueryResultsDrop,
            CreateRequestSupport,
            ShowRequestSupport,
            ShowClusterAdminState,
            ClearRemoteClusterDatabaseSchema,
            ShowClusterMonitoring,
            ShowClusterScaleIn,
            ShowClusterServices,
            ShowClusterNetwork,
            AlterClusterStorageKeys,
            ShowClusterStorageKeysHash,
            CreateEntityGroupCommand,
            AlterEntityGroup,
            AlterMergeEntityGroup,
            DropEntityGroup,
            ShowEntityGroup,
            ShowEntityGroups,
            AlterExtentContainersAdd,
            AlterExtentContainersDrop,
            AlterExtentContainersRecycle,
            AlterExtentContainersSet,
            ShowExtentContainers,
            DropEmptyExtentContainers,
            CleanDatabaseExtentContainers,
            ShowDatabaseExtentContainersCleanOperations,
            ClearDatabaseCacheQueryResults,
            ShowDatabaseCacheQueryResults,
            ShowDatabasesManagementGroups,
            AlterDatabaseStorageKeys,
            ClearDatabaseCacheStreamingIngestionSchema,
            ClearDatabaseCacheQueryWeakConsistency,
            ShowEntitySchema,
            ShowExtentDetails,
            ShowExtentColumnStorageStats,
            AttachExtentsIntoTableByContainer,
            AttachExtentsIntoTableByMetadata,
            AlterExtentTagsFromQuery,
            AlterMergeExtentTagsFromQuery,
            DropExtentTagsRetention,
            MergeDatabaseShardGroups,
            AlterFollowerClusterConfiguration,
            AddFollowerDatabaseAuthorizedPrincipals,
            DropFollowerDatabaseAuthorizedPrincipals,
            AlterFollowerDatabaseAuthorizedPrincipals,
            DropFollowerDatabasePolicyCaching,
            AlterFollowerDatabaseChildEntities,
            AlterFollowerDatabaseConfiguration,
            DropFollowerDatabases,
            ShowFollowerDatabase,
            AlterFollowerTablesPolicyCaching,
            DropFollowerTablesPolicyCaching,
            ShowFreshness,
            ShowFunctionSchemaAsJson,
            SetMaterializedViewAdmins,
            AddMaterializedViewAdmins,
            DropMaterializedViewAdmins,
            SetMaterializedViewConcurrency,
            ClearMaterializedViewStatistics,
            ShowMaterializedViewStatistics,
            ShowMaterializedViewDiagnostics,
            ShowMaterializedViewFailures,
            ShowMemory,
            CancelOperation,
            DisablePlugin,
            EnablePlugin,
            ShowPlugins,
            ShowPrincipalAccess,
            ShowDatabasePurgeOperation,
            ShowQueryExecution,
            AlterPoliciesOfRetention,
            DeletePoliciesOfRetention,
            CreateRowStore,
            DropRowStore,
            ShowRowStore,
            ShowRowStores,
            ShowRowStoreTransactions,
            ShowRowStoreSeals,
            ShowSchema,
            ShowCallStacks,
            ShowStreamingIngestionFailures,
            ShowStreamingIngestionStatistics,
            AlterTableRowStoreReferencesDropKey,
            AlterTableRowStoreReferencesDropRowStore,
            AlterTableRowStoreReferencesDropBlockedKeys,
            AlterTableRowStoreReferencesDisableKey,
            AlterTableRowStoreReferencesDisableRowStore,
            AlterTableRowStoreReferencesDisableBlockedKeys,
            SetTableRowStoreReferences,
            ShowTableRowStoreReferences,
            AlterTableColumnStatistics,
            AlterTableColumnStatisticsMethod,
            ShowTableColumnStatitics,
            ShowTableDimensions,
            DeleteTableRecords,
            ShowTableColumnsClassification,
            ShowTableRowStores,
            ShowTableRowStoreSealInfo,
            ShowTablesColumnStatistics,
            ShowTableUsageStatistics,
            ShowTableUsageStatisticsDetails,
            CreateTempStorage,
            DropTempStorage,
            DropStoredQueryResultContainers,
            DropUnusedStoredQueryResultContainers,
            EnableDatabaseMaintenanceMode,
            DisableDatabaseMaintenanceMode,
            ShowQueryCallTree,
            ShowExtentCorruptedDatetime,
            PatchExtentCorruptedDatetime
        };
    }
}

