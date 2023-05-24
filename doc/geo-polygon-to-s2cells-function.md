---
title:  geo_polygon_to_s2cells()
description: Learn how to use the geo_polygon_to_s2cells() function to calculate S2 cell tokens that cover a polygon or a multipolygon on Earth.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 03/09/2023
---
# geo_polygon_to_s2cells()

Calculates S2 cell tokens that cover a polygon or multipolygon on Earth. This function is a useful geospatial join tool.

Read more about [S2 cell hierarchy](https://s2geometry.io/devguide/s2cell_hierarchy).

## Syntax

`geo_polygon_to_s2cells(`*polygon*`,` *level*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *polygon* | dynamic | &check; | Polygon or multipolygon in the [GeoJSON format](https://tools.ietf.org/html/rfc7946).|
| *level* | int | | Defines the requested cell level. Supported values are in the range [0, 30]. If unspecified, the default value `11` is used.|

## Returns

Array of S2 cell token strings that cover a polygon or a multipolygon. If either the polygon or level is invalid, or the cell count exceeds the limit, the query will produce a null result.

> [!NOTE]
>
> * Covering the polygon with S2 cell tokens can be useful in matching coordinates to polygons that might include these coordinates and matching polygons to polygons.
> * The polygon covering tokens are of the same S2 cell level.
> * The maximum count of tokens per polygon is 65536.
> * The [geodetic datum](https://en.wikipedia.org/wiki/Geodetic_datum) used for measurements on Earth is a sphere. Polygon edges are [Geodesics](https://en.wikipedia.org/wiki/Geodesic) on the sphere.
> * If input polygon edges are straight cartesian lines, consider using [geo_polygon_densify()](geo-polygon-densify-function.md) in order to convert planar edges to geodesics.

**Motivation for covering polygons with S2 cell tokens**

Without this function, here's one approach we could take in order to classify coordinates into polygons containing these coordinates.

```kusto
let Polygons = 
    datatable(description:string, polygon:dynamic)
    [  
      "New York",  dynamic({"type":"Polygon","coordinates":[[[-73.85009765625,40.85744791303121],[-74.16046142578125,40.84290487729676],[-74.190673828125,40.59935608796518],[-73.83087158203125,40.61812224225511],[-73.85009765625,40.85744791303121]]]}),
      "Seattle",   dynamic({"type":"Polygon","coordinates":[[[-122.200927734375,47.68573021131587],[-122.4591064453125,47.68573021131587],[-122.4755859375,47.468949677672484],[-122.17620849609374,47.47266286861342],[-122.200927734375,47.68573021131587]]]}),
      "Las Vegas", dynamic({"type":"Polygon","coordinates":[[[-114.9,36.36],[-115.4498291015625,36.33282808737917],[-115.4498291015625,35.84453450421662],[-114.949951171875,35.902399875143615],[-114.9,36.36]]]}),
    ];
let Coordinates = 
    datatable(longitude:real, latitude:real)
    [
      real(-73.95),  real(40.75), // New York
      real(-122.3),  real(47.6),  // Seattle
      real(-115.18), real(36.16)  // Las Vegas
    ];
Polygons | extend dummy=1
| join kind=inner (Coordinates | extend dummy=1) on dummy
| where geo_point_in_polygon(longitude, latitude, polygon)
| project longitude, latitude, description
```

**Output**

|longitude|latitude|description|
|---|---|---|
|-73.95|40.75|New York city|
|-122.3|47.6|Seattle|
|-115.18|36.16|Las Vegas|

While this method works in some cases, it's inefficient. This method does a cross-join, meaning that it tries to match every polygon to every point. This process consumes a large amount of memory and compute resources.
Instead, we would like to match every polygon to a point with a high probability of containment success, and filter out other points.

This match can be achieved by the following process:

1. Converting polygons to S2 cells of level k,
1. Converting points to the same S2 cells level k,
1. Joining on S2 cells,
1. Filtering by [geo_point_in_polygon()](geo-point-in-polygon-function.md).

**Choosing the S2 cell level**

* Ideally we would want to cover every polygon with one or just a few unique cells such that no two polygons share the same cell.
* If the polygons are close to each other, choose the [S2 cell level](geo-point-to-s2cell-function.md) such that its cell edge will be smaller (4, 8, 12 times smaller) than the edge of the average polygon.
* If the polygons are far from each other, choose the [S2 cell level](geo-point-to-s2cell-function.md) such that its cell edge will be similar to the edge of the average polygon.
* In practice, covering a polygon with more than 10,000 cells might not yield good performance.
* Sample use cases:
* S2 cell level 5 might prove to be good for covering countries/regions.
* S2 cell level 16 can cover dense and relatively small Manhattan (New York) neighborhoods.
* S2 cell level 11 can be used for covering suburbs of Australia.
* Query run time and memory consumption might differ because of different S2 cell level values.

> [!WARNING]
> Covering a large-area polygon with small-area cells can lead to a huge amount of covering cells. As a result, the query might return null.

## Examples

The following example classifies coordinates into polygons.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=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" target="_blank">Run the query</a>

```kusto
let Polygons = 
    datatable(description:string, polygon:dynamic)
    [
        'Greenwich Village', dynamic({"type":"Polygon","coordinates":[[[-73.991460000000131,40.731738000000206],[-73.992854491775518,40.730082566051351],[-73.996772,40.725432000000154],[-73.997634685522883,40.725786309886963],[-74.002855946639244,40.728346630056791],[-74.001413,40.731065000000207],[-73.996796995070824,40.73736378205173],[-73.991724524037934,40.735245208931886],[-73.990703782359589,40.734781896080477],[-73.991460000000131,40.731738000000206]]]}),
        'Upper West Side',   dynamic({"type":"Polygon","coordinates":[[[-73.958357552055688,40.800369095633819],[-73.98143901556422,40.768762584141953],[-73.981548752788598,40.7685590292784],[-73.981565335901905,40.768307084720796],[-73.981754418060945,40.768399727738668],[-73.982038573548124,40.768387823012056],[-73.982268248204349,40.768298621883247],[-73.982384797518051,40.768097213086911],[-73.982320919746599,40.767894461792181],[-73.982155532845766,40.767756204474757],[-73.98238873834039,40.767411004834273],[-73.993650353659021,40.772145571634361],[-73.99415893763998,40.772493009137818],[-73.993831082030937,40.772931787850908],[-73.993891252437052,40.772955194876722],[-73.993962585514595,40.772944653908901],[-73.99401262480508,40.772882846631894],[-73.994122058082397,40.77292405902601],[-73.994136652588594,40.772901870174394],[-73.994301342391154,40.772970028663913],[-73.994281535134448,40.77299380206933],[-73.994376552751078,40.77303955110149],[-73.994294029824005,40.773156243992048],[-73.995023275860802,40.773481196576356],[-73.99508939189289,40.773388475039134],[-73.995013963716758,40.773358035426909],[-73.995050284699261,40.773297153189958],[-73.996240651898916,40.773789791397689],[-73.996195837470992,40.773852356184044],[-73.996098807369748,40.773951805299085],[-73.996179459973888,40.773986954351571],[-73.996095245226442,40.774086186437756],[-73.995572265161172,40.773870731394297],[-73.994017424135961,40.77321375261053],[-73.993935876811335,40.773179512586211],[-73.993861942928888,40.773269531698837],[-73.993822393527211,40.773381758622882],[-73.993767019318497,40.773483981224835],[-73.993698463744295,40.773562141052594],[-73.993358326468751,40.773926888327956],[-73.992622663865575,40.774974056037109],[-73.992577842766124,40.774956016359418],[-73.992527743951555,40.775002110439829],[-73.992469745815342,40.775024159551755],[-73.992403837191887,40.775018140390664],[-73.99226708903538,40.775116033858794],[-73.99217809026365,40.775279293897171],[-73.992059084937338,40.775497598192516],[-73.992125372394938,40.775509075053385],[-73.992226867797001,40.775482211026116],[-73.992329346608813,40.775468900958522],[-73.992361756801131,40.775501899766638],[-73.992386042960277,40.775557180424634],[-73.992087684712729,40.775983970821372],[-73.990927174149746,40.777566878763238],[-73.99039616003671,40.777585065679204],[-73.989461267506471,40.778875124584417],[-73.989175778438053,40.779287524015778],[-73.988868617400072,40.779692922911607],[-73.988871874499793,40.779713738253008],[-73.989219022880576,40.779697895209402],[-73.98927785904425,40.779723439271038],[-73.989409054180143,40.779737706471963],[-73.989498614927044,40.779725044389757],[-73.989596493388234,40.779698146683387],[-73.989679812902509,40.779677568658038],[-73.989752702937935,40.779671244211556],[-73.989842247806507,40.779680752670664],[-73.990040102120489,40.779707677698219],[-73.990137977524839,40.779699769704784],[-73.99033584033225,40.779661794394983],[-73.990430598697046,40.779664973055503],[-73.990622199396725,40.779676064914298],[-73.990745069505479,40.779671328184051],[-73.990872114282197,40.779646007643876],[-73.990961672224358,40.779639683751753],[-73.991057472829539,40.779652352625774],[-73.991157429497036,40.779669775606465],[-73.991242817404469,40.779671367084504],[-73.991255318289745,40.779650782516491],[-73.991294887120119,40.779630209208889],[-73.991321967649895,40.779631796041372],[-73.991359455569423,40.779585883337383],[-73.991551059227476,40.779574821437407],[-73.99141982585985,40.779755280287233],[-73.988886144117032,40.779878898532999],[-73.988939656706265,40.779956178440393],[-73.988926103530844,40.780059292013632],[-73.988911680264692,40.780096037146606],[-73.988919261468567,40.780226094343945],[-73.988381050202634,40.780981074045783],[-73.988232413846987,40.781233144215555],[-73.988210420831663,40.781225482542055],[-73.988140000000143,40.781409000000224],[-73.988041288067166,40.781585961353777],[-73.98810029382463,40.781602878305286],[-73.988076449145055,40.781650935001608],[-73.988018059972219,40.781634188810422],[-73.987960792842145,40.781770987031535],[-73.985465811970457,40.785360700575431],[-73.986172704965611,40.786068452258647],[-73.986455862401996,40.785919219081421],[-73.987072345615601,40.785189638820121],[-73.98711901394276,40.785210319004058],[-73.986497781023601,40.785951202887254],[-73.986164628806279,40.786121882448327],[-73.986128422486075,40.786239001331111],[-73.986071135219746,40.786240706026611],[-73.986027274789123,40.786228964236727],[-73.986097637849426,40.78605822569795],[-73.985429321269592,40.785413942184597],[-73.985081137732209,40.785921935110366],[-73.985198833254501,40.785966552197777],[-73.985170502389906,40.78601333415817],[-73.985216218673656,40.786030501816427],[-73.98525509797993,40.785976205511588],[-73.98524273937646,40.785972572653328],[-73.98524962933017,40.785963139855845],[-73.985281779186749,40.785978620950075],[-73.985240032884533,40.786035858136792],[-73.985683885242182,40.786222123919686],[-73.985717529004575,40.786175994668795],[-73.985765660297687,40.786196274858618],[-73.985682871922691,40.786309786213067],[-73.985636270930442,40.786290150649279],[-73.985670722564691,40.786242911993817],[-73.98520511880038,40.786047669212785],[-73.985211035607492,40.786039554883686],[-73.985162639946992,40.786020999769754],[-73.985131636312062,40.786060297019972],[-73.985016964065125,40.78601423719563],[-73.984655078830457,40.786534741807841],[-73.985743787901043,40.786570082854738],[-73.98589227228328,40.786426529019593],[-73.985942854994988,40.786452847880334],[-73.985949561556794,40.78648711396653],[-73.985812373526713,40.786616865357047],[-73.985135209703174,40.78658761889551],[-73.984619428584324,40.786586016349787],[-73.981952458164173,40.790393724337193],[-73.972823037363767,40.803428052816756],[-73.971036786332192,40.805918478839672],[-73.966701,40.804169000000186],[-73.959647,40.801156000000113],[-73.958508540159471,40.800682279767472],[-73.95853274080838,40.800491362464697],[-73.958357552055688,40.800369095633819]]]}),
        'Upper East Side',   dynamic({"type":"Polygon","coordinates":[[[-73.943592454622546,40.782747908206574],[-73.943648235390199,40.782656161333449],[-73.943870759887162,40.781273026571704],[-73.94345932494096,40.780048275653243],[-73.943213862652243,40.779317588660199],[-73.943004239504688,40.779639495474292],[-73.942716005450905,40.779544169476175],[-73.942712374762181,40.779214856940001],[-73.942535563208608,40.779090956062532],[-73.942893408188027,40.778614093246276],[-73.942438481745029,40.777315235766039],[-73.942244919522594,40.777104088947254],[-73.942074188038887,40.776917846977142],[-73.942002667222781,40.776185317382648],[-73.942620205199006,40.775180871576474],[-73.94285645694552,40.774796600349191],[-73.94293043781397,40.774676268036011],[-73.945870899588215,40.771692257932997],[-73.946618690150586,40.77093339256956],[-73.948664164778933,40.768857624399587],[-73.950069793030679,40.767025088383498],[-73.954418260786071,40.762184104951245],[-73.95650786241211,40.760285256574043],[-73.958787773424007,40.758213471309809],[-73.973015157270069,40.764278692864671],[-73.955760332998182,40.787906554459667],[-73.944023,40.782960000000301],[-73.943592454622546,40.782747908206574]]]}),
    ];
let Coordinates = 
    datatable(longitude:real, latitude:real)
    [
        real(-73.9741), 40.7914, // Upper West Side
        real(-73.9950), 40.7340, // Greenwich Village
        real(-73.9584), 40.7688, // Upper East Side
    ];
let Level = 16;
Polygons
| extend covering = geo_polygon_to_s2cells(polygon, Level) // cover every polygon with s2 cell token array
| mv-expand covering to typeof(string)                     // expand cells array such that every row will have one cell mapped to its polygon
| join kind=inner hint.strategy=broadcast                  // assume that Polygons count is small (In some specific case)
(
    Coordinates
    | extend covering = geo_point_to_s2cell(longitude, latitude, Level) // cover point with cell
) on covering // join on the cell, this filters out rows of point and polygons where the point definitely does not belong to the polygon
| where geo_point_in_polygon(longitude, latitude, polygon)
| project longitude, latitude, description
```

**Output**

|longitude|latitude|description|
|---|---|---|
|-73.9741|40.7914|Upper West Side|
|-73.995|40.734|Greenwich Village|
|-73.9584|40.7688|Upper East Side|

The following example filters out polygons that don't intersect with the area of the polygon of interest. The maximum error is diagonal of s2cell length. This example is based on a polygonized earth at night raster file.

```kusto
let intersection_level_hint = 7;
let area_of_interest = dynamic({"type": "Polygon","coordinates": [[[-73.94966125488281,40.79698248639272],[-73.95841598510742,40.800426144169315],[-73.98124694824219,40.76806170936614],[-73.97283554077148,40.7645513650551],[-73.94966125488281,40.79698248639272]]]});
let area_of_interest_covering = geo_polygon_to_s2cells(area_of_interest, intersection_level_hint);
EarthAtNight
| project value = features.properties.DN, polygon = features.geometry
| extend covering = geo_polygon_to_s2cells(polygon, intersection_level_hint)
| mv-apply c = covering to typeof(string) on
(
    summarize is_intersects = take_anyif(1, array_index_of(area_of_interest_covering, c) != -1)
)
| where is_intersects == 1
| count
```

**Output**

|Count|
|---|
|83|

Count of cells that will be needed in order to cover some polygon with S2 cells of level 5.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAy2OwQrDIBBE7/0K8ZSABxvIJSH/0LuIiFlSqd0VNQcp/fcqzWVmeLMLE6CwSKEehGxje0X79m748FIj8IU//hUX3BGl3aMtkPmilJJCatF07naXV+hU6++43mLyWFiejIMQTKEXoHF0NrYxm5KtJgAe5TkcQOZa0M5MnvpDHi4k2DyO6w+pbpZDpgAAAA==" target="_blank">Run the query</a>

```kusto
let polygon = dynamic({"type":"Polygon","coordinates":[[[0,0],[0,50],[100,50],[0,0]]]});
print s2_cell_token_count = array_length(geo_polygon_to_s2cells(polygon, 5));
```

**Output**

|s2_cell_token_count|
|---|
|286|

Covering a large-area polygon with small-area cells returns null.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVEoyM+pTM/PU7BVSKnMS8zNTNaoViqpLEhVslIKgEgp6Sgl5+cXpWTmJZakFitZRUdHG+gYxOoASVMQZWgAZYBEY2NrNa25Cooy80oU0lPz46HGx5fkxxcbJafm5BRrQIV0FIwNNK0BZqBOaoMAAAA=" target="_blank">Run the query</a>

```kusto
let polygon = dynamic({"type":"Polygon","coordinates":[[[0,0],[0,50],[100,50],[0,0]]]});
print geo_polygon_to_s2cells(polygon, 30);
```

**Output**

|print_0|
|---|
||

Covering a large-area polygon with small-area cells returns null.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVEoyM+pTM/PU7BVSKnMS8zNTNaoViqpLEhVslIKgEgp6Sgl5+cXpWTmJZakFitZRUdHG+gYxOoASVMQZWgAZYBEY2NrNa25Cooy80oUMovzSnNyNNJT8+OhtsSX5McXGyWn5uQUa0CFdBSMDTQ1rQE1TEGniwAAAA==" target="_blank">Run the query</a>

```kusto
let polygon = dynamic({"type":"Polygon","coordinates":[[[0,0],[0,50],[100,50],[0,0]]]});
print isnull(geo_polygon_to_s2cells(polygon, 30));
```

**Output**

|print_0|
|---|
|1|
