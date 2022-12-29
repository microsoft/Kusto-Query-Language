---
title: geo_union_polygons_array() - Azure Data Explorer
description: Learn how to use the geo_union_polygons_array() function to calculate the union of polygons or multipolygons on Earth.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 12/14/2022
---
# geo_union_polygons_array()

Calculates the union of polygons or multipolygons on Earth.

## Syntax

`geo_union_polygons_array(`*polygons*`)`

## Arguments

* *polygons*: An array of polygons or multipolygons in the [GeoJSON format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type.

## Returns

A polygon or a multipolygon in [GeoJSON Format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type. If any of the provided polygons or multipolygons is invalid, the query will produce a null result.

> [!NOTE]
>
> * The geospatial coordinates are interpreted as represented by the [WGS-84](https://earth-info.nga.mil/GandG/update/index.php?action=home) coordinate reference system.
> * The [geodetic datum](https://en.wikipedia.org/wiki/Geodetic_datum) used for measurements on Earth is a sphere. Polygon edges are [geodesics](https://en.wikipedia.org/wiki/Geodesic) on the sphere.
> * If input polygon edges are straight cartesian lines, consider using [geo_polygon_densify()](geo-polygon-densify-function.md) to convert planar edges to geodesics.

**Polygon definition and constraints**

dynamic({"type": "Polygon","coordinates": [ LinearRingShell, LinearRingHole_1, ..., LinearRingHole_N ]})

dynamic({"type": "MultiPolygon","coordinates": [[ LinearRingShell, LinearRingHole_1, ..., LinearRingHole_N], ..., [LinearRingShell, LinearRingHole_1, ..., LinearRingHole_M]]})

* LinearRingShell is required and defined as a `counterclockwise` ordered array of coordinates [[lng_1,lat_1],...,[lng_i,lat_i],...,[lng_j,lat_j],...,[lng_1,lat_1]]. There can be only one shell.
* LinearRingHole is optional and defined as a `clockwise` ordered array of coordinates [[lng_1,lat_1],...,[lng_i,lat_i],...,[lng_j,lat_j],...,[lng_1,lat_1]]. There can be any number of interior rings and holes.
* LinearRing vertices must be distinct with at least three coordinates. The first coordinate must be equal to the last. At least four entries are required.
* Coordinates [longitude, latitude] must be valid. Longitude must be a real number in the range [-180, +180] and latitude must be a real number in the range [-90, +90].
* LinearRingShell encloses at most half of the sphere. LinearRing divides the sphere into two regions. The smaller of the two regions will be chosen.
* LinearRing edge length must be less than 180 degrees. The shortest edge between the two vertices will be chosen.
* LinearRings must not cross and must not share edges. LinearRings may share vertices.

## Examples

The following example performs geospatial union on polygon rows.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(polygons:dynamic)
[
    dynamic({"type":"Polygon","coordinates":[[[-73.9495,40.7969],[-73.95807,40.80068],[-73.98201,40.76825],[-73.97317,40.76455],[-73.9495,40.7969]]]}),
    dynamic({"type":"Polygon","coordinates":[[[-73.94622,40.79249],[-73.96888,40.79282],[-73.9577,40.7789],[-73.94622,40.79249]]]}),
    dynamic({"type":"Polygon","coordinates":[[[-73.97335,40.77274],[-73.9936,40.76630],[-73.97171,40.75655],[-73.97335,40.77274]]]})
]
| summarize polygons_arr = make_list(polygons)
| project polygons_union = geo_union_polygons_array(polygons_arr)
```

**Output**

|polygons_union|
|---|
|{"type":"Polygon","coordinates":[[[-73.972599326729608,40.765330371902991],[-73.960302383706178,40.782140794645024],[-73.9577,40.7789],[-73.94622,40.79249],[-73.9526593223173,40.792584227716468],[-73.9495,40.7969],[-73.95807,40.80068],[-73.9639277517478,40.792748258673875],[-73.96888,40.792819999999992],[-73.9662719791645,40.7895734224338],[-73.9803360309571,40.770518810606404],[-73.9936,40.7663],[-73.97171,40.756550000000004],[-73.972599326729608,40.765330371902991]]]}|

The following example performs geospatial union on polygon columns.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(polygon1:dynamic, polygon2:dynamic)
[
    dynamic({"type":"Polygon","coordinates":[[[-73.9495,40.7969],[-73.95807,40.80068],[-73.98201,40.76825],[-73.97317,40.76455],[-73.9495,40.7969]]]}), dynamic({"type":"Polygon","coordinates":[[[-73.94622,40.79249],[-73.96888,40.79282],[-73.9577,40.7789],[-73.94622,40.79249]]]})
]
| project polygons_arr = pack_array(polygon1, polygon2)
| project polygons_union = geo_union_polygons_array(polygons_arr)
```

**Output**

|polygons_union|
|---|
|{"type":"Polygon","coordinates":[[[-73.9495,40.7969],[-73.95807,40.80068],[-73.9639277517478,40.792748258673875],[-73.96888,40.792819999999992],[-73.9662719791645,40.7895734224338],[-73.98201,40.76825],[-73.97317,40.76455],[-73.960302383706178,40.782140794645024],[-73.9577,40.7789],[-73.94622,40.79249],[-73.9526593223173,40.792584227716468],[-73.9495,40.7969]]]}|

The following example returns True because one of the polygons is invalid.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(polygons:dynamic)
[
    dynamic({"type":"Polygon","coordinates":[[[-73.9495,40.7969],[-73.95807,40.80068],[-73.98201,40.76825],[-73.97317,40.76455],[-73.9495,40.7969]]]}),
    dynamic({"type":"Polygon","coordinates":[[[-73.94622,40.79249]]]})
]
| summarize polygons_arr = make_list(polygons)
| project invalid_union = isnull(geo_union_polygons_array(polygons_arr))
```

**Output**

|invalid_union|
|---|
|True|
