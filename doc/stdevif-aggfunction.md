---
title: stdevif() (aggregation function) - Azure Data Explorer
description: Learn how to use the stdevif() function to calculate the standard deviation of an expression where the predicate evaluates to true.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/16/2022
---
# stdevif() (aggregation function)

Calculates the [standard deviation](stdev-aggfunction.md) of *Expr* in records for which *Predicate* evaluates to `true`.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

 `stdevif` `(`*Expr*`,`*Predicate*`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| *Expr* | string | &check; | Expression that will be used for aggregation calculation. |
| *Predicate* | string | &check; | Predicate that has to evaluate to `true`, in order for *Expr* to be added to the result. |

## Returns

Returns the standard deviation value of *Expr* in records for which *Predicate* evaluates to `true`.

## Example

The following example shows the standard deviation in a range of 1 to 100.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAytKzEtPVahQSCvKz1UwVCjJVzA0MFAoLkktUDDkqlEoLs3NTSzKrEoFCqWklmWmaVToKFSoGinY2ioYaAIA/zirvz0AAAA=" target="_blank">Run the query</a>

```kusto
range x from 1 to 100 step 1
| summarize stdevif(x, x%2 == 0)

```

**Results**

|stdevif_x|
|---|
|29.1547594742265|
