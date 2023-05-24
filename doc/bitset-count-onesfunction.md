---
title:  bitset_count_ones()
description: Learn how to use the bitset_count_ones() function to return the number of set bits in the binary representation of a number.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/23/2022
---
# bitset_count_ones()

Returns the number of set bits in the binary representation of a number.

## Syntax

`bitset_count_ones(`*value*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *value* | int | &check; | The value for which to calculate the number of set bits. |

## Returns

Returns the number of set bits in the binary representation of a number.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA9PXVzAxUrBVMDbSttA2UrBSSFI3MDAEQ3UFW6C4QlJmSbFCcWoJL1dBUWZeiUJ+XmoxUANIOLUkPjm/NK8kHiSmYWKkCQAj0l10TgAAAA==" target="_blank">Run the query</a>

```kusto
// 42 = 32+8+2 : b'00101010' == 3 bits set
print ones = bitset_count_ones(42) 
```

**Output**

|ones|
|---|
|3|
