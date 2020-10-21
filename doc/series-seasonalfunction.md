---
title: series_seasonal() - Azure Data Explorer
description: This article describes series_seasonal() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# series_seasonal()

Calculates the seasonal component of a series, according to the detected or given seasonal period.

## Syntax

`series_seasonal(`*series* `[,` *period*`])`

## Arguments

* *series*: Input numeric dynamic array
* *period* (optional): Integer number of bins in each seasonal period, possible values:
    *  -1 (default): Autodetects the period by using [series_periods_detect()](series-periods-detectfunction.md) with a threshold of *0.7*. Returns zeroes if seasonality isn't detected
    * Positive integer: Used as the period for the seasonal component
    * Any other value: Ignores seasonality and returns a series of zeroes

## Returns

Dynamic array of the same length as the *series* input that contains the calculated seasonal component of the series. The seasonal component is calculated as the *median* of all the values that correspond to the location of the bin, across the periods.

## Examples

### Auto detect the period

In the following example, the series' period is automatically detected. The first series' period is detected to be six bins and the second five bins. The third series' period is too short to be detected and returns a series of zeroes. 
See the next example on [how to force the period](#force-a-period).

<!-- csl: https://help.kusto.windows.net:443/Samples -->
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

### Force a period

In this example, the series' period is too short to be detected by [series_periods_detect()](series-periods-detectfunction.md), so we explicitly force the period to get the seasonal pattern.

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
print s=dynamic([1,3,5,1,3,5,2,4,6]) 
| union (print s=dynamic([1,3,5,2,4,6,1,3,5,2,4,6]))
| extend s_seasonal = series_seasonal(s,3)
```

|s|s_seasonal|
|---|---|
|[1,3,5,1,3,5,2,4,6]|[1.0,3.0,5.0,1.0,3.0,5.0,1.0,3.0,5.0]|
|[1,3,5,2,4,6,1,3,5,2,4,6]|[1.5,3.5,5.5,1.5,3.5,5.5,1.5,3.5,5.5,1.5,3.5,5.5]|
 
## Next steps

* [series_periods_detect()](series-periods-detectfunction.md)
* [series_periods_validate()](series-periods-validatefunction.md)
