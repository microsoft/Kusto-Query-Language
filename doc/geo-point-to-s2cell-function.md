---
title:  geo_point_to_s2cell()
description: Learn how to use the geo_point_to_s2cell() function to calculate the S2 cell token string value of a geographic location.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 03/09/2023
---
# geo_point_to_s2cell()

Calculates the S2 cell token string value of a geographic location.

S2 Cell can be a useful geospatial clustering tool. The S2 Cell is a cell on a spherical surface and its edges are geodesics. S2 Cell has 31 levels of hierarchy with area coverage ranging from 85,011,012.19km² at the highest level of 0 to 0.44 cm² at the lowest level of 30. S2 Cell preserves the cell center well during level increase from 0 to 30. Two geographic locations can be very close to each other but have different S2 cell tokens.

>[!NOTE]
> If you invoke the [geo_s2cell_to_central_point()](geo-s2cell-to-central-point-function.md) function on an S2 cell token string that was calculated on longitude x and latitude y, the function won't necessarily return x and y.

Read more about [S2 cell hierarchy](https://s2geometry.io/devguide/s2cell_hierarchy).

## Syntax

`geo_point_to_s2cell(`*longitude*`,` *latitude*`,` [ *level* ]`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *longitude* | real | &check; | Geospatial coordinate, longitude value in degrees. Valid value is a real number and in the range [-180, +180].|
| *latitude* | real | &check; | Geospatial coordinate, latitude value in degrees. Valid value is a real number and in the range [-90, +90].|
| *level* | int | | Defines the requested cell level. Supported values are in the range [0, 30]. If unspecified, the default value `11` is used.|

## Returns

The S2 cell token string value of a given geographic location. If the coordinates or levels are invalid, the query will produce an empty result.

## S2 Cell approximate area coverage per level value

For every level, the size of the S2 Cell is similar but not exactly equal. Nearby cell sizes tend to be more equal.

|Level|Minimum random cell edge length (UK)|Maximum random cell edge length (US)|
|--|--|--|
|0|7842 km|7842 km|
|1|3921 km|5004 km|
|2|1825 km|2489 km|
|3|840 km|1310 km|
|4|432 km|636 km|
|5|210 km|315 km|
|6|108 km|156 km|
|7|54 km|78 km|
|8|27 km|39 km|
|9|14 km|20 km|
|10|7 km|10 km|
|11|3 km|5 km|
|12|1699 m|2 km|
|13|850 m|1225 m|
|14|425 m|613 m|
|15|212 m|306 m|
|16|106 m|153 m|
|17|53 m|77 m|
|18|27 m|38 m|
|19|13 m|19 m|
|20|7 m|10 m|
|21|3 m|5 m|
|22|166 cm|2 m|
|23|83 cm|120 cm|
|24|41 cm|60 cm|
|25|21 cm|30 cm|
|26|10 cm|15 cm|
|27|5 cm|7 cm|
|28|2 cm|4 cm|
|29|12 mm|18 mm|
|30|6 mm|9 mm|

The table source can be found [in this S2 Cell statistical resource](https://s2geometry.io/resources/s2cell_statistics).

For comparison with other available grid systems, see [geospatial clustering with Kusto Query Language](geospatial-grid-systems.md).

## Examples

### US storm events aggregated by S2 Cell

:::image type="content" source="images/geo-point-to-s2cell-function/s2cell.png" alt-text="US s2cell.":::

```kusto
StormEvents
| project BeginLon, BeginLat
| summarize by hash=geo_point_to_s2cell(BeginLon, BeginLat, 5)
| project geo_s2cell_to_central_point(hash)
| render scatterchart with (kind=map)
```

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUSg2Sk7NyVGwVUhPzY8vyAcKxZfkx0NENXQtDPQMLU0tjCx1FIxM9SwMjIwMTXUULDQBug9pJTwAAAA=" target="_blank">Run the query</a>

```kusto
print s2cell = geo_point_to_s2cell(-80.195829, 25.802215, 8)
```

**Output**

| s2cell |
|--------|
| 88d9b  |

### Find a group of coordinates

The following example finds groups of coordinates. Every pair of coordinates in the group resides in the S2 cell with a maximum area of 1632.45 km².

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA42QzWrDMAyA73kK0VMCpj9J/1boYdtjlGLcRE3FHCvYymFjD1+3Zml32qyD0Yf0CakxEuNkMbdcGyF2mppdEE+uVWDZtSRDgzuPxsY8VoxpkR0ygMnrRMFiPl2U1VLBqlI39pZYtdq8jOw9sfV2WSZ2zL4hDF1nPH0h1Dw4gX3680LBP99sBiTYBejRQ+t56JMiey762S3EAZ35QG0pyPPGxR9yciAXTP5f5tMnhLJGa6O5RdY9kxMtrBPNxws+jqdgW9zMoYR7I58f7itUpqIvjwEAAA==" target="_blank">Run the query</a>

```kusto
datatable(location_id:string, longitude:real, latitude:real)
[
  "A", 10.1234, 53,
  "B", 10.3579, 53,
  "C", 10.6842, 53,
]
| summarize count = count(),                                        // items per group count
            locations = make_list(location_id)                      // items in the group
            by s2cell = geo_point_to_s2cell(longitude, latitude, 8) // s2 cell of the group
```

**Output**

| s2cell | count | locations |
|--------|-------|-----------|
| 47b1d  | 2     | ["A","B"] |
| 47ae3  | 1     | ["C"]     |

### Empty results

The following example produces an empty result because of the invalid coordinate input.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUSg2Sk7NyVGwVUhPzY8vyAcKxZfkx0NENYwNDHQMdSw0AU8vTgcrAAAA" target="_blank">Run the query</a>

```kusto
print s2cell = geo_point_to_s2cell(300,1,8)
```

**Output**

| s2cell |
|--------|
|        |

The following example produces an empty result because of the invalid level input.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUSg2Sk7NyVGwVUhPzY8vyAcKxZfkx0NENQx1DHWMTTUB9oFeACoAAAA=" target="_blank">Run the query</a>

```kusto
print s2cell = geo_point_to_s2cell(1,1,35)
```

**Output**

| s2cell |
|--------|
|        |

The following example produces an empty result because of the invalid level input.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUSg2Sk7NyVGwVUhPzY8vyAcKxZfkx0NENQx1DHWAIhp5pTk5mpoAYUJIkTEAAAA=" target="_blank">Run the query</a>

```kusto
print s2cell = geo_point_to_s2cell(1,1,int(null))
```

**Output**

| s2cell |
|--------|
|        |

## See also

* [geo_point_to_geohash()](geo-point-to-geohash-function.md)
* [geo_point_to_h3cell()](geo-point-to-h3cell-function.md)
