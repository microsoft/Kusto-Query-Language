---
title: Treemap visualization - Azure Data Explorer
description: Learn how to use the treemap visualization to visualize data.
ms.reviewer: alexans
ms.topic: reference
ms.date: 05/21/2023
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors-all
---
# Treemap

::: zone pivot="azuredataexplorer"

Treemaps display hierarchical data as a set of nested rectangles. Each level of the hierarchy is represented by a colored rectangle (branch) containing smaller rectangles (leaves).

> [!NOTE]
>
> * This visualization can only be used in the context of the [render operator](renderoperator.md).
> * This visualization can be used in Kusto.Explorer but is not available in the Azure Data Explorer web UI.

## Syntax

*T* `|` `render` `treemap` [`with` `(`*propertyName* `=` *propertyValue* [`,` ...]`)`]

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
| *T* | string | &check; | Input table name.
| *propertyName*, *propertyValue* | string | | A comma-separated list of key-value property pairs. See [supported properties](#supported-properties).|

### Supported properties

All properties are optional.

|***PropertyName***|***PropertyValue***                                                                   |
|--------------|----------------------------------------------------------------------------------|
|`series`      |Comma-delimited list of columns whose combined per-record values define the series that record belongs to.|

## Example

```kusto
StormEvents
| summarize StormEvents=count() by EventType, State
| sort by StormEvents
| limit 30
| render treemap with(title="Storm Events by EventType and State")
```

:::image type="content" source="images/visualization-treemap/treemap.png" alt-text="Screenshot of treemap visualization output." lightbox="images/visualization-treemap/treemap.png":::
::: zone-end

::: zone pivot="azuremonitor, fabric"

This visualization isn't supported.

::: zone-end
