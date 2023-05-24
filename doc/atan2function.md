---
title:  atan2()
description: Learn how to use the atan2() function to calculate an angle in radians between axes.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/03/2022
---
# atan2()

Calculates the angle, in radians, between the positive x-axis and the ray from the origin to the point (y, x).

## Syntax

`atan2(`*y*`,`*x*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *y* | real | &check; | The Y coordinate.|
| *x* | real | &check; | The X coordinate.|

## Returns

Returns the angle in radians between the positive x-axis and the ray from the origin to the point (y, x).

## Examples

The following example returns the angle measurements in radians.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/?query=H4sIAAAAAAAAAysoyswrUUgsScwzijdQsIWwNAx1DDUV9PUVAjIV9BVMFIoSUzIT84oVNExMFVJS04tSU4s1uWoUUitKUvNSoJoN4ZoNdHThuuE6DS0McGo1gmvVNdQxAGvVhVhthDBA1xJhAAA4Vl3utQAAAA==" target="_blank">Run the query</a>

```kusto
print atan2_0 = atan2(1,1) // Pi / 4 radians (45 degrees)
| extend atan2_1 = atan2(0,-1) // Pi radians (180 degrees)
| extend atan2_2 = atan2(-1,0) // - Pi / 2 radians (-90 degrees)
```

**Output**

|atan2_0|atan2_1|atan2_2|
|---|---|---|
|0.785398163397448|3.14159265358979|-1.5707963267949|
