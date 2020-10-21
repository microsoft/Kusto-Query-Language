---
title: make_set_if() (aggregation function) - Azure Data Explorer | Microsoft Docs
description: This article describes make_set_if() (aggregation function) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# make_set_if() (aggregation function)

Returns a `dynamic` (JSON) array of the set of distinct values that *Expr* takes in the group, for which *Predicate* evaluates to `true`.

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

## Syntax

`summarize` `make_set_if(`*Expr*, *Predicate* [`,` *MaxSize*]`)`

## Arguments

* *Expr*: Expression that will be used for aggregation calculation.
* *Predicate*: Predicate that has to evaluate to `true` for *Expr* to be added to the result.
* *MaxSize* is an optional integer limit on the maximum number of elements returned (default is *1048576*). MaxSize value cannot exceed 1048576.

## Returns

Returns a `dynamic` (JSON) array of the set of distinct values that *Expr* takes in the group, for which *Predicate* evaluates to `true`.
The array's sort order is undefined.

> [!TIP]
> To only count the distinct values, use [dcountif()](dcountif-aggfunction.md)

## See also

[`make_set`](./makeset-aggfunction.md) function, which does the same, without predicate expression.

## Example

```kusto
let T = datatable(name:string, day_of_birth:long)
[
   "John", 9,
   "Paul", 18,
   "George", 25,
   "Ringo", 7
];
T
| summarize make_set_if(name, strlen(name) > 4)
```

|set_name|
|----|
|["George", "Ringo"]|