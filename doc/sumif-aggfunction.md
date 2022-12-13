---
title: sumif() (aggregation function) - Azure Data Explorer
description: This article describes sumif() (aggregation function) in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 07/12/2022
---
# sumif() (aggregation function)

Calculates the sum of *Expr* in records for which *Predicate* evaluates to `true`.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

You can also use the [sum()](sum-aggfunction.md) function, which sums rows without predicate expression.

## Syntax

`sumif` `(`*Expr*`,`*Predicate*`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| *Expr* | string | &check; | Expression that will be used for aggregation calculation. |
| *Predicate* | string | &check; | Expression that will be used to filter rows. |

## Returns

Returns the sum of *Expr* for which *Predicate* evaluates to `true`.

## Example showing the sum of damages based on no casualty count

This example shows the sum total damage for storms without casualties.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVXBJzE1MT/XLd04sLk3MKclMLbYFSmamaWhAZJyL8guKtSHsACA7taikUlNHwyU1sSSj2CWzKDW5RBvC8cxLAXM1bW0NNBWSKhWCSxJLUgF0hdWZeAAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize DamageNoCasualties=sumif((DamageCrops+DamageProperty),(DeathsDirect+DeathsIndirect)==0) by State
```

**Results**

The results table shown includes only the first 10 rows.

| State                | DamageNoCasualties |
| -------------------- | ------------------ |
| TEXAS                | 242638700          |
| KANSAS               | 407360000          |
| IOWA                 | 135353700          |
| ILLINOIS             | 120394500          |
| MISSOURI             | 1096077450         |
| GEORGIA              | 1077448750         |
| MINNESOTA            | 230407300          |
| WISCONSIN            | 241550000          |
| NEBRASKA             | 70356050           |
| NEW YORK             | 58054000           |
| ... | ... |

## Example showing the sum of birth dates

This example shows the sum of the birth dates for all names that have more than 4 letters.

```kusto
let T = datatable(name:string, day_of_birth:long)
[
   "John", 9,
   "Paul", 18,
   "George", 25,
   "Ringo", 7
];
T
| summarize sumif(day_of_birth, strlen(name) > 4)
```

**Results**

|sumif_day_of_birth|
|----|
|32|
