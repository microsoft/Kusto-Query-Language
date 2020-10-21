---
title: isnull() - Azure Data Explorer
description: This article describes isnull() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# isnull()

Evaluates its sole argument and returns a `bool` value indicating if the argument evaluates to a null value.

```kusto
isnull(parse_json("")) == true
```

## Syntax

`isnull(`*Expr*`)`

## Returns

True or false, depending on whether or not the value is null.

**Notes**

* `string` values cannot be null. Use [isempty](./isemptyfunction.md)
  to determine if a value of type `string` is empty or not.

|x                |`isnull(x)`|
|-----------------|-----------|
|`""`             |`false`    |
|`"x"`            |`false`    |
|`parse_json("")`  |`true`     |
|`parse_json("[]")`|`false`    |
|`parse_json("{}")`|`false`    |

## Example

```kusto
T | where isnull(PossiblyNull) | count
```