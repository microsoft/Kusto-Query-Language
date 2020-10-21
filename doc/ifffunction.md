---
title: iff() - Azure Data Explorer | Microsoft Docs
description: This article describes iff() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# iff()

Evaluates the first argument (the predicate), and returns the value of either the second or third arguments, depending on whether the predicate evaluated to `true` (second) or `false` (third).

The second and third arguments must be of the same type.

## Syntax

`iff(`*predicate*`,` *ifTrue*`,` *ifFalse*`)`

## Arguments

* *predicate*: An expression that evaluates to a `boolean` value.
* *ifTrue*: An expression that gets evaluated and its value returned from the function if *predicate* evaluates to `true`.
* *ifFalse*: An expression that gets evaluated and its value returned from the function if *predicate* evaluates to `false`.

## Returns

This function returns the value of *ifTrue* if *predicate* evaluates to `true`,
or the value of *ifFalse* otherwise.

## Example

```kusto
T 
| extend day = iff(floor(Timestamp, 1d)==floor(now(), 1d), "today", "anotherday")
```