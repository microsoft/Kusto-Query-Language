---
title:  series_fill_linear()
description: Learn how to use the series_fill_linear() function to linearly interpolate missing values in a series.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/26/2023
---
# series_fill_linear()

Linearly interpolates missing values in a series.

Takes an expression containing dynamic numerical array as input, does linear interpolation for all instances of missing_value_placeholder, and returns the resulting array. If the beginning and end of the array contain missing_value_placeholder, then it will be replaced with the nearest value other than missing_value_placeholder. This feature can be turned off. If the whole array consists of the missing_value_placeholder, the array will be filled with constant_value, or 0 if not specified.  

## Syntax

`series_fill_linear(`*series*`,` [ *missing_value_placeholder* [`,`*fill_edges* [`,` *constant_value* ]]]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *series* | dynamic | &check; | An array of numeric values.|
| *missing_value_placeholder* | scalar | | Specifies a placeholder for missing values. The default value is `double(`*null*`)`. The value can be of any type that will be converted to actual element types. `double`(*null*), `long`(*null*) and `int`(*null*) have the same meaning.|
| *fill_edges* | bool | | Indicates whether *missing_value_placeholder* at the start and end of the array should be replaced with nearest value. `true` by default. If set to `false`, then *missing_value_placeholder* at the start and end of the array will be preserved.|
| *constant_value* | scalar | | Relevant only for arrays that entirely consist of *null* values. This parameter specifies a constant value with which to fill the series. Default value is 0. Setting this parameter it to `double(`*null*`)` will preserve the *null* values.|

## Returns

A series linear interpolation of *series* using the specified parameters. If *series* contains only `int` or `long` elements, then the linear interpolation will return rounded interpolated values rather than exact ones.

> [!NOTE]
>
> * If you create *series* using the [make-series](make-seriesoperator.md) operator, specify *null* as the default value to use interpolation functions like `series_fill_linear()` afterwards. See [explanation](make-seriesoperator.md#list-of-series-interpolation-functions).
> * If *missing_value_placeholder* is `double`(*null*), or omitted, then a result may contain *null* values. To fill these *null* values, use other interpolation functions. Only [series_outliers()](series-outliersfunction.md) supports *null* values in input arrays.
> * `series_fill_linear()` preserves the original type of the array elements.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/kvc9rf7q4d68qcw5sk2d6f.northeurope/databases/MyDatabase?query=H4sIAAAAAAAAA5WRzWrDMAzH74G8g44JmHYmXWEbO/QZdiwhaImSeSh2sZ2NwB5+dpq1a2kP0cGy/5b46YPJQ4Me4XVyHt+ZMrT2GZpRY6/qPE0g2P7oZjHb64FZgJRy9SDg+Ci28b75p8xBk76d9KK4/C1zAes17KzFEUwLjRlCAZF0j3emRdY1KXIi5QbjTfWK0QL+sZT2NyHXZ5nDcjs19a38B/TKOaU7+EIeyIHRPKZJ+ZImceZp8gMHaz6pnusJFYq5/5htBl+h7VxYkSOryFWtYq5YaUIbd5WLc3BFTUf3Q8U84Sw2Fubi7UCndLMwuUV2dAGvjXZ+ETwsayU38vEp/wWjK5zoigIAAA==" target="_blank">Run the query</a>

```kusto
let data = datatable(arr: dynamic)
    [
    dynamic([null, 111.0, null, 36.0, 41.0, null, null, 16.0, 61.0, 33.0, null, null]), // Array of double    
    dynamic([null, 111, null, 36, 41, null, null, 16, 61, 33, null, null]), // Similar array of int
    dynamic([null, null, null, null])                                                   // Array with missing values only
];
data
| project
    arr, 
    without_args = series_fill_linear(arr),
    with_edges = series_fill_linear(arr, double(null), true),
    wo_edges = series_fill_linear(arr, double(null), false),
    with_const = series_fill_linear(arr, double(null), true, 3.14159)  
```

**Output**

|`arr`|`without_args`|`with_edges`|`wo_edges`|`with_const`|
|---|---|---|---|---|
|[null,111.0,null,36.0,41.0,null,null,16.0,61.0,33.0,null,null]|[111.0,111.0,73.5,36.0,41.0,32.667,24.333,16.0,61.0,33.0,33.0,33.0]|[111.0,111.0,73.5,36.0,41.0,32.667,24.333,16.0,61.0,33.0,33.0,33.0]|[null,111.0,73.5,36.0,41.0,32.667,24.333,16.0,61.0,33.0,null,null]|[111.0,111.0,73.5,36.0,41.0,32.667,24.333,16.0,61.0,33.0,33.0,33.0]|
|[null,111,null,36,41,null,null,16,61,33,null,null]|[111,111,73,36,41,32,24,16,61,33,33,33]|[111,111,73,36,41,32,24,16,61,33,33,33]|[null,111,73,36,41,32,24,16,61,33,null,null]|[111,111,74,38,  41,32,24,16,61,33,33,33]|
|[null,null,null,null]|[0.0,0.0,0.0,0.0]|[0.0,0.0,0.0,0.0]|[0.0,0.0,0.0,0.0]|[3.14159,3.14159,3.14159,3.14159]|
