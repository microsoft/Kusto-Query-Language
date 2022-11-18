---
title: bitset_count_ones() - Azure Data Explorer
description: This article describes bitset_count_ones() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/22/2020
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

[**Run the Query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA9PXVzAxUrBVMDbSttA2UrBSSFI3MDAEQ3UFW6C4QlJmSbFCcWoJL1dBUWZeiUJ+XmoxUANIOLUkPjm/NK8kHiSmYWKkCQAj0l10TgAAAA==)

```kusto
// 42 = 32+8+2 : b'00101010' == 3 bits set
print ones = bitset_count_ones(42) 
```

|ones|
|---|
|3|
