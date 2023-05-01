---
title: Pie chart visualization - Azure Data Explorer
description: This article describes the pie chart visualization in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/26/2023
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# Pie chart

The pie chart visual needs a minimum of two columns in the query result. By default, the first column is used as the color axis. This column can contain text, datetime, or numeric data types. Other columns will be used to determine the size of each slice and contain numeric data types. Pie charts are used for presenting a composition of categories and their proportions out of a total.

::: zone pivot="azuredataexplorer"

The pie chart visual can also be used in the context of [Geospatial visualizations](geospatial-visualizations.md).

::: zone-end


> [!NOTE]
> This visualization can only be used in the context of the [render operator](renderoperator.md).

## Syntax

*T* `|` `render` `piechart` [`with` `(`*propertyName* `=` *propertyValue* [`,` ...]`)`]

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
| *T* | string | &check; | Input table name.
| *propertyName*, *propertyValue* | string | | A comma-separated list of key-value property pairs. See [supported properties](#supported-properties).|

### Supported properties

All properties are optional.

::: zone pivot="azuredataexplorer"

|*PropertyName*|*PropertyValue*                                                                   |
|--------------|----------------------------------------------------------------------------------|
|`accumulate`  |Whether the value of each measure gets added to all its predecessors. (`true` or `false`)|
|`kind`        |Further elaboration of the visualization kind.  For more information, see [`kind` property](#kind-property).                         |
|`legend`      |Whether to display a legend or not (`visible` or `hidden`).                       |
|`series`      |Comma-delimited list of columns whose combined per-record values define the series that record belongs to.|
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

| `kind` value | Description| 
|---|---|
| `map` | Expected columns are [Longitude, Latitude] or GeoJSON point, color-axis and numeric. Supported in Kusto Explorer desktop. For more information, see [Geospatial visualizations](geospatial-visualizations.md)

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0WNQQqFMBBD955icKU7PYBLT9ATVP+AA7aVaVQUD2/rB92EJDwSg6Cu39gjFhfF1TmrcjJFWPAYVo/u0aqm4SCT28wFRc4fRamdxQmobZJV9j9WWoTHySZ2F0wVBDN3pcmX9P98R8v6BgN8EaGKAAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize statecount=count() by State
| sort by statecount 
| limit 10
| render piechart with(title="Storm Events by State")
```

:::image type="content" source="images/visualization-piechart/pie-chart.png" alt-text="Screenshot of pie chart visualization output." lightbox="images/visualization-piechart/pie-chart.png":::