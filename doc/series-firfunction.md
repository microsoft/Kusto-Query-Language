---
title: series_fir() - Azure Data Explorer
description: This article describes series_fir() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/19/2019
---
# series_fir()

Applies a Finite Impulse Response (FIR) filter on a series.  

The function takes an expression containing a dynamic numerical array as input and applies a [Finite Impulse Response](https://en.wikipedia.org/wiki/Finite_impulse_response) filter. By specifying the `filter` coefficients, it can be used for calculating a moving average, smoothing, change-detection, and many more use cases. The function takes the column containing the dynamic array and a static dynamic array of the filter's coefficients as input, and applies the filter on the column. It outputs a new dynamic array column, containing the filtered output.  

## Syntax

`series_fir(`*x*`,` *filter* [`,` *normalize*[`,` *center*]]`)`

## Arguments

* *x*: Dynamic array cell of numeric values. Typically the resulting output of [make-series](make-seriesoperator.md) or [make_list](makelist-aggfunction.md) operators.
* *filter*: A constant expression containing the coefficients of the filter (stored as a dynamic array of numeric values).
* *normalize*: Optional Boolean value indicating whether the filter should be normalized. That is, divided by the sum of the coefficients. If filter contains negative values, then *normalize* must be specified as `false`, otherwise result will be `null`. If not specified, then a default value of *normalize* is assumed, depending on the presence of negative values in the *filter*. If *filter* contains at least one negative value, then *normalize* is assumed to be `false`.  
Normalization is a convenient way to make sure that the sum of the coefficients is 1. Then the filter doesn't amplify or attenuate the series. For example, the moving average of four bins could be specified by *filter*=[1,1,1,1] and *normalized*=true, which is easier than typing [0.25,0.25.0.25,0.25].
* *center*: An optional Boolean value that indicates whether the filter is applied symmetrically on a time window before and after the current point, or on a time window from the current point backwards. By default, center is false, which fits the scenario of streaming data, where we can only apply the filter on the current and older points. However, for ad-hoc processing you can set it to `true`, keeping it synchronized with the time series. See examples below. This parameter controls the filter’s [group delay](https://en.wikipedia.org/wiki/Group_delay_and_phase_delay).

## Examples

* Calculate a moving average of five points by setting *filter*=[1,1,1,1,1] and *normalize*=`true` (default). Note the effect of *center*=`false` (default) vs. `true`:

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
range t from bin(now(), 1h)-23h to bin(now(), 1h) step 1h
| summarize t=make_list(t)
| project id='TS', val=dynamic([0,0,0,0,0,0,0,0,0,10,20,40,100,40,20,10,0,0,0,0,0,0,0,0]), t
| extend 5h_MovingAvg=series_fir(val, dynamic([1,1,1,1,1])),
         5h_MovingAvg_centered=series_fir(val, dynamic([1,1,1,1,1]), true, true)
| render timechart
```

This query returns:  
*5h_MovingAvg*: Five points moving average filter. The spike is smoothed and its peak shifted by (5-1)/2 = 2h.  
*5h_MovingAvg_centered*: Same, but by setting `center=true`, the peak stays in its original location.

:::image type="content" source="images/series-firfunction/series-fir.png" alt-text="Series fir" border="false":::

* To calculate the difference between a point and its preceding one, set *filter*=[1,-1].

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
range t from bin(now(), 1h)-11h to bin(now(), 1h) step 1h
| summarize t=make_list(t)
| project id='TS',t,value=dynamic([0,0,0,0,2,2,2,2,3,3,3,3])
| extend diff=series_fir(value, dynamic([1,-1]), false, false)
| render timechart
```

:::image type="content" source="images/series-firfunction/series-fir2.png" alt-text="Series fir 2" border="false":::
