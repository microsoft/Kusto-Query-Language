---
title: external_table() - Azure Data Explorer | Microsoft Docs
description: This article describes external_table() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: rkarlin
ms.service: data-explorer
ms.topic: reference
ms.date: 08/21/2019
---
# external_table()

References an external table by name.

```kusto
external_table('StormEvent')
```

## Syntax

`external_table` `(` *TableName* [`,` *MappingName* ] `)`

## Arguments

* *TableName*: The name of the external table being queried.
  Must be a string literal referencing an external table of kind
  `blob` or `adl`. <!-- TODO: Document data formats supported -->

* *MappingName*: An optional name of the mapping object that maps the
  fields in the actual (external) data shards to the columns output
  by this function.

**Notes**

See [external tables](schema-entities/externaltables.md) for more information
on external tables.

See also [commands for managing external tables](../management/externaltables.md).

The `external_table` function has similar restrictions
as the [table](tablefunction.md) function.