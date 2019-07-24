# series_stats()

Returns statistics for a series in multiple columns.  

The `series_stats()` function takes a column containing dynamic numerical array as input and calculates the following columns:
* `min`: minimum value in the input array
* `min_idx`: minimum value in the input array
* `max`: maximum value in the input array
* `max_idx`: maximum value in the input array
* `avg`: average value of the input array
* `variance`: sample variance of input array
* `stdev`: sample standard deviation of the input array

*Note* that this function returns multiple columns therefore it cannot be used as an argument for another function.

**Syntax**

project `series_stats(`*x*`)` or extend `series_stats(`*x*`)` 
* Will return all mentioned above columns with the following names: series_stats_x_min, series_stats_x_min_idx and etc.
 
project (m, mi)=`series_stats(`*x*`)` or extend (m, mi)=`series_stats(`*x*`)`
* Will return the following columns: m (min) and mi (min_idx).

**Arguments**

* *x*: Dynamic array cell which is an array of numeric values. 

**Example**

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```
print x=dynamic([23,46,23,87,4,8,3,75,2,56,13,75,32,16,29]) 
| project series_stats(x)

```

|series_stats_x_min|series_stats_x_min_idx|series_stats_x_max|series_stats_x_max_idx|series_stats_x_avg|series_stats_x_stdev|series_stats_x_variance|
|---|---|---|---|---|---|---|
|2|8|87|3|32.8|28.5036338535483|812.457142857143|

