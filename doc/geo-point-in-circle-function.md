---
title: geo_point_in_circle() - Azure Data Explorer
description: Learn how to use the geo_point_in_circle() function to check if the geospatial coordinates are inside a circle on Earth.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 03/09/2023
---
# geo_point_in_circle()

Calculates whether the geospatial coordinates are inside a circle on Earth.

## Syntax

`geo_point_in_circle(`*p_longitude*`,` *p_latitude*`,` *pc_longitude*`,` *pc_latitude*`,` *c_radius*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *p_longitude* | real | &check; | Geospatial coordinate longitude value in degrees. Valid value is a real number and in the range [-180, +180].|
| *p_latitude* | real | &check; | Geospatial coordinate latitude value in degrees. Valid value is a real number and in the range [-90, +90].|
| *pc_longitude* | real | &check; | Circle center geospatial coordinate longitude value in degrees. Valid value is a real number and in the range [-180, +180].|
| *pc_latitude* | real | &check; | circle center geospatial coordinate latitude value in degrees. Valid value is a real number and in the range [-90, +90].|
| *c_radius* | real | &check; | Circle radius in meters. Valid value must be positive.|

## Returns

Indicates whether the geospatial coordinates are inside a circle. If the coordinates or circle is invalid, the query will produce a null result.

> [!NOTE]
>
>* The geospatial coordinates are interpreted as represented by the [WGS-84](https://earth-info.nga.mil/index.php?dir=wgs84&action=wgs84) coordinate reference system.
>* The [geodetic datum](https://en.wikipedia.org/wiki/Geodetic_datum) used to measure distance on Earth is a sphere.
>* A circle is a spherical cap on Earth. The radius of the cap is measured along the surface of the sphere.

## Examples

The following query finds all the places in the area defined by the following circle: Radius of 18 km, center at [-122.317404, 47.609119] coordinates.

:::image type="content" source="images/geo-point-in-circle-function/circle-seattle.png" alt-text="Places near Seattle.":::

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA3XS0UrDMBSA4fs+xbnbCrVL2rRp9wCCCArqnUiJ6bFG06RkERV8eKN1rN1qchUCH384aYUP+1HjWlvTKf/W4tah0Alo4afHQQuJ2513ynRxdB9BWD836zOaZWlOOSMsToDxtCQ1pXUCq1sU3mtcJXC6Nhu4MCCVkxphhuUpq3lZVSPGiorUVcCu9Wc/KAlXIcsaoeHcOtz5X3zE8MOjU9aBfdrDcJSZkdBJ/zI5JxUL8qVyr1qYdqlzmnlkMZaXZT5aGeOkzoJ1J6TtxeKL/608cmlGa17sG2nOiuDeYNvb5cR540P0Be/P6BA6tM1glfGNMs14e5jxYbwJTAY4mx+tCCFx8AZnX1D68Qt8AzYb0g4xAgAA" target="_blank">Run the query</a>

```kusto
datatable(longitude:real, latitude:real, place:string)
[
    real(-122.317404), 47.609119, 'Seattle',                   // In circle 
    real(-123.497688), 47.458098, 'Olympic National Forest',   // In exterior of circle  
    real(-122.201741), 47.677084, 'Kirkland',                  // In circle
    real(-122.443663), 47.247092, 'Tacoma',                    // In exterior of circle
    real(-122.121975), 47.671345, 'Redmond',                   // In circle
]
| where geo_point_in_circle(longitude, latitude, -122.317404, 47.609119, 18000)
| project place
```

**Output**

|place|
|---|
|Seattle|
|Kirkland|
|Redmond|

Storm events in Orlando. The events are filtered by 100 km within Orlando coordinates, and aggregated by event type and hash.

:::image type="content" source="images/geo-point-in-circle-function/orlando-storm-events.png" alt-text="Storm events in Orlando.":::

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA22PwU7DMAyG7zyFjw0KW7sBKoddkHaCG9yrLLVaszSJHHdjiIcn3SbRw0625P//PvlDAg/bA3pJd78QOXyhFXjFjvx78Pq6GdFwDn2eIubcsUdG6DA0MZCXhnxjia3D4laT0bjioa4W6/qlUhpW9eJp/fisoSrLEu6noTI0jcNgmH4QbBi9FAp2p3+rht6kHjYzq4QmrSw6d8OqZt9MjUtwqtgMZOMujGKCKj3XnOVNrjP6Fhkioe0NCxxJeij25NvNYKKC5RLynO7XKPkOzMGQMzuHQB7exiQBtt/RBc6kFtNeQvwDFpFHo3MBAAA=" target="_blank">Run the query</a>

```kusto
StormEvents
| project BeginLon, BeginLat, EventType
| where geo_point_in_circle(BeginLon, BeginLat, real(-81.3891), 28.5346, 1000 * 100)
| summarize count() by EventType, hash = geo_point_to_s2cell(BeginLon, BeginLat)
| project geo_s2cell_to_central_point(hash), EventType, count_
| render piechart with (kind=map) // map pie rendering available in Kusto Explorer desktop
```

The following example shows NY Taxi pickups within 10 meters of a particular location. Relevant pickups are aggregated by hash.

:::image type="content" source="images/geo-point-in-circle-function/circle-junction.png" alt-text="NY Taxi nearby Pickups.":::

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA42Py27CQAxF93yFlxMpRTBFgiz4lpFxLMZlMjNyHPEQH9+kSBW7dnt17zl2vlMwvMnqCVXLF5NBFbpMNaSSz2JTz+1vgvYTzN1rZGU4cwm1SLYgOZAoJXZ/z1tQxuQ+9p/rrvOHpoXdZr3f+a6F7aaZ4eM0DKjyYDjdIeIY4fimshJGT5yS+4fJ++btswXy2i4U4myK6YV1i2fpKueeFUZCM1aKqAZXsQjuIrmfLxmwNt9Em1nwNgEAAA==" target="_blank">Run the query</a>

```kusto
nyc_taxi
| project pickup_longitude, pickup_latitude
| where geo_point_in_circle( pickup_longitude, pickup_latitude, real(-73.9928), 40.7429, 10)
| summarize by hash = geo_point_to_s2cell(pickup_longitude, pickup_latitude, 22)
| project geo_s2cell_to_central_point(hash)
| render scatterchart with (kind = map)
```

The following example will return true.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUcjMi0/OLErOSVWwVUhPzY8vyAeKxsNFNXQNjYz0DE2MTc1MdBRMzPVMgSxzcx0FiLiBgYWlGUTcyNzY1FBHwdjUwEATAJnKnMpbAAAA" target="_blank">Run the query</a>

```kusto
print in_circle = geo_point_in_circle(-122.143564, 47.535677, -122.100896, 47.527351, 3500)
```

**Output**

|in_circle|
|---|
|1|

The following example will return false.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUcjMi0/OLErOSVWwVUhPzY8vyAeKxsNFNXQNjYz0DI3NTc1NdRRMzPXMjA3MLIx1FCDiBgYWlmZgcVMjc2NTQx0FY1MDA00AV0g5xlsAAAA=" target="_blank">Run the query</a>

```kusto
print in_circle = geo_point_in_circle(-122.137575, 47.630683, -122.100896, 47.527351, 3500)
```

**Output**

|in_circle|
|---|
|0|

The following example will return a null result because of the invalid coordinate input.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUcjMi0/OLErOSVWwVUhPzY8vyAeKxsNFNYwMDHQUDOFIEwAH8KEmNgAAAA==" target="_blank">Run the query</a>

```kusto
print in_circle = geo_point_in_circle(200, 1, 1, 1, 1)
```

**Output**

|in_circle|
|---|
||

The following example will return a  null result because of the invalid circle radius input.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUcjMi0/OLErOSVWwVUhPzY8vyAeKxsNFNQx1FOBI11ATAAIUtdY1AAAA" target="_blank">Run the query</a>

```kusto
print in_circle = geo_point_in_circle(1, 1, 1, 1, -1)
```

**Output**

|in_circle|
|---|
||
