---
title:  Card visualization
description: This article describes the card visualization in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/24/2023
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors-all
---
# Card

::: zone pivot="azuredataexplorer, fabric"

The card visual only shows one element. If there are multiple columns and rows in the output, the first result record is treated as set of scalar values and shows as a card.

> [!NOTE]
> This visualization can only be used in the context of the [render operator](renderoperator.md).

## Syntax

*T* `|` `render` `card` [`with` `(`*propertyName* `=` *propertyValue* [`,` ...]`)`]

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
| *T* | string | &check; | Input table name.|
| *propertyName*, *propertyValue* | string | | A comma-separated list of key-value property pairs. See [supported properties](#supported-properties).|

### Supported properties

All properties are optional.
    
|*PropertyName*|*PropertyValue*                                                                   |
|--------------|----------------------------------------------------------------------------------|
|`title`       |The title of the visualization (of type `string`).                                |

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAy2LsQqDQBAFe7/icZX5CAuLJFxjocH+8BZd0D1ZV0Xw42OC1RQz01jS6bmR2JKd2AdSQmPBqChc6+u3r3zpECTiH32O+WdeY0rRXUOXVrGLShJJ0QWN2NkG5MY20l0uYEHL2rNwcI8vTweEO3QAAAA=" target="_blank">Run the query</a>

```kusto 
StormEvents
| where State=="VIRGINIA" and EventType=="Flood"
| count
| render card with (title="Floods in Virginia")
```

:::image type="content" source="images/card/card.png" alt-text="Screenshot of card visual." lightbox="images/card/card.png":::

::: zone-end

::: zone pivot="azuremonitor"

This visualization isn't supported in Azure Monitor.

::: zone-end