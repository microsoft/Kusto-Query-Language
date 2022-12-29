---
title: geo_s2cell_to_polygon() - Azure Data Explorer
description: Learn how to use the geo_s2cell_to_polygon() function to calculate the polygon that represents the S2 Cell rectangular area.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 12/14/2022
---
# geo_s2cell_to_polygon()

Calculates the polygon that represents the S2 Cell rectangular area.

Read more about [S2 Cells](https://s2geometry.io/devguide/s2cell_hierarchy).

## Syntax

`geo_s2cell_to_polygon(`*s2cell*`)`

## Arguments

*s2cell*: S2 Cell token string value as it was calculated by [geo_point_to_s2cell()](geo-point-to-s2cell-function.md). The S2 Cell token maximum string length is 16 characters.

## Returns

Polygon in [GeoJSON Format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type. If the s2cell is invalid, the query will produce a null result.

> [!NOTE]
> S2 Cell edges are spherical geodesics.

## Examples

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print s2cellPolygon = geo_s2cell_to_polygon("89c259")
```

**Output**

|s2cellPolygon|
|---|
|{<br>"type": "Polygon",<br>"coordinates": [[[-74.030012249838478, 40.8012684339439], [-74.030012249838478, 40.7222262918358], [-73.935982114337421, 40.708880489804564], [-73.935982114337421, 40.787917134506841], [-74.030012249838478, 40.8012684339439]]]<br>}|

The following example assembles GeoJSON geometry collection of S2 Cell polygons.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(lng:real, lat:real)
[
    -73.956683, 40.807907,
    -73.916869, 40.818314,
    -73.989148, 40.743273,
]
| project s2_hash = geo_point_to_s2cell(lng, lat, 10)
| project s2_hash_polygon = geo_s2cell_to_polygon(s2_hash)
| summarize s2_hash_polygon_lst = make_list(s2_hash_polygon)
| project pack(
    "type", "Feature",
    "geometry", pack("type", "GeometryCollection", "geometries", s2_hash_polygon_lst),
    "properties", pack("name", "S2 Cell polygons collection"))
```

**Output**

|Column1|
|---|
|{<br>"type": "Feature",<br>"geometry": {"type": "GeometryCollection", "geometries": [<br>{"type": "Polygon", "coordinates": [[[-74.030012249838478, 40.880202851376716], [-74.030012249838478, 40.8012684339439], [-73.935982114337421, 40.787917134506841], [-73.935982114337421, 40.866846163445771], [-74.030012249838478, 40.880202851376716]]]},<br>{"type": "Polygon", "coordinates": [[[-73.935982114337421, 40.866846163445771], [-73.935982114337421, 40.787917134506841], [-73.841906340776248, 40.774477568182071], [-73.841906340776248, 40.853401155678846], [-73.935982114337421, 40.866846163445771]]]},<br>{"type": "Polygon", "coordinates": [[[-74.030012249838478, 40.8012684339439], [-74.030012249838478, 40.7222262918358], [-73.935982114337421, 40.708880489804564], [-73.935982114337421, 40.787917134506841], [-74.030012249838478, 40.8012684339439]]]}]<br>},<br> "properties": {"name": "S2 Cell polygons collection"}<br>}|

The following example returns a null result because of the invalid s2cell token input.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print s2cellPolygon = geo_s2cell_to_polygon("a")
```

**Output**

|s2cellPolygon|
|---|
||
