---
title: avgif() (aggregation function) - Azure Data Explorer
description: Learn how to use the avgif() function to return the average value of an expression where the predicate evaluates to true.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/03/2022
---
# avgif() (aggregation function)

Calculates the [average](avg-aggfunction.md) of *Expr* across the group for which *Predicate* evaluates to `true`.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`avgif` `(`*Expr*`,` *Predicate*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *Expr* | string | &check; | Expression used for aggregation calculation. Records with `null` values are ignored and not included in the calculation. |
| *Predicate* | string | &check; | Predicate that if true, the *Expr* calculated value will be added to the average. |

## Returns

Returns the average value of *Expr* across the group where *Predicate* evaluates to `true`.

## Example

This example calculates the average damage by state in cases where there was any damage.

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVXAsSy1KTE9NScwFkrYl+Tn5eekaiWXpGgouYCHnovyCYk1NHai68IzUPBd0tZlpGkiKdZDYCnYGmpoKSZUKwSWJJakAP4a4kIQAAAA=)**\]**

```kusto
StormEvents
| summarize Averagedamage=tolong(avg( DamageCrops)),AverageWhenDamage=tolong(avgif(DamageCrops,DamageCrops >0)) by State
```

**Results**
The results table shown includes only the first 10 rows.

| State                | Averagedamage | Averagewhendamage |
| -------------------- | ------------- | ----------------- |
| TEXAS                | 7524          | 491291            |
| KANSAS               | 15366         | 695021            |
| IOWA                 | 4332          | 28203             |
| ILLINOIS             | 44568         | 2574757           |
| MISSOURI             | 340719        | 8806281           |
| GEORGIA              | 490702        | 57239005          |
| MINNESOTA            | 2835          | 144175            |
| WISCONSIN            | 17764         | 438188            |
| NEBRASKA             | 21366         | 187726            |
| NEW YORK             | 5             | 10000             |
| ... | ... | ... |
