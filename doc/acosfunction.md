---
title: acos() - Azure Data Explorer
description: This article describes acos() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 10/23/2018
---
# acos()

Returns the angle whose cosine is the specified number (the inverse operation of [`cos()`](cosfunction.md)).

## Syntax

`acos(`*x*`)`

## Arguments

* *x*: A real number in range [-1, 1].

## Returns

* The value of the arc cosine of `x`
* `null` if `x` < -1 or `x` > 1
