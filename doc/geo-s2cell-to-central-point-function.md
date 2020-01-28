# geo_s2cell_to_central_point()

Calculates the geospatial coordinates that represent the center of S2 Cell.

For more information about S2 Cells, click [here](http://s2geometry.io/devguide/s2cell_hierarchy).

**Syntax**

`geo_s2cell_to_central_point(`*s2cell*`)`

**Arguments**

*s2cell*: S2 Cell Token string value as it was calculated by [geo_point_to_s2cell()](geo-point-to-s2cell-function.md). The S2 Cell token maximum string length is 16 characters.

**Returns**

The geospatial coordinate values in [GeoJSON Format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type. If the S2 cell token is invalid, the query will produce a null result.

> [!NOTE]
> The GeoJSON format specifies longitude first and latitude second.

**Examples**

<!-- csl: https://help.kusto.windows.net/Samples -->
```
print point = geo_s2cell_to_central_point("1234567")
| extend coordinates = point.coordinates
| extend longitude = coordinates[0], latitude = coordinates[1]
```

|point|coordinates|longitude|latitude|
|---|---|---|---|
|{<br>  "type": "Point",<br>  "coordinates": [<br>    9.86830731850408,<br>    27.468392925827604<br>  ]<br>}|[<br>  9.86830731850408,<br>  27.468392925827604<br>]|9.86830731850408|27.4683929258276|

The following example returns a null result because of the invalid S2 cell token input.
<!-- csl: https://help.kusto.windows.net/Samples -->
```
print point = geo_s2cell_to_central_point("a")
```

|point|
|---|
||
