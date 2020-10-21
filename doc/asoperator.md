---
title: as operator - Azure Data Explorer | Microsoft Docs
description: This article describes as operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# as operator

Binds a name to the operator's input tabular expression, thus allowing the query
to reference the value of the tabular expression multiple times without breaking
the query and binding a name through the [let statement](letstatement.md).

## Syntax

*T* `|` `as` [`hint.materialized` `=` `true`] *Name*

## Arguments

* *T*: A tabular expression.
* *Name*: A temporary name for the tabular expression.
* `hint.materialized`: If set to `true`, the value of the tabular expression will be
  materialized as if it was wrapped by a [materialize()](./materializefunction.md) function
  call.

> [!NOTE]
> * The name given by `as` will be used in the `withsource=` column of [union](./unionoperator.md), the `source_` column of [find](./findoperator.md), and the `$table` column of [search](./searchoperator.md).
> * The tabular expression named using the operator in a [join](./joinoperator.md)'s outer tabular input (`$left`) can also be used in the join's tabular inner input (`$right`).

## Examples

```kusto
// 1. In the following 2 example the union's generated TableName column will consist of 'T1' and 'T2'
range x from 1 to 10 step 1 
| as T1 
| union withsource=TableName T2

union withsource=TableName (range x from 1 to 10 step 1 | as T1), T2

// 2. In the following example, the 'left side' of the join will be: 
//      MyLogTable filtered by type == "Event" and Name == "Start"
//    and the 'right side' of the join will be: 
//      MyLogTable filtered by type == "Event" and Name == "Stop"
MyLogTable  
| where type == "Event"
| as T
| where Name == "Start"
| join (
    T
    | where Name == "Stop"
) on ActivityId
```