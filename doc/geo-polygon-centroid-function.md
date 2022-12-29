---
title: geo_polygon_centroid() - Azure Data Explorer
description: Learn how to use the geo_polygon_centroid() function to calculate the centroid of a polygon or a multipolygon on Earth.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 12/14/2022
---
# geo_polygon_centroid()

Calculates the centroid of a polygon or a multipolygon on Earth.

## Syntax

`geo_polygon_centroid(`*polygon*`)`

## Arguments

* *polygon*: Polygon or multipolygon in the [GeoJSON format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type.

## Returns

The centroid coordinate values in [GeoJSON Format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type. If polygon or multipolygon are invalid, the query will produce a null result.

> [!NOTE]
>
> * The geospatial coordinates are interpreted as represented by the [WGS-84](https://earth-info.nga.mil/GandG/update/index.php?action=home) coordinate reference system.
> * The [geodetic datum](https://en.wikipedia.org/wiki/Geodetic_datum) used for measurements on Earth is a sphere. Polygon edges are [geodesics](https://en.wikipedia.org/wiki/Geodesic) on the sphere.
> * If input polygon edges are straight cartesian lines, consider using [geo_polygon_densify()](geo-polygon-densify-function.md) to convert planar edges to geodesics.
> * If input is a multipolygon and contains more than one polygon, the result will be the centroid of polygons union.

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

The following example calculates NYC Central Park centroid.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let central_park = dynamic({"type":"Polygon","coordinates":[[[-73.9495,40.7969],[-73.95807266235352,40.80068603561921],[-73.98201942443848,40.76825672305777],[-73.97317886352539,40.76455136505513],[-73.9495,40.7969]]]});
print centroid = geo_polygon_centroid(central_park)
```

**Output**

|centroid|
|---|
|{"type": "Point", "coordinates": [-73.965735689907618, 40.782550538057812]}|

The following example calculates NYC Central Park centroid longitude.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let central_park = dynamic({"type":"Polygon","coordinates":[[[-73.9495,40.7969],[-73.95807266235352,40.80068603561921],[-73.98201942443848,40.76825672305777],[-73.97317886352539,40.76455136505513],[-73.9495,40.7969]]]});
print 
centroid = geo_polygon_centroid(central_park)
| project lng = centroid.coordinates[0]
```

**Output**

|lng|
|---|
|-73.9657356899076|

The following example performs union of polygons in multipolygon and calculates centroid of the unified polygon.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let polygons = dynamic({"type":"MultiPolygon","coordinates":[[[[-73.9495,40.7969],[-73.95807266235352,40.80068603561921],[-73.98201942443848,40.76825672305777],[-73.97317886352539,40.76455136505513],[-73.9495,40.7969]]],[[[-73.94262313842773,40.775991804565585],[-73.98107528686523,40.791849155467695],[-73.99600982666016,40.77092185281977],[-73.96150588989258,40.75609977566361],[-73.94262313842773,40.775991804565585]]]]});
print polygons_union_centroid = geo_polygon_centroid(polygons)
```

**Output**

|polygons_union_centroid|
|---|
|"type": "Point", "coordinates": [-73.968569587829577, 40.776310752555119]}|

The following example visualizes NYC Central Park centroid on a map

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let central_park = dynamic({"type":"Polygon","coordinates":[[[-73.9495,40.7969],[-73.95807266235352,40.80068603561921],[-73.98201942443848,40.76825672305777],[-73.97317886352539,40.76455136505513],[-73.9495,40.7969]]]});
print 
centroid = geo_polygon_centroid(central_park)
| render scatterchart with (kind = map)
```

:::image type="content" source="images/geo-polygon-centroid-function/nyc-central-park-centroid.png" alt-text="Screenshot of N Y C Central park centroid.":::

The following example returns True because of the invalid polygon.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print isnull(geo_polygon_centroid(dynamic({"type": "Polygon","coordinates": [[[0,0],[10,10],[10,10],[0,0]]]})))
```

**Output**

|print_0|
|---|
|True|
