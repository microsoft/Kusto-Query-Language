---
title: Geospatial grid system
description: Learn how to use geospatial grid systems to cluster geospatial data.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 12/18/2022
---

# Geospatial clustering

Geospatial data can be analyzed efficiently using grid systems to create geospatial clusters. You can use geospatial tools to aggregate, cluster, partition, reduce, join, and index geospatial data. These tools improve query runtime performance, reduce stored data size, and visualize aggregated geospatial data.

Azure Data Explorer supports the following methods of geospatial clustering:

* [Geohash](https://en.wikipedia.org/wiki/Geohash)
* [S2 Cell](https://s2geometry.io/devguide/s2cell_hierarchy)
* [H3 Cell](https://eng.uber.com/h3/)

The core functionalities of these methods are:

* Calculate hash\index\cell token of geospatial coordinate. Different geospatial coordinates that belong to same cell will have same cell token value.
* Calculate center point of hash\index\cell token. This point is useful because it may represent all the values in the cell.
* Calculate cell polygon. Calculating cell polygons is useful in cell visualization or other calculations, for example, distance, or point in polygon checks.

## Compare methods

| Criteria | Geohash | S2 Cell | H3 Cell |
|---|---|---|---|
| Levels of hierarchy | 18 | 31 | 16 |
| Cell shape | [Rectangle](geo-geohash-to-polygon-function.md) | [Rectangle](geo-s2cell-to-polygon-function.md) |[Hexagon](geo-h3cell-to-polygon-function.md) |
| Cell edges | straight | geodesic | straight |
| Projection system | None. Encodes latitude and longitude. | Cube face centered quadratic transform. | Icosahedron face centered gnomonic. |
| Neighbors count | [8](geo-geohash-neighbors-function.md) | [8](geo-s2cell-neighbors-function.md) | [6](geo-h3cell-neighbors-function.md) |
| Noticeable feature | Common prefixes indicate points proximity. | 31 hierarchy levels. | Cell shape is hexagonal. |
| Performance | Superb | Superb | Fast |
| Cover polygon with cells | Not supported | [Supported](geo-polygon-to-s2cells-function.md) | Not supported |
| Cell parent | Not supported | Not Supported | [Supported](geo-h3cell-parent-function.md) |
| Cell children | Not supported | Not Supported | [Supported](geo-h3cell-children-function.md) |
| Cell rings | Not supported | Not Supported | [Supported](geo-h3cell-rings-function.md) |

> [!TIP]
> If there is no preference for a specific tool, use the [S2 Cell](#s2-cell-functions).

> [!NOTE]
> Although the hashing\indexing of geospatial coordinates is very fast, there are cases where hashing\indexing on ingestion can be applied to improve query runtime. However, this process may increase stored data size.

## Geohash functions

|Function Name|
|---|
|[geo_point_to_geohash()](geo-point-to-geohash-function.md)|
|[geo_geohash_to_central_point()](geo-geohash-to-central-point-function.md)|
|[geo_geohash_neighbors()](geo-geohash-neighbors-function.md)|
|[geo_geohash_to_polygon()](geo-geohash-to-polygon-function.md)|

## S2 Cell functions

|Function Name|
|---|
|[geo_point_to_s2cell()](geo-point-to-s2cell-function.md)|
|[geo_s2cell_to_central_point()](geo-s2cell-to-central-point-function.md)|
|[geo_s2cell_neighbors()](geo-s2cell-neighbors-function.md)|
|[geo_s2cell_to_polygon()](geo-s2cell-to-polygon-function.md)|
|[geo_polygon_to_s2cells()](geo-polygon-to-s2cells-function.md)|

## H3 Cell functions

|Function Name|
|---|
|[geo_point_to_h3cell()](geo-point-to-h3cell-function.md)|
|[geo_h3cell_to_central_point()](geo-h3cell-to-central-point-function.md)|
|[geo_h3cell_neighbors()](geo-h3cell-neighbors-function.md)|
|[geo_h3cell_to_polygon()](geo-h3cell-to-polygon-function.md)|
|[geo_h3cell_parent()](geo-h3cell-parent-function.md)|
|[geo_h3cell_children()](geo-h3cell-children-function.md)|
|[geo_h3cell_rings()](geo-h3cell-rings-function.md)|

## Next steps

* [Geospatial data processing and analytics](/azure/architecture/example-scenario/data/geospatial-data-processing-analytics-azure)
