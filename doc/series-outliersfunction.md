---
title:  series_outliers()
description: Learn how to use the series_outliers() function to score anomaly points in a series.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/30/2023
---
# series_outliers()

Scores anomaly points in a series.

The function takes an expression with a dynamic numerical array as input, and generates a dynamic numeric array of the same length. Each value of the array indicates a score of a possible anomaly, using ["Tukey's test"](https://en.wikipedia.org/wiki/Outlier#Tukey's_fences). A value greater than 1.5 in the same element of the input indicates a rise or decline anomaly. A value less than -1.5, indicates a decline anomaly.

## Syntax

`series_outliers(`*series* [`,` *kind* ] [`,` *ignore_val* ] [`,` *min_percentile* ] [`,` *max_percentile* ]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *series* | dynamic | &check; | An array of numeric values.|
| *kind* | string | | The algorithm to use for outlier detection. The supported options are `"tukey"`, which is traditional "Tukey", and  `"ctukey"`, which is custom "Tukey". The default is `"ctukey"`.|
| *ignore_val* | int, long, or real | | A numeric value indicating the missing values in the series. The default is `double(`*null*`)`. The score of nulls and ignore values is set to `0`.|
| *min_percentile* | int, long, or real | | The minimum percentile to use to calculate the normal inter-quantile range. The default is 10. The value must be in the range `[2.0, 98.0]`. This parameter is only relevant for the `"ctukey"` *kind*.|
| *max_percentile* | int, long, or real | | The maximum percentile to use to calculate the normal inter-quantile range. The default is 90. The value must be in the range `[2.0, 98.0]`. This parameter is only relevant for the `"ctukey"` *kind*.|

The following table describes differences between `"tukey"` and `"ctukey"`:

| Algorithm | Default quantile range | Supports custom quantile range |
|-----------|----------------------- |--------------------------------|
| `"tukey"` | 25% / 75%              | No                             |
| `"ctukey"`| 10% / 90%              | Yes                            |

> [!TIP]
> The best way to use this function is to apply it to the results of the [make-series](make-seriesoperator.md) operator.

## Example

A time series with some noise creates outliers. If you would like to replace those outliers (noise) with the average value, use series_outliers() to detect the outliers, and then replace them.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA2VR0XKCQAx89yv2ESpWdMYZH0p/hblKwGs5jslFenb68Y0gakdeyG6ySW7DpmsIETV7hw3EY5PnCEK9osUvKAp1Fc6FreskoiiwzeEZY7jPMy3HC9h0VZJieUFLJLscK8QUa2yfC5Reo6GO2AjBIBjXt4RAbCng28oR/iStJQ4wgljoQFVrsM91oXByzrD90aULZ76obG2QJKbZtcMDeU7vD5iSZRAjIZmASuZBxTU947lC9W5YUezHBS7uyLknXyet75rbyAe+8qePlh46P+e0Z8/+kw6ygH4xG39TpymetSWT8wNVo/e3hu/YvO4uJ7gxb1gplf17Y3kFZmjmxGg8U9+aA93Fo+Fy1EsMepGGdD1Ww4jR2o4OR8PyB1kERXkkAgAA" target="_blank">Run the query</a>

```kusto
range x from 1 to 100 step 1 
| extend y=iff(x == 20 or x == 80, 10 * rand() + 10 + (50 - x) / 2, 10 * rand() + 10) // generate a sample series with outliers at x=20 and x=80
| summarize x=make_list(x), series=make_list(y)
| extend series_stats(series), outliers=series_outliers(series)
| mv-expand x to typeof(long), series to typeof(double), outliers to typeof(double)
| project
    x,
    series,
    outliers_removed=iff(outliers > 1.5 or outliers < -1.5, series_stats_series_avg, series) // replace outliers with the average
| render linechart
```

:::image type="content" source="images/series-outliersfunction/series-outliers.png" alt-text="Series outliers." border="false":::
