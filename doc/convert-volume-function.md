---
title: convert_volume() - Azure Data Explorer
description: This article describes convert_volume() in Azure Data Explorer.
ms.reviewer: itsagui
ms.topic: reference
ms.date: 07/03/2022
---
# convert_volume

Convert a volume value from one unit to another.

## Syntax

`convert_volume(`*value*`,`*from*`,`*to*`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| `value` | real | &check; | The value to be converted. |
| `from` | string | &check; | The unit to convert from. For possible values, see [Conversion units](#conversion-units). |
| `to` | string | &check; | The unit to convert to. For possible values, see [Conversion units](#conversion-units). |

### Conversion units

* AcreFoot
* AuTablespoon
* BoardFoot
* Centiliter
* CubicCentimeter
* CubicDecimeter
* CubicFoot
* CubicHectometer
* CubicInch
* CubicKilometer
* CubicMeter
* CubicMicrometer
* CubicMile
* CubicMillimeter
* CubicYard
* Decaliter
* DecausGallon
* Deciliter
* DeciusGallon
* HectocubicFoot
* HectocubicMeter
* Hectoliter
* HectousGallon
* ImperialBeerBarrel
* ImperialGallon
* ImperialOunce
* ImperialPint
* KilocubicFoot
* KilocubicMeter
* KiloimperialGallon
* Kiloliter
* KilousGallon
* Liter
* MegacubicFoot
* MegaimperialGallon
* Megaliter
* MegausGallon
* MetricCup
* MetricTeaspoon
* Microliter
* Milliliter
* OilBarrel
* UkTablespoon
* UsBeerBarrel
* UsCustomaryCup
* UsGallon
* UsLegalCup
* UsOunce
* UsPint
* UsQuart
* UsTablespoon
* UsTeaspoon

## Returns

Returns the input value converted from one volume unit to another.

## Examples

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKVGwVUjOzytLLSqJL8vPKc1N1TDUM9JRUHcuTcpM9k0tSS1SB/Ick4tS3fLzS9Q1Abo7scQ8AAAA)**\]**

```kusto
print result = convert_volume(1.2, 'CubicMeter', 'AcreFoot')
```

|result|
|---|
|0.0009728568|
