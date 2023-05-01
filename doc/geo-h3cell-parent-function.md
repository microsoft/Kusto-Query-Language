---
title: geo_h3cell_parent() - Azure Data Explorer
description: Learn how to use the geo_h3cell_parent() function to calculate the H3 cell parent.
ms.reviewer: mbrichko
ms.topic: reference
ms.date: 03/09/2023
---
# geo_h3cell_parent()

Calculates the H3 cell parent.

Read more about [H3 Cell](https://eng.uber.com/h3/).

## Syntax

`geo_h3cell_parent(`*h3cell*`,`*resolution*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *h3cell* | string | &check; | An H3 Cell token value as it was calculated by [geo_point_to_h3cell()](geo-point-to-h3cell-function.md).|
| *resolution* | int | | Defines the requested children cells resolution. Supported values are in the range [0, 14]. If unspecified, an immediate children token will be calculated.|

## Returns

H3 Cell parent token `string`. If the H3 Cell is invalid or parent resolution is higher than given cell, the query will produce an empty result.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShILErNK4lPTs3JUbBVSE/Nj88wBnHiIRIa6hZmRomGBuZGaRCgrgkAkZ+TCTgAAAA=" target="_blank">Run the query</a>

```kusto
print parent_cell = geo_h3cell_parent('862a1072fffffff')
```

**Output**

|parent_cell|
|---|
|852a1073fffffff|

The following example calculates cell parent at level 1.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShILErNK4lPTs3JUbBVSE/Nj88wBnHiIRIa6hZmRomGBuZGaRCgrqNgqAkA+3YzFzsAAAA=" target="_blank">Run the query</a>

```kusto
print parent_cell = geo_h3cell_parent('862a1072fffffff', 1)
```

**Output**

|parent_cell|
|---|
|812a3ffffffffff|

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShILErNK4kvSi1WsFVIT82PzzBOTs3Jic9JLUvN0UASgCjUAAsV5AO1xpfA5DQMdYDQQBMIAJou0OFVAAAA" target="_blank">Run the query</a>

```kusto
print parent_res = geo_h3cell_level(geo_h3cell_parent((geo_point_to_h3cell(1,1,10))))
```

**Output**

|parent_res|
|---|
|9|

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShILErNK4kvSi1WsFVIT82PzzBOTs3Jic9JLUvN0UASgCgEixTkA3XGl8CkNAx1gNBAU0fBWFMTAG9zfXpWAAAA" target="_blank">Run the query</a>

```kusto
print parent_res = geo_h3cell_level(geo_h3cell_parent(geo_point_to_h3cell(1,1,10), 3))
```

**Output**

|parent_res|
|---|
|3|

The following example produces an empty result because of the invalid cell input.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUcjMK0vMyUxRsFXILE7NLSip1EhPzY/PME5OzcmJL0gsSs0r0VA3NDJW19QEAJzHefMxAAAA" target="_blank">Run the query</a>

```kusto
print invalid = isempty(geo_h3cell_parent('123'))
```

**Output**

|invalid|
|---|
|1|

The following example produces an empty result because of the invalid parent resolution.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUcjMK0vMyUxRsFXILE7NLSip1EhPzY/PME5OzcmJL0gsSs0r0VC3MDNKNDQwN0qDAHUdBUMDA01NALIyRtVCAAAA" target="_blank">Run the query</a>

```kusto
print invalid = isempty(geo_h3cell_parent('862a1072fffffff', 100))
```

**Output**

|invalid|
|---|
|1|

The following example produces an empty result because parent can't be of a higher resolution than child.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUcjMK0vMyUxRsFXILE7NLSip1EhPzY/PME5OzcmJL0gsSs0r0VC3MDNKNDQwN0qDAHUdBUNTTU0A3k3StEEAAAA=" target="_blank">Run the query</a>

```kusto
print invalid = isempty(geo_h3cell_parent('862a1072fffffff', 15))
```

**Output**

|invalid|
|---|
|1|
