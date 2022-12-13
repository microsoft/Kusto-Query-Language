---
title: make_list_with_nulls() (aggregation function) - Azure Data Explorer
description: Learn how to use the make_list_with_nulls() aggregation function to create a `dynamic` JSON object (array) which includes null values in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 09/20/2022
---
# make_list_with_nulls() (aggregation function)

Creates a `dynamic` JSON object (array) of all the values of *Expr* in the group, including null values.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`make_list_with_nulls` `(` *Expr* `)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| *Expr* | string | &check; | Expression that will be used for aggregation calculation. |

## Returns

Returns a `dynamic` JSON object (array) of all the values of *Expr* in the group, including null values.
If the input to the `summarize` operator is not sorted, the order of elements in the resulting array is undefined.
If the input to the `summarize` operator is sorted, the order of elements in the resulting array tracks that of the input.

> [!TIP]
> Use the [`array_sort_asc()`](./arraysortascfunction.md) or [`array_sort_desc()`](./arraysortdescfunction.md) function to create an ordered list by some key.

## Example

The following example shows null values in the results.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAz3PywqDMBAF0H2+YnCl4KKFPi1d9TNKkVQHDU0mNhnpg358E4xmNuHcu7kaGXwvB/RwhlZyuLtGyEkarDw7RR2U4FWLFzsSV6CIC3EVEF4WYkmdxqyMnNOodVFOkX+O0sVgk8Bhw3N5tgGJZWcp0DZRj+8ku0WGubRPZBu2kxySkKXUOSZpsUmyXonbSUwjxQ/8aIx06otgPlp5DruNfGAd//VLcV/HHT5fNhd/aMS6MiQBAAA=" target="_blank">Run the query</a>

```kusto
let shapes = datatable (name:string , sideCount: int)
[
    "triangle", int(null),
    "square", 4,
    "rectangle", 4,
    "pentagon", 5,
    "hexagon", 6,
    "heptagon", 7,
    "octagon", 8,
    "nonagon", 9,
    "decagon", 10
];
shapes
| summarize mylist = make_list_with_nulls(sideCount)
```

**Results**

| mylist |
| ------ |
| [null,4,4,5,6,7,8,9,10] |
