---
title: sqrt() - Azure Data Explorer
description: This article describes sqrt() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 10/23/2018
---
# sqrt()

Returns the square root function.  

## Syntax

`sqrt(`*x*`)`

## Arguments

* *x*: A real number >= 0.

## Returns

* A positive number such that `sqrt(x) * sqrt(x) == x`
* `null` if the argument is negative or cannot be converted to a `real` value. 