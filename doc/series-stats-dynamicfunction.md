# series_stats_dynamic()

Returns statistics for a series in dynamic object.  

The `series_stats_dynamic()` function takes a column containing dynamic numerical array as input and generates a dynamic value with the following content:
* `min`: minimum value in the input array
* `min_idx`: minimum value in the input array
* `max`: maximum value in the input array
* `max_idx`: maximum value in the input array
* `avg`: average value of the input array
* `variance`: sample variance of input array
* `stdev`: sample standard deviation of the input array

**Syntax**

`series_stats_dynamic(`*x*`)`

**Arguments**

* *x*: Dynamic array cell which is an array of numeric values. 

**Example**

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```
print x=dynamic([23,46,23,87,4,8,3,75,2,56,13,75,32,16,29]) 
| project stats=series_stats_dynamic(x)

```


|stats
|---|
|{"min": 2.0, "min_idx": 8, "max": 87.0, "max_idx": 3, "avg": 32.8, "stdev": 28.503633853548269, "variance": 812.45714285714291 }
