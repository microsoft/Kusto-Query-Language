---
title: The guid data type - Azure Data Explorer | Microsoft Docs
description: This article describes The guid data type in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# The guid data type

The `guid` (`uuid`, `uniqueid`) data type represents a 128-bit globally-unique
value.

> [!WARNING]
> As of this writing, support for the `guid` type is incomplete.
> We strongly recommend that teams use values of type `string` instead.

## guid literals

To represent a literal of type `guid`, use the following format:

```kusto
guid(74be27de-1e4e-49d9-b579-fe0b331d3642)
```

The special form `guid(null)` represents the [null value](null-values.md).