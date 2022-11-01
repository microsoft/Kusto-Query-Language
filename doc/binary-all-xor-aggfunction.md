---
title: binary_all_xor() (aggregation function) - Azure Data Explorer
description: This article describes binary_all_xor() (aggregation function) in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 08/24/2022
---
# binary_all_xor() (aggregation function)

Accumulates values using the binary `XOR` operation for each summarization group, or in total if a group is not specified.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`binary_all_xor` `(`*Expr*`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| *Expr* | long | &check; | A long number used for the binary `AND`  calculation. |

## Returns

Returns a value that is aggregated using the binary `XOR` operation over records for each summarization group, or in total if a group is not specified.

## Example

The following example produces `CAFEF00D` using binary `XOR` operations:

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0tJLAHCpJxUjbzSXKuc/Lx0Ta5oLgUFgwoTExMDENYB8wxdIRDCszRwdHJyNLCE8AwMDJwMDUwddbhiuWoUiktzcxOLMqtSFYpSi0tzShRsFUrySwsKUos0SvIzUis0kjLzEosq4xNzcuIr8otA9mpqagIAuXol8IgAAAA=)**\]**

```kusto
datatable(num:long)
[
  0x44404440,
  0x1E1E1E1E,
  0x90ABBA09,
  0x000B105A,
]
| summarize result = toupper(tohex(binary_all_xor(num)))
```

**Results**

|results|
|--|
|CAFEF00D|
