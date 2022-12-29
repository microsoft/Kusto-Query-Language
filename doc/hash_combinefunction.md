---
title: hash_combine() - Azure Data Explorer
description: learn how to use the hash_combine() function to combine hash values of two or more hashes.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/18/2022
---
# hash_combine()

Combines hash values of two or more hashes.

## Syntax

`hash_combine(`*h1* `,` *h2* [`,` *h3* ...]`)`

## Arguments

* *h1*: Long value representing the first hash value.
* *h2*: Long value representing the second hash value.
* *hN*: Long value representing Nth hash value.

## Returns

The combined hash value of the given scalars.

## Examples

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print value1 = "Hello", value2 = "World"
| extend h1 = hash(value1), h2=hash(value2)
| extend combined = hash_combine(h1, h2)
```

**Output**

|value1|value2|h1|h2|combined|
|---|---|---|---|---|
|Hello|World|753694413698530628|1846988464401551951|-1440138333540407281|
