---
title:  cos()
description: Learn how to use the cos() function to return the cosine of the input value.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/27/2022
---
# cos()

Returns the cosine function value of the specified angle. The angle is specified in radians.

## Syntax

`cos(`*number*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *number* | real | &check; | The value in radians for which to calculate the cosine. |

## Returns

The cosine of *number* of radians.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUjOL9Yw1AQAT2Uc+QwAAAA=" target="_blank">Run the query</a>

```kusto
print cos(1)
```

**Output**

|result|
|--|
|0.54030230586813977|
