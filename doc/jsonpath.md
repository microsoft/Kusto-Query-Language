---
title: JSONPath syntax - Azure Data Explorer
description: This article describes JSONPath expressions in Azure Data Explorer.
ms.reviewer: igborodi
ms.topic: reference
ms.date: 12/20/2021
---

# JSONPath expressions

JSONPath notation describes the path to one or more elements in a JSON document.

The JSONPath notation is used in the following scenarios:

- To specify [data mappings for ingestion](../management/mappings.md)
- To specify [data mappings for external tables](../management/external-tables-azurestorage-azuredatalake.md#create-external-table-mapping)
- In KQL functions that process dynamic objects, like [bag_remove_keys()](bag-remove-keys-function.md) and [extractjson()](extractjsonfunction.md)

The following subset of the JSONPath notation is supported:

|Path expression|Description|
|---|---|
|`$`|Root object|
|`.` or `[ ]` | Child|
|`[ ]`|Array subscript|

> [!NOTE]
>
> - Wildcards, recursion, union, slices and current object are not supported.
> - JSON paths that include special characters should be escaped as [\'Property Name\'].
