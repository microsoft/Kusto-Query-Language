---
title: isnull() - Azure Data Explorer
description: Learn how to use the isnull() function to check if the argument value is null.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/21/2022
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

* `string` values can't be null. Use [isempty](./isemptyfunction.md)
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