---
title: facet operator - Azure Data Explorer | Microsoft Docs
description: This article describes facet operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# facet operator

Returns a set of tables, one for each specified column.
Each table specifies the list of values taken by its column.
An additional table can be created by using the `with` clause.

## Syntax

*T* `| facet by` *ColumnName* [`, ` ...] [`with (` *filterPipe* `)`

## Arguments

* *ColumnName:* The name of column in the input, to be summarized as an output table.
* *filterPipe:* A query expression applied to the input table to produce one of the outputs.

## Returns

Multiple tables: one for the `with` clause, and one for each column.

## Example

```kusto
MyTable 
| facet by city, eventType 
    with (where timestamp > ago(7d) | take 1000)
```