---
title: countif() (aggregation function) - Azure Data Explorer
description: This article describes countif() (aggregation function) in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 07/10/2022
---
# countif() (aggregation function)

Counts the rows for which *Predicate* evaluates to `true`.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`countif` `(`*Predicate*`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| *Predicate*|  string | &check; | Expression used for aggregation calculation. *Predicate* can be any scalar expression with a return type of bool (evaluating to true/false).

## Returns

Returns a count of rows for which *Predicate* evaluates to `true`.

## Example of counting storms by state

This example shows the number of storms with damage to crops by state.

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVQjJL0nMcc4vzSuxTQaRGpo6YKHwzJIMl8TcxPRUiHhmmgaE61yUX1CsYGegqZBUqRBckliSCgAAARcgWwAAAA==)**\]**

```kusto
StormEvents
| summarize TotalCount=count(),TotalWithDamage=countif(DamageCrops >0) by State
```

**Results**

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

## Example of counting based on string length

This example shows the number of names with more than 4 letters.

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

**Results**

|countif_|
|----|
|2|

## See also

[count()](count-aggfunction.md) function, which counts rows without predicate expression.
