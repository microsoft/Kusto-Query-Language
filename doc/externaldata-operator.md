---
title: externaldata operator - Azure Data Explorer | Microsoft Docs
description: This article describes externaldata operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 07/17/2019
---
# externaldata operator

The `externaldata` operator returns a table whose schema is defined in the query itself, and whose data is read from an external raw file.

> [!NOTE]
> This operator does not have a pipeline input.

**Syntax**

`externaldata` `(` *ColumnName* `:` *ColumnType* [`,` ...] `)` `[` *DataFileUri* `]` [`with` `(` *Prop1* `=` *Value1* [`,` ...] `)`]

**Arguments**

* *ColumnName*, *ColumnType*: Define the schema of the table. The syntax is the same as the syntax used when defining a table in [.create table](../management/tables.md#create-table).
* *DataFileUri*: The URI (including authentication option, if any) for the file holding the data.
* *Prop1*, *Value1*, ...: Additional properties that describe how to interpret the data in the raw file, as listed under [ingestion properties](../management/data-ingestion/index.md).
    * Currently supported properties: `format` and `ignoreFirstRecord`.
    * Currently supported [data formats](../management/data-ingestion/index.md#supported-data-formats) for this operator: `csv`, `tsv`, `scsv`, `sohsv`, `psv`, `txt`, `raw`.

**Returns**

The `externaldata` operator returns a data table of the given schema, whose data was parsed from the specified URI.

**Example**

The following example shows you how to find all the records in a table whose `UserID` column falls into a known set of IDs, held (one per line) in an external blob. Because the set is indirectly referenced by the query, it can be very large.

```kusto
Users
| where UserID in ((externaldata (UserID:string) [
    @"https://storageaccount.blob.core.windows.net/storagecontainer/users.txt"
      h@"?...SAS..."
    ]))
| ...
```