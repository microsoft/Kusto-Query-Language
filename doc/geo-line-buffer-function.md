---
title: geo_line_buffer() - Azure Data Explorer
description: Learn how to use the geo_line_buffer() function to calculate line buffer
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 04/24/2023
---
# geo_line_buffer()

Calculates polygon or multipolygon that contains all points within the given radius of the input line or multiline on Earth.

## Syntax

`geo_line_buffer(`*lineString*`,` *radius*`,` *tolerance*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *lineString* | dynamic | &check; | A LineString or MultiLineString in the [GeoJSON format](https://tools.ietf.org/html/rfc7946).|
| *radius* | real | &check; | Buffer radius in meters. Valid value must be positive.|
| *tolerance* | real | | Defines the tolerance in meters that determines how much a polygon can deviate from the ideal radius. If unspecified, the default value `10` is used. Tolerance should be no lower than 0.0001% of the radius. Specifying tolerance bigger than radius will lower the tolerance to biggest possible value below the radius.|

## Returns

Polygon or MultiPolygon around the input LineString or MultiLineString. If the coordinates or radius or tolerance is invalid, the query will produce a null result.

> [!NOTE]
>
> * The geospatial coordinates are interpreted as represented by the [WGS-84](https://earth-info.nga.mil/index.php?dir=wgs84&action=wgs84) coordinate reference system.
> * The [geodetic datum](https://en.wikipedia.org/wiki/Geodetic_datum) used to measure distance on Earth is a sphere.
> * If input line edges are straight cartesian lines, consider using [geo_line_densify()](geo-line-densify-function.md) in order to convert planar edges to geodesics.
> * Endcaps of the lines are round.
> * Both sides of the lines are buffered.

**LineString definition and constraints**

dynamic({"type": "LineString","coordinates": [[lng_1,lat_1], [lng_2,lat_2], ..., [lng_N,lat_N]]})

dynamic({"type": "MultiLineString","coordinates": [[line_1, line_2, ..., line_N]]})

* LineString coordinates array must contain at least two entries.
* Coordinates [longitude, latitude] must be valid where longitude is a real number in the range [-180, +180] and latitude is a real number in the range [-90, +90].
* Edge length must be less than 180 degrees. The shortest edge between the two vertices will be chosen.

## Examples

The following query calculates polygon around line, with radius of 4 meters and 0.1 meter tolerance

```kusto
let line = dynamic({"type":"LineString","coordinates":[[-80.66634997047466,24.894526340592122],[-80.67373241820246,24.890808090321286]]});
print buffer = geo_line_buffer(line, 4, 0.1)
```

|buffer|
|---|
|{"type": "Polygon", "coordinates": [ ... ]}|

The following query calculates buffer around each line and unifies result

```kusto
datatable(line:dynamic)
[
    dynamic({"type":"LineString","coordinates":[[14.429214068940496,50.10043066548272],[14.431184174126173,50.10046525983731]]}),
    dynamic({"type":"LineString","coordinates":[[14.43030222687753,50.100780677801936],[14.4303847111523,50.10020274910934]]})
]
| project buffer = geo_line_buffer(line, 2, 0.1)
| summarize polygons = make_list(buffer)
| project result = geo_union_polygons_array(polygons)
```

|result|
|---|
|{"type": "Polygon","coordinates": [ ... ]}|

The following example will return true, due to invalid line.

```kusto
print buffer = isnull(geo_line_buffer(dynamic({"type":"LineString"}), 5))
```

|buffer|
|---|
|True|

The following example will return true, due to invalid radius.

```kusto
print buffer = isnull(geo_line_buffer(dynamic({"type":"LineString","coordinates":[[0,0],[1,1]]}), 0))
```

|buffer|
|---|
|True|

