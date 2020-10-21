---
title: totimespan() - Azure Data Explorer
description: This article describes totimespan() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# totimespan()

Converts input to [timespan](./scalar-data-types/timespan.md) scalar.

```kusto
totimespan("0.00:01:00") == time(1min)
```

## Syntax

`totimespan(Expr)`

## Arguments

* *`Expr`*: Expression that will be converted to [timespan](./scalar-data-types/timespan.md).

## Returns

If conversion is successful, result will be a [timespan](./scalar-data-types/timespan.md) value.
Else, result will be null.
