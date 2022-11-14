---
title: isnotempty() - Azure Data Explorer
description: This article describes isnotempty() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/13/2022
---
# isnotempty()

Returns `true` if the argument isn't an empty string, and it isn't null.

> **Deprecated aliases:** notempty()

## Syntax

`isnotempty(`[*value*]`)`

## Example

```kusto
T
| where isnotempty(fieldName)
| count
```
