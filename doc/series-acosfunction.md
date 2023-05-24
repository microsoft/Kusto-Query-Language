---
title:  series_acos()
description: Learn how to use the series_acos() function to calculate the element-wise arccosine function of the numeric series input.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/22/2023
---
# series_acos()

Calculates the element-wise arccosine function of the numeric series input.

## Syntax

`series_acos(`*series*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *series* | dynamic | &check; | An array of numeric values over which the arccosine function is applied. |

## Returns

Dynamic array of calculated arccosine function values. Any non-numeric element yields a `null` element value.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgsKlKwVUipzEvMzUzWiNY11DHQMYzV5KpRSK0oSc1LASmIT0zOLwaqKk4tykwtBvM0gMKaALhJgmxCAAAA" target="_blank">Run the query</a>

```kusto
print arr = dynamic([-1,0,1])
| extend arr_acos = series_acos(arr)
```

**Output**

|arr|arr_acos|
|---|---|
|[-6.5,0,8.2]|[3.1415926535897931,1.5707963267948966,0.0]|
