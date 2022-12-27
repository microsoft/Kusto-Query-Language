---
title: isnotnull() - Azure Data Explorer
description: Learn how to use the isnotnull() function to check if the argument isn't null.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/21/2022
---
# isnotnull()

Returns `true` if the argument isn't null.

> **Deprecated aliases:** notnull()

## Syntax

`isnotnull(`[*value*]`)`

## Example

```kusto
T | where isnotnull(PossiblyNull) | count
```
