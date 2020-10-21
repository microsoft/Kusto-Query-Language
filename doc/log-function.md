---
title: log() - Azure Data Explorer | Microsoft Docs
description: This article describes log() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 08/11/2019
---
# log()

`log()` returns the natural logarithm function.  

## Syntax

`log(`*x*`)`

## Arguments

* *x*: A real number > 0.

## Returns

* The natural logarithm is the base-e logarithm: the inverse of the natural exponential function (exp).
* `null` if the argument is negative or null or can't be converted to a `real` value. 

## See also

* For common (base-10) logarithms, see [log10()](log10-function.md).
* For base-2 logarithms, see [log2()](log2-function.md)