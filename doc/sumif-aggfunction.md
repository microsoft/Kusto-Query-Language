---
title: sumif() (aggregation function) - Azure Data Explorer | Microsoft Docs
description: This article describes sumif() (aggregation function) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# sumif() (aggregation function)

Returns a sum of *Expr* for which *Predicate* evaluates to `true`.

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

See also - [sum()](sum-aggfunction.md) function, which sums rows without predicate expression.

**Syntax**

summarize `sumif(`*Expr*`,`*Predicate*`)`

**Arguments**

* *Expr*: Expression that will be used for aggregation calculation. 
* *Predicate*: predicate that if true, the *Expr* calculated value will be added to the sum. 

**Returns**

The sum value of *Expr* for which *Predicate* evaluates to `true`.

> [!TIP]
> Use `summarize sumif(expr, filter)` instead of `where filter | summarize sum(expr)`