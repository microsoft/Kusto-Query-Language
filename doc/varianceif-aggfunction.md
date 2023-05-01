---
title: varianceif() (aggregation function) - Azure Data Explorer
description: Learn how to use the varianceif() function to calculate the variance in an expression where the predicate evaluates to true.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/15/2023
---
# varianceif() (aggregation function)

Calculates the [variance](variance-aggfunction.md) of *expr* in records for which *predicate* evaluates to `true`.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`varianceif(`*expr*`,` *predicate*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*expr* | string | &check; | The expression to use for the variance calculation.|
|*predicate*| string | &check; | If *predicate* evaluates to `true`, the *expr* calculated value will be added to the variance.|

## Returns

Returns the variance value of *expr* in records for which *predicate* evaluates to `true`.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAytKzEtPVahQSCvKz1UwVCjJVzA0MFAoLkktUDDkqlEoLs3NTSzKrEpVKANSiXnJqZlpGhU6ChWqRgq2tgoGmgA5lfgVQAAAAA==" target="_blank">Run the query</a>

```kusto
range x from 1 to 100 step 1
| summarize varianceif(x, x%2 == 0)
```

**Output**

|varianceif_x|
|---|
|850|
