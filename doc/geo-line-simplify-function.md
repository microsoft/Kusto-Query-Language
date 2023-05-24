---
title:  geo_line_simplify()
description: Learn how to use the geo_line_simplify() function to simplify a line string or a multiline string.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 03/09/2023
---
# geo_line_simplify()

Simplifies a line or a multiline by replacing nearly straight chains of short edges with a single long edge on Earth.

## Syntax

`geo_line_simplify(`*lineString*`,` *tolerance*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *lineString* | dynamic | &check; | A LineString or MultiLineString in the [GeoJSON format](https://tools.ietf.org/html/rfc7946).|
| *tolerance* | int, long, or real | | Defines minimum distance in meters between any two vertices. Supported values are in the range [0, ~7,800,000 meters]. If unspecified, the default value `10` is used.|

## Returns

Simplified line or a multiline in the [GeoJSON format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type, with no two vertices with distance less than tolerance. If either the line or tolerance is invalid, the query will produce a null result.

> [!NOTE]
>
> * The geospatial coordinates are interpreted as represented by the [WGS-84](https://earth-info.nga.mil/index.php?dir=wgs84&action=wgs84) coordinate reference system.
> * The [geodetic datum](https://en.wikipedia.org/wiki/Geodetic_datum) used to measure distance on Earth is a sphere. Line edges are [geodesics](https://en.wikipedia.org/wiki/Geodesic) on the sphere.
> * If input line edges are straight cartesian lines, consider using [geo_line_densify()](geo-line-densify-function.md) in order to convert planar edges to geodesics.
> * If input is a multiline and contains more than one line, the result will be simplification of lines union.
> * High tolerance may cause small line to disappear.

**LineString definition and constraints**

dynamic({"type": "LineString","coordinates": [[lng_1,lat_1], [lng_2,lat_2], ..., [lng_N,lat_N]]})

dynamic({"type": "MultiLineString","coordinates": [[line_1, line_2, ..., line_N]]})

* LineString coordinates array must contain at least two entries.
* Coordinates [longitude, latitude] must be valid where longitude is a real number in the range [-180, +180] and latitude is a real number in the range [-90, +90].
* Edge length must be less than 180 degrees. The shortest edge between the two vertices will be chosen.

## Examples

The following example simplifies the line by removing vertices that are within a 10-meter distance from each other.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA02Rz2rDMAyH73uKkFMLWZEtWX869ga77VhCKY1XDGkS2lzC2LvPYVlT3SQ+9P0st3Es2tTF4r1opu50TefNdzlOQyz35Ueef4631F3Kqjz3/a1J3WmM93J/OLwK7kwA0bEpBwatCHaiBozgwQWvnurqwRmDOBZP9MepiUkwD+p4xYidaBBDXKhcKMF5Qr9SebeSA6e8SFWMssAFdCvFM4dZwguUg5LlgBj4KRkHn4sR8cF5JshR4dkpXplDMPC2YMGLOaKQIz9hhoDKwPz/TkKarSKKXNc/27eXIV90LO7pOrTpK8Umn/4S++P8C8dlOm3mriocbH8B+xvcNaEBAAA=" target="_blank">Run the query</a>

```kusto
let line = dynamic({"type":"LineString","coordinates":[[-73.97033169865608,40.789063020152824],[-73.97039607167244,40.78897975920816],[-73.9704617857933,40.78888837512432],[-73.97052884101868,40.7887949601531],[-73.9706052839756,40.788698498903564],[-73.97065222263336,40.78862640672032],[-73.97072866559029,40.78852791445617],[-73.97079303860664,40.788434498977836]]});
print simplified = geo_line_simplify(line, 10)
```

**Output**

|simplified|
|---|
|{"type": "LineString", "coordinates": [[-73.97033169865608, 40.789063020152824], [-73.97079303860664, 40.788434498977836]]}|

The following example simplifies lines and combines results into GeoJSON geometry collection.

```kusto
NY_Manhattan_Roads
| project road = features.geometry
| project road_simplified = geo_line_simplify(road, 100)
| summarize roads_lst = make_list(road_simplified)
| project geojson = bag_pack("type", "Feature","geometry", bag_pack("type", "GeometryCollection", "geometries", roads_lst), "properties", bag_pack("name", "roads"))
```

**Output**

|geojson|
|---|
|{"type": "Feature", "geometry": {"type": "GeometryCollection", "geometries": [ ... ]}, "properties": {"name": "roads"}}|

The following example simplifies lines and unifies result

```kusto
NY_Manhattan_Roads
| project road = features.geometry
| project road_simplified = geo_line_simplify(road, 100)
| summarize roads_lst = make_list(road_simplified)
| project roads = geo_union_lines_array(roads_lst)
```

**Output**

|roads|
|---|
|{"type": "MultiLineString", "coordinates": [ ... ]}|

The following example returns True because of the invalid line.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUcgsjs/MK0vMyUyJz8nMS1WwBYrklebkaKSn5oNF4oszcwtyMtMqNVIq8xJzM5M1qpVKKgtSlayUfIDSwSVAY9KVdJSS8/OLUjLzEktSi5WsoqMNdRQMY2NrNTU1Af5nM/VoAAAA" target="_blank">Run the query</a>

```kusto
print is_invalid_line = isnull(geo_line_simplify(dynamic({"type":"LineString","coordinates":[[1, 1]]})))
```

**Output**

|is_invalid_line|
|---|
|True|

The following example returns True because of the invalid tolerance.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAx3MQQrDIBBA0avIrBSmC7MM9AbddSkiEm0YmIwSbUBK717p9n349STpiloguSJTCkyS1X2KvJn1nstfQqOjMr2GTkPiQZv+QB81wwqPmZ99bnZA2Eo5E0nsucHqnEVlPboFF++/BtXNGvMDQCCfRnIAAAA=" target="_blank">Run the query</a>

```kusto
print is_invalid_line = isnull(geo_line_simplify(dynamic({"type":"LineString","coordinates":[[1, 1],[2,2]]}), -1))
```

**Output**

|is_invalid_line|
|---|
|True|

The following example returns True because high tolerance causes small line to disappear.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAx2MwQrDIBBEf0X2pCCl6THQP8gtxyAi0YaFzSrRFiTk37vtwAzDG5hyIDeF1SN/AmH0hJzUUwi/ifSW8p/4inshfHUdO4cdV31C6yXBCJPMc5ObDSysOR8RObRUYVyW4TZYJeGs1IcVO3cZQfefjPkC36xb1X4AAAA=" target="_blank">Run the query</a>

```kusto
print is_invalid_line = isnull(geo_line_simplify(dynamic({"type":"LineString","coordinates":[[1.1, 1.1],[1.2,1.2]]}), 100000))
```

**Output**

|is_invalid_line|
|---|
|True|
