---
title: geo_point_buffer() - Azure Data Explorer
description: Learn how to use the geo_point_buffer() function to calculate point buffer
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 04/24/2023
---
# geo_point_buffer()

Calculates polygon that contains all points within the given radius of the point on Earth.

## Syntax

`geo_point_buffer(`*longitude*`,` *latitude*`,` *radius*`,` *tolerance*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *longitude* | real | &check; | Geospatial coordinate longitude value in degrees. Valid value is a real number and in the range [-180, +180].|
| *latitude* | real | &check; | Geospatial coordinate latitude value in degrees. Valid value is a real number and in the range [-90, +90].|
| *radius* | real | &check; | Buffer radius in meters. Valid value must be positive.|
| *tolerance* | real || Defines the tolerance in meters that determines how much a polygon can deviate from the ideal radius. If unspecified, the default value `10` is used. Tolerance should be no lower than 0.0001% of the radius. Specifying tolerance bigger than radius will lower the tolerance to biggest possible value below the radius.|

## Returns

Polygon around the input point. If the coordinates or radius or tolerance is invalid, the query will produce a null result.

> [!NOTE]
>
>* The geospatial coordinates are interpreted as represented by the [WGS-84](https://earth-info.nga.mil/index.php?dir=wgs84&action=wgs84) coordinate reference system.
>* The [geodetic datum](https://en.wikipedia.org/wiki/Geodetic_datum) used to measure distance on Earth is a sphere.

## Examples

The following query calculates polygon around [-115.1745008278, 36.1497251277] coordinates, with 20km radius.

```kusto
print buffer = geo_point_buffer(-115.1745008278, 36.1497251277, 20000)
```

|buffer|
|---|
|{"type": "Polygon","coordinates": [ ... ]}|

The following query calculates buffer around each point and unifies result

```kusto
datatable(longitude:real, latitude:real, radius:real)
[
    real(-80.3212217992616), 25.268683367546604, 5000,
    real(-80.81717403605833), 24.82658441221962, 3000
]
| project buffer = geo_point_buffer(longitude, latitude, radius)
| summarize polygons = make_list(buffer)
| project result = geo_union_polygons_array(polygons)
```

|result|
|---|
|{"type": "MultiPolygon","coordinates": [ ... ]}|

The following example will return true, due to invalid point.

```kusto
print result = isnull(geo_point_buffer(200, 1,0.1))
```

|result|
|---|
|True|

The following example will return true, due to invalid radius.

```kusto
print result = isnull(geo_point_buffer(10, 10, -1))
```

|result|
|---|
|True|
