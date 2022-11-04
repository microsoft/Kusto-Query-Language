---
title: acos() - Azure Data Explorer
description: Learn how to use the function acos() to calculate the angle of the cosine input.
ms.reviewer: alexans
ms.topic: reference
ms.date: 09/20/2022
---
# acos()

Calculates the angle whose cosine is the specified number (the inverse operation of [`cos()`](cosfunction.md)).

## Syntax

`acos(`*x*`)`

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
| *x* | real | &check; | A real number in range [-1, 1]. |

## Returns

* The value of the arc cosine of `x`
* `null` if `x` < -1 or `x` > 1
