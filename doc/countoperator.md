---
title: count operator - Azure Data Explorer | Microsoft Docs
description: This article describes count operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# count operator

Returns the number of records in the input record set.

**Syntax**

`T | count`

**Arguments**

* *T*: The tabular data whose records are to be counted.

**Returns**

This function returns a table with a single record and column of type
`long`. The value of the only cell is the number of records in *T*. 

**Example**

```kusto
StormEvents | count
```