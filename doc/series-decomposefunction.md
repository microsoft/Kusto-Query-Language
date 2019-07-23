---
title: series_decompose() - Azure Data Explorer | Microsoft Docs
description: This article describes series_decompose() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 07/21/2019
---
# series_decompose()

Applies a decomposition transformation on a series.  

Takes an expression containing a series (dynamic numerical array) as input and decompose it to seasonal, trend and residual components.
 
**Syntax**

`series_decompose(`*Series* `[,` *Seasonality*`,` *Trend*`,` *Test_points*`])`

**Arguments**

* *Series*: Dynamic array cell which is an array of numeric values, typically the resulting output of [make-series](make-seriesoperator.md) or [make_list](makelist-aggfunction.md) operators
* *Seasonality*: An integer controlling the seasonal analysis, containing either
    * -1: autodetect seasonality (using [series_periods_detect](series-periods-detectfunction.md) [default] 
    * period: positive integer, specifying the expected period in number of bins unit. For example, if the series is in 1h bins, a weekly period is 168 bins
    * 0: no seasonality (i.e. skip extracting this component)    
* *Trend*: A string controlling the trend analysis, containing either
    * "avg": define trend component as average(x) [default]
    * "linefit": extract trend component using linear regression
    * "none": no trend, skip extracting this component    
* *Test_points*: 0 [default] or positive integer, specifying the number of points at the end of the series to exclude from the learning (regression) process. This parameter should be set for forecasting purpose

**Return**

 The function returns the following respective series:

* `baseline`: the predicted value of the series (sum of seasonal and trend components, see below)
* `seasonal`: the series of the seasonal component:
    * if period is not detected or explicitly set to 0: constant 0
    * if detected or set to positive integer: median of the series points in the same phase
* `trend`: the series of the trend component
* `residual`: the series of the residual component (i.e. x - baseline)
  

**Notes**

* Components execution order:
    1. Extract the seasonal series
    2. Subtract it from x, generating the deseasonal series
    3. Extract the trend component from the deseasonal series
    4. Create the baseline = seasonal + trend
    5. Create the residual = x - baseline
    
* Either seasonality and/or trend should be enabled, otherwise the function is redundant and just returns baseline = 0 and residual = x

**More about series decomposition**

This method is usually applied to time series of metrics expected to manifest periodic and/or trend behavior (e.g. service traffic and other usage metrics), in order to either forecast future metric values and/or detect anomalous ones. The implicit assumption of this regression process is that apart from the (a-priori known) seasonal and trend behavior, the time series is stochastic and randomly distributed; consequently, we can forecast future metric values from the seasonal and trend components (ignoring the residual part), while we can detect anomalous values based on outlier detection on the residual part only. Further details can be found in the [Time Series Decomposition chapter](https://www.otexts.org/fpp/6) of this great book.

**Examples**

**1. Weekly seasonality**

In the following example we generate a series with weekly seasonality and without trend, we then add some outliers to it. `series_decompose` finds auto-detects the seasonality and generates a baseline which is almost identical to the the seasonal component. The outliers we added can be clearly seen in the residuals component.

```kusto
let ts=range t from 1 to 24*7*5 step 1 
| extend Timestamp = datetime(2018-03-01 05:00) + 1h * t 
| extend y = 2*rand() + iff((t/24)%7>=5, 10.0, 15.0) - (((t%24)/10)*((t%24)/10)) // generate a series with weekly seasonality
| extend y=iff(t==150 or t==200 or t==780, y-8.0, y) // add some dip outliers
| extend y=iff(t==300 or t==400 or t==600, y+8.0, y) // add some spike outliers
| summarize Timestamp=make_list(Timestamp, 10000),y=make_list(y, 10000);
ts 
| extend series_decompose(y)
| render timechart  
```
![alt text](./Images/samples/series-decompose1.png "series-decompose1")

**2. Weekly seasonality with trend**

In this example we add a trend to the series from the previous example. First, we run `series_decompose` with the default parameters in which the trend `avg` default value only takes the average and doesn't compute the trend, we can see that the generated baseline doesn't contain the trend and is less accurate comparing to the previous example, it is most apparent when observing the trend in the residuals.

```kusto
let ts=range t from 1 to 24*7*5 step 1 
| extend Timestamp = datetime(2018-03-01 05:00) + 1h * t 
| extend y = 2*rand() + iff((t/24)%7>=5, 5.0, 15.0) - (((t%24)/10)*((t%24)/10)) + t/72.0 // generate a series with weekly seasonality and ongoing trend
| extend y=iff(t==150 or t==200 or t==780, y-8.0, y) // add some dip outliers
| extend y=iff(t==300 or t==400 or t==600, y+8.0, y) // add some spike outliers
| summarize Timestamp=make_list(Timestamp, 10000),y=make_list(y, 10000);
ts 
| extend series_decompose(y)
| render timechart  
```
![alt text](./Images/samples/series-decompose2.png "series-decompose2")

Next, we run the same example but since we are expecting a trend in the series, we specify `linefit` in the trend parameter. We can see that the positive trend is detected and the baseline is much closer to the input series. The residuals are close to zero with only the outliers standing out. We can see all the components on the series in the chart.

```kusto
let ts=range t from 1 to 24*7*5 step 1 
| extend Timestamp = datetime(2018-03-01 05:00) + 1h * t 
| extend y = 2*rand() + iff((t/24)%7>=5, 5.0, 15.0) - (((t%24)/10)*((t%24)/10)) + t/72.0 // generate a series with weekly seasonality and ongoing trend
| extend y=iff(t==150 or t==200 or t==780, y-8.0, y) // add some dip outliers
| extend y=iff(t==300 or t==400 or t==600, y+8.0, y) // add some spike outliers
| summarize Timestamp=make_list(Timestamp, 10000),y=make_list(y, 10000);
ts 
| extend series_decompose(y, -1, 'linefit')
| render timechart  
```
![alt text](./Images/samples/series-decompose3.png "series-decompose3")