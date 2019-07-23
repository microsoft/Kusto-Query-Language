---
title: isnotnull() - Azure Data Explorer | Microsoft Docs
description: This article describes isnotnull() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# isnotnull()

Returns `true` if the argument is not null.

**Syntax**

`isnotnull(`[*value*]`)`

`notnull(`[*value*]`)` - alias for `isnotnull`

**Example**

```kusto
T | where isnotnull(PossiblyNull) | count
```

Notice that there are other ways of achieving this effect:

```kusto
T | summarize count(PossiblyNull)
```