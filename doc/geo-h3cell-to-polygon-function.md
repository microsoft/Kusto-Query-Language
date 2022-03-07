---
title: geo_h3cell_to_polygon() - Azure Data Explorer
description: This article describes geo_h3cell_to_polygon() in Azure Data Explorer.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 06/03/2021
---
# geo_h3cell_to_polygon()

Calculates the polygon that represents the H3 Cell rectangular area.

Read more about [H3 Cell](https://eng.uber.com/h3/).

## Syntax

`geo_h3cell_to_polygon(`*h3cell*`)`

## Arguments

*h3cell*: H3 Cell token string value as it was calculated by [geo_point_to_h3cell()](geo-point-to-h3cell-function.md).

## Returns

Polygon in [GeoJSON Format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type. If the H3 Cell is invalid, the query will produce a null result.

> [!NOTE]
> H3 Cell polygon edges are straight lines and aren't geodesics. If an H3 Cell polygon is part of some other calculation, consider densifying it with [geo_polygon_densify()](geo-polygon-densify-function.md).

## Examples

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print geo_h3cell_to_polygon("862a1072fffffff")
```

|print_0|
|---|
|{<br>"type": "Polygon",<br>"coordinates": [[[-74.0022744646159, 40.735376026215022], [-74.046908029686236, 40.727986222489115], [-74.060610712223664, 40.696775140349033],[  -74.029724408156682, 40.672970047595463], [-73.985140983708192, 40.680349049267583],[  -73.971393761028622, 40.71154393543933], [-74.0022744646159, 40.735376026215022]]]<br>}|

The following example assembles GeoJSON geometry collection of H3 Cell polygons.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
// H3 cell GeoJSON collection
datatable(lng:real, lat:real)
[
    -73.956683, 40.807907,
    -73.916869, 40.818314,
    -73.989148, 40.743273,
]
| project h3_hash = geo_point_to_h3cell(lng, lat, 6)
| project h3_hash_polygon = geo_h3cell_to_polygon(h3_hash)
| summarize h3_hash_polygon_lst = make_list(h3_hash_polygon)
| project pack(
    "type", "Feature",
    "geometry", pack("type", "GeometryCollection", "geometries", h3_hash_polygon_lst),
    "properties", pack("name", "H3 polygons collection"))
```

|Column1|
|---|
|{<br>"type": "Feature",<br>"geometry": {"type": "GeometryCollection", "geometries": [{"type": "Polygon","coordinates": [[[-73.9609635556213, 40.829061732419916], [-74.005691351383675, 40.821680937801922], [-74.019448383546617, 40.790439140236963], [-73.988522328408948, 40.766594382212254], [-73.943844904976629, 40.773964402038523], [-73.930043202964953, 40.805189944379514], [-73.9609635556213, 40.829061732419916] ]]},<br>{"type": "Polygon", "coordinates": [[[-73.902385078754875, 40.867671551513595], [-73.94715685019348, 40.860310688399885], [-73.9609635556213, 40.829061732419916], [-73.930043202964953, 40.805189944379514], [-73.885321931061725, 40.812540084842404 ], [-73.871470551071766, 40.843772725733125], [ -73.902385078754875, 40.867671551513595]]]},<br>{"type": "Polygon","coordinates": [[[-73.943844904976629, 40.773964402038523], [-73.988522328408948, 40.766594382212254], [-74.0022744646159, 40.735376026215022], [-73.971393761028622, 40.71154393543933], [-73.926766604813565, 40.718903205013063], [ -73.912969923470314, 40.750105305345329 ], [-73.943844904976629, 40.773964402038523]]]}]<br>},<br>"properties": {"name": "H3 polygons collection"}<br>}|

The following example returns a null result because of the invalid H3 Cell token input.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print geo_h3cell_to_polygon("@")
```

|print_0|
|---|
||
