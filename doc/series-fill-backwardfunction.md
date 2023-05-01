---
title: series_fill_backward() - Azure Data Explorer
description: Learn how to use the series_fill_backward() function to perform a backward fill interpolation of missing values in a series.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/22/2023
---
# series_fill_backward()

Performs a backward fill interpolation of missing values in a series.

An expression containing dynamic numerical array is the input. The function replaces all instances of missing_value_placeholder with the nearest value from its right side (other than missing_value_placeholder), and returns the resulting array. The rightmost instances of missing_value_placeholder are preserved.

## Syntax

`series_fill_backward(`*series*`[,`*missing_value_placeholder*`])`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *series* | dynamic | &check; | An array of numeric values.|
| *missing_value_placeholder* | scalar | | Specifies a placeholder for missing values. The default value is `double(`*null*`)`. The value can be of any type that will be converted to actual element types. `double`(*null*), `long`(*null*) and `int`(*null*) have the same meaning.|

> [!NOTE]
>
> * If you create *series* using the [make-series](make-seriesoperator.md) operator, specify *null* as the default value to use interpolation functions like `series_fill_backward()` afterwards. See [explanation](make-seriesoperator.md#list-of-series-interpolation-functions).
> * If *missing_value_placeholder* is `double`(*null*), or omitted, then a result may contain *null* values. To fill these *null* values, use other interpolation functions. Only [series_outliers()](series-outliersfunction.md) supports *null* values in input arrays.
> * `series_fill_backward()` preserves the original type of the array elements.

## Returns

*series* with all instances of *missing_value_placeholder* filled backwards.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVFISSxJVLAFUyWJSTmpGolFRVYKKZV5ibmZyZpcCkAQDSahQhrRhoaGOgp5pTk5OgrGZjoKJnAehDQEipkBxYyNkcVjNYFGcMVac4Ht46pRKCjKz0pNLgEbDbRSRwHMSsvMyYlPSkzOLk8sSgE6qzi1KDO1OB5FGORCTQBymgAduwAAAA==" target="_blank">Run the query</a>

```kusto
let data = datatable(arr: dynamic)
    [
    dynamic([111, null, 36, 41, null, null, 16, 61, 33, null, null])   
];
data 
| project
    arr, 
    fill_backward = series_fill_backward(arr)
```

**Output**

|`arr`|`fill_backward`|
|---|---|
|[111,null,36,41,null,null,16,61,33,null,null]|[111,36,36,41,16,16,16,61,33,null,null]|

> [!TIP]
> Use [series_fill_forward](series-fill-forwardfunction.md) or [series-fill-const](series-fill-constfunction.md) to complete interpolation of the above array.
