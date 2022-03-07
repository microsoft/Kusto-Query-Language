---
title: minif() (aggregation function) - Azure Data Explorer
description: This article describes minif() (aggregation function) in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/13/2020
---
# minif() (aggregation function)

Returns the minimum value across the group for which *Predicate* evaluates to `true`.

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

See also - [min()](min-aggfunction.md) function, which returns the minimum value across the group without predicate expression.

## Syntax

 `minif` `(`*Expr*`,`*Predicate*`)`

## Arguments

* *Expr*: Expression that will be used for aggregation calculation.
* *Predicate*: predicate that if true, the *Expr* calculated value will be checked for minimum.

## Returns

The minimum value of *Expr* across the group for which *Predicate* evaluates to `true`.

## Examples

```kusto
range x from 1 to 100 step 1
| summarize minif(x, x > 50)
```

|minif_x|
|---|
|51|