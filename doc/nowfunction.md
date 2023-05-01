---
title: now() - Azure Data Explorer
description: Learn how to use the now() function to return the current UTC time.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/08/2023
---
# now()

Returns the current UTC time, optionally offset by a given [timespan](scalar-data-types/timespan.md).

The current UTC time will stay the same across all uses of `now()` in a single query statement, even if there's technically a small time difference between when each `now()` runs.

## Syntax

`now(`[ *offset* ]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *offset* | timespan | | A timespan to add to the current UTC clock time. The default value is 0.|

## Returns

The current UTC clock time, plus the *offset* time if provided, as a `datetime`.

## Example

The following example determines the interval since the storm events.

```kusto
T | where ... | extend Elapsed=now() - Timestamp
```
