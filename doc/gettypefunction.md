---
title: gettype() - Azure Data Explorer
description: Learn how to use the gettype() function to return a string representing the runtime type of its single argument.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/18/2022
---
# gettype()

Returns the runtime type of its single argument.

The runtime type may be different than the nominal (static) type for expressions whose nominal type is `dynamic`; in such cases `gettype()` can be useful to reveal the type of the actual value (how the value is encoded in memory).

## Syntax

`gettype(`*value*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *value* | scalar | &check; | The value for which to find the type. |

## Returns

A string representing the runtime type of *value*.

## Examples

|Expression                          |Returns      |
|------------------------------------|-------------|
|`gettype("a")`                      |`string`     |
|`gettype(111)`                      |`long`       |
|`gettype(1==1)`                     |`bool`       |
|`gettype(now())`                    |`datetime`   |
|`gettype(1s)`                       |`timespan`   |
|`gettype(parse_json('1'))`           |`int`        |
|`gettype(parse_json(' "abc" '))`     |`string`     |
|`gettype(parse_json(' {"abc":1} '))` |`dictionary` |
|`gettype(parse_json(' [1, 2, 3] '))` |`array`      |
|`gettype(123.45)`                   |`real`       |
|`gettype(guid(12e8b78d-55b4-46ae-b068-26d7a0080254))`|`guid`|
|`gettype(parse_json(''))`            |`null`|
