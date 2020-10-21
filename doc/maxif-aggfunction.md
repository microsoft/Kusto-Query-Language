---
title: maxif() (aggregation function) - Azure Data Explorer | Microsoft Docs
description: This article describes maxif() (aggregation function) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# maxif() (aggregation function)

Returns the maximum value across the group for which *Predicate* evaluates to `true`.

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

See also - [max()](max-aggfunction.md) function, which returns the maximum value across the group without predicate expression.

## Syntax

`summarize` `maxif(`*Expr*`,`*Predicate*`)`

## Arguments

* *Expr*: Expression that will be used for aggregation calculation. 
* *Predicate*: predicate that if true, the *Expr* calculated value will be checked for maximum.

## Returns

The maximum value of *Expr* across the group for which *Predicate* evaluates to `true`.

## Examples

```kusto
range x from 1 to 100 step 1
| summarize maxif(x, x < 50)
```

|maxif_x|
|---|
|49|