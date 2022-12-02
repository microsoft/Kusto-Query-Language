---
title: iif() - Azure Data Explorer
description: This article describes iif() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/27/2022
---
# iif()

Returns the value of *ifTrue* if *predicate* evaluates to `true`,
or the value of *ifFalse* otherwise.

## Syntax

`iif(`*predicate*`,` *ifTrue*`,` *ifFalse*`)`

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
| extend day = iif(floor(Timestamp, 1d)==floor(now(), 1d), "today", "anotherday")
```

An alias for [`iff()`](ifffunction.md).