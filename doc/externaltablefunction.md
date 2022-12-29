---
title: external_table() - Azure Data Explorer
description: Learn how to use the external_table() function to reference an external table by name.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/12/2022
---
# external_table()

References an [external table](schema-entities/externaltables.md) by name.

> [!NOTE]
>
> The `external_table` function has similar restrictions as the [table](tablefunction.md) function.
> Standard [query limits](../concepts/querylimits.md) apply to external table queries as well.

## Syntax

`external_table` `(` *TableName* [`,` *MappingName* ] `)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *TableName* | string| &check; | The name of the external table being queried. Must reference an external table of kind `blob`, `adl`, or `sql`.|
| *MappingName* | string | | A name of a mapping object that maps fields in the external data shards to columns output.|

## Next steps

* [External tables overview](schema-entities/externaltables.md)
* [Create and alter Azure Storage external tables](../management/external-tables-azurestorage-azuredatalake.md)
* [Create and alter SQL Server external tables](../management/external-sql-tables.md).
