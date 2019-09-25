# geo_point_to_geohash()

Calculates the Geohash string value for a geographic location.

More information on Geohash can be found [here](https://en.wikipedia.org/wiki/Geohash).

**Syntax**

`geo_point_to_geohash(`*longitude*`, `*latitude*`, `[*Accuracy*]`)`

**Arguments**

* *longitude*: Longitude value of a geographic location. Longitude x will be considered valid if x is a real number and x in range [-180, +180]. 
* *Latitude*: Latitude value of a geographic location. Latitude y will be considered valid if y is a real number and y in range [-90, +90]. 
* *Accuracy*: An optional `int` literal that defines the requested accuracy. Supported values are in range [1,18]. If unspecified, the default value `5` is used.

**Returns**

The Geohash string value of a given geographic location with requested accuracy length.


> [!NOTE]
> Invoking [geo_geohash_to_central_point()](geo-geohash-to-central-point-function.md) function on geohash string that was calculated on some longitude x and latitude y won't necessairly return x and y.

**Examples**

<!-- csl: https://help.kusto.windows.net/Samples -->
```
print geohash = geo_point_to_geohash(139.806115, 35.554128, 12)  
```

|geohash|
|---|
|xn76m27ty9g4|

<!-- csl: https://help.kusto.windows.net/Samples -->
```
print geohash = geo_point_to_geohash(-80.195829, 25.802215, 8)
```

|geohash|
|---|
|dhwfz15h|

The following example finds groups of coordinates. Every pair of coordinates in the group are no further than 1.22km from each other.

<!-- csl: https://help.kusto.windows.net/Samples -->
```
datatable(location_id:string, longitude:real, latitude:real)
[
  "A", double(-122.303404), 47.570482,
  "B", double(-122.304745), 47.567052,
  "C", double(-122.278156), 47.566936,
]
| summarize count = count(),                                          // items per group count
            locations = make_list(location_id)                        // items in group
         by geohash = geo_point_to_geohash(longitude, latitude, 6)    // geohash of the group
```

|geohash|count|locations|
|---|---|---|
|c23n8g|2|[<br>  "A",<br>  "B"<br>]|
|c23n97|1|[<br>  "C"<br>]|


> [!NOTE]
> Due to Geohash definition it's possible that 2 geographic locations are very close to each other but they have different Geohash codes.
