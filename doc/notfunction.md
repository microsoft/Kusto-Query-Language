---
title: not() - Azure Data Explorer | Microsoft Docs
description: This article describes not() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# not()

Reverses the value of its `bool` argument.

```kusto
not(false) == true
```

**Syntax**

`not(`*expr*`)`

**Arguments**

* *expr*: A `bool` expression to be reversed.

**Returns**

Returns the reversed logical value of its `bool` argument.