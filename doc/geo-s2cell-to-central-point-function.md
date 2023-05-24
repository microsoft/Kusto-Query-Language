---
title:  geo_s2cell_to_central_point()
description: Learn how to use the geo_s2cell_to_central_point() function to calculate the geospatial coordinates that represent the center of an S2 cell.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 12/14/2022
---
# geo_s2cell_to_central_point()

Calculates the geospatial coordinates that represent the center of an S2 cell.

Read more about [S2 cell hierarchy](https://s2geometry.io/devguide/s2cell_hierarchy).

## Syntax

`geo_s2cell_to_central_point(`*s2cell*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *s2cell* | string | &check; | S2 cell token value as it was calculated by [geo_point_to_s2cell()](geo-point-to-s2cell-function.md). The S2 cell token maximum string length is 16 characters.|

## Returns

The geospatial coordinate values in [GeoJSON Format](https://tools.ietf.org/html/rfc7946) and of a [dynamic](./scalar-data-types/dynamic.md) data type. If the S2 cell token is invalid, the query will produce a null result.

> [!NOTE]
> The GeoJSON format specifies longitude first and latitude second.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUSjIB5G2Cump+fHFRsmpOTnxJfnxyal5JUWJOfFgWQ0lQyNjE1MzcyVNrhqF1IqS1LwUheT8/KKUzLzEktRioG6wOj0kMYTCnPy89MyS0pRUoDIkBdEGsToKOYkl2KQMYwEypEkCnAAAAA==" target="_blank">Run the query</a>

```kusto
print point = geo_s2cell_to_central_point("1234567")
| extend coordinates = point.coordinates
| extend longitude = coordinates[0], latitude = coordinates[1]
```

**Output**

|point|coordinates|longitude|latitude|
|---|---|---|---|
|{<br>  "type": "Point",<br>  "coordinates": [<br>    9.86830731850408,<br>    27.468392925827604<br>  ]<br>}|[<br>  9.86830731850408,<br>  27.468392925827604<br>]|9.86830731850408|27.4683929258276|

The following example returns a null result because of the invalid S2 cell token input.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUSjIB5G2Cump+fHFRsmpOTnxJfnxyal5JUWJOfFgWQ2lRCVNADb75CkuAAAA" target="_blank">Run the query</a>

```kusto
print point = geo_s2cell_to_central_point("a")
```

**Output**

|point|
|---|
||
