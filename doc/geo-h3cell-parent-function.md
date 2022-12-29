---
title: geo_h3cell_parent() - Azure Data Explorer
description: Learn how to use the geo_h3cell_parent() function to calculate the H3 cell parent.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 12/14/2022
---
# geo_h3cell_parent()

Calculates the H3 cell parent.

Read more about [H3 Cell](https://eng.uber.com/h3/).

## Syntax

`geo_h3cell_parent(`*h3cell*`,`*resolution*`)`

## Arguments

* *h3cell*: H3 Cell token string value as it was calculated by [geo_point_to_h3cell()](geo-point-to-h3cell-function.md).
* *resolution*: An optional `int` that defines the requested parent cell resolution. Supported values are in the range [0, 14]. If unspecified, an immediate parent will be calculated.

## Returns

H3 Cell parent token `string`. If the H3 Cell is invalid or parent resolution is higher than given cell, the query will produce an empty result.

## Examples

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print parent_cell = geo_h3cell_parent('862a1072fffffff')
```

**Output**

|parent_cell|
|---|
|852a1073fffffff|

The following example calculates cell parent at level 1.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print parent_cell = geo_h3cell_parent('862a1072fffffff', 1)
```

**Output**

|parent_cell|
|---|
|812a3ffffffffff|

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print parent_res = geo_h3cell_level(geo_h3cell_parent((geo_point_to_h3cell(1,1,10))))
```

**Output**

|parent_res|
|---|
|9|

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print parent_res = geo_h3cell_level(geo_h3cell_parent(geo_point_to_h3cell(1,1,10), 3))
```

**Output**

|parent_res|
|---|
|3|

The following example produces an empty result because of the invalid cell input.

<!-- csl: net.tcp://localhost/$systemdb -->
```kusto
print invalid = isempty(geo_h3cell_parent('123'))
```

**Output**

|invalid|
|---|
|1|

The following example produces an empty result because of the invalid parent resolution.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print invalid = isempty(geo_h3cell_parent('862a1072fffffff', 100))
```

**Output**

|invalid|
|---|
|1|

The following example produces an empty result because parent can't be of a higher resolution than child.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print invalid = isempty(geo_h3cell_parent('862a1072fffffff', 15))
```

**Output**

|invalid|
|---|
|1|
