---
title: radians() - Azure Data Explorer
description: Learn how to use the radians() function to convert angle values from degrees to radians.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/17/2023
---
# radians()

Converts angle value in degrees into value in radians, using formula `radians = (PI / 180 ) * angle_in_degrees`

## Syntax

`radians(`*degrees*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *degrees* | real | &check; | The angle in degrees.|

## Returns

The corresponding angle in radians for an angle specified in degrees.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKTMlMzCs2ULCFMTUsDTR1YBxDJHFDCyQJIyQJYzMDTQAGCoiHTgAAAA==" target="_blank">Run the query</a>

```kusto
print radians0 = radians(90), radians1 = radians(180), radians2 = radians(360) 
```

**Output**

|radians0|radians1|radians2|
|---|---|---|
|1.5707963267949|3.14159265358979|6.28318530717959|
