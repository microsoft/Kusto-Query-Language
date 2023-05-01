---
title: sumif() (aggregation function) - Azure Data Explorer
description: Learn how to use the sumif() (aggregation function) function to calculate the sum of an expression value in records for which the predicate evaluates to true.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/05/2023
---
# sumif() (aggregation function)

Calculates the sum of *expr* in records for which *predicate* evaluates to `true`.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

You can also use the [sum()](sum-aggfunction.md) function, which sums rows without predicate expression.

## Syntax

`sumif(`*expr*`,`*predicate*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *expr* | string | &check; | The expression used for the aggregation calculation. |
| *predicate* | string | &check; | The expression used to filter rows. If the predicate evaluates to `true`, the row will be included in the result.|

## Returns

Returns the sum of *expr* for which *predicate* evaluates to `true`.

## Example showing the sum of damages based on no casualty count

This example shows the sum total damage for storms without casualties.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVXBJzE1MT/XLd04sLk3MKclMLbYFSmamaWhAZJyL8guKtSHsACA7taikUlNHwyU1sSSj2CWzKDW5RBvC8cxLAXM1bW0NNBWSKhWCSxJLUgF0hdWZeAAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize DamageNoCasualties=sumif((DamageCrops+DamageProperty),(DeathsDirect+DeathsIndirect)==0) by State
```

**Output**

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

**Output**

|sumif_day_of_birth|
|----|
|32|
