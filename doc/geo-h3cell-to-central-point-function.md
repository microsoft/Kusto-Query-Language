---
title: geo_h3cell_to_central_point() - Azure Data Explorer
description: Learn how to use the geo_h3cell_to_central_point() function to calculate the geospatial coordinates that represent the center of an H3 cell.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 06/03/2021
---
# geo_h3cell_to_central_point()

Calculates the geospatial coordinates that represent the center of an H3 Cell.

Read more about [H3 Cell](https://eng.uber.com/h3/).

## Syntax

`geo_h3cell_to_central_point(`*h3cell*`)`

## Arguments

*h3cell*: H3 cell token string value as it was calculated by [geo_point_to_h3cell()](geo-point-to-h3cell-function.md).

## Returns

The geospatial coordinate values in [GeoJSON Format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type. If the H3 cell token is invalid, the query will produce a null result.

> [!NOTE]
> The GeoJSON format specifies longitude first and latitude second.

## Examples

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print h3cell = geo_h3cell_to_central_point("862a1072fffffff")
```

**Output**

|h3cell|
|---|
|{<br>"type": "Point",<br>"coordinates": [-74.016008479792447, 40.7041679083504]<br>}|

The following example returns the longitude of the H3 Cell center point:

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print longitude = geo_h3cell_to_central_point("862a1072fffffff").coordinates[0]
```

**Output**

|longitude|
|---|
|-74.0160084797924|

The following example returns a null result because of the invalid H3 cell token input.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print h3cell = geo_h3cell_to_central_point("1")
```

**Output**

|h3cell|
|---|
||
