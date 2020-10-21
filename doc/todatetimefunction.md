---
title: todatetime() - Azure Data Explorer
description: This article describes todatetime() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# todatetime()

Converts input to [datetime](./scalar-data-types/datetime.md) scalar.

```kusto
todatetime("2015-12-24") == datetime(2015-12-24)
```

## Syntax

`todatetime(`*Expr*`)`

## Arguments

* *Expr*: Expression that will be converted to [datetime](./scalar-data-types/datetime.md).

## Returns

If the conversion is successful, the result will be a [datetime](./scalar-data-types/datetime.md) value.
Else, the result will be null.
 
> [!NOTE]
> Prefer using [datetime()](./scalar-data-types/datetime.md) when possible.
