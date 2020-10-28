---
title: dcount_intersect plugin - Azure Data Explorer
description: This article describes dcount_intersect plugin in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# dcount_intersect plugin

Calculates intersection between N sets based on `hll` values (N in range of [2..16]), and returns N `dcount` values.

Given sets S<sub>1</sub>, S<sub>2</sub>, .. S<sub>n</sub> - returns values will be representing distinct counts of:  
S<sub>1</sub>,  
S<sub>1</sub> ∩ S<sub>2</sub>,  
S<sub>1</sub> ∩ S<sub>2</sub> ∩ S<sub>3</sub>,  
... ,  
S<sub>1</sub> ∩ S<sub>2</sub> ∩ ... ∩ S<sub>n</sub>

```kusto
T | evaluate dcount_intersect(hll_1, hll_2, hll_3)
```

## Syntax

*T* `| evaluate` `dcount_intersect(`*hll_1*, *hll_2*, [`,` *hll_3*`,` ...]`)`

## Arguments

* *T*: The input tabular expression.
* *hll_i*: the values of set S<sub>i</sub> calculated with [`hll()`](./hll-aggfunction.md) function.

## Returns

Returns a table with N `dcount` values (per column, representing set intersections).
Column names are s0, s1, ... (until n-1).

## Examples

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
// Generate numbers from 1 to 100
range x from 1 to 100 step 1
| extend isEven = (x % 2 == 0), isMod3 = (x % 3 == 0), isMod5 = (x % 5 == 0)
// Calculate conditional HLL values (note that '0' is included in each of them as additional value, so we will subtract it later)
| summarize hll_even = hll(iif(isEven, x, 0), 2),
            hll_mod3 = hll(iif(isMod3, x, 0), 2),
            hll_mod5 = hll(iif(isMod5, x, 0), 2) 
// Invoke the plugin that calculates dcount intersections         
| evaluate dcount_intersect(hll_even, hll_mod3, hll_mod5)
| project evenNumbers = s0 - 1,             //                             100 / 2 = 50
          even_and_mod3 = s1 - 1,           // gcd(2,3) = 6, therefor:     100 / 6 = 16
          even_and_mod3_and_mod5 = s2 - 1   // gcd(2,3,5) is 30, therefore: 100 / 30 = 3 
```

|evenNumbers|even_and_mod3|even_and_mod3_and_mod5|
|---|---|---|
|50|16|3|
