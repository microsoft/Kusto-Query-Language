---
title: Numerical operators - Azure Data Explorer | Microsoft Docs
description: This article describes Numerical operators in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# Numerical operators

The types `int`, `long`, and `real` represent numerical types.
The following operators can be used between pairs of these types:

Operator       |Description                         |Example
---------------|------------------------------------|-----------------------
`+`	           |Add                                 |`3.14 + 3.14`, `ago(5m) + 5m`
`-`	           |Subtract                            |`0.23 - 0.22`,
`*`            |Multiply                            |`1s * 5`, `2 * 2`
`/`	           |Divide                              |`10m / 1s`, `4 / 2`
`%`            |Modulo                              |`4 % 2`
`<`	           |Less                                |`1 < 10`, `10sec < 1h`, `now() < datetime(2100-01-01)`
`>`	           |Greater                             |`0.23 > 0.22`, `10min > 1sec`, `now() > ago(1d)`
`==`           |Equals                              |`1 == 1`
`!=`	       |Not equals                          |`1 != 0`
`<=`           |Less or Equal                       |`4 <= 5`
`>=`           |Greater or Equal                    |`5 >= 4`
`in`           |Equals to one of the elements       |[see here](inoperator.md)
`!in`          |Not equals to any of the elements   |[see here](inoperator.md)

> [!NOTE]
> To convert from one numerical type to another, use `to*()` functions. For example, see [`tolong()`](tolongfunction.md) and [`toint()`](tointfunction.md).

**Comment regarding the modulo operator**

The modulo of two numbers always returns in Kusto a "small non-negative number".
Thus, the modulo of two numbers, *N* % *D*, is such that:
0 &le; (*N* % *D*) &lt; abs(*D*).

For example, the following query:

```kusto
print plusPlus = 14 % 12, minusPlus = -14 % 12, plusMinus = 14 % -12, minusMinus = -14 % -12
```

Produces this result:

|plusPlus  | minusPlus  | plusMinus  | minusMinus|
|----------|------------|------------|-----------|
|2         | 10         | 2          | 10        |
