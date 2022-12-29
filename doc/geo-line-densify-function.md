---
title: geo_line_densify() - Azure Data Explorer
description: Learn how to use the geo_line_densify() function to convert planar lines or multiline edges to geodesics.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 12/14/2022
---
# geo_line_densify()

Converts planar lines or multiline edges to geodesics by adding intermediate points.

## Syntax

`geo_line_densify(`*lineString*`,`*tolerance*`)`

`geo_line_densify(`*lineString*`,`*tolerance*`,`*preserve_crossing*`)`

## Arguments

* *lineString*: Line or multiline in the [GeoJSON format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type.
* *tolerance*: An optional numeric that defines maximum distance in meters between the original planar edge and the converted geodesic edge chain. Supported values are in the range [0.1, 10000]. If unspecified, the default value `10` is used.
* *preserve_crossing*: An optional boolean that preserves edge crossing over antimeridian. If unspecified, the default value `False` is used.

## Returns

Densified line in the [GeoJSON format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type. If either the line or tolerance is invalid, the query will produce a null result.

> [!NOTE]
>
> The geospatial coordinates are interpreted as represented by the [WGS-84](https://earth-info.nga.mil/GandG/update/index.php?action=home) coordinate reference system.

**LineString definition**

dynamic({"type": "LineString","coordinates": [[lng_1,lat_1], [lng_2,lat_2], ..., [lng_N,lat_N]]})

dynamic({"type": "MultiLineString","coordinates": [[line_1, line_2, ..., line_N]]})

* LineString coordinates array must contain at least two entries.
* The coordinates [longitude, latitude] must be valid. The longitude must be a real number in the range [-180, +180] and the latitude must be a real number in the range [-90, +90].
* The edge length must be less than 180 degrees. The shortest edge between the two vertices will be chosen.

**Constraints**

* The maximum number of points in the densified line is limited to 10485760.
* Storing lines in [dynamic](./scalar-data-types/dynamic.md) format has size limits.

**Motivation**

* [GeoJSON format](https://tools.ietf.org/html/rfc7946) defines an edge between two points as a straight cartesian line while Azure Data Explorer uses [geodesic](https://en.wikipedia.org/wiki/Geodesic).
* The decision to use geodesic or planar edges might depend on the dataset and is especially relevant in long edges.

## Examples

The following example densifies a road in Manhattan island. The edge is short and the distance between the planar edge and its geodesic counterpart is less than the distance specified by tolerance. As such, the result remains unchanged.

```kusto
print densified_line = tostring(geo_line_densify(dynamic({"type":"LineString","coordinates":[[-73.949247, 40.796860],[-73.973017, 40.764323]]})))
```

**Output**

|densified_line|
|---|
|{"type":"LineString","coordinates":[[-73.949247, 40.796860], [-73.973017, 40.764323]]}|

The following example densifies an edge of ~130-km length

```kusto
print densified_line = tostring(geo_line_densify(dynamic({"type":"LineString","coordinates":[[50, 50], [51, 51]]})))
```

**Output**

|densified_line|
|---|
|{"type":"LineString","coordinates":[[50,50],[50.125,50.125],[50.25,50.25],[50.375,50.375],[50.5,50.5],[50.625,50.625],[50.75,50.75],[50.875,50.875],[51,51]]}|

The following example returns a null result because of the invalid coordinate input.

```kusto
print densified_line = geo_line_densify(dynamic({"type":"LineString","coordinates":[[300,1],[1,1]]}))
```

**Output**

|densified_line|
|---|
||

The following example returns a null result because of the invalid tolerance input.

```kusto
print densified_line = geo_line_densify(dynamic({"type":"LineString","coordinates":[[1,1],[2,2]]}), 0)
```

**Output**

|densified_line|
|---|
||
