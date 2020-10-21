---
title: any() (aggregation function) - Azure Data Explorer | Microsoft Docs
description: This article describes any() (aggregation function) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# any() (aggregation function)

Arbitrarily chooses one record for each group in a [summarize operator](summarizeoperator.md),
and returns the value of one or more expressions over each such record.

## Syntax

`summarize` `any` `(` (*Expr* [`,` *Expr2* ...]) | `*` `)`

## Arguments

* *Expr*: An expression over each record selected from the input to return.
* *Expr2* .. *ExprN*: Additional expressions.

## Returns

The `any` aggregation function returns the values of the expressions calculated
for each of the records, selected randomly from each group of the summarize operator.

If the `*` argument is provided, the function behaves as if the expressions are all columns
of the input to the summarize operator barring the group-by columns, if any.

**Remarks**

This function is useful when you want to get a sample value of one or more columns
per value of the compound group key.

When the function is provided with a single column reference, it will attempt to
return a non-null/non-empty value, if such value is present.

As a result of the random nature of this function, using it multiple times in
a single application of the `summarize` operator is not equivalent to using
it a single time with multiple expressions. The former may have each application
select a different record, while the latter guarantees that all values are calculated
over a single record (per distinct group).

## Examples

Show Random Continent:

```kusto
Continents | summarize any(Continent)
```

:::image type="content" source="images/aggfunction/any1.png" alt-text="Any 1":::

Show all the details for a random record:

```kusto
Continents | summarize any(*)
```

:::image type="content" source="images/aggfunction/any2.png" alt-text="Any 2":::

Show all the details for each random continent:

```kusto
Continents | summarize any(*) by Continent
```

:::image type="content" source="images/aggfunction/any3.png" alt-text="Any 3":::
