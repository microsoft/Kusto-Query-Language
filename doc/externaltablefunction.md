---
title: external_table() - Azure Data Explorer
description: This article describes external_table() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 08/21/2019
---
# external_table()

References an [external table](schema-entities/externaltables.md) by name.

```kusto
external_table('StormEvent')
```

> [!NOTE]
> * The `external_table` function has similar restrictions as the [table](tablefunction.md) function.
> * Standard [query limits](../concepts/querylimits.md) apply to external table queries as well.

## Syntax

`external_table` `(` *TableName* [`,` *MappingName* ] `)`

## Arguments

* *TableName*: The name of the external table being queried.
  Must be a string literal referencing an external table of kind
  `blob`, `adl` or `sql`.

* *MappingName*: An optional name of the mapping object that maps the
  fields in the actual (external) data shards to the columns output
  by this function.

## Next steps

* [External tables overview](schema-entities/externaltables.md)
* [Create and alter Azure Storage external tables](../management/external-tables-azurestorage-azuredatalake.md)
* [Create and alter SQL Server external tables](../management/external-sql-tables.md)