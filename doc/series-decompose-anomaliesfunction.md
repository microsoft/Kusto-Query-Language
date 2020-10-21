---
title: series_decompose_anomalies() - Azure Data Explorer
description: This article describes series_decompose_anomalies() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 08/28/2019
---
# series_decompose_anomalies()

Anomaly Detection is based on series decomposition.
For more information, see [series_decompose()](series-decomposefunction.md).

The function takes an expression containing a series (dynamic numerical array) as input, and extracts anomalous points with scores.

## Syntax

`series_decompose_anomalies (`*Series* `[, ` *Threshold*`,` *Seasonality*`,` *Trend*`, ` *Test_points*`, ` *AD_method*`,` *Seasonality_threshold* `])`

## Arguments

* *Series*: Dynamic array cell that is an array of numeric values, typically the resulting output of [make-series](make-seriesoperator.md) or [make_list](makelist-aggfunction.md) operators
* *Threshold*: Anomaly threshold, default 1.5 (k value) for detecting mild or stronger anomalies
* *Seasonality*: An integer controlling the seasonal analysis, containing either
    * -1: Autodetect seasonality (using [series_periods_detect](series-periods-detectfunction.md)) [default]
    * 0: No seasonality (that is, skip extracting this component)
    * period: Positive integer, specifying the expected period in number of bins unit. For example, if the series is in one hour bins, a weekly period is 168 bins
* *Trend*: A string controlling the trend analysis, containing either
    * "avg": Define trend component as average of the series [default]
    * "none": No trend, skip extracting this component
    * "linefit": Extract trend component using linear regression
* *Test_points*: 0 [default] or a positive integer, that specifies the number of points at the end of the series to exclude from the learning (regression) process. This parameter should be set for forecasting purposes
* *AD_method*: A string controlling the anomaly detection method on the residual time series, containing one of:
    * “ctukey”: [Tukey’s fence test](https://en.wikipedia.org/wiki/Outlier#Tukey's_fences) with custom 10th-90th percentile range [default]
    * “tukey”: [Tukey’s fence test](https://en.wikipedia.org/wiki/Outlier#Tukey's_fences) with standard 25th-75th percentile range
For more information on residual time series, see [series_outliers](series-outliersfunction.md)
* *Seasonality_threshold*: The threshold for seasonality score when *Seasonality* is set to autodetect. The default score threshold is `0.6`. For more information, see [series_periods_detect](series-periods-detectfunction.md)

## Returns

 The function returns the following respective series:

* `ad_flag`: A ternary series containing (+1, -1, 0) marking up/down/no anomaly respectively
* `ad_score`: Anomaly score
* `baseline`: The predicted value of the series, according to the decomposition

## The algorithm

This function follows these steps:
1. Calls [series_decompose()](series-decomposefunction.md) with the respective parameters, to create the baseline and residuals series.
1. Calculates ad_score series by applying [series_outliers()](series-outliersfunction.md) with the chosen anomaly detection method on the residuals series.
1. Calculates the ad_flag series by applying the threshold on the ad_score to mark up/down/no anomaly respectively.
 
## Examples

### Detect anomalies in weekly seasonality

In the following example, generate a series with weekly seasonality, and then add some outliers to it. `series_decompose_anomalies` autodetects the seasonality and generates a baseline that captures the repetitive pattern. The outliers you added can be clearly spotted in the ad_score component.

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
let ts=range t from 1 to 24*7*5 step 1 
| extend Timestamp = datetime(2018-03-01 05:00) + 1h * t 
| extend y = 2*rand() + iff((t/24)%7>=5, 10.0, 15.0) - (((t%24)/10)*((t%24)/10)) // generate a series with weekly seasonality
| extend y=iff(t==150 or t==200 or t==780, y-8.0, y) // add some dip outliers
| extend y=iff(t==300 or t==400 or t==600, y+8.0, y) // add some spike outliers
| summarize Timestamp=make_list(Timestamp, 10000),y=make_list(y, 10000);
ts 
| extend series_decompose_anomalies(y)
| render timechart  
```

:::image type="content" source="images/series-decompose-anomaliesfunction/weekly-seasonality-outliers.png" alt-text="Weekly seasonality showing baseline and outliers" border="false":::

### Detect anomalies in weekly seasonality with trend

In this example, add a trend to the series from the previous example. First, run `series_decompose_anomalies` with the default parameters in which the trend `avg` default value only takes the average and doesn't compute the trend. The generated baseline doesn't contain the trend and is less exact, compared to the previous example. Consequently, some of the outliers you inserted in the data aren't detected because of the higher variance.

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
let ts=range t from 1 to 24*7*5 step 1 
| extend Timestamp = datetime(2018-03-01 05:00) + 1h * t 
| extend y = 2*rand() + iff((t/24)%7>=5, 5.0, 15.0) - (((t%24)/10)*((t%24)/10)) + t/72.0 // generate a series with weekly seasonality and ongoing trend
| extend y=iff(t==150 or t==200 or t==780, y-8.0, y) // add some dip outliers
| extend y=iff(t==300 or t==400 or t==600, y+8.0, y) // add some spike outliers
| summarize Timestamp=make_list(Timestamp, 10000),y=make_list(y, 10000);
ts 
| extend series_decompose_anomalies(y)
| extend series_decompose_anomalies_y_ad_flag = 
series_multiply(10, series_decompose_anomalies_y_ad_flag) // multiply by 10 for visualization purposes
| render timechart
```

:::image type="content" source="images/series-decompose-anomaliesfunction/weekly-seasonality-outliers-with-trend.png" alt-text="Weekly seasonality outliers with trend" border="false":::

Next, run the same example, but since you're expecting a trend in the series, specify `linefit` in the trend parameter. You can see that the baseline is much closer to the input series. All the inserted outliers are detected, and also some false positives. See the next example on tweaking the threshold.

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
let ts=range t from 1 to 24*7*5 step 1 
| extend Timestamp = datetime(2018-03-01 05:00) + 1h * t 
| extend y = 2*rand() + iff((t/24)%7>=5, 5.0, 15.0) - (((t%24)/10)*((t%24)/10)) + t/72.0 // generate a series with weekly seasonality and ongoing trend
| extend y=iff(t==150 or t==200 or t==780, y-8.0, y) // add some dip outliers
| extend y=iff(t==300 or t==400 or t==600, y+8.0, y) // add some spike outliers
| summarize Timestamp=make_list(Timestamp, 10000),y=make_list(y, 10000);
ts 
| extend series_decompose_anomalies(y, 1.5, -1, 'linefit')
| extend series_decompose_anomalies_y_ad_flag = 
series_multiply(10, series_decompose_anomalies_y_ad_flag) // multiply by 10 for visualization purposes
| render timechart  
```

:::image type="content" source="images/series-decompose-anomaliesfunction/weekly-seasonality-linefit-trend.png" alt-text="Weekly seasonality anomalies with linefit trend" border="false":::

### Tweak the anomaly detection threshold

A few noisy points were detected as anomalies in the previous example. Now increase the anomaly detection threshold from a default of 1.5 to 2.5. Use this interpercentile range, so that only stronger anomalies are detected. Now, only the outliers you inserted in the data, will be detected.

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
let ts=range t from 1 to 24*7*5 step 1 
| extend Timestamp = datetime(2018-03-01 05:00) + 1h * t 
| extend y = 2*rand() + iff((t/24)%7>=5, 5.0, 15.0) - (((t%24)/10)*((t%24)/10)) + t/72.0 // generate a series with weekly seasonality and onlgoing trend
| extend y=iff(t==150 or t==200 or t==780, y-8.0, y) // add some dip outliers
| extend y=iff(t==300 or t==400 or t==600, y+8.0, y) // add some spike outliers
| summarize Timestamp=make_list(Timestamp, 10000),y=make_list(y, 10000);
ts 
| extend series_decompose_anomalies(y, 2.5, -1, 'linefit')
| extend series_decompose_anomalies_y_ad_flag = 
series_multiply(10, series_decompose_anomalies_y_ad_flag) // multiply by 10 for visualization purposes
| render timechart  
```

:::image type="content" source="images/series-decompose-anomaliesfunction/weekly-seasonality-higher-threshold.png" alt-text="Weekly series anomalies with higher anomaly threshold" border="false":::
