---
title: convert_length() - Azure Data Explorer
description: Learn how to use the convert_length() function to convert a length input value from one unit to another.
ms.reviewer: itsagui
ms.topic: reference
ms.date: 11/27/2022
---
# convert_length

Convert a length value from one unit to another.

## Syntax

`convert_length(`*value*`,`*from*`,`*to*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *value* | real | &check; | The value to be converted. |
| *from* | string | &check; | The unit to convert from. For possible values, see [Conversion units](#conversion-units). |
| *to* | string | &check; | The unit to convert to. For possible values, see [Conversion units](#conversion-units). |

### Conversion units

* Angstrom
* AstronomicalUnit
* Centimeter
* Chain
* DataMile
* Decameter
* Decimeter
* DtpPica
* DtpPoint
* Fathom
* Foot
* Hand
* Hectometer
* Inch
* KilolightYear
* Kilometer
* Kiloparsec
* LightYear
* MegalightYear
* Megaparsec
* Meter
* Microinch
* Micrometer
* Mil
* Mile
* Millimeter
* Nanometer
* NauticalMile
* Parsec
* PrinterPica
* PrinterPoint
* Shackle
* SolarRadius
* Twip
* UsSurveyFoot
* Yard

## Returns

 Returns the input value converted from one length unit to another. Invalid units return `null`.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKVGwVUjOzytLLSqJz0nNSy/J0DDUM9JRUPdNLUktUgcy3PLzS9Q1AWLmFfkzAAAA" target="_blank">Run the query</a>

```kusto
print result = convert_length(1.2, 'Meter', 'Foot')
```

**Output**

|result|
|---|
|3.93700787401575|
