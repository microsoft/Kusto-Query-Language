---
title: series_outliers() - Azure Data Explorer
description: This article describes series_outliers() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/20/2019
---
# series_outliers()

Scores anomaly points in a series.

The function takes an expression with a dynamic numerical array as input, and generates a dynamic numeric array of the same length. Each value of the array indicates a score of a possible anomaly, using ["Tukey's test"](https://en.wikipedia.org/wiki/Outlier#Tukey's_fences). A value greater than 1.5 in the same element of the input indicates a rise or decline anomaly. A value less than -1.5, indicates a decline anomaly.

## Syntax

`series_outliers(`*x*`, `*kind*`, `*ignore_val*`, `*min_percentile*`, `*max_percentile*`)`

## Arguments

* *x*: Dynamic array cell that is an array of numeric values
* *kind*: Algorithm of outlier detection. Currently supports `"tukey"` (traditional "Tukey") and  `"ctukey"` (custom "Tukey"). Default is `"ctukey"`
* *ignore_val*: Numeric value indicating missing values in the series. Default is double(null). The score of nulls and ignore values is set to `0`
* *min_percentile*: For calculating the normal inter-quantile range. Default is 10, custom values supported are in range `[2.0, 98.0]` (`ctukey` only)
* *max_percentile*: same, default is 90, custom values supported are in range `[2.0, 98.0]` (ctukey only)

The following table describes differences between `"tukey"` and `"ctukey"`:

| Algorithm | Default quantile range | Supports custom quantile range |
|-----------|----------------------- |--------------------------------|
| `"tukey"` | 25% / 75%              | No                             |
| `"ctukey"`| 10% / 90%              | Yes                            |

> [!TIP]
> The best way to use this function is to apply it to the results of the [make-series](make-seriesoperator.md) operator.

## Example

A time series with some noise creates outliers. If you would like to replace those outliers (noise) with the average value, use series_outliers() to detect the outliers, and then replace them.

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
range x from 1 to 100 step 1 
| extend y=iff(x==20 or x==80, 10*rand()+10+(50-x)/2, 10*rand()+10) // generate a sample series with outliers at x=20 and x=80
| summarize x=make_list(x),series=make_list(y)
| extend series_stats(series), outliers=series_outliers(series)
| mv-expand x to typeof(long), series to typeof(double), outliers to typeof(double)
| project x, series , outliers_removed=iff(outliers > 1.5 or outliers < -1.5, series_stats_series_avg , series ) // replace outliers with the average
| render linechart
``` 

:::image type="content" source="images/series-outliersfunction/series-outliers.png" alt-text="Series outliers" border="false":::
