---
title: Set statement - Azure Data Explorer | Microsoft Docs
description: This article describes Set statement in Azure Data Explorer.
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
# Set statement

::: zone pivot="azuredataexplorer"

The `set` statement is used to set a query option for the duration of the query.
Query options control how a query executes and returns results. They can be
Boolean flags (off by default), or have an integer value. A query may contain
zero, one, or more set statements. Set statements affect only the tabular expression
statements that trail them in the program order.

* Query options can also be enabled programmatically by setting them in the
  `ClientRequestProperties` object. See [here](../api/netfx/request-properties.md).
  
* Query options are not formally a part of the Kusto language, and may be
  modified without being considered as a breaking language change.

## Syntax

`set` *OptionName* [`=` *OptionValue*]

## Example

```kusto
set querytrace;
Events | take 100
```

::: zone-end

::: zone pivot="azuremonitor"

This capability isn't supported in Azure Monitor

::: zone-end
