# externaldata operator

The `externaldata` operator returns a table whose schema is defined in the query itself,
and whose data is read from an external storage artifact (such as a blob in
Azure Blob Storage).

> [!NOTE]
> This operator does not have a pipeline input.

**Syntax**

`externaldata` `(` *ColumnName* `:` *ColumnType* [`,` ...] `)` `[` *StorageConnectionString* `]` [`with` `(` *Prop1* `=` *Value1* [`,` ...] `)`]

**Arguments**

* *ColumnName*, *ColumnType*: Define the schema of the table.
  The syntax is the same as the syntax used when defining a table in [.create table](../management/create-table-command.md).

* *StorageConnectionString*: The [storage connection string](../api/connection-strings/storage.md)
  describes the storage artifact holding the data to return.

* *Prop1*, *Value1*, ...: Additional properties that describe how to interpret
  the data retrieved from storage, as listed under [ingestion properties](../management/data-ingestion/index.md).
    * Currently supported properties: `format` and `ignoreFirstRecord`.
    * Supported data formats: any of the [ingestion data formats](https://docs.microsoft.com/azure/data-explorer/ingestion-supported-formats)
      are supported, including `csv`, `tsv`, `json`, `parquet`, `avro`.

**Returns**

The `externaldata` operator returns a data table of the given schema
whose data was parsed from the specified storage artifact
indicated by the storage connection string.

**Example**

The following example shows how to find all records in a table whose
`UserID` column falls into a known set of IDs, held (one per line) in an external blob.
Because the set is indirectly referenced by the query, it can be very large.

```
Users
| where UserID in ((externaldata (UserID:string) [
    @"https://storageaccount.blob.core.windows.net/storagecontainer/users.txt"
      h@"?...SAS..." // Secret token needed to access the blob
    ]))
| ...
```
