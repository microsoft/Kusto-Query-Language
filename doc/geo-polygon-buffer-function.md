---
title: geo_polygon_buffer() - Azure Data Explorer
description: Learn how to use the geo_polygon_buffer() function to calculate polygon buffer
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 04/24/2023
---
# geo_polygon_buffer()

Calculates polygon or multipolygon that contains all points within the given radius of the input polygon or multipolygon on Earth.

## Syntax

`geo_polygon_buffer(`*polygon*`,` *radius*`,` *tolerance*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *polygon* | dynamic | &check; | Polygon or multipolygon in the [GeoJSON format](https://tools.ietf.org/html/rfc7946).|
| *radius* | real | &check; | Buffer radius in meters. Valid value must be positive.|
| *tolerance* | real || Defines the tolerance in meters that determines how much a polygon can deviate from the ideal radius. If unspecified, the default value `10` is used. Tolerance should be no lower than 0.0001% of the radius. Specifying tolerance bigger than radius will lower the tolerance to biggest possible value below the radius.|

## Returns

Polygon or MultiPolygon around the input Polygon or multipolygon. If the coordinates or radius or tolerance is invalid, the query will produce a null result.

> [!NOTE]
>
> * The geospatial coordinates are interpreted as represented by the [WGS-84](https://earth-info.nga.mil/index.php?dir=wgs84&action=wgs84) coordinate reference system.
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

## Examples

The following query calculates polygon around input polygon, with radius of 10km.

```kusto
let polygon = dynamic({"type":"Polygon","coordinates":[[[139.813757,35.719666],[139.72558,35.71813],[139.727471,35.653231],[139.818721,35.657264],[139.813757,35.719666]]]});
print buffer = geo_polygon_buffer(polygon, 10000)
```

|buffer|
|---|
|{"type": "Polygon","coordinates": [ ... ]}|

The following query calculates buffer around each polygon and unifies result

```kusto
datatable(polygon:dynamic, radius:real )
[
    dynamic({"type":"Polygon","coordinates":[[[12.451218693639277,41.906457003556625],[12.445753852969375,41.90160968881543],[12.453514425793855,41.90361551885886],[12.451218693639277,41.906457003556625]]]}), 100,
    dynamic({"type":"Polygon","coordinates":[[[12.4566086734784,41.905119850039995],[12.453913683559591,41.903652663265234],[12.455485761012113,41.90146110630562],[12.4566086734784,41.905119850039995]]]}), 20
]
| project buffer = geo_polygon_buffer(polygon, radius)
| summarize polygons = make_list(buffer)
| project result = geo_union_polygons_array(polygons)
```

|result|
|---|
|{"type": "Polygon","coordinates": [ ... ]}|


The following example will return true, due to invalid polygon.

```kusto
print buffer = isnull(geo_polygon_buffer(dynamic({"type":"p"}), 1))
```

|buffer|
|---|
|True|

The following example will return true, due to invalid radius.

```kusto
print buffer = isnull(geo_polygon_buffer(dynamic({"type":"Polygon","coordinates":[[[10,10],[0,10],[0,0],[10,10]]]}), 0))
```

|buffer|
|---|
|True|

