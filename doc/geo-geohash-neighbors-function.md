---
title:  geo_geohash_neighbors()
description: Learn how to use the geo_geohash_neighbors() function to calculate geohash neighbors.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 03/09/2023
---
# geo_geohash_neighbors()

Calculates Geohash neighbors.

Read more about [`geohash`](https://en.wikipedia.org/wiki/Geohash).  

## Syntax

`geo_geohash_neighbors(`*geohash*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *geohash* | string | &check; | A geohash value as it was calculated by [geo_point_to_geohash()](geo-point-to-geohash-function.md). The geohash string must be between 1 and 18 characters.|

## Returns

An array of Geohash neighbors. If the Geohash is invalid, the query produces a null result.

## Examples

The following example calculates Geohash neighbors.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUchLzUzPSMovKlawVUhPzY8H4ozE4ox4uLiGenFpXl6luiYAmib90DAAAAA=" target="_blank">Run the query</a>

```kusto
print neighbors = geo_geohash_neighbors('sunny')
```

**Output**

|neighbors|
|---|
|["sunnt","sunpj","sunnx","sunpn","sunnv","sunpp","sunnz","sunnw"]|

The following example calculates an array of input Geohash with its neighbors.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVFIT83PSCzOULBVUC8uzcurVLfmKijKzCtRSE7NySkGCicWFSVWxifn5yUnlmgUJCZnx4NFNKAaNXVARsRDefF5qZnpGUn5RcVweU0AMyi9o2YAAAA=" target="_blank">Run the query</a>

```kusto
let geohash = 'sunny';
print cells = array_concat(pack_array(geohash), geo_geohash_neighbors(geohash))
```

**Output**

|cells|
|---|
|["sunny","sunnt","sunpj","sunnx","sunpn","sunnv","sunpp","sunnz","sunnw"]|

The following example calculates Geohash polygons GeoJSON geometry collection.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA2WQwU5EIQxF9/MVhM3wkvELjCsT/QzSwcpjBiiBPiPGj7c8USdxR29vT2+JyMojrdBW9aCObcu5H+8PpYbMymGMTWSoFbp1lB2wKeCudlfMHFxOA2FnZTMGv56ptt/+cvhU6e0O3wvkl0llUtwL0qtpLMv88JRKF3SsCsXuKY/Vt2AmOztmZ4yRtqUENXzgCCn+BFe0MTTJOSG3YCFdGmXxncHbcYnRI4U+Kf2EwFuVpxZXQq5d1P+259l8pBgFGSgPdY4EbFJJEvkSLUsLVv7W/kAZ0g76yaeX5QvXq2jehQEAAA==" target="_blank">Run the query</a>

```kusto
let geohash = 'sunny';
print cells = array_concat(pack_array(geohash), geo_geohash_neighbors(geohash))
| mv-expand cells to typeof(string)
| project polygons = geo_geohash_to_polygon(cells)
| summarize arr = make_list(polygons)
| project geojson = bag_pack("type", "Feature","geometry", bag_pack("type", "GeometryCollection", "geometries", arr), "properties", bag_pack("name", "polygons"))
```

**Output**

|geojson|
|---|
|{"type": "Feature","geometry": {"type": "GeometryCollection","geometries": [<br>  {"type":"Polygon","coordinates":[[[42.451171875,23.6865234375],[42.4951171875,23.6865234375],[42.4951171875,23.73046875],[42.451171875,23.73046875],[42.451171875,23.6865234375]]]},<br>  {"type":"Polygon","coordinates":[[[42.4072265625,23.642578125],[42.451171875,23.642578125],[42.451171875,23.6865234375],[42.4072265625,23.6865234375],[42.4072265625,23.642578125]]]},<br>  {"type":"Polygon","coordinates":[[[42.4072265625,23.73046875],[42.451171875,23.73046875],[42.451171875,23.7744140625],[42.4072265625,23.7744140625],[42.4072265625,23.73046875]]]},<br>  {"type":"Polygon","coordinates":[[[42.4951171875,23.642578125],[42.5390625,23.642578125],[42.5390625,23.6865234375],[42.4951171875,23.6865234375],[42.4951171875,23.642578125]]]},<br>  {"type":"Polygon","coordinates":[[[42.451171875,23.73046875],[42.4951171875,23.73046875],[42.4951171875,23.7744140625],[42.451171875,23.7744140625],[42.451171875,23.73046875]]]},<br>  {"type":"Polygon","coordinates":[[[42.4072265625,23.6865234375],[42.451171875,23.6865234375],[42.451171875,23.73046875],[42.4072265625,23.73046875],[42.4072265625,23.6865234375]]]},<br>  {"type":"Polygon","coordinates":[[[42.4951171875,23.73046875],[42.5390625,23.73046875],[42.5390625,23.7744140625],[42.4951171875,23.7744140625],[42.4951171875,23.73046875]]]},<br>  {"type":"Polygon","coordinates":[[[42.4951171875,23.6865234375],[42.5390625,23.6865234375],[42.5390625,23.73046875],[42.4951171875,23.73046875],[42.4951171875,23.6865234375]]]},<br>  {"type":"Polygon","coordinates":[[[42.451171875,23.642578125],[42.4951171875,23.642578125],[42.4951171875,23.6865234375],[42.451171875,23.6865234375],[42.451171875,23.642578125]]]}]},<br>  "properties": {"name": "polygons"}}|

The following example calculates polygon unions that represent Geohash and its neighbors.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA22PQQ4CIQxF956C3TCJrlwaz0IQK+AwLYGOEePhhZFJXLhoF7/977cBWLijgRDEWQx5QSzDaReTRxZNzVXWKemiDKHRLKM2k1oV+fWNe2GBVC2ns1MI3roLpbyNx91bzI8DPKPGa2cyCS4R6CYz1yjbdmKiOxgWkUKxhC34l8uk+kSujGbJyzzr5F/QTqz7s55ABZ/rlR3yB9y5C3rCDZn7R7WPH3yyyNkSAQAA" target="_blank">Run the query</a>

```kusto
let h3cell = 'sunny';
print cells = array_concat(pack_array(h3cell), geo_geohash_neighbors(h3cell))
| mv-expand cells to typeof(string)
| project polygons = geo_geohash_to_polygon(cells)
| summarize arr = make_list(polygons)
| project polygon = geo_union_polygons_array(arr)
```

**Output**

|polygon|
|---|
|{"type":"Polygon","coordinates":[[[42.4072265625,23.642578125],[42.451171875,23.642578125],[42.4951171875,23.642578125],[42.5390625,23.642578125],[42.5390625,23.686523437500004],[42.5390625,23.730468750000004],[42.5390625,23.7744140625],[42.4951171875,23.7744140625],[42.451171875,23.7744140625],[42.407226562499993,23.7744140625],[42.4072265625,23.73046875],[42.4072265625,23.6865234375],[42.4072265625,23.642578125]]]}|

The following example returns true because of the invalid Geohash token input.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUcjMK0vMyUxRsFXILM4rzcnRSE/NjwfijMTijPi81Mz0jKT8omIN9UR1TU0AbJVClTIAAAA=" target="_blank">Run the query</a>

```kusto
print invalid = isnull(geo_geohash_neighbors('a'))
```

**Output**

|invalid|
|---|
|1|
