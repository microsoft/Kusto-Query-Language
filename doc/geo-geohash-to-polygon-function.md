---
title: geo_geohash_to_polygon() - Azure Data Explorer
description: Learn how to use the geo_geohash_to_polygon() function to calculate the polygon that represents the geohash rectangular area. 
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 03/09/2023
---
# geo_geohash_to_polygon()

Calculates the polygon that represents the geohash rectangular area.

Read more about [geohash](https://en.wikipedia.org/wiki/Geohash).  

## Syntax

`geo_geohash_to_polygon(`*geohash*`)`
## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *geohash* | string | &check; | A geohash value as it was calculated by [geo_point_to_geohash()](geo-point-to-geohash-function.md). The geohash string must be between 1 and 18 characters.|

## Returns

Polygon in [GeoJSON Format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type. If the geohash is invalid, the query will produce a null result.

> [!NOTE]
> Geohash edges are straight lines and aren't geodesics. If the geohash polygon is part of some other calculation, consider densifying it with [geo_polygon_densify()](geo-polygon-densify-function.md).

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUXBPzc9ILM4IyM+pTM/PU7BVSE/Nj0+HCMaX5McXQCQ0lFKKTItKlTStAWzxQVs3AAAA" target="_blank">Run the query</a>

```kusto
print GeohashPolygon = geo_geohash_to_polygon("dr5ru");
```

**Output**

|GeohashPolygon|
|---|
|{<br>"type": "Polygon",<br>"coordinates": [<br>[[-74.00390625, 40.7373046875], [-73.9599609375, 40.7373046875], [-73.9599609375, 40.78125], [-74.00390625, 40.78125], [-74.00390625, 40.7373046875]]]<br>}|

The following example assembles GeoJSON geometry collection of geohash polygons.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA21RPW+DMBTc+RUWE0huEj4CplKnSo3UoR06VpXl0CdCYzCyXwaq/vgaMASltQc/3707n+3tlhxAnYQ5Devz2+sLKZWUUGKtWu9ToJ1HCYFsq3sNQlIiBY5V6L17xI67PNkU+T6OYkrS3SZnRbZj9EpFGcuKkWIRS6J0RbEiStmkSpM4T6j34f2QTqsvez6pXK6HoeKdqlvkqLiDh0RjGEr24V+V7Zd9pVqnnlFUMxE4aNCaS9MIXX/DrZpLg9ahEWfgsjYY3PDrg4+i4p0oz8F4Px/7DnxK/CcQeNG2nGBr0ADq3lKLYOk9OPJx+YEBdZIajN39EzB01jZIBxqnvqt5K5rZfHxOpzSrf/bD8Bc5oFVdCAIAAA==" target="_blank">Run the query</a>

```kusto
// Geohash GeoJSON collection
datatable(lng:real, lat:real)
[
    -73.975212, 40.789608,
    -73.916869, 40.818314,
    -73.989148, 40.743273,
]
| project geohash = geo_point_to_geohash(lng, lat, 5)
| project geohash_polygon = geo_geohash_to_polygon(geohash)
| summarize geohash_polygon_lst = make_list(geohash_polygon)
| project bag_pack(
    "type", "Feature",
    "geometry", bag_pack("type", "GeometryCollection", "geometries", geohash_polygon_lst),
    "properties", bag_pack("name", "Geohash polygons collection"))
```

**Output**

|Column1|
|---|
|{<br>"type": "Feature",<br>"geometry": {"type": "GeometryCollection","geometries": [<br>{"type": "Polygon", "coordinates": [[[-74.00390625, 40.78125], [-73.9599609375, 40.78125], [-73.9599609375, 40.8251953125],[ -74.00390625, 40.8251953125], [ -74.00390625, 40.78125]]]},<br>{"type": "Polygon", "coordinates": [[[ -73.9599609375, 40.78125], [-73.916015625, 40.78125], [-73.916015625, 40.8251953125], [-73.9599609375, 40.8251953125], [-73.9599609375, 40.78125]]]},<br>{"type": "Polygon", "coordinates": [[[-74.00390625, 40.7373046875], [-73.9599609375, 40.7373046875], [-73.9599609375, 40.78125], [-74.00390625, 40.78125], [-74.00390625, 40.7373046875]]]}]<br>},<br>"properties": {"name": "Geohash polygons collection"<br>}}|

The following example returns a null result because of the invalid geohash input.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUXBPzc9ILM4IyM+pTM/PU7BVSE/Nj0+HCMaX5McXQCQ0lBKVNK0B6T62yDMAAAA=" target="_blank">Run the query</a>

```kusto
print GeohashPolygon = geo_geohash_to_polygon("a");
```

**Output**

|GeohashPolygon|
|---|
||
