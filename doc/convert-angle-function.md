---
title: convert_angle() - Azure Data Explorer
description: Learn how to use the convert_angle() function to convert an angle input value from one unit to another.
ms.reviewer: itsagui
ms.topic: reference
ms.date: 11/27/2022
---
# convert_angle

Convert an angle value from one unit to another.

## Syntax

`convert_angle(`*value*`,`*from*`,`*to*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| `value` | real | &check; | The value to be converted. |
| `from` | string | &check; | The unit to convert from. For possible values, see [Conversion units](#conversion-units). |
| `to` | string | &check; | The unit to convert to. For possible values, see [Conversion units](#conversion-units). |

### Conversion units

* Arcminute
* Arcsecond
* Centiradian
* Deciradian
* Degree
* Gradian
* Microdegree
* Microradian
* Millidegree
* Milliradian
* Nanodegree
* Nanoradian
* NatoMil
* Radian
* Revolution
* Tilt

## Returns

 Returns the input value converted from one angle unit to another. Invalid units return `null`.

## Example

> [!div class="nextstepaction"]
> <a href=" https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKVGwVUjOzytLLSqJT8xLz0nVMNQz0lFQd0lNL0pNVQeyHIuSczPzSktS1TUBit/6iDgAAAA=" target="_blank">Run the query</a>

```kusto
print result = convert_angle(1.2, 'Degree', 'Arcminute')
```

**Output**

|result|
|---|
|72|
