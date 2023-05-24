---
title:  geo_h3cell_neighbors()
description: Learn how to use the geo_h3cell_neighbors() function to calculate the H3 cell neighbors.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 03/09/2023
---
# geo_h3cell_neighbors()

Calculates the H3 cell neighbors.

Read more about [H3 Cell](https://eng.uber.com/h3/).

## Syntax

`geo_h3cell_neighbors(`*h3cell*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *h3cell* | string | &check; | An H3 Cell token value as it was calculated by [geo_point_to_h3cell()](geo-point-to-h3cell-function.md).|

## Returns

An array of H3 cell neighbors. If the H3 Cell is invalid, the query will produce a null result.

> [!NOTE]
> If more than immidiate neighbors are needed, please see [geo_h3cell_rings()](geo-h3cell-rings-function.md).

## Examples

The following example calculates H3 cell neighbors.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUchLzUzPSMovKlawVUhPzY/PME5OzcmJhwtrqFuYGSUaGpgbpUGAuiYA0YTxRzkAAAA=" target="_blank">Run the query</a>

```kusto
print neighbors = geo_h3cell_neighbors('862a1072fffffff')
```

**Output**

|neighbors|
|---|
|["862a10727ffffff","862a10707ffffff","862a1070fffffff","862a10777ffffff","862a100dfffffff","862a100d7ffffff"]|

The following example calculates an array of input H3 cell with its neighbors.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVHIME5OzclRsFVQtzAzSjQ0MDdKgwB1a66Cosy8EgWQfDFQQWJRUWJlfHJ+XnJiiUZBYnJ2PFhEA2KCpo5Cemp+PIQTn5eamZ6RlF9UDJPVBADbK8PrbAAAAA==" target="_blank">Run the query</a>

```kusto
let h3cell = '862a1072fffffff';
print cells = array_concat(pack_array(h3cell), geo_h3cell_neighbors(h3cell))
```

**Output**

|cells|
|---|
|["862a1072fffffff","862a10727ffffff","862a10707ffffff","862a1070fffffff","862a10777ffffff","862a100dfffffff","862a100d7ffffff"]|

The following example calculates H3 cells polygons GeoJSON geometry collection.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA2WQ0UoDMRBF3/sVIS/NQgWtoIL4JOhnhGmcbtMmmZBMxRU/3sluigXzlLlz58xNArI63DsMQb2o9dPDFu5uH7f75ayfV7n4xKr1qxigFJiso+SATQZ3srNiFsKwUSOSXQqb0I+HHZV66Q6rHxU/b/ArQ/roSCbFU0bam8qyaWyeXOiIjlWmMI2U2t4rLJPtDTMj2kQ9xwjFf2MLKPYIJ7TBV8nYGddcgR0rJfHtYLTtFUa3EHqj9BsCn4tctbgicplE/W97781XCkGQnlJT+4jHKpUkkf/QsjRj4UX7AyWIM+iSTw/DL9SI6V2KAQAA" target="_blank">Run the query</a>

```kusto
let h3cell = '862a1072fffffff';
print cells = array_concat(pack_array(h3cell), geo_h3cell_neighbors(h3cell))
| mv-expand cells to typeof(string)
| project polygons = geo_h3cell_to_polygon(cells)
| summarize arr = make_list(polygons)
| project geojson = bag_pack("type", "Feature","geometry", bag_pack("type", "GeometryCollection", "geometries", arr), "properties", bag_pack("name", "polygons"))
```

**Output**

|geojson|
|---|
|{"type": "Feature","geometry": {"type": "GeometryCollection","geometries": [<br>  {"type":"Polygon","coordinates":[[[-74.0022744646159,40.735376026215022],[-74.046908029686236,40.727986222489115],[-74.060610712223664,40.696775140349033],[-74.029724408156682,40.672970047595463],[-73.985140983708192,40.680349049267583],[-73.971393761028622,40.71154393543933],[-74.0022744646159,40.735376026215022]]]},<br>  {"type":"Polygon","coordinates":[[[-74.019448383546617,40.790439140236963],[-74.064132193843633,40.783038509825],[-74.077839665342211,40.751803958414136],[-74.046908029686236,40.727986222489115],[-74.0022744646159,40.735376026215022],[-73.988522328408948,40.766594382212254],[-74.019448383546617,40.790439140236963]]]},<br>  {"type":"Polygon","coordinates":[[[-74.077839665342211,40.751803958414136],[-74.1224794808745,40.744383587828388],[-74.1361375042681,40.713156370029125],[-74.1052004095288,40.689365648097258],[-74.060610712223664,40.696775140349033],[-74.046908029686236,40.727986222489115],[-74.077839665342211,40.751803958414136]]]},<br>  {"type":"Polygon","coordinates":[[[-74.060610712223664,40.696775140349033],[-74.1052004095288,40.689365648097258],[-74.118853750491638,40.658161927046628],[-74.0879619670209,40.634383824229609],[-74.043422283844933,40.641782462872115],[-74.029724408156682,40.672970047595463],[-74.060610712223664,40.696775140349033]]]},<br>  {"type":"Polygon","coordinates":[[[-73.985140983708192,40.680349049267583],[-74.029724408156682,40.672970047595463],[-74.043422283844933,40.641782462872115],[-74.012581189358343,40.617990065981623],[-73.968047801220749,40.625358290164748],[-73.954305509472675,40.656529678451555],[-73.985140983708192,40.680349049267583]]]},<br>  {"type":"Polygon","coordinates":[[[-73.926766604813565,40.718903205013063],[-73.971393761028622,40.71154393543933],[-73.985140983708192,40.680349049267583],[-73.954305509472675,40.656529678451555],[-73.909728515658443,40.663878222244435],[-73.895936872069854,40.69505685239637],[-73.926766604813565,40.718903205013063]]]},<br>  {"type":"Polygon","coordinates":[[[-73.943844904976629,40.773964402038523],[-73.988522328408948,40.766594382212254],[-74.0022744646159,40.735376026215022],[-73.971393761028622,40.71154393543933],[-73.926766604813565,40.718903205013063],[-73.912969923470314,40.750105305345329],[-73.943844904976629,40.773964402038523]]]}]},<br>  "properties": {"name": "polygons"}}|

The following example calculates polygon unions that represent H3 cell and its neighbors.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA22PwQrCMAyG73uK3taBgk5QQXyWUmvW1bVNaTtx4sPbbh14MIdA8v/5kmiIpD8I0JpcSX0+tny/O7XdEvWlcl7ZSLIekoF7zycm0AoeqeNiYHOHLoRmQyQgWwpmQcn+hj6salN9iHlu4eW4vRdkRBInB9jRENMmmT3O4wNEJA71JNHmvT/YiKwIdEbkiTAaw716Qz4w2Q0fgGkV0o2F8YdbsKNVaFdkKP+k3HwBry+HcRoBAAA=" target="_blank">Run the query</a>

```kusto
let h3cell = '862a1072fffffff';
print cells = array_concat(pack_array(h3cell), geo_h3cell_neighbors(h3cell))
| mv-expand cells to typeof(string)
| project polygons = geo_h3cell_to_polygon(cells)
| summarize arr = make_list(polygons)
| project polygon = geo_union_polygons_array(arr)
```

**Output**

|polygon|
|---|
|{<br>  "type": "Polygon",<br>  "coordinates": [[[  -73.926766604813565,  40.718903205013063],[  -73.912969923470314,  40.750105305345329],[  -73.943844904976629,  40.773964402038523],[  -73.988522328408948,  40.766594382212254],[  -74.019448383546617,  40.79043914023697],[  -74.064132193843633,  40.783038509825005],[  -74.077839665342211,  40.751803958414136],[  -74.1224794808745,  40.744383587828388],[  -74.1361375042681,  40.713156370029125],[  -74.1052004095288,  40.689365648097251],[  -74.118853750491638,  40.658161927046628],[  -74.0879619670209,  40.6343838242296],[  -74.043422283844933,  40.641782462872115],[  -74.012581189358343,  40.617990065981623],[  -73.968047801220749,  40.625358290164755],[  -73.954305509472675,  40.656529678451555],[  -73.909728515658443,  40.663878222244442],[  -73.895936872069854,  40.695056852396377],[  -73.926766604813565,  40.718903205013063]]]}|

The following example returns true because of the invalid H3 Cell token input.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUcjMK0vMyUxRsFXILM4rzcnRSE/Nj88wTk7NyYnPS81Mz0jKLyrWUE9MSlbX1AQAU7dmMDMAAAA=" target="_blank">Run the query</a>

```kusto
print invalid = isnull(geo_h3cell_neighbors('abc'))
```

**Output**

|invalid|
|---|
|1|
