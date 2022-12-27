---
title: isutf8() - Azure Data Explorer
description: Learn how to use the isutf8() function to check if the argument is a valid utf8 string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/21/2022
---
# isutf8()

Returns `true` if the argument is a valid utf8 string.

```kusto
isutf8("some string") == true
```

## Syntax

`isutf8(`[*value*]`)`

## Returns

Indicates whether the argument is a valid utf8 string.

## Example

```kusto
T
| where isutf8(fieldName)
| count
```
