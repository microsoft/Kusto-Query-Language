---
title: isutf8() - Azure Data Explorer | Microsoft Docs
description: This article describes isutf8() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/31/2018
---
# isutf8()

Returns `true` if the argument is a valid utf8 string.
    
```kusto
isutf8("some string") == true
```

**Syntax**

`isutf8(`[*value*]`)`

**Returns**

Indicates whether the argument is a valid utf8 string.

**Example**

```kusto
T
| where isutf8(fieldName)
| count
```