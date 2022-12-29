---
title: geo_distance_2points() - Azure Data Explorer
description: Learn how to use the geo_distance_2points() function to calculate the shortest distance between two geospatial coordinates on Earth.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 12/18/2022
---
# geo_distance_2points()

Calculates the shortest distance in meters between two geospatial coordinates on Earth.

## Syntax

`geo_distance_2points(`*p1_longitude*`,`*p1_latitude*`,`*p2_longitude*`,`*p2_latitude*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*p1_longitude*| real | &check; | The longitude value in degrees of the first geospatial coordinate. A valid value is in the range [-180, +180].|
|*p1_latitude*| real | &check; | The latitude value in degrees of the first geospatial coordinate. A valid value is in the range [-90, +90].|
|*p2_longitude*| real | &check; | The longitude value in degrees of the second geospatial coordinate. A valid value is in the range [-180, +180].|
|*p2_latitude*| real | &check; | The latitude value in degrees of the second geospatial coordinate. A valid value is in the range [-90, +90].|

## Returns

The shortest distance, in meters, between two geographic locations on Earth. If the coordinates are invalid, the query will produce a null result.

> [!NOTE]
>
> * The geospatial coordinates are interpreted as represented by the [WGS-84](https://earth-info.nga.mil/GandG/update/index.php?action=home) coordinate reference system.
> * The [geodetic datum](https://en.wikipedia.org/wiki/Geodetic_datum) used to measure distance on Earth is a sphere.

## Examples

The following example finds the shortest distance between Seattle and Los Angeles.

:::image type="content" source="images/geo-distance-2points-function/distance_2points_seattle_los_angeles.png" alt-text="Distance between Seattle and Los Angeles.":::

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAz3KQQqAIBBA0avMskBFJ8exRWeRKAkXqaj3J1ctP+/XlvKAO/Vx5iuGlMMbR2wdDnhiCT9gLXPsizSIymp26AVYVsSeiAVIY7xCJvQzNqu02TW59QMCmjc+XwAAAA==" target="_blank">Run the query</a>

```kusto
print distance_in_meters = geo_distance_2points(-122.407628, 47.578557, -118.275287, 34.019056)
```

**Output**

| distance_in_meters |
|--------------------|
| 1546754.35197381   |

Here's an approximation of shortest path from Seattle to London. The line consists of coordinates along the LineString and within 500 meters from it.

:::image type="content" source="images/geo-distance-2points-function/line_seattle_london.png" alt-text="Seattle to London LineString.":::

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA03Oz2rDMAwG8PueQvhkD684paW0rG+w244hBM/WEm+JHBRBKNvefU5gf3T9vp8k9tQhJHjlPEIFkqFy28AsOEF19wkT5zcMAgN1cAX2FLWBe2D0g36o9ntjYfDyPzq7wpYeGTf0grIgkv4TsNuBM1D6G/0pHE5rcHbml3eY25hm8RSwnXIiaSW3QyLUZbMt2MYb+TEF/aHkNqG6qKeSPgsn6pRVIWeOibzgrC51vV63h1Nja2ePVdN8GQOPcHTrw4wUkWEOXgQ59J4FliQ96PdE8Tr6yXwDBC8v7i4BAAA=" target="_blank">Run the query</a>

```kusto
range i from 1 to 1000000 step 1
| project lng = rand() * real(-122), lat = rand() * 90
| where lng between(real(-122) .. 0) and lat between(47 .. 90)
| where geo_distance_point_to_line(lng,lat,dynamic({"type":"LineString","coordinates":[[-122,47],[0,51]]})) < 500
| render scatterchart with (kind=map)
```

The following example finds all rows in which the shortest distance between two coordinates is between 1 meter and 11 meters.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA22MsQrCQBBEe79iygghsPY2Qjo7P+A4c0M8IbvhspgUfryJgo1WMwxv3sWtDO2D6tPuCS5OTUh58qgdgwS3IDLgiJ4WvvthtLw+qhP7rGfTGp8WvUar6b1sGX2/WucbC/9Ir/SZVFSCpoHIxo7F7uz8l34BxwMseakAAAA=" target="_blank">Run the query</a>

```kusto
StormEvents
| extend distance_1_to_11m = geo_distance_2points(BeginLon, BeginLat, EndLon, EndLat)
| where distance_1_to_11m between (1 .. 11)
| project distance_1_to_11m
```

**Output**

| distance_1_to_11m |
|-------------------|
| 10.5723100154958  |
| 7.92153588248414  |

The following example returns a null result because of the invalid coordinate input.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUjJLC5JzEtOVbBVSE/Nj4dx440K8oHSxRrGBgY6hiCoCQDd7v6oMAAAAA==" target="_blank">Run the query</a>

```kusto
print distance = geo_distance_2points(300,1,1,1)
```

**Output**

| distance |
|----------|
|          |
