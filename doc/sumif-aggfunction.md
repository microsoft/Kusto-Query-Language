---
title: sumif() (aggregation function) - Azure Data Explorer | Microsoft Docs
description: This article describes sumif() (aggregation function) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# sumif() (aggregation function)

Returns a sum of *Expr* for which *Predicate* evaluates to `true`.

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

You can also use the [sum()](sum-aggfunction.md) function, which sums rows without predicate expression.

## Syntax

summarize `sumif(`*Expr*`,`*Predicate*`)`

## Arguments

* *Expr*: expression for aggregation calculation. 
* *Predicate*: predicate that, if true, the *Expr*'s calculated value will be added to the sum. 

## Returns

The sum value of *Expr* for which *Predicate* evaluates to `true`.

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
| summarize sumif(day_of_birth, strlen(name) > 4)
```

|sumif_day_of_birth|
|----|
|32|