---
title: current_principal() - Azure Data Explorer | Microsoft Docs
description: This article describes current_principal() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# current_principal()

Returns the current principal running this query.

**Syntax**

`current_principal()`

**Returns**

The current principal FQN as a `string`.

**Example**

```kusto
.show queries | where Principal == current_principal()
```