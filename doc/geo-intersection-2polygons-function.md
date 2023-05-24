---
title:  geo_intersection_2polygons()
description: Learn how to use the geo_intersection_2polygons() function to calculate the intersection of two polygons or multipolygons.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 03/09/2023
---
# geo_intersection_2polygons()

Calculates the intersection of two polygons or multipolygons.

## Syntax

`geo_intersection_2polygons(`*polygon1*`,`*polygon1*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *polygon1* | dynamic | &check; | Polygon or multipolygon in the [GeoJSON format](https://tools.ietf.org/html/rfc7946).|
| *polygon2* | dynamic | &check; | Polygon or multipolygon in the [GeoJSON format](https://tools.ietf.org/html/rfc7946).|

## Returns

Intersection in [GeoJSON Format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type. If Polygon or a MultiPolygon are invalid, the query will produce a null result.

> [!NOTE]
>
> * The geospatial coordinates are interpreted as represented by the [WGS-84](https://earth-info.nga.mil/index.php?dir=wgs84&action=wgs84) coordinate reference system.
> * The [geodetic datum](https://en.wikipedia.org/wiki/Geodetic_datum) used for measurements on Earth is a sphere. Polygon edges are [geodesics](https://en.wikipedia.org/wiki/Geodesic) on the sphere.
> * If input polygon edges are straight cartesian lines, consider using [geo_polygon_densify()](geo-polygon-densify-function.md) to convert planar edges to geodesics.

**Polygon definition and constraints**

dynamic({"type": "Polygon","coordinates": [LinearRingShell, LinearRingHole_1, ..., LinearRingHole_N ]})

dynamic({"type": "MultiPolygon","coordinates": [[LinearRingShell, LinearRingHole_1, ..., LinearRingHole_N ],..., [LinearRingShell, LinearRingHole_1, ..., LinearRingHole_M]]})

* LinearRingShell is required and defined as a `counterclockwise` ordered array of coordinates [[lng_1,lat_1],...,[lng_i,lat_i],...,[lng_j,lat_j],...,[lng_1,lat_1]]. There can be only one shell.
* LinearRingHole is optional and defined as a `clockwise` ordered array of coordinates [[lng_1,lat_1],...,[lng_i,lat_i],...,[lng_j,lat_j],...,[lng_1,lat_1]]. There can be any number of interior rings and holes.
* LinearRing vertices must be distinct with at least three coordinates. The first coordinate must be equal to the last. At least four entries are required.
* Coordinates [longitude, latitude] must be valid. Longitude must be a real number in the range [-180, +180] and latitude must be a real number in the range [-90, +90].
* LinearRingShell encloses at most half of the sphere. LinearRing divides the sphere into two regions. The smaller of the two regions will be chosen.
* LinearRing edge length must be less than 180 degrees. The shortest edge between the two vertices will be chosen.
* LinearRings must not cross and must not share edges. LinearRings may share vertices.
* Polygon contains its vertices.

> [!TIP]
>
> * Using literal Polygon or a MultiPolygon may result in better performance.

## Examples

The following example calculates intersection between two polygons. In this case, the result is a polygon.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA52STWrDMBBG9z2F0SoBN8yPRjNK6R26NyaExARDYoXEG1N696rEjr1qoQhpIT3mmzfo3PTFNZ2HU+qweC+OQ7e/tIfVp+uHa+O27uPx5kp3SOl2bLt939zdtqqqV+VNDAyRVTRQ9KWHjaqPZh6UiU3qcqIkiFHwGEaIjRFRDSL6J0UgDCEfGOjBsXr5AQ1lUQ0DqGEuITiFckTinBFtzvy1s7r+Wr+9nGd7+o89IbNQMCAYzQS8kIH3in7hj3mBicg0AMkGECIBLzACzT1SLqs6chjykDgaRZ6xP1Ifbtdb2/VF3s3t3hz6NnXZ8NSk3fJqR6P+fTX9gvI5kfU3hDmIFB0CAAA=" target="_blank">Run the query</a>

```kusto
let polygon1 = dynamic({"type":"Polygon","coordinates":[[[-73.9630937576294,40.77498840732385],[-73.963565826416,40.774383111780914],[-73.96205306053162,40.773745311181585],[-73.96160781383514,40.7743912365898],[-73.9630937576294,40.77498840732385]]]});
let polygon2 = dynamic({"type":"Polygon","coordinates":[[[-73.96213352680206,40.775045280447145],[-73.9631313085556,40.774578106920345],[-73.96207988262177,40.77416780398293],[-73.96213352680206,40.775045280447145]]]});
print intersection = geo_intersection_2polygons(polygon1, polygon2)
```

**Output**

|intersection|
|---|
|{"type": "Polygon",  "coordinates": [[[-73.962105776437156,40.774591360999679],[-73.962642403166868,40.774807020251778],[-73.9631313085556,40.774578106920352],[-73.962079882621765,40.774167803982927],[-73.962105776437156,40.774591360999679]]]}|

The following example calculates intersection between two polygons. In this case, the result is a point.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVEoyM+pTM/PM1SwVUipzEvMzUzWqFYqqSxIVbJSCoDIKekoJefnF6Vk5iWWpBYrWUVHRxvpmJjG6kQbQChDHRMTIAUWjI2t1bTmykGYbESaycZIZoEpYyAFFoSYXFCUmVeiAMSpRcWpySWZ+XlA89NT8+ORheKNoJYXa8D8pwN3jyYAACvP9/cAAAA=" target="_blank">Run the query</a>

```kusto
let polygon1 = dynamic({"type":"Polygon","coordinates":[[[2,45],[0,45],[1,44],[2,45]]]});
let polygon2 = dynamic({"type":"Polygon","coordinates":[[[3,44],[2,45],[2,43],[3,44]]]});
print intersection = geo_intersection_2polygons(polygon1, polygon2)
```

**Output**

|intersection|
|---|
|{"type": "Point","coordinates": [2,45]}|

The following two polygons intersection is a collection.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA3VQQQqDMBC89xUhJ4UQTKItWvqEQu9LENEggTQRTQ9S+vem0bYe2sMyuzPLMLtGeTQ4M/fOMnRC3Wybq26TO/bzoHCFL4uGCW6dGzttG68mXAEAJ3khCWQLMJLnASIp5SM97szXmf9yPt+M1//tQWwcI4gAkZShAWCUlS+R8n0MQLOsCCot4yKjotjHURziuFleAw6jth6FUuOkWq+dDTF75eotVfP1hil5v4l8zkqfcwvpPD4BAAA=" target="_blank">Run the query</a>

```kusto
let polygon1 = dynamic({"type":"Polygon","coordinates":[[[2,45],[0,45],[1,44],[2,45]]]});
let polygon2 = dynamic({"type":"MultiPolygon","coordinates":[[[[3,44],[2,45],[2,43],[3,44]]],[[[1.192,45.265],[1.005,44.943],[1.356,44.937],[1.192,45.265]]]]});
print intersection = geo_intersection_2polygons(polygon1, polygon2)
```

**Output**

|intersection|
|---|
|{"type": "GeometryCollection","geometries": [<br>{ "type": "Point", "coordinates": [2, 45]},<br>{ "type": "Polygon", "coordinates": [[[1.3227075526410679,45.003909145068739],[1.0404565374899824,45.004356403066552],[1.005,44.943],[1.356,44.937],[1.3227075526410679,45.003909145068739]]]}]}|

The following two polygons don't intersect.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVEoyM+pTM/PM1SwVUipzEvMzUzWqFYqqSxIVbJSCoDIKekoJefnF6Vk5iWWpBYrWUVHRxvpmJjG6kQbQChDHRMTIAUWjI2t1bTmykGYbESaycYQs4whJgONNIbwTKAmFxRl5pUoAHFqUXFqcklmfh7Q/PTU/HhkoXgjqOXFGjD/6cDdowkAMQjV5/cAAAA=" target="_blank">Run the query</a>

```kusto
let polygon1 = dynamic({"type":"Polygon","coordinates":[[[2,45],[0,45],[1,44],[2,45]]]});
let polygon2 = dynamic({"type":"Polygon","coordinates":[[[3,44],[3,45],[2,43],[3,44]]]});
print intersection = geo_intersection_2polygons(polygon1, polygon2)
```

**Output**

|intersection|
|---|
|{"type": "GeometryCollection", "geometries": []}|

The following example finds all counties in USA that intersect with area of interest polygon.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA4WQT0vEMBDF7/spYk8t1JLmT5Ou7EHEoyKIp1JK6c52K92kpFmkqN/dWbeyRQ+SSzL5zZt5rwdPagd1ZXdVZzw4GD3ZkO1k6kPXhO+BnwYI1sGT7afWmiAOGmvdtjO1hzFYF0VxrXiSZyzlXLJMU0azWNBEKUmFZJoKoVIhy3jmeIqHainljAmpdEqznFG+wBhVudYMZZWauTRTmvJcs5xfsH+mluVndLN6ea7u7NH4DsbVBxmcfYXGEzQI6HQHtT+i6wTrA7gTlDzePtzHpDn1TEukBXsA76ZfKjH5Tm7Ed2cNNiBXLUsVG87xjeFZNP6TeYSab3u84o+rp6oH0/p9uFT5GY8bRuRqQ+gXegS8Q7wBAAA=" target="_blank">Run the query</a>

```kusto
let area_of_interest = dynamic({"type":"Polygon","coordinates":[[[-73.96213352680206,40.775045280447145],[-73.9631313085556,40.774578106920345],[-73.96207988262177,40.77416780398293],[-73.96213352680206,40.775045280447145]]]});
US_Counties
| project name = features.properties.NAME, county = features.geometry
| project name, intersection = geo_intersection_2polygons(county, area_of_interest)
| where array_length(intersection.geometries) != 0
```

**Output**

|name|intersection|
|---|---|
|New York|{"type": "Polygon","coordinates": [[[-73.96213352680206, 40.775045280447145], [-73.9631313085556, 40.774578106920345], [-73.96207988262177,40.77416780398293],[-73.96213352680206, 40.775045280447145]]]}|

The following example will return a null result because one of the polygons is invalid.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA42QzUrFMBCF9z5FyaqFesnfTJIrvoP7UkpowyUYk5JGoYjvbmstiLhwNcycMx9zJrhSjS6WbMMw2/w8zCmstxSrx2pao33xY/1Oyjo7ciVPh0RaMqaUJx9tcQu5dl13r8TFSAOtpBdl0PTtMQJNFUfkAgTwXdSUokYqAJnh7LRpTpmRXEqhpf5ioOaAigsKSqnTpgRTWuOGAmEOmwRgAoHu5bT9vKPvP5qHu7CF9PHNBj/9J9++Mmcft6UlvoZQ31wattblxY3Fpzjwb8pS/8K2fz6zaT4BPdFP/2cBAAA=" target="_blank">Run the query</a>

```kusto
let central_park_polygon = dynamic({"type":"Polygon","coordinates":[[[-73.9495,40.7969],[-73.95807266235352,40.80068603561921],[-73.98201942443848,40.76825672305777],[-73.97317886352539,40.76455136505513],[-73.9495,40.7969]]]});
let invalid_polygon = dynamic({"type":"Polygon"});
print isnull(geo_intersection_2polygons(invalid_polygon, central_park_polygon))
```

**Output**

|print_0|
|---|
|1|
