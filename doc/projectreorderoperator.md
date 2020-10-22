---
title: project-reorder operator - Azure Data Explorer
description: This article describes project-reorder operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# project-reorder operator

Reorders columns in the result output.

```kusto
T | project-reorder Col2, Col1, Col* asc
```

## Syntax

*T* `| project-reorder` *ColumnNameOrPattern* [`asc`|`desc`] [`,` ...]

## Arguments

* *T*: The input table.
* *ColumnNameOrPattern:* The name of the column or column wildcard pattern added to the output.
* For wildcard patterns: specifying `asc` or `desc` orders columns using their names in ascending or descending order. If `asc` or `desc` aren't specified, the order is determined by the matching columns as they appear in the source table.

> [!NOTE]
> * In ambiguous *ColumnNameOrPattern* matching, the column appears in the first position matching the pattern.
> * Specifying columns for the `project-reorder` is optional. Columns that aren't specified explicitly appear as the last columns of the output table.
> * To remove columns, use [`project-away`](projectawayoperator.md).
> * To choose which columns to keep, use [`project-keep`](project-keep-operator.md).
> * To rename columns, use [`project-rename`](projectrenameoperator.md).

## Returns

A table that contains columns in the order specified by the operator arguments. `project-reorder` doesn't rename or remove columns from the table, therefore, all columns that existed in the source table, appear in the result table.

## Examples

Reorder a table with three columns (a, b, c) so the second column (b) will appear first.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print a='a', b='b', c='c'
|  project-reorder b
```

|b|a|c|
|---|---|---|
|b|a|c|

Reorder columns of a table so that columns starting with `a` will appear before other columns.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print b = 'b', a2='a2', a3='a3', a1='a1'
|  project-reorder a* asc
```

|a1|a2|a3|b|
|---|---|---|---|
|a1|a2|a3|b|
