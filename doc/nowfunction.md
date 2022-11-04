---
title: now() - Azure Data Explorer
description: This article describes now() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/13/2020
---
# now()

Returns the current UTC time, optionally offset by a given [timespan](scalar-data-types/timespan.md).

The current UTC time will stay the same across all uses of `now()` in a single query statement, even if there's technically a small time difference between when each `now()` runs.

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