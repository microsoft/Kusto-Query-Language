using System.Collections.Generic;

namespace Kusto.Language
{
    using Symbols;

    public static class Commands
    {
        #region Schema Commands
        #region Databases
        private static readonly TableSymbol ShowDatabaseResults =
            new TableSymbol(
                new ColumnSymbol("DatabaseName", ScalarTypes.String),
                new ColumnSymbol("PersistentStorage", ScalarTypes.String),
                new ColumnSymbol("Version", ScalarTypes.String),
                new ColumnSymbol("IsCurrent", ScalarTypes.Bool),
                new ColumnSymbol("DatabaseAccessMode", ScalarTypes.String),
                new ColumnSymbol("PrettyName", ScalarTypes.String),
                new ColumnSymbol("CurrentUseIsUnrestrictedViewer", ScalarTypes.Bool),
                new ColumnSymbol("DatabaseId", ScalarTypes.Guid));

        private static readonly TableSymbol ShowDatabaseDetailsResults =
            new TableSymbol(
                new ColumnSymbol("DatabaseName", ScalarTypes.String),
                new ColumnSymbol("PersistentStorage", ScalarTypes.String),
                new ColumnSymbol("Version", ScalarTypes.String),
                new ColumnSymbol("IsCurrent", ScalarTypes.Bool),
                new ColumnSymbol("DatabaseAccessMode", ScalarTypes.String),
                new ColumnSymbol("PrettyName", ScalarTypes.String),
                new ColumnSymbol("AuthorizedPrincipals", ScalarTypes.String),
                new ColumnSymbol("RetentionPolicy", ScalarTypes.String),
                new ColumnSymbol("MergePolicy", ScalarTypes.String),
                new ColumnSymbol("CachingPolicy", ScalarTypes.String),
                new ColumnSymbol("ShardingPolicy", ScalarTypes.String),
                new ColumnSymbol("StreamingIngestionPolicy", ScalarTypes.String),
                new ColumnSymbol("IngestionBatchingPolicy", ScalarTypes.String),
                new ColumnSymbol("TotalSize", ScalarTypes.Real),
                new ColumnSymbol("DatabaseId", ScalarTypes.Guid));

        public static readonly CommandSymbol ShowDatabase =
            new CommandSymbol("show database", ShowDatabaseResults);

        public static readonly CommandSymbol ShowDatabaseDetails =
            new CommandSymbol("show database details", "show database details", ShowDatabaseDetailsResults);

        public static readonly CommandSymbol ShowDatabaseIdentity =
            new CommandSymbol("show database identity",
                "show database identity",
                new TableSymbol(
                    new ColumnSymbol("DatabaseName", ScalarTypes.String),
                    new ColumnSymbol("PersistentStorage", ScalarTypes.String),
                    new ColumnSymbol("Version", ScalarTypes.String),
                    new ColumnSymbol("IsCurrent", ScalarTypes.Bool),
                    new ColumnSymbol("DatabaseAccessMode", ScalarTypes.String),
                    new ColumnSymbol("PrettyName", ScalarTypes.String),
                    new ColumnSymbol("CurrentUseIsUnrestrictedViewer", ScalarTypes.Bool),
                    new ColumnSymbol("DatabaseId", ScalarTypes.Guid)));

        public static readonly CommandSymbol ShowDatabasePolicies =
            new CommandSymbol("show database policies",
                "show database policies",
                new TableSymbol(
                    new ColumnSymbol("DatabaseName", ScalarTypes.String),
                    new ColumnSymbol("PersistentStorage", ScalarTypes.String),
                    new ColumnSymbol("Version", ScalarTypes.String),
                    new ColumnSymbol("IsCurrent", ScalarTypes.Bool),
                    new ColumnSymbol("DatabaseAccessMode", ScalarTypes.String),
                    new ColumnSymbol("PrettyName", ScalarTypes.String),
                    new ColumnSymbol("CurrentUseIsUnrestrictedViewer", ScalarTypes.Bool),
                    new ColumnSymbol("DatabaseId", ScalarTypes.Guid),
                    new ColumnSymbol("AuthorizedPrincipals", ScalarTypes.String),
                    new ColumnSymbol("RetentionPolicy", ScalarTypes.String),
                    new ColumnSymbol("MergePolicy", ScalarTypes.String),
                    new ColumnSymbol("CachingPolicy", ScalarTypes.String),
                    new ColumnSymbol("ShardingPolicy", ScalarTypes.String),
                    new ColumnSymbol("StreamingIngestionPolicy", ScalarTypes.String),
                    new ColumnSymbol("IngestionBatchingPolicy", ScalarTypes.String)));

        public static readonly CommandSymbol ShowDatabaseDataStats =
            new CommandSymbol("show database datastats",
                "show database datastats",
                new TableSymbol(
                    new ColumnSymbol("DatabaseName", ScalarTypes.String),
                    new ColumnSymbol("PersistentStorage", ScalarTypes.String),
                    new ColumnSymbol("Version", ScalarTypes.String),
                    new ColumnSymbol("IsCurrent", ScalarTypes.Bool),
                    new ColumnSymbol("DatabaseAccessMode", ScalarTypes.String),
                    new ColumnSymbol("PrettyName", ScalarTypes.String),
                    new ColumnSymbol("CurrentUseIsUnrestrictedViewer", ScalarTypes.Bool),
                    new ColumnSymbol("DatabaseId", ScalarTypes.Guid),
                    new ColumnSymbol("OriginalSize", ScalarTypes.Real),
                    new ColumnSymbol("ExtentSize", ScalarTypes.Real),
                    new ColumnSymbol("CompressedSize", ScalarTypes.Real),
                    new ColumnSymbol("IndexSize", ScalarTypes.Real),
                    new ColumnSymbol("RowCount", ScalarTypes.Long),
                    new ColumnSymbol("HotOriginalSize", ScalarTypes.Real),
                    new ColumnSymbol("HotExtentSize", ScalarTypes.Real),
                    new ColumnSymbol("HotCompressedSize", ScalarTypes.Real),
                    new ColumnSymbol("HotIndexSize", ScalarTypes.Real),
                    new ColumnSymbol("HotRowCount", ScalarTypes.Long)));

        public static readonly CommandSymbol ShowClusterDatabases =
            new CommandSymbol("show cluster databases", "show cluster databases ['(' { <database>:DatabaseName, ',' }+ ')']",
                new TableSymbol(
                    new ColumnSymbol("DatabaseName", ScalarTypes.String),
                    new ColumnSymbol("PersistentStorage", ScalarTypes.String),
                    new ColumnSymbol("Version", ScalarTypes.String),
                    new ColumnSymbol("IsCurrent", ScalarTypes.Bool),
                    new ColumnSymbol("DatabaseAccessMode", ScalarTypes.String),
                    new ColumnSymbol("PrettyName", ScalarTypes.String),
                    new ColumnSymbol("CurrentUseIsUnrestrictedViewer", ScalarTypes.Bool),
                    new ColumnSymbol("DatabaseId", ScalarTypes.Guid)));

        public static readonly CommandSymbol ShowClusterDatabasesDetails =
            new CommandSymbol("show cluster databases details",
                new TableSymbol(
                    new ColumnSymbol("DatabaseName", ScalarTypes.String),
                    new ColumnSymbol("PersistentStorage", ScalarTypes.String),
                    new ColumnSymbol("Version", ScalarTypes.String),
                    new ColumnSymbol("IsCurrent", ScalarTypes.Bool),
                    new ColumnSymbol("DatabaseAccessMode", ScalarTypes.String),
                    new ColumnSymbol("PrettyName", ScalarTypes.String),
                    new ColumnSymbol("AuthorizedPrincipals", ScalarTypes.String),
                    new ColumnSymbol("RetentionPolicy", ScalarTypes.String),
                    new ColumnSymbol("MergePolicy", ScalarTypes.String),
                    new ColumnSymbol("CachingPolicy", ScalarTypes.String),
                    new ColumnSymbol("ShardingPolicy", ScalarTypes.String),
                    new ColumnSymbol("StreamingIngestionPolicy", ScalarTypes.String),
                    new ColumnSymbol("IngestionBatchingPolicy", ScalarTypes.String),
                    new ColumnSymbol("TotalSize", ScalarTypes.Real),
                    new ColumnSymbol("DatabaseId", ScalarTypes.Guid)));

        public static readonly CommandSymbol ShowClusterDatabasesIdentity =
            new CommandSymbol("show cluster databases identity",
                new TableSymbol(
                    new ColumnSymbol("DatabaseName", ScalarTypes.String),
                    new ColumnSymbol("PersistentStorage", ScalarTypes.String),
                    new ColumnSymbol("Version", ScalarTypes.String),
                    new ColumnSymbol("IsCurrent", ScalarTypes.Bool),
                    new ColumnSymbol("DatabaseAccessMode", ScalarTypes.String),
                    new ColumnSymbol("PrettyName", ScalarTypes.String),
                    new ColumnSymbol("CurrentUseIsUnrestrictedViewer", ScalarTypes.Bool),
                    new ColumnSymbol("DatabaseId", ScalarTypes.Guid)));

        public static readonly CommandSymbol ShowClusterDatabasesPolicies =
            new CommandSymbol("show cluster databases policies",
                new TableSymbol(
                    new ColumnSymbol("DatabaseName", ScalarTypes.String),
                    new ColumnSymbol("PersistentStorage", ScalarTypes.String),
                    new ColumnSymbol("Version", ScalarTypes.String),
                    new ColumnSymbol("IsCurrent", ScalarTypes.Bool),
                    new ColumnSymbol("DatabaseAccessMode", ScalarTypes.String),
                    new ColumnSymbol("PrettyName", ScalarTypes.String),
                    new ColumnSymbol("CurrentUseIsUnrestrictedViewer", ScalarTypes.Bool),
                    new ColumnSymbol("DatabaseId", ScalarTypes.Guid),
                    new ColumnSymbol("AuthorizedPrincipals", ScalarTypes.String),
                    new ColumnSymbol("RetentionPolicy", ScalarTypes.String),
                    new ColumnSymbol("MergePolicy", ScalarTypes.String),
                    new ColumnSymbol("CachingPolicy", ScalarTypes.String),
                    new ColumnSymbol("ShardingPolicy", ScalarTypes.String),
                    new ColumnSymbol("StreamingIngestionPolicy", ScalarTypes.String),
                    new ColumnSymbol("IngestionBatchingPolicy", ScalarTypes.String)));

        public static readonly CommandSymbol ShowClusterDatabasesDataStats =
            new CommandSymbol("show cluster databases datastats",
                new TableSymbol(
                    new ColumnSymbol("DatabaseName", ScalarTypes.String),
                    new ColumnSymbol("PersistentStorage", ScalarTypes.String),
                    new ColumnSymbol("Version", ScalarTypes.String),
                    new ColumnSymbol("IsCurrent", ScalarTypes.Bool),
                    new ColumnSymbol("DatabaseAccessMode", ScalarTypes.String),
                    new ColumnSymbol("PrettyName", ScalarTypes.String),
                    new ColumnSymbol("CurrentUseIsUnrestrictedViewer", ScalarTypes.Bool),
                    new ColumnSymbol("DatabaseId", ScalarTypes.Guid),
                    new ColumnSymbol("OriginalSize", ScalarTypes.Real),
                    new ColumnSymbol("ExtentSize", ScalarTypes.Real),
                    new ColumnSymbol("CompressedSize", ScalarTypes.Real),
                    new ColumnSymbol("IndexSize", ScalarTypes.Real),
                    new ColumnSymbol("RowCount", ScalarTypes.Long),
                    new ColumnSymbol("HotOriginalSize", ScalarTypes.Real),
                    new ColumnSymbol("HotExtentSize", ScalarTypes.Real),
                    new ColumnSymbol("HotCompressedSize", ScalarTypes.Real),
                    new ColumnSymbol("HotIndexSize", ScalarTypes.Real),
                    new ColumnSymbol("HotRowCount", ScalarTypes.Long)));

        public static readonly CommandSymbol CreateDatabasePersist =
            new CommandSymbol("create database persist", "create database <name>:DatabaseName persist '(' { <string>:Container, ',' }+ ')' [ifnotexists]",
                new TableSymbol(
                    new ColumnSymbol("DatabaseName", ScalarTypes.String),
                    new ColumnSymbol("PersistentPath", ScalarTypes.String),
                    new ColumnSymbol("Created", ScalarTypes.Bool),
                    new ColumnSymbol("StoresMetadata", ScalarTypes.Bool),
                    new ColumnSymbol("StoresData", ScalarTypes.Bool)));

        public static readonly CommandSymbol CreateDatabaseVolatile =
            new CommandSymbol("create database volatile", "create database <name>:DatabaseName volatile [ifnotexists]",
                new TableSymbol(
                    new ColumnSymbol("DatabaseName", ScalarTypes.String),
                    new ColumnSymbol("PersistentPath", ScalarTypes.String),
                    new ColumnSymbol("Created", ScalarTypes.Bool),
                    new ColumnSymbol("StoresMetadata", ScalarTypes.Bool),
                    new ColumnSymbol("StoresData", ScalarTypes.Bool)));

        public static readonly CommandSymbol AttachDatabase =
            new CommandSymbol("attach database", "attach database [metadata] <database>:DatabaseName from (<string>:BlobContainerUrl ';' <string>:StorageAccountKey | <string>:Path)",
                new TableSymbol(
                    new ColumnSymbol("Step", ScalarTypes.String),
                    new ColumnSymbol("Duration", ScalarTypes.String)));

        public static readonly CommandSymbol DetachDatabase =
            new CommandSymbol("detach database", "detach database <database>:DatabaseName",
                new TableSymbol(
                    new ColumnSymbol("Table", ScalarTypes.String),
                    new ColumnSymbol("NumberOfRemovedExtents", ScalarTypes.String)));

        public static readonly CommandSymbol AlterDatabasePrettyName =
            new CommandSymbol("alter database prettyname", "alter database <database>:DatabaseName prettyname <string>:DatabasePrettyName",
                new TableSymbol(
                    new ColumnSymbol("DatabaseName", ScalarTypes.String),
                    new ColumnSymbol("PrettyName", ScalarTypes.String)));

        public static readonly CommandSymbol DropDatabasePrettyName =
            new CommandSymbol("drop database prettyname", "drop database <database>:DatabaseName prettyname",
                new TableSymbol(
                    new ColumnSymbol("DatabaseName", ScalarTypes.String),
                    new ColumnSymbol("PrettyName", ScalarTypes.String)));

        public static readonly CommandSymbol AlterDatabasePersistMetadata =
            new CommandSymbol("alter database persist metadata", "alter database <database>:DatabaseName persist metadata (<string>:BlobContainerUrl ';' <string>:StorageAccountKey | <string>:Path)",
                new TableSymbol(
                    new ColumnSymbol("Moniker", ScalarTypes.Guid),
                    new ColumnSymbol("Url", ScalarTypes.String),
                    new ColumnSymbol("State", ScalarTypes.String),
                    new ColumnSymbol("CreatedOn", ScalarTypes.DateTime),
                    new ColumnSymbol("MaxDateTime", ScalarTypes.DateTime),
                    new ColumnSymbol("IsRecyclable", ScalarTypes.Bool),
                    new ColumnSymbol("StoresDatabaseMetadata", ScalarTypes.Bool),
                    new ColumnSymbol("HardDeletePeriod", ScalarTypes.TimeSpan)));

        public static readonly CommandSymbol SetAccess =
            new CommandSymbol("set access", "set access <database>:DatabaseName to (readonly | readwrite):AccessMode",
                new TableSymbol(
                    new ColumnSymbol("DatabaseName", ScalarTypes.String),
                    new ColumnSymbol("AccessMode", ScalarTypes.String)));

        public static readonly TableSymbol ShowDatabaseSchemaResults =
            new TableSymbol(
                    new ColumnSymbol("DatabaseName", ScalarTypes.String),
                    new ColumnSymbol("TableName", ScalarTypes.String),
                    new ColumnSymbol("ColumnName", ScalarTypes.String),
                    new ColumnSymbol("ColumnType", ScalarTypes.String),
                    new ColumnSymbol("IsDefaultTable", ScalarTypes.Bool),
                    new ColumnSymbol("IsDefaultColumn", ScalarTypes.Bool),
                    new ColumnSymbol("PrettyName", ScalarTypes.String),
                    new ColumnSymbol("Version", ScalarTypes.String),
                    new ColumnSymbol("Folder", ScalarTypes.String),
                    new ColumnSymbol("DocString", ScalarTypes.String));

        public static readonly CommandSymbol ShowDatabaseSchema =
            new CommandSymbol("show database schema", "show database (schema | <database>:DatabaseName schema) [details] [if_later_than <string>:Version]",
                ShowDatabaseSchemaResults);

        public static readonly CommandSymbol ShowDatabaseSchemaAsJson =
            new CommandSymbol("show database schema as json", "show database (schema | <database>:DatabaseName schema) [if_later_than <string>:Version] as json",
                new TableSymbol(
                    new ColumnSymbol("DatabaseSchema", ScalarTypes.String)));

        public static readonly CommandSymbol ShowDatabasesSchema =
            new CommandSymbol("show databases schema", "show databases '(' { <database>:DatabaseName [if_later_than <string>:Version], ',' }+ ')' schema [details]",
                ShowDatabaseSchemaResults);

        public static readonly CommandSymbol ShowDatabasesSchemaAsJson =
            new CommandSymbol("show databases schema as json", "show databases '(' { <database>:DatabaseName [if_later_than <string>:Version], ',' }+ ')' schema as json",
                new TableSymbol(
                    new ColumnSymbol("DatabaseSchema", ScalarTypes.String)));
        #endregion

        #region Tables
        private static readonly TableSymbol ShowTablesResult =
            new TableSymbol(
                new ColumnSymbol("TableName", ScalarTypes.String),
                new ColumnSymbol("DatabaseName", ScalarTypes.String),
                new ColumnSymbol("Folder", ScalarTypes.String),
                new ColumnSymbol("DocString", ScalarTypes.String));

        private static readonly TableSymbol ShowTablesDetailsResult =
            new TableSymbol(
                new ColumnSymbol("TableName", ScalarTypes.String),
                new ColumnSymbol("DatabaseName", ScalarTypes.String),
                new ColumnSymbol("Folder", ScalarTypes.String),
                new ColumnSymbol("DocString", ScalarTypes.String),
                new ColumnSymbol("TotalExtents", ScalarTypes.Long),
                new ColumnSymbol("TotalExtentSize", ScalarTypes.Real),
                new ColumnSymbol("TotalOriginalSize", ScalarTypes.Real),
                new ColumnSymbol("TotalRowCount", ScalarTypes.Long),
                new ColumnSymbol("HotExtents", ScalarTypes.Long),
                new ColumnSymbol("HotExtentSize", ScalarTypes.Real),
                new ColumnSymbol("HotOriginalSize", ScalarTypes.Real),
                new ColumnSymbol("HotRowCount", ScalarTypes.Long),
                new ColumnSymbol("AuthorizedPrincipals", ScalarTypes.String),
                new ColumnSymbol("RetentionPolicy", ScalarTypes.String),
                new ColumnSymbol("CachingPolicy", ScalarTypes.String),
                new ColumnSymbol("ShardingPolicy", ScalarTypes.String),
                new ColumnSymbol("MergePolicy", ScalarTypes.String),
                new ColumnSymbol("StreamingIngestionPolicy", ScalarTypes.String),
                new ColumnSymbol("IngestionBatchingPolicy", ScalarTypes.String),
                new ColumnSymbol("MinExtentsCreationTime", ScalarTypes.DateTime),
                new ColumnSymbol("MaxExtentsCreationTime", ScalarTypes.DateTime),
                new ColumnSymbol("RowOrderPolicy", ScalarTypes.DateTime));

        private static readonly TableSymbol ShowTableSchemaResult =
            new TableSymbol(
                new ColumnSymbol("TableName", ScalarTypes.String),
                new ColumnSymbol("Schema", ScalarTypes.String),
                new ColumnSymbol("DatabaseName", ScalarTypes.String),
                new ColumnSymbol("Folder", ScalarTypes.String),
                new ColumnSymbol("DocString", ScalarTypes.String));

        public static readonly CommandSymbol ShowTables =
            new CommandSymbol("show tables", "show tables ['(' { <table>:TableName, ',' }+ ')']", ShowTablesResult);

        public static readonly CommandSymbol ShowTable =
            new CommandSymbol("show table", "show table <table>:TableName", 
                new TableSymbol(
                    new ColumnSymbol("AttributeName", ScalarTypes.String),
                    new ColumnSymbol("AttributeType", ScalarTypes.String),
                    new ColumnSymbol("ExtentSize", ScalarTypes.Long),
                    new ColumnSymbol("CompressionRatio", ScalarTypes.Real),
                    new ColumnSymbol("IndexSize", ScalarTypes.Long),
                    new ColumnSymbol("IndexSizePercent", ScalarTypes.Long),
                    new ColumnSymbol("OriginalSize", ScalarTypes.Long),
                    new ColumnSymbol("AttributeId", ScalarTypes.Guid),
                    new ColumnSymbol("SharedIndexSize", ScalarTypes.Long),
                    new ColumnSymbol("StorageEngineVersion", ScalarTypes.String)));

        public static readonly CommandSymbol ShowTablesDetails =
            new CommandSymbol("show tables details", "show tables ['(' { <table>:TableName, ',' }+ ')'] details", ShowTablesDetailsResult);

        public static readonly CommandSymbol ShowTableDetails =
            new CommandSymbol("show table details", "show table <table>:TableName details", ShowTablesDetailsResult);

        public static readonly CommandSymbol ShowTableCslSchema =
            new CommandSymbol("show table cslschema", "show table <table>:TableName cslschema", ShowTableSchemaResult);

        public static readonly CommandSymbol ShowTableSchemaAsJson =
            new CommandSymbol("show table schema as json", "show table <table>:TableName schema as json", ShowTableSchemaResult);

        private static readonly string TableSchema = "('(' { <name>:ColumnName ':'! <type>:ColumnType, ',' }+ ')')";
        private static readonly string TableProperties = "with '(' docstring '=' <string>:Documentation [',' folder! '=' <string>:FolderName] ')'";

        public static readonly CommandSymbol CreateTable =
            new CommandSymbol("create table",
                $"create table <name>:TableName {TableSchema} [{TableProperties}]",
                ShowTableSchemaResult);

        public static readonly CommandSymbol CreateMergeTable =
            new CommandSymbol("create-merge table",
                $"create-merge table <name>:TableName {TableSchema}",
                ShowTableSchemaResult);

        public static readonly CommandSymbol CreateTables =
            new CommandSymbol("create tables",
                $"create tables {{ <name>:TableName {TableSchema}, ',' }}+",
                ShowTableSchemaResult);

        public static readonly CommandSymbol AlterTable =
            new CommandSymbol("alter table",
                $"alter table <table> {TableSchema} [{TableProperties}]",
                ShowTableSchemaResult);

        public static readonly CommandSymbol AlterMergeTable =
            new CommandSymbol("alter-merge table",
                $"alter-merge table <table> {TableSchema} [{TableProperties}]",
                ShowTableSchemaResult);

        public static readonly CommandSymbol AlterTableDocString =
            new CommandSymbol("alter table docstring", "alter table <table>:TableName docstring <string>:Documentation", ShowTablesResult);

        public static readonly CommandSymbol AlterTableFolder =
            new CommandSymbol("alter table folder", "alter table <table>:TableName folder <string>:Folder", ShowTablesResult);

        public static readonly CommandSymbol RenameTable =
            new CommandSymbol("rename table", "rename table <table>:TableName to <name>:NewTableName");

        public static readonly CommandSymbol RenameTables =
            new CommandSymbol("rename tables", "rename tables { <name>:NewTableName '='! <table>:TableName, ',' }+");

        public static readonly CommandSymbol DropTable =
            new CommandSymbol("drop table", "drop table <table>:TableName [ifexists]",
                new TableSymbol(
                    new ColumnSymbol("TableName", ScalarTypes.String),
                    new ColumnSymbol("DatabaseName", ScalarTypes.String)));

        public static readonly CommandSymbol UndoDropTable =
            new CommandSymbol("undo drop table", "undo drop table <name> [as <name>:TableName] version '=' <string>:Version",
                new TableSymbol(
                    new ColumnSymbol("ExtentId", ScalarTypes.Guid),
                    new ColumnSymbol("NumberOfRecords", ScalarTypes.Long),
                    new ColumnSymbol("Status", ScalarTypes.String),
                    new ColumnSymbol("FailureReason", ScalarTypes.String)));

        private static readonly string TableNameList = "'(' { <table>:TableName, ',' }+ ')'";

        public static readonly CommandSymbol DropTables =
            new CommandSymbol("drop tables", $"drop tables {TableNameList} [ifexists]",
                new TableSymbol(
                    new ColumnSymbol("TableName", ScalarTypes.String),
                    new ColumnSymbol("DatabaseName", ScalarTypes.String)));

        private readonly static TableSymbol TableIngestionMappingResult =
            new TableSymbol(
                new ColumnSymbol("Name", ScalarTypes.String),
                new ColumnSymbol("Kind", ScalarTypes.String),
                new ColumnSymbol("Mapping", ScalarTypes.String));

        public static readonly CommandSymbol CreateTableIngestionMapping =
            new CommandSymbol("create table ingestion mapping", "create table <name>:TableName ingestion! (csv | json | avro | parquet | orc):MappingKind mapping <string>:MappingName <string>:MappingFormat", TableIngestionMappingResult);

        public static readonly CommandSymbol AlterTableIngestionMapping =
            new CommandSymbol("alter table ingestion mapping", "alter table <table>:TableName ingestion (csv | json | avro | parquet | orc):MappingKind mapping <string>:MappingName <string>:MappingFormat", TableIngestionMappingResult);

        public static readonly CommandSymbol ShowTableIngestionMappings =
            new CommandSymbol("show table ingestion mappings", "show table <table>:TableName ingestion (csv | json | avro | parquet | orc):MappingKind mappings", TableIngestionMappingResult);

        public static readonly CommandSymbol ShowTableIngestionMapping =
            new CommandSymbol("show table ingestion mapping", "show table <table>:TableName ingestion (csv | json | avro | parquet | orc):MappingKind mapping <string>:MappingName", TableIngestionMappingResult);

        public static readonly CommandSymbol DropTableIngestionMapping =
            new CommandSymbol("drop table ingestion mapping", "drop table <table>:TableName ingestion (csv | json | avro | parquet | orc):MappingKind mapping <string>:MappingName");
        #endregion

        #region Columns
        public static readonly CommandSymbol RenameColumn =
            new CommandSymbol("rename column", "rename column <database_table_column>:ColumnName to <name>:NewColumnName");

        public static readonly CommandSymbol RenameColumns =
            new CommandSymbol("rename columns", "rename columns { <name>:NewColumnName '='! <database_table_column>:ColumnName, ',' }+");

        public static readonly CommandSymbol AlterColumnType =
            new CommandSymbol("alter column type", "alter column <database_table_column>:ColumnName type '=' <type>:ColumnType");

        public static readonly CommandSymbol DropColumn =
            new CommandSymbol("drop column", "drop column <table_column>:ColumnName");

        public static readonly CommandSymbol DropTableColumns =
            new CommandSymbol("drop table columns", "drop table <table>:TableName columns '(' { <column>:ColumnName, ',' }+ ')'");

        public static readonly CommandSymbol AlterTableColumnDocStrings =
            new CommandSymbol("alter table column-docstrings", "alter table <table>:TableName column-docstrings '(' { <column>:ColumnName ':'! <string>:DocString, ',' }+ ')'");

        public static readonly CommandSymbol AlterMergeTableColumnDocStrings =
            new CommandSymbol("alter-merge table column-docstrings", "alter-merge table <table>:TableName column-docstrings '(' { <column>:ColumnName ':'! <string>:DocString, ',' }+ ')'");
        #endregion

        #region Functions
        private static readonly TableSymbol FunctionResult =
            new TableSymbol(
                new ColumnSymbol("Name", ScalarTypes.String),
                new ColumnSymbol("Parameters", ScalarTypes.String),
                new ColumnSymbol("Body", ScalarTypes.String),
                new ColumnSymbol("Folder", ScalarTypes.String),
                new ColumnSymbol("DocString", ScalarTypes.String));

        public static readonly CommandSymbol ShowFunctions =
            new CommandSymbol("show functions", FunctionResult);

        public static readonly CommandSymbol ShowFunction =
            new CommandSymbol("show function", "show function <function>:FunctionName", FunctionResult);

        public static readonly CommandSymbol CreateFunction =
            new CommandSymbol("create function", "create function [ifnotexists] [with '('! { <name>:PropertyName '='! <value>:Value, ',' } ')'!] <name>:FunctionName <function_declaration>", FunctionResult);

        public static readonly CommandSymbol AlterFunction =
            new CommandSymbol("alter function", "alter function [with '('! { <name>:PropertyName '='! <value>:Value, ',' }+ ')'!] <function>:FunctionName <function_declaration>", FunctionResult);

        public static readonly CommandSymbol CreateOrAlterFunction =
            new CommandSymbol("create-or-alter function", "create-or-alter function [with '('! { <name>:PropertyName '='! <value>:Value, ',' }+ ')'!] <name>:FunctionName <function_declaration>", FunctionResult);
         
        public static readonly CommandSymbol DropFunction =
            new CommandSymbol("drop function", "drop function <function>:FunctionName",
                new TableSymbol(
                    new ColumnSymbol("Name", ScalarTypes.String)));

        public static readonly CommandSymbol AlterFunctionDocString =
            new CommandSymbol("alter function docstring", "alter function <function> docstring <string>:Documentation", FunctionResult);

        public static readonly CommandSymbol AlterFunctionFolder =
            new CommandSymbol("alter function folder", "alter function <function>:FunctionName folder <string>:Folder", FunctionResult);
        #endregion

        #region External Tables
        private static readonly TableSymbol ExternalTableResult =
            new TableSymbol(
                new ColumnSymbol("TableName", ScalarTypes.String),
                new ColumnSymbol("TableType", ScalarTypes.String),
                new ColumnSymbol("Folder", ScalarTypes.String),
                new ColumnSymbol("DocString", ScalarTypes.String),
                new ColumnSymbol("Properties", ScalarTypes.String),
                new ColumnSymbol("ConnectionStrings", ScalarTypes.Dynamic));

        private static readonly TableSymbol ExternalTableSchemaResult =
            new TableSymbol(
                new ColumnSymbol("TableName", ScalarTypes.String),
                new ColumnSymbol("Schema", ScalarTypes.String),
                new ColumnSymbol("DatabaseName", ScalarTypes.String),
                new ColumnSymbol("Folder", ScalarTypes.String),
                new ColumnSymbol("DocString", ScalarTypes.String));

        private static readonly TableSymbol ExternalTableArtifactsResult =
            new TableSymbol(
                new ColumnSymbol("Uri", ScalarTypes.String));

        private static readonly TableSymbol ExternalTableFullResult =
            new TableSymbol(
                new ColumnSymbol("TableName", ScalarTypes.String),
                new ColumnSymbol("TableType", ScalarTypes.String),
                new ColumnSymbol("Folder", ScalarTypes.String),
                new ColumnSymbol("DocString", ScalarTypes.String),
                new ColumnSymbol("Schema", ScalarTypes.String),
                new ColumnSymbol("Properties", ScalarTypes.String));

        public static readonly CommandSymbol ShowExternalTables =
            new CommandSymbol("show external tables", ExternalTableResult);

        public static readonly CommandSymbol ShowExternalTable =
            new CommandSymbol("show external table", "show external table <name>:TableName", ExternalTableResult);

        public static readonly CommandSymbol ShowExternalTableCslSchema =
            new CommandSymbol("show external table cslschema", "show external table <name>:TableName cslschema", ExternalTableSchemaResult);

        public static readonly CommandSymbol ShowExternalTableSchema =
            new CommandSymbol("show external table schema", "show external table <name>:TableName schema as (json | csl)", ExternalTableSchemaResult);

        public static readonly CommandSymbol ShowExternalTableArtifacts =
            new CommandSymbol("show external table artifacts", "show external table <name>:TableName artifacts", ExternalTableArtifactsResult);

        public static readonly CommandSymbol DropExternalTable =
            new CommandSymbol("drop external table", "drop external table <name>:TableName", ExternalTableFullResult);

        private static readonly string CreateOrAlterExternalTableGrammar =
            @"external table <name>:TableName '(' { <name>:ColumnName ':'! <type>:ColumnType, ',' }+ ')'
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
            new CommandSymbol("create external table", "create " + CreateOrAlterExternalTableGrammar,
                ExternalTableFullResult);

        public static readonly CommandSymbol AlterExternalTable =
            new CommandSymbol("alter external table", "alter " + CreateOrAlterExternalTableGrammar,
                ExternalTableFullResult);
        #endregion
        #endregion

        #region Policy Commands
        private static readonly TableSymbol PolicyResult =
            new TableSymbol(
                new ColumnSymbol("PolicyName", ScalarTypes.String),
                new ColumnSymbol("EntityName", ScalarTypes.String),
                new ColumnSymbol("Policy", ScalarTypes.String),
                new ColumnSymbol("ChildEntities", ScalarTypes.Dynamic),
                new ColumnSymbol("EntityType", ScalarTypes.String));

        #region Caching
        public static readonly CommandSymbol ShowDatabasePolicyCaching =
            new CommandSymbol("show database policy caching", "show database <database> policy caching", PolicyResult);

        public static readonly CommandSymbol ShowTablePolicyCaching =
            new CommandSymbol("show table policy caching", "show table <database_table> policy caching", PolicyResult);

        public static readonly CommandSymbol AlterDatabasePolicyCaching =
            new CommandSymbol("alter database policy caching", "alter database <database> policy caching hot '=' <timespan>:Timespan", PolicyResult);

        public static readonly CommandSymbol AlterTablePolicyCaching =
            new CommandSymbol("alter table policy caching", "alter table <database_table> policy caching hot '=' <timespan>:Timespan", PolicyResult);

        public static readonly CommandSymbol AlterClusterPolicyCaching =
            new CommandSymbol("alter cluster policy caching", "alter cluster policy caching hot '=' <timespan>:Timespan");

        public static readonly CommandSymbol DeleteTablePolicyCaching =
            new CommandSymbol("delete table policy caching", "delete table <database_table> policy caching");
        #endregion

        #region IngestionTime
        public static readonly CommandSymbol ShowTablePolicyIngestionTime =
            new CommandSymbol("show table policy ingestiontime", "show table (<table> | '*'):TableName policy ingestiontime", PolicyResult);

        public static readonly CommandSymbol AlterTablePolicyIngestionTime =
            new CommandSymbol("alter table policy ingestiontime", "alter table <table>:TableName policy ingestiontime true", PolicyResult);

        public static readonly CommandSymbol AlterTablesPolicyIngestionTime =
            new CommandSymbol("alter tables policy ingestiontime", "alter tables '(' { <table>:TableName, ',' }+ ')' policy ingestiontime true", PolicyResult);

        public static readonly CommandSymbol DeleteTablePolicyIngestionTime =
            new CommandSymbol("delete table policy ingestiontime", "delete table <table>:TableName policy ingestiontime");
        #endregion

        #region Retention
        public static readonly CommandSymbol ShowTablePolicyRetention =
            new CommandSymbol("show table policy retention", "show table (<database_table> | '*'):TableName policy retention", PolicyResult);

        public static readonly CommandSymbol ShowDatabasePolicyRetention =
            new CommandSymbol("show database policy retention", "show database (<database> | '*'):DatabaseName policy retention", PolicyResult);

        public static readonly CommandSymbol AlterTablePolicyRetention =
            new CommandSymbol("alter table policy retention", "alter table <database_table>:TableName policy retention <string>:RetentionPolicy", PolicyResult);

        public static readonly CommandSymbol AlterDatabasePolicyRetention =
            new CommandSymbol("alter database policy retention", "alter database <database>:DatabaseName policy retention <string>:RetentionPolicy", PolicyResult);

        public static readonly CommandSymbol AlterTablesPolicyRetention =
            new CommandSymbol("alter tables policy retention", "alter tables '(' { <table>:TableName, ',' }+ ')' policy retention <string>:RetentionPolicy", PolicyResult);

        public static readonly CommandSymbol AlterMergeTablePolicyRetention =
            new CommandSymbol("alter-merge table policy retention", "alter-merge table <database_table>:TableName policy retention (<string>:RetentionPolicy | softdelete '='! <timespan>:SoftDeleteValue [recoverability '='! (disabled|enabled):RecoverabilityValue] | recoverability '='! (disabled|enabled):RecoverabilityValue)", PolicyResult);

        public static readonly CommandSymbol AlterMergeDatabasePolicyRetention =
            new CommandSymbol("alter-merge database policy retention", "alter-merge database <database>:DatabaseName policy retention (<string>:RetentionPolicy | softdelete '='! <timespan>:SoftDeleteValue [recoverability '='! (disabled|enabled):RecoverabilityValue] | recoverability '='! (disabled|enabled):RecoverabilityValue)", PolicyResult);

        public static readonly CommandSymbol DeleteTablePolicyRetention =
            new CommandSymbol("delete table policy retention", "delete table <database_table>:TableName policy retention");

        public static readonly CommandSymbol DeleteDatabasePolicyRetention =
            new CommandSymbol("delete database policy retention", "delete database <database>:DatabaseName policy retention");
        #endregion

        #region RowLevelSecurity
        public static readonly CommandSymbol ShowTablePolicyRowLevelSecurity =
            new CommandSymbol("show table policy row_level_security", "show table (<table> | '*'):TableName policy row_level_security", PolicyResult);

        public static readonly CommandSymbol AlterTablePolicyRowLevelSecurity =
            new CommandSymbol("alter table policy row_level_security", "alter table <table>:TableName policy row_level_security (enable | disable) <string>:Query", PolicyResult);

        public static readonly CommandSymbol DeleteTablePolicyRowLevelSecurity =
            new CommandSymbol("delete table policy row_level_security", "delete table <table>:TableName policy row_level_security");
        #endregion

        #region RowOrder
        public static readonly CommandSymbol ShowTablePolicyRowOrder =
            new CommandSymbol("show table policy roworder", "show table (<database_table> | '*'):TableName policy roworder", PolicyResult);

        public static readonly CommandSymbol AlterTablePolicyRowOrder =
            new CommandSymbol("alter table policy roworder", "alter table <database_table>:TableName policy roworder '('! { <column>:ColumnName (asc|desc)!, ',' }+ ')'!", PolicyResult);

        public static readonly CommandSymbol AlterTablesPolicyRowOrder =
            new CommandSymbol("alter tables policy roworder", "alter tables '(' { <table>:TableName, ',' }+ ')' policy roworder '(' { <name>:ColumnName (asc|desc)!, ',' }+ ')'", PolicyResult);

        public static readonly CommandSymbol AlterMergeTablePolicyRowOrder =
            new CommandSymbol("alter-merge table policy roworder", "alter-merge table <database_table>:TableName policy roworder '(' { <column>:ColumnName (asc|desc)!, ',' }+ ')'", PolicyResult);

        public static readonly CommandSymbol DeleteTablePolicyRowOrder =
            new CommandSymbol("delete table policy roworder", "delete table <database_table>:TableName policy roworder");
        #endregion

        #region Update
        public static readonly CommandSymbol ShowTablePolicyUpdate =
            new CommandSymbol("show table policy update", "show table (<database_table> | '*'):TableName policy update", PolicyResult);

        public static readonly CommandSymbol AlterTablePolicyUpdate =
            new CommandSymbol("alter table policy update", "alter table <database_table>:TableName policy update <string>:UpdatePolicy", PolicyResult);

        public static readonly CommandSymbol AlterMergeTablePolicyUpdate =
            new CommandSymbol("alter-merge table policy update", "alter-merge table <database_table>:TableName policy update <string>:UpdatePolicy", PolicyResult);

        public static readonly CommandSymbol DeleteTablePolicyUpdate =
            new CommandSymbol("delete table policy update", "delete table <database_table>:TableName policy update");
        #endregion

        #region Batch
        public static readonly CommandSymbol ShowDatabasePolicyIngestionBatching =
            new CommandSymbol("show database policy ingestionbatching", "show database <database>:DatabaseName policy ingestionbatching", PolicyResult);

        public static readonly CommandSymbol ShowTablePolicyIngestionBatching =
            new CommandSymbol("show table policy ingestionbatching", "show table <database_table>:TableName policy ingestionbatching", PolicyResult);

        public static readonly CommandSymbol AlterDatabasePolicyIngestionBatching =
            new CommandSymbol("alter database policy ingestionbatching", "alter database <database>:DatabaseName policy ingestionbatching <string>:IngestionBatchingPolicy", PolicyResult);

        public static readonly CommandSymbol AlterTablePolicyIngestionBatching =
            new CommandSymbol("alter table policy ingestionbatching", "alter table <database_table>:TableName policy ingestionbatching <string>:IngestionBatchingPolicy", PolicyResult);

        public static readonly CommandSymbol AlterTablesPolicyIngestionBatching =
            new CommandSymbol("alter tables policy ingestionbatching", "alter tables '(' { <table>:TableName, ',' }+ ')' policy ingestionbatching <string>:IngestionBatchingPolicy", PolicyResult);

        public static readonly CommandSymbol DeleteDatabasePolicyIngestionBatching =
            new CommandSymbol("delete database policy ingestionbatching", "delete database <database>:DatabaseName policy ingestionbatching");

        public static readonly CommandSymbol DeleteTablePolicyIngestionBatching =
            new CommandSymbol("delete table policy ingestionbatching", "delete table <database_table>:TableName policy ingestionbatching");
        #endregion

        #region Encoding
        public static readonly CommandSymbol ShowDatabasePolicyEncoding =
            new CommandSymbol("show database policy encoding", "show database <database>:DatabaseName policy encoding", PolicyResult);

        public static readonly CommandSymbol ShowTablePolicyEncoding =
            new CommandSymbol("show table policy encoding", "show table <database_table>:TableName policy encoding", PolicyResult);

        public static readonly CommandSymbol ShowColumnPolicyEncoding =
            new CommandSymbol("show column policy encoding", "show column <table_column>:ColumnName policy encoding", PolicyResult);

        public static readonly CommandSymbol AlterDatabasePolicyEncoding =
            new CommandSymbol("alter database policy encoding", "alter database <database>:DatabaseName policy encoding <string>:EncodingPolicy", PolicyResult);

        public static readonly CommandSymbol AlterTablePolicyEncoding =
            new CommandSymbol("alter table policy encoding", "alter table <database_table>:TableName policy encoding <string>:EncodingPolicy", PolicyResult);

        public static readonly CommandSymbol AlterColumnPolicyEncoding =
            new CommandSymbol("alter column policy encoding", "alter column <table_column>:ColumnName policy encoding <string>:EncodingPolicy", PolicyResult);

        public static readonly CommandSymbol AlterColumnPolicyEncodingType =
            new CommandSymbol("alter column policy encoding type", "alter column <table_column>:ColumnName policy encoding type '=' <string>:EncodingPolicyType", PolicyResult);

        public static readonly CommandSymbol AlterMergeDatabasePolicyEncoding =
            new CommandSymbol("alter-merge database policy encoding", "alter-merge database <database>:DatabaseName policy encoding <string>:EncodingPolicy", PolicyResult);

        public static readonly CommandSymbol AlterMergeTablePolicyEncoding =
            new CommandSymbol("alter-merge table policy encoding", "alter-merge table <database_table>:TableName policy encoding <string>:EncodingPolicy", PolicyResult);

        public static readonly CommandSymbol AlterMergeColumnPolicyEncoding =
            new CommandSymbol("alter-merge column policy encoding", "alter-merge column <table_column>:ColumnName policy encoding <string>:EncodingPolicy", PolicyResult);

        public static readonly CommandSymbol DeleteDatabasePolicyEncoding =
            new CommandSymbol("delete database policy encoding", "delete database <database>:DatabaseName policy encoding");

        public static readonly CommandSymbol DeleteTablePolicyEncoding =
            new CommandSymbol("delete table policy encoding", "delete table <database_table>:TableName policy encoding");

        public static readonly CommandSymbol DeleteColumnPolicyEncoding =
            new CommandSymbol("delete column policy encoding", "delete column <table_column>:ColumnName policy encoding");
        #endregion

        #region Merge
        public static readonly CommandSymbol ShowDatabasePolicyMerge =
            new CommandSymbol("show database policy merge", "show database (<database> | '*'):DatabaseName policy merge", PolicyResult);

        public static readonly CommandSymbol ShowTablePolicyMerge =
            new CommandSymbol("show table policy merge", "show table (<database_table> | '*'):TableName policy merge", PolicyResult);

        public static readonly CommandSymbol AlterDatabasePolicyMerge =
            new CommandSymbol("alter database policy merge", "alter database <database>:DatabaseName policy merge <string>:MergePolicy", PolicyResult);

        public static readonly CommandSymbol AlterTablePolicyMerge =
            new CommandSymbol("alter table policy merge", "alter table <database_table>:TableName policy merge <string>:MergePolicy", PolicyResult);

        public static readonly CommandSymbol AlterMergeDatabasePolicyMerge =
            new CommandSymbol("alter-merge database policy merge", "alter-merge database <database>:DatabaseName policy merge <string>:MergePolicy", PolicyResult);

        public static readonly CommandSymbol AlterMergeTablePolicyMerge =
            new CommandSymbol("alter-merge table policy merge", "alter-merge table <database_table>:TableName policy merge <string>:MergePolicy", PolicyResult);

        public static readonly CommandSymbol  DeleteDatabasePolicyMerge =
            new CommandSymbol("delete database policy merge", "delete database <database>:DatabaseName policy merge");

        public static readonly CommandSymbol DeleteTablePolicyMerge =
            new CommandSymbol("delete table policy merge", "delete table <database_table>:TableName policy merge");
        #endregion

        #region Partitioning
        public static readonly CommandSymbol ShowTablePolicyPartitioning =
            new CommandSymbol("show table policy partitioning", "show table <table>:TableName policy partitioning", PolicyResult);

        public static readonly CommandSymbol AlterTablePolicyPartitioning =
            new CommandSymbol("alter table policy partitioning", "alter table <table>:TableName policy partitioning <string>:Policy", PolicyResult);

        public static readonly CommandSymbol AlterMergeTablePolicyPartitioning =
            new CommandSymbol("alter-merge table policy partitioning", "alter-merge table <table>:TableName policy partitioning <string>:Policy", PolicyResult);

        public static readonly CommandSymbol DeleteTablePolicyPartitioning =
            new CommandSymbol("delete table policy partitioning", "delete table <table>:TableName policy partitioning", PolicyResult);
        #endregion

        #region RestrictedViewAccess
        public static readonly CommandSymbol ShowTablePolicyRestrictedViewAccess =
            new CommandSymbol("show table policy restricted_view_access", "show table (<database_table> | '*'):TableName policy restricted_view_access", PolicyResult);

        public static readonly CommandSymbol AlterTablePolicyRestrictedViewAccess =
            new CommandSymbol("alter table policy restricted_view_access", "alter table <database_table>:TableName policy restricted_view_access (true | false)", PolicyResult);

        public static readonly CommandSymbol AlterTablesPolicyRestrictedViewAccess =
            new CommandSymbol("alter tables policy restricted_view_access", "alter tables '(' { <table>:TableName, ',' }+ ')' policy restricted_view_access (true | false)", PolicyResult);

        public static readonly CommandSymbol DeleteTablePolicyRestrictedViewAccess =
            new CommandSymbol("delete table policy restricted_view_access", "delete table <database_table>:TableName policy restricted_view_access");
        #endregion

        #region RowStore
        public static readonly CommandSymbol ShowClusterPolicyRowStore =
            new CommandSymbol("show cluster policy rowstore", "show cluster policy rowstore", PolicyResult);

        public static readonly CommandSymbol AlterClusterPolicyRowStore =
            new CommandSymbol("alter cluster policy rowstore", "alter cluster policy rowstore <string>:RowStorePolicy", PolicyResult);

        public static readonly CommandSymbol AlterMergeClusterPolicyRowStore =
            new CommandSymbol("alter-merge cluster policy rowstore", "alter-merge cluster policy! rowstore <string>:RowStorePolicy", PolicyResult);
        #endregion

        #region Sandbox
        public static readonly CommandSymbol ShowClusterPolicySandbox =
            new CommandSymbol("show cluster policy sandbox", "show cluster policy sandbox", PolicyResult);

        public static readonly CommandSymbol AlterClusterPolicySandbox =
            new CommandSymbol("alter cluster policy sandbox", "alter cluster policy sandbox <string>:SandboxPolicy", PolicyResult);
        #endregion

        #region Sharding
        public static readonly CommandSymbol ShowDatabasePolicySharding =
            new CommandSymbol("show database policy sharding", "show database <database>:DatabaseName policy sharding", PolicyResult);

        public static readonly CommandSymbol ShowTablePolicySharding =
            new CommandSymbol("show table policy sharding", "show table (<database_table> | '*'):TableName policy sharding", PolicyResult);

        public static readonly CommandSymbol AlterDatabasePolicySharding =
            new CommandSymbol("alter database policy sharding", "alter database <database>:DatabaseName policy sharding <string>:ShardingPolicy", PolicyResult);

        public static readonly CommandSymbol AlterTablePolicySharding =
            new CommandSymbol("alter table policy sharding", "alter table <database_table>:TableName policy sharding <string>:ShardingPolicy", PolicyResult);

        public static readonly CommandSymbol AlterMergeDatabasePolicySharding =
            new CommandSymbol("alter-merge database policy sharding", "alter-merge database <database>:DatabaseName policy sharding <string>:ShardingPolicy", PolicyResult);

        public static readonly CommandSymbol AlterMergeTablePolicySharding =
            new CommandSymbol("alter-merge table policy sharding", "alter-merge table <database_table>:TableName policy sharding <string>:ShardingPolicy", PolicyResult);

        public static readonly CommandSymbol DeleteDatabasePolicySharding =
            new CommandSymbol("delete database policy sharding", "delete database <database>:DatabaseName policy sharding");

        public static readonly CommandSymbol DeleteTablePolicySharding =
            new CommandSymbol("delete table policy sharding", "delete table <database_table>:TableName policy sharding");
        #endregion

        #region StreamingIngestion
        public static readonly CommandSymbol ShowDatabasePolicyStreamingIngestion =
            new CommandSymbol("show database policy streamingingestion", "show database <database>:DatabaseName policy streamingingestion", PolicyResult);

        public static readonly CommandSymbol ShowTablePolicyStreamingIngestion =
            new CommandSymbol("show table policy streamingingestion", "show table <database_table>:TableName policy streamingingestion", PolicyResult);

        public static readonly CommandSymbol ShowClusterPolicyStreamingIngestion =
            new CommandSymbol("show cluster policy streamingingestion", "show cluster policy streamingingestion", PolicyResult);

        public static readonly CommandSymbol AlterDatabasePolicyStreamingIngestion =
            new CommandSymbol("alter database policy streamingingestion", "alter database <database>:DatabaseName policy streamingingestion <string>:StreamingIngestionPolicy", PolicyResult);

        public static readonly CommandSymbol AlterTablePolicyStreamingIngestion =
            new CommandSymbol("alter table policy streamingingestion", "alter table <database_table>:TableName policy streamingingestion <string>:StreamingIngestionPolicy", PolicyResult);

        public static readonly CommandSymbol AlterClusterPolicyStreamingIngestion =
            new CommandSymbol("alter cluster policy streamingingestion", "alter cluster policy streamingingestion <string>:StreamingIngestionPolicy", PolicyResult);

        public static readonly CommandSymbol DeleteDatabasePolicyStreamingIngestion =
            new CommandSymbol("delete database policy streamingingestion", "delete database <database>:DatabaseName policy streamingingestion");

        public static readonly CommandSymbol DeleteTablePolicyStreamingIngestion =
            new CommandSymbol("delete table policy streamingingestion", "delete table <database_table>:TableName policy streamingingestion");

        public static readonly CommandSymbol DeleteClusterPolicyStreamingIngestion =
            new CommandSymbol("delete cluster policy streamingingestion", "delete cluster policy streamingingestion");

        #endregion

        #region Callout
        public static readonly CommandSymbol ShowClusterPolicyCallout =
            new CommandSymbol("show cluster policy callout", PolicyResult);

        public static readonly CommandSymbol AlterClusterPolicyCallout =
            new CommandSymbol("alter cluster policy callout", "alter cluster policy callout <string>:Policy", PolicyResult);

        public static readonly CommandSymbol AlterMergeClusterPolicyCallout =
            new CommandSymbol("alter-merge cluster policy callout", "alter-merge cluster policy callout <string>:Policy", PolicyResult);

        public static readonly CommandSymbol DeleteClusterPolicyCallout =
                    new CommandSymbol("delete cluster policy callout", PolicyResult);
        #endregion

        #region Capacity
        public static readonly CommandSymbol ShowClusterPolicyCapacity =
            new CommandSymbol("show cluster policy capacity", PolicyResult);

        public static readonly CommandSymbol AlterClusterPolicyCapacity =
            new CommandSymbol("alter cluster policy capacity", "alter cluster policy capacity <string>:Policy", PolicyResult);

        public static readonly CommandSymbol AlterMergeClusterPolicyCapacity =
            new CommandSymbol("alter-merge cluster policy capacity", "alter-merge cluster policy capacity <string>:Policy", PolicyResult);
        #endregion

        #region Query Throttling
        public static readonly CommandSymbol ShowClusterPolicyQueryThrottling =
            new CommandSymbol("show cluster policy querythrottling", PolicyResult);

        public static readonly CommandSymbol AlterClusterPolicyQueryThrottling =
            new CommandSymbol("alter cluster policy querythrottling", "alter cluster policy querythrottling <string>:Policy", PolicyResult);

        public static readonly CommandSymbol DeleteClusterPolicyQueryThrottling =
            new CommandSymbol("delete cluster policy querythrottling", PolicyResult);
        #endregion

        #region Query Limit
        public static readonly CommandSymbol ShowClusterPolicyQueryLimit =
            new CommandSymbol("show cluster policy querylimit", PolicyResult);

        public static readonly CommandSymbol AlterClusterPolicyQueryLimit =
            new CommandSymbol("alter cluster policy querylimit", "alter cluster policy querylimit <string>:Policy", PolicyResult);
        #endregion

        #region Multi Database Admins
        public static readonly CommandSymbol ShowClusterPolicyMultiDatabaseAdmins =
            new CommandSymbol("show cluster policy multidatabaseadmins", PolicyResult);

        public static readonly CommandSymbol AlterClusterPolicyMultiDatabaseAdmins =
            new CommandSymbol("alter cluster policy multidatabaseadmins", "alter cluster policy multidatabaseadmins <string>:Policy", PolicyResult);

        public static readonly CommandSymbol AlterMergeClusterPolicyMultiDatabaseAdmins =
            new CommandSymbol("alter-merge cluster policy multidatabaseadmins", "alter-merge cluster policy multidatabaseadmins <string>:Policy", PolicyResult);
        #endregion
        #endregion

        #region Security Role Commands
        public static readonly CommandSymbol ShowPrincipalRoles =
            new CommandSymbol("show principal roles", "show principal [<string>:Principal] roles",
                new TableSymbol(
                    new ColumnSymbol("Scope", ScalarTypes.String),
                    new ColumnSymbol("DisplayName", ScalarTypes.String),
                    new ColumnSymbol("AADObjectID", ScalarTypes.String),
                    new ColumnSymbol("Role", ScalarTypes.String)));

        private static readonly TableSymbol ShowPrincipalsResult =
            new TableSymbol(
                new ColumnSymbol("Role", ScalarTypes.String),
                new ColumnSymbol("PrincipalType", ScalarTypes.String),
                new ColumnSymbol("PrincipalDisplayName", ScalarTypes.String),
                new ColumnSymbol("PrincipalObjectId", ScalarTypes.String),
                new ColumnSymbol("PrincipalFQN", ScalarTypes.String),
                new ColumnSymbol("Notes", ScalarTypes.String));

        public static readonly CommandSymbol ShowClusterPrincipals =
            new CommandSymbol("show cluster principals", ShowPrincipalsResult);

        public static readonly CommandSymbol ShowDatabasePrincipals =
            new CommandSymbol("show database principals", $"show database <database>:DatabaseName principals", ShowPrincipalsResult);

        public static readonly CommandSymbol ShowTablePrincipals =
            new CommandSymbol("show table principals", $"show table <table>:TableName principals", ShowPrincipalsResult);

        public static readonly CommandSymbol ShowFunctionPrincipals =
            new CommandSymbol("show function principals", $"show function <function>:FunctionName principals", ShowPrincipalsResult);

        private static string ClusterRole = "(admins | databasecreators | users | viewers):Role";
        private static string DatabaseRole = "(admins | ingestors | monitors | unrestrictedviewers | users | viewers):Role";
        private static string TableRole = "(admins | ingestors):Role";
        private static string FunctionRole = "admins:Role";
        private static string PrincipalsClause = "'(' { <string>:Principal, ',' }+ ')' [skip-results:SkipResults] [<string>:Notes]";
        private static string PrincipalsOrNoneClause = $"(none [skip-results:SkipResults] | {PrincipalsClause})";

        public static readonly CommandSymbol AddClusterRole =
            new CommandSymbol("add cluster role", $"add cluster {ClusterRole} {PrincipalsClause}", ShowPrincipalsResult);

        public static readonly CommandSymbol DropClusterRole =
            new CommandSymbol("drop cluster role", $"drop cluster {ClusterRole} {PrincipalsClause}");

        public static readonly CommandSymbol SetClusterRole =
            new CommandSymbol("set cluster role", $"set cluster {ClusterRole} {PrincipalsOrNoneClause}", ShowPrincipalsResult);


        public static readonly CommandSymbol AddDatabaseRole =
            new CommandSymbol("add database role", $"add database <database>:DatabaseName {DatabaseRole} {PrincipalsClause}", ShowPrincipalsResult);

        public static readonly CommandSymbol DropDatabaseRole =
            new CommandSymbol("drop database role", $"drop database <database>:DatabaseName {DatabaseRole} {PrincipalsClause}");

        public static readonly CommandSymbol SetDatabaseRole =
            new CommandSymbol("set database role", $"set database <database>:DatabaseName {DatabaseRole} {PrincipalsOrNoneClause}", ShowPrincipalsResult);


        public static readonly CommandSymbol AddTableRole =
            new CommandSymbol("add table role", $"add table <table>:TableName {TableRole} {PrincipalsClause}", ShowPrincipalsResult);

        public static readonly CommandSymbol DropTableRole =
            new CommandSymbol("drop table role", $"drop table <table>:TableName {TableRole} {PrincipalsClause}");

        public static readonly CommandSymbol SetTableRole =
            new CommandSymbol("set table role", $"set table <table>:TableName {TableRole} {PrincipalsOrNoneClause}", ShowPrincipalsResult);


        public static readonly CommandSymbol AddFunctionRole =
            new CommandSymbol("add function role", $"add function <function>:FunctionName {FunctionRole} {PrincipalsClause}", ShowPrincipalsResult);

        public static readonly CommandSymbol DropFunctionRole =
            new CommandSymbol("drop function role", $"drop function <function>:FunctionName {FunctionRole} {PrincipalsClause}");

        public static readonly CommandSymbol SetFunctionRole =
            new CommandSymbol("set function role", $"set function <function>:FunctionName {FunctionRole} {PrincipalsOrNoneClause}", ShowPrincipalsResult);


        private static readonly TableSymbol BlockedPrincipalsResult =
            new TableSymbol(
                new ColumnSymbol("PrincipalType", ScalarTypes.String),
                new ColumnSymbol("PrincipalDisplayName", ScalarTypes.String),
                new ColumnSymbol("PrincipalObjectId", ScalarTypes.String),
                new ColumnSymbol("PrincipalFQN", ScalarTypes.String),
                new ColumnSymbol("Application", ScalarTypes.String),
                new ColumnSymbol("User", ScalarTypes.String),
                new ColumnSymbol("BlockedUntil", ScalarTypes.DateTime),
                new ColumnSymbol("Reason", ScalarTypes.String));

        public static readonly CommandSymbol ShowClusterBlockedPrincipals =
            new CommandSymbol("show cluster blockedprincipals", BlockedPrincipalsResult);

        public static readonly CommandSymbol AddClusterBlockedPrincipals =
            new CommandSymbol("add cluster blockedprincipals", "add cluster blockedprincipals <string>:Principal [application <string>:AppName] [user <string>:UserName] [period <timespan>:Period] [reason <string>:Reason]", BlockedPrincipalsResult);

        public static readonly CommandSymbol DropClusterBlockedPrincipals =
            new CommandSymbol("drop cluster blockedprincipals", "drop cluster blockedprincipals <string>:Principal [application <string>:AppName] [user <string>:UserName]");
        #endregion

        #region Data Ingestion
        private static readonly string SourceDataLocatorList = "'(' { <string>:SourceDataLocator, ',' }+ ')'";
        private static readonly string PropertyList = "with '('! { <name>:PropertyName '='! <value>:PropertyValue, ',' }+ ')'";

        public static readonly CommandSymbol IngestIntoTable =
            new CommandSymbol("ingest into table", $"ingest [async] into table! <table>:TableName {SourceDataLocatorList} [{PropertyList}]",
                new TableSymbol(
                    new ColumnSymbol("ExtentId", ScalarTypes.Guid),
                    new ColumnSymbol("ItemLoaded", ScalarTypes.String),
                    new ColumnSymbol("Duration", ScalarTypes.TimeSpan),
                    new ColumnSymbol("HasErrors", ScalarTypes.Bool),
                    new ColumnSymbol("OperationId", ScalarTypes.Guid)));

        public static readonly CommandSymbol IngestInlineIntoTable =
            new CommandSymbol("ingest inline into table", $"ingest inline into! table <name>:TableName ('[' <bracketted_input_data>:Data ']' | {PropertyList} '<|'! <input_data>:Data | '<|' <input_data>:Data)",
                new TableSymbol(
                    new ColumnSymbol("ExtentId", ScalarTypes.Guid)));

        private static readonly TableSymbol DataIngestionSetAppendResult =
            new TableSymbol(
                new ColumnSymbol("ExtentId", ScalarTypes.Guid),
                new ColumnSymbol("OriginalSize", ScalarTypes.Long),
                new ColumnSymbol("ExtentSize", ScalarTypes.Long),
                new ColumnSymbol("ColumnSize", ScalarTypes.Long),
                new ColumnSymbol("IndexSize", ScalarTypes.Long),
                new ColumnSymbol("RowCount", ScalarTypes.Long));

        public static readonly CommandSymbol SetTable =
            new CommandSymbol("set table", $"set [async] <name>:TableName [{PropertyList}] '<|' <input_query>:QueryOrCommand", DataIngestionSetAppendResult);

        public static readonly CommandSymbol AppendTable =
            new CommandSymbol("append table", $"append [async] <table>:TableName [{PropertyList}] '<|' <input_query>:QueryOrCommand", DataIngestionSetAppendResult);

        public static readonly CommandSymbol SetOrAppendTable =
            new CommandSymbol("set-or-append table", $"set-or-append [async] <name>:TableName [{PropertyList}] '<|' <input_query>:QueryOrCommand", DataIngestionSetAppendResult);

        public static readonly CommandSymbol SetOrReplaceTable =
            new CommandSymbol("set-or-replace table", $"set-or-replace [async] <name>:TableName [{PropertyList}] '<|' <input_query>:QueryOrCommand", DataIngestionSetAppendResult);
        #endregion

        #region Data Export
        private static string DataConnectionStringList = "'(' { <string>:DataConnectionString, ',' }+ ')'";

        public static readonly CommandSymbol ExportToStorage =
            new CommandSymbol("export to storage", $"export [async] [compressed] to (csv|tsv|json|parquet) {DataConnectionStringList} [{PropertyList}] '<|' <input_query>:Query");

        public static readonly CommandSymbol ExportToSqlTable =
            new CommandSymbol("export to sql table", $"export [async] to sql <name>:SqlTableName <string>:SqlConnectionString [{PropertyList}] '<|' <input_query>:Query");

        public static readonly CommandSymbol ExportToExternalTable =
            new CommandSymbol("export to external table", $"export [async] to table <name>:ExternalTableName [{PropertyList}] '<|' <input_query>:Query");

        private static readonly string OverClause = "over '('! { <name>:TableName, ',' }+ ')'";

        public static readonly CommandSymbol CreateOrAlterContinuousExport =
            new CommandSymbol("create-or-alter continuous-export", $"create-or-alter continuous-export <name>:ContinuousExportName [{OverClause}] to table <name>:ExternalTableName [{PropertyList}] '<|' <input_query>:Query");

        private static readonly TableSymbol ShowContinuousExportResult =
            new TableSymbol(
                new ColumnSymbol("Name", ScalarTypes.String),
                new ColumnSymbol("ExternalTableName", ScalarTypes.String),
                new ColumnSymbol("Query", ScalarTypes.String),
                new ColumnSymbol("ForcedLatency", ScalarTypes.TimeSpan),
                new ColumnSymbol("IntervalBetweenRuns", ScalarTypes.TimeSpan),
                new ColumnSymbol("CursorScopedTables", ScalarTypes.String),
                new ColumnSymbol("ExportProperties", ScalarTypes.String),
                new ColumnSymbol("LastRunTime", ScalarTypes.DateTime),
                new ColumnSymbol("StartCursor", ScalarTypes.String),
                new ColumnSymbol("IsDisabled", ScalarTypes.Bool),
                new ColumnSymbol("LastRunResult", ScalarTypes.String),
                new ColumnSymbol("ExportedTo", ScalarTypes.DateTime),
                new ColumnSymbol("IsRunning", ScalarTypes.Bool));

        public static readonly CommandSymbol ShowContinuousExport =
            new CommandSymbol("show continuous-export", "show continuous-export <name>:ContinuousExportName", ShowContinuousExportResult);

        public static readonly CommandSymbol ShowContinuousExports =
            new CommandSymbol("show continuous-exports", "show continuous-exports", ShowContinuousExportResult);

        public static readonly CommandSymbol ShowContinuousExportExportedArtifacts =
            new CommandSymbol("show continuous-export exported-artifacts", "show continuous-export <name>:ContinuousExportName exported-artifacts",
                new TableSymbol(
                    new ColumnSymbol("Timestamp", ScalarTypes.DateTime),
                    new ColumnSymbol("ExternalTableName", ScalarTypes.String),
                    new ColumnSymbol("Path", ScalarTypes.String),
                    new ColumnSymbol("NumRecords", ScalarTypes.Long)));

        public static readonly CommandSymbol ShowContinuousExportFailures =
            new CommandSymbol("show continuous-export failures", "show continuous-export <name>:ContinuousExportName failures",
                new TableSymbol(
                    new ColumnSymbol("Timestamp", ScalarTypes.DateTime),
                    new ColumnSymbol("OperationId", ScalarTypes.String),
                    new ColumnSymbol("Name", ScalarTypes.String),
                    new ColumnSymbol("LastSuccessRun", ScalarTypes.DateTime),
                    new ColumnSymbol("FailureKind", ScalarTypes.String),
                    new ColumnSymbol("Details", ScalarTypes.String)));

        public static readonly CommandSymbol DropContinuousExport =
            new CommandSymbol("drop continuous-export", "drop continuous-export <name>:ContinousExportName", ShowContinuousExportResult);

        public static readonly CommandSymbol EnableContinuousExport =
            new CommandSymbol("enable continuous-export", "enable continuous-export <name>:ContinousExportName", ShowContinuousExportResult);

        public static readonly CommandSymbol DisableContinuousExport =
            new CommandSymbol("disable continuous-export", "disable continuous-export <name>:ContinousExportName", ShowContinuousExportResult);

        #endregion

        #region System Information Commands
        public static readonly CommandSymbol ShowCluster =
            new CommandSymbol("show cluster",
                "show cluster",
                new TableSymbol(
                    new ColumnSymbol("NodeId", ScalarTypes.String),
                    new ColumnSymbol("Address", ScalarTypes.String),
                    new ColumnSymbol("Name", ScalarTypes.String),
                    new ColumnSymbol("StartTime", ScalarTypes.DateTime),
                    new ColumnSymbol("IsAdmin", ScalarTypes.Bool),
                    new ColumnSymbol("MachineTotalMemory", ScalarTypes.Long),
                    new ColumnSymbol("MachineAvailableMemory", ScalarTypes.Long),
                    new ColumnSymbol("ProcessorCount", ScalarTypes.Int),
                    new ColumnSymbol("EnvironmentDescription", ScalarTypes.String)));

        public static readonly CommandSymbol ShowDiagnostics =
            new CommandSymbol("show diagnostics",
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
            new CommandSymbol("show capacity",
                "show capacity",
                new TableSymbol(
                    new ColumnSymbol("Resource", ScalarTypes.String),
                    new ColumnSymbol("Total", ScalarTypes.Long),
                    new ColumnSymbol("Consumed", ScalarTypes.Long),
                    new ColumnSymbol("Remaining", ScalarTypes.Long)));

        public static readonly CommandSymbol ShowOperations =
            new CommandSymbol("show operations",
                "show operations [(<guid>:OperationId | '(' { <guid>:OperationId, ',' }+ ')')]",
                new TableSymbol(
                    new ColumnSymbol("OperationId", ScalarTypes.Guid),
                    new ColumnSymbol("Operation", ScalarTypes.String),
                    new ColumnSymbol("NodeId", ScalarTypes.String),
                    new ColumnSymbol("StartedOn", ScalarTypes.DateTime),
                    new ColumnSymbol("LastUpdatedOn", ScalarTypes.DateTime),
                    new ColumnSymbol("Duration", ScalarTypes.TimeSpan),
                    new ColumnSymbol("State", ScalarTypes.String),
                    new ColumnSymbol("Status", ScalarTypes.String),
                    new ColumnSymbol("RootActivityId", ScalarTypes.Guid),
                    new ColumnSymbol("ShouldRetry", ScalarTypes.Bool),
                    new ColumnSymbol("Database", ScalarTypes.String),
                    new ColumnSymbol("Principal", ScalarTypes.String),
                    new ColumnSymbol("User", ScalarTypes.String),
                    new ColumnSymbol("AdminEpochStartTime", ScalarTypes.DateTime)));

        public static readonly CommandSymbol ShowOperationDetails =
            new CommandSymbol("show operation details",
                "show operation <guid>:OperationId details",
                new TableSymbol().Open()); // schema depends on operation

        private static readonly TableSymbol JournalResult =
            new TableSymbol(
                new ColumnSymbol("Event", ScalarTypes.String),
                new ColumnSymbol("EventTimestamp", ScalarTypes.DateTime),
                new ColumnSymbol("Database", ScalarTypes.String),
                new ColumnSymbol("EntityName", ScalarTypes.String),
                new ColumnSymbol("UpdatedEntityName", ScalarTypes.String),
                new ColumnSymbol("EntityVersion", ScalarTypes.String),
                new ColumnSymbol("EntityContainerName", ScalarTypes.String),
                new ColumnSymbol("OriginalEntityState", ScalarTypes.String),
                new ColumnSymbol("UpdatedEntityState", ScalarTypes.String),
                new ColumnSymbol("ChangeCommand", ScalarTypes.String),
                new ColumnSymbol("Principal", ScalarTypes.String),
                new ColumnSymbol("RootActivityId", ScalarTypes.Guid),
                new ColumnSymbol("ClientRequestId", ScalarTypes.String),
                new ColumnSymbol("User", ScalarTypes.String),
                new ColumnSymbol("OriginalEntityVersion", ScalarTypes.String));

        public static readonly CommandSymbol ShowJournal =
            new CommandSymbol("show journal", JournalResult);

        public static readonly CommandSymbol ShowDatabaseJournal =
            new CommandSymbol("show database journal", "show database <database>:DatabaseName journal", JournalResult);

        public static readonly CommandSymbol ShowClusterJournal =
            new CommandSymbol("show cluster journal", JournalResult);

        private static readonly TableSymbol QueryResults =
            new TableSymbol(
                new ColumnSymbol("ClientActivityId", ScalarTypes.String),
                new ColumnSymbol("Text", ScalarTypes.String),
                new ColumnSymbol("Database", ScalarTypes.String),
                new ColumnSymbol("StartedOn", ScalarTypes.DateTime),
                new ColumnSymbol("LastUpdatedOn", ScalarTypes.DateTime),
                new ColumnSymbol("Duration", ScalarTypes.TimeSpan),
                new ColumnSymbol("State", ScalarTypes.String),
                new ColumnSymbol("RootActivityId", ScalarTypes.String),
                new ColumnSymbol("User", ScalarTypes.String),
                new ColumnSymbol("FailureReason", ScalarTypes.String),
                new ColumnSymbol("TotalCpu", ScalarTypes.TimeSpan),
                new ColumnSymbol("CacheStatistics", ScalarTypes.Dynamic),
                new ColumnSymbol("Application", ScalarTypes.String),
                new ColumnSymbol("MemoryPeak", ScalarTypes.Long),
                new ColumnSymbol("ScannedExtentsStatistics", ScalarTypes.Dynamic),
                new ColumnSymbol("Principal", ScalarTypes.String),
                new ColumnSymbol("ClientRequestProperties", ScalarTypes.Dynamic),
                new ColumnSymbol("ResultSetStatistics", ScalarTypes.Dynamic));

        public static readonly CommandSymbol ShowQueries =
            new CommandSymbol("show queries", QueryResults);

        public static readonly CommandSymbol ShowRunningQueries =
            new CommandSymbol("show running queries", "show running queries [by (user <string>:UserName | '*')]", QueryResults);

        public static readonly CommandSymbol CancelQuery =
            new CommandSymbol("cancel query",
                "cancel query <string>:ClientRequestId");

        public static readonly CommandSymbol ShowQueryPlan =
            new CommandSymbol("show queryplan", "show queryplan '<|' <input_query>:Query",
                new TableSymbol(
                    new ColumnSymbol("ResultType", ScalarTypes.String),
                    new ColumnSymbol("RelopTree", ScalarTypes.String),
                    new ColumnSymbol("QueryPlan", ScalarTypes.String),
                    new ColumnSymbol("Stat", ScalarTypes.String)));

        public static readonly CommandSymbol ShowBasicAuthUsers =
            new CommandSymbol("show basicauth users",
                new TableSymbol(
                    new ColumnSymbol("UserName", ScalarTypes.String)));

        public static readonly CommandSymbol ShowCache =
            new CommandSymbol("show cache",
                new TableSymbol(
                    new ColumnSymbol("NodeId", ScalarTypes.String),
                    new ColumnSymbol("TotalMemoryCapacity", ScalarTypes.Long),
                    new ColumnSymbol("MemoryCacheCapacity", ScalarTypes.Long),
                    new ColumnSymbol("MemoryCacheInUse", ScalarTypes.Long),
                    new ColumnSymbol("MemoryCacheHitCount", ScalarTypes.Long),
                    new ColumnSymbol("TotalDiskCapacity", ScalarTypes.Long),
                    new ColumnSymbol("DiskCacheCapacity", ScalarTypes.Long),
                    new ColumnSymbol("DiskCacheInUse", ScalarTypes.Long),
                    new ColumnSymbol("DiskCacheHitCount", ScalarTypes.Long),
                    new ColumnSymbol("DiskCacheMissCount", ScalarTypes.Long),
                    new ColumnSymbol("MemoryCacheDetails", ScalarTypes.String),
                    new ColumnSymbol("DiskCacheDetails", ScalarTypes.String)));

        public static readonly CommandSymbol ShowCommands =
            new CommandSymbol("show commands", "show commands",
                new TableSymbol(
                    new ColumnSymbol("ClientActivityId", ScalarTypes.String),
                    new ColumnSymbol("CommandType", ScalarTypes.String),
                    new ColumnSymbol("Text", ScalarTypes.String),
                    new ColumnSymbol("Database", ScalarTypes.String),
                    new ColumnSymbol("StartedOn", ScalarTypes.DateTime),
                    new ColumnSymbol("LastUpdatedOn", ScalarTypes.DateTime),
                    new ColumnSymbol("Duration", ScalarTypes.TimeSpan),
                    new ColumnSymbol("State", ScalarTypes.String),
                    new ColumnSymbol("RootActivityId", ScalarTypes.Guid),
                    new ColumnSymbol("User", ScalarTypes.String),
                    new ColumnSymbol("FailureReason", ScalarTypes.String),
                    new ColumnSymbol("Application", ScalarTypes.String),
                    new ColumnSymbol("Principal", ScalarTypes.String),
                    new ColumnSymbol("TotalCpu", ScalarTypes.TimeSpan),
                    new ColumnSymbol("ResourcesUtilization", ScalarTypes.Dynamic),
                    new ColumnSymbol("ClientRequestProperties", ScalarTypes.Dynamic)));

        public static readonly CommandSymbol ShowCommandsAndQueries =
            new CommandSymbol("show commands-and-queries", "show commands-and-queries",
                new TableSymbol(
                    new ColumnSymbol("ClientActivityId", ScalarTypes.String),
                    new ColumnSymbol("CommandType", ScalarTypes.String),
                    new ColumnSymbol("Text", ScalarTypes.String),
                    new ColumnSymbol("Database", ScalarTypes.String),
                    new ColumnSymbol("StartedOn", ScalarTypes.DateTime),
                    new ColumnSymbol("LastUpdatedOn", ScalarTypes.DateTime),
                    new ColumnSymbol("Duration", ScalarTypes.TimeSpan),
                    new ColumnSymbol("State", ScalarTypes.String),
                    new ColumnSymbol("FailureReason", ScalarTypes.String),
                    new ColumnSymbol("RootActivityId", ScalarTypes.Guid),
                    new ColumnSymbol("User", ScalarTypes.String),
                    new ColumnSymbol("Application", ScalarTypes.String),
                    new ColumnSymbol("Principal", ScalarTypes.String),
                    new ColumnSymbol("ClientRequestProperties", ScalarTypes.Dynamic),
                    new ColumnSymbol("TotalCpu", ScalarTypes.TimeSpan),
                    new ColumnSymbol("MemoryPeak", ScalarTypes.Long),
                    new ColumnSymbol("CacheStatistics", ScalarTypes.Dynamic),
                    new ColumnSymbol("ScannedExtentsStatistics", ScalarTypes.Dynamic),
                    new ColumnSymbol("ResultSetStatistics", ScalarTypes.Dynamic)));

        public static readonly CommandSymbol ShowIngestionFailures =
            new CommandSymbol("show ingestion failures", "show ingestion failures [with '(' OperationId '=' <guid>:OperationId ')']",
                new TableSymbol(
                    new ColumnSymbol("OperationId", ScalarTypes.String),
                    new ColumnSymbol("Database", ScalarTypes.String),
                    new ColumnSymbol("Table", ScalarTypes.String),
                    new ColumnSymbol("FailedOn", ScalarTypes.DateTime),
                    new ColumnSymbol("IngestionSourcePath", ScalarTypes.String),
                    new ColumnSymbol("Details", ScalarTypes.String),
                    new ColumnSymbol("FailureKind", ScalarTypes.String),
                    new ColumnSymbol("RootActivityId", ScalarTypes.String),
                    new ColumnSymbol("OperationKind", ScalarTypes.String),
                    new ColumnSymbol("OriginatesFromUpdatePolicy", ScalarTypes.Bool)));

        #endregion

        #region Advanced Commands
        private static readonly TableSymbol ShowExtentsResults =
            new TableSymbol(
                new ColumnSymbol("ExtentId", ScalarTypes.Guid),
                new ColumnSymbol("DatabaseName", ScalarTypes.String),
                new ColumnSymbol("TableName", ScalarTypes.String),
                new ColumnSymbol("MaxCreatedOn", ScalarTypes.DateTime),
                new ColumnSymbol("OriginalSize", ScalarTypes.Real),
                new ColumnSymbol("ExtentSize", ScalarTypes.Real),
                new ColumnSymbol("CompressedSize", ScalarTypes.Real),
                new ColumnSymbol("IndexSize", ScalarTypes.Real),
                new ColumnSymbol("Blocks", ScalarTypes.Long),
                new ColumnSymbol("Segments", ScalarTypes.Long),
                new ColumnSymbol("AssignedDataNodes", ScalarTypes.String),
                new ColumnSymbol("LoadedDataNodes", ScalarTypes.String),
                new ColumnSymbol("ExtentContainerId", ScalarTypes.String),
                new ColumnSymbol("RowCount", ScalarTypes.Long),
                new ColumnSymbol("MinCreatedOn", ScalarTypes.DateTime),
                new ColumnSymbol("Tags", ScalarTypes.String));

        public static readonly CommandSymbol ShowClusterExtents =
            new CommandSymbol("show cluster extents", "show cluster extents [hot]", ShowExtentsResults);

        private static readonly string ExtentIdList = "'(' {<guid>:ExtentId, ','}+ ')'";
        private static readonly string TagWhereClause = "where { tags (has | contains | '!has' | '!contains')! <string>:Tag, and }+";

        public static readonly CommandSymbol ShowDatabaseExtents =
            new CommandSymbol("show database extents", $"show database <database>:DatabaseName extents [{ExtentIdList}] [hot] [{TagWhereClause}]", ShowExtentsResults);

        public static readonly CommandSymbol ShowTableExtents =
            new CommandSymbol("show table extents", $"show table <table>:TableName extents [{ExtentIdList}] [hot] [{TagWhereClause}]", ShowExtentsResults);

        public static readonly CommandSymbol ShowTablesExtents =
            new CommandSymbol("show tables extents", $"show tables {TableNameList} extents [{ExtentIdList}] [hot] [{TagWhereClause}]", ShowExtentsResults);

        private static readonly TableSymbol MergeExtentsResult =
            new TableSymbol(
                    new ColumnSymbol("OriginalExtentId", ScalarTypes.String),
                    new ColumnSymbol("ResultExtentId", ScalarTypes.String),
                    new ColumnSymbol("Duration", ScalarTypes.TimeSpan));

        private static readonly string GuidList = "'(' {<guid>:GUID, ','}+ ')'";
        public static readonly CommandSymbol MergeExtents =
            new CommandSymbol("merge extents", $"merge [async] <table>:TableName {GuidList} [with '(' rebuild '=' true ')']", MergeExtentsResult);

        public static readonly CommandSymbol MergeExtentsDryrun =
            new CommandSymbol("merge extents dryrun", $"merge dryrun <table>:TableName {GuidList}", MergeExtentsResult);

        private static readonly TableSymbol MoveExtentsResult =
            new TableSymbol(
                new ColumnSymbol("OriginalExtentId", ScalarTypes.String),
                new ColumnSymbol("ResultExtentId", ScalarTypes.String),
                new ColumnSymbol("Details", ScalarTypes.String));

        public static readonly CommandSymbol MoveExtentsFrom =
            new CommandSymbol("move extents from", $"move [async] extents (all | {GuidList}) from table <table>:SourceTableName to table <table>:DestinationTableName", MoveExtentsResult);

        public static readonly CommandSymbol MoveExtentsQuery =
            new CommandSymbol("move extents query", $"move [async] extents to table <table>:DestinationTableName '<|' <input_query>:Query", MoveExtentsResult);

        private static readonly TableSymbol DropExtentResult =
            new TableSymbol(
                new ColumnSymbol("ExtentId", ScalarTypes.String),
                new ColumnSymbol("TableName", ScalarTypes.String),
                new ColumnSymbol("CreatedOn", ScalarTypes.DateTime));

        //public static readonly CommandSymbol DropExtentsQuery =
        //    new CommandSymbol("drop extents query", "drop extents [whatif] '<|' <input_query>:Query", DropExtentResult);

        public static readonly CommandSymbol DropExtent =
            new CommandSymbol("drop extent", "drop extent <guid>:ExtentId [from <table>:TableName]", DropExtentResult);

        //public static readonly CommandSymbol DropExtents =
        //    new CommandSymbol("drop extents", "drop extents '(' { <guid>:ExtentId, ',' } ')' [from <table>:TableName]", DropExtentResult);

        private static readonly string DropProperties = "[older <long>:Older (days | hours)] from (all tables! | <table>:TableName) [trim by! (extentsize | datasize) <long>:TrimSize (MB | GB | bytes)] [limit <long>:LimitCount]";

        public static readonly CommandSymbol DropExtents =
            new CommandSymbol("drop extents",
                @"drop extents 
                    ('(' { <guid>:ExtentId, ',' }+ ')' [from <table>:TableName]
                     | whatif '<|'! <input_query>:Query
                     | '<|' <input_query>:Query
                     | older <long>:Older (days | hours) from (all tables! | <table>:TableName) [trim by! (extentsize | datasize)! <long>:TrimSize (MB | GB | bytes)] [limit <long>:LimitCount]
                     | from (all tables! | <table>:TableName) [trim by! (extentsize | datasize) <long>:TrimSize (MB | GB | bytes)] [limit <long>:LimitCount]
                     )", DropExtentResult);

        public static readonly CommandSymbol DropPretendExtentsByProperties =
            new CommandSymbol("drop-pretend extents by properties", $"drop-pretend extents {DropProperties}", DropExtentResult);

        public static readonly CommandSymbol ShowVersion =
            new CommandSymbol("show version", "show version",
                new TableSymbol(
                    new ColumnSymbol("BuildVersion", ScalarTypes.String),
                    new ColumnSymbol("BuildTime", ScalarTypes.DateTime),
                    new ColumnSymbol("ServiceType", ScalarTypes.String),
                    new ColumnSymbol("ProductVersion", ScalarTypes.String)));

        public static readonly CommandSymbol ClearTableData =
           new CommandSymbol("clear table data", "clear [async] table <table>:TableName data",
               new TableSymbol(
                   new ColumnSymbol("Status", ScalarTypes.String)));

        public static readonly CommandSymbol TableStreamingIngestionSchemaCacheClear =
           new CommandSymbol("clear table cache streamingingestion schema", "clear table <table>:TableName cache streamingingestion schema");

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
                TableStreamingIngestionSchemaCacheClear,
                #endregion
            };
    }
}
