---
title: cos() - Azure Data Explorer
description: Learn how to use the cos() function to return the cosine of the input value.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/27/2022
---
# cos()

Returns the cosine function.

## Syntax

`cos(`*number*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *number* | real | &check; | The value for which to calculate the cosine. |

## Returns

The cosine of *number*.

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
