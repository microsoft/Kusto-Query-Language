---
title: hash_sha256() - Azure Data Explorer | Microsoft Docs
description: This article describes hash_sha256() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# hash_sha256()

Returns a sha256 hash value for the input value.

**Syntax**

`hash_sha256(`*source*`)`

**Arguments**

* *source*: The value to be hashed.

**Returns**

The sha256 hash value of the given scalar, encoded as a hex string (a string
of characters, each two of which represent a single Hex number between 0
and 255).

> [!WARNING]
> The algorithm used by this function (SHA256) is guaranteed
> to not be modified in the future, but is very complex to calculate. Users that
> need a "lightweight" hash function for the duration of a single query are advised
> to use the function [hash()](./hashfunction.md) instead.

**Examples**

```kusto
hash_sha256("World")                   // 78ae647dc5544d227130a0682a51e30bc7777fbb6d8a8f17007463a3ecd1d524
hash_sha256(datetime("2015-01-01"))    // e7ef5635e188f5a36fafd3557d382bbd00f699bd22c671c3dea6d071eb59fbf8
```

The following example uses the hash_sha256 function to run a query on StartTime column of the data

```kusto
StormEvents 
| where hash_sha256(StartTime) == 0
| summarize StormCount = count(), TypeOfStorms = dcount(EventType) by State 
| top 5 by StormCount desc
```