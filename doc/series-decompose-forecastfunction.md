# series_decompose_forecast()

Forecast based on series decomposition.

Takes an expression containing a series (dynamic numerical array) as input and predict the values of last
trailing points (refer to [series_decompose](series-decomposefunction.md) for more details on the decomposition method).
 
**Syntax**

`series_decompose_forecast(`*Series* `,` *Points* `[,` *Seasonality*`,` *Trend*`,` *Seasonality_threshold*`])`

**Arguments**

* *Series*: Dynamic array cell that is an array of numeric values. Typically the resulting output of [make-series](make-seriesoperator.md) or [make_list](makelist-aggfunction.md) operators.
* *Points*: Integer specifying the number of points at the end of the series to predict (forecast). These points are excluded from the learning (regression) process.
* *Seasonality*: An integer controlling the seasonal analysis, containing one of the following:
    * -1: auto-detect seasonality using [series_periods_detect](series-periods-detectfunction.md) (default). 
    * period: positive integer, specifying the expected period in number of bins. For example, if the series is in 1h bins, a weekly period is 168 bins.
    * 0: no seasonality (skip extracting this component).   
* *Trend*: A string controlling the trend analysis, containing one of the following:
    * "linefit": extract trend component using linear regression (default).    
    * "avg": define trend component as average(x).
    * "none": no trend, skip extracting this component.   
* *Seasonality_threshold*: The threshold for seasonality score when *Seasonality* is set to auto-detect, the default score threshold is `0.6`. For more information, see [series_periods_detect](series-periods-detectfunction.md).

**Return**

 A dynamic array with the forecasted series
  

**Notes**

* The dynamic array of the original input series should include a number *points* slots to be forecasted, this is typically done by using [make-series](make-seriesoperator.md) and specifying the end time in range that includes the timeframe to forecast.
    
* Either seasonality and/or trend should be enabled, otherwise the function is redundant and just returns a series filled with zeroes.

**Example**

In the following example we generate a series of 4 weeks in an hourly grain with weekly seasonality and a small upward trend, we then use `make-series` and add another empty week to the series. `series_decompose_forecast` is called with a week (24*7 points), it automatically detects the seasonality and trend and generates a forecast of the entire 5 weeks period. 

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```
let ts=range t from 1 to 24*7*4 step 1 // generate 4 weeks of hourly data
| extend Timestamp = datetime(2018-03-01 05:00) + 1h * t 
| extend y = 2*rand() + iff((t/24)%7>=5, 5.0, 15.0) - (((t%24)/10)*((t%24)/10)) + t/72.0 // generate a series with weekly seasonality and ongoing trend
| extend y=iff(t==150 or t==200 or t==780, y-8.0, y) // add some dip outliers
| extend y=iff(t==300 or t==400 or t==600, y+8.0, y) // add some spike outliers
| make-series y=max(y) on Timestamp in range(datetime(2018-03-01 05:00), datetime(2018-03-01 05:00)+24*7*5h, 1h); // create a time series of 5 weeks (last week is empty)
ts 
| extend y_forcasted = series_decompose_forecast(y, 24*7)  // forecast a week forward
| render timechart 
```
![alt text](./Images/samples/series-decompose-forecast.png "series-decompose_forecast")

