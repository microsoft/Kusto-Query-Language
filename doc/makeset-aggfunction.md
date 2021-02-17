---
title: make_set() (aggregation function) - Azure Data Explorer | Microsoft Docs
description: This article describes make_set() (aggregation function) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 01/23/2020
---
# make_set() (aggregation function)

Returns a `dynamic` (JSON) array of the set of distinct values that *Expr* takes in the group.

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

## Syntax

`summarize` `make_set(`*Expr* [`,` *MaxSize*]`)`

## Arguments

* *Expr*: Expression for aggregation calculation.
* *MaxSize* is an optional integer limit on the maximum number of elements returned (default is *1048576*). MaxSize value cannot exceed 1048576.

> [!NOTE]
> `makeset()` is a legacy and obsolete version of the function `make_set`. The legacy version has a default limit of *MaxSize* = 128.

## Returns

Returns a `dynamic` (JSON) array of the set of distinct values that *Expr* takes in the group.
The array's sort order is undefined.

> [!TIP]
> To only count distinct values, use [dcount()](dcount-aggfunction.md)

## Example

```kusto
PageViewLog 
| summarize countries=make_set(country) by continent
```

:::image type="content" source="images/makeset-aggfunction/makeset.png" alt-text="Table showing Kusto Query summarize countries by continent in Azure Data Explorer":::

## See also

* Use [`mv-expand`](./mvexpandoperator.md) operator for the opposite function.
* [`make_set_if`](./makesetif-aggfunction.md) operator is similar to `make_set`, except it also accepts a predicate.
