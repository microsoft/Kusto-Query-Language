---
title: series_fir() - Azure Data Explorer
description: Learn how to use the series_fir() function to apply a Finite Impulse Response (FIR) filter on a series.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/26/2023
---
# series_fir()

Applies a Finite Impulse Response (FIR) filter on a series.  

The function takes an expression containing a dynamic numerical array as input and applies a [Finite Impulse Response](https://en.wikipedia.org/wiki/Finite_impulse_response) filter. By specifying the `filter` coefficients, it can be used for calculating a moving average, smoothing, change-detection, and many more use cases. The function takes the column containing the dynamic array and a static dynamic array of the filter's coefficients as input, and applies the filter on the column. It outputs a new dynamic array column, containing the filtered output.  

## Syntax

`series_fir(`*series*`,` *filter* [`,` *normalize*[`,` *center*]]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *series* | dynamic | &check; | An array of numeric values.|
| *filter* | dynamic | &check; | An array of numeric values containing the coefficients of the filter.|
| *normalize* | bool | | Indicates whether the filter should be normalized. That is, divided by the sum of the coefficients. If filter contains negative values, then *normalize* must be specified as `false`, otherwise result will be `null`. If not specified, then a default value of `true` is assumed, depending on the presence of negative values in the *filter*. If *filter* contains at least one negative value, then *normalize* is assumed to be `false`.|
| *center* | bool | | Indicates whether the filter is applied symmetrically on a time window before and after the current point, or on a time window from the current point backwards. By default, center is `false`, which fits the scenario of streaming data so that we can only apply the filter on the current and older points. However, for ad-hoc processing you can set it to `true`, keeping it synchronized with the time series. See examples below. This parameter controls the filter’s [group delay](https://en.wikipedia.org/wiki/Group_delay_and_phase_delay).|

> [!TIP]
> Normalization is a convenient way to make sure that the sum of the coefficients is 1. When *normalized* is `true`, the filter doesn't amplify or attenuate the series. For example, the moving average of four bins could be specified by *filter*=[1,1,1,1] and *normalized*=`true`, which is simpler than typing [0.25,0.25.0.25,0.25].

## Returns

A new dynamic array column containing the filtered output.  

## Examples

* Calculate a moving average of five points by setting *filter*=[1,1,1,1,1] and *normalize*=`true` (default). Note the effect of *center*=`false` (default) vs. `true`:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/kvc9rf7q4d68qcw5sk2d6f.northeurope/databases/MyDatabase?query=H4sIAAAAAAAAA41QwWrDMAy9F/IPutUGF5Z0O+awD+hpu40Q3ERN1NV2kdW0G/v4uU1Wxlih5kk8S+jpIba+QxDYcHCwJq98OCptIO81LKBY9iDhbz0K7hPJZl8QD85Zps8kUTr7jvWOoijR596ewxYbyWaQHrXl/PVlbsbfYHdl++Gto0a9PRi4hTxFkeLxwidSTI3/Uelph5w94EnQt2Phqa9XYSDfPQ9dGZEJY70hVsmMgaubPIlfUekftd/DdYNekLG9W8WA8AHHfLkNJ1fIIOSw6S3LN0kcwQiHAQAA" target="_blank">Run the query</a>

```kusto
range t from bin(now(), 1h) - 23h to bin(now(), 1h) step 1h
| summarize t=make_list(t)
| project
    id='TS',
    val=dynamic([0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 20, 40, 100, 40, 20, 10, 0, 0, 0, 0, 0, 0, 0, 0]),
    t
| extend
    5h_MovingAvg=series_fir(val, dynamic([1, 1, 1, 1, 1])),
    5h_MovingAvg_centered=series_fir(val, dynamic([1, 1, 1, 1, 1]), true, true)
| render timechart
```

This query returns:  
*5h_MovingAvg*: Five points moving average filter. The spike is smoothed and its peak shifted by (5-1)/2 = 2h.  
*5h_MovingAvg_centered*: Same, but by setting `center=true`, the peak stays in its original location.

:::image type="content" source="images/series-firfunction/series-fir.png" alt-text="Series fir." border="false":::

* To calculate the difference between a point and its preceding one, set *filter*=[1,-1].

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/kvc9rf7q4d68qcw5sk2d6f.northeurope/databases/MyDatabase?query=H4sIAAAAAAAAA12O3QrCMAxG7wXfIXd20IHV6z2F3skYdUtddG1HmvmHD+8cMkQ48IWE7xC24YQg4Dh6OFJQId5UpsG0GeRgTAsS//dJsB+H5eIFafDeMj1HReHtBauOkijJPree4xlrAWqK1X630iAarrYbsGgewXqq1WGtYWbzw3amnFR4FwwNNORckZAJU+WI1WTTMOuMhtyU45fOdgm/MQl4rCODkMe6tSxvKkW3gPcAAAA=" target="_blank">Run the query</a>

```kusto
range t from bin(now(), 1h) - 11h to bin(now(), 1h) step 1h
| summarize t=make_list(t)
| project id='TS', t, value=dynamic([0, 0, 0, 0, 2, 2, 2, 2, 3, 3, 3, 3])
| extend diff=series_fir(value, dynamic([1, -1]), false, false)
| render timechart
```

:::image type="content" source="images/series-firfunction/series-fir2.png" alt-text="Series fir 2." border="false":::
