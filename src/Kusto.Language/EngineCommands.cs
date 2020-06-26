using System.Collections.Generic;

namespace Kusto.Language
{
    using Symbols;

    public static class EngineCommands
    {
        private static readonly string UnknownResult = "()";

        #region Schema Commands
        #region Databases
        private static readonly string ShowDatabaseResults =
            "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, ReservedSlot1: bool, DatabaseId: guid, InTransitionTo: string)";

        private static readonly string ShowDatabaseDetailsResults =
            "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, AuthorizedPrincipals: string, RetentionPolicy: string, MergePolicy: string, ReservedSlot1: string, CachingPolicy: string, ShardingPolicy: string, StreamingIngestionPolicy: string, IngestionBatchingPolicy: string, TotalSize: real, DatabaseId: guid, InTransitionTo: string)";

        public static readonly CommandSymbol ShowDatabase =
            new CommandSymbol(nameof(ShowDatabase),
                "show database",
                ShowDatabaseResults);

        public static readonly CommandSymbol ShowDatabaseDetails =
            new CommandSymbol(nameof(ShowDatabaseDetails),
                "show database details",
                ShowDatabaseDetailsResults);

        public static readonly CommandSymbol ShowDatabaseIdentity =
            new CommandSymbol(nameof(ShowDatabaseIdentity),
                "show database identity",
                "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, CurrentUserIsUnrestrictedViewer: bool, DatabaseId: guid, InTransitionTo: string)");

        public static readonly CommandSymbol ShowDatabasePolicies =
            new CommandSymbol(nameof(ShowDatabasePolicies),
                "show database policies",
                "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, DatabaseId: guid, AuthorizedPrincipals: string, RetentionPolicy: string, MergePolicy: string, CachingPolicy: string, ShardingPolicy: string, StreamingIngestionPolicy: string, IngestionBatchingPolicy: string)");

        public static readonly CommandSymbol ShowDatabaseDataStats =
            new CommandSymbol(nameof(ShowDatabaseDataStats),
                "show database datastats",
                "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, " +
                    "CurrentUseIsUnrestrictedViewer: bool, DatabaseId: guid, " +
                    "OriginalSize: real, ExtentSize: real, CompressedSize: real, IndexSize: real, RowCount: long, " + 
                    "HotOriginalSize: real, HotExtentSize: real, HotCompressedSize: real, HotIndexSize: real, HotRowCount: long)");

        public static readonly CommandSymbol ShowClusterDatabases =
            new CommandSymbol(nameof(ShowClusterDatabases),
                "show cluster databases ['(' { <database>:DatabaseName, ',' }+ ')']",
                "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, ReservedSlot1: bool, DatabaseId: guid, InTransitionTo: string)");

        public static readonly CommandSymbol ShowClusterDatabasesDetails =
            new CommandSymbol(nameof(ShowClusterDatabasesDetails),
                "show cluster databases details",
                "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, AuthorizedPrincipals: string, RetentionPolicy: string, MergePolicy: string, ReservedSlot1: string, CachingPolicy: string, ShardingPolicy: string, StreamingIngestionPolicy: string, IngestionBatchingPolicy: string, TotalSize: real, DatabaseId: guid, InTransitionTo: string)");

        public static readonly CommandSymbol ShowClusterDatabasesIdentity =
            new CommandSymbol(nameof(ShowClusterDatabasesIdentity),
                "show cluster databases identity",
                "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, CurrentUserIsUnrestrictedViewer: bool, DatabaseId: guid, InTransitionTo: string)");

        public static readonly CommandSymbol ShowClusterDatabasesPolicies =
            new CommandSymbol(nameof(ShowClusterDatabasesPolicies),
                "show cluster databases policies",
                "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, DatabaseId: guid, AuthorizedPrincipals: string, RetentionPolicy: string, MergePolicy: string, CachingPolicy: string, ShardingPolicy: string, StreamingIngestionPolicy: string, IngestionBatchingPolicy: string)");

        public static readonly CommandSymbol ShowClusterDatabasesDataStats =
            new CommandSymbol(nameof(ShowClusterDatabasesDataStats),
                "show cluster databases datastats",
                "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, DatabaseId: guid, OriginalSize: real, ExtentSize: real, CompressedSize: real, IndexSize: real, RowCount: long, HotOriginalSize: real, HotExtentSize: real, HotCompressedSize: real, HotIndexSize: real, HotRowCount: long)");

        public static readonly CommandSymbol CreateDatabasePersist =
            new CommandSymbol(nameof(CreateDatabasePersist),
                "create database <name>:DatabaseName persist '(' { <string>:Container, ',' }+ ')' [ifnotexists]",
                "(DatabaseName: string, PersistentPath: string, Created: string, StoresMetadata: bool, StoresData: bool)");

        public static readonly CommandSymbol CreateDatabaseVolatile =
            new CommandSymbol(nameof(CreateDatabaseVolatile),
                "create database <name>:DatabaseName volatile [ifnotexists]",
                "(DatabaseName: string, PersistentPath: string, Created: bool, StoresMetadata: bool, StoresData: bool)");

        public static readonly CommandSymbol AttachDatabase =
            new CommandSymbol(nameof(AttachDatabase),
                "attach database [metadata] <database>:DatabaseName from (<string>:BlobContainerUrl ';' <string>:StorageAccountKey | <string>:Path)",
                "(Step: string, Duration: string)");

        public static readonly CommandSymbol DetachDatabase =
            new CommandSymbol(nameof(DetachDatabase),
                "detach database <database>:DatabaseName",
                "(Table: string, NumberOfRemovedExtents: string)");

        public static readonly CommandSymbol AlterDatabasePrettyName =
            new CommandSymbol(nameof(AlterDatabasePrettyName),
                "alter database <database>:DatabaseName prettyname <string>:DatabasePrettyName",
                "(DatabaseName: string, PrettyName: string)");

        public static readonly CommandSymbol DropDatabasePrettyName =
            new CommandSymbol(nameof(DropDatabasePrettyName),
                "drop database <database>:DatabaseName prettyname",
                "(DatabaseName: string, PrettyName: string)");

        public static readonly CommandSymbol AlterDatabasePersistMetadata =
            new CommandSymbol(nameof(AlterDatabasePersistMetadata),
                "alter database <database>:DatabaseName persist metadata (<string>:BlobContainerUrl ';' <string>:StorageAccountKey | <string>:Path)",
                "(Moniker: guid, Url: string, State: string, CreatedOn: datetime, MaxDateTime: datetime, IsRecyclable: bool, StoresDatabaseMetadata: bool, HardDeletePeriod: timespan)");

        public static readonly CommandSymbol SetAccess =
            new CommandSymbol(nameof(SetAccess),
                "set access <database>:DatabaseName to (readonly | readwrite):AccessMode",
                "(DatabaseName: string, RequestedAccessMode: string, Status: string)");

        public static readonly string ShowDatabaseSchemaResults =
            "(DatabaseName: string, TableName: string, ColumnName: string, ColumnType: string, " + 
            "IsDefaultTable: bool, IsDefaultColumn: bool, PrettyName: string, Version: string, Folder: string, DocString: string)";

        public static readonly CommandSymbol ShowDatabaseSchema =
            new CommandSymbol(nameof(ShowDatabaseSchema),
                "show database (schema | <database>:DatabaseName schema) [details] [if_later_than <string>:Version]",
                ShowDatabaseSchemaResults);

        public static readonly CommandSymbol ShowDatabaseSchemaAsJson =
            new CommandSymbol(nameof(ShowDatabaseSchemaAsJson),
                "show database (schema | <database>:DatabaseName schema) [if_later_than <string>:Version] as json",
                "(DatabaseSchema: string)");

        public static readonly CommandSymbol ShowDatabasesSchema =
            new CommandSymbol(nameof(ShowDatabasesSchema),
                "show databases '(' { <database>:DatabaseName [if_later_than <string>:Version], ',' }+ ')' schema [details]",
                ShowDatabaseSchemaResults);

        public static readonly CommandSymbol ShowDatabasesSchemaAsJson =
            new CommandSymbol(nameof(ShowDatabasesSchemaAsJson),
                "show databases '(' { <database>:DatabaseName [if_later_than <string>:Version], ',' }+ ')' schema as json",
                "(DatabaseSchema: string)");
        #endregion

        #region Tables
        private static readonly string ShowTablesResult =
            "(TableName: string, DatabaseName: string, Folder: string, DocString: string)";

        private static readonly string ShowTablesDetailsResult =
            "(TableName: string, DatabaseName: string, Folder: string, DocString: string, TotalExtents: long, TotalExtentSize: real, TotalOriginalSize: real, TotalRowCount: long, HotExtents: long, HotExtentSize: real, HotOriginalSize: real, HotRowCount: long, AuthorizedPrincipals: string, RetentionPolicy: string, CachingPolicy: string, ShardingPolicy: string, MergePolicy: string, StreamingIngestionPolicy: string, IngestionBatchingPolicy: string, MinExtentsCreationTime: datetime, MaxExtentsCreationTime: datetime, RowOrderPolicy: string)";

        private static readonly string ShowTableSchemaResult =
            "(TableName: string, Schema: string, DatabaseName: string, Folder: string, DocString: string)";

        public static readonly CommandSymbol ShowTables =
            new CommandSymbol(nameof(ShowTables),
                "show tables ['(' { <table>:TableName, ',' }+ ')']",
                ShowTablesResult);

        public static readonly CommandSymbol ShowTable =
            new CommandSymbol(nameof(ShowTable),
                "show table <table>:TableName",
                "(AttributeName: string, AttributeType: string, ExtentSize: long, CompressionRatio: real, IndexSize: long, IndexSizePercent: real, OriginalSize: long, AttributeId: guid, SharedIndexSize: long, StorageEngineVersion: string)");

        public static readonly CommandSymbol ShowTablesDetails =
            new CommandSymbol(nameof(ShowTablesDetails),
                "show tables ['(' { <table>:TableName, ',' }+ ')'] details",
                ShowTablesDetailsResult);

        public static readonly CommandSymbol ShowTableDetails =
            new CommandSymbol(nameof(ShowTableDetails),
                "show table <table>:TableName details",
                ShowTablesDetailsResult);

        public static readonly CommandSymbol ShowTableCslSchema =
            new CommandSymbol(nameof(ShowTableCslSchema),
                "show table <table>:TableName cslschema",
                ShowTableSchemaResult);

        public static readonly CommandSymbol ShowTableSchemaAsJson =
            new CommandSymbol(nameof(ShowTableSchemaAsJson),
                "show table <table>:TableName schema as json",
                ShowTableSchemaResult);

        private static readonly string TableSchema = "('(' { <name>:ColumnName ':'! <type>:ColumnType, ',' }+ ')')";
        private static readonly string TableProperties = "with '(' docstring '=' <string>:Documentation [',' folder! '=' <string>:FolderName] ')'";

        public static readonly CommandSymbol CreateTable =
            new CommandSymbol(nameof(CreateTable),
                $"create table <name>:TableName {TableSchema} [{TableProperties}]",
                ShowTableSchemaResult);

        public static readonly CommandSymbol CreateMergeTable =
            new CommandSymbol(nameof(CreateMergeTable),
                $"create-merge table <name>:TableName {TableSchema}",
                ShowTableSchemaResult);

        public static readonly CommandSymbol CreateTables =
            new CommandSymbol(nameof(CreateTables),
                $"create tables {{ <name>:TableName {TableSchema}, ',' }}+",
                "(TableName: string, DatabaseName: string, Folder: string, DocString: string)");

        public static readonly CommandSymbol AlterTable =
            new CommandSymbol(nameof(AlterTable),
                $"alter table <table> {TableSchema} [{TableProperties}]",
                ShowTableSchemaResult);

        public static readonly CommandSymbol AlterMergeTable =
            new CommandSymbol(nameof(AlterMergeTable),
                $"alter-merge table <table> {TableSchema} [{TableProperties}]",
                ShowTableSchemaResult);

        public static readonly CommandSymbol AlterTableDocString =
            new CommandSymbol(nameof(AlterTableDocString),
                "alter table <table>:TableName docstring <string>:Documentation",
                "(TableName: string, Schema: string, DatabaseName: string, Folder: string, DocString: string)");

        public static readonly CommandSymbol AlterTableFolder =
            new CommandSymbol(nameof(AlterTableFolder),
                "alter table <table>:TableName folder <string>:Folder",
                "(TableName: string, Schema: string, DatabaseName: string, Folder: string, DocString: string)");

        public static readonly CommandSymbol RenameTable =
            new CommandSymbol(nameof(RenameTable),
                "rename table <table>:TableName to <name>:NewTableName",
                ShowTablesResult);

        public static readonly CommandSymbol RenameTables =
            new CommandSymbol(nameof(RenameTables),
                "rename tables { <name>:NewTableName '='! <table>:TableName, ',' }+",
                ShowTablesResult);

        public static readonly CommandSymbol DropTable =
            new CommandSymbol(nameof(DropTable),
                "drop table <table>:TableName [ifexists]",
                "(TableName: string, DatabaseName: string)");

        public static readonly CommandSymbol UndoDropTable =
            new CommandSymbol(nameof(UndoDropTable),
                "undo drop table <name> [as <name>:TableName] version '=' <string>:Version",
                "(ExtentId: guid, NumberOfRecords: long, Status: string, FailureReason: string)");

        private static readonly string TableNameList = "'(' { <table>:TableName, ',' }+ ')'";

        public static readonly CommandSymbol DropTables =
            new CommandSymbol(nameof(DropTables),
                $"drop tables {TableNameList} [ifexists]",
                "(TableName: string, DatabaseName: string)");

        private readonly static string TableIngestionMappingResult =
            "(Name: string, Kind: string, Mapping: string, LastUpdatedOn: datetime, Database: string, Table: string)";

        public static readonly CommandSymbol CreateTableIngestionMapping =
            new CommandSymbol(nameof(CreateTableIngestionMapping),
                "create table <name>:TableName ingestion! (csv | json | avro | parquet | orc):MappingKind mapping <string>:MappingName <string>:MappingFormat",
                TableIngestionMappingResult);

        public static readonly CommandSymbol AlterTableIngestionMapping =
            new CommandSymbol(nameof(AlterTableIngestionMapping),
                "alter table <table>:TableName ingestion (csv | json | avro | parquet | orc):MappingKind mapping <string>:MappingName <string>:MappingFormat",
                TableIngestionMappingResult);

        public static readonly CommandSymbol ShowTableIngestionMappings =
            new CommandSymbol(nameof(ShowTableIngestionMappings),
                "show table <table>:TableName ingestion (csv | json | avro | parquet | orc):MappingKind mappings",
                TableIngestionMappingResult);

        public static readonly CommandSymbol ShowTableIngestionMapping =
            new CommandSymbol(nameof(ShowTableIngestionMapping),
                "show table <table>:TableName ingestion (csv | json | avro | parquet | orc):MappingKind mapping <string>:MappingName",
                TableIngestionMappingResult);

        public static readonly CommandSymbol DropTableIngestionMapping =
            new CommandSymbol(nameof(DropTableIngestionMapping),
                "drop table <table>:TableName ingestion (csv | json | avro | parquet | orc):MappingKind mapping <string>:MappingName",
                TableIngestionMappingResult);
        #endregion

        #region Columns

        public static readonly CommandSymbol RenameColumn =
            new CommandSymbol(nameof(RenameColumn), 
                "rename column <database_table_column>:ColumnName to <name>:NewColumnName",
                "(EntityName: string, DataType: string, Policy: string)");

        public static readonly CommandSymbol RenameColumns =
            new CommandSymbol(nameof(RenameColumns),
                "rename columns { <name>:NewColumnName '='! <database_table_column>:ColumnName, ',' }+",
                "(EntityName: string, DataType: string, Policy: string)");

        public static readonly CommandSymbol AlterColumnType =
            new CommandSymbol(nameof(AlterColumnType),
                "alter column <database_table_column>:ColumnName type '=' <type>:ColumnType",
                "(EntityName: string, DataType: string, Policy: string)");

        public static readonly CommandSymbol DropColumn =
            new CommandSymbol(nameof(DropColumn), 
                "drop column <table_column>:ColumnName",
                "(TableName: string, Schema: string, DatabaseName: string, Folder: string, DocString: string)");

        public static readonly CommandSymbol DropTableColumns =
            new CommandSymbol(nameof(DropTableColumns),
                "drop table <table>:TableName columns '(' { <column>:ColumnName, ',' }+ ')'",
                "(TableName: string, Schema: string, DatabaseName: string, Folder: string, DocString: string)");

        public static readonly CommandSymbol AlterTableColumnDocStrings =
            new CommandSymbol(nameof(AlterTableColumnDocStrings), 
                "alter table <table>:TableName column-docstrings '(' { <column>:ColumnName ':'! <string>:DocString, ',' }+ ')'",
                "(TableName: string, Schema: string, DatabaseName: string, Folder: string, DocString: string)");

        public static readonly CommandSymbol AlterMergeTableColumnDocStrings =
            new CommandSymbol(nameof(AlterMergeTableColumnDocStrings),
                "alter-merge table <table>:TableName column-docstrings '(' { <column>:ColumnName ':'! <string>:DocString, ',' }+ ')'",
                "(TableName: string, Schema: string, DatabaseName: string, Folder: string, DocString: string)");
        #endregion

        #region Functions
        private static readonly string FunctionResult =
            "(Name: string, Parameters: string, Body: string, Folder: string, DocString: string)";

        public static readonly CommandSymbol ShowFunctions =
            new CommandSymbol(nameof(ShowFunctions),
                "show functions", 
                FunctionResult);

        public static readonly CommandSymbol ShowFunction =
            new CommandSymbol(nameof(ShowFunction),
                "show function <function>:FunctionName",
                FunctionResult);

        public static readonly CommandSymbol CreateFunction =
            new CommandSymbol(nameof(CreateFunction),
                "create function [ifnotexists] [with '('! { <name>:PropertyName '='! <value>:Value, ',' } ')'!] <name>:FunctionName <function_declaration>",
                FunctionResult);

        public static readonly CommandSymbol AlterFunction =
            new CommandSymbol(nameof(AlterFunction),
                "alter function [with '('! { <name>:PropertyName '='! <value>:Value, ',' }+ ')'!] <function>:FunctionName <function_declaration>",
                FunctionResult);

        public static readonly CommandSymbol CreateOrAlterFunction =
            new CommandSymbol(nameof(CreateOrAlterFunction),
                "create-or-alter function [with '('! { <name>:PropertyName '='! <value>:Value, ',' }+ ')'!] <name>:FunctionName <function_declaration>",
                FunctionResult);

        public static readonly CommandSymbol DropFunction =
            new CommandSymbol(nameof(DropFunction),
                "drop function <function>:FunctionName",
                "(Name: string)");

        public static readonly CommandSymbol AlterFunctionDocString =
            new CommandSymbol(nameof(AlterFunctionDocString),
                "alter function <function> docstring <string>:Documentation", 
                FunctionResult);

        public static readonly CommandSymbol AlterFunctionFolder =
            new CommandSymbol(nameof(AlterFunctionFolder),
                "alter function <function>:FunctionName folder <string>:Folder", 
                FunctionResult);
        #endregion

        #region External Tables
        private static readonly string ExternalTableResult =
            "(TableName: string, TableType: string, Folder: string, DocString: string, Properties: string, ConnectionStrings: dynamic, Partitions: dynamic, PathFormat: string)";

        private static readonly string ExternalTableSchemaResult =
            "(TableName: string, Schema: string, DatabaseName: string, Folder: string, DocString: string)";

        private static readonly string ExternalTableArtifactsResult =
            "(Uri: string)";

        private static readonly string ExternalTableFullResult =
            "(TableName: string, TableType: string, Folder: string, DocString: string, Schema: string, Properties: string)";

        public static readonly CommandSymbol ShowExternalTables =
            new CommandSymbol(nameof(ShowExternalTables),
                "show external tables", 
                ExternalTableResult);

        public static readonly CommandSymbol ShowExternalTable =
            new CommandSymbol(nameof(ShowExternalTable),
                "show external table <table>:ExternalTableName",
                ExternalTableResult);

        public static readonly CommandSymbol ShowExternalTableCslSchema =
            new CommandSymbol(nameof(ShowExternalTableCslSchema),
                "show external table <table>:ExternalTableName cslschema",
                ExternalTableSchemaResult);

        public static readonly CommandSymbol ShowExternalTableSchema =
            new CommandSymbol(nameof(ShowExternalTableSchema),
                "show external table <table>:ExternalTableName schema as (json | csl)", 
                ExternalTableSchemaResult);

        public static readonly CommandSymbol ShowExternalTableArtifacts =
            new CommandSymbol(nameof(ShowExternalTableArtifacts),
                "show external table <table>:ExternalTableName artifacts [limit <long>:LimitCount]", 
                ExternalTableArtifactsResult);

        public static readonly CommandSymbol DropExternalTable =
            new CommandSymbol(nameof(DropExternalTable),
                "drop external table <table>:ExternalTableName",
                ExternalTableResult);

        private static readonly string CreateOrAlterExternalTableGrammar =
            @"external table <name>:ExternalTableName '(' { <name>:ColumnName ':'! <type>:ColumnType, ',' }+ ')'
              kind '='! (blob | adl):TableKind
              [partition by!
               (
                {(format_datetime '='! <string>:DateTimeFormat bin '('! <name>:DateTimeColumn ',' <timespan>:BinValue ')'
                  | bin '('! <name>:DateTimeColumn ',' <timespan>:BinValue ')'
                  | [<string>:StringPartitionPrefix] (<name>:StringColumn | hash '('! <name>:StringColumn ',' <long>:HashMod ')') [<string>:StringPartitionSuffix]), ','}+
               |
                '('
                 {<name>:PartitionName ':'!
                  (string:PartitionType ['=' <name>:StringColumn]
                   | datetime:PartitionType ['=' 
                     (<name>:DateTimeColumn 
                      | bin:PartitionFunction '('! <name>:DateTimeColumn ',' <timespan>:BinValue ')'
                      | (startofday | startofweek | startofmonth | startofyear):PartitionFunction '('! <name>:DateTimeColumn ')')]
                   | long:PartitionType '='! hash:PartitionFunction '(' <name>:StringColumn ',' <long>:HashMod ')'), ','}+
                ')'
                [pathformat '='! '(' 
                 [<string>:PathSeparator]
                 { (<name>:PartitionName | datetime_pattern '('! <string>:DateTimeFormat ',' <name>:PartitionName ')')
                  [<string>:PathSeparator] }+ ')']
               )
              ]
              dataformat '='! (avro | apacheavro | csv | json | multijson | parquet | psv | raw | scsv | sohsv | sstream | tsv | tsve | txt | w3clogfile):DataFormatKind
              '(' { <string>:StorageConnectionString, ',' }+ ')'
              [with '('! { <name>:PropertyName '='! <value>:Value, ',' }+ ')']";

        public static readonly CommandSymbol CreateExternalTable =
            new CommandSymbol(nameof(CreateExternalTable), 
                "create " + CreateOrAlterExternalTableGrammar,
                ExternalTableResult);

        public static readonly CommandSymbol AlterExternalTable =
            new CommandSymbol(nameof(AlterExternalTable),
                "alter " + CreateOrAlterExternalTableGrammar,
                ExternalTableFullResult);

        public static readonly CommandSymbol CreateOrAlterExternalTable =
            new CommandSymbol(nameof(CreateOrAlterExternalTable), 
                "create-or-alter " + CreateOrAlterExternalTableGrammar,
                ExternalTableFullResult);

        public static readonly CommandSymbol CreateExternalTableMapping =
            new CommandSymbol(nameof(CreateExternalTableMapping),
                "create external table <table>:ExternalTableName (csv | json | avro | parquet | orc):MappingKind mapping <string>:MappingName <string>:MappingFormat",
                TableIngestionMappingResult);

        public static readonly CommandSymbol AlterExternalTableMapping =
            new CommandSymbol(nameof(AlterExternalTableMapping),
                "alter external table <table>:ExternalTableName (csv | json | avro | parquet | orc):MappingKind mapping <string>:MappingName <string>:MappingFormat",
                TableIngestionMappingResult);

        public static readonly CommandSymbol ShowExternalTableMappings =
            new CommandSymbol(nameof(ShowExternalTableMappings),
                "show external table <table>:ExternalTableName (csv | json | avro | parquet | orc):MappingKind mappings",
                TableIngestionMappingResult);

        public static readonly CommandSymbol ShowExternalTableMapping =
            new CommandSymbol(nameof(ShowExternalTableMapping),
                "show external table <table>:ExternalTableName (csv | json | avro | parquet | orc):MappingKind mapping <string>:MappingName",
                TableIngestionMappingResult);

        public static readonly CommandSymbol DropExternalTableMapping =
            new CommandSymbol(nameof(DropExternalTableMapping),
                "drop external table <table>:ExternalTableName (csv | json | avro | parquet | orc):MappingKind mapping <string>:MappingName",
                TableIngestionMappingResult);
        
        #endregion

        #endregion

        #region Policy Commands
        private static readonly string PolicyResult =
            "(PolicyName: string, EntityName: string, Policy: string, ChildEntities: string, EntityType: string)";

        #region Caching
        public static readonly CommandSymbol ShowDatabasePolicyCaching =
            new CommandSymbol(nameof(ShowDatabasePolicyCaching), 
                "show database (<database> | '*'):DatabaseName policy caching",
                PolicyResult);

        public static readonly CommandSymbol ShowTablePolicyCaching =
            new CommandSymbol(nameof(ShowTablePolicyCaching), 
                "show table (<database_table> | '*'):TableName policy caching",
                PolicyResult);

        public static readonly CommandSymbol AlterDatabasePolicyCaching =
            new CommandSymbol(nameof(AlterDatabasePolicyCaching),
                "alter database <database> policy caching hot '=' <timespan>:Timespan",
                PolicyResult);

        public static readonly CommandSymbol AlterTablePolicyCaching =
            new CommandSymbol(nameof(AlterTablePolicyCaching),
                "alter table <database_table> policy caching hot '=' <timespan>:Timespan",
                PolicyResult);

        public static readonly CommandSymbol AlterClusterPolicyCaching =
            new CommandSymbol(nameof(AlterClusterPolicyCaching),
                "alter cluster policy caching hot '=' <timespan>:Timespan",
                PolicyResult);

        public static readonly CommandSymbol DeleteTablePolicyCaching =
            new CommandSymbol(nameof(DeleteTablePolicyCaching),
                "delete table <database_table> policy caching",
                PolicyResult);
        #endregion

        #region IngestionTime
        public static readonly CommandSymbol ShowTablePolicyIngestionTime =
            new CommandSymbol(nameof(ShowTablePolicyIngestionTime), 
                "show table (<table> | '*'):TableName policy ingestiontime",
                PolicyResult);

        public static readonly CommandSymbol AlterTablePolicyIngestionTime =
            new CommandSymbol(nameof(AlterTablePolicyIngestionTime),
                "alter table <table>:TableName policy ingestiontime true",
                PolicyResult);

        public static readonly CommandSymbol AlterTablesPolicyIngestionTime =
            new CommandSymbol(nameof(AlterTablesPolicyIngestionTime),
                "alter tables '(' { <table>:TableName, ',' }+ ')' policy ingestiontime true",
                PolicyResult);

        public static readonly CommandSymbol DeleteTablePolicyIngestionTime =
            new CommandSymbol(nameof(DeleteTablePolicyIngestionTime), 
                "delete table <table>:TableName policy ingestiontime",
                PolicyResult);
        #endregion

        #region Retention
        public static readonly CommandSymbol ShowTablePolicyRetention =
            new CommandSymbol(nameof(ShowTablePolicyRetention),
                "show table (<database_table> | '*'):TableName policy retention",
                PolicyResult);

        public static readonly CommandSymbol ShowDatabasePolicyRetention =
            new CommandSymbol(nameof(ShowDatabasePolicyRetention),
                "show database (<database> | '*'):DatabaseName policy retention",
                PolicyResult);

        public static readonly CommandSymbol AlterTablePolicyRetention =
            new CommandSymbol(nameof(AlterTablePolicyRetention),
                "alter table <database_table>:TableName policy retention <string>:RetentionPolicy",
                PolicyResult);

        public static readonly CommandSymbol AlterDatabasePolicyRetention =
            new CommandSymbol(nameof(AlterDatabasePolicyRetention),
                "alter database <database>:DatabaseName policy retention <string>:RetentionPolicy",
                PolicyResult);

        public static readonly CommandSymbol AlterTablesPolicyRetention =
            new CommandSymbol(nameof(AlterTablesPolicyRetention),
                "alter tables '(' { <table>:TableName, ',' }+ ')' policy retention <string>:RetentionPolicy",
                PolicyResult);

        public static readonly CommandSymbol AlterMergeTablePolicyRetention =
            new CommandSymbol(nameof(AlterMergeTablePolicyRetention),
                "alter-merge table <database_table>:TableName policy retention (<string>:RetentionPolicy | softdelete '='! <timespan>:SoftDeleteValue [recoverability '='! (disabled|enabled):RecoverabilityValue] | recoverability '='! (disabled|enabled):RecoverabilityValue)",
                PolicyResult);

        public static readonly CommandSymbol AlterMergeDatabasePolicyRetention =
            new CommandSymbol(nameof(AlterMergeDatabasePolicyRetention),
                "alter-merge database <database>:DatabaseName policy retention (<string>:RetentionPolicy | softdelete '='! <timespan>:SoftDeleteValue [recoverability '='! (disabled|enabled):RecoverabilityValue] | recoverability '='! (disabled|enabled):RecoverabilityValue)",
                PolicyResult);

        public static readonly CommandSymbol DeleteTablePolicyRetention =
            new CommandSymbol(nameof(DeleteTablePolicyRetention),
                "delete table <database_table>:TableName policy retention",
                PolicyResult);

        public static readonly CommandSymbol DeleteDatabasePolicyRetention =
            new CommandSymbol(nameof(DeleteDatabasePolicyRetention),
                "delete database <database>:DatabaseName policy retention",
                PolicyResult);
        #endregion

        #region RowLevelSecurity
        public static readonly CommandSymbol ShowTablePolicyRowLevelSecurity =
            new CommandSymbol(nameof(ShowTablePolicyRowLevelSecurity),
                "show table (<table> | '*'):TableName policy row_level_security",
                PolicyResult);

        public static readonly CommandSymbol AlterTablePolicyRowLevelSecurity =
            new CommandSymbol(nameof(AlterTablePolicyRowLevelSecurity),
                "alter table <table>:TableName policy row_level_security (enable | disable) <string>:Query",
                PolicyResult);

        public static readonly CommandSymbol DeleteTablePolicyRowLevelSecurity =
            new CommandSymbol(nameof(DeleteTablePolicyRowLevelSecurity),
                "delete table <table>:TableName policy row_level_security",
                PolicyResult);
        #endregion

        #region RowOrder
        public static readonly CommandSymbol ShowTablePolicyRowOrder =
            new CommandSymbol(nameof(ShowTablePolicyRowOrder),
                "show table (<database_table> | '*'):TableName policy roworder",
                PolicyResult);

        public static readonly CommandSymbol AlterTablePolicyRowOrder =
            new CommandSymbol(nameof(AlterTablePolicyRowOrder),
                "alter table <database_table>:TableName policy roworder '('! { <column>:ColumnName (asc|desc)!, ',' }+ ')'!",
                PolicyResult);

        public static readonly CommandSymbol AlterTablesPolicyRowOrder =
            new CommandSymbol(nameof(AlterTablesPolicyRowOrder),
                "alter tables '(' { <table>:TableName, ',' }+ ')' policy roworder '(' { <name>:ColumnName (asc|desc)!, ',' }+ ')'",
                PolicyResult);

        public static readonly CommandSymbol AlterMergeTablePolicyRowOrder =
            new CommandSymbol(nameof(AlterMergeTablePolicyRowOrder),
                "alter-merge table <database_table>:TableName policy roworder '(' { <column>:ColumnName (asc|desc)!, ',' }+ ')'",
                PolicyResult);

        public static readonly CommandSymbol DeleteTablePolicyRowOrder =
            new CommandSymbol(nameof(DeleteTablePolicyRowOrder),
                "delete table <database_table>:TableName policy roworder",
                PolicyResult);
        #endregion

        #region Update
        public static readonly CommandSymbol ShowTablePolicyUpdate =
            new CommandSymbol(nameof(ShowTablePolicyUpdate),
                "show table (<database_table> | '*'):TableName policy update",
                PolicyResult);

        public static readonly CommandSymbol AlterTablePolicyUpdate =
            new CommandSymbol(nameof(AlterTablePolicyUpdate),
                "alter table <database_table>:TableName policy update <string>:UpdatePolicy", 
                PolicyResult);

        public static readonly CommandSymbol AlterMergeTablePolicyUpdate =
            new CommandSymbol(nameof(AlterMergeTablePolicyUpdate), 
                "alter-merge table <database_table>:TableName policy update <string>:UpdatePolicy", 
                PolicyResult);

        public static readonly CommandSymbol DeleteTablePolicyUpdate =
            new CommandSymbol(nameof(DeleteTablePolicyUpdate),
                "delete table <database_table>:TableName policy update",
                PolicyResult);
        #endregion

        #region Batch
        public static readonly CommandSymbol ShowDatabasePolicyIngestionBatching =
            new CommandSymbol(nameof(ShowDatabasePolicyIngestionBatching),
                "show database (<database> | '*'):DatabaseName policy ingestionbatching",
                PolicyResult);

        public static readonly CommandSymbol ShowTablePolicyIngestionBatching =
            new CommandSymbol(nameof(ShowTablePolicyIngestionBatching),
                "show table (<database_table> | '*'):TableName policy ingestionbatching",
                PolicyResult);

        public static readonly CommandSymbol AlterDatabasePolicyIngestionBatching =
            new CommandSymbol(nameof(AlterDatabasePolicyIngestionBatching),
                "alter database <database>:DatabaseName policy ingestionbatching <string>:IngestionBatchingPolicy",
                PolicyResult);

        public static readonly CommandSymbol AlterTablePolicyIngestionBatching =
            new CommandSymbol(nameof(AlterTablePolicyIngestionBatching),
                "alter table <database_table>:TableName policy ingestionbatching <string>:IngestionBatchingPolicy",
                PolicyResult);

        public static readonly CommandSymbol AlterTablesPolicyIngestionBatching =
            new CommandSymbol(nameof(AlterTablesPolicyIngestionBatching),
                "alter tables '(' { <table>:TableName, ',' }+ ')' policy ingestionbatching <string>:IngestionBatchingPolicy",
                PolicyResult);

        public static readonly CommandSymbol DeleteDatabasePolicyIngestionBatching =
            new CommandSymbol(nameof(DeleteDatabasePolicyIngestionBatching),
                "delete database <database>:DatabaseName policy ingestionbatching",
                PolicyResult);

        public static readonly CommandSymbol DeleteTablePolicyIngestionBatching =
            new CommandSymbol(nameof(DeleteTablePolicyIngestionBatching),
                "delete table <database_table>:TableName policy ingestionbatching",
                PolicyResult);
        #endregion

        #region Encoding
        public static readonly CommandSymbol ShowDatabasePolicyEncoding =
            new CommandSymbol(nameof(ShowDatabasePolicyEncoding),
                "show database <database>:DatabaseName policy encoding",
                PolicyResult);

        public static readonly CommandSymbol ShowTablePolicyEncoding =
            new CommandSymbol(nameof(ShowTablePolicyEncoding),
                "show table <database_table>:TableName policy encoding", 
                PolicyResult);

        public static readonly CommandSymbol ShowColumnPolicyEncoding =
            new CommandSymbol(nameof(ShowColumnPolicyEncoding),
                "show column <table_column>:ColumnName policy encoding",
                PolicyResult);

        public static readonly CommandSymbol AlterDatabasePolicyEncoding =
            new CommandSymbol(nameof(AlterDatabasePolicyEncoding),
                "alter database <database>:DatabaseName policy encoding <string>:EncodingPolicy",
                PolicyResult);

        public static readonly CommandSymbol AlterTablePolicyEncoding =
            new CommandSymbol(nameof(AlterTablePolicyEncoding),
                "alter table <database_table>:TableName policy encoding <string>:EncodingPolicy",
                PolicyResult);

        public static readonly CommandSymbol AlterColumnPolicyEncoding =
            new CommandSymbol(nameof(AlterColumnPolicyEncoding),
                "alter column <table_column>:ColumnName policy encoding <string>:EncodingPolicy",
                PolicyResult);

        public static readonly CommandSymbol AlterColumnPolicyEncodingType =
            new CommandSymbol(nameof(AlterColumnPolicyEncodingType),
                "alter column <table_column>:ColumnName policy encoding type '=' <string>:EncodingPolicyType",
                PolicyResult);

        public static readonly CommandSymbol AlterMergeDatabasePolicyEncoding =
            new CommandSymbol(nameof(AlterMergeDatabasePolicyEncoding),
                "alter-merge database <database>:DatabaseName policy encoding <string>:EncodingPolicy", 
                PolicyResult);

        public static readonly CommandSymbol AlterMergeTablePolicyEncoding =
            new CommandSymbol(nameof(AlterMergeTablePolicyEncoding), 
                "alter-merge table <database_table>:TableName policy encoding <string>:EncodingPolicy",
                PolicyResult);

        public static readonly CommandSymbol AlterMergeColumnPolicyEncoding =
            new CommandSymbol(nameof(AlterMergeColumnPolicyEncoding),
                "alter-merge column <table_column>:ColumnName policy encoding <string>:EncodingPolicy",
                PolicyResult);

        public static readonly CommandSymbol DeleteDatabasePolicyEncoding =
            new CommandSymbol(nameof(DeleteDatabasePolicyEncoding), 
                "delete database <database>:DatabaseName policy encoding",
                PolicyResult);

        public static readonly CommandSymbol DeleteTablePolicyEncoding =
            new CommandSymbol(nameof(DeleteTablePolicyEncoding),
                "delete table <database_table>:TableName policy encoding",
                PolicyResult);

        public static readonly CommandSymbol DeleteColumnPolicyEncoding =
            new CommandSymbol(nameof(DeleteColumnPolicyEncoding),
                "delete column <table_column>:ColumnName policy encoding",
                PolicyResult);
        #endregion

        #region Merge
        public static readonly CommandSymbol ShowDatabasePolicyMerge =
            new CommandSymbol(nameof(ShowDatabasePolicyMerge),
                "show database (<database> | '*'):DatabaseName policy merge",
                PolicyResult);

        public static readonly CommandSymbol ShowTablePolicyMerge =
            new CommandSymbol(nameof(ShowTablePolicyMerge),
                "show table (<database_table> | '*'):TableName policy merge",
                PolicyResult);

        public static readonly CommandSymbol AlterDatabasePolicyMerge =
            new CommandSymbol(nameof(AlterDatabasePolicyMerge),
                "alter database <database>:DatabaseName policy merge <string>:MergePolicy",
                PolicyResult);

        public static readonly CommandSymbol AlterTablePolicyMerge =
            new CommandSymbol(nameof(AlterTablePolicyMerge),
                "alter table <database_table>:TableName policy merge <string>:MergePolicy",
                PolicyResult);

        public static readonly CommandSymbol AlterMergeDatabasePolicyMerge =
            new CommandSymbol(nameof(AlterMergeDatabasePolicyMerge),
                "alter-merge database <database>:DatabaseName policy merge <string>:MergePolicy",
                PolicyResult);

        public static readonly CommandSymbol AlterMergeTablePolicyMerge =
            new CommandSymbol(nameof(AlterMergeTablePolicyMerge), 
                "alter-merge table <database_table>:TableName policy merge <string>:MergePolicy",
                PolicyResult);

        public static readonly CommandSymbol DeleteDatabasePolicyMerge =
            new CommandSymbol(nameof(DeleteDatabasePolicyMerge),
                "delete database <database>:DatabaseName policy merge",
                PolicyResult);

        public static readonly CommandSymbol DeleteTablePolicyMerge =
            new CommandSymbol(nameof(DeleteTablePolicyMerge),
                "delete table <database_table>:TableName policy merge",
                PolicyResult);
        #endregion

        #region Partitioning
        public static readonly CommandSymbol ShowTablePolicyPartitioning =
            new CommandSymbol(nameof(ShowTablePolicyPartitioning), 
                "show table (<database_table> | '*'):TableName policy partitioning",
                PolicyResult);

        public static readonly CommandSymbol AlterTablePolicyPartitioning =
            new CommandSymbol(nameof(AlterTablePolicyPartitioning),
                "alter table <table>:TableName policy partitioning <string>:Policy",
                PolicyResult);

        public static readonly CommandSymbol AlterMergeTablePolicyPartitioning =
            new CommandSymbol(nameof(AlterMergeTablePolicyPartitioning),
                "alter-merge table <table>:TableName policy partitioning <string>:Policy",
                PolicyResult);

        public static readonly CommandSymbol DeleteTablePolicyPartitioning =
            new CommandSymbol(nameof(DeleteTablePolicyPartitioning),
                "delete table <table>:TableName policy partitioning",
                PolicyResult);
        #endregion

        #region RestrictedViewAccess
        public static readonly CommandSymbol ShowTablePolicyRestrictedViewAccess =
            new CommandSymbol(nameof(ShowTablePolicyRestrictedViewAccess),
                "show table (<database_table> | '*'):TableName policy restricted_view_access",
                PolicyResult);

        public static readonly CommandSymbol AlterTablePolicyRestrictedViewAccess =
            new CommandSymbol(nameof(AlterTablePolicyRestrictedViewAccess),
                "alter table <database_table>:TableName policy restricted_view_access (true | false)",
                PolicyResult);

        public static readonly CommandSymbol AlterTablesPolicyRestrictedViewAccess =
            new CommandSymbol(nameof(AlterTablesPolicyRestrictedViewAccess),
                "alter tables '(' { <table>:TableName, ',' }+ ')' policy restricted_view_access (true | false)",
                PolicyResult);

        public static readonly CommandSymbol DeleteTablePolicyRestrictedViewAccess =
            new CommandSymbol(nameof(DeleteTablePolicyRestrictedViewAccess),
                "delete table <database_table>:TableName policy restricted_view_access",
                PolicyResult);
        #endregion

        #region RowStore
        public static readonly CommandSymbol ShowClusterPolicyRowStore =
            new CommandSymbol(nameof(ShowClusterPolicyRowStore), 
                "show cluster policy rowstore",
                PolicyResult);

        public static readonly CommandSymbol AlterClusterPolicyRowStore =
            new CommandSymbol(nameof(AlterClusterPolicyRowStore),
                "alter cluster policy rowstore <string>:RowStorePolicy",
                PolicyResult);

        public static readonly CommandSymbol AlterMergeClusterPolicyRowStore =
            new CommandSymbol(nameof(AlterMergeClusterPolicyRowStore),
                "alter-merge cluster policy! rowstore <string>:RowStorePolicy",
                PolicyResult);
        #endregion

        #region Sandbox
        public static readonly CommandSymbol ShowClusterPolicySandbox =
            new CommandSymbol(nameof(ShowClusterPolicySandbox),
                "show cluster policy sandbox",
                PolicyResult);

        public static readonly CommandSymbol AlterClusterPolicySandbox =
            new CommandSymbol(nameof(AlterClusterPolicySandbox),
                "alter cluster policy sandbox <string>:SandboxPolicy",
                PolicyResult);
        #endregion

        #region Sharding
        public static readonly CommandSymbol ShowDatabasePolicySharding =
            new CommandSymbol(nameof(ShowDatabasePolicySharding),
                "show database (<database> | '*'):DatabaseName policy sharding",
                PolicyResult);

        public static readonly CommandSymbol ShowTablePolicySharding =
            new CommandSymbol(nameof(ShowTablePolicySharding),
                "show table (<database_table> | '*'):TableName policy sharding",
                PolicyResult);

        public static readonly CommandSymbol AlterDatabasePolicySharding =
            new CommandSymbol(nameof(AlterDatabasePolicySharding),
                "alter database <database>:DatabaseName policy sharding <string>:ShardingPolicy",
                PolicyResult);

        public static readonly CommandSymbol AlterTablePolicySharding =
            new CommandSymbol(nameof(AlterTablePolicySharding),
                "alter table <database_table>:TableName policy sharding <string>:ShardingPolicy",
                PolicyResult);

        public static readonly CommandSymbol AlterMergeDatabasePolicySharding =
            new CommandSymbol(nameof(AlterMergeDatabasePolicySharding),
                "alter-merge database <database>:DatabaseName policy sharding <string>:ShardingPolicy",
                PolicyResult);

        public static readonly CommandSymbol AlterMergeTablePolicySharding =
            new CommandSymbol(nameof(AlterMergeTablePolicySharding),
                "alter-merge table <database_table>:TableName policy sharding <string>:ShardingPolicy",
                PolicyResult);

        public static readonly CommandSymbol DeleteDatabasePolicySharding =
            new CommandSymbol(nameof(DeleteDatabasePolicySharding),
                "delete database <database>:DatabaseName policy sharding",
                PolicyResult);

        public static readonly CommandSymbol DeleteTablePolicySharding =
            new CommandSymbol(nameof(DeleteTablePolicySharding),
                "delete table <database_table>:TableName policy sharding",
                PolicyResult);
        #endregion

        #region StreamingIngestion
        public static readonly CommandSymbol ShowDatabasePolicyStreamingIngestion =
            new CommandSymbol(nameof(ShowDatabasePolicyStreamingIngestion),
                "show database <database>:DatabaseName policy streamingingestion",
                PolicyResult);

        public static readonly CommandSymbol ShowTablePolicyStreamingIngestion =
            new CommandSymbol(nameof(ShowTablePolicyStreamingIngestion),
                "show table <database_table>:TableName policy streamingingestion",
                PolicyResult);

        public static readonly CommandSymbol ShowClusterPolicyStreamingIngestion =
            new CommandSymbol(nameof(ShowClusterPolicyStreamingIngestion),
                "show cluster policy streamingingestion",
                PolicyResult);

        public static readonly CommandSymbol AlterDatabasePolicyStreamingIngestion =
            new CommandSymbol(nameof(AlterDatabasePolicyStreamingIngestion),
                "alter database <database>:DatabaseName policy streamingingestion <string>:StreamingIngestionPolicy",
                PolicyResult);

        public static readonly CommandSymbol AlterTablePolicyStreamingIngestion =
            new CommandSymbol(nameof(AlterTablePolicyStreamingIngestion),
                "alter table <database_table>:TableName policy streamingingestion <string>:StreamingIngestionPolicy",
                PolicyResult);

        public static readonly CommandSymbol AlterClusterPolicyStreamingIngestion =
            new CommandSymbol(nameof(AlterClusterPolicyStreamingIngestion),
                "alter cluster policy streamingingestion <string>:StreamingIngestionPolicy",
                PolicyResult);

        public static readonly CommandSymbol DeleteDatabasePolicyStreamingIngestion =
            new CommandSymbol(nameof(DeleteDatabasePolicyStreamingIngestion),
                "delete database <database>:DatabaseName policy streamingingestion",
                PolicyResult);

        public static readonly CommandSymbol DeleteTablePolicyStreamingIngestion =
            new CommandSymbol(nameof(DeleteTablePolicyStreamingIngestion),
                "delete table <database_table>:TableName policy streamingingestion",
                PolicyResult);

        public static readonly CommandSymbol DeleteClusterPolicyStreamingIngestion =
            new CommandSymbol(nameof(DeleteClusterPolicyStreamingIngestion),
                "delete cluster policy streamingingestion",
                PolicyResult);

        #endregion

        #region Callout
        public static readonly CommandSymbol ShowClusterPolicyCallout =
            new CommandSymbol(nameof(ShowClusterPolicyCallout),
                "show cluster policy callout",
                PolicyResult);

        public static readonly CommandSymbol AlterClusterPolicyCallout =
            new CommandSymbol(nameof(AlterClusterPolicyCallout),
                "alter cluster policy callout <string>:Policy",
                PolicyResult);

        public static readonly CommandSymbol AlterMergeClusterPolicyCallout =
            new CommandSymbol(nameof(AlterMergeClusterPolicyCallout), 
                "alter-merge cluster policy callout <string>:Policy",
                PolicyResult);

        public static readonly CommandSymbol DeleteClusterPolicyCallout =
            new CommandSymbol(nameof(DeleteClusterPolicyCallout),
                "delete cluster policy callout", 
                PolicyResult);
        #endregion

        #region Capacity
        public static readonly CommandSymbol ShowClusterPolicyCapacity =
            new CommandSymbol(nameof(ShowClusterPolicyCapacity),
                "show cluster policy capacity", 
                PolicyResult);

        public static readonly CommandSymbol AlterClusterPolicyCapacity =
            new CommandSymbol(nameof(AlterClusterPolicyCapacity), 
                "alter cluster policy capacity <string>:Policy",
                PolicyResult);

        public static readonly CommandSymbol AlterMergeClusterPolicyCapacity =
            new CommandSymbol(nameof(AlterMergeClusterPolicyCapacity),
                "alter-merge cluster policy capacity <string>:Policy",
                PolicyResult);
        #endregion

        #region Query Throttling
        public static readonly CommandSymbol ShowClusterPolicyQueryThrottling =
            new CommandSymbol(nameof(ShowClusterPolicyQueryThrottling),
                "show cluster policy querythrottling", 
                PolicyResult);

        public static readonly CommandSymbol AlterClusterPolicyQueryThrottling =
            new CommandSymbol(nameof(AlterClusterPolicyQueryThrottling),
                "alter cluster policy querythrottling <string>:Policy",
                PolicyResult);

        public static readonly CommandSymbol DeleteClusterPolicyQueryThrottling =
            new CommandSymbol(nameof(DeleteClusterPolicyQueryThrottling),
                "delete cluster policy querythrottling", 
                PolicyResult);
        #endregion

        #region Query Limit
        public static readonly CommandSymbol ShowClusterPolicyQueryLimit =
            new CommandSymbol(nameof(ShowClusterPolicyQueryLimit),
                "show cluster policy querylimit", 
                PolicyResult);

        public static readonly CommandSymbol AlterClusterPolicyQueryLimit =
            new CommandSymbol(nameof(AlterClusterPolicyQueryLimit),
                "alter cluster policy querylimit <string>:Policy",
                PolicyResult);
        #endregion

        #region Multi Database Admins
        public static readonly CommandSymbol ShowClusterPolicyMultiDatabaseAdmins =
            new CommandSymbol(nameof(ShowClusterPolicyMultiDatabaseAdmins),
                "show cluster policy multidatabaseadmins", 
                PolicyResult);

        public static readonly CommandSymbol AlterClusterPolicyMultiDatabaseAdmins =
            new CommandSymbol(nameof(AlterClusterPolicyMultiDatabaseAdmins),
                "alter cluster policy multidatabaseadmins <string>:Policy",
                PolicyResult);

        public static readonly CommandSymbol AlterMergeClusterPolicyMultiDatabaseAdmins =
            new CommandSymbol(nameof(AlterMergeClusterPolicyMultiDatabaseAdmins),
                "alter-merge cluster policy multidatabaseadmins <string>:Policy",
                PolicyResult);

        #endregion

        #endregion

        #region Security Role Commands
        public static readonly CommandSymbol ShowPrincipalRoles =
            new CommandSymbol(nameof(ShowPrincipalRoles),
                "show principal [<string>:Principal] roles",
                "(Scope: string, DisplayName: string, AADObjectID: string, Role: string)");

        private static readonly string ShowPrincipalsResult =
            "(Role: string, PrincipalType: string, PrincipalDisplayName: string, PrincipalObjectId: string, PrincipalFQN: string, Notes: string, RoleAssignmentIdentifier: string)";

        public static readonly CommandSymbol ShowClusterPrincipals =
            new CommandSymbol(nameof(ShowClusterPrincipals),
                "show cluster principals",
                ShowPrincipalsResult);

        public static readonly CommandSymbol ShowDatabasePrincipals =
            new CommandSymbol(nameof(ShowDatabasePrincipals),
                "show database <database>:DatabaseName principals",
                ShowPrincipalsResult);

        public static readonly CommandSymbol ShowTablePrincipals =
            new CommandSymbol(nameof(ShowTablePrincipals),
                "show table <table>:TableName principals",
                ShowPrincipalsResult);

        public static readonly CommandSymbol ShowFunctionPrincipals =
            new CommandSymbol(nameof(ShowFunctionPrincipals), 
                "show function <function>:FunctionName principals",
                ShowPrincipalsResult);

        private static string ClusterRole = "(admins | databasecreators | users | viewers):Role";
        private static string DatabaseRole = "(admins | ingestors | monitors | unrestrictedviewers | users | viewers):Role";
        private static string TableRole = "(admins | ingestors):Role";
        private static string FunctionRole = "admins:Role";
        private static string PrincipalsClause = "'(' { <string>:Principal, ',' }+ ')' [skip-results:SkipResults] [<string>:Notes]";
        private static string PrincipalsOrNoneClause = $"(none [skip-results:SkipResults] | {PrincipalsClause})";

        public static readonly CommandSymbol AddClusterRole =
            new CommandSymbol(nameof(AddClusterRole),
                $"add cluster {ClusterRole} {PrincipalsClause}",
                ShowPrincipalsResult);

        public static readonly CommandSymbol DropClusterRole =
            new CommandSymbol(nameof(DropClusterRole),
                $"drop cluster {ClusterRole} {PrincipalsClause}",
                ShowPrincipalsResult);

        public static readonly CommandSymbol SetClusterRole =
            new CommandSymbol(nameof(SetClusterRole),
                $"set cluster {ClusterRole} {PrincipalsOrNoneClause}",
                ShowPrincipalsResult);

        public static readonly CommandSymbol AddDatabaseRole =
            new CommandSymbol(nameof(AddDatabaseRole),
                $"add database <database>:DatabaseName {DatabaseRole} {PrincipalsClause}",
                ShowPrincipalsResult);

        public static readonly CommandSymbol DropDatabaseRole =
            new CommandSymbol(nameof(DropDatabaseRole),
                $"drop database <database>:DatabaseName {DatabaseRole} {PrincipalsClause}",
                ShowPrincipalsResult);

        public static readonly CommandSymbol SetDatabaseRole =
            new CommandSymbol(nameof(SetDatabaseRole),
                $"set database <database>:DatabaseName {DatabaseRole} {PrincipalsOrNoneClause}",
                ShowPrincipalsResult);

        public static readonly CommandSymbol AddTableRole =
            new CommandSymbol(nameof(AddTableRole),
                $"add table <table>:TableName {TableRole} {PrincipalsClause}",
                ShowPrincipalsResult);

        public static readonly CommandSymbol DropTableRole =
            new CommandSymbol(nameof(DropTableRole),
                $"drop table <table>:TableName {TableRole} {PrincipalsClause}",
                ShowPrincipalsResult);

        public static readonly CommandSymbol SetTableRole =
            new CommandSymbol(nameof(SetTableRole),
                $"set table <table>:TableName {TableRole} {PrincipalsOrNoneClause}",
                ShowPrincipalsResult);

        public static readonly CommandSymbol AddFunctionRole =
            new CommandSymbol(nameof(AddFunctionRole),
                $"add function <function>:FunctionName {FunctionRole} {PrincipalsClause}",
                ShowPrincipalsResult);

        public static readonly CommandSymbol DropFunctionRole =
            new CommandSymbol(nameof(DropFunctionRole),
                $"drop function <function>:FunctionName {FunctionRole} {PrincipalsClause}",
                ShowPrincipalsResult);

        public static readonly CommandSymbol SetFunctionRole =
            new CommandSymbol(nameof(SetFunctionRole),
                $"set function <function>:FunctionName {FunctionRole} {PrincipalsOrNoneClause}",
                ShowPrincipalsResult);

        private static readonly string BlockedPrincipalsResult =
            "(PrincipalType: string, PrincipalDisplayName: string, PrincipalObjectId: string, PrincipalFQN: string, Application: string, User: string, BlockedUntil: datetime, Reason: string)";

        public static readonly CommandSymbol ShowClusterBlockedPrincipals =
            new CommandSymbol(nameof(ShowClusterBlockedPrincipals),
                "show cluster blockedprincipals",
                BlockedPrincipalsResult);

        public static readonly CommandSymbol AddClusterBlockedPrincipals =
            new CommandSymbol(nameof(AddClusterBlockedPrincipals),
                "add cluster blockedprincipals <string>:Principal [application <string>:AppName] [user <string>:UserName] [period <timespan>:Period] [reason <string>:Reason]",
                BlockedPrincipalsResult);

        public static readonly CommandSymbol DropClusterBlockedPrincipals =
            new CommandSymbol(nameof(DropClusterBlockedPrincipals),
                "drop cluster blockedprincipals <string>:Principal [application <string>:AppName] [user <string>:UserName]",
                BlockedPrincipalsResult);
        #endregion

        #region Data Ingestion
        private static readonly string SourceDataLocatorList = "'(' { <string>:SourceDataLocator, ',' }+ ')'";
        private static readonly string PropertyList = "with '('! { <name>:PropertyName '='! <value>:PropertyValue, ',' }+ ')'";

        public static readonly CommandSymbol IngestIntoTable =
            new CommandSymbol(nameof(IngestIntoTable),
                $"ingest [async] into table! <table>:TableName {SourceDataLocatorList} [{PropertyList}]",
                "(ExtentId: guid, ItemLoaded: string, Duration: string, HasErrors: string, OperationId: guid)");

        public static readonly CommandSymbol IngestInlineIntoTable =
            new CommandSymbol(nameof(IngestInlineIntoTable),
                $"ingest inline into! table <name>:TableName ('[' <bracketed_input_data>:Data ']' | {PropertyList} '<|'! <input_data>:Data | '<|' <input_data>:Data)",
                "(ExtentId: guid)");

        private static readonly string DataIngestionSetAppendResult =
            "(ExtentId: guid, OriginalSize: long, ExtentSize: long, ColumnSize: long, IndexSize: long, RowCount: long)";

        public static readonly CommandSymbol SetTable =
            new CommandSymbol(nameof(SetTable), 
                $"set [async] <name>:TableName [{PropertyList}] '<|' <input_query>:QueryOrCommand", 
                DataIngestionSetAppendResult);

        public static readonly CommandSymbol AppendTable =
            new CommandSymbol(nameof(AppendTable), 
                $"append [async] <table>:TableName [{PropertyList}] '<|' <input_query>:QueryOrCommand", 
                DataIngestionSetAppendResult);

        public static readonly CommandSymbol SetOrAppendTable =
            new CommandSymbol(nameof(SetOrAppendTable), 
                $"set-or-append [async] <name>:TableName [{PropertyList}] '<|' <input_query>:QueryOrCommand", 
                DataIngestionSetAppendResult);

        public static readonly CommandSymbol SetOrReplaceTable =
            new CommandSymbol(nameof(SetOrReplaceTable), 
                $"set-or-replace [async] <name>:TableName [{PropertyList}] '<|' <input_query>:QueryOrCommand",
                DataIngestionSetAppendResult);
        #endregion

        #region Data Export
        private static string DataConnectionStringList = "'(' { <string>:DataConnectionString, ',' }+ ')'";

        public static readonly CommandSymbol ExportToStorage =
            new CommandSymbol(nameof(ExportToStorage),
                $"export [async] [compressed] to (csv|tsv|json|parquet) {DataConnectionStringList} [{PropertyList}] '<|' <input_query>:Query",
                UnknownResult);

        public static readonly CommandSymbol ExportToSqlTable =
            new CommandSymbol(nameof(ExportToSqlTable),
                $"export [async] to sql <name>:SqlTableName <string>:SqlConnectionString [{PropertyList}] '<|' <input_query>:Query",
                UnknownResult);

        public static readonly CommandSymbol ExportToExternalTable =
            new CommandSymbol(nameof(ExportToExternalTable), 
                $"export [async] to table <name>:ExternalTableName [{PropertyList}] '<|' <input_query>:Query",
                UnknownResult);

        private static readonly string OverClause = "over '('! { <name>:TableName, ',' }+ ')'";

        public static readonly CommandSymbol CreateOrAlterContinuousExport =
            new CommandSymbol(nameof(CreateOrAlterContinuousExport), 
                $"create-or-alter continuous-export <name>:ContinuousExportName [{OverClause}] to table <name>:ExternalTableName [{PropertyList}] '<|' <input_query>:Query",
                UnknownResult);

        private static readonly string ShowContinuousExportResult =
            "(Name: string, ExternalTableName: string, Query: string, " +
            "ForcedLatency: timespan, IntervalBetweenRuns: timespan, CursorScopedTables: string, ExportProperties: string, " +
            "LastRunTime: datetime, StartCursor: string, IsDisabled: bool, LastRunResult: string, ExportedTo: datetime, IsRunning: bool)";

        public static readonly CommandSymbol ShowContinuousExport =
            new CommandSymbol(nameof(ShowContinuousExport), 
                "show continuous-export <name>:ContinuousExportName", 
                ShowContinuousExportResult);

        public static readonly CommandSymbol ShowContinuousExports =
            new CommandSymbol(nameof(ShowContinuousExports), 
                "show continuous-exports", 
                ShowContinuousExportResult);

        public static readonly CommandSymbol ShowContinuousExportExportedArtifacts =
            new CommandSymbol(nameof(ShowContinuousExportExportedArtifacts),
                "show continuous-export <name>:ContinuousExportName exported-artifacts",
                "(Timestamp: datetime, ExternalTableName: string, Path: string, NumRecords: long)");

        public static readonly CommandSymbol ShowContinuousExportFailures =
            new CommandSymbol(nameof(ShowContinuousExportFailures),
                "show continuous-export <name>:ContinuousExportName failures",
                "(Timestamp: datetime, OperationId: string, Name: string, LastSuccessRun: datetime, FailureKind: string, Details: string)");

        public static readonly CommandSymbol DropContinuousExport =
            new CommandSymbol(nameof(DropContinuousExport), 
                "drop continuous-export <name>:ContinousExportName",
                ShowContinuousExportResult);

        public static readonly CommandSymbol EnableContinuousExport =
            new CommandSymbol(nameof(EnableContinuousExport),
                "enable continuous-export <name>:ContinousExportName", 
                ShowContinuousExportResult);

        public static readonly CommandSymbol DisableContinuousExport =
            new CommandSymbol(nameof(DisableContinuousExport),
                "disable continuous-export <name>:ContinousExportName", 
                ShowContinuousExportResult);

        #endregion

        #region System Information Commands
        public static readonly CommandSymbol ShowCluster =
            new CommandSymbol(nameof(ShowCluster),
                "show cluster",
                "(NodeId: string, Address: string, Name: string, StartTime: string, IsAdmin: bool, " + 
                    "MachineTotalMemory: long, MachineAvailableMemory: long, ProcessorCount: int, EnvironmentDescription: string)");

        public static readonly CommandSymbol ShowDiagnostics =
            new CommandSymbol(nameof(ShowDiagnostics),
                "show diagnostics",
                new TableSymbol(
                    new ColumnSymbol("IsHealthy", ScalarTypes.Bool),
                    new ColumnSymbol("EnvironmentDescription", ScalarTypes.String),
                    new ColumnSymbol("IsScaleOutRequired", ScalarTypes.Bool),
                    new ColumnSymbol("MachinesTotal", ScalarTypes.Int),
                    new ColumnSymbol("MachinesOffline", ScalarTypes.Int),
                    new ColumnSymbol("NodeLastRestartedOn", ScalarTypes.DateTime),
                    new ColumnSymbol("AdminLastElectedOn", ScalarTypes.DateTime),
                    // snew ColumnSymbol("ReservedSlot2", ScalarTypes.Real),
                    new ColumnSymbol("ExtentsTotal", ScalarTypes.Int),
                    new ColumnSymbol("DiskColdAllocationPercentage", ScalarTypes.Int),
                    new ColumnSymbol("InstancesTargetBasedOnDataCapacity", ScalarTypes.Int),
                    new ColumnSymbol("TotalOriginalDataSize", ScalarTypes.Real),
                    new ColumnSymbol("TotalExtentSize", ScalarTypes.Real),
                    new ColumnSymbol("IngestionsLoadFactor", ScalarTypes.Real),
                    new ColumnSymbol("IngestionsInProgress", ScalarTypes.Long),
                    new ColumnSymbol("IngestionsSuccessRate", ScalarTypes.Real),
                    new ColumnSymbol("MergesInProgress", ScalarTypes.Long),
                    new ColumnSymbol("BuildVersion", ScalarTypes.String),
                    new ColumnSymbol("BuildTime", ScalarTypes.DateTime),
                    new ColumnSymbol("ClusterDataCapacityFactor", ScalarTypes.Real),
                    new ColumnSymbol("IsDataWarmingRequired", ScalarTypes.Bool),
                    // new ColumnSymbol("ReservedSlot3", ScalarTypes.String),
                    new ColumnSymbol("DataWarmingLastRunOn", ScalarTypes.DateTime),
                    new ColumnSymbol("MergesSuccessRate", ScalarTypes.Real),
                    new ColumnSymbol("NotHealthyReason", ScalarTypes.String),
                    new ColumnSymbol("IsAttentionRequired", ScalarTypes.Bool),
                    new ColumnSymbol("AttentionRequiredReason", ScalarTypes.String),
                    new ColumnSymbol("ProductVersion", ScalarTypes.String),
                    new ColumnSymbol("FailedIngestOperations", ScalarTypes.Int),
                    new ColumnSymbol("FailedMergeOperations", ScalarTypes.Int),
                    new ColumnSymbol("MaxExtentsInSingleTable", ScalarTypes.Int),
                    new ColumnSymbol("TableWithMaxExtents", ScalarTypes.String),
                    new ColumnSymbol("WarmExtentSize", ScalarTypes.Real),
                    new ColumnSymbol("NumberOfDatabases", ScalarTypes.Int),
                    new ColumnSymbol("PurgeExtentsRebuildLoadFactor", ScalarTypes.Real),
                    new ColumnSymbol("PurgeExtentsRebuildInProgress", ScalarTypes.Long),
                    new ColumnSymbol("PurgesInProgress", ScalarTypes.Long),
                    new ColumnSymbol("MaxSoftRetentionPolicyViolation", ScalarTypes.TimeSpan),
                    // new ColumnSymbol("ReservedSlot4", ScalarTypes.String),
                    new ColumnSymbol("RowStoreLocalStorageCapacityFactor", ScalarTypes.Real),
                    new ColumnSymbol("ExportsLoadFactor", ScalarTypes.Real),
                    new ColumnSymbol("ExportsInProgress", ScalarTypes.Long),
                    new ColumnSymbol("PendingContinuousExports", ScalarTypes.Long),
                    new ColumnSymbol("MaxContinuousExportLatenessMinutes", ScalarTypes.Long),
                    new ColumnSymbol("RowStoreSealsInProgress", ScalarTypes.Long),
                    new ColumnSymbol("IsRowStoreUnhealthy", ScalarTypes.Bool),
                    new ColumnSymbol("MachinesSuspended", ScalarTypes.Int),
                    new ColumnSymbol("DataPartitioningLoadFactor", ScalarTypes.Real),
                    new ColumnSymbol("DataPartitioningOperationsInProgress", ScalarTypes.Long),
                    new ColumnSymbol("MinPartitioningPercentageInSingleTable", ScalarTypes.Real),
                    new ColumnSymbol("TableWithMinPartitioningPercentage", ScalarTypes.String)));

        public static readonly CommandSymbol ShowCapacity =
            new CommandSymbol(nameof(ShowCapacity),
                "show capacity",
                "(Resource: string, Total: long, Consumed: long, Remaining: long)");

        public static readonly CommandSymbol ShowOperations =
            new CommandSymbol(nameof(ShowOperations),
                "show operations [(<guid>:OperationId | '(' { <guid>:OperationId, ',' }+ ')')]",
                "(OperationId: guid, Operation: string, NodeId: string, StartedOn: datetime, LastUpdatedOn: datetime: Duration: timespan, State: string, Status: string, RootActivityId: guid, ShouldRetry: bool, Database: string, Principal: string, User: string, AdminEpochStartTime: datetime)");

        public static readonly CommandSymbol ShowOperationDetails =
            new CommandSymbol(nameof(ShowOperationDetails),
                "show operation <guid>:OperationId details",
                UnknownResult); // schema depends on operation

        private static readonly string JournalResult =
            "(Event: string, EventTimestamp: datetime, Database: string, EntityName: string, UpdatedEntityName: string, EntityVersion: string, EntityContainerName: string, " +
            "OriginalEntityState: string, UpdatedEntityState: string, ChangeCommand: string, Principal: string, RootActivityId: guid, ClientRequestString: string, " + 
            "User: string, OriginalEntityVersion: string)";

        public static readonly CommandSymbol ShowJournal =
            new CommandSymbol(nameof(ShowJournal),
                "show journal", 
                JournalResult);

        public static readonly CommandSymbol ShowDatabaseJournal =
            new CommandSymbol(nameof(ShowDatabaseJournal), 
                "show database <database>:DatabaseName journal", 
                JournalResult);

        public static readonly CommandSymbol ShowClusterJournal =
            new CommandSymbol(nameof(ShowClusterJournal),
                "show cluster journal", 
                JournalResult);

        private static readonly string QueryResults =
            "(ClientActivityId: string, Text: string, Database: string, StartedOn: datetime, LastUpdatedOn: datetime, Durantion: timespan, " +
            "State: string, RootActivityId: guid, User: string, FailureReason: string, TotalCpu: timespan, CacheStatistics: dynamic, " + 
            "Application: string, MemoryPeak: long, ScannedEventStatistics: dynamic, Pricipal: string, ClientRequestProperties: dynamic, ResultSetStatistics: dynamic)";

        public static readonly CommandSymbol ShowQueries =
            new CommandSymbol(nameof(ShowQueries),
                "show queries",
                "(ClientActivityId: string, Text: string, Database: string, StartedOn: datetime, LastUpdatedOn: datetime, Duration: timespan, State: string, RootActivityId: guid, User: string, FailureReason: string, TotalCpu: timespan, CacheStatistics: dynamic, Application: string, MemoryPeak: long, ScannedExtentsStatistics: dynamic, Principal: string, ClientRequestProperties: dynamic, ResultSetStatistics: dynamic)");

        public static readonly CommandSymbol ShowRunningQueries =
            new CommandSymbol(nameof(ShowRunningQueries), 
                "show running queries [by (user <string>:UserName | '*')]",
                QueryResults);

        public static readonly CommandSymbol CancelQuery =
            new CommandSymbol(nameof(CancelQuery),
                "cancel query <string>:ClientRequestId",
                UnknownResult);

        public static readonly CommandSymbol ShowQueryPlan =
            new CommandSymbol(nameof(ShowQueryPlan),
                "show queryplan '<|' <input_query>:Query",
                "(ResultType: string, RelopTree: string, QueryPlan: string, Stat: string)");

        public static readonly CommandSymbol ShowBasicAuthUsers =
            new CommandSymbol(nameof(ShowBasicAuthUsers),
                "show basicauth users",
                "(UserName: string)");

        public static readonly CommandSymbol ShowCache =
            new CommandSymbol(nameof(ShowCache),
                "show cache",
                "(NodeId: string, TotalMemoryCapacity: long, MemoryCacheCapacity: long, MemoryCacheInUse: long, MemoryCacheHitCount: long, " +
                "TotalDiskCapacity: long, DiskCacheCapacity: long, DiskCacheInUse: long, DiskCacheHitCount: long, DiskCacheMissCount: long, " +
                "MemoryCacheDetails: string, DiskCacheDetails: string)");

        public static readonly CommandSymbol ShowCommands =
            new CommandSymbol(nameof(ShowCommands),
                "show commands",
                "(ClientActivityId: string, CommandType: string, Text: string, Database: string, " + 
                "StartedOn: datetime, LastUpdatedOn: datetime, Duration: timespan, State: string, RootActivityId: guid, " + 
                "User: string, FailureReason: string, Application: string, Principal: string, TotalCpu: timespan, " + 
                "ResourceUtilization: dynamic, ClientRequestProperties: dynamic)");

        public static readonly CommandSymbol ShowCommandsAndQueries =
            new CommandSymbol(nameof(ShowCommandsAndQueries),
                "show commands-and-queries",
                "(ClientActivityId: string, CommandType: string, Text: string, Database: string, " +
                "StartedOn: datetime, LastUpdatedOn: datetime, Duration: timespan, State: string, RootActivityId: guid, " +
                "User: string, Application: string, Principal: string, ClientRequestProperties: dynamic, " +
                "TotalCpu: timespan, MemoryPeak: long, CacheStatistics: dynamic, ScannedExtentStatistics: dynamic, ResultSetStatistics: dynamic)");

        public static readonly CommandSymbol ShowIngestionFailures =
            new CommandSymbol(nameof(ShowIngestionFailures),
                "show ingestion failures [with '(' OperationId '=' <guid>:OperationId ')']",
                "(OperationId: guid, Database: string, Table: string, FailedOn: datetime, IngestionSourcePath: string, Details: string, FailureKind: string, RootActivityId: guid, OperationKind: string, OriginatesFromUpdatePolicy: bool, ErrorCode: string, Principal: string, ShouldRetry: bool, User: string, IngestionProperties: string)");
        #endregion

        #region Advanced Commands

        public static readonly CommandSymbol ShowClusterExtents =
            new CommandSymbol(nameof(ShowClusterExtents), 
                "show cluster extents [hot]",
                "(ExtentId: guid, DatabaseName: string, TableName: string, MaxCreatedOn: datetime, OriginalSize: real, ExtentSize: real, CompressedSize: real, IndexSize: real, Blocks: long, Segments: long, AssignedDataNodes: string, LoadedDataNodes: string, ExtentContainerId: string, RowCount: long, MinCreatedOn: datetime, Tags: string, Kind: string, Partition: string)");

        private static readonly string ExtentIdList = "'(' {<guid>:ExtentId, ','}+ ')'";
        private static readonly string TagWhereClause = "where { tags (has | contains | '!has' | '!contains')! <string>:Tag, and }+";

        public static readonly CommandSymbol ShowDatabaseExtents =
            new CommandSymbol(nameof(ShowDatabaseExtents),
                $"show database <database>:DatabaseName extents [{ExtentIdList}] [hot] [{TagWhereClause}]",
                "(ExtentId: guid, DatabaseName: string, TableName: string, MaxCreatedOn: datetime, OriginalSize: real, ExtentSize: real, CompressedSize: real, IndexSize: real, Blocks: long, Segments: long, AssignedDataNodes: string, LoadedDataNodes: string, ExtentContainerId: string, RowCount: long, MinCreatedOn: datetime, Tags: string, Kind: string, Partition: string)");

        public static readonly CommandSymbol ShowTableExtents =
            new CommandSymbol(nameof(ShowTableExtents),
                $"show table <table>:TableName extents [{ExtentIdList}] [hot] [{TagWhereClause}]",
                "(ExtentId: guid, DatabaseName: string, TableName: string, MaxCreatedOn: datetime, OriginalSize: real, ExtentSize: real, CompressedSize: real, IndexSize: real, Blocks: long, Segments: long, AssignedDataNodes: string, LoadedDataNodes: string, ExtentContainerId: string, RowCount: long, MinCreatedOn: datetime, Tags: string, Kind: string, Partition: string)");

        public static readonly CommandSymbol ShowTablesExtents =
            new CommandSymbol(nameof(ShowTablesExtents),
                $"show tables {TableNameList} extents [{ExtentIdList}] [hot] [{TagWhereClause}]",
                "(ExtentId: guid, DatabaseName: string, TableName: string, MaxCreatedOn: datetime, OriginalSize: real, ExtentSize: real, CompressedSize: real, IndexSize: real, Blocks: long, Segments: long, AssignedDataNodes: string, LoadedDataNodes: string, ExtentContainerId: string, RowCount: long, MinCreatedOn: datetime, Tags: string, Kind: string, Partition: string)");

        private static readonly string MergeExtentsResult =
            "(OriginalExtentId: string, ResultExtentId: string, Duration: timespan)";

        private static readonly string GuidList = "'(' {<guid>:GUID, ','}+ ')'";
        public static readonly CommandSymbol MergeExtents =
            new CommandSymbol(nameof(MergeExtents), 
                $"merge [async] <table>:TableName {GuidList} [with '(' rebuild '=' true ')']", 
                MergeExtentsResult);

        public static readonly CommandSymbol MergeExtentsDryrun =
            new CommandSymbol(nameof(MergeExtentsDryrun), 
                $"merge dryrun <table>:TableName {GuidList}", 
                MergeExtentsResult);

        private static readonly string MoveExtentsResult =
            "(OriginalExtentId: string, ResultExtentId: string, Details: string)";

        public static readonly CommandSymbol MoveExtentsFrom =
            new CommandSymbol(nameof(MoveExtentsFrom), 
                $"move [async] extents (all | {GuidList}) from table <table>:SourceTableName to table <table>:DestinationTableName",
                MoveExtentsResult);

        public static readonly CommandSymbol MoveExtentsQuery =
            new CommandSymbol(nameof(MoveExtentsQuery),
                $"move [async] extents to table <table>:DestinationTableName '<|' <input_query>:Query",
                MoveExtentsResult);

        private static readonly string DropExtentResult =
            "(ExtentId: guid, TableName: string, CreatedOn: datetime)";

        //public static readonly CommandSymbol DropExtentsQuery =
        //    new CommandSymbol("drop extents query", "drop extents [whatif] '<|' <input_query>:Query", DropExtentResult);

        public static readonly CommandSymbol DropExtent =
            new CommandSymbol(nameof(DropExtent), 
                "drop extent <guid>:ExtentId [from <table>:TableName]", 
                DropExtentResult);

        private static readonly string DropProperties = "[older <long>:Older (days | hours)] from (all tables! | <table>:TableName) [trim by! (extentsize | datasize) <long>:TrimSize (MB | GB | bytes)] [limit <long>:LimitCount]";

        public static readonly CommandSymbol DropExtents =
            new CommandSymbol(nameof(DropExtents),
                @"drop extents 
                    ('(' { <guid>:ExtentId, ',' }+ ')' [from <table>:TableName]
                     | whatif '<|'! <input_query>:Query
                     | '<|' <input_query>:Query
                     | older <long>:Older (days | hours) from (all tables! | <table>:TableName) [trim by! (extentsize | datasize)! <long>:TrimSize (MB | GB | bytes)] [limit <long>:LimitCount]
                     | from (all tables! | <table>:TableName) [trim by! (extentsize | datasize) <long>:TrimSize (MB | GB | bytes)] [limit <long>:LimitCount]
                     )", 
                DropExtentResult);

        public static readonly CommandSymbol DropPretendExtentsByProperties =
            new CommandSymbol(nameof(DropPretendExtentsByProperties), 
                $"drop-pretend extents {DropProperties}", 
                DropExtentResult);

        public static readonly CommandSymbol ShowVersion =
            new CommandSymbol(nameof(ShowVersion),
                "show version",
                "(BuildVersion: string, BuildTime: datetime, ServiceType: string, ProductVersion: string)");

        public static readonly CommandSymbol ClearTableData =
           new CommandSymbol(nameof(ClearTableData),
               "clear [async] table <table>:TableName data",
               "(Status: string)");

        public static readonly CommandSymbol ClearTableCacheStreamingIngestionSchema =
           new CommandSymbol(nameof(ClearTableCacheStreamingIngestionSchema), 
               "clear table <table>:TableName cache streamingingestion schema",
               "(NodeId: string, Status: string)");

        #endregion

        public static IReadOnlyList<CommandSymbol> All { get; } =
            new CommandSymbol[]
            {
                #region Schema Commands
                // Databases
                ShowDatabase,
                ShowDatabaseDetails,
                ShowDatabaseIdentity,
                ShowDatabasePolicies,
                ShowClusterDatabases,
                ShowClusterDatabasesDetails,
                ShowClusterDatabasesIdentity,
                ShowClusterDatabasesPolicies,
                ShowClusterDatabasesDataStats,
                CreateDatabasePersist,
                CreateDatabaseVolatile,
                AttachDatabase,
                DetachDatabase,
                AlterDatabasePrettyName,
                DropDatabasePrettyName,
                AlterDatabasePersistMetadata,
                SetAccess,
                ShowDatabaseSchema,
                ShowDatabaseSchemaAsJson,
                ShowDatabasesSchema,
                ShowDatabasesSchemaAsJson,

                // Tables
                ShowTables,
                ShowTable,
                ShowTablesDetails,
                ShowTableDetails,
                ShowTableCslSchema,
                ShowTableSchemaAsJson,
                CreateTable,
                CreateMergeTable,
                CreateTables,
                AlterTable,
                AlterMergeTable,
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
                AlterTableDocString,
                AlterTableFolder,

                // Columns
                RenameColumn,
                RenameColumns,
                AlterColumnType,
                DropColumn,
                DropTableColumns,
                AlterTableColumnDocStrings,
                AlterMergeTableColumnDocStrings,

                // Functions
                ShowFunctions,
                ShowFunction,
                DropFunction,
                AlterFunctionDocString,
                AlterFunctionFolder,
                CreateFunction,
                AlterFunction,
                CreateOrAlterFunction,

                // External Tables
                ShowExternalTables,
                ShowExternalTable,
                ShowExternalTableCslSchema,
                ShowExternalTableSchema,
                ShowExternalTableArtifacts,
                DropExternalTable,
                CreateExternalTable,
                AlterExternalTable,
                CreateExternalTableMapping,
                AlterExternalTableMapping,
                ShowExternalTableMapping,
                ShowExternalTableMappings,
                DropExternalTableMapping,
                #endregion

                #region Policy Commands
                // Caching
                ShowDatabasePolicyCaching,
                AlterDatabasePolicyCaching,
                ShowTablePolicyCaching,
                AlterTablePolicyCaching,
                AlterClusterPolicyCaching,
                DeleteTablePolicyCaching,

                // IngestionTime
                AlterTablePolicyIngestionTime,
                AlterTablesPolicyIngestionTime,
                ShowTablePolicyIngestionTime,
                DeleteTablePolicyIngestionTime,

                // RowLevelSecurity
                ShowTablePolicyRowLevelSecurity,
                AlterTablePolicyRowLevelSecurity,
                DeleteTablePolicyRowLevelSecurity,

                // Retention
                ShowTablePolicyRetention,
                ShowDatabasePolicyRetention,
                DeleteTablePolicyRetention,
                DeleteDatabasePolicyRetention,
                AlterTablePolicyRetention,
                AlterTablesPolicyRetention,
                AlterDatabasePolicyRetention,
                AlterMergeTablePolicyRetention,
                AlterMergeDatabasePolicyRetention,

                // RowOrder
                ShowTablePolicyRowOrder,
                DeleteTablePolicyRowOrder,
                AlterTablePolicyRowOrder,
                AlterTablesPolicyRowOrder,
                AlterMergeTablePolicyRowOrder,

                // Update
                ShowTablePolicyUpdate,
                AlterTablePolicyUpdate,
                AlterMergeTablePolicyUpdate,
                DeleteTablePolicyUpdate,

                // IngestionBatching
                ShowDatabasePolicyIngestionBatching,
                ShowTablePolicyIngestionBatching,
                AlterDatabasePolicyIngestionBatching,
                AlterTablePolicyIngestionBatching,
                AlterTablesPolicyIngestionBatching,
                DeleteDatabasePolicyIngestionBatching,
                DeleteTablePolicyIngestionBatching,

                // Encoding
                ShowDatabasePolicyEncoding,
                ShowTablePolicyEncoding,
                ShowColumnPolicyEncoding,
                AlterDatabasePolicyEncoding,
                AlterTablePolicyEncoding,
                AlterColumnPolicyEncoding,
                AlterColumnPolicyEncodingType,
                AlterMergeDatabasePolicyEncoding,
                AlterMergeTablePolicyEncoding,
                AlterMergeColumnPolicyEncoding,
                DeleteDatabasePolicyEncoding,
                DeleteTablePolicyEncoding,
                DeleteColumnPolicyEncoding,

                // Merge
                ShowDatabasePolicyMerge,
                ShowTablePolicyMerge,
                AlterDatabasePolicyMerge,
                AlterTablePolicyMerge,
                AlterMergeDatabasePolicyMerge,
                AlterMergeTablePolicyMerge,
                DeleteDatabasePolicyMerge,
                DeleteTablePolicyMerge,

                // Partitioning
                ShowTablePolicyPartitioning,
                AlterTablePolicyPartitioning,
                AlterMergeTablePolicyPartitioning,
                DeleteTablePolicyPartitioning,

                // Restricted View Access
                ShowTablePolicyRestrictedViewAccess,
                AlterTablePolicyRestrictedViewAccess,
                AlterTablesPolicyRestrictedViewAccess,
                DeleteTablePolicyRestrictedViewAccess,

                // Row Store
                ShowClusterPolicyRowStore,
                AlterClusterPolicyRowStore,
                AlterMergeClusterPolicyRowStore,

                // Sandbox
                ShowClusterPolicySandbox,
                AlterClusterPolicySandbox,

                // Sharding
                ShowDatabasePolicySharding,
                ShowTablePolicySharding,
                AlterDatabasePolicySharding,
                AlterTablePolicySharding,
                AlterMergeDatabasePolicySharding,
                AlterMergeTablePolicySharding,
                DeleteDatabasePolicySharding,
                DeleteTablePolicySharding,

                // Streaming Ingestion
                ShowDatabasePolicyStreamingIngestion,
                ShowTablePolicyStreamingIngestion,
                ShowClusterPolicyStreamingIngestion,
                AlterDatabasePolicyStreamingIngestion,
                AlterTablePolicyStreamingIngestion,
                AlterClusterPolicyStreamingIngestion,
                DeleteDatabasePolicyStreamingIngestion,
                DeleteTablePolicyStreamingIngestion,
                DeleteClusterPolicyStreamingIngestion,

                // Callout
                ShowClusterPolicyCallout,
                AlterClusterPolicyCallout,
                AlterMergeClusterPolicyCallout,
                DeleteClusterPolicyCallout,

                // Capacity
                ShowClusterPolicyCapacity,
                AlterClusterPolicyCapacity,
                AlterMergeClusterPolicyCapacity,

                // Query Throttling
                ShowClusterPolicyQueryThrottling,
                AlterClusterPolicyQueryThrottling,
                DeleteClusterPolicyQueryThrottling,

                // Query Limit
                ShowClusterPolicyQueryLimit,
                AlterClusterPolicyQueryLimit,

                // Multi Database Admins
                ShowClusterPolicyMultiDatabaseAdmins,
                AlterClusterPolicyMultiDatabaseAdmins,
                AlterMergeClusterPolicyMultiDatabaseAdmins,
                #endregion

                #region Security Role Commands
                ShowPrincipalRoles,

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
                #endregion

                #region Data Ingestion
                IngestInlineIntoTable,
                IngestIntoTable,
                SetTable,
                AppendTable,
                SetOrAppendTable,
                SetOrReplaceTable,
                #endregion

                #region Data Export
                ExportToStorage,
                ExportToSqlTable,
                ExportToExternalTable,
                CreateOrAlterContinuousExport,
                ShowContinuousExport,
                ShowContinuousExports,
                ShowContinuousExportExportedArtifacts,
                ShowContinuousExportFailures,
                DropContinuousExport,
                EnableContinuousExport,
                DisableContinuousExport,
                #endregion

                #region System Information Commands
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
                ShowCache,
                ShowCommands,
                ShowCommandsAndQueries,
                ShowIngestionFailures,
                #endregion

                #region Advanced Commands
                ShowClusterExtents,
                ShowDatabaseExtents,
                ShowTableExtents,
                ShowTablesExtents,
                MergeExtentsDryrun,
                MergeExtents,
                MoveExtentsFrom,
                MoveExtentsQuery,
                DropExtent,
                DropExtents,
                //DropExtentsQuery,
                //DropExtentsByProperties,
                DropPretendExtentsByProperties,
                ShowVersion,
                ClearTableData,
                ClearTableCacheStreamingIngestionSchema,
                #endregion
            };
    }
}
