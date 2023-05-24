---
title:  geo_intersection_2lines()
description: Learn how to use the geo_intersection_2lines() function to calculate the intersection of two line strings or multiline strings.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 03/09/2023
---
# geo_intersection_2lines()

Calculates the intersection of two lines or multilines.

## Syntax

`geo_intersection_2lines(`*lineString1*`,`*lineString2*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *lineString1* | dynamic | &check; | A line or multiline in the [GeoJSON format](https://tools.ietf.org/html/rfc7946).|
| *lineString2* | dynamic | &check; | A line or multiline in the [GeoJSON format](https://tools.ietf.org/html/rfc7946).|

## Returns

Intersection in [GeoJSON Format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type. If LineString or a MultiLineString are invalid, the query will produce a null result.

> [!NOTE]
>
> * The geospatial coordinates are interpreted as represented by the [WGS-84](https://earth-info.nga.mil/index.php?dir=wgs84&action=wgs84) coordinate reference system.
> * The [geodetic datum](https://en.wikipedia.org/wiki/Geodetic_datum) used to measure distance on Earth is a sphere. Line edges are [geodesics](https://en.wikipedia.org/wiki/Geodesic) on the sphere.
> * If input line edges are straight cartesian lines, consider using [geo_line_densify()](geo-line-densify-function.md) in order to convert planar edges to geodesics.

**LineString definition and constraints**

dynamic({"type": "LineString","coordinates": [[lng_1,lat_1], [lng_2,lat_2],..., [lng_N,lat_N]]})

dynamic({"type": "MultiLineString","coordinates": [[line_1, line_2,..., line_N]]})

* LineString coordinates array must contain at least two entries.
* Coordinates [longitude, latitude] must be valid where longitude is a real number in the range [-180, +180] and latitude is a real number in the range [-90, +90].
* Edge length must be less than 180 degrees. The shortest edge between the two vertices will be chosen.

> [!TIP]
>
> Use literal LineString or MultiLineString for better performance.

## Examples

The following example calculates intersection between two lines. In this case, the result is a point.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA52QsQqDMBRF935FyKSQikbjSyz9g24dRUT0IQFNRLNI6b83rUXs2uEt58I9lzegI4M2eHezNn1CrqRbTTPqNnhQt05IC3rbY8poa+3cadM4XGhRlmdIIwVSccWyOAIpEiEqtmEZqzjdMM95UlXP8HIafnz8L5+3KLEVSw67DzIh+AeDgvzrm3yPI/5wXrB12hqv7NHWR1Tz96QlODyCHVeGL/xzpu8mAQAA" target="_blank">Run the query</a>

```kusto
let lineString1 = dynamic({"type":"LineString","coordinates":[[-73.978929,40.785155],[-73.980903,40.782621]]});
let lineString2 = dynamic({"type":"LineString","coordinates":[[-73.985195,40.788275],[-73.974552,40.779761]]});
print intersection = geo_intersection_2lines(lineString1, lineString2)
```

**Output**

|intersection|
|---|
|{"type": "Point","coordinates": [-73.979837116670978,40.783989289772165]}|

The following example calculates intersection between two lines. In this case, the result is a line.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVHIycxLVbBVSKnMS8zNTNaoViqpLEhVslLyAYoHlxRl5qUr6Sgl5+cXpWTmJZakFitZRUfrmhvrWZpbWBpZ6pgY6JlbmBqamsbqQIQtDCwNjCHCRmZGhrGxtZrWXAVAc0oUgDi1qDg1uSQzPw9oZXpqfjyyULwRyC3FGiBSB+wuTQCXmF3SoQAAAA==" target="_blank">Run the query</a>

```kusto
let line = dynamic({"type":"LineString","coordinates":[[-73.978929,40.785155],[-73.980903,40.782621]]});
print intersection = geo_intersection_2lines(line, line)
```

**Output**

|intersection|
|---|
|{"type": "LineString","coordinates": [[ -73.978929, 40.785155],[ -73.980903, 40.782621]]}|

The following two lines don't intersect.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVHIycxLDS4pysxLN1SwVUipzEvMzUzWqFYqqSxIVbJS8oFLK+koJefnF6Vk5iWWpBYrWUVHG+ooGMbqRBvpKBjFxtZqWnPloJhnRKp5xjoKxkDzTHQUTCDmFQDVlSgAcWpRcWpySWZ+HtDI9NT8eGSheCOQlcUaSB7RQXaFJgAdJFio5gAAAA==" target="_blank">Run the query</a>

```kusto
let lineString1 = dynamic({"type":"LineString","coordinates":[[1, 1],[2, 2]]});
let lineString2 = dynamic({"type":"LineString","coordinates":[[3, 3],[4, 4]]});
print intersection = geo_intersection_2lines(lineString1, lineString2)
```

**Output**

|intersection|
|---|
|{"type": "GeometryCollection", "geometries": []}|

The following example will return a null result because one of lines is invalid.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA5WPsQoDIRBE+3zFYqVgo9ddyB+kSylyyLkcC2Y91ASOkH+PIRAuZeo3vJlJ2CAR46UV4sXACeLG4UqzfIi2rShGcf5iocWcc4nEoWEVo3NGg/HaWQ3W+6c6HtKPz/7rGzQMH9HaAw2I7yFR7BqqfEtJLpinDrBUnBtlnuy7rcrdB70foNQL22IrxuIAAAA=" target="_blank">Run the query</a>

```kusto
let lineString1 = dynamic({"type":"LineString","coordinates":[[1, 1],[2, 2]]});
let lineString2 = dynamic({"type":"LineString","coordinates":[[3, 3]]});
print invalid = isnull(geo_intersection_2lines(lineString1, lineString2))
```

**Output**

|invalid|
|---|
|1|
