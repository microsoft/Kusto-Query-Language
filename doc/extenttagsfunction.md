---
title: extent_tags() - Azure Data Explorer | Microsoft Docs
description: This article describes extent_tags() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# extent_tags()

::: zone pivot="azuredataexplorer"

Returns a dynamic array with the [tags](../management/extents-overview.md#extent-tagging) of the data shard ("extent") that the current record is in. 

Applying this function to calculated data, which isn't attached to a data shard, returns an empty value.

## Syntax

`extent_tags()`

## Returns

A value of type `dynamic` that is an array holding the current record's extent tags,
or an empty value.

## Examples

Some query operators preserve the information about the data shard hosting the record.
These operators include `where`, `extend`, and `project`.
The following example shows how to get a list the tags of all the data shards
that have records from an hour ago, with a specific value for the
column `ActivityId`. 


```kusto
T
| where Timestamp > ago(1h)
| where ActivityId == 'dd0595d4-183e-494e-b88e-54c52fe90e5a'
| extend tags = extent_tags()
| summarize by tostring(tags)
```

The following example shows how to obtain a count of all records from the 
last hour, which are stored in extents tagged with the tag `MyTag`
(and potentially other tags), but not tagged with the tag `drop-by:MyOtherTag`.

```kusto
T
| where Timestamp > ago(1h)
| extend Tags = extent_tags()
| where Tags has_cs 'MyTag' and Tags !has_cs 'drop-by:MyOtherTag'
| count
```

> [!NOTE]
> Filtering on the value of `extent_tags()` performs best when one of the following string operators is used:
> `has`, `has_cs`, `!has`, `!has_cs`.

::: zone-end

::: zone pivot="azuremonitor"

This capability isn't supported in Azure Monitor

::: zone-end
