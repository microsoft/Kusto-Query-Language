---
title:  geo_geohash_to_central_point()
description: Learn how to use the geo_geohash_to_central() function to calculate the geospatial coordinates that represent the center of a geohash rectangular area.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 03/09/2023
---
# geo_geohash_to_central_point()

Calculates the geospatial coordinates that represent the center of a geohash rectangular area.

Read more about [`geohash`](https://en.wikipedia.org/wiki/Geohash).  

## Syntax

`geo_geohash_to_central_point(`*geohash*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *geohash* | string | &check; | A geohash value as it was calculated by [geo_point_to_geohash()](geo-point-to-geohash-function.md). The geohash string must be between 1 and 18 characters.|

## Returns

The geospatial coordinate values in [GeoJSON Format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type. If the geohash is invalid, the query will produce a null result.

> [!NOTE]
> The GeoJSON format specifies longitude first and latitude second.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUSjIB5G2Cump+fFAnJFYnBFfkh+fnJpXUpSYEw+W1lAqLs3Lq1TS5KpRSK0oSc1LUUjOzy9KycxLLEktBmoGq9JDEkMozMnPS88sKU1JBSpDUhBtEKujkJNYgk3KMBYAnhfZ4psAAAA=" target="_blank">Run the query</a>

```kusto
print point = geo_geohash_to_central_point("sunny")
| extend coordinates = point.coordinates
| extend longitude = coordinates[0], latitude = coordinates[1]
```

**Output**

|point|coordinates|longitude|latitude|
|---|---|---|---|
|{<br>  "type": "Point",<br>  "coordinates": [<br>    42.47314453125,<br>    23.70849609375<br>  ]<br>}|[<br>  42.47314453125,<br>  23.70849609375<br>]|42.47314453125|23.70849609375|

The following example returns a null result because of the invalid geohash input.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUhPzc9ILM5QsAWx4qG8+JL8+OTUvJKixJz4gnygMg2lRCVNAEhNnjMxAAAA" target="_blank">Run the query</a>

```kusto
print geohash = geo_geohash_to_central_point("a")
```

**Output**

|geohash|
|---|
||

### Creating location deep-links for Bing Maps

You can use the geohash value to create a deep-link URL to Bing Maps by pointing to the geohash center point:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA32RS0/DMBCE7/kVo16cSGkNJ1BRhQRXuCD1gFBlGWdpLBI7st2G8vjvOA+gCgif7F17duYz51h7gg9Omy2UNUoGMjJoaxAslKN4xlXXvJUNCqJmXmnzjPXdDZ6crSGxJTtvrDYhqSig34lgRS0bsXMVVkhFX1wWByNrrXKIoENFy2FqhuQtQVzxGKenrAyh8UvO27ZdPMYLC2VrHtX8pW9WvdKCRY1hp6x1hY6OyT+cbpAz8XfvZJOj70VLgoyyBaWDjSwa+LhIOMe1NXtyoQtUSl/2AMgEckOoHNIUCCUZ7CIzNk3K/iXWwxmVp3jG8ghkymfEMx2XxlfiSLCz6mQ1ZP/WzL7Usi5kEwV/8q3A/P68fd0fzljyDnqJX19gMPXbaTqWcszu7Q7SEUpyNMs+AXziAcBEAgAA" target="_blank">Run the query</a>

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

**Output**

|geohash|url|
|---|---|
|sv8wzvy7|[https://www.bing.com/maps?sp=point.32.15620994567871_34.80245590209961_You+are+here](https://www.bing.com/maps?sp=point.32.15620994567871_34.80245590209961_You+are+here)|
