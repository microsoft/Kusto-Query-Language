---
title: totimespan() - Azure Data Explorer
description: This article describes totimespan() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/03/2022
---
# totimespan()

Converts input to [timespan](./scalar-data-types/timespan.md) scalar.

```kusto
totimespan("0.00:01:00") == time(1min)
```

> **Deprecated aliases:** totime()

## Syntax

`totimespan(Expr)`

## Arguments

* *`Expr`*: Expression that will be converted to [timespan](./scalar-data-types/timespan.md).

## Returns

If conversion is successful, result will be a [timespan](./scalar-data-types/timespan.md) value.
Else, result will be null.
