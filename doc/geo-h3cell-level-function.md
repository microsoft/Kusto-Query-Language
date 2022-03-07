---
title: geo_h3cell_level() - Azure Data Explorer
description: This article describes geo_h3cell_level() in Azure Data Explorer.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 10/10/2021
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

|cell_res|
|---|
|6|


<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print cell_res = geo_h3cell_level(geo_point_to_h3cell(1,1,10))
```

|cell_res|
|---|
|10|


The following example returns true because of the invalid H3 Cell token input.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print invalid_res = isnull(geo_h3cell_level('abc'))
```

|invalid_res|
|---|
|1|