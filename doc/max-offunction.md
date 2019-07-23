---
title: max_of() - Azure Data Explorer | Microsoft Docs
description: This article describes max_of() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# max_of()

Returns the maximum value of several evaluated numeric expressions.

```kusto
max_of(10, 1, -3, 17) == 17
```

**Syntax**

`max_of` `(`*expr_1*`,` *expr_2* ...`)`

**Arguments**

* *expr_i*: A scalar expression, to be evaluated.

- All arguments must be of the same type.
- Maximum of 64 arguments is supported.

**Returns**

The maximum value of all argument expressions.

**Example**

```kusto
print result = max_of(10, 1, -3, 17) 
```

|result|
|---|
|17|