---
title:  log10()
description: Learn how to use the log10() function to return the common (base-10) logarithm of the input.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/05/2023
---
# log10()

`log10()` returns the common (base-10) logarithm of the input.

## Syntax

`log10(`*number*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*number*| real | &check; | The number for which to calculate the base-10 logarithm.|

## Returns

* The common logarithm is the base-10 logarithm: the inverse of the exponential function (exp) with base 10.
* `null` if the argument is negative or null or can't be converted to a `real` value.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKbHNyU83NNAw1QQAQyXyFRUAAAA=" target="_blank">Run the query</a>

```kusto
print result=log10(5)
```

**Output**

|result|
|--|
|0.69897000433601886|

## See also

* For natural (base-e) logarithms, see [log()](log-function.md).
* For base-2 logarithms, see [log2()](log2-function.md)