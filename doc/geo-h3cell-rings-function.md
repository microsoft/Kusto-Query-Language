---
title: geo_h3cell_rings() - Azure Data Explorer
description: This article describes geo_h3cell_rings() in Azure Data Explorer.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 10/10/2021
---
# geo_h3cell_rings()

Calculates the H3 cell Rings.

Read more about [H3 Cell](https://eng.uber.com/h3/).

## Syntax

`geo_h3cell_rings(`*h3cell*`, `*distance*`)`

## Arguments

* *h3cell*: H3 Cell token string value as it was calculated by [geo_point_to_h3cell()](geo-point-to-h3cell-function.md).
* *distance*: An `int` that defines the maximum ring distance from given cell. Valid distance is in range [0, 142].

## Returns

An ordered array of ring arrays where 1st ring contains the original cell, 2nd ring contains neighboring cells, and so on. If either the H3 Cell or distance is invalid, the query will produce a null result.

> [!NOTE]
> * For H3 Cell immidiate neighbors only, please see [geo_h3cell_neighbors()](geo-h3cell-neighbors-function.md).
> * A cell might be not present in the ring if pentagonal distortion was encountered.

## Examples

The following example produces rings up to distance 2.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print rings = geo_h3cell_rings('861f8894fffffff', 2)
```

|rings|
|---|
|[<br> ["861f8894fffffff"],<br> ["861f88947ffffff","861f8895fffffff","861f88867ffffff","861f8d497ffffff","861f8d4b7ffffff","861f8896fffffff"],<br> ["861f88967ffffff","861f88977ffffff","861f88957ffffff","861f8882fffffff","861f88877ffffff","861f88847ffffff","861f8886fffffff","861f8d49fffffff","861f8d487ffffff","861f8d4a7ffffff","861f8d59fffffff","861f8d597ffffff"]<br> ]|

The following example produces all cells at level 1 (all neighbors).

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print neighbors = geo_h3cell_rings('861f8894fffffff', 1)[1]
```

|neighbors|
|---|
|["861f88947ffffff", "861f8895fffffff", "861f88867ffffff", "861f8d497ffffff", "861f8d4b7ffffff","861f8896fffffff"]|

The following example produces list of cells from all rings.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print rings = geo_h3cell_rings('861f8894fffffff', 1)
| mv-apply rings on 
(
  summarize cells = make_list(rings)
)
```

|cells|
|---|
|["861f8894fffffff","861f88947ffffff","861f8895fffffff","861f88867ffffff","861f8d497ffffff","861f8d4b7ffffff","861f8896fffffff"]|

The following example assembles GeoJSON geometry collection of all cells.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print rings = geo_h3cell_rings('861f8894fffffff', 1)
| mv-apply rings on 
(
  summarize make_list(rings)
)
| mv-expand list_rings to typeof(string)
| project polygon = geo_h3cell_to_polygon(list_rings)
| summarize polygon_lst = make_list(polygon)
| project geojson = pack(
    "type", "Feature",
    "geometry", pack("type", "GeometryCollection", "geometries", polygon_lst),
    "properties", pack("name", "H3 polygons collection"))
```

|geojson|
|---|
|{ "type": "Feature", "geometry": { "type": "GeometryCollection", "geometries": [ ... ... ... ]}, "properties": { "name": "H3 polygons collection" }}|


The following example returns true because of the invalid cell.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print is_null = isnull(geo_h3cell_rings('abc', 3))
```

|is_null|
|---|
|1|

The following example returns true because of the invalid distance.

<!-- csl: net.tcp://localhost/$systemdb -->
```kusto
print is_null = isnull(geo_h3cell_rings('861f8894fffffff', 150))
```

|is_null|
|---|
|1|
