---
title: binary_all_and() (aggregation function) - Azure Data Explorer
description: This article describes binary_all_and() (aggregation function) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/24/2020
---
# binary_all_and() (aggregation function)

Accumulates values using the binary `AND` operation per summarization group (or in total, if summarization is done without grouping).

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

## Syntax

summarize `binary_all_and(`*Expr*`)`

## Arguments

* *Expr*: long number.

## Returns

Returns a value that is aggregated using the binary `AND` operation over records per summarization group (or in total, if summarization is done without grouping).

## Example

Producing 'cafe-food' using binary `AND` operations:

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(num:long)
[
  0xFFFFFFFF, 
  0xFFFFF00F,
  0xCFFFFFFD,
  0xFAFEFFFF,
]
| summarize result = toupper(tohex(binary_all_and(num)))
```

|result|
|---|
|CAFEF00D|
