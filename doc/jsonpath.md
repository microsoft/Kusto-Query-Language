---
title: JSONPath syntax - Azure Data Explorer
description: Learn how to use JSONPath expressions to specify data mappings and KQL functions that process dynamic objects.
ms.reviewer: igborodi
ms.topic: reference
ms.date: 12/22/2022
---

# JSONPath expressions

JSONPath notation describes the path to one or more elements in a JSON document.

The JSONPath notation is used in the following scenarios:

- To specify [data mappings for ingestion](../management/mappings.md)
- To specify [data mappings for external tables](../management/external-table-mapping-create.md)
- In KQL functions that process dynamic objects, like [bag_remove_keys()](bag-remove-keys-function.md) and [extract_json()](extractjsonfunction.md)

The following subset of the JSONPath notation is supported:

|Path expression|Description|
|---|---|
|`$`|Root object|
|`.` | Selects the specified property in a parent object. <br> Use this notation if the property doesn't contain special characters. |
|`['property']` or `["property"]`| Selects the specified property in a parent object. Make sure you put single quotes or double quotes around the property name. <br> Use this notation if the property name contains special characters, such as spaces, or begins with a character other than `A..Za..z_`. |
|`[n]`| Selects the n-th element from an array. Indexes are 0-based. |

> [!NOTE]
>
> Wildcards, recursion, union, slices, and current object are not supported.
