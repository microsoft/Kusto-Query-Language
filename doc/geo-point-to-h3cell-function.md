---
title: geo_point_to_h3cell() - Azure Data Explorer
description: Learn how to use the geo_point_to_h3cell() function to calculate the H3 Cell token string value of a geographic location.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 12/14/2022
---
# geo_point_to_h3cell()

Calculates the H3 Cell token string value of a geographic location.

Read more about [H3 Cell](https://eng.uber.com/h3/).

## Syntax

`geo_point_to_h3cell(`*longitude*`,`*latitude*`,`*resolution*`)`

## Arguments

* *longitude*: Longitude value of a geographic location. Longitude *x* will be considered valid if *x* is a real number and *x* is in the range [-180, +180].
* *latitude*: Latitude value of a geographic location. Latitude *y* will be considered valid if y is a real number and y in the range [-90, +90].
* *resolution*: An optional `int` that defines the requested cell resolution. Supported values are in the range [0, 15]. If unspecified, the default value `6` is used.

## Returns

The H3 Cell token string value of a given geographic location. If the coordinates or levels are invalid, the query will produce an empty result.

> [!NOTE]
>
> * H3 Cell can be a useful geospatial clustering tool.
> * H3 Cell has 16 levels of hierarchy with area coverage ranging from 4,250,547km² at the highest level 0 to 0.9m² at the lowest level 15.
> * H3 Cell has a unique hexagon shape and this leads some unique properties:
> * Hexagons have 6 neighbors
> * Hexagons allow us to approximate radiuses easily and all neighbors are equidistant
> * Hexagons are visually pleasant
> * In some rare cases the shape is pentagon.
> * H3 Cell has a rectangular area on a plane surface.
> * Invoking the [geo_h3cell_to_central_point()](geo-h3cell-to-central-point-function.md) function on an H3 Cell token string that was calculated on longitude x and latitude y won't necessarily return x and y.
> * It's possible that two geographic locations are very close to each other but have different H3 Cell tokens.

**H3 Cell approximate area coverage per resolution value**

|Level|Average Hexagon Edge Length|
|--|--|
|0|1108 km|
|1|419 km|
|2|158 km|
|3|60 km|
|4|23 km|
|5|8 km|
|6|3 km|
|7|1 km|
|8|460 m|
|9|174 m|
|10|66 m|
|11|25 m|
|12|9 m|
|13|3 m|
|14|1 m|
|15|0.5 m|

The table source can be found [in this H3 Cell statistical resource](https://h3geo.org/docs/core-library/restable/).

See also [geo_point_to_s2cell()](geo-point-to-s2cell-function.md), [geo_point_to_geohash()](geo-point-to-geohash-function.md).

For comparison with other available grid systems. see [geospatial clustering with Kusto Query Language](geospatial-grid-systems.md).

## Examples

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print h3cell = geo_point_to_h3cell(-74.04450446039874, 40.689250859314974, 6)
```

**Output**

|h3cell|
|---|
|862a1072fffffff|

The following example finds groups of coordinates. Every pair of coordinates in the group resides in the H3 Cell with average hexagon area of 253 km².

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(location_id:string, longitude:real, latitude:real)
[
    "A", -73.956683, 40.807907,
    "B", -73.916869, 40.818314,
    "C", -73.989148, 40.743273,
]
| summarize count = count(),                                         // Items per group count
            locations = make_list(location_id)                       // Items in the group
            by h3cell = geo_point_to_h3cell(longitude, latitude, 5)  // H3 Cell of the group
```

**Output**

|h3cell|count|locations|
|---|---|---|
|852a100bfffffff|2|[<br>  "A",<br>  "B"<br>]|
|852a1073fffffff|1|[<br>  "C"<br>]|

The following example produces an empty result because of the invalid coordinate input.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print h3cell = geo_point_to_h3cell(300,1,8)
```

**Output**

|h3cell|
|---|
||

The following example produces an empty result because of the invalid level input.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print h3cell = geo_point_to_h3cell(1,1,16)
```

**Output**

|h3cell|
|---|
||

The following example produces an empty result because of the invalid level input.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print h3cell = geo_point_to_h3cell(1,1,int(null))
```

**Output**

|h3cell|
|---|
||
