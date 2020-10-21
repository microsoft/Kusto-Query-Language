---
title: prev() - Azure Data Explorer
description: This article describes prev() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# prev()

Returns the value of a specific column in a specified row.
The specified row is at a specified offset from the current row in a [serialized row set](./windowsfunctions.md#serialized-row-set).

## Syntax

There are several possibilities.

* `prev(column)`

* `prev(column, offset)`

* `prev(column, offset, default_value)`

## Arguments

* `column`: The column to get the values from.

* `offset`: The offset to go back in rows. When no offset is specified, a default offset 1 is used.

* `default_value`: The default value to be used when there are no previous rows to take the value from. When no default value is specified, null is used.

## Examples

```kusto
Table | serialize | extend prevA = prev(A,1)
| extend diff = A - prevA
| where diff > 1

Table | serialize prevA = prev(A,1,10)
| extend diff = A - prevA
| where diff <= 10
```
