---
title: stdev() (aggregation function) - Azure Data Explorer
description: Learn how to use the stdev() aggregation function to calculate a standard deviation using Bessel's correction in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 09/20/2022
---
# stdev() (aggregation function)

Calculates the standard deviation of *Expr* across the group, using [Bessel's correction](https://en.wikipedia.org/wiki/Bessel's_correction) for a small data set that is considered a [sample](https://en.wikipedia.org/wiki/Sample_%28statistics%29).

For a large data set that is representative of the population, use [stdevp() (aggregation function)](stdevp-aggfunction.md).

* Used formula:

:::image type="content" source="images/stdev-aggfunction/stdev-sample.png" alt-text="Image showing a Stdev sample formula.":::

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`stdev` `(`*Expr*`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| *Expr* | string | &check; | Expression that will be used for aggregation calculation. |

## Returns

Returns the standard deviation value of *Expr* across the group.

## Example

The following example shows the standard deviation for the group.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAytKzEtPVahQSCvKz1UwVCjJVzBVKC5JLVAw5KpRKC7NzU0syqxKVchNzE6Nz8ksLtGo0NQBKkhJLQOyAG3qbWE9AAAA" target="_blank">Run the query</a>

```kusto
range x from 1 to 5 step 1
| summarize make_list(x), stdev(x)
```

**Results**

|list_x|stdev_x|
|---|---|
|[ 1, 2, 3, 4, 5]|1.58113883008419|
