---
title: maxif() (aggregation function) - Azure Data Explorer
description: Learn how to use the maxif() function to calculate the maximum value of an expression where the predicate evaluates to true.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/16/2022
---
# maxif() (aggregation function)

Calculates the maximum value of *Expr* in records for which *Predicate* evaluates to `true`.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

See also - [max()](max-aggfunction.md) function, which returns the maximum value across the group without predicate expression.

## Syntax

`maxif` `(`*Expr*`,`*Predicate*`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| *Expr* | string | &check; | Expression that will be used for aggregation calculation. |
| *Predicate* | string | &check; | Expression that will be used to filter rows. |

## Returns

Returns the maximum value of *Expr* in records for which *Predicate* evaluates to `true`.

## Example

This example shows the maximum damage for events with no casualties.

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAyWMMQ7CQAwEe17hkigp+ICrHAUFCCkvMImBk7g7ZDvRBfF4IlztjFa7gxVJx4Wz6e4LXI3zBIESPRg9eilvbZ2vG7PY2kFgsqeiR4jCo7Uupzz9dbvTOSWS+GE4U/WHS+lJZ3pZZMVENd73XnS+BsRDA7cVBiPjH/V0dHmeAAAA)**\]**

```kusto
StormEvents
| extend Damage=DamageCrops+DamageProperty, Deaths=DeathsDirect+DeathsIndirect
| summarize MaxDamageNoCasualties=maxif(Damage,Deaths ==0) by State
```

**Results**

The results table shown includes only the first 10 rows.

| State                | MaxDamageNoCasualties |
| -------------------- | --------------------- |
| TEXAS                | 25000000              |
| KANSAS               | 37500000              |
| IOWA                 | 15000000              |
| ILLINOIS             | 5000000               |
| MISSOURI             | 500005000             |
| GEORGIA              | 344000000             |
| MINNESOTA            | 38390000              |
| WISCONSIN            | 45000000              |
| NEBRASKA             | 4000000               |
| NEW YORK             | 26000000              |
| ... | ... |
