---
title:  series_ceiling()
description: Learn how to use the series_ceiling() function to calculate the element-wise ceiling function of the numeric series input.
ms.reviewer: afridman
ms.topic: reference
ms.date: 01/22/2023
---
# series_ceiling()

Calculates the element-wise ceiling function of the numeric series input.

## Syntax

`series_ceiling(`*series*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *series* | dynamic | &check; | An array of numeric values over which the ceiling function is applied. |

## Returns

Dynamic array of the calculated ceiling function. Any non-numeric element yields a `null` element value.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShWsFVIqcxLzM1M1ojWNdQz1THUMdIzjdXkqlFIrShJzUtRKI5PTs3MycxLByotTi3KTIULaBRrAgCQd2nZRgAAAA==" target="_blank">Run the query</a>

```kusto
print s = dynamic([-1.5,1,2.5])
| extend s_ceiling = series_ceiling(s)
```

**Output**

|s|s_ceiling|
|---|---|
|[-1.5,1,2.5]|[-1.0,1.0,3.0]|