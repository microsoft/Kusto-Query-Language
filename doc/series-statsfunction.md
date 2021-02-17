---
title: series_stats() - Azure Data Explorer
description: This article describes series_stats() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 01/27/2021
---
# series_stats()

`series_stats()` returns statistics for a numerical series using multiple columns.  

The `series_stats()` function takes an expression returning a dynamical numerical array as input, and calculates the following statistics:

Statistic | Description
---|---
 `min` | Minimum value in the input array.
 `min_idx`| The first position of the minimum value in the input array.
`max` | Maximum value in the input array.
`max_idx`| First position of the maximum value in the input array.
`avg`| Average value of the input array.
 `variance` | Sample variance of input array.
 `stdev`| Sample standard deviation of the input array.

> [!NOTE] 
> This function returns multiple values, so it can't be used as the input for another function.
> Consider using [series_stats_dynamic](./series-stats-dynamicfunction.md) if you only need a single value, such as "average".

## Syntax

`...` `|` `extend` `series_stats` `(` *Expr* [`,` *IgnoreNonFinite*] `)`

`...` `|` `extend` `(` *Name1* [`,` *Name2*...] `)` `=` `series_stats` `(` *Expr* [`,` *IgnoreNonFinite*] `)`

## Arguments

* *Expr*: An expression that returns a value of type `dynamic`, holding
  an array of numeric values. Numeric values are values for which arithmetic
  operators are defined.
  
* *IgnoreNonFinite*: A Boolean expression that specifies whether to calculate the
  statistics while ignoring non-finite values of *Expr* (`null`, `NaN`, `inf`, and so on).
  If `false`, a single item in *Expr* with this value will result in
  a value of `null` generated for all statistics values. The default value is `false`.

## Returns

### Syntax 1

The following syntax results in the following new columns being added where *Expr* is the column reference `x`: `series_stats_x_min`, `series_stats_x_idx`, and so on.

`...` `|` `extend` `series_stats` `(` *Expr* [`,` *IgnoreNonFinite*] `)`

### Syntax 2

The following syntax results in columns named `Name1`, `Name2`, and so on, containing these values in order.

`...` `|` `extend` `(` *Name1* [`,` *Name2*...] `)` `=` `series_stats` `(` *Expr* [`,` *IgnoreNonFinite*] `)`

## Example

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
print x=dynamic([23,46,23,87,4,8,3,75,2,56,13,75,32,16,29]) 
| project series_stats(x)

```

|series_stats_x_min|series_stats_x_min_idx|series_stats_x_max|series_stats_x_max_idx|series_stats_x_avg|series_stats_x_stdev|series_stats_x_variance|
|---|---|---|---|---|---|---|
|2|8|87|3|32.8|28.5036338535483|812.457142857143|
