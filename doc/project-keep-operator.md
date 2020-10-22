---
title: project-keep operator - Azure Data Explorer
description: This article describes project-keep operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 10/21/2020
---
# project-keep operator

Select what columns from the input to keep in the output.

```kusto
T | project-keep price, quantity, zz*
```

The order of the columns in the result is determined by their original order in the table. Only the columns that were specified as arguments are kept. The other columns are excluded from the result. See also [`project`](projectoperator.md).

## Syntax

*T* `| project-keep` *ColumnNameOrPattern* [`,` ...]

## Arguments

* *T*: The input table
* *ColumnNameOrPattern:* The name of the column or column wildcard-pattern to be kept in the output.

## Returns

A table with columns that were named as arguments. Contains same number of rows as the input table.

> [!TIP]
>* To rename columns, use [`project-rename`](projectrenameoperator.md).
>* To reorder columns, use [`project-reorder`](projectreorderoperator.md).
>* You can `project-keep` any columns that are present in the original table or that were computed as part of the query.

## Example

The input table `T` has three columns of type `long`: `A`, `B`, and `C`.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(A1:long, A2:long, B:long) [1, 2, 3]
| project-keep A*    // Keeps only columns A1 and A2 in the output
```

|A1|A2|
|---|---|
|1|2|

## See also

To choose what columns from the input to exclude from the output, use [project-away](projectawayoperator.md).
