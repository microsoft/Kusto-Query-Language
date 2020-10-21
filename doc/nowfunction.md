---
title: now() - Azure Data Explorer | Microsoft Docs
description: This article describes now() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# now()

Returns the current UTC clock time, optionally offset by a given timespan.
This function can be used multiple times in a statement and the clock time being referenced will be the same for all instances.

```kusto
now()
now(-2d)
```

## Syntax

`now(`[*offset*]`)`

## Arguments

* *offset*: A `timespan`, added to the current UTC clock time. Default: 0.

## Returns

The current UTC clock time as a `datetime`.

`now()` + *offset* 

## Example

Determines the interval since the event identified by the predicate:

```kusto
T | where ... | extend Elapsed=now() - Timestamp
```