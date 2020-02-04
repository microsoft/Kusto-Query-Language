# geo_point_to_geohash()

Calculates the Geohash string value for a geographic location.

For more information about Geohash, see [here](https://en.wikipedia.org/wiki/Geohash).  

**Syntax**

`geo_point_to_geohash(`*longitude*`, `*latitude*`, `[*accuracy*]`)`

**Arguments**

* *longitude*: Longitude value of a geographic location. Longitude x will be considered valid if x is a real number and is in the range [-180, +180]. 
* *latitude*: Latitude value of a geographic location. Latitude y will be considered valid if y is a real number and y is in the range [-90, +90]. 
* *accuracy*: An optional `int` that defines the requested accuracy. Supported values are in the range [1,18]. If unspecified, the default value `5` is used.

**Returns**

The Geohash string value of a given geographic location with requested accuracy length. If the coordinate or accuracy are invalid, the query will produce an empty result.

> [!NOTE]
>
> * Geohash can be a useful geospatial clustering tool.
> * Geohash has 18 accuracy levels with area coverage ranging from 25 Million kmÂ² at the highest level 1 to 0.6 Î¼Â² at the lowest level 18.
> * Common prefix of Geohashes indicate on proximity of points to each other. The longer a shared prefix is, the closer the two places are. Accuracy value translates to geohash length.
> * Geohash is a rectangular area on a plane surface.
> * Invoking the [geo_geohash_to_central_point()](geo-geohash-to-central-point-function.md) function on a geohash string that was calculated on longitude x and latitude y won't necessarily return x and y.
> * Due to the Geohash definition, it's possible that two geographic locations are very close to each other but have different Geohash codes.

**Geohash rectangular area coverage per accuracy value:**

| Accuracy | Width     | Height    |
|----------|-----------|-----------|
| 1        | 5000 km   | 5000 km   |
| 2        | 1250 km   | 625 km    |
| 3        | 156.25 km | 156.25 km |
| 4        | 39.06 km  | 19.53 km  |
| 5        | 4.88 km   | 4.88 km   |
| 6        | 1.22 km   | 0.61 km   |
| 7        | 152.59 m  | 152.59 m  |
| 8        | 38.15 m   | 19.07 m   |
| 9        | 4.77 m    | 4.77 m    |
| 10       | 1.19 m    | 0.59 m    |
| 11       | 149.01 mm | 149.01 mm |
| 12       | 37.25 mm  | 18.63 mm  |
| 13       | 4.66 mm   | 4.66 mm   |
| 14       | 1.16 mm   | 0.58 mm   |
| 15       | 145.52 Î¼  | 145.52 Î¼  |
| 16       | 36.28 Î¼   | 18.19 Î¼   |
| 17       | 4.55 Î¼    | 4.55 Î¼    |
| 18       | 1.14 Î¼    | 0.57 Î¼    |

See also [geo_point_to_s2cell()](geo-point-to-s2cell-function.md).

**Examples**

US storm events aggregated by geohash.
![US Geohash](./images/queries/geo/geohash.png)
<!-- csl: https://help.kusto.windows.net/Samples -->
```
StormEvents
| project BeginLon, BeginLat
| summarize by hash=geo_point_to_geohash(BeginLon, BeginLat, 3)
| project geo_geohash_to_central_point(hash)
| render scatterchart with (kind=map) // map rendering available in Kusto Explorer desktop
```

<!-- csl: https://help.kusto.windows.net/Samples -->
```
print geohash = geo_point_to_geohash(139.806115, 35.554128, 12)  
```

| geohash      |
|--------------|
| xn76m27ty9g4 |

<!-- csl: https://help.kusto.windows.net/Samples -->
```
print geohash = geo_point_to_geohash(-80.195829, 25.802215, 8)
```

|geohash|
|---|
|dhwfz15h|

The following example finds groups of coordinates. Every pair of coordinates in the group reside in a rectangular area of 4.88 km on 4.88 km.
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
            by geohash = geo_point_to_geohash(longitude, latitude)    // geohash of the group
```

| geohash | count | locations  |
|---------|-------|------------|
| c23n8   | 2     | ["A", "B"] |
| c23n9   | 1     | ["C"]      |

The following example produces an empty result because of the invalid coordinate input.
<!-- csl: https://help.kusto.windows.net/Samples -->
```
print geohash = geo_point_to_geohash(200,1,8)
```

| geohash |
|---------|
|         |

The following example produces an empty result because of the invalid accuracy input.
<!-- csl: https://help.kusto.windows.net/Samples -->
```
print geohash = geo_point_to_geohash(1,1,int(null))
```

| geohash |
|---------|
|         |
