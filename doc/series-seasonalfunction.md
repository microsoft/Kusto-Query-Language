---
title: series_seasonal() - Azure Data Explorer
description: Learn how to use the series_seasonal() function to calculate the seasonal component of a series according to the detected seasonal period.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/30/2023
---
# series_seasonal()

Calculates the seasonal component of a series, according to the detected or given seasonal period.

## Syntax

`series_seasonal(`*series* [`,` *period* ]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *series* | dynamic | &check; | An array of numeric values.|
| *period* | int | | The number of bins for each seasonal period. This value can be any positive integer. By default, the value is set to -1, which automatically detects the period using the [series_periods_detect()](series-periods-detectfunction.md) with a threshold of *0.7*. If seasonality is not detected, the function returns zeros. If a different value is set, it ignores seasonality and returns a series of zeros.|

## Returns

A dynamic array of the same length as the *series* input that contains the calculated seasonal component of the series. The seasonal component is calculated as the *median* of all the values that correspond to the location of the bin, across the periods.

## Examples

### Auto detect the period

In the following example, the series' period is automatically detected. The first series' period is detected to be six bins and the second five bins. The third series' period is too short to be detected and returns a series of zeroes.
See the next example on [how to force the period](#force-a-period).

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUSi2TanMS8zNTNaINtJRMNVRMNZRMAGTQK4hmCRZJFaTq0ahNC8zP09BowDdFgugApAiEyhtAMVkicVq4rPKEOwiU7CjgFrMwI5FEYHoT60oSc1LUSiOL05NLM7PS8xRsFUoTi3KTEWIaBRrAgCrZVUQMAEAAA==" target="_blank">Run the query</a>

```kusto
print s=dynamic([2, 5, 3, 4, 3, 2, 1, 2, 3, 4, 3, 2, 1, 2, 3, 4, 3, 2, 1, 2, 3, 4, 3, 2, 1])
| union (print s=dynamic([8, 12, 14, 12, 10, 10, 12, 14, 12, 10, 10, 12, 14, 12, 10, 10, 12, 14, 12, 10]))
| union (print s=dynamic([1, 3, 5, 2, 4, 6, 1, 3, 5, 2, 4, 6]))
| extend s_seasonal = series_seasonal(s)
```

**Output**

|s|s_seasonal|
|---|---|
|[2,5,3,4,3,2,1,2,3,4,3,2,1,2,3,4,3,2,1,2,3,4,3,2,1]|[1.0,2.0,3.0,4.0,3.0,2.0,1.0,2.0,3.0,4.0,3.0,2.0,1.0,2.0,3.0,4.0,3.0,2.0,1.0,2.0,3.0,4.0,3.0,2.0,1.0]|
|[8,12,14,12,10,10,12,14,12,10,10,12,14,12,10,10,12,14,12,10]|[10.0,12.0,14.0,12.0,10.0,10.0,12.0,14.0,12.0,10.0,10.0,12.0,14.0,12.0,10.0,10.0,12.0,14.0,12.0,10.0]|
|[1,3,5,2,4,6,1,3,5,2,4,6]|[0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0]|

### Force a period

In this example, the series' period is too short to be detected by [series_periods_detect()](series-periods-detectfunction.md), so we explicitly force the period to get the seasonal pattern.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUSi2TanMS8zNTNaINtRRMNZRMNVRgDOMdBRMdBTMYjUVuGoUSvMy8/MUNApwaoOqxqZfE6g/taIkNS9FoTi+ODWxOD8vMUfBVqE4tSgzFSGiUQzUqQkAj46UZJkAAAA=" target="_blank">Run the query</a>

```kusto
print s=dynamic([1, 3, 5, 1, 3, 5, 2, 4, 6]) 
| union (print s=dynamic([1, 3, 5, 2, 4, 6, 1, 3, 5, 2, 4, 6]))
| extend s_seasonal = series_seasonal(s, 3)
```

**Output**

|s|s_seasonal|
|---|---|
|[1,3,5,1,3,5,2,4,6]|[1.0,3.0,5.0,1.0,3.0,5.0,1.0,3.0,5.0]|
|[1,3,5,2,4,6,1,3,5,2,4,6]|[1.5,3.5,5.5,1.5,3.5,5.5,1.5,3.5,5.5,1.5,3.5,5.5]|
 
## See also

* [series_periods_detect()](series-periods-detectfunction.md)
* [series_periods_validate()](series-periods-validatefunction.md)
