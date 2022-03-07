---
title: sum() (aggregation function) - Azure Data Explorer
description: This article describes sum() (aggregation function) in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 10/23/2018
---
# sum() (aggregation function)

Calculates the sum of *Expr* across the group. 

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

## Syntax

`sum` `(`*Expr*`)`

## Arguments

* *Expr*: Expression that will be used for aggregation calculation. 

## Returns

The sum value of *Expr* across the group.
 