---
title: isnotempty() - Azure Data Explorer
description: Learn how to use the isnotempty() function to check if the argument isn't an empty string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/21/2022
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
