---
title:  series_decompose()
description: Learn how to use the series_decompose() function to apply a decomposition transformation on a series.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/12/2023
---
# series_decompose()

Applies a decomposition transformation on a series.  

Takes an expression containing a series (dynamic numerical array) as input and decomposes it to seasonal, trend, and residual components.

## Syntax

`series_decompose(`*Series* `,` [ *Seasonality*`,` *Trend*`,` *Test_points*`,` *Seasonality_threshold* ]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*Series*| dynamic | &check; | An array of numeric values, typically the resulting output of [make-series](make-seriesoperator.md) or [make_list](makelist-aggfunction.md) operators.|
|*Seasonality*|int|| Controls the seasonal analysis. The possible values are:<br/><br/>- `-1`: Autodetect seasonality using [series_periods_detect](series-periods-detectfunction.md). This is the default value.<br/>- Period: A positive integer specifying the expected period in number of bins. For example, if the series is in `1 - h` bins, a weekly period is 168 bins.<br/>- `0`: No seasonality, so skip extracting this component.|
|*Trend*|string|| Controls the trend analysis. The possible values are:<br/><br/>- `avg`: Define trend component as `average(x)`. This is the default.<br/>- `linefit`: Extract trend component using linear regression.<br/>- `none`: No trend, so skip extracting this component.<br/>
|*Test_points*|int|| A positive integer specifying the number of points at the end of the series to exclude from the learning, or regression, process. This parameter should be set for forecasting purposes. The default value is 0.|
|*Seasonality_threshold*|real|| The threshold for seasonality score when *Seasonality* is set to autodetect. The default score threshold is 0.6.<br/><br/>For more information, see [series_periods_detect](series-periods-detectfunction.md).|

## Returns

 The function returns the following respective series:

* `baseline`: the predicted value of the series (sum of seasonal and trend components, see below).
* `seasonal`: the series of the seasonal component:
  * if the period isn't detected or is explicitly set to 0: constant 0.
  * if detected or set to positive integer: median of the series points in the same phase
* `trend`: the series of the trend component.
* `residual`: the series of the residual component (that is, x - baseline).
  
>[!NOTE]
> * Component execution order:
>
> 1. Extract the seasonal series
> 1. Subtract it from x, generating the deseasonal series
> 1. Extract the trend component from the deseasonal series
> 1. Create the baseline = seasonal + trend
> 1. Create the residual = x - baseline
>
> * Either seasonality and, or trend should be enabled. Otherwise, the function is redundant, and just returns baseline = 0 and residual = x.

**More about series decomposition**

This method is usually applied to time series of metrics expected to manifest periodic and/or trend behavior. You can use the method to  forecast future metric values and/or detect anomalous values. The implicit assumption of this regression process is that apart from seasonal and trend behavior, the time series is stochastic and randomly distributed. Forecast future metric values from the seasonal and trend components while ignoring the residual part. Detect anomalous values based on outlier detection only on the residual part only. Further details can be found in the [Time Series Decomposition chapter](https://otexts.com/fpp2/decomposition.html).

## Examples

### Weekly seasonality

In the following example, we generate a series with weekly seasonality and without trend, we then add some outliers to it. `series_decompose` finds and automatically detects the seasonality, and generates a baseline that is almost identical to the seasonal component. The outliers we added can be clearly seen in the residuals component.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA21QQW7CMBC884q9IDkpIXYgBbVyX9E7svACFnEc2Yuoqz6+m1ZADvhgzc5oZ1bTIQElHU1/RCA4xOBBAQVo1uWmbCERDkzMfgC/CHsLn85jIuMH0GANIfEsGqm2lVxVUoFs36Qs4AXUCUp2fGxm3mhKTrJi1N3hIATVzbqYbz50uwAll5L/dsnrFQgW5yzWShblBBdQ13DEHiOHg4GE0WGCq6MTXBHPXWbKpNCbzlGepOsxkLRWrYQQgVEjb2iz5eRcbccD8l+CsRZS8AjWDRAu1DmM6Ynb6u6xvqNXOdq8PHNLgzvj1C9dvDfRfeOjWO3NGXedSyTu3NgOv2KRJ2q+se8zSpOi/yvZWdwHP4SEIhcsRpaQ72PL/clE+gWepuS1+gEAAA==" target="_blank">Run the query</a>

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

:::image type="content" source="images/samples/series-decompose1.png" alt-text="Series decompose 1.":::

### Weekly seasonality with trend

In this example, we add a trend to the series from the previous example. First, we run `series_decompose` with the default parameters. The trend `avg` default value only takes the average and doesn't compute the trend. The generated baseline doesn't contain the trend. When observing the trend in the residuals, it becomes apparent that this example is less accurate than the previous example.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA21RQW7CMBC884q5ICWBECeQBrVKX9E7ssgSLJI4shdRV318N6qAHPDBGs9oZ0fjjhjsa6eHlsA4OdsjB1sUu6RKSnimUYjFL+ibaWjwZXryrPsRNRrNxPKOCpXvU7VNVQ5VvisVY4X8jEQcn5NBJopENjXRpJvTKYo4K3bxsvqsyzXKjVojlztGiki0pWhZruJkhqdJzqpio5BlaGkgJyGg4ckZ8rgZPuNGdOmCUNrbQXeGA2Qr7NBaM7RgJ3FmueopCtd1XipYB0GFuqNqL6FCup+yhXjaqZsG3vaExoywV+4MOf/Cbfvw2D3Qm5psVq/c/GguNPfz177XzvzQs/K61xc6dMZz9OCkMiUnXoeZGu7sx4L97Av+Szo0dLT9aD1FIRZxqoMkn1gez9rxH0eeYfoUAgAA" target="_blank">Run the query</a>

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

:::image type="content" source="images/samples/series-decompose2.png" alt-text="Series decompose 2.":::

Next, we rerun the same example. Since we're expecting a trend in the series, we specify `linefit` in the trend parameter. We can see that the positive trend is detected and the baseline is much closer to the input series. The residuals are close to zero, and only the outliers stand out. We can see all the components on the series in the chart.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA21R0Y6CMBB89yvmxQgIUlBOcxfuK+7dNLJgI1DSrvG43MffkovKg31otrO7M5NpSwz2pdN9Q2DUznbIwBb5LtpHBTzTIMDiF/TN1Ff4Mh151t2AEpVmYnkHucoOidomKoMq3pUKsUZ2RiSMz81RNvJIlKpg6pu6DgJO81243H+WRYxio2JkcodIEEhvKb00U2E0q6dNTvf5RiFN0VBPTkxAw5Mz5HEzfMaN6NKOAmlve90aHiGqsH1jTd+AndiZ+SonK1yWWaFgHaTK1b3aH8TUmBwmb2M4aeqqgrcdoTID7JVbQ86/YNs+OHaP6k1NNOtXbH4wF5rz+WvXaWd+6Bl52ekLHVvjOXhgEpmSE8bjrDve0Y8F+9kX/Id0rOhku8F6mgaTLMaqNT3VhlehzE7pkNgVhdNZO/4DpkIQcyMCAAA=" target="_blank">Run the query</a>

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

:::image type="content" source="images/samples/series-decompose3.png" alt-text="Series decompose 3.":::

## See also

* Visualize results with an [anomalychart](visualization-anomalychart.md)
