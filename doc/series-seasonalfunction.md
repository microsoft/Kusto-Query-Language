---
title: series_seasonal() - Azure Data Explorer | Microsoft Docs
description: This article describes series_seasonal() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# series_seasonal()

Calculates the seasonal component of a series according to the detected or given seasonal period.

**Syntax**

`series_seasonal(`*series* `[,` *period*`])`

**Arguments**

* *series*: Input numeric dynamic array
* *period* (optional): Integer number of bins in each seasonal period, possible values:
    *  -1 (default): auto detect the period using [series_periods_detect()](series-periods-detectfunction.md) with a threshold of *0.7*, returns zeroes if seasonality is not detected
    * positive integer: will be used as the period for the seasonal component
    * any other value: ignore seasonality and return a series of zeroes

**Returns**

Dynamic array of the same length as the *series* input containing the calculated seasonal component of the series. The seasonal component is calculated as the *median* of all the values corresponding to the location of the bin across the periods.

**See also:**

* [series_periods_detect()](series-periods-detectfunction.md)
* [series_periods_validate()](series-periods-validatefunction.md)

**Examples**

**1. Auto detect the period**

In the following example the series' period is automatically detected, the first series' period is detected to be 6 bins and the second 5 bins, the third series' period is too short to be detected and returns a series of zeroes (see next example on how to force the period).

```kusto
print s=dynamic([2,5,3,4,3,2,1,2,3,4,3,2,1,2,3,4,3,2,1,2,3,4,3,2,1])
| union (print s=dynamic([8,12,14,12,10,10,12,14,12,10,10,12,14,12,10,10,12,14,12,10]))
| union (print s=dynamic([1,3,5,2,4,6,1,3,5,2,4,6]))
| extend s_seasonal = series_seasonal(s)
```
|s|s_seasonal|
|---|---|
|[2,5,3,4,3,2,1,2,3,4,3,2,1,2,3,4,3,2,1,2,3,4,3,2,1]|[1.0,2.0,3.0,4.0,3.0,2.0,1.0,2.0,3.0,4.0,3.0,2.0,1.0,2.0,3.0,4.0,3.0,2.0,1.0,2.0,3.0,4.0,3.0,2.0,1.0]|
|[8,12,14,12,10,10,12,14,12,10,10,12,14,12,10,10,12,14,12,10]|[10.0,12.0,14.0,12.0,10.0,10.0,12.0,14.0,12.0,10.0,10.0,12.0,14.0,12.0,10.0,10.0,12.0,14.0,12.0,10.0]|
|[1,3,5,2,4,6,1,3,5,2,4,6]|[0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0]|



**2. Force a period**

In the following example the series' period is too short to be detected by [series_periods_detect()](series-periods-detectfunction.md) so we force the period explicitly to get the seasonal pattern.

```kusto
print s=dynamic([1,3,5,1,3,5,2,4,6]) 
| union (print s=dynamic([1,3,5,2,4,6,1,3,5,2,4,6]))
| extend s_seasonal = series_seasonal(s,3)
```
|s|s_seasonal|
|---|---|
|[1,3,5,1,3,5,2,4,6]|[1.0,3.0,5.0,1.0,3.0,5.0,1.0,3.0,5.0]|
|[1,3,5,2,4,6,1,3,5,2,4,6]|[1.5,3.5,5.5,1.5,3.5,5.5,1.5,3.5,5.5,1.5,3.5,5.5]|