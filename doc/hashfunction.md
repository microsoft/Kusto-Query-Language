---
title: hash() - Azure Data Explorer
description: Learn how to use the hash() function to return the hash value of the input.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/18/2022
---
# hash()

Returns a hash value for the input value.

## Syntax

`hash(`*source* [`,` *mod*]`)`

## Arguments

* *source*: The value to be hashed.
* *mod*: An optional modulo value to be applied to the hash result, so that
  the output value is between `0` and *mod* - 1

## Returns

The hash value of *source*. If *mod* is specified, the function returns the hash value modulo the value of *mod*.

> [!WARNING]
> The function uses the *xxhash64* algorithm to calculate the hash for each scalar, but this may change. We therefore only recommend using this function within a single query where all invocations of the function will use the same algorithm.
>
> If you need to persist a combined hash, we recommend using [hash_sha256()](sha256hashfunction.md), [hash_sha1()](sha1-hash-function.md), or [hash_md5()](md5hashfunction.md) and combining the hashes into a single hash with a [bitwise operator](binoperators.md). Note that these functions are more complex to calculate than `hash()`.

## Examples

```kusto
hash("World")                   // 1846988464401551951
hash("World", 100)              // 51 (1846988464401551951 % 100)
hash(datetime("2015-01-01"))    // 1380966698541616202
```

You can use the `hash()` function for sampling data if the values in one of its columns is uniformly distributed. In the following example, *StartTime* values are uniformly distributed and the function is used to run a query on 10% of the data.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
StormEvents 
| where hash(StartTime, 10) == 0
| summarize StormCount = count(), TypeOfStorms = dcount(EventType) by State 
| top 5 by StormCount desc
```
