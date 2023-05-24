---
title:  row_cumsum()
description: Learn how to use the row_cumsum() function to calculate the cumulative sum of a column in a serialized row set.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/19/2023
---
# row_cumsum()

Calculates the cumulative sum of a column in a [serialized row set](./windowsfunctions.md#serialized-row-set).

## Syntax

`row_cumsum(` *term* [`,` *restart*] `)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *term*| int, long, or real | &check; | The expression indicating the value to be summed.|
| *restart*| bool | | Indicates when the accumulation operation should be restarted, or set back to 0. It can be used to indicate partitions in the data.|

## Returns

The function returns the cumulative sum of its argument.

## Examples

The following example shows how to calculate the cumulative sum of the first
few even integers.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAw3ITQqDMBBA4X1O8TYFhVmo1f5BTlJKGXVohaiQVITSwzfv271RP1kfjEJvhHV5ldwduVpohKPQCp1wEs7CRbgKdeUe7sf+tmgoBxq8p8orWZw0TF9jSD6u+3PY5rTNhZZ/2Wk/Y2kAAAA=" target="_blank">Run the query</a>

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

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA2WPSw6CQAxA95yisoJkFuBnIQkLvYYxZgYaJJmPmSkqiYe3A7qBtovm9TVpW0mcSiNkVhqsAvnedgKMs3SveksCgtTSj5V2tsuTSwIc6Un3DaYCSq6iKMRMz04xgwX9u1uutcu0PB6W7o7pfuVONLrX5APOt+hBjRDvBhma39Gx5TG+CW0L5Ejq2rvXrRlMGEw2fyPmrU0ND4/P6fU8/wKEiBU0DAEAAA==" target="_blank">Run the query</a>

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
