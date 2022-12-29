---
title: binary_shift_left() - Azure Data Explorer
description: Learn how to use the binary_shift_left() function to perform a binary shift left operation on a pair of numbers. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/21/2022
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
If n is negative, a NULL value is returned.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswr0UjKzEssqowvzshMK4nPSU0r0TDUMdLUBADck7ZgHQAAAA==" target="_blank">Run the query</a>

```kusto
binary_shift_left(1,2)
```

**Output**

|Result|
|------|
|4 |
