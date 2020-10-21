---
title: extent_id() - Azure Data Explorer | Microsoft Docs
description: This article describes extent_id() in Azure Data Explorer.
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
# extent_id()

::: zone pivot="azuredataexplorer"

Returns a unique identifier that identifies the data shard ("extent") that the current record resides in.

Applying this function to calculated data that isn't attached to a data shard returns an empty guid (all zeros).

## Syntax

`extent_id()`

## Returns

A value of type `guid` that identifies the current record's data shard,
or an empty guid (all zeros).

## Example

The following example shows how to get a list of all the data shards
that have records from an hour ago with a specific value for the
column `ActivityId`. It demonstrates that some query operators (here,
the `where` operator, and also `extend` and `project`)
preserve the information about the data shard hosting the record.

```kusto
T
| where Timestamp > ago(1h)
| where ActivityId == 'dd0595d4-183e-494e-b88e-54c52fe90e5a'
| extend eid=extent_id()
| summarize by eid
```

::: zone-end

::: zone pivot="azuremonitor"

This capability isn't supported in Azure Monitor

::: zone-end
