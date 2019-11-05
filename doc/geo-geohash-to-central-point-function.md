# geo_geohash_to_central_point()

Calculates the geospatial coordinates that represent the center of a Geohash rectangular area. For more information about Geohash, click [here](https://en.wikipedia.org/wiki/Geohash).  

**Syntax**

`geo_geohash_to_central_point(`*geohash*`)`

**Arguments**

*geohash*: Geohash value for a geographic location. The geohash string can be 1 to 18 characters.

**Returns**

The geospatial coordinate values in [GeoJSON Format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type. If the geohash is invalid, the query will produce a null result.

> [!NOTE]
> The GeoJSON format specifies longitude first and latitude second.

**Examples**

<!-- csl: https://help.kusto.windows.net/Samples -->
```
print point = geo_geohash_to_central_point("sunny")
| extend coordinates = point.coordinates
| extend longitude = coordinates[0]
| extend latitude = coordinates[1]
```

|point|coordinates|longitude|latitude|
|---|---|---|---|
|{<br>  "type": "Point",<br>  "coordinates": [<br>    42.47314453125,<br>    23.70849609375<br>  ]<br>}|[<br>  42.47314453125,<br>  23.70849609375<br>]|42.47314453125|23.70849609375|

The following example will return a null result because of the invalid geohash input.

<!-- csl: https://help.kusto.windows.net/Samples -->
```
print geohash = geo_geohash_to_central_point("a")
```

|geohash|
|---|
||
