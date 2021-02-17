---
title: extend operator - Azure Data Explorer | Microsoft Docs
description: This article describes extend operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
ms.localizationpriority: high
---
# extend operator

Create calculated columns and append them to the result set.

```kusto
T | extend duration = endTime - startTime
```

## Syntax

*T* `| extend` [*ColumnName* | `(`*ColumnName*[`,` ...]`)` `=`] *Expression* [`,` ...]

## Arguments

* *T*: The input tabular result set.
* *ColumnName:* Optional. The name of the column to add or update. If omitted, the name will be generated. If *Expression* returns more than one column, a list of column names can be specified in parentheses. In this case *Expression*'s output columns will be given the specified names, dropping the rest of the output columns, if there are any. If a list of the column names is not specified, all *Expression*'s output columns with generated names will be added to the output.
* *Expression:* A calculation over the columns of the input.

## Returns

A copy of the input tabular result set, such that:
1. Column names noted by `extend` that already exist in the input are removed
   and appended as their new calculated values.
2. Column names noted by `extend` that do not exist in the input are appended
   as their new calculated values.

**Tips**

* The `extend` operator adds a new column to the input result set, which does
  **not** have an index. In most cases, if the new column is set to be exactly
  the same as an existing table column that has an index, Kusto can automatically
  use the existing index. However, in some complex scenarios this propagation is
  not done. In such cases, if the goal is to rename a column,
  use the [`project-rename` operator](projectrenameoperator.md) instead.

## Example

```kusto
Logs
| extend
    Duration = CreatedOn - CompletedOn
    , Age = now() - CreatedOn
    , IsSevere = Level == "Critical" or Level == "Error"
```

You can use the [series_stats](series-statsfunction.md) function to return multiple columns.