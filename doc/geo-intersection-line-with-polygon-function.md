---
title: geo_intersection_line_with_polygon() - Azure Data Explorer
description: Learn how to use the geo_intersection_line_with_polygon() function to calculate the intersection of a line string or a multiline string with a polygon or a multipolygon.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 12/14/2022
---
# geo_intersection_line_with_polygon()

Calculates the intersection of a line or a multiline with a polygon or a multipolygon.

## Syntax

`geo_intersection_line_with_polygon(`*lineString*`,`*polygon*`)`

## Arguments

* *lineString*: LineString or MultiLineString in the [GeoJSON format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type.
* *polygon*: Polygon or MultiPolygon in the [GeoJSON format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type.

## Returns

Intersection in [GeoJSON Format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type. If lineString or a multiLineString or a polygon or a multipolygon are invalid, the query will produce a null result.

> [!NOTE]
>
> * The geospatial coordinates are interpreted as represented by the [WGS-84](https://earth-info.nga.mil/GandG/update/index.php?action=home) coordinate reference system.
> * The [geodetic datum](https://en.wikipedia.org/wiki/Geodetic_datum) used to measure distance on Earth is a sphere. Line edges are [geodesics](https://en.wikipedia.org/wiki/Geodesic) on the sphere.
> * If input line or a polygon edges are straight cartesian lines, consider using [geo_line_densify()](geo-line-densify-function.md) or a [geo_polygon_densify()](geo-polygon-densify-function.md) in order to convert planar edges to geodesics.

**LineString definition and constraints**

dynamic({"type": "LineString","coordinates": [[lng_1,lat_1], [lng_2,lat_2], ..., [lng_N,lat_N]]})

dynamic({"type": "MultiLineString","coordinates": [[line_1, line_2, ..., line_N]]})

* LineString coordinates array must contain at least two entries.
* Coordinates [longitude, latitude] must be valid where longitude is a real number in the range [-180, +180] and latitude is a real number in the range [-90, +90].
* Edge length must be less than 180 degrees. The shortest edge between the two vertices will be chosen.

**Polygon definition and constraints**

dynamic({"type": "Polygon","coordinates": [LinearRingShell, LinearRingHole_1, ..., LinearRingHole_N]})

dynamic({"type": "MultiPolygon","coordinates": [[LinearRingShell, LinearRingHole_1, ..., LinearRingHole_N],..., [LinearRingShell, LinearRingHole_1, ..., LinearRingHole_M]]})

* LinearRingShell is required and defined as a `counterclockwise` ordered array of coordinates [[lng_1,lat_1],...,[lng_i,lat_i],...,[lng_j,lat_j],...,[lng_1,lat_1]]. There can be only one shell.
* LinearRingHole is optional and defined as a `clockwise` ordered array of coordinates [[lng_1,lat_1],...,[lng_i,lat_i],...,[lng_j,lat_j],...,[lng_1,lat_1]]. There can be any number of interior rings and holes.
* LinearRing vertices must be distinct with at least three coordinates. The first coordinate must be equal to the last. At least four entries are required.
* Coordinates [longitude, latitude] must be valid. Longitude must be a real number in the range [-180, +180] and latitude must be a real number in the range [-90, +90].
* LinearRingShell encloses at most half of the sphere. LinearRing divides the sphere into two regions. The smaller of the two regions will be chosen.
* LinearRing edge length must be less than 180 degrees. The shortest edge between the two vertices will be chosen.
* LinearRings must not cross and must not share edges. LinearRings may share vertices.
* Polygon contains its vertices.

> [!TIP]
>
> Use literal LineString or MultiLineString for better performance.

## Examples

The following example calculates intersection between line and polygon. In this case, the result is a line.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let lineString = dynamic({"type":"LineString","coordinates":[[-73.985195,40.788275],[-73.974552,40.779761]]});
let polygon = dynamic({"type":"Polygon","coordinates":[[[-73.9712905883789,40.78580561168767],[-73.98004531860352,40.775276834803655],[-73.97000312805176,40.77852663535664],[-73.9712905883789,40.78580561168767]]]});
print intersection = geo_intersection_line_with_polygon(lineString, polygon)
```

**Output**

|intersection|
|---|
|{"type": "LineString","coordinates": [[-73.975611956578192,40.78060906714618],[-73.974552,40.779761]]}|

The following example calculates intersection between line and polygon. In this case, the result is a multiline.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let lineString = dynamic({"type":"LineString","coordinates":[[-110.522, 39.198],[-91.428, 40.880]]});
let polygon = dynamic({"type":"Polygon","coordinates":[[[-90.263,36.738],[-102.041,45.274],[-109.335,36.527],[-90.263,36.738]],[[-100.393,41.705],[-103.139,38.925],[-97.558,39.113],[-100.393,41.705]]]});
print intersection = geo_intersection_line_with_polygon(lineString, polygon)
```

**Output**

|intersection|
|---|
|{"type": "MultiLineString","coordinates": [[[  -106.89353655881905,  39.769226209776306],[  -101.74448553679453,  40.373506008712525]],[[-99.136499431328858,  40.589336512699994],[-95.284527737311791,  40.799060242246348]]]}|

The following line and polygon don't intersect.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let lineString = dynamic({"type":"LineString","coordinates":[[1, 1],[2, 2]]});
let polygon = dynamic({"type":"Polygon","coordinates":[[[-73.9712905883789,40.78580561168767],[-73.98004531860352,40.775276834803655],[-73.97000312805176,40.77852663535664],[-73.9712905883789,40.78580561168767]]]});
print intersection = geo_intersection_line_with_polygon(lineString, polygon)
```

**Output**

|intersection|
|---|
|{"type": "GeometryCollection","geometries": []}|

The following example finds all roads in the NYC GeoJSON roads table that intersects with the area of interest literal polygon.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let area_of_interest = dynamic({"type":"Polygon","coordinates":[[[-73.95768642425537,40.80065354924362],[-73.9582872390747,40.80089719667298],[-73.95869493484497,40.80050736035672],[-73.9580512046814,40.80019873831593],[-73.95768642425537,40.80065354924362]]]});
NY_Manhattan_Roads
| project name = features.properties.Label, road = features.geometry
| project name, intersection = geo_intersection_line_with_polygon(road, area_of_interest)
| where array_length(intersection.geometries) != 0
```

**Output**

|name|intersection|
|---|---|
|CentralParkW|{"type":"MultiLineString","coordinates":[[[-73.958295846836933,40.800316027289647],[-73.9582724,40.8003415]],[[-73.958413422194482,40.80037239620097],[-73.9584093,40.8003797]]]}|
|FrederickDouglassCir|{"type":"LineString","coordinates":[[-73.9579272943862,40.800751229494182],[-73.9579019,40.8007238],[-73.9578688,40.8006749],[-73.9578508,40.8006203],[-73.9578459,40.800570199999996],[-73.9578484,40.80053310000001],[-73.9578627,40.800486700000008],[-73.957913,40.800421100000008],[-73.9579668,40.8003923],[-73.9580189,40.80037260000001],[-73.9580543,40.8003616],[-73.9581237,40.8003395],[-73.9581778,40.8003365],[-73.9582724,40.8003415],[-73.958308,40.8003466],[-73.9583328,40.8003517],[-73.9583757,40.8003645],[-73.9584093,40.8003797],[-73.9584535,40.80041099999999],[-73.9584818,40.8004536],[-73.958507000000012,40.8004955],[-73.9585217,40.800562400000004],[-73.9585282,40.8006155],[-73.958416200000016,40.8007325],[-73.9583541,40.8007785],[-73.9582772,40.800811499999995],[-73.9582151,40.8008285],[-73.958145918999392,40.800839887820239]]}|
|W110thSt|{"type":"MultiLineString","coordinates":[[[-73.957828446036331,40.800476476316327],[-73.9578627,40.800486700000008]],[[-73.9585282,40.8006155],[-73.958565492035873,40.800631133466972]],[[-73.958416200000016,40.8007325],[-73.958446850928084,40.800744577466617]]]}|
|WestDr|{"type":"LineString","coordinates":[[-73.9580543,40.8003616],[-73.958009693938735,40.800250494588468]]}|

The following example finds all counties in the USA that intersect with area of interest literal LineString.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let area_of_interest = dynamic({"type":"LineString","coordinates":[[-73.97159099578857,40.794513338780895],[-73.96738529205322,40.792758888618756],[-73.96978855133057,40.789769718601505]]});
US_Counties
| project name = features.properties.NAME, county = features.geometry
| project name, intersection = geo_intersection_line_with_polygon(area_of_interest, county)
| where array_length(intersection.geometries) != 0
```

**Output**

|name|intersection|
|---|---|
|New York|{"type": "LineString","coordinates": [[-73.971590995788574, 40.794513338780895], [-73.967385292053223, 40.792758888618756],[-73.969788551330566, 40.789769718601512]]}|

The following example will return a null result because the LineString is invalid.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let lineString = dynamic({"type":"LineString","coordinates":[[-73.985195,40.788275]]});
let polygon = dynamic({"type":"Polygon","coordinates":[[[-73.95768642425537,40.80065354924362],[-73.9582872390747,40.80089719667298],[-73.95869493484497,40.80050736035672],[-73.9580512046814,40.80019873831593],[-73.95768642425537,40.80065354924362]]]});
print is_invalid = isnull(geo_intersection_2lines(lineString, polygon))
```

**Output**

|is_invalid|
|---|
|1|

The following example will return a null result because the polygon is invalid.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let lineString = dynamic({"type":"LineString","coordinates":[[-73.97159099578857,40.794513338780895],[-73.96738529205322,40.792758888618756],[-73.96978855133057,40.789769718601505]]});
let polygon = dynamic({"type":"Polygon","coordinates":[]});
print is_invalid = isnull(geo_intersection_2lines(lineString, polygon))
```

**Output**

|is_invalid|
|---|
|1|
