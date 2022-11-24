---
title: binary_all_or() (aggregation function) - Azure Data Explorer
description: Learn how to use the binary_all_or() function to aggregate values using the binary OR operation.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/20/2022
---
# binary_all_or() (aggregation function)

Accumulates values using the binary `OR` operation for each summarization group, or in total if a group is not specified.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`binary_all_or` `(`*Expr*`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| *Expr* | long | &check; | A long number used for the binary `AND`  calculation. |

## Returns

Returns an aggregated value using the binary `OR` operation over records for each summarization group, or in total if a group is not specified.

## Example

The following example produces `CAFEF00D` using binary `OR` operations:

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0tJLAHCpJxUjbzSXKuc/Lx0Ta5oLgUFgwoLEDAwsNAB80yMDMAAwjMwMDczR+KBgamOAlcsV41CcWlubmJRZlWqQlFqcWlOiYKtQkl+aUFBapFGSX5GaoVGUmZeYlFlfGJOTnx+EcheTU1NAEGLHNSIAAAA)**\]**

```kusto
datatable(num:long)
[
  0x88888008,
  0x42000000,
  0x00767000,
  0x00000005, 
]
| summarize result = toupper(tohex(binary_all_or(num)))
```

**Results**

|result|
|---|
|CAFEF00D|
