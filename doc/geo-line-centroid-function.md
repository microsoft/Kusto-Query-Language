---
title:  geo_line_centroid()
description: Learn how to use the geo_line_centroid() function to calculate the centroid of a line or a multiline on Earth.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 03/09/2023
---
# geo_line_centroid()

Calculates the centroid of a line or a multiline on Earth.

## Syntax

`geo_line_centroid(`*lineString*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *lineString* | dynamic | &check; | A LineString or MultiLineString in the [GeoJSON format](https://tools.ietf.org/html/rfc7946).|

## Returns

The centroid coordinate values in [GeoJSON Format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type. If the line or the multiline is invalid, the query will produce a null result.

> [!NOTE]
>
> * The geospatial coordinates are interpreted as represented by the [WGS-84](https://earth-info.nga.mil/index.php?dir=wgs84&action=wgs84) coordinate reference system.
> * The [geodetic datum](https://en.wikipedia.org/wiki/Geodetic_datum) used to measure distance on Earth is a sphere. Line edges are [geodesics](https://en.wikipedia.org/wiki/Geodesic) on the sphere.
> * If input line edges are straight cartesian lines, consider using [geo_line_densify()](geo-line-densify-function.md) in order to convert planar edges to geodesics.
> * If input is a multiline and contains more than one line, the result will be the centroid of lines union.

**LineString definition and constraints**

dynamic({"type": "LineString","coordinates": [[lng_1,lat_1], [lng_2,lat_2], ..., [lng_N,lat_N]]})

dynamic({"type": "MultiLineString","coordinates": [[line_1, line_2, ..., line_N]]})

* LineString coordinates array must contain at least two entries.
* Coordinates [longitude, latitude] must be valid where longitude is a real number in the range [-180, +180] and latitude is a real number in the range [-90, +90].
* Edge length must be less than 180 degrees. The shortest edge between the two vertices will be chosen.

## Examples

The following example calculates line centroid.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVHIycxLVbBVSKnMS8zNTNaoViqpLEhVslLyAYoHlxRl5qUr6Sgl5+cXpWTmJZakFitZRUfrmhvrWZqaW5rpKJgY6FkYGJgYxeooQITNjQ3NwcLmZiYmFmaxsbWa1lwFQHNKFJJT80qK8jNTgNalp+bHg2yOh4lpgHia1gBYQZ0fkgAAAA==" target="_blank">Run the query</a>

```kusto
let line = dynamic({"type":"LineString","coordinates":[[-73.95796, 40.80042], [-73.97317, 40.764486]]});
print centroid = geo_line_centroid(line);
```

**Output**

|centroid|
|---|
|{"type": "Point", "coordinates": [-73.965567057230942, 40.782453249627416]}|

The following example calculates line centroid longitude.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA02QwWrDMAyG732K4FMLWZBkW5Y79ga77RhCKYkJLp0TMl/Ktnefs5Ax3X7p45f030Ou7jGF6qUaHun6Hvvjp8qPOaizei39t7zENKpa9dO0DDFdc/hQ57Z9crrxVsARM2mrLdUGGgEwxGgMstdou3rjjGdGskaEBFfOefboLEIp+4c5jU6Ei5fV/hdjY1ZdzEBrs3NCCIWxSOztxjlhLgKBCHeMyzHsWFDEyLZVQBw4q8nrrvs+PR/m8l2u+pDyMsWhhDCG6bLmcdl7x1WdDl/VvEy30Je00li4fdz8y6WF7gcIBtdSTwEAAA==" target="_blank">Run the query</a>

```kusto
let line = dynamic({"type":"LineString","coordinates":[[-73.95807266235352,40.800426144169315],[-73.94966125488281,40.79691751000055],[-73.97317886352539,40.764486356930334],[-73.98210525512695,40.76786669510221],[-73.96004676818848,40.7980870753293]]});
print centroid = geo_line_centroid(line)
| project lng = centroid.coordinates[0]
```

**Output**

|lng|
|---|
|-73.9660675626837|

The following example visualizes line centroid on a map.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAzWPwWrDMBBE7/kKoZMNaVh5tSsppX/Qnno0JhhbJKKObBSVEtr+e+WaHGf37ezM5LOYQvTiRYz32F/DUH3LfF+8PMq3zymH17J8zynEs9zLYZ7TGGKf/U0e27Z9MnhwZJxli4qZoNlrOFgAIgYH0CjN3O03ziqHSA4NrZDhopVRlsho7gq0UdppQMONVQbtP+i4cKQAVtuHmUFlrGWkhtBtfhqJUYMuf1XXdb/1824pubMYfMxpDmPpePbzaa17esyqVdW7H5F8HH0St6HP2afh0qcsvkK+iOojxPX02i/1H97E8mMuAQAA" target="_blank">Run the query</a>

```kusto
let line = dynamic({"type":"MultiLineString","coordinates":[[[-73.95798683166502,40.800556090021466],[-73.98193359375,40.76819171855746]],[[-73.94940376281738,40.79691751000055],[-73.97317886352539,40.76435634049001]]]});
print centroid = geo_line_centroid(line)
| render scatterchart with (kind = map)
```

:::image type="content" source="images/geo-line-centroid-function/nyc-central-park-centroid.png" alt-text="Screenshot of N Y C Central park line centroid.":::

The following example returns True because of the invalid line.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAx2MQQqAIBAAvyJ7UvDSNegH3TpKiOkiC7aGbYeI/p50nBmYoxGLotNvIflCjGrqxFcpOmP9jY/I0iolnW4OO0X9gNwHwghzz4v0RQYLsdaWiIPgCaNzg1XDur7GmA+R8wB3ZAAAAA==" target="_blank">Run the query</a>

```kusto
print is_bad_line = isnull(geo_line_centroid(dynamic({"type":"LineString","coordinates":[[1, 1]]})))
```

**Output**

|is_bad_line|
|---|
|True|
