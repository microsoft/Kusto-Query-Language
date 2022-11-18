---
title: binary_shift_left() - Azure Data Explorer
description: This article describes binary_shift_left() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/10/2022
---
# binary_shift_left()

Returns binary shift left operation on a pair of numbers.

## Syntax

`binary_shift_left(`*value*`,`*shift*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *value* | int | &check; | The value to shift left. |
| *shift* | int | &check; | The number of bits to shift left. |

## Returns

Returns binary shift left operation on a pair of numbers: value << (shift%64).
If n is negative a NULL value is returned.

## Example

[**Run the Query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswr0UjKzEssqowvzshMK4nPSU0r0TDUMdLUBADck7ZgHQAAAA==)

```kusto
binary_shift_left(1,2)
```

|Result|
|------|
|4 |
