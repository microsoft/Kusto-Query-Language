---
title: convert_temperature() - Azure Data Explorer
description: This article describes convert_temperature() in Azure Data Explorer.
ms.reviewer: itsagui
ms.topic: reference
ms.date: 07/03/2022
---
# convert_temperature

Convert a temperature value from one unit to another.

## Syntax

`convert_temperature(`*value*`,`*from*`,`*to*`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| `value` | real | &check; | The value to be converted. |
| `from` | string | &check; | The unit to convert from. For possible values, see [Conversion units](#conversion-units). |
| `to` | string | &check; | The unit to convert to. For possible values, see [Conversion units](#conversion-units). |

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

## Examples

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKVGwVUjOzytLLSqJL0nNLUgtSiwpLUrVMNQz0lFQ907NKcvMUweyXFLTi1JTnVNzijNLi9U1AVVJ6WxCAAAA)**\]**

```kusto
print result = convert_temperature(1.2, 'Kelvin', 'DegreeCelsius')
```

|result|
|---|
|-271.95|
