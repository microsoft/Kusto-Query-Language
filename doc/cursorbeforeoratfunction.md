---
title: cursor_before_or_at() - Azure Data Explorer
description: Learn how to use the cursor_before_or_at() function to compare the ingestion time of the records of a table against the database cursor time.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/24/2022
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# cursor_before_or_at()

::: zone pivot="azuredataexplorer"

A predicate function run over the records of a table to compare their ingestion time against the database cursor time.

## Syntax

`cursor_before_or_at` `(` *RHS* `)`

## Arguments

* *RHS*: Either an empty string literal, or a valid database cursor value.

## Returns

A scalar value of type `bool` that indicates whether the record was ingested
before or at the database cursor *RHS* (`true`) or not (`false`).

**Notes**

See [database cursors](../management/databasecursor.md) for additional
details on database cursors.

This function can only be invoked on records of a table which has the
[IngestionTime policy](../management/ingestiontimepolicy.md) enabled.

::: zone-end

::: zone pivot="azuremonitor"

This capability isn't supported in Azure Monitor

::: zone-end
