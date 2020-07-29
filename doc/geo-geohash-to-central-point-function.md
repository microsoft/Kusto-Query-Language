---
title: geo_geohash_to_central_point() - Azure Data Explorer
description: This article describes geo_geohash_to_central_point() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mbrichko
ms.service: data-explorer
ms.topic: reference
ms.date: 01/27/2020
---
# geo_geohash_to_central_point()

Calculates the geospatial coordinates that represent the center of a geohash rectangular area.

Read more about [`geohash`](https://en.wikipedia.org/wiki/Geohash).  

## Syntax

`geo_geohash_to_central_point(`*geohash*`)`

## Arguments

*geohash*: Geohash string value as it was calculated by [geo_point_to_geohash()](geo-point-to-geohash-function.md). The geohash string can be 1 to 18 characters.

## Returns

The geospatial coordinate values in [GeoJSON Format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type. If the geohash is invalid, the query will produce a null result.

> [!NOTE]
> The GeoJSON format specifies longitude first and latitude second.

## Examples

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print point = geo_geohash_to_central_point("sunny")
| extend coordinates = point.coordinates
| extend longitude = coordinates[0], latitude = coordinates[1]
```

|point|coordinates|longitude|latitude|
|---|---|---|---|
|{<br>  "type": "Point",<br>  "coordinates": [<br>    42.47314453125,<br>    23.70849609375<br>  ]<br>}|[<br>  42.47314453125,<br>  23.70849609375<br>]|42.47314453125|23.70849609375|

The following example returns a null result because of the invalid geohash input.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print geohash = geo_geohash_to_central_point("a")
```

|geohash|
|---|
||

## Example: Creating location deep-links for Bing Maps

You can use the geohash value to create a deep-link URL to Bing Maps by pointing to the geohash center point:

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
// Use string concatenation to create Bing Map deep-link URL from a geo-point
let point_to_map_url = (_point:dynamic, _title:string) 
{
    strcat('https://www.bing.com/maps?sp=point.', _point.coordinates[1] ,'_', _point.coordinates[0], '_', url_encode(_title)) 
};
// Convert geohash to center point, and then use 'point_to_map_url' to create Bing Map deep-link
let geohash_to_map_url = (_geohash:string, _title:string)
{
    point_to_map_url(geo_geohash_to_central_point(_geohash), _title)
};
print geohash = 'sv8wzvy7'
| extend url = geohash_to_map_url(geohash, "You are here")
```

|geohash|url|
|---|---|
|sv8wzvy7|[https://www.bing.com/maps?sp=point.32.15620994567871_34.80245590209961_You+are+here](https://www.bing.com/maps?sp=point.32.15620994567871_34.80245590209961_You+are+here)|
