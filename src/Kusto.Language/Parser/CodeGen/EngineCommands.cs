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
        private static readonly string _schema4 = "(DatabaseName: string, Version: string, PersistentStorage: string, MetadataPersistentStorage: string, DatabaseAccessMode: string)";
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
        private static readonly string _schema17 = "(TableName: string, QueryAccelerationPolicy: string, QueryAccelerationState: string)";
        private static readonly string _schema18 = "(TableName: string, TableType: string, Folder: string, DocString: string, Schema: string, Properties: string)";
        private static readonly string _schema19 = "()";
        private static readonly string _schema20 = "(WorkloadGroupName: string, WorkloadGroup:string)";
        private static readonly string _schema21 = "(PolicyName: string, EntityName: string, Policy: string, ChildEntities: string, EntityType: string)";
        private static readonly string _schema22 = "(ExternalTableName: string, IsEnabled: bool, Hot: timespan, HotSize: long, LastUpdatedDateTime: datetime, AccelerationPendingDataFilesCount:long, AccelerationPendingDataFilesSize:long, AccelerationCompletePercentage: double, NotHealthyReason: string)";
        private static readonly string _schema23 = "(Name: string, Kind: string, ConnectionString: string, IsEnabled: bool, AutoApplyToNewTables: bool)";
        private static readonly string _schema24 = "(Scope: string, DisplayName: string, AADObjectID: string, Role: string)";
        private static readonly string _schema25 = "(Role: string, PrincipalType: string, PrincipalDisplayName: string, PrincipalObjectId: string, PrincipalFQN: string, Notes: string, RoleAssignmentIdentifier: string)";
        private static readonly string _schema26 = "(PrincipalType: string, PrincipalDisplayName: string, PrincipalObjectId: string, PrincipalFQN: string, Application: string, User: string, BlockedUntil: datetime, Reason: string)";
        private static readonly string _schema27 = "(ExtentId: guid, OriginalSize: long, ExtentSize: long, ColumnSize: long, IndexSize: long, RowCount: long)";
        private static readonly string _schema28 = "(Name: string, ExternalTableName: string, Query: string, ForcedLatency: timespan, IntervalBetweenRuns: timespan, CursorScopedTables: string, ExportProperties: string, LastRunTime: datetime, StartCursor: string, IsDisabled: bool, LastRunResult: string, ExportedTo: datetime, IsRunning: bool)";
        private static readonly string _schema29 = "(Name: string, SourceTable: string, Query: string, MaterializedTo: datetime, LastRun: datetime, LastRunResult: string, IsHealthy: bool, IsEnabled: bool, Folder: string, DocString: string, AutoUpdateSchema: bool, EffectiveDateTime: datetime, Lookback:timespan, LookbackColumn:string)";
        private static readonly string _schema30 = "(MaterializedViewName: string, DatabaseName: string, Folder: string, DocString: string, TotalExtents: long, TotalExtentSize: real, TotalOriginalSize: real, TotalRowCount: long, HotExtents: long, HotExtentSize: real, HotOriginalSize: real, HotRowCount: long, AuthorizedPrincipals: string, RetentionPolicy: string, CachingPolicy: string, ShardingPolicy: string, MergePolicy: string, MinExtentsCreationTime: datetime, MaxExtentsCreationTime: datetime)";
        private static readonly string _schema31 = "(Timestamp: datetime, ExternalTableName: string, Path: string, NumRecords: long)";
        private static readonly string _schema32 = "(Timestamp: datetime, OperationId: string, Name: string, LastSuccessRun: datetime, FailureKind: string, Details: string)";
        private static readonly string _schema33 = "(Event: string, EventTimestamp: datetime, Database: string, EntityName: string, UpdatedEntityName: string, EntityVersion: string, EntityContainerName: string, OriginalEntityState: string, UpdatedEntityState: string, ChangeCommand: string, Principal: string, RootActivityId: guid, ClientRequestId: string, User: string, OriginalEntityVersion: string)";
        private static readonly string _schema34 = "(ExtentId: guid, DatabaseName: string, TableName: string, MaxCreatedOn: datetime, OriginalSize: real, ExtentSize: real, CompressedSize: real, IndexSize: real, Blocks: long, Segments: long, ExtentContainerId: string, RowCount: long, MinCreatedOn: datetime, Tags: string, Kind: string, DeletedRowCount: long)";
        private static readonly string _schema35 = "(ExtentId: guid, DatabaseName: string, TableName: string, ExtentMetadata: string)";
        private static readonly string _schema36 = "(TableId: guid, long ShardGroupCount: long, ShardCount: long, RowCount: long, OriginalSize: long, ShardSize: long, CompressedSize: long, IndexSize: long, DeletedRowCount: long, PartitionedRowCount: long)";
        private static readonly string _schema37 = "(OriginalExtentId: string, ResultExtentId: string, Duration: timespan)";
        private static readonly string _schema38 = "(OriginalExtentId: string, ResultExtentId: string, Details: string)";
        private static readonly string _schema39 = "(ExtentId: guid, TableName: string, CreatedOn: datetime)";
        private static readonly string _schema40 = "(StoredQueryResultId:guid, Name:string, DatabaseName:string, PrincipalIdentity:string, SizeInBytes:long, RowCount:long, CreatedOn:datetime, ExpiresOn:datetime)";
        private static readonly string _schema41 = "(Name:string, SnapshotTime:datetime, ModelName:string, ModelId:guid, ModelCreationTime:datetime)";
        private static readonly string _schema42 = "(DatabaseName:string, Name:string, SnapshotTime:datetime, ModelName:string, ModelId:guid, TotalCpu:timespan, MemoryPeak:long, Duration:timespan, Details:string, NodesCount:long, EdgesCount:long, NodesSize:long, EdgesSize:long)";
        private static readonly string _schema43 = "(Name: string, Entities: string)";
        private static readonly string _schema44 = "(ExtentContainerId:guid, Url:string, State:string, CreatedOn:datetime, MaxDateTime:datetime, IsRecyclable:bool, StoresDatabaseMetadataPointer:bool, HardDeletePeriod:timespan, ActiveMetadataContainer:bool, MetadataContainer:bool)";
        private static readonly string _schema45 = "(DatabaseName:string, EntityType:string, EntityName:string, DocString:string, Folder:string, CslInputSchema:string, Content:string, CslOutputSchema:string, Properties:dynamic)";

        public static readonly CommandSymbol ShowDatabase =
            new CommandSymbol(
                "ShowDatabase",
                _schema0,
                "DatabaseShowCommand(null, (PropertyName, PropertyValue)*)");

        public static readonly CommandSymbol ShowDatabaseDetails =
            new CommandSymbol(
                "ShowDatabaseDetails",
                _schema1,
                "DatabaseShowCommand(flavor, (PropertyName, PropertyValue)*)");

        public static readonly CommandSymbol ShowDatabaseIdentity =
            new CommandSymbol(
                "ShowDatabaseIdentity",
                _schema2,
                "DatabaseShowCommand('identity', (PropertyName, PropertyValue)*)");

        public static readonly CommandSymbol ShowDatabasePolicies =
            new CommandSymbol(
                "ShowDatabasePolicies",
                _schema3,
                "DatabaseShowCommand('policies', (PropertyName, PropertyValue)*)");

        public static readonly CommandSymbol ShowDatabaseDataStats =
            new CommandSymbol(
                "ShowDatabaseDataStats",
                "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, CurrentUseIsUnrestrictedViewer: bool, DatabaseId: guid, OriginalSize: real, ExtentSize: real, CompressedSize: real, IndexSize: real, RowCount: long, HotOriginalSize: real, HotExtentSize: real, HotCompressedSize: real, HotIndexSize: real, HotRowCount: long)",
                "DatabaseShowCommand('datastats', (PropertyName, PropertyValue)*)");

        public static readonly CommandSymbol ShowDatabaseMetadata =
            new CommandSymbol(
                "ShowDatabaseMetadata",
                _schema4,
                "DatabaseShowCommand('metadata', (PropertyName, PropertyValue)*)");

        public static readonly CommandSymbol ShowClusterDatabases =
            new CommandSymbol(
                "ShowClusterDatabases",
                _schema0,
                "DatabasesShowCommand(null, DatabaseName*, (PropertyName, PropertyValue)*)");

        public static readonly CommandSymbol ShowClusterDatabasesDetails =
            new CommandSymbol(
                "ShowClusterDatabasesDetails",
                _schema1,
                "DatabasesShowCommand('details', DatabaseName*, (PropertyName, PropertyValue)*)");

        public static readonly CommandSymbol ShowClusterDatabasesIdentity =
            new CommandSymbol(
                "ShowClusterDatabasesIdentity",
                _schema2,
                "DatabasesShowCommand('identity', DatabaseName*, (PropertyName, PropertyValue)*)");

        public static readonly CommandSymbol ShowClusterDatabasesPolicies =
            new CommandSymbol(
                "ShowClusterDatabasesPolicies",
                _schema3,
                "DatabasesShowCommand('policies', DatabaseName*, (PropertyName, PropertyValue)*)");

        public static readonly CommandSymbol ShowClusterDatabasesDataStats =
            new CommandSymbol(
                "ShowClusterDatabasesDataStats",
                "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, DatabaseId: guid, OriginalSize: real, ExtentSize: real, CompressedSize: real, IndexSize: real, RowCount: long, HotOriginalSize: real, HotExtentSize: real, HotCompressedSize: real, HotIndexSize: real, HotRowCount: long)",
                "DatabasesShowCommand('datastats', DatabaseName*, (PropertyName, PropertyValue)*)");

        public static readonly CommandSymbol ShowClusterDatabasesMetadata =
            new CommandSymbol(
                "ShowClusterDatabasesMetadata",
                _schema4,
                "DatabasesShowCommand('metadata', DatabaseName*, (PropertyName, PropertyValue)*)");

        public static readonly CommandSymbol CreateDatabase =
            new CommandSymbol(
                "CreateDatabase",
                "(DatabaseName: string, PersistentPath: string, Created: string, StoresMetadata: bool, StoresData: bool)",
                "DatabaseCreateCommand(DatabaseName, Path*, IfNotExists?, (PropertyName, PropertyValue)*)");

        public static readonly CommandSymbol AttachDatabase =
            new CommandSymbol(
                "AttachDatabase",
                "(Step: string, Duration: string)",
                "DatabaseAttachCommand(DatabaseName, Path, ReadOnly ?? 'ReadWrite', Version, (PropertyName, PropertyValue)*)");

        public static readonly CommandSymbol DetachDatabase =
            new CommandSymbol(
                "DetachDatabase",
                "(Table: string, NumberOfRemovedExtents: string)",
                "DatabaseDetachCommand(DatabaseName, IfExists?, SkipSeal?)");

        public static readonly CommandSymbol AlterDatabasePrettyName =
            new CommandSymbol(
                "AlterDatabasePrettyName",
                _schema5,
                "DatabaseSetPrettyNameCommand(DatabaseName, PrettyName)");

        public static readonly CommandSymbol DropDatabasePrettyName =
            new CommandSymbol(
                "DropDatabasePrettyName",
                _schema5,
                "DatabaseSetPrettyNameCommand(DatabaseName, null)");

        public static readonly CommandSymbol AlterDatabasePersistMetadata =
            new CommandSymbol(
                "AlterDatabasePersistMetadata",
                "(Moniker: guid, Url: string, State: string, CreatedOn: datetime, MaxDateTime: datetime, IsRecyclable: bool, StoresDatabaseMetadata: bool, HardDeletePeriod: timespan)",
                "DatabaseMetadataContainerAlterCommand(DatabaseName, Path, AllowNonEmptyContainer?)");

        public static readonly CommandSymbol SetAccess =
            new CommandSymbol(
                "SetAccess",
                "(DatabaseName: string, RequestedAccessMode: string, Status: string)",
                "DatabaseSetAccessModeCommand(DatabaseName, AccessMode)");

        public static readonly CommandSymbol ShowDatabaseSchema =
            new CommandSymbol(
                "ShowDatabaseSchema",
                _schema6,
                "SchemaShowCommand(Details?, (DatabaseName, Version))");

        public static readonly CommandSymbol ShowDatabaseSchemaAsJson =
            new CommandSymbol(
                "ShowDatabaseSchemaAsJson",
                _schema7,
                "SchemaShowAsJsonCommand((DatabaseName, Version), (PropertyName, PropertyValue)*)");

        public static readonly CommandSymbol ShowDatabaseSchemaAsCslScript =
            new CommandSymbol(
                "ShowDatabaseSchemaAsCslScript",
                "(DatabaseSchemaScript: string)",
                "DatabaseSchemaShowAsCslCommand(Script?, (DatabaseName, Version), (PropertyName, PropertyValue)*)");

        public static readonly CommandSymbol ShowDatabaseCslSchema =
            new CommandSymbol(
                "ShowDatabaseCslSchema",
                _schema10,
                "DatabaseSchemaShowAsCslCommand(Script?, (DatabaseName, Version))");

        public static readonly CommandSymbol ShowDatabaseSchemaViolations =
            new CommandSymbol(
                "ShowDatabaseSchemaViolations",
                "(EntityKind: string, EntityName: string, Property: string, Reason: string)",
                "DatabaseSchemaViolationsShowCommand(DatabaseName)");

        public static readonly CommandSymbol ShowDatabasesSchema =
            new CommandSymbol(
                "ShowDatabasesSchema",
                _schema6,
                "SchemaShowCommand(Details?, (DatabaseName, Version)*)");

        public static readonly CommandSymbol ShowDatabasesSchemaAsJson =
            new CommandSymbol(
                "ShowDatabasesSchemaAsJson",
                _schema7,
                "SchemaShowAsJsonCommand((DatabaseName, Version)*, (PropertyName, PropertyValue)*)");

        public static readonly CommandSymbol CreateDatabaseIngestionMapping =
            new CommandSymbol("CreateDatabaseIngestionMapping", _schema8);

        public static readonly CommandSymbol CreateOrAlterDatabaseIngestionMapping =
            new CommandSymbol("CreateOrAlterDatabaseIngestionMapping", _schema8);

        public static readonly CommandSymbol AlterDatabaseIngestionMapping =
            new CommandSymbol("AlterDatabaseIngestionMapping", _schema8);

        public static readonly CommandSymbol AlterMergeDatabaseIngestionMapping =
            new CommandSymbol("AlterMergeDatabaseIngestionMapping", _schema8);

        public static readonly CommandSymbol ShowDatabaseIngestionMappings =
            new CommandSymbol("ShowDatabaseIngestionMappings", _schema8);

        public static readonly CommandSymbol ShowIngestionMappings =
            new CommandSymbol("ShowIngestionMappings", _schema19);

        public static readonly CommandSymbol DropDatabaseIngestionMapping =
            new CommandSymbol("DropDatabaseIngestionMapping", _schema8);

        public static readonly CommandSymbol ShowTables =
            new CommandSymbol("ShowTables", _schema11);

        public static readonly CommandSymbol ShowTable =
            new CommandSymbol("ShowTable", "(AttributeName: string, AttributeType: string, ExtentSize: long, CompressionRatio: real, IndexSize: long, IndexSizePercent: real, OriginalSize: long, AttributeId: guid, SharedIndexSize: long, StorageEngineVersion: string)");

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

        public static readonly CommandSymbol UndoDropExtentContainer =
            new CommandSymbol("UndoDropExtentContainer", "(ContainerId: guid, RestoreContainerFinalState: string)");

        public static readonly CommandSymbol DropTable =
            new CommandSymbol("DropTable", _schema12);

        public static readonly CommandSymbol UndoDropTable =
            new CommandSymbol("UndoDropTable", "(ExtentId: guid, NumberOfRecords: long, Status: string, FailureReason: string)");

        public static readonly CommandSymbol DropTables =
            new CommandSymbol("DropTables", _schema12);

        public static readonly CommandSymbol CreateTableIngestionMapping =
            new CommandSymbol("CreateTableIngestionMapping", _schema13);

        public static readonly CommandSymbol CreateOrAlterTableIngestionMapping =
            new CommandSymbol("CreateOrAlterTableIngestionMapping", _schema13);

        public static readonly CommandSymbol AlterTableIngestionMapping =
            new CommandSymbol("AlterTableIngestionMapping", _schema13);

        public static readonly CommandSymbol AlterMergeTableIngestionMapping =
            new CommandSymbol("AlterMergeTableIngestionMapping", _schema13);

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
            new CommandSymbol("DropFunction", "(Name: string)");

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

        public static readonly CommandSymbol ShowExternalTablesDetails =
            new CommandSymbol("ShowExternalTablesDetails", _schema17);

        public static readonly CommandSymbol ShowExternalTableDetails =
            new CommandSymbol("ShowExternalTableDetails", _schema17);

        public static readonly CommandSymbol ShowExternalTableCslSchema =
            new CommandSymbol("ShowExternalTableCslSchema", _schema10);

        public static readonly CommandSymbol ShowExternalTableSchema =
            new CommandSymbol("ShowExternalTableSchema", _schema10);

        public static readonly CommandSymbol ShowExternalTableArtifacts =
            new CommandSymbol("ShowExternalTableArtifacts", "(Uri: string, Partition: dynamic, Size: long)");

        public static readonly CommandSymbol DropExternalTable =
            new CommandSymbol("DropExternalTable", _schema16);

        public static readonly CommandSymbol CreateStorageExternalTable =
            new CommandSymbol("CreateStorageExternalTable", _schema16);

        public static readonly CommandSymbol AlterStorageExternalTable =
            new CommandSymbol("AlterStorageExternalTable", _schema18);

        public static readonly CommandSymbol CreateOrAlterStorageExternalTable =
            new CommandSymbol("CreateOrAlterStorageExternalTable", _schema18);

        public static readonly CommandSymbol CreateSqlExternalTable =
            new CommandSymbol("CreateSqlExternalTable", _schema16);

        public static readonly CommandSymbol AlterSqlExternalTable =
            new CommandSymbol("AlterSqlExternalTable", _schema18);

        public static readonly CommandSymbol CreateOrAlterSqlExternalTable =
            new CommandSymbol("CreateOrAlterSqlExternalTable", _schema18);

        public static readonly CommandSymbol CreateExternalTableMapping =
            new CommandSymbol("CreateExternalTableMapping", _schema13);

        public static readonly CommandSymbol SetExternalTableAdmins =
            new CommandSymbol("SetExternalTableAdmins", _schema19);

        public static readonly CommandSymbol AddExternalTableAdmins =
            new CommandSymbol("AddExternalTableAdmins", _schema19);

        public static readonly CommandSymbol DropExternalTableAdmins =
            new CommandSymbol("DropExternalTableAdmins", _schema19);

        public static readonly CommandSymbol AlterExternalTableDocString =
            new CommandSymbol("AlterExternalTableDocString", _schema19);

        public static readonly CommandSymbol AlterExternalTableFolder =
            new CommandSymbol("AlterExternalTableFolder", _schema19);

        public static readonly CommandSymbol ShowExternalTablePrincipals =
            new CommandSymbol("ShowExternalTablePrincipals", _schema19);

        public static readonly CommandSymbol ShowFabric =
            new CommandSymbol("ShowFabric", _schema19);

        public static readonly CommandSymbol AlterExternalTableMapping =
            new CommandSymbol("AlterExternalTableMapping", _schema13);

        public static readonly CommandSymbol ShowExternalTableMappings =
            new CommandSymbol("ShowExternalTableMappings", _schema13);

        public static readonly CommandSymbol ShowExternalTableMapping =
            new CommandSymbol("ShowExternalTableMapping", _schema13);

        public static readonly CommandSymbol DropExternalTableMapping =
            new CommandSymbol("DropExternalTableMapping", _schema13);

        public static readonly CommandSymbol ShowWorkloadGroups =
            new CommandSymbol("ShowWorkloadGroups", _schema20);

        public static readonly CommandSymbol ShowWorkloadGroup =
            new CommandSymbol("ShowWorkloadGroup", _schema20);

        public static readonly CommandSymbol CreateOrAleterWorkloadGroup =
            new CommandSymbol("CreateOrAleterWorkloadGroup", _schema20);

        public static readonly CommandSymbol AlterMergeWorkloadGroup =
            new CommandSymbol("AlterMergeWorkloadGroup", _schema20);

        public static readonly CommandSymbol DropWorkloadGroup =
            new CommandSymbol("DropWorkloadGroup", _schema20);

        public static readonly CommandSymbol ShowDatabasePolicyCaching =
            new CommandSymbol("ShowDatabasePolicyCaching", _schema21);

        public static readonly CommandSymbol ShowTablePolicyCaching =
            new CommandSymbol("ShowTablePolicyCaching", _schema21);

        public static readonly CommandSymbol ShowTableStarPolicyCaching =
            new CommandSymbol("ShowTableStarPolicyCaching", _schema21);

        public static readonly CommandSymbol ShowColumnPolicyCaching =
            new CommandSymbol("ShowColumnPolicyCaching", _schema21);

        public static readonly CommandSymbol ShowMaterializedViewPolicyCaching =
            new CommandSymbol("ShowMaterializedViewPolicyCaching", _schema21);

        public static readonly CommandSymbol ShowGraphModelPolicyCaching =
            new CommandSymbol("ShowGraphModelPolicyCaching", _schema21);

        public static readonly CommandSymbol ShowGraphModelStarPolicyCaching =
            new CommandSymbol("ShowGraphModelStarPolicyCaching", _schema21);

        public static readonly CommandSymbol ShowClusterPolicyCaching =
            new CommandSymbol("ShowClusterPolicyCaching", _schema21);

        public static readonly CommandSymbol AlterDatabasePolicyCaching =
            new CommandSymbol("AlterDatabasePolicyCaching", _schema21);

        public static readonly CommandSymbol AlterTablePolicyCaching =
            new CommandSymbol("AlterTablePolicyCaching", _schema21);

        public static readonly CommandSymbol AlterTablesPolicyCaching =
            new CommandSymbol("AlterTablesPolicyCaching", _schema19);

        public static readonly CommandSymbol AlterColumnPolicyCaching =
            new CommandSymbol("AlterColumnPolicyCaching", _schema21);

        public static readonly CommandSymbol AlterMaterializedViewPolicyCaching =
            new CommandSymbol("AlterMaterializedViewPolicyCaching", _schema21);

        public static readonly CommandSymbol AlterGraphModelPolicyCaching =
            new CommandSymbol("AlterGraphModelPolicyCaching", _schema21);

        public static readonly CommandSymbol AlterClusterPolicyCaching =
            new CommandSymbol("AlterClusterPolicyCaching", _schema21);

        public static readonly CommandSymbol DeleteDatabasePolicyCaching =
            new CommandSymbol("DeleteDatabasePolicyCaching", _schema21);

        public static readonly CommandSymbol DeleteTablePolicyCaching =
            new CommandSymbol("DeleteTablePolicyCaching", _schema21);

        public static readonly CommandSymbol DeleteColumnPolicyCaching =
            new CommandSymbol("DeleteColumnPolicyCaching", _schema21);

        public static readonly CommandSymbol DeleteMaterializedViewPolicyCaching =
            new CommandSymbol("DeleteMaterializedViewPolicyCaching", _schema21);

        public static readonly CommandSymbol DeleteGraphModelPolicyCaching =
            new CommandSymbol("DeleteGraphModelPolicyCaching", _schema21);

        public static readonly CommandSymbol DeleteClusterPolicyCaching =
            new CommandSymbol("DeleteClusterPolicyCaching", _schema21);

        public static readonly CommandSymbol ShowTablePolicyIngestionTime =
            new CommandSymbol("ShowTablePolicyIngestionTime", _schema21);

        public static readonly CommandSymbol ShowTableStarPolicyIngestionTime =
            new CommandSymbol("ShowTableStarPolicyIngestionTime", _schema21);

        public static readonly CommandSymbol AlterTablePolicyIngestionTime =
            new CommandSymbol("AlterTablePolicyIngestionTime", _schema21);

        public static readonly CommandSymbol AlterTablesPolicyIngestionTime =
            new CommandSymbol("AlterTablesPolicyIngestionTime", _schema21);

        public static readonly CommandSymbol DeleteTablePolicyIngestionTime =
            new CommandSymbol("DeleteTablePolicyIngestionTime", _schema21);

        public static readonly CommandSymbol ShowTablePolicyRetention =
            new CommandSymbol("ShowTablePolicyRetention", _schema21);

        public static readonly CommandSymbol ShowTableStarPolicyRetention =
            new CommandSymbol("ShowTableStarPolicyRetention", _schema21);

        public static readonly CommandSymbol ShowGraphPolicyRetention =
            new CommandSymbol("ShowGraphPolicyRetention", _schema21);

        public static readonly CommandSymbol ShowGraphStarPolicyRetention =
            new CommandSymbol("ShowGraphStarPolicyRetention", _schema21);

        public static readonly CommandSymbol ShowDatabasePolicyRetention =
            new CommandSymbol("ShowDatabasePolicyRetention", _schema21);

        public static readonly CommandSymbol AlterTablePolicyRetention =
            new CommandSymbol("AlterTablePolicyRetention", _schema21);

        public static readonly CommandSymbol AlterMaterializedViewPolicyRetention =
            new CommandSymbol("AlterMaterializedViewPolicyRetention", _schema21);

        public static readonly CommandSymbol AlterDatabasePolicyRetention =
            new CommandSymbol("AlterDatabasePolicyRetention", _schema21);

        public static readonly CommandSymbol AlterGraphModelPolicyRetention =
            new CommandSymbol("AlterGraphModelPolicyRetention", _schema21);

        public static readonly CommandSymbol AlterTablesPolicyRetention =
            new CommandSymbol("AlterTablesPolicyRetention", _schema21);

        public static readonly CommandSymbol AlterMergeTablePolicyRetention =
            new CommandSymbol("AlterMergeTablePolicyRetention", _schema21);

        public static readonly CommandSymbol AlterMergeMaterializedViewPolicyRetention =
            new CommandSymbol("AlterMergeMaterializedViewPolicyRetention", _schema21);

        public static readonly CommandSymbol AlterMergeDatabasePolicyRetention =
            new CommandSymbol("AlterMergeDatabasePolicyRetention", _schema21);

        public static readonly CommandSymbol DeleteTablePolicyRetention =
            new CommandSymbol("DeleteTablePolicyRetention", _schema21);

        public static readonly CommandSymbol DeleteDatabasePolicyRetention =
            new CommandSymbol("DeleteDatabasePolicyRetention", _schema21);

        public static readonly CommandSymbol ShowDatabasePolicyHardRetentionViolations =
            new CommandSymbol("ShowDatabasePolicyHardRetentionViolations", _schema19);

        public static readonly CommandSymbol ShowDatabasePolicySoftRetentionViolations =
            new CommandSymbol("ShowDatabasePolicySoftRetentionViolations", _schema19);

        public static readonly CommandSymbol ShowTablePolicyRowLevelSecurity =
            new CommandSymbol("ShowTablePolicyRowLevelSecurity", _schema21);

        public static readonly CommandSymbol ShowTableStarPolicyRowLevelSecurity =
            new CommandSymbol("ShowTableStarPolicyRowLevelSecurity", _schema21);

        public static readonly CommandSymbol AlterTablePolicyRowLevelSecurity =
            new CommandSymbol("AlterTablePolicyRowLevelSecurity", _schema21);

        public static readonly CommandSymbol DeleteTablePolicyRowLevelSecurity =
            new CommandSymbol("DeleteTablePolicyRowLevelSecurity", _schema21);

        public static readonly CommandSymbol ShowMaterializedViewPolicyRowLevelSecurity =
            new CommandSymbol("ShowMaterializedViewPolicyRowLevelSecurity", _schema21);

        public static readonly CommandSymbol AlterMaterializedViewPolicyRowLevelSecurity =
            new CommandSymbol("AlterMaterializedViewPolicyRowLevelSecurity", _schema21);

        public static readonly CommandSymbol DeleteMaterializedViewPolicyRowLevelSecurity =
            new CommandSymbol("DeleteMaterializedViewPolicyRowLevelSecurity", _schema21);

        public static readonly CommandSymbol ShowTablePolicyRowOrder =
            new CommandSymbol("ShowTablePolicyRowOrder", _schema21);

        public static readonly CommandSymbol ShowTableStarPolicyRowOrder =
            new CommandSymbol("ShowTableStarPolicyRowOrder", _schema21);

        public static readonly CommandSymbol AlterTablePolicyRowOrder =
            new CommandSymbol("AlterTablePolicyRowOrder", _schema21);

        public static readonly CommandSymbol AlterTablesPolicyRowOrder =
            new CommandSymbol("AlterTablesPolicyRowOrder", _schema21);

        public static readonly CommandSymbol AlterMergeTablePolicyRowOrder =
            new CommandSymbol("AlterMergeTablePolicyRowOrder", _schema21);

        public static readonly CommandSymbol DeleteTablePolicyRowOrder =
            new CommandSymbol("DeleteTablePolicyRowOrder", _schema21);

        public static readonly CommandSymbol ShowTablePolicyUpdate =
            new CommandSymbol("ShowTablePolicyUpdate", _schema21);

        public static readonly CommandSymbol ShowTableStarPolicyUpdate =
            new CommandSymbol("ShowTableStarPolicyUpdate", _schema21);

        public static readonly CommandSymbol AlterTablePolicyUpdate =
            new CommandSymbol("AlterTablePolicyUpdate", _schema21);

        public static readonly CommandSymbol AlterMergeTablePolicyUpdate =
            new CommandSymbol("AlterMergeTablePolicyUpdate", _schema21);

        public static readonly CommandSymbol DeleteTablePolicyUpdate =
            new CommandSymbol("DeleteTablePolicyUpdate", _schema21);

        public static readonly CommandSymbol ShowClusterPolicyIngestionBatching =
            new CommandSymbol("ShowClusterPolicyIngestionBatching", _schema21);

        public static readonly CommandSymbol ShowDatabasePolicyIngestionBatching =
            new CommandSymbol("ShowDatabasePolicyIngestionBatching", _schema21);

        public static readonly CommandSymbol ShowTablePolicyIngestionBatching =
            new CommandSymbol("ShowTablePolicyIngestionBatching", _schema21);

        public static readonly CommandSymbol ShowTableStarPolicyIngestionBatching =
            new CommandSymbol("ShowTableStarPolicyIngestionBatching", _schema21);

        public static readonly CommandSymbol AlterClusterPolicyIngestionBatching =
            new CommandSymbol("AlterClusterPolicyIngestionBatching", _schema21);

        public static readonly CommandSymbol AlterMergeClusterPolicyIngestionBatching =
            new CommandSymbol("AlterMergeClusterPolicyIngestionBatching", _schema21);

        public static readonly CommandSymbol AlterDatabasePolicyIngestionBatching =
            new CommandSymbol("AlterDatabasePolicyIngestionBatching", _schema21);

        public static readonly CommandSymbol AlterMergeDatabasePolicyIngestionBatching =
            new CommandSymbol("AlterMergeDatabasePolicyIngestionBatching", _schema21);

        public static readonly CommandSymbol AlterTablePolicyIngestionBatching =
            new CommandSymbol("AlterTablePolicyIngestionBatching", _schema21);

        public static readonly CommandSymbol AlterMergeTablePolicyIngestionBatching =
            new CommandSymbol("AlterMergeTablePolicyIngestionBatching", _schema21);

        public static readonly CommandSymbol AlterTablesPolicyIngestionBatching =
            new CommandSymbol("AlterTablesPolicyIngestionBatching", _schema21);

        public static readonly CommandSymbol DeleteDatabasePolicyIngestionBatching =
            new CommandSymbol("DeleteDatabasePolicyIngestionBatching", _schema21);

        public static readonly CommandSymbol DeleteTablePolicyIngestionBatching =
            new CommandSymbol("DeleteTablePolicyIngestionBatching", _schema21);

        public static readonly CommandSymbol ShowDatabasePolicyEncoding =
            new CommandSymbol("ShowDatabasePolicyEncoding", _schema21);

        public static readonly CommandSymbol ShowTablePolicyEncoding =
            new CommandSymbol("ShowTablePolicyEncoding", _schema21);

        public static readonly CommandSymbol ShowColumnPolicyEncoding =
            new CommandSymbol("ShowColumnPolicyEncoding", _schema21);

        public static readonly CommandSymbol AlterDatabasePolicyEncoding =
            new CommandSymbol("AlterDatabasePolicyEncoding", _schema21);

        public static readonly CommandSymbol AlterTablePolicyEncoding =
            new CommandSymbol("AlterTablePolicyEncoding", _schema21);

        public static readonly CommandSymbol AlterTableColumnsPolicyEncoding =
            new CommandSymbol("AlterTableColumnsPolicyEncoding", _schema21);

        public static readonly CommandSymbol AlterColumnPolicyEncoding =
            new CommandSymbol("AlterColumnPolicyEncoding", _schema21);

        public static readonly CommandSymbol AlterColumnsPolicyEncodingByQuery =
            new CommandSymbol("AlterColumnsPolicyEncodingByQuery", _schema21);

        public static readonly CommandSymbol AlterColumnPolicyEncodingType =
            new CommandSymbol("AlterColumnPolicyEncodingType", _schema21);

        public static readonly CommandSymbol AlterMergeDatabasePolicyEncoding =
            new CommandSymbol("AlterMergeDatabasePolicyEncoding", _schema21);

        public static readonly CommandSymbol AlterMergeTablePolicyEncoding =
            new CommandSymbol("AlterMergeTablePolicyEncoding", _schema21);

        public static readonly CommandSymbol AlterMergeColumnPolicyEncoding =
            new CommandSymbol("AlterMergeColumnPolicyEncoding", _schema21);

        public static readonly CommandSymbol DeleteDatabasePolicyEncoding =
            new CommandSymbol("DeleteDatabasePolicyEncoding", _schema21);

        public static readonly CommandSymbol DeleteTablePolicyEncoding =
            new CommandSymbol("DeleteTablePolicyEncoding", _schema21);

        public static readonly CommandSymbol DeleteColumnPolicyEncoding =
            new CommandSymbol("DeleteColumnPolicyEncoding", _schema21);

        public static readonly CommandSymbol ShowDatabasePolicyMerge =
            new CommandSymbol("ShowDatabasePolicyMerge", _schema21);

        public static readonly CommandSymbol ShowTablePolicyMerge =
            new CommandSymbol("ShowTablePolicyMerge", _schema21);

        public static readonly CommandSymbol ShowTableStarPolicyMerge =
            new CommandSymbol("ShowTableStarPolicyMerge", _schema21);

        public static readonly CommandSymbol AlterDatabasePolicyMerge =
            new CommandSymbol("AlterDatabasePolicyMerge", _schema21);

        public static readonly CommandSymbol AlterTablePolicyMerge =
            new CommandSymbol("AlterTablePolicyMerge", _schema21);

        public static readonly CommandSymbol AlterTablesPolicyMerge =
            new CommandSymbol("AlterTablesPolicyMerge", _schema21);

        public static readonly CommandSymbol AlterMergeDatabasePolicyMerge =
            new CommandSymbol("AlterMergeDatabasePolicyMerge", _schema21);

        public static readonly CommandSymbol AlterMergeTablePolicyMerge =
            new CommandSymbol("AlterMergeTablePolicyMerge", _schema21);

        public static readonly CommandSymbol AlterMergeMaterializedViewPolicyMerge =
            new CommandSymbol("AlterMergeMaterializedViewPolicyMerge", _schema21);

        public static readonly CommandSymbol DeleteDatabasePolicyMerge =
            new CommandSymbol("DeleteDatabasePolicyMerge", _schema21);

        public static readonly CommandSymbol DeleteTablePolicyMerge =
            new CommandSymbol("DeleteTablePolicyMerge", _schema21);

        public static readonly CommandSymbol ShowExternalTablePolicyQueryAcceleration =
            new CommandSymbol("ShowExternalTablePolicyQueryAcceleration", _schema21);

        public static readonly CommandSymbol ShowExternalTablesPolicyQueryAcceleration =
            new CommandSymbol("ShowExternalTablesPolicyQueryAcceleration", _schema21);

        public static readonly CommandSymbol AlterExternalTablePolicyQueryAcceleration =
            new CommandSymbol("AlterExternalTablePolicyQueryAcceleration", _schema21);

        public static readonly CommandSymbol AlterMergeExternalTablePolicyQueryAcceleration =
            new CommandSymbol("AlterMergeExternalTablePolicyQueryAcceleration", _schema21);

        public static readonly CommandSymbol DeleteExternalTablePolicyQueryAcceleration =
            new CommandSymbol("DeleteExternalTablePolicyQueryAcceleration", _schema21);

        public static readonly CommandSymbol ShowExternalTableQueryAccelerationStatatistics =
            new CommandSymbol("ShowExternalTableQueryAccelerationStatatistics", _schema22);

        public static readonly CommandSymbol ShowExternalTablesQueryAccelerationStatatistics =
            new CommandSymbol("ShowExternalTablesQueryAccelerationStatatistics", _schema22);

        public static readonly CommandSymbol AlterTablePolicyMirroring =
            new CommandSymbol("AlterTablePolicyMirroring", _schema21);

        public static readonly CommandSymbol AlterMergeTablePolicyMirroring =
            new CommandSymbol("AlterMergeTablePolicyMirroring", _schema21);

        public static readonly CommandSymbol AlterTablePolicyMirroringWithJson =
            new CommandSymbol("AlterTablePolicyMirroringWithJson", _schema21);

        public static readonly CommandSymbol AlterMergeTablePolicyMirroringWithJson =
            new CommandSymbol("AlterMergeTablePolicyMirroringWithJson", _schema21);

        public static readonly CommandSymbol DeleteTablePolicyMirroring =
            new CommandSymbol("DeleteTablePolicyMirroring", _schema21);

        public static readonly CommandSymbol ShowTablePolicyMirroring =
            new CommandSymbol("ShowTablePolicyMirroring", _schema21);

        public static readonly CommandSymbol ShowTableStarPolicyMirroring =
            new CommandSymbol("ShowTableStarPolicyMirroring", _schema21);

        public static readonly CommandSymbol CreateMirroringTemplate =
            new CommandSymbol("CreateMirroringTemplate", _schema23);

        public static readonly CommandSymbol CreateOrAlterMirroringTemplate =
            new CommandSymbol("CreateOrAlterMirroringTemplate", _schema23);

        public static readonly CommandSymbol AlterMirroringTemplate =
            new CommandSymbol("AlterMirroringTemplate", _schema23);

        public static readonly CommandSymbol AlterMergeMirroringTemplate =
            new CommandSymbol("AlterMergeMirroringTemplate", _schema23);

        public static readonly CommandSymbol DeleteMirroringTemplate =
            new CommandSymbol("DeleteMirroringTemplate", _schema23);

        public static readonly CommandSymbol ShowMirroringTemplate =
            new CommandSymbol("ShowMirroringTemplate", _schema23);

        public static readonly CommandSymbol ShowMirroringTemplates =
            new CommandSymbol("ShowMirroringTemplates", _schema23);

        public static readonly CommandSymbol ApplyMirroringTemplate =
            new CommandSymbol("ApplyMirroringTemplate", _schema23);

        public static readonly CommandSymbol ShowTablePolicyPartitioning =
            new CommandSymbol("ShowTablePolicyPartitioning", _schema21);

        public static readonly CommandSymbol ShowTableStarPolicyPartitioning =
            new CommandSymbol("ShowTableStarPolicyPartitioning", _schema21);

        public static readonly CommandSymbol AlterTablePolicyPartitioning =
            new CommandSymbol("AlterTablePolicyPartitioning", _schema21);

        public static readonly CommandSymbol AlterMergeTablePolicyPartitioning =
            new CommandSymbol("AlterMergeTablePolicyPartitioning", _schema21);

        public static readonly CommandSymbol AlterMaterializedViewPolicyPartitioning =
            new CommandSymbol("AlterMaterializedViewPolicyPartitioning", _schema21);

        public static readonly CommandSymbol AlterMergeMaterializedViewPolicyPartitioning =
            new CommandSymbol("AlterMergeMaterializedViewPolicyPartitioning", _schema21);

        public static readonly CommandSymbol DeleteTablePolicyPartitioning =
            new CommandSymbol("DeleteTablePolicyPartitioning", _schema21);

        public static readonly CommandSymbol DeleteMaterializedViewPolicyPartitioning =
            new CommandSymbol("DeleteMaterializedViewPolicyPartitioning", _schema21);

        public static readonly CommandSymbol ShowTablePolicyRestrictedViewAccess =
            new CommandSymbol("ShowTablePolicyRestrictedViewAccess", _schema21);

        public static readonly CommandSymbol ShowTableStarPolicyRestrictedViewAccess =
            new CommandSymbol("ShowTableStarPolicyRestrictedViewAccess", _schema21);

        public static readonly CommandSymbol AlterTablePolicyRestrictedViewAccess =
            new CommandSymbol("AlterTablePolicyRestrictedViewAccess", _schema21);

        public static readonly CommandSymbol AlterTablesPolicyRestrictedViewAccess =
            new CommandSymbol("AlterTablesPolicyRestrictedViewAccess", _schema21);

        public static readonly CommandSymbol DeleteTablePolicyRestrictedViewAccess =
            new CommandSymbol("DeleteTablePolicyRestrictedViewAccess", _schema21);

        public static readonly CommandSymbol ShowClusterPolicyRowStore =
            new CommandSymbol("ShowClusterPolicyRowStore", _schema21);

        public static readonly CommandSymbol AlterClusterPolicyRowStore =
            new CommandSymbol("AlterClusterPolicyRowStore", _schema21);

        public static readonly CommandSymbol AlterMergeClusterPolicyRowStore =
            new CommandSymbol("AlterMergeClusterPolicyRowStore", _schema21);

        public static readonly CommandSymbol DeleteClusterPolicyRowStore =
            new CommandSymbol("DeleteClusterPolicyRowStore", _schema21);

        public static readonly CommandSymbol ShowClusterPolicySandbox =
            new CommandSymbol("ShowClusterPolicySandbox", _schema21);

        public static readonly CommandSymbol AlterClusterPolicySandbox =
            new CommandSymbol("AlterClusterPolicySandbox", _schema21);

        public static readonly CommandSymbol DeleteClusterPolicySandbox =
            new CommandSymbol("DeleteClusterPolicySandbox", _schema21);

        public static readonly CommandSymbol ShowClusterSandboxesStats =
            new CommandSymbol("ShowClusterSandboxesStats", _schema21);

        public static readonly CommandSymbol ShowDatabasePolicySharding =
            new CommandSymbol("ShowDatabasePolicySharding", _schema21);

        public static readonly CommandSymbol ShowTablePolicySharding =
            new CommandSymbol("ShowTablePolicySharding", _schema21);

        public static readonly CommandSymbol ShowTableStarPolicySharding =
            new CommandSymbol("ShowTableStarPolicySharding", _schema21);

        public static readonly CommandSymbol AlterDatabasePolicySharding =
            new CommandSymbol("AlterDatabasePolicySharding", _schema21);

        public static readonly CommandSymbol AlterTablePolicySharding =
            new CommandSymbol("AlterTablePolicySharding", _schema21);

        public static readonly CommandSymbol AlterMergeDatabasePolicySharding =
            new CommandSymbol("AlterMergeDatabasePolicySharding", _schema21);

        public static readonly CommandSymbol AlterMergeTablePolicySharding =
            new CommandSymbol("AlterMergeTablePolicySharding", _schema21);

        public static readonly CommandSymbol DeleteDatabasePolicySharding =
            new CommandSymbol("DeleteDatabasePolicySharding", _schema21);

        public static readonly CommandSymbol DeleteTablePolicySharding =
            new CommandSymbol("DeleteTablePolicySharding", _schema21);

        public static readonly CommandSymbol AlterClusterPolicySharding =
            new CommandSymbol("AlterClusterPolicySharding", _schema21);

        public static readonly CommandSymbol AlterMergeClusterPolicySharding =
            new CommandSymbol("AlterMergeClusterPolicySharding", _schema21);

        public static readonly CommandSymbol DeleteClusterPolicySharding =
            new CommandSymbol("DeleteClusterPolicySharding", _schema21);

        public static readonly CommandSymbol ShowClusterPolicySharding =
            new CommandSymbol("ShowClusterPolicySharding", _schema21);

        public static readonly CommandSymbol ShowDatabasePolicyShardsGrouping =
            new CommandSymbol("ShowDatabasePolicyShardsGrouping", _schema21);

        public static readonly CommandSymbol AlterDatabasePolicyShardsGrouping =
            new CommandSymbol("AlterDatabasePolicyShardsGrouping", _schema21);

        public static readonly CommandSymbol AlterMergeDatabasePolicyShardsGrouping =
            new CommandSymbol("AlterMergeDatabasePolicyShardsGrouping", _schema21);

        public static readonly CommandSymbol DeleteDatabasePolicyShardsGrouping =
            new CommandSymbol("DeleteDatabasePolicyShardsGrouping", _schema21);

        public static readonly CommandSymbol ShowDatabasePolicyStreamingIngestion =
            new CommandSymbol("ShowDatabasePolicyStreamingIngestion", _schema21);

        public static readonly CommandSymbol ShowTablePolicyStreamingIngestion =
            new CommandSymbol("ShowTablePolicyStreamingIngestion", _schema21);

        public static readonly CommandSymbol ShowClusterPolicyStreamingIngestion =
            new CommandSymbol("ShowClusterPolicyStreamingIngestion", _schema21);

        public static readonly CommandSymbol AlterDatabasePolicyStreamingIngestion =
            new CommandSymbol("AlterDatabasePolicyStreamingIngestion", _schema21);

        public static readonly CommandSymbol AlterMergeDatabasePolicyStreamingIngestion =
            new CommandSymbol("AlterMergeDatabasePolicyStreamingIngestion", _schema21);

        public static readonly CommandSymbol AlterTablePolicyStreamingIngestion =
            new CommandSymbol("AlterTablePolicyStreamingIngestion", _schema21);

        public static readonly CommandSymbol AlterMergeTablePolicyStreamingIngestion =
            new CommandSymbol("AlterMergeTablePolicyStreamingIngestion", _schema21);

        public static readonly CommandSymbol AlterClusterPolicyStreamingIngestion =
            new CommandSymbol("AlterClusterPolicyStreamingIngestion", _schema21);

        public static readonly CommandSymbol AlterMergeClusterPolicyStreamingIngestion =
            new CommandSymbol("AlterMergeClusterPolicyStreamingIngestion", _schema21);

        public static readonly CommandSymbol DeleteDatabasePolicyStreamingIngestion =
            new CommandSymbol("DeleteDatabasePolicyStreamingIngestion", _schema21);

        public static readonly CommandSymbol DeleteTablePolicyStreamingIngestion =
            new CommandSymbol("DeleteTablePolicyStreamingIngestion", _schema21);

        public static readonly CommandSymbol DeleteClusterPolicyStreamingIngestion =
            new CommandSymbol("DeleteClusterPolicyStreamingIngestion", _schema21);

        public static readonly CommandSymbol ShowDatabasePolicyManagedIdentity =
            new CommandSymbol("ShowDatabasePolicyManagedIdentity", _schema21);

        public static readonly CommandSymbol ShowClusterPolicyManagedIdentity =
            new CommandSymbol("ShowClusterPolicyManagedIdentity", _schema21);

        public static readonly CommandSymbol AlterDatabasePolicyManagedIdentity =
            new CommandSymbol("AlterDatabasePolicyManagedIdentity", _schema21);

        public static readonly CommandSymbol AlterMergeDatabasePolicyManagedIdentity =
            new CommandSymbol("AlterMergeDatabasePolicyManagedIdentity", _schema21);

        public static readonly CommandSymbol AlterClusterPolicyManagedIdentity =
            new CommandSymbol("AlterClusterPolicyManagedIdentity", _schema21);

        public static readonly CommandSymbol AlterMergeClusterPolicyManagedIdentity =
            new CommandSymbol("AlterMergeClusterPolicyManagedIdentity", _schema21);

        public static readonly CommandSymbol DeleteDatabasePolicyManagedIdentity =
            new CommandSymbol("DeleteDatabasePolicyManagedIdentity", _schema21);

        public static readonly CommandSymbol DeleteClusterPolicyManagedIdentity =
            new CommandSymbol("DeleteClusterPolicyManagedIdentity", _schema21);

        public static readonly CommandSymbol ShowTablePolicyAutoDelete =
            new CommandSymbol("ShowTablePolicyAutoDelete", _schema21);

        public static readonly CommandSymbol AlterTablePolicyAutoDelete =
            new CommandSymbol("AlterTablePolicyAutoDelete", _schema21);

        public static readonly CommandSymbol DeleteTablePolicyAutoDelete =
            new CommandSymbol("DeleteTablePolicyAutoDelete", _schema21);

        public static readonly CommandSymbol ShowClusterPolicyCallout =
            new CommandSymbol("ShowClusterPolicyCallout", _schema21);

        public static readonly CommandSymbol AlterClusterPolicyCallout =
            new CommandSymbol("AlterClusterPolicyCallout", _schema21);

        public static readonly CommandSymbol AlterMergeClusterPolicyCallout =
            new CommandSymbol("AlterMergeClusterPolicyCallout", _schema21);

        public static readonly CommandSymbol DeleteClusterPolicyCallout =
            new CommandSymbol("DeleteClusterPolicyCallout", _schema21);

        public static readonly CommandSymbol ShowClusterPolicyCapacity =
            new CommandSymbol("ShowClusterPolicyCapacity", _schema21);

        public static readonly CommandSymbol AlterClusterPolicyCapacity =
            new CommandSymbol("AlterClusterPolicyCapacity", _schema21);

        public static readonly CommandSymbol AlterMergeClusterPolicyCapacity =
            new CommandSymbol("AlterMergeClusterPolicyCapacity", _schema21);

        public static readonly CommandSymbol ShowClusterPolicyRequestClassification =
            new CommandSymbol("ShowClusterPolicyRequestClassification", _schema21);

        public static readonly CommandSymbol AlterClusterPolicyRequestClassification =
            new CommandSymbol("AlterClusterPolicyRequestClassification", _schema21);

        public static readonly CommandSymbol AlterMergeClusterPolicyRequestClassification =
            new CommandSymbol("AlterMergeClusterPolicyRequestClassification", _schema21);

        public static readonly CommandSymbol DeleteClusterPolicyRequestClassification =
            new CommandSymbol("DeleteClusterPolicyRequestClassification", _schema21);

        public static readonly CommandSymbol ShowClusterPolicyMultiDatabaseAdmins =
            new CommandSymbol("ShowClusterPolicyMultiDatabaseAdmins", _schema21);

        public static readonly CommandSymbol AlterClusterPolicyMultiDatabaseAdmins =
            new CommandSymbol("AlterClusterPolicyMultiDatabaseAdmins", _schema21);

        public static readonly CommandSymbol AlterMergeClusterPolicyMultiDatabaseAdmins =
            new CommandSymbol("AlterMergeClusterPolicyMultiDatabaseAdmins", _schema21);

        public static readonly CommandSymbol ShowDatabasePolicyDiagnostics =
            new CommandSymbol("ShowDatabasePolicyDiagnostics", _schema21);

        public static readonly CommandSymbol ShowClusterPolicyDiagnostics =
            new CommandSymbol("ShowClusterPolicyDiagnostics", _schema21);

        public static readonly CommandSymbol AlterDatabasePolicyDiagnostics =
            new CommandSymbol("AlterDatabasePolicyDiagnostics", _schema21);

        public static readonly CommandSymbol AlterMergeDatabasePolicyDiagnostics =
            new CommandSymbol("AlterMergeDatabasePolicyDiagnostics", _schema21);

        public static readonly CommandSymbol AlterClusterPolicyDiagnostics =
            new CommandSymbol("AlterClusterPolicyDiagnostics", _schema21);

        public static readonly CommandSymbol AlterMergeClusterPolicyDiagnostics =
            new CommandSymbol("AlterMergeClusterPolicyDiagnostics", _schema21);

        public static readonly CommandSymbol DeleteDatabasePolicyDiagnostics =
            new CommandSymbol("DeleteDatabasePolicyDiagnostics", _schema21);

        public static readonly CommandSymbol ShowClusterPolicyQueryWeakConsistency =
            new CommandSymbol("ShowClusterPolicyQueryWeakConsistency", _schema21);

        public static readonly CommandSymbol AlterClusterPolicyQueryWeakConsistency =
            new CommandSymbol("AlterClusterPolicyQueryWeakConsistency", _schema21);

        public static readonly CommandSymbol AlterMergeClusterPolicyQueryWeakConsistency =
            new CommandSymbol("AlterMergeClusterPolicyQueryWeakConsistency", _schema21);

        public static readonly CommandSymbol ShowTablePolicyExtentTagsRetention =
            new CommandSymbol("ShowTablePolicyExtentTagsRetention", _schema21);

        public static readonly CommandSymbol ShowTableStarPolicyExtentTagsRetention =
            new CommandSymbol("ShowTableStarPolicyExtentTagsRetention", _schema21);

        public static readonly CommandSymbol ShowDatabasePolicyExtentTagsRetention =
            new CommandSymbol("ShowDatabasePolicyExtentTagsRetention", _schema21);

        public static readonly CommandSymbol AlterTablePolicyExtentTagsRetention =
            new CommandSymbol("AlterTablePolicyExtentTagsRetention", _schema21);

        public static readonly CommandSymbol AlterDatabasePolicyExtentTagsRetention =
            new CommandSymbol("AlterDatabasePolicyExtentTagsRetention", _schema21);

        public static readonly CommandSymbol DeleteTablePolicyExtentTagsRetention =
            new CommandSymbol("DeleteTablePolicyExtentTagsRetention", _schema21);

        public static readonly CommandSymbol DeleteDatabasePolicyExtentTagsRetention =
            new CommandSymbol("DeleteDatabasePolicyExtentTagsRetention", _schema21);

        public static readonly CommandSymbol ShowPrincipalRoles =
            new CommandSymbol("ShowPrincipalRoles", _schema24);

        public static readonly CommandSymbol ShowDatabasePrincipalRoles =
            new CommandSymbol("ShowDatabasePrincipalRoles", _schema24);

        public static readonly CommandSymbol ShowTablePrincipalRoles =
            new CommandSymbol("ShowTablePrincipalRoles", _schema24);

        public static readonly CommandSymbol ShowGraphModelPrincipalRoles =
            new CommandSymbol("ShowGraphModelPrincipalRoles", _schema24);

        public static readonly CommandSymbol ShowExternalTablesPrincipalRoles =
            new CommandSymbol("ShowExternalTablesPrincipalRoles", _schema24);

        public static readonly CommandSymbol ShowFunctionPrincipalRoles =
            new CommandSymbol("ShowFunctionPrincipalRoles", _schema24);

        public static readonly CommandSymbol ShowClusterPrincipalRoles =
            new CommandSymbol("ShowClusterPrincipalRoles", _schema24);

        public static readonly CommandSymbol ShowClusterPrincipals =
            new CommandSymbol("ShowClusterPrincipals", _schema25);

        public static readonly CommandSymbol ShowDatabasePrincipals =
            new CommandSymbol("ShowDatabasePrincipals", _schema25);

        public static readonly CommandSymbol ShowTablePrincipals =
            new CommandSymbol("ShowTablePrincipals", _schema25);

        public static readonly CommandSymbol ShowGraphModelPrincipals =
            new CommandSymbol("ShowGraphModelPrincipals", _schema25);

        public static readonly CommandSymbol ShowFunctionPrincipals =
            new CommandSymbol("ShowFunctionPrincipals", _schema25);

        public static readonly CommandSymbol AddClusterRole =
            new CommandSymbol("AddClusterRole", _schema25);

        public static readonly CommandSymbol DropClusterRole =
            new CommandSymbol("DropClusterRole", _schema25);

        public static readonly CommandSymbol SetClusterRole =
            new CommandSymbol("SetClusterRole", _schema25);

        public static readonly CommandSymbol AddDatabaseRole =
            new CommandSymbol("AddDatabaseRole", _schema25);

        public static readonly CommandSymbol DropDatabaseRole =
            new CommandSymbol("DropDatabaseRole", _schema25);

        public static readonly CommandSymbol SetDatabaseRole =
            new CommandSymbol("SetDatabaseRole", _schema25);

        public static readonly CommandSymbol AddTableRole =
            new CommandSymbol("AddTableRole", _schema25);

        public static readonly CommandSymbol DropTableRole =
            new CommandSymbol("DropTableRole", _schema25);

        public static readonly CommandSymbol SetTableRole =
            new CommandSymbol("SetTableRole", _schema25);

        public static readonly CommandSymbol AddFunctionRole =
            new CommandSymbol("AddFunctionRole", _schema25);

        public static readonly CommandSymbol DropFunctionRole =
            new CommandSymbol("DropFunctionRole", _schema25);

        public static readonly CommandSymbol SetFunctionRole =
            new CommandSymbol("SetFunctionRole", _schema25);

        public static readonly CommandSymbol ShowClusterBlockedPrincipals =
            new CommandSymbol("ShowClusterBlockedPrincipals", _schema26);

        public static readonly CommandSymbol AddClusterBlockedPrincipals =
            new CommandSymbol("AddClusterBlockedPrincipals", _schema26);

        public static readonly CommandSymbol DropClusterBlockedPrincipals =
            new CommandSymbol("DropClusterBlockedPrincipals", _schema26);

        public static readonly CommandSymbol SetClusterMaintenanceMode =
            new CommandSymbol("SetClusterMaintenanceMode", _schema19);

        public static readonly CommandSymbol IngestIntoTable =
            new CommandSymbol("IngestIntoTable", "(ExtentId: guid, ItemLoaded: string, Duration: string, HasErrors: string, OperationId: guid)");

        public static readonly CommandSymbol IngestInlineIntoTable =
            new CommandSymbol("IngestInlineIntoTable", "(ExtentId: guid)");

        public static readonly CommandSymbol SetTable =
            new CommandSymbol("SetTable", _schema27);

        public static readonly CommandSymbol AppendTable =
            new CommandSymbol("AppendTable", _schema27);

        public static readonly CommandSymbol SetOrAppendTable =
            new CommandSymbol("SetOrAppendTable", _schema27);

        public static readonly CommandSymbol SetOrReplaceTable =
            new CommandSymbol("SetOrReplaceTable", _schema27);

        public static readonly CommandSymbol ExportToStorage =
            new CommandSymbol("ExportToStorage", _schema19);

        public static readonly CommandSymbol ExportToSqlTable =
            new CommandSymbol("ExportToSqlTable", _schema19);

        public static readonly CommandSymbol ExportToExternalTable =
            new CommandSymbol("ExportToExternalTable", _schema19);

        public static readonly CommandSymbol CreateOrAlterContinuousExport =
            new CommandSymbol("CreateOrAlterContinuousExport", _schema19);

        public static readonly CommandSymbol ShowContinuousExport =
            new CommandSymbol("ShowContinuousExport", _schema28);

        public static readonly CommandSymbol ShowContinuousExports =
            new CommandSymbol("ShowContinuousExports", _schema28);

        public static readonly CommandSymbol ShowClusterPendingContinuousExports =
            new CommandSymbol("ShowClusterPendingContinuousExports", _schema28);

        public static readonly CommandSymbol ShowContinuousExportExportedArtifacts =
            new CommandSymbol("ShowContinuousExportExportedArtifacts", _schema31);

        public static readonly CommandSymbol ShowContinuousExportFailures =
            new CommandSymbol("ShowContinuousExportFailures", _schema32);

        public static readonly CommandSymbol SetContinuousExportCursor =
            new CommandSymbol("SetContinuousExportCursor", _schema19);

        public static readonly CommandSymbol DropContinuousExport =
            new CommandSymbol("DropContinuousExport", _schema28);

        public static readonly CommandSymbol EnableContinuousExport =
            new CommandSymbol("EnableContinuousExport", _schema28);

        public static readonly CommandSymbol DisableContinuousExport =
            new CommandSymbol("DisableContinuousExport", _schema28);

        public static readonly CommandSymbol CreateMaterializedView =
            new CommandSymbol("CreateMaterializedView", _schema19);

        public static readonly CommandSymbol CreateMaterializedViewOverMaterializedView =
            new CommandSymbol("CreateMaterializedViewOverMaterializedView", _schema19);

        public static readonly CommandSymbol RenameMaterializedView =
            new CommandSymbol("RenameMaterializedView", _schema29);

        public static readonly CommandSymbol ShowMaterializedView =
            new CommandSymbol("ShowMaterializedView", _schema29);

        public static readonly CommandSymbol ShowMaterializedViews =
            new CommandSymbol("ShowMaterializedViews", _schema29);

        public static readonly CommandSymbol ShowMaterializedViewsDetails =
            new CommandSymbol("ShowMaterializedViewsDetails", _schema30);

        public static readonly CommandSymbol ShowMaterializedViewDetails =
            new CommandSymbol("ShowMaterializedViewDetails", _schema30);

        public static readonly CommandSymbol ShowMaterializedViewPolicyRetention =
            new CommandSymbol("ShowMaterializedViewPolicyRetention", _schema21);

        public static readonly CommandSymbol ShowMaterializedViewPolicyMerge =
            new CommandSymbol("ShowMaterializedViewPolicyMerge", _schema21);

        public static readonly CommandSymbol ShowMaterializedViewPolicyPartitioning =
            new CommandSymbol("ShowMaterializedViewPolicyPartitioning", _schema21);

        public static readonly CommandSymbol ShowMaterializedViewExtents =
            new CommandSymbol("ShowMaterializedViewExtents", _schema34);

        public static readonly CommandSymbol AlterMaterializedView =
            new CommandSymbol("AlterMaterializedView", _schema29);

        public static readonly CommandSymbol AlterMaterializedViewOverMaterializedView =
            new CommandSymbol("AlterMaterializedViewOverMaterializedView", _schema29);

        public static readonly CommandSymbol CreateOrAlterMaterializedView =
            new CommandSymbol("CreateOrAlterMaterializedView", _schema29);

        public static readonly CommandSymbol CreateOrAlterMaterializedViewOverMaterializedView =
            new CommandSymbol("CreateOrAlterMaterializedViewOverMaterializedView", _schema29);

        public static readonly CommandSymbol DropMaterializedView =
            new CommandSymbol("DropMaterializedView", _schema29);

        public static readonly CommandSymbol EnableDisableMaterializedView =
            new CommandSymbol("EnableDisableMaterializedView", _schema29);

        public static readonly CommandSymbol ShowMaterializedViewPrincipals =
            new CommandSymbol("ShowMaterializedViewPrincipals", _schema25);

        public static readonly CommandSymbol ShowMaterializedViewSchemaAsJson =
            new CommandSymbol("ShowMaterializedViewSchemaAsJson", _schema10);

        public static readonly CommandSymbol ShowMaterializedViewCslSchema =
            new CommandSymbol("ShowMaterializedViewCslSchema", _schema10);

        public static readonly CommandSymbol AlterMaterializedViewFolder =
            new CommandSymbol("AlterMaterializedViewFolder", _schema29);

        public static readonly CommandSymbol AlterMaterializedViewDocString =
            new CommandSymbol("AlterMaterializedViewDocString", _schema29);

        public static readonly CommandSymbol AlterMaterializedViewLookback =
            new CommandSymbol("AlterMaterializedViewLookback", _schema29);

        public static readonly CommandSymbol AlterMaterializedViewAutoUpdateSchema =
            new CommandSymbol("AlterMaterializedViewAutoUpdateSchema", _schema29);

        public static readonly CommandSymbol ClearMaterializedViewData =
            new CommandSymbol("ClearMaterializedViewData", _schema39);

        public static readonly CommandSymbol SetMaterializedViewCursor =
            new CommandSymbol("SetMaterializedViewCursor", _schema19);

        public static readonly CommandSymbol ShowTableOperationsMirroringStatus =
            new CommandSymbol("ShowTableOperationsMirroringStatus", "(TableName: string, IsEnabled: bool, ExportProperties: string, ManagedIdentityIdentifier: string, IsExportRunning: bool, LastExportStartTime: datetime, LastExportResult: string, LastExportedDataTime: datetime)");

        public static readonly CommandSymbol ShowTableOperationsMirroringExportedArtifacts =
            new CommandSymbol("ShowTableOperationsMirroringExportedArtifacts", _schema31);

        public static readonly CommandSymbol ShowTableOperationsMirroringFailures =
            new CommandSymbol("ShowTableOperationsMirroringFailures", _schema32);

        public static readonly CommandSymbol ShowCluster =
            new CommandSymbol("ShowCluster", "(NodeId: string, Address: string, Name: string, StartTime: datetime, IsAdmin: bool, MachineTotalMemory: long, MachineAvailableMemory: long, ProcessorCount: int, EnvironmentDescription: string)");

        public static readonly CommandSymbol ShowClusterDetails =
            new CommandSymbol("ShowClusterDetails", "(NodeId: string, Address: string, Name: string, StartTime: datetime, IsAdmin: bool, MachineTotalMemory: long, MachineAvailableMemory: long, ProcessorCount: int, EnvironmentDescription: string, NetAndClsVersion: string)");

        public static readonly CommandSymbol ShowDiagnostics =
            new CommandSymbol("ShowDiagnostics", "(IsHealthy: bool, EnvironmentDescription: string, IsScaleOutRequired: bool, MachinesTotal: int, MachinesOffline: int, NodeLastRestartedOn: datetime, AdminLastElectedOn: datetime, ExtentsTotal: int, DiskColdAllocationPercentage: int, InstancesTargetBasedOnDataCapacity: int, TotalOriginalDataSize: real, TotalExtentSize: real, IngestionsLoadFactor: real, IngestionsInProgress: long, IngestionsSuccessRate: real, MergesInProgress: long, BuildVersion: string, BuildTime: datetime, ClusterDataCapacityFactor: real, IsDataWarmingRequired: bool, DataWarmingLastRunOn: datetime, MergesSuccessRate: real, NotHealthyReason: string, IsAttentionRequired: bool, AttentionRequiredReason: string, ProductVersion: string, FailedIngestOperations: int, FailedMergeOperations: int, MaxExtentsInSingleTable: int, TableWithMaxExtents: string, MergesLoadFactor: double, WarmExtentSize: real, NumberOfDatabases: int, PurgeExtentsRebuildLoadFactor: real, PurgeExtentsRebuildInProgress: long, PurgesInProgress: long, RowStoreLocalStorageCapacityFactor: real, ExportsLoadFactor: real, ExportsInProgress: long, PendingContinuousExports: long, MaxContinuousExportLatenessMinutes: long, RowStoreSealsInProgress: long, IsRowStoreUnhealthy: bool, MachinesSuspended: int, DataPartitioningLoadFactor: real, DataPartitioningOperationsInProgress: long, MinPartitioningPercentageInSingleTable: real, TableWithMinPartitioningPercentage: string, V3DataCapacityFactor: real, MaterializedViewsInProgress: long, DataPartitioningOperationsInProgress: real, IngestionCapacityUtilization: real, ShardsWarmingStatus: string, ShardsWarmingTemperature: real, ShardsWarmingDetails: string, StoredQueryResultsInProgress: long, HotDataDiskSpaceUsage: real, MirroringOperationsLoadFactor: real, MirroringOperationsInProgress: long, QueryAccelerationLoadFactor: real, QueryAccelerationInProgress: long, QueryAccelerationCapacityUtilization: real, GraphSnapshotsLoadFactor: real, GraphSnapshotsInProgress: long");

        public static readonly CommandSymbol ShowCapacity =
            new CommandSymbol("ShowCapacity", "(Resource: string, Total: long, Consumed: long, Remaining: long)");

        public static readonly CommandSymbol ShowOperations =
            new CommandSymbol("ShowOperations", "(OperationId: guid, Operation: string, NodeId: string, StartedOn: datetime, LastUpdatedOn: datetime, Duration: timespan, State: string, Status: string, RootActivityId: guid, ShouldRetry: bool, Database: string, Principal: string, User: string, AdminEpochStartTime: datetime, VirtualCluster: string)");

        public static readonly CommandSymbol ShowOperationDetails =
            new CommandSymbol("ShowOperationDetails", _schema19);

        public static readonly CommandSymbol ShowJournal =
            new CommandSymbol("ShowJournal", _schema33);

        public static readonly CommandSymbol ShowDatabaseJournal =
            new CommandSymbol("ShowDatabaseJournal", _schema33);

        public static readonly CommandSymbol ShowClusterJournal =
            new CommandSymbol("ShowClusterJournal", _schema33);

        public static readonly CommandSymbol ShowQueries =
            new CommandSymbol("ShowQueries", "(ClientActivityId: string, Text: string, Database: string, StartedOn: datetime, LastUpdatedOn: datetime, Duration: timespan, State: string, RootActivityId: guid, User: string, FailureReason: string, TotalCpu: timespan, CacheStatistics: dynamic, Application: string, MemoryPeak: long, ScannedExtentsStatistics: dynamic, Principal: string, ClientRequestProperties: dynamic, ResultSetStatistics: dynamic, WorkloadGroup: string, OverallQueryStats: string, VirtualCluster: string)");

        public static readonly CommandSymbol ShowRunningQueries =
            new CommandSymbol("ShowRunningQueries", "(ClientActivityId: string, Text: string, Database: string, StartedOn: datetime, LastUpdatedOn: datetime, Duration: timespan, State: string, RootActivityId: guid, User: string, FailureReason: string, TotalCpu: timespan, CacheStatistics: dynamic, Application: string, MemoryPeak: long, ScannedEventStatistics: dynamic, Pricipal: string, ClientRequestProperties: dynamic, ResultSetStatistics: dynamic, WorkloadGroup: string)");

        public static readonly CommandSymbol CancelQuery =
            new CommandSymbol("CancelQuery", _schema19);

        public static readonly CommandSymbol ShowQueryPlan =
            new CommandSymbol("ShowQueryPlan", "(ResultType: string, Format: string, Content: string)");

        public static readonly CommandSymbol ShowCache =
            new CommandSymbol("ShowCache", "(NodeId: string, TotalMemoryCapacity: long, MemoryCacheCapacity: long, MemoryCacheInUse: long, MemoryCacheHitCount: long, TotalDiskCapacity: long, DiskCacheCapacity: long, DiskCacheInUse: long, DiskCacheHitCount: long, DiskCacheMissCount: long, MemoryCacheDetails: string, DiskCacheDetails: string, WarmedShardsSize: long, ShardsSizeToWarm: long, HotShardsSize: long)");

        public static readonly CommandSymbol AlterCache =
            new CommandSymbol("AlterCache", _schema19);

        public static readonly CommandSymbol ShowCommands =
            new CommandSymbol("ShowCommands", "(ClientActivityId: string, CommandType: string, Text: string, Database: string, StartedOn: datetime, LastUpdatedOn: datetime, Duration: timespan, State: string, RootActivityId: guid, User: string, FailureReason: string, Application: string, Principal: string, TotalCpu: timespan, ResourcesUtilization: dynamic, ClientRequestProperties: dynamic, WorkloadGroup: string, VirtualCluster: string)");

        public static readonly CommandSymbol ShowCommandsAndQueries =
            new CommandSymbol("ShowCommandsAndQueries", "(ClientActivityId: string, CommandType: string, Text: string, Database: string, StartedOn: datetime, LastUpdatedOn: datetime, Duration: timespan, State: string, FailureReason: string, RootActivityId: guid, User: string, Application: string, Principal: string, ClientRequestProperties: dynamic, TotalCpu: timespan, MemoryPeak: long, CacheStatistics: dynamic, ScannedExtentsStatistics: dynamic, ResultSetStatistics: dynamic, WorkloadGroup: string, VirtualCluster: string)");

        public static readonly CommandSymbol ShowIngestionFailures =
            new CommandSymbol("ShowIngestionFailures", "(OperationId: guid, Database: string, Table: string, FailedOn: datetime, IngestionSourcePath: string, Details: string, FailureKind: string, RootActivityId: guid, OperationKind: string, OriginatesFromUpdatePolicy: bool, ErrorCode: string, Principal: string, ShouldRetry: bool, User: string, IngestionProperties: string)");

        public static readonly CommandSymbol ShowDataOperations =
            new CommandSymbol("ShowDataOperations", "(Timestamp: datetime, Database: string, Table: string, ClientActivityId: string, OperationKind: string, OriginalSize: long, ExtentSize: long, RowCount: long, Duration: timespan, TotalCpu: timespan, ExtentCount: long, Principal: string, Properties: string)");

        public static readonly CommandSymbol ShowDatabaseKeyVaultSecrets =
            new CommandSymbol("ShowDatabaseKeyVaultSecrets", "(KeyVaultSecretId: string)");

        public static readonly CommandSymbol ShowClusterExtents =
            new CommandSymbol("ShowClusterExtents", _schema34);

        public static readonly CommandSymbol ShowClusterExtentsMetadata =
            new CommandSymbol("ShowClusterExtentsMetadata", _schema35);

        public static readonly CommandSymbol ShowDatabaseExtents =
            new CommandSymbol("ShowDatabaseExtents", _schema34);

        public static readonly CommandSymbol ShowDatabaseExtentsMetadata =
            new CommandSymbol("ShowDatabaseExtentsMetadata", _schema35);

        public static readonly CommandSymbol ShowDatabaseExtentTagsStatistics =
            new CommandSymbol("ShowDatabaseExtentTagsStatistics", "(TableName: string, TotalExtentsCount: long, TaggedExtentsCount: long, TotalTagsCount: long, TotalTagsLength: long, DropByTagsCount: long, DropByTagsLength: long, IngestByTagsCount: long, IngestByTagsLength: long)");

        public static readonly CommandSymbol ShowDatabaseExtentsPartitioningStatistics =
            new CommandSymbol("ShowDatabaseExtentsPartitioningStatistics", "(TableName: string, PartitioningPolicy: dynamic, TotalRowCount: long, PartitionedRowCount: long, PartitionedRowsPercentage: double)");

        public static readonly CommandSymbol ShowTableExtents =
            new CommandSymbol("ShowTableExtents", _schema34);

        public static readonly CommandSymbol ShowTableExtentsMetadata =
            new CommandSymbol("ShowTableExtentsMetadata", _schema35);

        public static readonly CommandSymbol TableShardsGroupShow =
            new CommandSymbol("TableShardsGroupShow", _schema34);

        public static readonly CommandSymbol TableShardsGroupMetadataShow =
            new CommandSymbol("TableShardsGroupMetadataShow", _schema35);

        public static readonly CommandSymbol TableShardGroupsShow =
            new CommandSymbol("TableShardGroupsShow", "(Id: guid, ShardCount: long, RowCount: long, OriginalSize: long, ShardSize: long, CompressedSize: long, IndexSize: long, DeletedRowCount: long, PartitionedRowCount: long, DateTimeColumnRanges: dynamic, Partition: dynamic, CreationTimeRange: dynamic)");

        public static readonly CommandSymbol TableShardGroupsStatisticsShow =
            new CommandSymbol("TableShardGroupsStatisticsShow", _schema36);

        public static readonly CommandSymbol TablesShardGroupsStatisticsShow =
            new CommandSymbol("TablesShardGroupsStatisticsShow", _schema36);

        public static readonly CommandSymbol DatabaseShardGroupsStatisticsShow =
            new CommandSymbol("DatabaseShardGroupsStatisticsShow", _schema36);

        public static readonly CommandSymbol MergeExtents =
            new CommandSymbol("MergeExtents", _schema37);

        public static readonly CommandSymbol MergeExtentsDryrun =
            new CommandSymbol("MergeExtentsDryrun", _schema37);

        public static readonly CommandSymbol MoveExtentsFrom =
            new CommandSymbol("MoveExtentsFrom", _schema38);

        public static readonly CommandSymbol MoveExtentsQuery =
            new CommandSymbol("MoveExtentsQuery", _schema38);

        public static readonly CommandSymbol TableShuffleExtents =
            new CommandSymbol("TableShuffleExtents", _schema38);

        public static readonly CommandSymbol TableShuffleExtentsQuery =
            new CommandSymbol("TableShuffleExtentsQuery", _schema38);

        public static readonly CommandSymbol ReplaceExtents =
            new CommandSymbol("ReplaceExtents", _schema38);

        public static readonly CommandSymbol DropExtent =
            new CommandSymbol("DropExtent", _schema39);

        public static readonly CommandSymbol DropExtents =
            new CommandSymbol("DropExtents", _schema39);

        public static readonly CommandSymbol DropExtentsPartitionMetadata =
            new CommandSymbol("DropExtentsPartitionMetadata", _schema19);

        public static readonly CommandSymbol DropPretendExtentsByProperties =
            new CommandSymbol("DropPretendExtentsByProperties", _schema39);

        public static readonly CommandSymbol ShowVersion =
            new CommandSymbol("ShowVersion", "(BuildVersion: string, BuildTime: datetime, ServiceType: string, ProductVersion: string)");

        public static readonly CommandSymbol ClearTableData =
            new CommandSymbol("ClearTableData", "(Status: string)");

        public static readonly CommandSymbol ClearTableCacheStreamingIngestionSchema =
            new CommandSymbol("ClearTableCacheStreamingIngestionSchema", "(NodeId: string, Status: string)");

        public static readonly CommandSymbol ShowStorageArtifactsCleanupState =
            new CommandSymbol("ShowStorageArtifactsCleanupState", "(DatabaseName: string, StorageAccount: string, ContainerId: guid, ContainerName: string, CleanupStateBlobName: string, LastModified: datetime, SizeInBytes: long, Content: string)");

        public static readonly CommandSymbol ClusterDropStorageArtifactsCleanupState =
            new CommandSymbol("ClusterDropStorageArtifactsCleanupState", "(ContainerId: guid, ContainerName: string, FileName: string, Succeeded: bool, FailureDetails: string)");

        public static readonly CommandSymbol StoredQueryResultSet =
            new CommandSymbol("StoredQueryResultSet", _schema19);

        public static readonly CommandSymbol StoredQueryResultSetOrReplace =
            new CommandSymbol("StoredQueryResultSetOrReplace", _schema19);

        public static readonly CommandSymbol StoredQueryResultsShow =
            new CommandSymbol("StoredQueryResultsShow", _schema40);

        public static readonly CommandSymbol StoredQueryResultShowSchema =
            new CommandSymbol("StoredQueryResultShowSchema", "(StoredQueryResult:string, Schema:string)");

        public static readonly CommandSymbol StoredQueryResultDrop =
            new CommandSymbol("StoredQueryResultDrop", _schema40);

        public static readonly CommandSymbol StoredQueryResultsDrop =
            new CommandSymbol("StoredQueryResultsDrop", _schema40);

        public static readonly CommandSymbol GraphModelCreateOrAlter =
            new CommandSymbol("GraphModelCreateOrAlter", "(Name:string, CreationTime:datetime, Id:guid, SnapshotCount:long, Model:string, RetentionPolicy:string, CachingPolicy:string)");

        public static readonly CommandSymbol GraphModelShow =
            new CommandSymbol("GraphModelShow", _schema19);

        public static readonly CommandSymbol GraphModelsShow =
            new CommandSymbol("GraphModelsShow", _schema19);

        public static readonly CommandSymbol GraphModelDrop =
            new CommandSymbol("GraphModelDrop", "(Name:string, CreationTime:datetime, Id:guid)");

        public static readonly CommandSymbol SetGraphModelAdmins =
            new CommandSymbol("SetGraphModelAdmins", _schema19);

        public static readonly CommandSymbol AddGraphModelAdmins =
            new CommandSymbol("AddGraphModelAdmins", _schema19);

        public static readonly CommandSymbol DropGraphModelAdmins =
            new CommandSymbol("DropGraphModelAdmins", _schema19);

        public static readonly CommandSymbol GraphSnapshotMake =
            new CommandSymbol("GraphSnapshotMake", "(Name:string, SnapshotTime:datetime, ModelName:string, ModelId:guid, ModelCreationTime:datetime, NodesCount: long, EdgesCount: long, RetentionPolicy:string, CachingPolicy:string)");

        public static readonly CommandSymbol GraphSnapshotShow =
            new CommandSymbol("GraphSnapshotShow", _schema19);

        public static readonly CommandSymbol GraphSnapshotsShow =
            new CommandSymbol("GraphSnapshotsShow", _schema19);

        public static readonly CommandSymbol GraphSnapshotDrop =
            new CommandSymbol("GraphSnapshotDrop", _schema41);

        public static readonly CommandSymbol GraphSnapshotsDrop =
            new CommandSymbol("GraphSnapshotsDrop", _schema41);

        public static readonly CommandSymbol GraphSnapshotShowStatistics =
            new CommandSymbol("GraphSnapshotShowStatistics", _schema42);

        public static readonly CommandSymbol GraphSnapshotsShowStatistics =
            new CommandSymbol("GraphSnapshotsShowStatistics", _schema42);

        public static readonly CommandSymbol GraphSnapshotShowFailures =
            new CommandSymbol("GraphSnapshotShowFailures", "(OperationId:guid, DatabaseName:string, Name:string, SnapshotTime:datetime, ModelName:string, ModelId:guid, TotalCpu:timespan, MemoryPeak:long, Duration:timespan, Details:string, FailureReason:string, FailureKind:string)");

        public static readonly CommandSymbol ShowCertificates =
            new CommandSymbol("ShowCertificates", _schema19);

        public static readonly CommandSymbol ShowCloudSettings =
            new CommandSymbol("ShowCloudSettings", _schema19);

        public static readonly CommandSymbol ShowCommConcurrency =
            new CommandSymbol("ShowCommConcurrency", _schema19);

        public static readonly CommandSymbol ShowCommPools =
            new CommandSymbol("ShowCommPools", _schema19);

        public static readonly CommandSymbol ShowFabricCache =
            new CommandSymbol("ShowFabricCache", _schema19);

        public static readonly CommandSymbol ShowFabricLocks =
            new CommandSymbol("ShowFabricLocks", _schema19);

        public static readonly CommandSymbol ShowFabricClocks =
            new CommandSymbol("ShowFabricClocks", _schema19);

        public static readonly CommandSymbol ShowFeatureFlags =
            new CommandSymbol("ShowFeatureFlags", _schema19);

        public static readonly CommandSymbol ShowMemPools =
            new CommandSymbol("ShowMemPools", _schema19);

        public static readonly CommandSymbol ShowServicePoints =
            new CommandSymbol("ShowServicePoints", _schema19);

        public static readonly CommandSymbol ShowTcpConnections =
            new CommandSymbol("ShowTcpConnections", _schema19);

        public static readonly CommandSymbol ShowTcpPorts =
            new CommandSymbol("ShowTcpPorts", _schema19);

        public static readonly CommandSymbol ShowThreadPools =
            new CommandSymbol("ShowThreadPools", _schema19);

        public static readonly CommandSymbol ExecuteDatabaseScript =
            new CommandSymbol("ExecuteDatabaseScript", _schema19);

        public static readonly CommandSymbol ExecuteClusterScript =
            new CommandSymbol("ExecuteClusterScript", _schema19);

        public static readonly CommandSymbol CreateRequestSupport =
            new CommandSymbol("CreateRequestSupport", _schema19);

        public static readonly CommandSymbol ShowRequestSupport =
            new CommandSymbol("ShowRequestSupport", _schema19);

        public static readonly CommandSymbol ShowClusterAdminState =
            new CommandSymbol("ShowClusterAdminState", _schema19);

        public static readonly CommandSymbol ClearRemoteClusterDatabaseSchema =
            new CommandSymbol("ClearRemoteClusterDatabaseSchema", _schema19);

        public static readonly CommandSymbol ShowClusterMonitoring =
            new CommandSymbol("ShowClusterMonitoring", _schema19);

        public static readonly CommandSymbol ShowClusterScaleIn =
            new CommandSymbol("ShowClusterScaleIn", _schema19);

        public static readonly CommandSymbol ShowClusterServices =
            new CommandSymbol("ShowClusterServices", "(NodeId: string, IsClusterAdmin: bool, IsFabricManager: bool, IsDatabaseAdmin: bool, IsWeakConsistencyNode: bool, IsRowStoreHostNode: bool, IsDataNode: bool)");

        public static readonly CommandSymbol ShowClusterNetwork =
            new CommandSymbol("ShowClusterNetwork", _schema19);

        public static readonly CommandSymbol AlterClusterStorageKeys =
            new CommandSymbol("AlterClusterStorageKeys", _schema19);

        public static readonly CommandSymbol ShowClusterStorageKeysHash =
            new CommandSymbol("ShowClusterStorageKeysHash", _schema19);

        public static readonly CommandSymbol AlterFabricServiceAssignmentsCommand =
            new CommandSymbol("AlterFabricServiceAssignmentsCommand", _schema19);

        public static readonly CommandSymbol DropFabricServiceAssignmentsCommand =
            new CommandSymbol("DropFabricServiceAssignmentsCommand", _schema19);

        public static readonly CommandSymbol CreateEntityGroupCommand =
            new CommandSymbol("CreateEntityGroupCommand", _schema43);

        public static readonly CommandSymbol CreateOrAlterEntityGroupCommand =
            new CommandSymbol("CreateOrAlterEntityGroupCommand", _schema43);

        public static readonly CommandSymbol AlterEntityGroup =
            new CommandSymbol("AlterEntityGroup", _schema43);

        public static readonly CommandSymbol AlterMergeEntityGroup =
            new CommandSymbol("AlterMergeEntityGroup", _schema43);

        public static readonly CommandSymbol DropEntityGroup =
            new CommandSymbol("DropEntityGroup", _schema43);

        public static readonly CommandSymbol ShowEntityGroup =
            new CommandSymbol("ShowEntityGroup", _schema43);

        public static readonly CommandSymbol ShowEntityGroups =
            new CommandSymbol("ShowEntityGroups", _schema43);

        public static readonly CommandSymbol AlterExtentContainersAdd =
            new CommandSymbol("AlterExtentContainersAdd", _schema44);

        public static readonly CommandSymbol AlterExtentContainersDrop =
            new CommandSymbol("AlterExtentContainersDrop", _schema19);

        public static readonly CommandSymbol AlterExtentContainersRecycle =
            new CommandSymbol("AlterExtentContainersRecycle", _schema19);

        public static readonly CommandSymbol AlterExtentContainersSet =
            new CommandSymbol("AlterExtentContainersSet", _schema44);

        public static readonly CommandSymbol ShowExtentContainers =
            new CommandSymbol("ShowExtentContainers", _schema44);

        public static readonly CommandSymbol DropEmptyExtentContainers =
            new CommandSymbol("DropEmptyExtentContainers", _schema19);

        public static readonly CommandSymbol CleanDatabaseExtentContainers =
            new CommandSymbol("CleanDatabaseExtentContainers", _schema19);

        public static readonly CommandSymbol ShowDatabaseExtentContainersCleanOperations =
            new CommandSymbol("ShowDatabaseExtentContainersCleanOperations", _schema19);

        public static readonly CommandSymbol ClearDatabaseCacheQueryResults =
            new CommandSymbol("ClearDatabaseCacheQueryResults", _schema19);

        public static readonly CommandSymbol ShowDatabaseCacheQueryResults =
            new CommandSymbol("ShowDatabaseCacheQueryResults", _schema19);

        public static readonly CommandSymbol ShowDatabasesManagementGroups =
            new CommandSymbol("ShowDatabasesManagementGroups", _schema19);

        public static readonly CommandSymbol AlterDatabaseStorageKeys =
            new CommandSymbol("AlterDatabaseStorageKeys", _schema19);

        public static readonly CommandSymbol ClearDatabaseCacheStreamingIngestionSchema =
            new CommandSymbol("ClearDatabaseCacheStreamingIngestionSchema", _schema19);

        public static readonly CommandSymbol ClearDatabaseCacheQueryWeakConsistency =
            new CommandSymbol("ClearDatabaseCacheQueryWeakConsistency", _schema19);

        public static readonly CommandSymbol ShowEntitySchema =
            new CommandSymbol("ShowEntitySchema", _schema19);

        public static readonly CommandSymbol ShowExtentDetails =
            new CommandSymbol("ShowExtentDetails", _schema19);

        public static readonly CommandSymbol ShowExtentColumnStorageStats =
            new CommandSymbol("ShowExtentColumnStorageStats", _schema19);

        public static readonly CommandSymbol AttachExtentsIntoTableByContainer =
            new CommandSymbol("AttachExtentsIntoTableByContainer", _schema19);

        public static readonly CommandSymbol AttachExtentsIntoTableByMetadata =
            new CommandSymbol("AttachExtentsIntoTableByMetadata", _schema19);

        public static readonly CommandSymbol AlterExtentTagsFromQuery =
            new CommandSymbol("AlterExtentTagsFromQuery", _schema19);

        public static readonly CommandSymbol AlterMergeExtentTagsFromQuery =
            new CommandSymbol("AlterMergeExtentTagsFromQuery", _schema19);

        public static readonly CommandSymbol DropExtentTagsFromQuery =
            new CommandSymbol("DropExtentTagsFromQuery", _schema19);

        public static readonly CommandSymbol DropExtentTagsFromTable =
            new CommandSymbol("DropExtentTagsFromTable", _schema19);

        public static readonly CommandSymbol DropExtentTagsRetention =
            new CommandSymbol("DropExtentTagsRetention", _schema19);

        public static readonly CommandSymbol MergeDatabaseShardGroups =
            new CommandSymbol("MergeDatabaseShardGroups", _schema19);

        public static readonly CommandSymbol AlterFollowerClusterConfiguration =
            new CommandSymbol("AlterFollowerClusterConfiguration", _schema19);

        public static readonly CommandSymbol AddFollowerDatabaseAuthorizedPrincipals =
            new CommandSymbol("AddFollowerDatabaseAuthorizedPrincipals", _schema19);

        public static readonly CommandSymbol DropFollowerDatabaseAuthorizedPrincipals =
            new CommandSymbol("DropFollowerDatabaseAuthorizedPrincipals", _schema19);

        public static readonly CommandSymbol AlterFollowerDatabaseAuthorizedPrincipals =
            new CommandSymbol("AlterFollowerDatabaseAuthorizedPrincipals", _schema19);

        public static readonly CommandSymbol DropFollowerDatabasePolicyCaching =
            new CommandSymbol("DropFollowerDatabasePolicyCaching", _schema19);

        public static readonly CommandSymbol AlterFollowerDatabaseChildEntities =
            new CommandSymbol("AlterFollowerDatabaseChildEntities", _schema19);

        public static readonly CommandSymbol AlterFollowerDatabaseConfiguration =
            new CommandSymbol("AlterFollowerDatabaseConfiguration", _schema19);

        public static readonly CommandSymbol DropFollowerDatabases =
            new CommandSymbol("DropFollowerDatabases", _schema19);

        public static readonly CommandSymbol ShowFollowerDatabase =
            new CommandSymbol("ShowFollowerDatabase", _schema19);

        public static readonly CommandSymbol AlterFollowerTablesPolicyCaching =
            new CommandSymbol("AlterFollowerTablesPolicyCaching", _schema19);

        public static readonly CommandSymbol DropFollowerTablesPolicyCaching =
            new CommandSymbol("DropFollowerTablesPolicyCaching", _schema19);

        public static readonly CommandSymbol ShowFreshness =
            new CommandSymbol("ShowFreshness", _schema19);

        public static readonly CommandSymbol ShowFunctionSchemaAsJson =
            new CommandSymbol("ShowFunctionSchemaAsJson", _schema19);

        public static readonly CommandSymbol SetMaterializedViewAdmins =
            new CommandSymbol("SetMaterializedViewAdmins", _schema19);

        public static readonly CommandSymbol AddMaterializedViewAdmins =
            new CommandSymbol("AddMaterializedViewAdmins", _schema19);

        public static readonly CommandSymbol DropMaterializedViewAdmins =
            new CommandSymbol("DropMaterializedViewAdmins", _schema19);

        public static readonly CommandSymbol SetMaterializedViewConcurrency =
            new CommandSymbol("SetMaterializedViewConcurrency", _schema19);

        public static readonly CommandSymbol ClearMaterializedViewStatistics =
            new CommandSymbol("ClearMaterializedViewStatistics", _schema19);

        public static readonly CommandSymbol ShowMaterializedViewStatistics =
            new CommandSymbol("ShowMaterializedViewStatistics", _schema19);

        public static readonly CommandSymbol ShowMaterializedViewDiagnostics =
            new CommandSymbol("ShowMaterializedViewDiagnostics", _schema19);

        public static readonly CommandSymbol ShowMaterializedViewFailures =
            new CommandSymbol("ShowMaterializedViewFailures", _schema19);

        public static readonly CommandSymbol ShowMemory =
            new CommandSymbol("ShowMemory", _schema19);

        public static readonly CommandSymbol CancelOperation =
            new CommandSymbol("CancelOperation", _schema19);

        public static readonly CommandSymbol DisablePlugin =
            new CommandSymbol("DisablePlugin", _schema19);

        public static readonly CommandSymbol EnablePlugin =
            new CommandSymbol("EnablePlugin", _schema19);

        public static readonly CommandSymbol ShowPlugins =
            new CommandSymbol("ShowPlugins", _schema19);

        public static readonly CommandSymbol ShowPrincipalAccess =
            new CommandSymbol("ShowPrincipalAccess", _schema19);

        public static readonly CommandSymbol ShowDatabasePurgeOperation =
            new CommandSymbol("ShowDatabasePurgeOperation", _schema19);

        public static readonly CommandSymbol ShowQueryExecution =
            new CommandSymbol("ShowQueryExecution", _schema19);

        public static readonly CommandSymbol AlterPoliciesOfRetention =
            new CommandSymbol("AlterPoliciesOfRetention", _schema19);

        public static readonly CommandSymbol DeletePoliciesOfRetention =
            new CommandSymbol("DeletePoliciesOfRetention", _schema19);

        public static readonly CommandSymbol CreateRowStore =
            new CommandSymbol("CreateRowStore", _schema19);

        public static readonly CommandSymbol DropRowStore =
            new CommandSymbol("DropRowStore", _schema19);

        public static readonly CommandSymbol ShowRowStore =
            new CommandSymbol("ShowRowStore", "(RowStoreName:string,RowStoreId:guid,RowStoreKey:string,OrdinalFrom:long,OrdinalTo:long,EstimatedDataSize:long,MinWriteAheadLogOffset:long, LocalStorageSize:long,LocalStorageStartOffset:long,Status:int,StatusLastUpdatedOn:datetime,DatabaseName:string,TableName:string,AssignedToNode:string,LatestIngestionTime:datetime)");

        public static readonly CommandSymbol ShowRowStores =
            new CommandSymbol("ShowRowStores", "(RowStoreName:string,RowStoreId:guid,WriteAheadLogStorage:string,PersistentStorage:string,IsActive:bool,AssignedToNode:string,NumberOfKeys:long,WriteAheadLogSize:long, WriteAheadLogStartOffset:long,LocalStorageSize:long,WriteAheadDistanceToSizeRatioThreshold:real,InsertsConcurrencyLimit:long,KeyInsertsConcurrencyLimit:long,UnsealedSizePerKeyLimit:long, NodeInsertsConcurrencyLimit:long,Status:string,StatusLastUpdatedOn:datetime,UsageTags:string,IsEmpty:bool,IsDataAvailableForQuery:bool)");

        public static readonly CommandSymbol ShowRowStoreTransactions =
            new CommandSymbol("ShowRowStoreTransactions", _schema19);

        public static readonly CommandSymbol ShowRowStoreSeals =
            new CommandSymbol("ShowRowStoreSeals", _schema19);

        public static readonly CommandSymbol ShowSchema =
            new CommandSymbol("ShowSchema", _schema19);

        public static readonly CommandSymbol ShowCallStacks =
            new CommandSymbol("ShowCallStacks", _schema19);

        public static readonly CommandSymbol ShowFileSystem =
            new CommandSymbol("ShowFileSystem", _schema19);

        public static readonly CommandSymbol ShowRunningCallouts =
            new CommandSymbol("ShowRunningCallouts", _schema19);

        public static readonly CommandSymbol ShowStreamingIngestionFailures =
            new CommandSymbol("ShowStreamingIngestionFailures", "(Database: string, Table: string, Principal: string, RootActivityId: guid, IngestionProperties: dynamic, Count: long, FirstFailureOn: datetime, LastFailureOn: datetime, FailureKind: string, ErrorCode: string, Details: string)");

        public static readonly CommandSymbol ShowStreamingIngestionStatistics =
            new CommandSymbol("ShowStreamingIngestionStatistics", "(Database:string, Table:string, StartTime:datetime, EndTime:datetime, Count:long, MinDuration:timespan, MaxDuration:timespan, AvgDuration:timespan, TotalDataSize:long, MinDataSize:long, MaxDataSize:long, TotalRowCount:long, MinRowCount:long, MaxRowCount:long, IngestionStatus:string, NumOfRowStoresReferences:int, Principal:string, NodeId:string, IngestionProperties:dynamic)");

        public static readonly CommandSymbol AlterTableRowStoreReferencesDropKey =
            new CommandSymbol("AlterTableRowStoreReferencesDropKey", _schema19);

        public static readonly CommandSymbol AlterTableRowStoreReferencesDropRowStore =
            new CommandSymbol("AlterTableRowStoreReferencesDropRowStore", _schema19);

        public static readonly CommandSymbol AlterTableRowStoreReferencesDropBlockedKeys =
            new CommandSymbol("AlterTableRowStoreReferencesDropBlockedKeys", _schema19);

        public static readonly CommandSymbol AlterTableRowStoreReferencesDisableKey =
            new CommandSymbol("AlterTableRowStoreReferencesDisableKey", _schema19);

        public static readonly CommandSymbol AlterTableRowStoreReferencesDisableRowStore =
            new CommandSymbol("AlterTableRowStoreReferencesDisableRowStore", _schema19);

        public static readonly CommandSymbol AlterTableRowStoreReferencesDisableBlockedKeys =
            new CommandSymbol("AlterTableRowStoreReferencesDisableBlockedKeys", _schema19);

        public static readonly CommandSymbol SetTableRowStoreReferences =
            new CommandSymbol("SetTableRowStoreReferences", _schema19);

        public static readonly CommandSymbol ShowTableRowStoreReferences =
            new CommandSymbol("ShowTableRowStoreReferences", "(DatabaseName:string, TableName:string, RowstoreReferenceKey:string, RowStoreName:string, EnabledForIngestion:bool)");

        public static readonly CommandSymbol AlterTableColumnStatistics =
            new CommandSymbol("AlterTableColumnStatistics", _schema19);

        public static readonly CommandSymbol AlterTableColumnStatisticsMethod =
            new CommandSymbol("AlterTableColumnStatisticsMethod", _schema19);

        public static readonly CommandSymbol ShowTableColumnStatitics =
            new CommandSymbol("ShowTableColumnStatitics", _schema19);

        public static readonly CommandSymbol ShowTableDimensions =
            new CommandSymbol("ShowTableDimensions", _schema19);

        public static readonly CommandSymbol DeleteTableRecords =
            new CommandSymbol("DeleteTableRecords", _schema19);

        public static readonly CommandSymbol TableDataUpdate =
            new CommandSymbol("TableDataUpdate", _schema19);

        public static readonly CommandSymbol DeleteMaterializedViewRecords =
            new CommandSymbol("DeleteMaterializedViewRecords", _schema19);

        public static readonly CommandSymbol ShowTableColumnsClassification =
            new CommandSymbol("ShowTableColumnsClassification", _schema19);

        public static readonly CommandSymbol ShowTableRowStores =
            new CommandSymbol("ShowTableRowStores", "(DatabaseName:string,TableName:string,ExtentId:guid,IsSealed:bool,RowStoreName:string,RowStoreId:string,RowStoreKey:string,OrdinalFrom:long,OrdinalTo:long,WriteAheadLogSize:long,LocalStorageSize:long,EstimatedDataSize:long,MinWriteAheadLogOffset:long)");

        public static readonly CommandSymbol ShowTableRowStoreSealInfo =
            new CommandSymbol("ShowTableRowStoreSealInfo", _schema19);

        public static readonly CommandSymbol ShowTablesColumnStatistics =
            new CommandSymbol("ShowTablesColumnStatistics", _schema19);

        public static readonly CommandSymbol ShowTableDataStatistics =
            new CommandSymbol("ShowTableDataStatistics", "(ColumnName: string, ColumnType: string, ColumnId: guid, OriginalSize: long, ExtentSize: long, CompressionRatio: real, DataCompressionSize: long, SharedIndexSize: long, IndexSize: long, IndexSizePercent: real,StorageEngineVersion: string, PresentRowCount: long, DeletedRowCount: long, SamplePercent: real, IncludeColdData: bool)");

        public static readonly CommandSymbol CreateTempStorage =
            new CommandSymbol("CreateTempStorage", _schema19);

        public static readonly CommandSymbol DropTempStorage =
            new CommandSymbol("DropTempStorage", _schema19);

        public static readonly CommandSymbol DropStoredQueryResultContainers =
            new CommandSymbol("DropStoredQueryResultContainers", _schema19);

        public static readonly CommandSymbol DropUnusedStoredQueryResultContainers =
            new CommandSymbol("DropUnusedStoredQueryResultContainers", _schema19);

        public static readonly CommandSymbol EnableDatabaseMaintenanceMode =
            new CommandSymbol("EnableDatabaseMaintenanceMode", _schema19);

        public static readonly CommandSymbol DisableDatabaseMaintenanceMode =
            new CommandSymbol("DisableDatabaseMaintenanceMode", _schema19);

        public static readonly CommandSymbol EnableDatabaseStreamingIngestionMaintenanceMode =
            new CommandSymbol("EnableDatabaseStreamingIngestionMaintenanceMode", _schema19);

        public static readonly CommandSymbol DisableDatabaseStreamingIngestionMaintenanceMode =
            new CommandSymbol("DisableDatabaseStreamingIngestionMaintenanceMode", _schema19);

        public static readonly CommandSymbol ShowQueryCallTree =
            new CommandSymbol("ShowQueryCallTree", _schema19);

        public static readonly CommandSymbol ShowExtentCorruptedDatetime =
            new CommandSymbol("ShowExtentCorruptedDatetime", _schema19);

        public static readonly CommandSymbol PatchExtentCorruptedDatetime =
            new CommandSymbol("PatchExtentCorruptedDatetime", _schema19);

        public static readonly CommandSymbol ClearClusterCredStoreCache =
            new CommandSymbol("ClearClusterCredStoreCache", _schema19);

        public static readonly CommandSymbol ClearClusterGroupMembershipCache =
            new CommandSymbol("ClearClusterGroupMembershipCache", _schema19);

        public static readonly CommandSymbol ClearExternalArtifactsCache =
            new CommandSymbol("ClearExternalArtifactsCache", _schema19);

        public static readonly CommandSymbol ShowDatabasesEntities =
            new CommandSymbol("ShowDatabasesEntities", _schema45);

        public static readonly CommandSymbol ShowDatabaseEntity =
            new CommandSymbol("ShowDatabaseEntity", _schema45);

        public static readonly CommandSymbol ReplaceDatabaseKeyVaultSecrets =
            new CommandSymbol("ReplaceDatabaseKeyVaultSecrets", _schema19);

        public static readonly IReadOnlyList<CommandSymbol> All = new CommandSymbol[]
        {
            ShowDatabase,
            ShowDatabaseDetails,
            ShowDatabaseIdentity,
            ShowDatabasePolicies,
            ShowDatabaseDataStats,
            ShowDatabaseMetadata,
            ShowClusterDatabases,
            ShowClusterDatabasesDetails,
            ShowClusterDatabasesIdentity,
            ShowClusterDatabasesPolicies,
            ShowClusterDatabasesDataStats,
            ShowClusterDatabasesMetadata,
            CreateDatabase,
            AttachDatabase,
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
            CreateOrAlterDatabaseIngestionMapping,
            AlterDatabaseIngestionMapping,
            AlterMergeDatabaseIngestionMapping,
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
            UndoDropExtentContainer,
            DropTable,
            UndoDropTable,
            DropTables,
            CreateTableIngestionMapping,
            CreateOrAlterTableIngestionMapping,
            AlterTableIngestionMapping,
            AlterMergeTableIngestionMapping,
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
            ShowExternalTablesDetails,
            ShowExternalTableDetails,
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
            ShowGraphModelPolicyCaching,
            ShowGraphModelStarPolicyCaching,
            ShowClusterPolicyCaching,
            AlterDatabasePolicyCaching,
            AlterTablePolicyCaching,
            AlterTablesPolicyCaching,
            AlterColumnPolicyCaching,
            AlterMaterializedViewPolicyCaching,
            AlterGraphModelPolicyCaching,
            AlterClusterPolicyCaching,
            DeleteDatabasePolicyCaching,
            DeleteTablePolicyCaching,
            DeleteColumnPolicyCaching,
            DeleteMaterializedViewPolicyCaching,
            DeleteGraphModelPolicyCaching,
            DeleteClusterPolicyCaching,
            ShowTablePolicyIngestionTime,
            ShowTableStarPolicyIngestionTime,
            AlterTablePolicyIngestionTime,
            AlterTablesPolicyIngestionTime,
            DeleteTablePolicyIngestionTime,
            ShowTablePolicyRetention,
            ShowTableStarPolicyRetention,
            ShowGraphPolicyRetention,
            ShowGraphStarPolicyRetention,
            ShowDatabasePolicyRetention,
            AlterTablePolicyRetention,
            AlterMaterializedViewPolicyRetention,
            AlterDatabasePolicyRetention,
            AlterGraphModelPolicyRetention,
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
            AlterMergeClusterPolicyIngestionBatching,
            AlterDatabasePolicyIngestionBatching,
            AlterMergeDatabasePolicyIngestionBatching,
            AlterTablePolicyIngestionBatching,
            AlterMergeTablePolicyIngestionBatching,
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
            AlterColumnsPolicyEncodingByQuery,
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
            AlterMergeMaterializedViewPolicyMerge,
            DeleteDatabasePolicyMerge,
            DeleteTablePolicyMerge,
            ShowExternalTablePolicyQueryAcceleration,
            ShowExternalTablesPolicyQueryAcceleration,
            AlterExternalTablePolicyQueryAcceleration,
            AlterMergeExternalTablePolicyQueryAcceleration,
            DeleteExternalTablePolicyQueryAcceleration,
            ShowExternalTableQueryAccelerationStatatistics,
            ShowExternalTablesQueryAccelerationStatatistics,
            AlterTablePolicyMirroring,
            AlterMergeTablePolicyMirroring,
            AlterTablePolicyMirroringWithJson,
            AlterMergeTablePolicyMirroringWithJson,
            DeleteTablePolicyMirroring,
            ShowTablePolicyMirroring,
            ShowTableStarPolicyMirroring,
            CreateMirroringTemplate,
            CreateOrAlterMirroringTemplate,
            AlterMirroringTemplate,
            AlterMergeMirroringTemplate,
            DeleteMirroringTemplate,
            ShowMirroringTemplate,
            ShowMirroringTemplates,
            ApplyMirroringTemplate,
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
            AlterMergeDatabasePolicyManagedIdentity,
            AlterClusterPolicyManagedIdentity,
            AlterMergeClusterPolicyManagedIdentity,
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
            ShowGraphModelPrincipalRoles,
            ShowExternalTablesPrincipalRoles,
            ShowFunctionPrincipalRoles,
            ShowClusterPrincipalRoles,
            ShowClusterPrincipals,
            ShowDatabasePrincipals,
            ShowTablePrincipals,
            ShowGraphModelPrincipals,
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
            SetClusterMaintenanceMode,
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
            CreateMaterializedViewOverMaterializedView,
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
            AlterMaterializedViewOverMaterializedView,
            CreateOrAlterMaterializedView,
            CreateOrAlterMaterializedViewOverMaterializedView,
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
            ShowTableOperationsMirroringStatus,
            ShowTableOperationsMirroringExportedArtifacts,
            ShowTableOperationsMirroringFailures,
            ShowCluster,
            ShowClusterDetails,
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
            ShowCache,
            AlterCache,
            ShowCommands,
            ShowCommandsAndQueries,
            ShowIngestionFailures,
            ShowDataOperations,
            ShowDatabaseKeyVaultSecrets,
            ShowClusterExtents,
            ShowClusterExtentsMetadata,
            ShowDatabaseExtents,
            ShowDatabaseExtentsMetadata,
            ShowDatabaseExtentTagsStatistics,
            ShowDatabaseExtentsPartitioningStatistics,
            ShowTableExtents,
            ShowTableExtentsMetadata,
            TableShardsGroupShow,
            TableShardsGroupMetadataShow,
            TableShardGroupsShow,
            TableShardGroupsStatisticsShow,
            TablesShardGroupsStatisticsShow,
            DatabaseShardGroupsStatisticsShow,
            MergeExtents,
            MergeExtentsDryrun,
            MoveExtentsFrom,
            MoveExtentsQuery,
            TableShuffleExtents,
            TableShuffleExtentsQuery,
            ReplaceExtents,
            DropExtent,
            DropExtents,
            DropExtentsPartitionMetadata,
            DropPretendExtentsByProperties,
            ShowVersion,
            ClearTableData,
            ClearTableCacheStreamingIngestionSchema,
            ShowStorageArtifactsCleanupState,
            ClusterDropStorageArtifactsCleanupState,
            StoredQueryResultSet,
            StoredQueryResultSetOrReplace,
            StoredQueryResultsShow,
            StoredQueryResultShowSchema,
            StoredQueryResultDrop,
            StoredQueryResultsDrop,
            GraphModelCreateOrAlter,
            GraphModelShow,
            GraphModelsShow,
            GraphModelDrop,
            SetGraphModelAdmins,
            AddGraphModelAdmins,
            DropGraphModelAdmins,
            GraphSnapshotMake,
            GraphSnapshotShow,
            GraphSnapshotsShow,
            GraphSnapshotDrop,
            GraphSnapshotsDrop,
            GraphSnapshotShowStatistics,
            GraphSnapshotsShowStatistics,
            GraphSnapshotShowFailures,
            ShowCertificates,
            ShowCloudSettings,
            ShowCommConcurrency,
            ShowCommPools,
            ShowFabricCache,
            ShowFabricLocks,
            ShowFabricClocks,
            ShowFeatureFlags,
            ShowMemPools,
            ShowServicePoints,
            ShowTcpConnections,
            ShowTcpPorts,
            ShowThreadPools,
            ExecuteDatabaseScript,
            ExecuteClusterScript,
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
            AlterFabricServiceAssignmentsCommand,
            DropFabricServiceAssignmentsCommand,
            CreateEntityGroupCommand,
            CreateOrAlterEntityGroupCommand,
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
            DropExtentTagsFromQuery,
            DropExtentTagsFromTable,
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
            ShowFileSystem,
            ShowRunningCallouts,
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
            TableDataUpdate,
            DeleteMaterializedViewRecords,
            ShowTableColumnsClassification,
            ShowTableRowStores,
            ShowTableRowStoreSealInfo,
            ShowTablesColumnStatistics,
            ShowTableDataStatistics,
            CreateTempStorage,
            DropTempStorage,
            DropStoredQueryResultContainers,
            DropUnusedStoredQueryResultContainers,
            EnableDatabaseMaintenanceMode,
            DisableDatabaseMaintenanceMode,
            EnableDatabaseStreamingIngestionMaintenanceMode,
            DisableDatabaseStreamingIngestionMaintenanceMode,
            ShowQueryCallTree,
            ShowExtentCorruptedDatetime,
            PatchExtentCorruptedDatetime,
            ClearClusterCredStoreCache,
            ClearClusterGroupMembershipCache,
            ClearExternalArtifactsCache,
            ShowDatabasesEntities,
            ShowDatabaseEntity,
            ReplaceDatabaseKeyVaultSecrets
        };
    }
}

