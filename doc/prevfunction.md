---
title: prev() - Azure Data Explorer | Microsoft Docs
description: This article describes prev() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# prev()

Returns the value of a column in a row that it at some offset prior to the
current row in a [serialized row set](./windowsfunctions.md#serialized-row-set).

**Syntax**

`prev(column)`

`prev(column, offset)`

`prev(column, offset, default_value)`

**Arguments**

* `column`: the column to get the values from.

* `offset`: the offset to go back in rows. When no offset is specified a default offset 1 is used.

* `default_value`: the default value to be used when there is no previous rows to take the value from. When no default value is specified, null is used.


**Examples**
```kusto
Table | serialize | extend prevA = prev(A,1)
| extend diff = A - prevA
| where diff > 1

Table | serialize prevA = prev(A,1,10)
| extend diff = A - prevA
| where diff <= 10
```