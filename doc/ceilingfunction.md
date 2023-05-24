---
title:  ceiling()
description: Learn how to use the ceiling() function to calculate the smallest integer greater than, or equal to, the specified numeric expression.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/10/2022
---
# ceiling()

Calculates the smallest integer greater than, or equal to, the specified numeric expression.

## Syntax

`ceiling(`*number*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *number* | int, long, or real | &check; | The value to round up. |

## Returns

The smallest integer greater than, or equal to, the specified numeric expression.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUg2VLBVSE7NzMnMS9fQNdQz1NRRSDZCEjMACRgjC+hZagIAMiJDFDwAAAA=" target="_blank">Run the query</a>

```kusto
print c1 = ceiling(-1.1), c2 = ceiling(0), c3 = ceiling(0.9)
```

**Output**

|c1|c2|c3|
|---|---|---|
|-1|0|1|
