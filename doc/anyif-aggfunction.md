---
title: anyif() (aggregation function) - Azure Data Explorer | Microsoft Docs
description: This article describes anyif() (aggregation function) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# anyif() (aggregation function)

Arbitrarily selects one record for each group in a [summarize operator](summarizeoperator.md), for which the predicate
is "true". The function returns the value of an expression over each such record.

> [!NOTE]
> This function is useful when you want to get a sample value of one column per value of the compound group key, subject to some predicate that is "true".
> If such a value is present, the function attempts to return a non-null/non-empty value.

## Syntax

`summarize` `anyif` `(` *Expr*, *Predicate* `)`

## Arguments

* *Expr*: An expression over each record selected from the input to return.
* *Predicate*: Predicate to indicate which records may be considered for evaluation.

## Returns

The `anyif` aggregation function returns the value of the expression calculated
for each of the records randomly selected from each group of the summarize operator. Only records for which *Predicate* returns "true" may be selected. If the predicate doesn't return "true", a null value is produced.

## Examples

Show a random continent that has a population of 300 to 600 million.

```kusto
Continents | summarize anyif(Continent, Population between (300000000 .. 600000000))
```

:::image type="content" source="images/aggfunction/any1.png" alt-text="Any 1":::
