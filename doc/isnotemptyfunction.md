---
title: isnotempty() - Azure Data Explorer
description: This article describes isnotempty() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# isnotempty()

Returns `true` if the argument isn't an empty string, and it isn't null.

```kusto
isnotempty("") == false
```

## Syntax

`isnotempty(`[*value*]`)`

`notempty(`[*value*]`)` -- alias of `isnotempty`

## Example

```kusto
T
| where isnotempty(fieldName)
| count
```
