---
title: minif() (aggregation function) - Azure Data Explorer
description: Learn how to use the minif() function to return the minimum value of an expression where the predicate evaluates to true.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/12/2023
---
# minif() (aggregation function)

Returns the minimum of *Expr* in records for which *Predicate* evaluates to `true`.

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

See also - [min()](min-aggfunction.md) function, which returns the minimum value across the group without predicate expression.

## Syntax

 `minif` `(`*Expr*`,`*Predicate*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *Expr* | string | &check; | Expression that will be used for aggregation calculation. |
| *Predicate* | string | &check; | Expression that will be used to filter rows. |

## Returns

The minimum value of *Expr* in records for which *Predicate* evaluates to `true`.

## Example

This example shows the minimum damage for events with casualties (Except 0)

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA3WOsQ6CUAxFd7+iIwQGfwAXcHAwMWFwrlKlCa+Y16Ji/Hif1NXp3tvenrS1MYbtncR09QZ6GkkHDQa8UuVSx/GmhftD8hRtLqEhtF4rl4Yjna3wsJNuiQmnUwgY+UWwZ3HCka2vUSccjEmrwMKXzFdl5gDYrHPA9MZv/s05nGZoDY0gcR89xb/MVF+uWWU0mYYh+1PMPyFcEcH8AAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| extend Damage=DamageCrops+DamageProperty, Deaths=DeathsDirect+DeathsIndirect
| summarize MinDamageWithCasualties=minif(Damage,(Deaths >0) and (Damage >0)) by State 
| where MinDamageWithCasualties >0 and isnotnull(MinDamageWithCasualties)
```

**Output**

The results table shown includes only the first 10 rows.

| State          | MinDamageWithCasualties |
| -------------- | ----------------------- |
| TEXAS          | 8000                    |
| KANSAS         | 5000                    |
| IOWA           | 45000                   |
| ILLINOIS       | 100000                  |
| MISSOURI       | 10000                   |
| GEORGIA        | 500000                  |
| MINNESOTA      | 200000                  |
| WISCONSIN      | 10000                   |
| NEW YORK       | 25000                   |
| NORTH CAROLINA | 15000                   |
| ... | ... |
