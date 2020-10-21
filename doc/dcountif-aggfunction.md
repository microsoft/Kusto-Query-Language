---
title: dcountif() (aggregation function) - Azure Data Explorer | Microsoft Docs
description: This article describes dcountif() (aggregation function) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# dcountif() (aggregation function)

Returns an estimate of the number of distinct values of *Expr* of rows for which *Predicate* evaluates to `true`. 

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md).

Read about the [estimation accuracy](dcount-aggfunction.md#estimation-accuracy).

## Syntax

summarize `dcountif(`*Expr*, *Predicate*, [`,` *Accuracy*]`)`

## Arguments

* *Expr*: Expression that will be used for aggregation calculation.
* *Predicate*: Expression that will be used to filter rows.
* *Accuracy*, if specified, controls the balance between speed and accuracy.
    * `0` = the least accurate and fastest calculation. 1.6% error
    * `1` = the default, which balances accuracy and calculation time; about 0.8% error.
    * `2` = accurate and slow calculation; about 0.4% error.
    * `3` = extra accurate and slow calculation; about 0.28% error.
    * `4` = super accurate and slowest calculation; about 0.2% error.
	
## Returns

Returns an estimate of the number of distinct values of *Expr*  of rows for which *Predicate* evaluates to `true` in the group. 

## Example

```kusto
PageViewLog | summarize countries=dcountif(country, country startswith "United") by continent
```

**Tip: Offset error**

`dcountif()` might result in a one-off error in the edge cases where all rows
pass, or none of the rows pass, the `Predicate` expression