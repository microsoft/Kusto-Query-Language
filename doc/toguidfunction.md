---
title: toguid() - Azure Data Explorer | Microsoft Docs
description: This article describes toguid() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# toguid()

Converts input to [`guid`](./scalar-data-types/guid.md) representation.

```kusto
toguid("70fc66f7-8279-44fc-9092-d364d70fce44") == guid("70fc66f7-8279-44fc-9092-d364d70fce44")
```

> [!NOTE]
> Prefer using [guid()](./scalar-data-types/guid.md) when possible.

## Syntax

`toguid(`*Expr*`)`

## Arguments

* *Expr*: Expression that will be converted to [`guid`](./scalar-data-types/guid.md) scalar. 

## Returns

If conversion is successful, result will be a [`guid`](./scalar-data-types/guid.md) scalar.
If conversion is not successful, result will be `null`.
