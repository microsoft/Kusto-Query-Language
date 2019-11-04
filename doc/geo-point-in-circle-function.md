# geo_point_in_circle()

Calculates whether the geospatial coordinates are inside a circle on Earth.

**Syntax**

`geo_point_in_circle(`*p_longitude*`, `*p_latitude*`, `*pc_longitude*`, `*pc_latitude*`, `*c_radius*`)`

**Arguments**

* *p_longitude*: geospatial coordinate longitude value in degrees. Valid value is a real number and in range [-180, +180].
* *p_latitude*: geospatial coordinate latitude value in degrees. Valid value is a real number and in range [-90, +90].
* *pc_longitude*: circle center geospatial coordinate longitude value in degrees. Valid value is a real number and in range [-180, +180].
* *pc_latitude*: circle center geospatial coordinate latitude value in degrees. Valid value is a real number and in range [-90, +90].
* *c_radius*: circle radius in meters. Valid value must be positive.

**Returns**

Indication whether the geospatial coordinates are inside a circle. In case of invalid coordinates\circle the query will produce null result.


**Notes**
* Geospatial coordinates are interpreted as represented per the [WGS-84](https://earth-info.nga.mil/GandG/update/index.php?action=home) reference system which is the most popular coordinate reference system today.
* The [geodetic datum](https://en.wikipedia.org/wiki/Geodetic_datum) that is being used in order to measure distance on Earth is sphere and circle is a spherical cap on Earth.

**Examples**

The following query finds all places in the area defined by a circle whose center is at [-122.317404, 47.609119] coordinate and radius of 18km.
![alt text](./images/queries/geo/circle_seattle.png)

<!-- csl: https://mbrichkokustocluster.westeurope.dev.kusto.windows.net/tesddb1 -->
```
datatable(longitude:real, latitude:real, place:string)
[
    real(-122.317404), 47.609119, 'Seattle',                   // In circle 
    real(-123.497688), 47.458098, 'Olympic National Forest',   // In exterior of circle  
    real(-122.201741), 47.677084, 'Kirkland',                  // In circle
    real(-122.443663), 47.247092, 'Tacoma',                    // In exterior of circle
    real(-122.121975), 47.671345, 'Redmond',                   // In circle
]
| where geo_point_in_circle(longitude, latitude, -122.317404, 47.609119, 18000) > 0
| project place
```

|place|
|---|
|Seattle|
|Kirkland|
|Redmond|

The following example will return true.
<!-- csl: https://help.kusto.windows.net/Samples -->
```
print in_circle = geo_point_in_circle(-122.143564, 47.535677, -122.100896, 47.527351, 3500)
```

|in_circle|
|---|
|1|

The following example will return false.
<!-- csl: https://help.kusto.windows.net/Samples -->
```
print in_circle = geo_point_in_circle(-122.137575, 47.630683, -122.100896, 47.527351, 3500)
```

|in_circle|
|---|
|0|

The following example will return null result because of the bad input (invalid coordinate).
<!-- csl: https://help.kusto.windows.net/Samples -->
```
print in_circle = geo_point_in_circle(200, 1, 1, 1, 1)
```

|in_circle|
|---|
||

The following example will return null result because of the bad input (invalid circle radius).
<!-- csl: https://help.kusto.windows.net/Samples -->
```
print in_circle = geo_point_in_circle(1, 1, 1, 1, -1)
```

|in_circle|
|---|
||
