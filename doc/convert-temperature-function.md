---
title: convert_temperature() - Azure Data Explorer
description: Learn how to use the convert_temperature() function to convert a temperature input value from one unit to another.
ms.reviewer: itsagui
ms.topic: reference
ms.date: 11/23/2022
---
# convert_temperature

Convert a temperature value from one unit to another.

## Syntax

`convert_temperature(`*value*`,`*from*`,`*to*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *value* | real | &check; | The value to be converted. |
| *from* | string | &check; | The unit to convert from. For possible values, see [Conversion units](#conversion-units). |
| *to* | string | &check; | The unit to convert to. For possible values, see [Conversion units](#conversion-units). |

### Conversion units

* DegreeCelsius
* DegreeDelisle
* DegreeFahrenheit
* DegreeNewton
* DegreeRankine
* DegreeReaumur
* DegreeRoemer
* Kelvin
* MillidegreeCelsius
* SolarTemperature

## Returns

 Returns the input value converted from one temperature unit to another.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKVGwVUjOzytLLSqJL0nNLUgtSiwpLUrVMNQz0lFQ907NKcvMUweyXFLTi1JTnVNzijNLi9U1AVVJ6WxCAAAA" target="_blank">Run the query</a>

```kusto
print result = convert_temperature(1.2, 'Kelvin', 'DegreeCelsius')
```

**Output**

|result|
|---|
|-271.95|
