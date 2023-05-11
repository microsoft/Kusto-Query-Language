---
title: Set statement - Azure Data Explorer
description: Learn how to use the set statement to set a query option for the duration of the query.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/30/2023
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# Set statement

::: zone pivot="azuredataexplorer"

The `set` statement is used to set a query option for the duration of the query.
Query options control how a query executes and returns results. They can be boolean flags, which are `false` by default, or have an integer value. A query may contain zero, one, or more set statements. Set statements affect only the tabular expression statements that trail them in the program order. Any two statements must be separated by a semicolon.

* Query options can also be enabled programmatically by setting them in the
  `ClientRequestProperties` object. For more information, see [ClientRequestProperties](../api/netfx/request-properties.md).
  
* Query options aren't formally a part of the Kusto language, and may be
  modified without being considered as a breaking language change.

## Syntax

`set` *OptionName* [`=` *OptionValue*]

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *OptionName* | string | &check; | The name of the query option.|
| *OptionValue* | | &check; | The value of the query option.|

## Example

```kusto
set querytrace;
Events | take 100
```

::: zone-end

::: zone pivot="azuremonitor"

This capability isn't supported in Azure Monitor.

::: zone-end
