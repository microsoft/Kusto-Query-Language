---
title: maxif() (aggregation function) - Azure Data Explorer
description: Learn how to use the maxif() function to calculate the maximum value of an expression where the predicate evaluates to true.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/12/2023
---
# maxif() (aggregation function)

Calculates the maximum value of *expr* in records for which *predicate* evaluates to `true`.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

See also - [max()](max-aggfunction.md) function, which returns the maximum value across the group without predicate expression.

## Syntax

`maxif(`*expr*`,`*predicate*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *expr* | string | &check; | The expression used for the aggregation calculation. |
| *predicate* | string | &check; | The expression used to filter rows. |

## Returns

Returns the maximum value of *expr* in records for which *predicate* evaluates to `true`.

## Example

This example shows the maximum damage for events with no casualties.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAzWMPQ7CMAxGdyTu4BEEAxfw1DAwgJB6AkMNRMIJsl2UIg4PbWB6+n70Ws8q2ycnt/nsDVycUweBhK6MFY3mh8HqVx6/idWHNQQmvxlWhKh89vE1xV3qpmJ0Wi9CGl8MeypVcsgNWU93j2woVOJlUYe/FRBhs4TTAK2T8wcBsgcBpgAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| extend Damage=DamageCrops + DamageProperty, Deaths=DeathsDirect + DeathsIndirect
| summarize MaxDamageNoCasualties=maxif(Damage, Deaths == 0) by State
```

**Output**

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
