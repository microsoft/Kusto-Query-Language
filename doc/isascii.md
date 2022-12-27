---
title: isascii() - Azure Data Explorer
description: Learn how to use the isascii() to check if the argument is a valid ascii string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/21/2022
---
# isascii()

Returns `true` if the argument is a valid ascii string.

```kusto
isascii("some string") == true
```

## Syntax

`isascii(`[*value*]`)`

## Returns

Indicates whether the argument is a valid ascii string.

## Example

```kusto
T
| where isascii(fieldName)
| count
```
