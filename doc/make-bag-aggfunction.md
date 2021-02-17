---
title: make_bag() (aggregation function) - Azure Data Explorer
description: This article describes the make_bag() aggregation function in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# make_bag() (aggregation function)

Returns a `dynamic` (JSON) property-bag (dictionary) of all the values of *`Expr`* in the group.

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

## Syntax

`summarize` `make_bag(`*`Expr`* [`,` *MaxSize*]`)`

## Arguments

* *Expr*: Expression of type `dynamic` that is used for aggregation calculations.
* *MaxSize* is an optional integer limit on the maximum number of elements returned. The default is *1048576*. MaxSize value can't exceed *1048576*.

> [!NOTE]
> `make_dictionary()` is a legacy and obsolete version of `make_bag()`. The legacy version has a default limit of *MaxSize* = 128.

## Returns

Returns a `dynamic` (JSON) property-bag (dictionary) of all the values of *`Expr`* in the group, which are property-bags.
Non-dictionary values will be skipped.
If a key appears in more than one row, an arbitrary value, out of the possible values for this key, will be selected.

## See also

Use the [bag_unpack()](bag-unpackplugin.md) plugin for expanding dynamic JSON objects into columns that use property bag keys. 

## Examples

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

|dict|
|----|
|{ "prop01": "val_a", "prop02": "val_b", "prop03": "val_c" } |

Use the [bag_unpack()](bag-unpackplugin.md) plugin for transforming the bag keys in the make_bag() output into columns. 

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

|prop01|prop02|prop03|
|---|---|---|
|val_a|val_b|val_c|
