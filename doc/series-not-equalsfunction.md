---
title:  series_not_equals()
description: Learn how to use the series_not_equals() function to calculate the element-wise not equals (`!=`) logic operation of two numeric series inputs.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/30/2023
---
# series_not_equals()

Calculates the element-wise not equals (`!=`) logic operation of two numeric series inputs.

## Syntax

`series_not_equals(`*series1*`,` *series2*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *series1*, *series2* | dynamic | &check; | The arrays of numeric values to be element-wise compared.|

## Returns

Dynamic array of booleans containing the calculated element-wise not equal logic operation between the two inputs. Any non-numeric element or non-existing element (arrays of different sizes) yields a `null` element value.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUSg2VLBVSKnMS8zNTNaINtQx0jGJ1dRRKDZCFjYBChvGanLVKKRWlKTmpQB1xefll8SnFpYm5hTHgxUXpxZlphYjCWsUG4LM0QQA1OZCDmgAAAA=" target="_blank">Run the query</a>

```kusto
print s1 = dynamic([1,2,4]), s2 = dynamic([4,2,1])
| extend s1_not_equals_s2 = series_not_equals(s1, s2)
```

**Output**

|s1|s2|s1_not_equals_s2|
|---|---|---|
|[1,2,4]|[4,2,1]|[true,false,true]|

## See also

For entire series statistics comparisons, see:

* [series_stats()](series-statsfunction.md)
* [series_stats_dynamic()](series-stats-dynamicfunction.md)
