---
title: dcount_intersect plugin - Azure Data Explorer
description: Learn how to use the dcount_intersect plugin to calculate the intersection between N sets based on hyper log log (hll) values.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/11/2022
---
# dcount_intersect plugin

Calculates intersection between N sets based on `hll` values (N in range of [2..16]), and returns N `dcount` values. The plugin is invoked with the [`evaluate`](evaluateoperator.md) operator.

## Syntax

*T* `| evaluate` `dcount_intersect(`*hll_1*, *hll_2*, [`,` *hll_3*`,` ...]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*T*|string|&check;| The input tabular expression. |
|*hll_i*| The values of set S<sub>i</sub> calculated with the [`hll()`](./hll-aggfunction.md) function.|

## Returns

Returns a table with N `dcount` values (per column, representing set intersections).
Column names are s0, s1, ... (until n-1).

Given sets S<sub>1</sub>, S<sub>2</sub>, .. S<sub>n</sub> return values will be representing distinct counts of:  
S<sub>1</sub>,  
S<sub>1</sub> ∩ S<sub>2</sub>,  
S<sub>1</sub> ∩ S<sub>2</sub> ∩ S<sub>3</sub>,  
... ,  
S<sub>1</sub> ∩ S<sub>2</sub> ∩ ... ∩ S<sub>n</sub>

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA4WSQWvjMBCF7/kV71JqgxbbMc4hkFMpuwvd/oWgypNEu7IUJDkNpT++mrh2nUK7PomZeU/6nqco8JMseRkJtu+eyAfsvOtQITpUZbnw0u4J5+sqQqQjqsUr6BzJttDh/kQWG2Rn3GCJzQZlLlL5j2vrsVxflZux3AzlRVHgThrVG36McrbVUTsrDX49POAkTU8BmXWpGQ8y4ra8TUbQVpm+pfQEC5LqALdLfeogA2Q7eVz0AsHhmfCsjUHon6KXKkJH8JU+Tzih7zrp9QvhYMyWBqZ0zLTeZQOkwFlcKJa5WGD2saIbcD8UzP8/RfNZ0cwU4Fx+25P7x9yEo+n3CfUSgRrjCmiV621isQkkkGLqMN3D/4kD4GCHwe00mI2gYgKYTg1HcvTub5oDzzy+r8gGocQPVGJOg/TQ7z7em4J3A005i4F9t9K2Y3ah+uycfPeqzZaizlN/JTgHTzvn1zPfVWpVq698xwNHHZZ8wZWvaHLepbr88Kb1u3FdJk39Bu22I/gpAwAA" target="_blank">Run the query</a>

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

**Output**

|evenNumbers|even_and_mod3|even_and_mod3_and_mod5|
|---|---|---|
|50|16|3|
