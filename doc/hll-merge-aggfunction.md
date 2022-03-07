---
title: hll_merge() (aggregation function) - Azure Data Explorer
description: This article describes hll_merge() (aggregation function) in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 04/15/2019
---
# hll_merge() (aggregation function)

Merges `HLL` results across the group into a single `HLL` value.

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md).

For more information, see the [underlying algorithm (*H*yper*L*og*L*og) and estimation accuracy](dcount-aggfunction.md#estimation-accuracy).

## Syntax

`hll_merge` `(`*Expr*`)`

## Arguments

* *Expr*: Expression that will be used for the aggregation calculation.

## Returns

The function returns the merged `hll` values of `*Expr*` across the group.
 
**Tips**

1) Use the function [dcount_hll](dcount-hllfunction.md) to calculate the `dcount` from `hll` / `hll-merge` aggregation functions.
