# geo_point_to_geohash()

Calculates the Geohash string value for a geographic location.

For more information about Geohash, click [here](https://en.wikipedia.org/wiki/Geohash).  

**Syntax**

`geo_point_to_geohash(`*longitude*`, `*latitude*`, `[*Accuracy*]`)`

**Arguments**

* *longitude*: Longitude value of a geographic location. Longitude x will be considered valid if x is a real number and x is in range [-180, +180]. 
* *Latitude*: Latitude value of a geographic location. Latitude y will be considered valid if y is a real number and y in in range [-90, +90]. 
* *Accuracy*: An optional `int` that defines the requested accuracy. Supported values are in the range [1,18]. If unspecified, the default value `5` is used.

**Returns**

The Geohash string value of a given geographic location with requested accuracy length. If the coordinate or accuracy are invalid, the query will produce an empty result.


> [!NOTE]
>* Invoking the [geo_geohash_to_central_point()](geo-geohash-to-central-point-function.md) function on a geohash string that was calculated on longitude x and latitude y won't necessairly return x and y.
>* Due to the Geohash definition, it's possible that two geographic locations are very close to each other but have different Geohash codes.

**Geohash rectangular area coverage per accuracy value**

|Accuracy|Width|Height|
|---|---|--|
|1|5000 km|5000 km|
|2|1250 km|625 km|
|3|156.25 km|156.25 km|
|4|39.06 km|19.53 km|
|5|4.88 km|4.88 km|
|6|1.22 km|0.61 km|
|7|152.59 m|152.59 m|
|8|38.15 m|19.07 m|
|9|4.77 m|4.77 m|
|10|1.19 m| 0.59 m|
|11|149.01 mm|149.01 mm|
|12|37.25 mm|18.63 mm|
|13|4.66 mm|4.66 mm|
|14|1.16 mm|0.58 mm|
|15|145.52 Î¼|145.52 Î¼|
|16|36.28 Î¼|18.19 Î¼|
|17|4.55 Î¼|4.55 Î¼|
|18|1.14 Î¼|0.57 Î¼|

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
            locations = make_list(location_id)                        // items in the group
         by geohash = geo_point_to_geohash(longitude, latitude, 6)    // geohash of the group
```

|geohash|count|locations|
|---|---|---|
|c23n8g|2|[<br>  "A",<br>  "B"<br>]|
|c23n97|1|[<br>  "C"<br>]|

The following example produces an empty result because of the invalid coordinate input.
<!-- csl: https://help.kusto.windows.net/Samples -->
```
print geohash = geo_point_to_geohash(200,1,8)
```

|geohash|
|---|
||
