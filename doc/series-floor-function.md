---
title: series_floor() - Azure Data Explorer
description: This article describes series_floor() in Azure Data Explorer.
ms.reviewer: afridman
ms.topic: reference
ms.date: 11/07/2022
---
# series_floor()

Calculates the element-wise floor function of the numeric series input.

## Syntax

`series_floor(`*series*`)`

## Arguments

* *series*: Input numeric array, on which the floor function is applied. The argument must be a dynamic array.

## Returns

Dynamic array of the calculated floor function. Any non-numeric element yields a `null` element value.

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print s = dynamic([-1.5,1,2.5])
| extend s_floor = series_floor(s)
```

**Output**

|s|s_floor|
|---|---|
|[-1.5,1,2.5]|[-2.0,1.0,2.0]|