---
title: geo_h3cell_children() - Azure Data Explorer
description: Learn how to use the geo_h3cell_children() function to calculate the H3 cell children.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 03/09/2023
---
# geo_h3cell_children()

Calculates the H3 cell children.

Read more about [H3 Cell](https://eng.uber.com/h3/).

## Syntax

`geo_h3cell_children(`*h3cell*`,`*resolution*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *h3cell* | string | &check; | An H3 Cell token value as it was calculated by [geo_point_to_h3cell()](geo-point-to-h3cell-function.md).|
| *resolution* | int | | Defines the requested children cells resolution. Supported values are in the range [1, 15]. If unspecified, an immediate children token will be calculated.|

## Returns

Array of H3 Cell children tokens. If the H3 Cell is invalid or child resolution is lower than given cell, the query will produce a null result.

> [!NOTE]
>
> A difference between cell resolution and its children can't be more than 5. A difference of 5 levels will be resulted in up to 16807 children tokens.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUjOyMxJKUrNU7BVSE/Nj88wTk7NyYmHiWqoW5gZJRoamBulQYC6JgDo0XQKNwAAAA==" target="_blank">Run the query</a>

```kusto
print children = geo_h3cell_children('862a1072fffffff')
```

**Output**

|children|
|---|
|[ "872a10728ffffff", "872a10729ffffff", "872a1072affffff", "872a1072bffffff", "872a1072cffffff", "872a1072dffffff", "872a1072effffff" ]|

The following example counts children 3 levels below a given cell.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVHIMI5PTs3JUbBVULcwM0o0NDA3SoMAdWsFroKizLwSheSMzJyUotS8+OT8UiDXViGxqCixMj4nNS+9JEMjPTU/PsMYZEg8TKEG1FQdBSTJnNSy1ByYjKaCtoKxpiYAhE7I7YIAAAA=" target="_blank">Run the query</a>

```kusto
let h3_cell = '862a1072fffffff'; 
print children_count = array_length(geo_h3cell_children(h3_cell, geo_h3cell_level(h3_cell) + 3))
```

**Output**

|children_count|
|---|
|343|

The following example assembles GeoJSON geometry collection of H3 Cell children polygons.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA22PwU7DMAyG73sKq5e1EkiwSsCFExLwFlHovCZbEkeJhyji4XFouoKET8nv//9sx2QDw2Cs2ycM8AgjkjL9gM6pRW23D3c7fXtzvzvMte02X+Dfr/Ej6rBf00zAU0Q6tJmFOxZbTHTEoY74y2dSkdw0UmgXREnks/c62U8E0yujs1lcymUWgtcnVM5mnlO/hwj8mKmc8aZHFfVwajcg1ZS1mitonlHzOclzlsXvkdMkrUvg4n2pzSdyTuCWQlFrxGKW3z8LdhUtK0VMPPtWeND+B/7aQw1lGNYBXfcNNfE/VZEBAAA=" target="_blank">Run the query</a>

```kusto
print children = geo_h3cell_children('862a1072fffffff')
| mv-expand children to typeof(string)
| project child = geo_h3cell_to_polygon(children)
| summarize h3_hash_polygon_lst = make_list(child)
| project geojson = bag_pack(
    "type", "Feature",
    "geometry", bag_pack("type", "GeometryCollection", "geometries", h3_hash_polygon_lst),
    "properties", bag_pack("name", "H3 polygons collection"))
```

**Output**

|geojson|
|---|
|{ "type": "Feature", "geometry": { "type": "GeometryCollection", "geometries": [ ... ... ... ] }, "properties": { "name": "H3 polygons collection" }}|

The following example returns true because of the invalid cell.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUcgsjs8rzclRsAWyQAyN9NT8+Azj5NScnPjkjMyclKLUPA31xKRkdU1NABWsmhkyAAAA" target="_blank">Run the query</a>

```kusto
print is_null = isnull(geo_h3cell_children('abc'))
```

**Output**

|is_null|
|---|
|1|

The following example returns true because the level difference between cell and its children is more than 5.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUcgsjs8rzclRsAWyQAyN9NT8+Azj5NScnPjkjMyclKLUPLBYQT5QeXwJTFLDUEcBiCw1gZSppiYAGXNpOU0AAAA=" target="_blank">Run the query</a>

```kusto
print is_null = isnull(geo_h3cell_children(geo_point_to_h3cell(1, 1, 9), 15))
```

**Output**

|is_null|
|---|
|1|
