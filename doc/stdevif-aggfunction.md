---
title: stdevif() (aggregation function) - Azure Data Explorer | Microsoft Docs
description: This article describes stdevif() (aggregation function) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# stdevif() (aggregation function)

Calculates the [stdev](stdev-aggfunction.md) of *Expr* across the group for which *Predicate* evaluates to `true`.

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

## Syntax

summarize `stdevif(`*Expr*`, `*Predicate*`)`

## Arguments

* *Expr*: Expression that will be used for aggregation calculation. 
* *Predicate*:  predicate that if true, the *Expr* calculated value will be added to the standard deviation.

## Returns

The standard deviation value of *Expr* across the group where *Predicate* evaluates to `true`.
 
## Examples

```kusto
range x from 1 to 100 step 1
| summarize stdevif(x, x%2 == 0)

```

|stdevif_x|
|---|
|29.1547594742265|