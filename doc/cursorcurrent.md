---
title: cursor_current(), current_cursor() - Azure Data Explorer | Microsoft Docs
description: This article describes cursor_current(), current_cursor() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 12/10/2019
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# cursor_current(), current_cursor()

::: zone pivot="azuredataexplorer"

Retrieves the current value of the cursor of the database in scope. (The names `cursor_current`
and `current_cursor` are synonyms.)

## Syntax

`cursor_current()`

## Returns

Returns a single value of type `string` which encodes the current value of the
cursor of the database in scope.

**Notes**

See [database cursors](../management/databasecursor.md) for additional
details on database cursors.

::: zone-end

::: zone pivot="azuremonitor"

This capability isn't supported in Azure Monitor

::: zone-end
