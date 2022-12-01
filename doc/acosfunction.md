---
title: acos() - Azure Data Explorer
description: Learn how to use the function acos() to calculate the angle of the cosine input.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/20/2022
---
# acos()

Calculates the angle whose cosine is the specified number. Inverse operation of [`cos()`](cosfunction.md).

## Syntax

`acos(`*x*`)`

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
| *x* | real | &check; | The value used to calculate the arc cosine. |

## Returns

The value of the arc cosine of `x`. The return value is `null` if `x` < -1 or `x` > 1.
