---
title: geo_distance_point_to_line() - Azure Data Explorer
description: This article describes geo_distance_point_to_line() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mbrichko
ms.service: data-explorer
ms.topic: reference
ms.date: 03/11/2020
---
# geo_distance_point_to_line()

Calculates the shortest distance between a coordinate and a line on Earth.

**Syntax**

`geo_distance_point_to_line(`*longitude*`, `*latitude*`, `*lineString*`)`

**Arguments**

* *longitude*: Geospatial coordinate longitude value in degrees. Valid value is a real number and in the range [-180, +180].
* *latitude*: Geospatial coordinate latitude value in degrees. Valid value is a real number and in the range [-90, +90].
* *lineString*: Line in the [GeoJSON format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type.

**Returns**

The shortest distance, in meters, between a coordinate and a line on Earth. If the coordinate or lineString are invalid, the query will produce a null result.

> [!NOTE]
> * The geospatial coordinates are interpreted as represented by the [WGS-84](https://earth-info.nga.mil/GandG/update/index.php?action=home) coordinate reference system.
> * The [geodetic datum](https://en.wikipedia.org/wiki/Geodetic_datum) used to measure distance on Earth is a sphere. Line edges are [geodesics](https://en.wikipedia.org/wiki/Geodesic) on the sphere.
> * If input line edges are straight cartesian lines, consider using [geo_line_densify()](geo-line-densify-function.md) in order to convert planar edges to geodesics.

**LineString definition and constraints**

dynamic({"type": "LineString","coordinates": [ [lng_1,lat_1], [lng_2,lat_2] ,..., [lng_N,lat_N] ]})

* LineString coordinates array must contain at least two entries.
* Coordinates [longitude,latitude] must be valid where longitude is a real number in the range [-180, +180] and latitude is a real number in the range [-90, +90].
* Edge length must be less than 180 degrees. The shortest edge between the two vertices will be chosen.

> [!TIP]
> For better performance, use literal lines.

**Examples**

The following example finds the shortest distance between North Las Vegas Airport and a nearby road.

:::image type="content" source="images/geo-distance-point-to-line-function/distance-point-to-line.png" alt-text="Distance between North Las Vegas Airport and road":::

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print distance_in_meters = geo_distance_point_to_line(-115.199625, 36.210419, dynamic({ "type":"LineString","coordinates":[[-115.115385,36.229195],[-115.136995,36.200366],[-115.140252,36.192470],[-115.143558,36.188523],[-115.144076,36.181954],[-115.154662,36.174483],[-115.166431,36.176388],[-115.183289,36.175007],[-115.192612,36.176736],[-115.202485,36.173439],[-115.225355,36.174365]]}))
```

| distance_in_meters |
|--------------------|
| 3797.88887253334   |

Storm events in south coast US. The events are filtered by a maximum distance of 5 km from the defined shore line.

:::image type="content" source="images/geo-distance-point-to-line-function/us-south-coast-storm-events.png" alt-text="Storm events in the US south coast":::

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let southCoast = dynamic({"type":"LineString","coordinates":[[-97.18505859374999,25.997549919572112],[-97.58056640625,26.96124577052697],[-97.119140625,27.955591004642553],[-94.04296874999999,29.726222319395504],[-92.98828125,29.82158272057499],[-89.18701171875,29.11377539511439],[-89.384765625,30.315987718557867],[-87.5830078125,30.221101852485987],[-86.484375,30.4297295750316],[-85.1220703125,29.6880527498568],[-84.00146484374999,30.14512718337613],[-82.6611328125,28.806173508854776],[-82.81494140625,28.033197847676377],[-82.177734375,26.52956523826758],[-80.9912109375,25.20494115356912]]});
StormEvents
| project BeginLon, BeginLat, EventType
| where geo_distance_point_to_line(BeginLon, BeginLat, southCoast) < 5000
| render scatterchart with (kind=map) // map rendering available in Kusto Explorer desktop
```

NY taxi pickups. Pickups are filtered by maximum distance of 0.1 m from the defined line.

:::image type="content" source="images/geo-distance-point-to-line-function/park-ave-ny-road.png" alt-text="Storm events in US south coast":::

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
nyc_taxi
| project pickup_longitude, pickup_latitude
| where geo_distance_point_to_line(pickup_longitude, pickup_latitude, dynamic({"type":"LineString","coordinates":[[-73.97583961486816,40.75486445336327],[-73.95215034484863,40.78743027428638]]})) <= 0.1
| take 50
| render scatterchart with (kind=map) // map rendering available in Kusto Explorer desktop
```

The following example will return a null result because of the invalid LineString input.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print distance_in_meters = geo_distance_point_to_line(1,1, dynamic({ "type":"LineString"}))
```

| distance_in_meters |
|--------------------|
|                    |

The following example will return a null result because of the invalid coordinate input.

```kusto
print distance_in_meters = geo_distance_point_to_line(300, 3, dynamic({ "type":"LineString","coordinates":[[1,1],[2,2]]}))
```

| distance_in_meters |
|--------------------|
|                    |
