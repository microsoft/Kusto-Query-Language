---
title: stdevp() (aggregation function) - Azure Data Explorer
description: Learn how to use the stdevp() aggregation function to calculate the standard deviation in an expression in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 09/21/2022
---
# stdevp() (aggregation function)

Calculates the standard deviation of *Expr* across the group, considering the group as a [population](https://en.wikipedia.org/wiki/Statistical_population) for a large data set that is representative of the population.

For a small data set that is a [sample](https://en.wikipedia.org/wiki/Sample_%28statistics%29), use [stdev() (aggregation function)](stdev-aggfunction.md).

The following formula is used:

:::image type="content" source="images/stdevp-aggfunction/stdev-population.png" alt-text="Image showing a Stdev sample formula.":::

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`stdevp` `(`*Expr*`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
|*Expr* | string | &check; | Expression that will be used for aggregation calculation.

## Returns

Returns the standard deviation value of *Expr* across the group.

## Example

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAytKzEtPVahQSCvKz1UwVCjJVzBVKC5JLVAw5KpRKC7NzU0syqxKVchNzE6Nz8ksLtGo0NQBKkhJLSsAMgEGYndiPgAAAA==)**\]**

```kusto
range x from 1 to 5 step 1
| summarize make_list(x), stdevp(x)
```

**Results**

|list_x|stdevp_x|
|---|---|
|[ 1, 2, 3, 4, 5]|1.4142135623731|
