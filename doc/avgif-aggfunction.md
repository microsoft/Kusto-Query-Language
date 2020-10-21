---
title: avgif() (aggregation function) - Azure Data Explorer | Microsoft Docs
description: This article describes avgif() (aggregation function) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# avgif() (aggregation function)

Calculates the [average](avg-aggfunction.md) of *Expr* across the group for which *Predicate* evaluates to `true`.

* Can only be used in context of aggregation inside [summarize](summarizeoperator.md)

## Syntax

summarize `avgif(`*Expr*`, `*Predicate*`)`

## Arguments

* *Expr*: Expression that will be used for aggregation calculation. Records with `null` values are ignored and not included in the calculation.
* *Predicate*:  predicate that if true, the *Expr* calculated value will be added to the average.

## Returns

The average value of *Expr* across the group where *Predicate* evaluates to `true`.
 
## Examples

```kusto
range x from 1 to 100 step 1
| summarize avgif(x, x%2 == 0)
```

|avgif_x|
|---|
|51|