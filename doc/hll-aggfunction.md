---
title: hll() (aggregation function) - Azure Data Explorer | Microsoft Docs
description: This article describes hll() (aggregation function) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 04/15/2019
---
# hll() (aggregation function)

Calculates the Intermediate results of [dcount](dcount-aggfunction.md) across the group. 

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md).

Read about the [underlying algorithm (*H*yper*L*og*L*og) and estimation accuracy](dcount-aggfunction.md#estimation-accuracy).

**Syntax**

`summarize` `hll(`*Expr* [`,` *Accuracy*]`)`

**Arguments**

* *Expr*: Expression that will be used for aggregation calculation. 
* *Accuracy*, if specified, controls the balance between speed and accuracy.
    * `0` = the least accurate and fastest calculation. 1.6% error
    * `1` = the default, which balances accuracy and calculation time; about 0.8% error.
    * `2` = accurate and slow calculation; about 0.4% error.
    * `3` = extra accurate and slow calculation; about 0.28% error.
    * `4` = super accurate and slowest calculation; about 0.2% error.
	
**Returns**

The Intermediate results of distinct count of *Expr* across the group.
 
**Tips**

1) You may use the aggregation function [hll_merge](hll-merge-aggfunction.md) to merge more than one hll intermediate results (it works on hll output only).

2) You may use the function [dcount_hll] (dcount-hllfunction.md) which will calculate the dcount from hll / hll_merge aggregation functions.

**Examples**

```kusto
StormEvents
| summarize hll(DamageProperty) by bin(StartTime,10m)

```

|StartTime|hll_DamageProperty|
|---|---|
|2007-09-18 20:00:00.0000000|[[1024,14],[-5473486921211236216,-6230876016761372746,3953448761157777955,4246796580750024372],[]]|
|2007-09-20 21:50:00.0000000|[[1024,14],[4835649640695509390],[]]|
|2007-09-29 08:10:00.0000000|[[1024,14],[4246796580750024372],[]]|
|2007-12-30 16:00:00.0000000|[[1024,14],[4246796580750024372,-8936707700542868125],[]]|