---
title:  series_decompose_anomalies()
description: Learn how to use series_decompose_anomalies() function to extract anomalous points from a dynamic numerical array.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/12/2023
---
# series_decompose_anomalies()

Anomaly Detection is based on series decomposition.
For more information, see [series_decompose()](series-decomposefunction.md).

The function takes an expression containing a series (dynamic numerical array) as input, and extracts anomalous points with scores.

## Syntax

`series_decompose_anomalies (`*Series*`,` [ *Threshold*`,` *Seasonality*`,` *Trend*`,` *Test_points*`,` *AD_method*`,` *Seasonality_threshold* ]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*Series*| dynamic | &check; | An array of numeric values, typically the resulting output of [make-series](make-seriesoperator.md) or [make_list](makelist-aggfunction.md) operators.|
|*Threshold*|real||The anomaly threshold. The default is 1.5, k value, for detecting mild or stronger anomalies.|
|*Seasonality*|int|| Controls the seasonal analysis. The possible values are:<br/><br/>- `-1`: Autodetect seasonality using [series_periods_detect](series-periods-detectfunction.md). This is the default value.<br/>- Period: A positive integer specifying the expected period in number of bins. For example, if the series is in `1 - h` bins, a weekly period is 168 bins.<br/>- `0`: No seasonality, so skip extracting this component.|
|*Trend*|string|| Controls the trend analysis. The possible values are:<br/><br/>- `avg`: Define trend component as `average(x)`. This is the default.<br/>- `linefit`: Extract trend component using linear regression.<br/>- `none`: No trend, so skip extracting this component.<br/>
|*Test_points*|int|| A positive integer specifying the number of points at the end of the series to exclude from the learning, or regression, process. This parameter should be set for forecasting purposes. The default value is 0.|
|*AD_method*|string||Controls the anomaly detection method on the residual time series, containing one of the following values:<br/><br/>- `ctukey`: [Tukey’s fence test](https://en.wikipedia.org/wiki/Outlier#Tukey's_fences) with custom 10th-90th percentile range. This is the default.<br/>- `tukey`: [Tukey’s fence test](https://en.wikipedia.org/wiki/Outlier#Tukey's_fences) with standard 25th-75th percentile range.<br/><br/>For more information on residual time series, see [series_outliers](series-outliersfunction.md).
|*Seasonality_threshold*|real|| The threshold for seasonality score when *Seasonality* is set to autodetect. The default score threshold is 0.6.<br/><br/>For more information, see [series_periods_detect](series-periods-detectfunction.md).|

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

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA21QQW7CMBC884q9IDkpIXYgBbVyX9E7svACFnEc2Yuoqz6+m1ZADvhgjWe8M6vpkICSjqY/IhAcYvCggAI063JTtpAIByZmP4BfhL2FT+cxkfEDaLCGkPgtGqm2lVxVUoFs36Qs4AXUCUp2fExmnmhKTrJi1N3hIATVzbqYbz50uwAll5LvdsnjFQgW5yzWShblBBdQ13DEHiOHg4GE0WGCq6MTXBHPXWbKpNCbzlGepOsxkLRWrYQQgVEjb2iz5eRcbccF8l+CsRZS8AjWDRAu1DmM6Ynb6u6xvqNXOdq8PHNLgzvj1C9dvDfRfeOjWO3NGXedSyTu3NgOn2KRJ2q+se8zSpOi/yvZWdwHP4SEO9MHz3VgErngb5E/IW/K5vuTifQLuaqurwQCAAA=" target="_blank">Run the query</a>

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

:::image type="content" source="images/series-decompose-anomaliesfunction/weekly-seasonality-outliers.png" alt-text="Weekly seasonality showing baseline and outliers." border="false":::

### Detect anomalies in weekly seasonality with trend

In this example, add a trend to the series from the previous example. First, run `series_decompose_anomalies` with the default parameters in which the trend `avg` default value only takes the average and doesn't compute the trend. The generated baseline doesn't contain the trend and is less exact, compared to the previous example. Consequently, some of the outliers you inserted in the data aren't detected because of the higher variance.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA41RwXKjMAy95yvepTNAk2JoaDLtsF+xd8ZbBPEEY8YWbd3Zj1/RblIOPZQDI0t6T09PAzE41F6PPYHReWdRgB3KfXbIKgSmSRKbv6A3prHFb2MpsLYTarSaieWdlKo47tT9ThVQ1aNSKW5RnJAJ4xcyCqLMZFKbLHXTdUnCeblPbw6/6mqL6k5tUcg/xQ6J1G6klhcqzVbxguT8UN4p5Dl6GsmLCGgE8oYCXg2f8Ep0HqKkdHCjHgxHyFS4sXdm7MFe5Kx01YsUruuiUnAeEpXqEh2OIirujou2mC4zddsiOEtozQQ382DIh2/Y7q8c+2v0oBaa2+/YwmTOtOYLs7Xam3f6sry2+kzNYAIn15xYpuRLt3FVjZfs04bD6gSfJjUtPTs7uUCNHp0VgygkMf1JWxMb3TbdoHs55uZ/n50HNtMQk0LW+gn2Y/MLCn+iiEUnDr2YMEvru2bjRkyzXwgWL5aDkTgoSz+ftOd/zGMKx7YCAAA=" target="_blank">Run the query</a>

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

:::image type="content" source="images/series-decompose-anomaliesfunction/weekly-seasonality-outliers-with-trend.png" alt-text="Weekly seasonality outliers with trend." border="false":::

Next, run the same example, but since you're expecting a trend in the series, specify `linefit` in the trend parameter. You can see that the baseline is much closer to the input series. All the inserted outliers are detected, and also some false positives. See the next example on tweaking the threshold.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA41RwXKbMBC9+yveJRMgxghiak8z9Ct6Z9SwYI0RYqQlqTL9+C5p7fiQQ3TQrHb13r59OxKDQ+P1NBAYvXcWJdih2meHrEZgmiWx+QP6zTR1+GksBdZ2RoNOM7G8k0qVx1w95qqEqr8rleIB5QmZMH4goyCqTDp1yVo3fZ8kXFT79O7wo6m3qHdqi1LuFDkSqd1JrShVmt3EK5KLQ7VTKAoMNJEXEdAI5A0FvBo+4ZXoPEZJ6eAmPRqOkK5w0+DMNIC9yLnR1axSuGnKWsF5SFSpS3Q4iqiYH1dtMV176q5DcJbQmRlu4dGQD5+wPV459tfom1ppHj5jC7M50y1fWKzV3rzRh+WN1WdqRxM4uebEMiUn3cabarxknzYcblbwz6S2o2dnZxeo1ZOzYhCFd8hOtpCXW9yPZqLe8H36FWgbW921/agHWfDm/z+7jGzmMSaljPoV7LsbFxR+RRkAvbj2YsIiX980GzdhXvxKsPqzLpHEVTHi+aQ9/wVL2dxLygIAAA==" target="_blank">Run the query</a>

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

:::image type="content" source="images/series-decompose-anomaliesfunction/weekly-seasonality-linefit-trend.png" alt-text="Weekly seasonality anomalies with linefit trend." border="false":::

### Tweak the anomaly detection threshold

A few noisy points were detected as anomalies in the previous example. Now increase the anomaly detection threshold from a default of 1.5 to 2.5. Use this interpercentile range, so that only stronger anomalies are detected. Now, only the outliers you inserted in the data, will be detected.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA41RTVPjMAy991fowpCEpnFCQzsw2V+x94whSuqpPzK2ApjZH7/yLi05cMAHjyz5PT09aSSg0HlpJwSC0TsDNZCDZl8cihYC4cyJzR/Ad0I7wG9lMJA0M3QwSELid9aI+liK+1LUINpHIXK4g/oEBTN+ISMjmoI7DVmqq3HMMqqafX5z+NW1W2h3Ygs13zmUkHHthmtVLfJiFSckVYdmJ6CqYEKLnkWAhIBeYYA3RSd4QzzryCkZnJVaUQTuCs7qySk7AXnWsxLWJS3UdXUrwHngqBGX6HBkVbE8JnExT03lMEBwBmFQM7iFtEIfvmG7v3Lsr9GDSDR337GFWZ1xzRcWY6RXH/jleWfkGXutAmXXHHsm+OTbuKrGS/ZpQ2G1g/8u9QO+ODO7gL20zrBDGBKk2fEaynoLt1pZHBXd5j+B9rGXQz9qOfGGN5//zKJJzTpmNY/6E+w/Ny4oeI48AIzs2qsKC3/9kKSchXnxiSD5k5aI7Cob8XKSnv4C5QDdI8sCAAA=" target="_blank">Run the query</a>

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

:::image type="content" source="images/series-decompose-anomaliesfunction/weekly-seasonality-higher-threshold.png" alt-text="Weekly series anomalies with higher anomaly threshold." border="false":::
