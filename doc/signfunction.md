---
title: sign() - Azure Data Explorer | Microsoft Docs
description: This article describes sign() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# sign()

Sign of a numeric expression

**Syntax**

`sign(`*x*`)`

**Arguments**

* *x*: A real number.

**Returns**

* The positive (+1), zero (0), or negative (-1) sign of the specified expression. 

**Examples**

```kusto
print s1 = sign(-42), s2 = sign(0), s3 = sign(11.2)

```

|s1|s2|s3|
|---|---|---|
|-1|0|1|