---
title: geo_geohash_neighbors() - Azure Data Explorer
description: Learn how to use the geo_geohash_neighbors() function to calculate geohash neighbors.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 12/14/2022
---
# geo_geohash_neighbors()

Calculates Geohash neighbors.

Read more about [`geohash`](https://en.wikipedia.org/wiki/Geohash).  

## Syntax

`geo_geohash_neighbors(`*geohash*`)`

## Arguments

*geohash*: Geohash string value as it was calculated by [geo_point_to_geohash()](geo-point-to-geohash-function.md). The geohash string must be between 1 and 18 characters.

## Returns

An array of Geohash neighbors. If the Geohash is invalid, the query will produce a null result.

## Examples

The following example calculates Geohash neighbors.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print neighbors = geo_geohash_neighbors('sunny')
```

|neighbors|
|---|
|["sunnt","sunpj","sunnx","sunpn","sunnv","sunpp","sunnz","sunnw"]|

The following example calculates an array of input Geohash with its neighbors.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let geohash = 'sunny';
print cells = array_concat(pack_array(geohash), geo_geohash_neighbors(geohash))
```

|cells|
|---|
|["sunny","sunnt","sunpj","sunnx","sunpn","sunnv","sunpp","sunnz","sunnw"]|

The following example calculates Geohash polygons GeoJSON geometry collection.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let geohash = 'sunny';
print cells = array_concat(pack_array(geohash), geo_geohash_neighbors(geohash))
| mv-expand cells to typeof(string)
| project polygons = geo_geohash_to_polygon(cells)
| summarize arr = make_list(polygons)
| project geojson = pack("type", "Feature","geometry", pack("type", "GeometryCollection", "geometries", arr), "properties", pack("name", "polygons"))
```

|geojson|
|---|
|{"type": "Feature","geometry": {"type": "GeometryCollection","geometries": [<br>  {"type":"Polygon","coordinates":[[[42.451171875,23.6865234375],[42.4951171875,23.6865234375],[42.4951171875,23.73046875],[42.451171875,23.73046875],[42.451171875,23.6865234375]]]},<br>  {"type":"Polygon","coordinates":[[[42.4072265625,23.642578125],[42.451171875,23.642578125],[42.451171875,23.6865234375],[42.4072265625,23.6865234375],[42.4072265625,23.642578125]]]},<br>  {"type":"Polygon","coordinates":[[[42.4072265625,23.73046875],[42.451171875,23.73046875],[42.451171875,23.7744140625],[42.4072265625,23.7744140625],[42.4072265625,23.73046875]]]},<br>  {"type":"Polygon","coordinates":[[[42.4951171875,23.642578125],[42.5390625,23.642578125],[42.5390625,23.6865234375],[42.4951171875,23.6865234375],[42.4951171875,23.642578125]]]},<br>  {"type":"Polygon","coordinates":[[[42.451171875,23.73046875],[42.4951171875,23.73046875],[42.4951171875,23.7744140625],[42.451171875,23.7744140625],[42.451171875,23.73046875]]]},<br>  {"type":"Polygon","coordinates":[[[42.4072265625,23.6865234375],[42.451171875,23.6865234375],[42.451171875,23.73046875],[42.4072265625,23.73046875],[42.4072265625,23.6865234375]]]},<br>  {"type":"Polygon","coordinates":[[[42.4951171875,23.73046875],[42.5390625,23.73046875],[42.5390625,23.7744140625],[42.4951171875,23.7744140625],[42.4951171875,23.73046875]]]},<br>  {"type":"Polygon","coordinates":[[[42.4951171875,23.6865234375],[42.5390625,23.6865234375],[42.5390625,23.73046875],[42.4951171875,23.73046875],[42.4951171875,23.6865234375]]]},<br>  {"type":"Polygon","coordinates":[[[42.451171875,23.642578125],[42.4951171875,23.642578125],[42.4951171875,23.6865234375],[42.451171875,23.6865234375],[42.451171875,23.642578125]]]}]},<br>  "properties": {"name": "polygons"}}|

The following example calculates polygon unions that represent Geohash and its neighbors.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let h3cell = 'sunny';
print cells = array_concat(pack_array(h3cell), geo_geohash_neighbors(h3cell))
| mv-expand cells to typeof(string)
| project polygons = geo_geohash_to_polygon(cells)
| summarize arr = make_list(polygons)
| project polygon = geo_union_polygons_array(arr)
```

|polygon|
|---|
|{"type":"Polygon","coordinates":[[[42.4072265625,23.642578125],[42.451171875,23.642578125],[42.4951171875,23.642578125],[42.5390625,23.642578125],[42.5390625,23.686523437500004],[42.5390625,23.730468750000004],[42.5390625,23.7744140625],[42.4951171875,23.7744140625],[42.451171875,23.7744140625],[42.407226562499993,23.7744140625],[42.4072265625,23.73046875],[42.4072265625,23.6865234375],[42.4072265625,23.642578125]]]}|

The following example returns true because of the invalid Geohash token input.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print invalid = isnull(geo_geohash_neighbors('a'))
```

|invalid|
|---|
|1|
