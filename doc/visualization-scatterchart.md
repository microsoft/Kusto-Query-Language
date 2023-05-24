---
title:  Scatter chart visualization
description: This article describes the scatter chart visualization in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/29/2023
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors-all
---
# Scatter chart

In a scatter chart visual, the first column is the x-axis and should be a numeric column. Other numeric columns are y-axes. Scatter plots are used to observe relationships between variables. The scatter chart visual can also be used in the context of [Geospatial visualizations](geospatial-visualizations.md).


> [!NOTE]
> This visualization can only be used in the context of the [render operator](renderoperator.md).

## Syntax

*T* `|` `render` `scatterchart` [`with` `(`*propertyName* `=` *propertyValue* [`,` ...]`)`]

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
| *T* | string | &check; | Input table name.|
| *propertyName*, *propertyValue* | string | | A comma-separated list of key-value property pairs. See [supported properties](#supported-properties).|

### Supported properties

All properties are optional.

::: zone pivot="azuredataexplorer, fabric"

|*PropertyName*|*PropertyValue*                                                                   |
|--------------|----------------------------------------------------------------------------------|
|`accumulate`  |Whether the value of each measure gets added to all its predecessors. (`true` or `false`)|
|`kind`        |Further elaboration of the visualization kind.  For more information, see [`kind` property](#kind-property).                         |
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

|*PropertyName*|*PropertyValue*                                                                   |
|--------------|----------------------------------------------------------------------------------|
|`kind`        |Further elaboration of the visualization kind. For more information, see [`kind` property](#kind-property).                        |
|`series`      |Comma-delimited list of columns whose combined per-record values define the series that record belongs to.|
|`title`       |The title of the visualization (of type `string`).                                |

::: zone-end


#### `kind` property

This visualization can be further elaborated by providing the `kind` property.
The supported values of this property are:

| `kind` value | Description |
|---|---|
|`map`              |Expected columns are [Longitude, Latitude] or GeoJSON point. Series column is optional. For more information, see [Geospatial visualizations](geospatial-visualizations.md). |

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0WOQQ6CMBBF955iwgoSPQI73JNwgpFOoNp2mukAYjy8hSjufvLf/3mdsvjrTEHT6Q1p8h7FvmhLZYMeB2qFI4mu1W2FTlEpc475MUVoOU4O1XJoUBE4HEAUvlOvF1zwvxIKhgRSj6ok/YiisFgdoXyqVUd1sZMQj9viDN/mZwFml4IskzY4E46GfFyP1hgK1QcXijj20QAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize sum(DamageProperty)by State
| lookup PopulationData on State
| project-away State
| render scatterchart with (xtitle="State population", title="Property damage by state", legend=hidden)
```

:::image type="content" source="images/visualization-scatterchart/scatter-chart.png" alt-text="Screenshot of scatter chart visualization output." lightbox="images/visualization-scatterchart/scatter-chart.png":::