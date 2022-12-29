---
title: series_asin() - Azure Data Explorer
description: This article describes series_asin() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 10/11/2021
---
# series_asin()

Calculates the element-wise arcsine function of the numeric series input.

## Syntax

`series_asin(`*series*`)`

## Arguments

* *series*: Input numeric array, on which the arcsine function is applied. The argument must be a dynamic array. 

## Returns

Dynamic array of calculated arcsine function values. Any non-numeric element yields a `null` element value.

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print arr = dynamic([-1,0,1])
| extend arr_asin = series_asin(arr)
```

**Output**

|arr|arr_asin|
|---|---|
|[-6.5,0,8.2]|[1.5707963267948966,0.0,1.5707963267948966]|

