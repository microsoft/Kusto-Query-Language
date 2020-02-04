# geo_point_to_s2cell()

Calculates the S2 cell token string value for a geographic location.

For more information about S2 Cells, click [here](http://s2geometry.io/devguide/s2cell_hierarchy).

**Syntax**

`geo_point_to_s2cell(`*longitude*`, `*latitude*`, `*level*`)`

**Arguments**

* *longitude*: Longitude value of a geographic location. Longitude x will be considered valid if x is a real number and x is in range [-180, +180]. 
* *latitude*: Latitude value of a geographic location. Latitude y will be considered valid if y is a real number and y in in range [-90, +90]. 
* *level*: An optional `int` that defines the requested cell level. Supported values are in the range [0,30]. If unspecified, the default value `11` is used.

**Returns**

The S2 Cell Token string value of a given geographic location. If the coordinate or level are invalid, the query will produce an empty result.

> [!NOTE]
>
> * S2Cell can be a useful geospatial clustering tool.
> * S2Cell has 31 levels of hierarchy with area coverage ranging from 85,011,012.19kmÂ² at the highest level 0 to 00.44cmÂ² at the lowest level 30.
> * S2Cell has preserves the cell center well during level increase from 0 to 30. This is because cells reside along a [Hilbert Curve](https://en.wikipedia.org/wiki/Hilbert_curve).
> * S2Cell is a cell on a sphere surface and it's edges are geodesics.
> * Invoking the [geo_s2cell_to_central_point()](geo-s2cell-to-central-point-function.md) function on a s2cell token string that was calculated on longitude x and latitude y won't necessarily return x and y.
> * It's possible that two geographic locations are very close to each other but have different S2 Cell tokens.

**S2 Cell area coverage per level value:**

| Level | Min area    | Max area    | Average area | Units |
|-------|-------------|-------------|--------------|-------|
| 0     | 85011012.19 | 85011012.19 | 85011012.19  | kmÂ²   |
| 1     | 21252753.05 | 21252753.05 | 21252753.05  | kmÂ²   |
| 2     | 4919708.23  | 6026521.16  | 5313188.26   | kmÂ²   |
| 3     | 1055377.48  | 1646455.5   | 1328297.07   | kmÂ²   |
| 4     | 231564.06   | 413918.15   | 332074.27    | kmÂ²   |
| 5     | 53798.67    | 104297.91   | 83018.57     | kmÂ²   |
| 6     | 12948.81    | 26113.3     | 20754.64     | kmÂ²   |
| 7     | 3175.44     | 6529.09     | 5188.66      | kmÂ²   |
| 8     | 786.2       | 1632.45     | 1297.17      | kmÂ²   |
| 9     | 195.59      | 408.12      | 324.29       | kmÂ²   |
| 10    | 48.78       | 102.03      | 81.07        | kmÂ²   |
| 11    | 12.18       | 25.51       | 20.27        | kmÂ²   |
| 12    | 3.04        | 6.38        | 5.07         | kmÂ²   |
| 13    | 0.76        | 1.59        | 1.27         | kmÂ²   |
| 14    | 0.19        | 0.4         | 0.32         | kmÂ²   |
| 15    | 47520.3     | 99638.93    | 79172.67     | mÂ²    |
| 16    | 11880.08    | 24909.73    | 19793.17     | mÂ²    |
| 17    | 2970.02     | 6227.43     | 4948.29      | mÂ²    |
| 18    | 742.5       | 1556.86     | 1237.07      | mÂ²    |
| 19    | 185.63      | 389.21      | 309.27       | mÂ²    |
| 20    | 46.41       | 97.3        | 77.32        | mÂ²    |
| 21    | 11.6        | 24.33       | 19.33        | mÂ²    |
| 22    | 2.9         | 6.08        | 4.83         | mÂ²    |
| 23    | 0.73        | 1.52        | 1.21         | mÂ²    |
| 24    | 0.18        | 0.38        | 0.3          | mÂ²    |
| 25    | 453.19      | 950.23      | 755.05       | cmÂ²   |
| 26    | 113.3       | 237.56      | 188.76       | cmÂ²   |
| 27    | 28.32       | 59.39       | 47.19        | cmÂ²   |
| 28    | 7.08        | 14.85       | 11.8         | cmÂ²   |
| 29    | 1.77        | 3.71        | 2.95         | cmÂ²   |
| 30    | 0.44        | 0.93        | 0.74         | cmÂ²   |

The table source can be found [here](http://s2geometry.io/resources/s2cell_statistics).

See also [geo_point_to_geohash()](geo-point-to-geohash-function.md).

**Examples**

US storm events aggregated by s2cell.
![US S2Cell](./images/queries/geo/s2cell.png)
<!-- csl: https://help.kusto.windows.net/Samples -->
```
StormEvents
| project BeginLon, BeginLat
| summarize by hash=geo_point_to_s2cell(BeginLon, BeginLat, 5)
| project geo_s2cell_to_central_point(hash)
| render scatterchart with (kind=map) // map rendering available in Kusto Explorer desktop
```

<!-- csl: https://help.kusto.windows.net/Samples -->
```
print s2cell = geo_point_to_s2cell(-80.195829, 25.802215, 8)
```

| s2cell |
|--------|
| 88d9b  |

The following example finds groups of coordinates. Every pair of coordinates in the group reside in s2cell with maximum area of 1632.45 kmÂ².
<!-- csl: https://help.kusto.windows.net/Samples -->
```
datatable(location_id:string, longitude:real, latitude:real)
[
  "A", 10.1234, 53,
  "B", 10.3579, 53,
  "C", 10.6842, 53,
]
| summarize count = count(),                                        // items per group count
            locations = make_list(location_id)                      // items in the group
            by s2cell = geo_point_to_s2cell(longitude, latitude, 8) // s2 cell of the group
```

| s2cell | count | locations |
|--------|-------|-----------|
| 47b1d  | 2     | ["A","B"] |
| 47ae3  | 1     | ["C"]     |

The following example produces an empty result because of the invalid coordinate input.
<!-- csl: https://help.kusto.windows.net/Samples -->
```
print s2cell = geo_point_to_s2cell(300,1,8)
```

| s2cell |
|--------|
|        |

The following example produces an empty result because of the invalid level input.
<!-- csl: https://help.kusto.windows.net/Samples -->
```
print s2cell = geo_point_to_s2cell(1,1,35)
```

| s2cell |
|--------|
|        |

The following example produces an empty result because of the invalid level input.
<!-- csl: https://help.kusto.windows.net/Samples -->
```
print s2cell = geo_point_to_s2cell(1,1,int(null))
```

| s2cell |
|--------|
|        |
