---
title: make_list() (aggregation function) - Azure Data Explorer
description: Learn how to use the make_list() function to create a dynamic JSON object array of all the values of the expressions in the group.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/05/2023
adobe-target: true
---
# make_list() (aggregation function)

Creates a `dynamic` array of all the values of *expr* in the group.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

> **Deprecated aliases:** makelist()

## Syntax

`make_list(`*expr* [`,` *maxSize*]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *expr* | dynamic | &check; | The expression used for the aggregation calculation. |
| *maxSize* | int |  | The maximum number of elements returned. The default and max value is 1048576. |

> [!NOTE]
> The deprecated version has a default *maxSize* limit of 128.

## Returns

Returns a `dynamic` array of all the values of *expr* in the group.
If the input to the `summarize` operator isn't sorted, the order of elements in the resulting array is undefined.
If the input to the `summarize` operator is sorted, the order of elements in the resulting array tracks that of the input.

> [!TIP]
> Use the [`array_sort_asc()`](./arraysortascfunction.md) or [`array_sort_desc()`](./arraysortdescfunction.md) function to create an ordered list by some key.

## Examples

### One column

The following example makes a list out of a single column:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0XPzwrCMAwG8Huh7xB2crCD4v+JJx9DROIWtmKbzrYDFR/eTldNLuGX7/JpCuBb7MjDHmoMcS+aYMJoqAQfnOKmAK9qOtieQwmKQy7FUQqIk8U/cqMpK2BejOZvPbpBFkkcVSHFftgRB2wsR1sma+k+0upPXYqtk9kq0SYRWx5pm6imaqTZVIrTTopvUSle4Htj0KkngXlo5UMsb/BK5+H+dM/ff3I+dBkBAAA=" target="_blank">Run the query</a>

```kusto
let shapes = datatable (name: string, sideCount: int)
[
    "triangle", 3,
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
| summarize mylist = make_list(name)
```

**Output**

|mylist|
|---|
|["triangle","square","rectangle","pentagon","hexagon","heptagon","octagon","nonagon","decagon"]|

### Using the 'by' clause

The following example runs a query using the `by` clause:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0XQ3YrCMBAF4PtA3uEgCCv0wp/d9Y9eiU/gpYjEdqjBZlqbVFR8eKPbcSc3w5dzc6akAH80NXmkyE2I71ASvtg4WsCHxnKRwNucVlXLYQHLYaDVVivE6cV/w0VJvQSTpDN/bk3zkm+RhrIgsQ/WxMEUFUf7ETvStaPff6olNhWrMqGZEFfc0Vwop6yj0VCr3VKrv6JaPeBb50xj7wR3K60PsbwzJ9q/9nf3AQ43WL++EG+kfAx9DoE+xkhTDJ/VCjwFQQEAAA==" target="_blank">Run the query</a>

```kusto
let shapes = datatable (name: string, sideCount: int)
[
    "triangle", 3,
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
| summarize mylist = make_list(name) by isEvenSideCount = sideCount % 2 == 0
```

**Output**

|isEvenSideCount| mylist|
|---|---|
|false|["triangle","pentagon","heptagon","nonagon"]|
|true|["square","rectangle","hexagon","octagon","decagon"]|

### Packing a dynamic object

The following examples show how to [pack](./packfunction.md) a dynamic object in a column before making it a list.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA03Pz0rEMBAG8Hsg7zAUhBZ6WP/rSk/iE3gUWabN0A3bTGuTyq748E6ws5ocEn75JvANlCDucaIIDThMstuBoGQMtIWYZs99DdE7eh4XTlvwnCpr3qwBWYW8I/cDFTVc16vFjwXnLDcqM3VJY2eciBP2I4vdqu3puNLdH00au1cbO6UHJR55pUclR91Klxtr3p+s+S1qzTfQMRE7cFK6xX43YXcoi9xZ0vmopYZ2Lv71r/JwXELA2X8RhNPgY5JPAh5ol++lq6A9gY8vn8SvOiaJ8xdwAVfQNLD5ARd0KSV7AQAA" target="_blank">Run the query</a>

```kusto
let shapes = datatable (name: string, sideCount: int)
[
    "triangle", 3,
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
| extend d = bag_pack("name", name, "sideCount", sideCount)
| summarize mylist = make_list(d) by isEvenSideCount = sideCount % 2 == 0
```

**Output**

|isEvenSideCount|mylist|
|---|---|
|false|[{"name":"triangle","sideCount":3},{"name":"pentagon","sideCount":5},{"name":"heptagon","sideCount":7},{"name":"nonagon","sideCount":9}]|
|true|[{"name":"square","sideCount":4},{"name":"rectangle","sideCount":4},{"name":"hexagon","sideCount":6},{"name":"octagon","sideCount":8},{"name":"decagon","sideCount":10}]|

## See also

[`make_list_if`](./makelistif-aggfunction.md) operator is similar to `make_list`, except it also accepts a predicate.
