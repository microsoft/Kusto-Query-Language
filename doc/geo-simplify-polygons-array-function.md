---
title: geo_simplify_polygons_array() - Azure Data Explorer
description: Learn how to use the geo_simplify_polygons_array() function to simplify polygons by replacing nearly straight chains of short edges with a single long edge on Earth.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 12/14/2022
---
# geo_simplify_polygons_array()

Simplifies polygons by replacing nearly straight chains of short edges with a single long edge on Earth.

## Syntax

`geo_simplify_polygons_array(`*polygons*`,`*tolerance*`)`

## Arguments

* *polygons*: An array of polygons or multipolygons in the [GeoJSON format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type.
* *tolerance*: An optional numeric that defines minimum distance in meters between any two vertices. Supported values are in the range [0, ~7,800,000 meters). If unspecified, the default value `10` is used.

## Returns

Simplified polygon or a multipolygon in the [GeoJSON format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type, with no two vertices with distance less than tolerance. If either the polygon or tolerance is invalid, the query will produce a null result.

> [!NOTE]
>
> * If input is a single polygon, please see [geo_polygon_simplify()](geo-polygon-simplify-function.md).
> * The geospatial coordinates are interpreted as represented by the [WGS-84](https://earth-info.nga.mil/GandG/update/index.php?action=home) coordinate reference system.
> * The [geodetic datum](https://en.wikipedia.org/wiki/Geodetic_datum) used for measurements on Earth is a sphere. Polygon edges are [geodesics](https://en.wikipedia.org/wiki/Geodesic) on the sphere.
> * If input polygon edges are straight cartesian lines, consider using [geo_polygon_densify()](geo-polygon-densify-function.md) to convert planar edges to geodesics.
> * If input is a multipolygon and contains more than one polygon, the result will be the area of polygons union.
> * High tolerance may cause small polygon to disappear.

**Polygon definition and constraints**

dynamic({"type": "Polygon","coordinates": [ LinearRingShell, LinearRingHole_1, ..., LinearRingHole_N ]})

dynamic({"type": "MultiPolygon","coordinates": [[ LinearRingShell, LinearRingHole_1, ..., LinearRingHole_N ], ..., [LinearRingShell, LinearRingHole_1, ..., LinearRingHole_M]]})

* LinearRingShell is required and defined as a `counterclockwise` ordered array of coordinates [[lng_1,lat_1],...,[lng_i,lat_i],...,[lng_j,lat_j],...,[lng_1,lat_1]]. There can be only one shell.
* LinearRingHole is optional and defined as a `clockwise` ordered array of coordinates [[lng_1,lat_1],...,[lng_i,lat_i],...,[lng_j,lat_j],...,[lng_1,lat_1]]. There can be any number of interior rings and holes.
* LinearRing vertices must be distinct with at least three coordinates. The first coordinate must be equal to the last. At least four entries are required.
* Coordinates [longitude, latitude] must be valid. Longitude must be a real number in the range [-180, +180] and latitude must be a real number in the range [-90, +90].
* LinearRingShell encloses at most half of the sphere. LinearRing divides the sphere into two regions. The smaller of the two regions will be chosen.
* LinearRing edge length must be less than 180 degrees. The shortest edge between the two vertices will be chosen.
* LinearRings must not cross and must not share edges. LinearRings may share vertices.

## Examples

The following example simplifies polygons with mutual borders (USA states), by removing vertices that are within a 100-meter distance from each other.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
US_States
| project polygon = features.geometry
| summarize lst = make_list(polygon)
| project polygons = geo_simplify_polygons_array(lst, 100)
```

**Output**

|polygons|
|---|
|{ "type": "MultiPolygon", "coordinates": [ ... ]]}|


The following example returns True because one of the polygons is invalid.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(polygons:dynamic)
[
    dynamic({"type":"Polygon","coordinates":[[[-73.9495,40.7969],[-73.95807,40.80068],[-73.98201,40.76825],[-73.97317,40.76455],[-73.9495,40.7969]]]}),
    dynamic({"type":"Polygon","coordinates":[[[-73.94622,40.79249]]]}),
    dynamic({"type":"Polygon","coordinates":[[[-73.97335,40.77274],[-73.9936,40.76630],[-73.97171,40.75655],[-73.97335,40.77274]]]})
]
| summarize arr = make_list(polygons)
| project is_invalid_polygon = isnull(geo_simplify_polygons_array(arr))
```

**Output**

|is_invalid_polygon|
|---|
|1|

The following example returns True because of the invalid tolerance.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(polygons:dynamic)
[
    dynamic({"type":"Polygon","coordinates":[[[-73.9495,40.7969],[-73.95807,40.80068],[-73.98201,40.76825],[-73.97317,40.76455],[-73.9495,40.7969]]]}),
    dynamic({"type":"Polygon","coordinates":[[[-73.94622,40.79249],[-73.96888,40.79282],[-73.9577,40.7789],[-73.94622,40.79249]]]}),
    dynamic({"type":"Polygon","coordinates":[[[-73.97335,40.77274],[-73.9936,40.76630],[-73.97171,40.75655],[-73.97335,40.77274]]]})
]
| summarize arr = make_list(polygons)
| project is_null = isnull(geo_simplify_polygons_array(arr, -1))
```

**Output**

|is_null|
|---|
|1|

The following example returns True because high tolerance causes polygon to disappear.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(polygons:dynamic)
[
    dynamic({"type":"Polygon","coordinates":[[[-73.9495,40.7969],[-73.95807,40.80068],[-73.98201,40.76825],[-73.97317,40.76455],[-73.9495,40.7969]]]}),
    dynamic({"type":"Polygon","coordinates":[[[-73.94622,40.79249],[-73.96888,40.79282],[-73.9577,40.7789],[-73.94622,40.79249]]]}),
    dynamic({"type":"Polygon","coordinates":[[[-73.97335,40.77274],[-73.9936,40.76630],[-73.97171,40.75655],[-73.97335,40.77274]]]})
]
| summarize arr = make_list(polygons)
| project is_null = isnull(geo_simplify_polygons_array(arr, 10000))
```

**Output**

|is_null|
|---|
|1|
