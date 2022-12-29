---
title: geo_intersects_2polygons() - Azure Data Explorer
description: Learn how to use the geo_intersects_2polygons() function to calculate whether two polygons or multipolygons intersect
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 12/14/2022
---
# geo_intersects_2polygons()

Calculates whether two polygons or multipolygons intersect.

## Syntax

`geo_intersects_2polygons(`*polygon1*`,`*polygon1*`)`

## Arguments

* *polygon1*: Polygon or multipolygon in the [GeoJSON format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type.
* *polygon2*: Polygon or multipolygon in the [GeoJSON format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type.

## Returns

Indicates whether two polygons or multipolygons intersect. If the Polygon or the MultiPolygon are invalid, the query will produce a null result.

> [!NOTE]
>
> * The geospatial coordinates are interpreted as represented by the [WGS-84](https://earth-info.nga.mil/GandG/update/index.php?action=home) coordinate reference system.
> * The [geodetic datum](https://en.wikipedia.org/wiki/Geodetic_datum) used for measurements on Earth is a sphere. Polygon edges are [geodesics](https://en.wikipedia.org/wiki/Geodesic) on the sphere.
> * If input polygon edges are straight cartesian lines, consider using [geo_polygon_densify()](geo-polygon-densify-function.md) to convert planar edges to geodesics.

**Polygon definition and constraints**

dynamic({"type": "Polygon","coordinates": [LinearRingShell, LinearRingHole_1, ..., LinearRingHole_N]})

dynamic({"type": "MultiPolygon","coordinates": [[LinearRingShell, LinearRingHole_1, ..., LinearRingHole_N], ..., [LinearRingShell, LinearRingHole_1, ..., LinearRingHole_M]]})

* LinearRingShell is required and defined as a `counterclockwise` ordered array of coordinates [[lng_1,lat_1], ..., [lng_i,lat_i], ...,[lng_j,lat_j], ...,[lng_1,lat_1]]. There can be only one shell.
* LinearRingHole is optional and defined as a `clockwise` ordered array of coordinates [[lng_1,lat_1], ...,[lng_i,lat_i], ...,[lng_j,lat_j], ...,[lng_1,lat_1]]. There can be any number of interior rings and holes.
* LinearRing vertices must be distinct with at least three coordinates. The first coordinate must be equal to the last. At least four entries are required.
* Coordinates [longitude, latitude] must be valid. Longitude must be a real number in the range [-180, +180] and latitude must be a real number in the range [-90, +90].
* LinearRingShell encloses at most half of the sphere. LinearRing divides the sphere into two regions. The smaller of the two regions will be chosen.
* LinearRing edge length must be less than 180 degrees. The shortest edge between the two vertices will be chosen.
* LinearRings must not cross and must not share edges. LinearRings may share vertices.
* Polygon contains its vertices.

> [!TIP]
>
> Use literal LineString or MultiLineString for better performance.

## Examples

The following example checks whether some two literal polygons intersects.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let polygon1 = dynamic({"type":"Polygon","coordinates":[[[-73.9630937576294,40.77498840732385],[-73.963565826416,40.774383111780914],[-73.96205306053162,40.773745311181585],[-73.96160781383514,40.7743912365898],[-73.9630937576294,40.77498840732385]]]});
let polygon2 = dynamic({"type":"Polygon","coordinates":[[[-73.96213352680206,40.775045280447145],[-73.9631313085556,40.774578106920345],[-73.96207988262177,40.77416780398293],[-73.96213352680206,40.775045280447145]]]});
print geo_intersects_2polygons(polygon1, polygon2)
```

**Output**

|print_0|
|---|
|True|

The following example finds all counties in the USA that intersect with area of interest literal polygon.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let area_of_interest = dynamic({"type":"Polygon","coordinates":[[[-73.96213352680206,40.775045280447145],[-73.9631313085556,40.774578106920345],[-73.96207988262177,40.77416780398293],[-73.96213352680206,40.775045280447145]]]});
US_Counties
| project name = features.properties.NAME, county = features.geometry
| where geo_intersects_2polygons(county, area_of_interest)
| project name
```

**Output**

|name|
|---|
|New York|

The following example will return a null result because one of the polygons is invalid.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let central_park_polygon = dynamic({"type":"Polygon","coordinates":[[[-73.9495,40.7969],[-73.95807266235352,40.80068603561921],[-73.98201942443848,40.76825672305777],[-73.97317886352539,40.76455136505513],[-73.9495,40.7969]]]});
let invalid_polygon = dynamic({"type":"Polygon"});
print isnull(geo_intersects_2polygons(invalid_polygon, central_park_polygon))
```

**Output**

|print_0|
|---|
|True|
