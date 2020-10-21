---
title: log10() - Azure Data Explorer | Microsoft Docs
description: This article describes log10() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 08/11/2019
---
# log10()

`log10()` returns the common (base-10) logarithm function.  

## Syntax

`log10(`*x*`)`

## Arguments

* *x*: A real number > 0.

## Returns

* The common logarithm is the base-10 logarithm: the inverse of the exponential function (exp) with base 10.
* `null` if the argument is negative or null or can't be converted to a `real` value. 

## See also

* For natural (base-e) logarithms, see [log()](log-function.md).
* For base-2 logarithms, see [log2()](log2-function.md)