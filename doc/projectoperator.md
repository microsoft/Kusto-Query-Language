---
title: Project operator - Azure Data Explorer | Microsoft Docs
description: This article describes Project operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
ms.localizationpriority: high
---
# project operator

Select the columns to include, rename or drop, and insert new computed columns. 

The order of the columns in the result is specified by the order of the arguments. Only the columns specified in the arguments are included in the result. Any other columns in the input are dropped.  (See also `extend`.)

```kusto
T | project cost=price*quantity, price
```

## Syntax

*T* `| project` *ColumnName* [`=` *Expression*] [`,` ...]
  
or
  
*T* `| project` [*ColumnName* | `(`*ColumnName*[`,`]`)` `=`] *Expression* [`,` ...]

## Arguments

* *T*: The input table.
* *ColumnName:* Optional name of a column to appear in the output. If there is no *Expression*, then *ColumnName* is mandatory and a column of that name must appear in the input. If omitted, the name will be automatically generated. If *Expression* returns more than one column, a list of column names can be specified in parentheses. In this case *Expression*'s output columns will be given the specified names, dropping all rest of the output columns, if there are any. If list of the column names is not specified, all *Expression*'s output columns with generated names will be added to the output.
* *Expression:* Optional scalar expression referencing the input columns. If *ColumnName* is not omitted then *Expression* is mandatory.

    It is legal to return a new calculated column with the same name as an existing column in the input.

## Returns

A table that has the columns named as arguments, and as many rows as the input table.

## Example

The following example shows several kinds of manipulations that can be done
using the `project` operator. The input table `T` has three columns of type `int`: `A`, `B`, and `C`. 

```kusto
T
| project
    X=C,                       // Rename column C to X
    A=2*B,                     // Calculate a new column A from the old B
    C=strcat("-",tostring(C)), // Calculate a new column C from the old C
    B=2*B                      // Calculate a new column B from the old B
```

[series_stats](series-statsfunction.md) is an example of a function that returns multiple columns.