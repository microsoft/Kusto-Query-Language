---
title: geo_union_lines_array() - Azure Data Explorer
description: Learn how to use the geo_union_lines_array() function to calculate the union of line strings or multiline strings on Earth.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 03/09/2023
---
# geo_union_lines_array()

Calculates the union of lines or multilines on Earth.

## Syntax

`geo_union_lines_array(`*lineStrings*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *lineStrings* | dynamic | &check; | An array of lines or multilines in the [GeoJSON format](https://tools.ietf.org/html/rfc7946).|

## Returns

A line or a multiline in [GeoJSON Format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type. If any of the provided lines or multilines is invalid, the query will produce a null result.

> [!NOTE]
>
> * The geospatial coordinates are interpreted as represented by the [WGS-84](https://earth-info.nga.mil/index.php?dir=wgs84&action=wgs84) coordinate reference system.
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

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8WSSWrEMBBF9z6F0coGp1FpbkNukF2WxhjFFo06ttTI6oUz3D1KPBwgBFK1KX3eLz6oBh1Tv4ymGK0zcz0sTk+2L7Mmy1Ntz+IdxeVmUI2eEvUcg3UXVKHe+zBYp6OZUd00D5KezlwoqhQTBAvBK4ZPCnNM1BmYwklvqx2jVBJJgSR1wyR8N4CUB8WSkQvCBKh9GWCCMaNAAdr2s6x+nfMvAuwYp5JRgbFinK4YY5A0SFYO/P+DpgBZm33k832adLBvJv/5706HkD/mk3413WjnuF5BmcBb8FfTxw27O+tdAi/Gr3N32PVSHHP5BY0haIxPAgAA" target="_blank">Run the query</a>

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

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA62Rz2rDMAzG73kK41MDXrH8v4G9QW87hlC8xBRvnR1c9xC2vfucJulltzHrok/8PklYg80lXi9ud/HBQTNMwX74nqBZsk3WVVuh8la5+8R5Gh1u8LFQLzn5cMYE9zGmwQeb3RU3bfuk+f4gleHGCMWoUpIIujdUUmYOIAwt9Y5sGOeaaQ6sVFdMwxwAWj8oUYxSMaHAbM2AMkoFBw7Qdd81+dOO/zF8wyTXgitKjZB8wYSAUoNilSDnJauu+kJjim+uz/ePvp5sSugZjbZ/n1M7LedYz1D/wm/Bx1AMZxeX/PRos3rvef0DcJZ9Yt8BAAA=" target="_blank">Run the query</a>

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

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA5WQzWrFIBCF93kKcZVAenHUqDfQN+iuyxCCTeTirdGLMYX0591r70/2ndnMHL5zGGbSKfebM6Wz3izttHk927EqugLluq/lF07bxeAWv2TqNUXrT7jGYwhxsl4ns+C2654kOxwboZhSXFAiRFNzclCkIVQdgSuS9b5+YIxJKhnQrN4xCX8NIOVO8WxsBOUC1CMMCCWEM2AAff9T1f+/E2p0tRZ98Y2WdZ51tJ8GXR8w6BjRM5r1uxmcXdLtLVUGLzGczZiQ9R/a2WlYvQ0+o3bxq3PlyYSbNOw5eiv3uap+AQk/yOdqAQAA" target="_blank">Run the query</a>

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
