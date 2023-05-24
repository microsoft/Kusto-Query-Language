---
title:  cursor_after()
description: Learn how to use the cursor_after() function to compare the ingestion time of the records of a table against the database cursor time.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/27/2020
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors-all
---
# cursor_after()

::: zone pivot="azuredataexplorer, fabric"

A predicate run over the records of a table to compare their ingestion time against a database cursor.

> [!NOTE]
> This function can only be invoked on records of a table that has the
[IngestionTime policy](../management/ingestiontimepolicy.md) enabled.

## Syntax

`cursor_after(`*RHS*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *RHS* | string | &check; | Either an empty string literal or a valid database cursor value.|

## Returns

A scalar value of type `bool` that indicates whether the record was ingested
after the database cursor *RHS* (`true`) or not (`false`).

## See also

See [database cursors](../management/databasecursor.md) for additional
details on database cursors.

::: zone-end

::: zone pivot="azuremonitor"

This capability isn't supported in Azure Monitor

::: zone-end
