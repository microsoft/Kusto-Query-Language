---
title: isascii() - Azure Data Explorer | Microsoft Docs
description: This article describes isascii() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# isascii()

Returns `true` if the argument is a valid ascii string.
    
```kusto
isascii("some string") == true
```

## Syntax

`isascii(`[*value*]`)`

## Returns

Indicates whether the argument is a valid ascii string.

## Example

```kusto
T
| where isascii(fieldName)
| count
```