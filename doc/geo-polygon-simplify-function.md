---
title: geo_polygon_simplify() - Azure Data Explorer
description: Learn how to use the geo_polygon_simplify() function to simplify a polygon or a multipolygon.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 12/14/2022
---
# geo_polygon_simplify()

Simplifies a polygon or a multipolygon by replacing nearly straight chains of short edges with a single long edge on Earth.

## Syntax

`geo_polygon_simplify(`*polygon*`,`*tolerance*`)`

## Arguments

* *polygon*: Polygon or multipolygon in the [GeoJSON format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type.
* *tolerance*: An optional numeric that defines minimum distance in meters between any two vertices. Supported values are in the range [0, ~7,800,000 meters). If unspecified, the default value `10` is used.

## Returns

Simplified polygon or a multipolygon in the [GeoJSON format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type, with no two vertices with distance less than tolerance. If either the polygon or tolerance is invalid, the query will produce a null result.

> [!NOTE]
>
> * If input has more than one polygon, with mutual borders, please see [geo_simplify_polygons_array()](geo-simplify-polygons-array-function.md).
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

The following example simplifies polygons by removing vertices that are within a 10-meter distance from each other.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let polygon = dynamic({"type":"Polygon","coordinates":[[[-73.94885122776031,40.79673476355657],[-73.94885927438736,40.79692258628347],[-73.94887939095497,40.79692055577034],[-73.9488673210144,40.79693476936093],[-73.94888743758202,40.79693476936093],[-73.9488834142685,40.796959135509105],[-73.94890084862709,40.79695304397289],[-73.94906312227248,40.79710736271788],[-73.94923612475395,40.7968708081794],[-73.94885122776031,40.79673476355657]]]});
print simplified = geo_polygon_simplify(polygon)
```

**Output**

|simplified|
|---|
|{"type": "Polygon", "coordinates": [[[-73.948851227760315, 40.796734763556572],[-73.949063122272477, 40.797107362717881],[-73.949236124753952, 40.7968708081794],[-73.948851227760315, 40.796734763556572]]]}|

The following example simplifies polygons and combines results into GeoJSON geometry collection.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
Polygons
| project polygon = features.geometry
| project simplified = geo_polygon_simplify(polygon, 1000)
| summarize lst = make_list(simplified)
| project geojson = pack("type", "Feature","geometry", pack("type", "GeometryCollection", "geometries", lst), "properties", pack("name", "polygons"))
```

**Output**

|geojson|
|---|
|{"type": "Feature", "geometry": {"type": "GeometryCollection", "geometries": [ ... ]}, "properties": {"name": "polygons"}}|

The following example simplifies polygons and unifies result

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
US_States
| project polygon = features.geometry
| project simplified = geo_polygon_simplify(polygon, 1000)
| summarize lst = make_list(simplified)
| project polygons = geo_union_polygons_array(lst)
```

**Output**

|polygons|
|---|
|{"type": "MultiPolygon", "coordinates": [ ... ]}|

The following example returns True because of the invalid polygon.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let polygon = dynamic({"type":"Polygon","coordinates":[[[5,48],[5,48]]]});
print is_invalid_polygon = isnull(geo_polygon_simplify(polygon))
```

**Output**

|is_invalid_polygon|
|---|
|1|

The following example returns True because of the invalid tolerance.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let polygon = dynamic({"type":"Polygon","coordinates":[[[5,48],[0,50],[0,47],[4,47],[5,48]]]});
print is_invalid_polygon = isnull(geo_polygon_simplify(polygon, -0.1))
```

**Output**

|is_invalid_polygon|
|---|
|1|

The following example returns True because high tolerance causes polygon to disappear.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let polygon = dynamic({"type":"Polygon","coordinates":[[[5,48],[0,50],[0,47],[4,47],[5,48]]]});
print is_invalid_polygon = isnull(geo_polygon_simplify(polygon, 1000000))
```

**Output**

|is_invalid_polygon|
|---|
|1|
