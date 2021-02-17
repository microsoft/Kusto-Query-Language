---
title: geo_distance_point_to_line() - Azure Data Explorer
description: This article describes geo_distance_point_to_line() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mbrichko
ms.service: data-explorer
ms.topic: reference
ms.date: 03/11/2020
---
# geo_distance_point_to_line()

Calculates the shortest distance between a coordinate and a line or multiline on Earth.

## Syntax

`geo_distance_point_to_line(`*longitude*`, `*latitude*`, `*lineString*`)`

## Arguments

* *longitude*: Geospatial coordinate longitude value in degrees. Valid value is a real number and in the range [-180, +180].
* *latitude*: Geospatial coordinate latitude value in degrees. Valid value is a real number and in the range [-90, +90].
* *lineString*: Line or multiline in the [GeoJSON format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type.

## Returns

The shortest distance, in meters, between a coordinate and a line on Earth. If the coordinate or lineString are invalid, the query will produce a null result.

> [!NOTE]
> * The geospatial coordinates are interpreted as represented by the [WGS-84](https://earth-info.nga.mil/GandG/update/index.php?action=home) coordinate reference system.
> * The [geodetic datum](https://en.wikipedia.org/wiki/Geodetic_datum) used to measure distance on Earth is a sphere. Line edges are [geodesics](https://en.wikipedia.org/wiki/Geodesic) on the sphere.
> * If input line edges are straight cartesian lines, consider using [geo_line_densify()](geo-line-densify-function.md) in order to convert planar edges to geodesics.

**LineString definition and constraints**

dynamic({"type": "LineString","coordinates": [ [lng_1,lat_1], [lng_2,lat_2] ,..., [lng_N,lat_N] ]})

dynamic({"type": "MultiLineString","coordinates": [ [ line_1, line_2 ,..., line_N ] ]})

* LineString coordinates array must contain at least two entries.
* Coordinates [longitude,latitude] must be valid where longitude is a real number in the range [-180, +180] and latitude is a real number in the range [-90, +90].
* Edge length must be less than 180 degrees. The shortest edge between the two vertices will be chosen.

> [!TIP]
> * For better performance, use literal lines.
> * If you want to know the shortest distance between one or more points to many lines, consider folding these lines into one multiline. See the [example](#examples) below.

## Examples

The following example finds the shortest distance between North Las Vegas Airport and a nearby road.

:::image type="content" source="images/geo-distance-point-to-line-function/distance-point-to-line.png" alt-text="Distance between North Las Vegas Airport and road":::

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print distance_in_meters = geo_distance_point_to_line(-115.199625, 36.210419, dynamic({ "type":"LineString","coordinates":[[-115.115385,36.229195],[-115.136995,36.200366],[-115.140252,36.192470],[-115.143558,36.188523],[-115.144076,36.181954],[-115.154662,36.174483],[-115.166431,36.176388],[-115.183289,36.175007],[-115.192612,36.176736],[-115.202485,36.173439],[-115.225355,36.174365]]}))
```

| distance_in_meters |
|--------------------|
| 3797.88887253334   |

Storm events in south coast US. The events are filtered by a maximum distance of 5 km from the defined shore line.

:::image type="content" source="images/geo-distance-point-to-line-function/us-south-coast-storm-events.png" alt-text="Storm events in the US south coast":::

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let southCoast = dynamic({"type":"LineString","coordinates":[[-97.18505859374999,25.997549919572112],[-97.58056640625,26.96124577052697],[-97.119140625,27.955591004642553],[-94.04296874999999,29.726222319395504],[-92.98828125,29.82158272057499],[-89.18701171875,29.11377539511439],[-89.384765625,30.315987718557867],[-87.5830078125,30.221101852485987],[-86.484375,30.4297295750316],[-85.1220703125,29.6880527498568],[-84.00146484374999,30.14512718337613],[-82.6611328125,28.806173508854776],[-82.81494140625,28.033197847676377],[-82.177734375,26.52956523826758],[-80.9912109375,25.20494115356912]]});
StormEvents
| project BeginLon, BeginLat, EventType
| where geo_distance_point_to_line(BeginLon, BeginLat, southCoast) < 5000
| render scatterchart with (kind=map) // map rendering available in Kusto Explorer desktop
```

NY taxi pickups. Pickups are filtered by maximum distance of 0.1 meters from the defined multiline.

:::image type="content" source="images/geo-distance-point-to-line-function/madison-ave-road.png" alt-text="NYC taxi pickups on Madison Ave":::

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let MadisonAve = dynamic({"type":"MultiLineString","coordinates":[[[-73.9879823,40.7408625],[-73.9876492,40.7413345],[-73.9874982,40.7415046],[-73.9870343,40.7421446],[-73.9865812,40.7427655],[-73.9861292,40.7433756],[-73.9856813,40.7439956],[-73.9854932,40.7442606],[-73.9852232,40.7446216],[-73.9847903,40.7452305],[-73.9846232,40.7454536],[-73.9844803,40.7456606],[-73.9843413,40.7458585],[-73.9839533,40.7463955],[-73.9839002,40.7464696],[-73.9837683,40.7466566],[-73.9834342,40.7471015],[-73.9833833,40.7471746],[-73.9829712,40.7477686],[-73.9824752,40.7484255],[-73.9820262,40.7490436],[-73.9815623,40.7496566],[-73.9811212,40.7502796],[-73.9809762,40.7504976],[-73.9806982,40.7509255],[-73.9802752,40.7515216],[-73.9798033,40.7521795],[-73.9795863,40.7524656],[-73.9793082,40.7528316],[-73.9787872,40.7534725],[-73.9783433,40.7540976],[-73.9778912,40.7547256],[-73.9774213,40.7553365],[-73.9769402,40.7559816],[-73.9764622,40.7565766],[-73.9760073,40.7572036],[-73.9755592,40.7578366],[-73.9751013,40.7584665],[-73.9746532,40.7590866],[-73.9741902,40.7597326],[-73.9737632,40.7603566],[-73.9733032,40.7609866],[-73.9728472,40.7616205],[-73.9723422,40.7622826],[-73.9718672,40.7629556],[-73.9714042,40.7635726],[-73.9709362,40.7642185],[-73.9705282,40.7647636],[-73.9704903,40.7648196],[-73.9703342,40.7650355],[-73.9701562,40.7652826],[-73.9700322,40.7654535],[-73.9695742,40.7660886],[-73.9691232,40.7667166],[-73.9686672,40.7673375],[-73.9682142,40.7679605],[-73.9677482,40.7685786],[-73.9672883,40.7692076],[-73.9668412,40.7698296],[-73.9663882,40.7704605],[-73.9659222,40.7710936],[-73.9654262,40.7717756],[-73.9649292,40.7724595],[-73.9644662,40.7730955],[-73.9640012,40.7737285],[-73.9635382,40.7743615],[-73.9630692,40.7749936],[-73.9626122,40.7756275],[-73.9621172,40.7763106],[-73.9616111,40.7769896],[-73.9611552,40.7776245],[-73.9606891,40.7782625],[-73.9602212,40.7788866],[-73.9597532,40.7795236],[-73.9595842,40.7797445],[-73.9592942,40.7801635],[-73.9591122,40.7804105],[-73.9587982,40.7808305],[-73.9582992,40.7815116],[-73.9578452,40.7821455],[-73.9573802,40.7827706],[-73.9569262,40.7833965],[-73.9564802,40.7840315],[-73.9560102,40.7846486],[-73.9555601,40.7852755],[-73.9551221,40.7859005],[-73.9546752,40.7865426],[-73.9542571,40.7871505],[-73.9541771,40.7872335],[-73.9540892,40.7873366],[-73.9536971,40.7879115],[-73.9532792,40.7884706],[-73.9532142,40.7885205],[-73.9531522,40.7885826],[-73.9527382,40.7891785],[-73.9523081,40.7897545],[-73.9518332,40.7904115],[-73.9513721,40.7910435],[-73.9509082,40.7916695],[-73.9504602,40.7922995],[-73.9499882,40.7929195],[-73.9495051,40.7936045],[-73.9490071,40.7942835],[-73.9485542,40.7949065],[-73.9480832,40.7955345],[-73.9476372,40.7961425],[-73.9471772,40.7967915],[-73.9466841,40.7974475],[-73.9453432,40.7992905],[-73.9448332,40.7999835],[-73.9443442,40.8006565],[-73.9438862,40.8012945],[-73.9434262,40.8019196],[-73.9431412,40.8023325],[-73.9429842,40.8025585],[-73.9425691,40.8031855],[-73.9424401,40.8033609],[-73.9422987,40.8035533],[-73.9422013,40.8036857],[-73.9421022,40.8038205],[-73.9420024,40.8039552],[-73.9416372,40.8044485],[-73.9411562,40.8050725],[-73.9406471,40.8057176],[-73.9401481,40.8064135],[-73.9397022,40.8070255],[-73.9394081,40.8074155],[-73.9392351,40.8076495],[-73.9387842,40.8082715],[-73.9384681,40.8087086],[-73.9383211,40.8089025],[-73.9378792,40.8095215],[-73.9374011,40.8101795],[-73.936405,40.8115707],[-73.9362328,40.8118098]],[[-73.9362328,40.8118098],[-73.9362432,40.8118567],[-73.9361239,40.8120222],[-73.9360302,40.8120805]],[[-73.9362328,40.8118098],[-73.9361571,40.8118294],[-73.9360443,40.8119993],[-73.9360302,40.8120805]],[[-73.9360302,40.8120805],[-73.9359423,40.8121378],[-73.9358551,40.8122385],[-73.9352181,40.8130815],[-73.9348702,40.8135515],[-73.9347541,40.8137145],[-73.9346332,40.8138615],[-73.9345542,40.8139595],[-73.9344981,40.8139945],[-73.9344571,40.8140165],[-73.9343962,40.8140445],[-73.9343642,40.8140585],[-73.9343081,40.8140725],[-73.9341971,40.8140895],[-73.9341041,40.8141005],[-73.9340022,40.8140965],[-73.9338442,40.8141005],[-73.9333712,40.8140895],[-73.9325541,40.8140755],[-73.9324561,40.8140705],[-73.9324022,40.8140695]],[[-73.9360302,40.8120805],[-73.93605,40.8121667],[-73.9359632,40.8122805],[-73.9353631,40.8130795],[-73.9351482,40.8133625],[-73.9350072,40.8135415],[-73.9347441,40.8139168],[-73.9346611,40.8140125],[-73.9346101,40.8140515],[-73.9345401,40.8140965],[-73.9344381,40.8141385],[-73.9343451,40.8141555],[-73.9342991,40.8141675],[-73.9341552,40.8141985],[-73.9338601,40.8141885],[-73.9333991,40.8141815],[-73.9323981,40.8141665]]]});
nyc_taxi
| project pickup_longitude, pickup_latitude
| where geo_distance_point_to_line(pickup_longitude, pickup_latitude, MadisonAve) <= 0.1
| take 100
| render scatterchart with (kind=map) // map rendering available in Kusto Explorer desktop
```

The following example folds many lines into one multiline and queries this multiline. The query finds all taxi pickups that happened 10km away from all roads in Manhattan.

:::image type="content" source="images/geo-distance-point-to-line-function/lines-folding.png" alt-text="Lines folding":::

```kusto
let ManhattanRoads =
    datatable(features:dynamic)
    [
        dynamic({"type":"Feature","properties":{"Label":"145thStreetBrg"},"geometry":{"type":"MultiLineString","coordinates":[[[-73.9322259,40.8194635],[-73.9323259,40.8194743],[-73.9323973,40.8194779]]]}}),
        dynamic({"type":"Feature","properties":{"Label":"W120thSt"},"geometry":{"type":"MultiLineString","coordinates":[[[-73.9619541,40.8104844],[-73.9621542,40.8105725],[-73.9630542,40.8109455],[-73.9635902,40.8111714],[-73.9639492,40.8113174],[-73.9640502,40.8113705]]]}}),
        dynamic({"type":"Feature","properties":{"Label":"1stAve"},"geometry":{"type":"MultiLineString","coordinates":[[[-73.9704124,40.748033],[-73.9702043,40.7480906],[-73.9696892,40.7487346],[-73.9695012,40.7491976],[-73.9694522,40.7493196]],[[-73.9699932,40.7488636],[-73.9694522,40.7493196]],[[-73.9694522,40.7493196],[-73.9693113,40.7494946],[-73.9688832,40.7501056],[-73.9686562,40.7504196],[-73.9684231,40.7507476],[-73.9679832,40.7513586],[-73.9678702,40.7514986]],[[-73.9676833,40.7520426],[-73.9675462,40.7522286],[-73.9673532,40.7524976],[-73.9672892,40.7525906],[-73.9672122,40.7526806]]]}})
        // ... more roads ...
    ];
let allRoads=toscalar(
    ManhattanRoads
    | project road_coordinates=features.geometry.coordinates
    | summarize make_list(road_coordinates)
    | project multipolygon = pack("type","MultiLineString", "coordinates", list_road_coordinates));
nyc_taxi
| project pickup_longitude, pickup_latitude
| where pickup_longitude != 0 and pickup_latitude != 0
| where geo_distance_point_to_line(pickup_longitude, pickup_latitude, todynamic(allRoads)) > 10000
| take 10
| render scatterchart with (kind=map)
```

The following example will return a null result because of the invalid LineString input.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print distance_in_meters = geo_distance_point_to_line(1,1, dynamic({ "type":"LineString"}))
```

| distance_in_meters |
|--------------------|
|                    |

The following example will return a null result because of the invalid coordinate input.

```kusto
print distance_in_meters = geo_distance_point_to_line(300, 3, dynamic({ "type":"LineString","coordinates":[[1,1],[2,2]]}))
```

| distance_in_meters |
|--------------------|
|                    |
