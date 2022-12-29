---
title: series_abs() - Azure Data Explorer
description: This article describes series_abs() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 10/11/2021
---
# series_abs()

Calculates the element-wise absolute value of the numeric series input.

## Syntax

`series_abs(`*series*`)`

## Arguments

* *series*: Input numeric array, on which the absolute value function is applied. The argument must be a dynamic array. 

## Returns

Dynamic array of calculated absolute value. Any non-numeric element yields a `null` element value.

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print arr = dynamic([-6.5,0,8.2])
| extend arr_abs = series_abs(arr)
```

**Output**

|arr|arr_abs|
|---|---|
|[-6.5,0,8.2]|[6.5,0,8.2]|

