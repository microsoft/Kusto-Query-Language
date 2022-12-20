---
title: hash_xxhash64() - Azure Data Explorer
description: Learn how to use the hash_xxhash64() function to return the xxhash64 value of the input.
ms.reviewer: slneimer
ms.topic: reference
ms.date: 12/18/2022
---
# hash_xxhash64()

Returns an xxhash64 value for the input value.

## Syntax

`hash_xxhash64(`*source* [`,` *mod*]`)`

## Arguments

* *source*: The value to be hashed.
* *mod*: An optional modulo value to be applied to the hash result, so that
  the output value is between `0` and *mod* - 1

## Returns

The hash value of *source*. If *mod* is specified, the function returns the hash value modulo the value of *mod*.

## Examples

```kusto
xxhash64("World")                   // 1846988464401551951
xxhash64("World", 100)              // 51 (1846988464401551951 % 100)
xxhash64(datetime("2015-01-01"))    // 1380966698541616202
```
