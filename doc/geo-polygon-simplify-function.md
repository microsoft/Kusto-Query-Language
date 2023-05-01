---
title: geo_polygon_simplify() - Azure Data Explorer
description: Learn how to use the geo_polygon_simplify() function to simplify a polygon or a multipolygon.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 03/09/2023
---
# geo_polygon_simplify()

Simplifies a polygon or a multipolygon by replacing nearly straight chains of short edges with a single long edge on Earth.

## Syntax

`geo_polygon_simplify(`*polygon*`,` *tolerance*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *polygon* | dynamic | &check; | Polygon or multipolygon in the [GeoJSON format](https://tools.ietf.org/html/rfc7946).|
| *tolerance* | int, long, or real | | Defines maximum distance in meters between the original planar edge and the converted geodesic edge chain. Supported values are in the range [0.1, 10000]. If unspecified, the default value is  `10`.|

## Returns

Simplified polygon or a multipolygon in the [GeoJSON format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type, with no two vertices with distance less than tolerance. If either the polygon or tolerance is invalid, the query will produce a null result.

> [!NOTE]
>
> * If input has more than one polygon, with mutual borders, please see [geo_simplify_polygons_array()](geo-simplify-polygons-array-function.md).
> * The geospatial coordinates are interpreted as represented by the [WGS-84](https://earth-info.nga.mil/index.php?dir=wgs84&action=wgs84) coordinate reference system.
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

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA33Ry2rCUBAG4H2fImSlkMqcuZyZsfQdupcgYlIJaBI0m1D67j3FaLJyO/PBP5dzPWR9dx5PXZt9ZtXYHi7NcfWTD2Nf59v8697Ki/zYddeqaQ9Dfcu3u93uXWnjbCYBUTUChYJhox6VWCOJRNGymJmjMplSnJgjikW0pBdMnRxc2PXJQERUgXjBUggGCMwP9Z/pFMFpoSwlqhgCvmbEgTGaPJR4SPODB5CZOYBxGljBn46AyRXNn8whUjoIKrLdmQZIS6MGNZsZUgzIKuSPVFMwsKC+3PPlccvyd/3x1l+bdshuzaU/N99NXaUvnupuP/10PzXG1VRY/wEdFq3Y8QEAAA==" target="_blank">Run the query</a>

```kusto
let polygon = dynamic({"type":"Polygon","coordinates":[[[-73.94885122776031,40.79673476355657],[-73.94885927438736,40.79692258628347],[-73.94887939095497,40.79692055577034],[-73.9488673210144,40.79693476936093],[-73.94888743758202,40.79693476936093],[-73.9488834142685,40.796959135509105],[-73.94890084862709,40.79695304397289],[-73.94906312227248,40.79710736271788],[-73.94923612475395,40.7968708081794],[-73.94885122776031,40.79673476355657]]]});
print simplified = geo_polygon_simplify(polygon)
```

**Output**

|simplified|
|---|
|{"type": "Polygon", "coordinates": [[[-73.948851227760315, 40.796734763556572],[-73.949063122272477, 40.797107362717881],[-73.949236124753952, 40.7968708081794],[-73.948851227760315, 40.796734763556572]]]}|

The following example simplifies polygons and combines results into GeoJSON geometry collection.

```kusto
Polygons
| project polygon = features.geometry
| project simplified = geo_polygon_simplify(polygon, 1000)
| summarize lst = make_list(simplified)
| project geojson = bag_pack("type", "Feature","geometry", bag_pack("type", "GeometryCollection", "geometries", lst), "properties", bag_pack("name", "polygons"))
```

**Output**

|geojson|
|---|
|{"type": "Feature", "geometry": {"type": "GeometryCollection", "geometries": [ ... ]}, "properties": {"name": "polygons"}}|

The following example simplifies polygons and unifies result

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA2WOQQoCMQxF954iyxZExgN4isF1KZoZou20JOmig4c3i4qC2///e8l1DrNGRTm8oHJ54E2hltTXssEFFozaGOW0Ysmo3H9WQrkmWgjvNrQ+DCyMorsRHOE8TZM3VFrOkWlHSKJG5fjEkEjUfWX+/xEZB9pGpv+EITLH7szk3/JsgkXFAAAA" target="_blank">Run the query</a>

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

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0WMQQrDIBAA732F7EnBYwslIX/IXUQkmrCw2ZVoC1L69x7S0tPADAzlpopQ34TVpFLnuOOiX9B6yTDAfCawsIgcCTm2XGFwzt3s9e7tCe/fZryUA7kprAH5GQlT+H+x8oNIb1l+MlTcC+Ha9VcY8wFgUlqniwAAAA==" target="_blank">Run the query</a>

```kusto
let polygon = dynamic({"type":"Polygon","coordinates":[[[5,48],[5,48]]]});
print is_invalid_polygon = isnull(geo_polygon_simplify(polygon))
```

**Output**

|is_invalid_polygon|
|---|
|1|

The following example returns True because of the invalid tolerance.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0WNwQoCIRQA732FeFKwMHApdukfuouIrLY8cJ+yWiDRvxe7RaeBmcPEUElOsU0JyYX4hm6GkT1pbTnQnl63RAUdU1o8oKuh0F5r3Ql1NkJL0ckV6vSB2rA2Y1582OUFsBIoFvDhInj7n0HBe4xsCuknbYE5R7g19hWC7OXhyPkbMhZUVaYAAAA=" target="_blank">Run the query</a>

```kusto
let polygon = dynamic({"type":"Polygon","coordinates":[[[5,48],[0,50],[0,47],[4,47],[5,48]]]});
print is_invalid_polygon = isnull(geo_polygon_simplify(polygon, -0.1))
```

**Output**

|is_invalid_polygon|
|---|
|1|

The following example returns True because high tolerance causes polygon to disappear.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0WNwQoCIRRF932FvJWCCwOHYqJ/mL2IyGjDA0dltECify+cors5cM7iBl9JTqEtKZIrcS3aFWf6hNqyhxGmPQGHOaXNYbTVFxiVUgOXZ82V4IPokKcP5I7etH6xyyFvGCvBYjA+bEBn/mdY4j0Euvj0k6bgmgPeGv0KTo6ij7E3IUIHfKkAAAA=" target="_blank">Run the query</a>

```kusto
let polygon = dynamic({"type":"Polygon","coordinates":[[[5,48],[0,50],[0,47],[4,47],[5,48]]]});
print is_invalid_polygon = isnull(geo_polygon_simplify(polygon, 1000000))
```

**Output**

|is_invalid_polygon|
|---|
|1|
