---
title: geo_union_lines_array() - Azure Data Explorer
description: Learn how to use the geo_union_lines_array() function to calculate the union of line strings or multiline strings on Earth.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 12/14/2022
---
# geo_union_lines_array()

Calculates the union of lines or multilines on Earth.

## Syntax

`geo_union_lines_array(`*lineStrings*`)`

## Arguments

* *lineStrings*: An array of lines or multilines in the [GeoJSON format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type.

## Returns

A line or a multiline in [GeoJSON Format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type. If any of the provided lines or multilines is invalid, the query will produce a null result.

> [!NOTE]
>
> * The geospatial coordinates are interpreted as represented by the [WGS-84](https://earth-info.nga.mil/GandG/update/index.php?action=home) coordinate reference system.
> * The [geodetic datum](https://en.wikipedia.org/wiki/Geodetic_datum) used for measurements on Earth is a sphere. Polygon edges are [geodesics](https://en.wikipedia.org/wiki/Geodesic) on the sphere.
> * If input line edges are straight cartesian lines, consider using [geo_line_densify()](geo-line-densify-function.md) in order to convert planar edges to geodesics.

**LineString definition and constraints**

dynamic({"type": "LineString","coordinates": [[lng_1,lat_1], [lng_2,lat_2], ..., [lng_N,lat_N]]})

dynamic({"type": "MultiLineString","coordinates": [[line_1, line_2, ..., line_N]]})

* LineString coordinates array must contain at least two entries.
* Coordinates [longitude, latitude] must be valid where longitude is a real number in the range [-180, +180] and latitude is a real number in the range [-90, +90].
* Edge length must be less than 180 degrees. The shortest edge between the two vertices will be chosen.

## Examples

The following example performs geospatial union on line rows.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(lines:dynamic)
[
    dynamic({"type":"LineString","coordinates":[[-73.95683884620665,40.80502891480884],[-73.95633727312088,40.8057171711177],[-73.95489156246185,40.80510200431311]]}),
    dynamic({"type":"LineString","coordinates":[[-73.95633727312088,40.8057171711177],[-73.95489156246185,40.80510200431311],[-73.95537436008453,40.804413741624515]]}),
    dynamic({"type":"LineString","coordinates":[[-73.95633727312088,40.8057171711177],[-73.95489156246185,40.80510200431311]]})
]
| summarize lines_arr = make_list(lines)
| project lines_union = geo_union_lines_array(lines_arr)
```

**Output**

|lines_union|
|---|
|{"type": "LineString", "coordinates": [[-73.956838846206651, 40.805028914808844], [-73.95633727312088, 40.8057171711177], [ -73.954891562461853, 40.80510200431312], [-73.955374360084534, 40.804413741624522]]}|

The following example performs geospatial union on line columns.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(line1:dynamic, line2:dynamic)
[
    dynamic({"type":"LineString","coordinates":[[-73.95683884620665,40.80502891480884],[-73.95633727312088,40.8057171711177],[-73.95489156246185,40.80510200431311]]}), dynamic({"type":"LineString","coordinates":[[-73.95633727312088,40.8057171711177],[-73.95489156246185,40.80510200431311],[-73.95537436008453,40.804413741624515]]})
]
| project lines_arr = pack_array(line1, line2)
| project lines_union = geo_union_lines_array(lines_arr)
```

**Output**

|lines_union|
|---|
|{"type": "LineString", "coordinates":[[-73.956838846206651, 40.805028914808844], [-73.95633727312088, 40.8057171711177], [-73.954891562461853, 40.80510200431312], [-73.955374360084534, 40.804413741624522]]}|

The following example returns True because one of the lines is invalid.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(lines:dynamic)
[
    dynamic({"type":"LineString","coordinates":[[-73.95683884620665,40.80502891480884],[-73.95633727312088,40.8057171711177],[-73.95489156246185,40.80510200431311]]}),
    dynamic({"type":"LineString","coordinates":[[1, 1]]})
]
| summarize lines_arr = make_list(lines)
| project invalid_union = isnull(geo_union_lines_array(lines_arr))
```

**Output**

|invalid_union|
|---|
|True|
