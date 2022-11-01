---
title: ago() - Azure Data Explorer
description: Learn how to use the ago() function to subtract a given timespan from the current UTC clock time.
ms.reviewer: alexans
ms.topic: reference
ms.date: 09/20/2022
---
# ago()

Subtracts the given timespan from the current UTC clock time.

```kusto
ago(1h)
ago(1d)
```

Like `now()`, this function can be used multiple times
in a statement and the UTC clock time being referenced will be the same
for all instantiations.

## Syntax

`ago(`*a_timespan*`)`

## Arguments

* *[a_timespan](scalar-data-types/timespan.md)*: Interval to subtract from the current UTC clock time
(`now()`).

## Returns

`now() - a_timespan`

## Example

All rows with a timestamp in the past hour:

```kusto
T | where Timestamp > ago(1h)
```
