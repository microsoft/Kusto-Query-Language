# geo_distance_2points()

Calculates the distance between 2 geospatial coordinates on Earth.

**Syntax**

`geo_distance_2points(`*p1_longitude*`, `*p1_latitude*`, `*p2_longitude*`, `*p2_latitude*`,)`

**Arguments**

* *p1_longitude*: 1st geospatial coordinate longitude value in degrees. Valid value is a real number and in range [-180, +180].
* *p1_latitude*: 1st geospatial coordinate latitude value in degrees. Valid value is a real number and in range [-90, +90].
* *p2_longitude*: 2nd geospatial coordinate longitude value in degrees. Valid value is a real number and in range [-180, +180].
* *p2_latitude*: 2nd geospatial coordinate latitude value in degrees. Valid value is a real number and in range [-90, +90].

**Returns**

The distance in meters between 2 geographic locations on Earth.


> [!NOTE]
> Geospatial coordinates are interpreted as represented per the [WGS-84](https://earth-info.nga.mil/GandG/update/index.php?action=home) reference system which is the most popular coordinate reference system today.
> The [geodetic datum](https://en.wikipedia.org/wiki/Geodetic_datum) that is being used in order to measure distance on Earth is sphere.

**Examples**

The following example finds distance between Seattle and Los Angels.

<!-- csl: https://help.kusto.windows.net/Samples -->
```
print distance_in_meters = geo_distance_2points(-122.407628, 47.578557, -118.275287, 34.019056)
```

|distance_in_meters|
|---|
|1546754.35197381|
