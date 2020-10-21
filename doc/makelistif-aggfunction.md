---
title: make_list_if() (aggregation function) - Azure Data Explorer | Microsoft Docs
description: This article describes make_list_if() (aggregation function) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# make_list_if() (aggregation function)

Returns a `dynamic` (JSON) array of all the values of *Expr* in the group, for which *Predicate* evaluates to `true`.

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

## Syntax

`summarize` `make_list_if(`*Expr*, *Predicate* [`,` *MaxSize*]`)`

## Arguments

* *Expr*: Expression that will be used for aggregation calculation.
* *Predicate*: Predicate that has to evaluate to `true`, in order for *Expr* to be added to the result.
* *MaxSize* is an optional integer limit on the maximum number of elements returned (default is *1048576*). MaxSize value cannot exceed 1048576.

## Returns

Returns a `dynamic` (JSON) array of all the values of *Expr* in the group, for which *Predicate* evaluates to `true`.
If the input to the `summarize` operator is not sorted, the order of elements in the resulting array is undefined.
If the input to the `summarize` operator is sorted, the order of elements in the resulting array tracks that of the input.

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
| summarize make_list_if(name, strlen(name) > 4)
```

|list_name|
|----|
|["George", "Ringo"]|

## See also

[`make_list`](./makelist-aggfunction.md) function, which does the same, without predicate expression.