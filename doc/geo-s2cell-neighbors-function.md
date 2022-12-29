---
title: geo_s2cell_neighbors() - Azure Data Explorer
description: Learn how to use the geo_s2cell_neighbors() function to calculate S2 cell neighbors.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 12/14/2022
---
# geo_s2cell_neighbors()

Calculates S2 cell neighbors.

Read more about [S2 cell hierarchy](https://s2geometry.io/devguide/s2cell_hierarchy).

## Syntax

`geo_s2cell_neighbors(`*s2cell*`)`

## Arguments

*s2cell*: S2 cell token string value as it was calculated by [geo_point_to_s2cell()](geo-point-to-s2cell-function.md). The S2 cell token maximum string length is 16 characters.

## Returns

An array of S2 cell neighbors. If the S2 Cell is invalid, the query will produce a null result.

> [!NOTE]
> S2 Cell edges are spherical geodesics.

## Examples

The following example calculates S2 cell neighbors.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print neighbors = geo_s2cell_neighbors('89c259')
```

**Output**

|neighbors|
|---|
|["89c25d","89c2f9","89c251","89c257","89c25f","89c25b","89c2f7","89c2f5"]|

The following example calculates an array of input S2 cell with its neighbors.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let s2cell = '89c259';
print cells = array_concat(pack_array(s2cell), geo_s2cell_neighbors(s2cell))
```

**Output**

|cells|
|---|
|["89c259","89c25d","89c2f9","89c251","89c257","89c25f","89c25b","89c2f7","89c2f5"]|

The following example calculates S2 cells polygons GeoJSON geometry collection.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let s2cell = '89c259';
print cells = array_concat(pack_array(s2cell), geo_s2cell_neighbors(s2cell))
| mv-expand cells to typeof(string)
| project polygons = geo_s2cell_to_polygon(cells)
| summarize arr = make_list(polygons)
| project geojson = pack("type", "Feature","geometry", pack("type", "GeometryCollection", "geometries", arr), "properties", pack("name", "polygons"))
```

**Output**

|geojson|
|---|
|{"type": "Feature","geometry": {"type": "GeometryCollection","geometries": [<br>  {"type": "Polygon","coordinates": [[[  -74.030012249838478,  40.8012684339439],[  -74.030012249838478,  40.7222262918358],[  -73.935982114337421,  40.708880489804564],[  -73.935982114337421,  40.787917134506841],[  -74.030012249838478,  40.8012684339439]]]},<br>  {"type": "Polygon","coordinates": [[[  -73.935982114337421,  40.708880489804564],[  -73.935982114337421,  40.629736433321796],[  -73.841906340776248,  40.616308079144915],[  -73.841906340776248,  40.695446474556284],[  -73.935982114337421,  40.708880489804564]]]},<br>  {"type": "Polygon","coordinates": [[[  -74.1239959854733,  40.893471289549765],[  -74.1239959854733,  40.814531536204242],[  -74.030012249838478,  40.8012684339439],[  -74.030012249838478,  40.880202851376716],[  -74.1239959854733,  40.893471289549765]]]},<br>  {"type": "Polygon","coordinates": [[[  -74.1239959854733,  40.735483949993387],[  -74.1239959854733,  40.656328734184143],[  -74.030012249838478,  40.643076628676461],[  -74.030012249838478,  40.7222262918358],[  -74.1239959854733,  40.735483949993387]]]},<br>  {"type": "Polygon","coordinates": [[[  -74.1239959854733,  40.814531536204242],[  -74.1239959854733,  40.735483949993387],[  -74.030012249838478,  40.7222262918358],[  -74.030012249838478,  40.8012684339439],[  -74.1239959854733,  40.814531536204242]]]},<br>  {"type": "Polygon","coordinates": [[[  -73.935982114337421,  40.787917134506841],[  -73.935982114337421,  40.708880489804564],[  -73.841906340776248,  40.695446474556284],[  -73.841906340776248,  40.774477568182071],[  -73.935982114337421,  40.787917134506841]]]},<br>  {"type": "Polygon","coordinates": [[[  -74.030012249838478,  40.7222262918358],[  -74.030012249838478,  40.643076628676461],[  -73.935982114337421,  40.629736433321796],[  -73.935982114337421,  40.708880489804564],[  -74.030012249838478,  40.7222262918358]]]},<br>  {"type": "Polygon","coordinates": [[[  -74.030012249838478,  40.880202851376716],[  -74.030012249838478,  40.8012684339439],[  -73.935982114337421,  40.787917134506841],[  -73.935982114337421,  40.866846163445771],[  -74.030012249838478,  40.880202851376716]]]},<br>  {"type": "Polygon","coordinates": [[[  -73.935982114337421,  40.866846163445771],[  -73.935982114337421,  40.787917134506841],[  -73.841906340776248,  40.774477568182071],[  -73.841906340776248,  40.853401155678846],[  -73.935982114337421,  40.866846163445771]]]}]},<br>  "properties": {"name": "polygons"}}|

The following example calculates polygon unions that represent S2 cell and its neighbors.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let s2cell = '89c259';
print cells = array_concat(pack_array(s2cell), geo_s2cell_neighbors(s2cell))
| mv-expand cells to typeof(string)
| project polygons = geo_s2cell_to_polygon(cells)
| summarize arr = make_list(polygons)
| project polygon = geo_union_polygons_array(arr)
```

**Output**

|polygon|
|---|
|{"type": "Polygon","coordinates": [[[-73.841906340776248,40.695446474556284],[-73.841906340776248,40.774477568182071],[-73.841906340776248,40.853401155678846],[-73.935982114337421,40.866846163445771],[-74.030012249838478,40.880202851376716],[-74.1239959854733,40.893471289549758],[-74.1239959854733,40.814531536204242],[-74.1239959854733,40.735483949993387],[-74.1239959854733,40.656328734184143],[-74.030012249838478,40.643076628676461],[-73.935982114337421,40.629736433321796],[-73.841906340776248,40.616308079144915],[-73.841906340776248,40.695446474556284]]]}|

The following example returns true because of the invalid S2 Cell token input.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print invalid = isnull(geo_s2cell_neighbors('a'))
```

**Output**

|invalid|
|---|
|1|
