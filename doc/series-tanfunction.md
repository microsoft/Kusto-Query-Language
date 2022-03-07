---
title: series_tan() - Azure Data Explorer
description: This article describes series_tan() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 10/11/2021
---
# series_tan()

Calculates the element-wise tangent function of the numeric series input.

## Syntax

`series_tan(`*series*`)`

## Arguments

* *series*: Input numeric array, on which the tangent function is applied. The argument must be a dynamic array. 

## Returns

Dynamic array of calculated tangent function values. Any non-numeric element yields a `null` element value.

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print arr = dynamic([-1,0,1])
| extend arr_tan = series_tan(arr)
```

|arr|arr_tan|
|---|---|
|[-6.5,0,8.2]|[-1.5574077246549023,0.0,1.5574077246549023]|

