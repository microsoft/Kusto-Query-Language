---
title: make_bag_if() (aggregation function) - Azure Data Explorer
description: Learn how to use the make_bag_if() function to create a dynamic JSON property bag of expression values where the predicate evaluates to true.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/16/2022
---
# make_bag_if() (aggregation function)

Creates a `dynamic` JSON property bag (dictionary) of *Expr* values in records for which *Predicate* evaluates to `true`.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`make_bag_if` `(`*Expr*`,` *Predicate* [`,` *MaxSize*]`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| *Expr* | dynamic | &check; | Expression used for aggregation calculation. |
| *Predicate* | boolean | &check; | Predicate that evaluates to `true`, in order for *'Expr'* to be added to the result. |
| *MaxSize* | integer |  | Limit on the maximum number of elements returned. The default value is *1048576* and can't exceed 1048576. |

## Returns

Returns a `dynamic` JSON property bag (dictionary) of *Expr* values in records for which *Predicate* evaluates to `true`.
Non-dictionary values will be skipped.
If a key appears in more than one row, an arbitrary value, out of the possible values for this key, will be selected.

> [!NOTE]
> This function without the predicate is similar to [`make_bag`](./make-bag-aggfunction.md).

## Example

The following example shows a packed JSON property bag.

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA1WOTQuDMAyG7/0VwZNCD/u4OfwX3saQtEYpVi1tHGPsxy8bOlxyecOTFx5PDDVU0CLLGk95iHMoE0c39Rru6Bf6XSFS6ywylWaefaGuCmSyT+NwzDRk8t6gBI4L6R08bdBI6NCnP3reqF2r6nZRtXoBPZimFoL4BbTDV211KgSnZRwxuieBWHE14kCNwb5xXR52ssUbsXCntuQAAAA=)**\]**

```kusto
let T = datatable(prop:string, value:string, predicate:bool)
[
    "prop01", "val_a", true,
    "prop02", "val_b", false,
    "prop03", "val_c", true
];
T
| extend p = pack(prop, value)
| summarize dict=make_bag_if(p, predicate)
```

**Results**

|dict|
|----|
|{ "prop01": "val_a", "prop03": "val_c" } |

Use [bag_unpack()](bag-unpackplugin.md) plugin for transforming the bag keys in the make_bag_if() output into columns.

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA1WOywqDMBBF9/mKwZWCiz52Fv/CXSlhoqMEo4Y8Sin9+E5Ei002J5x7wzUUoIEaOgx8laHcusVWPjg9DyU80UT6vayjTrcYqFLLYgpxF8AnS43TOSsh47hEhuAilQd52aVi6NH4P3vdbbtVxeMmGvEBegWaO7C8z2I7rtO2TQVrH6cJnX4TKBzqCUeSDFL3uT1sTUlKHeYUlHFeP2MsvmCOilb+AAAA)**\]**

```kusto
let T = datatable(prop:string, value:string, predicate:bool)
[
    "prop01", "val_a", true,
    "prop02", "val_b", false,
    "prop03", "val_c", true
];
T
| extend p = pack(prop, value)
| summarize bag=make_bag_if(p, predicate)
| evaluate bag_unpack(bag)
```

**Results**

|prop01|prop03|
|---|---|
|val_a|val_c|
