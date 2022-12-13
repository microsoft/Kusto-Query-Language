---
title: make_list() (aggregation function) - Azure Data Explorer
description: This article describes make_list() (aggregation function) in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/09/2022
adobe-target: true
---
# make_list() (aggregation function)

Creates a `dynamic` JSON object (array) of all the values of *Expr* in the group.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

> **Deprecated aliases:** makelist()

## Syntax

`make_list` `(`*Expr* [`,` *MaxSize*]`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| *Expr* | dynamic | &check; | Expression used for aggregation calculations. |
| *MaxSize* | integer |  | The limit on the maximum number of elements returned. The default is *1048576* and can't exceed *1048576*. |

> [!NOTE]
> The deprecated version has a default *MaxSize* limit of 128.

## Returns

Returns a `dynamic` JSON array of all the values of *Expr* in the group.
If the input to the `summarize` operator isn't sorted, the order of elements in the resulting array is undefined.
If the input to the `summarize` operator is sorted, the order of elements in the resulting array tracks that of the input.

> [!TIP]
> Use the [`array_sort_asc()`](./arraysortascfunction.md) or [`array_sort_desc()`](./arraysortdescfunction.md) function to create an ordered list by some key.

## Examples

### One column

The following example makes a list out of a single column:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAz3PzwrCMAwG8HufIuzkYAfF/xNPPoaI1C1sxTadbQYqPrydy0wu4Zfv8llkiK3uMMIRas1pbxZhRtphCZGDoaaAaGo8+Z64BEOcq7OCNFn6amosZgUsi5Hio9dhgJVAwIqn0GQdEuvGU6K1UItPkc1fuim0FfLVJDsR8iSyF6mxElnM1eWgxnLqA7F3TgfzRnAvayKnvk7f8Trcv7r5F8QGBpEMAQAA" target="_blank">Run the query</a>

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

**Results**

|mylist|
|---|
|["triangle","square","rectangle","pentagon","hexagon","heptagon","octagon","nonagon","decagon"]|

### Using the 'by' clause

The following example runs a query using the `by` clause:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAz3Py26DMBCF4b2f4ggpUiOxIEnTCxWrKE+QZRVVDoyIVTwmeKhK1YePCQZ7Y30+m78hgb/qljwKVFrCvTSEJ9aWcnjpDNcpvKno4HqWHIZlrT4VwknCr+a6oSTFLp3I33rdjfAcoaNS5tFsLbHo2nGgfaQr/UZ5WaSdR6+RXClukrco7Dhu3qNUVEbZZOr8oaY49Q/fW6s780ewQ2O8hF6rv+lrfD9y17gMMP74Q3yae8NoaccKWxQFsjvUEHjHNAEAAA==" target="_blank">Run the query</a>

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

**Results**

|isEvenSideCount| mylist|
|---|---|
|false|["triangle","pentagon","heptagon","nonagon"]|
|true|["square","rectangle","hexagon","octagon","decagon"]|

### Packing a dynamic object

The following examples show how to [pack](./packfunction.md) a dynamic object in a column before making it a list.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA03PTWrDMBAF4L1OMRgKNniR/qcpXoWeIMtSysQaHBFr5FjjkJQevmMshVoLi09vhF5PAvGAA0VowKLo2vcEJaOnDUQZHXc1RGdpGyaWDTiWynwa0K/QU+Sup6KGx3qheJpwnOEpwUit5FC2gViwC6z0nOhAlyQvNxly6DVRaCUssk7CgVPmLYmlNsn9yny9m6Wc+QW6CLEFqz0HbI9lMVfU2Pyr9eG5YvGvbqVzcfIeR/dD4K+9i6LzHo/0Pe9LW8H+Ci5+nIl3eUoTtxvgDh6gaWD1B75NBjppAQAA" target="_blank">Run the query</a>

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

**Results**

|mylist|isEvenSideCount|
|---|---|
|false|[{"name":"triangle","sideCount":3},{"name":"pentagon","sideCount":5},{"name":"heptagon","sideCount":7},{"name":"nonagon","sideCount":9}]|
|true|[{"name":"square","sideCount":4},{"name":"rectangle","sideCount":4},{"name":"hexagon","sideCount":6},{"name":"octagon","sideCount":8},{"name":"decagon","sideCount":10}]|

## See also

[`make_list_if`](./makelistif-aggfunction.md) operator is similar to `make_list`, except it also accepts a predicate.
