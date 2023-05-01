---
title: Ladder chart visualization - Azure Data Explorer
description: This article describes the ladder chart visualization in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 08/03/2022
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# Ladder chart

::: zone pivot="azuredataexplorer"

 Last two columns are the x-axis, other columns are y-axis

> [!NOTE]
> * This visualization can only be used in the context of the [render operator](renderoperator.md).
> * This visualization can be used in Kusto.Explorer but is not available in the Azure Data Explorer web UI.

## Syntax

*T* `|` `render` `ladderchart` [`with` `(`*propertyName* `=` *propertyValue* [`,` ...]`)`]

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
| *T* | string | &check; | Input table name.
| *propertyName*, *propertyValue* | string | | A comma-separated list of key-value property pairs. See [supported properties](#supported-properties).|

### Supported properties

All properties are optional.
    
|*PropertyName*|*PropertyValue*                                                                   |
|--------------|----------------------------------------------------------------------------------|
|`accumulate`  |Whether the value of each measure gets added to all its predecessors. (`true` or `false`)|
|`legend`      |Whether to display a legend or not (`visible` or `hidden`).                       |
|`series`      |Comma-delimited list of columns whose combined per-record values define the series that record belongs to.|
|`ymin`        |The minimum value to be displayed on Y-axis.                                      |
|`ymax`        |The maximum value to be displayed on Y-axis.                                      |
|`title`       |The title of the visualization (of type `string`).                                |
|`xaxis`       |How to scale the x-axis (`linear` or `log`).                                      |
|`xcolumn`     |Which column in the result is used for the x-axis.                                |
|`xtitle`      |The title of the x-axis (of type `string`).                                       |
|`yaxis`       |How to scale the y-axis (`linear` or `log`).                                      |
|`ycolumns`    |Comma-delimited list of columns that consist of the values provided per value of the x column.|
|`ytitle`      |The title of the y-axis (of type `string`).                                       |

::: zone-end

::: zone pivot="azuremonitor"

This visualization isn't supported in Azure Monitor.

::: zone-end