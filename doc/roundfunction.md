---
title: round() - Azure Data Explorer
description: Learn how to use the round() function to round the number to the specified precision.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/17/2023
---
# round()

Returns the rounded number to the specified precision.

## Syntax

`round(`*number* [`,` *precision*]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *number*| long or real | &check; | The number to calculate the round on.|
| *precision*| int | | The number of digits to round to. The default is 0.|

## Returns

The rounded number to the specified precision.

Round is different from the [`bin()`](binfunction.md) function in
that the `round()` function rounds a number to a specific number of digits while the `bin()` function rounds the value to an integer multiple of a given bin size. For example, `round(2.15, 1)` returns 2.2 while `bin(2.15, 1)` returns 2.

## Examples

```kusto
round(2.15, 1)      // 2.2
round(2.15)         // 2 // equivalent to round(2.15, 0)
round(-50.55, -2)   // -100
round(21.5, -1)     // 20
```
