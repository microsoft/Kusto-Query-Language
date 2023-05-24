---
title:  cot()
description: Learn how to use the cot() function to calculate the trigonometric cotangent of the specified angle in radians.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/27/2022
---
# cot()

Calculates the trigonometric cotangent of the specified angle, in radians.

## Syntax

`cot(`*number*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *number* | real | &check; | The value for which to calculate the cotangent. |

## Returns

The cotangent function value for *number*.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUjOL9Ew1AQA9l3LZAwAAAA=" target="_blank">Run the query</a>

```kusto
print cot(1)
```

**Output**

|result|
|--|
|0.64209261593433065|
