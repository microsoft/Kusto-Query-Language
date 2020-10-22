---
title: project-away operator - Azure Data Explorer
description: This article describes project-away operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# project-away operator

Select what columns from the input to exclude from the output.

```kusto
T | project-away price, quantity, zz*
```

The order of the columns in the result is determined by their original order in the table. Only the columns that were specified as arguments are dropped. The other columns are included in the result. (See also `project`.)

## Syntax

*T* `| project-away` *ColumnNameOrPattern* [`,` ...]

## Arguments

* *T*: The input table
* *ColumnNameOrPattern:* The name of the column or column wildcard-pattern to be removed from the output.

## Returns

A table with columns that were not named as arguments. Contains same number of rows as the input table.

> [!TIP]
>
> * To rename columns, use [`project-rename`](projectrenameoperator.md).
> * To reorder columns, use [`project-reorder`](projectreorderoperator.md).
> * You can `project-away` any columns that are present in the original table or that were computed as part of the query.

## Examples

The input table `T` has three columns of type `long`: `A`, `B`, and `C`.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(A:long, B:long, C:long) [1, 2, 3]
| project-away C    // Removes column C from the output
```

|A|B|
|---|---|
|1|2|

Removing columns starting with 'a'.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print a2='a2', b = 'b', a3='a3', a1='a1'
| project-away a*
```

|b|
|---|
|b|

## See also

To choose what columns from the input to keep in the output, use [project-keep](project-keep-operator.md).
