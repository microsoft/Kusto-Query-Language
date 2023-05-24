---
title:  log()
description: Learn how to use the log() function to return the natural logarithm of the input.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/03/2023
---
# log()

The natural logarithm is the base-e logarithm: the inverse of the natural exponential function (exp).  

## Syntax

`log(`*number*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*number*| real | &check; | The number for which to calculate the logarithm.|

## Returns

* `log()` returns the natural logarithm of the input.
* `null` if the argument is negative or null or can't be converted to a `real` value.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKbHNyU/XMNUEAE7U1nYTAAAA" target="_blank">Run the query</a>

```kusto
print result=log(5)
```

**Output**

|result|
|--|
|1.6094379124341003|

## See also

* For common (base-10) logarithms, see [log10()](log10-function.md).
* For base-2 logarithms, see [log2()](log2-function.md).
