---
title: log10() - Azure Data Explorer | Microsoft Docs
description: This article describes log10() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# log10()

Returns the common (base-10) logarithm function.  

**Syntax**

`log10(`*x*`)`

**Arguments**

* *x*: A real number > 0.

**Returns**

* The common logarithm is the base-10 logarithm: the inverse of the exponential function (exp) with base 10.
* For natural (base-e) logarithms, see [log()](log-function.md).
* For base-2 logarithms, see [log2()](log2-function.md)
* `null` if the argument is negative or null or cannot be converted to a `real` value. 