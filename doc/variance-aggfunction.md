---
title: variance() (aggregation function) - Azure Data Explorer
description: Learn how to use the variance() aggregation function to calculate the sample variance of the expression across the group.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/02/2023
---
# variance() (aggregation function)

Calculates the variance of *expr* across the group, considering the group as a [sample](https://en.wikipedia.org/wiki/Sample_%28statistics%29).

The following formula is used:

:::image type="content" source="images/variance-aggfunction/variance-sample.png" alt-text="Image showing a variance sample formula.":::

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`variance(`*expr*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*expr* | real | &check; | The expression used for the variance calculation.|

## Returns

Returns the variance value of *expr* across the group.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAytKzEtPVahQSCvKz1UwVCjJVzBVKC5JLVAw5KpRKC7NzU0syqxKVchNzE6Nz8ksLtGo0NRRKAMKJuYlpwI5ADQ5+T5AAAAA" target="_blank">Run the query</a>

```kusto
range x from 1 to 5 step 1
| summarize make_list(x), variance(x) 
```

**Output**

|list_x|variance_x|
|---|---|
|[ 1, 2, 3, 4, 5]|2.5|
