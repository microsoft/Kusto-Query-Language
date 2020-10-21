---
title: series_stats() - Azure Data Explorer
description: This article describes series_stats() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/10/2020
---
# series_stats()

`series_stats()` returns statistics for a series in multiple columns.  

The `series_stats()` function takes a column containing dynamic numerical array as input and calculates the following columns:
* `min`: minimum value in the input array
* `min_idx`: first position of the minimum value in the input array
* `max`: maximum value in the input array
* `max_idx`: first position of the maximum value in the input array
* `avg`: average value of the input array
* `variance`: sample variance of input array
* `stdev`: sample standard deviation of the input array

> [!NOTE] 
> This function returns multiple columns so it can't be used as an argument for another function.

## Syntax

project `series_stats(`*x* `[,`*ignore_nonfinite*`])` or extend `series_stats(`*x*`)` 
Returns all above-mentioned columns with the following names: series_stats_x_min, series_stats_x_min_idx and etc.
 
project (m, mi)=`series_stats(`*x*`)` or extend (m, mi)=`series_stats(`*x*`)`
Returns the following columns: m (min) and mi (min_idx).

## Arguments

* *x*: Dynamic array cell, which is an array of numeric values. 
* *ignore_nonfinite*: Boolean (optional, default: `false`) flag that specifies whether to calculate the statistics while ignoring non-finite values (*null*, *NaN*, *inf*, etc.). If set to `false`, the returned values would be `null` if non-finite values are present in the array.

## Example

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
print x=dynamic([23,46,23,87,4,8,3,75,2,56,13,75,32,16,29]) 
| project series_stats(x)

```

|series_stats_x_min|series_stats_x_min_idx|series_stats_x_max|series_stats_x_max_idx|series_stats_x_avg|series_stats_x_stdev|series_stats_x_variance|
|---|---|---|---|---|---|---|
|2|8|87|3|32.8|28.5036338535483|812.457142857143|
