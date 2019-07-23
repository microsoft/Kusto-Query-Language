---
title: series_decompose_anomalies() - Azure Data Explorer | Microsoft Docs
description: This article describes series_decompose_anomalies() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 06/23/2019
---
# series_decompose_anomalies()

Anomaly Detection based on series decomposition (refer to [series_decompose()](series-decomposefunction.md)) 

Takes an expression containing a series (dynamic numerical array) as input and extract anomalous points with scores.

**Syntax**

`series_decompose_anomalies (`*Series*`, ` *Threshold*`,` *Seasonality*`,` *Trend*`, ` *Test_points*`, ` *AD_method*`)`

**Arguments**

* *Series*: Dynamic array cell which is an array of numeric values, typically the resulting output of [make-series](make-seriesoperator.md) or [make_list](makelist-aggfunction.md) operators
* *Threshold*: Anomaly threshold, default 1.5 (k value) for detecting mild or stronger anomalies
* *Seasonality*: An integer controlling the seasonal analysis, containing either
    * -1: autodetect seasonality (using [series_periods_detect](series-periods-detectfunction.md)) [default] 
    * 0: no seasonality (i.e. skip extracting this component)
    * period: positive integer, specifying the expected period in number of bins unit. For example, if the series is in 1h bins, a weekly period is 168 bins
* *Trend*: A string controlling the trend analysis, containing either    
    * "avg": define trend component as average of the series [default]
    * "none": no trend, skip extracting this component 
    * "linefit": extract trend component using linear regression
* *Test_points*: 0 [default] or positive integer, specifying the number of points at the end of the series to exclude from the learning (regression) process. This parameter should be set for forecasting purpose
* *AD_method*: A string controlling the anomaly detection method (see [series_outliers](series-outliersfunction.md)) on the residual time series, containing either    
    * “ctukey”: [Tukey’s fence test](https://en.wikipedia.org/wiki/Outlier#Tukey's_fences) with custom 10th-90th percentile range [default]
    * “tukey”: [Tukey’s fence test](https://en.wikipedia.org/wiki/Outlier#Tukey's_fences) with standard 25th-75th percentile range



**Return**

 The function returns the following respective series:

* `ad_flag`: a ternary series containing (+1, -1, 0) marking up/down/no anomaly respectively
* `ad_score`: anomaly score
* `baseline`: the predicted value of the series according to the decomposition

**More about the algorithm**

This function follows these steps:
1. Calls [series_decompose()](series-decomposefunction.md) with the respective parameters to create the baseline and residuals series
2. Calculates ad_score series by applying [series_outliers()](series-outliersfunction.md) with the chosen anomaly detection method on the residuals series
3. Calculates the ad_flag series by applying the threshold on the ad_score to mark up/down/no anomaly respectively
 
**Examples**

**1. Detecting anomalies in Weekly seasonality**

In the following example we generate a series with weekly seasonality, we then add some outliers to it. `series_decompose_anomalies` auto-detects the seasonality and generates a baseline which captures the repetitive pattern. The outliers we added can be clearly spotted in the ad_score component.

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
![alt text](./Images/samples/series-decompose-anomalies1.png "series-decompose-anomalies1")

**2. Detecting anomalies in Weekly seasonality with trend**

In this example we add a trend to the series from the previous example. First, we run `series_decompose_anomalies` with the default parameters in which the trend `avg` default value only takes the average and doesn't compute the trend, we can see that the generated baseline doesn't contain the trend and is less accurate comparing to the previous example, consequently, some of the outliers we inserted in the data are not detected due to the higher variance.

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
![alt text](./Images/samples/series-decompose-anomalies2.png "series-decompose-anomalies2")

Next, we run the same example but since we are expecting a trend in the series, we specify `linefit` in the trend parameter. We can see that the baseline is much closer to the input series. All the outliers we inserted are detected, as well as some false positives (see next example on tuning the threshold).

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
![alt text](./Images/samples/series-decompose-anomalies3.png "series-decompose-anomalies3")

**3. Tweaking the anomaly detection threshold**

In the previous example a few noisy points were detected as anomalies, in this example we increase the anomaly detection threshold from a default of 1.5 to 2.5 the interpercentile range so that only stronger anomalies are detected. We can see that now only the outliers we inserted in the data are detected.

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
![alt text](./Images/samples/series-decompose-anomalies4.png "series-decompose-anomalies4")