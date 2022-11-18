---
title: binary_shift_right() - Azure Data Explorer
description: This article describes binary_shift_right() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/10/2022
---
# binary_shift_right()

Returns binary shift right operation on a pair of numbers.

## Syntax

`binary_shift_right(`*value*`,`*shift*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *value* | int | &check; | The value to shift right. |
| *shift* | int | &check; | The number of bits to shift right. |

## Returns

Returns binary shift right operation on a pair of numbers: value >> (shift%64).
If n is negative a NULL value is returned.

## Examples

[**Run the Query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswr0UjKzEssqowvzshMK4kvykzPKNEw1DHS1AQAd48PPR4AAAA=)

```kusto
binary_shift_right(1,2)
```

|Result|
|------|
|0 |
