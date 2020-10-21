---
title: ceiling() - Azure Data Explorer | Microsoft Docs
description: This article describes ceiling() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# ceiling()

Calculates the smallest integer greater than, or equal to, the specified numeric expression.

## Syntax

`ceiling(`*x*`)`

## Arguments

* *x*: A real number.

## Returns

* The smallest integer greater than, or equal to, the specified numeric expression. 

## Examples

```kusto
print c1 = ceiling(-1.1), c2 = ceiling(0), c3 = ceiling(0.9)
```

|c1|c2|c3|
|---|---|---|
|-1|0|1|