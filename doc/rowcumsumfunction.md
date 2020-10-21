---
title: row_cumsum() - Azure Data Explorer | Microsoft Docs
description: This article describes row_cumsum() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# row_cumsum()

Calculates the cumulative sum of a column in a [serialized row set](./windowsfunctions.md#serialized-row-set).

## Syntax

`row_cumsum` `(` *Term* [`,` *Restart*] `)`

* *Term* is an expression indicating the value to be summed.
  The expression must be a scalar of one of the following types:
  `decimal`, `int`, `long`, or `real`. Null *Term* values do not affect the
  sum.
* *Restart* is an optional argument of type `bool` that indicates when the
  accumulation operation should be restarted (set back to 0). It can be
  used to indicate partitions of the data; see the second example below.

## Returns

The function returns the cumulative sum of its argument.

## Examples

The following example shows how to calculate the cumulative sum of the first
few even integers.

```kusto
datatable (a:long) [
    1, 2, 3, 4, 5, 6, 7, 8, 9, 10
]
| where a%2==0
| serialize cs=row_cumsum(a)
```

a    | cs
-----|-----
2    | 2
4    | 6
6    | 12
8    | 20
10   | 30

This example shows how to calculate the cumulative sum (here, of `salary`)
when the data is partitioned (here, by `name`):

```kusto
datatable (name:string, month:int, salary:long)
[
    "Alice", 1, 1000,
    "Bob",   1, 1000,
    "Alice", 2, 2000,
    "Bob",   2, 1950,
    "Alice", 3, 1400,
    "Bob",   3, 1450,
]
| order by name asc, month asc
| extend total=row_cumsum(salary, name != prev(name))
```

name   | month  | salary  | total
-------|--------|---------|------
Alice  | 1      | 1000    | 1000
Alice  | 2      | 2000    | 3000
Alice  | 3      | 1400    | 4400
Bob    | 1      | 1000    | 1000
Bob    | 2      | 1950    | 2950
Bob    | 3      | 1450    | 4400