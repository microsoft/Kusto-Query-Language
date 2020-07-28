---
title: externaldata operator - Azure Data Explorer
description: This article describes the external data operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: rkarlin
ms.service: data-explorer
ms.topic: reference
ms.date: 03/24/2020
---
# externaldata operator

The `externaldata` operator returns a table whose schema is defined in the query itself, and whose data is read from an external storage artifact, such as a blob in
Azure Blob Storage.

**Syntax**

`externaldata` `(` *ColumnName* `:` *ColumnType* [`,` ...] `)` `[` *StorageConnectionString* `]` [`with` `(` *Prop1* `=` *Value1* [`,` ...] `)`]

**Arguments**

* *ColumnName*, *ColumnType*: The arguments define the schema of the table.
  The syntax is the same as the syntax used when defining a table in [.create table](../management/create-table-command.md).

* *StorageConnectionString*: The [storage connection string](../api/connection-strings/storage.md) describes the storage artifact holding the data to return.

* *Prop1*, *Value1*, ...: Additional properties that describe how to interpret
  the data retrieved from storage, as listed under [ingestion properties](../../ingestion-properties.md).
    * Currently supported properties: `format` and `ignoreFirstRecord`.
    * Supported data formats: Any of the [ingestion data formats](../../ingestion-supported-formats.md) are supported, including `CSV`, `TSV`, `JSON`, `Parquet`, `Avro`.

> [!NOTE]
> This operator does not have a pipeline input.

**Returns**

The `externaldata` operator returns a data table of the given schema whose data was parsed from the specified storage artifact, indicated by the storage connection string.

**Examples**

The following example shows how to find all records in a table whose `UserID` column falls into a known set of IDs, held (one per line) in an external blob.
Because the set is indirectly referenced by the query, it can be large.

```kusto
Users
| where UserID in ((externaldata (UserID:string) [
    @"https://storageaccount.blob.core.windows.net/storagecontainer/users.txt"
      h@"?...SAS..." // Secret token needed to access the blob
    ]))
| ...
```

The following example queries multiple data files stored in external storage.

```kusto
externaldata(Timestamp:datetime, ProductId:string, ProductDescription:string)
[
  h@"https://mycompanystorage.blob.core.windows.net/archivedproducts/2019/01/01/part-00000-7e967c99-cf2b-4dbb-8c53-ce388389470d.csv.gz?...SAS...",
  h@"https://mycompanystorage.blob.core.windows.net/archivedproducts/2019/01/02/part-00000-ba356fa4-f85f-430a-8b5a-afd64f128ca4.csv.gz?...SAS...",
  h@"https://mycompanystorage.blob.core.windows.net/archivedproducts/2019/01/03/part-00000-acb644dc-2fc6-467c-ab80-d1590b23fc31.csv.gz?...SAS..."
]
with(format="csv")
| summarize count() by ProductId
```

The above example can be thought of as a quick way to query multiple data files without defining an [external table](schema-entities/externaltables.md).

> [!NOTE]
> Data partitioning isn't recognized by the `externaldata()` operator.
