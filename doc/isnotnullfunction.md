---
title: isnotnull() - Azure Data Explorer | Microsoft Docs
description: This article describes isnotnull() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# isnotnull()

Returns `true` if the argument is not null.

## Syntax

`isnotnull(`[*value*]`)`

`notnull(`[*value*]`)` - alias for `isnotnull`

## Example

```kusto
T | where isnotnull(PossiblyNull) | count
```
