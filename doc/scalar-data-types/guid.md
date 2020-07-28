---
title: The guid data type - Azure Data Explorer | Microsoft Docs
description: This article describes The guid data type in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: rkarlin
ms.service: data-explorer
ms.topic: reference
ms.date: 03/15/2020
---
# The guid data type

The `guid` (`uuid`, `uniqueid`) data type represents a 128-bit globally-unique
value.

> [!WARNING]
> As of this writing, support for the `guid` type is incomplete.
> The main gap is the lack of an index on columns of this type,
> affecting the performance of queries that predicate over this type.
> We strongly recommend that teams use values of type `string` instead.

## guid literals

To represent a literal of type `guid`, use the following format:

```kusto
guid(74be27de-1e4e-49d9-b579-fe0b331d3642)
```

The special form `guid(null)` represents the [null value](null-values.md).