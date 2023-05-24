---
title:  monthofyear()
description: Learn how to use the monthofyear() function to get the integer representation of the month.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/08/2023
---
# monthofyear()

Returns the integer number from 1-12 representing the month number of the given year.

> The `monthofyear()` and `getmonth()` functions are equivalent

## Syntax

`monthofyear(`*date*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *date* | datetime | &check; | The date for which to find the month number. |

## Returns

An integer from 1-12 representing the month number of the given year.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKbHNzc8rychPq0xNLNJISSxJLcnMTdVQMjIwNNU1NNI1NFHS1AQAVHohRTAAAAA=" target="_blank">Run the query</a>

```kusto
print result=monthofyear(datetime("2015-12-14"))
```

**Output**

|result|
|--|
|12|
