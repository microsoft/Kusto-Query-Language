---
title: ago() - Azure Data Explorer | Microsoft Docs
description: This article describes ago() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
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

* *a_timespan*: Interval to subtract from the current UTC clock time
(`now()`).

## Returns

`now() - a_timespan`

## Example

All rows with a timestamp in the past hour:

```kusto
T | where Timestamp > ago(1h)
```