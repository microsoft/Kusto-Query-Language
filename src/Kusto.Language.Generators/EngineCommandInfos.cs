// <#+
#if !T4
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

///////////////////////////////////////////////////////
//
//  After editing this file, you need to do the following in Visual Studio:
//     1. Right click on EngineCommands.tt and select "Run Custom Tool"
//     2. Right click on EngineCommandGrammar.tt and select "Run Custom Tool"
//
///////////////////////////////////////////////////////

///////////////////////////////////////////////////////
//
//  Predefined grammars that can be used inside command grammars:
//
//  names:
//      <name>                          an identifier or bracketed name:  name or ['name']
//      <qualified_name>                <name>.<name>
//      <wildcarded_name>               a name with asterisks in it:  name*
//      <qualified_wildcarded_name>     <name>.<wildcarded_name>
//      <column>                        a column name reference
//      <table>                         a table name reference
//      <externaltable>                 an external table reference
//      <materializedview>              a materialized view reference
//      <function>                      a stored function name
//      <entitygroup>                   an entity group name
//      <database>                      a database name
//      <cluster>                       a cluster name
//      <database_table>                <table> or <database>.<table>
//      <database_externaltable>        <externaltable> or <database>.<externaltable>
//      <database_materializedview>     <materialized_view> or <database>.<materializedview>
//      <database_function>             <function> or <database>.<function>
//      <database_entitygroup>          <entitygroup> or <database>.<entitygroup>
//      <database_table_column>         <column> or <table>.<column> or <database>.<table>.<column>
//      <table_column>                  <column> or <table>.<column>
//
//  values:
//      <value>                         an scalar value
//      <timespan>                      a timespan literal
//      <datetime>                      a datetime literal
//      <string>                        a string literal in quotes:   "string" or 'string'
//      <bracketed_string>              a string literal in brackets: [I'm a string]
//      <bool>                          a boolean value
//      <long>                          a long value
//      <int>                           an int value
//      <decimal>                       a decimal value
//      <real>                          a real value
//      <type>                          a type name (long, int, string, etc)
//      <guid>                          a guid value
//
//  other syntax:
//      <function_declaration>          a function parameter and body declaration: (...) { ... }
//      <function_body>                 a function body declaration:  { ... }
//      <input_query>                   a query after an input or output pipe
//      <input_script>                  a list of commands
//      <input_data>                    the remaining text of the command
//      <bracketed_input_data>          [ data until end of command ]
//
///////////////////////////////////////////////////////


namespace Kusto.Language.Generators
{
#endif

    public static class EngineCommandInfos
    {
        private static readonly string UnknownResult = "()";

        private static string PropertyList(string propertyNameRule = null) =>
            string.IsNullOrEmpty(propertyNameRule)
                ? "with '(' { PropertyName=<name> '=' PropertyValue=<value>, ',' }+ ')'"
                : $"with '(' {{ PropertyName=({propertyNameRule} | <name>) '=' PropertyValue=<value>, ',' }}+ ')'";


        #region Schema Commands
        #region Databases

        private static readonly string DatabasesNameList = "'(' { DatabaseName=<database>, ',' }+ ')'";
        private static readonly string WildcardedDatabasesNameList = "'(' { DatabaseName=(<wildcarded_name> | <database>), ',' }+ ')'";

        public static readonly CommandInfo ShowDatabase =
            new CommandInfo(nameof(ShowDatabase),
                $"show database [{PropertyList()}]",
                "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, ReservedSlot1: bool, DatabaseId: guid, InTransitionTo: string)",
                "DatabaseShowCommand(null, (PropertyName, PropertyValue)*)");

        public static readonly CommandInfo ShowDatabaseDetails =
            new CommandInfo(nameof(ShowDatabaseDetails),
                $"show database flavor=(details | verbose) [{PropertyList()}]",
                "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, AuthorizedPrincipals: string, RetentionPolicy: string, MergePolicy: string, ReservedSlot1: string, CachingPolicy: string, ShardingPolicy: string, StreamingIngestionPolicy: string, IngestionBatchingPolicy: string, TotalSize: real, DatabaseId: guid, InTransitionTo: string)",
                "DatabaseShowCommand(flavor, (PropertyName, PropertyValue)*)");

        public static readonly CommandInfo ShowDatabaseIdentity =
            new CommandInfo(nameof(ShowDatabaseIdentity),
                $"show database identity [{PropertyList()}]",
                "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, CurrentUserIsUnrestrictedViewer: bool, DatabaseId: guid, InTransitionTo: string)",
                "DatabaseShowCommand('identity', (PropertyName, PropertyValue)*)");

        public static readonly CommandInfo ShowDatabasePolicies =
            new CommandInfo(nameof(ShowDatabasePolicies),
                $"show database policies [{PropertyList()}]",
                "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, DatabaseId: guid, AuthorizedPrincipals: string, RetentionPolicy: string, MergePolicy: string, CachingPolicy: string, ShardingPolicy: string, StreamingIngestionPolicy: string, IngestionBatchingPolicy: string)",
                "DatabaseShowCommand('policies', (PropertyName, PropertyValue)*)");

        public static readonly CommandInfo ShowDatabaseDataStats =
            new CommandInfo(nameof(ShowDatabaseDataStats),
                $"show database datastats [{PropertyList()}]", 
                "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, " +
                    "CurrentUseIsUnrestrictedViewer: bool, DatabaseId: guid, " +
                    "OriginalSize: real, ExtentSize: real, CompressedSize: real, IndexSize: real, RowCount: long, " +
                    "HotOriginalSize: real, HotExtentSize: real, HotCompressedSize: real, HotIndexSize: real, HotRowCount: long)",
                "DatabaseShowCommand('datastats', (PropertyName, PropertyValue)*)");

        public static readonly CommandInfo ShowDatabaseMetadata =
            new CommandInfo(nameof(ShowDatabaseMetadata),
                $"show database metadata [{PropertyList()}]",
                "(DatabaseName: string, Version: string, PersistentStorage: string, MetadataPersistentStorage: string, DatabaseAccessMode: string)",
                "DatabaseShowCommand('metadata', (PropertyName, PropertyValue)*)");

        public static readonly CommandInfo ShowClusterDatabases =
            new CommandInfo(nameof(ShowClusterDatabases),
                $"show [cluster] databases [{WildcardedDatabasesNameList}] [{PropertyList()}]", 
                "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, ReservedSlot1: bool, DatabaseId: guid, InTransitionTo: string)",
                "DatabasesShowCommand(null, DatabaseName*, (PropertyName, PropertyValue)*)");

        public static readonly CommandInfo ShowClusterDatabasesDetails =
            new CommandInfo(nameof(ShowClusterDatabasesDetails),
                $"show [cluster] databases [{WildcardedDatabasesNameList}] (details|verbose) [{PropertyList()}]",
                "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, AuthorizedPrincipals: string, RetentionPolicy: string, MergePolicy: string, ReservedSlot1: string, CachingPolicy: string, ShardingPolicy: string, StreamingIngestionPolicy: string, IngestionBatchingPolicy: string, TotalSize: real, DatabaseId: guid, InTransitionTo: string)",
                "DatabasesShowCommand('details', DatabaseName*, (PropertyName, PropertyValue)*)");

        public static readonly CommandInfo ShowClusterDatabasesIdentity =
            new CommandInfo(nameof(ShowClusterDatabasesIdentity),
                $"show [cluster] databases [{WildcardedDatabasesNameList}] identity [{PropertyList()}]",
                "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, CurrentUserIsUnrestrictedViewer: bool, DatabaseId: guid, InTransitionTo: string)",
                "DatabasesShowCommand('identity', DatabaseName*, (PropertyName, PropertyValue)*)");

        public static readonly CommandInfo ShowClusterDatabasesPolicies =
            new CommandInfo(nameof(ShowClusterDatabasesPolicies),
                $"show [cluster] databases [{WildcardedDatabasesNameList}] policies [{PropertyList()}]",
                "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, DatabaseId: guid, AuthorizedPrincipals: string, RetentionPolicy: string, MergePolicy: string, CachingPolicy: string, ShardingPolicy: string, StreamingIngestionPolicy: string, IngestionBatchingPolicy: string)",
                "DatabasesShowCommand('policies', DatabaseName*, (PropertyName, PropertyValue)*)");

        public static readonly CommandInfo ShowClusterDatabasesDataStats =
            new CommandInfo(nameof(ShowClusterDatabasesDataStats),
                $"show [cluster] databases [{WildcardedDatabasesNameList}] datastats [{PropertyList()}]",
                "(DatabaseName: string, PersistentStorage: string, Version: string, IsCurrent: bool, DatabaseAccessMode: string, PrettyName: string, DatabaseId: guid, OriginalSize: real, ExtentSize: real, CompressedSize: real, IndexSize: real, RowCount: long, HotOriginalSize: real, HotExtentSize: real, HotCompressedSize: real, HotIndexSize: real, HotRowCount: long)",
                "DatabasesShowCommand('datastats', DatabaseName*, (PropertyName, PropertyValue)*)");

        public static readonly CommandInfo ShowClusterDatabasesMetadata =
            new CommandInfo(nameof(ShowClusterDatabasesMetadata),
                $"show [cluster] databases [{WildcardedDatabasesNameList}] metadata [{PropertyList()}]",
                "(DatabaseName: string, Version: string, PersistentStorage: string, MetadataPersistentStorage: string, DatabaseAccessMode: string)",
                "DatabasesShowCommand('metadata', DatabaseName*, (PropertyName, PropertyValue)*)");

        public static readonly CommandInfo CreateDatabase =
            new CommandInfo(nameof(CreateDatabase),
                $"create database DatabaseName=<name> [persist '(' {{ Path=<string>, ',' }}+ ')' | volatile] [IfNotExists=ifnotexists] [{PropertyList()}]",
                "(DatabaseName: string, PersistentPath: string, Created: string, StoresMetadata: bool, StoresData: bool)",
                "DatabaseCreateCommand(DatabaseName, Path*, IfNotExists?, (PropertyName, PropertyValue)*)");

        public static readonly CommandInfo AttachDatabase =
            new CommandInfo(nameof(AttachDatabase),
                $"(attach|#load) [database #[all|metadata] DatabaseName=<database>] from Path=<string> [ReadOnly=readonly [version '=' Version=<string>]] [{PropertyList()}]",
                "(Step: string, Duration: string)",
                "DatabaseAttachCommand(DatabaseName, Path, ReadOnly ?? 'ReadWrite', Version, (PropertyName, PropertyValue)*)");

        public static readonly CommandInfo DetachDatabase =
            new CommandInfo(nameof(DetachDatabase),
                "(detach|#drop) database DatabaseName=<database> [IfExists=ifexists] [SkipSeal='skip-seal']",
                "(Table: string, NumberOfRemovedExtents: string)",
                "DatabaseDetachCommand(DatabaseName, IfExists?, SkipSeal?)");

        public static readonly CommandInfo AlterDatabasePrettyName =
            new CommandInfo(nameof(AlterDatabasePrettyName),
                "(alter|set) database [DatabaseName=<database>] prettyname PrettyName=<string>",
                "(DatabaseName: string, PrettyName: string)",
                "DatabaseSetPrettyNameCommand(DatabaseName, PrettyName)");

        public static readonly CommandInfo DropDatabasePrettyName =
            new CommandInfo(nameof(DropDatabasePrettyName),
                "drop database DatabaseName=<database> prettyname",
                "(DatabaseName: string, PrettyName: string)",
                "DatabaseSetPrettyNameCommand(DatabaseName, null)");

        public static readonly CommandInfo AlterDatabasePersistMetadata =
            new CommandInfo(nameof(AlterDatabasePersistMetadata),
                "alter database DatabaseName=<database> persist metadata [Path=<string> [AllowNonEmptyContainer='allow-non-empty-container']]",
                "(Moniker: guid, Url: string, State: string, CreatedOn: datetime, MaxDateTime: datetime, IsRecyclable: bool, StoresDatabaseMetadata: bool, HardDeletePeriod: timespan)",
                "DatabaseMetadataContainerAlterCommand(DatabaseName, Path, AllowNonEmptyContainer?)");

        public static readonly CommandInfo SetAccess =
            new CommandInfo(nameof(SetAccess),
                "set access DatabaseName=<database> to AccessMode=(readonly | readwrite)",
                "(DatabaseName: string, RequestedAccessMode: string, Status: string)",
                "DatabaseSetAccessModeCommand(DatabaseName, AccessMode)");

        public static readonly CommandInfo ShowDatabaseSchema =
            new CommandInfo(nameof(ShowDatabaseSchema),
                "show database [DatabaseName=<database>] schema [Details=details] [if_later_than Version=<string>]",
                "(DatabaseName: string, TableName: string, ColumnName: string, ColumnType: string, IsDefaultTable: bool, IsDefaultColumn: bool, PrettyName: string, Version: string, Folder: string, DocString: string)",
                "SchemaShowCommand(Details?, (DatabaseName, Version))");

        public static readonly CommandInfo ShowDatabaseSchemaAsJson =
            new CommandInfo(nameof(ShowDatabaseSchemaAsJson),
                $"show database (schema | DatabaseName=<database> schema) [if_later_than Version=<string>] as json [{PropertyList()}]",
                "(DatabaseSchema: string)",
                "SchemaShowAsJsonCommand((DatabaseName, Version), (PropertyName, PropertyValue)*)");

        public static readonly CommandInfo ShowDatabaseSchemaAsCslScript =
            new CommandInfo(nameof(ShowDatabaseSchemaAsCslScript),
                $"show database (schema | DatabaseName=<database> schema) [if_later_than Version=<string>] as (kql | csl) [Script=script] [{PropertyList()}]",
                "(DatabaseSchemaScript: string)",
                "DatabaseSchemaShowAsCslCommand(Script?, (DatabaseName, Version), (PropertyName, PropertyValue)*)");

        public static readonly CommandInfo ShowDatabaseCslSchema =
            new CommandInfo(nameof(ShowDatabaseCslSchema),
                $"show database [databaseName=<database>] (cslschema | kqlschema) [Script=script] [if_later_than databaseVersion=<string>]",
                "(TableName: string, Schema: string, DatabaseName: string, Folder: string, DocString: string)",
                "DatabaseSchemaShowAsCslCommand(Script?, (DatabaseName, Version))");

        public static readonly CommandInfo ShowDatabaseSchemaViolations =
            new CommandInfo(nameof(ShowDatabaseSchemaViolations),
                "show database [DatabaseName=<database>] schema violations",
                "(EntityKind: string, EntityName: string, Property: string, Reason: string)",
                "DatabaseSchemaViolationsShowCommand(DatabaseName)");

        public static readonly CommandInfo ShowDatabasesSchema =
            new CommandInfo(nameof(ShowDatabasesSchema),
                "show databases (schema | '(' { (DatabaseName=<database> [if_later_than Version=<string>]), ',' }+ ')' schema) [Details=details]",
                "(DatabaseName: string, TableName: string, ColumnName: string, ColumnType: string, IsDefaultTable: bool, IsDefaultColumn: bool, PrettyName: string, Version: string, Folder: string, DocString: string)",
                "SchemaShowCommand(Details?, (DatabaseName, Version)*)");

        public static readonly CommandInfo ShowDatabasesSchemaAsJson =
            new CommandInfo(nameof(ShowDatabasesSchemaAsJson),
                $"show databases (schema | '(' {{ (DatabaseName=<database> [if_later_than Version=<string>]), ',' }}+ ')' schema) [details] as json [{PropertyList()}]",
                "(DatabaseSchema: string)",
                "SchemaShowAsJsonCommand((DatabaseName, Version)*, (PropertyName, PropertyValue)*)");

        private readonly static string DatabaseIngestionMappingResult =
           "(Name: string, Kind: string, Mapping: string, LastUpdatedOn: datetime, Database: string)";

        public static readonly CommandInfo CreateDatabaseIngestionMapping =
            new CommandInfo(nameof(CreateDatabaseIngestionMapping),
                "create database DatabaseName=<name> ingestion MappingKind=(csv | json | avro | parquet | orc | w3clogfile | azmonstream) mapping MappingName=<string> MappingFormat=<string>",
                DatabaseIngestionMappingResult);

        public static readonly CommandInfo CreateOrAlterDatabaseIngestionMapping =
            new CommandInfo(nameof(CreateOrAlterDatabaseIngestionMapping),
                "create-or-alter database DatabaseName=<name> ingestion MappingKind=(csv | json | avro | parquet | orc | w3clogfile | azmonstream) mapping MappingName=<string> MappingFormat=<string>",
                DatabaseIngestionMappingResult);

        public static readonly CommandInfo AlterDatabaseIngestionMapping =
            new CommandInfo(nameof(AlterDatabaseIngestionMapping),
                "alter database DatabaseName=<database> ingestion MappingKind=(csv | json | avro | parquet | orc | w3clogfile | azmonstream) mapping MappingName=<string> MappingFormat=<string>",
                DatabaseIngestionMappingResult);

        public static readonly CommandInfo AlterMergeDatabaseIngestionMapping =
            new CommandInfo(nameof(AlterMergeDatabaseIngestionMapping),
                "alter-merge database DatabaseName=<database> ingestion MappingKind=(csv | json | avro | parquet | orc | w3clogfile | azmonstream) mapping MappingName=<string> MappingFormat=<string>",
                DatabaseIngestionMappingResult);

        public static readonly CommandInfo ShowDatabaseIngestionMappings =
            new CommandInfo(nameof(ShowDatabaseIngestionMappings),
                $"show database [databaseName=<database>] ingestion [MappingKind=(csv | avro | apacheavro | json | parquet | sstream | orc | w3clogfile | azmonstream)] mappings [name=<string>] [{PropertyList()}]",
                DatabaseIngestionMappingResult);

        public static readonly CommandInfo ShowIngestionMappings =
            new CommandInfo(nameof(ShowIngestionMappings),
                $"show [cluster] ingestion [MappingKind=(csv | avro | apacheavro | json | parquet | sstream | orc | w3clogfile | azmonstream)] mappings [{PropertyList()}]",
                UnknownResult);

        public static readonly CommandInfo DropDatabaseIngestionMapping =
            new CommandInfo(nameof(DropDatabaseIngestionMapping),
                "drop database DatabaseName=<database> ingestion MappingKind=(csv | json | avro | parquet | orc | w3clogfile | azmonstream) mapping MappingName=<string>",
                DatabaseIngestionMappingResult);

        #endregion

        #region Tables
        private static readonly string ExtentIdList = "'(' {ExtentId=<guid>, ','}+ ')'";

        private static readonly string ShowTablesResult =
            "(TableName: string, DatabaseName: string, Folder: string, DocString: string)";

        private static readonly string ShowTablesDetailsResult =
            "(TableName: string, DatabaseName: string, Folder: string, DocString: string, TotalExtents: long, TotalExtentSize: real, TotalOriginalSize: real, TotalRowCount: long, HotExtents: long, HotExtentSize: real, HotOriginalSize: real, HotRowCount: long, AuthorizedPrincipals: string, RetentionPolicy: string, CachingPolicy: string, ShardingPolicy: string, MergePolicy: string, StreamingIngestionPolicy: string, IngestionBatchingPolicy: string, MinExtentsCreationTime: datetime, MaxExtentsCreationTime: datetime, RowOrderPolicy: string, TableId: guid)";

        private static readonly string ShowTableSchemaResult =
            "(TableName: string, Schema: string, DatabaseName: string, Folder: string, DocString: string)";

        public static readonly CommandInfo ShowTables =
            new CommandInfo(nameof(ShowTables),
                "show tables ['(' { TableName=<table>, ',' }+ ')']",
                ShowTablesResult);

        public static readonly CommandInfo ShowTable =
            new CommandInfo(nameof(ShowTable),
                "show table TableName=<table>",
                "(AttributeName: string, AttributeType: string, ExtentSize: long, CompressionRatio: real, IndexSize: long, IndexSizePercent: real, OriginalSize: long, AttributeId: guid, SharedIndexSize: long, StorageEngineVersion: string)");

        public static readonly CommandInfo ShowTablesDetails =
            new CommandInfo(nameof(ShowTablesDetails),
                "show tables ['(' { TableName=<table>, ',' }+ ')'] details",
                ShowTablesDetailsResult);

        public static readonly CommandInfo ShowTableDetails =
            new CommandInfo(nameof(ShowTableDetails),
                "show table TableName=<table> details",
                ShowTablesDetailsResult);

        public static readonly CommandInfo ShowTableCslSchema =
            new CommandInfo(nameof(ShowTableCslSchema),
                "show table TableName=<table> (kqlschema | cslschema)",
                ShowTableSchemaResult);

        public static readonly CommandInfo ShowTableSchemaAsJson =
            new CommandInfo(nameof(ShowTableSchemaAsJson),
                "show table TableName=<table> schema as json",
                ShowTableSchemaResult);

        private static readonly string TableSchema = "('(' { ColumnName=<name> ':' ColumnType=<type>, ',' }+ ')')";

        private static readonly string TableProperties = PropertyList("docstring | folder");

        public static readonly CommandInfo CreateTable =
            new CommandInfo(nameof(CreateTable),
                $"create table TableName=<name> {TableSchema} [{TableProperties}]",
                ShowTableSchemaResult);

        public static readonly CommandInfo CreateTableBasedOnAnother =
            new CommandInfo(nameof(CreateTableBasedOnAnother),
                $"create table NewTableName=<name> based-on TableName=<name> [ifnotexists] [{TableProperties}]",
                ShowTableSchemaResult);

        public static readonly CommandInfo CreateMergeTable =
            new CommandInfo(nameof(CreateMergeTable),
                $"create-merge table TableName=<name> {TableSchema} [{TableProperties}]",
                ShowTableSchemaResult);

        public static readonly CommandInfo DefineTable =
            new CommandInfo(nameof(DefineTable),
                $"define table TableName=<name> {TableSchema} [{TableProperties}]",
                ShowTableSchemaResult);

        public static readonly CommandInfo CreateTables =
            new CommandInfo(nameof(CreateTables),
                $"create tables {{ TableName=<name> {TableSchema}, ',' }}+ [{PropertyList()}]",
                "(TableName: string, DatabaseName: string, Folder: string, DocString: string)");

        public static readonly CommandInfo CreateMergeTables =
            new CommandInfo(nameof(CreateMergeTables),
                $"create-merge tables {{ TableName=<name> {TableSchema}, ',' }}+ [{PropertyList()}]",
                "(TableName: string, DatabaseName: string, Folder: string, DocString: string)");

        public static readonly CommandInfo DefineTables =
            new CommandInfo(nameof(DefineTables),
                $"define tables {{ TableName=<name> {TableSchema}, ',' }}+ [{PropertyList()}]",
                "(TableName: string, DatabaseName: string, Folder: string, DocString: string)");

        public static readonly CommandInfo AlterTable =
            new CommandInfo(nameof(AlterTable),
                $"alter table TableName=<table> {TableSchema} [{TableProperties}]",
                ShowTableSchemaResult);

        public static readonly CommandInfo AlterMergeTable =
            new CommandInfo(nameof(AlterMergeTable),
                $"alter-merge table TableName=<table> {TableSchema} [{TableProperties}]",
                ShowTableSchemaResult);

        public static readonly CommandInfo AlterTableDocString =
            new CommandInfo(nameof(AlterTableDocString),
                "alter table TableName=<table> docstring Documentation=<string>",
                "(TableName: string, Schema: string, DatabaseName: string, Folder: string, DocString: string)");

        public static readonly CommandInfo AlterTableFolder =
            new CommandInfo(nameof(AlterTableFolder),
                "alter table TableName=<table> folder Folder=<string>",
                "(TableName: string, Schema: string, DatabaseName: string, Folder: string, DocString: string)");

        public static readonly CommandInfo RenameTable =
            new CommandInfo(nameof(RenameTable),
                "rename table TableName=<table> to NewTableName=<name>",
                ShowTablesResult);

        public static readonly CommandInfo RenameTables =
            new CommandInfo(nameof(RenameTables),
                "rename tables { NewTableName=<name> '=' TableName=<table>, ',' }+",
                ShowTablesResult);

        public static readonly CommandInfo UndoDropExtentContainer =
            new CommandInfo(nameof(UndoDropExtentContainer),
                "undo drop extentcontainer ContainerID=<guid>",
                "(ContainerId: guid, RestoreContainerFinalState: string)");

        public static readonly CommandInfo DropTable =
            new CommandInfo(nameof(DropTable),
                "drop table TableName=<table> [ifexists]",
                "(TableName: string, DatabaseName: string)");

        public static readonly CommandInfo UndoDropTable =
            new CommandInfo(nameof(UndoDropTable),
                "undo drop table <name> [as TableName=<name>] version '=' Version=<string> (internal)?",
                "(ExtentId: guid, NumberOfRecords: long, Status: string, FailureReason: string)");

        private static readonly string TableNameList = "'(' { TableName=<table>, ',' }+ ')'";

        public static readonly CommandInfo DropTables =
            new CommandInfo(nameof(DropTables),
                $"drop tables {TableNameList} [ifexists]",
                "(TableName: string, DatabaseName: string)");

        private readonly static string TableIngestionMappingResult =
            "(Name: string, Kind: string, Mapping: string, LastUpdatedOn: datetime, Database: string, Table: string)";

        public static readonly CommandInfo CreateTableIngestionMapping =
            new CommandInfo(nameof(CreateTableIngestionMapping),
                $"create table TableName=<name> ingestion MappingKind=(csv | json | avro | parquet | orc | w3clogfile | azmonstream) mapping MappingName=<string> MappingFormat=<string> [{PropertyList()}]",
                TableIngestionMappingResult);

        public static readonly CommandInfo CreateOrAlterTableIngestionMapping =
            new CommandInfo(nameof(CreateOrAlterTableIngestionMapping),
                $"create-or-alter table TableName=<table> ingestion MappingKind=(csv | json | avro | parquet | orc | w3clogfile | azmonstream) mapping MappingName=<string> MappingFormat=<string> [{PropertyList()}]",
                TableIngestionMappingResult);

        public static readonly CommandInfo AlterTableIngestionMapping =
            new CommandInfo(nameof(AlterTableIngestionMapping),
                $"alter table TableName=<table> ingestion MappingKind=(csv | json | avro | parquet | orc | w3clogfile | azmonstream) mapping MappingName=<string> MappingFormat=<string> [{PropertyList()}]",
                TableIngestionMappingResult);

        public static readonly CommandInfo AlterMergeTableIngestionMapping =
            new CommandInfo(nameof(AlterMergeTableIngestionMapping),               
                $"alter-merge table TableName=<table> ingestion MappingKind=(csv | json | avro | parquet | orc | w3clogfile | azmonstream) mapping MappingName=<string> MappingFormat=<string> [{PropertyList()}]",
                TableIngestionMappingResult);

        public static readonly CommandInfo ShowTableIngestionMappings =
            new CommandInfo(nameof(ShowTableIngestionMappings),
                "show table TableName=<table> ingestion [MappingKind=(csv | json | avro | parquet | orc | w3clogfile | azmonstream)] mappings",
                TableIngestionMappingResult);

        public static readonly CommandInfo ShowTableIngestionMapping =
            new CommandInfo(nameof(ShowTableIngestionMapping),
                "show table TableName=<table> ingestion [MappingKind=(csv | json | avro | parquet | orc | w3clogfile | azmonstream)] mapping MappingName=<string>",
                TableIngestionMappingResult);

        public static readonly CommandInfo DropTableIngestionMapping =
            new CommandInfo(nameof(DropTableIngestionMapping),
                "drop table TableName=<table> ingestion MappingKind=(csv | json | avro | parquet | orc | w3clogfile | azmonstream) mapping MappingName=<string>",
                TableIngestionMappingResult);
        #endregion

        #region Columns

        public static readonly CommandInfo RenameColumn =
            new CommandInfo(nameof(RenameColumn),
                "rename column ColumnName=<database_table_column> to NewColumnName=<name>",
                "(EntityName: string, DataType: string, Policy: string)");

        public static readonly CommandInfo RenameColumns =
            new CommandInfo(nameof(RenameColumns),
                "rename columns { NewColumnName=<name> '=' ColumnName=<database_table_column>, ',' }+",
                "(EntityName: string, DataType: string, Policy: string)");

        public static readonly CommandInfo AlterColumnType =
            new CommandInfo(nameof(AlterColumnType),
                "alter column ColumnName=<database_table_column> type '=' ColumnType=<type>",
                "(EntityName: string, DataType: string, Policy: string)");

        public static readonly CommandInfo DropColumn =
            new CommandInfo(nameof(DropColumn),
                "drop column ColumnName=<table_column>",
                "(TableName: string, Schema: string, DatabaseName: string, Folder: string, DocString: string)");

        public static readonly CommandInfo DropTableColumns =
            new CommandInfo(nameof(DropTableColumns),
                "drop table TableName=<table> columns '(' { ColumnName=<column>, ',' }+ ')'",
                "(TableName: string, Schema: string, DatabaseName: string, Folder: string, DocString: string)");

        public static readonly CommandInfo AlterTableColumnDocStrings =
            new CommandInfo(nameof(AlterTableColumnDocStrings),
                "alter table TableName=<table> column-docstrings '(' { ColumnName=<column> ':' DocString=<string>, ',' }+ ')'",
                "(TableName: string, Schema: string, DatabaseName: string, Folder: string, DocString: string)");

        public static readonly CommandInfo AlterMergeTableColumnDocStrings =
            new CommandInfo(nameof(AlterMergeTableColumnDocStrings),
                "alter-merge table TableName=<table> column-docstrings '(' { ColumnName=<column> ':' DocString=<string>, ',' }+ ')'",
                "(TableName: string, Schema: string, DatabaseName: string, Folder: string, DocString: string)");
        #endregion

        #region Functions
        private static readonly string FunctionResult =
            "(Name: string, Parameters: string, Body: string, Folder: string, DocString: string)";

        public static readonly CommandInfo ShowFunctions =
            new CommandInfo(nameof(ShowFunctions),
                "show functions",
                FunctionResult);

        public static readonly CommandInfo ShowFunction =
            new CommandInfo(nameof(ShowFunction),
                $"show function FunctionName=<function> [{PropertyList()}]",
                FunctionResult);

        public static readonly CommandInfo CreateFunction =
            new CommandInfo(nameof(CreateFunction),
                $"create function [ifnotexists] [{PropertyList()}] FunctionName=<name> <function_declaration>",
                FunctionResult);

        public static readonly CommandInfo AlterFunction =
            new CommandInfo(nameof(AlterFunction),
                $"alter function [{PropertyList()}] FunctionName=<function> <function_declaration>",
                FunctionResult);

        public static readonly CommandInfo CreateOrAlterFunction =
            new CommandInfo(nameof(CreateOrAlterFunction),
                $"create-or-alter function [{PropertyList()}] FunctionName=<name> <function_declaration>",
                FunctionResult);

        private static readonly string FunctionNameList = "'(' { FunctionName=<function>, ',' }+ ')'";

        public static readonly CommandInfo DropFunction =
            new CommandInfo(nameof(DropFunction),
                "drop function FunctionName=<function> [ifexists]",
                "(Name: string)");

        public static readonly CommandInfo DropFunctions =
            new CommandInfo(nameof(DropFunctions),
                $"drop functions {FunctionNameList} [ifexists]",
                FunctionResult);

        public static readonly CommandInfo AlterFunctionDocString =
            new CommandInfo(nameof(AlterFunctionDocString),
                "alter function FunctionName=<function> docstring Documentation=<string>",
                FunctionResult);

        public static readonly CommandInfo AlterFunctionFolder =
            new CommandInfo(nameof(AlterFunctionFolder),
                "alter function FunctionName=<function> folder Folder=<string>",
                FunctionResult);
        #endregion

        #region EntityGroups
        private static readonly string EntityGroupShowResult = "(Name: string, Entities: string)";
        #endregion

        #region External Tables
        private static readonly string ExternalTableResult =
            "(TableName: string, TableType: string, Folder: string, DocString: string, Properties: string, ConnectionStrings: dynamic, Partitions: dynamic, PathFormat: string)";

        private static readonly string ExternalTableDetailsResult =
            "(TableName: string, QueryAccelerationPolicy: string, QueryAccelerationState: string)";

        private static readonly string ExternalTableSchemaResult =
            "(TableName: string, Schema: string, DatabaseName: string, Folder: string, DocString: string)";

        private static readonly string ExternalTableArtifactsResult =
            "(Uri: string, Partition: dynamic, Size: long)";

        private static readonly string ExternalTableFullResult =
            "(TableName: string, TableType: string, Folder: string, DocString: string, Schema: string, Properties: string)";

        public static readonly CommandInfo ShowExternalTables =
            new CommandInfo(nameof(ShowExternalTables),               
                $"show external tables [{PropertyList()}]",
                ExternalTableResult);

        public static readonly CommandInfo ShowExternalTable =
            new CommandInfo(nameof(ShowExternalTable),
                $"show external table ExternalTableName=<externaltable> [{PropertyList()}]",
                ExternalTableResult);

        public static readonly CommandInfo ShowExternalTablesDetails =
            new CommandInfo(nameof(ShowExternalTablesDetails),
                "show external tables details",
                ExternalTableDetailsResult);

        public static readonly CommandInfo ShowExternalTableDetails =
            new CommandInfo(nameof(ShowExternalTableDetails),
                "show external table ExternalTableName=<externaltable> details",
                ExternalTableDetailsResult);

        public static readonly CommandInfo ShowExternalTableCslSchema =
            new CommandInfo(nameof(ShowExternalTableCslSchema),
                "show external table ExternalTableName=<externaltable> (cslschema | kqlschema)",
                ExternalTableSchemaResult);

        public static readonly CommandInfo ShowExternalTableSchema =
            new CommandInfo(nameof(ShowExternalTableSchema),
                "show external table ExternalTableName=<externaltable> schema as (json | csl | kql)",
                ExternalTableSchemaResult);

        public static readonly CommandInfo ShowExternalTableArtifacts =
            new CommandInfo(nameof(ShowExternalTableArtifacts),
                "show external table ExternalTableName=<externaltable> artifacts [limit LimitCount=<long>]",
                ExternalTableArtifactsResult);

        public static readonly CommandInfo DropExternalTable =
            new CommandInfo(nameof(DropExternalTable),
                "drop external table ExternalTableName=<externaltable>",
                ExternalTableResult);

        private static readonly string CreateOrAlterStorageExternalTableGrammar =
            @"((external table ExternalTableName=<name> '(' { ColumnName=<name> ':' ColumnType=<type>, ',' }+ ')'
              kind '=' TableKind=(storage | #blob | #adl)
              [partition by
               '('
                {PartitionName=<name> ':'
                 (PartitionType=string ['=' StringColumn=<name>]
                  | PartitionType=datetime ['='
                    (PartitionFunction=bin '(' DateTimeColumn=<name> ',' BinValue=<timespan> ')'
                     | PartitionFunction=(startofday | startofweek | startofmonth | startofyear) '(' DateTimeColumn=<name> ')')]
                  | PartitionType=long '=' PartitionFunction=hash '(' StringColumn=<name> ',' HashMod=<long> ')'), ','}+
               ')'
               [pathformat '=' '('
                [PathSeparator=<string>]
                { (PartitionName=<name> | datetime_pattern '(' DateTimeFormat=<string> ',' PartitionName=<name> ')')
                 [PathSeparator=<string>] }+ ')']
              ]
              [catalog '=' CatalogExpression=<string>]
              dataformat '=' DataFormatKind=(avro | apacheavro | csv | json | multijson | orc | parquet | psv | raw | scsv | sohsv | sstream | tsv | tsve | txt | w3clogfile | azmonstream)
              '(' { StorageConnectionString=<string>, ',' }+ ')'
              [with '(' { PropertyName=<name> '=' Value=<value>, ',' }+ ')'])
              |
              (external table ExternalTableName=<name> ['(' { ColumnName=<name> ':' ColumnType=<type>, ',' }+ ')']
              kind '=' TableKind=(delta)
              '(' StorageConnectionString=<string> ')'
              [with '(' { PropertyName=<name> '=' Value=<value>, ',' }+ ')']))";

        private static readonly string CreateOrAlterSqlExternalTableGrammar =
            @"external table ExternalTableName=<name> '(' { ColumnName=<name> ':' ColumnType=<type>, ',' }+ ')'
              kind '=' TableKind=(sql)
              [table '=' <name>]
              '(' <string> ')'
              [with '(' { PropertyName=<name> '=' Value=<value>, ',' }+ ')']"; 

        public static readonly CommandInfo CreateStorageExternalTable =
            new CommandInfo(nameof(CreateStorageExternalTable),
                "create " + CreateOrAlterStorageExternalTableGrammar,
                ExternalTableResult);

        public static readonly CommandInfo AlterStorageExternalTable =
            new CommandInfo(nameof(AlterStorageExternalTable),
                "alter " + CreateOrAlterStorageExternalTableGrammar,
                ExternalTableFullResult);

        public static readonly CommandInfo CreateOrAlterStorageExternalTable =
            new CommandInfo(nameof(CreateOrAlterStorageExternalTable),
                "create-or-alter " + CreateOrAlterStorageExternalTableGrammar,
                ExternalTableFullResult);

        public static readonly CommandInfo CreateSqlExternalTable =
            new CommandInfo(nameof(CreateSqlExternalTable),
                "create " + CreateOrAlterSqlExternalTableGrammar,
                ExternalTableResult);

        public static readonly CommandInfo AlterSqlExternalTable =
            new CommandInfo(nameof(AlterSqlExternalTable),
                "alter " + CreateOrAlterSqlExternalTableGrammar,
                ExternalTableFullResult);

        public static readonly CommandInfo CreateOrAlterSqlExternalTable =
            new CommandInfo(nameof(CreateOrAlterSqlExternalTable),
                "create-or-alter " + CreateOrAlterSqlExternalTableGrammar,
                ExternalTableFullResult);

        public static readonly CommandInfo CreateExternalTableMapping =
            new CommandInfo(nameof(CreateExternalTableMapping),
                "create external table ExternalTableName=<externaltable> mapping MappingName=<string> MappingFormat=<string>",
                TableIngestionMappingResult);

        public static readonly CommandInfo SetExternalTableAdmins =
            new CommandInfo(nameof(SetExternalTableAdmins),
                "set external table externalTableName=<externaltable> admins (none [skip-results] | '(' {principal=<string>, ','}+ ')' [skip-results] [notes=<string>])",
                UnknownResult);

        public static readonly CommandInfo AddExternalTableAdmins =
            new CommandInfo(nameof(AddExternalTableAdmins),
                "add external table externalTableName=<externaltable> admins '(' {principal=<string>, ','}+ ')' [skip-results] [notes=<string>]",
                UnknownResult);

        public static readonly CommandInfo DropExternalTableAdmins =
            new CommandInfo(nameof(DropExternalTableAdmins),
                "drop external table externalTableName=<externaltable> admins '(' {principal=<string>, ','}+ ')' [skip-results] [notes=<string>]",
                UnknownResult);

        public static readonly CommandInfo AlterExternalTableDocString =
            new CommandInfo(nameof(AlterExternalTableDocString),
                "alter external table tableName=<externaltable> docstring docStringValue=<string>",
                UnknownResult);

        public static readonly CommandInfo AlterExternalTableFolder =
            new CommandInfo(nameof(AlterExternalTableFolder),
                "alter external table tableName=<externaltable> folder folderValue=<string>",
                UnknownResult);

        public static readonly CommandInfo ShowExternalTablePrincipals =
            new CommandInfo(nameof(ShowExternalTablePrincipals),
                "show external table tableName=<externaltable> principals",
                UnknownResult);

        public static readonly CommandInfo ShowFabric =
            new CommandInfo(nameof(ShowFabric),
                "show fabric id=<name>",
                UnknownResult);

        public static readonly CommandInfo AlterExternalTableMapping =
            new CommandInfo(nameof(AlterExternalTableMapping),
                "alter external table ExternalTableName=<externaltable> mapping MappingName=<string> MappingFormat=<string>",
                TableIngestionMappingResult);

        public static readonly CommandInfo ShowExternalTableMappings =
            new CommandInfo(nameof(ShowExternalTableMappings),
                "show external table ExternalTableName=<externaltable> mappings",
                TableIngestionMappingResult);

        public static readonly CommandInfo ShowExternalTableMapping =
            new CommandInfo(nameof(ShowExternalTableMapping),
                "show external table ExternalTableName=<externaltable> mapping MappingName=<string>",
                TableIngestionMappingResult);

        public static readonly CommandInfo DropExternalTableMapping =
            new CommandInfo(nameof(DropExternalTableMapping),
                "drop external table ExternalTableName=<externaltable> mapping MappingName=<string>",
                TableIngestionMappingResult);
        #endregion

        #region Workload groups
        private static readonly string WorkloadGroupResult =
            "(WorkloadGroupName: string, WorkloadGroup:string)";

        public static readonly CommandInfo ShowWorkloadGroups =
            new CommandInfo(nameof(ShowWorkloadGroups),
                "show workload_groups",
                WorkloadGroupResult);

        public static readonly CommandInfo ShowWorkloadGroup =
            new CommandInfo(nameof(ShowWorkloadGroup),
                "show workload_group WorkloadGroup=<name>",
                WorkloadGroupResult);

        public static readonly CommandInfo CreateOrAleterWorkloadGroup =
            new CommandInfo(nameof(CreateOrAleterWorkloadGroup),
                "create-or-alter workload_group WorkloadGroupName=<name> WorkloadGroup=<string>",
                WorkloadGroupResult);

        public static readonly CommandInfo AlterMergeWorkloadGroup =
            new CommandInfo(nameof(AlterMergeWorkloadGroup),
                "alter-merge workload_group WorkloadGroupName=<name> WorkloadGroup=<string>",
                WorkloadGroupResult);

        public static readonly CommandInfo DropWorkloadGroup =
            new CommandInfo(nameof(DropWorkloadGroup),
                "drop workload_group WorkloadGroupName=<name>",
                WorkloadGroupResult);
        #endregion
        #endregion

        #region Policy Commands
        private static readonly string PolicyResult =
            "(PolicyName: string, EntityName: string, Policy: string, ChildEntities: string, EntityType: string)";

        #region Caching
        public static readonly CommandInfo ShowDatabasePolicyCaching =
            new CommandInfo(nameof(ShowDatabasePolicyCaching),
                "show database [DatabaseName=(<database> | '*')] policy caching",
                PolicyResult);

        public static readonly CommandInfo ShowTablePolicyCaching =
            new CommandInfo(nameof(ShowTablePolicyCaching),
                "show table TableName=<database_table> policy caching",
                PolicyResult);

        public static readonly CommandInfo ShowTableStarPolicyCaching =
            new CommandInfo(nameof(ShowTableStarPolicyCaching),
                "show table '*' policy caching",
                PolicyResult);

        public static readonly CommandInfo ShowColumnPolicyCaching =
            new CommandInfo(nameof(ShowColumnPolicyCaching),
                "show column ColumnName=(<database_table_column> | '*') policy caching",
                PolicyResult);

        public static readonly CommandInfo ShowMaterializedViewPolicyCaching =
            new CommandInfo(nameof(ShowMaterializedViewPolicyCaching),
                "show materialized-view MaterializedViewName=<database_materializedview> policy caching",
                PolicyResult);

        public static readonly CommandInfo ShowGraphModelPolicyCaching =
            new CommandInfo(nameof(ShowGraphModelPolicyCaching),
                "show graph_model ModelName=<graph_model> policy caching",
                PolicyResult);

        public static readonly CommandInfo ShowGraphModelStarPolicyCaching =
            new CommandInfo(nameof(ShowGraphModelStarPolicyCaching),
                "show graph_model '*' policy caching",
                PolicyResult);

        public static readonly CommandInfo ShowClusterPolicyCaching =
            new CommandInfo(nameof(ShowClusterPolicyCaching),
                "show cluster policy caching",
                PolicyResult);

        private static readonly string HotPolicy =
            "(hot '=' Timespan=<timespan> | hotdata '=' HotData=<timespan> hotindex '=' HotIndex=<timespan>)";

        public static readonly CommandInfo AlterDatabasePolicyCaching =
            new CommandInfo(nameof(AlterDatabasePolicyCaching),
                $"alter database [DatabaseName=<database>] policy caching {HotPolicy}",
                PolicyResult);

        public static readonly CommandInfo AlterTablePolicyCaching =
            new CommandInfo(nameof(AlterTablePolicyCaching),
                $"alter table TableName=<database_table> policy caching {HotPolicy}",
                PolicyResult);

        public static readonly CommandInfo AlterTablesPolicyCaching =
            new CommandInfo(nameof(AlterTablesPolicyCaching),
                $"alter tables '(' {{TableName=<table>, ','}}+ ')' policy caching {HotPolicy} [[','] {{hot_window '=' p=(d1=<datetime> '..' d2=<datetime>), ','}}+]",
                UnknownResult);

        public static readonly CommandInfo AlterColumnPolicyCaching =
            new CommandInfo(nameof(AlterColumnPolicyCaching),
                $"alter column ColumnName=<database_table_column> policy caching {HotPolicy}",
                PolicyResult);

        public static readonly CommandInfo AlterMaterializedViewPolicyCaching =
            new CommandInfo(nameof(AlterMaterializedViewPolicyCaching),
                $"alter materialized-view MaterializedViewName=<database_materializedview> policy caching {HotPolicy}",
                PolicyResult);

        public static readonly CommandInfo AlterGraphModelPolicyCaching =
            new CommandInfo(nameof(AlterGraphModelPolicyCaching),
                $"alter graph_model ModelName=<graph_model> policy caching {HotPolicy}",
                PolicyResult);

        public static readonly CommandInfo AlterClusterPolicyCaching =
            new CommandInfo(nameof(AlterClusterPolicyCaching),
                $"alter cluster policy caching {HotPolicy}",
                PolicyResult);

        public static readonly CommandInfo DeleteDatabasePolicyCaching =
            new CommandInfo(nameof(DeleteDatabasePolicyCaching),
                "delete database DatabaseName=<database> policy caching",
                PolicyResult);

        public static readonly CommandInfo DeleteTablePolicyCaching =
            new CommandInfo(nameof(DeleteTablePolicyCaching),
                "delete table TableName=<database_table> policy caching",
                PolicyResult);

        public static readonly CommandInfo DeleteColumnPolicyCaching =
            new CommandInfo(nameof(DeleteColumnPolicyCaching),
                "delete column ColumnName=<database_table_column> policy caching",
                PolicyResult);

        public static readonly CommandInfo DeleteMaterializedViewPolicyCaching =
            new CommandInfo(nameof(DeleteMaterializedViewPolicyCaching),
                "delete materialized-view MaterializedViewName=<database_materializedview> policy caching",
                PolicyResult);

        public static readonly CommandInfo DeleteGraphModelPolicyCaching =
            new CommandInfo(nameof(DeleteGraphModelPolicyCaching),
                "delete graph_model ModelName=<graph_model> policy caching",
                PolicyResult);

        public static readonly CommandInfo DeleteClusterPolicyCaching =
            new CommandInfo(nameof(DeleteClusterPolicyCaching),
                "delete cluster policy caching",
                PolicyResult);
        #endregion

        #region IngestionTime
        public static readonly CommandInfo ShowTablePolicyIngestionTime =
            new CommandInfo(nameof(ShowTablePolicyIngestionTime),
                "show table TableName=<database_table> policy ingestiontime",
                PolicyResult);

        public static readonly CommandInfo ShowTableStarPolicyIngestionTime =
            new CommandInfo(nameof(ShowTableStarPolicyIngestionTime),
                "show table '*' policy ingestiontime",
                PolicyResult);

        public static readonly CommandInfo AlterTablePolicyIngestionTime =
            new CommandInfo(nameof(AlterTablePolicyIngestionTime),
                "alter table TableName=<database_table> policy ingestiontime true",
                PolicyResult);

        public static readonly CommandInfo AlterTablesPolicyIngestionTime =
            new CommandInfo(nameof(AlterTablesPolicyIngestionTime),
                "alter tables '(' { TableName=<table>, ',' }+ ')' policy ingestiontime true",
                PolicyResult);

        public static readonly CommandInfo DeleteTablePolicyIngestionTime =
            new CommandInfo(nameof(DeleteTablePolicyIngestionTime),
                "delete table TableName=<database_table> policy ingestiontime",
                PolicyResult);
        #endregion

        #region Retention
        public static readonly CommandInfo ShowTablePolicyRetention =
            new CommandInfo(nameof(ShowTablePolicyRetention),
                "show table TableName=<database_table> policy retention",
                PolicyResult);

        public static readonly CommandInfo ShowTableStarPolicyRetention =
            new CommandInfo(nameof(ShowTableStarPolicyRetention),
                "show table '*' policy retention",
                PolicyResult);

        public static readonly CommandInfo ShowGraphPolicyRetention =
            new CommandInfo(nameof(ShowGraphPolicyRetention),
                "show graph_model ModelName=<graph_model> policy retention",
                PolicyResult);

        public static readonly CommandInfo ShowGraphStarPolicyRetention =
            new CommandInfo(nameof(ShowGraphStarPolicyRetention),
                "show graph_model '*' policy retention",
                PolicyResult);

        public static readonly CommandInfo ShowDatabasePolicyRetention =
            new CommandInfo(nameof(ShowDatabasePolicyRetention),
                "show database [DatabaseName=(<database> | '*')] policy retention",
                PolicyResult);

        public static readonly CommandInfo AlterTablePolicyRetention =
            new CommandInfo(nameof(AlterTablePolicyRetention),
                "alter table TableName=<database_table> policy retention RetentionPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMaterializedViewPolicyRetention =
            new CommandInfo(nameof(AlterMaterializedViewPolicyRetention),
                "alter materialized-view MaterializedViewName=<database_materializedview> policy retention RetentionPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterDatabasePolicyRetention =
            new CommandInfo(nameof(AlterDatabasePolicyRetention),
                "alter database [DatabaseName=<database>] policy retention RetentionPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterGraphModelPolicyRetention =
            new CommandInfo(nameof(AlterGraphModelPolicyRetention),
                "alter graph_model ModelName=<graph_model> policy retention RetentionPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterTablesPolicyRetention =
            new CommandInfo(nameof(AlterTablesPolicyRetention),
                "alter tables '(' { TableName=<table>, ',' }+ ')' policy retention RetentionPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeTablePolicyRetention =
            new CommandInfo(nameof(AlterMergeTablePolicyRetention),
                "alter-merge table TableName=<database_table> policy retention (RetentionPolicy=<string> | softdelete '=' SoftDeleteValue=<timespan> [recoverability '=' RecoverabilityValue=(disabled|enabled)] | recoverability '=' RecoverabilityValue=(disabled|enabled))",
                PolicyResult);

        public static readonly CommandInfo AlterMergeMaterializedViewPolicyRetention =
            new CommandInfo(nameof(AlterMergeMaterializedViewPolicyRetention),
                "alter-merge materialized-view MaterializedViewName=<database_materializedview> policy retention (RetentionPolicy=<string> | softdelete '=' SoftDeleteValue=<timespan> [recoverability '=' RecoverabilityValue=(disabled|enabled)] | recoverability '=' RecoverabilityValue=(disabled|enabled))",
                PolicyResult);

        public static readonly CommandInfo AlterMergeDatabasePolicyRetention =
            new CommandInfo(nameof(AlterMergeDatabasePolicyRetention),
                "alter-merge database [DatabaseName=<database>] policy retention (RetentionPolicy=<string> | softdelete '=' SoftDeleteValue=<timespan> [recoverability '=' RecoverabilityValue=(disabled|enabled)] | recoverability '=' RecoverabilityValue=(disabled|enabled))",
                PolicyResult);

        public static readonly CommandInfo DeleteTablePolicyRetention =
            new CommandInfo(nameof(DeleteTablePolicyRetention),
                "delete table TableName=<database_table> policy retention",
                PolicyResult);

        public static readonly CommandInfo DeleteDatabasePolicyRetention =
            new CommandInfo(nameof(DeleteDatabasePolicyRetention),
                "delete database DatabaseName=<database> policy retention",
                PolicyResult);

        public static readonly CommandInfo ShowDatabasePolicyHardRetentionViolations =
            new CommandInfo(nameof(ShowDatabasePolicyHardRetentionViolations),
                "show database databaseName=<database> policy hardretention violations",
                UnknownResult);

        public static readonly CommandInfo ShowDatabasePolicySoftRetentionViolations =
            new CommandInfo(nameof(ShowDatabasePolicySoftRetentionViolations),
                "show database databaseName=<database> policy softretention violations",
                UnknownResult);

        #endregion

        #region RowLevelSecurity
        public static readonly CommandInfo ShowTablePolicyRowLevelSecurity =
            new CommandInfo(nameof(ShowTablePolicyRowLevelSecurity),
                "show table TableName=<database_table> policy row_level_security",
                PolicyResult);

        public static readonly CommandInfo ShowTableStarPolicyRowLevelSecurity =
            new CommandInfo(nameof(ShowTableStarPolicyRowLevelSecurity),
                "show table '*' policy row_level_security",
                PolicyResult);

        public static readonly CommandInfo AlterTablePolicyRowLevelSecurity =
            new CommandInfo(nameof(AlterTablePolicyRowLevelSecurity),
                "alter table TableName=<database_table> policy row_level_security (enable | disable) [with '(' { PropertyName=<name> '=' Value=<value>, ',' } ')'] Query=<string>",
                PolicyResult);

        public static readonly CommandInfo DeleteTablePolicyRowLevelSecurity =
            new CommandInfo(nameof(DeleteTablePolicyRowLevelSecurity),
                "delete table TableName=<database_table> policy row_level_security",
                PolicyResult);

        public static readonly CommandInfo ShowMaterializedViewPolicyRowLevelSecurity =
            new CommandInfo(nameof(ShowMaterializedViewPolicyRowLevelSecurity),
                "show materialized-view MaterializedViewName=<database_materializedview> policy row_level_security",
                PolicyResult);

        public static readonly CommandInfo AlterMaterializedViewPolicyRowLevelSecurity =
            new CommandInfo(nameof(AlterMaterializedViewPolicyRowLevelSecurity),
                "alter materialized-view MaterializedViewName=<database_materializedview> policy row_level_security (enable | disable) Query=<string>",
                PolicyResult);

        public static readonly CommandInfo DeleteMaterializedViewPolicyRowLevelSecurity =
            new CommandInfo(nameof(DeleteMaterializedViewPolicyRowLevelSecurity),
                "delete materialized-view MaterializedViewName=<database_materializedview> policy row_level_security",
                PolicyResult);
        #endregion

        #region RowOrder
        public static readonly CommandInfo ShowTablePolicyRowOrder =
            new CommandInfo(nameof(ShowTablePolicyRowOrder),
                "show table TableName=<database_table> policy roworder",
                PolicyResult);

        public static readonly CommandInfo ShowTableStarPolicyRowOrder =
            new CommandInfo(nameof(ShowTableStarPolicyRowOrder),
                "show table '*' policy roworder",
                PolicyResult);

        public static readonly CommandInfo AlterTablePolicyRowOrder =
            new CommandInfo(nameof(AlterTablePolicyRowOrder),
                "alter table TableName=<database_table> policy roworder '(' { ColumnName=<column> (asc|desc), ',' }+ ')'",
                PolicyResult);

        public static readonly CommandInfo AlterTablesPolicyRowOrder =
            new CommandInfo(nameof(AlterTablesPolicyRowOrder),
                "alter tables '(' { TableName=<table>, ',' }+ ')' policy roworder '(' { ColumnName=<name> (asc|desc), ',' }+ ')'",
                PolicyResult);

        public static readonly CommandInfo AlterMergeTablePolicyRowOrder =
            new CommandInfo(nameof(AlterMergeTablePolicyRowOrder),
                "alter-merge table TableName=<database_table> policy roworder '(' { ColumnName=<column> (asc|desc), ',' }+ ')'",
                PolicyResult);

        public static readonly CommandInfo DeleteTablePolicyRowOrder =
            new CommandInfo(nameof(DeleteTablePolicyRowOrder),
                "delete table TableName=<database_table> policy roworder",
                PolicyResult);
        #endregion

        #region Update
        public static readonly CommandInfo ShowTablePolicyUpdate =
            new CommandInfo(nameof(ShowTablePolicyUpdate),
                "show table TableName=<database_table> policy update",
                PolicyResult);

        public static readonly CommandInfo ShowTableStarPolicyUpdate =
            new CommandInfo(nameof(ShowTableStarPolicyUpdate),
                "show table '*' policy update",
                PolicyResult);

        public static readonly CommandInfo AlterTablePolicyUpdate =
            new CommandInfo(nameof(AlterTablePolicyUpdate),
                $"alter table TableName=<database_table> policy update UpdatePolicy=<string> [{PropertyList()}]",
                PolicyResult);

        public static readonly CommandInfo AlterMergeTablePolicyUpdate =
            new CommandInfo(nameof(AlterMergeTablePolicyUpdate),
                $"alter-merge table TableName=<database_table> policy update UpdatePolicy=<string> [{PropertyList()}]",
                PolicyResult);

        public static readonly CommandInfo DeleteTablePolicyUpdate =
            new CommandInfo(nameof(DeleteTablePolicyUpdate),
                "delete table TableName=<database_table> policy update",
                PolicyResult);
        #endregion

        #region IngestionBatching
        public static readonly CommandInfo ShowClusterPolicyIngestionBatching =
            new CommandInfo(nameof(ShowClusterPolicyIngestionBatching),
                "show cluster policy ingestionbatching",
                PolicyResult);

        public static readonly CommandInfo ShowDatabasePolicyIngestionBatching =
            new CommandInfo(nameof(ShowDatabasePolicyIngestionBatching),
                "show database [DatabaseName=(<database> | '*')] policy ingestionbatching",
                PolicyResult);

        public static readonly CommandInfo ShowTablePolicyIngestionBatching =
            new CommandInfo(nameof(ShowTablePolicyIngestionBatching),
                "show table TableName=<database_table> policy ingestionbatching",
                PolicyResult);

        public static readonly CommandInfo ShowTableStarPolicyIngestionBatching =
            new CommandInfo(nameof(ShowTableStarPolicyIngestionBatching),
                "show table '*' policy ingestionbatching",
                PolicyResult);

        public static readonly CommandInfo AlterClusterPolicyIngestionBatching =
            new CommandInfo(nameof(AlterClusterPolicyIngestionBatching),
                "alter cluster policy ingestionbatching IngestionBatchingPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeClusterPolicyIngestionBatching =
            new CommandInfo(nameof(AlterMergeClusterPolicyIngestionBatching),
                "alter-merge cluster policy ingestionbatching IngestionBatchingPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterDatabasePolicyIngestionBatching =
            new CommandInfo(nameof(AlterDatabasePolicyIngestionBatching),
                "alter database [DatabaseName=<database>] policy ingestionbatching IngestionBatchingPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeDatabasePolicyIngestionBatching =
            new CommandInfo(nameof(AlterMergeDatabasePolicyIngestionBatching),
                "alter-merge database [DatabaseName=<database>] policy ingestionbatching IngestionBatchingPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterTablePolicyIngestionBatching =
            new CommandInfo(nameof(AlterTablePolicyIngestionBatching),
                "alter table TableName=<database_table> policy ingestionbatching IngestionBatchingPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeTablePolicyIngestionBatching =
            new CommandInfo(nameof(AlterMergeTablePolicyIngestionBatching),
                "alter-merge table TableName=<database_table> policy ingestionbatching IngestionBatchingPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterTablesPolicyIngestionBatching =
            new CommandInfo(nameof(AlterTablesPolicyIngestionBatching),
                "alter tables '(' { TableName=<table>, ',' }+ ')' policy ingestionbatching IngestionBatchingPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo DeleteDatabasePolicyIngestionBatching =
            new CommandInfo(nameof(DeleteDatabasePolicyIngestionBatching),
                "delete database DatabaseName=<database> policy ingestionbatching",
                PolicyResult);

        public static readonly CommandInfo DeleteTablePolicyIngestionBatching =
            new CommandInfo(nameof(DeleteTablePolicyIngestionBatching),
                "delete table TableName=<database_table> policy ingestionbatching",
                PolicyResult);
        #endregion

        #region Encoding
        public static readonly CommandInfo ShowDatabasePolicyEncoding =
            new CommandInfo(nameof(ShowDatabasePolicyEncoding),
                "show database DatabaseName=<database> policy encoding",
                PolicyResult);

        public static readonly CommandInfo ShowTablePolicyEncoding =
            new CommandInfo(nameof(ShowTablePolicyEncoding),
                "show table TableName=<database_table> policy encoding",
                PolicyResult);

        public static readonly CommandInfo ShowColumnPolicyEncoding =
            new CommandInfo(nameof(ShowColumnPolicyEncoding),
                "show column ColumnName=<table_column> policy encoding",
                PolicyResult);

        public static readonly CommandInfo AlterDatabasePolicyEncoding =
            new CommandInfo(nameof(AlterDatabasePolicyEncoding),
                "alter database [DatabaseName=<database>] policy encoding EncodingPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterTablePolicyEncoding =
            new CommandInfo(nameof(AlterTablePolicyEncoding),
                "alter table TableName=<database_table> policy encoding EncodingPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterTableColumnsPolicyEncoding =
            new CommandInfo(nameof(AlterTableColumnsPolicyEncoding),
                "alter table TableName=<table> columns policy encoding EncodingPolicies=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterColumnPolicyEncoding =
            new CommandInfo(nameof(AlterColumnPolicyEncoding),
                "alter column ColumnName=<database_table_column> policy encoding EncodingPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterColumnsPolicyEncodingByQuery =
            new CommandInfo(nameof(AlterColumnsPolicyEncodingByQuery),
                "alter columns policy encoding EncodingPolicy=<string> '<|' QueryOrCommand=<input_query>",
                PolicyResult);

        public static readonly CommandInfo AlterColumnPolicyEncodingType =
            new CommandInfo(nameof(AlterColumnPolicyEncodingType),
                "alter column ColumnName=<database_table_column> policy encoding type '=' EncodingPolicyType=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeDatabasePolicyEncoding =
            new CommandInfo(nameof(AlterMergeDatabasePolicyEncoding),
                "alter-merge database [DatabaseName=<database>] policy encoding EncodingPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeTablePolicyEncoding =
            new CommandInfo(nameof(AlterMergeTablePolicyEncoding),
                "alter-merge table TableName=<database_table> policy encoding EncodingPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeColumnPolicyEncoding =
            new CommandInfo(nameof(AlterMergeColumnPolicyEncoding),
                "alter-merge column ColumnName=<table_column> policy encoding EncodingPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo DeleteDatabasePolicyEncoding =
            new CommandInfo(nameof(DeleteDatabasePolicyEncoding),
                "delete database DatabaseName=<database> policy encoding",
                PolicyResult);

        public static readonly CommandInfo DeleteTablePolicyEncoding =
            new CommandInfo(nameof(DeleteTablePolicyEncoding),
                "delete table TableName=<database_table> policy encoding",
                PolicyResult);

        public static readonly CommandInfo DeleteColumnPolicyEncoding =
            new CommandInfo(nameof(DeleteColumnPolicyEncoding),
                "delete column ColumnName=<table_column> policy encoding",
                PolicyResult);
        #endregion

        #region DMR Policy
        //public static readonly CommandInfo AlterColumnPolicyDataMovementRestriction =
        //    new CommandInfo(nameof(AlterColumnPolicyDataMovementRestriction),
        //        "alter column ColumnName=<database_table_column> policy data_movement_restriction with label '=' Label=<string>",
        //        PolicyResult);

        //public static readonly CommandInfo ShowColumnPolicyDataMovementRestriction =
        //    new CommandInfo(nameof(ShowColumnPolicyDataMovementRestriction),
        //        "show column ColumnName=<database_table_column> policy data_movement_restriction",
        //        PolicyResult);

        //public static readonly CommandInfo DropColumnPolicyDataMovementRestriction =
        //    new CommandInfo(nameof(DropColumnPolicyDataMovementRestriction),
        //        "drop column ColumnName=<database_table_column> policy data_movement_restriction",
        //        PolicyResult);
        #endregion

        #region Merge
        public static readonly CommandInfo ShowDatabasePolicyMerge =
            new CommandInfo(nameof(ShowDatabasePolicyMerge),
                "show database [DatabaseName=(<database> | '*')] policy merge",
                PolicyResult);

        public static readonly CommandInfo ShowTablePolicyMerge =
            new CommandInfo(nameof(ShowTablePolicyMerge),
                "show table TableName=<database_table> policy merge",
                PolicyResult);

        public static readonly CommandInfo ShowTableStarPolicyMerge =
            new CommandInfo(nameof(ShowTableStarPolicyMerge),
                "show table '*' policy merge",
                PolicyResult);

        public static readonly CommandInfo AlterDatabasePolicyMerge =
            new CommandInfo(nameof(AlterDatabasePolicyMerge),
                "alter database [DatabaseName=<database>] policy merge MergePolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterTablePolicyMerge =
            new CommandInfo(nameof(AlterTablePolicyMerge),
                "alter table TableName=<database_table> policy merge MergePolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterTablesPolicyMerge =
            new CommandInfo(nameof(AlterTablesPolicyMerge),
                "alter tables '(' {TableName=<table>, ','}+ ')' policy merge policy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeDatabasePolicyMerge =
            new CommandInfo(nameof(AlterMergeDatabasePolicyMerge),
                "alter-merge database [DatabaseName=<database>] policy merge MergePolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeTablePolicyMerge =
            new CommandInfo(nameof(AlterMergeTablePolicyMerge),
                "alter-merge table TableName=<database_table> policy merge MergePolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeMaterializedViewPolicyMerge =
            new CommandInfo(nameof(AlterMergeMaterializedViewPolicyMerge),
                "alter-merge materialized-view MaterializedViewName=<materializedview> policy merge MergePolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo DeleteDatabasePolicyMerge =
            new CommandInfo(nameof(DeleteDatabasePolicyMerge),
                "delete database DatabaseName=<database> policy merge",
                PolicyResult);

        public static readonly CommandInfo DeleteTablePolicyMerge =
            new CommandInfo(nameof(DeleteTablePolicyMerge),
                "delete table TableName=<database_table> policy merge",
                PolicyResult);
        #endregion

        #region Cluster Level DMR Policy
        //public static readonly CommandInfo AlterClusterPolicyDataMovementRestriction =
        //    new CommandInfo(nameof(AlterClusterPolicyDataMovementRestriction),
        //        "alter cluster policy data_movement_restriction with '(' { label=<string>, ',' }+ ')'",
        //        PolicyResult);

        //public static readonly CommandInfo ShowClusterPolicyDataMovementRestriction =
        //    new CommandInfo(nameof(ShowClusterPolicyDataMovementRestriction),
        //        "show cluster policy data_movement_restriction",
        //        PolicyResult);

        //public static readonly CommandInfo DropClusterPolicyDataMovementRestriction =
        //    new CommandInfo(nameof(DropClusterPolicyDataMovementRestriction),
        //        "delete cluster policy data_movement_restriction",
        //        PolicyResult);
        #endregion

        #region Query Acceleration
        public static readonly CommandInfo ShowExternalTablePolicyQueryAcceleration =
            new CommandInfo(nameof(ShowExternalTablePolicyQueryAcceleration),
                "show external table ExternalTableName=<externaltable> policy query_acceleration",
                PolicyResult);

        public static readonly CommandInfo ShowExternalTablesPolicyQueryAcceleration =
            new CommandInfo(nameof(ShowExternalTablesPolicyQueryAcceleration),
                "show external table '*' policy query_acceleration",
                PolicyResult);

        public static readonly CommandInfo AlterExternalTablePolicyQueryAcceleration =
            new CommandInfo(nameof(AlterExternalTablePolicyQueryAcceleration),
                "alter external table ExternalTableName=<externaltable> policy query_acceleration Policy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeExternalTablePolicyQueryAcceleration =
            new CommandInfo(nameof(AlterMergeExternalTablePolicyQueryAcceleration),
                "alter-merge external table ExternalTableName=<externaltable> policy query_acceleration Policy=<string>",
                PolicyResult);

        public static readonly CommandInfo DeleteExternalTablePolicyQueryAcceleration =
            new CommandInfo(nameof(DeleteExternalTablePolicyQueryAcceleration),
                "delete external table ExternalTableName=<externaltable> policy query_acceleration",
                PolicyResult);

        private static readonly string ShowExternalTableQueryAccelerationStatisticsResult =
            "(ExternalTableName: string, IsEnabled: bool, Hot: timespan, HotSize: long, LastUpdatedDateTime: datetime, AccelerationPendingDataFilesCount:long, AccelerationPendingDataFilesSize:long, AccelerationCompletePercentage: double, NotHealthyReason: string)";

        public static readonly CommandInfo ShowExternalTableQueryAccelerationStatatistics =
            new CommandInfo(nameof(ShowExternalTableQueryAccelerationStatatistics),
                "show external table ExternalTableName=<externaltable> operations query_acceleration statistics",
                ShowExternalTableQueryAccelerationStatisticsResult);

        public static readonly CommandInfo ShowExternalTablesQueryAccelerationStatatistics =
            new CommandInfo(nameof(ShowExternalTablesQueryAccelerationStatatistics),
                "show external tables operations query_acceleration statistics",
                ShowExternalTableQueryAccelerationStatisticsResult);
        #endregion

        #region Mirroring
        private static readonly string AlterTablePolicyMirroringGrammar =
            @"[partition by
            '('
            {PartitionName=<name> ':'
                (PartitionType=string '=' StringColumn=<name>
                | PartitionType=datetime '='
                    (PartitionFunction=bin '(' DateTimeColumn=<name> ',' BinValue=<timespan> ')'
                    | PartitionFunction=(startofday | startofweek | startofmonth | startofyear) '(' DateTimeColumn=<name> ')')
                ), ','}+
            ')'
            [pathformat '=' '('
                [PathSeparator=<string>]
                { (PartitionName=<name> | datetime_pattern '(' DateTimeFormat=<string> ',' PartitionName=<name> ')')
                 [PathSeparator=<string>] }+ ')']
            ]
            kind '=' KindType=(delta)
            ['(' { StorageConnectionString=<string>, ',' }+ ')']
            [with '(' { PropertyName=<name> '=' Value=<value>, ',' }+ ')']";

        public static readonly CommandInfo AlterTablePolicyMirroring =
            new CommandInfo(nameof(AlterTablePolicyMirroring),
                "alter table TableName=<database_table> policy mirroring " + AlterTablePolicyMirroringGrammar,
                PolicyResult);

        public static readonly CommandInfo AlterMergeTablePolicyMirroring =
            new CommandInfo(nameof(AlterMergeTablePolicyMirroring),
                "alter-merge table TableName=<database_table> policy mirroring " + AlterTablePolicyMirroringGrammar,
                PolicyResult);

        public static readonly CommandInfo AlterTablePolicyMirroringWithJson =
            new CommandInfo(nameof(AlterTablePolicyMirroringWithJson),
                "alter table TableName=<database_table> policy mirroring Policy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeTablePolicyMirroringWithJson =
            new CommandInfo(nameof(AlterMergeTablePolicyMirroringWithJson),
                "alter-merge table TableName=<database_table> policy mirroring Policy=<string>",
                PolicyResult);

        public static readonly CommandInfo DeleteTablePolicyMirroring =
            new CommandInfo(nameof(DeleteTablePolicyMirroring),
                "delete table TableName=<database_table> policy mirroring",
                PolicyResult);

        public static readonly CommandInfo ShowTablePolicyMirroring =
            new CommandInfo(nameof(ShowTablePolicyMirroring),
                "show table TableName=<database_table> policy mirroring",
                PolicyResult);

        public static readonly CommandInfo ShowTableStarPolicyMirroring =
            new CommandInfo(nameof(ShowTableStarPolicyMirroring),
                "show table '*' policy mirroring",
                PolicyResult);

        private static readonly string ShowMirroringTemplatesResult = "(Name: string, Kind: string, ConnectionString: string, IsEnabled: bool, AutoApplyToNewTables: bool)";

        private static readonly string CreateOrAlterMirroringTemplateGrammar =
            @"mirroring-template TemplateName=<name>
              [kind '=' KindType=(delta)]
              ['(' ConnectionString=<string> ')']
              [with '(' { (AutoApplyToNewTables | IsEnabled | Backfill) '=' (true | false), ',' }+ ')']";

        public static readonly CommandInfo CreateMirroringTemplate = 
            new CommandInfo(nameof(CreateMirroringTemplate),
                "create " + CreateOrAlterMirroringTemplateGrammar,
                ShowMirroringTemplatesResult);

        public static readonly CommandInfo CreateOrAlterMirroringTemplate =
            new CommandInfo(nameof(CreateOrAlterMirroringTemplate),
                "create-or-alter " + CreateOrAlterMirroringTemplateGrammar,
                ShowMirroringTemplatesResult);

        public static readonly CommandInfo AlterMirroringTemplate =
            new CommandInfo(nameof(AlterMirroringTemplate),
                "alter " + CreateOrAlterMirroringTemplateGrammar,
                ShowMirroringTemplatesResult);

        public static readonly CommandInfo AlterMergeMirroringTemplate =
            new CommandInfo(nameof(AlterMergeMirroringTemplate),
                "alter-merge " + CreateOrAlterMirroringTemplateGrammar,
                ShowMirroringTemplatesResult);

        public static readonly CommandInfo DeleteMirroringTemplate = 
            new CommandInfo(nameof(DeleteMirroringTemplate),
                "delete mirroring-template TemplateName=<name>",
                ShowMirroringTemplatesResult);

        public static readonly CommandInfo ShowMirroringTemplate =
            new CommandInfo(nameof(ShowMirroringTemplate),
                "show mirroring-template TemplateName=<name>",
                ShowMirroringTemplatesResult);

        public static readonly CommandInfo ShowMirroringTemplates =
            new CommandInfo(nameof(ShowMirroringTemplates),
                "show mirroring-templates",
                ShowMirroringTemplatesResult);

        public static readonly CommandInfo ApplyMirroringTemplate =
            new CommandInfo(nameof(ApplyMirroringTemplate),
                @"apply mirroring-template TemplateName=<name>
                        ('<|' Query=<input_query> | { TableName=<table>, ',' }+)",
                ShowMirroringTemplatesResult);

        #endregion

        #region Partitioning
        public static readonly CommandInfo ShowTablePolicyPartitioning =
            new CommandInfo(nameof(ShowTablePolicyPartitioning),
                "show table TableName=<database_table> policy partitioning",
                PolicyResult);

        public static readonly CommandInfo ShowTableStarPolicyPartitioning =
            new CommandInfo(nameof(ShowTableStarPolicyPartitioning),
                "show table '*' policy partitioning",
                PolicyResult);

        public static readonly CommandInfo AlterTablePolicyPartitioning =
            new CommandInfo(nameof(AlterTablePolicyPartitioning),
                "alter table TableName=<database_table> policy partitioning Policy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeTablePolicyPartitioning =
            new CommandInfo(nameof(AlterMergeTablePolicyPartitioning),
                "alter-merge table TableName=<database_table> policy partitioning Policy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMaterializedViewPolicyPartitioning =
            new CommandInfo(nameof(AlterMaterializedViewPolicyPartitioning),
                "alter materialized-view MaterializedViewName=<database_materializedview> policy partitioning Policy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeMaterializedViewPolicyPartitioning =
            new CommandInfo(nameof(AlterMergeMaterializedViewPolicyPartitioning),
                "alter-merge materialized-view MaterializedViewName=<database_materializedview> policy partitioning Policy=<string>",
                PolicyResult);

        public static readonly CommandInfo DeleteTablePolicyPartitioning =
            new CommandInfo(nameof(DeleteTablePolicyPartitioning),
                "delete table TableName=<database_table> policy partitioning",
                PolicyResult);

        public static readonly CommandInfo DeleteMaterializedViewPolicyPartitioning =
            new CommandInfo(nameof(DeleteMaterializedViewPolicyPartitioning),
                "delete materialized-view MaterializedViewName=<database_materializedview> policy partitioning",
                PolicyResult);
        #endregion

        #region RestrictedViewAccess
        public static readonly CommandInfo ShowTablePolicyRestrictedViewAccess =
            new CommandInfo(nameof(ShowTablePolicyRestrictedViewAccess),
                "show table TableName=<database_table> policy restricted_view_access",
                PolicyResult);

        public static readonly CommandInfo ShowTableStarPolicyRestrictedViewAccess =
            new CommandInfo(nameof(ShowTableStarPolicyRestrictedViewAccess),
                "show table '*' policy restricted_view_access",
                PolicyResult);

        public static readonly CommandInfo AlterTablePolicyRestrictedViewAccess =
            new CommandInfo(nameof(AlterTablePolicyRestrictedViewAccess),
                "alter table TableName=<database_table> policy restricted_view_access (true | false)",
                PolicyResult);

        public static readonly CommandInfo AlterTablesPolicyRestrictedViewAccess =
            new CommandInfo(nameof(AlterTablesPolicyRestrictedViewAccess),
                "alter tables '(' { TableName=<table>, ',' }+ ')' policy restricted_view_access (true | false)",
                PolicyResult);

        public static readonly CommandInfo DeleteTablePolicyRestrictedViewAccess =
            new CommandInfo(nameof(DeleteTablePolicyRestrictedViewAccess),
                "delete table TableName=<database_table> policy restricted_view_access",
                PolicyResult);
        #endregion

        #region RowStore
        public static readonly CommandInfo ShowClusterPolicyRowStore =
            new CommandInfo(nameof(ShowClusterPolicyRowStore),
                "show cluster policy rowstore",
                PolicyResult);

        public static readonly CommandInfo AlterClusterPolicyRowStore =
            new CommandInfo(nameof(AlterClusterPolicyRowStore),
                "alter cluster policy rowstore RowStorePolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeClusterPolicyRowStore =
            new CommandInfo(nameof(AlterMergeClusterPolicyRowStore),
                "alter-merge cluster policy rowstore RowStorePolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo DeleteClusterPolicyRowStore =
            new CommandInfo(nameof(DeleteClusterPolicyRowStore),
                "delete cluster policy rowstore",
                PolicyResult);
        #endregion

        #region Sandbox
        public static readonly CommandInfo ShowClusterPolicySandbox =
            new CommandInfo(nameof(ShowClusterPolicySandbox),
                "show cluster policy sandbox",
                PolicyResult);

        public static readonly CommandInfo AlterClusterPolicySandbox =
            new CommandInfo(nameof(AlterClusterPolicySandbox),
                "alter cluster policy sandbox SandboxPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo DeleteClusterPolicySandbox =
            new CommandInfo(nameof(DeleteClusterPolicySandbox),
                "delete cluster policy sandbox",
                PolicyResult);

        public static readonly CommandInfo ShowClusterSandboxesStats =
            new CommandInfo(nameof(ShowClusterSandboxesStats),
                "show cluster sandboxes stats",
                PolicyResult);

        #endregion

        #region Sharding
        public static readonly CommandInfo ShowDatabasePolicySharding =
            new CommandInfo(nameof(ShowDatabasePolicySharding),
                "show database [DatabaseName=(<database> | '*')] policy sharding",
                PolicyResult);

        public static readonly CommandInfo ShowTablePolicySharding =
            new CommandInfo(nameof(ShowTablePolicySharding),
                "show table TableName=<database_table> policy sharding",
                PolicyResult);

        public static readonly CommandInfo ShowTableStarPolicySharding =
            new CommandInfo(nameof(ShowTableStarPolicySharding),
                "show table '*' policy sharding",
                PolicyResult);

        public static readonly CommandInfo AlterDatabasePolicySharding =
            new CommandInfo(nameof(AlterDatabasePolicySharding),
                "alter database [DatabaseName=<database>] policy sharding ShardingPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterTablePolicySharding =
            new CommandInfo(nameof(AlterTablePolicySharding),
                "alter table TableName=<database_table> policy sharding ShardingPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeDatabasePolicySharding =
            new CommandInfo(nameof(AlterMergeDatabasePolicySharding),
                "alter-merge database [DatabaseName=<database>] policy sharding ShardingPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeTablePolicySharding =
            new CommandInfo(nameof(AlterMergeTablePolicySharding),
                "alter-merge table TableName=<database_table> policy sharding ShardingPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo DeleteDatabasePolicySharding =
            new CommandInfo(nameof(DeleteDatabasePolicySharding),
                "delete database DatabaseName=<database> policy sharding",
                PolicyResult);

        public static readonly CommandInfo DeleteTablePolicySharding =
            new CommandInfo(nameof(DeleteTablePolicySharding),
                "delete table TableName=<database_table> policy sharding",
                PolicyResult);

        public static readonly CommandInfo AlterClusterPolicySharding =
            new CommandInfo(nameof(AlterClusterPolicySharding),
                "alter cluster policy sharding policy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeClusterPolicySharding =
            new CommandInfo(nameof(AlterMergeClusterPolicySharding),
                "alter-merge cluster policy sharding policy=<string>",
                PolicyResult);

        public static readonly CommandInfo DeleteClusterPolicySharding =
            new CommandInfo(nameof(DeleteClusterPolicySharding),
                "delete cluster policy sharding",
                PolicyResult);

        public static readonly CommandInfo ShowClusterPolicySharding =
            new CommandInfo(nameof(ShowClusterPolicySharding),
                $"show cluster policy sharding [{PropertyList()}]",
                PolicyResult);
        #endregion

        #region ShardsGrouping
        public static readonly CommandInfo ShowDatabasePolicyShardsGrouping =
            new CommandInfo(nameof(ShowDatabasePolicyShardsGrouping),
                "show database [DatabaseName=(<database> | '*')] policy #shards_grouping",
                PolicyResult);
        public static readonly CommandInfo AlterDatabasePolicyShardsGrouping =
            new CommandInfo(nameof(AlterDatabasePolicyShardsGrouping),
                "alter database [DatabaseName=<database>] policy #shards_grouping ShardsGroupingPolicy=<string>",
                PolicyResult);
        public static readonly CommandInfo AlterMergeDatabasePolicyShardsGrouping =
            new CommandInfo(nameof(AlterMergeDatabasePolicyShardsGrouping),
                "alter-merge database [DatabaseName=<database>] policy #shards_grouping ShardsGroupingPolicy=<string>",
                PolicyResult);
        public static readonly CommandInfo DeleteDatabasePolicyShardsGrouping =
            new CommandInfo(nameof(DeleteDatabasePolicyShardsGrouping),
                "delete database DatabaseName=<database> policy #shards_grouping",
                PolicyResult);
        #endregion

        #region StreamingIngestion
        public static readonly CommandInfo ShowDatabasePolicyStreamingIngestion =
            new CommandInfo(nameof(ShowDatabasePolicyStreamingIngestion),
                "show database DatabaseName=<database> policy streamingingestion",
                PolicyResult);

        public static readonly CommandInfo ShowTablePolicyStreamingIngestion =
            new CommandInfo(nameof(ShowTablePolicyStreamingIngestion),
                "show table TableName=<database_table> policy streamingingestion",
                PolicyResult);

        public static readonly CommandInfo ShowClusterPolicyStreamingIngestion =
            new CommandInfo(nameof(ShowClusterPolicyStreamingIngestion),
                "show cluster policy streamingingestion",
                PolicyResult);

        public static readonly CommandInfo AlterDatabasePolicyStreamingIngestion =
            new CommandInfo(nameof(AlterDatabasePolicyStreamingIngestion),
                "alter database [DatabaseName=<database>] policy streamingingestion (StreamingIngestionPolicy=<string>|Status=(enable|disable))",
                PolicyResult);

        public static readonly CommandInfo AlterMergeDatabasePolicyStreamingIngestion =
            new CommandInfo(nameof(AlterMergeDatabasePolicyStreamingIngestion),
                "alter-merge database [DatabaseName=<database>] policy streamingingestion StreamingIngestionPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterTablePolicyStreamingIngestion =
            new CommandInfo(nameof(AlterTablePolicyStreamingIngestion),
                "alter table TableName=<database_table> policy streamingingestion (StreamingIngestionPolicy=<string>|Status=(enable|disable))",
                PolicyResult);

        public static readonly CommandInfo AlterMergeTablePolicyStreamingIngestion =
            new CommandInfo(nameof(AlterMergeTablePolicyStreamingIngestion),
                "alter-merge table TableName=<database_table> policy streamingingestion StreamingIngestionPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterClusterPolicyStreamingIngestion =
            new CommandInfo(nameof(AlterClusterPolicyStreamingIngestion),
                "alter cluster policy streamingingestion StreamingIngestionPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeClusterPolicyStreamingIngestion =
            new CommandInfo(nameof(AlterMergeClusterPolicyStreamingIngestion),
                "alter-merge cluster policy streamingingestion policy=<string>",
                PolicyResult);

        public static readonly CommandInfo DeleteDatabasePolicyStreamingIngestion =
            new CommandInfo(nameof(DeleteDatabasePolicyStreamingIngestion),
                "delete database DatabaseName=<database> policy streamingingestion",
                PolicyResult);

        public static readonly CommandInfo DeleteTablePolicyStreamingIngestion =
            new CommandInfo(nameof(DeleteTablePolicyStreamingIngestion),
                "delete table TableName=<database_table> policy streamingingestion",
                PolicyResult);

        public static readonly CommandInfo DeleteClusterPolicyStreamingIngestion =
            new CommandInfo(nameof(DeleteClusterPolicyStreamingIngestion),
                "delete cluster policy streamingingestion",
                PolicyResult);
        #endregion

        #region ManagedIdentity
        public static readonly CommandInfo ShowDatabasePolicyManagedIdentity =
            new CommandInfo(nameof(ShowDatabasePolicyManagedIdentity),
                "show database DatabaseName=<database> policy managed_identity",
                PolicyResult);

        public static readonly CommandInfo ShowClusterPolicyManagedIdentity =
            new CommandInfo(nameof(ShowClusterPolicyManagedIdentity),
                "show cluster policy managed_identity",
                PolicyResult);

        public static readonly CommandInfo AlterDatabasePolicyManagedIdentity =
            new CommandInfo(nameof(AlterDatabasePolicyManagedIdentity),
                "alter database [DatabaseName=<database>] policy managed_identity ManagedIdentityPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeDatabasePolicyManagedIdentity =
            new CommandInfo(nameof(AlterMergeDatabasePolicyManagedIdentity),
                "alter-merge database [DatabaseName=<database>] policy managed_identity ManagedIdentityPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterClusterPolicyManagedIdentity =
            new CommandInfo(nameof(AlterClusterPolicyManagedIdentity),
                "alter cluster policy managed_identity ManagedIdentityPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeClusterPolicyManagedIdentity =
            new CommandInfo(nameof(AlterMergeClusterPolicyManagedIdentity),
                "alter-merge cluster policy managed_identity ManagedIdentityPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo DeleteDatabasePolicyManagedIdentity =
            new CommandInfo(nameof(DeleteDatabasePolicyManagedIdentity),
                "delete database DatabaseName=<database> policy managed_identity",
                PolicyResult);

        public static readonly CommandInfo DeleteClusterPolicyManagedIdentity =
            new CommandInfo(nameof(DeleteClusterPolicyManagedIdentity),
                "delete cluster policy managed_identity",
                PolicyResult);

        #endregion

        #region AutoDelete

        public static readonly CommandInfo ShowTablePolicyAutoDelete =
            new CommandInfo(nameof(ShowTablePolicyAutoDelete),
                "show table TableName=(<database_table>) policy auto_delete",
                PolicyResult);

        public static readonly CommandInfo AlterTablePolicyAutoDelete =
            new CommandInfo(nameof(AlterTablePolicyAutoDelete),
                "alter table TableName=<database_table> policy auto_delete AutoDeletePolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo DeleteTablePolicyAutoDelete =
            new CommandInfo(nameof(DeleteTablePolicyAutoDelete),
                "delete table TableName=<database_table> policy auto_delete",
                PolicyResult);

        #endregion 

        #region Callout
        public static readonly CommandInfo ShowClusterPolicyCallout =
            new CommandInfo(nameof(ShowClusterPolicyCallout),
                "show cluster policy callout",
                PolicyResult);

        public static readonly CommandInfo AlterClusterPolicyCallout =
            new CommandInfo(nameof(AlterClusterPolicyCallout),
                "alter cluster policy callout Policy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeClusterPolicyCallout =
            new CommandInfo(nameof(AlterMergeClusterPolicyCallout),
                "alter-merge cluster policy callout Policy=<string>",
                PolicyResult);

        public static readonly CommandInfo DeleteClusterPolicyCallout =
            new CommandInfo(nameof(DeleteClusterPolicyCallout),
                "delete cluster policy callout",
                PolicyResult);
        #endregion

        #region Capacity
        public static readonly CommandInfo ShowClusterPolicyCapacity =
            new CommandInfo(nameof(ShowClusterPolicyCapacity),
                "show cluster policy capacity",
                PolicyResult);

        public static readonly CommandInfo AlterClusterPolicyCapacity =
            new CommandInfo(nameof(AlterClusterPolicyCapacity),
                "alter cluster policy capacity Policy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeClusterPolicyCapacity =
            new CommandInfo(nameof(AlterMergeClusterPolicyCapacity),
                "alter-merge cluster policy capacity Policy=<string>",
                PolicyResult);
        #endregion

        #region Request classification
        public static readonly CommandInfo ShowClusterPolicyRequestClassification =
            new CommandInfo(nameof(ShowClusterPolicyRequestClassification),
                "show cluster policy request_classification",
                PolicyResult);

        public static readonly CommandInfo AlterClusterPolicyRequestClassification =
            new CommandInfo(nameof(AlterClusterPolicyRequestClassification),
                "alter cluster policy request_classification Policy=<string> '<|' Query=<input_query>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeClusterPolicyRequestClassification =
            new CommandInfo(nameof(AlterMergeClusterPolicyRequestClassification),
                "alter-merge cluster policy request_classification Policy=<string>",
                PolicyResult);

        public static readonly CommandInfo DeleteClusterPolicyRequestClassification =
                new CommandInfo(nameof(DeleteClusterPolicyRequestClassification),
                "delete cluster policy request_classification",
                PolicyResult);
        #endregion

        #region Multi Database Admins
        public static readonly CommandInfo ShowClusterPolicyMultiDatabaseAdmins =
            new CommandInfo(nameof(ShowClusterPolicyMultiDatabaseAdmins),
                "show cluster policy multidatabaseadmins",
                PolicyResult);

        public static readonly CommandInfo AlterClusterPolicyMultiDatabaseAdmins =
            new CommandInfo(nameof(AlterClusterPolicyMultiDatabaseAdmins),
                "alter cluster policy multidatabaseadmins Policy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeClusterPolicyMultiDatabaseAdmins =
            new CommandInfo(nameof(AlterMergeClusterPolicyMultiDatabaseAdmins),
                "alter-merge cluster policy multidatabaseadmins Policy=<string>",
                PolicyResult);

        #endregion

        #region Diagnostics
        public static readonly CommandInfo ShowDatabasePolicyDiagnostics =
            new CommandInfo(nameof(ShowDatabasePolicyDiagnostics),
                "show database DatabaseName=<database> policy diagnostics",
                PolicyResult);

        public static readonly CommandInfo ShowClusterPolicyDiagnostics =
            new CommandInfo(nameof(ShowClusterPolicyDiagnostics),
                "show cluster policy diagnostics",
                PolicyResult);

        public static readonly CommandInfo AlterDatabasePolicyDiagnostics =
            new CommandInfo(nameof(AlterDatabasePolicyDiagnostics),
                "alter database [DatabaseName=<database>] policy diagnostics PolicyName=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeDatabasePolicyDiagnostics =
            new CommandInfo(nameof(AlterMergeDatabasePolicyDiagnostics),
                "alter-merge database [DatabaseName=<database>] policy diagnostics PolicyName=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterClusterPolicyDiagnostics =
            new CommandInfo(nameof(AlterClusterPolicyDiagnostics),
                "alter cluster policy diagnostics PolicyName=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeClusterPolicyDiagnostics =
            new CommandInfo(nameof(AlterMergeClusterPolicyDiagnostics),
                "alter-merge cluster policy diagnostics PolicyName=<string>",
                PolicyResult);

        public static readonly CommandInfo DeleteDatabasePolicyDiagnostics =
            new CommandInfo(nameof(DeleteDatabasePolicyDiagnostics),
                "delete database DatabaseName=<database> policy diagnostics",
                PolicyResult);
        #endregion

        #region Weak Consistency Query
        public static readonly CommandInfo ShowClusterPolicyQueryWeakConsistency =
            new CommandInfo(nameof(ShowClusterPolicyQueryWeakConsistency),
                "show cluster policy query_weak_consistency",
                PolicyResult);

        public static readonly CommandInfo AlterClusterPolicyQueryWeakConsistency =
            new CommandInfo(nameof(AlterClusterPolicyQueryWeakConsistency),
                "alter cluster policy query_weak_consistency Policy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterMergeClusterPolicyQueryWeakConsistency =
            new CommandInfo(nameof(AlterMergeClusterPolicyQueryWeakConsistency),
                "alter-merge cluster policy query_weak_consistency Policy=<string>",
                PolicyResult);
        #endregion

        #region Extent Tags Retention
        public static readonly CommandInfo ShowTablePolicyExtentTagsRetention =
            new CommandInfo(nameof(ShowTablePolicyExtentTagsRetention),
                "show table TableName=<database_table> policy extent_tags_retention",
                PolicyResult);

        public static readonly CommandInfo ShowTableStarPolicyExtentTagsRetention =
            new CommandInfo(nameof(ShowTableStarPolicyExtentTagsRetention),
                "show table '*' policy extent_tags_retention",
                PolicyResult);

        public static readonly CommandInfo ShowDatabasePolicyExtentTagsRetention =
            new CommandInfo(nameof(ShowDatabasePolicyExtentTagsRetention),
                "show database [DatabaseName=(<database> | '*')] policy extent_tags_retention",
                PolicyResult);

        public static readonly CommandInfo AlterTablePolicyExtentTagsRetention =
            new CommandInfo(nameof(AlterTablePolicyExtentTagsRetention),
                "alter table TableName=<database_table> policy extent_tags_retention ExtentTagsRetentionPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo AlterDatabasePolicyExtentTagsRetention =
            new CommandInfo(nameof(AlterDatabasePolicyExtentTagsRetention),
                "alter database [DatabaseName=<database>] policy extent_tags_retention ExtentTagsRetentionPolicy=<string>",
                PolicyResult);

        public static readonly CommandInfo DeleteTablePolicyExtentTagsRetention =
            new CommandInfo(nameof(DeleteTablePolicyExtentTagsRetention),
                "delete table TableName=<database_table> policy extent_tags_retention",
                PolicyResult);

        public static readonly CommandInfo DeleteDatabasePolicyExtentTagsRetention =
            new CommandInfo(nameof(DeleteDatabasePolicyExtentTagsRetention),
                "delete database DatabaseName=<database> policy extent_tags_retention",
                PolicyResult);
        #endregion
        #endregion

        #region Security Role Commands

        private static readonly string ShowPrincipalRolesResult =
            "(Scope: string, DisplayName: string, AADObjectID: string, Role: string)";

        public static readonly CommandInfo ShowPrincipalRoles =
            new CommandInfo(nameof(ShowPrincipalRoles),
                $"show principal (roles | Principal=<string> roles) [{PropertyList()}]",
                ShowPrincipalRolesResult);

        public static readonly CommandInfo ShowDatabasePrincipalRoles =
            new CommandInfo(nameof(ShowDatabasePrincipalRoles),
                $"show database DatabaseName=<database> principal (roles | Principal=<string> roles) [{PropertyList()}]",
                ShowPrincipalRolesResult);

        public static readonly CommandInfo ShowTablePrincipalRoles =
            new CommandInfo(nameof(ShowTablePrincipalRoles),
                $"show table TableName=<table> principal (roles | Principal=<string> roles) [{PropertyList()}]",
                ShowPrincipalRolesResult);

        public static readonly CommandInfo ShowGraphModelPrincipalRoles =
            new CommandInfo(nameof(ShowGraphModelPrincipalRoles),
                $"show graph_model <graph_model> principal (roles | Principal=<string> roles) [{PropertyList()}]",
                ShowPrincipalRolesResult);

        public static readonly CommandInfo ShowExternalTablesPrincipalRoles =
            new CommandInfo(nameof(ShowExternalTablesPrincipalRoles),
                $"show external table ExternalTableName=<externaltable> principal (roles | Principal=<string> roles) [{PropertyList()}]",
                ShowPrincipalRolesResult);

        public static readonly CommandInfo ShowFunctionPrincipalRoles =
            new CommandInfo(nameof(ShowFunctionPrincipalRoles),
                $"show function FunctionName=<function> principal (roles | Principal=<string> roles) [{PropertyList()}]",
                ShowPrincipalRolesResult);

        public static readonly CommandInfo ShowClusterPrincipalRoles =
            new CommandInfo(nameof(ShowClusterPrincipalRoles),
                $"show cluster principal (roles | Principal=<string> roles) [{PropertyList()}]",
                ShowPrincipalRolesResult);

        private static readonly string ShowPrincipalsResult =
            "(Role: string, PrincipalType: string, PrincipalDisplayName: string, PrincipalObjectId: string, PrincipalFQN: string, Notes: string, RoleAssignmentIdentifier: string)";

        public static readonly CommandInfo ShowClusterPrincipals =
            new CommandInfo(nameof(ShowClusterPrincipals),
                "show cluster principals",
                ShowPrincipalsResult);

        public static readonly CommandInfo ShowDatabasePrincipals =
            new CommandInfo(nameof(ShowDatabasePrincipals),
                "show database [DatabaseName=<database>] principals",
                ShowPrincipalsResult);

        public static readonly CommandInfo ShowTablePrincipals =
            new CommandInfo(nameof(ShowTablePrincipals),
                "show table TableName=<table> principals",
                ShowPrincipalsResult);

        public static readonly CommandInfo ShowGraphModelPrincipals =
            new CommandInfo(nameof(ShowGraphModelPrincipals),
                "show graph_model <graph_model> principals",
                ShowPrincipalsResult);

        public static readonly CommandInfo ShowFunctionPrincipals =
            new CommandInfo(nameof(ShowFunctionPrincipals),
                "show function FunctionName=<function> principals",
                ShowPrincipalsResult);

        private static string ClusterRole = "Role=(admins | alldatabasesadmins | alldatabasesviewers | alldatabasesmonitors | databasecreators | monitors | ops | users | viewers)";
        private static string DatabaseRole = "Role=(admins | ingestors | monitors | unrestrictedviewers | users | viewers)";
        private static string TableRole = "Role=(admins | ingestors)";
        private static string FunctionRole = "Role=admins";
        private static string PrincipalsClause = "'(' { Principal=<string>, ',' }+ ')' [SkipResults=skip-results] [Notes=<string>]";
        private static string PrincipalsOrNoneClause = $"(none [SkipResults=skip-results] | {PrincipalsClause})";

        public static readonly CommandInfo AddClusterRole =
            new CommandInfo(nameof(AddClusterRole),
                $"add cluster {ClusterRole} {PrincipalsClause}",
                ShowPrincipalsResult);

        public static readonly CommandInfo DropClusterRole =
            new CommandInfo(nameof(DropClusterRole),
                $"drop cluster {ClusterRole} {PrincipalsClause}",
                ShowPrincipalsResult);

        public static readonly CommandInfo SetClusterRole =
            new CommandInfo(nameof(SetClusterRole),
                $"set cluster {ClusterRole} {PrincipalsOrNoneClause}",
                ShowPrincipalsResult);

        public static readonly CommandInfo AddDatabaseRole =
            new CommandInfo(nameof(AddDatabaseRole),
                $"add database [DatabaseName=<database>] {DatabaseRole} {PrincipalsClause}",
                ShowPrincipalsResult);

        public static readonly CommandInfo DropDatabaseRole =
            new CommandInfo(nameof(DropDatabaseRole),
                $"drop database DatabaseName=<database> {DatabaseRole} {PrincipalsClause}",
                ShowPrincipalsResult);

        public static readonly CommandInfo SetDatabaseRole =
            new CommandInfo(nameof(SetDatabaseRole),
                $"set database DatabaseName=<database> {DatabaseRole} {PrincipalsOrNoneClause}",
                ShowPrincipalsResult);

        public static readonly CommandInfo AddTableRole =
            new CommandInfo(nameof(AddTableRole),
                $"add table TableName=<table> {TableRole} {PrincipalsClause}",
                ShowPrincipalsResult);

        public static readonly CommandInfo DropTableRole =
            new CommandInfo(nameof(DropTableRole),
                $"drop table TableName=<table> {TableRole} {PrincipalsClause}",
                ShowPrincipalsResult);

        public static readonly CommandInfo SetTableRole =
            new CommandInfo(nameof(SetTableRole),
                $"set table TableName=<table> {TableRole} {PrincipalsOrNoneClause}",
                ShowPrincipalsResult);

        public static readonly CommandInfo AddFunctionRole =
            new CommandInfo(nameof(AddFunctionRole),
                $"add function FunctionName=<function> {FunctionRole} {PrincipalsClause}",
                ShowPrincipalsResult);

        public static readonly CommandInfo DropFunctionRole =
            new CommandInfo(nameof(DropFunctionRole),
                $"drop function FunctionName=<function> {FunctionRole} {PrincipalsClause}",
                ShowPrincipalsResult);

        public static readonly CommandInfo SetFunctionRole =
            new CommandInfo(nameof(SetFunctionRole),
                $"set function FunctionName=<function> {FunctionRole} {PrincipalsOrNoneClause}",
                ShowPrincipalsResult);

        private static readonly string BlockedPrincipalsResult =
            "(PrincipalType: string, PrincipalDisplayName: string, PrincipalObjectId: string, PrincipalFQN: string, Application: string, User: string, BlockedUntil: datetime, Reason: string)";

        public static readonly CommandInfo ShowClusterBlockedPrincipals =
            new CommandInfo(nameof(ShowClusterBlockedPrincipals),
                "show cluster blockedprincipals",
                BlockedPrincipalsResult);

        public static readonly CommandInfo AddClusterBlockedPrincipals =
            new CommandInfo(nameof(AddClusterBlockedPrincipals),
                "add cluster blockedprincipals Principal=<string> [application AppName=<string>] [user UserName=<string>] [period Period=<timespan>] [reason Reason=<string>]",
                BlockedPrincipalsResult);

        public static readonly CommandInfo DropClusterBlockedPrincipals =
            new CommandInfo(nameof(DropClusterBlockedPrincipals),
                "drop cluster blockedprincipals Principal=<string> [application AppName=<string>] [user UserName=<string>]",
                BlockedPrincipalsResult);

        public static readonly CommandInfo SetClusterMaintenanceMode =
            new CommandInfo(nameof(SetClusterMaintenanceMode),
                $"(enable | disable) cluster maintenance_mode",
                UnknownResult);
        #endregion

        #region Data Ingestion
        private static readonly string PathOrPathList = "(Path=<string> | '(' { Path=<string>, ',' }+ ')')";

        private static readonly string DataIngestionPropertyList =
            PropertyList(
                "ingestionMapping | ingestionMappingReference | creationTime | distributed | docstring | extend_schema | folder | format | ingestIfNotExists | " +
                "ignoreFirstRecord | persistDetails | policy_ingestionTime | recreate_schema | tags | validationPolicy | zipPattern | small_dimension_table");

        public static readonly CommandInfo IngestIntoTable =
            new CommandInfo(nameof(IngestIntoTable),
                $"ingest [async] into table TableName=<table> {PathOrPathList} [{DataIngestionPropertyList}]",
                "(ExtentId: guid, ItemLoaded: string, Duration: string, HasErrors: string, OperationId: guid)");

        public static readonly CommandInfo IngestInlineIntoTable =
            new CommandInfo(nameof(IngestInlineIntoTable),
                $"ingest inline into table TableName=<name> ('[' Data=<bracketed_input_data> ']' | {DataIngestionPropertyList} '<|' Data=<input_data> | '<|' Data=<input_data>)",
                "(ExtentId: guid)");

        private static readonly string DataIngestionSetAppendResult =
            "(ExtentId: guid, OriginalSize: long, ExtentSize: long, ColumnSize: long, IndexSize: long, RowCount: long)";

        public static readonly CommandInfo SetTable =
            new CommandInfo(nameof(SetTable),
                $"set [async] TableName=<name> [{DataIngestionPropertyList}] '<|' QueryOrCommand=<input_query>",
                DataIngestionSetAppendResult);

        public static readonly CommandInfo AppendTable =
            new CommandInfo(nameof(AppendTable),
                $"append [async] TableName=<table> [{DataIngestionPropertyList}] '<|' QueryOrCommand=<input_query>",
                DataIngestionSetAppendResult);

        public static readonly CommandInfo SetOrAppendTable =
            new CommandInfo(nameof(SetOrAppendTable),
                $"set-or-append [async] TableName=<name> [{DataIngestionPropertyList}] '<|' QueryOrCommand=<input_query>",
                DataIngestionSetAppendResult);

        public static readonly CommandInfo SetOrReplaceTable =
            new CommandInfo(nameof(SetOrReplaceTable),
                $"set-or-replace [async] TableName=<name> [{DataIngestionPropertyList}] '<|' QueryOrCommand=<input_query>",
                DataIngestionSetAppendResult);
        #endregion

        #region Data Export
        private static string DataConnectionStringList = "'(' { DataConnectionString=<string>, ',' }+ ')'";

        public static readonly CommandInfo ExportToStorage =
            new CommandInfo(nameof(ExportToStorage),
                $"export [async] [compressed] to (csv|tsv|json|parquet) {DataConnectionStringList} [{PropertyList()}] '<|' Query=<input_query>",
                UnknownResult);

        public static readonly CommandInfo ExportToSqlTable =
            new CommandInfo(nameof(ExportToSqlTable),
                $"export [async] to sql SqlTableName=<name> SqlConnectionString=<string> [{PropertyList()}] '<|' Query=<input_query>",
                UnknownResult);

        public static readonly CommandInfo ExportToExternalTable =
            new CommandInfo(nameof(ExportToExternalTable),
                $"export [async] to table ExternalTableName=<externaltable> [{PropertyList()}] '<|' Query=<input_query>",
                UnknownResult);

        private static readonly string OverClause = "over '(' { TableName=<name>, ',' }+ ')'";

        public static readonly CommandInfo CreateOrAlterContinuousExport =
            new CommandInfo(nameof(CreateOrAlterContinuousExport),
                $"create-or-alter continuous-export ContinuousExportName=<name> [{OverClause}] to table ExternalTableName=<externaltable> [{PropertyList()}] '<|' Query=<input_query>",
                UnknownResult);

        private static readonly string ShowContinuousExportResult =
            "(Name: string, ExternalTableName: string, Query: string, " +
            "ForcedLatency: timespan, IntervalBetweenRuns: timespan, CursorScopedTables: string, ExportProperties: string, " +
            "LastRunTime: datetime, StartCursor: string, IsDisabled: bool, LastRunResult: string, ExportedTo: datetime, IsRunning: bool)";

        public static readonly CommandInfo ShowContinuousExport =
            new CommandInfo(nameof(ShowContinuousExport),
                "show continuous-export ContinuousExportName=<name>",
                ShowContinuousExportResult);

        public static readonly CommandInfo ShowContinuousExports =
            new CommandInfo(nameof(ShowContinuousExports),
                "show continuous-exports",
                ShowContinuousExportResult);

        public static readonly CommandInfo ShowClusterPendingContinuousExports =
            new CommandInfo(nameof(ShowClusterPendingContinuousExports),
                $"show cluster pending continuous-exports [{PropertyList()}]",
                ShowContinuousExportResult);

        public static readonly CommandInfo ShowContinuousExportExportedArtifacts =
            new CommandInfo(nameof(ShowContinuousExportExportedArtifacts),
                "show continuous-export ContinuousExportName=<name> exported-artifacts",
                "(Timestamp: datetime, ExternalTableName: string, Path: string, NumRecords: long)");

        public static readonly CommandInfo ShowContinuousExportFailures =
            new CommandInfo(nameof(ShowContinuousExportFailures),
                "show continuous-export ContinuousExportName=<name> failures",
                "(Timestamp: datetime, OperationId: string, Name: string, LastSuccessRun: datetime, FailureKind: string, Details: string)");

        public static readonly CommandInfo SetContinuousExportCursor =
            new CommandInfo(nameof(SetContinuousExportCursor),
                "set continuous-export jobName=<name> cursor to cursorValue=<string>",
                UnknownResult);

        public static readonly CommandInfo DropContinuousExport =
            new CommandInfo(nameof(DropContinuousExport),
                "drop continuous-export ContinuousExportName=<name>",
                ShowContinuousExportResult);

        public static readonly CommandInfo EnableContinuousExport =
            new CommandInfo(nameof(EnableContinuousExport),
                "enable continuous-export ContinuousExportName=<name>",
                ShowContinuousExportResult);

        public static readonly CommandInfo DisableContinuousExport =
            new CommandInfo(nameof(DisableContinuousExport),
                "disable continuous-export ContinousExportName=<name>",
                ShowContinuousExportResult);

        #endregion

        #region Materialized Views

        private static readonly string MaterializedViewCreatePropertyList =
            PropertyList("lookback | lookback_column | backfill | effectiveDateTime | updateExtentsCreationTime | autoUpdateSchema | dimensionTables | dimensionMaterializedViews | folder | docString");

        private static readonly string MaterializedViewAlterPropertyList =
            PropertyList("lookback | lookback_column | dimensionTables | dimensionMaterializedViews");

        private static readonly string ShowMaterializedViewResult =
           "(Name: string, SourceTable: string, Query: string, " +
           "MaterializedTo: datetime, LastRun: datetime, LastRunResult: string, IsHealthy: bool, " +
           "IsEnabled: bool, Folder: string, DocString: string, AutoUpdateSchema: bool, EffectiveDateTime: datetime, Lookback:timespan, LookbackColumn:string)";

        private static readonly string ShowMaterializedViewsDetailsResult =
            "(MaterializedViewName: string, DatabaseName: string, Folder: string, DocString: string, TotalExtents: long, TotalExtentSize: real, TotalOriginalSize: real, TotalRowCount: long, HotExtents: long, HotExtentSize: real, HotOriginalSize: real, HotRowCount: long, AuthorizedPrincipals: string, RetentionPolicy: string, CachingPolicy: string, ShardingPolicy: string, MergePolicy: string, MinExtentsCreationTime: datetime, MaxExtentsCreationTime: datetime)";

        public static readonly CommandInfo CreateMaterializedView =
            new CommandInfo(nameof(CreateMaterializedView),
                $"create [async] [ifnotexists] materialized-view [{MaterializedViewCreatePropertyList}] " +
                "MaterializedViewName=<name> on table SourceTableName=<table> SourceBody=<function_body>",
                UnknownResult);

        public static readonly CommandInfo CreateMaterializedViewOverMaterializedView =
            new CommandInfo(nameof(CreateMaterializedViewOverMaterializedView),
                $"create [async] [ifnotexists] materialized-view [{MaterializedViewCreatePropertyList}] " +
                "MaterializedViewName=<name> on materialized-view SourceMaterializedViewName=<materializedview> SourceBody=<function_body>",
                UnknownResult);

        public static readonly CommandInfo RenameMaterializedView =
            new CommandInfo(nameof(RenameMaterializedView),
                "rename materialized-view MaterializedViewName=<materializedview> to NewMaterializedViewName=<name>",
                ShowMaterializedViewResult);

        public static readonly CommandInfo ShowMaterializedView =
           new CommandInfo(nameof(ShowMaterializedView),
               "show materialized-view MaterializedViewName=<materializedview>",
               ShowMaterializedViewResult);

        public static readonly CommandInfo ShowMaterializedViews =
           new CommandInfo(nameof(ShowMaterializedViews),
               "show materialized-views",
               ShowMaterializedViewResult);

        public static readonly CommandInfo ShowMaterializedViewsDetails =
            new CommandInfo(nameof(ShowMaterializedViewsDetails),
                "show materialized-views ['(' { MaterializedViewName=<materializedview>, ',' }+ ')'] details",
                ShowMaterializedViewsDetailsResult);

        public static readonly CommandInfo ShowMaterializedViewDetails =
            new CommandInfo(nameof(ShowMaterializedViewDetails),
                "show materialized-view MaterializedViewName=<materializedview> details",
                ShowMaterializedViewsDetailsResult);

        public static readonly CommandInfo ShowMaterializedViewPolicyRetention =
            new CommandInfo(nameof(ShowMaterializedViewPolicyRetention),
                "show materialized-view MaterializedViewName=<materializedview> policy retention",
                PolicyResult);

        public static readonly CommandInfo ShowMaterializedViewPolicyMerge =
            new CommandInfo(nameof(ShowMaterializedViewPolicyMerge),
                "show materialized-view MaterializedViewName=<materializedview> policy merge",
                PolicyResult);

        public static readonly CommandInfo ShowMaterializedViewPolicyPartitioning =
           new CommandInfo(nameof(ShowMaterializedViewPolicyPartitioning),
               "show materialized-view MaterializedViewName=<materializedview> policy partitioning",
               PolicyResult);

        public static readonly CommandInfo ShowMaterializedViewExtents =
            new CommandInfo(nameof(ShowMaterializedViewExtents),
                 $"show materialized-view MaterializedViewName=<materializedview> extents [{ExtentIdList}] [hot]",
                "(ExtentId: guid, DatabaseName: string, TableName: string, MaxCreatedOn: datetime, OriginalSize: real, ExtentSize: real, CompressedSize: real, IndexSize: real, Blocks: long, Segments: long, ExtentContainerId: string, RowCount: long, MinCreatedOn: datetime, Tags: string, Kind: string, DeletedRowCount: long)");

        public static readonly CommandInfo AlterMaterializedView =
            new CommandInfo(nameof(AlterMaterializedView),
                $"alter materialized-view [{MaterializedViewAlterPropertyList}] MaterializedViewName=<materializedview> on table SourceTableName=<table> SourceBody=<function_body>",
                ShowMaterializedViewResult);

        public static readonly CommandInfo AlterMaterializedViewOverMaterializedView =
            new CommandInfo(nameof(AlterMaterializedViewOverMaterializedView),
                $"alter materialized-view [{MaterializedViewAlterPropertyList}] MaterializedViewName=<materializedview> on materialized-view SourceMaterializedViewName=<materializedview> SourceBody=<function_body>",
                ShowMaterializedViewResult);

        public static readonly CommandInfo CreateOrAlterMaterializedView =
            new CommandInfo(nameof(CreateOrAlterMaterializedView),
                $"create-or-alter materialized-view [{MaterializedViewCreatePropertyList}] MaterializedViewName=<name> on table SourceTableName=<table> SourceBody=<function_body>",
                ShowMaterializedViewResult);

        public static readonly CommandInfo CreateOrAlterMaterializedViewOverMaterializedView =
            new CommandInfo(nameof(CreateOrAlterMaterializedViewOverMaterializedView),
                $"create-or-alter materialized-view [{MaterializedViewCreatePropertyList}] MaterializedViewName=<name> on materialized-view SourceMaterializedViewName=<materializedview> SourceBody=<function_body>",
                ShowMaterializedViewResult);

        public static readonly CommandInfo DropMaterializedView =
            new CommandInfo(nameof(DropMaterializedView),
                $"drop materialized-view MaterializedViewName=<materializedview>",
                ShowMaterializedViewResult);

        public static readonly CommandInfo EnableDisableMaterializedView =
            new CommandInfo(nameof(EnableDisableMaterializedView),
                $"(enable | disable) materialized-view MaterializedViewName=<materializedview>",
                ShowMaterializedViewResult);

        public static readonly CommandInfo ShowMaterializedViewPrincipals =
            new CommandInfo(nameof(ShowMaterializedViewPrincipals),
                "show materialized-view MaterializedViewName=<materializedview> principals",
                ShowPrincipalsResult);

        public static readonly CommandInfo ShowMaterializedViewSchemaAsJson =
            new CommandInfo(nameof(ShowMaterializedViewSchemaAsJson),
                "show materialized-view MaterializedViewName=<materializedview> schema as json",
                ShowTableSchemaResult);

        public static readonly CommandInfo ShowMaterializedViewCslSchema =
            new CommandInfo(nameof(ShowMaterializedViewCslSchema),
                "show materialized-view MaterializedViewName=<materializedview> (kqlschema | cslschema)",
                ShowTableSchemaResult);

        public static readonly CommandInfo AlterMaterializedViewFolder =
            new CommandInfo(nameof(AlterMaterializedViewFolder),
                "alter materialized-view MaterializedViewName=<materializedview> folder Folder=<string>",
                ShowMaterializedViewResult);

        public static readonly CommandInfo AlterMaterializedViewDocString =
            new CommandInfo(nameof(AlterMaterializedViewDocString),
                "alter materialized-view MaterializedViewName=<materializedview> docstring Documentation=<string>",
                ShowMaterializedViewResult);

        // TODO: Add support for .alter lookbck_column
        public static readonly CommandInfo AlterMaterializedViewLookback =
            new CommandInfo(nameof(AlterMaterializedViewLookback),
                "alter materialized-view MaterializedViewName=<materializedview> lookback Lookback=<timespan>",
                ShowMaterializedViewResult);

        public static readonly CommandInfo AlterMaterializedViewAutoUpdateSchema =
            new CommandInfo(nameof(AlterMaterializedViewAutoUpdateSchema),
                "alter materialized-view MaterializedViewName=<materializedview> autoUpdateSchema (true|false)",
                ShowMaterializedViewResult);

        public static readonly CommandInfo ClearMaterializedViewData =
           new CommandInfo(nameof(ClearMaterializedViewData),
               "clear materialized-view MaterializedViewName=<materializedview> data",
               "(ExtentId: guid, TableName: string, CreatedOn: datetime)");

        public static readonly CommandInfo SetMaterializedViewCursor =
            new CommandInfo(nameof(SetMaterializedViewCursor),
                "set materialized-view MaterializedViewName=<materializedview> cursor to CursorValue=<string>",
                UnknownResult);

        #endregion

        #region Mirroring Operations
        public static readonly CommandInfo ShowTableOperationsMirroringStatus =
            new CommandInfo(nameof(ShowTableOperationsMirroringStatus),
                "show table TableName=(<table> | '*') operations mirroring-status",
                "(TableName: string, IsEnabled: bool, " +
                "ExportProperties: string, ManagedIdentityIdentifier: string, IsExportRunning: bool, LastExportStartTime: datetime, " +
                "LastExportResult: string, LastExportedDataTime: datetime)");

        public static readonly CommandInfo ShowTableOperationsMirroringExportedArtifacts =
            new CommandInfo(nameof(ShowTableOperationsMirroringExportedArtifacts),
                "show table TableName=<table> operations mirroring-exported-artifacts",
                "(Timestamp: datetime, ExternalTableName: string, Path: string, NumRecords: long)");

        public static readonly CommandInfo ShowTableOperationsMirroringFailures =
            new CommandInfo(nameof(ShowTableOperationsMirroringFailures),
                "show table TableName=<table> operations mirroring-failures",
                "(Timestamp: datetime, OperationId: string, Name: string, LastSuccessRun: datetime, FailureKind: string, Details: string)");
        #endregion

        #region System Information Commands
        public static readonly CommandInfo ShowCluster =
            new CommandInfo(nameof(ShowCluster),
                "show cluster",
                "(NodeId: string, Address: string, Name: string, StartTime: datetime, IsAdmin: bool, " +
                    "MachineTotalMemory: long, MachineAvailableMemory: long, ProcessorCount: int, EnvironmentDescription: string)");

        public static readonly CommandInfo ShowClusterDetails =
            new CommandInfo(nameof(ShowClusterDetails),
                "show cluster details",
                "(NodeId: string, Address: string, Name: string, StartTime: datetime, IsAdmin: bool, " +
                    "MachineTotalMemory: long, MachineAvailableMemory: long, ProcessorCount: int, EnvironmentDescription: string, NetAndClsVersion: string)");

        public static readonly CommandInfo ShowDiagnostics =
            new CommandInfo(nameof(ShowDiagnostics),
                "show diagnostics [with '(' scope '=' Scope=(cluster | workloadgroup) ')']",
                "(IsHealthy: bool, "
                + "EnvironmentDescription: string, "
                + "IsScaleOutRequired: bool, "
                + "MachinesTotal: int, "
                + "MachinesOffline: int, "
                + "NodeLastRestartedOn: datetime, "
                + "AdminLastElectedOn: datetime, "
                + "ExtentsTotal: int, "
                + "DiskColdAllocationPercentage: int, "
                + "InstancesTargetBasedOnDataCapacity: int, "
                + "TotalOriginalDataSize: real, "
                + "TotalExtentSize: real, "
                + "IngestionsLoadFactor: real, "
                + "IngestionsInProgress: long, "
                + "IngestionsSuccessRate: real, "
                + "MergesInProgress: long, "
                + "BuildVersion: string, "
                + "BuildTime: datetime, "
                + "ClusterDataCapacityFactor: real, "
                + "IsDataWarmingRequired: bool, "
                + "DataWarmingLastRunOn: datetime, "
                + "MergesSuccessRate: real, "
                + "NotHealthyReason: string, "
                + "IsAttentionRequired: bool, "
                + "AttentionRequiredReason: string, "
                + "ProductVersion: string, "
                + "FailedIngestOperations: int, "
                + "FailedMergeOperations: int, "
                + "MaxExtentsInSingleTable: int, "
                + "TableWithMaxExtents: string, "
                + "MergesLoadFactor: double, "
                + "WarmExtentSize: real, "
                + "NumberOfDatabases: int, "
                + "PurgeExtentsRebuildLoadFactor: real, "
                + "PurgeExtentsRebuildInProgress: long, "
                + "PurgesInProgress: long, "
                + "RowStoreLocalStorageCapacityFactor: real, "
                + "ExportsLoadFactor: real, "
                + "ExportsInProgress: long, "
                + "PendingContinuousExports: long, "
                + "MaxContinuousExportLatenessMinutes: long, "
                + "RowStoreSealsInProgress: long, "
                + "IsRowStoreUnhealthy: bool, "
                + "MachinesSuspended: int, "
                + "DataPartitioningLoadFactor: real, "
                + "DataPartitioningOperationsInProgress: long, "
                + "MinPartitioningPercentageInSingleTable: real, "
                + "TableWithMinPartitioningPercentage: string, "
                + "V3DataCapacityFactor: real, "
                + "MaterializedViewsInProgress: long, "
                + "DataPartitioningOperationsInProgress: real, "
                + "IngestionCapacityUtilization: real, "
                + "ShardsWarmingStatus: string, "
                + "ShardsWarmingTemperature: real, "
                + "ShardsWarmingDetails: string, "
                + "StoredQueryResultsInProgress: long, "
                + "HotDataDiskSpaceUsage: real, "
                + "MirroringOperationsLoadFactor: real, "
                + "MirroringOperationsInProgress: long, "
                + "QueryAccelerationLoadFactor: real, "
                + "QueryAccelerationInProgress: long, "
                + "QueryAccelerationCapacityUtilization: real, "
                + "GraphSnapshotsLoadFactor: real, "
                + "GraphSnapshotsInProgress: long");

        public static readonly CommandInfo ShowCapacity =
            new CommandInfo(nameof(ShowCapacity),
                "show capacity" +
                " (Resource=(ingestions | extents-merge | table-purge | data-export | mirroring | query-acceleration | extents-partition | streaming-ingestion-post-processing | materialized-view | graph_snapshot | queries" +
                "| stored-query-results | purge-storage-artifacts-cleanup | periodic-storage-artifacts-cleanup))?" +
                " [with '(' scope '=' Scope=(cluster | workloadgroup) ')']",
                "(Resource: string, Total: long, Consumed: long, Remaining: long)");

        public static readonly CommandInfo ShowOperations =
            new CommandInfo(nameof(ShowOperations),
                "show operations [(OperationId=<guid> | '(' { OperationId=<guid>, ',' }+ ')')]",
                "(OperationId: guid, Operation: string, NodeId: string, StartedOn: datetime, LastUpdatedOn: datetime, Duration: timespan, State: string, Status: string, RootActivityId: guid, ShouldRetry: bool, Database: string, Principal: string, User: string, AdminEpochStartTime: datetime, VirtualCluster: string)");

        public static readonly CommandInfo ShowOperationDetails =
            new CommandInfo(nameof(ShowOperationDetails),
                "show operation OperationId=<guid> details",
                UnknownResult); // schema depends on operation

        private static readonly string JournalResult =
            "(Event: string, EventTimestamp: datetime, Database: string, EntityName: string, UpdatedEntityName: string, EntityVersion: string, EntityContainerName: string, " +
            "OriginalEntityState: string, UpdatedEntityState: string, ChangeCommand: string, Principal: string, RootActivityId: guid, ClientRequestId: string, " +
            "User: string, OriginalEntityVersion: string)";

        public static readonly CommandInfo ShowJournal =
            new CommandInfo(nameof(ShowJournal),
                "show journal",
                JournalResult);

        public static readonly CommandInfo ShowDatabaseJournal =
            new CommandInfo(nameof(ShowDatabaseJournal),
                "show database DatabaseName=<database> journal",
                JournalResult);

        public static readonly CommandInfo ShowClusterJournal =
            new CommandInfo(nameof(ShowClusterJournal),
                "show cluster journal",
                JournalResult);

        private static readonly string QueryResults =
            "(ClientActivityId: string, Text: string, Database: string, StartedOn: datetime, LastUpdatedOn: datetime, Duration: timespan, " +
            "State: string, RootActivityId: guid, User: string, FailureReason: string, TotalCpu: timespan, CacheStatistics: dynamic, " +
            "Application: string, MemoryPeak: long, ScannedEventStatistics: dynamic, Pricipal: string, ClientRequestProperties: dynamic, ResultSetStatistics: dynamic, WorkloadGroup: string)";

        public static readonly CommandInfo ShowQueries =
            new CommandInfo(nameof(ShowQueries),
                "show queries",
                "(ClientActivityId: string, Text: string, Database: string, StartedOn: datetime, LastUpdatedOn: datetime, Duration: timespan, State: string, RootActivityId: guid, User: string, FailureReason: string, TotalCpu: timespan, CacheStatistics: dynamic, Application: string, MemoryPeak: long, ScannedExtentsStatistics: dynamic, Principal: string, ClientRequestProperties: dynamic, ResultSetStatistics: dynamic, WorkloadGroup: string, OverallQueryStats: string, VirtualCluster: string)");

        public static readonly CommandInfo ShowRunningQueries =
            new CommandInfo(nameof(ShowRunningQueries),
                "show running queries [by (user UserName=<string> | '*')]",
                QueryResults);

        public static readonly CommandInfo CancelQuery =
            new CommandInfo(nameof(CancelQuery),
                "cancel query ClientRequestId=<string>",
                UnknownResult);

        private static readonly string ShowQueryPlanPropertyList =
            PropertyList("reconstructCsl | showExternalArtifacts"
                );

        public static readonly CommandInfo ShowQueryPlan =
            new CommandInfo(nameof(ShowQueryPlan),
                $"show queryplan [{ShowQueryPlanPropertyList}] '<|' Query=<input_query>",
                "(ResultType: string, Format: string, Content: string)");

        public static readonly CommandInfo ShowCache =
            new CommandInfo(nameof(ShowCache),
                "show cache",
                "(NodeId: string, TotalMemoryCapacity: long, MemoryCacheCapacity: long, MemoryCacheInUse: long, MemoryCacheHitCount: long, " +
                "TotalDiskCapacity: long, DiskCacheCapacity: long, DiskCacheInUse: long, DiskCacheHitCount: long, DiskCacheMissCount: long, " +
                "MemoryCacheDetails: string, DiskCacheDetails: string, WarmedShardsSize: long, ShardsSizeToWarm: long, HotShardsSize: long)");

        public static readonly CommandInfo AlterCache =
            new CommandInfo(nameof(AlterCache),
                "alter cache on ('*' | NodeList=<bracketed_string>) Action=<bracketed_string>",
                UnknownResult);

        public static readonly CommandInfo ShowCommands =
            new CommandInfo(nameof(ShowCommands),
                "show commands",
                "(ClientActivityId: string, CommandType: string, Text: string, Database: string, " +
                "StartedOn: datetime, LastUpdatedOn: datetime, Duration: timespan, State: string, RootActivityId: guid, " +
                "User: string, FailureReason: string, Application: string, Principal: string, TotalCpu: timespan, " +
                "ResourcesUtilization: dynamic, ClientRequestProperties: dynamic, WorkloadGroup: string, VirtualCluster: string)");

        public static readonly CommandInfo ShowCommandsAndQueries =
            new CommandInfo(nameof(ShowCommandsAndQueries),
                "show commands-and-queries",
                "(ClientActivityId: string, CommandType: string, Text: string, Database: string, " +
                "StartedOn: datetime, LastUpdatedOn: datetime, Duration: timespan, State: string, FailureReason: string, RootActivityId: guid, " +
                "User: string, Application: string, Principal: string, ClientRequestProperties: dynamic, " +
                "TotalCpu: timespan, MemoryPeak: long, CacheStatistics: dynamic, ScannedExtentsStatistics: dynamic, ResultSetStatistics: dynamic, WorkloadGroup: string, VirtualCluster: string)");

        public static readonly CommandInfo ShowIngestionFailures =
            new CommandInfo(nameof(ShowIngestionFailures),
                "show ingestion failures [with '(' OperationId '=' OperationId=<guid> ')']",
                "(OperationId: guid, Database: string, Table: string, FailedOn: datetime, IngestionSourcePath: string, Details: string, FailureKind: string, RootActivityId: guid, OperationKind: string, OriginatesFromUpdatePolicy: bool, ErrorCode: string, Principal: string, ShouldRetry: bool, User: string, IngestionProperties: string)");

        public static readonly CommandInfo ShowDataOperations =
            new CommandInfo(nameof(ShowDataOperations),
                "show data operations",
                "(Timestamp: datetime, Database: string, Table: string, ClientActivityId: string, OperationKind: string, OriginalSize: long, ExtentSize: long, RowCount: long, Duration: timespan, TotalCpu: timespan, ExtentCount: long, Principal: string, Properties: string)");

        public static readonly CommandInfo ShowDatabaseKeyVaultSecrets =
            new CommandInfo(nameof(ShowDatabaseKeyVaultSecrets),
                "#(show database keyvault secrets)",
                "(KeyVaultSecretId: string)");
        #endregion

        #region Advanced Commands
        private static readonly string ShowExtentsResult = "(ExtentId: guid, DatabaseName: string, TableName: string, MaxCreatedOn: datetime, OriginalSize: real, ExtentSize: real, CompressedSize: real, IndexSize: real, Blocks: long, Segments: long, ExtentContainerId: string, RowCount: long, MinCreatedOn: datetime, Tags: string, Kind: string, DeletedRowCount: long)";
        private static readonly string ShowExtentsMetadataResult = "(ExtentId: guid, DatabaseName: string, TableName: string, ExtentMetadata: string)";

        private static readonly string TagWhereClause = "where { tags (has | contains | '!has' | '!contains') Tag=<string>, and }+";
        private static readonly string WithFilteringPolicyClause = "with '(' extentsShowFilteringRuntimePolicy '=' policy=<value> ')' ";
        private static readonly string ShowExtentsSuffix = $"[{TagWhereClause}] [{WithFilteringPolicyClause}]";

        #region Cluster extents
        public static readonly CommandInfo ShowClusterExtents =
            new CommandInfo(nameof(ShowClusterExtents),
                $"show cluster extents [{ExtentIdList}] [hot] {ShowExtentsSuffix}",
                ShowExtentsResult);

        public static readonly CommandInfo ShowClusterExtentsMetadata =
            new CommandInfo(nameof(ShowClusterExtentsMetadata),
                $"show cluster extents [{ExtentIdList}] [hot] metadata {ShowExtentsSuffix}",
                ShowExtentsMetadataResult);
        #endregion

        #region Database extents
        public static readonly CommandInfo ShowDatabaseExtents =
            new CommandInfo(nameof(ShowDatabaseExtents),
                $"show (database [DatabaseName=<database>] | databases {DatabasesNameList}) extents [{ExtentIdList}] [hot] {ShowExtentsSuffix}",
                ShowExtentsResult);

        public static readonly CommandInfo ShowDatabaseExtentsMetadata =
            new CommandInfo(nameof(ShowDatabaseExtentsMetadata),
                $"show (database [DatabaseName=<database>] | databases {DatabasesNameList}) extents [{ExtentIdList}] [hot] metadata {ShowExtentsSuffix}",
                ShowExtentsMetadataResult);

        public static readonly CommandInfo ShowDatabaseExtentTagsStatistics =
            new CommandInfo(nameof(ShowDatabaseExtentTagsStatistics),
                $"show database extent tags statistics [with '(' minCreationTime '=' minCreationTime=<value> ')']",
                "(TableName: string, TotalExtentsCount: long, TaggedExtentsCount: long, TotalTagsCount: long, TotalTagsLength: long, DropByTagsCount: long, DropByTagsLength: long, IngestByTagsCount: long, IngestByTagsLength: long)");

        public static readonly CommandInfo ShowDatabaseExtentsPartitioningStatistics =
            new CommandInfo(nameof(ShowDatabaseExtentsPartitioningStatistics),
                $"show database [DatabaseName=<database>] extents partitioning statistics",
                "(TableName: string, PartitioningPolicy: dynamic, TotalRowCount: long, PartitionedRowCount: long, PartitionedRowsPercentage: double)");
        #endregion

        #region Table extents
        public static readonly CommandInfo ShowTableExtents =
            new CommandInfo(nameof(ShowTableExtents),
                $"show (table TableName=<table> | tables {TableNameList}) extents [{ExtentIdList}] [hot] {ShowExtentsSuffix}",
                ShowExtentsResult);

        public static readonly CommandInfo ShowTableExtentsMetadata =
            new CommandInfo(nameof(ShowTableExtentsMetadata),
                $"show (table TableName=<table> | tables {TableNameList}) extents [{ExtentIdList}] [hot] metadata {ShowExtentsSuffix}",
                ShowExtentsMetadataResult);
        #endregion

        #region Shard groups
        private static readonly string ShardGroupsShowResult = "(Id: guid, ShardCount: long, RowCount: long, OriginalSize: long, ShardSize: long, CompressedSize: long, IndexSize: long, DeletedRowCount: long, PartitionedRowCount: long, DateTimeColumnRanges: dynamic, Partition: dynamic, CreationTimeRange: dynamic)";
        private static readonly string ShardGroupsStatisticsShowResult = "(TableId: guid, long ShardGroupCount: long, ShardCount: long, RowCount: long, OriginalSize: long, ShardSize: long, CompressedSize: long, IndexSize: long, DeletedRowCount: long, PartitionedRowCount: long)";

        public static readonly CommandInfo TableShardsGroupShow =
            new CommandInfo(nameof(TableShardsGroupShow),
                "show table TableName=<table> #shards-group ShardsGroupId=<guid> #shards",
                ShowExtentsResult);

        public static readonly CommandInfo TableShardsGroupMetadataShow =
            new CommandInfo(nameof(TableShardsGroupMetadataShow),
                "show table TableName=<table> #shards-group ShardsGroupId=<guid> #shards #metadata",
                ShowExtentsMetadataResult);

        public static readonly CommandInfo TableShardGroupsShow =
            new CommandInfo(nameof(TableShardGroupsShow),
                "show table TableName=<table> #shard-groups",
                ShardGroupsShowResult);

        public static readonly CommandInfo TableShardGroupsStatisticsShow =
            new CommandInfo(nameof(TableShardGroupsStatisticsShow),
                "show table TableName=<table> #shard-groups #statistics",
                ShardGroupsStatisticsShowResult);

        public static readonly CommandInfo TablesShardGroupsStatisticsShow =
            new CommandInfo(nameof(TablesShardGroupsStatisticsShow),
                "show tables ['(' { TableName=<table>, ',' }+ ')'] #shard-groups #statistics",
                ShardGroupsStatisticsShowResult);

        public static readonly CommandInfo DatabaseShardGroupsStatisticsShow =
            new CommandInfo(nameof(DatabaseShardGroupsStatisticsShow),
                "show database [DatabaseName=<database>] #shard-groups #statistics",
                ShardGroupsStatisticsShowResult);
        #endregion

        private static readonly string MergeExtentsResult =
            "(OriginalExtentId: string, ResultExtentId: string, Duration: timespan)";

        private static readonly string GuidList = "'(' {GUID=<guid>, ','}+ ')'";
        public static readonly CommandInfo MergeExtents =
            new CommandInfo(nameof(MergeExtents),
                $"merge [async] TableName=<table> {GuidList} [with '(' rebuild '=' true ')']",
                MergeExtentsResult);

        public static readonly CommandInfo MergeExtentsDryrun =
            new CommandInfo(nameof(MergeExtentsDryrun),
                $"merge dryrun TableName=<table> {GuidList}",
                MergeExtentsResult);

        private static readonly string MoveExtentsResult =
            "(OriginalExtentId: string, ResultExtentId: string, Details: string)";

        public static readonly CommandInfo MoveExtentsFrom =
            new CommandInfo(nameof(MoveExtentsFrom),
                $"move [async] extents (all from table SourceTableName=<table> to table DestinationTableName=<table> | from table SourceTableName=<table> to table DestinationTableName=<table> [with '(' {{ PropertyName=<name> '=' Value=<value>, ',' }} ')'] {GuidList})",
                MoveExtentsResult);

        public static readonly CommandInfo MoveExtentsQuery =
            new CommandInfo(nameof(MoveExtentsQuery),
                $"move [async] extents to table DestinationTableName=<table> [with '(' {{ PropertyName=<name> '=' Value=<value>, ',' }} ')'] '<|' Query=<input_query>",
                MoveExtentsResult);

        public static readonly CommandInfo TableShuffleExtents =
            new CommandInfo(nameof(TableShuffleExtents),
                $"shuffle [async] table TableName=<table> extents (all | {GuidList}) [with '(' {{ PropertyName=<name> '=' Value=<value>, ',' }} ')']",
                MoveExtentsResult);

        public static readonly CommandInfo TableShuffleExtentsQuery =
            new CommandInfo(nameof(TableShuffleExtentsQuery),
                $"shuffle [async] table tableName=<table> extents [with '(' {{ PropertyName=<name> '=' Value=<value>, ',' }} ')'] '<|' Query=<input_query>",
                MoveExtentsResult);

        public static readonly CommandInfo ReplaceExtents =
            new CommandInfo(nameof(ReplaceExtents),
                $"replace [async] extents in table DestinationTableName=<table> [with '(' {{ PropertyName=<name> '=' Value=<value>, ',' }} ')'] '<|' '{{' ExtentsToDropQuery=<input_query> '}}' ',' '{{' ExtentsToMoveQuery=<input_query> '}}'",
                MoveExtentsResult);

        private static readonly string DropExtentResult =
            "(ExtentId: guid, TableName: string, CreatedOn: datetime)";

        //public static readonly CommandInfo DropExtentsQuery =
        //    new CommandInfo("drop extents query", "drop extents [whatif] '<|' Query=<input_query>", DropExtentResult);

        public static readonly CommandInfo DropExtent =
            new CommandInfo(nameof(DropExtent),
                "drop extent ExtentId=<guid> [from TableName=<table>]",
                DropExtentResult);

        private static readonly string DropProperties = "[older Older=<long> (days | hours)] from (all tables | TableName=<table>) [trim by (extentsize | datasize) TrimSize=<long> (MB | GB | bytes)] [limit LimitCount=<long>]";

        public static readonly CommandInfo DropExtents =
            new CommandInfo(nameof(DropExtents),
                @"drop extents 
                    ('(' { ExtentId=<guid>, ',' }+ ')' [from TableName=<table>]
                     | whatif '<|' Query=<input_query>
                     | '<|' Query=<input_query>
                     | older Older=<long> (days | hours) from (all tables | TableName=<table>) [trim by (extentsize | datasize) TrimSize=<long> (MB | GB | bytes)] [limit LimitCount=<long>]
                     | from (all tables | TableName=<table>) [trim by (extentsize | datasize) TrimSize=<long> (MB | GB | bytes)] [limit LimitCount=<long>]
                     )",
                DropExtentResult);

        public static readonly CommandInfo DropExtentsPartitionMetadata =
            new CommandInfo(nameof(DropExtentsPartitionMetadata),
                "drop extents partition metadata from table TableName=<table> " +
                " (between '(' d1=<datetime> '..' d2=<datetime> ')' )?  " +
                "csl=('<|' <input_query>)",
                UnknownResult);

        //public static readonly CommandInfo DropAsyncExtentsPartitionMetadata =
        //    new CommandInfo(nameof(DropAsyncExtentsPartitionMetadata),
        //        "drop async extents partition metadata csl=('<|' <input_query>)",
        //        UnknownResult);

        public static readonly CommandInfo DropPretendExtentsByProperties =
            new CommandInfo(nameof(DropPretendExtentsByProperties),
                $"drop-pretend extents {DropProperties}",
                DropExtentResult);

        public static readonly CommandInfo ShowVersion =
            new CommandInfo(nameof(ShowVersion),
                "show version",
                "(BuildVersion: string, BuildTime: datetime, ServiceType: string, ProductVersion: string)");

        public static readonly CommandInfo ClearTableData =
           new CommandInfo(nameof(ClearTableData),
               "clear [async] table TableName=<table> data",
               "(Status: string)");

        public static readonly CommandInfo ClearTableCacheStreamingIngestionSchema =
           new CommandInfo(nameof(ClearTableCacheStreamingIngestionSchema),
               "clear table TableName=<table> cache streamingingestion schema",
               "(NodeId: string, Status: string)");

        public static readonly CommandInfo ShowStorageArtifactsCleanupState =
            new CommandInfo(nameof(ShowStorageArtifactsCleanupState),
                $"show database (data | metadata) storage artifacts cleanup state [with '(' {{ PropertyName=<name> '=' Value=<value>, ',' }} ')']",
                "(DatabaseName: string, StorageAccount: string, ContainerId: guid, ContainerName: string, CleanupStateBlobName: string, LastModified: datetime, SizeInBytes: long, Content: string)");

        public static readonly CommandInfo ClusterDropStorageArtifactsCleanupState =
            new CommandInfo(nameof(ClusterDropStorageArtifactsCleanupState),
                $"drop cluster (data | metadata | all) storage artifacts cleanup state",
                "(ContainerId: guid, ContainerName: string, FileName: string, Succeeded: bool, FailureDetails: string)");
        #endregion

        #region StoredQueryResults

        public static readonly CommandInfo StoredQueryResultSet =
            new CommandInfo(nameof(StoredQueryResultSet),
                $"set [async] [ifnotexists] stored_query_result StoredQueryResultName=<name> [{DataIngestionPropertyList}] '<|' Query=<input_query>",
                UnknownResult);

        public static readonly CommandInfo StoredQueryResultSetOrReplace =
            new CommandInfo(nameof(StoredQueryResultSetOrReplace),
                $"set-or-replace [async] stored_query_result StoredQueryResultName=<name> [{DataIngestionPropertyList}] '<|' Query=<input_query>",
                UnknownResult);

        private static string StoredQueryResultsShowResult =
            "(StoredQueryResultId:guid, Name:string, DatabaseName:string, PrincipalIdentity:string, SizeInBytes:long, RowCount:long, CreatedOn:datetime, ExpiresOn:datetime)";

        public static readonly CommandInfo StoredQueryResultsShow =
            new CommandInfo(nameof(StoredQueryResultsShow),
                "show stored_query_results [StoredQueryResultName=<name>] [with '(' { PropertyName=<name> '=' Value=<value>, ',' } ')']",
                StoredQueryResultsShowResult);

        public static readonly CommandInfo StoredQueryResultShowSchema =
            new CommandInfo(nameof(StoredQueryResultShowSchema),
                "show stored_query_result StoredQueryResultName=<name> schema",
                "(StoredQueryResult:string, Schema:string)");

        public static readonly CommandInfo StoredQueryResultDrop =
            new CommandInfo(nameof(StoredQueryResultDrop),
                "drop stored_query_result StoredQueryResultName=<name>",
                StoredQueryResultsShowResult);

        public static readonly CommandInfo StoredQueryResultsDrop =
            new CommandInfo(nameof(StoredQueryResultsDrop),
                "drop stored_query_results by user Principal=<string>",
                StoredQueryResultsShowResult);

        #endregion

        #region GraphCommands

        #region Graph Model Commands
        private static string GraphModelShowDetailsResult =
            "(Name:string, CreationTime:datetime, Id:guid, SnapshotCount:long, Model:string, RetentionPolicy:string, CachingPolicy:string)";

        private static string GraphModelShowResult =
            "(Name:string, CreationTime:datetime, Id:guid)";

        public static readonly CommandInfo GraphModelCreateOrAlter =
            new CommandInfo(nameof(GraphModelCreateOrAlter),
            "create-or-alter graph_model <name> <string>",
            GraphModelShowDetailsResult);

        public static readonly CommandInfo GraphModelShow =
            new CommandInfo(nameof(GraphModelShow),
            "show graph_model <graph_model> [details] [with '(' { PropertyName=<name> '=' Value=<value>, ',' } ')']",
            UnknownResult);

        public static readonly CommandInfo GraphModelsShow =
            new CommandInfo(nameof(GraphModelsShow),
            "show graph_models [details] [with '(' { PropertyName=<name> '=' Value=<value>, ',' } ')']",
            UnknownResult);

        public static readonly CommandInfo GraphModelDrop =
            new CommandInfo(nameof(GraphModelDrop),
            "drop graph_model <graph_model>",
            GraphModelShowResult);

        public static readonly CommandInfo SetGraphModelAdmins =
            new CommandInfo(nameof(SetGraphModelAdmins),
                "set graph_model <graph_model> admins (none | '(' {principal=<string>, ','}+ ')' [notes=<string>])",
                UnknownResult);

        public static readonly CommandInfo AddGraphModelAdmins =
            new CommandInfo(nameof(AddGraphModelAdmins),
                "add graph_model <graph_model> admins '(' {principal=<string>, ','}+ ')' [notes=<string>]",
                UnknownResult);

        public static readonly CommandInfo DropGraphModelAdmins =
            new CommandInfo(nameof(DropGraphModelAdmins),
                "drop graph_model <graph_model> admins '(' {principal=<string>, ','}+ ')' [notes=<string>]",
                UnknownResult);
        #endregion

        #region Graph Snapshot Commands
        private static string GraphSnapshotShowResult =
            "(Name:string, SnapshotTime:datetime, ModelName:string, ModelId:guid, ModelCreationTime:datetime)";

        private static string GraphSnapshotShowDetailsResult =
            "(Name:string, SnapshotTime:datetime, ModelName:string, ModelId:guid, ModelCreationTime:datetime, NodesCount: long, EdgesCount: long, RetentionPolicy:string, CachingPolicy:string)";

        private static string GraphSnapshotsShowStatisticsResult =
            "(DatabaseName:string, Name:string, SnapshotTime:datetime, ModelName:string, ModelId:guid, TotalCpu:timespan, MemoryPeak:long, Duration:timespan, Details:string, NodesCount:long, EdgesCount:long, NodesSize:long, EdgesSize:long)";

        private static string GraphSnapshotsShowFailuresResult =
            "(OperationId:guid, DatabaseName:string, Name:string, SnapshotTime:datetime, ModelName:string, ModelId:guid, TotalCpu:timespan, MemoryPeak:long, Duration:timespan, Details:string, FailureReason:string, FailureKind:string)";

        public static readonly CommandInfo GraphSnapshotMake =
            new CommandInfo(nameof(GraphSnapshotMake),
            "make [async] graph_snapshot <name> from <graph_model>",
            GraphSnapshotShowDetailsResult);

        public static readonly CommandInfo GraphSnapshotShow =
            new CommandInfo(nameof(GraphSnapshotShow),
            "show graph_snapshot <graph_model_snapshot> [details]",
            UnknownResult);

        public static readonly CommandInfo GraphSnapshotsShow =
            new CommandInfo(nameof(GraphSnapshotsShow),
            "show graph_snapshots <qualified_wildcarded_name> [details]",
            UnknownResult);

        public static readonly CommandInfo GraphSnapshotDrop =
            new CommandInfo(nameof(GraphSnapshotDrop),
            "drop graph_snapshot <graph_model_snapshot>",
            GraphSnapshotShowResult);

        public static readonly CommandInfo GraphSnapshotsDrop =
            new CommandInfo(nameof(GraphSnapshotsDrop),
            "drop graph_snapshots <qualified_wildcarded_name>",
            GraphSnapshotShowResult);

        public static readonly CommandInfo GraphSnapshotShowStatistics =
            new CommandInfo(nameof(GraphSnapshotShowStatistics),
            "show graph_snapshot <qualified_wildcarded_name> #statistics",
            GraphSnapshotsShowStatisticsResult);

        public static readonly CommandInfo GraphSnapshotsShowStatistics =
            new CommandInfo(nameof(GraphSnapshotsShowStatistics),
            "show graph_snapshots <qualified_wildcarded_name> #statistics",
            GraphSnapshotsShowStatisticsResult);

        public static readonly CommandInfo GraphSnapshotShowFailures =
            new CommandInfo(nameof(GraphSnapshotShowFailures),
            "show graph_snapshots <qualified_wildcarded_name> #failures",
            GraphSnapshotsShowFailuresResult);
        #endregion

        #endregion

        #region Other show commands
        public static readonly CommandInfo ShowCertificates =
            new CommandInfo(nameof(ShowCertificates),
                $"show certificates",
                UnknownResult);

        public static readonly CommandInfo ShowCloudSettings =
            new CommandInfo(nameof(ShowCloudSettings),
                $"show cloudsettings",
                UnknownResult);

        public static readonly CommandInfo ShowCommConcurrency =
            new CommandInfo(nameof(ShowCommConcurrency),
                $"show commconcurrency",
                UnknownResult);

        public static readonly CommandInfo ShowCommPools =
            new CommandInfo(nameof(ShowCommPools),
                $"show commpools",
                UnknownResult);

        public static readonly CommandInfo ShowFabricCache =
            new CommandInfo(nameof(ShowFabricCache),
                $"show fabriccache",
                UnknownResult);

        public static readonly CommandInfo ShowFabricLocks =
            new CommandInfo(nameof(ShowFabricLocks),
                $"show fabriclocks",
                UnknownResult);

        public static readonly CommandInfo ShowFabricClocks =
            new CommandInfo(nameof(ShowFabricClocks),
                $"show fabricclocks",
                UnknownResult);

        public static readonly CommandInfo ShowFeatureFlags =
            new CommandInfo(nameof(ShowFeatureFlags),
                $"show featureflags",
                UnknownResult);

        public static readonly CommandInfo ShowMemPools =
            new CommandInfo(nameof(ShowMemPools),
                $"show mempools",
                UnknownResult);

        public static readonly CommandInfo ShowServicePoints =
            new CommandInfo(nameof(ShowServicePoints),
                $"show servicepoints",
                UnknownResult);

        public static readonly CommandInfo ShowTcpConnections =
            new CommandInfo(nameof(ShowTcpConnections),
                $"show tcpconnections",
                UnknownResult);

        public static readonly CommandInfo ShowTcpPorts =
            new CommandInfo(nameof(ShowTcpPorts),
                $"show tcpports",
                UnknownResult);

        public static readonly CommandInfo ShowThreadPools =
            new CommandInfo(nameof(ShowThreadPools),
                $"show threadpools",
                UnknownResult);
        #endregion

        #region uncategorized
        public static readonly CommandInfo ExecuteDatabaseScript =
            new CommandInfo(nameof(ExecuteDatabaseScript),
                $"execute [database] script [{PropertyList("ContinueOnErrors|ThrowOnErrors")}] '<|' <input_script>",
                UnknownResult);

        public static readonly CommandInfo ExecuteClusterScript =
            new CommandInfo(nameof(ExecuteClusterScript),
                $"execute cluster script [{PropertyList("ContinueOnErrors|ThrowOnErrors")}] '<|' <input_script>",
                UnknownResult);

        public static readonly CommandInfo CreateRequestSupport =
            new CommandInfo(nameof(CreateRequestSupport),
                $"create request_support [{PropertyList()}]",
                UnknownResult);

        public static readonly CommandInfo ShowRequestSupport =
            new CommandInfo(nameof(ShowRequestSupport),
                "show request_support key=<string>",
                UnknownResult);

        public static readonly CommandInfo ShowClusterAdminState =
            new CommandInfo(nameof(ShowClusterAdminState),
                "show cluster admin state",
                UnknownResult);

        public static readonly CommandInfo ClearRemoteClusterDatabaseSchema =
            new CommandInfo(nameof(ClearRemoteClusterDatabaseSchema),
                "clear cache remote-schema cluster '(' clusterName=<string> ')' '.' database '(' databaseName=<string> ')'",
                UnknownResult);

        public static readonly CommandInfo ShowClusterMonitoring =
            new CommandInfo(nameof(ShowClusterMonitoring),
                "show cluster monitoring",
                UnknownResult);

        public static readonly CommandInfo ShowClusterScaleIn =
            new CommandInfo(nameof(ShowClusterScaleIn),
                "show cluster scalein num=(<int> | <long>) nodes",
                UnknownResult);

        public static readonly CommandInfo ShowClusterServices =
            new CommandInfo(nameof(ShowClusterServices),
                "show cluster services",
                "(NodeId: string, IsClusterAdmin: bool, IsFabricManager: bool, IsDatabaseAdmin: bool" +
                ", IsWeakConsistencyNode: bool, IsRowStoreHostNode: bool, IsDataNode: bool)");

        public static readonly CommandInfo ShowClusterNetwork =
            new CommandInfo(nameof(ShowClusterNetwork),
                "show cluster network [bytes=<long>]",
                UnknownResult);

        public static readonly CommandInfo AlterClusterStorageKeys =
            new CommandInfo(nameof(AlterClusterStorageKeys),
                $"alter [async] cluster storage keys [{PropertyList()}] decryption-certificate-thumbprint thumbprint=<string>",
                UnknownResult);

        public static readonly CommandInfo ShowClusterStorageKeysHash =
            new CommandInfo(nameof(ShowClusterStorageKeysHash),
            "show cluster storage keys hash",
            UnknownResult);

        public static readonly CommandInfo AlterFabricServiceAssignmentsCommand =
            new CommandInfo(nameof(AlterFabricServiceAssignmentsCommand),
                $"alter fabric service serviceType=<string> (assignment serviceId=<string> to nodeId=<string> | assignments serviceToNode=<string>)",
                UnknownResult);

        public static readonly CommandInfo DropFabricServiceAssignmentsCommand =
            new CommandInfo(nameof(DropFabricServiceAssignmentsCommand),
                $"drop fabric service serviceType=<string> assignments",
                UnknownResult);

        public static readonly CommandInfo CreateEntityGroupCommand = new CommandInfo(nameof(CreateEntityGroupCommand), "create entity_group EntityGroupName=<name> [ifnotexists] '(' ({(cluster '(' clusterName=<string> ')' | cluster '(' clusterName=<string> ')' '.' database '(' databaseName=<string> ')' | database '(' databaseName=<string> ')'), ','}+ ) ')'", EntityGroupShowResult);

        public static readonly CommandInfo CreateOrAlterEntityGroupCommand = new CommandInfo(nameof(CreateOrAlterEntityGroupCommand), "create-or-alter entity_group EntityGroupName=<name> '(' ({(cluster '(' clusterName=<string> ')' '.' database '(' databaseName=<string> ')' | database '(' databaseName=<string> ')'), ','}+ ) ')'", EntityGroupShowResult);

        public static readonly CommandInfo AlterEntityGroup = new CommandInfo(nameof(AlterEntityGroup), "alter entity_group EntityGroupName=<entitygroup> '(' ({(cluster '(' clusterName=<string> ')' '.' database '(' databaseName=<string> ')' | database '(' databaseName=<string> ')'), ','}+ ')')", EntityGroupShowResult);

        public static readonly CommandInfo AlterMergeEntityGroup = new CommandInfo(nameof(AlterMergeEntityGroup), "alter-merge entity_group EntityGroupName=<entitygroup> '(' ({(cluster '(' clusterName=<string> ')' '.' database '(' databaseName=<string> ')' | database '(' databaseName=<string> ')'), ','}+) ')'", EntityGroupShowResult);

        public static readonly CommandInfo DropEntityGroup = new CommandInfo(nameof(DropEntityGroup), "drop entity_group EntityGroupName=<entitygroup>", EntityGroupShowResult);

        public static readonly CommandInfo ShowEntityGroup = new CommandInfo(nameof(ShowEntityGroup), "show entity_group EntityGroupName=<entitygroup>", EntityGroupShowResult);

        public static readonly CommandInfo ShowEntityGroups = new CommandInfo(nameof(ShowEntityGroups), "show entity_groups", EntityGroupShowResult);

        // extent containers
        private static readonly string ExtentContainers =
           "(ExtentContainerId:guid, Url:string, State:string, CreatedOn:datetime, MaxDateTime:datetime, IsRecyclable:bool, StoresDatabaseMetadataPointer:bool, HardDeletePeriod:timespan, ActiveMetadataContainer:bool, MetadataContainer:bool)";

        public static readonly CommandInfo AlterExtentContainersAdd =
            new CommandInfo(nameof(AlterExtentContainersAdd),
                "alter extentcontainers databaseName=<database> add container=<string> [hardDeletePeriod=<timespan> containerId=<guid>]",
                ExtentContainers);

        public static readonly CommandInfo AlterExtentContainersDrop =
            new CommandInfo(nameof(AlterExtentContainersDrop),
                "alter extentcontainers databaseName=<database> drop [container=<guid>]",
                UnknownResult);

        public static readonly CommandInfo AlterExtentContainersRecycle =
            new CommandInfo(nameof(AlterExtentContainersRecycle),
                "alter extentcontainers databaseName=<database> recycle (container=<guid> | older hours=(<int> | <long>) hours)",
                UnknownResult);

        public static readonly CommandInfo AlterExtentContainersSet =
            new CommandInfo(nameof(AlterExtentContainersSet),
                "alter extentcontainers databaseName=<database> set state container=<guid> to (readonly | readwrite)",
                ExtentContainers);

        public static readonly CommandInfo ShowExtentContainers =
            new CommandInfo(nameof(ShowExtentContainers),
                $"show extentcontainers [{PropertyList()}]",
                ExtentContainers);

        public static readonly CommandInfo DropEmptyExtentContainers =
            new CommandInfo(nameof(DropEmptyExtentContainers),
                $"drop [async] empty extentcontainers databaseName=<database> until '=' d=<datetime> [whatif] [{PropertyList()}]",
                UnknownResult);

        public static readonly CommandInfo CleanDatabaseExtentContainers =
            new CommandInfo(nameof(CleanDatabaseExtentContainers),
                "clean databases [async] ['(' {databaseName=<database>, ','} ')'] extentcontainers",
                UnknownResult);

        public static readonly CommandInfo ShowDatabaseExtentContainersCleanOperations =
            new CommandInfo(nameof(ShowDatabaseExtentContainersCleanOperations),
                "show database databaseName=<database> extentcontainers clean operations [obj=<guid>]",
                UnknownResult);

        public static readonly CommandInfo ClearDatabaseCacheQueryResults =
            new CommandInfo(nameof(ClearDatabaseCacheQueryResults),
                "clear database cache query_results",
                UnknownResult);

        public static readonly CommandInfo ShowDatabaseCacheQueryResults =
            new CommandInfo(nameof(ShowDatabaseCacheQueryResults),
                "show database cache query_results",
                UnknownResult);

        public static readonly CommandInfo ShowDatabasesManagementGroups =
            new CommandInfo(nameof(ShowDatabasesManagementGroups),
                "show databases management groups",
                UnknownResult);

        //public static readonly CommandInfo ShowDatabasesPolicies =
        //    new CommandInfo(nameof(ShowDatabasesPolicies),
        //        "show databases '(' {DatabaseName=<database>, ','} ')' policies '(' {policyName=<name>, ','} ')'",
        //        UnknownResult);

        //public static readonly CommandInfo ShowDatabasesPrincipals =
        //    new CommandInfo(nameof(ShowDatabasesPrincipals),
        //        $"show databases '(' {{DatabaseName=<database>, ','}} ')' principals [from tenants '(' {{tenant=<string>, ','}} ')'] [{PropertyList()}]",
        //        UnknownResult);

        public static readonly CommandInfo AlterDatabaseStorageKeys =
            new CommandInfo(nameof(AlterDatabaseStorageKeys),
                $"alter [async] database databaseName=<database> storage keys [{PropertyList()}] decryption-certificate-thumbprint thumbprint=<string>",
                UnknownResult);

        public static readonly CommandInfo ClearDatabaseCacheStreamingIngestionSchema =
            new CommandInfo(nameof(ClearDatabaseCacheStreamingIngestionSchema),
                "clear database cache streamingingestion schema",
                UnknownResult);

        public static readonly CommandInfo ClearDatabaseCacheQueryWeakConsistency =
            new CommandInfo(nameof(ClearDatabaseCacheQueryWeakConsistency),
                "clear database cache query_weak_consistency",
                UnknownResult);

        public static readonly CommandInfo ShowEntitySchema =
            new CommandInfo(nameof(ShowEntitySchema),
                $"show entity entity=<qualified_name> schema as json [in databases '(' {{item=<string>, ','}}+ ')'] [except excludedFunctions=<string>] [{PropertyList()}]",
                UnknownResult);

        public static readonly CommandInfo ShowExtentDetails =
            new CommandInfo(nameof(ShowExtentDetails),
                "show tableName=<name> extent [details] [eid=<guid>] [with '(' { PropertyName=<name> '=' Value=<value>, ',' } ')']",
                UnknownResult);

        public static readonly CommandInfo ShowExtentColumnStorageStats =
            new CommandInfo(nameof(ShowExtentColumnStorageStats),
                "show tableName=<name> extent extentId=<guid> column columnName=<name> storage stats",
                UnknownResult);

        public static readonly CommandInfo AttachExtentsIntoTableByContainer =
            new CommandInfo(nameof(AttachExtentsIntoTableByContainer),
                "attach extents into table tableName=<table> by container containerUri=<string> {eid=<guid>}+",
                UnknownResult);

        public static readonly CommandInfo AttachExtentsIntoTableByMetadata =
            new CommandInfo(nameof(AttachExtentsIntoTableByMetadata),
                "attach [async] extents {into table tableName=<table>} by metadata csl=('<|' <input_query>)",
                UnknownResult);

        public static readonly CommandInfo AlterExtentTagsFromQuery =
            new CommandInfo(nameof(AlterExtentTagsFromQuery),
                "alter [async] table tableName=<table> extent tags '(' {t=<string>, ','}+ ')' [with '(' { PropertyName=<name> '=' Value=<value>, ',' } ')'] csl=('<|' <input_query>)",
                UnknownResult);

        public static readonly CommandInfo AlterMergeExtentTagsFromQuery =
            new CommandInfo(nameof(AlterMergeExtentTagsFromQuery),
                "alter-merge [async] table tableName=<table> extent tags '(' {t=<string>, ','}+ ')' [with '(' { PropertyName=<name> '=' Value=<value>, ',' } ')'] csl=('<|' <input_query>)",
                UnknownResult);

        public static readonly CommandInfo DropExtentTagsFromQuery =
            new CommandInfo(nameof(DropExtentTagsFromQuery),
                "drop [async] table tableName=<table> extent tags [with '(' { PropertyName=<name> '=' Value=<value>, ',' } ')'] csl=('<|' <input_query>) ",
                UnknownResult);

        public static readonly CommandInfo DropExtentTagsFromTable =
            new CommandInfo(nameof(DropExtentTagsFromTable),
                "drop [async] table tableName=<table> extent tags '(' {t=<string>, ','}+ ')' [with '(' { PropertyName=<name> '=' Value=<value>, ',' } ')']",
                UnknownResult);

        public static readonly CommandInfo DropExtentTagsRetention =
            new CommandInfo(nameof(DropExtentTagsRetention),
                "drop extent tags retention",
                UnknownResult);

        public static readonly CommandInfo MergeDatabaseShardGroups =
            new CommandInfo(nameof(MergeDatabaseShardGroups),
                "merge #database #shard-groups",
                UnknownResult);

        public static readonly CommandInfo AlterFollowerClusterConfiguration =
            new CommandInfo(nameof(AlterFollowerClusterConfiguration),
                "alter follower cluster configuration from leaderClusterMetadataPath=<string> (follow-authorized-principals '=' followAuthorizedPrincipals=<bool> | default-principals-modification-kind '=' modificationKind=(none | union | replace) | default-caching-policies-modification-kind '=' modificationKind=(none | union | replace) | database-name-prefix '=' databaseNamePrefix=<name>)",
                UnknownResult);

        public static readonly CommandInfo AddFollowerDatabaseAuthorizedPrincipals =
            new CommandInfo(nameof(AddFollowerDatabaseAuthorizedPrincipals),
                "add follower database dbName=<database> [from leaderClusterMetadataPath=<string>] operationRole=(admins | users | viewers | unrestrictedviewers | monitors) '(' {principal=<string>, ','}+ ')' [notes=<string>]",
                UnknownResult);

        public static readonly CommandInfo DropFollowerDatabaseAuthorizedPrincipals =
            new CommandInfo(nameof(DropFollowerDatabaseAuthorizedPrincipals),
                "drop follower database dbName=<database> operationRole=(admins | users | viewers | unrestrictedviewers | monitors) [from leaderClusterMetadataPath=<string>] '(' {principal=<string>, ','}+ ')'",
                UnknownResult);

        public static readonly CommandInfo AlterFollowerDatabaseAuthorizedPrincipals =
            new CommandInfo(nameof(AlterFollowerDatabaseAuthorizedPrincipals),
                "alter follower database dbName=<database> [from leaderClusterMetadataPath=<string>] policy caching (hotdata '=' hotDataToken=<timespan> hotindex '=' hotIndexToken=<timespan> | hot '=' hotToken=<timespan>) hotWindows=[[','] {hot_window '=' p=(d1=<datetime> '..' d2=<datetime>), ','}+]",
                UnknownResult);

        public static readonly CommandInfo DropFollowerDatabasePolicyCaching =
            new CommandInfo(nameof(DropFollowerDatabasePolicyCaching),
                "delete follower database dbName=<database> policy caching",
                UnknownResult);

        public static readonly CommandInfo AlterFollowerDatabaseChildEntities =
            new CommandInfo(nameof(AlterFollowerDatabaseChildEntities),
                "alter follower database dbName=<database> [from leaderClusterMetadataPath=<string>] (tables | external tables | materialized-views) entityListKind=(exclude | include) operationName=(add | drop) '(' {ename=<wildcarded_name>, ','}+ ')'",
                UnknownResult);

        public static readonly CommandInfo AlterFollowerDatabaseConfiguration =
            new CommandInfo(nameof(AlterFollowerDatabaseConfiguration),
                "alter follower database dbName=<database> [from leaderClusterMetadataPath=<string>] (principals-modification-kind '=' modificationKind=(none | union | replace) | caching-policies-modification-kind '=' modificationKind=(none | union | replace) | prefetch-extents '=' prefetchExtents=<bool> | metadata serializedDatabaseMetadataOverride=<string> | database-name-override '=' databaseNameOverride=<name>)",
                UnknownResult);

        //public static readonly CommandInfo AddFollowerDatabase =
        //    new CommandInfo(nameof(AddFollowerDatabase),
        //        "add follower database databaseName=<database> from leaderClusterMetadataPath=<string> metadata serializedDatabaseMetadataOverride=<string> | add follower (database databaseName=<wildcarded_name> [tables [exclude '(' {tableName=<wildcarded_name>, ','}+ ')'] [include '(' {tableName=<wildcarded_name>, ','}+ ')']] [external tables [exclude '(' {externalTableName=<wildcarded_name>, ','}+ ')'] [include '(' {externalTableName=<wildcarded_name>, ','}+ ')']] [materialized-views [exclude '(' {materializedViewName=<wildcarded_name>, ','}+ ')'] [include '(' {materializedViewName=<wildcarded_name>, ','}+ ')']] | databases '(' {databaseName=<database>, ','}+ ')') from leaderClusterMetadataPath=<string> [default-principals-modification-kind '=' modificationKind=(none | union | replace)] [default-caching-policies-modification-kind '=' modificationKind=(none | union | replace)]",
        //        UnknownResult);

        public static readonly CommandInfo DropFollowerDatabases =
            new CommandInfo(nameof(DropFollowerDatabases),
                "drop follower (database databaseName=<database> | databases '(' {databaseName=<database>, ','}+ ')') from leaderClusterMetadataPath=<string>",
                UnknownResult);

        public static readonly CommandInfo ShowFollowerDatabase =
            new CommandInfo(nameof(ShowFollowerDatabase),
                "show follower (database databaseName=<database> | databases ['(' {databaseName=<database>, ','} ')'])",
                UnknownResult);

        public static readonly CommandInfo AlterFollowerTablesPolicyCaching =
            new CommandInfo(nameof(AlterFollowerTablesPolicyCaching),
                "alter follower database dbName=<database> [from leaderClusterMetadataPath=<string>] (table name=<table> | materialized-view name=<materializedview> | tables '(' {name=<name>, ','}+ ')' | materialized-views '(' {name=<name>, ','}+ ')') policy caching (hotdata '=' hotDataToken=<timespan> hotindex '=' hotIndexToken=<timespan> | hot '=' hotToken=<timespan>) hotWindows=[[','] {hot_window '=' p=(d1=<datetime> '..' d2=<datetime>), ','}+]",
                UnknownResult);

        public static readonly CommandInfo DropFollowerTablesPolicyCaching =
            new CommandInfo(nameof(DropFollowerTablesPolicyCaching),
                "delete follower database dbName=<database> (table name=<table> | materialized-view name=<materializedview> | tables '(' {name=<name>, ','}+ ')' | materialized-views '(' {name=<name>, ','}+ ')') policy caching",
                UnknownResult);

        public static readonly CommandInfo ShowFreshness =
            new CommandInfo(nameof(ShowFreshness),
                "show #freshness tableName=<table> [column columnName=<column>] [threshold threshold=<long>]",
                UnknownResult);

        public static readonly CommandInfo ShowFunctionSchemaAsJson =
            new CommandInfo(nameof(ShowFunctionSchemaAsJson),
                "show function functionName=<function> schema as json [with '(' { PropertyName=<name> '=' Value=<value>, ',' } ')']",
                UnknownResult);

        public static readonly CommandInfo SetMaterializedViewAdmins =
            new CommandInfo(nameof(SetMaterializedViewAdmins),
                "set materialized-view materializedViewName=<materializedview> admins (none | '(' {principal=<string>, ','}+ ')' [notes=<string>])",
                UnknownResult);

        public static readonly CommandInfo AddMaterializedViewAdmins =
            new CommandInfo(nameof(AddMaterializedViewAdmins),
                "add materialized-view materializedViewName=<materializedview> admins '(' {principal=<string>, ','}+ ')' [notes=<string>]",
                UnknownResult);

        public static readonly CommandInfo DropMaterializedViewAdmins =
            new CommandInfo(nameof(DropMaterializedViewAdmins),
                "drop materialized-view materializedViewName=<materializedview> admins '(' {principal=<string>, ','}+ ')' [notes=<string>]",
                UnknownResult);

        public static readonly CommandInfo SetMaterializedViewConcurrency =
            new CommandInfo(nameof(SetMaterializedViewConcurrency),
                "set materialized-view viewName=<materializedview> concurrency ['=' n=(<int> | <long>)]",
                UnknownResult);

        public static readonly CommandInfo ClearMaterializedViewStatistics =
            new CommandInfo(nameof(ClearMaterializedViewStatistics),
                "clear materialized-view viewName=<materializedview> statistics",
                UnknownResult);

        public static readonly CommandInfo ShowMaterializedViewStatistics =
            new CommandInfo(nameof(ShowMaterializedViewStatistics),
                "show materialized-view viewName=<materializedview> statistics",
                UnknownResult);

        public static readonly CommandInfo ShowMaterializedViewDiagnostics =
            new CommandInfo(nameof(ShowMaterializedViewDiagnostics),
                $"show materialized-view viewName=<materializedview> diagnostics [{PropertyList()}]",
                UnknownResult);

        public static readonly CommandInfo ShowMaterializedViewFailures =
            new CommandInfo(nameof(ShowMaterializedViewFailures),
                "show materialized-view viewName=<materializedview> failures",
                UnknownResult);

        public static readonly CommandInfo ShowMemory =
            new CommandInfo(nameof(ShowMemory),
                "show memory [details]",
                UnknownResult);

        public static readonly CommandInfo CancelOperation =
            new CommandInfo(nameof(CancelOperation),
                $"cancel operation obj=<guid> [{PropertyList()}]",
                UnknownResult);

        public static readonly CommandInfo DisablePlugin =
            new CommandInfo(nameof(DisablePlugin),
                "disable plugin pluginName=(<string> | <name>)",
                UnknownResult);

        public static readonly CommandInfo EnablePlugin =
            new CommandInfo(nameof(EnablePlugin),
                "enable plugin name=(<string> | <name>)",
                UnknownResult);

        public static readonly CommandInfo ShowPlugins =
            new CommandInfo(nameof(ShowPlugins),
                $"show plugins [{PropertyList()}]",
                UnknownResult);

        public static readonly CommandInfo ShowPrincipalAccess =
            new CommandInfo(nameof(ShowPrincipalAccess),
                $"show principal access [{PropertyList()}]",
                UnknownResult);

        public static readonly CommandInfo ShowDatabasePurgeOperation =
            new CommandInfo(nameof(ShowDatabasePurgeOperation),
                "show database databaseName=<database> purge (operation obj=<guid> | operations [obj=<guid>])",
                UnknownResult);

        public static readonly CommandInfo ShowQueryExecution =
            new CommandInfo(nameof(ShowQueryExecution),
                "show queryexecution queryText=('<|' <input_query>)",
                UnknownResult);

        public static readonly CommandInfo AlterPoliciesOfRetention =
            new CommandInfo(nameof(AlterPoliciesOfRetention),
                "alter policies of retention [internal] policies=<string>",
                UnknownResult);

        public static readonly CommandInfo DeletePoliciesOfRetention =
            new CommandInfo(nameof(DeletePoliciesOfRetention),
                "delete policies of retention '(' {entity=<string>, ','}+ ')'",
                UnknownResult);

        //public static readonly CommandInfo AttachRowStore =
        //    new CommandInfo(nameof(AttachRowStore),
        //        $"attach rowstore rowStoreName=<name> rowStoreId=<guid> writeaheadlog waLogPath=<string> [{PropertyList()}]",
        //        UnknownResult);

        public static readonly CommandInfo CreateRowStore =
            new CommandInfo(nameof(CreateRowStore),
                $"create rowstore [{PropertyList()}]",
                UnknownResult);

        public static readonly CommandInfo DropRowStore =
            new CommandInfo(nameof(DropRowStore),
                "(drop | detach) rowstore rowStoreName=<name> [ifexists]",
                UnknownResult);

        private static readonly string RowStore = "(RowStoreName:string,RowStoreId:guid,RowStoreKey:string,OrdinalFrom:long,OrdinalTo:long,EstimatedDataSize:long,MinWriteAheadLogOffset:long, " +
            "LocalStorageSize:long,LocalStorageStartOffset:long,Status:int,StatusLastUpdatedOn:datetime,DatabaseName:string,TableName:string,AssignedToNode:string,LatestIngestionTime:datetime)";
        public static readonly CommandInfo ShowRowStore =
            new CommandInfo(nameof(ShowRowStore),
                "show rowstore rowStoreName=<name>",
                RowStore);

        private static readonly string RowStores =
        "(RowStoreName:string,RowStoreId:guid,WriteAheadLogStorage:string,PersistentStorage:string,IsActive:bool,AssignedToNode:string,NumberOfKeys:long,WriteAheadLogSize:long, " +
          "WriteAheadLogStartOffset:long,LocalStorageSize:long,WriteAheadDistanceToSizeRatioThreshold:real,InsertsConcurrencyLimit:long,KeyInsertsConcurrencyLimit:long,UnsealedSizePerKeyLimit:long, " +
          "NodeInsertsConcurrencyLimit:long,Status:string,StatusLastUpdatedOn:datetime,UsageTags:string,IsEmpty:bool,IsDataAvailableForQuery:bool)";

        public static readonly CommandInfo ShowRowStores =
            new CommandInfo(nameof(ShowRowStores),
                "show rowstores",
                RowStores);

        public static readonly CommandInfo ShowRowStoreTransactions =
            new CommandInfo(nameof(ShowRowStoreTransactions),
                "show rowstore transactions",
                UnknownResult);

        public static readonly CommandInfo ShowRowStoreSeals =
            new CommandInfo(nameof(ShowRowStoreSeals),
                $"show rowstore seals tableName=<string> [{PropertyList()}]",
                UnknownResult);

        public static readonly CommandInfo ShowSchema =
            new CommandInfo(nameof(ShowSchema),
                "show [cluster] schema [details | as json]",
                UnknownResult);

        public static readonly CommandInfo ShowCallStacks =
            new CommandInfo(nameof(ShowCallStacks),
                $"show callstacks [{PropertyList()}]",
                UnknownResult);

        public static readonly CommandInfo ShowFileSystem =
            new CommandInfo(nameof(ShowFileSystem),
                $"show filesystem [{PropertyList()}]",
                UnknownResult);

        public static readonly CommandInfo ShowRunningCallouts =
            new CommandInfo(nameof(ShowRunningCallouts),
                $"show running callouts",
                UnknownResult);

        private static readonly string StreamingIngestionFailures =
            "(Database: string, Table: string, Principal: string, RootActivityId: guid, IngestionProperties: dynamic, Count: long, FirstFailureOn: datetime, LastFailureOn: datetime, FailureKind: string, ErrorCode: string, Details: string)";

        public static readonly CommandInfo ShowStreamingIngestionFailures =
            new CommandInfo(nameof(ShowStreamingIngestionFailures),
                "show streamingingestion failures",
                StreamingIngestionFailures);

        private static readonly string StreamingIngestionStatistics =
            "(Database:string, Table:string, StartTime:datetime, EndTime:datetime, Count:long, MinDuration:timespan, MaxDuration:timespan, AvgDuration:timespan, TotalDataSize:long, " +
            "MinDataSize:long, MaxDataSize:long, TotalRowCount:long, MinRowCount:long, MaxRowCount:long, IngestionStatus:string, NumOfRowStoresReferences:int, " +
            "Principal:string, NodeId:string, IngestionProperties:dynamic)";

        public static readonly CommandInfo ShowStreamingIngestionStatistics =
            new CommandInfo(nameof(ShowStreamingIngestionStatistics),
                "show streamingingestion statistics",
                StreamingIngestionStatistics);

        public static readonly CommandInfo AlterTableRowStoreReferencesDropKey =
            new CommandInfo(nameof(AlterTableRowStoreReferencesDropKey),
                $@"alter table TableName=<database_table> rowstore_references drop key rowStoreKey=<string> [{PropertyList()}]",
                UnknownResult);

        public static readonly CommandInfo AlterTableRowStoreReferencesDropRowStore =
            new CommandInfo(nameof(AlterTableRowStoreReferencesDropRowStore),
                $@"alter table TableName=<database_table> rowstore_references drop rowstore rowStoreName=<name> [{PropertyList()}]",
                UnknownResult);

        public static readonly CommandInfo AlterTableRowStoreReferencesDropBlockedKeys =
            new CommandInfo(nameof(AlterTableRowStoreReferencesDropBlockedKeys),
                $@"alter table TableName=<database_table> rowstore_references drop blocked keys [{PropertyList()}]",
                UnknownResult);

        public static readonly CommandInfo AlterTableRowStoreReferencesDisableKey =
            new CommandInfo(nameof(AlterTableRowStoreReferencesDisableKey),
                $@"alter table TableName=<database_table> rowstore_references disable key rowStoreKey=<string> [{PropertyList()}]",
                UnknownResult);

        public static readonly CommandInfo AlterTableRowStoreReferencesDisableRowStore =
            new CommandInfo(nameof(AlterTableRowStoreReferencesDisableRowStore),
                $@"alter table TableName=<database_table> rowstore_references disable rowstore rowStoreName=<name> [{PropertyList()}]",
                UnknownResult);

        public static readonly CommandInfo AlterTableRowStoreReferencesDisableBlockedKeys =
            new CommandInfo(nameof(AlterTableRowStoreReferencesDisableBlockedKeys),
                $@"alter table TableName=<database_table> rowstore_references disable blocked keys [{PropertyList()}]",
                UnknownResult);

        public static readonly CommandInfo SetTableRowStoreReferences =
            new CommandInfo(nameof(SetTableRowStoreReferences),
                "set table TableName=<database_table> rowstore_references references=<string>",
                UnknownResult);

        private static readonly string TableRowStoreReferences =
            "(DatabaseName:string, TableName:string, RowstoreReferenceKey:string, RowStoreName:string, EnabledForIngestion:bool)";

        public static readonly CommandInfo ShowTableRowStoreReferences =
            new CommandInfo(nameof(ShowTableRowStoreReferences),
                "show table TableName=<database_table> rowstore_references",
                TableRowStoreReferences);

        public static readonly CommandInfo AlterTableColumnStatistics =
            new CommandInfo(nameof(AlterTableColumnStatistics),
                "alter table TableName=<table> column statistics {c2=<name> statisticsValues2=<string>, ','}",
                UnknownResult);

        public static readonly CommandInfo AlterTableColumnStatisticsMethod =
            new CommandInfo(nameof(AlterTableColumnStatisticsMethod),
                "alter table TableName=<database_table> column statistics method '=' newMethod=<string>",
                UnknownResult);

        public static readonly CommandInfo ShowTableColumnStatitics =
            new CommandInfo(nameof(ShowTableColumnStatitics),
                "show table TableName=<table> column statistics",
                UnknownResult);

        public static readonly CommandInfo ShowTableDimensions =
            new CommandInfo(nameof(ShowTableDimensions),
                "show table TableName=<table> dimensions",
                UnknownResult);

        public static readonly CommandInfo DeleteTableRecords =
            new CommandInfo(nameof(DeleteTableRecords),
                $"delete [async] table TableName=<table> records [{PropertyList()}] csl=('<|' <input_query>)",
                UnknownResult);

        public static readonly CommandInfo TableDataUpdate =
            new CommandInfo(nameof(TableDataUpdate),
                $"update [async] table TableName=<table> delete DeleteIdentifier=<name> append AppendIdentifier=<name> [{PropertyList()}] csl=('<|' <input_query>)",
                UnknownResult);

        public static readonly CommandInfo DeleteMaterializedViewRecords =
            new CommandInfo(nameof(DeleteMaterializedViewRecords),
                $"delete [async] materialized-view MaterializedViewName=<materializedview> records [{PropertyList()}] csl=('<|' <input_query>)",
                UnknownResult);

        public static readonly CommandInfo ShowTableColumnsClassification =
            new CommandInfo(nameof(ShowTableColumnsClassification),
                "show table TableName=<table> columns classification",
                UnknownResult);

        private static readonly string TableRowStores =
            "(DatabaseName:string,TableName:string,ExtentId:guid,IsSealed:bool,RowStoreName:string,RowStoreId:string,RowStoreKey:string,OrdinalFrom:long," +
            "OrdinalTo:long,WriteAheadLogSize:long,LocalStorageSize:long,EstimatedDataSize:long,MinWriteAheadLogOffset:long)";

        public static readonly CommandInfo ShowTableRowStores =
            new CommandInfo(nameof(ShowTableRowStores),
                "show table tableName=<database_table> rowstores",
                TableRowStores);

        public static readonly CommandInfo ShowTableRowStoreSealInfo =
            new CommandInfo(nameof(ShowTableRowStoreSealInfo),
                "show table tableName=<database_table> rowstore_sealinfo",
                UnknownResult);

        //public static readonly CommandInfo ShowDatabasesTablesPolicies =
        //    new CommandInfo(nameof(ShowDatabasesTablesPolicies),
        //        "show databases '(' {DatabaseName=<database>, ','} ')' tables policies '(' {PolicyName=<name>, ','} ')'",
        //        UnknownResult);

        public static readonly CommandInfo ShowTablesColumnStatistics =
            new CommandInfo(nameof(ShowTablesColumnStatistics),
                "show tables column statistics older outdatewindow=<timespan>",
                UnknownResult
                );

        public static readonly CommandInfo ShowTableDataStatistics =
            new CommandInfo(nameof(ShowTableDataStatistics),
                $"show table TableName=<table> data statistics [{PropertyList("samplepercent|scope|from|to")}]",
                "(ColumnName: string, ColumnType: string, ColumnId: guid, OriginalSize: long, ExtentSize: long, " +
                "CompressionRatio: real, DataCompressionSize: long, SharedIndexSize: long, IndexSize: long, IndexSizePercent: real," +
                "StorageEngineVersion: string, PresentRowCount: long, DeletedRowCount: long, SamplePercent: real, IncludeColdData: bool)");

        public static readonly CommandInfo CreateTempStorage =
            new CommandInfo(nameof(CreateTempStorage),
                "create tempstorage",
                UnknownResult);

        public static readonly CommandInfo DropTempStorage =
            new CommandInfo(nameof(DropTempStorage),
                "drop tempstorage older olderThan=<timespan>",
                UnknownResult);

        public static readonly CommandInfo DropStoredQueryResultContainers =
            new CommandInfo(nameof(DropStoredQueryResultContainers),
                "drop storedqueryresultcontainers DatabaseName=<database> {containerId=<guid>}+",
                UnknownResult);

        public static readonly CommandInfo DropUnusedStoredQueryResultContainers =
            new CommandInfo(nameof(DropUnusedStoredQueryResultContainers),
                "drop unused storedqueryresultcontainers databaseName=<database>",
                UnknownResult);

        public static readonly CommandInfo EnableDatabaseMaintenanceMode =
            new CommandInfo(nameof(EnableDatabaseMaintenanceMode),
                "enable database DatabaseName=<database> maintenance_mode",
                UnknownResult);

        public static readonly CommandInfo DisableDatabaseMaintenanceMode =
            new CommandInfo(nameof(DisableDatabaseMaintenanceMode),
                "disable database DatabaseName=<database> maintenance_mode",
                UnknownResult);

        public static readonly CommandInfo EnableDatabaseStreamingIngestionMaintenanceMode =
            new CommandInfo(nameof(EnableDatabaseStreamingIngestionMaintenanceMode),
                "enable database streamingingestion_maintenance_mode",
                UnknownResult);

        public static readonly CommandInfo DisableDatabaseStreamingIngestionMaintenanceMode =
            new CommandInfo(nameof(DisableDatabaseStreamingIngestionMaintenanceMode),
                "disable database streamingingestion_maintenance_mode",
                UnknownResult);

        public static readonly CommandInfo ShowQueryCallTree =
            new CommandInfo(nameof(ShowQueryCallTree),
                "show query call-tree queryText=('<|' <input_query>)",
                UnknownResult);

        public static readonly CommandInfo ShowExtentCorruptedDatetime =
            new CommandInfo(nameof(ShowExtentCorruptedDatetime),
                "show [table <table>] extents #(corrupted datetime)",
                UnknownResult);

        public static readonly CommandInfo PatchExtentCorruptedDatetime =
            new CommandInfo(nameof(PatchExtentCorruptedDatetime),
                "#(patch [table <table>] extents corrupted datetime)",
                UnknownResult);

        public static readonly CommandInfo ClearClusterCredStoreCache =
            new CommandInfo(nameof(ClearClusterCredStoreCache),
                "clear cluster cache credstore",
                UnknownResult);

        public static readonly CommandInfo ClearClusterGroupMembershipCache =
            new CommandInfo(nameof(ClearClusterGroupMembershipCache),
                "clear cluster cache groupmembership [with '(' { PropertyName=<name> '=' Value=<value>, ',' } ')']",
                UnknownResult);

        public static readonly CommandInfo ClearExternalArtifactsCache =
            new CommandInfo(nameof(ClearExternalArtifactsCache),
                "clear cluster cache external-artifacts ('(' {ArtifactUri=<string>, ','}+ ')')",
                UnknownResult);

        public static readonly CommandInfo ShowDatabasesEntities =
            new CommandInfo(nameof(ShowDatabasesEntities),
                "show databases entities [with '(' { PropertyName=<name> '=' Value=<value>, ',' } ')']",
                "(DatabaseName:string, EntityType:string, EntityName:string, DocString:string, Folder:string, CslInputSchema:string, Content:string, CslOutputSchema:string, Properties:dynamic)");

        public static readonly CommandInfo ShowDatabaseEntity =
            new CommandInfo(nameof(ShowDatabaseEntity),
                "show database DatabaseName=<database> entity EntityName=<name> [with '(' { PropertyName=<name> '=' Value=<value>, ',' } ')']",
                "(DatabaseName:string, EntityType:string, EntityName:string, DocString:string, Folder:string, CslInputSchema:string, Content:string, CslOutputSchema:string, Properties:dynamic)");

        public static readonly CommandInfo ReplaceDatabaseKeyVaultSecrets =
            new CommandInfo(nameof(ReplaceDatabaseKeyVaultSecrets),
                "#(replace database keyvaultsecrets secrets=<name>)",
                UnknownResult);
        #endregion
    }

#if !T4
}
#endif
// #>