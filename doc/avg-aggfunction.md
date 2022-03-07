---
title: avg() (aggregation function) - Azure Data Explorer
description: This article describes avg() (aggregation function) in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 09/26/2019
---
# avg() (aggregation function)

Calculates the average (arithmetic mean) of *Expr* across the group. 

* Can only be used in context of aggregation inside [summarize](summarizeoperator.md)

## Syntax

`avg` `(`*Expr*`)`

## Arguments

* *Expr*: Expression that will be used for aggregation calculation. Records with `null` values are ignored and not included in the calculation.

## Returns

The average value of *Expr* across the group.
 
