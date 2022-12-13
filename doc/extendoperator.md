---
title: extend operator - Azure Data Explorer
description: This article describes extend operator in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/24/2022
---
# extend operator

Create calculated columns and append them to the result set.

## Syntax

*T* `| extend` [*ColumnName* | `(`*ColumnName*[`,` ...]`)` `=`] *Expression* [`,` ...]

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
| *T* | string | &check; | Tabular input to extend. |
| *ColumnName* | string | | Name of the column to add or update. |
| *Expression* | string | &check; | Calculation to perform over the input.|

* If *ColumnName* is omitted, the output column name of *Expression* will be automatically generated.
* If *Expression* returns more than one column, a list of column names can be specified in parentheses. Then, *Expression*'s output columns will be given the specified names. If a list of the column names is not specified, all *Expression*'s output columns with generated names will be added to the output.

## Returns

A copy of the input tabular result set, such that:

1. Column names noted by `extend` that already exist in the input are removed
   and appended as their new calculated values.
1. Column names noted by `extend` that do not exist in the input are appended
   as their new calculated values.

> [!NOTE]
> The `extend` operator adds a new column to the input result set, which does
  **not** have an index. In most cases, if the new column is set to be exactly
  the same as an existing table column that has an index, Kusto can automatically
  use the existing index. However, in some complex scenarios this propagation is
  not done. In such cases, if the goal is to rename a column, use the [`project-rename` operator](projectrenameoperator.md) instead.

## Example

[**Run the query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5uWqUUitKEnNS+HlUgACl9KixJLM/DwFWwXXvJSQzNxUBV2F4JLEohIQWweqKLU4uSizAKouGGRacGlubmJRpZ5LakliZk6xHpISAKfNOhtuAAAA)

```kusto
StormEvents
| extend
    Duration = EndTime - StartTime,
    Description = StormSummary.Details.Description
```

## See also

* Use [series_stats](series-statsfunction.md) to return multiple columns
