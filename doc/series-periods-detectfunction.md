---
title: series_periods_detect() - Azure Data Explorer
description: This article describes series_periods_detect() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/19/2019
---
# series_periods_detect()

Finds the most significant periods that exist in a time series.  

Often, a metric measuring an application’s traffic, is characterized by two significant periods: a weekly and a daily. 
The function `series_periods_detect()` detects these two dominant periods in a time series.  
The function takes as input:
* A column containing a dynamic array of time series. Typically, the column is the resulting output of [make-series](make-seriesoperator.md) operator.
* Two `real` numbers defining the minimal and maximal period size, the number of bins to search for. For example, for a 1h bin, the size of a daily period would be 24. 
* A `long` number defining the total number of periods for the function to search. 

The function outputs two columns:
* *periods*: A dynamic array containing the periods that have been found, in units of the bin size, ordered by their scores.
* *scores*: A dynamic array containing values between 0 and 1. Each array measures the significance of a period in its respective position in the *periods* array.
 
## Syntax

`series_periods_detect(`*x*`,` *min_period*`,` *max_period*`,` *num_periods*`)`

## Arguments

* *x*: Dynamic array scalar expression that is an array of numeric values, typically the resulting output of [make-series](make-seriesoperator.md) or [make_list](makelist-aggfunction.md) operators.
* *min_period*: A `real` number specifying the minimal period to search for.
* *max_period*: A `real` number specifying the maximal period to search for.
* *num_periods*: A `long` number specifying the maximum required number of periods. This number will be the length of the output dynamic arrays.

> [!IMPORTANT]
> * The algorithm can detect periods containing at least 4 points and at most half of the series length. 
>
> * Set the *min_period* a little below, and *max_period* a little above, the periods you expect to find in the time series. For example, if you have an hourly aggregated signal, and you look for both daily and weekly periods (24 and 168 hours respectively), you can set *min_period*=0.8\*24, *max_period*=1.2\*168, and leave 20% margins around these periods.
>
> * The input time series must be regular. That is, aggregated in constant bins, which is always the case if it has been created using [make-series](make-seriesoperator.md). Otherwise, the output is meaningless.

## Example

The following query embeds a snapshot of a month of an application’s traffic, aggregated twice a day. The bin size is 12 hours.

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
print y=dynamic([80,139,87,110,68,54,50,51,53,133,86,141,97,156,94,149,95,140,77,61,50,54,47,133,72,152,94,148,105,162,101,160,87,63,53,55,54,151,103,189,108,183,113,175,113,178,90,71,62,62,65,165,109,181,115,182,121,178,114,170])
| project x=range(1, array_length(y), 1), y  
| render linechart 
```

:::image type="content" source="images/series-periods/series-periods.png" alt-text="Series periods":::

Running `series_periods_detect()` on this series, results in the weekly period, 14 points long.

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
print y=dynamic([80,139,87,110,68,54,50,51,53,133,86,141,97,156,94,149,95,140,77,61,50,54,47,133,72,152,94,148,105,162,101,160,87,63,53,55,54,151,103,189,108,183,113,175,113,178,90,71,62,62,65,165,109,181,115,182,121,178,114,170])
| project x=range(1, array_length(y), 1), y  
| project series_periods_detect(y, 0.0, 50.0, 2)
```

| series\_periods\_detect\_y\_periods  | series\_periods\_detect\_y\_periods\_scores |
|-------------|-------------------|
| [14.0, 0.0] | [0.84, 0.0]  |


> [!NOTE] 
> The daily period that can be also seen in the chart wasn't found becasue the sampling is too coarse (12h bin size), so a daily period of 2 bins is below the minimum period size of 4 points, required by the algorithm.
