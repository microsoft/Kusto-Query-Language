---
title: series_log() - Azure Data Explorer
description: This article describes series_log() in Azure Data Explorer.
ms.reviewer: afridman
ms.topic: reference
ms.date: 11/07/2022
---
# series_log()

Calculates the element-wise natural logarithm function (base-e) of the numeric series input.

## Syntax

`series_log(`*series*`)`

## Arguments

* *series*: Input numeric array, on which the natural logarithm function is applied. The argument must be a dynamic array.

## Returns

Dynamic array of the calculated natural logarithm function. Any non-numeric element yields a `null` element value.

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print s = dynamic([1,2,3])
| extend s_log = series_log(s)
```

**Output**

|s|s_log|
|---|---|
|[1,2,3]|[0.0,0.69314718055994529,1.0986122886681098]|