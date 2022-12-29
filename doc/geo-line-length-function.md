---
title: geo_line_length() - Azure Data Explorer
description: Learn how to use the geo_line_length() function to calculate the total length of a line string or a multiline string on Earth.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 12/14/2022
---
# geo_line_length()

Calculates the total length of a line or a multiline on Earth.

## Syntax

`geo_line_length(`*lineString*`)`

## Arguments

* *lineString*: Line or multiline in the [GeoJSON format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type.

## Returns

The total length of a line or a multiline, in meters, on Earth. If the line or multiline is invalid, the query will produce a null result.

> [!NOTE]
>
> * The geospatial coordinates are interpreted as represented by the [WGS-84](https://earth-info.nga.mil/GandG/update/index.php?action=home) coordinate reference system.
> * The [geodetic datum](https://en.wikipedia.org/wiki/Geodetic_datum) used to measure distance on Earth is a sphere. Line edges are [geodesics](https://en.wikipedia.org/wiki/Geodesic) on the sphere.
> * If input line edges are straight cartesian lines, consider using [geo_line_densify()](geo-line-densify-function.md) in order to convert planar edges to geodesics.
> * If input is a multiline and contains more than one line, the result will be total length of lines union.

**LineString definition and constraints**

dynamic({"type": "LineString","coordinates": [[lng_1,lat_1], [lng_2,lat_2], ..., [lng_N,lat_N]]})

dynamic({"type": "MultiLineString","coordinates": [[line_1, line_2, ..., line_N]]})

* LineString coordinates array must contain at least two entries.
* Coordinates [longitude, latitude] must be valid where longitude is a real number in the range [-180, +180] and latitude is a real number in the range [-90, +90].
* Edge length must be less than 180 degrees. The shortest edge between the two vertices will be chosen.

## Examples

The following example calculates the total line length, in meters.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let line = dynamic({"type":"LineString","coordinates":[[-73.95807266235352,40.800426144169315],[-73.94966125488281,40.79691751000055],[-73.97317886352539,40.764486356930334]]});
print length = geo_line_length(line)
```

**Output**

|length|
|---|
|4922.48016992081|

The following example calculates total multiline length, in meters.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let line = dynamic({"type":"MultiLineString","coordinates":[[[-73.95798683166502,40.800556090021466],[-73.98193359375,40.76819171855746]],[[-73.94940376281738,40.79691751000055],[-73.97317886352539,40.76435634049001]]]});
print length = geo_line_length(line)
```

**Output**

|length|
|---|
|8262.24339753741|

The following example returns True because of the invalid line.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print is_bad_line = isnull(geo_line_length(dynamic({"type":"LineString","coordinates":[[1, 1]]})))
```

**Output**

|is_bad_line|
|---|
|True|
