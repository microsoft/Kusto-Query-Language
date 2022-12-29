---
title: geo_h3cell_level() - Azure Data Explorer
description: Learn how to use the geo_h3cell_level() function to calculate the H3 cell resolution.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 12/14/2022
---
# geo_h3cell_level()

Calculates the H3 cell resolution.

Read more about [H3 Cell](https://eng.uber.com/h3/).

## Syntax

`geo_h3cell_level(`*h3cell*`)`

## Arguments

*h3cell*: H3 Cell token string value as it was calculated by [geo_point_to_h3cell()](geo-point-to-h3cell-function.md).

## Returns

An Integer that represents H3 Cell level. Valid level is in range [0, 15]. If the H3 Cell is invalid, the query will produce a null result.

## Examples

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print cell_res = geo_h3cell_level('862a1072fffffff')
```

**Output**

|cell_res|
|---|
|6|

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print cell_res = geo_h3cell_level(geo_point_to_h3cell(1,1,10))
```

**Output**

|cell_res|
|---|
|10|

The following example returns true because of the invalid H3 Cell token input.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print invalid_res = isnull(geo_h3cell_level('abc'))
```

**Output**

|invalid_res|
|---|
|1|