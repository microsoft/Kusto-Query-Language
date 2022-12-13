---
title: top operator - Azure Data Explorer
description: This article describes top operator in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/24/2022
---
# top operator

Returns the first *N* records sorted by the specified columns.

## Syntax

*T* `| top` *NumberOfRows* `by` *Expression* [`asc` | `desc`] [`nulls first` | `nulls last`]

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *T* | tabular | &check; | Input to sort. |
| *NumberOfRows* | int | &check; | The number of rows of *T* to return.|
| *Expression* | string | &check; | Scalar expression by which to sort. The type of the values must be numeric, date, time or string.
| `asc` or `desc` | string | | Controls whether the selection is from the "bottom" or "top" of the range. Default `desc`.
| `nulls first` or `nulls last`  | string | | Controls whether null values appear at the "bottom" or "top" of the range. Default for `asc` is `nulls first`. Default for `desc` is `nulls last`.|

> [!TIP]
> `top 5 by name` is equivalent to the expression `sort by name | take 5` both from semantic and performance perspectives.

## Example

Show top 3 storms with most direct injuries.

[**Run the query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5uWqUSjJL1AwVkiqVPDMyyotykwtdsksSk0uAQCehD//JgAAAA==)

```kusto
StormEvents
| top 3 by InjuriesDirect
```

The below table shows only the relevant column. Run the query above to see more storm details for these events.

|InjuriesDirect|...|
|--|--|
|519|...|
|422|...|
|200|...|

## See also

* Use [top-nested](topnestedoperator.md) operator to produce hierarchical (nested) top results.
