---
title:  convert_energy()
description: Learn how to use the convert_energy() function to convert an energy input value from one unit to another.
ms.reviewer: itsagui
ms.topic: reference
ms.date: 11/27/2022
---
# convert_energy

Convert an energy value from one unit to another.

## Syntax

`convert_energy(`*value*`,`*from*`,`*to*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *value* | real | &check; | The value to be converted. |
| *from* | string | &check; | The unit to convert from. For possible values, see [Conversion units](#conversion-units). |
| *to* | string | &check; | The unit to convert to. For possible values, see [Conversion units](#conversion-units). |

### Conversion units

* BritishThermalUnit
* Calorie
* DecathermEc
* DecathermImperial
* DecathermUs
* ElectronVolt
* Erg
* FootPound
* GigabritishThermalUnit
* GigaelectronVolt
* Gigajoule
* GigawattDay
* GigawattHour
* HorsepowerHour
* Joule
* KilobritishThermalUnit
* Kilocalorie
* KiloelectronVolt
* Kilojoule
* KilowattDay
* KilowattHour
* MegabritishThermalUnit
* Megacalorie
* MegaelectronVolt
* Megajoule
* MegawattDay
* MegawattHour
* Millijoule
* TeraelectronVolt
* TerawattDay
* TerawattHour
* ThermEc
* ThermImperial
* ThermUs
* WattDay
* WattHour

## Returns

 Returns the input value converted from one energy unit to another. Invalid units return `null`.

## Example

> [!div class="nextstepaction"]
> <a href=" https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKVGwVUjOzytLLSqJT81LLUqv1DDUM9JRUPfKL81JVQcynIoySzKLM0IyUotyE3NC8zJL1DUBDSFj0EEAAAA=" target="_blank">Run the query</a>

```kusto
print result = convert_energy(1.2, 'Joule', 'BritishThermalUnit')
```

**Output**

|result|
|---|
|0.00113738054437598|
