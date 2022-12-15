---
title: extend operator - Azure Data Explorer
description: Learn how to use the extend operator to create calculated columns and append them to the result set.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/11/2022
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

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKCjKz0pNLlFwzUsJycxN1VEILkksKgExgZKpFSWpeSkKLqVFiSWZ+XkKtjBlCroIdQCqSrMYUAAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| project EndTime, StartTime
| extend Duration = EndTime - StartTime
```

The following table shows only the first 10 results. To see the full output, run the query.

|EndTime|StartTime|Duration|
|--|--|--|
|2007-01-01T00:00:00Z| 2007-01-01T00:00:00Z |00:00:00|
|2007-01-01T00:25:00Z| 2007-01-01T00:25:00Z |00:00:00|
|2007-01-01T02:24:00Z| 2007-01-01T02:24:00Z |00:00:00|
|2007-01-01T03:45:00Z| 2007-01-01T03:45:00Z |00:00:00|
|2007-01-01T04:35:00Z| 2007-01-01T04:35:00Z |00:00:00|
|2007-01-01T04:37:00Z| 2007-01-01T03:37:00Z |01:00:00|
|2007-01-01T05:00:00Z| 2007-01-01T00:00:00Z |05:00:00|
|2007-01-01T05:00:00Z| 2007-01-01T00:00:00Z |05:00:00|
|2007-01-01T06:00:00Z| 2007-01-01T00:00:00Z |06:00:00|
|2007-01-01T06:00:00Z| 2007-01-01T00:00:00Z |06:00:00|

## See also

* Use [series_stats](series-statsfunction.md) to return multiple columns
