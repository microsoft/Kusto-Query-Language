---
title: externaldata operator - Azure Data Explorer
description: This article describes the external data operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 03/24/2020
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# externaldata operator

The `externaldata` operator returns a table whose schema is defined in the query itself, and whose data is read from an external storage artifact, such as a blob in 
Azure Blob Storage or a file in Azure Data Lake Storage.

::: zone pivot="azuredataexplorer"

::: zone-end

::: zone pivot="azuremonitor"

> [!NOTE]
> `externaldata` operator usage in Azure Monitor should be limited to small reference tables. It is not designed for large data volumes. If large volumes are needed, it is better to ingest them as custom logs.

::: zone-end


## Syntax

`externaldata` `(` *ColumnName* `:` *ColumnType* [`,` ...] `)`   
`[` *StorageConnectionString* [`,` ...] `]`   
[`with` `(` *PropertyName* `=` *PropertyValue* [`,` ...] `)`]

## Arguments

* *ColumnName*, *ColumnType*: The arguments define the schema of the table.
  The syntax is the same as the syntax used when defining a table in [`.create table`](../management/create-table-command.md).

* *StorageConnectionString*: [Storage connection strings](../api/connection-strings/storage.md) that describe the storage artifacts holding the data to return.

* *PropertyName*, *PropertyValue*, ...: Additional properties that describe how to interpret
  the data retrieved from storage, as listed under [ingestion properties](../../ingestion-properties.md).

Currently supported properties are:

| Property         | Type     | Description       |
|------------------|----------|-------------------|
| `format`         | `string` | Data format. If not specified, an attempt is made to detect the data format from file extension (defaults to `CSV`). Any of the [ingestion data formats](../../ingestion-supported-formats.md) are supported. |
| `ignoreFirstRecord` | `bool` | If set to true, indicates that the first record in every file is ignored. This property is useful when querying CSV files with headers. |
| `ingestionMapping` | `string` | A string value that indicates how to map data from the source file to the actual columns in the operator result set. See [data mappings](../management/mappings.md). |


> [!NOTE]
> * This operator doesn't accept any pipeline input.
> * Standard [query limits](../concepts/querylimits.md) apply to external data queries as well.

## Returns

The `externaldata` operator returns a data table of the given schema whose data was parsed from the specified storage artifact, indicated by the storage connection string.

## Examples

**Fetch a list of user IDs stored in Azure Blob Storage**

The following example shows how to find all records in a table whose `UserID` column falls into a known set of IDs, held (one per line) in an external storage file. Since the data format isn't specified, the detected data format is `TXT`.

```kusto
Users
| where UserID in ((externaldata (UserID:string) [
    @"https://storageaccount.blob.core.windows.net/storagecontainer/users.txt" 
      h@"?...SAS..." // Secret token needed to access the blob
    ]))
| ...
```

**Query multiple data files**

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
> Data partitioning isn't recognized by the `externaldata` operator.

**Query hierarchical data formats**

To query hierarchical data format, such as `JSON`, `Parquet`, `Avro`, or `ORC`, `ingestionMapping` must be specified in the operator properties. 
In this example, there's a JSON file stored in Azure Blob Storage with the following contents:

```JSON
{
  "timestamp": "2019-01-01 10:00:00.238521",   
  "data": {    
    "tenant": "e1ef54a6-c6f2-4389-836e-d289b37bcfe0",   
    "method": "RefreshTableMetadata"   
  }   
}   
{
  "timestamp": "2019-01-01 10:00:01.845423",   
  "data": {   
    "tenant": "9b49d0d7-b3e6-4467-bb35-fa420a25d324",   
    "method": "GetFileList"   
  }   
}
...
```

To query this file using the `externaldata` operator, a data mapping must be specified. The mapping dictates how to map JSON fields to the operator result set columns:

```kusto
externaldata(Timestamp: datetime, TenantId: guid, MethodName: string)
[ 
   h@'https://mycompanystorage.blob.core.windows.net/events/2020/09/01/part-0000046c049c1-86e2-4e74-8583-506bda10cca8.json?...SAS...'
]
with(format='multijson', ingestionMapping='[{"Column":"Timestamp","Properties":{"Path":"$.time"}},{"Column":"TenantId","Properties":{"Path":"$.data.tenant"}},{"Column":"MethodName","Properties":{"Path":"$.data.method"}}]')
```

The `MultiJSON` format is used here because single JSON records are spanned into multiple lines.

For more info on mapping syntax, see [data mappings](../management/mappings.md).
