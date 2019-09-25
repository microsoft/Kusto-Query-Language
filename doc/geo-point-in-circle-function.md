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

Indication whether the geospatial coordinates are inside a circle.


> [!NOTE]
> Geospatial coordinates are interpreted as represented per the [WGS-84](https://earth-info.nga.mil/GandG/update/index.php?action=home) reference system which is the most popular coordinate reference system today.
> The [geodetic datum](https://en.wikipedia.org/wiki/Geodetic_datum) that is being used in order to measure distance on Earth is sphere and circle is a spherical cap on Earth.

**Examples**

<!-- csl: https://help.kusto.windows.net/Samples -->
```
print in_circle = geo_point_in_circle(-122.143564, 47.535677, -122.100896, 47.527351, 3500) // true
```

|in_circle|
|---|
|1|

<!-- csl: https://help.kusto.windows.net/Samples -->
```
print in_circle = geo_point_in_circle(-122.137575, 47.630683, -122.100896, 47.527351, 3500) // false
```

|in_circle|
|---|
|0|
