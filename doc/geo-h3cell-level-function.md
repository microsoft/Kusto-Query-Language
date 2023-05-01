---
title: geo_h3cell_level() - Azure Data Explorer
description: Learn how to use the geo_h3cell_level() function to calculate the H3 cell resolution.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 03/09/2023
---
# geo_h3cell_level()

Calculates the H3 cell resolution.

Read more about [H3 Cell](https://eng.uber.com/h3/).

## Syntax

`geo_h3cell_level(`*h3cell*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *h3cell* | string | &check; | An H3 Cell token value as it was calculated by [geo_point_to_h3cell()](geo-point-to-h3cell-function.md).|

## Returns

An integer that represents H3 Cell level. Valid level is in range [0, 15]. If the H3 Cell is invalid, the query will produce a null result.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUhOzcmJL0otVrBVSE/Nj88wBgvkpJal5mioW5gZJRoamBulQYC6JgA3an62NAAAAA==" target="_blank">Run the query</a>

```kusto
print cell_res = geo_h3cell_level('862a1072fffffff')
```

**Output**

|cell_res|
|---|
|6|

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUhOzcmJL0otVrBVSE/Nj88wBgvkpJal5miABArygariS2AyGoY6QGigqQkAmqtQej4AAAA=" target="_blank">Run the query</a>

```kusto
print cell_res = geo_h3cell_level(geo_point_to_h3cell(1,1,10))
```

**Output**

|cell_res|
|---|
|10|

The following example returns true because of the invalid H3 Cell token input.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUcjMK0vMyUyJL0otVrBVyCzOK83J0UhPzY/PME5OzcmJz0ktS83RUE9MSlbX1AQAUAf8gDMAAAA=" target="_blank">Run the query</a>

```kusto
print invalid_res = isnull(geo_h3cell_level('abc'))
```

**Output**

|invalid_res|
|---|
|1|
