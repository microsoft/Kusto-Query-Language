---
title:  countif() (aggregation function)
description: Learn how to use the countif() function to count the rows where the predicate evaluates to true.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/27/2022
---
# countif() (aggregation function)

Counts the rows in which *predicate* evaluates to `true`.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`countif` `(`*predicate*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *predicate*|  string | &check; | The expression used for aggregation calculation. The value can be any scalar expression with a return type of bool.

## Returns

Returns a count of rows in which *predicate* evaluates to `true`.

## Examples

### Count storms by state

This example shows the number of storms with damage to crops by state.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVQjJL0nMcc4vzSuxTQaRGpo6YKHwzJIMl8TcxPRUiHhmmgaE61yUX1CsYGegqZBUqRBckliSCgAAARcgWwAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize TotalCount=count(),TotalWithDamage=countif(DamageCrops >0) by State
```

The results table shown includes only the first 10 rows.

| State                | TotalCount | TotalWithDamage |
| -------------------- | ---------- | --------------- |
| TEXAS                | 4701       | 72              |
| KANSAS               | 3166       | 70              |
| IOWA                 | 2337       | 359             |
| ILLINOIS             | 2022       | 35              |
| MISSOURI             | 2016       | 78              |
| GEORGIA              | 1983       | 17              |
| MINNESOTA            | 1881       | 37              |
| WISCONSIN            | 1850       | 75              |
| NEBRASKA             | 1766       | 201             |
| NEW YORK             | 1750       | 1               |
| ... | ... | ... |

### Count based on string length

This example shows the number of names with more than 4 letters.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAyWNMQvCMBBG9/yKo1MDWRRFregqOIl0EympXtNAcgdpMij+eI+Wb3pv+F7ADC2c4G2zrA9Yk43YTDl5ckb0p+Oh633KYxOYnFYPBQDVlUeqDBzMTDdbgtBqv+AFOTkUsd4u4i5nLLxTz6Nq1Q+mEqNN/ovw4kLZD7UUA9Jc13CGjf4DYumr9poAAAA=" target="_blank">Run the query</a>

```kusto
let T = datatable(name:string, day_of_birth:long)
[
   "John", 9,
   "Paul", 18,
   "George", 25,
   "Ringo", 7
];
T
| summarize countif(strlen(name) > 4)
```

**Output**

|countif_|
|----|
|2|

## See also

[count()](count-aggfunction.md) function, which counts rows without predicate expression.
