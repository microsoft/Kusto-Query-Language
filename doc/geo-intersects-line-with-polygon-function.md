---
title: geo_intersects_line_with_polygon() - Azure Data Explorer
description: Learn how to use the geo_intersects_line_with_polygon() function to check if a line string or a multiline string intersect with a polygon or a multipolygon.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 03/09/2023
---
# geo_intersects_line_with_polygon()

Calculates whether a line or multiline intersect with a polygon or a multipolygon.

## Syntax

`geo_intersects_line_with_polygon(`*lineString*`,`*polygon*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *lineString* | dynamic | &check; | A LineString or MultiLineString in the [GeoJSON format](https://tools.ietf.org/html/rfc7946).|
| *polygon* | dynamic | &check; | A Polygon or MultiPolygon in the [GeoJSON format](https://tools.ietf.org/html/rfc7946).|

## Returns

Indicates whether the line or multiline intersects with polygon or a multipolygon. If lineString or a multiLineString or a polygon or a multipolygon are invalid, the query will produce a null result.

> [!NOTE]
>
> * The geospatial coordinates are interpreted as represented by the [WGS-84](https://earth-info.nga.mil/index.php?dir=wgs84&action=wgs84) coordinate reference system.
> * The [geodetic datum](https://en.wikipedia.org/wiki/Geodetic_datum) used to measure distance on Earth is a sphere. Line edges are [geodesics](https://en.wikipedia.org/wiki/Geodesic) on the sphere.
> * If input line or a polygon edges are straight cartesian lines, consider using [geo_line_densify()](geo-line-densify-function.md) or a [geo_polygon_densify()](geo-polygon-densify-function.md) in order to convert planar edges to geodesics.

**LineString definition and constraints**

dynamic({"type": "LineString","coordinates": [[lng_1,lat_1], [lng_2,lat_2], ..., [lng_N,lat_N]]})

dynamic({"type": "MultiLineString","coordinates": [[line_1, line_2, ..., line_N]]})

* LineString coordinates array must contain at least two entries.
* Coordinates [longitude, latitude] must be valid where longitude is a real number in the range [-180, +180] and latitude is a real number in the range [-90, +90].
* Edge length must be less than 180 degrees. The shortest edge between the two vertices will be chosen.

**Polygon definition and constraints**

dynamic({"type": "Polygon","coordinates": [ LinearRingShell, LinearRingHole_1, ..., LinearRingHole_N]})

dynamic({"type": "MultiPolygon","coordinates": [[LinearRingShell, LinearRingHole_1, ..., LinearRingHole_N], ..., [LinearRingShell, LinearRingHole_1, ..., LinearRingHole_M]]})

* LinearRingShell is required and defined as a `counterclockwise` ordered array of coordinates [[lng_1,lat_1], ...,[lng_i,lat_i], ...,[lng_j,lat_j], ...,[lng_1,lat_1]]. There can be only one shell.
* LinearRingHole is optional and defined as a `clockwise` ordered array of coordinates [[lng_1,lat_1], ...,[lng_i,lat_i], ...,[lng_j,lat_j], ...,[lng_1,lat_1]]. There can be any number of interior rings and holes.
* LinearRing vertices must be distinct with at least three coordinates. The first coordinate must be equal to the last. At least four entries are required.
* Coordinates [longitude, latitude] must be valid. Longitude must be a real number in the range [-180, +180] and latitude must be a real number in the range [-90, +90].
* LinearRingShell encloses at most half of the sphere. LinearRing divides the sphere into two regions. The smaller of the two regions will be chosen.
* LinearRing edge length must be less than 180 degrees. The shortest edge between the two vertices will be chosen.
* LinearRings must not cross and must not share edges. LinearRings may share vertices.
* Polygon doesn't necessarily contain its vertices.

> [!TIP]
>
> Use literal LineString or MultiLineString for better performance.

## Examples

The following example checks whether a literal LineString intersects with a Polygon.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA32QzUrEQBCE7z5FmNMuxGX++mdWfAMPgsclhCU7xIGYhGRAgvjujs5mc1A89KW66K+rOh+LLvT+JU6hb4vH4rL057fQ7D5EXEYvjuLpthWlaIZhuoT+HP0sjqfTPZmDY1AOSisPxKwJqjLLZAH0j0yOUFXV5/7hrku4ceiWduj/Yj3n1W/Q9aTSTgKzIXYZCCwBlUImpBXMUlowilGa9QHQhGwsS4OwPSilNEqnC4ow+xg0ogEDiPZm+x+ac42pn1ik8dPsmzincK0f6k2ov0uu30N8ra/5d1vt5drJ/gtbMVZzjwEAAA==" target="_blank">Run the query</a>

```kusto
let lineString = dynamic({"type":"LineString","coordinates":[[-73.985195,40.788275],[-73.974552,40.779761]]});
let polygon = dynamic({"type":"Polygon","coordinates":[[[-73.9712905883789,40.78580561168767],[-73.98004531860352,40.775276834803655],[-73.97000312805176,40.77852663535664],[-73.9712905883789,40.78580561168767]]]});
print intersects = geo_intersects_line_with_polygon(lineString, polygon)
```

**Output**

|intersects|
|---|
|True|

The following example finds all roads in the NYC GeoJSON roads table that intersect with area of interest literal polygon.

```kusto
let area_of_interest = dynamic({"type":"Polygon","coordinates":[[[-73.95768642425537,40.80065354924362],[-73.9582872390747,40.80089719667298],[-73.95869493484497,40.80050736035672],[-73.9580512046814,40.80019873831593],[-73.95768642425537,40.80065354924362]]]});
NY_Manhattan_Roads
| project name = features.properties.Label, road = features.geometry
| where geo_intersects_line_with_polygon(road, area_of_interest)
| project name
```

**Output**

|name|
|---|
|Central Park W|
|Frederick Douglass Cir|
|W 110th St|
|West Dr|

The following example finds all counties in the USA that intersect with area of interest literal LineString.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA12QQUvDMBiG7/sVIacNakmbpUkmHkS8qZfhaZQQum9dRpeU9BujqP/dlKrIcnx58vC9bwdIbARrwsE4jxBhQPJA9qO3Z9csPyiOPdANfXEethidb2lGmxDi3nmLMNDNbncnea5lITTTWkilhMzWLJd6LQrOuZKKKS3qbOYqyZUodckEL8uZK6VQ6VWFkqL64/Rkmgzsx6e0TGGhKlYIJur6a3W/eN+ap3Dx6GBYfJI+hhM0SNLtkEocwOIlFcpT3kOcoPzt8fU5I830Z/yPtBDOgHFMlusxrUBSMA8yJONgutTfXB0eTR+6sQ1+ebvar3V1c8g3SxOfMGIBAAA=" target="_blank">Run the query</a>

```kusto
let area_of_interest = dynamic({"type":"LineString","coordinates":[[-73.97159099578857,40.794513338780895],[-73.96738529205322,40.792758888618756],[-73.96978855133057,40.789769718601505]]});
US_Counties
| project name = features.properties.NAME, county = features.geometry
| where geo_intersects_line_with_polygon(area_of_interest, county)
| project name
```

**Output**

|name|
|---|
|New York|

The following example will return a null result because the LineString is invalid.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA4WQTWuDQBBA7/0VsicFG/ZjZncmof+gh0KPQUIwiwibVXRzkNL/XoMxHlrocZjHY+YFn7LQRv+ZhjY22Vt2meL52tb5l0hT78VevD+3ohR11w2XNp6TH8X+eHx1ZseEirEEuXNE2mFVfReHlzB7+y5MTRf/kn4sq9/GRYnOkgUNGtG4u5qktGgQWIOxuiofGGly2rB0sFLETrG1TjNtlGVgAwTAK4bSGSsNzuCGSVRagiUFD0oxOUNGIZsn9c9ly/f9nCtl7RhvIeSN707z6IfR12k86XvuMd+il2uoovgBkcn4944BAAA=" target="_blank">Run the query</a>

```kusto
let lineString = dynamic({"type":"LineString","coordinates":[[-73.985195,40.788275]]});
let polygon = dynamic({"type":"Polygon","coordinates":[[[-73.95768642425537,40.80065354924362],[-73.9582872390747,40.80089719667298],[-73.95869493484497,40.80050736035672],[-73.9580512046814,40.80019873831593],[-73.95768642425537,40.80065354924362]]]});
print isnull(geo_intersects_2lines(lineString, polygon))
```

**Output**

|print_0|
|---|
|True|

The following example will return a null result because the polygon is invalid.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA21QTWvDMAy971cEnxLIij+qSOroP9hhsGMJpaQmGDw7xO4hjP33OcvWHTbdpPf09PS8zZV3wb7m2YWxOlbXJVze3FC/i7xMVhzE8x0VrRhinK8uXLJN4nA6PaLZMSpgyQxIBNju5Q55D8oYQ0iSGPp243VoCDRrCUbrjacRqFSnCKG783hVWhXktx4xlqGiTiqQ0PcfzdODL8an6Jcxhv9cv2zQH8tfu1P5JlcuhZv39WjjubR2TnbI6azXNFL9m0n7c6ZpPgG+oq1pLQEAAA==" target="_blank">Run the query</a>

```kusto
let lineString = dynamic({"type":"LineString","coordinates":[[-73.97159099578857,40.794513338780895],[-73.96738529205322,40.792758888618756],[-73.96978855133057,40.789769718601505]]});
let polygon = dynamic({"type":"Polygon","coordinates":[]});
print isnull(geo_intersects_2lines(lineString, polygon))
```

**Output**

|print_0|
|---|
|True|
