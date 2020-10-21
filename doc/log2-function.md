---
title: log2() - Azure Data Explorer | Microsoft Docs
description: This article describes log2() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 08/11/2019
---
# log2()

`log2()` returns the base-2 logarithm function.  

## Syntax

`log2(`*x*`)`

## Arguments

* *x*: A real number > 0.

## Returns

* The logarithm is the base-2 logarithm: the inverse of the exponential function (exp) with base 2.
* `null` if the argument is negative or null or can't be converted to a `real` value. 

## See also

* For natural (base-e) logarithms, see [log()](log-function.md).
* For common (base-10) logarithms, see [log10()](log10-function.md).