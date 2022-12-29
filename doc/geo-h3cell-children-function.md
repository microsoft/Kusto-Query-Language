---
title: geo_h3cell_children() - Azure Data Explorer
description: Learn how to use the geo_h3cell_children() function to calculate the H3 cell children.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 12/14/2022
---
# geo_h3cell_children()

Calculates the H3 cell children.

Read more about [H3 Cell](https://eng.uber.com/h3/).

## Syntax

`geo_h3cell_children(`*h3cell*`,`*resolution*`)`

## Arguments

* *h3cell*: H3 Cell token string value as it was calculated by [geo_point_to_h3cell()](geo-point-to-h3cell-function.md).
* *resolution*: An optional `int` that defines the requested children cells resolution. Supported values are in the range [1, 15]. If unspecified, an immediate children token will be calculated.

## Returns

Array of H3 Cell children tokens. If the H3 Cell is invalid or child resolution is lower than given cell, the query will produce a null result.

> [!NOTE]
>
> A difference between cell resolution and its children can't be more than 5. A difference of 5 levels will be resulted in up to 16807 children tokens.

## Examples

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print children = geo_h3cell_children('862a1072fffffff')
```

**Output**

|children|
|---|
|[ "872a10728ffffff", "872a10729ffffff", "872a1072affffff", "872a1072bffffff", "872a1072cffffff", "872a1072dffffff", "872a1072effffff" ]|

The following example counts children 3 levels below a given cell.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let h3_cell = '862a1072fffffff'; 
print children_count = array_length(geo_h3cell_children(h3_cell, geo_h3cell_level(h3_cell) + 3))
```

**Output**

|children_count|
|---|
|343|

The following example assembles GeoJSON geometry collection of H3 Cell children polygons.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print children = geo_h3cell_children('862a1072fffffff')
| mv-expand children to typeof(string)
| project child = geo_h3cell_to_polygon(children)
| summarize h3_hash_polygon_lst = make_list(child)
| project geojson = pack(
    "type", "Feature",
    "geometry", pack("type", "GeometryCollection", "geometries", h3_hash_polygon_lst),
    "properties", pack("name", "H3 polygons collection"))
```

**Output**

|geojson|
|---|
|{ "type": "Feature", "geometry": { "type": "GeometryCollection", "geometries": [ ... ... ... ] }, "properties": { "name": "H3 polygons collection" }}|

The following example returns true because of the invalid cell.

<!-- csl: net.tcp://localhost/$systemdb -->
```kusto
print is_null = isnull(geo_h3cell_children('abc'))
```

**Output**

|is_null|
|---|
|1|

The following example returns true because the level difference between cell and its children is more than 5.

<!-- csl: net.tcp://localhost/$systemdb -->
```kusto
print is_null = isnull(geo_h3cell_children(geo_point_to_h3cell(1, 1, 9), 15))
```

**Output**

|is_null|
|---|
|1|
