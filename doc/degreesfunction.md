---
title: degrees() - Azure Data Explorer
description: Learn how to use the degrees() function to convert angle values from radians to values in degrees.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/11/2022
---
# degrees()

Converts angle value in radians into value in degrees, using the formula `degrees = (180 / PI ) * angle_in_radians`.

## Syntax

`degrees(`*radians*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *radians* | real | &check; | The angle in radians to convert to degrees. |

## Returns

The corresponding angle in degrees for an angle specified in radians.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUhJTS9KTS02ULCFMTUKMjU09U00dWAChmhyWoZ6pghZIyRZA00AiS3HB1UAAAA=" target="_blank">Run the query</a>

```kusto
print degrees0 = degrees(pi()/4), degrees1 = degrees(pi()*1.5), degrees2 = degrees(0)
```

**Output**

|degrees0|degrees1|degrees2|
|---|---|---|
|45|270|0|
