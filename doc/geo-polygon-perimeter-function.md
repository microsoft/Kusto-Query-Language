---
title: geo_polygon_perimeter() - Azure Data Explorer
description: Learn how to use the geo_polygon_perimeter() function to calculate the length of the boundary of a polygon or a multipolygon on Earth.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 03/09/2023
---
# geo_polygon_perimeter()

Calculates the length of the boundary of a polygon or a multipolygon on Earth.

## Syntax

`geo_polygon_perimeter(`*polygon*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *polygon* | dynamic | &check; | Polygon or multipolygon in the [GeoJSON format](https://tools.ietf.org/html/rfc7946).|

## Returns

The length of the boundary of polygon or a multipolygon, in meters, on Earth. If polygon or multipolygon are invalid, the query will produce a null result.

> [!NOTE]
>
> * The geospatial coordinates are interpreted as represented by the [WGS-84](https://earth-info.nga.mil/index.php?dir=wgs84&action=wgs84) coordinate reference system.
> * The [geodetic datum](https://en.wikipedia.org/wiki/Geodetic_datum) used for measurements on Earth is a sphere. Polygon edges are [geodesics](https://en.wikipedia.org/wiki/Geodesic) on the sphere.
> * If input polygon edges are straight cartesian lines, consider using [geo_polygon_densify()](geo-polygon-densify-function.md) to convert planar edges to geodesics.
> * If input is a multipolygon and contains more than one polygon, the result will be the length of the boundary of polygons union.

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

The following example calculates the NYC Central Park perimeter, in meters.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA02QzWrDMBCE730Ko1MCbpC02h+l9B16N8YYRwRTRxKqLqb03ZvUuOQ0MPMx7OwSajOFWMu4DHksn817c1njeJunw7eqaw7qrD7Ssl5TVK2aUiqXOY41fKlz13WvDCfvPLZOn9iT79vNQtFsiSwgoH2EojUJaUAy3podE6uNd9Y5ECd/HSQWiS1oZOYdYzAsQvcqBL9hDtEAoX7Ijj3f0fc/x7eXXOZYmxzKfAs1lPu2a0hD3uYM//7h+QHHX4p2a4cSAQAA" target="_blank">Run the query</a>

```kusto
let central_park = dynamic({"type":"Polygon","coordinates":[[[-73.9495,40.7969],[-73.95807266235352,40.80068603561921],[-73.98201942443848,40.76825672305777],[-73.97317886352539,40.76455136505513],[-73.9495,40.7969]]]});
print perimeter = geo_polygon_perimeter(central_park)
```

**Output**

|perimeter|
|---|
|9930.30149604938|

The following example performs union of polygons in multipolygon and calculates perimeter of the unified polygon.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA4WRTWrDMBBG9z2F8SoBN4xGmr+UHqHQfQkhJCIYHDs4ziKU3r2TuHa7qzaC0dPojb4mD8W5a27Hrr0Ur8Xh1u5O9X7xWQ63cy7X5du1Ger3ESirct91/aFud0O+lOsPX88SV5aMqgQrMbZNNZZIQZAZI0XC+6ECsDJE4mAYJkwRgiVMKWrSRw9WJBaMQCIyYRKDqLK3omgjlohCZIL7NmF/PTZenPTQPYK/gCLxAQiZBYVETKQ0ywQQQnVNwpFzKFkgSixsM2cM4ObMDIHHfuAzqd8N9ivNwe1UTQ1pnI0YzAFijjx/wb92vr6WL0/nvm49q9zXpzzk3sM65m77k912ri+mNJff9ugUsNsBAAA=" target="_blank">Run the query</a>

```kusto
let polygons = dynamic({"type":"MultiPolygon","coordinates":[[[[-73.9495,40.7969],[-73.95807266235352,40.80068603561921],[-73.98201942443848,40.76825672305777],[-73.97317886352539,40.76455136505513],[-73.9495,40.7969]]],[[[-73.94262313842773,40.775991804565585],[-73.98107528686523,40.791849155467695],[-73.99600982666016,40.77092185281977],[-73.96150588989258,40.75609977566361],[-73.94262313842773,40.775991804565585]]]]});
print perimeter = geo_polygon_perimeter(polygons)
```

**Output**

|perimeter|
|---|
|15943.5384578745|

The following example returns True because of the invalid polygon.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA02KQQrDIBBFryKzUnBhtoXeoXsRkTiEATMjxgYk5O61dNPV5733ayPuio5IfKZCWT0n8LsUvaHEKmVswrFiox07Np0Hp51WfUEfFeGh4PX7gIVVpGXi1PGYwXvvrAvWL84u//u1IdzGmA8jsUSgfQAAAA==" target="_blank">Run the query</a>

```kusto
print is_invalid = isnull(geo_polygon_perimeter(dynamic({"type": "Polygon","coordinates": [[[0,0],[10,10],[10,10],[0,0]]]})))
```

**Output**

|is_invalid|
|---|
|True|
