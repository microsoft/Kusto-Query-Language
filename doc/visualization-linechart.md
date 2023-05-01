---
title: Line chart visualization - Azure Data Explorer
description: This article describes the line chart visualization in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/26/2023
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# Line chart

::: zone pivot="azuredataexplorer"

The line chart visual is the most basic type of chart. The first column of the query should be numeric and is used as the x-axis. Other numeric columns are the y-axes. Line charts track changes over short and long periods of time. When smaller changes exist, line graphs are more useful than bar graphs.

 
> [!NOTE]
> This visualization can only be used in the context of the [render operator](renderoperator.md).

## Syntax

*T* `|` `render` `linechart` [`with` `(` *propertyName* `=` *propertyValue* [`,` ...] `)`]

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
|`ysplit`      |How to split multiple the visualization. For more information, see [Multiple y-axes](#ysplit-property).                             |
|`ytitle`      |The title of the y-axis (of type `string`).                                       |

#### `ysplit` property

This visualization supports splitting into multiple y-axis values:

|`ysplit`  |Description                                                       |
|----------|------------------------------------------------------------------|
|`none`    |A single y-axis is displayed for all series data. (Default)       |
|`axes`    |A single chart is displayed with multiple y-axes (one per series).|
|`panels`  |One chart is rendered for each `ycolumn` value (up to some limit).|
    
## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKM9ILUpVCC5JLEm1tVUK8wxy9/TzdFQCyhQU5WelJpeA5IpKQjJzU3UUXBJzE9NTA4ryC1KLSiqBaopS81JSixRyMvNSkzOAygCpk5aiXAAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| where State=="VIRGINIA"
| project StartTime, DamageProperty
| render linechart 
```

:::image type="content" source="images/visualization-linechart/line-chart.png" alt-text="Screenshot of line chart visualization output." lightbox="images/visualization-linechart/line-chart.png":::

::: zone-end

::: zone pivot="azuremonitor"

This visualization isn't supported in Azure Monitor.

::: zone-end