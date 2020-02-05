# externaldata operator

The `externaldata` operator returns a table whose schema is defined in the query itself, and whose data is read from an external raw file.

> [!NOTE]
> This operator does not have a pipeline input.

**Syntax**

`externaldata` `(` *ColumnName* `:` *ColumnType* [`,` ...] `)` `[` *DataFileUri* `]` [`with` `(` *Prop1* `=` *Value1* [`,` ...] `)`]

**Arguments**

* *ColumnName*, *ColumnType*: Define the schema of the table. The syntax is the same as the syntax used when defining a table in [.create table](../management/create-table-command.md).
* *DataFileUri*: The URI (including authentication option, if any) for the file holding the data.
* *Prop1*, *Value1*, ...: Additional properties that describe how to interpret the data in the raw file, as listed under [ingestion properties](../management/data-ingestion/index.md).
    * Currently supported properties: `format` and `ignoreFirstRecord`.
    * Supported data formats: any of the [ingestion data formats](../management/data-ingestion/index.md#supported-data-formats) are supported, including `csv`, `tsv`, `json`, `parquet`, `avro`.

**Returns**

The `externaldata` operator returns a data table of the given schema, whose data was parsed from the specified URI.

**Example**

The following example shows you how to find all the records in a table whose `UserID` column falls into a known set of IDs, held (one per line) in an external blob. Because the set is indirectly referenced by the query, it can be very large.

<!-- csl -->
```
Users
| where UserID in ((externaldata (UserID:string) [
    @"https://storageaccount.blob.core.windows.net/storagecontainer/users.txt"
      h@"?...SAS..."
    ]))
| ...
```
