---
title: render operator - Azure Data Explorer
description: This article describes render operator in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 08/09/2022
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# render operator

Instructs the user agent to render the results of the query in a particular way.

```kusto
range x from 0.0 to 2*pi() step 0.01 | extend y=sin(x) | render linechart
```

> [!NOTE]
>
> * The render operator should be the last operator in the query, and used only with queries that produce a single tabular data stream result.
> * The render operator does not modify data. It injects an annotation ("Visualization") into the result's extended properties. The annotation contains the information provided by the operator in the query.
> * The interpretation of the visualization information is done by the user agent. Different agents (such as Kusto.Explorer,Kusto.WebExplorer) might support different visualizations.

> [!TIP]
>
> * Use `where`, `summarize` and `top` to limit the volume that you display.
> * Sort the data to define the order of the x-axis.
> * User agents are free to "guess" the value of properties that are not specified
  by the query. In particular, having "uninteresting" columns in the schema of
  the result might translate into them guessing wrong. Try projecting-away such
  columns when that happens.

## Syntax

*T* `|` `render` *Visualization* [`with` `(` *PropertyName* `=` *PropertyValue* [`,` ...] `)`]

## Arguments

* *Visualization* indicates the kind of visualization to use. The supported values are:

::: zone pivot="azuredataexplorer"

|*Visualization*     |Description|Example|
|--------------------|---|---|
| `anomalychart`     | Similar to timechart, but [highlights anomalies](./samples.md#get-more-from-your-data-by-using-kusto-with-machine-learning) using [series_decompose_anomalies](./series-decompose-anomaliesfunction.md) function. | **[**Click to run sample query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA3WR3W7CMAyF73mKI25KpRbaTmjSUJ8CpF1WoXVptPxUifmb9vBLoGO7GFeR7ePv2I4ihpamYdToBBNLTYuqKF/zosyLdbqZqagQl/8UVV68oKreimLSdVFUDZtZR9o2WnxQ48lJ8tXsCzHM7yHMUdfidFiEN4U12AXoloUe0Turp4nYTsaeaYzs/RVedgis80CObkFdI9ltywTAagV4UtQyRKiZgyLEaTGZ9taFQqtIGHI4SX8USn4KltYEJF2YTIeFMFaHPPkMvrWOMuxFoEpDaVjujmo6aq0erafmIY+7ZCiX6wx5mSGJHb3kJA1sF8jB8q69toNwjLPkYfGTseqoja//eLNkRXXyTnuIcVyCneh72cL2YQdtDQ8ZHvIkDcsfPWH+3AvPvObx0FMXD/RLhfDYW9VhtNKwj/8U69M1b2S//AbRUQMWQQIAAA==)** |
| `areachart`        | Area graph. First column is the x-axis and should be a numeric column. Other numeric columns are y-axes. | **[**Click to run sample query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0tJzc2PL04tykwtNuKqUUitKEnNS1GACMSnZZbEG+Vk5qUWa1Rq6iCLggSBYkAdRUD1qUUKiUWpickZiUUlCgrlmSUZGhXJ+TmluXm2FZoApaRQYmIAAAA=)** |
| `barchart`         | First column is the x-axis and can be text, datetime or numeric. Other columns are numeric, displayed as horizontal strips.|  **[**Click to run sample query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5lIAghqF4tLc3MSizKpUhVSQcHxyfmleiS2Y1NBUSKpUCC5JLEmFKi7PSC1CUahgp2BoAJUsKMrPSk0ugWjQQVYFVVCUmpeSWqSQlFiUnJFYVAIAB5xR2owAAAA=)** |
| `card`             | First result record is treated as set of scalar values and shows as a card. |  **[**Click to run sample query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0tJzc2PL04tykwtNuKqUUitKEnNS1GACMSnZZbEG+Vk5qUWa1Rq6iCLggSBYkAdRUD1qUUKCsmJRSkKQFCeWZKhUZGcn1Oam2dboQkA5CRu0GAAAAA=)** |
| `columnchart`      | Like `barchart` with vertical strips instead of horizontal strips.|  **[**Click to run sample query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5lIAghqF4tLc3MSizKpUhVSQcHxyfmleiS2Y1NBUSKpUCC5JLEmFKi7PSC1CUahgp2BoAJUsKMrPSk0ugWjQQVYFVVCUmpeSWqSQnJ9TmpuXnJFYVAIAJOFS3Y8AAAA=)** |
| `ladderchart`      | Last two columns are the x-axis, other columns are y-axis.| |
| `linechart`        | Line graph. First column is x-axis, and should be a numeric column. Other numeric columns are y-axes. |  **[**Click to run sample query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0tJzc2PL04tykwtNuKqUUitKEnNS1GACMSnZZbEG+Vk5qUWa1Rq6iCLggSBYkAdRUD1qUUKIIHkjMSiEoXyzJIMjYrk/JzS3DzbCk0AUIIJ02EAAAA=)** |
| `piechart`         | First column is color-axis, second column is numeric. |  **[**Click to run sample query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5uWqUSguzc1NLMqsSlUoLkksSU3OL80rsQWTGpoKSZUKwSBRsML8ohKQAEKZAkg4JzM3s0TB0ADELkrNS0ktUijITE3OSASqLsksyUm1VfKtVAjITFVwBovBjFQCADspGXyIAAAA)** |
| `pivotchart`       | Displays a pivot table and chart. User can interactively select data, columns, rows and various chart types. |   |
| `scatterchart`     | Points graph. First column is x-axis and should be a numeric column. Other numeric columns are y-axes. |  **[**Click to run sample query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0tJzc2PL04tykwtNuKqUUitKEnNS1GACMSnZZbEG+Vk5qUWa1Rq6iCLggSBYkAdRUD1qUUKCsXJiSUlqUXJGYlFJQoK5ZklGRoVyfk5pbl5thWaAI8A701mAAAA)** |
| `stackedareachart` | Stacked area graph. First column is x-axis, and should be a numeric column. Other numeric columns are y-axes. |  **[**Click to run sample query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA03LSwqAIBRG4XmruEODRs5bi4j+oeQDrjcyaPEZTZp+nOORq2ngiKanm9AFxdMHZotidIoFTV3z8tcXh42DRw8mamLdDm8Z1gXLQkRnlKC6q+nIZe3zAzEfsitrAAAA)** |
| `table`            | Default - results are shown as a table.|  **[**Click to run sample query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5lIAghqF4tLc3MSizKpUhVSQcHxyfmleiS2Y1NBUSKpUCC5JLEmFKi7PSC1CUahgp2BoAJUsKMrPSk0ugWjQQVYFVVCUmpeSWqRQkpiUkwoAW+Ur0IkAAAA=)** |
| `timechart`        | Line graph. First column is x-axis, and must be datetime. Other (numeric) columns are y-axes. There's one string column whose values are used to "group" the numeric columns and create different lines in the chart (further string columns are ignored). |   [**Click to run sample query**](https://dataexplorer.azure.com/clusters/help/databases/SampleMetrics?query=H4sIAAAAAAAAA2WOQQ6CMBBF95xiljQRaIq69wCu5AKlHaEJbc10KsF4eEtM3Lj9+f/913WXaSKcNKMFQhPJgok5MMQnEiw6MfRg9ZbABegleBcyYyrdFJfMLoZqIB3SPZJHe0MqsysyOZOqN6wzEsLgPCbW/gEj8ooYalv+uKS1kko18tiok2jbv7SXQhRKyt5rci/8qtUCxg1GF+of+ABn6fcqYbDFe6eYWRN/AGyZf0DgAAAA) |
| `timepivot`        | Interactive navigation over the events time-line (pivoting on time axis)|  |

> [!NOTE]
> The ladderchart, pivotchart, and timepivot visualizations can be used in Kusto.Explorer but are not available in the Azure Data Explorer web UI.

::: zone-end

::: zone pivot="azuremonitor"

|*Visualization*     |Description|
|--------------------|-|
| `areachart`        | Area graph. First column is the x-axis and should be a numeric column. Other numeric columns are y-axes. |
| `barchart`         | First column is the x-axis and can be text, datetime or numeric. Other columns are numeric, displayed as horizontal strips.|
| `columnchart`      | Like `barchart` with vertical strips instead of horizontal strips.|
| `piechart`         | First column is color-axis, second column is numeric. |
| `scatterchart`     | Points graph. First column is the x-axis and should be a numeric column. Other numeric columns are y-axes. |
| `table`            | Default - results are shown as a table.|
| `timechart`        | Line graph. First column is x-axis, and should be datetime. Other (numeric) columns are y-axes. There's one string column whose values are used to "group" the numeric columns and create different lines in the chart (further string columns are ignored).|

::: zone-end

* *PropertyName*/*PropertyValue* indicate additional information to use when rendering.
  All properties are optional. The supported properties are:

::: zone pivot="azuredataexplorer"

|*PropertyName*|*PropertyValue*                                                                   |
|--------------|----------------------------------------------------------------------------------|
|`accumulate`  |Whether the value of each measure gets added to all its predecessors. (`true` or `false`)|
|`kind`        |Further elaboration of the visualization kind. See below.                         |
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
|`ysplit`      |How to split multiple the visualization. See below.                               |
|`ytitle`      |The title of the y-axis (of type `string`).                                       |
|`anomalycolumns`|Property relevant only for `anomalychart`. Comma-delimited list of columns, which will be considered as anomaly series and displayed as points on the chart|

::: zone-end

::: zone pivot="azuremonitor"

|*PropertyName*|*PropertyValue*                                                                   |
|--------------|----------------------------------------------------------------------------------|
|`kind`        |Further elaboration of the visualization kind. See below.                         |
|`series`      |Comma-delimited list of columns whose combined per-record values define the series that record belongs to.|
|`title`       |The title of the visualization (of type `string`).                                |

::: zone-end

Some visualizations can be further elaborated by providing the `kind` property, such as:

|*Visualization*|`kind`             |Description                        |
|---------------|-------------------|-----------------------------------|
|`areachart`    |`default`          |Each "area" stands on its own.     |
|               |`unstacked`        |Same as `default`.                 |
|               |`stacked`          |Stack "areas" to the right.        |
|               |`stacked100`       |Stack "areas" to the right and stretch each one to the same width as the others.|
|`barchart`     |`default`          |Each "bar" stands on its own.      |
|               |`unstacked`        |Same as `default`.                 |
|               |`stacked`          |Stack "bars".                      |
|               |`stacked100`       |Stack "bars" and stretch each one to the same width as the others.|
|`columnchart`  |`default`          |Each "column" stands on its own.   |
|               |`unstacked`        |Same as `default`.                 |
|               |`stacked`          |Stack "columns" one atop the other.|
|               |`stacked100`       |Stack "columns" and stretch each one to the same height as the others.|
|`scatterchart` |`map`              |Expected columns are [Longitude, Latitude] or GeoJSON point. Series column is optional.|
|`piechart`     |`map`              |Expected columns are [Longitude, Latitude] or GeoJSON point, color-axis and numeric. Supported in Kusto Explorer desktop.|

::: zone pivot="azuredataexplorer"

Some visualizations support splitting into multiple y-axis values:

|`ysplit`  |Description                                                       |
|----------|------------------------------------------------------------------|
|`none`    |A single y-axis is displayed for all series data. (Default)       |
|`axes`    |A single chart is displayed with multiple y-axes (one per series).|
|`panels`  |One chart is rendered for each `ycolumn` value (up to some limit).|

> [!NOTE]
> The data model of the render operator looks at the tabular data as if it has
three kinds of columns:
>
> * The x axis column (indicated by the `xcolumn` property).
> * The series columns (any number of columns indicated by the `series` property.)
  For each record, the combined values of these columns defines a single series,
  and the chart has as many series as there are distinct combined values.
> * The y axis columns (any number of columns indicated by the `ycolumns`
  property).
  For each record, the series has as many measurements ("points" in the chart)
  as there are y-axis columns.

## How to render continuous data

Several visualizations are used for rendering sequences of values, for example, `linechart`, `timechart`, and `areachart`.
These visualizations have the following conceptual model:

* One column in the table represents the x-axis of the data. This column can be explicitly defined using the
    `xcolumn` property. If not defined, the user agent will pick the first column that is appropriate for the visualization.
  * For example: in the `timechart` visualization, the user agent will use the first `datetime` column.
  * If this column is of type `dynamic` and it holds an array, the individual values in the array will be treated as the values of the x-axis.
* One or more columns in the table represent one or more measures that vary by the x-axis.
    These columns can be explicitly defined using the `ycolumns` property. If not defined, the user agent will pick all columns that are appropriate for the visualization.
  * For example: in the `timechart` visualization, the user agent will use all columns with a numeric value that have not been specified otherwise.
  * If the x-axis is an array, the values of each y-axis should also be an array of a similar length, with each y-axis occurring in a single column.
* Zero or more columns in the table represent a unique set of dimensions that group together the measures. These columns can be specified by the `series` property, or the user agent will pick them automatically from the columns that are otherwise unspecified.

### Conceptual example

You have a set of anemometers (wind gauges) that measure the wind force, speed, and direction. These wind gauges are spread over a large geographic region.

The data from these measurements is found in a table with one record per measurement by each device, with columns for the timestamp (x-axis), measurements (three y-axes), and a longitude/latitude location (the series). 

Using the `render` operator and the `timechart` visualization, you can render time graphs of each measurement in a different panel over time, with each line representing a different device by its longitute/latitude position.



## Example: render linechart 

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
range x from -2 to 2 step 0.1
| extend sin = sin(x), cos = cos(x)
| extend x_sign = iif(x > 0, "x_pos", "x_neg")
| extend sum_sign = iif(sin + cos > 0, "sum_pos", "sum_neg")
| render linechart with  (ycolumns = sin, cos, series = x_sign, sum_sign)
```

[Rendering examples in the tutorial](./tutorial.md#displaychartortable)

[Anomaly detection](./samples.md#get-more-from-your-data-by-using-kusto-with-machine-learning)

::: zone-end

::: zone pivot="azuremonitor"

> [!NOTE]
> The data model of the render operator looks at the tabular data as if it has
three kinds of columns:
>
> * The x axis column (indicated by the `xcolumn` property).
> * The series columns (any number of columns indicated by the `series` property.)
> * The y axis columns (any number of columns indicated by the `ycolumns`
  property).
  For each record, the series has as many measurements ("points" in the chart)
  as there are y-axis columns.


## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
InsightsMetrics
| where Computer == "DC00.NA.contosohotels.com"
| where Namespace  == "Processor" and Name == "UtilizationPercentage"
| summarize avg(Val) by Computer, bin(TimeGenerated, 1h)
| render timechart
```

[Rendering examples in the tutorial](./tutorial.md?pivots=azuremonitor#display-a-chart-or-table-render-1)

::: zone-end
