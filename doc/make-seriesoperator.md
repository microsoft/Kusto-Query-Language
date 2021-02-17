---
title: make-series operator - Azure Data Explorer
description: This article describes make-series operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 03/16/2020
ms.localizationpriority: high
---
# make-series operator

Create series of specified aggregated values along a specified axis.

```kusto
T | make-series sum(amount) default=0, avg(price) default=0 on timestamp from datetime(2016-01-01) to datetime(2016-01-10) step 1d by fruit, supplier
```

## Syntax

*T* `| make-series` [*MakeSeriesParamters*]
      [*Column* `=`] *Aggregation* [`default` `=` *DefaultValue*] [`,` ...]
    `on` *AxisColumn* [`from` *start*] [`to` *end*] `step` *step* 
    [`by`
      [*Column* `=`] *GroupExpression* [`,` ...]]

## Arguments

* *Column:* Optional name for a result column. Defaults to a name derived from the expression.
* *DefaultValue:* Default value that will be used instead of absent values. If there is no row with specific values of *AxisColumn* and *GroupExpression*, then in the results the corresponding element of the array will be assigned a *DefaultValue*. If *DefaultValue* is omitted, then 0 is assumed. 
* *Aggregation:* A call to an [aggregation function](make-seriesoperator.md#list-of-aggregation-functions) such as `count()` or `avg()`, with column names as arguments. See the [list of aggregation functions](make-seriesoperator.md#list-of-aggregation-functions). Only aggregation functions that return numeric results can be used with the `make-series` operator.
* *AxisColumn:* A column on which the series will be ordered. It could be considered as timeline, but besides `datetime` any numeric types are accepted.
* *start*: (optional) The low bound value of the *AxisColumn* for each of the series to be built. *start*, *end*, and *step* are used to build an array of *AxisColumn* values within a given range and using specified *step*. All *Aggregation* values are ordered respectively to this array. This *AxisColumn* array is also the last output column in the output that has the same name as *AxisColumn*. If a *start* value is not specified, the start is the first bin (step) which has data in each series.
* *end*: (optional) The high bound (non-inclusive) value of the *AxisColumn*. The last index of the time series is smaller than this value (and will be *start* plus integer multiple of *step* that is smaller than *end*). If *end* value is not provided, it will be the upper bound of the last bin (step) which has data per each series.
* *step*: The difference between two consecutive elements of the *AxisColumn* array (that is, the bin size).
* *GroupExpression:* An expression over the columns that provides a set of distinct values. Typically it's a column name that already provides a restricted set of values. 
* *MakeSeriesParameters*: Zero or more (space-separated) parameters in the form of *Name* `=` *Value* 
	that control the behavior. The following parameters are supported: 
  
  |Name           |Values                                        |Description                                                                                        |
  |---------------|-------------------------------------|------------------------------------------------------------------------------|
  |`kind`          |`nonempty`								 |Produces default result when the input of make-series operator is empty|                                

## Returns

The input rows are arranged into groups having the same values of the `by` expressions and the `bin_at(`*AxisColumn*`, `*step*`, `*start*`)` expression. Then the specified aggregation functions are computed over each group, producing a row for each group. The result contains the `by` columns, *AxisColumn* column and also at least one column for each computed aggregate. (Aggregation that multiple columns or non-numeric results are not supported.)

This intermediate result has as many rows as there are distinct combinations of `by` and `bin_at(`*AxisColumn*`, `*step*`, `*start*`)` values.

Finally the rows from the intermediate result arranged into groups having the same values of the `by` expressions and all aggregated values are arranged into arrays (values of `dynamic` type). For each aggregation, there is one column containing its array with the same name. The last column in the output of the range function with all *AxisColumn* values. Its value is repeated for all rows. 

Due to the fill missing bins by default value, the resulting pivot table has the same number of bins (that is, aggregated values) for all series  

**Note**

Although you can provide arbitrary expressions for both the aggregation and grouping expressions, it's more efficient to use simple column names.

**Alternate Syntax**

*T* `| make-series`
      [*Column* `=`] *Aggregation* [`default` `=` *DefaultValue*] [`,` ...]
    `on` *AxisColumn* `in` `range(`*start*`,` *stop*`,` *step*`)`
    [`by`
      [*Column* `=`] *GroupExpression* [`,` ...]]

The generated series from the alternate syntax differs from the main syntax in two aspects:
* The *stop* value is inclusive.
* Binning the index axis is generated with bin() and not bin_at(), which means that *start* may not be included in the generated series.

It is recommended to use the main syntax of make-series and not the alternate syntax.

**Distribution and Shuffle**

`make-series` supports `summarize` [shufflekey hints](shufflequery.md) using the syntax hint.shufflekey.

## List of aggregation functions

|Function|Description|
|--------|-----------|
|[any()](any-aggfunction.md)|Returns a random non-empty value for the group|
|[avg()](avg-aggfunction.md)|Returns an average value across the group|
|[avgif()](avgif-aggfunction.md)|Returns an average with the predicate of the group|
|[count()](count-aggfunction.md)|Returns a count of the group|
|[countif()](countif-aggfunction.md)|Returns a count with the predicate of the group|
|[dcount()](dcount-aggfunction.md)|Returns an  approximate distinct count of the group elements|
|[dcountif()](dcountif-aggfunction.md)|Returns an approximate distinct count with the predicate of the group|
|[max()](max-aggfunction.md)|Returns the maximum value across the group|
|[maxif()](maxif-aggfunction.md)|Returns the maximum value with the predicate of the group|
|[min()](min-aggfunction.md)|Returns the minimum value across the group|
|[minif()](minif-aggfunction.md)|Returns the minimum value with the predicate of the group|
|[percentile()](percentiles-aggfunction.md)|Returns the percentile value across the group|
|[stdev()](stdev-aggfunction.md)|Returns the standard deviation across the group|
|[sum()](sum-aggfunction.md)|Returns the sum of the elements within the group|
|[sumif()](sumif-aggfunction.md)|Returns the sum of the elements with the predicate of the group|
|[variance()](variance-aggfunction.md)|Returns the variance across the group|

## List of series analysis functions

|Function|Description|
|--------|-----------|
|[series_fir()](series-firfunction.md)|Applies [Finite Impulse Response](https://en.wikipedia.org/wiki/Finite_impulse_response) filter|
|[series_iir()](series-iirfunction.md)|Applies [Infinite Impulse Response](https://en.wikipedia.org/wiki/Infinite_impulse_response) filter|
|[series_fit_line()](series-fit-linefunction.md)|Finds a straight line that is the best approximation of the input|
|[series_fit_line_dynamic()](series-fit-line-dynamicfunction.md)|Finds a line that is the best approximation of the input, returning dynamic object|
|[series_fit_2lines()](series-fit-2linesfunction.md)|Finds two lines that are the best approximation of the input|
|[series_fit_2lines_dynamic()](series-fit-2lines-dynamicfunction.md)|Finds two lines that are the best approximation of the input, returning dynamic object|
|[series_outliers()](series-outliersfunction.md)|Scores anomaly points in a series|
|[series_periods_detect()](series-periods-detectfunction.md)|Finds the most significant periods that exist in a time series|
|[series_periods_validate()](series-periods-validatefunction.md)|Checks whether a time series contains periodic patterns of given lengths|
|[series_stats_dynamic()](series-stats-dynamicfunction.md)|Return multiple columns with the common statistics (min/max/variance/stdev/average)|
|[series_stats()](series-statsfunction.md)|Generates a dynamic value with the common statistics (min/max/variance/stdev/average)|

For a complete list of series analysis functions see: [Series processing functions](scalarfunctions.md#series-processing-functions)

## List of series interpolation functions

|Function|Description|
|--------|-----------|
|[series_fill_backward()](series-fill-backwardfunction.md)|Performs backward fill interpolation of missing values in a series|
|[series_fill_const()](series-fill-constfunction.md)|Replaces missing values in a series with a specified constant value|
|[series_fill_forward()](series-fill-forwardfunction.md)|Performs forward fill interpolation of missing values in a series|
|[series_fill_linear()](series-fill-linearfunction.md)|Performs linear interpolation of missing values in a series|

* Note: Interpolation functions by default assume `null` as a missing value. Therefore specify `default=`*double*(`null`) in `make-series` if you intend to use interpolation functions for the series. 

## Example
  
 A table that shows arrays of the numbers and average prices of each fruit from each supplier ordered by the timestamp with specified range. There's a row in the output for each distinct combination of fruit and supplier. The output columns show the fruit, supplier, and arrays of: count, average, and the whole timeline (from 2016-01-01 until 2016-01-10). All arrays are sorted by the respective timestamp and all gaps are filled with default values (0 in this example). All other input columns are ignored.
  
```kusto
T | make-series PriceAvg=avg(Price) default=0
on Purchase from datetime(2016-09-10) to datetime(2016-09-13) step 1d by Supplier, Fruit
```

:::image type="content" source="images/make-seriesoperator/makeseries.png" alt-text="Three tables. The first lists raw data, the second has only distinct supplier-fruit-date combinations, and the third contains the make-series results.":::  

<!-- csl: https://help.kusto.windows.net:443/Samples --> 
```kusto
let data=datatable(timestamp:datetime, metric: real)
[
  datetime(2016-12-31T06:00), 50,
  datetime(2017-01-01), 4,
  datetime(2017-01-02), 3,
  datetime(2017-01-03), 4,
  datetime(2017-01-03T03:00), 6,
  datetime(2017-01-05), 8,
  datetime(2017-01-05T13:40), 13,
  datetime(2017-01-06), 4,
  datetime(2017-01-07), 3,
  datetime(2017-01-08), 8,
  datetime(2017-01-08T21:00), 8,
  datetime(2017-01-09), 2,
  datetime(2017-01-09T12:00), 11,
  datetime(2017-01-10T05:00), 5,
];
let interval = 1d;
let stime = datetime(2017-01-01);
let etime = datetime(2017-01-10);
data
| make-series avg(metric) on timestamp from stime to etime step interval 
```
  
|avg_metric|timestamp|
|---|---|
|[ 4.0, 3.0, 5.0, 0.0, 10.5, 4.0, 3.0, 8.0, 6.5 ]|[ "2017-01-01T00:00:00.0000000Z", "2017-01-02T00:00:00.0000000Z", "2017-01-03T00:00:00.0000000Z", "2017-01-04T00:00:00.0000000Z", "2017-01-05T00:00:00.0000000Z", "2017-01-06T00:00:00.0000000Z", "2017-01-07T00:00:00.0000000Z", "2017-01-08T00:00:00.0000000Z", "2017-01-09T00:00:00.0000000Z" ]|  


When the input to `make-series` is empty, the default behavior of `make-series` produces an empty result as well.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let data=datatable(timestamp:datetime, metric: real)
[
  datetime(2016-12-31T06:00), 50,
  datetime(2017-01-01), 4,
  datetime(2017-01-02), 3,
  datetime(2017-01-03), 4,
  datetime(2017-01-03T03:00), 6,
  datetime(2017-01-05), 8,
  datetime(2017-01-05T13:40), 13,
  datetime(2017-01-06), 4,
  datetime(2017-01-07), 3,
  datetime(2017-01-08), 8,
  datetime(2017-01-08T21:00), 8,
  datetime(2017-01-09), 2,
  datetime(2017-01-09T12:00), 11,
  datetime(2017-01-10T05:00), 5,
];
let interval = 1d;
let stime = datetime(2017-01-01);
let etime = datetime(2017-01-10);
data
| limit 0
| make-series avg(metric) default=1.0 on timestamp from stime to etime step interval 
| count 
```

|Count|
|---|
|0|


Using `kind=nonempty` in `make-series` will produce a non-empty result of the default values:

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let data=datatable(timestamp:datetime, metric: real)
[
  datetime(2016-12-31T06:00), 50,
  datetime(2017-01-01), 4,
  datetime(2017-01-02), 3,
  datetime(2017-01-03), 4,
  datetime(2017-01-03T03:00), 6,
  datetime(2017-01-05), 8,
  datetime(2017-01-05T13:40), 13,
  datetime(2017-01-06), 4,
  datetime(2017-01-07), 3,
  datetime(2017-01-08), 8,
  datetime(2017-01-08T21:00), 8,
  datetime(2017-01-09), 2,
  datetime(2017-01-09T12:00), 11,
  datetime(2017-01-10T05:00), 5,
];
let interval = 1d;
let stime = datetime(2017-01-01);
let etime = datetime(2017-01-10);
data
| limit 0
| make-series kind=nonempty avg(metric) default=1.0 on timestamp from stime to etime step interval 
```

|avg_metric|timestamp|
|---|---|
|[<br>  1.0,<br>  1.0,<br>  1.0,<br>  1.0,<br>  1.0,<br>  1.0,<br>  1.0,<br>  1.0,<br>  1.0<br>]|[<br>  "2017-01-01T00:00:00.0000000Z",<br>  "2017-01-02T00:00:00.0000000Z",<br>  "2017-01-03T00:00:00.0000000Z",<br>  "2017-01-04T00:00:00.0000000Z",<br>  "2017-01-05T00:00:00.0000000Z",<br>  "2017-01-06T00:00:00.0000000Z",<br>  "2017-01-07T00:00:00.0000000Z",<br>  "2017-01-08T00:00:00.0000000Z",<br>  "2017-01-09T00:00:00.0000000Z"<br>]|
