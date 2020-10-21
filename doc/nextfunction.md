---
title: next() - Azure Data Explorer | Microsoft Docs
description: This article describes next() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# next()

Returns the value of a column in a row that it at some offset following the
current row in a [serialized row set](./windowsfunctions.md#serialized-row-set).

## Syntax

`next(column)`

`next(column, offset)`

`next(column, offset, default_value)`

## Arguments

* `column`: the column to get the values from.

* `offset`: the offset to go ahead in rows. When no offset is specified a default offset 1 is used.

* `default_value`: the default value to be used when there is no next rows to take the value from. When no default value is specified, null is used.


## Examples
```kusto
Table | serialize | extend nextA = next(A,1)
| extend diff = A - nextA
| where diff > 1

Table | serialize nextA = next(A,1,10)
| extend diff = A - nextA
| where diff <= 10
```