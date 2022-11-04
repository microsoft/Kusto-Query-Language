---
title: count_distinctif() (aggregation function) - Azure Data Explorer - (preview)
description: This article describes count_distinctif() (aggregation function) in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/03/2022
---
# count_distinctif() (aggregation function) - (preview)

Conditionally counts unique values specified by the scalar expression per summary group, or the total number of unique values if the summary group is omitted. Only records for which *Predicate* evaluates to `true` are counted.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

If you only need an estimation of unique values count, we recommend using the less resource-consuming [dcountif](dcountif-aggfunction.md) aggregation function.

> [!NOTE]
> This function is limited to 100M unique values. An attempt to apply the function on an expression returning too many values will produce a runtime error (HRESULT: 0x80DA0012).

## Syntax

`count_distinctif` `(`*Expr*`,` *Predicate*`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| *Expr*| scalar | &check; | A scalar expression whose unique values are to be counted. |
| *Predicate* | string | &check; | Expression that is used to filter records to be aggregated. |

## Returns

Long integer value indicating the number of unique values of *`Expr`* per summary group, for all records for which the *Predicate* evaluates to `true`.

## Example

This example shows how many types of death-causing storm events happened in each state. Only storm events with a nonzero count of deaths will be counted.

```kusto
StormEvents
| summarize UniqueFatalEvents=count_distinctif(EventType,(DeathsDirect + DeathsIndirect)>0) by State
| where UniqueFatalEvents > 0
| top 5 by UniqueFatalEvents
```

**Results**

| State           | UniqueFatalEvents |
| --------------- | ----------------- |
| TEXAS           | 12                |
| CALIFORNIA      | 12                |
| OKLAHOMA        | 10                |
| NEW YORK        | 9                 |
| KANSAS          | 9                 |
