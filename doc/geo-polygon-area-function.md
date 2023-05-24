---
title:  geo_polygon_area()
description: Learn how to use the geo_polygon_area() function to calculate the area of a polygon or a multipolygon on Earth.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 03/09/2023
---
# geo_polygon_area()

Calculates the area of a polygon or a multipolygon on Earth.

## Syntax

`geo_polygon_area(`*polygon*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *polygon* | dynamic | &check; | Polygon or multipolygon in the [GeoJSON format](https://tools.ietf.org/html/rfc7946).|

## Returns

The area of a polygon or a multipolygon, in square meters, on Earth. If the polygon or the multipolygon is invalid, the query will produce a null result.

> [!NOTE]
>
> * The geospatial coordinates are interpreted as represented by the [WGS-84](https://earth-info.nga.mil/index.php?dir=wgs84&action=wgs84) coordinate reference system.
> * The [geodetic datum](https://en.wikipedia.org/wiki/Geodetic_datum) used for measurements on Earth is a sphere. Polygon edges are [geodesics](https://en.wikipedia.org/wiki/Geodesic) on the sphere.
> * If input polygon edges are straight cartesian lines, consider using [geo_polygon_densify()](geo-polygon-densify-function.md) to convert planar edges to geodesics.
> * If input is a multipolygon and contains more than one polygon, the result will be the area of polygons union.

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

The following example calculates NYC Central Park area.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA02Py2rDMBBF9/0Ko1UCbpA0modS+g/dG2OEI4KpKxlVG1P6741rDFkN3Dlc7pljbcaYagnzsITy2bw3tzWFr2k8/ai6LlFd1Uee13tOqlVjzuU2pVDjt7p2XffKcPHOY+v0hT35vt0jFM2WyAIC2u0pWpOQBiTjrTkwsdp4Z50DcfLfQWKR2IJGZj4wBsMi9KhC8DvmEA0Q6u0c2POOvv89v70sZUq1CSWGh9Y95mHZTYYtOj1rn/8Au8DFaggBAAA=" target="_blank">Run the query</a>

```kusto
let central_park = dynamic({"type":"Polygon","coordinates":[[[-73.9495,40.7969],[-73.95807266235352,40.80068603561921],[-73.98201942443848,40.76825672305777],[-73.97317886352539,40.76455136505513],[-73.9495,40.7969]]]});
print area = geo_polygon_area(central_park)
```

**Output**

|area|
|---|
|3475207.28346606|

The following example performs union of polygons in multipolygon and calculates area on the unified polygon.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA4WRzWrDMAyA73uKklMLXZFl669jjzDYfZQS2lACWRLS9BDG3n1qvbS7TReD9Fn+ZDXVuOi7Zjp17XnxujhObflZH5ZfxTj1VbEt3i7NWL9noFgXh64bjnVbjtW52H54PEvcWDJaJ9iIse3WOUUKgswYKRJeiwrAyhCJg2GYMUUIljClqElvPViRWDACiciMSQyiyt6KomUsEYXIBNdjxv567Dw566F7BH8BReINEDILComYSOkuE0AI1TUJM+dQskCUWNjunDGAmzMzBM79wGdSvxvsIc3B7VRNDSnPRgzmADFHvn/Bv3Ye36uXp36o28eu9pe27tp9OVSlr+1Udfvfyi21nLHVD9mabgXgAQAA" target="_blank">Run the query</a>

```kusto
let polygons = dynamic({"type":"MultiPolygon","coordinates":[[[[-73.9495,40.7969],[-73.95807266235352,40.80068603561921],[-73.98201942443848,40.76825672305777],[-73.97317886352539,40.76455136505513],[-73.9495,40.7969]]],[[[-73.94262313842773,40.775991804565585],[-73.98107528686523,40.791849155467695],[-73.99600982666016,40.77092185281977],[-73.96150588989258,40.75609977566361],[-73.94262313842773,40.775991804565585]]]]});
print polygons_union_area = geo_polygon_area(polygons)
```

**Output**

|polygons_union_area|
|---|
|10889971.5343487|

The following example calculates top 5 biggest US states by area.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsNjg8uSSxJLeaqUSgoys9KTS5RyEvMTVWwVUhLTSwpLUot1gOKF6QWlWQCmX6Ovq46CgX5OZXp+XlANemp+fFQXnxKal5xZlqlBlwfUDI3taSoUhPNbB2FxKLURDTdICENKAekoSS/QMFUIakSojYltTgZAArmjpSrAAAA" target="_blank">Run the query</a>

```kusto
US_States
| project name = features.properties.NAME, polygon = geo_polygon_densify(features.geometry)
| project name, area = geo_polygon_area(polygon)
| top 5 by area desc
```

**Output**

|name|area|
|---|---|
|Alaska|1550934810070.61|
|Texas|693231378868.483|
|California|410339536449.521|
|Montana|379583933973.436|
|New Mexico|314979912310.579|

The following example returns True because of the invalid polygon.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUcgszivNydFIT82PL8jPqUzPz4tPLEpN1EipzEvMzUzWqFYqqSxIVbJSUAqASCvpKCXn5xelZOYllqQWAyWio6MNdAxidaINDXQMkWmQaGxsraamJgCVD2IfawAAAA==" target="_blank">Run the query</a>

```kusto
print isnull(geo_polygon_area(dynamic({"type": "Polygon","coordinates": [[[0,0],[10,10],[10,10],[0,0]]]})))
```

**Output**

|print_0|
|---|
|True|
