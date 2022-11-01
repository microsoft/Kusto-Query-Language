---
title: variancep() (aggregation function) - Azure Data Explorer
description: Learn how to use the variancep() aggregation function to calculate the variance of an expression as a population in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 09/21/2022
---
# variancep() (aggregation function)

Calculates the variance of *Expr* across the group, considering the group as a [population](https://en.wikipedia.org/wiki/Statistical_population).

The following formula is used:

:::image type="content" source="images/variancep-aggfunction/variance-population.png" alt-text="Image showing a variance sample formula." :::

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`variancep` `(`*Expr*`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
|*Expr* | string | &check; | Expression that will be used for aggregation calculation.|

## Returns

Returns the variance value of *Expr* across the group.

## Example

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAytKzEtPVahQSCvKz1UwVCjJVzBVKC5JLVAw5KpRKC7NzU0syqxKVchNzE6Nz8ksLtGo0NRRKAMKJuYlpxYAeQCFH59wQQAAAA==)**\]**

```kusto
range x from 1 to 5 step 1
| summarize make_list(x), variancep(x) 
```

**Results**

|list_x|variance_x|
|---|---|
|[ 1, 2, 3, 4, 5]|2|
