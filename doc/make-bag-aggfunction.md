---
title: make_bag() (aggregation function) - Azure Data Explorer
description: This article describes the make_bag() aggregation function in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 08/24/2022
---
# make_bag() (aggregation function)

Creates a `dynamic` JSON property bag (dictionary) of all the values of *`Expr`* in the group.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

 `make_bag` `(`*Expr* [`,` *MaxSize*]`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| *Expr* | dynamic | &check; | Expression used for aggregation calculations. |
| *MaxSize* | integer |  | The limit on the maximum number of elements returned. The default is *1048576* and can't exceed *1048576*. |

> [!NOTE]
> `make_dictionary()` has been deprecated in favor of `make_bag()`. The legacy version has a default *MaxSize* limit of 128.

## Returns

Returns a `dynamic` JSON property bag (dictionary) of all the values of *`Expr`* in the group, which are property bags.
Non-dictionary values will be skipped.
If a key appears in more than one row, an arbitrary value, out of the possible values for this key, will be selected.

## Example

The following example shows a packed JSON property bag.

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVEIUbBVSEksAcKknFSNgqL8AqvikqLMvHQdhbLEnNJUKE+TK5pLAQiUQCoMDJV0FJSA0vGJSjpIwkYw4SQUYWOYcDJQONaaK4SrRiG1oiQ1L0WhAGh7QWJyNthiqI2aQOni0tzcxKLMqlSFlMzkEtvcxOzU+KTEdI0CTQBPpqLVtAAAAA==)**\]**

```kusto
let T = datatable(prop:string, value:string)
[
    "prop01", "val_a",
    "prop02", "val_b",
    "prop03", "val_c",
];
T
| extend p = pack(prop, value)
| summarize dict=make_bag(p)
```

**Results**

|dict|
|----|
|{ "prop01": "val_a", "prop02": "val_b", "prop03": "val_c" } |

Use the [bag_unpack()](bag-unpackplugin.md) plugin for transforming the bag keys in the make_bag() output into columns.

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA1WNvQrDMAyEdz+F8BSDh/5sLXmLbKUYOREhxHZNYpdS+vCVTQKttJy+O3GOEnTQwoCJ1zpq4vKIlzUtUxg1PNFl2i4lbgJ4ZEkcjlKDZNug1D/4tGP7h8877hnfr6ITH6BXojBA5PaI/VyLt0bF9pq9x2V6E1gcW48zGRZNLB6VFKZqmRzqO0v1BTGl9vXOAAAA)**\]**

```kusto
let T = datatable(prop:string, value:string)
[
    "prop01", "val_a",
    "prop02", "val_b",
    "prop03", "val_c",
];
T
| extend p = pack(prop, value)
| summarize bag=make_bag(p)
| evaluate bag_unpack(bag)
```

**Results**

|prop01|prop02|prop03|
|---|---|---|
|val_a|val_b|val_c|

## See also

[bag_unpack()](bag-unpackplugin.md)
