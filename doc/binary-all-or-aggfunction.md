---
title: binary_all_or() (aggregation function) - Azure Data Explorer
description: This article describes binary_all_or() (aggregation function) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/24/2020
---
# binary_all_or() (aggregation function)

Accumulates values using the binary `OR` operation per summarization group (or in total, if summarization is done without grouping).

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

## Syntax

summarize `binary_all_or(`*Expr*`)`

## Arguments

* *Expr*: long number.

## Returns

Returns a value that is aggregated using the binary `OR` operation over records per summarization group (or in total, if summarization is done without grouping).

## Example

Producing 'cafe-food' using binary `OR` operations:

<!-- csl: https://help.kusto.windows.net/Samples -->
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

|result|
|---|
|CAFEF00D|
