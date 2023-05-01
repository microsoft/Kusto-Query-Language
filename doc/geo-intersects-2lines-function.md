---
title: geo_intersects_2lines() - Azure Data Explorer
description: Learn how to use the geo_intersects_2lines() function to check if two line strings or multiline strings intersect.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 03/09/2023
---
# geo_intersects_2lines()

Calculates whether two lines or multilines intersect.

## Syntax

`geo_intersects_2lines(`*lineString1*`,`*lineString2*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *lineString1* | dynamic | &check; | A line or multiline in the [GeoJSON format](https://tools.ietf.org/html/rfc7946).|
| *lineString2* | dynamic | &check; | A line or multiline in the [GeoJSON format](https://tools.ietf.org/html/rfc7946).|

## Returns

Indicates whether two lines or multilines intersect. If lineString or a multiLineString are invalid, the query will produce a null result.

> [!NOTE]
>
> * The geospatial coordinates are interpreted as represented by the [WGS-84](https://earth-info.nga.mil/index.php?dir=wgs84&action=wgs84) coordinate reference system.
> * The [geodetic datum](https://en.wikipedia.org/wiki/Geodetic_datum) used to measure distance on Earth is a sphere. Line edges are [geodesics](https://en.wikipedia.org/wiki/Geodesic) on the sphere.
> * If input line edges are straight cartesian lines, consider using [geo_line_densify()](geo-line-densify-function.md) in order to convert planar edges to geodesics.

**LineString definition and constraints**

dynamic({"type": "LineString","coordinates": [[lng_1,lat_1], [lng_2,lat_2], ..., [lng_N,lat_N]]})

dynamic({"type": "MultiLineString","coordinates": [[line_1, line_2, ..., line_N]]})

* LineString coordinates array must contain at least two entries.
* Coordinates [longitude, latitude] must be valid where longitude is a real number in the range [-180, +180] and latitude is a real number in the range [-90, +90].
* Edge length must be less than 180 degrees. The shortest edge between the two vertices will be chosen.

> [!TIP]
>
> Use literal LineString or MultiLineString for better performance.

## Examples

The following example checks whether some two literal lines intersects.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA52QsQqDMBRF935FyKSQij6NL7H0D7p1FBHRIAEbxWSR0n9vWou1a4e3nAv3XN6gHBm0UVc3a9Mn5Ey6xTQ33QZ36pZJ0YJetpgy2o7j3GnTOGVpUZZHTCOJQoJkWRyh4AnnFVuxiGWcrhhySKrqEZ4Ow48P/vJ5i+RrsQDcfJhxDm+MEvOPb/I9jvhTs1Wts17Yq7H+ghpec2ywewLbLwyfFywOhCIBAAA=" target="_blank">Run the query</a>

```kusto
let lineString1 = dynamic({"type":"LineString","coordinates":[[-73.978929,40.785155],[-73.980903,40.782621]]});
let lineString2 = dynamic({"type":"LineString","coordinates":[[-73.985195,40.788275],[-73.974552,40.779761]]});
print intersects = geo_intersects_2lines(lineString1, lineString2)
```

**Output**

|intersects|
|---|
|True|

The following example finds all roads in the NYC GeoJSON roads table that intersects with some lines of interest.

```kusto
let my_road = dynamic({"type":"LineString","coordinates":[[-73.97892951965332,40.78515573551921],[-73.98090362548828,40.78262115769851]]});
NY_Manhattan_Roads
| project name = features.properties.Label, road = features.geometry
| where geo_intersects_2lines(road, my_road)
| project name
```

**Output**

|name|
|---|
|Broadway|
|W 78th St|
|W 79th St|
|W 80th St|
|W 81st St|

The following example will return a null result because one of lines is invalid.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA52PQQqDMBRE9z1FyCpCKjE2TWLpDbrrUoKIfiQQo5h0IaV3ryVQ7LbLmeHP++MgImc93ONi/VCgK+pX3462I08c1xlwhW/fGFPcTdPSW99GCLiq66Mscy2V5pqeWC6VKIQwNNmKaVYmm595YcwruxzcD4//xdsoWqRixaVIxfN2EJEN/uEcGWBqNglLgC6Ghn+Igex20v0TWfYGvZfHxAYBAAA=" target="_blank">Run the query</a>

```kusto
let lineString1 = dynamic({"type":"LineString","coordinates":[[-73.978929,40.785155],[-73.980903,40.782621]]});
let lineString2 = dynamic({"type":"LineString","coordinates":[[-73.985195,40.788275]]});
print isnull(geo_intersects_2lines(lineString1, lineString2))
```

**Output**

|print_0|
|---|
|True|
