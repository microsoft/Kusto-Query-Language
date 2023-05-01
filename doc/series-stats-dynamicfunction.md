---
title: series_stats_dynamic() - Azure Data Explorer
description: Learn how to use the series_stats_dynamic() function to calculate the statistics for a series in a dynamic object.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/30/2023
---
# series_stats_dynamic()

Returns statistics for a series in a dynamic object.  

## Syntax

`series_stats_dynamic(`*series* [`,` *ignore_nonfinite* ]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *series* | dynamic | &check; | An array of numeric values.|
| *ignore_nonfinite* | bool | | Indicates whether to calculate the statistics while ignoring non-finite values, such as *null*, *NaN*, *inf*, and so on. The default is `false`, which returns `null` if non-finite values are present in the array.|

## Returns

A dynamic property bag object with the following content:

* `min`: The minimum value in the input array.
* `min_idx`: The first position of the minimum value in the input array.
* `max`: The maximum value in the input array.
* `max_idx`: The first position of the maximum value in the input array.
* `avg`: The average value of the input array.
* `variance`: The sample variance of input array.
* `stdev`: The sample standard deviation of the input array.
* `sum`: The sum of the values in the input array.
* `len`: The length of the input array.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUaiwTanMS8zNTNaINjLWUTAx01EA0RbmQDaQ0lEAcsxNgYI6CqZAOUMo1xjINwSptYzVVOCqUSgoys9KTS5RKC5JLCm2LU4tykwtjgdz4mHmV2gCANNsjChyAAAA" target="_blank">Run the query</a>

```kusto
print x=dynamic([23, 46, 23, 87, 4, 8, 3, 75, 2, 56, 13, 75, 32, 16, 29]) 
| project stats=series_stats_dynamic(x)
```

**Output**

|stats|
|---|
|{"min": 2.0, "min_idx": 8, "max": 87.0, "max_idx": 3, "avg": 32.8, "stdev": 28.503633853548269, "variance": 812.45714285714291, "sum": 492.0, "len": 15}|
