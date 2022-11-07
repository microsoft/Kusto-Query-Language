---
title: isnotnull() - Azure Data Explorer
description: This article describes isnotnull() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/03/2022
---
# isnotnull()

Returns `true` if the argument is not null.

> **Deprecated aliases:** notnull()

## Syntax

`isnotnull(`[*value*]`)`

## Example

```kusto
T | where isnotnull(PossiblyNull) | count
```
